// WARNING: DEV-ONLY AUTH BYPASS
// This middleware must NOT be enabled in production.
// Controlled by appsettings.Development.json -> DevAuth.AutoAdminBypassEnabled.

using System.Security.Claims;
using CoreAuthService = GFC.Core.Interfaces.IAuthenticationService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GFC.BlazorServer.Auth;

public class DevAuthAutoAdminMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptions<DevAuthOptions> _options;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<DevAuthAutoAdminMiddleware> _logger;

    public DevAuthAutoAdminMiddleware(
        RequestDelegate next,
        IOptions<DevAuthOptions> options,
        IHostEnvironment hostEnvironment,
        ILogger<DevAuthAutoAdminMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_hostEnvironment.IsDevelopment())
        {
            await _next(context);
            return;
        }

        var settings = _options.Value;
        if (settings is null || !settings.AutoAdminBypassEnabled)
        {
            await _next(context);
            return;
        }

        // If a normal user is already authenticated, do nothing.
        var scopedAuthService = context.RequestServices.GetService<CoreAuthService>();
        var existingUser = scopedAuthService?.GetCurrentUser();
        if (existingUser?.IsActive == true)
        {
            await _next(context);
            return;
        }

        if (context.User?.Identity?.IsAuthenticated != true)
        {
            var devPrincipal = GetOrCreateDevPrincipal(context);

            context.User = devPrincipal;
            context.Items[DevAuthDefaults.DevPrincipalItemKey] = devPrincipal;

            try
            {
                await context.SignInAsync(DevAuthDefaults.AuthenticationScheme, devPrincipal);
            }
            catch (InvalidOperationException ex)
            {
                // Scheme isn't configured in this Blazor-only app; that's OK because we also set HttpContext.User.
                _logger.LogDebug(ex, "DevAuth auto-admin sign-in skipped because no auth scheme is configured.");
            }
        }

        _logger.LogInformation("DevAuth auto-admin bypass active: treating request as Dev Admin.");
        await _next(context);
    }

    private static ClaimsPrincipal GetOrCreateDevPrincipal(HttpContext context)
    {
        if (context.Items.TryGetValue(DevAuthDefaults.DevPrincipalItemKey, out var existing) &&
            existing is ClaimsPrincipal existingPrincipal &&
            existingPrincipal.Identity?.IsAuthenticated == true)
        {
            return existingPrincipal;
        }

        return DevAuthIdentityFactory.CreateDevAdminPrincipal();
    }
}

public static class DevAuthAutoAdminApplicationBuilderExtensions
{
    public static IApplicationBuilder UseDevAuthAutoAdmin(this IApplicationBuilder app)
    {
        return app.UseMiddleware<DevAuthAutoAdminMiddleware>();
    }
}

