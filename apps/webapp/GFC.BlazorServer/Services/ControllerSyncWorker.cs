using GFC.Core.Interfaces;
using GFC.Core.Services;
using GFC.BlazorServer.Services.Controllers;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Background service that processes controller sync queue with infinite retry strategy
/// </summary>
public class ControllerSyncWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ControllerSyncWorker> _logger;

    public ControllerSyncWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<ControllerSyncWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Controller Sync Worker started");

        // On startup, recover any items that were stuck in 'PROCESSING' (e.g. from a crash/restart)
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var syncQueue = scope.ServiceProvider.GetRequiredService<IControllerSyncQueueRepository>();
            await syncQueue.ResetPendingAsync();
            _logger.LogInformation("Recovered orphaned processing items from sync queue.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reset sync queue on startup");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var syncQueue = scope.ServiceProvider.GetRequiredService<IControllerSyncQueueRepository>();
                var syncService = scope.ServiceProvider.GetRequiredService<IControllerSyncService>();

                // Get pending items
                var pendingItems = await syncQueue.GetPendingItemsAsync();

                if (pendingItems.Any())
                {
                    _logger.LogInformation("Processing {Count} pending sync items", pendingItems.Count);
                }

                foreach (var item in pendingItems)
                {
                    // Calculate next retry time based on attempt count
                    var nextRetryTime = CalculateNextRetryTime(item.AttemptCount, item.LastAttemptDate);

                    // Skip if not time to retry yet
                    if (DateTime.Now < nextRetryTime)
                    {
                        continue;
                    }

                    try
                    {
                        await syncService.ProcessQueueItemAsync(item.QueueId, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        // Logged inside service, but we catch to continue with next item
                        _logger.LogWarning("Worker failed to process sync item {QueueId}: {Message}", item.QueueId, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in controller sync worker");
            }

            // Run every 5 seconds to check for new items or items ready to retry
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        _logger.LogInformation("Controller Sync Worker stopped");
    }

    private DateTime CalculateNextRetryTime(int attemptCount, DateTime? lastAttemptDate)
    {
        if (lastAttemptDate == null)
        {
            return DateTime.Now; // First attempt - immediate
        }

        TimeSpan delay = attemptCount switch
        {
            0 => TimeSpan.Zero,              // Immediate
            1 => TimeSpan.FromSeconds(30),   // 30 seconds
            2 => TimeSpan.FromMinutes(1),    // 1 minute
            _ => TimeSpan.FromMinutes(2)     // 2 minutes maximum retry wait for local hardware
        };

        return lastAttemptDate.Value.Add(delay);
    }
}
