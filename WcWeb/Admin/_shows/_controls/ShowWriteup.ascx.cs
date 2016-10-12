using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Wcss;
using wctMain.Controller;
using WctControls.WebControls;

namespace wctMain.Admin._shows._controls
{
    //this page does not allow inserts - that must be done from show picker
    public partial class ShowWriteup : wctMain.Controller.AdminBase.ShowEditBase, IPostBackEventHandler
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
                    Control chooser = (Control)this.Parent.Parent.FindControl("ShowChooser1");
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

                    //LinkButton btnFormUpdate = (LinkButton)form.FindControl("btnFormUpdate");
                    //if (btnFormUpdate != null)
                    //    btnFormUpdate.OnClientClick = wct.ClientClickMethodCall;

                    string writeup = (Regex.Replace(show.ShowWriteup_Derived, "src=\"http://", "src=\"//", RegexOptions.IgnoreCase)).Trim();
                    wct.EditorTextBox.Text = writeup;
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

                    //display any errors and return 
                    if (error.ErrorList.Count > 0)
                    {
                        error.DisplayErrors();
                        return;
                    }

                    #endregion
                    
                    Admin._customControls.WctCkEditor wct = (Admin._customControls.WctCkEditor)form.FindControl("WctCkEditor1");
                    show.ShowWriteup = wct.CkEditorValue;

                    if (firstShowDate.IsDirty)
                    {
                        firstShowDate.Save();
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

        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }

        #endregion
        
}
}//454 - 141105 11am
