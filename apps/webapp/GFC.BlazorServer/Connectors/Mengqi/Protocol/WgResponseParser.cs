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
        // Data follows 8-byte header
        return data.Slice(8, 64 - 8 - 1); // -1 for tail sum
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
        var payload = GetPayload(packet);

        // N3000 Status Poll (0x20) Response Layout:
        // Byte 8-11:  Controller SN (Payload 0-3)
        // Byte 12-15: Index (Payload 4-7)
        // Byte 20-26: Timestamp (Payload 12-18)
        
        uint totalEvents = 0;
        DateTime? controllerTime = null;

        if (payload.Length >= 18)
        {
            totalEvents = BinaryPrimitives.ReadUInt32LittleEndian(payload[4..8]);

            try 
            {
                var p = payload.Slice(12); // Packet Byte 20
                int century = DecodeBcd(p[0]);
                int yearPart = DecodeBcd(p[1]);
                int fullYear = (century * 100) + yearPart;
                int month = DecodeBcd(p[2]);
                int day = DecodeBcd(p[3]);
                int hour = DecodeBcd(p[4]);
                int minute = DecodeBcd(p[5]);
                int second = DecodeBcd(p[6]);

                controllerTime = new DateTime(fullYear, month, day, hour, minute, second);
            }
            catch 
            {
                // Fallback
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

        // SSI Card Read (0x50) Verified Layout (Shifted 8 bytes from Legacy):
        // [0..3] Card Number (Packet 16-19)
        // [4..11] Dates (Packet 20-27) -> Start (4), End (4)
        // [12] Permission Mask (Packet 28)

        // 1. Card ID
        var cardId = BinaryPrimitives.ReadUInt32LittleEndian(payload[0..4]);

        // 2. Dates (BCD)
        var validFrom = ParseBcdDate(payload.Slice(4, 4));
        var validTo = ParseBcdDate(payload.Slice(8, 4));

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
        // Packet 8-11:   Last Index (Payload 0-3)
        // Packet 12-31:  Event record (Payload 4-23)
        // Event data offsets (relative to Packet Offset 12):
        // [0]            Event Type (Packet 12)
        // [1]            Result / Reason Code (Packet 13)
        // [2]            Door Number (Packet 14)
        // [3]            Reader Number?
        // [8..11]        Card ID (Packet 20-23)
        // [12..18]       Timestamp (Packet 24-30)
 
        if (payload.Length < 24) return (events, 0); 
        
        uint ControllerLastIndex = BinaryPrimitives.ReadUInt32LittleEndian(payload[0..4]);
        var eventData = payload.Slice(4, 20); // Byte 12-31
        
        var category = eventData[0];
        var card = BinaryPrimitives.ReadUInt32LittleEndian(eventData[8..12]);
        
        DateTime timestamp = DateTime.MinValue;
        try
        {
            // Timestamp at eventData[12..18] -> Packet 24-30
            var century = DecodeBcd(eventData[12]);
            var year = DecodeBcd(eventData[13]);
            var month = DecodeBcd(eventData[14]);
            var day = DecodeBcd(eventData[15]);
            var hour = DecodeBcd(eventData[16]);
            var minute = DecodeBcd(eventData[17]);
            var second = DecodeBcd(eventData[18]);
            
            if (month > 0 && month <= 12 && day > 0 && day <= 31)
            {
                var fullYear = (century * 100) + year;
                timestamp = new DateTime(fullYear, month, day, hour, minute, second, DateTimeKind.Utc);
            }
        }
        catch { }
 
        var resultReason = eventData[1];
        var door = eventData[2];
        
        return (new List<ControllerEvent> 
        { 
            new ControllerEvent
            {
                CardNumber = (category == 1 && card > 0) ? (long)card : 0, 
                DoorOrReader = door,
                EventType = (ControllerEventType)category, 
                ReasonCode = resultReason,
                TimestampUtc = timestamp,
                RawIndex = requestedIndex
            } 
        }, ControllerLastIndex);
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
        
        var dateStr = $"20{payload[32]:D2}-{payload[33]:D2}-{payload[34]:D2}";
        DateTime.TryParse(dateStr, out var date);
 
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
        return ((bcd >> 4) * 10) + (bcd & 0x0F);
    }
}
