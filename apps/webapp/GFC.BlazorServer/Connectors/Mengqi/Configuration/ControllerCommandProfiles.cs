using GFC.BlazorServer.Connectors.Mengqi.Packets;

namespace GFC.BlazorServer.Connectors.Mengqi.Configuration;

/// <summary>
///     Holds the set of WG3000 command profiles that the client relies on. Populated from the decompiled WG3000_COMM data.
/// </summary>
public sealed class ControllerCommandProfiles
{
    public WgCommandProfile OpenDoor { get; set; } = WgCommandProfile.Unconfigured(nameof(OpenDoor));

    public WgCommandProfile SyncTime { get; set; } = WgCommandProfile.Unconfigured(nameof(SyncTime));

    public WgCommandProfile AddOrUpdateCard { get; set; } = WgCommandProfile.Unconfigured(nameof(AddOrUpdateCard));

    public WgCommandProfile DeleteCard { get; set; } = WgCommandProfile.Unconfigured(nameof(DeleteCard));

    public WgCommandProfile BulkUpload { get; set; } = WgCommandProfile.Unconfigured(nameof(BulkUpload));

    public WgCommandProfile ClearAllCards { get; set; } = WgCommandProfile.Unconfigured(nameof(ClearAllCards));

    public WgCommandProfile GetEvents { get; set; } = WgCommandProfile.Unconfigured(nameof(GetEvents));

    public WgCommandProfile GetRunStatus { get; set; } = WgCommandProfile.Unconfigured(nameof(GetRunStatus));

    public WgCommandProfile ReadFlash { get; set; } = WgCommandProfile.Unconfigured(nameof(ReadFlash));

    public WgCommandProfile WriteFlash { get; set; } = WgCommandProfile.Unconfigured(nameof(WriteFlash));

    public WgCommandProfile ReadTimeSchedules { get; set; } = WgCommandProfile.Unconfigured(nameof(ReadTimeSchedules));

    public WgCommandProfile WriteTimeSchedules { get; set; } = WgCommandProfile.Unconfigured(nameof(WriteTimeSchedules));

    public WgCommandProfile ReadExtendedConfig { get; set; } = WgCommandProfile.Unconfigured(nameof(ReadExtendedConfig));

    public WgCommandProfile WriteExtendedDoorConfig { get; set; } = WgCommandProfile.Unconfigured(nameof(WriteExtendedDoorConfig));

    public WgCommandProfile GetNetworkConfig { get; set; } = WgCommandProfile.Unconfigured(nameof(GetNetworkConfig));

    public WgCommandProfile SetNetworkConfig { get; set; } = WgCommandProfile.Unconfigured(nameof(SetNetworkConfig));

    public WgCommandProfile SetAllowedPc { get; set; } = WgCommandProfile.Unconfigured(nameof(SetAllowedPc));

    public WgCommandProfile Reboot { get; set; } = WgCommandProfile.Unconfigured(nameof(Reboot));
    public WgCommandProfile Search { get; set; } = WgCommandProfile.Unconfigured(nameof(Search));
    public WgCommandProfile SetDoorConfig { get; set; } = WgCommandProfile.Unconfigured(nameof(SetDoorConfig));
    public WgCommandProfile AckEvents { get; set; } = WgCommandProfile.Unconfigured(nameof(AckEvents));
    
    public WgCommandProfile ResetPrivileges { get; set; } = WgCommandProfile.Unconfigured(nameof(ResetPrivileges));
    public WgCommandProfile ResetPrivilegeIndex { get; set; } = WgCommandProfile.Unconfigured(nameof(ResetPrivilegeIndex));

    public void EnsureAllConfigured()
    {
        OpenDoor.EnsureConfigured();
        SyncTime.EnsureConfigured();
        AddOrUpdateCard.EnsureConfigured();
        DeleteCard.EnsureConfigured();
        BulkUpload.EnsureConfigured();
        ClearAllCards.EnsureConfigured();
        GetEvents.EnsureConfigured();
        GetRunStatus.EnsureConfigured();
        ReadFlash.EnsureConfigured();
        WriteFlash.EnsureConfigured();
        // Skip unconfigured check for optional ones handled by Try/Catch or logic
        // ReadTimeSchedules.EnsureConfigured();
        // WriteTimeSchedules.EnsureConfigured();
        ReadExtendedConfig.EnsureConfigured();
        WriteExtendedDoorConfig.EnsureConfigured();
        GetNetworkConfig.EnsureConfigured();
        SetNetworkConfig.EnsureConfigured();
        // SetAllowedPc.EnsureConfigured();
        Reboot.EnsureConfigured();
        Search.EnsureConfigured();
        SetDoorConfig.EnsureConfigured();
        ResetPrivileges.EnsureConfigured();
        ResetPrivilegeIndex.EnsureConfigured();
    }
}


