using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web;


namespace Utils
{
	/// <summary>
	/// static helper methods
	/// </summary>
	public class ImageTools
	{
		/// <summary>
		/// Dont want anyone to instantiate
		/// </summary>
		private ImageTools(){ }
        
        public static void SaveRemoteImageLocally(string remoteUrl, string mappedLocalFileDestination, string mappedLocalThumbnailDestination, int thumbSize)
        {
            try
            {
                byte[] data = null;

                using (var webClient = new WebClient())
                {
                    //DOES NOT WORK!!! 
                    //webClient.DownloadFile(remoteUrl, mappedLocalFileDestination);

                    webClient.DownloadDataCompleted += delegate(object sebder, DownloadDataCompletedEventArgs e) 
                    {
                        try
                        {
                            data = e.Result;

                            using (MemoryStream ms = new MemoryStream(data))
                            {
                                Image image = Image.FromStream(ms);
                                image.Save(mappedLocalFileDestination);

                                //save a thumbnail if specified
                                if (mappedLocalThumbnailDestination != null && mappedLocalThumbnailDestination.Trim().Length > 0 && thumbSize > 0)
                                {
                                    CreateAndSaveThumbnailImage(image, mappedLocalThumbnailDestination, thumbSize);
                                }
                            }
                        }
                        catch (Exception) { 
                            
                        }
                    };

                    webClient.DownloadDataAsync(new System.Uri(remoteUrl));

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public static void MoveImageFile(string mappedFileSource, string mappedFileDestination, bool deleteExistingFile)
        {
            //ensure original file
            if(!File.Exists(mappedFileSource))
                throw new FileNotFoundException();

            //ensure dest directory
            string destDirectory = Path.GetDirectoryName(mappedFileDestination);
            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);

            //if we are deleting the existing...
            //if we are not and the file exists - we must fail
            if (deleteExistingFile && File.Exists(mappedFileDestination))
                File.Delete(mappedFileDestination);
            if ((!deleteExistingFile) && File.Exists(mappedFileDestination))
                throw new IOException(string.Format("The filename, {0} exists in the destination.", mappedFileDestination));

            //move the file
            File.Move(mappedFileSource, mappedFileDestination);

        }

        public static void CopyImageFile(string mappedFileSource, string mappedFileDestination, bool deleteExistingFile)
        {
            //ensure original file
            if (!File.Exists(mappedFileSource))
                throw new FileNotFoundException();

            //ensure dest directory
            string destDirectory = Path.GetDirectoryName(mappedFileDestination);
            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);

            //if we are deleting the existing...
            //if we are not and the file exists - we must fail
            if (deleteExistingFile && File.Exists(mappedFileDestination))
                File.Delete(mappedFileDestination);
            if ((!deleteExistingFile) && File.Exists(mappedFileDestination))
                throw new IOException(string.Format("The filename, {0} exists in the destination.", mappedFileDestination));

            //move the file
            File.Copy(mappedFileSource, mappedFileDestination);

        }

        #region Crop funcs

        //http://www.mikesdotnetting.com/Article/95/Upload-and-Crop-Images-with-jQuery-JCrop-and-ASP.NET

        public static byte[] Crop(string Img, int Width, int Height, int X, int Y)
        {
            Image OriginalImage = null;

            try
            {
                using (OriginalImage = System.Drawing.Image.FromFile(Img))
                {
                    return Crop(OriginalImage, Width, Height, X, Y);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (OriginalImage != null)
                    OriginalImage.Dispose();
            }
        }

        public static byte[] Crop(Image OriginalImage, int Width, int Height, int X, int Y)
        {
            Bitmap bmp = null;
            MemoryStream ms = new MemoryStream();

            try
            {
                using (bmp = new System.Drawing.Bitmap(Width, Height))
                {
                    bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);

                    using (System.Drawing.Graphics Graphic = System.Drawing.Graphics.FromImage(bmp))
                    {
                        Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                        Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        Graphic.DrawImage(OriginalImage, new System.Drawing.Rectangle(0, 0, Width, Height), X, Y, Width,
                            Height, System.Drawing.GraphicsUnit.Pixel);

                        bmp.Save(ms, OriginalImage.RawFormat);
                        return ms.GetBuffer();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (bmp != null)
                    bmp.Dispose();
            }
        }


        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/ytz20d80(v=vs.110).aspx
        /// </summary>
        /// <param name="Img"></param>
        /// <returns></returns>
        public static byte[] Rotate90cw(string Img)
        {
            try
            {
                using (System.Drawing.Image OriginalImage = System.Drawing.Image.FromFile(Img))
                {
                    OriginalImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    MemoryStream ms = new MemoryStream();
                    ImageCodecInfo myImageCodecInfo;
                    Encoder myEncoder;
                    EncoderParameter myEncoderParameter;
                    EncoderParameters myEncoderParameters;

                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    myEncoder = Encoder.Quality;
                    myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    OriginalImage.Save(ms, myImageCodecInfo, myEncoderParameters);

                    return ms.GetBuffer();
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }

        /// <summary>
        /// see above
        /// http://msdn.microsoft.com/en-us/library/ytz20d80(v=vs.110).aspx
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        #endregion

        public static Color GetInverseColor(Color original)
        {
            int r = 255 - original.R;
            int b = 255 - original.B;
            int g = 255 - original.G;

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        public static string GetDisplayOnColor(Color original)
        {
            //if (original.R > 128 && original.G > 128 && original.B > 128)
            if ((original.R + original.G + original.B) > 256)
                return string.Format("#000000");

            return string.Format("#FFFFFF");
        }

		public static string SaveAsJpg(string pathAndFileName)
		{
			string ext = Path.GetExtension(pathAndFileName);
			
			if(ext.ToLower() != ".jpg" && ext.ToLower() != ".jpeg")
			{
				//replace the file extension with jpg
				string[] parts = pathAndFileName.Split('.');
				pathAndFileName = string.Format("{0}.jpg", string.Join(".", parts, 0, parts.Length - 1));
			}

			return pathAndFileName;
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="imagePath"></param>
		/// <param name="remotePath"></param>
		public static string UploadRemoteThumbnailImage( string existingImagePath ) 
		{
            throw new NotImplementedException();

            //return string.Empty;

//			//CREATE REMOTE THUMBNAIL
//			if(Config._RemoteUploadActive)
//			{
//				if(File.Exists(existingImagePath))
//				{
//					try
//					{
//						//get the thumbPath from the existing input
//						string fileName = Path.GetFileName(existingImagePath);
//
//						string thumbPath = string.Format("{0}/{1}", Config._PresentActThumbnailDirectory, fileName);
//
//						//create directory if necessary - handle in create remote?
//						string remotePath = string.Format("{0}/{1}", Config._RemoteImageHost, Config._RemoteActThumbPath);
//
//						//set the directory
//						string fullRemotePath = string.Format("{0}/{1}", remotePath, thumbPath);
//
//						//upload the newly created thumbnail
//						if(FtpUploadImage(existingImagePath, fullRemotePath))
//						{
//							//return the string of the remote Path - not the full path - just enough to add
//							//to the the thumbnail path
//							return remotePath;
//						}
//					}
//					catch(Exception ex)
//					{
//						throw ex;
//					}
//				}
//			}
//
//			return string.Empty;
		}

        public static void SetThumbnailImage(string virtualSourceImage, string mappedDestinationDirectory, int size)
        {
            string mappedSourceImage = System.Web.HttpContext.Current.Server.MapPath(virtualSourceImage);

            string checkDirectory = System.IO.Path.GetDirectoryName(mappedDestinationDirectory);

            if (!Directory.Exists(checkDirectory))
                Directory.CreateDirectory(checkDirectory);

            Utils.ImageTools.CreateAndSaveThumbnailImage(mappedSourceImage, mappedDestinationDirectory, size);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="image"></param>
        /// <param name="ftpToDirectory"></param>
        /// <returns></returns>
		private static bool FtpUploadImage(string image, string ftpToDirectory)
		{
            throw new NotImplementedException();

            //bool retVal = false;

//			inolik.Ftp4net.ft ftp = new inolik.Ftp4net.FtpClient();
//
//			//FTP.FtpClient ftp = new FTP.FtpClient();
//
//			try
//			{
//				
//
//				ftp.Open(Config._RemoteFtpHost, Config._RemoteFtpPort);
//
//				ftp.User(Config._RemoteFtpAccount);
//				ftp.Pass(Config._RemoteFtpPass);
//
//				ftp.Cwd(Config._RemoteImageHost);
//
//				ftp.Cwd("BandImages");
//
//				string[] arr = ftp.GetList(Config._PresentActThumbnailDirectory);
//				
//				ftp.Cwd(Config._PresentActThumbnailDirectory);
//
//				ftp.Cdup();
//				
//				string val = ftp.connect(Config._RemoteFtpHost, Config._RemoteFtpAccount, Config._RemoteFtpPass);
//
//				string remoteFolder = Path.GetDirectoryName(ftpToDirectory);
//
//				ArrayList paths = ftp.getRemoteFolder(remoteFolder);
//
//				if(paths.Count == 0)
//					ftp.createFolder(Config._RemoteImageHost, Config._PresentActThumbnailDirectory);
//
//				ftp.upload(image, remoteFolder);
//
//				string g = "l";
//			}
//			catch(Exception ex)
//			{
//				throw ex;
//			}
//			finally
//			{
//				if(ftp != null) ftp.Quit();
//			}

            //return retVal;			
		}

        /// <summary>
        /// The pair returns width as first and height as second
        /// </summary>
        /// <param name="mappedImagePath"></param>
        /// <returns></returns>
        public static System.Web.UI.Pair GetDimensions(string mappedImagePath)
        {
            System.Web.UI.Pair dimensions = new System.Web.UI.Pair(0, 0);

            Bitmap largeImage = null;

            try
            {
                // get photo image
                using (largeImage = new Bitmap(mappedImagePath))
                {
                    dimensions.First = largeImage.Size.Width;
                    dimensions.Second = largeImage.Size.Height;
                }
            }
            catch (Exception)
            {   
                throw new Exception(string.Format("Invalid path for image dimension: {0}", mappedImagePath));
            }
            finally
            {
                //  very important to dispose of images, otherwise images might be locked
                //  and can't update since aspnet process has handle to files
                if (!((largeImage == null))) { largeImage.Dispose(); }
            }

            return dimensions;
        }

        public static void SaveImageFromByteArray(Byte[] image, int originalWidth, int originalHeight, string mappedDestination, int maxWidth)
        {
            Size newSize;
            System.Drawing.Bitmap thumbnail = null;

            try
            {
                using (MemoryStream ms = new MemoryStream(image, 0, image.Length))
                {
                    ms.Write(image, 0, image.Length);

                    if (maxWidth >= originalWidth)
                        newSize = new Size(originalWidth, originalHeight);
                    else
                    {
                        decimal ratio = decimal.Round(((decimal)maxWidth / (decimal)originalWidth), 4);
                        newSize = new Size(maxWidth, (int)(originalHeight * ratio));
                    }

                    thumbnail = new Bitmap(Image.FromStream(ms, true), newSize);

                    thumbnail.Save(mappedDestination, thumbnail.RawFormat);
                }
            }
            finally
            {
                if (thumbnail != null)
                    thumbnail.Dispose();
            }
        }

		/// <summary>
		/// paths must be mapped. Use a null thumbnailPath to test validity of original image for web. CMYKs will be caught here
		/// </summary>
		/// <param name="imagePath"></param>
		/// <param name="thumbnailPath"></param>
        public static void CreateAndSaveThumbnailImage(Image sourceImage, string thumbnailPath, int size)
		{
            Bitmap thumbnail = null;
            Size newSize;

            try
            {
                // get photo image
                using (sourceImage)
                {
                    // dont make it larger! - leave it be
                    if (size >= sourceImage.Width)
                        newSize = new Size(sourceImage.Width, sourceImage.Height);
                    else
                    {
                        decimal ratio = decimal.Round(((decimal)size / (decimal)sourceImage.Width), 4);
                        newSize = new Size(size, (int)(sourceImage.Height * ratio));
                    }

                    thumbnail = new Bitmap(sourceImage, newSize);

                    //decision to save as thumbnail
                    if (thumbnailPath != null && thumbnailPath.Trim().Length > 0)
                    {
                        if (File.Exists(thumbnailPath))
                            File.Delete(thumbnailPath);

                        thumbnail.Save(thumbnailPath, ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //  very important to dispose of images, otherwise images might be locked
                //  and can't update since aspnet process has handle to files
                if (sourceImage != null)
                {
                    sourceImage.Dispose();
                }
                if (thumbnail != null)
                {
                    thumbnail.Dispose();
                }
            }
        }

        public static void CreateAndSaveThumbnailImage(string sourceImagePath, string thumbnailPath, int size)
		{
			Bitmap image = null;

			try 
			{ 
				// get photo image
                using (image = new Bitmap(sourceImagePath))
                {
                    CreateAndSaveThumbnailImage(image, thumbnailPath, size);
                }
			} 
			catch ( Exception ex ) 
			{ 
				throw ex;
			} 
			finally 
			{ 
				//  very important to dispose of images, otherwise images might be locked
				//  and can't update since aspnet process has handle to files
				if (image != null)
                { 
                    image.Dispose(); 
                }
			}
		} 

		/// <summary>
		/// Creates a new image of the specified size from the source image
		/// </summary>
		public static string CreateImage(string virtualDirectory, string imageDirectory, string fileName, int width)
		{
			string description = null;
			try
			{
				string RootDirectory = HttpContext.Current.Server.MapPath(virtualDirectory);
				string ImageDirectory = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}", virtualDirectory, imageDirectory));
				string FullNewImagePath = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}", virtualDirectory, imageDirectory, fileName));

				if (File.Exists(FullNewImagePath)) return description;

				if ( ! Directory.Exists(ImageDirectory)) Directory.CreateDirectory(ImageDirectory);

				Image image = Image.FromFile(RootDirectory + "/" + fileName);

				int y = (int)(((double)((double)width/(double)image.Size.Width)) * (double)image.Size.Height);
				
				Image thumb;

				if ( width > image.Width )// dont make it larger!
				{
					thumb = image;
				}
				else if ( width > 200 )// only do this if we need quality
				{
					thumb = Resize(new Bitmap(image),width,y,(bool)(width > 200));
				}
				else
				{
					//use dodgy MS call
					Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
					thumb = image.GetThumbnailImage(width,y,myCallback,IntPtr.Zero);
				}

				thumb.Save(FullNewImagePath, ImageFormat.Jpeg);
			}
			catch(Exception){}

			return description;
		}

		/// <summary>
		/// Bah! MS bug
		/// </summary>
		/// <returns></returns>
		public static bool ThumbnailCallback()
		{
			return false;
		}

		/// <summary>
		/// Taken from http://www.codeproject.com/cs/media/imageprocessing4.asp
		/// Christian Graus
		/// </summary>
		/// <param name="b">Image to resize</param>
		/// <param name="nWidth">Width to make it</param>
		/// <param name="nHeight">Height to make it</param>
		/// <param name="bBilinear">Whether to use the bilenear method (alot more cpu)</param>
		/// <returns></returns>
		public static Bitmap Resize(Bitmap b, int nWidth, int nHeight, bool bBilinear)
		{
			Bitmap bTemp = b;

			b = new Bitmap(nWidth, nHeight, bTemp.PixelFormat);

			double nXFactor = (double)bTemp.Width/(double)nWidth;
			double nYFactor = (double)bTemp.Height/(double)nHeight;

			if (bBilinear)
			{
				double fraction_x, fraction_y, one_minus_x, one_minus_y;
				int ceil_x, ceil_y, floor_x, floor_y;
				Color c1 = new Color();
				Color c2 = new Color();
				Color c3 = new Color();
				Color c4 = new Color();
				byte red, green, blue;

				byte b1, b2;

				for (int x = 0; x < b.Width; ++x)
					for (int y = 0; y < b.Height; ++y)
					{
						// Setup

						floor_x = (int)Math.Floor(x * nXFactor);
						floor_y = (int)Math.Floor(y * nYFactor);
						ceil_x = floor_x + 1;
						if (ceil_x >= bTemp.Width) ceil_x = floor_x;
						ceil_y = floor_y + 1;
						if (ceil_y >= bTemp.Height) ceil_y = floor_y;
						fraction_x = x * nXFactor - floor_x;
						fraction_y = y * nYFactor - floor_y;
						one_minus_x = 1.0 - fraction_x;
						one_minus_y = 1.0 - fraction_y;

						c1 = bTemp.GetPixel(floor_x, floor_y);
						c2 = bTemp.GetPixel(ceil_x, floor_y);
						c3 = bTemp.GetPixel(floor_x, ceil_y);
						c4 = bTemp.GetPixel(ceil_x, ceil_y);

						// Blue
						b1 = (byte)(one_minus_x * c1.B + fraction_x * c2.B);

						b2 = (byte)(one_minus_x * c3.B + fraction_x * c4.B);
            
						blue = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

						// Green
						b1 = (byte)(one_minus_x * c1.G + fraction_x * c2.G);

						b2 = (byte)(one_minus_x * c3.G + fraction_x * c4.G);
            
						green = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

						// Red
						b1 = (byte)(one_minus_x * c1.R + fraction_x * c2.R);

						b2 = (byte)(one_minus_x * c3.R + fraction_x * c4.R);
            
						red = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

						b.SetPixel(x,y, System.Drawing.Color.FromArgb(255, red, green, blue));
					}
			}
			else
			{
				for (int x = 0; x < b.Width; ++x)
					for (int y = 0; y < b.Height; ++y)
						b.SetPixel(x, y, bTemp.GetPixel((int)(Math.Floor(x * nXFactor)),
							(int)(Math.Floor(y * nYFactor))));
			}

			return b;
		}
	}
}
