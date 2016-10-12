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
    /// Controller class for Z2SubscriptionTransfer
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class Z2SubscriptionTransferController
    {
        // Preload our schema..
        Z2SubscriptionTransfer thisSchemaLoad = new Z2SubscriptionTransfer();
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
        public Z2SubscriptionTransferCollection FetchAll()
        {
            Z2SubscriptionTransferCollection coll = new Z2SubscriptionTransferCollection();
            Query qry = new Query(Z2SubscriptionTransfer.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Z2SubscriptionTransferCollection FetchByID(object Id)
        {
            Z2SubscriptionTransferCollection coll = new Z2SubscriptionTransferCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public Z2SubscriptionTransferCollection FetchByQuery(Query qry)
        {
            Z2SubscriptionTransferCollection coll = new Z2SubscriptionTransferCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Z2SubscriptionTransfer.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Z2SubscriptionTransfer.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TZ2SubscriptionId,string Email,string ListSource,DateTime? DtTransferred,bool? TransferSubscribedStatus,DateTime? DtSourceListUpdated)
	    {
		    Z2SubscriptionTransfer item = new Z2SubscriptionTransfer();
		    
            item.DtStamp = DtStamp;
            
            item.TZ2SubscriptionId = TZ2SubscriptionId;
            
            item.Email = Email;
            
            item.ListSource = ListSource;
            
            item.DtTransferred = DtTransferred;
            
            item.TransferSubscribedStatus = TransferSubscribedStatus;
            
            item.DtSourceListUpdated = DtSourceListUpdated;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TZ2SubscriptionId,string Email,string ListSource,DateTime? DtTransferred,bool? TransferSubscribedStatus,DateTime? DtSourceListUpdated)
	    {
		    Z2SubscriptionTransfer item = new Z2SubscriptionTransfer();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TZ2SubscriptionId = TZ2SubscriptionId;
				
			item.Email = Email;
				
			item.ListSource = ListSource;
				
			item.DtTransferred = DtTransferred;
				
			item.TransferSubscribedStatus = TransferSubscribedStatus;
				
			item.DtSourceListUpdated = DtSourceListUpdated;
				
	        item.Save(UserName);
	    }
    }
}
