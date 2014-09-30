using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail_Phishing.DAL
{
    class InitializeDatabase
    {
        private LocalServiceDbContext _db = new LocalServiceDbContext();
        private List<MailTemplate> mailTemplates = new List<MailTemplate>();

        //
        // Constructor
        public InitializeDatabase()
        {
            //mailTemplates = _db.MailTemplates.Local.ToList();

            //if (mailTemplates.Count == 0)
            //{
            //    mailTemplates = GetSampleMailTemplates();

            //    foreach (var template in mailTemplates)
            //    {
            //        _db.MailTemplates.AddRange(mailTemplates);
            //        _db.SaveChanges();
            //    }
            //}
        }

        //
        // Return a list of sample mail templates
        private List<MailTemplate> GetSampleMailTemplates(int number = 5)
        {
            List<MailTemplate> sampleMailTemplates = new List<MailTemplate>();

            for (int i = 0; i < number; i++)
            {
                var sample = new MailTemplate();
                sample.MailSubject = "Sample Mail";
                sample.MailBody = "Sample Mail Template Body";

                sampleMailTemplates.Add(sample);
            }

            return sampleMailTemplates;
        }

    }

}
