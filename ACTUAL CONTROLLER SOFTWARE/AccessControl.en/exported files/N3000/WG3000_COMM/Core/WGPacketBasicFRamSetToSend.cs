using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001FC RID: 508
	internal class WGPacketBasicFRamSetToSend : WGPacket
	{
		// Token: 0x06000E4A RID: 3658 RVA: 0x0010762C File Offset: 0x0010662C
		public WGPacketBasicFRamSetToSend()
		{
			this.m_FramIndex = uint.MaxValue;
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0010763B File Offset: 0x0010663B
		public WGPacketBasicFRamSetToSend(uint framIndex, uint newValue)
		{
			this.m_FramIndex = uint.MaxValue;
			this.m_FramIndex = framIndex;
			this.m_newValue = newValue;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00107658 File Offset: 0x00106658
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
			Array.Copy(BitConverter.GetBytes(this.m_FramIndex), 0, array, 20, 4);
			Array.Copy(BitConverter.GetBytes(this.m_newValue), 0, array, 24, 4);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(28U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0010773F File Offset: 0x0010673F
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x00107747 File Offset: 0x00106747
		public uint FramIndex
		{
			get
			{
				return this.m_FramIndex;
			}
			set
			{
				this.m_FramIndex = value;
			}
		}

		// Token: 0x04001B8C RID: 7052
		private const int m_len = 28;

		// Token: 0x04001B8D RID: 7053
		private uint m_FramIndex;

		// Token: 0x04001B8E RID: 7054
		private uint m_newValue;
	}
}
