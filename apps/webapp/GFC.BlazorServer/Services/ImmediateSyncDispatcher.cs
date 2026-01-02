using GFC.Core.Interfaces;
using GFC.BlazorServer.Services.Controllers;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

public class ImmediateSyncDispatcher : IImmediateSyncDispatcher
{
    private readonly IControllerSyncQueueRepository _queueRepo;
    private readonly IControllerSyncService _syncService;
    private readonly IKeyCardRepository _keyCardRepository;
    private readonly ILogger<ImmediateSyncDispatcher> _logger;

    public ImmediateSyncDispatcher(
        IControllerSyncQueueRepository queueRepo,
        IControllerSyncService syncService,
        IKeyCardRepository keyCardRepository,
        ILogger<ImmediateSyncDispatcher> logger)
    {
        _queueRepo = queueRepo;
        _syncService = syncService;
        _keyCardRepository = keyCardRepository;
        _logger = logger;
    }

    public async Task DispatchSyncAsync(int keyCardId, bool activate, CancellationToken ct = default)
    {
        var card = _keyCardRepository.GetById(keyCardId);
        if (card == null) return;

        // 1. Add to queue first (safety first, ensures we don't lose the intent if we crash during immediate try)
        var queueId = await _queueRepo.AddAsync(new GFC.Core.Models.ControllerSyncQueueItem
        {
            KeyCardId = keyCardId,
            CardNumber = card.CardNumber,
            Action = activate ? "ACTIVATE" : "DEACTIVATE",
            QueuedDate = DateTime.Now,
            Status = "PENDING",
            AttemptCount = 0
        });

        // 2. Try to process it immediately in a background-safe way
        _ = Task.Run(async () =>
        {
            try
            {
                // We give a small delay to ensure the DB transaction for AddAsync is committed
                // and to avoid blocking the main UI thread.
                await Task.Delay(100, ct); 
                await _syncService.ProcessQueueItemAsync(queueId, ct);
                _logger.LogInformation("Successfully performed immediate sync for card {CardNumber}", card.CardNumber);
            }
            catch (Exception ex)
            {
                // If immediate try fails (e.g. controller offline), we don't need to do anything.
                // The item is already in 'PENDING' state in the queue, 
                // and the ControllerSyncWorker will pick it up automatically.
                _logger.LogInformation("Immediate sync for card {CardNumber} deferred to background queue: {Message}", 
                    card.CardNumber, ex.Message);
            }
        }, ct);
    }
}
