namespace GFC.Core.Models;

/// <summary>
/// Represents a user account in the system.
/// </summary>
public class AppUser
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public int? MemberId { get; set; } // Link to member if user is a member
    public DateTime CreatedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? Notes { get; set; }
    public bool PasswordChangeRequired { get; set; }
    public string? PassCodeHash { get; set; }
    public bool MfaEnabled { get; set; }
    public string? MfaSecretKey { get; set; }
    public decimal? HourlyRate { get; set; }
    public bool IsTrackedEmployee { get; set; }

    // W-4 Tax Profile (Federal)
    public string FilingStatus { get; set; } = "Single";
    public bool HasMultipleJobs { get; set; }
    public decimal DependentsAmount { get; set; }
    public decimal OtherIncomeAmount { get; set; }
    public decimal DeductionsAmount { get; set; }
    public decimal ExtraWithholdingAmount { get; set; }
    public string? SocialSecurityNumber { get; set; }
}

public enum LoginResultCode { Success, InvalidCredentials, AccountLockedOrDisabled, MfaRequired, Error } 

public class GfcLoginResult 
{ 
    public LoginResultCode Code { get; set; } 
    public bool Success => Code == LoginResultCode.Success; 
    public AppUser? User { get; set; } 
    public bool PasswordChangeRequired { get; set; } 
    public string? ErrorMessageForLog { get; set; } 
    public string? DeviceToken { get; set; } 
    public List<GFC.Core.DTOs.MobilePermissionDto>? Permissions { get; set; } 
    public List<string>? AllowedRoutes { get; set; } 
}
