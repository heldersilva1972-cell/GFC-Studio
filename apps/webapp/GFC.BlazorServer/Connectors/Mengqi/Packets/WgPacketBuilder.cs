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
        // Compute CRC over the entire 64-byte frame and write to offset 2-3
        WriteCrc(span);

        return buffer;
    }

    internal static void WriteCrc(Span<byte> packet)
    {
        var crc = Crc16Ibm.Compute(packet);
        BinaryPrimitives.WriteUInt16LittleEndian(packet[2..4], crc);
    }
}


