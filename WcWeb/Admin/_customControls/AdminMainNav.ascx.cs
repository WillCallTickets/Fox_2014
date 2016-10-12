using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using wctMain.Controller;

namespace wctMain.Admin._customControls
{
    public partial class AdminMainNav : MainBaseControl
    {
        protected override void OnLoad(EventArgs e)
        {
            litMenu.DataBind();         
        }
        
        protected void litMenu_DataBinding(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string page = this.Page.ToString().ToLower();
            //allows us to easily change the color scheme
            string normal = "primary";
            string active = "info";

            bool canSuperOnly = this.Page.User.IsInRole("Super");
            bool canAdmin = this.Page.User.IsInRole("Administrator");
            bool canViewContent = (canAdmin || this.Page.User.IsInRole("ContentEditor"));
            bool canKiosk = (canAdmin || this.Page.User.IsInRole("Kiosker"));
            bool canMassMail = (canAdmin || this.Page.User.IsInRole("MassMailer"));
            bool canVenueData = (this.Page.User.IsInRole("VenueDataViewer"));
            
            //publish
            if (canViewContent)
            {
                sb.AppendLine(string.Format("<a id=\"publishMenuButton\" class=\"btn btn-danger btn-block btn-lg\" style=\"font-weight:bold;margin-bottom:15px;\" >{0}</a>", 
                    "Publish"));
            
                //Shows            
                sb.AppendLine(string.Format("<a class=\"btn btn-{0} btn-block\" href=\"/Admin/_shows/ShowDirector.aspx\">Shows</a>",
                    (page.IndexOf("showdirector_aspx") != -1) ? active : normal));
                //Banners
                sb.AppendLine(string.Format("<a class=\"btn btn-{0} btn-block\" href=\"/Admin/_collectionEditors/_banner/Banner_Director.aspx\">Banners</a>",
                    (page.IndexOf("banner_director_aspx") != -1) ? active : normal));
            }

            //Kiosk
            if (canKiosk)
            {
                sb.AppendLine(string.Format("<a class=\"btn btn-{0} btn-block\" href=\"/Admin/_collectionEditors/_kiosk/Kiosk_Director.aspx\">Kiosks</a>",
                    (page.IndexOf("kiosk_director_aspx") != -1) ? active : normal));
            }

            //mass mailer
            if (canMassMail)
            {
                sb.AppendLine(string.Format("<a class=\"btn btn-{0} btn-block\" href=\"/Admin/_mailer/Mail_Director.aspx\">Mass Mailers</a>",
                    (page.IndexOf("mail_director_aspx") != -1) ? active : normal));
            }

            //Posts
            if (canViewContent)
            {
                sb.AppendLine(string.Format("<a class=\"btn btn-{0} btn-block\" href=\"/Admin/_collectionEditors/_post/Post_Director.aspx\">Posts</a>",
                    (page.IndexOf("post_director_aspx") != -1) ? active : normal));
            

                //Editors
                sb.AppendLine(string.Format("<a class=\"btn btn-{0} btn-block\" href=\"/Admin/_editors/Editor_Director.aspx?p=act\">Editors</a>",
                    (page.IndexOf("editor_director_aspx") != -1 || page.IndexOf("edituser_aspx") != -1) ? active : normal));

                if (page.IndexOf("editor_director_aspx") != -1 || page.IndexOf("edituser_aspx") != -1)
                {
                    //acts/ages/customer/genre/promoter/venue/video/faq/employee
                    sb.AppendLine(string.Format("<div class=\"admin-menu-link\">"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_editors/Editor_Director.aspx?p=act\">Acts</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_editors/Editor_Director.aspx?p=age\">Ages</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_editors/Editor_Director.aspx?p=customer\">Customers</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_editors/Editor_Director.aspx?p=employee\">Employees</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_editors/Editor_Director.aspx?p=faq\">Faqs</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_editors/Editor_Director.aspx?p=genre\">Genres</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_editors/Editor_Director.aspx?p=promoter\">Promoters</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_editors/Editor_Director.aspx?p=defaultshow\">Set Default Show</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_editors/Editor_Director.aspx?p=venue\">Venues</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_editors/Editor_Director.aspx?p=video\">Video</a>"));
                    sb.AppendLine(string.Format("</div>"));
                }
            }

            //Settings
            if (canAdmin)
            {   
                sb.AppendLine(string.Format("<a class=\"btn btn-{0} btn-block\" href=\"/Admin/_settings/Settings_Director.aspx?p={1}\">Settings</a>",
                    (page.IndexOf("settings_director_aspx") != -1) ? active : normal,
                    (canSuperOnly) ? "admin" : "default"));

                if (page.IndexOf("settings_director_aspx") != -1)
                {
                    //add new/errors/admin/fbintegration/searches/defaults/email/images/downloads/order flow/page msgs
                    sb.AppendLine(string.Format("<div class=\"admin-menu-link\">"));

                    if (canSuperOnly)
                    {
                        sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=admin\">Admin</a>"));
                        sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=addnew\">Add New</a>"));
                        sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=errors\">Errors</a>"));
                        sb.AppendLine("<br/>");
                    }

                    sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=default\">Defaults</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=downloads\">Downloads</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=email\">Email</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=fb_integration\">Fb Integration</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=images\">Images</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=flow\">Order Flow</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=pagemsg\">Page Msgs</a>"));
                    sb.AppendLine(string.Format("<a href=\"/Admin/_settings/Settings_Director.aspx?p=searches\">Searches</a>"));

                    sb.AppendLine(string.Format("</div>"));
                }
            }
            
            //Venue Data
            if (canVenueData)
            {
                sb.AppendLine(string.Format("<a class=\"btn btn-{0} btn-block accord\" href=\"/Admin/_venueData/VenueData_Director.aspx\">Venue Data</a>",
                    (page == "asp.admin__venuedata_venuedata_director_aspx") ? active : normal));

                if (page == "asp.admin__venuedata_venuedata_director_aspx")
                {
                    sb.AppendLine(string.Format("<div class=\"admin-menu-link\">"));
                    sb.AppendLine("<a href=\"/Admin/_venueData/VenueData_Director.aspx?p=vnuview\">data view</a>");
                    sb.AppendLine("<a href=\"/Admin/_venueData/VenueData_Director.aspx?p=vnuactq\">act queries</a>");
                    sb.AppendLine("<a href=\"/Admin/_venueData/VenueData_Director.aspx?p=vnureport\">saved reports</a>");
                    sb.AppendLine("</div>");
                }
            }

            litMenu.Text = sb.ToString();
        }
    }
}
