using System.Net;
using System.Net.Mail;
using GFC.Core.Interfaces;
using Microsoft.Extensions.Logging;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Services;

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

    public async Task SendEmailAsync(string recipientEmail, string subject, string body, Dictionary<string, byte[]>? attachments = null)
    {
        var settings = await _settingsService.GetAsync();

        if (!settings.EmailEnabled)
        {
            _logger.LogInformation("Email sending skipped: Globally disabled in settings.");
            return;
        }

        if (string.IsNullOrEmpty(settings.SmtpHost) || settings.SmtpPort <= 0)
        {
            _logger.LogWarning("Email sending failed: SMTP Host or Port not configured.");
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
                From = new MailAddress(settings.SmtpFromAddress ?? "noreply@liquorhub.com", settings.SmtpFromName ?? "Liquor Hub"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(recipientEmail);

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachment.Value), attachment.Key));
                }
            }

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation($"Email sent successfully to {recipientEmail}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send email to {recipientEmail}");
            throw;
        }
    }
}
