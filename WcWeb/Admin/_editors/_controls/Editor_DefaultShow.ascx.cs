using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;

using Newtonsoft.Json;

using Wcss;
using wctMain.Controller;
using WctControls.WebControls;

namespace wctMain.Admin._editors._controls
{
    public partial class Editor_DefaultShow : MainBaseControl
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void ddlShowDates_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            ddl.SelectedIndex = -1;

            int idx = (ddl.ID.IndexOf("Fox") != -1) ? _Config.Default_ShowDateId : _Config.Default_BT_ShowDateId;

            if (idx > 0)
            {
                ListItem li = ddl.Items.FindByValue(idx.ToString());

                if (li != null)
                    li.Selected = true;
            }
        }

        protected void ddlShowDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            int idx = int.Parse(ddl.SelectedValue);

            if (ddl.ID.IndexOf("Fox") != -1 && idx != _Config.Default_ShowDateId)
            {
                _Config.Default_ShowDateId = idx;
            }
            else if (ddl.ID.IndexOf("Fox") == -1 && idx != _Config.Default_BT_ShowDateId)
            {
                _Config.Default_BT_ShowDateId = idx;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            DropDownList ddl = ddlFox;

            if (btn.ID.IndexOf("Fox") != -1)
                _Config.Default_ShowDateId = 0;
            else
            {
                _Config.Default_BT_ShowDateId = 0;
                ddl = ddlBT;
            }

            ddl.DataBind();
        }   
}
}