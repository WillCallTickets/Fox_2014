<%@ WebHandler Language="C#" Class="Admin._customControls.PrincipalOrder" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;

using Wcss;

namespace Admin._customControls
{

    public class PrincipalOrder : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                    wctMain.Admin.AdminContext atx = new wctMain.Admin.AdminContext();

                    //get info from request                    
                    string newOrder = context.Request.Params["newOrder"].ToString();
                    string entityType = context.Request.Params["entityType"].ToString().ToLower();
                    string[] fresh = newOrder.Split(',');
                    
                    //bypass show editor collections
                    if (entityType != "showlink" && entityType != "jshowact" && entityType != "jshowpromoter")
                    {
                        //dont allow editing in all mode -0 should be handled in calling client
                        if (atx.CurrentEditPrincipal == _Enums.Principal.all)
                        {
                            //return a json response
                            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
                            {
                                status = "cannot order an ALL collection"
                            }));
                            context.Response.StatusCode = 200;
                        }
                    }
                    
                    

                    if (entityType == "showlink" || entityType == "jshowact" || entityType == "jshowpromoter")
                    {
                        for (int i = 0; i < fresh.Length; i++)
                        {   
                            Utils._Collection.IDisplayOrderable ent =
                                (entityType == Wcss.ShowLink.Schema.TableName.ToLower()) ? (Utils._Collection.IDisplayOrderable)new ShowLink(int.Parse(fresh[i])) :
                                (entityType == Wcss.JShowAct.Schema.TableName.ToLower()) ? (Utils._Collection.IDisplayOrderable)new JShowAct(int.Parse(fresh[i])) : 
                                (entityType == Wcss.JShowPromoter.Schema.TableName.ToLower()) ? (Utils._Collection.IDisplayOrderable)new JShowPromoter(int.Parse(fresh[i])) : 
                                null;
                            
                            if (ent != null && ent.Id.ToString() == fresh[i])
                            {
                                ent.DisplayOrder = i;
                                ent.GetType().InvokeMember("Save", System.Reflection.BindingFlags.InvokeMethod, null, ent, null);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < fresh.Length; i++)
                        {
                            _PrincipalBase.IPrincipal ent =
                                (entityType == Wcss.Post.Schema.TableName.ToLower()) ? (_PrincipalBase.IPrincipal)new Post(int.Parse(fresh[i])) :
                                (entityType == Wcss.FaqItem.Schema.TableName.ToLower()) ? (_PrincipalBase.IPrincipal)new FaqItem(int.Parse(fresh[i])) :
                                (entityType == Wcss.Kiosk.Schema.TableName.ToLower()) ? (_PrincipalBase.IPrincipal)new Kiosk(int.Parse(fresh[i])) :
                                (entityType == Wcss.SalePromotion.Schema.TableName.ToLower()) ? (_PrincipalBase.IPrincipal)new SalePromotion(int.Parse(fresh[i])) :
                                (entityType == Wcss.Employee.Schema.TableName.ToLower()) ? (_PrincipalBase.IPrincipal)new Employee(int.Parse(fresh[i])) :
                                null;

                            if (ent != null && ent.Id.ToString() == fresh[i])
                            {
                                _PrincipalBase.Principaled entP =
                                    (entityType == Wcss.Post.Schema.TableName.ToLower()) ? (_PrincipalBase.Principaled)new Post_Principal((Post)ent) :
                                    (entityType == Wcss.FaqItem.Schema.TableName.ToLower()) ? (_PrincipalBase.Principaled)new FaqItem_Principal((FaqItem)ent) :
                                    (entityType == Wcss.Kiosk.Schema.TableName.ToLower()) ? (_PrincipalBase.Principaled)new Kiosk_Principal((Kiosk)ent) :
                                    (entityType == Wcss.SalePromotion.Schema.TableName.ToLower()) ? (_PrincipalBase.Principaled)new SalePromotion_Principal((SalePromotion)ent) :
                                    (entityType == Wcss.Employee.Schema.TableName.ToLower()) ? (_PrincipalBase.Principaled)new Employee_Principal((Employee)ent) :
                                    null;

                                if (entP != null)
                                {
                                    entP.PrincipalOrder_Set(atx.CurrentEditPrincipal, i);
                                    ent.GetType().InvokeMember("Save", System.Reflection.BindingFlags.InvokeMethod, null, ent, null);
                                }
                            }
                        }
                    }

                    //return a json response
                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        status = "success"
                    }));
                    context.Response.StatusCode = 200;

                }
                catch (Exception ex)
                {                    
                    context.Response.Write("Error: " + ex.Message);
                    context.Response.StatusCode = 250;
                    context.Response.Status = "250 Ordering failed";// ex.Message;
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