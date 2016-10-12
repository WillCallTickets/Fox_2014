using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Admin._collectionEditors._post._controls
{
    public partial class Post_Edit : wctMain.Controller.AdminBase.PrincipaledCollectionBaseEditor
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
        protected override string preRenderKey { get { return "postedit"; } }
        protected override _PrincipalBase.Helpers.CollectionSearchCriteria searchCriteria { get { return Atx.CollectionCriteria_Post; } set { Atx.CollectionCriteria_Post = value; } }
        protected override void FormatListingInfo(_PrincipalBase.IPrincipal entity, Literal lit)
        {
            Post ent = (Post)entity;

            if (ent != null && lit != null)
            {
                lit.Text = string.Format("{1} <span style=\"display:inline-block;width:200px;position:relative;top:5px;height:20px;overflow:hidden;font-weight:bold;\">{0}</span>",
                    ent.Name,
                    string.Format("<input type=\"checkbox\" class=\"\" disabled=\"disabled\" name=\"activus_{0}\" id=\"activus_{0}\" {1}>",
                        ent.Id.ToString(), (ent.IsActive) ? "checked=\"checked\"" : string.Empty)
                        ).Trim();

                lit.Text += string.Format("<span style=\"display:inline-block;width:100px;margin-left:10px;\"><small><b>{0}</b></small></span>", ent.VcPrincipal);
                lit.Text += string.Format("<span style=\"display:inline-block;width:200px;margin-left:10px;white-space:nowrap;overflow-x:hidden;position:relative;top:8px;\"><small><b>title</b> {0}</small></span>", ent.Slug ?? string.Empty);
            }
        }

        #region ListView

        protected override void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            base.listEnt_ItemDataBound(sender, e);

            ListView cont = (ListView)sender;
            Post ent = (e.Item.DataItem != null) ? (Post)e.Item.DataItem : null;

            if (ent != null && cont.EditIndex != -1 && e.Item.DisplayIndex == cont.EditIndex)
            {
                EditPost_Container container = (EditPost_Container)e.Item.FindControl("EditPost_Container1");
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


                    Post ent = new Post();
                    ent.DtStamp = DateTime.Now;
                    ent.DtModified = DateTime.Now;
                    ent.VcPrincipal = _PrincipalBase.PrincipalListToString(principalSelections);
                    ent.IsActive = false;
                    ent.Name = e.Values["Name"].ToString().Trim();
                    ent.Description = string.Empty;
                    ent.Slug = Post.FormatSlug(ent.Name);
                    ent.ValueX = string.Empty;

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

            Atx.CurrentPostRecord = null;
            Atx.CurrentPostRecord = new Post((int)list.DataKeys[list.EditIndex].Value);
        }


        protected override void listEnt_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            Post.Delete(e.Keys["Id"]);
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
                    int idx = Atx.CurrentPostRecord.Id;
                    Atx.CurrentPostRecord = null;
                    Atx.CurrentPostRecord = new Post(idx);
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
