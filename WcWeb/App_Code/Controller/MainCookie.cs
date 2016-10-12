using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Wcss;

namespace wctMain.Controller
{
	/// <summary>
	/// Summary description for SessionContext.
	/// </summary>
	public class MainCookie : Utils.SessionCookieManager
	{
		public MainCookie() : base () { }

        #region Cookies that have keys registered in _Enums for use in js, etc

        #region Collection Criteria cookies - SearchCollection - Key = accc

        /// <summary>
        /// active (0=all, 1=active, 2=notactive, 3=running), startdate, enddate, searchterms
        /// </summary>
        public _PrincipalBase.Helpers.CollectionSearchCriteria _collectionCriteria_Banner
        {
            get
            {   
                string cooKey = _Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Banner);
                string val = this.getCookie(cooKey);
                _PrincipalBase.Helpers.CollectionSearchCriteria criteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(val);

                if (val == null || val.Trim().Length == 0)
                    setCookie(cooKey, criteria.Serialized());

                return criteria;
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Banner), 
                    value.Serialized());
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria _collectionCriteria_Kiosk
        {
            get
            {
                string cooKey = _Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Kiosk);
                string val = this.getCookie(cooKey);
                _PrincipalBase.Helpers.CollectionSearchCriteria criteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(val);

                if (val == null || val.Trim().Length == 0)
                    setCookie(cooKey, criteria.Serialized());

                return criteria;
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Kiosk),
                    value.Serialized());
            }
        }
        
        public _PrincipalBase.Helpers.CollectionSearchCriteria _collectionCriteria_Post
        {
            get
            {
                string cooKey = _Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Post);
                string val = this.getCookie(cooKey);
                _PrincipalBase.Helpers.CollectionSearchCriteria criteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(val);

                if (val == null || val.Trim().Length == 0)
                    setCookie(cooKey, criteria.Serialized());

                return criteria;
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Post),
                    value.Serialized());
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria _collectionCriteria_Employee
        {
            get
            {
                string cooKey = _Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Employee);
                string val = this.getCookie(cooKey);
                _PrincipalBase.Helpers.CollectionSearchCriteria criteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(val);

                if (val == null || val.Trim().Length == 0)
                    setCookie(cooKey, criteria.Serialized());

                return criteria;
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Employee),
                    value.Serialized());
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria _collectionCriteria_Faq
        {
            get
            {
                string cooKey = _Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Faq);
                string val = this.getCookie(cooKey);
                _PrincipalBase.Helpers.CollectionSearchCriteria criteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(val);

                if (val == null || val.Trim().Length == 0)
                    setCookie(cooKey, criteria.Serialized());

                return criteria;
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Faq),
                    value.Serialized());
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria _collectionCriteria_Show
        {
            get
            {
                string cooKey = _Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Show);
                string val = this.getCookie(cooKey);
                _PrincipalBase.Helpers.CollectionSearchCriteria criteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(val);

                if (val == null || val.Trim().Length == 0)
                    setCookie(cooKey, criteria.Serialized());

                return criteria;
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Show),
                    value.Serialized());
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria _collectionCriteria_Act
        {
            get
            {
                string cooKey = _Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Act);
                string val = this.getCookie(cooKey);
                _PrincipalBase.Helpers.CollectionSearchCriteria criteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(val);

                if (val == null || val.Trim().Length == 0)
                    setCookie(cooKey, criteria.Serialized());

                return criteria;
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Act),
                    value.Serialized());
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria _collectionCriteria_Promoter
        {
            get
            {
                string cooKey = _Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Promoter);
                string val = this.getCookie(cooKey);
                _PrincipalBase.Helpers.CollectionSearchCriteria criteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(val);

                if (val == null || val.Trim().Length == 0)
                    setCookie(cooKey, criteria.Serialized());

                return criteria;
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Promoter),
                    value.Serialized());
            }
        }

        public _PrincipalBase.Helpers.CollectionSearchCriteria _collectionCriteria_Venue
        {
            get
            {
                string cooKey = _Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Venue);
                string val = this.getCookie(cooKey);
                _PrincipalBase.Helpers.CollectionSearchCriteria criteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(val);

                if (val == null || val.Trim().Length == 0)
                    setCookie(cooKey, criteria.Serialized());

                return criteria;
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminCollectionCriteriaCookie.CollectionCriteria_Venue),
                    value.Serialized());
            }
        }

        #endregion

        #region Context Tab Cookies - Key = rake

        public string _activeEditBannerContextTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditBannerContextTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditBannerContextTab), value);
            }
        }

        public string _activeEditKioskContextTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditKioskContextTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditKioskContextTab), value);
            }
        }

        public string _activeEditPostContextTab//raptsd
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditPostContextTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditPostContextTab), value);
            }
        }

        public string _activeEditEmployeeContextTab//rakeer
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditEmployeeContextTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditEmployeeContextTab), value);
            }
        }

        public string _activeEditFaqContextTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditFaqContextTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditFaqContextTab), value);
            }
        }

        //public string _activeEditShowContextTab
        //{
        //    get
        //    {
        //        return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditShowContextTab));
        //    }
        //    set
        //    {
        //        base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditShowContextTab), value);
        //    }
        //}

        public string _activeEditActContextTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditActContextTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditActContextTab), value);
            }
        }

        public string _activeEditPromoterContextTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditPromoterContextTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditPromoterContextTab), value);
            }
        }

        public string _activeEditVenueContextTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditVenueContextTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContextCookie.ActiveEditVenueContextTab), value);
            }
        }

        #endregion

        #region Edit Container Tabs - Key = ract

        public string _activeEditBannerContainerTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditBannerContainerTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditBannerContainerTab), value);
            }
        }

        public string _activeEditKioskContainerTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditKioskContainerTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditKioskContainerTab), value);
            }
        }

        public string _activeEditPostContainerTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditPostContainerTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditPostContainerTab), value);
            }
        }

        public string _activeEditEmployeeContainerTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditEmployeeContainerTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditEmployeeContainerTab), value);
            }
        }

        public string _activeEditFaqContainerTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditFaqContainerTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditFaqContainerTab), value);
            }
        }

        public string _activeEditShowContainerTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditShowContainerTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditShowContainerTab), value);
            }
        }

        public string _activeEditActContainerTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditActContainerTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditActContainerTab), value);
            }
        }

        public string _activeEditPromoterContainerTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditPromoterContainerTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditPromoterContainerTab), value);
            }
        }

        public string _activeEditVenueContainerTab
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditVenueContainerTab));
            }
            set
            {
                base.setCookie(_Enums.GetDescription(_Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditVenueContainerTab), value);
            }
        }

        #endregion

        #endregion



        public string _currentEditPrincipal
        {
            get
            {
                string val = base.getCookie("aepr");
                if (val == null || val.Trim().Length == 0)
                {
                    base.setPersistentCookie("aepr", string.Empty);
                    return (string.Empty);
                }

                return val;
            }
            set
            {
                base.setPersistentCookie("aepr", value);
            }
        }


        #region VenueData Cookies


        public int _VDQueryActId
        {
            get
            {
                string val = base.getCookie("vdqaid");
                if (val == null || val.Trim().Length == 0)
                {
                    val = "0";
                    base.setPersistentCookie("vdqaid", val);
                }

                return int.Parse(val);
            }
            set
            {
                base.setPersistentCookie("vdqaid", value.ToString());
            }
        }

        public string _activeDataEntryTab
        {
            get
            {
                string val = base.getCookie("vdadet");
                if (val == null || val.Trim().Length == 0)
                {
                    base.setPersistentCookie("vdadet", string.Empty);
                    return (string.Empty);
                }

                return val;
            }
            set
            {
                base.setPersistentCookie("vdadet", value);
            }
        }

        //public int VD_CurrentShowEntryId
        //{
        //    get
        //    {
        //        string val = base.getCookie("vdcseid");
        //        if (val == null || val.Trim().Length == 0)
        //        {
        //            val = "0";
        //            base.setPersistentCookie("vdcseid", val);
        //        }

        //        return int.Parse(val);
        //    }
        //    set
        //    {
        //        base.setPersistentCookie("vdcseid", value.ToString());
        //    }
        //}

        //public int VD_CurrentShowViewId
        //{
        //    get
        //    {
        //        string val = base.getCookie("vdcsvid");
        //        if (val == null || val.Trim().Length == 0)
        //        {
        //            val = "0";
        //            base.setPersistentCookie("vdcsvid", val);
        //        }

        //        return int.Parse(val);
        //    }
        //    set
        //    {
        //        base.setPersistentCookie("vdcsvid", value.ToString());
        //    }
        //}

        //public int VD_CurrentVenueEntryId
        //{
        //    get
        //    {
        //        string val = base.getCookie("vdcveid");
        //        if (val == null || val.Trim().Length == 0)
        //        {
        //            val = "0";
        //            base.setPersistentCookie("vdcveid", val);
        //        }

        //        return int.Parse(val);
        //    }
        //    set
        //    {
        //        base.setPersistentCookie("vdcveid", value.ToString());
        //    }
        //}

        //public int VD_CurrentVenueViewId
        //{
        //    get
        //    {
        //        string val = base.getCookie("vdcvvid");
        //        if (val == null || val.Trim().Length == 0)
        //        {
        //            val = "0";
        //            base.setPersistentCookie("vdcvvid", val);
        //        }

        //        return int.Parse(val);
        //    }
        //    set
        //    {
        //        base.setPersistentCookie("vdcvvid", value.ToString());
        //    }
        //}

        //public DateTime VD_VenueSelectStartDate
        //{
        //    get
        //    {
        //        string val = base.getCookie("vdcsd");
        //        if (val == null || val.Trim().Length == 0)
        //        {
        //            val = DateTime.Now.Date.AddDays(-1).ToString("MM/dd/yyyy hh:mm tt");
        //            base.setPersistentCookie("vdcsd", val);
        //        }

        //        return DateTime.Parse(val);
        //    }
        //    set
        //    {
        //        base.setPersistentCookie("vdcsd", value.ToString("MM/dd/yyyy hh:mm tt"));
        //    }
        //}

        //public DateTime VD_VenueSelectEndDate
        //{
        //    get
        //    {
        //        string val = base.getCookie("vdced");
        //        if (val == null || val.Trim().Length == 0)
        //        {
        //            val = DateTime.Now.Date.AddDays(-1).ToString("MM/dd/yyyy hh:mm tt");
        //            base.setPersistentCookie("vdced", val);
        //        }

        //        return DateTime.Parse(val);
        //    }
        //    set
        //    {
        //        base.setPersistentCookie("vdced", value.ToString("MM/dd/yyyy hh:mm tt"));
        //    }
        //}

        #endregion

        #region General Cookies

        /// <summary>
        /// marketing program
        /// </summary>
		public string MarketingProgram
		{
			get
			{
				return base.getCookie("evt", "mkp");
			}
			set
			{
				base.setCookie("evt", "mkp", value.Trim());
			}
		}

        #endregion

        #region Admin Cookies

        public string _campaignContentAddShowContextTab
        {
            get
            {
                string val = base.getCookie("cmpcasct");
                if (val == null || val.Trim().Length == 0)
                {
                    base.setPersistentCookie("cmpcasct", string.Empty);
                    return (string.Empty);
                }

                return val;
            }
            set
            {
                base.setPersistentCookie("cmpcasct", value);
            }
        }

        public string _campaignEditContextTab
        {
            get
            {
                string val = base.getCookie("cmpedcx");
                if (val == null || val.Trim().Length == 0)
                {
                    base.setPersistentCookie("cmpedcx", string.Empty);
                    return (string.Empty);
                }

                return val;
            }
            set
            {
                base.setPersistentCookie("cmpedcx", value);
            }
        }

        public string _activeMailerSendTab
        {
            get
            {
                string val = base.getCookie("amtdd");
                if (val == null || val.Trim().Length == 0)
                {
                    base.setPersistentCookie("amtdd", string.Empty);
                    return (string.Empty);
                }

                return val;
            }
            set
            {
                base.setPersistentCookie("amtdd", value);
            }
        }

        public string _activeMailerReviewTab
        {
            get
            {
                string val = base.getCookie("amrrd");
                if (val == null || val.Trim().Length == 0)
                {
                    base.setPersistentCookie("amrrd", string.Empty);
                    return (string.Empty);
                }

                return val;
            }
            set
            {
                base.setPersistentCookie("amrrd", value);
            }
        }

        public int _vwDwn
        {
            get
            {
                string id = base.getCookie("vwdwn");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setCookie("vwdwn", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setCookie("vwdwn", value.ToString());
            }
        }

        public int _vwDte
        {
            get
            {
                string id = base.getCookie("vwdte");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setCookie("vwdte", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setCookie("vwdte", value.ToString());
            }
        }

        /// <summary>
        /// Subscription Email
        /// </summary>
        public int _seid
        {
            get
            {
                string id = base.getCookie("seid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setCookie("seid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setCookie("seid", value.ToString());
            }
        }

        /// <summary>
        /// Act Id
        /// </summary>
        public int _acid
        {
            get
            {
                string id = base.getCookie("acid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setCookie("acid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setCookie("acid", value.ToString());
            }
        }

        public int _vdacid
        {
            get
            {
                string id = base.getCookie("vdacid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setCookie("vdacid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setCookie("vdacid", value.ToString());
            }
        }

        /// <summary>
        /// Promoter id
        /// </summary>
        public int _prid
        {
            get
            {
                string id = base.getCookie("prid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setCookie("prid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setCookie("prid", value.ToString());
            }
        }

        /// <summary>
        /// ShowLink id
        /// </summary>
        public int _slid
        {
            get
            {
                string id = base.getCookie("slid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setCookie("slid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setCookie("slid", value.ToString());
            }
        }

        /// <summary>
        /// Venue id
        /// </summary>
        public int _vnid
        {
            get
            {
                string id = base.getCookie("vnid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setCookie("vnid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setCookie("vnid", value.ToString());
            }
        }

        #endregion
    }
}
