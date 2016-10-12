using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;

using Wcss;
using WctControls.WebControls;
using wctMain.Admin;

namespace wctMain.Controller.AdminBase
{
    public abstract class ShowEditBase : MainBaseControl
    {
        protected abstract FormView mainForm { get; }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            mainForm.DeleteItem();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mainForm.DataBind();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            mainForm.UpdateItem(false);
        }

        protected void btnChangeShowName_Click(object sender, EventArgs e)
        {
            ChangeShowName();
        }

        protected void ChangeShowName()
        {
            if (!Atx.CurrentShowRecord.ShowNameMatches(false))
            {
                Atx.CurrentShowRecord.ShowNameMatches(true);

                AdminEvent.OnShowNameChanged(this);

                Atx.ResetCurrentShowRecord();
            }
        }
    }

}
