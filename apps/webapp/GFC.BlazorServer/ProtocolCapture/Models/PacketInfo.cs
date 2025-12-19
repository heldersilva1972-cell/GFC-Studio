namespace GFC.BlazorServer.ProtocolCapture.Models;

/// <summary>
/// Represents a parsed packet payload with UI-friendly metadata.
/// </summary>
public class PacketInfo
{
    public int Id { get; set; }
    public int Length { get; set; }
    public string Direction { get; set; } = "Unknown";
    public byte[] Payload { get; set; } = Array.Empty<byte>();
    public string HexPreview { get; set; } = string.Empty;
    public string FullHexDump { get; set; } = string.Empty;
}

