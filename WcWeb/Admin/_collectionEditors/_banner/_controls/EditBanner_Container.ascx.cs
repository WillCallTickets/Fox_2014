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

namespace wctMain.Admin._collectionEditors._banner._controls
{
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    [ToolboxData("<{0}:EditBanner_Container runat='Server' ></{0}:EditBanner_Container>")]
    public partial class EditBanner_Container : wctMain.Controller.AdminBase.PrincipaledCollectionContainerControl
    {
        public override _PrincipalBase.IPrincipal GetBindingIPrincipal()
        {
            return (_PrincipalBase.IPrincipal)Atx.CurrentBannerRecord;
        }
        protected override string contextTabCookieKey { get { return _Enums.GetDescription(Wcss._Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditBannerContainerTab); } }
        protected override string contextTabValue { get { return Atx.ActiveEditBannerContainerTab; } }
        protected override List<string> tabs
        {
            get
            {
                return new List<string>(new string[] { "Info", "Image" });
            }
        }


        #region Page-Control Overhead
        
        public override void ExplicitBind()
        {
            rptNavTabs.DataBind();

            //Bind individual control elements
            EditBanner_Info1.ExplicitBind();
            EditBanner_Image1.ExplicitBind();
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
                        childView = (FormView)EditBanner_Info1.FindControl("formEnt");
                        break;
                    case "image":
                        childView = (FormView)EditBanner_Image1.FindControl("formEnt");
                        break;  
                }

                base.HandleTabCommand(cmd, childView, e);
            }
        }

        #endregion
    }
}
