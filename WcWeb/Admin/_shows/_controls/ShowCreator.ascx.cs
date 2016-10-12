using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Newtonsoft.Json;

using Wcss;
using wctMain.Controller;
using WctControls.WebControls;

//FormatString='<%# WctControls.WebControls.Bootstrap.DateTimePicker.Time_FormatString %>' 
namespace wctMain.Admin._shows._controls
{
    public partial class ShowCreator : MainBaseControl, IPostBackEventHandler
    {
        #region PostBack

        public void RaisePostBackEvent(string eventArgument)
        {
            Dictionary<string, string> args = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventArgument);
            string commandName = args["commandName"].ToLower();

            //switch (commandName)
            //{
            //    case "dtworkaround":
            //        break;
            //}
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Atx.CurrentActId = 0;
                Atx.CurrentVenueId = 0;                
            }
            
            rdoPrincipal.DataBind();            
            ddlAges.DataBind();
        }

        #region Add Show

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            base.Redirect("/Admin/_shows/ShowDirector.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ErrorDisplayLabel error = ErrorDisplayLabel1;
            error.ResetErrors();
            
            try
            {
                #region Gather inputs and validate

                DateTime dos = DateTime.MinValue;
                DateTime announce = DateTime.MinValue;
                DateTime onsale = DateTime.MinValue;

                //start - dos
                string iDos = txtDateStart.Text.Trim();
                if (iDos.Length == 0 || iDos == DateTime.MinValue.ToString())
                    error.ErrorList.Add("Show Date is required.");
                else
                {
                    if (!DateTime.TryParse(iDos, out dos))
                        error.ErrorList.Add("Show Date is not valid.");

                    if (dos.Year < 1992)
                        error.ErrorList.Add("Show Date is not valid.");
                }

                string iAnn = txtDateAnnounce.Text.Trim();
                //announce - min value is ignored
                if (iAnn.Length > 0)
                {
                    if (!DateTime.TryParse(iAnn, out announce))
                        error.ErrorList.Add("Announce Date is not valid.");
                }

                //onsale - min value is ignored
                string iOnsale = txtDateOnsale.Text.Trim();
                if (iOnsale.Length > 0)
                {
                    if (!DateTime.TryParse(iOnsale, out onsale))
                        error.ErrorList.Add("OnSale Date is not valid.");
                }

                //show time
                string showtime = (chkTba.Checked) ? "TBA" : txtShowTime.Text.Trim();

                //principal
                _Enums.Principal prince = (_Enums.Principal)Enum.Parse(typeof(_Enums.Principal), rdoPrincipal.SelectedValue, true);

                //venue
                Venue venue = new Venue(int.Parse(ddlVenue.SelectedValue));

                //ages
                Age age = new Age(int.Parse(ddlAges.SelectedValue));

                //act
                Act act = new Act(Editor_Actc.SelectedIdx);

                if(act == null || act.Id == 0)
                    error.ErrorList.Add("Please select an act for this show. Your selection was not found.");

                //display any errors and return 
                if (error.ErrorList.Count > 0)
                {
                    error.DisplayErrors();
                    return;
                }

                #endregion

                #region Create the initial show

                Show show = new Show();
                show.ApplicationId = _Config.APPLICATION_ID;
                show.DtStamp = DateTime.Now;

                show.VenueRecord = venue;

                ActCollection acts = new ActCollection();
                acts.Add(act);

                show.Name = Show.CalculatedShowName(dos, venue, acts);

                if (announce > Utils.Constants._MinDate)
                    show.AnnounceDate = announce;
                if (onsale > Utils.Constants._MinDate)
                    show.DateOnSale = onsale;

                show.IsActive = true;
                show.Centered_X = true;
                show.Centered_Y = true;

                show.VcPrincipal = prince.ToString();

                show.Save();

                #endregion

                #region Create the First Show Date

                ShowDate sd = new ShowDate();
                sd.DtStamp = DateTime.Now;
                sd.AgeRecord = age;
                sd.ShowRecord = show;
                sd.DateOfShow = DateTime.Parse(dos.ToString("yyyy-MM-dd hh:mmtt"));
                sd.ShowTime = showtime;
                sd.IsAutoBilling = true;
                sd.MenuBilling = null;
                sd.IsActive = true;

                ShowStatus newStat = _Lookits.ShowStatii.GetList().Find(delegate(ShowStatus match) { return (match.Name == "OnSale"); });
                if (newStat != null)
                    sd.ShowStatusRecord = newStat;

                sd.Save();

                #endregion

                #region Ensure the JShowHeadline act

                JShowAct showAct = new JShowAct();
                showAct.DtStamp = DateTime.Now;
                showAct.TShowDateId = sd.Id;
                showAct.TActId = act.Id;
                showAct.DisplayOrder = sd.JShowActRecords().Count;//should be zero
                showAct.TopBilling = true;

                sd.JShowActRecords().Add(showAct);
                sd.JShowActRecords().SaveAll();

                #endregion

                #region Cleanup and Binding

                //does this also happen in mode change? Answer - NO!!!!
                //reset current act to null
                Atx.CurrentActRecord = null;

                //we need to micromanage the sequence of events
                //do not fire showchosen event as it will cause extra binding
                Atx.SetCurrentShowRecord(show.Id);

                //if the current active edit principal does not match what we are adding to here - then change
                //right now - go with all
                if (Atx.CurrentEditPrincipal != _Enums.Principal.all && Atx.CurrentEditPrincipal != prince)
                    Atx.CurrentEditPrincipal = _Enums.Principal.all;

                //if the new dated is less than the selector's startdate- then reset the date
                if (Atx.CurrentShowRecord.FirstDate < Atx.CurrentShowListStartDate)
                    Atx.CurrentShowListStartDate = Atx.CurrentShowRecord.FirstDate;
                
                #endregion

                //let parent handle redirection
                AdminEvent.OnShowChosen(this, show.Id);

            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                error.ErrorList.Add(ex.Message);
                error.DisplayErrors();
            }
        }

        #endregion

        #region List Binding

        protected void rdoPrincipal_DataBinding(object sender, EventArgs e)
        {
            List<string> list = new List<string>(Enum.GetNames(typeof(_Enums.Principal)));

            //limit to fox and bt only
            list.Remove(_Enums.Principal.all.ToString());
            list.Remove(_Enums.Principal.z2.ToString());

            (sender as RadioButtonList).DataSource = list;
        }

        protected void listControl_DataBound(object sender, EventArgs e)
        {
            ListControl lct = (ListControl)sender;
            if (lct.SelectedIndex == -1 && lct.Items.Count > 0)
                lct.SelectedIndex = 0;
        }

        protected void rdoPrincipal_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlVenue.DataBind();
            btnAdd.DataBind();
        }

        protected void SqlVenue_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            string prince = Request.Form["ctl00$MainContent$ctl01$rdoPrincipal"] ?? "fox";
            
            e.Command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@principal", prince));

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("DECLARE  @DefaultVenue varchar(256) ");
            sb.Append("SET		@DefaultVenue = ");
            sb.Append("         CASE ");
            sb.Append("             WHEN @principal = 'fox' THEN 'The Fox Theatre' ");
            sb.Append("             WHEN @principal = 'bt' THEN 'The Boulder Theater' ");
            sb.Append("             ELSE '' ");
            sb.Append("         END ");

            sb.Append("SELECT   v.[Id], v.[Name], ");
            sb.Append("CASE WHEN v.[Name] = @DefaultVenue THEN '' ELSE v.[Name] END AS [VenueRank]	");
            sb.Append("FROM [Venue] v ");
            //leave the all comparison even though w are not using here
            sb.Append("WHERE (CASE WHEN @principal = 'all' THEN 1 WHEN CHARINDEX(@principal, v.[vcPrincipal]) >= 1 THEN 1 ELSE 0 END = 1) ");
            sb.Append("ORDER BY [VenueRank] ");

            e.Command.CommandText = sb.ToString();
        }

        protected void ddlVenue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            btnAdd.DataBind();
        }

        protected void ddlAges_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.Items.Count == 0)
                ddl.DataSource = _Lookits.Ages;

            ddl.DataTextField = "Name";
            ddl.DataValueField = "Id";
        }

        protected void ddlAges_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (!IsPostBack && ddl.Items.Count > 0)
            {
                int sel = _Lookits.Ages.GetList().FindIndex(delegate(Age match) { return (match.Name.ToLower() == _Config._Default_Age.Name.ToLower()); });
                ddl.SelectedIndex = sel;
            }
        }

        #endregion

}
}
