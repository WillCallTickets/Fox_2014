using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;
using wctMain.Controller;


namespace wctMain.Admin._customControls
{
    /// <summary>
    /// This control sets the cookie - that is all. Clients should subscribe to the AdminEvent.CurrentCurrentEditPrincipalChanged() event example TBA
    /// In the future - it could apply other filters
    /// </summary>
    [ToolboxData("<{0}:CollectionContextPrincipalPicker runat='Server' Title='' AppliedTo='Edit' ></{0}:CollectionContextPrincipalPicker>")]
    public partial class CollectionContextPrincipalPicker : MainBaseControl, wctMain.Interfaces.IPrincipalPicker
    {
        public string Title { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if(!IsPostBack)//dont leave this in!!!
            rptPrincipal.DataBind();            
        }

        protected void rptPrincipal_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            rpt.DataSource = Enum.GetNames(typeof(_Enums.Principal)).ToList<string>();
        }

        protected void rptPrincipal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
        }

        protected void rptPrincipal_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Repeater rpt = (Repeater)source;

            if (e.CommandName.ToLower() == "select")
            {
                string s = e.CommandArgument.ToString();

                Atx.CurrentEditPrincipal = (_Enums.Principal)Enum.Parse(typeof(_Enums.Principal), s, true);

                AdminEvent.OnCurrentActiveEditPrincipalChanged(this, Atx.CurrentEditPrincipal);

                //Atx.UpdateOrderableCookies();
                
                rptPrincipal.DataBind();

                //bubble up - necessary for binding other view elements
                RaiseBubbleEvent(this, e);
            }
        }
}
}