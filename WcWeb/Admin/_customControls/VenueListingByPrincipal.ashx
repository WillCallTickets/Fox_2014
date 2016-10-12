<%@ WebHandler Language="C#" Class="Admin._customControls.VenueListingByPrincipal" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;

using Wcss;

namespace Admin._customControls
{
    public class nvPair
    {
        public string name;
        public string val;
        
        public nvPair(string Name, string Val)
        {
            name = Name;
            val = Val;
        }
    }
    
    public class VenueListingByPrincipal : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                    string principal = context.Request.Params["principal"].ToString();
                    string limit = context.Request.Params["limit"].ToString();

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    sb.Append("DECLARE  @DefaultVenue varchar(256) ");
                    sb.Append("SET		@DefaultVenue = ");
                    sb.Append("         CASE ");
                    sb.Append("             WHEN @principal = 'fox' THEN 'The Fox Theatre' ");
                    sb.Append("             WHEN @principal = 'bt' THEN 'The Boulder Theater' ");
                    sb.Append("             ELSE '' ");
                    sb.Append("         END ");

                    sb.AppendFormat("SELECT TOP {0}  v.[Id], v.[Name], ", limit);
                    sb.Append("CASE WHEN v.[Name] = @DefaultVenue THEN '' ELSE v.[Name] END AS [VenueRank]	");
                    sb.Append("FROM [Venue] v ");
                    //leave the all comparison even though w are not using here
                    sb.Append("WHERE (CASE WHEN @principal = 'all' THEN 1 WHEN CHARINDEX(@principal, v.[vcPrincipal]) >= 1 THEN 1 ELSE 0 END = 1) ");
                    sb.Append("ORDER BY [VenueRank] ");


                    _DatabaseCommandHelper dch = new _DatabaseCommandHelper(sb.ToString());
                    dch.AddCmdParameter("@principal", principal, System.Data.DbType.String);

                    List<nvPair> list = new List<nvPair>();

                    using (System.Data.IDataReader dr = SubSonic.DataService.GetReader(dch.Cmd))
                    {
                        while (dr.Read())
                        {
                            string name = dr.GetValue(dr.GetOrdinal("Name")).ToString();
                            string value = dr.GetValue(dr.GetOrdinal("Id")).ToString();
                            list.Add(new nvPair(name, value));
                        }

                        dr.Close();
                    }


                    if (list.Count == 0)
                    {
                        //TODO - not sure why this doesn't work
                        //this has to be done in 2 parts/calls - otherwise principal is lost
                        //string str = string.Format("Please create a venue for this owner", context.Request.Params["principal"].ToString());
                        
                        list.Add(new nvPair("Please create a venue for this owner", "0"));
                    }
                    
                    string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(list);

                    //return a json response
                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        success = "success",
                        listing = json
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