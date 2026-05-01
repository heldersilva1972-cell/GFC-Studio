using GFC.Core.Interfaces;
using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Dispatches email requests to the configured provider (Resend or SMTP) via EmailProviderFactory.
/// </summary>
public class EmailService(IEmailProviderFactory factory) : IEmailService
{
    public Task SendEmailAsync(string recipientEmail, string subject, string body, Dictionary<string, byte[]>? attachments = null)
    {
        return factory.GetEmailService().SendEmailAsync(recipientEmail, subject, body, attachments);
    }

    public Task SendOrderEmailAsync(string toEmail, string subject, LiquorOrder order)
    {
        return factory.GetEmailService().SendOrderEmailAsync(toEmail, subject, order);
    }
}
