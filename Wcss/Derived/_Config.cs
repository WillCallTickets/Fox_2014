using System;
using System.Configuration;
using System.Collections.Generic;

namespace Wcss
{
	/// <summary>
	/// Wraps the app.config file by exposing it's key/value pairs
	/// as static properties.
	/// </summary>
	/// <example>
	/// For example, instead of using
	/// <code>DataManager dm = new DataManager(ConfigurationManager.AppSettings["dsn"]);</code>
	/// the wrapped static method can be used:
	/// <code>DataManager dm = new DataManager(Config.Dsn);</code>
	/// </example>

	public partial class _Config
    {
        /// <summary>
        /// This does not refresh collections
        /// </summary>
        public static SiteConfig AddNewConfig(_Enums.SiteConfigContext context, _Enums.ConfigDataTypes datatype, int maxlength,
            string configname, string description, string defaultvalue)
        {
            SiteConfig config = new SiteConfig();
            config.ApplicationId = _Config.APPLICATION_ID;
            config.Context = context.ToString();
            config.DataType = datatype.ToString().TrimStart('_');
            config.DtStamp = DateTime.Now;
            config.MaxLength = maxlength;
            config.Name = configname;
            config.Description = description;
            config.ValueX = defaultvalue;
            config.Save();
            _Lookits.SiteConfigs.Add(config);
            return config;
        }

        
        public static bool _SiteIsInTestMode { get { return bool.Parse(ConfigurationManager.AppSettings["stp_SiteIsInTestMode"]); } }

        //Global vars

        #region Default ShowDate

        private static ShowDate _default_ShowDate_Instance = null;
        public static int Default_ShowDateId
        {
            get
            {  
                //load from db
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.GlobalPrivate.ToString().ToLower() &&
                            match.Name.ToLower() == "default_showdateid" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });

                    
                if (config == null)
                {
                    SiteConfig newConfig = new SiteConfig();
                    newConfig.ApplicationId = _Config.APPLICATION_ID;
                    newConfig.Context = _Enums.SiteConfigContext.GlobalPrivate.ToString();
                    newConfig.DataType = "int";
                    newConfig.Description = "Stores the default id of the show date to display on first entry to the site.";
                    newConfig.DtStamp = DateTime.Now;
                    newConfig.MaxLength = 10;
                    newConfig.Name = "Default_ShowDateId";
                    newConfig.ValueX = "0";

                    newConfig.Save();

                    _Lookits.RefreshAll();

                    config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.GlobalPrivate.ToString().ToLower() &&
                            match.Name.ToLower() == "default_showdateid" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                }
                
                if (config != null && config.Id > 0)
                {
                    if (_default_ShowDate_Instance == null || _default_ShowDate_Instance.Id != config.Id)
                    {
                        _default_ShowDate_Instance = new ShowDate(config.Id);
                    }

                    if (_default_ShowDate_Instance.DateOfShow_ToSortBy > _Config.SHOWOFFSETDATE)
                    {
                        config.ValueX = "0";
                        config.Save();
                    }

                    return int.Parse(config.ValueX.Trim());
                }
              
                //default
                return 0;
              
            }
            set
            {
                    //determine if the show is in the past
                if (value > 0)
                {
                    ShowDate sd = new ShowDate(value);
                    if (sd == null)
                        throw new ArgumentNullException("ShowDate is null at set default show date id.");
                }

                try
                {
                    //load from db
                    SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                        delegate(SiteConfig match)
                        {
                            return (match.Context.ToLower() == _Enums.SiteConfigContext.GlobalPrivate.ToString().ToLower() &&
                                match.Name.ToLower() == "default_showdateid" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                        });

                    if (config == null)
                        throw new ArgumentNullException();

                    if (value.ToString() != config.ValueX)
                    {
                        config.ValueX = value.ToString();
                        config.Save();

                        _default_ShowDate_Instance = null;
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }
        }

        private static ShowDate _default_BT_ShowDate_Instance = null;
        public static int Default_BT_ShowDateId
        {
            get
            {
                //load from db
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.GlobalPrivate.ToString().ToLower() &&
                            match.Name.ToLower() == "default_bt_showdateid" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });


                if (config == null)
                {
                    SiteConfig newConfig = new SiteConfig();
                    newConfig.ApplicationId = _Config.APPLICATION_ID;
                    newConfig.Context = _Enums.SiteConfigContext.GlobalPrivate.ToString();
                    newConfig.DataType = "int";
                    newConfig.Description = "Stores the default id of the BT show date to display on first entry to the site.";
                    newConfig.DtStamp = DateTime.Now;
                    newConfig.MaxLength = 10;
                    newConfig.Name = "Default_BT_ShowDateId";
                    newConfig.ValueX = "0";

                    newConfig.Save();

                    _Lookits.RefreshAll();

                    config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.GlobalPrivate.ToString().ToLower() &&
                            match.Name.ToLower() == "default_bt_showdateid" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                }

                if (config != null && config.Id > 0)
                {
                    if (_default_BT_ShowDate_Instance == null || _default_BT_ShowDate_Instance.Id != config.Id)
                    {
                        _default_BT_ShowDate_Instance = new ShowDate(config.Id);
                    }

                    if (_default_BT_ShowDate_Instance.DateOfShow_ToSortBy > _Config.SHOWOFFSETDATE)
                    {
                        config.ValueX = "0";
                        config.Save();
                    }

                    return int.Parse(config.ValueX.Trim());
                }

                //default
                return 0;

            }
            set
            {
                //determine if the show is in the past
                if (value > 0)
                {
                    ShowDate sd = new ShowDate(value);
                    if (sd == null)
                        throw new ArgumentNullException("ShowDate is null at set default show date id.");
                }

                try
                {
                    //load from db
                    SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                        delegate(SiteConfig match)
                        {
                            return (match.Context.ToLower() == _Enums.SiteConfigContext.GlobalPrivate.ToString().ToLower() &&
                                match.Name.ToLower() == "default_bt_showdateid" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                        });

                    if (config == null)
                        throw new ArgumentNullException();

                    if (value.ToString() != config.ValueX)
                    {
                        config.ValueX = value.ToString();
                        config.Save();

                        _default_BT_ShowDate_Instance = null;
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }
        }

        #endregion 

        public static readonly string SPACERIMAGEPATH = "/assets/images/spacer.gif";

        public static bool SqlServerIsAvailable()
        {
            try
            {
                string[] sps = SubSonic.DataService.Provider.GetTableNameList();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        //ErrorLogging
        public static bool _ErrorLogToDB { get { return bool.Parse(ConfigurationManager.AppSettings["erlg_ErrorLogToDB"]); } }
        public static bool _ErrorLogToEventViewer { get { return bool.Parse(ConfigurationManager.AppSettings["erlg_ErrorLogToEventViewer"]); } }

        protected _Config(){ }

        /// <summary>
        /// Returns the date with an adjustment to turn the clock over at, say, 2AM. So the start of the day is 2AM and not midnight
        /// </summary>
        public static DateTime SHOWOFFSETDATE
        {
            get
            {
                return SHOWOFFSET_SET(DateTime.Now);//.AddHours(-_Config.DayTurnoverTime).Date;
            }
        }
        public static DateTime SHOWOFFSET_SET(DateTime date)
        {
            return date.AddHours(-_Config.DayTurnoverTime).Date;
        }

        public static bool ConfigTest()
        {
            bool result = true;

            //TestFlow();
            //TestImages();
            //TestPageMsg();
            //TestShip();
            //TestDefault();
            //TestEmail();
            //TestService();
            //TestAdmin();

            return result;
        }

        /// <summary>
        /// the hour at which everything will turn over to the next day - add this many hours
        /// </summary>
        public const int DayTurnoverTime = 3;

        
        #region Alt display properties
        
        public static string _GoogleAnalyticsId { get { return ConfigurationManager.AppSettings["stp_GoogleAnalyticsId"]; } }
        public static string _GoogleAPI_DeveloperKey { get { return ConfigurationManager.AppSettings["stp_GoogleAPI_DeveloperKey"]; } }

        #endregion

        #region CONSTANTS

        #region Service Exclusive

        public static bool svc_ServiceTestMode { get { return bool.Parse(ConfigurationManager.AppSettings["svc_ServiceTestMode"]); } }
        public static string svc_ServiceTestEmail { get { return ConfigurationManager.AppSettings["svc_ServiceTestEmail"]; } }
        public static string svc_ServiceTestFromName { get { return ConfigurationManager.AppSettings["svc_ServiceTestFromName"]; } }
        public static int svc_MaxThreads { get { return int.Parse(ConfigurationManager.AppSettings["svc_MaxThreads"]); } }
        public static bool svc_UseSqlDebug { get { return bool.Parse(ConfigurationManager.AppSettings["svc_UseSqlDebug"]); } }

        public static string svc_ServiceEmail { get { return ConfigurationManager.AppSettings["svc_ServiceEmail"]; } }
        public static string svc_ServiceFromName { get { return ConfigurationManager.AppSettings["svc_ServiceFromName"]; } }

        public static string svc_SitePhysicalAddress { get { return ConfigurationManager.AppSettings["svc_SitePhysicalAddress"]; } }
        public static string svc_WebmasterEmail { get { return ConfigurationManager.AppSettings["svc_WebmasterEmail"]; } }
        public static string svc_AbsoluteBadmailPath { get { return ConfigurationManager.AppSettings["_AbsoluteBadmailPath"]; } }
        public static string svc_MappedVirtualResourceDirectory { get { return ConfigurationManager.AppSettings["MappedVirtualResourceDirectory"]; } }

        /// <summary>
        /// Interval between rows PROCESSED
        /// </summary>
        public static int svc_JobIntervalMilliSeconds { get { return int.Parse(ConfigurationManager.AppSettings["svc_JobIntervalMilliSeconds"]); } }
        /// <summary>
        /// Interval between rows GATHERED
        /// </summary>
        public static int svc_PauseBetweenBatches { get { return int.Parse(ConfigurationManager.AppSettings["svc_PauseBetweenBatches"]); } }
        /// <summary>
        /// Number Of Rows to gather at a time
        /// </summary>
        public static int svc_BatchRetrievalSize { get { return int.Parse(ConfigurationManager.AppSettings["svc_BatchRetrievalSize"]); } }
        /// <summary>
        /// Number Of Days to leave processed mails in queue
        /// </summary>
        public static int svc_ArchiveAfterDays { get { return int.Parse(ConfigurationManager.AppSettings["svc_ArchiveAfterDays"]); } }
        
        #endregion

        private static Guid _appId = Guid.Empty;
        public static Guid APPLICATION_ID
        {
            get
            {
                if (_appId == Guid.Empty && _Config.APPLICATION_NAME != null)
                {
                    AspnetApplication app = new AspnetApplication();
                    app.LoadAndCloseReader(AspnetApplication.FetchByParameter("ApplicationName", _Config.APPLICATION_NAME));

                    if (app != null)
                        _appId = app.ApplicationId;
                }

                return _appId;
            }
        }

        public static string DSN { get { return System.Configuration.ConfigurationManager.ConnectionStrings["WillCallConnectionString"].ToString(); } }
       
        #endregion

        #region Flow

        private static void TestFlow()
        {
            string val;
            int idx;
            bool bbb;

            TimeSpan t = _Config._BoxOffice_TicketSales_Start;
            bbb = _Config._Display_Venue;
            bbb = _Config._Display_AllShowsInMenu;
            idx = _Config._Display_Next_N_Events;
            val = _Config._Display_Next_N_Events_Title;
            val = _Config._DisplayMenu_Title;
            bbb = _Config._Display_CalendarView;
            val = _Config._Display_CalendarView_Title;
            bbb = _Config._DisplayDatesAsRange;
            val = _Config._SiteTitle;
            val = _Config._ShowLinks_Header;
            bbb = _Config._DisplayMenu_IncludeBTEvents;
            val = _Config._URL_BTEventFeed;            
            bbb = _Enable_StereoPosterSplashPage;
            val = _Config._YouTubePlaylist;
            bbb = _Config._LogAgentRequests;
        }

        public static bool _LogAgentRequests
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                           match.Name.ToLower() == "log_agent_requests" && match.ValueX != null &&
                           Utils.Validation.IsBoolean(match.ValueX));

                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return bool.Parse(config.ValueX.Trim());
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Flow, _Enums.ConfigDataTypes._boolean, 5,
                        "Log_Agent_Requests",
                        "Meant to be used temporarily to track user-agents making requests. ",
                        "false");

                    return bool.Parse(config.ValueX);
                }
            }
        }

        public static string _URL_BTEventFeed
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "url_bteventfeed" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.Flow.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 256;
                    config.Name = "URL_BTEventFeed";
                    config.Description = "The url to the bt event feed";
                    config.ValueX = @"http://bouldertheater.com/admin-eventfeed";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }

        public static bool _DisplayMenu_IncludeBTEvents
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "displaymenu_includebtevents" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.Flow.ToString();
                    config.DataType = _Enums.ConfigDataTypes._boolean.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 5;
                    config.Name = "DisplayMenu_IncludeBTEvents";
                    config.Description = "A switch to include events pulled from the BT's RSS feed.";
                    config.ValueX = "false";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return bool.Parse(config.ValueX);
                }
            }
        }

        /// <summary>
        /// This does not atcually set a timezone - it just displays one
        /// </summary>
        public static string _TimeZone_Display
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "timezone_display" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.Flow.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 5;
                    config.Name = "TimeZone_Display";
                    config.Description = "This does not actually set a timezone. But rather it sets the value to display for timezone info";
                    config.ValueX = "MST";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }

        
        public static TimeSpan _DosTicket_SalesCutoff
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "dosticket_salescutoff" && match.ValueX != null &&
                            Utils.Validation.IsDecimal(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return TimeSpan.FromHours(double.Parse(config.ValueX.Trim()));

                return TimeSpan.FromHours(17);
            }
        }

        public static TimeSpan _BoxOffice_TicketSales_Start
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "boxoffice_ticketsales_start" && match.ValueX != null &&
                            Utils.Validation.IsDecimal(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return TimeSpan.FromHours(double.Parse(config.ValueX.Trim()));

                return TimeSpan.FromHours(8);
            }
        }
        public static bool _DisplayDatesAsRange
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "displaydatesasrange" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _Display_Venue
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_venue" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _Display_AllShowsInMenu
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_allshowsinmenu" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        /// <summary>
        /// note the backwards logic here due to the wording of WITHOUT
        /// </summary>
        public static bool _DisplayMenu_MonthsWithoutShows
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_menu_monthswithoutshows" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static int _Display_Next_N_Events
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_next_n_events" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 0;
            }
        }
        public static string _Display_Next_N_Events_Title
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_next_n_events_title" && match.ValueX != null);
                    });
                if (config != null)
                    return config.ValueX;

                return null;
            }
        }
        public static string _DisplayMenu_Title
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "displaymenu_title" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.Flow.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 50;
                    config.Name = "DisplayMenu_Title";
                    config.Description = "The title of the left hand side event menu.";
                    config.ValueX = "Upcoming Shows";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }
        public static string _SiteLogo
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "sitelogo" && match.ValueX != null);
                    });
                if (config != null)
                    return config.ValueX;

                return string.Empty;
            }
        }
        public static bool _Display_CalendarView
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_calendarview" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static string _Display_CalendarView_Title
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_calendarviewtitle" && match.ValueX != null);
                    });
                if (config != null)
                    return config.ValueX;

                return null;
            }
        }
        
        public static string _SiteTitle
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "sitetitle" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _ShowLinks_Header
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "containsshowlinks_header" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static bool _ExternalTicketUrl_OpensNewWindow
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "externalticketurl_opensnewwindow" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return true;
            }
        }
        
        public static bool _Enable_StereoPosterSplashPage
	        {
	            get
	            {
	                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
	                    delegate(SiteConfig match)
	                    {
	                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
	                            match.Name.ToLower() == "enablestereopostersplash" && match.ValueX != null &&
	                            Utils.Validation.IsBoolean(match.ValueX));
	                    });
	                if (config != null && config.Id > 0)
	                    return bool.Parse(config.ValueX);
	                else
	                {
	                    config = new SiteConfig();
	                    config.ApplicationId = _Config.APPLICATION_ID;
	                    config.Context = _Enums.SiteConfigContext.Flow.ToString();
	                    config.DataType = _Enums.ConfigDataTypes._boolean.ToString().TrimStart('_');
	                    //config.Description = 
	                    config.DtStamp = DateTime.Now;
	                    config.MaxLength = 5;
	                    config.Name = "EnableStereoPosterSplash";
	                    config.Description = "The master switch for the Stereo Poster splash page.";
	                    config.ValueX = "false";
	                    config.Save();
	
	                    _Lookits.SiteConfigs.Add(config);
	
	                    return bool.Parse(config.ValueX);
	                }
	            }
        }

        public static string _YouTubePlaylist
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "youtubeplaylist" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return config.ValueX.Trim();
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Images, _Enums.ConfigDataTypes._string, 500,
                        "YouTubePlaylist",
                        "A comma-separated list of playlists to include in the youtube player. ",
                        "PLemkhiE8QQpv6i_wyVpxmQZN7VAn0lruf");

                    return config.ValueX;
                }
            }
        }
        

        #endregion

        #region Images

        private static void TestImages()
        {
            int idx;
            //bool bbb;
            string val;

            val = _Config._BannerDimensionText;
            idx = _Config._ActThumbSizeSm;
            idx = _Config._ActThumbSizeLg;
            idx = _Config._ActThumbSizeMax;
            idx = _Config._VenueThumbSizeSm;
            idx = _Config._VenueThumbSizeLg;
            idx = _Config._VenueThumbSizeMax;
            idx = _Config._ShowThumbSizeSm;
            idx = _Config._ShowThumbSizeLg;
            idx = _Config._ShowThumbSizeMax;
            idx = _Config._PromoterThumbSizeSm;
            idx = _Config._PromoterThumbSizeLg;
            idx = _Config._PromoterThumbSizeMax; 
            idx = _Config._MP3_Player_DisplayWidth;
            idx = _Config._MP3_Player_DisplayHeight;
            val = _Config._DefaultShowImage;
        }
        
        public static string _DefaultShowImage
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "defaultshowimage" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return config.ValueX.Trim();
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Images, _Enums.ConfigDataTypes._string, 500,
                        "DefaultShowImage",
                        "The image to use as the default for shows. Specify from site root. aka /WillCallResources/Images or /assets/images",
                        "/WillCallResources/Images/Misc/foxdefault.png");

                    return config.ValueX;
                }
            }
        }

        public static string _BannerDimensionText
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "bannerdimensiontext" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static int _ActThumbSizeSm
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "act_thumbnail_size_small" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 75;
            }
        }
        public static int _ActThumbSizeLg
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "act_thumbnail_size_large" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 125;
            }
        }
        public static int _ActThumbSizeMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "act_thumbnail_size_max" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 175;
            }
        }
        public static int _ShowThumbSizeSm
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "show_thumbnail_size_small" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 75;
            }
        }
        public static int _ShowThumbSizeLg
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "show_thumbnail_size_large" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 125;
            }
        }
        public static int _ShowThumbSizeMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "show_thumbnail_size_max" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 300;
            }
        }
        public static int _PromoterThumbSizeSm
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "promoter_thumbnail_size_small" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 75;
            }
        }
        public static int _PromoterThumbSizeLg
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "promoter_thumbnail_size_large" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 125;
            }
        }
        public static int _PromoterThumbSizeMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "promoter_thumbnail_size_max" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 300;
            }
        }
        public static int _VenueThumbSizeSm
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "venue_thumbnail_size_small" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 75;
            }
        }
        public static int _VenueThumbSizeLg
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "venue_thumbnail_size_large" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 125;
            }
        }
        public static int _VenueThumbSizeMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "venue_thumbnail_size_max" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 175;
            }
        }
        public static int _MP3_Player_DisplayWidth
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "mp3_player_displaywidth" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 175;
            }
        }
        public static int _MP3_Player_DisplayHeight
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "mp3_player_displayheight" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 175;
            }
        }

        #endregion

        #region PageMsg

        private static void TestPageMsg()
        {
            string val;
            //int idx;
            //bool bbb;

            val = _Config._Message_ContactPage;
            val = _Config._Message_CreateNewAccount;
            val = _Config._Mailer_ControlTitle;
            val = _Config._Mailer_ControlGreeting;
            val = _Config._PageTitle_Header;
            val = _Config._Message_Goodwill;
        }
        
        public static string _TicketingText_Default
	        {
	            get
	            {
	                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
	                    delegate(SiteConfig match)
	                    {
	                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
	                            match.Name.ToLower() == "ticketingtext_default" && match.ValueX != null && match.ValueX.Trim().Length > 0);
	                    });                
	                if (config != null && config.Id > 0)
	                    return config.ValueX.Trim();
	                else
	                {
	                    config = new SiteConfig();
	                    config.ApplicationId = _Config.APPLICATION_ID;
	                    config.Context = _Enums.SiteConfigContext.PageMsg.ToString();
	                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
	                    //config.Description = 
	                    config.DtStamp = DateTime.Now;
	                    config.MaxLength = 5000;
	                    config.Name = "TicketingText_Default";
	                    config.Description = "This will show under all shows as a further description for ticket information.";
	                    config.ValueX = "Additional Service Fees May Apply";
	                    config.Save();
	
	                    _Lookits.SiteConfigs.Add(config);
	
	                    return config.ValueX;
	                }
	            }
	        }
	        public static string _TicketingText_AllAges
	        {
	            get
	            {
	                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
	                    delegate(SiteConfig match)
	                    {
	                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
	                            match.Name.ToLower() == "ticketingtext_allages" && match.ValueX != null && match.ValueX.Trim().Length > 0);
	                    });
	                if (config != null && config.Id > 0)
	                    return config.ValueX.Trim();
	                else
	                {
	                    config = new SiteConfig();
	                    config.ApplicationId = _Config.APPLICATION_ID;
	                    config.Context = _Enums.SiteConfigContext.PageMsg.ToString();
	                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
	                    //config.Description = 
	                    config.DtStamp = DateTime.Now;
	                    config.MaxLength = 5000;
	                    config.Name = "TicketingText_AllAges";
	                    config.Description = "This will show under ALLAGES shows as a further description for ticket information.";
	                    config.ValueX = "All 21+ Tickets MUST be accompanied by a Valid Photo ID.<br/>All Ages is 12+.";
	                    config.Save();
	
	                    _Lookits.SiteConfigs.Add(config);
	
	                    return config.ValueX;
	                }
	            }
	        }
	        public static string _TicketingText_TwentyOnePlus
	        {
	            get
	            {
	                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
	                    delegate(SiteConfig match)
	                    {
	                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
	                            match.Name.ToLower() == "ticketingtext_twentyoneplus" && match.ValueX != null && match.ValueX.Trim().Length > 0);
	                    });
	                if (config != null && config.Id > 0)
	                    return config.ValueX.Trim();
	                else
	                {
	                    config = new SiteConfig();
	                    config.ApplicationId = _Config.APPLICATION_ID;
	                    config.Context = _Enums.SiteConfigContext.PageMsg.ToString();
	                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
	                    //config.Description = 
	                    config.DtStamp = DateTime.Now;
	                    config.MaxLength = 5000;
	                    config.Name = "TicketingText_TwentyOnePlus";
	                    config.Description = "This will show under 21+ shows as a further description for ticket information.";
	                    config.ValueX = "All 21+ Tickets MUST be accompanied by a Valid Photo ID. No Exceptions.";
	                    config.Save();
	
	                    _Lookits.SiteConfigs.Add(config);
	
	                    return config.ValueX;
	                }
	            }
        }
        
        public static string _PageTitle_Header
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "pagetitle_header" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static string _Message_Goodwill
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_goodwill" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static string _Message_ContactPage
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_contactpage" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Message_CreateNewAccount
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_createnewaccount" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Mailer_ControlTitle
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "mailer_controltitle" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Mailer_ControlGreeting
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "mailer_controlgreeting" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }

        #endregion

        #region Default

        private static void TestDefault()
        {
            string val;
            int idx;
            //decimal dec;
            //bool bbb;

            Age a = _Config._Default_Age;
            val = _Config._Default_PreVenueText;
            val = _Config._Default_VenueName;
            val = _Config._Default_ActName;
            val = _Config._Default_CountryCode;
            val = _Config._MainOffice_Phone;
            val = _Config._BoxOffice_Phone;
            val = _Config._Site_Keywords;
            val = _Config._Site_Description;
            idx = _Config._JustAnnouncedWindow_Days;
        }

        public static int _JustAnnouncedWindow_Days
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "justannouncedwindow_days" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.Default.ToString();
                    config.DataType = _Enums.ConfigDataTypes._int.ToString().TrimStart('_');
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 50;
                    config.Name = "JustAnnouncedWindow_Days";
                    config.Description = "The number of days to include just announced shows. 14 days = shows announced in the last two weeks";
                    config.ValueX = @"14";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return int.Parse(config.ValueX);
                }
            }
        }

        public static string _Site_Keywords
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "site_keywords" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX != null)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Default, _Enums.ConfigDataTypes._string, 500,
                        "Site_Keywords",
                        "Sets the value of the meta keywords tag",
                        string.Empty);

                    return config.ValueX;
                }
            }
        }

        public static string _Site_Description
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "site_description" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX != null)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Default, _Enums.ConfigDataTypes._string, 300,
                        "Site_Description",
                        "Sets the value of the meta description tag. Please limit to 160 chars or less.",
                        string.Empty);

                    return config.ValueX;
                }
            }
        }

        public static Age _Default_Age
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_age" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return (Age)_Lookits.Ages.GetList().Find(delegate(Age match) { return (match.Name.ToLower() == config.ValueX.Trim().ToLower()); });

                Age defaultAge = (Age)_Lookits.Ages.GetList().Find(delegate(Age match) { return (match.Name.ToLower() == "all ages"); });
                if (defaultAge == null)
                    throw new Exception("Default Age could not be found");

                return defaultAge;
            }
        }
        public static string _Default_PreVenueText
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_prevenuetext" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MailerTemplate_ShowEvent_DateFormat
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "mlrtplt_showevent_dateformat" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MailerTemplate_ShowLinearEvent_DateFormat
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "mlrtplt_showlinearevent_dateformat" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        /// <summary>
        /// Defaults To empty string
        /// </summary>
        public static string _Default_VenueName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_venuename" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Default_ActName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_actname" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Default_CountryCode
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_countrycode" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
       
        public static string _MainOffice_Phone
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "mainoffice_phone" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _BoxOffice_Phone
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "boxoffice_phone" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static bool _BannerOrder_Random
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "bannerorder_random" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static int _BannerDisplayTime
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "bannerdisplaytime" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 3200;
            }
        }

        public static int _KioskDisplayTime
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "kioskdisplaytime" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Default, _Enums.ConfigDataTypes._int, 25,
                        "KioskDisplayTime",
                        "The default milliseconds to display a kiosk. ",
                        "12000");

                    return int.Parse(config.ValueX);
                }
            }
        }

        #endregion

        #region Email

        private static void TestEmail()
        {
            string val;
            //int idx;
            //decimal dec;
            //bool bbb;

            val = _Config._CustomerService_Email;
            val = _Config._CustomerService_FromName;
            val = _Config._MassMailService_Email;
            val = _Config._MassMailService_FromName;
            val = _Config._MailerCorrespondence_Email;
            val = _Config._MailerCorrespondence_FromName;
            val = _Config._SiteDirector_Prepend_Url;
        }

        public static string _SiteDirector_Prepend_Url
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "sitedirector_prependurl" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.Email.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 50;
                    config.Name = "SiteDirector_PrependUrl";
                    config.Description = "For mailer links, specifies the link to the SD.aspx page that will handle links";
                    config.ValueX = @"http://z2ent.com/sd.aspx?url=";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }

        public static string _CustomerInquiryTemplate { get { return ConfigurationManager.AppSettings["stp_CustomerInquiryTemplate"]; } }
        public static string _ForgotPasswordTemplate { get { return ConfigurationManager.AppSettings["stp_ForgotPasswordTemplate"]; } }
        
        public static string _CustomerService_Email
        {
            get
            {
                try
                {
                    SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                        delegate(SiteConfig match)
                        {
                            return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                                match.Name.ToLower() == "customerservice_email" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                        });
                    if (config != null && config.Id > 0)
                        return config.ValueX.Trim();
                }
                catch(Exception)
                {
                    string appConfigVal = _Config.svc_ServiceEmail ?? string.Empty;
                    return appConfigVal;
                }

                return string.Empty;
            }
        }
        public static string _CustomerService_FromName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "customerservice_fromname" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }                    
        public static string _MailerCorrespondence_Email
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "mailercorrespondence_email" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MailerCorrespondence_FromName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "mailercorrespondence_fromname" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MassMailService_Email
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "massmailservice_email" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MassMailService_FromName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "massmailservice_fromname" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }

        #endregion

        #region Admin

        private static void TestAdmin()
        {
            string val;
            //int idx;
            //decimal dec;
            bool bbb;

            bbb = _Config._FAQ_Page_On;
            bbb = _Config._TwitterWidget_On;
            bbb = _Config._TwitterModule_On;
            //bbb = _Config._PremailerApiOn;
            //val = _Config._PremailerApiUrl;
            bbb = _Config._AllowCustomerInitiatedNameChanges;
            bbb = _Config._MaintenanceMode_On;
            bbb = _Config._SubscriptionsActive;
            val = _Config._CC_DeveloperEmail;
            _Enums.SiteEntityMode md = _Config._Site_Entity_Mode;
            val = _Config._Site_Entity_Name;
            val = _Config._Site_Entity_HomePage;
            val = _Config._Site_Entity_PhysicalAddress;
            val = _Config._Site_Entity_WebmasterEmail;            
            val = _Config._Admin_EmailAddress;
            val = _Config._Merchant_ChargeStatement_Descriptor;
        }

        //public static bool _PremailerApiOn
        //{
        //    get
        //    {
        //        SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
        //            delegate(SiteConfig match)
        //            {
        //                return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
        //                    match.Name.ToLower() == "premailerapi_on" && match.ValueX != null &&
        //                    Utils.Validation.IsBoolean(match.ValueX));
        //            });
        //        if (config != null && config.Id > 0)
        //            return bool.Parse(config.ValueX);
        //        else
        //        {
        //            config = _Config.AddNewConfig(_Enums.SiteConfigContext.Admin, _Enums.ConfigDataTypes._boolean, 5,
        //                "PremailerAPI_On",
        //                "The premailer API prvides functions for mailer preparation.",
        //                "true");

        //            return bool.Parse(config.ValueX);
        //        }
        //    }
        //}
        //public static string _PremailerApiUrl
        //{
        //    get
        //    {
        //        SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
        //            delegate(SiteConfig match)
        //            {
        //                return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
        //                    match.Name.ToLower() == "premailerapi_url" && match.ValueX != null);
        //            });
        //        if (config != null && config.Id > 0 && config.ValueX != null)
        //            return config.ValueX;
        //        else
        //        {
        //            config = _Config.AddNewConfig(_Enums.SiteConfigContext.Admin, _Enums.ConfigDataTypes._string, 500,
        //                "PremailerAPI_Url",
        //                "http://premailer.dialect.ca/api/0.1/documents",
        //                string.Empty);

        //            return config.ValueX;
        //        }
        //    }
        //}
        public static bool _SubscriptionsActive
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "subscriptions_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Admin, _Enums.ConfigDataTypes._boolean, 5,
                        "Subscriptions_Active",
                        "Toggles the activation of subscriptions/newsletters.",
                        "true");

                    return bool.Parse(config.ValueX);
                }
            }
        }

        public static bool _AllowCustomerInitiatedNameChanges
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "allowcustomerinitiatednamechanges" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _MaintenanceMode_On
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "maintenancemode_on" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return true;//if it aint found - dont use 
            }
        }
        public static bool _FAQ_Page_On
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "faq_page_on" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _TwitterWidget_On
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "twitterwidget_on" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _TwitterModule_On
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "twittermodule_on" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static string _CC_DeveloperEmail
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "cc_developer_email" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static _Enums.SiteEntityMode _Site_Entity_Mode
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "site_entity_mode" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return (_Enums.SiteEntityMode)Enum.Parse(typeof(_Enums.SiteEntityMode), config.ValueX, true);

                throw new Exception("mode not found");
            }
        }
        public static string _Site_Entity_Name
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "site_entity_name" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static string _Site_Entity_HomePage
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "site_entity_homepage" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static string _Site_Entity_PhysicalAddress
        {
            get
            {
                try
                {
                    SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                        delegate(SiteConfig match)
                        {
                            return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                                match.Name.ToLower() == "site_entity_physicaladdress" && match.ValueX != null);
                        });
                    if (config != null && config.Id > 0)
                        return config.ValueX;
                }
                catch(Exception)
                {
                    string appConfigVal = _Config.svc_SitePhysicalAddress ?? string.Empty;
                    return appConfigVal;
                }

                return String.Empty;
            }
        }
        public static string _Site_Entity_WebmasterEmail
        {
            get
            {
               try
                {
                    SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                        delegate(SiteConfig match)
                        {
                            return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                                match.Name.ToLower() == "site_entity_webmasteremail" && match.ValueX != null);
                        });
                    if (config != null && config.Id > 0)
                        return config.ValueX;
                }
                catch(Exception)
                {
                    string appConfigVal = _Config.svc_WebmasterEmail ?? string.Empty;
                    return appConfigVal;
                }

                return String.Empty;
            }
        }
        public static string _Admin_EmailAddress
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "admin_emailaddress" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static string _Merchant_ChargeStatement_Descriptor
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "merchant_chargestatement_descriptor" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }

        #endregion

        #region SITE AND DEVELOPMENT

        public static DateTime  _ApplicationStartDate { get { return DateTime.Parse(ConfigurationManager.AppSettings["stp_ApplicationStartDate"]); } }
        
        public static string    APPLICATION_NAME { get { return ConfigurationManager.AppSettings["stp_ApplicationName"]; } }
        public static string    _applicationName { get { return _Config.APPLICATION_NAME; } }//keep for legacy purposes

        public static string    _DomainName { get { return ConfigurationManager.AppSettings["stp_DomainName"]; } }
        public static int       _DataExpiryMins { get { return int.Parse(ConfigurationManager.AppSettings["stp_DataExpiryMins"]); } }
        public static bool      _AutoLogin { get { return bool.Parse(ConfigurationManager.AppSettings["stp_AutoLogin"]); } }
        public static string    _AutoLoginName { get { return ConfigurationManager.AppSettings["stp_AutoLoginName"]; } }
        public static string    _AutoLoginPass { get { return ConfigurationManager.AppSettings["stp_AutoLoginPass"]; } }
        public static string    _ErrorLogPath { get { return ConfigurationManager.AppSettings["stp_ErrorLogPath"]; } }
        public static string    _ErrorLogTitle { get { return ConfigurationManager.AppSettings["stp_ErrorLogTitle"]; } }
        public static bool      _ErrorsToDebugger { get { return _Config._DomainName.ToLower() == "localhost" || _Config._DomainName.ToLower().IndexOf("local.") != -1; } }
        public static bool      _LogRenderError { get { return bool.Parse(ConfigurationManager.AppSettings["stp_LogRenderError"]); } }		
        public static string    _MappedRootDirectory { get { return ConfigurationManager.AppSettings["stp_MappedRootDirectory"]; } }
        public static string    _VirtualResourceDir { get { return ConfigurationManager.AppSettings["stp_VirtualResourceDir"]; } }
        
        /// <summary>
        /// returns the mapped file - string is logs/dependencyfiles/filename.txt
        /// </summary>
        public static string    _MappedShowDependencyFile { get { 
            string dep = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["stp_ShowDependencyFile"]); 
            return dep; } }

        public static string    _ThemeFolder { get { return ConfigurationManager.AppSettings["stp_ThemeFolder"]; } }

        #endregion

        #region MESSAGES

        public static string _ErrorPageMessage { get { return ConfigurationManager.AppSettings["stp_ErrorPageMessage"]; } }
        
        #endregion

        #region IMAGES & MP3s

        private static string Image_UI_Path { get { return string.Format("/{0}/Images/UI/", _Config._VirtualResourceDir); } }
        public static string _ActImageStorage_Local { get { return string.Format("/{0}/Images/Acts/", _Config._VirtualResourceDir); } }
        public static string _VenueImageStorage_Local { get { return string.Format("/{0}/Images/Venues/", _Config._VirtualResourceDir); } }
        public static string _PromoterImageStorage_Local { get { return string.Format("/{0}/Images/Promoters/", _Config._VirtualResourceDir); } }
        public static string _ShowImageStorage_Local { get { return string.Format("/{0}/Images/Shows/", _Config._VirtualResourceDir); } }
        public static string _EmailerImageStorage_Local { get { return string.Format("/{0}/Images/Emailer/", _Config._VirtualResourceDir); } }
        public static string _UploadImageStorage_Local { get { return string.Format("/{0}/Images/Uploads/", _Config._VirtualResourceDir); } }
        public static string _AdvertImageStorage_Local { get { return string.Format("/{0}/Images/Adverts/", _Config._VirtualResourceDir); } }

        #endregion
    }
}
