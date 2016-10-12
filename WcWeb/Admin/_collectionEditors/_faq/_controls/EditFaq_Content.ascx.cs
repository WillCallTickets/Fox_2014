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
    [ToolboxData("<{0}:EditFaq_Content runat='Server'></{0}:EditFaq_Content>")]
    public partial class EditFaq_Content : wctMain.Controller.AdminBase.PrincipaledCollectionContainerItem
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
            
            //Hairy!!! - but works for nested CKeditors
            Repeater rpt = (Repeater)form.NamingContainer.NamingContainer.FindControl("rptNavTabs");
            foreach (Control c in rpt.Controls)
            {
                if (c is RepeaterItem)
                {
                    if (((RepeaterItem)c).ItemType == ListItemType.Footer)
                    {
                        LinkButton link = (LinkButton)c.FindControl("btnSave");

                        Admin._customControls.WctCkEditor wct = (Admin._customControls.WctCkEditor)form.FindControl("WctCkEditor1");
                        link.OnClientClick = wct.ClientClickMethodCall;
                        break;
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
                    Admin._customControls.WctCkEditor wct = (Admin._customControls.WctCkEditor)form.FindControl("WctCkEditor1");                   
                    ent.Answer = wct.CkEditorValue;
                                        
                    //if (error.ErrorList.Count > 1)
                    //    throw new Exception("Errors ocurred:");

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