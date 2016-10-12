using System;

using wctMain.Controller;

namespace wctMain.Admin
{
    public partial class _Default : MainBasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.User.IsInRole("Administrator") || this.User.IsInRole("ContentEditor") || this.User.IsInRole("Manifester") ||
                this.User.IsInRole("OrderFiller") || this.User.IsInRole("Super") || this.User.IsInRole("ReportViewer"))
            { }//base.Redirect("/Admin/ShowEditor.aspx");
            else if (this.User.IsInRole("MassMailer"))
            { }//base.Redirect("/Admin/Mailers.aspx");
            else
                base.Redirect("/");
        }
    }
}