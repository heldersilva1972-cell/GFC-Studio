using System;

namespace WG3000_COMM.Core
{
	// Token: 0x0200020D RID: 525
	internal class WGPacketWith1152_internal : WGPacket
	{
		// Token: 0x06000E9A RID: 3738 RVA: 0x00108FAC File Offset: 0x00107FAC
		public WGPacketWith1152_internal()
		{
			for (int i = 0; i < this.ucData.Length; i++)
			{
				this.ucData[i] = 0;
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00108FEC File Offset: 0x00107FEC
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

		// Token: 0x04001BC4 RID: 7108
		public byte[] ucData = new byte[1152];
	}
}
