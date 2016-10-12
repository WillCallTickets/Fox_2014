using System;
using System.IO;

namespace Wcss
{
    public class _ImageManager
    {
        public interface IImageManagerParent
        {
            string GetDisplayUrl();
            void SetDisplayUrl(string s);
            _ImageManager GetImageManager();
            int GetPicWidth();
            void SetPicWidth(int i);
            int GetPicHeight();
            void SetPicHeight(int i);
            bool GetCtrX();
            void SetCtrX(bool b);
            bool GetCtrY();
            void SetCtrY(bool b);
            /// <summary>
            /// Deletes the image mgr to force re-creation of thumbs. Also reset width, height and centering values
            /// </summary>
            void ResetImageManager();
        }

        private string _originalUrl = null;
        private string _OriginalUrl
        {
            get{
                return _originalUrl;
            }
            set{
                _originalUrl = value;
            }
        }
        /// <summary>
        /// Path function creates backslashes - must replace
        /// </summary>
        private string _originalDirectory_Virtual { get { return Path.GetDirectoryName(_OriginalUrl).Replace("\\","/"); } }
        private string _originalFileName { get { return Path.GetFileName(_OriginalUrl); } }

        private _ImageManager() { }
        public _ImageManager(string originalUrl)
        {
            _OriginalUrl = originalUrl;
        }

        public static void UpdatePictureDimensions(int idx, string tableName, int width, int height)
        {
            try
            {
                SPs.TxPictureUpdate(idx, tableName, width, height).Execute();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        public static void EnsureThumbDirectories(string context)
        {
            string ctx = null;

            if (context.ToLower() == "act")
                ctx = _Config._ActImageStorage_Local;
            else if (context.ToLower() == "venue")
                ctx = _Config._VenueImageStorage_Local;
            else if (context.ToLower() == "show")
                ctx = _Config._ShowImageStorage_Local;
            else if (context.ToLower() == "promoter")
                ctx = _Config._PromoterImageStorage_Local;
            else if (context.ToLower() == "advert")
                ctx = _Config._AdvertImageStorage_Local;

            if (ctx != null)
            {
                string smallPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}", ctx, _ImageManager.smallThumbPath));
                string largePath = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}", ctx, _ImageManager.largeThumbPath));
                string maxPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}", ctx, _ImageManager.maxThumbPath));

                if (!Directory.Exists(smallPath))
                    Directory.CreateDirectory(smallPath);
                if (!Directory.Exists(largePath))
                    Directory.CreateDirectory(largePath);
                if (!Directory.Exists(maxPath))
                    Directory.CreateDirectory(maxPath);
            }
            else
            {
                if (context.ToLower() == "uploads")
                    ctx = _Config._UploadImageStorage_Local;
                else if (context.ToLower() == "mailer")
                    ctx = _Config._EmailerImageStorage_Local;

                if (ctx != null)
                {
                    string mappedPath = System.Web.HttpContext.Current.Server.MapPath(ctx);
                    if (!Directory.Exists(mappedPath))
                        Directory.CreateDirectory(mappedPath);
                }
            }
        }


        public static string smallThumbPath = "/thumbsm/";
        public static string largeThumbPath = "/thumblg/";
        public static string maxThumbPath = "/thumbmx/";

        /// <summary>
        /// be sure to reset pictureUrl or DiplayUrl as well as pic height and width on parent
        /// </summary>
        public void Delete()
        {
            if (_OriginalUrl != null && _OriginalUrl.Trim().Length > 0)
            {
                //if we can find the local thumbnail - delete it
                string mapped = System.Web.HttpContext.Current.Server.MapPath(_OriginalUrl);

                if (File.Exists(mapped))
                    File.Delete(mapped);

                DeleteThumbnails();

                _OriginalUrl = null;
            }
        }
        public void DeleteThumbnails()
        {
            DeleteThumbnails_Small();
            DeleteThumbnails_Large();
            DeleteThumbnails_Max();

            DeleteRemoteThumbnails();
        }
        public void DeleteThumbnails_Small()
        {
            if (_OriginalUrl != null && _OriginalUrl.Trim().Length > 0)
            {
                string small = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}{2}", _originalDirectory_Virtual, smallThumbPath, _originalFileName));
                if (File.Exists(small)) File.Delete(small);
            }
        }
        public void DeleteThumbnails_Large()
        {
            if (_OriginalUrl != null && _OriginalUrl.Trim().Length > 0)
            {
                string large = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}{2}", _originalDirectory_Virtual, largeThumbPath, _originalFileName));
                if (File.Exists(large)) File.Delete(large);
            }
        }
        public void DeleteThumbnails_Max()
        {
            if (_OriginalUrl != null && _OriginalUrl.Trim().Length > 0)
            {
                string max = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}{2}", _originalDirectory_Virtual, maxThumbPath, _originalFileName));
                if (File.Exists(max)) File.Delete(max);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        internal void DeleteRemoteThumbnails()
        {
            return;
        }


        //small thumbnail - local,remote and effective

        /// <summary>
        /// determines if file exists before handing back a url
        /// </summary>
        public string OriginalUrl
        {
            get
            {   
                //added in http to deal with old images                
                if (_OriginalUrl == null || (_OriginalUrl.ToLower().IndexOf("://") != -1) || (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(_OriginalUrl))))
                    return string.Empty;

                string path = Path.GetDirectoryName(_OriginalUrl).Replace("\\", "/");
                string file = Path.GetFileName(_OriginalUrl);
                return string.Format("{0}/{1}", path, file);
            }
        }
        
        public string Thumbnail_Small
        {
            get
            {
                return (false) ? Thumbnail_CalculateRemote(_ImageManager.smallThumbPath) :
                    Thumbnail_CalculateLocal(_ImageManager.smallThumbPath);
            }
        }
        
        public string Thumbnail_Large
        {
            get
            {
                return (false) ? Thumbnail_CalculateRemote(_ImageManager.largeThumbPath) :
                    Thumbnail_CalculateLocal(_ImageManager.largeThumbPath);
            }
        }
        
        public string Thumbnail_Max
        {
            get
            {
                return (false) ? Thumbnail_CalculateRemote(_ImageManager.maxThumbPath) :
                    Thumbnail_CalculateLocal(_ImageManager.maxThumbPath);
            }
        }
        
        public void CreateAllThumbs()
        {
            Thumbnail_CalculateLocal(smallThumbPath);
            Thumbnail_CalculateLocal(largeThumbPath);
            Thumbnail_CalculateLocal(maxThumbPath);
        }

        private string Thumbnail_CalculateLocal(string thumbPath)
        {
            if (_OriginalUrl == null || _OriginalUrl.Trim().Length == 0 || (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(_OriginalUrl))))
                return string.Empty;

            string destPathAndFile = string.Format("{0}{1}{2}", _originalDirectory_Virtual, thumbPath, _originalFileName);
            string mappedDestFilePath = System.Web.HttpContext.Current.Server.MapPath(destPathAndFile);


            //get the context from the path - presuppose act
            int sm = _Config._ActThumbSizeSm;
            int lg = _Config._ActThumbSizeLg;
            int mx = _Config._ActThumbSizeMax;

            //test for act
            //test for show
            if (_OriginalUrl.ToLower().IndexOf(_Config._ShowImageStorage_Local.ToLower()) != -1)
            {
                sm = _Config._ShowThumbSizeSm;
                lg = _Config._ShowThumbSizeLg;
                mx = _Config._ShowThumbSizeMax;
            }
            //test for venue
            else if (_OriginalUrl.ToLower().IndexOf(_Config._VenueImageStorage_Local.ToLower()) != -1)
            {
                sm = _Config._VenueThumbSizeSm;
                lg = _Config._VenueThumbSizeLg;
                mx = _Config._VenueThumbSizeMax;
            }
            else if (_OriginalUrl.ToLower().IndexOf(_Config._PromoterImageStorage_Local.ToLower()) != -1)
            {
                sm = _Config._PromoterThumbSizeSm;
                lg = _Config._PromoterThumbSizeLg;
                mx = _Config._PromoterThumbSizeMax;
            }
            else if (_OriginalUrl.ToLower().IndexOf(_Config._AdvertImageStorage_Local.ToLower()) != -1)
            {
                mx = Wcss.Kiosk.MaxImageWidth;
                lg = (int)Math.Floor((decimal)Wcss.Kiosk.MaxImageWidth/2);
                sm = (int)Math.Floor((decimal)Wcss.Kiosk.MaxImageWidth / 4);
            }

            //ensure file + path exists
            //create thumbnail and save it 
            int size = sm;

            if (thumbPath == _ImageManager.largeThumbPath)
                size = lg;
            else if (thumbPath == _ImageManager.maxThumbPath)
                size = mx;

            if (!File.Exists(mappedDestFilePath))
                Utils.ImageTools.SetThumbnailImage(_OriginalUrl, mappedDestFilePath, size);

            return string.Format("{0}{1}{2}", _originalDirectory_Virtual, thumbPath, _originalFileName);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="thumbPath"></param>
        /// <returns></returns>
        private string Thumbnail_CalculateRemote(string thumbPath)
        {
            return string.Empty;

            //FUTURE
            //ensure file + path exists
            //return string.Format("{0}{1}{2}{3}", _Config._MerchImageStorage_Remote, Path, ThumbPath, ImageName);


            //int g = _Config._ActThumbSizeLg;
            //int h = _Config._VenueThumbSizeLg;
        }

    }
}

