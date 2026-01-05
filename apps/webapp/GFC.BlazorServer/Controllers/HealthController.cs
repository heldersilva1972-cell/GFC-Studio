using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GFC.Core.Interfaces;
using GFC.BlazorServer.Services.Operations;
using GFC.BlazorServer.Auth;

namespace GFC.BlazorServer.Controllers;

/// <summary>
/// Health check endpoints for system monitoring and VPN connection testing
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IUserConnectionService _connectionService;
    private readonly IOperationsService _operationsService;
    private readonly ILogger<HealthController> _logger;

    public HealthController(
        IUserConnectionService connectionService,
        IOperationsService operationsService,
        ILogger<HealthController> logger)
    {
        _connectionService = connectionService;
        _operationsService = operationsService;
        _logger = logger;
    }

    /// <summary>
    /// Basic health check endpoint (Public Safe)
    /// </summary>
    [HttpGet("/health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetHealth()
    {
        try 
        {
            var info = await _operationsService.GetHealthInfoAsync();
            if (info.IsHealthy)
            {
                return Ok("OK");
            }
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "FAIL");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "FAIL");
        }
    }

    /// <summary>
    /// Detailed health check endpoint (Admin Only)
    /// </summary>
    [HttpGet("/health/details")]
    [Authorize(Policy = AppPolicies.RequireAdmin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHealthDetails()
    {
        var info = await _operationsService.GetHealthInfoAsync();
        return Ok(info);
    }

    /// <summary>
    /// Legacy API endpoint
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        // For backward compatibility, return a simple JSON
        var info = await _operationsService.GetHealthInfoAsync();
        return Ok(new
        {
            status = info.IsHealthy ? "healthy" : "unhealthy",
            timestamp = DateTime.UtcNow,
            version = info.AppVersion
        });
    }

    /// <summary>
    /// VPN connection test endpoint.
    /// Returns 200 OK if the request is coming from VPN subnet.
    /// </summary>
    [HttpGet("vpn-check")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult VpnCheck()
    {
        try
        {
            var locationType = _connectionService.LocationType;
            var ipAddress = _connectionService.IpAddress;

            _logger.LogInformation(
                "VPN check from IP: {IpAddress}, Location: {LocationType}",
                ipAddress,
                locationType);

            if (locationType == LocationType.VPN)
            {
                return Ok(new
                {
                    connected = true,
                    locationType = locationType.ToString(),
                    message = "VPN connection verified"
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    connected = false,
                    locationType = locationType.ToString(),
                    message = "Not connected via VPN"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during VPN check");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                connected = false,
                message = "Error checking VPN connection"
            });
        }
    }

    /// <summary>
    /// Returns the client's IP address and connection information
    /// </summary>
    [HttpGet("connection-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetConnectionInfo()
    {
        try
        {
            return Ok(new
            {
                ipAddress = _connectionService.IpAddress,
                locationType = _connectionService.LocationType.ToString(),
                isVpn = _connectionService.LocationType == LocationType.VPN,
                isLan = _connectionService.LocationType == LocationType.LAN,
                isPublic = _connectionService.LocationType == LocationType.Public,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting connection info");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = "Error retrieving connection information"
            });
        }
    }
}
