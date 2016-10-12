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
	#region Tables Struct
	public partial struct Tables
	{
		
		public static string Act = @"Act";
        
		public static string Age = @"Age";
        
		public static string AspnetApplication = @"aspnet_Applications";
        
		public static string AspnetMembership = @"aspnet_Membership";
        
		public static string AspnetPath = @"aspnet_Paths";
        
		public static string AspnetPersonalizationAllUser = @"aspnet_PersonalizationAllUsers";
        
		public static string AspnetPersonalizationPerUser = @"aspnet_PersonalizationPerUser";
        
		public static string AspnetProfile = @"aspnet_Profile";
        
		public static string AspnetRole = @"aspnet_Roles";
        
		public static string AspnetSchemaVersion = @"aspnet_SchemaVersions";
        
		public static string AspnetUser = @"aspnet_Users";
        
		public static string AspnetUsersInRole = @"aspnet_UsersInRoles";
        
		public static string AspnetWebEventEvent = @"aspnet_WebEvent_Events";
        
		public static string EmailLetter = @"EmailLetter";
        
		public static string EmailParam = @"EmailParam";
        
		public static string EmailParamArchive = @"EmailParamArchive";
        
		public static string Employee = @"Employee";
        
		public static string EventQ = @"EventQ";
        
		public static string EventQArchive = @"EventQArchive";
        
		public static string FaqCategorie = @"FaqCategorie";
        
		public static string FaqItem = @"FaqItem";
        
		public static string Genre = @"Genre";
        
		public static string HintQuestion = @"HintQuestion";
        
		public static string HistorySubscriptionEmail = @"HistorySubscriptionEmail";
        
		public static string ICalendar = @"ICalendar";
        
		public static string JShowAct = @"JShowAct";
        
		public static string JShowPromoter = @"JShowPromoter";
        
		public static string Kiosk = @"Kiosk";
        
		public static string Mailer = @"Mailer";
        
		public static string MailerContent = @"MailerContent";
        
		public static string MailerTemplate = @"MailerTemplate";
        
		public static string MailerTemplateContent = @"MailerTemplateContent";
        
		public static string MailerTemplateSubstitution = @"MailerTemplateSubstitution";
        
		public static string MailQueue = @"MailQueue";
        
		public static string MailQueueArchive = @"MailQueueArchive";
        
		public static string Post = @"Post";
        
		public static string PrincipalConfig = @"PrincipalConfig";
        
		public static string Promoter = @"Promoter";
        
		public static string SalePromotion = @"SalePromotion";
        
		public static string Search = @"Search";
        
		public static string Show = @"Show";
        
		public static string ShowDate = @"ShowDate";
        
		public static string ShowEvent = @"ShowEvent";
        
		public static string ShowGenre = @"ShowGenre";
        
		public static string ShowLink = @"ShowLink";
        
		public static string ShowStatus = @"ShowStatus";
        
		public static string SiteConfig = @"SiteConfig";
        
		public static string Subscription = @"Subscription";
        
		public static string SubscriptionEmail = @"SubscriptionEmail";
        
		public static string SubscriptionUser = @"SubscriptionUser";
        
		public static string UserPreviousEmail = @"User_PreviousEmail";
        
		public static string UserEvent = @"UserEvent";
        
		public static string VdShowExpense = @"VdShowExpense";
        
		public static string VdShowGenre = @"VdShowGenre";
        
		public static string VdShowInfo = @"VdShowInfo";
        
		public static string VdShowTicket = @"VdShowTicket";
        
		public static string VdShowTicketOutlet = @"VdShowTicketOutlet";
        
		public static string Venue = @"Venue";
        
		public static string Z2Subscription = @"Z2Subscription";
        
		public static string Z2SubscriptionRequest = @"Z2SubscriptionRequest";
        
		public static string Z2SubscriptionTransfer = @"Z2SubscriptionTransfer";
        
	}
	#endregion
    #region Schemas
    public partial class Schemas {
		
		public static TableSchema.Table Act{
            get { return DataService.GetSchema("Act","WillCall"); }
		}
        
		public static TableSchema.Table Age{
            get { return DataService.GetSchema("Age","WillCall"); }
		}
        
		public static TableSchema.Table AspnetApplication{
            get { return DataService.GetSchema("aspnet_Applications","WillCall"); }
		}
        
		public static TableSchema.Table AspnetMembership{
            get { return DataService.GetSchema("aspnet_Membership","WillCall"); }
		}
        
		public static TableSchema.Table AspnetPath{
            get { return DataService.GetSchema("aspnet_Paths","WillCall"); }
		}
        
		public static TableSchema.Table AspnetPersonalizationAllUser{
            get { return DataService.GetSchema("aspnet_PersonalizationAllUsers","WillCall"); }
		}
        
		public static TableSchema.Table AspnetPersonalizationPerUser{
            get { return DataService.GetSchema("aspnet_PersonalizationPerUser","WillCall"); }
		}
        
		public static TableSchema.Table AspnetProfile{
            get { return DataService.GetSchema("aspnet_Profile","WillCall"); }
		}
        
		public static TableSchema.Table AspnetRole{
            get { return DataService.GetSchema("aspnet_Roles","WillCall"); }
		}
        
		public static TableSchema.Table AspnetSchemaVersion{
            get { return DataService.GetSchema("aspnet_SchemaVersions","WillCall"); }
		}
        
		public static TableSchema.Table AspnetUser{
            get { return DataService.GetSchema("aspnet_Users","WillCall"); }
		}
        
		public static TableSchema.Table AspnetUsersInRole{
            get { return DataService.GetSchema("aspnet_UsersInRoles","WillCall"); }
		}
        
		public static TableSchema.Table AspnetWebEventEvent{
            get { return DataService.GetSchema("aspnet_WebEvent_Events","WillCall"); }
		}
        
		public static TableSchema.Table EmailLetter{
            get { return DataService.GetSchema("EmailLetter","WillCall"); }
		}
        
		public static TableSchema.Table EmailParam{
            get { return DataService.GetSchema("EmailParam","WillCall"); }
		}
        
		public static TableSchema.Table EmailParamArchive{
            get { return DataService.GetSchema("EmailParamArchive","WillCall"); }
		}
        
		public static TableSchema.Table Employee{
            get { return DataService.GetSchema("Employee","WillCall"); }
		}
        
		public static TableSchema.Table EventQ{
            get { return DataService.GetSchema("EventQ","WillCall"); }
		}
        
		public static TableSchema.Table EventQArchive{
            get { return DataService.GetSchema("EventQArchive","WillCall"); }
		}
        
		public static TableSchema.Table FaqCategorie{
            get { return DataService.GetSchema("FaqCategorie","WillCall"); }
		}
        
		public static TableSchema.Table FaqItem{
            get { return DataService.GetSchema("FaqItem","WillCall"); }
		}
        
		public static TableSchema.Table Genre{
            get { return DataService.GetSchema("Genre","WillCall"); }
		}
        
		public static TableSchema.Table HintQuestion{
            get { return DataService.GetSchema("HintQuestion","WillCall"); }
		}
        
		public static TableSchema.Table HistorySubscriptionEmail{
            get { return DataService.GetSchema("HistorySubscriptionEmail","WillCall"); }
		}
        
		public static TableSchema.Table ICalendar{
            get { return DataService.GetSchema("ICalendar","WillCall"); }
		}
        
		public static TableSchema.Table JShowAct{
            get { return DataService.GetSchema("JShowAct","WillCall"); }
		}
        
		public static TableSchema.Table JShowPromoter{
            get { return DataService.GetSchema("JShowPromoter","WillCall"); }
		}
        
		public static TableSchema.Table Kiosk{
            get { return DataService.GetSchema("Kiosk","WillCall"); }
		}
        
		public static TableSchema.Table Mailer{
            get { return DataService.GetSchema("Mailer","WillCall"); }
		}
        
		public static TableSchema.Table MailerContent{
            get { return DataService.GetSchema("MailerContent","WillCall"); }
		}
        
		public static TableSchema.Table MailerTemplate{
            get { return DataService.GetSchema("MailerTemplate","WillCall"); }
		}
        
		public static TableSchema.Table MailerTemplateContent{
            get { return DataService.GetSchema("MailerTemplateContent","WillCall"); }
		}
        
		public static TableSchema.Table MailerTemplateSubstitution{
            get { return DataService.GetSchema("MailerTemplateSubstitution","WillCall"); }
		}
        
		public static TableSchema.Table MailQueue{
            get { return DataService.GetSchema("MailQueue","WillCall"); }
		}
        
		public static TableSchema.Table MailQueueArchive{
            get { return DataService.GetSchema("MailQueueArchive","WillCall"); }
		}
        
		public static TableSchema.Table Post{
            get { return DataService.GetSchema("Post","WillCall"); }
		}
        
		public static TableSchema.Table PrincipalConfig{
            get { return DataService.GetSchema("PrincipalConfig","WillCall"); }
		}
        
		public static TableSchema.Table Promoter{
            get { return DataService.GetSchema("Promoter","WillCall"); }
		}
        
		public static TableSchema.Table SalePromotion{
            get { return DataService.GetSchema("SalePromotion","WillCall"); }
		}
        
		public static TableSchema.Table Search{
            get { return DataService.GetSchema("Search","WillCall"); }
		}
        
		public static TableSchema.Table Show{
            get { return DataService.GetSchema("Show","WillCall"); }
		}
        
		public static TableSchema.Table ShowDate{
            get { return DataService.GetSchema("ShowDate","WillCall"); }
		}
        
		public static TableSchema.Table ShowEvent{
            get { return DataService.GetSchema("ShowEvent","WillCall"); }
		}
        
		public static TableSchema.Table ShowGenre{
            get { return DataService.GetSchema("ShowGenre","WillCall"); }
		}
        
		public static TableSchema.Table ShowLink{
            get { return DataService.GetSchema("ShowLink","WillCall"); }
		}
        
		public static TableSchema.Table ShowStatus{
            get { return DataService.GetSchema("ShowStatus","WillCall"); }
		}
        
		public static TableSchema.Table SiteConfig{
            get { return DataService.GetSchema("SiteConfig","WillCall"); }
		}
        
		public static TableSchema.Table Subscription{
            get { return DataService.GetSchema("Subscription","WillCall"); }
		}
        
		public static TableSchema.Table SubscriptionEmail{
            get { return DataService.GetSchema("SubscriptionEmail","WillCall"); }
		}
        
		public static TableSchema.Table SubscriptionUser{
            get { return DataService.GetSchema("SubscriptionUser","WillCall"); }
		}
        
		public static TableSchema.Table UserPreviousEmail{
            get { return DataService.GetSchema("User_PreviousEmail","WillCall"); }
		}
        
		public static TableSchema.Table UserEvent{
            get { return DataService.GetSchema("UserEvent","WillCall"); }
		}
        
		public static TableSchema.Table VdShowExpense{
            get { return DataService.GetSchema("VdShowExpense","WillCall"); }
		}
        
		public static TableSchema.Table VdShowGenre{
            get { return DataService.GetSchema("VdShowGenre","WillCall"); }
		}
        
		public static TableSchema.Table VdShowInfo{
            get { return DataService.GetSchema("VdShowInfo","WillCall"); }
		}
        
		public static TableSchema.Table VdShowTicket{
            get { return DataService.GetSchema("VdShowTicket","WillCall"); }
		}
        
		public static TableSchema.Table VdShowTicketOutlet{
            get { return DataService.GetSchema("VdShowTicketOutlet","WillCall"); }
		}
        
		public static TableSchema.Table Venue{
            get { return DataService.GetSchema("Venue","WillCall"); }
		}
        
		public static TableSchema.Table Z2Subscription{
            get { return DataService.GetSchema("Z2Subscription","WillCall"); }
		}
        
		public static TableSchema.Table Z2SubscriptionRequest{
            get { return DataService.GetSchema("Z2SubscriptionRequest","WillCall"); }
		}
        
		public static TableSchema.Table Z2SubscriptionTransfer{
            get { return DataService.GetSchema("Z2SubscriptionTransfer","WillCall"); }
		}
        
	
    }
    #endregion
    #region View Struct
    public partial struct Views 
    {
		
		public static string VwAspnetApplication = @"vw_aspnet_Applications";
        
		public static string VwAspnetMembershipUser = @"vw_aspnet_MembershipUsers";
        
		public static string VwAspnetProfile = @"vw_aspnet_Profiles";
        
		public static string VwAspnetRole = @"vw_aspnet_Roles";
        
		public static string VwAspnetUser = @"vw_aspnet_Users";
        
		public static string VwAspnetUsersInRole = @"vw_aspnet_UsersInRoles";
        
		public static string VwAspnetWebPartStatePath = @"vw_aspnet_WebPartState_Paths";
        
		public static string VwAspnetWebPartStateShared = @"vw_aspnet_WebPartState_Shared";
        
		public static string VwAspnetWebPartStateUser = @"vw_aspnet_WebPartState_User";
        
    }
    #endregion
    
    #region Query Factories
	public static partial class DB
	{
        public static DataProvider _provider = DataService.Providers["WillCall"];
        static ISubSonicRepository _repository;
        public static ISubSonicRepository Repository {
            get {
                if (_repository == null)
                    return new SubSonicRepository(_provider);
                return _repository; 
            }
            set { _repository = value; }
        }
	
        public static Select SelectAllColumnsFrom<T>() where T : RecordBase<T>, new()
	    {
            return Repository.SelectAllColumnsFrom<T>();
            
	    }
	    public static Select Select()
	    {
            return Repository.Select();
	    }
	    
		public static Select Select(params string[] columns)
		{
            return Repository.Select(columns);
        }
	    
		public static Select Select(params Aggregate[] aggregates)
		{
            return Repository.Select(aggregates);
        }
   
	    public static Update Update<T>() where T : RecordBase<T>, new()
	    {
            return Repository.Update<T>();
	    }
     
	    
	    public static Insert Insert()
	    {
            return Repository.Insert();
	    }
	    
	    public static Delete Delete()
	    {
            
            return Repository.Delete();
	    }
	    
	    public static InlineQuery Query()
	    {
            
            return Repository.Query();
	    }
	    	    
	    
	}
    #endregion
    
}
namespace Erlg
{
	#region Tables Struct
	public partial struct Tables
	{
		
		public static string Log = @"Log";
        
		public static string LogArchive = @"LogArchive";
        
	}
	#endregion
    #region Schemas
    public partial class Schemas {
		
		public static TableSchema.Table Log{
            get { return DataService.GetSchema("Log","ErrorLog"); }
		}
        
		public static TableSchema.Table LogArchive{
            get { return DataService.GetSchema("LogArchive","ErrorLog"); }
		}
        
	
    }
    #endregion
    #region View Struct
    public partial struct Views 
    {
		
    }
    #endregion
    
    #region Query Factories
	public static partial class DB
	{
        public static DataProvider _provider = DataService.Providers["ErrorLog"];
        static ISubSonicRepository _repository;
        public static ISubSonicRepository Repository {
            get {
                if (_repository == null)
                    return new SubSonicRepository(_provider);
                return _repository; 
            }
            set { _repository = value; }
        }
	
        public static Select SelectAllColumnsFrom<T>() where T : RecordBase<T>, new()
	    {
            return Repository.SelectAllColumnsFrom<T>();
            
	    }
	    public static Select Select()
	    {
            return Repository.Select();
	    }
	    
		public static Select Select(params string[] columns)
		{
            return Repository.Select(columns);
        }
	    
		public static Select Select(params Aggregate[] aggregates)
		{
            return Repository.Select(aggregates);
        }
   
	    public static Update Update<T>() where T : RecordBase<T>, new()
	    {
            return Repository.Update<T>();
	    }
     
	    
	    public static Insert Insert()
	    {
            return Repository.Insert();
	    }
	    
	    public static Delete Delete()
	    {
            
            return Repository.Delete();
	    }
	    
	    public static InlineQuery Query()
	    {
            
            return Repository.Query();
	    }
	    	    
	    
	}
    #endregion
    
}
#region Databases
public partial struct Databases 
{
	
	public static string WillCall = @"WillCall";
    
	public static string ErrorLog = @"ErrorLog";
    
}
#endregion