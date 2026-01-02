namespace GFC.Core.Interfaces;

/// <summary>
/// Bridge interface to allow Core services to trigger immediate hardware sync
/// when the controller is online, falling back to the queue only if needed.
/// </summary>
public interface IImmediateSyncDispatcher
{
    /// <summary>
    /// Attempts to sync a card status immediately.
    /// Implementation should try direct communication and only queue if offline.
    /// </summary>
    Task DispatchSyncAsync(int keyCardId, bool activate, CancellationToken ct = default);
}
