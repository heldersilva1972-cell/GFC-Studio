namespace GFC.Core.DTOs;

public class DeviceSessionDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DeviceToken { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime LastUsedUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsStation { get; set; }
    public string? StationName { get; set; }
    public string? LoginMode { get; set; }
}
