using System;
using System.Web.UI;

using Wcss;

//<div class="m-social" data-bind="text: $data.MobileSocial"></div>

namespace wctMain.View.Partials
{
    [ToolboxData("<{0}:UpcomingShowList runat='server' ></{0}:UpcomingShowList>")]
    public partial class UpcomingShowList : wctMain.Controller.MainBaseControl
    {
        #region Props

        private string _menuTitle = null;
        protected string menuTitle
        {
            get
            {
                if (_menuTitle == null)
                    _menuTitle = (_Config._DisplayMenu_IncludeBTEvents) ? "Z2Ent Events" : _Config._DisplayMenu_Title;

                return _menuTitle;
            }
        }
        
        #endregion

        #region Page Loop and Main Logic

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
        }

        #endregion
    }
}