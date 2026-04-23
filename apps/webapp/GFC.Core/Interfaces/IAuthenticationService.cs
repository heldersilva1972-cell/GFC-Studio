// [REFRESHED]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces;

public interface IAuthenticationService
{
    Task<GFC.Core.Models.GfcLoginResult> LoginAsync(string username, string password, string? ipAddress = null, bool rememberDevice = false);
    Task<GFC.Core.Models.GfcLoginResult> LoginWithDeviceTokenAsync(string token, string? ipAddress = null);
    Task<GFC.Core.Models.GfcLoginResult> VerifyMfaCodeAsync(int userId, string code, string? ipAddress = null, bool rememberDevice = false);
    Task<GFC.Core.Models.GfcLoginResult> LoginMagicLinkAsync(int userId, string? ipAddress = null);
    Task<GFC.Core.Models.GfcLoginResult> FinalizeMfaLoginAsync(int userId, bool rememberDevice, string? ipAddress = null);
    Task<GFC.Core.Models.GfcLoginResult> LoginWithPasskeyAsync(string username, string? ipAddress = null);
    MfaSetupInfo GenerateMfaSetup(AppUser user);
    Task LogoutAsync(string? deviceToken = null);
    AppUser? GetCurrentUser();
    Task<AppUser?> RefreshCurrentUserAsync();
}
