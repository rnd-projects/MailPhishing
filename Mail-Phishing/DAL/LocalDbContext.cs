using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity; 

namespace Mail_Phishing.DAL
{
    class LocalDbContext : DbContext
    {
        public DbSet<MailTemplate> MailTemplates { get; set; } 
    }
}
