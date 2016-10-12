using System;
using System.Web;

using wctMain.Admin;

namespace wctMain.Masters
{
    public partial class TemplateAdmin : System.Web.UI.MasterPage
    {
        /// <summary>
        /// It is important to note that we are using this function for 2 different purposes
        /// 1) get the starting date for the calendar - format needs to be yyyy-MM-dd HH:mm
        /// 2) get the initial date for display by the input - yyyy-MM-dd hh:mm tt
        /// //ie bug - hidden field must live outside of input group div
        /// and run this in JS to get current val $('#showdatecopycontrol').datetimepicker('update');
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        protected string getStartDateAndTime(string format) {

            //we are getting this => 1/20/2014 3:30:00 AM
            //but need this => 1979-09-16T05:25:07Z

            string dt = "";

            if (Atx.CurrentShowRecord != null)
                dt = Atx.CurrentShowRecord.LastDate.AddDays(1).ToString(format);

            return dt;
        }
       

        private Admin.AdminContext _atx = null;
        protected Admin.AdminContext Atx
        {
            get
            {
                if (_atx == null)
                {
                    _atx = new Admin.AdminContext();
                }

                return _atx;
            }
        }

        public override void Dispose()
        {
            wctMain.Admin.AdminEvent.ShowChosen -= new AdminEvent.ShowChosenEvent(AdminEvent_ShowChosen);         
            base.Dispose();
        }

        protected void AdminEvent_ShowChosen(object sender, AdminEvent.ShowChosenEventArgs e)
        {
            //hdnCurrentShowUrl.Value = (Atx.CurrentShowRecord != null) ? Atx.CurrentShowRecord.FirstShowDate.ConfiguredUrl : string.Empty;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Response.Expires = -300;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-45);
            Response.CacheControl = "no-cache";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-45));
            Response.AddHeader("pragma", "no-cache");

            wctMain.Admin.AdminEvent.ShowChosen += new AdminEvent.ShowChosenEvent(AdminEvent_ShowChosen);

            adminbody.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string cts = Atx.GetModalContext(false);

            if (!IsPostBack)
            {
                FormMain.Action = Request.RawUrl;

                if (this.Page.User.Identity.IsAuthenticated && this.Page.User.IsInRole("Administrator"))
                    this.Session["IsAdmin"] = true;

                hdnProcessingMessage.Value = wctMain.Admin.AdminContext.GetRandomProcessingMessage;

                hdnUserName.Value = Profile.UserName;
                hdnCurrentShowId.Value = (Atx.CurrentShowRecord != null) ? Atx.CurrentShowRecord.Id.ToString() : "0";
            }
        }
    }
}