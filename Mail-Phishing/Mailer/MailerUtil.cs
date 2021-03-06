﻿using System;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Generic;

//using System.Diagnostics;
//using System.Runtime.InteropServices;

using Mail_Phishing.DAL;

namespace Mail_Phishing.Mailer
{
    public class MailerUtil
    {
        private SmtpClient client = new SmtpClient();
        private string mailhost = ConfigurationManager.AppSettings["MailHost"];
        private MailAddress replyTo = new MailAddress(ConfigurationManager.AppSettings["ReplyTo"]);
        private string emailDisplayName = Convert.ToString(ConfigurationManager.AppSettings["SenderDisplayName"]);
        
        public delegate List<string> GetDistributionListMembersDelegate(string DNORFILTER);


        public void SendMail(string emailAddress, string templateSubject, string templateBody)
        {
            MailAddress from = new MailAddress(replyTo.Address, emailDisplayName);
            MailAddress to = new MailAddress(@emailAddress);
            MailMessage mail = new MailMessage(from, to);

            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = mailhost;

            //mail.ReplyToList = {replyto};
            mail.IsBodyHtml = true;
            mail.Subject = templateSubject;
            mail.Body = templateBody;

            client.Send(mail);
        }

        public void SendMail(Delegate method, MailTemplate template, params object[] args)
        {
            List<string> emailAddresses = (List<string>)method.DynamicInvoke(args);

            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = mailhost;

            if (emailAddresses.Count > 0)
            {
                MailAddress from = new MailAddress(replyTo.Address, emailDisplayName);

                foreach (string emailAddress in emailAddresses)
                {
                    MailAddress to = new MailAddress(emailAddress);
                    MailMessage mail = new MailMessage(from,to);

                    mail.IsBodyHtml = true;
                    mail.Subject = template.MailSubject;
                    mail.Body = template.MailBody;

                    //client.Send(mail);
                }
            }//end-if

        }

    }

}
