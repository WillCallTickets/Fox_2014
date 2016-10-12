<%@ WebHandler Language="C#" Class="Admin._customControls.ImageBase_Upload" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;

using Wcss;

namespace Admin._customControls
{

    public class ImageBase_Upload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated && _Common.IsAuthdAdminUser())
            {
                context.Response.ContentType = "text/plain";//allow for error handling without json //application/json
                context.Response.Expires = -1;
                string mappedUpload = string.Empty;
                
                try
                {
                    //get info from request
                    string entityType = context.Request.Params["entityType"].ToString();
                    string idx = context.Request.Params["itemId"].ToString();
                    
                    //throw new Exception("bullshit hath occurred");
                    wctMain.Admin.AdminContext atx = new wctMain.Admin.AdminContext();    
                    SalePromotion ent = atx.CurrentBannerRecord;                    
                    
                    /*
                     * 
                     * save to wct/images/banners
                     * use filename for image name
                     * 
                     */
                    

                    if (ent != null)
                    {
                        //validate file
                        HttpPostedFile postedFile = context.Request.Files["Filedata"];

                        string filename = string.Format("{0}_{1}", Path.GetFileNameWithoutExtension(postedFile.FileName), ent.Id.ToString());
                        string fileExt = Path.GetExtension(postedFile.FileName).ToLower();
                        if (fileExt.Trim().Length == 0 || (fileExt != ".jpg" && fileExt != ".jpeg" && fileExt != ".gif" && fileExt != ".png"))
                            throw new Exception("Valid file types are jpg, jpeg, gif and png only.");
                        
                        
                        //if we have an existing banner - delete it
                        if (ent.BannerUrl != null && ent.BannerUrl.Trim().Length > 0)
                        {
                            string mappedExisting = context.Server.MapPath(string.Format("{0}{1}", SalePromotion.Banner_VirtualDirectory, ent.BannerUrl));
                            if (File.Exists(mappedExisting))
                                File.Delete(mappedExisting);
                        }


                        mappedUpload = context.Server.MapPath(string.Format("{0}{1}{2}", SalePromotion.Banner_VirtualDirectory, filename, fileExt));

                        //if (File.Exists(mappedUpload))
                        //{
                        //    //File.Delete(mappedUpload);
                        //    //just checking
                        //}

                        //this will overwrite the file - moodifies the file
                        postedFile.SaveAs(mappedUpload);

                        //see if you can create dimensions
                        //if this does not work - then there is something wrong with the image - maybe cmyk                    
                        Utils.ImageTools.GetDimensions(mappedUpload);
                        
                        //record image to object
                        ent.BannerUrl = string.Format("{0}{1}", filename, fileExt);
                        ent.Save();
                    
                        //return a json response
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { 
                            path = SalePromotion.Banner_VirtualDirectory,
                            url = Path.GetFileName(mappedUpload)
                        }));
                        context.Response.StatusCode = 200;
                    }
                    else
                        throw new ArgumentNullException("Current show not found");
                }
                catch (Exception ex)
                {
                    if (File.Exists(mappedUpload))
                        File.Delete(mappedUpload);
                    
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