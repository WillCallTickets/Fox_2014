using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Controller.AdminBase
{
    #region CollectionBase

    //public abstract class AlphaOrderedCollectionBaseEditor : MainBaseControl, IPostBackEventHandler
    //{
    //    #region Properties

    //    protected abstract wctMain.Interfaces.ICollectionPager collectionPager { get; }
    //    protected abstract System.Web.UI.WebControls.ListView listEntity { get; }
    //    protected abstract System.Web.UI.UpdatePanel updatePanel { get; }
    //    protected abstract string preRenderKey { get; }
    //    //protected abstract string searchCriteria { get; set; }

    //    /// <summary>
    //    /// Formats the information on each line of a listing
    //    /// </summary>
    //    protected abstract void FormatListingInfo(Wcss._AlphaOrderedEntity.IAlphaOrderedEntity entity, Literal lit);

    //    #endregion

    //    #region Collection Paging

    //    protected bool isSelectCount;

    //    protected virtual void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    //    {
    //        isSelectCount = e.ExecutingSelectCount;

    //        if (!isSelectCount)
    //        {
    //            e.Arguments.StartRowIndex = collectionPager.StartRowIndex;
    //            e.Arguments.MaximumRows = collectionPager.PageSize;
    //        }
    //    }

    //    protected void objData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    //    {
    //        if (isSelectCount && e.ReturnValue != null && e.ReturnValue.GetType().Name == "Int32")
    //        {
    //            collectionPager.DataSetSize = (int)e.ReturnValue;
    //            ExplicitBind();
    //        }
    //    }

    //    protected void collectionPager_CollectionPagerChanged(object sender, wctMain.Admin.AdminEvent.CollectionPagerEventArgs e)
    //    {
    //        Atx.AdminPageSize = e.NewPageSize;
    //        ExplicitBind();
    //    }

    //    #endregion

    //    #region PageOverhead

    //    protected override void OnInit(EventArgs e)
    //    {
    //        base.OnInit(e);
    //        collectionPager.CollectionPagerChanged += new wctMain.Admin.AdminEvent.CollectionPagerChangedEvent(collectionPager_CollectionPagerChanged);

    //        if (!this.Page.User.IsInRole("ContentEditor") && !this.Page.User.IsInRole("Super"))
    //            base.Redirect("/Admin/Default.aspx");
    //    }

    //    public override void Dispose()
    //    {
    //        collectionPager.CollectionPagerChanged -= new wctMain.Admin.AdminEvent.CollectionPagerChangedEvent(collectionPager_CollectionPagerChanged);
    //        base.Dispose();
    //    }

    //    public virtual void ExplicitBind()
    //    {
    //        collectionPager.DataBind();
    //        listEntity.DataBind();
    //    }

    //    void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
    //    {
    //        HandlePostBackEvent(eventArgument);
    //    }

    //    /// <summary>
    //    /// this is hardly ever used - orderable does not post back on success
    //    /// </summary>
    //    /// <param name="eventArgument"></param>
    //    protected virtual void HandlePostBackEvent(string eventArgument)
    //    {
    //        string[] args = eventArgument.Split('~');
    //        string command = args[0];

    //        switch (command.ToLower())
    //        {
    //            case "rebind":
    //                ExplicitBind();
    //                break;
    //        }
    //    }
        
    //    #endregion

    //    #region List Entity

    //    protected int listItemNum = 0;

    //    protected void listEnt_DataBinding(object sender, EventArgs e)
    //    {
    //        listItemNum = (collectionPager.PageIndex * collectionPager.PageSize) + 1;
    //    }

    //    protected virtual void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
    //    {
    //        ListView cont = (ListView)sender;

    //        LinkButton linkEdit = (LinkButton)e.Item.FindControl("linkEdit");
    //        if (linkEdit != null)
    //            linkEdit.Text = string.Format("{0}", listItemNum++.ToString());
    //    }

    //    protected virtual void listEnt_ItemInserting(object sender, ListViewInsertEventArgs e)
    //    {
    //        ListView list = (ListView)sender;
            
    //        //item gets inserted at the head of the list
    //        collectionPager.PageIndex = 0;
    //        //ExplicitBind();

    //        //refresh the search criteria
    //        //searchCriteria = new _PrincipalBase.Helpers.CollectionSearchCriteria();

    //        //on insert - refresh the list before selecting edit item
    //        list.DataBind();

    //        // Keeps listview in sync with current object
    //        SetListEditItem(list, 0);
    //    }

    //    protected void listEnt_ItemEditing(object sender, ListViewEditEventArgs e)
    //    {
    //        ListView list = (ListView)sender;
    //        SetListEditItem(list, e.NewEditIndex);
    //        //list.DataBind(); makes list bind twice!
    //    }

    //    protected void listEnt_ItemCanceling(object sender, ListViewCancelEventArgs e)
    //    {
    //        ListView list = (ListView)sender;
    //        list.EditIndex = -1;
    //        list.InsertItemPosition = InsertItemPosition.None;

    //        //ExplicitBind();
    //    }

    //    protected virtual void listEnt_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    //    {
    //        e.Cancel = true;//let object datasource ignore operation
    //        ExplicitBind();
    //    }

    //    protected virtual void SetListEditItem(ListView list, int idx)
    //    {
    //        list.InsertItemPosition = InsertItemPosition.None;
    //        list.EditIndex = idx;
    //    }

    //    protected void listEnt_ItemCommand(object sender, ListViewCommandEventArgs e)
    //    {
    //        switch (e.CommandName.ToLower())
    //        {
    //            case "new":
    //                ShowNewInterface((ListView)sender);
    //                break;
    //        }
    //    }

    //    protected void Cancel_Insert(object sender, ObjectDataSourceMethodEventArgs e)
    //    {
    //        e.Cancel = true;
    //    }

    //    protected void linkNew_Click(object sender, EventArgs e)
    //    {
    //        ShowNewInterface(listEntity);
    //    }

    //    protected void ShowNewInterface(ListView list)
    //    {
    //        //only allow inserts/inserting when we are in edit mode! UnblockUI is not properly disabling the NEW button
    //        //if (_navTabContext.ToLower() != "edit")
    //        //{
    //        //    ExplicitBind();
    //        //    return;
    //        //}

    //        list.EditIndex = -1;
    //        list.InsertItemPosition = InsertItemPosition.FirstItem;
    //        list.DataBind();
    //    }

    //    #endregion
    //}

    #endregion






    #region PrincipaledCollectionBase

    /// <summary>
    /// Handles the xxx_Edit controls
    /// </summary>
    public abstract class PrincipaledCollectionBaseEditor : MainBaseControl, IPostBackEventHandler
    {
        #region Properties

        protected abstract wctMain.Interfaces.ICollectionPager collectionContextPager { get; }
        protected abstract System.Web.UI.WebControls.ListView listEntity { get; }
        protected abstract System.Web.UI.UpdatePanel updatePanel { get; }
        protected abstract string preRenderKey { get; }
        protected abstract _PrincipalBase.Helpers.CollectionSearchCriteria searchCriteria { get; set; }
        
        /// <summary>
        /// Formats the information on each line of a listing
        /// </summary>
        protected abstract void FormatListingInfo(Wcss._PrincipalBase.IPrincipal entity, Literal lit);

        #endregion

        #region Collection Paging

        protected bool isSelectCount;

        /// <summary>
        /// Add params in order of the method list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = collectionContextPager.StartRowIndex;
                e.Arguments.MaximumRows = collectionContextPager.PageSize;
            }

            //reset everything if we are doing a running status check
            if (this.searchCriteria.Status == _Enums.CollectionSearchCriteriaStatusType.orderable)
            {
                this.searchCriteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(this.searchCriteria.Status.ToString(), null, null, null);
            }

            if (!e.InputParameters.Contains("principal"))
                e.InputParameters.Add("principal", Atx.CurrentEditPrincipal.ToString());

            //all, active, notactive, running
            if (!e.InputParameters.Contains("status"))
                e.InputParameters.Add("status", this.searchCriteria.Status.ToString());

            //dates are only used on kiosks and banners
            //only faq uses category 
            //but all of these are search criteria
            if (!e.InputParameters.Contains("startDate"))
                e.InputParameters.Add("startDate", this.searchCriteria.StartDate);

            if (!e.InputParameters.Contains("endDate"))
                e.InputParameters.Add("endDate", this.searchCriteria.EndDate);

            if (!e.InputParameters.Contains("searchTerms"))
                e.InputParameters.Add("searchTerms", this.searchCriteria.SearchTerms);
        }

        protected void objData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (isSelectCount && e.ReturnValue != null && e.ReturnValue.GetType().Name == "Int32")
            {
                collectionContextPager.DataSetSize = (int)e.ReturnValue;
                ExplicitBind();
            }
        }

        protected void collectionPager_CollectionPagerChanged(object sender, wctMain.Admin.AdminEvent.CollectionPagerEventArgs e)
        {
            Atx.AdminPageSize = e.NewPageSize;
            ExplicitBind();
        }

        #endregion

        #region Page Overhead

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            collectionContextPager.CollectionPagerChanged += new wctMain.Admin.AdminEvent.CollectionPagerChangedEvent(collectionPager_CollectionPagerChanged);

            if (!this.Page.User.IsInRole("ContentEditor") && !this.Page.User.IsInRole("Super"))
                base.Redirect("/Admin/Default.aspx");
        }

        public override void Dispose()
        {
            collectionContextPager.CollectionPagerChanged -= new wctMain.Admin.AdminEvent.CollectionPagerChangedEvent(collectionPager_CollectionPagerChanged);
            base.Dispose();
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            HandlePostBackEvent(eventArgument);
        }

        /// <summary>
        /// this is hardly ever used - orderable does not post back on success
        /// </summary>
        /// <param name="eventArgument"></param>
        protected virtual void HandlePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split('~');
            string command = args[0];

            switch (command.ToLower())
            {
                case "rebind":
                    ExplicitBind();
                    break;
            }
        }

        /// <summary>
        /// Respond to Principal changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool OnBubbleEvent(object source, EventArgs e)
        {
            bool handled = false;

            //handle principal change
            if (source is wctMain.Interfaces.IPrincipalPicker &&
                e is RepeaterCommandEventArgs)
            {
                collectionContextPager.PageIndex = 0;
                ExplicitBind();
                handled = true;
            }

            return handled;
        }

        protected override void OnPreRender(EventArgs e)
        {
            Type type = this.GetType();

            //add a script to register the text area as a ckeditor control
            //the key is per id as it should be loaded once per instance
            string key = string.Format("{0}_{1}", preRenderKey, DateTime.Now.Ticks.ToString());

            if (!Page.ClientScript.IsStartupScriptRegistered(type, key))
            {
                string javascript = string.Format("toggleTabRowControls('{0}', '{1}'); ",
                    "none",
                    "edit");

                //be cognizant of which control to register with!
                ScriptManager.RegisterStartupScript(updatePanel, type, key, string.Format("{0}{1}{0}", Environment.NewLine, javascript), true);
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Binding is handled via explicit binds
        /// </summary>
        protected virtual void Page_Load(object sender, EventArgs e) 
        { 
        }

        public virtual void ExplicitBind()
        {
            collectionContextPager.DataBind();
            listEntity.DataBind();
        }

        #endregion

        #region List Entity

        protected int listItemNum = 0;

        protected void listEnt_DataBinding(object sender, EventArgs e)
        {
            listItemNum = (collectionContextPager.PageIndex * collectionContextPager.PageSize) + 1;            
        }

        protected virtual void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            LinkButton linkEdit = (LinkButton)e.Item.FindControl("linkEdit");
            if (linkEdit != null)
                linkEdit.Text = string.Format("{0}", listItemNum++.ToString());
        }

        protected virtual void listEnt_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListView list = (ListView)sender;

            //reset principal to all for ordering and selection to work properly
            //and then set new item - inserts need to bind first when item added to top of list
            Atx.CurrentEditPrincipal = Wcss._Enums.Principal.all;

            wctMain.Interfaces.IPrincipalPicker principalPicker = (wctMain.Interfaces.IPrincipalPicker)this.FindControl("Picker_Principal1");
            if (principalPicker != null)
                principalPicker.DataBind();

            //item gets inserted at the head of the list
            collectionContextPager.PageIndex = 0;
            //ExplicitBind();

            //refresh the search criteria
            searchCriteria = new _PrincipalBase.Helpers.CollectionSearchCriteria();

            //this needs to follow the SetListItem or else the command control events are not bound properly??
            //on insert - refresh the list before selecting edit item
            list.DataBind();

            // Keeps listview in sync with current object
            SetListEditItem(list, 0);
            
        }

        protected void listEnt_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView list = (ListView)sender;
            SetListEditItem(list, e.NewEditIndex);
            //list.DataBind(); //makes list bind twice - but without it - events do not fire correctly??
        }

        protected void listEnt_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListView list = (ListView)sender;
            list.EditIndex = -1;
            list.InsertItemPosition = InsertItemPosition.None;
        }

        protected virtual void listEnt_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            e.Cancel = true;//let object datasource ignore operation
            ExplicitBind();
        }

        protected virtual void SetListEditItem(ListView list, int idx)
        {
            list.InsertItemPosition = InsertItemPosition.None;
            list.EditIndex = idx;
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

        protected void Cancel_Insert(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.Cancel = true;
        }

        protected void linkNew_Click(object sender, EventArgs e)
        {
            ShowNewInterface(listEntity);
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

        #endregion

        #region Helpers

        protected void chkPrincipal_DataBinding(object sender, EventArgs e)
        {
            Wcss._PrincipalBase.chkPrincipal_DataBinding(sender, e, false);
        }

        #endregion
    }

    #endregion
}

