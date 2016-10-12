using System;
using System.Web;
using System.Text;
using System.Xml.Serialization;
using System.Threading;

namespace Wcss
{
    public partial class ShowDate
    {
        /// ************************************************************************
        /// Configure show urls
        /// ************************************************************************

        /// <summary>
        /// Returns Date in the format yyyy-mm-dd-hhmmtt-eventname
        /// event name is stripped of funny chars, whitespace removed
        /// </summary>
        public string ConfiguredUrl_withDomain
        {
            get
            {
                return string.Format("http://{0}/{1}", _Config._DomainName, this.ConfiguredUrl);
            }
        }

        public static string Url_Dashed_ConvertFromSlashed(string url)
        {
            return url.Trim(new char[] { '/' }).Replace("/", "-").Trim();
        }

        // SEE TESTAPP for example

        static ReaderWriterLockSlim _slimLock_ConfiguredUrl = new ReaderWriterLockSlim();
        private string _configuredUrl = null;

        /// <summary>
        /// Returns Date in the format yyyy-mm-dd-hhmmtt-eventname
        /// event name is stripped of funny chars, whitespace removed
        /// </summary>
        public string ConfiguredUrl
        {
            get
            {
                try
                {
                    _slimLock_ConfiguredUrl.EnterReadLock();

                    if (_configuredUrl == null)
                    {
                        _slimLock_ConfiguredUrl.ExitReadLock();
                        _slimLock_ConfiguredUrl.EnterWriteLock();

                        string[] parts = this.ShowRecord.Name.Split('-');
                        if (parts.Length < 3)
                        {
                            _Error.LogToFile(string.Format("showid: {0} --- showdateid: {1} --- showname: {2}",
                               this.TShowId.ToString(), this.Id.ToString(), this.ShowRecord.Name
                               ),
                               string.Format("Fox2014Error_{0}", DateTime.Now.Date.ToString("MM_dd_yyyy")));

                            throw new ArgumentException(string.Format("Show name - not enough parts: {0}", this.ShowRecord.Name));
                        }

                        // not used
                        // string _date = parts[0];
                        // string _venue = parts[1];

                        // index and count
                        // start at index 2 - go until length - 2
                        int _startIdx = 2;
                        string _event = String.Join("-", parts, _startIdx, parts.Length - _startIdx);
                        string _parsedEvent = Utils.ParseHelper.FriendlyFormat(_event);

                        _configuredUrl = string.Format("{0}/{1}", this.DateOfShow.ToString("yyyy/MM/dd/hhmmtt"), _parsedEvent).TrimEnd('/');

                        _configuredUrl = ShowDate.Url_Dashed_ConvertFromSlashed(_configuredUrl);
                    }

                    return _configuredUrl;
                }
                catch (Exception e)
                {
                    _Error.LogException(e);
                }
                finally
                {
                    if (_slimLock_ConfiguredUrl.IsWriteLockHeld)
                        _slimLock_ConfiguredUrl.ExitWriteLock();

                    if (_slimLock_ConfiguredUrl.IsReadLockHeld)
                        _slimLock_ConfiguredUrl.ExitReadLock();
                }

                return string.Empty;
            }
        }

        /// ************************************************************************
        /// end of configure url
        /// ************************************************************************




        public string listOfActs(bool headliners, bool includeAllTexts)
        {
            sb.Length = 0;

            JShowActCollection coll = new JShowActCollection();
            coll.AddRange(this.JShowActRecords().GetList().FindAll(
                delegate(JShowAct entity) { return ((headliners) ? (entity.TopBilling_Effective) : (!entity.TopBilling_Effective)); }));
            if (coll.Count > 1)
                coll.Sort("IDisplayOrder", true);

            foreach (JShowAct js in coll)
            {
                sb.AppendFormat("{0} {1} {2} {3} {4}~",
                    (includeAllTexts && js.PreText != null) ? js.PreText : string.Empty,
                    js.ActRecord.Name_Displayable,
                    (includeAllTexts && js.ActText != null) ? js.ActText : string.Empty,
                    (includeAllTexts && js.Featuring != null) ? js.Featuring : string.Empty,
                    (includeAllTexts && js.PostText != null) ? js.PostText : string.Empty).ToString().Trim();
            }

            Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);

            return sb.ToString();
        }

        private string _showingsCsv = null;
        /// <summary>
        /// generally you will only go after this if it has been determined that this object is multiple. This does not worry about matching acts. The 
        /// fact that they are the same show is enough to qualify the relationship
        /// </summary>
        public string Showings_csv
        {
            get
            {
                if (_showingsCsv == null)
                {
                    if (this.ShowRecord.ShowDateRecords().Count > 0)
                    {
                        ShowDateCollection coll = new ShowDateCollection();
                        coll.AddRange(this.ShowRecord.ShowDateRecords().GetList().FindAll(delegate(ShowDate match)
                        {
                            return (match.IsActive && match.DateOfShow.AddHours(-_Config.DayTurnoverTime).Date == this.DateOfShow.AddHours(-_Config.DayTurnoverTime).Date);
                        }));

                        if (coll.Count > 1)
                        {
                            coll.Sort("DtDateOfShow", true);

                            foreach (ShowDate sd in coll)
                                _showingsCsv += string.Format("{0},", sd.ShowTime);

                            _showingsCsv = _showingsCsv.TrimEnd(',');
                        }
                    }
                }
                
                return _showingsCsv;
            }
            set { _showingsCsv = null; }
        }

        private string _actString = null;
        private string _actString_Headliner = null;
        /// <summary>
        /// somewhat simplified - just checks the act's name - no pre, post, etc are evaluated
        /// </summary>
        public string ActString
        {
            get
            {
                if (_actString == null && this.JShowActRecords().Count > 0)
                {
                    _actString_Headliner = null;

                    System.Collections.Generic.List<JShowAct> list = new System.Collections.Generic.List<JShowAct>();
                    list.AddRange(this.JShowActRecords());
                    list.Sort(new Utils.Reflector.CompareEntities<JShowAct>(Utils.Reflector.Direction.Ascending, "IDisplayOrder"));

                    foreach (JShowAct jsa in list)
                    {
                        if(_actString_Headliner == null)
                            _actString_Headliner = jsa.ActRecord.Name;

                        _actString += string.Format("{0}~", jsa.ActRecord.Name);
                    }

                    _actString = _actString.TrimEnd('~');
                }
                
                return _actString;
            }
            set { _actString = null; }
        }

        /// <summary>
        /// This is the date that the show was created or announced
        /// </summary>
        public DateTime PseudoPublishDate { get { return (this.DtStamp > this.ShowRecord.AnnounceDate) ? this.DtStamp : this.ShowRecord.AnnounceDate; } }

        public ShowDateDisplay Display = null; //new Display(this);

        public override void Initialize()
        {
            base.Initialize();
            if(this.Display == null)
                this.Display = new ShowDateDisplay(this);
        }

        #region Properties
        [XmlAttribute("IsAutoBilling")]
        public bool IsAutoBilling
        {
            get { return this.BAutoBilling; }
            set { this.BAutoBilling = value; }
        }
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        } 
        public bool CoHeadline
        {
            get 
            {
                JShowActCollection acts = new JShowActCollection();
                acts.AddRange(this.JShowActRecords().GetList().FindAll(delegate(JShowAct match) { return (match.TopBilling_Effective); }));

                int count = acts.Count;

                return (count > 1);
            }
        }
        /// <summary>
        /// Includes the door time - so MM/dd/yyyy hh:mmtt
        /// </summary>
        [XmlAttribute("DateOfShow")]
        public DateTime DateOfShow
        {
            get { return DtDateOfShow; }
            set { this.DtDateOfShow = value; }
        }
        [XmlAttribute("IsLateNightShow")]
        public bool IsLateNightShow
        {
            get { return this.BLateNightShow; }
            set { this.BLateNightShow = value; }
        }
        /// <summary>
        /// Returns an adjustment for late night shows
        /// </summary>
        public DateTime DateOfShow_ToSortBy
        {
            get
            {
                if (this.IsLateNightShow)
                    return this.DtDateOfShow.AddHours(24);

                return this.DateOfShow;
            }
        }
        public bool IsOn
        {
            get
            {
                if (this.ShowStatusRecord.Name == _Enums.ShowDateStatus.OnSale.ToString() || this.ShowStatusRecord.Name == _Enums.ShowDateStatus.SoldOut.ToString())
                    return true;

                return false;
            }
        }
        public string StatusName
        {
            get
            {
                if (this.ShowStatusRecord != null)
                    return ShowStatusRecord.Name;

                return _Enums.ShowDateStatus.OnSale.ToString();
            }
        }
        #endregion

        #region DisplayProperties
        StringBuilder sb = new StringBuilder();

        public string cartPromoter { get { return this.ShowRecord.cartPromoter; } }
        
        public string listMainAct
        {
            get
            {
                sb.Length = 0;

                JShowActCollection coll = new JShowActCollection();
                coll.AddRange(this.JShowActRecords().GetList().FindAll(
                    delegate(JShowAct entity) { return (entity.TopBilling_Effective); }));
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                foreach (JShowAct js in coll)
                {
                    sb.AppendFormat("{0} {1} {2} {3} {4}~",
                        (js.PreText != null) ? js.PreText : string.Empty,
                        js.ActRecord.Name_Displayable,
                        (js.ActText != null) ? js.ActText : string.Empty,
                        (js.Featuring != null) ? js.Featuring : string.Empty,
                        (js.PostText != null) ? js.PostText : string.Empty).ToString().Trim();
                }

                Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);

                return sb.ToString();
            }
        }
        public string wc_CartHeadliner
        {
            get
            {
                sb.Length = 0;

                JShowActCollection coll = new JShowActCollection();
                coll.AddRange(this.JShowActRecords().GetList().FindAll(
                    delegate(JShowAct entity) { return (entity.TopBilling_Effective); }));
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                //always display co headlines
                if (coll.Count > 0)
                {
                    foreach (JShowAct ent in coll)
                    {
                        if (ent.PreText != null && ent.PreText.Trim().Length > 0)
                            sb.AppendFormat("<span class='pretext'>{0}</span> ", HttpUtility.HtmlEncode(ent.PreText.Trim()));

                        sb.AppendFormat("<span class='name'>{0}</span> ", HttpUtility.HtmlEncode(ent.ActRecord.Name_Displayable));

                        if (ent.ActText != null && ent.ActText.Trim().Length > 0)
                            sb.AppendFormat("<span class='extra'>{0}</span> ", HttpUtility.HtmlEncode(ent.ActText.Trim()));

                        if (ent.Featuring != null && ent.Featuring.Trim().Length > 0)
                            sb.AppendFormat("<span class='featuring'>{0}</span> ", HttpUtility.HtmlEncode(ent.Featuring.Trim()));

                        if (ent.PostText != null && ent.PostText.Trim().Length > 0)
                            sb.AppendFormat("<span class='posttext'>{0}</span>", HttpUtility.HtmlEncode(ent.PostText.Trim()));

                        sb.Append("~");
                    }

                    Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                }

                return sb.ToString();
            }
        }
        public string altCartOpeners
        {
            get
            {
                sb.Length = 0;

                JShowActCollection coll = new JShowActCollection();
                coll.AddRange(this.JShowActRecords().GetList().FindAll(
                    delegate(JShowAct entity) { return (!entity.TopBilling_Effective); }));
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                if (coll.Count > 0)
                {
                    sb.AppendFormat("<span class='with'> with </span>");
                    foreach (JShowAct ent in coll)
                    {
                        if (ent.PreText != null && ent.PreText.Trim().Length > 0)
                            sb.AppendFormat("<span class='pretext'>{0}</span> ", HttpUtility.HtmlEncode( ent.PreText.Trim()) );

                        sb.AppendFormat("<span class='name'>{0}</span> ", HttpUtility.HtmlEncode(ent.ActRecord.Name_Displayable));

                        if (ent.ActText != null && ent.ActText.Trim().Length > 0)
                            sb.AppendFormat("<span class='extra'>{0}</span> ", HttpUtility.HtmlEncode(ent.ActText.Trim()));

                        if (ent.Featuring != null && ent.Featuring.Trim().Length > 0)
                            sb.AppendFormat("<span class='featuring'>{0}</span> ", HttpUtility.HtmlEncode(ent.Featuring.Trim()));

                        if (ent.PostText != null && ent.PostText.Trim().Length > 0)
                            sb.AppendFormat("<span class='posttext'>{0}</span>", HttpUtility.HtmlEncode(ent.PostText.Trim()));

                        sb.Append("~");
                    }

                    Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                }

                return sb.ToString();
            }
        }
        public string lst_EventName
        {
            get
            {
                sb.Length = 0;

                //add the act if not in act mode
                if (_Config._Site_Entity_Mode == _Enums.SiteEntityMode.Venue || 
                    _Config._Site_Entity_Mode == _Enums.SiteEntityMode.Promoter)
                {
                    JShowActCollection coll = new JShowActCollection();
                    coll.AddRange(this.JShowActRecords().GetList().FindAll(
                        delegate(JShowAct entity) { return (entity.TopBilling_Effective); }));
                    if (coll.Count > 1)
                        coll.Sort("IDisplayOrder", true);

                    //always display co headlines
                    if (coll.Count > 0)
                    {
                        foreach (JShowAct ent in coll)
                            sb.AppendFormat("{0}~", ent.ActRecord.Name_Displayable);

                        Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                    }
                }

                //add the venue info if not in venue mode
                if (_Config._Site_Entity_Mode == _Enums.SiteEntityMode.Act || 
                    _Config._Site_Entity_Mode == _Enums.SiteEntityMode.Promoter)
                {
                    string state = (this.ShowRecord.VenueRecord.State == null) ? string.Empty : this.ShowRecord.VenueRecord.State.Trim();
                    string city = (this.ShowRecord.VenueRecord.City == null) ? string.Empty : this.ShowRecord.VenueRecord.City.Trim();
                    string divider = (state.Length > 0 && city.Length > 0) ? ", " : string.Empty;
                    string littleAddress = string.Empty;

                    if (state.Trim().Length > 0 || city.Trim().Length > 0)
                        littleAddress = string.Format(" - {0}{1}{2}", city, divider, state);

                    sb.Insert(0, string.Format("{0}{1}", this.ShowRecord.VenueRecord.Name_Displayable, littleAddress));
                }

                sb.Insert(0, string.Format("{0} - ", this.DateOfShow.ToString("MM/dd/yy hh:mmtt")));

                return sb.ToString();
            }
        }
        public string wc_CartOpeners
        {
            get
            {
                sb.Length = 0;

                JShowActCollection coll = new JShowActCollection();
                coll.AddRange(this.JShowActRecords().GetList().FindAll(
                    delegate(JShowAct entity) { return (!entity.TopBilling_Effective); }));
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                if (coll.Count > 0)
                {
                    sb.AppendFormat("<span class='with'> with </span>");
                    foreach (JShowAct ent in coll)
                    {
                        if(ent.PreText != null && ent.PreText.Trim().Length > 0)
                            sb.AppendFormat("<span class='pretext'>{0}</span> ", HttpUtility.HtmlEncode(ent.PreText.Trim()));
                        
                        sb.AppendFormat("<span class='name'>{0}</span> ", HttpUtility.HtmlEncode(ent.ActRecord.Name_Displayable));
                        
                        if(ent.ActText != null && ent.ActText.Trim().Length > 0)
                            sb.AppendFormat("<span class='extra'>{0}</span> ", HttpUtility.HtmlEncode(ent.ActText.Trim()));

                        if(ent.Featuring != null && ent.Featuring.Trim().Length > 0)
                            sb.AppendFormat("<span class='featuring'>{0}</span> ", HttpUtility.HtmlEncode(ent.Featuring.Trim()));

                        if(ent.PostText != null && ent.PostText.Trim().Length > 0)
                            sb.AppendFormat("<span class='posttext'>{0}</span>", HttpUtility.HtmlEncode(ent.PostText.Trim()));

                        sb.Append("~");
                    }

                    Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                }

                return sb.ToString();
            }
        }
        #endregion

        public bool IsSoldOut 
        { 
            get 
            { 
                return this.ShowRecord.IsSoldOut || (this.ShowStatusRecord.Name.ToLower() == _Enums.ShowDateStatus.SoldOut.ToString().ToLower()); 
            } 
        }

        public string DoorTime { get { return DateOfShow.ToShortTimeString().Replace(" PM", "").Replace(" AM", ""); } }
                
        public string AgesString { get { if (this.AgeRecord != null) return this.AgeRecord.Name; return _Config._Default_Age.Name; } }

        /// <summary>
        /// shows the date of the show as well as showname part from its parent show
        /// </summary>
        public string ListingString { get { return string.Format("{0} - {1}", this.DateOfShow.ToString("ddd MM/dd/yyyy hh:mmtt"), 
            this.ShowRecord.ShowNamePartCondense); } }
    }
}
