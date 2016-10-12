<%@ WebHandler Language="C#" Class="Admin._customControls.ImageManager_Upload" %>

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
    public class ImageManager_Upload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated && _Common.IsAuthdAdminUser())
            {
                context.Response.ContentType = "text/plain";//allow for error handling without json //application/json
                context.Response.Expires = -1;

                wctMain.Admin.AdminContext ctx = new wctMain.Admin.AdminContext();                

                string entityType = context.Request.Params["entityType"] ?? string.Empty;

                _ImageManager.IImageManagerParent ent =
                    (entityType.Equals(Wcss.Kiosk.Schema.TableName, StringComparison.OrdinalIgnoreCase)) ? (_ImageManager.IImageManagerParent)ctx.CurrentKioskRecord :
                    (entityType.Equals(Wcss.Show.Schema.TableName, StringComparison.OrdinalIgnoreCase)) ? (_ImageManager.IImageManagerParent)ctx.CurrentShowRecord :
                    null;
                
                try
                {
                    HttpPostedFile postedFile = context.Request.Files["Filedata"];

                    string uploadExt = Path.GetExtension(postedFile.FileName).ToLower();

                    if (uploadExt.Trim().Length == 0 || (uploadExt != ".jpg" && uploadExt != ".jpeg" && uploadExt != ".gif" && uploadExt != ".png"))
                        throw new Exception("Valid file types are jpg, jpeg, gif and png only.");
                    
                    //create the file to save
                    string mappedDestination = context.Server.MapPath(
                        string.Format("{0}{1}{2}{3}", 
                        (entityType.Equals(Wcss.Kiosk.Schema.TableName, StringComparison.OrdinalIgnoreCase)) ? 
                        _Config._AdvertImageStorage_Local : _Config._ShowImageStorage_Local,
                        (entityType.Equals(Wcss.Kiosk.Schema.TableName, StringComparison.OrdinalIgnoreCase)) ? 
                        "kskimg" : "shimg",
                        ((SubSonic.IActiveRecord)ent).GetColumnValue("Id").ToString(),
                        uploadExt
                        ));



                    //get rid of old content
                    ent.ResetImageManager();


                    //////////////////////
                    //save the new file
                    postedFile.SaveAs(mappedDestination);
                    /////////////////////
                    
                    

                    //save the new image name to the object
                    ent.SetDisplayUrl(Path.GetFileName(mappedDestination));
                    
                    
                    //force creation of image manager and set vars
                    _ImageManager imgMgr = ent.GetImageManager();
                    imgMgr.CreateAllThumbs();
                    
                    
                    //save the entity changes
                    ((SubSonic.IActiveRecord)ent).Save();

                    //return a response
                    context.Response.Write(Path.GetFileName(mappedDestination));
                    context.Response.StatusCode = 200;              
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