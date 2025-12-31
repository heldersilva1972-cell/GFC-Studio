using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001D5 RID: 469
	public class MjControlTaskItem
	{
		// Token: 0x06000A77 RID: 2679 RVA: 0x000E54BD File Offset: 0x000E44BD
		public MjControlTaskItem()
		{
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x000E54C8 File Offset: 0x000E44C8
		internal MjControlTaskItem(byte[] bytControlItem)
		{
			if (bytControlItem.Length == MjControlTaskItem.byteLen)
			{
				this.m_hms = (ushort)(((int)bytControlItem[1] << 8) + (int)bytControlItem[0]);
				this.m_ymdStart = (ushort)(((int)bytControlItem[3] << 8) + (int)bytControlItem[2]);
				this.m_ymdEnd = (ushort)(((int)bytControlItem[5] << 8) + (int)bytControlItem[4]);
				this.m_weekdayControl = bytControlItem[6];
				this.m_paramValue = bytControlItem[7];
				this.m_paramLoc = (ushort)(((int)bytControlItem[9] << 8) + (int)bytControlItem[8]);
			}
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x000E5538 File Offset: 0x000E4538
		public void CopyFrom(MjControlTaskItem mjc)
		{
			byte[] array = mjc.ToBytes();
			this.m_hms = (ushort)(((int)array[1] << 8) + (int)array[0]);
			this.m_ymdStart = (ushort)(((int)array[3] << 8) + (int)array[2]);
			this.m_ymdEnd = (ushort)(((int)array[5] << 8) + (int)array[4]);
			this.m_weekdayControl = array[6];
			this.m_paramValue = array[7];
			this.m_paramLoc = (ushort)(((int)array[9] << 8) + (int)array[8]);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x000E55A0 File Offset: 0x000E45A0
		public byte[] ToBytes()
		{
			byte[] array = new byte[MjControlTaskItem.byteLen];
			Array.Copy(BitConverter.GetBytes(this.m_hms), 0, array, 0, 2);
			Array.Copy(BitConverter.GetBytes(this.m_ymdStart), 0, array, 2, 2);
			Array.Copy(BitConverter.GetBytes(this.m_ymdEnd), 0, array, 4, 2);
			Array.Copy(BitConverter.GetBytes((short)this.m_weekdayControl), 0, array, 6, 1);
			Array.Copy(BitConverter.GetBytes((short)this.m_paramValue), 0, array, 7, 1);
			Array.Copy(BitConverter.GetBytes(this.m_paramLoc), 0, array, 8, 2);
			return array;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x000E5631 File Offset: 0x000E4631
		internal static int byteLen
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x000E5635 File Offset: 0x000E4635
		public static int flashStartAddr
		{
			get
			{
				return MjControlTaskItem.flashStartAddr_internal;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x000E563C File Offset: 0x000E463C
		internal static int flashStartAddr_internal
		{
			get
			{
				return 4976640;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x000E5644 File Offset: 0x000E4644
		// (set) Token: 0x06000A7F RID: 2687 RVA: 0x000E5686 File Offset: 0x000E4686
		public DateTime hms
		{
			get
			{
				byte[] array = new byte[4];
				Array.Copy(BitConverter.GetBytes(5153), 0, array, 0, 2);
				Array.Copy(BitConverter.GetBytes(this.m_hms), 0, array, 2, 2);
				return wgTools.WgDateToMsDate(array, 0);
			}
			set
			{
				this.m_hms = (ushort)((wgTools.MsDateToWgDate(value) >> 16) & 65535U);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x000E569E File Offset: 0x000E469E
		// (set) Token: 0x06000A81 RID: 2689 RVA: 0x000E56A6 File Offset: 0x000E46A6
		public int paramLoc
		{
			get
			{
				return (int)this.m_paramLoc;
			}
			set
			{
				this.m_paramLoc = (ushort)(value & 65535);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x000E56B6 File Offset: 0x000E46B6
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x000E56BE File Offset: 0x000E46BE
		public byte paramValue
		{
			get
			{
				return this.m_paramValue;
			}
			set
			{
				this.m_paramValue = value;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x000E56C7 File Offset: 0x000E46C7
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x000E56CF File Offset: 0x000E46CF
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

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x000E56D8 File Offset: 0x000E46D8
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x000E571B File Offset: 0x000E471B
		public DateTime ymdEnd
		{
			get
			{
				byte[] array = new byte[4];
				Array.Copy(BitConverter.GetBytes(this.m_ymdEnd), 0, array, 0, 2);
				Array.Copy(BitConverter.GetBytes(this.m_hms), 0, array, 2, 2);
				return wgTools.WgDateToMsDate(array, 0);
			}
			set
			{
				this.m_ymdEnd = (ushort)(wgTools.MsDateToWgDate(value) & 65535U);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x000E5730 File Offset: 0x000E4730
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x000E5773 File Offset: 0x000E4773
		public DateTime ymdStart
		{
			get
			{
				byte[] array = new byte[4];
				Array.Copy(BitConverter.GetBytes(this.m_ymdStart), 0, array, 0, 2);
				Array.Copy(BitConverter.GetBytes(this.m_hms), 0, array, 2, 2);
				return wgTools.WgDateToMsDate(array, 0);
			}
			set
			{
				this.m_ymdStart = (ushort)(wgTools.MsDateToWgDate(value) & 65535U);
			}
		}

		// Token: 0x04001900 RID: 6400
		private ushort m_hms;

		// Token: 0x04001901 RID: 6401
		private ushort m_paramLoc;

		// Token: 0x04001902 RID: 6402
		private byte m_paramValue;

		// Token: 0x04001903 RID: 6403
		private byte m_weekdayControl;

		// Token: 0x04001904 RID: 6404
		private ushort m_ymdEnd;

		// Token: 0x04001905 RID: 6405
		private ushort m_ymdStart;
	}
}
