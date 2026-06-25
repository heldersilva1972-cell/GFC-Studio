using System.Security.Claims;
using System.Text.Encodings.Web;
using GFC.BlazorServer.Auth;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GFC.BlazorServer.Auth;

/// <summary>
/// Correct Authentication Handler for the GFC System.
/// Validates the custom 'GFC_DeviceTrustToken' cookie against the database via IAuthenticationService.
/// This enables standard [Authorize] attributes to function correctly across all controllers.
/// </summary>
public class GfcTokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly GFC.Core.Interfaces.IAuthenticationService _authenticationService;
    private readonly IMemoryCache _cache;

    public GfcTokenAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        GFC.Core.Interfaces.IAuthenticationService authenticationService,
        IMemoryCache cache) : base(options, logger, encoder)
    {
        _authenticationService = authenticationService;
        _cache = cache;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // 1. Extract token from cookie or Authorization header
        if (!Request.Cookies.TryGetValue("GFC_DeviceTrustToken", out var token) || string.IsNullOrEmpty(token))
        {
            // Fallback to Authorization header for standalone mobile apps (handles Cross-Origin cookie blocking)
            var authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authHeader.Substring("Bearer ".Length).Trim();
            }
        }

        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.NoResult();
        }

        try 
        {
            // 2. Validate token via the core authentication service (with cache-aside)
            var cacheKey = $"device_token_validation:{token}";
            if (!_cache.TryGetValue(cacheKey, out GfcLoginResult result))
            {
                Console.WriteLine($"[AUTH SERVER] Validating token against DB: {token.Substring(0, Math.Min(token.Length, 15))}...");
                result = await _authenticationService.LoginWithDeviceTokenAsync(token);
                if (result != null && result.Success && result.User != null)
                {
                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(15));
                }
                else
                {
                    Console.WriteLine($"[AUTH SERVER] DB Validation FAILED. Code: {result?.Code}, Error: {result?.ErrorMessageForLog ?? "None"}");
                }
            }

            if (result != null && result.Success && result.User != null)
            {
                Console.WriteLine($"[AUTH SERVER] Validation SUCCESS for user: {result.User.Username} (ID: {result.User.UserId})");
                // 3. Build claims identical to CustomAuthenticationStateProvider.BuildPrincipal
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.User.Username),
                    new Claim(ClaimTypes.NameIdentifier, result.User.UserId.ToString()),
                    new Claim("UserId", result.User.UserId.ToString()),
                    new Claim("IsAdmin", result.User.IsAdmin ? "true" : "false")
                };

                // Check for station identity cookie
                if (Request.Cookies.ContainsKey("GFC_StationIdentity"))
                {
                    claims.Add(new Claim("IsStation", "true"));
                }

                // Map roles
                if (result.User.IsAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, AppRoles.Admin));
                }

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error validating GFC Device Trust Token");
            Console.WriteLine($"[AUTH SERVER] CRITICAL EXCEPTION validating token: {ex.Message}");
            return AuthenticateResult.Fail("Authentication service error");
        }

        Console.WriteLine($"[AUTH SERVER] Rejecting request: Invalid or expired token.");
        return AuthenticateResult.Fail("Invalid or expired token");
    }
}

