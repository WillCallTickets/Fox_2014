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
	/// Strongly-typed collection for the AspnetPersonalizationPerUser class.
	/// </summary>
    [Serializable]
	public partial class AspnetPersonalizationPerUserCollection : ActiveList<AspnetPersonalizationPerUser, AspnetPersonalizationPerUserCollection>
	{	   
		public AspnetPersonalizationPerUserCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AspnetPersonalizationPerUserCollection</returns>
		public AspnetPersonalizationPerUserCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AspnetPersonalizationPerUser o = this[i];
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
	/// This is an ActiveRecord class which wraps the aspnet_PersonalizationPerUser table.
	/// </summary>
	[Serializable]
	public partial class AspnetPersonalizationPerUser : ActiveRecord<AspnetPersonalizationPerUser>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AspnetPersonalizationPerUser()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AspnetPersonalizationPerUser(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AspnetPersonalizationPerUser(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AspnetPersonalizationPerUser(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("aspnet_PersonalizationPerUser", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "Id";
				colvarId.DataType = DbType.Guid;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = false;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = true;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				
						colvarId.DefaultSetting = @"(newid())";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarPathId = new TableSchema.TableColumn(schema);
				colvarPathId.ColumnName = "PathId";
				colvarPathId.DataType = DbType.Guid;
				colvarPathId.MaxLength = 0;
				colvarPathId.AutoIncrement = false;
				colvarPathId.IsNullable = true;
				colvarPathId.IsPrimaryKey = false;
				colvarPathId.IsForeignKey = true;
				colvarPathId.IsReadOnly = false;
				colvarPathId.DefaultSetting = @"";
				
					colvarPathId.ForeignKeyTableName = "aspnet_Paths";
				schema.Columns.Add(colvarPathId);
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = true;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = true;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				
					colvarUserId.ForeignKeyTableName = "aspnet_Users";
				schema.Columns.Add(colvarUserId);
				
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
				DataService.Providers["WillCall"].AddSchema("aspnet_PersonalizationPerUser",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Id")]
		[Bindable(true)]
		public Guid Id 
		{
			get { return GetColumnValue<Guid>(Columns.Id); }
			set { SetColumnValue(Columns.Id, value); }
		}
		  
		[XmlAttribute("PathId")]
		[Bindable(true)]
		public Guid? PathId 
		{
			get { return GetColumnValue<Guid?>(Columns.PathId); }
			set { SetColumnValue(Columns.PathId, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid? UserId 
		{
			get { return GetColumnValue<Guid?>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
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
		/// Returns a AspnetPath ActiveRecord object related to this AspnetPersonalizationPerUser
		/// 
		/// </summary>
		public Wcss.AspnetPath AspnetPath
		{
			get { return Wcss.AspnetPath.FetchByID(this.PathId); }
			set { SetColumnValue("PathId", value.PathId); }
		}
		
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this AspnetPersonalizationPerUser
		/// 
		/// </summary>
		public Wcss.AspnetUser AspnetUser
		{
			get { return Wcss.AspnetUser.FetchByID(this.UserId); }
			set { SetColumnValue("UserId", value.UserId); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varId,Guid? varPathId,Guid? varUserId,byte[] varPageSettings,DateTime varLastUpdatedDate)
		{
			AspnetPersonalizationPerUser item = new AspnetPersonalizationPerUser();
			
			item.Id = varId;
			
			item.PathId = varPathId;
			
			item.UserId = varUserId;
			
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
		public static void Update(Guid varId,Guid? varPathId,Guid? varUserId,byte[] varPageSettings,DateTime varLastUpdatedDate)
		{
			AspnetPersonalizationPerUser item = new AspnetPersonalizationPerUser();
			
				item.Id = varId;
			
				item.PathId = varPathId;
			
				item.UserId = varUserId;
			
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
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PathIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PageSettingsColumn
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
			 public static string Id = @"Id";
			 public static string PathId = @"PathId";
			 public static string UserId = @"UserId";
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
