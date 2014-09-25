using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Configuration;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mail_Phishing.Mailer
{
    public class MailerUtil
    {
        private SmtpClient client = new SmtpClient();
        private string mailhost = ConfigurationManager.AppSettings["MailHost"];
        private MailAddress notificationsEmail = new MailAddress(ConfigurationManager.AppSettings["NotificationsEmail"]);
        private MailAddress replyTo = new MailAddress(ConfigurationManager.AppSettings["ReplyTo"]);

        public delegate List<string> GetDistributionListMembersDelegate(string DNORFILTER);

        public void SenMail(string emailAddress, string templateSubject, string templateBody)
        {
            MailMessage mail = new MailMessage(notificationsEmail.Address, @emailAddress);
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

        public void SenMail(Delegate method, params object[] args)
        {
            List<string> emailAddresses = (List<string>)method.DynamicInvoke(args);
            //List<string> emailAddresses = new List<string>();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = mailhost;

            //MailMessage mail = new MailMessage(notificationsEmail.Address, @emailAddress);
            if (emailAddresses.Count > 0)
            {

                foreach (string emailAddress in emailAddresses)
                {

                    MailMessage mail = new MailMessage(replyTo.Address, emailAddress);

                    mail.IsBodyHtml = true;
                    mail.Subject = "this is a spam test";
                    mail.Body = "this is a spam test ";

                    client.Send(mail);
                }
            }

        }

    }

}
