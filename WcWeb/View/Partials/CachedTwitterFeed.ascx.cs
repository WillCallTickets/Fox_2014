using System;
using System.Web.UI;
using System.Linq;
using System.Text.RegularExpressions;

using System.Collections.Generic;
using Spring.Social.Twitter.Api;
using Spring.Social.Twitter.Api.Impl;

//<%@ OutputCache Duration="500" VaryByParam="None" %> 
/* JUNE 2013
 * 
 * http://www.springframework.net/social-twitter/
 * 
 * 
 */

namespace wctMain.View.Partials
{
    public partial class CachedTwitterFeed : UserControl
    {
        public string Account { get; set; }
        public int Tweets { get; set; }
        public int CacheTTL { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div id=\"tweet-wrapper\" class=\"main-inner\">");
            writer.Write("<div id=\"tweet-container\">");

            writer.Write("<ul class=\"list-unstyled\">");

            try
            {   
                foreach (var t in GetTweets().Take(Tweets))
                {
                    string s = Regex.Replace(
                        t.Text,//HttpUtility.HtmlEncode(t),
                        @"[a-z]+://[^\s]+",
                        x => "<a href=\'" + x.Value.Replace("'", "\"") + "\'>" + x.Value + "</a>",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase
                        );

                    writer.Write("<li>{0}</li>", s);
                }
            }
            catch (Exception ex)
            {
                Wcss._Error.LogException(ex);
                writer.Write("<li>Twitter service unavailable</li>");
            }

            writer.Write("</ul>");

            writer.Write("</div>");//tweet container            
            writer.Write("</div>");//tweet-wrapper

        }
        
        public IList<Tweet> GetTweets()
        {
            string TwitterConsumerKey = "S1ATyuPwpHCa4SzrLdVw";
            string TwitterConsumerSecret = "ZcjsnXTVj5Nck16fkvd4YBwZmwHX4gSkvbkRF1w4cm4";
            string TwitterAccessToken = "39596522-VpIG55Utv0Ys6wafP0jWNBSuxLTG45Q3cuuHfk5Ig";
            string TwitterAccessTokenSecret = "LgbT19sDjzicfyCJaMMJOBSaJdk94sR2USzWQvYnA";
            
            try
            {
                ITwitter twitter = new TwitterTemplate(TwitterConsumerKey, TwitterConsumerSecret, TwitterAccessToken, TwitterAccessTokenSecret);
                IList<Tweet> tweets = twitter.TimelineOperations.GetUserTimelineAsync().Result;

                return tweets;
            }
            catch (Exception)
            {
                //if (ex.Message.ToLower().IndexOf("one or more errors occurred") != -1)
                //    Wcss._Error.LogToFile(
                //        string.Format("{0}{1}{2}",
                //            ex.Message,
                //            Environment.NewLine,
                //            ex.StackTrace), 
                //        string.Format(
                //            "TwitterFeed_{0}", 
                //            DateTime.Now.ToString("MM_dd_yyyy")));
                //else
                //    Wcss._Error.LogException(ex);
            }

            return null;
        }
    }
}