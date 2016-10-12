using System;
using System.Configuration;

namespace z2Main.Controller
{
    /// <summary>
    /// Summary description for Z2Config
    /// </summary>
    public partial class Z2Config
    {
        public Z2Config() {}

        //FROM WEB.CONFIG

        public static string APPLICATION_NAME { get { return ConfigurationManager.AppSettings["stp_ApplicationName"].ToString(); } }
        public static string GOOGLE_ANALYTICS_ID { get { return ConfigurationManager.AppSettings["stp_GoogleAnalyticsId"].ToString(); } }
        public static string DOMAIN_NAME { get { return ConfigurationManager.AppSettings["stp_DomainName"].ToString(); } }
        public static DateTime APPLICATION_START_DATE { get { return DateTime.Parse(ConfigurationManager.AppSettings["stp_ApplicationStartDate"].ToString()); } }
        public static string ERROR_LOG_PATH { get { return ConfigurationManager.AppSettings["stp_ErrorLogPath"].ToString(); } }
        public static string VIRTUAL_RESOURCE_DIR { get { return ConfigurationManager.AppSettings["stp_VirtualResourceDir"].ToString(); } }
        public static string VIRTUAL_BANNER_DIR { get { return ConfigurationManager.AppSettings["stp_VirtualBannerDir"].ToString(); } }

        /// <summary>
        /// returns the mapped file - string is logs/dependencyfiles/filename.txt
        /// </summary>
        public static string MAPPED_SHOW_DEPENDENCY_FILE { get { return System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["stp_ShowDependencyFile"].ToString()); } }


        public static string CUSTOMERSERVICEEMAIL_Z2
        {
            get
            {
                return "customerservice@z2ent.com";
            }
        }

        public static string CUSTOMERSERVICEFROMNAME_Z2
        {
            get
            {
                return "Z2 Entertainment Customer Service";
            }
        }
    }
}