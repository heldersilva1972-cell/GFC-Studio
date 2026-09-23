using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IRentalEmailDispatcher
    {
        Task<EmailResult> SendRentalEmailAsync(
            WebsiteSettings settings,
            string recipientEmail,
            string subject,
            string htmlBody,
            string? ccEmail = null);

        Task<EmailResult> TestRentalEmailConnectionAsync(
            WebsiteSettings settings,
            string testRecipientEmail);
    }
}
