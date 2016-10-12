using System;
using System.Web.Services;
using System.Collections.Generic;

using wctMain.Controller;

using Wcss;

namespace wctMain.Admin._venueData
{
    public partial class VenueData_Director : MainBasePage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string control = string.Empty;
            string req = Request.QueryString["p"];
            if (req != null && req.Trim().Length > 0)
                control = req;
        }
     
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);//_directorContext != "mlrsend");//deal with iframes and protocol load
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {   
            SetPageControl();
        }

        public string _directorContext
        {
            get
            {
                string controlToLoad = "vnuactq";

                string req = Request.QueryString["p"];

                if (req != null && req.Trim().Length > 0)
                    controlToLoad = req;

                return controlToLoad;
            }
        }

        private void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string controlToLoad = _directorContext;

            switch (controlToLoad.ToLower())
            {
                //templating
                
                case "vnuview":
                    controlToLoad = @"_controls\VenueData_DataView";
                    break;
                case "vnuactq":
                    controlToLoad = @"_controls\VenueData_DataActQ";
                    break;
                case "vnureport":
                    controlToLoad = @"_controls\VenueData_DataReport";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"{0}.ascx", controlToLoad)));
        }
    }
}