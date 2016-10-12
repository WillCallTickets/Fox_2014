using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Wcss;

namespace wctMain.Model
{   
    /// <summary>
    /// EventDate is the culmination/extraction of a ShowDate object. It is pared down to only the 
    /// information necessary for displaying on the front-end of the website.
    /// In reality, it is only used for the left hand event menu listing and the featured/upcoming shows control.
    /// Only the ID is used to fetch the show information.
    /// </summary>
    public class EventDate
    {
        public string Id { get; set; }
        public string EventOrigin { get; set; }
        public string Url { get; set; }
        public double AnnounceTicks { get; set; }
        public string OnSaleDate { get; set; }
        public double OnSaleTicks { get; set; }//dtOnSale is on ly necessary for the show display
        public double ShowDateTicks { get; set; }
        ///showdates are not dependent on timezone, etc
        public string DisplayMonth { get; set; }
        ///showdates are not dependent on timezone, etc
        public string DisplayDay { get; set; }
        public string DoorTime { get; set; }
        public string ShowTime { get; set; }
        public string SortTime { get; set; }

        //set this in a static method that examines an eventdate collection and sets the values there
        //if there is another show on the same date - show the time
        //public string DisplayTime { get; set; }//aids in connecting logic to the display

        public string Status { get; set; }
        public string Alert { get; set; }        
        public string Pricing { get; set; }
        public string TicketUrl { get; set; }
        public string Ages { get; set; }
        public string Billing { get; set; }
        public string BillingOpens { get; set; }
        
        //ctors
        public EventDate() { }
        
        public EventDate(ShowDate sd)
        {
            if (!sd.IsActive)
                throw new ArgumentOutOfRangeException("Show must be active to be listed.");
            Id = sd.Id.ToString(); 
            EventOrigin = "foxevent";
            Url = sd.ConfiguredUrl;

            AnnounceTicks = Utils.ParseHelper.DateTime_To_JavascriptDate(sd.ShowRecord.AnnounceDate.ToUniversalTime());
            OnSaleDate = (sd.ShowRecord.DateOnSale > DateTime.Now) ? string.Format("{0}", sd.ShowRecord.DateOnSale.ToString("ddd MMM dd")) : string.Empty;
            OnSaleTicks = (OnSaleDate.Trim().Length > 0) ? Utils.ParseHelper.DateTime_To_JavascriptDate(sd.ShowRecord.DateOnSale.ToUniversalTime()) : 0;
            
            ShowDateTicks = Utils.ParseHelper.DateTime_To_JavascriptDate(sd.DateOfShow.ToUniversalTime());
            //displayed showdates are not dependent on timezone, etc
            DisplayMonth = sd.DateOfShow.ToString("MMM");
            DisplayDay = sd.DateOfShow.Day.ToString();

            DoorTime = sd.DoorTime;
            ShowTime = sd.ShowTime ?? string.Empty;
            SortTime = sd.DateOfShow_ToSortBy.ToString("yyyy/MM/dd hh:mm:tt");

            string tmpStatus = Utils.ParseHelper.StripHtmlTags(sd.StatusName).Trim();
            if (sd.IsSoldOut || tmpStatus == _Enums.ShowDateStatus.SoldOut.ToString())
                tmpStatus = "Sold Out!";
            Status = ( tmpStatus != _Enums.ShowDateStatus.OnSale.ToString() ) ? tmpStatus : string.Empty;
            Alert = Utils.ParseHelper.StripHtmlTags(sd.ShowRecord.ShowAlert ?? string.Empty).Trim();
            Pricing = Utils.ParseHelper.StripHtmlTags(sd.PricingText ?? string.Empty).Trim();
            TicketUrl = sd.TicketUrl ?? string.Empty;
            Ages = Utils.ParseHelper.StripHtmlTags(sd.AgesString ?? string.Empty).Trim();
            Billing = Utils.ParseHelper.StripHtmlTags((sd.MenuBilling != null && sd.MenuBilling.Trim().Length > 0) ?
                sd.MenuBilling.Trim() : sd.Display.Heads_NoFeatures).Trim();
            
            // ensure that billing has something!!!
            if (Billing.Trim().Length == 0) {

                Billing = sd.ShowRecord.GetShowMainActPart;
            }

            BillingOpens = Utils.ParseHelper.StripHtmlTags(sd.ShowRecord.listOpeners).Trim();
        }

        public EventDate(BT_EventItem bt)
        {
            EventOrigin = "btevent";
            Id = (EventOrigin + Url).GetHashCode().ToString();
            Url = bt.ShowUrl;
            AnnounceTicks = 0;
            OnSaleTicks = AnnounceTicks;
            OnSaleDate = string.Empty;

            ShowDateTicks = Utils.ParseHelper.DateTime_To_JavascriptDate(bt.ShowDate.ToUniversalTime());
            //displayed showdates are not dependent on timezone, etc
            DisplayMonth = bt.ShowDate.ToString("MMM");
            DisplayDay = bt.ShowDate.Day.ToString();

            DoorTime = string.Empty;
            ShowTime = string.Empty;
            SortTime = bt.ShowDate.ToString("yyyy/MM/dd hh:mm:tt");

            Status = Utils.ParseHelper.StripHtmlTags(bt.Status).Trim();
            Alert = string.Empty;
            Pricing = string.Empty;
            TicketUrl = string.Empty;
            Ages = string.Empty;
            Billing = Utils.ParseHelper.StripHtmlTags(bt.Title).Trim();
        }
    }
}