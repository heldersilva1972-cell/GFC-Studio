using GFC.Core.Enums;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GFC.BlazorServer.Services;

public interface IEmailProviderFactory
{
    IEmailService GetEmailService();
}

public class EmailProviderFactory(
    IServiceProvider serviceProvider,
    IOptionsMonitor<EmailSettings> emailSettings) : IEmailProviderFactory
{
    public IEmailService GetEmailService()
    {
        var settings = emailSettings.CurrentValue;
        
        return settings.Provider switch
        {
            EmailProvider.Resend => serviceProvider.GetRequiredService<ResendApiService>(),
            EmailProvider.SMTP => serviceProvider.GetRequiredService<SmtpEmailService>(),
            _ => serviceProvider.GetRequiredService<SmtpEmailService>()
        };
    }
}
