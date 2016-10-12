using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data;
using System.Collections.Generic;
//using System.Web.Script.Serialization;

using Wcss;
using wctMain.Model;

namespace wctMain
{
    public partial class Index : wctMain.Controller.MainBasePage
    {
        #region Properties
        
        private string _rawParsed = null;
        public string RawParsed
        {
            get
            {
                if(_rawParsed == null)
                    _rawParsed = this.Request.RawUrl.Trim(new char[] { '/', '#', '!' }).Replace(".aspx", string.Empty).Trim().ToLower();

                return _rawParsed;
            }
        } 

        #endregion

        #region Page Loop and Main Logic

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            QualifySsl(false);
        }
        
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
            WriteMetaTags();
            
            //put event list to the page - side menu and upcoming
            Output_EventDateModel_Json();
        }

        #endregion

        #region Methods        

        private void WriteMetaTags()
        {
            //determine if we are to use an event or a just follow the url to populate meta data - contact, faq
            ShowDate sd = null;

            string title = RawParsed.Replace("-", " ").Replace("/", " - ").Trim();
            string url = RawParsed;
            string image = _Config._DefaultShowImage;
            string description = title.Trim();

            // EXAMINE URL for show content
            //an empty url indicates that we should get defult show
            //allow for an old show not being found 
            //show urls have dashes - static pages do not and have slashes
            //legacy shows have slashes but are redirected with dashes
            if (url.Trim().Length == 0)
            {
                sd = Ctx.ShowRepo_Web_Displayable[0].FirstShowDate;
            }
            else if (url.IndexOf("-") != -1)
            {
                Show s = Ctx.GetCurrentShowByUrl(this.Request.RawUrl, false, true);
                if (s != null)
                    sd = s.FirstShowDate;
            }

            if (sd != null)
            {
                //set the default show - from socials Date/Acts/ dont include @Venue 95 chars max (fb) 70 -twtr
                title = sd.ShowRecord.Display.DisplaySocial_Description_ForSocialPosts.Trim();
                url = sd.ConfiguredUrl_withDomain;
                
                if (sd.ShowRecord.ShowImageUrl_Backstretch == null)
                    image = _Config._DefaultShowImage;
                else
                    image = sd.ShowRecord.ShowImageUrl;

                if (Request.UserAgent.ToLower().IndexOf("facebo") != -1) {

                    HtmlMeta og_image_width = (HtmlMeta)this.Page.Master.FindControl("og_image_width");
                    HtmlMeta og_image_height = (HtmlMeta)this.Page.Master.FindControl("og_image_height");

                    // calculate lg img dimensions from show image
                    if (og_image_width != null && og_image_height != null)
                    {
                        try
                        {
                            System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(image));
                            og_image_width.Content = p.First.ToString();
                            og_image_height.Content = p.Second.ToString();
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);
                        }
                    }
                
                }

                description = sd.ShowRecord.Display.Display_Time_Ages_Tickets;
            } 
            else if(Request.UserAgent.ToLower().IndexOf("facebo") != -1)
            {
                //just copied logic from above
                Show s = Ctx.GetCurrentShowByUrl(this.Request.RawUrl, false, false);
                if (s != null)
                {
                    ShowDate _sd = s.FirstShowDate;
                    title = _sd.ShowRecord.Display.DisplaySocial_Description_ForSocialPosts.Trim();
                    url = _sd.ConfiguredUrl_withDomain;
                    description = _sd.ShowRecord.Display.Display_Time_Ages_Tickets;
                    

                    if (_sd.ShowRecord.ShowImageUrl_Backstretch == null)
                    {
                        image = _Config._DefaultShowImage;
                    }
                    else
                    {
                        image = _sd.ShowRecord.ShowImageUrl;// _Backstretch.ToLower().Replace("thumbmx", "thumblg");
                    }
                    
                    HtmlMeta og_image_width = (HtmlMeta)this.Page.Master.FindControl("og_image_width");
                    HtmlMeta og_image_height = (HtmlMeta)this.Page.Master.FindControl("og_image_height");
                    
                    // calculate lg img dimensions from show image
                    if (og_image_width != null && og_image_height != null) {
                        try
                        {
                            System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(image));
                            og_image_width.Content = p.First.ToString();
                            og_image_height.Content = p.Second.ToString();
                        }
                        catch (Exception ex) {
                            _Error.LogException(ex);
                        }
                    }

                }
            }


            // don't follow if no sd
            if (sd == null) {

                //string robo = "<META NAME=\"ROBOTS\" CONTENT=\"NOINDEX, NOFOLLOW\">";
                HtmlMeta robo = (HtmlMeta)this.Page.Master.FindControl("meta_robo");
                robo.Name = "ROBOTS";
                robo.Content = "NOINDEX, NOFOLLOW";

            }
            //end of examining for show content


            //retrieve tags and rewrite
            HtmlMeta og_title = (HtmlMeta)this.Page.Master.FindControl("og_title");
            HtmlMeta og_url = (HtmlMeta)this.Page.Master.FindControl("og_url");
            HtmlMeta og_image = (HtmlMeta)this.Page.Master.FindControl("og_image");
            HtmlMeta og_description = (HtmlMeta)this.Page.Master.FindControl("og_description");
            HtmlMeta og_appid = (HtmlMeta)this.Page.Master.FindControl("og_appid");
            HtmlMeta og_admins = (HtmlMeta)this.Page.Master.FindControl("og_admins");            

            //max values are based on twitter cards - should be lowest denominator
            og_title.Content = title;//70 max


            // this happens when we only have raw url and can't match an sd
            if (!url.ToLower().StartsWith("http")) { 
                url = string.Format("http://{0}/{1}", _Config._DomainName, url);

            }


            og_url.Content = url.Replace("www.", string.Empty).Replace("WWW.", string.Empty);

            if (image.Trim().Length == 0)
                image = _Config._DefaultShowImage;

            og_description.Content = (description.Length > 200) ? description.Remove(199).Trim() : description;//200 max
            og_appid.Content = _Config._FacebookIntegration_App_Id.ToString();
            og_admins.Content = _Config._FacebookIntegration_App_AdminList.ToString();

            og_image.Content = string.Format("http://{0}/{1}?{2}", 
                _Config._DomainName, 
                image.TrimStart('/').Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries)[0],
                DateTime.Now.Ticks.ToString());

            //google image is handled in backstretch
        }
        
        private void Output_EventDateModel_Json( )
        {
            string data = Ctx.WebData_Json;

            //output to content holder
            ContentPlaceHolder cont = (ContentPlaceHolder)this.Page.Master.FindControl("JsonObjects");
            if (cont != null)
            {
                cont.Controls.Clear();
                cont.Controls.Add(new LiteralControl(data));

                string uAgent = "no agent recorded";
                try
                {
                    uAgent = Request.UserAgent;
                }
                catch (Exception ex) { }
                
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                
                sb.AppendLine("<script type=\"text/javascript\">");
                sb.AppendLine();
                sb.AppendFormat("var uAgent = '{0}';", uAgent);
                sb.AppendLine();
                sb.AppendLine("</script>");
                sb.AppendLine();
                
                cont.Controls.Add(new LiteralControl(sb.ToString()));
            }
        }

        #endregion

    }
}
