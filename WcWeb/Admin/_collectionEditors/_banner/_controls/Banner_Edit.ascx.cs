using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Admin._collectionEditors._banner._controls
{
    public partial class Banner_Edit : wctMain.Controller.AdminBase.PrincipaledCollectionBaseEditor
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
        protected override string preRenderKey { get { return "banneredit"; } }
        protected override _PrincipalBase.Helpers.CollectionSearchCriteria searchCriteria { get { return Atx.CollectionCriteria_Banner; } set { Atx.CollectionCriteria_Banner = value; } }
        protected override void FormatListingInfo(_PrincipalBase.IPrincipal entity, Literal lit)
        {
            SalePromotion ent = (SalePromotion)entity;

            if (ent != null && lit != null)
            {
                //image
                lit.Text = (ent.BannerUrl != null && ent.BannerUrl.Trim().Length > 0) ? string.Format("<img src=\"{0}\" width=\"275\" height=\"35px\" title=\"{1}\" />",
                    string.Format("{0}://{1}/{2}/Images/Banners/{3}", this.Request.Url.Scheme, _Config._DomainName, _Config._VirtualResourceDir, ent.BannerUrl),
                    ent.Name) :
                    "<div style=\"display:inline-block;width:275;\">&nbsp;</div>";
            }
        }

        #region ListView

        protected override void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            base.listEnt_ItemDataBound(sender, e);

            ListView cont = (ListView)sender;
            SalePromotion ent = (e.Item.DataItem != null) ? (SalePromotion)e.Item.DataItem : null;

            if (ent != null && cont.EditIndex != -1 && e.Item.DisplayIndex == cont.EditIndex)
            {
                EditBanner_Container container = (EditBanner_Container)e.Item.FindControl("EditBanner_Container1");
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
                    if (e.Values["DisplayText"] == null || e.Values["DisplayText"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("DisplayText is required.");

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


                    SalePromotion ent = new SalePromotion();
                    ent.ApplicationId = _Config.APPLICATION_ID;
                    ent.DtStamp = DateTime.Now;
                    ent.VcPrincipal = _PrincipalBase.PrincipalListToString(principalSelections);
                    ent.Name = e.Values["Name"].ToString().Trim();
                    ent.DisplayText = e.Values["DisplayText"].ToString().Trim();
                    ent.AdditionalText = (e.Values["AdditionalText"] != null && e.Values["AdditionalText"].ToString().Trim().Length > 0) ?
                        e.Values["AdditionalText"].ToString().Trim() : null;
                    ent.BannerTimeout = _Config._BannerDisplayTime;
                    ent.IsActive = false;

                    ent.Save();


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

            Atx.CurrentBannerRecord = null;
            Atx.CurrentBannerRecord = new SalePromotion((int)list.DataKeys[list.EditIndex].Value);
        }

        protected override void listEnt_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            SalePromotion.Delete(e.Keys["Id"]);
            base.listEnt_ItemDeleting(sender, e);
        }

        #endregion

        #region Page-Control Overhead

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
                    int idx = Atx.CurrentBannerRecord.Id;
                    Atx.CurrentBannerRecord = null;
                    Atx.CurrentBannerRecord = new SalePromotion(idx);
                    ExplicitBind();
                    break;
                case "rebind":
                    ExplicitBind();
                    break;
            }
        }

        #endregion
        
}
}