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
	/// Strongly-typed collection for the Z2SubscriptionTransfer class.
	/// </summary>
    [Serializable]
	public partial class Z2SubscriptionTransferCollection : ActiveList<Z2SubscriptionTransfer, Z2SubscriptionTransferCollection>
	{	   
		public Z2SubscriptionTransferCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>Z2SubscriptionTransferCollection</returns>
		public Z2SubscriptionTransferCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Z2SubscriptionTransfer o = this[i];
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
	/// This is an ActiveRecord class which wraps the Z2SubscriptionTransfer table.
	/// </summary>
	[Serializable]
	public partial class Z2SubscriptionTransfer : ActiveRecord<Z2SubscriptionTransfer>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Z2SubscriptionTransfer()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Z2SubscriptionTransfer(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Z2SubscriptionTransfer(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Z2SubscriptionTransfer(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Z2SubscriptionTransfer", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTZ2SubscriptionId = new TableSchema.TableColumn(schema);
				colvarTZ2SubscriptionId.ColumnName = "tZ2SubscriptionId";
				colvarTZ2SubscriptionId.DataType = DbType.Int32;
				colvarTZ2SubscriptionId.MaxLength = 0;
				colvarTZ2SubscriptionId.AutoIncrement = false;
				colvarTZ2SubscriptionId.IsNullable = false;
				colvarTZ2SubscriptionId.IsPrimaryKey = false;
				colvarTZ2SubscriptionId.IsForeignKey = true;
				colvarTZ2SubscriptionId.IsReadOnly = false;
				colvarTZ2SubscriptionId.DefaultSetting = @"";
				
					colvarTZ2SubscriptionId.ForeignKeyTableName = "Z2Subscription";
				schema.Columns.Add(colvarTZ2SubscriptionId);
				
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
				
				TableSchema.TableColumn colvarListSource = new TableSchema.TableColumn(schema);
				colvarListSource.ColumnName = "ListSource";
				colvarListSource.DataType = DbType.AnsiString;
				colvarListSource.MaxLength = 50;
				colvarListSource.AutoIncrement = false;
				colvarListSource.IsNullable = false;
				colvarListSource.IsPrimaryKey = false;
				colvarListSource.IsForeignKey = false;
				colvarListSource.IsReadOnly = false;
				colvarListSource.DefaultSetting = @"";
				colvarListSource.ForeignKeyTableName = "";
				schema.Columns.Add(colvarListSource);
				
				TableSchema.TableColumn colvarDtTransferred = new TableSchema.TableColumn(schema);
				colvarDtTransferred.ColumnName = "dtTransferred";
				colvarDtTransferred.DataType = DbType.DateTime;
				colvarDtTransferred.MaxLength = 0;
				colvarDtTransferred.AutoIncrement = false;
				colvarDtTransferred.IsNullable = true;
				colvarDtTransferred.IsPrimaryKey = false;
				colvarDtTransferred.IsForeignKey = false;
				colvarDtTransferred.IsReadOnly = false;
				colvarDtTransferred.DefaultSetting = @"";
				colvarDtTransferred.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtTransferred);
				
				TableSchema.TableColumn colvarTransferSubscribedStatus = new TableSchema.TableColumn(schema);
				colvarTransferSubscribedStatus.ColumnName = "TransferSubscribedStatus";
				colvarTransferSubscribedStatus.DataType = DbType.Boolean;
				colvarTransferSubscribedStatus.MaxLength = 0;
				colvarTransferSubscribedStatus.AutoIncrement = false;
				colvarTransferSubscribedStatus.IsNullable = true;
				colvarTransferSubscribedStatus.IsPrimaryKey = false;
				colvarTransferSubscribedStatus.IsForeignKey = false;
				colvarTransferSubscribedStatus.IsReadOnly = false;
				colvarTransferSubscribedStatus.DefaultSetting = @"";
				colvarTransferSubscribedStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTransferSubscribedStatus);
				
				TableSchema.TableColumn colvarDtSourceListUpdated = new TableSchema.TableColumn(schema);
				colvarDtSourceListUpdated.ColumnName = "dtSourceListUpdated";
				colvarDtSourceListUpdated.DataType = DbType.DateTime;
				colvarDtSourceListUpdated.MaxLength = 0;
				colvarDtSourceListUpdated.AutoIncrement = false;
				colvarDtSourceListUpdated.IsNullable = true;
				colvarDtSourceListUpdated.IsPrimaryKey = false;
				colvarDtSourceListUpdated.IsForeignKey = false;
				colvarDtSourceListUpdated.IsReadOnly = false;
				colvarDtSourceListUpdated.DefaultSetting = @"";
				colvarDtSourceListUpdated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtSourceListUpdated);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Z2SubscriptionTransfer",schema);
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
		  
		[XmlAttribute("TZ2SubscriptionId")]
		[Bindable(true)]
		public int TZ2SubscriptionId 
		{
			get { return GetColumnValue<int>(Columns.TZ2SubscriptionId); }
			set { SetColumnValue(Columns.TZ2SubscriptionId, value); }
		}
		  
		[XmlAttribute("Email")]
		[Bindable(true)]
		public string Email 
		{
			get { return GetColumnValue<string>(Columns.Email); }
			set { SetColumnValue(Columns.Email, value); }
		}
		  
		[XmlAttribute("ListSource")]
		[Bindable(true)]
		public string ListSource 
		{
			get { return GetColumnValue<string>(Columns.ListSource); }
			set { SetColumnValue(Columns.ListSource, value); }
		}
		  
		[XmlAttribute("DtTransferred")]
		[Bindable(true)]
		public DateTime? DtTransferred 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtTransferred); }
			set { SetColumnValue(Columns.DtTransferred, value); }
		}
		  
		[XmlAttribute("TransferSubscribedStatus")]
		[Bindable(true)]
		public bool? TransferSubscribedStatus 
		{
			get { return GetColumnValue<bool?>(Columns.TransferSubscribedStatus); }
			set { SetColumnValue(Columns.TransferSubscribedStatus, value); }
		}
		  
		[XmlAttribute("DtSourceListUpdated")]
		[Bindable(true)]
		public DateTime? DtSourceListUpdated 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtSourceListUpdated); }
			set { SetColumnValue(Columns.DtSourceListUpdated, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Z2Subscription ActiveRecord object related to this Z2SubscriptionTransfer
		/// 
		/// </summary>
		private Wcss.Z2Subscription Z2Subscription
		{
			get { return Wcss.Z2Subscription.FetchByID(this.TZ2SubscriptionId); }
			set { SetColumnValue("tZ2SubscriptionId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Z2Subscription _z2subscriptionrecord = null;
		
		public Wcss.Z2Subscription Z2SubscriptionRecord
		{
		    get
            {
                if (_z2subscriptionrecord == null)
                {
                    _z2subscriptionrecord = new Wcss.Z2Subscription();
                    _z2subscriptionrecord.CopyFrom(this.Z2Subscription);
                }
                return _z2subscriptionrecord;
            }
            set
            {
                if(value != null && _z2subscriptionrecord == null)
			        _z2subscriptionrecord = new Wcss.Z2Subscription();
                
                SetColumnValue("tZ2SubscriptionId", value.Id);
                _z2subscriptionrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTZ2SubscriptionId,string varEmail,string varListSource,DateTime? varDtTransferred,bool? varTransferSubscribedStatus,DateTime? varDtSourceListUpdated)
		{
			Z2SubscriptionTransfer item = new Z2SubscriptionTransfer();
			
			item.DtStamp = varDtStamp;
			
			item.TZ2SubscriptionId = varTZ2SubscriptionId;
			
			item.Email = varEmail;
			
			item.ListSource = varListSource;
			
			item.DtTransferred = varDtTransferred;
			
			item.TransferSubscribedStatus = varTransferSubscribedStatus;
			
			item.DtSourceListUpdated = varDtSourceListUpdated;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTZ2SubscriptionId,string varEmail,string varListSource,DateTime? varDtTransferred,bool? varTransferSubscribedStatus,DateTime? varDtSourceListUpdated)
		{
			Z2SubscriptionTransfer item = new Z2SubscriptionTransfer();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TZ2SubscriptionId = varTZ2SubscriptionId;
			
				item.Email = varEmail;
			
				item.ListSource = varListSource;
			
				item.DtTransferred = varDtTransferred;
			
				item.TransferSubscribedStatus = varTransferSubscribedStatus;
			
				item.DtSourceListUpdated = varDtSourceListUpdated;
			
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
        
        
        
        public static TableSchema.TableColumn TZ2SubscriptionIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn EmailColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ListSourceColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DtTransferredColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TransferSubscribedStatusColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtSourceListUpdatedColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TZ2SubscriptionId = @"tZ2SubscriptionId";
			 public static string Email = @"Email";
			 public static string ListSource = @"ListSource";
			 public static string DtTransferred = @"dtTransferred";
			 public static string TransferSubscribedStatus = @"TransferSubscribedStatus";
			 public static string DtSourceListUpdated = @"dtSourceListUpdated";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
