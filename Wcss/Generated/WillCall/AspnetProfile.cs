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
	/// Strongly-typed collection for the AspnetProfile class.
	/// </summary>
    [Serializable]
	public partial class AspnetProfileCollection : ActiveList<AspnetProfile, AspnetProfileCollection>
	{	   
		public AspnetProfileCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AspnetProfileCollection</returns>
		public AspnetProfileCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AspnetProfile o = this[i];
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
	/// This is an ActiveRecord class which wraps the aspnet_Profile table.
	/// </summary>
	[Serializable]
	public partial class AspnetProfile : ActiveRecord<AspnetProfile>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AspnetProfile()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AspnetProfile(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AspnetProfile(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AspnetProfile(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("aspnet_Profile", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = false;
				colvarUserId.IsPrimaryKey = true;
				colvarUserId.IsForeignKey = true;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				
					colvarUserId.ForeignKeyTableName = "aspnet_Users";
				schema.Columns.Add(colvarUserId);
				
				TableSchema.TableColumn colvarPropertyNames = new TableSchema.TableColumn(schema);
				colvarPropertyNames.ColumnName = "PropertyNames";
				colvarPropertyNames.DataType = DbType.String;
				colvarPropertyNames.MaxLength = 1073741823;
				colvarPropertyNames.AutoIncrement = false;
				colvarPropertyNames.IsNullable = false;
				colvarPropertyNames.IsPrimaryKey = false;
				colvarPropertyNames.IsForeignKey = false;
				colvarPropertyNames.IsReadOnly = false;
				colvarPropertyNames.DefaultSetting = @"";
				colvarPropertyNames.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPropertyNames);
				
				TableSchema.TableColumn colvarPropertyValuesString = new TableSchema.TableColumn(schema);
				colvarPropertyValuesString.ColumnName = "PropertyValuesString";
				colvarPropertyValuesString.DataType = DbType.String;
				colvarPropertyValuesString.MaxLength = 1073741823;
				colvarPropertyValuesString.AutoIncrement = false;
				colvarPropertyValuesString.IsNullable = false;
				colvarPropertyValuesString.IsPrimaryKey = false;
				colvarPropertyValuesString.IsForeignKey = false;
				colvarPropertyValuesString.IsReadOnly = false;
				colvarPropertyValuesString.DefaultSetting = @"";
				colvarPropertyValuesString.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPropertyValuesString);
				
				TableSchema.TableColumn colvarPropertyValuesBinary = new TableSchema.TableColumn(schema);
				colvarPropertyValuesBinary.ColumnName = "PropertyValuesBinary";
				colvarPropertyValuesBinary.DataType = DbType.Binary;
				colvarPropertyValuesBinary.MaxLength = 2147483647;
				colvarPropertyValuesBinary.AutoIncrement = false;
				colvarPropertyValuesBinary.IsNullable = false;
				colvarPropertyValuesBinary.IsPrimaryKey = false;
				colvarPropertyValuesBinary.IsForeignKey = false;
				colvarPropertyValuesBinary.IsReadOnly = false;
				colvarPropertyValuesBinary.DefaultSetting = @"";
				colvarPropertyValuesBinary.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPropertyValuesBinary);
				
				TableSchema.TableColumn colvarLastUpdatedDate = new TableSchema.TableColumn(schema);
				colvarLastUpdatedDate.ColumnName = "LastUpdatedDate";
				colvarLastUpdatedDate.DataType = DbType.DateTime;
				colvarLastUpdatedDate.MaxLength = 0;
				colvarLastUpdatedDate.AutoIncrement = false;
				colvarLastUpdatedDate.IsNullable = false;
				colvarLastUpdatedDate.IsPrimaryKey = false;
				colvarLastUpdatedDate.IsForeignKey = false;
				colvarLastUpdatedDate.IsReadOnly = false;
				colvarLastUpdatedDate.DefaultSetting = @"";
				colvarLastUpdatedDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastUpdatedDate);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("aspnet_Profile",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid UserId 
		{
			get { return GetColumnValue<Guid>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("PropertyNames")]
		[Bindable(true)]
		public string PropertyNames 
		{
			get { return GetColumnValue<string>(Columns.PropertyNames); }
			set { SetColumnValue(Columns.PropertyNames, value); }
		}
		  
		[XmlAttribute("PropertyValuesString")]
		[Bindable(true)]
		public string PropertyValuesString 
		{
			get { return GetColumnValue<string>(Columns.PropertyValuesString); }
			set { SetColumnValue(Columns.PropertyValuesString, value); }
		}
		  
		[XmlAttribute("PropertyValuesBinary")]
		[Bindable(true)]
		public byte[] PropertyValuesBinary 
		{
			get { return GetColumnValue<byte[]>(Columns.PropertyValuesBinary); }
			set { SetColumnValue(Columns.PropertyValuesBinary, value); }
		}
		  
		[XmlAttribute("LastUpdatedDate")]
		[Bindable(true)]
		public DateTime LastUpdatedDate 
		{
			get { return GetColumnValue<DateTime>(Columns.LastUpdatedDate); }
			set { SetColumnValue(Columns.LastUpdatedDate, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this AspnetProfile
		/// 
		/// </summary>
		private Wcss.AspnetUser AspnetUser
		{
			get { return Wcss.AspnetUser.FetchByID(this.UserId); }
			set { SetColumnValue("UserId", value.UserId); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.AspnetUser _aspnetuserrecord = null;
		
		public Wcss.AspnetUser AspnetUserRecord
		{
		    get
            {
                if (_aspnetuserrecord == null)
                {
                    _aspnetuserrecord = new Wcss.AspnetUser();
                    _aspnetuserrecord.CopyFrom(this.AspnetUser);
                }
                return _aspnetuserrecord;
            }
            set
            {
                if(value != null && _aspnetuserrecord == null)
			        _aspnetuserrecord = new Wcss.AspnetUser();
                
                SetColumnValue("UserId", value.UserId);
                _aspnetuserrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varUserId,string varPropertyNames,string varPropertyValuesString,byte[] varPropertyValuesBinary,DateTime varLastUpdatedDate)
		{
			AspnetProfile item = new AspnetProfile();
			
			item.UserId = varUserId;
			
			item.PropertyNames = varPropertyNames;
			
			item.PropertyValuesString = varPropertyValuesString;
			
			item.PropertyValuesBinary = varPropertyValuesBinary;
			
			item.LastUpdatedDate = varLastUpdatedDate;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varUserId,string varPropertyNames,string varPropertyValuesString,byte[] varPropertyValuesBinary,DateTime varLastUpdatedDate)
		{
			AspnetProfile item = new AspnetProfile();
			
				item.UserId = varUserId;
			
				item.PropertyNames = varPropertyNames;
			
				item.PropertyValuesString = varPropertyValuesString;
			
				item.PropertyValuesBinary = varPropertyValuesBinary;
			
				item.LastUpdatedDate = varLastUpdatedDate;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PropertyNamesColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn PropertyValuesStringColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PropertyValuesBinaryColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn LastUpdatedDateColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string UserId = @"UserId";
			 public static string PropertyNames = @"PropertyNames";
			 public static string PropertyValuesString = @"PropertyValuesString";
			 public static string PropertyValuesBinary = @"PropertyValuesBinary";
			 public static string LastUpdatedDate = @"LastUpdatedDate";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
