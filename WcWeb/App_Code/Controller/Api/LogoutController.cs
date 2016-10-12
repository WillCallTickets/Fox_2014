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
    /// Handles legacy urls that coudl not be handled in routing and redirects to the correct controller
    /// </summary>
    public class LogoutController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string intention = this.Request.RequestUri.PathAndQuery.TrimStart('/').ToLower().Replace("api/", string.Empty);
            var response = Request.CreateResponse(HttpStatusCode.NotFound);

            switch (intention)
            {
                case "logout":
                    
                    System.Web.Security.FormsAuthentication.SignOut();

                    response.StatusCode = HttpStatusCode.Redirect;
                    response.Headers.Location = new Uri(string.Format("{0}://{1}", 
                        this.Request.RequestUri.Scheme, this.Request.RequestUri.Host));
                    break;
            }

            return response;
        }
    }
}