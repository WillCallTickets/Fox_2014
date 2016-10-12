using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Admin._collectionEditors._employee._controls
{
    public partial class Employee_Edit : wctMain.Controller.AdminBase.PrincipaledCollectionBaseEditor
    {
        protected override wctMain.Interfaces.ICollectionPager collectionContextPager
        {
            get { return this.CollectionContextPager1; }
        }
        protected override System.Web.UI.WebControls.ListView listEntity
        {
            get { return this.listEnt; }
        }
        protected override System.Web.UI.UpdatePanel updatePanel
        {
            get { return this.UpdatePanel1; }
        }
        protected override string preRenderKey { get { return "employeeedit"; } }
        protected override _PrincipalBase.Helpers.CollectionSearchCriteria searchCriteria { get { return Atx.CollectionCriteria_Employee; } set { Atx.CollectionCriteria_Employee = value; } }
        protected override void FormatListingInfo(_PrincipalBase.IPrincipal entity, Literal lit)
        {
            Employee ent = (Employee)entity;

            if (ent != null && lit != null)
            {
                lit.Text = string.Format("{2} <span style=\"display:inline-block;width:200px;position:relative;top:5px;overflow:hidden;font-weight:bold;\">{0}, {1}</span>",
                    ent.LastName,
                    ent.FirstName,
                    string.Format("<input type=\"checkbox\" class=\"\" disabled=\"disabled\" name=\"activus_{0}\" id=\"activus_{0}\" {1}>",
                        ent.Id.ToString(), (ent.IsListInDirectory) ? "checked=\"checked\"" : string.Empty)
                        ).Trim();

                lit.Text += string.Format("<span style=\"display:inline-block;width:60px;margin-left:10px;\"><small><b>{0}</b></small></span>", ent.VcPrincipal);
                lit.Text += string.Format("<span style=\"display:inline-block;width:200px;margin-left:10px;\"><small><b>email</b> {0}</small></span>", ent.EmailAddress_Derived);
                lit.Text += string.Format("<span style=\"display:inline-block;width:200px;margin-left:10px;white-space:nowrap;overflow-x:hidden;position:relative;top:8px;\"><small><b>title</b> {0}</small></span>", ent.Title);
            }
        }

        #region ListView
                
        protected override void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            base.listEnt_ItemDataBound(sender, e);

            ListView cont = (ListView)sender;
            Employee ent = (e.Item.DataItem != null) ? (Employee)e.Item.DataItem : null;

            if (ent != null && cont.EditIndex != -1 && e.Item.DisplayIndex == cont.EditIndex)
            {
                EditEmployee_Container container = (EditEmployee_Container)e.Item.FindControl("EditEmployee_Container1");
                container.ExplicitBind();
            }
            else if (ent != null && e.Item.ItemType == ListViewItemType.DataItem)
            {
                Literal litInfo = (Literal)e.Item.FindControl("litInfo");
                FormatListingInfo(ent, litInfo);
            }
        }
        
        protected override void listEnt_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListView list = (ListView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)e.Item.FindControl("ErrorDisplayLabel1");

            if (error != null)
            {
                error.ResetErrors();

                try
                {
                    //Validation
                    if (e.Values["FirstName"] == null || e.Values["FirstName"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("FirstName is required.");
                    if (e.Values["LastName"] == null || e.Values["LastName"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("LastName is required.");
                    if (e.Values["Title"] == null || e.Values["Title"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("Title is required.");
                    if (e.Values["EmailAddress"] == null || e.Values["EmailAddress"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("EmailAddress is required.");
                    if (!Utils.Validation.IsValidEmail(e.Values["EmailAddress"].ToString()))
                        error.ErrorList.Add("Please enter a valid EmailAddress.");

                    List<_Enums.Principal> principalSelections = new List<_Enums.Principal>();

                    CheckBoxList chkPrincipal = (CheckBoxList)e.Item.FindControl("chkPrincipal");
                    if (chkPrincipal != null)
                    {
                        //no need to check for existing items as the list has been newly created
                        foreach (ListItem li in chkPrincipal.Items)
                            if (li.Selected)
                                principalSelections.Add(_PrincipalBase.PrincipalFromString(li.Text));
                    }

                    //we must have at least one principal selected - cannot be all
                    if (principalSelections.Contains(_Enums.Principal.all))
                        error.ErrorList.Add("Belongs To can contain all.");
                    else if (principalSelections.Count == 0)
                        error.ErrorList.Add("Belongs To must have at least one selection.");

                    if (error.ErrorList.Count > 0)
                    {
                        error.DisplayErrors();
                        e.Cancel = true;
                        return;
                    }

                    Employee ent = new Employee();
                    ent.ApplicationId = _Config.APPLICATION_ID;
                    ent.DtStamp = DateTime.Now;
                    ent.VcPrincipal = _PrincipalBase.PrincipalListToString(principalSelections);
                    ent.EmailAddress = e.Values["EmailAddress"].ToString().Trim();
                    ent.FirstName = e.Values["FirstName"].ToString().Trim();
                    ent.LastName = e.Values["LastName"].ToString().Trim();
                    ent.Title = e.Values["Title"].ToString().Trim();
                    ent.IsListInDirectory = true;
                    
                    ent.Save();


                    //call cleanup & rebind
                    base.listEnt_ItemInserting(sender, e);

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

        protected override void SetListEditItem(ListView list, int idx)
        {
            base.SetListEditItem(list, idx);

            Atx.CurrentEmployeeRecord = null;
            Atx.CurrentEmployeeRecord = new Employee((int)list.DataKeys[list.EditIndex].Value);
        }

        protected override void listEnt_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            Employee.Delete(e.Keys["Id"]);
            base.listEnt_ItemDeleting(sender, e);
        }
        
        #endregion

        #region Page-Control Overhead

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (hdnCollectionTableName != null)
                hdnCollectionTableName.DataBind();
            base.Page_Load(sender, e);
        }

        /// <summary>
        /// If argument contains {"commandName":"selectedIdFromTypeahead","newIdx":"10409"} then use dict
        /// Then handle the command accordingly
        /// </summary>
        /// <param name="eventArgument"></param>
        protected override void HandlePostBackEvent(string eventArgument)
        {
            string commandName = string.Empty;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] args = new string[12];//arbitrary number of indices

            if (eventArgument.IndexOf("commandName") != -1)
            {
                dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(eventArgument);
                commandName = dict["commandName"].ToLower();
            }
            else// if (eventArgument.IndexOf("~") != -1)
            {
                args = eventArgument.Split('~');
                commandName = args[0];
            }

            switch (commandName.ToLower())
            {
                case "selectedidfromtypeahead":
                    this.searchCriteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(
                        _Enums.CollectionSearchCriteriaStatusType.all.ToString(), null, null, string.Format("newIdx-{0}", dict["newIdx"]));
                    ExplicitBind();
                    break;
                case "refresh_post_upload":
                    //refresh current object
                    int idx = Atx.CurrentEmployeeRecord.Id;
                    Atx.CurrentEmployeeRecord = null;
                    Atx.CurrentEmployeeRecord = new Employee(idx);
                    ExplicitBind();
                    break;
                case "rebind":
                    ExplicitBind();
                    break;
            }
        }

        #endregion
}
}
