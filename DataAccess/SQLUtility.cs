using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace LW1190.DataAccess
{
    public class SQLUtility : IDisposable
    {
        int iTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

        // Create a new dictionary of strings, with string keys
        Dictionary<string, object> ParamList = new Dictionary<string, object>();

        public void Dispose()
        {
            ParamList.Clear();
            ParamList = null;
        }


        public void AddParameters(string Parameter_Name, object Parameter_Value)
        {
            ParamList.Add(Parameter_Name, Parameter_Value);
        }

        public void ExecActionStoredProc(string sStoredProc)
        {
            string sCnn = ConfigurationManager.ConnectionStrings[Global.CONN_STR_NAME_RPT].ConnectionString;

            using (SqlConnection cnn = new SqlConnection(sCnn))
            {
                using (SqlCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandTimeout = iTimeout;
                    cmd.CommandText = sStoredProc;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // When you use foreach to enumerate dictionary elements,
                    // the elements are retrieved as KeyValuePair objects.
                    foreach (KeyValuePair<string, object> kvp in ParamList)
                    {
                        cmd.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                    }

                    cnn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable ExecStoredProc(string sStoredProc)
        {
            string sCnn = ConfigurationManager.ConnectionStrings[Global.CONN_STR_NAME_RPT].ConnectionString;

            using (SqlConnection cnn = new SqlConnection(sCnn))
            {
                using (SqlCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandTimeout = iTimeout;
                    cmd.CommandText = sStoredProc;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // When you use foreach to enumerate dictionary elements,
                    // the elements are retrieved as KeyValuePair objects.
                    foreach (KeyValuePair<string, object> kvp in ParamList)
                    {
                        cmd.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                    }

                    cnn.Open();
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    return dt;
                }
            }
        }

        public DataTable ExecSQL(string sSQL, string sCnnName)
        {
            string sCnn = ConfigurationManager.ConnectionStrings[sCnnName].ConnectionString;

            using (SqlConnection cnn = new SqlConnection(sCnn))
            {
                using (SqlCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandTimeout = iTimeout;
                    cmd.CommandText = sSQL;
                    cmd.CommandType = CommandType.Text;

                    cnn.Open();
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    return dt;
                }
            }
        }

    }
}
