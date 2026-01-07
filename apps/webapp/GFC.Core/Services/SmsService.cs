using System.Threading.Tasks;
using GFC.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace GFC.Core.Services;

public class SmsService : ISmsService
{
    private readonly ILogger<SmsService> _logger;

    public SmsService(ILogger<SmsService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        // MOCK IMPLEMENTATION: In a real scenario, this would use Twilio or another SMS gateway.
        // For now, we log the message to the console for development testing.
        
        _logger.LogInformation("[SMS MOCK] To {PhoneNumber}: {Message}", phoneNumber, message);
        
        // Simulate network delay
        await Task.Delay(500);
        
        return true;
    }
}
