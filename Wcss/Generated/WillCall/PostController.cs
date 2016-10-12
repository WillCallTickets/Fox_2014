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
    /// Controller class for Post
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PostController
    {
        // Preload our schema..
        Post thisSchemaLoad = new Post();
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
        public PostCollection FetchAll()
        {
            PostCollection coll = new PostCollection();
            Query qry = new Query(Post.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PostCollection FetchByID(object Id)
        {
            PostCollection coll = new PostCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PostCollection FetchByQuery(Query qry)
        {
            PostCollection coll = new PostCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Post.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Post.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string VcPrincipal,bool BActive,string Name,string Slug,string Description,string ValueX,DateTime DtStamp,DateTime DtModified,string VcJsonOrdinal)
	    {
		    Post item = new Post();
		    
            item.VcPrincipal = VcPrincipal;
            
            item.BActive = BActive;
            
            item.Name = Name;
            
            item.Slug = Slug;
            
            item.Description = Description;
            
            item.ValueX = ValueX;
            
            item.DtStamp = DtStamp;
            
            item.DtModified = DtModified;
            
            item.VcJsonOrdinal = VcJsonOrdinal;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string VcPrincipal,bool BActive,string Name,string Slug,string Description,string ValueX,DateTime DtStamp,DateTime DtModified,string VcJsonOrdinal)
	    {
		    Post item = new Post();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.VcPrincipal = VcPrincipal;
				
			item.BActive = BActive;
				
			item.Name = Name;
				
			item.Slug = Slug;
				
			item.Description = Description;
				
			item.ValueX = ValueX;
				
			item.DtStamp = DtStamp;
				
			item.DtModified = DtModified;
				
			item.VcJsonOrdinal = VcJsonOrdinal;
				
	        item.Save(UserName);
	    }
    }
}
