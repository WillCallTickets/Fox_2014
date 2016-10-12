using System;
using System.Web.UI;

using wctMain.Admin;

namespace wctMain.Interfaces
{
    /// <summary>
    /// Summary description
    /// </summary>
    public interface IPrincipalPicker
    {
        string Title { get; set; }
        void DataBind();
    }
}