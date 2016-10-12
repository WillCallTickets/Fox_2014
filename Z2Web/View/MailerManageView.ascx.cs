using System;

using Wcss;
using z2Main.Controller;

namespace z2Main.View
{
    public partial class MailerManageView : ControlBase
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
