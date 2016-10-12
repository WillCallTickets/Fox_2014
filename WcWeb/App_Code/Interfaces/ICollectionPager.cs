using System;
using System.Web.UI;

using wctMain.Admin;

namespace wctMain.Interfaces
{
    /// <summary>
    /// Summary description
    /// </summary>
    public interface ICollectionPager
    {
        ITemplate Template { get; set; }
        string PagerTitle { get; set; }
        int DataSetSize { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int DataPages { get; }
        string PageButtonClass { get; set; }
        int StartRowIndex { get; }
        
        event wctMain.Admin.AdminEvent.CollectionPagerChangedEvent CollectionPagerChanged;
        void OnCollectionPagerChanged(int newIndex);
        void OnCollectionPageSizeChanged(int newPageSize);

        void DataBind();
    }
}