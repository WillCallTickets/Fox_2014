using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Utils
{
    public class UrlHelper
    {
        #region Static Props

        /// <summary>
        /// Alpha Numeric and special chars "$-_.+!*'(),"
        /// Used for character pool for naming Acts, Promoters, Venues
        /// per http://www.rfc-editor.org/rfc/rfc1738.txt
        /// </summary>
        public static Regex regexUrlAllowableCharsPerRfc = new Regex(@"^[a-zA-Z0-9\s\$\-_\.\+\!\*\'\(\)\,]+$", RegexOptions.IgnoreCase);
        public static Regex anchors = new Regex(@"(?<linkStart> (SRC|HREF)\s*=\s*[""']?)(?<linkProper>[^#'"">]+?)(?<linkEnd>[ '""]+?)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        public static Regex linksonly = new Regex(@"(?<linkStart> (HREF)\s*=\s*[""']?)(?<linkProper>[^#'"">]+?)(?<linkEnd>[ '""]+?)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        #endregion

        #region Static Methods

        /// <summary>
        /// Indictaes if the url is AOK with RFC standards
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAllowableAlphaOrderableName(string input)
        {
            Match match = Regex.Match(input, regexUrlAllowableCharsPerRfc.ToString());

            return match.Success;
        }

        /// <summary>
        /// https://support.google.com/analytics/answer/1033867?hl=en&ref_topic=1032998
        /// </summary>
        public static string BuildGoogleCampaignQS(string campaignName)
        {
            return BuildGoogleCampaignQS("Z2", "email", campaignName);
        }

        /// <summary>
        /// Allow for later changes to indicate diff sources and mediums
        /// </summary>
        private static string BuildGoogleCampaignQS(string source, string medium, string campaignName)
        {
            return string.Format("utm_source={0}&utm_medium={1}&utm_campaign={2}",
                System.Web.HttpUtility.HtmlEncode(source),
                System.Web.HttpUtility.HtmlEncode(medium),
                System.Web.HttpUtility.HtmlEncode(campaignName));
        }

        

        #endregion

        #region Member Props

        private string _prependUrl { get; set; }
        private string _appendToUrl { get; set; }
        private string _removeFromUrl { get; set; }

        public string DefaultDomainName { get; set; }
        public string EmailLetterName { get; set; }
        public string GoogleCampaignQs
        {
            get
            {
                if (EmailLetterName != null && EmailLetterName.Trim().Length > 0)
                    return BuildGoogleCampaignQS(EmailLetterName);

                return null;
            }
        }

        #endregion

        #region Constructors

        public UrlHelper( string _defaultDomainName )
        {
            DefaultDomainName = _defaultDomainName;
        }

        public UrlHelper(string _defaultDomainName, string _emailLetterName)
        {
            DefaultDomainName = _defaultDomainName;
            EmailLetterName = _emailLetterName;
        }

        #endregion

        #region Member Methods

        public string EnsureAbsoluteUrls(string txt)
        {
            string result = anchors.Replace(txt, new MatchEvaluator(RegexMatch_EnsureAbsoluteUrl));
            return result;
        }

        /// <summary>
        /// Url helper must be created with a domain name that equals the link we would like to redirect to
        /// That url should be in the format http://z2ent.com/Sd.aspx?url=
        /// </summary>
        /// <param name="content"></param>
        /// <param name="prependUrl"></param>
        /// <returns></returns>
        public string PrependToUrl(string content, string urlToPrepend)
        {
            _prependUrl = urlToPrepend;
            return linksonly.Replace(content, new MatchEvaluator(RegexMatch_PrependToUrl));
        }

        /// <summary>
        /// add content to end
        /// </summary>
        /// <param name="content"></param>
        /// <param name="urlToPrepend"></param>
        /// <returns></returns>
        public string AppendToUrl(string content, string appendToUrl)
        {
            _appendToUrl = appendToUrl;
            return linksonly.Replace(content, new MatchEvaluator(RegexMatch_AppendToUrl));
        }

        public string RemoveFromUrl(string content, string urlPartToRemove)
        {
            _removeFromUrl = urlPartToRemove;
            return linksonly.Replace(content, new MatchEvaluator(RegexMatch_RemoveTextFromUrl));
        }

        private string RegexMatch_EnsureAbsoluteUrl(Match m)
        {
            //we have start and end because the quoting could be different
            string start = m.Groups["linkStart"].Value;
            string link = m.Groups["linkProper"].Value.TrimEnd('.');
            string end = (m.Groups["linkEnd"] != null) ? m.Groups["linkEnd"].Value : string.Empty;

            //ignore mailtos
            if (link.IndexOf("mailto", StringComparison.OrdinalIgnoreCase) == -1)
            {
                Uri result = null;

                //if we are simply missing the protocol/scheme then try it with http
                //this is how we avoid adding our config Domain to an external link
                //if (link.IndexOf("http") == -1)//work for https too
                //    Uri.TryCreate(string.Format("http://{0}", link), UriKind.Absolute, out result);

                if (result == null)
                {
                    if (!Uri.TryCreate(link, UriKind.Absolute, out result))
                    {
                        bool ext = false;

                        //try to determine if it is an external link without a protocol
                        //if the path has a dot [com, net, org, edu] slash in it
                        link = link.TrimStart(new char[] { '/' });
                        string[] parts = link.Split('/');
                        if (parts.Length > 0 && parts[0].IndexOf(".") != -1)
                        {
                            Match match = Regex.Match(parts[0], @"\.(com|org|net|edu)$", RegexOptions.IgnoreCase);

                            if (match.Success)
                                ext = true;
                        }

                        Uri.TryCreate(
                            string.Format("http://{0}{1}",
                                (ext) ? string.Empty : string.Format("{0}/", DefaultDomainName),
                                link),
                            UriKind.Absolute, out result);
                    }
                }

                if (result != null)
                {
                    //if the host does not contain a dot - then reform with a domainname
                    if (result.Host.IndexOf(".") == -1)
                        return string.Format("{0}{1}{2}", start,

                            //scheme/domain/rest of link
                            string.Format("{0}://{1}/{2}{3}",
                            result.Scheme.Replace("file", "http"),
                            DefaultDomainName,
                            result.Host.TrimStart(new char[] { '/' }),//should never have this - but to be safe...
                            result.PathAndQuery),

                            end);

                    return string.Format("{0}{1}{2}", start, result.AbsoluteUri.Replace("file://", "http://"), end);
                }
            }

            return m.Value;
        }

        /// <summary>        
        /// a prepend link will always include a ?someKey=
        /// leave any other ?s in the link - let other handling routines deal since we will be splitting after url=
        /// some of those urls will need the ?
        /// don't go changing case of links ie google.com links are case sensitive - times are case-sensitive 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string RegexMatch_PrependToUrl(Match m)
        {
            string linkToPrepend = _prependUrl;

            //we have start and end because the quoting could be different
            string start = m.Groups["linkStart"].Value;
            string link = m.Groups["linkProper"].Value.TrimEnd('.');
            string end = (m.Groups["linkEnd"] != null) ? m.Groups["linkEnd"].Value : string.Empty;

            //ignore mailtos and where the link contains the prependToLink
            //prependtext needs to match case due to replace (no case option)
            if (link.IndexOf("mailto", StringComparison.OrdinalIgnoreCase) == -1 && link.IndexOf(linkToPrepend) == -1)
            {
                //DON'T DO THIS LEAVE ?s in the original url
                //transform ? in link to ampersand
                //if (linkToPrepend.IndexOf("?") != -1 && link.IndexOf("?") != -1)
                //    link = link.Replace("?", "&");

                string prep = CleanUrl_EnforceProperQsFormat(string.Format("{0}&{1}", linkToPrepend, link));
                
                //simply change the link to be the original link plus the prepend
                return string.Format("{0}{1}{2}", start, prep, end);
            }

            //if no match - return the original
            return m.Value;
        }

        /// <summary>
        /// Add ? if not already there
        /// </summary>
        private string RegexMatch_AppendToUrl(Match m)
        {
            //we will decide if we need to add an ampersand or q later on
            string qsToAppend = _appendToUrl.Trim(new char[] { '&', '?' });
            
            //we have start and end because the quoting could be different
            string start = m.Groups["linkStart"].Value;
            string link = m.Groups["linkProper"].Value.TrimEnd('.');
            string end = (m.Groups["linkEnd"] != null) ? m.Groups["linkEnd"].Value : string.Empty;

            //ignore mailtos and where the link contains the appendToLink
            //appendtext needs to match case due to replace (no case option)
            if (link.IndexOf("mailto", StringComparison.OrdinalIgnoreCase) == -1 && link.IndexOf(qsToAppend) == -1)
            {
                //we are either starting a new qs or adding to existing
                link = CleanUrl_EnforceProperQsFormat(string.Format("{0}{1}{2}", link, (link.IndexOf("?") == -1) ? "?" : "&", qsToAppend));
                
                return string.Format("{0}{1}{2}", start, link, end);
            }

            return m.Value;
        }

        /// <summary>
        /// removes specified from url - takes care of dangling ?s and &s
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string RegexMatch_RemoveTextFromUrl(Match m)
        {
            //get link from match
            string link = m.Groups["linkProper"].Value.TrimEnd('.');

            //just match on content
            string linkToRemove = _removeFromUrl.Trim(new char[] { '?', '&' });

            //ignore mailtos and where the link contains the prependToLink
            //removetext needs to match case due to replace (no case option)
            if (link.IndexOf("mailto", StringComparison.OrdinalIgnoreCase) == -1 && link.IndexOf(linkToRemove) != -1)
            {
                string start = m.Groups["linkStart"].Value;  //a href="              
                string end = (m.Groups["linkEnd"] != null) ? m.Groups["linkEnd"].Value : string.Empty;//" target="_blank">xxx</a>

                return string.Format("{0}{1}{2}", 
                    start, 
                    CleanUrl_EnforceProperQsFormat(link.Replace(linkToRemove, string.Empty)), 
                    end);
            }

            return m.Value;//if no match - return the original
        }

        /// <summary>
        /// Makes sure we don't have weird QS formats. Removes double ampersands, enforces a ? for a url if necessary, etc
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static string CleanUrl_EnforceProperQsFormat(string link)
        {
            if (link.IndexOf("?") != -1 || link.IndexOf("&") == -1)
            {
                //remove the match
                //cleanup artifacts - trim any trailing stuff
                string newLink = link.Replace("??", "?").Replace("&&", "&").Replace("?&", "?").Replace("&?", "&").Replace("=&", "=").Trim(new char[] { '?', '&' });

                StringBuilder sb = new StringBuilder();
                string[] parts = newLink.Split(new char[] { '?', '&' });

                int len = parts.Length;
                for (int i = 0; i < len; i++)
                {
                    sb.Append(parts[i]);

                    if (i == 0 && len > 1)
                        sb.Append("?");
                    else if (i > 0 && (i < len - 1))
                        sb.Append("&");
                }

                return sb.ToString().Trim(new char[] { '?', '&' });

            }
            
            return link;
        }

        #endregion
    }
}
