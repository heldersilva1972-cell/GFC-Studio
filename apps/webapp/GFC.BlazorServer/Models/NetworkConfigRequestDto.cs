namespace GFC.BlazorServer.Models;

/// <summary>
/// DTO for setting network configuration on a controller.
/// </summary>
public class NetworkConfigRequestDto
{
    public string? IpAddress { get; set; }
    public string? SubnetMask { get; set; }
    public string? Gateway { get; set; }
    public int? Port { get; set; }
    public bool DhcpEnabled { get; set; }
}

/// <summary>
/// DTO for setting allowed PC IP and communication password.
/// </summary>
public class AllowedPcAndPasswordRequestDto
{
    public string? AllowedPcIp { get; set; }
    public string? CommPassword { get; set; } // Only sent if password is being changed
}
