using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web.Script.Serialization;

using Wcss;
using wctMain.Model;

namespace wctMain.Admin
{
    public partial class ShowDisplayer : wctMain.Controller.MainBasePage
    {
        #region Page Loop and Main Logic

        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
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
            pnlShowListingContent.Controls.Add(LoadControl(string.Format(@"~\View\ShowView.ascx")));
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            base.Redirect(this.Request.RawUrl);
        }

        #endregion
    }
}
