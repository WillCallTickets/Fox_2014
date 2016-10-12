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
	/// Strongly-typed collection for the Show class.
	/// </summary>
    [Serializable]
	public partial class ShowCollection : ActiveList<Show, ShowCollection>
	{	   
		public ShowCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ShowCollection</returns>
		public ShowCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Show o = this[i];
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
	/// This is an ActiveRecord class which wraps the Show table.
	/// </summary>
	[Serializable]
	public partial class Show : ActiveRecord<Show>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Show()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Show(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Show(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Show(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Show", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.AnsiString;
				colvarName.MaxLength = 300;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = false;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarDtAnnounceDate = new TableSchema.TableColumn(schema);
				colvarDtAnnounceDate.ColumnName = "dtAnnounceDate";
				colvarDtAnnounceDate.DataType = DbType.DateTime;
				colvarDtAnnounceDate.MaxLength = 0;
				colvarDtAnnounceDate.AutoIncrement = false;
				colvarDtAnnounceDate.IsNullable = true;
				colvarDtAnnounceDate.IsPrimaryKey = false;
				colvarDtAnnounceDate.IsForeignKey = false;
				colvarDtAnnounceDate.IsReadOnly = false;
				colvarDtAnnounceDate.DefaultSetting = @"";
				colvarDtAnnounceDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtAnnounceDate);
				
				TableSchema.TableColumn colvarDtDateOnSale = new TableSchema.TableColumn(schema);
				colvarDtDateOnSale.ColumnName = "dtDateOnSale";
				colvarDtDateOnSale.DataType = DbType.DateTime;
				colvarDtDateOnSale.MaxLength = 0;
				colvarDtDateOnSale.AutoIncrement = false;
				colvarDtDateOnSale.IsNullable = true;
				colvarDtDateOnSale.IsPrimaryKey = false;
				colvarDtDateOnSale.IsForeignKey = false;
				colvarDtDateOnSale.IsReadOnly = false;
				colvarDtDateOnSale.DefaultSetting = @"";
				colvarDtDateOnSale.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtDateOnSale);
				
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
				
				TableSchema.TableColumn colvarBSoldOut = new TableSchema.TableColumn(schema);
				colvarBSoldOut.ColumnName = "bSoldOut";
				colvarBSoldOut.DataType = DbType.Boolean;
				colvarBSoldOut.MaxLength = 0;
				colvarBSoldOut.AutoIncrement = false;
				colvarBSoldOut.IsNullable = false;
				colvarBSoldOut.IsPrimaryKey = false;
				colvarBSoldOut.IsForeignKey = false;
				colvarBSoldOut.IsReadOnly = false;
				
						colvarBSoldOut.DefaultSetting = @"((0))";
				colvarBSoldOut.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBSoldOut);
				
				TableSchema.TableColumn colvarShowAlert = new TableSchema.TableColumn(schema);
				colvarShowAlert.ColumnName = "ShowAlert";
				colvarShowAlert.DataType = DbType.AnsiString;
				colvarShowAlert.MaxLength = 500;
				colvarShowAlert.AutoIncrement = false;
				colvarShowAlert.IsNullable = true;
				colvarShowAlert.IsPrimaryKey = false;
				colvarShowAlert.IsForeignKey = false;
				colvarShowAlert.IsReadOnly = false;
				colvarShowAlert.DefaultSetting = @"";
				colvarShowAlert.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowAlert);
				
				TableSchema.TableColumn colvarTVenueId = new TableSchema.TableColumn(schema);
				colvarTVenueId.ColumnName = "TVenueId";
				colvarTVenueId.DataType = DbType.Int32;
				colvarTVenueId.MaxLength = 0;
				colvarTVenueId.AutoIncrement = false;
				colvarTVenueId.IsNullable = false;
				colvarTVenueId.IsPrimaryKey = false;
				colvarTVenueId.IsForeignKey = true;
				colvarTVenueId.IsReadOnly = false;
				
						colvarTVenueId.DefaultSetting = @"((10000))";
				
					colvarTVenueId.ForeignKeyTableName = "Venue";
				schema.Columns.Add(colvarTVenueId);
				
				TableSchema.TableColumn colvarDisplayNotes = new TableSchema.TableColumn(schema);
				colvarDisplayNotes.ColumnName = "DisplayNotes";
				colvarDisplayNotes.DataType = DbType.AnsiString;
				colvarDisplayNotes.MaxLength = 1000;
				colvarDisplayNotes.AutoIncrement = false;
				colvarDisplayNotes.IsNullable = true;
				colvarDisplayNotes.IsPrimaryKey = false;
				colvarDisplayNotes.IsForeignKey = false;
				colvarDisplayNotes.IsReadOnly = false;
				colvarDisplayNotes.DefaultSetting = @"";
				colvarDisplayNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayNotes);
				
				TableSchema.TableColumn colvarShowTitle = new TableSchema.TableColumn(schema);
				colvarShowTitle.ColumnName = "ShowTitle";
				colvarShowTitle.DataType = DbType.AnsiString;
				colvarShowTitle.MaxLength = 300;
				colvarShowTitle.AutoIncrement = false;
				colvarShowTitle.IsNullable = true;
				colvarShowTitle.IsPrimaryKey = false;
				colvarShowTitle.IsForeignKey = false;
				colvarShowTitle.IsReadOnly = false;
				colvarShowTitle.DefaultSetting = @"";
				colvarShowTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowTitle);
				
				TableSchema.TableColumn colvarDisplayUrl = new TableSchema.TableColumn(schema);
				colvarDisplayUrl.ColumnName = "DisplayUrl";
				colvarDisplayUrl.DataType = DbType.AnsiString;
				colvarDisplayUrl.MaxLength = 300;
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
				
				TableSchema.TableColumn colvarShowHeader = new TableSchema.TableColumn(schema);
				colvarShowHeader.ColumnName = "ShowHeader";
				colvarShowHeader.DataType = DbType.AnsiString;
				colvarShowHeader.MaxLength = 300;
				colvarShowHeader.AutoIncrement = false;
				colvarShowHeader.IsNullable = true;
				colvarShowHeader.IsPrimaryKey = false;
				colvarShowHeader.IsForeignKey = false;
				colvarShowHeader.IsReadOnly = false;
				colvarShowHeader.DefaultSetting = @"";
				colvarShowHeader.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowHeader);
				
				TableSchema.TableColumn colvarShowWriteup = new TableSchema.TableColumn(schema);
				colvarShowWriteup.ColumnName = "ShowWriteup";
				colvarShowWriteup.DataType = DbType.AnsiString;
				colvarShowWriteup.MaxLength = -1;
				colvarShowWriteup.AutoIncrement = false;
				colvarShowWriteup.IsNullable = true;
				colvarShowWriteup.IsPrimaryKey = false;
				colvarShowWriteup.IsForeignKey = false;
				colvarShowWriteup.IsReadOnly = false;
				colvarShowWriteup.DefaultSetting = @"";
				colvarShowWriteup.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowWriteup);
				
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
				
				TableSchema.TableColumn colvarFacebookEventUrl = new TableSchema.TableColumn(schema);
				colvarFacebookEventUrl.ColumnName = "FacebookEventUrl";
				colvarFacebookEventUrl.DataType = DbType.AnsiString;
				colvarFacebookEventUrl.MaxLength = 256;
				colvarFacebookEventUrl.AutoIncrement = false;
				colvarFacebookEventUrl.IsNullable = true;
				colvarFacebookEventUrl.IsPrimaryKey = false;
				colvarFacebookEventUrl.IsForeignKey = false;
				colvarFacebookEventUrl.IsReadOnly = false;
				colvarFacebookEventUrl.DefaultSetting = @"";
				colvarFacebookEventUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFacebookEventUrl);
				
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
				
				TableSchema.TableColumn colvarVcJustAnnouncedStatus = new TableSchema.TableColumn(schema);
				colvarVcJustAnnouncedStatus.ColumnName = "vcJustAnnouncedStatus";
				colvarVcJustAnnouncedStatus.DataType = DbType.AnsiString;
				colvarVcJustAnnouncedStatus.MaxLength = 25;
				colvarVcJustAnnouncedStatus.AutoIncrement = false;
				colvarVcJustAnnouncedStatus.IsNullable = true;
				colvarVcJustAnnouncedStatus.IsPrimaryKey = false;
				colvarVcJustAnnouncedStatus.IsForeignKey = false;
				colvarVcJustAnnouncedStatus.IsReadOnly = false;
				
						colvarVcJustAnnouncedStatus.DefaultSetting = @"(NULL)";
				colvarVcJustAnnouncedStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcJustAnnouncedStatus);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Show",schema);
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
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("DtAnnounceDate")]
		[Bindable(true)]
		public DateTime? DtAnnounceDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtAnnounceDate); }
			set { SetColumnValue(Columns.DtAnnounceDate, value); }
		}
		  
		[XmlAttribute("DtDateOnSale")]
		[Bindable(true)]
		public DateTime? DtDateOnSale 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtDateOnSale); }
			set { SetColumnValue(Columns.DtDateOnSale, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("BSoldOut")]
		[Bindable(true)]
		public bool BSoldOut 
		{
			get { return GetColumnValue<bool>(Columns.BSoldOut); }
			set { SetColumnValue(Columns.BSoldOut, value); }
		}
		  
		[XmlAttribute("ShowAlert")]
		[Bindable(true)]
		public string ShowAlert 
		{
			get { return GetColumnValue<string>(Columns.ShowAlert); }
			set { SetColumnValue(Columns.ShowAlert, value); }
		}
		  
		[XmlAttribute("TVenueId")]
		[Bindable(true)]
		public int TVenueId 
		{
			get { return GetColumnValue<int>(Columns.TVenueId); }
			set { SetColumnValue(Columns.TVenueId, value); }
		}
		  
		[XmlAttribute("DisplayNotes")]
		[Bindable(true)]
		public string DisplayNotes 
		{
			get { return GetColumnValue<string>(Columns.DisplayNotes); }
			set { SetColumnValue(Columns.DisplayNotes, value); }
		}
		  
		[XmlAttribute("ShowTitle")]
		[Bindable(true)]
		public string ShowTitle 
		{
			get { return GetColumnValue<string>(Columns.ShowTitle); }
			set { SetColumnValue(Columns.ShowTitle, value); }
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
		  
		[XmlAttribute("ShowHeader")]
		[Bindable(true)]
		public string ShowHeader 
		{
			get { return GetColumnValue<string>(Columns.ShowHeader); }
			set { SetColumnValue(Columns.ShowHeader, value); }
		}
		  
		[XmlAttribute("ShowWriteup")]
		[Bindable(true)]
		public string ShowWriteup 
		{
			get { return GetColumnValue<string>(Columns.ShowWriteup); }
			set { SetColumnValue(Columns.ShowWriteup, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
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
		  
		[XmlAttribute("FacebookEventUrl")]
		[Bindable(true)]
		public string FacebookEventUrl 
		{
			get { return GetColumnValue<string>(Columns.FacebookEventUrl); }
			set { SetColumnValue(Columns.FacebookEventUrl, value); }
		}
		  
		[XmlAttribute("VcPrincipal")]
		[Bindable(true)]
		public string VcPrincipal 
		{
			get { return GetColumnValue<string>(Columns.VcPrincipal); }
			set { SetColumnValue(Columns.VcPrincipal, value); }
		}
		  
		[XmlAttribute("VcJustAnnouncedStatus")]
		[Bindable(true)]
		public string VcJustAnnouncedStatus 
		{
			get { return GetColumnValue<string>(Columns.VcJustAnnouncedStatus); }
			set { SetColumnValue(Columns.VcJustAnnouncedStatus, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.JShowPromoterCollection colJShowPromoterRecords;
		public Wcss.JShowPromoterCollection JShowPromoterRecords()
		{
			if(colJShowPromoterRecords == null)
			{
				colJShowPromoterRecords = new Wcss.JShowPromoterCollection().Where(JShowPromoter.Columns.TShowId, Id).Load();
				colJShowPromoterRecords.ListChanged += new ListChangedEventHandler(colJShowPromoterRecords_ListChanged);
			}
			return colJShowPromoterRecords;
		}
				
		void colJShowPromoterRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colJShowPromoterRecords[e.NewIndex].TShowId = Id;
				colJShowPromoterRecords.ListChanged += new ListChangedEventHandler(colJShowPromoterRecords_ListChanged);
            }
		}
		private Wcss.KioskCollection colKioskRecords;
		public Wcss.KioskCollection KioskRecords()
		{
			if(colKioskRecords == null)
			{
				colKioskRecords = new Wcss.KioskCollection().Where(Kiosk.Columns.TShowId, Id).Load();
				colKioskRecords.ListChanged += new ListChangedEventHandler(colKioskRecords_ListChanged);
			}
			return colKioskRecords;
		}
				
		void colKioskRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colKioskRecords[e.NewIndex].TShowId = Id;
				colKioskRecords.ListChanged += new ListChangedEventHandler(colKioskRecords_ListChanged);
            }
		}
		private Wcss.ShowDateCollection colShowDateRecords;
		public Wcss.ShowDateCollection ShowDateRecords()
		{
			if(colShowDateRecords == null)
			{
				colShowDateRecords = new Wcss.ShowDateCollection().Where(ShowDate.Columns.TShowId, Id).Load();
				colShowDateRecords.ListChanged += new ListChangedEventHandler(colShowDateRecords_ListChanged);
			}
			return colShowDateRecords;
		}
				
		void colShowDateRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowDateRecords[e.NewIndex].TShowId = Id;
				colShowDateRecords.ListChanged += new ListChangedEventHandler(colShowDateRecords_ListChanged);
            }
		}
		private Wcss.ShowGenreCollection colShowGenreRecords;
		public Wcss.ShowGenreCollection ShowGenreRecords()
		{
			if(colShowGenreRecords == null)
			{
				colShowGenreRecords = new Wcss.ShowGenreCollection().Where(ShowGenre.Columns.TShowId, Id).Load();
				colShowGenreRecords.ListChanged += new ListChangedEventHandler(colShowGenreRecords_ListChanged);
			}
			return colShowGenreRecords;
		}
				
		void colShowGenreRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowGenreRecords[e.NewIndex].TShowId = Id;
				colShowGenreRecords.ListChanged += new ListChangedEventHandler(colShowGenreRecords_ListChanged);
            }
		}
		private Wcss.ShowLinkCollection colShowLinkRecords;
		public Wcss.ShowLinkCollection ShowLinkRecords()
		{
			if(colShowLinkRecords == null)
			{
				colShowLinkRecords = new Wcss.ShowLinkCollection().Where(ShowLink.Columns.TShowId, Id).Load();
				colShowLinkRecords.ListChanged += new ListChangedEventHandler(colShowLinkRecords_ListChanged);
			}
			return colShowLinkRecords;
		}
				
		void colShowLinkRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowLinkRecords[e.NewIndex].TShowId = Id;
				colShowLinkRecords.ListChanged += new ListChangedEventHandler(colShowLinkRecords_ListChanged);
            }
		}
		private Wcss.VdShowExpenseCollection colVdShowExpenseRecords;
		public Wcss.VdShowExpenseCollection VdShowExpenseRecords()
		{
			if(colVdShowExpenseRecords == null)
			{
				colVdShowExpenseRecords = new Wcss.VdShowExpenseCollection().Where(VdShowExpense.Columns.TShowId, Id).Load();
				colVdShowExpenseRecords.ListChanged += new ListChangedEventHandler(colVdShowExpenseRecords_ListChanged);
			}
			return colVdShowExpenseRecords;
		}
				
		void colVdShowExpenseRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colVdShowExpenseRecords[e.NewIndex].TShowId = Id;
				colVdShowExpenseRecords.ListChanged += new ListChangedEventHandler(colVdShowExpenseRecords_ListChanged);
            }
		}
		private Wcss.VdShowGenreCollection colVdShowGenreRecords;
		public Wcss.VdShowGenreCollection VdShowGenreRecords()
		{
			if(colVdShowGenreRecords == null)
			{
				colVdShowGenreRecords = new Wcss.VdShowGenreCollection().Where(VdShowGenre.Columns.TShowId, Id).Load();
				colVdShowGenreRecords.ListChanged += new ListChangedEventHandler(colVdShowGenreRecords_ListChanged);
			}
			return colVdShowGenreRecords;
		}
				
		void colVdShowGenreRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colVdShowGenreRecords[e.NewIndex].TShowId = Id;
				colVdShowGenreRecords.ListChanged += new ListChangedEventHandler(colVdShowGenreRecords_ListChanged);
            }
		}
		private Wcss.VdShowInfoCollection colVdShowInfoRecords;
		public Wcss.VdShowInfoCollection VdShowInfoRecords()
		{
			if(colVdShowInfoRecords == null)
			{
				colVdShowInfoRecords = new Wcss.VdShowInfoCollection().Where(VdShowInfo.Columns.TShowId, Id).Load();
				colVdShowInfoRecords.ListChanged += new ListChangedEventHandler(colVdShowInfoRecords_ListChanged);
			}
			return colVdShowInfoRecords;
		}
				
		void colVdShowInfoRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colVdShowInfoRecords[e.NewIndex].TShowId = Id;
				colVdShowInfoRecords.ListChanged += new ListChangedEventHandler(colVdShowInfoRecords_ListChanged);
            }
		}
		private Wcss.VdShowTicketCollection colVdShowTicketRecords;
		public Wcss.VdShowTicketCollection VdShowTicketRecords()
		{
			if(colVdShowTicketRecords == null)
			{
				colVdShowTicketRecords = new Wcss.VdShowTicketCollection().Where(VdShowTicket.Columns.TShowId, Id).Load();
				colVdShowTicketRecords.ListChanged += new ListChangedEventHandler(colVdShowTicketRecords_ListChanged);
			}
			return colVdShowTicketRecords;
		}
				
		void colVdShowTicketRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colVdShowTicketRecords[e.NewIndex].TShowId = Id;
				colVdShowTicketRecords.ListChanged += new ListChangedEventHandler(colVdShowTicketRecords_ListChanged);
            }
		}
		private Wcss.VdShowTicketOutletCollection colVdShowTicketOutletRecords;
		public Wcss.VdShowTicketOutletCollection VdShowTicketOutletRecords()
		{
			if(colVdShowTicketOutletRecords == null)
			{
				colVdShowTicketOutletRecords = new Wcss.VdShowTicketOutletCollection().Where(VdShowTicketOutlet.Columns.TShowId, Id).Load();
				colVdShowTicketOutletRecords.ListChanged += new ListChangedEventHandler(colVdShowTicketOutletRecords_ListChanged);
			}
			return colVdShowTicketOutletRecords;
		}
				
		void colVdShowTicketOutletRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colVdShowTicketOutletRecords[e.NewIndex].TShowId = Id;
				colVdShowTicketOutletRecords.ListChanged += new ListChangedEventHandler(colVdShowTicketOutletRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Show
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
		/// Returns a Venue ActiveRecord object related to this Show
		/// 
		/// </summary>
		private Wcss.Venue Venue
		{
			get { return Wcss.Venue.FetchByID(this.TVenueId); }
			set { SetColumnValue("TVenueId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Venue _venuerecord = null;
		
		public Wcss.Venue VenueRecord
		{
		    get
            {
                if (_venuerecord == null)
                {
                    _venuerecord = new Wcss.Venue();
                    _venuerecord.CopyFrom(this.Venue);
                }
                return _venuerecord;
            }
            set
            {
                if(value != null && _venuerecord == null)
			        _venuerecord = new Wcss.Venue();
                
                SetColumnValue("TVenueId", value.Id);
                _venuerecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varName,DateTime? varDtAnnounceDate,DateTime? varDtDateOnSale,bool varBActive,bool varBSoldOut,string varShowAlert,int varTVenueId,string varDisplayNotes,string varShowTitle,string varDisplayUrl,int varIPicWidth,int varIPicHeight,string varShowHeader,string varShowWriteup,DateTime varDtStamp,Guid varApplicationId,bool varBCtrX,bool varBCtrY,string varFacebookEventUrl,string varVcPrincipal,string varVcJustAnnouncedStatus)
		{
			Show item = new Show();
			
			item.Name = varName;
			
			item.DtAnnounceDate = varDtAnnounceDate;
			
			item.DtDateOnSale = varDtDateOnSale;
			
			item.BActive = varBActive;
			
			item.BSoldOut = varBSoldOut;
			
			item.ShowAlert = varShowAlert;
			
			item.TVenueId = varTVenueId;
			
			item.DisplayNotes = varDisplayNotes;
			
			item.ShowTitle = varShowTitle;
			
			item.DisplayUrl = varDisplayUrl;
			
			item.IPicWidth = varIPicWidth;
			
			item.IPicHeight = varIPicHeight;
			
			item.ShowHeader = varShowHeader;
			
			item.ShowWriteup = varShowWriteup;
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.BCtrX = varBCtrX;
			
			item.BCtrY = varBCtrY;
			
			item.FacebookEventUrl = varFacebookEventUrl;
			
			item.VcPrincipal = varVcPrincipal;
			
			item.VcJustAnnouncedStatus = varVcJustAnnouncedStatus;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varName,DateTime? varDtAnnounceDate,DateTime? varDtDateOnSale,bool varBActive,bool varBSoldOut,string varShowAlert,int varTVenueId,string varDisplayNotes,string varShowTitle,string varDisplayUrl,int varIPicWidth,int varIPicHeight,string varShowHeader,string varShowWriteup,DateTime varDtStamp,Guid varApplicationId,bool varBCtrX,bool varBCtrY,string varFacebookEventUrl,string varVcPrincipal,string varVcJustAnnouncedStatus)
		{
			Show item = new Show();
			
				item.Id = varId;
			
				item.Name = varName;
			
				item.DtAnnounceDate = varDtAnnounceDate;
			
				item.DtDateOnSale = varDtDateOnSale;
			
				item.BActive = varBActive;
			
				item.BSoldOut = varBSoldOut;
			
				item.ShowAlert = varShowAlert;
			
				item.TVenueId = varTVenueId;
			
				item.DisplayNotes = varDisplayNotes;
			
				item.ShowTitle = varShowTitle;
			
				item.DisplayUrl = varDisplayUrl;
			
				item.IPicWidth = varIPicWidth;
			
				item.IPicHeight = varIPicHeight;
			
				item.ShowHeader = varShowHeader;
			
				item.ShowWriteup = varShowWriteup;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.BCtrX = varBCtrX;
			
				item.BCtrY = varBCtrY;
			
				item.FacebookEventUrl = varFacebookEventUrl;
			
				item.VcPrincipal = varVcPrincipal;
			
				item.VcJustAnnouncedStatus = varVcJustAnnouncedStatus;
			
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
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DtAnnounceDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DtDateOnSaleColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BSoldOutColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowAlertColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn TVenueIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayNotesColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowTitleColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayUrlColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn IPicWidthColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn IPicHeightColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowHeaderColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowWriteupColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn BCtrXColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn BCtrYColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn FacebookEventUrlColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn VcPrincipalColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn VcJustAnnouncedStatusColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string Name = @"Name";
			 public static string DtAnnounceDate = @"dtAnnounceDate";
			 public static string DtDateOnSale = @"dtDateOnSale";
			 public static string BActive = @"bActive";
			 public static string BSoldOut = @"bSoldOut";
			 public static string ShowAlert = @"ShowAlert";
			 public static string TVenueId = @"TVenueId";
			 public static string DisplayNotes = @"DisplayNotes";
			 public static string ShowTitle = @"ShowTitle";
			 public static string DisplayUrl = @"DisplayUrl";
			 public static string IPicWidth = @"iPicWidth";
			 public static string IPicHeight = @"iPicHeight";
			 public static string ShowHeader = @"ShowHeader";
			 public static string ShowWriteup = @"ShowWriteup";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string BCtrX = @"bCtrX";
			 public static string BCtrY = @"bCtrY";
			 public static string FacebookEventUrl = @"FacebookEventUrl";
			 public static string VcPrincipal = @"vcPrincipal";
			 public static string VcJustAnnouncedStatus = @"vcJustAnnouncedStatus";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colJShowPromoterRecords != null)
                {
                    foreach (Wcss.JShowPromoter item in colJShowPromoterRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colKioskRecords != null)
                {
                    foreach (Wcss.Kiosk item in colKioskRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colShowDateRecords != null)
                {
                    foreach (Wcss.ShowDate item in colShowDateRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colShowGenreRecords != null)
                {
                    foreach (Wcss.ShowGenre item in colShowGenreRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colShowLinkRecords != null)
                {
                    foreach (Wcss.ShowLink item in colShowLinkRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colVdShowExpenseRecords != null)
                {
                    foreach (Wcss.VdShowExpense item in colVdShowExpenseRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colVdShowGenreRecords != null)
                {
                    foreach (Wcss.VdShowGenre item in colVdShowGenreRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colVdShowInfoRecords != null)
                {
                    foreach (Wcss.VdShowInfo item in colVdShowInfoRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colVdShowTicketRecords != null)
                {
                    foreach (Wcss.VdShowTicket item in colVdShowTicketRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colVdShowTicketOutletRecords != null)
                {
                    foreach (Wcss.VdShowTicketOutlet item in colVdShowTicketOutletRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colJShowPromoterRecords != null)
                {
                    colJShowPromoterRecords.SaveAll();
               }
		
                if (colKioskRecords != null)
                {
                    colKioskRecords.SaveAll();
               }
		
                if (colShowDateRecords != null)
                {
                    colShowDateRecords.SaveAll();
               }
		
                if (colShowGenreRecords != null)
                {
                    colShowGenreRecords.SaveAll();
               }
		
                if (colShowLinkRecords != null)
                {
                    colShowLinkRecords.SaveAll();
               }
		
                if (colVdShowExpenseRecords != null)
                {
                    colVdShowExpenseRecords.SaveAll();
               }
		
                if (colVdShowGenreRecords != null)
                {
                    colVdShowGenreRecords.SaveAll();
               }
		
                if (colVdShowInfoRecords != null)
                {
                    colVdShowInfoRecords.SaveAll();
               }
		
                if (colVdShowTicketRecords != null)
                {
                    colVdShowTicketRecords.SaveAll();
               }
		
                if (colVdShowTicketOutletRecords != null)
                {
                    colVdShowTicketOutletRecords.SaveAll();
               }
		}
        #endregion
	}
}
