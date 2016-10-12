using System;
using System.Web;
using System.Web.UI;

using Wcss;

namespace wctMain.Masters
{
    public partial class Master_Kiosk : System.Web.UI.MasterPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(this.Page_Load);

            Response.Expires = -300;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-45);
            Response.CacheControl = "no-cache";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-45));
            Response.AddHeader("pragma", "no-cache");

            //<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
            //<meta http-equiv="X-UA-Compatible" content="IE=edge;chrome=1">
            //http://www.validatethis.co.uk/news/fix-bad-value-x-ua-compatible-once-and-for-all/
            if (Request.ServerVariables["HTTP_USER_AGENT"] != null &&
                Request.ServerVariables["HTTP_USER_AGENT"].ToString().IndexOf("MSIE") > -1)
                Response.AddHeader("X-UA-Compatible", "IE=edge;chrome=1");
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
