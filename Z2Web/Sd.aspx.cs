using System;
using System.Text;

namespace z2Main
{
    /// <summary>
    /// The Sd stands for "Site Director". This page is simply a redirect page. It will redirect to the url specified in the querystring. 
    /// It is used for logging/tracking requests. 
    /// IIS logging will handle the actual logging
    /// url - the url to redirect to
    /// seid - the subscriptionEmail Id
    /// No need to qualify ssl as this is just a redirector
    /// </summary>
    public partial class Sd : System.Web.UI.Page
    {   

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get segment data
            string url = Request.QueryString["url"];
            
            if (url == null) {
                Response.Redirect("/");
            }   
            else
            {
                string[] urlString = Request.RawUrl.Split(new string[] { "url=" }, StringSplitOptions.RemoveEmptyEntries);

                //0 will  be the sd.aspx?
                //1 contains our url
                url = urlString[1];

                //it is possible that a link arrives here without the proper formatting
                //ensure that the first & ampersand is converted to a ?
                //any other ?'s should be converted to &
                StringBuilder sb = new StringBuilder();
                string[] parts = url.Replace("&&", "&").Replace("??","?").Replace("&?", "&").Replace("?&", "&").Split(new char[] { '?', '&' });
                int len = parts.Length;
                for (int i = 0; i < len; i++)
                {
                    sb.Append(parts[i]);

                    if(i == 0 && len > 1)
                        sb.Append("?");
                    else if(i > 0 && (i < len-1))
                        sb.Append("&");
                }

                string formattedUrl = sb.ToString();

                //if the url begins with www add http
                if (formattedUrl.ToLower().StartsWith("www."))
                    formattedUrl = string.Format("http://{0}", formattedUrl);

                Response.Redirect(formattedUrl);
            }
        }
    }
}
