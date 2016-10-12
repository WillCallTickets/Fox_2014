using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace z2Main.Masters
{
    // in the head tag prefix="og: http://ogp.me/ns#"

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

        protected z2Main.Controller.Z2Context _ztx = null;
        protected z2Main.Controller.Z2Context Ztx
        {
            get
            {
                if (_ztx == null)
                    _ztx = new Controller.Z2Context();

                return _ztx;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptFox.DataBind();
                rptBt.DataBind();
            }
        }

        protected void rptList_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            if (rpt.ID == "rptFox")
                rpt.DataSource = Ztx.FoxUpcomingShows;
            else
                rpt.DataSource = Ztx.BtRssEventListing;            
        }

        protected void rptList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litLink = (Literal)e.Item.FindControl("litLink");

                if (rpt.ID == "rptFox")
                {
                    Show ent = (Show)e.Item.DataItem;
                    ShowDate sd = ent.FirstShowDate;

                    string billing = Utils.ParseHelper.StripHtmlTags((sd.MenuBilling != null && sd.MenuBilling.Trim().Length > 0) ?
                        sd.MenuBilling.Trim() : sd.Display.Heads_NoFeatures).Trim();

                    litLink.Text = string.Format("<li><a href=\"http://www.foxtheatre.com/{0}\" alt=\"{1}\" ><span class=\"date\">{2}</span><span class=\"billing\">{3}</span></a></li>",
                       HttpUtility.UrlDecode(sd.ConfiguredUrl), ent.Name, 
                        sd.DateOfShow.ToString("MM/dd"), 
                        billing.ToLower());
                }
                else
                {
                    BT_EventItem ent = (BT_EventItem)e.Item.DataItem;

                    litLink.Text = string.Format("<li><a href=\"{0}\" alt=\"{1}\" ><span class=\"date\">{2}</span><span class=\"billing\">{3}</span></a></li>", 
                        ent.ShowUrl,
                        ent.Title,
                        ent.ShowDate.ToString("MM/dd"),
                        HttpUtility.HtmlDecode(ent.Headliners) 
                        );
                    // use next for testing announce date
                    // litLink.Text += string.Format("<li>{0}</li>", ent.AnnounceDate.ToString("MM/dd hh:mmtt"));
                }
            }
        }
}
}
