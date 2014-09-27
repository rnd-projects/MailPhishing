﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mail_Phishing.DAL;
using System.Data;

namespace Mail_Phishing.DAL
{
    public class MailTemplate
    {
        public int ID { get; set; }
        public string MailSubject { get; set; }
        public string MailDescription { get; set; }
        public string MailBody { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<MailTemplate> GetMailTemplates()
        {
            DataTable dt;
            List<MailTemplate> mailTemplates;

            dt = DBRoutines.GetAllMailTemplates();

            mailTemplates = dt.AsEnumerable().Select(
                row => new MailTemplate
                {
                    ID = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row["ID"])),
                    MailBody = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row["MailBody"])),
                    MailSubject = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row["MailSubject"]))

                }).ToList();

            return mailTemplates;
        }

        public static List<MailTemplate> GetDummyData()
        {
            List<MailTemplate> templates = new List<MailTemplate>() 
            {
                new MailTemplate {
                    ID = 1,
                    MailSubject = "Mail Template 1 Title",
                    MailDescription = "Mail Template 1 Description",
                    MailBody = "Mail Template 1 Body"
                },

                new MailTemplate {
                    ID = 2,
                    MailSubject = "Mail Template 2 Title",
                    MailDescription = "Mail Template 2 Description",
                    MailBody = "Mail Template 2 Body"
                },

                new MailTemplate {
                    ID = 3,
                    MailSubject = "Mail Template 3 Title",
                    MailDescription = "Mail Template 3 Description",
                    MailBody = "Mail Template 3 Body"
                }
            };

            return templates;
        }
    }
}