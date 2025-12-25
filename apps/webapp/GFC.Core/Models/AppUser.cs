namespace GFC.Core.Models;

/// <summary>
/// Represents a user account in the system.
/// </summary>
public class AppUser
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public int? MemberId { get; set; } // Link to member if user is a member
    public DateTime CreatedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? Notes { get; set; }
    public bool PasswordChangeRequired { get; set; }
    public bool MfaEnabled { get; set; }
    public string? MfaSecretKey { get; set; }
}

