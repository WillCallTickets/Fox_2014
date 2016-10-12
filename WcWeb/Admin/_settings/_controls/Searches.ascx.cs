using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin._settings._controls
{
    public partial class Searches : MainBaseControl
    {
        /// <summary>
        /// subscribe to the picker changing the principal
        /// </summary>
        protected override bool OnBubbleEvent(object source, EventArgs e)
        {
            bool handled = false;

            //handle principal change
            if (source is wctMain.Interfaces.IPrincipalPicker &&
                e is RepeaterCommandEventArgs)
            {
                GridView1.DataBind();
                handled = true;
            }

            return handled;
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@vcPrincipal"].Value = Atx.CurrentEditPrincipal;
        }
    }
}