using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Reflection;
using System.IO;

using Wcss;

namespace wctMain.Service
{
    /// <summary>
    /// Summary description for Service. Parse input and pass on to page to be rendered
    /// </summary>
    [WebService(Namespace = "http://foxtheatre.com/Svc/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
    public class controlrenderer : System.Web.Services.WebService
    {
        public controlrenderer() { }
        
        [WebMethod(EnableSession = true)]
        public object getView(string view, string context)
        {
            return controlrenderer.renderView(view, context, "agent not sent");
        }

        [WebMethod(EnableSession = true)]
        public object getView(string view, string context, string uAgent)
        {
            return controlrenderer.renderView(view, context, uAgent);
        }

        private static object renderView(string view, string context, string uAgent)
        {
            string control = string.Empty;
            string prop1 = "context";
            string val1 = context.Trim(new char[] { '/' });
            string titleVal = string.Empty;
            
            //handle mobile mail manager
            if (view.IndexOf("MobileMailManager") != -1)
            {
                string parsedView = string.Empty;
                if (view.IndexOf("=") != -1)
                {
                    string[] parts = view.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if(parts.Length > 1)
                        parsedView = parts[1].ToLower();
                }

                view = (parsedView.Trim().Length == 0) ? "subscribe" : parsedView;
            }
            
            switch (view)
            {
                case "search":
                    control = "/View/" + view + "View.ascx";
                    prop1 = "terms";
                    val1 = context;
                    break;
                case "directions":
                case "mailerconfirm":
                case "unsubscribe":
                case "subscribe":
                case "unsubscribemailup":
                case "subscribemailup":
                case "contact":
                case "faq":
                case "about":
                case "history":
                case "mailermanage":
                case "parking":
                case "production":
                case "privacy":
                case "terms":
                case "accommodations":
                case "studentlaminate":
                case "textus":
                    control = "/View/" + view + "View.ascx";
                    break;
                case "mailer":
                    break;

                case "adminshow":
                default:
                    control = "/View/ShowView.ascx";
                    val1 = view.Trim(new char[] { '/' });
                    titleVal = val1;
                    break;
            }

            return controlrenderer.RenderUserControl(control, prop1, val1, titleVal, uAgent);
        }

        /// <summary>
        /// Renders the specified UserControl. In this case - views
        /// </summary>
        /// <param name="path"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private static object RenderUserControl(string path, string propertyName1, object propertyValue1, string titleVal, string uAgent)
        {
            wctMain.Controller.MainBasePage pageHolder = new Controller.MainBasePage();
                        
            UserControl viewControl =
               (UserControl)pageHolder.LoadControl(path);
            

            Type viewControlType = viewControl.GetType();
            PropertyInfo property1 = viewControlType.GetProperty(propertyName1);
            if (property1 != null)
                property1.SetValue(viewControl, propertyValue1, null);
            PropertyInfo property2 = viewControlType.GetProperty("uAgent");
            if (property2 != null)
                property2.SetValue(viewControl, uAgent, null);

            
            pageHolder.Controls.Add(viewControl);
            StringWriter output = new StringWriter();
            HttpContext.Current.Server.Execute(pageHolder, output, true);

            return new 
            {
                title = getViewTitle(pageHolder, titleVal),
                renderedView = output.ToString()                
            };
        }

        private static string getViewTitle(wctMain.Controller.MainBasePage basePage, string titleVal)
        {
            if (titleVal.Trim().Length > 0)
            {
                Show _show;

                if(titleVal == "adminshow") 
                    _show = basePage.Atx.CurrentShowRecord;
                else
                    _show = basePage.Ctx.GetCurrentShowByUrl(titleVal, true, false);

                if (_show != null)
                    return _show.Display.ShowHeader;
            }

            return string.Empty;
        }


        //TODO not done yet!!!!
        //private static string RenderUserPage(string path, string propertyName1, object propertyValue1)
        //{

            



        //    StringWriter _writer = new StringWriter();
        //    HttpContext.Current.Server.Execute("/View/WysEditor.aspx", _writer);

        //    string html = _writer.ToString();
        //    return html;

        //    //Type t = BuildManager.GetCompiledType("/mypage.aspx");
        //    //Page p = (Page)Activator.CreateInstance(t);
        //    //p.ProcessRequest(HttpContext.Current);
        //}
    }
}