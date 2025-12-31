using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001D6 RID: 470
	public class MjControlTimeSeg
	{
		// Token: 0x06000A8A RID: 2698 RVA: 0x000E5788 File Offset: 0x000E4788
		private DateTime GetHms(ushort curhms)
		{
			byte[] array = new byte[4];
			Array.Copy(BitConverter.GetBytes(5153), 0, array, 0, 2);
			Array.Copy(BitConverter.GetBytes(curhms), 0, array, 2, 2);
			return wgTools.WgDateToMsDate(array, 0);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x000E57C5 File Offset: 0x000E47C5
		private ushort SetHms(DateTime tm)
		{
			return (ushort)((wgTools.MsDateToWgDate(tm) >> 16) & 65504U);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x000E57D8 File Offset: 0x000E47D8
		public byte[] ToBytes()
		{
			byte[] array = new byte[MjControlTimeSeg.byteLen];
			for (int i = 0; i < MjControlTimeSeg.byteLen; i++)
			{
				array[i] = byte.MaxValue;
			}
			array[0] = this.m_weekdayControl;
			array[1] = this.m_nextSeg;
			array[2] = (byte)(this.m_LimittedMode & 255);
			array[3] = (byte)(this.m_TimeSegLimittedAccess & 255);
			ushort num = this.m_hmsStart1 & 65504;
			Array.Copy(BitConverter.GetBytes(num), 0, array, 4, 2);
			num = this.m_hmsEnd1 & 65504;
			Array.Copy(BitConverter.GetBytes(num), 0, array, 6, 2);
			num = this.m_hmsStart2 & 65504;
			Array.Copy(BitConverter.GetBytes(num), 0, array, 8, 2);
			num = this.m_hmsEnd2 & 65504;
			Array.Copy(BitConverter.GetBytes(num), 0, array, 10, 2);
			num = this.m_hmsStart3 & 65504;
			Array.Copy(BitConverter.GetBytes(num), 0, array, 12, 2);
			num = this.m_hmsEnd3 & 65504;
			Array.Copy(BitConverter.GetBytes(num), 0, array, 14, 2);
			Array.Copy(BitConverter.GetBytes(this.m_ymdStart), 0, array, 16, 2);
			Array.Copy(BitConverter.GetBytes(this.m_ymdEnd), 0, array, 18, 2);
			array[20] = (byte)(this.m_LimittedAccess1 & 255);
			array[21] = (byte)(this.m_LimittedAccess2 & 255);
			array[22] = (byte)(this.m_LimittedAccess3 & 255);
			array[23] = this.m_ControlByHoliday;
			array[24] = (byte)(this.m_TimeSegMonthLimittedAccess & 255);
			return array;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x000E5964 File Offset: 0x000E4964
		internal static int byteLen
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x000E5968 File Offset: 0x000E4968
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x000E5970 File Offset: 0x000E4970
		public byte ControlByHoliday
		{
			get
			{
				return this.m_ControlByHoliday;
			}
			set
			{
				this.m_ControlByHoliday = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x000E5979 File Offset: 0x000E4979
		internal static int flashStartAddr
		{
			get
			{
				return 4968448;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x000E5980 File Offset: 0x000E4980
		// (set) Token: 0x06000A92 RID: 2706 RVA: 0x000E598E File Offset: 0x000E498E
		public DateTime hmsEnd1
		{
			get
			{
				return this.GetHms(this.m_hmsEnd1);
			}
			set
			{
				this.m_hmsEnd1 = this.SetHms(value);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x000E599D File Offset: 0x000E499D
		// (set) Token: 0x06000A94 RID: 2708 RVA: 0x000E59AB File Offset: 0x000E49AB
		public DateTime hmsEnd2
		{
			get
			{
				return this.GetHms(this.m_hmsEnd2);
			}
			set
			{
				this.m_hmsEnd2 = this.SetHms(value);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x000E59BA File Offset: 0x000E49BA
		// (set) Token: 0x06000A96 RID: 2710 RVA: 0x000E59C8 File Offset: 0x000E49C8
		public DateTime hmsEnd3
		{
			get
			{
				return this.GetHms(this.m_hmsEnd3);
			}
			set
			{
				this.m_hmsEnd3 = this.SetHms(value);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x000E59D7 File Offset: 0x000E49D7
		// (set) Token: 0x06000A98 RID: 2712 RVA: 0x000E59E5 File Offset: 0x000E49E5
		public DateTime hmsStart1
		{
			get
			{
				return this.GetHms(this.m_hmsStart1);
			}
			set
			{
				this.m_hmsStart1 = this.SetHms(value);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x000E59F4 File Offset: 0x000E49F4
		// (set) Token: 0x06000A9A RID: 2714 RVA: 0x000E5A02 File Offset: 0x000E4A02
		public DateTime hmsStart2
		{
			get
			{
				return this.GetHms(this.m_hmsStart2);
			}
			set
			{
				this.m_hmsStart2 = this.SetHms(value);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x000E5A11 File Offset: 0x000E4A11
		// (set) Token: 0x06000A9C RID: 2716 RVA: 0x000E5A1F File Offset: 0x000E4A1F
		public DateTime hmsStart3
		{
			get
			{
				return this.GetHms(this.m_hmsStart3);
			}
			set
			{
				this.m_hmsStart3 = this.SetHms(value);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x000E5A2E File Offset: 0x000E4A2E
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x000E5A36 File Offset: 0x000E4A36
		public int LimittedAccess1
		{
			get
			{
				return this.m_LimittedAccess1;
			}
			set
			{
				if ((value >= 0) & (value <= 31))
				{
					this.m_LimittedAccess1 = value;
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x000E5A51 File Offset: 0x000E4A51
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x000E5A59 File Offset: 0x000E4A59
		public int LimittedAccess2
		{
			get
			{
				return this.m_LimittedAccess2;
			}
			set
			{
				if ((value >= 0) & (value <= 31))
				{
					this.m_LimittedAccess2 = value;
				}
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x000E5A74 File Offset: 0x000E4A74
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x000E5A7C File Offset: 0x000E4A7C
		public int LimittedAccess3
		{
			get
			{
				return this.m_LimittedAccess3;
			}
			set
			{
				if ((value >= 0) & (value <= 31))
				{
					this.m_LimittedAccess3 = value;
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x000E5A97 File Offset: 0x000E4A97
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x000E5A9F File Offset: 0x000E4A9F
		public int LimittedMode
		{
			get
			{
				return this.m_LimittedMode;
			}
			set
			{
				if (value == 0 || value == 1)
				{
					this.m_LimittedMode = value;
				}
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x000E5AAF File Offset: 0x000E4AAF
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x000E5AB7 File Offset: 0x000E4AB7
		public int MonthLimittedAccess
		{
			get
			{
				return this.m_TimeSegMonthLimittedAccess;
			}
			set
			{
				if ((value >= 0) & (value <= 254))
				{
					this.m_TimeSegMonthLimittedAccess = value;
				}
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x000E5AD5 File Offset: 0x000E4AD5
		// (set) Token: 0x06000AA8 RID: 2728 RVA: 0x000E5ADD File Offset: 0x000E4ADD
		public byte nextSeg
		{
			get
			{
				return this.m_nextSeg;
			}
			set
			{
				this.m_nextSeg = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x000E5AE6 File Offset: 0x000E4AE6
		// (set) Token: 0x06000AAA RID: 2730 RVA: 0x000E5AEE File Offset: 0x000E4AEE
		public byte SegIndex
		{
			get
			{
				return this._SegIndex;
			}
			set
			{
				this._SegIndex = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x000E5AF7 File Offset: 0x000E4AF7
		// (set) Token: 0x06000AAC RID: 2732 RVA: 0x000E5AFF File Offset: 0x000E4AFF
		public int TotalLimittedAccess
		{
			get
			{
				return this.m_TimeSegLimittedAccess;
			}
			set
			{
				if ((value >= 0) & (value <= 254))
				{
					this.m_TimeSegLimittedAccess = value;
				}
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x000E5B1D File Offset: 0x000E4B1D
		// (set) Token: 0x06000AAE RID: 2734 RVA: 0x000E5B25 File Offset: 0x000E4B25
		public byte weekdayControl
		{
			get
			{
				return this.m_weekdayControl;
			}
			set
			{
				this.m_weekdayControl = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x000E5B30 File Offset: 0x000E4B30
		// (set) Token: 0x06000AB0 RID: 2736 RVA: 0x000E5B67 File Offset: 0x000E4B67
		public DateTime ymdEnd
		{
			get
			{
				byte[] array = new byte[4];
				Array.Copy(BitConverter.GetBytes(this.m_ymdEnd), 0, array, 0, 2);
				array[2] = 0;
				array[3] = 0;
				return wgTools.WgDateToMsDate(array, 0);
			}
			set
			{
				this.m_ymdEnd = (ushort)(wgTools.MsDateToWgDate(value) & 65535U);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x000E5B7C File Offset: 0x000E4B7C
		// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x000E5BB3 File Offset: 0x000E4BB3
		public DateTime ymdStart
		{
			get
			{
				byte[] array = new byte[4];
				Array.Copy(BitConverter.GetBytes(this.m_ymdStart), 0, array, 0, 2);
				array[2] = 0;
				array[3] = 0;
				return wgTools.WgDateToMsDate(array, 0);
			}
			set
			{
				this.m_ymdStart = (ushort)(wgTools.MsDateToWgDate(value) & 65535U);
			}
		}

		// Token: 0x04001906 RID: 6406
		private byte _SegIndex;

		// Token: 0x04001907 RID: 6407
		private byte m_ControlByHoliday = 1;

		// Token: 0x04001908 RID: 6408
		private ushort m_hmsEnd1;

		// Token: 0x04001909 RID: 6409
		private ushort m_hmsEnd2;

		// Token: 0x0400190A RID: 6410
		private ushort m_hmsEnd3;

		// Token: 0x0400190B RID: 6411
		private ushort m_hmsStart1;

		// Token: 0x0400190C RID: 6412
		private ushort m_hmsStart2;

		// Token: 0x0400190D RID: 6413
		private ushort m_hmsStart3;

		// Token: 0x0400190E RID: 6414
		private int m_LimittedAccess1;

		// Token: 0x0400190F RID: 6415
		private int m_LimittedAccess2;

		// Token: 0x04001910 RID: 6416
		private int m_LimittedAccess3;

		// Token: 0x04001911 RID: 6417
		private int m_LimittedMode;

		// Token: 0x04001912 RID: 6418
		private byte m_nextSeg;

		// Token: 0x04001913 RID: 6419
		private byte[] m_reserved1 = new byte[2];

		// Token: 0x04001914 RID: 6420
		private byte[] m_reserved2 = new byte[10];

		// Token: 0x04001915 RID: 6421
		private int m_TimeSegLimittedAccess;

		// Token: 0x04001916 RID: 6422
		private int m_TimeSegMonthLimittedAccess;

		// Token: 0x04001917 RID: 6423
		private byte m_weekdayControl = byte.MaxValue;

		// Token: 0x04001918 RID: 6424
		private ushort m_ymdEnd;

		// Token: 0x04001919 RID: 6425
		private ushort m_ymdStart;
	}
}
