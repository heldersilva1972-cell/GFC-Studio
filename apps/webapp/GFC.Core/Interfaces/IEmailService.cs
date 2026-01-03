// [NEW]
using System.Threading.Tasks;

namespace GFC.Core.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string recipientEmail, string subject, string body);
}
