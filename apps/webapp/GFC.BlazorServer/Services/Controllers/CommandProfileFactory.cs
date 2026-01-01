using GFC.BlazorServer.Connectors.Mengqi.Configuration;
using GFC.BlazorServer.Connectors.Mengqi.Packets;

namespace GFC.BlazorServer.Services.Controllers;

internal static class CommandProfileFactory
{
    public static ControllerCommandProfiles CreateDefaults()
    {
        // N3000 series uses 0x17 (23) as the frame header (Type)
        // All packets are fixed at 64 bytes.
        return new ControllerCommandProfiles
        {
            OpenDoor = WgCommandProfile.Create("OpenDoor", 23, 1, WgPacketFormat.Basic, 4, 64), 
            SyncTime = WgCommandProfile.Create("SyncTime", 23, 48, WgPacketFormat.Basic, 8, 64),
            AddOrUpdateCard = WgCommandProfile.Create("AddOrUpdateCard", 23, 80, WgPacketFormat.Privilege, 56, 64),
            DeleteCard = WgCommandProfile.Create("DeleteCard", 23, 82, WgPacketFormat.Privilege, 56, 64),
            BulkUpload = WgCommandProfile.Create("BulkUpload", 23, 60, WgPacketFormat.Privilege, 40, 64),
            ClearAllCards = WgCommandProfile.Create("ClearAllCards", 23, 84, WgPacketFormat.Basic, 0, 64),
            GetEvents = WgCommandProfile.Create("GetEvents", 23, 176, WgPacketFormat.Basic, 4, 64),
            AckEvents = WgCommandProfile.Create("AckEvents", 23, 178, WgPacketFormat.Basic, 4, 64),
            GetRunStatus = WgCommandProfile.Create("GetRunStatus", 23, 32, WgPacketFormat.Basic, 0, 64),
            ReadFlash = WgCommandProfile.Create("ReadFlash", 23, 16, WgPacketFormat.Basic, 12, 64),
            WriteFlash = WgCommandProfile.Create("WriteFlash", 23, 17, WgPacketFormat.Basic, 0, 64),
            ReadTimeSchedules = WgCommandProfile.Unconfigured(nameof(ControllerCommandProfiles.ReadTimeSchedules)),
            WriteTimeSchedules = WgCommandProfile.Unconfigured(nameof(ControllerCommandProfiles.WriteTimeSchedules)),
            ReadExtendedConfig = WgCommandProfile.Create("ReadExtendedConfig", 23, 24, WgPacketFormat.Basic, 0, 64),
            WriteExtendedDoorConfig = WgCommandProfile.Create("WriteExtendedDoorConfig", 23, 22, WgPacketFormat.Basic, 64, 64),
            GetNetworkConfig = WgCommandProfile.Create("GetNetworkConfig", 23, 92, WgPacketFormat.Basic, 0, 64),
            SetNetworkConfig = WgCommandProfile.Create("SetNetworkConfig", 23, 92, WgPacketFormat.Basic, 16, 64),
            SetAllowedPc = WgCommandProfile.Unconfigured(nameof(ControllerCommandProfiles.SetAllowedPc)),
            Reboot = WgCommandProfile.Create("Reboot", 23, 254, WgPacketFormat.Basic, 0, 64),
            Search = WgCommandProfile.Create("Search", 23, 148, WgPacketFormat.Basic, 0, 64),
            SetDoorConfig = WgCommandProfile.Create("SetDoorConfig", 23, 142, WgPacketFormat.Basic, 64, 64),
            GetDoorParams = WgCommandProfile.Create("GetDoorParams", 23, 90, WgPacketFormat.Basic, 4, 64), // 90 = 0x5A
            ResetPrivileges = WgCommandProfile.Create("ResetPrivileges", 23, 16, WgPacketFormat.Basic, 0, 64),
            ResetPrivilegeIndex = WgCommandProfile.Create("ResetPrivilegeIndex", 23, 17, WgPacketFormat.Basic, 0, 64)
        };
    }
}
