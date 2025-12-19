namespace GFC.Core.Models;

/// <summary>
/// Application-wide settings.
/// </summary>
public class AppSettings
{
    public string SettingKey { get; set; } = string.Empty;
    public string SettingValue { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? LastModified { get; set; }
    public string? ModifiedBy { get; set; }
}

