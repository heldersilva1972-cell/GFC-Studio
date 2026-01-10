using System;
using System.Linq;
using System.Collections.Generic;
using GFC.BlazorServer.Connectors.Mengqi.Protocol;
using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Diagnostics
{
    public static class EventSpecVerifier
    {
        public static void Run()
        {
            Console.WriteLine("=== WG3000 EVENT SPEC VERIFICATION ===");

            // Sample RX hex from user spec
            string hex = "17 20 00 00 38 F9 4D 0D 26 01 00 00 01 01 02 01 C0 15 CE 00 20 26 01 09 15 11 03 01 01 01 00 00 00 00 00 00 00 15 38 18 00 00 00 00 00 00 00 00 00 00 00 26 01 09 00 00 29 34 61 79 19 02 00 00";
            
            byte[] bytes = hex.Split(' ')
                .Select(s => Convert.ToByte(s, 16))
                .ToArray();

            // Run parser
            var (events, lastIndex) = WgResponseParser.ParseEvents(bytes);

            if (!events.Any())
            {
                Console.WriteLine("FAILED: No events parsed from buffer.");
                return;
            }

            var e = events[0];
            uint sn = BitConverter.ToUInt32(bytes, 4);

            Console.WriteLine($"SN: {sn} (Expected: 223213880)");
            Console.WriteLine($"Index: {e.RawIndex} (Expected: 294)");
            Console.WriteLine($"Door: {e.DoorOrReader} (Expected: 2)");
            Console.WriteLine($"Card: {e.CardNumber} (Expected: 13505984)");
            Console.WriteLine($"Timestamp: {e.TimestampUtc:yyyy-MM-dd HH:mm:ss} (Expected: 2026-01-09 15:11:03)");

            bool success = true;
            if (sn != 223213880) success = false;
            if (e.RawIndex != 294) success = false;
            if (e.DoorOrReader != 2) success = false;
            if (e.CardNumber != 13505984) success = false;
            if (e.TimestampUtc != new DateTime(2026, 1, 9, 15, 11, 3, DateTimeKind.Utc)) success = false;

            if (success)
            {
                Console.WriteLine("\n✅ VERIFICATION PASSED: Parser matches specification exactly.");
            }
            else
            {
                Console.WriteLine("\n❌ VERIFICATION FAILED: Discrepancy found between parsed data and specification.");
            }
        }
    }
}
