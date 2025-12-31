using System;

namespace WG3000_COMM.Core
{
	// Token: 0x0200020C RID: 524
	public class WGPacketWith1152 : WGPacket
	{
		// Token: 0x06000E97 RID: 3735 RVA: 0x00108DC0 File Offset: 0x00107DC0
		public WGPacketWith1152()
		{
			for (int i = 0; i < this.ucData.Length; i++)
			{
				this.ucData[i] = 0;
			}
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00108E00 File Offset: 0x00107E00
		public new byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[1172];
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
			Array.Copy(this.ucData, 0, array, 20, this.ucData.Length);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(1172U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00108EDC File Offset: 0x00107EDC
		public new byte[] ToBytesNoPassword(ushort srcPort)
		{
			byte[] array = new byte[1172];
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
			Array.Copy(this.ucData, 0, array, 20, this.ucData.Length);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(1172U, array)), 0, array, 2, 2);
			return array;
		}

		// Token: 0x04001BC3 RID: 7107
		public byte[] ucData = new byte[1152];
	}
}
