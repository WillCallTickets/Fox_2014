using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
//using System.Web.Mvc;
using System.IO;
using System.Text;

using System.Runtime.Serialization;

using Wcss;
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;

namespace wctMain.Controller.Api
{
    /// <summary>
    /// SUMMARY: Headliner @ Venue
    /// DESCRIPTION: Date, Acts @ Venue
    /// EVENT START: universal time
    /// </summary>
    public class iCalController : ApiController
    {
        iCalendar icalendar;

        // GET api/<controller>/<query>
        public HttpResponseMessage Get(string calItemLink)
        {
            //http://stackoverflow.com/questions/12145390/how-to-set-downloading-file-name-in-asp-net-mvc-web-api
            //see utils.fileloader.

            //see showView for logic
            MainContext ctx = new MainContext();            
            iCalendarSerializer serializer = new iCalendarSerializer();

            if (calItemLink.ToLower().IndexOf("mailershow") == -1)
            {
                Show s = ctx.GetCurrentShowByUrl(calItemLink, false, false);

                if (s != null)
                {
                    CreateCalendarIcs(s);
                    string filename = string.Format("{0}_{1}.ics", s.FirstShowDate.DateOfShow_ToSortBy.ToString("yyyyMMdd"), s.GetShowMainActPart);
                    Utils.FileLoader.WriteDownloadToContext(serializer.SerializeToString(icalendar), string.Format("Filename={0}", filename));

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    return response;
                }
            }
            else //try to find an exiting entry
            {
                string query = Request.RequestUri.Query.TrimStart(new char[] { '?' });
                string[] pieces = query.Split(new char[] { '&' });
                string url = null;
                foreach (string p in pieces)
                {
                    string[] parts = p.Split(new char[] { '=' });
                    if (parts.Length == 2 && parts[0] == "mlrshow")
                    {
                        url = parts[1];
                        break;
                    }
                }

                if (url != null)
                {
                    Wcss.ICalendar ICAL = new Wcss.ICalendar("UrlKey", url);
                    if (ICAL != null && ICAL.Id >= 10000)
                    {
                        string filename = string.Format("{0}.ics",
                            ICAL.UrlKey.ToLower().Replace("https://", string.Empty).Replace("http://", string.Empty).Replace("/", "_"));

                        Utils.FileLoader.WriteDownloadToContext(ICAL.SerializedCalendar, string.Format("Filename={0}", filename));

                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        return response;
                    }
                }
            }

            var responseNotFound = Request.CreateResponse(HttpStatusCode.NotFound);
            responseNotFound.Headers.Location = new Uri(string.Format("{0}://{1}/Error.aspx",
                this.Request.RequestUri.Scheme, this.Request.RequestUri.Host));
            return responseNotFound;
        }

        public HttpResponseMessage Get()
        {
            var responseNotFound = Request.CreateResponse(HttpStatusCode.NotFound);
            responseNotFound.Headers.Location = new Uri(string.Format("{0}://{1}/Error.aspx",
                this.Request.RequestUri.Scheme, this.Request.RequestUri.Host));
            return responseNotFound;
        }

        ///**************************************************
        /// Stick with mac osx format \n for newlines - who uses outlook?
        ///**************************************************
        
        
        /// <summary>
        /// Created when mailershow is imported
        /// </summary>
        /// <param name="ms"></param>
        public static void CreateCalendarIcs(MailerShow ms)
        {
            DateTime startTime = Utils.ParseHelper.JavascriptDate_To_DateTime(ms.SortDate);
            string _acts = string.Format("{0} {1}", ms.Headliner, ms.Opener).Trim();

            //create the ical
            iCalendar cal = CreateICalEvent(
                ms.SiteUrl, 
                ms.Venue, 
                startTime,
                ms.Headliner,
                string.Format("{0} @ {1}\\n{2}", startTime.ToString("MMM dd yyyy"), ms.Venue, _acts));
            
            //determine if we have an existing entry in the db and save
            Wcss.ICalendar ICAL = new Wcss.ICalendar("UrlKey", ms.SiteUrl);

            if (ICAL == null || ICAL.Id < 10000)
            {
                ICAL = new Wcss.ICalendar();
                ICAL.UrlKey = ms.SiteUrl;
            }

            iCalendarSerializer serializer = new iCalendarSerializer();
            ICAL.SerializedCalendar = serializer.SerializeToString(cal);

            ICAL.Save();
        }

        /*** Internal Methods ***/
        private void CreateCalendarIcs(Show _show)
        {
            ShowDate sd = _show.FirstShowDate;
            DateTime startTime = sd.DateOfShow_ToSortBy;
            string _acts = string.Format("{0} {1}", _show.listHeadliners, _show.listOpeners).Trim();

            //create the ical
            icalendar = CreateICalEvent(
                sd.ConfiguredUrl_withDomain,
                _show.DisplayVenueName,
                startTime,
                _show.GetShowMainActPart,
                string.Format("{0} @ {1}\\n{2}", startTime.ToString("MMM dd yyyy"), _show.DisplayVenueName, _acts));

            //outlook does not like the recurrence pattern
            //RecurrencePattern rp = new RecurrencePattern(FrequencyType.None);
            //evt.RecurrenceRules.Add(rp);
        }

        private static iCalendar CreateICalEvent(string url_full, string venue, DateTime startTime, string summary, string description)
        {
            //create the ical
            iCalendar ical = new iCalendar();
            ical.Name = "VCALENDAR";
            ical.Method = "PUBLISH";
            ical.Version = "2.0";
            //ical.AddLocalTimeZone();//when this is added - event is all day instead of in slot

            //create the ical event
            Event evt = ical.Create<Event>();
            evt.UID = url_full;
            if (url_full != null && url_full.Trim().Length > 0)
            {
                try
                {
                    evt.Url = new Uri(url_full);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }
            evt.Location = Utils.ParseHelper.DoubleSpaceToSingle(venue.Replace("-", " ").Replace(",", " ")).Trim().ToUpper();
            evt.Start = (iCalDateTime)startTime.ToUniversalTime();
            evt.End = (iCalDateTime)startTime.AddHours(5).ToUniversalTime();
            evt.Summary = Utils.ParseHelper.DoubleSpaceToSingle(
                summary.Replace("-", " ").Replace(",", " ")).Trim().ToUpper();
            evt.Description = string.Format("{0}\\n*end time approximate *", 
                Utils.ParseHelper.DoubleSpaceToSingle(description.Replace("-", " ").Replace(",", " "))).Trim();

            return ical;
        }
    }
}