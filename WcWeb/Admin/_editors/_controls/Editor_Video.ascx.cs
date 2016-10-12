using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;

using Newtonsoft.Json;

using Wcss;
using wctMain.Controller;
using WctControls.WebControls;

namespace wctMain.Admin._editors._controls
{
    public partial class Editor_Video : MainBaseControl
    {
        #region Page Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtPlaylistId.DataBind();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SiteConfig config = _Lookits.SiteConfigs.GetList().Find(delegate(SiteConfig match) { return (match.Name.ToLower() == "youtubeplaylist"); });
            if (config != null)
            {
                config.ValueX = txtPlaylistId.Text.Trim();
                config.Save();
            }

            txtPlaylistId.Text = _Config._YouTubePlaylist;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtPlaylistId.Text = _Config._YouTubePlaylist;
        }

        #endregion
}
}