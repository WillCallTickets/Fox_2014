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
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    public partial class Data_VenueSelect : MainBaseControl
    {
        ///
        ///Boilerplate
        ///
        #region Page-Control Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindEditorControls();
        }

        protected void BindEditorControls()
        {
            ddlVenue.DataBind();
            txtVenueSelectStart.DataBind();
            txtVenueSelectEnd.DataBind();
        }

        #endregion

        protected void ddlVenue_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //allow for default item (count <= 1)
            //if (ddl.Items.Count <= 1 && Vtx.VD_CurrentOrg.Venues.Count() > 0)
            //{
            //    ddl.DataSource = Vtx.VD_CurrentOrg.Venues;
            //    ddl.DataTextField = "VenueName";
            //    ddl.DataValueField = "Id";
            //}
        }

        protected void ddlVenue_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            ddl.SelectedIndex = -1;

            //set the selected according to the session
            //ListItem li = ddl.Items.FindByValue(Vtx.VD_CurrentVenueViewId.ToString());
            //if (li != null)
            //    li.Selected = true;
        }

        protected void ddlVenue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //Vtx.VD_CurrentVenueViewId = int.Parse(ddl.SelectedValue);
        }

        protected void txtVenueSelectDate_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            DateTime dt = DateTime.Parse(txt.Text);

            //if (txt.ID.IndexOf("Start") != -1)
            //    Vtx.VenueSelectStartDate = dt;
            //else if (txt.ID.IndexOf("End") != -1)
            //    Vtx.VenueSelectEndDate = dt;
        }
}
}
