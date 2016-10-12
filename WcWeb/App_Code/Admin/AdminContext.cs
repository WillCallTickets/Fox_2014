using System;
using System.Collections.Generic;
using System.Web.Caching;

using Wcss;
using SubSonic;
using wctMain.Controller;
using System.Linq;

namespace wctMain.Admin
{	
	/// <summary>
	/// Summary description for SessionContext.
	/// </summary>
	public class AdminContext : MainContext
    { 
        public AdminContext() : base() 
        { 
            
        }

        public void PublishSite()
        {
            base.Publish();
        }

        public bool IsSuperSession(System.Security.Principal.IPrincipal user)
        {
            //only set itonce per session
            if (Session["Admin_SuperSession"] == null && user != null)
            {
                //if in role - set to username - otherwise empty
                Session["Admin_SuperSession"] = (user.IsInRole("Super")) ? user.Identity.Name : string.Empty;
            }
            
            return Session["Admin_SuperSession"].ToString() == user.Identity.Name;
        }

        #region CkEditor Helpers

        public string GetModalContext(bool useReferrer)
        {
            string refer = (useReferrer) ? this.Request.UrlReferrer.PathAndQuery : this.Request.Path;
            string query = (useReferrer) ? this.Request.UrlReferrer.Query : this.Request.QueryString.ToString();

            return (refer.IndexOf("ShowEditor.aspx") != -1 && query.IndexOf("p=acts") == -1) ? "Show" :
                //(refer.IndexOf("ShowEditor.aspx") != -1 && query.IndexOf("p=acts") != -1) ? "ShowBilling" :
                (refer.IndexOf("EntityEditor.aspx") != -1 && query.IndexOf("p=faq") != -1) ? "FaqItem" :
                (refer.IndexOf("Mailers.aspx") != -1 && query.IndexOf("p=mlredit") != -1) ? "MailerContent" : string.Empty;
        }

        public string GetWysEditorTitle(bool useReferrer)
        {
            string context = GetModalContext(useReferrer);
            return
                (context == "Show") ? ((this.CurrentShowRecord != null) ? this.CurrentShowRecord.Name : string.Empty) :
                //(context == "ShowBilling") ? ((this.CurrentShowRecord != null) ? string.Format("Billing - {0}", this.CurrentShowRecord.Name) : string.Empty) :
                (context == "MailerContent") ? ((this.CurrentMailerContent != null) ? this.CurrentMailerContent.Title : string.Empty) :
                (context == "FaqItem") ? ((this.CurrentFaqItem != null) ? this.CurrentFaqItem.Question : string.Empty) :
                string.Empty;
        }

        #endregion
        
        ///// <summary>
        ///// TODO!!!
        ///// </summary>
        //public void SaveCurrentShowBeforContinue()
        //{
        //    if (this.CurrentShowRecord != null && this.CurrentShowRecord.IsDirty)
        //    {
        //        //do something here - popup
        //        //get confirmation for save or warn?
        //        //do we need to catch this event earlier?

        //        //if we say don't continue - reinstate details page? do we know last page(context)?
        //    }
        //}

        #region Principal

        public _Enums.Principal CurrentEditPrincipal
        {
            get
            {
                string s = (this.Scookie._currentEditPrincipal != null && this.Scookie._currentEditPrincipal.Trim().Length > 0 
                    && this.Scookie._currentEditPrincipal != _Enums.Principal.all.ToString()) ?
                    this.Scookie._currentEditPrincipal : _Enums.Principal.all.ToString();

                return (_Enums.Principal)Enum.Parse(typeof(_Enums.Principal), s, true);
            }
            set
            {
                this.Scookie._currentEditPrincipal = value.ToString();
            }
        }

        #region Cookie Contexts 

        #region CollectionCriteria

        //public void UpdateOrderableCookies()
        //{
        //    if (this.CurrentEditPrincipal == _Enums.Principal.all)
        //    {
        //        CollectionCriteria_Banner.Status = _Enums.CollectionSearchCriteriaStatusType.all;
        //        CollectionCriteria_Kiosk.Status = _Enums.CollectionSearchCriteriaStatusType.all;
        //        CollectionCriteria_Post.Status = _Enums.CollectionSearchCriteriaStatusType.all;
        //        CollectionCriteria_Employee.Status = _Enums.CollectionSearchCriteriaStatusType.all;
        //        CollectionCriteria_Faq.Status = _Enums.CollectionSearchCriteriaStatusType.all;
        //        CollectionCriteria_Show.Status = _Enums.CollectionSearchCriteriaStatusType.all;
        //        CollectionCriteria_Act.Status = _Enums.CollectionSearchCriteriaStatusType.all;
        //        CollectionCriteria_Promoter.Status = _Enums.CollectionSearchCriteriaStatusType.all;
        //        CollectionCriteria_Venue.Status = _Enums.CollectionSearchCriteriaStatusType.all;
        //    }
        //}

        public _PrincipalBase.Helpers.CollectionSearchCriteria CollectionCriteria_Banner
        {
            get 
            { 
                return this.Scookie._collectionCriteria_Banner; 
            }
            set
            {
                this.Scookie._collectionCriteria_Banner = value;
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria CollectionCriteria_Kiosk
        {
            get
            {
                return this.Scookie._collectionCriteria_Kiosk;
            }
            set
            {
                this.Scookie._collectionCriteria_Kiosk = value;
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria CollectionCriteria_Post
        {
            get
            {
                return this.Scookie._collectionCriteria_Post;
            }
            set
            {
                this.Scookie._collectionCriteria_Post = value;
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria CollectionCriteria_Employee
        {
            get
            {
                return this.Scookie._collectionCriteria_Employee;
            }
            set
            {
                this.Scookie._collectionCriteria_Employee = value;
            }
        }
        
        public _PrincipalBase.Helpers.CollectionSearchCriteria CollectionCriteria_Faq
        {
            get
            {
                return this.Scookie._collectionCriteria_Faq;
            }
            set
            {
                this.Scookie._collectionCriteria_Faq = value;
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria CollectionCriteria_Show
        {
            get
            {
                return this.Scookie._collectionCriteria_Show;
            }
            set
            {
                this.Scookie._collectionCriteria_Show = value;
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria CollectionCriteria_Act
        {
            get
            {
                return this.Scookie._collectionCriteria_Act;
            }
            set
            {
                this.Scookie._collectionCriteria_Act = value;
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria CollectionCriteria_Promoter
        {
            get
            {
                return this.Scookie._collectionCriteria_Promoter;
            }
            set
            {
                this.Scookie._collectionCriteria_Promoter = value;
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria CollectionCriteria_Venue
        {
            get
            {
                return this.Scookie._collectionCriteria_Venue;
            }
            set
            {
                this.Scookie._collectionCriteria_Venue = value;
            }
        }

        #endregion

        #region Collection Nav Tabs

        public string ActiveEditBannerContextTab { get { return this.Scookie._activeEditBannerContextTab; } set { this.Scookie._activeEditBannerContextTab = value; } }
        public string ActiveEditEmployeeContextTab { get { return this.Scookie._activeEditEmployeeContextTab; } set { this.Scookie._activeEditEmployeeContextTab = value; } }
        public string ActiveEditFaqContextTab { get { return this.Scookie._activeEditFaqContextTab; } set { this.Scookie._activeEditFaqContextTab = value; } }
        public string ActiveEditKioskContextTab { get { return this.Scookie._activeEditKioskContextTab; } set { this.Scookie._activeEditKioskContextTab = value; } }
        public string ActiveEditPostContextTab { get { return this.Scookie._activeEditPostContextTab; } set { this.Scookie._activeEditPostContextTab = value; } }
        public string ActiveEditActContextTab { get { return this.Scookie._activeEditActContextTab; } set { this.Scookie._activeEditActContextTab = value; } }
        public string ActiveEditPromoterContextTab { get { return this.Scookie._activeEditPromoterContextTab; } set { this.Scookie._activeEditPromoterContextTab = value; } }
        public string ActiveEditVenueContextTab { get { return this.Scookie._activeEditVenueContextTab; } set { this.Scookie._activeEditVenueContextTab = value; } }

        #endregion

        #region Collection Container Tabs

        public string ActiveEditBannerContainerTab { get { return this.Scookie._activeEditBannerContainerTab; } set { this.Scookie._activeEditBannerContainerTab = value; } }
        public string ActiveEditEmployeeContainerTab { get { return this.Scookie._activeEditEmployeeContainerTab; } set { this.Scookie._activeEditEmployeeContainerTab = value; } }
        public string ActiveEditFaqContainerTab { get { return this.Scookie._activeEditFaqContainerTab; } set { this.Scookie._activeEditFaqContainerTab = value; } }
        public string ActiveEditKioskContainerTab { get { return this.Scookie._activeEditKioskContainerTab; } set { this.Scookie._activeEditKioskContainerTab = value; } }
        public string ActiveEditPostContainerTab { get { return this.Scookie._activeEditPostContainerTab; } set { this.Scookie._activeEditPostContainerTab = value; } }
        public string ActiveEditShowContainerTab { get { return this.Scookie._activeEditShowContainerTab; } set { this.Scookie._activeEditShowContainerTab = value; } }
        public string ActiveEditActContainerTab { get { return this.Scookie._activeEditActContainerTab; } set { this.Scookie._activeEditActContainerTab = value; } }
        public string ActiveEditPromoterContainerTab { get { return this.Scookie._activeEditPromoterContainerTab; } set { this.Scookie._activeEditPromoterContainerTab = value; } }
        public string ActiveEditVenueContainerTab { get { return this.Scookie._activeEditVenueContainerTab; } set { this.Scookie._activeEditVenueContainerTab = value; } }

        #endregion

        #endregion

        /// <summary>
        /// The principal context we are using for selection/viewing
        /// </summary>
        

        #region _Principal Collections
                

        #region CollectionCurrentEntities

        public SalePromotion CurrentBannerRecord
        {
            get
            {
                return (SalePromotion)Session["CurrentBannerRecord"];
            }
            set
            {
                if (Session["CurrentBannerRecord"] == null)
                    Session.Add("CurrentBannerRecord", value);
                else
                    Session["CurrentBannerRecord"] = value;
            }
        }

        public Kiosk CurrentKioskRecord
        {
            get
            {
                return (Kiosk)Session["CurrentKioskRecord"];
            }
            set
            {
                if (Session["CurrentKioskRecord"] == null)
                    Session.Add("CurrentKioskRecord", value);
                else
                    Session["CurrentKioskRecord"] = value;
            }
        }

        public Post CurrentPostRecord
        {
            get
            {
                return (Post)Session["CurrentPostRecord"];
            }
            set
            {
                if (Session["CurrentPostRecord"] == null)
                    Session.Add("CurrentPostRecord", value);
                else
                    Session["CurrentPostRecord"] = value;
            }
        }

        public Employee CurrentEmployeeRecord
        {
            get
            {
                return (Employee)Session["CurrentEmployeeRecord"];
            }
            set
            {
                if (Session["CurrentEmployeeRecord"] == null)
                    Session.Add("CurrentEmployeeRecord", value);
                else
                    Session["CurrentEmployeeRecord"] = value;
            }
        }

        /// <summary>
        /// This is the more recent object and should be used for principal collections
        /// </summary>
        public FaqItem CurrentFaqRecord
        {
            get
            {
                return (FaqItem)Session["CurrentFaqRecord"];
            }
            set
            {
                if (Session["CurrentFaqRecord"] == null)
                    Session.Add("CurrentFaqRecord", value);
                else
                    Session["CurrentFaqRecord"] = value;
            }
        }

        public string CurrentFaqCategorySelection
        {
            get
            {
                if (Session["CurrentFaqCategorySelection"] == null)
                    Session["CurrentFaqCategorySelection"] = _Lookits.FaqCategories[0].Name;

                return Session["CurrentFaqCategorySelection"].ToString();
            }
            set
            {
                if (Session["CurrentFaqCategorySelection"] == null)
                    Session.Add("CurrentFaqCategorySelection", value);
                else
                    Session["CurrentFaqCategorySelection"] = value;
            }
        }


        //put here for completeness - implementation is below
        //public Show CurrentShowRecord
        //{
        //    get
        //    {
        //        return (Show)Session["CurrentShowRecord"];
        //    }
        //    set
        //    {
        //        if (Session["CurrentShowRecord"] == null)
        //            Session.Add("CurrentShowRecord", value);
        //        else
        //            Session["CurrentShowRecord"] = value;
        //    }
        //}

        public Act CurrentActRecord
        {
            get
            {
                return (Act)Session["CurrentActRecord"];
            }
            set
            {
                if (Session["CurrentActRecord"] == null)
                    Session.Add("CurrentActRecord", value);
                else
                    Session["CurrentActRecord"] = value;

                CurrentActId = (value != null) ? value.Id : 0;
            }
        }

        public Promoter CurrentPromoterRecord
        {
            get
            {
                return (Promoter)Session["CurrentPromoterRecord"];
            }
            set
            {
                if (Session["CurrentPromoterRecord"] == null)
                    Session.Add("CurrentPromoterRecord", value);
                else
                    Session["CurrentPromoterRecord"] = value;

                CurrentPromoterId = (value != null) ? value.Id : 0;
            }
        }

        public Venue CurrentVenueRecord
        {
            get
            {
                return (Venue)Session["CurrentVenueRecord"];
            }
            set
            {
                if (Session["CurrentVenueRecord"] == null)
                    Session.Add("CurrentVenueRecord", value);
                else
                    Session["CurrentVenueRecord"] = value;

                CurrentVenueId = (value != null) ? value.Id : 0;
            }
        }

        #region Show Records

        /// <summary>
        ///  Holds the starting date from which to list shows - defaults to today - 3days
        /// </summary>
        public DateTime CurrentShowListStartDate
        {
            get
            {
                if (Session["CurrentShowListStartDate"] == null)
                    return DateTime.Now.AddDays(-5);//allow for long weekends

                return (DateTime)Session["CurrentShowListStartDate"];
            }
            set
            {
                if (Session["CurrentShowListStartDate"] == null)
                    Session.Add("CurrentShowListStartDate", value);
                else
                    Session["CurrentShowListStartDate"] = value;
            }
        }

        public Show CurrentShowRecord { get { return (Show)Session["Admin_CurrentShow"]; } }

        /// <summary>
        /// This will update the session showId and will also fire an event indicating that the show record has changed
        /// </summary>
        public Show SetCurrentShowRecord(int idx)
        {
            idx = SetCurrentShowId(idx);

            //this is not redundant - enforces a refresh
            if (Session["Admin_CurrentShow"] != null)
                Session.Remove("Admin_CurrentShow");

            //reset linked objects
            this.CurrentShowLinkId = 0;

            if (idx == 0)
                return null;

            Show s = new Show(idx);

            if (s != null && s.ApplicationId == _Config.APPLICATION_ID)
                Session.Add("Admin_CurrentShow", s);

            return (Show)Session["Admin_CurrentShow"];
        }

        /// <summary>
        /// This will update the session showId only - DOES NOT fire an event to indicate the show record has changed
        /// </summary>
        private int SetCurrentShowId(int idx)
        {
            if (idx == 0)// && Session["Admin_CurrentShowId"] != null)
            {
                Session.Remove("Admin_CurrentShowId");
                return idx;
            }
            //..if the idx has, in fact, changed
            else if (Session["Admin_CurrentShowId"] != null && (int)Session["Admin_CurrentShowId"] != idx)
            {
                Session.Remove("Admin_CurrentShowId");
                Session.Add("Admin_CurrentShowId", idx);
            }
            else if (Session["Admin_CurrentShowId"] == null && idx > 0)
                Session.Add("Admin_CurrentShowId", idx);

            return (int)Session["Admin_CurrentShowId"];

        }

        public void ResetCurrentShowRecord()
        {
            if (CurrentShowRecord != null)
            {
                SetCurrentShowRecord(CurrentShowRecord.Id);
            }
        }

        #endregion

        #endregion

        #endregion - end collections

        #endregion - end principal

        #region VD Data

        public string ActiveDataEntryTab { get { return this.Scookie._activeDataEntryTab; } set { this.Scookie._activeDataEntryTab = value; } }

        public Show VD_CurrentDataEntryShowRecord
        {
            get
            {
                return (Show)Session["VD_CurrentDataEntryShowRecord"];
            }
            set
            {
                if (Session["VD_CurrentDataEntryShowRecord"] == null)
                    Session.Add("VD_CurrentDataEntryShowRecord", value);
                else
                    Session["VD_CurrentDataEntryShowRecord"] = value;
            }
        }

        public int VDQueryActId { get { return this.Scookie._VDQueryActId; } set { this.Scookie._VDQueryActId = value; } }

        public Act _vd_CurrentQueryActRecord = null;
        public Act VD_CurrentQueryActRecord 
        {
            get 
            {
                if(VDQueryActId == 0)
                {
                    Session.Remove("VD_CurrentQueryActRecord");
                }
                else
                {
                    Act ent = (Act)Session["VD_CurrentQueryActRecord"];

                    if(ent == null)
                        Session.Add("VD_CurrentQueryActRecord", new Act(VDQueryActId));
                    else if(ent != null && ent.Id != VDQueryActId)
                        Session["VD_CurrentQueryActRecord"] = new Act(VDQueryActId);
                }

                return (Act)Session["VD_CurrentQueryActRecord"];
            }
        }

        public void VD_CurrentQueryActRecord_Reset()
        {
            VDQueryActId = 0;
        }

        #endregion

        #region Admin Cookies, Controls and User preferences
        
        //default page size for gridviews, etc
        public int AdminPageSize
        {
            get
            {
                return (System.Web.HttpContext.Current.Profile as ProfileCommon).Preferences.PageSize;
            }
            set
            {
                (System.Web.HttpContext.Current.Profile as ProfileCommon).Preferences.PageSize = value;
            }
        }

        public string CurrentProcessError
        {
            get
            {
                if (Session["Admin_CurrentProcessError"] != null)
                    return (string)Session["Admin_CurrentProcessError"];

                return null;
            }
            set
            {
                Session.Remove("Admin_CurrentProcessError");

                if (value != null)
                    Session.Add("Admin_CurrentProcessError", value);
            }
        }

        /// <summary>
        /// Holds a processed list of emails. It should be disposed of immediately after loading 
        /// </summary>
        public List<string> CurrentAddressList
        {
            get
            {
                if (Session["Admin_CurrentAddressList"] != null)
                    return (List<string>)Session["Admin_CurrentAddressList"];

                return null;
            }
            set
            {
                Session.Remove("Admin_CurrentAddressList");

                if (value != null)
                    Session.Add("Admin_CurrentAddressList", value);
            }
        }

        public List<string> CurrentDisplayList
        {
            get
            {
                if (Session["Admin_CurrentDisplayList"] != null)
                    return (List<string>)Session["Admin_CurrentDisplayList"];

                return null;
            }
            set
            {
                Session.Remove("Admin_CurrentDisplayList");

                if (value != null)
                    Session.Add("Admin_CurrentDisplayList", value);
            }
        }

        public DateTime ShowChooserStartDate
        {
            get
            {
                if (Session["Admin_ChooserStartDate"] == null)
                    Session["Admin_ChooserStartDate"] = DateTime.Parse(DateTime.Now.ToString("MM/1/yyyy"));

                return (DateTime)Session["Admin_ChooserStartDate"];
            }
            set
            {
                Session["Admin_ChooserStartDate"] = value;
            }
        }

        public string CampaignEditAddEditShowsContextTab { get { return this.Scookie._campaignContentAddShowContextTab; } set { this.Scookie._campaignContentAddShowContextTab = value; } }
        public string CampaignEditContextTab { get { return this.Scookie._campaignEditContextTab; } set { this.Scookie._campaignEditContextTab = value; } }
        public string ActiveMailerSendTab { get { return this.Scookie._activeMailerSendTab; } set { this.Scookie._activeMailerSendTab = value; } }

        public string ActiveMailerReviewTab { get { return this.Scookie._activeMailerReviewTab; } set { this.Scookie._activeMailerReviewTab = value; } }
        
        public int vwDwnId { get { return this.Scookie._vwDwn; } set { Scookie._vwDwn = value; } }
        public int vwDateId { get { return this.Scookie._vwDte; } set { Scookie._vwDte = value; } }

        public int CurrentActId { get { return this.Scookie._acid; } set { Scookie._acid = value; } }
        public int CurrentVDActId { get { return this.Scookie._vdacid; } set { Scookie._vdacid = value; } }
        public int CurrentPromoterId { get { return this.Scookie._prid; } set { Scookie._prid = value; } }
        public int CurrentShowLinkId { get { return this.Scookie._slid; } set { Scookie._slid = value; } }
        public int CurrentVenueId { get { return this.Scookie._vnid; } set { Scookie._vnid = value; } }

        #endregion

        #region Mailer

        public SubscriptionEmail CurrentSubscriptionEmail
        {
            get
            {
                return (SubscriptionEmail)Session["Admin_CurrentSubscriptionEmail"];
            }
            set
            {
                Session.Remove("Admin_CurrentSubscriptionEmail");

                if (value != null)
                    Session.Add("Admin_CurrentSubscriptionEmail", value);
            }
        }
        public EmailLetter CurrentEmailLetter
        {
            get
            {
                return (EmailLetter)Session["Admin_CurrentEmailLetter"];
            }
            set
            {
                Session.Remove("Admin_CurrentEmailLetter");

                if (value != null)
                    Session.Add("Admin_CurrentEmailLetter", value);
            }
        }
        
        public Mailer CurrentMailer
        {
            get
            {
                return (Mailer)Session["Admin_CurrentMailer"];
            }
            set
            {
                Session.Remove("Admin_CurrentMailer");

                if (value != null)
                    Session.Add("Admin_CurrentMailer", value);
            }
        }

        public MailerContent CurrentMailerContent
        {
            get
            {
                return (MailerContent)Session["Admin_CurrentMailerContent"];
            }
            set
            {
                Session.Remove("Admin_CurrentMailerContent");

                if (value != null)
                    Session.Add("Admin_CurrentMailerContent", value);
            }
        }
        
        public MailerTemplate CurrentMailerTemplate
        {
            get
            {
                return (MailerTemplate)Session["Admin_CurrentMailerTemplate"];
            }
            set
            {
                Session.Remove("Admin_CurrentMailerTemplate");

                if (value != null)
                    Session.Add("Admin_CurrentMailerTemplate", value);
            }
        }

        public MailerTemplateCreation CurrentMailerTemplateCreation
        {
            get
            {
                return (Admin.MailerTemplateCreation)Session["Admin_CurrentMailerTemplateCreation"];
            }
            set
            {
                Session.Remove("Admin_CurrentMailerTemplateCreation");

                if (value != null)
                    Session.Add("Admin_CurrentMailerTemplateCreation", value);
            }
        }

        public FaqItem CurrentFaqItem
        {
            get
            {
                return (FaqItem)Session["Admin_CurrentFaqItem"];
            }
            set
            {
                Session.Remove("Admin_CurrentFaqItem");

                if (value != null)
                    Session.Add("Admin_CurrentFaqItem", value);
            }
        }

        public void RefreshMailer()
        {
            int idx = (this.CurrentMailer != null) ? this.CurrentMailer.Id : 0;
            int templateId = (this.CurrentMailer != null) ? this.CurrentMailer.TMailerTemplateId : (this.CurrentMailerTemplate != null) ? this.CurrentMailerTemplate.Id : 0;

            this.CurrentMailer = null;
            this.CurrentMailerTemplate = null;

            if (idx > 0)
            {
                this.CurrentMailer = Mailer.FetchByID(idx);
                if (this.CurrentMailer != null)
                    this.CurrentMailerTemplate = MailerTemplate.FetchByID(this.CurrentMailer.TMailerTemplateId);
            }
            else if (templateId > 0)
                this.CurrentMailerTemplate = MailerTemplate.FetchByID(templateId);
        }

        #endregion

        #region Promoters

        public PromoterCollection Promoters
        {
            get
            {
                if (Cache["Admin_Promoters"] == null)
                {
                    PromoterCollection coll = new PromoterCollection();
                    coll.Where(Promoter.Columns.ApplicationId, _Config.APPLICATION_ID).OrderByAsc("NameRoot").Load();

                    Cache.Insert("Admin_Promoters", coll, null, 
                        System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
                }

                return (PromoterCollection)Cache["Admin_Promoters"];
            }
        }

        public void Clear_Promoters() { Cache.Remove("Admin_Promoters"); }

        #endregion

        #region Venues

        public VenueCollection Venues
        {
            get
            {
                if (Cache["Admin_Venues"] == null)
                {
                    VenueCollection coll = new VenueCollection();
                    coll.Where(Venue.Columns.ApplicationId, _Config.APPLICATION_ID).OrderByAsc("Name").Load();

                    Cache.Insert("Admin_Venues", coll, null, 
                        System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
                }

                return (VenueCollection)Cache["Admin_Venues"];
            }
        }

        public void Clear_Venues() { Cache.Remove("Admin_Venues"); }
        
        #endregion

        #region BLOCKING

        //*** note bracket issue - we cannot concatenate with format - must use + - due to brackets

        private static List<string> _blockUIProcessingMessages = null;
        private static List<string> BlockUI_ProcessingMessages
        {
            get
            {
                if (_blockUIProcessingMessages == null)
                {
                    _blockUIProcessingMessages = new List<string>();
                    _blockUIProcessingMessages.Add("...Processing Request...");
                    _blockUIProcessingMessages.Add("...Granting Request...");
                    _blockUIProcessingMessages.Add("...Brewing...");
                    _blockUIProcessingMessages.Add("...Eating The Donuts...");
                    _blockUIProcessingMessages.Add("...Making The Donuts...");
                    _blockUIProcessingMessages.Add("...Hard Hat Area...");
                    _blockUIProcessingMessages.Add("...Exhausting Resources...");
                    _blockUIProcessingMessages.Add("...Clearing Cobwebs...");
                    _blockUIProcessingMessages.Add("...Making My Way Back To You Babe...");
                    _blockUIProcessingMessages.Add("...Taking Time Out...");
                    _blockUIProcessingMessages.Add("...Shaking That Money Maker...");
                    _blockUIProcessingMessages.Add("...Please Wait...");
                    _blockUIProcessingMessages.Add("...Calculating PI...");
                    _blockUIProcessingMessages.Add("...Stopping Global Warming...");
                    _blockUIProcessingMessages.Add("...Pasteurizing...");
                    _blockUIProcessingMessages.Add("...Distilling...");
                    _blockUIProcessingMessages.Add("...Virtualizing...");
                    _blockUIProcessingMessages.Add("...Minding P's and Q's...");
                    _blockUIProcessingMessages.Add("...Distilling...");
                    _blockUIProcessingMessages.Add("...Putting A Bead On...");
                    _blockUIProcessingMessages.Add("...Creating Matrices...");
                    _blockUIProcessingMessages.Add("...Opening Vortex...");
                    _blockUIProcessingMessages.Add("...Placing A Hold On...");
                    _blockUIProcessingMessages.Add("...Engaging Warp Engines...");
                    _blockUIProcessingMessages.Add("...Grooving With A Pict...");
                    _blockUIProcessingMessages.Add("...Eating more pork...");
                    _blockUIProcessingMessages.Add("...Shaking off the heebie jeebies...");
                    _blockUIProcessingMessages.Add("...Looking for Mulva...");
                    _blockUIProcessingMessages.Add("...Biting off more than I can chew...");
                    _blockUIProcessingMessages.Add("...Four score and seven...");
                    _blockUIProcessingMessages.Add("...The humanity...");
                    _blockUIProcessingMessages.Add("...Gunter glieben glauchen globen...");
                    _blockUIProcessingMessages.Add("...For those about to rock....We Salute You...");
                    _blockUIProcessingMessages.Add("...I, Claudius...");
                    _blockUIProcessingMessages.Add("...It's a small world, but I wouldn't want to paint it...");
                    _blockUIProcessingMessages.Add("...I've written several children's books ... Not on purpose...");





                    //_blockUIProcessingMessages.Add("...Al...Al, Al, Al...Al...");
                    //_blockUIProcessingMessages.Add("...NICE CHEST!...");
                }

                return _blockUIProcessingMessages;
            }
        }

        public static string GetRandomProcessingMessage
        {
            get
            {
                return System.Web.HttpUtility.HtmlEncode(
                    BlockUI_ProcessingMessages[Utils.ParseHelper.GenerateRandomNumber(0, BlockUI_ProcessingMessages.Count - 1)]).Replace("'","&#39;");//make it apply to zero index
            }
        }

        #endregion
	}
}
