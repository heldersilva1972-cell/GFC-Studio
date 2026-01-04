using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using GFC.BlazorServer.Services;
using GFC.BlazorServer.Services.Vpn;
using GFC.Core.Interfaces;
using GFC.Core.Services;
using System.ComponentModel.DataAnnotations;
using GFC.BlazorServer.Auth;

namespace GFC.BlazorServer.Controllers;

/// <summary>
/// Public API controller for onboarding gateway.
/// This controller is accessible without VPN for bootstrapping new devices.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("onboarding")]
public class OnboardingController : ControllerBase
{
    private readonly IVpnConfigurationService _vpnConfigService;
    private readonly IBlazorSystemSettingsService _systemSettingsService;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<OnboardingController> _logger;

    public OnboardingController(
        IVpnConfigurationService vpnConfigService,
        IBlazorSystemSettingsService systemSettingsService,
        IAuditLogger auditLogger,
        ILogger<OnboardingController> logger)
    {
        _vpnConfigService = vpnConfigService;
        _systemSettingsService = systemSettingsService;
        _auditLogger = auditLogger;
        _logger = logger;
    }

    /// <summary>
    /// Validates an onboarding token and returns user information if valid.
    /// </summary>
    /// <param name="token">The onboarding token to validate</param>
    /// <returns>User information if token is valid</returns>
    [HttpGet("validate")]
    [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ValidateToken([Required] string token)
    {
        try
        {
            // Check if onboarding is enabled
            var settings = await _systemSettingsService.GetSystemSettingsAsync();
            if (settings == null || !settings.EnableOnboarding)
            {
                _logger.LogWarning("Onboarding attempt rejected: Onboarding is disabled");
                return StatusCode(StatusCodes.Status403Forbidden, new { error = "Onboarding is currently disabled" });
            }

            // Check if system is in safe mode
            if (settings.SafeModeEnabled)
            {
                _logger.LogWarning("Onboarding attempt rejected: System is in safe mode");
                return StatusCode(StatusCodes.Status403Forbidden, new { error = "System is in safe mode" });
            }

            // Validate token
            var userId = await _vpnConfigService.ValidateOnboardingTokenAsync(token);
            if (!userId.HasValue)
            {
                _logger.LogWarning("Invalid or expired token: {Token}", token.Substring(0, Math.Min(8, token.Length)));
                return NotFound(new { error = "Invalid or expired token" });
            }

            _logger.LogInformation("Token validated successfully for user {UserId}", userId.Value);
            _auditLogger.Log(AuditLogActions.VpnOnboardingStarted, null, userId.Value, $"Onboarding initiated via gateway. IP: {Request.HttpContext.Connection.RemoteIpAddress}");

            return Ok(new TokenValidationResponse
            {
                Valid = true,
                UserId = userId.Value,
                UserName = $"User {userId.Value}" // TODO: Get actual username from user service
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating onboarding token");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while validating the token" });
        }
    }

    /// <summary>
    /// Generates and downloads a WireGuard configuration file for the user.
    /// </summary>
    /// <param name="token">The onboarding token</param>
    /// <param name="deviceName">Optional device name</param>
    /// <param name="deviceType">Optional device type</param>
    /// <returns>WireGuard configuration file</returns>
    [HttpGet("config")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConfiguration([Required] string token, string? deviceName = null, string? deviceType = null)
    {
        try
        {
            // Check if onboarding is enabled
            var settings = await _systemSettingsService.GetSystemSettingsAsync();
            if (settings == null || !settings.EnableOnboarding)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { error = "Onboarding is currently disabled" });
            }
 
            // Validate token (must not be used yet)
            var userId = await _vpnConfigService.ValidateOnboardingTokenAsync(token);
            if (!userId.HasValue)
            {
                return NotFound(new { error = "Invalid or already used token" });
            }
 
            // Generate configuration (this creates/retrieves persistent VpnProfile)
            var configContent = await _vpnConfigService.GenerateConfigForUserAsync(userId.Value, deviceName, deviceType);
 
            _logger.LogInformation("WireGuard config generated for user {UserId}", userId.Value);
 
            // Return as downloadable file
            var bytes = System.Text.Encoding.UTF8.GetBytes(configContent);
            return File(bytes, "application/x-wireguard-profile", "gfc-access.conf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating configuration");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while generating the configuration" });
        }
    }
 
    /// <summary>
    /// Downloads the Apple (.mobileconfig) setup profile.
    /// This includes the WireGuard config, DNS, and Root CA.
    /// </summary>
    [HttpGet("apple-profile")]
    [Produces("application/x-apple-aspen-config")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppleProfile([Required] string token)
    {
        try
        {
            // Validate token
            var userId = await _vpnConfigService.ValidateOnboardingTokenAsync(token);
            if (!userId.HasValue)
            {
                return NotFound(new { error = "Invalid or expired token" });
            }

            var profileBytes = await _vpnConfigService.GenerateAppleProfileAsync(userId.Value);
            
            // Note: In production, this should be signed using a certificate (CMS/PKCS7).
            // For now, we return the raw XML. iOS/macOS will still accept it but show "Unsigned".

            _auditLogger.Log(AuditLogActions.VpnAppleProfileDownloaded, userId.Value, userId.Value, $"Apple .mobileconfig profile downloaded. IP: {Request.HttpContext.Connection.RemoteIpAddress}");
            return File(profileBytes, "application/x-apple-aspen-config", "GFC-Access.mobileconfig");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error serving Apple profile");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Downloads the Windows one-click setup script.
    /// Injects the token and API URL into the script.
    /// </summary>
    [HttpGet("windows-setup")]
    [Produces("application/x-powershell")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWindowsSetupScript([Required] string token)
    {
        try
        {
            // Validate token
            var userId = await _vpnConfigService.ValidateOnboardingTokenAsync(token);
            if (!userId.HasValue)
            {
                return NotFound(new { error = "Invalid or expired token" });
            }

            var scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "infrastructure", "scripts", "Install-GfcVpn.ps1");
            
            if (!System.IO.File.Exists(scriptPath))
            {
                // Try alternate path (deployment)
                scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "scripts", "Install-GfcVpn.ps1");
            }

            if (!System.IO.File.Exists(scriptPath))
            {
                _logger.LogError("Setup script template not found at {Path}", scriptPath);
                return NotFound("Setup script template not found.");
            }

            var scriptContent = await System.IO.File.ReadAllTextAsync(scriptPath);
            
            // Inject values (simple string replacement or regex if needed)
            // The template uses mandatory parameters, so we can just append a call at the bottom
            // or modify the default values.
            
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var finalScript = scriptContent
                .Replace("param(", "param(\n    [string]$Token = \"" + token + "\",\n    [string]$ApiUrl = \"" + baseUrl + "\",")
                .Replace("[Parameter(Mandatory=$true)]\n    [string]$Token,", "")
                .Replace("[Parameter(Mandatory=$false)]\n    [string]$ApiUrl = \"https://gfc.lovanow.com\"", "");

            // Note: The above replacement is a bit brittle, let's just prepend the variables
            // and remove the Param block or make parameters optional.
            // Actually, the template I wrote has parameters. Let's just append the execution line.
            
            var footer = $"\n\n# Auto-generated execution call\nInstall-GfcVpn -Token \"{token}\" -ApiUrl \"{baseUrl}\"";
            // Wait, my template is not a function. It's a script. 
            // So I should just modify the parameter defaults.
            
            var bytes = System.Text.Encoding.UTF8.GetBytes(scriptContent
                .Replace("$Token,", $"$Token = \"{token}\",")
                .Replace("$ApiUrl = \"https://gfc.lovanow.com\"", $"$ApiUrl = \"{baseUrl}\""));

            _auditLogger.Log(AuditLogActions.VpnWindowsSetupDownloaded, userId.Value, userId.Value, $"Windows One-Click setup script downloaded. IP: {Request.HttpContext.Connection.RemoteIpAddress}");
            return File(bytes, "application/x-powershell", "Setup-GFC-VPN.ps1");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error serving setup script");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Downloads the internal GFC Root CA certificate.
    /// This establishes trust for the gfc.lovanow.com domain.
    /// </summary>
    [HttpGet("ca-cert")]
    [Produces("application/x-x509-ca-cert")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCaCertificate([FromQuery] string? token = null)
    {
        // Check Access
        bool isAuthorized = false;

        // 1. Check if user is Admin (cookie auth)
        if (User.Identity?.IsAuthenticated == true && User.IsInRole(AppRoles.Admin))
        {
            isAuthorized = true;
        }
        // 2. Check if valid token provided
        else if (!string.IsNullOrEmpty(token))
        {
            // We use ValidateOnboardingTokenAsync purely to check validity.
            // This token is NOT marked as used here, just checked.
            // Setup scripts will use the token later for 'config', which validates it again.
            var userId = await _vpnConfigService.ValidateOnboardingTokenAsync(token);
            if (userId.HasValue)
            {
                isAuthorized = true;
            }
        }

        if (!isAuthorized)
        {
            _logger.LogWarning("Unauthorized attempt to download Root CA. Token provided: {HasToken}, User: {User}, IP: {IP}", 
                !string.IsNullOrEmpty(token), 
                User.Identity?.Name ?? "Anonymous",
                Request.HttpContext.Connection.RemoteIpAddress);
            return StatusCode(StatusCodes.Status403Forbidden, "Access denied. Admin rights or valid token required.");
        }

        try
        {
            // Assuming the CA cert is stored in a known location relative to the app
            var caPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "certs", "GFC_Root_CA.cer");
            
            // Fallback for development/testing
            if (!System.IO.File.Exists(caPath))
            {
                // Try the infrastructure path if we're in the source tree
                caPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "infrastructure", "ca", "GFC_Root_CA.cer");
            }
 
            if (!System.IO.File.Exists(caPath))
            {
                _logger.LogWarning("Root CA certificate not found at {Path}", caPath);
                return NotFound("Certificate not found. Contact administrator.");
            }
 
            _auditLogger.Log(AuditLogActions.VpnCaCertDownloaded, null, null, $"Root CA Certificate downloaded from onboarding gateway. IP: {Request.HttpContext.Connection.RemoteIpAddress}");
            var bytes = await System.IO.File.ReadAllBytesAsync(caPath);
            return File(bytes, "application/x-x509-ca-cert", "GFC_Root_CA.cer");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error serving CA certificate");
            return StatusCode(500, "Internal server error");
        }
    }
 
    /// <summary>
    /// Marks an onboarding token as used/completed.
    /// </summary>
    /// <param name="token">The onboarding token</param>
    /// <param name="request">Completion details</param>
    /// <returns>Success status</returns>
    [HttpPost("complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteOnboarding(
        [Required] string token,
        [FromBody] OnboardingCompletionRequest request)
    {
        try
        {
            // Validate token
            var userId = await _vpnConfigService.ValidateOnboardingTokenAsync(token);
            if (!userId.HasValue)
            {
                return NotFound(new { error = "Invalid or expired token" });
            }
 
            // Mark token as used in database
            await _vpnConfigService.SetTokenUsedAsync(token);
 
            _logger.LogInformation(
                "Onboarding completed for user {UserId}, Device: {DeviceInfo}, Platform: {Platform}, Success: {TestPassed}",
                userId.Value,
                request.DeviceInfo ?? "Unknown",
                request.Platform ?? "Unknown",
                request.TestPassed);
 
            _auditLogger.Log(AuditLogActions.VpnOnboardingCompleted, null, userId.Value, $"Gateway wizard finished. Platform: {request.Platform}, Device: {request.DeviceInfo}");
 
            return Ok(new { success = true, message = "Onboarding marked as completed" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing onboarding");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while completing onboarding" });
        }
    }
}

/// <summary>
/// Response model for token validation
/// </summary>
public class TokenValidationResponse
{
    public bool Valid { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
}

/// <summary>
/// Request model for onboarding completion
/// </summary>
public class OnboardingCompletionRequest
{
    public string? DeviceInfo { get; set; }
    public string? Platform { get; set; }
    public bool TestPassed { get; set; }
}
