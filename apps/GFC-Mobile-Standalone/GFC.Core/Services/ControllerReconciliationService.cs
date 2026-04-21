using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Extensions.Logging;

namespace GFC.Core.Services;

public class ControllerReconciliationService
{
    private readonly IKeyCardRepository _keyCardRepo;
    private readonly ILogger<ControllerReconciliationService> _logger;

    public ControllerReconciliationService(
        IKeyCardRepository keyCardRepo,
        ILogger<ControllerReconciliationService> logger)
    {
        _keyCardRepo = keyCardRepo;
        _logger = logger;
    }

    public async Task<ReconciliationResult> ReconcileAsync(List<string> controllerCardNumbers, CancellationToken ct = default)
    {
        var result = new ReconciliationResult();
        
        var dbActiveCards = await Task.Run(() => _keyCardRepo.GetAll()
            .Where(c => c.IsActive)
            .ToList(), ct);
        
        var controllerSet = controllerCardNumbers.ToHashSet();
        
        foreach (var dbCard in dbActiveCards)
        {
            if (!controllerSet.Contains(dbCard.CardNumber))
            {
                _logger.LogWarning("Card {CardNumber} active in DB but missing from controller", dbCard.CardNumber);
                dbCard.IsControllerSynced = false;
                result.OutOfSyncCards.Add(dbCard.CardNumber);
            }
            else
            {
                dbCard.IsControllerSynced = true;
                dbCard.LastControllerSyncDate = DateTime.Now;
                result.SyncedCards.Add(dbCard.CardNumber);
            }
            
            _keyCardRepo.Update(dbCard);
        }
        
        var dbCardNumbers = dbActiveCards.Select(c => c.CardNumber).ToHashSet();
        result.GhostCards = controllerSet.Except(dbCardNumbers).ToList();
        
        if (result.GhostCards.Any())
        {
            _logger.LogWarning("Found {Count} ghost cards in controller: {Cards}", 
                result.GhostCards.Count, string.Join(", ", result.GhostCards));
        }
        
        return result;
    }
}

public class ReconciliationResult
{
    public List<string> SyncedCards { get; set; } = new();
    public List<string> OutOfSyncCards { get; set; } = new();
    public List<string> GhostCards { get; set; } = new();
}
