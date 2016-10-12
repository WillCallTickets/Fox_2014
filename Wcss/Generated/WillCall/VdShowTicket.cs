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
	/// Strongly-typed collection for the VdShowTicket class.
	/// </summary>
    [Serializable]
	public partial class VdShowTicketCollection : ActiveList<VdShowTicket, VdShowTicketCollection>
	{	   
		public VdShowTicketCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>VdShowTicketCollection</returns>
		public VdShowTicketCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                VdShowTicket o = this[i];
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
	/// This is an ActiveRecord class which wraps the VdShowTicket table.
	/// </summary>
	[Serializable]
	public partial class VdShowTicket : ActiveRecord<VdShowTicket>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public VdShowTicket()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public VdShowTicket(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public VdShowTicket(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public VdShowTicket(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("VdShowTicket", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTicketDescription = new TableSchema.TableColumn(schema);
				colvarTicketDescription.ColumnName = "TicketDescription";
				colvarTicketDescription.DataType = DbType.String;
				colvarTicketDescription.MaxLength = 256;
				colvarTicketDescription.AutoIncrement = false;
				colvarTicketDescription.IsNullable = false;
				colvarTicketDescription.IsPrimaryKey = false;
				colvarTicketDescription.IsForeignKey = false;
				colvarTicketDescription.IsReadOnly = false;
				colvarTicketDescription.DefaultSetting = @"";
				colvarTicketDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTicketDescription);
				
				TableSchema.TableColumn colvarTicketQualifier = new TableSchema.TableColumn(schema);
				colvarTicketQualifier.ColumnName = "TicketQualifier";
				colvarTicketQualifier.DataType = DbType.String;
				colvarTicketQualifier.MaxLength = 50;
				colvarTicketQualifier.AutoIncrement = false;
				colvarTicketQualifier.IsNullable = true;
				colvarTicketQualifier.IsPrimaryKey = false;
				colvarTicketQualifier.IsForeignKey = false;
				colvarTicketQualifier.IsReadOnly = false;
				colvarTicketQualifier.DefaultSetting = @"";
				colvarTicketQualifier.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTicketQualifier);
				
				TableSchema.TableColumn colvarBReserved = new TableSchema.TableColumn(schema);
				colvarBReserved.ColumnName = "bReserved";
				colvarBReserved.DataType = DbType.Boolean;
				colvarBReserved.MaxLength = 0;
				colvarBReserved.AutoIncrement = false;
				colvarBReserved.IsNullable = false;
				colvarBReserved.IsPrimaryKey = false;
				colvarBReserved.IsForeignKey = false;
				colvarBReserved.IsReadOnly = false;
				
						colvarBReserved.DefaultSetting = @"((0))";
				colvarBReserved.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBReserved);
				
				TableSchema.TableColumn colvarMBasePrice = new TableSchema.TableColumn(schema);
				colvarMBasePrice.ColumnName = "mBasePrice";
				colvarMBasePrice.DataType = DbType.Currency;
				colvarMBasePrice.MaxLength = 0;
				colvarMBasePrice.AutoIncrement = false;
				colvarMBasePrice.IsNullable = false;
				colvarMBasePrice.IsPrimaryKey = false;
				colvarMBasePrice.IsForeignKey = false;
				colvarMBasePrice.IsReadOnly = false;
				
						colvarMBasePrice.DefaultSetting = @"((0))";
				colvarMBasePrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMBasePrice);
				
				TableSchema.TableColumn colvarMServiceCharge = new TableSchema.TableColumn(schema);
				colvarMServiceCharge.ColumnName = "mServiceCharge";
				colvarMServiceCharge.DataType = DbType.Currency;
				colvarMServiceCharge.MaxLength = 0;
				colvarMServiceCharge.AutoIncrement = false;
				colvarMServiceCharge.IsNullable = false;
				colvarMServiceCharge.IsPrimaryKey = false;
				colvarMServiceCharge.IsForeignKey = false;
				colvarMServiceCharge.IsReadOnly = false;
				
						colvarMServiceCharge.DefaultSetting = @"((0))";
				colvarMServiceCharge.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMServiceCharge);
				
				TableSchema.TableColumn colvarAdditionalDescription = new TableSchema.TableColumn(schema);
				colvarAdditionalDescription.ColumnName = "AdditionalDescription";
				colvarAdditionalDescription.DataType = DbType.String;
				colvarAdditionalDescription.MaxLength = 256;
				colvarAdditionalDescription.AutoIncrement = false;
				colvarAdditionalDescription.IsNullable = true;
				colvarAdditionalDescription.IsPrimaryKey = false;
				colvarAdditionalDescription.IsForeignKey = false;
				colvarAdditionalDescription.IsReadOnly = false;
				colvarAdditionalDescription.DefaultSetting = @"";
				colvarAdditionalDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAdditionalDescription);
				
				TableSchema.TableColumn colvarMAdditionalCharge = new TableSchema.TableColumn(schema);
				colvarMAdditionalCharge.ColumnName = "mAdditionalCharge";
				colvarMAdditionalCharge.DataType = DbType.Currency;
				colvarMAdditionalCharge.MaxLength = 0;
				colvarMAdditionalCharge.AutoIncrement = false;
				colvarMAdditionalCharge.IsNullable = false;
				colvarMAdditionalCharge.IsPrimaryKey = false;
				colvarMAdditionalCharge.IsForeignKey = false;
				colvarMAdditionalCharge.IsReadOnly = false;
				
						colvarMAdditionalCharge.DefaultSetting = @"((0))";
				colvarMAdditionalCharge.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMAdditionalCharge);
				
				TableSchema.TableColumn colvarMEach = new TableSchema.TableColumn(schema);
				colvarMEach.ColumnName = "mEach";
				colvarMEach.DataType = DbType.Currency;
				colvarMEach.MaxLength = 0;
				colvarMEach.AutoIncrement = false;
				colvarMEach.IsNullable = true;
				colvarMEach.IsPrimaryKey = false;
				colvarMEach.IsForeignKey = false;
				colvarMEach.IsReadOnly = true;
				colvarMEach.DefaultSetting = @"";
				colvarMEach.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMEach);
				
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
				DataService.Providers["WillCall"].AddSchema("VdShowTicket",schema);
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
		  
		[XmlAttribute("TicketDescription")]
		[Bindable(true)]
		public string TicketDescription 
		{
			get { return GetColumnValue<string>(Columns.TicketDescription); }
			set { SetColumnValue(Columns.TicketDescription, value); }
		}
		  
		[XmlAttribute("TicketQualifier")]
		[Bindable(true)]
		public string TicketQualifier 
		{
			get { return GetColumnValue<string>(Columns.TicketQualifier); }
			set { SetColumnValue(Columns.TicketQualifier, value); }
		}
		  
		[XmlAttribute("BReserved")]
		[Bindable(true)]
		public bool BReserved 
		{
			get { return GetColumnValue<bool>(Columns.BReserved); }
			set { SetColumnValue(Columns.BReserved, value); }
		}
		  
		[XmlAttribute("MBasePrice")]
		[Bindable(true)]
		public decimal MBasePrice 
		{
			get { return GetColumnValue<decimal>(Columns.MBasePrice); }
			set { SetColumnValue(Columns.MBasePrice, value); }
		}
		  
		[XmlAttribute("MServiceCharge")]
		[Bindable(true)]
		public decimal MServiceCharge 
		{
			get { return GetColumnValue<decimal>(Columns.MServiceCharge); }
			set { SetColumnValue(Columns.MServiceCharge, value); }
		}
		  
		[XmlAttribute("AdditionalDescription")]
		[Bindable(true)]
		public string AdditionalDescription 
		{
			get { return GetColumnValue<string>(Columns.AdditionalDescription); }
			set { SetColumnValue(Columns.AdditionalDescription, value); }
		}
		  
		[XmlAttribute("MAdditionalCharge")]
		[Bindable(true)]
		public decimal MAdditionalCharge 
		{
			get { return GetColumnValue<decimal>(Columns.MAdditionalCharge); }
			set { SetColumnValue(Columns.MAdditionalCharge, value); }
		}
		  
		[XmlAttribute("MEach")]
		[Bindable(true)]
		public decimal? MEach 
		{
			get { return GetColumnValue<decimal?>(Columns.MEach); }
			set { SetColumnValue(Columns.MEach, value); }
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
        
		
		private Wcss.VdShowTicketOutletCollection colVdShowTicketOutletRecords;
		public Wcss.VdShowTicketOutletCollection VdShowTicketOutletRecords()
		{
			if(colVdShowTicketOutletRecords == null)
			{
				colVdShowTicketOutletRecords = new Wcss.VdShowTicketOutletCollection().Where(VdShowTicketOutlet.Columns.TVdShowTicketId, Id).Load();
				colVdShowTicketOutletRecords.ListChanged += new ListChangedEventHandler(colVdShowTicketOutletRecords_ListChanged);
			}
			return colVdShowTicketOutletRecords;
		}
				
		void colVdShowTicketOutletRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colVdShowTicketOutletRecords[e.NewIndex].TVdShowTicketId = Id;
				colVdShowTicketOutletRecords.ListChanged += new ListChangedEventHandler(colVdShowTicketOutletRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Show ActiveRecord object related to this VdShowTicket
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
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varTShowId,string varTicketDescription,string varTicketQualifier,bool varBReserved,decimal varMBasePrice,decimal varMServiceCharge,string varAdditionalDescription,decimal varMAdditionalCharge,decimal? varMEach,int varIOrdinal,DateTime varDtStamp)
		{
			VdShowTicket item = new VdShowTicket();
			
			item.TShowId = varTShowId;
			
			item.TicketDescription = varTicketDescription;
			
			item.TicketQualifier = varTicketQualifier;
			
			item.BReserved = varBReserved;
			
			item.MBasePrice = varMBasePrice;
			
			item.MServiceCharge = varMServiceCharge;
			
			item.AdditionalDescription = varAdditionalDescription;
			
			item.MAdditionalCharge = varMAdditionalCharge;
			
			item.MEach = varMEach;
			
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
		public static void Update(int varId,int varTShowId,string varTicketDescription,string varTicketQualifier,bool varBReserved,decimal varMBasePrice,decimal varMServiceCharge,string varAdditionalDescription,decimal varMAdditionalCharge,decimal? varMEach,int varIOrdinal,DateTime varDtStamp)
		{
			VdShowTicket item = new VdShowTicket();
			
				item.Id = varId;
			
				item.TShowId = varTShowId;
			
				item.TicketDescription = varTicketDescription;
			
				item.TicketQualifier = varTicketQualifier;
			
				item.BReserved = varBReserved;
			
				item.MBasePrice = varMBasePrice;
			
				item.MServiceCharge = varMServiceCharge;
			
				item.AdditionalDescription = varAdditionalDescription;
			
				item.MAdditionalCharge = varMAdditionalCharge;
			
				item.MEach = varMEach;
			
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
        
        
        
        public static TableSchema.TableColumn TicketDescriptionColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TicketQualifierColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BReservedColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn MBasePriceColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn MServiceChargeColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn AdditionalDescriptionColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn MAdditionalChargeColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn MEachColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn IOrdinalColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TShowId = @"tShowId";
			 public static string TicketDescription = @"TicketDescription";
			 public static string TicketQualifier = @"TicketQualifier";
			 public static string BReserved = @"bReserved";
			 public static string MBasePrice = @"mBasePrice";
			 public static string MServiceCharge = @"mServiceCharge";
			 public static string AdditionalDescription = @"AdditionalDescription";
			 public static string MAdditionalCharge = @"mAdditionalCharge";
			 public static string MEach = @"mEach";
			 public static string IOrdinal = @"iOrdinal";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colVdShowTicketOutletRecords != null)
                {
                    foreach (Wcss.VdShowTicketOutlet item in colVdShowTicketOutletRecords)
                    {
                        if (item.TVdShowTicketId != Id)
                        {
                            item.TVdShowTicketId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colVdShowTicketOutletRecords != null)
                {
                    colVdShowTicketOutletRecords.SaveAll();
               }
		}
        #endregion
	}
}
