using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GFC.BlazorServer.Services;

public class ConfigureEmailSettings(IServiceProvider serviceProvider) : IConfigureOptions<EmailSettings>
{
    public void Configure(EmailSettings options)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var settingsService = scope.ServiceProvider.GetRequiredService<IBlazorSystemSettingsService>();
            
            // Use synchronous method to get settings for options configuration
            var settings = settingsService.GetSettings();

            options.Provider = settings.EmailProvider;
            options.ResendApiKey = settings.ResendApiKey;
            options.SmtpHost = settings.SmtpHost;
            options.SmtpPort = settings.SmtpPort;
            options.SmtpUsername = settings.SmtpUsername;
            options.SmtpPassword = settings.SmtpPassword;
            options.SmtpEnableSsl = settings.SmtpEnableSsl;
            options.FromAddress = settings.SmtpFromAddress ?? string.Empty;
            options.FromName = settings.SmtpFromName ?? "GFC System";
        }
        catch (Exception)
        {
            // Fallback to defaults or appsettings values if DB is not available
        }
    }
}
