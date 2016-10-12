

namespace wctMain.Admin._collectionEditors._post
{
    public partial class Post_Director : wctMain.Controller.AdminBase.DirectorBase
    {
        protected override string defaultControlCode { get { return "pstedit"; } }
        protected override string defaultControlPath { get { return @"_controls\Post_Edit"; } }
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