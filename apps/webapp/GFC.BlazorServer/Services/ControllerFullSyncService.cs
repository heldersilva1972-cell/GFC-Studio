using GFC.Core.Interfaces;
using GFC.BlazorServer.Services.Controllers;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Handles full synchronization of all active cards to the controller
/// </summary>
public class ControllerFullSyncService
{
    private readonly IKeyCardRepository _keyCardRepository;
    private readonly IControllerClient _controllerClient;
    private readonly IControllerSyncQueueRepository _syncQueueRepository;
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ControllerRegistryService _controllerRegistry;
    private readonly ILogger<ControllerFullSyncService> _logger;

    public ControllerFullSyncService(
        IKeyCardRepository keyCardRepository,
        IControllerClient controllerClient,
        IControllerSyncQueueRepository syncQueueRepository,
        IDbContextFactory<GfcDbContext> contextFactory,
        ControllerRegistryService controllerRegistry,
        ILogger<ControllerFullSyncService> logger)
    {
        _keyCardRepository = keyCardRepository;
        _controllerClient = controllerClient;
        _syncQueueRepository = syncQueueRepository;
        _contextFactory = contextFactory;
        _controllerRegistry = controllerRegistry;
        _logger = logger;
    }

    /// <summary>
    /// Perform a full sync of all active cards to the controller
    /// </summary>
    public async Task<FullSyncResult> PerformFullSyncAsync(CancellationToken ct = default)
    {
        var result = new FullSyncResult
        {
            StartTime = DateTime.Now
        };

        try
        {
            _logger.LogInformation("Starting full controller sync");

            // Step 1: Get all active cards from database
            var allCards = await Task.Run(() => _keyCardRepository.GetAll(), ct);
            var activeCards = allCards.Where(c => c.IsActive).ToList();
            
            result.TotalCards = activeCards.Count;
            _logger.LogInformation("Found {Count} active cards to sync", activeCards.Count);

            // Step 2: Clear all controllers first to ensure a clean state
            var controllers = await _controllerRegistry.GetControllersAsync(includeDoors: false, cancellationToken: ct);
            foreach (var controller in controllers)
            {
                try
                {
                    await _controllerClient.ClearAllCardsAsync(controller.Id, ct);
                    _logger.LogInformation("Cleared controller {ControllerId}", controller.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to clear controller {ControllerId} - continuing with sync", controller.Id);
                }
            }

            // Step 3: Sync each active card
            await using var dbContext = await _contextFactory.CreateDbContextAsync(ct);
            foreach (var card in activeCards)
            {
                if (ct.IsCancellationRequested)
                {
                    result.WasCancelled = true;
                    break;
                }

                try
                {
                    // Get door permissions for this card
                    var permissions = await dbContext.MemberDoorAccesses
                        .Include(a => a.Door)
                        .Where(a => a.CardNumber == card.CardNumber && a.IsEnabled)
                        .ToListAsync(ct);

                    if (!permissions.Any())
                    {
                        _logger.LogWarning("No door permissions found for active card {CardNumber} - deactivating", card.CardNumber);
                        await _controllerClient.DeletePrivilegeAsync(long.Parse(card.CardNumber), ct);
                    }
                    else
                    {
                        foreach (var perm in permissions)
                        {
                            var controllerId = perm.Door?.ControllerId ?? 0;
                            if (controllerId == 0)
                            {
                                var door = await dbContext.Doors.FindAsync(new object[] { perm.DoorId }, ct);
                                if (door != null) controllerId = door.ControllerId;
                            }

                            if (controllerId == 0) continue;

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
                                CardNumber = long.Parse(card.CardNumber),
                                TimeProfileIndex = timeProfileIndex,
                                Enabled = true
                            };

                            await _controllerClient.AddOrUpdatePrivilegeAsync(privilege, ct);
                        }
                    }
                    
                    result.SuccessCount++;
                    _logger.LogDebug("Synced card {CardNumber}", card.CardNumber);
                }
                catch (Exception ex)
                {
                    result.FailureCount++;
                    result.Errors.Add($"Card {card.CardNumber}: {ex.Message}");
                    _logger.LogError(ex, "Failed to sync card {CardNumber}", card.CardNumber);
                }
            }

            // Step 4: Clear all pending sync queue items (they're now synced)
            try
            {
                var pendingItems = await _syncQueueRepository.GetPendingItemsAsync();
                foreach (var item in pendingItems)
                {
                    await _syncQueueRepository.MarkAsCompletedAsync(item.QueueId);
                }
                result.QueueItemsCleared = pendingItems.Count;
                
                _logger.LogInformation("Cleared {Count} pending sync queue items", pendingItems.Count);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to clear sync queue items");
            }

            result.EndTime = DateTime.Now;
            result.Success = result.FailureCount == 0 && !result.WasCancelled;

            _logger.LogInformation(
                "Full sync completed: {Success}/{Total} cards synced, {Failed} failures, {Duration}s",
                result.SuccessCount, result.TotalCards, result.FailureCount, 
                (result.EndTime.Value - result.StartTime).TotalSeconds);

            return result;
        }
        catch (Exception ex)
        {
            result.EndTime = DateTime.Now;
            result.Success = false;
            result.Errors.Add($"Full sync failed: {ex.Message}");
            
            _logger.LogError(ex, "Full sync operation failed");
            return result;
        }
    }
}

/// <summary>
/// Result of a full sync operation
/// </summary>
public class FullSyncResult
{
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool Success { get; set; }
    public bool WasCancelled { get; set; }
    public int TotalCards { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public int QueueItemsCleared { get; set; }
    public List<string> Errors { get; set; } = new();

    public TimeSpan Duration => EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;
}
