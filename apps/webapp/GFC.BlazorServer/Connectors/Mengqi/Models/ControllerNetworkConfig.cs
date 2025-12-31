using System.Net;

namespace GFC.BlazorServer.Connectors.Mengqi.Models;

public sealed class ControllerNetworkConfig
{
    public IPAddress IpAddress { get; set; } = IPAddress.None;

    public IPAddress SubnetMask { get; set; } = IPAddress.None;

    public IPAddress Gateway { get; set; } = IPAddress.None;

    public bool DhcpEnabled { get; set; }

    public int UdpPort { get; set; }

    public int TcpPort { get; set; }
}


