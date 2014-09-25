﻿using System;
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

        public MainWindow()
        {
            InitializeComponent();
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

            DLGridData = new List<DistributionList>()
            {
                new DistributionList { DType = DLT.DL, CN = "CN-01", FILORDN = "FILORDN-01" },
                new DistributionList { DType = DLT.DL, CN = "CN-02", FILORDN = "FILORDN-02" },
                new DistributionList { DType = DLT.DDL, CN = "CN-03", FILORDN = "FILORDN-03" },
                new DistributionList { DType = DLT.DDL, CN = "CN-04", FILORDN = "FILORDN-04" }
            };

            DLGrid.ItemsSource = DLGridData;
        }

        private void DLSearchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DLGrid_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

    }

}
