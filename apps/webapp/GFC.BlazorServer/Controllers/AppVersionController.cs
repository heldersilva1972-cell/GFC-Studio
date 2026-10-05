using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/app")]
[Microsoft.AspNetCore.Cors.EnableCors("GfcEcosystemPolicy")]
public class AppVersionController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly IDeviceTrustService _deviceTrustService;
    private readonly ILogger<AppVersionController> _logger;

    public AppVersionController(
        IWebHostEnvironment env,
        IDeviceTrustService deviceTrustService,
        ILogger<AppVersionController> logger)
    {
        _env = env;
        _deviceTrustService = deviceTrustService;
        _logger = logger;
    }

    [HttpGet("version")]
    [AllowAnonymous]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> GetAppVersion()
    {
        var packagesDir = Path.Combine(_env.ContentRootPath, "App_Data", "Packages");
        var versionFile = Path.Combine(packagesDir, "version.json");

        if (System.IO.File.Exists(versionFile))
        {
            try
            {
                var json = await System.IO.File.ReadAllTextAsync(versionFile);
                using var doc = JsonDocument.Parse(json);
                return Ok(doc.RootElement.Clone());
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse version.json in App_Data/Packages. Falling back to default metadata.");
            }
        }

        // Default Version Metadata for GFC Connect
        return Ok(new
        {
            latestVersionCode = 100,
            latestVersionName = "1.0.0",
            downloadUrl = "/api/app/download/latest.apk",
            mandatoryUpdate = false,
            releaseNotes = "Initial release of GFC Connect with biometric access and push notifications.",
            releasedAt = DateTime.UtcNow
        });
    }

    [HttpGet("download/latest.apk")]
    [AllowAnonymous]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> DownloadLatestApk([FromQuery] string? token)
    {
        // 1. Verify Authentication (Cookie, Bearer Header, or Token Query Parameter)
        bool isAuthenticated = User.Identity?.IsAuthenticated == true;

        if (!isAuthenticated)
        {
            // Extract token from query or header
            var authToken = token;
            if (string.IsNullOrEmpty(authToken))
            {
                var header = Request.Headers["Authorization"].ToString();
                if (header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    authToken = header.Substring("Bearer ".Length).Trim();
                }
            }

            if (!string.IsNullOrEmpty(authToken))
            {
                isAuthenticated = await _deviceTrustService.ValidateTokenAsync(authToken) ||
                                  await _deviceTrustService.IsStationTokenAsync(authToken);
            }
        }

        if (!isAuthenticated)
        {
            _logger.LogWarning("[GFC-CONNECT] Unauthorized APK download attempt rejected.");
            return Unauthorized(new { error = "Unauthorized. Please provide a valid device token or log in." });
        }

        // 2. Locate Private APK Package
        var packagesDir = Path.Combine(_env.ContentRootPath, "App_Data", "Packages");
        var primaryApk = Path.Combine(packagesDir, "gfc-connect.apk");
        var fallbackApk = Path.Combine(packagesDir, "latest.apk");

        var targetFile = System.IO.File.Exists(primaryApk) ? primaryApk : (System.IO.File.Exists(fallbackApk) ? fallbackApk : null);

        if (targetFile == null)
        {
            _logger.LogError("[GFC-CONNECT] Requested APK not found on server at {PackagesDir}", packagesDir);
            return NotFound(new { error = "GFC Connect APK package is not currently available on the server." });
        }

        // 3. Stream File Securely with Cache-Control Headers
        Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";

        var fileStream = new FileStream(targetFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(fileStream, "application/vnd.android.package-archive", "gfc-connect.apk", enableRangeProcessing: true);
    }
}
