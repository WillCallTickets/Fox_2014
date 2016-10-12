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
    [ToolboxData("<{0}:Editor_Act runat='Server' SelectedIdx='' HideSelectionPanel='false' HideDisplayWebsiteDelete='false' ></{0}:Editor_Act>")]
    public partial class Editor_Act : MainBaseControl, IPostBackEventHandler
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
                Atx.CurrentActId = int.Parse(idx);
            else
                Atx.CurrentActId = 0;

            OnSelectedActChanged(new AdminEvent.EditorEntityChangedEventArgs(Atx.CurrentActId, ""));

            search_act.Text = string.Empty;
            hidSelectedValue.Value = "0";

            FormView1.ChangeMode(FormViewMode.ReadOnly);
            FormView1.DataBind();
        }

        private static readonly object SelectedEditActChangedEventKey = new object();

        public delegate void SelectedEditActChangedEventHandler(object sender, wctMain.Admin.AdminEvent.EditorEntityChangedEventArgs e);

        public event SelectedEditActChangedEventHandler SelectedActChanged
        {
            add { Events.AddHandler(SelectedEditActChangedEventKey, value); }
            remove { Events.RemoveHandler(SelectedEditActChangedEventKey, value); }
        }

        public virtual void OnSelectedActChanged(wctMain.Admin.AdminEvent.EditorEntityChangedEventArgs e)
        {
            SelectedEditActChangedEventHandler handler = (SelectedEditActChangedEventHandler)Events[SelectedEditActChangedEventKey];

            if (handler != null)
                handler(this, e);
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

        //save the id and name of the current act into object state
        public int SelectedIdx
        {
            get
            {
                return Atx.CurrentActId;
            }
            set
            {
                Atx.CurrentActId = value;
            }
        }

        protected Act _act = null;
        protected Act Entity
        {
            get
            {
                if (_act == null && SelectedIdx > 0 || (_act != null && _act.Id != SelectedIdx))
                {
                    if (SelectedIdx == 0)
                        _act = null;
                    else
                    {
                        _act = Act.FetchByID(SelectedIdx);

                        if (_act != null && _act.ApplicationId != _Config.APPLICATION_ID)
                        {
                            SelectedIdx = 0;
                            _act = null;
                        }
                    }
                }

                return _act;
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
            //only if we have an act selected
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

            search_act.Attributes.Add("autocomplete", "off");         
            search_act.Attributes.Add("data-searchtype", "Act");
            search_act.Attributes.Add("placeholder", "Search Acts");
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

        //data is bound by a sqldataobject - the key is the admin cookie for currentActId - acid
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView view = (FormView)sender;

            pnlSelect.Visible = ((!_hideSelectionPanel) && (view.CurrentMode == FormViewMode.ReadOnly || (view.CurrentMode == FormViewMode.Edit && Atx.CurrentActId == 0)));

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
                        btnWeb.NavigateUrl = Utils.ParseHelper.FormatUrlFromString(Entity.Website, true, false);
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
                Act.Delete((int)e.Keys["Id"]);

                if (Entity != null)
                {
                    if (Entity.ImageManager != null)
                        Entity.ImageManager.Delete();

                    Entity.PictureUrl = null;
                }

                Atx.CurrentActId = 0;
                Atx.CurrentActRecord = null;
                form.DataBind();
            }
            catch (Exception ex)
            {
                error.ErrorList.Add(((ex.Message.IndexOf("The DELETE statement conflicted with the REFERENCE constraint ") != -1) ?
                    "This act is tied into existing shows and cannot be deleted." : ex.Message));
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

            //convert all whitespace (incl \r\n\v\t\f) to single space char
            nameInput = Utils.ParseHelper.ReplaceExtraWhitespaces(nameInput);

            //make sure input is uppercase
            e.Values["Name"] = nameInput.ToUpper();
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
                //OnSelectedActChanged(new AdminEvent.EditorEntityChangedEventArgs(SelectedIdx, ""));
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
            //and that is when we are trying to add an existing act - in this case we would like
            //to just select the act that was being inserted
            if(e.Exception != null && e.Exception.Message.ToLower().IndexOf("cannot insert duplicate key row in object") != -1)
            {
                //get the act
                //assign to the editor
                Act a = new Act();
                a.LoadAndCloseReader(Act.FetchByParameter("NameRoot", e.Values["Name"].ToString()));

                //if found
                if (a.Id > 0)
                {
                    SelectedIdx = a.Id;
                    e.ExceptionHandled = true;
                    e.KeepInInsertMode = false;

                    //OnSelectedActChanged(new AdminEvent.EditorEntityChangedEventArgs(a.Id, ""));

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
            //    form.ChangeMode(FormViewMode.Edit);

            }
        }

        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            ErrorDisplayLabel error = (ErrorDisplayLabel)((FormView)sender).FindControl("ErrorDisplayLabel1");
            error.ResetErrors();

            string nameInput = (e.NewValues["Name"] != null) ? e.NewValues["Name"].ToString() : string.Empty;

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

            //convert all whitespace (incl \r\n\v\t\f) to single space char
            nameInput = Utils.ParseHelper.ReplaceExtraWhitespaces(nameInput);

            //make sure input is uppercase
            e.NewValues["Name"] = nameInput.ToUpper();
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

            //Unfortunaly this will not work easy due to page flow - for some other time
            //string input = search_act.Text.Trim();
            //TextBox txt = (TextBox)FormView1.FindControl("NameTextBox");
            //if (txt != null)
            //    txt.Text = input;
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

        
}
}