using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Extensions.Logging;

namespace GFC.Core.Services;

/// <summary>
/// Manages automatic activation/deactivation of cards based on eligibility
/// </summary>
public class KeyCardLifecycleService
{
    private readonly IKeyCardRepository _keyCardRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IControllerSyncQueueRepository _syncQueueRepository;
    private readonly ICardDeactivationLogRepository _deactivationLogRepository;
    private readonly IDuesRepository _duesRepository;
    private readonly IDuesYearSettingsRepository _settingsRepository;
    private readonly ILogger<KeyCardLifecycleService> _logger;
    private readonly KeyCardService _keyCardService;

    public KeyCardLifecycleService(
        IKeyCardRepository keyCardRepository,
        IMemberRepository memberRepository,
        IControllerSyncQueueRepository syncQueueRepository,
        ICardDeactivationLogRepository deactivationLogRepository,
        IDuesRepository duesRepository,
        IDuesYearSettingsRepository settingsRepository,
        ILogger<KeyCardLifecycleService> logger,
        KeyCardService keyCardService)
    {
        _keyCardRepository = keyCardRepository;
        _memberRepository = memberRepository;
        _syncQueueRepository = syncQueueRepository;
        _deactivationLogRepository = deactivationLogRepository;
        _duesRepository = duesRepository;
        _settingsRepository = settingsRepository;
        _logger = logger;
        _keyCardService = keyCardService;
    }

    /// <summary>
    /// Process a single member's card eligibility (called after dues payment)
    /// </summary>
    public async Task ProcessMemberAsync(int memberId, int year, CancellationToken ct = default)
    {
        try
        {
            var member = _memberRepository.GetMemberById(memberId);
            if (member == null)
            {
                _logger.LogWarning("Member {MemberId} not found", memberId);
                return;
            }

            var isEligible = _keyCardService.IsEligibleForCard(member, year);
            var activeCard = _keyCardRepository.GetActiveMemberCard(memberId);

            if (isEligible && activeCard != null && !activeCard.IsActive)
            {
                // Member is now eligible and has an inactive card - reactivate it
                await ReactivateCardAsync(activeCard.KeyCardId, "Dues paid - automatic reactivation", ct);
                _logger.LogInformation("Reactivated card {CardId} for member {MemberId}", activeCard.KeyCardId, memberId);
            }
            else if (!isEligible && activeCard != null && activeCard.IsActive)
            {
                // Member is no longer eligible - deactivate card
                await DeactivateCardAsync(activeCard.KeyCardId, "DuesUnpaid", "Automatic deactivation - dues not satisfied", ct);
                _logger.LogInformation("Deactivated card {CardId} for member {MemberId}", activeCard.KeyCardId, memberId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing member {MemberId} card eligibility", memberId);
        }
    }

    /// <summary>
    /// Deactivate a card with a specific reason
    /// </summary>
    public async Task DeactivateCardAsync(int keyCardId, string reason, string notes, CancellationToken ct = default)
    {
        try
        {
            var card = _keyCardRepository.GetById(keyCardId);
            if (card == null)
            {
                _logger.LogWarning("Card {CardId} not found", keyCardId);
                return;
            }

            // Update card status in database
            card.IsActive = false;
            _keyCardRepository.Update(card);

            // Queue sync to controller
            await SyncCardToControllerAsync(keyCardId, false, ct);
            
            // Log deactivation
            await _deactivationLogRepository.AddAsync(new CardDeactivationLog
            {
                KeyCardId = keyCardId,
                MemberId = card.MemberId,
                DeactivatedDate = DateTime.Now,
                Reason = reason,
                Notes = notes,
                ControllerSynced = false // Will be updated when sync logs success
            });

            _logger.LogInformation("Deactivated card {CardId} - Reason: {Reason}", keyCardId, reason);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating card {CardId}", keyCardId);
            throw;
        }
    }

    /// <summary>
    /// Reactivate a card (dues-related deactivation only)
    /// </summary>
    public async Task ReactivateCardAsync(int keyCardId, string notes, CancellationToken ct = default)
    {
        try
        {
            var card = _keyCardRepository.GetById(keyCardId);
            if (card == null)
            {
                _logger.LogWarning("Card {CardId} not found", keyCardId);
                return;
            }

            // Update card status in database
            card.IsActive = true;
            _keyCardRepository.Update(card);

            // Queue sync to controller
            await SyncCardToControllerAsync(keyCardId, true, ct);

            _logger.LogInformation("Reactivated card {CardId}", keyCardId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reactivating card {CardId}", keyCardId);
            throw;
        }
    }

    /// <summary>
    /// Sync card status to physical controller (with queue fallback)
    /// </summary>
    public async Task SyncCardToControllerAsync(int keyCardId, bool activate, CancellationToken ct = default)
    {
        var card = _keyCardRepository.GetById(keyCardId);
        if (card == null)
        {
            _logger.LogWarning("Card {CardId} not found for sync", keyCardId);
            return;
        }

        try
        {
            // Queue for background processing
            await QueueSyncAsync(keyCardId, card.CardNumber, activate, null);
            
            _logger.LogInformation("Queued card {CardNumber} for {Action}", 
                card.CardNumber, activate ? "ACTIVATE" : "DEACTIVATE");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to queue card {CardNumber} for sync", card.CardNumber);
            await QueueSyncAsync(keyCardId, card.CardNumber, activate, ex.Message);
        }
    }

    private async Task QueueSyncAsync(int keyCardId, string cardNumber, bool activate, string? error)
    {
        await _syncQueueRepository.AddAsync(new ControllerSyncQueueItem
        {
            KeyCardId = keyCardId,
            CardNumber = cardNumber,
            Action = activate ? "ACTIVATE" : "DEACTIVATE",
            QueuedDate = DateTime.Now,
            LastError = error,
            Status = "PENDING",
            AttemptCount = 0
        });
    }

    /// <summary>
    /// Get members at risk (unpaid, within grace period)
    /// </summary>
    public async Task<List<MemberAtRiskDto>> GetMembersAtRiskAsync(int year, CancellationToken ct = default)
    {
        // Get settings for grace period logic
        var settings = _settingsRepository.GetSettingsForYear(year);
        if (settings == null || settings.GraceEndDate < DateTime.Today)
        {
            return new List<MemberAtRiskDto>();
        }

        // Get all active cards
        var allCards = await Task.Run(() => _keyCardRepository.GetAll(), ct);
        var activeCards = allCards.Where(c => c.IsActive).ToDictionary(c => c.MemberId);

        // Get unpaid dues
        var dues = await Task.Run(() => _duesRepository.GetDuesForYear(year), ct);
        var unpaidDues = dues.Where(d => d.PaidDate == null && (d.Amount ?? 0) > 0).ToList();

        var riskList = new List<MemberAtRiskDto>();
        foreach (var d in unpaidDues)
        {
            if (activeCards.TryGetValue(d.MemberID, out var card))
            {
                var member = await Task.Run(() => _memberRepository.GetMemberById(d.MemberID), ct);
                if (member == null || member.Status == "LIFE") continue; // Life members exempt

                riskList.Add(new MemberAtRiskDto
                {
                    MemberId = d.MemberID,
                    FullName = $"{member.FirstName} {member.LastName}",
                    CardNumber = card.CardNumber,
                    GraceEndDate = settings.GraceEndDate,
                    DaysRemaining = (settings.GraceEndDate.Value - DateTime.Today).Days
                });
            }
        }
        
        return riskList;
    }
}

/// <summary>
/// DTO for members at risk of losing card access
/// </summary>
public class MemberAtRiskDto
{
    public int MemberId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public DateTime? GraceEndDate { get; set; }
    public int DaysRemaining { get; set; }
}
