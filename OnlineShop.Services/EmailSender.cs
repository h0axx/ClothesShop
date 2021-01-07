using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace OnlineShop.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConifg;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConifg = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        private void Send(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConifg.SmtpServer, _emailConifg.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConifg.UserName, _emailConifg.Password);

                    client.Send(emailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailConifg.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) 
            { Text = string.Format("<a href={0}>Activate account</a>", message.Content) };

            return emailMessage;
        }
    }
}
