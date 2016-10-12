using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web;
using System.Text;
using System.Linq;

using Wcss;

// <%@ OutputCache Duration="500" VaryByParam="none" %>


namespace wctMain
{
    public partial class Json_UpcomingShows : wctMain.Controller.MainBasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            this.Theme = string.Empty;
        }

        private ShowDateCollection dates = null;
        

        protected string _JSONtext = string.Empty;
        protected DateTime startDate = DateTime.Now;
        protected DateTime endDate = DateTime.Now;
        protected string domain = _Config._DomainName;
        protected string defaultImage = string.Format("http://{0}/{1}/Images/UI/{2}", _Config._DomainName, _Config._VirtualResourceDir, "logoforiggli.jpg");

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
            Response.ContentEncoding = Encoding.UTF8;

            dates = new ShowDateCollection();
            foreach (Show s in Ctx.ShowRepo_Web_Displayable)
                if((!dates.Contains(s.FirstShowDate)))
                    dates.Add(s.FirstShowDate);

            if (dates.Count > 0)
            {
                startDate = dates[0].DateOfShow;
                endDate = dates[dates.Count - 1].DateOfShow;
            }

            ConstructJSON();

            Response.Write(_JSONtext);
        }
        
        public static char[] trimchars = { ' ', ',' };

        public void ConstructJSON()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("{");
            string title = string.Format("Upcoming Shows - {0} {1}",
                _Config._Site_Entity_Name, "BOULDER, CO");
                //,
                //startDate.ToString("MM/dd/yy"), endDate.ToString("MM/dd/yy"));

            sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("publishDateStamp", Utils.ParseHelper.ParseJSON(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"))));


            sb.AppendFormat("{0}, ", 
                Utils.ParseHelper.ReturnJSONFormat("lastBuildDate", Utils.ParseHelper.JavascriptDate_To_DateTime(Ctx.PublishVersion_Announced).ToString("MM-dd-yyyy HH:mm:ss")));


            //convert to long to avoid any decimal places
            //sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("lastBuildDate", Utils.ParseHelper.JavascriptDate_To_DateTime( Utils.ParseHelper.ParseJSON(
            //    ((long)Ctx.PublishVersion_Announced).ToString())));

            sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("title", Utils.ParseHelper.ParseJSON(title)));
            sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("homePage", _Config._Site_Entity_HomePage));
            sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("copyright", string.Format("Copyright {0} {1}", 
                DateTime.Now.Year.ToString(), Utils.ParseHelper.ParseJSON(_Config._Site_Entity_Name))));
            sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("siteImage", defaultImage));

            sb.AppendFormat("\"showDates\": [ ");

            int i = 0;

            //loop thru showdates
            foreach(ShowDate sd in dates)
            {
                sb.AppendFormat("{{ \"{0}\": \"{1}\", ", "showDate", Utils.ParseHelper.ParseJSON(sd.DateOfShow.ToString("MM/dd/yyyy")));

                // OLD method before the app changes in Nov 2014
                //sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("title", 
                //    string.Format("{0} {1}", 
                //    Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Headliners_NoMarkup_Verbose_NoLinks),
                //    Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Openers_NoMarkup_Verbose_NoLinks).Trim())));

                string billing = Utils.ParseHelper.ParseJSON(Utils.ParseHelper.StripHtmlTags((sd.MenuBilling != null && sd.MenuBilling.Trim().Length > 0) ?
                    sd.MenuBilling.Trim() : sd.Display.Heads_NoFeatures)).Trim();

                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("title", billing));


                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("venue", Utils.ParseHelper.ParseJSON(sd.ShowRecord.VenueRecord.Name)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showImage",
                    string.Format("http://{0}{1}", domain, sd.ShowRecord.ShowImageUrl_Backstretch)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("siteEventUrl",
                    string.Format("http://{0}/{1}", domain, sd.ConfiguredUrl)));

                string eventUrl = (sd.TicketUrl != null && sd.TicketUrl.Trim().Length > 0) ? sd.TicketUrl : string.Empty;
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("ticketLink", eventUrl));

                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showId", sd.TShowId.ToString()));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("dateStatus", Utils.ParseHelper.ParseJSON(sd.ShowRecord.ShowAlert)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showTitle", Utils.ParseHelper.ParseJSON(sd.ShowRecord.ShowTitle)));
                
                ////string presents = Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Promoters_NoMarkup_NoLinks);
                ////presents = presents.Replace(@"'", @"\'");
                ////presents = presents.Replace(@"/", @"\/");
                
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("presentedBy", Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Promoters_NoMarkup_NoLinks)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("header", Utils.ParseHelper.ParseJSON(sd.ShowRecord.ShowHeader)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("headliner", Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Headliners_NoMarkup_Verbose_NoLinks)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("opener", Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Openers_NoMarkup_Verbose_NoLinks)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showNotes", Utils.ParseHelper.ParseJSON(sd.ShowRecord.DisplayNotes)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("doorTime", sd.DateOfShow.ToString("hh:mmtt")));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showTime", Utils.ParseHelper.ParseJSON(sd.ShowTime)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("ages", Utils.ParseHelper.ParseJSON(sd.AgesString)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("ticketPricing", Utils.ParseHelper.ParseJSON(sd.PricingText)));

                //do not substring the writeup until you can ensure that a tag will not be split
                string decodedWriteup = System.Web.HttpUtility.HtmlDecode(sd.ShowRecord.ShowWriteup_Derived);
                string stripped = Utils.ParseHelper.StripHtmlTags(decodedWriteup);
                string parsed = Utils.ParseHelper.ParseJSON(stripped);
                //remove - escape double quotes

                string jsoned = Utils.ParseHelper.ReturnJSONFormat("writeup", parsed);
                sb.AppendFormat("{0}, ", jsoned);

                ////int descLength = 175;
                //string writeup = Utils.ParseHelper.StripHtmlTags( Utils.ParseHelper.ParseJSON(sd.ShowRecord.ShowWriteup_Derived));
                ////if (writeup.Length > 0 && writeup.Length > descLength)
                ////    writeup = string.Format("{0}...", writeup.Substring(0, descLength));
                ////writeup = writeup.Replace("\'", @"\'").Replace("\"", @"\""").Replace("/", @"\/");
                ////writeup = writeup.Replace('"', '\"');
                //sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("writeup", writeup));

                //mp3 section - keep for legacy compatibility
                sb.AppendFormat("\"{0}\": [ ] ", "mp3s");
                //end mp3 section

                sb.AppendFormat("}}");
                sb.AppendFormat("{0} ", (i == (dates.Count - 1)) ? string.Empty : ",");
                i++;
                //end showdates
            }

            sb.AppendFormat(" ] ");            
            sb.AppendFormat("}}");
            
            _JSONtext = sb.ToString();
        }
}
}
