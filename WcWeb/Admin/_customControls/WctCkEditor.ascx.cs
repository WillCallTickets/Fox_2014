using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin._customControls
{
    /// <summary>
    /// Notes
    /// There needs to be a click event assigned that puts the value of the ckeditor into a hidden value for retrieval 
    /// CkEditor obscures the "new" text from the TextBox here
    /// 
    /// Event though we have a var to set width - this should be enclosed in a div that sets a width to contain the control
    /// 
    /// string javascript = string.Format("try {{ CKEDITOR.replace( 'txtEditor' ); }} catch(e) {{}} 
    /// function updateHiddenEditorValue() {{ $('#hdnEditorValue').val(CKEDITOR.instances.txtEditor.getData()); }}");
    /// 
    ///  
    /// To set value from client
    ///     --in databound event
    ///     LinkButton btnSave1 = (LinkButton)form.FindControl("btnSave1");
    ///     Admin._customControls.WctCkEditor wct = (Admin._customControls.WctCkEditor)form.FindControl("WctCkEditor1");
    ///     if (btnSave1 != null && wct != null)
    ///     {
    ///         btnSave1.OnClientClick = wct.ClientClickMethodCall;
    ///     }
    ///     
    ///     --in updating event
    ///     Admin._customControls.WctCkEditor wct = (Admin._customControls.WctCkEditor)form.FindControl("WctCkEditor1");
    ///     Atx.CurrentMailerContent.VcJsonContent = wct.CkEditorValue;
    ///
    /// 
    ///  For nested clients....Hairy!!! - but works for nested CKeditors    
    ///        Repeater rpt = (Repeater)form.NamingContainer.NamingContainer.FindControl("rptNavTabs");
    ///        foreach (Control c in rpt.Controls)
    ///        {
    ///            if (c is RepeaterItem)
    ///            {
    ///                if (((RepeaterItem)c).ItemType == ListItemType.Footer)
    ///                {
    ///                    LinkButton link = (LinkButton)c.FindControl("btnSave");
    ///
    ///                    Admin._customControls.WctCkEditor wct = (Admin._customControls.WctCkEditor)form.FindControl("WctCkEditor1");
    ///                    link.OnClientClick = wct.ClientClickMethodCall;
    ///                    break;
    ///                }
    ///            }
    ///        }
    /// 
    /// </summary>
    [ToolboxData("<{0}:WctCkEditor runat='Server'  Text='' Height='' ></{0}:WctCkEditor>")]
    public partial class WctCkEditor : System.Web.UI.UserControl
    {
        public TextBox EditorTextBox
        {
            get
            {
                return this.txtEditor;
            }
        } 

        public string Text 
        { 
            get 
            {
                return this.txtEditor.Text; 
            } 
            set 
            { 
                this.txtEditor.Text = value;
            } 
        }

        public int Height
        {
            get
            {
                return (int)this.txtEditor.Height.Value;
            }
            set
            {
                this.txtEditor.Height = value;
            }
        }

        //public int Width
        //{
        //    get
        //    {
        //        return (int)this.txtEditor.Width.Value;
        //    }
        //    set
        //    {
        //        this.txtEditor.Width = value;
        //    }
        //}

        //public int Rows
        //{
        //    get
        //    {
        //        return (int)this.txtEditor.Rows;
        //    }
        //    set
        //    {
        //        this.txtEditor.Rows = value;
        //    }
        //}
        
        public string CkEditorValue 
        {
            get
            {
                return this.hdnEditorValue.Value;
            }
        }
        
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

        }

        /// <summary>
        /// The calling client should use this string in the Update button's onclient click method call
        /// </summary>
        /// <returns></returns>
        public string ClientClickMethodCall
        {
            get
            {
                return string.Format("updateHiddenCkEditorValue('{0}', '{1}');", this.txtEditor.ClientID, this.hdnEditorValue.ClientID);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            Type type = this.GetType();

            //add a script to ensure a handler for the click event - this should only be added once for all instances
            if (!Page.ClientScript.IsClientScriptBlockRegistered(type, type.FullName))
            {
                string javascript = string.Format("function updateHiddenCkEditorValue(ckelementId, ckhiddenId) {{ $('#' + ckhiddenId).val(CKEDITOR.instances[ckelementId].getData()); }}");
                ScriptManager.RegisterClientScriptBlock(this, type, type.FullName, string.Format("{0}{1}{0}", Environment.NewLine, javascript), true);
            }

            //add a script to register the text area as a ckeditor control
            //the key is per id as it should be loaded once per instance
            if (!Page.ClientScript.IsStartupScriptRegistered(type, this.ID))
            {
                string javascript = string.Format("try {{ CKEDITOR.replace( '{0}', {{ height: {1} }} ); }} catch(e) {{ alert (e); }} ", this.txtEditor.ClientID, this.Height);
                //produces -> try { CKEDITOR.replace( 'ctl00_MainContent_ctl01_WctCkEditor1_txtEditor' ); } catch(e) { alert (e); }

                ScriptManager.RegisterStartupScript(this, type, this.ID, string.Format("{0}{1}{0}", Environment.NewLine, javascript), true);
            }

            base.OnPreRender(e);
        }
}
}