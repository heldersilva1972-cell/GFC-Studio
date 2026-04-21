using System.Threading.Tasks;

namespace GFC.Core.Interfaces;

public interface ISmsService
{
    /// <summary>
    /// Sends an SMS message to the specified phone number.
    /// </summary>
    Task<bool> SendSmsAsync(string phoneNumber, string message);
}
