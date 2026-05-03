using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text;

namespace GFC.BlazorServer.Services;

public class SmtpEmailService(
    IOptionsMonitor<EmailSettings> emailSettings,
    IAuditLogger auditLogger,
    ILogger<SmtpEmailService> logger) : IEmailService
{
    private EmailSettings _settings => emailSettings.CurrentValue;

    public async Task<EmailResult> SendEmailAsync(string recipientEmail, string subject, string body, Dictionary<string, byte[]>? attachments = null, string? ccEmail = null)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
            message.To.Add(new MailboxAddress("", recipientEmail));
            
            if (!string.IsNullOrWhiteSpace(ccEmail))
            {
                foreach (var address in ccEmail.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    if (address.Contains("@"))
                    {
                        message.Cc.Add(new MailboxAddress("", address));
                    }
                }
            }

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    bodyBuilder.Attachments.Add(attachment.Key, attachment.Value);
                }
            }

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            
            // Check if host and port are set
            if (string.IsNullOrEmpty(_settings.SmtpHost))
            {
                return EmailResult.Failure("SMTP Host is not configured.");
            }

            await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, _settings.SmtpEnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
            
            if (!string.IsNullOrEmpty(_settings.SmtpUsername))
            {
                await client.AuthenticateAsync(_settings.SmtpUsername, _settings.SmtpPassword);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            
            logger.LogInformation("Email sent successfully via SMTP to {Recipient}", recipientEmail);
            return EmailResult.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SMTP ERROR] {DateTime.Now}: {ex}");
            logger.LogError(ex, "Failed to send email via SMTP to {Recipient}", recipientEmail);
            auditLogger.Log("SMTP Email Failure", null, null, $"Recipient: {recipientEmail} | Error: {ex.Message}");
            return EmailResult.Failure(ex.Message);
        }
    }
}
