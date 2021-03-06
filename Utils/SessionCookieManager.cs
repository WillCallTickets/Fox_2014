using System;
using System.Web;

namespace Utils
{
	/// <summary>
	/// Summary description for cookies.
	/// </summary>
	public class SessionCookieManager
	{
		protected System.Web.HttpResponse response;
        protected System.Web.HttpRequest request;

        public SessionCookieManager() 
        {
        }

		public SessionCookieManager(System.Web.HttpResponse oResponse)
		{
			response = oResponse;
		}

		public static void ClearCookies()
		{
			HttpContext.Current.Request.Cookies.Clear();
			HttpContext.Current.Response.Cookies.Clear();			
		}

		protected string getCookie(string sCookieSet, string sCookieName)
		{
			HttpCookie c = HttpContext.Current.Request.Cookies[sCookieSet];
			if (c == null)
				return "";
			else
				return c.Values[sCookieName];
		}

        public string getCookie(string sCookieName)
        {
            HttpCookie c = HttpContext.Current.Request.Cookies[sCookieName];
            if (c == null)
                return string.Empty;
            
            return c.Value;
        }

        protected string getInitCookieVal(string cooKey)
        {
            string val = this.getCookie(cooKey);
            if (val == null || val.Trim().Length == 0)
            {
                this.setPersistentCookie(cooKey, string.Empty);
                return (string.Empty);
            }

            return val;
        }

		protected void setCookie(string sCookieSet, string sCookieName, string sCookieValue)
		{
			HttpCookie c = HttpContext.Current.Response.Cookies[sCookieSet];
			if (c == null)
			{
				c = new HttpCookie(sCookieSet);
				HttpContext.Current.Response.Cookies.Add(c);
			}
			
			c.Values.Remove(sCookieName);
			c.Values.Add(sCookieName,sCookieValue);
			

			HttpCookie d = HttpContext.Current.Request.Cookies[sCookieSet];
			if (d == null)
			{
				d = new HttpCookie(sCookieSet);
				HttpContext.Current.Request.Cookies.Add(c);
			}
			
			d.Values.Remove(sCookieName);
			d.Values.Add(sCookieName,sCookieValue);
		}

        /// <summary>
        /// session cookie
        /// </summary>
        /// <param name="sCookieName"></param>
        /// <param name="sCookieValue"></param>
        protected void setCookie(string sCookieName, string sCookieValue)
        {
            HttpCookie c = HttpContext.Current.Response.Cookies[sCookieName];
            if (c == null)
            {
                c = new HttpCookie(sCookieName);
                HttpContext.Current.Response.Cookies.Add(c);
            }

            c.Value = sCookieValue;

            HttpCookie d = HttpContext.Current.Request.Cookies[sCookieName];
            if (d == null)
            {
                d = new HttpCookie(sCookieName);
                HttpContext.Current.Request.Cookies.Add(c);
            }

            d.Value = sCookieValue;
        }

        protected void setPersistentCookie(string sCookieName, string sCookieValue)
        {
            HttpCookie c = HttpContext.Current.Response.Cookies[sCookieName];
            if (c == null)
            {
                c = new HttpCookie(sCookieName);
                c.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(c);
            }

            c.Value = sCookieValue;
            if(c.Expires == DateTime.MinValue)
                c.Expires = DateTime.Now.AddYears(1);

            HttpCookie d = HttpContext.Current.Request.Cookies[sCookieName];
            if (d == null)
            {
                d = new HttpCookie(sCookieName);
                d.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Request.Cookies.Add(c);
            }

            d.Value = sCookieValue;
            if (d.Expires == DateTime.MinValue)
                d.Expires = DateTime.Now.AddYears(1);
        }

        protected void setExpirableCookie(string sCookieName, string sCookieValue, TimeSpan timeFromNow)
        {
            HttpCookie c = HttpContext.Current.Response.Cookies[sCookieName];
            if (c == null)
            {
                c = new HttpCookie(sCookieName);
                c.Expires = DateTime.Now.Add(timeFromNow);
                HttpContext.Current.Response.Cookies.Add(c);
            }

            c.Value = sCookieValue;
            if (c.Expires == DateTime.MinValue)
                c.Expires = DateTime.Now.Add(timeFromNow);

            HttpCookie d = HttpContext.Current.Request.Cookies[sCookieName];
            if (d == null)
            {
                d = new HttpCookie(sCookieName);
                d.Expires = DateTime.Now.Add(timeFromNow);
                HttpContext.Current.Request.Cookies.Add(c);
            }

            d.Value = sCookieValue;
            if (d.Expires == DateTime.MinValue)
                d.Expires = DateTime.Now.Add(timeFromNow);
        }
	}
}


