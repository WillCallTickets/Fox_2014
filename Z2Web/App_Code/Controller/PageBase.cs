using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

using Wcss;

namespace z2Main.Controller
{
    public partial class PageBase : System.Web.UI.Page
    {
        public PageBase()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private Z2Context _ztx;
        public Z2Context Ztx
        {
            get
            {
                if (_ztx == null)
                    _ztx = new Z2Context();

                return _ztx;
            }
            set
            {
                _ztx = value;
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            //set marketing program key
            string mpk = Request.QueryString["mp"];
            if (mpk != null)
                Ztx.MarketingProgramKey = mpk;

            //we are going to use our own theme links - not pre-genned
            this.Theme = string.Empty;

            base.OnPreInit(e);
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
        
        protected override void OnError(EventArgs e)
        {
            base.OnError(e);

            Exception objError = Server.GetLastError().GetBaseException();

            if (objError != null)
            {
                Wcss._Error.LogException(objError);

                string err = string.Format("Error Caught in Application_Error event\nError in: {0}\nError Message: {1}\nStack Trace: {2}",
                    Request.Url.ToString(), objError.Message.ToString(), (objError.StackTrace != null) ? objError.StackTrace.ToString() : string.Empty);

                Ztx.CurrentPageException = err;

                this.Redirect("/Error.aspx");
            }
        }

        public void Redirect(string url)
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

    }
}