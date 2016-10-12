using System;

using Wcss;
using z2Main.Controller;

//test url - sub a new userid
//http://local.fox2014.com/mailerconfirm?pqd=c04a2a38-994f-4658-aabe-a9d473c1b309

namespace z2Main.View
{
    public partial class MailerConfirmView : ControlBase
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
                string qry = this.Request.UrlReferrer.Query.TrimStart(new char[] {'?'});
                string[] pieces = qry.Split(new char[] { '&' });

                string eml = null;
                string pqd = null;

                if (pieces.Length == 2)
                {
                    foreach (string s in pieces)
                    {
                        string[] parts = s.Split(new char[] { '=' });
                        if (parts.Length == 2 && parts[0] == "e")
                            eml = parts[1];
                        if (parts.Length == 2 && parts[0] == "pqd")
                            pqd = parts[1];
                    }
                }

                //pqd does nothing - TODO make it do something
                if (pqd == null || pqd.Trim().Length == 0)
                    throw new ArgumentOutOfRangeException("pqd not found on mail confirmation");

                if (eml == null || eml.Trim().Length == 0)
                    throw new ArgumentOutOfRangeException("eml not found on mail confirmation");

                _context = eml;
            }
            catch(Exception ex)
            {
                _Error.LogException(ex);
            }
        }      
    }
}
