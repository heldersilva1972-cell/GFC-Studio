using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000206 RID: 518
	internal class WGPacketPrivilegeWithUserName : WGPacket
	{
		// Token: 0x06000E84 RID: 3716 RVA: 0x0010869F File Offset: 0x0010769F
		public WGPacketPrivilegeWithUserName()
		{
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x001086A7 File Offset: 0x001076A7
		public WGPacketPrivilegeWithUserName(MjRegisterCard mj)
		{
			this._mj = mj;
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x001086B8 File Offset: 0x001076B8
		public new byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[1044];
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
			Array.Copy(this._mj.userName, 0, array, 44, 32);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(1044U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x001087AA File Offset: 0x001077AA
		// (set) Token: 0x06000E88 RID: 3720 RVA: 0x001087B2 File Offset: 0x001077B2
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

		// Token: 0x04001BB6 RID: 7094
		private const int m_len = 1044;

		// Token: 0x04001BB7 RID: 7095
		private MjRegisterCard _mj;
	}
}
