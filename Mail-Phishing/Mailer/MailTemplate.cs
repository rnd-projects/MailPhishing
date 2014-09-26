using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail_Phishing.Mailer
{
    public class MailTemplate
    {
        public int ID { get; set; }
        public string MailTitle { get; set; }
        public string MailDescription { get; set; }
        public string MailBody { get; set; }

        public static List<MailTemplate> GetMailTemplates()
        {
            List<MailTemplate> templates = new List<MailTemplate>() 
            {
                new MailTemplate {
                    ID = 1,
                    MailTitle = "Mail Template 1 Title",
                    MailDescription = "Mail Template 1 Description",
                    MailBody = "Mail Template 1 Body"
                },

                new MailTemplate {
                    ID = 2,
                    MailTitle = "Mail Template 2 Title",
                    MailDescription = "Mail Template 2 Description",
                    MailBody = "Mail Template 2 Body"
                },

                new MailTemplate {
                    ID = 3,
                    MailTitle = "Mail Template 3 Title",
                    MailDescription = "Mail Template 3 Description",
                    MailBody = "Mail Template 3 Body"
                }
            };

            return templates;
        }
    }
}
