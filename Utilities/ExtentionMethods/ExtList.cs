using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class ExtList
    {
        public static string GenSQLCommaStr<T>(this List<T> lt)
              => "'" + string.Join("','", lt) + "'";

        public static DataTable ConvertToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dt.Columns.Add(property.Name, property.PropertyType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }

        public static List<List<T>> DivideListIntoLists<T>(this List<T> inputList, int subListLength) where T : IComparable<T>
        {
            var ret = new List<List<T>>();
            while (inputList.Count > subListLength)
            {
                ret.Add(inputList.Take(subListLength).ToList());
                inputList.RemoveRange(0, subListLength);
            }
            if (inputList.Count < subListLength)
            {
                ret.Add(inputList);
            }
            return ret;
        }





    }
}
