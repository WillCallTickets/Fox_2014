using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WctControls.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ErrorDisplayLabel runat=server ></{0}:ErrorDisplayLabel>")]
    public class ErrorDisplayLabel : Label
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public override string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        private List<string> _errors = new List<string>();

        public List<string> ErrorList
        {
            get
            {
                return this._errors;
            }
        }

        public void ResetErrors()
        {
            this._errors.Clear();
            this.Text = string.Empty;
        }

        public void DisplayErrors()
        {
            string errorlist = this.ErrorOutput;

            this.Text = (errorlist == null || errorlist.Trim().Length == 0) ? string.Empty :
                string.Format("<div class=\"alert alert-danger error-listing\" style=\"display:block;\">{0}</div>", errorlist);            
        }

        private string ErrorOutput
        {
            get
            {
                if (_errors.Count > 0)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<ul>");

                    foreach (string s in _errors)
                        sb.AppendFormat("<li>{0}</li>", s);

                    sb.Append("</ul>");

                    return sb.ToString();
                }
            
                return string.Empty;
            }
        }

        //protected override void RenderContents(HtmlTextWriter output)
        protected override void Render(HtmlTextWriter output)
        {
            output.Write(Text);
        }
    }
}
