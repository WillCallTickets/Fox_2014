using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using System.Web.UI;

namespace Wcss
{
    [Serializable]
    public partial class ShowDisplay
    {
        private Show _entity = null;
        private Show Entity { get { return _entity; } }

        //private string _showHeader = null;
        public string ShowHeader
        {
            get
            {
                if (Entity != null)
                {
                    string statii = string.Empty;

                    //only include status for forward shows
                    if (Entity.LastDate >= _Config.SHOWOFFSETDATE)
                    {
                        ShowDate defaultSD = Entity.FirstShowDate;

                        if ((!Entity.IsSoldOut) && defaultSD.ShowStatusRecord.Name == _Enums.ShowDateStatus.OnSale.ToString())
                        {
                            statii = defaultSD.Display.BuyLink;
                        }
                        else
                        {
                            //cancelled, moved, new date, notactive, onsale, pending, postponed, soldout
                            string status = (Entity.IsSoldOut) ? _Enums.ShowDateStatus.SoldOut.ToString() : defaultSD.StatusName;

                            if (status == _Enums.ShowDateStatus.SoldOut.ToString())
                                status = "Sold Out";

                            statii = string.Format("<div class=\"status-status\">{0}!</div>", status);
                        }
                    }

                    //event date and event status
                    return string.Format("<div itemprop=\"startDate\" content=\"{3}\" class=\"event-date\">{1}</div>{0}<div class=\"event-status\">{2}</div>", 
                        Environment.NewLine,
                        Entity.Display.ShowDateListing_Simple,
                        statii,
                        Entity.FirstShowDate.DateOfShow_ToSortBy.ToUniversalTime().ToString("yyyy-MM-ddTHHmmss")
                        );
                }

                return string.Empty;
            }
        }

        private string _showDateListing_Simple = null;
        /// <summary>
        /// A simple listing of dates - no statii
        /// </summary>
        public string ShowDateListing_Simple
        {
            get
            {
                if (_showDateListing_Simple == null)
                {
                    List<string> list = new List<string>();
                    List<ShowDate> dates = new List<ShowDate>();
                    dates.AddRange(ShowDateCollection);

                    foreach (ShowDate sd in dates)
                    {
                        //151220 changed from showing the year - if users can't figure this out - God help us
                        //string newDate = (sd.DateOfShow.Date.Year != DateTime.Now.Date.Year) ? sd.DateOfShow.ToString("ddd MMM dd yyyy") : sd.DateOfShow.ToString("ddd MMM dd");
                        string newDate = sd.DateOfShow.ToString("ddd MMM dd");
                        if(!list.Contains(newDate))
                            list.Add(newDate);

                        string listing = null;
                        foreach(string s in list)
                            listing += string.Format("<div class=\"date-listing\">{0}~</div>", s);

                        _showDateListing_Simple = Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(listing);
                    }

                    if (dates.Count > 1)
                    {
                        _showDateListing_Simple = string.Format("<div class=\"mult-date date-count-{0}\">{1}</div>", 
                            dates.Count.ToString(),
                            _showDateListing_Simple);
                    }
                }

                return _showDateListing_Simple;
            }
        }
        
        /// <summary>
        /// quick summary based off of showname - no need to include date here - is implied by the calendar object and will
        /// be reiterated in the description
        /// </summary>
        public string DisplaySocial_Subject
        {
            get
            {
                return Utils.ParseHelper.DoubleSpaceToSingle(string.Format("{0} @ {1}",
                    _entity.GetShowMainActPart,
                    _entity.DisplayVenueName.Replace("-", " ")
                    ));
            }
        }



        /// <summary>
        /// Just a generic description - feel free to use whatever is necessary. This format is only one standard. 
        /// Nov 04, 2013 TYCHO with BEACON @ THE FOX THEATRE BOULDER, CO
        /// update April 2015 - only show venue if not fox
        /// documentation has previously stated that this should only be 70 chars - but let social media parse
        /// REMOVE colons!!!
        /// </summary>
        public string DisplaySocial_Description 
        {
            get
            {
                return string.Format("{0} {1} @ {2}",
                    DisplaySocial_Date, DisplaySocial_Acts, DisplaySocial_Venue);
            }
        }

        /// <summary>
        /// update April 2015 - only show venue if not fox
        /// documentation has previously stated that this should only be 70 chars - but let social media parse
        /// REMOVE colons!!!
        /// </summary>
        public string DisplaySocial_Description_ForSocialPosts
        {
            get
            {
                string venue = (string.Compare(_entity.VenueRecord.Name, _Config._Default_VenueName, true) != 0) ?
                    string.Format(" @ {0}", DisplaySocial_Venue) : string.Empty;

                ShowDate sd = _entity.FirstShowDate;

                string heads = (sd.MenuBilling != null && sd.MenuBilling.Trim().Length > 0) ?
                    sd.MenuBilling.Trim() : _entity.Display.Headliners_NoMarkup_Verbose_NoLinks;
                string opens = _entity.Display.Openers_NoMarkup_Verbose_NoLinks;
                
                string acts = Utils.ParseHelper.DoubleSpaceToSingle(RemoveSpecialGuest(string.Format("{0} {1}",
                    (heads.Trim().Length > 0) ? string.Format("{0}", heads) : string.Empty,
                    (opens.Trim().Length > 0) ? opens : string.Empty))).Trim();
                
                return string.Format("{0} {1}{2}",
                    DisplaySocial_Date, acts, venue);//.Replace(":", string.Empty);
            }
        }

        public string Display_Time_Ages_Tickets
        {
            get
            {
                /* FROM Show View control
                 *  litInfo.Text = string.Format("<div class=\"event-times\"><span>Doors: {0}</span><span>Show: {1}</span><span>{2}</span></div>{3}",
                    defaultSD.DateOfShow.ToString("hh:mmtt"),
                    (defaultSD.ShowTime != null && defaultSD.ShowTime.Trim().Length > 0) ? (defaultSD.ShowTime) : "tba",
                    defaultSD.AgesString,
                    Environment.NewLine);

                if (defaultSD.PricingText != null && defaultSD.PricingText.Trim().Length > 0)
                    litInfo.Text += string.Format("<div itemscope itemtype=\"http://schema.org/Offer\" class=\"event-pricing\"><span itemprop=\"name\">{0}</span></div>{1}",
                        defaultSD.PricingText.Trim(), 
                        Environment.NewLine);
                 */

                ShowDate sd = _entity.FirstShowDate;

                string ticketInfo = (sd != null && sd.PricingText != null) ? 
                    string.Format(" | {0}", sd.PricingText.Trim()) : string.Empty;

                //door time - show time - ages - tickets
                return string.Format("Doors {0} | Show {1} | {2}{3}",
                    sd.DateOfShow.ToString("hh:mmtt"),
                    (sd.ShowTime != null && sd.ShowTime.Trim().Length > 0) ? (sd.ShowTime) : "tba",
                    sd.AgesString,
                    (sd != null && sd.PricingText != null) ? string.Format(" | {0}", sd.PricingText.Trim()) : string.Empty
                    );//.Replace(":", string.Empty);
            }
        }

        public string DisplaySocial_Date
        {
            get
            {
                return _entity.FirstShowDate.DateOfShow_ToSortBy.ToString("MMM dd, yyyy").ToUpper();
            }
        }
        
        public string DisplaySocial_Venue
        {
            get
            {                
                return Utils.ParseHelper.DoubleSpaceToSingle(
                    _entity.DisplayVenueName.Replace("-", " ")).Trim().ToUpper();
            }
        }

        public string DisplaySocial_DateAndVenue
        {
            get
            {
                return string.Format("{0} @ {1}",
                    DisplaySocial_Date, DisplaySocial_Venue);
            }
        }

        public string DisplaySocial_Acts//update - let client decide - bool includeNewLineAfterHeadliner) - client can replace newline
        {
            get
            {
                string heads = _entity.Display.Headliners_NoMarkup_Verbose_NoLinks;
                string opens = _entity.Display.Openers_NoMarkup_Verbose_NoLinks;

                return Utils.ParseHelper.DoubleSpaceToSingle(RemoveSpecialGuest(string.Format("{0}{1}{2}",
                    (heads.Trim().Length > 0) ? string.Format("{0}", heads) : string.Empty,
                    (heads.Trim().Length > 0 && opens.Trim().Length > 0) ? Environment.NewLine : " ",
                    (opens.Trim().Length > 0) ? opens : string.Empty))).Trim();
            }
        }
        
        public static string RemoveSpecialGuest(string line)
        {
            //if we are in the upcoming section - remove the special guest
            if (line.EndsWith("with special guest", StringComparison.OrdinalIgnoreCase))
                line = line.Remove(line.IndexOf("with special guest", StringComparison.OrdinalIgnoreCase));
            else if (line.EndsWith("with special guests", StringComparison.OrdinalIgnoreCase))
                line = line.Remove(line.IndexOf("with special guests", StringComparison.OrdinalIgnoreCase));
            else if (line.EndsWith("& special guest", StringComparison.OrdinalIgnoreCase))
                line = line.Remove(line.IndexOf("& special guest", StringComparison.OrdinalIgnoreCase));
            else if (line.EndsWith("& special guests", StringComparison.OrdinalIgnoreCase))
                line = line.Remove(line.IndexOf("& special guests", StringComparison.OrdinalIgnoreCase));
            else if (line.EndsWith("&amp; special guest", StringComparison.OrdinalIgnoreCase))
                line = line.Remove(line.IndexOf("&amp; special guest", StringComparison.OrdinalIgnoreCase));
            else if (line.EndsWith("&amp; special guests", StringComparison.OrdinalIgnoreCase))
                line = line.Remove(line.IndexOf("&amp; special guests", StringComparison.OrdinalIgnoreCase));
            else if (line.EndsWith(", special guest", StringComparison.OrdinalIgnoreCase))
                line = line.Remove(line.IndexOf(", special guest", StringComparison.OrdinalIgnoreCase));
            else if (line.EndsWith(", special guests", StringComparison.OrdinalIgnoreCase))
                line = line.Remove(line.IndexOf(", special guests", StringComparison.OrdinalIgnoreCase));

            return line;
        }

        private ShowDateCollection _showDateCollection = null;
        public ShowDateCollection ShowDateCollection
        {
            get
            {
                if (_showDateCollection == null || _showDateCollection.Count == 0)
                {
                    _showDateCollection = new ShowDateCollection();
                    _showDateCollection.AddRange(Entity.ShowDateRecords().GetList()
                        .FindAll(delegate(ShowDate match) { return (match.IsActive && match.DateOfShow > _Config.SHOWOFFSETDATE); }));

                    //allow for past show dates in the show
                    if (_showDateCollection.Count == 0)
                        _showDateCollection.AddRange(Entity.ShowDateRecords().GetList()
                        .FindAll(delegate(ShowDate match) { return (match.IsActive); }));

                    if (_showDateCollection.Count > 1)
                        _showDateCollection.Sort("DtDateOfShow", true);
                }

                return _showDateCollection;
            }
        }
        public ShowDisplay(Show entity)
        {
            _entity = entity;
        }

        #region Acts
        private string fmOpeners(bool includeMarkup, bool includeLinks, bool verbose)
        {
            return fmActs(false, includeMarkup, includeLinks, verbose);
        }
        private string fmHeadliners(bool includeMarkup, bool includeLinks, bool verbose)
        {
            return fmActs(true, includeMarkup, includeLinks, verbose);
        }
        private string fmActs(bool headliners, bool includeMarkup, bool includeLinks, bool verbose)
        {
            StringBuilder fm = new StringBuilder();
            ShowDateCollection sdc = new ShowDateCollection();
            sdc.AddRange(ShowDateCollection.GetList());

            if (sdc.Count > 0)
            {
                List<string> Dates = new List<string>();

                List<Pair> vals = new List<Pair>();

                foreach (ShowDate sd in sdc)
                {
                    string currentDate = sd.DateOfShow.ToString("MM/dd~");
                    Dates.Add(currentDate);
                    string currentAct = (headliners) ? sd.Display.fmHeadliners(includeMarkup, includeLinks, verbose) :
                        sd.Display.fmOpeners(includeMarkup, includeLinks, verbose);

                    if (currentAct.Trim().Length > 0)
                    {
                        //just look for matching text
                        List<Pair> found = vals.FindAll(delegate(Pair match) { return (match.Second.ToString() == currentAct); });

                        //if we have a match on text - add date to date part
                        if (found.Count > 0)
                            found[0].First = string.Format("{0}{1}", found[0].First.ToString(), currentDate);
                        else
                            vals.Add(new Pair(currentDate, currentAct));
                    }
                }

                if (vals.Count > 0)
                {
                    if (includeMarkup)
                        fm.AppendFormat("<div class=\"actsection {0}\">", (headliners) ? "head" : "open");

                    string allDates = string.Join("", Dates.ToArray());

                    bool withHasBeenIncluded = false;
                    bool isAuto = this.Entity.ShowDateRecords()[0].IsAutoBilling;

                    for (int i = 0; i < vals.Count; i++)
                    {
                        //if we have auto billing, 
                        //and if the pair in question is an opener
                        if (isAuto && (!withHasBeenIncluded) && (!headliners))
                        {
                            if (includeMarkup)
                                fm.Append("<span class=\"with\">");

                            fm.Append(" with ");

                            if (includeMarkup)
                                fm.Append("</span>");

                            withHasBeenIncluded = true;
                        }

                        //if you want the name before date - use this
                        fm.Append(vals[i].Second.ToString());

                        //if the dates do not match all of the dates for the show - then display those dates
                        if (vals[i].First.ToString() != allDates)
                            fm.AppendFormat(" ({0})<span class=\"dateboundary\">&nbsp;</span> ", Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(vals[i].First.ToString()));

                        fm.Append("~");
                    }

                    if (isAuto)
                        Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(fm);
                    else
                        fm.Replace("~", " ");

                    if (includeMarkup)
                        fm.Append("</div>");
                }
            }

            return System.Text.RegularExpressions.Regex.Replace(fm.ToString(),@"\s+", " ");
        }
        #endregion

        #region Times

        private string _times_NoMarkup_ShowTimeOnly = null;
        public string Times_NoMarkup_ShowTimeOnly
        {
            get
            {
                if (_times_NoMarkup_ShowTimeOnly == null)
                    _times_NoMarkup_ShowTimeOnly = this.fmTimes(false, false, false, false, false, true).TrimStart('0');

                return _times_NoMarkup_ShowTimeOnly;
            }
        }

        private string _times_Markup_Ages_Pricing_DateNotes_DateStatus = null;
        /// <summary>
        /// fmTimes(true, true, true, true, true);
        /// </summary>
        public string Times_Markup_Ages_Pricing_DateNotes_DateStatus
        {
            get
            {
                if (_times_Markup_Ages_Pricing_DateNotes_DateStatus == null)
                    _times_Markup_Ages_Pricing_DateNotes_DateStatus = this.fmTimes(true, true, true, true, true);

                return _times_Markup_Ages_Pricing_DateNotes_DateStatus;
            }
        }

        private string _pricingOnly = null;
        public string Pricing_Only
        {
            get
            {
                if (_pricingOnly == null)
                    _pricingOnly = this.fmTimes(false, false, true, false, false);

                return _pricingOnly;
            }
        }

        private string fmTimes(bool includeMarkup, bool includeAges, bool includePricing, bool includeDateNotes, bool includeDateStatus)
        {
            return fmTimes(includeMarkup, includeAges, includePricing, includeDateNotes, includeDateStatus, false);
        }
        private string fmTimes(bool includeMarkup, bool includeAges, bool includePricing, bool includeDateNotes, bool includeDateStatus, bool showTimesOnly)
        {
            StringBuilder fm = new StringBuilder();
            StringBuilder tix = new StringBuilder();

            if (ShowDateCollection.Count > 0)
            {
                List<Pair> vals = new List<Pair>();

                if (showTimesOnly)
                {
                    foreach (ShowDate s in ShowDateCollection)
                    {
                        string showTime = (s.ShowTime != null && s.ShowTime.Trim().Length > 0) ? s.ShowTime.Trim() : "tba";

                        //first is date - then ages
                        Pair existing = vals.Find(delegate(Pair match) { return (match.Second.ToString() == showTime); } );
                        //if it exists - then add the date to the first part of the pair
                        if (existing != null)
                            existing.First = string.Format("{0}~{1}", existing.First, s.DateOfShow.ToString("MM/dd hh:mmtt"));
                        //else - add a new pair
                        else
                            vals.Add(new Pair(s.DateOfShow.ToString("MM/dd hh:mmtt"), showTime));
                    }

                    //now interpret the list of pairs
                    if(vals.Count == 1)
                    {
                        fm.AppendFormat("{0}{1}{2}", (includeMarkup) ? "<span class=\"times\">" : string.Empty, 
                            (vals[0].Second.ToString()), (includeMarkup) ? "</span>" : string.Empty);
                    }
                    else
                    {
                        foreach(Pair p in vals)
                        {
                            if (includeMarkup)
                                fm.Append("<span class=\"times\">");

                            //get the dates out of the First
                            fm.Append(string.Format("{0} {1} ", Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(p.First.ToString()), 
                                (p.Second.ToString())));

                            if (includeMarkup)
                                fm.Append("</span>");
                        }
                    }

                    return fm.ToString();
                }

                foreach (ShowDate s in ShowDateCollection)
                {
                    if (includeMarkup)
                        fm.Append("<div class=\"showinfo-showdate\">");
                    
                    //dont bother with date if there is only one
                    fm.AppendFormat("{0}{1}Doors: {2} Show: {3}{4}",//not how order has changed!!!081110
                        (includeMarkup) ? "<div class=\"times\">" : string.Empty,
                        (ShowDateCollection.Count > 1) ? s.DateOfShow.ToString("ddd MM/dd ") : string.Empty,
                        s.DateOfShow.ToString("hh:mmtt"),
                        (s.ShowTime != null && s.ShowTime.Trim().Length > 0) ? (s.ShowTime) : "tba",
                        (includeMarkup) ? "</div>" : string.Empty);

                    string pricing = s.PricingText;
                    if (includePricing)
                    {
                        if (pricing != null && pricing.Trim().Length > 0)
                        {
                            if (includeMarkup)
                                fm.AppendFormat("<div class=\"showdate-pricing\">");
                            fm.Append((pricing));
                            if (includeMarkup)
                                fm.AppendFormat("</div>");
                        }
                    }

                    if (includeAges)
                        fm.AppendFormat("{0}{1}{2}", (includeMarkup) ? "<div class=\"show-ages\">" : string.Empty,
                            (s.AgesString), (includeMarkup) ? "</div>" : string.Empty);

                    string buy = s.Display.BuyLink ?? string.Empty;
                    string soc = string.Empty;// BuildSocialContent(); //CreateSharrreModule();  //BuildSocialContent();


                    fm.AppendLine();
                    fm.AppendLine("<div class=\"show-ticketline\">");

                    //establish first line            
                    if (buy.Length > 0)//if just buy is > 0
                    {
                        fm.Append(buy);
                        fm.AppendLine();
                    }

                    fm.AppendLine(s.Display.TicketBoilerplate);//always include boilerplate
                    fm.AppendLine("</div>");//end show-ticketline

                    //fm.AppendLine("</div><div class=\"clearfix\"></div>");//end show-ticketline


                    //establish second line - always create something so that the rowspan value is taken into account
                    if (soc.Length > 0)
                    {
                        fm.AppendLine("<div class=\"show-social\">");
                        fm.AppendLine(soc);
                        fm.AppendLine("</div><div class=\"clearfix\"></div>");//??
                    }

                    fm.AppendLine();


                    #region Ages and DateStatus
                    
                    //only if show other than onsale
                    if (includeDateStatus && (s.IsSoldOut || (s.StatusName.Length > 0 && s.StatusName != _Enums.ShowDateStatus.OnSale.ToString())))
                    {
                        fm.AppendLine("<div class=\"show-statii\">");
                        //always include markup for status
                        if (includeMarkup)
                            fm.AppendFormat("<span class=\"label label-danger showdate-status\">");

                        string status = s.StatusName;
                        if (s.IsSoldOut || status == _Enums.ShowDateStatus.SoldOut.ToString())
                            status = "Sold Out";

                        fm.AppendFormat("{0}!", (status));

                        if (includeMarkup)
                            fm.AppendFormat("</span>");

                        fm.AppendLine("</div>");
                    }

                    #endregion
                    
                    if (includeMarkup)
                        fm.AppendFormat("</div>");//close showinfo-showdate
                }
            }

            return fm.ToString();
        }

       
        
        /// <summary>
        /// consider event date past sell date
        /// consider show onsale time
        /// consider sold out show                        
        /// </summary>
        

        private string _mailer_Ages_NoMarkup = null;
        public string Mailer_Ages_NoMarkup
        {
            get
            {
                if (_mailer_Ages_NoMarkup == null)
                    _mailer_Ages_NoMarkup = this.mailerAges(false);

                return _mailer_Ages_NoMarkup;
            }
        }
        private string _mailer_Ages_Markup = null;
        public string Mailer_Ages_Markup
        {
            get
            {
                if (_mailer_Ages_Markup == null)
                    _mailer_Ages_Markup = this.mailerAges(true);

                return _mailer_Ages_Markup;
            }
        }
        private string mailerAges(bool includeMarkup)
        {
            StringBuilder fm = new StringBuilder();

            if (ShowDateCollection.Count > 0)
            {
                List<Pair> ages = new List<Pair>();

                foreach (ShowDate s in ShowDateCollection)
                {
                    //first is date - then ages
                    Pair existing = ages.Find(delegate(Pair match) { return (match.Second.ToString() == s.AgesString); } );
                    //if it exists - then add the date to the first part of the pair
                    if (existing != null)
                        existing.First = string.Format("{0}~{1}", existing.First, s.DateOfShow.ToString("MM/dd hh:mmtt"));
                    //else - add a new pair
                    else
                        ages.Add(new Pair(s.DateOfShow.ToString("MM/dd hh:mmtt"), s.AgesString));
                }

                //now interpret the list of pairs
                if(ages.Count == 1)
                {
                    fm.AppendFormat("{0}{1}{2}", (includeMarkup) ? "<div><span class=\"mlr_ages\">" : string.Empty, 
                        (ages[0].Second.ToString()), (includeMarkup) ? "</span></div>" : string.Empty);
                }
                else
                {
                    foreach(Pair p in ages)
                    {
                        if (includeMarkup)
                            fm.Append("<div><span class=\"mlr_ages\">");

                        //get the dates out of the First
                        fm.Append(string.Format("{0} {1} ", Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(p.First.ToString()), 
                            (p.Second.ToString())));

                        if (includeMarkup)
                            fm.Append("</span></div>");
                    }
                }
            }

            return fm.ToString();
        }
        private string _mailer_Times_Markup = null;
        public string Mailer_Times_Markup
        {
            get
            {
                if (_mailer_Times_Markup == null)
                    _mailer_Times_Markup = this.mailerTimes(true);

                return _mailer_Times_Markup;
            }
        }
        private string _mailer_Times_NoMarkup = null;
        public string Mailer_Times_NoMarkup
        {
            get
            {
                if (_mailer_Times_NoMarkup == null)
                    _mailer_Times_NoMarkup = this.mailerTimes(false);

                return _mailer_Times_NoMarkup;
            }
        }
        private string mailerTimes(bool includeMarkup)
        {
            StringBuilder fm = new StringBuilder();

            if (ShowDateCollection.Count > 0)
            {
                List<Pair> pairs = new List<Pair>();

                foreach (ShowDate s in ShowDateCollection)
                {
                    //first is date - then ages
                    Pair existing = pairs.Find(delegate(Pair match) { return (match.Second.ToString() == string.Format("Doors: {0} / Show: {1}", s.DoorTime, s.ShowTime)); });
                    //if it exists - then add the date to the first part of the pair
                    if (existing != null)
                        existing.First = string.Format("{0}~{1}", existing.First, string.Format("Doors: {0} / Show: {1}", s.DoorTime, s.ShowTime));
                    //else - add a new pair
                    else
                        pairs.Add(new Pair(s.DateOfShow.ToString("MM/dd hh:mmtt"), string.Format("Doors: {0} / Show: {1}", s.DoorTime, s.ShowTime)));
                }

                //now interpret the list of pairs
                if (pairs.Count == 1)
                {
                    fm.AppendFormat("{0}{1}{2}", (includeMarkup) ? "<div class=\"mlr_times\">" : string.Empty, 
                        (pairs[0].Second.ToString()), (includeMarkup) ? "</div>" : string.Empty);
                }
                else
                {
                    foreach (Pair p in pairs)
                    {
                        if (includeMarkup)
                            fm.Append("<div class=\"mlr_times\">");

                        //get the dates out of the First
                        fm.Append(string.Format("{0} {1} ", Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(p.First.ToString()), 
                            (p.Second.ToString())));

                        if (includeMarkup)
                            fm.Append("</div>");
                    }
                }
            }

            return fm.ToString();
        }
        private string _mailer_Pricing_Markup = null;
        public string Mailer_Pricing_Markup
        {
            get
            {
                if (_mailer_Pricing_Markup == null)
                    _mailer_Pricing_Markup = this.mailerPricing(true);
                
                return _mailer_Pricing_Markup;
            }
        }
        private string _mailer_Pricing_NoMarkup = null;
        public string Mailer_Pricing_NoMarkup
        {
            get
            {
                if (_mailer_Pricing_NoMarkup == null)
                    _mailer_Pricing_NoMarkup = this.mailerPricing(false);

                return _mailer_Pricing_NoMarkup;
            }
        }
        private string mailerPricing(bool includeMarkup)
        {
            StringBuilder fm = new StringBuilder();

            if (ShowDateCollection.Count > 0)
            {
                List<Pair> pairs = new List<Pair>();

                foreach (ShowDate s in ShowDateCollection)
                {
                    string priceText = s.PricingText ?? string.Empty;

                    //first is date - then ages
                    Pair existing = pairs.Find(delegate(Pair match) { return (match.Second.ToString() == priceText); });
                    //if it exists - then add the date to the first part of the pair
                    if (existing != null)
                        existing.First = string.Format("{0}~{1}", existing.First, priceText);
                    //else - add a new pair
                    else
                        pairs.Add(new Pair(s.DateOfShow.ToString("MM/dd hh:mmtt"), priceText));
                }

                //now interpret the list of pairs
                if (pairs.Count == 1 && pairs[0].Second.ToString().Trim().Length > 0)
                {
                    fm.AppendFormat("{0}{1}{2}", (includeMarkup) ? "<div class=\"mlr_pricing\">" : string.Empty, (pairs[0].Second.ToString()), (includeMarkup) ? "</div>" : string.Empty);
                }
                else
                {
                    foreach (Pair p in pairs)
                    {
                        if (pairs[0].Second.ToString().Trim().Length > 0)
                        {
                            if (includeMarkup)
                                fm.Append("<div class=\"mlr_pricing\">");

                            //get the dates out of the First
                            fm.Append(string.Format("{0} {1} ", Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(p.First.ToString()), (p.Second.ToString())));

                            if (includeMarkup)
                                fm.Append("</div>");
                        }
                    }
                }
            }

            return fm.ToString();
        }
        #endregion

        #region Venue

        private string _venue_NoMarkup_NoLinks_NoAddress_NoLeadIn = null;
        public string Venue_NoMarkup_NoLinks_NoAddress_NoLeadIn
        {
            get
            {
                if (_venue_NoMarkup_NoLinks_NoAddress_NoLeadIn == null)
                {
                    _venue_NoMarkup_NoLinks_NoAddress_NoLeadIn = this.fmVenue(false, false, false, null);
                }

                return _venue_NoMarkup_NoLinks_NoAddress_NoLeadIn;
            }
        }

        private string _venue_Markup_NoLinks_NoAddress_LeadIn = null;
        /// <summary>
        /// Displays the date formatted for display next event control
        /// </summary>
        public string Venue_Markup_NoLinks_NoAddress_LeadIn
        {
            get
            {
                if (_venue_Markup_NoLinks_NoAddress_LeadIn == null)
                {
                    _venue_Markup_NoLinks_NoAddress_LeadIn = this.fmVenue(true, false, false, "At ");
                }

                return _venue_Markup_NoLinks_NoAddress_LeadIn;
            }
        }
        private string _venueForeign_Markup_Links_Address_LeadIn = null;
        /// <summary>
        /// Displays the date formatted for display next event control
        /// _show.Display.fmVenue(true, true, true, "This Show At");
        /// </summary>
        public string VenueForeign_Markup_Links_Address_LeadIn
        {
            get
            {
                if (_venueForeign_Markup_Links_Address_LeadIn == null)
                {
                    _venueForeign_Markup_Links_Address_LeadIn = this.fmVenue(true, true, true, "@");
                }

                return _venueForeign_Markup_Links_Address_LeadIn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leadIn">a string to lead in with -- The fabulous "Fox Theatre"</param>
        /// <returns></returns>
        private string fmVenue(bool includeMarkup, bool includeLinks, bool includeAddress, string leadIn)
        {
            StringBuilder fm = new StringBuilder();

            //This show at VENUE on ADDRESS
            if (includeMarkup)
                fm.Append("<div class=\"venuetable\">");

            if (leadIn != null && leadIn.Trim().Length > 0)
                fm.AppendFormat("{0} ", (leadIn).Trim());

            if (includeMarkup)
                fm.Append("<span class=\"venue-name\">");

            fm.Append((Entity.VenueRecord.Name_Displayable));

            if (includeMarkup)
                fm.Append("</span>");

            string address = Entity.VenueRecord.ShortAddress;
            if (includeAddress && address != null && address.Trim().Length > 0)
            {
                if (includeMarkup)
                    fm.Append("<span class=\"venue-address\">");

                fm.Append((address));

                if (includeMarkup)
                    fm.Append("</span>");
            }

            if (includeMarkup)
                fm.Append("</div>");

            return fm.ToString();
        }

        #endregion

        #region Ages

        private string fmAges(bool includeMarkup)
        {
            StringBuilder fm = new StringBuilder();
            List<string> Dates = new List<string>();

            List<Pair> vals = new List<Pair>();

            foreach (ShowDate sd in ShowDateCollection)
            {
                string currentDate = sd.DateOfShow.ToString("MM/dd~");
                Dates.Add(currentDate);
                string current = sd.Display.fmAges(includeMarkup);

                //just look for matching text
                List<Pair> found = vals.FindAll(delegate(Pair match) { return (match.Second.ToString() == current); });

                //if we have a match on text - add date to date part
                if (found.Count > 0)
                    found[0].First = string.Format("{0}{1}", found[0].ToString(), currentDate);
                else
                    vals.Add(new Pair(currentDate, current));
            }

            if (includeMarkup)
                fm.AppendFormat("<span class=\"agesection\">{0}</span>");

            string allDates = string.Join("", Dates.ToArray());

            foreach (Pair p in vals)
            {
                //if the dates do not match all of the dates for the show - then display those dates
                if (p.First.ToString() != allDates)
                    fm.AppendFormat("({0}) ", Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(p.First.ToString()));

                fm.Append(p.Second.ToString());
            }

            if (includeMarkup)
                fm.Append("</span>");

            return fm.ToString();
        }

        #endregion

        #region Dates

        private string _date_Markup_3Day_NoTime_Ranged = null;
        /// <summary>
        /// fmDateFormatted(true, 3, false);
        /// </summary>
        public string Date_Markup_3Day_NoTime_Ranged
        {
            get
            {
                if (_date_Markup_3Day_NoTime_Ranged == null)
                    _date_Markup_3Day_NoTime_Ranged = this.fmDateFormatted(true, 3, false, true, true);

                return _date_Markup_3Day_NoTime_Ranged;
            }
        }
        private string _date_Markup_3Day_NoTime_ListAll = null;
        /// <summary>
        /// fmDateFormatted(true, 3, false);
        /// </summary>
        public string Date_Markup_3Day_NoTime_ListAll
        {
            get
            {
                if (_date_Markup_3Day_NoTime_ListAll == null)
                    _date_Markup_3Day_NoTime_ListAll = this.fmDateFormatted(true, 3, false, false, true);

                return _date_Markup_3Day_NoTime_ListAll;
            }
        }
        private string _date_Markup_3Day_NoTime_Ranged_NoStatus = null;
        /// <summary>
        /// fmDateFormatted(true, 3, false);
        /// </summary>
        public string Date_Markup_3Day_NoTime_Ranged_NoStatus
        {
            get
            {
                if (_date_Markup_3Day_NoTime_Ranged_NoStatus == null)
                    _date_Markup_3Day_NoTime_Ranged_NoStatus = this.fmDateFormatted(true, 3, false, true, false);

                return _date_Markup_3Day_NoTime_Ranged_NoStatus;
            }
        }
        private string _date_Markup_3Day_NoTime_ListAll_NoStatus = null;
        /// <summary>
        /// fmDateFormatted(true, 3, false);
        /// </summary>
        public string Date_Markup_3Day_NoTime_ListAll_NoStatus
        {
            get
            {
                if (_date_Markup_3Day_NoTime_ListAll_NoStatus == null)
                    _date_Markup_3Day_NoTime_ListAll_NoStatus = this.fmDateFormatted(true, 3, false, false, false);

                return _date_Markup_3Day_NoTime_ListAll_NoStatus;
            }
        }
        private string _date_NoMarkup_3Day_NoTime_ListAll_NoStatus = null;
        /// <summary>
        /// fmDateFormatted(true, 3, false);
        /// </summary>
        public string Date_NoMarkup_3Day_NoTime_ListAll_NoStatus
        {
            get
            {
                if (_date_NoMarkup_3Day_NoTime_ListAll_NoStatus == null)
                    _date_NoMarkup_3Day_NoTime_ListAll_NoStatus = this.fmDateFormatted(false, 3, false, false, false);

                return _date_NoMarkup_3Day_NoTime_ListAll_NoStatus;
            }
        }
        private string _date_NoMarkup_3Day_NoTime_Ranged = null;
        /// <summary>
        /// fmDateFormatted(true, 3, false);
        /// </summary>
        public string Date_NoMarkup_3Day_NoTime_Ranged
        {
            get
            {
                if (_date_NoMarkup_3Day_NoTime_Ranged == null)
                    _date_NoMarkup_3Day_NoTime_Ranged = this.fmDateFormatted(false, 3, false, true, true);

                return _date_NoMarkup_3Day_NoTime_Ranged;
            }
        }
        private string _date_NoMarkup_3Day_NoTime_Ranged_NoStatus = null;
        /// <summary>
        /// fmDateFormatted(true, 3, false);
        /// </summary>
        public string Date_NoMarkup_3Day_NoTime_Ranged_NoStatus
        {
            get
            {
                if (_date_NoMarkup_3Day_NoTime_Ranged_NoStatus == null)
                    _date_NoMarkup_3Day_NoTime_Ranged_NoStatus = this.fmDateFormatted(false, 3, false, true, false);

                return _date_NoMarkup_3Day_NoTime_Ranged_NoStatus;
            }
        }
        private string _date_NoMarkup_3Day_NoTime_ListAll = null;
        /// <summary>
        /// fmDateFormatted(true, 3, false);
        /// </summary>
        public string Date_NoMarkup_3Day_NoTime_ListAll
        {
            get
            {
                if (_date_NoMarkup_3Day_NoTime_ListAll == null)
                    _date_NoMarkup_3Day_NoTime_ListAll = this.fmDateFormatted(false, 3, false, false, true);

                return _date_NoMarkup_3Day_NoTime_ListAll;
            }
        }

        private string fmDateFormatted(bool displayDatesAsRange)
        {
            return fmDateFormatted(false, displayDatesAsRange);
        }
        private string fmDateFormatted(bool includeMarkup, bool displayDatesAsRange)
        {
            return fmDateFormatted(includeMarkup, 3, true, displayDatesAsRange, true);
        }
        private void CreateStandardDateDisplay(List<Pair> dates)
        {
            foreach (ShowDate sd in ShowDateCollection)
            {
                //only show statii that are not onsale
                string status = (sd.StatusName != _Enums.ShowDateStatus.OnSale.ToString()) ? sd.StatusName : string.Empty;
                if (status.ToLower() == "soldout")
                    status = "Sold Out";

                //display the year for past shows...
                Pair p = new Pair(status, (sd.DateOfShow > _Config.SHOWOFFSETDATE) ? sd.DateOfShow.ToString("ddd MM/dd") : sd.DateOfShow.ToString("ddd MM/dd/yyyy"));
                dates.Add(p);
            }
        }
        private string fmDateFormatted(bool includeMarkup, int dayLength, bool includeTime, bool displayDatesAsRange, bool includeStatus)
        {
            StringBuilder fm = new StringBuilder();

            if (ShowDateCollection.Count > 0)
            {
                List<Pair> dates = new List<Pair>();

                if ((ShowDateCollection[0].Showings_csv != null) || (displayDatesAsRange && ShowDateCollection.Count > 2))
                {
                    //if the dates are consecutive....make sure to check for mult shows in one day
                    List<DateTime> ds = new List<DateTime>();
                    foreach (ShowDate sd in ShowDateCollection)
                        ds.Add(sd.DateOfShow);

                    if (Utils.Validation.DatesSpanMoreThanTwoDays(ds))
                    {
                        string status = string.Empty;
                        //create a range listing
                        if(Entity.IsSoldOut)
                            status = "Sold Out";

                        string range = string.Format("{0} - {1}", ShowDateCollection[0].DateOfShow.ToString("ddd MM/dd"),
                            ShowDateCollection[ShowDateCollection.Count-1].DateOfShow.ToString("ddd MM/dd"));

                        Pair p = new Pair(status, range);
                        dates.Add(p);
                    }
                    else
                        CreateStandardDateDisplay(dates);
                }
                else
                {
                    CreateStandardDateDisplay(dates);
                }

                foreach (Pair p in dates)
                {
                    if (includeMarkup)
                        fm.AppendFormat("<span class=\"datesection\">");

                    fm.AppendFormat("{0}{1}{2}", (includeMarkup) ? "<span class=\"date\">" : string.Empty, p.Second.ToString(),
                        (includeMarkup) ? "</span>" : string.Empty);

                    if (includeStatus && p.First.ToString().Length > 0)
                        fm.AppendFormat(" {0}{1}!{2}", (includeMarkup) ? "<span class=\"datestatus\">" : string.Empty,(p.First.ToString()).Trim(),
                            (includeMarkup) ? "</span>" : string.Empty);

                    fm.Append("~");

                    if (includeMarkup)
                        fm.AppendFormat("</span>");
                }

                Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(fm);
            }

            return fm.ToString();
        }

        #endregion

        #region ListAll

        private string _allActs_Markup_Verbose_Links = null;
        /// <summary>
        /// fmAllActs(true, true, true);
        /// </summary>
        public string AllActs_Markup_Verbose_Links
        {
            get
            {
                if (_allActs_Markup_Verbose_Links == null)
                    _allActs_Markup_Verbose_Links = this.fmAllActs(true, true, true);

                return _allActs_Markup_Verbose_Links;
            }
        }
        private string _allActs_Markup_NoVerbose_NoLinks = null;
        /// <summary>
        /// fmAllActs(true, false, false);
        /// </summary>
        public string AllActs_Markup_NoVerbose_NoLinks
        {
            get
            {
                if (_allActs_Markup_NoVerbose_NoLinks == null)
                    _allActs_Markup_NoVerbose_NoLinks = this.fmAllActs(true, false, false);

                return _allActs_Markup_NoVerbose_NoLinks;
            }
        }
        private string _headliners_NoMarkup_Verbose_NoLinks = null;
        /// <summary>
        /// fmAllActs(true, false, false);
        /// </summary>
        public string Headliners_NoMarkup_Verbose_NoLinks
        {
            get
            {
                if (_headliners_NoMarkup_Verbose_NoLinks == null)
                    _headliners_NoMarkup_Verbose_NoLinks = this.fmHeadliners(false, false, true);

                return _headliners_NoMarkup_Verbose_NoLinks;
            }
        }
        private string _openers_NoMarkup_Verbose_NoLinks = null;
        /// <summary>
        /// fmAllActs(true, false, false);
        /// </summary>
        public string Openers_NoMarkup_Verbose_NoLinks
        {
            get
            {
                if (_openers_NoMarkup_Verbose_NoLinks == null)
                    _openers_NoMarkup_Verbose_NoLinks = this.fmOpeners(false, false, true);

                return _openers_NoMarkup_Verbose_NoLinks;
            }
        }
        
        private string fmAllActs(bool includeMarkup, bool verbose, bool includeLinks)
        {
            return fmAllActs(includeMarkup, verbose, includeLinks, verbose, includeLinks);
        }
        private string fmAllActs(bool includeMarkup, bool verboseHeads, bool includeLinksHeads, bool verboseOpens, bool includeLinksOpens)
        {
            string heads = this.fmHeadliners(includeMarkup, verboseHeads, includeLinksHeads);
            string opens = this.fmOpeners(includeMarkup, verboseOpens, includeLinksOpens);

            return string.Format("{0}{1} {2}{3}",
                (includeMarkup) ? "<div class=\"show-acts\">" : string.Empty,
                heads, opens,
                (includeMarkup) ? "</div>" : string.Empty);
        }
        #endregion

        #region Promoter

        private string _promoters_Markup_Links = null;
        /// <summary>
        /// this.fmPromoters(true, false);
        /// </summary>
        public string Promoters_Markup_Links
        {
            get
            {
                if (_promoters_Markup_Links == null)
                    _promoters_Markup_Links = this.fmPromoters(true, true);

                return _promoters_Markup_Links;
            }
        }
        private string _promoters_Markup_NoLinks = null;
        /// <summary>
        /// this.fmPromoters(true, false);
        /// </summary>
        public string Promoters_Markup_NoLinks
        {
            get
            {
                if (_promoters_Markup_NoLinks == null)
                    _promoters_Markup_NoLinks = this.fmPromoters(true, false);

                return _promoters_Markup_NoLinks;
            }
        }
        private string _promoters_NoMarkup_NoLinks = null;
        /// <summary>
        /// this.fmPromoters(true, false);
        /// </summary>
        public string Promoters_NoMarkup_NoLinks
        {
            get
            {
                if (_promoters_NoMarkup_NoLinks == null)
                    _promoters_NoMarkup_NoLinks = this.fmPromoters(false, false);

                return _promoters_NoMarkup_NoLinks;
            }
        }

        private string fmPromoters(bool includeMarkup, bool displayLinks)
        {
            StringBuilder fm = new StringBuilder();

            JShowPromoterCollection coll = new JShowPromoterCollection();
            coll.AddRange(Entity.JShowPromoterRecords());
            if (coll.Count > 1)
                coll.Sort("IDisplayOrder", true);

            if (coll.Count > 0)
            {
                foreach (JShowPromoter ent in coll)
                {
                    Promoter obj = ent.PromoterRecord;

                    if (displayLinks && obj.Website != null)
                        fm.AppendFormat("<a href=\"{0}\" target=\"_blank\">", obj.Website_Configured);

                    string pre = ent.PreText;
                    if (pre != null && pre.Trim().Length > 0)
                        fm.AppendFormat("{0}{1}{2} ", (includeMarkup) ? "<span class=\"pretext\">" : string.Empty, 
                            (pre), (includeMarkup) ? "</span>" : string.Empty);

                    fm.AppendFormat("{0}{1}{2}", (includeMarkup) ? "<span class=\"name\">" : string.Empty, 
                        (obj.Name), (includeMarkup) ? "</span>" : string.Empty);

                    string objText = ent.PromoterText;
                    if (objText != null && objText.Trim().Length > 0)
                        fm.AppendFormat(" {0}{1}{2}", (includeMarkup) ? "<span class=\"extra\">" : string.Empty, 
                            (objText), (includeMarkup) ? "</span>" : string.Empty);

                    string post = ent.PostText;
                    if (post != null && post.Trim().Length > 0)
                        fm.AppendFormat(" {0}{1}{2}", (includeMarkup) ? "<span class=\"posttext\">" : string.Empty, 
                            (post), (includeMarkup) ? "</span>" : string.Empty);

                    if (displayLinks && obj.Website != null)
                        fm.AppendFormat("</a>");

                    fm.Append("~");
                }

                Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(fm);

                fm.AppendFormat(" {0}Present{1}...{2}", (includeMarkup) ? "<span class=\"present\">" : string.Empty,
                    (coll.Count == 1) ? "s" : string.Empty, (includeMarkup) ? "</span>" : string.Empty);
            }

            return fm.ToString();
        }

        #endregion
    }
}

