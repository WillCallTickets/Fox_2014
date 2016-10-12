<%@ WebHandler Language="C#" Class="Admin._customControls.MailerImageRotate" %>

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

    public class MailerImageRotate : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated && _Common.IsAuthdAdminUser())
            {
                context.Response.ContentType = "text/plain";//allow for error handling without json //application/json
                context.Response.Expires = -1;

                try
                {
                    //throw new Exception("bullshit hath occurred");
                    wctMain.Admin.AdminContext ctx = new wctMain.Admin.AdminContext();

                    string entityType = context.Request.Params["entityType"].ToString();
                    string idx = context.Request.Params["itemId"].ToString();
                    
                    MailerContent ent = ctx.CurrentMailerContent;
                    MailerShow s = ent.MailerShowList.Find(delegate(MailerShow match) { return (match.Id == idx); });
                    
                    /*
                     * Determine if image is from show - make an upload copy
                     * Always make a new thumbnail
                     */

                    if(s != null)
                    {
                        if (s.Id == null || s.Id.Trim().Length == 0)
                            s.Id = Path.GetRandomFileName();
                        
                        //WORK FROM THE UPLOAD!!!
                        //set working file to upload version
                        string filename = Path.GetFileName(s.ImageUrl);
                        string mappedUpload = context.Server.MapPath(string.Format("{0}{1}",
                            _Config._UploadImageStorage_Local, filename));


                        //if the image file is from the show file - copy show file to uploads to use as working file
                        if (s.ImageUrl.IndexOf(_Config._ShowImageStorage_Local) != -1)
                        {
                            //get original file to work from
                            string working = context.Server.MapPath(string.Format("{0}{1}",
                                _Config._ShowImageStorage_Local, Path.GetFileName(s.ImageUrl)));

                            //copy to the upload location - with the new filename based on the objects id
                            mappedUpload = context.Server.MapPath(string.Format("{0}{1}{2}",
                                _Config._UploadImageStorage_Local, s.Id, Path.GetExtension(s.ImageUrl)));

                            //update the parent object
                            s.ImageUrl = string.Format("{0}{1}{2}",
                                _Config._EmailerImageStorage_Local, s.Id, Path.GetExtension(s.ImageUrl));

                            if (File.Exists(mappedUpload))
                                File.Delete(mappedUpload);
                            
                            File.Copy(working, mappedUpload);
                        }

                        //if we dont have a thumbnail - create one prior to any rotation
                        string mappedThumbnail = context.Server.MapPath(s.ImageUrl);

                        if (!File.Exists(mappedThumbnail))
                            Utils.ImageTools.CreateAndSaveThumbnailImage(mappedUpload, mappedThumbnail, _Config._ShowThumbSizeSm);

                        //get dims of thumb (non-rotated)
                        System.Web.UI.Pair pt = Utils.ImageTools.GetDimensions(mappedThumbnail);
                        int wT = (int)pt.First;
                        int hT = (int)pt.Second;
                        
                        
                        
                        
                        //get original (non-rotated) dimensions 
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(mappedUpload);
                        int width = (int)p.First;
                        int height = (int)p.Second;
                        
                        //rotate the working image and save to uploads
                        byte[] RotatedImage = Utils.ImageTools.Rotate90cw(mappedUpload);

                        if (File.Exists(mappedUpload))
                            File.Delete(mappedUpload);
                        
                        //SAVE WITH ROTATED DIMENSIONS!
                        Utils.ImageTools.SaveImageFromByteArray(RotatedImage, height, width, mappedUpload, height);
                        
                        
                        
                        //create a rotated the thumbnail
                        byte[] RotatedThumb = Utils.ImageTools.Rotate90cw(mappedThumbnail);
                        
                        //delete any existing thumbnails
                        if (File.Exists(mappedThumbnail))
                            File.Delete(mappedThumbnail);

                        //SAVE WITH ROTATED DIMENSIONS!
                        Utils.ImageTools.SaveImageFromByteArray(RotatedThumb, hT, wT, mappedThumbnail, hT);

                        
                        //save - image may have come from a show
                        ent.VcJsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(ent.MailerShowList);
                        ent.Save();

                        
                        //return a response
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            url = s.ImageUrl
                        }));
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("Error: " + ex.Message);
                    context.Response.StatusCode = 250;
                    context.Response.Status = "250 Crop failed";// ex.Message;
                    context.Response.StatusDescription = ex.Message;
                    context.Response.Flush();
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