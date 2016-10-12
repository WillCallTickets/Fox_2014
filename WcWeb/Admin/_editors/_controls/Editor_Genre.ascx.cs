using System;
using System.Web.UI.WebControls;

using Wcss;
using wctMain.Controller;
using WctControls.WebControls;

namespace wctMain.Admin._editors._controls
{
    public partial class Editor_Genre : MainBaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ExplicitBind();
        }

        protected void ExplicitBind()
        {
            listEnt.DataBind();
        }

        #region List View

        protected int listItemNum = 0;

        protected void listEnt_DataBinding(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;

            list.DataSource = _Lookits.Genres;

            string[] keyNames = { "Id" };
            list.DataKeyNames = keyNames;

            listItemNum = 0;
        }

        protected void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView cont = (ListView)sender;

            LinkButton linkEdit = (LinkButton)e.Item.FindControl("linkEdit");
            if (linkEdit != null)
                linkEdit.Text = string.Format("{0}", listItemNum++.ToString());
        }

        protected void listEnt_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView list = (ListView)sender;
            SetListEditItem(list, e.NewEditIndex);
            list.DataBind(); 
        }

        protected void listEnt_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListView list = (ListView)sender;
            
            ErrorDisplayLabel error = (ErrorDisplayLabel)list.EditItem.FindControl("ErrorDisplayLabel1");
            Genre ent = (Genre)_Lookits.Genres.Find((int)list.DataKeys[e.ItemIndex]["Id"]);

            if (ent != null && error != null)
            {
                error.ResetErrors();

                try
                {
                    //Validation
                    if (e.NewValues["Name"] == null || e.NewValues["Name"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("Name is required.");

                    if (error.ErrorList.Count > 0)
                    {
                        error.DisplayErrors();
                        return;
                    }

                    ent.Name = e.NewValues["Name"].ToString().Trim();
                    
                    if (ent.IsDirty)
                    {
                        ent.Save();
                        _Lookits.RefreshLookup(_Enums.LookupTableNames.Genres.ToString());                        
                    }

                    e.Cancel = false;
                    list.EditIndex = -1;
                    list.InsertItemPosition = InsertItemPosition.None;
                    list.DataBind();

                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);

                    error.ErrorList.Add(ex.Message);
                    error.DisplayErrors();
                    e.Cancel = true;
                    return;
                }
            }
        }

        protected void listEnt_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListView list = (ListView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)e.Item.FindControl("ErrorDisplayLabel1");

            if (error != null)
            {
                error.ResetErrors();

                try
                {
                    //Validation
                    if (e.Values["Name"] == null || e.Values["Name"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("Name is required.");

                    if (error.ErrorList.Count > 0)
                    {
                        error.DisplayErrors();
                        e.Cancel = true;
                        return;
                    }

                    string name = e.Values["Name"].ToString().Trim();
                    string description = (e.Values["Description"] != null) ? e.Values["Description"].ToString().Trim() : null;

                    Genre newItem = _Lookits.Genres.AddItemToCollection(name, description);

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.Genres.ToString());
                    
                    e.Cancel = false;
                    list.EditIndex = -1;
                    list.InsertItemPosition = InsertItemPosition.None;
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
        }

        protected void listEnt_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListView list = (ListView)sender;
            list.EditIndex = -1;
            list.InsertItemPosition = InsertItemPosition.None;

            ExplicitBind();
        }

        protected virtual void SetListEditItem(ListView list, int idx)
        {
            list.InsertItemPosition = InsertItemPosition.None;
            list.EditIndex = idx;
        }

        protected void listEnt_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListView list = (ListView)sender;
            _Lookits.Genres.DeleteGenreFromCollection((int)list.DataKeys[e.ItemIndex]["Id"]);
            e.Cancel = false;
            ExplicitBind();
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

        protected void linkNew_Click(object sender, EventArgs e)
        {
            ShowNewInterface(listEnt);
        }

        protected void ShowNewInterface(ListView list)
        {
            //only allow inserts/inserting when we are in edit mode! UnblockUI is not properly disabling the NEW button
            //if (_navTabContext.ToLower() != "edit")
            //{
            //    ExplicitBind();
            //    return;
            //}

            list.EditIndex = -1;
            list.InsertItemPosition = InsertItemPosition.FirstItem;
            list.DataBind();
        }

        protected void Cancel_Insert(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion

    }
}