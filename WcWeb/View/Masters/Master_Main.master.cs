using System;
using System.Web;
using System.Web.UI;

using Wcss;

namespace wctMain.Masters
{
    // in the head tag prefix="og: http://ogp.me/ns#"
    /*
     *     <meta property="twitter:card" content="summary">
    <meta property='twitter:domain' content='foxtheatre.com/' />
    <meta id="twitter_image" property='twitter:image:src' content='http:/foxtheatre.com/WillCallResources/Images/bgs/balcrowd.jpg' />
    <meta id="twitter_title" property='twitter:title' content='Home page' />
    <meta id="twitter_url" property='twitter:url' content='http://www.foxtheatre.com/' />
    <meta id="twitter_description" property='twitter:description' content="Located on The Hill, The Fox Theatre is the premier 
        live music club in Boulder, Colorado.  Recently voted 4th best music venue in the country by Rolling Stone Magazine, 
        the Fox offers top-notch talent, a world-class sound system, and an intimate 625 capacity atmosphere.  Check out our 
        event lineup and join us for a memorable experience." />
     */

    public partial class Master_Main : System.Web.UI.MasterPage
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

        protected wctMain.Controller.MainContext _ctx = new Controller.MainContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.GetCurrent(Page).AuthenticationService.Path = "/Authentication_JSON_AppService.axd";

            if (litGglVenue.Text == string.Empty)
            {
                if (_ctx.DefaultVenue != null)
                {
                    wctMain.Model.GoogleProp.OrganizationSchema org = new Model.GoogleProp.OrganizationSchema(_ctx.DefaultVenue);
                    litGglVenue.Text = string.Format("<div style=\"display:none\">{0}</div>{1}", org.WriteToFormat(null, false), Environment.NewLine);
                }
            }
        }
}
}
