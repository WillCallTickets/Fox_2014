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
    /// Controller class for Z2SubscriptionRequest
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class Z2SubscriptionRequestController
    {
        // Preload our schema..
        Z2SubscriptionRequest thisSchemaLoad = new Z2SubscriptionRequest();
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
        public Z2SubscriptionRequestCollection FetchAll()
        {
            Z2SubscriptionRequestCollection coll = new Z2SubscriptionRequestCollection();
            Query qry = new Query(Z2SubscriptionRequest.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Z2SubscriptionRequestCollection FetchByID(object Id)
        {
            Z2SubscriptionRequestCollection coll = new Z2SubscriptionRequestCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public Z2SubscriptionRequestCollection FetchByQuery(Query qry)
        {
            Z2SubscriptionRequestCollection coll = new Z2SubscriptionRequestCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Z2SubscriptionRequest.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Z2SubscriptionRequest.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TZ2SubscriptionId,string Source,string SubscriptionRequest,string IpAddress)
	    {
		    Z2SubscriptionRequest item = new Z2SubscriptionRequest();
		    
            item.DtStamp = DtStamp;
            
            item.TZ2SubscriptionId = TZ2SubscriptionId;
            
            item.Source = Source;
            
            item.SubscriptionRequest = SubscriptionRequest;
            
            item.IpAddress = IpAddress;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TZ2SubscriptionId,string Source,string SubscriptionRequest,string IpAddress)
	    {
		    Z2SubscriptionRequest item = new Z2SubscriptionRequest();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TZ2SubscriptionId = TZ2SubscriptionId;
				
			item.Source = Source;
				
			item.SubscriptionRequest = SubscriptionRequest;
				
			item.IpAddress = IpAddress;
				
	        item.Save(UserName);
	    }
    }
}
