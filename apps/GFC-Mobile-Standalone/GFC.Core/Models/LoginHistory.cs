namespace GFC.Core.Models;

/// <summary>
/// Tracks user login events.
/// </summary>
public class LoginHistory
{
    public int LoginHistoryId { get; set; }
    public int? UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime LoginDate { get; set; }
    public string? IpAddress { get; set; }
    public bool LoginSuccessful { get; set; }
    public string? FailureReason { get; set; }
}

