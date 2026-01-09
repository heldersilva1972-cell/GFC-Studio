using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Background service that automatically syncs events from all controllers to the database
/// </summary>
public class ControllerEventSyncService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ControllerEventSyncService> _logger;
    private readonly TimeSpan _syncInterval = TimeSpan.FromSeconds(30); // Sync every 30 seconds

    public ControllerEventSyncService(
        IServiceScopeFactory scopeFactory,
        ILogger<ControllerEventSyncService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        
        // This MUST appear in logs if service is being constructed
        _logger.LogCritical("!!! CONTROLLER EVENT SYNC SERVICE CONSTRUCTOR CALLED !!!");
        Console.WriteLine("!!! CONTROLLER EVENT SYNC SERVICE CONSTRUCTOR CALLED !!!");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogCritical("========================================");
            _logger.LogCritical("=== CONTROLLER EVENT SYNC SERVICE STARTING ===");
            _logger.LogCritical("=== Interval: {Interval}s ===", _syncInterval.TotalSeconds);
            _logger.LogCritical("========================================");

            // Wait 10 seconds on startup to let the app fully initialize
            _logger.LogCritical("Waiting 10 seconds for app initialization...");
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            _logger.LogCritical("=== STARTING SYNC LOOP NOW ===");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SyncAllControllersAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    // Clean shutdown
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Controller Event Sync Service");
                }

                await Task.Delay(_syncInterval, stoppingToken);
            }

            _logger.LogInformation("Controller Event Sync Service stopped");
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Controller Event Sync Service cancelled during startup");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "FATAL: Controller Event Sync Service failed to start!");
            // No throw - don't kill the app if background sync fails
        }
    }

    private async Task SyncAllControllersAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<GfcDbContext>>();
        var controllerClient = scope.ServiceProvider.GetRequiredService<IControllerClient>();
        var eventService = scope.ServiceProvider.GetRequiredService<ControllerEventService>();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        
        // Get all registered controllers
        var controllers = await db.Controllers
            .Where(c => c.IsEnabled)
            .Select(c => new { c.Id, c.SerialNumber, c.Name })
            .ToListAsync(cancellationToken);

        if (!controllers.Any())
        {
            _logger.LogInformation("No enabled controllers found for event sync");
            return;
        }

        _logger.LogError("=== EVENT SYNC: Syncing events for {Count} controllers ===", controllers.Count);

        foreach (var controller in controllers)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            try
            {
                // Sync events from this controller
                var newEventCount = await eventService.SyncFromControllerAsync(
                    controller.SerialNumber,
                    controllerClient,
                    progressCallback: null,
                    cancellationToken: cancellationToken);

                if (newEventCount > 0)
                {
                    _logger.LogError(
                        "=== EVENT SYNC: Synced {Count} new events from controller {Name} (SN: {Serial}) ===",
                        newEventCount,
                        controller.Name,
                        controller.SerialNumber);
                }
                else
                {
                    _logger.LogDebug("=== EVENT SYNC: No new events for controller {Name} (SN: {Serial}) ===",
                        controller.Name,
                        controller.SerialNumber);
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                // Log but don't crash - continue with other controllers
                _logger.LogWarning(ex, 
                    "Failed to sync events from controller {Name} (SN: {Serial})", 
                    controller.Name, 
                    controller.SerialNumber);
            }
        }
    }
}
