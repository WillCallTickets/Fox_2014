using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Wcss;
using WctControls.WebControls;

namespace wctMain.Admin._collectionEditors._faq._controls
{
    public partial class Faq_Edit : wctMain.Controller.AdminBase.PrincipaledCollectionBaseEditor
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
        protected override string preRenderKey { get { return "faqedit"; } }
        protected override _PrincipalBase.Helpers.CollectionSearchCriteria searchCriteria { get { return Atx.CollectionCriteria_Faq; } set { Atx.CollectionCriteria_Faq = value; } }
        protected override void FormatListingInfo(_PrincipalBase.IPrincipal entity, Literal lit)
        {
            //SalePromotion ent = (SalePromotion)entity;

            //if (ent != null && lit != null)
            //{
            // //   lit.Text = ;
            //}
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (hdnCollectionTableName != null)
                hdnCollectionTableName.DataBind();

            rptCategorie.DataBind();

            base.Page_Load(sender, e);
        }

        #region ListView

        protected override void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!e.InputParameters.Contains("category"))
                e.InputParameters.Add("category", Atx.CurrentFaqCategorySelection);

            base.objData_Selecting(sender, e);
        }

        protected override void listEnt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            base.listEnt_ItemDataBound(sender, e);

            ListView cont = (ListView)sender;
            FaqItem ent = (e.Item.DataItem != null) ? (FaqItem)e.Item.DataItem : null;

            if (ent != null && cont.EditIndex != -1 && e.Item.DisplayIndex == cont.EditIndex)
            {
                EditFaq_Container container = (EditFaq_Container)e.Item.FindControl("EditFaq_Container1");
                container.ExplicitBind();
            }
            else if (ent != null && e.Item.ItemType == ListViewItemType.DataItem)
            {
                //Literal litInfo = (Literal)e.Item.FindControl("litInfo");
                //FormatListingInfo(ent, litInfo);
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
                    if (e.Values["Question"] == null || e.Values["Question"].ToString().Trim().Length == 0)
                        error.ErrorList.Add("Question is required.");

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


                    FaqItem ent = new FaqItem();                    
                    ent.DtStamp = DateTime.Now;
                    ent.IsActive = false;
                    ent.VcPrincipal = _PrincipalBase.PrincipalListToString(principalSelections);
                    
                    RadioButtonList rdoCategory = (RadioButtonList)e.Item.FindControl("rdoCategory");
                    ent.TFaqCategorieId = int.Parse(rdoCategory.SelectedValue);

                    ent.Question = e.Values["Question"].ToString().Trim();
                    ent.Answer = (e.Values["Answer"] != null) ? e.Values["Answer"].ToString().Trim() : string.Empty;
                    
                    ent.Save();

                    // category - sync
                    _Lookits.RefreshLookup("FaqCategories");
                    Atx.CurrentFaqCategorySelection = rdoCategory.SelectedItem.Text;
                    Atx.CurrentFaqRecord = ent;

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

            Atx.CurrentFaqRecord = null;
            Atx.CurrentFaqRecord = new FaqItem((int)list.DataKeys[list.EditIndex].Value);
        }

        protected override void listEnt_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            FaqItem.Delete(e.Keys["Id"]);
            base.listEnt_ItemDeleting(sender, e);
        }

        #endregion

        #region Page-Control Overhead

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
                    int idx = Atx.CurrentFaqRecord.Id;
                    Atx.CurrentFaqRecord = null;
                    Atx.CurrentFaqRecord = new FaqItem(idx);
                    ExplicitBind();
                    break;
                case "rebind":
                    ExplicitBind();
                    break;
            }
        }

        #endregion

        #region Faq Categories for insert panel

        protected void rdoCategory_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rdo = (RadioButtonList)sender;

            if (listEnt.InsertItemPosition != InsertItemPosition.None)
            {
                ListItemCollection coll = new ListItemCollection();

                foreach (FaqCategorie f in _Lookits.FaqCategories)
                    coll.Add(new ListItem(f.Name, f.Id.ToString()));

                rdo.DataSource = coll;
                rdo.DataTextField = "text";
                rdo.DataValueField = "value";
            }
        }

        protected void rdoCategory_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rdo = (RadioButtonList)sender;

            //set radio to current category context
            if (listEnt.InsertItemPosition != InsertItemPosition.None)
            {
                ListItem li = rdo.Items.FindByText(Atx.CurrentFaqCategorySelection);
                if (li != null)
                    li.Selected = true;
                else
                    rdo.SelectedIndex = 0;
            }
        }

        #endregion

        protected void rptCategorie_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            List<ListItem> list = new List<ListItem>();

            foreach (FaqCategorie f in _Lookits.FaqCategories)
                list.Add(new ListItem(f.Name, f.Id.ToString()));

            rpt.DataSource = list;
        }

        protected void rptCategorie_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptCategorie_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Repeater rpt = (Repeater)source;

            if (e.CommandName.ToLower() == "select")
            {
                if (Atx.CurrentFaqCategorySelection != e.CommandArgument.ToString())
                {
                    Atx.CurrentFaqCategorySelection = e.CommandArgument.ToString();
                    rptCategorie.DataBind();
                    listEnt.DataBind();
                }
            }
        }
}
}