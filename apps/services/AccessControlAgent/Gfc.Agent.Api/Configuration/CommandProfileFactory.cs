using Gfc.ControllerClient.Configuration;
using Gfc.ControllerClient.Packets;

namespace Gfc.Agent.Api.Configuration;

internal static class CommandProfileFactory
{
    public static ControllerCommandProfiles CreateDefaults()
    {
        return new ControllerCommandProfiles
        {
            OpenDoor = WgCommandProfile.Create("OpenDoor", 23, 64, WgPacketFormat.Basic, 3, 0),
            SyncTime = WgCommandProfile.Create("SyncTime", 23, 48, WgPacketFormat.Basic, 8, 0),
            AddOrUpdateCard = WgCommandProfile.Create("AddOrUpdateCard", 36, 62, WgPacketFormat.Privilege, 40, 0),
            DeleteCard = WgCommandProfile.Create("DeleteCard", 36, 63, WgPacketFormat.Privilege, 40, 0),
            BulkUpload = WgCommandProfile.Create("BulkUpload", 36, 60, WgPacketFormat.Privilege, 40, 0),
            ClearAllCards = WgCommandProfile.Create("ClearAllCards", 36, 64, WgPacketFormat.Basic, 0, 0),
            GetEvents = WgCommandProfile.Create("GetEvents", 23, 176, WgPacketFormat.Basic, 4, 64),
            GetRunStatus = WgCommandProfile.Create("GetRunStatus", 23, 32, WgPacketFormat.Basic, 0, 32),
            ReadFlash = WgCommandProfile.Create("ReadFlash", 33, 16, WgPacketFormat.Basic, 12, 0),
            WriteFlash = WgCommandProfile.Create("WriteFlash", 33, 17, WgPacketFormat.Basic, 0, 0),
            ReadTimeSchedules = WgCommandProfile.Unconfigured(nameof(ControllerCommandProfiles.ReadTimeSchedules)),
            WriteTimeSchedules = WgCommandProfile.Unconfigured(nameof(ControllerCommandProfiles.WriteTimeSchedules)),
            ReadExtendedConfig = WgCommandProfile.Create("ReadExtendedConfig", 36, 24, WgPacketFormat.Basic, 0, 64),
            WriteExtendedDoorConfig = WgCommandProfile.Create("WriteExtendedDoorConfig", 36, 22, WgPacketFormat.Basic, 64, 0),
            GetNetworkConfig = WgCommandProfile.Create("GetNetworkConfig", 23, 92, WgPacketFormat.Basic, 0, 32),
            SetNetworkConfig = WgCommandProfile.Create("SetNetworkConfig", 23, 92, WgPacketFormat.Basic, 16, 0),
            SetAllowedPc = WgCommandProfile.Unconfigured(nameof(ControllerCommandProfiles.SetAllowedPc)),
            Reboot = WgCommandProfile.Create("Reboot", 23, 254, WgPacketFormat.Basic, 0, 0)
        };
    }
}

