using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace wctMain
{
    public partial class FB_test : wctMain.Controller.MainBasePage
    {
        #region Properties

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

        #endregion

        #region Page Loop and Main Logic

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            QualifySsl(false);
        }

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
            HtmlGenericControl divAgent = (HtmlGenericControl)FindControl("divAgent");
            if (divAgent != null) {


                string userAgent = this.Request.UserAgent;
                divAgent.InnerText = userAgent;

                string msg1 = string.Format("{0}: {1} by {2}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ssstt"), "Test Page Hit", userAgent);
                Wcss._Error.LogToFile(msg1, string.Format("Agent_Log_{0}", DateTime.Now.ToString("MM_dd_yyyy")));
            

                 HtmlGenericControl divFacebook = (HtmlGenericControl)FindControl("divFacebook");
                 if (divFacebook != null)
                 {
                     bool agentIsFB = userAgent.ToLower().IndexOf("facebo") != -1;
                     divFacebook.Visible = agentIsFB;

                     if (divFacebook.Visible)
                     {
                         divFacebook.InnerText = userAgent;

                         string msg2 = string.Format("{0}: {1} by {2}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ssstt"), "FaceBook Hit", userAgent);
                         Wcss._Error.LogToFile(msg2, string.Format("FB_Log_{0}", DateTime.Now.ToString("MM_dd_yyyy")));
                     }
                 }
            
            }
        }

        #endregion
    }
}