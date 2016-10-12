using System;
using System.Web;

using Wcss;

namespace wctMain.Controller
{
    public partial class MainBasePage : System.Web.UI.Page
	{
        protected override void OnError(EventArgs e)
        {
            base.OnError(e);

            Exception objError = Server.GetLastError().GetBaseException();

            if (objError != null)
            {
                Wcss._Error.LogException(objError);

                string err = string.Format("Error Caught in Application_Error event\nError in: {0}\nError Message: {1}\nStack Trace: {2}",
                    Request.Url.ToString(), objError.Message.ToString(), (objError.StackTrace != null) ? objError.StackTrace.ToString() : string.Empty);

                Ctx.CurrentPageException = err;

                this.Redirect("/Error.aspx");
            }
        }

		public void Redirect( string url)
		{
            try
            {
                Response.Redirect(url, true);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogToFile(string.Format("Page causing redirect error: {0}", url),
                    string.Format("{0}{1}", _Config._ErrorLogTitle, DateTime.Now.ToString("MM_dd_yyyy")));
                _Error.LogException(ex);
            }
		}

        public void QualifySsl(bool sslRequired)
        {
            if (sslRequired && !Request.IsSecureConnection)
            {
                try
                {
                    string uri = Request.Url.AbsoluteUri.Replace("http://", "https://");

                    this.Redirect(uri);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
            else if ((!sslRequired) && Request.IsSecureConnection)
            {
                try
                {
                    string uri = Request.Url.AbsoluteUri.Replace("https://", "http://");

                    this.Redirect(uri);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        public bool IsPageInAdminContext
        {
            get
            {
                return this.MasterPageFile != null &&
                    (this.MasterPageFile.IndexOf("TemplateAdmin") != -1 || this.MasterPageFile.IndexOf("TemplateBlank") != -1 ||
                    this.MasterPageFile.IndexOf("TemplateVenuData") != -1);
            }
        }
        
        private string _rawParsed = null;
        public string RawParsed
        {
            get
            {
                if (_rawParsed == null)
                    _rawParsed = this.Request.RawUrl.Trim(new char[] { '/', '#', '!' }).Replace(".aspx", string.Empty).Trim().ToLower();

                return _rawParsed;
            }
        }

        public string UserAgentInformation
        {
            get
            {
                if (Request != null)
                    return string.Format("{0}: User: {1} Url: {2} IP: {3} Agent: {4} Platform: {5} Browser: {6} {7} {8}.{9}",
                        DateTime.Now.ToString(),
                        (this.User != null) ? User.Identity.Name : "user unknown",
                        Request.Url, Request.UserHostAddress, Request.UserAgent, Request.Browser.Platform,
                        Request.Browser.Browser, Request.Browser.Version, Request.Browser.MajorVersion,
                        Request.Browser.MinorVersion);
                else
                    return string.Format("{0}: User: {1} Request Object Unknown",
                        DateTime.Now.ToString(), (this.User != null) ? User.Identity.Name : "user unknown");
            }
        }

        #region Legacy

        public static bool DisplayRightContent(System.Web.UI.Page page)
        {
            return false;
        }
        
        /// <summary>
        /// tells us where to redirect to after an old user is update
        /// </summary>
        public string RedirectOnAuth
        {
            get
            {
                return (string)Session["RedirectOnAuth"];
            }
            set
            {
                if (value == null)
                    Session.Remove("RedirectOnAuth");
                else
                    Session["RedirectOnAuth"] = value;
            }
        }

        #endregion

        private wctMain.Admin.AdminContext _atx;
        public wctMain.Admin.AdminContext Atx
        {
            get
            {
                if (_Common.IsAuthdAdminUser())
                {
                    if (_atx == null)
                        _atx = new wctMain.Admin.AdminContext();

                    return _atx;
                }

                return null;
            }
            set
            {
                if (_Common.IsAuthdAdminUser())
                    _atx = value;
            }
        }

		private	MainContext _ctx;
        public MainContext Ctx
		{
			get
			{
				if(_ctx == null)
                    _ctx = new MainContext();					
				
				return _ctx;
			}
			set
			{
				_ctx = value;
			}
		}

        protected override void OnInit(EventArgs e)
        {
            if (this.ToString().ToLower().IndexOf("asp.rss_aspx") != -1)
                return;

            base.OnInit(e);
        }

        protected override void OnPreInit(EventArgs e)
        {
            //dont disrupt a transaction in progress
            string page = this.ToString().ToLower();

            if (_Config._MaintenanceMode_On && 
                page.IndexOf("kiosk_aspx") == -1 &&
                page.IndexOf("register_aspx") == -1 &&
                page.IndexOf("controls_jpegimage_aspx") == -1 &&
                (!HttpContext.Current.User.IsInRole("Administrator")))
            {
                Redirect("/Maintenance.aspx");
            }

            //set marketing program key
            string mpk = Request.QueryString["mp"];
            if (mpk != null)
                Ctx.MarketingProgramKey = mpk;

            //we are going to use our own theme links - not pregenned
            this.Theme = string.Empty;// "Events";

            base.OnPreInit(e);
        }

        public void FlashError()
        {
            if (Ctx.CurrentPageException != null)
            {
                Ctx.DisplayErrorText(Ctx.CurrentPageException);
                Ctx.CurrentPageException = null;
            }
        }
    }
}
