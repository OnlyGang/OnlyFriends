using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace OnlyFriends.Models
{
    public class EmailSender
    {
        private const string senderEmail = "lasmclasabaltag@gmail.com";
        private const string senderPassword = "ceimaibuni";
        private const string smtpServer = "smtp.gmail.com";
        private const int smtpPort = 587;
        public void SendEmailNotification(string toEmail, string subject, string content)
        {
            
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword);

            MailMessage email = new MailMessage(toEmail, toEmail, subject, content);

            email.IsBodyHtml = true;
            email.BodyEncoding = UTF8Encoding.UTF8;
            try
            {
                System.Diagnostics.Debug.WriteLine("Sending Email...");
                smtpClient.Send(email);
                System.Diagnostics.Debug.WriteLine("Email sent.");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
    }
}