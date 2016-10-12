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
namespace Wcss{
    public partial class SPs{
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_AnyDataInTables Procedure
        /// </summary>
        public static StoredProcedure AspnetAnyDataInTables(int? TablesToCheck)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_AnyDataInTables", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@TablesToCheck", TablesToCheck, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Applications_CreateApplication Procedure
        /// </summary>
        public static StoredProcedure AspnetApplicationsCreateApplication(string ApplicationName, Guid? ApplicationId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Applications_CreateApplication", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddOutputParameter("@ApplicationId", DbType.Guid, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_CheckSchemaVersion Procedure
        /// </summary>
        public static StoredProcedure AspnetCheckSchemaVersion(string Feature, string CompatibleSchemaVersion)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_CheckSchemaVersion", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Feature", Feature, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CompatibleSchemaVersion", CompatibleSchemaVersion, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_ChangePasswordQuestionAndAnswer Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipChangePasswordQuestionAndAnswer(string ApplicationName, string UserName, string NewPasswordQuestion, string NewPasswordAnswer)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_ChangePasswordQuestionAndAnswer", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@NewPasswordQuestion", NewPasswordQuestion, DbType.String, null, null);
        	
            sp.Command.AddParameter("@NewPasswordAnswer", NewPasswordAnswer, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_CreateUser Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipCreateUser(string ApplicationName, string UserName, string Password, string PasswordSalt, string Email, string PasswordQuestion, string PasswordAnswer, bool? IsApproved, DateTime? CurrentTimeUtc, DateTime? CreateDate, int? UniqueEmail, int? PasswordFormat, Guid? UserId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_CreateUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Password", Password, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PasswordSalt", PasswordSalt, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Email", Email, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PasswordQuestion", PasswordQuestion, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PasswordAnswer", PasswordAnswer, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsApproved", IsApproved, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@CreateDate", CreateDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UniqueEmail", UniqueEmail, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordFormat", PasswordFormat, DbType.Int32, 0, 10);
        	
            sp.Command.AddOutputParameter("@UserId", DbType.Guid, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_FindUsers_LIKE_ProfileParameter Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipFindUsersLikeProfileParameter(string ApplicationName, string ParamName, string ParamValue, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_FindUsers_LIKE_ProfileParameter", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ParamName", ParamName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ParamValue", ParamValue, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_FindUsersByEmail Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipFindUsersByEmail(string ApplicationName, string EmailToMatch, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_FindUsersByEmail", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@EmailToMatch", EmailToMatch, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_FindUsersByName Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipFindUsersByName(string ApplicationName, string UserNameToMatch, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_FindUsersByName", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserNameToMatch", UserNameToMatch, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_FindUsersByProfileParameter Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipFindUsersByProfileParameter(string ApplicationName, string ParamName, string ParamValue, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_FindUsersByProfileParameter", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ParamName", ParamName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ParamValue", ParamValue, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetAllUsers Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetAllUsers(string ApplicationName, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetAllUsers", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetNumberOfUsersOnline Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetNumberOfUsersOnline(string ApplicationName, int? MinutesSinceLastInActive, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetNumberOfUsersOnline", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@MinutesSinceLastInActive", MinutesSinceLastInActive, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetPassword Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetPassword(string ApplicationName, string UserName, int? MaxInvalidPasswordAttempts, int? PasswordAttemptWindow, DateTime? CurrentTimeUtc, string PasswordAnswer)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetPassword", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@MaxInvalidPasswordAttempts", MaxInvalidPasswordAttempts, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordAttemptWindow", PasswordAttemptWindow, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PasswordAnswer", PasswordAnswer, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetPasswordWithFormat Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetPasswordWithFormat(string ApplicationName, string UserName, bool? UpdateLastLoginActivityDate, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetPasswordWithFormat", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UpdateLastLoginActivityDate", UpdateLastLoginActivityDate, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetUserByEmail Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetUserByEmail(string ApplicationName, string Email)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetUserByEmail", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Email", Email, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetUserByName Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetUserByName(string ApplicationName, string UserName, DateTime? CurrentTimeUtc, bool? UpdateLastActivity)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetUserByName", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UpdateLastActivity", UpdateLastActivity, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_GetUserByUserId Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipGetUserByUserId(Guid? UserId, DateTime? CurrentTimeUtc, bool? UpdateLastActivity)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_GetUserByUserId", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@UserId", UserId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UpdateLastActivity", UpdateLastActivity, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_ResetPassword Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipResetPassword(string ApplicationName, string UserName, string NewPassword, int? MaxInvalidPasswordAttempts, int? PasswordAttemptWindow, string PasswordSalt, DateTime? CurrentTimeUtc, int? PasswordFormat, string PasswordAnswer)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_ResetPassword", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@NewPassword", NewPassword, DbType.String, null, null);
        	
            sp.Command.AddParameter("@MaxInvalidPasswordAttempts", MaxInvalidPasswordAttempts, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordAttemptWindow", PasswordAttemptWindow, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordSalt", PasswordSalt, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PasswordFormat", PasswordFormat, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordAnswer", PasswordAnswer, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_SetPassword Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipSetPassword(string ApplicationName, string UserName, string NewPassword, string PasswordSalt, DateTime? CurrentTimeUtc, int? PasswordFormat)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_SetPassword", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@NewPassword", NewPassword, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PasswordSalt", PasswordSalt, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PasswordFormat", PasswordFormat, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_UnlockUser Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipUnlockUser(string ApplicationName, string UserName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_UnlockUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_UpdateUser Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipUpdateUser(string ApplicationName, string UserName, string Email, string Comment, bool? IsApproved, DateTime? LastLoginDate, DateTime? LastActivityDate, int? UniqueEmail, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_UpdateUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Email", Email, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Comment", Comment, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsApproved", IsApproved, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@LastLoginDate", LastLoginDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@LastActivityDate", LastActivityDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UniqueEmail", UniqueEmail, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Membership_UpdateUserInfo Procedure
        /// </summary>
        public static StoredProcedure AspnetMembershipUpdateUserInfo(string ApplicationName, string UserName, bool? IsPasswordCorrect, bool? UpdateLastLoginActivityDate, int? MaxInvalidPasswordAttempts, int? PasswordAttemptWindow, DateTime? CurrentTimeUtc, DateTime? LastLoginDate, DateTime? LastActivityDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Membership_UpdateUserInfo", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsPasswordCorrect", IsPasswordCorrect, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@UpdateLastLoginActivityDate", UpdateLastLoginActivityDate, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@MaxInvalidPasswordAttempts", MaxInvalidPasswordAttempts, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PasswordAttemptWindow", PasswordAttemptWindow, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@LastLoginDate", LastLoginDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@LastActivityDate", LastActivityDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Paths_CreatePath Procedure
        /// </summary>
        public static StoredProcedure AspnetPathsCreatePath(Guid? ApplicationId, string Path, Guid? PathId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Paths_CreatePath", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationId", ApplicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddOutputParameter("@PathId", DbType.Guid, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Personalization_GetApplicationId Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationGetApplicationId(string ApplicationName, Guid? ApplicationId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Personalization_GetApplicationId", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddOutputParameter("@ApplicationId", DbType.Guid, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAdministration_DeleteAllState Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAdministrationDeleteAllState(bool? AllUsersScope, string ApplicationName, int? Count)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAdministration_DeleteAllState", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@AllUsersScope", AllUsersScope, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddOutputParameter("@Count", DbType.Int32, 0, 10);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAdministration_FindState Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAdministrationFindState(bool? AllUsersScope, string ApplicationName, int? PageIndex, int? PageSize, string Path, string UserName, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAdministration_FindState", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@AllUsersScope", AllUsersScope, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAdministration_GetCountOfState Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAdministrationGetCountOfState(int? Count, bool? AllUsersScope, string ApplicationName, string Path, string UserName, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAdministration_GetCountOfState", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddOutputParameter("@Count", DbType.Int32, 0, 10);
            
            sp.Command.AddParameter("@AllUsersScope", AllUsersScope, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAdministration_ResetSharedState Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAdministrationResetSharedState(int? Count, string ApplicationName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAdministration_ResetSharedState", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddOutputParameter("@Count", DbType.Int32, 0, 10);
            
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAdministration_ResetUserState Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAdministrationResetUserState(int? Count, string ApplicationName, DateTime? InactiveSinceDate, string UserName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAdministration_ResetUserState", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddOutputParameter("@Count", DbType.Int32, 0, 10);
            
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAllUsers_GetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAllUsersGetPageSettings(string ApplicationName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAllUsers_GetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAllUsers_ResetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAllUsersResetPageSettings(string ApplicationName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAllUsers_ResetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationAllUsers_SetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationAllUsersSetPageSettings(string ApplicationName, string Path, byte[] PageSettings, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationAllUsers_SetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageSettings", PageSettings, DbType.Binary, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationPerUser_GetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationPerUserGetPageSettings(string ApplicationName, string UserName, string Path, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationPerUser_GetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationPerUser_ResetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationPerUserResetPageSettings(string ApplicationName, string UserName, string Path, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationPerUser_ResetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_PersonalizationPerUser_SetPageSettings Procedure
        /// </summary>
        public static StoredProcedure AspnetPersonalizationPerUserSetPageSettings(string ApplicationName, string UserName, string Path, byte[] PageSettings, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_PersonalizationPerUser_SetPageSettings", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Path", Path, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PageSettings", PageSettings, DbType.Binary, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_DeleteInactiveProfiles Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileDeleteInactiveProfiles(string ApplicationName, int? ProfileAuthOptions, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_DeleteInactiveProfiles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ProfileAuthOptions", ProfileAuthOptions, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_DeleteProfiles Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileDeleteProfiles(string ApplicationName, string UserNames)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_DeleteProfiles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserNames", UserNames, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_GetNumberOfInactiveProfiles Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileGetNumberOfInactiveProfiles(string ApplicationName, int? ProfileAuthOptions, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_GetNumberOfInactiveProfiles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ProfileAuthOptions", ProfileAuthOptions, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_GetProfiles Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileGetProfiles(string ApplicationName, int? ProfileAuthOptions, int? PageIndex, int? PageSize, string UserNameToMatch, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_GetProfiles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ProfileAuthOptions", ProfileAuthOptions, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageIndex", PageIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@UserNameToMatch", UserNameToMatch, DbType.String, null, null);
        	
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_GetProperties Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileGetProperties(string ApplicationName, string UserName, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_GetProperties", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Profile_SetProperties Procedure
        /// </summary>
        public static StoredProcedure AspnetProfileSetProperties(string ApplicationName, string PropertyNames, string PropertyValuesString, byte[] PropertyValuesBinary, string UserName, bool? IsUserAnonymous, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Profile_SetProperties", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PropertyNames", PropertyNames, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PropertyValuesString", PropertyValuesString, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PropertyValuesBinary", PropertyValuesBinary, DbType.Binary, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsUserAnonymous", IsUserAnonymous, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_RegisterSchemaVersion Procedure
        /// </summary>
        public static StoredProcedure AspnetRegisterSchemaVersion(string Feature, string CompatibleSchemaVersion, bool? IsCurrentVersion, bool? RemoveIncompatibleSchema)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_RegisterSchemaVersion", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Feature", Feature, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CompatibleSchemaVersion", CompatibleSchemaVersion, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsCurrentVersion", IsCurrentVersion, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@RemoveIncompatibleSchema", RemoveIncompatibleSchema, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Roles_CreateRole Procedure
        /// </summary>
        public static StoredProcedure AspnetRolesCreateRole(string ApplicationName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Roles_CreateRole", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Roles_DeleteRole Procedure
        /// </summary>
        public static StoredProcedure AspnetRolesDeleteRole(string ApplicationName, string RoleName, bool? DeleteOnlyIfRoleIsEmpty)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Roles_DeleteRole", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@DeleteOnlyIfRoleIsEmpty", DeleteOnlyIfRoleIsEmpty, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Roles_GetAllRoles Procedure
        /// </summary>
        public static StoredProcedure AspnetRolesGetAllRoles(string ApplicationName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Roles_GetAllRoles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Roles_RoleExists Procedure
        /// </summary>
        public static StoredProcedure AspnetRolesRoleExists(string ApplicationName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Roles_RoleExists", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Setup_RemoveAllRoleMembers Procedure
        /// </summary>
        public static StoredProcedure AspnetSetupRemoveAllRoleMembers(string name)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Setup_RemoveAllRoleMembers", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@name", name, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Setup_RestorePermissions Procedure
        /// </summary>
        public static StoredProcedure AspnetSetupRestorePermissions(string name)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Setup_RestorePermissions", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@name", name, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UnRegisterSchemaVersion Procedure
        /// </summary>
        public static StoredProcedure AspnetUnRegisterSchemaVersion(string Feature, string CompatibleSchemaVersion)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UnRegisterSchemaVersion", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Feature", Feature, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CompatibleSchemaVersion", CompatibleSchemaVersion, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Users_CreateUser Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersCreateUser(Guid? ApplicationId, string UserName, bool? IsUserAnonymous, DateTime? LastActivityDate, Guid? UserId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Users_CreateUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationId", ApplicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsUserAnonymous", IsUserAnonymous, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@LastActivityDate", LastActivityDate, DbType.DateTime, null, null);
        	
            sp.Command.AddOutputParameter("@UserId", DbType.Guid, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_Users_DeleteUser Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersDeleteUser(string ApplicationName, string UserName, int? TablesToDeleteFrom, int? NumTablesDeletedFrom)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_Users_DeleteUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@TablesToDeleteFrom", TablesToDeleteFrom, DbType.Int32, 0, 10);
        	
            sp.Command.AddOutputParameter("@NumTablesDeletedFrom", DbType.Int32, 0, 10);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_AddUsersToRoles Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesAddUsersToRoles(string ApplicationName, string UserNames, string RoleNames, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_AddUsersToRoles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserNames", UserNames, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleNames", RoleNames, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_FindUsersInRole Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesFindUsersInRole(string ApplicationName, string RoleName, string UserNameToMatch)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_FindUsersInRole", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserNameToMatch", UserNameToMatch, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_GetRolesForUser Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesGetRolesForUser(string ApplicationName, string UserName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_GetRolesForUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_GetUsersInRoles Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesGetUsersInRoles(string ApplicationName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_GetUsersInRoles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_IsUserInRole Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesIsUserInRole(string ApplicationName, string UserName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_IsUserInRole", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserName", UserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleName", RoleName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_UsersInRoles_RemoveUsersFromRoles Procedure
        /// </summary>
        public static StoredProcedure AspnetUsersInRolesRemoveUsersFromRoles(string ApplicationName, string UserNames, string RoleNames)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_UsersInRoles_RemoveUsersFromRoles", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@UserNames", UserNames, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RoleNames", RoleNames, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the aspnet_WebEvent_LogEvent Procedure
        /// </summary>
        public static StoredProcedure AspnetWebEventLogEvent(string EventId, DateTime? EventTimeUtc, DateTime? EventTime, string EventType, decimal? EventSequence, decimal? EventOccurrence, int? EventCode, int? EventDetailCode, string Message, string ApplicationPath, string ApplicationVirtualPath, string MachineName, string RequestUrl, string ExceptionType, string Details)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("aspnet_WebEvent_LogEvent", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@EventId", EventId, DbType.AnsiStringFixedLength, null, null);
        	
            sp.Command.AddParameter("@EventTimeUtc", EventTimeUtc, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EventTime", EventTime, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EventType", EventType, DbType.String, null, null);
        	
            sp.Command.AddParameter("@EventSequence", EventSequence, DbType.Decimal, 0, 19);
        	
            sp.Command.AddParameter("@EventOccurrence", EventOccurrence, DbType.Decimal, 0, 19);
        	
            sp.Command.AddParameter("@EventCode", EventCode, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@EventDetailCode", EventDetailCode, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Message", Message, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ApplicationPath", ApplicationPath, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ApplicationVirtualPath", ApplicationVirtualPath, DbType.String, null, null);
        	
            sp.Command.AddParameter("@MachineName", MachineName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@RequestUrl", RequestUrl, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ExceptionType", ExceptionType, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Details", Details, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetEmployeesInContext Procedure
        /// </summary>
        public static StoredProcedure TxGetEmployeesInContext(int? StartRowIndex, int? PageSize, string Principal, string Status, string SearchTerms)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetEmployeesInContext", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@SearchTerms", SearchTerms, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetEmployeesInContextCount Procedure
        /// </summary>
        public static StoredProcedure TxGetEmployeesInContextCount(string Principal, string Status, string SearchTerms)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetEmployeesInContextCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@SearchTerms", SearchTerms, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetFaqsInContext Procedure
        /// </summary>
        public static StoredProcedure TxGetFaqsInContext(int? StartRowIndex, int? PageSize, string Principal, string Status, string Category, string SearchTerms)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetFaqsInContext", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@SearchTerms", SearchTerms, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetFaqsInContextCount Procedure
        /// </summary>
        public static StoredProcedure TxGetFaqsInContextCount(string Principal, string Status, string Category, string SearchTerms)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetFaqsInContextCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@SearchTerms", SearchTerms, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetJustAnnouncedShows Procedure
        /// </summary>
        public static StoredProcedure TxGetJustAnnouncedShows(Guid? applicationId, string principal, DateTime? minAnnounceDate, DateTime? minShowDate, int? startRowIndex, int? pageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetJustAnnouncedShows", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@principal", principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@minAnnounceDate", minAnnounceDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@minShowDate", minShowDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@startRowIndex", startRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@pageSize", pageSize, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetKiosksInContext Procedure
        /// </summary>
        public static StoredProcedure TxGetKiosksInContext(int? StartRowIndex, int? PageSize, string Principal, string Status, DateTime? StartDate, DateTime? EndDate, string SearchTerms)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetKiosksInContext", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@SearchTerms", SearchTerms, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetKiosksInContextCount Procedure
        /// </summary>
        public static StoredProcedure TxGetKiosksInContextCount(string Principal, string Status, DateTime? StartDate, DateTime? EndDate, string SearchTerms)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetKiosksInContextCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@SearchTerms", SearchTerms, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetPostsInContext Procedure
        /// </summary>
        public static StoredProcedure TxGetPostsInContext(int? StartRowIndex, int? PageSize, string Principal, string Status, string SearchTerms)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetPostsInContext", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@SearchTerms", SearchTerms, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetPostsInContextCount Procedure
        /// </summary>
        public static StoredProcedure TxGetPostsInContextCount(string Principal, string Status, string SearchTerms)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetPostsInContextCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@SearchTerms", SearchTerms, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetSalePromotionsInContext Procedure
        /// </summary>
        public static StoredProcedure TxGetSalePromotionsInContext(int? StartRowIndex, int? PageSize, string Principal, string Status, DateTime? StartDate, DateTime? EndDate, string SearchTerms)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetSalePromotionsInContext", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@StartRowIndex", StartRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@PageSize", PageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@SearchTerms", SearchTerms, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetSalePromotionsInContextCount Procedure
        /// </summary>
        public static StoredProcedure TxGetSalePromotionsInContextCount(string Principal, string Status, DateTime? StartDate, DateTime? EndDate, string SearchTerms)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetSalePromotionsInContextCount", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@SearchTerms", SearchTerms, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetSaleShowDates Procedure
        /// </summary>
        public static StoredProcedure TxGetSaleShowDates(Guid? applicationId, string nowName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetSaleShowDates", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@nowName", nowName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetShowByUrl Procedure
        /// </summary>
        public static StoredProcedure TxGetShowByUrl(Guid? applicationId, string url, bool? checkActiveDisplayable)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetShowByUrl", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@url", url, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@checkActiveDisplayable", checkActiveDisplayable, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetShowDatesInRange Procedure
        /// </summary>
        public static StoredProcedure TxGetShowDatesInRange(Guid? applicationId, string defaultVenue, int? selectedVenueId, string startDate, string endDate, int? startRowIndex, int? pageSize, bool? returnSimpleRows, bool? returnShowDateRows, bool? returnTicketRows)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetShowDatesInRange", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@defaultVenue", defaultVenue, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@selectedVenueId", selectedVenueId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@startDate", startDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@endDate", endDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@startRowIndex", startRowIndex, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@pageSize", pageSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@returnSimpleRows", returnSimpleRows, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@returnShowDateRows", returnShowDateRows, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@returnTicketRows", returnTicketRows, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetShowDatesInRange_Count Procedure
        /// </summary>
        public static StoredProcedure TxGetShowDatesInRangeCount(Guid? applicationId, int? selectedVenueId, string startDate, string endDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetShowDatesInRange_Count", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@selectedVenueId", selectedVenueId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@startDate", startDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@endDate", endDate, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_GetTypeahead_Suggestions Procedure
        /// </summary>
        public static StoredProcedure TxGetTypeaheadSuggestions(string Principal, string Context, string Query, bool? ActiveRequired, int? Limit)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_GetTypeahead_Suggestions", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Principal", Principal, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Context", Context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Query", Query, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ActiveRequired", ActiveRequired, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@Limit", Limit, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_JOB_GetBatch_EventData Procedure
        /// </summary>
        public static StoredProcedure TxJobGetBatchEventData(Guid? applicationId, Guid? threadGuid, int? batchSize, int? retrySeconds, int? archiveAfterDays)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_JOB_GetBatch_EventData", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@threadGuid", threadGuid, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@batchSize", batchSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@retrySeconds", retrySeconds, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@archiveAfterDays", archiveAfterDays, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_JOB_GetBatch_MailData Procedure
        /// </summary>
        public static StoredProcedure TxJobGetBatchMailData(Guid? applicationId, Guid? threadGuid, int? batchSize, int? retrySeconds, int? archiveAfterDays)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_JOB_GetBatch_MailData", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@threadGuid", threadGuid, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@batchSize", batchSize, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@retrySeconds", retrySeconds, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@archiveAfterDays", archiveAfterDays, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_JOB_MailReport Procedure
        /// </summary>
        public static StoredProcedure TxJobMailReport(string applicationName, bool? isDaily)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_JOB_MailReport", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationName", applicationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@isDaily", isDaily, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Mailer_LetterStats Procedure
        /// </summary>
        public static StoredProcedure TxMailerLetterStats(Guid? appId, string StartDate, string EndDate, int? letterId, int? queued, int? sent, int? total)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Mailer_LetterStats", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@letterId", letterId, DbType.Int32, 0, 10);
        	
            sp.Command.AddOutputParameter("@queued", DbType.Int32, 0, 10);
            
            sp.Command.AddOutputParameter("@sent", DbType.Int32, 0, 10);
            
            sp.Command.AddOutputParameter("@total", DbType.Int32, 0, 10);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Mailer_ProcessCustomEmailerList Procedure
        /// </summary>
        public static StoredProcedure TxMailerProcessCustomEmailerList(Guid? appId, string emailList)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Mailer_ProcessCustomEmailerList", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@emailList", emailList, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Picture_Update Procedure
        /// </summary>
        public static StoredProcedure TxPictureUpdate(int? Idx, string Context, int? Width, int? Height)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Picture_Update", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@Idx", Idx, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Context", Context, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Width", Width, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Height", Height, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_SendEmailTemplate Procedure
        /// </summary>
        public static StoredProcedure TxSendEmailTemplate(Guid? applicationId, string emailTemplate, string sendDate, string fromName, string fromAddress, string toAddress, string paramNames, string paramValues, string bccEmail, int? priority, string result)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_SendEmailTemplate", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@emailTemplate", emailTemplate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sendDate", sendDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@fromName", fromName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@fromAddress", fromAddress, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@toAddress", toAddress, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@paramNames", paramNames, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@paramValues", paramValues, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@bccEmail", bccEmail, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@priority", priority, DbType.Int32, 0, 10);
        	
            sp.Command.AddOutputParameter("@result", DbType.AnsiString, null, null);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_GetSubsForUser Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionGetSubsForUser(Guid? appId, string userName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_GetSubsForUser", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@userName", userName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_InsertMailerIntoQueue Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionInsertMailerIntoQueue(Guid? applicationId, int? subscriptionEmailId, string sendDate, string fromName, string fromAddress, int? priority)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_InsertMailerIntoQueue", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@subscriptionEmailId", subscriptionEmailId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@sendDate", sendDate, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@fromName", fromName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@fromAddress", fromAddress, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@priority", priority, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_PauseMailerInQueue Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionPauseMailerInQueue(int? subscriptionEmailId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_PauseMailerInQueue", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@subscriptionEmailId", subscriptionEmailId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_RemoveMailerFromQueue Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionRemoveMailerFromQueue(int? subscriptionEmailId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_RemoveMailerFromQueue", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@subscriptionEmailId", subscriptionEmailId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_RemoveUserFromUnauthorizedSubs Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionRemoveUserFromUnauthorizedSubs(Guid? appId, string userName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_RemoveUserFromUnauthorizedSubs", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@appId", appId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@userName", userName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Subscription_RestartMailerInQueue Procedure
        /// </summary>
        public static StoredProcedure TxSubscriptionRestartMailerInQueue(int? subscriptionEmailId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Subscription_RestartMailerInQueue", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@subscriptionEmailId", subscriptionEmailId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_User_HasMembership Procedure
        /// </summary>
        public static StoredProcedure TxUserHasMembership(Guid? applicationId, string userName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_User_HasMembership", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@applicationId", applicationId, DbType.Guid, null, null);
        	
            sp.Command.AddParameter("@userName", userName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the tx_Z2SubscriptionUpdate Procedure
        /// </summary>
        public static StoredProcedure TxZ2SubscriptionUpdate(string email, string sourcePage, string subRequest, string ipAddress, string initialsourceQuery)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("tx_Z2SubscriptionUpdate", DataService.GetInstance("WillCall"), "dbo");
        	
            sp.Command.AddParameter("@email", email, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sourcePage", sourcePage, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@subRequest", subRequest, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ipAddress", ipAddress, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@initialsourceQuery", initialsourceQuery, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
    }
    
}
