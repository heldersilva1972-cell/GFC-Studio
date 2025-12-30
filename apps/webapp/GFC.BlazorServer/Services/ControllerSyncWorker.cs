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
    private bool _wasOffline = false;

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

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var syncQueue = scope.ServiceProvider.GetRequiredService<IControllerSyncQueueRepository>();
                var controllerClient = scope.ServiceProvider.GetRequiredService<IControllerClient>();

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
                        // Mark as processing
                        await syncQueue.UpdateStatusAsync(item.QueueId, "PROCESSING");

                        // Attempt sync to controller
                        if (item.Action == "ACTIVATE")
                        {
                            // Get door permissions for this card
                            var dbContext = scope.ServiceProvider.GetRequiredService<GFC.BlazorServer.Data.GfcDbContext>();
                            var permissions = await dbContext.MemberDoorAccesses
                                .Include(a => a.Door)
                                .Where(a => a.CardNumber == item.CardNumber && a.IsEnabled)
                                .ToListAsync(stoppingToken);

                            if (!permissions.Any())
                            {
                                _logger.LogWarning("No door permissions found for card {CardNumber} - deactivating instead", item.CardNumber);
                                await controllerClient.DeletePrivilegeAsync(long.Parse(item.CardNumber), stoppingToken);
                            }
                            else
                            {
                                foreach (var perm in permissions)
                                {
                                    var controllerId = perm.Door?.ControllerId ?? 0;
                                    if (controllerId == 0)
                                    {
                                        var door = await dbContext.Doors.FindAsync(new object[] { perm.DoorId }, stoppingToken);
                                        if (door != null) controllerId = door.ControllerId;
                                    }

                                    if (controllerId == 0) continue;

                                    // Resolve time profile index
                                    int? timeProfileIndex = null;
                                    if (perm.TimeProfileId.HasValue)
                                    {
                                        var link = await dbContext.ControllerTimeProfileLinks
                                            .FirstOrDefaultAsync(l => l.ControllerId == controllerId && l.TimeProfileId == perm.TimeProfileId.Value, stoppingToken);
                                        timeProfileIndex = link?.ControllerProfileIndex;
                                    }

                                    var privilege = new CardPrivilegeModel
                                    {
                                        ControllerId = controllerId,
                                        DoorId = perm.DoorId,
                                        CardNumber = long.Parse(item.CardNumber),
                                        TimeProfileIndex = timeProfileIndex,
                                        Enabled = true
                                    };

                                    await controllerClient.AddOrUpdatePrivilegeAsync(privilege, stoppingToken);
                                }
                            }
                        }
                        else
                        {
                            await controllerClient.DeletePrivilegeAsync(long.Parse(item.CardNumber), stoppingToken);
                        }

                        // Success - mark as completed
                        await syncQueue.MarkAsCompletedAsync(item.QueueId);
                        _logger.LogInformation(
                            "Successfully synced queued item {QueueId} for card {CardNumber} (action {Action}, attempt {Attempt})",
                            item.QueueId, item.CardNumber, item.Action, item.AttemptCount + 1);
                    }
                    catch (Exception ex)
                    {
                        // Failed - increment attempt count and log error
                        // NEVER give up - just keep retrying
                        await syncQueue.IncrementAttemptAsync(item.QueueId, ex.Message);
                        _logger.LogWarning(ex,
                            "Failed to sync queued item {QueueId} (attempt {Attempt}) - will retry in 30 minutes",
                            item.QueueId, item.AttemptCount + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in controller sync worker");
            }

            // Run every 5 minutes to check for items ready to retry
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
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
            1 => TimeSpan.FromMinutes(5),    // 5 minutes
            2 => TimeSpan.FromMinutes(15),   // 15 minutes
            _ => TimeSpan.FromMinutes(30)    // 30 minutes forever
        };

        return lastAttemptDate.Value.Add(delay);
    }
}
