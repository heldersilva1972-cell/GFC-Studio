using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000207 RID: 519
	public class WGPacketSSI_FLASH : WGPacket
	{
		// Token: 0x06000E89 RID: 3721 RVA: 0x001087BB File Offset: 0x001077BB
		public WGPacketSSI_FLASH()
		{
			this.ucData = new byte[1024];
			base.iCallReturn = 0;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x001087DC File Offset: 0x001077DC
		public WGPacketSSI_FLASH(byte[] rcvdata)
			: base(rcvdata)
		{
			this.ucData = new byte[1024];
			this.iStartFlashAddr = BitConverter.ToUInt32(rcvdata, 20);
			this.iEndFlashAddr = BitConverter.ToUInt32(rcvdata, 24);
			Array.Copy(rcvdata, 28, this.ucData, 0, this.ucData.Length);
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00108834 File Offset: 0x00107834
		public new byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[1052];
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
			Array.Copy(BitConverter.GetBytes(this.iStartFlashAddr), 0, array, 20, 4);
			Array.Copy(BitConverter.GetBytes(this.iEndFlashAddr), 0, array, 24, 4);
			Array.Copy(this.ucData, 0, array, 28, this.ucData.Length);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(1052U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x04001BB8 RID: 7096
		public uint iEndFlashAddr;

		// Token: 0x04001BB9 RID: 7097
		public uint iStartFlashAddr;

		// Token: 0x04001BBA RID: 7098
		public byte[] ucData;
	}
}
