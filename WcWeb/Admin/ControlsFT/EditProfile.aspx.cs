using System;

using wctMain.Controller;

namespace wctMain.Admin.ControlsFT
{
    public partial class EditProfile : MainBasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            panelContent.Controls.Add(LoadControl(@"\Admin\ControlsFT\UserProfileMstr.ascx"));
        }
    }
}
