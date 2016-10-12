using System;
using System.Net.Mail;

using Wcss;
//using wctMain.Controller;

namespace z2Main.View
{
    public partial class MailerExampleView : System.Web.UI.UserControl
   {
       public string url { get; set; }
       protected bool _isPopup 
       { 
           get 
        {
            return true;
        } 
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
       }
   }
}
