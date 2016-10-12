using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Admin._collectionEditors._faq._controls
{
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    [ToolboxData("<{0}:EditFaq_Info runat='Server'></{0}:EditFaq_Info>")]
    public partial class EditFaq_Info : wctMain.Controller.AdminBase.PrincipaledCollectionContainerItem
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
            FaqItem ent = (FaqItem)form.DataItem;

            if (ent != null)
            {
                CheckBoxList chk = (CheckBoxList)form.FindControl("chkPrincipal");

                if (chk != null)
                {
                    chk.SelectedIndex = -1;

                    foreach (ListItem li in chk.Items)
                    {
                        FaqItem_Principal ep = new FaqItem_Principal(ent);
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
            FaqItem ent = (FaqItem)BindingEntity;

            if (ent != null && error != null)
            {
                error.ResetErrors();
                
                try
                {
                    //Validation
                    if (e.NewValues["Question"] == null || e.NewValues["Question"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("Question is required.");

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

                    // category
                    RadioButtonList rdoCategory = (RadioButtonList)form.FindControl("rdoCategory");
                    ent.TFaqCategorieId = int.Parse(rdoCategory.SelectedValue);
                    ent.Question = e.NewValues["Question"].ToString().Trim();
                    ent.IsActive = (bool)e.NewValues["IsActive"];


                    //keep ordinals in sync
                    FaqItem_Principal ep = new FaqItem_Principal(ent);
                    ep.SyncOrds();

                    // category - sync
                    _Lookits.RefreshLookup("FaqCategories");
                    Atx.CurrentFaqCategorySelection = rdoCategory.SelectedItem.Text;

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

        #region Faq Categories

        protected void rdoCategory_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rdo = (RadioButtonList)sender;

            ListItemCollection coll = new ListItemCollection();

            foreach (FaqCategorie f in _Lookits.FaqCategories)
                coll.Add(new ListItem(f.Name, f.Id.ToString()));

            rdo.DataSource = coll;
            rdo.DataTextField = "text";
            rdo.DataValueField = "value";
        }

        protected void rdoCategory_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rdo = (RadioButtonList)sender;

            //reset
            rdo.SelectedIndex = -1;

            ListItem li = rdo.Items.FindByValue(Atx.CurrentFaqRecord.FaqCategorieRecord.Id.ToString());
            if (li != null)
                li.Selected = true;
        }

        #endregion
    }
}
