using GFC.Core.Enums;

namespace GFC.Core.Models;

public class EmailSettings
{
    public EmailProvider Provider { get; set; } = EmailProvider.SMTP;
    
    // Resend
    public string? ResendApiKey { get; set; }
    
    // SMTP
    public string? SmtpHost { get; set; }
    public int SmtpPort { get; set; } = 587;
    public string? SmtpUsername { get; set; }
    public string? SmtpPassword { get; set; }
    public bool SmtpEnableSsl { get; set; } = true;

    // Global
    public string FromAddress { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
}
