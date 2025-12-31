using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using GFC.BlazorServer.Connectors.Mengqi.Models;
using GFC.BlazorServer.Connectors.Mengqi.Packets;

namespace GFC.BlazorServer.Connectors.Mengqi.Protocol;

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

        // Allow 0x17 (Request/Response) or 0x18 (Some N3000 responses)
        if (packet[0] != profile.PacketType && packet[0] != 0x18)
        {
            throw new InvalidOperationException($"Controller response type {packet[0]:X2} did not match expected {profile.PacketType:X2} for command {profile.Name}.");
        }
        
        if (packet[1] != profile.CommandCode)
        {
            throw new InvalidOperationException($"Controller response function {packet[1]:X2} did not match expected {profile.CommandCode:X2} for command {profile.Name}.");
        }
    }

    public static RunStatusModel ParseRunStatus(ReadOnlySpan<byte> packet)
    {
        var payload = GetPayload(packet);
        var doors = new List<RunStatusModel.DoorStatus>(4);
        
        // N3000 response: [Type][Cmd][CRC][SN][DATA...]
        // payload starts at frame offset 4 (at the SN)
        // Actual status data starts at payload offset 4 (frame offset 8)
        if (payload.Length >= 8)
        {
            var data = payload[4..];
            for (var i = 0; i < 4; i++)
            {
                doors.Add(new RunStatusModel.DoorStatus
                {
                    DoorNumber = i + 1,
                    IsDoorOpen = (data[0] & (1 << i)) != 0,
                    IsRelayOn = (data[1] & (1 << i)) != 0,
                    IsSensorActive = (data.Length > 2 && (data[2] & (1 << i)) != 0)
                });
            }
        }

        return new RunStatusModel
        {
            Doors = doors,
            RelayStates = Array.Empty<bool>(),
            IsFireAlarmActive = payload.Length >= 8 && (payload[7] & 0x01) != 0,
            IsTamperActive = payload.Length >= 8 && (payload[7] & 0x02) != 0
        };
    }

    public static (IReadOnlyList<ControllerEvent> Events, uint LastIndex) ParseEvents(ReadOnlySpan<byte> packet)
    {
        var payload = GetPayload(packet);
        var events = new List<ControllerEvent>();
        const int recordLength = 16;
        
        // Skip 4-byte SN at start of payload
        if (payload.Length < 4) return (events, 0);
        var data = payload[4..];

        for (var offset = 0; offset + recordLength <= data.Length; offset += recordLength)
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

    public static DiscoveryResult ParseDiscovery(ReadOnlySpan<byte> packet)
    {
        var payload = GetPayload(packet);
        if (payload.Length < 60) // N3000 returns 64 bytes total, payload starts at 4
        {
            throw new InvalidOperationException("Discovery response payload is too short.");
        }

        // Based on N3000 Broadcast Search response (payload starts at frame offset 4)
        var sn = BinaryPrimitives.ReadUInt32LittleEndian(payload[0..4]);
        var ip = $"{payload[4]}.{payload[5]}.{payload[6]}.{payload[7]}";
        var mask = $"{payload[8]}.{payload[9]}.{payload[10]}.{payload[11]}";
        var gate = $"{payload[12]}.{payload[13]}.{payload[14]}.{payload[15]}";
        var mac = $"{payload[16]:X2}:{payload[17]:X2}:{payload[18]:X2}:{payload[19]:X2}:{payload[20]:X2}:{payload[21]:X2}";
        var version = $"{payload[22]}.{payload[23]}";
        
        // Date: 20YY/MM/DD
        var dateStr = $"20{payload[24]:D2}-{payload[25]:D2}-{payload[26]:D2}";
        DateTime.TryParse(dateStr, out var date);

        // Port (usually at offset 28 in payload / 32 in frame)
        var port = BinaryPrimitives.ReadUInt16LittleEndian(payload[28..30]);
        if (port == 0) port = 60000;

        // Allowed PC IP (usually at offset 30 or 34 in payload)
        // In some variants it's at offset 30 (bytes 34-37 of frame)
        var allowedIp = $"{payload[30]}.{payload[31]}.{payload[32]}.{payload[33]}";

        return new DiscoveryResult
        {
            SerialNumber = sn,
            IpAddress = ip,
            SubnetMask = mask,
            Gateway = gate,
            MacAddress = mac,
            FirmwareVersion = version,
            ControllerDate = date,
            Port = port,
            AllowedPcIp = allowedIp
        };
    }
}


