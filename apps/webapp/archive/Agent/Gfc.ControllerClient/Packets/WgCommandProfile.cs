using System;

namespace Gfc.ControllerClient.Packets;

/// <summary>
///     Describes how to construct a specific WG3000 command (type/code + packet format + payload sizing).
///     Concrete values must be copied from the reverse-engineered WG3000_COMM code.
/// </summary>
public sealed class WgCommandProfile
{
    private WgCommandProfile(
        string name,
        byte packetType,
        byte commandCode,
        WgPacketFormat packetFormat,
        int requestPayloadLength,
        int expectedResponseLength,
        bool requiresAck)
    {
        Name = name;
        PacketType = packetType;
        CommandCode = commandCode;
        PacketFormat = packetFormat;
        RequestPayloadLength = requestPayloadLength;
        ExpectedResponseLength = expectedResponseLength;
        RequiresAck = requiresAck;
        IsConfigured = true;
    }

    private WgCommandProfile(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public byte PacketType { get; }

    public byte CommandCode { get; }

    public WgPacketFormat PacketFormat { get; }

    public int RequestPayloadLength { get; }

    public int ExpectedResponseLength { get; }

    public bool RequiresAck { get; }

    public bool IsConfigured { get; }

    public static WgCommandProfile Create(
        string name,
        byte packetType,
        byte commandCode,
        WgPacketFormat format,
        int requestPayloadLength,
        int expectedResponseLength,
        bool requiresAck = true) =>
        new(name, packetType, commandCode, format, requestPayloadLength, expectedResponseLength, requiresAck);

    public static WgCommandProfile Unconfigured(string name) => new(name);

    public void EnsureConfigured()
    {
        if (IsConfigured)
        {
            return;
        }

        throw new InvalidOperationException($"Command profile '{Name}' has not been configured with WG3000 type/code values.");
    }
}

