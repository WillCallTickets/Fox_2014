using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace wctMain.Controller.Api
{
    public class SearchesController : ApiController
    {
        // GET api/<controller>/<query>
        /// <summary>
        /// Generic search that handles slashes - pretty much returns nothing on a date search
        /// </summary>
        /// <returns></returns>
        public List<object> Get()
        {
            //clean up the query - this handles slashes in the input
            //only deal with the {*pathinfo}
            char delimiter = '/';
            string pathinfo = this.ControllerContext.RouteData.Values["pathinfo"].ToString();
            string[] tokens = pathinfo.Trim(delimiter).Split(delimiter);


            //we just need to separate the actual query from the entire string
            string query = string.Join(delimiter.ToString(), tokens, 0, tokens.Length - 2);
            int limit = int.Parse(tokens[tokens.Length-2]);
            int pagenum = int.Parse(tokens[tokens.Length - 1]);

            string accessLevel = this.ControllerContext.RouteData.Values["accessLevel"].ToString();
            return wctMain.Service.TypeaheadService.GetTypeahead(accessLevel, "Act", query, limit, pagenum);
        }

        //// GET api/<controller>/<query>/<limit>/<pagenum>
        ///// <summary>
        ///// Handles act searches coming from the website (not admin) search control
        ///// </summary>
        ///// <param name="query"></param>
        ///// <param name="limit"></param>
        ///// <param name="pagenum"></param>
        ///// <returns></returns>
        //public List<object> Get(string query, int limit, int pagenum)
        //{
        //    string accessLevel = this.ControllerContext.RouteData.Values["accessLevel"].ToString();
        //    return wctMain.Service.TypeaheadService.GetTypeahead("act", query, limit, pagenum);
        //}

        /// <summary>
        /// handles searches tied to an admin control - with context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="query"></param>
        /// <param name="limit"></param>
        /// <param name="pagenum"></param>
        /// <returns></returns>
        public List<object> Get(string context, string query, int limit, int pagenum)
        {
            string accessLevel = this.ControllerContext.RouteData.Values["accessLevel"].ToString();

            return wctMain.Service.TypeaheadService.GetTypeahead(accessLevel, context, query, limit, pagenum);
        }
    }
}