

namespace wctMain.Admin._collectionEditors._faq
{
    public partial class Faq_Director : wctMain.Controller.AdminBase.DirectorBase
    {
        protected override string defaultControlCode { get { return "faqedit"; } }
        protected override string defaultControlPath { get { return @"_controls\Faq_Edit"; } }
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