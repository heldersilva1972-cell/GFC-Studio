using System;

namespace WG3000_COMM.Core
{
	// Token: 0x0200020B RID: 523
	internal class WGPacketWith1024 : WGPacket
	{
		// Token: 0x06000E95 RID: 3733 RVA: 0x00108CCC File Offset: 0x00107CCC
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
			Array.Copy(this.ucData, 0, array, 20, this.ucData.Length);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(1044U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x04001BC2 RID: 7106
		public byte[] ucData = new byte[1024];
	}
}
