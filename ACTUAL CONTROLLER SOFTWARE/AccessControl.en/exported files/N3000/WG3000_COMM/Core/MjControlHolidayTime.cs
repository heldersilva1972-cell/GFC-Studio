using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001D4 RID: 468
	public class MjControlHolidayTime
	{
		// Token: 0x06000A71 RID: 2673 RVA: 0x000E5332 File Offset: 0x000E4332
		public MjControlHolidayTime()
		{
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x000E533C File Offset: 0x000E433C
		public MjControlHolidayTime(byte[] holiday)
		{
			if (holiday.Length == MjControlHolidayTime.byteLen)
			{
				this.bForceWork = false;
				this.dtStart = wgTools.WgDateToMsDate(holiday, 0);
				this.dtEnd = wgTools.WgDateToMsDate(holiday, 4);
				if (((int)holiday[0] & MjControlHolidayTime.forceWorkBitLoc) > 0)
				{
					this.bForceWork = true;
					this.dtEnd = wgTools.WgDateToMsDate(holiday, 0);
					this.dtStart = wgTools.WgDateToMsDate(holiday, 4);
				}
			}
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x000E53A8 File Offset: 0x000E43A8
		public byte[] ToBytes()
		{
			byte[] array = new byte[MjControlHolidayTime.byteLen];
			if (this.bForceWork)
			{
				Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtEnd)), 2, array, 0, 2);
				Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtEnd)), 0, array, 2, 2);
				Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtStart)), 2, array, 4, 2);
				Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtStart)), 0, array, 6, 2);
				array[0] = array[0] | (byte)MjControlHolidayTime.forceWorkBitLoc;
				return array;
			}
			Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtStart)), 2, array, 0, 2);
			Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtStart)), 0, array, 2, 2);
			Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtEnd)), 2, array, 4, 2);
			Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtEnd)), 0, array, 6, 2);
			array[0] = array[0] & ~(byte)MjControlHolidayTime.forceWorkBitLoc;
			return array;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x000E54B0 File Offset: 0x000E44B0
		internal static int byteLen
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x000E54B3 File Offset: 0x000E44B3
		internal static int flashStartAddr_internal
		{
			get
			{
				return 4997120;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x000E54BA File Offset: 0x000E44BA
		internal static int forceWorkBitLoc
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x040018FD RID: 6397
		public bool bForceWork;

		// Token: 0x040018FE RID: 6398
		public DateTime dtEnd;

		// Token: 0x040018FF RID: 6399
		public DateTime dtStart;
	}
}
