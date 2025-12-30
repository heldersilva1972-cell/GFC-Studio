namespace GFC.Core.Models;

/// <summary>
/// Represents an item in the controller sync queue
/// </summary>
public class ControllerSyncQueueItem
{
    public int QueueId { get; set; }
    public int KeyCardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // "ACTIVATE" or "DEACTIVATE"
    public DateTime QueuedDate { get; set; }
    public int AttemptCount { get; set; }
    public DateTime? LastAttemptDate { get; set; }
    public string? LastError { get; set; }
    public string Status { get; set; } = "PENDING"; // "PENDING", "PROCESSING", "COMPLETED"
    public DateTime? CompletedDate { get; set; }
}
