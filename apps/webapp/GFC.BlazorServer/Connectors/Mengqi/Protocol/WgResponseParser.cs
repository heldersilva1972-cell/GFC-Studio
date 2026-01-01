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

        // N3000 Status Poll (0x20) Verified Layout:
        // [0..3] SN (4 bytes) - Frame 4-7
        // [4..7] Last Index (4 bytes) - Frame 8-11
        // [8] Record Type - Frame 12
        // [9] Access Result - Frame 13
        // ...
        // [15] Door Number? (Summary says Frame 19 -> Payload 11?)
        // ...
        // [16..22] Timestamp (7 bytes BCD) - Frame 20-26
        
        uint totalEvents = 0;
        DateTime? controllerTime = null;

        if (payload.Length >= 23)
        {
            // Last Index (Bytes 8-11 -> Payload 4-7)
            totalEvents = BinaryPrimitives.ReadUInt32LittleEndian(payload[4..8]);

            // Timestamp (Bytes 20-26 -> Payload 16-22)
            // Layout: Century, Year, Month, Day, Hour, Minute, Second
            try 
            {
                var p = payload; // shorthand
                int century = DecodeBcd(p[16]);
                int yearPart = DecodeBcd(p[17]);
                int fullYear = (century * 100) + yearPart;
                int month = DecodeBcd(p[18]);
                int day = DecodeBcd(p[19]);
                int hour = DecodeBcd(p[20]);
                int minute = DecodeBcd(p[21]);
                int second = DecodeBcd(p[22]);

                controllerTime = new DateTime(fullYear, month, day, hour, minute, second);
            }
            catch 
            {
                // Fallback or ignore corrupt time
            }
        }
        
        // Note: Door Status bits were not explicitly defined in the new 0x20 spec.
        // We will return empty door/relay states to avoid misleading the UI with incorrect offsets.
        // The UI should rely on events or specific queries if needed.

        return new RunStatusModel
        {
            Doors = Array.Empty<RunStatusModel.DoorStatus>(), 
            RelayStates = Array.Empty<bool>(),
            IsFireAlarmActive = false, 
            IsTamperActive = false,
            TotalCards = 0,
            TotalEvents = totalEvents,
            ControllerTime = controllerTime
        };
    }

    public static CardPrivilegeModel ParseCardData(ReadOnlySpan<byte> packet)
    {
        var payload = GetPayload(packet);
        if (payload.Length < 21) throw new InvalidOperationException("Card data payload too short.");

        // N3000 Card Read (0x50) Verified Layout:
        // [4..7] Card ID (Bytes 8-11)
        // [8..15] Dates (Bytes 12-19) -> Start (4), End (4)
        // [16] Permission Mask (Byte 20)

        // 1. Card ID
        var cardId = BinaryPrimitives.ReadUInt32LittleEndian(payload[4..8]);

        // 2. Dates (BCD)
        // Start: Payload 8..11 (Cent, Year, Month, Day)
        var validFrom = ParseBcdDate(payload.Slice(8, 4));
        
        // End: Payload 12..15 (Cent, Year, Month, Day)
        var validTo = ParseBcdDate(payload.Slice(12, 4));

        // 3. Permissions
        var mask = payload[16];
        var doorList = new List<int>();
        if ((mask & 0x01) != 0) doorList.Add(1);
        if ((mask & 0x02) != 0) doorList.Add(2);
        
        return new CardPrivilegeModel
        {
            CardNumber = cardId,
            ValidFrom = validFrom,
            ValidTo = validTo,
            DoorMask = mask,
            DoorList = doorList
        };
    }

    private static DateTime? ParseBcdDate(ReadOnlySpan<byte> bcdData)
    {
        try
        {
            int century = DecodeBcd(bcdData[0]);
            int year = DecodeBcd(bcdData[1]);
            int month = DecodeBcd(bcdData[2]);
            int day = DecodeBcd(bcdData[3]);
            
            if (month == 0 || day == 0) return null; // Empty/Invalid

            return new DateTime((century * 100) + year, month, day);
        }
        catch
        {
            return null;
        }
    }

    public static (IReadOnlyList<ControllerEvent> Events, uint LastIndex) ParseEvents(ReadOnlySpan<byte> packet)
    {
        var payload = GetPayload(packet);
        var events = new List<ControllerEvent>();
        
        // N3000 Bulk Log (0xB0-0xB4) Verified Record Length: 20 Bytes
        const int recordLength = 20;
        
        // Skip 4-byte SN + 4-byte Unknown/Count at start of payload (Offset 0-7)?
        // User instructions say: "buffer contains multiple event logs... loop through this buffer".
        // Usually N3000 packet starts with SN (4 bytes).
        // Let's assume payload[4..] is where the list starts.
        if (payload.Length < 4) return (events, 0);
        var data = payload[4..];

        for (var offset = 0; offset + recordLength <= data.Length; offset += recordLength)
        {
            var slice = data.Slice(offset, recordLength);
            
            // Layout:
            // [0..3] Index (uint)
            // [4..7] Card Number (uint)
            // [8..14] Timestamp (BCD 7 bytes)
            // [15] Door Number
            // [16] Event Type
            // [17] Direction (0=In, 1=Out)
            // [18..19] Padding

            var index = BinaryPrimitives.ReadUInt32LittleEndian(slice[0..4]);
            var card = BinaryPrimitives.ReadUInt32LittleEndian(slice[4..8]);
            
            DateTime timestamp = DateTime.MinValue;
            try
            {
                var century = DecodeBcd(slice[8]);
                var year = DecodeBcd(slice[9]);
                var month = DecodeBcd(slice[10]);
                var day = DecodeBcd(slice[11]);
                var hour = DecodeBcd(slice[12]);
                var minute = DecodeBcd(slice[13]);
                var second = DecodeBcd(slice[14]);
                timestamp = new DateTime((century * 100) + year, month, day, hour, minute, second);
            }
            catch { /* corrupt time in log */ }

            var door = slice[15];
            var eventTypeByte = slice[16];
            // EventType mapping might need adjustment if hardware codes differ from standard enum
            var eventType = (ControllerEventType)eventTypeByte; 
            
            events.Add(new ControllerEvent
            {
                CardNumber = card,
                DoorOrReader = door,
                EventType = eventType,
                ReasonCode = index, // Using ReasonCode property to store the Log Index for tracking
                TimestampUtc = timestamp
            });
        }

        // The LastIndex is traditionally the index of the last record found.
        // Since we are reading the index from the record itself (slice[0..4]), 
        // we can take the index of the *last successfully processed event*.
        uint lastIndex = 0;
        if (events.Count > 0)
        {
            // Cast strictly to uint as ReasonCode is technically nullable/uint variant in some contexts
            lastIndex = (uint)events[^1].ReasonCode; 
        }

        return (events, lastIndex);
    }

    public static DiscoveryResult ParseDiscovery(ReadOnlySpan<byte> packet)
    {
        var payload = GetPayload(packet);
        if (payload.Length < 60)
        {
            throw new InvalidOperationException("Discovery response payload is too short.");
        }

        var sn = BinaryPrimitives.ReadUInt32LittleEndian(payload[0..4]);
        var ip = $"{payload[4]}.{payload[5]}.{payload[6]}.{payload[7]}";
        var mask = $"{payload[8]}.{payload[9]}.{payload[10]}.{payload[11]}";
        var gate = $"{payload[12]}.{payload[13]}.{payload[14]}.{payload[15]}";
        var mac = $"{payload[16]:X2}:{payload[17]:X2}:{payload[18]:X2}:{payload[19]:X2}:{payload[20]:X2}:{payload[21]:X2}";
        var version = $"{payload[22]}.{payload[23]}";
        
        var dateStr = $"20{payload[24]:D2}-{payload[25]:D2}-{payload[26]:D2}";
        DateTime.TryParse(dateStr, out var date);

        var modes = new byte[4];
        payload[28..32].CopyTo(modes);

        var port = BinaryPrimitives.ReadUInt16LittleEndian(payload[32..34]);
        if (port == 0) port = 60000;

        var allowedIp = $"{payload[34]}.{payload[35]}.{payload[36]}.{payload[37]}";

        var delays = new byte[4];
        if (payload.Length >= 36)
        {
            payload[32..36].CopyTo(delays);
        }

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
            AllowedPcIp = allowedIp,
            DoorModes = modes,
            DoorDelays = delays
        };
    }

    private static int BcdToInt(byte bcd) => DecodeBcd(bcd);

    public static int DecodeBcd(byte bcd)
    {
        return ((bcd >> 4) * 10) + (bcd & 0x0F);
    }
}
