using System.Net;
using System.Threading.Tasks;
using Gfc.Agent.Api.Configuration;
using Gfc.Agent.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gfc.Agent.Api.Auth;

internal sealed class ApiKeyAuthenticationMiddleware
{
    private const string HeaderName = "X-Agent-Key";
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiKeyAuthenticationMiddleware> _logger;
    private readonly IOptionsMonitor<AgentApiOptions> _options;

    public ApiKeyAuthenticationMiddleware(
        RequestDelegate next,
        ILogger<ApiKeyAuthenticationMiddleware> logger,
        IOptionsMonitor<AgentApiOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var expected = _options.CurrentValue.ApiKey;
        if (string.IsNullOrWhiteSpace(expected))
        {
            _logger.LogWarning("Agent API key is not configured.");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(ApiResponse.Failure("Agent API key is not configured."));
            return;
        }

        var provided = ExtractApiKey(context);
        if (!string.Equals(expected, provided, System.StringComparison.Ordinal))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsJsonAsync(ApiResponse.Failure("Authentication failed."));
            return;
        }

        await _next(context);
    }

    private static string? ExtractApiKey(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(HeaderName, out var headerValue))
        {
            return headerValue.ToString();
        }

        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var value = authHeader.ToString();
            const string prefix = "Bearer ";
            if (value.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
            {
                return value.Substring(prefix.Length).Trim();
            }
        }

        return null;
    }
}

