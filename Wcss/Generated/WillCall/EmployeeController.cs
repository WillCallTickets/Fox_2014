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
    /// Controller class for Employee
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EmployeeController
    {
        // Preload our schema..
        Employee thisSchemaLoad = new Employee();
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
        public EmployeeCollection FetchAll()
        {
            EmployeeCollection coll = new EmployeeCollection();
            Query qry = new Query(Employee.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EmployeeCollection FetchByID(object Id)
        {
            EmployeeCollection coll = new EmployeeCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EmployeeCollection FetchByQuery(Query qry)
        {
            EmployeeCollection coll = new EmployeeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Employee.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Employee.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Login,string EmailAddress,string EPassword,string Dept,string TitleDescription,string Title,string FirstName,string Mi,string LastName,string Extension,bool BListInDirectory,DateTime DtStamp,Guid ApplicationId,string VcPrincipal,string VcJsonOrdinal)
	    {
		    Employee item = new Employee();
		    
            item.Login = Login;
            
            item.EmailAddress = EmailAddress;
            
            item.EPassword = EPassword;
            
            item.Dept = Dept;
            
            item.TitleDescription = TitleDescription;
            
            item.Title = Title;
            
            item.FirstName = FirstName;
            
            item.Mi = Mi;
            
            item.LastName = LastName;
            
            item.Extension = Extension;
            
            item.BListInDirectory = BListInDirectory;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.VcPrincipal = VcPrincipal;
            
            item.VcJsonOrdinal = VcJsonOrdinal;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Login,string EmailAddress,string EPassword,string Dept,string TitleDescription,string Title,string FirstName,string Mi,string LastName,string Extension,bool BListInDirectory,DateTime DtStamp,Guid ApplicationId,string VcPrincipal,string VcJsonOrdinal)
	    {
		    Employee item = new Employee();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Login = Login;
				
			item.EmailAddress = EmailAddress;
				
			item.EPassword = EPassword;
				
			item.Dept = Dept;
				
			item.TitleDescription = TitleDescription;
				
			item.Title = Title;
				
			item.FirstName = FirstName;
				
			item.Mi = Mi;
				
			item.LastName = LastName;
				
			item.Extension = Extension;
				
			item.BListInDirectory = BListInDirectory;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.VcPrincipal = VcPrincipal;
				
			item.VcJsonOrdinal = VcJsonOrdinal;
				
	        item.Save(UserName);
	    }
    }
}
