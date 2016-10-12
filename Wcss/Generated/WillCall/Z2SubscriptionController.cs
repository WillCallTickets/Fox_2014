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
    /// Controller class for Z2Subscription
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class Z2SubscriptionController
    {
        // Preload our schema..
        Z2Subscription thisSchemaLoad = new Z2Subscription();
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
        public Z2SubscriptionCollection FetchAll()
        {
            Z2SubscriptionCollection coll = new Z2SubscriptionCollection();
            Query qry = new Query(Z2Subscription.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Z2SubscriptionCollection FetchByID(object Id)
        {
            Z2SubscriptionCollection coll = new Z2SubscriptionCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public Z2SubscriptionCollection FetchByQuery(Query qry)
        {
            Z2SubscriptionCollection coll = new Z2SubscriptionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Z2Subscription.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Z2Subscription.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtCreated,DateTime? DtModified,string Email,string IpAddress,bool BSubscribed,int? TZ2SubscriptionHistoryId,string InitialSourceQuery)
	    {
		    Z2Subscription item = new Z2Subscription();
		    
            item.DtCreated = DtCreated;
            
            item.DtModified = DtModified;
            
            item.Email = Email;
            
            item.IpAddress = IpAddress;
            
            item.BSubscribed = BSubscribed;
            
            item.TZ2SubscriptionHistoryId = TZ2SubscriptionHistoryId;
            
            item.InitialSourceQuery = InitialSourceQuery;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtCreated,DateTime? DtModified,string Email,string IpAddress,bool BSubscribed,int? TZ2SubscriptionHistoryId,string InitialSourceQuery)
	    {
		    Z2Subscription item = new Z2Subscription();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtCreated = DtCreated;
				
			item.DtModified = DtModified;
				
			item.Email = Email;
				
			item.IpAddress = IpAddress;
				
			item.BSubscribed = BSubscribed;
				
			item.TZ2SubscriptionHistoryId = TZ2SubscriptionHistoryId;
				
			item.InitialSourceQuery = InitialSourceQuery;
				
	        item.Save(UserName);
	    }
    }
}
