using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Models
{
    public class SendEmailModel
    {
        public string Send(string ToEmail, string Token)
        {
            string FromEmail = "kshreyask1@gmail.com";
            MailMessage mailMessage = new MailMessage(FromEmail, ToEmail);
            string MailBody = "The token for reset is : " + Token;
            mailMessage.Subject = "Token for forget Password";
            mailMessage.Body = MailBody;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587);
            NetworkCredential networkCredential = new NetworkCredential("kshreyask1@gmail.com", "rzim dtgr oheh xozy");

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredential;
            smtpClient.Send(mailMessage);
            return ToEmail;
        }
    }
}
