

namespace wctMain.Admin._collectionEditors._employee
{
    public partial class Employee_Director : wctMain.Controller.AdminBase.DirectorBase
    {
        protected override string defaultControlCode { get { return "empedit"; } }
        protected override string defaultControlPath { get { return @"_controls\Employee_Edit"; } }
        protected override System.Web.UI.WebControls.Panel controlPanel
        {
            get { return this.Content; }
        }
        protected override System.Web.UI.WebControls.HiddenField hdnCollectionName
        {
            get { return null; }
        }
    }
}