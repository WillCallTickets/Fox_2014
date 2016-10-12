using System;
using System.Configuration;
using System.Collections.Generic;

namespace Wcss
{
    public partial class _Config
    {
        public static void TestFacebook()
        {
            string val;
            int idx;
            bool bbb;

            val = _Config._FacebookIntegration_App_AdminList;
            val = _Config._FacebookIntegration_App_ApiKey;
            val = _Config._FacebookIntegration_App_Id;
            val = _Config._FacebookIntegration_App_Name;
            val = _Config._FacebookIntegration_App_Secret;
            val = _Config._FacebookIntegration_App_Url;
            val = _Config._FacebookIntegration_Like_Action;
            bbb = _Config._FacebookIntegration_Like_Active;
            val = _Config._FacebookIntegration_Like_ColorScheme;
            idx = _Config._FacebookIntegration_Like_Height;
            val = _Config._FacebookIntegration_Like_Layout;
            bbb = _Config._FacebookIntegration_Like_RenderAsIFrame;
            bbb = _Config._FacebookIntegration_Like_ShowFaces;
            idx = _Config._FacebookIntegration_Like_Width;
        }

        #region Like Button

        public static bool _FacebookIntegration_Like_Active
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._boolean.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 5;
                    config.Name = "FacebookIntegration_Like_Active";
                    config.Description = "The master switch for allowing the facebook like button.";
                    config.ValueX = "false";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return bool.Parse(config.ValueX);
                }
            }
        }
        public static bool _FacebookIntegration_Like_RenderAsIFrame
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_renderasiframe" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._boolean.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 5;
                    config.Name = "FacebookIntegration_Like_RenderAsIFrame";
                    config.Description = "Toggles the like control from an IFrame to an XFBML implementation. XFBML does not allow hiding comments";
                    config.ValueX = "false";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return bool.Parse(config.ValueX);
                }
            }
        }
        public static string _FacebookIntegration_Like_Layout
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_layout" && match.ValueX != null );
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 50;
                    config.Name = "FacebookIntegration_Like_Layout";
                    config.Description = "Valid values are standard (minwidth: 225 - default 450x35 - 450x80 with photos), button_count (minwidth: 90 - default 90x20) and box_count (minwidth: 55 - default 55x65).";
                    config.ValueX = "standard";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }
        public static string _FacebookIntegration_Like_Action
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_action" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 50;
                    config.Name = "FacebookIntegration_Like_Action";
                    config.Description = "Valid values are like or recommend";
                    config.ValueX = "like";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }
        public static string _FacebookIntegration_Like_ColorScheme
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_colorscheme" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 50;
                    config.Name = "FacebookIntegration_Like_ColorScheme";
                    config.Description = "Valid values are light or dark";
                    config.ValueX = "dark";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }
        public static bool _FacebookIntegration_Like_ShowFaces
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_showfaces" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._boolean.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 5;
                    config.Name = "FacebookIntegration_Like_ShowFaces";
                    config.Description = "Toggles the like control to show users pictures";
                    config.ValueX = "false";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return bool.Parse(config.ValueX);
                }
            }
        }
        public static int _FacebookIntegration_Like_Width
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_width" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._int.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 10;
                    config.Name = "FacebookIntegration_Like_Width";
                    config.Description = "The width of the control. 399 turns off comments in the IFrame implementation. See docs for layout dimensions.";
                    config.ValueX = "399";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return int.Parse(config.ValueX);
                }
            }
        }
        public static int _FacebookIntegration_Like_Height
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_height" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._int.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 10;
                    config.Name = "FacebookIntegration_Like_Height";
                    config.Description = "The height of the control. See docs for layout dimensions.";
                    config.ValueX = "42";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return int.Parse(config.ValueX);
                }
            }
        }

        #endregion

        #region App settings

        public static string _FacebookIntegration_App_Id
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_id" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 50;
                    config.Name = "FacebookIntegration_App_Id";
                    config.Description = "The application id.";
                    config.ValueX = "172208982825393";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }

        public static string _FacebookIntegration_App_Secret
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_secret" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 50;
                    config.Name = "FacebookIntegration_App_Secret";
                    config.Description = "The application secret.";
                    config.ValueX = "3994e4a343f796435f472066329c217d";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }

        public static string _FacebookIntegration_App_Name
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_name" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 256;
                    config.Name = "FacebookIntegration_App_Name";
                    config.Description = "The registered application name.";
                    config.ValueX = "The Fox Theatre - Boulder, CO";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }

        public static string _FacebookIntegration_App_Url
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_url" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 256;
                    config.Name = "FacebookIntegration_App_Url";
                    config.Description = "The registered application's url.";
                    config.ValueX = @"http://www.foxtheatre.com/";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }

        public static string _FacebookIntegration_App_ApiKey
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_apikey" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 50;
                    config.Name = "FacebookIntegration_App_ApiKey";
                    config.Description = "The api key.";
                    config.ValueX = "ced4e5d786e14b3652c771e4ffda7e1a";
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }

        public static string _FacebookIntegration_App_AdminList
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_adminlist" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = new SiteConfig();
                    config.ApplicationId = _Config.APPLICATION_ID;
                    config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
                    config.DataType = _Enums.ConfigDataTypes._string.ToString().TrimStart('_');
                    //config.Description = 
                    config.DtStamp = DateTime.Now;
                    config.MaxLength = 1024;
                    config.Name = "FacebookIntegration_App_AdminList";
                    config.Description = "The list of admin ids - comma separated.";
                    config.ValueX = "100000624633880"; //100000624633880 is my rk profile
                    config.Save();

                    _Lookits.SiteConfigs.Add(config);

                    return config.ValueX;
                }
            }
        }


        #endregion
    }
}
