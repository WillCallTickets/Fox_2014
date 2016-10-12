using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
//using System.Web.Mvc;

using Wcss;

namespace z2Main.Controller.Api
{
    /// <summary>
    /// Handles legacy urls that could not be handled in routing and redirects to the correct controller
    /// </summary>
    public class SdController : ApiController
    {
        // GET api/<controller>/<query>
        /// <summary>
        /// link is just a placeholder - not setup to do anything
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string link)
        {
            string linked = Request.RequestUri.Query.TrimStart(new char[] { '?' });
            
            
            if(linked != null && linked.Trim().Length > 0)
            {

                //make sure we have a protocol specified - deafult to http
                if (linked.ToLower().IndexOf("http://") == -1 || linked.ToLower().IndexOf("https://") == -1)
                    linked = string.Format("http://{0}", linked.TrimStart(new char[] { '/' } ));

                Uri redirect = new Uri(linked);
                
                var response = Request.CreateResponse(HttpStatusCode.Redirect);
                response.Headers.Location = redirect;
                return response;
            }

            var responseNotFound = Request.CreateResponse(HttpStatusCode.NotFound);
            responseNotFound.Headers.Location = new Uri(string.Format("{0}://{1}/Error.aspx",
                this.Request.RequestUri.Scheme, this.Request.RequestUri.Host));
            return responseNotFound;

        }
    }
}