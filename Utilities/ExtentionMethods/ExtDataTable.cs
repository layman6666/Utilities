using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class ExtDataTable
    {
        public static string ReturnFirstColumnFirstRow(this DataTable dt)
        {
            DataRow[] rows = dt.Select();
            if (rows.Count() == 0)
            {
                return null;
            }
            else
            {
                return rows[0][0].ToString();
            }
        }

        public static List<string> ReturnFirstColumn(this DataTable dt)
        {
            var retList = new List<string>();
            DataRow[] rows = dt.Select();
            if (rows.Count() == 0)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < rows.Count(); i++)
                {
                    retList.Add(rows[i][0].ToString());
                }
            }
            return retList;
        }

        public static List<string> ReturnFirstRow(this DataTable dt)
        {
            DataRow[] rows = dt.Select();
            if (rows.Count() == 0)
            {
                return null;
            }
            else
            {
                var retResult = new List<string>();
                for (int item = 0; item < rows[0].ItemArray.Length; item++)
                {
                    retResult.Add(rows[0][item].ToString());
                }
                return retResult;
            }
        }

        public static List<string> ReturnAllRows(this DataTable dt, int endColumnIndex)
        {
            DataRow[] rows = dt.Select();
            if (rows.Count() == 0)
            {
                return null;
            }
            else
            {
                var retResult = new List<string>();
                for (int i = 0; i < rows.Count(); i++)
                {
                    for (int j = 0; j < endColumnIndex; j++)
                    {
                        retResult.Add(rows[i][j].ToString());
                    }
                }
                return retResult;
            }
        }

        public static List<string> ReturnAssignColumn(this DataTable dt, string assignColumnName)
        {
            var retList = new List<string>();
            DataRow[] rows = dt.Select();
            if (rows.Count() == 0)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < rows.Count(); i++)
                {
                    retList.Add(rows[i][assignColumnName].ToString());
                }
            }
            return retList;
        }

        public static Dictionary<string, string> ReturnTwoColumnsToDictionary(this DataTable dt, int keyColumnIndex, int valueColumnIndex)
        {
            var retDic = new Dictionary<string, string>();
            foreach (DataRow rowItem in dt.Rows)
            {
                retDic[rowItem[keyColumnIndex].ToString()] = rowItem[valueColumnIndex].ToString();
            }
            return retDic;
        }

        public static Dictionary<string, string> ReturnTwoColumnsToDictionary(this DataTable dt, string keyColumnName, string valueColumnName)
        {
            var retDic = new Dictionary<string, string>();
            foreach (DataRow rowItem in dt.Rows)
            {
                retDic[rowItem[keyColumnName].ToString()] = rowItem[valueColumnName].ToString();
            }
            return retDic;
        }

        public static Dictionary<string, string> ReturnFirstRowToDictionary(this DataTable dt)
        {
            var retDic = new Dictionary<string, string>();
            DataRow[] rows = dt.Select();
            if (rows.Count() == 0)
            {
                return null;
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                retDic.Add(dt.Columns[i].ColumnName, rows[0][i].ToString());
            }
            return retDic;
        }

        public static DataTable Transposition(this DataTable dt)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("ColumnName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dtNew.Columns.Add("Value" + (i + 1).ToString(), typeof(string));
            }
            foreach (DataColumn dc in dt.Columns)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["ColumnName"] = dc.ColumnName;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    drNew[i + 1] = dt.Rows[i][dc].ToString();
                }
                dtNew.Rows.Add(drNew);
            }
            return dtNew;
        }


    }
}
