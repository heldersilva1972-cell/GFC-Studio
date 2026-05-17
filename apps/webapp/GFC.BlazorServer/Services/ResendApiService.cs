using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resend;
using System.Text;

namespace GFC.BlazorServer.Services;

public class ResendApiService(
    IOptionsMonitor<EmailSettings> emailSettings,
    IAuditLogger auditLogger,
    ILogger<ResendApiService> logger,
    IResend resend) : IEmailService
{
    private EmailSettings _settings => emailSettings.CurrentValue;

    public async Task<EmailResult> SendEmailAsync(string recipientEmail, string subject, string body, Dictionary<string, byte[]>? attachments = null, string? ccEmail = null)
    {
        try
        {
            // 1. Validation Checks
            if (string.IsNullOrWhiteSpace(_settings.ResendApiKey) || !_settings.ResendApiKey.StartsWith("re_"))
            {
                return EmailResult.Failure("Resend API Key is missing or invalid. It must start with 're_'.");
            }

            if (string.IsNullOrWhiteSpace(_settings.FromAddress))
            {
                return EmailResult.Failure("Sender 'From' address is missing.");
            }

            if (!_settings.FromAddress.Contains("@"))
            {
                return EmailResult.Failure($"Invalid From address: '{_settings.FromAddress}'");
            }

            var message = new EmailMessage();
            
            // Ensure collections are initialized (Resend library uses custom EmailAddressList type)
            if (message.To == null) message.To = new EmailAddressList();
            if (message.Cc == null) message.Cc = new EmailAddressList();
            
            // Attachments is a standard List<EmailAttachment> in this library
            if (message.Attachments == null) message.Attachments = new List<EmailAttachment>();

            message.From = string.IsNullOrWhiteSpace(_settings.FromName) 
                ? _settings.FromAddress 
                : $"\"{_settings.FromName}\" <{_settings.FromAddress}>";
            
            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                return EmailResult.Failure("Recipient email is empty.");
            }
            message.To.Add(recipientEmail);
            
            if (!string.IsNullOrWhiteSpace(ccEmail))
            {
                foreach (var address in ccEmail.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    if (address.Contains("@"))
                    {
                        message.Cc.Add(address);
                    }
                }
            }

            message.Subject = subject;
            message.HtmlBody = body;

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(new EmailAttachment
                    {
                        Filename = attachment.Key,
                        Content = attachment.Value
                    });
                }
            }

            await resend.EmailSendAsync(message);
            logger.LogInformation("Email sent successfully via Resend to {Recipient}", recipientEmail);
            return EmailResult.Ok();
        }
        catch (Exception ex)
        {
            // Debug Output: Log full stack trace to VS Output window
            Console.WriteLine($"[RESEND ERROR] {DateTime.Now}: {ex}");
            
            logger.LogError(ex, "Failed to send email via Resend to {Recipient}", recipientEmail);
            auditLogger.Log("Resend Email Failure", null, null, $"Recipient: {recipientEmail} | Error: {ex.Message}");
            
            return EmailResult.Failure($"Resend Error: {ex.Message}");
        }
    }
}
