using Microsoft.AspNetCore.Components.Authorization;
using GFC.Core.Models;

namespace GFC.Mobile.Auth;

public interface ICustomAuthenticationStateProvider
{
    Task<AuthenticationState> GetAuthenticationStateAsync();
    AppUser? GetCurrentUser();
    Task LoginAsync(AppUser user, string token, int expiresInMinutes = 1440);
    Task LogoutAsync(string? token = null);
    Task RefreshUserAsync();
    Task ForceReAuthAsync();
}
