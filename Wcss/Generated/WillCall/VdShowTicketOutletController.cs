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
    /// Controller class for VdShowTicketOutlet
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class VdShowTicketOutletController
    {
        // Preload our schema..
        VdShowTicketOutlet thisSchemaLoad = new VdShowTicketOutlet();
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
        public VdShowTicketOutletCollection FetchAll()
        {
            VdShowTicketOutletCollection coll = new VdShowTicketOutletCollection();
            Query qry = new Query(VdShowTicketOutlet.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VdShowTicketOutletCollection FetchByID(object Id)
        {
            VdShowTicketOutletCollection coll = new VdShowTicketOutletCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public VdShowTicketOutletCollection FetchByQuery(Query qry)
        {
            VdShowTicketOutletCollection coll = new VdShowTicketOutletCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (VdShowTicketOutlet.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (VdShowTicketOutlet.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TShowId,int TVdShowTicketId,string OutletName,int IAllotment,int ISold,int IOrdinal,DateTime DtStamp)
	    {
		    VdShowTicketOutlet item = new VdShowTicketOutlet();
		    
            item.TShowId = TShowId;
            
            item.TVdShowTicketId = TVdShowTicketId;
            
            item.OutletName = OutletName;
            
            item.IAllotment = IAllotment;
            
            item.ISold = ISold;
            
            item.IOrdinal = IOrdinal;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TShowId,int TVdShowTicketId,string OutletName,int IAllotment,int ISold,int IOrdinal,DateTime DtStamp)
	    {
		    VdShowTicketOutlet item = new VdShowTicketOutlet();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TShowId = TShowId;
				
			item.TVdShowTicketId = TVdShowTicketId;
				
			item.OutletName = OutletName;
				
			item.IAllotment = IAllotment;
				
			item.ISold = ISold;
				
			item.IOrdinal = IOrdinal;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
