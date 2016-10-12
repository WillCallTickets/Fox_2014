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
	/// Strongly-typed collection for the AspnetPath class.
	/// </summary>
    [Serializable]
	public partial class AspnetPathCollection : ActiveList<AspnetPath, AspnetPathCollection>
	{	   
		public AspnetPathCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AspnetPathCollection</returns>
		public AspnetPathCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AspnetPath o = this[i];
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
	/// This is an ActiveRecord class which wraps the aspnet_Paths table.
	/// </summary>
	[Serializable]
	public partial class AspnetPath : ActiveRecord<AspnetPath>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AspnetPath()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AspnetPath(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AspnetPath(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AspnetPath(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("aspnet_Paths", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
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
				
				TableSchema.TableColumn colvarPathId = new TableSchema.TableColumn(schema);
				colvarPathId.ColumnName = "PathId";
				colvarPathId.DataType = DbType.Guid;
				colvarPathId.MaxLength = 0;
				colvarPathId.AutoIncrement = false;
				colvarPathId.IsNullable = false;
				colvarPathId.IsPrimaryKey = true;
				colvarPathId.IsForeignKey = false;
				colvarPathId.IsReadOnly = false;
				
						colvarPathId.DefaultSetting = @"(newid())";
				colvarPathId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPathId);
				
				TableSchema.TableColumn colvarPath = new TableSchema.TableColumn(schema);
				colvarPath.ColumnName = "Path";
				colvarPath.DataType = DbType.String;
				colvarPath.MaxLength = 256;
				colvarPath.AutoIncrement = false;
				colvarPath.IsNullable = false;
				colvarPath.IsPrimaryKey = false;
				colvarPath.IsForeignKey = false;
				colvarPath.IsReadOnly = false;
				colvarPath.DefaultSetting = @"";
				colvarPath.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPath);
				
				TableSchema.TableColumn colvarLoweredPath = new TableSchema.TableColumn(schema);
				colvarLoweredPath.ColumnName = "LoweredPath";
				colvarLoweredPath.DataType = DbType.String;
				colvarLoweredPath.MaxLength = 256;
				colvarLoweredPath.AutoIncrement = false;
				colvarLoweredPath.IsNullable = false;
				colvarLoweredPath.IsPrimaryKey = false;
				colvarLoweredPath.IsForeignKey = false;
				colvarLoweredPath.IsReadOnly = false;
				colvarLoweredPath.DefaultSetting = @"";
				colvarLoweredPath.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLoweredPath);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("aspnet_Paths",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("PathId")]
		[Bindable(true)]
		public Guid PathId 
		{
			get { return GetColumnValue<Guid>(Columns.PathId); }
			set { SetColumnValue(Columns.PathId, value); }
		}
		  
		[XmlAttribute("Path")]
		[Bindable(true)]
		public string Path 
		{
			get { return GetColumnValue<string>(Columns.Path); }
			set { SetColumnValue(Columns.Path, value); }
		}
		  
		[XmlAttribute("LoweredPath")]
		[Bindable(true)]
		public string LoweredPath 
		{
			get { return GetColumnValue<string>(Columns.LoweredPath); }
			set { SetColumnValue(Columns.LoweredPath, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.AspnetPersonalizationAllUserCollection colAspnetPersonalizationAllUsers;
		public Wcss.AspnetPersonalizationAllUserCollection AspnetPersonalizationAllUsers()
		{
			if(colAspnetPersonalizationAllUsers == null)
			{
				colAspnetPersonalizationAllUsers = new Wcss.AspnetPersonalizationAllUserCollection().Where(AspnetPersonalizationAllUser.Columns.PathId, PathId).Load();
				colAspnetPersonalizationAllUsers.ListChanged += new ListChangedEventHandler(colAspnetPersonalizationAllUsers_ListChanged);
			}
			return colAspnetPersonalizationAllUsers;
		}
				
		void colAspnetPersonalizationAllUsers_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetPersonalizationAllUsers[e.NewIndex].PathId = PathId;
				colAspnetPersonalizationAllUsers.ListChanged += new ListChangedEventHandler(colAspnetPersonalizationAllUsers_ListChanged);
            }
		}
		private Wcss.AspnetPersonalizationPerUserCollection colAspnetPersonalizationPerUserRecords;
		public Wcss.AspnetPersonalizationPerUserCollection AspnetPersonalizationPerUserRecords()
		{
			if(colAspnetPersonalizationPerUserRecords == null)
			{
				colAspnetPersonalizationPerUserRecords = new Wcss.AspnetPersonalizationPerUserCollection().Where(AspnetPersonalizationPerUser.Columns.PathId, PathId).Load();
				colAspnetPersonalizationPerUserRecords.ListChanged += new ListChangedEventHandler(colAspnetPersonalizationPerUserRecords_ListChanged);
			}
			return colAspnetPersonalizationPerUserRecords;
		}
				
		void colAspnetPersonalizationPerUserRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetPersonalizationPerUserRecords[e.NewIndex].PathId = PathId;
				colAspnetPersonalizationPerUserRecords.ListChanged += new ListChangedEventHandler(colAspnetPersonalizationPerUserRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this AspnetPath
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
		public static void Insert(Guid varApplicationId,Guid varPathId,string varPath,string varLoweredPath)
		{
			AspnetPath item = new AspnetPath();
			
			item.ApplicationId = varApplicationId;
			
			item.PathId = varPathId;
			
			item.Path = varPath;
			
			item.LoweredPath = varLoweredPath;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varApplicationId,Guid varPathId,string varPath,string varLoweredPath)
		{
			AspnetPath item = new AspnetPath();
			
				item.ApplicationId = varApplicationId;
			
				item.PathId = varPathId;
			
				item.Path = varPath;
			
				item.LoweredPath = varLoweredPath;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PathIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn PathColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn LoweredPathColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ApplicationId = @"ApplicationId";
			 public static string PathId = @"PathId";
			 public static string Path = @"Path";
			 public static string LoweredPath = @"LoweredPath";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colAspnetPersonalizationAllUsers != null)
                {
                    foreach (Wcss.AspnetPersonalizationAllUser item in colAspnetPersonalizationAllUsers)
                    {
                        if (item.PathId != PathId)
                        {
                            item.PathId = PathId;
                        }
                    }
               }
		
                if (colAspnetPersonalizationPerUserRecords != null)
                {
                    foreach (Wcss.AspnetPersonalizationPerUser item in colAspnetPersonalizationPerUserRecords)
                    {
                        if (item.PathId != PathId)
                        {
                            item.PathId = PathId;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colAspnetPersonalizationAllUsers != null)
                {
                    colAspnetPersonalizationAllUsers.SaveAll();
               }
		
                if (colAspnetPersonalizationPerUserRecords != null)
                {
                    colAspnetPersonalizationPerUserRecords.SaveAll();
               }
		}
        #endregion
	}
}
