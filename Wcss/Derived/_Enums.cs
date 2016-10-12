using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

namespace Wcss
{
    public class _Enums
    {

        //Associating Strings with enums in C#
        //http://blog.spontaneouspublicity.com/associating-strings-with-enums-in-c
        /// <summary>
        /// Gets the description based on the Enum Key
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static object GetEnumFromDescription(Type enumType, string description, string defaultEnum)
        {
            foreach (Enum value in Enum.GetValues(enumType))
                if (GetDescription(value).CompareTo(description) == 0)
                    return value;

            return Enum.Parse(enumType, defaultEnum, true);
        }

        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

        //********************************

        //***************************************************************************************
        //***************************************************************************************
        // in Controls.js
        // keep entities in sync
        // function getCooKeyInContext(context, entityType) {
        // note the convention of cooKeyName + entity type name
        //***************************************************************************************
        //***************************************************************************************

        /// <summary>
        /// Cookies that have keys registered in _Enums for use in js, etc, cross-platform access
        /// </summary>
        public class CookEnums
        {
            public enum ShowEditContext
            {
                [Description("sel")]
                Selection,
                //[Description("new")]
                //New,
                [Description("det")]
                Details,
                [Description("wrt")]
                Writeup,
                [Description("act")]
                Acts,
                [Description("img")]
                Images,                
                [Description("prm")]
                Promoters,
                [Description("lnk")]
                Links,
                [Description("dta")]
                Data
            }

            public enum AdminNavTabContextCookie
            {
                [Description("rakebnr")]
                ActiveEditBannerContextTab,
                [Description("rakeksk")]
                ActiveEditKioskContextTab,
                [Description("rakepst")]
                ActiveEditPostContextTab,
                [Description("rakeemp")]
                ActiveEditEmployeeContextTab,
                [Description("rakefaq")]
                ActiveEditFaqContextTab,
                [Description("rakesho")]
                ActiveEditShowContextTab,
                [Description("rakeact")]
                ActiveEditActContextTab,
                [Description("rakeprm")]
                ActiveEditPromoterContextTab,
                [Description("rakevnu")]
                ActiveEditVenueContextTab
            }

            public enum AdminNavTabContainerCookie
            {
                [Description("ractbnr")]
                ActiveEditBannerContainerTab,
                [Description("ractksk")]
                ActiveEditKioskContainerTab,
                [Description("ractpst")]
                ActiveEditPostContainerTab,
                [Description("ractemp")]
                ActiveEditEmployeeContainerTab,
                [Description("ractfaq")]
                ActiveEditFaqContainerTab,
                [Description("ractsho")]
                ActiveEditShowContainerTab,
                [Description("ractact")]
                ActiveEditActContainerTab,
                [Description("ractprm")]
                ActiveEditPromoterContainerTab,
                [Description("ractvnu")]
                ActiveEditVenueContainerTab
            }

            public enum AdminCollectionCriteriaCookie
            {
                [Description("acccbnr")]
                CollectionCriteria_Banner,
                [Description("acccksk")]
                CollectionCriteria_Kiosk,
                [Description("acccpst")]
                CollectionCriteria_Post,                
                [Description("acccemp")]
                CollectionCriteria_Employee,
                [Description("acccfaq")]
                CollectionCriteria_Faq,
                [Description("acccsho")]
                CollectionCriteria_Show,
                [Description("acccact")]
                CollectionCriteria_Act,
                [Description("acccprm")]
                CollectionCriteria_Promoter,
                [Description("acccvnu")]
                CollectionCriteria_Venue
            }

            public class Z2Cookies
            {
                public enum SessionCookies
                {
                    [Description("mktpk")]
                    MarketingProgramKey
                }
            }            
        }

        public enum JustAnnouncedStatus 
        {
            normal,
            include,
            remove        
        }

        /// <summary>
        /// 
        /// </summary>
        public enum CollectionSearchCriteriaStatusType
        {
            [Description("not active")]
            notactive = 0,
            [Description("active")]
            active = 1,
            [Description("all")]
            all = 2,
            [Description("orderable")]
            orderable = 3
        }

        /// <summary>
        /// A short CODE for a venue principal
        /// </summary>
        public enum Principal
        {
            all,
            fox,
            bt,
            chq,
            z2
        }

        public enum MailTemplateTypes
        {
            Is3rdPartySender
        }

        public enum MailProcessorContext
        {
            test,
            customlist,
            subscription
        }

        /// <summary>
        /// Must also register for handling in wctMain.Svc.controlrenderer
        /// </summary>
        public enum StaticPage
        {
            faq, contact, directions, mailerconfirm, mailermanage, subscribe, subscribemailup, unsubscribe, unsubscribemailup, privacy, terms, history, parking, production, about, accommodations, textus, studentlaminate
        }

        public enum RelationContext //- this will just be table name
        {
            NA,
            ShowDate
        }
        public enum FlagName
        {
            Facebook_ShowDate_Exclude
        }
        public enum FB_Api
        {
            FB_Like,
            FB_UnLike,
            Likes
        }
        
        public enum MailerContentName
        {
            highlight,
            banner,
            mainlisting,
            secondarylisting,
            specialinterest,
            custom1,
            custom2
        }
        
        public enum EmailTemplateContext
        {
            none,
            highlight,
            showlist,
            trivia,
            briefshowlist
        }

        public enum EmailLetterSiteTemplate
        {
            ChangePasswordEmail_txt,
            ChangeUserName_txt,
            CustomerExchange_html,
            CustomerForgotPassword_html,
            CustomerForgotPassword_txt,
            CustomerRefund_html,
            CustomerEmailTemplate_html,
            MailerSignupNotification_txt,
            Message_txt,
            PasswordRecoveryEmail_txt,
            PasswordResetEmail_txt,
            PurchaseConfirmationEmail_html,
            RegisterEmail_txt,
            ShowDateChange_html
        }

        public enum PublishEvent
        {
            Publish,
            EndPublish
        }

        public enum ViewingMode
        {
            List,
            Calendar
        }

        //keep in synch with StateManager _LookupTableNames && _Lookits....
        //MAKE PLURAL!!!!!
        public enum LookupTableNames  
        {
            Ages,             
            Employees,
            FaqCategories,
            FaqItems, Genres,
            HintQuestions,     
            ShowStatii, 
            SiteConfigs, 
            Subscriptions
        }
        
        public enum ConfigDataTypes
        {
            _string,
            _int,
            _decimal,
            _boolean
        }

        public enum SiteConfigContext
        {
            Images,
            FB_Integration,
            Flow,
            Default,
            Ship,
            PageMsg,
            Email,
            Admin,
            Service,
            Download,
            GlobalPrivate
        }

        public enum EventQStatus
        {
            Pending,
            Processing,
            Success,
            Failed,
            UserNotFound
        }

        public enum EventQContext
        {
            AdminNotification,
            Invoice,
            ShowDate,
            ShowTicket,
            User,
            Report,
            Mailer,
            Merch,
            SalePromotion
        }

        public enum EventQVerb
        {
            _Create,
            _Read,
            _Update,
            _Delete,
            _ActAdded,
            _PromotionAdded,
            _TicketAdded,
            AccountUpdate,
            AuthDecline,
            SentCorrespondence,
            ResendConfirmationEmail,
            CartCleared,
            ChangePickupName,
            ChangeShipDate,
            ChangeShipMethod,
            ChangeShipNotes,
            ChangeNotes,
            ChangeShipAddress,
            ChangeUserName,
            IncorrectPasswordHintSubmitted,
            InvalidCoupon,
            InventoryError,
            InventoryNotification,
            InventoryTransferred,
            PasswordReset,
            PromotionNotSelected,
            Publish,
            Refund,
            Role_Delete,
            Role_Add,
            Report_Mailer_Daily,
            TrackingChange,
            RequestPassword,
            StoreCreditAdjustment,
            SubscriptionUpdate,//
            StartingDownload,
            SuccessfulDownload,
            UserCreated,
            UserSentContactMessage,//
            UserSentRegistrationConfirm,//
            UserPurchase,
            UserLogin,
            UserPointsActivity,
            UserUpdate,
            Exchange,
            LotteryStatusUpdate,
            Mailer_Remove,//
            Mailer_SignupAwaitConfirm,//
            Mailer_FailureNotification,//
            Merch_SalePriceChange            
        }

        public enum SiteEntityMode
        {
            Act,
            Venue,
            Promoter
        }
        
        public enum SessionRequirements
        {
            None,
            Session,
            SSLSession
        }

        public enum AuthenticationTypes
        {
            None,
            WebUser,
            WebAdmin
        }

        public enum EmailFormats
        {
            html,
            text
        }
        public enum GenderTypes
        {
            male,
            female,
            noneSpecified
        }

        public enum HtmlFormatMode
        {
            none,
            simple,
            all
        }

        public enum FaqCategories
        {
            General,
            Tickets,
            Merch,
            Shipping
        }
        public enum PerformedByTypes
        {
            AdminSite,
            CustomerSite
        }

        public enum ShowDateStatus
        {
            Pending = 0,
            OnSale,
            SoldOut,
            Cancelled,
            Postponed,
            Moved,
            NotActive
        }
    }
}
