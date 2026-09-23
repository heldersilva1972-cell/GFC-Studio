using GFC.Core.Interfaces;
using GFC.Core.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using Resend;
using System;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class RentalEmailDispatcher : IRentalEmailDispatcher
    {
        private readonly ILogger<RentalEmailDispatcher> _logger;

        public RentalEmailDispatcher(ILogger<RentalEmailDispatcher> logger)
        {
            _logger = logger;
        }

        public async Task<EmailResult> SendRentalEmailAsync(
            WebsiteSettings settings,
            string recipientEmail,
            string subject,
            string htmlBody,
            string? ccEmail = null)
        {
            if (settings == null)
            {
                return EmailResult.Failure("WebsiteSettings not available.");
            }

            if (settings.MasterEmailKillSwitch)
            {
                _logger.LogWarning("Email delivery suppressed by MasterEmailKillSwitch.");
                return EmailResult.Failure("Email sending is currently disabled by Master Email Kill Switch.");
            }

            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                return EmailResult.Failure("Recipient email is empty.");
            }

            var provider = settings.RentalEmailProvider ?? "SMTP";
            if (provider.Equals("Resend", StringComparison.OrdinalIgnoreCase))
            {
                return await SendViaResendAsync(settings, recipientEmail, subject, htmlBody, ccEmail);
            }

            return await SendViaSmtpAsync(settings, recipientEmail, subject, htmlBody, ccEmail);
        }

        public async Task<EmailResult> TestRentalEmailConnectionAsync(
            WebsiteSettings settings,
            string testRecipientEmail)
        {
            if (string.IsNullOrWhiteSpace(testRecipientEmail))
            {
                return EmailResult.Failure("Please provide a valid recipient email address to send the test message.");
            }

            string subject = $"[Test Connection] Gloucester Fraternity Club - Rental Mailer Test";
            string body = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; background: #f8fafc; border-radius: 8px;'>
                    <h3 style='color: #0d1b2a;'>Gloucester Fraternity Club - Hall Rental Email Engine</h3>
                    <p>This is a live test email verifying that your dedicated Hall Rental email settings are functioning properly.</p>
                    <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 15px 0;'/>
                    <p><strong>Configured Sender:</strong> {settings.RentalSenderName} &lt;{settings.RentalSenderEmail}&gt;</p>
                    <p><strong>Engine:</strong> {settings.RentalEmailProvider}</p>
                    <p><strong>Timestamp:</strong> {DateTime.Now:MMMM dd, yyyy h:mm tt}</p>
                </div>";

            return await SendRentalEmailAsync(settings, testRecipientEmail, subject, body);
        }

        private async Task<EmailResult> SendViaSmtpAsync(
            WebsiteSettings settings,
            string recipientEmail,
            string subject,
            string htmlBody,
            string? ccEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(settings.RentalSmtpHost))
                {
                    return EmailResult.Failure("SMTP Host is not configured in Hall Rental Settings.");
                }

                var message = new MimeMessage();
                var fromName = string.IsNullOrWhiteSpace(settings.RentalSenderName) ? "GFC Hall Rentals" : settings.RentalSenderName;
                var fromAddress = string.IsNullOrWhiteSpace(settings.RentalSenderEmail) ? "rentals@gloucesterfraternityclub.com" : settings.RentalSenderEmail;

                message.From.Add(new MailboxAddress(fromName, fromAddress));
                message.To.Add(new MailboxAddress("", recipientEmail));

                if (!string.IsNullOrWhiteSpace(ccEmail))
                {
                    foreach (var addr in ccEmail.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        if (addr.Contains("@"))
                        {
                            message.Cc.Add(new MailboxAddress("", addr));
                        }
                    }
                }

                message.Subject = subject;
                var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody.Replace("\n", "<br/>") };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                var port = settings.RentalSmtpPort > 0 ? settings.RentalSmtpPort : 587;
                var secureOptions = settings.RentalSmtpEnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;

                await client.ConnectAsync(settings.RentalSmtpHost, port, secureOptions);

                if (!string.IsNullOrEmpty(settings.RentalSmtpUsername))
                {
                    await client.AuthenticateAsync(settings.RentalSmtpUsername, settings.RentalSmtpPassword ?? string.Empty);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Hall rental email sent successfully via SMTP to {Recipient}", recipientEmail);
                return EmailResult.Successful($"Email successfully sent via SMTP to {recipientEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP transmission failure for {Recipient}", recipientEmail);
                var innerMsg = ex.InnerException != null ? $" ({ex.InnerException.Message})" : "";
                return EmailResult.Failure($"SMTP transmission failed: {ex.Message}{innerMsg}");
            }
        }

        private async Task<EmailResult> SendViaResendAsync(
            WebsiteSettings settings,
            string recipientEmail,
            string subject,
            string htmlBody,
            string? ccEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(settings.RentalResendApiKey) || !settings.RentalResendApiKey.StartsWith("re_"))
                {
                    return EmailResult.Failure("Resend API Key is missing or invalid. It must start with 're_'.");
                }

                var resendClient = ResendClient.Create(settings.RentalResendApiKey);

                var fromName = string.IsNullOrWhiteSpace(settings.RentalSenderName) ? "GFC Hall Rentals" : settings.RentalSenderName;
                var fromAddress = string.IsNullOrWhiteSpace(settings.RentalSenderEmail) ? "rentals@gloucesterfraternityclub.com" : settings.RentalSenderEmail;

                var message = new EmailMessage
                {
                    From = $"\"{fromName}\" <{fromAddress}>",
                    Subject = subject,
                    HtmlBody = htmlBody.Replace("\n", "<br/>")
                };

                message.To.Add(recipientEmail);

                if (!string.IsNullOrWhiteSpace(ccEmail))
                {
                    foreach (var addr in ccEmail.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        if (addr.Contains("@"))
                        {
                            message.Cc.Add(addr);
                        }
                    }
                }

                var response = await resendClient.EmailSendAsync(message);
                if (response.Success)
                {
                    _logger.LogInformation("Hall rental email sent successfully via Resend API to {Recipient}", recipientEmail);
                    return EmailResult.Successful($"Email successfully sent via Resend API to {recipientEmail}");
                }

                return EmailResult.Failure($"Resend delivery failed: {response.Exception?.Message ?? "Unknown error"}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Resend API failure for {Recipient}", recipientEmail);
                return EmailResult.Failure($"Resend API error: {ex.Message}");
            }
        }
    }
}
