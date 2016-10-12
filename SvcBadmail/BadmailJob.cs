using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;

using Jobs;
using Wcss;

namespace SvcBadmail
{
	/// <summary>
	/// runs a multithreadable Job
	/// </summary>
	public class BadmailJob : Jobs.Job
	{		
		/// <summary>
		/// provides mails to process
		/// </summary>
		//private EventData eventData = null;
        private readonly int _batchSize = 1000;

		/// <summary>
		/// Only constructor
		/// </summary>
        public BadmailJob() : base()
		{
            //if (_Config._ErrorsToDebugger)
            //    Debug.WriteLine("Starting Job...", "BadmailJob");

            //eventData = new EventData(this.JobId);

            //testing git changes
        }

        #region Properties
        //props
        private DirectoryInfo _badmailDirectory = null;
        protected DirectoryInfo BadmailDirectory
        {
            get
            {
                if (_badmailDirectory == null)
                {
                    //set initial directory
                    string initialDir = _Config.svc_AbsoluteBadmailPath;

                    if (!Directory.Exists(initialDir))
                        throw new DirectoryNotFoundException("Badmail directory specified does not exist.");

                    _badmailDirectory = new DirectoryInfo(initialDir);
                }

                return _badmailDirectory;
            }
        }
        private DirectoryInfo _toProcessDirectory = null;
        protected DirectoryInfo ToProcessDirectory
        {
            get
            {
                if (_toProcessDirectory == null)
                {
                    string processDir = string.Format("{0}{1}", _Config._MappedRootDirectory, @"\BadmailService\ToProcess\");

                    if (!Directory.Exists(processDir))
                        Directory.CreateDirectory(processDir);

                    _toProcessDirectory = new DirectoryInfo(processDir);
                }

                return _toProcessDirectory;
            }
        }
        private DirectoryInfo _nothandledDirectory = null;
        protected DirectoryInfo NotHandledDirectory
        {
            get
            {
                if (_nothandledDirectory == null)
                {
                    string nothandledDir = string.Format("{0}{1}", _Config._MappedRootDirectory, @"\BadmailService\NotHandled\");

                    if (!Directory.Exists(nothandledDir))
                        Directory.CreateDirectory(nothandledDir);

                    _nothandledDirectory = new DirectoryInfo(nothandledDir);
                }

                return _nothandledDirectory;
            }
        }
        #endregion


        /// <summary>
		/// Entry point
        /// see http://noggle.com/?p=23 
		/// </summary>
		/// <returns></returns>
		public override bool DoJob()
		{
            if (_Config.SqlServerIsAvailable())
            {
                //delete non-essential files and move others into process dir
                RemoveUnwantedFilesAndMoveToProcessDir();

                //process email in process dir
                ProcessBadmail();
            }
            else
            {   
                _Error.LogToFile("Sql Server not available.", string.Format("{0}{1}", _Config._ErrorLogTitle, DateTime.Now.ToString("MM_dd_yyyy")));

                System.Threading.AutoResetEvent waitingEvent = new System.Threading.AutoResetEvent(false);
                waitingEvent.WaitOne(28800000, true);//every 8 minutes - 8 * 60 * 60 * 1000 = 28800000
            }

            return true;
		}

        

        protected void ProcessBadmail()
        {
            List<FileInfo> files = new List<FileInfo>();
            
            files.AddRange(ToProcessDirectory.GetFiles("*.bad"));

            //restrain the workload
            if (files.Count > _batchSize)
                files.RemoveRange(_batchSize, files.Count - _batchSize);

            foreach (FileInfo file in files)
            {
                //string sender = null;
                string recipient = null;
                string status = null;
                string diagCode = null;
                string subId = null;
                bool handled = false;
                char[] badChars = { '=', '\'' };

                if (File.Exists(file.FullName))
                {
                    string line;
                    //string previousLine = null;


                    #region Populate Info

                    //populate needed info from the file
                    using (StreamReader reader = new StreamReader(file.FullName))
                    {
                        try
                        {
                            //read the file and assign values
                            while ((line = reader.ReadLine()) != null)
                            {
                                //**note add an extra char to avoid space
                                if (line.IndexOf("Final-Recipient:", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    recipient = line.Substring(line.ToLower().IndexOf("Final-Recipient:", StringComparison.OrdinalIgnoreCase) + 16).Replace("rfc822;", string.Empty).Trim();
                                }
                                else if (line.IndexOf("Status:", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    status = line.Substring(line.IndexOf("Status:") + 7).Trim();
                                }
                                else if (line.IndexOf("Diagnostic-Code:", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    diagCode = line.Substring(line.IndexOf("Diagnostic-Code:", StringComparison.OrdinalIgnoreCase) + 16).Trim();
                                }
                                //else if (previousLine != null && previousLine.IndexOf("subemlid=", StringComparison.OrdinalIgnoreCase) != -1)
                                //{
                                //    //deal with the value spanning consecutive lines
                                //    //...subemlid=
                                //    //10682 />=0A=0A =0A</head>=0A=0A<body>=0A=0A<tabl
                                //    string _line = line;
                                //    int idx = _line.IndexOf(" />");
                                //    string part = _line.Substring(0, idx + 1).TrimEnd(badChars).Trim();

                                //    int tryint = 0;
                                //    if (int.TryParse(part, out tryint))
                                //    {
                                //        subId = tryint.ToString();
                                //    }
                                //    else 
                                //        subId = "Could not parse SubId from mailer.";

                                //    //no reason to read the email any further - we have done all we can do
                                //    break;
                                //}
                                //else 
                                else if (line.IndexOf("subemlid=", StringComparison.OrdinalIgnoreCase) != -1)//subemlid=3D'10006'
                                {
                                    string _line = line;
                                    int idx = _line.IndexOf("subemlid=", StringComparison.OrdinalIgnoreCase);
                                    string part1 = _line.Substring(idx + 12).Replace("\"",string.Empty).TrimStart(badChars).TrimEnd(badChars).Trim();

                                    int tryint = 0;
                                    if(int.TryParse(part1, out tryint))
                                    {
                                        subId = tryint.ToString();

                                        //no reason to read the email any further
                                        break;
                                    }
                                    //else
                                    //    previousLine = line;
                                }
                            }
                        }
                        catch (System.FormatException fe)
                        {
                            _Error.LogException(fe);
                            handled = true;
                        }
                        catch (Exception ex)
                        {
                            //services do not log exception
                            _Error.LogException(ex);
                            if (recipient != null)
                            {
                                _Error.LogException(new Exception(recipient));
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }//end of using

                    #endregion


                    #region Decide how to handle - or if to handle or not

                    ////disregard non list emails
                    //if (subId == null && recipient != null && status != null)
                    //{

                    //    EventQ.CreateMailerNotification(DateTime.Now, "BadmailService", recipient, _Enums.EventQVerb.Mailer_Remove, "no sub id found",
                    //        status, (diagCode != null) ? diagCode : "Recipient was not found at host");
                    //    handled = true;
                    //}
                    //else 


                    if (recipient != null && status != null)
                    {
                        string sts = status.Replace(".", string.Empty).Trim();

                        switch (sts)
                        {
                            case "520"://Message identified as SPAM 
                            case "560":// Lone CR or LF in body (see RFC2822 section 2.3)  //TODO: notify admin properly
                                //dont handle and notify admin
                                //currently we notfiy admin - which means the admin must check msgs to find issues
                                handled = false;
                                break;

                            //dont handle and dont notify admin
                            case "518"://MX invalid #439
                            case "533"://unrecognized command
                            case "554"://unrecognized parameter
                                handled = false;
                                break;

                
                            //CONNECTION PROBLEMS
                            //content or security violation
                            //notify admin of connection issues - be sure to once a day at most?
                            //put into handled folder
                            case "571"://571 Permissions problem.  For some reason the sender is not allowed to email this account.  Perhaps an anonymous user is trying to send mail to a distribution list.
                                EventQ.CreateMailerNotification(DateTime.Now, "BadmailService", recipient, _Enums.EventQVerb.Mailer_Remove, null,
                                    status + " permissions prolem", (diagCode != null) ? diagCode : "571 Permissions problem.  For some reason the sender is not allowed to email this account");
                                handled = true;
                                break;

                            case "447"://447 Problem with a timeout.  Check receiving server connectors.
                                EventQ.CreateMailerNotification(DateTime.Now, "BadmailService", recipient, _Enums.EventQVerb.Mailer_Remove, null,
                                    status + " timeout problem", (diagCode != null) ? diagCode : "447 Problem with a timeout.  Check receiving server connectors.");
                                handled = true;
                                break;

                            //////////////////////



                            //mailbox full - handle and ignore
                            case "522":
                            case "523"://msg size is greater than allowed at remote host
                            //case "417"://sender address rejected
                            case "421"://sender verification in progress
                                handled = true;
                                break;

                            //no user or discontinued - remove email from list - handle
                            case "510"://inactive user
                            case "511":
                            case "550":
                            case "551":
                            case "500"://userunknown - no mailbox
                            case "521"://mailbox inactive
                            case "530"://addressee unknown
                            case "535"://bad address
                            case "540"://mispelled domain eg. msn.co, yahho.com, colrado.edu
                            case "541"://Recipient address rejected: Access Denied
                            case "570"://email not valid, local policy violation
                            case "400"://size limit exceeded, mailbox disabled, no such recipient
                            case "516"://mailbox no longer valid
                            //these are not unknown user codes - but the email domains will never accept our address
                            case "417"://sender address rejected
                            case "471"://bad envelope from address - does not like "pleasedonotreply" address


                                //insert a service event to remove the user from our lists
                                EventQ.CreateMailerNotification(DateTime.Now, "BadmailService", recipient, _Enums.EventQVerb.Mailer_Remove, subId,
                                    status, (diagCode != null) ? diagCode : "[diagCode is null] Recipient was not found at host");

                                handled = true;

                                break;
                        }//end switch
                    }//end else if

                    #endregion

                    //if we cannot process - move to unhandled directory
                    if (handled)//toss it out
                    {
                        file.Delete();
                    }
                    else
                    {
                        string destination = string.Format(@"{0}{1}", this.NotHandledDirectory, file.Name);

                        if (File.Exists(destination))
                            File.Delete(destination);

                        file.MoveTo(destination);
                    }
                }//end of file exists

            }//end foreach

        }

        #region Collation

        protected void RemoveUnwantedFilesAndMoveToProcessDir()
        {
            List<FileInfo> files = new List<FileInfo>();

            files.AddRange(BadmailDirectory.GetFiles("*.bdp"));
            for (int i = 0; i < files.Count; i++)
                files[i].Delete();

            files.Clear();

            files.AddRange(BadmailDirectory.GetFiles("*.bdr"));
            for (int i = 0; i < files.Count; i++)
                files[i].Delete();

            files.Clear();

            files.AddRange(BadmailDirectory.GetFiles("*.bad"));

            //restrain the workload
            if (files.Count > _batchSize)
                files.RemoveRange(_batchSize, files.Count - _batchSize);

            //remove delays and move failures to process dir
            foreach (FileInfo file in files)
            {
                if (File.Exists(file.FullName))
                {
                    using (StreamReader reader = new StreamReader(file.FullName))
                    {
                        string line;

                        try
                        {
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.IndexOf("Subject: Delivery Status Notification (Delay)", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    reader.Close();
                                    file.Delete();
                                    break;
                                }
                                else if (line.IndexOf("Subject: Delivery Status Notification (Failure)", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    reader.Close();
                                    //if the file exists in the destination - delete it
                                    string destination = string.Format(@"{0}{1}", this.ToProcessDirectory, file.Name);
                                    
                                    if (File.Exists(destination))
                                        File.Delete(destination);

                                    file.MoveTo(destination);

                                    break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                }
            }
        }
        #endregion

        public override void CleanUp()
        {
            //eventData.CommitAll();
        }
	}
}
