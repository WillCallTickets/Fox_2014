using System;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Web.Services;

using Wcss;
using wctMain.Controller;

namespace wctMain.View
{
    public partial class ContactView : MainBaseControl
    {
        public string url { get; set; }
        public string context { get; set; }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        public override void Dispose()
        {
            this.Load -= new System.EventHandler(this.Page_Load);
            base.Dispose();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //MainContext Ctx = new MainContext();

            if (!IsPostBack && pnlContact.Visible)
                litEmployeeInfo.DataBind();

            if (context == null || context == string.Empty)
                context = "contactus";

            string normalPane = "tab-pane fade";
            string activePane = "tab-pane fade active in";            

            contactus.Attributes.Add("class", (context == contactus.ID.ToString()) ? activePane : normalPane);
            li_contactus.Attributes.Add("class", (context == contactus.ID.ToString()) ? "active" : string.Empty);

            boxoffices.Attributes.Add("class", (context == boxoffices.ID.ToString()) ? activePane : normalPane);
            li_boxoffices.Attributes.Add("class", (context == boxoffices.ID.ToString()) ? "active" : string.Empty);

            officeinfo.Attributes.Add("class", (context == officeinfo.ID.ToString()) ? activePane : normalPane);
            li_officeinfo.Attributes.Add("class", (context == officeinfo.ID.ToString()) ? "active" : string.Empty);
        }

        protected void litEmployeeInfo_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (Employee emp in Ctx.EmployeeList)
            {
                Utils.MailtoHelper helper = new Utils.MailtoHelper(emp.EmailAddress, string.Empty, string.Empty, 
                    string.Format("{0} {1}", emp.FirstName, emp.LastName),
                    "btn btn-foxt btn-xs"
                    );

                sb.AppendFormat("<tr><th>{0}</th><td>{1}</td><td class=\"extension\">{2}</td></tr>",
                    helper.FormattedMailtoLink,
                    emp.Title,
                    (emp.Extension != null && emp.Extension.Trim().Length > 0) ? string.Format("ext {0}", emp.Extension) : "&nbsp;");
            }

            lit.Text = sb.ToString();
        }
    }
}