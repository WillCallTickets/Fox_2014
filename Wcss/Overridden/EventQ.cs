using System;

namespace Wcss
{
    public partial class EventQ
    {
        public static string CheckMailerSignupEventFrequency(Guid userId, string ip)
        {
            string result = null;

            //TODO: put these 4 values into config
            //set vars
            int userLimit = 4;
            DateTime userDate = DateTime.Now.AddHours(-12);
            int ipLimit = 12;
            DateTime ipDate = DateTime.Now.AddHours(-12);

            _Enums.EventQVerb verb = _Enums.EventQVerb.Mailer_SignupAwaitConfirm;

            SubSonic.QueryCommand cmdUser =
                       new SubSonic.QueryCommand("SELECT COUNT(eq.[Id]) FROM [EventQ] eq WHERE eq.[UserId] = @userId AND eq.[dtStamp] > @userDate AND eq.[Context] = @context AND eq.[Verb] = @verb; ", 
                           SubSonic.DataService.Provider.Name);
            cmdUser.Parameters.Add("@userId", userId.ToString());
            cmdUser.Parameters.Add("@context", _Enums.EventQContext.User.ToString());
            cmdUser.Parameters.Add("@verb", verb.ToString());
            cmdUser.Parameters.Add("@userDate", userDate, System.Data.DbType.DateTime);
            int pastUsers = (int)SubSonic.DataService.ExecuteScalar(cmdUser);

            if (pastUsers >= userLimit)
                result = "Please wait 12 hours before attempting another request.";


            if (result == null)
            {
                SubSonic.QueryCommand cmdIp =
                    new SubSonic.QueryCommand("SELECT COUNT(eq.[Id]) FROM [EventQ] eq WHERE eq.[Ip] = @ip AND eq.[dtStamp] > @ipDate AND eq.[Context] = @context AND eq.[Verb] = @verb; ", 
                        SubSonic.DataService.Provider.Name);
                cmdIp.Parameters.Add("@ip", ip);
                cmdIp.Parameters.Add("@context", _Enums.EventQContext.User.ToString());
                cmdIp.Parameters.Add("@verb", verb);
                cmdIp.Parameters.Add("@ipDate", ipDate, System.Data.DbType.DateTime);
                int pastIps = (int)SubSonic.DataService.ExecuteScalar(cmdIp);

                if (pastIps >= ipLimit)
                    result = "Please wait 12 hours before attempting another request.";

            }

            return result;
        }

        /// <summary>
        /// notifies admin of mailer issues - also badmail services. this requires processing to actually send the mail
        /// </summary>
        public static EventQ CreateMailerNotification(DateTime dateToProcess, string creatorName, string affectedUserName, _Enums.EventQVerb verb, string oldValue, 
            string newValue, string description)
        {
            Guid userId = Guid.Empty;
            AspnetUser usr = null;

            EventQ eventQ = new EventQ();
            eventQ.ApplicationId = _Config.APPLICATION_ID;
            eventQ.DateToProcess = dateToProcess;
            eventQ.CreatorName = creatorName;

            if (affectedUserName != null)
            {
                eventQ.UserName = affectedUserName;

                //config is handled by module
                usr = AspnetUser.GetUserByUserName(affectedUserName);

                if (usr != null)
                {
                    userId = usr.UserId;
                    eventQ.UserId = userId;
                }
                else
                {
                    eventQ.UserId = Guid.Empty;
                    //throw new Exception("User specified does not match application.");
                }
            }

            eventQ.Context = _Enums.EventQContext.Mailer.ToString();
            eventQ.Verb = verb.ToString();

            if(oldValue != null)
                eventQ.OldValue = oldValue;
            if(newValue != null)
                eventQ.NewValue = newValue;

            if(description != null)
                eventQ.Description = (description.Length >= 2000) ? description.Substring(0, 1999).Trim() : description.Trim();

            eventQ.DtStamp = DateTime.Now;
            eventQ.AttemptsRemaining = 3;
            eventQ.Ip = (System.Web.HttpContext.Current != null) ? System.Web.HttpContext.Current.Request.UserHostAddress : "127.0.0.1";

            eventQ.Save();

            if (usr != null)
            {
                UserEvent evt = new UserEvent();
                evt.TEventQId = eventQ.Id;
                evt.UserId = usr.UserId;
                evt.DtStamp = DateTime.Now;
                evt.Save();
            }

            return eventQ;
        }
        
        public static EventQ CreateAdminNotification(DateTime dateToProcess, string processGoneBad, string affectedUserName, 
            _Enums.EventQVerb verb, string oldValue, string newValue, string description)
        {
            Guid userId = Guid.Empty;
            AspnetUser usr = null;

            EventQ eventQ = new EventQ();
            eventQ.ApplicationId = _Config.APPLICATION_ID;
            eventQ.DateToProcess = dateToProcess;
            eventQ.CreatorName = processGoneBad;

            if (affectedUserName != null)
            {
                eventQ.UserName = affectedUserName;

                //config is handled by module
                usr = AspnetUser.GetUserByUserName(affectedUserName);

                if (usr != null)
                {
                    userId = usr.UserId;
                    eventQ.UserId = userId;
                }
                else
                {
                    eventQ.UserId = Guid.Empty;
                    //throw new Exception("User specified does not match application.");
                }
            }

            eventQ.Context = _Enums.EventQContext.AdminNotification.ToString();
            eventQ.Verb = verb.ToString();

            if (oldValue != null)
                eventQ.OldValue = oldValue;
            if (newValue != null)
                eventQ.NewValue = newValue;

            if (description != null)
                eventQ.Description = (description.Length >= 2000) ? description.Substring(0, 1999).Trim() : description.Trim();

            eventQ.DtStamp = DateTime.Now;
            eventQ.AttemptsRemaining = 3;
            eventQ.Ip = (System.Web.HttpContext.Current != null) ? System.Web.HttpContext.Current.Request.UserHostAddress : "127.0.0.1";

            eventQ.Save();

            if (usr != null)
            {
                UserEvent evt = new UserEvent();
                evt.TEventQId = eventQ.Id;
                evt.UserId = usr.UserId;
                evt.DtStamp = DateTime.Now;
                evt.Save();
            }

            return eventQ;
        }

        /// <summary>
        /// this may or may not require processing. If used for logging, provide a dateProcessed
        /// </summary>
        public static EventQ CreateChangeUserNameEvent(DateTime dateToProcess, DateTime? dateProcessed, string creatorName, string oldUserName,
            string newUserName, _Enums.EventQStatus status, string reason)
        {
            EventQ eventQ = new EventQ();
            eventQ.ApplicationId = _Config.APPLICATION_ID;
            eventQ.DateToProcess = dateToProcess;
            if (dateProcessed.HasValue)
                eventQ.DateProcessed = dateProcessed;

            //simply gets who did the operation
            AspnetUser usr = AspnetUser.GetUserByUserName(creatorName);
            
            Guid creatorId = (usr != null) ? usr.UserId : Guid.Empty;

            eventQ.CreatorId = creatorId;
            eventQ.CreatorName = creatorName;

            //at this point we have already updated the user
            string affectedUserName = (status == _Enums.EventQStatus.Success) ? newUserName.ToLower() : oldUserName.ToLower();
            AspnetUser affected = AspnetUser.GetUserByUserName(affectedUserName);

            if (affected == null)
                throw new Exception(string.Format("{0} does not exist in this application.", affectedUserName));

            Guid affectedUserId = (affected != null) ? affected.UserId : Guid.Empty;
            
            eventQ.UserId = affectedUserId;
            eventQ.UserName = oldUserName;//this should point to old address - in case of failure - also leaves a more proper trail
            
            eventQ.Context = _Enums.EventQContext.User.ToString();
            eventQ.Verb = _Enums.EventQVerb.ChangeUserName.ToString();

            eventQ.OldValue = oldUserName;
            eventQ.NewValue = newUserName;

            if (reason != null)
                eventQ.Description = (reason.Length >= 2000) ? reason.Substring(0, 1999).Trim() : reason.Trim();

            eventQ.Status = status.ToString();
            eventQ.DtStamp = DateTime.Now;
            eventQ.Ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            eventQ.Save();

            UserEvent ue = new UserEvent();
            ue.DtStamp = DateTime.Now;
            ue.UserId = affectedUserId;
            ue.TEventQId = eventQ.Id;;

            ue.Save();

            return eventQ;
        }

        /// <summary>
        /// Used for logging events to the event queue - where they have already been processed
        /// </summary>
        public static EventQ LogEvent(DateTime dateToProcess, DateTime dateProcessed, _Enums.EventQStatus status, string creatorName, 
            Guid affectedUserId, string affectedUserName, _Enums.EventQContext context, _Enums.EventQVerb verb, string oldValue, string newValue, 
            string description)
        {
            //log to eventQ as processed
            EventQ eventQ = new EventQ();
            eventQ.ApplicationId = _Config.APPLICATION_ID;
            if(dateToProcess != DateTime.MinValue)
                eventQ.DateToProcess = dateToProcess;
            if (dateProcessed != DateTime.MinValue)
                eventQ.DateProcessed = dateProcessed;
            eventQ.Status = status.ToString();

            AspnetUser usr = null;

            if (creatorName != null)
                usr = AspnetUser.GetUserByUserName(creatorName);

            Guid creatorId = (usr != null) ? usr.UserId : Guid.Empty;

            eventQ.CreatorId = creatorId;
            
            if (creatorName != null && creatorName.Trim().Length > 0)
                eventQ.CreatorName = creatorName;
            if (affectedUserId != null && affectedUserId != Guid.Empty)
                eventQ.UserId = affectedUserId;
            if (affectedUserName != null && affectedUserName.Trim().Length > 0)
            {
                eventQ.UserName = affectedUserName;
                if (affectedUserId == Guid.Empty)
                {
                    if (creatorName == affectedUserName)
                        eventQ.UserId = creatorId;
                    else
                    {
                        AspnetUser usr2 = null;
                        usr2 = AspnetUser.GetUserByUserName(affectedUserName);
                        if (usr2 != null)
                            eventQ.UserId = usr2.UserId;
                    }
                }
            }
            
            eventQ.Context = context.ToString();
            eventQ.Verb = verb.ToString();

            if(oldValue != null && oldValue.Trim().Length > 0)
                eventQ.OldValue = oldValue;
            if (newValue != null && newValue.Trim().Length > 0)
                eventQ.NewValue = newValue;
            if (description != null)
                eventQ.Description = (description.Length >= 2000) ? description.Substring(0, 1999).Trim() : description.Trim();

            eventQ.DtStamp = DateTime.Now;
            eventQ.Ip = (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null) ? 
                System.Web.HttpContext.Current.Request.UserHostAddress : string.Empty;

            eventQ.Save();

            return eventQ;
        }
    }
}


