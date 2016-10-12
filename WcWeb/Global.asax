<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Net.Http" %>

<script runat="server">

    private static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        ////"/api/searches/%QUERY/15/0",
        ////string entityType, string query, int limit, int pagenum
        //RouteTable.Routes.MapHttpRoute(
        //    name: "ApiUserSearch",
        //    routeTemplate: "api/searches/{query}/{limit}/{pagenum}",
        //    defaults: new { controller = "searches", limit = @"\d+", pagenum = @"\d+" }
        //);
        
        ////string entityType, string query, int limit, int pagenum
        //RouteTable.Routes.MapHttpRoute(
        //    name: "ApiCollSearches",
        //    routeTemplate: "api/searches/{context}/{query}/{limit}/{pagenum}",
        //    defaults: new { controller = "searches", limit = @"\d+", pagenum = @"\d+" }
        //);

        //RouteTable.Routes.MapHttpRoute(
        //    name: "ApiSearch",
        //    routeTemplate: "api/{controller}/{query}/{limit}/{pagenum}",
        //    defaults: new { limit = 10, pagenum = 0 },
        //    constraints: new { limit = @"\d+", pagenum = @"\d+" }
        //);

        

        RouteTable.Routes.MapHttpRoute(
            name: "ApiAdminSearches",
            routeTemplate: "api/searches/admin/{context}/{query}/{limit}/{pagenum}",
            defaults: new { controller = "searches", limit = @"\d+", pagenum = @"\d+", accessLevel = "admin" }
        );

        RouteTable.Routes.MapHttpRoute(
            name: "ApiUserSearches",
            routeTemplate: "api/searches/{context}/{query}/{limit}/{pagenum}",
            defaults: new { controller = "searches", limit = @"\d+", pagenum = @"\d+", accessLevel = "normal" }
        );
        
        RouteTable.Routes.MapHttpRoute(
            name: "ApiSlashSearch",
            routeTemplate: "api/searches/{*pathinfo}",
            defaults: new { controller = "searches", pathinfo = System.Web.Http.RouteParameter.Optional, accessLevel = "normal" }
        );
        
        RouteTable.Routes.MapHttpRoute(
            name: "ApiDefault",
            routeTemplate: "api/{controller}/{*pathinfo}",            
            defaults: new { pathinfo = System.Web.Http.RouteParameter.Optional }
        );


        //RouteTable.Routes.MapHttpRoute(
        //    name: "KioskDefault",
        //    routeTemplate: "kiosk/{*pathinfo}",
        //    defaults: new { controller = "Kiosk", pathinfo = System.Web.Http.RouteParameter.Optional }
        //);

        //RouteTable.Routes.MapHttpRoute(
        //    name: "legacyEvent",
        //    routeTemplate: "{year}/{month}/{day}/{time}/{eventtitle}",
        //    defaults: new { controller = "Legacy" },
        //    constraints: new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
        //);

        //RouteTable.Routes.MapHttpRoute(
        //    name: "siteDirector",
        //    routeTemplate: "sd/{*link}",//link should always be null - we are looking for querystring vars
        //    defaults: new { controller = "Sd" }
        //);


        //route requests for mobile mail manager
        RouteTable.Routes.Add(new Route("MobileMailManager.aspx{pathInfo}", new StopRoutingHandler()));
        

        RouteTable.Routes.Add(new Route("admin/{*pathInfo}", new StopRoutingHandler()));
             
        // StopRoutingHandler for .axd and .asmx requests 
        //routes.Add(new Route("Admin/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Add(new Route("admin/Register.aspx/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Add(new Route("Services/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Add(new Route("MainServices/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Add(new Route("images/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}"); 
        
        RouteTable.Routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
        
        RouteTable.Routes.Add(new Route("{service}.asmx/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Add(new Route("{service}.ashx/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Add(new Route("{service}.asax/{*pathInfo}", new StopRoutingHandler()));
        RouteTable.Routes.Add(new Route("favicon.ico", new StopRoutingHandler()));

        RouteTable.Routes.MapHttpRoute(
            name: "ical",
            routeTemplate: "ical/{calItemLink}",
            defaults: new { controller = "iCal" }
        );
        
        
        string mainlanding = "~/Index.aspx"; 
        string staticpage = "~/Site.aspx";
        
        //Handle legacy event pages
        
        RouteTable.Routes.MapHttpRoute(
            name: "legacyEvent",
            routeTemplate: "{year}/{month}/{day}/{time}/{eventtitle}",
            defaults: new { controller = "Legacy" },
            constraints: new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
        );
        RouteTable.Routes.MapHttpRoute(
            name: "legacyEventAspx",
            routeTemplate: "Event.aspx",
            defaults: new { controller = "Legacy" }
        );
        RouteTable.Routes.MapHttpRoute(
            name: "legacyEventAspxParams",
            routeTemplate: "Event.aspx{pathinfo}",
            defaults: new { controller = "Legacy", pathinfo = System.Web.Http.RouteParameter.Optional }            
        );
        RouteTable.Routes.MapHttpRoute(
            name: "legacyIndex",
            routeTemplate: "Index2.aspx",
            defaults: new { controller = "Legacy" }
        );
        RouteTable.Routes.MapHttpRoute(
            name: "legacyStoreChoose",
            routeTemplate: "Store/{*chooseticket}",
            defaults: new { controller = "Legacy" }
        );

        //NOTE static pages may need to be registered in /App_Code/Service/controlrenderer.cs

        RouteTable.Routes.MapPageRoute("guestAddPage", "guestadd.asp", "~/Error.aspx");//404.html in order to use html files - you need to add a build provider
        
        //handle adverts
        RouteTable.Routes.MapPageRoute("advertPage", "kiosk/{*params}", "~/Kiosk.aspx");
        
        
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
        Wcss._ImageManager.EnsureThumbDirectories("act");
        Wcss._ImageManager.EnsureThumbDirectories("venue");
        Wcss._ImageManager.EnsureThumbDirectories("show");
        Wcss._ImageManager.EnsureThumbDirectories("advert");
        Wcss._ImageManager.EnsureThumbDirectories("emailer");
        Wcss._ImageManager.EnsureThumbDirectories("uploads");
        
        // routing        
        RegisterRoutes(System.Web.Routing.RouteTable.Routes);
        
        //bundle files
        CreateBundles();        
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
                Response.Redirect("/Admin/MaxLengthError.html", true);
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
