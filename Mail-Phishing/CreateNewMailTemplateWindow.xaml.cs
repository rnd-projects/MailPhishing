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
//using System.Data.Entity; 

using Mail_Phishing.DAL;

namespace Mail_Phishing
{
    /// <summary>
    /// Interaction logic for EditMailTemplates.xaml
    /// </summary>
    public partial class CreateNewMailTemplateWindow : Window
    {
        System.Windows.Data.CollectionViewSource mailTemplateViewSource;


        public CreateNewMailTemplateWindow()
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
            SaveDataButton.IsEnabled = false;
            EmailTemplateSubjectTextBox.IsReadOnly = true;
            EmailTemplateBodyRichTextBox.IsReadOnly = true;
        }

        private void UnlockWindowControls()
        {
            SaveDataButton.IsEnabled = true;
            EmailTemplateSubjectTextBox.IsReadOnly = false;
            EmailTemplateBodyRichTextBox.IsReadOnly = false;
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            MailTemplate newTemplate;

            string mailSubjectText = string.Empty;
            string mailBodyText = string.Empty;
            char[] disallowedCharacters = new char[] { '\r', '\n', '\t' };

            // Lock the controls
            LockWindowControls();

            mailSubjectText = EmailTemplateSubjectTextBox.Text.Trim();

            var textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                EmailTemplateBodyRichTextBox.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                EmailTemplateBodyRichTextBox.Document.ContentEnd
            );

            mailBodyText = textRange.Text.Trim();
            string[] tempStringArray = mailBodyText.Split(disallowedCharacters, StringSplitOptions.RemoveEmptyEntries);
            mailBodyText = String.Join(" ", tempStringArray);

            if (!string.IsNullOrEmpty(mailSubjectText) && !string.IsNullOrEmpty(mailBodyText))
            {
                // Update the mail templates
                newTemplate = new MailTemplate();

                newTemplate.MailSubject = mailSubjectText;
                newTemplate.MailBody = mailBodyText;

                MailTemplate.AdddMailTemplate(newTemplate);
            }

            // UnLock the controls
            UnlockWindowControls();
        }

    }//end-class

}
