using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin._shows._controls
{
    public partial class ShowSelector : MainBaseControl, IPostBackEventHandler
    {
        #region PostBack

        public void RaisePostBackEvent(string eventArgument)
        {
            Dictionary<string, string> args = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventArgument);
            string commandName = args["commandName"].ToLower();

            switch (commandName)
            {
                case "selectedidfromtypeahead":

                    Atx.SetCurrentShowRecord(int.Parse(args["newIdx"]));
                    Atx.CurrentShowListStartDate = Atx.CurrentShowRecord.FirstDate;

                    if (Atx.CurrentEditPrincipal != _Enums.Principal.all && Atx.CurrentShowRecord.VcPrincipal.IndexOf(Atx.CurrentEditPrincipal.ToString()) == -1)
                        Atx.CurrentEditPrincipal = _Enums.Principal.all;

                    AdminEvent.OnShowChosen(this, Atx.CurrentShowRecord.Id);

                    break;
            }
        }

        #endregion

        protected string activeClass = "btn btn-warning";
        protected string inactiveClass = "btn btn-primary";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            wctMain.Admin.AdminEvent.ShowNameChanged += new AdminEvent.ShowNameChangeEvent(AdminEvent_ShowNameChanged);

            search_show.Attributes.Add("data-searchtype", "Show");
            search_show.Attributes.Add("placeholder", "Search");
        }

        public override void Dispose()
        {
            wctMain.Admin.AdminEvent.ShowNameChanged -= new AdminEvent.ShowNameChangeEvent(AdminEvent_ShowNameChanged);
            base.Dispose();
        }

        protected void AdminEvent_ShowNameChanged(object sender, EventArgs e)
        {
            ddlShow.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                userIsDataViewer = this.Page.User.IsInRole("VenueDataViewer");

                txtShowListStartDate.DataBind();
                rptContext.DataBind();
                rptPrincipal.DataBind();
            }
        }

        protected void txtShowListStartDate_TextChanged(object sender, EventArgs e)
        {
            DateTime validatedInput = DateTime.MinValue;

            if (DateTime.TryParse(txtShowListStartDate.Text, out validatedInput))
            {
                Atx.CurrentShowListStartDate = validatedInput;
                ddlShow.DataBind();
            }
        }

        protected void ddlShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //set current show in context - and id
            int idx = int.Parse(ddl.SelectedValue);

            AdminEvent.OnShowChosen(this, idx);
        }
        
        protected void ddlShow_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            ddl.SelectedIndex = -1;

            //ensure a selection item
            if(ddl.Items.Count == 0 || ddl.Items[0].Value != "0")
                ddl.Items.Insert(0, new ListItem("<-- SELECT A SHOW -->", "0"));

            //match current show to selection
            int idx = (Atx.CurrentShowRecord != null) ? Atx.CurrentShowRecord.Id : 0;

            ListItem li = ddl.Items.FindByValue(idx.ToString());
            if (li != null)
                li.Selected = true;
            else
                ddl.SelectedIndex = 0;//select default item
        }

        protected void SqlShowList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            // take principal into account
            // set default venue from principal
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("DECLARE  @DefaultVenue varchar(256) ");
            sb.Append("SET		@DefaultVenue = ");
            sb.Append("         CASE ");
            sb.Append("             WHEN @Principal = 'fox' THEN 'The Fox Theatre' ");
            sb.Append("             WHEN @Principal = 'bt' THEN 'The Boulder Theater' ");
            sb.Append("             ELSE '' ");
            sb.Append("         END ");
            sb.Append("SELECT   TOP 200 s.[Id],  ");
            sb.Append("         (s.[Name] ");

            //keep this here in case we ever want to add the city, state to the listing
            //sb.Append("         + ' ' + ISNULL(v.[City],'') + ");
            //add punctuation if necessary
            //sb.Append("         CASE WHEN v.[City] IS NOT NULL AND LEN(LTRIM(RTRIM(v.[City]))) > 0 AND v.[State] IS NOT NULL AND LEN(LTRIM(RTRIM(v.[State]))) > 0 THEN ', ' ELSE '' END + ");
            //sb.Append("         ISNULL(v.[State],'') ");
            //end city/state

            sb.Append("         ) as ShowName, ");
            //MaxDate determines if showdate is within our range - not for sorting
            sb.Append("         MAX(sd.[dtDateOfShow]) as MaxDate, ");
            sb.Append("         SUBSTRING(s.[Name], 0, CHARINDEX(' - ', s.[Name])) AS [DateRank], ");
            sb.Append("         CASE WHEN v.[Name] = @DefaultVenue THEN '' ELSE v.[Name] END AS [VenueRank]	");
            sb.Append("FROM [Show] s FULL OUTER JOIN [ShowDate] sd ON sd.[tShowId] = s.[Id] LEFT OUTER JOIN Venue v ON s.[TVenueId] = v.[Id] ");
            sb.Append("WHERE (CASE WHEN @Principal = 'all' THEN 1 WHEN CHARINDEX(@Principal, s.[vcPrincipal]) >= 1 THEN 1 ELSE 0 END = 1) ");
            sb.Append("GROUP BY s.[Id], s.[Name], v.[City], v.[State], v.[Name] HAVING MAX(sd.[dtDateOfShow]) >= @startDate ");
            sb.Append("ORDER BY [DateRank], [VenueRank] ");

            e.Command.CommandText = sb.ToString();

            e.Command.Parameters["@startDate"].Value = Atx.CurrentShowListStartDate;
            e.Command.Parameters["@Principal"].Value = Atx.CurrentEditPrincipal.ToString();
        }


        #region btnGroup Principal

        protected void rptPrincipal_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            rpt.DataSource = Enum.GetNames(typeof(_Enums.Principal)).ToList<string>();
        }

        protected void rptPrincipal_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Repeater rpt = (Repeater)source;

            if (e.CommandName.ToLower() == "select")
            {
                string s = e.CommandArgument.ToString();

                Atx.CurrentEditPrincipal = (_Enums.Principal)Enum.Parse(typeof(_Enums.Principal), s, true);
                ddlShow.DataBind();
                rpt.DataBind();
            }
        }

        #endregion

        #region btnGroup Context

        protected bool userIsDataViewer = false;

        protected void rptContext_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            //bind to enums for show edit context
            List<_Enums.CookEnums.ShowEditContext> list = new List<_Enums.CookEnums.ShowEditContext>();
            list.AddRange(_Enums.EnumToList<_Enums.CookEnums.ShowEditContext>());
            //list.Remove(_Enums.CookEnums.ShowEditContext.Data);
            rpt.DataSource = list;
        }

        protected void rptContext_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            LinkButton btn = (LinkButton)e.Item.FindControl("btnContext");

            string edQ = GetEditContextFromQS();

            btn.Text = (e.Item.DataItem.ToString() == "Selection") ? "New" : e.Item.DataItem.ToString();
            btn.CommandName = e.Item.DataItem.ToString();            
            btn.CssClass = (edQ.ToLower() == btn.CommandName.ToLower()) ? activeClass : inactiveClass;

            //enable in context
            if (e.Item.DataItem.ToString() == "Selection")
                btn.Enabled = (edQ.ToLower() != _Enums.CookEnums.ShowEditContext.Selection.ToString().ToLower());
            else if(e.Item.DataItem.ToString() == "Data")
                btn.Enabled = (userIsDataViewer && Atx.CurrentShowRecord != null);
            else
                btn.Enabled = (Atx.CurrentShowRecord != null);
        }

        /// <summary>
        /// 
        /// if we are selecting a new context - nav to the new context
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptContext_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Repeater rpt = (Repeater)source;

            base.Redirect(string.Format("/Admin/_shows/ShowDirector.aspx?p={0}", e.CommandName));
        }

        private string GetEditContextFromQS()
        {
            string controlToLoad = "selection";

            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            return controlToLoad;
        }

        #endregion
}
}