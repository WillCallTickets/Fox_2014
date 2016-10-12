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

	public partial class _PrincipalConfig
    {
        public _Enums.Principal Principal;

        public _PrincipalConfig(_Enums.Principal principal)
        {
            if (principal == _Enums.Principal.all)
                throw new ArgumentOutOfRangeException("Cannot use all principal - requires a specific principal");

            this.Principal = principal;
        }


        /// <summary>        
        /// This does not refresh collections
        /// </summary>
        public static PrincipalConfig AddNew(
            _Enums.Principal principal,
            _Enums.SiteConfigContext context, 
            _Enums.ConfigDataTypes datatype, 
            int maxlength,
            string configname, 
            string description, 
            string defaultvalue)
        {
            if (principal == _Enums.Principal.all)
                throw new ArgumentOutOfRangeException("Cannot use all principal - use siteConfig for sitewide configs");

            PrincipalConfig config = new PrincipalConfig();
            config.Principal = principal;
            config.Context = context.ToString();
            config.DataType = datatype.ToString().TrimStart('_');
            config.DtStamp = DateTime.Now;
            config.MaxLength = maxlength;
            config.Name = configname;
            config.Description = description;
            config.ValueX = defaultvalue;
            config.Save();
            
            return config;
        }

    }
}
