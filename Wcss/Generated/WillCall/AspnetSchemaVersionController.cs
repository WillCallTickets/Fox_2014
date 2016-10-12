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
    /// Controller class for aspnet_SchemaVersions
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AspnetSchemaVersionController
    {
        // Preload our schema..
        AspnetSchemaVersion thisSchemaLoad = new AspnetSchemaVersion();
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
        public AspnetSchemaVersionCollection FetchAll()
        {
            AspnetSchemaVersionCollection coll = new AspnetSchemaVersionCollection();
            Query qry = new Query(AspnetSchemaVersion.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetSchemaVersionCollection FetchByID(object Feature)
        {
            AspnetSchemaVersionCollection coll = new AspnetSchemaVersionCollection().Where("Feature", Feature).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetSchemaVersionCollection FetchByQuery(Query qry)
        {
            AspnetSchemaVersionCollection coll = new AspnetSchemaVersionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Feature)
        {
            return (AspnetSchemaVersion.Delete(Feature) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Feature)
        {
            return (AspnetSchemaVersion.Destroy(Feature) == 1);
        }
        
        
        
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(string Feature,string CompatibleSchemaVersion)
        {
            Query qry = new Query(AspnetSchemaVersion.Schema);
            qry.QueryType = QueryType.Delete;
            qry.AddWhere("Feature", Feature).AND("CompatibleSchemaVersion", CompatibleSchemaVersion);
            qry.Execute();
            return (true);
        }        
       
    	
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Feature,string CompatibleSchemaVersion,bool IsCurrentVersion)
	    {
		    AspnetSchemaVersion item = new AspnetSchemaVersion();
		    
            item.Feature = Feature;
            
            item.CompatibleSchemaVersion = CompatibleSchemaVersion;
            
            item.IsCurrentVersion = IsCurrentVersion;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string Feature,string CompatibleSchemaVersion,bool IsCurrentVersion)
	    {
		    AspnetSchemaVersion item = new AspnetSchemaVersion();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Feature = Feature;
				
			item.CompatibleSchemaVersion = CompatibleSchemaVersion;
				
			item.IsCurrentVersion = IsCurrentVersion;
				
	        item.Save(UserName);
	    }
    }
}
