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
	/// Strongly-typed collection for the VdShowGenre class.
	/// </summary>
    [Serializable]
	public partial class VdShowGenreCollection : ActiveList<VdShowGenre, VdShowGenreCollection>
	{	   
		public VdShowGenreCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>VdShowGenreCollection</returns>
		public VdShowGenreCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                VdShowGenre o = this[i];
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
	/// This is an ActiveRecord class which wraps the VdShowGenre table.
	/// </summary>
	[Serializable]
	public partial class VdShowGenre : ActiveRecord<VdShowGenre>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public VdShowGenre()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public VdShowGenre(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public VdShowGenre(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public VdShowGenre(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("VdShowGenre", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTShowId = new TableSchema.TableColumn(schema);
				colvarTShowId.ColumnName = "tShowId";
				colvarTShowId.DataType = DbType.Int32;
				colvarTShowId.MaxLength = 0;
				colvarTShowId.AutoIncrement = false;
				colvarTShowId.IsNullable = false;
				colvarTShowId.IsPrimaryKey = false;
				colvarTShowId.IsForeignKey = true;
				colvarTShowId.IsReadOnly = false;
				colvarTShowId.DefaultSetting = @"";
				
					colvarTShowId.ForeignKeyTableName = "Show";
				schema.Columns.Add(colvarTShowId);
				
				TableSchema.TableColumn colvarTParentGenreId = new TableSchema.TableColumn(schema);
				colvarTParentGenreId.ColumnName = "tParentGenreId";
				colvarTParentGenreId.DataType = DbType.Int32;
				colvarTParentGenreId.MaxLength = 0;
				colvarTParentGenreId.AutoIncrement = false;
				colvarTParentGenreId.IsNullable = true;
				colvarTParentGenreId.IsPrimaryKey = false;
				colvarTParentGenreId.IsForeignKey = true;
				colvarTParentGenreId.IsReadOnly = false;
				colvarTParentGenreId.DefaultSetting = @"";
				
					colvarTParentGenreId.ForeignKeyTableName = "VdShowGenre";
				schema.Columns.Add(colvarTParentGenreId);
				
				TableSchema.TableColumn colvarGenreName = new TableSchema.TableColumn(schema);
				colvarGenreName.ColumnName = "GenreName";
				colvarGenreName.DataType = DbType.AnsiString;
				colvarGenreName.MaxLength = 256;
				colvarGenreName.AutoIncrement = false;
				colvarGenreName.IsNullable = false;
				colvarGenreName.IsPrimaryKey = false;
				colvarGenreName.IsForeignKey = false;
				colvarGenreName.IsReadOnly = false;
				colvarGenreName.DefaultSetting = @"";
				colvarGenreName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGenreName);
				
				TableSchema.TableColumn colvarBIsMainGenre = new TableSchema.TableColumn(schema);
				colvarBIsMainGenre.ColumnName = "bIsMainGenre";
				colvarBIsMainGenre.DataType = DbType.Boolean;
				colvarBIsMainGenre.MaxLength = 0;
				colvarBIsMainGenre.AutoIncrement = false;
				colvarBIsMainGenre.IsNullable = false;
				colvarBIsMainGenre.IsPrimaryKey = false;
				colvarBIsMainGenre.IsForeignKey = false;
				colvarBIsMainGenre.IsReadOnly = false;
				
						colvarBIsMainGenre.DefaultSetting = @"((1))";
				colvarBIsMainGenre.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBIsMainGenre);
				
				TableSchema.TableColumn colvarIOrdinal = new TableSchema.TableColumn(schema);
				colvarIOrdinal.ColumnName = "iOrdinal";
				colvarIOrdinal.DataType = DbType.Int32;
				colvarIOrdinal.MaxLength = 0;
				colvarIOrdinal.AutoIncrement = false;
				colvarIOrdinal.IsNullable = false;
				colvarIOrdinal.IsPrimaryKey = false;
				colvarIOrdinal.IsForeignKey = false;
				colvarIOrdinal.IsReadOnly = false;
				
						colvarIOrdinal.DefaultSetting = @"((-1))";
				colvarIOrdinal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIOrdinal);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("VdShowGenre",schema);
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
		  
		[XmlAttribute("TShowId")]
		[Bindable(true)]
		public int TShowId 
		{
			get { return GetColumnValue<int>(Columns.TShowId); }
			set { SetColumnValue(Columns.TShowId, value); }
		}
		  
		[XmlAttribute("TParentGenreId")]
		[Bindable(true)]
		public int? TParentGenreId 
		{
			get { return GetColumnValue<int?>(Columns.TParentGenreId); }
			set { SetColumnValue(Columns.TParentGenreId, value); }
		}
		  
		[XmlAttribute("GenreName")]
		[Bindable(true)]
		public string GenreName 
		{
			get { return GetColumnValue<string>(Columns.GenreName); }
			set { SetColumnValue(Columns.GenreName, value); }
		}
		  
		[XmlAttribute("BIsMainGenre")]
		[Bindable(true)]
		public bool BIsMainGenre 
		{
			get { return GetColumnValue<bool>(Columns.BIsMainGenre); }
			set { SetColumnValue(Columns.BIsMainGenre, value); }
		}
		  
		[XmlAttribute("IOrdinal")]
		[Bindable(true)]
		public int IOrdinal 
		{
			get { return GetColumnValue<int>(Columns.IOrdinal); }
			set { SetColumnValue(Columns.IOrdinal, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.VdShowGenreCollection colChildVdShowGenreRecords;
		public Wcss.VdShowGenreCollection ChildVdShowGenreRecords()
		{
			if(colChildVdShowGenreRecords == null)
			{
				colChildVdShowGenreRecords = new Wcss.VdShowGenreCollection().Where(VdShowGenre.Columns.TParentGenreId, Id).Load();
				colChildVdShowGenreRecords.ListChanged += new ListChangedEventHandler(colChildVdShowGenreRecords_ListChanged);
			}
			return colChildVdShowGenreRecords;
		}
				
		void colChildVdShowGenreRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colChildVdShowGenreRecords[e.NewIndex].TParentGenreId = Id;
				colChildVdShowGenreRecords.ListChanged += new ListChangedEventHandler(colChildVdShowGenreRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Show ActiveRecord object related to this VdShowGenre
		/// 
		/// </summary>
		private Wcss.Show Show
		{
			get { return Wcss.Show.FetchByID(this.TShowId); }
			set { SetColumnValue("tShowId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Show _showrecord = null;
		
		public Wcss.Show ShowRecord
		{
		    get
            {
                if (_showrecord == null)
                {
                    _showrecord = new Wcss.Show();
                    _showrecord.CopyFrom(this.Show);
                }
                return _showrecord;
            }
            set
            {
                if(value != null && _showrecord == null)
			        _showrecord = new Wcss.Show();
                
                SetColumnValue("tShowId", value.Id);
                _showrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a VdShowGenre ActiveRecord object related to this VdShowGenre
		/// 
		/// </summary>
		private Wcss.VdShowGenre ParentVdShowGenre
		{
			get { return Wcss.VdShowGenre.FetchByID(this.TParentGenreId); }
			set { SetColumnValue("tParentGenreId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.VdShowGenre _parentvdshowgenrerecord = null;
		
		public Wcss.VdShowGenre ParentVdShowGenreRecord
		{
		    get
            {
                if (_parentvdshowgenrerecord == null)
                {
                    _parentvdshowgenrerecord = new Wcss.VdShowGenre();
                    _parentvdshowgenrerecord.CopyFrom(this.ParentVdShowGenre);
                }
                return _parentvdshowgenrerecord;
            }
            set
            {
                if(value != null && _parentvdshowgenrerecord == null)
			        _parentvdshowgenrerecord = new Wcss.VdShowGenre();
                
                SetColumnValue("tParentGenreId", value.Id);
                _parentvdshowgenrerecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varTShowId,int? varTParentGenreId,string varGenreName,bool varBIsMainGenre,int varIOrdinal,DateTime varDtStamp)
		{
			VdShowGenre item = new VdShowGenre();
			
			item.TShowId = varTShowId;
			
			item.TParentGenreId = varTParentGenreId;
			
			item.GenreName = varGenreName;
			
			item.BIsMainGenre = varBIsMainGenre;
			
			item.IOrdinal = varIOrdinal;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,int varTShowId,int? varTParentGenreId,string varGenreName,bool varBIsMainGenre,int varIOrdinal,DateTime varDtStamp)
		{
			VdShowGenre item = new VdShowGenre();
			
				item.Id = varId;
			
				item.TShowId = varTShowId;
			
				item.TParentGenreId = varTParentGenreId;
			
				item.GenreName = varGenreName;
			
				item.BIsMainGenre = varBIsMainGenre;
			
				item.IOrdinal = varIOrdinal;
			
				item.DtStamp = varDtStamp;
			
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
        
        
        
        public static TableSchema.TableColumn TShowIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TParentGenreIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn GenreNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BIsMainGenreColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IOrdinalColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TShowId = @"tShowId";
			 public static string TParentGenreId = @"tParentGenreId";
			 public static string GenreName = @"GenreName";
			 public static string BIsMainGenre = @"bIsMainGenre";
			 public static string IOrdinal = @"iOrdinal";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colChildVdShowGenreRecords != null)
                {
                    foreach (Wcss.VdShowGenre item in colChildVdShowGenreRecords)
                    {
                        if (item.TParentGenreId != Id)
                        {
                            item.TParentGenreId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colChildVdShowGenreRecords != null)
                {
                    colChildVdShowGenreRecords.SaveAll();
               }
		}
        #endregion
	}
}
