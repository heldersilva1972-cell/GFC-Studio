using System;
using WG3000_COMM.DataOper;

namespace WG3000_COMM.Core
{
	// Token: 0x020001DC RID: 476
	public class MjRegisterCard
	{
		// Token: 0x06000AFB RID: 2811 RVA: 0x000E9BB8 File Offset: 0x000E8BB8
		public MjRegisterCard()
		{
			this.m_password = 345678U;
			this.m_ymdStart = new DateTime(2010, 1, 1);
			this.m_ymdEnd = new DateTime(2099, 12, 31);
			this.m_controlIndex = new byte[4];
			this.m_moreCardGroup = new byte[4];
			this.m_extfunction = 0;
			this.m_maxSwipe = 0;
			this.m_hmsEnd = DateTime.Parse("2099-12-31 23:59:59");
			this.m_oneSwipe = false;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x000E9CBC File Offset: 0x000E8CBC
		public byte ControlSegIndexGet(byte DoorNo)
		{
			if (DoorNo < 1 || DoorNo > 4)
			{
				throw new IndexOutOfRangeException();
			}
			return this.m_controlIndex[(int)(DoorNo - 1)];
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x000E9CD6 File Offset: 0x000E8CD6
		public void ControlSegIndexSet(byte DoorNo, byte ControlSegIndex)
		{
			if (DoorNo < 1 || DoorNo > 4)
			{
				throw new IndexOutOfRangeException();
			}
			this.m_controlIndex[(int)(DoorNo - 1)] = ControlSegIndex;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x000E9CF1 File Offset: 0x000E8CF1
		public bool FirstCardGet(byte DoorNo)
		{
			if (DoorNo < 1 || DoorNo > 4)
			{
				throw new IndexOutOfRangeException();
			}
			return (byte)(this.m_option & (MjRegisterCard.RegisterCardOption)(1 << (int)(DoorNo - 1))) > 0;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x000E9D18 File Offset: 0x000E8D18
		public void FirstCardSet(byte DoorNo, bool val)
		{
			if (DoorNo < 1 || DoorNo > 4)
			{
				throw new IndexOutOfRangeException();
			}
			if (val)
			{
				this.m_option |= (MjRegisterCard.RegisterCardOption)(1 << (int)(DoorNo - 1));
				return;
			}
			this.m_option &= (MjRegisterCard.RegisterCardOption)(~(MjRegisterCard.RegisterCardOption)(1 << (int)(DoorNo - 1)));
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000E9D66 File Offset: 0x000E8D66
		public byte MoreCardGroupIndexGet(byte DoorNo)
		{
			if (DoorNo < 1 || DoorNo > 4)
			{
				throw new IndexOutOfRangeException();
			}
			return this.m_moreCardGroup[(int)(DoorNo - 1)];
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x000E9D80 File Offset: 0x000E8D80
		public void MoreCardGroupIndexSet(byte DoorNo, byte moreCardGroupIndex)
		{
			if (DoorNo < 1 || DoorNo > 4 || moreCardGroupIndex >= 16)
			{
				throw new IndexOutOfRangeException();
			}
			this.m_moreCardGroup[(int)(DoorNo - 1)] = moreCardGroupIndex;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000E9DA0 File Offset: 0x000E8DA0
		public byte[] ToBytes()
		{
			byte[] array = new byte[4 + MjRegisterCard.byteLen];
			Array.Copy(BitConverter.GetBytes(this.m_flashAddr), 0, array, 0, 4);
			Array.Copy(BitConverter.GetBytes(this.m_CardId), 0, array, 4, 8);
			array[12] = (byte)this.m_option;
			Array.Copy(BitConverter.GetBytes(this.m_password), 0, array, 13, 3);
			Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDateYMD(this.m_ymdStart)), 0, array, 16, 2);
			Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDateYMD(this.m_ymdEnd)), 0, array, 18, 2);
			Array.Copy(this.m_controlIndex, 0, array, 20, 4);
			int num = (int)(this.m_moreCardGroup[0] & 15);
			num += (int)(this.m_moreCardGroup[1] & 15) << 4;
			num += (int)(this.m_moreCardGroup[2] & 15) << 8;
			num += (int)(this.m_moreCardGroup[3] & 15) << 12;
			Array.Copy(BitConverter.GetBytes(num), 0, array, 24, 2);
			if (this.m_extfunction == 1)
			{
				int num2 = (int)(wgTools.MsDateToWgDateHMS(this.m_hmsEnd) >> 5);
				array[26] = (byte)(num2 & 255);
				if (this.m_oneSwipe)
				{
					array[27] = (byte)(((num2 & 16128) >> 8) | (this.m_extfunction + 1 << 6));
				}
				else
				{
					array[27] = (byte)(((num2 & 16128) >> 8) | (this.m_extfunction << 6));
				}
			}
			else if (this.m_extfunction == 0)
			{
				string text = this.m_ymdEnd.ToString("HH:mm");
				if (this.m_maxSwipe == 0 && !text.Equals("00:00") && !text.Equals("23:59"))
				{
					int num3 = (int)(wgTools.MsDateToWgDateHMS(this.m_ymdEnd) >> 5);
					array[26] = (byte)(num3 & 255);
					if (this.m_oneSwipe)
					{
						array[27] = (byte)(((num3 & 16128) >> 8) | 128);
					}
					else
					{
						array[27] = (byte)(((num3 & 16128) >> 8) | 64);
					}
				}
				else
				{
					array[26] = (byte)(this.m_maxSwipe & 255);
					array[27] = (byte)(((this.m_maxSwipe & 256) >> 8) | (this.m_extfunction << 6));
				}
			}
			if (this.m_FloorControl)
			{
				Array.Copy(BitConverter.GetBytes(this.m_AllowFloors), 0, array, 21, 3);
				array[25] = (byte)((this.m_AllowFloors >> 24) & 255UL);
				array[24] = (array[24] & 15) + (byte)((this.m_AllowFloors >> 32 << 4) & 240UL);
				array[12] = (array[12] & 241) + (byte)(((this.m_AllowFloors >> 36) & 7UL) << 1);
				array[15] = (array[15] & 63) + (byte)(((this.m_AllowFloors >> 39) & 3UL) << 6);
				return array;
			}
			if (icConsumer.gTimeSecondEnabled)
			{
				int num4 = (int)(wgTools.MsDateToWgDateHMS(this.m_ymdEnd) >> 5);
				array[26] = (byte)(num4 & 255);
				if (this.m_oneSwipe)
				{
					array[27] = (byte)(((num4 & 16128) >> 8) | 128);
				}
				else
				{
					array[27] = (byte)(((num4 & 16128) >> 8) | 64);
				}
				int num5 = (int)(this.m_moreCardGroup[0] & 15);
				num5 += (int)(this.m_moreCardGroup[1] & 15) << 4;
				num5 += 32768;
				num5 += this.m_ymdEnd.Second << 8;
				if ((this.m_ymdStart.Second & 1) > 0)
				{
					num5 += 16384;
				}
				array[25] = (byte)(num5 >> 8);
				num4 = (int)wgTools.MsDateToWgDateHMS(this.m_ymdStart);
				array[22] = (byte)(num4 & 255);
				array[23] = (byte)(num4 >> 8);
			}
			return array;
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x000EA120 File Offset: 0x000E9120
		public int Update(string strRegisterCardAll)
		{
			if (string.IsNullOrEmpty(strRegisterCardAll) || strRegisterCardAll.Length != MjRegisterCard.byteLen * 2)
			{
				return -1;
			}
			byte[] array = new byte[MjRegisterCard.byteLen];
			for (int i = 0; i < MjRegisterCard.byteLen; i++)
			{
				array[i] = Convert.ToByte(strRegisterCardAll.Substring(i * 2, 2), 16);
			}
			return this.Update(array, 0U);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x000EA180 File Offset: 0x000E9180
		public int Update(byte[] bytRegisterCard, uint flashAddr)
		{
			try
			{
				if (BitConverter.ToInt64(bytRegisterCard, 0) <= 0L)
				{
					return -1;
				}
				this.m_flashAddr = flashAddr;
				this.m_CardId = BitConverter.ToInt64(bytRegisterCard, 0);
				this.m_option = (MjRegisterCard.RegisterCardOption)bytRegisterCard[8];
				this.m_password = (BitConverter.ToUInt32(bytRegisterCard, 8) >> 8) & 4194303U;
				byte[] array = new byte[] { 0, 0, 0, 0 };
				Array.Copy(bytRegisterCard, 12, array, 0, 2);
				this.m_ymdStart = wgTools.WgDateToMsDate(array, 0);
				Array.Copy(bytRegisterCard, 14, array, 0, 2);
				this.m_ymdEnd = wgTools.WgDateToMsDate(array, 0);
				Array.Copy(bytRegisterCard, 16, this.m_controlIndex, 0, 4);
				this.m_moreCardGroup[0] = bytRegisterCard[20] & 15;
				this.m_moreCardGroup[1] = (byte)(bytRegisterCard[20] >> 4);
				this.m_moreCardGroup[2] = bytRegisterCard[21] & 15;
				this.m_moreCardGroup[3] = (byte)(bytRegisterCard[21] >> 4);
				this.m_extfunction = 0;
				this.m_oneSwipe = false;
				if ((bytRegisterCard[23] & 192) > 0)
				{
					this.m_extfunction = 1;
					byte[] array2 = new byte[]
					{
						bytRegisterCard[14],
						bytRegisterCard[15],
						(byte)(((int)bytRegisterCard[22] + ((int)(bytRegisterCard[23] & 63) << 8) << 5) & 255),
						(byte)(((int)bytRegisterCard[22] + ((int)(bytRegisterCard[23] & 63) << 8) << 5 >> 8) & 255)
					};
					this.m_hmsEnd = wgTools.WgDateToMsDate(array2, 0);
					this.m_hmsEnd = DateTime.Parse(this.m_hmsEnd.ToString("yyyy-MM-dd HH:mm:59"));
					this.m_ymdEnd = DateTime.Parse(this.m_ymdEnd.ToString("yyyy-MM-dd ") + this.m_hmsEnd.ToString("HH:mm:ss"));
					if ((bytRegisterCard[23] & 192) == 128)
					{
						this.m_oneSwipe = true;
					}
				}
				else
				{
					this.m_maxSwipe = (int)bytRegisterCard[22] + ((int)bytRegisterCard[23] << 8);
				}
				this.m_AllowFloors = 0UL;
				this.m_AllowFloors += (ulong)bytRegisterCard[17];
				this.m_AllowFloors += (ulong)((long)((long)bytRegisterCard[18] << 8));
				this.m_AllowFloors += (ulong)((long)((long)bytRegisterCard[19] << 16));
				this.m_AllowFloors += (ulong)((long)((long)bytRegisterCard[21] << 24));
				this.m_AllowFloors += (ulong)((long)(bytRegisterCard[20] >> 4));
				this.m_AllowFloors += ((ulong)bytRegisterCard[8] & 14UL) >> 1 << 36;
				this.m_AllowFloors += (ulong)((long)((long)(bytRegisterCard[11] >> 6) << 7));
				return 1;
			}
			catch (Exception)
			{
			}
			return -1;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x000EA418 File Offset: 0x000E9418
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x000EA420 File Offset: 0x000E9420
		public ulong AllowFloors
		{
			get
			{
				return this.AllowFloors_internal;
			}
			set
			{
				this.AllowFloors_internal = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x000EA429 File Offset: 0x000E9429
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x000EA431 File Offset: 0x000E9431
		internal ulong AllowFloors_internal
		{
			get
			{
				return this.m_AllowFloors;
			}
			set
			{
				if (value < 2199023255552UL)
				{
					this.m_AllowFloors = value;
					this.m_FloorControl = true;
				}
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x000EA44D File Offset: 0x000E944D
		internal static int byteLen
		{
			get
			{
				return 24;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x000EA451 File Offset: 0x000E9451
		// (set) Token: 0x06000B0B RID: 2827 RVA: 0x000EA463 File Offset: 0x000E9463
		public long CardID
		{
			get
			{
				return this.m_CardId & long.MaxValue;
			}
			set
			{
				this.m_CardId = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x000EA46C File Offset: 0x000E946C
		public uint FlashAddr
		{
			get
			{
				return this.m_flashAddr;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x000EA474 File Offset: 0x000E9474
		// (set) Token: 0x06000B0E RID: 2830 RVA: 0x000EA47C File Offset: 0x000E947C
		public DateTime hmsEnd
		{
			get
			{
				return this.m_hmsEnd;
			}
			set
			{
				this.m_extfunction = 1;
				this.m_hmsEnd = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x000EA48C File Offset: 0x000E948C
		// (set) Token: 0x06000B10 RID: 2832 RVA: 0x000EA49B File Offset: 0x000E949B
		public bool IsActivated
		{
			get
			{
				return (byte)(this.m_option & MjRegisterCard.RegisterCardOption.Activate) == 0;
			}
			set
			{
				if (value)
				{
					this.m_option &= ~MjRegisterCard.RegisterCardOption.Activate;
					return;
				}
				this.m_option |= MjRegisterCard.RegisterCardOption.Activate;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x000EA4C4 File Offset: 0x000E94C4
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x000EA4D6 File Offset: 0x000E94D6
		public bool IsDeleted
		{
			get
			{
				return (byte)(this.m_option & MjRegisterCard.RegisterCardOption.NotDeleted) == 0;
			}
			set
			{
				if (value)
				{
					this.m_option &= ~MjRegisterCard.RegisterCardOption.NotDeleted;
					return;
				}
				this.m_option |= MjRegisterCard.RegisterCardOption.NotDeleted;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x000EA4FF File Offset: 0x000E94FF
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x000EA50E File Offset: 0x000E950E
		public bool IsSuperCard
		{
			get
			{
				return (byte)(this.m_option & MjRegisterCard.RegisterCardOption.SuperCard) > 0;
			}
			set
			{
				if (value)
				{
					this.m_option |= MjRegisterCard.RegisterCardOption.SuperCard;
					return;
				}
				this.m_option &= ~MjRegisterCard.RegisterCardOption.SuperCard;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x000EA537 File Offset: 0x000E9537
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x000EA53F File Offset: 0x000E953F
		public int maxSwipe
		{
			get
			{
				return this.m_maxSwipe;
			}
			set
			{
				if (value > 0)
				{
					this.m_extfunction = 0;
					this.m_maxSwipe = value & 511;
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x000EA559 File Offset: 0x000E9559
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x000EA561 File Offset: 0x000E9561
		public bool oneSwipe
		{
			get
			{
				return this.m_oneSwipe;
			}
			set
			{
				this.m_oneSwipe = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x000EA56A File Offset: 0x000E956A
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x000EA572 File Offset: 0x000E9572
		public uint Password
		{
			get
			{
				return this.m_password;
			}
			set
			{
				if (value <= 999999U)
				{
					this.m_password = value;
				}
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x000EA583 File Offset: 0x000E9583
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x000EA58C File Offset: 0x000E958C
		public byte[] userName
		{
			get
			{
				return this.m_userName;
			}
			set
			{
				for (int i = 0; i < 33; i++)
				{
					this.m_userName[i] = 0;
				}
				int num = 0;
				while (num < 32 && num < value.Length)
				{
					this.m_userName[num] = value[num];
					num++;
				}
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x000EA5CD File Offset: 0x000E95CD
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x000EA5D5 File Offset: 0x000E95D5
		public DateTime ymdEnd
		{
			get
			{
				return this.m_ymdEnd;
			}
			set
			{
				if (value > this.m_ymdMin)
				{
					this.m_ymdEnd = value;
				}
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x000EA5EC File Offset: 0x000E95EC
		// (set) Token: 0x06000B20 RID: 2848 RVA: 0x000EA5F4 File Offset: 0x000E95F4
		public DateTime ymdStart
		{
			get
			{
				return this.m_ymdStart;
			}
			set
			{
				if (value > this.m_ymdMin)
				{
					this.m_ymdStart = value;
				}
			}
		}

		// Token: 0x04001989 RID: 6537
		private ulong m_AllowFloors;

		// Token: 0x0400198A RID: 6538
		private long m_CardId;

		// Token: 0x0400198B RID: 6539
		private byte[] m_controlIndex = new byte[4];

		// Token: 0x0400198C RID: 6540
		private int m_extfunction;

		// Token: 0x0400198D RID: 6541
		private uint m_flashAddr;

		// Token: 0x0400198E RID: 6542
		private bool m_FloorControl;

		// Token: 0x0400198F RID: 6543
		private DateTime m_hmsEnd = DateTime.Parse("2099-12-31 23:59:59");

		// Token: 0x04001990 RID: 6544
		private int m_maxSwipe;

		// Token: 0x04001991 RID: 6545
		private byte[] m_moreCardGroup = new byte[4];

		// Token: 0x04001992 RID: 6546
		private bool m_oneSwipe;

		// Token: 0x04001993 RID: 6547
		private MjRegisterCard.RegisterCardOption m_option = MjRegisterCard.RegisterCardOption.NotDeleted | MjRegisterCard.RegisterCardOption.Reserved5;

		// Token: 0x04001994 RID: 6548
		private uint m_password = 345678U;

		// Token: 0x04001995 RID: 6549
		private byte[] m_userName = new byte[33];

		// Token: 0x04001996 RID: 6550
		private DateTime m_ymdEnd = new DateTime(2020, 12, 31);

		// Token: 0x04001997 RID: 6551
		private DateTime m_ymdMin = new DateTime(2001, 1, 1);

		// Token: 0x04001998 RID: 6552
		private DateTime m_ymdStart = new DateTime(2009, 1, 1);

		// Token: 0x020001DD RID: 477
		[Flags]
		private enum RegisterCardOption : byte
		{
			// Token: 0x0400199A RID: 6554
			Activate = 64,
			// Token: 0x0400199B RID: 6555
			firstCardOfDoor1 = 1,
			// Token: 0x0400199C RID: 6556
			firstCardOfDoor2 = 2,
			// Token: 0x0400199D RID: 6557
			firstCardOfDoor3 = 4,
			// Token: 0x0400199E RID: 6558
			firstCardOfDoor4 = 8,
			// Token: 0x0400199F RID: 6559
			NotDeleted = 128,
			// Token: 0x040019A0 RID: 6560
			Reserved5 = 32,
			// Token: 0x040019A1 RID: 6561
			SuperCard = 16
		}
	}
}
