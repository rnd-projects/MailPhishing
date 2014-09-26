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
        public static List<DistributionList> DLGridData;
        public static List<DistributionList> SelectedDLs;
        public static DistributionListUtil DLUtils = new DistributionListUtil();
        

        public MainWindow()
        {
            InitializeComponent();

            SelectedDLs = new List<DistributionList>();

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

        private void UpdateStatusBarLabel(int selected = -1, int searchMatch = -1)
        {
            string statusBarLabelText = string.Empty;

            //Handle default values
            if(selected < 0) selected = DLGrid.SelectedItems.Count;
            if(searchMatch < 0) searchMatch = (string.IsNullOrEmpty(DLSearchTextbox.Text) ? 0 :  DLGrid.Items.Count);

            //Handle the presence or absence of a filtered collection of items
            statusBarLabelText = String.Format("Selected: {0}", selected);

            if (searchMatch > 0)
            {
                statusBarLabelText = String.Format("{0} | Search Match: {1}", statusBarLabelText, searchMatch);
            }

            //Bind the local status string to the status bar label
            StatusBarLabel.Content = statusBarLabelText;
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
                searchText = searchText.ToLower();

                var temp = DLGridData.Where(item => item.CN.ToLower().Contains(searchText)).ToList();

                DLGrid.ItemsSource = temp;

                UpdateStatusBarLabel(searchMatch: temp.Count);
            }
            else
            {
                DLGrid.ItemsSource = DLGridData;
                UpdateStatusBarLabel(searchMatch: 0);
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

            UpdateStatusBarLabel(selected: DLGrid.SelectedItems.Count);
        }

        private void DLSendMailButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDLs = new List<DistributionList>();

            foreach (var dl in DLGrid.SelectedItems)
            {
                SelectedDLs.Add((DistributionList)dl);
            }

            SelectMailTemplateWindow window = new SelectMailTemplateWindow();
            window.Show();
        }

        private void DLViewRecipientsButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }

}
