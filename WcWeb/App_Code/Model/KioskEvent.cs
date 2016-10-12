using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Wcss;

namespace wctMain.Model
{   
    /// <summary>
    /// Only the ID is used to fetch the show information.
    /// </summary>
    public class KioskEvent
    {
        public string Id { get; set; }
        public int DisplayOrder { get; set; }
        public int Timeout { get; set; }
        public string Name { get; set; }
        public string DisplayUrl { get; set; }
        public string CtrX { get; set; }
        public string CtrY { get; set; }
        public double StartDate { get; set; }
        public double EndDate { get; set; }
        public string DivText { get; set; }
        
        //ctors
        public KioskEvent() { }
        
        public KioskEvent(Kiosk ent)
        {
            if (!ent.IsActive)
                throw new ArgumentOutOfRangeException("Kiosk must be active to be listed.");

            Id = ent.Id.ToString(); 
            //DisplayOrder = ent.DisplayOrder;
            Timeout = ent.Timeout;
            Name = ent.Name;
            DisplayUrl = (ent.ImageManager != null) ? ent.ImageManager.Thumbnail_Max : string.Empty;
            CtrX = ent.Centered_X.ToString();
            CtrY = ent.Centered_Y.ToString();

            StartDate = (ent.DateStart != DateTime.MinValue) ? Utils.ParseHelper.DateTime_To_JavascriptDate(ent.DateStart) : 0;
            EndDate = (ent.DateEnd != DateTime.MaxValue) ? Utils.ParseHelper.DateTime_To_JavascriptDate(ent.DateEnd) : Utils.ParseHelper.DateTime_To_JavascriptDate(DateTime.MaxValue);
            DivText = ent.TextDiv;
        }
    }
}