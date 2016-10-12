using System;

using wctMain.Controller;
using Wcss;

namespace wctMain.Admin._shows
{
    public partial class ShowDirector : MainBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            wctMain.Admin.AdminEvent.ShowChosen += new Admin.AdminEvent.ShowChosenEvent(EventHandler_ShowChosenChanged);
        }
        
        public override void Dispose()
        {
            wctMain.Admin.AdminEvent.ShowChosen -= new Admin.AdminEvent.ShowChosenEvent(EventHandler_ShowChosenChanged);
            base.Dispose();
        }

        private void EventHandler_ShowChosenChanged(object sender, Admin.AdminEvent.ShowChosenEventArgs e)
        {
            //reset the current show
            Atx.SetCurrentShowRecord(e.ChosenId);
            Atx.CurrentActId = 0;
            Atx.CurrentVenueId = 0;

            //if we choose a show - move away from the create page
            if (Request.QueryString["p"] == null || Request.QueryString["p"].ToLower() == "selection")
                base.Redirect("/Admin/_shows/ShowDirector.aspx?p=details");
            else
                base.Redirect(Request.RawUrl);
        }

        protected override void OnPreInit(EventArgs e)
        {

            string req = Request.QueryString["p"];

            QualifySsl(req != null && req.ToLower() != "media");

            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageControl();
        }

        private string controlToLoad = "Selection";
        protected string controlTitle = "Create a new Show";
        protected void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "selection":
                    controlToLoad = "ShowCreator";
                    controlTitle = "Create a new Show";
                    break;
                case "details":
                    controlToLoad = "ShowDetails";
                    controlTitle = "Show Details";
                    break;
                case "images":
                    controlToLoad = "ShowImages";
                    controlTitle = "Show Images";
                    break;
                case "data":
                    controlToLoad = "ShowData";
                    controlTitle = "Show Data";
                    break;
                case "links":
                    controlToLoad = "ShowLinks";
                    controlTitle = "Show Links";
                    break;
                case "writeup":
                    controlToLoad = "ShowWriteup";
                    controlTitle = "Show Writeup";
                    break;
                case "acts":
                    controlToLoad = "ShowDate_Acts";
                    controlTitle = "Show Acts";
                    break;
                case "promoters":
                    controlToLoad = "ShowPromoters";
                    controlTitle = "Show Promoters";
                    break;
            }

            System.Web.UI.Control ctrl = LoadControl(string.Format(@"_controls\{0}.ascx", controlToLoad));

            if(!Content.Controls.Contains(ctrl))
                Content.Controls.Add(ctrl);
        }
    }
}