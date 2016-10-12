using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using System.Web.UI;

namespace Wcss
{
    public partial class Show
    {
        #region wc_

        public string wc_DisplayVenueMini
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                if (_Config._Display_Venue)
                    sb.AppendFormat("<span class='venuemini'>{0}</span>", (this.VenueRecord.Name));

                return sb.ToString();
            }
        }

        public string wc_DisplayShowTimes(bool getAges, bool includePricing)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<div class='showtimes'>");

            ShowDateCollection coll = new ShowDateCollection();
            coll.AddRange(this.ShowDateRecords().GetList().FindAll(delegate(ShowDate match) { return (match.IsActive == true); }));
            if (coll.Count > 1)
                coll.Sort("DtDateOfShow", true);

            foreach (ShowDate s in coll)
            {
                sb.AppendFormat("<span class='times'>");

                //dont bother with date if there is only one - changed on 090625 - always show dates
                sb.AppendFormat("{0}{1}{2} Doors: {3} Show: {4}", 
                    s.DateOfShow.ToString("ddd MM/dd"),
                    (s.IsSoldOut) ? string.Format(" (Sold Out!)") : string.Empty,//1
                    (getAges) ? string.Format(" <span class='ages'>{0}</span>", (s.AgesString)) : string.Empty, //2
                    s.DateOfShow.ToString("hh:mmtt"),//3
                    (s.ShowTime != null && s.ShowTime.Trim().Length > 0) ? (s.ShowTime) : "tba");//4

                sb.AppendFormat("</span>");

                string pricing = s.PricingText;
                if (includePricing && pricing != null && pricing.Trim().Length > 0)
                    sb.AppendFormat("<span class=\"dateprice\">{0}</span>", (pricing));
            }

            //Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
            sb.AppendFormat("</div>");
            return sb.ToString();
        }

        private string _openerDisplay = null;
        public string wc_DisplayOpeners
        {
            get
            {
                if (_openerDisplay == null)
                {
                    System.Text.StringBuilder listing = new System.Text.StringBuilder();
                    List<Triplet> ActList = new List<Triplet>();

                    List<JShowAct> AllActs = new List<JShowAct>();

                    ShowDateCollection dates = new ShowDateCollection();
                    dates.CopyFrom(this.ShowDateRecords());
                    if (dates.Count > 1)
                        dates.Sort("DtDateOfShow", true);


                    string DATELIST = string.Empty;

                    //sort thru the dates
                    for (int i = 0; i < dates.Count; i++)
                    {
                        //record currentValue
                        string currentDate = string.Format("{0}~", dates[i].DateOfShow.ToString("MM/dd"));
                        DATELIST += currentDate;

                        JShowActCollection acts = new JShowActCollection();
                        acts.AddRange(dates[i].JShowActRecords().GetList().FindAll(
                            delegate(JShowAct match) { return (!match.TopBilling_Effective); }));
                        if (acts.Count > 1)
                            acts.Sort("IDisplayOrder", true);//list in order of importance

                        for (int j = 0; j < acts.Count; j++)
                        {
                            Triplet existing = (Triplet)ActList.Find(
                                delegate(Triplet match) { return ((int)match.First == (int)acts[j].TActId); });

                            if (existing != null)
                                existing.Third += currentDate;//update the date list value
                            else
                            {
                                Triplet t = new Triplet(acts[j].TActId, acts[j].Id, currentDate);//i+j is not consecutive but it does go in order without using another var
                                ActList.Add(t);
                                AllActs.Add(acts[j]);//match to first occurrence
                            }
                        }
                    }

                    //we now have a datelist to compare each act with - we also have the AllActs collection to match data with
                    //if the dates do not match, then we will display them with the act
                    if (ActList.Count > 0)//dont add anything if we dont need to
                    {
                        listing.AppendFormat("{0}{0}<span class='openerlist'>with ", Utils.Constants.NewLine);//leave space);

                        for (int i = 0; i < ActList.Count; i++)
                        {
                            Triplet savedInfo = (Triplet)ActList[i];

                            List<JShowAct> joinActColl = AllActs.FindAll(
                                delegate(JShowAct match) { return (match.Id == int.Parse(ActList[i].Second.ToString())); });

                            if (joinActColl.Count > 0)
                            {
                                JShowAct joinAct = joinActColl[0];
                                Act _act = joinAct.ActRecord;

                                listing.Append("<span class='opener'>");

                                //start link here if exists
                                bool hasWebsite = (_act.Website != null && _act.Website.Trim().Length > 0);

                                if (hasWebsite)//allow for invalid links - how esle to find them? possibly log
                                    listing.AppendFormat("<a href='{0}' class='openerwebsite'>",
                                        Utils.ParseHelper.FormatUrlFromString(_act.Website.Trim()));

                                if (joinAct.PreText != null && joinAct.PreText.Trim().Length > 0)
                                    listing.AppendFormat("<span class='openerpretext'>{0}</span>", (joinAct.PreText).Trim());

                                //always do act text
                                listing.AppendFormat("<span class='openeract'>{0}{1}</span>", _act.Name_Displayable, 
                                    (joinAct.ActText != null && joinAct.ActText.Trim().Length > 0) ? (joinAct.ActText).Trim() : string.Empty);

                                if (joinAct.Featuring != null && joinAct.Featuring.Trim().Length > 0)
                                    listing.AppendFormat("<span class='openerfeaturing'>{0}</span>", (joinAct.Featuring).Trim());

                                if (joinAct.PostText != null && joinAct.PostText.Trim().Length > 0)
                                    listing.AppendFormat("<span class='openerposttext'>{0}</span>", (joinAct.PostText).Trim());

                                //only show mismatches
                                string savedDateList = savedInfo.Third.ToString();
                                if (savedDateList != DATELIST)
                                    listing.AppendFormat("<span class='openerdatelist'>({0}) </span>", //leave space
                                        Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(savedDateList));

                                //end link here if exists
                                if (hasWebsite)//allow for invalid links - how esle to find them? possibly log
                                    listing.Append("</a>");

                                //end individual opener
                                listing.Append("~</span>");
                            }
                        }

                        Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(listing);

                        listing.Append("</span>");
                    }

                    _openerDisplay = Utils.ParseHelper.ParseCommasAndAmpersands(listing.ToString());
                }

                return _openerDisplay;
            }
        }

        #endregion

        #region JustifiedVenue

        /// <summary>
        /// Displays the Venue with all of its components wrapped within span tags. Specify false when desiring a non-tabled layout.
        /// The default is true (tabled layout)
        /// </summary>
        /// <returns></returns>
        public string DisplayVenue_Justified
        {
            get
            {
                return this.DisplayVenue_JustifiedAndWrapped(true);
            }
        }
        private string _venue_Table_Links = null;
        public string Venue_Table_Links
        {
            get
            {
                if (_venue_Table_Links == null)
                    _venue_Table_Links = this.DisplayVenue_JustifiedAndWrapped(false);

                return _venue_Table_Links;
            }
        }
        public string DisplayVenue_JustifiedAndWrapped(bool useLinks)
        {
            return this.DisplayVenue_Wrapped(true, useLinks);
        }
        public string DisplayVenue_Wrapped(bool useTable, bool useLinks)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (_Config._Display_Venue && this.VenueRecord != null)
            {
                if (useTable)
                    sb.AppendFormat("<table border='0' cellspacing='0' cellpadding='0' class='venuetable'><tr><td style='width: 100%;'>");
                else
                    sb.Append("<span class=\"venuetable\">");

                if(useTable)
                    sb.AppendFormat("{0}", (useLinks && this.VenueRecord.WebsiteUrl != null && this.VenueRecord.WebsiteUrl.Trim().Length > 0) ?
                        string.Format("<a href='{0}' target='_blank' title='{1}'>", 
                        Utils.ParseHelper.FormatUrlFromString(this.VenueRecord.WebsiteUrl, true, false), 
                        (this.VenueRecord.listInfo)) : string.Empty);

                sb.AppendFormat("<span class='name'>{0}</span>", (this.VenueRecord.Name_Displayable));

                if (useTable)
                    sb.AppendFormat("{0}", (useLinks && this.VenueRecord.WebsiteUrl != null && this.VenueRecord.WebsiteUrl.Trim().Length > 0) ?
                       "</a>" : string.Empty);//close the link if necessary
                
                if (useTable)
                    sb.AppendFormat("</td>");

                //city and state
                string state = (this.VenueRecord.State == null) ? string.Empty : (this.VenueRecord.State.Trim());
                string city = (this.VenueRecord.City == null) ? string.Empty : (this.VenueRecord.City.Trim());
                string divider = (state.Length > 0 && city.Length > 0) ? ", " : string.Empty;

                if (state.Trim().Length > 0 || city.Trim().Length > 0)
                {
                    if (useTable)
                        sb.Append("<td align='right'>");

                    sb.AppendFormat("<span class='venuelocation'> {0}{1}{2}</span>", city, divider, state);

                    if (useTable)
                        sb.Append("</td>");
                }

                if (useTable)
                    sb.AppendFormat("</tr></table>");
                else
                    sb.Append("</span>");
            }

            return sb.ToString();
        }

        #endregion


        #region DisplayProperties

        private string _name_WithLocation = null;
        /// <summary>
        /// returns date - venue, location - acts
        /// </summary>
        [XmlAttribute("Name_WithLocation")]
        public string Name_WithLocation
        {
            get
            {
                if(_name_WithLocation == null)
                    _name_WithLocation = (string.Format("{0} - {1}, {2}", 
                        this.Name, this.VenueRecord.City ?? string.Empty, this.VenueRecord.State ?? string.Empty));

                return _name_WithLocation;
            }
        }

        private string _listHeadliners = null;
        [XmlAttribute("listHeadliners")]
        public string listHeadliners
        {
            get
            {
                if (_listHeadliners == null)
                {
                    //return this.Display.fmHeadliners(false, false, true);

                    StringBuilder sb = new StringBuilder();

                    //only search first show for headliners
                    if (this.ShowDateRecords().Count > 0)
                    {
                        JShowActCollection coll = new JShowActCollection();
                        coll.AddRange(this.ShowDateRecords()[0].JShowActRecords().GetList().FindAll(
                            delegate(JShowAct entity) { return (entity.TopBilling_Effective); }));
                        if (coll.Count > 1)
                            coll.Sort("IDisplayOrder", true);

                        foreach (JShowAct entity in coll)
                        {
                            sb.AppendFormat("{0}{1}{2}{3}{4}~",
                                (entity.PreText != null) ? string.Format("{0} ", (entity.PreText.Trim())) : string.Empty,
                                (entity.ActRecord.Name_Displayable),
                                (entity.ActText != null) ? string.Format(" {0}", (entity.ActText.Trim())) : string.Empty,
                                (entity.Featuring != null) ? string.Format(" {0}", (entity.Featuring.Trim())) : string.Empty,
                                (entity.PostText != null) ? string.Format(" {0}", (entity.PostText.Trim())) : string.Empty);
                        }

                        Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                    }

                    _listHeadliners = sb.ToString().Trim();
                }

                return _listHeadliners;
            }
        }

        private string _listOpeners = null;
        [XmlAttribute("listOpeners")]
        public string listOpeners
        {
            get
            {
                if (_listOpeners == null)
                {
                    string dateList = string.Empty;
                    List<Pair> entityList = new List<Pair>();

                    foreach (ShowDate sd in this.ShowDateRecords())
                    {
                        dateList += string.Format("{0}~", sd.DateOfShow.ToString("MM/dd"));

                        JShowActCollection coll = new JShowActCollection();
                        coll.AddRange(sd.JShowActRecords().GetList().FindAll(
                            delegate(JShowAct entity) { return (!entity.TopBilling_Effective); }));
                        if(coll.Count > 1)
                            coll.Sort("IDisplayOrder", true);

                        foreach (JShowAct entity in coll)
                        {
                            string formedDate = string.Format("{0}~", sd.DateOfShow.ToString("MM/dd"));
                            string formedName = string.Format("{0} {1} {2} {3} {4}",
                                (entity.PreText != null) ? (entity.PreText.Trim()) : string.Empty,
                                (entity.ActRecord.Name_Displayable),
                                (entity.ActText != null) ? (entity.ActText.Trim()) : string.Empty,
                                (entity.Featuring != null) ? (entity.Featuring.Trim()) : string.Empty,
                                (entity.PostText != null) ? (entity.PostText.Trim()) : string.Empty).Trim();

                            formedName = System.Text.RegularExpressions.Regex.Replace(formedName, @"\s+", " ");

                            //if exists then update
                            Pair exists = Utils.ParseHelper.ValueInPair(2, entityList, formedName);
                            if (exists != null)
                                exists.First += formedDate;
                            else
                                entityList.Add(new Pair(formedDate, formedName));

                        }
                    }

                    //now we have a list of acts to create output with
                    StringBuilder sb = new StringBuilder();

                    foreach (Pair p in entityList)
                    {
                        sb.AppendFormat("{0}{1}~", (p.First.ToString() != dateList) ?
                            Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(p.First.ToString()) : string.Empty,
                            p.Second.ToString());
                    }

                    if (sb.Length > 0)
                    {
                        Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                        sb.Insert(0, "with ");
                    }

                    _listOpeners = sb.ToString().Trim();
                }

                return _listOpeners;
            }
        }

        private string _cartPromoter = null;
        [XmlAttribute("cartPromoter")]
        public string cartPromoter
        {
            get
            {
                if (_cartPromoter == null)
                {
                    StringBuilder sb = new StringBuilder();

                    JShowPromoterCollection coll = new JShowPromoterCollection();
                    coll.AddRange(this.JShowPromoterRecords());
                    if (coll.Count > 1)
                        coll.Sort("IDisplayOrder", true);

                    if (coll.Count > 0)
                    {
                        foreach (JShowPromoter ent in coll)
                        {
                            if(ent.PreText != null)
                                sb.AppendFormat("<span class='pretext'>{0}</span>", HttpUtility.HtmlEncode(ent.PreText));

                            sb.AppendFormat("<span class='name'>{0}</span>", HttpUtility.HtmlEncode(ent.PromoterRecord.Name));

                            if(ent.PromoterText != null)
                                sb.AppendFormat("<span class='extra'>{0}</span>", HttpUtility.HtmlEncode(ent.PromoterText));

                            if(ent.PostText != null)
                                sb.AppendFormat("<span class='posttext'>{0}</span>", HttpUtility.HtmlEncode(ent.PostText));

                            sb.Append("~");
                        }

                        Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);

                        sb.AppendFormat("<span class='present'> Present{0}...</span>", (coll.Count == 1) ? "s" : string.Empty);
                    }

                    _cartPromoter = sb.ToString();
                }

                return _cartPromoter;
            }
        }
        private string _showDateList = null;
        [XmlAttribute("ShowDateList")]
        public string ShowDateList { get {
            if (_showDateList == null)
                _showDateList = (_Config._DisplayDatesAsRange) ? this.Display.Date_Markup_3Day_NoTime_Ranged : this.Display.Date_Markup_3Day_NoTime_ListAll;

            return _showDateList;
        } }
        private string _showDateListNoStatus = null;
        [XmlAttribute("ShowDateListNoStatus")]
        public string ShowDateListNoStatus { get {
            if (_showDateListNoStatus == null)
                _showDateListNoStatus = (_Config._DisplayDatesAsRange) ? this.Display.Date_Markup_3Day_NoTime_Ranged_NoStatus : this.Display.Date_Markup_3Day_NoTime_ListAll_NoStatus;

            return _showDateListNoStatus;
        } }
        private string _showDateListNoStatusNoMarkup = null;
        [XmlAttribute("ShowDateListNoStatusNoMarkup")]
        public string ShowDateListNoStatusNoMarkup
        {
            get
            {
                if (_showDateListNoStatusNoMarkup == null)
                    _showDateListNoStatusNoMarkup = (_Config._DisplayDatesAsRange) ? 
                        this.Display.Date_NoMarkup_3Day_NoTime_Ranged_NoStatus : 
                        this.Display.Date_NoMarkup_3Day_NoTime_ListAll_NoStatus;

                return _showDateListNoStatusNoMarkup;
            }
        }
           
        /// <summary>
        /// used for when you can only use 25 chars
        /// </summary>
        [XmlAttribute("ShowNamePartCondense")]
        public string ShowNamePartCondense
        {
            get
            {
                if (this.GetShowVenueMainActPart.Length < 22)
                    return this.GetShowVenueMainActPart;
                else
                    return this.GetShowVenueMainActPart.PadRight(25, '.');
            }
        }
        private string _menuDate = null;
        [XmlAttribute("menuDate")]
        public string menuDate
        {
            get
            {
                if (_menuDate == null)
                {
                    StringBuilder sb = new StringBuilder();

                    ShowDateCollection coll = new ShowDateCollection();
                    coll.AddRange(this.ShowDateRecords());
                    if (coll.Count > 1)
                        coll.Sort("DtDateOfShow", true);

                    foreach (ShowDate s in coll)
                        sb.AppendFormat("{0}~", s.DateOfShow.ToString("MM/dd"));

                    Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);

                    _menuDate = sb.ToString();
                }

                return _menuDate;
            }
        }
        private string _menuHeadline = null;
        [XmlAttribute("menuHeadline")]
        public string menuHeadline
        {
            get
            {
                if (_menuHeadline == null)
                {
                    StringBuilder sb = new StringBuilder();

                    ShowDateCollection coll = new ShowDateCollection();
                    coll.AddRange(this.ShowDateRecords());
                    if (coll.Count > 1)
                        coll.Sort("DtDateOfShow", true);

                    //add the headliners
                    foreach (Act a in this.HeadlinerCollection)
                        sb.AppendFormat("{0}~", (a.Name_Displayable));

                    Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);

                    _menuHeadline = sb.ToString();
                }

                return _menuHeadline;
            }
        }
        #endregion
        
    }
}

