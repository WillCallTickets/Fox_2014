using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;
using wctMain.Model;

namespace wctMain
{
    public partial class MobileMailManager : wctMain.Controller.MainBasePage
    {
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string control = string.Empty;
            string req = Request.QueryString["p"];
            if (req != null && req.Trim().Length > 0)
                control = req;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //SetPageControl();
        }

        protected string defaultControlCode = "MailerManage";

        protected string GetQueryP()
        {
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                return req;

            return defaultControlCode;
        }

        protected virtual void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string controlToLoad = GetQueryP();

            switch (controlToLoad.ToLower())
            {
                case "mailermanage":
                    controlToLoad = "MailerManageView";
                    break;
                case "subscribe":
                    controlToLoad = "SubscribeView";
                    break;
                case "unsubscribe":
                    controlToLoad = "UnsubscribeView";
                    break;
            }

            //pnlContent.Controls.Add(LoadControl(string.Format(@"/View/{0}.ascx", controlToLoad)));
        }

        #endregion

    }
}