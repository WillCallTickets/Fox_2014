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
	/// Strongly-typed collection for the ICalendar class.
	/// </summary>
    [Serializable]
	public partial class ICalendarCollection : ActiveList<ICalendar, ICalendarCollection>
	{	   
		public ICalendarCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ICalendarCollection</returns>
		public ICalendarCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ICalendar o = this[i];
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
	/// This is an ActiveRecord class which wraps the ICalendar table.
	/// </summary>
	[Serializable]
	public partial class ICalendar : ActiveRecord<ICalendar>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ICalendar()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ICalendar(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ICalendar(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ICalendar(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ICalendar", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarDtStamp.ColumnName = "DtStamp";
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
				
				TableSchema.TableColumn colvarUrlKey = new TableSchema.TableColumn(schema);
				colvarUrlKey.ColumnName = "UrlKey";
				colvarUrlKey.DataType = DbType.AnsiString;
				colvarUrlKey.MaxLength = 256;
				colvarUrlKey.AutoIncrement = false;
				colvarUrlKey.IsNullable = false;
				colvarUrlKey.IsPrimaryKey = false;
				colvarUrlKey.IsForeignKey = false;
				colvarUrlKey.IsReadOnly = false;
				colvarUrlKey.DefaultSetting = @"";
				colvarUrlKey.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUrlKey);
				
				TableSchema.TableColumn colvarSerializedCalendar = new TableSchema.TableColumn(schema);
				colvarSerializedCalendar.ColumnName = "SerializedCalendar";
				colvarSerializedCalendar.DataType = DbType.AnsiString;
				colvarSerializedCalendar.MaxLength = 4000;
				colvarSerializedCalendar.AutoIncrement = false;
				colvarSerializedCalendar.IsNullable = false;
				colvarSerializedCalendar.IsPrimaryKey = false;
				colvarSerializedCalendar.IsForeignKey = false;
				colvarSerializedCalendar.IsReadOnly = false;
				colvarSerializedCalendar.DefaultSetting = @"";
				colvarSerializedCalendar.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSerializedCalendar);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ICalendar",schema);
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
		  
		[XmlAttribute("UrlKey")]
		[Bindable(true)]
		public string UrlKey 
		{
			get { return GetColumnValue<string>(Columns.UrlKey); }
			set { SetColumnValue(Columns.UrlKey, value); }
		}
		  
		[XmlAttribute("SerializedCalendar")]
		[Bindable(true)]
		public string SerializedCalendar 
		{
			get { return GetColumnValue<string>(Columns.SerializedCalendar); }
			set { SetColumnValue(Columns.SerializedCalendar, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,string varUrlKey,string varSerializedCalendar)
		{
			ICalendar item = new ICalendar();
			
			item.DtStamp = varDtStamp;
			
			item.UrlKey = varUrlKey;
			
			item.SerializedCalendar = varSerializedCalendar;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,string varUrlKey,string varSerializedCalendar)
		{
			ICalendar item = new ICalendar();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.UrlKey = varUrlKey;
			
				item.SerializedCalendar = varSerializedCalendar;
			
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
        
        
        
        public static TableSchema.TableColumn UrlKeyColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn SerializedCalendarColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"DtStamp";
			 public static string UrlKey = @"UrlKey";
			 public static string SerializedCalendar = @"SerializedCalendar";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
