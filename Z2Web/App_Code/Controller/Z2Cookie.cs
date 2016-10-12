using System;
using System.ComponentModel.DataAnnotations;

using Wcss;

namespace z2Main.Controller
{
    /// <summary>
    /// Summary description for SessionContext.
    /// </summary>
    public partial class Z2Cookie : Utils.SessionCookieManager
    {
        public Z2Cookie() : base () { }

        #region Registered Cookies

        public string MarketingProgram
        {
            get
            {
                return base.getInitCookieVal(_Enums.GetDescription(_Enums.CookEnums.Z2Cookies.SessionCookies.MarketingProgramKey));
            }
            set
            {
                base.setPersistentCookie(_Enums.GetDescription(_Enums.CookEnums.Z2Cookies.SessionCookies.MarketingProgramKey), value);
            }
        }

        #endregion
    }
}