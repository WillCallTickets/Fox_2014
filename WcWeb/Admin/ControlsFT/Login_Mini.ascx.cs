using System;
using System.Web.UI.WebControls;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin.ControlsFT
{
    public partial class Login_Mini : MainBaseControl
    {
        protected void Page_Load(object sender, EventArgs e) {}

        protected void linkLogout_Click(object sender, EventArgs e)
        {
            //if we are on a secure page - redirect to home page after logout
            Ctx.LogoutUser();

            //Server.Transfer does not work if a page is ssl and going to non-ssl, etc 081106
            
            base.Redirect("/");
        }

        protected void linkRegister_Click(object sender, EventArgs e)
        {
            //redirect to register page - be sure to include current url for redirecturl
            base.Redirect(string.Format("/Admin/Register.aspx?ReturnUrl={0}", System.Web.HttpUtility.UrlEncode(this.Request.RawUrl)));
        }
    }
}