<%@ WebHandler Language="C#" Class="Admin._customControls.NamedEntityOperations" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;

using Wcss;


namespace Admin._customControls
{

    public class NamedEntityOperations : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated && _Common.IsAuthdAdminUser())
            {
                context.Response.ContentType = "text/plain";//allow for error handling without json //application/json
                context.Response.Expires = -1;
                
                wctMain.Admin.AdminContext ctx = new wctMain.Admin.AdminContext();

                try
                {
                    //throw new Exception("Bullshit hath ocurred");
                    
                    string commandOperation = context.Request.Params["commandOperation"] ?? string.Empty;
                    string CTX = context.Request.Params["ctx"].ToLower() ?? string.Empty;
                    string input = context.Request.Params["input"] ?? string.Empty;

                    bool handled = true;
                    bool exists = false;
                
                    switch (commandOperation.ToLower())
                    {
                        case "verifyexistence":
                            if (CTX == "act")
                            {
                                Act a = new Act("Name", input);
                                if (a != null && a.Id > 0)
                                    exists = true;
                            }
                            else if (CTX == "promoter")
                            {
                                Promoter a = new Promoter("Name", input);
                                if (a != null && a.Id > 0)
                                    exists = true;
                            }
                            else if (CTX == "venue")
                            {
                                Venue a = new Venue("Name", input);
                                if (a != null && a.Id > 0)
                                    exists = true;
                            }

                            //return a json response
                            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
                            {
                                status = exists.ToString().ToLower()
                            }));
                            context.Response.StatusCode = 200;

                            handled = true;
                            break;
                    }


                    if(!handled)
                        throw new InvalidOperationException("Operation not found");
                    
                }
                catch (Exception ex)
                {
                    context.Response.Write("Error: " + ex.Message);
                    context.Response.StatusCode = 250;
                    context.Response.Status = "250 Rotate failed";// ex.Message;
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