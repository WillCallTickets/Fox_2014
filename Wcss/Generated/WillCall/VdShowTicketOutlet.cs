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
	/// Strongly-typed collection for the VdShowTicketOutlet class.
	/// </summary>
    [Serializable]
	public partial class VdShowTicketOutletCollection : ActiveList<VdShowTicketOutlet, VdShowTicketOutletCollection>
	{	   
		public VdShowTicketOutletCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>VdShowTicketOutletCollection</returns>
		public VdShowTicketOutletCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                VdShowTicketOutlet o = this[i];
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
	/// This is an ActiveRecord class which wraps the VdShowTicketOutlet table.
	/// </summary>
	[Serializable]
	public partial class VdShowTicketOutlet : ActiveRecord<VdShowTicketOutlet>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public VdShowTicketOutlet()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public VdShowTicketOutlet(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public VdShowTicketOutlet(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public VdShowTicketOutlet(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("VdShowTicketOutlet", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTVdShowTicketId = new TableSchema.TableColumn(schema);
				colvarTVdShowTicketId.ColumnName = "tVdShowTicketId";
				colvarTVdShowTicketId.DataType = DbType.Int32;
				colvarTVdShowTicketId.MaxLength = 0;
				colvarTVdShowTicketId.AutoIncrement = false;
				colvarTVdShowTicketId.IsNullable = false;
				colvarTVdShowTicketId.IsPrimaryKey = false;
				colvarTVdShowTicketId.IsForeignKey = true;
				colvarTVdShowTicketId.IsReadOnly = false;
				colvarTVdShowTicketId.DefaultSetting = @"";
				
					colvarTVdShowTicketId.ForeignKeyTableName = "VdShowTicket";
				schema.Columns.Add(colvarTVdShowTicketId);
				
				TableSchema.TableColumn colvarOutletName = new TableSchema.TableColumn(schema);
				colvarOutletName.ColumnName = "OutletName";
				colvarOutletName.DataType = DbType.AnsiString;
				colvarOutletName.MaxLength = 256;
				colvarOutletName.AutoIncrement = false;
				colvarOutletName.IsNullable = false;
				colvarOutletName.IsPrimaryKey = false;
				colvarOutletName.IsForeignKey = false;
				colvarOutletName.IsReadOnly = false;
				colvarOutletName.DefaultSetting = @"";
				colvarOutletName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOutletName);
				
				TableSchema.TableColumn colvarIAllotment = new TableSchema.TableColumn(schema);
				colvarIAllotment.ColumnName = "iAllotment";
				colvarIAllotment.DataType = DbType.Int32;
				colvarIAllotment.MaxLength = 0;
				colvarIAllotment.AutoIncrement = false;
				colvarIAllotment.IsNullable = false;
				colvarIAllotment.IsPrimaryKey = false;
				colvarIAllotment.IsForeignKey = false;
				colvarIAllotment.IsReadOnly = false;
				
						colvarIAllotment.DefaultSetting = @"((0))";
				colvarIAllotment.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIAllotment);
				
				TableSchema.TableColumn colvarISold = new TableSchema.TableColumn(schema);
				colvarISold.ColumnName = "iSold";
				colvarISold.DataType = DbType.Int32;
				colvarISold.MaxLength = 0;
				colvarISold.AutoIncrement = false;
				colvarISold.IsNullable = false;
				colvarISold.IsPrimaryKey = false;
				colvarISold.IsForeignKey = false;
				colvarISold.IsReadOnly = false;
				colvarISold.DefaultSetting = @"";
				colvarISold.ForeignKeyTableName = "";
				schema.Columns.Add(colvarISold);
				
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
				DataService.Providers["WillCall"].AddSchema("VdShowTicketOutlet",schema);
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
		  
		[XmlAttribute("TVdShowTicketId")]
		[Bindable(true)]
		public int TVdShowTicketId 
		{
			get { return GetColumnValue<int>(Columns.TVdShowTicketId); }
			set { SetColumnValue(Columns.TVdShowTicketId, value); }
		}
		  
		[XmlAttribute("OutletName")]
		[Bindable(true)]
		public string OutletName 
		{
			get { return GetColumnValue<string>(Columns.OutletName); }
			set { SetColumnValue(Columns.OutletName, value); }
		}
		  
		[XmlAttribute("IAllotment")]
		[Bindable(true)]
		public int IAllotment 
		{
			get { return GetColumnValue<int>(Columns.IAllotment); }
			set { SetColumnValue(Columns.IAllotment, value); }
		}
		  
		[XmlAttribute("ISold")]
		[Bindable(true)]
		public int ISold 
		{
			get { return GetColumnValue<int>(Columns.ISold); }
			set { SetColumnValue(Columns.ISold, value); }
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
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Show ActiveRecord object related to this VdShowTicketOutlet
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
		/// Returns a VdShowTicket ActiveRecord object related to this VdShowTicketOutlet
		/// 
		/// </summary>
		private Wcss.VdShowTicket VdShowTicket
		{
			get { return Wcss.VdShowTicket.FetchByID(this.TVdShowTicketId); }
			set { SetColumnValue("tVdShowTicketId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.VdShowTicket _vdshowticketrecord = null;
		
		public Wcss.VdShowTicket VdShowTicketRecord
		{
		    get
            {
                if (_vdshowticketrecord == null)
                {
                    _vdshowticketrecord = new Wcss.VdShowTicket();
                    _vdshowticketrecord.CopyFrom(this.VdShowTicket);
                }
                return _vdshowticketrecord;
            }
            set
            {
                if(value != null && _vdshowticketrecord == null)
			        _vdshowticketrecord = new Wcss.VdShowTicket();
                
                SetColumnValue("tVdShowTicketId", value.Id);
                _vdshowticketrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varTShowId,int varTVdShowTicketId,string varOutletName,int varIAllotment,int varISold,int varIOrdinal,DateTime varDtStamp)
		{
			VdShowTicketOutlet item = new VdShowTicketOutlet();
			
			item.TShowId = varTShowId;
			
			item.TVdShowTicketId = varTVdShowTicketId;
			
			item.OutletName = varOutletName;
			
			item.IAllotment = varIAllotment;
			
			item.ISold = varISold;
			
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
		public static void Update(int varId,int varTShowId,int varTVdShowTicketId,string varOutletName,int varIAllotment,int varISold,int varIOrdinal,DateTime varDtStamp)
		{
			VdShowTicketOutlet item = new VdShowTicketOutlet();
			
				item.Id = varId;
			
				item.TShowId = varTShowId;
			
				item.TVdShowTicketId = varTVdShowTicketId;
			
				item.OutletName = varOutletName;
			
				item.IAllotment = varIAllotment;
			
				item.ISold = varISold;
			
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
        
        
        
        public static TableSchema.TableColumn TVdShowTicketIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn OutletNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IAllotmentColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ISoldColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn IOrdinalColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TShowId = @"tShowId";
			 public static string TVdShowTicketId = @"tVdShowTicketId";
			 public static string OutletName = @"OutletName";
			 public static string IAllotment = @"iAllotment";
			 public static string ISold = @"iSold";
			 public static string IOrdinal = @"iOrdinal";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
