using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlTypes;

using Wcss;
using wctMain.Controller;

namespace wctMain.View
{
    public partial class SearchView : MainBaseControl
    {
        public string terms { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        public override void Dispose()
        {
            this.Load -= new System.EventHandler(this.Page_Load);
            base.Dispose();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindControls();
        }

        public void BindControls()
        {
            rptList.DataBind();
        }

        protected void rptList_DataBinding(object sender, EventArgs e) 
        {
            Repeater rpt = (Repeater)sender;

            if (terms.Trim().Length > 2)
            {
                StringBuilder sb = new StringBuilder();
                
                //TODO: make this value work in context
                int timeBetweenSearches = 0;
                DateTime dateSearch = DateTime.MinValue;
                string searchContext = "terms";
                string searchCriteria = System.Web.HttpUtility.UrlDecode(terms).TrimStart('?');

                string showAvailability = "AND (CASE WHEN @principal = 'all' THEN 1 ELSE CASE WHEN (CHARINDEX(@principal, s.[vcPrincipal]) >= 1) THEN 1 ELSE 0 END END = 1) AND s.[bActive] = 1 AND ISNULL(s.[dtAnnounceDate], DATEADD(dd, -1, @now)) < @now ";

                sb.Append("SELECT DISTINCT s.[Id] as 'Id', s.[Name], sd.[dtDateOfShow] as 'ShowDate' ");
                sb.Append("FROM [Show] s LEFT OUTER JOIN [ShowDate] sd ON sd.[TShowId] = s.[Id] ");

                //determine if date or string
                if (DateTime.TryParse(searchCriteria, out dateSearch))
                {
                    int year = 0, month = 0, day = 0;
                    searchContext = "date";                    

                    if (searchCriteria.ToLower().IndexOf("am") != -1 || searchCriteria.ToLower().IndexOf("pm") != -1)
                    {
                        //if we are looking for a date with time - then search on full date with time - no need to parse it out
                        sb.AppendFormat("WHERE sd.[dtDateOfShow] = @criteriaDate ");
                    }
                    else
                    {
                        year = dateSearch.Year;
                        month = dateSearch.Month;
                        day = dateSearch.Day;

                        sb.AppendFormat("WHERE DATEPART(yyyy, sd.[dtDateOfShow]) = {0} AND DATEPART(mm, sd.[dtDateOfShow]) = {1} AND DATEPART(dd, sd.[dtDateOfShow]) = {2} ", 
                            year, month, day);                        
                    }

                    sb.AppendFormat(" {0} ", showAvailability);
                }
                else
                {
                    //this should be done on client - not sql server - but for future reference                    
                    sb.Append("LEFT OUTER JOIN [JShowAct] jsa ON jsa.[TShowDateId] = sd.[Id] LEFT OUTER JOIN [Act] a ON a.[Id] = jsa.[TActId] ");
                    sb.AppendFormat("WHERE (s.[Name] LIKE '%' + @criteria + '%' OR a.[Name] LIKE '%' + @criteria + '%') {0} ", showAvailability);
                    sb.Append("ORDER BY s.[Name] DESC ");
                }



                if (searchCriteria.Trim().Length > 0)
                {
                    //this section built in reverse///
                    sb.Insert(0, "SELECT -1 as 'Id', '' as 'Name', '' as 'ShowDate';  RETURN END ");                    
                    sb.Insert(0,
                        string.Format("IF EXISTS (SELECT * FROM [Search] WHERE (CHARINDEX(@principal, [VcPrincipal]) >= 1) AND [vcContext] = @context AND [Terms] = @terms AND [IpAddress] = @ip AND (DATEADD(ss, {0}, [dtStamp]) > (getDate()))) BEGIN ",
                        timeBetweenSearches.ToString()));
                    sb.Insert(0, "DECLARE @results int ");
                    //////////////////////////////////

                    sb.Append("SELECT @results = @@ROWCOUNT ");
                    sb.Append("INSERT [Search] ([vcPrincipal], [vcContext], [Terms], [iResults], [EmailAddress], [IpAddress]) ");
                    sb.Append("VALUES (@principal, @context, @terms, @results, @email, @ip); ");

                    //determine principal from host


                    List<Triplet> coll = new List<Triplet>();
                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@now", DateTime.Now, DbType.DateTime);
                    cmd.Parameters.Add("@criteria", searchCriteria, DbType.String);
                    cmd.Parameters.Add("@criteriaDate", (dateSearch != DateTime.MinValue) ? dateSearch : SqlDateTime.Null, DbType.DateTime);

                    string prince = _PrincipalBase.PrincipalFromUrlHost(this.Request).ToString();
                    cmd.Parameters.Add("@principal", prince, DbType.String);

                    cmd.Parameters.Add("@context", searchContext, DbType.String);
                    cmd.Parameters.Add("@terms", searchCriteria, DbType.String);
                    cmd.Parameters.Add("@email", string.Empty, DbType.String);
                    cmd.Parameters.Add("@ip", (this.Request != null && this.Request.UserHostAddress != null) ? this.Request.UserHostAddress : string.Empty, DbType.String);

                    try
                    {
                        using (System.Data.IDataReader dr = SubSonic.DataService.GetReader(cmd))
                        {
                            while (dr.Read())
                            {
                                Triplet p = new Triplet((int)dr.GetValue(dr.GetOrdinal("Id")),
                                    dr.GetValue(dr.GetOrdinal("Name")).ToString(),
                                    dr.GetValue(dr.GetOrdinal("ShowDate")).ToString());

                                if (p.First.ToString() == "-1")
                                {
                                    litTitle.Text = string.Format("Please allow {0} seconds between similar searches.",
                                        timeBetweenSearches.ToString());
                                    return;
                                }

                                coll.Add(p);
                            }

                            dr.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        litTitle.Text = string.Format("Sorry, there was an error with your search");

                        rptList.Visible = false;
                    }
                                        
                    litTitle.Text = string.Format("{0} result{1} for <span class=\"searchterm\">&#34;{2}&#34;</span>",
                        coll.Count.ToString(),
                        (coll.Count > 1) ? "s" : string.Empty,
                        System.Web.HttpUtility.HtmlEncode(searchCriteria));
                    
                    rptList.Visible = true;
                    rptList.DataSource = coll;
                }
                else
                {
                    litTitle.Text = string.Format("Sorry, there are no results for your search");
                    rptList.Visible = false;
                }
            }
        }

        //set up the links for the found items
        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            Literal lit = (Literal)e.Item.FindControl("litItem");

            if (lit != null && e.Item.DataItem != null)
            {
                Triplet p = (Triplet)e.Item.DataItem;

                //first is the show id - not needed here
                //second is show name - parse out the event part
                string showname = p.Second.ToString();
                int secondDash = showname.LastIndexOf(" - ");
                string eventname = showname.Substring(secondDash).TrimStart(new char[] { ' ', '-' });

                //third is the showdate
                DateTime showDate = DateTime.Parse(p.Third.ToString());


                //create the url - from the parts of the params
                //we are only simulating the ShowDate.FriendlySlashedUrl here - not using ShowDate object
                string dashedurl = string.Format("{0}-{1}",
                     showDate.ToString("yyyy-MM-dd-hhmmtt"),
                     Utils.ParseHelper.FriendlyFormat(eventname.ToLower())
                     );

                //Create the text for the link
                string link = System.Web.HttpUtility.HtmlEncode(string.Format("{0} {1}",
                    showDate.ToString("MM/dd/yyyy hh:mmtt"),
                    eventname
                    ));


                //create the link
                lit.Text = string.Format("<a href=\"/{0}\">{1}</a>",
                    dashedurl,
                    link);
            }
        }
    }
}
