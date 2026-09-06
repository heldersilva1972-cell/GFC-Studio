using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services.Diagnostics;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[AllowAnonymous]
[EnableCors("GfcEcosystemPolicy")]
public class HealthController : ControllerBase
{
    private readonly DatabaseHealthService _dbHealthService;
    private readonly IDbContextFactory<GfcDbContext> _dbFactory;
    private readonly ILogger<HealthController> _logger;

    public HealthController(
        DatabaseHealthService dbHealthService, 
        IDbContextFactory<GfcDbContext> dbFactory,
        ILogger<HealthController> logger)
    {
        _dbHealthService = dbHealthService;
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Legacy heartbeat for root path
    /// </summary>
    [HttpGet("health")]
    public IActionResult GetRootHealth() => Ok(new { Status = "Healthy", Service = "GFC.BlazorServer" });

    /// <summary>
    /// Mobile/API heartbeat used by GFC.Mobile and ConnectivityService
    /// </summary>
    [HttpGet("api/health")]
    public async Task<IActionResult> GetApiHealth()
    {
        // We perform a light check. Even if DB is down, the API is "alive".
        // This prevents the mobile app from entering a hard 'Offline' loop 
        // while the server is still trying to connect to SQL.
        try 
        {
            var dbStatus = await _dbHealthService.TestDatabaseConnectionAsync();
            if (dbStatus.Success)
            {
                // Asynchronously update POS terminal LastSeenAt without blocking heartbeat response
                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var db = await _dbFactory.CreateDbContextAsync();
                        await db.PosTerminals
                            .Where(t => !t.IsDeleted)
                            .ExecuteUpdateAsync(s => s.SetProperty(t => t.LastSeenAt, DateTime.UtcNow));
                    }
                    catch
                    {
                        // Ignore heartbeat db update collision
                    }
                });
            }

            return Ok(new { 
                Status = "Online", 
                Database = dbStatus.Success ? "Connected" : "Disconnected",
                Timestamp = DateTime.UtcNow 
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Health check partially failed: {Message}", ex.Message);
            return Ok(new { Status = "Online", Database = "Error", Message = "Service is up, but database is unreachable." });
        }
    }
}
