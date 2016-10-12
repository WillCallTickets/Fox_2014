<%@ WebHandler Language="C#" Class="z2Main.Handler.EmailSubscriptionRequest" %>

using System;
using System.Web;
using System.Collections.Generic;
//using System.Web.UI;
//using System.IO;
//using System.Drawing;

using Wcss;

namespace z2Main.Handler
{

    public class EmailSubscriptionRequest : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            List<string> msgs = new List<string>();
            
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
                
            try
            {
                //throw new Exception("bullshit hath occurred");
                string email = context.Request.Params["email"].ToString().ToLower();
                string sourcePage = context.Request.Params["source"].ToString();
                bool privacy = bool.Parse(context.Request.Params["privacy"].ToString());
                string requestType = context.Request.Params["requestType"].ToString();

                if (!Utils.Validation.IsValidEmail(email))
                {
                    email = HttpUtility.UrlDecode(email);
                    if (!Utils.Validation.IsValidEmail(email))
                        msgs.Add("The email you have submitted is not a valid email address.");
                }

                //only need privacy for subscribing - automated tasks may have to provide privacy
                if (requestType == "subscribe" && (!privacy))
                    msgs.Add("You must acknowledge that you have read the privacy policy.");

                if (msgs.Count > 0)
                {
                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        status = "Error",
                        messages = msgs
                    }));
                    context.Response.StatusCode = 200;

                    return;
                }

                //END OF VALIDATION



                string msg = string.Empty;
                string status = "Success";
                string sourceQuery = string.Empty;


                //if we are subscribing - record referral
                if (requestType == "subscribe" && context.Request != null && context.Request.UrlReferrer != null &&
                    context.Request.UrlReferrer.Query != null)
                {
                    sourceQuery = context.Request.UrlReferrer.Query.TrimStart(new char[] { '?' });
                }


                using (System.Data.IDataReader dr = SPs.TxZ2SubscriptionUpdate(email, sourcePage, requestType, context.Request.UserHostAddress, sourceQuery).GetReader())
                {
                    //examine result
                    while (dr.Read())
                        msg = dr.GetValue(0).ToString();
                    dr.Close();
                }

                if (msg != "SUCCESS" && msg.IndexOf(" pending.") == -1)
                {
                    status = "Error";
                    msgs.Add(msg);
                }
                else if (sourcePage == "Website Signup" && requestType == "subscribe")
                {
                    //send a confirmation email for the client to respond to for verification
                    MailQueue.SendZ2MailerSignupNotification(email, DateTime.Now,
                        string.Format("the sign up control located on our Newsletter page at Z2ent.com.", ""));
                }


                //return a json response
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    status = status,
                    messages = msgs
                }));
                context.Response.StatusCode = 200;

            }
            catch (Exception ex)
            {
                msgs.Add(ex.Message);

                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    status = "Error",
                    messages = msgs
                }));
                
                context.Response.StatusCode = 250;
                context.Response.Status = "250 RenderView failed";// ex.Message;
                context.Response.StatusDescription = ex.Message;
                context.Response.Flush();

                _Error.LogException(ex);      
                
                
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

        /// <summary>
        /// Renders the specified UserControl. In this case - views
        /// </summary>
        //private static void RenderUserControl(string pathToView)
        //{
        //    z2Main.Controller.PageBase pageHolder = new Controller.PageBase();

        //    UserControl viewControl =
        //       (UserControl)pageHolder.LoadControl(pathToView);

        //    //Type viewControlType = viewControl.GetType();
            
        //    //PropertyInfo property1 = viewControlType.GetProperty(propertyName1);

        //    //if (property1 != null)
        //    //    property1.SetValue(viewControl, propertyValue1, null);

        //    pageHolder.Controls.Add(viewControl);
        //    StringWriter output = new StringWriter();
        //    HttpContext.Current.Server.Execute(pageHolder, output, true);

        //    return new
        //    {
        //        renderedView = output.ToString()
        //    };
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}