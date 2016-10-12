using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;

using System.Linq;

using SubSonic;
using Wcss;
using wctMain.Controller;
using WctControls.WebControls;

namespace wctMain.Admin._venueData._controls
{
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    [ToolboxData("<{0}:DataEntry_ShowInfo runat='Server' ></{0}:DataEntry_ShowInfo>")]
    public partial class DataEntry_ShowInfo : MainBaseControl, Wcss.VenueData.Helpers.IExplicitBinder
    {
        /// <summary>
        /// Show holds all the collections that we need to edit
        /// </summary>
        protected Show BindingShowRecord 
        { 
            get 
            {
                return ((Wcss.VenueData.Helpers.IReBindingShowControl)this.NamingContainer).GetBindingShowRecord(); 
            } 
        }

        #region Page-Control Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
            //BindEditorControls();//let container make the call
        }

        public void ExplicitBind()
        {
            formEntry.DataBind();
        }

        #endregion

        protected void formEntry_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            List<VdShowInfo> list = new List<VdShowInfo>();

            if (BindingShowRecord != null)
                list.Add(BindingShowRecord.VdShowInfoRecord);

            form.DataSource = list;
            form.DataKeyNames = new string[] { "Id" };
        }

        protected void formEntry_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
        }

        protected void formEntry_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = ErrorDisplayLabel1;
            VdShowInfo ent = BindingShowRecord.VdShowInfoRecord;

            if (ent != null && error != null)
            {
                error.ResetErrors();

                try
                {
                    ent.Agent = e.NewValues["Agent"].ToString();
                    ent.Buyer = e.NewValues["Buyer"].ToString();
                    ent.TicketGross = decimal.Parse(e.NewValues["TicketGross"].ToString());
                    ent.TicketsSold = int.Parse(e.NewValues["TicketsSold"].ToString());
                    ent.CompsIn = int.Parse(e.NewValues["CompsIn"].ToString());
                    ent.FacilityFee = decimal.Parse(e.NewValues["FacilityFee"].ToString());
                    ent.Concessions = decimal.Parse(e.NewValues["Concessions"].ToString());
                    ent.BarTotal = decimal.Parse(e.NewValues["BarTotal"].ToString());
                    ent.BarPerHead = decimal.Parse(e.NewValues["BarPerHead"].ToString());
                    ent.MarketingDays = int.Parse(e.NewValues["MarketingDays"].ToString());
                    ent.Mod = e.NewValues["MOD"].ToString();
                    ent.ProductionLabor = decimal.Parse(e.NewValues["ProductionLabor"].ToString());
                    ent.SecurityLabor = decimal.Parse(e.NewValues["SecurityLabor"].ToString());
                    ent.Hospitality = decimal.Parse(e.NewValues["Hospitality"].ToString());
                    ent.MarketPlays = int.Parse(e.NewValues["MarketPlays"].ToString());
                    ent.Notes = e.NewValues["Notes"].ToString();

                    if (ent.IsDirty)
                    {
                        ent.DateModified = DateTime.Now;
                        ent.Save();
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    error.ErrorList.Add(ex.Message);
                    error.DisplayErrors();
                }
            }

            BindParentContainer();
        }

        public void BindParentContainer()
        {
            ((Wcss.VenueData.Helpers.IReBindingShowControl)this.NamingContainer).ExplicitBind();
        }

        protected void formEntry_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
        }

        protected void formEntry_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = ErrorDisplayLabel1;
            error.ResetErrors();
            error.DisplayErrors();//reset any error display

            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)//handles cancel correctly
                BindParentContainer();
        }
}
}
