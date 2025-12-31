using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000201 RID: 513
	public class WGPacketBasicRunInformation4ServerToSend : WGPacket
	{
		// Token: 0x06000E67 RID: 3687 RVA: 0x00107F42 File Offset: 0x00106F42
		public WGPacketBasicRunInformation4ServerToSend()
		{
			this.m_swipeIndex = uint.MaxValue;
			this.m_timeout = 15U;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00107F59 File Offset: 0x00106F59
		public WGPacketBasicRunInformation4ServerToSend(uint swipeIndex)
		{
			this.m_swipeIndex = uint.MaxValue;
			this.m_timeout = 15U;
			this.m_swipeIndex = swipeIndex;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00107F78 File Offset: 0x00106F78
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

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x00108058 File Offset: 0x00107058
		// (set) Token: 0x06000E6B RID: 3691 RVA: 0x00108060 File Offset: 0x00107060
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

		// Token: 0x04001BA9 RID: 7081
		private const int m_len = 28;

		// Token: 0x04001BAA RID: 7082
		private uint m_swipeIndex;

		// Token: 0x04001BAB RID: 7083
		private uint m_timeout;
	}
}
