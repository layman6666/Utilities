using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace TestUtilities
{
    class Program
    {
        static void Main(string[] args)
        {

            #region test EmailHelper
            //string to = "chauncey.wang@finisar.com";
            //string title = "test";
            //string body = "Just a test";
            //if (EmailHelper.SendEmail(to, title, body))
            //    Console.WriteLine("send mail successfully");




            #endregion

            #region test SqlHelper

            //string sql = "select * from [MyQQ].[dbo].[Users]";
            //var dt = SqlHelper.GetDataTable(sql);





            #endregion

            #region test ExcelHelper

            //string sql = "select * from [MyQQ].[dbo].[Users]";
            //var dt = SqlHelper.GetDataTable(sql);
            //ExcelHelper.DatatableToExcel(dt, "testc.xlsx");
            //ExcelHelper.DatatableToExcel(dt, "txst.xls");


            //var excel = ExcelHelper.ExcelToDatatable("testc.xlsx");

            #endregion


            #region test Md5

            //string a=  Md5Helper.GetFileMd5("testc.xlsx");
            //string b = Md5Helper.GetStringMd5("chauncey.wang");
           


            #endregion



        }
    }
}
