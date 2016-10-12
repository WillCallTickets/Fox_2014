using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.IO;

using Wcss;
using wctMain.Model;

namespace wctMain
{
    /// <summary>
    /// For the calendar mode to work - all calendar kiosks must have the word calendar in them
    /// </summary>
    public partial class Kiosk : wctMain.Controller.MainBasePage
    {
        protected string classInfo = string.Empty;
        protected string mode = string.Empty;

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

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            //find the meta refresh and replace the url with what we have here
            string req = this.Request.RawUrl;//rawurl

            HtmlMeta refresh = (HtmlMeta)this.Page.Master.FindControl("metaRefresh");
            if (refresh != null)
            {
                //refresh.Attributes("URL", req);                
                bool test = this.Request.QueryString.ToString().IndexOf("test", StringComparison.OrdinalIgnoreCase) != -1;
                //time is in seconds
                int refreshInterval = (test) ? 100 : 900;// 00;//900 is 15 minutes

                refresh.Content = string.Format("{0}; URL={1}", refreshInterval.ToString(), req);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //set the mode for the page - empty is all, fox is fox, bt is bt, z2 is z2
            List<string> segments = new List<string>();
            
            segments.AddRange(Request.Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));

            //segment[1] is designated context
            if (segments.Count > 1)
            {   
                string seg = segments[1].ToLower().Trim(new char[] { '/' }).Trim();

                if (seg == String.Empty || seg == "fox")
                    mode = _Enums.Principal.fox.ToString();
                else if (seg == "all")
                    mode = _Enums.Principal.all.ToString();
                else if (seg == "bt")
                    mode = _Enums.Principal.bt.ToString();
                else if (seg == "z2")
                    mode = _Enums.Principal.z2.ToString();
                else if (seg == "cal" || seg == "calendar")
                    mode = "calendar";

            }

            //other segments for future use here
            //if (segments.Count > 2)


            //class information will be derived from the query string
            //ex: http://local.fox2014.com/kiosk/bt?bt-balcony
            //just use the querystring as a location or device name
            string req = Request.QueryString.ToString();
            int idx = req.IndexOf("&");
            if (idx != -1)
                req = req.Substring(0, idx);

            if (req.Trim().Length > 0)
                classInfo = req;
            
            rptKiosk.DataBind();                

            Output_KioskModel_Json();
        }

        //Json model is sync'd to list collection, json feeds the js view model
        private void Output_KioskModel_Json()
        {
            string data = Ctx.KioskData_Json;

            //output to content holder
            ContentPlaceHolder cont = (ContentPlaceHolder)this.Page.Master.FindControl("JsonObjects");
            if (cont != null)
                cont.Controls.Add(new LiteralControl(data));
        }

        protected void rptKiosk_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            //ensure kiosk are current
            rpt.DataSource = GetListInTime;
        }

        private List<Wcss.Kiosk> GetListInTime
        {
            get
            {
                DateTime now = DateTime.Now;
                List<Wcss.Kiosk> list = GetListInContext;
                return list.FindAll(delegate(Wcss.Kiosk k) { return (k.DateStart < now && k.DateEnd > now); });
            }
        }

        private List<Wcss.Kiosk> GetListInContext
        {
            get
            {
                if (mode == "calendar")
                    return Ctx.KioskList
                        .FindAll(delegate(Wcss.Kiosk k) { return (k.Name.ToLower().IndexOf(mode) != -1); });
                else if (mode != "all")
                    return Ctx.KioskList
                        .FindAll(delegate(Wcss.Kiosk k) { return (k.VcPrincipal.IndexOf(mode, StringComparison.OrdinalIgnoreCase) != -1); });
                
                //fallthrough - return all
                return Ctx.KioskList;

            }

        }
                
        protected void rptKiosk_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            Wcss.Kiosk ent = (Wcss.Kiosk)e.Item.DataItem;
            Literal litViewport = (Literal)e.Item.FindControl("litViewport");
            Literal litImage = (Literal)e.Item.FindControl("litImage");
            Literal litDivText = (Literal)e.Item.FindControl("litDivText");


            // catch the image manager null issue prior to this - not filling in the info here just breaks the page as it leaves
            // some things filled in and tags are left dangling with no content
            if (ent != null && litViewport != null && litImage != null && litDivText != null)
            {
                //allow for a test override
                bool test = this.Request.QueryString.ToString().IndexOf("test", StringComparison.OrdinalIgnoreCase) != -1;

                //timeout is in milliseconds
                int timeout = (test) ? 4000 : ent.Timeout;

                //timeout needs to be on entire container
                litViewport.Text = string.Format("<div class=\"kiosk-viewport\" data-cycle-timeout=\"{0}\">", timeout.ToString());


                // avoid breaking the page when an image is not specified
                string imageSrc = string.Empty;
                
                if (ent.ImageManager != null && ent.ImageManager.OriginalUrl != null &&
                    ent.ImageManager.OriginalUrl.Trim().Length > 0 && ent.ImageManager.Thumbnail_Max.Trim().Length > 0)
                {
                    imageSrc = ent.ImageManager.Thumbnail_Max.Trim();
                }

                litImage.Text = string.Format("<img src=\"{0}\" alt=\"{1}\" width=\"{2}\" />",
                    imageSrc,
                    ent.Name,
                    Wcss.Kiosk.MaxImageWidth.ToString()
                );



                litDivText.Text = ent.TextDiv;
            }
        }
    }
}