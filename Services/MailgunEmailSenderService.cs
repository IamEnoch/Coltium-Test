using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Coltium_Test.Services;

public class MailGunEmailSenderService(
    IHttpClientFactory httpClientFactory,
    IConfiguration config,
    ILogger<MailGunEmailSenderService> logger)
    : IEmailSender
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Mailgun");
    private readonly ILogger _logger = logger;

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(config["Mailgun:Domain"])) throw new Exception("Mailgun domain is not configured.");

        using var form = new MultipartFormDataContent();

        SetFormParam("from", $"Your Name <postmaster@{config["Mailgun:Domain"]}>");
        SetFormParam("to", toEmail);
        SetFormParam("subject", subject);
        SetFormParam("text", message);
        SetFormParam("html", $"<html><body><p>{message}</p></body></html>");

        var result = await _httpClient.PostAsync(string.Empty, form);

        if (result.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Email to {toEmail} queued successfully!");
        }


        else
        {
            var errorContent = await result.Content.ReadAsStringAsync();
            _logger.LogError($"Failed to send email to {toEmail}: {errorContent}");
        }

        return;

        // Function to set form parameters for Mailgun
        void SetFormParam(string key, string value)
        {
            form.Add(new StringContent(value, Encoding.UTF8, "text/plain"), key);
        }
    }
}