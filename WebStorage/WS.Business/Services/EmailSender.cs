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
    public class EmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var messageMime = new MimeMessage();
            messageMime.From.Add(new MailboxAddress("WebStorage", "webstorage12@gmail.com"));
            messageMime.To.Add(new MailboxAddress("", email));
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
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("webstorage12@gmail.com", "webStorage12345");
                await client.SendAsync(messageMime);
                await client.DisconnectAsync(true);
            }
        }
    }
}
