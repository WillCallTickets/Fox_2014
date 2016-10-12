using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;

namespace Wcss
{
    public partial class MailProcessor
    {
        /// <summary>
        /// Returns stats for a mailer
        /// </summary>
        public class ProcessingCustomResult
        {
            public int OriginalListCount { get; set; }

            public int EmailsSubmitted { get; set; }
            public int ValidDistinctEmails { get; set; }
            public int NotInFoxDb { get; set; }
            public int InFoxDb { get; set; }
            public int FoxSubscribes { get; set; }
            public int FoxUnsubscribes { get; set; }
            public List<string> FinalList { get; set; }
            //assign subscriber number when not using a custom list
            public int FinalListCount { get { return (FinalList != null && FinalList.Count > 0) ? FinalList.Count : FoxSubscribes; } }
            public DateTime SendDate = DateTime.MaxValue;

            public ProcessingCustomResult(int originalListCount)
            {
                OriginalListCount = originalListCount;                
                FinalList = new List<string>();
            }

            public override string ToString()
            {
                return string.Format("*Submitted: {0}  Valid: {1}  NotInFox: {2}  InFox: {3}  Subbed: {4}  Unsub: {5}  Final: {6}",
                    this.EmailsSubmitted.ToString(), this.ValidDistinctEmails.ToString(), this.NotInFoxDb.ToString(), this.InFoxDb.ToString(),
                    this.FoxSubscribes.ToString(), this.FoxUnsubscribes.ToString(), this.FinalListCount.ToString(),
                    (SendDate != DateTime.MaxValue) ? string.Format("<br/>{0} emails submitted to MailQueue for delivery - beginning {1}", 
                        this.FinalListCount.ToString(), this.SendDate.ToLongTimeString()) : string.Empty);
            }
        }


        /// <summary>
        /// Sends an email to the recipients specified in the text box. Parses out emails from the text box. Context determines priority
        /// For Test emails only
        /// </summary>
        public static int SendTestToTextBoxRecipients(SubscriptionEmail subEmail, TextBox mailList, DateTime sendDate)
        {
            //handles empty lists errors
            List<string> validEmails = Utils.ParseHelper.ReturnValidListOfEmails(mailList.Text, false);

            PopulateMailQueueFromListAndEmail(subEmail, validEmails, sendDate, _Enums.MailProcessorContext.test);

            return validEmails.Count;
        }

        /// <summary>
        /// Send a mailer to those subscribed to our email newsletter
        /// </summary>
        /// <param name="subEmail"></param>
        /// <param name="sendDate"></param>
        /// <returns></returns>
        public static ProcessingCustomResult ProcessSubscriberMailer(SubscriptionEmail subEmail, DateTime sendDate)
        {
            subEmail.EnsurePublication();

            int numSent = (int)SPs.TxSubscriptionInsertMailerIntoQueue(_Config.APPLICATION_ID, subEmail.Id, sendDate.ToString(),
               _Config._MassMailService_FromName, _Config._MassMailService_Email, 10).ExecuteScalar();

            //create a history event for the subemail
            HistorySubscriptionEmail.Insert(DateTime.Now, subEmail.Id, sendDate, numSent);

            ProcessingCustomResult result = new ProcessingCustomResult(numSent);
            result.EmailsSubmitted = numSent;
            result.FoxSubscribes = numSent;
            result.InFoxDb = numSent;
            result.ValidDistinctEmails = numSent;
            result.SendDate = sendDate;

            return result;
        }

        
        /// <summary>
        /// Takes a list of emails and determines if they are eligible to receive email from us. Allows for emails not in our database and removes names that are unsubscribed.
        /// Returns statistics of the given list.
        /// DOES NOT SEND EMAIL
        /// </summary>
        /// <param name="mailList">TextBox control with list of emails to process</param>
        /// <returns>total emails entered, total valid emails, emails not in fox db, emails in fox db, fox subscribes, fox unsubscribes, final list</returns>
        public static ProcessingCustomResult ProcessCustomListSubscribers(TextBox mailList)
        {
            return ProcessCustomListSubscribers(mailList, null, DateTime.MaxValue);
        }
        /// <summary>
        /// Takes a list of emails and determines if they are eligible to receive email from us. Allows for emails not in our database and removes names that are unsubscribed.
        /// Returns statistics of the given list.
        /// DOES SEND EMAIL
        /// </summary>
        /// <param name="mailList">TextBox control with list of emails to process</param>
        /// <returns>total emails entered, total valid emails, emails not in fox db, emails in fox db, fox subscribes, fox unsubscribes, final list and when mail will be processed</returns>
        public static ProcessingCustomResult ProcessCustomListSubscribers(TextBox mailList, SubscriptionEmail subEmail, DateTime sendDate)
        {
            List<string> validEmails = Utils.ParseHelper.ReturnValidListOfEmails(mailList.Text, false);
            ProcessingCustomResult result = new ProcessingCustomResult(validEmails.Count);

            //run an SP to clean emails and return stats
            using (IDataReader rdr = SPs.TxMailerProcessCustomEmailerList(_Config.APPLICATION_ID, string.Join(",", validEmails.ToArray())).GetReader())
            {
                while(rdr.Read())
                    result.EmailsSubmitted = (int)rdr.GetValue(0);
                rdr.NextResult();

                while (rdr.Read())
                    result.ValidDistinctEmails = (int)rdr.GetValue(0);
                rdr.NextResult();

                while (rdr.Read())
                    result.NotInFoxDb = (int)rdr.GetValue(0);
                rdr.NextResult();

                while (rdr.Read())
                    result.InFoxDb = (int)rdr.GetValue(0);
                rdr.NextResult();

                while (rdr.Read())
                    result.FoxSubscribes = (int)rdr.GetValue(0);
                rdr.NextResult();

                while (rdr.Read())
                    result.FoxUnsubscribes = (int)rdr.GetValue(0);
                rdr.NextResult();

                while (rdr.Read())
                    result.FinalList.Add(rdr.GetValue(0).ToString());

                rdr.Close();
            }


            //send if indicated
            if (subEmail != null && sendDate != DateTime.MaxValue)
            {
                PopulateMailQueueFromListAndEmail(subEmail, validEmails, sendDate, _Enums.MailProcessorContext.customlist);                
                result.SendDate = sendDate;
            }

            return result;
        }


        private static string PopulateMailQueueFromListAndEmail(SubscriptionEmail subEmail, List<string> validEmails, DateTime sendDate, _Enums.MailProcessorContext context)
        {
            if (subEmail == null)
                throw new ArgumentNullException("SubscriptionEmail cannot be null.");

            int priority = (context == _Enums.MailProcessorContext.test) ? 0 : 10;

            subEmail.EnsurePublication();

            //send an email
            //test emails should be processed immediately and should have highest priority
            MailQueue.SendSubscriptionEmailToList(subEmail.Id, validEmails, sendDate, priority);

            return string.Empty;
        }
    }
}
