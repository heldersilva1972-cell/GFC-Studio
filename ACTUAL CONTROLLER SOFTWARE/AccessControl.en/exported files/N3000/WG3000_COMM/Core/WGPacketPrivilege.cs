using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000205 RID: 517
	internal class WGPacketPrivilege : WGPacket
	{
		// Token: 0x06000E7F RID: 3711 RVA: 0x0010859E File Offset: 0x0010759E
		public WGPacketPrivilege()
		{
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x001085A6 File Offset: 0x001075A6
		public WGPacketPrivilege(MjRegisterCard mj)
		{
			this._mj = mj;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x001085B8 File Offset: 0x001075B8
		public new byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[44];
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
			Array.Copy(this._mj.ToBytes(), 4, array, 20, MjRegisterCard.byteLen);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(44U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0010868E File Offset: 0x0010768E
		// (set) Token: 0x06000E83 RID: 3715 RVA: 0x00108696 File Offset: 0x00107696
		public MjRegisterCard mjrc
		{
			get
			{
				return this._mj;
			}
			set
			{
				this._mj = value;
			}
		}

		// Token: 0x04001BB4 RID: 7092
		private const int m_len = 44;

		// Token: 0x04001BB5 RID: 7093
		private MjRegisterCard _mj;
	}
}
