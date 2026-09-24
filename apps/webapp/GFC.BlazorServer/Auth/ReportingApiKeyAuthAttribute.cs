using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GFC.BlazorServer.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ReportingApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ReportingApiKeyHeader = "X-Reporting-Key";
        private const string AuthHeader = "Authorization";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();
            var httpContext = context.HttpContext;
            var dbContext = httpContext.RequestServices.GetRequiredService<GfcDbContext>();

            string? extractedKey = null;

            if (httpContext.Request.Headers.TryGetValue(ReportingApiKeyHeader, out var customHeaderVal))
            {
                extractedKey = customHeaderVal.FirstOrDefault();
            }
            else if (httpContext.Request.Headers.TryGetValue(AuthHeader, out var authHeaderVal))
            {
                var authStr = authHeaderVal.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(authStr) && authStr.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    extractedKey = authStr.Substring(7).Trim();
                }
            }

            var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            var endpoint = httpContext.Request.Path.Value ?? "/api/reporting";
            var queryString = httpContext.Request.QueryString.Value;

            if (string.IsNullOrWhiteSpace(extractedKey))
            {
                await LogRequestAsync(dbContext, null, endpoint, queryString, clientIp, null, userAgent, (int)stopwatch.ElapsedMilliseconds, 0, 401, "API Key was not provided");
                context.Result = new JsonResult(new { error = "Reporting API Key was not provided. Use 'X-Reporting-Key' or 'Authorization: Bearer <key>'." })
                {
                    StatusCode = 401
                };
                return;
            }

            var keyHash = ComputeSha256Hash(extractedKey);

            var apiKeyRecord = await dbContext.ReportingApiKeys
                .Include(k => k.CreatedByUser)
                .FirstOrDefaultAsync(k => k.KeyHash == keyHash && k.IsActive);

            if (apiKeyRecord == null)
            {
                await LogRequestAsync(dbContext, null, endpoint, queryString, clientIp, null, userAgent, (int)stopwatch.ElapsedMilliseconds, 0, 401, "Invalid or inactive API key");
                context.Result = new JsonResult(new { error = "Invalid or inactive Reporting API Key." })
                {
                    StatusCode = 401
                };
                return;
            }

            if (apiKeyRecord.ExpiresAtUtc.HasValue && apiKeyRecord.ExpiresAtUtc.Value < DateTime.UtcNow)
            {
                await LogRequestAsync(dbContext, apiKeyRecord.Id, endpoint, queryString, clientIp, null, userAgent, (int)stopwatch.ElapsedMilliseconds, 0, 403, "API Key has expired");
                context.Result = new JsonResult(new { error = "Reporting API Key has expired." })
                {
                    StatusCode = 403
                };
                return;
            }

            // Resolve Known Device Name (e.g. from VpnProfiles or configured AllowedIpAddress)
            string? resolvedDeviceName = apiKeyRecord.BoundDeviceName;
            if (string.IsNullOrWhiteSpace(resolvedDeviceName))
            {
                var vpnProfile = await dbContext.Set<VpnProfile>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.AssignedIP == clientIp && v.RevokedAt == null);

                if (vpnProfile != null && !string.IsNullOrWhiteSpace(vpnProfile.DeviceName))
                {
                    resolvedDeviceName = vpnProfile.DeviceName;
                }
            }

            // Check IP Binding constraint if specified
            if (!string.IsNullOrWhiteSpace(apiKeyRecord.AllowedIpAddress) && !apiKeyRecord.AllowedIpAddress.Equals(clientIp, StringComparison.OrdinalIgnoreCase))
            {
                await LogRequestAsync(dbContext, apiKeyRecord.Id, endpoint, queryString, clientIp, resolvedDeviceName, userAgent, (int)stopwatch.ElapsedMilliseconds, 0, 403, $"IP address {clientIp} not permitted for this key");
                context.Result = new JsonResult(new { error = "This API Key is locked to a specific authorized workstation/IP." })
                {
                    StatusCode = 403
                };
                return;
            }

            // Check Dataset Access Permission
            if (!string.IsNullOrWhiteSpace(apiKeyRecord.AllowedDatasets) && !apiKeyRecord.AllowedDatasets.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                var allowedList = apiKeyRecord.AllowedDatasets.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var pathSegment = endpoint.TrimEnd('/').Split('/').LastOrDefault() ?? "";
                
                if (!endpoint.EndsWith("/metadata", StringComparison.OrdinalIgnoreCase) && 
                    !allowedList.Contains(pathSegment, StringComparer.OrdinalIgnoreCase))
                {
                    await LogRequestAsync(dbContext, apiKeyRecord.Id, endpoint, queryString, clientIp, resolvedDeviceName, userAgent, (int)stopwatch.ElapsedMilliseconds, 0, 403, $"Access to dataset '{pathSegment}' is not authorized for this key");
                    context.Result = new JsonResult(new { error = $"Access to dataset '{pathSegment}' is restricted for this API Key." })
                    {
                        StatusCode = 403
                    };
                    return;
                }
            }

            // Update Last Used timestamp
            apiKeyRecord.LastUsedAtUtc = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();

            // Store in HttpContext items so the action can record record count and complete the log
            httpContext.Items["ReportingApiKeyId"] = apiKeyRecord.Id;
            httpContext.Items["ReportingClientIp"] = clientIp;
            httpContext.Items["ReportingResolvedDevice"] = resolvedDeviceName;
            httpContext.Items["ReportingUserAgent"] = userAgent;

            var executedContext = await next();
            stopwatch.Stop();

            int recordCount = 0;
            if (httpContext.Items.TryGetValue("ReportingRecordCount", out var rcObj) && rcObj is int count)
            {
                recordCount = count;
            }

            int statusCode = executedContext.HttpContext.Response.StatusCode;
            await LogRequestAsync(dbContext, apiKeyRecord.Id, endpoint, queryString, clientIp, resolvedDeviceName, userAgent, (int)stopwatch.ElapsedMilliseconds, recordCount, statusCode, null);
        }

        private static async Task LogRequestAsync(GfcDbContext db, int? apiKeyId, string endpoint, string? queryString, string ip, string? deviceName, string? userAgent, int durationMs, int recordCount, int statusCode, string? error)
        {
            try
            {
                var log = new ReportingApiLog
                {
                    ApiKeyId = apiKeyId,
                    RequestTimestampUtc = DateTime.UtcNow,
                    Endpoint = endpoint,
                    QueryParameters = queryString,
                    IpAddress = ip,
                    ResolvedDeviceName = deviceName,
                    UserAgent = string.IsNullOrWhiteSpace(userAgent) ? null : (userAgent.Length > 250 ? userAgent.Substring(0, 250) : userAgent),
                    DurationMs = durationMs,
                    RecordCount = recordCount,
                    ResponseStatusCode = statusCode,
                    ErrorMessage = error
                };

                db.ReportingApiLogs.Add(log);
                await db.SaveChangesAsync();
            }
            catch
            {
                // Telemetry logging must not break API responses
            }
        }

        public static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
