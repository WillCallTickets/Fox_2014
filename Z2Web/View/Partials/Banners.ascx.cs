using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using z2Main.Controller;

namespace z2Main.View.Partials
{
    [ToolboxData("<{0}:Banners runat='server'></{0}:Banners>")]
    public partial class Banners : ControlBase
    {
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
            if (!IsPostBack)
            {   
                rptSlides.DataBind();                
            }
        }

        protected void rptSlides_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;            
            rpt.DataSource = Ztx.Banners;
        }
        
        protected void rptSlides_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            SalePromotion ent = (SalePromotion)e.Item.DataItem;
            Literal litSlideStart = (Literal)e.Item.FindControl("litSlideStart");
            Literal litSlideEnd = (Literal)e.Item.FindControl("litSlideEnd");

            if (ent != null && litSlideStart != null && litSlideEnd != null)
            {
                //string s = ent.Banner_VirtualFilePath;

                //string filePath = string.Format("{0}/{1}", Z2Config.VIRTUAL_BANNER_DIR, ent.BannerUrl);
                //string mappedFile = System.Web.HttpContext.Current.Server.MapPath(filePath);

                //string f = null;
                //if (System.IO.File.Exists(mappedFile))
                //    f = filePath;


                litSlideStart.Text = string.Format("<div data-cycle-timeout=\"{0}\">", ent.BannerTimeout.ToString());

                //structure must be a div (for margin spacing) and the anchor within (if available)
                string link = (ent.BannerClickUrl != null && ent.BannerClickUrl.Trim().Length > 0) ? ent.BannerClickUrl.Trim() : null;

                if(link != null)
                    litSlideStart.Text += string.Format("<a href=\"{0}\" title=\"{1}\" >", link, ent.DisplayText ?? string.Empty);

                litSlideEnd.Text = (link != null) ? "</a></div>" : "</div>";
            }
        }
}
}