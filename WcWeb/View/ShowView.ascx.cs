using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Wcss;
using wctMain.Controller;

namespace wctMain.View
{
    public partial class ShowView : MainBaseControl
    {
        public string context { get; set; }
        public string uAgent { get; set; }

        protected Show _show;
        private string _venue = string.Empty;
        protected bool isAdmin = false;
        protected bool isAdminPreviewInTab = false;
        protected ShowDate defaultSD = null;

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
            //sets show in admin context
            isAdmin = false;
            isAdminPreviewInTab = false;

            try
            {
                //there is another way to be an admin
                //and it can be specified in the QS - used for previewing shows
                //if we come in this way, the session is checked for actual admin
                string qry = this.Request.UrlReferrer.Query;
                if(qry != null && qry.ToLower() == "?adm=t")
                {
                    //check for logged in user
                    if (_Common.IsAuthdAdminUser())
                        isAdminPreviewInTab = true; 
                }

                isAdmin = this.Request.UrlReferrer.PathAndQuery.IndexOf("/Admin/", StringComparison.OrdinalIgnoreCase) != -1;
            }
            catch (Exception ex) 
            { 
                string request = (this.Request != null) ? 
                    string.Format("request is found for {0}", this.ToString()) : "request not found";
                string referrer = (this.Request != null && this.Request.UrlReferrer != null) ? 
                    string.Format("referrer: {0}", this.Request.UrlReferrer.ToString()) : "referrer not found";
                string pathquery = (this.Request != null && this.Request.UrlReferrer != null && this.Request.UrlReferrer.PathAndQuery != null) ? 
                    string.Format("pathquery: {0}", this.Request.UrlReferrer.PathAndQuery) : "pathquery not found";
                string plainurl = (this.Request != null && this.Request.Url != null) ?
                    string.Format("plainurl: {0}", this.Request.Url.ToString()) : "plainurl not found";

                _Error.LogToFile(string.Format("{0}{3}{1}{3}{2}{3}{4}{3}", request, referrer, pathquery, Environment.NewLine, plainurl), 
                    string.Format("{0}{1}", _Config._ErrorLogTitle, DateTime.Now.ToString("MM_dd_yyyy")));

                _Error.LogException(ex);
            }

            //examine request for admin request


            // allow facebook crawlers to view page
            // https://developers.facebook.com/docs/sharing/webmasters/crawler
            // user agent strings:
            // facebookexternalhit/1.1 (+http://www.facebook.com/externalhit_uatext.php)
            // facebookexternalhit/1.1
            // facebot

            // *Actual log data
            //#Fields: date time 	s-ip 		cs-method 	cs-uri-stem 						cs-uri-query 	s-port 	cs-username 	c-ip 
            //2016-02-25 23:10:33 	74.208.99.108 	GET		/willcallresources/images/shows/thumbmx/shimg16996.jpg 	- 		80 	- 		66.220.146.22 

            //cs(User-Agent) 									cs(Referer) 	sc-status 	sc-substatus 	sc-win32-status time-taken
            //facebookexternalhit/1.1+(+http://www.facebook.com/externalhit_uatext.php) 	- 		200 		0 		0 		312


            bool isFacebookRequest = uAgent.ToLower().IndexOf("facebo") != -1;

            
            if (_Config._LogAgentRequests) {

                _Error.LogToFile(string.Format("{0}\r\nUSER AGENT:\t{1}\r\nCONTEXT(URL):\t{2}\r\nIsFB Session: \t{3}",
                    DateTime.Now.ToString("yyyy/MM/dd hh:mm:sstt"),
                    uAgent,
                    "ShowView.ascx.cs(99): " + context,
                    (isFacebookRequest) ? "FB req" : "not FB"
                    ),
                    string.Format("AgentRequests_{0}", DateTime.Now.ToString("yyyy_MM_dd"))
                    );
            }


            if (isFacebookRequest)
            {
                try
                {
                    _Error.LogToFile(string.Format("{0} - Facebook Crawler\r\nUSER AGENT:\t{1}CONTEXT(URL):\t{2}",
                        DateTime.Now.ToString("yyyy/MM/dd hh:mm:sstt"),
                        uAgent,
                        context
                        ),
                        string.Format("FacebookCrawlerLogs_{0}", DateTime.Now.ToString("yyyy_MM_dd"))
                        );
                }
                catch(Exception ex)
                {
                    _Error.LogException(ex);
                }
            }


            if (!IsPostBack)
            {
                if (isAdminPreviewInTab || isFacebookRequest)
                {
                    _show = Ctx.GetAdminShowByUrl(context);

                }
                else if (isAdmin && Atx.CurrentShowRecord != null)
                    _show = Atx.CurrentShowRecord;
                else
                {
                    //allow an un-announced show - but make sure it is active
                    //this allows facebook to scrape
                    //TODO: don't allow unannounced shows!
                    // 160602 - fuck facebook!
                    // TODO - fix this dern facebook issue asap!!!

                    _show = Ctx.GetCurrentShowByUrl(context, false, true);
                    if (_show != null && (!_show.IsActive))
                        _show = null;
                }

                defaultSD = (_show != null) ? _show.FirstShowDate : null;

                BindControl();

                StringBuilder sb = new StringBuilder();

                if(_show == null)
                {
                    sb.AppendLine();
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("doBackstretch('show');");
                    sb.AppendLine("</script>");
                    litShowMore.Text = sb.ToString();
                }
                if (_show != null)
                {
                    

                    if (isAdmin) {
                        //Registered this with the modal
                        sb.AppendLine();
                        sb.AppendLine("<script type=\"text/javascript\">");
                    }
                    else {                        

                        wctMain.Model.GoogleProp.EventSchema evt = new wctMain.Model.GoogleProp.EventSchema(_show);
                        sb.AppendLine();

                        sb.AppendLine("<script type=\"text/javascript\">");

                        sb.AppendLine("registerShowMore('show');");
                        sb.AppendLine("doBackstretch('show');");
                        sb.AppendLine("registerMailto();");

                        sb.AppendLine();

                        string title = "The Fox Theatre";
                        string url = string.Format("http://{0}", _Config._DomainName);
                        string image = _Config._DefaultShowImage;
                        string description = "The Fox Theatre - Boulder, CO";

                        ShowDate sd = _show.FirstShowDate;
                        //70
                        //title = sd.ShowRecord.Display.DisplaySocial_Description.Trim();
                        title = sd.ShowRecord.Display.DisplaySocial_Description_ForSocialPosts.Trim();

                        url = sd.ConfiguredUrl_withDomain.Replace("www.", string.Empty).Replace("WWW.", string.Empty);

                        image = (_show.ShowImageUrl_Backstretch != null) ? _show.ShowImageUrl_Backstretch.ToLower().Replace("thumbmx", "thumblg") : _Config._DefaultShowImage;
                        if (image.Trim().Length == 0)
                            image = _Config._DefaultShowImage;

                        //200
                        //description = title.Trim();
                        description = sd.ShowRecord.Display.Display_Time_Ages_Tickets;
                        if(description.Length > 200) 
                            description = description.Remove(199).Trim();//200 max

                        //because we do not have a parent page - we need to write the vals to the page and then have JS propagate
                        //sb.AppendFormat("var OG_title='{0}'; ", HttpUtility.HtmlEncode((title.Length > 70) ? title.Remove(68) : title));
                        sb.AppendFormat("var OG_title=\"{0}\"; ", title.Replace("\"","\\\""));
                        sb.AppendLine();
                        sb.AppendFormat("var OG_url=\"{0}\"; ", url);
                        sb.AppendLine();

                        if (image.Length == 0)
                            image = _Config._DefaultShowImage;
                        if (image.Length == 0)
                            _Error.LogToFile(string.Format("hey no image for show {0}", (_show == null) ? "show is null" : _show.Name), "showImageMissingFile");

                        sb.AppendFormat("var OG_image=\"{0}\"; ", image);
                        sb.AppendLine();
                        sb.AppendFormat("var OG_description=\"{0}\"; ", description);
                        sb.AppendLine();
                        sb.AppendLine("registerMeta();");
                                              
                    }

                    sb.AppendLine("</script>");  
                    litShowMore.Text = sb.ToString();
                }
            }            
        }
        
        public void BindControl()
        {   
            Description_fox();
            litShowLinks.DataBind();
        }
        
        private void Description_fox() {

            if (_show != null)
            {
                //VENUE (if appropriate)
                if (_show.VenueRecord.Name.ToLower() != _Config._Default_VenueName.ToLower())
                {
                    _venue = _show.Display.VenueForeign_Markup_Links_Address_LeadIn;
                    litVenue.Text = _venue;
                }

                //PROMOTERS
                string prom = _show.Display.Promoters_Markup_Links;
                if (prom != null && prom.Trim().Length > 0)
                    litPromoter.Text = string.Format("<div class=\"event-promoter\">{0}</div>", prom.Trim());

                //HEADER - status, then showtitle, showheader - showheader is legacy
                if ((_show.ShowAlert != null && _show.ShowAlert.Trim().Length > 0) ||
                    (_show.ShowTitle != null && _show.ShowTitle.Trim().Length > 0) ||
                    (_show.ShowHeader != null && _show.ShowHeader.Trim().Length > 0) 
                    )
                {
                    litHeader.Text = string.Format("<div class=\"event-header\">{0}{1}{2}</div>",
                        (_show.ShowAlert != null && _show.ShowAlert.Trim().Length > 0) ? string.Format("<div class=\"status-alert\">{0}</div>", _show.ShowAlert) : string.Empty,
                        (_show.ShowTitle != null && _show.ShowTitle.Trim().Length > 0) ? string.Format("<div class=\"event-title\">{0}</div>", _show.ShowTitle) : string.Empty,
                        (_show.ShowHeader != null && _show.ShowHeader.Trim().Length > 0) ? string.Format("<div class=\"event-showheader\">{0}</div>", _show.ShowHeader) : string.Empty
                        );
                }

                //EVENT DESCRIPTION - always show
                string allacts = _show.Display.AllActs_Markup_Verbose_Links.Replace("show-acts", "event-acts");

                litEventDescription.Text = 
                    allacts.Replace(" & ", " &amp; ").Trim();

                //DISPLAY NOTES
                if (_show.DisplayNotes != null && _show.DisplayNotes.Trim().Length > 0)
                    litNotes.Text = string.Format("<div class=\"event-notes\">{0}</div>", _show.DisplayNotes.Trim());

                //EVENTINFORMATION - times,pricing,ages, boilerplate
                litInfo.Text = string.Format("<div class=\"event-times\"><span>Doors: {0}</span><span>Show: {1}</span><span>{2}</span></div>{3}",
                    defaultSD.DateOfShow.ToString("hh:mmtt"),
                    (defaultSD.ShowTime != null && defaultSD.ShowTime.Trim().Length > 0) ? (defaultSD.ShowTime) : "tba",
                    defaultSD.AgesString,
                    Environment.NewLine);

                if (defaultSD.PricingText != null && defaultSD.PricingText.Trim().Length > 0)
                    litInfo.Text += string.Format("<div itemscope itemtype=\"http://schema.org/Offer\" class=\"event-pricing\"><span itemprop=\"name\">{0}</span></div>{1}",
                        defaultSD.PricingText.Trim(), 
                        Environment.NewLine);
                

                //only show this for shows in the future
                if (_show.LastDate >= _Config.SHOWOFFSETDATE)
                {
                    //BOILERPLATE and SOCIAL tags                    
                    litAdditionalInfo.Text += string.Format("{0}{1}", defaultSD.Display.TicketBoilerplate, Environment.NewLine);

                    //links for the user to share THIS particular show's content
                    ShowDisplaySocials sosh = new ShowDisplaySocials(_show);
                    litAdditionalInfo.Text += string.Format("<div class=\"event-social\">{0}</div>{1}",
                        sosh.SocialOutput(false, "small", false),
                        Environment.NewLine
                        );                 
                }

                //WRITEUP
                string writeup = _show.ShowWriteup_Derived;

                if (writeup.Length > 0 && writeup.Trim().ToLower() != "<br />" && writeup.Trim().ToLower() != "<br/>")
                    litWriteup.Text = string.Format("<div class=\"writeup-container showmore\">{0}</div>",
                        writeup.Replace("'", "&#39;"));
                else
                    litWriteup.Text = string.Empty;
            }
        }

        protected void litShowLinks_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //show links
            if (_show != null && _show.ShowLinkRecords().Count > 0)
            {
                ShowLinkCollection internalColl = new ShowLinkCollection();
                ShowLinkCollection externalColl = _show.ShowLinkRecords().RemoteLinks_Active;

                //history shows should not link
                if (_show.LastDate >= _Config.SHOWOFFSETDATE)
                    internalColl.AddRange(_show.ShowLinkRecords().GetList().FindAll(delegate(ShowLink match)
                    {
                        if (match.IsActive && match.IsShowLink)
                        {
                            Show linkedShow = null;

                            try
                            {
                                linkedShow = Ctx.GetCurrentShowById(int.Parse(match.LinkUrl), false);

                                return linkedShow != null && linkedShow.IsActive && linkedShow.AnnounceDate < _Config.SHOWOFFSETDATE && linkedShow.LastDate > _Config.SHOWOFFSETDATE;
                            }
                            catch (Exception) { }
                        }

                        return false;
                    }));

                if (internalColl.Count > 1)
                    internalColl.Sort("IDisplayOrder", true);

                if (internalColl.Count > 0 || externalColl.Count > 0)
                {
                    sb.AppendFormat("<div class=\"showlink-container\">");

                    if (internalColl.Count > 0 && _Config._ShowLinks_Header != null && _Config._ShowLinks_Header.Length > 0)
                        sb.AppendFormat("<div class=\"showlinkheader\">{0}</div>", _Config._ShowLinks_Header);

                    foreach (ShowLink sl in internalColl)
                        sb.AppendFormat("<span class=\"showlinkinternal\">{0}</span>", sl.LinkUrl_Formatted(false));

                    foreach (ShowLink sl in externalColl)
                        sb.AppendFormat("<span class=\"showlinkexternal\">{0}</span>", sl.LinkUrl_Formatted(true));

                    sb.AppendFormat("</div>");
                }
            }

            lit.Text = sb.ToString();
        }        
    }
}//442
