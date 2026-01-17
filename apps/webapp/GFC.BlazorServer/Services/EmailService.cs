using System.Net;
using System.Net.Mail;
using GFC.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

public class EmailService : IEmailService
{
    private readonly IBlazorSystemSettingsService _settingsService;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IBlazorSystemSettingsService settingsService,
        ILogger<EmailService> logger)
    {
        _settingsService = settingsService;
        _logger = logger;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        var settings = await _settingsService.GetAsync();

        if (!settings.EmailEnabled)
        {
            _logger.LogInformation("Email sending skipped: Globally disabled in settings.");
            return;
        }

        if (string.IsNullOrEmpty(settings.SmtpHost) || string.IsNullOrEmpty(settings.SmtpFromAddress))
        {
            _logger.LogWarning("Email sending failed: SMTP Host or From Address not configured.");
            return;
        }

        try
        {
            using var client = new SmtpClient(settings.SmtpHost, settings.SmtpPort)
            {
                EnableSsl = settings.SmtpEnableSsl,
                Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(settings.SmtpFromAddress, settings.SmtpFromName ?? "GFC System"),
                Subject = subject,
                Body = body,
                IsBodyHtml = body.Contains("<") // Simple heuristic for HTML detection
            };

            mailMessage.To.Add(recipientEmail);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation("Email successfully sent to {Recipient}", recipientEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient} via {Host}", recipientEmail, settings.SmtpHost);
            // We don't necessarily want to crash the whole app if an email fails, 
            // but we want to log it clearly.
        }
    }
}
