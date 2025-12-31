using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000202 RID: 514
	internal class WGPacketBasicRunInformation4ServerToSend_internal : WGPacket
	{
		// Token: 0x06000E6C RID: 3692 RVA: 0x00108069 File Offset: 0x00107069
		public WGPacketBasicRunInformation4ServerToSend_internal()
		{
			this.m_swipeIndex = uint.MaxValue;
			this.m_timeout = 15U;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00108080 File Offset: 0x00107080
		public WGPacketBasicRunInformation4ServerToSend_internal(uint swipeIndex)
		{
			this.m_swipeIndex = uint.MaxValue;
			this.m_timeout = 15U;
			this.m_swipeIndex = swipeIndex;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x001080A0 File Offset: 0x001070A0
		public byte[] ToBytes()
		{
			byte[] array = new byte[28];
			array[0] = base.type;
			array[1] = base.code;
			array[2] = 0;
			array[3] = 0;
			Array.Copy(BitConverter.GetBytes(this._xid), 0, array, 4, 4);
			Array.Copy(BitConverter.GetBytes(base.iDevSnFrom), 0, array, 8, 4);
			Array.Copy(BitConverter.GetBytes(base.iDevSnTo), 0, array, 12, 4);
			array[16] = base.iCallReturn;
			array[17] = this.driverVer;
			array[18] = (byte)wgTools.gPTC_internal;
			array[19] = this.reserved19;
			Array.Copy(BitConverter.GetBytes(this.m_swipeIndex), 0, array, 20, 4);
			Array.Copy(BitConverter.GetBytes(this.m_timeout), 0, array, 24, 4);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(28U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x00108180 File Offset: 0x00107180
		// (set) Token: 0x06000E70 RID: 3696 RVA: 0x00108188 File Offset: 0x00107188
		public uint swipeIndexToRead
		{
			get
			{
				return this.m_swipeIndex;
			}
			set
			{
				this.m_swipeIndex = value;
			}
		}

		// Token: 0x04001BAC RID: 7084
		private const int m_len = 28;

		// Token: 0x04001BAD RID: 7085
		private uint m_swipeIndex;

		// Token: 0x04001BAE RID: 7086
		private uint m_timeout;
	}
}
