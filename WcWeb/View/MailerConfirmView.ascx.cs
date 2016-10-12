using System;

using Wcss;
using wctMain.Controller;

//test url - sub a new userid
//http://local.fox2014.com/mailerconfirm?pqd=c04a2a38-994f-4658-aabe-a9d473c1b309

namespace wctMain.View
{
    public partial class MailerConfirmView : MainBaseControl
    {
        public string _context { get; set; }

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
            SetContext();
        }

        public void SetContext()
        {
            try
            {
                string qry = this.Request.UrlReferrer.Query;
                string pqd = (qry.IndexOf("?pqd=") != -1) ? qry.Replace("?pqd=", string.Empty) : string.Empty;

                _context = pqd;
            }
            catch(Exception ex)
            {
                _Error.LogException(ex);
            }
        }      
    }
}
