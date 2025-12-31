using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001FE RID: 510
	public class WGPacketBasicLoginVisitControlSetToSend : WGPacket
	{
		// Token: 0x06000E54 RID: 3668 RVA: 0x0010785B File Offset: 0x0010685B
		public WGPacketBasicLoginVisitControlSetToSend()
		{
			base.type = 32;
			base.code = 227;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00107878 File Offset: 0x00106878
		public WGPacketBasicLoginVisitControlSetToSend(byte cmdOption, int loginPassword, int loginManagePassword, int loginPasswordNew, int loginManagePasswordNew, int loginID)
		{
			base.type = 32;
			base.code = 227;
			this.m_cmdOption = cmdOption;
			this.m_loginPassword = loginPassword;
			this.m_loginManagePassword = loginManagePassword;
			this.m_loginPasswordNew = loginPasswordNew;
			this.m_loginManagePasswordNew = loginManagePasswordNew;
			this.m_loginID = loginID;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x001078CC File Offset: 0x001068CC
		public new byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[76];
			array[0] = base.type;
			array[1] = base.code;
			Array.Copy(BitConverter.GetBytes(srcPort), 0, array, 2, 2);
			Array.Copy(BitConverter.GetBytes(this._xid), 0, array, 4, 4);
			Array.Copy(BitConverter.GetBytes(base.iDevSnFrom), 0, array, 8, 4);
			Array.Copy(BitConverter.GetBytes(base.iDevSnTo), 0, array, 12, 4);
			base.iCallReturn_loginID = (byte)this.m_loginID;
			array[16] = base.iCallReturn;
			array[17] = this.driverVer;
			array[18] = (byte)wgTools.gPTC_internal;
			array[19] = this.reserved19;
			array[20] = this.m_cmdOption;
			array[21] = this.m_param1;
			array[22] = this.m_param2;
			array[23] = this.m_param3;
			Array.Copy(BitConverter.GetBytes(this.m_loginPassword), 0, array, 24, 4);
			Array.Copy(BitConverter.GetBytes(this.m_loginManagePassword), 0, array, 28, 4);
			Array.Copy(BitConverter.GetBytes(this.m_loginPasswordNew), 0, array, 32, 4);
			Array.Copy(BitConverter.GetBytes(this.m_loginManagePasswordNew), 0, array, 36, 4);
			Array.Copy(BitConverter.GetBytes(this.m_loginPasswordNew), 0, array, 40, 4);
			Array.Copy(BitConverter.GetBytes(this.m_loginManagePasswordNew), 0, array, 44, 4);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(76U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x04001B91 RID: 7057
		private const int m_len = 76;

		// Token: 0x04001B92 RID: 7058
		private byte m_cmdOption;

		// Token: 0x04001B93 RID: 7059
		private int m_loginID;

		// Token: 0x04001B94 RID: 7060
		private int m_loginManagePassword;

		// Token: 0x04001B95 RID: 7061
		private int m_loginManagePasswordNew;

		// Token: 0x04001B96 RID: 7062
		private int m_loginPassword;

		// Token: 0x04001B97 RID: 7063
		private int m_loginPasswordNew;

		// Token: 0x04001B98 RID: 7064
		private byte m_param1;

		// Token: 0x04001B99 RID: 7065
		private byte m_param2;

		// Token: 0x04001B9A RID: 7066
		private byte m_param3;
	}
}
