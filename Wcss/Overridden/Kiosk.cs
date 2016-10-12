using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SubSonic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wcss
{
    public partial class Kiosk_Principal : _PrincipalBase.Principaled
    {
        public Kiosk Kiosk { get; set; }

        public Kiosk_Principal() : base() { }

        public Kiosk_Principal(Kiosk kiosk)
            : base(kiosk)
        {
            Kiosk = kiosk;
        }

        public static void SortListByPrincipal(_Enums.Principal prince, ref List<Kiosk> list)
        {
            //leave for testing ordering
            //foreach (SalePromotion sp in list)
            //{
            //    string name = sp.Name;
            //    int weight = new Kiosk_Principal(sp).PrincipalWeight(prince);
            //    int order = new Kiosk_Principal(sp).PrincipalOrder_Get(prince);
            //}

            list = new List<Kiosk>(
                list
                .OrderBy(x => new Kiosk_Principal(x).PrincipalWeight(prince))
                .ThenBy(x => new Kiosk_Principal(x).PrincipalOrder_Get(prince))
                .ThenBy(x => x.Name));
        }

        public void SyncOrdinals()
        {
            this.Kiosk.VcJsonOrdinal = JsonConvert.SerializeObject(base.SyncOrds());
        }

        private List<_PrincipalBase.PrincipalOrdinal> _principalOrdinalList = null;
        public override List<_PrincipalBase.PrincipalOrdinal> PrincipalOrdinalList
        {
            get
            {

                if (_principalOrdinalList == null)
                {
                    //determine that this collection is a MailerShow Collection
                    if (this.Kiosk.VcJsonOrdinal != null && this.Kiosk.VcJsonOrdinal.Trim().Length > 0)
                    {
                        try
                        {
                            _principalOrdinalList = JsonConvert.DeserializeObject<List<_PrincipalBase.PrincipalOrdinal>>(this.Kiosk.VcJsonOrdinal);
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);

                            //create a default
                            _principalOrdinalList = new List<_PrincipalBase.PrincipalOrdinal>();
                        }
                    }
                    else
                        _principalOrdinalList = new List<_PrincipalBase.PrincipalOrdinal>();
                }

                return _principalOrdinalList;
            }
            set
            {
                this.Kiosk.VcJsonOrdinal = JsonConvert.SerializeObject(value);
                _principalOrdinalList = null;
            }
        }
    }

    public partial class Kiosk : _ImageManager.IImageManagerParent, _PrincipalBase.IPrincipal
    {
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
            this.ImageManager_Delete(false);
        }

        public static readonly int MaxImageWidth = 1920;

        #region Table Properties

        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }

        /// <summary>
        /// Specify in mSecs
        /// </summary>
        [XmlAttribute("BannerTimeout")]
        public int Timeout
        {
            get { return this.ITimeoutMsecs; }
            set { this.ITimeoutMsecs = value; }
        }
        
        [XmlAttribute("Centered_X")]
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

        [XmlAttribute("Centered_Y")]
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

        [XmlAttribute("DateStart")]
        public DateTime DateStart
        {
            get { return (!this.DtStartDate.HasValue) ? DateTime.MinValue : this.DtStartDate.Value; }
            set { this.DtStartDate = (value == DateTime.MinValue) ? (DateTime?)null : value; }
        }

        //dtEndDate
        [XmlAttribute("DateEnd")]
        public DateTime DateEnd
        {
            get { return (!this.DtEndDate.HasValue) ? DateTime.MaxValue : this.DtEndDate.Value; }
            set { this.DtEndDate = (value == DateTime.MaxValue) ? (DateTime?)null : value; }
        }
                
        #endregion

        #region Derived Properties

        /// <summary>
        /// is active, has started and hasn't ended and is displayable in future
        /// </summary>
        /// <param name="cacheOffsetSpan"></param>
        /// <returns></returns>
        public bool IsValidForCacheList(TimeSpan cacheOffset)
        {
            return this.IsActive &&
                this.DateStart < DateTime.Now.Add(cacheOffset) &&
                this.DateEnd > DateTime.Now;
        }

        private bool _hasStarted
        {
            get
            {
                return this.DateStart < DateTime.Now;
            }
        }
        private bool _hasEnded
        {
            get
            {
                return this.DateEnd < DateTime.Now;
            }
        }
        /// <summary>
        /// Ensures that entity is active, has started and has not ended
        /// </summary>
        public bool IsCurrentlyRunning
        {
            get
            {
                return (this.IsActive && this._hasStarted && (!this._hasEnded));
            }
        }


        /// <summary>
        /// Date/Venue/Title/Heads/Openers/Description
        /// </summary>
        public string TextDiv
        {
            get
            {
                return string.Format("<div class=\"{0}\"><div class=\"kiosk-venue-date\">{1} {2}</div>{3}{4}{5}{6}</div>", 
                    string.Format("kiosktext {0}", this.TextCss ?? string.Empty),
                   
                    (this.EventDate != null && this.EventDate.Trim().Length > 0) ? string.Format("<span class=\"kiosk-date\">{0}</span>", this.EventDate) : string.Empty,
                    (this.EventVenue != null && this.EventVenue.Trim().Length > 0) ? string.Format("<span class=\"kiosk-venue\">{0}</span>", this.EventVenue) : string.Empty,

                    (this.EventTitle != null && this.EventTitle.Trim().Length > 0) ? string.Format("<div class=\"kiosk-title\">{0}</div>", this.EventTitle) : string.Empty,
                    (this.EventHeads != null && this.EventHeads.Trim().Length > 0) ? string.Format("<div class=\"kiosk-heads\">{0}</div>", this.EventHeads) : string.Empty,
                    (this.EventOpeners != null && this.EventOpeners.Trim().Length > 0) ? string.Format("<div class=\"kiosk-openers\">{0}</div>", this.EventOpeners) : string.Empty,
                    (this.EventDescription != null && this.EventDescription.Trim().Length > 0) ? string.Format("<div class=\"kiosk-description\">{0}</div>", this.EventDescription) : string.Empty
                    );
            }
        }

        private static Dictionary<string, string> _cssClass = null;
        /// <summary>
        /// no spaces on classnames!
        /// </summary>
        public static Dictionary<string, string> CssClassList
        {
            get
            {
                if (_cssClass == null)
                {
                    _cssClass = new Dictionary<string, string>();
                    _cssClass.Add("Light Text", "kiosk-lighttext");
                    _cssClass.Add("Dark Text", "kiosk-darktext");
                    _cssClass.Add("Top Left", "kiosk-postl");
                    _cssClass.Add("Top Right", "kiosk-postr");
                    _cssClass.Add("Bottom Left", "kiosk-posbl");
                    _cssClass.Add("Bottom Right", "kiosk-posbr");
                    _cssClass.Add("Top Center", "kiosk-postc"); 
                    _cssClass.Add("Middle Center", "kiosk-posmc");
                    _cssClass.Add("Bottom Center", "kiosk-posbc");
                }

                return _cssClass;
            }
        }

        public bool IsLightText { get { return (!IsDarkText); } }//default!
        public bool IsDarkText { get { return (this.ClassList.Contains(Kiosk.CssClassList["Dark Text"])); } }

        public bool IsPosTopRight { get { return (this.ClassList.Contains(Kiosk.CssClassList["Top Right"])); } }
        public bool IsPosTopCenter { get { return (this.ClassList.Contains(Kiosk.CssClassList["Top Center"])); } }
        public bool IsPosMiddleCenter { get { return (this.ClassList.Contains(Kiosk.CssClassList["Middle Center"])); } }
        public bool IsPosBottomCenter { get { return (this.ClassList.Contains(Kiosk.CssClassList["Bottom Center"])); } }
        public bool IsPosTopLeft { get { return (this.ClassList.Contains(Kiosk.CssClassList["Top Left"])); } }
        public bool IsPosBottomRight { get { return (this.ClassList.Contains(Kiosk.CssClassList["Bottom Right"])); } }
        public bool IsPosBottomLeft
        {
            get
            {
                return (this.ClassList.Contains(Kiosk.CssClassList["Bottom Left"]) ||
            ((!IsPosTopRight) && (!IsPosTopCenter) && (!IsPosMiddleCenter) && (!IsPosBottomCenter) && (!IsPosTopLeft) && (!IsPosBottomRight)));
            }
        }//default!
        
        public void ChangeTextColor(string newColorClass)
        {
            this.ClassList.Remove(CssClassList["Light Text"]);
            this.ClassList.Remove(CssClassList["Dark Text"]);

            this.ClassList.Add(CssClassList[newColorClass]);

            UpdateDivTextClass();
        }

        public void ChangeTextPosition(string newPositionName)
        {
            //save the text color - note logic is reverse of above
            string txtColor = (IsLightText) ? CssClassList["Light Text"] : CssClassList["Dark Text"];

            string newPos = CssClassList[newPositionName];

            this.ClassList.Clear();

            this.ClassList.Add(txtColor);
            this.ClassList.Add(newPos);

            UpdateDivTextClass();
        }

        protected void UpdateDivTextClass()
        {
            this.TextCss = string.Join(" ", ClassList);
        }
        
        public string TextColorClass
        {
            get
            {
                return (this.IsDarkText) ? Kiosk.CssClassList["Dark Text"] :
                    Kiosk.CssClassList["Light Text"];//default!
            }
        }

        public string TextColorClassName
        {
            get
            {
                return Kiosk.CssClassList.FirstOrDefault(x => x.Value == this.TextColorClass).Key;
            }
        }

        public string PositionClass
        {
            get
            {
                return (this.IsPosBottomLeft) ? Kiosk.CssClassList["Bottom Left"] : //default! should be first option
                    (this.IsPosBottomRight) ? Kiosk.CssClassList["Bottom Right"] :
                    (this.IsPosTopRight) ? Kiosk.CssClassList["Top Right"] :
                    (this.IsPosTopCenter) ? Kiosk.CssClassList["Top Center"] :
                    (this.IsPosMiddleCenter) ? Kiosk.CssClassList["Middle Center"] :
                    (this.IsPosBottomCenter) ? Kiosk.CssClassList["Bottom Center"] :
                    Kiosk.CssClassList["Top Left"];
            }
        }

        public string PositionClassName
        {
            get
            {
                return Kiosk.CssClassList.FirstOrDefault(x => x.Value == this.PositionClass).Key;
            }
        }

        private List<string> _classList;
        public List<string> ClassList
        {
            get
            {
                if (_classList == null)
                {
                    _classList = new List<string>();

                    if (this.TextCss != null && this.TextCss.Trim().Length > 0)
                        _classList = this.TextCss.Split(' ').ToList();
                }

                return _classList;
            }
        }

        #endregion

        #region Image Mgmt

        private _ImageManager _imageManager = null;
        /// <summary>
        /// Create a new image manager from existing image. Resets width and height.
        /// </summary>
        public _ImageManager ImageManager
        {
            get
            {
                if (_imageManager == null && this.DisplayUrl != null && this.DisplayUrl.Length > 0)
                {
                    this.PicWidth = 0;
                    this.PicHeight = 0;

                    _imageManager = new _ImageManager(string.Format("{0}{1}", _Config._AdvertImageStorage_Local, this.DisplayUrl.ToLower()));

                    this.IPicHeight = PicHeight;//this sets both dims
                }

                return _imageManager;
            }
            set { _imageManager = null; }
        }
        

        public string Url_Original { get { return this.ImageManager.OriginalUrl; } }
        public string Thumbnail_Small { get { return this.ImageManager.Thumbnail_Small; } }
        public string Thumbnail_Large { get { return this.ImageManager.Thumbnail_Large; } }
        public string Thumbnail_Max { get { return this.ImageManager.Thumbnail_Max; } }

        [XmlAttribute("PicWidth")]
        public int PicWidth
        {
            get
            {
                if (this.IPicWidth == 0 && this.DisplayUrl != null && this.DisplayUrl.Trim().Length > 0)
                {
                    try
                    {
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(this.ImageManager.OriginalUrl));
                        this.IPicWidth = (int)p.First;
                        this.IPicHeight = (int)p.Second;

                        _ImageManager.UpdatePictureDimensions(this.Id, Wcss.Kiosk.table.ToString(), this.IPicWidth, this.IPicHeight);
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

        [XmlAttribute("PicHeight")]
        public int PicHeight
        {
            get
            {
                if (this.IPicHeight == 0 && this.DisplayUrl != null && this.DisplayUrl.Trim().Length > 0)
                {
                    try
                    {
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(this.ImageManager.OriginalUrl));
                        this.IPicWidth = (int)p.First;
                        this.IPicHeight = (int)p.Second;

                        _ImageManager.UpdatePictureDimensions(this.Id, Wcss.Kiosk.table.ToString(), this.IPicWidth, this.IPicHeight);
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
                
        /// <summary>
        /// Plumbing to delete an image and clean the entity. Deletes the image, resets image, dimensions and centering
        /// </summary>
        /// <param name="saveToDB">Option to save changes to the DB</param>
        public void ImageManager_Delete(bool saveToDB)
        {
            if (this.ImageManager != null)
                this.ImageManager.Delete();

            this._imageManager = null;

            this.DisplayUrl = null;
            this.PicWidth = 0;
            this.PicHeight = 0;
            this.Centered_X = true;
            this.Centered_Y = true;

            if (saveToDB)
                this.Save();
        }

        /// <summary>
        /// ensures cleanup of old image, resets dimensions, recreates thumbs and saves to db
        /// </summary>
        /// <param name="mappedFile"></param>
        public void ImageManager_AssignNewImage(string mappedFile)
        {
            this.ImageManager_Delete(false);

            //set display url
            this.DisplayUrl = System.IO.Path.GetFileName(mappedFile);
                    
            //force creation of image manager and set vars            
            this.ImageManager.CreateAllThumbs();
                    
            //save the entity changes
            this.Save();
        }

        #endregion

        #region DataSource methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">all, active, notactive</param>
        public static KioskCollection GetKiosksInContext(int startRowIndex, int maximumRows,
            string principal, string status, DateTime startDate, DateTime endDate, string searchTerms)
        {
            KioskCollection coll = new KioskCollection();

            coll.LoadAndCloseReader(SPs.TxGetKiosksInContext(startRowIndex, maximumRows,
                principal, status,
                Utils.ParseHelper.SanitizeDateTimeToSqlDateTime(startDate),
                Utils.ParseHelper.SanitizeDateTimeToSqlDateTime(endDate),
                searchTerms).GetReader());

            return coll;
        }

        public static int GetKiosksInContextCount(
            string principal, string status, DateTime startDate, DateTime endDate, string searchTerms)
        {
            int count = 0;

            using (System.Data.IDataReader dr = SPs.TxGetKiosksInContextCount(
                principal, status,
                Utils.ParseHelper.SanitizeDateTimeToSqlDateTime(startDate),
                Utils.ParseHelper.SanitizeDateTimeToSqlDateTime(endDate),
                searchTerms).GetReader())
            {
                while (dr.Read())
                    count = (int)dr.GetValue(0);
                dr.Close();
            }

            return count;
        }


        #endregion
    }
}
