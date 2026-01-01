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

    public static byte[] BuildSyncTimePayload(WgCommandProfile profile, DateTime utcTime)
    {
        var payload = Allocate(profile, 8);
        var now = utcTime.ToLocalTime();
        
        // As per user verified structure:
        // Offset 0: Century prefix (0x20)
        // Offset 1: Year (BCD)
        // Offset 2: Month (BCD)
        // Offset 3: Day (BCD)
        // Offset 4: Hour (BCD)
        // Offset 5: Minute (BCD)
        // Offset 6: Second (BCD)
        // Offset 7: 0x00
        
        Guard.AgainstOutOfRange(now.Month, 1, 12, "Month");
        Guard.AgainstOutOfRange(now.Day, 1, 31, "Day");
        Guard.AgainstOutOfRange(now.Hour, 0, 23, "Hour");
        Guard.AgainstOutOfRange(now.Minute, 0, 59, "Minute");
        Guard.AgainstOutOfRange(now.Second, 0, 59, "Second");

        payload[0] = ToBcd(now.Year / 100); // Century (e.g. 20)
        payload[1] = ToBcd(now.Year % 100); // Year (e.g. 25)
        payload[2] = ToBcd(now.Month);
        payload[3] = ToBcd(now.Day);
        payload[4] = ToBcd(now.Hour);
        payload[5] = ToBcd(now.Minute);
        payload[6] = ToBcd(now.Second);
        payload[7] = 0x00;
        
        return payload;
    }

    public static byte[] BuildPrivilegePayload(WgCommandProfile profile, CardPrivilegeModel model, bool markAsDeleted)
    {
        // For N3000 "Add/Modify Privilege" (0x50) and "Delete" (0x52)
        // User-verified structure for N3000:
        // Offset 0-3: Card Number (4 bytes, Little Endian)
        // Offset 4-7: Start Date (4 bytes: Century, Year, Month, Day)
        // Offset 8-11: End Date (4 bytes: Century, Year, Month, Day)
        // Offset 12-15: Door Control (4 bytes: 1=Allowed, 0=Denied per door)

        var payload = Allocate(profile, 56);
        var span = payload.AsSpan();

        // 1. Card Number (4 bytes)
        BinaryPrimitives.WriteUInt32LittleEndian(span[0..4], unchecked((uint)model.CardNumber));

        if (profile.CommandCode == 82 || markAsDeleted)
        {
            // For Delete (0x52) or marked as deleted, we set dates and doors to 0
            // Some models might just ignore the rest, but 0 is safest.
            return payload;
        }

        // 2. Start Date (4 bytes: Cent, Year, Month, Day)
        WriteDate(span[4..8], model.ValidFrom ?? new DateTime(2025, 1, 1));

        // 3. End Date (4 bytes: Cent, Year, Month, Day)
        WriteDate(span[8..12], model.ValidTo ?? new DateTime(2029, 12, 31));

        // 4. Door Control (Offsets 12-15)
        // Even if DoorMask is provided, we prefer DoorList if available for clarity
        byte mask = model.DoorMask != 0 ? model.DoorMask : BuildDoorMask(model.DoorList);
        span[12] = (byte)((mask & 0x01) > 0 ? 1 : 0); // Door 1
        span[13] = (byte)((mask & 0x02) > 0 ? 1 : 0); // Door 2
        span[14] = (byte)((mask & 0x04) > 0 ? 1 : 0); // Door 3 (Disabled)
        span[15] = (byte)((mask & 0x08) > 0 ? 1 : 0); // Door 4 (Disabled)

        // The user's verified hex shows the rest of the 56-byte payload (Packet Offset 24-63) is 0.
        // Allocate(profile, 56) already zero-initializes.

        return payload;
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

    private static byte[] Allocate(WgCommandProfile profile, int minimumLength)
    {
        var length = profile.RequestPayloadLength > 0 ? profile.RequestPayloadLength : minimumLength;
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
