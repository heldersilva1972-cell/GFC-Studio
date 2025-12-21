using System;
using System.Buffers.Binary;
using Gfc.ControllerClient.Configuration;
using Gfc.ControllerClient.Utilities;

namespace Gfc.ControllerClient.Packets;

internal sealed class WgPacketBuilder
{
    internal const int HeaderLength = 24;
    internal const int CrcFieldOffset = HeaderLength - 2;
    private readonly ControllerClientOptions _options;

    public WgPacketBuilder(ControllerClientOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public byte[] Build(uint targetControllerSn, ushort xid, WgCommandProfile profile, ReadOnlySpan<byte> payload)
    {
        profile.EnsureConfigured();
        if (payload.Length != profile.RequestPayloadLength && profile.RequestPayloadLength != 0)
        {
            throw new ArgumentException(
                $"Payload length {payload.Length} does not match expected {profile.RequestPayloadLength} for {profile.Name}.",
                nameof(payload));
        }

        var buffer = new byte[HeaderLength + payload.Length];
        var span = buffer.AsSpan();

        span[0] = profile.PacketType;
        span[1] = profile.CommandCode;
        BinaryPrimitives.WriteUInt16LittleEndian(span[2..], (ushort)payload.Length);
        BinaryPrimitives.WriteUInt16LittleEndian(span[4..], xid);
        BinaryPrimitives.WriteUInt32LittleEndian(span[6..], _options.LocalControllerSn);
        BinaryPrimitives.WriteUInt32LittleEndian(span[10..], targetControllerSn);
        BinaryPrimitives.WriteUInt16LittleEndian(span[14..], 0); // flags/reserved
        span[16] = _options.DriverVersion;
        span[17] = _options.ProtocolMarker;
        BinaryPrimitives.WriteUInt16LittleEndian(span[18..], 0); // reserved

        payload.CopyTo(span[HeaderLength..]);

        // Compute CRC over everything except the final two CRC bytes.
        WriteCrc(span);

        return buffer;
    }

    internal static void WriteCrc(Span<byte> packet)
    {
        var crc = Crc16Ibm.Compute(packet[..(packet.Length - 2)]);
        BinaryPrimitives.WriteUInt16LittleEndian(packet[CrcFieldOffset..(CrcFieldOffset + 2)], crc);
    }
}

