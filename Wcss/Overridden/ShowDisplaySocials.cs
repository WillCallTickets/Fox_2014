using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Wcss
{
    public class ShowDisplaySocials
    {
        object _showObject = null;
        string _url = null;
        string _urlFull = null;
        string _shortSubject = null;
        string _subject = null;
        string _twitterAccount = null;
        string _location = null;
        string _sprop = null;
        string _acts = null;
        string _dateVenue = null;
        string _facebookEventUrl = null;
        /// <summary>
        /// Date Acts @ Venue
        /// </summary>
        string _showDescription = null;
        string _universalStart = null;
        string _universalEnd = null;

        //list images
        string imgFacebook = "<img src=\"/assets/images/social/Facebook-64.png\" border=\"0\" alt=\"RSVP on Facebook\" />";
        //string imgTwitter = "Post to Twitter";
        //string imgGooglePlus = "handled by google";//included for completeness        
        string imgEmail = "<img src=\"/assets/images/social/flatemail-64.png\" border=\"0\" alt=\"Email your friends aobut this show.\" />";
        string imgiCal = "<img src=\"/assets/images/social/flat-ical-64.png\" border=\"0\" alt=\"Add this event to your iCal or outlook.\" />";
        string imgGoogleCalendar = "<img src=\"/assets/images/social/google-calendar-64.png\" border=\"0\" alt=\"Add this event to your Google calendar.\" />";

        //string imgEmailMlr = "<img src=\"/assets/images/social/flatemail-64.png\" border=\"0\" alt=\"Email your friends aobut this show.\" />";
        //string imgiCalMlr = "<img src=\"/assets/images/social/flat-ical-64.png\" border=\"0\" alt=\"Add this event to your iCal or outlook.\" />";
        //string imgGoogleCalendarMlr = "<img src=\"/assets/images/social/google-calendar-64.png\" border=\"0\" alt=\"Add this event to your Google calendar.\" />";

        public ShowDisplaySocials(Show s)
        {
            _showObject = s;
            Show _show = (Wcss.Show)_showObject;
            ShowDate _sd = _show.FirstShowDate;

            _url = _sd.ConfiguredUrl;
            _urlFull = string.Format("http://{0}/{1}", _Config._DomainName, _sd.ConfiguredUrl);
            _universalStart = _show.FirstShowDate.DateOfShow.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");
            _universalEnd = _show.FirstShowDate.DateOfShow.AddHours(5).ToUniversalTime().ToString("yyyyMMddTHHmmssZ");
            _shortSubject = _show.Display.DisplaySocial_Subject.ToUpper();
            _subject = string.Format("{0} {1}", _show.FirstShowDate.DateOfShow.ToString("MMM dd, yyyy"), _shortSubject);
            _showDescription = //Nov 04, 2013 TYCHO with BEACON @ THE FOX THEATRE BOULDER, CO
                _show.Display.DisplaySocial_Description;

            _twitterAccount = "foxtheatreco";
            _location = "The%20Fox%20Theatre%20-%20Boulder%2C%20CO";
            _sprop = "The%20Fox%20Theatre&sprop=name:foxtheatre.com";
            _acts = _show.Display.DisplaySocial_Acts;
            _dateVenue = _show.Display.DisplaySocial_DateAndVenue;
            _facebookEventUrl = s.FacebookEventUrl;
        }

        public ShowDisplaySocials(MailerShow ms)
        {
            _showObject = ms;
            MailerShow _show = (Wcss.MailerShow)_showObject;
            DateTime _showDate = Utils.ParseHelper.JavascriptDate_To_DateTime(ms.SortDate);

            _urlFull = _show.SiteUrl;
            int idx = _urlFull.ToLower().IndexOf(".com/");
            if(idx != -1)
            {
                string tmp = _urlFull.Substring(idx + 5);
                _url = tmp;
            }

            _universalStart = _showDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");
            _universalEnd = _showDate.AddHours(5).ToUniversalTime().ToString("yyyyMMddTHHmmssZ");

            _shortSubject = Utils.ParseHelper.DoubleSpaceToSingle(string.Format("{0} @ {1}",
                ms.Headliner,
                ms.Venue.Replace("-", " ")
                ));
            _subject = Utils.ParseHelper.DoubleSpaceToSingle(string.Format("{0} {1}",
                _showDate.ToString("MMM dd, yyyy"), _subject));

            string open = (ms.Opener != null) ? Wcss.ShowDisplay.RemoveSpecialGuest(ms.Opener).Trim() : string.Empty;
            _acts = Utils.ParseHelper.DoubleSpaceToSingle(string.Format("{0} {1}",
                ms.Headliner, (open.Length > 0) ? open : string.Empty));

            //_showDescription = //Nov 04, 2013 TYCHO with BEACON @ THE FOX THEATRE BOULDER, CO
            _showDescription = string.Format("{0} {1} @ {2}",
                _showDate.ToString("MMM dd, yyyy"), _acts, ms.Venue.Replace("-", " "));


            if (_show.Venue.IndexOf("Boulder Theater", StringComparison.OrdinalIgnoreCase) != -1)
            {
                _twitterAccount = "BoulderTheater";
                _location = "Boulder%20Theater%20-%20Boulder%2C%20CO";
                _sprop = "Boulder%20Theater&sprop=name:bouldertheater.com";
            }
            else if (_show.Venue.IndexOf("Fox Theatre", StringComparison.OrdinalIgnoreCase) != -1)
            {
                _twitterAccount = "foxtheatreco";
                _location = "The%20Fox%20Theatre%20-%20Boulder%2C%20CO";
                _sprop = "The%20Fox%20Theatre&sprop=name:foxtheatre.com";
                _url = _urlFull.ToLower().Replace("https://", string.Empty).Replace("http://", string.Empty).Replace("www.foxtheatre.com/", string.Empty).Replace("foxtheatre.com/", string.Empty);
            }
            else 
            {
                _twitterAccount = null;
                _location = HttpUtility.UrlEncode(ms.Venue);
                _sprop = _location;
            }

            _dateVenue = string.Format("{0} @ {1}",
                _showDate.ToString("MMM dd, yyyy"), ms.Venue).ToUpper();

            _facebookEventUrl = ms.FacebookEventUrl;
        }

        /// <summary>
        /// This is only for MailerShow objects
        /// </summary>
        /// <returns></returns>
        public string SocialOutputMailer()
        {
            StringBuilder sb = new StringBuilder();

            if (_showObject != null)
            {
                //twitter - facebook and ical

                //facebook
                if (this._facebookEventUrl != null && this._facebookEventUrl.Trim().Length > 0)
                {
                    sb.Append("<div class=\"mlr-social-facebk\">");
                    sb.AppendLine();
                    sb.AppendFormat("<a href=\"{0}\" target=\"_blank\" title=\"{1}\">{2}</a>", _facebookEventUrl, _subject, imgFacebook);
                    sb.AppendLine();
                    sb.Append("</div>");
                    sb.AppendLine();
                }

                //twitter
                //sb.Append("<div class=\"show-social twtr\">");
                //sb.AppendLine();
                //sb.AppendFormat("<a href=\"https://twitter.com/share\" title=\"Post to Twitter\" target=\"_blank\" data-count=\"none\" data-size=\"medium\" data-text=\"\" class=\"twitter-share-button\" data-url=\"{0}\" data-text=\"{1}\" data-via=\"{4}\"{3}>{2}</a>",
                //    _urlFull,
                //    _showDescription,
                //    "",
                //    " target=\"_blank\"",
                //    _twitterAccount);
                //sb.AppendLine();
                //sb.Append("</div>");
                //sb.AppendLine();


                //ical - handled by controller      
                //no need for blank target - simple http request for a file
                sb.Append("<div class=\"mlr-social-ical\">");
                sb.AppendFormat("<a href=\"{0}/iCal/mailershow?mlrshow={1}\" title=\"Save this event to iCal or Outlook\" >{2}</a>", 
                    string.Format("http://{0}", _Config._DomainName),
                    _urlFull, imgiCal);
                sb.AppendLine("</div>");
                sb.AppendLine();
                //end ical


                //google calendar - will not work as intended unless times are specified
                sb.Append("<div class=\"mlr-social-gcal\">");
                sb.AppendFormat("<a title=\"Save this event to Google Calendar\" href=\"http://www.google.com/calendar/event?action=TEMPLATE&text={0}", 
                    HttpUtility.UrlEncode(_shortSubject));
                sb.AppendFormat("&dates={0}/{1}", _universalStart, _universalEnd);
                sb.AppendFormat("&details={0}",
                    //string.Format("{0}%0A{1}%0A{2}%0A*{3}*",
                    //    HttpUtility.UrlEncode(_dateVenue),
                    string.Format("{0}%0A{1}%0A*{2}",                        
                        HttpUtility.UrlEncode(_acts),
                        _urlFull,
                        HttpUtility.UrlEncode("end time approximate"))
                    );
                sb.AppendFormat("&location={0}&trp=false&sprop={1}\" ", _location, _sprop);
                sb.AppendFormat("target=\"_blank\">{0}</a>", imgGoogleCalendar);
                sb.AppendLine("</div>");
                sb.AppendLine();

            }

            return sb.ToString();

        }

        /// <summary>
        /// minimized version is twitter and facebook only
        /// </summary>
        /// <param name="socialNetworksOnly">ignore mailto, ical and google cal</param>
        /// <param name="buttonSize">small or large - default is small. Facebook button is set. For google, Tall is wider</param>
        /// <param name="blankTarget">experimental - trying to see if this will help on the phone display issues and click area</param>
        /// <returns></returns>
        public string SocialOutput(bool targetMobile, string buttonSize, bool blankTarget)
        {
            bool facebook   = true;
            bool twitter = true;// (targetMobile) ? false : true;
            bool googleplus = true;
            bool email = (targetMobile) ? false : true;
            bool ical = (targetMobile) ? false : true;
            bool gcal = (targetMobile) ? false : true;


            if(buttonSize == null)
                buttonSize = "small";


            StringBuilder sb = new StringBuilder();
            if (_showObject != null)
            {

                if (facebook)
                {
                    //*******************
                    //facebook
                    sb.Append("<div class=\"show-social facebk\">");
                    sb.AppendLine();

                    //url
                    //sb.AppendFormat("<a href=\"http://www.facebook.com/plugins/like.php?href={0}&width=90&layout=button_count&action=like&show_faces=false", 
                    //    HttpUtility.UrlEncode(_urlFull));
                    //sb.AppendLine("&share=false&height=21&appId=172208982825393\" >fb</a>");

                    //iframe
                    //sb.AppendFormat("<iframe src=\"//www.facebook.com/plugins/like.php?href={0}&amp;width=90&amp;layout=button_count&amp;action=like&amp;", HttpUtility.UrlEncode(_urlFull));
                    //sb.AppendLine("show_faces=false&amp;share=false&amp;height=21&amp;appId=172208982825393\" ");
                    //sb.AppendLine("scrolling=\"no\" frameborder=\"0\" style=\"border:none; overflow:hidden; width:90px; height:21px;\" allowTransparency=\"true\"></iframe>");

                    //xfbml
                    //sb.AppendFormat("<fb:like href=\"{0}\" ", _urlFull);
                    //sb.AppendLine("width=\"90\" layout=\"button_count\" action=\"like\" show_faces=\"false\" share=\"false\"></fb:like>");

                    //html5 - not sure it will work with one page
                    //sb.AppendFormat("<div class=\"fb-like\" data-href=\"{0}\" data-width=\"90\" data-height=\"20\"  ", 
                    sb.AppendFormat("<div class=\"fb-like\" data-href=\"{0}\" ", _urlFull);
                    //button_count = horiz layout
                    sb.AppendLine("data-layout=\"standard\" data-action=\"like\" data-show-faces=\"false\" data-share=\"false\"></div>");
                    sb.AppendLine();

                    ////https://developers.facebook.com/docs/plugins/share-button/
                    //sb.AppendFormat("<a href=\"#\" onclick=\"window.open('https://www.facebook.com/sharer/sharer.php?u={0}',", _urlFull);
                    //sb.AppendLine("'facebook-share-dialog','width=626,height=436'); return false;\">Share on Facebook</a>");

                    sb.Append("</div>");
                    sb.AppendLine();
                }//end facebook

                if (twitter && _twitterAccount != null && _twitterAccount.Trim().Length > 0)
                {
                    //*******************
                    //twitter
                    sb.Append("<div class=\"show-social twtr\">");
                    sb.AppendLine();
                    sb.AppendFormat("<a href=\"https://twitter.com/share\" title=\"Post to Twitter\" target=\"_blank\" data-count=\"none\" data-size=\"medium\" data-text=\"\" class=\"twitter-share-button\" data-url=\"{0}\" data-text=\"{1}\" data-via=\"{4}\"{3}>{2}</a>",
                        _urlFull,
                        _showDescription,
                        "",
                        (blankTarget) ? " target=\"_blank\"" : string.Empty,
                        _twitterAccount);
                    sb.AppendLine();
                    sb.Append("</div>");
                    sb.AppendLine();
                }//end twitter

                if (googleplus)
                {
                    //*******************
                    //google+
                    // *** This may not work on localhost correctly
                    sb.Append("<div class=\"show-social g-plus\">");
                    sb.AppendLine();

                    //without bubbles
                    //medium - 32w  20h
                    //standard - 38w height 24
                    //tall - 50w x 20h
                    sb.AppendFormat("<div class=\"g-plusone\" data-size=\"{0}\" data-annotation=\"none\" data-href=\"{1}\"></div>",
                        (buttonSize.ToLower() == "small") ? "medium" : "tall",
                        _urlFull);
                    //showDescription is scraped from page
                    sb.AppendLine();
                    sb.Append("</div>");
                    sb.AppendLine();
                }//end googleplus


                if (email || (ical && _url != null) || gcal)
                {
                    sb.Append("<div class=\"social-contact\">");
                    sb.AppendLine();
                }

                if(email){
                    //*******************
                    //email
                    Utils.MailtoHelper mail = new Utils.MailtoHelper(
                        string.Empty,
                        _subject,
                        string.Format("{1}{0}{0}{2}",
                        Environment.NewLine,
                        _showDescription,
                        _urlFull),
                        imgEmail);
                    sb.Append("<div class=\"social-img ml-mlr\">");
                    sb.AppendLine();
                    sb.Append(mail.FormattedMailtoLink);
                    sb.AppendLine();
                    sb.Append("</div>");
                    sb.AppendLine();
                }//end email

                if(ical && _url != null) {
                    //*******************
                    //ical - handled by controller      
                    sb.Append("<div class=\"social-img ical\">");
                    sb.AppendFormat("<a href=\"/iCal/{0}\" title=\"Save this event to iCal or Outlook\" >{1}</a>", _url, imgiCal);
                    sb.AppendLine("</div>");
                    sb.AppendLine();
                }//end ical

                if(gcal) {
                    //*******************
                    //google calendar - will not work as intended unless times are specified
                    sb.Append("<div class=\"social-img gcal\">");
                    sb.AppendFormat("<a title=\"Save this event to Google Calendar\" href=\"http://www.google.com/calendar/event?action=TEMPLATE&text={0}", 
                        HttpUtility.UrlEncode(_shortSubject));
                    sb.AppendFormat("&dates={0}/{1}", _universalStart, _universalEnd);
                    //sb.AppendFormat("&details={0}",
                    //    HttpUtility.UrlEncode(string.Format("{0} - {1}", _acts, _dateVenue)));
                    sb.AppendFormat("&details={0}",
                        string.Format("{0}%0A{1}%0A{2}%0A*{3}*",
                            HttpUtility.UrlEncode(_dateVenue),
                            HttpUtility.UrlEncode(_acts),                            
                            _urlFull,
                            HttpUtility.UrlEncode("end time approximate"))
                        );
                    sb.AppendFormat("&location={0}&trp=false&sprop={1}\"", _location, _sprop);
                    sb.AppendFormat("target=\"_blank\">{0}</a>", imgGoogleCalendar);
                    sb.AppendLine("</div>");
                    sb.AppendLine();
                }//end gcal


                if(email || ical || gcal) {
                    sb.Append("</div>");//end social contact
                    sb.AppendLine();
                }

            }

            return sb.ToString();

        }
    }
}
