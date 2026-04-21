using System.Threading.Tasks;
using System.Net.Http;
using GFC.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace GFC.Core.Services;

public class SmsService : ISmsService
{
    private readonly ILogger<SmsService> _logger;
    private readonly ISystemSettingsService _settingsService;
    private readonly IHttpClientFactory _httpClientFactory;

    public SmsService(ILogger<SmsService> logger, ISystemSettingsService settingsService, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _settingsService = settingsService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        if (!await _settingsService.GetSmsEnabledAsync())
        {
            _logger.LogInformation("SMS sending skipped: Globally disabled in settings.");
            return false;
        }

        var accountSid = await _settingsService.GetTwilioAccountSidAsync();
        var authTokenStr = await _settingsService.GetTwilioAuthTokenAsync();
        var fromNumber = await _settingsService.GetTwilioFromNumberAsync();
        
        if (string.IsNullOrWhiteSpace(accountSid) || 
            string.IsNullOrWhiteSpace(authTokenStr) || 
            string.IsNullOrWhiteSpace(fromNumber))
        {
            _logger.LogWarning("SMS sending failed: Twilio credentials not fully configured.");
            return false;
        }

        try
        {
            using var client = _httpClientFactory.CreateClient();
            var authHeader = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{accountSid}:{authTokenStr}"));
            
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

            var values = new Dictionary<string, string>
            {
                { "To", phoneNumber },
                { "From", fromNumber },
                { "Body", message }
            };

            var content = new FormUrlEncodedContent(values);
            var url = $"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/Messages.json";
            
            var response = await client.PostAsync(url, content);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SMS sent successfully to {PhoneNumber}", phoneNumber);
                return true;
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("Twilio SMS failed: {StatusCode} - {Error}", response.StatusCode, errorBody);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while sending Twilio SMS");
            return false;
        }
    }
}
