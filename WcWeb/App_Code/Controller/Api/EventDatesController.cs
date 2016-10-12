using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using wctMain.Model;

namespace wctMain.Controller.Api
{
    public class EventDatesController : ApiController
    {
        // GET api/<controller>/<query>
        public List<EventDate> Get()
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("**USER** {0} {1} {2} accessed", DateTime.Now.ToLongTimeString(), this.ToString(), "Get()"));

            MainContext _ctx = new MainContext();

            List<EventDate> list = new List<EventDate>();
            list.AddRange(MainContext.ConvertShowDatesToEventDates(_ctx.ShowRepo_Web_Displayable));

            return list;
        }
    }
}