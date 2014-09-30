using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel; 

namespace Mail_Phishing.DAL
{
    public class MailTemplate
    {
        public int ID { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
    }
}
