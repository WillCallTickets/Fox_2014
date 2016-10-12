using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Wcss;
using wctMain.Controller;
using WctControls.WebControls;

namespace wctMain.Admin._shows._controls
{
    public partial class ShowPromoters : wctMain.Controller.AdminBase.ShowEditBase, IPostBackEventHandler
    {
        protected override FormView mainForm { get { return null; } }

        //handles description update from iframe
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split('~');
            string command = args[0];

            switch (command.ToLower())
            {
                case "rebind":
                    //FormView1.DataBind();
                    break;
                case "rebindpostorder":
                    listEnt.DataBind();
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentShowRecord == null)
                base.Redirect("/Admin/_shows/ShowDirector.aspx");

            if (!IsPostBack)
            {
                hdnCollectionTableName.DataBind();
                Atx.CurrentPromoterId = 0;
                FormView1.DataBind();
            }
        }

        #region Entity Listing

        protected int listItemNum = 0;

        protected void SqlShowPromoters_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@ShowId"].Value = Atx.CurrentShowRecord.Id;
        }

        protected void listEnt_DataBinding(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            listItemNum = 0;
        }

        protected void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView list = (ListView)sender;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            JShowPromoter ent = Atx.CurrentShowRecord.JShowPromoterRecords().GetList()
                .Find(delegate(JShowPromoter match) { return (match.Id == (int)drv.Row.ItemArray.GetValue(drv.Row.Table.Columns.IndexOf("Id"))); });
            
            LinkButton linkEdit = (LinkButton)e.Item.FindControl("linkEdit");
            if (linkEdit != null)
                linkEdit.Text = string.Format("{0}", listItemNum++.ToString());

            Literal litInfo = (Literal)e.Item.FindControl("litInfo");
            
            if (litInfo != null && ent != null)
                litInfo.Text = string.Format("{0}{1}", 
                    (Atx.IsSuperSession(this.Page.User)) ? string.Format("{0} - ", ent.Id.ToString()) : string.Empty, ent.ToString());
        }
        
        protected void listEnt_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "new":
                    ShowNewInterface((ListView)sender);
                    break;
            }
        }

        protected void listEnt_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListView list = (ListView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)list.EditItem.FindControl("ErrorDisplayLabel1");
            error.ResetErrors();

            JShowPromoter ent = Atx.CurrentShowRecord.JShowPromoterRecords().GetList()
                .Find(delegate(JShowPromoter match) { return (match.Id == (int)e.Keys["Id"]); });

            try
            {
                ent.PreText = (string)e.NewValues["PreText"];
                ent.PromoterText = (string)e.NewValues["PromoterText"];
                ent.PostText = (string)e.NewValues["PostText"];

                if (ent.IsDirty)
                    ent.Save();

                //reset here because name edits need to reflected regardless of other inputs
                Atx.ResetCurrentShowRecord();

                e.Cancel = false;
                list.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                error.ErrorList.Add(ex.Message);
                error.DisplayErrors();

                e.Cancel = true;
            }
        }

        protected void listEnt_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListView list = (ListView)sender;
            int idx = (int)e.Keys["Id"];
            Atx.CurrentShowRecord.JShowPromoterRecords().DeleteFromCollection(idx);
                
            Atx.CurrentPromoterId = 0;
            e.Cancel = false;//let object datasource ignore operation

            Atx.ResetCurrentShowRecord();
            list.DataBind();
        }

        protected void listEnt_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView list = (ListView)sender;
            SetListEditItem(list, e.NewEditIndex);
        }

        protected void listEnt_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListView list = (ListView)sender;
            Atx.CurrentPromoterId = 0;
            list.EditIndex = -1;
            list.InsertItemPosition = InsertItemPosition.None;
            list.DataBind();
        }

        protected void SetListEditItem(ListView list, int idx)
        {
            list.InsertItemPosition = InsertItemPosition.None;
            list.EditIndex = idx;
        }

        protected void ShowNewInterface(ListView list)
        {
            list.EditIndex = -1;
            list.InsertItemPosition = InsertItemPosition.LastItem;
            list.DataBind();
        }

        #endregion

        #region FormView 

        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)form.FindControl("ErrorDisplayLabel1");
            error.ResetErrors();

            wctMain.Admin._editors._controls.Editor_Promoter ei = (wctMain.Admin._editors._controls.Editor_Promoter)form.FindControl("Editor_Promoteri");

            try
            {
                int idx = ei.SelectedIdx;

                //Validation
                if (idx <= 0)
                    error.ErrorList.Add("Please select a promoter or create a new one.");

                if (error.ErrorList.Count > 0)
                {
                    error.DisplayErrors();
                    e.Cancel = true;
                    return;
                }

                Atx.CurrentShowRecord.JShowPromoterRecords().AddPromoterToCollection(Atx.CurrentShowRecord.Id, idx);

                //ent.Save();
                Atx.ResetCurrentShowRecord();
                Atx.CurrentPromoterId = 0;

                //same as cancel
                e.Cancel = false;
                //form.DataSource = null;
                form.ChangeMode(FormViewMode.Edit);
                form.DataBind();

                listEnt.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                error.ErrorList.Add(ex.Message);
                error.DisplayErrors();

                e.Cancel = true;
            }
        }

        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)//handles cancel correctly
                form.DataBind();
        }

        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            string cmd = e.CommandName.ToLower().Trim();

            switch (cmd)
            {
                case "cancel":
                    Atx.CurrentPromoterId = 0;
                    break;
            }
        }

        #endregion
    }
}//560 11/8/14 11am - 289 11/10/14 9:30am