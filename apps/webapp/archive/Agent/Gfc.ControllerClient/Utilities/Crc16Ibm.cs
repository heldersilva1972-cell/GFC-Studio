using System;

namespace Gfc.ControllerClient.Utilities;

internal static class Crc16Ibm
{
    private const ushort Polynomial = 0xA001;

    public static ushort Compute(ReadOnlySpan<byte> buffer)
    {
        ushort crc = 0xFFFF;
        foreach (var b in buffer)
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

