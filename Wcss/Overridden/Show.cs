using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Threading;
using System.Web.UI;

namespace Wcss
{
    public partial class Show : _ImageManager.IImageManagerParent, _PrincipalBase.IPrincipal
    {
        public bool ShowNameMatches(bool performChangeWhenItDoesntMatch)
        {
            //get first show date
            ShowDateCollection dateColl = new ShowDateCollection();
            dateColl.AddRange(this.ShowDateRecords().GetList().FindAll(
                delegate(ShowDate match) { return (match.IsActive == true); }));
            if (dateColl.Count > 1)
                dateColl.Sort("DtDateOfShow", true);

            if (dateColl.Count == 0)
                return false;

            DateTime firstDate = dateColl[0].DateOfShow;

            //get headliners from first date
            JShowActCollection actColl = new JShowActCollection();
            actColl.AddRange(dateColl[0].JShowActRecords().GetList().FindAll(
                delegate(JShowAct match) { return (match.TopBilling_Effective == true); }));
            if (actColl.Count > 1)
                actColl.Sort("IDisplayOrder", true);

            ActCollection coll = new ActCollection();
            foreach (JShowAct join in actColl)
                coll.Add(join.ActRecord);

            if (coll.Count == 0)
            {
                string error = string.Format("No headliner: ShowId: {0} Name: {1}", this.Id, this.Name);
                _Error.LogException(new Exception(error));
                return false;//dont allow to make a name without a headline act
            }

            string calcdName = Show.CalculatedShowName(firstDate, this.VenueRecord, coll);

            if (performChangeWhenItDoesntMatch && (this.Name != calcdName))
            {
                this.Name = calcdName;

                this.Save();

                return true;
            }

            return (this.Name == calcdName);
        }

        public static string CalculatedShowName(DateTime sDate, Venue venue, ActCollection headlineActs)
        {
            //300 total length - but lets keep to 255 for authnet
            //yyyy-mm-dd hh:mmtt - (22)
            //venue - 70
            //name - 155
            StringBuilder sb = new StringBuilder();
            string _venue = (venue.Name_Displayable.Length > 55) ? venue.Name_Displayable.Substring(0, 55) : venue.Name_Displayable.Trim();
            _venue = _venue.Replace('-',' ');
            _venue = Utils.ParseHelper.DoubleSpaceToSingle(_venue);

            StringBuilder _head = new StringBuilder();

            foreach (Act act in headlineActs)
                _head.AppendFormat("{0}~", act.Name);

            Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(_head);

            if (_head.Length > 150)
            {
                _head.Length = 150;
                _head.Append("...");
            }
        
            sb.AppendFormat("{0} {1} - {2} - {3}", sDate.ToString("yyyy/MM/dd"), sDate.ToString("hh:mm tt"), _venue, _head.ToString());

            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// Date from Show.Name - first portion of the show name - the date!
        /// dont store locally - only use as a formula
        /// </summary>]
        public DateTime GetShowDatePart
        {
            get
            {
                string name = this.Name;
                return DateTime.Parse(name.Substring(0, 19).Trim());
            }
        }

        /// <summary>
        /// Venue and Main Act from Show.Name - everything after the date in the show name
        /// dont store locally - only use as a formula
        /// </summary>]
        public string GetShowVenueMainActPart 
        {
            get
            {
                string name = this.Name;
                return name.Substring(name.IndexOf("-", 0) + 1, name.Length - (name.IndexOf("-", 0) + 1)).Trim();
            }
        }

        /// <summary>
        /// Main Act from Show.Name - everything after the venue in the show name
        /// dont store locally - only use as a formula
        /// </summary>]
        public string GetShowMainActPart
        {
            get
            {
                string name = this.GetShowVenueMainActPart;
                return name.Substring(name.IndexOf("-", 0) + 1, name.Length - (name.IndexOf("-", 0) + 1)).Trim();
            }
        }


        public string GetDisplayUrl() { return this.DisplayUrl; }
        public void SetDisplayUrl(string s) { this.DisplayUrl = s; }
        public _ImageManager GetImageManager() { return this.ImageManager; }
        public int GetPicWidth() { return this.PicWidth; }
        public void SetPicWidth(int i) { this.PicWidth = i; }
        public int GetPicHeight() { return this.PicHeight; }
        public void SetPicHeight(int i) { this.PicHeight = i; }
        public bool GetCtrX() { return this.Centered_X; }
        public void SetCtrX(bool b) { this.Centered_X = b; }
        public bool GetCtrY() { return this.Centered_Y; }
        public void SetCtrY(bool b) { this.Centered_Y = b; }
        public void ResetImageManager()
        {
            this.ImageManager_Delete();
        }

        private string _ShowOwnedBy = null;
        public string ShowOwnedBy
        {
            get
            {
                if (_ShowOwnedBy == null)
                {
                    string prince = (this.VcPrincipal ?? string.Empty).Trim().ToLower();
                    _ShowOwnedBy = string.Format("{0}",
                        (prince == "fox") ? "Fox Theatre" :
                        (prince == "bt") ? "Boulder Theater" :
                        (prince == "z2") ? "Z2 Entertainment" : "???");
                }

                return _ShowOwnedBy;
            }
        }

        public VdShowInfo VdShowInfoRecord
        {
            get
            {
                if (this.VdShowInfoRecords().Count == 0)
                {
                    VdShowInfo ent = new VdShowInfo();
                    ent.DateStamp = DateTime.Now;
                    ent.DateModified = DateTime.Now;
                    this.VdShowInfoRecords().Add(ent);
                    this.VdShowInfoRecords().SaveAll();
                }

                return this.VdShowInfoRecords()[0];
            }
        }

        public bool ContainsShowDateWithMatchingUrl(string url)
        {
            int idx = this.ShowDateRecords().GetList()
                .FindIndex(delegate(ShowDate match)
                {
                    return (match.ConfiguredUrl == url && match.IsActive);
                });

            return idx != -1 && this.IsDisplayable;
        }

        
        private string _nameForAdminListing_htmlsnippet = null;
        /// <summary>
        /// This is a helper function to format the event's ;listing
        /// </summary>
        public string NameForAdminListing_htmlsnippet
        {
            get
            {
                if(_nameForAdminListing_htmlsnippet == null)
                {
                //Nov 12 '14 @ Venue
                //ActName(eventnamepart)
                }

                return _nameForAdminListing_htmlsnippet;
            }
        }
        
        #region Show Image & DisplayUrl

         #region Image Mgmt

        /// <summary>
        /// note this method is only available for show
        /// It does not save record!
        /// </summary>
        public void ImageManager_Delete()
        {
            //TODO: make sure we are not deleting any default pics - venues!!
            if (this.DisplayUrl != null && this.DisplayUrl.Trim().Length > 0)
            {
                int matchingUrls = new SubSonic.Select().From<Show>().Where("DisplayUrl").IsEqualTo(this.DisplayUrl).GetRecordCount();

                if(matchingUrls == 1)
                    this.ImageManager.Delete();
            }

            //have this out of the loop so that non-synced urls can be cleaned up            
            this._imageManager = null;

            this.PicWidth = 0;
            this.PicHeight = 0;
            this.DisplayUrl = null;
            this.Centered_X = true;
            this.Centered_Y = true;
            this.Save();
        }

        private _ImageManager _imageManager = null;
        public _ImageManager ImageManager
        {
            get
            {
                if (_imageManager == null && this.DisplayUrl != null && this.DisplayUrl.Length > 0)
                {
                    this.PicWidth = 0;
                    this.PicHeight = 0;

                    _imageManager = new _ImageManager(string.Format("{0}{1}", _Config._ShowImageStorage_Local, this.DisplayUrl.ToLower()));

                    this.IPicHeight = PicHeight;//this sets both dims
                }

                return _imageManager;
            }
            set { _imageManager = null; }
        }

        /// <summary>
        /// Image link: The link to the originally uploaded file - all thumbnails are created from this file. This is the virtual path to 
        /// the dir eg /resources/images/shows/
        /// </summary>
        public string Url_Original { get { return this.ImageManager.OriginalUrl; } }
        public string Thumbnail_Small { get { return this.ImageManager.Thumbnail_Small; } }
        public string Thumbnail_Large { get { return this.ImageManager.Thumbnail_Large; } }
        public string Thumbnail_Max { get { return this.ImageManager.Thumbnail_Max; } }

        public int PicWidth
        {
            get
            {
                if (this.IPicWidth == 0 && DisplayUrl != null)
                {
                    try
                    {
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(this.ImageManager.OriginalUrl));
                        this.IPicWidth = (int)p.First;
                        this.IPicHeight = (int)p.Second;

                        _ImageManager.UpdatePictureDimensions(this.Id, Wcss.Show.table.ToString(), this.IPicWidth, this.IPicHeight);
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                    }
                }

                return this.IPicWidth;
            }
            set
            {
                this.IPicWidth = value;
            }
        }

        public int PicHeight
        {
            get
            {
                if (this.IPicHeight == 0 && DisplayUrl != null)
                {
                    try
                    {
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(this.ImageManager.OriginalUrl));
                        this.IPicWidth = (int)p.First;
                        this.IPicHeight = (int)p.Second;

                        _ImageManager.UpdatePictureDimensions(this.Id, Wcss.Show.table.ToString(), this.IPicWidth, this.IPicHeight);
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                    }
                }

                return this.IPicHeight;
            }
            set
            {
                this.IPicHeight = value;
            }
        }
        //we use the image dimensions to figure out which where the imageis coming from
  
        public decimal ImageRatio
        {
            get
            {
                if (PicWidth == 0 || PicHeight == 0)
                    return 0.0m;

                decimal dec = (decimal)PicWidth / (decimal)PicHeight;
                return decimal.Round(dec, 4);
            }
        }
        public bool IsSquare { get { return PicHeight == PicWidth; } }
        public bool IsLandscape { get { return PicHeight < PicWidth; } }
        public bool IsPortrait { get { return PicHeight > PicWidth; } }

        #endregion

        /// <summary>
        /// returns a single image for the show. will return the headliner image - otherwise, an empty string. this is not null
        /// </summary>
        private string _showImageUrl = null;
        public string ShowImageUrl
        {
            get
            {
                if (_showImageUrl == null && this.ImageManager != null)
                {
                    _showImageUrl = this.ImageManager.Thumbnail_Max;
                }

                return _showImageUrl;
            }
            set
            {
                _showImageUrl = value;
            }
        }

        public string ShowImageUrl_Backstretch
        {
            get
            {
                if (ShowImageUrl == null || ShowImageUrl.Trim().Length == 0)
                    return null;

                return string.Format("{0}{1}{2}{3}{4}", 
                    ShowImageUrl,
                    //the defaults are true - so only do this if false
                    ((!this.Centered_Y) || (!this.Centered_X)) ? "?" : string.Empty,
                    (!this.Centered_Y) ? "y=0" : string.Empty,
                    ((!this.Centered_Y) && (!this.Centered_X)) ? "&" : string.Empty,//add ampersand if both options
                    (!this.Centered_X) ? "x=0" : string.Empty
                    );
            }
        }

        #endregion

        public ShowDisplay Display = null; //new Display(this);
        
        public override void Initialize()
        {
            base.Initialize();
            if(this.Display == null)
                this.Display = new ShowDisplay(this);
        }
        
        public bool AllShowDatesSoldOut
        {
            get
            {
                foreach (ShowDate sd in this.ShowDateRecords())
                    if (!sd.IsSoldOut)
                        return false;

                return true;
            }
        }

        public bool IsSoldOut
        {
            get
            {
                return this.BSoldOut;
            }
            set
            {
                this.BSoldOut = value;
            }
        }

        public bool Centered_X
        {
            get
            {
                return this.BCtrX;
            }
            set
            {
                this.BCtrX = value;
            }
        }

        public bool Centered_Y
        {
            get
            {
                return this.BCtrY;
            }
            set
            {
                this.BCtrY = value;
            }
        }

        public bool IsVirtuallySoldOut
        {
            get
            {
                ShowDateCollection coll = new ShowDateCollection();
                coll.AddRange(this.ShowDateRecords().GetList().FindAll(delegate(ShowDate match) { return (match.IsActive == true); }));

                foreach (ShowDate sd in coll)
                {
                    if (!sd.IsSoldOut)
                        return false;
                }

                return (coll.Count > 0);//return false if there are no active shows
            }
        }

        #region Properties

        public _Enums.JustAnnouncedStatus JustAnnouncedStatus 
        {
            get 
            {
                if(this.VcJustAnnouncedStatus == null)
                    return _Enums.JustAnnouncedStatus.normal;

                return (_Enums.JustAnnouncedStatus)Enum.Parse(typeof(_Enums.JustAnnouncedStatus), this.VcJustAnnouncedStatus, true);
            }
            set 
            {
                if (value == _Enums.JustAnnouncedStatus.normal)
                    this.VcJustAnnouncedStatus = null;
                else
                    this.VcJustAnnouncedStatus = value.ToString().ToLower();
            }
        }

        public DateTime AnnounceDate
        {
            get { return (!this.DtAnnounceDate.HasValue) ? Utils.Constants._MinDate : this.DtAnnounceDate.Value; }
            set { this.DtAnnounceDate = (value > Utils.Constants._MinDate) ? value : (DateTime?)null; }
        }
        public DateTime AnnounceDate_Virtual {
            get {
                return (AnnounceDate != Utils.Constants._MinDate) ? this.AnnounceDate : this.DtStamp;
            }
        }


        public DateTime DateOnSale
        {
            get { return (!this.DtDateOnSale.HasValue) ? Utils.Constants._MinDate : this.DtDateOnSale.Value; }
            set { this.DtDateOnSale = (value > Utils.Constants._MinDate) ? value : (DateTime?)null; }
        }
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }
        /// <summary>
        /// ensures show is announced and active - see other IsDisplayable properties (web and ticketing)
        /// </summary>
        public bool IsDisplayable
        {
            get { return this.IsActive && this.AnnounceDate < DateTime.Now; }
        }
        public string ShowHeader_Derived { get { return (this.ShowHeader == null) ? string.Empty : this.ShowHeader; } set { this.ShowHeader = value.Trim(); } }

        public string ShowWriteup_Derived { get { return (this.ShowWriteup == null) ? string.Empty : this.ShowWriteup; } set { this.ShowWriteup = value.Trim(); } }
        
        #endregion
        
        public bool AllShowsAreDayOfShowOrPast
        {
            get
            {
                foreach (ShowDate sd in this.ShowDateRecords().GetList()
                    .FindAll(delegate(ShowDate match) { return (match.IsActive == true); }))
                {
                    if (sd.DateOfShow.Date > DateTime.Now.Date)
                        return false;
                }

                return true;
            }
        }

        public bool IsCompletelyOfStatus(_Enums.ShowDateStatus st)
        {
            foreach (ShowDate sd in this.ShowDateRecords())
            {
                if (sd.IsActive && sd.ShowStatusRecord.Name != st.ToString())
                    return false;
            }

            return true;
        }
        public ActCollection HeadlinerCollection
        {
            get
            {
                ActCollection coll = new ActCollection();

                foreach (ShowDate sd in this.ShowDateRecords())
                {
                    JShowActCollection coll2 = new JShowActCollection();
                    coll2.AddRange(sd.JShowActRecords().GetList().FindAll(delegate(JShowAct match) { return (match.TopBilling_Effective); }));
                    if (coll2.Count > 1)
                        coll2.Sort("IDisplayOrder", true);

                    foreach (JShowAct js in coll2)
                    {
                        if (!coll.Contains(js.ActRecord))
                            coll.Add(js.ActRecord);
                    }
                }

                return coll;
            }
        }

       
        private string _displayName = null;
        public string DisplayShowName
        {
            get
            {
                if (_displayName == null)
                    _displayName = string.Format("{0} {1}{2} {3}", this.GetShowVenueMainActPart, this.VenueRecord.City,
                        (this.VenueRecord.City != null && this.VenueRecord.City.Trim().Length > 0 && this.VenueRecord.State != null &&
                        this.VenueRecord.State.Trim().Length > 0) ? ", " : string.Empty, this.VenueRecord.State).Trim();

                return _displayName;
            }
        }
        private string _displayVenueName = null;
        public string DisplayVenueName
        {
            get
            {
                if (_displayVenueName == null)
                    _displayVenueName = string.Format("{0} - {1}{2} {3}", this.VenueRecord.Name, this.VenueRecord.City,
                        (this.VenueRecord.City != null && this.VenueRecord.City.Trim().Length > 0 && this.VenueRecord.State != null &&
                        this.VenueRecord.State.Trim().Length > 0) ? ", " : string.Empty, this.VenueRecord.State).Trim();

                return _displayVenueName;
            }
        }
        
        public bool HasPromoter(Promoter p)
        {
            foreach (JShowPromoter js in this.JShowPromoterRecords())
                if (js.PromoterRecord.Id == p.Id) return true;

            return false;
        }

        public bool HasShowDate(int showDateId)
        {
            foreach (ShowDate sd in this.ShowDateRecords())
            {
                if (sd.Id == showDateId) return true;
            }

            return false;
        }

        private ShowDate _firstShowDate = null;
        public ShowDate FirstShowDate
        {
            get
            {
                DateTime firstdate = FirstDate;

                return _firstShowDate;
            }
        }
        private DateTime _firstDate = DateTime.MinValue;
        [XmlAttribute("FirstDate")]
        public DateTime FirstDate
        {
            get
            {
                if(_firstDate == DateTime.MinValue)
                {
                    _firstShowDate = null;

                    if (this.ShowDateRecords().Count == 0)
                    {
                        _firstDate = Utils.Constants._MinDate;
                    }
                    else
                    {
                        ShowDateCollection coll = new ShowDateCollection();
                        coll.AddRange(this.ShowDateRecords().GetList().FindAll(
                            delegate(ShowDate match) { return (match.IsActive); }));
                        if (coll.Count > 1)
                            coll.Sort("DateOfShow_ToSortBy", true);

                        if (coll.Count == 0)
                            _firstDate = Utils.Constants._MinDate;
                        else
                        {
                            _firstShowDate = coll[0];
                            _firstDate = _firstShowDate.DateOfShow;
                        }
                    }


                    if (_firstDate == DateTime.MinValue || _firstDate == Utils.Constants._MinDate)
                        _firstDate = this.GetShowDatePart;
                }

                return _firstDate;
            }
        }
        private DateTime _lastDate = DateTime.MinValue;
        [XmlAttribute("LastDate")]
        public DateTime LastDate
        {
            get
            {
                if (_lastDate == DateTime.MinValue)
                {
                    if (this.ShowDateRecords().Count == 0)
                        _lastDate = Utils.Constants._MinDate;
                    else
                    {
                        ShowDateCollection coll = new ShowDateCollection();
                        coll.AddRange(this.ShowDateRecords().GetList().FindAll(
                            delegate(ShowDate match) { return (match.IsActive); }));
                        if (coll.Count > 1)
                            coll.Sort("DateOfShow_ToSortBy", false);

                        if (coll.Count == 0)
                            _lastDate = Utils.Constants._MinDate;
                        else
                            _lastDate = coll[0].DateOfShow;
                    }

                    if (_lastDate == DateTime.MinValue || _lastDate == Utils.Constants._MinDate)
                        _lastDate = this.GetShowDatePart;
                }

                return _lastDate;
            }
        }
       
        public bool IsLateShowOf(ShowDate sd)
        {
            DateTime LateNightShowTime = sd.DateOfShow.AddDays(1).Date.AddHours(3);//until 3AM

            //if this show contains an active showdate that is a late night show of sd then true
            foreach (ShowDate sDate in this.ShowDateRecords())
            {
                //if active and the status is good and if the date is past the show to compare with and less than 3 in the morning of the next day
                if (sDate.IsActive && sDate.IsOn && sDate.DateOfShow > sd.DateOfShow && sDate.DateOfShow < LateNightShowTime)
                {
                    return true;
                }
            }

            return false;
        }

        public void Deactivate()
        {
            this.IsActive = false;

            ShowStatus status = (ShowStatus)_Lookits.ShowStatii.GetList().Find(delegate (ShowStatus match) { return match.Name == _Enums.ShowDateStatus.NotActive.ToString(); } );

            if(status != null)
                foreach (ShowDate sd in this.ShowDateRecords())
                    sd.ShowStatusRecord = status;
        }

        public ActCollection GetAllActs()
        {
            ActCollection coll = new ActCollection();

            foreach (ShowDate sd in this.ShowDateRecords())
            {
                JShowActCollection coll2 = new JShowActCollection();
                coll2.AddRange(sd.JShowActRecords());
                if (coll2.Count > 1)
                    coll2.Sort("IDisplayOrder", true);

                foreach (JShowAct js in coll2)
                {
                    if(!coll.Contains(js.ActRecord))
                        coll.Add(js.ActRecord);
                }
            }

            return coll;
        }

        public Show CopyShow(DateTime newDate, string userName, Guid providerUserKey)
        {
            Show s = null;
            try
            {
                s = new Show();
                s.ApplicationId = this.ApplicationId;
                s.BActive = false;//!IMPORTANT
                s.ShowHeader = this.ShowHeader;
                s.ShowWriteup = this.ShowWriteup;
                s.BSoldOut = false;
                s.DisplayNotes = this.DisplayNotes;
                s.DisplayUrl = this.DisplayUrl;
                s.Centered_X = this.Centered_X;
                s.Centered_Y = this.Centered_Y;
                s.DtAnnounceDate = this.DtAnnounceDate;
                s.DtDateOnSale = this.DtDateOnSale;
                s.DtStamp = DateTime.Now;
                s.VcPrincipal = this.VcPrincipal;

                //construct name from new date
                s.Name = Show.CalculatedShowName(newDate, this.Venue, this.HeadlinerCollection);

                s.ShowImageUrl = this.ShowImageUrl;
                s.ShowTitle = this.ShowTitle;
                s.TVenueId = this.TVenueId;

                //show collections - promoters/links
                foreach (JShowPromoter jsp in this.JShowPromoterRecords())
                {
                    JShowPromoter newPromo = new JShowPromoter();
                    newPromo.DtStamp = DateTime.Now;
                    newPromo.IDisplayOrder = jsp.IDisplayOrder;
                    newPromo.PostText = jsp.PostText;
                    newPromo.PreText = jsp.PreText;
                    newPromo.PromoterText = jsp.PromoterText;
                    newPromo.TPromoterId = jsp.TPromoterId;

                    //newPromo.TShowId = this.Id;
                    s.JShowPromoterRecords().Add(newPromo);
                }

                foreach (ShowLink sl in this.ShowLinkRecords())
                {
                    ShowLink newLink = new ShowLink();
                    newLink.BActive = sl.BActive;
                    newLink.DisplayText = sl.DisplayText;
                    newLink.DtStamp = DateTime.Now;
                    newLink.IDisplayOrder = sl.IDisplayOrder;
                    newLink.LinkUrl = sl.LinkUrl;

                    //newLink.TShowId = this.Id;
                    s.ShowLinkRecords().Add(newLink);
                }

                s.Save();

                //dates - acts
                //only add from first showdate            
                ShowDate dateToCopy = this.FirstShowDate;

                ShowDate newItem = new ShowDate();
                newItem.DtStamp = DateTime.Now;
                newItem.TShowId = s.Id;
                newItem.DateOfShow = DateTime.Parse(newDate.ToString("yyyy-MM-dd hh:mmtt"));
                newItem.IsAutoBilling = dateToCopy.IsAutoBilling;
                newItem.MenuBilling = dateToCopy.MenuBilling;
                newItem.IsActive = true;//always make new additions active - allow change in edit mode
                newItem.ShowTime = dateToCopy.ShowTime;
                newItem.TAgeId = dateToCopy.AgeRecord.Id;
                newItem.PricingText = (dateToCopy == null) ? null : dateToCopy.PricingText;                

                ShowStatus onsale = _Lookits.ShowStatii.GetList().Find(delegate(ShowStatus match) { return (match.Name == _Enums.ShowDateStatus.OnSale.ToString()); });
                newItem.TStatusId = onsale.Id;                

                newItem.Save();

                //copy all acts
                foreach (JShowAct join in dateToCopy.JShowActRecords())
                {
                    JShowAct js = new JShowAct();
                    js.TShowDateId = newItem.Id;//this has changed from join !!!
                    js.TActId = join.TActId;
                    js.PreText = join.PreText;
                    js.ActText = join.ActText;
                    js.Featuring = join.Featuring;
                    js.PostText = join.PostText;
                    js.DisplayOrder = join.DisplayOrder;
                    js.TopBilling = join.TopBilling;

                    newItem.JShowActRecords().Add(js);
                }

                newItem.JShowActRecords().SaveAll();

                s.ShowDateRecords().Add(newItem);

                //update showname when all items have been added in 
                //and saved?
                ShowNameMatches(true);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                _Error.SendAdministrativeEmail(string.Format("Show copy failed. Trying to copy from - {0} TO {1}", this.Name, newDate.ToString()));

                if (s != null)
                    Show.Delete(s.Id);

                s = null;

                //let it bubble
                throw (ex);
            }

            return s;
        }

        #region Child Collections

        #region ShowDates
        /// <summary>
        /// please note that dateToCopy can be null
        /// </summary>
        /// <param name="dateToAdd"></param>
        /// <param name="showTime"></param>
        /// <param name="dateToCopy"></param>
        /// <returns></returns>
        public ShowDate AddShowDateFromShowDate(DateTime dateToAdd, string showTime, ShowDate dateToCopy, string userName, Guid providerUserKey)
        {
            ShowDate newItem = new ShowDate();
            newItem.DtStamp = DateTime.Now;
            newItem.TShowId = this.Id;
            newItem.DateOfShow = DateTime.Parse(dateToAdd.ToString("yyyy-MM-dd hh:mmtt"));
            newItem.IsAutoBilling = dateToCopy.IsAutoBilling;
            newItem.MenuBilling = dateToCopy.MenuBilling;
            newItem.IsActive = true;//always make new additions active - allow change in edit mode
            newItem.ShowTime = (dateToCopy == null) ? showTime : dateToCopy.ShowTime;
            Age age = (dateToCopy == null) ? (Age)_Lookits.Ages.GetList().Find(delegate(Age match) { return (match.Name == _Config._Default_Age.ToString()); }) :
                dateToCopy.AgeRecord;
            newItem.TAgeId = (age != null) ? age.Id : 0;
            newItem.PricingText = (dateToCopy == null) ? null : dateToCopy.PricingText;

            ShowStatus newStatus = (dateToCopy == null) ?
                (ShowStatus)_Lookits.ShowStatii.GetList().Find(delegate(ShowStatus match) { return (match.Name.ToLower() == "onsale"); }) :
                dateToCopy.ShowStatusRecord;
            newItem.TStatusId = (newStatus != null) ? newStatus.Id : 0;            

            try
            {
                newItem.Save();
            }
            catch (Exception e)
            {
                _Error.LogException(e);
                throw e;
            }

            //save children
            if (dateToCopy != null)
            {
                //verify that date to add is valid - cannot exist anywhere else
                ShowDate exists = this.ShowDateRecords().GetList().Find(
                    delegate(ShowDate match) { return (match.DateOfShow == dateToAdd); });

                if (exists != null)
                    throw new Exception(string.Format("A date already exists for the date and time specified. Please edit that date."));

                //copy all acts
                foreach (JShowAct join in dateToCopy.JShowActRecords())
                {
                    JShowAct js = new JShowAct();
                    js.TShowDateId = newItem.Id;//this has changed from join !!!
                    js.TActId = join.TActId;
                    js.PreText = join.PreText;
                    js.ActText = join.ActText;
                    js.Featuring = join.Featuring;
                    js.PostText = join.PostText;
                    js.DisplayOrder = join.DisplayOrder;
                    js.TopBilling = join.TopBilling;

                    newItem.JShowActRecords().Add(js);
                }

                newItem.JShowActRecords().SaveAll();
            }

            this.ShowDateRecords().Add(newItem);

            return newItem;
        }

        public bool DeleteShowDate(int idx)
        {
            ShowDate entity = (ShowDate)this.ShowDateRecords().Find(idx);

            if (entity != null)
            {
                try
                {
                    this.ShowDateRecords().Remove(entity);

                    ShowDate.Delete(idx);

                    this.ShowDateRecords().SaveAll();

                    return true;
                }
                catch (Exception e)
                {
                    _Error.LogException(e);
                    throw e;
                }
            }

            return false;
        }
        #endregion

        #endregion

    }
}



// LEGACY
/*
public static string CalculatedShowName(DateTime sDate, Venue venue, ActCollection headlineActs)
{
    //300 total length - but lets keep to 255 for authnet
    //yyyy-mm-dd hh:mmtt - (22)
    //venue - 70
    //name - 155

    //format the proper date time
    StringBuilder sb = new StringBuilder();

    string _venue = (venue.Name_Displayable.Length > 55) ? venue.Name_Displayable.Substring(0, 55) + "..." : venue.Name_Displayable.Trim();

    StringBuilder _head = new StringBuilder();

    foreach (Act act in headlineActs)
        _head.AppendFormat("{0}~", act.Name);

    Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(_head);

    if (_head.Length > 150)
    {
        _head.Length = 150;
        _head.Append("...");
    }

    sb.AppendFormat("{0} {1} - {2} - {3}", sDate.ToString("yyyy/MM/dd"), sDate.ToString("hh:mm tt"), _venue, _head.ToString());

    return sb.ToString().ToUpper();
}

*/