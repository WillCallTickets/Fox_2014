using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin.ControlsFT
{
    public partial class NavMain : MainBaseControl
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<Triplet> list = new List<Triplet>();

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            sb.Length = 0;
            list.Clear();

            this.GatherMenuItems();

            foreach(Triplet t in list)
            {
                string text = t.First.ToString();
                
                sb.AppendFormat("<td>");
                
                //the span here holds the right side image
                //the b element alows us to vertically align the text
                //if we were to try and align the text within the span - the image would get all cut up
                sb.AppendFormat("{3}{4}<a href=\"{0}\" title=\"{1}\" class=\"btn btn-xs btn-info\" onclick=\"this.blur();\"><span>{2}</span></a></td>", 
                    t.Third.ToString(), t.Second.ToString(), text,
                    Utils.Constants.NewLine, Utils.Constants.Tabs(2));
            }

            litControlContent.Text = sb.ToString();
        }

        private List<Triplet> GatherMenuItems()
        {
            list.Add(new Triplet("Events", "see tickets on sale", "/"));

            if (_Config._FAQ_Page_On)
                list.Add(new Triplet("FAQ", "frequently asked questions", "/faq"));

            list.Add(new Triplet("Contact", "contact customer support", "/Contact/officeinfo"));
            
            if (!base.IsPageAdminContext)
                list.Add(new Triplet("Directions", "Get directions to the Fox Theatre", "/directions"));

            if (this.Page.User.IsInRole("Administrator"))
                list.Add(new Triplet("My Account", "view account details & purchase history", "/Admin/ControlsFT/EditProfile.aspx"));                
            
            if (this.Page.User.IsInRole("Manifester") || this.Page.User.IsInRole("OrderFiller") || this.Page.User.IsInRole("Administrator") ||
                this.Page.User.IsInRole("ContentEditor") || this.Page.User.IsInRole("MassMailer") || this.Page.User.IsInRole("ReportViewer"))
                list.Add(new Triplet("Admin", "administration", "/Admin/Default.aspx"));

            return list;
        }
    }
}