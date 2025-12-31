using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000204 RID: 516
	internal class WGPacketCPU_CONFIG_READToSend : WGPacket
	{
		// Token: 0x06000E76 RID: 3702 RVA: 0x0010829B File Offset: 0x0010729B
		public WGPacketCPU_CONFIG_READToSend()
		{
			this.m_startAddr = uint.MaxValue;
			this.m_datalen = 1024U;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x001082B5 File Offset: 0x001072B5
		public WGPacketCPU_CONFIG_READToSend(uint startAddr, uint datalen)
		{
			this.m_startAddr = uint.MaxValue;
			this.m_datalen = 1024U;
			this.m_startAddr = startAddr;
			this.m_datalen = datalen;
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x001082E0 File Offset: 0x001072E0
		public new byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[28];
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
			Array.Copy(BitConverter.GetBytes(this.m_startAddr), 0, array, 20, 4);
			Array.Copy(BitConverter.GetBytes(this.m_datalen), 0, array, 24, 4);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(28U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x001083C8 File Offset: 0x001073C8
		public byte[] ToBytesCPUConfigNoPassword(ushort srcPort)
		{
			byte[] array = new byte[28];
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
			Array.Copy(BitConverter.GetBytes(this.m_startAddr), 0, array, 20, 4);
			Array.Copy(BitConverter.GetBytes(this.m_datalen), 0, array, 24, 4);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(28U, array)), 0, array, 2, 2);
			return array;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x001084A4 File Offset: 0x001074A4
		public byte[] ToBytesCPUConfigNoPassword(ushort srcPort, int i18)
		{
			byte[] array = new byte[28];
			array[0] = base.type;
			array[1] = base.code;
			Array.Copy(BitConverter.GetBytes(srcPort), 0, array, 2, 2);
			Array.Copy(BitConverter.GetBytes(this._xid), 0, array, 4, 4);
			Array.Copy(BitConverter.GetBytes(base.iDevSnFrom), 0, array, 8, 4);
			Array.Copy(BitConverter.GetBytes(base.iDevSnTo), 0, array, 12, 4);
			array[16] = base.iCallReturn;
			array[17] = this.driverVer;
			array[18] = (byte)i18;
			array[19] = this.reserved19;
			Array.Copy(BitConverter.GetBytes(this.m_startAddr), 0, array, 20, 4);
			Array.Copy(BitConverter.GetBytes(this.m_datalen), 0, array, 24, 4);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(28U, array)), 0, array, 2, 2);
			return array;
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0010857C File Offset: 0x0010757C
		// (set) Token: 0x06000E7C RID: 3708 RVA: 0x00108584 File Offset: 0x00107584
		public uint datalen
		{
			get
			{
				return this.m_datalen;
			}
			set
			{
				this.m_datalen = value;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x0010858D File Offset: 0x0010758D
		// (set) Token: 0x06000E7E RID: 3710 RVA: 0x00108595 File Offset: 0x00107595
		public uint startAddr
		{
			get
			{
				return this.m_startAddr;
			}
			set
			{
				this.m_startAddr = value;
			}
		}

		// Token: 0x04001BB1 RID: 7089
		private const int m_len = 28;

		// Token: 0x04001BB2 RID: 7090
		private uint m_datalen;

		// Token: 0x04001BB3 RID: 7091
		private uint m_startAddr;
	}
}
