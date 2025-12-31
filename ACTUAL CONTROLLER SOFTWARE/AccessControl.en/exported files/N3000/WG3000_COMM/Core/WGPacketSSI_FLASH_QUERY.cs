using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000209 RID: 521
	public class WGPacketSSI_FLASH_QUERY : WGPacket
	{
		// Token: 0x06000E8F RID: 3727 RVA: 0x00108AB4 File Offset: 0x00107AB4
		public WGPacketSSI_FLASH_QUERY()
		{
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00108ABC File Offset: 0x00107ABC
		public WGPacketSSI_FLASH_QUERY(byte typePar, byte codePar, uint snToPar, uint iStartFlashAddrPar, uint iEndFlashAddrPar)
			: base(typePar, codePar, snToPar)
		{
			this.iStartFlashAddr = iStartFlashAddrPar;
			this.iEndFlashAddr = iEndFlashAddrPar;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00108AD8 File Offset: 0x00107AD8
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

		// Token: 0x04001BBE RID: 7102
		public uint iEndFlashAddr;

		// Token: 0x04001BBF RID: 7103
		public uint iStartFlashAddr;
	}
}
