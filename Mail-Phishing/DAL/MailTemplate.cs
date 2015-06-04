using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mail_Phishing.DAL
{
    public class MailTemplate
    {
        //
        // SCHEMA
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public string MailSubject { get; set; }

        [Required]
        public string MailBody { get; set; }

        [Required]
        public string EscapeCharacters { get; set; }


        //
        // Utils
        private static DBlib DBRoutines = new DBlib();


        //
        // Constructor
        public MailTemplate() { }


        //
        // Get, Add, Update.
        public static List<MailTemplate> GetMailTemplates()
        {
            DataTable dt;
            List<MailTemplate> templates;

            dt = DBRoutines.SELECT_ALL_FROM(TablesInfo.GetDescription(TablesInfo.MailTemplates.TableName));

            templates = dt.AsEnumerable().Select(
                row => new MailTemplate
                {
                    ID = Convert.ToInt32(Helpers.ReturnZeroIfNull(row[TablesInfo.GetDescription(TablesInfo.MailTemplates.ID)])),
                    MailSubject = Convert.ToString(Helpers.ReturnEmptyIfNull(row[TablesInfo.GetDescription(TablesInfo.MailTemplates.MailSubject)])),
                    MailBody = Convert.ToString(Helpers.ReturnEmptyIfNull(row[TablesInfo.GetDescription(TablesInfo.MailTemplates.MailBody)])),
                    EscapeCharacters = Convert.ToString(Helpers.ReturnEmptyIfNull(row[TablesInfo.GetDescription(TablesInfo.MailTemplates.EscapeSemicolons)]))
                }).ToList();


            foreach(var template in templates)
            {
                if(template.EscapeCharacters == "Y")
                {
                    template.MailBody = template.MailBody.Replace("###", ";");
                }
            }

            return templates;
        }


        public static int AdddMailTemplate(MailTemplate newTemplate)
        {
            int recordRowNumber = -1;
            Dictionary<string, object> values;

            if ( newTemplate != null && (!string.IsNullOrEmpty(newTemplate.MailSubject) && !string.IsNullOrEmpty(newTemplate.MailBody) && !string.IsNullOrEmpty(newTemplate.EscapeCharacters)) )
            {
                if (newTemplate.EscapeCharacters == "Y")
                {
                    newTemplate.MailBody = newTemplate.MailBody.Replace(";", "###");
                    newTemplate.MailBody = newTemplate.MailBody.Replace("'", "`");
                }

                values = new Dictionary<string, object>
                {
                    { TablesInfo.GetDescription(TablesInfo.MailTemplates.MailSubject), newTemplate.MailSubject },
                    { TablesInfo.GetDescription(TablesInfo.MailTemplates.MailBody), newTemplate.MailBody },
                    { TablesInfo.GetDescription(TablesInfo.MailTemplates.EscapeSemicolons), newTemplate.EscapeCharacters }
                };

                recordRowNumber = DBRoutines.INSERT(
                    TablesInfo.GetDescription(TablesInfo.MailTemplates.TableName),
                    values,
                    TablesInfo.GetDescription(TablesInfo.MailTemplates.ID));
            }

            return recordRowNumber;
        }


        public static bool EditMailTemplate(MailTemplate existingTemplate)
        {
            bool status = false;
            Dictionary<string, object> values;
            string tableName = TablesInfo.GetDescription(TablesInfo.MailTemplates.TableName);
            string tableIdField = TablesInfo.GetDescription(TablesInfo.MailTemplates.ID);

            if (existingTemplate != null)
            {
                values = new Dictionary<string, object>();

                values.Add(TablesInfo.GetDescription(TablesInfo.MailTemplates.EscapeSemicolons), existingTemplate.EscapeCharacters);

                if(!string.IsNullOrEmpty(existingTemplate.MailSubject))
                {
                    values.Add(TablesInfo.GetDescription(TablesInfo.MailTemplates.MailSubject), existingTemplate.MailSubject);
                }

                if (!string.IsNullOrEmpty(existingTemplate.MailBody))
                {
                    if (existingTemplate.EscapeCharacters == "Y")
                    {
                        existingTemplate.MailBody = existingTemplate.MailBody.Replace(";", "###");
                        existingTemplate.MailBody = existingTemplate.MailBody.Replace("'", "`");
                    }

                    values.Add(TablesInfo.GetDescription(TablesInfo.MailTemplates.MailBody), existingTemplate.MailBody);
                }

                status = DBRoutines.UPDATE(tableName, values, tableIdField, existingTemplate.ID);
            }

            return status;
        }

        //TODO: Implement DELETE
    }
}
