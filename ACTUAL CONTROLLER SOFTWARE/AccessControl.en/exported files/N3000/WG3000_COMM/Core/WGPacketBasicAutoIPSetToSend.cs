using System;
using System.Net;

namespace WG3000_COMM.Core
{
	// Token: 0x020001FB RID: 507
	internal class WGPacketBasicAutoIPSetToSend : WGPacket
	{
		// Token: 0x06000E47 RID: 3655 RVA: 0x00107437 File Offset: 0x00106437
		public WGPacketBasicAutoIPSetToSend()
		{
			this.m_ip = "0.0.0.0";
			this.m_mask = "255.0.0.0";
			this.m_gateway = "0.0.0.0";
			this.m_ipstart = 123U;
			this.m_ipend = 253U;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00107474 File Offset: 0x00106474
		public WGPacketBasicAutoIPSetToSend(uint cmdOption, string ip, string mask, string gateway, uint ipstart, uint ipend)
		{
			this.m_ip = "0.0.0.0";
			this.m_mask = "255.0.0.0";
			this.m_gateway = "0.0.0.0";
			this.m_ipstart = 123U;
			this.m_ipend = 253U;
			this.m_cmdOption = cmdOption;
			this.m_ip = ip;
			this.m_mask = mask;
			this.m_gateway = gateway;
			this.m_ipstart = ipstart;
			this.m_ipend = ipend;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x001074E8 File Offset: 0x001064E8
		public new byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[40];
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
			Array.Copy(BitConverter.GetBytes(this.m_cmdOption), 0, array, 20, 4);
			IPAddress.Parse(this.m_ip).GetAddressBytes().CopyTo(array, 24);
			IPAddress.Parse(this.m_mask).GetAddressBytes().CopyTo(array, 28);
			IPAddress.Parse(this.m_gateway).GetAddressBytes().CopyTo(array, 32);
			Array.Copy(BitConverter.GetBytes(this.m_ipstart), 0, array, 38, 1);
			Array.Copy(BitConverter.GetBytes(this.m_ipend), 0, array, 39, 1);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(40U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x04001B85 RID: 7045
		private const int m_len = 40;

		// Token: 0x04001B86 RID: 7046
		private uint m_cmdOption;

		// Token: 0x04001B87 RID: 7047
		private string m_gateway;

		// Token: 0x04001B88 RID: 7048
		private string m_ip;

		// Token: 0x04001B89 RID: 7049
		private uint m_ipend;

		// Token: 0x04001B8A RID: 7050
		private uint m_ipstart;

		// Token: 0x04001B8B RID: 7051
		private string m_mask;
	}
}
