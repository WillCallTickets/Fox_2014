using System;
using System.Web;
using System.Web.Caching;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace Wcss
{
    public class _ContextBase
    {
        protected HttpRequest Request;
        protected System.Web.Caching.Cache Cache;

        public _ContextBase()
        {
            Request = Utils.StateManager.Instance.CurrentContext.Request;
            Cache = Utils.StateManager.Instance.Cache;
        }

        //handle publish requests from admin
        protected void Publish()
        {
            //log it - this is instrumental in troubleshooting the cause of site errors/issues
            _Error.LogPublishEvent(DateTime.Now, _Enums.PublishEvent.Publish, HttpContext.Current.User.Identity.Name);
            Utils.StateManager.RemoveWebsiteCacheItems();

            UpdateCacheDependencyFile(_Enums.PublishEvent.Publish, HttpContext.Current.User.Identity.Name);
        }

        public void UpdateCacheDependencyFile(_Enums.PublishEvent evt, string userName)
        {
            string writeLineToFile = string.Format("{0} key: {1} reason: {2}{3}", 
                DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"), evt.ToString(), userName, Environment.NewLine);

            Utils.FileLoader.SaveToFile(_Config._MappedShowDependencyFile, writeLineToFile);
        }

        public double PublishVersion_Web
        {
            get
            {
                if (Cache["PublishVersion_Web"] == null)
                {
                    Cache.Insert("PublishVersion_Web", Utils.ParseHelper.DateTime_To_JavascriptDate(DateTime.Now),
                        new CacheDependency(null, new string[] { "ShowDates_MasterList" }));
                }

                double ver = (Cache["PublishVersion_Web"] != null) ? (double)Cache["PublishVersion_Web"] : double.MinValue;
                return (ver != double.MinValue) ? ver : Utils.ParseHelper.DateTime_To_JavascriptDate(DateTime.Now);
            }
        }

        static ReaderWriterLockSlim _slimLock_Master = new ReaderWriterLockSlim();

        /// <summary>
        /// BEWARE!!! This collection contains ALL FUTURE SHOWS!!!
        /// 
        /// You will probably need to filter this by to get only currently active shows
        /// Note that filtered objects are in a diff namespace due to their reliance on Linq
        /// 
        /// Think of this as the master collection. By keeping this in cache - we can work on 
        /// objects in the db without yet publishing (to some degree)
        /// 
        /// This collection needs to service both admin (real-time) and cached web
        /// </summary>
        public ShowDateCollection _ShowDates_MasterList_WebUseCopyOnly
        {
            get
            {
                try
                {
                    _slimLock_Master.EnterReadLock();

                    if (Cache["ShowDates_MasterList"] == null)
                    {
                        _slimLock_Master.ExitReadLock();
                        _slimLock_Master.EnterWriteLock();

                        //allow some time for the shows to be displayed
                        //gets shows with a date greater than now(specified) that are active
                        //add 8 hours so show will display after doors and for rest of evening
                        //this returns active shows
                        using (IDataReader rdr = SPs.TxGetSaleShowDates(_Config.APPLICATION_ID, _Config.SHOWOFFSETDATE.ToString("yyyy/MM/dd hh:mmtt")).GetReader())
                        {
                            ShowDateCollection showDates = new ShowDateCollection();
                            showDates.LoadAndCloseReader(rdr);

                            if (showDates.Count > 0)
                            {
                                //populate show url - do the fetching
                                foreach (ShowDate sd in showDates)
                                {
                                    string url = sd.ConfiguredUrl_withDomain;
                                }

                                //coordinate connected items in StateManager => RemoveConnectedItems
                                Cache.Insert("ShowDates_MasterList", showDates,
                                    new CacheDependency(_Config._MappedShowDependencyFile),
                                    DateTime.Now.AddMinutes(_Config._DataExpiryMins),//set to 5 hours - absolute expiry!
                                    System.Web.Caching.Cache.NoSlidingExpiration,
                                    CacheItemPriority.Normal,
                                    Utils.StateManager.OnMasterCacheItemRemoved);//trigger
                            }
                        }
                    }

                    return (ShowDateCollection)Cache["ShowDates_MasterList"];
                }
                catch (Exception e)
                {
                    _Error.LogException(e);

                }
                finally
                {
                    if (_slimLock_Master.IsWriteLockHeld)
                        _slimLock_Master.ExitWriteLock();

                    if (_slimLock_Master.IsReadLockHeld)
                        _slimLock_Master.ExitReadLock();
                }

                return null;
            }
        }
    }
}
