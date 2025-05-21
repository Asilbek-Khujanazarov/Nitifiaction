using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.Configuration;
using System.Net.Mail;
using System.Net;

namespace PatientRecovery.NotificationService.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(Notification notification)
        {
            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = notification.Title,
                Body = notification.Message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(notification.RecipientEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}