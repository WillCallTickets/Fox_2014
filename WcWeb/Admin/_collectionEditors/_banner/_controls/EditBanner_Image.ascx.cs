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
    [ToolboxData("<{0}:EditBanner_Image runat='Server'></{0}:EditBanner_Image>")]
    public partial class EditBanner_Image : wctMain.Controller.AdminBase.PrincipaledCollectionContainerItem
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

        
        //implement custom events
        protected override void formEntity_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            SalePromotion ent = (SalePromotion)form.DataItem;
            Literal litImageEditBox = (Literal)form.FindControl("litImageEditBox");

            //(banners will be sized to 960wx90h)--fox
            //(images should be 650px wide and up to 150 in height)
            int width = 500;

            if (litImageEditBox != null)
            {
                if (ent.BannerUrl != null && ent.BannerUrl.Trim().Length > 0)
                {
                    litImageEditBox.Text = string.Format("<img src=\"{0}{1}\" border=\"0\" alt=\"\" width=\"{2}\" style=\"display:block;\" />",
                        Wcss.SalePromotion.Banner_VirtualDirectory, ent.BannerUrl, width.ToString());
                }
                else
                    litImageEditBox.Text = string.Format("<img src=\"/assets/images/view.gif\" border=\"0\" alt=\"\"  style=\"display:block;\" /> no image specified");
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
                    ent.BannerClickUrl = (e.NewValues["BannerClickUrl"] != null && e.NewValues["BannerClickUrl"].ToString().Trim().Length > 0) ?
                        e.NewValues["BannerClickUrl"].ToString().Trim() : string.Empty;

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
}
}
