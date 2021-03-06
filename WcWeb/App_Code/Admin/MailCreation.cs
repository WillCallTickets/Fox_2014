﻿using System;
using System.Data;

using Wcss;

namespace wctMain.Admin
{
    /// <summary>
    /// Summary description for MailCreation
    /// </summary>
    public class MailCreation
    {
        /// <summary>
        /// returns the id of the subscriptionEmail
        /// </summary>
        public static int CreateEmailLetterAndSubscriptionEmail(
            int subscriptionId, string emailLetterName,
            string subject, string styleContent,
            string htmlVersion, string textVersion)
        {
            //call api to parse info
            /*
             * try - catch - etc
             * 
             * get html with inline style and text version
             * 
             * after this style should be set to empty
             * 
             * 
             * 
             * 
             * =====================================
             * if the method call fails - rely on previous methods to add analytics to links
             * 
             * create another method to add in links
             * 
             */


            //create an email letter
            //bodyname, bodyfile, subject, bodyEncoding(Html)
            SubSonic.QueryCommand qry = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            qry.Parameters.Add("@autoName", emailLetterName);
            qry.Parameters.Add("@styleContent", styleContent);
            qry.Parameters.Add("@htmlVersion", htmlVersion);
            qry.Parameters.Add("@textVersion", textVersion);
            qry.Parameters.Add("@subject", subject);
            qry.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);

            sb.Append("INSERT INTO EmailLetter([ApplicationId], [Name], [StyleContent], [HtmlVersion], [TextVersion], [Subject]) ");
            sb.Append("VALUES (@appId, @autoName, @styleContent, @htmlVersion, @textVersion, @subject) ");
            sb.Append("SELECT SCOPE_IDENTITY() ");

            qry.CommandSql = sb.ToString();

            int inserted = int.Parse(SubSonic.DataService.ExecuteScalar(qry).ToString());

            //create a subscriptionEmail
            sb.Length = 0;
            qry.Parameters.Clear();
            qry.CommandSql = string.Empty;

            qry.Parameters.Add("@subId", subscriptionId, DbType.Int32);
            qry.Parameters.Add("@emlId", inserted, DbType.Int32);

            //create a unique GUID based filename for this subscriptionemail
            qry.Parameters.Add("@path", SubscriptionEmail.ConstructPostedFileName());

            sb.Append("INSERT INTO SubscriptionEmail([TSubscriptionId], [TEmailLetterId], [PostedFileName], [CssFile]) ");
            sb.Append("VALUES (@subId, @emlId, @path, '') ");
            sb.Append("SELECT SCOPE_IDENTITY() ");

            qry.CommandSql = sb.ToString();

            int subscriptionEmailId = int.Parse(SubSonic.DataService.ExecuteScalar(qry).ToString());

            _Lookits.RefreshLookup(_Enums.LookupTableNames.Subscriptions.ToString());

            return subscriptionEmailId;
        }
    }
}