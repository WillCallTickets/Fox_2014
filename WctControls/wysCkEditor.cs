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
    [ToolboxData("<{0}:wysCkEditor runat=server ID=''></{0}:wysCkEditor>")]
    public class wysCkEditor : Control, INamingContainer
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

        private string _id = null;
        
        public string ID
        {
            get
            {
                if (_id == null)
                    throw new ArgumentNullException();

                return _id;
            }

            set
            {
                _id = value;
            }
        }

        protected string EditorID
        {
            get
            {
                return string.Format("{0}_TextEditor", ID);
            }
        }

        protected string HiddenValueID
        {
            get
            {
                return string.Format("{0}_HiddenValue", ID);
            }
        }

        public TextBox EditorTextBox 
        {
            get 
            {
                EnsureChildControls();
                return (TextBox)this.FindControl(EditorID);
            }
        }

        public HiddenField HiddenValue
        {
            get
            {
                EnsureChildControls();
                return (HiddenField)this.FindControl(HiddenValueID);
            }
        }

        public event EventHandler TxtChanged;

        protected virtual void OnTxtChanged(EventArgs ce)
        {
            if (TxtChanged != null)
            {
                EnsureChildControls();
                HiddenField hdnEditorValue = (HiddenField)this.FindControl(HiddenValueID);
            }
        }

        private void EditorTextChanged(Object sender, EventArgs e)
        {
            OnTxtChanged(new EventArgs());
        }

        protected override void CreateChildControls()
        {
            
            TextBox txtCkEditor = new TextBox();
            txtCkEditor.ID = EditorID;
            txtCkEditor.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtCkEditor.TextMode = TextBoxMode.MultiLine;
            txtCkEditor.TextChanged += new EventHandler(this.EditorTextChanged);
            Controls.Add(txtCkEditor);

            HiddenField hdnEditorValue = new HiddenField();
            hdnEditorValue.ID = HiddenValueID;
            hdnEditorValue.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            Controls.Add(hdnEditorValue);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //add a script to register the text area as a ckeditor control
            //string javascript = string.Format("try {{ CKEDITOR.replace( 'txtCkEditor' ); }} catch(e) {{}} ");// function updateHiddenEditorValue() {{ $('#hdnEditorValue').val(CKEDITOR.instances.txtEditor.getData()); }}");//  function updateHiddenEditorValue() {{ $(#hdnEditorValue).val(CKEDITOR.instances.txtEditor.getData()); }}");
            ////ScriptManager.RegisterClientScriptBlock(this, this.GetType(), DateTime.Now.Ticks.ToString(), "function updateHiddenEditorValue() {$('#hdnEditorValue').val(CKEDITOR.instances.txtEditor.getData());}", true);
            //this.Page.ScriptManager.RegisterStartupScript(this, this.GetType(), DateTime.Now.Ticks.ToString(), javascript, true);

            //ClientScriptManager
        }
        
        //protected override void Render(HtmlTextWriter output)
        //{
        //    output.Write(Text);
        //}
    }
}
