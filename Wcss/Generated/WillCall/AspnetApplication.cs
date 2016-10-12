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
	/// Strongly-typed collection for the AspnetApplication class.
	/// </summary>
    [Serializable]
	public partial class AspnetApplicationCollection : ActiveList<AspnetApplication, AspnetApplicationCollection>
	{	   
		public AspnetApplicationCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AspnetApplicationCollection</returns>
		public AspnetApplicationCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AspnetApplication o = this[i];
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
	/// This is an ActiveRecord class which wraps the aspnet_Applications table.
	/// </summary>
	[Serializable]
	public partial class AspnetApplication : ActiveRecord<AspnetApplication>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AspnetApplication()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AspnetApplication(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AspnetApplication(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AspnetApplication(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("aspnet_Applications", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarApplicationName = new TableSchema.TableColumn(schema);
				colvarApplicationName.ColumnName = "ApplicationName";
				colvarApplicationName.DataType = DbType.String;
				colvarApplicationName.MaxLength = 256;
				colvarApplicationName.AutoIncrement = false;
				colvarApplicationName.IsNullable = false;
				colvarApplicationName.IsPrimaryKey = false;
				colvarApplicationName.IsForeignKey = false;
				colvarApplicationName.IsReadOnly = false;
				colvarApplicationName.DefaultSetting = @"";
				colvarApplicationName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApplicationName);
				
				TableSchema.TableColumn colvarLoweredApplicationName = new TableSchema.TableColumn(schema);
				colvarLoweredApplicationName.ColumnName = "LoweredApplicationName";
				colvarLoweredApplicationName.DataType = DbType.String;
				colvarLoweredApplicationName.MaxLength = 256;
				colvarLoweredApplicationName.AutoIncrement = false;
				colvarLoweredApplicationName.IsNullable = false;
				colvarLoweredApplicationName.IsPrimaryKey = false;
				colvarLoweredApplicationName.IsForeignKey = false;
				colvarLoweredApplicationName.IsReadOnly = false;
				colvarLoweredApplicationName.DefaultSetting = @"";
				colvarLoweredApplicationName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLoweredApplicationName);
				
				TableSchema.TableColumn colvarApplicationId = new TableSchema.TableColumn(schema);
				colvarApplicationId.ColumnName = "ApplicationId";
				colvarApplicationId.DataType = DbType.Guid;
				colvarApplicationId.MaxLength = 0;
				colvarApplicationId.AutoIncrement = false;
				colvarApplicationId.IsNullable = false;
				colvarApplicationId.IsPrimaryKey = true;
				colvarApplicationId.IsForeignKey = false;
				colvarApplicationId.IsReadOnly = false;
				
						colvarApplicationId.DefaultSetting = @"(newid())";
				colvarApplicationId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApplicationId);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.String;
				colvarDescription.MaxLength = 256;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("aspnet_Applications",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ApplicationName")]
		[Bindable(true)]
		public string ApplicationName 
		{
			get { return GetColumnValue<string>(Columns.ApplicationName); }
			set { SetColumnValue(Columns.ApplicationName, value); }
		}
		  
		[XmlAttribute("LoweredApplicationName")]
		[Bindable(true)]
		public string LoweredApplicationName 
		{
			get { return GetColumnValue<string>(Columns.LoweredApplicationName); }
			set { SetColumnValue(Columns.LoweredApplicationName, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.AspnetMembershipCollection colAspnetMembershipRecords;
		public Wcss.AspnetMembershipCollection AspnetMembershipRecords()
		{
			if(colAspnetMembershipRecords == null)
			{
				colAspnetMembershipRecords = new Wcss.AspnetMembershipCollection().Where(AspnetMembership.Columns.ApplicationId, ApplicationId).Load();
				colAspnetMembershipRecords.ListChanged += new ListChangedEventHandler(colAspnetMembershipRecords_ListChanged);
			}
			return colAspnetMembershipRecords;
		}
				
		void colAspnetMembershipRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetMembershipRecords[e.NewIndex].ApplicationId = ApplicationId;
				colAspnetMembershipRecords.ListChanged += new ListChangedEventHandler(colAspnetMembershipRecords_ListChanged);
            }
		}
		private Wcss.AspnetPathCollection colAspnetPaths;
		public Wcss.AspnetPathCollection AspnetPaths()
		{
			if(colAspnetPaths == null)
			{
				colAspnetPaths = new Wcss.AspnetPathCollection().Where(AspnetPath.Columns.ApplicationId, ApplicationId).Load();
				colAspnetPaths.ListChanged += new ListChangedEventHandler(colAspnetPaths_ListChanged);
			}
			return colAspnetPaths;
		}
				
		void colAspnetPaths_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetPaths[e.NewIndex].ApplicationId = ApplicationId;
				colAspnetPaths.ListChanged += new ListChangedEventHandler(colAspnetPaths_ListChanged);
            }
		}
		private Wcss.AspnetRoleCollection colAspnetRoles;
		public Wcss.AspnetRoleCollection AspnetRoles()
		{
			if(colAspnetRoles == null)
			{
				colAspnetRoles = new Wcss.AspnetRoleCollection().Where(AspnetRole.Columns.ApplicationId, ApplicationId).Load();
				colAspnetRoles.ListChanged += new ListChangedEventHandler(colAspnetRoles_ListChanged);
			}
			return colAspnetRoles;
		}
				
		void colAspnetRoles_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetRoles[e.NewIndex].ApplicationId = ApplicationId;
				colAspnetRoles.ListChanged += new ListChangedEventHandler(colAspnetRoles_ListChanged);
            }
		}
		private Wcss.AspnetUserCollection colAspnetUsers;
		public Wcss.AspnetUserCollection AspnetUsers()
		{
			if(colAspnetUsers == null)
			{
				colAspnetUsers = new Wcss.AspnetUserCollection().Where(AspnetUser.Columns.ApplicationId, ApplicationId).Load();
				colAspnetUsers.ListChanged += new ListChangedEventHandler(colAspnetUsers_ListChanged);
			}
			return colAspnetUsers;
		}
				
		void colAspnetUsers_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetUsers[e.NewIndex].ApplicationId = ApplicationId;
				colAspnetUsers.ListChanged += new ListChangedEventHandler(colAspnetUsers_ListChanged);
            }
		}
		private Wcss.ActCollection colActRecords;
		public Wcss.ActCollection ActRecords()
		{
			if(colActRecords == null)
			{
				colActRecords = new Wcss.ActCollection().Where(Act.Columns.ApplicationId, ApplicationId).Load();
				colActRecords.ListChanged += new ListChangedEventHandler(colActRecords_ListChanged);
			}
			return colActRecords;
		}
				
		void colActRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colActRecords[e.NewIndex].ApplicationId = ApplicationId;
				colActRecords.ListChanged += new ListChangedEventHandler(colActRecords_ListChanged);
            }
		}
		private Wcss.AgeCollection colAgeRecords;
		public Wcss.AgeCollection AgeRecords()
		{
			if(colAgeRecords == null)
			{
				colAgeRecords = new Wcss.AgeCollection().Where(Age.Columns.ApplicationId, ApplicationId).Load();
				colAgeRecords.ListChanged += new ListChangedEventHandler(colAgeRecords_ListChanged);
			}
			return colAgeRecords;
		}
				
		void colAgeRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAgeRecords[e.NewIndex].ApplicationId = ApplicationId;
				colAgeRecords.ListChanged += new ListChangedEventHandler(colAgeRecords_ListChanged);
            }
		}
		private Wcss.EmailLetterCollection colEmailLetterRecords;
		public Wcss.EmailLetterCollection EmailLetterRecords()
		{
			if(colEmailLetterRecords == null)
			{
				colEmailLetterRecords = new Wcss.EmailLetterCollection().Where(EmailLetter.Columns.ApplicationId, ApplicationId).Load();
				colEmailLetterRecords.ListChanged += new ListChangedEventHandler(colEmailLetterRecords_ListChanged);
			}
			return colEmailLetterRecords;
		}
				
		void colEmailLetterRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colEmailLetterRecords[e.NewIndex].ApplicationId = ApplicationId;
				colEmailLetterRecords.ListChanged += new ListChangedEventHandler(colEmailLetterRecords_ListChanged);
            }
		}
		private Wcss.EmployeeCollection colEmployeeRecords;
		public Wcss.EmployeeCollection EmployeeRecords()
		{
			if(colEmployeeRecords == null)
			{
				colEmployeeRecords = new Wcss.EmployeeCollection().Where(Employee.Columns.ApplicationId, ApplicationId).Load();
				colEmployeeRecords.ListChanged += new ListChangedEventHandler(colEmployeeRecords_ListChanged);
			}
			return colEmployeeRecords;
		}
				
		void colEmployeeRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colEmployeeRecords[e.NewIndex].ApplicationId = ApplicationId;
				colEmployeeRecords.ListChanged += new ListChangedEventHandler(colEmployeeRecords_ListChanged);
            }
		}
		private Wcss.EventQCollection colEventQRecords;
		public Wcss.EventQCollection EventQRecords()
		{
			if(colEventQRecords == null)
			{
				colEventQRecords = new Wcss.EventQCollection().Where(EventQ.Columns.ApplicationId, ApplicationId).Load();
				colEventQRecords.ListChanged += new ListChangedEventHandler(colEventQRecords_ListChanged);
			}
			return colEventQRecords;
		}
				
		void colEventQRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colEventQRecords[e.NewIndex].ApplicationId = ApplicationId;
				colEventQRecords.ListChanged += new ListChangedEventHandler(colEventQRecords_ListChanged);
            }
		}
		private Wcss.FaqCategorieCollection colFaqCategorieRecords;
		public Wcss.FaqCategorieCollection FaqCategorieRecords()
		{
			if(colFaqCategorieRecords == null)
			{
				colFaqCategorieRecords = new Wcss.FaqCategorieCollection().Where(FaqCategorie.Columns.ApplicationId, ApplicationId).Load();
				colFaqCategorieRecords.ListChanged += new ListChangedEventHandler(colFaqCategorieRecords_ListChanged);
			}
			return colFaqCategorieRecords;
		}
				
		void colFaqCategorieRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colFaqCategorieRecords[e.NewIndex].ApplicationId = ApplicationId;
				colFaqCategorieRecords.ListChanged += new ListChangedEventHandler(colFaqCategorieRecords_ListChanged);
            }
		}
		private Wcss.HintQuestionCollection colHintQuestionRecords;
		public Wcss.HintQuestionCollection HintQuestionRecords()
		{
			if(colHintQuestionRecords == null)
			{
				colHintQuestionRecords = new Wcss.HintQuestionCollection().Where(HintQuestion.Columns.ApplicationId, ApplicationId).Load();
				colHintQuestionRecords.ListChanged += new ListChangedEventHandler(colHintQuestionRecords_ListChanged);
			}
			return colHintQuestionRecords;
		}
				
		void colHintQuestionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHintQuestionRecords[e.NewIndex].ApplicationId = ApplicationId;
				colHintQuestionRecords.ListChanged += new ListChangedEventHandler(colHintQuestionRecords_ListChanged);
            }
		}
		private Wcss.MailQueueCollection colMailQueueRecords;
		public Wcss.MailQueueCollection MailQueueRecords()
		{
			if(colMailQueueRecords == null)
			{
				colMailQueueRecords = new Wcss.MailQueueCollection().Where(MailQueue.Columns.ApplicationId, ApplicationId).Load();
				colMailQueueRecords.ListChanged += new ListChangedEventHandler(colMailQueueRecords_ListChanged);
			}
			return colMailQueueRecords;
		}
				
		void colMailQueueRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMailQueueRecords[e.NewIndex].ApplicationId = ApplicationId;
				colMailQueueRecords.ListChanged += new ListChangedEventHandler(colMailQueueRecords_ListChanged);
            }
		}
		private Wcss.PromoterCollection colPromoterRecords;
		public Wcss.PromoterCollection PromoterRecords()
		{
			if(colPromoterRecords == null)
			{
				colPromoterRecords = new Wcss.PromoterCollection().Where(Promoter.Columns.ApplicationId, ApplicationId).Load();
				colPromoterRecords.ListChanged += new ListChangedEventHandler(colPromoterRecords_ListChanged);
			}
			return colPromoterRecords;
		}
				
		void colPromoterRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colPromoterRecords[e.NewIndex].ApplicationId = ApplicationId;
				colPromoterRecords.ListChanged += new ListChangedEventHandler(colPromoterRecords_ListChanged);
            }
		}
		private Wcss.KioskCollection colKioskRecords;
		public Wcss.KioskCollection KioskRecords()
		{
			if(colKioskRecords == null)
			{
				colKioskRecords = new Wcss.KioskCollection().Where(Kiosk.Columns.ApplicationId, ApplicationId).Load();
				colKioskRecords.ListChanged += new ListChangedEventHandler(colKioskRecords_ListChanged);
			}
			return colKioskRecords;
		}
				
		void colKioskRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colKioskRecords[e.NewIndex].ApplicationId = ApplicationId;
				colKioskRecords.ListChanged += new ListChangedEventHandler(colKioskRecords_ListChanged);
            }
		}
		private Wcss.SalePromotionCollection colSalePromotionRecords;
		public Wcss.SalePromotionCollection SalePromotionRecords()
		{
			if(colSalePromotionRecords == null)
			{
				colSalePromotionRecords = new Wcss.SalePromotionCollection().Where(SalePromotion.Columns.ApplicationId, ApplicationId).Load();
				colSalePromotionRecords.ListChanged += new ListChangedEventHandler(colSalePromotionRecords_ListChanged);
			}
			return colSalePromotionRecords;
		}
				
		void colSalePromotionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSalePromotionRecords[e.NewIndex].ApplicationId = ApplicationId;
				colSalePromotionRecords.ListChanged += new ListChangedEventHandler(colSalePromotionRecords_ListChanged);
            }
		}
		private Wcss.ShowCollection colShowRecords;
		public Wcss.ShowCollection ShowRecords()
		{
			if(colShowRecords == null)
			{
				colShowRecords = new Wcss.ShowCollection().Where(Show.Columns.ApplicationId, ApplicationId).Load();
				colShowRecords.ListChanged += new ListChangedEventHandler(colShowRecords_ListChanged);
			}
			return colShowRecords;
		}
				
		void colShowRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowRecords[e.NewIndex].ApplicationId = ApplicationId;
				colShowRecords.ListChanged += new ListChangedEventHandler(colShowRecords_ListChanged);
            }
		}
		private Wcss.SiteConfigCollection colSiteConfigRecords;
		public Wcss.SiteConfigCollection SiteConfigRecords()
		{
			if(colSiteConfigRecords == null)
			{
				colSiteConfigRecords = new Wcss.SiteConfigCollection().Where(SiteConfig.Columns.ApplicationId, ApplicationId).Load();
				colSiteConfigRecords.ListChanged += new ListChangedEventHandler(colSiteConfigRecords_ListChanged);
			}
			return colSiteConfigRecords;
		}
				
		void colSiteConfigRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSiteConfigRecords[e.NewIndex].ApplicationId = ApplicationId;
				colSiteConfigRecords.ListChanged += new ListChangedEventHandler(colSiteConfigRecords_ListChanged);
            }
		}
		private Wcss.SubscriptionCollection colSubscriptionRecords;
		public Wcss.SubscriptionCollection SubscriptionRecords()
		{
			if(colSubscriptionRecords == null)
			{
				colSubscriptionRecords = new Wcss.SubscriptionCollection().Where(Subscription.Columns.ApplicationId, ApplicationId).Load();
				colSubscriptionRecords.ListChanged += new ListChangedEventHandler(colSubscriptionRecords_ListChanged);
			}
			return colSubscriptionRecords;
		}
				
		void colSubscriptionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSubscriptionRecords[e.NewIndex].ApplicationId = ApplicationId;
				colSubscriptionRecords.ListChanged += new ListChangedEventHandler(colSubscriptionRecords_ListChanged);
            }
		}
		private Wcss.VenueCollection colVenueRecords;
		public Wcss.VenueCollection VenueRecords()
		{
			if(colVenueRecords == null)
			{
				colVenueRecords = new Wcss.VenueCollection().Where(Venue.Columns.ApplicationId, ApplicationId).Load();
				colVenueRecords.ListChanged += new ListChangedEventHandler(colVenueRecords_ListChanged);
			}
			return colVenueRecords;
		}
				
		void colVenueRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colVenueRecords[e.NewIndex].ApplicationId = ApplicationId;
				colVenueRecords.ListChanged += new ListChangedEventHandler(colVenueRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varApplicationName,string varLoweredApplicationName,Guid varApplicationId,string varDescription)
		{
			AspnetApplication item = new AspnetApplication();
			
			item.ApplicationName = varApplicationName;
			
			item.LoweredApplicationName = varLoweredApplicationName;
			
			item.ApplicationId = varApplicationId;
			
			item.Description = varDescription;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varApplicationName,string varLoweredApplicationName,Guid varApplicationId,string varDescription)
		{
			AspnetApplication item = new AspnetApplication();
			
				item.ApplicationName = varApplicationName;
			
				item.LoweredApplicationName = varLoweredApplicationName;
			
				item.ApplicationId = varApplicationId;
			
				item.Description = varDescription;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ApplicationNameColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn LoweredApplicationNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ApplicationName = @"ApplicationName";
			 public static string LoweredApplicationName = @"LoweredApplicationName";
			 public static string ApplicationId = @"ApplicationId";
			 public static string Description = @"Description";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colAspnetMembershipRecords != null)
                {
                    foreach (Wcss.AspnetMembership item in colAspnetMembershipRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colAspnetPaths != null)
                {
                    foreach (Wcss.AspnetPath item in colAspnetPaths)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colAspnetRoles != null)
                {
                    foreach (Wcss.AspnetRole item in colAspnetRoles)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colAspnetUsers != null)
                {
                    foreach (Wcss.AspnetUser item in colAspnetUsers)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colActRecords != null)
                {
                    foreach (Wcss.Act item in colActRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colAgeRecords != null)
                {
                    foreach (Wcss.Age item in colAgeRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colEmailLetterRecords != null)
                {
                    foreach (Wcss.EmailLetter item in colEmailLetterRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colEmployeeRecords != null)
                {
                    foreach (Wcss.Employee item in colEmployeeRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colEventQRecords != null)
                {
                    foreach (Wcss.EventQ item in colEventQRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colFaqCategorieRecords != null)
                {
                    foreach (Wcss.FaqCategorie item in colFaqCategorieRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colHintQuestionRecords != null)
                {
                    foreach (Wcss.HintQuestion item in colHintQuestionRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colMailQueueRecords != null)
                {
                    foreach (Wcss.MailQueue item in colMailQueueRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colPromoterRecords != null)
                {
                    foreach (Wcss.Promoter item in colPromoterRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colKioskRecords != null)
                {
                    foreach (Wcss.Kiosk item in colKioskRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colSalePromotionRecords != null)
                {
                    foreach (Wcss.SalePromotion item in colSalePromotionRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colShowRecords != null)
                {
                    foreach (Wcss.Show item in colShowRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colSiteConfigRecords != null)
                {
                    foreach (Wcss.SiteConfig item in colSiteConfigRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colSubscriptionRecords != null)
                {
                    foreach (Wcss.Subscription item in colSubscriptionRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colVenueRecords != null)
                {
                    foreach (Wcss.Venue item in colVenueRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colAspnetMembershipRecords != null)
                {
                    colAspnetMembershipRecords.SaveAll();
               }
		
                if (colAspnetPaths != null)
                {
                    colAspnetPaths.SaveAll();
               }
		
                if (colAspnetRoles != null)
                {
                    colAspnetRoles.SaveAll();
               }
		
                if (colAspnetUsers != null)
                {
                    colAspnetUsers.SaveAll();
               }
		
                if (colActRecords != null)
                {
                    colActRecords.SaveAll();
               }
		
                if (colAgeRecords != null)
                {
                    colAgeRecords.SaveAll();
               }
		
                if (colEmailLetterRecords != null)
                {
                    colEmailLetterRecords.SaveAll();
               }
		
                if (colEmployeeRecords != null)
                {
                    colEmployeeRecords.SaveAll();
               }
		
                if (colEventQRecords != null)
                {
                    colEventQRecords.SaveAll();
               }
		
                if (colFaqCategorieRecords != null)
                {
                    colFaqCategorieRecords.SaveAll();
               }
		
                if (colHintQuestionRecords != null)
                {
                    colHintQuestionRecords.SaveAll();
               }
		
                if (colMailQueueRecords != null)
                {
                    colMailQueueRecords.SaveAll();
               }
		
                if (colPromoterRecords != null)
                {
                    colPromoterRecords.SaveAll();
               }
		
                if (colKioskRecords != null)
                {
                    colKioskRecords.SaveAll();
               }
		
                if (colSalePromotionRecords != null)
                {
                    colSalePromotionRecords.SaveAll();
               }
		
                if (colShowRecords != null)
                {
                    colShowRecords.SaveAll();
               }
		
                if (colSiteConfigRecords != null)
                {
                    colSiteConfigRecords.SaveAll();
               }
		
                if (colSubscriptionRecords != null)
                {
                    colSubscriptionRecords.SaveAll();
               }
		
                if (colVenueRecords != null)
                {
                    colVenueRecords.SaveAll();
               }
		}
        #endregion
	}
}
