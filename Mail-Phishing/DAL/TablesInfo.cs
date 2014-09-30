using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mail_Phishing.DAL
{
    public static class TablesInfo
    {
        public enum MailTemplates
        {
            [Description("MailTemplates")]
            TableName,
            [Description("Id")]
            ID,
            [Description("MailSubject")]
            MailSubject,
            [Description("MailBody")]
            MailBody
        }



        /**
         * Functions
         */
        /// <summary>
        /// Gets the Description attribute of enum
        /// </summary>
        /// <param name="value">Enum Name</param>
        /// <returns>Field Description</returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] descAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descAttributes != null && descAttributes.Length > 0)
                return descAttributes[0].Description;
            else
                return value.ToString();
        }


        /// <summary>
        /// Gets the DefaultValue attribute of the enum
        /// </summary>
        /// <param name="value">Enum Name</param>
        /// <returns>Field Description</returns>
        public static string GetValue(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DefaultValueAttribute[] valueAttributes = (DefaultValueAttribute[])fieldInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false);

            if (valueAttributes != null && valueAttributes.Length > 0)
                return valueAttributes[0].Value.ToString();
            else
                return value.ToString();
        }
    }
}
