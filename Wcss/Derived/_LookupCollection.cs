using System;
using System.Text;
using System.Collections.Generic;

namespace Wcss
{
    public partial class _Lookits
    {
        private static System.Web.Caching.Cache _cache
        {
            get
            {
                return Utils.StateManager.Instance.Cache;
            }
        }

        #region Collection definitions

        public static Wcss.AgeCollection Ages
        {
            get
            {
                
                if (_cache["Lookup_Ages"] == null)
                {
                    Wcss.AgeCollection _ages = new Wcss.AgeCollection()
                        .Where(Age.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.Age.Columns.Name.ToString()).Load();

                    _cache.Add("Lookup_Ages", _ages, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.AgeCollection)_cache["Lookup_Ages"];
            }
        }

        public static Wcss.FaqCategorieCollection FaqCategories
        {
            get
            {

                if (_cache["Lookup_FaqCategories"] == null)
                {
                    Wcss.FaqCategorieCollection _faqcats = new Wcss.FaqCategorieCollection()
                        .Where(FaqCategorie.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.FaqCategorie.Columns.IDisplayOrder.ToString()).Load();

                    _cache.Add("Lookup_FaqCategories", _faqcats, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.FaqCategorieCollection)_cache["Lookup_FaqCategories"];
            }
        }

        public static Wcss.GenreCollection Genres
        {
            get
            {
                if (_cache["Lookup_Genres"] == null)
                {
                    Wcss.GenreCollection _genres = new Wcss.GenreCollection()
                        .Where(Genre.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.Genre.Columns.Name.ToString()).Load();

                    _cache.Add("Lookup_Genres", _genres, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.GenreCollection)_cache["Lookup_Genres"];
            }
        }

        public static Wcss.HintQuestionCollection HintQuestions
        {
            get
            {
                if (_cache["Lookup_HintQuestions"] == null)
                {
                    Wcss.HintQuestionCollection _hintQuestions = new Wcss.HintQuestionCollection()
                        .Where(HintQuestion.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.HintQuestion.Columns.IDisplayOrder.ToString()).Load();

                    _cache.Add("Lookup_HintQuestions", _hintQuestions, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.HintQuestionCollection)_cache["Lookup_HintQuestions"];
            }
        }

        /// <summary>
        /// Statii are the same across apps
        /// </summary>
        public static Wcss.ShowStatusCollection ShowStatii
        {
            get
            {
                if (_cache["Lookup_ShowStatii"] == null)
                {
                    Wcss.ShowStatusCollection _showStatii = new Wcss.ShowStatusCollection().OrderByAsc(Wcss.ShowStatus.Columns.Name.ToString()).Load();

                    _cache.Add("Lookup_ShowStatii", _showStatii, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.ShowStatusCollection)_cache["Lookup_ShowStatii"];
            }
        }

        public static Wcss.SiteConfigCollection SiteConfigs
        {
            get
            {
                if (_cache["Lookup_SiteConfigs"] == null)
                {
                    //could be unnecessary - but.... (081102)
                    _cache.Remove("Lookup_SiteConfigs");
                    Wcss.SiteConfigCollection _siteConfigs = new Wcss.SiteConfigCollection()
                        .Where(SiteConfig.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.SiteConfig.Columns.Context).OrderByAsc(Wcss.SiteConfig.Columns.Name).Load();

                    _cache.Add("Lookup_SiteConfigs", _siteConfigs, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.SiteConfigCollection)_cache["Lookup_SiteConfigs"];
            }
        }   
    
        public static Wcss.SubscriptionCollection Subscriptions
        {
            get
            {
                if (_cache["Lookup_Subscriptions"] == null)
                {
                    Wcss.SubscriptionCollection _subs = new Wcss.SubscriptionCollection()
                        .Where(Subscription.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByDesc(Wcss.Subscription.Columns.DtStamp).Load();

                    _cache.Add("Lookup_Subscriptions", _subs, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.SubscriptionCollection)_cache["Lookup_Subscriptions"];
            }
        }

        #endregion

        static _Lookits() { }

        //MAKE PLURAL!!!!!
        //Keep in sync with StateManager and enums
        //public const string[] _LookupTableNames = {"Ages", "Employees", "FaqCategories", "FaqItems", "Genres", "HintQuestions", 
        //    "ShowStatii", 
        //    "SiteConfigs", "Subscriptions" };


        #region Methods

        /// <summary>
        /// Removes all cached keys that are prefixed with the word "Lookups_"
        /// </summary>
        public static void RefreshLookup(string collectionName)
        {
            //only if we have approved the key!
            foreach (string s in Enum.GetNames(typeof(_Enums.LookupTableNames)))
            {
                if (s.ToLower().Equals(collectionName.ToLower()))
                {
                    string key = string.Format("Lookup_{0}", s);
                    object obj = _cache[key];
                    if (obj != null)
                        _cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes all cached keys that are prefixed with the word "Lookups_"
        /// </summary>
        public static void RefreshAll()
        {
            foreach (string s in Enum.GetNames(typeof(_Enums.LookupTableNames)))
            {
                string key = string.Format("Lookup_{0}", s);
                object obj = _cache[key];
                if (obj != null)
                    _cache.Remove(key);
            }
        }

        #endregion
    }
}
