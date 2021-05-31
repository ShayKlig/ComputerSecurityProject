using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ComputerSecurityProject.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }

    public class SendGridMailService
    {
        public static void SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = "shayush11@gmail.com";
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var msg = new MailMessage(from, toEmail, subject, content);
            //var response = await client.SendEmailAsync(msg);
            var smtpClient = new SmtpClient("smtp.sendgrid.net", 465);
            var creds = new NetworkCredential
            {
                UserName = "apikey",
                Password = apiKey
            };
            smtpClient.Credentials = creds;
            smtpClient.Send(msg);
        }
    }
}