using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Admin._collectionEditors._kiosk._controls
{
    public partial class Kiosk_Edit : wctMain.Controller.AdminBase.PrincipaledCollectionBaseEditor
    {
        protected override wctMain.Interfaces.ICollectionPager collectionContextPager
        {
            get { return this.CollectionContextPager1; }
        }
        protected override System.Web.UI.WebControls.ListView listEntity
        {
            get { return this.listEnt; }
        }
        protected override System.Web.UI.UpdatePanel updatePanel
        {
            get { return this.UpdatePanel1; }
        }
        protected override string preRenderKey { get { return "kioskedit"; } }
        protected override _PrincipalBase.Helpers.CollectionSearchCriteria searchCriteria { get { return Atx.CollectionCriteria_Kiosk; } set { Atx.CollectionCriteria_Kiosk = value; } }
        protected override void FormatListingInfo(_PrincipalBase.IPrincipal entity, Literal lit)
        {
            Kiosk ent = (Kiosk)entity;

            if (ent != null && lit != null)
            {
                if (ent.EventDate != null && ent.EventDate.Trim().Length > 0)
                    lit.Text += string.Format("<span>{0}</span>", ent.EventDate.Trim());
                if(ent.EventVenue != null && ent.EventVenue.Trim().Length > 0)
                    lit.Text += string.Format("<span>{0}</span>", ent.EventVenue.Trim());
                if (ent.EventTitle != null && ent.EventTitle.Trim().Length > 0)
                    lit.Text += string.Format("<span>{0}</span>", ent.EventTitle.Trim());
                if (ent.EventHeads != null && ent.EventHeads.Trim().Length > 0)
                    lit.Text += string.Format("<span>{0}</span>", ent.EventHeads.Trim());
                if (ent.EventOpeners != null && ent.EventOpeners.Trim().Length > 0)
                    lit.Text += string.Format("<span>{0}</span>", ent.EventOpeners.Trim());
                if (ent.EventDescription != null && ent.EventDescription.Trim().Length > 0)
                    lit.Text += string.Format("<span>{0}</span>", ent.EventDescription.Trim());
            }
        }

        #region ListView
                
        protected override void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            base.listEnt_ItemDataBound(sender, e);

            ListView cont = (ListView)sender;
            Kiosk ent = (e.Item.DataItem != null) ? (Kiosk)e.Item.DataItem : null;

            if (ent != null && cont.EditIndex != -1 && e.Item.DisplayIndex == cont.EditIndex)
            {
                EditKiosk_Container container = (EditKiosk_Container)e.Item.FindControl("EditKiosk_Container1");
                container.ExplicitBind();
            }
            else if (ent != null && e.Item.ItemType == ListViewItemType.DataItem)
            {
                Literal litInfo = (Literal)e.Item.FindControl("litInfo");
                FormatListingInfo(ent, litInfo);
            }
        }
        
        protected override void listEnt_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListView list = (ListView)sender;
            ErrorDisplayLabel error = (ErrorDisplayLabel)e.Item.FindControl("ErrorDisplayLabel1");

            if (error != null)
            {
                error.ResetErrors();

                try
                {
                    //Validation
                    if (e.Values["Name"] == null || e.Values["Name"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("Name is required.");

                    List<_Enums.Principal> principalSelections = new List<_Enums.Principal>();

                    CheckBoxList chkPrincipal = (CheckBoxList)e.Item.FindControl("chkPrincipal");
                    if (chkPrincipal != null)
                    {
                        //no need to check for existing items as the list has been newly created
                        foreach (ListItem li in chkPrincipal.Items)
                            if (li.Selected)
                                principalSelections.Add(_PrincipalBase.PrincipalFromString(li.Text));
                    }

                    //we must have at least one principal selected - cannot be all
                    if (principalSelections.Contains(_Enums.Principal.all))
                        error.ErrorList.Add("Belongs To cannot contain ALL.");
                    else if (principalSelections.Count == 0)
                        error.ErrorList.Add("Belongs To must have at least one selection.");


                    if (error.ErrorList.Count > 0)
                    {
                        error.DisplayErrors();
                        e.Cancel = true;
                        return;
                    }


                    Kiosk ent = new Kiosk();
                    ent.ApplicationId = _Config.APPLICATION_ID;
                    ent.DtStamp = DateTime.Now;
                    ent.VcPrincipal = _PrincipalBase.PrincipalListToString(principalSelections);
                    ent.Timeout = _Config._KioskDisplayTime;
                    ent.Name = e.Values["Name"].ToString().Trim();
                    ent.PicWidth = 0;
                    ent.PicHeight = 0;
                    ent.IsActive = false;
                    ent.DateStart = DateTime.Now;

                    //initial save - get the id!
                    ent.Save();


                    //see if we are importing info from a show
                    DropDownList ddl = (DropDownList)e.Item.FindControl("ddlPopulate");

                    if (ddl != null && ddl.SelectedValue != "0")
                    {
                        Show s = new Show(int.Parse(ddl.SelectedValue));

                        ent.TShowId = s.Id;

                        ent.EventVenue = s.VenueRecord.Name_Displayable;

                        //remove the!
                        if (ent.EventVenue.StartsWith("The ", StringComparison.OrdinalIgnoreCase))
                            ent.EventVenue = ent.EventVenue.Remove(0, 4).Trim();

                        if (ent.EventVenue.Trim().Length > 0)
                        {
                            ent.EventVenue = ent.EventVenue.Insert(0, "at ");
                        }

                        ent.EventDate = s.FirstDate.ToString("MMMM dd");
                        ent.EventTitle = s.ShowTitle;
                        ent.EventHeads = s.listHeadliners;
                        ent.EventOpeners = s.listOpeners;
                        ent.DateEnd = s.FirstDate;
                        //skip description

                        //copy image, but don't fail the entire copy if we can't find the image
                        try
                        {
                            string dest = string.Format("{0}{1}{2}{3}",
                                _Config._AdvertImageStorage_Local,
                                "kskimg",
                                ent.Id.ToString(),
                                System.IO.Path.GetExtension(s.ImageManager.OriginalUrl)
                                );

                            string mappedDestination = Server.MapPath(dest);

                            Utils.ImageTools.CopyImageFile(Server.MapPath(s.ImageManager.OriginalUrl),
                                mappedDestination,
                                true);

                            ent.DisplayUrl = System.IO.Path.GetFileName(mappedDestination);
                            ent.PicWidth = s.PicWidth;
                            ent.PicHeight = s.PicHeight;
                            ent.Centered_X = s.Centered_X;
                            ent.Centered_Y = s.Centered_Y;
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);
                        }

                        ent.Save();
                    }


                    //call cleanup & rebind
                    base.listEnt_ItemInserting(sender, e);

                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    error.ErrorList.Add(ex.Message);
                    error.DisplayErrors();

                    e.Cancel = true;
                }
            }
        }

        protected override void SetListEditItem(ListView list, int idx)
        {
            base.SetListEditItem(list, idx);

            Atx.CurrentKioskRecord = null;
            Atx.CurrentKioskRecord = new Kiosk((int)list.DataKeys[list.EditIndex].Value);
        }

        protected override void listEnt_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            Kiosk.Delete(e.Keys["Id"]);
            base.listEnt_ItemDeleting(sender, e);
        }

        #endregion

        #region Page Overhead

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (hdnCollectionTableName != null)
                hdnCollectionTableName.DataBind();
            base.Page_Load(sender, e);
        }

        /// <summary>
        /// If argument contains {"commandName":"selectedIdFromTypeahead","newIdx":"10409"} then use dict
        /// Then handle the command accordingly
        /// </summary>
        /// <param name="eventArgument"></param>
        protected override void HandlePostBackEvent(string eventArgument)
        {
            string commandName = string.Empty;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] args = new string[12];//arbitrary number of indices

            if (eventArgument.IndexOf("commandName") != -1)
            {
                dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(eventArgument);
                commandName = dict["commandName"].ToLower();
            }
            else// if (eventArgument.IndexOf("~") != -1)
            {
                args = eventArgument.Split('~');
                commandName = args[0];
            }

            switch (commandName.ToLower())
            {
                case "selectedidfromtypeahead":
                    this.searchCriteria = new _PrincipalBase.Helpers.CollectionSearchCriteria(
                        _Enums.CollectionSearchCriteriaStatusType.all.ToString(), null, null, string.Format("newIdx-{0}", dict["newIdx"]));
                    ExplicitBind();
                    break;
                case "refresh_post_upload":
                    //refresh current object
                   int idx = Atx.CurrentKioskRecord.Id;
                    Atx.CurrentKioskRecord = null;
                    Atx.CurrentKioskRecord = new Kiosk(idx);
                    ExplicitBind();
                    break;
                case "rebind":
                    ExplicitBind();
                    break;
            }
        }

        #endregion
        
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
}
}
