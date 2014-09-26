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

        public SelectMailTemplateWindow()
        {
            InitializeComponent();

            mailTemplates = MailTemplate.GetMailTemplates();

            ListOfEmails.ItemsSource = mailTemplates;
            ListOfEmails.DisplayMemberPath = "MailTitle";
        }

        private void ConfirmSendMail_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListOfEmails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                MailTemplate selectedTemplate = e.AddedItems[0] as MailTemplate;

                ReviewEmailTextBlock.Text = selectedTemplate.MailBody;
            }
        }
    }
}
