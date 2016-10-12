using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Linq;
using System.Xml;

using Wcss;

namespace wctMain
{
    /* http://deepumi.wordpress.com/2010/03/14/create-rss-2-0-and-atom-1-0-in-asp-net-3-5-csharp/
     * 
     * <mp3s><![CDATA[<asp:Literal id="litMp3" runat="server" />]]></mp3s>
                    <writeup><![CDATA[<%#Eval("ShowRecord.ShowWriteup_Derived") %>]]></writeup>
     * <%@ OutputCache Duration="120" VaryByParam="none" %>
     * * 
     */

    public partial class Rss : wctMain.Controller.MainBasePage
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
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            string domain = _Config._DomainName;
            string defaultImage = string.Format("http://{0}/{1}/Images/UI/{2}", _Config._DomainName, _Config._VirtualResourceDir, "logoforiggli.jpg");
            //int maxShows = 75;
            Uri uri = new Uri("http://foxtheatre.com");
            string title = "Upcoming Shows at The Fox Theatre BOULDER, CO";
            
            List<ShowDate> dates = new List<ShowDate>();
            foreach (Show s in Ctx.ShowRepo_Web_Displayable)
                if((!dates.Contains(s.FirstShowDate)))
                    dates.Add(s.FirstShowDate);

            int count = dates.Count();
            if (count > 0)
            {
                startDate = dates.First().DateOfShow;
                endDate = dates.Last().DateOfShow;
            }

            string description = string.Format("Upcoming Shows at The Fox Theatre BOULDER, CO", startDate.ToString("MM/dd/yy"), endDate.ToString("MM/dd/yy"));

            List<SyndicationItem> items = new List<SyndicationItem>();
            foreach(ShowDate sd in dates)
            {
                Uri luri = new Uri(string.Format("http://{0}/{1}", domain, sd.ConfiguredUrl));

                string str = sd.ShowStatusRecord.Name;
                
                string status = (str != null) ? 
                    (str.Trim().Length > 0 && str.ToLower() != "onsale") ?  string.Format(" ***{0}***", str) : string.Empty
                    : string.Empty;
                
                SyndicationItem oItem = new SyndicationItem(
                    string.Format("{0} {1}{2}", sd.DateOfShow.ToString("MM/dd/yyyy"), sd.ShowRecord.Display.Headliners_NoMarkup_Verbose_NoLinks, status),
                    string.Empty,
                    luri
                );
                
                oItem.Content = SyndicationContent.CreateHtmlContent(string.Format("{0}", getDescription(sd)));

                items.Add(oItem);
            }


            Response.Clear();
            Response.ContentType = "application/rss+xml";

            if (items.Count > 0)
            {
                SyndicationFeed feed = new SyndicationFeed(title, description, uri, items);
                feed.LastUpdatedTime = DateTime.Now;

                Rss20FeedFormatter formatter = new Rss20FeedFormatter(feed);

                XmlWriter writer = XmlWriter.Create(Response.Output, null);

                formatter.WriteTo(writer);

                writer.Flush();
            }
        }
  
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        private string getDescription(ShowDate date)
        {
            sb.Length = 0;

            sb.AppendFormat("<div>published: {0}</div>", (date.ShowRecord.AnnounceDate > date.DateOfShow_ToSortBy) ?
                        date.ShowRecord.AnnounceDate.ToLongDateString() : date.DtStamp.ToLongDateString());
            sb.AppendFormat("<div><strong>{0}</strong></div>", date.ShowRecord.VenueRecord.Name.Trim());
            string status = date.ShowRecord.ShowAlert;
            if (status != null && status.Trim().Length > 0)
                sb.AppendFormat("<div>{0}</div>", status.Trim());

            sb.AppendLine("<p>");
            sb.AppendLine("<strong>");

            //promoter 
            string promo = date.ShowRecord.Display.Promoters_NoMarkup_NoLinks;
            if (promo != null && promo.Trim().Length > 0)
                sb.AppendFormat("<div>{0}</div>", promo.Trim());

            if (date.ShowRecord.ShowTitle != null && date.ShowRecord.ShowTitle.Trim().Length > 0)
                sb.AppendFormat("<div>{0}</div>", date.ShowRecord.ShowTitle.Trim());

            //heads - opens
            //top text 
            string header = date.ShowRecord.ShowHeader;
            if (header != null && header.Trim().Length > 0)
                sb.AppendFormat("<div>{0}</div>", header.Trim());
            sb.AppendFormat("<div>{0}</div>", date.ShowRecord.Display.Headliners_NoMarkup_Verbose_NoLinks);
            string open = date.ShowRecord.Display.Openers_NoMarkup_Verbose_NoLinks;
            if (open != null && open.Trim().Length > 0)
                sb.AppendFormat("<div>{0}</div>", open.Trim());

            sb.AppendLine("</strong></p>");

            //displaynotes
            string notes = date.ShowRecord.DisplayNotes;
            if (notes != null && notes.Trim().Length > 0)
                sb.AppendFormat("<p>{0}</p>", notes.Trim());

            sb.AppendLine("<p>");

            //date of show - showtime - ages                    
            sb.AppendFormat("<div>Doors: {0} / Show: {1} / {2}</div>", date.DateOfShow.ToString("hh:mmtt"), date.ShowTime, date.AgesString);


            //link to tickets
            string tix = date.TicketUrl;
            string tixString = string.Empty;
            if (tix != null && tix.Trim().Length > 0 && (!date.ShowRecord.IsSoldOut) && (!date.IsSoldOut))
                tixString = string.Format("<a href=\"{0}\"><strong>Buy Ticekts!</strong></a>", tix.Trim());

            //pricing
            string pricing = date.PricingText;
            string priceString = string.Empty;
            if (pricing != null && pricing.Trim().Length > 0)
                priceString = string.Format("{0}", pricing.Trim());

            if (tixString.Length > 0 || priceString.Length > 0)
            {
                sb.Append("<div>");
                if (tixString.Length > 0)
                    sb.AppendFormat("{0} ", tixString);
                if (priceString.Length > 0)
                    sb.Append(priceString);
                sb.Append("</div>");
            }

            sb.AppendLine("</p>");


            int descLength = 350;
            string writeup = Utils.ParseHelper.StripHtmlTags(date.ShowRecord.ShowWriteup_Derived);
            if (writeup.Length > 0 && writeup.Length > descLength)
                writeup = string.Format("{0}...  <a href=\"{1}\" >[read more]</a>",
                    writeup.Substring(0, descLength),
                    date.ConfiguredUrl);
            if (writeup.Length > 0)
                sb.AppendFormat("<p>{0}</p>", writeup);

            sb.Append("<p></p><p></p>");

            return sb.ToString();
        }
    }
}