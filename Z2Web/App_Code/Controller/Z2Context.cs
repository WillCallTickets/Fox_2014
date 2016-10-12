using System;
using System.Data;
using System.Web.SessionState;
using System.Web.Caching;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Net;

using Wcss;
using SubSonic;

namespace z2Main.Controller
{
    /// <summary>
    /// Summary description for Z2Context
    /// </summary>
    public class Z2Context : _ContextBase
    {
        _Enums.Principal WebContextPrincipal = _Enums.Principal.z2;

        public Z2Cookie z2cookie;
        public HttpSessionState Session;
        
        public Z2Context(HttpSessionState session)
        {
            Session = session;
            Session.Timeout = 30;
            z2cookie = new Z2Cookie();

            DependencyFileChanged = new System.Web.Caching.CacheDependency(Z2Config.MAPPED_SHOW_DEPENDENCY_FILE);
            CacheMasterKeyRemoved = new System.Web.Caching.CacheItemRemovedCallback(CacheMasterKeyRemovedCallback);

            //init master
            double latest = Z2LatestCacheVersion;
        }


        public Z2Context()
        {
            if (HttpContext.Current.Session != null)
            {
                Session = HttpContext.Current.Session;
                Session.Timeout = 30;
                z2cookie = new Z2Cookie();  
            }

            if (this.z2cookie == null)
                z2cookie = new Z2Cookie();

            DependencyFileChanged = new System.Web.Caching.CacheDependency(Z2Config.MAPPED_SHOW_DEPENDENCY_FILE);
            CacheMasterKeyRemoved = new System.Web.Caching.CacheItemRemovedCallback(CacheMasterKeyRemovedCallback);

            //init master
            double latest = Z2LatestCacheVersion;
            //MasterCacheItemChanged = new CacheDependency(null, new string[] { MasterCacheKey });
        }

        #region Caching Policy Framework

        /// <summary>
        /// Establish a file dependency for refresh - assign a value in constructor
        /// 
        /// Other cached items should follow the LatestPublishedVersion changes as a dependency for refresh
        /// 
        /// http://www.high-flying.co.uk/c-sharp/cache.insert-with-file-dependency-and-callback-on-cache-removal.html
        /// </summary>
        public System.Web.Caching.CacheItemRemovedCallback CacheMasterKeyRemoved = null;
        public System.Web.Caching.CacheDependency DependencyFileChanged = null;
        public System.Web.Caching.CacheDependency MasterCacheItemChanged = null;
        private readonly string MasterCacheKey = "Z2LatestCacheVersion";

        /// <summary>
        /// Other stuff to do when cache is refreshed
        /// </summary>
        public void CacheMasterKeyRemovedCallback(String k, Object v, CacheItemRemovedReason r)
        {
            //do stuff here
        }

        /// <summary>
        /// This is the master key for the cache in this scope - it is simply a double that records the ticks of the last update/refresh
        /// </summary>
        public double Z2LatestCacheVersion
        {
            get
            {
                if (Cache[MasterCacheKey] == null)
                {
                    //this will expire either when the file is updated or in four hours
                    Cache.Insert(MasterCacheKey, Utils.ParseHelper.DateTime_To_JavascriptDate(DateTime.Now), 
                        DependencyFileChanged,
                        DateTime.Now.AddHours(4), System.Web.Caching.Cache.NoSlidingExpiration, 
                        CacheItemPriority.Normal, CacheMasterKeyRemovedCallback);
                }

                //provide a fallback value for null
                double ver = (Cache[MasterCacheKey] != null) ? (double)Cache[MasterCacheKey] : double.MinValue;
                return (ver != double.MinValue) ? ver : Utils.ParseHelper.DateTime_To_JavascriptDate(DateTime.Now);
            }
        }

        #endregion 


        #region Cached Collections

        private const string _foxUpcomingShows = "Z2FoxUpcomingShows";
        public List<Show> FoxUpcomingShows
        {
            get
            {
                if(Cache[_foxUpcomingShows] == null)
                {
                    List<Show> list = new List<Show>();

                    using (IDataReader rdr = SPs.TxGetSaleShowDates(_Config.APPLICATION_ID, _Config.SHOWOFFSETDATE.ToString("yyyy/MM/dd hh:mmtt")).GetReader())
                    {
                        ShowDateCollection showDates = new ShowDateCollection();
                        showDates.LoadAndCloseReader(rdr);

                        foreach (ShowDate sd in showDates)
                        {
                            if (sd.ShowRecord.VcPrincipal.ToLower().IndexOf("fox") != -1 && sd.ShowRecord.IsDisplayable)
                            {
                                // if this show does not ALREADY exist in the list
                                if (list.FindIndex(delegate(Show match) { return (match.Id == sd.TShowId); }) == -1)
                                {
                                    list.Add(sd.ShowRecord);
                                }
                            }
                        }
                    }

                    //sort!!!
                    list = new List<Show>(list.OrderBy(x => x.FirstDate)
                        .ThenBy(x => (x.VenueRecord.Name == "The Fox Theatre"))
                        .ThenBy(x => x.VenueRecord.Name));

                    //insert into cache - MSDN recommends UtcNow date
                    Cache.Insert(_foxUpcomingShows, list, new CacheDependency(null, new string[] { MasterCacheKey }));
                }

                return (List<Show>)Cache[_foxUpcomingShows];
            }
        }

        private readonly string _btRssEventListing = "Z2BtRssEventListing";
        /// <summary>
        /// Collection needs to be filtered to not show yet to be announced shows
        /// </summary>
        public List<BT_EventItem> BtRssEventListing
        {
            get
            {
                if (Cache[_btRssEventListing] == null)
                {
                    List<BT_EventItem> events = new List<BT_EventItem>();

                    try
                    {
                        string feedUrl = _Config._URL_BTEventFeed; ;// "http://vasl.webfactional.com/boulder/admin-eventfeed-v.php";//_Config._URL_BTEventFeed;

                        var syncClient = new WebClient();
                        var content = syncClient.DownloadString(feedUrl);

                        BT_EventFeed feed = Newtonsoft.Json.JsonConvert.DeserializeObject<BT_EventFeed>(content);

                        events = feed.eventlist.FindAll(delegate (BT_EventItem match) { return match.ShowDate > DateTime.Now.Date && match.AnnounceDate < DateTime.Now; });
                    }
                    catch (Exception e)
                    {
                        _Error.LogException(e);

                    }
                 
                    //if we have an empty collection - try to refresh in five minutes
                    //site could be temporarily down
                    if(events.Count == 0)
                        Cache.Insert(_btRssEventListing, events, new CacheDependency(null, new string[] { MasterCacheKey }), 
                            DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
                    else
                        Cache.Insert(_btRssEventListing, events, new CacheDependency(null, new string[] { MasterCacheKey }));
                    
                }

                return (List<BT_EventItem>)Cache[_btRssEventListing];
            }
        }

        private readonly string _banners = "Z2Banners";
        public List<SalePromotion> Banners
        {
            get
            {
                if (Cache[_banners] == null)
                {
                    //insert into cache
                    Cache.Insert(_banners,
                        SalePromotion.GetCurrentRunningBannerList(_Enums.Principal.all, false, _Config._BannerOrder_Random),
                        new CacheDependency(null, new string[] { MasterCacheKey }), DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return (List<SalePromotion>)Cache[_banners];
            }
        }

        private readonly string _employeeListing = "Z2Employees";
        public List<Employee> EmployeeListing
        {
            get
            {
                if (Cache[_employeeListing] == null)
                {
                    _Enums.Principal prince = WebContextPrincipal;

                    List<Employee> list = new List<Employee>(
                        Employee.GetEmployeesInContext(0, int.MaxValue, prince.ToString(), _Enums.CollectionSearchCriteriaStatusType.orderable.ToString(), 
                            DateTime.MinValue, DateTime.MaxValue, null).GetList());

                    //Sort!!!!
                    Employee_Principal.SortListByPrincipal(prince, ref list);


                    Cache.Insert(_employeeListing, list, new CacheDependency(null, new string[] { MasterCacheKey }));
                }

                return (List<Employee>)Cache[_employeeListing];
            }
        }

        #endregion


        #region Cookie objects

        /// <summary>
        /// Holds any keys for special handling - referals
        /// </summary>
        public string MarketingProgramKey { get { return this.z2cookie.MarketingProgram; } set { this.z2cookie.MarketingProgram = value; } }

        #endregion

        #region Boilerplate - DisplayErrors

        public string CurrentPageException
        {
            get
            {
                if (Session["PageException"] == null)
                    return null;

                return (string)Session["PageException"];
            }
            set
            {
                Session["PageException"] = value;
            }
        }

        public string FormatErrorText(string error)
        {
            return string.Format("<div class=\"alert alert-danger\">{0}</div>", error);
        }

        #endregion

    }
}