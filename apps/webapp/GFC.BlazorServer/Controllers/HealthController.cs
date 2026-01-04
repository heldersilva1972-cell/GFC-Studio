using Microsoft.AspNetCore.Mvc;
using GFC.Core.Interfaces;

namespace GFC.BlazorServer.Controllers;

/// <summary>
/// Health check endpoints for system monitoring and VPN connection testing
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IUserConnectionService _connectionService;
    private readonly ILogger<HealthController> _logger;

    public HealthController(
        IUserConnectionService connectionService,
        ILogger<HealthController> logger)
    {
        _connectionService = connectionService;
        _logger = logger;
    }

    /// <summary>
    /// Basic health check endpoint
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
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
