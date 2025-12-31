using System;
using System.Collections;

namespace WG3000_COMM.Core
{
	// Token: 0x020001C3 RID: 451
	public class wgMjControllerRunInformation
	{
		// Token: 0x06000965 RID: 2405 RVA: 0x000DDC4C File Offset: 0x000DCC4C
		public wgMjControllerRunInformation()
		{
			this.Reset();
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x000DDCD8 File Offset: 0x000DCCD8
		public void Clear()
		{
			this.Reset();
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x000DDCE0 File Offset: 0x000DCCE0
		public int GetDoorImageIndex(int doorNO)
		{
			int num = 0;
			if (doorNO > 0 && doorNO <= 4)
			{
				if (this.IsOpen(doorNO))
				{
					num = 2;
				}
				else
				{
					num = 1;
				}
				if (this.appError > 0)
				{
					return num + 3;
				}
				if ((this.m_warnInfo[doorNO - 1] & 16) > 0)
				{
					if (this.GetDoorImageIndex4InvalidCard(doorNO) > 0)
					{
						num += 3;
					}
					return num;
				}
				if (this.m_warnInfo[doorNO - 1] > 0)
				{
					num += 3;
				}
			}
			return num;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x000DDD48 File Offset: 0x000DCD48
		public int GetDoorImageIndex4InvalidCard(int doorNO)
		{
			if (wgMjControllerRunInformation.activeInvalidCardTimeout != 0 && wgMjControllerRunInformation.activeInvalidCardTimeout != 1 && wgMjControllerRunInformation.activeInvalidCardTimeout <= 10)
			{
				if (this.invalidCard[doorNO - 1] == null)
				{
					this.invalidCard[doorNO - 1] = new ArrayList();
				}
				if (DateTime.Now.Subtract(this.invalidCardStart[doorNO - 1]).TotalSeconds > 60.0)
				{
					this.lastWarnIndex[doorNO - 1] = 0U;
					this.invalidCardStart[doorNO - 1] = DateTime.Now;
					this.invalidCardNO[doorNO - 1] = 0L;
					if (this.invalidCard[doorNO - 1] == null)
					{
						this.invalidCard[doorNO - 1] = new ArrayList();
					}
					this.invalidCard[doorNO - 1].Clear();
				}
				int num = 0;
				while (num < 10 && this.newSwipes[num] != null)
				{
					if ((int)this.newSwipes[num].DoorNo == doorNO)
					{
						if (this.newSwipes[num].eventCategory != 1 && this.newSwipes[num].eventCategory != 3)
						{
							break;
						}
						if (this.invalidCardNO[doorNO - 1] == 0L || this.invalidCardNO[doorNO - 1] != this.newSwipes[num].CardID)
						{
							if (this.invalidCardNO[doorNO - 1] != 0L || this.lastWarnIndex[doorNO - 1] < this.newSwipes[num].IndexInDataFlash)
							{
								this.invalidCardNO[doorNO - 1] = this.newSwipes[num].CardID;
								this.invalidCard[doorNO - 1].Clear();
								this.invalidCard[doorNO - 1].Add(this.newSwipes[num].IndexInDataFlash);
								break;
							}
							break;
						}
						else
						{
							if (this.newSwipes[num].IndexInDataFlash > (uint)this.invalidCard[doorNO - 1][0] && this.invalidCard[doorNO - 1].IndexOf(this.newSwipes[num].IndexInDataFlash) < 0)
							{
								this.invalidCard[doorNO - 1].Add(this.newSwipes[num].IndexInDataFlash);
							}
							if (this.invalidCard[doorNO - 1].Count >= wgMjControllerRunInformation.activeInvalidCardTimeout)
							{
								break;
							}
						}
					}
					num++;
				}
				if (this.invalidCard[doorNO - 1].Count < wgMjControllerRunInformation.activeInvalidCardTimeout)
				{
					return 0;
				}
				this.lastWarnIndex[doorNO - 1] = (uint)this.invalidCard[doorNO - 1][0];
				for (int i = 0; i < this.invalidCard[doorNO - 1].Count; i++)
				{
					if (this.lastWarnIndex[doorNO - 1] <= (uint)this.invalidCard[doorNO - 1][i])
					{
						this.lastWarnIndex[doorNO - 1] = (uint)this.invalidCard[doorNO - 1][i];
					}
				}
				this.invalidCardStart[doorNO - 1] = DateTime.Now;
				this.invalidCardNO[doorNO - 1] = 0L;
				this.invalidCard[doorNO - 1].Clear();
			}
			return 1;
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x000DE056 File Offset: 0x000DD056
		public bool IsOpen(int doorNO)
		{
			if (doorNO <= 0 || doorNO > 4)
			{
				throw new IndexOutOfRangeException();
			}
			return ((int)this.m_pbdsStatus & (1 << 4 + doorNO - 1)) > 0;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x000DE07A File Offset: 0x000DD07A
		public uint LngFRam(int index)
		{
			if (index < 0 || index >= this.m_lngFRam.Length)
			{
				throw new IndexOutOfRangeException();
			}
			return this.m_lngFRam[index];
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x000DE099 File Offset: 0x000DD099
		public bool LockRelayActive(int doorNO)
		{
			if (doorNO <= 0 || doorNO > 4)
			{
				throw new IndexOutOfRangeException();
			}
			return ((int)this.m_lockStatus & (1 << doorNO - 1)) > 0;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x000DE0BB File Offset: 0x000DD0BB
		public bool PushButtonActive(int doorNO)
		{
			if (doorNO <= 0 || doorNO > 4)
			{
				throw new IndexOutOfRangeException();
			}
			return ((int)this.m_pbdsStatus & (1 << doorNO - 1)) > 0;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x000DE0DD File Offset: 0x000DD0DD
		public uint ReaderValidSwipeGet(int readerNo)
		{
			if (readerNo < 1 || readerNo > 4)
			{
				throw new IndexOutOfRangeException();
			}
			return this.m_lngFRam[readerNo - 1 + 4];
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000DE0FC File Offset: 0x000DD0FC
		private void Reset()
		{
			this.dtNow = DateTime.Parse("2000-1-1");
			this.weekday = 0;
			this.m_reserved1 = 0;
			this.m_wgcticks = 0U;
			this.m_swipeStartIndex = 0U;
			this.m_swipeStartAddr = 0U;
			this.m_swipeEndIndex = 0U;
			this.m_registerCardNumTotal = 0U;
			this.m_deletedRegisterCardNum = 0U;
			this.m_registerCardStartAddr = 0U;
			this.m_pbdsStatus = 0;
			this.m_lockStatus = 0;
			this.m_extendIOStatus = 0;
			this.m_appError = 0;
			this.m_warnInfo[0] = 0;
			this.m_warnInfo[1] = 0;
			this.m_warnInfo[2] = 0;
			this.m_warnInfo[3] = 0;
			for (int i = 0; i < 16; i++)
			{
				this.m_lngFRam[i] = 0U;
			}
			for (int i = 0; i < 32; i++)
			{
				this.m_reservedBytes[i] = 0;
			}
			this.m_pbdsStatusHigh = 0;
			this.driverVersion = "V";
			this.m_dtPowerOn = DateTime.Parse("2000-1-1");
			this.refreshTime = DateTime.Parse("2000-1-1");
			this.CurrentControllerSN = uint.MaxValue;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000DE1FD File Offset: 0x000DD1FD
		public void UpdateInfo(byte[] wgpktData, int startIndex, uint ControllerSN)
		{
			this.UpdateInfo_internal(wgpktData, startIndex, ControllerSN);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000DE208 File Offset: 0x000DD208
		internal void UpdateInfo_internal(byte[] wgpktData, int startIndex, uint ControllerSN)
		{
			for (int i = 0; i < 340; i++)
			{
				this.privBytes[i] = wgpktData[startIndex + i];
			}
			string text = string.Format("20{0:X2}-{1:X2}-{2:X2} {3:X2}:{4:X2}:{5:X2}", new object[]
			{
				wgpktData[startIndex],
				wgpktData[startIndex + 1],
				wgpktData[startIndex + 2],
				wgpktData[startIndex + 4],
				wgpktData[startIndex + 5],
				wgpktData[startIndex + 6]
			});
			this.dtNow = DateTime.Parse("2000-1-1");
			DateTime dateTime;
			if (DateTime.TryParse(text, out dateTime))
			{
				this.dtNow = dateTime;
			}
			this.weekday = wgpktData[startIndex + 3];
			this.m_reserved1 = wgpktData[startIndex + 7];
			this.m_wgcticks = BitConverter.ToUInt32(wgpktData, startIndex - 20 + 28);
			this.m_swipeStartIndex = BitConverter.ToUInt32(wgpktData, startIndex - 20 + 32);
			this.m_swipeStartAddr = BitConverter.ToUInt32(wgpktData, startIndex - 20 + 36);
			this.m_swipeEndIndex = BitConverter.ToUInt32(wgpktData, startIndex - 20 + 40);
			this.m_registerCardNumTotal = BitConverter.ToUInt32(wgpktData, startIndex - 20 + 44);
			this.m_deletedRegisterCardNum = BitConverter.ToUInt32(wgpktData, startIndex - 20 + 48);
			this.m_registerCardStartAddr = BitConverter.ToUInt32(wgpktData, startIndex - 20 + 52);
			this.m_pbdsStatus = wgpktData[startIndex - 20 + 56];
			this.m_lockStatus = wgpktData[startIndex - 20 + 57];
			this.m_extendIOStatus = wgpktData[startIndex - 20 + 58];
			this.m_appError = wgpktData[startIndex - 20 + 59];
			this.m_warnInfo[0] = wgpktData[startIndex - 20 + 60];
			this.m_warnInfo[1] = wgpktData[startIndex - 20 + 61];
			this.m_warnInfo[2] = wgpktData[startIndex - 20 + 62];
			this.m_warnInfo[3] = wgpktData[startIndex - 20 + 63];
			if (wgTools.gReaderBrokenWarnActive > 0)
			{
				if ((this.m_warnInfo[0] & 64) > 0 && (this.m_extendIOStatus & 64) != 0 && (this.m_extendIOStatus & 32) != 0)
				{
					this.m_warnInfo[0] = this.m_warnInfo[0] ^ 64;
				}
				else if ((this.m_warnInfo[0] & 64) > 0 && (this.m_extendIOStatus & 64) == 0 && (this.m_extendIOStatus & 32) != 0)
				{
					this.m_warnInfo[1] = this.m_warnInfo[1] | 64;
					this.m_warnInfo[0] = this.m_warnInfo[0] ^ 64;
				}
				else if ((this.m_warnInfo[0] & 64) > 0 && (this.m_extendIOStatus & 64) == 0 && (this.m_extendIOStatus & 32) == 0)
				{
					this.m_warnInfo[1] = this.m_warnInfo[1] | 64;
				}
			}
			int num = startIndex - 20 + 64;
			for (int j = 0; j < 10; j++)
			{
				if (this.newSwipes[j] == null)
				{
					this.newSwipes[j] = new wgMjControllerSwipeRecord(wgpktData, (uint)(startIndex - 20 + 68 + j * 20), ControllerSN, BitConverter.ToUInt32(wgpktData, startIndex - 20 + 64 + j * 20));
				}
				else
				{
					this.newSwipes[j].Update(wgpktData, (uint)(startIndex - 20 + 68 + j * 20), ControllerSN, BitConverter.ToUInt32(wgpktData, startIndex - 20 + 64 + j * 20));
				}
			}
			for (int k = 0; k < 16; k++)
			{
				this.m_lngFRam[k] = BitConverter.ToUInt32(wgpktData, startIndex - 20 + 264 + 4 * k);
			}
			for (int l = 0; l < 32; l++)
			{
				this.m_reservedBytes[l] = wgpktData[startIndex - 20 + 328 + l];
			}
			this.m_pbdsStatusHigh = wgpktData[startIndex - 20 + 328 + 18];
			this.driverVersion = string.Format("V{0:d}.{1:d}", wgpktData[startIndex - 20 + 17], wgpktData[startIndex - 20 + 328 + 19]);
			this.m_dtPowerOn = DateTime.Parse("2000-1-1");
			this.m_dtPowerOn = wgTools.WgDateToMsDate(wgpktData, startIndex - 20 + 328 + 20);
			this.m_totalPerson4AntibackShare = (uint)wgpktData[startIndex - 20 + 328 + 25] * 256U + (uint)wgpktData[startIndex - 20 + 328 + 24];
			this.m_multiInput40 = (ulong)this.m_totalPerson4AntibackShare;
			this.m_multiInput40 <<= 32;
			this.m_multiInput40 += (ulong)this.m_swipeStartIndex;
			this.refreshTime = DateTime.Now;
			this.CurrentControllerSN = ControllerSN;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x000DE660 File Offset: 0x000DD660
		internal void UpdateInfo_internal64(byte[] wgpktData, int startIndex, uint ControllerSN)
		{
			for (int i = 0; i < 64; i++)
			{
				this.privBytes[i] = wgpktData[startIndex + i];
			}
			string text = string.Format("20{0:X2}-{1:X2}-{2:X2} {3:X2}:{4:X2}:{5:X2}", new object[]
			{
				wgpktData[51],
				wgpktData[52],
				wgpktData[53],
				wgpktData[37],
				wgpktData[38],
				wgpktData[39]
			});
			this.dtNow = DateTime.Parse("2000-1-1");
			DateTime dateTime;
			if (DateTime.TryParse(text, out dateTime))
			{
				this.dtNow = dateTime;
			}
			this.m_wgcticks = Math.Max((uint)DateTime.Now.Ticks, 1U);
			this.m_swipeEndIndex = BitConverter.ToUInt32(wgpktData, 8);
			this.m_pbdsStatus = 0;
			if (wgpktData[28] > 0)
			{
				this.m_pbdsStatus |= 16;
			}
			if (wgpktData[29] > 0)
			{
				this.m_pbdsStatus |= 32;
			}
			if (wgpktData[30] > 0)
			{
				this.m_pbdsStatus |= 64;
			}
			if (wgpktData[31] > 0)
			{
				this.m_pbdsStatus |= 128;
			}
			if (wgpktData[32] > 0)
			{
				this.m_pbdsStatus |= 1;
			}
			if (wgpktData[33] > 0)
			{
				this.m_pbdsStatus |= 2;
			}
			if (wgpktData[34] > 0)
			{
				this.m_pbdsStatus |= 4;
			}
			if (wgpktData[35] > 0)
			{
				this.m_pbdsStatus |= 8;
			}
			this.m_lockStatus = wgpktData[49];
			this.m_appError = wgpktData[36];
			byte[] array = new byte[16];
			Array.Copy(wgpktData, 16, array, 0, 4);
			Array.Copy(wgpktData, 44, array, 4, 4);
			Array.Copy(wgpktData, 56, array, 8, 8);
			uint num = BitConverter.ToUInt32(wgpktData, 8);
			if (num > 0U)
			{
				new MjRec(array, 0U, ControllerSN, num);
				if (this.newSwipes[0] == null)
				{
					this.newSwipes[0] = new wgMjControllerSwipeRecord(array, 0U, ControllerSN, num);
				}
				else
				{
					this.newSwipes[0].Update(array, 0U, ControllerSN, num);
				}
			}
			this.m_pbdsStatusHigh = wgpktData[50];
			this.m_dtPowerOn = DateTime.Parse("2000-1-1");
			this.refreshTime = DateTime.Now;
			this.CurrentControllerSN = ControllerSN;
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x000DE8A3 File Offset: 0x000DD8A3
		internal void UpdateInfo_registercard_internal64(byte[] wgpktData, int startIndex, uint ControllerSN)
		{
			this.m_registerCardNumTotal = BitConverter.ToUInt32(wgpktData, startIndex + 8);
			this.m_deletedRegisterCardNum = 0U;
			this.CurrentControllerSN = ControllerSN;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x000DE8C2 File Offset: 0x000DD8C2
		public byte WarnInfo(int doorNo)
		{
			if (doorNo <= 0 || doorNo > 4)
			{
				throw new IndexOutOfRangeException();
			}
			return this.m_warnInfo[doorNo - 1];
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x000DE8DC File Offset: 0x000DD8DC
		public byte appError
		{
			get
			{
				return this.m_appError;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x000DE8E4 File Offset: 0x000DD8E4
		public string BytesDataStr
		{
			get
			{
				return BitConverter.ToString(this.privBytes).Replace("-", "");
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x000DE900 File Offset: 0x000DD900
		// (set) Token: 0x06000977 RID: 2423 RVA: 0x000DE908 File Offset: 0x000DD908
		public uint CurrentControllerSN
		{
			get
			{
				return this._CurrentControllerSN;
			}
			private set
			{
				this._CurrentControllerSN = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x000DE911 File Offset: 0x000DD911
		public uint deletedRegisterCardNum
		{
			get
			{
				return this.m_deletedRegisterCardNum;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x000DE919 File Offset: 0x000DD919
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x000DE921 File Offset: 0x000DD921
		public string driverVersion
		{
			get
			{
				return this._driverVersion;
			}
			private set
			{
				this._driverVersion = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x000DE92A File Offset: 0x000DD92A
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x000DE932 File Offset: 0x000DD932
		public DateTime dtNow
		{
			get
			{
				return this._dtNow;
			}
			private set
			{
				this._dtNow = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x000DE93B File Offset: 0x000DD93B
		public DateTime dtPowerOn
		{
			get
			{
				return this.m_dtPowerOn;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x000DE943 File Offset: 0x000DD943
		public byte extendIOStatus
		{
			get
			{
				return this.m_extendIOStatus;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x000DE94B File Offset: 0x000DD94B
		public bool FireIsActive
		{
			get
			{
				return (this.m_pbdsStatusHigh & 2) > 0;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x000DE958 File Offset: 0x000DD958
		public bool ForceLockIsActive
		{
			get
			{
				return (this.m_pbdsStatusHigh & 1) > 0;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x000DE968 File Offset: 0x000DD968
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x000DE9DE File Offset: 0x000DD9DE
		public uint lastGetRecordIndex
		{
			get
			{
				if (this.m_lngFRam[8] == 0U)
				{
					return 0U;
				}
				if ((this.swipeEndIndex & 4278190080U) > 0U && (this.swipeEndIndex & 4278190080U) + this.m_lngFRam[8] > this.swipeEndIndex)
				{
					return (this.swipeEndIndex & 4278190080U) - 16777216U + this.m_lngFRam[8];
				}
				return (this.swipeEndIndex & 4278190080U) + this.m_lngFRam[8];
			}
			set
			{
				this.m_lngFRam[8] = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x000DE9E9 File Offset: 0x000DD9E9
		public byte lockStatus
		{
			get
			{
				return this.m_lockStatus;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x000DE9F1 File Offset: 0x000DD9F1
		public ulong mutliInput40
		{
			get
			{
				return this.m_multiInput40;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x000DE9F9 File Offset: 0x000DD9F9
		public int netSpeedCode
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x000DE9FC File Offset: 0x000DD9FC
		public uint newRecordsNum
		{
			get
			{
				int num = 0;
				if (this.lastGetRecordIndex == 0U || (ulong)this.swipeEndIndex >= (ulong)(this.lastGetRecordIndex + 204800U) - (ulong)((long)(4096 / wgMjControllerSwipeRecord.SwipeSize)) - 1UL || this.swipeEndIndex < this.lastGetRecordIndex)
				{
					if (this.swipeEndIndex > 0U)
					{
						if (this.swipeEndIndex >= 204800U)
						{
							num = 204800 - 4096 / wgMjControllerSwipeRecord.SwipeSize + (int)((ulong)this.swipeEndIndex % (ulong)((long)(4096 / wgMjControllerSwipeRecord.SwipeSize)));
						}
						else
						{
							num = (int)this.swipeEndIndex;
						}
					}
				}
				else
				{
					num = (int)(this.swipeEndIndex - this.lastGetRecordIndex);
				}
				if (num < 0)
				{
					num = 0;
				}
				return (uint)num;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x000DEAA5 File Offset: 0x000DDAA5
		public byte pbdsStatus
		{
			get
			{
				return this.m_pbdsStatus;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x000DEAAD File Offset: 0x000DDAAD
		public byte pbdsStatusHigh
		{
			get
			{
				return this.m_pbdsStatusHigh;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x000DEAB5 File Offset: 0x000DDAB5
		public static int pktlen
		{
			get
			{
				return 360;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x000DEABC File Offset: 0x000DDABC
		// (set) Token: 0x0600098B RID: 2443 RVA: 0x000DEAC4 File Offset: 0x000DDAC4
		public DateTime refreshTime
		{
			get
			{
				return this._refreshTime;
			}
			private set
			{
				this._refreshTime = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x000DEACD File Offset: 0x000DDACD
		public uint registerCardNum
		{
			get
			{
				if (this.m_registerCardNumTotal < this.m_deletedRegisterCardNum)
				{
					return 0U;
				}
				return this.m_registerCardNumTotal - this.m_deletedRegisterCardNum;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x000DEAEC File Offset: 0x000DDAEC
		public uint registerCardNumTotal
		{
			get
			{
				return this.m_registerCardNumTotal;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x000DEAF4 File Offset: 0x000DDAF4
		public uint registerCardStartAddr
		{
			get
			{
				return this.m_registerCardStartAddr;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x000DEAFC File Offset: 0x000DDAFC
		public byte reserved1
		{
			get
			{
				return this.m_reserved1;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x000DEB04 File Offset: 0x000DDB04
		public byte[] reservedBytes
		{
			get
			{
				return this.m_reservedBytes;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x000DEB0C File Offset: 0x000DDB0C
		public uint swipeEndIndex
		{
			get
			{
				return this.m_swipeEndIndex;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x000DEB14 File Offset: 0x000DDB14
		public uint swipeStartAddr
		{
			get
			{
				return this.m_swipeStartAddr;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x000DEB1C File Offset: 0x000DDB1C
		public uint swipeStartIndex
		{
			get
			{
				return this.m_swipeStartIndex;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x000DEB24 File Offset: 0x000DDB24
		public uint totalPerson4AntibackShare
		{
			get
			{
				return this.m_totalPerson4AntibackShare;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x000DEB2C File Offset: 0x000DDB2C
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x000DEB34 File Offset: 0x000DDB34
		public byte weekday
		{
			get
			{
				return this._weekday;
			}
			private set
			{
				this._weekday = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x000DEB3D File Offset: 0x000DDB3D
		public uint wgcticks
		{
			get
			{
				return this.m_wgcticks;
			}
		}

		// Token: 0x0400184D RID: 6221
		private const int DoorMax = 4;

		// Token: 0x0400184E RID: 6222
		private const int m_len = 340;

		// Token: 0x0400184F RID: 6223
		private const uint FRamIndex4LastGetRecord = 8U;

		// Token: 0x04001850 RID: 6224
		private uint _CurrentControllerSN;

		// Token: 0x04001851 RID: 6225
		private string _driverVersion;

		// Token: 0x04001852 RID: 6226
		private DateTime _dtNow;

		// Token: 0x04001853 RID: 6227
		private DateTime _refreshTime;

		// Token: 0x04001854 RID: 6228
		private byte _weekday;

		// Token: 0x04001855 RID: 6229
		public static int activeInvalidCardTimeout;

		// Token: 0x04001856 RID: 6230
		private ArrayList[] invalidCard = new ArrayList[4];

		// Token: 0x04001857 RID: 6231
		private long[] invalidCardNO = new long[4];

		// Token: 0x04001858 RID: 6232
		private DateTime[] invalidCardStart = new DateTime[4];

		// Token: 0x04001859 RID: 6233
		private uint[] lastWarnIndex = new uint[4];

		// Token: 0x0400185A RID: 6234
		private byte m_appError;

		// Token: 0x0400185B RID: 6235
		private uint m_deletedRegisterCardNum;

		// Token: 0x0400185C RID: 6236
		private DateTime m_dtPowerOn;

		// Token: 0x0400185D RID: 6237
		private byte m_extendIOStatus;

		// Token: 0x0400185E RID: 6238
		private uint[] m_lngFRam = new uint[16];

		// Token: 0x0400185F RID: 6239
		private byte m_lockStatus;

		// Token: 0x04001860 RID: 6240
		private ulong m_multiInput40;

		// Token: 0x04001861 RID: 6241
		private byte m_pbdsStatus;

		// Token: 0x04001862 RID: 6242
		private byte m_pbdsStatusHigh;

		// Token: 0x04001863 RID: 6243
		private uint m_registerCardNumTotal;

		// Token: 0x04001864 RID: 6244
		private uint m_registerCardStartAddr;

		// Token: 0x04001865 RID: 6245
		private byte m_reserved1;

		// Token: 0x04001866 RID: 6246
		private byte[] m_reservedBytes = new byte[32];

		// Token: 0x04001867 RID: 6247
		private uint m_swipeEndIndex;

		// Token: 0x04001868 RID: 6248
		private uint m_swipeStartAddr;

		// Token: 0x04001869 RID: 6249
		private uint m_swipeStartIndex;

		// Token: 0x0400186A RID: 6250
		private uint m_totalPerson4AntibackShare;

		// Token: 0x0400186B RID: 6251
		private byte[] m_warnInfo = new byte[4];

		// Token: 0x0400186C RID: 6252
		private uint m_wgcticks;

		// Token: 0x0400186D RID: 6253
		protected internal wgMjControllerSwipeRecord[] newSwipes = new wgMjControllerSwipeRecord[10];

		// Token: 0x0400186E RID: 6254
		private byte[] privBytes = new byte[340];
	}
}
