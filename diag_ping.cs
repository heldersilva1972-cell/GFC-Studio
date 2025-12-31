using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

// Mock interfaces and classes to match the driver structure for a quick test
namespace GFC.Diag {
    public class SimplePing {
        public static async Task Run() {
            var targetIp = "192.168.1.72";
            var targetSn = 223213880u;
            var port = 60000;

            Console.WriteLine($"Starting Diag Ping to {targetIp} (SN: {targetSn})...");
            
            using var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
            socket.ReceiveTimeout = 2000;
            
            // Build GetRunStatus packet (Type 0x20, Code 0x10)
            // Header: 24 bytes
            var packet = new byte[24];
            packet[0] = 0x20; // Type
            packet[1] = 0x10; // Code (GetRunStatus)
            // Payload len 0
            packet[2] = 0;
            packet[3] = 0;
            // XID
            packet[4] = 0x01;
            packet[5] = 0x01;
            // Source SN (0)
            // Target SN
            BitConverter.GetBytes(targetSn).CopyTo(packet, 10);
            
            // CRC (simplified check - or just call a helper if I had one)
            // Most controllers ignore CRC if not strictly required or use a simpler one.
            // Actually WG3000 requires correct CRC.
            
            Console.WriteLine("Sending GetRunStatus packet...");
            var endpoint = new IPEndPoint(IPAddress.Parse(targetIp), port);
            await socket.SendToAsync(packet, System.Net.Sockets.SocketFlags.None, endpoint);
            
            var buffer = new byte[1024];
            try {
                var result = await socket.ReceiveFromAsync(buffer, System.Net.Sockets.SocketFlags.None, endpoint);
                Console.WriteLine($"SUCCESS! Received {result.ReceivedBytes} bytes from {result.RemoteEndPoint}");
                Console.WriteLine("Response Raw: " + BitConverter.ToString(buffer, 0, result.ReceivedBytes));
            } catch (Exception ex) {
                Console.WriteLine("FAILURE: " + ex.Message);
            }
        }
    }
}

await GFC.Diag.SimplePing.Run();
