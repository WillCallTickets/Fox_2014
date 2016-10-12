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
	/// Strongly-typed collection for the Z2SubscriptionRequest class.
	/// </summary>
    [Serializable]
	public partial class Z2SubscriptionRequestCollection : ActiveList<Z2SubscriptionRequest, Z2SubscriptionRequestCollection>
	{	   
		public Z2SubscriptionRequestCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>Z2SubscriptionRequestCollection</returns>
		public Z2SubscriptionRequestCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Z2SubscriptionRequest o = this[i];
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
	/// This is an ActiveRecord class which wraps the Z2SubscriptionRequest table.
	/// </summary>
	[Serializable]
	public partial class Z2SubscriptionRequest : ActiveRecord<Z2SubscriptionRequest>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Z2SubscriptionRequest()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Z2SubscriptionRequest(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Z2SubscriptionRequest(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Z2SubscriptionRequest(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Z2SubscriptionRequest", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarSource = new TableSchema.TableColumn(schema);
				colvarSource.ColumnName = "Source";
				colvarSource.DataType = DbType.AnsiString;
				colvarSource.MaxLength = 50;
				colvarSource.AutoIncrement = false;
				colvarSource.IsNullable = false;
				colvarSource.IsPrimaryKey = false;
				colvarSource.IsForeignKey = false;
				colvarSource.IsReadOnly = false;
				colvarSource.DefaultSetting = @"";
				colvarSource.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSource);
				
				TableSchema.TableColumn colvarSubscriptionRequest = new TableSchema.TableColumn(schema);
				colvarSubscriptionRequest.ColumnName = "SubscriptionRequest";
				colvarSubscriptionRequest.DataType = DbType.AnsiString;
				colvarSubscriptionRequest.MaxLength = 25;
				colvarSubscriptionRequest.AutoIncrement = false;
				colvarSubscriptionRequest.IsNullable = false;
				colvarSubscriptionRequest.IsPrimaryKey = false;
				colvarSubscriptionRequest.IsForeignKey = false;
				colvarSubscriptionRequest.IsReadOnly = false;
				colvarSubscriptionRequest.DefaultSetting = @"";
				colvarSubscriptionRequest.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSubscriptionRequest);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Z2SubscriptionRequest",schema);
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
		  
		[XmlAttribute("Source")]
		[Bindable(true)]
		public string Source 
		{
			get { return GetColumnValue<string>(Columns.Source); }
			set { SetColumnValue(Columns.Source, value); }
		}
		  
		[XmlAttribute("SubscriptionRequest")]
		[Bindable(true)]
		public string SubscriptionRequest 
		{
			get { return GetColumnValue<string>(Columns.SubscriptionRequest); }
			set { SetColumnValue(Columns.SubscriptionRequest, value); }
		}
		  
		[XmlAttribute("IpAddress")]
		[Bindable(true)]
		public string IpAddress 
		{
			get { return GetColumnValue<string>(Columns.IpAddress); }
			set { SetColumnValue(Columns.IpAddress, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Z2Subscription ActiveRecord object related to this Z2SubscriptionRequest
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
		public static void Insert(DateTime varDtStamp,int varTZ2SubscriptionId,string varSource,string varSubscriptionRequest,string varIpAddress)
		{
			Z2SubscriptionRequest item = new Z2SubscriptionRequest();
			
			item.DtStamp = varDtStamp;
			
			item.TZ2SubscriptionId = varTZ2SubscriptionId;
			
			item.Source = varSource;
			
			item.SubscriptionRequest = varSubscriptionRequest;
			
			item.IpAddress = varIpAddress;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTZ2SubscriptionId,string varSource,string varSubscriptionRequest,string varIpAddress)
		{
			Z2SubscriptionRequest item = new Z2SubscriptionRequest();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TZ2SubscriptionId = varTZ2SubscriptionId;
			
				item.Source = varSource;
			
				item.SubscriptionRequest = varSubscriptionRequest;
			
				item.IpAddress = varIpAddress;
			
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
        
        
        
        public static TableSchema.TableColumn SourceColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn SubscriptionRequestColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IpAddressColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TZ2SubscriptionId = @"tZ2SubscriptionId";
			 public static string Source = @"Source";
			 public static string SubscriptionRequest = @"SubscriptionRequest";
			 public static string IpAddress = @"IpAddress";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
