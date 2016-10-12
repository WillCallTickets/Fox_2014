<%@ WebHandler Language="C#" Class="Admin._customControls.ShowInformationRetrieval" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;

using Wcss;

namespace Admin._customControls
{

    public class ShowInformationRetrieval : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                    
                    //example of how to consume result in JS
                    //var res = JSON.parse(result);                    
                    //txt.val(res["url"]);
                    
                    //wctMain.Admin.AdminContext ctx = new wctMain.Admin.AdminContext();

                    string commandOperation = context.Request.Params["commandOperation"].ToString();
                    string showId = context.Request.Params["showId"].ToString();

                    int idx = 0;
                    if (!int.TryParse(showId, out idx))
                        throw new ArgumentOutOfRangeException("show id is not a valid integer");

                    Show s = new Show(idx);

                    if (s == null)
                        throw new ArgumentOutOfRangeException(string.Format("No show found for id: {0}", idx.ToString()));
                    
                    switch(commandOperation)
                    {
                        case "getShowUrl":
                            //return a response
                            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(
                                new
                                {
                                    url = string.Format("/{0}", s.FirstShowDate.ConfiguredUrl)
                                }));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("Error: " + ex.Message);
                    context.Response.StatusCode = 250;
                    context.Response.Status = "250 ShowInformation Retrieval failed";// ex.Message;
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