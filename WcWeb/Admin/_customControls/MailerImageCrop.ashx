<%@ WebHandler Language="C#" Class="Admin._customControls.MailerImageCrop" %>

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

    public class MailerImageCrop : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated && _Common.IsAuthdAdminUser())
            {
                context.Response.ContentType = "text/plain";//allow for error handling without json //application/json
                context.Response.Expires = -1;

                /*
                 * Determine if we are using a "show" or "upload" image
                 *  - if show, create a copy in upload
                 *  - then we work from the uploaded file
                 *  -- apply image ratio, etc to larger version (better quality)
                 *  - either way - also save to a thumbnail
                */

                try
                {
                    //throw new Exception("bullshit hath occurred");
                    wctMain.Admin.AdminContext ctx = new wctMain.Admin.AdminContext();

                    string entityType = context.Request.Params["entityType"].ToString();
                    string idx = context.Request.Params["itemId"].ToString();
                    string displayWidth = context.Request.Params["displayWidth"].ToString();
                    
                    string x1 = context.Request.Params["x1"] ?? string.Empty;
                    string y1 = context.Request.Params["y1"] ?? string.Empty;
                    string x2 = context.Request.Params["x2"] ?? string.Empty;
                    string y2 = context.Request.Params["y2"] ?? string.Empty;
                    string w = context.Request.Params["w"] ?? string.Empty;
                    string h = context.Request.Params["h"] ?? string.Empty;

                    MailerContent ent = ctx.CurrentMailerContent;
                    MailerShow s = ent.MailerShowList.Find(delegate(MailerShow match) { return (match.Id == idx); });
                    
                    if(s != null)
                    {
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

                        
                        //configure destination
                        string mappedThumbnail = context.Server.MapPath(s.ImageUrl);

                        //delet any existing thumbnails
                        if (File.Exists(mappedThumbnail))
                            File.Delete(mappedThumbnail);                        
                        
                        //configure ratios/heights/widths
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(mappedUpload);
                        int origWidth = (int)p.First;
                        int origHeight = (int)p.Second;
                        decimal origRatio = decimal.Round((decimal)origWidth / (decimal)origHeight, 4);

                        decimal originalToDisplayedRatio = decimal.Round((decimal)origWidth /(decimal)int.Parse(displayWidth), 4);


                        //Work from original
                        int width = (int)(int.Parse(w) * originalToDisplayedRatio);
                        int height = (int)(int.Parse(h) * originalToDisplayedRatio);
                        int x = (int)(int.Parse(x1) * originalToDisplayedRatio);
                        int y = (int)(int.Parse(y1) * originalToDisplayedRatio);
                        
                        
                        //crop it
                        byte[] CroppedImage = Utils.ImageTools.Crop(mappedUpload, width, height, x, y);

                        Utils.ImageTools.SaveImageFromByteArray(CroppedImage, width, height, mappedThumbnail, _Config._ShowThumbSizeSm);
                        
                        ent.VcJsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(ent.MailerShowList);
                        ent.Save();
                        
                        //return a response
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(
                            new
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