using System;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.IO;
//using System.Text;
//using System.Text.RegularExpressions;
using System.Collections.Generic;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin._shows._controls
{
    public partial class ShowImages : wctMain.Controller.AdminBase.ShowEditBase, IPostBackEventHandler
    {
        protected override FormView mainForm { get { return null; } }

        protected ShowDate firstShowDate = null;
        protected int displayWidth = 700;

        #region Page-Control Overhead

        //handles description update from iframe
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split('~');
            string command = args[0];

            switch (command.ToLower())
            {
                case "rebind":

                    if (Atx.CurrentShowRecord.ImageManager != null)
                    {
                        // we may need to delete all thumbs first
                        Atx.CurrentShowRecord.ImageManager.CreateAllThumbs();
                    }


                    BindEditorControls();
                    break;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            wctMain.Admin.AdminEvent.ShowChosen += new AdminEvent.ShowChosenEvent(RefreshCurrent);
        }

        public override void Dispose()
        {
            wctMain.Admin.AdminEvent.ShowChosen -= new AdminEvent.ShowChosenEvent(RefreshCurrent);
            base.Dispose();
        }

        protected void RefreshCurrent(object sender, EventArgs e)
        {
            BindEditorControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentShowRecord == null)
                base.Redirect("/Admin/_shows/ShowDirector.aspx");

            firstShowDate = Atx.CurrentShowRecord.FirstShowDate;

            if (!IsPostBack)
            {
                BindEditorControls();
            }     
        }

        protected void BindEditorControls()
        {
            frmImaging.DataBind();            
        }

        #endregion

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Atx.CurrentShowRecord.ImageManager_Delete();

            BindEditorControls();
        }

        #region Details

        protected void frmImaging_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            List<Show> list = new List<Show>();

            if (Atx.CurrentShowRecord != null)
                list.Add(Atx.CurrentShowRecord);

            form.DataSource = list;
            form.DataKeyNames = new string[] { "Id" };
        }

        protected void frmImaging_DataBound(object sender, EventArgs e)
        {
            BindImageDisplay(sender);
        }

        protected void frmSubscriptionEmail_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)
                form.DataBind();
        }

        protected void BindImageDisplay(object sender)
        {
            FormView form = (FormView)sender;
            Literal litImageFromText = (Literal)form.FindControl("litImageFromText");
            Literal litImage = (Literal)form.FindControl("litImage");
                        
            int width = 0;
            int height = 0;

            if (Atx.CurrentShowRecord.ImageManager != null)
            {
                width = Atx.CurrentShowRecord.PicWidth;
                height = Atx.CurrentShowRecord.PicHeight;
            }
                    
            if (litImageFromText != null)
            {
                //indicate portrait or landscape and ratio
                string aspect = (Atx.CurrentShowRecord.ImageManager == null) ? "no image" : 
                    (Atx.CurrentShowRecord.IsPortrait) ? "portrait" : 
                    (Atx.CurrentShowRecord.IsLandscape) ? "landscape" : "square";

                try
                {
                    litImageFromText.Text = string.Format("<li class=\"list-group-item list-group-item-info\"><span class=\"{2}\">{0}: {1}</span>&nbsp;&nbsp;&nbsp;<span class=\"{3}\">w: {4} &nbsp; h: {5}</span></li>",
                        aspect,
                        (Atx.CurrentShowRecord.ImageManager == null) ? "0.0" : Atx.CurrentShowRecord.ImageRatio.ToString("n4"),
                        (Atx.CurrentShowRecord.ImageManager == null) ? "" :
                            ((aspect == "landscape" && (Atx.CurrentShowRecord.ImageRatio > 1.5M)) ||
                            (aspect == "portrait" && (Atx.CurrentShowRecord.ImageRatio < .7M))) ? "aspect-warning" : string.Empty,
                        (width < 1024 || height < 1024) ? "aspect-warning" : string.Empty,
                        width.ToString(),
                        height.ToString()
                        );
                }
                catch(Exception ex)
                {
                    _Error.LogException(ex);
                    litImageFromText.Text = string.Format("<li>image error</li>");
                }
            }        
        }

        protected void CheckCtr_CheckChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            if (chk.ID.IndexOf("chkCtrY") != -1)
            {
                Atx.CurrentShowRecord.Centered_Y = chk.Checked;
                Atx.CurrentShowRecord.Save();                
            }
            else if (chk.ID.IndexOf("chkCtrX") != -1)
            {
                Atx.CurrentShowRecord.Centered_X = chk.Checked;
                Atx.CurrentShowRecord.Save();                
            }

            Atx.ResetCurrentShowRecord();

            ((FormView)chk.NamingContainer).DataBind();
        }

        #endregion
}
}//482
