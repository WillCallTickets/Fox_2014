<%@ WebHandler Language="C#" Class="z2Main.Handler.MailupApi" %>

using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Net;
//using System.Web.UI;
//using System.IO;
//using System.Drawing;

using Wcss;
/*
 * TAKE NOTE!!!!!
 * The ip address of the caller must be registered in the mailup backend per 
 * http://help.mailup.com/display/mailupapi/HTTP+API+Examples
 * 
 * 
 * 
http://help.mailup.com/display/mailupapi/HTTP+API+Specifications#HTTPAPISpecifications-Xmlunsubscribe.aspx(Unsubscription)
http://help.mailup.com/display/mailupapi/Codes+Table+and+List+GUID
 * 
 * 
 * https://http://d9c4a.s76.it/
Xmlunsubscribe.aspx (Unsubscription)
This function forces the unsubscription of a subscriber from a list.
URL
The URL to use depends on your MailUp account's unique URL. If your MailUp admin console address is xyzw.espsrv.com, 
 *  the correct action URL for the form (the URL of the form handler) will be http://xyzw.espsrv.com/frontend/Xmlunsubscribe.aspx
REQUEST PARAMETERS
Name Mandatory? Description
ListGuid Y Alphanumeric code associated to a distribution list
List Y List ID
Email N 

RESPONSE VALUES
CODE DESCRIPTION 
0 Recipient unsubscribed successfully
1 Generic error
3 Recipient unknown / already unsubscribed

 * 
 * 
 * 
 * When a user unsubscribes - we should remove them from all lists - easier to comprehend on the user side and most likely what they intend
 * 
*/

namespace z2Main.Handler
{

    public class MailupApi : IHttpHandler
    {
        public class MailApi_CallAndResponse
        {
            public string urlCalled { get; set; }
            public string ListName { get; set; }
            public string ListGuid { get; set; }
            public string ListId { get; set; }
            public string Email { get; set; }
            public string ResponseCode { get; set; }
            public string ResponseTranscribed { get; set; }

            public MailApi_CallAndResponse(string _url, string listName, string listGuid, string listId, string email)
            {
                urlCalled = _url;
                ListName = listName;
                ListGuid = listGuid;
                ListId = listId;
                Email = email;
            }

            public string EventDescription()
            {
                return string.Format("{0} / {1} / {2}", urlCalled, ListName, ResponseTranscribed);
            }
        }
        
        public void ProcessRequest(HttpContext context)
        {
            List<string> msgs = new List<string>();
            
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
                
            try
            {
                string baseUrl = "http://d9c4a.s76.it/frontend/";
                    
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
                //if(requestType == "subscribe" && (!privacy))
                //    msgs.Add("You must acknowledge that you have read the privacy policy.");

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
                
                //END OF INPUT VALIDATION


                string status = "Success";                
                string functionPage = "Xmlunsubscribe.aspx";
                
                switch(requestType.ToLower())
                {
                    case"unsubscribe":
                        functionPage = "Xmlunsubscribe.aspx";
                        break;
                }
                
                //we will be calling this twice
                WebClient mailup = new WebClient();                
                
                //once for newsletter and once for targeted
                //http://help.mailup.com/display/mailupapi/Codes+Table+and+List+GUID
                //Z2 Newsletter for Boulder & Fox Theatres, Frontgate Ticketing Purchasers 
                string urlToCall = string.Format("{0}{1}", baseUrl, functionPage);
                List<MailApi_CallAndResponse> list = new List<MailApi_CallAndResponse>();
                list.Add(new MailApi_CallAndResponse(urlToCall, "Z2 Newsletter for The Boulder Theater and Fox Theatre", "6a84e6f1-4b57-4e98-be93-0316fc068529", "2", email));
                list.Add(new MailApi_CallAndResponse(urlToCall, "The Frontgate Ticketing Purchaser Newsletter", "67213ea4-04b3-4ca0-ab1e-af899468e28c", "3", email));
                
                
                foreach(MailApi_CallAndResponse mcr in list)
                {
                    string url = string.Format("{0}?listguid={1}&list={2}&email={3}",
                        mcr.urlCalled,
                        mcr.ListGuid,
                        mcr.ListId,
                        mcr.Email
                        );
                    
                    mcr.ResponseCode = mailup.DownloadString(url).Trim();//response has carriage returns!                 
                }


                //handle the two calls
                if (requestType == "unsubscribe")
                {
                    string pastTense = "unsubscribed";
                    
                    //evaluate returns
                    foreach (MailApi_CallAndResponse mcr in list)
                    {
                        if (mcr.ResponseCode == "0")
                            mcr.ResponseTranscribed = string.Format("You have been {0} from {1}.", pastTense, mcr.ListName);
                        else if (mcr.ResponseCode == "1")
                            mcr.ResponseTranscribed = string.Format("An error ocurred while attempting to {0} you from {1}.", requestType, mcr.ListName);
                        else if (mcr.ResponseCode == "3")
                            mcr.ResponseTranscribed = string.Format("You are {0} from {1}.", pastTense, mcr.ListName);

                        //log event
                        EventQ.LogEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, "MailUpApiHandler",
                            Guid.Empty, email, _Enums.EventQContext.Mailer, _Enums.EventQVerb.SubscriptionUpdate, string.Empty, requestType, mcr.EventDescription());

                        //TODO do not add a msg about past purchases if they are unsubbed from newsletter
                        msgs.Add(mcr.ResponseTranscribed);
                    }
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
                if (ex.Message.ToLower().IndexOf("unexpected error occurred on a receive") != -1)
                {
                    msgs.Add("Caller's IP is not registered with API provider or invalid URL.");
                    _Error.SendAdministrativeEmail(string.Format("{0} - {1}", "Register with Mailup for Api or invalid URL", _Config._DomainName));
                }
                else
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}