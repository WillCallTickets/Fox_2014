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
	/// Strongly-typed collection for the Z2Subscription class.
	/// </summary>
    [Serializable]
	public partial class Z2SubscriptionCollection : ActiveList<Z2Subscription, Z2SubscriptionCollection>
	{	   
		public Z2SubscriptionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>Z2SubscriptionCollection</returns>
		public Z2SubscriptionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Z2Subscription o = this[i];
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
	/// This is an ActiveRecord class which wraps the Z2Subscription table.
	/// </summary>
	[Serializable]
	public partial class Z2Subscription : ActiveRecord<Z2Subscription>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Z2Subscription()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Z2Subscription(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Z2Subscription(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Z2Subscription(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Z2Subscription", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarDtCreated = new TableSchema.TableColumn(schema);
				colvarDtCreated.ColumnName = "dtCreated";
				colvarDtCreated.DataType = DbType.DateTime;
				colvarDtCreated.MaxLength = 0;
				colvarDtCreated.AutoIncrement = false;
				colvarDtCreated.IsNullable = false;
				colvarDtCreated.IsPrimaryKey = false;
				colvarDtCreated.IsForeignKey = false;
				colvarDtCreated.IsReadOnly = false;
				
						colvarDtCreated.DefaultSetting = @"(getdate())";
				colvarDtCreated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtCreated);
				
				TableSchema.TableColumn colvarDtModified = new TableSchema.TableColumn(schema);
				colvarDtModified.ColumnName = "dtModified";
				colvarDtModified.DataType = DbType.DateTime;
				colvarDtModified.MaxLength = 0;
				colvarDtModified.AutoIncrement = false;
				colvarDtModified.IsNullable = true;
				colvarDtModified.IsPrimaryKey = false;
				colvarDtModified.IsForeignKey = false;
				colvarDtModified.IsReadOnly = false;
				colvarDtModified.DefaultSetting = @"";
				colvarDtModified.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtModified);
				
				TableSchema.TableColumn colvarEmail = new TableSchema.TableColumn(schema);
				colvarEmail.ColumnName = "Email";
				colvarEmail.DataType = DbType.AnsiString;
				colvarEmail.MaxLength = 256;
				colvarEmail.AutoIncrement = false;
				colvarEmail.IsNullable = false;
				colvarEmail.IsPrimaryKey = false;
				colvarEmail.IsForeignKey = false;
				colvarEmail.IsReadOnly = false;
				colvarEmail.DefaultSetting = @"";
				colvarEmail.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEmail);
				
				TableSchema.TableColumn colvarIpAddress = new TableSchema.TableColumn(schema);
				colvarIpAddress.ColumnName = "IpAddress";
				colvarIpAddress.DataType = DbType.AnsiString;
				colvarIpAddress.MaxLength = 25;
				colvarIpAddress.AutoIncrement = false;
				colvarIpAddress.IsNullable = false;
				colvarIpAddress.IsPrimaryKey = false;
				colvarIpAddress.IsForeignKey = false;
				colvarIpAddress.IsReadOnly = false;
				colvarIpAddress.DefaultSetting = @"";
				colvarIpAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIpAddress);
				
				TableSchema.TableColumn colvarBSubscribed = new TableSchema.TableColumn(schema);
				colvarBSubscribed.ColumnName = "bSubscribed";
				colvarBSubscribed.DataType = DbType.Boolean;
				colvarBSubscribed.MaxLength = 0;
				colvarBSubscribed.AutoIncrement = false;
				colvarBSubscribed.IsNullable = false;
				colvarBSubscribed.IsPrimaryKey = false;
				colvarBSubscribed.IsForeignKey = false;
				colvarBSubscribed.IsReadOnly = false;
				
						colvarBSubscribed.DefaultSetting = @"((0))";
				colvarBSubscribed.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBSubscribed);
				
				TableSchema.TableColumn colvarTZ2SubscriptionHistoryId = new TableSchema.TableColumn(schema);
				colvarTZ2SubscriptionHistoryId.ColumnName = "tZ2SubscriptionHistoryId";
				colvarTZ2SubscriptionHistoryId.DataType = DbType.Int32;
				colvarTZ2SubscriptionHistoryId.MaxLength = 0;
				colvarTZ2SubscriptionHistoryId.AutoIncrement = false;
				colvarTZ2SubscriptionHistoryId.IsNullable = true;
				colvarTZ2SubscriptionHistoryId.IsPrimaryKey = false;
				colvarTZ2SubscriptionHistoryId.IsForeignKey = false;
				colvarTZ2SubscriptionHistoryId.IsReadOnly = false;
				colvarTZ2SubscriptionHistoryId.DefaultSetting = @"";
				colvarTZ2SubscriptionHistoryId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTZ2SubscriptionHistoryId);
				
				TableSchema.TableColumn colvarInitialSourceQuery = new TableSchema.TableColumn(schema);
				colvarInitialSourceQuery.ColumnName = "InitialSourceQuery";
				colvarInitialSourceQuery.DataType = DbType.AnsiString;
				colvarInitialSourceQuery.MaxLength = 256;
				colvarInitialSourceQuery.AutoIncrement = false;
				colvarInitialSourceQuery.IsNullable = true;
				colvarInitialSourceQuery.IsPrimaryKey = false;
				colvarInitialSourceQuery.IsForeignKey = false;
				colvarInitialSourceQuery.IsReadOnly = false;
				colvarInitialSourceQuery.DefaultSetting = @"";
				colvarInitialSourceQuery.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInitialSourceQuery);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Z2Subscription",schema);
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
		  
		[XmlAttribute("DtCreated")]
		[Bindable(true)]
		public DateTime DtCreated 
		{
			get { return GetColumnValue<DateTime>(Columns.DtCreated); }
			set { SetColumnValue(Columns.DtCreated, value); }
		}
		  
		[XmlAttribute("DtModified")]
		[Bindable(true)]
		public DateTime? DtModified 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtModified); }
			set { SetColumnValue(Columns.DtModified, value); }
		}
		  
		[XmlAttribute("Email")]
		[Bindable(true)]
		public string Email 
		{
			get { return GetColumnValue<string>(Columns.Email); }
			set { SetColumnValue(Columns.Email, value); }
		}
		  
		[XmlAttribute("IpAddress")]
		[Bindable(true)]
		public string IpAddress 
		{
			get { return GetColumnValue<string>(Columns.IpAddress); }
			set { SetColumnValue(Columns.IpAddress, value); }
		}
		  
		[XmlAttribute("BSubscribed")]
		[Bindable(true)]
		public bool BSubscribed 
		{
			get { return GetColumnValue<bool>(Columns.BSubscribed); }
			set { SetColumnValue(Columns.BSubscribed, value); }
		}
		  
		[XmlAttribute("TZ2SubscriptionHistoryId")]
		[Bindable(true)]
		public int? TZ2SubscriptionHistoryId 
		{
			get { return GetColumnValue<int?>(Columns.TZ2SubscriptionHistoryId); }
			set { SetColumnValue(Columns.TZ2SubscriptionHistoryId, value); }
		}
		  
		[XmlAttribute("InitialSourceQuery")]
		[Bindable(true)]
		public string InitialSourceQuery 
		{
			get { return GetColumnValue<string>(Columns.InitialSourceQuery); }
			set { SetColumnValue(Columns.InitialSourceQuery, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.Z2SubscriptionRequestCollection colZ2SubscriptionRequestRecords;
		public Wcss.Z2SubscriptionRequestCollection Z2SubscriptionRequestRecords()
		{
			if(colZ2SubscriptionRequestRecords == null)
			{
				colZ2SubscriptionRequestRecords = new Wcss.Z2SubscriptionRequestCollection().Where(Z2SubscriptionRequest.Columns.TZ2SubscriptionId, Id).Load();
				colZ2SubscriptionRequestRecords.ListChanged += new ListChangedEventHandler(colZ2SubscriptionRequestRecords_ListChanged);
			}
			return colZ2SubscriptionRequestRecords;
		}
				
		void colZ2SubscriptionRequestRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colZ2SubscriptionRequestRecords[e.NewIndex].TZ2SubscriptionId = Id;
				colZ2SubscriptionRequestRecords.ListChanged += new ListChangedEventHandler(colZ2SubscriptionRequestRecords_ListChanged);
            }
		}
		private Wcss.Z2SubscriptionTransferCollection colZ2SubscriptionTransferRecords;
		public Wcss.Z2SubscriptionTransferCollection Z2SubscriptionTransferRecords()
		{
			if(colZ2SubscriptionTransferRecords == null)
			{
				colZ2SubscriptionTransferRecords = new Wcss.Z2SubscriptionTransferCollection().Where(Z2SubscriptionTransfer.Columns.TZ2SubscriptionId, Id).Load();
				colZ2SubscriptionTransferRecords.ListChanged += new ListChangedEventHandler(colZ2SubscriptionTransferRecords_ListChanged);
			}
			return colZ2SubscriptionTransferRecords;
		}
				
		void colZ2SubscriptionTransferRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colZ2SubscriptionTransferRecords[e.NewIndex].TZ2SubscriptionId = Id;
				colZ2SubscriptionTransferRecords.ListChanged += new ListChangedEventHandler(colZ2SubscriptionTransferRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtCreated,DateTime? varDtModified,string varEmail,string varIpAddress,bool varBSubscribed,int? varTZ2SubscriptionHistoryId,string varInitialSourceQuery)
		{
			Z2Subscription item = new Z2Subscription();
			
			item.DtCreated = varDtCreated;
			
			item.DtModified = varDtModified;
			
			item.Email = varEmail;
			
			item.IpAddress = varIpAddress;
			
			item.BSubscribed = varBSubscribed;
			
			item.TZ2SubscriptionHistoryId = varTZ2SubscriptionHistoryId;
			
			item.InitialSourceQuery = varInitialSourceQuery;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtCreated,DateTime? varDtModified,string varEmail,string varIpAddress,bool varBSubscribed,int? varTZ2SubscriptionHistoryId,string varInitialSourceQuery)
		{
			Z2Subscription item = new Z2Subscription();
			
				item.Id = varId;
			
				item.DtCreated = varDtCreated;
			
				item.DtModified = varDtModified;
			
				item.Email = varEmail;
			
				item.IpAddress = varIpAddress;
			
				item.BSubscribed = varBSubscribed;
			
				item.TZ2SubscriptionHistoryId = varTZ2SubscriptionHistoryId;
			
				item.InitialSourceQuery = varInitialSourceQuery;
			
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
        
        
        
        public static TableSchema.TableColumn DtCreatedColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DtModifiedColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn EmailColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IpAddressColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BSubscribedColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TZ2SubscriptionHistoryIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn InitialSourceQueryColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtCreated = @"dtCreated";
			 public static string DtModified = @"dtModified";
			 public static string Email = @"Email";
			 public static string IpAddress = @"IpAddress";
			 public static string BSubscribed = @"bSubscribed";
			 public static string TZ2SubscriptionHistoryId = @"tZ2SubscriptionHistoryId";
			 public static string InitialSourceQuery = @"InitialSourceQuery";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colZ2SubscriptionRequestRecords != null)
                {
                    foreach (Wcss.Z2SubscriptionRequest item in colZ2SubscriptionRequestRecords)
                    {
                        if (item.TZ2SubscriptionId != Id)
                        {
                            item.TZ2SubscriptionId = Id;
                        }
                    }
               }
		
                if (colZ2SubscriptionTransferRecords != null)
                {
                    foreach (Wcss.Z2SubscriptionTransfer item in colZ2SubscriptionTransferRecords)
                    {
                        if (item.TZ2SubscriptionId != Id)
                        {
                            item.TZ2SubscriptionId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colZ2SubscriptionRequestRecords != null)
                {
                    colZ2SubscriptionRequestRecords.SaveAll();
               }
		
                if (colZ2SubscriptionTransferRecords != null)
                {
                    colZ2SubscriptionTransferRecords.SaveAll();
               }
		}
        #endregion
	}
}
