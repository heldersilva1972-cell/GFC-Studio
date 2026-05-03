using GFC.Core.Interfaces;
using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Dispatches email requests to the configured provider (Resend or SMTP) via EmailProviderFactory.
/// </summary>
public class EmailService(IEmailProviderFactory factory) : IEmailService
{
    public Task<EmailResult> SendEmailAsync(string recipientEmail, string subject, string body, Dictionary<string, byte[]>? attachments = null, string? ccEmail = null)
    {
        return factory.GetEmailService().SendEmailAsync(recipientEmail, subject, body, attachments, ccEmail);
    }
}
