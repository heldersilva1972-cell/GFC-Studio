using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GFC.BlazorServer.ProtocolCapture.Helpers;

/// <summary>
/// Shared helpers for hex conversions and tshark output parsing.
/// </summary>
public static class HexUtils
{
    private static readonly Regex ByteRegex = new(@"\b[0-9A-Fa-f]{2}\b", RegexOptions.Compiled);

    public static string ToHexPreview(byte[] payload, int maxBytes)
    {
        if (payload.Length == 0)
        {
            return string.Empty;
        }

        var take = Math.Min(payload.Length, maxBytes);
        var sb = new StringBuilder(take * 3);
        for (var i = 0; i < take; i++)
        {
            if (i > 0)
            {
                sb.Append(' ');
            }
            sb.Append(payload[i].ToString("X2", CultureInfo.InvariantCulture));
        }
        if (payload.Length > take)
        {
            sb.Append(" …");
        }

        return sb.ToString();
    }

    public static string ToFormattedHexDump(byte[] payload)
    {
        if (payload.Length == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        var offset = 0;
        while (offset < payload.Length)
        {
            var lineBytes = payload.Skip(offset).Take(16).ToArray();
            sb.Append(offset.ToString("X4", CultureInfo.InvariantCulture));
            sb.Append(": ");
            sb.Append(string.Join(" ", lineBytes.Select(b => b.ToString("X2", CultureInfo.InvariantCulture))));
            sb.AppendLine();
            offset += lineBytes.Length;
        }

        return sb.ToString().TrimEnd();
    }

    public static List<byte[]> ParseTsharkHexDump(string hexDump)
    {
        var packets = new List<byte[]>();
        var buffer = new List<byte>();
        var lines = hexDump.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var rawLine in lines)
        {
            var line = rawLine.TrimEnd();

            if (line.StartsWith("Frame ", StringComparison.OrdinalIgnoreCase))
            {
                Flush();
                continue;
            }

            if (line.Length == 0 || !char.IsDigit(line[0]))
            {
                continue;
            }

            foreach (Match match in ByteRegex.Matches(line))
            {
                if (byte.TryParse(match.Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
                {
                    buffer.Add(value);
                }
            }
        }

        Flush();
        return packets;

        void Flush()
        {
            if (buffer.Count == 0)
            {
                return;
            }

            packets.Add(buffer.ToArray());
            buffer.Clear();
        }
    }
}

