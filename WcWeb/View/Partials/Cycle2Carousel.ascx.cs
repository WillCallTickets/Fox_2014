using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using wctMain.Controller;



/* Notes on structure
 * 
 * carousel-container, defined outside of this component, defines the top and bottom margins and l-r auto to center the component
 * 
 * cycle-wrapper defines the width of the carousel, as well as defining the outer border and outer box shadow
 *  mobile is 95%, med is 85% and large is 80%
 * 
 */

namespace wctMain.View.Partials
{
    [ToolboxData("<{0}:Cycle2Carousel runat='server'></{0}:Cycle2Carousel>")]
    public partial class Cycle2Carousel : MainBaseControl
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
            rpt.DataSource = Ctx.BannerList;
        }
        
        protected void rptSlides_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            SalePromotion ent = (SalePromotion)e.Item.DataItem;
            Literal litSlideStart = (Literal)e.Item.FindControl("litSlideStart");
            Literal litSlideEnd = (Literal)e.Item.FindControl("litSlideEnd");

            if (ent != null && litSlideStart != null && litSlideEnd != null)
            {
                string s = ent.Banner_VirtualFilePath;

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