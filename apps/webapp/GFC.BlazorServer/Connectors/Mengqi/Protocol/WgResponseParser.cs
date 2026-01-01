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

    public static (List<ControllerEvent> Events, uint ControllerLastIndex) ParseEvents(ReadOnlySpan<byte> packet, uint requestedIndex = 0)
    {
        var payload = GetPayload(packet);
        var events = new List<ControllerEvent>();
        
        // N3000 Standard Mode (0xB0) Response Structure (64 bytes total):
        // Packet[0..1]:    Type (0x17) + Command (0xB0)
        // Packet[2..7]:    Header data
        // Packet[8..11]:   Last Index (4 bytes, little-endian) - this is at payload[4..7]
        // Packet[12..31]:  Single 20-byte event record - this is at payload[8..27]
        // Packet[32..62]:  Padding (zeros)
        // Packet[63]:      Checksum
        
        if (payload.Length < 28) return (events, 0); // Need at least 8 header + 20 event bytes
        
        // Extract last index from payload bytes 4-7 (packet bytes 8-11)
        uint ControllerLastIndex = BinaryPrimitives.ReadUInt32LittleEndian(payload[4..8]);
        
        // Single event record starts at payload byte 8 (packet byte 12)
        var eventData = payload.Slice(8, 20);
        
        // Log the raw 20-byte record for debugging
        var hexDump = string.Join(" ", eventData.ToArray().Select(b => b.ToString("X2")));
        System.Diagnostics.Debug.WriteLine($"Event Record: {hexDump}");
        
        // N3000 Event Log Record Layout (20 bytes) - Verified by USER:
        // [0]      Event Category (0x01 = Swipe, 0x03 = System/Sensor) - Packet Index 12
        // [1..3]   Padding/Misc
        // [4..7]   Card Number (uint32, little-endian) - Packet Index 16-19
        // [8..14]  Timestamp (7 bytes BCD: Century, Year, Month, Day, Hour, Min, Sec) - Packet Index 20-26
        // [15]     Result / Reason Code (0x01=Allowed, 0x1C=Closed, etc.) - Packet Index 27
        // [16]     Door Number (1, 2, 3, or 4) - Packet Index 28
        // [17..19] Reserved

        var category = eventData[0];
        var card = BinaryPrimitives.ReadUInt32LittleEndian(eventData[4..8]);
        
        DateTime timestamp = DateTime.MinValue;
        try
        {
            // Read BCD timestamp from bytes 8-14
            var century = DecodeBcd(eventData[8]);
            var year = DecodeBcd(eventData[9]);
            var month = DecodeBcd(eventData[10]);
            var day = DecodeBcd(eventData[11]);
            var hour = DecodeBcd(eventData[12]);
            var minute = DecodeBcd(eventData[13]);
            var second = DecodeBcd(eventData[14]);
            
            if (month > 0 && month <= 12 && day > 0 && day <= 31)
            {
                var fullYear = (century * 100) + year;
                timestamp = new DateTime(fullYear, month, day, hour, minute, second, DateTimeKind.Utc);
            }
        }
        catch { /* Fallback to MinValue on parse error */ }

        var resultReason = eventData[15];
        var door = eventData[16];
        
        // Map Category to EventType and Result to ReasonCode
        // Category 1 = Swipe, Category 3 = System/Sensor
        return (new List<ControllerEvent> 
        { 
            new ControllerEvent
            {
                CardNumber = (category == 1 && card > 0) ? (long)card : 0, 
                DoorOrReader = door,
                EventType = (ControllerEventType)category, 
                ReasonCode = resultReason,
                TimestampUtc = timestamp,
                RawIndex = requestedIndex // Use the index we requested
            } 
        }, ControllerLastIndex);
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

    /// <summary>
    /// Parses the response from command 0x5A "Get Door Parameters"
    /// Request: 17 5A ... 01 ... (Door Index)
    /// Response: Byte 8=ControlMode, 9=Delay, 10=Interlock, 12=Timeout, 13=Verify
    /// </summary>
    public static DoorHardwareConfig ParseDoorParams(ReadOnlySpan<byte> packet, int doorIndex)
    {
        var payload = GetPayload(packet); 
        // GetPayload returns packet[4..]. 
        // Packet Byte 8  => Payload[4] (Control Mode)
        // Packet Byte 9  => Payload[5] (Unlock Duration)
        // Packet Byte 10 => Payload[6] (Interlock)
        // Packet Byte 11 => Payload[7] (Unused/Reserved)
        // Packet Byte 12 => Payload[8] (Door Ajar Timeout)
        // Packet Byte 13 => Payload[9] (Verification)

        if (payload.Length < 10) // Need at least up to Payload[9]
        {
             // Fallback or empty if too short
             return new DoorHardwareConfig { DoorIndex = doorIndex };
        }

        return new DoorHardwareConfig
        {
            DoorIndex = doorIndex,
            ControlMode = (DoorControlMode)payload[4],
            UnlockDuration = payload[5], 
            Interlock = (DoorInterlockMode)payload[6],
            SensorType = payload[7], // Byte 11, presumed Sensor Type
            DoorAjarTimeout = payload[8],
            Verification = (DoorVerificationMode)payload[9]
        };
    }

    private static int BcdToInt(byte bcd) => DecodeBcd(bcd);

    public static int DecodeBcd(byte bcd)
    {
        return ((bcd >> 4) * 10) + (bcd & 0x0F);
    }
}
