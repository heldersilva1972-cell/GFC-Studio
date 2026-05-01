using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string recipientEmail, string subject, string body, Dictionary<string, byte[]>? attachments = null);
    Task SendOrderEmailAsync(string toEmail, string subject, LiquorOrder order);
}
