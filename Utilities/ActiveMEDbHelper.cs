using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{

    public enum DB
    {
        E_Data = 1,
        Engineering = 3,
        Automation = 6,
        CML = 10,
        CamstarSHG_Read = 11,
        ITSummaryTable = 26,
        WuxParallel = 32,
        CamstarMESRead = 41,
        MESUAT = 42,
        CamstarMESDLL = 50,
        AD = 123,
        MESReport = 124,
        OSA = 125,
        WebFinance = 37,
        Traning = 126
    }

   public class ActiveMEDbHelper
    {

        private SqlConnection CONN;
        Dictionary<string, string> dbKey = new Dictionary<string, string>();
        public bool IsOpened { get { return CONN.State == ConnectionState.Open; } }


        public ActiveMEDbHelper(DB dbtype)
        {
            var sqlcs = new SqlConnectionStringBuilder();
            var connStr = "server=WUX-PARALLEL;Connect Timeout=15;Min Pool Size=10;Max Pool Size=50;database=WebActiveME;User Id=parallel;Pwd=P@rt1st";
            var conn = new SqlConnection(connStr);
            conn.Open();
            CONN = conn;
            dbKey.Add("id", ((int)dbtype).ToString());
            var row = ExecSPReturnTable("QueryDBInfo", dbKey).ReturnFirstRow();
            conn.Dispose();
            if (row != null)
            {
                sqlcs["uid"] = row[0].ToString();
                sqlcs["pwd"] = row[1].ToString();
                sqlcs["server"] = row[2].ToString();
                sqlcs["database"] = row[3].ToString();
                sqlcs.MinPoolSize = 10;
                sqlcs.MinPoolSize = 30;
                conn = new SqlConnection(sqlcs.ConnectionString);
                conn.Open();
                CONN = conn;
            }
        }

        public void ExecuteNonQuery(string sql)
        {
            var cmd = CONN.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandTimeout = 0;
            cmd.ExecuteNonQuery();
        }
        public void ExecuteNonQuery(string sql, params SqlParameter[] values)
        {
            var cmd = CONN.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddRange(values);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        public int BulkCopy(string DestinationTableName, DataTable dt)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(CONN))
            {
                bulkCopy.DestinationTableName = DestinationTableName;
                try
                {
                    bulkCopy.WriteToServer(dt);
                    return 1;
                }
                catch (Exception ee)
                {
                    return 0;
                }

            }
        }

        public int BulkColumnsCopy(string DestinationTableName, DataTable dt)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(CONN))
            {
                bulkCopy.DestinationTableName = DestinationTableName;
                try
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                    }
                    bulkCopy.WriteToServer(dt);
                    return 1;
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }

        public int BulkCopy(DataTable dt, string DestinationTableName)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(CONN))
            {
                bulkCopy.DestinationTableName = DestinationTableName;
                try
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        bulkCopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                    }

                    bulkCopy.WriteToServer(dt);
                    return 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }

            }
        }

        public DataSet ExeCuteQueryReturnDataSet(string sql)
        {
            var dt = new DataSet();
            var myAd = new SqlDataAdapter(sql, CONN);
            myAd.SelectCommand.CommandTimeout = 0;
            myAd.Fill(dt);
            return dt;
        }

        public void Disconnect()
        {
            CONN.Close();
        }

        //store procedure operation

        public DataTable ExecSPReturnTable(string spname, Dictionary<string, string> ParaPair)
        {
            SqlCommand cmd = CONN.CreateCommand();
            if (CONN.State != ConnectionState.Open)
                CONN.Open();
            cmd.Connection = CONN;
            cmd.CommandText = spname;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var para in ParaPair)
            {
                cmd.Parameters.Add(new SqlParameter(para.Key, para.Value));

            }
            var dt = new DataTable();
            SqlDataAdapter myAd = new SqlDataAdapter(cmd);
            myAd.SelectCommand.CommandTimeout = 0;
            myAd.Fill(dt);
            return dt;
        }

        public DataTable ExecSPReturnTable(string spname)
        {
            SqlCommand cmd = CONN.CreateCommand();
            if (CONN.State != ConnectionState.Open)
                CONN.Open();
            cmd.Connection = CONN;
            cmd.CommandText = spname;
            cmd.CommandType = CommandType.StoredProcedure;
            var dt = new DataTable();
            SqlDataAdapter myAd = new SqlDataAdapter(cmd);
            myAd.SelectCommand.CommandTimeout = 0;
            myAd.Fill(dt);
            return dt;
        }

        public DataTable ExecuteQueryReturnTable(string sql)
        {
            var dt = new DataTable();
            SqlDataAdapter myAd = new SqlDataAdapter(sql, CONN);
            myAd.SelectCommand.CommandTimeout = 0;
            myAd.Fill(dt);
            return dt;
        }

        public DataTable ExecuteQueryReturnTable(string sql, params SqlParameter[] values)
        {
            var dt = new DataTable();
            SqlDataAdapter myAd = new SqlDataAdapter(sql, CONN);
            myAd.SelectCommand.Parameters.AddRange(values);
            myAd.SelectCommand.CommandTimeout = 0;
            myAd.Fill(dt);
            myAd.SelectCommand.Parameters.Clear();
            return dt;
        }

        public DataTable ExecuteSQLFile(string sqlFilePath)
        {
            List<string> sqlFile = this.GetSqlFile(sqlFilePath).Where(x => x.Trim() != string.Empty).ToList();
            DataTable dataTable = new DataTable();
            for (int i = 0; i < sqlFile.Count - 1; i++)
            {
                this.ExecuteNonQuery(sqlFile[i].ToString());
            }
            return this.ExecuteQueryReturnTable(sqlFile[sqlFile.Count - 1].ToString());
        }

        private List<string> GetSqlFile(string strFileName)
        {
            List<string> list = new List<string>();
            bool flag = !File.Exists(strFileName);
            List<string> result;
            if (flag)
            {
                result = list;
            }
            else
            {
                StreamReader streamReader = new StreamReader(strFileName, Encoding.Default);
                string text = "";
                while (streamReader.Peek() > -1)
                {
                    string text2 = streamReader.ReadLine();
                    bool flag2 = text2 == "";
                    if (!flag2)
                    {
                        bool flag3 = !(text2.Trim().ToUpper() == "GO");
                        if (flag3)
                        {
                            text += text2;
                            text += "\r\n";
                        }
                        else
                        {
                            list.Add(text);
                            text = "";
                        }
                    }
                }
                streamReader.Close();
                result = list;
            }
            return result;
        }

        #region 注销2
        ///// <summary>
        ///// Return a row as a dictionary
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        ///// 
        //public Dictionary<string, string> ExecuteQueryReturnDic(string sql)
        //{
        //    var ret = new Dictionary<string, string>();
        //    var dt = ExecuteQueryReturnTable(sql);
        //    DataRow[] rows = dt.Select();
        //    if (rows.Length == 1)
        //    {
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            ret.Add(dt.Columns[i].ColumnName, rows[0][i].ToString());
        //        }
        //        ret.Add("MSSQL_QUERY_DIC_RESULT", "TRUE");
        //    }
        //    else
        //    {
        //        ret.Add("MSSQL_QUERY_DIC_RESULT", "FALSE");
        //    }
        //    return ret;
        //}

        //public Dictionary<string, string> ExecuteQueryReturnDicMany(string sql)
        //{
        //    var ret = new Dictionary<string, string>();
        //    var dt = ExecuteQueryReturnTable(sql);
        //    DataRow[] rows = dt.Select();
        //    for (int i = 0; i < rows.Count(); i++)
        //    {
        //        ret.Add(rows[i][0].ToString(), rows[i][1].ToString());
        //    }
        //    return ret;
        //}
        ///// <summary>
        ///// Return a row as a list
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        //public List<string> ExecuteQueryReturnOneRowToList(string sql)
        //{
        //    var ret = new List<string>();
        //    var dt = ExecuteQueryReturnTable(sql);
        //    DataRow[] rows = dt.Select();
        //    if (rows.Length == 1)
        //    {
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            ret.Add(rows[0][i].ToString());
        //        }
        //    }
        //    return ret;
        //}
        #endregion
    }





}

