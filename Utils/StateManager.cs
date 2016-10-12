using System;
using System.Web;
using System.Web.UI;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    public enum ContextState
    {
        Application, Session, Cache, ViewState
    }

    /////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////    
    /// <summary>
    /// Keep all cached items in sync. Changes not committed with 
    /// "Published Now!" (btn action) in admin will be published 
    /// eventually due to the expiry of the main cached item. 
    /// When the item expires - it refreshes from whatever lives 
    /// in the DB wether or not it was explicitly published.    
    /// 
    /// </summary>
    /////////////////////////////////////////////////////////////
    public sealed class StateManager
    {
        #region Singleton pattern

        private static readonly StateManager obj = new StateManager();

        private System.Web.UI.StateBag ViewState = new StateBag();

        private StateManager() 
        { 
        }

        static StateManager() 
        {
        }

        public static StateManager Instance
        {
            get
            {
                return obj;
            }
        }

        public System.Web.HttpContext CurrentContext
        {
            get { return HttpContext.Current; }
        }

        public System.Web.Caching.Cache Cache
        {
            get { return HttpContext.Current.Cache; }
        }

        public void Add(string key, object data, ContextState state)
        {
            switch (state)
            {
                case ContextState.Application:
                    HttpContext.Current.Application.Add(key, data);
                    break;
                case ContextState.Session:
                    HttpContext.Current.Session.Add(key, data);
                    break;
                case ContextState.Cache:
                    HttpContext.Current.Cache.Insert(key, data);
                    break;
                case ContextState.ViewState: ViewState.Add(key, data);
                    break;
            }
        }

        public void Remove(string key, ContextState state)
        {

            if (HttpContext.Current == null)
            {
                return;
            }

            switch (state)
            {
                case ContextState.Application:
                    if (HttpContext.Current.Application[key] != null)
                        HttpContext.Current.Application.Remove(key);
                    break;
                case ContextState.Session:
                    if (HttpContext.Current.Session[key] != null)
                        HttpContext.Current.Session.Remove(key);
                    break;
                case ContextState.Cache:
                    if (HttpContext.Current.Cache[key] != null)
                        HttpContext.Current.Cache.Remove(key);
                    break;
                case ContextState.ViewState:
                    if (ViewState[key] != null)
                        ViewState.Remove(key);
                    break;
            }
        }

        public object Get(string key, ContextState state)
        {
            switch (state)
            {
                case ContextState.Application:
                    if (HttpContext.Current.Application[key] != null)
                        return HttpContext.Current.Application[key];
                    break;
                case ContextState.Session:
                    if (HttpContext.Current.Session[key] != null)
                        return HttpContext.Current.Session[key];
                    break;
                case ContextState.Cache:
                    if (HttpContext.Current.Cache[key] != null)
                        return HttpContext.Current.Cache[key];
                    break;
                case ContextState.ViewState:
                    if (ViewState[key] != null)
                        return ViewState[key];
                    break;
                default: return null;
            }
            return null;
        }

        public int Count(ContextState state)
        {
            switch (state)
            {
                case ContextState.Application:
                    if (HttpContext.Current.Application != null)
                        return HttpContext.Current.Application.Count;
                    break;
                case ContextState.Session:
                    if (HttpContext.Current.Session != null)
                        return HttpContext.Current.Session.Count;
                    break;
                case ContextState.Cache:
                    if (HttpContext.Current.Cache != null)
                        return HttpContext.Current.Cache.Count;
                    break;
                case ContextState.ViewState:
                    if (ViewState != null)
                        return ViewState.Count;
                    break;
                default: return 0;
            }

            return 0;
        }

        #endregion

        #region Cache Objects

        /// <summary>
        /// A delegate to handle the removal of an object - this is tied to the master cache object
        /// </summary>
        private static System.Web.Caching.CacheItemRemovedCallback MasterCacheItemRemoved = new CacheItemRemovedCallback(OnMasterCacheItemRemoved);

        /// <summary>
        /// Note we only call the lookup reset on the show dates - as the reset website cache calls reset
        /// </summary>
        public static void OnMasterCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            if (key.Equals("ShowDates_MasterList"))
            {
                KillCachedChildren();
            }
        }

        #region VenD Caching Helpers

        public static void RemoveCacheItems_ExactKey(string key)
        {
            if (Utils.StateManager.Instance.Cache[key] != null)
                Utils.StateManager.Instance.Remove(key, Utils.ContextState.Cache);
        }

        public static void RemoveCacheItems_StartsWithKey(string startsWithKey)
        {
            //get a list of matching keys
            List<string> matchingKeys = new List<string>();
            
            IDictionaryEnumerator en = Utils.StateManager.Instance.Cache.GetEnumerator();

            while(en.MoveNext())
                if (en.Key.ToString().StartsWith(startsWithKey))
                    matchingKeys.Add(en.Key.ToString());

            foreach (string s in matchingKeys)
                RemoveCacheItems_ExactKey(s);
        }

        #endregion

        //this will start a chain of events
        public static void RemoveWebsiteCacheItems()
        {
            if (Utils.StateManager.Instance.Cache["ShowDates_MasterList"] != null)
                Utils.StateManager.Instance.Remove("ShowDates_MasterList", Utils.ContextState.Cache);
            else
                KillCachedChildren();
        }

        private static void KillCachedChildren()
        {
            RemoveConnectedItems();
            ResetLookupCache();
        }

        /// <summary>
        /// removes items built off the parent show dates master list
        /// </summary>
        private static void RemoveConnectedItems()
        {
            //this has all changed to cachedependencies 
            //collections self-destruct when parent is removed

            //the private collection - the master pool of data for the web
            //Utils.StateManager.Instance.Remove("foxShowRepositoryWeb", Utils.ContextState.Cache);
            //the collection of announced shows within the pool
            //short shelf life to keep data fresh
            //Utils.StateManager.Instance.Remove("foxShowsWebDisplayable", Utils.ContextState.Cache);
            //the following already has a cache dependency declared for the above
            //Utils.StateManager.Instance.Remove("foxEventDatesDisplayable", Utils.ContextState.Cache);

        }

        //MAKE PLURAL!!!!!
        //Keep in sync with Wcss._LookupCollection table names
        public static string[] _LookupTableNames = {"Ages", "Employees", "FaqCategories", "FaqItems", "Genres", 
            "HintQuestions", "ShowStatii", "SiteConfigs", "Subscriptions" };

        private static void ResetLookupCache()
        {
            foreach (string s in _LookupTableNames)
                Utils.StateManager.Instance.Remove(string.Format("Lookup_{0}", s), Utils.ContextState.Cache);

            //others
            Utils.StateManager.Instance.Remove("BT_EventItem_Listings", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("BackgroundImageList", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("BannerList", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("KioskList", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("DefaultVenue", Utils.ContextState.Cache);//for seo
            
            //KioskList_Admin
            //Admin_Promoters
            //Admin_Venues
            Utils.StateManager.Instance.Remove("KioskList_Admin", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("Admin_Promoters", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("Admin_Venues", Utils.ContextState.Cache);
        }

        #endregion
    }
}
