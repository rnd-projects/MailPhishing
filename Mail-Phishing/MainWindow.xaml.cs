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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mail_Phishing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //DLGrid.Columns = 
        }

        private void SampleProgramCode()
        {
            //MailerUtil mailer = new MailerUtil();
            //DistributionListUtil dlu = new DistributionListUtil();

            ////create appropriate functions for DL and DDL
            //GetMembersEmail DLEmails = new GetMembersEmail(dlu.listDLMembers);
            //GetMembersEmail DDLEmails = new GetMembersEmail(dlu.listDDLMembers);

            ////MailerUtil.GetDistributionListMembersDelegate dluHandler = new MailerUtil.GetDistributionListMembersDelegate(dlu.GetDistributionListMembers);



            ////Calling It directly
            //Func<string, List<string>> dluHandler = (param) => DLEmails(param);
            //Func<string, List<string>> ddluHandler = (param) => DDLEmails(param);
            ////List<string> xxx = ddluHandler.Invoke("(&(physicalDeliveryOfficeName=MOA)(!(name=SystemMailbox{*))(!(name=CAS_{*))(!(msExchRecipientTypeDetails=16777216))(!(msExchRecipientTypeDetails=536870912))(!(msExchRecipientTypeDetails=8388608)))");

            ////dlu.GetDynamicDistributionLists();
            ////dlu.GetDistributionLists();

            ////calling it through Mailer
            //mailer.SenMail(ddluHandler, new object[] { "(&(physicalDeliveryOfficeName=MOA)(!(name=SystemMailbox{*))(!(name=CAS_{*))(!(msExchRecipientTypeDetails=16777216))(!(msExchRecipientTypeDetails=536870912))(!(msExchRecipientTypeDetails=8388608)))" });
        }

        private void DLSearchSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = DLSearchTextbox.Text;

        }

        private void DLSearchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
