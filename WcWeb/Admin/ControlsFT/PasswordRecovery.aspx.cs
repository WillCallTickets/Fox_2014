using System;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin.ControlsFT
{
    public partial class PasswordRecovery : MainBasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            panelContent.Controls.Add(LoadControl(string.Format(@"\Admin\ControlsFT\PasswordRecovery.ascx")));
        }
    }
}
