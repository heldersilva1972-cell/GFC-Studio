namespace GFC.BlazorServer.Models;

/// <summary>
/// Network configuration for a controller.
/// </summary>
public class NetworkConfigDto
{
    public string? IpAddress { get; set; }
    public int? Port { get; set; }
    public string? SubnetMask { get; set; }
    public string? Gateway { get; set; }
    public bool DhcpEnabled { get; set; }
}
