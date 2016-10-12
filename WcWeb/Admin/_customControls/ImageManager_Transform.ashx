<%@ WebHandler Language="C#" Class="Admin._customControls.ImageManager_Transform" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;

using Wcss;

/*
 * http://www.script-home.com/jquery-jcrop-plug-in-implementation-image-selection-function.html
 */

namespace Admin._customControls
{

    public class ImageManager_Transform : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated && _Common.IsAuthdAdminUser())
            {
                context.Response.ContentType = "text/plain";//allow for error handling without json //application/json
                context.Response.Expires = -1;
                
                wctMain.Admin.AdminContext ctx = new wctMain.Admin.AdminContext();
                Bitmap originalImageLoaded = null;

                string entityType = context.Request.Params["entityType"] ?? string.Empty;
                string transform = context.Request.Params["transformType"] ?? string.Empty;

                _ImageManager.IImageManagerParent ent =
                    (entityType.Equals(Wcss.Kiosk.Schema.TableName, StringComparison.OrdinalIgnoreCase)) ? (_ImageManager.IImageManagerParent)ctx.CurrentKioskRecord :
                    (entityType.Equals(Wcss.Show.Schema.TableName, StringComparison.OrdinalIgnoreCase)) ? (_ImageManager.IImageManagerParent)ctx.CurrentShowRecord :
                    null;
                
                try
                {
                    //throw new Exception("Bullshit hath ocurred");
                    
                    //establish ImageManager or die
                    _ImageManager imgMgr = ent.GetImageManager();

                    if (imgMgr == null || imgMgr.OriginalUrl == null || imgMgr.OriginalUrl.Trim().Length == 0)
                        throw new Exception("There is no ImageManager for the object");

                    //ensure an image file
                    string mappedOriginal = context.Server.MapPath(imgMgr.OriginalUrl);
                    if (!File.Exists(mappedOriginal))
                        throw new Exception("The image file does not exist");

                    
                    byte[] transformedImage = null;
                    int originalWidth, x, y, width, height, displayWidth;
                    
                    if (transform == "crop")
                    {
                        originalWidth = 0;
                        x = Convert.ToInt32(context.Request.Params["x1"]);
                        y = Convert.ToInt32(context.Request.Params["y1"]);
                        width = Convert.ToInt32(context.Request.Params["w"]);
                        height = Convert.ToInt32(context.Request.Params["h"]);
                        displayWidth = Convert.ToInt32(context.Request.Params["displayWidth"]);

                        //load the image and update info
                        using (originalImageLoaded = new Bitmap(mappedOriginal))
                        {
                            originalWidth = originalImageLoaded.Width;

                            //set ratio
                            double conversionRatio = (double)originalWidth / displayWidth;

                            //transform dimensions
                            x = (int)(x * conversionRatio);
                            y = (int)(y * conversionRatio);
                            int newwidth = (int)(width * conversionRatio);
                            int newheight = (int)(height * conversionRatio);

                            //create an in-memory image
                            transformedImage = Utils.ImageTools.Crop(originalImageLoaded, newwidth, newheight, x, y);
                        }
                    }
                    else if (transform == "rotate")
                    {
                        transformedImage = Utils.ImageTools.Rotate90cw(mappedOriginal);
                    }


                    //we need an image to continue
                    if (transformedImage.Length == 0)
                        throw new Exception("Failed to create a cropped image.");


                    //save a copy of the original to a cropped directory and use a random file name - keep orginal extension
                    Utils.ImageTools.MoveImageFile(
                        mappedOriginal,
                        string.Format("{0}\\cropped\\{1}{2}", Path.GetDirectoryName(mappedOriginal), Path.GetRandomFileName(), Path.GetExtension(mappedOriginal)),
                        false);


                    //force object re-creation - reset parent entity image info
                    ent.ResetImageManager();


                    //save the transformed image to the entites upload folder
                    using (MemoryStream ms = new MemoryStream(transformedImage, 0, transformedImage.Length))
                    {
                        ms.Write(transformedImage, 0, transformedImage.Length);

                        using (System.Drawing.Image imgTransform = System.Drawing.Image.FromStream(ms, true))
                        {
                            imgTransform.Save(mappedOriginal, imgTransform.RawFormat);

                            ent.SetDisplayUrl(Path.GetFileName(mappedOriginal));
                            ent.SetPicWidth(imgTransform.Width);
                            ent.SetPicHeight(imgTransform.Height);
                        }
                    }

                    //save the entity changes
                    ((SubSonic.IActiveRecord)ent).Save();
                    
                    //return a response
                    context.Response.Write(Path.GetFileName(mappedOriginal));
                    context.Response.StatusCode = 200;
                    
                }
                catch (Exception ex)
                {
                    context.Response.Write("Error: " + ex.Message);
                    context.Response.StatusCode = 250;
                    context.Response.Status = "250 Rotate failed";// ex.Message;
                    context.Response.StatusDescription = ex.Message;
                    context.Response.Flush();
                }
                finally
                {
                    if (originalImageLoaded != null)
                        originalImageLoaded.Dispose();
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}