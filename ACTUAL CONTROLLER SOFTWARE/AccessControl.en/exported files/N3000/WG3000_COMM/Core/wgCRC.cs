using System;
using System.Runtime.InteropServices;

namespace WG3000_COMM.Core
{
	// Token: 0x020001EF RID: 495
	internal class wgCRC
	{
		// Token: 0x06000C65 RID: 3173 RVA: 0x000FA234 File Offset: 0x000F9234
		public static uint calFRamCrc4Bit(uint val)
		{
			uint num = val & 268435455U;
			byte[] array = new byte[]
			{
				(byte)(num & 255U),
				(byte)((num >> 8) & 255U),
				(byte)((num >> 16) & 255U),
				(byte)((num >> 24) & 255U)
			};
			uint num2 = 3U;
			for (uint num3 = 0U; num3 < 4U; num3 += 1U)
			{
				num2 ^= (uint)((uint)array[(int)((UIntPtr)num3)] << 8);
				for (uint num4 = 0U; num4 < 8U; num4 += 1U)
				{
					if ((num2 & 32768U) > 0U)
					{
						num2 = (num2 << 1) ^ 9U;
					}
					else
					{
						num2 <<= 1;
					}
				}
			}
			return num | (num2 << 28);
		}

		// Token: 0x06000C66 RID: 3174
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int CRC_16_IBM(int len, byte[] data);

		// Token: 0x06000C67 RID: 3175 RVA: 0x000FA2D0 File Offset: 0x000F92D0
		public static ushort CRC_16_IBM_CSharp(uint len, byte[] data)
		{
			uint num = 0U;
			if (len > 4U)
			{
				byte[] array = new byte[len];
				Array.Copy(data, array, (long)((ulong)len));
				array[2] = 0;
				array[3] = 0;
				num = (uint)wgCRC.CRC_16_IBM((int)len, array);
			}
			return (ushort)(num & 65535U);
		}
	}
}
