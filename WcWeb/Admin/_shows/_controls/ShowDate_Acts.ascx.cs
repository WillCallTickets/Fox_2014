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
    public partial class ShowDate_Acts : wctMain.Controller.AdminBase.ShowEditBase, IPostBackEventHandler
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
                Atx.CurrentActId = 0;
                rdoBilling.DataBind();
                FormView1.DataBind();
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ////ShowNewInterface(listEnt);
            //listEnt.EditIndex = -1;
            //listEnt.InsertItemPosition = InsertItemPosition.LastItem;

        }

        #region Radio Billing Method 

        protected void rdoBilling_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;

            //get selected date
            ShowDate sd = Atx.CurrentShowRecord.FirstShowDate;

            string dateBillingMethod = (sd == null || sd.IsAutoBilling) ? "Auto" : "Legacy";

            rbl.SelectedIndex = -1;
            ListItem li = rbl.Items.FindByValue(dateBillingMethod);
            
            if (li != null)
                li.Selected = true;
            else
                rbl.SelectedIndex = 0;//auto            
        }

        protected void rdoBilling_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            string selection = rbl.SelectedValue.ToLower();

            //change the showdates billing method            
            string sql = string.Format("UPDATE [ShowDate] SET [bAutoBilling] = {0} ", (selection == "auto") ? "1" : "0");
            sql += " WHERE [Id] = @dateIdx ";

            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@showIdx", Atx.CurrentShowRecord.Id, DbType.Int32);
            cmd.Parameters.Add("@dateIdx", Atx.CurrentShowRecord.FirstShowDate.Id, DbType.Int32);

            try
            {
                SubSonic.DataService.ExecuteQuery(cmd);

                //reset show data
                int index = Atx.CurrentShowRecord.Id;
                Atx.SetCurrentShowRecord(index);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            //ExplicitBind();
        }

        #endregion

        #region Act Listing

        protected int listItemNum = 0;

        protected void SqlShowActs_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@ShowDateId"].Value = Atx.CurrentShowRecord.FirstShowDate.Id;
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
            JShowAct ent = Atx.CurrentShowRecord.FirstShowDate.JShowActRecords().GetList()
                .Find(delegate(JShowAct match) { return (match.Id == (int)drv.Row.ItemArray.GetValue(drv.Row.Table.Columns.IndexOf("Id"))); });
            
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

            JShowAct ent = Atx.CurrentShowRecord.FirstShowDate.JShowActRecords().GetList()
                .Find(delegate(JShowAct match) { return (match.Id == (int)e.Keys["Id"]); });

            try
            {
                //there must be a least one headliner!
                //so if we are turning off headliner - check
                //semi-unecessary as the sort routine will pick first listed as head
                //bool oldHead = (bool)e.OldValues["TopBilling"];
                bool newHead = (bool)e.NewValues["TopBilling"];
                //if (oldHead && (!newHead))
                //{
                //    int occurrence = Atx.CurrentShowRecord.FirstShowDate.JShowActRecords().GetList()
                //        .FindIndex(delegate(JShowAct match) { return (match.Id != (int)e.Keys["Id"] && match.TopBilling); });
                //    if (occurrence == -1)
                //        error.ErrorList.Add("There must be at least one headliner specified for this show.");
                //}

                ////display any errors and return 
                //if (error.ErrorList.Count > 0)
                //{
                //    error.DisplayErrors();
                //    e.Cancel = true;
                //    return;
                //}

                ent.TopBilling = newHead;
                ent.PreText = (string)e.NewValues["PreText"];
                ent.ActText = (string)e.NewValues["ActText"];
                ent.Featuring = (string)e.NewValues["Featuring"];
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
            Atx.CurrentShowRecord.FirstShowDate.JShowActRecords().DeleteFromCollection(idx);
                
            Atx.CurrentActId = 0;
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
            Atx.CurrentActId = 0;
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


        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)form.FindControl("ErrorDisplayLabel1");
            error.ResetErrors();

            wctMain.Admin._editors._controls.Editor_Act ei = (wctMain.Admin._editors._controls.Editor_Act)form.FindControl("Editor_Acti");

            try
            {
                int idx = ei.SelectedIdx;

                //Validation
                if (idx <= 0)
                    error.ErrorList.Add("Please select an act or create a new one.");

                if (error.ErrorList.Count > 0)
                {
                    error.DisplayErrors();
                    e.Cancel = true;
                    return;
                }

                ShowDate selectedDate = Atx.CurrentShowRecord.FirstShowDate;

                System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(Profile.UserName);
                selectedDate.JShowActRecords().AddActToCollection(selectedDate, idx, string.Empty, mem.UserName, (Guid)mem.ProviderUserKey);

                //ent.Save();
                Atx.ResetCurrentShowRecord();
                Atx.CurrentActId = 0;

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
                    Atx.CurrentActId = 0;
                    break;
            }
        }
}
}//560 11/8/14 11am - 289 11/10/14 9:30am