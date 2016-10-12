using System;

using wctMain.Controller;

namespace wctMain.Admin._settings
{
    public partial class Settings_Director : MainBasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {   
            SetPageControl();
        }
     
        private void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string controlToLoad = "Default";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "default":
                case "images":
                case "downloads":
                case "ship":
                case "flow":
                case "pagemsg":
                case "admin":
                case "email":
                case "fb_integration":
                case "service":
                case "addnew":
                    controlToLoad = @"_controls\Settings";
                    break;
                
                case "searches":
                    controlToLoad = @"_controls\Searches";
                    break;
                case "errors":
                    controlToLoad = @"_controls\Errors";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"{0}.ascx", controlToLoad)));
        }
    }
}