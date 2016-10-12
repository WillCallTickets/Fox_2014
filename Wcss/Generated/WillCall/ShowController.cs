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
    /// Controller class for Show
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ShowController
    {
        // Preload our schema..
        Show thisSchemaLoad = new Show();
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
        public ShowCollection FetchAll()
        {
            ShowCollection coll = new ShowCollection();
            Query qry = new Query(Show.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowCollection FetchByID(object Id)
        {
            ShowCollection coll = new ShowCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowCollection FetchByQuery(Query qry)
        {
            ShowCollection coll = new ShowCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Show.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Show.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,DateTime? DtAnnounceDate,DateTime? DtDateOnSale,bool BActive,bool BSoldOut,string ShowAlert,int TVenueId,string DisplayNotes,string ShowTitle,string DisplayUrl,int IPicWidth,int IPicHeight,string ShowHeader,string ShowWriteup,DateTime DtStamp,Guid ApplicationId,bool BCtrX,bool BCtrY,string FacebookEventUrl,string VcPrincipal,string VcJustAnnouncedStatus)
	    {
		    Show item = new Show();
		    
            item.Name = Name;
            
            item.DtAnnounceDate = DtAnnounceDate;
            
            item.DtDateOnSale = DtDateOnSale;
            
            item.BActive = BActive;
            
            item.BSoldOut = BSoldOut;
            
            item.ShowAlert = ShowAlert;
            
            item.TVenueId = TVenueId;
            
            item.DisplayNotes = DisplayNotes;
            
            item.ShowTitle = ShowTitle;
            
            item.DisplayUrl = DisplayUrl;
            
            item.IPicWidth = IPicWidth;
            
            item.IPicHeight = IPicHeight;
            
            item.ShowHeader = ShowHeader;
            
            item.ShowWriteup = ShowWriteup;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.BCtrX = BCtrX;
            
            item.BCtrY = BCtrY;
            
            item.FacebookEventUrl = FacebookEventUrl;
            
            item.VcPrincipal = VcPrincipal;
            
            item.VcJustAnnouncedStatus = VcJustAnnouncedStatus;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Name,DateTime? DtAnnounceDate,DateTime? DtDateOnSale,bool BActive,bool BSoldOut,string ShowAlert,int TVenueId,string DisplayNotes,string ShowTitle,string DisplayUrl,int IPicWidth,int IPicHeight,string ShowHeader,string ShowWriteup,DateTime DtStamp,Guid ApplicationId,bool BCtrX,bool BCtrY,string FacebookEventUrl,string VcPrincipal,string VcJustAnnouncedStatus)
	    {
		    Show item = new Show();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Name = Name;
				
			item.DtAnnounceDate = DtAnnounceDate;
				
			item.DtDateOnSale = DtDateOnSale;
				
			item.BActive = BActive;
				
			item.BSoldOut = BSoldOut;
				
			item.ShowAlert = ShowAlert;
				
			item.TVenueId = TVenueId;
				
			item.DisplayNotes = DisplayNotes;
				
			item.ShowTitle = ShowTitle;
				
			item.DisplayUrl = DisplayUrl;
				
			item.IPicWidth = IPicWidth;
				
			item.IPicHeight = IPicHeight;
				
			item.ShowHeader = ShowHeader;
				
			item.ShowWriteup = ShowWriteup;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.BCtrX = BCtrX;
				
			item.BCtrY = BCtrY;
				
			item.FacebookEventUrl = FacebookEventUrl;
				
			item.VcPrincipal = VcPrincipal;
				
			item.VcJustAnnouncedStatus = VcJustAnnouncedStatus;
				
	        item.Save(UserName);
	    }
    }
}
