using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Coltium_Test.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSenderService> _logger;

        public EmailSenderService(IConfiguration config, ILogger<EmailSenderService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpAuthUsername = Environment.GetEnvironmentVariable("AZURE_COMMUNICATION_USERNAME");
            var smtpAuthPassword = Environment.GetEnvironmentVariable("AZURE_COMMUNICATION_PASSWORD");
            var sender = Environment.GetEnvironmentVariable("AZURE_COMMUNICATION_SENDER");
            var smtpHostUrl = "smtp.azurecomm.net";

            if (string.IsNullOrEmpty(smtpAuthUsername) || string.IsNullOrEmpty(smtpAuthPassword) || string.IsNullOrEmpty(sender))
            {
                throw new Exception("SMTP credentials are not configured properly.");
            }

            var client = new SmtpClient(smtpHostUrl)
            {
                Port = 587,
                Credentials = new NetworkCredential(smtpAuthUsername, smtpAuthPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(sender, toEmail, subject, message);

            try
            {
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"The email to {toEmail} was successfully sent using Smtp.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Smtp send failed with the exception: {ex.Message}.");
            }
        }
    }
}