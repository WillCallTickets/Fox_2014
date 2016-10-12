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
	/// Strongly-typed collection for the VdShowExpense class.
	/// </summary>
    [Serializable]
	public partial class VdShowExpenseCollection : ActiveList<VdShowExpense, VdShowExpenseCollection>
	{	   
		public VdShowExpenseCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>VdShowExpenseCollection</returns>
		public VdShowExpenseCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                VdShowExpense o = this[i];
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
	/// This is an ActiveRecord class which wraps the VdShowExpense table.
	/// </summary>
	[Serializable]
	public partial class VdShowExpense : ActiveRecord<VdShowExpense>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public VdShowExpense()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public VdShowExpense(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public VdShowExpense(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public VdShowExpense(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("VdShowExpense", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarDtIncurred = new TableSchema.TableColumn(schema);
				colvarDtIncurred.ColumnName = "dtIncurred";
				colvarDtIncurred.DataType = DbType.DateTime;
				colvarDtIncurred.MaxLength = 0;
				colvarDtIncurred.AutoIncrement = false;
				colvarDtIncurred.IsNullable = true;
				colvarDtIncurred.IsPrimaryKey = false;
				colvarDtIncurred.IsForeignKey = false;
				colvarDtIncurred.IsReadOnly = false;
				
						colvarDtIncurred.DefaultSetting = @"(getdate())";
				colvarDtIncurred.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtIncurred);
				
				TableSchema.TableColumn colvarExpenseCategory = new TableSchema.TableColumn(schema);
				colvarExpenseCategory.ColumnName = "ExpenseCategory";
				colvarExpenseCategory.DataType = DbType.AnsiString;
				colvarExpenseCategory.MaxLength = 256;
				colvarExpenseCategory.AutoIncrement = false;
				colvarExpenseCategory.IsNullable = true;
				colvarExpenseCategory.IsPrimaryKey = false;
				colvarExpenseCategory.IsForeignKey = false;
				colvarExpenseCategory.IsReadOnly = false;
				colvarExpenseCategory.DefaultSetting = @"";
				colvarExpenseCategory.ForeignKeyTableName = "";
				schema.Columns.Add(colvarExpenseCategory);
				
				TableSchema.TableColumn colvarExpenseName = new TableSchema.TableColumn(schema);
				colvarExpenseName.ColumnName = "ExpenseName";
				colvarExpenseName.DataType = DbType.AnsiString;
				colvarExpenseName.MaxLength = 256;
				colvarExpenseName.AutoIncrement = false;
				colvarExpenseName.IsNullable = false;
				colvarExpenseName.IsPrimaryKey = false;
				colvarExpenseName.IsForeignKey = false;
				colvarExpenseName.IsReadOnly = false;
				colvarExpenseName.DefaultSetting = @"";
				colvarExpenseName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarExpenseName);
				
				TableSchema.TableColumn colvarNotes = new TableSchema.TableColumn(schema);
				colvarNotes.ColumnName = "Notes";
				colvarNotes.DataType = DbType.AnsiString;
				colvarNotes.MaxLength = 500;
				colvarNotes.AutoIncrement = false;
				colvarNotes.IsNullable = true;
				colvarNotes.IsPrimaryKey = false;
				colvarNotes.IsForeignKey = false;
				colvarNotes.IsReadOnly = false;
				colvarNotes.DefaultSetting = @"";
				colvarNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNotes);
				
				TableSchema.TableColumn colvarMAmount = new TableSchema.TableColumn(schema);
				colvarMAmount.ColumnName = "mAmount";
				colvarMAmount.DataType = DbType.Currency;
				colvarMAmount.MaxLength = 0;
				colvarMAmount.AutoIncrement = false;
				colvarMAmount.IsNullable = false;
				colvarMAmount.IsPrimaryKey = false;
				colvarMAmount.IsForeignKey = false;
				colvarMAmount.IsReadOnly = false;
				
						colvarMAmount.DefaultSetting = @"((0))";
				colvarMAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMAmount);
				
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
				DataService.Providers["WillCall"].AddSchema("VdShowExpense",schema);
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
		  
		[XmlAttribute("DtIncurred")]
		[Bindable(true)]
		public DateTime? DtIncurred 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtIncurred); }
			set { SetColumnValue(Columns.DtIncurred, value); }
		}
		  
		[XmlAttribute("ExpenseCategory")]
		[Bindable(true)]
		public string ExpenseCategory 
		{
			get { return GetColumnValue<string>(Columns.ExpenseCategory); }
			set { SetColumnValue(Columns.ExpenseCategory, value); }
		}
		  
		[XmlAttribute("ExpenseName")]
		[Bindable(true)]
		public string ExpenseName 
		{
			get { return GetColumnValue<string>(Columns.ExpenseName); }
			set { SetColumnValue(Columns.ExpenseName, value); }
		}
		  
		[XmlAttribute("Notes")]
		[Bindable(true)]
		public string Notes 
		{
			get { return GetColumnValue<string>(Columns.Notes); }
			set { SetColumnValue(Columns.Notes, value); }
		}
		  
		[XmlAttribute("MAmount")]
		[Bindable(true)]
		public decimal MAmount 
		{
			get { return GetColumnValue<decimal>(Columns.MAmount); }
			set { SetColumnValue(Columns.MAmount, value); }
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
		/// Returns a Show ActiveRecord object related to this VdShowExpense
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
		public static void Insert(int varTShowId,DateTime? varDtIncurred,string varExpenseCategory,string varExpenseName,string varNotes,decimal varMAmount,int varIOrdinal,DateTime varDtStamp)
		{
			VdShowExpense item = new VdShowExpense();
			
			item.TShowId = varTShowId;
			
			item.DtIncurred = varDtIncurred;
			
			item.ExpenseCategory = varExpenseCategory;
			
			item.ExpenseName = varExpenseName;
			
			item.Notes = varNotes;
			
			item.MAmount = varMAmount;
			
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
		public static void Update(int varId,int varTShowId,DateTime? varDtIncurred,string varExpenseCategory,string varExpenseName,string varNotes,decimal varMAmount,int varIOrdinal,DateTime varDtStamp)
		{
			VdShowExpense item = new VdShowExpense();
			
				item.Id = varId;
			
				item.TShowId = varTShowId;
			
				item.DtIncurred = varDtIncurred;
			
				item.ExpenseCategory = varExpenseCategory;
			
				item.ExpenseName = varExpenseName;
			
				item.Notes = varNotes;
			
				item.MAmount = varMAmount;
			
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
        
        
        
        public static TableSchema.TableColumn DtIncurredColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ExpenseCategoryColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ExpenseNameColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn NotesColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn MAmountColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn IOrdinalColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TShowId = @"tShowId";
			 public static string DtIncurred = @"dtIncurred";
			 public static string ExpenseCategory = @"ExpenseCategory";
			 public static string ExpenseName = @"ExpenseName";
			 public static string Notes = @"Notes";
			 public static string MAmount = @"mAmount";
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
