namespace GFC.BlazorServer.Connectors.Mengqi.Models;

public sealed class ExtendedConfigDto
{
    public byte FirmwareMajor { get; init; }

    public byte FirmwareMinor { get; init; }

    public byte BoardType { get; init; }

    public bool AntiPassbackEnabled { get; init; }

    public bool InterlockEnabled { get; init; }
}


