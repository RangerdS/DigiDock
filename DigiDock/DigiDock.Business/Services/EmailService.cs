using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace DigiDock.Business.Services
{
    public class EmailService
    {
        private readonly IConfiguration configuration;
        private readonly LogQueueService logQueueService;

        public EmailService(IConfiguration configuration, LogQueueService logQueueService)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.logQueueService = logQueueService ?? throw new ArgumentNullException(nameof(logQueueService));
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailSettings = configuration.GetSection("EmailSettings");
            var smtpSettings = GetSmtpSettings(toEmail, emailSettings);

            var smtpClient = new SmtpClient(smtpSettings["SmtpServer"])
            {
                Port = int.Parse(smtpSettings["SmtpPort"]),
                Credentials = new NetworkCredential(smtpSettings["SmtpUser"], smtpSettings["SmtpPass"]),
                EnableSsl = true,
            };
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["FromEmail"], smtpSettings["FromName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
            logQueueService.EnqueueLog("Information", $"Email sent to {toEmail} with subject: {subject}");
        }

        private IConfigurationSection GetSmtpSettings(string toEmail, IConfigurationSection emailSettings)
        {
            if (toEmail.EndsWith("@sinansaglam.com.tr", StringComparison.OrdinalIgnoreCase))
            {
                return emailSettings.GetSection("Yandex");
            }
            else  //(toEmail.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                return emailSettings.GetSection("Gmail");
            }
        }
    }
}