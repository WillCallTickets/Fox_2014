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
    /// Controller class for VdShowTicket
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class VdShowTicketController
    {
        // Preload our schema..
        VdShowTicket thisSchemaLoad = new VdShowTicket();
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
        public VdShowTicketCollection FetchAll()
        {
            VdShowTicketCollection coll = new VdShowTicketCollection();
            Query qry = new Query(VdShowTicket.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VdShowTicketCollection FetchByID(object Id)
        {
            VdShowTicketCollection coll = new VdShowTicketCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public VdShowTicketCollection FetchByQuery(Query qry)
        {
            VdShowTicketCollection coll = new VdShowTicketCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (VdShowTicket.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (VdShowTicket.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TShowId,string TicketDescription,string TicketQualifier,bool BReserved,decimal MBasePrice,decimal MServiceCharge,string AdditionalDescription,decimal MAdditionalCharge,decimal? MEach,int IOrdinal,DateTime DtStamp)
	    {
		    VdShowTicket item = new VdShowTicket();
		    
            item.TShowId = TShowId;
            
            item.TicketDescription = TicketDescription;
            
            item.TicketQualifier = TicketQualifier;
            
            item.BReserved = BReserved;
            
            item.MBasePrice = MBasePrice;
            
            item.MServiceCharge = MServiceCharge;
            
            item.AdditionalDescription = AdditionalDescription;
            
            item.MAdditionalCharge = MAdditionalCharge;
            
            item.MEach = MEach;
            
            item.IOrdinal = IOrdinal;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TShowId,string TicketDescription,string TicketQualifier,bool BReserved,decimal MBasePrice,decimal MServiceCharge,string AdditionalDescription,decimal MAdditionalCharge,decimal? MEach,int IOrdinal,DateTime DtStamp)
	    {
		    VdShowTicket item = new VdShowTicket();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TShowId = TShowId;
				
			item.TicketDescription = TicketDescription;
				
			item.TicketQualifier = TicketQualifier;
				
			item.BReserved = BReserved;
				
			item.MBasePrice = MBasePrice;
				
			item.MServiceCharge = MServiceCharge;
				
			item.AdditionalDescription = AdditionalDescription;
				
			item.MAdditionalCharge = MAdditionalCharge;
				
			item.MEach = MEach;
				
			item.IOrdinal = IOrdinal;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
