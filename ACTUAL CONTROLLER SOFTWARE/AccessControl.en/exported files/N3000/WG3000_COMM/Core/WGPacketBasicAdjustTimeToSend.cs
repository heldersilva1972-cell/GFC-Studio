using System;
using System.Globalization;

namespace WG3000_COMM.Core
{
	// Token: 0x020001FA RID: 506
	internal class WGPacketBasicAdjustTimeToSend : WGPacket
	{
		// Token: 0x06000E43 RID: 3651 RVA: 0x0010728A File Offset: 0x0010628A
		public WGPacketBasicAdjustTimeToSend(DateTime dt)
		{
			this._dt = dt;
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0010729C File Offset: 0x0010629C
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
			array[20] = byte.Parse(this._dt.ToString("yy"), NumberStyles.AllowHexSpecifier);
			array[21] = byte.Parse(this._dt.ToString("MM"), NumberStyles.AllowHexSpecifier);
			array[22] = byte.Parse(this._dt.ToString("dd"), NumberStyles.AllowHexSpecifier);
			array[23] = (byte)this._dt.DayOfWeek;
			array[24] = byte.Parse(this._dt.ToString("HH"), NumberStyles.AllowHexSpecifier);
			array[25] = byte.Parse(this._dt.ToString("mm"), NumberStyles.AllowHexSpecifier);
			array[26] = byte.Parse(this._dt.ToString("ss"), NumberStyles.AllowHexSpecifier);
			array[27] = byte.MaxValue;
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(28U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x00107426 File Offset: 0x00106426
		// (set) Token: 0x06000E46 RID: 3654 RVA: 0x0010742E File Offset: 0x0010642E
		public DateTime datetimeAdjust
		{
			get
			{
				return this._dt;
			}
			set
			{
				this._dt = value;
			}
		}

		// Token: 0x04001B83 RID: 7043
		private const int m_len = 28;

		// Token: 0x04001B84 RID: 7044
		private DateTime _dt;
	}
}
