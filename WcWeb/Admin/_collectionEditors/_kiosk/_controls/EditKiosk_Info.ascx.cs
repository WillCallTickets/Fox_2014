using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Admin._collectionEditors._kiosk._controls
{
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    [ToolboxData("<{0}:EditKiosk_Info runat='Server'></{0}:EditKiosk_Info>")]
    public partial class EditKiosk_Info : wctMain.Controller.AdminBase.PrincipaledCollectionContainerItem
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
            Kiosk ent = (Kiosk)form.DataItem;

            if (ent != null)
            {
                CheckBoxList chk = (CheckBoxList)form.FindControl("chkPrincipal");

                if (chk != null)
                {
                    chk.SelectedIndex = -1;

                    foreach (ListItem li in chk.Items)
                    {
                        Kiosk_Principal ep = new Kiosk_Principal(ent);
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
            Kiosk ent = (Kiosk)BindingEntity;

            if (ent != null && error != null)
            {
                error.ResetErrors();
                
                try
                {
                    //Validation
                    if (e.NewValues["Name"] == null || e.NewValues["Name"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("Name is required.");

                    List<_Enums.Principal> principalSelections = new List<_Enums.Principal>();
                    CheckBoxList chkPrincipal = (CheckBoxList)form.FindControl("chkPrincipal");
                    if (chkPrincipal != null)
                    {
                        //no need to check for existing items as the list has been newly created
                        foreach (ListItem li in chkPrincipal.Items)
                            if (li.Selected)
                                principalSelections.Add(_PrincipalBase.PrincipalFromString(li.Text));
                    }

                    //we must have at least one principal selected - cannot be all
                    if (principalSelections.Contains(_Enums.Principal.all))
                        error.ErrorList.Add("Belongs To cannot contain ALL.");
                    else if (principalSelections.Count == 0)
                        error.ErrorList.Add("Belongs To must have at least one selection.");

                    if (error.ErrorList.Count > 0)
                    {
                        DisplayUpdateErrors();
                        return;
                    }


                    ent.Name = e.NewValues["Name"].ToString().Trim();
                    ent.IsActive = (bool)e.NewValues["IsActive"];
                    ent.Timeout = int.Parse(e.NewValues["Timeout"].ToString());
                    ent.EventVenue = e.NewValues["EventVenue"].ToString();
                    ent.EventDate = e.NewValues["EventDate"].ToString();
                    ent.EventTitle = e.NewValues["EventTitle"].ToString();
                    ent.EventHeads = e.NewValues["EventHeads"].ToString();
                    ent.EventOpeners = e.NewValues["EventOpeners"].ToString();
                    ent.EventDescription = e.NewValues["EventDescription"].ToString();
                    ent.DateStart = (DateTime)e.NewValues["DateStart"];
                    ent.DateEnd = (DateTime)e.NewValues["DateEnd"];

                    ent.VcPrincipal = _PrincipalBase.PrincipalListToString(principalSelections);

                    //keep ordinals in sync
                    Kiosk_Principal ep = new Kiosk_Principal(ent);
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
