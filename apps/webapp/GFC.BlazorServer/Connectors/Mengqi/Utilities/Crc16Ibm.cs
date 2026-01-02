using System;

namespace GFC.BlazorServer.Connectors.Mengqi.Utilities;

internal static class Crc16Ibm
{
    private const ushort Polynomial = 0xA001;

    public static ushort Compute(ReadOnlySpan<byte> buffer)
    {
        // CRITICAL: Match vendor implementation for legacy N3000/Mengqi controllers.
        // The legacy CRC-16 calculation range MUST EXCLUDE bytes 2 and 3 (the checksum placeholder).
        
        ushort crc = 0;

        // Process bytes 0 and 1
        for (int i = 0; i < 2 && i < buffer.Length; i++)
        {
            crc = UpdateCrc(crc, buffer[i]);
        }

        // Process bytes 4 through end
        for (int i = 4; i < buffer.Length; i++)
        {
            crc = UpdateCrc(crc, buffer[i]);
        }

        return crc;
    }

    /// <summary>
    /// Computes CRC over the entire buffer (including bytes 2 and 3). 
    /// Used for SSI protocol where bytes 2-3 are pre-populated with salt (srcPort).
    /// </summary>
    public static ushort ComputeSsi(ReadOnlySpan<byte> buffer)
    {
        ushort crc = 0;
        for (int i = 0; i < buffer.Length; i++)
        {
            crc = UpdateCrc(crc, buffer[i]);
        }
        return crc;
    }

    private static ushort UpdateCrc(ushort crc, byte b)
    {
        crc ^= b;
        for (var i = 0; i < 8; i++)
        {
            if ((crc & 1) != 0)
            {
                crc = (ushort)((crc >> 1) ^ Polynomial);
            }
            else
            {
                crc >>= 1;
            }
        }
        return crc;
    }
}


