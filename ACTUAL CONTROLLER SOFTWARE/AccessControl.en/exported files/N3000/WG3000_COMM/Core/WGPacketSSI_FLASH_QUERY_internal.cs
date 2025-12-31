using System;

namespace WG3000_COMM.Core
{
	// Token: 0x0200020A RID: 522
	internal class WGPacketSSI_FLASH_QUERY_internal : WGPacket
	{
		// Token: 0x06000E92 RID: 3730 RVA: 0x00108BBF File Offset: 0x00107BBF
		public WGPacketSSI_FLASH_QUERY_internal()
		{
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00108BC7 File Offset: 0x00107BC7
		public WGPacketSSI_FLASH_QUERY_internal(byte typePar, byte codePar, uint snToPar, uint iStartFlashAddrPar, uint iEndFlashAddrPar)
			: base(typePar, codePar, snToPar)
		{
			this.iStartFlashAddr = iStartFlashAddrPar;
			this.iEndFlashAddr = iEndFlashAddrPar;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00108BE4 File Offset: 0x00107BE4
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
			Array.Copy(BitConverter.GetBytes(this.iStartFlashAddr), 0, array, 20, 4);
			Array.Copy(BitConverter.GetBytes(this.iEndFlashAddr), 0, array, 24, 4);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(28U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x04001BC0 RID: 7104
		public uint iEndFlashAddr;

		// Token: 0x04001BC1 RID: 7105
		public uint iStartFlashAddr;
	}
}
