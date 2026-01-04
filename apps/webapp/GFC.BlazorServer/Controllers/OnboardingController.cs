using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using GFC.BlazorServer.Services;
using GFC.BlazorServer.Services.Vpn;
using System.ComponentModel.DataAnnotations;

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
    private readonly ILogger<OnboardingController> _logger;

    public OnboardingController(
        IVpnConfigurationService vpnConfigService,
        IBlazorSystemSettingsService systemSettingsService,
        ILogger<OnboardingController> logger)
    {
        _vpnConfigService = vpnConfigService;
        _systemSettingsService = systemSettingsService;
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
    /// <returns>WireGuard configuration file</returns>
    [HttpGet("config")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConfiguration([Required] string token)
    {
        try
        {
            // Check if onboarding is enabled
            var settings = await _systemSettingsService.GetSystemSettingsAsync();
            if (settings == null || !settings.EnableOnboarding)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { error = "Onboarding is currently disabled" });
            }

            // Validate token
            var userId = await _vpnConfigService.ValidateOnboardingTokenAsync(token);
            if (!userId.HasValue)
            {
                return NotFound(new { error = "Invalid or expired token" });
            }

            // Generate configuration
            var configContent = await _vpnConfigService.GenerateConfigForUserAsync(userId.Value);

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
    /// Downloads the internal GFC Root CA certificate.
    /// This establishes trust for the gfc.lovanow.com domain.
    /// </summary>
    [HttpGet("ca-cert")]
    [Produces("application/x-x509-ca-cert")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCaCertificate()
    {
        try
        {
            // Assuming the CA cert is stored in a known location relative to the app
            // In a real environment, this would be configured in SystemSettings
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

            // TODO: Mark token as used in database
            // TODO: Log device information
            // TODO: Send notification to admins (optional)

            _logger.LogInformation(
                "Onboarding completed for user {UserId}, Device: {DeviceInfo}",
                userId.Value,
                request.DeviceInfo ?? "Unknown");

            return Ok(new { success = true, message = "Onboarding completed successfully" });
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
