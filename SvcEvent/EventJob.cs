using System;
using System.IO;
using System.Net.Mail;

using Jobs;
using Wcss;

namespace SvcEvent
{
	/// <summary>
	/// runs a multithreadable Job
	/// </summary>
	public class EventJob : Jobs.Job
	{		
		/// <summary>
		/// provides mails to process
		/// </summary>
		private EventData eventData = null;

		/// <summary>
		/// Only constructor
		/// </summary>
		public EventJob() : base()
		{
            if (Wcss._Config._ErrorsToDebugger)
			    System.Diagnostics.Debug.WriteLine("Starting Job...", "EventJob");

            eventData = new EventData(this.JobId);
		}

		/// <summary>
		/// Entry point
        /// see http://noggle.com/?p=23 
		/// </summary>
		/// <returns></returns>
		public override bool DoJob()
		{
            if (!_Config.SqlServerIsAvailable())
            {
                _Error.LogToFile("Sql Server not available.", string.Format("{0}{1}", _Config._ErrorLogTitle, DateTime.Now.ToString("MM_dd_yyyy")));

                System.Threading.AutoResetEvent waitingEvent = new System.Threading.AutoResetEvent(false);
                waitingEvent.WaitOne(28800000, true);//every 8 minutes - 8 * 60 * 60 * 1000 = 28800000

                return true;
            }

			EventQ q = null;

            try
            {
                //keep this for testing
                //throw new Exception("find the dir");

                q = eventData.GetNextEvent();

                if (q == null) return true;

                //Process row by verb
                if (ProcessEvent(q))
                {
                    //update mq row
                    q.Status = "Success";
                    q.DateProcessed = DateTime.Now;
                    q.ThreadLock = null;
                    q.Save();
                }

                System.Diagnostics.Debug.WriteLine(string.Format("Event ({0}) has been processed", q.Verb));
            }
            catch (Exception e)
            {
                if (Wcss._Config._ErrorsToDebugger)
                {
                    System.Diagnostics.Debug.WriteLine("Event service failure...");
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }

                if (q != null)
                {
                    q.Status = e.Message + "\n" + e.StackTrace;
                    if (q.Status.Length > 2000)
                        q.Status = q.Status.Substring(0, 1995);

                    q.DateProcessed = DateTime.Now;
                    q.ThreadLock = null;
                    q.AttemptsRemaining -= 1;
                    q.Save();
                }

                Wcss._Error.LogException(e);
            }
						
			return true;
		}
        public void SaveToFile(string mappedPathAndName, string body)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                    fs = new FileStream(mappedPathAndName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs);

                    sw.Write(body);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (sw != null) sw.Close();
            if (fs != null) fs.Close();

        }
        public bool ProcessEvent(EventQ q)
        {
            _Enums.EventQContext context = (_Enums.EventQContext)Enum.Parse(typeof(_Enums.EventQContext), q.Context, true);
            _Enums.EventQVerb verb = (_Enums.EventQVerb)Enum.Parse(typeof(_Enums.EventQVerb), q.Verb, true);
            
            switch (context)
            {
                case _Enums.EventQContext.AdminNotification:
                    string details = string.Format("{1}: {2}{0}{3}: {4}", Environment.NewLine, q.CreatorName, q.OldValue, 
                        q.NewValue, q.Description);
                    MailQueue.SendEmail(_Config.svc_ServiceEmail, q.CreatorName, _Config._CC_DeveloperEmail, null, null, details, details, null, null, true, null);

                    return true;
                    break;                
            }

            switch (verb)
            {       
                case _Enums.EventQVerb.Mailer_FailureNotification:
                    //send an email to the admin to tell him that there are mailer issues
                    MailQueue.SendEmail(_Config.svc_ServiceEmail, "Badmail Service", _Config._CC_DeveloperEmail, null, null, q.NewValue, q.Description, null, null, true, null);

                    return true;
                    break;

                #region MailerRemove
                case _Enums.EventQVerb.Mailer_Remove:

                    try
                    {
                        //we have a userid - ensure a subscription record
                        SubscriptionUser su = MailQueue.EnsureSubscriptionUser(DateTime.Now, q.UserName, false, "EventJob", null, null,
                            10001);

                        if (su.UserId == null || su.UserId == Guid.Empty)
                            throw new Exception(string.Format("EventJob MailerRemove - Unable to identify user: {0}", q.UserName));

                        return true;
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                    }

                    break;

                #endregion

            }

            return false;
        }

        public override void CleanUp()
        {
            //eventData.CommitAll();
        }
	}
}
