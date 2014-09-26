using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using Mail_Phishing.Mailer;

namespace Mail_Phishing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<DistributionList> DLGridData;
        public static DistributionListUtil DLUtils = new DistributionListUtil();
        

        public MainWindow()
        {
            InitializeComponent();

            DLGridData = GetDLGridData();
            DLGrid.ItemsSource = DLGridData;
        }


        private List<DistributionList> GetDLGridData()
        {
            List<DistributionList> data = new List<DistributionList>();

            data = DLUtils.GetDistributionLists();
            data.AddRange(DLUtils.GetDynamicDistributionLists());

            data = data.OrderBy(item => item.CN).ToList();

            return data;
        }

        //private void SampleProgramCode()
        //{
        //    MailerUtil mailer = new MailerUtil();
        //    DistributionListUtil dlu = new DistributionListUtil();

        //    //create appropriate functions for DL and DDL
        //    GetMembersEmail DLEmails = new GetMembersEmail(dlu.listDLMembers);
        //    GetMembersEmail DDLEmails = new GetMembersEmail(dlu.listDDLMembers);

        //    //MailerUtil.GetDistributionListMembersDelegate dluHandler = new MailerUtil.GetDistributionListMembersDelegate(dlu.GetDistributionListMembers);

        //    //Calling It directly
        //    Func<string, List<string>> dluHandler = (param) => DLEmails(param);
        //    Func<string, List<string>> ddluHandler = (param) => DDLEmails(param);
        //    //List<string> xxx = ddluHandler.Invoke("(&(physicalDeliveryOfficeName=MOA)(!(name=SystemMailbox{*))(!(name=CAS_{*))(!(msExchRecipientTypeDetails=16777216))(!(msExchRecipientTypeDetails=536870912))(!(msExchRecipientTypeDetails=8388608)))");

        //    //dlu.GetDynamicDistributionLists();
        //    //dlu.GetDistributionLists();

        //    //calling it through Mailer
        //    mailer.SenMail(ddluHandler, new object[] { "(&(physicalDeliveryOfficeName=MOA)(!(name=SystemMailbox{*))(!(name=CAS_{*))(!(msExchRecipientTypeDetails=16777216))(!(msExchRecipientTypeDetails=536870912))(!(msExchRecipientTypeDetails=8388608)))" });
        //}

        private void DLSearchSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = DLSearchTextbox.Text;

            if (!string.IsNullOrEmpty(searchText))
            {
                var temp = DLGridData;

                searchText = searchText.ToLower();

                temp = temp.Where(item => item.CN.ToLower().Contains(searchText)).ToList();

                DLGrid.ItemsSource = temp;
            }
            else
            {
                DLGrid.ItemsSource = DLGridData;
            }
        }

        private void DLSearchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = DLSearchTextbox.Text;
            
            if (!string.IsNullOrEmpty(searchText))
            {
                var temp = DLGridData;

                searchText = searchText.ToLower();

                temp = temp.Where(item => item.CN.ToLower().Contains(searchText)).ToList();

                DLGrid.ItemsSource = temp;
            }
            else
            {
                DLGrid.ItemsSource = DLGridData;
            }
        }

        private void DLGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DLGrid.SelectedItems.Count > 0)
            {
                DLSendMailButton.IsEnabled = true;
            }
            else
            {
                DLSendMailButton.IsEnabled = false;
            }
        }

        private void DLSendMailButton_Click(object sender, RoutedEventArgs e)
        {
            List<DistributionList> selectedDLs = new List<DistributionList>();

            foreach (var dl in DLGrid.SelectedItems)
            {
                selectedDLs.Add((DistributionList)dl);
            }
        }

    }

}
