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
    /// Controller class for VdShowGenre
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class VdShowGenreController
    {
        // Preload our schema..
        VdShowGenre thisSchemaLoad = new VdShowGenre();
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
        public VdShowGenreCollection FetchAll()
        {
            VdShowGenreCollection coll = new VdShowGenreCollection();
            Query qry = new Query(VdShowGenre.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VdShowGenreCollection FetchByID(object Id)
        {
            VdShowGenreCollection coll = new VdShowGenreCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public VdShowGenreCollection FetchByQuery(Query qry)
        {
            VdShowGenreCollection coll = new VdShowGenreCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (VdShowGenre.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (VdShowGenre.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TShowId,int? TParentGenreId,string GenreName,bool BIsMainGenre,int IOrdinal,DateTime DtStamp)
	    {
		    VdShowGenre item = new VdShowGenre();
		    
            item.TShowId = TShowId;
            
            item.TParentGenreId = TParentGenreId;
            
            item.GenreName = GenreName;
            
            item.BIsMainGenre = BIsMainGenre;
            
            item.IOrdinal = IOrdinal;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TShowId,int? TParentGenreId,string GenreName,bool BIsMainGenre,int IOrdinal,DateTime DtStamp)
	    {
		    VdShowGenre item = new VdShowGenre();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TShowId = TShowId;
				
			item.TParentGenreId = TParentGenreId;
				
			item.GenreName = GenreName;
				
			item.BIsMainGenre = BIsMainGenre;
				
			item.IOrdinal = IOrdinal;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
