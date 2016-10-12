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
	/// Strongly-typed collection for the Post class.
	/// </summary>
    [Serializable]
	public partial class PostCollection : ActiveList<Post, PostCollection>
	{	   
		public PostCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PostCollection</returns>
		public PostCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Post o = this[i];
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
	/// This is an ActiveRecord class which wraps the Post table.
	/// </summary>
	[Serializable]
	public partial class Post : ActiveRecord<Post>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Post()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Post(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Post(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Post(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Post", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarVcPrincipal = new TableSchema.TableColumn(schema);
				colvarVcPrincipal.ColumnName = "vcPrincipal";
				colvarVcPrincipal.DataType = DbType.AnsiString;
				colvarVcPrincipal.MaxLength = 100;
				colvarVcPrincipal.AutoIncrement = false;
				colvarVcPrincipal.IsNullable = false;
				colvarVcPrincipal.IsPrimaryKey = false;
				colvarVcPrincipal.IsForeignKey = false;
				colvarVcPrincipal.IsReadOnly = false;
				colvarVcPrincipal.DefaultSetting = @"";
				colvarVcPrincipal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcPrincipal);
				
				TableSchema.TableColumn colvarBActive = new TableSchema.TableColumn(schema);
				colvarBActive.ColumnName = "bActive";
				colvarBActive.DataType = DbType.Boolean;
				colvarBActive.MaxLength = 0;
				colvarBActive.AutoIncrement = false;
				colvarBActive.IsNullable = false;
				colvarBActive.IsPrimaryKey = false;
				colvarBActive.IsForeignKey = false;
				colvarBActive.IsReadOnly = false;
				
						colvarBActive.DefaultSetting = @"((1))";
				colvarBActive.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBActive);
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.AnsiString;
				colvarName.MaxLength = 50;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = false;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarSlug = new TableSchema.TableColumn(schema);
				colvarSlug.ColumnName = "Slug";
				colvarSlug.DataType = DbType.AnsiString;
				colvarSlug.MaxLength = 50;
				colvarSlug.AutoIncrement = false;
				colvarSlug.IsNullable = false;
				colvarSlug.IsPrimaryKey = false;
				colvarSlug.IsForeignKey = false;
				colvarSlug.IsReadOnly = false;
				colvarSlug.DefaultSetting = @"";
				colvarSlug.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSlug);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 256;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarValueX = new TableSchema.TableColumn(schema);
				colvarValueX.ColumnName = "Value";
				colvarValueX.DataType = DbType.AnsiString;
				colvarValueX.MaxLength = -1;
				colvarValueX.AutoIncrement = false;
				colvarValueX.IsNullable = true;
				colvarValueX.IsPrimaryKey = false;
				colvarValueX.IsForeignKey = false;
				colvarValueX.IsReadOnly = false;
				colvarValueX.DefaultSetting = @"";
				colvarValueX.ForeignKeyTableName = "";
				schema.Columns.Add(colvarValueX);
				
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
				
				TableSchema.TableColumn colvarDtModified = new TableSchema.TableColumn(schema);
				colvarDtModified.ColumnName = "dtModified";
				colvarDtModified.DataType = DbType.DateTime;
				colvarDtModified.MaxLength = 0;
				colvarDtModified.AutoIncrement = false;
				colvarDtModified.IsNullable = false;
				colvarDtModified.IsPrimaryKey = false;
				colvarDtModified.IsForeignKey = false;
				colvarDtModified.IsReadOnly = false;
				
						colvarDtModified.DefaultSetting = @"(getdate())";
				colvarDtModified.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtModified);
				
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
				DataService.Providers["WillCall"].AddSchema("Post",schema);
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
		  
		[XmlAttribute("VcPrincipal")]
		[Bindable(true)]
		public string VcPrincipal 
		{
			get { return GetColumnValue<string>(Columns.VcPrincipal); }
			set { SetColumnValue(Columns.VcPrincipal, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("Slug")]
		[Bindable(true)]
		public string Slug 
		{
			get { return GetColumnValue<string>(Columns.Slug); }
			set { SetColumnValue(Columns.Slug, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("ValueX")]
		[Bindable(true)]
		public string ValueX 
		{
			get { return GetColumnValue<string>(Columns.ValueX); }
			set { SetColumnValue(Columns.ValueX, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("DtModified")]
		[Bindable(true)]
		public DateTime DtModified 
		{
			get { return GetColumnValue<DateTime>(Columns.DtModified); }
			set { SetColumnValue(Columns.DtModified, value); }
		}
		  
		[XmlAttribute("VcJsonOrdinal")]
		[Bindable(true)]
		public string VcJsonOrdinal 
		{
			get { return GetColumnValue<string>(Columns.VcJsonOrdinal); }
			set { SetColumnValue(Columns.VcJsonOrdinal, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varVcPrincipal,bool varBActive,string varName,string varSlug,string varDescription,string varValueX,DateTime varDtStamp,DateTime varDtModified,string varVcJsonOrdinal)
		{
			Post item = new Post();
			
			item.VcPrincipal = varVcPrincipal;
			
			item.BActive = varBActive;
			
			item.Name = varName;
			
			item.Slug = varSlug;
			
			item.Description = varDescription;
			
			item.ValueX = varValueX;
			
			item.DtStamp = varDtStamp;
			
			item.DtModified = varDtModified;
			
			item.VcJsonOrdinal = varVcJsonOrdinal;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varVcPrincipal,bool varBActive,string varName,string varSlug,string varDescription,string varValueX,DateTime varDtStamp,DateTime varDtModified,string varVcJsonOrdinal)
		{
			Post item = new Post();
			
				item.Id = varId;
			
				item.VcPrincipal = varVcPrincipal;
			
				item.BActive = varBActive;
			
				item.Name = varName;
			
				item.Slug = varSlug;
			
				item.Description = varDescription;
			
				item.ValueX = varValueX;
			
				item.DtStamp = varDtStamp;
			
				item.DtModified = varDtModified;
			
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
        
        
        
        public static TableSchema.TableColumn VcPrincipalColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn SlugColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ValueXColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DtModifiedColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn VcJsonOrdinalColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string VcPrincipal = @"vcPrincipal";
			 public static string BActive = @"bActive";
			 public static string Name = @"Name";
			 public static string Slug = @"Slug";
			 public static string Description = @"Description";
			 public static string ValueX = @"Value";
			 public static string DtStamp = @"dtStamp";
			 public static string DtModified = @"dtModified";
			 public static string VcJsonOrdinal = @"vcJsonOrdinal";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
