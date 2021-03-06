﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using System.Data.Entity;

using Mail_Phishing.DAL;
using Mail_Phishing.Mailer;

namespace Mail_Phishing
{
    /// <summary>
    /// Interaction logic for SelectMailTemplateWindow.xaml
    /// </summary>
    public partial class SelectMailTemplateWindow : Window
    {
        System.Windows.Data.CollectionViewSource mailTemplateViewSource;

        private static MailerUtil MailerUtils = new MailerUtil();
        private static DistributionListUtil DLUtils = new DistributionListUtil();
        private delegate List<string> GetMembersEmail(string DNORFILTER);

        
        public SelectMailTemplateWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (mailTemplateViewSource == null)
            {
                mailTemplateViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("mailTemplateViewSource")));
            }

            mailTemplateViewSource.Source = MailTemplate.GetMailTemplates();
        }


        private void LockWindowControls()
        {
            EmailTemplatesComboBox.IsEnabled = false;
            ConfirmSendMailButton.IsEnabled = false;
        }

        private void UnlockWindowControls()
        {
            EmailTemplatesComboBox.IsEnabled = true;
            ConfirmSendMailButton.IsEnabled = true;
        }


        private void ConfirmSendMailButton_Click(object sender, RoutedEventArgs e)
        {
            //Lock the controls first
            LockWindowControls();

            //create appropriate functions for DL and DDL
            GetMembersEmail DLEmails = new GetMembersEmail(DLUtils.listDLMembers);
            GetMembersEmail DDLEmails = new GetMembersEmail(DLUtils.listDDLMembers);

            //Calling It directly
            Func<string, List<string>> dluHandler = (param) => DLEmails(param);
            Func<string, List<string>> ddluHandler = (param) => DDLEmails(param);

            //calling it through Mailer
            //MailerUtils.SendMail(ddluHandler, new object[] { "(&(physicalDeliveryOfficeName=MOA)(!(name=SystemMailbox{*))(!(name=CAS_{*))(!(msExchRecipientTypeDetails=16777216))(!(msExchRecipientTypeDetails=536870912))(!(msExchRecipientTypeDetails=8388608)))" });

            //Get the selected mail template
            MailTemplate template = EmailTemplatesComboBox.SelectedItem as MailTemplate;

            //Get selected dls and fetch their users emails
            List<DistributionList> selectedDLs = new List<DistributionList>();
            List<string> emails = new List<string>();

            string mailBody = template.MailBody;
            mailBody = template.MailBody.Replace("%%%EMAIL%%%", "aalhour@ccc.gr");
            MailerUtils.SendMail("aalhour@ccc.gr", template.MailSubject, mailBody);

            mailBody = template.MailBody.Replace("%%%EMAIL%%%", "skahoush@ccc.gr");
            MailerUtils.SendMail("skahoush@ccc.gr", template.MailSubject, mailBody);

            mailBody = template.MailBody.Replace("%%%EMAIL%%%", "mali@ccc.gr");
            MailerUtils.SendMail("mali@ccc.gr", template.MailSubject, mailBody);

            //foreach (var dl in SelectedDLsListBox.Items)
            //{
            //    var castedDL = (DistributionList)dl;
            //    selectedDLs.Add(castedDL);

            //    if (castedDL.DType.Equals(DLT.DL))
            //        emails.AddRange(DLEmails(castedDL.FILORDN));
            //    else
            //        emails.AddRange(DDLEmails(castedDL.FILORDN));
            //}

            ////FINAL CODE
            //foreach (var dl in selectedDLs)
            //{
            //    if (dl.DType.Equals(DLT.DL))
            //    {
            //        //MailerUtils.SendMail(dluHandler, template, new object[] { dl.FILORDN });
            //    }
            //    else
            //    {
            //        //MailerUtils.SendMail(ddluHandler, template, new object[] { dl.FILORDN });
            //    }
            //}


            //var greeceEmails = emails.Where(mail => mail.Contains("@ccc.gr")).Distinct().ToList();
            //emails = emails.Where(mail => mail.Contains("@ccc.gr") == false).Distinct().Reverse().ToList();

            //int counter = 0;

            //foreach (var mail in emails)
            //{
            //    MailerUtils.SendMail(mail, template.MailSubject, template.MailBody);

            //    counter++;

            //    if (counter % 1000 == 0)
            //    {
            //        string x = string.Empty;
            //    }
            //}

            //Show message box
            MessageBox.Show("Emails were sent successfully.", "ISD Mail Phishing", MessageBoxButton.OK);

            //Unlock the windows controls
            UnlockWindowControls();
        }

        private void ListOfEmails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                MailTemplate selectedTemplate = e.AddedItems[0] as MailTemplate;

                //ReviewEmailTextBlock.SelectAll();
                //ReviewEmailTextBlock.Selection.Text = "";
                ReviewEmailTextBlock.Document.Blocks.Clear();
                ReviewEmailTextBlock.AppendText(selectedTemplate.MailBody);

                ConfirmSendMailButton.IsEnabled = true;
            }
            else
            {
                ConfirmSendMailButton.IsEnabled = false;
            }
        }

    }

}
