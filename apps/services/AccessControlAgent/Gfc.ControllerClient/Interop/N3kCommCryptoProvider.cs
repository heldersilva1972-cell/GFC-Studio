using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace Gfc.ControllerClient.Interop;

internal static class N3kCommCryptoProvider
{
    public static void Encrypt(Span<byte> payload, string password)
    {
        Transform(payload, password, NativeMethods.PacketEncrypt);
    }

    public static void Decrypt(Span<byte> payload, string password)
    {
        Transform(payload, password, NativeMethods.PacketDecrypt);
    }

    private static void Transform(Span<byte> payload, string password, NativeTransform transform)
    {
        if (string.IsNullOrWhiteSpace(password) || payload.IsEmpty)
        {
            return;
        }

        var passBytes = GetPasswordBytes(password);
        var buffer = ArrayPool<byte>.Shared.Rent(payload.Length);
        try
        {
            payload.CopyTo(buffer);
            var result = transform(passBytes, passBytes.Length, buffer, payload.Length);
            if (result != 0)
            {
                throw new InvalidOperationException($"n3k_comm.dll returned error code {result}.");
            }

            buffer.AsSpan(0, payload.Length).CopyTo(payload);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static byte[] GetPasswordBytes(string password)
    {
        var bytes = new byte[Math.Min(16, password.Length)];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)password[i];
        }

        return bytes;
    }

    private delegate int NativeTransform(byte[] password, int passwordLength, byte[] payload, int payloadLength);

    private static class NativeMethods
    {
        [DllImport("n3k_comm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int PacketEncrypt(byte[] password, int passwordLength, byte[] payload, int payloadLength);

        [DllImport("n3k_comm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int PacketDecrypt(byte[] password, int passwordLength, byte[] payload, int payloadLength);
    }
}

