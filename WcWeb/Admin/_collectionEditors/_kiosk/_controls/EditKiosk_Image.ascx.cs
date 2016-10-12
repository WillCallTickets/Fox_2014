using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using WctControls.WebControls;

using System.IO;

namespace wctMain.Admin._collectionEditors._kiosk._controls
{   
    [ToolboxData("<{0}:EditKiosk_Image runat='Server'></{0}:EditKiosk_Image>")]
    public partial class EditKiosk_Image : wctMain.Controller.AdminBase.PrincipaledCollectionContainerItem
    {
        //declare local props
        protected int displayWidth = 500;
        protected decimal cropRatio = 1.77M;//1920x1080 (1080p) 1.77 1.6M;//1440x900 = 1.6 - 


        //assign base class items
        protected override WctControls.WebControls.ErrorDisplayLabel errorDisplay
        {
            get { return this.ErrorDisplayLabel1; }
        }

        protected override System.Web.UI.WebControls.FormView formEntity
        {
            get { return this.formEnt; }
        }

        #region Form Events
        
        protected override void formEntity_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Kiosk ent = (Kiosk)form.DataItem;
            Literal litImageEditBox = (Literal)form.FindControl("litImageEditBox");

            if (litImageEditBox != null)
            {
                decimal ratio = (ent.PicWidth > 0) ? (decimal)displayWidth/(decimal)ent.PicWidth : 0;
                int height = (int)(ent.PicHeight * ratio);

                //NOTE: Styled dimensions do not work because jcrop takes over
                // and panels interefere
                litImageEditBox.Text = 
                    (ent.ImageManager == null) ? 
                    "There is no image for this kiosk."
                    :
                    string.Format("<img id=\"imageeditbox\" src=\"{0}?{3}\" alt=\"\" width=\"{1}px\" height=\"{2}px\" />",
                    ent.ImageManager.OriginalUrl,
                    displayWidth.ToString(), height.ToString(),
                    DateTime.Now.Ticks.ToString()
                    );
            }
        }

        protected override void formEntity_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            ErrorDisplayLabel error = ErrorDisplayLabel1;
            Kiosk ent = (Kiosk)BindingEntity;

            if (ent != null && error != null)
            {
                error.ResetErrors();
                
                try
                {
                    ////ent.Centered_X = (bool)e.NewValues["Centered_X"];
                    //ent.Centered_Y = (bool)e.NewValues["Centered_Y"];

                    if (ent.IsDirty)
                    {
                        ent.Save();
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    error.ErrorList.Add(ex.Message);
                    DisplayUpdateErrors();
                    return;
                }
            }

            BindParentContainer();
        }

        protected override void formEntity_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;

            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "import":
                    DropDownList ddl = (DropDownList)form.FindControl("ddlShowImageImport");
                    if (ddl != null && ddl.SelectedValue != "0")
                    {   
                        try
                        {
                            CopyShowImage(ddl);
                            BindParentContainer();
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);

                            ErrorDisplayLabel error = ErrorDisplayLabel1;
                            error.ErrorList.Add(ex.Message);
                            error.DisplayErrors();
                        }
                    }

                    break;
            }
        }

        #endregion

        #region Not implemented in base class

        protected void SqlShowList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
            e.Command.Parameters["@startDate"].Value = DateTime.Now.Date;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("CREATE TABLE #tmpShowIds(ShowId int); INSERT #tmpShowIds(ShowId) ");
            sb.Append("SELECT DISTINCT(s.[Id]) AS 'ShowId' FROM [ShowDate] sd, [Show] s ");
            sb.Append("WHERE sd.[dtDateOfShow] > @startDate AND sd.[tShowId] = s.[Id] AND s.[ApplicationId] = @appId ");
            sb.Append("IF EXISTS (SELECT * FROM [#tmpShowIds]) BEGIN  ");
            sb.Append("SELECT ' [ Select Show ]' as ShowName, 0 as ShowId UNION   ");
            sb.Append("SELECT s.[Name] + ' - ' +  ");
            sb.Append("ISNULL(v.[City],'') + ' ' + ISNULL(v.[State],'') as ShowName, s.[Id] as ShowId  ");
            sb.Append("FROM #tmpShowIds ids, Show s LEFT OUTER JOIN [Venue] v ON s.[tVenueId] = v.[Id]  ");
            sb.Append("WHERE ids.[ShowId] = s.[Id] AND s.[ApplicationId] = @appId  ");
            sb.Append("ORDER BY ShowName ASC END ELSE BEGIN SELECT  ' [..NO Shows..]' as ShowName, 0 as ShowId END ");

            e.Command.CommandText = sb.ToString();
        }

        /// <summary>
        /// Caller will need to save changes
        /// </summary>
        /// <param name="advertId"></param>
        /// <param name="showId"></param>
        protected void CopyShowImage(DropDownList ddl)
        {
            string mappedDestination = string.Empty;

            try
            {
                Show s = Show.FetchByID(int.Parse(ddl.SelectedValue));

                if (s == null)
                    throw new Exception("The selected show could not be found.");

                if (s.DisplayUrl == null || s.DisplayUrl.Trim().Length == 0)
                    throw new Exception("The selected show does not have an assigned image.");

                if(s.ImageManager == null)
                    throw new Exception("The selected show does not have an image manager.");

                string mappedShowImage = Server.MapPath(s.Url_Original);

                if(!File.Exists(mappedShowImage))
                    throw new Exception("The selected show does not have an existing image.");

             
                Kiosk ent = (Kiosk)this.BindingEntity;
                ent.ImageManager_Delete(false);

                //copy to new location
                mappedDestination = Server.MapPath(
                    string.Format("{0}{1}",
                        _Config._AdvertImageStorage_Local,
                        string.Format("kskimg{0}{1}", ent.Id.ToString(), Path.GetExtension(s.DisplayUrl))
                        ));

                File.Copy(mappedShowImage, mappedDestination);

                ent.Centered_X = s.Centered_X;
                ent.Centered_Y = s.Centered_Y;
                ent.ImageManager_AssignNewImage(mappedDestination);

            }
            catch (OutOfMemoryException)
            {
                if (File.Exists(mappedDestination))
                    File.Delete(mappedDestination);

                throw new System.ArgumentOutOfRangeException(string.Format("An Image file could not be created from the file specified - \"{0}\" ", mappedDestination));
            }
            catch (Exception ex)
            {
                if (File.Exists(mappedDestination))
                    File.Delete(mappedDestination);

                Wcss._Error.LogException(ex);

                throw ex;
            }
        }

        #endregion
    }
}
