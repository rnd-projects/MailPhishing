using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace Mail_Phishing.DAL
{
    public class DBlib
    {
        private static string ConnectionString_Lync = ConfigurationManager.ConnectionStrings["MailPhishingContext"].ConnectionString.ToString();

        private OleDbConnection DBInitializeConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }

        //
        // DATABASE SELECT, INSERT, UPDATE
        public DataTable SELECT_ALL_FROM(string tableName)
        {
            OleDbDataReader dr;
            OleDbConnection conn;
            OleDbCommand comm;
            DataTable dt = new DataTable();
            string selectQuery = string.Empty;

            //Construct selectQuery
            selectQuery = string.Format("SELECT * FROM [{0}]", tableName);

            conn = DBInitializeConnection(ConnectionString_Lync);
            comm = new OleDbCommand(selectQuery, conn);

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


        public int INSERT(string tableName, Dictionary<string, object> columnsValues, string idFieldName)
        {
            OleDbConnection conn;
            OleDbCommand comm;
            StringBuilder whereStatement = new StringBuilder();
            StringBuilder fields = new StringBuilder(); fields.Append("(");
            StringBuilder values = new StringBuilder(); values.Append("(");
            

            //Fields and values
            foreach (KeyValuePair<string, object> pair in columnsValues)
            {
                fields.Append("[" + pair.Key + "],");

                if (pair.Value == null)
                {
                    values.Append("NULL" + ",");
                }
                else if (pair.Value is int || pair.Value is Double)
                {
                    values.Append(pair.Value + ",");
                }
                else if (pair.Value is DateTime && (DateTime)pair.Value == DateTime.MinValue)
                {
                    continue;
                }
                else
                {
                    values.Append("'" + pair.Value.ToString().Replace("'", "`") + "'" + ",");
                }
            }

            fields.Remove(fields.Length - 1, 1).Append(")");
            values.Remove(values.Length - 1, 1).Append(")");

            string insertQuery = string.Format("INSERT INTO [{0}] {1} OUTPUT INSERTED.{2}  VALUES {3}", tableName, fields, idFieldName, values);

            conn = DBInitializeConnection(ConnectionString_Lync);
            comm = new OleDbCommand(insertQuery, conn);

            int recordID = 0;
            try
            {
                conn.Open();
                recordID = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }

            return recordID;
        }


        public bool UPDATE(string tableName, Dictionary<string, object> columnsValues, string idFieldName, Int64 ID)
        {
            OleDbConnection conn;
            OleDbCommand comm;
            StringBuilder fieldsValues = new StringBuilder();
            string insertQuery = string.Empty;


            foreach (KeyValuePair<string, object> pair in columnsValues)
            {

                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                {
                    fieldsValues.Append("[" + pair.Key + "]=" + pair.Value + ",");
                }
                else if (valueType == typeof(DateTime) && (DateTime)pair.Value == DateTime.MinValue)
                {
                    continue;
                }
                else
                {
                    fieldsValues.Append("[" + pair.Key + "]=" + "'" + pair.Value + "'" + ",");
                }
            }

            fieldsValues.Remove(fieldsValues.Length - 1, 1);

            insertQuery = string.Format("UPDATE  [{0}] SET {1} WHERE [{2}]={3}", tableName, fieldsValues, idFieldName, ID);

            conn = DBInitializeConnection(ConnectionString_Lync);
            comm = new OleDbCommand(insertQuery, conn);
            comm.CommandTimeout = 360;

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
