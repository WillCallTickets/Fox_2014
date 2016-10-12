using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Caching;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;

using Wcss;
using SubSonic;
using wctMain.Model;

namespace wctMain.Controller
{
	public class MainContext : _ContextBase
    {

        /// <summary>
        /// string prince = _PrincipalBase.PrincipalFromUrlHost(this.Request).ToString();
        /// </summary>
        public _Enums.Principal WebContextPrincipal = _Enums.Principal.fox;

        #region WebContext, Cache Overhead And Callbacks

        public MainCookie Scookie;
        public HttpSessionState Session;
        
        public MainContext(HttpSessionState session)
        {
            Session = session;
            Session.Timeout = 30;
            Scookie = new MainCookie();            
        }

        public MainContext()
        {
            if (HttpContext.Current.Session != null)
            {
                Session = HttpContext.Current.Session;
                Session.Timeout = 30;
                Scookie = new MainCookie();
            }

            if (this.Scookie == null)
                Scookie = new MainCookie();
        }

        #endregion

        public bool InDevMode {
            get
            {
                return (Wcss._Config._DomainName.ToLower() != "local.fox2014.com" ||
                        Wcss._Config._DomainName.ToLower() != "localhost");
            }
        }

        /// <summary>
        /// tells us where to redirect to after an old user is update
        /// </summary>
        public string RedirectOnAuth
        {
            get
            {
                return (string)Session["RedirectOnAuth"];
            }
            set
            {
                if (value == null)
                    Session.Remove("RedirectOnAuth");
                else
                    Session["RedirectOnAuth"] = value;
            }
        }

        public string UserInfo
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    return string.Format("{1} IP: {3} User: {2}{0} Agent: {4}{0}Platform: {5} Browser: {6} {7}",
                        Environment.NewLine,
                        DateTime.Now.ToString(),
                        (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated) ? 
                            HttpContext.Current.User.Identity.Name : "unknown",
                        HttpContext.Current.Request.UserHostAddress,
                        HttpContext.Current.Request.UserAgent,
                        HttpContext.Current.Request.Browser.Platform,
                        HttpContext.Current.Request.Browser.Browser, 
                        HttpContext.Current.Request.Browser.Version);
                }
                else
                    return string.Format("{0}: User: {1} Request Object Unknown",
                        DateTime.Now.ToString(), " unknown");
            }
        }

        #region Show Retrieval

        /// <summary>
        /// allows access to all shows - active or not
        /// </summary>
        /// <param name="url"></param>
        /// <param name="assignDefaultOnEmptyUrl"></param>
        /// <returns></returns>
        public Show GetAdminShowByUrl(string url)
        {
            Show s = null;

            if (url == null || url.Trim().Length == 0)
            {
                s = ShowRepo_Web_Displayable[0];
            }
            else
            {
                //examine url for legacy format
                url = url.TrimStart('/');

                //find the matching showdate
                //get the id
                //load a show by the id

                ShowCollection coll = new ShowCollection();

                //make sure that this is active and displayable - TEST!!!
                using (IDataReader rdr = SPs.TxGetShowByUrl(_Config.APPLICATION_ID, url, false).GetReader())
                    coll.LoadAndCloseReader(rdr);

                if (coll.Count > 0)
                    s = coll[0];

                //we have created the show - so double check for name
                if (s != null && s.Name.Trim().Length == 0)
                    s = null;
            }

            return s;
        }

        /// <summary>
        /// Returns an Active Show. Displayable is an option
        /// First ensures that we have url and will set a default if requested. Do this for default pages, but
        /// not for pages where we would like to ensure a null result to indicate not found.
        /// If the show is not matched within the current list of shows, it will serach the DB with an SP.
        /// </summary>
        public Show GetCurrentShowByUrl(string url, bool assignDefaultOnEmptyUrl, bool mustBeDisplayble)
        {
            Show s = null;

            if (url == null || url.Trim().Length == 0)
            {
                if (assignDefaultOnEmptyUrl && this.ShowRepo_Web_Displayable.Count > 0)
                    s = ShowRepo_Web_Displayable[0];
            }
            else
            {
                //examine url for legacy format
                url = url.TrimStart('/');

                //attempt to match the url to active and displayble shows
                s = ShowRepo_Web_Displayable.Find(delegate(Show match)
                {
                    return (match.ContainsShowDateWithMatchingUrl(url));
                });

                //if no match then search db
                if (s == null)
                {
                    ShowCollection coll = new ShowCollection();

                    //make sure that this is active and displayable - TEST!!!
                    using (IDataReader rdr = SPs.TxGetShowByUrl(_Config.APPLICATION_ID, url, false).GetReader())
                        coll.LoadAndCloseReader(rdr);

                    if (coll.Count > 0)
                        s = coll[0];


                    //checks for valid selection
                    if (s != null)
                    {
                        //all shows must be active in this context!
                        if (!s.IsActive)
                            s = null;

                        //we have created the show - so double check for name
                        if (s != null && s.Name.Trim().Length == 0)
                            s = null;

                        //check displayable flag
                        if (s != null && (mustBeDisplayble) && (!s.IsDisplayable))
                            s = null;
                    }
                }
            }

            return s;
        }

        public Show GetCurrentShowById(int idx, bool includePastShowsInSearch)
        {
            return GetCurrentShowById(idx, includePastShowsInSearch, true);
        }
        public Show GetCurrentShowById(int idx, bool includePastShowsInSearch, bool webDisplayableOnly)
        {
            Show s = null;

            //attempt to match the url to active and displayble shows
            s = ShowRepo_Web_Displayable.Find(delegate(Show match)
            {
                return (match.Id == idx);// && match.IsDisplayable);
            });

            if (s != null && s.Id == 0)
                s = null;

            //if no match then search db
            if (s == null && includePastShowsInSearch)
                s = new Show(idx);
            
            //we have created the show - so double check for name
            if (s != null && s.Id == 0)
                s = null;

            if (s != null && webDisplayableOnly && (!s.IsDisplayable))
                s = null;
                
            return s;
        }

        /// <summary>
        /// EventDateID is a ShowDateId
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="includePastShowsInSearch"></param>
        /// <param name="webDisplayableOnly"></param>
        /// <returns></returns>
        public Show GetCurrentShowByEventDateId(int idx, bool includePastShowsInSearch, bool webDisplayableOnly)
        {
            Show s = null;

            //attempt to match the url to active and displayble shows
            s = ShowRepo_Web_Displayable.Find(delegate(Show match)
            {
                return (match.ShowDateRecords().GetList().FindIndex(delegate (ShowDate child) { return (child.Id == idx); } ) != -1);
            });

            if (s != null && s.Id == 0)
                s = null;

            //if no match then search db
            if (s == null && includePastShowsInSearch)
                s = new Show(idx);

            //we have created the show - so double check for name
            if (s != null && s.Id == 0)
                s = null;

            if (s != null && webDisplayableOnly && (!s.IsDisplayable))
                s = null;

            return s;
        }

        #endregion

        #region Cached lookups and singles

        /// <summary>
        /// only get displayable shows from cached shows
        /// set max number of shows to return
        /// </summary>
        public List<Show> JustAnnounced
        {
            get 
            {
                List<Show> list = new List<Show>();
                list.AddRange(JustAnnouncedShowCache.FindAll(delegate(Show match) { return (match.IsDisplayable); }));
                list.Sort(delegate(Show x, Show y) { return x.FirstShowDate.DateOfShow_ToSortBy.CompareTo(y.FirstShowDate.DateOfShow_ToSortBy); });

                return list;
            }
        }

        private List<Show> JustAnnouncedShowCache
        {
            get
            {
                if (Cache["JustAnnouncedShowCache"] == null)
                {
                    ShowCollection coll = new ShowCollection();

                    //use shows announced within the last month as a default
                    DateTime announcedWithinTheLast = DateTime.Now.AddDays(-1*_Config._JustAnnouncedWindow_Days);
                    DateTime showDateGreaterThan = DateTime.Now.AddHours(-12);//SP will adjust to correct time

                    using (IDataReader rdr = SPs.TxGetJustAnnouncedShows(_Config.APPLICATION_ID, _Enums.Principal.fox.ToString(),
                            announcedWithinTheLast, showDateGreaterThan, 0, 10000).GetReader())
                    {   
                        coll.LoadAndCloseReader(rdr);
                    }

                    //now fill list with what we have in the other cached objects - utilize what may have already been queried
                    //opted for ShowRepo_Web_Displayable - let's see how it works out
                    List<Show> list = new List<Show>();

                    foreach(Show s in coll)
                    {
                        Show cachedShow = this.ShowRepo_Web_Displayable.Find(delegate(Show match)
                        {
                            return (match.Id == s.Id);
                        });

                        if (cachedShow != null && cachedShow.Id > 0)
                            list.Add(cachedShow);
                    }


                    //insert into cache
                    //vars:
                    //  principal = fox 
                    //  minAnnounceDate = 1 month ago = shows announced within the last month
                    //  minShowDate = now = show has to be in future starting with now
                    //refresh every 15 minutes
                    //dependency on master show list - changes on publish
                    Cache.Insert("JustAnnouncedShowCache", list,new CacheDependency(null, new string[] { "ShowDates_MasterList" }), 
                        DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return (List<Show>)Cache["JustAnnouncedShowCache"];
            }
        }

        public string KioskData_Json
        {
            get
            {
                if (Cache["KioskDataJson"] == null)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine();

                    //Version information - note that this is tied to the show dates and not the eventdates - closer to source
                    sb.AppendFormat("var wdataVersion = {0};", this.PublishVersion_Web.ToString());
                    sb.AppendLine();

                    sb.AppendFormat("var wannVersion = {0};", this.PublishVersion_Announced.ToString());
                    sb.AppendLine();

                    //Events
                    List<KioskEvent> list = new List<KioskEvent>();
                    
                    foreach (Kiosk ent in this.KioskList)
                    {
                        KioskEvent ed = new KioskEvent(ent);

                        if (!list.Contains(ed))
                            list.Add(ed);
                    }

                    sb.AppendFormat("var kiosks = {0};",
                        (list.Count > 0) ? serializer.Serialize(list) : string.Empty);
                    sb.AppendLine();

                    sb.AppendFormat("var defaultImage = '{0}';", "/WillCallResources/Images/bgs/balcrowdbw.jpg");
                    sb.AppendLine();

                    sb.AppendLine("</script>");
                    
                    Cache.Insert("KioskDataJson", sb.ToString(), new CacheDependency(null, new string[] { "KioskList" }));
                }

                return Cache["KioskDataJson"].ToString();
            }
        }

        ////expires every four hours for freshness
        /// <summary>
        /// Maintain a list of all Active kiosks and soon to be active kiosks
        /// Let the client also decide if the announce Date is past
        /// </summary>
        public List<Kiosk> KioskList
        {
            get
            {
                if (Cache["KioskList"] == null)
                {
                    int announceDelay = (int)Math.Floor(1.33 * 60 * 60);

                    string sql = "SELECT * FROM [Kiosk] k WHERE k.[ApplicationId] = @appId AND k.[bActive] = 1 AND ";
                    sql += "(k.[dtStartDate] IS NULL OR k.[dtStartDate] < @cacheFutureDate) AND ";
                    sql += "(k.[dtEndDate] IS NULL OR k.[dtEndDate] > getDate()) ";
                    // if we decide to list kiosks by principal....
                    //sql += "(k.[dtEndDate] IS NULL OR k.[dtEndDate] > getDate()) AND ";
                    //sql += "CHARINDEX(@principal, k.[vcPrincipal]) >= 1 ";

                    _DatabaseCommandHelper dch = new _DatabaseCommandHelper(sql);

                    dch.AddCmdParameter("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                    dch.AddCmdParameter("@cacheFutureDate", DateTime.Now.Add(TimeSpan.FromSeconds((Double)announceDelay)), System.Data.DbType.DateTime);
                    // if we decide to list kiosks by principal....
                    // dch.AddCmdParameter("@principal", WebContextPrincipal.ToString(), System.Data.DbType.String);


                    KioskCollection coll = new KioskCollection();
                    dch.PopulateCollectionByReader<KioskCollection>(coll);


                    List<Kiosk> list = new List<Kiosk>(coll);
                    Kiosk_Principal.SortListByPrincipal(WebContextPrincipal, ref list);

                    //insert into cache
                    Cache.Insert("KioskList", list, null, DateTime.Now.AddHours(4), System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return (List<Kiosk>)Cache["KioskList"];
            }
        }

        public List<SalePromotion> BannerList
        {
            get
            {
                if (Cache["BannerList"] == null)
                {
                    //insert into cache
                    Cache.Insert("BannerList", 
                        SalePromotion.GetCurrentRunningBannerList(WebContextPrincipal, false, _Config._BannerOrder_Random), 
                        null, DateTime.Now.AddHours(4), System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return (List<SalePromotion>)Cache["BannerList"];
            }
        }

        /// <summary>
        /// Note name has lookup in it - leave as it is handled by refresh on publish
        /// 2 days is fine - these won't referesh often and are not tied to time
        /// </summary>
        public List<Employee> EmployeeList
        {
            get
            {

                if (Cache["Lookup_Employees"] == null)
                {
                    Wcss.EmployeeCollection coll = new Wcss.EmployeeCollection()
                        .Where(Employee.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .Where(Employee.Columns.BListInDirectory, true)
                        .Where(Employee.Columns.VcPrincipal, SubSonic.Comparison.Like, string.Format("%{0}%", WebContextPrincipal.ToString()))
                        .Load();

                    List<Employee> list = new List<Employee>(coll);

                    Employee_Principal.SortListByPrincipal(WebContextPrincipal, ref list);

                    Cache.Add("Lookup_Employees", list, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (List<Employee>)Cache["Lookup_Employees"];
            }
        }

        public Venue DefaultVenue
        {
            get
            {
                if (Cache["DefaultVenue"] == null)
                {
                    Venue v = new Venue("Name", _Config._Default_VenueName);
                    if (v != null)
                        Cache.Insert("DefaultVenue", v);//dependencies tied to showdates
                }

                return (Venue)Cache["DefaultVenue"];
            }
        }

        public void BT_EventItem_Listing_Reset()
        {
            Utils.StateManager.Instance.Remove("BT_EventItem_Listing", Utils.ContextState.Cache);
        }


        //static ReaderWriterLockSlim _slimLock_BT = new ReaderWriterLockSlim();

        /// <summary>
        /// This only returns the list of events from the feed. There is other info available - just not used right now. 
        /// </summary>
        public List<BT_EventItem> BT_EventItem_Listing
        {
            get
            {
                //try
                //{
                    //_slimLock_BT.EnterReadLock();

                    if (Cache["BT_EventItem_Listing"] == null)
                    {
                        //_slimLock_BT.ExitReadLock();
                        //_slimLock_BT.EnterWriteLock();

                        List<BT_EventItem> events = new List<BT_EventItem>();

                        // http://boulder theater.com/admin-eventfeed
                        // http://vasl.webfactional.com/boulder/admin-eventfeed-v.php

                        string feedUrl = _Config._URL_BTEventFeed;

                        var syncClient = new WebClient();
                        var content = syncClient.DownloadString(feedUrl);

                        if (content != null)
                        {
                            BT_EventFeed feed = Newtonsoft.Json.JsonConvert.DeserializeObject<BT_EventFeed>(content);
                            
                            events.AddRange(feed.eventlist);
                            try
                            {
                                events.Sort(delegate(BT_EventItem x, BT_EventItem y)
                                {
                                    return x.ShowDate.CompareTo(y.ShowDate);
                                });
                            }
                            catch (Exception ex)
                            {
                                _Error.LogException(ex);
                            }
                            
                        }

                        //if we have an empty collection - try to refresh in five minutes
                        //site could be temporarily down
                        if (events.Count == 0)
                            Cache.Insert("BT_EventItem_Listing", events, null,
                                DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
                        else
                        {
                            int idx = 1;
                            foreach (BT_EventItem bte in events) {
                                bte.Id = idx++;
                            }

                            Cache.Insert("BT_EventItem_Listing", events, null,
                                DateTime.Now.AddMinutes(12), System.Web.Caching.Cache.NoSlidingExpiration);
                        }
                    }

                    return (List<BT_EventItem>)Cache["BT_EventItem_Listing"];
                //}
                //catch (Exception e)
                //{
                //    _Error.LogException(e);

                //}
                ////finally
                ////{
                ////    if (_slimLock_BT.IsWriteLockHeld)
                ////        _slimLock_BT.ExitWriteLock();

                ////    if (_slimLock_BT.IsReadLockHeld)
                ////        _slimLock_BT.ExitReadLock();
                ////}

                //return null;
            }
        }
       
        #endregion

        #region Cached ***Show and Date Collections - Utils.StateManager - put here due to reliance on Linq***

        //SHOW COLLECTIONS AND OBJECTS        
        /// <summary>
        /// WARNING - contains ALL FUTURE SHOWS!!! Should only be used by admin directly. Everyone else should use the web displayable collection below
        /// 
        /// CONTAINS ONY FOX SHOWS
        /// 
        /// This is the master Show list for the web portion of the site and services both web and admin
        /// Create Announced Dates collections from this object - leave the base.ShowDates_MasterList alone and use this object as the working copy.
        /// Announced collections should use this a as a pool for constructing their own data
        /// This object has no cache expiry assigned. It's expiry is tied to the parent object (ShowDates_MasterList)
        /// Master list is set to expire evey 300 minutes (5 hours)
        /// </summary>
        public List<Show> ShowRepo_Web
        {
            get
            {
                if (Cache["foxShowRepositoryWeb"] == null)
                {
                    //working objects - lists and collections
                    List<Show> working = new List<Show>();
                    List<Show> workingSorted = new List<Show>();

                    List<ShowDate> dates = _ShowDates_MasterList_WebUseCopyOnly.GetList()
                        .FindAll(delegate(ShowDate entity) { return (entity.ShowRecord.VcPrincipal.ToLower() == _Enums.Principal.fox.ToString()); });

                    //establish working set - all active shows from the master list                    
                    foreach (ShowDate sd in dates)
                    {
                        string test = sd.ConfiguredUrl;

                        int idx = working.FindIndex(delegate (Show match) { return (match.Id == sd.TShowId); });

                        if (sd.IsActive && sd.ShowRecord.IsActive && (idx == -1))
                            working.Add(sd.ShowRecord);
                    }

                    //sort our working set so that default venue shows are order prior to outside venues
                    var sortedColl =
                        from listItem in working
                        select listItem;

                    workingSorted.AddRange(sortedColl.OrderBy(x => x.FirstShowDate.DateOfShow_ToSortBy)
                        .ThenBy(x => (x.VenueRecord.Name_Displayable.ToLower() == _Config._Default_VenueName.ToLower()) ?
                            string.Empty : x.VenueRecord.NameRoot));

                    //dispose of working objects
                    working = null;

                    //insert into cache
                    Cache.Insert("foxShowRepositoryWeb", workingSorted, 
                        new CacheDependency(null, new string[] { "ShowDates_MasterList" }));
                }

                return (List<Show>)Cache["foxShowRepositoryWeb"];
            }
        }

        public double PublishVersion_Announced
        {
            get
            {
                if (Cache["PublishVersion_Announced"] == null)
                {
                    //foxShows collection will have a unique expiry
                    Cache.Insert("PublishVersion_Announced", Utils.ParseHelper.DateTime_To_JavascriptDate(DateTime.Now),                        
                        new CacheDependency(null, new string[] { "foxShowsWebDisplayable" }));
                }

                double ver = (Cache["PublishVersion_Announced"] != null) ? (double)Cache["PublishVersion_Announced"] : double.MinValue;
                return (ver != double.MinValue) ? ver : Utils.ParseHelper.DateTime_To_JavascriptDate(DateTime.Now);
            }
        }
        
        /// <summary>
        /// This object has a short expiry (minutesToDelayAnnounce) so that it refreshes to include only announced shows
        /// This saves a db hit but keeps our display data fresh - once again for shows yet to be announced
        /// The time may be adjusted for finer grain control, but as it is only handling announced shows as of
        ///     this moment, than every 10 mins is fine
        /// </summary>
        public List<Show> ShowRepo_Web_Displayable
        {
            get
            {
                if (Cache["foxShowsWebDisplayable"] == null)
                {
                    // 160213
                    // someone found a way to scrape our data so now we need to be more careful of the public data
                    // the new plan is that when the cache expires, find the next announcedate in line that is greater than now
                    // and set that as the new expiry vs 12 hours from now
                    // if there are no expiries - then 12 hours
                    DateTime now = DateTime.Now;


                    // I read somewhere that UTC date is preferred, but then
                    // I also read that the caching call converts to UTC date 
                    //DateTime utcNow = DateTime.UtcNow;

                    List<Show> working = new List<Show>();

                    // leeway makes it so we don't have to be "to the millisecond" on the nextAnnounce
                    int leeway = 10;
                    DateTime nextExpiry = DateTime.MaxValue;

                    //add shows where aannounce <= now + leeway
                    foreach (Show s in ShowRepo_Web)
                    {
                        //compare the announce date with some leeway against now
                        if (s.AnnounceDate_Virtual.AddMinutes(-leeway) <= now)
                            working.Add(s);
                        else if (s.AnnounceDate_Virtual < nextExpiry)
                            nextExpiry = s.AnnounceDate_Virtual;
                    }

                    // we do NOT need leeway here
                    if (nextExpiry > now.AddHours(12))
                        nextExpiry = now.AddHours(12);

                    // adjust next announce so that it is sometime before the actual
                    // but not as far in advance as leeway
                    nextExpiry = nextExpiry.AddMinutes(-leeway + 2);

                    // insert into cache - MSDN recommends UtcNow date
                    // expiry should be four minutes prior to actual time - allow some leeway
                    // if something gets scraped in that 4 mins - than so be it
                    Cache.Insert("foxShowsWebDisplayable", 
                        working, 
                        new CacheDependency(null, new string[] { "ShowDates_MasterList" }),
                        nextExpiry, 
                        Cache.NoSlidingExpiration);

                    //Log the event so we can track
                    //todo remove this when we are certain things are running smoothly
                    _Error.LogToFile(
                        string.Format("{0} ShowRepo_Web_Displayable \r\nPUBLISHED: \t{1}\r\nEXPIRES: \t{2}",
                            DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.ffftt").ToUpper(),
                            now.ToString("yyyy/MM/dd hh:mm:ss.ffftt").ToLower(),
                            nextExpiry.ToString("yyyy/MM/dd hh:mm:ss.ffftt").ToLower()),
                            "AutoPublishLogger"
                        );               
                }

                return (List<Show>)Cache["foxShowsWebDisplayable"];
            }
        }

        /// <summary>
        /// Create a list of showdates (Events) that are used for displaying menus
        /// </summary>
        public static List<EventDate> ConvertShowDatesToEventDates(List<Show> showlist)
        {
            List<EventDate> working = new List<EventDate>();

            foreach (Show s in showlist)
            {
                //since this is just for the listing - we only need shows in the future
                ShowDateCollection coll = new ShowDateCollection();
                        
                coll.AddRange(s.ShowDateRecords().GetList().FindAll(delegate(ShowDate match) { 
                    return (match.IsActive && match.DateOfShow_ToSortBy > _Config.SHOWOFFSETDATE); } ));

                if(coll.Count > 1)
                    coll.Sort("DateOfShow_ToSortBy", true);

                foreach (ShowDate sd in coll)
                {
                    EventDate ed = new EventDate(sd);

                    if(!working.Contains(ed))
                        working.Add(ed);
                }
            }

            return working;
        }

        public string WebData_Json
        {
            get 
            {
                if (Cache["foxWebDataJson"] == null)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine();

                    //Version information - note that this is tied to the show dates and not the eventdates - closer to source
                    sb.AppendFormat("var wdataVersion = {0};", this.PublishVersion_Web.ToString());
                    sb.AppendLine();
                    sb.AppendFormat("var wannVersion = {0};", this.PublishVersion_Announced.ToString());
                    sb.AppendLine();

                    //Statics
                    sb.AppendFormat("var staticPageList = {0};",
                        serializer.Serialize(Enum.GetNames(typeof(_Enums.StaticPage))));
                    sb.AppendLine();

                    //Events
                    List<EventDate> list = new List<EventDate>();
                    list.AddRange(MainContext.ConvertShowDatesToEventDates(this.ShowRepo_Web_Displayable));
              
                    sb.AppendFormat("var events = {0};",
                        (list.Count > 0) ? serializer.Serialize(list) : string.Empty);
                    sb.AppendLine();
                    

                    sb.AppendFormat("var defaultImage = '{0}';", "/WillCallResources/Images/bgs/balcrowdbw.jpg");
                    sb.AppendLine();

                    sb.AppendFormat("var defaultShowDateId = {0};", _Config.Default_ShowDateId.ToString());
                    sb.AppendLine();

                    sb.AppendLine("</script>");

                    //foxShows collection will have a unique expiry
                    Cache.Insert("foxWebDataJson", sb.ToString(), 
                        new CacheDependency(null, new string[] { "foxShowsWebDisplayable" }));
                }

                return Cache["foxWebDataJson"].ToString();
            }
        }
                
        #endregion

        #region Cookie objects

        /// <summary>
        /// Holds any keys for special handling - referals
        /// </summary>
        public string MarketingProgramKey { get { return this.Scookie.MarketingProgram; } set { this.Scookie.MarketingProgram = value; } }

        #endregion

        #region Boilerplate - DisplayErrors, LogoutUser

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

        public string DisplayErrorText(string error)
        {
            return string.Format("<div class=\"alert alert-danger\">{0}</div>", error);
        }

        public void LogoutUser()
        {
            Session.RemoveAll();
            Session.Abandon();

            System.Web.Security.FormsAuthentication.SignOut();
        }

        #endregion

	}
}
