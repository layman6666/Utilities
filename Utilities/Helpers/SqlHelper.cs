using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class SqlHelper
    {

        private static readonly string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public static DataTable GetDataTable(string sql, CommandType type=CommandType.Text, params SqlParameter[] parms)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    if (parms != null)
                    {
                        adapter.SelectCommand.Parameters.AddRange(parms);
                    }
                    adapter.SelectCommand.CommandType = type;//CommandType.Text
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }

            }
        }


        public static int ExecuteNonquery(string sql, CommandType type=CommandType.Text, params SqlParameter[] parms)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parms != null)
                    {
                        cmd.Parameters.AddRange(parms);
                    }
                    cmd.CommandType = type;
                    conn.Open();
                    return cmd.ExecuteNonQuery();

                }

            }
        }



        public static object ExecuteScalar(string sql, CommandType type=CommandType.Text, params SqlParameter[] parms)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parms != null)
                    {
                        cmd.Parameters.AddRange(parms);
                    }
                    cmd.CommandType = type;
                    conn.Open();
                    return cmd.ExecuteScalar();
                }

            }
        }




    }
}
