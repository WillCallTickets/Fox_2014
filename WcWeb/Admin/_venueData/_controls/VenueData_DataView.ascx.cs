using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using System.Linq;

using SubSonic;
using Wcss;
using wctMain.Controller;

namespace wctMain.Admin._venueData._controls
{
    public partial class VenueData_DataView : MainBaseControl
    {
        ///
        ///Boilerplate and postbackevent handler
        ///
        #region Page-Control Overhead

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.Page.User.IsInRole("VenueDataViewer"))
                base.Redirect("/Admin/_venueData/VenueData_Director.aspx?p=vnuentry");

            //wctMain.Admin.AdminEvent.CurrentMailerChanged += new AdminEvent.CurrentMailerChangedEvent(RefreshCurrent);

            ////key into the MailerSelector when in insert mode
            //FormView frmSelector = (FormView)Mailer_Menu1.FindControl("FormMailer");
            //if (frmSelector != null)
            //    frmSelector.ModeChanging += frmMailer_ModeChanging;
        }

        void frmMailer_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            workspace.Visible = e.NewMode != FormViewMode.Insert;
        }

        public override void Dispose()
        {
            //FormView frmSelector = (FormView)Mailer_Menu1.FindControl("FormMailer");
            //if (frmSelector != null)
            //    frmSelector.ModeChanging -= frmMailer_ModeChanging;

            //wctMain.Admin.AdminEvent.CurrentMailerChanged -= new AdminEvent.CurrentMailerChangedEvent(RefreshCurrent);
            //base.Dispose();
        }

        protected void RefreshCurrent(object sender, EventArgs e)
        {
            BindEditorControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindEditorControls();
        }

        protected void BindEditorControls()
        {
            //frmCurrentMailer.DataBind();
        }

        #endregion

    }
}
