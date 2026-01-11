using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;
using GFC.BlazorServer.Connectors.Mengqi.Models;
using GFC.BlazorServer.Connectors.Mengqi.Packets;
using GFC.BlazorServer.Connectors.Mengqi.Utilities;

namespace GFC.BlazorServer.Connectors.Mengqi.Protocol;

internal static class WgPayloadFactory
{
    public static byte[] BuildOpenDoorPayload(WgCommandProfile profile, int doorNo, int? durationSeconds)
    {
        Guard.AgainstOutOfRange(doorNo, 1, 4, nameof(doorNo));
        var payload = Allocate(profile, 3);
        payload[0] = (byte)doorNo;
        BinaryPrimitives.WriteUInt16LittleEndian(payload.AsSpan(1), (ushort)(durationSeconds ?? 0));
        return payload;
    }

    public static byte[] BuildClearAllCardsPayload(WgCommandProfile profile)
    {
        // Safety Password: The sequence 55 AA 55 AA starting at Byte 8 (Payload Offset 0) is mandatory.
        var payload = Allocate(profile, 56);
        payload[0] = 0x55;
        payload[1] = 0xAA;
        payload[2] = 0x55;
        payload[3] = 0xAA;
        return payload;
    }

    public static byte[] BuildSyncTimePayload(WgCommandProfile profile, DateTime localTime)
    {
        // Controller expects data at packet offset 20 for this firmware.
        // Standard start is offset 8. So we add 12 bytes padding.
        var payload = Allocate(profile, 20); 
        var now = localTime;
        
        // Offset 0-11: Padding (0x00)
        // Offset 12: Century
        // Offset 13: Year
        // ...
        
        Guard.AgainstOutOfRange(now.Month, 1, 12, "Month");
        Guard.AgainstOutOfRange(now.Day, 1, 31, "Day");
        Guard.AgainstOutOfRange(now.Hour, 0, 23, "Hour");
        Guard.AgainstOutOfRange(now.Minute, 0, 59, "Minute");
        Guard.AgainstOutOfRange(now.Second, 0, 59, "Second");

        int baseOffset = 12;
        payload[baseOffset + 0] = ToBcd(now.Year / 100);
        payload[baseOffset + 1] = ToBcd(now.Year % 100);
        payload[baseOffset + 2] = ToBcd(now.Month);
        payload[baseOffset + 3] = ToBcd(now.Day);
        payload[baseOffset + 4] = ToBcd(now.Hour);
        payload[baseOffset + 5] = ToBcd(now.Minute);
        payload[baseOffset + 6] = ToBcd(now.Second);
        payload[baseOffset + 7] = 0x00;
        
        return payload;
    }

    public static byte[] BuildPrivilegePayload(WgCommandProfile profile, CardPrivilegeModel model, bool markAsDeleted)
    {
        // For N3000 "Add/Modify Privilege" (0x50) and "Delete" (0x52)
        // Explicit Mapping (Payload starts at Packet Offset 8):
        // Offset 0-3:   Card Number (Packet 8-11)
        // Offset 4-7:   Start Date (Packet 12-15) - BCD Format
        // Offset 8-11:  Expiry Date (Packet 16-19) - BCD Format
        // Offset 12:    Door 1 (Packet 20) - 0x01 Allowed, 0x00 Denied
        // Offset 13:    Door 2 (Packet 21)
        // Offset 14:    Door 3 (Packet 22)
        // Offset 15:    Door 4 (Packet 23)
        // Offset 16-19: Timezone Index (Packet 24-27) - 0x01 for "Always"
 
        var payload = Allocate(profile, 55); // Use full payload space
        var span = payload.AsSpan();
 
        // 1. Card Number (4 bytes as per specification)
        BinaryPrimitives.WriteUInt32LittleEndian(span[0..4], unchecked((uint)model.CardNumber));
 
        if (profile.CommandCode == 82 || markAsDeleted)
        {
            // For Delete (0x52) or marked as deleted, we send Card ID with no access
            return payload;
        }
 
        // 2. Dates (BCD Format: CC YY MM DD)
        WriteBcdDate(span[4..8], model.ValidFrom ?? new DateTime(2025, 1, 1));
        WriteBcdDate(span[8..12], model.ValidTo ?? new DateTime(2099, 12, 31));
 
        // 3. Door Access (Offsets 12-15)
        // Each byte corresponds to a door. 0x01 = Access, 0x00 = Denied.
        for (int i = 0; i < 4; i++)
        {
            span[12 + i] = model.DoorList.Contains(i + 1) ? (byte)0x01 : (byte)0x00;
        }
 
        // 4. Timezone Indices (Offsets 16-19)
        // Controller expects one timezone index per door.
        // Index 1 usually means "Always Allowed".
        for (int i = 0; i < 4; i++)
        {
            byte tz = 1;
            if (model.TimeZones != null && model.TimeZones.Count > i && model.TimeZones[i] > 0)
            {
                tz = model.TimeZones[i];
            }
            else if (model.TimeProfileIndex.HasValue && model.TimeProfileIndex.Value > 0)
            {
                tz = (byte)model.TimeProfileIndex.Value;
            }
            span[16 + i] = tz;
        }
 
        return payload;
    }
 
    private static void WriteBcdDate(Span<byte> span, DateTime date)
    {
        // Format: Cent, Year, Month, Day (e.g., 20 26 01 01)
        span[0] = ToBcd(date.Year / 100);
        span[1] = ToBcd(date.Year % 100);
        span[2] = ToBcd(date.Month);
        span[3] = ToBcd(date.Day);
    }

    private static void WriteDate(Span<byte> span, DateTime date)
    {
        // Format: Cent, Year, Month, Day (e.g., 20 25 01 01)
        // We use BCD-like byte values as requested
        span[0] = (byte)ToBcd(date.Year / 100);
        span[1] = (byte)ToBcd(date.Year % 100);
        span[2] = (byte)ToBcd(date.Month);
        span[3] = (byte)ToBcd(date.Day);
    }

    public static byte ToBcd(int value)
    {
        // Example: 31 becomes 0x31 (Binary: 0011 0001)
        // Matches byte.Parse(dt.ToString("yy"), NumberStyles.AllowHexSpecifier) logic requested by user
        if (value > 99) throw new ArgumentOutOfRangeException(nameof(value), "BCD value cannot exceed 99");
        return (byte)((value / 10 << 4) | (value % 10));
    }

    public static byte[] BuildFlashReadPayload(WgCommandProfile profile, FlashArea area, int start, int length)
    {
        var payload = Allocate(profile, 12);
        payload[0] = (byte)area;
        BinaryPrimitives.WriteInt32LittleEndian(payload.AsSpan(4, 4), start);
        BinaryPrimitives.WriteInt32LittleEndian(payload.AsSpan(8, 4), length);
        return payload;
    }

    public static byte[] BuildFlashWritePayload(WgCommandProfile profile, FlashArea area, int start, ReadOnlySpan<byte> data)
    {
        var payload = Allocate(profile, 12 + data.Length);
        payload[0] = (byte)area;
        BinaryPrimitives.WriteInt32LittleEndian(payload.AsSpan(4, 4), start);
        BinaryPrimitives.WriteInt32LittleEndian(payload.AsSpan(8, 4), data.Length);
        data.CopyTo(payload.AsSpan(12));
        return payload;
    }

    public static byte[] BuildDoorConfigPayload(WgCommandProfile profile, int doorIndex, byte controlMode, byte relayDelay, byte doorSensor, byte interlock)
    {
        // 0x8E command uses a specific layout within the 64-byte frame (offset 8 of packet is offset 0 of this payload)
        var payload = Allocate(profile, 56);
        payload[0] = (byte)doorIndex;  // Packet Offset 8
        payload[1] = controlMode;      // Packet Offset 9: 1=AlwaysOpen, 2=AlwaysClosed, 3=Controlled
        payload[2] = relayDelay;       // Packet Offset 10: seconds
        payload[3] = doorSensor;       // Packet Offset 11: 0=None, 1=NC, 2=NO
        
        // Interlock is at Packet Offset 14, which is Payload Offset 6
        payload[6] = interlock;        // Packet Offset 14: 0=None, 1=2Door, 2=3Door, 3=4Door
        
        return payload;
    }

    public static byte[] BuildGetDoorParamsPayload(WgCommandProfile profile, int doorIndex)
    {
        // 0x5A Request: 
        // Byte 8: Door Index (Payload[0])
        // Bytes 9-12: Safety Password (55 AA 55 AA) (Payload[1-4])
        // Validated by USER: "Put the Password 55 AA 55 AA at Bytes 9, 10, 11, and 12."
        
        var payload = Allocate(profile, 5); 
        payload[0] = (byte)doorIndex; 
        
        // Safety Password
        payload[1] = 0x55;
        payload[2] = 0xAA;
        payload[3] = 0x55;
        payload[4] = 0xAA;
        
        return payload;
    }

    private static byte[] Allocate(WgCommandProfile profile, int minimumLength)
    {
        // Hybrid SSI Standard: Header 16 + TailSum 1 = 17 bytes overhead.
        // Payload must NOT exceed 47 bytes in a 64-byte frame.
        const int MaxSsiPayload = 47; 
        
        var length = profile.RequestPayloadLength > 0 ? profile.RequestPayloadLength : minimumLength;
        length = Math.Min(length, MaxSsiPayload);
        
        return new byte[length];
    }

    private static byte BuildDoorMask(IReadOnlyCollection<int> doors)
    {
        byte mask = 0;
        foreach (var door in doors)
        {
            Guard.AgainstOutOfRange(door, 1, 4, nameof(doors));
            mask |= (byte)(1 << (door - 1));
        }

        return mask;
    }

    private static uint ToControllerDate(DateTime? date)
    {
        if (!date.HasValue)
        {
            return 0;
        }

        var local = date.Value.ToLocalTime();
        var y = (uint)local.Year;
        var m = (uint)local.Month;
        var d = (uint)local.Day;
        return (y << 16) | (m << 8) | d;
    }
}
