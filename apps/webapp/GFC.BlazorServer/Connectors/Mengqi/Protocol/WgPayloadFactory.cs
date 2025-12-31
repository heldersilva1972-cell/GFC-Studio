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
        var current = utcTime.ToLocalTime();
        payload[0] = (byte)(current.Year - 2000);
        payload[1] = (byte)current.Month;
        payload[2] = (byte)current.Day;
        payload[3] = (byte)current.Hour;
        payload[4] = (byte)current.Minute;
        payload[5] = (byte)current.Second;
        payload[6] = (byte)(((int)current.DayOfWeek + 6) % 7 + 1);
        payload[7] = 0; // reserved
        return payload;
    }

    public static byte[] BuildPrivilegePayload(WgCommandProfile profile, CardPrivilegeModel model, bool markAsDeleted)
    {
        // Handle Delete Card (0x52)
        if (profile.CommandCode == 82)
        {
            var deletePayload = Allocate(profile, 56);
            // Lower 32 bits of CardID at Offset 0
            BinaryPrimitives.WriteUInt32LittleEndian(deletePayload.AsSpan(0, 4), unchecked((uint)model.CardNumber));
            // Upper 32 bits of CardID at Offset 36 (Packet Offset 44)
            BinaryPrimitives.WriteUInt32LittleEndian(deletePayload.AsSpan(36, 4), (uint)(model.CardNumber >> 32));
            return deletePayload;
        }

        // For N3000 "Add Privilege" (0x50), the payload is 56 bytes (to fill the 64-byte frame from offset 8).
        // It's a special structure:
        // Offset 0-23: MjRegisterCard data (24 bytes)
        // Offset 24-31: Zero padding
        // Offset 32-35: XID (Handled by PacketBuilder, will be overwritten)
        // Offset 36-39: Zero padding
        // Offset 40-55: Duplicated MjRegisterCard data (partial, 16 bytes)

        // 1. Construct the 24-byte MjRegisterCard buffer first
        Span<byte> cardData = stackalloc byte[24];
        
        // 0-7: Card ID (8 bytes)
        BinaryPrimitives.WriteUInt32LittleEndian(cardData[0..], unchecked((uint)model.CardNumber));
        // Note: Upper 4 bytes of CardID are usually 0 for standard WG26/34. 
        // We write 0 explicitly to be safe, or assume model.CardNumber is long.
         BinaryPrimitives.WriteUInt32LittleEndian(cardData[4..], (uint)(model.CardNumber >> 32));

        // 8: Option
        // Active = 0xA0 (NotDeleted 0x80 | Reserved 0x20)
        // Deleted = 0x20
        byte option = markAsDeleted ? (byte)0x20 : (byte)0xA0;
        cardData[8] = option;

        // 9-11: Password (3 bytes) - Default 0
        cardData[9] = 0;
        cardData[10] = 0;
        cardData[11] = 0;

        // 12-13: Start Date (Compressed YMD)
        BinaryPrimitives.WriteUInt16LittleEndian(cardData[12..], ToCompressedDate(model.ValidFrom ?? DateTime.Today));

        // 14-15: End Date (Compressed YMD)
        BinaryPrimitives.WriteUInt16LittleEndian(cardData[14..], ToCompressedDate(model.ValidTo ?? DateTime.Today.AddYears(10)));
        
        // 16-19: Door Control (4 bytes)
        // model.DoorMask 0 means all doors? Or specific mask.
        // N3000 usually uses 1 byte per door index?
        // Actually MjRegisterCard uses 4 bytes for "ControlSegIndex" or similar.
        // But verify ps1: $cardStruct[16..19] = 1 (Allow).
        // If DoorMask is set, we need to map it.
        // Let's assume DoorMask bits map to doors 1-4.
        // If bit 0 is set, Door 1 is allowed (1). If not, 0.
        byte doorMask = model.DoorMask != 0 ? model.DoorMask : BuildDoorMask(model.DoorList);
        cardData[16] = (byte)((doorMask & 0x01) > 0 ? 1 : 0);
        cardData[17] = (byte)((doorMask & 0x02) > 0 ? 1 : 0);
        cardData[18] = (byte)((doorMask & 0x04) > 0 ? 1 : 0);
        cardData[19] = (byte)((doorMask & 0x08) > 0 ? 1 : 0);

        // 20-21: More Cards (0)
        cardData[20] = 0;
        cardData[21] = 0;

        // 22-23: Ext/MaxSwipe (0)
        cardData[22] = 0;
        cardData[23] = 0;

        // 2. Build the final payload
        var payload = Allocate(profile, 56);
        var pSpan = payload.AsSpan();

        // Copy generic 24 bytes to beginning (Offset 0 of payload)
        cardData.CopyTo(pSpan[0..]);

        // Copy partial 16 bytes (starting from Option at index 8) to offset 40
        // This duplication is required by the 0x50 command protocol.
        cardData.Slice(8, 16).CopyTo(pSpan[40..]);

        return payload;
    }

    private static ushort ToCompressedDate(DateTime date)
    {
        // Format: (Year-2000)<<9 | Month<<5 | Day
        var y = (date.Year >= 2000) ? date.Year - 2000 : 0;
        var m = date.Month;
        var d = date.Day;
        return (ushort)((y << 9) | (m << 5) | d);
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


