using System;

using wctMain.Controller;

namespace wctMain.Admin._editors
{
    public partial class Editor_Director : MainBasePage
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

        protected string controlTitle = "Create a new Show";
        protected bool IsCollectionEditor = false;
        private void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string controlToLoad = "CustomerPicker";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "age":
                    controlToLoad = @"_controls\Editor_Age";
                    controlTitle = "Ages Editor";
                    break;
                case "act":
                    controlToLoad = @"_controls\Editor_Act";
                    controlTitle = "Act Editor";
                    break;
                case "customer":
                    controlToLoad = @"_controls\Editor_Customer";
                    controlTitle = "Customer Editor";
                    break;
                case "employee":
                    controlToLoad = @"/Admin/_collectionEditors/_employee/_controls/Employee_Edit";
                    controlTitle = "Employee Editor";
                    IsCollectionEditor = true;
                    break;
                case "faq":
                    controlToLoad = @"/Admin/_collectionEditors/_faq/_controls/Faq_Edit";
                    controlTitle = "FAQ Editor";
                    IsCollectionEditor = true;
                    break;
                case "genre":
                    controlToLoad = @"_controls\Editor_Genre";
                    controlTitle = "Genre Editor";
                    break;
                case "promoter":
                    controlToLoad = @"_controls\Editor_Promoter";
                    controlTitle = "Promoter Editor";
                    break;
                case "defaultshow":
                    controlToLoad = @"_controls\Editor_DefaultShow";
                    controlTitle = "Set Default Show";
                    break;                    
                case "venue":
                    controlToLoad = @"_controls\Editor_Venue";
                    controlTitle = "Venue Editor";
                    break;
                case "video":
                    controlToLoad = @"_controls\Editor_Video";
                    controlTitle = "Video Information";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"{0}.ascx", controlToLoad)));
        }
    }
}