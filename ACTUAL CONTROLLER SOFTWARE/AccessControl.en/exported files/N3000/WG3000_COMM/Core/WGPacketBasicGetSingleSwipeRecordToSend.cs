using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001FD RID: 509
	internal class WGPacketBasicGetSingleSwipeRecordToSend : WGPacket
	{
		// Token: 0x06000E4F RID: 3663 RVA: 0x00107750 File Offset: 0x00106750
		public WGPacketBasicGetSingleSwipeRecordToSend()
		{
			this.m_SwipeIndex = uint.MaxValue;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0010775F File Offset: 0x0010675F
		public WGPacketBasicGetSingleSwipeRecordToSend(uint framIndex)
		{
			this.m_SwipeIndex = uint.MaxValue;
			this.m_SwipeIndex = framIndex;
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00107778 File Offset: 0x00106778
		public new byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[24];
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
			Array.Copy(BitConverter.GetBytes(this.m_SwipeIndex), 0, array, 20, 4);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(24U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x0010784A File Offset: 0x0010684A
		// (set) Token: 0x06000E53 RID: 3667 RVA: 0x00107852 File Offset: 0x00106852
		public uint SwipeIndex
		{
			get
			{
				return this.m_SwipeIndex;
			}
			set
			{
				this.m_SwipeIndex = value;
			}
		}

		// Token: 0x04001B8F RID: 7055
		private const int m_len = 24;

		// Token: 0x04001B90 RID: 7056
		private uint m_SwipeIndex;
	}
}
