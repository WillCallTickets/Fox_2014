using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

using Wcss;
using wctMain.Controller;


namespace wctMain.Admin._customControls
{
    [ToolboxData("<{0}:CollectionContextPager runat='Server' PagerTitle='' PageSize='' PageIndex='0' ></{0}:CollectionContextPager>")]
    public partial class CollectionContextPager : MainBaseControl, IPostBackEventHandler, wctMain.Interfaces.ICollectionPager
    {
        public void RaisePostBackEvent(string eventArgument)
        {
            List<string> parts = new List<string>();
            parts.AddRange(eventArgument.ToLower().Split('~'));

            if (parts.Count > 0)
            {
                string cmd = parts[0].ToLower();

                switch (cmd)
                {
                    case "pagelink":
                        int arg = int.Parse(parts[1]);
                        OnCollectionPagerChanged(arg - 1);
                        break;
                }
            }
        }

        #region PageEvents && Enums

        /// <summary>
        /// fullfill interface requirement
        /// </summary>
        public string PageButtonClass { get; set; }

        public enum CssClasses
        {
            selectedpage,
            disabled
        }

        public event wctMain.Admin.AdminEvent.CollectionPagerChangedEvent CollectionPagerChanged;
        
        /// <summary>
        /// called for index changes - Caller should rebind control
        /// </summary>
        public void OnCollectionPagerChanged(int newIndex)
        {
            if (newIndex < 0)
                newIndex = 0;

            //when we change page size - reset the page index as well
            PageIndex = newIndex;

            if (CollectionPagerChanged != null) { CollectionPagerChanged(this, new wctMain.Admin.AdminEvent.CollectionPagerEventArgs(PageSize, PageIndex)); }
        }
        
        /// <summary>
        /// called when page size is changed
        /// </summary>
        public void OnCollectionPageSizeChanged(int newPageSize)
        {
            if (newPageSize < 0)
                newPageSize = 0;

            //when we change page size - reset the page index as well
            PageSize = newPageSize;
            PageIndex = 0;

            if (CollectionPagerChanged != null) { CollectionPagerChanged(this, new wctMain.Admin.AdminEvent.CollectionPagerEventArgs(PageSize, PageIndex)); }
        }

        #endregion

        #region Page Overhead

        protected _PrincipalBase.Helpers.CollectionSearchCriteria searchCriteria 
        { 
            get 
            { 
                //TODO: find this value from the director's hidden sortable field
                //string context = this.NamingContainer.NamingContainer
                //this.NamingContainer.NamingContainer.NamingContainer.NamingContainer
                //on second thought - maybe title is the way to go

                //go by pager title
                string title = PagerTitle.ToLower();

                if(title.IndexOf("banner") != -1)
                    return Atx.CollectionCriteria_Banner;
                else if (title.IndexOf("kiosk") != -1)
                    return Atx.CollectionCriteria_Kiosk;
                else if (title.IndexOf("post") != -1)
                    return Atx.CollectionCriteria_Post;
                else if (title.IndexOf("employee") != -1)
                    return Atx.CollectionCriteria_Employee;
                else if (title.IndexOf("faq") != -1)
                    return Atx.CollectionCriteria_Faq;
                else if (title.IndexOf("show") != -1)
                    return Atx.CollectionCriteria_Show;

                //fallback
                return new _PrincipalBase.Helpers.CollectionSearchCriteria();
            }
            set
            {
                string title = PagerTitle.ToLower();

                if (title.IndexOf("banner") != -1)
                    Atx.CollectionCriteria_Banner = value;
                else if (title.IndexOf("kiosk") != -1)
                    Atx.CollectionCriteria_Kiosk = value;
                else if (title.IndexOf("post") != -1)
                    Atx.CollectionCriteria_Post = value;
                else if (title.IndexOf("employee") != -1)
                    Atx.CollectionCriteria_Employee = value;
                else if (title.IndexOf("faq") != -1)
                    Atx.CollectionCriteria_Faq = value;
                else if (title.IndexOf("show") != -1)
                    Atx.CollectionCriteria_Show = value;
            }
        }

        protected void lnkCriteria_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string tmp = searchCriteria.Serialized();

            _PrincipalBase.Helpers.CollectionSearchCriteria criteria =
                new _PrincipalBase.Helpers.CollectionSearchCriteria(rdoStatus.SelectedValue, txtDateStart.Text, txtDateEnd.Text,
                    string.Empty);//terms is set somewhere else - so re-use value?TODO

            string newCookie = criteria.Serialized();

            //always do this for sake of binding
            searchCriteria = criteria;

            //reset the search!
            OnCollectionPagerChanged(0);
        }

        private ITemplate _template;
        [PersistenceMode(PersistenceMode.InnerProperty),
        TemplateContainer(typeof(TemplateControl))]
        public ITemplate Template
        {
            get { return _template; }
            set { _template = value; }
        }

        private string _pagerTitle;
        public string PagerTitle
        {
            get { return _pagerTitle; }
            set { _pagerTitle = value; }
        }

        private int _pgDataSetSize = 10000;
        private int _pgPageIndex = 0;
        private int _pgPageSize = 20;
        //private string _pageButtonClass;
        
        /// <summary>
        /// if we are to use the jquery blockUI
        /// </summary>
        public int DataSetSize { get { return _pgDataSetSize; } set { _pgDataSetSize = value; } }
        
        /// <summary>
        /// zero-based - internally converted to 1 based for link text
        /// </summary>
        public int PageIndex { get { return _pgPageIndex; } set { _pgPageIndex = value; } }
        
        public int PageSize { get { return _pgPageSize; } set { _pgPageSize = value; } }
        
        public int DataPages { get { if (DataSetSize == 0) return 0; return (int)Math.Ceiling((double)DataSetSize / PageSize); } }
        
        public int StartRowIndex { get { return ((PageIndex * PageSize) + 1); } }

        protected override void LoadControlState(object savedState)
        {
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this._pgDataSetSize = (int)ctlState[1];
            this._pgPageIndex = (int)ctlState[2];
            this._pgPageSize = (int)ctlState[3];            
            this._pagerTitle = ctlState[4].ToString();
        }

        protected override object SaveControlState()
        {
            object[] ctlState = new object[6];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = DataSetSize;
            ctlState[2] = PageIndex;
            ctlState[3] = PageSize;
            ctlState[4] = PagerTitle;
            return ctlState;
        }

        protected override void OnInit(EventArgs e)
        {
            if (_template != null)
                _template.InstantiateIn(placeValidation);

            base.OnInit(e);
            this.Page.RegisterRequiresControlState(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if(ddlPageSize.Items.Count == 0)
                ddlPageSize.DataBind();
        }

        #endregion

        public override void DataBind()
        {            
            SetPagerControls();
            rdoStatus.DataBind();

            string terms = (searchCriteria.SearchTerms != null && searchCriteria.SearchTerms.Trim().Length > 0) ? searchCriteria.SearchTerms.Replace("newIdx-", "Id = ").Trim() : null;

            string termLine = (terms != null) ?
                string.Format(" with terms <span class=\"hilit\">{0}</span> ", 
                (terms.Length > 40) ? terms.Substring(0,37) + "..." : terms
                ) : string.Empty;

            string dates = (searchCriteria.StartDate != DateTime.MinValue || searchCriteria.EndDate != DateTime.MaxValue) ?
                string.Format(" from <span class=\"hilit\">{0}</span> to <span class=\"hilit\">{1}</span>", 
                (searchCriteria.StartDate != DateTime.MinValue) ? searchCriteria.StartDate.ToString("MM/dd/yyyy hh:mmtt") : "-", 
                (searchCriteria.EndDate != DateTime.MaxValue) ? searchCriteria.EndDate.ToString("MM/dd/yyyy hh:mmtt") : "-"
                ) : string.Empty;

            //category, criteria status, type, owner
            //employee, faq, banners, kiosks, posts
            string collName = string.Empty;
            if (PagerTitle.IndexOf("banner", StringComparison.OrdinalIgnoreCase) != -1)
                collName = "Banners";
            else if (PagerTitle.IndexOf("employee", StringComparison.OrdinalIgnoreCase) != -1)
                collName = "Employees";
            else if (PagerTitle.IndexOf("faq", StringComparison.OrdinalIgnoreCase) != -1)
                collName = "Faqs";
            else if (PagerTitle.IndexOf("kiosk", StringComparison.OrdinalIgnoreCase) != -1)
                collName = "Kiosks";
            else if (PagerTitle.IndexOf("post", StringComparison.OrdinalIgnoreCase) != -1)
                collName = "Posts";

            litStatus.Text = string.Format("Currently viewing <span class=\"hilit\">{1} {2}</span> owned by <span class=\"hilit\">{3}</span>{5}{0}{4}",
                (PagerTitle.ToLower().IndexOf("faqs") != -1) ? string.Format(" in the <span class=\"hilit\">{0}</span> faq category ", Atx.CurrentFaqCategorySelection) : string.Empty,//use for things such as faq category - add a space if used!
                _Enums.GetDescription((Wcss._Enums.CollectionSearchCriteriaStatusType)Enum
                    .Parse(typeof(Wcss._Enums.CollectionSearchCriteriaStatusType), this.searchCriteria.Status.ToString(), true)),
                collName,
                Atx.CurrentEditPrincipal.ToString(),
                dates,
                (terms != null) ? termLine : string.Empty
                );
        }
        
        #region PageSize

        protected void ddlPageSize_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.Items.Count == 0)
            {
                for (int i = 5; i <= 100; i += 5)
                {
                    ListItem li = new ListItem(i.ToString());

                    if (i == this.PageSize)
                        li.Selected = true;

                    ddl.Items.Add(li);
                }
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            int current = PageSize;
            int newSize = int.Parse(ddl.SelectedValue);

            //workaround a blank display page that occurs when page data is out of new page bounds
            if (current != newSize)
                OnCollectionPageSizeChanged(newSize);//event handles informing the caller - which will rebind control         
        }

        #endregion

        #region List of Links

        private int _itmCount = 7;//max 10 links
        private int _halfMax = 5;
        
        protected void rptPageLink_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            List<ListItem> list = new List<ListItem>(10);

            //set the range
            int startPageNum = ((PageIndex - _halfMax) < 0) ? 1 : PageIndex - _halfMax + 1;
            int endPageNum = (startPageNum + _itmCount - 1);

            //when 1
            //if we have over ten possible pages
                //if the page is > 1 then '...'
            //else
                //1

            string last = "...";
            string first = (DataPages > 10 && startPageNum > 1) ? last : "1";

            for (int i = startPageNum; i <= endPageNum; i++)
                list.Add(new ListItem(
                    (i == startPageNum) ? first : (i == endPageNum) ? last : i.ToString(),
                    i.ToString()//value is equal to the count + index - 1-based
                    ));

            rpt.DataSource = list;
        }
        
        protected void rptPageLink_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            //if it is the current selection - disable the nav
            //highlight the selection
            ListItem li = (ListItem)e.Item.DataItem;
            Literal litLiStart = (Literal)e.Item.FindControl("litLiStart");
            LinkButton btnPage = (LinkButton)e.Item.FindControl("btnPage");

            if (li != null && litLiStart != null && btnPage != null)
            {
                if (li.Value == (PageIndex + 1).ToString())
                    litLiStart.Text = "<li class=\"active\">";
                else if (int.Parse(li.Value) > DataPages)
                    litLiStart.Text = "<li style=\"display:none\">";
                else
                    litLiStart.Text = "<li>";
            }
        }

        #endregion

        protected void nav_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string comm = btn.CommandName.ToLower();

            switch (comm)
            {
                case "firstpage":
                    OnCollectionPagerChanged(0);
                    break;
                case "prevpage":
                    OnCollectionPagerChanged(PageIndex - 1);
                    break;
                case "nextpage":
                    OnCollectionPagerChanged(PageIndex + 1);
                    break;
                case "lastpage":
                    OnCollectionPagerChanged(DataPages - 1);
                    break;
                case "page":
                    int newPage = int.Parse(btn.CommandArgument) - 1;
                    OnCollectionPagerChanged(newPage);
                    break;
            }
        }

        /// <summary>
        /// bind the ddlPageSize, Viewing, 4 main buttons and page repeater
        /// </summary>
        private void SetPagerControls()
        {
            ddlPageSize.DataBind();
            rptPageLink.DataBind();
            
            //first - 
            btnFirst.Enabled = DataPages > 1 && PageIndex > 0;
            btnFirst.CommandName = (btnFirst.Enabled) ? "firstpage" : string.Empty;

            //prev - must be on a page other than first page and must have data
            btnPrev.Enabled = btnFirst.Enabled;
            btnPrev.CommandName = (btnPrev.Enabled) ? "prevpage" : string.Empty;

            //next - determined by having pages and not being on thelast page
            btnNext.Enabled = DataPages > 1 && PageIndex != (DataPages - 1);
            btnNext.CommandName = (btnNext.Enabled) ? "nextpage" : string.Empty;

            //last - 
            btnLast.Enabled = btnNext.Enabled;//offset for zero index
            btnLast.CommandName = (btnLast.Enabled) ? "lastpage" : string.Empty;

            //viewing x out of y
            int lastView = (PageIndex * PageSize) + PageSize;
            if (lastView > DataSetSize)
                lastView = DataSetSize;

            litViewing.Text = string.Format("{0} to {1} of {2}", ((PageIndex * PageSize) + 1).ToString(), lastView.ToString(), DataSetSize.ToString());
        }


        #region Search Criteria 

        protected void rdoStatus_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rdo = (RadioButtonList)sender;

            ListItemCollection coll = new ListItemCollection();

            foreach (_Enums.CollectionSearchCriteriaStatusType s in Enum.GetValues(typeof(_Enums.CollectionSearchCriteriaStatusType)))
                coll.Add(new ListItem(_Enums.GetDescription(s), s.ToString()));

            rdo.DataSource = coll;
            rdo.DataTextField = "text";
            rdo.DataValueField = "value";

            txtDateStart.Text = (searchCriteria.StartDate != DateTime.MinValue) ? searchCriteria.StartDate.ToString("MM/dd/yyyy hh:mmtt") : string.Empty;
            txtDateEnd.Text = (searchCriteria.EndDate != DateTime.MaxValue) ? searchCriteria.EndDate.ToString("MM/dd/yyyy hh:mmtt") : string.Empty;
        }

        protected void rdoStatus_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rdo = (RadioButtonList)sender;
            
            //reset
            rdo.SelectedIndex = -1;

            ListItem criteriaStatus = rdo.Items.FindByValue(searchCriteria.Status.ToString());
            if (criteriaStatus != null)
                criteriaStatus.Selected = true;

            ListItem orderableItem = rdo.Items.FindByValue(_Enums.CollectionSearchCriteriaStatusType.orderable.ToString());
            if(orderableItem != null)
                orderableItem.Enabled = (Atx.CurrentEditPrincipal != _Enums.Principal.all);
        }

        #endregion

        
}
}