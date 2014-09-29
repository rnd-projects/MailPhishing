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
using Mail_Phishing.DAL;

namespace Mail_Phishing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InitializeDatabase dbInitializer;
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


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Initializes database upon object-creation
            dbInitializer = new InitializeDatabase();
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
            window.SelectedDLsListBox.ItemsSource = SelectedDLs;
            window.ShowDialog();
        }

        private void DLViewRecipientsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsDropDownButton_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void EditMailTemplatesButton_Click(object sender, RoutedEventArgs e)
        {
            //ORIGINAL MAIL SUBJECT is "SAMPLE MAIL 001"
            //var template = MailTemplate.GetMailTemplates().First();
            //template.MailSubject = template.MailSubject + (" --upd");
            //bool status = MailTemplate.UpdateMailTemplate(template);

            EditMailTemplatesWindow window = new EditMailTemplatesWindow();
            window.ShowDialog();
        }

        private void CreateNewMailTemplatesButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewMailTemplateWindow window = new CreateNewMailTemplateWindow();
            window.ShowDialog();
        }

    }

}
