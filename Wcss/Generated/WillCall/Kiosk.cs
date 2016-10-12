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
	/// Strongly-typed collection for the Kiosk class.
	/// </summary>
    [Serializable]
	public partial class KioskCollection : ActiveList<Kiosk, KioskCollection>
	{	   
		public KioskCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>KioskCollection</returns>
		public KioskCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Kiosk o = this[i];
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
	/// This is an ActiveRecord class which wraps the Kiosk table.
	/// </summary>
	[Serializable]
	public partial class Kiosk : ActiveRecord<Kiosk>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Kiosk()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Kiosk(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Kiosk(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Kiosk(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Kiosk", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarDtStamp.IsNullable = false;
				colvarDtStamp.IsPrimaryKey = false;
				colvarDtStamp.IsForeignKey = false;
				colvarDtStamp.IsReadOnly = false;
				
						colvarDtStamp.DefaultSetting = @"(getdate())";
				colvarDtStamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStamp);
				
				TableSchema.TableColumn colvarBActive = new TableSchema.TableColumn(schema);
				colvarBActive.ColumnName = "bActive";
				colvarBActive.DataType = DbType.Boolean;
				colvarBActive.MaxLength = 0;
				colvarBActive.AutoIncrement = false;
				colvarBActive.IsNullable = false;
				colvarBActive.IsPrimaryKey = false;
				colvarBActive.IsForeignKey = false;
				colvarBActive.IsReadOnly = false;
				
						colvarBActive.DefaultSetting = @"((0))";
				colvarBActive.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBActive);
				
				TableSchema.TableColumn colvarITimeoutMsecs = new TableSchema.TableColumn(schema);
				colvarITimeoutMsecs.ColumnName = "iTimeoutMsecs";
				colvarITimeoutMsecs.DataType = DbType.Int32;
				colvarITimeoutMsecs.MaxLength = 0;
				colvarITimeoutMsecs.AutoIncrement = false;
				colvarITimeoutMsecs.IsNullable = false;
				colvarITimeoutMsecs.IsPrimaryKey = false;
				colvarITimeoutMsecs.IsForeignKey = false;
				colvarITimeoutMsecs.IsReadOnly = false;
				
						colvarITimeoutMsecs.DefaultSetting = @"((6000))";
				colvarITimeoutMsecs.ForeignKeyTableName = "";
				schema.Columns.Add(colvarITimeoutMsecs);
				
				TableSchema.TableColumn colvarTShowId = new TableSchema.TableColumn(schema);
				colvarTShowId.ColumnName = "TShowId";
				colvarTShowId.DataType = DbType.Int32;
				colvarTShowId.MaxLength = 0;
				colvarTShowId.AutoIncrement = false;
				colvarTShowId.IsNullable = true;
				colvarTShowId.IsPrimaryKey = false;
				colvarTShowId.IsForeignKey = true;
				colvarTShowId.IsReadOnly = false;
				colvarTShowId.DefaultSetting = @"";
				
					colvarTShowId.ForeignKeyTableName = "Show";
				schema.Columns.Add(colvarTShowId);
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.AnsiString;
				colvarName.MaxLength = 500;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = false;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarDisplayUrl = new TableSchema.TableColumn(schema);
				colvarDisplayUrl.ColumnName = "DisplayUrl";
				colvarDisplayUrl.DataType = DbType.AnsiString;
				colvarDisplayUrl.MaxLength = 500;
				colvarDisplayUrl.AutoIncrement = false;
				colvarDisplayUrl.IsNullable = true;
				colvarDisplayUrl.IsPrimaryKey = false;
				colvarDisplayUrl.IsForeignKey = false;
				colvarDisplayUrl.IsReadOnly = false;
				colvarDisplayUrl.DefaultSetting = @"";
				colvarDisplayUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayUrl);
				
				TableSchema.TableColumn colvarIPicWidth = new TableSchema.TableColumn(schema);
				colvarIPicWidth.ColumnName = "iPicWidth";
				colvarIPicWidth.DataType = DbType.Int32;
				colvarIPicWidth.MaxLength = 0;
				colvarIPicWidth.AutoIncrement = false;
				colvarIPicWidth.IsNullable = false;
				colvarIPicWidth.IsPrimaryKey = false;
				colvarIPicWidth.IsForeignKey = false;
				colvarIPicWidth.IsReadOnly = false;
				
						colvarIPicWidth.DefaultSetting = @"((0))";
				colvarIPicWidth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIPicWidth);
				
				TableSchema.TableColumn colvarIPicHeight = new TableSchema.TableColumn(schema);
				colvarIPicHeight.ColumnName = "iPicHeight";
				colvarIPicHeight.DataType = DbType.Int32;
				colvarIPicHeight.MaxLength = 0;
				colvarIPicHeight.AutoIncrement = false;
				colvarIPicHeight.IsNullable = false;
				colvarIPicHeight.IsPrimaryKey = false;
				colvarIPicHeight.IsForeignKey = false;
				colvarIPicHeight.IsReadOnly = false;
				
						colvarIPicHeight.DefaultSetting = @"((0))";
				colvarIPicHeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIPicHeight);
				
				TableSchema.TableColumn colvarBCtrX = new TableSchema.TableColumn(schema);
				colvarBCtrX.ColumnName = "bCtrX";
				colvarBCtrX.DataType = DbType.Boolean;
				colvarBCtrX.MaxLength = 0;
				colvarBCtrX.AutoIncrement = false;
				colvarBCtrX.IsNullable = false;
				colvarBCtrX.IsPrimaryKey = false;
				colvarBCtrX.IsForeignKey = false;
				colvarBCtrX.IsReadOnly = false;
				
						colvarBCtrX.DefaultSetting = @"((1))";
				colvarBCtrX.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBCtrX);
				
				TableSchema.TableColumn colvarBCtrY = new TableSchema.TableColumn(schema);
				colvarBCtrY.ColumnName = "bCtrY";
				colvarBCtrY.DataType = DbType.Boolean;
				colvarBCtrY.MaxLength = 0;
				colvarBCtrY.AutoIncrement = false;
				colvarBCtrY.IsNullable = false;
				colvarBCtrY.IsPrimaryKey = false;
				colvarBCtrY.IsForeignKey = false;
				colvarBCtrY.IsReadOnly = false;
				
						colvarBCtrY.DefaultSetting = @"((1))";
				colvarBCtrY.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBCtrY);
				
				TableSchema.TableColumn colvarEventVenue = new TableSchema.TableColumn(schema);
				colvarEventVenue.ColumnName = "EventVenue";
				colvarEventVenue.DataType = DbType.AnsiString;
				colvarEventVenue.MaxLength = 500;
				colvarEventVenue.AutoIncrement = false;
				colvarEventVenue.IsNullable = true;
				colvarEventVenue.IsPrimaryKey = false;
				colvarEventVenue.IsForeignKey = false;
				colvarEventVenue.IsReadOnly = false;
				colvarEventVenue.DefaultSetting = @"";
				colvarEventVenue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEventVenue);
				
				TableSchema.TableColumn colvarEventDate = new TableSchema.TableColumn(schema);
				colvarEventDate.ColumnName = "EventDate";
				colvarEventDate.DataType = DbType.AnsiString;
				colvarEventDate.MaxLength = 500;
				colvarEventDate.AutoIncrement = false;
				colvarEventDate.IsNullable = true;
				colvarEventDate.IsPrimaryKey = false;
				colvarEventDate.IsForeignKey = false;
				colvarEventDate.IsReadOnly = false;
				colvarEventDate.DefaultSetting = @"";
				colvarEventDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEventDate);
				
				TableSchema.TableColumn colvarEventTitle = new TableSchema.TableColumn(schema);
				colvarEventTitle.ColumnName = "EventTitle";
				colvarEventTitle.DataType = DbType.AnsiString;
				colvarEventTitle.MaxLength = 500;
				colvarEventTitle.AutoIncrement = false;
				colvarEventTitle.IsNullable = true;
				colvarEventTitle.IsPrimaryKey = false;
				colvarEventTitle.IsForeignKey = false;
				colvarEventTitle.IsReadOnly = false;
				colvarEventTitle.DefaultSetting = @"";
				colvarEventTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEventTitle);
				
				TableSchema.TableColumn colvarEventHeads = new TableSchema.TableColumn(schema);
				colvarEventHeads.ColumnName = "EventHeads";
				colvarEventHeads.DataType = DbType.AnsiString;
				colvarEventHeads.MaxLength = 500;
				colvarEventHeads.AutoIncrement = false;
				colvarEventHeads.IsNullable = true;
				colvarEventHeads.IsPrimaryKey = false;
				colvarEventHeads.IsForeignKey = false;
				colvarEventHeads.IsReadOnly = false;
				colvarEventHeads.DefaultSetting = @"";
				colvarEventHeads.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEventHeads);
				
				TableSchema.TableColumn colvarEventOpeners = new TableSchema.TableColumn(schema);
				colvarEventOpeners.ColumnName = "EventOpeners";
				colvarEventOpeners.DataType = DbType.AnsiString;
				colvarEventOpeners.MaxLength = 500;
				colvarEventOpeners.AutoIncrement = false;
				colvarEventOpeners.IsNullable = true;
				colvarEventOpeners.IsPrimaryKey = false;
				colvarEventOpeners.IsForeignKey = false;
				colvarEventOpeners.IsReadOnly = false;
				colvarEventOpeners.DefaultSetting = @"";
				colvarEventOpeners.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEventOpeners);
				
				TableSchema.TableColumn colvarEventDescription = new TableSchema.TableColumn(schema);
				colvarEventDescription.ColumnName = "EventDescription";
				colvarEventDescription.DataType = DbType.AnsiString;
				colvarEventDescription.MaxLength = 2000;
				colvarEventDescription.AutoIncrement = false;
				colvarEventDescription.IsNullable = true;
				colvarEventDescription.IsPrimaryKey = false;
				colvarEventDescription.IsForeignKey = false;
				colvarEventDescription.IsReadOnly = false;
				colvarEventDescription.DefaultSetting = @"";
				colvarEventDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEventDescription);
				
				TableSchema.TableColumn colvarTextCss = new TableSchema.TableColumn(schema);
				colvarTextCss.ColumnName = "TextCss";
				colvarTextCss.DataType = DbType.AnsiString;
				colvarTextCss.MaxLength = 256;
				colvarTextCss.AutoIncrement = false;
				colvarTextCss.IsNullable = true;
				colvarTextCss.IsPrimaryKey = false;
				colvarTextCss.IsForeignKey = false;
				colvarTextCss.IsReadOnly = false;
				colvarTextCss.DefaultSetting = @"";
				colvarTextCss.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTextCss);
				
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
				DataService.Providers["WillCall"].AddSchema("Kiosk",schema);
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
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("ITimeoutMsecs")]
		[Bindable(true)]
		public int ITimeoutMsecs 
		{
			get { return GetColumnValue<int>(Columns.ITimeoutMsecs); }
			set { SetColumnValue(Columns.ITimeoutMsecs, value); }
		}
		  
		[XmlAttribute("TShowId")]
		[Bindable(true)]
		public int? TShowId 
		{
			get { return GetColumnValue<int?>(Columns.TShowId); }
			set { SetColumnValue(Columns.TShowId, value); }
		}
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("DisplayUrl")]
		[Bindable(true)]
		public string DisplayUrl 
		{
			get { return GetColumnValue<string>(Columns.DisplayUrl); }
			set { SetColumnValue(Columns.DisplayUrl, value); }
		}
		  
		[XmlAttribute("IPicWidth")]
		[Bindable(true)]
		public int IPicWidth 
		{
			get { return GetColumnValue<int>(Columns.IPicWidth); }
			set { SetColumnValue(Columns.IPicWidth, value); }
		}
		  
		[XmlAttribute("IPicHeight")]
		[Bindable(true)]
		public int IPicHeight 
		{
			get { return GetColumnValue<int>(Columns.IPicHeight); }
			set { SetColumnValue(Columns.IPicHeight, value); }
		}
		  
		[XmlAttribute("BCtrX")]
		[Bindable(true)]
		public bool BCtrX 
		{
			get { return GetColumnValue<bool>(Columns.BCtrX); }
			set { SetColumnValue(Columns.BCtrX, value); }
		}
		  
		[XmlAttribute("BCtrY")]
		[Bindable(true)]
		public bool BCtrY 
		{
			get { return GetColumnValue<bool>(Columns.BCtrY); }
			set { SetColumnValue(Columns.BCtrY, value); }
		}
		  
		[XmlAttribute("EventVenue")]
		[Bindable(true)]
		public string EventVenue 
		{
			get { return GetColumnValue<string>(Columns.EventVenue); }
			set { SetColumnValue(Columns.EventVenue, value); }
		}
		  
		[XmlAttribute("EventDate")]
		[Bindable(true)]
		public string EventDate 
		{
			get { return GetColumnValue<string>(Columns.EventDate); }
			set { SetColumnValue(Columns.EventDate, value); }
		}
		  
		[XmlAttribute("EventTitle")]
		[Bindable(true)]
		public string EventTitle 
		{
			get { return GetColumnValue<string>(Columns.EventTitle); }
			set { SetColumnValue(Columns.EventTitle, value); }
		}
		  
		[XmlAttribute("EventHeads")]
		[Bindable(true)]
		public string EventHeads 
		{
			get { return GetColumnValue<string>(Columns.EventHeads); }
			set { SetColumnValue(Columns.EventHeads, value); }
		}
		  
		[XmlAttribute("EventOpeners")]
		[Bindable(true)]
		public string EventOpeners 
		{
			get { return GetColumnValue<string>(Columns.EventOpeners); }
			set { SetColumnValue(Columns.EventOpeners, value); }
		}
		  
		[XmlAttribute("EventDescription")]
		[Bindable(true)]
		public string EventDescription 
		{
			get { return GetColumnValue<string>(Columns.EventDescription); }
			set { SetColumnValue(Columns.EventDescription, value); }
		}
		  
		[XmlAttribute("TextCss")]
		[Bindable(true)]
		public string TextCss 
		{
			get { return GetColumnValue<string>(Columns.TextCss); }
			set { SetColumnValue(Columns.TextCss, value); }
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
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
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
		/// Returns a AspnetApplication ActiveRecord object related to this Kiosk
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
		
		
		/// <summary>
		/// Returns a Show ActiveRecord object related to this Kiosk
		/// 
		/// </summary>
		private Wcss.Show Show
		{
			get { return Wcss.Show.FetchByID(this.TShowId); }
			set { SetColumnValue("TShowId", value.Id); }
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
                
                SetColumnValue("TShowId", value.Id);
                _showrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,bool varBActive,int varITimeoutMsecs,int? varTShowId,string varName,string varDisplayUrl,int varIPicWidth,int varIPicHeight,bool varBCtrX,bool varBCtrY,string varEventVenue,string varEventDate,string varEventTitle,string varEventHeads,string varEventOpeners,string varEventDescription,string varTextCss,DateTime? varDtStartDate,DateTime? varDtEndDate,Guid varApplicationId,string varVcPrincipal,string varVcJsonOrdinal)
		{
			Kiosk item = new Kiosk();
			
			item.DtStamp = varDtStamp;
			
			item.BActive = varBActive;
			
			item.ITimeoutMsecs = varITimeoutMsecs;
			
			item.TShowId = varTShowId;
			
			item.Name = varName;
			
			item.DisplayUrl = varDisplayUrl;
			
			item.IPicWidth = varIPicWidth;
			
			item.IPicHeight = varIPicHeight;
			
			item.BCtrX = varBCtrX;
			
			item.BCtrY = varBCtrY;
			
			item.EventVenue = varEventVenue;
			
			item.EventDate = varEventDate;
			
			item.EventTitle = varEventTitle;
			
			item.EventHeads = varEventHeads;
			
			item.EventOpeners = varEventOpeners;
			
			item.EventDescription = varEventDescription;
			
			item.TextCss = varTextCss;
			
			item.DtStartDate = varDtStartDate;
			
			item.DtEndDate = varDtEndDate;
			
			item.ApplicationId = varApplicationId;
			
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
		public static void Update(int varId,DateTime varDtStamp,bool varBActive,int varITimeoutMsecs,int? varTShowId,string varName,string varDisplayUrl,int varIPicWidth,int varIPicHeight,bool varBCtrX,bool varBCtrY,string varEventVenue,string varEventDate,string varEventTitle,string varEventHeads,string varEventOpeners,string varEventDescription,string varTextCss,DateTime? varDtStartDate,DateTime? varDtEndDate,Guid varApplicationId,string varVcPrincipal,string varVcJsonOrdinal)
		{
			Kiosk item = new Kiosk();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.BActive = varBActive;
			
				item.ITimeoutMsecs = varITimeoutMsecs;
			
				item.TShowId = varTShowId;
			
				item.Name = varName;
			
				item.DisplayUrl = varDisplayUrl;
			
				item.IPicWidth = varIPicWidth;
			
				item.IPicHeight = varIPicHeight;
			
				item.BCtrX = varBCtrX;
			
				item.BCtrY = varBCtrY;
			
				item.EventVenue = varEventVenue;
			
				item.EventDate = varEventDate;
			
				item.EventTitle = varEventTitle;
			
				item.EventHeads = varEventHeads;
			
				item.EventOpeners = varEventOpeners;
			
				item.EventDescription = varEventDescription;
			
				item.TextCss = varTextCss;
			
				item.DtStartDate = varDtStartDate;
			
				item.DtEndDate = varDtEndDate;
			
				item.ApplicationId = varApplicationId;
			
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
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ITimeoutMsecsColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayUrlColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn IPicWidthColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn IPicHeightColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn BCtrXColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn BCtrYColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn EventVenueColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn EventDateColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn EventTitleColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn EventHeadsColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn EventOpenersColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn EventDescriptionColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn TextCssColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStartDateColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn DtEndDateColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn VcPrincipalColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn VcJsonOrdinalColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string BActive = @"bActive";
			 public static string ITimeoutMsecs = @"iTimeoutMsecs";
			 public static string TShowId = @"TShowId";
			 public static string Name = @"Name";
			 public static string DisplayUrl = @"DisplayUrl";
			 public static string IPicWidth = @"iPicWidth";
			 public static string IPicHeight = @"iPicHeight";
			 public static string BCtrX = @"bCtrX";
			 public static string BCtrY = @"bCtrY";
			 public static string EventVenue = @"EventVenue";
			 public static string EventDate = @"EventDate";
			 public static string EventTitle = @"EventTitle";
			 public static string EventHeads = @"EventHeads";
			 public static string EventOpeners = @"EventOpeners";
			 public static string EventDescription = @"EventDescription";
			 public static string TextCss = @"TextCss";
			 public static string DtStartDate = @"dtStartDate";
			 public static string DtEndDate = @"dtEndDate";
			 public static string ApplicationId = @"ApplicationId";
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
