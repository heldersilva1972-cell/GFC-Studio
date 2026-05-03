using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IEmailService
{
    Task<EmailResult> SendEmailAsync(string recipientEmail, string subject, string body, Dictionary<string, byte[]>? attachments = null, string? ccEmail = null);
}
