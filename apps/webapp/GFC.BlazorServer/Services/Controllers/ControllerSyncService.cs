using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.Controllers;

public interface IControllerSyncService
{
    Task ProcessQueueItemAsync(int queueId, CancellationToken ct = default);
    Task ProcessAllPendingAsync(CancellationToken ct = default);
}

public class ControllerSyncService : IControllerSyncService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly IControllerSyncQueueRepository _queueRepo;
    private readonly IControllerClient _controllerClient;
    private readonly ICommunicationLogService _logService;
    private readonly ILogger<ControllerSyncService> _logger;

    public ControllerSyncService(
        IDbContextFactory<GfcDbContext> contextFactory,
        IControllerSyncQueueRepository queueRepo,
        IControllerClient controllerClient,
        ICommunicationLogService logService,
        ILogger<ControllerSyncService> logger)
    {
        _contextFactory = contextFactory;
        _queueRepo = queueRepo;
        _controllerClient = controllerClient;
        _logService = logService;
        _logger = logger;
    }

    public async Task ProcessAllPendingAsync(CancellationToken ct = default)
    {
        var pendingItems = await _queueRepo.GetPendingItemsAsync();
        foreach (var item in pendingItems)
        {
            if (ct.IsCancellationRequested) break;
            await ProcessQueueItemAsync(item.QueueId, ct);
        }
    }

    public async Task ProcessQueueItemAsync(int queueId, CancellationToken ct = default)
    {
        var item = await _queueRepo.GetByIdAsync(queueId);
        if (item == null || item.Status != "PENDING")
        {
            _logger.LogWarning("Queue item {QueueId} not found or not in PENDING status for processing", queueId);
            return;
        }

        try
        {
            // Mark as processing
            await _queueRepo.UpdateStatusAsync(item.QueueId, "PROCESSING");
            _logger.LogInformation("Processing sync item {QueueId} (Card: {CardNumber}, Action: {Action})", 
                item.QueueId, item.CardNumber, item.Action);

            await using var dbContext = await _contextFactory.CreateDbContextAsync(ct);

            // Attempt sync to controller
            if (item.Action == "ACTIVATE")
            {
                // Get door permissions for this card
                var permissions = await dbContext.MemberDoorAccesses
                    .Include(a => a.Door)
                    .Where(a => a.CardNumber == item.CardNumber && a.IsEnabled)
                    .ToListAsync(ct);

                if (!permissions.Any())
                {
                    _logger.LogWarning("No door permissions found for card {CardNumber}. Skipping activation.", item.CardNumber);
                    await _queueRepo.IncrementAttemptAsync(item.QueueId, "No door permissions found for card.");
                    return;
                }
                
                foreach (var perm in permissions)
                {
                    var controllerId = perm.Door?.ControllerId ?? 0;
                    if (controllerId == 0)
                    {
                        var door = await dbContext.Doors.FindAsync(new object[] { perm.DoorId }, ct);
                        if (door != null) controllerId = door.ControllerId;
                    }

                    if (controllerId == 0)
                    {
                        _logger.LogWarning("Could not resolve Controller ID for door {DoorId}", perm.DoorId);
                        continue;
                    }

                    // Resolve time profile index
                    int? timeProfileIndex = null;
                    if (perm.TimeProfileId.HasValue)
                    {
                        var link = await dbContext.ControllerTimeProfileLinks
                            .FirstOrDefaultAsync(l => l.ControllerId == controllerId && l.TimeProfileId == perm.TimeProfileId.Value, ct);
                        timeProfileIndex = link?.ControllerProfileIndex;
                    }

                    var privilege = new CardPrivilegeModel
                    {
                        ControllerId = controllerId,
                        DoorId = perm.DoorId,
                        CardNumber = long.Parse(item.CardNumber),
                        TimeProfileIndex = timeProfileIndex ?? 1, // Default to Always if not specified
                        Enabled = true
                    };

                    await _controllerClient.AddOrUpdatePrivilegeAsync(privilege, ct);
                }
            }
            else
            {
                // DEACTIVATE logic
                // Find all controllers that have this card and remove it
                // We'll iterate through all controllers since we don't know exactly which ones have it
                // in the current schema without more complex tracking.
                // But for now, we'll use the controller associated with the card's doors if available.
                
                var controllers = await dbContext.Controllers.ToListAsync(ct);
                foreach (var controller in controllers)
                {
                    await _controllerClient.DeletePrivilegeAsync(long.Parse(item.CardNumber), ct);
                }
            }

            // Success - mark as completed
            await _queueRepo.MarkAsCompletedAsync(item.QueueId);
            _logger.LogInformation(
                "Successfully synced queued item {QueueId} for card {CardNumber} (action {Action})",
                item.QueueId, item.CardNumber, item.Action);
        }
        catch (Exception ex)
        {
            // Failed - increment attempt count and log error
            await _queueRepo.IncrementAttemptAsync(item.QueueId, ex.Message);
            _logger.LogWarning(ex, "Failed to sync queued item {QueueId}", item.QueueId);
            throw; // Re-throw to allow UI to handle if needed
        }
    }
}
