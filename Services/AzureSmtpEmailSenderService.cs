using Azure;
using Azure.Communication.Email;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Coltium_Test.Services;

public class AzureSmtpEmailSenderService(
    IConfiguration config,
    ILogger<AzureSmtpEmailSenderService> logger,
    IWebHostEnvironment env)
    : IEmailSender
{
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var connectionString = config["Commservices:ConnectionString"];
        var sender = config["SenderEmail"];

        if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(sender))
        {
            logger.LogError("Azure Communication Services connection string or sender is not configured properly.");
            throw new Exception("Azure Communication Services connection string or sender is not configured properly.");
        }

        var emailClient = new EmailClient(connectionString);

        var emailMessage = new EmailMessage(
            sender,
            content: new EmailContent(subject)
            {
                PlainText = message,
                Html = $"<html><body><p>{message}</p></body></html>"
            },
            recipients: new EmailRecipients(new List<EmailAddress> { new(toEmail) })
        );

        try
        {
            var emailSendOperation = await emailClient.SendAsync(WaitUntil.Completed, emailMessage);
            logger.LogInformation($"The email to {toEmail} was successfully sent using Azure Communication Services.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Azure Communication Services email send failed with the exception: {ex.Message}.");
        }
    }
}