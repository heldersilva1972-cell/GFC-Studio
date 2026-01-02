using System;
using System.Buffers.Binary;
using GFC.BlazorServer.Connectors.Mengqi.Configuration;
using GFC.BlazorServer.Connectors.Mengqi.Utilities;

namespace GFC.BlazorServer.Connectors.Mengqi.Packets;

internal sealed class WgPacketBuilder
{
    internal const int HeaderLength = 4;   // Type, Cmd, CRC (2 bytes)
    internal const int FullFrameLength = 64; // N3000 standard frame size
    private readonly ControllerClientOptions _options;

    public WgPacketBuilder(ControllerClientOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public byte[] Build(uint targetControllerSn, ushort xid, WgCommandProfile profile, ReadOnlySpan<byte> payload, ushort localPort = 60000)
    {
        profile.EnsureConfigured();
        
        // N3000 controllers expect a fixed 64-byte frame
        var buffer = new byte[FullFrameLength];
        var span = buffer.AsSpan();

        // 1. Core Header (Offsets 0-3)
        span[0] = profile.PacketType;                                           // Offset 0: Header (0x17)
        span[1] = profile.CommandCode;                                          // Offset 1: Function Code
        
        // 2. Control Header (Offsets 4-7)
        // Offset 4-7: Destination Serial Number (Target ID)
        BinaryPrimitives.WriteUInt32LittleEndian(span[4..8], targetControllerSn);
 
        // 3. Command Payload (Offset 8 onwards)
        if (!payload.IsEmpty)
        {
            var maxPayload = FullFrameLength - 8 - 1; // -1 for tail sum
            var bytesToCopy = Math.Min(payload.Length, maxPayload);
            payload.Slice(0, bytesToCopy).CopyTo(span[8..]);
        }

        // 4. Checksum (Salted CRC for SSI)
        // Set salt (localPort) into CRC position for calculation
        BinaryPrimitives.WriteUInt16LittleEndian(span[2..4], localPort); 
        var crc = Crc16Ibm.ComputeSsi(span);
        BinaryPrimitives.WriteUInt16LittleEndian(span[2..4], crc);

        // 5. Final Integrity Check (Tail Sum at Byte 63)
        // All configuration and state packets in this version require the 1-byte summation at the end.
        long fullSum = 0;
        for (int i = 0; i < 63; i++)
        {
            fullSum += span[i];
        }
        span[63] = (byte)(fullSum & 0xFF);

        return buffer;
    }

    internal static void WriteCrc(Span<byte> packet)
    {
        var crc = Crc16Ibm.Compute(packet);
        BinaryPrimitives.WriteUInt16LittleEndian(packet[2..4], crc);
    }
    
    internal static void WriteSummationChecksum(Span<byte> packet)
    {
        // The checksum is the sum of all bytes in the 64-byte packet
        // excluding the checksum positions at index 2 and 3.
        long sum = 0;
        for (int i = 0; i < packet.Length; i++)
        {
            if (i == 2 || i == 3) continue; 
            sum += packet[i];
        }
        // Low byte first, then high byte (Little Endian)
        packet[2] = (byte)(sum & 0xFF);
        packet[3] = (byte)((sum >> 8) & 0xFF);
    }
}


