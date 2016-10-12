using System;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;

using Wcss;
using wctMain.Controller;

namespace wctMain.View
{
    public partial class FaqView : MainBaseControl
    {
        public string url { get; set; }
        public string context { get; set; }

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
            if (context == null || context.Trim().Length == 0)
                context = "general";

            if (!IsPostBack)
                BindControls();
        }

        public void BindControls()
        {
            ListView1.DataBind();
        }

        protected void ListView1_DataBinding(object sender, EventArgs e)
        {
            ListView view = (ListView)sender;

            if (view.Items.Count == 0 || view.DataSource == null)
            {
                FaqCategorieCollection coll = new FaqCategorieCollection();
                coll.AddRange(_Lookits.FaqCategories.GetList().FindAll(delegate(FaqCategorie match) { return (match.IsActive); }));
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                view.DataSource = coll;

                Repeater rpt = (Repeater)view.FindControl("rptTabNav");
                if (rpt != null)
                {
                    rpt.DataSource = coll;
                    rpt.DataBind();
                }
            }

            string[] keyNames = { "Id" };
            view.DataKeyNames = keyNames;
        }

        protected void rptTabNav_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            FaqCategorie ent = (FaqCategorie)e.Item.DataItem;
            Literal lit = (Literal)e.Item.FindControl("litItem");

            if (ent != null && lit != null)
            {
                lit.Text = string.Format("<li{0}><a href=\"#{1}\" rel=\"/faq/{1}\" class=\"btn\" data-toggle=\"tab\">{2}</a></li>",
                    (context == ent.Name.ToLower()) ? " class=\"active\"" : string.Empty,
                    ent.Name.ToLower(),
                    ent.Display_Preferred);
            }
        } 

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView control = (ListView)sender;
            FaqCategorie ent = (FaqCategorie)e.Item.DataItem;
            Literal lit = (Literal)e.Item.FindControl("litTabPaneStart");

            if (ent != null && lit != null)
            {
                lit.Text = string.Format("<div class=\"tab-pane fade{0}\" id=\"{1}\">",
                    (context == ent.Name.ToLower()) ? " active in" : string.Empty,
                    ent.Name.ToLower());

                Repeater rpt = (Repeater)e.Item.FindControl("rptFaqs");
                
                FaqItemCollection coll = new FaqItemCollection();
                coll.AddRange(ent.FaqItemRecords().GetList().FindAll(delegate(FaqItem match) { return (match.IsActive && 
                    (new FaqItem_Principal(match).HasPrincipal(Ctx.WebContextPrincipal))
                    ); }));

                List<FaqItem> list = new List<FaqItem>(coll);

                FaqItem_Principal.SortListByPrincipal(Ctx.WebContextPrincipal, ref list);


                if (rpt != null && list.Count > 0)
                {
                    rpt.DataSource = list;
                    rpt.DataBind();
                }
            }
        }

        int _itemCounter = 0;
        string _categorie = string.Empty;

        protected void rptFaqs_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            _itemCounter = 0;
            _categorie = string.Empty;
        }

        protected void rptFaqs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            FaqItem ent = (FaqItem)e.Item.DataItem;
            Literal itemtoggle = (Literal)e.Item.FindControl("litItemToggle");
            Literal itemcontainer = (Literal)e.Item.FindControl("litItemContainer");

            if (e.Item.ItemType == ListItemType.Header)
            {
                _categorie = ((List<FaqItem>)rpt.DataSource)[0].FaqCategorieRecord.Name.ToLower();

                Literal header = (Literal)e.Item.FindControl("litHeader");
                
                if (header != null)
                    header.Text = string.Format("<div class=\"panel-group\" id=\"accordion{0}\">", 
                        _categorie);
            }
            else if (itemtoggle != null && itemcontainer != null)
            {
                itemtoggle.Text = string.Format("<a class=\"accordion-toggle\" data-toggle=\"collapse\" data-parent=\"#accordion{0}\" href=\"#collapse{0}{1}\">",
                    _categorie,
                    _itemCounter.ToString());

             
                itemcontainer.Text = string.Format("<div id=\"collapse{0}{1}\" class=\"panel-collapse collapse{2}\">",
                    _categorie,
                    _itemCounter.ToString(),
                    //(_itemCounter == 0) ? " in" : 
                    string.Empty );

                _itemCounter++;
            }
        }
    }
}
