using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Utils.Controls
{
    [ToolboxData("<{0}:LiteralErrorHandler runat=server></{0}:LiteralErrorHandler>")]
    public class LiteralErrorHandler : Literal
    {
        List<string> _errors = new List<string>();

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
    }
}
