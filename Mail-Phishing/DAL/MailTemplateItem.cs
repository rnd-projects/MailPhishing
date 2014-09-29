using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mail_Phishing.DAL;
using System.Data;

namespace Mail_Phishing.DAL
{
    public class MailTemplateItem
    {
        public int ID { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<MailTemplateItem> GetMailTemplates()
        {
            DataTable dt;
            List<MailTemplateItem> mailTemplates;

            dt = DBRoutines.GetAllMailTemplates();

            mailTemplates = dt.AsEnumerable().Select(
                row => new MailTemplateItem
                {
                    ID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row["ID"])),
                    MailBody = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row["MailBody"])),
                    MailSubject = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row["MailSubject"]))

                }).ToList();

            return mailTemplates;
        }

        public static List<MailTemplateItem> GetDummyData()
        {
            List<MailTemplateItem> templates = new List<MailTemplateItem>() 
            {
                new MailTemplateItem {
                    ID = 1,
                    MailSubject = "Mail Template 1 Title",
                    MailBody = "Mail Template 1 Body"
                },

                new MailTemplateItem {
                    ID = 2,
                    MailSubject = "Mail Template 2 Title",
                    MailBody = "Mail Template 2 Body"
                },

                new MailTemplateItem {
                    ID = 3,
                    MailSubject = "Mail Template 3 Title",
                    MailBody = "Mail Template 3 Body"
                }
            };

            return templates;
        }

        public static bool UpdateMailTemplate(MailTemplateItem template)
        {
            bool status = false;
            Dictionary<string, object> updateValues = new Dictionary<string, object>();

            if (template != null)
            {
                if (!string.IsNullOrEmpty(template.MailSubject))
                {
                    updateValues.Add("MailSubject", template.MailSubject);
                }

                if (!string.IsNullOrEmpty(template.MailBody))
                {
                    updateValues.Add("MailBody", template.MailBody);
                }

                status = DBRoutines.UpdateMailTemplateRecord(template.ID, updateValues);
            }

            return status;
        }
    }
}
