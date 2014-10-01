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
    public partial class EditMailTemplatesWindow : Window
    {
        System.Windows.Data.CollectionViewSource mailTemplateViewSource;


        public EditMailTemplatesWindow()
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
            SaveChangesButton.IsEnabled = false;
            EmailTemplateSubjectTextBox.IsReadOnly = true;
            EmailTemplateBodyRichTextBox.IsReadOnly = true;
        }

        private void UnlockWindowControls()
        {
            SaveChangesButton.IsEnabled = true;
            EmailTemplateSubjectTextBox.IsReadOnly = false;
            EmailTemplateBodyRichTextBox.IsReadOnly = false;
        }

        private void EmailTemplatesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmailTemplatesComboBox.SelectedIndex > -1)
            {
                var template = EmailTemplatesComboBox.SelectedItem as MailTemplate;

                EmailTemplateSubjectTextBox.Text = template.MailSubject;

                EmailTemplateBodyRichTextBox.Document.Blocks.Clear();
                EmailTemplateBodyRichTextBox.AppendText(template.MailBody);

                UnlockWindowControls();
            }
            else
            {
                LockWindowControls();
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            bool status = false;
            string mailSubjectText = string.Empty;
            string mailBodyText = string.Empty;
            MailTemplate selectedTemplate;
            char[] disallowedCharacters = new char[] { '\r', '\n', '\t' };

            if(EmailTemplatesComboBox.SelectedIndex > -1)
            {
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
                mailBodyText = String.Join("<br />", tempStringArray);

                if (!string.IsNullOrEmpty(mailSubjectText) && !string.IsNullOrEmpty(mailBodyText))
                {
                    // Update the mail templates
                    selectedTemplate = EmailTemplatesComboBox.SelectedItem as MailTemplate;

                    selectedTemplate.MailSubject = mailSubjectText;
                    selectedTemplate.MailBody = mailBodyText;

                    status = MailTemplate.EditMailTemplate(selectedTemplate);

                    mailTemplateViewSource.Source = MailTemplate.GetMailTemplates();

                    MessageBox.Show("Changes were saved successfully.", "ISD Mail Phishing", MessageBoxButton.OK);
                }

                // UnLock the controls
                UnlockWindowControls();
            }
        }

    }//end-class

}
