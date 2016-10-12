using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using SubSonic;

using System.Net.Mail;
using Utils.ExtensionMethods;

namespace Wcss
{
    public partial class MailQueue
    {
        #region Properties

        [XmlAttribute("IsMassMailer")]
        public bool IsMassMailer
        {
            get { return (!this.BMassMailer.HasValue) ? false : this.BMassMailer.Value; }
            set { this.BMassMailer = value; }
        }

        #endregion

        public static void LogSent_Subscription(string cc, string bcc, DateTime toProcess, DateTime processed,
            string fromAddress, string fromName, string toAddress, string status, bool massMailer,
            int priority, int subEmailId)
        {
            LogSentMail(cc, bcc, toProcess, processed, fromAddress, fromName, toAddress, status, massMailer,
                priority, subEmailId, 0);
        }
        public static void LogSent_EmailLetter(string cc, string bcc, DateTime toProcess, DateTime processed,
            string fromAddress, string fromName, string toAddress, string status, bool massMailer,
            int priority, int emailLetterId)
        {
            LogSentMail(cc, bcc, toProcess, processed, fromAddress, fromName, toAddress, status, massMailer,
                priority, 0, emailLetterId);
        }
        public static void LogSent(string cc, string bcc, DateTime toProcess, DateTime processed,
            string fromAddress, string fromName, string toAddress, string status, bool massMailer,
            int priority)
        {
            LogSentMail(cc, bcc, toProcess, processed, fromAddress, fromName, toAddress, status, massMailer,
                priority, 0, 0);
        }
        private static void LogSentMail(string cc, string bcc, DateTime toProcess, DateTime processed,
            string fromAddress, string fromName, string toAddress, string status, bool massMailer, 
            int priority, int subEmailId, int emailLetterId)
        {
            MailQueue q = new MailQueue();
            q.ApplicationId = _Config.APPLICATION_ID;
            q.DtStamp = DateTime.Now;
            q.AttemptsRemaining = 0;
            q.Bcc = bcc;
            q.Cc = cc;
            q.DateProcessed = processed;
            q.DateToProcess = toProcess;
            q.FromAddress = fromAddress;
            q.FromName = fromName;
            q.IsMassMailer = massMailer;
            q.Priority = priority;
            q.Status = status;
            q.ToAddress = toAddress;

            if(subEmailId > 0)
                q.TSubscriptionEmailId = subEmailId;
            if(emailLetterId > 0)
                q.TEmailLetterId = emailLetterId;

            q.Save();
        }

        private static void SetSubscriptionEmailToListCommandParams(SubSonic.QueryCommand cmd, EmailLetter ltr, SubscriptionEmail subEmail,
            DateTime dateToSend, int priority, bool isMassEmail)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
            cmd.Parameters.Add("@letterId", ltr.Id, System.Data.DbType.Int32);
            cmd.Parameters.Add("@subscriptionEmailId", subEmail.Id, System.Data.DbType.Int32);
            cmd.Parameters.Add("@dateToSend", dateToSend, System.Data.DbType.DateTime);
            cmd.Parameters.Add("@fromName", (isMassEmail) ? _Config._MassMailService_FromName : _Config._CustomerService_FromName);
            cmd.Parameters.Add("@fromAddress", (isMassEmail) ? _Config._MassMailService_Email : _Config._CustomerService_Email);
            cmd.Parameters.Add("@priority", priority, System.Data.DbType.Int32);
            cmd.Parameters.Add("@mass", isMassEmail, System.Data.DbType.Boolean);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="now">designated outside of this method as it may be performed in a loop</param>
        /// <param name="emailAddress"></param>
        /// <param name="subscribe"></param>
        /// <param name="serviceCreator">required: badmailservice, page, etc. The client calling this method</param>
        /// <param name="mailerName">null allowed</param>
        /// <param name="subEmail">can be null</param>
        /// <param name="subscriptionId">required</param>
        public static SubscriptionUser EnsureSubscriptionUser(DateTime now, string emailAddress, bool subscribe, 
            string serviceCreator, string mailerName, SubscriptionEmail subEmail, int subscriptionId)
        {
            string creatorName = string.Format("{0}{1}", 
                serviceCreator, 
                (mailerName != null && mailerName.Trim().Length > 0) ? string.Format(": {0}", mailerName) : string.Empty);

            string description = (subEmail != null) ? string.Format("Created by {0} emailsubscription id: {1}", serviceCreator, subEmail.Id.ToString()) :
                string.Format("Created by {0} for subscription id {1}", serviceCreator, subscriptionId.ToString());
            

            //Do this here as opposed to the mailjob - the mailjob is complex enough already               
            AspnetUser au = new AspnetUser();

            _DatabaseCommandHelper dch = new _DatabaseCommandHelper(
                "SELECT * FROM [Aspnet_Users] WHERE [ApplicationId] = @appId AND [UserName] = @email");

            dch.AddCmdParameter("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
            dch.AddCmdParameter("@email", emailAddress, System.Data.DbType.String);

            dch.PopulateCollectionByReader<AspnetUser>(au);

            if (au.UserId == null || au.UserId == Guid.Empty)
            {
                au.ApplicationId = _Config.APPLICATION_ID;
                au.LoweredUserName = emailAddress.ToLower();
                au.LastActivityDate = now;
                au.UserName = emailAddress;                
                au.Save();

                AspnetUsersInRole air = new AspnetUsersInRole();
                air.AspnetRoleRecord = new AspnetRole(AspnetRole.RoleNameColumn.ColumnName, "WebUser");
                air.AspnetUserRecord = au;

                air.Save();

                UserEvent.NewUserEvent(now, now, _Enums.EventQStatus.Success, creatorName,
                    au.UserId, au.LoweredUserName,
                    _Enums.EventQContext.User, _Enums.EventQVerb.UserCreated,
                    null, null, description, true);
            }


            SubscriptionUser su = new SubscriptionUser();

            _DatabaseCommandHelper dch1 = new _DatabaseCommandHelper(
                "SELECT * FROM [SubscriptionUser] WHERE [UserId] = @userId AND [TSubscriptionId] = @subscriptionId");

            dch1.AddCmdParameter("@userId", au.UserId, System.Data.DbType.Guid);
            dch1.AddCmdParameter("@subscriptionId", subscriptionId, System.Data.DbType.Int32);

            dch1.PopulateCollectionByReader<SubscriptionUser>(su);

            if (su.UserId == null || su.UserId == Guid.Empty)
            {
                //record subscription update
                UserEvent.NewUserEvent(now, now, _Enums.EventQStatus.Success, creatorName,
                    au.UserId, au.LoweredUserName,
                    _Enums.EventQContext.User, _Enums.EventQVerb.SubscriptionUpdate,
                    "Subscription created for removal", subscribe.ToString(), description, true);

                su.UserId = au.UserId;
                su.TSubscriptionId = (subEmail != null) ? subEmail.TSubscriptionId : 10001;
                su.IsSubscribed = subscribe;
                su.LastActionDate = now;
                su.IsHtmlFormat = true;
                su.DtStamp = now;
                su.Save();
            }
            else
            {
                //record subscription update
                UserEvent.NewUserEvent(now, now, _Enums.EventQStatus.Success, creatorName,
                    au.UserId, au.LoweredUserName,
                    _Enums.EventQContext.User, _Enums.EventQVerb.SubscriptionUpdate,
                    su.IsSubscribed.ToString(), subscribe.ToString(), description, true);

                su.IsSubscribed = subscribe;
                su.LastActionDate = now;
                su.Save();
            }

            return su;
        }

        /// <summary>
        /// TODO: This is a wee bit convoluted - work on this to make the loop more logical
        /// Sends the email to the given list and records a history object for subscription email
        /// Uses the SetSubscriptionEmailToListCommandParams() method within the loop to reset command vars. This is done to keep the command size limited.
        /// Subscriptions are inherintly massemails
        /// </summary>
        /// <param name="subscriptionEmailId"></param>
        /// <param name="validatedEmail"></param>
        /// <param name="dateToSend"></param>
        /// <param name="priority">0 is quickest</param>
        public static void SendSubscriptionEmailToList(int subscriptionEmailId, List<string> validatedEmail,
            DateTime dateToSend, int priority)
        {
            bool isMassEmail = true;

            SubscriptionEmail subEmail = SubscriptionEmail.FetchByID(subscriptionEmailId);

            if (subEmail != null && subEmail.SubscriptionRecord.ApplicationId == _Config.APPLICATION_ID)
            {
                EmailLetter ltr = subEmail.EmailLetterRecord;
                string mailName = subEmail.PostedFileName.Replace(".html", string.Empty);

                StringBuilder sb = new StringBuilder();
                SubSonic.QueryCommand cmd = new QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
                SetSubscriptionEmailToListCommandParams(cmd, ltr, subEmail, dateToSend, priority, isMassEmail);

                int i = 0;
                DateTime now = DateTime.Now;

                foreach (string emailAddress in validatedEmail)
                {
                    EnsureSubscriptionUser(now, emailAddress, true, "SendSubscriptionEmailToList", mailName, subEmail, subEmail.TSubscriptionId);

                    //dont do too many at a time                    
                    string emailParamName = string.Format("@toAddress_{0}", i++);//increment i
                    cmd.Parameters.Add(emailParamName, emailAddress);

                    sb.Append("INSERT MailQueue ([ApplicationId], [TEmailLetterId], [TSubscriptionEmailId], [DateToProcess], [FromName], [FromAddress], [Priority], [bMassMailer], ");
                    sb.Append("[ToAddress]) ");
                    sb.Append("VALUES (@appId, @letterId, @subscriptionEmailId, @dateToSend, @fromName, @fromAddress, @priority, @mass, ");
                    sb.AppendFormat("{0} )", emailParamName);

                    //do one hundred at a time
                    if (i % 100 == 0)
                    {
                        cmd.CommandSql = sb.ToString();

                        try
                        {
                            SubSonic.DataService.ExecuteQuery(cmd);
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex, true);
                            throw ex;
                        }

                        //reset command params, string and counter
                        SetSubscriptionEmailToListCommandParams(cmd, ltr, subEmail, dateToSend, priority, isMassEmail);
                        sb.Length = 0;
                        i = 0;
                    }
                }

                //do the leftovers
                if (sb.Length > 0)
                {
                    cmd.CommandSql = sb.ToString();

                    try
                    {
                        SubSonic.DataService.ExecuteQuery(cmd);
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex, true);
                        throw ex;
                    }
                }

                //create a history event for the subemail
                HistorySubscriptionEmail.Insert(DateTime.Now, subscriptionEmailId, dateToSend, validatedEmail.Count);
            }
        }

        /// <summary>
        /// sends 2 emails - 1 to the old and one to the new
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void SendZ2MailerSignupNotification(string email, DateTime requestDate, string originDescription)
        {
            //email signing up
            //userid
            //time of request
            //where the mail was sent from - site
            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();

            dict.Add("<% Email %>", email);
            dict.Add("<% UserId %>", Guid.NewGuid().ToString());
            dict.Add("<% SubscriptionName %>", "Z2ent Newsletter");
            dict.Add("<% RequestDate %>", requestDate.ToString("MM/dd/yyyy hh:mmtt"));
            dict.Add("<% OriginDescription %>", originDescription);
            dict.Add("<% SiteEntityName %>", _Config._Site_Entity_Name);
            dict.Add("<% DomainName %>", _Config._DomainName);

            string templateName = "Z2MailerSignupNotification.txt";
            string template = string.Format("/{0}/MailTemplates/SiteTemplates/{1}", _Config._VirtualResourceDir, templateName);
            string mappedFile = (System.Web.HttpContext.Current != null) ?
                System.Web.HttpContext.Current.Server.MapPath(template) :
                string.Format("{0}\\Z2Web{1}", _Config._MappedRootDirectory, template);

            //we do the replacements here because this will be sent as text
            string body = Utils.FileLoader.FileToString(mappedFile);
            body = Utils.ParseHelper.DoReplacements(body, dict, false);//dont do html replacements in this case


            //TODO: update hardcoded strings
            MailQueue.SendEmail("news@z2ent.com", "Z2ent Newsletter",
                (_Config._SiteIsInTestMode) ? _Config._Admin_EmailAddress : email, null, null,
                "You're almost there! Please confirm your request", null, body, null, false, templateName);   
        }

        public static void SendMailerSignupNotification(string email, string userId, DateTime requestDate, string originDescription, Subscription sub)
        {
            //email signing up
            //userid
            //time of request
            //where the mail was sent from - site
            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();

            dict.Add("<% Email %>", email);
            dict.Add("<% UserId %>", userId);
            dict.Add("<% SubscriptionName %>", sub.Name);
            dict.Add("<% RequestDate %>", requestDate.ToString("MM/dd/yyyy hh:mmtt"));
            dict.Add("<% OriginDescription %>", originDescription);
            dict.Add("<% SiteEntityName %>", _Config._Site_Entity_Name);
            dict.Add("<% DomainName %>", _Config._DomainName);

            string templateName = "MailerSignupNotification.txt";
            string template = string.Format("/{0}/MailTemplates/SiteTemplates/{1}", _Config._VirtualResourceDir, templateName);
            string mappedFile = (System.Web.HttpContext.Current != null) ?
                System.Web.HttpContext.Current.Server.MapPath(template) :
                string.Format("{0}\\WcWeb{1}", _Config._MappedRootDirectory, template);

            //we do the replacements here because this will be sent as text
            string body = Utils.FileLoader.FileToString(mappedFile);
            body = Utils.ParseHelper.DoReplacements(body, dict, false);//dont do html replacements in this case

            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                (_Config._SiteIsInTestMode) ? _Config._Admin_EmailAddress : email, null, null,
                "Mailing List Confirmation Request", null, body, null, false, templateName);
        }

        /// <summary>
        /// This should be wrapped in the callers
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="fromName"></param>
        /// <param name="toName"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="replacements"></param>
        /// <param name="htmlEmail"></param>
        public static void SendEmail(string fromEmail, string fromName, string toEmail, string cc, string bcc, string subject, 
            string body, string textVersion, System.Collections.Specialized.ListDictionary replacements, bool htmlEmail, 
            string emailLetterName)
        {
            MailMessage mail = new MailMessage();

            try
            {
                if (textVersion != null && textVersion.Length > 0)
                {
                    AlternateView plainView = AlternateView
                                .CreateAlternateViewFromString(textVersion, new System.Net.Mime.ContentType("text/plain"));

                    // if this is not set, it passes it as base64
                    plainView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                    mail.AlternateViews.Add(plainView);
                }

                if (body != null && body.Length > 0)
                {
                    if (replacements != null && replacements.Count > 0)
                        body = Utils.ParseHelper.DoReplacements(body, replacements, htmlEmail);

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body,
                            new System.Net.Mime.ContentType("text/html"));

                    // if this is not set, it passes it as base64
                    htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;

                    mail.AlternateViews.Add(htmlView);
                }

                mail.From = new MailAddress(fromEmail, fromName);
                mail.To.Add(new MailAddress(toEmail));
                if (cc != null && cc.Trim().Length > 0)
                    mail.CC.Add(new MailAddress(cc));
                if (bcc != null && bcc.Trim().Length > 0)
                    mail.Bcc.Add(new MailAddress(bcc));
                mail.Subject = subject;

                SmtpClient client = new SmtpClient();
                client.Send(mail);

                //if the email derives its content from an EmailLetter object then log this action
                if (emailLetterName != null && emailLetterName.Trim().Length > 0)
                {
                    QueryCommand cmd = new QueryCommand("SELECT Id FROM EmailLetter WHERE [ApplicationId] = @appId AND [Name] = @name ", 
                        SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                    cmd.Parameters.Add("@name", emailLetterName.Trim());

                    object obj = DataService.ExecuteScalar(cmd);

                    if (obj != null)
                    {
                        DateTime nowDate = DateTime.Now;
                        MailQueue mq = new MailQueue();
                        mq.ApplicationId = _Config.APPLICATION_ID;
                        mq.AttemptsRemaining = 3;
                        mq.Bcc = bcc;
                        mq.IsMassMailer = false;
                        mq.Cc = cc;
                        mq.DateToProcess = nowDate;
                        mq.DateProcessed = nowDate;
                        mq.DtStamp = nowDate;
                        mq.TEmailLetterId = (int)obj;
                        mq.ToAddress = toEmail;
                        mq.FromAddress = fromEmail;
                        mq.FromName = fromName;
                        mq.Priority = 10;
                        mq.Status = "Sent";
                        mq.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex, true);
            }
        }

        public static void SendForgotPass(string email, string pass)
        {
            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();

            dict.Add("<% UserPass %>", pass);

            string templateName = "CustomerForgotPassword.txt";
            string template = string.Format("/{0}/MailTemplates/SiteTemplates/{1}", _Config._VirtualResourceDir, templateName);
            string mappedFile = (System.Web.HttpContext.Current != null) ?
                System.Web.HttpContext.Current.Server.MapPath(template) :
                string.Format("{0}\\WcWeb{1}", _Config._MappedRootDirectory, template);

            string body = Utils.FileLoader.FileToString(mappedFile);
            body = Utils.ParseHelper.DoReplacements(body, dict, false);//dont do html replacements in this case

            //send info to old email - customer service is cc'd on this one
            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                (_Config._SiteIsInTestMode) ? _Config._Admin_EmailAddress : email, null, null,                
                "Your Requested Information", null, body, null, false, templateName);
        }

        /// <summary>
        /// sends 2 emails - 1 to the old and one to the new
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void SendUserChangeEmail(string oldName, string newName)
        {
            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();

            dict.Add("<% OldName %>", oldName);
            dict.Add("<% NewName %>", newName);

            string templateName = "ChangeUserName.txt";
            string template = string.Format("/{0}/MailTemplates/SiteTemplates/{1}", _Config._VirtualResourceDir, templateName);
            string mappedFile = (System.Web.HttpContext.Current != null) ?
                System.Web.HttpContext.Current.Server.MapPath(template) :
                string.Format("{0}\\WcWeb{1}", _Config._MappedRootDirectory, template);

            string body = Utils.FileLoader.FileToString(mappedFile);
            body = Utils.ParseHelper.DoReplacements(body, dict, false);//dont do html replacements in this case

            //send info to old email - customer service is cc'd on this one
            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                (_Config._SiteIsInTestMode) ? _Config._Admin_EmailAddress : oldName, _Config._CustomerService_Email, null,
                "Your Information Has Changed", null, body, null, false, templateName);

            //send email to new address
            //**note only one copy needs to be sent to customer service - don't cc here
            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                (_Config._SiteIsInTestMode) ? _Config._Admin_EmailAddress : newName, null,
                null, "Your Information Has Changed", null, body, null, false, templateName);

        }

        private static void SendEmailTemplate(string templateName, DateTime sendTime, string fromName, string fromAddress,
            string toAddress, string paramNames, string paramValues)
        {
            SendEmailTemplate(templateName, sendTime, fromName, fromAddress, toAddress, paramNames, paramValues, string.Empty, 1);
        }
        private static void SendEmailTemplate(string templateName, DateTime sendTime, string fromName, string fromAddress,
            string toAddress, string paramNames, string paramValues, string bccEmail, int priority)
        {
            if (!Utils.Validation.IsValidEmail(toAddress))
                throw new Exception(string.Format("{0} is not a valid email address.", toAddress));

            string result = string.Empty;

            StoredProcedure proc = Wcss.SPs.TxSendEmailTemplate(_Config.APPLICATION_ID, templateName, Utils.ParseHelper.Parse_DateForTableInsert(sendTime),
                fromName, fromAddress, toAddress, paramNames, paramValues, bccEmail, priority, result);

            proc.Execute();
            result = proc.OutputValues[0].ToString();

            if (!result.ToLower().Equals("success"))
                throw new Exception(string.Format("{0}", result));
        }

    }
}
