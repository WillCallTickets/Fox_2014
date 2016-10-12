using System;
using System.Web.UI.WebControls;

using Wcss;

namespace z2Main.Controller
{
    public partial class ControlBase : System.Web.UI.UserControl
    {   
        public Z2Context Ztx
        {
            get
            {
                return ((PageBase)base.Page).Ztx;
            }
        }

        public void Redirect(string url)
        {
            ((PageBase)base.Page).Redirect(url);
        }

        public string UserAgentInformation { get { return ((PageBase)base.Page).UserAgentInformation; } }

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
	}
}