using Microsoft.AspNetCore.Components.Authorization;
using GFC.Core.Models;
using GFC.Core.Interfaces;
using System.Threading.Tasks;

namespace GFC.Mobile.Auth;

public interface ICustomAuthenticationStateProvider
{
    Task<AuthenticationState> GetAuthenticationStateAsync();
    AppUser? GetCurrentUser();
    Task<LoginResult> LoginAsync(string username, string password, bool rememberDevice);
    Task<LoginResult> LoginWithUserAsync(int userId);
    Task LogoutAsync(string? token = null);
    Task RefreshUserAsync();
    Task ForceReAuthAsync();
}
