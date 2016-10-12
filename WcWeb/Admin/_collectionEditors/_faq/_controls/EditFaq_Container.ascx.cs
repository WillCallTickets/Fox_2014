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

namespace wctMain.Admin._collectionEditors._faq._controls
{
    /// <summary>
    /// Only for data view - data entry has different cookies for state
    /// </summary>
    [ToolboxData("<{0}:EditFaq_Container runat='Server' ></{0}:EditFaq_Container>")]
    public partial class EditFaq_Container : wctMain.Controller.AdminBase.PrincipaledCollectionContainerControl
    {
        public override _PrincipalBase.IPrincipal GetBindingIPrincipal()
        {
            return (_PrincipalBase.IPrincipal)Atx.CurrentFaqRecord;
        }
        protected override string contextTabCookieKey { get { return _Enums.GetDescription(Wcss._Enums.CookEnums.AdminNavTabContainerCookie.ActiveEditFaqContainerTab); } }
        protected override string contextTabValue { get { return Atx.ActiveEditFaqContainerTab; } }
        protected override List<string> tabs
        {
            get
            {
                return new List<string>(new string[] { "Info", "Answer" });
            }
        }


        #region Page-Control Overhead

        public override void ExplicitBind()
        {
            rptNavTabs.DataBind();

            //Bind individual control elements
            EditFaq_Info1.ExplicitBind();
            EditFaq_Content1.ExplicitBind();
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
                        childView = (FormView)EditFaq_Info1.FindControl("formEnt");
                        break;
                    case "answer":
                        childView = (FormView)EditFaq_Content1.FindControl("formEnt");
                        break;
                }

                base.HandleTabCommand(cmd, childView, e);
            }
        }

        #endregion
    }
}
