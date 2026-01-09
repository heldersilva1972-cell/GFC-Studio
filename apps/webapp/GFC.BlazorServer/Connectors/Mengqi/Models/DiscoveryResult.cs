using System;

namespace GFC.BlazorServer.Connectors.Mengqi.Models;

public sealed class DiscoveryResult
{
    public uint SerialNumber { get; init; }
    public string IpAddress { get; init; } = string.Empty;
    public string SubnetMask { get; init; } = string.Empty;
    public string Gateway { get; init; } = string.Empty;
    public string MacAddress { get; init; } = string.Empty;
    public string FirmwareVersion { get; init; } = string.Empty;
    public DateTime? ControllerDate { get; init; }
    public int Port { get; init; }
    public string AllowedPcIp { get; init; } = string.Empty;
    public byte[] DoorModes { get; init; } = new byte[4];
    public byte[] DoorDelays { get; init; } = new byte[4];
    public byte[] Interlocks { get; init; } = new byte[4];
    public byte[] DoorAjarTimeouts { get; init; } = new byte[4];
    public byte[] Verifications { get; init; } = new byte[4];
}
