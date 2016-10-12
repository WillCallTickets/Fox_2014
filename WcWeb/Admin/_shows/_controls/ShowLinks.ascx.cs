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
    public partial class ShowLinks : wctMain.Controller.AdminBase.ShowEditBase, IPostBackEventHandler
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
                    Atx.ResetCurrentShowRecord();
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
                listEnt.DataBind();
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ShowNewInterface(listEnt);
        }

        #region Show Listing

        protected void ddlShowLinks_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            System.Collections.Generic.List<ListItem> list = new System.Collections.Generic.List<ListItem>();

            ShowCollection coll = new ShowCollection();
            foreach (ShowDate sd in Atx._ShowDates_MasterList_WebUseCopyOnly)
            {
                if (Atx.CurrentShowRecord.Id != sd.TShowId && (!coll.GetList().Exists(delegate(Show match) { return (match.Id == sd.TShowId); })))
                    coll.Add(sd.ShowRecord);
            }
            foreach (Show s in coll)
                list.Add(new ListItem(s.Name_WithLocation, s.Id.ToString()));

            if (list.Count > 1)
                list.Sort(new Utils.Reflector.CompareEntities<ListItem>(Utils.Reflector.Direction.Ascending, "Text"));

            list.Insert(0, new ListItem("<-- Select A Show -->", "0"));

            ddl.DataSource = list;
            ddl.DataTextField = "Text";
            ddl.DataValueField = "Value";
        }

        protected void ddlShowLinks_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            int idx = (listEnt.SelectedDataKey != null) ? (int)listEnt.SelectedDataKey["Id"] : 0;

            if (idx > 0)
            {
                ShowLink ent = (ShowLink)Atx.CurrentShowRecord.ShowLinkRecords().Find(idx);

                if (ent != null && ent.IsShowLink)
                {
                    ddl.SelectedIndex = -1;

                    ListItem li = ddl.Items.FindByValue(ent.LinkUrl.ToString());
                    if (li != null)
                        li.Selected = true;
                    else
                    {
                        Show s = Show.FetchByID(ent.LinkUrl.ToString());
                        if (s != null && s.ApplicationId == _Config.APPLICATION_ID)
                        {
                            ListItem oldShow = new ListItem(s.Name_WithLocation, s.Id.ToString());
                            oldShow.Selected = true;
                            ddl.Items.Add(oldShow);
                        }
                    }
                }
            }
            
        }

        #endregion

        #region Link Listing

        protected int listItemNum = 0;

        protected void listEnt_DataBinding(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            listItemNum = 0;

            ShowLinkCollection coll = new ShowLinkCollection();
            coll.AddRange(Atx.CurrentShowRecord.ShowLinkRecords());
            if(coll.Count > 0)
                coll.Sort("IDisplayOrder", true);

            list.DataSource = coll;
        }

        protected void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView list = (ListView)sender;
            ShowLink ent = (ShowLink)e.Item.DataItem;
            
            LinkButton linkEdit = (LinkButton)e.Item.FindControl("linkEdit");
            if (linkEdit != null)
                linkEdit.Text = string.Format("{0}", listItemNum++.ToString());

            Literal litInfo = (Literal)e.Item.FindControl("litInfo");            
            if (litInfo != null && ent != null)
                litInfo.Text = string.Format("{0}{1}",
                    (Atx.IsSuperSession(this.Page.User)) ? string.Format("{0} - ", ent.Id.ToString()) : string.Empty, ent.LinkUrl_Formatted(true));
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

            ShowLink ent = (ShowLink)Atx.CurrentShowRecord.ShowLinkRecords().GetList().Find(delegate(ShowLink match) { return (match.Id == (int)e.Keys["Id"]); });

            if (ent != null)
            {
                CheckBox chkActive = (CheckBox)list.EditItem.FindControl("chkActive");

                try
                {
                    //if the ent is a show link - don't let it be edited
                    string link = (string)e.NewValues["LinkUrl"];
                    if (ent.IsRemoteLink && link.Length == 0)
                        error.ErrorList.Add("Link is required.");

                    if (ent.IsShowLink)
                        link = ent.LinkUrl;
                    else if (link.Length > 0)
                    {
                        //validate the url
                        if (!Utils.Validation.IsValidUrl(link))
                            error.ErrorList.Add("Please enter a valid url for the link.");
                    }

                    if (link.Trim().Length == 0)
                        throw new Exception("Input could not be configured.");

                    //display Text is required
                    string display = (string)e.NewValues["DisplayText"];
                    if (display.Length == 0)
                        error.ErrorList.Add("Display text is required.");
                    
                    if (error.ErrorList.Count > 0)
                    {
                        error.DisplayErrors();
                        e.Cancel = true;
                        return;
                    }


                    ent.IsActive = chkActive.Checked;
                    ent.DisplayText = display;
                    ent.LinkUrl = link;

                    if (ent.IsDirty)
                        ent.Save();

                    //reset here because name edits need to reflected regardless of other inputs
                    Atx.ResetCurrentShowRecord();

                    e.Cancel = false;

                    //same as cancel
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

        protected void listEnt_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListView list = (ListView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)e.Item.FindControl("ErrorDisplayLabel1");
            error.ResetErrors();
            
            try
            {
                //Validation
                DropDownList ddlShow = (DropDownList)list.InsertItem.FindControl("ddlShowLinks");

                string link = (string)e.Values["LinkUrl"] ?? string.Empty;
                string display = (string)e.Values["DisplayText"] ?? string.Empty;
                int showId = int.Parse(ddlShow.SelectedValue);

                //display Text is required for remote links
                if (showId <= 0 && display.Length == 0)
                    throw new Exception("Display text is required when entering a custom link.");
                else
                {
                    ShowDateCollection coll = new ShowDateCollection();
                    coll.AddRange(Atx._ShowDates_MasterList_WebUseCopyOnly.GetList().FindAll(delegate(ShowDate match) { return (match.TShowId == showId); }));
                    if (coll.Count > 0)
                    {
                        Show linker = coll[0].ShowRecord;
                        display = linker.Name_WithLocation;
                    }
                }

                //ensure the link context is singular
                if (link.Length > 0 && showId > 0)
                    throw new Exception("Please select either a link or a show. You cannot select both.");

                //at least one must be chosen
                if(link.Length == 0 && showId == 0)
                    throw new Exception("Please enter a link or select a show.");

                string inputLink = string.Empty;

                //validate input
                if (link.Length > 0)
                {
                    if (!Utils.Validation.IsValidUrl(link))
                        throw new Exception("Please enter a valid url for the link.");

                    inputLink = link.Trim();
                }
                else if (showId > 0)
                    inputLink = showId.ToString();
                    
                if(inputLink.Trim().Length == 0)
                    throw new Exception("Input could not be configured.");

                
                if (error.ErrorList.Count > 0)
                {
                    error.DisplayErrors();
                    e.Cancel = true;
                    return;
                }

                ShowLink added = Atx.CurrentShowRecord.ShowLinkRecords().AddToCollection(Atx.CurrentShowRecord.Id, inputLink, display);

                Atx.ResetCurrentShowRecord();

                e.Cancel = false;

                //same as cancel
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

        protected void listEnt_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            e.Cancel = true;//let object datasource ignore operation
            //ExplicitBind();
        }

        protected void listEnt_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView list = (ListView)sender;
            SetListEditItem(list, e.NewEditIndex);
        }

        protected void listEnt_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListView list = (ListView)sender;
            list.EditIndex = -1;
            list.InsertItemPosition = InsertItemPosition.None;
            list.DataBind();
        }

        protected void SetListEditItem(ListView list, int idx)
        {
            list.InsertItemPosition = InsertItemPosition.None;
            list.EditIndex = idx;
            list.DataBind();
        }

        protected void ShowNewInterface(ListView list)
        {
            list.EditIndex = -1;
            list.InsertItemPosition = InsertItemPosition.LastItem;
            list.DataBind();
        }

        #endregion
}
}//560 11/8/14 11am - 289 11/10/14 9:30am