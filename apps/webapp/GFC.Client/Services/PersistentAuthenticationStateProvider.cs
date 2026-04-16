using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GFC.Client.Services;

/// <summary>
/// WASM-side AuthenticationStateProvider.
/// Reads the authentication state that was established server-side and passed
/// down through the Blazor persistent component state mechanism.
/// This satisfies @attribute [Authorize] and AuthStateProvider injection
/// in InteractiveWebAssembly components.
/// </summary>
public class PersistentAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly Task<AuthenticationState> _unauthenticated =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    private readonly Task<AuthenticationState> _authenticationState;

    public PersistentAuthenticationStateProvider(PersistentComponentState state)
    {
        if (!state.TryTakeFromJson<UserInfo>("UserInfo", out var userInfo) || userInfo is null)
        {
            _authenticationState = _unauthenticated;
            return;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userInfo.Name ?? ""),
            new(ClaimTypes.NameIdentifier, userInfo.UserId ?? ""),
        };

        if (userInfo.IsAdmin)
            claims.Add(new Claim("IsAdmin", "true"));

        var identity = new ClaimsIdentity(claims, authenticationType: nameof(PersistentAuthenticationStateProvider));
        _authenticationState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => _authenticationState;
}

public sealed class UserInfo
{
    public string? UserId { get; init; }
    public string? Name { get; init; }
    public bool IsAdmin { get; init; }
}
