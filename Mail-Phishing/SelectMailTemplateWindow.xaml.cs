using System;
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

using Mail_Phishing.Mailer;

namespace Mail_Phishing
{
    /// <summary>
    /// Interaction logic for SelectMailTemplateWindow.xaml
    /// </summary>
    public partial class SelectMailTemplateWindow : Window
    {
        private List<MailTemplate> mailTemplates;
        private static MailerUtil MailerUtils = new MailerUtil();
        private static DistributionListUtil DLUtils = new DistributionListUtil();
        private delegate List<string> GetMembersEmail(string DNORFILTER);

        
        public SelectMailTemplateWindow()
        {
            InitializeComponent();

            mailTemplates = MailTemplate.GetMailTemplates();

            EmailTemplatesComboBox.ItemsSource = mailTemplates;
            EmailTemplatesComboBox.DisplayMemberPath = "MailTitle";
        }


        private void ConfirmSendMailButton_Click(object sender, RoutedEventArgs e)
        {
            //create appropriate functions for DL and DDL
            GetMembersEmail DLEmails = new GetMembersEmail(DLUtils.listDLMembers);
            GetMembersEmail DDLEmails = new GetMembersEmail(DLUtils.listDDLMembers);

            //MailerUtil.GetDistributionListMembersDelegate dluHandler = new MailerUtil.GetDistributionListMembersDelegate(dlu.GetDistributionListMembers);

            //Calling It directly
            Func<string, List<string>> dluHandler = (param) => DLEmails(param);
            Func<string, List<string>> ddluHandler = (param) => DDLEmails(param);
            //List<string> xxx = ddluHandler.Invoke("(&(physicalDeliveryOfficeName=MOA)(!(name=SystemMailbox{*))(!(name=CAS_{*))(!(msExchRecipientTypeDetails=16777216))(!(msExchRecipientTypeDetails=536870912))(!(msExchRecipientTypeDetails=8388608)))");

            //dlu.GetDynamicDistributionLists();
            //dlu.GetDistributionLists();

            //calling it through Mailer
            //MailerUtils.SendMail(ddluHandler, new object[] { "(&(physicalDeliveryOfficeName=MOA)(!(name=SystemMailbox{*))(!(name=CAS_{*))(!(msExchRecipientTypeDetails=16777216))(!(msExchRecipientTypeDetails=536870912))(!(msExchRecipientTypeDetails=8388608)))" });

            List<DistributionList> selectedDLs = new List<DistributionList>();
            List<string> emails = new List<string>();

            foreach (var dl in SelectedDLsListBox.Items)
            {
                var castedDL = (DistributionList)dl;

                selectedDLs.Add(castedDL);

                if (castedDL.DType.Equals(DLT.DL))
                    emails.AddRange(DLEmails(castedDL.FILORDN));
                else
                    emails.AddRange(DDLEmails(castedDL.FILORDN));
            }
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

        private void ListOfSelectedDLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
