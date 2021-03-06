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
    /// Controller class for ICalendar
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ICalendarController
    {
        // Preload our schema..
        ICalendar thisSchemaLoad = new ICalendar();
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
        public ICalendarCollection FetchAll()
        {
            ICalendarCollection coll = new ICalendarCollection();
            Query qry = new Query(ICalendar.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ICalendarCollection FetchByID(object Id)
        {
            ICalendarCollection coll = new ICalendarCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ICalendarCollection FetchByQuery(Query qry)
        {
            ICalendarCollection coll = new ICalendarCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ICalendar.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ICalendar.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,string UrlKey,string SerializedCalendar)
	    {
		    ICalendar item = new ICalendar();
		    
            item.DtStamp = DtStamp;
            
            item.UrlKey = UrlKey;
            
            item.SerializedCalendar = SerializedCalendar;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,string UrlKey,string SerializedCalendar)
	    {
		    ICalendar item = new ICalendar();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.UrlKey = UrlKey;
				
			item.SerializedCalendar = SerializedCalendar;
				
	        item.Save(UserName);
	    }
    }
}
