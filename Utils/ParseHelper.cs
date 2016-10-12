using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Utils
{
    public class ListFiller
    {
        private static List<ListItem> stateProvinces = new List<ListItem>();

        public static List<ListItem> StateProvinces
        {
            get
            {
                if (stateProvinces == null || stateProvinces.Count == 00)
                {
                    stateProvinces.Add(new ListItem("Alaska", "AK"));
                    stateProvinces.Add(new ListItem("Alabama", "AL"));
                    stateProvinces.Add(new ListItem("Arkansas", "AR"));
                    stateProvinces.Add(new ListItem("Arizona", "AZ"));
                    stateProvinces.Add(new ListItem("California", "CA"));
                    stateProvinces.Add(new ListItem("Colorado", "CO"));
                    stateProvinces.Add(new ListItem("Connecticut", "CT"));
                    stateProvinces.Add(new ListItem("D.C.", "DC"));
                    stateProvinces.Add(new ListItem("Delaware", "DE"));
                    stateProvinces.Add(new ListItem("Florida", "FL"));
                    stateProvinces.Add(new ListItem("Georgia", "GA"));
                    stateProvinces.Add(new ListItem("Hawaii", "HI"));
                    stateProvinces.Add(new ListItem("Idaho", "ID"));
                    stateProvinces.Add(new ListItem("Illinois", "IL"));
                    stateProvinces.Add(new ListItem("Indiana", "IN"));
                    stateProvinces.Add(new ListItem("Iowa", "IA"));
                    stateProvinces.Add(new ListItem("Kansas", "KS"));
                    stateProvinces.Add(new ListItem("Kentucky", "KY"));
                    stateProvinces.Add(new ListItem("Louisiana", "LA"));
                    stateProvinces.Add(new ListItem("Massachusetts", "MA"));
                    stateProvinces.Add(new ListItem("Maryland", "MD"));
                    stateProvinces.Add(new ListItem("Maine", "ME"));
                    stateProvinces.Add(new ListItem("Michigan", "MI"));
                    stateProvinces.Add(new ListItem("Minnesota", "MN"));
                    stateProvinces.Add(new ListItem("Missouri", "MO"));
                    stateProvinces.Add(new ListItem("Mississippi", "MS"));
                    stateProvinces.Add(new ListItem("Montana", "MT"));
                    stateProvinces.Add(new ListItem("North Carolina", "NC"));
                    stateProvinces.Add(new ListItem("North Dakota", "ND"));
                    stateProvinces.Add(new ListItem("Nebraska", "NE"));
                    stateProvinces.Add(new ListItem("New Hampshire", "NH"));
                    stateProvinces.Add(new ListItem("New Jersey", "NJ"));
                    stateProvinces.Add(new ListItem("New Mexico", "NM"));
                    stateProvinces.Add(new ListItem("Nevada", "NV"));
                    stateProvinces.Add(new ListItem("New York", "NY"));
                    stateProvinces.Add(new ListItem("Ohio", "OH"));
                    stateProvinces.Add(new ListItem("Oklahoma", "OK"));
                    stateProvinces.Add(new ListItem("Oregon", "OR"));
                    stateProvinces.Add(new ListItem("Pennsylvania", "PA"));
                    stateProvinces.Add(new ListItem("Rhode Island", "RI"));
                    stateProvinces.Add(new ListItem("South Carolina", "SC"));
                    stateProvinces.Add(new ListItem("South Dakota", "SD"));
                    stateProvinces.Add(new ListItem("Tennessee", "TN"));
                    stateProvinces.Add(new ListItem("Texas", "TX"));
                    stateProvinces.Add(new ListItem("Utah", "UT"));
                    stateProvinces.Add(new ListItem("Virgina", "VA"));
                    stateProvinces.Add(new ListItem("VI", "VI"));
                    stateProvinces.Add(new ListItem("Vermont", "VT"));
                    stateProvinces.Add(new ListItem("Washington", "WA"));
                    stateProvinces.Add(new ListItem("Wisconsin", "WI"));
                    stateProvinces.Add(new ListItem("West Virgina", "WV"));
                    stateProvinces.Add(new ListItem("Wyoming", "WY"));

                    stateProvinces.Add(new ListItem("Alberta", "AB"));
                    stateProvinces.Add(new ListItem("British Columbia", "BC"));
                    stateProvinces.Add(new ListItem("Manitoba", "MB"));
                    stateProvinces.Add(new ListItem("New Brunswick", "NB"));
                    stateProvinces.Add(new ListItem("Newfoundland and Labrador", "NF"));
                    stateProvinces.Add(new ListItem("Nova Scotia", "NS"));
                    stateProvinces.Add(new ListItem("Northwest Territories", "NT"));
                    stateProvinces.Add(new ListItem("Nunavut", "NU"));
                    stateProvinces.Add(new ListItem("Ontario", "ON"));
                    stateProvinces.Add(new ListItem("Price Edward Is.", "PE"));
                    stateProvinces.Add(new ListItem("Quebec", "PQ"));
                    stateProvinces.Add(new ListItem("Saskatchewan", "SK"));
                    stateProvinces.Add(new ListItem("Yukon Territory", "YK"));

                    stateProvinces.Add(new ListItem("Puerto Rico", "PR"));
                    stateProvinces.Add(new ListItem("US Virgin Islands", "VI"));
                    /*
                    stateProvinces.Add(new ListItem("Canal Zone", "CZ"));
                    stateProvinces.Add(new ListItem("Guam", "GU"));
                    stateProvinces.Add(new ListItem("Military", "MY"));
                     */
                }

                return stateProvinces;
            }
        }

        private static List<ListItem> countries = new List<ListItem>();

        public static List<ListItem> CountryListing
        {
            get
            {
                if (countries == null || countries.Count == 00)
                {
                    countries.Add(new ListItem("United States", "US"));
                    countries.Add(new ListItem("Canada", "CA"));
                    countries.Add(new ListItem("United Kingdom", "GB"));
                }

                return countries;
            }
        }
    }

	/// <summary>
	/// Summary description for FillList.
	/// </summary>
	public class ParseHelper
	{
		ArrayList messages = new ArrayList();


        public ParseHelper() { }
        
        /// <summary>
        /// TODO: does not work!
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        //public static string RemoveInvalidFileNameChars(string s)
        //{
        //    foreach (char c in System.IO.Path.GetInvalidPathChars())
        //        s = s.Replace(c.ToString(), string.Empty);

        //    return s;
        //}

        /// <summary>
        /// return a sql safe version - min and max dates
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime SanitizeDateTimeToSqlDateTime(DateTime date)
        {
            if (date == DateTime.MinValue)
                date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            if (date == DateTime.MaxValue)
                date = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;

            return date;
        }

        public static string RemoveQuotes(string s)
        {
            char[] arr = new char[] { '\'', '"'};

            foreach (char c in arr)
                s = s.Replace(c.ToString(), string.Empty);

            return s;
        }

        /// <summary>
        /// Allow alpha-numeric chars only as well as dashes and underscores. For help with filenames - not passwords. removes spaces
        /// </summary>
        public static string StripInvalidChars_Filename(string s)
        {
            return Regex.Replace(s, @"[^\w-_]", "");
        }
        
        /// <summary>
        /// Splits a string/int list into a comma separated list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string SplitListIntoString<T>(List<T> list)
        {
            return SplitListIntoString(list, false);
        }

        public static string SplitListIntoString<T>(List<T> list, bool separateWithSingleQuote)
        {
            return SplitListIntoString(list, separateWithSingleQuote, ',');
        }

        public static string SplitListIntoString<T>(List<T> list, bool separateWithSingleQuote, char delimiter)
        {
            //parse items into a list
            //be sure to remove trailing commma when passed to SP
            StringBuilder sb = new StringBuilder();
            foreach (T idx in list)
                sb.AppendFormat("{0}{1}{0}{2}", (separateWithSingleQuote) ? "'" : string.Empty, idx, delimiter);

            return sb.ToString().TrimEnd(delimiter).Trim();
        }

        /// <summary>
        /// TODO: create a generic version
        /// </summary>
        /// <param name="csvList"></param>
        /// <returns></returns>
        public static List<string> CsvSeparatedStringToList(string csvList)
        {
            List<string> list = new List<string>();
            if (csvList != null && csvList.TrimEnd(',').Trim().Length > 0)
            {
                string[] parts = csvList.Split(',');
                foreach(string s in parts)
                    list.Add(s);
            }

            return list;
        }
        		
        public static string ParseCommasAndAmpersands(string parse)
        {
            return parse.Replace(" , ",", ").Replace(" & ", " &amp; ");
        }

        public static string ParseToLength(string parse, int length)
        {
            parse = parse.Trim();

            if (parse.Length > length)
                return parse.Substring(0, length).Trim();

            return parse;
        }

        public static List<string> ReturnValidListOfEmails(string input, bool reportBackInvalidList)
        {
            List<string> valid = new List<string>();
            List<string> invalid = new List<string>();

            if (input.Trim().Length == 0)
                throw new ArgumentOutOfRangeException("No email addresses were specified for delivery.");

            //do this next step to 1)remove any angle brackets 2)format the string so that we can SPLIT it later on ~
            //this may seem redundant - but it is necessary to facilitate how the production server may see things
            string parsed = input.Replace("<", string.Empty).Replace(">", string.Empty).Replace(",", "~")
                .Replace(Environment.NewLine, "~").Replace("\r\n", "~").Replace("\n", "~").Trim();

            string[] mails = parsed.Split('~');

            foreach (string s in mails)
            {
                if (s.Trim().Length > 0)
                {
                    if (!Utils.Validation.IsValidEmail(s.Trim()))
                        invalid.Add(s.Trim());
                    else
                        valid.Add(s.Trim());
                }
            }

            //do not send partial lists - return here
            if (invalid.Count > 0)
            {
                string msg = string.Format("The following emails are invalid and need to be removed or corrected to continue processing:<br/>");
                foreach (string s in invalid)
                    msg += string.Format("{0}<br />", s);

                if(reportBackInvalidList)
                    throw new ArgumentException(msg);
            }

            return valid;
        }

        public static string StringListToLine(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in list)
                sb.AppendLine(s);

            return sb.ToString();
        }

        protected static List<string> tags = new List<string>();

        private static readonly string ST = "st";
        private static readonly string ND = "nd";
        private static readonly string RD = "rd";
        private static readonly string TH = "th";

        public static string ReturnNumberAsOrdinalString(int num)
        {
            string numString = num.ToString();
            if (num < 4 || num > 20)
            {
                if (numString.EndsWith("1"))
                    return string.Format("{0}{1}", numString, ST);
                else if (numString.EndsWith("2"))
                    return string.Format("{0}{1}", numString, ND);
                else if (numString.EndsWith("3"))
                    return string.Format("{0}{1}", numString, RD);
            }

            return string.Format("{0}{1}", numString, TH);
        }

        public static string ReturnJSONFormat<T>(string name, T givenType)
        {
            return string.Format("\"{0}\": \"{1}\"", name, (givenType != null) ? givenType.ToString() : string.Empty);
        }
        
        /// <summary>
        /// removes html tags but leaves anchors and image tags
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string ParseJSON(string txt)
        {
            if (txt == null)
                return null;

            string result = txt.Replace("\t", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);

            result = regexTag.Replace(result, new MatchEvaluator(EvaluateHtml_AllowAnchorsAndImages));
            result = result.Replace("\"", @"\""").Replace("/", @"\/");
            //html encode it here???
            //result = regexNonTag.Replace(result, new MatchEvaluator(EvaluateNonTagTextHtml_EscapeQuotesReplaceCharCodes));
            
            return Regex.Replace(result, @"\s+", " ");//result.Replace("  ", " ");
        }
        
        public static Regex regexTag = new Regex("<(\"[^\"]*\"|\'[^\']*\'|[^\'\">])*>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        public static Regex regexTagMatch = new Regex("(?<match><(\"[^\"]*\"|\'[^\']*\'|[^\'\">])*>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        public static Regex regexLink = new Regex(@"(?<match>((ht|f)tp(s?))\://((([a-zA-Z0-9_\-]{2,}\.)+[a-zA-Z]{2,})|((?:(?:25[0-5]|2[0-4]\d|[01]\d\d|\d?\d)(?(\.?\d)\.)){4}))(:[a-zA-Z0-9]+)?(/[a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~]*)?)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        public static Regex regexNonTag = new Regex("", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        
        private static string EvaluateNonTagTextHtml_EscapeQuotesReplaceCharCodes(Match m)
        {
            string result = m.Value;

            if (result.Length > 0)
            {
                //analyze the non tagged content
                result = Regex.Replace(result, "&nbsp;", " ", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "&laquo;", "<<", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "&raquo;", ">>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "&amp;", "&", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                
                result = Regex.Replace(result, "&#39;|\'", "\'", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "&quot;|\"", "\"", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }

            return result;
        }

        private static string EvaluateHtml_AllowAnchorsAndImages(Match m)
        {
            //determine if the match is an anchor - if so return the text - anchor and all
            if (m.Value.ToLower().StartsWith("<a ") || m.Value.ToLower().StartsWith("<img ") ||
                m.Value.ToLower().EndsWith("/a>") || m.Value.ToLower().StartsWith("/img>"))
                return m.Value;

            //otherwise return a blank string
            return string.Empty;
        }

        /// <summary>
        /// This will examine the provided text for links and will replace fox.com with <a href="fox.com">fox.com</a>
        /// The link must...
        /// </summary>
        public static string ConvertTwitterAnchors(string parse)
        {
            return ParseHelper.regexLink.Replace(parse, new MatchEvaluator(Utils.ParseHelper.ReturnFullLinkForLinkText));
        }

        private static string ReturnFullLinkForLinkText(Match m)
        {
            //we have start and end because the quoting could be different
            string link = m.Groups["match"].Value;

            //only append a querystring to those links on our site
            return link.Replace(link, string.Format("<a target=\"_blank\" href=\"{0}\">{0}</a>", link));
        }

        public static string ConvertLinkToHref(string parse)
        {
            return ParseHelper.regexLink.Replace(parse, new MatchEvaluator(Utils.ParseHelper.ReturnFullLinkForLinkText));
        }

        private static string ReturnLinkHref(Match m)
        {
            //we have start and end because the quoting could be different
            string link = m.Groups["match"].Value;

            //only append a querystring to those links on our site
            return link.Replace(link, string.Format("<a target=\"_blank\" href=\"{0}\">{0}</a>", link));
        }

        /// <summary>
        /// right now this just deals with UL and LI
        /// </summary>
        public static string XmlToText(string parse)
        {
            parse = Regex.Replace(parse, @"<(\/)?ul>", Environment.NewLine, RegexOptions.IgnoreCase);
            parse = Regex.Replace(parse, "<li>", Environment.NewLine, RegexOptions.IgnoreCase);
            return Regex.Replace(parse, regexTag.ToString(), string.Empty);
        }

        /// <summary>
        /// this will remove html tags and replace double quotes, single quotes and ampersands. It will also reduce double spaces to single and will 
        /// remove extra whitespace surrounding commas
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EscCommonSequencesInHtml_AvoidInnerHtml(string input)
        {
            return EscCommonSequencesInHtml_AvoidInnerHtml(input, true);
        }

        public static string RemoveWhitespace(string s)
        {
            return Regex.Replace(s.Trim(), @"\s+", string.Empty);
        }

        /// <summary>
        /// Replaces all occurrences of whitespace (incl \r\n\v\t\f) with single spaces
        /// </summary>
        public static string ReplaceExtraWhitespaces(string s)
        {
            return Regex.Replace(s.Trim(), @"\s+", " ");
        }

        public static string EscCommonSequencesInHtml_AvoidInnerHtml(string input, bool removeTags)
        {   
            tags.Clear();

            string result = string.Empty;

            if(removeTags)
                result = regexTag.Replace(result, new MatchEvaluator(EvaluateHtml_AllowAnchorsAndImages));

            //take out the tags and mark the position
            result = ParseHelper.regexTagMatch.Replace(input, new MatchEvaluator(Utils.ParseHelper.ReturnPlaceholderForMatch));
            
            //substitute for common elements in text
            result = result.Replace(" & ", " &amp; ").Replace("'", "&#39;").Replace("\"", "&quot;").Trim();

            //now put tags back into the string
            int count = tags.Count;
            if (count > 0)
                for (int i = 1; i <= count; i++)
                    result = result.Replace(string.Format("<${0}>", i), tags[i - 1]);

            tags.Clear();            

            return Regex.Replace(result.Trim(), @"\s+", " ").Replace(" , ", ", ");
        }

        public static string DoubleSpaceToSingle(string s)
        {
            return Regex.Replace(s.Trim(), @"\s+", " ");
        }

        /// <summary>
        /// remove any html tags
        /// replace non 1-9 and non a-z to -
        /// replace spaces with -
        /// remove mult dashes
        /// make sure we dont end on a dash
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FriendlyFormat(string s)
        {
            string ns = string.Empty;
            char delimiter = '-';

            //remove any html
            s = Utils.ParseHelper.StripHtmlTags(s).ToLower();

            //Regex.Replace(result, @"\s+", " ")
            ns = Regex.Replace(s.Replace("'", ""), @"\W", delimiter.ToString(), RegexOptions.IgnoreCase);

            //chars we allow - see utils allowable per rfc regex
            //$-_.+ !*'(),
            //chars we would not like to use in a url - personal preference
            //!*(),
            ns = ns.Replace("!", string.Empty).Replace("*", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace(",", "-");
            
            //replace itself
            ns = Regex.Replace(ns, @"-+", delimiter.ToString(), RegexOptions.IgnoreCase);
            ns = ns.Trim(new char[] { ' ', '-' });


            if (ns.Length == 0 || ns == "-" || ns.EndsWith("-"))//although - from above, ns cannot end with dash
                ns = System.Web.HttpUtility.HtmlEncode(ns);

            if(ns.StartsWith(delimiter.ToString()))
                ns = string.Format("evt{0}", ns);
            if(ns.EndsWith(delimiter.ToString()))
                ns = string.Format("{0}evt", ns);

            //now capitalize the main words
            ns = CapitalizeParts(ns, delimiter);

            return ns;
        }

        /// <summary>
        /// Strip html,spaces and return an aplha numeric string
        /// </summary>
        public static string FormatAlphaNumeric(string s)
        {
            string ns = string.Empty;

            //remove any html
            s = Utils.ParseHelper.StripHtmlTags(s).ToLower();

            //Regex.Replace(result, @"\s+", " ")
            ns = Regex.Replace(s.Replace("'", ""), @"\W", string.Empty, RegexOptions.IgnoreCase);
            //replace itself
            ns = Regex.Replace(ns, @"-+", string.Empty, RegexOptions.IgnoreCase);
            ns = ns.Trim(new char[] { ' ', '-' });

            return ns;
        }

        public static string CapitalizeParts(string s, char delimiter)
        {
            string[] pieces = s.Split(delimiter);
            for (int i = 0; i < pieces.Length; i++)
            {
                string part = pieces[i].Trim();
                if (part.Length == 1)
                    pieces[i] = part.ToUpper();
                else if (part.Length > 1)
                    pieces[i] = string.Format("{0}{1}", part.Substring(0, 1).ToUpper(), part.Remove(0, 1));
                else
                    pieces[i] = string.Empty;
            }

            return String.Join(delimiter.ToString(), pieces);
        }

        public static string EscCommonSequencesInHtml_AvoidInnerHtml_RemoveLinksImgs(string input, bool removeTags)
        {   
            tags.Clear();

            string result = string.Empty;

            if(removeTags)
                result = regexTag.Replace(result, string.Empty);

            //take out the tags and mark the position
            result = ParseHelper.regexTagMatch.Replace(input, new MatchEvaluator(Utils.ParseHelper.ReturnPlaceholderForMatch));
            
            //substitute for common elements in text
            result = result.Replace(" & ", " &amp; ").Replace("'", "&#39;").Replace("\"", "&quot;").Trim();

            //now put tags back into the string unless they are images or links
            int count = tags.Count;
            if (count > 0)
                for (int i = 1; i <= count; i++)
                {
                    string tagLine = tags[i - 1];
                    result = result.Replace(string.Format("<${0}>", i), 
                        (tagLine.ToLower().StartsWith("<a ") || tagLine.ToLower().StartsWith("<img ") ||
                        tagLine.ToLower().EndsWith("/a>") || tagLine.ToLower().EndsWith("/img>") || tagLine.ToLower().EndsWith("/ img>")) ? string.Empty : tagLine);
                }

            tags.Clear();            

            return Regex.Replace(result.Trim(), @"\s+", " ").Replace(" , ", ", ");
        }

        public static string LinksToHref(string input)
        {
            tags.Clear();
            string original = input;
            string result = string.Empty;
            
            //take out the tags and mark the position
            result = ParseHelper.regexTagMatch.Replace(input, new MatchEvaluator(Utils.ParseHelper.ReturnPlaceholderForMatch));

            //substitute for common elements in text
            result = result.Replace(" & ", " &amp; ").Replace("'", "&#39;").Replace("\"", "&quot;").Trim();

            //now put tags back into the string unless they are images or links
            int count = tags.Count;

            if (count > 0)
            {
                for (int i = 1; i <= count; i++)
                {
                    string tagLine = tags[i-1];

                    if (tagLine.ToLower().StartsWith("<a "))
                    {
                        //get the href
                        tagLine = Regex.Replace(tagLine.Trim(), @"\s+", string.Empty);
                        int start = tagLine.ToLower().IndexOf("href=") + 5;
                        int end = tagLine.IndexOf("\"", start + 1) + 1;

                        char[] stripChars = { '\\', '"' };
                        string href = tagLine.Substring(start, end-start).Trim(stripChars);

                        if (!(tagLine.EndsWith("/>") || tagLine.EndsWith("/ >")))
                        {
                            //TODO test with anchor at end
                            //TODO test with a broken anchor
                            //find the closing tag
                            int j = i;
                            while (j++ < count)
                            {
                                if (tags[j-1].ToLower().EndsWith("/a>") || tags[j-1].ToLower().EndsWith("/ a>"))
                                {
                                    //remove the tag
                                    int startDelete = result.IndexOf(string.Format("<${0}>", i));
                                    string endTag = string.Format("<${0}>", j);
                                    int endDelete = result.IndexOf(endTag) + endTag.Length;
                                    result = result.Remove(startDelete, endDelete - startDelete);
                                    //plop in the href
                                    result = result.Insert(startDelete, href);
                                    break;
                                }
                            }
                        }

                        //remove everything in the text of the anchor - return the href only
                    }
                }
            }

            tags.Clear();

            result = Utils.ParseHelper.StripHtmlTags(result, " ");
            return Regex.Replace(result.Trim(), @"\s+", " ").Replace(" , ", ", ");
        }
        
        public static string ReturnPlaceholderForMatch(Match m)
        {
            //we have start and end because the quoting could be different
            string start = m.Groups["match"].Value;
            tags.Add(start);

            //only append a querystring to those links on our site
            return start.Replace(start, string.Format("<${0}>", tags.Count));
        }

        public static string StripHtmlTags(string parse)
        {
            return StripHtmlTags(parse, string.Empty);
        }

        public static string StripHtmlTags(string parse, string replaceString)
        {
            return Regex.Replace(parse, regexTag.ToString(), replaceString);
        }

        //public static string ReplaceDoubleQuotesInString_BypassTags(string parse)
        //{
        //    return string.Empty;
        //}

        public static string ParseJsAlert(string parse)
        {
            return ParseJsAlert(parse, 256);
        }

        public static string ParseJsAlert(string parse, int length)
        {
            parse = parse.Replace("'", string.Empty);
            parse = parse.Replace("\r\n", " ").Replace("\t", " ");

            if (parse.Length > 200)
                parse = parse.Substring(0, 200);

            return parse.Trim();
        }

        public static string EscapeDoubleQuote_HtmlEquivalent(string str)
        {
            return str.Replace("\"", "&quot;");
        }

        /// <summary>
        /// Converts the input plain-text to HTML version, replacing carriage returns
        /// and spaces with <br /> and &nbsp;
        /// </summary>
        public static string ConvertToHtml(string content)
        {
            content = System.Web.HttpUtility.HtmlEncode(content);
            content = content.Replace("  ", "&nbsp;&nbsp;").Replace(
               "\t", "&nbsp;&nbsp;&nbsp;").Replace("\n", "<br>");
            return content;
        }

        public static int AbsoluteValue(int i)
        {
            return (i < 0) ? (i * (-1)) : i;
        }

        public static decimal AbsoluteValue(decimal d)
        {
            return (d < 0) ? decimal.Negate(d) : d;
        }

        public static string FormatUrlFromString(string link)
        {
            return FormatUrlFromString(link, true, false);
        }

        public static string FormatUrlFromString(string link, bool httpRequired)
        {
            return FormatUrlFromString(link, httpRequired, false);
        }

        /// <summary>
        /// Verifies that a link has the correct format and protocol and ecodes the url
        /// </summary>
        /// <param name="link"></param>
        /// <param name="httpRequired"></param>
        /// <param name="secureLinkRequired"></param>
        /// <returns></returns>
        public static string FormatUrlFromString(string link, bool httpRequired, bool secureLinkRequired)
        {
            if (link == null)
                return string.Empty;

            string rawLink = link.ToLower();
            if (rawLink.StartsWith("http") && rawLink.IndexOf("//") != -1)
                rawLink = link.Substring(link.IndexOf("//") + 2);

            if (httpRequired)
                return rawLink.Insert(0, string.Format("http{0}://", (secureLinkRequired) ? "s" : string.Empty));

            return rawLink;
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            Random r = new Random();
            
            lock (r) 
            { 
                return r.Next(min, max); 
            } 
        }

        /// <summary>
        /// generates a random password with alpha chars only - use Membership.GeneratePassword() if you want non-alpha 
        /// </summary>
        public static string GenerateRandomPassword(int length)
        {
            String _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            Byte[] randomBytes = new Byte[length];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            char[] chars = new char[length];

            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < length; i++)
            {
                chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
            }

            return new string(chars);
        }

        public static string GetStateCode(string state)
        {
            string _state = state.Trim().ToLower();

            if (_state.Trim().Length > 2)
            {
                List<ListItem> list = Utils.ListFiller.StateProvinces;

                ListItem li = list.Find(delegate(ListItem match) { return (match.Text.Trim().ToLower() == _state); });

                if (li != null)
                    _state = li.Value;
            }

            return _state.ToUpper();
        }

        public static string GetCountryCode(string country)
        {
            string _country = country.Trim().ToLower();

            if (_country.Trim().Length > 2)
            {
                List<ListItem> list = Utils.ListFiller.CountryListing;

                ListItem li = list.Find(delegate(ListItem match) { return (match.Text.Trim().ToLower() == _country); });

                if (li != null)
                    _country = li.Value;
            }

            return _country.ToUpper();
        }
        
        public static string Parse_DateForTableInsert(DateTime date)
        {
            return date.ToString("MM/dd/yyyy hh:mmtt");
        }

        public static string ExtractListValueString<T>(List<T> list, char delimiter)
        {
            char[] delims = new char[2];
            delims[0] = ' ';
            delims[1] = delimiter;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (T item in list)
                sb.AppendFormat("{0}{1} ", item.ToString(), delimiter);

            return sb.ToString().TrimEnd(delims);
        }

        public static string ExtractListValueString(CheckBoxList list)
        {
            return ParseHelper.DelimiterSeparatedList(list.Items, Utils.Constants.Separator);
        }

        private static string DelimiterSeparatedList(ListItemCollection coll, char delimiter)
        {
            StringBuilder prms = new StringBuilder();

            foreach (ListItem li in coll)
                if (li.Selected)
                    prms.AppendFormat("{0}{1}", li.Value, delimiter);

            return prms.ToString();
        }

        public static string DoReplacements(string template, ListDictionary replacements)
        {
            return DoReplacements(template, replacements, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="replacements"></param>
        /// <param name="doHtmlReplacement">indicates wether we should use \r\n of <br> for newlines</br></param>
        /// <returns></returns>
        public static string DoReplacements(string template, ListDictionary replacements, bool doHtmlReplacement)
        {
            string replacedTemplate = template;

            foreach (DictionaryEntry e in replacements)
            {
                string keyName = e.Key.ToString();
                string val = (e.Value != null) ? e.Value.ToString() : string.Empty;

                if(replacedTemplate.IndexOf(keyName) != -1 )
                    replacedTemplate = replacedTemplate.Replace(keyName, 
                        (doHtmlReplacement) ? e.Value.ToString().Replace(Environment.NewLine, "<br/>") : e.Value.ToString());
            }

            return replacedTemplate;
        }

        public static string ConvertEmptyStringToNull(string str)
        {
            if (str == null || str.Trim().Length == 0)
                return null;

            return str;
        }

        public static string FormatPhoneNumberWithDecimal(string number)
        {
            number = number.Trim();
            //todo test for all nummeric - regex

            number = number.Insert(number.Length - 4, ".");
            if (number.Length != 10)
                number = number.Insert(3, ".");

            return number;
        }

        public static Pair ValueInPair(int ordinal, List<Pair> list, string value)
        {
            foreach (Pair p in list)
            {
                if ((ordinal == 1 && p.First.ToString().ToLower() == value.ToLower()) ||
                    (ordinal == 2 && p.Second.ToString().ToLower() == value.ToLower()) )
                {
                    return p;
                }
            }

            return null;
        }

        public static Triplet ValueInTriplet(int ordinal, List<Triplet> list, string value)
        {
            foreach (Triplet t in list)
            {
                if( (ordinal == 1 && t.First.ToString().ToLower() == value.ToLower()) ||
                    (ordinal == 2 && t.Second.ToString().ToLower() == value.ToLower()) ||
                    (ordinal == 3 && t.Third.ToString().ToLower() == value.ToLower()) )
                {
                    return t;
                }
            }

            return null;
        }

        public static string ConvertList_ToCommaAmpersandString(List<string> list)
        {
            string listing = null;
            foreach (string s in list)
                listing += string.Format("{0}~", s);

            return ConvertTildesToCommasAndAmpersands(listing);
        }

        public static string ConvertTildesToCommasAndAmpersands(string sb)
        {
            return ConvertTildesToCommasAndSeparator(sb, "&");
        }

        public static string ConvertTildesToCommasAndSeparator(string sb, string separator)
        {
            sb = sb.Trim();//, string.Empty, sb.Length - 1, 1);//remove the last tilde if it exists

            if (sb.IndexOf('~') != -1)
            {
                int last = sb.LastIndexOf("~");
                sb = sb.Remove(last, 1);

                last = sb.LastIndexOf("~");
                if (last > -1)
                {
                    sb = sb.Remove(last, 1);

                    sb = sb.Insert(last, string.Format(" {0} ", separator));
                    sb = sb.Replace("~", ", ");//convert tildes to commas
                }
            }

            return Regex.Replace(sb, @"\s+", " ");
        }

        public static void ConvertTildesToCommasAndAmpersands(StringBuilder sb)
        {
            ConvertTildesToCommasAndSeparator(sb, "&");
        }

        public static void ConvertTildesToCommasAndSeparator(StringBuilder sb, string separator)
        {
            string parse = (sb == null || sb.Length == 0) ? string.Empty : sb.ToString().Trim();
            
            if (parse.Length == 0) 
                return;

            sb.Length = 0;
            
            if (parse.ToString().IndexOf('~') != -1)
            {
                int last = parse.LastIndexOf("~");
                parse = parse.Remove(last, 1);

                last = parse.LastIndexOf("~");
                if (last > -1)
                {
                    parse = parse.Remove(last, 1);

                    parse = parse.Insert(last, string.Format(" {0} ", separator));
                    parse = parse.Replace("~", ", ");			//convert tildes to commas
                }
            }

            sb.Append(Regex.Replace(parse, @"\s+", " "));
        }

		public static System.Globalization.CultureInfo UsFormat = new System.Globalization.CultureInfo("en-US", true);

		public static bool ValidateDate(string date)
		{
			try
			{
				DateTime dt = DateTime.Parse(date, UsFormat);

				return true;
			}
			catch{}
			
			return false;
		}

		public static DateTime ParseDate(string date)
		{
			return DateTime.Parse(date, UsFormat);
		}

		public static void FillListWithNums(DropDownList list, int lowValue, int topValue)
		{
			if(list.Items.Count == 0)
			{
				for(int i=lowValue; i<=topValue; i++)
					list.Items.Add(new ListItem(i.ToString()));
				
				list.SelectedIndex = -1;
			}
		}

        /// <summary>
        /// milliseconds of the date since the js event horizon
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static double DateTime_To_JavascriptDate(DateTime dt)
        {
            DateTime d1 = new DateTime(1970, 1, 1);

            DateTime u1 = d1.ToUniversalTime();
            double ts = new TimeSpan(u1.Ticks).TotalMilliseconds;

            double js = new TimeSpan(dt.Ticks - d1.Ticks).TotalMilliseconds;
            return js;
        }

        /// <summary>
        /// date since event horizon + milliseconds
        /// presupposes that milliseconds was created from above - or via javascript
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DateTime JavascriptDate_To_DateTime(double milliseconds)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            return d1.AddMilliseconds(milliseconds);
        }

        public static string ConvertTo_ProperCase(string text)
        {
            System.Globalization.TextInfo myTI = new System.Globalization.CultureInfo("en-US", false).TextInfo;
            return myTI.ToTitleCase(text.ToLower());
        }
	
		public static string StringToProperCase(string prop)
		{
			string[] content = prop.Trim().ToLower().Split(' ');
			string properCase = "";

			foreach(string s in content)
			{
				if(s.Length > 0)
				{
					if(s == "mfa") properCase = string.Format("{0} ", s.ToUpper());
					else if( (s.Substring(0,1).ToUpper().CompareTo("A") >= 0 && s.Substring(0,1).ToUpper().CompareTo("Z") <= 0) ||
						(s.Substring(0,1).ToUpper().CompareTo("0") >= 0 && s.Substring(0,1).ToUpper().CompareTo("9") <= 0) )//is in the range of ascii A-Z or 0-9
						properCase += string.Format("{0}{1} ", s.Substring(0,1).ToUpper(), s.Substring(1));
					else
					{
						//split the string into a char array
						//and when we hit the first A-Z or 0-9 cap it and return rest
						char[] chrs = s.ToCharArray();
						bool capped = false;
						for(int i=0; i<chrs.Length; i++)
						{
							if( capped == false &&
								((chrs[i].ToString().ToUpper().CompareTo("A") >= 0 && chrs[i].ToString().ToUpper().CompareTo("Z") <= 0) ||
								(chrs[i].ToString().ToUpper().CompareTo("0") >= 0 && chrs[i].ToString().ToUpper().CompareTo("9") <= 0)) )
							{
								capped = true;
								properCase += chrs[i].ToString().ToUpper();
							}
							else
								properCase += chrs[i].ToString();

						}

						properCase += string.Format(" ");
					}
						
				}
			}

			return properCase.Trim();
		}
	}
}
