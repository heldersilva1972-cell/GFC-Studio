using GFC.BlazorServer.Models;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for syncing member privileges to controllers via Agent API.
/// </summary>
public interface IMemberPrivilegeSyncService
{
    /// <summary>
    /// Computes effective access for a member, grouping by controller.
    /// </summary>
    Task<EffectiveAccessResult> ComputeEffectiveAccessAsync(int memberId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Syncs member privileges to all relevant controllers.
    /// </summary>
    Task<SyncResult> SyncMemberPrivilegesAsync(int memberId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Removes a card from all controllers.
    /// </summary>
    Task<SyncResult> RemoveCardFromControllersAsync(int memberId, string cardNumber, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of computing effective access for a member.
/// </summary>
public class EffectiveAccessResult
{
    public int MemberId { get; set; }
    public string? CardNumber { get; set; }
    public bool HasActiveCard { get; set; }
    public List<ControllerAccessGroup> Controllers { get; set; } = new();
}

/// <summary>
/// Access configuration for a single controller.
/// </summary>
public class ControllerAccessGroup
{
    public int ControllerId { get; set; }
    public string ControllerName { get; set; } = string.Empty;
    public uint SerialNumber { get; set; }
    public string SerialNumberDisplay { get; set; } = string.Empty;
    public List<DoorAccessConfig> Doors { get; set; } = new();
}

/// <summary>
/// Access configuration for a single door.
/// </summary>
public class DoorAccessConfig
{
    public int DoorId { get; set; }
    public int DoorIndex { get; set; }
    public string DoorName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public int? TimeProfileIndex { get; set; }
}

/// <summary>
/// Result of a sync operation.
/// </summary>
public class SyncResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public int ControllersProcessed { get; set; }
    public int ControllersSucceeded { get; set; }
}
