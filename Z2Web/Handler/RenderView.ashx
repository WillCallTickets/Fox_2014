<%@ WebHandler Language="C#" Class="z2Main.Handler.RenderView" %>

using System;
using System.Web;
using System.Web.UI;
using System.IO;
//using System.Drawing;

using Wcss;

namespace z2Main.Handler
{

    public class RenderView : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
                
            try
            {
                //throw new Exception("bullshit hath occurred");
                string viewToRender = context.Request.Params["viewToRender"].ToString().ToLower();

                string control = string.Empty;

                if (viewToRender == "unsubscribe")
                    viewToRender = "mailermanage";
                
                switch (viewToRender)
                {
                    case "about":
                    case "contact":
                    case "newsletter":
                    case "privacy":
                    case "mailerconfirm":
                    case "mailerexample":
                    case "mailermanage":
                    case "signuppopup":
                        control = "/View/" + viewToRender + "View.ascx";
                        break;

                    case "signupcampaign":
                        control = "/View/newsletterView.ascx";
                        break;

                    default:
                        control = "/View/NewsletterView.ascx";
                        break;
                }
                
                
                z2Main.Controller.PageBase pageHolder = new Controller.PageBase();
                UserControl viewControl =
                   (UserControl)pageHolder.LoadControl(control);
                pageHolder.Controls.Add(viewControl);
                
                StringWriter output = new StringWriter();
                HttpContext.Current.Server.Execute(pageHolder, output, true);
                
                
                //return a json response
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                      renderedView = output.ToString()
                }));
                context.Response.StatusCode = 200;

            }
            catch (Exception ex)
            {                    
                context.Response.Write("Error: " + ex.Message);
                context.Response.StatusCode = 250;
                context.Response.Status = "250 RenderView failed";// ex.Message;
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
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}