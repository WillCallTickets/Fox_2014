using System;
using System.Net.Mail;

using Jobs;
using Wcss;

namespace SvcMail
{
	/// <summary>
	/// runs a multithreadable Mail Job
	/// </summary>
	public class MailJob : Jobs.Job
	{		
		/// <summary>
		/// provides mails to process
		/// </summary>
		private MailData mailData = null;

		/// <summary>
		/// Only constructor
		/// </summary>
		public MailJob() : base()
		{
            if (Wcss._Config._ErrorsToDebugger)
			    System.Diagnostics.Debug.WriteLine("Starting Job...", "MailJob");

            mailData = new MailData(this.JobId);
		}

		/// <summary>
		/// Entry point
        /// see http://noggle.com/?p=23 
		/// </summary>
		/// <returns></returns>
		public override bool DoJob()
		{
            //Wcss._Error.LogException(new Exception("throwdown"));

            if (!_Config.SqlServerIsAvailable())
            {
                _Error.LogToFile("Sql Server not available.", string.Format("{0}{1}", _Config._ErrorLogTitle, DateTime.Now.ToString("MM_dd_yyyy")));

                System.Threading.AutoResetEvent waitingEvent = new System.Threading.AutoResetEvent(false);
                waitingEvent.WaitOne(28800000, true);//every 8 minutes - 8 * 60 * 60 * 1000 = 28800000

                return true;
            }

			MailQueue mq = null;

            try
            {
                //keep this for testing
               // throw new Exception("find the dir");

                mq = mailData.GetNextMail();

                if (mq == null) return true;

                MailMessage mail = new MailMessage();
                mail.Subject = mq.EmailLetterRecord.Subject;

                mail.From = new MailAddress(mq.FromAddress, mq.FromName);
                mail.To.Add(new MailAddress((_Config.svc_ServiceTestMode) ? _Config.svc_ServiceTestEmail : mq.ToAddress));

                
                //MassMailers need to be handled in a special manner
                //if it is a mass email - then it has a subscription
                if (mq.IsMassMailer)
                {
                    SubscriptionEmail subEmail = mq.SubscriptionEmailRecord;
                    bool Is3rdParty = (mq.EmailLetterRecord.TextVersion == _Enums.MailTemplateTypes.Is3rdPartySender.ToString());

                    //let 3rd party senders build text version
                    if ((!Is3rdParty) && mq.EmailLetterRecord.TextVersion != null && mq.EmailLetterRecord.TextVersion.Trim().Length > 0)
                    {
                        //only do this once - this gets set to null in sender
                        if (subEmail.ConstructedText == null || subEmail.ConstructedText.Trim().Length == 0)
                        {
                            string update = string.Format("UPDATE [SubscriptionEmail] SET [Constructed_Text] = @txt WHERE [Id] = @id; ");
                            SubSonic.QueryCommand construct = new SubSonic.QueryCommand(update, SubSonic.DataService.Provider.Name);
                            subEmail.ConstructedText = subEmail.CreateShell_Text(false, true);
                            construct.Parameters.Add("@txt", subEmail.ConstructedText);
                            construct.Parameters.Add("@id", subEmail.Id, System.Data.DbType.Int32);
                            SubSonic.DataService.ExecuteQuery(construct);
                        }

                        string plainText = subEmail.ConstructedText;

                        AlternateView plainView = AlternateView
                            .CreateAlternateViewFromString(plainText, new System.Net.Mime.ContentType("text/plain"));
                        
                        // if this is not set, it passes it as base64
                        plainView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                        mail.AlternateViews.Add(plainView);
                    }


                    //only do this once - this gets set to null in sender
                    if (subEmail.ConstructedHtml == null || subEmail.ConstructedHtml.Trim().Length == 0)
                    {
                        string update = string.Format("UPDATE [SubscriptionEmail] SET [Constructed_Html] = @html WHERE [Id] = @id; ");
                        SubSonic.QueryCommand construct = new SubSonic.QueryCommand(update, SubSonic.DataService.Provider.Name);
                        subEmail.ConstructedHtml = subEmail.CreateShell_Html(true, true, true, true);
                        construct.Parameters.Add("@html", subEmail.ConstructedHtml);
                        construct.Parameters.Add("@id", subEmail.Id, System.Data.DbType.Int32);
                        SubSonic.DataService.ExecuteQuery(construct);
                    }

                    if ((subEmail.FinalHtml == null || subEmail.FinalHtml.Trim().Length == 0) && subEmail.ConstructedHtml.Trim().Length > 0)
                    {
                        string update = string.Format("UPDATE [SubscriptionEmail] SET [Final_Html] = @html WHERE [Id] = @id; ");
                        SubSonic.QueryCommand construct = new SubSonic.QueryCommand(update, SubSonic.DataService.Provider.Name);
                        Utils.CssInliner cin = new Utils.CssInliner(subEmail.ConstructedHtml);
                        subEmail.FinalHtml = cin.HtmlAfter;
                        construct.Parameters.Add("@html", subEmail.FinalHtml);
                        construct.Parameters.Add("@id", subEmail.Id, System.Data.DbType.Int32);
                        SubSonic.DataService.ExecuteQuery(construct);
                    }

                    //use full html specs if css is included
                    string emailer = (subEmail.FinalHtml != null && subEmail.FinalHtml.Trim().Length > 0) ? 
                        subEmail.FinalHtml : subEmail.ConstructedHtml;



                    //*************************************************************
                    // ALWAYS INCLUDE A TRACKING LINK
                    //*************************************************************
                    if (!Is3rdParty)
                    {
                        //include tracking image with name of campaign and userid
                        AspnetUser au = new AspnetUser();

                        _DatabaseCommandHelper dch = new _DatabaseCommandHelper(
                            "SELECT * FROM [Aspnet_Users] WHERE [ApplicationId] = @appId AND [UserName] = @email");

                        dch.AddCmdParameter("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                        dch.AddCmdParameter("@email", mq.ToAddress, System.Data.DbType.String);

                        dch.PopulateCollectionByReader<AspnetUser>(au);

                        string tracking = string.Format("<img src=\"http://{0}/assets/images/tracking.gif?utm_campaign={1}&usr={2}&pry={3}\" alt=\"\" width=\"1\" height=\"1\" />",
                            _Config._DomainName,
                            System.Web.HttpUtility.UrlEncode(mq.EmailLetterRecord.Name),
                            (au.UserId == null || au.UserId == Guid.Empty) ? mq.ToAddress : au.UserId.ToString(),
                            mq.Priority.ToString()
                            );

                        //if we have a tracking tag - then replace - otherwise just prepend to the end body tag
                        if (emailer.IndexOf(MailerContent.tagTracking) != -1)
                            emailer = emailer.Replace(MailerContent.tagTracking, tracking);
                        else
                            emailer = emailer.Replace("</body>", string.Format("{0}</body>", tracking));
                        //end of tracking
                    }
                    else
                    {
                        //because we will only ever be sending as Z2
                        string tracking = string.Format("<img src=\"http://z2ent.com/assets/images/tracking.gif?utm_campaign={0}\" alt=\"\" width=\"1\" height=\"1\" />",
                           System.Web.HttpUtility.UrlEncode(mq.EmailLetterRecord.Name));

                        //if we have a tracking tag - then replace - otherwise just prepend to the end body tag
                        if (emailer.IndexOf(MailerContent.tagTracking) != -1)
                            emailer = emailer.Replace(MailerContent.tagTracking, tracking);
                        else
                            emailer = emailer.Replace("</body>", string.Format("{0}</body>", tracking));
                        //end of tracking
                    }


                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailer,
                        new System.Net.Mime.ContentType("text/html"));

                    // if this is not set, it passes it as base64
                    htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;

                    mail.AlternateViews.Add(htmlView);
                                        
                }
                else
                    mail.Body = mq.EmailLetterRecord.HtmlVersion;
                

                SmtpClient client = new SmtpClient();                 
                client.Send(mail);

                //update mq row
                mq.Status = "Sent";
                mq.DateProcessed = DateTime.Now;
                mq.ThreadLock = null;
                mq.Save();

                System.Diagnostics.Debug.WriteLine(string.Format("Email has been sent to {0}", mq.ToAddress));
            }
            catch (Exception e)
            {
                if (Wcss._Config._ErrorsToDebugger)
                {
                    System.Diagnostics.Debug.WriteLine("Mail service failure...");
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }

                if (mq != null)
                {
                    mq.Status = e.Message + "\n" + e.StackTrace;
                    if (mq.Status.Length > 2000)
                        mq.Status = mq.Status.Substring(0, 1995);

                    mq.DateProcessed = DateTime.Now;
                    mq.ThreadLock = null;
                    mq.AttemptsRemaining -= 1;
                    mq.Save();
                }

                Wcss._Error.LogException(e);
            }
						
			return true;
		}

        public override void CleanUp()
        {
            //mailData.CommitAll();
        }
	}
}
