using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using Gfc.ControllerClient.Models;
using Gfc.ControllerClient.Packets;

namespace Gfc.ControllerClient.Protocol;

internal static class WgResponseParser
{
    public static ReadOnlySpan<byte> GetPayload(ReadOnlySpan<byte> packet)
        => packet.Length <= WgPacketBuilder.HeaderLength ? ReadOnlySpan<byte>.Empty : packet[WgPacketBuilder.HeaderLength..];

    public static void EnsureAck(ReadOnlySpan<byte> packet, WgCommandProfile profile)
    {
        if (packet.Length < 2)
        {
            throw new InvalidOperationException("Controller response is too short.");
        }

        if (packet[0] != profile.PacketType || packet[1] != profile.CommandCode)
        {
            throw new InvalidOperationException($"Controller response did not match command {profile.Name}.");
        }
    }

    public static RunStatusModel ParseRunStatus(ReadOnlySpan<byte> packet)
    {
        var payload = GetPayload(packet);
        var doors = new List<RunStatusModel.DoorStatus>(4);
        if (payload.Length >= 2)
        {
            for (var i = 0; i < 4; i++)
            {
                doors.Add(new RunStatusModel.DoorStatus
                {
                    DoorNumber = i + 1,
                    IsDoorOpen = (payload[0] & (1 << i)) != 0,
                    IsRelayOn = (payload[1] & (1 << i)) != 0,
                    IsSensorActive = (payload.Length > 2 && (payload[2] & (1 << i)) != 0)
                });
            }
        }

        return new RunStatusModel
        {
            Doors = doors,
            RelayStates = Array.Empty<bool>(),
            IsFireAlarmActive = payload.Length > 3 && (payload[3] & 0x01) != 0,
            IsTamperActive = payload.Length > 3 && (payload[3] & 0x02) != 0
        };
    }

    public static (IReadOnlyList<ControllerEvent> Events, uint LastIndex) ParseEvents(ReadOnlySpan<byte> packet)
    {
        var payload = GetPayload(packet);
        var events = new List<ControllerEvent>();
        const int recordLength = 16;
        for (var offset = 0; offset + recordLength <= payload.Length; offset += recordLength)
        {
            var slice = payload.Slice(offset, recordLength);
            var card = BinaryPrimitives.ReadUInt32LittleEndian(slice);
            var door = slice[4];
            var eventType = (ControllerEventType)slice[5];
            var reason = BinaryPrimitives.ReadUInt16LittleEndian(slice[6..]);
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(BinaryPrimitives.ReadUInt32LittleEndian(slice[8..])).UtcDateTime;

            events.Add(new ControllerEvent
            {
                CardNumber = card,
                DoorOrReader = door,
                EventType = eventType,
                ReasonCode = reason,
                TimestampUtc = timestamp
            });
        }

        var lastIndex = payload.Length >= 4
            ? BinaryPrimitives.ReadUInt32LittleEndian(payload[^4..])
            : 0;
        return (events, lastIndex);
    }
}

