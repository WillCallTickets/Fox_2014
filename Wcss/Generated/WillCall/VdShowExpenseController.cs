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
    /// Controller class for VdShowExpense
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class VdShowExpenseController
    {
        // Preload our schema..
        VdShowExpense thisSchemaLoad = new VdShowExpense();
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
        public VdShowExpenseCollection FetchAll()
        {
            VdShowExpenseCollection coll = new VdShowExpenseCollection();
            Query qry = new Query(VdShowExpense.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VdShowExpenseCollection FetchByID(object Id)
        {
            VdShowExpenseCollection coll = new VdShowExpenseCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public VdShowExpenseCollection FetchByQuery(Query qry)
        {
            VdShowExpenseCollection coll = new VdShowExpenseCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (VdShowExpense.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (VdShowExpense.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TShowId,DateTime? DtIncurred,string ExpenseCategory,string ExpenseName,string Notes,decimal MAmount,int IOrdinal,DateTime DtStamp)
	    {
		    VdShowExpense item = new VdShowExpense();
		    
            item.TShowId = TShowId;
            
            item.DtIncurred = DtIncurred;
            
            item.ExpenseCategory = ExpenseCategory;
            
            item.ExpenseName = ExpenseName;
            
            item.Notes = Notes;
            
            item.MAmount = MAmount;
            
            item.IOrdinal = IOrdinal;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TShowId,DateTime? DtIncurred,string ExpenseCategory,string ExpenseName,string Notes,decimal MAmount,int IOrdinal,DateTime DtStamp)
	    {
		    VdShowExpense item = new VdShowExpense();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TShowId = TShowId;
				
			item.DtIncurred = DtIncurred;
				
			item.ExpenseCategory = ExpenseCategory;
				
			item.ExpenseName = ExpenseName;
				
			item.Notes = Notes;
				
			item.MAmount = MAmount;
				
			item.IOrdinal = IOrdinal;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
