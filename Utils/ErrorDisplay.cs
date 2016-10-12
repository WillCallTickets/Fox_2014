using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// TODO move this class to WctControls?

namespace Utils
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ErrorDisplay runat=server></{0}:ErrorDisplay>")]
    public class ErrorDisplay : Label
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
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

        public void ResetErrors()
        {
            this._errors.Clear();
        }

        public void DisplayErrors()
        {
            string error = Utils.Validation.DisplayValidationErrors(_errors);

            this.Text = (error == null || error.Trim().Length == 0) ? string.Empty :
                string.Format("<div class=\"warning\">{0}</div>", error);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(Text);
        }
    }
}
