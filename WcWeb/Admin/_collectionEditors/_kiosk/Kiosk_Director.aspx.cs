

namespace wctMain.Admin._collectionEditors._kiosk
{
    public partial class Kiosk_Director : wctMain.Controller.AdminBase.DirectorBase
    {
        protected override string defaultControlCode { get { return "kskedit"; } }
        protected override string defaultControlPath { get { return @"_controls\Kiosk_Edit"; } }
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