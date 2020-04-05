using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Md5Helper
    {

        public static string GetStringMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            // 将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder str = new StringBuilder();
            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                str.Append(data[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位
            }
            // 返回十六进制字符串  
            return str.ToString();
        }


        static public string GetFileMd5(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = md5.ComputeHash(file);
            string str = System.BitConverter.ToString(hash_byte);
            str = str.Replace("-", "");
            return str.ToLower();
        }




        //public static string GetFileMd5(string filePath)
        //{
        //    byte[] bytes;
        //    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        //    {
        //        MD5 md5 = new MD5CryptoServiceProvider();

        //        bytes = md5.ComputeHash(fileStream);
        //    }

        //    StringBuilder stringBuilder = new StringBuilder();

        //    foreach (var b in bytes)
        //    {
        //        stringBuilder.Append(b.ToString("x2"));
        //    }

        //    return stringBuilder.ToString();
        //}




    }
}
