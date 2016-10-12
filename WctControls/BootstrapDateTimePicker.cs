using System;
using System.Collections.Specialized;
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
    public class Bootstrap
    {
        public class DateTimePicker
        {
            /// <summary>
            /// Sep 02 2013
            /// </summary>
            public const string Date_FormatString = "MMM dd yyyy";

            /// <summary>
            /// Sep 02 2012 04:30 pm
            /// </summary>
            public const string DateTime_FormatString = "MMM dd yyyy hh:mm tt";

            /// <summary>
            /// 04:30 pm
            /// </summary>
            public const string Time_FormatString = "hh:mm tt";
        }
    }

    /// <summary>
    /// If Date is empty - than compareempty will be null?
    /// to center the control - place in a <div style="display:inline-block;margin:auto;">
    /// TODO add ctr capabilty from within this control
    /// Order of listed properties makes a diff!
    /// </summary>
    [DefaultProperty("Date")]
    [ToolboxData("<{0}:BootstrapDateTimePicker Label='' DateCompareEmpty='min' FormatString='' IsRequired='false' Date='' runat=server></{0}:BootstrapDateTimePicker>")]    
    public class BootstrapDateTimePicker : TextBox
    {
        private bool _isRequired = false;
        public bool IsRequired
        {
            get { return _isRequired; }
            set { _isRequired = value; }
        }

        //public string FormatString { get; set; }

        private string _formatString = null;
        ////[DataMember(Order = 0)]
        public string FormatString
        {
            get
            {
                if (_formatString == null)
                    return Bootstrap.DateTimePicker.DateTime_FormatString;

                return _formatString;
            }
            set
            {
                _formatString = value;

                SetText();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        private DateTime _date = DateTime.MinValue;
        public DateTime Date 
        { 
            get
            {
                DateTime dt = (this.Text.Trim().Length == 0) ? 
                    (DateCompareEmpty.ToLower() == "min") ? DateTime.MinValue : DateTime.MaxValue :
                    DateTime.Parse(this.Text.Trim());

                if (dt.Year < 1980)
                    dt = DateTime.MinValue;

                return dt;
            }
            set
            {   
                if (value == DateTime.MinValue || value == System.Data.SqlTypes.SqlDateTime.MinValue)
                    _date = DateTime.MinValue;
                else
                    _date = value;

                SetText();

                //The old way - keep it in case the new way does not work
                //DateTime compare = (DateCompareEmpty == null || DateCompareEmpty.ToLower() == "min") ? DateTime.MinValue : DateTime.MaxValue;
                //this.Text = (_date != compare) ? _date.ToString(FormatString) : string.Empty;
                //       //_date.ToString(Bootstrap.DateTimePicker.DateTime_FormatString) : string.Empty;
            }
        }

        private void SetText()
        {
            this.Text = (_date != DateTime.MinValue && _date < DateTime.MaxValue) ? _date.ToString(FormatString) : string.Empty;
        }
        
        /// <summary>
        /// A Date to compare to that should be renedered as an empty string.
        /// A start date should be compared to DateTime.MinValue and an end date to DateTime.MaxValue
        /// </summary>
        public string DateCompareEmpty { get; set; }//min or max

        public string Label { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            //<div class="input-group" style="width:220px;">
            //    <asp:TextBox ID="txtDateStart" runat="server" ClientIDMode="Static" Text='<%#Bind("DateStart") %>' TextMode="DateTimeLocal"
            //            CssClass="btdatetimepicker kiosk-dtpicker form-control"
            //            />   
            //    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
            //</div>


            Page.VerifyRenderingInServerForm(this);


            writer.AddAttribute(HtmlTextWriterAttribute.Class, "input-group btdatetimepicker-group");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //render the label
            writer.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("input-group-addon{0}", (IsRequired) ? " alert-danger" : string.Empty));
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            
            if(Label != null && Label.Trim().Length > 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "input-group-label");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(Label);
                writer.RenderEndTag();//end label span
            }

            
            writer.AddAttribute(HtmlTextWriterAttribute.Class, 
                (this.CssClass != null && this.CssClass.ToLower().IndexOf("-showtime-dtpicker") != -1) ?
                "glyphicon glyphicon-time" : "glyphicon glyphicon-calendar");
            writer.RenderBeginTag(HtmlTextWriterTag.I);
            writer.RenderEndTag();//end icon
            
            writer.RenderEndTag();//end of span addon


            //render the textbox
            this.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //this.TextMode = TextBoxMode.DateTimeLocal;//makes a mess dont use it!

            string css = string.Format("btdatetimepicker form-control{0}", (this.CssClass.Trim().Length > 0) ? string.Format(" {0}", this.CssClass) : string.Empty);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, css);
            
            base.Render(writer);
            

            

            writer.RenderEndTag();//end of input group div
        }
    }
}
