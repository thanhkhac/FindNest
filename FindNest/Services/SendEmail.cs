using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace FindNest.Services
{
    public class EmailSettings
    {
        public string? FromEmail { get; set; }
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string? AppPassword { get; set; } 

    }

    public class SendEmail : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public SendEmail(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var appPassword = _emailSettings.AppPassword;

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true 
            };
            mail.To.Add(email);
            SmtpClient smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);

            smtpClient.Credentials = new NetworkCredential(_emailSettings.FromEmail, appPassword);
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mail);
        }
    }
}