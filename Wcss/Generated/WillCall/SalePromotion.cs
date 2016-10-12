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
	/// Strongly-typed collection for the SalePromotion class.
	/// </summary>
    [Serializable]
	public partial class SalePromotionCollection : ActiveList<SalePromotion, SalePromotionCollection>
	{	   
		public SalePromotionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SalePromotionCollection</returns>
		public SalePromotionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SalePromotion o = this[i];
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
	/// This is an ActiveRecord class which wraps the SalePromotion table.
	/// </summary>
	[Serializable]
	public partial class SalePromotion : ActiveRecord<SalePromotion>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SalePromotion()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SalePromotion(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SalePromotion(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SalePromotion(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SalePromotion", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarDtStamp = new TableSchema.TableColumn(schema);
				colvarDtStamp.ColumnName = "dtStamp";
				colvarDtStamp.DataType = DbType.DateTime;
				colvarDtStamp.MaxLength = 0;
				colvarDtStamp.AutoIncrement = false;
				colvarDtStamp.IsNullable = true;
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
				
				TableSchema.TableColumn colvarIBannerTimeoutMsecs = new TableSchema.TableColumn(schema);
				colvarIBannerTimeoutMsecs.ColumnName = "iBannerTimeoutMsecs";
				colvarIBannerTimeoutMsecs.DataType = DbType.Int32;
				colvarIBannerTimeoutMsecs.MaxLength = 0;
				colvarIBannerTimeoutMsecs.AutoIncrement = false;
				colvarIBannerTimeoutMsecs.IsNullable = false;
				colvarIBannerTimeoutMsecs.IsPrimaryKey = false;
				colvarIBannerTimeoutMsecs.IsForeignKey = false;
				colvarIBannerTimeoutMsecs.IsReadOnly = false;
				
						colvarIBannerTimeoutMsecs.DefaultSetting = @"((2400))";
				colvarIBannerTimeoutMsecs.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIBannerTimeoutMsecs);
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.AnsiString;
				colvarName.MaxLength = 256;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = false;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarDisplayText = new TableSchema.TableColumn(schema);
				colvarDisplayText.ColumnName = "DisplayText";
				colvarDisplayText.DataType = DbType.AnsiString;
				colvarDisplayText.MaxLength = 1000;
				colvarDisplayText.AutoIncrement = false;
				colvarDisplayText.IsNullable = true;
				colvarDisplayText.IsPrimaryKey = false;
				colvarDisplayText.IsForeignKey = false;
				colvarDisplayText.IsReadOnly = false;
				colvarDisplayText.DefaultSetting = @"";
				colvarDisplayText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayText);
				
				TableSchema.TableColumn colvarAdditionalText = new TableSchema.TableColumn(schema);
				colvarAdditionalText.ColumnName = "AdditionalText";
				colvarAdditionalText.DataType = DbType.AnsiString;
				colvarAdditionalText.MaxLength = 500;
				colvarAdditionalText.AutoIncrement = false;
				colvarAdditionalText.IsNullable = true;
				colvarAdditionalText.IsPrimaryKey = false;
				colvarAdditionalText.IsForeignKey = false;
				colvarAdditionalText.IsReadOnly = false;
				colvarAdditionalText.DefaultSetting = @"";
				colvarAdditionalText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAdditionalText);
				
				TableSchema.TableColumn colvarBannerUrl = new TableSchema.TableColumn(schema);
				colvarBannerUrl.ColumnName = "BannerUrl";
				colvarBannerUrl.DataType = DbType.AnsiString;
				colvarBannerUrl.MaxLength = 256;
				colvarBannerUrl.AutoIncrement = false;
				colvarBannerUrl.IsNullable = true;
				colvarBannerUrl.IsPrimaryKey = false;
				colvarBannerUrl.IsForeignKey = false;
				colvarBannerUrl.IsReadOnly = false;
				colvarBannerUrl.DefaultSetting = @"";
				colvarBannerUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBannerUrl);
				
				TableSchema.TableColumn colvarBannerClickUrl = new TableSchema.TableColumn(schema);
				colvarBannerClickUrl.ColumnName = "BannerClickUrl";
				colvarBannerClickUrl.DataType = DbType.AnsiString;
				colvarBannerClickUrl.MaxLength = 256;
				colvarBannerClickUrl.AutoIncrement = false;
				colvarBannerClickUrl.IsNullable = true;
				colvarBannerClickUrl.IsPrimaryKey = false;
				colvarBannerClickUrl.IsForeignKey = false;
				colvarBannerClickUrl.IsReadOnly = false;
				colvarBannerClickUrl.DefaultSetting = @"";
				colvarBannerClickUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBannerClickUrl);
				
				TableSchema.TableColumn colvarDtStartDate = new TableSchema.TableColumn(schema);
				colvarDtStartDate.ColumnName = "dtStartDate";
				colvarDtStartDate.DataType = DbType.DateTime;
				colvarDtStartDate.MaxLength = 0;
				colvarDtStartDate.AutoIncrement = false;
				colvarDtStartDate.IsNullable = true;
				colvarDtStartDate.IsPrimaryKey = false;
				colvarDtStartDate.IsForeignKey = false;
				colvarDtStartDate.IsReadOnly = false;
				colvarDtStartDate.DefaultSetting = @"";
				colvarDtStartDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStartDate);
				
				TableSchema.TableColumn colvarDtEndDate = new TableSchema.TableColumn(schema);
				colvarDtEndDate.ColumnName = "dtEndDate";
				colvarDtEndDate.DataType = DbType.DateTime;
				colvarDtEndDate.MaxLength = 0;
				colvarDtEndDate.AutoIncrement = false;
				colvarDtEndDate.IsNullable = true;
				colvarDtEndDate.IsPrimaryKey = false;
				colvarDtEndDate.IsForeignKey = false;
				colvarDtEndDate.IsReadOnly = false;
				colvarDtEndDate.DefaultSetting = @"";
				colvarDtEndDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtEndDate);
				
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
				DataService.Providers["WillCall"].AddSchema("SalePromotion",schema);
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
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime? DtStamp 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("IBannerTimeoutMsecs")]
		[Bindable(true)]
		public int IBannerTimeoutMsecs 
		{
			get { return GetColumnValue<int>(Columns.IBannerTimeoutMsecs); }
			set { SetColumnValue(Columns.IBannerTimeoutMsecs, value); }
		}
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("DisplayText")]
		[Bindable(true)]
		public string DisplayText 
		{
			get { return GetColumnValue<string>(Columns.DisplayText); }
			set { SetColumnValue(Columns.DisplayText, value); }
		}
		  
		[XmlAttribute("AdditionalText")]
		[Bindable(true)]
		public string AdditionalText 
		{
			get { return GetColumnValue<string>(Columns.AdditionalText); }
			set { SetColumnValue(Columns.AdditionalText, value); }
		}
		  
		[XmlAttribute("BannerUrl")]
		[Bindable(true)]
		public string BannerUrl 
		{
			get { return GetColumnValue<string>(Columns.BannerUrl); }
			set { SetColumnValue(Columns.BannerUrl, value); }
		}
		  
		[XmlAttribute("BannerClickUrl")]
		[Bindable(true)]
		public string BannerClickUrl 
		{
			get { return GetColumnValue<string>(Columns.BannerClickUrl); }
			set { SetColumnValue(Columns.BannerClickUrl, value); }
		}
		  
		[XmlAttribute("DtStartDate")]
		[Bindable(true)]
		public DateTime? DtStartDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStartDate); }
			set { SetColumnValue(Columns.DtStartDate, value); }
		}
		  
		[XmlAttribute("DtEndDate")]
		[Bindable(true)]
		public DateTime? DtEndDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtEndDate); }
			set { SetColumnValue(Columns.DtEndDate, value); }
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
		/// Returns a AspnetApplication ActiveRecord object related to this SalePromotion
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
		public static void Insert(DateTime? varDtStamp,Guid varApplicationId,bool varBActive,int varIBannerTimeoutMsecs,string varName,string varDisplayText,string varAdditionalText,string varBannerUrl,string varBannerClickUrl,DateTime? varDtStartDate,DateTime? varDtEndDate,string varVcPrincipal,string varVcJsonOrdinal)
		{
			SalePromotion item = new SalePromotion();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.BActive = varBActive;
			
			item.IBannerTimeoutMsecs = varIBannerTimeoutMsecs;
			
			item.Name = varName;
			
			item.DisplayText = varDisplayText;
			
			item.AdditionalText = varAdditionalText;
			
			item.BannerUrl = varBannerUrl;
			
			item.BannerClickUrl = varBannerClickUrl;
			
			item.DtStartDate = varDtStartDate;
			
			item.DtEndDate = varDtEndDate;
			
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
		public static void Update(int varId,DateTime? varDtStamp,Guid varApplicationId,bool varBActive,int varIBannerTimeoutMsecs,string varName,string varDisplayText,string varAdditionalText,string varBannerUrl,string varBannerClickUrl,DateTime? varDtStartDate,DateTime? varDtEndDate,string varVcPrincipal,string varVcJsonOrdinal)
		{
			SalePromotion item = new SalePromotion();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.BActive = varBActive;
			
				item.IBannerTimeoutMsecs = varIBannerTimeoutMsecs;
			
				item.Name = varName;
			
				item.DisplayText = varDisplayText;
			
				item.AdditionalText = varAdditionalText;
			
				item.BannerUrl = varBannerUrl;
			
				item.BannerClickUrl = varBannerClickUrl;
			
				item.DtStartDate = varDtStartDate;
			
				item.DtEndDate = varDtEndDate;
			
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
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IBannerTimeoutMsecsColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayTextColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn AdditionalTextColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn BannerUrlColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn BannerClickUrlColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStartDateColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn DtEndDateColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn VcPrincipalColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn VcJsonOrdinalColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string BActive = @"bActive";
			 public static string IBannerTimeoutMsecs = @"iBannerTimeoutMsecs";
			 public static string Name = @"Name";
			 public static string DisplayText = @"DisplayText";
			 public static string AdditionalText = @"AdditionalText";
			 public static string BannerUrl = @"BannerUrl";
			 public static string BannerClickUrl = @"BannerClickUrl";
			 public static string DtStartDate = @"dtStartDate";
			 public static string DtEndDate = @"dtEndDate";
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
