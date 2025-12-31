using System;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using WG3000_COMM.DataOper;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Core
{
	// Token: 0x020001DB RID: 475
	public class MjRec : wgMjControllerSwipeRecord
	{
		// Token: 0x06000ADB RID: 2779 RVA: 0x000E7396 File Offset: 0x000E6396
		public MjRec()
		{
			this.m_beginYMD = DateTime.Parse("2000-1-1");
			this.m_endYMD = DateTime.Parse("2000-1-1");
			this.doorName = "";
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x000E73C9 File Offset: 0x000E63C9
		public MjRec(string strRecordAll)
			: base(strRecordAll)
		{
			this.m_beginYMD = DateTime.Parse("2000-1-1");
			this.m_endYMD = DateTime.Parse("2000-1-1");
			this.doorName = "";
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x000E73FD File Offset: 0x000E63FD
		public MjRec(byte[] rec, uint startIndex)
			: base(rec, startIndex)
		{
			this.m_beginYMD = DateTime.Parse("2000-1-1");
			this.m_endYMD = DateTime.Parse("2000-1-1");
			this.doorName = "";
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x000E7432 File Offset: 0x000E6432
		public MjRec(byte[] rec, uint startIndex, uint ControllerSN, uint loc)
			: base(rec, startIndex, ControllerSN, loc)
		{
			this.m_beginYMD = DateTime.Parse("2000-1-1");
			this.m_endYMD = DateTime.Parse("2000-1-1");
			this.doorName = "";
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x000E746C File Offset: 0x000E646C
		public string GetDetailedRecord(icController current_control, uint RecControllerSN)
		{
			string text = "";
			if (this.strRecordAllInput == "0".PadLeft(48, '0'))
			{
				return "  ";
			}
			if (base.eventCategory == 1 || base.eventCategory == 0)
			{
				byte swipeStatus = base.SwipeStatus;
				if (swipeStatus <= 19)
				{
					switch (swipeStatus)
					{
					case 0:
					case 1:
					case 2:
					case 3:
						text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordSwipe);
						goto IL_049F;
					default:
						switch (swipeStatus)
						{
						case 16:
						case 17:
						case 18:
						case 19:
							text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordSwipeOpen);
							goto IL_049F;
						}
						break;
					}
				}
				else
				{
					switch (swipeStatus)
					{
					case 32:
					case 33:
					case 34:
					case 35:
						text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordSwipeClose);
						goto IL_049F;
					default:
						switch (swipeStatus)
						{
						case 132:
						case 133:
						case 134:
						case 135:
							text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessPCControl);
							goto IL_049F;
						default:
							switch (swipeStatus)
							{
							case 144:
							case 145:
							case 146:
							case 147:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessNOPRIVILEGE);
								goto IL_049F;
							case 160:
							case 161:
							case 162:
							case 163:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessERRPASSWORD);
								goto IL_049F;
							case 196:
							case 197:
							case 198:
							case 199:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_ANTIBACK);
								goto IL_049F;
							case 200:
							case 201:
							case 202:
							case 203:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_MORECARD);
								goto IL_049F;
							case 204:
							case 205:
							case 206:
							case 207:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_FIRSTCARD);
								goto IL_049F;
							case 208:
							case 209:
							case 210:
							case 211:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessDOORNC);
								goto IL_049F;
							case 212:
							case 213:
							case 214:
							case 215:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_INTERLOCK);
								goto IL_049F;
							case 216:
							case 217:
							case 218:
							case 219:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_LIMITEDTIMES);
								goto IL_049F;
							case 220:
							case 221:
							case 222:
							case 223:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR);
								goto IL_049F;
							case 224:
							case 225:
							case 226:
							case 227:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessINVALIDTIMEZONE);
								goto IL_049F;
							case 228:
							case 229:
							case 230:
							case 231:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_INORDER);
								goto IL_049F;
							case 232:
							case 233:
							case 234:
							case 235:
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_SWIPEGAPLIMIT);
								goto IL_049F;
							}
							break;
						}
						break;
					}
				}
				text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccess);
				IL_049F:
				if (base.IsQRCardNO)
				{
					text = text.Replace(CommonStr.strRecordSwipeQR_Swipe, CommonStr.strRecordSwipeQR_QR);
				}
			}
			if (base.eventCategory == 3 || base.eventCategory == 2)
			{
				if (base.IsPassed)
				{
					if (wgMjController.IsElevator((int)base.ControllerSN))
					{
						if (base.currentSwipeTimes >= 128)
						{
							text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordSwipeWithCount4MultiFloor1);
						}
						else
						{
							string text2 = MjRec.control4GetDetailedRecord.GetFloorName(base.floorNo);
							if (!string.IsNullOrEmpty(text2))
							{
								text2 = " [" + text2 + "]";
							}
							text = string.Format("{0}{1}{2}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordSwipeWithCount4Floor, base.currentSwipeTimes) + text2;
						}
					}
					else
					{
						text = string.Format("{0}{1}{2}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordSwipeWithCount, base.currentSwipeTimes);
					}
				}
				else
				{
					text = string.Format("{0}{1}{2}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT, base.currentSwipeTimes);
				}
			}
			if (base.eventCategory == 4 || base.eventCategory == 5)
			{
				long cardID = base.CardID;
				if (cardID <= 12L)
				{
					if (cardID >= 0L)
					{
						switch ((int)cardID)
						{
						case 0:
							if (base.SwipeStatus != 0)
							{
								if ((base.SwipeStatus & 130) == 130)
								{
									text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)(base.SwipeStatus + 1)), CommonStr.strRecordPowerOn);
								}
								else if ((base.SwipeStatus & 160) == 160)
								{
									text = string.Format("{0}{1}-{2}", MjRec.control4GetDetailedRecord.GetDoorName((int)(base.SwipeStatus + 1)), CommonStr.strRecordReset, "LDO");
								}
								else if ((base.SwipeStatus & 144) == 144)
								{
									text = string.Format("{0}{1}-{2}", MjRec.control4GetDetailedRecord.GetDoorName((int)(base.SwipeStatus + 1)), CommonStr.strRecordReset, "SW");
								}
								else if ((base.SwipeStatus & 136) == 136)
								{
									text = string.Format("{0}{1}-{2}", MjRec.control4GetDetailedRecord.GetDoorName((int)(base.SwipeStatus + 1)), CommonStr.strRecordReset, "WDT");
								}
								else if ((base.SwipeStatus & 132) == 132)
								{
									text = string.Format("{0}{1}-{2}", MjRec.control4GetDetailedRecord.GetDoorName((int)(base.SwipeStatus + 1)), CommonStr.strRecordReset, "BOR");
								}
								else if ((base.SwipeStatus & 129) == 129)
								{
									text = string.Format("{0}{1}-{2}", MjRec.control4GetDetailedRecord.GetDoorName((int)(base.SwipeStatus + 1)), CommonStr.strRecordReset, "EXT");
								}
							}
							else
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)(base.SwipeStatus + 1)), CommonStr.strRecordPowerOn);
							}
							if (!string.IsNullOrEmpty(text) && RecControllerSN > 0U)
							{
								text = string.Format("{0}[{1}]", text, RecControllerSN.ToString());
							}
							break;
						case 1:
							if (base.SwipeStatus >= 0 && base.SwipeStatus <= 3)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)(base.SwipeStatus + 1)), CommonStr.strRecordPushButton);
							}
							break;
						case 2:
							if (base.SwipeStatus >= 0 && base.SwipeStatus <= 3)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)(base.SwipeStatus + 1)), CommonStr.strRecordPushButtonOpen);
							}
							break;
						case 3:
							if (base.SwipeStatus >= 0 && base.SwipeStatus <= 3)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)(base.SwipeStatus + 1)), CommonStr.strRecordPushButtonClose);
							}
							break;
						case 4:
							if (base.SwipeStatus >= 128 && base.SwipeStatus <= 131)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)((base.SwipeStatus & 3) + 1)), CommonStr.strRecordPushButtonInvalid_Disable);
							}
							break;
						case 5:
							if (base.SwipeStatus >= 128 && base.SwipeStatus <= 131)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)((base.SwipeStatus & 3) + 1)), CommonStr.strRecordPushButtonInvalid_ForcedLock);
							}
							break;
						case 6:
							if (base.SwipeStatus >= 128 && base.SwipeStatus <= 131)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)((base.SwipeStatus & 3) + 1)), CommonStr.strRecordPushButtonInvalid_NotOnLine);
							}
							break;
						case 7:
							if (base.SwipeStatus >= 128 && base.SwipeStatus <= 131)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)((base.SwipeStatus & 3) + 1)), CommonStr.strRecordPushButtonInvalid_INTERLOCK);
							}
							break;
						case 8:
							if (base.SwipeStatus >= 0 && base.SwipeStatus <= 3)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), CommonStr.strRecordDoorOpen);
							}
							break;
						case 9:
							if (base.SwipeStatus >= 0 && base.SwipeStatus <= 3)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), CommonStr.strRecordDoorClosed);
							}
							break;
						case 10:
							if (base.SwipeStatus >= 0 && base.SwipeStatus <= 3)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordSuperPasswordDoorOpen);
							}
							break;
						case 11:
							if (base.SwipeStatus >= 0 && base.SwipeStatus <= 3)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordSuperPasswordOpen);
							}
							break;
						case 12:
							if (base.SwipeStatus >= 0 && base.SwipeStatus <= 3)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordSuperPasswordClose);
							}
							break;
						}
					}
				}
				else if (cardID <= 90L && cardID >= 81L)
				{
					switch ((int)(cardID - 81L))
					{
					case 0:
						if (base.SwipeStatus >= 128 && base.SwipeStatus <= 131)
						{
							text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), CommonStr.strRecordThreat);
						}
						break;
					case 1:
						if (base.SwipeStatus >= 128 && base.SwipeStatus <= 131)
						{
							text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), CommonStr.strRecordThreatOpen);
						}
						break;
					case 2:
						if (base.SwipeStatus >= 128 && base.SwipeStatus <= 131)
						{
							text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), CommonStr.strRecordInvalidSwipeWarn);
						}
						break;
					case 3:
						if (base.SwipeStatus >= 128 && base.SwipeStatus <= 131)
						{
							if (!wgAppConfig.IsActivateOpenTooLongWarn)
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), CommonStr.strRecordWarnLeftOpen);
							}
							else
							{
								text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), CommonStr.strOpenTooLongWarn2015);
							}
						}
						break;
					case 4:
						if (base.SwipeStatus >= 128 && base.SwipeStatus <= 131)
						{
							text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), CommonStr.strRecordWarnOpenByForce);
						}
						break;
					case 5:
						if (base.SwipeStatus == 128)
						{
							text = CommonStr.strRecordWarnFire;
							if (current_control != null)
							{
								text = MjRec.control4GetDetailedRecord.GetDoorName(1);
								if (wgMjController.GetControllerType((int)base.ControllerSN) == 2)
								{
									text = text + "," + MjRec.control4GetDetailedRecord.GetDoorName(2);
								}
								if (wgMjController.GetControllerType((int)base.ControllerSN) == 4)
								{
									text += string.Format(",{0},{1},{2}", MjRec.control4GetDetailedRecord.GetDoorName(2), MjRec.control4GetDetailedRecord.GetDoorName(3), MjRec.control4GetDetailedRecord.GetDoorName(4));
								}
								text += CommonStr.strRecordWarnFire;
							}
						}
						break;
					case 6:
						if (base.SwipeStatus == 128)
						{
							text = CommonStr.strRecordWarnCloseByForce;
							if (current_control != null)
							{
								text = MjRec.control4GetDetailedRecord.GetDoorName(1);
								if (wgMjController.GetControllerType((int)base.ControllerSN) == 2)
								{
									text = text + "," + MjRec.control4GetDetailedRecord.GetDoorName(2);
								}
								if (wgMjController.GetControllerType((int)base.ControllerSN) == 4)
								{
									text += string.Format(",{0},{1},{2}", MjRec.control4GetDetailedRecord.GetDoorName(2), MjRec.control4GetDetailedRecord.GetDoorName(3), MjRec.control4GetDetailedRecord.GetDoorName(4));
								}
								text += CommonStr.strRecordWarnCloseByForce;
							}
						}
						break;
					case 7:
						if (base.SwipeStatus == 128)
						{
							text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName(1), wgAppConfig.ReplaceSpecialWord(CommonStr.strRecordWarnGuardAgainstTheft, "KEY_strRecordWarnGuardAgainstTheft"));
						}
						break;
					case 8:
						if (base.SwipeStatus == 128)
						{
							text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName(1), wgAppConfig.ReplaceSpecialWord(CommonStr.strRecordWarn24Hour, "KEY_strRecordWarn24Hour"));
						}
						break;
					case 9:
						if (base.SwipeStatus == 128)
						{
							text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName(1), wgAppConfig.ReplaceSpecialWord(CommonStr.strRecordWarnEmergencyCall, "KEY_strRecordWarnEmergencyCall"));
						}
						break;
					}
				}
			}
			if (base.eventCategory != 6)
			{
				return text;
			}
			switch (base.SwipeStatus)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				text = string.Format("{0}{1}{2}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRecordRemoteOpenDoor_ByUSBReader), wgAppConfig.displayPartCardNO(base.CardID.ToString()));
				break;
			case 4:
			case 5:
			case 6:
			case 7:
				text = string.Format("{0}{1}{2}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRecordRemoteOpenDoor_ByUSBReader), wgAppConfig.displayPartCardNO(base.CardID.ToString()));
				break;
			case 16:
			case 17:
			case 18:
			case 19:
				text = string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetDoorName((int)base.DoorNo), wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRecordRemoteOpenDoor));
				break;
			}
			if (!wgMjController.IsElevator((int)RecControllerSN))
			{
				return text;
			}
			if (base.currentSwipeTimes >= 128)
			{
				return string.Format("{0}{1}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordRemoteOpenDoor4MultiFloor1);
			}
			string text3 = MjRec.control4GetDetailedRecord.GetFloorName(base.floorNo);
			if (!string.IsNullOrEmpty(text3))
			{
				text3 = " [" + text3 + "]";
				return string.Format("{0}{1}{2}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordRemoteOpenDoor4Floor, text3);
			}
			return string.Format("{0}{1}{2}", MjRec.control4GetDetailedRecord.GetReaderName((int)base.ReaderNo), CommonStr.strRecordRemoteOpenDoor4Floor, base.currentSwipeTimes);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000E84F4 File Offset: 0x000E74F4
		public string getSFZName()
		{
			string text = "";
			if (base.IsSFZName && string.IsNullOrEmpty(this.m_consumerName))
			{
				byte[] array = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 32, 0 };
				for (int i = 0; i < 8; i++)
				{
					array[i] = (byte)((base.CardID >> i * 8) & 255L);
				}
				if (base.IsHigh8EqualOne)
				{
					array[7] = array[7] | 128;
				}
				string text2 = Encoding.GetEncoding("UTF-16").GetString(array).Replace("\0", "")
					.Trim();
				if (text2.Length > 0)
				{
					text = text2;
				}
			}
			return text;
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000E859C File Offset: 0x000E759C
		public void GetUserInfoFromDB()
		{
			try
			{
				string text = " SELECT  f_ConsumerName,  f_GroupName, f_BeginYMD, f_EndYMD, f_ConsumerID, f_ConsumerNO ";
				text = text + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  WHERE  t_b_Consumer.f_CardNO = " + base.CardID.ToString();
				if (MjRec.bCertIDEnabled)
				{
					text = " SELECT f_ConsumerName, f_GroupName, f_BeginYMD, f_EndYMD, t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_CertificateID ";
					text = text + " FROM (t_b_Consumer LEFT JOIN t_b_Group ON t_b_Consumer.f_GroupID=t_b_Group.f_GroupID) LEFT JOIN t_b_Consumer_Other ON t_b_Consumer.f_ConsumerID=t_b_Consumer_Other.f_ConsumerID  WHERE  t_b_Consumer.f_CardNO = " + base.CardID.ToString();
				}
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				dbCommand.CommandText = text;
				dbConnection.Open();
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.Read())
				{
					this.m_consumerID = (int)dbDataReader["f_ConsumerID"];
					this.m_consumerName = dbDataReader["f_ConsumerName"] as string;
					this.m_consumerNO = (dbDataReader["f_ConsumerNO"] as string).Trim();
					this.m_deptName = dbDataReader["f_GroupName"] as string;
					DateTime.TryParse(dbDataReader["f_BeginYMD"].ToString(), out this.m_beginYMD);
					DateTime.TryParse(dbDataReader["f_EndYMD"].ToString(), out this.m_endYMD);
					if (MjRec.bCertIDEnabled)
					{
						this.m_consumerCertID = dbDataReader["f_CertificateID"] as string;
					}
				}
				else
				{
					this.m_consumerName = "";
					this.m_consumerNO = "";
					this.m_deptName = "";
					if (MjRec.bCertIDEnabled)
					{
						this.m_consumerCertID = "";
					}
				}
				dbDataReader.Close();
				dbConnection.Close();
				dbCommand.Dispose();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000E877C File Offset: 0x000E777C
		public bool GetUserInfoWithLocationFromDB()
		{
			bool flag = false;
			try
			{
				this.m_consumerID = 0;
				string text = " SELECT  f_ConsumerName, f_BeginYMD, f_EndYMD, t_b_Consumer.f_ConsumerID, f_ConsumerNO ";
				text = text + " ,[f_DoorEnabled],[f_PrivilegeTypeID],[f_Last_InOut],[f_lastSwipe_RecID],[f_lastRemoteOpen_ControllerSN]  FROM t_b_Consumer LEFT OUTER JOIN t_b_Consumer_Location ON t_b_Consumer_Location.f_ConsumerID = t_b_Consumer.f_ConsumerID  WHERE  t_b_Consumer.f_CardNO = " + base.CardID.ToString();
				bool flag2 = MjRec.bCertIDEnabled;
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString + ";Connection Timeout=1");
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
					dbCommand.CommandTimeout = 1;
				}
				dbCommand.CommandText = text;
				dbConnection.Open();
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.Read())
				{
					this.m_consumerID = (int)dbDataReader["f_ConsumerID"];
					this.m_consumerName = dbDataReader["f_ConsumerName"] as string;
					this.m_consumerNO = (dbDataReader["f_ConsumerNO"] as string).Trim();
					DateTime.TryParse(dbDataReader["f_BeginYMD"].ToString(), out this.m_beginYMD);
					DateTime.TryParse(dbDataReader["f_EndYMD"].ToString(), out this.m_endYMD);
					if (MjRec.bCertIDEnabled)
					{
						this.m_consumerCertID = dbDataReader["f_CertificateID"] as string;
					}
					this.m_DoorEnabled = (int)((byte)dbDataReader["f_DoorEnabled"]);
					this.m_PrivilegeTypeID = (int)dbDataReader["f_PrivilegeTypeID"];
					if (wgTools.SetObjToStr(dbDataReader["f_Last_InOut"]) == "" || wgTools.SetObjToStr(dbDataReader["f_lastSwipe_RecID"]) == "" || wgTools.SetObjToStr(dbDataReader["f_lastRemoteOpen_ControllerSN"]) == "")
					{
						wgTools.WgDebugWrite("有问题的: " + this.m_consumerID.ToString() + "," + this.m_consumerName, new object[0]);
						this.m_Last_InOut = 0;
						this.m_lastSwipe_RecID = 0;
						this.m_lastRemoteOpen_ControllerSN = 0;
					}
					else
					{
						this.m_Last_InOut = (int)((byte)dbDataReader["f_Last_InOut"]);
						this.m_lastSwipe_RecID = (int)dbDataReader["f_lastSwipe_RecID"];
						this.m_lastRemoteOpen_ControllerSN = (int)dbDataReader["f_lastRemoteOpen_ControllerSN"];
					}
				}
				else
				{
					this.m_consumerName = "";
					this.m_consumerNO = "";
					this.m_deptName = "";
					if (MjRec.bCertIDEnabled)
					{
						this.m_consumerCertID = "";
					}
				}
				dbDataReader.Close();
				dbConnection.Close();
				dbCommand.Dispose();
				flag = true;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return flag;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x000E8A54 File Offset: 0x000E7A54
		public void GetUserInfoWithLocationFromDBTest()
		{
			try
			{
				this.m_consumerID = 0;
				int num = 0;
				string text = " SELECT  f_ConsumerName, f_BeginYMD, f_EndYMD, t_b_Consumer.f_ConsumerID, f_ConsumerNO ";
				text += " ,[f_DoorEnabled],[f_PrivilegeTypeID],[f_Last_InOut],[f_lastSwipe_RecID],[f_lastRemoteOpen_ControllerSN]  FROM t_b_Consumer LEFT OUTER JOIN t_b_Consumer_Location ON t_b_Consumer_Location.f_ConsumerID = t_b_Consumer.f_ConsumerID ";
				bool flag = MjRec.bCertIDEnabled;
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString + ";Connection Timeout=1");
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
					dbCommand.CommandTimeout = 1;
				}
				dbCommand.CommandText = text;
				dbConnection.Open();
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				this.m_consumerName = "";
				this.m_consumerNO = "";
				this.m_deptName = "";
				if (MjRec.bCertIDEnabled)
				{
					this.m_consumerCertID = "";
				}
				while (dbDataReader.Read())
				{
					num++;
					this.m_consumerID = (int)dbDataReader["f_ConsumerID"];
					this.m_consumerName = dbDataReader["f_ConsumerName"] as string;
					this.m_consumerNO = (dbDataReader["f_ConsumerNO"] as string).Trim();
					DateTime.TryParse(dbDataReader["f_BeginYMD"].ToString(), out this.m_beginYMD);
					DateTime.TryParse(dbDataReader["f_EndYMD"].ToString(), out this.m_endYMD);
					if (MjRec.bCertIDEnabled)
					{
						this.m_consumerCertID = dbDataReader["f_CertificateID"] as string;
					}
					this.m_DoorEnabled = (int)((byte)dbDataReader["f_DoorEnabled"]);
					this.m_PrivilegeTypeID = (int)dbDataReader["f_PrivilegeTypeID"];
					if (wgTools.SetObjToStr(dbDataReader["f_Last_InOut"]) == "" || wgTools.SetObjToStr(dbDataReader["f_lastSwipe_RecID"]) == "" || wgTools.SetObjToStr(dbDataReader["f_lastRemoteOpen_ControllerSN"]) == "")
					{
						wgTools.WgDebugWrite("有问题的: " + this.m_consumerID.ToString() + "," + this.m_consumerName, new object[0]);
						this.m_Last_InOut = 0;
						this.m_lastSwipe_RecID = 0;
						this.m_lastRemoteOpen_ControllerSN = 0;
					}
					else
					{
						this.m_Last_InOut = (int)((byte)dbDataReader["f_Last_InOut"]);
						this.m_lastSwipe_RecID = (int)dbDataReader["f_lastSwipe_RecID"];
						this.m_lastRemoteOpen_ControllerSN = (int)dbDataReader["f_lastRemoteOpen_ControllerSN"];
					}
				}
				wgTools.WgDebugWrite("count=" + num, new object[0]);
				dbDataReader.Close();
				dbConnection.Close();
				dbCommand.Dispose();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x000E8D40 File Offset: 0x000E7D40
		public bool isValidPassedSwipe()
		{
			if (base.IsSwipeRecord && (base.eventCategory == 1 || base.eventCategory == 0))
			{
				byte swipeStatus = base.SwipeStatus;
				switch (swipeStatus)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					return true;
				default:
					switch (swipeStatus)
					{
					case 16:
					case 17:
					case 18:
					case 19:
						return true;
					default:
						switch (swipeStatus)
						{
						case 32:
						case 33:
						case 34:
						case 35:
							return true;
						}
						break;
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x000E8DC0 File Offset: 0x000E7DC0
		public string ToDisplayDetail()
		{
			string text = "";
			if (base.IsSwipeRecord)
			{
				text += string.Format("{0}: \t{1:d}\r\n", CommonStr.strCardID, wgAppConfig.displayPartCardNO(base.CardID));
				if (this.m_consumerName == null)
				{
					this.GetUserInfoFromDB();
				}
				text = text + string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceWorkNO(CommonStr.strReplaceWorkNO), this.m_consumerNO) + string.Format("{0}: \t{1}\r\n", CommonStr.strName, this.m_consumerName);
				if (MjRec.bCertIDEnabled)
				{
					text += string.Format("{0}: \t{1}\r\n", CommonStr.strSFZCertificateIDCode, this.m_consumerCertID);
				}
				text += string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment), this.m_deptName);
			}
			else if (base.IsRemoteOpen)
			{
				if (wgMjController.IsElevator((int)base.ControllerSN))
				{
					text += string.Format("{0}: \t{1:d}\r\n", CommonStr.strCardID, wgAppConfig.displayPartCardNO(base.CardID));
					if (this.m_consumerName == null)
					{
						this.GetUserInfoFromDB();
					}
					text = text + string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceWorkNO(CommonStr.strReplaceWorkNO), this.m_consumerNO) + string.Format("{0}: \t{1}\r\n", CommonStr.strName, this.m_consumerName);
					if (MjRec.bCertIDEnabled)
					{
						text += string.Format("{0}: \t{1}\r\n", CommonStr.strSFZCertificateIDCode, this.m_consumerCertID);
					}
					text += string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment), this.m_deptName);
				}
				else if (base.SwipeStatus < 4)
				{
					text += string.Format("{0}: \t{1:d}\r\n", CommonStr.strCardID, wgAppConfig.displayPartCardNO(base.CardID));
					if (this.m_consumerName == null)
					{
						this.GetUserInfoFromDB();
					}
					text = text + string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceWorkNO(CommonStr.strReplaceWorkNO), this.m_consumerNO) + string.Format("{0}: \t{1}\r\n", CommonStr.strName, this.m_consumerName);
					if (MjRec.bCertIDEnabled)
					{
						text += string.Format("{0}: \t{1}\r\n", CommonStr.strSFZCertificateIDCode, this.m_consumerCertID);
					}
					text += string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment), this.m_deptName);
				}
				else if (base.SwipeStatus < 8)
				{
					text += string.Format("{0}: \t{1:d}\r\n", CommonStr.strCardID, wgAppConfig.displayPartCardNO(base.CardID));
					if (this.m_consumerName == null)
					{
						this.GetUserInfoFromDB();
					}
					text = text + string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceWorkNO(CommonStr.strReplaceWorkNO), this.m_consumerNO) + string.Format("{0}: \t{1}\r\n", CommonStr.strName, this.m_consumerName);
					if (MjRec.bCertIDEnabled)
					{
						text += string.Format("{0}: \t{1}\r\n", CommonStr.strSFZCertificateIDCode, this.m_consumerCertID);
					}
					text += string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment), this.m_deptName);
				}
				else if (base.SwipeStatus < 20 && base.SwipeStatus >= 16)
				{
					text += string.Format("{0}: \t{1:d}\r\n", CommonStr.strCardID, wgAppConfig.displayPartCardNO(base.CardID));
				}
			}
			return text + string.Format("{0}: \t{1}\r\n", CommonStr.strReadDate, base.ReadDate.ToString(wgTools.DisplayFormat_DateYMDHMSWeek)) + string.Format("{0}: \t{1}\r\n", CommonStr.strAddr, string.IsNullOrEmpty(this.m_Address) ? base.ControllerSN.ToString() : this.m_Address) + string.Format("{0}: \t{1}\r\n", CommonStr.strSwipeStatus, this.GetDetailedRecord(null, base.ControllerSN));
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x000E9174 File Offset: 0x000E8174
		public string ToDisplayDetail4TCPServer()
		{
			string text = "";
			text += string.Format("ReasonNo: \t{0:d}\r\n", base.ReasonNo);
			if (base.IsSwipeRecord)
			{
				text += string.Format("{0}: \t{1:d}\r\n", CommonStr.strCardID, wgAppConfig.displayPartCardNO(base.CardID));
				if (this.m_consumerName == null)
				{
					this.GetUserInfoFromDB();
				}
				text = text + string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceWorkNO(CommonStr.strReplaceWorkNO), this.m_consumerNO) + string.Format("{0}: \t{1}\r\n", CommonStr.strName, this.m_consumerName);
				if (MjRec.bCertIDEnabled)
				{
					text += string.Format("{0}: \t{1}\r\n", CommonStr.strSFZCertificateIDCode, this.m_consumerCertID);
				}
				text += string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment), this.m_deptName);
			}
			else if (base.IsRemoteOpen)
			{
				if (base.SwipeStatus < 4)
				{
					text += string.Format("{0}: \t{1:d}\r\n", CommonStr.strCardID, wgAppConfig.displayPartCardNO(base.CardID));
					if (this.m_consumerName == null)
					{
						this.GetUserInfoFromDB();
					}
					text = text + string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceWorkNO(CommonStr.strReplaceWorkNO), this.m_consumerNO) + string.Format("{0}: \t{1}\r\n", CommonStr.strName, this.m_consumerName);
					if (MjRec.bCertIDEnabled)
					{
						text += string.Format("{0}: \t{1}\r\n", CommonStr.strSFZCertificateIDCode, this.m_consumerCertID);
					}
					text += string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment), this.m_deptName);
				}
				else if (base.SwipeStatus < 8)
				{
					text += string.Format("{0}: \t{1:d}\r\n", CommonStr.strCardID, wgAppConfig.displayPartCardNO(base.CardID));
					if (this.m_consumerName == null)
					{
						this.GetUserInfoFromDB();
					}
					text = text + string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceWorkNO(CommonStr.strReplaceWorkNO), this.m_consumerNO) + string.Format("{0}: \t{1}\r\n", CommonStr.strName, this.m_consumerName);
					if (MjRec.bCertIDEnabled)
					{
						text += string.Format("{0}: \t{1}\r\n", CommonStr.strSFZCertificateIDCode, this.m_consumerCertID);
					}
					text += string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment), this.m_deptName);
				}
				else if (base.SwipeStatus < 20 && base.SwipeStatus >= 16)
				{
					text += string.Format("{0}: \t{1:d}\r\n", CommonStr.strCardID, wgAppConfig.displayPartCardNO(base.CardID));
				}
			}
			text = text + string.Format("{0}: \t{1}\r\n", CommonStr.strReadDate, base.ReadDate.ToString(wgTools.DisplayFormat_DateYMDHMSWeek)) + string.Format("{0}: \t{1}\r\n", CommonStr.strAddr, string.IsNullOrEmpty(this.doorName) ? base.ControllerSN.ToString() : this.doorName);
			if (base.IsSwipeRecord)
			{
				text += string.Format("{0}: \t{1}\r\n", CommonStr.strInOut, base.IsEnterIn ? CommonStr.strInDoor : CommonStr.strOutDoor);
			}
			return text + string.Format("{0}: \t{1}\r\n", CommonStr.strSwipeStatus, this.GetDetailedRecord(null, base.ControllerSN));
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x000E94BC File Offset: 0x000E84BC
		public string ToDisplayInfo()
		{
			string text = "";
			if (base.IsSwipeRecord)
			{
				text += string.Format("{0:d}-", wgAppConfig.displayPartCardNO(base.CardID));
				if (this.m_consumerName == null)
				{
					this.GetUserInfoFromDB();
				}
				if (base.IsSFZName && string.IsNullOrEmpty(this.m_consumerName))
				{
					this.m_consumerName = this.getSFZName();
				}
				text = text + string.Format("{0}-", this.m_consumerNO.Trim()) + string.Format("{0}-", this.m_consumerName);
				if (MjRec.bCertIDEnabled)
				{
					text += string.Format("{0}-", this.m_consumerCertID);
				}
				text += string.Format("{0}-", this.m_deptName);
			}
			else if (base.IsRemoteOpen)
			{
				if (wgMjController.IsElevator((int)base.ControllerSN))
				{
					text += string.Format("{0:d}-", wgAppConfig.displayPartCardNO(base.CardID));
					if (this.m_consumerName == null)
					{
						this.GetUserInfoFromDB();
					}
					text = text + string.Format("{0}-", this.m_consumerNO.Trim()) + string.Format("{0}-", this.m_consumerName);
					if (MjRec.bCertIDEnabled)
					{
						text += string.Format("{0}-", this.m_consumerCertID);
					}
					text += string.Format("{0}-", this.m_deptName);
				}
				else if (base.SwipeStatus < 4)
				{
					text += string.Format("{0:d}-", wgAppConfig.displayPartCardNO(base.CardID));
					if (this.m_consumerName == null)
					{
						this.GetUserInfoFromDB();
					}
					text = text + string.Format("{0}-", this.m_consumerNO.Trim()) + string.Format("{0}-", this.m_consumerName);
					if (MjRec.bCertIDEnabled)
					{
						text += string.Format("{0}-", this.m_consumerCertID);
					}
					text += string.Format("{0}-", this.m_deptName);
				}
				else if (base.SwipeStatus < 8)
				{
					text += string.Format("{0:d}-", wgAppConfig.displayPartCardNO(base.CardID));
					if (this.m_consumerName == null)
					{
						this.GetUserInfoFromDB();
					}
					text = text + string.Format("{0}-", this.m_consumerNO.Trim()) + string.Format("{0}-", this.m_consumerName);
					if (MjRec.bCertIDEnabled)
					{
						text += string.Format("{0}-", this.m_consumerCertID);
					}
					text += string.Format("{0}-", this.m_deptName);
				}
				else if (base.SwipeStatus < 20 && base.SwipeStatus >= 16)
				{
					text += string.Format("{0:d}-", wgAppConfig.displayPartCardNO(base.CardID));
				}
			}
			return text + string.Format("{0}-", base.ReadDate.ToString(wgTools.DisplayFormat_DateYMDHMSWeek)) + string.Format("{0}-", string.IsNullOrEmpty(this.m_Address) ? base.ControllerSN.ToString() : this.m_Address) + string.Format("{0}", this.GetDetailedRecord(null, base.ControllerSN));
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x000E9804 File Offset: 0x000E8804
		public string ToDisplayInfo4Led(string dispFormat)
		{
			string text = "";
			if (!base.IsSwipeRecord)
			{
				if (!base.IsRemoteOpen)
				{
					return text;
				}
				if (this.m_consumerName == null)
				{
					this.GetUserInfoFromDB();
				}
				if (string.IsNullOrEmpty(this.m_consumerName))
				{
					return text;
				}
				string text2 = "通过";
				return this.m_consumerName + " " + text2;
			}
			else
			{
				if (this.m_consumerName == null)
				{
					this.GetUserInfoFromDB();
				}
				if (string.IsNullOrEmpty(this.m_consumerName))
				{
					return text;
				}
				string text3 = "禁止通过";
				if (base.eventCategory == 1 || base.eventCategory == 0)
				{
					byte swipeStatus = base.SwipeStatus;
					switch (swipeStatus)
					{
					case 0:
					case 1:
					case 2:
					case 3:
						text3 = "通过";
						break;
					default:
						switch (swipeStatus)
						{
						case 16:
						case 17:
						case 18:
						case 19:
							text3 = "通过";
							break;
						default:
							switch (swipeStatus)
							{
							case 32:
							case 33:
							case 34:
							case 35:
								text3 = "通过";
								break;
							}
							break;
						}
						break;
					}
				}
				return this.m_consumerName + " " + text3;
			}
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x000E990C File Offset: 0x000E890C
		public string ToDisplayInfo4LedMode1(string dispFormat)
		{
			string text = "";
			if (!base.IsSwipeRecord)
			{
				return text;
			}
			if (this.m_consumerName == null)
			{
				this.GetUserInfoFromDB();
			}
			if (string.IsNullOrEmpty(this.m_consumerName))
			{
				return text;
			}
			string text2 = "禁止通过";
			if (base.eventCategory == 1 || base.eventCategory == 0)
			{
				byte swipeStatus = base.SwipeStatus;
				switch (swipeStatus)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					text2 = "通过";
					break;
				default:
					switch (swipeStatus)
					{
					case 16:
					case 17:
					case 18:
					case 19:
						text2 = "通过";
						break;
					default:
						switch (swipeStatus)
						{
						case 32:
						case 33:
						case 34:
						case 35:
							text2 = "通过";
							break;
						}
						break;
					}
					break;
				}
			}
			if (string.IsNullOrEmpty(text2) || !base.IsEnterIn)
			{
				return text;
			}
			if (string.IsNullOrEmpty(this.m_deptName))
			{
				return this.m_consumerName + "/ /" + base.ReadDate.ToString("HH:mm:ss");
			}
			if (this.m_deptName.LastIndexOf("\\") > 0)
			{
				return string.Concat(new string[]
				{
					this.m_consumerName,
					"/",
					this.m_deptName.Substring(this.m_deptName.LastIndexOf("\\") + 1),
					"/",
					base.ReadDate.ToString("HH:mm:ss")
				});
			}
			return string.Concat(new string[]
			{
				this.m_consumerName,
				"/",
				this.m_deptName,
				"/",
				base.ReadDate.ToString("HH:mm:ss")
			});
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000E9ACC File Offset: 0x000E8ACC
		public int updateOnlyFloorNames(string[] floorName40)
		{
			return MjRec.control4GetDetailedRecord.updateFloorNames(floorName40);
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x000E9ADC File Offset: 0x000E8ADC
		public void UpdateOnlySimple(int consumerID, string consumerName, string dtpName, string consumerNO)
		{
			if (!string.IsNullOrEmpty(consumerName))
			{
				this.m_consumerID = consumerID;
				this.m_consumerName = consumerName;
				this.m_consumerNO = consumerNO.Trim();
				this.m_deptName = dtpName;
				return;
			}
			this.m_consumerName = "";
			this.m_consumerNO = "";
			this.m_deptName = "";
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x000E9B35 File Offset: 0x000E8B35
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x000E9B3D File Offset: 0x000E8B3D
		public string address
		{
			get
			{
				return this.m_Address;
			}
			set
			{
				this.m_Address = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x000E9B46 File Offset: 0x000E8B46
		public DateTime beginYMD
		{
			get
			{
				return this.m_beginYMD;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x000E9B4E File Offset: 0x000E8B4E
		public string consumerCertID
		{
			get
			{
				return this.m_consumerCertID;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x000E9B56 File Offset: 0x000E8B56
		public int consumerID
		{
			get
			{
				return this.m_consumerID;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x000E9B5E File Offset: 0x000E8B5E
		public string consumerName
		{
			get
			{
				return this.m_consumerName;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x000E9B66 File Offset: 0x000E8B66
		public string consumerNO
		{
			get
			{
				return this.m_consumerNO;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x000E9B6E File Offset: 0x000E8B6E
		public int doorEnabled
		{
			get
			{
				return this.m_DoorEnabled;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x000E9B76 File Offset: 0x000E8B76
		public DateTime endYMD
		{
			get
			{
				return this.m_endYMD;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x000E9B7E File Offset: 0x000E8B7E
		public string groupname
		{
			get
			{
				return this.m_deptName;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x000E9B86 File Offset: 0x000E8B86
		public int lastInOut
		{
			get
			{
				return this.m_Last_InOut;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x000E9B8E File Offset: 0x000E8B8E
		public int lastRemoteOpen_ControllerSN
		{
			get
			{
				return this.m_lastRemoteOpen_ControllerSN;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x000E9B96 File Offset: 0x000E8B96
		public int lastSwipeRecID
		{
			get
			{
				return this.m_lastSwipe_RecID;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x000E9B9E File Offset: 0x000E8B9E
		public int privilegeTypeID
		{
			get
			{
				return this.m_PrivilegeTypeID;
			}
		}

		// Token: 0x04001978 RID: 6520
		public const int RecordSizeInDb = 48;

		// Token: 0x04001979 RID: 6521
		public static bool bCertIDEnabled = false;

		// Token: 0x0400197A RID: 6522
		public string doorName;

		// Token: 0x0400197B RID: 6523
		private static icController control4GetDetailedRecord = new icController();

		// Token: 0x0400197C RID: 6524
		private DateTime m_beginYMD;

		// Token: 0x0400197D RID: 6525
		private DateTime m_endYMD;

		// Token: 0x0400197E RID: 6526
		private string m_Address;

		// Token: 0x0400197F RID: 6527
		private string m_consumerCertID;

		// Token: 0x04001980 RID: 6528
		private int m_consumerID;

		// Token: 0x04001981 RID: 6529
		private string m_consumerName;

		// Token: 0x04001982 RID: 6530
		private string m_consumerNO;

		// Token: 0x04001983 RID: 6531
		private string m_deptName;

		// Token: 0x04001984 RID: 6532
		private int m_DoorEnabled;

		// Token: 0x04001985 RID: 6533
		private int m_Last_InOut;

		// Token: 0x04001986 RID: 6534
		private int m_lastRemoteOpen_ControllerSN;

		// Token: 0x04001987 RID: 6535
		private int m_lastSwipe_RecID;

		// Token: 0x04001988 RID: 6536
		private int m_PrivilegeTypeID;
	}
}
