using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using wctMain.Controller;

using System.Web.UI;

namespace wctMain.View.Masters
{
    public partial class Master_Account : System.Web.UI.MasterPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Response.Expires = -300;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-45);
            Response.CacheControl = "no-cache";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-45));
            Response.AddHeader("pragma", "no-cache");
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (! IsPostBack)
            {
                MainContext ctx = new MainContext();
            }

            //insert a div to uniquely identify the page in css
            startPageId.Text = string.Format("<div id=\"{0}\">", this.Page.ToString().Replace(".", "_"));            
        }
    }
}