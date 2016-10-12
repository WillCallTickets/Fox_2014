using System;
using System.Web;
using System.Web.UI.WebControls;

using Wcss;

namespace wctMain.View.Masters
{
    public partial class TemplateBlankMaster : System.Web.UI.MasterPage
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
        }
    }
}