using GFC.Core.Interfaces;
using GFC.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GFC.Core.Services
{
    public class TemporaryCardLifecycleService
    {
        private readonly ITemporaryCardRepository _tempRepo;
        private readonly IImmediateSyncDispatcher _syncDispatcher;
        private readonly ICardDeactivationLogRepository _logRepo;

        public TemporaryCardLifecycleService(
            ITemporaryCardRepository tempRepo,
            IImmediateSyncDispatcher syncDispatcher,
            ICardDeactivationLogRepository logRepo)
        {
            _tempRepo = tempRepo;
            _syncDispatcher = syncDispatcher;
            _logRepo = logRepo;
        }

        public async Task ActivateCardAsync(int id, string? performedBy = null, CancellationToken ct = default)
        {
            var card = await _tempRepo.GetByIdAsync(id);
            if (card == null || string.IsNullOrWhiteSpace(card.CardNumber)) return;

            card.IsActive = true;
            await _tempRepo.UpdateAsync(card);

            // Sync to door
            await _syncDispatcher.DispatchTempCardSyncAsync(card.CardNumber, activate: true, ct);

            // Log activity
            await _logRepo.AddAsync(new CardDeactivationLog
            {
                KeyCardId = null,
                MemberId = null,
                DeactivatedDate = DateTime.Now,
                Reason = "Activated",
                ControllerSynced = true,
                SyncedDate = DateTime.Now,
                Notes = $"Temporary card activated for {card.HolderName} (Purpose: {card.Purpose})",
                PerformedBy = performedBy ?? "System"
            });
        }

        public async Task DeactivateCardAsync(int id, string? performedBy = null, CancellationToken ct = default)
        {
            var card = await _tempRepo.GetByIdAsync(id);
            if (card == null) return;

            card.IsActive = false;
            await _tempRepo.UpdateAsync(card);

            if (!string.IsNullOrWhiteSpace(card.CardNumber))
            {
                // Sync to door
                await _syncDispatcher.DispatchTempCardSyncAsync(card.CardNumber, activate: false, ct);

                // Log activity
                await _logRepo.AddAsync(new CardDeactivationLog
                {
                    KeyCardId = null,
                    MemberId = null,
                    DeactivatedDate = DateTime.Now,
                    Reason = "Deactivated",
                    ControllerSynced = true,
                    SyncedDate = DateTime.Now,
                    Notes = $"Temporary card deactivated for {card.HolderName}",
                    PerformedBy = performedBy ?? "System"
                });
            }
        }

        public async Task ReplaceCardAsync(int id, string newCardNumber, string? performedBy = null, CancellationToken ct = default)
        {
            var card = await _tempRepo.GetByIdAsync(id);
            if (card == null) return;

            var oldCardNumber = card.CardNumber;

            // Update card number
            card.CardNumber = newCardNumber;
            await _tempRepo.UpdateAsync(card);

            // If active, sync old as deactivate, new as activate
            if (card.IsActive)
            {
                if (!string.IsNullOrWhiteSpace(oldCardNumber))
                {
                    await _syncDispatcher.DispatchTempCardSyncAsync(oldCardNumber, activate: false, ct);
                }
                await _syncDispatcher.DispatchTempCardSyncAsync(newCardNumber, activate: true, ct);
            }

            // Log activity
            await _logRepo.AddAsync(new CardDeactivationLog
            {
                KeyCardId = null,
                MemberId = null,
                DeactivatedDate = DateTime.Now,
                Reason = "Reassigned",
                ControllerSynced = true,
                SyncedDate = DateTime.Now,
                Notes = $"Temporary card replaced for {card.HolderName}. Old: {oldCardNumber}, New: {newCardNumber}",
                PerformedBy = performedBy ?? "System"
            });
        }

        public async Task UnassignCardAsync(int id, string? performedBy = null, CancellationToken ct = default)
        {
            var card = await _tempRepo.GetByIdAsync(id);
            if (card == null) return;

            var oldCardNumber = card.CardNumber;

            // Deactivate and clear card number
            card.IsActive = false;
            card.CardNumber = null;
            await _tempRepo.UpdateAsync(card);

            if (!string.IsNullOrWhiteSpace(oldCardNumber))
            {
                await _syncDispatcher.DispatchTempCardSyncAsync(oldCardNumber, activate: false, ct);

                await _logRepo.AddAsync(new CardDeactivationLog
                {
                    KeyCardId = null,
                    MemberId = null,
                    DeactivatedDate = DateTime.Now,
                    Reason = "Unassigned",
                    ControllerSynced = true,
                    SyncedDate = DateTime.Now,
                    Notes = $"Card #{oldCardNumber} was unassigned/removed from {card.HolderName}",
                    PerformedBy = performedBy ?? "System"
                });
            }
        }

        public async Task CheckExpirationsAsync(CancellationToken ct = default)
        {
            var all = await _tempRepo.GetAllAsync();
            var now = DateTime.Now;
            foreach (var card in all)
            {
                if (card.IsActive && now >= card.ActiveTo)
                {
                    await DeactivateCardAsync(card.TemporaryCardId, "System (Expired)", ct);
                }
            }
        }
    }
}
