using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Wcss;
using wctMain.Controller;
using WctControls.WebControls;

/*
 *   <!-- form group templates

                            <div class="form-group">	
                                <div class="input-group">
	                                <span class="input-group-addon">

	                                </span>

                                </div>
                            </div>

                            <div class="form-group">
	                            <div class="form-group col-xs-6" style="padding-left:0;">
	                                <div class="input-group">
		                            <span class="input-group-addon">

		                            </span>

	                                </div>
	                            </div>
	                            <div class="form-group col-xs-6" style="padding-right:0;">
	                                <div class="input-group">
		                            <span class="input-group-addon">

		                            </span>

	                                </div>
	                            </div>
                            </div>


                            -->
 * */
namespace wctMain.Admin._shows._controls
{
    //this page does not allow inserts - that must be done from show picker
    public partial class ShowDetails : wctMain.Controller.AdminBase.ShowEditBase, IPostBackEventHandler
    {
        protected ShowDate firstShowDate = null;
        protected override FormView mainForm { get { return this.FormView1; } }

        //handles description update from iframe
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split('~');
            string command = args[0];

            switch (command.ToLower())
            {
                case "reload":
                    
                    Show s = new Show(int.Parse(args[1]));
                    Atx.ShowRepo_Web.Add(s);
                    Atx.SetCurrentShowRecord(s.Id);
                    
                    //redo the picker
                    Control chooser = (Control)this.Parent.Parent.FindControl("ShowSelector1");
                    if(chooser != null)
                        chooser.DataBind();

                    FormView1.DataBind();
                    break;
                case "rebind":
                    FormView1.DataBind();
                    break;                
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentShowRecord == null)
                base.Redirect("/Admin/_shows/ShowDirector.aspx");

            firstShowDate = Atx.CurrentShowRecord.FirstShowDate;

            if (!IsPostBack)
            {
                string link = string.Format("return confirm('Are you sure you want to delete {0}?')", 
                    Utils.ParseHelper.ParseJsAlert(Atx.CurrentShowRecord.Name));
                btnDelete.OnClientClick = link;

                FormView1.DataBind();
            }
        }

        protected void ddlSaleStatus_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl != null)
            {
                if (ddl.Items.Count == 0)
                {
                    ddl.DataSource = _Lookits.ShowStatii;
                }

                ddl.DataTextField = "Name";
                ddl.DataValueField = "Id";
            }
        }

        protected void ddlSaleStatus_OnDataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl != null)
            {
                ddl.SelectedIndex = -1;

                if (firstShowDate != null && firstShowDate.Id > 0)
                    ddl.Items.FindByValue(firstShowDate.TStatusId.ToString()).Selected = true;
                else
                    ddl.Items.FindByText("OnSale").Selected = true;
            }
        }

        #region Details

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            ShowCollection coll = new ShowCollection();
            coll.Add(Atx.CurrentShowRecord);

            form.DataSource = coll;
            string[] keyNames = { "Id", "Name" };
            form.DataKeyNames = keyNames;
        }

        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Show show = Atx.CurrentShowRecord;

            Literal litFB = (Literal)form.FindControl("litFB");
            if (litFB != null)
                litFB.DataBind();
            
            //explain where the image is from 
            //from headline - from show
            if (form.CurrentMode == FormViewMode.Edit)
            {
                Admin._customControls.WctCkEditor wct = (Admin._customControls.WctCkEditor)form.FindControl("WctCkEditor1");

                if (wct != null)
                {
                    //lives outside of form at top of page
                    btnUpdate.OnClientClick = wct.ClientClickMethodCall;
                    btnFormUpdate.OnClientClick = wct.ClientClickMethodCall;
                }

                CheckBox chkLate = (CheckBox)form.FindControl("chkLate");
                if(chkLate != null)
                    chkLate.Checked = firstShowDate.IsLateNightShow;

                DropDownList ddlAges = (DropDownList)form.FindControl("ddlAges");

                if (ddlAges != null)
                {
                    if (ddlAges.Items.Count == 0)
                        ddlAges.DataSource = _Lookits.Ages;

                    ddlAges.DataTextField = "Name";
                    ddlAges.DataValueField = "Id";
                    ddlAges.DataBind();

                    ddlAges.SelectedIndex = -1;
                    if (firstShowDate != null && firstShowDate.Id > 0)
                        ddlAges.Items.FindByValue(firstShowDate.TAgeId.ToString()).Selected = true;
                }

                TextBox url = (TextBox)form.FindControl("txtUrl");
                if (url != null)
                {
                    url.Text = firstShowDate.TicketUrl;

                    HyperLink test = (HyperLink)FormView1.FindControl("linkTestWebsite");
                    if (test != null)
                        test.DataBind();
                }

                TextBox facebookEventUrl = (TextBox)form.FindControl("txtFacebookEventUrl");
                if (facebookEventUrl != null)
                {
                    facebookEventUrl.Text = show.FacebookEventUrl ?? string.Empty;

                    HyperLink testFacebook = (HyperLink)FormView1.FindControl("linkTestFacebookEvent");
                    if (testFacebook != null)
                        testFacebook.DataBind();
                }
            }
        }

        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            Show show = Atx.CurrentShowRecord;
            ErrorDisplayLabel error = (ErrorDisplayLabel)form.FindControl("ErrorDisplayLabel1");
            error.ResetErrors();

            if (show != null)
            {
                try
                {
                    #region Validate input

                    DateTime dos = DateTime.MinValue;
                    DateTime announce = DateTime.MinValue;
                    DateTime onsale = DateTime.MinValue;

                    //start - dos
                    string iDos = e.NewValues["FirstShowDate.DtDateOfShow"].ToString();
                    if (iDos.Length == 0 || iDos == DateTime.MinValue.ToString())
                        error.ErrorList.Add("Show Date is required.");
                    else
                    {
                        if (!DateTime.TryParse(iDos, out dos))
                            error.ErrorList.Add("Show Date is not valid.");

                        if (dos.Year < 1992)
                            error.ErrorList.Add("Show Date is not valid.");
                    }

                    string iAnn = e.NewValues["AnnounceDate"].ToString();
                    //announce - min value is ignored
                    if (iAnn.Length > 0)
                    {
                        if (!DateTime.TryParse(iAnn, out announce))
                            error.ErrorList.Add("Announce Date is not valid.");
                    }

                    //onsale - min value is ignored
                    string iOnsale = e.NewValues["DateOnSale"].ToString();
                    if (iOnsale.Length > 0)
                    {
                        if (!DateTime.TryParse(iOnsale, out onsale))
                            error.ErrorList.Add("OnSale Date is not valid.");
                    }

                    TextBox txtUrl = (TextBox)form.FindControl("txtUrl");
                    string ticketUrl = txtUrl.Text.Trim();
                    if (ticketUrl.Trim().Length > 0 && (!Utils.Validation.IsValidUrl(ticketUrl)))
                    {
                        error.ErrorList.Add("Ticket Url is not a valid url.");
                    }

                    TextBox txtFacebookEventUrl = (TextBox)form.FindControl("txtFacebookEventUrl");
                    string fbUrl = txtFacebookEventUrl.Text.Trim();
                    if (fbUrl.Trim().Length > 0 && (!Utils.Validation.IsValidUrl(fbUrl)))
                    {
                        error.ErrorList.Add("Facebook Event Url is not a valid url.");
                    }
                    
                    //display any errors and return 
                    if (error.ErrorList.Count > 0)
                    {
                        error.DisplayErrors();
                        return;
                    }

                    #endregion

                    bool syncshowname = false;

                    if (dos != firstShowDate.DateOfShow)
                    {
                        firstShowDate.DateOfShow = dos;
                        syncshowname = true;
                    }

                    if (announce != show.AnnounceDate)
                        show.AnnounceDate = announce;

                    if (onsale != show.DateOnSale)
                        show.DateOnSale = onsale;


                    //show time
                    string showtime = ((BootstrapDateTimePicker)form.FindControl("txtShowTime")).Text.Trim();
                    if (firstShowDate.ShowTime != showtime)
                        firstShowDate.ShowTime = showtime;
                        
                    CheckBox chkActive = (CheckBox)form.FindControl("chkActive");
                    if (chkActive != null && show.IsActive != chkActive.Checked)
                        show.IsActive = chkActive.Checked;

                    CheckBox chkLate = (CheckBox)form.FindControl("chkLate");
                    if(show.FirstShowDate.IsLateNightShow != chkLate.Checked)
                        firstShowDate.IsLateNightShow = chkLate.Checked;

                    RadioButtonList rdoJust = (RadioButtonList)form.FindControl("rdoJust");
                    if (show.JustAnnouncedStatus.ToString().ToLower() != rdoJust.SelectedValue.ToLower())
                        show.JustAnnouncedStatus = (_Enums.JustAnnouncedStatus)Enum.Parse(typeof(_Enums.JustAnnouncedStatus), rdoJust.SelectedValue, true);

                    DropDownList ddlAges = (DropDownList)form.FindControl("ddlAges");
                    if (firstShowDate.TAgeId != int.Parse(ddlAges.SelectedValue))
                        firstShowDate.TAgeId = int.Parse(ddlAges.SelectedValue);
                    
                    DropDownList ddlSaleStatus = (DropDownList)form.FindControl("ddlSaleStatus");
                    if (ddlSaleStatus != null && firstShowDate.TStatusId != int.Parse(ddlSaleStatus.SelectedValue))
                    {
                        //adjust parent item
                        if (ddlSaleStatus.SelectedItem.Text != Wcss._Enums.ShowDateStatus.SoldOut.ToString() && (Atx.CurrentShowRecord.IsSoldOut))
                            Atx.CurrentShowRecord.IsSoldOut = false;
                        else if (ddlSaleStatus.SelectedItem.Text == Wcss._Enums.ShowDateStatus.SoldOut.ToString() && (!Atx.CurrentShowRecord.IsSoldOut))
                            Atx.CurrentShowRecord.IsSoldOut = true;

                        firstShowDate.TStatusId = int.Parse(ddlSaleStatus.SelectedValue);
                    }

                    TextBox txtPricing = (TextBox)form.FindControl("txtPricing");
                    if (txtPricing != null && firstShowDate.PricingText != txtPricing.Text.Trim())
                        firstShowDate.PricingText = txtPricing.Text.Trim();

                    if (firstShowDate.TicketUrl != ticketUrl)
                        firstShowDate.TicketUrl = ticketUrl;

                    if (show.FacebookEventUrl != fbUrl)
                        show.FacebookEventUrl = fbUrl;

                    TextBox txtBilling = (TextBox)form.FindControl("txtBilling");
                    if (txtBilling != null && firstShowDate.MenuBilling != txtBilling.Text.Trim())
                        firstShowDate.MenuBilling = txtBilling.Text.Trim();

                    //alert
                    TextBox txtAlert = (TextBox)form.FindControl("txtAlert");
                    if (txtAlert != null && show.ShowAlert != txtAlert.Text.Trim())
                        show.ShowAlert = txtAlert.Text.Trim();

                    TextBox txtTitle = (TextBox)form.FindControl("txtTitle");
                    if (show.ShowTitle != txtTitle.Text.Trim())             
                        show.ShowTitle = txtTitle.Text.Trim();

                    TextBox txtDisplayNotes = (TextBox)form.FindControl("txtDisplayNotes");
                    if (txtDisplayNotes != null && show.DisplayNotes != txtDisplayNotes.Text.Trim())
                        show.DisplayNotes = txtDisplayNotes.Text.Trim();

                    //Admin._customControls.WctCkEditor wct = (Admin._customControls.WctCkEditor)form.FindControl("WctCkEditor1");
                    //show.ShowWriteup = wct.CkEditorValue;


                    if (firstShowDate.IsDirty)
                    {
                        firstShowDate.Save();
                    }
                    if (syncshowname == true)
                    {
                        ChangeShowName();
                    }
                    show.Save();

                    //ensure Show has latest data
                    Atx.ResetCurrentShowRecord();

                    e.Cancel = false;

                    form.DataBind();
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    error.ErrorList.Add(ex.Message);
                    error.DisplayErrors();
                    e.Cancel = true;
                }
            }
        }

        protected void FormView1_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            FormView form = (FormView)sender;
            Show show = Atx.CurrentShowRecord;
            ErrorDisplayLabel error = (ErrorDisplayLabel)form.FindControl("ErrorDisplayLabel1");
            error.ResetErrors();

            if (show != null)
            {
                try
                {
                    //TODO
                    //see if show has any sales
                    ShowDateCollection coll = new ShowDateCollection();
                    coll.AddRange(Atx.CurrentShowRecord.ShowDateRecords());

                    while (coll.Count > 0)
                    {
                        Atx.CurrentShowRecord.DeleteShowDate(coll[0].Id);
                        coll.RemoveAt(0);
                    }

                    Show.Delete(Atx.CurrentShowRecord.Id);
                    Atx.SetCurrentShowRecord(0);

                    e.Cancel = false;

                    base.Redirect("/Admin/_shows/ShowDirector.aspx");
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    error.ErrorList.Add(ex.Message);
                    error.DisplayErrors();
                    e.Cancel = true;
                }
            }
        }

        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }

        protected void txtUrl_TextChanged(object sender, EventArgs e)
        {
            HyperLink test = (HyperLink)FormView1.FindControl("linkTestWebsite");
            if (test != null)
                test.DataBind();
        }

        protected void linkTestWebsite_DataBinding(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)FormView1.FindControl("txtUrl");

            if (txt != null)
            {
                string input = txt.Text.Trim();

                //update the test link button
                HyperLink test = (HyperLink)sender;
                
                test.Enabled = false;

                if (input.Trim().Length > 0 && Utils.Validation.IsValidUrl(input))
                {
                    test.NavigateUrl = Utils.ParseHelper.FormatUrlFromString(input, true, false);
                    test.Enabled = true;
                }
            }
        }

        protected void txtFacebookEventUrl_TextChanged(object sender, EventArgs e)
        {
            HyperLink test = (HyperLink)FormView1.FindControl("linkTestFacebookEvent");

            if (test != null)
                test.DataBind();
        }

        protected void linkTestFacebookEvent_DataBinding(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)FormView1.FindControl("txtFacebookEventUrl");

            if (txt != null)
            {
                string input = txt.Text.Trim();

                //update the test link button
                HyperLink test = (HyperLink)sender;

                test.Enabled = false;

                if (input.Trim().Length > 0 && Utils.Validation.IsValidUrl(input))
                {
                    test.NavigateUrl = Utils.ParseHelper.FormatUrlFromString(input, true, false);
                    test.Enabled = true;
                }
            }
        }

        #endregion

        protected string getActiveClass()
        {
            return (Atx.CurrentShowRecord != null && (!Atx.CurrentShowRecord.IsActive)) ? "show-not-active form-control" : "form-control";
        }

        protected void btnVenueEditor_Click(object sender, EventArgs e)
        {
            base.Redirect("/Admin/EntityEditor.aspx?p=venue");
        }
        
        /// <summary>
        /// deal with tba
        /// </summary>
        protected void txtShowTime_DataBinding(object sender, EventArgs e)
        {
            Show s = Atx.CurrentShowRecord;

            if (s != null)
            {
                BootstrapDateTimePicker pik = sender as BootstrapDateTimePicker;
                if (pik != null)
                {
                    string showTime = s.FirstShowDate.ShowTime ?? string.Empty;

                    CheckBox chkTba = (CheckBox)((FormView)pik.NamingContainer).FindControl("chkTba");
                    if (chkTba != null)
                        chkTba.Checked = (showTime.ToLower() == "tba");

                    pik.Text = (showTime.ToLower() != "tba") ? showTime : string.Empty;

                }
            }
        }

        /// <summary>
        /// fill radio with enum values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdoJust_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rdo = (RadioButtonList)sender;

            List<string> list = new List<string>(Enum.GetNames(typeof(_Enums.JustAnnouncedStatus)));

            rdo.DataSource = list;
        }

        /// <summary>
        /// set selected value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdoJust_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rdo = (RadioButtonList)sender;
                        
            ListItem li = rdo.Items.FindByText(Atx.CurrentShowRecord.JustAnnouncedStatus.ToString());
            if (li != null)
                li.Selected = true;
            else
                rdo.SelectedIndex = 0;
        }
}
}//454 - 141105 11am
