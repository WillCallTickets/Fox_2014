using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Reflection;
using System.IO;


using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;



using Wcss;

namespace z2Main.Service
{
    /// <summary>
    /// Summary description for Service. Parse input and pass on to page to be rendered
    /// </summary>
    [WebService(Namespace = "http://foxtheatre.com/Svc/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
    public class getRenderedView : System.Web.Services.WebService
    {
        public getRenderedView() {
        }

        [WebMethod(EnableSession = true)]
        public object getView(string view)
        {   
            return getRenderedView.renderView(view, string.Empty);
        }

        private static object renderView(string view, string context)
        {
            string control = string.Empty;
            string prop1 = string.Empty;
            string val1 = string.Empty;// context.Trim(new char[] { '/' });
            string titleVal = string.Empty;

            switch (view)
            {
                case "contact":
                case "newsletter":
                case "privacy":
                    control = "/View/" + view + "View.ascx";
                    break;

                default:
                    control = "/View/AboutView.ascx";
                    break;
            }

            return getRenderedView.RenderUserControl(control, prop1, val1, titleVal);
        }

        /// <summary>
        /// Renders the specified UserControl. In this case - views
        /// </summary>
        /// <param name="path"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private static object RenderUserControl(string path, string propertyName1, object propertyValue1, string titleVal)
        {
            z2Main.Controller.PageBase pageHolder = new Controller.PageBase();

            UserControl viewControl =
               (UserControl)pageHolder.LoadControl(path);

            Type viewControlType = viewControl.GetType();
            PropertyInfo property1 = viewControlType.GetProperty(propertyName1);

            if (property1 != null)
                property1.SetValue(viewControl, propertyValue1, null);

            pageHolder.Controls.Add(viewControl);
            StringWriter output = new StringWriter();
            HttpContext.Current.Server.Execute(pageHolder, output, true);

            return new
            {
                renderedView = output.ToString()
            };
        }

        //private static string getViewTitle(z2Main.Controller.PageBase basePage, string titleVal)
        //{
        //    if (titleVal.Trim().Length > 0)
        //    {
        //        Show _show;

        //        if (titleVal == "adminshow")
        //            _show = basePage.Atx.CurrentShowRecord;
        //        else
        //            _show = basePage.Ctx.GetCurrentShowByUrl(titleVal, true);

        //        if (_show != null)
        //            return _show.Display.ShowHeader;
        //    }

        //    return string.Empty;
        //}

    }
}