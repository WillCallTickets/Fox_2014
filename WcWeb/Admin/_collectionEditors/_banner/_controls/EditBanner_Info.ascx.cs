using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Admin._collectionEditors._banner._controls
{
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    [ToolboxData("<{0}:EditBanner_Info runat='Server'></{0}:EditBanner_Info>")]
    public partial class EditBanner_Info : wctMain.Controller.AdminBase.PrincipaledCollectionContainerItem
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


        #region Not implemented in base class

        protected void ddlShowList_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            ddl.DataSource = Wcss.VenueData.Helpers
                .ShowList_Get(_Enums.Principal.all, 150, DateTime.Now.AddDays(-3), "Select a Show for Click Url then click Insert Selection");
            ddl.DataTextField = "Text";
            ddl.DataValueField = "Value";
        }

        #endregion


        #region Form Events

        protected override void formEntity_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            SalePromotion ent = (SalePromotion)form.DataItem;            

            if (ent != null)
            {
                CheckBoxList chk = (CheckBoxList)form.FindControl("chkPrincipal");

                if (chk != null)
                {
                    chk.SelectedIndex = -1;

                    foreach (ListItem li in chk.Items)
                    {
                        SalePromotion_Principal ep = new SalePromotion_Principal(ent);
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
            SalePromotion ent = (SalePromotion)BindingEntity;  

            if (ent != null && error != null)
            {
                error.ResetErrors();
                
                try
                {   
                    //Validation
                    if (e.NewValues["Name"] == null || e.NewValues["Name"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("Name is required.");
                    if (e.NewValues["DisplayText"] == null || e.NewValues["DisplayText"].ToString().Trim().Length == 0)
                            error.ErrorList.Add("DisplayText is required.");

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

                    ent.VcPrincipal = _PrincipalBase.PrincipalListToString(principalSelections);
                    ent.Name = e.NewValues["Name"].ToString();
                    ent.DisplayText = e.NewValues["DisplayText"].ToString();
                    ent.AdditionalText = (e.NewValues["AdditionalText"] != null) ? e.NewValues["AdditionalText"].ToString() : string.Empty;
                    //ent.BannerUrl = (e.NewValues["BannerUrl"] != null) ? e.NewValues["BannerUrl"].ToString() : string.Empty;                    
                    ent.BannerTimeout = (e.NewValues["BannerTimeout"] != null) ? int.Parse(e.NewValues["BannerTimeout"].ToString()) : _Config._BannerDisplayTime;
                    ent.IsActive = (bool)e.NewValues["IsActive"];
                    ent.DateStart = (DateTime)e.NewValues["DateStart"];
                    ent.DateEnd = (DateTime)e.NewValues["DateEnd"];

                    //show click url handled by "Insert Selection"
                    ent.BannerClickUrl = (e.NewValues["BannerClickUrl"] != null) ? e.NewValues["BannerClickUrl"].ToString() : string.Empty;

                    //keep ordinals in sync
                    SalePromotion_Principal ep = new SalePromotion_Principal(ent);
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
