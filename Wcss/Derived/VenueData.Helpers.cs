using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wcss.VenueData
{
    // STUB
    //namespace Interfaces
    //{
    //    public class AdminInterfaces
    //    {
    //        public interface IPrincipalOrderable
    //        {
    //            static object UpdateOrderable(string newOrder);
    //        }
    //    }

    //}

    public class Helpers
    {
        //No methods need to be defined? Grants a way to find the control
        public interface IExplicitBinder
        {
            void ExplicitBind();            
        }

        /// <summary>
        /// TODO reimplement
        /// </summary>
        public interface IReBindingShowControl : IExplicitBinder
        {
            Show GetBindingShowRecord();
        }

        public interface IBindingIPrincipalControl : IExplicitBinder
        {
            Wcss._PrincipalBase.IPrincipal GetBindingIPrincipal();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal">Either ALL or a single principal</param>
        /// <param name="numShows"></param>
        /// <param name="minDate"></param>
        /// <returns></returns>
        public static List<System.Web.UI.WebControls.ListItem> ShowList_Get(_Enums.Principal principal, int numShows, DateTime startDate, string title)
        {
            if(title == null)
                title = "Select Show";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("CREATE TABLE #tmpShowIds(ShowId int); INSERT #tmpShowIds(ShowId) ");
            sb.AppendFormat("SELECT DISTINCT TOP {0} (s.[Id]) AS 'ShowId' FROM [ShowDate] sd, [Show] s ", numShows.ToString());


            sb.Append("WHERE ");            
            if(principal != _Enums.Principal.all)
                sb.AppendFormat("CHARINDEX({0}, s.[vcPrincipal]) >= 1 AND ", principal.ToString());            
            sb.Append("s.[ApplicationId] = @appId AND ");
            sb.Append("sd.[dtDateOfShow] >= @startDate AND sd.[tShowId] = s.[Id]  ");


            sb.Append("IF EXISTS (SELECT * FROM [#tmpShowIds]) BEGIN  ");
            sb.AppendFormat("SELECT ' [..{0}..]' as [ShowName], 0 as [ShowId] UNION   ", title);
            sb.Append("SELECT s.[Name] + ' - ' +  ");
            sb.Append("ISNULL(v.[City],'') + ' ' + ISNULL(v.[State],'') as [ShowName], s.[Id] as [ShowId]  ");
            sb.Append("FROM [#tmpShowIds] ids, [Show] s LEFT OUTER JOIN [Venue] v ON s.[tVenueId] = v.[Id]  ");
            sb.Append("WHERE ids.[ShowId] = s.[Id] AND s.[ApplicationId] = @appId  ");
            sb.Append("ORDER BY [ShowName] ASC END ELSE BEGIN SELECT  ' [..NO Shows..]' as [ShowName], 0 as ShowId END ");


            _DatabaseCommandHelper dch = new _DatabaseCommandHelper(sb.ToString());

            dch.AddCmdParameter("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
            dch.AddCmdParameter("@startDate", startDate, System.Data.DbType.Date);

            List<System.Web.UI.WebControls.ListItem> list = new List<System.Web.UI.WebControls.ListItem>();

            try{
                list.AddRange(dch.LoadListItemList("ShowName", "ShowId"));
            }
            catch(Exception ex)
            {
                _Error.LogException(ex);
            }

            return list;
        }
    }
}