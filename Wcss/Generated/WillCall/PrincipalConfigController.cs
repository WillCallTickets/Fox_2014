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
    /// Controller class for PrincipalConfig
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PrincipalConfigController
    {
        // Preload our schema..
        PrincipalConfig thisSchemaLoad = new PrincipalConfig();
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
        public PrincipalConfigCollection FetchAll()
        {
            PrincipalConfigCollection coll = new PrincipalConfigCollection();
            Query qry = new Query(PrincipalConfig.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PrincipalConfigCollection FetchByID(object Id)
        {
            PrincipalConfigCollection coll = new PrincipalConfigCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PrincipalConfigCollection FetchByQuery(Query qry)
        {
            PrincipalConfigCollection coll = new PrincipalConfigCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (PrincipalConfig.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (PrincipalConfig.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string VcPrincipal,string DataType,int MaxLength,string Context,string Description,string Name,string ValueX,DateTime DtStamp,DateTime DtModified)
	    {
		    PrincipalConfig item = new PrincipalConfig();
		    
            item.VcPrincipal = VcPrincipal;
            
            item.DataType = DataType;
            
            item.MaxLength = MaxLength;
            
            item.Context = Context;
            
            item.Description = Description;
            
            item.Name = Name;
            
            item.ValueX = ValueX;
            
            item.DtStamp = DtStamp;
            
            item.DtModified = DtModified;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string VcPrincipal,string DataType,int MaxLength,string Context,string Description,string Name,string ValueX,DateTime DtStamp,DateTime DtModified)
	    {
		    PrincipalConfig item = new PrincipalConfig();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.VcPrincipal = VcPrincipal;
				
			item.DataType = DataType;
				
			item.MaxLength = MaxLength;
				
			item.Context = Context;
				
			item.Description = Description;
				
			item.Name = Name;
				
			item.ValueX = ValueX;
				
			item.DtStamp = DtStamp;
				
			item.DtModified = DtModified;
				
	        item.Save(UserName);
	    }
    }
}
