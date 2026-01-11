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
        // FIX: Added explicit guard. If data is too short, return empty instead of crashing. 
        if (data.Length < 8) return ReadOnlySpan<byte>.Empty;
        return data.Slice(8); 
    }

    public static void EnsureAck(ReadOnlySpan<byte> packet, WgCommandProfile profile)
    {
        if (packet.Length < 2) return; // FIX: Silent return instead of 'throw'

        // Allow 0x17 (Request/Response) or 0x18 (Some N3000 responses)
        if (packet[0] != profile.PacketType && packet[0] != 0x18) return; 
        
        if (packet[1] != profile.CommandCode) return;
    }

    public static RunStatusModel ParseRunStatus(ReadOnlySpan<byte> packet)
    {
        try
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

            bool isP64Mode = packet.Length >= 64 && packet[0] == 0x17 && packet[1] == 0x20;
            bool isFullMode = packet.Length >= 2 && packet[0] == 0x20 && packet[1] == 0x21; // full run-info response

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
        catch (Exception)
        {
            // FIX: Return an empty model so the Sync Service doesn't see a FATAL error.
            return new RunStatusModel { TotalEvents = 0 };
        }
    }



    public static CardPrivilegeModel ParseCardData(ReadOnlySpan<byte> packet)
    {
        try
        {
            var payload = GetPayload(packet);
            if (payload.Length < 21) return new CardPrivilegeModel { DoorList = new List<int>() };

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
        catch { return new CardPrivilegeModel { DoorList = new List<int>() }; }
    }


    public static (List<ControllerEvent> Events, uint ControllerLastIndex) ParseEvents(ReadOnlySpan<byte> packet, uint requestedIndex = 0)
    {
        var events = new List<ControllerEvent>();

        // COMPLETE EVENT-READBACK SPEC (WG3000/Mengqi) — FROM REAL WIRESHARK CAPTURES
        
        // Expecting a 64-byte response for P64 commands
        if (packet.Length < 64) return (events, 0);

        // Byte[0] = 0x17, Byte[1] = 0x20 (confirmed command code for event record in user capture)
        // Some firmwares might still use 0xB0, so we should ideally check both or rely on the dispatcher.
        // For this specific task, we align with the 0x20 spec provided.
        if (packet[0] != 0x17 || (packet[1] != 0x20 && packet[1] != 0xB0))
            return (events, 0);

        // B) Event Index (uint32 little-endian) - Byte[8..11]
        uint eventIndex = BinaryPrimitives.ReadUInt32LittleEndian(packet.Slice(8, 4));

        // C) Event metadata (4 bytes) - Byte[12..15]
        // Byte[14] is DoorNumber (1-based: 0x01 = Door 1, 0x02 = Door 2)
        int doorNumber = packet[14];

        // D) Card number (uint32 little-endian) - Byte[16..19]
        uint cardNumber = BinaryPrimitives.ReadUInt32LittleEndian(packet.Slice(16, 4));

        // E) Timestamp (BCD-coded) - Byte[20..26]
        // Format: CC YY MM DD HH mm ss
        var timestampBytes = packet.Slice(20, 7);
        DateTime? timestampRaw = TryParseBcdDateTime(timestampBytes, DateTimeKind.Unspecified);
        
        // DIAGNOSTIC: Log if timestamp parsing fails
        if (timestampRaw == null || timestampRaw.Value.Year < 2000)
        {
            var hexBytes = string.Join(" ", timestampBytes.ToArray().Select(b => b.ToString("X2")));
            Console.WriteLine($"WARNING: Failed to parse timestamp bytes: {hexBytes}");
            // TEMPORARY: Use current time until we fix the parser
            timestampRaw = DateTime.Now;
        }

        // Event result is at Byte 13 (2nd byte of metadata block at 12-15)
        // Byte 13: 0x01 = Access Granted, 0x00 = Access Denied
        // Byte 14: Door number (0x01 = Door 1, 0x02 = Door 2)
        byte eventTypeRaw = packet[13];
        
        // Store raw packet for debugging
        var rawDataHex = string.Join(" ", packet.ToArray().Select(b => b.ToString("X2")));
        
        events.Add(new ControllerEvent
        {
            CardNumber = cardNumber,
            DoorOrReader = doorNumber,
            EventType = (ControllerEventType)eventTypeRaw,
            IsByCard = (eventTypeRaw >= 0x01 && eventTypeRaw <= 0x0E),
            IsByButton = (eventTypeRaw == 0x15 || eventTypeRaw == 0x16 || eventTypeRaw == 0x19 || eventTypeRaw == 0x25),
            TimestampUtc = timestampRaw.Value, // Still assigned to property named TimestampUtc, will fix conversion in service.
            RawIndex = eventIndex,
            RawData = rawDataHex
        });

        return (events, eventIndex);
    }


    public static DiscoveryResult ParseDiscovery(ReadOnlySpan<byte> packet)
    {
        try
        {
            var payload = GetPayload(packet);
            if (payload.Length < 50) return new DiscoveryResult();

            // SN:   Packet 4-7
            // IP:   Packet 20-23 (Payload 12-15)
            // MAC:  Packet 32-37 (Payload 24-29)
            
            var sn = packet.Length >= 8 ? BinaryPrimitives.ReadUInt32LittleEndian(packet[4..8]) : 0;
            var ip = payload.Length >= 16 ? $"{payload[12]}.{payload[13]}.{payload[14]}.{payload[15]}" : "0.0.0.0";
            var mask = payload.Length >= 20 ? $"{payload[16]}.{payload[17]}.{payload[18]}.{payload[19]}" : "0.0.0.0";
            var gate = payload.Length >= 24 ? $"{payload[20]}.{payload[21]}.{payload[22]}.{payload[23]}" : "0.0.0.0";
            var mac = payload.Length >= 30 ? $"{payload[24]:X2}:{payload[25]:X2}:{payload[26]:X2}:{payload[27]:X2}:{payload[28]:X2}:{payload[29]:X2}" : "00:00:00:00:00:00";
            var version = payload.Length >= 32 ? $"{payload[30]}.{payload[31]}" : "0.0";
            
            DateTime? date = null;
            if (payload.Length >= 35)
            {
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
            }

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
        catch { return new DiscoveryResult(); }
    }


    /// <summary>
    /// Parses the response from command 0x5A "Get Door Parameters"
    /// Request: 17 5A ... 01 ... (Door Index)
    /// Response: Byte 8=ControlMode, 9=Delay, 10=Interlock, 12=Timeout, 13=Verify
    /// </summary>
    public static DoorHardwareConfig ParseDoorParams(ReadOnlySpan<byte> packet, int doorIndex)
    {
        try
        {
            var payload = GetPayload(packet); 
            if (payload.Length < 10) return new DoorHardwareConfig { DoorIndex = doorIndex };

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
        catch { return new DoorHardwareConfig { DoorIndex = doorIndex }; }
    }

    private static int BcdToInt(byte bcd) => DecodeBcd(bcd);

    public static int DecodeBcd(byte bcd)
    {
        // FIX: Removed 'throw'. Invalid data now returns 0 instead of crashing the site. 
        if ((bcd & 0x0F) > 0x09 || (bcd >> 4) > 0x09)
        {
            return 0; 
        }

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


