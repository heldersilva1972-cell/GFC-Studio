using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResult> LoginAsync(string username, string password, string? ipAddress = null);
    Task<LoginResult> VerifyMfaCodeAsync(int userId, string code, string? ipAddress = null);
    MfaSetupInfo GenerateMfaSetup(AppUser user);
    void Logout();
    AppUser? GetCurrentUser();
}

public enum LoginResultCode
{
    Success,
    InvalidCredentials,
    AccountLockedOrDisabled,
    MfaRequired,
    Error
}

public class LoginResult
{
    public LoginResultCode Code { get; set; }
    public bool Success => Code == LoginResultCode.Success;
    public AppUser? User { get; set; }
    public bool PasswordChangeRequired { get; set; }
    public string? ErrorMessageForLog { get; set; }
}

