using System;

namespace GFC.BlazorServer.Connectors.Mengqi.Utilities;

internal static class Crc16Ibm
{
    private const ushort Polynomial = 0xA001;

    public static ushort Compute(ReadOnlySpan<byte> buffer)
    {
        // CRITICAL: Match vendor implementation (wgCRC.cs lines 44-56)
        // Must zero out bytes 2-3 before calculating CRC!
        var tempBuffer = buffer.ToArray();
        if (tempBuffer.Length > 4)
        {
            tempBuffer[2] = 0;
            tempBuffer[3] = 0;
        }
        
        // CRC-16 IBM with initial value 0 (not 0xFFFF!)
        ushort crc = 0;
        foreach (var b in tempBuffer)
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
        }

        return crc;
    }
}


