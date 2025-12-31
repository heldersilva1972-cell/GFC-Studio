using System;
using System.Collections.Generic;

namespace WG3000_COMM.Core
{
	// Token: 0x020001D7 RID: 471
	public class wgMjControllerSwipeRecord
	{
		// Token: 0x06000AB5 RID: 2741 RVA: 0x000E60C4 File Offset: 0x000E50C4
		public wgMjControllerSwipeRecord()
		{
			this.m_isWiegand26 = true;
			this.m_isSwipe = true;
			this.m_floorNo = -1;
			this.m_isPassed = true;
			this.m_iReaderNo = 1;
			this.m_isBiDirection = true;
			this.m_isEnterIn = true;
			this.m_isUserCardNO = true;
			this.privBytes = new byte[16];
			this.strRecordAllInput = "";
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000E6128 File Offset: 0x000E5128
		public wgMjControllerSwipeRecord(string strRecordAll)
		{
			this.m_isWiegand26 = true;
			this.m_isSwipe = true;
			this.m_floorNo = -1;
			this.m_isPassed = true;
			this.m_iReaderNo = 1;
			this.m_isBiDirection = true;
			this.m_isEnterIn = true;
			this.m_isUserCardNO = true;
			this.privBytes = new byte[16];
			this.strRecordAllInput = "";
			this.strRecordAllInput = strRecordAll;
			this.Update(strRecordAll);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000E619C File Offset: 0x000E519C
		protected internal wgMjControllerSwipeRecord(byte[] rec, uint startIndex)
		{
			this.m_isWiegand26 = true;
			this.m_isSwipe = true;
			this.m_floorNo = -1;
			this.m_isPassed = true;
			this.m_iReaderNo = 1;
			this.m_isBiDirection = true;
			this.m_isEnterIn = true;
			this.m_isUserCardNO = true;
			this.privBytes = new byte[16];
			this.strRecordAllInput = "";
			this.Update(rec, startIndex, 0U, 0U);
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000E620C File Offset: 0x000E520C
		protected internal wgMjControllerSwipeRecord(byte[] rec, uint startIndex, uint ControllerSN, uint loc)
		{
			this.m_isWiegand26 = true;
			this.m_isSwipe = true;
			this.m_floorNo = -1;
			this.m_isPassed = true;
			this.m_iReaderNo = 1;
			this.m_isBiDirection = true;
			this.m_isEnterIn = true;
			this.m_isUserCardNO = true;
			this.privBytes = new byte[16];
			this.strRecordAllInput = "";
			this.Update(rec, startIndex, ControllerSN, loc);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x000E627C File Offset: 0x000E527C
		private int categoryGet()
		{
			int num;
			if (this.m_isSwipeLimitted)
			{
				if (this.m_isPassed)
				{
					num = 2;
				}
				else
				{
					num = 3;
				}
			}
			else
			{
				num = 0;
				if (!this.m_isSwipe)
				{
					num += 4;
				}
				if (this.m_isRemoteOpen)
				{
					num += 2;
				}
				if (!this.m_isPassed)
				{
					num++;
				}
			}
			this.m_eventCategory = num;
			return num;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x000E62D4 File Offset: 0x000E52D4
		private int reasonGet()
		{
			this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.None;
			if (this.eventCategory == 0)
			{
				if (this.SwipeStatus >= 0 && this.SwipeStatus <= 3)
				{
					this.m_Reason = 0;
				}
				byte swipeStatus = this.SwipeStatus;
				switch (swipeStatus)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipe;
					break;
				default:
					switch (swipeStatus)
					{
					case 16:
					case 17:
					case 18:
					case 19:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipeOpen;
						break;
					default:
						switch (swipeStatus)
						{
						case 32:
						case 33:
						case 34:
						case 35:
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipeClose;
							break;
						}
						break;
					}
					break;
				}
			}
			if (this.eventCategory == 2)
			{
				this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipe;
			}
			if (this.eventCategory == 1)
			{
				byte swipeStatus2 = this.SwipeStatus;
				switch (swipeStatus2)
				{
				case 132:
				case 133:
				case 134:
				case 135:
					this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessPCControl;
					break;
				default:
					switch (swipeStatus2)
					{
					case 144:
					case 145:
					case 146:
					case 147:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE;
						goto IL_02D8;
					case 160:
					case 161:
					case 162:
					case 163:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessERRPASSWORD;
						goto IL_02D8;
					case 196:
					case 197:
					case 198:
					case 199:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK;
						goto IL_02D8;
					case 200:
					case 201:
					case 202:
					case 203:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD;
						goto IL_02D8;
					case 204:
					case 205:
					case 206:
					case 207:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD;
						goto IL_02D8;
					case 208:
					case 209:
					case 210:
					case 211:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessDOORNC;
						goto IL_02D8;
					case 212:
					case 213:
					case 214:
					case 215:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK;
						goto IL_02D8;
					case 216:
					case 217:
					case 218:
					case 219:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES;
						goto IL_02D8;
					case 220:
					case 221:
					case 222:
					case 223:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR;
						goto IL_02D8;
					case 224:
					case 225:
					case 226:
					case 227:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE;
						goto IL_02D8;
					case 228:
					case 229:
					case 230:
					case 231:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER;
						goto IL_02D8;
					case 232:
					case 233:
					case 234:
					case 235:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT;
						goto IL_02D8;
					}
					this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccess;
					break;
				}
			}
			IL_02D8:
			if (this.eventCategory == 3)
			{
				this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT;
			}
			if (this.eventCategory == 4)
			{
				long cardID = this.CardID;
				if (cardID <= 12L && cardID >= 0L)
				{
					switch ((int)cardID)
					{
					case 0:
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordPowerOn;
						break;
					case 1:
						if (this.SwipeStatus >= 0 && this.SwipeStatus <= 3)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButton;
						}
						break;
					case 2:
						if (this.SwipeStatus >= 0 && this.SwipeStatus <= 3)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonOpen;
						}
						break;
					case 3:
						if (this.SwipeStatus >= 0 && this.SwipeStatus <= 3)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonClose;
						}
						break;
					case 8:
						if (this.SwipeStatus >= 0 && this.SwipeStatus <= 3)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDoorOpen;
						}
						break;
					case 9:
						if (this.SwipeStatus >= 0 && this.SwipeStatus <= 3)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordDoorClosed;
						}
						break;
					case 10:
						if (this.SwipeStatus >= 0 && this.SwipeStatus <= 3)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordSuperPasswordDoorOpen;
						}
						break;
					case 11:
						if (this.SwipeStatus >= 0 && this.SwipeStatus <= 3)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordSuperPasswordOpen;
						}
						break;
					case 12:
						if (this.SwipeStatus >= 0 && this.SwipeStatus <= 3)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordSuperPasswordClose;
						}
						break;
					}
				}
			}
			if (this.eventCategory == 5)
			{
				long cardID2 = this.CardID;
				if (cardID2 <= 7L)
				{
					if (cardID2 >= 0L)
					{
						switch ((int)cardID2)
						{
						case 0:
							if (this.SwipeStatus != 0)
							{
								if ((this.SwipeStatus & 130) == 130)
								{
									this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordPowerOn;
								}
								else
								{
									this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordReset;
								}
							}
							else
							{
								this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordPowerOn;
							}
							break;
						case 4:
							if (this.SwipeStatus >= 128 && this.SwipeStatus <= 131)
							{
								this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_Disable;
							}
							break;
						case 5:
							if (this.SwipeStatus >= 128 && this.SwipeStatus <= 131)
							{
								this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_ForcedLock;
							}
							break;
						case 6:
							if (this.SwipeStatus >= 128 && this.SwipeStatus <= 131)
							{
								this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_NotOnLine;
							}
							break;
						case 7:
							if (this.SwipeStatus >= 128 && this.SwipeStatus <= 131)
							{
								this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK;
							}
							break;
						}
					}
				}
				else if (cardID2 <= 90L && cardID2 >= 81L)
				{
					switch ((int)(cardID2 - 81L))
					{
					case 0:
						if (this.SwipeStatus >= 128 && this.SwipeStatus <= 131)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnThreat;
						}
						break;
					case 1:
						if (this.SwipeStatus >= 128 && this.SwipeStatus <= 131)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnThreatOpen;
						}
						break;
					case 2:
						if (this.SwipeStatus >= 128 && this.SwipeStatus <= 131)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnThreatClose;
						}
						break;
					case 3:
						if (this.SwipeStatus >= 128 && this.SwipeStatus <= 131)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnLeftOpen;
						}
						break;
					case 4:
						if (this.SwipeStatus >= 128 && this.SwipeStatus <= 131)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnOpenByForce;
						}
						break;
					case 5:
						if (this.SwipeStatus == 128)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnFire;
						}
						break;
					case 6:
						if (this.SwipeStatus == 128)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnCloseByForce;
						}
						break;
					case 7:
						if (this.SwipeStatus == 128)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnGuardAgainstTheft;
						}
						break;
					case 8:
						if (this.SwipeStatus == 128)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarn24Hour;
						}
						break;
					case 9:
						if (this.SwipeStatus == 128)
						{
							this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnEmergencyCall;
						}
						break;
					}
				}
			}
			if (this.eventCategory == 6)
			{
				byte swipeStatus3 = this.SwipeStatus;
				switch (swipeStatus3)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader;
					break;
				default:
					switch (swipeStatus3)
					{
					case 16:
					case 17:
					case 18:
					case 19:
						string.Format("DoorNo {0},{1}", this.DoorNo.ToString(), "Remote Open Door By Software ");
						this.m_ReasonNo = wgMjControllerSwipeRecord.SwipeStatusNo.RecordRemoteOpenDoor;
						break;
					}
					break;
				}
			}
			return 1;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x000E6A94 File Offset: 0x000E5A94
		public string ToDisplaySimpleInfo(bool bChinese)
		{
			string text = "";
			Dictionary<wgMjControllerSwipeRecord.SwipeStatusNo, string> dictionary = wgMjControllerSwipeRecord.SwipeStatusDesc;
			if (bChinese)
			{
				dictionary = wgMjControllerSwipeRecord.SwipeStatusDesc_Cn;
			}
			if (this.m_eventCategory == 0 || this.m_eventCategory == 2)
			{
				text = string.Format("{0}: \t{1:d}\r\n", "CardID", this.CardID) + string.Format("{0}: \t{1:d}\r\n", "DoorNO", this.m_iDoorNo);
				if (this.m_isEnterIn)
				{
					text += string.Format(" \t[{0}]\r\n", "In");
				}
				else
				{
					text += string.Format(" \t[{0}]\r\n", "Out");
				}
				text = text + string.Format("{0}: \t{1:d}\r\n", "ReaderNO", this.m_iReaderNo) + string.Format("{0}: \t{1}\r\n", "Swipe Status", dictionary[this.m_ReasonNo]) + string.Format("{0}: \t{1}\r\n", "Read Date", this.ReadDate.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (this.m_eventCategory == 1 || this.m_eventCategory == 3)
			{
				text = string.Format("{0}: \t{1:d}\r\n", "CardID", this.CardID) + string.Format("{0}: \t{1:d}\r\n", "DoorNO", this.m_iDoorNo);
				if (this.m_isEnterIn)
				{
					text += string.Format(" \t[{0}]\r\n", "In");
				}
				else
				{
					text += string.Format(" \t[{0}]\r\n", "Out");
				}
				text = text + string.Format("{0}: \t{1:d}\r\n", "ReaderNO", this.m_iReaderNo) + string.Format("{0}: \t{1}\r\n", "Swipe Status", dictionary[this.m_ReasonNo]) + string.Format("{0}: \t{1}\r\n", "Read Date", this.ReadDate.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (this.m_eventCategory == 4)
			{
				text = string.Format("{0}: \t{1:d}\r\n", "CardID", this.CardID) + string.Format("{0}: \t{1:d}\r\n", "DoorNO", this.m_iDoorNo) + string.Format("{0}: \t{1}\r\n", "Swipe Status", dictionary[this.m_ReasonNo]) + string.Format("{0}: \t{1}\r\n", "Read Date", this.ReadDate.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (this.m_eventCategory == 5)
			{
				text = string.Format("{0}: \t{1:d}\r\n", "CardID", this.CardID) + string.Format("{0}: \t{1:d}\r\n", "DoorNO", this.m_iDoorNo) + string.Format("{0}: \t{1}\r\n", "Swipe Status", dictionary[this.m_ReasonNo]) + string.Format("{0}: \t{1}\r\n", "Read Date", this.ReadDate.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (this.m_eventCategory == 6)
			{
				text = string.Format("{0}: \t{1:d}\r\n", "CardID", this.CardID) + string.Format("{0}: \t{1:d}\r\n", "DoorNO", this.m_iDoorNo) + string.Format("{0}: \t{1}\r\n", "Swipe Status", dictionary[this.m_ReasonNo]) + string.Format("{0}: \t{1}\r\n", "Read Date", this.ReadDate.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			return text;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x000E6DFF File Offset: 0x000E5DFF
		public string ToStringRaw()
		{
			return string.Format("{0:x8}{1:x8}{2}", this.m_ulControllerSN, this.m_ulIndexInDataFlash, BitConverter.ToString(this.privBytes).Replace("-", ""));
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x000E6E3C File Offset: 0x000E5E3C
		public void Update(string strRecordAll)
		{
			if (!string.IsNullOrEmpty(strRecordAll) && strRecordAll.Length == 48)
			{
				byte[] array = new byte[16];
				uint num = Convert.ToUInt32(strRecordAll.Substring(0, 8), 16);
				uint num2 = Convert.ToUInt32(strRecordAll.Substring(8, 8), 16);
				for (int i = 0; i < 16; i++)
				{
					array[i] = Convert.ToByte(strRecordAll.Substring(16 + i * 2, 2), 16);
				}
				this.Update(array, 0U, num, num2);
			}
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x000E6EB4 File Offset: 0x000E5EB4
		public void Update(byte[] rec, uint startIndex, uint ControllerSN, uint loc)
		{
			this.m_ulControllerSN = ControllerSN;
			this.m_ulIndexInDataFlash = loc;
			Array.Copy(rec, (long)((ulong)startIndex), this.privBytes, 0L, 16L);
			this.m_ulCardID = BitConverter.ToInt64(this.privBytes, 0);
			this.m_dtRead = wgTools.WgDateToMsDate(this.privBytes, 8);
			wgMjControllerSwipeRecord.SwipeOption swipeOption = (wgMjControllerSwipeRecord.SwipeOption)this.privBytes[12];
			this.m_bytRecOption = this.privBytes[12];
			this.m_bytStatus = this.privBytes[13];
			this.m_swipeOption = swipeOption;
			this.m_isSwipeLimitted = false;
			this.m_currentSwipeTimes = 0;
			this.m_isSwipe = (byte)(swipeOption & wgMjControllerSwipeRecord.SwipeOption.swipe) == 0;
			this.m_isWiegand26 = (byte)(swipeOption & wgMjControllerSwipeRecord.SwipeOption.wg26) == 0;
			this.m_floorNo = -1;
			byte b = this.privBytes[13];
			this.m_swipeStatus = b;
			if (this.m_isSwipe)
			{
				if ((byte)(swipeOption & wgMjControllerSwipeRecord.SwipeOption.LimittedAccess) != 0)
				{
					this.m_isSwipeLimitted = true;
					this.m_isWiegand26 = true;
					this.m_currentSwipeTimes = (int)(this.m_swipeOption & ~(wgMjControllerSwipeRecord.SwipeOption.BiDirection | wgMjControllerSwipeRecord.SwipeOption.LimittedAccess | wgMjControllerSwipeRecord.SwipeOption.OneSecond | wgMjControllerSwipeRecord.SwipeOption.swipe | wgMjControllerSwipeRecord.SwipeOption.wg26));
					this.m_currentSwipeTimes += (this.m_swipeStatus & 124) >> 2;
					if ((byte)(this.m_swipeOption & wgMjControllerSwipeRecord.SwipeOption.wg26) > 0)
					{
						this.m_currentSwipeTimes |= 256;
					}
					if (wgMjController.IsElevator((int)ControllerSN))
					{
						if (this.m_currentSwipeTimes >= 128)
						{
							this.m_floorNo = 0;
						}
						else
						{
							this.m_floorNo = this.m_currentSwipeTimes;
						}
					}
				}
			}
			else if ((byte)(swipeOption & wgMjControllerSwipeRecord.SwipeOption.LimittedAccess) != 0 && wgMjController.IsElevator((int)ControllerSN))
			{
				this.m_currentSwipeTimes = (int)(this.m_swipeOption & ~(wgMjControllerSwipeRecord.SwipeOption.BiDirection | wgMjControllerSwipeRecord.SwipeOption.LimittedAccess | wgMjControllerSwipeRecord.SwipeOption.OneSecond | wgMjControllerSwipeRecord.SwipeOption.swipe | wgMjControllerSwipeRecord.SwipeOption.wg26));
				this.m_currentSwipeTimes += (this.m_swipeStatus & 124) >> 2;
				if ((byte)(this.m_swipeOption & wgMjControllerSwipeRecord.SwipeOption.wg26) > 0)
				{
					this.m_currentSwipeTimes |= 256;
				}
				if (this.m_currentSwipeTimes >= 128)
				{
					this.m_floorNo = 0;
				}
				else
				{
					this.m_floorNo = this.m_currentSwipeTimes;
				}
			}
			if ((byte)(swipeOption & wgMjControllerSwipeRecord.SwipeOption.OneSecond) > 0)
			{
				this.m_dtRead = this.m_dtRead.AddSeconds(1.0);
			}
			this.m_iReaderNo = (b & 3) + 1;
			if (this.m_ulCardID == 0L)
			{
				this.m_iReaderNo = 1;
			}
			if (wgMjController.IsElevator((int)ControllerSN))
			{
				this.m_iReaderNo = 1;
			}
			this.m_isPassed = (b & 128) == 0;
			this.m_Reason = (byte)((b >> 2) & 31);
			this.m_iDoorNo = this.m_iReaderNo;
			this.m_isBiDirection = false;
			if ((byte)(swipeOption & wgMjControllerSwipeRecord.SwipeOption.BiDirection) > 0)
			{
				this.m_isBiDirection = true;
			}
			this.m_isEnterIn = true;
			this.m_AddressIsReader = false;
			this.m_isRemoteOpen = (byte)(swipeOption & (wgMjControllerSwipeRecord.SwipeOption.LimittedAccess | wgMjControllerSwipeRecord.SwipeOption.swipe)) == 6;
			this.m_isUserCardNO = this.m_isSwipe || (this.m_isRemoteOpen && this.m_swipeStatus < 8);
			this.categoryGet();
			if (this.m_isSwipe)
			{
				this.m_AddressIsReader = true;
			}
			else if (this.eventCategory == 4 && (this.m_ulCardID == 10L || this.m_ulCardID == 11L || this.m_ulCardID == 12L))
			{
				this.m_AddressIsReader = true;
			}
			if (this.m_AddressIsReader)
			{
				if (this.m_isBiDirection)
				{
					this.m_iDoorNo = (byte)(this.m_iReaderNo + 1 >> 1);
					if ((this.m_iReaderNo & 1) == 0)
					{
						this.m_isEnterIn = false;
					}
				}
			}
			else if (this.m_isBiDirection)
			{
				if (this.m_iDoorNo <= 2)
				{
					this.m_iReaderNo = (byte)(((int)this.m_iDoorNo << 1) - 1);
					if (wgMjControllerSwipeRecord.bRemoteOpenBiDirection > 0)
					{
						this.m_AddressIsReader = true;
					}
				}
				this.m_isEnterIn = true;
				if ((b & 4) > 0)
				{
					wgMjControllerSwipeRecord.bRemoteOpenBiDirection = 1;
					this.m_AddressIsReader = true;
					if (this.m_iDoorNo <= 2)
					{
						this.m_iReaderNo = (byte)(((int)this.m_iDoorNo << 1) - 1);
						this.m_iReaderNo += 1;
						this.m_isEnterIn = false;
					}
				}
			}
			if (wgMjController.IsElevator((int)ControllerSN))
			{
				this.m_iReaderNo = 1;
			}
			this.reasonGet();
			if (wgTools.gReaderBrokenWarnActive > 0 && this.m_ReasonNo == wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnEmergencyCall)
			{
				this.m_iDoorNo = 2;
				this.m_iReaderNo = 2;
				if (this.m_isBiDirection)
				{
					this.m_iReaderNo = 3;
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x000E7299 File Offset: 0x000E6299
		public bool addressIsReader
		{
			get
			{
				return this.m_AddressIsReader;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x000E72A1 File Offset: 0x000E62A1
		public byte bytRecOption
		{
			get
			{
				return this.privBytes[12];
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x000E72AC File Offset: 0x000E62AC
		internal byte bytRecOption_Internal
		{
			get
			{
				return this.m_bytRecOption;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x000E72B4 File Offset: 0x000E62B4
		public byte bytStatus
		{
			get
			{
				return this.privBytes[13];
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x000E72BF File Offset: 0x000E62BF
		public long CardID
		{
			get
			{
				return this.m_ulCardID;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x000E72C7 File Offset: 0x000E62C7
		public uint ControllerSN
		{
			get
			{
				return this.m_ulControllerSN;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x000E72CF File Offset: 0x000E62CF
		public int currentSwipeTimes
		{
			get
			{
				return this.m_currentSwipeTimes;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x000E72D7 File Offset: 0x000E62D7
		public byte DoorNo
		{
			get
			{
				return this.m_iDoorNo;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x000E72DF File Offset: 0x000E62DF
		public int eventCategory
		{
			get
			{
				return this.m_eventCategory;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x000E72E7 File Offset: 0x000E62E7
		public int floorNo
		{
			get
			{
				return this.m_floorNo;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x000E72EF File Offset: 0x000E62EF
		public uint IndexInDataFlash
		{
			get
			{
				return this.m_ulIndexInDataFlash;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x000E72F7 File Offset: 0x000E62F7
		internal uint indexInDataFlash_Internal
		{
			get
			{
				return this.m_ulIndexInDataFlash;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x000E72FF File Offset: 0x000E62FF
		public bool IsBiDirection
		{
			get
			{
				return this.m_isBiDirection;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x000E7307 File Offset: 0x000E6307
		public bool IsEnterIn
		{
			get
			{
				return this.m_isEnterIn;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x000E730F File Offset: 0x000E630F
		public bool IsHigh8EqualOne
		{
			get
			{
				return (this.privBytes[14] & 2) > 0;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x000E731F File Offset: 0x000E631F
		public bool IsPassed
		{
			get
			{
				return this.m_isPassed;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x000E7327 File Offset: 0x000E6327
		public bool IsQRCardNO
		{
			get
			{
				return (this.privBytes[14] & 8) > 0;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x000E7337 File Offset: 0x000E6337
		public bool IsRemoteOpen
		{
			get
			{
				return this.m_isRemoteOpen;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x000E733F File Offset: 0x000E633F
		public bool IsSFZName
		{
			get
			{
				return (this.privBytes[14] & 4) > 0;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x000E734F File Offset: 0x000E634F
		public bool IsSwipeRecord
		{
			get
			{
				return this.m_isSwipe;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x000E7357 File Offset: 0x000E6357
		public bool IsUserCardNO
		{
			get
			{
				return this.m_isUserCardNO;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x000E735F File Offset: 0x000E635F
		public bool IsWiegand26
		{
			get
			{
				return this.m_isWiegand26;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x000E7367 File Offset: 0x000E6367
		public DateTime ReadDate
		{
			get
			{
				return this.m_dtRead;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x000E736F File Offset: 0x000E636F
		public byte ReaderNo
		{
			get
			{
				return this.m_iReaderNo;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x000E7377 File Offset: 0x000E6377
		public int ReasonNo
		{
			get
			{
				return (int)this.m_ReasonNo;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x000E737F File Offset: 0x000E637F
		public byte Reserved1
		{
			get
			{
				return this.privBytes[14];
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x000E738A File Offset: 0x000E638A
		internal static int SwipeSize
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x000E738E File Offset: 0x000E638E
		public byte SwipeStatus
		{
			get
			{
				return this.m_swipeStatus;
			}
		}

		// Token: 0x0400191A RID: 6426
		private const int RecordSizeInDb = 48;

		// Token: 0x0400191B RID: 6427
		private const int swipeByteSize = 16;

		// Token: 0x0400191C RID: 6428
		internal const int SwipeMaxNum = 204800;

		// Token: 0x0400191D RID: 6429
		public static int bRemoteOpenBiDirection = 0;

		// Token: 0x0400191E RID: 6430
		private bool m_AddressIsReader;

		// Token: 0x0400191F RID: 6431
		private byte m_bytRecOption;

		// Token: 0x04001920 RID: 6432
		private byte m_bytStatus;

		// Token: 0x04001921 RID: 6433
		private int m_currentSwipeTimes;

		// Token: 0x04001922 RID: 6434
		private DateTime m_dtRead;

		// Token: 0x04001923 RID: 6435
		private int m_eventCategory;

		// Token: 0x04001924 RID: 6436
		private int m_floorNo;

		// Token: 0x04001925 RID: 6437
		private byte m_iDoorNo;

		// Token: 0x04001926 RID: 6438
		private byte m_iReaderNo;

		// Token: 0x04001927 RID: 6439
		private bool m_isBiDirection;

		// Token: 0x04001928 RID: 6440
		private bool m_isEnterIn;

		// Token: 0x04001929 RID: 6441
		private bool m_isPassed;

		// Token: 0x0400192A RID: 6442
		private bool m_isRemoteOpen;

		// Token: 0x0400192B RID: 6443
		private bool m_isSwipe;

		// Token: 0x0400192C RID: 6444
		private bool m_isSwipeLimitted;

		// Token: 0x0400192D RID: 6445
		private bool m_isUserCardNO;

		// Token: 0x0400192E RID: 6446
		private bool m_isWiegand26;

		// Token: 0x0400192F RID: 6447
		private byte m_Reason;

		// Token: 0x04001930 RID: 6448
		private wgMjControllerSwipeRecord.SwipeStatusNo m_ReasonNo;

		// Token: 0x04001931 RID: 6449
		private wgMjControllerSwipeRecord.SwipeOption m_swipeOption;

		// Token: 0x04001932 RID: 6450
		private byte m_swipeStatus;

		// Token: 0x04001933 RID: 6451
		private long m_ulCardID;

		// Token: 0x04001934 RID: 6452
		private uint m_ulControllerSN;

		// Token: 0x04001935 RID: 6453
		private uint m_ulIndexInDataFlash;

		// Token: 0x04001936 RID: 6454
		private byte[] privBytes;

		// Token: 0x04001937 RID: 6455
		internal string strRecordAllInput;

		// Token: 0x04001938 RID: 6456
		private static Dictionary<wgMjControllerSwipeRecord.SwipeStatusNo, string> SwipeStatusDesc = new Dictionary<wgMjControllerSwipeRecord.SwipeStatusNo, string>
		{
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipe,
				"Swipe"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipeClose,
				"Swipe Close"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipeOpen,
				"Swipe Open"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipeWithCount,
				"Swipe Times="
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessPCControl,
				"Denied Access:PC Control"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE,
				"Denied Access:No PRIVILEGE"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessERRPASSWORD,
				"Denied Access: Wrong PASSWORD"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK,
				"Denied Access: AntiBack"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD,
				"Denied Access: More Cards"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD,
				"Denied Access: First Card Open"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessDOORNC,
				"Denied Access: Door Set NC"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK,
				"Denied Access: InterLock"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES,
				"Denied Access: Limited Times"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR,
				"Denied Access: Limited Person Indoor"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE,
				"Denied Access: Invalid Timezone"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER,
				"Denied Access: In Order"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT,
				"Denied Access: SWIPE GAP LIMIT"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccess,
				"Denied Access"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT,
				"Denied Access: Limited Times With Count"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButton,
				"Push Button"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonOpen,
				"Push Button Open"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonClose,
				"Push Button Close"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDoorOpen,
				"Door Open"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDoorClosed,
				"Door Closed"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSuperPasswordDoorOpen,
				"Super Password Open Door"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSuperPasswordOpen,
				"Super Password Open"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSuperPasswordClose,
				"Super Password Close"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPowerOn,
				"Controller Power On"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordReset,
				"Controller Reset"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_Disable,
				"Push Button Invalid: Disable"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_ForcedLock,
				"Push Button Invalid: Forced Lock"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_NotOnLine,
				"Push Button Invalid: Not On Line"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK,
				"Push Button Invalid: InterLock"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnThreat,
				"Threat"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnThreatOpen,
				"Threat Open"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnThreatClose,
				"Threat Close"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnLeftOpen,
				"Open too long"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnOpenByForce,
				"Forced Open"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnFire,
				"Fire"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnCloseByForce,
				"Forced Close"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnGuardAgainstTheft,
				"Guard Against Theft"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarn24Hour,
				"7*24Hour Zone"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnEmergencyCall,
				"Emergency Call"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordRemoteOpenDoor,
				"Remote Open Door"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader,
				"Remote Open Door By USB Reader"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.None,
				"Denied Access"
			}
		};

		// Token: 0x04001939 RID: 6457
		private static Dictionary<wgMjControllerSwipeRecord.SwipeStatusNo, string> SwipeStatusDesc_Cn = new Dictionary<wgMjControllerSwipeRecord.SwipeStatusNo, string>
		{
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipe,
				"刷卡开门"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipeClose,
				"刷卡关"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipeOpen,
				"刷卡开"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSwipeWithCount,
				"刷卡开门,已刷次数"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessPCControl,
				"刷卡禁止通过: 电脑控制"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE,
				"刷卡禁止通过: 没有权限"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessERRPASSWORD,
				"刷卡禁止通过: 密码不对"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK,
				"刷卡禁止通过: 反潜回"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD,
				"刷卡禁止通过: 多卡"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD,
				"刷卡禁止通过: 首卡"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessDOORNC,
				"刷卡禁止通过: 门为常闭"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK,
				"刷卡禁止通过: 互锁"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES,
				"刷卡禁止通过: 受刷卡次数限制"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR,
				"刷卡禁止通过: 门内人数限制"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE,
				"刷卡禁止通过: 卡过期,或不在有效时段,或假期约束"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER,
				"刷卡禁止通过: 按顺序进出限制"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT,
				"刷卡禁止通过: 刷卡间隔约束"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccess,
				"刷卡禁止通过: 原因不明"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT,
				"刷卡禁止通过: 刷卡次数限制, 已刷次数"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButton,
				"按钮开门"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonOpen,
				"按钮开"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonClose,
				"按钮关"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDoorOpen,
				"门打开[门磁信号]"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordDoorClosed,
				"门关闭[门磁信号]"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSuperPasswordDoorOpen,
				"超级密码开门"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSuperPasswordOpen,
				"超级密码开"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordSuperPasswordClose,
				"超级密码关"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPowerOn,
				"控制器上电"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordReset,
				"控制器复位"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_Disable,
				"按钮不开门: 按钮禁用"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_ForcedLock,
				"按钮不开门: 强制关门"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_NotOnLine,
				"按钮不开门: 门不在线"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK,
				"按钮不开门: 互锁"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnThreat,
				"胁迫报警"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnThreatOpen,
				"胁迫报警开"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnThreatClose,
				"胁迫报警关"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnLeftOpen,
				"门长时间未关报警"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnOpenByForce,
				"强行闯入报警"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnFire,
				"火警"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnCloseByForce,
				"强制关门"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnGuardAgainstTheft,
				"防盗报警"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarn24Hour,
				"烟雾煤气温度报警"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordWarnEmergencyCall,
				"紧急呼救报警"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordRemoteOpenDoor,
				"操作员远程开门"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader,
				"发卡器确定发出的远程开门"
			},
			{
				wgMjControllerSwipeRecord.SwipeStatusNo.None,
				"禁止通过: 原因不明"
			}
		};

		// Token: 0x020001D8 RID: 472
		public enum EventCategory
		{
			// Token: 0x0400193B RID: 6459
			SwipePass,
			// Token: 0x0400193C RID: 6460
			SwipeNOPass,
			// Token: 0x0400193D RID: 6461
			SwipePassLimitted,
			// Token: 0x0400193E RID: 6462
			SwipeNOPassLimitted,
			// Token: 0x0400193F RID: 6463
			ValidEvent,
			// Token: 0x04001940 RID: 6464
			Warn,
			// Token: 0x04001941 RID: 6465
			RemoteOpen
		}

		// Token: 0x020001D9 RID: 473
		[Flags]
		private enum SwipeOption : byte
		{
			// Token: 0x04001943 RID: 6467
			BiDirection = 16,
			// Token: 0x04001944 RID: 6468
			LimittedAccess = 4,
			// Token: 0x04001945 RID: 6469
			OneSecond = 8,
			// Token: 0x04001946 RID: 6470
			remoteOpen = 4,
			// Token: 0x04001947 RID: 6471
			swipe = 2,
			// Token: 0x04001948 RID: 6472
			wg26 = 1
		}

		// Token: 0x020001DA RID: 474
		private enum SwipeStatusNo
		{
			// Token: 0x0400194A RID: 6474
			None,
			// Token: 0x0400194B RID: 6475
			RecordSwipe,
			// Token: 0x0400194C RID: 6476
			RecordSwipeClose,
			// Token: 0x0400194D RID: 6477
			RecordSwipeOpen,
			// Token: 0x0400194E RID: 6478
			RecordSwipeWithCount,
			// Token: 0x0400194F RID: 6479
			RecordDeniedAccessPCControl,
			// Token: 0x04001950 RID: 6480
			RecordDeniedAccessNOPRIVILEGE,
			// Token: 0x04001951 RID: 6481
			RecordDeniedAccessERRPASSWORD,
			// Token: 0x04001952 RID: 6482
			RecordDeniedAccessSPECIAL_ANTIBACK,
			// Token: 0x04001953 RID: 6483
			RecordDeniedAccessSPECIAL_MORECARD,
			// Token: 0x04001954 RID: 6484
			RecordDeniedAccessSPECIAL_FIRSTCARD,
			// Token: 0x04001955 RID: 6485
			RecordDeniedAccessDOORNC,
			// Token: 0x04001956 RID: 6486
			RecordDeniedAccessSPECIAL_INTERLOCK,
			// Token: 0x04001957 RID: 6487
			RecordDeniedAccessSPECIAL_LIMITEDTIMES,
			// Token: 0x04001958 RID: 6488
			RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR,
			// Token: 0x04001959 RID: 6489
			RecordDeniedAccessINVALIDTIMEZONE,
			// Token: 0x0400195A RID: 6490
			RecordDeniedAccessSPECIAL_INORDER,
			// Token: 0x0400195B RID: 6491
			RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT,
			// Token: 0x0400195C RID: 6492
			RecordDeniedAccess,
			// Token: 0x0400195D RID: 6493
			RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT,
			// Token: 0x0400195E RID: 6494
			RecordPushButton,
			// Token: 0x0400195F RID: 6495
			RecordPushButtonOpen,
			// Token: 0x04001960 RID: 6496
			RecordPushButtonClose,
			// Token: 0x04001961 RID: 6497
			RecordDoorOpen,
			// Token: 0x04001962 RID: 6498
			RecordDoorClosed,
			// Token: 0x04001963 RID: 6499
			RecordSuperPasswordDoorOpen,
			// Token: 0x04001964 RID: 6500
			RecordSuperPasswordOpen,
			// Token: 0x04001965 RID: 6501
			RecordSuperPasswordClose,
			// Token: 0x04001966 RID: 6502
			RecordPowerOn,
			// Token: 0x04001967 RID: 6503
			RecordReset,
			// Token: 0x04001968 RID: 6504
			RecordPushButtonInvalid_Disable,
			// Token: 0x04001969 RID: 6505
			RecordPushButtonInvalid_ForcedLock,
			// Token: 0x0400196A RID: 6506
			RecordPushButtonInvalid_NotOnLine,
			// Token: 0x0400196B RID: 6507
			RecordPushButtonInvalid_INTERLOCK,
			// Token: 0x0400196C RID: 6508
			RecordWarnThreat,
			// Token: 0x0400196D RID: 6509
			RecordWarnThreatOpen,
			// Token: 0x0400196E RID: 6510
			RecordWarnThreatClose,
			// Token: 0x0400196F RID: 6511
			RecordWarnLeftOpen,
			// Token: 0x04001970 RID: 6512
			RecordWarnOpenByForce,
			// Token: 0x04001971 RID: 6513
			RecordWarnFire,
			// Token: 0x04001972 RID: 6514
			RecordWarnCloseByForce,
			// Token: 0x04001973 RID: 6515
			RecordWarnGuardAgainstTheft,
			// Token: 0x04001974 RID: 6516
			RecordWarn24Hour,
			// Token: 0x04001975 RID: 6517
			RecordWarnEmergencyCall,
			// Token: 0x04001976 RID: 6518
			RecordRemoteOpenDoor,
			// Token: 0x04001977 RID: 6519
			RecordRemoteOpenDoor_ByUSBReader
		}
	}
}
