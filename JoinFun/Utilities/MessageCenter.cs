using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace JoinFun.Utilities
{
    public class MessageCenter
    {
        public void SendEmail(List<string> mailList, string subject, string content)
        {
            MailMessage msg = new MailMessage();
            //msg.To.Add(string.Join(",", mailList.ToList()));
            //加入密件副本
            msg.Bcc.Add(string.Join(",", mailList.ToList()));
            msg.From = new MailAddress("joinfun2019@gmail.com", "JoinFun", System.Text.Encoding.UTF8);
            msg.Subject = subject;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;

            msg.Body = content;
            msg.IsBodyHtml = true;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.Priority = MailPriority.Normal;
            //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
            #region 其它 Host
            /*
             *  outlook.com smtp.live.com port:25
             *  yahoo smtp.mail.yahoo.com.tw port:465
            */
            #endregion
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            //設定帳號密碼
            client.Credentials = new System.Net.NetworkCredential("joinfun2019@gmail.com", "JOINFUN12345678");//括號內輸入自己的Gmail信箱帳號與密碼
            //Gmail SSL
            client.EnableSsl = true;
            client.Send(msg);
        }
    }
}