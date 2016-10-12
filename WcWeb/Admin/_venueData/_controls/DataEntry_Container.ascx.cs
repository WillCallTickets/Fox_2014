using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;

using System.Linq;

using SubSonic;
using Wcss;
using wctMain.Controller;

namespace wctMain.Admin._venueData._controls
{
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    [ToolboxData("<{0}:DataEntry_Container runat='Server' ></{0}:DataEntry_Container>")]
    public partial class DataEntry_Container : MainBaseControl, Wcss.VenueData.Helpers.IReBindingShowControl
    {
        protected Show BindingShowRecord 
        { 
            get 
            { 
                return (this.NamingContainer.TemplateControl.ToString().ToLower().IndexOf("venuedata_dataactq_ascx") != -1) ? 
                    Atx.VD_CurrentDataEntryShowRecord : 
                    Atx.CurrentShowRecord; 
            } 
        }

        public Show GetBindingShowRecord()
        {
            return BindingShowRecord;
        }

        #region Page-Control Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
            //BindEditorControls();
        }

        public void ExplicitBind()
        {
            litName.Text = BindingShowRecord.Name;

            BindNavTabs();

            DataEntry_ShowInfo1.ExplicitBind();
        }

        #endregion
        
        private List<string> tabs = new List<string>(new string[] { "Info", "Genres", "Tickets", "Concessions", "Expenses", "Marketing" });

        protected string _navTabContext
        {
            get
            {
                if (tabs.Contains(Atx.ActiveDataEntryTab.Trim()))
                    return Atx.ActiveDataEntryTab.Trim();

                return tabs[0];
            }
        }

        protected void BindNavTabs()
        {
            Repeater rpt = rptNavTabs;

            if (rpt != null)
            {
                //set up the divs to match the current context
                foreach (string s in tabs)
                {
                    //find a div with a like name
                    Literal div = (Literal)this.FindControl(string.Format("div{0}", s));
                    if (div != null)
                    {
                        div.Text = string.Format("<div id=\"{0}\" class=\"tab-pane panel fade{1}\" style=\"vertical-align:top;\">",
                            s,
                            (_navTabContext == s) ? " in active" : string.Empty);
                    }
                }

                rpt.DataSource = tabs;
                rpt.DataBind();
            }
        }

        protected void rptNavTabs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            string ent = (string)e.Item.DataItem;
            Literal lit = (Literal)e.Item.FindControl("litItem");

            if (ent != null && lit != null)
            {
                lit.Text = string.Format("<li{0}><a href=\"#{1}\" onclick=\"Set_Cookie('{2}', '{1}');\" class=\"btn\" data-toggle=\"tab\">{1}</a></li>",
                    (_navTabContext == ent) ? " class=\"active\"" : string.Empty,
                    ent,
                    "vdadet");//cookie key for this context
            }
        }

}
}
