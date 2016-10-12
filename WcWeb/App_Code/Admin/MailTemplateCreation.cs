using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Reflection;

using Wcss;

namespace wctMain.Admin
{
    /// <summary>
    /// Summary description for MailTemplateCreation
    /// </summary>
    public class MailerTemplateCreation
    {
        private Mailer _mailer = null;
        public Mailer MailerObject { get { return _mailer; } set { _mailer = value;  } }
        public StringBuilder _textVersion;
        public StringBuilder _htmlVersion;

        #region TagNames

        public static readonly string replacementTag = "{0}";//
        public const string tagRepeat_Start = "<REPEAT>";//
        public const string tagRepeat_End = "</REPEAT>";//
        public const string tagTitle = "<TITLE>";//
        public const string tagContent = "<CONTENT>";//
        public const string tagHasContentStart = "<HASCONTENT>";//
        public const string tagHasContentEnd = "</HASCONTENT>";//
        public const string tagContentExistsStart = "<HASCONTENT=TRUE>";//
        public const string tagContentExistsEnd = "</HASCONTENT=TRUE>";//
        public const string tagDate = "<DATE>";//
        public const string tagHeader = "<HEADER>";//
        public const string tagHeadliner = "<HEADLINER>";//
        public const string tagImageHeightMax = "<IMAGEHEIGHTMAX>";//        
        public const string tagOpener = "<OPENER>";//
        public const string tagPromoter = "<PROMOTER>";//
        public const string tagShowImageUrl = "<SHOWIMAGEURL>"; //
        public const string tagEventUrl = "<EVENTURL>";//
        public const string tagShowTitle = "<SHOWTITLE>";//
        public const string tagStatus = "<STATUS>";        
        public const string tagStatStart = "<STAT>";
        public const string tagStatEnd = "</STAT>"; 
        public const string tagTimes = "<TIMES>";//
        public const string tagVenue = "<VENUE>";//
        public const string tagAges = "<AGES>";//
        public const string tagPricing = "<PRICING>";//
        public const string tagTextStart = "<TEXT_";//
        public const string tagSocial = "<SOCIAL>";//

        public const string tagContest = "<CONTESTTEXT>";//
        public const string tagPreHeader = "<PREHEADERCONTENT>";//
        public const string tagAlert = "<ALERT>";
        public const string tagExtra = "<EXTRA>";

        #endregion
                
        public MailerTemplateCreation(Mailer mailer) 
        {
            //Exit if no mailer
            if (mailer == null)
                return;

            _mailer = new Mailer();
            _mailer.CopyFrom(mailer);

            _textVersion = new StringBuilder();
            _htmlVersion = new StringBuilder();
        }

        public void CreateMailerPreview()
        {
            if (MailerObject != null)
            {
                MailerTemplate mailerTemplate = MailerObject.MailerTemplateRecord;
                //get the style
                string _style = mailerTemplate.Style;
                //get the header
                string _header = mailerTemplate.Header;

                //text version ignores footer, header and style - but we stick in a line or 2 for better readability
                //subject before preheader
                _textVersion.AppendLine();
                _textVersion.AppendLine();
                _textVersion.AppendLine(this.MailerObject.Subject);
                _textVersion.AppendLine();
                _textVersion.AppendLine();
                
                
                //PREHEADER - place it in here prior to the header
                MailerContentCollection preheaders = new MailerContentCollection();
                preheaders.AddRange(this.MailerObject.MailerContentRecords().GetList()
                    .FindAll(delegate(MailerContent match) { return (match.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.preheader); }));

                if (preheaders.Count > 1)
                    preheaders.GetList().Sort(new Utils.Reflector.CompareEntities<MailerContent>(Utils.Reflector.Direction.Ascending, "DisplayOrder"));

                foreach (MailerContent mContent in preheaders)
                {
                    CreateMailerSection(mContent);
                }
                

                //HEADER
                _htmlVersion.Append(_header);


                //MAIN
                MailerContentCollection coll = new MailerContentCollection();
                coll.AddRange(this.MailerObject.MailerContentRecords().GetList()
                    .FindAll(delegate(MailerContent match) { return (match.MailerTemplateContentRecord.TemplateAsset != MailerTemplateContent.ContentAsset.preheader); } ));
                
                if (coll.Count > 1)
                    coll.GetList().Sort(new Utils.Reflector.CompareEntities<MailerContent>(Utils.Reflector.Direction.Ascending, "DisplayOrder"));

                foreach (MailerContent mContent in coll)
                {
                    CreateMailerSection(mContent);
                }


                //FOOTER - ignore in text?
                string _footer = mailerTemplate.Footer;
                _htmlVersion.Append(_footer);


                //3RD PARTY SENDERS - essentially mailer 2.0
                //no hosted version
                //no txt version
                //take care of abs urls
                //google links will not be included - handled elsewhere
                if (mailerTemplate.Is3rdPartySender)
                {
                    _textVersion.Length = 0;
                    _textVersion.Append(_Enums.MailTemplateTypes.Is3rdPartySender.ToString());

                    //ENSURE ABSOLUTES
                    Utils.UrlHelper _urlr = new Utils.UrlHelper(_Config._DomainName);
                    string html = _urlr.EnsureAbsoluteUrls(_htmlVersion.ToString());

                    MailerTemplateContent sitedirector = mailerTemplate.MailerTemplateContentRecords().GetList()
                        .Find(delegate (MailerTemplateContent match) { return (                       match.TemplateAsset == MailerTemplateContent.ContentAsset.sitedirector); });

                    //prepend the site director
                    if (sitedirector != null)
                        html = _urlr.PrependToUrl(html, sitedirector.Title);

                    _htmlVersion.Clear();
                    _htmlVersion.Append(html);

                }
            }
        }

        /// <summary>
        /// appends replaced content to html version
        /// </summary>
        /// <param name="mailerContent"></param>
        private void CreateMailerSection(MailerContent mailerContent)
        {
            if (mailerContent.IsActive)
            {
                //init template sections
                string _templateStart = GetTemplatePart("start", mailerContent);
                string _repeatableTemplate = GetTemplatePart("repeat", mailerContent);
                string _templateMiddle = string.Empty;
                string _templateEnd = GetTemplatePart("end", mailerContent);

                //title is only in start template
                if (_templateStart.Trim().Length > 0)
                {
                    _templateStart = ReplacePreheaderElement(_templateStart, mailerContent);
                    _templateStart = ReplaceTitleElement(_templateStart, mailerContent);
                    _templateStart = ReplaceContestElement(_templateStart, mailerContent);
                    _templateStart = ReplaceTextElements(_templateStart, mailerContent);
                }
                
                //content will only reside in start or end templates
                if (_repeatableTemplate.TrimEnd().Length > 0)
                {
                    _templateMiddle = LoopRepeatableContent(_repeatableTemplate, _templateMiddle, mailerContent);
                }

                if (_templateEnd.Trim().Length > 0)
                    _templateEnd = ReplaceContentElement(ReplaceTextElements(_templateEnd, mailerContent), mailerContent);
                else if (_templateStart.Trim().Length > 0)
                    _templateStart = ReplaceContentElement(_templateStart, mailerContent);

                _htmlVersion.Append(_templateStart);
                _htmlVersion.Append(_templateMiddle);
                _htmlVersion.Append(_templateEnd);
            }
        }

        private string ReplacePreheaderElement(string template, MailerContent mailerContent)
        {
            string copy = String.Copy(template);
            string _prehead = string.Empty;

            if (mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.preheader)
            {
                if (copy.IndexOf(tagPreHeader) != -1 && mailerContent.PreheaderText != null && mailerContent.PreheaderText.Trim().Length > 0)
                {
                    MailerTemplateSubstitution _ctext = mailerContent.MailerTemplateContentRecord.MailerTemplateSubstitutionRecords().GetList()
                        .Find(delegate(MailerTemplateSubstitution match) { return (match.TagName == tagPreHeader); });

                    _prehead = (_ctext.TagValue != null && _ctext.TagValue.Trim().Length > 0) ?
                        _ctext.TagValue.Replace(replacementTag, System.Web.HttpUtility.HtmlEncode(mailerContent.PreheaderText.Trim())) :
                        mailerContent.PreheaderText.Trim();

                    _textVersion.AppendLine(FormatForText(mailerContent.PreheaderText.Trim()));
                    _textVersion.AppendLine();
                }
            }

            return copy.Replace(tagPreHeader, Utils.ParseHelper.EscCommonSequencesInHtml_AvoidInnerHtml(_prehead));
        }

        private string ReplaceTitleElement(string template, MailerContent mailerContent)
        {
            string copy = String.Copy(template);

            string _title = (mailerContent.Title != null && mailerContent.Title.Trim().Length > 0) ? mailerContent.Title.Trim() : string.Empty;
            _textVersion.AppendLine(FormatForText(_title));
            _textVersion.AppendLine();

            MailerTemplateSubstitution subTitle = mailerContent.MailerTemplateContentRecord.MailerTemplateSubstitutionRecords().GetList()
                .Find(delegate(MailerTemplateSubstitution match) { return (match.TagName == tagTitle); });

            //if no tag then dont replace
            if (subTitle != null && subTitle.TagValue.Trim().Length > 0)
            {
                string tagValue = subTitle.TagValue.Trim();
                string replacement = tagValue.Replace(replacementTag, System.Web.HttpUtility.HtmlEncode(_title));
                _title = replacement;
            }

            return copy.Replace(tagTitle, Utils.ParseHelper.EscCommonSequencesInHtml_AvoidInnerHtml(_title));
        }

        private string ReplaceContestElement(string template, MailerContent mailerContent)
        {
            string copy = String.Copy(template);
            string _contest = string.Empty;

            if (mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.contest)
            {
                if(copy.IndexOf(tagContest) != -1 && mailerContent.ContestText != null && mailerContent.ContestText.Trim().Length > 0)
                {
                    MailerTemplateSubstitution _ctext = mailerContent.MailerTemplateContentRecord.MailerTemplateSubstitutionRecords().GetList()
                        .Find(delegate(MailerTemplateSubstitution match) { return (match.TagName == tagContest); });

                    _contest = (_ctext.TagValue != null && _ctext.TagValue.Trim().Length > 0) ?
                        //_ctext.TagValue.Replace(replacementTag, System.Web.HttpUtility.HtmlEncode(mailerContent.ContestText.Trim())) :
                        _ctext.TagValue.Replace(replacementTag, mailerContent.ContestText.Trim()) :
                        mailerContent.ContestText.Trim();

                    _textVersion.AppendLine(FormatForText(mailerContent.ContestText.Trim()));
                    _textVersion.AppendLine();
                }
            }

            return copy.Replace(tagContest, Utils.ParseHelper.EscCommonSequencesInHtml_AvoidInnerHtml(_contest));
        }

        private string ReplaceTextElements(string template, MailerContent mailerContent)
        {
            string templateCopy = String.Copy(template);

            // find the list of substitutes that begin with <TEXT_
            List<MailerTemplateSubstitution> list = new List<MailerTemplateSubstitution>();
            list.AddRange(mailerContent.MailerTemplateContentRecord.MailerTemplateSubstitutionRecords()
                .GetList().FindAll(delegate(MailerTemplateSubstitution match) { return (match.TagName.StartsWith(tagTextStart) ); } ));

            if (list.Count > 0)
            {
                foreach (MailerTemplateSubstitution sub in list)
                {
                    if (templateCopy.IndexOf(sub.TagName) != -1)
                    {
                        //place the value into the text version
                        _textVersion.AppendLine(sub.TagValue);
                        _textVersion.AppendLine();

                        //remove the tag from the template - we are done with it
                        templateCopy = templateCopy.Replace(sub.TagName, string.Empty);
                    }
                }
            }

            return templateCopy;
        }

        private string LoopRepeatableContent(string template, string allContent, MailerContent mailerContent)
        {
            int i = 0;

            string separator = mailerContent.MailerTemplateContentRecord.SeparatorTemplate;
            if (separator != null && separator.Trim().Length == 0)
                separator = null;

            //fill intended collections
            string flags = mailerContent.MailerTemplateContentRecord.VcFlags;            

            ShowEventCollection showEvents = new ShowEventCollection();
            showEvents.AddRange(mailerContent.ShowEventRecords.GetList().FindAll(delegate(ShowEvent match) { return (match.IsActive); } ));

            List<MailerShow> mailershows = new List<MailerShow>();
            mailershows.AddRange(mailerContent.MailerShowList);

            //if we have mailer shows - ensure social cals
            if (mailershows.Count > 1)
            {
                foreach (MailerShow ms in mailershows)
                {
                    //create or update info
                    wctMain.Controller.Api.iCalController.CreateCalendarIcs(ms);
                }
            }

            //contest section
            List<MailerBanner> mailerbanners = new List<MailerBanner>();
            mailerbanners.AddRange(mailerContent.MailerBannerList);


            //if we have a simple show list
            if (flags != null && flags == SimpleShow.flagString)
            {
                List<SimpleShow> list = SimpleShow.ShowList(mailerContent.VcContent);
                foreach (SimpleShow simp in list)
                {
                    allContent += ReplaceConditionalContent(ReplaceRepeatElement(template, mailerContent, simp));

                    if (i++ < (list.Count - 1)) 
                    {
                        _textVersion.AppendLine();

                        if(separator != null && separator.TrimEnd().Length > 0)
                            allContent += separator;
                    }
                }
            }
            else if (showEvents.Count > 0)
            {
                showEvents.Sort("IOrdinal", true);
                int showCount = showEvents.Count;

                foreach(ShowEvent se in showEvents)
                {
                    allContent += ReplaceConditionalContent(ReplaceRepeatElement(template, mailerContent, se));

                    if (i++ < (showCount - 1))
                    {
                        _textVersion.AppendLine();

                        if (separator != null && separator.TrimEnd().Length > 0)
                            allContent += separator;
                    }
                }
            }
            else if (mailershows.Count > 0)
            {
                mailershows.Sort(new Utils.Reflector.CompareEntities<MailerShow>(Utils.Reflector.Direction.Ascending, "Ordinal"));
                int count = mailershows.Count;

                foreach (MailerShow obj in mailershows)
                {
                    allContent += ReplaceConditionalContent(ReplaceRepeatElement(template, mailerContent, obj));

                    if (i++ < (count - 1))
                    {
                        _textVersion.AppendLine();

                        if (separator != null && separator.TrimEnd().Length > 0)
                            allContent += separator;
                    }
                }
            }
            else if (mailerbanners.Count > 0)
            {
                mailerbanners.Sort(new Utils.Reflector.CompareEntities<MailerBanner>(Utils.Reflector.Direction.Ascending, "Ordinal"));
                int count = mailerbanners.Count;

                foreach (MailerBanner obj in mailerbanners)
                {
                    allContent += ReplaceConditionalContent(ReplaceRepeatElement(template, mailerContent, obj));

                    if (i++ < (count - 1))
                    {
                        _textVersion.AppendLine();

                        if (separator != null && separator.TrimEnd().Length > 0)
                            allContent += separator;
                    }
                }
            }

            return allContent;
        }

        private string ReplaceConditionalContent(string repeatElement)
        {
            //replace conditional content
            if (repeatElement.IndexOf(tagHasContentStart) != -1 && repeatElement.IndexOf(tagHasContentEnd) != -1 &&
                repeatElement.IndexOf(tagContentExistsStart) != -1 && repeatElement.IndexOf(tagContentExistsEnd) != -1)
            {
                int start = repeatElement.IndexOf(tagHasContentStart);
                int end = repeatElement.IndexOf(tagHasContentEnd);

                if (start + tagHasContentStart.Length == end)
                {
                    repeatElement = repeatElement.Remove(repeatElement.IndexOf(tagContentExistsStart), 
                        repeatElement.IndexOf(tagContentExistsEnd) + tagContentExistsStart.Length + 1 - repeatElement.IndexOf(tagContentExistsStart));
                }
                else
                {
                    repeatElement = repeatElement.Replace(tagContentExistsStart, string.Empty);
                    repeatElement = repeatElement.Replace(tagContentExistsEnd, string.Empty);
                }
            }

            return repeatElement;
        }
        
        /// <summary>
        /// We must rely on the elements here - no freebies - OT FOR WAGATAIL OR ANYTHING WITH A SIMPLESHOW
        /// an element will only be replaced if it has a substitution element
        /// </summary>
        /// <param name="template"></param>
        /// <param name="mailerContent"></param>
        /// <returns></returns>
        private string ReplaceRepeatElement(string template, MailerContent mailerContent, object showObject)
        {
            if(showObject == null)
                return string.Empty;

            MailerShow mlrshow = showObject as MailerShow;
            MailerBanner mlrbanner = showObject as MailerBanner;

            string templateCopy = String.Copy(template);

            foreach (MailerTemplateSubstitution sub in mailerContent.MailerTemplateContentRecord.MailerTemplateSubstitutionRecords())
            {
                string tagName = sub.TagName;

                //do not do TITLE and CONTENT in here
                if (tagName != tagTitle && tagName != tagContent)
                {
                    //TODO: make this an input
                    //bool displayDatesAsRange = false;
                    string replacement = string.Empty;
                    string tagValue = sub.TagValue;
                    string showValue = string.Empty;

                    //special case
                    if (tagName == tagImageHeightMax)
                    {
                        //if the image height is greater than 120px - then set to 120px                        
                        int max = int.Parse(tagValue);
                        
                        try
                        {
                            System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(((Show)showObject).ImageManager.Thumbnail_Small));

                            if ((int)p.Second > max)
                                max = (int)p.Second;
                        }
                        catch (Exception) { }

                        replacement = max.ToString();
                    }
                    else if (mlrshow != null || mlrbanner != null)
                    {
                        showValue = GetShowValueFromTag(tagName, showObject);

                        
                        //HANDLED IN ELSE CASE!!!!
                        //if showValue is null and we are trying to get the link to the tickets
                        //then try again using the site url as a replacement
                        //and change the link wording to "More Info"
                        // aka Fallback to siteurl/facebookeventurl (for chat shows)
                        if (mlrshow != null && tagName == "<TIXURL>" && (showValue == null || showValue.Trim().Length == 0))
                        {
                            //if it's a chautauqua show - then get from facebook
                            if(mlrshow.Venue != null && mlrshow.Venue.IndexOf("Chautauqua") != -1)
                            {
                                showValue = GetShowValueFromTag("<FACEBOOKEVENTURL>", showObject);
                            }
                            else
                                showValue = GetShowValueFromTag("<SITEURL>", showObject);

                            if (showValue != null && showValue.TrimEnd().Length > 0)
                            {
                                if (mlrshow.Venue != null && mlrshow.Venue.IndexOf("Chautauqua") != -1)
                                {
                                    replacement = tagValue.Replace(replacementTag, showValue);
                                }
                                else
                                {
                                    replacement = tagValue.Replace(replacementTag, showValue).Replace("Tickets!", "More Info");
                                }
                                
                            }
                        }
                        else if (showValue != null && showValue.TrimEnd().Length > 0)
                        {
                            replacement = tagValue.Replace(replacementTag, showValue);

                            if (tagName != tagSocial && tagName != "<DATEONSALE>" && tagName != "<SITEURL>" && tagName != "<IMAGEURL>" && tagName != "<AGES>")
                            {
                                if (tagName == "<DATE_DAY>" || tagName == "<DATE_MONTH>")
                                    _textVersion.AppendFormat("{0} ", FormatForText(showValue));
                                else if (tagName == "<TIXURL>")
                                {
                                    _textVersion.AppendFormat("Buy Tickets: {0}", FormatForText(showValue));
                                    _textVersion.AppendLine();
                                }
                                else
                                    _textVersion.AppendLine(FormatForText(showValue));
                            }
                        }
                    }
                    else
                    {
                        SimpleShow simpleShow = (showObject.GetType().ToString() == "Wcss.SimpleShow") ? (SimpleShow)showObject : null;
                        ShowEvent showEvent = (showObject.GetType().ToString() == "Wcss.ShowEvent") ? (ShowEvent)showObject : null;

                        if (showEvent != null)
                        {
                            //handle dates separately
                            if (tagName == tagDate && tagValue.IndexOf(SimpleShow.flagSeparator) != -1)
                            {
                                //get tagValue pieces
                                string[] pieces = tagValue.Split(SimpleShow.flagSeparator.ToCharArray());
                                string dte = pieces[0];
                                string sta = pieces[1];

                                string configured = showEvent.DateString.Trim();

                                //if there is a status in the string - replace
                                if (showEvent.DateString.IndexOf(MailerContent.tagDateStatusStart) != -1)
                                {
                                    string[] seps = { replacementTag };
                                    string[] statiiParts = sta.Split(seps, StringSplitOptions.RemoveEmptyEntries);
                                    string start = statiiParts[0];
                                    string end = statiiParts[1];

                                    configured = configured.Replace(MailerContent.tagDateStatusStart, start)
                                        .Replace(MailerContent.tagDateStatusEnd, end);
                                }

                                if (configured != null && configured.Trim().Length > 0)
                                {
                                    showValue = configured;
                                    replacement = showValue;
                                }
                            }
                            else
                            {
                                showValue = GetShowValueFromTag(tagName, showObject);

                                if (showValue != null && showValue.TrimEnd().Length > 0)
                                {
                                    //remove any formatting 
                                    //showValue = showValue, true);
                                    replacement = tagValue.Replace(replacementTag, showValue);
                                }
                            }
                        }
                        else//it is a simple show
                        {
                            showValue = GetShowValueFromTag(tagName, showObject);

                            if (showValue != null && showValue.TrimEnd().Length > 0)
                            {
                                //remove any formatting 
                                //showValue = showValue;

                                //if we are the opener tag and are in the upcoming shows section
                                //we need to remove special guest - this is going to bite us sometimes though
                                //on 2 day runs, etc where we have diff acts each day
                                if (tagName == tagOpener && mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.showlinear)
                                    showValue = ShowDisplay.RemoveSpecialGuest(showValue);

                                replacement = tagValue.Replace(replacementTag, showValue);
                            }
                        }

                        //TEXT VERSION
                        //if (tagName != tagShowId && tagName != tagShowDisplayUrl && showValue != null && showValue.TrimEnd().Length > 0)
                        if (tagName != tagContest && tagName != tagEventUrl && tagName != tagShowImageUrl && showValue != null && showValue.TrimEnd().Length > 0)
                        {
                            //some sections should not write to new line for every value
                            if (mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.show)
                                _textVersion.AppendLine(FormatForText(showValue));
                            else if (mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.showlinear ||
                                mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.simple)
                                _textVersion.Append(string.Format("{0} ", FormatForText(showValue)));
                        }
                    }

                    templateCopy = templateCopy.Replace(tagName, Utils.ParseHelper.EscCommonSequencesInHtml_AvoidInnerHtml(replacement,true));

                }
            }

            return templateCopy;
        }
        
        private string FormatForText(string format)
        {
            return Utils.ParseHelper.StripHtmlTags(format).Replace(@"\r\n", Environment.NewLine).Replace(@"\t", string.Empty)
                .Replace("&#39;", "'").Replace("&amp;", "&").Replace("&ndash;", "-").Replace("&nbsp;", "-").Trim();
        }

        private string ReplaceContentElement(string template, MailerContent mailerContent)
        {
            string copy = String.Copy(template);

            string _content = (mailerContent.MailerTemplateContentRecord.TemplateAsset != MailerTemplateContent.ContentAsset.contest && 
                mailerContent.VcContent != null && mailerContent.VcContent.Trim().Length > 0) ? 
                mailerContent.VcContent.Trim() : 
                (mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.editor && 
                mailerContent.VcJsonContent != null && mailerContent.VcJsonContent.Trim().Length > 0) ?
                mailerContent.VcJsonContent.Trim() : 
                string.Empty;


            string flags = mailerContent.MailerTemplateContentRecord.VcFlags;

            if(flags == null || flags.Trim().Length == 0)
            {
                _textVersion.AppendLine(FormatForText(_content));
                _textVersion.AppendLine();
            }

            MailerTemplateSubstitution subContent = mailerContent.MailerTemplateContentRecord.MailerTemplateSubstitutionRecords().GetList()
                .Find(delegate(MailerTemplateSubstitution match) { return (match.TagName == tagContent); });

            //if no tag then dont replace
            //content should be encoded already
            if (subContent != null && subContent.TagValue.Trim().Length > 0)
            {   
                string tagValue = subContent.TagValue.Trim();
                string replacement = tagValue.Replace(replacementTag, _content);
                _content = replacement;
            }

            return copy.Replace(tagContent, Utils.ParseHelper.EscCommonSequencesInHtml_AvoidInnerHtml(_content));
        }
        
        private string GetTemplatePart(string context, MailerContent mailerContent)
        {
            string template = mailerContent.MailerTemplateContentRecord.Template;
            int rptStart = template.IndexOf(tagRepeat_Start);
            string[] seps = {tagRepeat_Start, tagRepeat_End};

            switch (context.ToLower())
            {
                case "start":
                    if (rptStart > -1)
                    {
                        string[] parts = template.Split(seps, StringSplitOptions.None); 
                        return parts[0];
                    }
                    return template;
                    break;
                case "repeat":
                    if (rptStart > -1)
                    {
                        string[] parts = template.Split(seps, StringSplitOptions.None); 
                        return parts[1];
                    }
                    break;
                case "end":
                    if (rptStart > -1)
                    {
                        string[] parts = template.Split(seps, StringSplitOptions.None); 
                        return parts[2];
                    }
                    break;
            }

            return string.Empty;
        }
        
        private string GetShowValueFromTag(string tagName, object showObject)
        {
            // the object will be one of three things
            SimpleShow simple = (showObject.GetType().ToString() == "Wcss.SimpleShow") ? (SimpleShow)showObject : null;
            ShowEvent showEvent = (showObject.GetType().ToString() == "Wcss.ShowEvent") ? (ShowEvent)showObject : null;
            Show show = (showObject.GetType().ToString() == "Wcss.Show") ? (Show)showObject : null;
            MailerShow mlrshow = showObject as MailerShow;
            MailerBanner mlrbanner = showObject as MailerBanner;
            
            //Return Value
            string showValue = string.Empty;

            if (tagName == tagSocial && mlrshow != null)
            {
                //build the social content for a milershow object
                StringBuilder sb = new StringBuilder();

                ShowDisplaySocials sosh = new ShowDisplaySocials(mlrshow);
                sb.AppendFormat("{0}", sosh.SocialOutputMailer());

                if (sb.Length > 0)
                {
                    sb.Insert(0, "<div class=\"mlr-social\">");
                    sb.AppendLine();
                    sb.AppendLine("</div>");
                }

                showValue = sb.ToString();
            }
            else if(mlrshow != null || mlrbanner != null)
            {
                string tag = tagName.Trim(new char[] { '<', '>' });

                //names much match properties!!
                Type t = (mlrshow != null) ? mlrshow.GetType() : mlrbanner.GetType();

                PropertyInfo p = t.GetProperty(tag, BindingFlags.Public | BindingFlags.Instance|BindingFlags.IgnoreCase);

                if(p != null)
                {
                    string res = (mlrshow != null) ? p.GetValue(mlrshow, null).ToString() : p.GetValue(mlrbanner, null).ToString();

                    return res;
                }
                
                return string.Empty;
            }
            else 
            {
                //establish show url from given object
                string showUrl = string.Empty;

                if(show != null)
                    showUrl = show.FirstShowDate.ConfiguredUrl;
                else if(showEvent != null)
                {
                    Show s = Show.FetchByID(showEvent.ParentId);
                    if(s != null)
                        showUrl = s.FirstShowDate.ConfiguredUrl;
                }
                else if(simple != null)
                {
                    Show s = Show.FetchByID(simple.IDX);
                    if(s != null)
                        showUrl = s.FirstShowDate.ConfiguredUrl;
                }

                bool useShow = show != null;
                bool useShowEvent = showEvent != null;

                if(showObject != null)
                {
                    //ignore title and content tags here
                    switch (tagName)
                    {
                        case tagAges:
                            showValue = (useShow) ? show.Display.Mailer_Ages_NoMarkup.Trim() : (useShowEvent) ? showEvent.Ages : null;
                            break;
                        case tagDate:
                            showValue = (useShow) ? show.Display.Date_Markup_3Day_NoTime_ListAll.Trim() : (useShowEvent) ? showEvent.DateString : simple.DATE;
                            if (showValue.IndexOf(tagStatStart) != -1 && showValue.IndexOf(tagStatStart) != -1)
                            {
                                showValue = showValue.Replace(tagStatStart, "<span class=\"status\">");
                                showValue = showValue.Replace(tagStatEnd, "</span>");
                            }
                            break;
                        case tagHeader:
                            showValue = (useShow) ? show.ShowHeader_Derived.Trim() : (useShowEvent) ? showEvent.Header : null;
                            break;
                        case tagHeadliner:
                            showValue = (useShow) ? show.Display.Headliners_NoMarkup_Verbose_NoLinks : (useShowEvent) ? showEvent.Headliner : simple.HEADLINER;
                            break;
                        case tagOpener:
                            showValue = (useShow) ? show.Display.Openers_NoMarkup_Verbose_NoLinks.Trim() : (useShowEvent) ? showEvent.Opener : simple.OPENER;
                            break;
                        case tagPromoter:
                            showValue = (useShow) ? show.Display.Promoters_NoMarkup_NoLinks : (useShowEvent) ? showEvent.Promoter : null;
                            break;
                        case tagShowImageUrl:
                            showValue = (useShow && show.ImageManager != null) ? show.ImageManager.Thumbnail_Small : (useShowEvent) ? showEvent.ImageUrl : null;
                            if (showValue != null && showValue.Length > 0)
                                showValue = showValue.Insert(0, string.Format("http://{0}{1}", _Config._DomainName, (showValue.StartsWith("/")) ? string.Empty : "/"));
                            break;
                        case tagEventUrl:
                            showValue = string.Format("http://{0}/{1}", _Config._DomainName, showUrl);
                            break;
                        //case tagShowId:
                        //    showValue = (useShow) ? show.Id.ToString() : 
                        //        (useShowEvent && showEvent.ParentId > 0 && showEvent.ParentType == ShowEvent.ParentTypes.Show) ? showEvent.TParentId.ToString() : null;
                        //    break;
                        case tagShowTitle:
                            showValue = (useShow) ? (show.ShowTitle != null && show.ShowTitle.Trim().Length > 0) ? show.ShowTitle.Trim() : null : null;
                            break;
                        case tagStatus:
                            showValue = (useShow) ? show.ShowAlert : (useShowEvent) ? showEvent.Status : simple.STATUS;
                            break;
                        case tagTimes:
                            showValue = (useShow) ? show.Display.Times_NoMarkup_ShowTimeOnly : (useShowEvent) ? showEvent.Times : null;
                            break;
                        case tagPricing:
                            showValue = (useShow) ? show.Display.Mailer_Pricing_NoMarkup : (useShowEvent) ? showEvent.Pricing : null;
                            break;
                        case tagVenue:
                            showValue = (useShow) ? show.Display.Venue_NoMarkup_NoLinks_NoAddress_NoLeadIn : (useShowEvent) ? showEvent.Venue : simple.VENUE;
                            break;
                    }
                }
            }

            return showValue;
        }
    }
}