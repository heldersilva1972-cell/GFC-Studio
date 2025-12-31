using System;
using System.Net;

namespace GFC.BlazorServer.Connectors.Mengqi.Configuration;

/// <summary>
///     Represents the network endpoint (UDP/TCP) and security metadata for a controller.
/// </summary>
public sealed class ControllerEndpoint
{
    public ControllerEndpoint(IPAddress address, int udpPort, int tcpPort, string? commPassword = null)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address));
        UdpPort = udpPort;
        TcpPort = tcpPort;
        CommPassword = commPassword;
    }

    public IPAddress Address { get; }

    public int UdpPort { get; }

    public int TcpPort { get; }

    public string? CommPassword { get; }
}


