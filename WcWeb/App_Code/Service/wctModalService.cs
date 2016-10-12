using System;
using System.Web.Services;
using System.Net.Mail;
using System.IO;

using Newtonsoft.Json;

using Wcss;
using wctMain.Controller;

namespace wctMain.Service
{
    /// <summary>
    /// Summary description for FBService
    /// </summary>
    [WebService(Namespace = "http://foxtheatre.com/Svc/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
    public class wctModalService : System.Web.Services.WebService
    {
        MainContext _ctx;

        public wctModalService()
        {
            _ctx = new MainContext();
        }

        #region Mailer 

        [WebMethod]
        public string mailerConfirm(string confirmId, string userIp)
        {
            string retVal = "ERROR - Sorry, user was not found.";

            if (confirmId != null && confirmId.Trim().Length > 0)
            {
                AspnetUser user = null;

                //get the user from the id 
                try
                {
                    user = new AspnetUser("UserId", confirmId);
                    if (user == null || user.UserId == Guid.Empty)
                        throw new Exception ("User was not found");

                    DateTime now = DateTime.Now;

                    //get the subs for 
                    SubscriptionUserCollection coll = new SubscriptionUserCollection().Where("UserId", user.UserId.ToString()).Load();

                    //subscribe to the default subscription
                    SubscriptionUser defaultsub = coll.GetList().Find(delegate(SubscriptionUser match) { return match.SubscriptionRecord.IsDefault; });

                    //if not there create
                    if (defaultsub == null)
                    {
                        defaultsub = new SubscriptionUser();
                        defaultsub.DtStamp = DateTime.Now;
                        defaultsub.IsHtmlFormat = true;
                        defaultsub.IsSubscribed = false;//this will be subscribed in a moment - see below
                        defaultsub.LastActionDate = DateTime.Now;
                        Subscription defaultNewsletter = _Lookits.Subscriptions.GetList().Find(delegate(Subscription match) { return match.IsDefault; });
                        defaultsub.TSubscriptionId = (defaultNewsletter != null) ? defaultNewsletter.Id : 0;
                        defaultsub.UserId = user.UserId;
                    }

                    //update the sub and save and record event
                    if (!defaultsub.IsSubscribed)
                    {
                        defaultsub.IsSubscribed = true;
                        defaultsub.Save();

                        //record events
                        string subInfo = string.Format("{0}~{1}~ Page: {2}", defaultsub.SubscriptionRecord.Id, defaultsub.SubscriptionRecord.Name, "mailerConfirm");

                        UserEvent.NewUserEvent(user.LoweredUserName, now, now, _Enums.EventQStatus.Success, user.LoweredUserName, _Enums.EventQContext.User,
                            _Enums.EventQVerb.SubscriptionUpdate, "Not Subscribed", "Subscribed", subInfo, true);
                    }

                    retVal = string.Format("{0} is now subscribed to our newsletter.", user.LoweredUserName);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    retVal = string.Format("ERROR - {0}", ex.Message);
                }
            }

            return retVal;
        }

        [WebMethod]
        public string mailerSubscribe(string emailAddress, string profileName, string userIp)
        {
             string retVal = string.Empty;

             try
             {
                 emailAddress = emailAddress.Trim();
                 if (!Utils.Validation.IsValidEmail(emailAddress))
                     throw new InvalidDataException("Please enter a valid email address.");

                 //decide which subscription to unsubscribe from 
                 DateTime now = DateTime.Now;
                 Subscription defaultNewsletter = (Subscription)_Lookits.Subscriptions.GetList()
                     .Find(delegate(Subscription match) { return match.IsDefault; });

                 if (defaultNewsletter == null)
                     throw new Exception("We're sorry but there are no default subscriptions to unsubscribe from.");

                 AspnetUser user = AspnetUser.GetUserByUserName(emailAddress);

                 if (user == null || user.UserId == Guid.Empty)
                 {
                     user = new AspnetUser();
                     user.ApplicationId = _Config.APPLICATION_ID;
                     user.UserName = emailAddress;
                     user.LoweredUserName = emailAddress;
                     user.LastActivityDate = DateTime.Now;
                     user.Save();
                 }

                 //get the subs for 
                SubscriptionUserCollection coll = new SubscriptionUserCollection().Where("UserId", user.UserId.ToString()).Load();

                //if the user is authenticated for the email in question...then we can sign up right away
                //note* profile name will only be provided if authd
                if (user.UserName == profileName)
                {
                    //subscribe to the default subscription
                    SubscriptionUser defaultsub = coll.GetList().Find(delegate(SubscriptionUser match) { return match.SubscriptionRecord.IsDefault; });

                    //if not there create
                    if (defaultsub == null)
                    {
                        defaultsub = new SubscriptionUser();
                        defaultsub.DtStamp = DateTime.Now;
                        defaultsub.IsHtmlFormat = true;
                        defaultsub.IsSubscribed = false;//this will be subscribed in a moment - see below
                        defaultsub.LastActionDate = DateTime.Now;
                        defaultsub.TSubscriptionId = defaultNewsletter.Id;
                        defaultsub.UserId = user.UserId;
                    }

                    //update the sub and save and record event
                    if (!defaultsub.IsSubscribed)
                    {
                        defaultsub.IsSubscribed = true;
                        defaultsub.Save();

                        //record events
                        string subInfo = string.Format("{0}~{1}~Page: {2}", defaultsub.SubscriptionRecord.Id, defaultsub.SubscriptionRecord.Name, "subscribe");

                        UserEvent.NewUserEvent(user.LoweredUserName, now, now, _Enums.EventQStatus.Success, user.LoweredUserName, _Enums.EventQContext.User,
                            _Enums.EventQVerb.SubscriptionUpdate, "Not Subscribed", "Subscribed", subInfo, true);
                    }

                    retVal = string.Format("{0} is subscribed to our newsletter.", user.LoweredUserName);
                }
                else//user is not authd
                {
                    string freqResult = EventQ.CheckMailerSignupEventFrequency(user.UserId, userIp);

                    if (freqResult != null)
                        retVal = string.Format("ERROR - {0}", freqResult);

                    //if the name is already subscribed then quit
                    else
                    {
                        SubscriptionUser defaultsub = coll.GetList().Find(delegate(SubscriptionUser match) { return match.SubscriptionRecord.IsDefault; });
                        if (defaultsub != null && defaultsub.IsSubscribed)
                        {
                            retVal = string.Format("{0} is currently subscribed to the newsletter. If you intended to unsubscribe, please use the link below.", user.UserName);
                        }
                        else
                        {
                            //NOW we can go ahead and sign up the name - send an email
                            //dont create a subscriptionuser here - wait for signup page - where we will create a tracking event
                            //send account id in email to track
                            MailQueue.SendMailerSignupNotification(user.UserName, user.UserId.ToString(), DateTime.Now,
                                string.Format("the sign up control located on our {0} page.", "subscribe"), defaultNewsletter);

                            //where from and what subscription
                            string subInfo = string.Format("{0}~{1}~Page: {2}", defaultNewsletter.Id, defaultNewsletter.Name, "subscribe");

                            UserEvent.NewUserEvent(user.LoweredUserName, now, now, _Enums.EventQStatus.Success, user.LoweredUserName, _Enums.EventQContext.User,
                                _Enums.EventQVerb.Mailer_SignupAwaitConfirm, null, null, subInfo, true);

                            //and return a msg to wait for email
                            retVal = string.Format("A confirmation email has been sent to {0}", user.UserName);
                        }
                    }
                }
             }
             catch (System.Threading.ThreadAbortException) { }
             catch (Exception ex)
             {
                 _Error.LogException(ex);
                 retVal = string.Format("ERROR - {0}", ex.Message);
             }

             return retVal;
        }

        [WebMethod]
        public string mailerUnsubscribe(string emailAddress)
        {
            string retVal = string.Empty;
            
            try
            {
                emailAddress = emailAddress.Trim();
                if (!Utils.Validation.IsValidEmail(emailAddress))
                    throw new InvalidDataException("Please enter a valid email address.");

                //decide which subscription to unsubscribe from 
                DateTime now = DateTime.Now;
                Subscription defaultNewsletter = (Subscription)_Lookits.Subscriptions.GetList()
                    .Find(delegate(Subscription match) { return match.IsDefault; });

                if (defaultNewsletter == null)
                    throw new Exception("We're sorry but there are no default subscriptions to unsubscribe from.");

                AspnetUser user = AspnetUser.GetUserByUserName(emailAddress);

                if (user == null || user.UserId == Guid.Empty)
                {   
                    retVal = string.Format("ERROR - {0} is not in our database. Please check your entry and try again.", emailAddress);

                    //Use the following if you want to create an unsubscription structure (user,subemail) for an unsubscribed user...
                    // this creates a situation where the user thinks they have unsubscribed - but they are using wrong email
                    //_Error.LogException(new Exception(string.Format("User not in db - email address ({0}) unsubscribed", emailAddress)));

                    //user = new AspnetUser();
                    //user.ApplicationId = _Config.APPLICATION_ID;
                    //user.UserName = emailAddress;
                    //user.LoweredUserName = emailAddress;
                    //user.LastActivityDate = DateTime.Now;
                    //user.Save();
                    
                    //UserEvent.NewUserEvent(user.LoweredUserName, now, now, _Enums.EventQStatus.Success, user.LoweredUserName, _Enums.EventQContext.User,
                    //    _Enums.EventQVerb.UserCreated, "Email address not in db", "User added", "", true);

                    ////loop thru subscriptions and unassign any possible subs - TODO this will change to specify subscriptions!!
                    //foreach(Subscription sub in _Lookits.Subscriptions)
                    //{
                    //    if (sub.AspnetRoleRecord.RoleName.ToLower() == "webuser")
                    //    {
                    //        SubscriptionUser subUser = new SubscriptionUser();
                    //        subUser.UserId = user.UserId;
                    //        subUser.TSubscriptionId = sub.Id;
                    //        subUser.BSubscribed = false;
                    //        subUser.DtLastActionDate = DateTime.Now;
                    //        subUser.BHtmlFormat = true;
                    //        subUser.DtStamp = DateTime.Now;
                    //        subUser.Save();

                    //        UserEvent.NewUserEvent(user.LoweredUserName, now, now, _Enums.EventQStatus.Success, user.LoweredUserName, _Enums.EventQContext.User,
                    //            _Enums.EventQVerb.SubscriptionUpdate, "", "Not Subscribed", "Email address requested for unsubscribe", true);
                    //    }
                    //}
                }
                else
                {
                    SubscriptionUserCollection coll = new SubscriptionUserCollection().Where("UserId", user.UserId.ToString()).Load();

                    bool hasSubscription = false;

                    foreach (SubscriptionUser su in coll)
                    {
                        if (su.SubscriptionRecord.AspnetRoleRecord.RoleName.ToLower() == "webuser" && su.IsSubscribed)
                        {
                            hasSubscription = true;
                            su.IsSubscribed = false;
                            su.Save();

                            string subInfo = string.Format("{0}~{1}~Page: {2}", su.SubscriptionRecord.Id, su.SubscriptionRecord.Name, "unsubscribe");

                            UserEvent.NewUserEvent(user.LoweredUserName, now, now, _Enums.EventQStatus.Success, user.LoweredUserName, _Enums.EventQContext.User,
                                _Enums.EventQVerb.SubscriptionUpdate, "Subscribed", "Not Subscribed", subInfo, true);
                        }
                    }

                    if (hasSubscription)
                    {
                        //remove all instances from the mailqueue
                        SubSonic.QueryCommand cmd =
                            new SubSonic.QueryCommand("DELETE FROM [MailQueue] WHERE [ToAddress] = @username AND ([DateProcessed] IS NULL OR [Status] IS NULL); ",
                                SubSonic.DataService.Provider.Name);
                        cmd.Parameters.Add("@username", user.LoweredUserName);
                        SubSonic.DataService.ExecuteQuery(cmd);

                        //return a result
                        retVal = string.Format("{0} has been removed from our mailers.", user.LoweredUserName);
                    }
                    else
                        retVal = string.Format("ERROR - {0} is not currently subscribed to our mailer. Please check your entry and try again. If you feel that this may be in error, please use the lilnk below to contact us.", user.LoweredUserName);
                }

            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                retVal = string.Format("ERROR - {0}", ex.Message);
            }

            return retVal;
        }

        #endregion

        #region Show Copy

        [WebMethod]
        public object showCopy(string newShowDate, string userName, string currentShowId)
        {
            try
            {
                wctMain.Admin.AdminContext _atx = new wctMain.Admin.AdminContext();
                Show currentShow =  _atx.ShowRepo_Web.Find(delegate(Show match)
                {
                    return (match.Id.ToString() == currentShowId);
                });

                if(currentShow == null)
                    currentShow = new Show(int.Parse(currentShowId));

                //validate date input
                DateTime dos = DateTime.Parse(newShowDate);

                //deny mistakes
                if (dos.Year < 1990)
                    throw new Exception("Please select a valid year.");

                //deny other shows that may exist at this date and time? - let them fail

                System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(userName);

                //make a new show!
                Show newShow = currentShow.CopyShow(dos, mem.UserName, (Guid)mem.ProviderUserKey);

                if (newShow != null)
                    return new { 
                        error = string.Empty,
                        newShowId = newShow.Id
                    
                    };

            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                return new {
                    newShowId = 0,
                    error = ex.Message
                };
            }

            return new
            {
                newShowId = 0,
                error = "There was some sort of error not caught."
            };
        }

        #endregion

        #region Contact

        [WebMethod]
        public string sendContactEmail(string emailAddress, string fromName, string subject, string message)
        {
            MailMessage msg = new MailMessage();

            try
            {
                emailAddress = emailAddress.Trim();
                if (!Utils.Validation.IsValidEmail(emailAddress))
                    throw new InvalidDataException("Please enter a valid email address.");

                fromName = Utils.ParseHelper.StripHtmlTags(fromName.Trim());
                subject = Utils.ParseHelper.StripHtmlTags(subject.Trim());
                message = message.Trim();

                //send the mail
                msg.IsBodyHtml = false;
                msg.From = new MailAddress(emailAddress, fromName);
                msg.To.Add(new MailAddress(_Config._CustomerService_Email, _Config._CustomerService_FromName));
                msg.Subject = string.Format("Contact Us - {0}", subject);

                string body = message;
                body = body.Insert(body.Length, string.Format("\r\n\r\n\r\n====================\r\n\r\n{0}", _ctx.UserInfo));
                msg.Body = body.Insert(0, string.Format("===================={2}From: {0} <{1}>{2}Subject: {3}{2}===================={2}{2}",
                    fromName, emailAddress, Environment.NewLine, subject));
                
                SmtpClient client = new SmtpClient();
                client.Send(msg);

                //body is ok here because it is less than max length of description column
                UserEvent.NewUserEvent(emailAddress, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, emailAddress,
                    _Enums.EventQContext.User, _Enums.EventQVerb.UserSentContactMessage, null, null, body, true);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                return string.Format("ERROR - {0}", ex.Message);
            }

            return "Your message has been sent!";
        }

        #endregion

        #region Writeup

        /// <summary>
        /// This will retrieve the entire writeup section for a show. Used for a modal control 
        /// to display the [show more...] info
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [WebMethod]
        public object getWriteup(string idx)
        {
            Show s = null;
            int _idx = 0;

            if (int.TryParse(idx, out _idx))
            {
                //for writeups - who cares if the show is displayable - any attempt to filter by displayable should have been handled beforehand
                s = _ctx.GetCurrentShowById(_idx, true, false);
            }

            return new
            {
                title = (s != null) ? s.Display.ShowHeader : string.Empty,

                // - showheader is legacy
                renderedView = 
                    
                    ((s != null) ?
                        string.Format("<div class=\"writeup-wrapper\">{0}{1}<div class=\"writeup-acts\">{2}</div><div class=\"writeup-container\">{3}</div></div>", 
                            (s.ShowTitle != null && s.ShowTitle.Trim().Length > 0) ? string.Format("<div class=\"writeup-title\">{0}</div>", s.ShowTitle.Trim()) : string.Empty,
                            (s.ShowHeader != null && s.ShowHeader.Trim().Length > 0) ? string.Format("<div class=\"writeup-showheader\">{0}</div>", s.ShowHeader) : string.Empty,

                            s.Display.AllActs_Markup_NoVerbose_NoLinks,

                            s.ShowWriteup_Derived.Replace("'", "&#39;")) : string.Empty)
            };
        }

        #endregion

        #region Social

        /// <summary>
        /// This will retrieve the entire writeup section for a show. Used for a modal control 
        /// to display the [show more...] info
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [WebMethod]
        public object getSocial(string eventDateIdx)
        {
            Show s = null;
            ShowDisplaySocials sosh = null;
            int _idx = 0;

            if (int.TryParse(eventDateIdx, out _idx))
            {
                //do not judge if the show is displayable - any attempt to filter by displayable should have been handled beforehand
                s = _ctx.GetCurrentShowByEventDateId(_idx, true, false);
                sosh = new ShowDisplaySocials(s);
            }

            return new
            {
                view =
                    (sosh != null) ? sosh.SocialOutput(true, "large", true) : string.Empty
            };
        }

        #endregion

    }    
}