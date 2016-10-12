using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wcss
{
    #region Text And Images
        
    public partial class MailerBanner
    {
        public int Ordinal { get; set; }
        public int BannerId { get; set; }
        public string ImageUrl { get; set; }
        public string AltText { get; set; }
        public string ClickUrl { get; set; }

        public MailerBanner(int ordinal, SalePromotion p)
        {
            this.Ordinal = ordinal;
            this.BannerId = p.Id;
            this.ImageUrl = string.Format("http://{0}/{1}/Images/Banners/{2}", 
                _Config._DomainName, _Config._VirtualResourceDir, p.BannerUrl);
            this.AltText = p.DisplayText;
            this.ClickUrl = (p.BannerClickUrl != null) ? p.BannerClickUrl : string.Empty;
        }

        public MailerBanner()
        { 
        }

        public MailerBanner(int ordinal)
        {
            this.Ordinal = ordinal;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    
    #endregion

    #region MailerShow

    /// <summary>
    /// Handles show display for the mailer. Serializes to JSON
    /// </summary>    
    public partial class MailerShow
    {
        public string Id { get; set; }
        public int ShowId { get; set; }
        public int Ordinal { get; set; }
        public string Venue { get; set; }
        public string Venue_Class {
            get {
                return Utils.ParseHelper.RemoveWhitespace(this.Venue.ToLower());
            }
        }
        public double SortDate { get; set; }
        public string Date_Display { get; set; }
        public string Date_Day { get; set; }
        public string Date_Month { get; set; }
        public string Date_DayNum { get; set; }
        public string DateOnSale { get; set; }
        public string Ages { get; set; }
        public string Alert { get; set; }
        public string Presents { get; set; }
        public string Headliner { get; set; }
        public string Opener { get; set; }
        public string Extra { get; set; }
        public string SiteUrl { get; set; }        
        public string TixUrl { get; set; }
        public string ImageUrl { get; set; }
        public string FacebookEventUrl { get; set; }
        public string vcPrincipal { get; set; }    

        /// <summary>
        /// Provides a complete path - http...
        /// </summary>
        //[JsonIgnore]
        //public string EventUrl
        //{
        //    get
        //    {
        //        return (this.SiteUrl == null || this.SiteUrl.Trim().Length == 0) ? string.Empty : string.Format("http://{0}/{1}", _Config._DomainName, this.SiteUrl);
        //    }
        //}


        /// <summary>
        /// just a means to ensure that the 
        /// </summary>
        /// <returns></returns>
        private double VerifiedSortDate(DateTime date, string showName)
        {
            string datein = date.ToString("MM dd yyyy hh:mmtt");

            double d = Utils.ParseHelper.DateTime_To_JavascriptDate(date);

            DateTime checkDate = Utils.ParseHelper.JavascriptDate_To_DateTime(d);

            string dateout = checkDate.ToString("MM dd yyyy hh:mmtt");

            return d;
        }

        public MailerShow(int ordinal, Show s)
        {
            Id = System.IO.Path.GetRandomFileName();
            ShowId = s.Id;
            Ordinal = ordinal;
            Venue = Utils.ParseHelper.ConvertTo_ProperCase(s.VenueRecord.Name_Displayable);
            SortDate = VerifiedSortDate(s.FirstShowDate.DateOfShow_ToSortBy, s.DisplayShowName);
            Date_Display = s.FirstShowDate.DtDateOfShow.ToString("ddd MMM dd");
            Date_Day = s.FirstShowDate.DtDateOfShow.ToString("dddd");
            Date_Month = s.FirstShowDate.DtDateOfShow.ToString("MMMM");
            Date_DayNum = s.FirstShowDate.DtDateOfShow.ToString("dd").TrimStart('0');
            DateOnSale = (s.DtDateOnSale == null) ? string.Empty : s.DtDateOnSale.Value.ToString("ddd MMM d");
            Ages = Utils.ParseHelper.ConvertTo_ProperCase(s.FirstShowDate.AgesString);
            Alert = Utils.ParseHelper.ConvertTo_ProperCase(s.ShowAlert ?? string.Empty);
            Presents = Utils.ParseHelper.ConvertTo_ProperCase(s.Display.Promoters_NoMarkup_NoLinks);
            Headliner = Utils.ParseHelper.ConvertTo_ProperCase(s.listHeadliners.Trim());
            Opener = Utils.ParseHelper.ConvertTo_ProperCase(s.listOpeners.Trim()).Replace("With ", "with ");
            Extra = string.Empty;
            SiteUrl = s.FirstShowDate.ConfiguredUrl_withDomain;//absolute path
            TixUrl = s.FirstShowDate.TicketUrl;
            ImageUrl = (s.ImageManager != null) ? s.ImageManager.Thumbnail_Small : string.Empty;//save as virtual
            FacebookEventUrl = s.FacebookEventUrl ?? string.Empty;

            vcPrincipal = s.VcPrincipal;
        }

        //public MailerShow(int ordinal, BTEvent bt)
        //{
        //    Id = System.IO.Path.GetRandomFileName();
        //    ShowId = -1;
        //    Ordinal = ordinal;
        //    Venue = Utils.ParseHelper.ConvertTo_ProperCase("The Boulder Theater");
        //    SortDate = VerifiedSortDate(bt.Date, bt.Title);
        //    Date_Display = bt.Date.ToString("ddd MMM dd");
        //    Date_Day = bt.Date.ToString("dddd");
        //    Date_Month = bt.Date.ToString("MMMM");
        //    Date_DayNum = bt.Date.ToString("dd").TrimStart('0');
        //    DateOnSale = (bt.DateOnSale != DateTime.MinValue) ? bt.DateOnSale.ToString("ddd MMM d") : string.Empty;
        //    Ages = (bt.Ages != null && bt.Ages.Trim().Length > 0) ? Utils.ParseHelper.ConvertTo_ProperCase(bt.Ages.Trim()) : string.Empty;
        //    Alert = string.Empty;
        //    Presents = (bt.Presenter != null && bt.Presenter.Trim().Length > 0) ? Utils.ParseHelper.ConvertTo_ProperCase(bt.Presenter.Trim()) : string.Empty;
        //    Headliner = (bt.Headliners != null && bt.Headliners.Trim().Length > 0) ? Utils.ParseHelper.ConvertTo_ProperCase(bt.Headliners.Trim()) : string.Empty;
        //    Opener = (bt.Openers != null && bt.Openers.Trim().Length > 0) ? Utils.ParseHelper.ConvertTo_ProperCase(bt.Openers.Trim()).Replace("With ", "with ") : string.Empty;
        //    Extra = string.Empty;
        //    SiteUrl = bt.SiteUrl ?? string.Empty;
        //    TixUrl = bt.TixUrl ?? string.Empty;
        //    FacebookEventUrl = bt.FacebookEventUrl ?? string.Empty;

        //    ImportRemoteImage(bt.ImageUrl);            
        //}

        public MailerShow(int ordinal, BT_EventItem bt)
        {
            Id = System.IO.Path.GetRandomFileName();
            ShowId = -1;
            Ordinal = ordinal;
            Venue = Utils.ParseHelper.ConvertTo_ProperCase("The Boulder Theater");
            SortDate = VerifiedSortDate(bt.ShowDate, bt.Title);
            Date_Display = bt.ShowDate.ToString("ddd MMM dd");
            Date_Day = bt.ShowDate.ToString("dddd");
            Date_Month = bt.ShowDate.ToString("MMMM");
            Date_DayNum = bt.ShowDate.ToString("dd").TrimStart('0');
            DateOnSale = (bt.DateOnSale.HasValue) ? bt.DateOnSale.Value.ToString("ddd MMM d") : string.Empty;
            Ages = (bt.Ages != null && bt.Ages.Trim().Length > 0) ? Utils.ParseHelper.ConvertTo_ProperCase(bt.Ages.Trim()) : string.Empty;
            Alert = string.Empty;
            Presents = (bt.PresentedBy != null && bt.PresentedBy.Trim().Length > 0) ? Utils.ParseHelper.ConvertTo_ProperCase(bt.PresentedBy.Trim()) : string.Empty;
            Headliner = (bt.Headliners != null && bt.Headliners.Trim().Length > 0) ? Utils.ParseHelper.ConvertTo_ProperCase(bt.Headliners.Trim()) : string.Empty;
            Opener = (bt.Openers != null && bt.Openers.Trim().Length > 0) ? Utils.ParseHelper.ConvertTo_ProperCase(bt.Openers.Trim()).Replace("With ", "with ") : string.Empty;
            Extra = string.Empty;
            SiteUrl = bt.ShowUrl ?? string.Empty;
            TixUrl = bt.TicketLink ?? string.Empty;
            FacebookEventUrl = bt.FacebookEventUrl ?? string.Empty;
            vcPrincipal = "bt";

            try
            {
                ImportRemoteImage(bt.ShowImage);
            }
            catch (Exception ex) {
                _Error.LogException(ex);
            }
            
        }

        /// <summary>
        /// Save Image url as a virtual path
        /// </summary>
        /// <param name="remoteImageUrl"></param>
        private void ImportRemoteImage(string remoteImageUrl)
        {

            //when creating from a bt show - try to copy the image over to the mailer folder
            //images need to follow the pattern for fox mailerShow uploads!
            if (remoteImageUrl != null && remoteImageUrl.Trim().Length > 0)
            {
                //establish filenames and 
                //ensure the files do not exist
                string mappedUpload = HttpContext.Current.Server.MapPath(string.Format("{0}{1}{2}",
                    _Config._UploadImageStorage_Local,
                    this.Id,
                    Path.GetExtension(remoteImageUrl)
                    ));

                //derive thumbnail filename from upload
                string mappedThumbnail = HttpContext.Current.Server.MapPath(string.Format("{0}{1}",
                    _Config._EmailerImageStorage_Local, Path.GetFileName(mappedUpload)));

                //on the one and a million chance that this is possible
                if (File.Exists(mappedUpload))
                    throw new FileLoadException("mailer image upload file already exists!");
                if (File.Exists(mappedThumbnail))
                    throw new FileLoadException("mailer image thumbnail file already exists!");


                //save the file to the upload dir
                //save a thumbnail of the local image - use Show Image dimensions
                Utils.ImageTools.SaveRemoteImageLocally(remoteImageUrl, mappedUpload, mappedThumbnail, _Config._ShowThumbSizeSm);


                //set the object image urls
                ImageUrl = string.Format("{0}{1}", Wcss._Config._EmailerImageStorage_Local, Path.GetFileName(mappedThumbnail));
            }
        }

        /// <summary>
        /// generic implementation
        /// </summary>
        public MailerShow(int ord)
        {
            Id = System.IO.Path.GetRandomFileName();
            this.Ordinal = ord;            
        }

        public MailerShow()
        {
        //    this.Ordinal = ordinal;            
        }

        /// <summary>
        /// used for generic shows - tba
        /// </summary>
        public MailerShow(int ordinal, string venue, string genericShowName)
        {
            Id = System.IO.Path.GetRandomFileName();
            ShowId = -1;
            Ordinal = ordinal;
            Venue = Utils.ParseHelper.ConvertTo_ProperCase(venue);
            SortDate = (double)0;
            Date_Day = string.Empty;
            Date_Month = string.Empty;
            Date_DayNum = string.Empty;
            DateOnSale = string.Empty;
            Ages = string.Empty;
            Alert = string.Empty;
            Presents = string.Empty;
            Headliner = Utils.ParseHelper.ConvertTo_ProperCase(genericShowName);
            Opener = string.Empty;
            Extra = string.Empty;
            SiteUrl = string.Empty;
            TixUrl = string.Empty;
            ImageUrl = string.Empty;
            FacebookEventUrl = string.Empty;
            vcPrincipal = string.Empty;
        }

        public MailerShow(string jsonString)
        {
            JsonConvert.PopulateObject(jsonString, this);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    #endregion
}
