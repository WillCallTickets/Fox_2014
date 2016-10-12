using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;

using Wcss;
using wctMain.Controller;

namespace wctMain.Admin._settings._controls
{
    public partial class Errors : MainBaseControl
    {
        StringBuilder sb = new StringBuilder();
        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, "ErrorLog");
        
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            btnArchive.Visible = grid.Rows.Count > 0;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            FormView1.DataBind();
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.SelectedIndex = 0;
        }


        protected void FormView1_DataBound(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@ApplicationName"].Value = Wcss._Config.APPLICATION_NAME;
        }

        protected void btnArchive_Click(object sender, EventArgs e)
        {
            sb.Length = 0;//INSERT LogArchive(Id, [Date], [Source], [Message], [Form], [Querystring], [TargetSite], [StackTrace], [Referrer], [IpAddress], [Email], [ApplicationName]) ");
            sb.Append("CREATE TABLE #tmpLog(Idx int); INSERT #tmpLog(Idx) ");
            sb.Append("SELECT l.[Id] as 'Idx' FROM [Log] l WHERE l.[ApplicationName] = @appName; ");

            sb.Append("INSERT [LogArchive] ");
            sb.Append("SELECT l.* FROM [Log] l, [#tmpLog] tl ");
            sb.Append("WHERE tl.[Idx] NOT IN (SELECT [Id] FROM [LogArchive]) AND l.[Id] = tl.[Idx]; ");

            sb.Append("DELETE FROM [Log] WHERE [Id] IN (SELECT [Idx] FROM #tmpLog); ");

            sb.Append("DROP TABLE #tmpLog; ");

            //cmd.Provider.con
            string name = cmd.ProviderName;
            cmd.CommandSql = sb.ToString();
            cmd.Parameters.Add("@appName", _Config.APPLICATION_NAME);

            try
            {
                SubSonic.DataService.ExecuteQuery(cmd);

                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            GridView1.DataBind();
        }
    }
}