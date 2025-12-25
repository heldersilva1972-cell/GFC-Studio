// [NEW]
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email notification.
        /// </summary>
        /// <param name="recipientEmail">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The HTML body of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendEmailAsync(string recipientEmail, string subject, string body);
    }
}
