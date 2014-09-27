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
using System.Data.Entity;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace Mail_Phishing.DAL
{
    public class DBLib
    {
        private string ConnectionString = string.Empty;

        public DBLib()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Mail_Phishing.Properties.Settings.LocalDBConnectionString"].ConnectionString;
        }

        private SqlCeConnection DBInitializeConnection(string connectionString)
        {
            return new SqlCeConnection(connectionString);
        }

        public DataTable GetAllMailTemplates()
        {
            string selectQuery;
            SqlCeDataReader dr;
            DataTable dt = new DataTable();
            
            selectQuery = String.Format("SELECT MailTemplates.* FROM MailTemplates");

            SqlCeConnection conn = DBInitializeConnection(ConnectionString);
            SqlCeCommand comm = new SqlCeCommand(selectQuery, conn);

            try
            {
                conn.Open();
                dr = comm.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }


        public bool UpdateMailTemplateRecord(int templateID, Dictionary<string, object> values)
        {
            string updateQuery = string.Empty;
            string tableName = "MailTemplates";
            string tableIDFieldName = "ID";
            StringBuilder fieldsValues = new StringBuilder();

            foreach (KeyValuePair<string, object> pair in values)
            {
                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                {
                    fieldsValues.Append("[" + pair.Key + "]=" + pair.Value + ",");
                }
                else
                {
                    fieldsValues.Append("[" + pair.Key + "]=" + "'" + pair.Value + "'" + ",");
                }
            }

            fieldsValues.Remove(fieldsValues.Length - 1, 1);

            updateQuery = String.Format("UPDATE {0} SET {1} WHERE {2}={3}", tableName, fieldsValues, tableIDFieldName, templateID);

            SqlCeConnection conn = DBInitializeConnection(ConnectionString);
            SqlCeCommand comm = new SqlCeCommand(updateQuery, conn);
            
            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally
            {
                conn.Close();
            }
        }

    }//end-class

}
