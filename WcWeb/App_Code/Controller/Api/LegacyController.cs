using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
//using System.Web.Mvc;

using Wcss;

namespace wctMain.Controller.Api
{
    /// <summary>
    /// Handles legacy urls that could not be handled in routing and redirects to the correct controller
    /// </summary>
    public class LegacyController : ApiController
    {
        // GET api/<controller>/<query>
        public HttpResponseMessage Get(int year, int month, int day, string time, string eventtitle)
        {
            //see showView for logic
            MainContext ctx = new MainContext();
            string converted = ShowDate.Url_Dashed_ConvertFromSlashed(this.Request.RequestUri.PathAndQuery);
            Show s = ctx.GetCurrentShowByUrl(converted, false, true);

            if (s != null)
            {
                var response = Request.CreateResponse(HttpStatusCode.MovedPermanently);
                response.Headers.Location = new Uri(string.Format("{0}://{1}/{2}",
                    this.Request.RequestUri.Scheme, this.Request.RequestUri.Host, s.FirstShowDate.ConfiguredUrl));
                return response;
            }

            var responseNotFound = Request.CreateResponse(HttpStatusCode.NotFound);
            responseNotFound.Headers.Location = new Uri(string.Format("{0}://{1}/Error.aspx",
                this.Request.RequestUri.Scheme, this.Request.RequestUri.Host));
            return responseNotFound;
        }

        public HttpResponseMessage Get()
        {
            string intention = this.Request.RequestUri.PathAndQuery.TrimStart('/').ToLower();
            
            var response = Request.CreateResponse(HttpStatusCode.NotFound);

            if (intention.IndexOf("index2.aspx") != -1) 
            {

                _Error.LogToFile(string.Format("{0}: {1} referral: {2} useragent: {3}", 
                    DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"), 
                    intention,
                    (Request.Headers.Referrer != null) ? Request.Headers.Referrer.ToString() : "no referrer",
                    (Request.Headers.UserAgent != null) ? Request.Headers.UserAgent.ToString() : "no agent"),

                string.Format("Index2Requests_{0}", DateTime.Now.ToString("MM_dd_yyyy")));

                response = Request.CreateResponse(HttpStatusCode.MovedPermanently);

                MainContext ctx = new MainContext();
                string query = this.Request.RequestUri.Query.TrimStart('?');
                string[] pairs = query.Split('&');
                int idx = 0;

                try
                {
                    foreach (string s in pairs)
                    {
                        if (s.ToLower().IndexOf("showdisplayid") != -1)
                        {
                            string idPart = s.Split('=')[1];
                            idx = int.Parse(idPart);
                            break;
                        }
                    }
                }
                catch (Exception ex) {
                    _Error.LogException(ex);
                }

                if (idx != 0)
                {
                    Show s = ctx.GetCurrentShowById(idx, true);
                    if (s != null && s.Id >= 10000)
                    {
                        response.Headers.Location = new Uri(string.Format("{0}://{1}/{2}",
                           this.Request.RequestUri.Scheme, this.Request.RequestUri.Host, s.FirstShowDate.ConfiguredUrl));

                        return response;
                    }
                }

                response.Headers.Location = new Uri(string.Format("{0}://{1}/",
                    this.Request.RequestUri.Scheme, this.Request.RequestUri.Host));
            }
            else if (intention.IndexOf("event.aspx") != -1 || intention.IndexOf("store/") != -1)
            {
                response.StatusCode = HttpStatusCode.MovedPermanently;
                response.Headers.Location = new Uri(string.Format("{0}://{1}",
                    this.Request.RequestUri.Scheme, this.Request.RequestUri.Host));
            }

            return response;
        }
    }
}