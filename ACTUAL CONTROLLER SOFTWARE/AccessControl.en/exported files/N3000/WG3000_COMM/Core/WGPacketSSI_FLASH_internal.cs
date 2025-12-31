using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000208 RID: 520
	internal class WGPacketSSI_FLASH_internal : WGPacket
	{
		// Token: 0x06000E8C RID: 3724 RVA: 0x00108938 File Offset: 0x00107938
		public WGPacketSSI_FLASH_internal()
		{
			this.ucData = new byte[1024];
			base.iCallReturn = 0;
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00108958 File Offset: 0x00107958
		public WGPacketSSI_FLASH_internal(byte[] rcvdata)
			: base(rcvdata)
		{
			this.ucData = new byte[1024];
			this.iStartFlashAddr = BitConverter.ToUInt32(rcvdata, 20);
			this.iEndFlashAddr = BitConverter.ToUInt32(rcvdata, 24);
			Array.Copy(rcvdata, 28, this.ucData, 0, this.ucData.Length);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x001089B0 File Offset: 0x001079B0
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

		// Token: 0x04001BBB RID: 7099
		public uint iEndFlashAddr;

		// Token: 0x04001BBC RID: 7100
		public uint iStartFlashAddr;

		// Token: 0x04001BBD RID: 7101
		public byte[] ucData;
	}
}
