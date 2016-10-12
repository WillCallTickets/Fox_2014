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
    [ToolboxData("<{0}:EditKiosk_Arrangement runat='Server'></{0}:EditKiosk_Arrangement>")]
    public partial class EditKiosk_Arrangement : wctMain.Controller.AdminBase.PrincipaledCollectionContainerItem
    {
        //declare local props
        protected int displayWidth = 650;//635


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

        protected void rdoTextColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rdo = (RadioButtonList)sender;
            Kiosk ent = (Kiosk)BindingEntity;

            ent.ChangeTextColor(rdo.SelectedItem.Text);
            ent.Save();
            BindParentContainer();
        }

        protected void rdoKioskPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rdo = (RadioButtonList)sender;
            Kiosk ent = (Kiosk)BindingEntity;

            ent.ChangeTextPosition(rdo.SelectedItem.Text);
            ent.Save();
            BindParentContainer();
        }

        #endregion


        #region Form Events

        protected override void formEntity_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Kiosk ent = (Kiosk)form.DataItem;

            if (ent != null)
            {
                //KIOSK styling selection
                RadioButtonList rdoTextColor = (RadioButtonList)form.FindControl("rdoTextColor");

                if (rdoTextColor != null)
                {
                    rdoTextColor.SelectedIndex = -1;

                    ListItem li = rdoTextColor.Items.FindByValue(ent.TextColorClass);

                    if (li != null)
                        li.Selected = true;
                    else
                        rdoTextColor.SelectedIndex = 0;
                }


                RadioButtonList rdoKioskPosition = (RadioButtonList)form.FindControl("rdoKioskPosition");

                if (rdoKioskPosition != null)
                {
                    rdoKioskPosition.SelectedIndex = -1;

                    ListItem li = rdoKioskPosition.Items.FindByValue(ent.PositionClass);

                    if (li != null)
                        li.Selected = true;
                    else
                        rdoKioskPosition.SelectedIndex = 0;
                }
                //end styling selection


                //viewport controls
                Literal litViewportImage = (Literal)form.FindControl("litViewportImage");
                Literal litViewportText = (Literal)form.FindControl("litViewportText");

                if (litViewportImage != null && litViewportText != null)
                {
                    //image is condtional
                    if (ent.ImageManager != null)
                    {
                        litViewportImage.Text = string.Format("<img src=\"{0}\" alt=\"{1}\" data-cycle-timeout=\"{2}\" class=\"{3}\" width=\"{4}px\" />",
                            ent.ImageManager.Thumbnail_Max,
                            ent.Name,
                            ent.Timeout,
                            string.Empty, 
                            displayWidth.ToString()
                        );
                    }
                    else
                        litViewportImage.Text = string.Format("No image specified.");

                    //text is always
                    litViewportText.Text = ent.TextDiv;
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
                    //CheckBox chkCtrY = (CheckBox)form.FindControl("chkCtrY");

                    //ent.Centered_Y = (!chkCtrY.Checked);

                    ////ent.Name = e.NewValues["Name"].ToString();
                    ////ent.IsActive = (bool)e.NewValues["IsActive"];
                    ////ent.Timeout = int.Parse(e.NewValues["Timeout"].ToString());
                    ////ent.EventVenue = e.NewValues["EventVenue"].ToString();
                    ////ent.EventDate = e.NewValues["EventDate"].ToString();
                    ////ent.EventTitle = e.NewValues["EventTitle"].ToString();
                    ////ent.EventHeads = e.NewValues["EventHeads"].ToString();
                    ////ent.EventOpeners = e.NewValues["EventOpeners"].ToString();
                    ////ent.EventDescription = e.NewValues["EventDescription"].ToString();
                    ////ent.DateStart = (DateTime)e.NewValues["DateStart"];
                    ////ent.DateEnd = (DateTime)e.NewValues["DateEnd"];

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
