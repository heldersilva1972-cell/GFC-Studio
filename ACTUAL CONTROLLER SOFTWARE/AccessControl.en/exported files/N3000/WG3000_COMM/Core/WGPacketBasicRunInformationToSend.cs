using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000203 RID: 515
	internal class WGPacketBasicRunInformationToSend : WGPacket
	{
		// Token: 0x06000E71 RID: 3697 RVA: 0x00108191 File Offset: 0x00107191
		public WGPacketBasicRunInformationToSend()
		{
			this.m_swipeIndex = uint.MaxValue;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x001081A0 File Offset: 0x001071A0
		public WGPacketBasicRunInformationToSend(uint swipeIndex)
		{
			this.m_swipeIndex = uint.MaxValue;
			this.m_swipeIndex = swipeIndex;
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x001081B8 File Offset: 0x001071B8
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
			Array.Copy(BitConverter.GetBytes(this.m_swipeIndex), 0, array, 20, 4);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(24U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0010828A File Offset: 0x0010728A
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x00108292 File Offset: 0x00107292
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

		// Token: 0x04001BAF RID: 7087
		private const int m_len = 24;

		// Token: 0x04001BB0 RID: 7088
		private uint m_swipeIndex;
	}
}
