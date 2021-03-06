using System;
using System.Net.Mail;

using Wcss;
using wctMain.Controller;

namespace wctMain.View
{
    public partial class StudentLaminateView : MainBaseControl
   {
       public string context { get; set; }

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
