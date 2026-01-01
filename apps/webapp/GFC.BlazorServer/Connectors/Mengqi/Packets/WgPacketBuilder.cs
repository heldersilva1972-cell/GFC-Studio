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

    public byte[] Build(uint targetControllerSn, ushort xid, WgCommandProfile profile, ReadOnlySpan<byte> payload)
    {
        profile.EnsureConfigured();
        
        // N3000 controllers expect a fixed 64-byte frame
        var buffer = new byte[FullFrameLength];
        var span = buffer.AsSpan();

        // 1. Core Header (Offsets 0-3)
        span[0] = profile.PacketType;                                           // Offset 0: Header (0x17)
        span[1] = profile.CommandCode;                                          // Offset 1: Function Code
        BinaryPrimitives.WriteUInt16LittleEndian(span[2..], 0);                // Offset 2-3: CRC (Zero during calc)

        // 2. Control Info (Offsets 4-7)
        // For N3000, the target Serial Number always occupies bytes 4-7.
        BinaryPrimitives.WriteUInt32LittleEndian(span[4..], targetControllerSn);
        
        // 2b. XID (Offsets 40-43)
        // N3000 Short Packet (0x17) originally placed sequence ID at offset 40.
        // However, verified hardware tests show that for certain configuration commands (like 0x8E), 
        // the controller expects these bytes to be zero (or they interfere with the payload).
        // Removing for maximum compatibility with N3000 baseline.
        // BinaryPrimitives.WriteUInt32LittleEndian(span[40..], (uint)xid);

        // 3. Command Payload (Offset 8 onwards)
        if (!payload.IsEmpty)
        {
            var maxPayload = FullFrameLength - 8;
            var bytesToCopy = Math.Min(payload.Length, maxPayload);
            payload.Slice(0, bytesToCopy).CopyTo(span[8..]);
        }

        // 4. Checksum
        // If targeting the "Unlock" Wildcard SN (0x00118000), use Summation Checksum. Otherwise use CRC16-IBM.
        const uint WildcardSn = 0x00118000; // 00 80 11 00 (Little Endian)
        if (targetControllerSn == WildcardSn || profile.CommandCode == 0x24) // 0x24 is Broadcast Search
        {
            WriteSummationChecksum(span);
        }
        else
        {
            WriteCrc(span);
        }

        // 5. Special Integrity Check for Privilege Upload (0x50) and Clear (0x54)
        // User Requirement: "Byte 63 (the final byte) must contain a 1-byte summation checksum of all preceding 63 bytes."
        if (profile.CommandCode == 0x50 || profile.CommandCode == 0x54)
        {
            long fullSum = 0;
            for (int i = 0; i < 63; i++)
            {
                fullSum += span[i];
            }
            span[63] = (byte)(fullSum & 0xFF);
        }

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


