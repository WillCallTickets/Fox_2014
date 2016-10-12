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
	/// Strongly-typed collection for the AspnetPersonalizationAllUser class.
	/// </summary>
    [Serializable]
	public partial class AspnetPersonalizationAllUserCollection : ActiveList<AspnetPersonalizationAllUser, AspnetPersonalizationAllUserCollection>
	{	   
		public AspnetPersonalizationAllUserCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AspnetPersonalizationAllUserCollection</returns>
		public AspnetPersonalizationAllUserCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AspnetPersonalizationAllUser o = this[i];
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
	/// This is an ActiveRecord class which wraps the aspnet_PersonalizationAllUsers table.
	/// </summary>
	[Serializable]
	public partial class AspnetPersonalizationAllUser : ActiveRecord<AspnetPersonalizationAllUser>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AspnetPersonalizationAllUser()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AspnetPersonalizationAllUser(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AspnetPersonalizationAllUser(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AspnetPersonalizationAllUser(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("aspnet_PersonalizationAllUsers", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPathId = new TableSchema.TableColumn(schema);
				colvarPathId.ColumnName = "PathId";
				colvarPathId.DataType = DbType.Guid;
				colvarPathId.MaxLength = 0;
				colvarPathId.AutoIncrement = false;
				colvarPathId.IsNullable = false;
				colvarPathId.IsPrimaryKey = true;
				colvarPathId.IsForeignKey = true;
				colvarPathId.IsReadOnly = false;
				colvarPathId.DefaultSetting = @"";
				
					colvarPathId.ForeignKeyTableName = "aspnet_Paths";
				schema.Columns.Add(colvarPathId);
				
				TableSchema.TableColumn colvarPageSettings = new TableSchema.TableColumn(schema);
				colvarPageSettings.ColumnName = "PageSettings";
				colvarPageSettings.DataType = DbType.Binary;
				colvarPageSettings.MaxLength = 2147483647;
				colvarPageSettings.AutoIncrement = false;
				colvarPageSettings.IsNullable = false;
				colvarPageSettings.IsPrimaryKey = false;
				colvarPageSettings.IsForeignKey = false;
				colvarPageSettings.IsReadOnly = false;
				colvarPageSettings.DefaultSetting = @"";
				colvarPageSettings.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPageSettings);
				
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
				DataService.Providers["WillCall"].AddSchema("aspnet_PersonalizationAllUsers",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PathId")]
		[Bindable(true)]
		public Guid PathId 
		{
			get { return GetColumnValue<Guid>(Columns.PathId); }
			set { SetColumnValue(Columns.PathId, value); }
		}
		  
		[XmlAttribute("PageSettings")]
		[Bindable(true)]
		public byte[] PageSettings 
		{
			get { return GetColumnValue<byte[]>(Columns.PageSettings); }
			set { SetColumnValue(Columns.PageSettings, value); }
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
		/// Returns a AspnetPath ActiveRecord object related to this AspnetPersonalizationAllUser
		/// 
		/// </summary>
		private Wcss.AspnetPath AspnetPath
		{
			get { return Wcss.AspnetPath.FetchByID(this.PathId); }
			set { SetColumnValue("PathId", value.PathId); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.AspnetPath _aspnetpathrecord = null;
		
		public Wcss.AspnetPath AspnetPathRecord
		{
		    get
            {
                if (_aspnetpathrecord == null)
                {
                    _aspnetpathrecord = new Wcss.AspnetPath();
                    _aspnetpathrecord.CopyFrom(this.AspnetPath);
                }
                return _aspnetpathrecord;
            }
            set
            {
                if(value != null && _aspnetpathrecord == null)
			        _aspnetpathrecord = new Wcss.AspnetPath();
                
                SetColumnValue("PathId", value.PathId);
                _aspnetpathrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varPathId,byte[] varPageSettings,DateTime varLastUpdatedDate)
		{
			AspnetPersonalizationAllUser item = new AspnetPersonalizationAllUser();
			
			item.PathId = varPathId;
			
			item.PageSettings = varPageSettings;
			
			item.LastUpdatedDate = varLastUpdatedDate;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varPathId,byte[] varPageSettings,DateTime varLastUpdatedDate)
		{
			AspnetPersonalizationAllUser item = new AspnetPersonalizationAllUser();
			
				item.PathId = varPathId;
			
				item.PageSettings = varPageSettings;
			
				item.LastUpdatedDate = varLastUpdatedDate;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn PathIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PageSettingsColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn LastUpdatedDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PathId = @"PathId";
			 public static string PageSettings = @"PageSettings";
			 public static string LastUpdatedDate = @"LastUpdatedDate";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
