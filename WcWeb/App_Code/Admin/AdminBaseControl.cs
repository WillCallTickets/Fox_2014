using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Controller.AdminBase
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PrincipaledCollectionContainerItem : MainBaseControl, Wcss.VenueData.Helpers.IExplicitBinder
    {
        protected abstract WctControls.WebControls.ErrorDisplayLabel errorDisplay { get; }
        protected abstract System.Web.UI.WebControls.FormView formEntity { get; }
                
        protected virtual _PrincipalBase.IPrincipal BindingEntity {
            get {
                return ((Wcss.VenueData.Helpers.IBindingIPrincipalControl)this.NamingContainer).GetBindingIPrincipal();
            }
        }

        //Allow binding to be controlled externally
        protected void Page_Load(object sender, EventArgs e) {}

        //disable auto-bind
        public override void DataBind() {}

        public virtual void ExplicitBind()
        {
            formEntity.DataBind();
        }

        public virtual void BindParentContainer()
        {
            ((Wcss.VenueData.Helpers.IExplicitBinder)this.NamingContainer).ExplicitBind();
        }

        public virtual void BindParentContainerContextTabControl()
        {
            Repeater rpt = (Repeater)((wctMain.Controller.AdminBase.PrincipaledCollectionContainerControl)this.NamingContainer).FindControl("rptNavTabs");
            if (rpt != null)
                rpt.DataBind();
        }
        //End auto- binding

        //Not all derived use this - 
        protected virtual void chkPrincipal_DataBinding(object sender, EventArgs e)
        {
            _PrincipalBase.chkPrincipal_DataBinding(sender, e, false);
        }

        //Form Events
        protected void formEntity_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            List<Wcss._PrincipalBase.IPrincipal> list = new List<Wcss._PrincipalBase.IPrincipal>();
            list.Add(BindingEntity);

            form.DataSource = list;
            form.DataKeyNames = new string[] { "Id" };
        }

        protected abstract void formEntity_DataBound(object sender, EventArgs e);

        /// <summary>
        /// Bind entity to (IPrincipal)BindingEntity as DataItem will be null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void formEntity_ItemUpdating(object sender, FormViewUpdateEventArgs e);

        /// <summary>
        /// Shows the errors and binds the navTabs of the container
        /// </summary>
        protected void DisplayUpdateErrors()
        {
            errorDisplay.DisplayErrors();

            BindParentContainerContextTabControl();
        }

        protected void formEntity_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;

            errorDisplay.ResetErrors();
            errorDisplay.DisplayErrors();//reset any error display

            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)//handles cancel correctly
                BindParentContainer();
        }

        protected virtual void formEntity_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            throw new NotImplementedException();
        }
    }




    public abstract class PrincipaledCollectionContainerControl : MainBaseControl, Wcss.VenueData.Helpers.IExplicitBinder, Wcss.VenueData.Helpers.IBindingIPrincipalControl
    {
        public abstract _PrincipalBase.IPrincipal GetBindingIPrincipal();
        protected abstract string contextTabCookieKey { get; }
        protected abstract string contextTabValue { get; }

        /// <summary>
        /// Containers must specify their tab list
        /// </summary>
        protected abstract List<string> tabs { get; }

        #region Page Overhead
        
        //force explicit binding
        protected void Page_Load(object sender, EventArgs e) { }

        public override void DataBind() { }

        //container must override so that it can include its custom items - info, image, arrangement, etc
        public abstract void ExplicitBind();

        #endregion

        #region Tabbed Navigation - Context

        protected string _navTabContext
        {
            get
            {
                if (tabs.Contains(contextTabValue.Trim()))
                    return contextTabValue.Trim();

                return tabs[0];
            }
        }

        protected void rptNavTabs_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            if (rpt != null)
            {
                //set up the divs to match the current context
                foreach (string s in tabs)
                {
                    //find a div with a like name
                    Literal div = (Literal)this.FindControl(string.Format("div{0}", s));
                    if (div != null)
                    {
                        div.Text = string.Format("<div id=\"{0}\" class=\"tab-pane panel fade{1}\" style=\"vertical-align:top;\">",
                            s,
                            (_navTabContext == s) ? " in active" : string.Empty);
                    }
                }
            }

            rpt.DataSource = tabs;
        }

        protected void rptNavTabs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            string ent = (string)e.Item.DataItem;
            Literal lit = (Literal)e.Item.FindControl("litItem");

            if (ent != null && lit != null)
            {
                lit.Text = string.Format("<li{0}><a href=\"#{1}\" onclick=\"Set_Cookie('{2}', '{1}');\" rel=\"{2}\" class=\"btn pager-context-nav\" data-toggle=\"tab\">{1}</a></li>",
                    (_navTabContext == ent) ? " class=\"active\"" : string.Empty,
                    ent,
                    contextTabCookieKey);//cookie key for this context
            }
        }

        /// <summary>
        /// The derived control needs to direct its own traffic
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected abstract void rptNavTabs_ItemCommand(object source, RepeaterCommandEventArgs e);

        /// <summary>
        /// The derived controls will generally share this functionality
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="view"></param>
        /// <param name="e"></param>
        public void HandleTabCommand(string cmd, FormView view, RepeaterCommandEventArgs e)
        {
            switch (cmd)
            {
                case "update":
                    if (view != null)
                        view.UpdateItem(true);
                    break;
                case "reset":
                    ErrorDisplayLabel err = (ErrorDisplayLabel)((PrincipaledCollectionContainerItem)view.NamingContainer).FindControl("ErrorDisplayLabel1");
                    if (err != null)
                        err.ResetErrors();
                    ExplicitBind();
                    break;
                case "cancel":
                    //bubble this up - gets caught parent list view in listEnt_ItemCommand => listEnt_ItemCanceling
                    RaiseBubbleEvent(this, e);
                    break;
            }
        }

        #endregion
    }




    #region PrincipaledCollectionBase


    /// <summary>
    /// Handles the xxx_Edit controls
    /// </summary>
    public abstract class PrincipaledCollectionBase : MainBaseControl, IPostBackEventHandler
    {
        #region Properties

        protected abstract wctMain.Interfaces.ICollectionPager collectionPager { get; }
        protected abstract System.Web.UI.WebControls.Repeater repeatTabbedNavigation { get; }
        protected abstract System.Web.UI.WebControls.ListView listEntity { get; }
        protected abstract System.Web.UI.WebControls.Repeater repeatOrder { get; }
        protected abstract System.Web.UI.UpdatePanel updatePanel { get; }        
        protected abstract string preRenderKey { get; }
        protected abstract string contextTabCookieKey { get; }
        protected abstract string contextTabValue { get; }

        /// <summary>
        /// Provides a basic listing for tabs. 
        /// </summary>
        protected virtual List<string> tabs
        {
            get
            {
                return new List<string>(new string[] { "Edit", "Order" });
            }
        }

        protected abstract List<Wcss._PrincipalBase.IPrincipal> SortedOrderList { get; }

        /// <summary>
        /// Formats the information on each line of a listing
        /// </summary>
        protected abstract void FormatListingInfo(Wcss._PrincipalBase.IPrincipal entity, Literal lit);

        #endregion

        #region Collection Paging

        private bool isSelectCount;

        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = collectionPager.StartRowIndex;
                e.Arguments.MaximumRows = collectionPager.PageSize;
            }

            if (!e.InputParameters.Contains("principal"))
                e.InputParameters.Add("principal", Atx.CurrentEditPrincipal.ToString());
        }

        protected void objData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (isSelectCount && e.ReturnValue != null && e.ReturnValue.GetType().Name == "Int32")
            {
                collectionPager.DataSetSize = (int)e.ReturnValue;
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
            collectionPager.CollectionPagerChanged += new wctMain.Admin.AdminEvent.CollectionPagerChangedEvent(collectionPager_CollectionPagerChanged);

            if (!this.Page.User.IsInRole("ContentEditor") && !this.Page.User.IsInRole("Super"))
                base.Redirect("/Admin/Default.aspx");
        }

        public override void Dispose()
        {
            collectionPager.CollectionPagerChanged -= new wctMain.Admin.AdminEvent.CollectionPagerChangedEvent(collectionPager_CollectionPagerChanged);
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

        protected override bool OnBubbleEvent(object source, EventArgs e)
        {
            bool handled = false;

            //handle principal change
            if (source is wctMain.Interfaces.IPrincipalPicker &&
                e is RepeaterCommandEventArgs)
            {
                collectionPager.PageIndex = 0;
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
                    contextTabCookieKey,
                    contextTabValue);

                //be cognizant of which control to register with!
                ScriptManager.RegisterStartupScript(updatePanel, type, key, string.Format("{0}{1}{0}", Environment.NewLine, javascript), true);
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Binding is handled via explicit binds
        /// </summary>
        protected virtual void Page_Load(object sender, EventArgs e) { }

        public virtual void ExplicitBind()
        {
            collectionPager.DataBind();
            repeatTabbedNavigation.DataBind();
            listEntity.DataBind();
            repeatOrder.DataBind();
        }

        #endregion

        #region Tabbed Navigation - Context

        protected string _navTabContext
        {
            get
            {
                if (tabs.Contains(contextTabValue.Trim()))
                    return contextTabValue.Trim();

                return tabs[0];
            }
        }

        protected void rptNavTabs_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            if (rpt != null)
            {
                //set up the divs to match the current context
                foreach (string s in tabs)
                {
                    //find a div with a like name
                    Literal div = (Literal)this.FindControl(string.Format("div{0}", s));
                    if (div != null)
                    {
                        div.Text = string.Format("<div id=\"{0}\" class=\"tab-pane panel fade{1}\" style=\"vertical-align:top;\">",
                            s,
                            (_navTabContext == s) ? " in active" : string.Empty);
                    }
                }
            }

            rpt.DataSource = tabs;
        }

        protected void rptNavTabs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            string ent = (string)e.Item.DataItem;
            Literal lit = (Literal)e.Item.FindControl("litItem");

            if (ent != null && lit != null)
            {
                lit.Text = string.Format("<li{0}><a href=\"#{1}\" onclick=\"Set_Cookie('{2}', '{1}');\" rel=\"{2}\" class=\"btn pager-context-nav\" data-toggle=\"tab\">{1}</a></li>",
                    (_navTabContext == ent) ? " class=\"active\"" : string.Empty,
                    ent,
                    contextTabCookieKey);//cookie key for this context
            }
        }

        #endregion

        #region Helpers

        protected void chkPrincipal_DataBinding(object sender, EventArgs e)
        {
            Wcss._PrincipalBase.chkPrincipal_DataBinding(sender, e, false);
        }

        #endregion

        #region List Entity

        protected int listItemNum = 0;

        protected void listEnt_DataBinding(object sender, EventArgs e)
        {
            listItemNum = (collectionPager.PageIndex * collectionPager.PageSize) + 1;
        }

        protected virtual void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView cont = (ListView)sender;

            LinkButton linkEdit = (LinkButton)e.Item.FindControl("linkEdit");
            if (linkEdit != null)
            {
                linkEdit.Text = string.Format("{0}", listItemNum++.ToString());
            }
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
            collectionPager.PageIndex = 0;
            //ExplicitBind();

            //on insert - refresh the list before selecting edit item
            list.DataBind();

            // Keeps listview in sync with current object
            SetListEditItem(list, 0);
        }

        protected void listEnt_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView list = (ListView)sender;
            SetListEditItem(list, e.NewEditIndex);
            //list.DataBind(); makes list bind twice!
        }

        protected void listEnt_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListView list = (ListView)sender;
            list.EditIndex = -1;
            list.InsertItemPosition = InsertItemPosition.None;

            //ExplicitBind();
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

        protected void linkNew_Click(object sender, EventArgs e)
        {
            ShowNewInterface(listEntity);
        }

        protected void ShowNewInterface(ListView list)
        {
            //only allow inserts/inserting when we are in edit mode! UnblockUI is not properly disabling the NEW button
            if (_navTabContext.ToLower() != "edit")
            {
                ExplicitBind();
                return;
            }
            
            list.EditIndex = -1;
            list.InsertItemPosition = InsertItemPosition.FirstItem;
            list.DataBind();
        }

        protected void Cancel_Insert(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion

        #region Order Listing Repeater

        protected int row = 0;

        protected void rptOrder_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            //reset row number on each bind
            row = 1;

            rpt.DataSource = SortedOrderList;
        }

        protected void rptOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal litRowNum = (Literal)e.Item.FindControl("litRowNum");
            if (litRowNum != null)
            {
                litRowNum.Text = string.Format("<span class=\"badge item-number\">{0}</span>",
                    row++.ToString());
            }

            BindOrderItem((Repeater)sender, e);
        }

        protected abstract void BindOrderItem(Repeater rpt, RepeaterItemEventArgs e);

        #endregion
    }

    #endregion

    #region DirectorBasePage

    public abstract class DirectorBaseSimple : MainBasePage
    {
        /// <summary>
        /// this is the code used in the querystring as in ?p={controlCode}
        /// </summary>
        protected abstract string defaultControlCode { get; }
        /// <summary>
        /// this is the relative path to the ascx control
        /// </summary>
        protected abstract string defaultControlPath { get; }
        protected abstract System.Web.UI.WebControls.Panel controlPanel { get; }

        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string control = string.Empty;
            string req = Request.QueryString["p"];
            if (req != null && req.Trim().Length > 0)
                control = req;
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            SetPageControl();
        }

        protected string GetQueryP()
        {
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                return req;

            return defaultControlCode;
        }

        protected virtual void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string controlToLoad = GetQueryP();

            if (controlToLoad == defaultControlCode)
                controlToLoad = defaultControlPath;

            controlPanel.Controls.Add(LoadControl(string.Format(@"{0}.ascx", controlToLoad)));
        }
    }

    public abstract class DirectorBase : DirectorBaseSimple
    {   
        protected abstract System.Web.UI.WebControls.HiddenField hdnCollectionName { get; }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if(hdnCollectionName != null)
                hdnCollectionName.DataBind();
            base.Page_Load(sender, e);
        }
    }

    #endregion
}
