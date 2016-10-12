using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Newtonsoft.Json;

using System.Net;
using System.Web;

using Utils;

namespace Wcss
{
    public partial class SubscriptionEmail
    {
        /// <summary>
        /// legacy tag
        /// </summary>
        public static string publishLinkPlaceholder = "<!--PUBLISHLINK-->";
        public static string optoutPlaceholder = "<!--OPTOUT-->";
        /// <summary>
        /// new newsletter tag as of May 2014
        /// </summary>
        public static string publishLinkOnlyPlaceholder = "<!--PUBLISHONLY-->";
        public static string optoutInfoPlaceholder = "<!--OPTOUTINFO-->";

        [XmlAttribute("DateLastSent")]
        public DateTime DateLastSent
        {
            get { return (this.DtLastSent.HasValue) ? this.DtLastSent.Value : DateTime.MinValue; }
            set { this.DtLastSent = (value > DateTime.MinValue) ? (DateTime?)value : null; }
        }

        public static string Path_MailTemplate = string.Format("/{0}/MailTemplates/", _Config._VirtualResourceDir);
        public static string Path_PostedSubscriptions = string.Format("{0}Subscriptions/", Path_MailTemplate);
        public static string Path_PostedImages = string.Format("{0}Images/", Path_MailTemplate);
        public static string Path_PostedCss = string.Format("{0}Css/", Path_MailTemplate);
        public static string Css_OptOut = ".autopublishlink{margin-bottom: 1em;} .autopublishlink A, .autooptout A{display:inline;} .autooptout{margin-top: 1em;} ";

        public string PublishedPathAndFile_Virtual
        {
            get
            {
                return string.Format("{0}{1}", SubscriptionEmail.Path_PostedSubscriptions, this.PostedFileName);
            }
        }

        public string PublishedPathAndFile_Mapped
        {
            get
            {
                if (System.Web.HttpContext.Current != null)
                    return System.Web.HttpContext.Current.Server.MapPath(this.PublishedPathAndFile_Virtual);

                return string.Format("{0}{1}", _Config._MappedRootDirectory, this.PublishedPathAndFile_Virtual);
            }
        }

        public string PublishedPathAndFile_Url
        {
            get
            {
                return string.Format("http://{0}{1}", _Config._DomainName, this.PublishedPathAndFile_Virtual);
            }
        }

        [XmlAttribute("EmailLetterName")]
        public string EmailLetterName
        {
            get { return this.EmailLetterRecord.Name; }
        }

        [XmlAttribute("CssFile_Parsed")]
        public string CssFile_Parsed
        {
            get { return this.CssFile ?? string.Empty; }
        }

        public static string ConstructPostedFileName()
        {
            return string.Format("{0}.html", Guid.NewGuid().ToString().Replace("-", string.Empty));
        }

        public static string ConstructBodyName(string name, bool appendHtml)
        {
            string datePart = string.Empty;
            string strippedName = name.Trim();

            if (strippedName.IndexOf("_") != -1)
            {
                string leading = strippedName.Substring(0, strippedName.IndexOf("_") - 1);
                if (Utils.Validation.IsDate(leading))
                    datePart = leading;

                strippedName = strippedName.Substring(strippedName.IndexOf("_")).TrimStart('_');
            }

            strippedName = Utils.ParseHelper.StringToProperCase(strippedName).Replace(" ", string.Empty).Replace("/","-").Replace("\\","-");

            if(appendHtml && (!strippedName.EndsWith(".html", StringComparison.OrdinalIgnoreCase)))
                strippedName = string.Format("{0}.html", strippedName);

            return string.Format("{0}_{1}", (datePart.Trim().Length > 0) ? datePart : DateTime.Now.ToString("yyMMddhhmmtt"), 
                strippedName);
        }

        /// <summary>
        /// Sets the constructed email values to null to allow the service to rewrite. This needs to be rewritten by the service to ensure 
        /// the most up to dat version
        /// </summary>
        private void PrepareForPublication()
        {
            this.ConstructedHtml = null;
            this.ConstructedText = null;
            this.FinalHtml = null;
            this.Save();
        }

        /// <summary>
        /// Must be called prior to publishing the path in the email as it will change the body name and 
        /// where it is posted if needed. Ensures that the email has been converted to html and is published to the web
        /// </summary>
        /// <param name="subEmailIdx"></param>
        public void EnsurePublication()
        {
            this.PrepareForPublication();

            //posted file and mapped resource dir overlap - so use get directory
            //map the path
            string mappedPath = Path.GetFullPath(PublishedPathAndFile_Mapped);

            //if file exists - delete
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                // clear existing contents
                if (File.Exists(mappedPath))
                    File.Delete(mappedPath);

                fs = new FileStream(mappedPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                sw = new StreamWriter(fs, Encoding.GetEncoding("utf-8"));

                sw.Write(this.CreateShell_Html(true, false, false, true));
                
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
            finally
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        /// creates an html formatted string from a text file - with options for "cannot view" and opt-out links
        /// </summary>
        /// <param name="insertHtmlStructure">Determines if we need to add the doctype, html, head, etc. This should only be skipped(false) for the 
        /// file viewer that is contained in the html rendered for the viewer page</param>
        /// <param name="includeCantViewAndOptOutLinks">Determines if the "cannot view" and opt out links are included. Not necessary for published versions</param>
        /// <param name="convertVirtualLinksToUrls">True for an email that does not contain embedded images. Sets a link to the hosted images, etc.</param>
        /// <param name="UseCampaignLinks">emailLetter object does not get the conversion</param>
        public string CreateShell_Html(
            bool insertHtmlStructure, 
            bool includeViewAndOptOutLinks, 
            bool convertVirtualLinksToUrls,
            bool useCampaignLinks)
        {
            string domainName = _Config._DomainName;
            string virtResourceDirectory = _Config._VirtualResourceDir;
            
            string cssPathAndFile = (this.CssFile == null) ? string.Empty :
                string.Format("http://{0}{1}{2}", domainName, SubscriptionEmail.Path_PostedCss, this.CssFile.Trim());

            bool Is3rdParty = (this.EmailLetterRecord.TextVersion == _Enums.MailTemplateTypes.Is3rdPartySender.ToString());
            string content = this.EmailLetterRecord.HtmlVersion.Trim();
                 
            int tabOrdinal = 1;
            StringBuilder sb = new StringBuilder();

            //this should only be skipped for the file viewer that is contained in html already
            if (insertHtmlStructure)
            {
                //http://htmlemailboilerplate.com/
                sb.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
                sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.AppendLine("<head>");
                sb.AppendFormat("{0}<title>{1}</title>", Constants.Tabs(tabOrdinal), this.EmailLetterRecord.Subject.Trim());
                sb.AppendLine();
                sb.AppendFormat("{0}<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />", Constants.Tabs(tabOrdinal));
                sb.AppendLine();
                sb.AppendFormat("{0}<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/>", Constants.Tabs(tabOrdinal));
                sb.AppendLine();
                sb.AppendLine();

                if(Is3rdParty)
                    sb.AppendFormat("{2}{0}<meta name=\"subemlid\" content=\"{1}\" />", Constants.Tabs(tabOrdinal), this.Id, Constants.NewLine);
                else
                    sb.AppendFormat("{2}{0}<meta subemlid=\"{1}\" />", Constants.Tabs(tabOrdinal), this.Id, Constants.NewLine);

                sb.AppendLine();
                sb.AppendLine();


                //styling
                sb.AppendFormat("{0}<style type=\"text/css\">", Constants.Tabs(tabOrdinal));
                sb.AppendLine();

                if (!Is3rdParty)
                {
                    //always add these style to deal with opt out links, problems viewing link and address info
                    if (includeViewAndOptOutLinks && content.IndexOf(publishLinkOnlyPlaceholder) == -1)
                    {
                        sb.AppendFormat("{0}{1}", Constants.Tabs(tabOrdinal + 1), SubscriptionEmail.Css_OptOut);
                        sb.AppendLine();
                        sb.AppendLine();
                    }
                }

                //add addtitional styling
                string style = this.EmailLetterRecord.StyleContent;
                if (style != null && style.Trim().Length > 0)
                {
                    sb.AppendFormat("{0}{1}", Constants.Tabs(tabOrdinal + 1), style);
                    sb.AppendLine();
                    sb.AppendLine();
                }

                sb.AppendFormat("{0}</style>", Constants.Tabs(tabOrdinal));
                sb.AppendLine();

                sb.AppendLine("</head>");
                sb.AppendLine();
                sb.AppendLine("<body>");
                sb.AppendLine();
            }

            ///NEW METHOD - place before campaign links
            if (!Is3rdParty)
            {
                if ((includeViewAndOptOutLinks) && this.PostedFileName.Trim().Length > 0)
                {
                    string publishLink = "<table id=\"publishTable\" style=\"width:97%;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td class=\"publishcell\" style=\"font-size:14px;color:#333333;padding-bottom:10px;\">";
                    publishLink += string.Format("If you are having problems reading this email, please visit <a href=\"{0}\" class=\"publishlink\" style=\"color:#000001;\">{1}</a></td></tr></table>",
                        System.Web.HttpUtility.HtmlEncode(Utils.ParseHelper.FormatUrlFromString(this.PublishedPathAndFile_Url, true, false)),
                        this.EmailLetterRecord.Name);

                    //if the template has a place where it has specified this link to go....
                    if (content.IndexOf(publishLinkOnlyPlaceholder) != -1)
                    {
                        content = content.Replace(publishLinkOnlyPlaceholder,
                            string.Format("If you are having problems reading this email, please visit <a href=\"{0}\" class=\"publish-link\">{1}</a>",
                            System.Web.HttpUtility.HtmlEncode(Utils.ParseHelper.FormatUrlFromString(this.PublishedPathAndFile_Url, true, false)),
                            this.EmailLetterRecord.Name
                            ));
                    }
                    else if (content.IndexOf(publishLinkPlaceholder) != -1)
                    {
                        content = content.Replace(publishLinkPlaceholder, publishLink);
                    }
                    //otherwise - just plop into the constructed doc
                    else
                    {
                        sb.AppendLine(publishLink);
                    }
                }

                
                //configure links if necessary - otherwise, no changes
                if (useCampaignLinks)
                    content = ConfigureBodyLinks_Html(content);

                //images and hrefs in emails should have non-relative links
                //3rd party already does this
                if (convertVirtualLinksToUrls)
                    content = ConvertImgVirtualPathsToAbsolutePaths(content);
                //content = ConvertHrefVirtualPathsToAbsolutePaths(content);
            }

            sb.Append(content);

            if (!Is3rdParty)
            {
                if (includeViewAndOptOutLinks)
                {
                    if (content.IndexOf(optoutInfoPlaceholder) != -1)
                    {
                        sb.Replace(optoutInfoPlaceholder, this.AddOptOut("htmlnostyle"));
                    }
                    else
                    {
                        string optout = this.AddOptOut("html");

                        if (content.IndexOf(optoutPlaceholder) != -1)
                            sb.Replace(optoutPlaceholder, optout);
                        else
                            sb.AppendLine(optout);
                    }
                }
            }

            if (insertHtmlStructure)
                sb.Append("</body></html>");

            return sb.ToString();
        }

        /// <summary>
        /// Creates a plain text view email and adds "cannot view" and opt-out links
        /// </summary>
        /// <param name="useComments">Comments are not necessary for a plain text view</param>
        /// /// <param name="UseCampaignLinks">emailLetter object does not get the conversion</param>
        public string CreateShell_Text(bool useComments, bool useCampaignLinks)
        {
            if (this.EmailLetterRecord.TextVersion == null || this.EmailLetterRecord.TextVersion.Trim().Length == 0 ||
                this.EmailLetterRecord.TextVersion == _Enums.MailTemplateTypes.Is3rdPartySender.ToString())
                return string.Empty;

            string domainName = _Config._DomainName;

            StringBuilder sb = new StringBuilder();

            if (useComments)
                sb.AppendFormat("<!--{0}", Constants.NewLine);

            //this can be found in the meta tag of the html
            //sb.AppendFormat("SubscriptionEmailId={1}{0}", Constants.NewLines(2), this.Id);

            sb.AppendFormat("{1}{0}", Constants.NewLines(2), this.EmailLetterRecord.Subject.Trim());

            //insert cannot view link
            //note that we are using encoding here - to help with cut and paste
            //the rest of the TEXT email should remain as is
            sb.AppendFormat("If you are having problems reading this email, \r\nplease visit {0}{1}",
                System.Web.HttpUtility.HtmlEncode(this.PublishedPathAndFile_Url), Constants.NewLines(2));

            //content
            sb.AppendFormat("{0}{1}", (useCampaignLinks) ? ConfigureBodyLinks_Text(this.EmailLetterRecord.TextVersion) : this.EmailLetterRecord.TextVersion, 
                Constants.NewLines(2));

            //insert optout link
            sb.AppendFormat(this.AddOptOut("text"));

            if (useComments)
                sb.AppendFormat("{0}-->{0}", Constants.NewLine);

            return sb.ToString();
        }


        private string AddOptOut(string context) 
        {
            string _result = string.Empty;

            string opt1 = "You have received this email either by purchasing tickets to one of our events from our ticketing provider or by signing up for our newsletter.";
            string opt2 = "To update your email preferences, please visit {0}.";
            string opt3 = "To unsubscribe from future email communications, please visit {0} or send an email to {1}. You may also write us at {2} (please allow 10 business days for your request to be processed)."; 

            string manage = string.Format("http://{0}/mailermanage", _Config._DomainName);
            string unsubscribe = string.Format("http://{0}/unsubscribe", _Config._DomainName);


            if (context != null && context.ToLower() == "text")
            {
                _result = string.Format("{0}{1}{0}{2}{0}{3}{0}",
                    Constants.NewLines(2),
                    opt1,
                    string.Format(opt2, manage),
                    string.Format(opt3, unsubscribe, _Config._CustomerService_Email, _Config._Site_Entity_PhysicalAddress.Replace("803", "80&#8203;3"))
                    );
            }
            else
            {
                _result = string.Format("<table {0} cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td {1}>",
                    (context.ToLower() == "htmlnostyle") ? "class=\"optout-table\"" : "id=\"optoutTable\" style=\"width:97%;\" ",
                    (context.ToLower() == "htmlnostyle") ? "class=\"optcell\"" : "class=\"optoutcell\" style=\"font-size:14px;color:#333333;\""
                    );

                _result += string.Format("<div style=\"margin:0;padding-bottom:10px;\">{0}</div><div style=\"margin:0;padding-bottom:10px;\">{1}</div><div style=\"margin:0;padding-bottom:10px;\">{2}</div></td></tr></table>",
                    opt1,
                    string.Format(opt2, string.Format("our <a href=\"{0}\">mail manager</a>", manage)),
                    string.Format(opt3, 
                        string.Format(" our <a href=\"{0}\">unsubscribe</a> page", unsubscribe),
                        string.Format("<a href=\"mailto:{0}?subject=unsubscribe\">customer service</a>", _Config._CustomerService_Email),
                        _Config._Site_Entity_PhysicalAddress.Replace("803", "803&#8203;"))//add non-space char to trick ios into not making a link - zip should work for bt and fox
                    );
            }

            return _result;
        }


        #region LinkHandling

        ////TODO: work out css and other attachment besides images
        private string ConvertImgVirtualPathsToAbsolutePaths(string content)
        {
            content = content.Replace("src=\"/", "~&**)%");
            content = content.Replace("src='/", "~&**)%");
            return content.Replace("~&**)%", string.Format("src=\"http://{0}/", _Config._DomainName));
        }

        private string ConvertHrefVirtualPathsToAbsolutePaths(string content)
        {
            content = content.Replace("href=\"/", "~&**)%");
            content = content.Replace("href='/", "~&**)%");
            return content.Replace("~&**)%", string.Format("href=\"http://{0}/", _Config._DomainName));
        }


        private string ConfigureBodyLinks_Html(string txt)
        {
            //Debug.WriteLine(url);

            Regex getLinks2 = new Regex(@"(?<linkStart> HREF\s*=\s*[""']?)(?<linkProper>[^#'"">]+?)(?<linkEnd>[ '""]+?)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string result = getLinks2.Replace(txt, new MatchEvaluator(this.EvaluateLinkForCampaign));

            //Debug.WriteLine(result);

            return result;
        }

        //TODO
        private string ConfigureBodyLinks_Text(string txt)
        {
            Regex getLinks = new Regex(@"((http(s)?://))(?<linkProper>[^'"" >]+?)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            string result = getLinks.Replace(txt, new MatchEvaluator(this.EvaluateLinkForCampaign));

            return result;
        }

        //helps with tracking links
        private string EvaluateLinkForCampaign(Match m)
        {
            //we have start and end because the quoting could be different
            string start = m.Groups["linkStart"].Value;
            string link = m.Groups["linkProper"].Value.TrimEnd('.');
            string end = (m.Groups["linkEnd"] != null) ? m.Groups["linkEnd"].Value : string.Empty;

            Regex storeSite = new Regex(string.Format(@"{0}", Wcss._Config._DomainName.Replace("www.", string.Empty)));
            Match isStoreLink = storeSite.Match(link);

            //only append a querystring to those links on our site
            string newLink = string.Empty;
            

            //TODO revisit
            if (link.IndexOf("mailto:", StringComparison.OrdinalIgnoreCase) != -1)
            {
                //dont include the other "qs" var for a subject - just add the name
                newLink = string.Format("{0}{1}{2}{3}{4}",
                    start,
                    link,
                    //add the ? to begin the qs portion
                    (link.IndexOf("?") == -1) ? "?" : string.Empty,
                    //if there is no subject - add one
                    (link.IndexOf("subject=", StringComparison.OrdinalIgnoreCase) == -1) ? string.Format("subject={0}", this.EmailLetterName) : string.Empty,
                    end);
            }
            else
            {
                Regex z2Site = new Regex(@"z2ent.com/", RegexOptions.IgnoreCase);
                Match isZ2Link = z2Site.Match(link);

                //we only come here when not 3rd party - being sent as fox
                //in that case we leave the z2 links as is
                //See Utils.UrlHelper to see how z2 links are handled as a 3rd party sender

                newLink =
                    (isStoreLink.Success) ?
                    string.Format("{0}{1}{2}{3}{4}", start, link, (link.IndexOf("?") != -1) ? "&" : "?", GoogleCampaignQS, end) :
                    (isZ2Link.Success) ?
                    m.Value :
                    string.Format("{0}http://{1}/{2}{3}", start, Wcss._Config._DomainName, DirectorCampaignLink(link), end);
            }

            return newLink;
        }
        
        public string GoogleCampaignQS
        {
            get
            {
                return string.Format("utm_source=WctMlr&utm_medium=email&utm_campaign={0}",
                    System.Web.HttpUtility.HtmlEncode(this.EmailLetterName));
            }
        }
        

        private string DirectorCampaignLink(string link)
        {
            return string.Format("Sd.aspx?{0}={1}&url={2}&{3}",
                "seid", this.Id.ToString(), System.Web.HttpUtility.UrlEncode(link), GoogleCampaignQS);
        }

        #endregion
    }
}
