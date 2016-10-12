using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Admin._collectionEditors._post._controls
{
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    [ToolboxData("<{0}:EditPost_Info runat='Server'></{0}:EditPost_Info>")]
    public partial class EditPost_Info : wctMain.Controller.AdminBase.PrincipaledCollectionContainerItem
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
            Post ent = (Post)form.DataItem;

            if (ent != null)
            {
                CheckBoxList chk = (CheckBoxList)form.FindControl("chkPrincipal");

                if (chk != null)
                {
                    chk.SelectedIndex = -1;

                    foreach (ListItem li in chk.Items)
                    {
                        Post_Principal ep = new Post_Principal(ent);
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
            Post ent = (Post)BindingEntity;

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
                    ent.Slug = e.NewValues["Slug"].ToString().Trim();
                    //if slug is empty - form from name - name must be valid
                    if (ent.Name != null && ent.Name.Trim().Length > 0 &&
                        (ent.Slug == null || ent.Slug.Trim().Length == 0))
                        ent.Slug = Post.FormatSlug(ent.Name);
                    ent.IsActive = (bool)e.NewValues["IsActive"];
                    ent.Description = e.NewValues["Description"].ToString();

                    //keep ordinals in sync
                    Post_Principal ep = new Post_Principal(ent);
                    ep.SyncOrds();


                    if (ent.IsDirty)
                    {
                        ent.DtModified = DateTime.Now;
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
