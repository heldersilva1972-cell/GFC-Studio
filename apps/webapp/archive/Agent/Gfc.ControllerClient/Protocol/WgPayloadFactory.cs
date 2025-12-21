using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;
using Gfc.ControllerClient.Models;
using Gfc.ControllerClient.Packets;
using Gfc.ControllerClient.Utilities;

namespace Gfc.ControllerClient.Protocol;

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
        var payload = Allocate(profile, 40);
        BinaryPrimitives.WriteUInt32LittleEndian(payload, unchecked((uint)model.CardNumber));
        payload[4] = model.DoorMask != 0 ? model.DoorMask : BuildDoorMask(model.DoorList);

        var tzSpan = payload.AsSpan(5, 4);
        var timeZones = model.TimeZones;
        for (var i = 0; i < tzSpan.Length && i < timeZones.Count; i++)
        {
            tzSpan[i] = timeZones[i];
        }

        BinaryPrimitives.WriteUInt32LittleEndian(payload.AsSpan(9, 4), ToControllerDate(model.ValidFrom));
        BinaryPrimitives.WriteUInt32LittleEndian(payload.AsSpan(13, 4), ToControllerDate(model.ValidTo));
        BinaryPrimitives.WriteUInt16LittleEndian(payload.AsSpan(17, 2), (ushort)(markAsDeleted ? 0xFFFF : model.Flags));

        if (!string.IsNullOrWhiteSpace(model.HolderName))
        {
            var bytesWritten = Encoding.ASCII.GetBytes(model.HolderName!, payload.AsSpan(19));
            if (bytesWritten < payload.Length - 19)
            {
                payload[19 + bytesWritten] = 0;
            }
        }

        return payload;
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

