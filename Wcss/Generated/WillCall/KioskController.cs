using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
namespace Wcss
{
    /// <summary>
    /// Controller class for Kiosk
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class KioskController
    {
        // Preload our schema..
        Kiosk thisSchemaLoad = new Kiosk();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public KioskCollection FetchAll()
        {
            KioskCollection coll = new KioskCollection();
            Query qry = new Query(Kiosk.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public KioskCollection FetchByID(object Id)
        {
            KioskCollection coll = new KioskCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public KioskCollection FetchByQuery(Query qry)
        {
            KioskCollection coll = new KioskCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Kiosk.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Kiosk.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,bool BActive,int ITimeoutMsecs,int? TShowId,string Name,string DisplayUrl,int IPicWidth,int IPicHeight,bool BCtrX,bool BCtrY,string EventVenue,string EventDate,string EventTitle,string EventHeads,string EventOpeners,string EventDescription,string TextCss,DateTime? DtStartDate,DateTime? DtEndDate,Guid ApplicationId,string VcPrincipal,string VcJsonOrdinal)
	    {
		    Kiosk item = new Kiosk();
		    
            item.DtStamp = DtStamp;
            
            item.BActive = BActive;
            
            item.ITimeoutMsecs = ITimeoutMsecs;
            
            item.TShowId = TShowId;
            
            item.Name = Name;
            
            item.DisplayUrl = DisplayUrl;
            
            item.IPicWidth = IPicWidth;
            
            item.IPicHeight = IPicHeight;
            
            item.BCtrX = BCtrX;
            
            item.BCtrY = BCtrY;
            
            item.EventVenue = EventVenue;
            
            item.EventDate = EventDate;
            
            item.EventTitle = EventTitle;
            
            item.EventHeads = EventHeads;
            
            item.EventOpeners = EventOpeners;
            
            item.EventDescription = EventDescription;
            
            item.TextCss = TextCss;
            
            item.DtStartDate = DtStartDate;
            
            item.DtEndDate = DtEndDate;
            
            item.ApplicationId = ApplicationId;
            
            item.VcPrincipal = VcPrincipal;
            
            item.VcJsonOrdinal = VcJsonOrdinal;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,bool BActive,int ITimeoutMsecs,int? TShowId,string Name,string DisplayUrl,int IPicWidth,int IPicHeight,bool BCtrX,bool BCtrY,string EventVenue,string EventDate,string EventTitle,string EventHeads,string EventOpeners,string EventDescription,string TextCss,DateTime? DtStartDate,DateTime? DtEndDate,Guid ApplicationId,string VcPrincipal,string VcJsonOrdinal)
	    {
		    Kiosk item = new Kiosk();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.BActive = BActive;
				
			item.ITimeoutMsecs = ITimeoutMsecs;
				
			item.TShowId = TShowId;
				
			item.Name = Name;
				
			item.DisplayUrl = DisplayUrl;
				
			item.IPicWidth = IPicWidth;
				
			item.IPicHeight = IPicHeight;
				
			item.BCtrX = BCtrX;
				
			item.BCtrY = BCtrY;
				
			item.EventVenue = EventVenue;
				
			item.EventDate = EventDate;
				
			item.EventTitle = EventTitle;
				
			item.EventHeads = EventHeads;
				
			item.EventOpeners = EventOpeners;
				
			item.EventDescription = EventDescription;
				
			item.TextCss = TextCss;
				
			item.DtStartDate = DtStartDate;
				
			item.DtEndDate = DtEndDate;
				
			item.ApplicationId = ApplicationId;
				
			item.VcPrincipal = VcPrincipal;
				
			item.VcJsonOrdinal = VcJsonOrdinal;
				
	        item.Save(UserName);
	    }
    }
}
