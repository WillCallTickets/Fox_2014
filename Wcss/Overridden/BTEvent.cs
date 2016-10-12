using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ServiceModel.Syndication;
using System.Web.UI;
using System.Web;
using Newtonsoft.Json;

using CsQuery;

namespace Wcss
{
    public partial class BT_EventFeed
    {
        public string publishdatestamp { get; set; }
        public string lastbuilddate { get; set; }
        public string title { get; set; }
        public string homepage { get; set; }
        public string copyright { get; set; }
        public List<BT_EventItem> eventlist { get; set; }
    }

    /// <summary>
    /// Property names must correspond to feed prop names
    /// </summary>
    public partial class BT_EventItem
    {
        //*** Put these into feed
        public int Id { get; set; }
        public DateTime AnnounceDate { get; set; }
        public string ShowUrl { get; set; }
        public string FacebookEventUrl { get; set; }
        public string Status { get; set; }
        
        public DateTime? DateOnSale { get; set; }

        public string Venue { get; set; }
        public DateTime ShowDate { get; set; }
        public string DoorTime { get; set; }
        public string ShowTime { get; set; }
        public string Ages { get; set; }        
        public string TicketPrice { get; set; }
        public string TicketLink { get; set; }
        public string ShowImage { get; set; }
        public string ShowThumbnail { get; set; }

        public string Title { get; set; }
        public string PresentedBy { get; set; }
        public string Headliners { get; set; }
        public string Openers { get; set; }

        public BT_EventItem() {}

        /// <summary>
        /// Used for generic listing in lists
        /// </summary>
        public BT_EventItem(string headliner, string status)
        {
            this.Headliners = headliner;
            this.Venue = "Boulder Theater";
            this.ShowDate = DateTime.Now;
            this.Status = status;
        }

        public string DateAndTitleForListing
        {
            get
            {
                string s = string.Format("{0} - {1}", 
                    ShowDate.ToString("yyyy/MM/dd"), 
                    (Headliners != null && Headliners.Trim().Length > 0) ? Headliners.ToUpper() : 
                    (Title != null && Title.Trim().Length > 0) ? Title.ToUpper() : 
                    (Openers != null && Openers.Trim().Length > 0) ? Openers.ToUpper() :
                    "***THIS SHOW IS UNTITLED***").Replace(",", " ");
                s = Utils.ParseHelper.StripHtmlTags(s);
                s = (s.Length > 50) ? s.Substring(0, 49).Trim() : s;

                //s = HttpUtility.HtmlDecode(s);
                //s = HttpUtility.HtmlEncode(s);
                
                return s;


            }
        }

        public bool IsThumbnailSameAsImage
        {
            get
            {
                return (ShowThumbnail != null && ShowThumbnail.Trim().Length > 0 && ShowImage != null && ShowThumbnail.Trim() == ShowImage.Trim());
            }
        }
    }
}