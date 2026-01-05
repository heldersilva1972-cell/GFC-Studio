namespace GFC.Shared.Agent.Commands;

/// <summary>
/// Allowlisted command types that can be executed by the Agent.
/// This is the ONLY set of operations permitted - no arbitrary commands allowed.
/// </summary>
public enum AllowedCommandType
{
    // ========================================================================
    // SAFE OPERATIONS (No confirmation required)
    // ========================================================================
    
    /// <summary>Open a specific door</summary>
    OpenDoor,
    
    /// <summary>Get controller run status and configuration</summary>
    GetRunStatus,
    
    /// <summary>Get door events by index range</summary>
    GetEventsByIndex,
    
    /// <summary>Synchronize controller time with server</summary>
    SyncTime,
    
    /// <summary>Get controller network configuration</summary>
    GetNetworkConfig,
    
    /// <summary>Get door configuration</summary>
    GetDoorConfig,
    
    // ========================================================================
    // PRIVILEGE MANAGEMENT (Moderate risk)
    // ========================================================================
    
    /// <summary>Add a new card privilege</summary>
    PrivilegeAdd,
    
    /// <summary>Update existing card privilege</summary>
    PrivilegeUpdate,
    
    /// <summary>Delete card privilege</summary>
    PrivilegeDelete,
    
    /// <summary>Get privilege by card number</summary>
    PrivilegeGet,
    
    // ========================================================================
    // DANGEROUS OPERATIONS (Require explicit confirmation)
    // ========================================================================
    
    /// <summary>Upload multiple privileges at once (DANGEROUS)</summary>
    BulkUpload,
    
    /// <summary>Clear all card privileges from controller (DANGEROUS)</summary>
    ClearAllCards,
    
    /// <summary>Read flash memory (DANGEROUS - diagnostic only)</summary>
    ReadFlash,
    
    /// <summary>Write flash memory (DANGEROUS - can brick controller)</summary>
    WriteFlash,
    
    /// <summary>Read FRAM memory (DANGEROUS - diagnostic only)</summary>
    ReadFRAM,
    
    /// <summary>Write FRAM memory (DANGEROUS - can corrupt data)</summary>
    WriteFRAM,
    
    /// <summary>Change network configuration (DANGEROUS - can lose connectivity)</summary>
    SetNetworkConfig,
    
    /// <summary>Change door configuration (DANGEROUS)</summary>
    SetDoorConfig,
    
    /// <summary>Reboot controller (DANGEROUS - causes downtime)</summary>
    Reboot,
    
    /// <summary>Factory reset controller (DANGEROUS - loses all data)</summary>
    FactoryReset
}

/// <summary>
/// Extension methods for command type classification
/// </summary>
public static class AllowedCommandTypeExtensions
{
    /// <summary>
    /// Returns true if this command requires explicit user confirmation before execution
    /// </summary>
    public static bool RequiresConfirmation(this AllowedCommandType commandType)
    {
        return commandType switch
        {
            AllowedCommandType.BulkUpload => true,
            AllowedCommandType.ClearAllCards => true,
            AllowedCommandType.ReadFlash => true,
            AllowedCommandType.WriteFlash => true,
            AllowedCommandType.ReadFRAM => true,
            AllowedCommandType.WriteFRAM => true,
            AllowedCommandType.SetNetworkConfig => true,
            AllowedCommandType.SetDoorConfig => true,
            AllowedCommandType.Reboot => true,
            AllowedCommandType.FactoryReset => true,
            _ => false
        };
    }
    
    /// <summary>
    /// Returns the risk level of this command
    /// </summary>
    public static CommandRiskLevel GetRiskLevel(this AllowedCommandType commandType)
    {
        return commandType switch
        {
            AllowedCommandType.OpenDoor => CommandRiskLevel.Low,
            AllowedCommandType.GetRunStatus => CommandRiskLevel.Low,
            AllowedCommandType.GetEventsByIndex => CommandRiskLevel.Low,
            AllowedCommandType.SyncTime => CommandRiskLevel.Low,
            AllowedCommandType.GetNetworkConfig => CommandRiskLevel.Low,
            AllowedCommandType.GetDoorConfig => CommandRiskLevel.Low,
            AllowedCommandType.PrivilegeGet => CommandRiskLevel.Low,
            
            AllowedCommandType.PrivilegeAdd => CommandRiskLevel.Medium,
            AllowedCommandType.PrivilegeUpdate => CommandRiskLevel.Medium,
            AllowedCommandType.PrivilegeDelete => CommandRiskLevel.Medium,
            
            AllowedCommandType.BulkUpload => CommandRiskLevel.High,
            AllowedCommandType.ClearAllCards => CommandRiskLevel.Critical,
            AllowedCommandType.ReadFlash => CommandRiskLevel.High,
            AllowedCommandType.WriteFlash => CommandRiskLevel.Critical,
            AllowedCommandType.ReadFRAM => CommandRiskLevel.High,
            AllowedCommandType.WriteFRAM => CommandRiskLevel.Critical,
            AllowedCommandType.SetNetworkConfig => CommandRiskLevel.Critical,
            AllowedCommandType.SetDoorConfig => CommandRiskLevel.High,
            AllowedCommandType.Reboot => CommandRiskLevel.High,
            AllowedCommandType.FactoryReset => CommandRiskLevel.Critical,
            
            _ => CommandRiskLevel.Unknown
        };
    }
    
    /// <summary>
    /// Returns a human-readable description of what this command does
    /// </summary>
    public static string GetDescription(this AllowedCommandType commandType)
    {
        return commandType switch
        {
            AllowedCommandType.OpenDoor => "Remotely unlock a door for one-time access",
            AllowedCommandType.GetRunStatus => "Retrieve controller status and configuration",
            AllowedCommandType.GetEventsByIndex => "Fetch door access events from controller log",
            AllowedCommandType.SyncTime => "Synchronize controller clock with server time",
            AllowedCommandType.GetNetworkConfig => "Retrieve network settings from controller",
            AllowedCommandType.GetDoorConfig => "Retrieve door configuration settings",
            AllowedCommandType.PrivilegeAdd => "Add a new access card to the controller",
            AllowedCommandType.PrivilegeUpdate => "Update an existing access card's permissions",
            AllowedCommandType.PrivilegeDelete => "Remove an access card from the controller",
            AllowedCommandType.PrivilegeGet => "Retrieve access card details",
            AllowedCommandType.BulkUpload => "Upload multiple access cards at once (DANGEROUS: can overload controller)",
            AllowedCommandType.ClearAllCards => "Delete ALL access cards from controller (DANGEROUS: cannot be undone)",
            AllowedCommandType.ReadFlash => "Read controller flash memory (DANGEROUS: diagnostic use only)",
            AllowedCommandType.WriteFlash => "Write controller flash memory (DANGEROUS: can brick device)",
            AllowedCommandType.ReadFRAM => "Read controller FRAM memory (DANGEROUS: diagnostic use only)",
            AllowedCommandType.WriteFRAM => "Write controller FRAM memory (DANGEROUS: can corrupt data)",
            AllowedCommandType.SetNetworkConfig => "Change controller network settings (DANGEROUS: can lose connectivity)",
            AllowedCommandType.SetDoorConfig => "Change door behavior settings (DANGEROUS: affects security)",
            AllowedCommandType.Reboot => "Restart the controller (DANGEROUS: causes temporary downtime)",
            AllowedCommandType.FactoryReset => "Reset controller to factory defaults (DANGEROUS: loses ALL data)",
            _ => "Unknown command"
        };
    }
}

/// <summary>
/// Risk level classification for commands
/// </summary>
public enum CommandRiskLevel
{
    Unknown,
    Low,
    Medium,
    High,
    Critical
}
