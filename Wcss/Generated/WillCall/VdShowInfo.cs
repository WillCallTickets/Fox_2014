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
	/// Strongly-typed collection for the VdShowInfo class.
	/// </summary>
    [Serializable]
	public partial class VdShowInfoCollection : ActiveList<VdShowInfo, VdShowInfoCollection>
	{	   
		public VdShowInfoCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>VdShowInfoCollection</returns>
		public VdShowInfoCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                VdShowInfo o = this[i];
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
	/// This is an ActiveRecord class which wraps the VdShowInfo table.
	/// </summary>
	[Serializable]
	public partial class VdShowInfo : ActiveRecord<VdShowInfo>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public VdShowInfo()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public VdShowInfo(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public VdShowInfo(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public VdShowInfo(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("VdShowInfo", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarAgent = new TableSchema.TableColumn(schema);
				colvarAgent.ColumnName = "Agent";
				colvarAgent.DataType = DbType.String;
				colvarAgent.MaxLength = 256;
				colvarAgent.AutoIncrement = false;
				colvarAgent.IsNullable = true;
				colvarAgent.IsPrimaryKey = false;
				colvarAgent.IsForeignKey = false;
				colvarAgent.IsReadOnly = false;
				colvarAgent.DefaultSetting = @"";
				colvarAgent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAgent);
				
				TableSchema.TableColumn colvarBuyer = new TableSchema.TableColumn(schema);
				colvarBuyer.ColumnName = "Buyer";
				colvarBuyer.DataType = DbType.String;
				colvarBuyer.MaxLength = 256;
				colvarBuyer.AutoIncrement = false;
				colvarBuyer.IsNullable = true;
				colvarBuyer.IsPrimaryKey = false;
				colvarBuyer.IsForeignKey = false;
				colvarBuyer.IsReadOnly = false;
				colvarBuyer.DefaultSetting = @"";
				colvarBuyer.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBuyer);
				
				TableSchema.TableColumn colvarMTicketGross = new TableSchema.TableColumn(schema);
				colvarMTicketGross.ColumnName = "mTicketGross";
				colvarMTicketGross.DataType = DbType.Currency;
				colvarMTicketGross.MaxLength = 0;
				colvarMTicketGross.AutoIncrement = false;
				colvarMTicketGross.IsNullable = true;
				colvarMTicketGross.IsPrimaryKey = false;
				colvarMTicketGross.IsForeignKey = false;
				colvarMTicketGross.IsReadOnly = false;
				colvarMTicketGross.DefaultSetting = @"";
				colvarMTicketGross.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMTicketGross);
				
				TableSchema.TableColumn colvarITicketsSold = new TableSchema.TableColumn(schema);
				colvarITicketsSold.ColumnName = "iTicketsSold";
				colvarITicketsSold.DataType = DbType.Int32;
				colvarITicketsSold.MaxLength = 0;
				colvarITicketsSold.AutoIncrement = false;
				colvarITicketsSold.IsNullable = true;
				colvarITicketsSold.IsPrimaryKey = false;
				colvarITicketsSold.IsForeignKey = false;
				colvarITicketsSold.IsReadOnly = false;
				colvarITicketsSold.DefaultSetting = @"";
				colvarITicketsSold.ForeignKeyTableName = "";
				schema.Columns.Add(colvarITicketsSold);
				
				TableSchema.TableColumn colvarICompsIn = new TableSchema.TableColumn(schema);
				colvarICompsIn.ColumnName = "iCompsIn";
				colvarICompsIn.DataType = DbType.Int32;
				colvarICompsIn.MaxLength = 0;
				colvarICompsIn.AutoIncrement = false;
				colvarICompsIn.IsNullable = true;
				colvarICompsIn.IsPrimaryKey = false;
				colvarICompsIn.IsForeignKey = false;
				colvarICompsIn.IsReadOnly = false;
				colvarICompsIn.DefaultSetting = @"";
				colvarICompsIn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarICompsIn);
				
				TableSchema.TableColumn colvarMFacilityFee = new TableSchema.TableColumn(schema);
				colvarMFacilityFee.ColumnName = "mFacilityFee";
				colvarMFacilityFee.DataType = DbType.Currency;
				colvarMFacilityFee.MaxLength = 0;
				colvarMFacilityFee.AutoIncrement = false;
				colvarMFacilityFee.IsNullable = true;
				colvarMFacilityFee.IsPrimaryKey = false;
				colvarMFacilityFee.IsForeignKey = false;
				colvarMFacilityFee.IsReadOnly = false;
				colvarMFacilityFee.DefaultSetting = @"";
				colvarMFacilityFee.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMFacilityFee);
				
				TableSchema.TableColumn colvarMConcessions = new TableSchema.TableColumn(schema);
				colvarMConcessions.ColumnName = "mConcessions";
				colvarMConcessions.DataType = DbType.Currency;
				colvarMConcessions.MaxLength = 0;
				colvarMConcessions.AutoIncrement = false;
				colvarMConcessions.IsNullable = true;
				colvarMConcessions.IsPrimaryKey = false;
				colvarMConcessions.IsForeignKey = false;
				colvarMConcessions.IsReadOnly = false;
				colvarMConcessions.DefaultSetting = @"";
				colvarMConcessions.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMConcessions);
				
				TableSchema.TableColumn colvarMBarTotal = new TableSchema.TableColumn(schema);
				colvarMBarTotal.ColumnName = "mBarTotal";
				colvarMBarTotal.DataType = DbType.Currency;
				colvarMBarTotal.MaxLength = 0;
				colvarMBarTotal.AutoIncrement = false;
				colvarMBarTotal.IsNullable = true;
				colvarMBarTotal.IsPrimaryKey = false;
				colvarMBarTotal.IsForeignKey = false;
				colvarMBarTotal.IsReadOnly = false;
				colvarMBarTotal.DefaultSetting = @"";
				colvarMBarTotal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMBarTotal);
				
				TableSchema.TableColumn colvarMBarPerHead = new TableSchema.TableColumn(schema);
				colvarMBarPerHead.ColumnName = "mBarPerHead";
				colvarMBarPerHead.DataType = DbType.Currency;
				colvarMBarPerHead.MaxLength = 0;
				colvarMBarPerHead.AutoIncrement = false;
				colvarMBarPerHead.IsNullable = true;
				colvarMBarPerHead.IsPrimaryKey = false;
				colvarMBarPerHead.IsForeignKey = false;
				colvarMBarPerHead.IsReadOnly = false;
				colvarMBarPerHead.DefaultSetting = @"";
				colvarMBarPerHead.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMBarPerHead);
				
				TableSchema.TableColumn colvarIMarketingDays = new TableSchema.TableColumn(schema);
				colvarIMarketingDays.ColumnName = "iMarketingDays";
				colvarIMarketingDays.DataType = DbType.Int32;
				colvarIMarketingDays.MaxLength = 0;
				colvarIMarketingDays.AutoIncrement = false;
				colvarIMarketingDays.IsNullable = true;
				colvarIMarketingDays.IsPrimaryKey = false;
				colvarIMarketingDays.IsForeignKey = false;
				colvarIMarketingDays.IsReadOnly = false;
				colvarIMarketingDays.DefaultSetting = @"";
				colvarIMarketingDays.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIMarketingDays);
				
				TableSchema.TableColumn colvarMod = new TableSchema.TableColumn(schema);
				colvarMod.ColumnName = "MOD";
				colvarMod.DataType = DbType.String;
				colvarMod.MaxLength = 256;
				colvarMod.AutoIncrement = false;
				colvarMod.IsNullable = true;
				colvarMod.IsPrimaryKey = false;
				colvarMod.IsForeignKey = false;
				colvarMod.IsReadOnly = false;
				colvarMod.DefaultSetting = @"";
				colvarMod.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMod);
				
				TableSchema.TableColumn colvarMProdLabor = new TableSchema.TableColumn(schema);
				colvarMProdLabor.ColumnName = "mProdLabor";
				colvarMProdLabor.DataType = DbType.Currency;
				colvarMProdLabor.MaxLength = 0;
				colvarMProdLabor.AutoIncrement = false;
				colvarMProdLabor.IsNullable = true;
				colvarMProdLabor.IsPrimaryKey = false;
				colvarMProdLabor.IsForeignKey = false;
				colvarMProdLabor.IsReadOnly = false;
				colvarMProdLabor.DefaultSetting = @"";
				colvarMProdLabor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMProdLabor);
				
				TableSchema.TableColumn colvarMSecurityLabor = new TableSchema.TableColumn(schema);
				colvarMSecurityLabor.ColumnName = "mSecurityLabor";
				colvarMSecurityLabor.DataType = DbType.Currency;
				colvarMSecurityLabor.MaxLength = 0;
				colvarMSecurityLabor.AutoIncrement = false;
				colvarMSecurityLabor.IsNullable = true;
				colvarMSecurityLabor.IsPrimaryKey = false;
				colvarMSecurityLabor.IsForeignKey = false;
				colvarMSecurityLabor.IsReadOnly = false;
				colvarMSecurityLabor.DefaultSetting = @"";
				colvarMSecurityLabor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMSecurityLabor);
				
				TableSchema.TableColumn colvarMHospitality = new TableSchema.TableColumn(schema);
				colvarMHospitality.ColumnName = "mHospitality";
				colvarMHospitality.DataType = DbType.Currency;
				colvarMHospitality.MaxLength = 0;
				colvarMHospitality.AutoIncrement = false;
				colvarMHospitality.IsNullable = true;
				colvarMHospitality.IsPrimaryKey = false;
				colvarMHospitality.IsForeignKey = false;
				colvarMHospitality.IsReadOnly = false;
				colvarMHospitality.DefaultSetting = @"";
				colvarMHospitality.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMHospitality);
				
				TableSchema.TableColumn colvarIMarketPlays = new TableSchema.TableColumn(schema);
				colvarIMarketPlays.ColumnName = "iMarketPlays";
				colvarIMarketPlays.DataType = DbType.Int32;
				colvarIMarketPlays.MaxLength = 0;
				colvarIMarketPlays.AutoIncrement = false;
				colvarIMarketPlays.IsNullable = true;
				colvarIMarketPlays.IsPrimaryKey = false;
				colvarIMarketPlays.IsForeignKey = false;
				colvarIMarketPlays.IsReadOnly = false;
				colvarIMarketPlays.DefaultSetting = @"";
				colvarIMarketPlays.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIMarketPlays);
				
				TableSchema.TableColumn colvarNotes = new TableSchema.TableColumn(schema);
				colvarNotes.ColumnName = "Notes";
				colvarNotes.DataType = DbType.AnsiString;
				colvarNotes.MaxLength = 2000;
				colvarNotes.AutoIncrement = false;
				colvarNotes.IsNullable = true;
				colvarNotes.IsPrimaryKey = false;
				colvarNotes.IsForeignKey = false;
				colvarNotes.IsReadOnly = false;
				colvarNotes.DefaultSetting = @"";
				colvarNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNotes);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("VdShowInfo",schema);
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
		  
		[XmlAttribute("Agent")]
		[Bindable(true)]
		public string Agent 
		{
			get { return GetColumnValue<string>(Columns.Agent); }
			set { SetColumnValue(Columns.Agent, value); }
		}
		  
		[XmlAttribute("Buyer")]
		[Bindable(true)]
		public string Buyer 
		{
			get { return GetColumnValue<string>(Columns.Buyer); }
			set { SetColumnValue(Columns.Buyer, value); }
		}
		  
		[XmlAttribute("MTicketGross")]
		[Bindable(true)]
		public decimal? MTicketGross 
		{
			get { return GetColumnValue<decimal?>(Columns.MTicketGross); }
			set { SetColumnValue(Columns.MTicketGross, value); }
		}
		  
		[XmlAttribute("ITicketsSold")]
		[Bindable(true)]
		public int? ITicketsSold 
		{
			get { return GetColumnValue<int?>(Columns.ITicketsSold); }
			set { SetColumnValue(Columns.ITicketsSold, value); }
		}
		  
		[XmlAttribute("ICompsIn")]
		[Bindable(true)]
		public int? ICompsIn 
		{
			get { return GetColumnValue<int?>(Columns.ICompsIn); }
			set { SetColumnValue(Columns.ICompsIn, value); }
		}
		  
		[XmlAttribute("MFacilityFee")]
		[Bindable(true)]
		public decimal? MFacilityFee 
		{
			get { return GetColumnValue<decimal?>(Columns.MFacilityFee); }
			set { SetColumnValue(Columns.MFacilityFee, value); }
		}
		  
		[XmlAttribute("MConcessions")]
		[Bindable(true)]
		public decimal? MConcessions 
		{
			get { return GetColumnValue<decimal?>(Columns.MConcessions); }
			set { SetColumnValue(Columns.MConcessions, value); }
		}
		  
		[XmlAttribute("MBarTotal")]
		[Bindable(true)]
		public decimal? MBarTotal 
		{
			get { return GetColumnValue<decimal?>(Columns.MBarTotal); }
			set { SetColumnValue(Columns.MBarTotal, value); }
		}
		  
		[XmlAttribute("MBarPerHead")]
		[Bindable(true)]
		public decimal? MBarPerHead 
		{
			get { return GetColumnValue<decimal?>(Columns.MBarPerHead); }
			set { SetColumnValue(Columns.MBarPerHead, value); }
		}
		  
		[XmlAttribute("IMarketingDays")]
		[Bindable(true)]
		public int? IMarketingDays 
		{
			get { return GetColumnValue<int?>(Columns.IMarketingDays); }
			set { SetColumnValue(Columns.IMarketingDays, value); }
		}
		  
		[XmlAttribute("Mod")]
		[Bindable(true)]
		public string Mod 
		{
			get { return GetColumnValue<string>(Columns.Mod); }
			set { SetColumnValue(Columns.Mod, value); }
		}
		  
		[XmlAttribute("MProdLabor")]
		[Bindable(true)]
		public decimal? MProdLabor 
		{
			get { return GetColumnValue<decimal?>(Columns.MProdLabor); }
			set { SetColumnValue(Columns.MProdLabor, value); }
		}
		  
		[XmlAttribute("MSecurityLabor")]
		[Bindable(true)]
		public decimal? MSecurityLabor 
		{
			get { return GetColumnValue<decimal?>(Columns.MSecurityLabor); }
			set { SetColumnValue(Columns.MSecurityLabor, value); }
		}
		  
		[XmlAttribute("MHospitality")]
		[Bindable(true)]
		public decimal? MHospitality 
		{
			get { return GetColumnValue<decimal?>(Columns.MHospitality); }
			set { SetColumnValue(Columns.MHospitality, value); }
		}
		  
		[XmlAttribute("IMarketPlays")]
		[Bindable(true)]
		public int? IMarketPlays 
		{
			get { return GetColumnValue<int?>(Columns.IMarketPlays); }
			set { SetColumnValue(Columns.IMarketPlays, value); }
		}
		  
		[XmlAttribute("Notes")]
		[Bindable(true)]
		public string Notes 
		{
			get { return GetColumnValue<string>(Columns.Notes); }
			set { SetColumnValue(Columns.Notes, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Show ActiveRecord object related to this VdShowInfo
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
		public static void Insert(int varTShowId,DateTime varDtStamp,DateTime varDtModified,string varAgent,string varBuyer,decimal? varMTicketGross,int? varITicketsSold,int? varICompsIn,decimal? varMFacilityFee,decimal? varMConcessions,decimal? varMBarTotal,decimal? varMBarPerHead,int? varIMarketingDays,string varMod,decimal? varMProdLabor,decimal? varMSecurityLabor,decimal? varMHospitality,int? varIMarketPlays,string varNotes)
		{
			VdShowInfo item = new VdShowInfo();
			
			item.TShowId = varTShowId;
			
			item.DtStamp = varDtStamp;
			
			item.DtModified = varDtModified;
			
			item.Agent = varAgent;
			
			item.Buyer = varBuyer;
			
			item.MTicketGross = varMTicketGross;
			
			item.ITicketsSold = varITicketsSold;
			
			item.ICompsIn = varICompsIn;
			
			item.MFacilityFee = varMFacilityFee;
			
			item.MConcessions = varMConcessions;
			
			item.MBarTotal = varMBarTotal;
			
			item.MBarPerHead = varMBarPerHead;
			
			item.IMarketingDays = varIMarketingDays;
			
			item.Mod = varMod;
			
			item.MProdLabor = varMProdLabor;
			
			item.MSecurityLabor = varMSecurityLabor;
			
			item.MHospitality = varMHospitality;
			
			item.IMarketPlays = varIMarketPlays;
			
			item.Notes = varNotes;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,int varTShowId,DateTime varDtStamp,DateTime varDtModified,string varAgent,string varBuyer,decimal? varMTicketGross,int? varITicketsSold,int? varICompsIn,decimal? varMFacilityFee,decimal? varMConcessions,decimal? varMBarTotal,decimal? varMBarPerHead,int? varIMarketingDays,string varMod,decimal? varMProdLabor,decimal? varMSecurityLabor,decimal? varMHospitality,int? varIMarketPlays,string varNotes)
		{
			VdShowInfo item = new VdShowInfo();
			
				item.Id = varId;
			
				item.TShowId = varTShowId;
			
				item.DtStamp = varDtStamp;
			
				item.DtModified = varDtModified;
			
				item.Agent = varAgent;
			
				item.Buyer = varBuyer;
			
				item.MTicketGross = varMTicketGross;
			
				item.ITicketsSold = varITicketsSold;
			
				item.ICompsIn = varICompsIn;
			
				item.MFacilityFee = varMFacilityFee;
			
				item.MConcessions = varMConcessions;
			
				item.MBarTotal = varMBarTotal;
			
				item.MBarPerHead = varMBarPerHead;
			
				item.IMarketingDays = varIMarketingDays;
			
				item.Mod = varMod;
			
				item.MProdLabor = varMProdLabor;
			
				item.MSecurityLabor = varMSecurityLabor;
			
				item.MHospitality = varMHospitality;
			
				item.IMarketPlays = varIMarketPlays;
			
				item.Notes = varNotes;
			
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
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DtModifiedColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn AgentColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BuyerColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn MTicketGrossColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ITicketsSoldColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ICompsInColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn MFacilityFeeColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn MConcessionsColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn MBarTotalColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn MBarPerHeadColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn IMarketingDaysColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn ModColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn MProdLaborColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn MSecurityLaborColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn MHospitalityColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn IMarketPlaysColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn NotesColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TShowId = @"tShowId";
			 public static string DtStamp = @"dtStamp";
			 public static string DtModified = @"dtModified";
			 public static string Agent = @"Agent";
			 public static string Buyer = @"Buyer";
			 public static string MTicketGross = @"mTicketGross";
			 public static string ITicketsSold = @"iTicketsSold";
			 public static string ICompsIn = @"iCompsIn";
			 public static string MFacilityFee = @"mFacilityFee";
			 public static string MConcessions = @"mConcessions";
			 public static string MBarTotal = @"mBarTotal";
			 public static string MBarPerHead = @"mBarPerHead";
			 public static string IMarketingDays = @"iMarketingDays";
			 public static string Mod = @"MOD";
			 public static string MProdLabor = @"mProdLabor";
			 public static string MSecurityLabor = @"mSecurityLabor";
			 public static string MHospitality = @"mHospitality";
			 public static string IMarketPlays = @"iMarketPlays";
			 public static string Notes = @"Notes";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
