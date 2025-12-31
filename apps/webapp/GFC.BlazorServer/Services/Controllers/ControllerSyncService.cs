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
                    
                    _logService.Log(new CommunicationLogEntry
                    {
                        ControllerSn = 0,
                        Operation = "ACTIVATE",
                        IsError = true,
                        ErrorMessage = "No door permissions found in database for this card. Sync skipped.",
                        Description = $"Card {item.CardNumber} has no assigned doors in MemberDoorAccess table."
                    });

                    await _queueRepo.IncrementAttemptAsync(item.QueueId, "No door permissions found for card.");
                    return;
                }
                
                // Group permissions by controller
                var perController = permissions.GroupBy(p => p.Door?.ControllerId ?? 0);

                foreach (var group in perController)
                {
                    var controllerId = group.Key;
                    if (controllerId == 0) continue;

                    _logger.LogInformation("Performing Robust Step-by-Step Sync for controller {Id}...", controllerId);

                    _logger.LogInformation("Performing Triple-Handshake Sync (Reset & Unlock) for controller {Id}...", controllerId);

                    // 1-3. Execute Deep Reset Sequence (Broadcast Search -> Clear -> Sync Time -> Verify)
                    // This now handles the "Month 26" lock recovery automatically.
                    await _controllerClient.ResetControllerAsync(controllerId, ct: ct);
                    
                    // 4. Verify Clock (already done inside ResetControllerAsync, but getting status for logging)
                    var status = await _controllerClient.GetRunStatusAsync(controllerId, ct).ConfigureAwait(false);

                    if (!status.ControllerTimeUtc.HasValue || status.ControllerTimeUtc.Value.Year < 2025)
                    {
                        var currentYear = status.ControllerTimeUtc?.Year.ToString() ?? "Unknown";
                        var errorMsg = $"[Hardware Clock Lock] Controller {controllerId} rejected sync. Year is still {currentYear}. Clear All (0x54) might have failed.";
                        _logger.LogError(errorMsg);
                        await _queueRepo.IncrementAttemptAsync(item.QueueId, errorMsg);
                        throw new InvalidOperationException(errorMsg);
                    }
                    _logger.LogInformation("Clock Verified: {Time}. Cleaning up Ghost Data (Step 4.5)...", status.ControllerTimeUtc);

                    // 4.5 Wipe Ghost Data (0x8E) - Resolve unmapped memory residue on Doors 3 & 4
                    await _controllerClient.SetDoorConfigAsync(controllerId.ToString(), 3, 0x00, 0x00, 0, 0, ct).ConfigureAwait(false);
                    await _controllerClient.SetDoorConfigAsync(controllerId.ToString(), 4, 0x00, 0x00, 0, 0, ct).ConfigureAwait(false);

                    // 5. Add Card (0x50)
                    var doorIndexes = group
                        .Select(p => p.Door?.DoorIndex ?? 0)
                        .Where(idx => idx > 0)
                        .ToList();

                    if (!doorIndexes.Any()) continue;

                    int? timeProfileIndex = null;
                    var firstPerm = group.First();
                    if (firstPerm.TimeProfileId.HasValue)
                    {
                        var link = await dbContext.ControllerTimeProfileLinks
                            .FirstOrDefaultAsync(l => l.ControllerId == controllerId && l.TimeProfileId == firstPerm.TimeProfileId.Value, ct);
                        timeProfileIndex = link?.ControllerProfileIndex;
                    }

                    var privilege = new CardPrivilegeModel
                    {
                        ControllerId = controllerId,
                        CardNumber = long.Parse(item.CardNumber),
                        DoorIndexes = doorIndexes,
                        TimeProfileIndex = timeProfileIndex ?? 1,
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
