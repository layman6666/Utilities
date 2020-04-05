using CDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class EmailHelper
    {

        public static bool SendEmail(string from, string to, string title, string body, string password, string server, int port, bool ssl, string attachment)
        {
            bool flag = false;
            try
            {
                #region
                Message oMsg = new Message();

                var conf = oMsg.Configuration;
                var oFields = conf.Fields;

                oFields["http://schemas.microsoft.com/cdo/configuration/sendusing"].Value = CdoSendUsing.cdoSendUsingPort;
                oFields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"].Value = CdoProtocolsAuthentication.cdoAnonymous;//CdoProtocolsAuthentication.cdoBasic;
                if (ssl)
                {
                    oFields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"].Value = true;
                }
                oFields["http://schemas.microsoft.com/cdo/configuration/smtpserver"].Value = server;//必填，而且要真实可用   
                oFields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"].Value = port;//邮箱端口
                oFields["http://schemas.microsoft.com/cdo/configuration/sendemailaddress"].Value = from;//发送者邮箱
                oFields["http://schemas.microsoft.com/cdo/configuration/sendusername"].Value = from;//邮箱发送者名称   
                oFields["http://schemas.microsoft.com/cdo/configuration/sendpassword"].Value = password;   //邮箱发送者密码，必须真实   
                oFields.Update();

                oMsg.Configuration = conf;
                oMsg.Subject = title;//主题
                oMsg.HTMLBody = body;//邮件正文                      
                oMsg.From = from;//发送者
                oMsg.To = to;//接收者  多个人分号隔开  "a@123.com;b@456.com"
                oMsg.Send();//发送
                oMsg.AddAttachment(attachment);
                flag = true;
                #endregion
            }
            catch (Exception)
            {
                flag = false;
                throw new Exception();
            }
            return flag;
        }

        public static bool SendEmail(string to, string title, string body)
        {
            bool flag = false;


            string from = "test@finisar.com";
            string password = "";
            string server = "smtp.finisar.com";
            int port = 25;
            bool ssl = false;

            try
            {
                #region
                Message oMsg = new Message();
                var conf = oMsg.Configuration;
                var oFields = conf.Fields;

                oFields["http://schemas.microsoft.com/cdo/configuration/sendusing"].Value = CdoSendUsing.cdoSendUsingPort;
                oFields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"].Value = CdoProtocolsAuthentication.cdoAnonymous;//CdoProtocolsAuthentication.cdoBasic;
                if (ssl)
                {
                    oFields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"].Value = true;
                }
                oFields["http://schemas.microsoft.com/cdo/configuration/smtpserver"].Value = server;//必填，而且要真实可用   
                oFields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"].Value = port;//邮箱端口
                oFields["http://schemas.microsoft.com/cdo/configuration/sendemailaddress"].Value = from;//发送者邮箱 
                oFields["http://schemas.microsoft.com/cdo/configuration/sendusername"].Value = from;//邮箱发送者名称   
                oFields["http://schemas.microsoft.com/cdo/configuration/sendpassword"].Value = password;   //邮箱发送者密码，必须真实   
                oFields.Update();

                oMsg.Configuration = conf;
                oMsg.Subject = title;//主题
                oMsg.HTMLBody = body;//邮件正文                      
                oMsg.From = from;//发送者
                oMsg.To = to;//接收者
                oMsg.Send();//发送
                flag = true;
                #endregion
            }
            catch (Exception)
            {
                flag = false;
                throw new Exception();
            }
            return flag;
        }



    }
}
