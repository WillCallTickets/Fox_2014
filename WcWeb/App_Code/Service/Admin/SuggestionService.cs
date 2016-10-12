using System;
using System.Web.Services;
using System.Collections.Generic;
using System.Text;
using System.Data;

using wctMain.Admin;

namespace wctMain.Service.Admin
{
    /// <summary>
    /// Summary description for SuggestionService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
    public class SuggestionService : System.Web.Services.WebService
    {
        private AdminContext _ctx = null;
        protected AdminContext Ctx 
        { 
            get 
            {
                if (_ctx == null)
                    _ctx = new AdminContext();

                return _ctx; 
            } 
        }

        public SuggestionService() {}

        private string _actSql = null;
        private string _promoterSql = null;
        private string _venueSql = null;
        private string _searchSql = null;

        public static List<string> GetSearchTypeahead(string s)
        {
            SuggestionService ss = new SuggestionService();
            
            //todo - convert to json
            return ss.Name_Suggestions(s, 10, "search");
        }

        protected string GetSuggestionSql(string context)
        {
            string tableName = "Act";

            switch (context.ToLower())
            {
                case "act":
                    if (_actSql != null)
                        return _actSql;
                    tableName = "Act";
                    break;
                case "promoter":
                    if (_promoterSql != null)
                        return _promoterSql;
                    tableName = "Promoter";
                    break;
                case "venue":
                    if (_venueSql != null)
                        return _venueSql;
                    tableName = "Venue";
                    break;
                case "search":
                    if (_searchSql != null)
                        return _searchSql;
                    tableName = "Act";
                    break;
            }

            StringBuilder sb = new StringBuilder();

            //sb.Append("SELECT Suggestion, Id FROM (	 SELECT	Distinct(a.[Name]) as 'Suggestion', a.[Id] as 'Id', ROW_NUMBER() OVER (ORDER BY a.[NameRoot] ASC) AS RowNum ");
            //sb.AppendFormat("FROM	[{0}] a WHERE a.[ApplicationId] = @appId AND CASE @SearchLike WHEN 0 THEN ", tableName);
            //sb.Append("CASE WHEN (@Hint = 't' OR @Hint = 'th' OR @Hint = 'the' OR @Hint = 'the ') THEN ");
            //sb.Append("CASE WHEN (a.[Name] >= @Hint) OR (a.[NameRoot] >= @Hint) THEN 1 ELSE 0 END ");
            //sb.Append("ELSE CASE WHEN (a.[NameRoot] >= @Hint) THEN 1 ELSE 0 END END ELSE ");
            //sb.Append("CASE WHEN (a.[Name] LIKE @Hint + '%') OR (a.[NameRoot] LIKE @Hint + '%') THEN 1 ELSE 0 END END = 1 ) Suggestions ");
            //sb.Append("WHERE Suggestions.RowNum BETWEEN 0 AND @ResultSize ");

            sb.AppendFormat("FROM	[{0}] a WHERE a.[ApplicationId] = @appId AND ", tableName);            
            sb.Append("CASE WHEN (a.[Name] LIKE @Hint + '%') OR (a.[NameRoot] LIKE @Hint + '%') THEN 1 ELSE 0 END = 1 ) Suggestions ");
            sb.Append("WHERE Suggestions.RowNum BETWEEN 0 AND @ResultSize ");

            switch (context.ToLower())
            {
                case "act":
                    sb.Insert(0, string.Format("SELECT Suggestion, Id FROM (SELECT Distinct(a.[Name]) as 'Suggestion', a.[Id] as 'Id', ROW_NUMBER() OVER (ORDER BY a.[NameRoot] ASC) AS RowNum "));
                    _actSql = sb.ToString();
                    break;
                case "promoter":
                    sb.Insert(0, string.Format("SELECT Suggestion, Id FROM (SELECT Distinct(a.[Name]) as 'Suggestion', a.[Id] as 'Id', ROW_NUMBER() OVER (ORDER BY a.[NameRoot] ASC) AS RowNum "));
                    _promoterSql = sb.ToString();
                    break;
                case "venue":
                    sb.Insert(0, string.Format("SELECT Suggestion, Id FROM (SELECT Distinct(a.[Name]) + CASE WHEN a.[City] IS NOT NULL THEN ' - ' + a.[City] ELSE '' END + CASE WHEN a.[City] IS NOT NULL AND a.[State] IS NOT NULL THEN ', ' + a.[State] ELSE '' END as 'Suggestion', a.[Id] as 'Id', ROW_NUMBER() OVER (ORDER BY a.[NameRoot] ASC) AS RowNum "));
                    _venueSql = sb.ToString();
                    break;
                case "search":                    
                    sb.Insert(0, string.Format("SELECT Suggestion, Id FROM (SELECT Distinct(a.[Name]) as 'Suggestion', a.[Id] as 'Id', ROW_NUMBER() OVER (ORDER BY a.[NameRoot] ASC) AS RowNum "));
                    _searchSql = sb.ToString();
                    break;
            }

            return sb.ToString();
        }

        [WebMethod]
        public List<string> Name_Suggestions(string prefixText, int count, string contextKey)
        {
            List<string> suggestions = new List<string>();
            string sql = GetSuggestionSql(contextKey);
            
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@appId", Wcss._Config.APPLICATION_ID, DbType.Guid);
            cmd.Parameters.Add("@Hint", prefixText);
            cmd.Parameters.Add("@ResultSize", count, System.Data.DbType.Int32);

            try
            {
                using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
                {
                    while (dr.Read())
                        suggestions.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                            dr.GetValue(dr.GetOrdinal("Suggestion")).ToString(), dr.GetValue(dr.GetOrdinal("Id")).ToString()));

                    dr.Close();
                }

                return suggestions;
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                Wcss._Error.LogException(sex);
            }
            catch (Exception ex)
            {
                Wcss._Error.LogException(ex);
            }

            //return an empty result on error?
            return new List<string>();
        }
    }
}