using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using GFC.BlazorServer.Connectors.Mengqi.Models;
using GFC.BlazorServer.Connectors.Mengqi.Packets;

namespace GFC.BlazorServer.Connectors.Mengqi.Protocol;

internal static class WgResponseParser
{
    public static ReadOnlySpan<byte> GetPayload(ReadOnlySpan<byte> data)
    {
        if (data.Length <= 8) return ReadOnlySpan<byte>.Empty;
        return data.Slice(8); // safest; callers must validate expected payload size per command
    }

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
        if (packet.Length < 2)
        {
            return new RunStatusModel
            {
                Doors = Array.Empty<RunStatusModel.DoorStatus>(),
                RelayStates = Array.Empty<bool>(),
                IsFireAlarmActive = false,
                IsTamperActive = false,
                TotalCards = 0,
                TotalEvents = 0,
                ControllerTime = null
            };
        }

        uint highestIndex = 0;

        bool isP64Mode = packet.Length == 64 && packet[0] == 0x17 && packet[1] == 0x20;
        bool isFullMode = packet[0] == 0x20 && packet[1] == 0x21; // full run-info response

        if (isP64Mode)
        {
            // ✅ Vendor behavior: swipe end index is at offset 8..11 in 64-byte run-info
            if (packet.Length >= 12)
            {
                uint swipeEndIndex = BinaryPrimitives.ReadUInt32LittleEndian(packet.Slice(8, 4));
                if (swipeEndIndex != 0xFFFFFFFF)
                {
                    highestIndex = swipeEndIndex;
                }
            }
        }
        else if (isFullMode)
        {
            // ✅ Vendor behavior: 10 swipe summaries; each has IndexInDataFlash at 64 + i*20
            const int swipeArrayStart = 64;
            const int swipeEntrySize = 20;
            const int maxSwipes = 10;

            for (int i = 0; i < maxSwipes; i++)
            {
                int offset = swipeArrayStart + (i * swipeEntrySize);
                if (offset + 4 > packet.Length) break;

                uint index = BinaryPrimitives.ReadUInt32LittleEndian(packet.Slice(offset, 4));
                if (index != 0xFFFFFFFF && index > highestIndex)
                {
                    highestIndex = index;
                }
            }
        }

        // Controller time (BCD) – bytes 20..26 (7 bytes)
        DateTime? controllerTime = null;
        if (packet.Length >= 27)
        {
            controllerTime = TryParseBcdDateTime(packet.Slice(20, 7));
        }

        return new RunStatusModel
        {
            Doors = Array.Empty<RunStatusModel.DoorStatus>(),
            RelayStates = Array.Empty<bool>(),
            IsFireAlarmActive = false,
            IsTamperActive = false,
            TotalCards = 0,

            // NOTE: rename later (this is NOT a "total count"; it's the latest index pointer)
            TotalEvents = highestIndex,

            // If BCD failed, we can fallback to Now or leave null
            ControllerTime = controllerTime
        };
    }



    public static CardPrivilegeModel ParseCardData(ReadOnlySpan<byte> packet)
    {
        var payload = GetPayload(packet);
        if (payload.Length < 21) throw new InvalidOperationException("Card data payload too short.");

        // SSI Card Read (0x50) Verified Layout (Shifted 8 bytes from Legacy):
        // [0..3] Card Number (Packet 16-19)
        // [4..11] Dates (Packet 20-27) -> Start (4), End (4)
        // [12] Permission Mask (Packet 28)

        // 1. Card ID
        var cardId = BinaryPrimitives.ReadUInt32LittleEndian(payload[0..4]);

        // 2. Dates (BCD)
        var validFrom = TryParseBcdDateTime(payload.Slice(4, 4))?.Date;
        var validTo = TryParseBcdDateTime(payload.Slice(8, 4))?.Date;


        // 3. Permissions
        var mask = payload[12];
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


    public static (List<ControllerEvent> Events, uint ControllerLastIndex) ParseEvents(ReadOnlySpan<byte> packet, uint requestedIndex = 0)
    {
        var events = new List<ControllerEvent>();

        // Expecting a 64-byte response for P64 commands
        if (packet.Length < 64)
            return (events, 0);

        // Only parse if this is the GetSingleSwipeRecord/Events response (0x17 0xB0)
        if (!(packet[0] == 0x17 && packet[1] == 0xB0))
            return (events, 0);

        // Vendor path: record payload starts at byte 24
        const int recordOffset = 24;
        const int recordLen = 16;

        if (recordOffset + recordLen > packet.Length)
            return (events, 0);

        var rec = packet.Slice(recordOffset, recordLen);

        // If record is all zeros, treat as empty/missing record
        bool empty = true;
        for (int i = 0; i < rec.Length; i++)
        {
            if (rec[i] != 0) { empty = false; break; }
        }
        if (empty)
        {
            // Still return ControllerLastIndex if present
            uint maybeIndex = BinaryPrimitives.ReadUInt32LittleEndian(packet.Slice(40, 4));
            return (events, maybeIndex);
        }

        // Vendor record layout: CardID is 8 bytes at 0..7 (some systems only use lower 4)
        ulong card64 = BinaryPrimitives.ReadUInt64LittleEndian(rec.Slice(0, 8));
        uint card32 = (uint)(card64 & 0xFFFFFFFF);

        // Vendor record layout: date/time starts at byte 8
        // Try packed format (4 bytes) first as per vendor WgDateToMsDate logic
        DateTime? parsedTime = TryParsePackedDateTime(rec.Slice(8, 4));
        
        // Fallback to BCD (7 bytes) if packed failed or looks like garbage
        if (parsedTime == null)
        {
            parsedTime = TryParseBcdDateTime(rec.Slice(8, 7), DateTimeKind.Utc);
        }

        DateTime timestampUtc = parsedTime ?? DateTime.MinValue;


        // ControllerLastIndex/loc echoed (vendor reads bytes 40..43)

        uint controllerLastIndex = BinaryPrimitives.ReadUInt32LittleEndian(packet.Slice(40, 4));

        events.Add(new ControllerEvent
        {
            CardNumber = card32 != 0 ? card32 : (long)card64,
            DoorOrReader = 0,               // TODO: decode via swipe record status/reader fields once mapped
            EventType = ControllerEventType.Unknown, // TODO: map from record flags later
            ReasonCode = 0,                 // TODO: map from record flags later
            TimestampUtc = timestampUtc,
            RawIndex = requestedIndex
        });

        return (events, controllerLastIndex);
    }


    public static DiscoveryResult ParseDiscovery(ReadOnlySpan<byte> packet)
    {
        var payload = GetPayload(packet);
        if (payload.Length < 50)
        {
            throw new InvalidOperationException("Discovery response payload is too short.");
        }
 
        // SN:   Packet 4-7
        // IP:   Packet 20-23 (Payload 12-15)
        // MAC:  Packet 32-37 (Payload 24-29)
        
        var sn = BinaryPrimitives.ReadUInt32LittleEndian(packet[4..8]);
        var ip = $"{payload[12]}.{payload[13]}.{payload[14]}.{payload[15]}";
        var mask = $"{payload[16]}.{payload[17]}.{payload[18]}.{payload[19]}";
        var gate = $"{payload[20]}.{payload[21]}.{payload[22]}.{payload[23]}";
        var mac = $"{payload[24]:X2}:{payload[25]:X2}:{payload[26]:X2}:{payload[27]:X2}:{payload[28]:X2}:{payload[29]:X2}";
        var version = $"{payload[30]}.{payload[31]}";
        
        DateTime? date = null;
        try
        {
            int year = DecodeBcd(payload[32]);
            int month = DecodeBcd(payload[33]);
            int day = DecodeBcd(payload[34]);
            if (month is >= 1 and <= 12 && day is >= 1 and <= 31)
            {
                date = new DateTime(2000 + year, month, day);
            }
        }
        catch { /* invalid date */ }
 
        return new DiscoveryResult
        {
            SerialNumber = sn,
            IpAddress = ip,
            SubnetMask = mask,
            Gateway = gate,
            MacAddress = mac,
            FirmwareVersion = version,
            ControllerDate = date
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
            ControlMode = (DoorControlMode)payload[0],  // Packet 16
            UnlockDuration = payload[1],                // Packet 17 (Verified by Test Script)
            Interlock = (DoorInterlockMode)payload[2],  // Packet 18
            SensorType = payload[3],                     // Packet 19
            DoorAjarTimeout = payload[4],                // Packet 20
            Verification = (DoorVerificationMode)payload[5] // Packet 21
        };
    }

    private static int BcdToInt(byte bcd) => DecodeBcd(bcd);

    public static int DecodeBcd(byte bcd)
    {
        // Sanity check for valid BCD nibbles
        if ((bcd & 0x0F) > 0x09 || (bcd >> 4) > 0x09)
            throw new ArgumentException("Invalid BCD byte.");

        return ((bcd >> 4) * 10) + (bcd & 0x0F);
    }

    private static DateTime? TryParseBcdDateTime(ReadOnlySpan<byte> data, DateTimeKind kind = DateTimeKind.Unspecified)
    {
        // Handle both 4-byte (YMD) and 7-byte (YMDHMS) BCD
        if (data.Length < 4) return null;

        try
        {
            int century, year, month, day, hour = 0, minute = 0, second = 0;

            if (data.Length >= 7)
            {
                century = DecodeBcd(data[0]);
                year = DecodeBcd(data[1]);
                month = DecodeBcd(data[2]);
                day = DecodeBcd(data[3]);
                hour = DecodeBcd(data[4]);
                minute = DecodeBcd(data[5]);
                second = DecodeBcd(data[6]);
            }
            else
            {
                // 4-byte format assumes century 2000
                century = DecodeBcd(data[0]);
                year = DecodeBcd(data[1]);
                month = DecodeBcd(data[2]);
                day = DecodeBcd(data[3]);
            }

            // Range validation
            if (month is < 1 or > 12) return null;
            if (day is < 1 or > 31) return null;
            if (hour is < 0 or > 23) return null;
            if (minute is < 0 or > 59) return null;
            if (second is < 0 or > 59) return null;

            return new DateTime(century * 100 + year, month, day, hour, minute, second, kind);
        }
        catch { return null; }
    }

    /// <summary>
    /// Parses vendor packed date format (4 bytes)
    /// </summary>
    private static DateTime? TryParsePackedDateTime(ReadOnlySpan<byte> data)
    {
        if (data.Length < 4) return null;
        try
        {
            ushort num = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(0, 2));
            ushort num2 = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(2, 2));
            
            int second = (num2 & 31) << 1;
            int minute = (num2 >> 5) & 63;
            int hour = num2 >> 11;
            
            int day = num & 31;
            int month = (num >> 5) & 15;
            int year = (num >> 9) + 2000;

            if (month is < 1 or > 12) return null;
            if (day is < 1 or > 31) return null;
            if (hour is < 0 or > 23) return null;
            if (minute is < 0 or > 59) return null;
            if (second is < 0 or > 59) return null;

            return new DateTime(year, month, day, hour, minute, second);
        }
        catch { return null; }
    }
}


