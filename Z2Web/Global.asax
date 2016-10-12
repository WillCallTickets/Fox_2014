<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Net.Http" %>
<%@ Import Namespace="System.Web.Optimization" %>

<script runat="server">

    /// <summary>
    /// found this tool but haven't looked into it yet
    /// http://blogs.msdn.com/b/webdev/archive/2013/04/04/debugging-asp-net-web-api-with-route-debugger.aspx
    /// </summary>



    private static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        RouteTable.Routes.MapHttpRoute(
            name: "ApiSearch",
            routeTemplate: "api/{controller}/{query}/{limit}/{pagenum}",
            defaults: new { limit = 10, pagenum = 0 },
            constraints: new { limit = @"\d+", pagenum = @"\d+" }
        );
        RouteTable.Routes.MapHttpRoute(
            name: "ApiDefault",
            routeTemplate: "api/{controller}/{*pathinfo}",            
            defaults: new { pathinfo = System.Web.Http.RouteParameter.Optional }
        );

        RouteTable.Routes.MapHttpRoute(
            name: "siteDirector",
            routeTemplate: "sd/{*link}",//link should always be null - we are looking for querystring vars
            defaults: new { controller = "Sd" }
        );

        RouteTable.Routes.Add(new Route("admin/{*pathInfo}", new StopRoutingHandler()));
             
        // StopRoutingHandler for .axd and .asmx requests 
        //routes.Add(new Route("Admin/{*pathInfo}", new StopRoutingHandler()));
        //RouteTable.Routes.Add(new Route("admin/Register.aspx/{*pathInfo}", new StopRoutingHandler()));
        //RouteTable.Routes.Add(new Route("Services/{*pathInfo}", new StopRoutingHandler()));
        
        //RouteTable.Routes.Add(new Route("images/{*pathInfo}", new StopRoutingHandler()));
        
        RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");         
        RouteTable.Routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));        
        RouteTable.Routes.Add(new Route("{service}.asmx/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Add(new Route("{service}.ashx/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Add(new Route("{service}.asax/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Add(new Route("favicon.ico", new StopRoutingHandler()));


        //***************************************************************************************
        //NOTE static pages may need to be registered in /App_Code/Service/controlrenderer.cs
        //***************************************************************************************
               
        string mainlanding = "~/index.aspx"; 
        string staticpage = "~/site.aspx";
        
        RouteTable.Routes.MapPageRoute("guestAddPage", "guestadd.asp", "~/Error.aspx");//404.html in order to use html files - you need to add a build provider
        
        //handle kiosk
        RouteTable.Routes.MapPageRoute("kioskPage", "kiosk", "~/kiosk.aspx");
        
        
        //static pages past and present
        RouteTable.Routes.MapPageRoute("aspxPageParams", "{staticpage}.aspx{staticparams}", staticpage + "{staticparams}");
        RouteTable.Routes.MapPageRoute("aspxPage", "{staticpage}.aspx", staticpage);
        RouteTable.Routes.MapPageRoute("defaultRouteWithParams", "{*pathInfo}", mainlanding);
        RouteTable.Routes.MapPageRoute("defaultRoute", "", mainlanding);
        
        
        //handle 404 errors gracefully
        //string errorpage = "/Error.aspx";
        //RouteTable.Routes.MapPageRoute("error1", "{misc}", errorpage);
        //RouteTable.Routes.MapPageRoute("error2", "{misc1}/{misc2}", errorpage);
        //RouteTable.Routes.MapPageRoute("error3", "{misc1}/{misc2}/{misc3}", errorpage);
        //RouteTable.Routes.MapPageRoute("error4", "{misc1}/{misc2}/{misc3}/{misc4}", errorpage);        
    }

    void Application_Start(Object sender, EventArgs e)
    {
        // Code that runs on application startup
        //Wcss._ImageManager.EnsureThumbDirectories("act");
        
        // routing        
        RegisterRoutes(System.Web.Routing.RouteTable.Routes);
        
        //bundle files
        //CreateBundles();        
    }

    private void CreateBundles()
    {
        //var cssBundle = new Bundle("/assets/styles");
        //cssBundle.IncludeDirectory("/assets/styles", "*.css");
        
        
        //BundleTable.Bundles.Add(cssBundle);
    }
    
    void Application_Error(Object sender, EventArgs e)
    {
        if (Server != null)
        {
            Exception err = Server.GetLastError();
            if (err != null && err.InnerException != null && err.InnerException.Message != null &&
                err.InnerException.Message.ToLower().IndexOf("maximum request length exceeded") != -1)
            {
                Response.ClearHeaders();
                Server.ClearError();
                //Response.Redirect("/Admin/MaxLengthError.html", true);
            }
        }
    }
    
    void Application_BeginRequest(object sender, EventArgs e)
    {
        //string strCurrentPath = Request.Path.ToLower();
        //handle urls where the number of elements does not match
        //handle 404 errors

    }

    void Application_End(Object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }
    
    void Session_Start(Object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        
    }

    void Session_End(Object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        
    }
           
</script>
