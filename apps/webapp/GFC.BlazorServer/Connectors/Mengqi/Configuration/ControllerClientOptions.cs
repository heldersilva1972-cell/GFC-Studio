using System;
using GFC.BlazorServer.Connectors.Mengqi.Packets;

namespace GFC.BlazorServer.Connectors.Mengqi.Configuration;

/// <summary>
///     Configures low-level transport and protocol settings for the WG3000 client.
/// </summary>
public sealed class ControllerClientOptions
{
    public TimeSpan SendTimeout { get; set; } = TimeSpan.FromSeconds(5);

    public TimeSpan ReceiveTimeout { get; set; } = TimeSpan.FromSeconds(5);

    public int MaxRetries { get; set; } = 2;

    public uint LocalControllerSn { get; set; }

    public byte DriverVersion { get; set; }

    public byte ProtocolMarker { get; set; }

    public ControllerCommandProfiles CommandProfiles { get; set; } = new();
}


