using Microsoft.AspNetCore.Components.Authorization;
using GFC.Core.Models;
using GFC.Core.Interfaces;
using System.Threading.Tasks;

namespace GFC.Mobile.Auth;

public interface ICustomAuthenticationStateProvider
{
    Task<AuthenticationState> GetAuthenticationStateAsync();
    AppUser? GetCurrentUser();
    Task<GFC.Core.Models.GfcLoginResult> LoginAsync(string username, string password, bool rememberDevice);
    Task<GFC.Core.Models.GfcLoginResult> LoginWithUserAsync(int userId);
    Task LogoutAsync(string? token = null);
    Task RefreshUserAsync();
    Task ForceReAuthAsync();
}
