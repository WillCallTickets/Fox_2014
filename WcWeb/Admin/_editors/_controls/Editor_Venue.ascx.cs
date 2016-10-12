using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;

using Newtonsoft.Json;

using Wcss;
using wctMain.Controller;
using WctControls.WebControls;

namespace wctMain.Admin._editors._controls
{
    /// <summary>
    /// if we are in the editor mode - we will show the tune list. can only edit picture in editor mode
    /// </summary>
    [ToolboxData("<{0}:Editor_Venue runat='Server' SelectedIdx='' HideSelectionPanel='false' HideDisplayWebsiteDelete='false' ></{0}:Editor_Venue>")]
    public partial class Editor_Venue : MainBaseControl, IPostBackEventHandler
    {
        #region PostBack

        public void RaisePostBackEvent(string eventArgument)
        {
            Dictionary<string, string> args = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventArgument);
            string commandName = args["commandName"].ToLower();

            switch (commandName)
            {
                case "selectedidfromtypeahead":
                    ReactToEntitySelection(args["newIdx"]);
                    break;
            }
        }

        #endregion
        
        #region Ajax selection

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            ReactToEntitySelection(hidSelectedValue.Value);
        }

        private void ReactToEntitySelection(string idx)
        {
            if (Utils.Validation.IsInteger(idx))
                Atx.CurrentVenueId = int.Parse(idx);
            else
                Atx.CurrentVenueId = 0;

            search_venue.Text = string.Empty;
            hidSelectedValue.Value = "0";

            FormView1.ChangeMode(FormViewMode.ReadOnly);
            FormView1.DataBind();
        }

        #endregion

        #region Properties

        private bool _hideSelectionPanel;
        public bool HideSelectionPanel
        {
            get { return _hideSelectionPanel; }
            set
            {
                _hideSelectionPanel = value;
            }
        }

        private bool _hideDisplayWebsiteDelete;
        public bool HideDisplayWebsiteDelete
        {
            get { return _hideDisplayWebsiteDelete; }
            set
            {
                _hideDisplayWebsiteDelete = value;
            }
        }

        //save the id and name of the current entity into object state
        public int SelectedIdx
        {
            get
            {
                return Atx.CurrentVenueId;
            }
            set
            {
                Atx.CurrentVenueId = value;
            }
        }

        protected Venue _venue = null;
        protected Venue Entity
        {
            get
            {
                if (_venue == null && SelectedIdx > 0 || (_venue != null && _venue.Id != SelectedIdx))
                {
                    if (SelectedIdx == 0)
                        _venue = null;
                    else
                    {
                        _venue = Venue.FetchByID(SelectedIdx);

                        if (_venue != null && _venue.ApplicationId != _Config.APPLICATION_ID)
                        {
                            SelectedIdx = 0;
                            _venue = null;
                        }
                    }
                }

                return _venue;
            }
        }
        
        public string SelectedName
        {
            get
            {
                TextBox txt = (TextBox)FormView1.FindControl("NameTextBox");
                return (txt != null) ? txt.Text.Trim() : string.Empty;
            }
        }

        public bool _NameMatchesDisplayName()
        {
            //only if we have an entity selected
            return (Entity != null && Entity.Id > 0 && Entity.Name == Entity.Name_Displayable); 
        }

        #endregion

        #region Page Overhead

        protected override object SaveControlState()
        {
            object[] ctlState = new object[5];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = this._hideSelectionPanel;
            ctlState[2] = this._hideDisplayWebsiteDelete;
            return ctlState;
        }
        
        protected override void LoadControlState(object savedState)
        {
            if (savedState == null)
                return;
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this._hideSelectionPanel = (bool)ctlState[1];
            this._hideDisplayWebsiteDelete = (bool)ctlState[2];
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.RegisterRequiresControlState(this);

            search_venue.Attributes.Add("autocomplete", "off");         
            search_venue.Attributes.Add("data-searchtype", "Venue");
            search_venue.Attributes.Add("placeholder", "Search Venues");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        #endregion

        #region FormView

        protected void WebsiteTextBox_TextChanged(object sender, EventArgs e)
        {
            HyperLink test = (HyperLink)FormView1.FindControl("linkTestWebsite");
            if (test != null)
                test.DataBind();
        }

        protected void linkTestWebsite_DataBinding(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)FormView1.FindControl("WebsiteTextBox");

            if (txt != null)
            {
                string input = txt.Text.Trim();

                //update the test link button
                HyperLink test = (HyperLink)sender;

                if (Utils.Validation.IsValidUrl(input))
                    test.NavigateUrl = Utils.ParseHelper.FormatUrlFromString(input, true, false);
            }
        }

        //data is bound by a sqldataobject - the key is the admin cookie for currentId - acid, prid, vnid
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView view = (FormView)sender;

            pnlSelect.Visible = ((!_hideSelectionPanel) && (view.CurrentMode == FormViewMode.ReadOnly || (view.CurrentMode == FormViewMode.Edit && Atx.CurrentVenueId == 0)));

            if (view.DataItem != null)
            {
                Literal img = (Literal)view.FindControl("litImgThumb");

                if (img != null && Entity != null)
                    img.DataBind();

                RegularExpressionValidator regexWeb = (RegularExpressionValidator)view.FindControl("regexWebsite");
                if (regexWeb != null)
                    regexWeb.ValidationExpression = Utils.Validation.regexUrl.ToString();

                HyperLink btnWeb = (HyperLink)view.FindControl("btnWebsiteUrl");
                if (btnWeb != null && Entity != null)
                {
                    if (btnWeb.Visible)
                        btnWeb.NavigateUrl = Utils.ParseHelper.FormatUrlFromString(Entity.WebsiteUrl, true, false);
                    else
                        btnWeb.NavigateUrl = string.Empty;
                }
            }
        }

        protected void FormView1_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)form.FindControl("ErrorDisplayLabel1");
            error.ResetErrors();

            try
            {
                Venue.Delete((int)e.Keys["Id"]);

                if (Entity != null)
                {
                    if (Entity.ImageManager != null)
                        Entity.ImageManager.Delete();

                    Entity.PictureUrl = null;
                }

                Atx.CurrentVenueId = 0;
                Atx.CurrentVenueRecord = null;
                form.DataBind();
            }
            catch (Exception ex)
            {
                error.ErrorList.Add(((ex.Message.IndexOf("The DELETE statement conflicted with the REFERENCE constraint ") != -1) ?
                    "This venue is tied into existing shows and cannot be deleted." : ex.Message));
                error.DisplayErrors();
                e.Cancel = true;
            }
        }
        
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)form.FindControl("ErrorDisplayLabel1");
            
            error.ResetErrors();

            //validate input per rfc
            string nameInput = e.Values["Name"].ToString();

            //dont even bother showing a msg if no input
            if (nameInput.Trim().Length == 0)
            {
                e.Cancel = true;
                return;
            }

            if (!Utils.UrlHelper.IsAllowableAlphaOrderableName(nameInput))
                error.ErrorList.Add("The Name you have entered contains invalid chars. Use Display Name to display these chars.");

            //display any errors and return 
            if (error.ErrorList.Count > 0)
            {
                error.DisplayErrors();
                e.Cancel = true;
                return;
            }

            //convert all whitespace (incl \r\n\v\t\f to single space char
            nameInput = Utils.ParseHelper.ReplaceExtraWhitespaces(nameInput);

            //make sure input is uppercase
            e.Values["Name"] = nameInput.ToUpper();

            //get principal
            RadioButtonList rdoPrincipal = (RadioButtonList)form.FindControl("rdoPrincipal");
            e.Values["vcPrincipal"] = rdoPrincipal.SelectedValue;

            e.Values["@appId"] = Wcss._Config.APPLICATION_ID;
        }

        protected void SqlDetails_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }

        protected void SqlDetails_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Command.Parameters["@NewId"] != null && Utils.Validation.IsInteger(e.Command.Parameters["@NewId"].Value.ToString()))
            {
                SelectedIdx = (int)e.Command.Parameters["@NewId"].Value;
            }
            else
                SelectedIdx = 0;
        }

        protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)form.FindControl("ErrorDisplayLabel1");
            error.ResetErrors();

            //there is a certain exception that we want to handle
            //and that is when we are trying to add an existing entity - in this case we would like
            //to just select the entity that was being inserted
            if(e.Exception != null && e.Exception.Message.ToLower().IndexOf("cannot insert duplicate key row in object") != -1)
            {
                //get the entity and assign to the editor
                Venue a = new Venue();
                a.LoadAndCloseReader(Venue.FetchByParameter("NameRoot", e.Values["Name"].ToString()));

                //if found
                if (a.Id > 0)
                {
                    SelectedIdx = a.Id;
                    e.ExceptionHandled = true;
                    e.KeepInInsertMode = false;

                    return;
                }
                //otherwise - show the error
            }
            else if (e.Exception != null && e.Exception.Message.ToLower().IndexOf("cannot insert duplicate key row in object") == -1)
            {
                error.ErrorList.Add(e.Exception.Message);
                error.DisplayErrors();
                e.KeepInInsertMode = true;
                e.ExceptionHandled = true;
            }
            else
            {
                e.KeepInInsertMode = false;
            }
        }

        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)form.FindControl("ErrorDisplayLabel1");
            error.ResetErrors();

            try
            {
                //make sure input is uppercase
                string nameInput = e.NewValues["Name"].ToString();

                if (!Utils.UrlHelper.IsAllowableAlphaOrderableName(nameInput))
                    error.ErrorList.Add("The Name you have entered contains invalid chars. Use Display Name to display these chars.");

                string displayName = (e.NewValues["DisplayName"] != null && e.NewValues["DisplayName"].ToString().Trim().Length > 0)
                    ? e.NewValues["DisplayName"].ToString().Trim() : null;

                string website = (e.NewValues["Website"] != null && e.NewValues["Website"].ToString().Trim().Length > 0)
                    ? e.NewValues["Website"].ToString().Trim().ToLower() : null;

                if (website != null && (!Utils.Validation.IsValidUrl(website)))
                    error.ErrorList.Add("The website you have entered is not a valid url.");

                int capacity = 0;
                string cap = (e.NewValues["iCapacity"] != null && e.NewValues["iCapacity"].ToString().Trim().Length > 0)
                    ? e.NewValues["iCapacity"].ToString().Trim() : null;
                if (cap != null && (!int.TryParse(cap, out capacity)))
                    error.ErrorList.Add("Capacity must be an integer.");
                
                //display any errors and return 
                if (error.ErrorList.Count > 0)
                {
                    error.DisplayErrors();
                    e.Cancel = true;
                    return;
                }

                //convert all whitespace (incl \r\n\v\t\f to single space char
                nameInput = Utils.ParseHelper.ReplaceExtraWhitespaces(nameInput);
                e.NewValues["Name"] = nameInput.ToUpper();
                e.NewValues["DisplayName"] = (nameInput != displayName) ? displayName : string.Empty;
                e.NewValues["Website"] = website;
                e.NewValues["iCapacity"] = capacity;

                e.Cancel = false;
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                error.ErrorList.Add(ex.Message);
                error.DisplayErrors();

                e.Cancel = true;
            }
        }

        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)FormView1.FindControl("ErrorDisplayLabel1");
            error.ResetErrors();

            if (e.Exception != null)
            {
                string ec = e.Exception.Message;
                if (ec.IndexOf("Cannot insert duplicate key row") != -1)
                    ec = "The entry you have made already exists.";

                error.ErrorList.Add(ec);
                error.DisplayErrors();

                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
            }
            else {
                e.KeepInEditMode = false;
            }
        }
        
        protected void SqlDetails_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }

        #endregion

        protected void FormView1_ModeChanged(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)FormView1.FindControl("ErrorDisplayLabel1");
            if(error != null)
                error.ResetErrors();
            form.DataBind();
        }

        protected void btnInlineNew_Click(object sender, EventArgs e)
        {
            FormView1.ChangeMode(FormViewMode.Insert);
        }

        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            string cmd = e.CommandName.ToLower().Trim();

            switch (cmd)
            {
                case "cancel":
                    if (form.CurrentMode == FormViewMode.Edit || form.CurrentMode == FormViewMode.Insert)
                    {
                        form.ChangeMode(FormViewMode.ReadOnly);
                    }
                    break;
            }
        }

        #region Insert Template methods

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

        #endregion

        
}
}