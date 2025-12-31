using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001FF RID: 511
	public class WGPacketBasicPCAllowedIPSetToSend : WGPacket
	{
		// Token: 0x06000E57 RID: 3671 RVA: 0x00107A3C File Offset: 0x00106A3C
		public WGPacketBasicPCAllowedIPSetToSend()
		{
			this.m_webpassword = new byte[16];
			this.m_webpasswordNew = new byte[16];
			this.m_param4 = new byte[8];
			base.type = 32;
			base.code = 224;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00107A88 File Offset: 0x00106A88
		public WGPacketBasicPCAllowedIPSetToSend(byte cmdOption, byte param1, byte param2, byte param3, byte[] password, byte[] passwordNew, byte[] param4)
		{
			this.m_webpassword = new byte[16];
			this.m_webpasswordNew = new byte[16];
			this.m_param4 = new byte[8];
			base.type = 32;
			base.code = 224;
			this.m_cmdOption = cmdOption;
			this.m_param1 = param1;
			this.m_param2 = param2;
			this.m_param3 = param3;
			for (int i = 0; i < this.m_webpassword.Length; i++)
			{
				this.m_webpassword[i] = 0;
			}
			if (password != null && password.Length > 0)
			{
				Array.Copy(password, 0, this.m_webpassword, 0, Math.Min(password.Length, this.m_webpassword.Length));
			}
			for (int j = 0; j < this.m_webpasswordNew.Length; j++)
			{
				this.m_webpasswordNew[j] = 0;
			}
			if (passwordNew != null)
			{
				Array.Copy(passwordNew, 0, this.m_webpasswordNew, 0, Math.Min(passwordNew.Length, this.m_webpasswordNew.Length));
			}
			for (int k = 0; k < this.m_param4.Length; k++)
			{
				this.m_param4[k] = 0;
			}
			if (param4 != null)
			{
				Array.Copy(param4, 0, this.m_param4, 0, Math.Min(this.m_param4.Length, param4.Length));
			}
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00107BB8 File Offset: 0x00106BB8
		public new byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[76];
			array[0] = base.type;
			array[1] = base.code;
			Array.Copy(BitConverter.GetBytes(srcPort), 0, array, 2, 2);
			Array.Copy(BitConverter.GetBytes(this._xid), 0, array, 4, 4);
			Array.Copy(BitConverter.GetBytes(base.iDevSnFrom), 0, array, 8, 4);
			Array.Copy(BitConverter.GetBytes(base.iDevSnTo), 0, array, 12, 4);
			array[16] = base.iCallReturn;
			array[17] = this.driverVer;
			array[18] = (byte)wgTools.gPTC_internal;
			array[19] = this.reserved19;
			array[20] = this.m_cmdOption;
			array[21] = this.m_param1;
			array[22] = this.m_param2;
			array[23] = this.m_param3;
			Array.Copy(this.m_webpassword, 0, array, 24, 16);
			Array.Copy(this.m_param4, 0, array, 40, 8);
			Array.Copy(this.m_webpasswordNew, 0, array, 56, 16);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(76U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x04001B9B RID: 7067
		private const int m_len = 76;

		// Token: 0x04001B9C RID: 7068
		private byte m_cmdOption;

		// Token: 0x04001B9D RID: 7069
		private byte m_param1;

		// Token: 0x04001B9E RID: 7070
		private byte m_param2;

		// Token: 0x04001B9F RID: 7071
		private byte m_param3;

		// Token: 0x04001BA0 RID: 7072
		private byte[] m_param4;

		// Token: 0x04001BA1 RID: 7073
		private byte[] m_webpassword;

		// Token: 0x04001BA2 RID: 7074
		private byte[] m_webpasswordNew;
	}
}
