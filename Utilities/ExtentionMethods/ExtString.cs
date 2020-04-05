using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
   public static class ExtString
    {
        public static string ReturnFormatString(this String str, List<string> parameters)
        {
            return string.Format(str, parameters);
        }

        public static string ReturnFormatString(this String str, string parameter)
        {
            return string.Format(str, parameter);
        }

        public static string ReturnFormatString(this String str, string parameter1, string parameter2)
        {
            return string.Format(str, parameter1, parameter2);
        }

        public static string ReturnFormatString(this String str, string parameter1, string parameter2, string parameter3)
        {
            {
                return string.Format(str, parameter1, parameter2, parameter3);
            }
        }

        public static List<List<T>> DivideListIntoLists<T>(this List<T> inputList, int divideLength)
        {
            var ret = new List<List<T>>(inputList.Count / divideLength + 1);
            while (inputList.Count > divideLength)
            {
                ret.Add(inputList.Skip(inputList.Count - divideLength).ToList());
                inputList.RemoveRange(inputList.Count - divideLength, divideLength);
            }
            ret.Add(inputList);
            return ret;
        }





    }
}
