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
// <auto-generated />
namespace Wcss
{
    /// <summary>
    /// Controller class for VdShowInfo
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class VdShowInfoController
    {
        // Preload our schema..
        VdShowInfo thisSchemaLoad = new VdShowInfo();
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
        public VdShowInfoCollection FetchAll()
        {
            VdShowInfoCollection coll = new VdShowInfoCollection();
            Query qry = new Query(VdShowInfo.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VdShowInfoCollection FetchByID(object Id)
        {
            VdShowInfoCollection coll = new VdShowInfoCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public VdShowInfoCollection FetchByQuery(Query qry)
        {
            VdShowInfoCollection coll = new VdShowInfoCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (VdShowInfo.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (VdShowInfo.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TShowId,DateTime DtStamp,DateTime DtModified,string Agent,string Buyer,decimal? MTicketGross,int? ITicketsSold,int? ICompsIn,decimal? MFacilityFee,decimal? MConcessions,decimal? MBarTotal,decimal? MBarPerHead,int? IMarketingDays,string Mod,decimal? MProdLabor,decimal? MSecurityLabor,decimal? MHospitality,int? IMarketPlays,string Notes)
	    {
		    VdShowInfo item = new VdShowInfo();
		    
            item.TShowId = TShowId;
            
            item.DtStamp = DtStamp;
            
            item.DtModified = DtModified;
            
            item.Agent = Agent;
            
            item.Buyer = Buyer;
            
            item.MTicketGross = MTicketGross;
            
            item.ITicketsSold = ITicketsSold;
            
            item.ICompsIn = ICompsIn;
            
            item.MFacilityFee = MFacilityFee;
            
            item.MConcessions = MConcessions;
            
            item.MBarTotal = MBarTotal;
            
            item.MBarPerHead = MBarPerHead;
            
            item.IMarketingDays = IMarketingDays;
            
            item.Mod = Mod;
            
            item.MProdLabor = MProdLabor;
            
            item.MSecurityLabor = MSecurityLabor;
            
            item.MHospitality = MHospitality;
            
            item.IMarketPlays = IMarketPlays;
            
            item.Notes = Notes;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TShowId,DateTime DtStamp,DateTime DtModified,string Agent,string Buyer,decimal? MTicketGross,int? ITicketsSold,int? ICompsIn,decimal? MFacilityFee,decimal? MConcessions,decimal? MBarTotal,decimal? MBarPerHead,int? IMarketingDays,string Mod,decimal? MProdLabor,decimal? MSecurityLabor,decimal? MHospitality,int? IMarketPlays,string Notes)
	    {
		    VdShowInfo item = new VdShowInfo();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TShowId = TShowId;
				
			item.DtStamp = DtStamp;
				
			item.DtModified = DtModified;
				
			item.Agent = Agent;
				
			item.Buyer = Buyer;
				
			item.MTicketGross = MTicketGross;
				
			item.ITicketsSold = ITicketsSold;
				
			item.ICompsIn = ICompsIn;
				
			item.MFacilityFee = MFacilityFee;
				
			item.MConcessions = MConcessions;
				
			item.MBarTotal = MBarTotal;
				
			item.MBarPerHead = MBarPerHead;
				
			item.IMarketingDays = IMarketingDays;
				
			item.Mod = Mod;
				
			item.MProdLabor = MProdLabor;
				
			item.MSecurityLabor = MSecurityLabor;
				
			item.MHospitality = MHospitality;
				
			item.IMarketPlays = IMarketPlays;
				
			item.Notes = Notes;
				
	        item.Save(UserName);
	    }
    }
}
