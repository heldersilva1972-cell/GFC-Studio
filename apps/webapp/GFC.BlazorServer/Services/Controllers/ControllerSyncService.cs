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
    private readonly IKeyCardRepository _keyCardRepository;
    private readonly ILogger<ControllerSyncService> _logger;

    public ControllerSyncService(
        IDbContextFactory<GfcDbContext> contextFactory,
        IControllerSyncQueueRepository queueRepo,
        IControllerClient controllerClient,
        ICommunicationLogService logService,
        IKeyCardRepository keyCardRepository,
        ILogger<ControllerSyncService> logger)
    {
        _contextFactory = contextFactory;
        _queueRepo = queueRepo;
        _controllerClient = controllerClient;
        _logService = logService;
        _keyCardRepository = keyCardRepository;
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
                    _logger.LogWarning("No door permissions found for card {CardNumber}. Removing from all controllers for cleanup.", item.CardNumber);
                    await _controllerClient.DeletePrivilegeAsync(long.Parse(item.CardNumber), ct);
                    await _queueRepo.MarkAsCompletedAsync(item.QueueId);
                    return;
                }
                
                // Group permissions by controller
                var perController = permissions.GroupBy(p => p.Door?.ControllerId ?? 0);

                foreach (var group in perController)
                {
                    var controllerId = group.Key;
                    if (controllerId == 0) continue;

                    _logger.LogInformation("Syncing card {CardNumber} to controller {Id}...", item.CardNumber, controllerId);

                    // 1. Add/Update Card (Standard Sync)
                    // If the controller fails with a known 'Clock Lock' or timeout, we report it.
                    // We no longer perform a full reset automatically on every sync as it wipes the entire controller memory.

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
                // Find all controllers and remove the card
                _logger.LogInformation("Removing card {CardNumber} from all controllers...", item.CardNumber);
                await _controllerClient.DeletePrivilegeAsync(long.Parse(item.CardNumber), ct);
            }

            // Success - mark item as completed in queue
            await _queueRepo.MarkAsCompletedAsync(item.QueueId);
            
            // CRITICAL: Update the KeyCard record to confirm it is now synced with hardware
            try
            {
                var card = _keyCardRepository.GetById(item.KeyCardId);
                if (card != null)
                {
                    card.IsControllerSynced = true;
                    card.LastControllerSyncDate = DateTime.Now;
                    _keyCardRepository.Update(card);
                    _logger.LogInformation("Confirmed sync status for card {CardNumber} in database.", item.CardNumber);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to update IsControllerSynced for card {CardNumber}, but hardware sync was successful.", item.CardNumber);
            }

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
