using System;
using System.Web.Security;
using System.Web.UI.WebControls;

using System.Net.Mail;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin
{
    public partial class Register : MainBasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            panelContent.Controls.Add(LoadControl(@"\Admin\ControlsFT\Register.ascx")); 
        }
    }
}
