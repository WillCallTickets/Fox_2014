using System;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.IO;
//using System.Text;
//using System.Text.RegularExpressions;
using System.Collections.Generic;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin._shows._controls
{
    public partial class ShowData : MainBaseControl
    {   
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!((this.Page.User.IsInRole("VenueDataEntry")) || (this.Page.User.IsInRole("VenueDataViewer"))))
                base.Redirect("/Admin/_shows/ShowDirector.aspx");

            wctMain.Admin.AdminEvent.ShowChosen += new AdminEvent.ShowChosenEvent(BindEditorControls);
        }

        public override void Dispose()
        {
            wctMain.Admin.AdminEvent.ShowChosen -= new AdminEvent.ShowChosenEvent(BindEditorControls);
            base.Dispose();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentShowRecord == null)
                base.Redirect("/Admin/_shows/ShowDirector.aspx");

            if (!IsPostBack)
            {
                BindEditorControls(null, null);
            }
        }

        protected void BindEditorControls(object sender, EventArgs e)
        {
            this.DataEntry_Container1.ExplicitBind();
        }
}
}
