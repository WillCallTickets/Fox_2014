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

namespace wctMain.Admin._collectionEditors._employee._controls
{
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    [ToolboxData("<{0}:EditEmployee_Container runat='Server' ></{0}:EditEmployee_Container>")]
    public partial class EditEmployee_Container : wctMain.Controller.AdminBase.PrincipaledCollectionContainerControl
    {
        public override _PrincipalBase.IPrincipal GetBindingIPrincipal()
        {
            return (_PrincipalBase.IPrincipal)Atx.CurrentEmployeeRecord;
        }
        protected override string contextTabCookieKey { get { return _Enums.GetDescription(Wcss._Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditEmployeeContainerTab); } }
        protected override string contextTabValue { get { return Atx.ActiveEditEmployeeContainerTab; } }
        protected override List<string> tabs
        {
            get
            {
                return new List<string>(new string[] { "Info" });
            }
        }


        #region Page-Control Overhead

        public override void ExplicitBind()
        {
            rptNavTabs.DataBind();

            //Bind individual control elements
            EditEmployee_Info1.ExplicitBind();
        }

        #endregion
        

        #region Tabs
        
        protected override void rptNavTabs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Repeater rpt = (Repeater)source;

            if (e.CommandName != null && e.CommandName.Trim().Length > 0)
            {
                FormView childView = null;
                string cmd = e.CommandName.ToLower();

                switch (_navTabContext.ToLower())
                {
                    case "info":
                        childView = (FormView)EditEmployee_Info1.FindControl("formEnt");
                        break;                    
                }

                base.HandleTabCommand(cmd, childView, e);
            }
        }

        #endregion
}
}
