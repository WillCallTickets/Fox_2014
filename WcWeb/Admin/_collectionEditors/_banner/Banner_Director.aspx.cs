

namespace wctMain.Admin._collectionEditors._banner
{
    public partial class Banner_Director : wctMain.Controller.AdminBase.DirectorBase
    {
        protected override string defaultControlCode { get { return "banneredit"; } }
        protected override string defaultControlPath { get { return @"_controls\Banner_Edit"; } }
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