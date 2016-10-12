<%@ WebHandler Language="C#" Class="Admin._customControls.MailerImageUpload" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;

using Wcss;

/*
 * http://www.codeproject.com/Articles/697038/Implementing-multiple-file-upload-with-progress-ba
 */

namespace Admin._customControls
{

    public class MailerImageUpload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                    MailerShow s = ent.MailerShowList.Find(delegate(MailerShow match) { return (match.Id == idx); } );
                    
                    
                    /*
                     * 
                     * LEAVE THE SHOW FILES ALONE!
                     * 
                     * if we are uploading, we are using a different store for mail images
                     * uploads will go to an upload folder (TBA viewable at some other time)
                     * the image will also create a thumbnail to be stored in the .../emailer/ folder
                     * 
                     * if the file exists in the upload, do not overwrite, rename the old file
                     * 
                     */
                    

                    if (s != null)
                    {
                        if (s.Id == null || s.Id.Trim().Length == 0)
                            s.Id = Path.GetRandomFileName();
                        
                        HttpPostedFile postedFile = context.Request.Files["Filedata"];

                        string filename = Path.GetFileNameWithoutExtension(postedFile.FileName);
                        string fileExt = Path.GetExtension(postedFile.FileName).ToLower();
                        if (fileExt.Trim().Length == 0 || (fileExt != ".jpg" && fileExt != ".jpeg" && fileExt != ".gif" && fileExt != ".png"))
                            throw new Exception("Valid file types are jpg, jpeg, gif and png only.");

                        string mappedUpload = context.Server.MapPath(string.Format("{0}{1}{2}", Wcss._Config._UploadImageStorage_Local, s.Id, fileExt));
                        string virtualThumbnail = string.Format("{0}{1}{2}", Wcss._Config._EmailerImageStorage_Local, s.Id, fileExt);
                        string mappedThumbnail = context.Server.MapPath(virtualThumbnail);

                        int thumbWidth = Wcss._Config._ShowThumbSizeSm;

                        //ensure directories
                        if (!Directory.Exists(Path.GetDirectoryName(mappedUpload)))
                            Directory.CreateDirectory(Path.GetDirectoryName(mappedUpload));
                        if (!Directory.Exists(Path.GetDirectoryName(mappedThumbnail)))
                            Directory.CreateDirectory(Path.GetDirectoryName(mappedThumbnail));
                        
                        //if the file exists in the upload, do not overwrite, rename the old file
                        //Rename thumbnail as well
                        if (File.Exists(mappedUpload))
                        {
                            string moveUpload = context.Server.MapPath(string.Format("{0}{1}{2}", Wcss._Config._UploadImageStorage_Local, Path.GetRandomFileName(), fileExt));
                            File.Move(mappedUpload, moveUpload);
                        }
                        if (File.Exists(mappedThumbnail))
                        {
                            //always delete thumbnails
                            File.Delete(mappedThumbnail);
                        }   
                        
                        
                        //save the new files
                        string tmpUpload = string.Format("{0}tmp_{1}{2}", 
                            Path.GetDirectoryName(mappedUpload), Path.GetFileNameWithoutExtension(mappedUpload), Path.GetExtension(mappedUpload));
                        
                        postedFile.SaveAs(tmpUpload);

                        Utils.ImageTools.CreateAndSaveThumbnailImage(tmpUpload, mappedUpload, _Config._ShowThumbSizeMax);
                        
                        if (File.Exists(tmpUpload))
                            File.Delete(tmpUpload);
                        
                        
                        //create the thumbnail and save
                        Utils.ImageTools.CreateAndSaveThumbnailImage(mappedUpload, mappedThumbnail, thumbWidth);
                        
                        
                        //save the image info back to the object
                        s.ImageUrl = virtualThumbnail;

                        ent.VcJsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(ent.MailerShowList);
                        ent.Save();
                    
                        //return a json response
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { 
                            width = thumbWidth,
                            url = virtualThumbnail
                        }));
                        context.Response.StatusCode = 200;
                    }
                    else
                        throw new ArgumentNullException("Current show not found");
                }
                catch (Exception ex)
                {   
                    context.Response.Write("Error: " + ex.Message);
                    context.Response.StatusCode = 250;
                    context.Response.Status = "250 Upload failed";// ex.Message;
                    context.Response.StatusDescription = ex.Message;
                    context.Response.Flush();

                    /*http://stackoverflow.com/questions/6901841/uploadify-v3-onuploaderror-howto
                    upload errors is the following: 
                    HTTP_ERROR	 : -200, 
                    MISSING_UPLOAD_URL	 : -210, 
                    IO_ERROR	 : -220, 
                    SECURITY_ERROR	 : -230, 
                    UPLOAD_LIMIT_EXCEEDED	 : -240, 
                    UPLOAD_FAILED	 : -250, 
                    SPECIFIED_FILE_ID_NOT_FOUND	: -260, 
                    FILE_VALIDATION_FAILED	 : -270, 
                    FILE_CANCELLED	 : -280, 
                    UPLOAD_STOPPED	 : -290 
                     */
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