using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Interfaces;

namespace WS.Business.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var messageMime = new MimeMessage();
            messageMime.From.Add(new MailboxAddress("WebStorage", "webstorage12@gmail.com"));
            messageMime.To.Add(new MailboxAddress("naren", email));
            messageMime.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            messageMime.Body = bodyBuilder.ToMessageBody();
            //messageMime.Body = new TextPart("plain")
            //{
            //    Text = message
            //};
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("webstorage12@gmail.com", "webStorage12345");
                client.Send(messageMime);
                client.Disconnect(true);
            }


            return Task.CompletedTask;
        }
    }
}
