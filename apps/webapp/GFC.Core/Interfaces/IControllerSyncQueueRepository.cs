using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Repository for managing controller sync queue operations
/// </summary>
public interface IControllerSyncQueueRepository
{
    /// <summary>
    /// Add a new sync item to the queue
    /// </summary>
    Task<int> AddAsync(ControllerSyncQueueItem item);
    
    /// <summary>
    /// Get all pending sync items
    /// </summary>
    Task<List<ControllerSyncQueueItem>> GetPendingItemsAsync();
    
    /// <summary>
    /// Get count of pending syncs
    /// </summary>
    Task<int> GetPendingCountAsync();
    
    /// <summary>
    /// Update sync item status
    /// </summary>
    Task UpdateStatusAsync(int queueId, string status);
    
    /// <summary>
    /// Mark sync item as completed
    /// </summary>
    Task MarkAsCompletedAsync(int queueId);
    
    /// <summary>
    /// Increment attempt count and update error
    /// </summary>
    Task IncrementAttemptAsync(int queueId, string error);
    
    /// <summary>
    /// Get sync items by status
    /// </summary>
    Task<List<ControllerSyncQueueItem>> GetByStatusAsync(string status);
    
    /// <summary>
    /// Get sync item by ID
    /// </summary>
    Task<ControllerSyncQueueItem?> GetByIdAsync(int queueId);
    
    /// <summary>
    /// Delete completed items older than specified days
    /// </summary>
    Task DeleteCompletedOlderThanAsync(int days);

    /// <summary>
    /// Get the timestamp of the last successful sync
    /// </summary>
    Task<DateTime?> GetLastCompletedTimeAsync();

    /// <summary>
    /// Reset all pending or processing items to allow immediate retry
    /// </summary>
    Task ResetPendingAsync();
}
