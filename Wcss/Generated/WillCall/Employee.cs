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
	/// Strongly-typed collection for the Employee class.
	/// </summary>
    [Serializable]
	public partial class EmployeeCollection : ActiveList<Employee, EmployeeCollection>
	{	   
		public EmployeeCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>EmployeeCollection</returns>
		public EmployeeCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Employee o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
		
		
	}
	/// <summary>
	/// This is an ActiveRecord class which wraps the Employee table.
	/// </summary>
	[Serializable]
	public partial class Employee : ActiveRecord<Employee>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Employee()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Employee(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Employee(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Employee(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
		}
		
		protected static void SetSQLProps() { GetTableSchema(); }
		
		#endregion
		
		#region Schema and Query Accessor	
		public static Query CreateQuery() { return new Query(Schema); }
		public static TableSchema.Table Schema
		{
			get
			{
				if (BaseSchema == null)
					SetSQLProps();
				return BaseSchema;
			}
		}
		
		private static void GetTableSchema() 
		{
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("Employee", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "Id";
				colvarId.DataType = DbType.Int32;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = true;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = true;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				colvarId.DefaultSetting = @"";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarLogin = new TableSchema.TableColumn(schema);
				colvarLogin.ColumnName = "Login";
				colvarLogin.DataType = DbType.AnsiString;
				colvarLogin.MaxLength = 256;
				colvarLogin.AutoIncrement = false;
				colvarLogin.IsNullable = true;
				colvarLogin.IsPrimaryKey = false;
				colvarLogin.IsForeignKey = false;
				colvarLogin.IsReadOnly = false;
				colvarLogin.DefaultSetting = @"";
				colvarLogin.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLogin);
				
				TableSchema.TableColumn colvarEmailAddress = new TableSchema.TableColumn(schema);
				colvarEmailAddress.ColumnName = "EmailAddress";
				colvarEmailAddress.DataType = DbType.AnsiString;
				colvarEmailAddress.MaxLength = 256;
				colvarEmailAddress.AutoIncrement = false;
				colvarEmailAddress.IsNullable = false;
				colvarEmailAddress.IsPrimaryKey = false;
				colvarEmailAddress.IsForeignKey = false;
				colvarEmailAddress.IsReadOnly = false;
				colvarEmailAddress.DefaultSetting = @"";
				colvarEmailAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEmailAddress);
				
				TableSchema.TableColumn colvarEPassword = new TableSchema.TableColumn(schema);
				colvarEPassword.ColumnName = "ePassword";
				colvarEPassword.DataType = DbType.AnsiString;
				colvarEPassword.MaxLength = 256;
				colvarEPassword.AutoIncrement = false;
				colvarEPassword.IsNullable = true;
				colvarEPassword.IsPrimaryKey = false;
				colvarEPassword.IsForeignKey = false;
				colvarEPassword.IsReadOnly = false;
				colvarEPassword.DefaultSetting = @"";
				colvarEPassword.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEPassword);
				
				TableSchema.TableColumn colvarDept = new TableSchema.TableColumn(schema);
				colvarDept.ColumnName = "Dept";
				colvarDept.DataType = DbType.AnsiString;
				colvarDept.MaxLength = 50;
				colvarDept.AutoIncrement = false;
				colvarDept.IsNullable = true;
				colvarDept.IsPrimaryKey = false;
				colvarDept.IsForeignKey = false;
				colvarDept.IsReadOnly = false;
				colvarDept.DefaultSetting = @"";
				colvarDept.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDept);
				
				TableSchema.TableColumn colvarTitleDescription = new TableSchema.TableColumn(schema);
				colvarTitleDescription.ColumnName = "TitleDescription";
				colvarTitleDescription.DataType = DbType.AnsiString;
				colvarTitleDescription.MaxLength = 75;
				colvarTitleDescription.AutoIncrement = false;
				colvarTitleDescription.IsNullable = true;
				colvarTitleDescription.IsPrimaryKey = false;
				colvarTitleDescription.IsForeignKey = false;
				colvarTitleDescription.IsReadOnly = false;
				colvarTitleDescription.DefaultSetting = @"";
				colvarTitleDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTitleDescription);
				
				TableSchema.TableColumn colvarTitle = new TableSchema.TableColumn(schema);
				colvarTitle.ColumnName = "Title";
				colvarTitle.DataType = DbType.AnsiString;
				colvarTitle.MaxLength = 50;
				colvarTitle.AutoIncrement = false;
				colvarTitle.IsNullable = true;
				colvarTitle.IsPrimaryKey = false;
				colvarTitle.IsForeignKey = false;
				colvarTitle.IsReadOnly = false;
				colvarTitle.DefaultSetting = @"";
				colvarTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTitle);
				
				TableSchema.TableColumn colvarFirstName = new TableSchema.TableColumn(schema);
				colvarFirstName.ColumnName = "FirstName";
				colvarFirstName.DataType = DbType.AnsiString;
				colvarFirstName.MaxLength = 50;
				colvarFirstName.AutoIncrement = false;
				colvarFirstName.IsNullable = true;
				colvarFirstName.IsPrimaryKey = false;
				colvarFirstName.IsForeignKey = false;
				colvarFirstName.IsReadOnly = false;
				colvarFirstName.DefaultSetting = @"";
				colvarFirstName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFirstName);
				
				TableSchema.TableColumn colvarMi = new TableSchema.TableColumn(schema);
				colvarMi.ColumnName = "MI";
				colvarMi.DataType = DbType.AnsiString;
				colvarMi.MaxLength = 20;
				colvarMi.AutoIncrement = false;
				colvarMi.IsNullable = true;
				colvarMi.IsPrimaryKey = false;
				colvarMi.IsForeignKey = false;
				colvarMi.IsReadOnly = false;
				colvarMi.DefaultSetting = @"";
				colvarMi.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMi);
				
				TableSchema.TableColumn colvarLastName = new TableSchema.TableColumn(schema);
				colvarLastName.ColumnName = "LastName";
				colvarLastName.DataType = DbType.AnsiString;
				colvarLastName.MaxLength = 100;
				colvarLastName.AutoIncrement = false;
				colvarLastName.IsNullable = true;
				colvarLastName.IsPrimaryKey = false;
				colvarLastName.IsForeignKey = false;
				colvarLastName.IsReadOnly = false;
				colvarLastName.DefaultSetting = @"";
				colvarLastName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastName);
				
				TableSchema.TableColumn colvarExtension = new TableSchema.TableColumn(schema);
				colvarExtension.ColumnName = "Extension";
				colvarExtension.DataType = DbType.AnsiString;
				colvarExtension.MaxLength = 10;
				colvarExtension.AutoIncrement = false;
				colvarExtension.IsNullable = true;
				colvarExtension.IsPrimaryKey = false;
				colvarExtension.IsForeignKey = false;
				colvarExtension.IsReadOnly = false;
				colvarExtension.DefaultSetting = @"";
				colvarExtension.ForeignKeyTableName = "";
				schema.Columns.Add(colvarExtension);
				
				TableSchema.TableColumn colvarBListInDirectory = new TableSchema.TableColumn(schema);
				colvarBListInDirectory.ColumnName = "bListInDirectory";
				colvarBListInDirectory.DataType = DbType.Boolean;
				colvarBListInDirectory.MaxLength = 0;
				colvarBListInDirectory.AutoIncrement = false;
				colvarBListInDirectory.IsNullable = false;
				colvarBListInDirectory.IsPrimaryKey = false;
				colvarBListInDirectory.IsForeignKey = false;
				colvarBListInDirectory.IsReadOnly = false;
				
						colvarBListInDirectory.DefaultSetting = @"((1))";
				colvarBListInDirectory.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBListInDirectory);
				
				TableSchema.TableColumn colvarDtStamp = new TableSchema.TableColumn(schema);
				colvarDtStamp.ColumnName = "dtStamp";
				colvarDtStamp.DataType = DbType.DateTime;
				colvarDtStamp.MaxLength = 0;
				colvarDtStamp.AutoIncrement = false;
				colvarDtStamp.IsNullable = false;
				colvarDtStamp.IsPrimaryKey = false;
				colvarDtStamp.IsForeignKey = false;
				colvarDtStamp.IsReadOnly = false;
				
						colvarDtStamp.DefaultSetting = @"(getdate())";
				colvarDtStamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStamp);
				
				TableSchema.TableColumn colvarApplicationId = new TableSchema.TableColumn(schema);
				colvarApplicationId.ColumnName = "ApplicationId";
				colvarApplicationId.DataType = DbType.Guid;
				colvarApplicationId.MaxLength = 0;
				colvarApplicationId.AutoIncrement = false;
				colvarApplicationId.IsNullable = false;
				colvarApplicationId.IsPrimaryKey = false;
				colvarApplicationId.IsForeignKey = true;
				colvarApplicationId.IsReadOnly = false;
				colvarApplicationId.DefaultSetting = @"";
				
					colvarApplicationId.ForeignKeyTableName = "aspnet_Applications";
				schema.Columns.Add(colvarApplicationId);
				
				TableSchema.TableColumn colvarVcPrincipal = new TableSchema.TableColumn(schema);
				colvarVcPrincipal.ColumnName = "vcPrincipal";
				colvarVcPrincipal.DataType = DbType.AnsiString;
				colvarVcPrincipal.MaxLength = 100;
				colvarVcPrincipal.AutoIncrement = false;
				colvarVcPrincipal.IsNullable = false;
				colvarVcPrincipal.IsPrimaryKey = false;
				colvarVcPrincipal.IsForeignKey = false;
				colvarVcPrincipal.IsReadOnly = false;
				
						colvarVcPrincipal.DefaultSetting = @"('fox')";
				colvarVcPrincipal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcPrincipal);
				
				TableSchema.TableColumn colvarVcJsonOrdinal = new TableSchema.TableColumn(schema);
				colvarVcJsonOrdinal.ColumnName = "vcJsonOrdinal";
				colvarVcJsonOrdinal.DataType = DbType.AnsiString;
				colvarVcJsonOrdinal.MaxLength = 100;
				colvarVcJsonOrdinal.AutoIncrement = false;
				colvarVcJsonOrdinal.IsNullable = false;
				colvarVcJsonOrdinal.IsPrimaryKey = false;
				colvarVcJsonOrdinal.IsForeignKey = false;
				colvarVcJsonOrdinal.IsReadOnly = false;
				
						colvarVcJsonOrdinal.DefaultSetting = @"('')";
				colvarVcJsonOrdinal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcJsonOrdinal);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Employee",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Id")]
		[Bindable(true)]
		public int Id 
		{
			get { return GetColumnValue<int>(Columns.Id); }
			set { SetColumnValue(Columns.Id, value); }
		}
		  
		[XmlAttribute("Login")]
		[Bindable(true)]
		public string Login 
		{
			get { return GetColumnValue<string>(Columns.Login); }
			set { SetColumnValue(Columns.Login, value); }
		}
		  
		[XmlAttribute("EmailAddress")]
		[Bindable(true)]
		public string EmailAddress 
		{
			get { return GetColumnValue<string>(Columns.EmailAddress); }
			set { SetColumnValue(Columns.EmailAddress, value); }
		}
		  
		[XmlAttribute("EPassword")]
		[Bindable(true)]
		public string EPassword 
		{
			get { return GetColumnValue<string>(Columns.EPassword); }
			set { SetColumnValue(Columns.EPassword, value); }
		}
		  
		[XmlAttribute("Dept")]
		[Bindable(true)]
		public string Dept 
		{
			get { return GetColumnValue<string>(Columns.Dept); }
			set { SetColumnValue(Columns.Dept, value); }
		}
		  
		[XmlAttribute("TitleDescription")]
		[Bindable(true)]
		public string TitleDescription 
		{
			get { return GetColumnValue<string>(Columns.TitleDescription); }
			set { SetColumnValue(Columns.TitleDescription, value); }
		}
		  
		[XmlAttribute("Title")]
		[Bindable(true)]
		public string Title 
		{
			get { return GetColumnValue<string>(Columns.Title); }
			set { SetColumnValue(Columns.Title, value); }
		}
		  
		[XmlAttribute("FirstName")]
		[Bindable(true)]
		public string FirstName 
		{
			get { return GetColumnValue<string>(Columns.FirstName); }
			set { SetColumnValue(Columns.FirstName, value); }
		}
		  
		[XmlAttribute("Mi")]
		[Bindable(true)]
		public string Mi 
		{
			get { return GetColumnValue<string>(Columns.Mi); }
			set { SetColumnValue(Columns.Mi, value); }
		}
		  
		[XmlAttribute("LastName")]
		[Bindable(true)]
		public string LastName 
		{
			get { return GetColumnValue<string>(Columns.LastName); }
			set { SetColumnValue(Columns.LastName, value); }
		}
		  
		[XmlAttribute("Extension")]
		[Bindable(true)]
		public string Extension 
		{
			get { return GetColumnValue<string>(Columns.Extension); }
			set { SetColumnValue(Columns.Extension, value); }
		}
		  
		[XmlAttribute("BListInDirectory")]
		[Bindable(true)]
		public bool BListInDirectory 
		{
			get { return GetColumnValue<bool>(Columns.BListInDirectory); }
			set { SetColumnValue(Columns.BListInDirectory, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("VcPrincipal")]
		[Bindable(true)]
		public string VcPrincipal 
		{
			get { return GetColumnValue<string>(Columns.VcPrincipal); }
			set { SetColumnValue(Columns.VcPrincipal, value); }
		}
		  
		[XmlAttribute("VcJsonOrdinal")]
		[Bindable(true)]
		public string VcJsonOrdinal 
		{
			get { return GetColumnValue<string>(Columns.VcJsonOrdinal); }
			set { SetColumnValue(Columns.VcJsonOrdinal, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Employee
		/// 
		/// </summary>
		private Wcss.AspnetApplication AspnetApplication
		{
			get { return Wcss.AspnetApplication.FetchByID(this.ApplicationId); }
			set { SetColumnValue("ApplicationId", value.ApplicationId); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.AspnetApplication _aspnetapplicationrecord = null;
		
		public Wcss.AspnetApplication AspnetApplicationRecord
		{
		    get
            {
                if (_aspnetapplicationrecord == null)
                {
                    _aspnetapplicationrecord = new Wcss.AspnetApplication();
                    _aspnetapplicationrecord.CopyFrom(this.AspnetApplication);
                }
                return _aspnetapplicationrecord;
            }
            set
            {
                if(value != null && _aspnetapplicationrecord == null)
			        _aspnetapplicationrecord = new Wcss.AspnetApplication();
                
                SetColumnValue("ApplicationId", value.ApplicationId);
                _aspnetapplicationrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varLogin,string varEmailAddress,string varEPassword,string varDept,string varTitleDescription,string varTitle,string varFirstName,string varMi,string varLastName,string varExtension,bool varBListInDirectory,DateTime varDtStamp,Guid varApplicationId,string varVcPrincipal,string varVcJsonOrdinal)
		{
			Employee item = new Employee();
			
			item.Login = varLogin;
			
			item.EmailAddress = varEmailAddress;
			
			item.EPassword = varEPassword;
			
			item.Dept = varDept;
			
			item.TitleDescription = varTitleDescription;
			
			item.Title = varTitle;
			
			item.FirstName = varFirstName;
			
			item.Mi = varMi;
			
			item.LastName = varLastName;
			
			item.Extension = varExtension;
			
			item.BListInDirectory = varBListInDirectory;
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.VcPrincipal = varVcPrincipal;
			
			item.VcJsonOrdinal = varVcJsonOrdinal;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varLogin,string varEmailAddress,string varEPassword,string varDept,string varTitleDescription,string varTitle,string varFirstName,string varMi,string varLastName,string varExtension,bool varBListInDirectory,DateTime varDtStamp,Guid varApplicationId,string varVcPrincipal,string varVcJsonOrdinal)
		{
			Employee item = new Employee();
			
				item.Id = varId;
			
				item.Login = varLogin;
			
				item.EmailAddress = varEmailAddress;
			
				item.EPassword = varEPassword;
			
				item.Dept = varDept;
			
				item.TitleDescription = varTitleDescription;
			
				item.Title = varTitle;
			
				item.FirstName = varFirstName;
			
				item.Mi = varMi;
			
				item.LastName = varLastName;
			
				item.Extension = varExtension;
			
				item.BListInDirectory = varBListInDirectory;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.VcPrincipal = varVcPrincipal;
			
				item.VcJsonOrdinal = varVcJsonOrdinal;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn LoginColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn EmailAddressColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn EPasswordColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DeptColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn TitleDescriptionColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TitleColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn FirstNameColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn MiColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn LastNameColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn ExtensionColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn BListInDirectoryColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn VcPrincipalColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn VcJsonOrdinalColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string Login = @"Login";
			 public static string EmailAddress = @"EmailAddress";
			 public static string EPassword = @"ePassword";
			 public static string Dept = @"Dept";
			 public static string TitleDescription = @"TitleDescription";
			 public static string Title = @"Title";
			 public static string FirstName = @"FirstName";
			 public static string Mi = @"MI";
			 public static string LastName = @"LastName";
			 public static string Extension = @"Extension";
			 public static string BListInDirectory = @"bListInDirectory";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string VcPrincipal = @"vcPrincipal";
			 public static string VcJsonOrdinal = @"vcJsonOrdinal";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
