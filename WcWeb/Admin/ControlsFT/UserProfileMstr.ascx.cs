using System;

using wctMain.Controller;

namespace wctMain.Admin.ControlsFT
{
    public partial class UserProfileMstr : MainBaseControl
    {
        protected void Page_Load(object sender, EventArgs e) {}

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UserProfile1.SaveProfile();
            lblFeedbackOK.Visible = true;
        }
    }
}
