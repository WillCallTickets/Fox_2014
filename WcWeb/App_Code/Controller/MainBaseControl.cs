using System;
using System.Web.UI.WebControls;

using Wcss;
using wctMain.Admin;

namespace wctMain.Controller
{
	public partial class MainBaseControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// previously userWebInfo
        /// </summary>
        public string UserAgentInformation { get { return ((MainBasePage)base.Page).UserAgentInformation; } }

        /// <summary>
        /// Indicates if there was an error to display - ShowException == true indicates there was an error
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected bool ShowException(Exception ex)
        {
            if (ex != null)
            {
                CustomValidator custom = (CustomValidator)this.FindControl("CustomValidation");
                if (custom == null)
                    custom = (CustomValidator)this.FindControl("CustomPageValidation");

                if (custom != null)
                {
                    custom.IsValid = false;
                    custom.ErrorMessage = ex.Message;
                    return true;
                }
            }
            return false;
        }

        private MainContext _ctx;
        public MainContext Ctx
        {
            get
            {
                if (_ctx == null)
                    _ctx = new MainContext();

                return _ctx;
            }
            set
            {
                _ctx = value;
            }
        }

        public AdminContext Atx
        {
            get
            {
                return (this.Page == null) ? new AdminContext() : (AdminContext)((wctMain.Controller.MainBasePage)this.Page).Atx;
            }
            set
            {
                ((wctMain.Controller.MainBasePage)this.Page).Atx = value;
            }
        }

        public void Redirect(string url)
        {
            ((MainBasePage)base.Page).Redirect(url);
        }

        public bool IsPageAdminContext { get { return ((MainBasePage)this.Page).IsPageInAdminContext; } }
	}
}
