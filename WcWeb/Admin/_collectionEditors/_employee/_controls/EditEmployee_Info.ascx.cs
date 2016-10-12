using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Admin._collectionEditors._employee._controls
{
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    [ToolboxData("<{0}:EditEmployee_Info runat='Server'></{0}:EditEmployee_Info>")]
    public partial class EditEmployee_Info : wctMain.Controller.AdminBase.PrincipaledCollectionContainerItem
    {
        //assign base class items
        protected override WctControls.WebControls.ErrorDisplayLabel errorDisplay
        {
            get { return this.ErrorDisplayLabel1; }
        }

        protected override System.Web.UI.WebControls.FormView formEntity
        {
            get { return this.formEnt; }
        }


        #region Form Events

        protected override void formEntity_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Employee ent = (Employee)form.DataItem;

            if (ent != null)
            {
                CheckBoxList chk = (CheckBoxList)form.FindControl("chkPrincipal");

                if (chk != null)
                {
                    chk.SelectedIndex = -1;

                    foreach (ListItem li in chk.Items)
                    {
                        Employee_Principal ep = new Employee_Principal(ent);
                        if (ep.HasPrincipal(_PrincipalBase.PrincipalFromString(li.Text)))
                            li.Selected = true;
                    }
                }
            }
        }
        
        protected override void formEntity_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = ErrorDisplayLabel1;
            Employee ent = (Employee)BindingEntity;

            if (ent != null && error != null)
            {
                error.ResetErrors();
                
                try
                {   
                    //fox employees require first, last and email
                    //z2 requires only title and email

                    //Validation
                    if (e.NewValues["FirstName"] == null || e.NewValues["FirstName"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("FirstName is required.");
                    if (e.NewValues["LastName"] == null || e.NewValues["LastName"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("LastName is required.");
                    if (e.NewValues["Title"] == null || e.NewValues["Title"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("Title is required.");
                    if (e.NewValues["EmailAddress"] == null || e.NewValues["EmailAddress"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("EmailAddress is required.");
                    if(!Utils.Validation.IsValidEmail(e.NewValues["EmailAddress"].ToString()))
                        error.ErrorList.Add("Please enter a valid EmailAddress.");

                    List<_Enums.Principal> princes = new List<_Enums.Principal>();
                    CheckBoxList chkPrincipal = (CheckBoxList)form.FindControl("chkPrincipal");
                    if (chkPrincipal != null)
                    {
                        //no need to check for existing items as the list has been newly created
                        foreach (ListItem li in chkPrincipal.Items)
                            if (li.Selected)
                                princes.Add(_PrincipalBase.PrincipalFromString(li.Text));
                    }

                    //we must have at least one principal selected - cannot be all
                    if (princes.Contains(_Enums.Principal.all))
                        error.ErrorList.Add("Belongs To cannot contain ALL.");
                    else if (princes.Count == 0)
                        error.ErrorList.Add("Belongs To must have at least one selection.");


                    if (error.ErrorList.Count > 0)
                    {
                        DisplayUpdateErrors();
                        return;
                    }
                    

                    ent.FirstName = e.NewValues["FirstName"].ToString();
                    ent.LastName = e.NewValues["LastName"].ToString();
                    ent.EmailAddress = e.NewValues["EmailAddress"].ToString();
                    ent.Title = e.NewValues["Title"].ToString();
                    //ent.Extension = e.NewValues["Extension"].ToString();
                    ent.IsListInDirectory = bool.Parse(e.NewValues["IsListInDirectory"].ToString());
                    ent.VcPrincipal = _PrincipalBase.PrincipalListToString(princes);
                    

                    //keep ordinals in sync
                    Employee_Principal ep = new Employee_Principal(ent);
                    ep.SyncOrds();

                    
                    if (ent.IsDirty)
                    {
                        ent.Save();
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    error.ErrorList.Add(ex.Message);
                    DisplayUpdateErrors();
                    return;
                }
            }

            BindParentContainer();
        }

        #endregion
    }
}
