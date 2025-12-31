using System;
using System.Collections;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using WG3000_COMM.Core;

namespace WG3000_COMM.DataOper
{
	// Token: 0x02000220 RID: 544
	internal class icControllerConfigureFromDB
	{
		// Token: 0x06000FBF RID: 4031 RVA: 0x001186A8 File Offset: 0x001176A8
		public static int getControllerConfigureFromDBByControllerID(int ControllerID, ref wgMjControllerConfigure controlConfigure, ref wgMjControllerTaskList controlTaskList, ref wgMjControllerHolidaysList controlHolidayList)
		{
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
			int num = ControllerID;
			if (num <= 0)
			{
				return -1;
			}
			int num2 = 0;
			int num3 = 0;
			dbConnection.Open();
			string text = " SELECT *, f_ZoneNO  FROM t_b_Controller LEFT JOIN t_b_Controller_Zone ON t_b_Controller_Zone.f_ZoneID = t_b_Controller.f_ZoneID WHERE f_ControllerID =  " + num.ToString();
			dbCommand.CommandText = text;
			dbCommand.CommandText = text;
			DbDataReader dbDataReader = dbCommand.ExecuteReader();
			if (dbDataReader.Read())
			{
				num2 = (int)dbDataReader["f_ControllerSN"];
				if (!string.IsNullOrEmpty(wgTools.SetObjToStr(dbDataReader["f_ZoneNO"])))
				{
					num3 = int.Parse(wgTools.SetObjToStr(dbDataReader["f_ZoneNO"]));
				}
			}
			dbDataReader.Close();
			text = " SELECT * from t_b_door where [f_controllerID]= " + ControllerID.ToString() + " order by [f_DoorNO] ASC";
			dbCommand.CommandText = text;
			dbDataReader = dbCommand.ExecuteReader();
			int num4 = 0;
			while (dbDataReader.Read())
			{
				num4++;
				controlConfigure.DoorControlSet(num4, (int)dbDataReader["f_DoorControl"]);
				controlConfigure.DoorDelaySet(num4, (int)dbDataReader["f_DoorDelay"]);
				controlConfigure.doorNameSet(num4, wgTools.SetObjToStr(dbDataReader["f_DoorName"]));
				if (wgAppConfig.getParamValBoolByNO(134))
				{
					controlConfigure.MorecardNeedCardsSet(num4, (int)dbDataReader["f_MoreCards_Total"]);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 1, (int)dbDataReader["f_MoreCards_Grp1"]);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 2, (int)dbDataReader["f_MoreCards_Grp2"]);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 3, (int)dbDataReader["f_MoreCards_Grp3"]);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 4, (int)dbDataReader["f_MoreCards_Grp4"]);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 5, (int)dbDataReader["f_MoreCards_Grp5"]);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 6, (int)dbDataReader["f_MoreCards_Grp6"]);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 7, (int)dbDataReader["f_MoreCards_Grp7"]);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 8, (int)dbDataReader["f_MoreCards_Grp8"]);
				}
				else
				{
					controlConfigure.MorecardNeedCardsSet(num4, 0);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 1, 0);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 2, 0);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 3, 0);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 4, 0);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 5, 0);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 6, 0);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 7, 0);
					controlConfigure.MorecardGroupNeedCardsSet(num4, 8, 0);
				}
				if ((int)dbDataReader["f_MoreCards_Grp1"] > 0 || (int)dbDataReader["f_MoreCards_Grp2"] > 0 || (int)dbDataReader["f_MoreCards_Grp3"] > 0 || (int)dbDataReader["f_MoreCards_Grp4"] > 0 || (int)dbDataReader["f_MoreCards_Grp5"] > 0 || (int)dbDataReader["f_MoreCards_Grp6"] > 0 || (int)dbDataReader["f_MoreCards_Grp7"] > 0 || (int)dbDataReader["f_MoreCards_Grp8"] > 0)
				{
					controlConfigure.MorecardSequenceInputSet(num4, ((int)dbDataReader["f_MoreCards_Option"] & 16) > 0);
				}
				else
				{
					controlConfigure.MorecardSequenceInputSet(num4, false);
				}
				controlConfigure.MorecardSingleGroupEnableSet(num4, ((int)dbDataReader["f_MoreCards_Option"] & 8) > 0);
				controlConfigure.MorecardSingleGroupStartNOSet(num4, ((int)dbDataReader["f_MoreCards_Option"] & 7) + 1);
				controlConfigure.DoorDisableTimesegMinSet(num4, 0);
			}
			dbDataReader.Close();
			text = " SELECT * from t_b_reader where [f_controllerID]= " + ControllerID.ToString() + " order by [f_ReaderNO] ASC";
			dbCommand.CommandText = text;
			dbDataReader = dbCommand.ExecuteReader();
			int num5 = 0;
			bool paramValBoolByNO = wgAppConfig.getParamValBoolByNO(123);
			while (dbDataReader.Read())
			{
				num5++;
				if (paramValBoolByNO)
				{
					controlConfigure.ReaderPasswordSet(num5, (int)dbDataReader["f_PasswordEnabled"]);
					controlConfigure.InputCardNOOpenSet(num5, (int)dbDataReader["f_InputCardno_Enabled"]);
				}
				else
				{
					controlConfigure.ReaderPasswordSet(num5, 0);
					controlConfigure.InputCardNOOpenSet(num5, 0);
				}
			}
			dbDataReader.Close();
			controlConfigure.fourtimes_set_no = 0;
			if (wgAppConfig.getParamValBoolByNO(170))
			{
				controlConfigure.fourtimes_set_no = 15;
				string text2 = "";
				string text3 = "";
				string text4 = "";
				wgAppConfig.getSystemParamValue(170, out text2, out text3, out text4);
				if (!string.IsNullOrEmpty(text4))
				{
					string[] array = text4.Split(new char[] { ';' });
					if (array.Length >= 2 && !string.IsNullOrEmpty(array[1]))
					{
						text = string.Format(" SELECT * from t_b_reader where [f_controllerID]= {0} and f_ReaderID IN ({1}) order by [f_ReaderNO] ASC", ControllerID.ToString(), array[1]);
						dbCommand.CommandText = text;
						dbDataReader = dbCommand.ExecuteReader();
						int num6 = 0;
						while (dbDataReader.Read())
						{
							num6 += 1 << (int)dbDataReader["f_ReaderNO"] - 1;
						}
						int num7 = 0;
						int.TryParse(array[0], out num7);
						if (num7 < 16)
						{
							num6 += num7 << 4;
						}
						controlConfigure.fourtimes_set_no = num6;
						dbDataReader.Close();
					}
				}
			}
			controlConfigure.swipe_auto_addcard = 0;
			int num8 = 0;
			if (wgAppConfig.getParamValBoolByNO(146))
			{
				string text5 = "";
				string text6 = "";
				string text7 = "";
				wgAppConfig.getSystemParamValue(146, out text5, out text6, out text7);
				if (!string.IsNullOrEmpty(wgTools.SetObjToStr(text7)))
				{
					text = string.Format(" SELECT * from t_b_door where [f_controllerID]= {0} AND [f_DoorID] IN ({1}) order by [f_DoorNO] ASC", ControllerID.ToString(), text7);
					dbCommand.CommandText = text;
					dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						num8 |= 1 << int.Parse(wgTools.SetObjToStr(dbDataReader["f_DoorNO"])) - 1;
					}
					dbDataReader.Close();
				}
				if (wgAppConfig.getParamValBoolByNO(214) && num8 > 0)
				{
					num8 |= 64;
				}
			}
			controlConfigure.lockSwitchOption = num8;
			controlConfigure.swipeGap = int.Parse("0" + wgAppConfig.getSystemParamByNO(147));
			text = " SELECT * from t_b_Controller where [f_controllerID]= " + ControllerID;
			dbCommand.CommandText = text;
			dbDataReader = dbCommand.ExecuteReader();
			if (dbDataReader.Read())
			{
				controlConfigure.DoorInterlockSet(1, 0);
				controlConfigure.DoorInterlockSet(2, 0);
				controlConfigure.DoorInterlockSet(3, 0);
				controlConfigure.DoorInterlockSet(4, 0);
				if (wgAppConfig.getParamValBoolByNO(133))
				{
					switch ((int)dbDataReader["f_InterLock"])
					{
					case 1:
						controlConfigure.DoorInterlockSet(1, 49);
						controlConfigure.DoorInterlockSet(2, 50);
						break;
					case 2:
						controlConfigure.DoorInterlockSet(3, 196);
						controlConfigure.DoorInterlockSet(4, 200);
						break;
					case 3:
						controlConfigure.DoorInterlockSet(1, 49);
						controlConfigure.DoorInterlockSet(2, 50);
						controlConfigure.DoorInterlockSet(3, 196);
						controlConfigure.DoorInterlockSet(4, 200);
						break;
					case 4:
						controlConfigure.DoorInterlockSet(1, 113);
						controlConfigure.DoorInterlockSet(2, 114);
						controlConfigure.DoorInterlockSet(3, 116);
						break;
					case 8:
						controlConfigure.DoorInterlockSet(1, 241);
						controlConfigure.DoorInterlockSet(2, 242);
						controlConfigure.DoorInterlockSet(3, 244);
						controlConfigure.DoorInterlockSet(4, 248);
						break;
					}
				}
				if (wgAppConfig.getParamValBoolByNO(132))
				{
					controlConfigure.antiback = (int)dbDataReader["f_AntiBack"] % 10;
					controlConfigure.indoorPersonsMax = ((int)dbDataReader["f_AntiBack"] - controlConfigure.antiback) / 10;
				}
				else
				{
					controlConfigure.antiback = 0;
					controlConfigure.indoorPersonsMax = 0;
				}
				controlConfigure.moreCardRead4Reader = (int)dbDataReader["f_MoreCards_GoInOut"];
				int num9 = int.Parse(wgAppConfig.getSystemParamByNO(40));
				controlConfigure.doorOpenTimeout = num9;
				string[] array2 = wgTools.SetObjToStr(dbDataReader["f_PeripheralControl"]).Split(new char[] { ',' });
				if (!wgAppConfig.getParamValBoolByNO(124) || array2.Length != 27)
				{
					array2 = "126,30,30,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,10,10,10,0,0,0,0".Split(new char[] { ',' });
				}
				int[] array3 = new int[4];
				int[] array4 = new int[4];
				int[] array5 = new int[4];
				int[] array6 = new int[4];
				decimal[] array7 = new decimal[4];
				int[] array8 = new int[4];
				int i = 0;
				int num10 = int.Parse(array2[i++]);
				int num11 = int.Parse(array2[i++]);
				int num12 = int.Parse(array2[i++]);
				array3[0] = int.Parse(array2[i++]);
				array3[1] = int.Parse(array2[i++]);
				array3[2] = int.Parse(array2[i++]);
				array3[3] = int.Parse(array2[i++]);
				array4[0] = int.Parse(array2[i++]);
				array4[1] = int.Parse(array2[i++]);
				array4[2] = int.Parse(array2[i++]);
				array4[3] = int.Parse(array2[i++]);
				array5[0] = int.Parse(array2[i++]);
				array5[1] = int.Parse(array2[i++]);
				array5[2] = int.Parse(array2[i++]);
				array5[3] = int.Parse(array2[i++]);
				array6[0] = int.Parse(array2[i++]);
				array6[1] = int.Parse(array2[i++]);
				array6[2] = int.Parse(array2[i++]);
				array6[3] = int.Parse(array2[i++]);
				array7[0] = decimal.Parse(array2[i++], CultureInfo.InvariantCulture);
				array7[1] = decimal.Parse(array2[i++], CultureInfo.InvariantCulture);
				array7[2] = decimal.Parse(array2[i++], CultureInfo.InvariantCulture);
				array7[3] = decimal.Parse(array2[i++], CultureInfo.InvariantCulture);
				array8[0] = int.Parse(array2[i++]);
				array8[1] = int.Parse(array2[i++]);
				array8[2] = int.Parse(array2[i++]);
				array8[3] = int.Parse(array2[i++]);
				controlConfigure.ext_AlarmControlMode = num10;
				controlConfigure.ext_SetAlarmOnDelay = num11;
				controlConfigure.ext_SetAlarmOffDelay = num12;
				for (i = 0; i < 4; i++)
				{
					if (array8[i] > 0)
					{
						controlConfigure.Ext_doorSet(i, array3[i]);
						controlConfigure.Ext_controlSet(i, array4[i]);
						controlConfigure.Ext_warnSignalEnabledSet(i, array5[i]);
						controlConfigure.Ext_warnSignalEnabled2Set(i, array6[i]);
						controlConfigure.Ext_timeoutSet(i, (int)array7[i]);
					}
					else
					{
						controlConfigure.Ext_doorSet(i, 0);
						controlConfigure.Ext_controlSet(i, 0);
						controlConfigure.Ext_warnSignalEnabledSet(i, 0);
						controlConfigure.Ext_warnSignalEnabled2Set(i, 0);
						controlConfigure.Ext_timeoutSet(i, 0);
					}
				}
				int num13 = 0;
				num13 += (((int)dbDataReader["f_ForceWarn"] > 0) ? 1 : 0);
				num13 += (((int)dbDataReader["f_DoorOpenTooLong"] > 0) ? 2 : 0);
				num13 += (((int)dbDataReader["f_DoorInvalidOpen"] > 0) ? 4 : 0);
				num13 += 8;
				num13 += (((int)dbDataReader["f_InvalidCardWarn"] > 0) ? 16 : 0);
				num13 += 32;
				if ((array3[0] == 16 && array8[0] > 0) || (array3[1] == 16 && array8[1] > 0) || (array3[2] == 16 && array8[2] > 0) || (array3[3] == 16 && array8[3] > 0))
				{
					num13 += 64;
				}
				else
				{
					controlConfigure.ext_Alarm_Status = 0;
				}
				if (wgAppConfig.getParamValBoolByNO(141))
				{
					num13 += 128;
				}
				if (!wgAppConfig.getParamValBoolByNO(124))
				{
					num13 = 0;
				}
				controlConfigure.warnSetup = num13;
				controlConfigure.xpPassword = int.Parse(wgAppConfig.getSystemParamByNO(24));
				controlConfigure.open_too_long = (wgAppConfig.IsActivateOpenTooLongWarn ? 165 : 0);
				if (wgAppConfig.getParamValBoolByNO(211))
				{
					controlConfigure.invalid_swipe_warntimeout = int.Parse(wgAppConfig.getSystemParamByNO(211));
				}
				else
				{
					controlConfigure.invalid_swipe_warntimeout = 0;
				}
				if (!wgAppConfig.getParamValBoolByNO(124) || !wgAppConfig.getParamValBoolByNO(60))
				{
					controlConfigure.fire_broadcast_receive = 0;
					controlConfigure.fire_broadcast_send = 0;
				}
				else
				{
					controlConfigure.fire_broadcast_receive = 15;
					controlConfigure.fire_broadcast_send = 1;
					if (wgAppConfig.getSystemParamByNO(60) == "2" && ((num3 > 0) & (num3 < 253)))
					{
						controlConfigure.fire_broadcast_send = num3 + 1;
					}
				}
				if (!wgAppConfig.getParamValBoolByNO(133) || !wgAppConfig.getParamValBoolByNO(61) || wgMjController.GetControllerType(num2) == 1)
				{
					controlConfigure.interlock_broadcast_receive = 0;
					controlConfigure.interlock_broadcast_send = 0;
				}
				else
				{
					controlConfigure.interlock_broadcast_receive = 5;
					controlConfigure.interlock_broadcast_send = 1;
					if (wgAppConfig.getSystemParamByNO(61) == "2" && ((num3 > 0) & (num3 < 253)))
					{
						controlConfigure.interlock_broadcast_send = num3 + 1;
					}
				}
				if (!wgAppConfig.getParamValBoolByNO(132) || !wgAppConfig.getParamValBoolByNO(62))
				{
					controlConfigure.antiback_broadcast_send = 0;
					if (controlConfigure.indoorPersonsMax > 0)
					{
						controlConfigure.antiback_broadcast_send = 254;
					}
				}
				else
				{
					controlConfigure.antiback_broadcast_send = 1;
					if (wgAppConfig.getSystemParamByNO(62) == "2" && ((num3 > 0) & (num3 < 253)))
					{
						controlConfigure.antiback_broadcast_send = num3 + 1;
					}
				}
				controlConfigure.receventWarn = ((num13 > 0) ? 1 : 0);
				controlConfigure.receventPB = (wgAppConfig.getParamValBoolByNO(101) ? 1 : 0);
				controlConfigure.receventDS = (wgAppConfig.getParamValBoolByNO(102) ? 1 : 0);
				controlConfigure.mobile_as_card_input = (wgAppConfig.getParamValBoolByNO(152) ? 165 : 0);
				controlConfigure.invalidCard_ledbeep_output_disable = (wgAppConfig.getParamValBoolByNO(165) ? 165 : 0);
				controlConfigure.disablePushbuttonSet(1, 0);
				controlConfigure.disablePushbuttonSet(2, 0);
				controlConfigure.disablePushbuttonSet(3, 0);
				controlConfigure.disablePushbuttonSet(4, 0);
			}
			dbDataReader.Close();
			int j = 0;
			while (j < 16)
			{
				j++;
				controlConfigure.SuperpasswordSet(j, 16777215);
			}
			if (paramValBoolByNO)
			{
				text = " SELECT f_ReaderNO  from t_b_Reader  ";
				text = text + " where [t_b_Reader].[f_ControllerID] = " + ControllerID.ToString() + " order by [f_ReaderNO] ASC";
				dbCommand.CommandText = text;
				ArrayList arrayList = new ArrayList();
				dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					arrayList.Add(dbDataReader["f_ReaderNO"]);
				}
				dbDataReader.Close();
				text = " SELECT f_Password,t_b_Reader.f_ReaderNO,t_b_ReaderPassword.f_BAll,t_b_ReaderPassword.f_ReaderID   from t_b_ReaderPassword LEFT JOIN  t_b_Reader ON t_b_ReaderPassword.f_ReaderID = t_b_Reader.f_ReaderID ";
				text = text + " where [f_BAll] = 1 Or [t_b_Reader].[f_ControllerID] = " + ControllerID.ToString();
				dbCommand.CommandText = text;
				dbDataReader = dbCommand.ExecuteReader();
				int[] array9 = new int[] { 1, 1, 1, 1 };
				while (dbDataReader.Read())
				{
					if ((int)dbDataReader["f_BAll"] == 1)
					{
						if (array9[0] <= 4)
						{
							controlConfigure.SuperpasswordSet(array9[0]++, (int)dbDataReader["f_Password"]);
						}
						if (array9[1] <= 4)
						{
							controlConfigure.SuperpasswordSet(4 + array9[1]++, (int)dbDataReader["f_Password"]);
						}
						if (array9[2] <= 4)
						{
							controlConfigure.SuperpasswordSet(8 + array9[2]++, (int)dbDataReader["f_Password"]);
						}
						if (array9[3] <= 4)
						{
							controlConfigure.SuperpasswordSet(12 + array9[3]++, (int)dbDataReader["f_Password"]);
						}
					}
					else
					{
						j = arrayList.IndexOf(dbDataReader["f_ReaderNO"]);
						if (array9[j] <= 4)
						{
							controlConfigure.SuperpasswordSet(array9[j] + j * 4, (int)dbDataReader["f_Password"]);
							array9[j]++;
						}
					}
				}
				dbDataReader.Close();
			}
			controlConfigure.FirstCardInfoSet(1, 0);
			controlConfigure.FirstCardInfoSet(2, 0);
			controlConfigure.FirstCardInfoSet(3, 0);
			controlConfigure.FirstCardInfoSet(4, 0);
			controlTaskList = new wgMjControllerTaskList();
			if (wgAppConfig.getParamValBoolByNO(135))
			{
				text = " SELECT  f_FirstCard_Enabled,f_DoorNO ";
				text = text + ", f_FirstCard_BeginHMS, f_FirstCard_BeginControl , f_FirstCard_EndHMS , f_FirstCard_EndControl, f_FirstCard_Weekday  FROM  t_b_door Where f_FirstCard_Enabled> 0 AND [f_ControllerID] = " + ControllerID.ToString() + " ORDER BY f_DoorNO ";
				dbCommand.CommandText = text;
				dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					MjControlTaskItem mjControlTaskItem = new MjControlTaskItem();
					mjControlTaskItem.ymdStart = DateTime.Parse("2010-1-1");
					mjControlTaskItem.ymdEnd = DateTime.Parse("2099-12-31");
					mjControlTaskItem.hms = wgTools.wgDateTimeParse(dbDataReader["f_FirstCard_BeginHMS"]);
					mjControlTaskItem.weekdayControl = (byte)((int)dbDataReader["f_FirstCard_Weekday"]);
					switch ((int)dbDataReader["f_FirstCard_BeginControl"])
					{
					case 0:
						mjControlTaskItem.paramValue = 19;
						break;
					case 1:
						mjControlTaskItem.paramValue = 17;
						break;
					case 2:
						mjControlTaskItem.paramValue = 18;
						break;
					case 3:
						mjControlTaskItem.paramValue = 20;
						break;
					default:
						mjControlTaskItem.paramValue = 0;
						break;
					}
					mjControlTaskItem.paramLoc = (int)(180 + (byte)dbDataReader["f_DoorNO"] - 1);
					if (controlTaskList.AddItem(mjControlTaskItem) < 0)
					{
						wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
					}
					mjControlTaskItem = new MjControlTaskItem();
					mjControlTaskItem.ymdStart = DateTime.Parse("2010-1-1");
					mjControlTaskItem.ymdEnd = DateTime.Parse("2099-12-31");
					mjControlTaskItem.hms = wgTools.wgDateTimeParse(dbDataReader["f_FirstCard_EndHMS"]);
					mjControlTaskItem.weekdayControl = (byte)((int)dbDataReader["f_FirstCard_Weekday"]);
					switch ((int)dbDataReader["f_FirstCard_EndControl"])
					{
					case 0:
						mjControlTaskItem.paramValue = 0;
						break;
					case 1:
						mjControlTaskItem.paramValue = 0;
						break;
					case 2:
						mjControlTaskItem.paramValue = 0;
						break;
					case 3:
						mjControlTaskItem.paramValue = 4;
						mjControlTaskItem.paramValue += 16;
						break;
					default:
						mjControlTaskItem.paramValue = 0;
						break;
					}
					mjControlTaskItem.paramLoc = (int)(180 + (byte)dbDataReader["f_DoorNO"] - 1);
					if (controlTaskList.AddItem(mjControlTaskItem) < 0)
					{
						wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
					}
					mjControlTaskItem = new MjControlTaskItem();
					mjControlTaskItem.ymdStart = DateTime.Parse("2010-1-1");
					mjControlTaskItem.ymdEnd = DateTime.Parse("2099-12-31");
					mjControlTaskItem.hms = wgTools.wgDateTimeParse(dbDataReader["f_FirstCard_EndHMS"]);
					mjControlTaskItem.weekdayControl = (byte)((int)dbDataReader["f_FirstCard_Weekday"]);
					switch ((int)dbDataReader["f_FirstCard_EndControl"])
					{
					case 0:
						mjControlTaskItem.paramValue = 3;
						break;
					case 1:
						mjControlTaskItem.paramValue = 1;
						break;
					case 2:
						mjControlTaskItem.paramValue = 2;
						break;
					case 3:
						mjControlTaskItem.paramValue = 3;
						break;
					default:
						mjControlTaskItem.paramValue = 3;
						break;
					}
					mjControlTaskItem.paramLoc = (int)(26 + (byte)dbDataReader["f_DoorNO"] - 1);
					if (controlTaskList.AddItem(mjControlTaskItem) < 0)
					{
						wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
					}
				}
				dbDataReader.Close();
			}
			if (wgAppConfig.getParamValBoolByNO(131))
			{
				bool flag = false;
				text = " SELECT t_b_ControllerTaskList.*,t_b_Door.f_DoorNO, t_b_Door.f_ControllerID FROM t_b_ControllerTaskList ";
				text = text + " LEFT JOIN t_b_Door ON t_b_ControllerTaskList.f_DoorID = t_b_Door.f_DoorID  where t_b_ControllerTaskList.[f_DoorID]=0 OR [f_controllerID]= " + ControllerID.ToString();
				dbCommand.CommandText = text;
				dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					MjControlTaskItem mjControlTaskItem2 = new MjControlTaskItem();
					mjControlTaskItem2.ymdStart = (DateTime)dbDataReader["f_BeginYMD"];
					mjControlTaskItem2.ymdEnd = (DateTime)dbDataReader["f_EndYMD"];
					mjControlTaskItem2.hms = (DateTime)dbDataReader["f_OperateTime"];
					int num14 = 0;
					num14 = num14 * 2 + (int)((byte)dbDataReader["f_Sunday"]);
					num14 = num14 * 2 + (int)((byte)dbDataReader["f_Saturday"]);
					num14 = num14 * 2 + (int)((byte)dbDataReader["f_Friday"]);
					num14 = num14 * 2 + (int)((byte)dbDataReader["f_Thursday"]);
					num14 = num14 * 2 + (int)((byte)dbDataReader["f_Wednesday"]);
					num14 = num14 * 2 + (int)((byte)dbDataReader["f_Tuesday"]);
					num14 = num14 * 2 + (int)((byte)dbDataReader["f_Monday"]);
					mjControlTaskItem2.weekdayControl = (byte)num14;
					mjControlTaskItem2.paramLoc = 0;
					if ((int)dbDataReader["f_DoorID"] == 0)
					{
						switch ((int)dbDataReader["f_DoorControl"])
						{
						case 0:
						{
							mjControlTaskItem2.paramValue = 3;
							for (int k = 0; k < wgMjController.GetControllerType(num2); k++)
							{
								MjControlTaskItem mjControlTaskItem3 = new MjControlTaskItem();
								mjControlTaskItem3.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem3.paramLoc = 26 + k;
								controlTaskList.AddItem(mjControlTaskItem3);
							}
							break;
						}
						case 1:
						{
							mjControlTaskItem2.paramValue = 1;
							for (int l = 0; l < wgMjController.GetControllerType(num2); l++)
							{
								MjControlTaskItem mjControlTaskItem4 = new MjControlTaskItem();
								mjControlTaskItem4.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem4.paramLoc = 26 + l;
								controlTaskList.AddItem(mjControlTaskItem4);
							}
							break;
						}
						case 2:
						{
							mjControlTaskItem2.paramValue = 2;
							for (int m = 0; m < wgMjController.GetControllerType(num2); m++)
							{
								MjControlTaskItem mjControlTaskItem5 = new MjControlTaskItem();
								mjControlTaskItem5.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem5.paramLoc = 26 + m;
								controlTaskList.AddItem(mjControlTaskItem5);
							}
							break;
						}
						case 3:
						case 4:
						{
							mjControlTaskItem2.paramValue = 0;
							if ((int)dbDataReader["f_DoorControl"] == 3)
							{
								mjControlTaskItem2.paramValue = 2;
							}
							for (int n = 0; n < wgMjController.GetControllerType(num2); n++)
							{
								MjControlTaskItem mjControlTaskItem6 = new MjControlTaskItem();
								mjControlTaskItem6.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem6.paramLoc = 256 + n;
								controlTaskList.AddItem(mjControlTaskItem6);
							}
							break;
						}
						case 5:
						case 6:
						case 7:
						{
							mjControlTaskItem2.paramValue = 0;
							if ((int)dbDataReader["f_DoorControl"] == 7 || (int)dbDataReader["f_DoorControl"] == 6)
							{
								mjControlTaskItem2.paramValue = 1;
							}
							for (int num15 = 0; num15 < 4; num15++)
							{
								MjControlTaskItem mjControlTaskItem7 = new MjControlTaskItem();
								mjControlTaskItem7.CopyFrom(mjControlTaskItem2);
								if (wgMjController.GetControllerType(num2) != 4 && (int)dbDataReader["f_DoorControl"] == 6 && (num15 == 1 || num15 == 3))
								{
									mjControlTaskItem7.paramValue = 0;
								}
								mjControlTaskItem7.paramLoc = 38 + num15;
								controlTaskList.AddItem(mjControlTaskItem7);
							}
							break;
						}
						case 8:
						case 9:
						{
							mjControlTaskItem2.paramValue = 0;
							for (int num16 = 0; num16 < wgMjController.GetControllerType(num2); num16++)
							{
								if ((int)dbDataReader["f_DoorControl"] == 8)
								{
									mjControlTaskItem2.paramValue = (byte)controlConfigure.MorecardNeedCardsGet(num16 + 1);
								}
								MjControlTaskItem mjControlTaskItem8 = new MjControlTaskItem();
								mjControlTaskItem8.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem8.paramLoc = 184 + num16;
								controlTaskList.AddItem(mjControlTaskItem8);
							}
							break;
						}
						case 10:
						{
							mjControlTaskItem2.paramValue = 0;
							for (int num17 = 0; num17 < wgMjController.GetControllerType(num2); num17++)
							{
								mjControlTaskItem2.paramValue += (byte)(1 << num17);
							}
							mjControlTaskItem2.paramLoc = 55;
							controlTaskList.AddItem(mjControlTaskItem2);
							break;
						}
						case 11:
						{
							mjControlTaskItem2.paramValue = 1;
							for (int num18 = 0; num18 < wgMjController.GetControllerType(num2); num18++)
							{
								MjControlTaskItem mjControlTaskItem9 = new MjControlTaskItem();
								mjControlTaskItem9.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem9.paramValue = 1;
								mjControlTaskItem9.paramLoc = 262 + num18;
								controlTaskList.AddItem(mjControlTaskItem9);
							}
							break;
						}
						case 12:
						{
							mjControlTaskItem2.paramValue = 0;
							for (int num19 = 0; num19 < wgMjController.GetControllerType(num2); num19++)
							{
								MjControlTaskItem mjControlTaskItem10 = new MjControlTaskItem();
								mjControlTaskItem10.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem10.paramValue = 0;
								mjControlTaskItem10.paramLoc = 262 + num19;
								controlTaskList.AddItem(mjControlTaskItem10);
							}
							break;
						}
						case 13:
							if (!wgAppConfig.IsAccessControlBlue && controlConfigure.warnSetup != 0)
							{
								mjControlTaskItem2.paramValue = 0;
								MjControlTaskItem mjControlTaskItem11 = new MjControlTaskItem();
								mjControlTaskItem11.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem11.paramValue = 0;
								mjControlTaskItem11.paramLoc = 48;
								controlTaskList.AddItem(mjControlTaskItem11);
							}
							break;
						case 14:
							if (!wgAppConfig.IsAccessControlBlue && controlConfigure.warnSetup != 0)
							{
								mjControlTaskItem2.paramValue = (byte)controlConfigure.warnSetup;
								MjControlTaskItem mjControlTaskItem12 = new MjControlTaskItem();
								mjControlTaskItem12.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem12.paramValue = (byte)controlConfigure.warnSetup;
								mjControlTaskItem12.paramLoc = 48;
								controlTaskList.AddItem(mjControlTaskItem12);
								flag = true;
							}
							break;
						default:
							mjControlTaskItem2.paramValue = 0;
							mjControlTaskItem2.paramLoc = 0;
							break;
						}
					}
					else
					{
						switch ((int)dbDataReader["f_DoorControl"])
						{
						case 0:
							mjControlTaskItem2.paramValue = 3;
							mjControlTaskItem2.paramLoc = (int)(26 + (byte)dbDataReader["f_DoorNO"] - 1);
							if (controlTaskList.AddItem(mjControlTaskItem2) < 0)
							{
								wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
							}
							break;
						case 1:
							mjControlTaskItem2.paramValue = 1;
							mjControlTaskItem2.paramLoc = (int)(26 + (byte)dbDataReader["f_DoorNO"] - 1);
							if (controlTaskList.AddItem(mjControlTaskItem2) < 0)
							{
								wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
							}
							break;
						case 2:
							mjControlTaskItem2.paramValue = 2;
							mjControlTaskItem2.paramLoc = (int)(26 + (byte)dbDataReader["f_DoorNO"] - 1);
							if (controlTaskList.AddItem(mjControlTaskItem2) < 0)
							{
								wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
							}
							break;
						case 3:
						case 4:
							mjControlTaskItem2.paramValue = 0;
							if ((int)dbDataReader["f_DoorControl"] == 3)
							{
								mjControlTaskItem2.paramValue = 2;
							}
							mjControlTaskItem2.paramLoc = 256 + (int)((byte)dbDataReader["f_DoorNO"]) - 1;
							controlTaskList.AddItem(mjControlTaskItem2);
							break;
						case 5:
						case 6:
						case 7:
							mjControlTaskItem2.paramValue = 0;
							if ((int)dbDataReader["f_DoorControl"] == 7 || (int)dbDataReader["f_DoorControl"] == 6)
							{
								mjControlTaskItem2.paramValue = 1;
							}
							if (wgMjController.GetControllerType(num2) == 4)
							{
								mjControlTaskItem2.paramLoc = (int)(38 + (byte)dbDataReader["f_DoorNO"] - 1);
								controlTaskList.AddItem(mjControlTaskItem2);
							}
							else if ((byte)dbDataReader["f_DoorNO"] <= 2)
							{
								mjControlTaskItem2.paramLoc = (int)(38 + ((byte)dbDataReader["f_DoorNO"] - 1) * 2);
								MjControlTaskItem mjControlTaskItem13 = new MjControlTaskItem();
								mjControlTaskItem13.CopyFrom(mjControlTaskItem2);
								controlTaskList.AddItem(mjControlTaskItem13);
								if ((int)dbDataReader["f_DoorControl"] == 6)
								{
									mjControlTaskItem2.paramValue = 0;
								}
								mjControlTaskItem2.paramLoc = (int)(38 + ((byte)dbDataReader["f_DoorNO"] - 1) * 2 + 1);
								controlTaskList.AddItem(mjControlTaskItem2);
							}
							break;
						case 8:
						case 9:
							mjControlTaskItem2.paramValue = 0;
							if ((int)dbDataReader["f_DoorControl"] == 8)
							{
								mjControlTaskItem2.paramValue = (byte)controlConfigure.MorecardNeedCardsGet((int)((byte)dbDataReader["f_DoorNO"]));
							}
							mjControlTaskItem2.paramLoc = (int)(184 + (byte)dbDataReader["f_DoorNO"] - 1);
							controlTaskList.AddItem(mjControlTaskItem2);
							break;
						case 10:
							mjControlTaskItem2.paramValue = (byte)(1 << (int)((byte)dbDataReader["f_DoorNO"] - 1));
							mjControlTaskItem2.paramLoc = 55;
							controlTaskList.AddItem(mjControlTaskItem2);
							break;
						case 11:
							mjControlTaskItem2.paramValue = 1;
							mjControlTaskItem2.paramLoc = 262 + (int)((byte)dbDataReader["f_DoorNO"]) - 1;
							controlTaskList.AddItem(mjControlTaskItem2);
							break;
						case 12:
							mjControlTaskItem2.paramValue = 0;
							mjControlTaskItem2.paramLoc = 262 + (int)((byte)dbDataReader["f_DoorNO"]) - 1;
							controlTaskList.AddItem(mjControlTaskItem2);
							break;
						case 13:
							if (!wgAppConfig.IsAccessControlBlue && controlConfigure.warnSetup != 0)
							{
								mjControlTaskItem2.paramValue = 0;
								MjControlTaskItem mjControlTaskItem14 = new MjControlTaskItem();
								mjControlTaskItem14.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem14.paramValue = 0;
								mjControlTaskItem14.paramLoc = 48;
								controlTaskList.AddItem(mjControlTaskItem14);
							}
							break;
						case 14:
							if (!wgAppConfig.IsAccessControlBlue && controlConfigure.warnSetup != 0)
							{
								mjControlTaskItem2.paramValue = (byte)controlConfigure.warnSetup;
								MjControlTaskItem mjControlTaskItem15 = new MjControlTaskItem();
								mjControlTaskItem15.CopyFrom(mjControlTaskItem2);
								mjControlTaskItem15.paramValue = (byte)controlConfigure.warnSetup;
								mjControlTaskItem15.paramLoc = 48;
								controlTaskList.AddItem(mjControlTaskItem15);
								flag = true;
							}
							break;
						default:
							mjControlTaskItem2.paramValue = 0;
							mjControlTaskItem2.paramLoc = 0;
							break;
						}
					}
				}
				dbDataReader.Close();
				if (flag)
				{
					controlConfigure.warnSetup = 0;
				}
			}
			if (wgMjController.IsElevator(num2))
			{
				controlConfigure.DoorControlSet(2, 2);
				controlConfigure.DoorDelaySet(2, 0);
				controlConfigure.DoorControlSet(3, 2);
				controlConfigure.DoorDelaySet(3, 0);
				controlConfigure.DoorControlSet(4, 2);
				controlConfigure.DoorDelaySet(4, 0);
				text = " SELECT t_b_Floor.*  from t_b_Floor  ";
				text = text + " where [t_b_Floor].[f_ControllerID] = " + ControllerID.ToString() + " order by [f_FloorNO] ASC";
				dbCommand.CommandText = text;
				ArrayList arrayList2 = new ArrayList();
				ArrayList arrayList3 = new ArrayList();
				dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					arrayList2.Add(dbDataReader["f_FloorNO"]);
					arrayList3.Add(dbDataReader["f_FloorID"]);
				}
				dbDataReader.Close();
				if (arrayList2.Count > 0)
				{
					text = " SELECT t_b_ControllerTaskList4Floor.* FROM t_b_ControllerTaskList4Floor ";
					dbCommand.CommandText = text;
					dbDataReader = dbCommand.ExecuteReader();
					bool flag2 = false;
					while (dbDataReader.Read())
					{
						int num20 = 0;
						string[] array10 = dbDataReader["f_notes"].ToString().Split(new char[] { ',' });
						bool flag3 = false;
						for (int num21 = 0; num21 < 40; num21++)
						{
							int num22 = arrayList2.IndexOf(num21 + 1);
							if (num22 >= 0)
							{
								bool flag4 = false;
								foreach (string text8 in array10)
								{
									if (text8 == arrayList3[num22].ToString())
									{
										flag4 = true;
									}
								}
								if (flag4)
								{
									flag3 = true;
									num20 += 1 << (num21 & 7);
								}
							}
							if ((num21 & 7) == 7 && flag3)
							{
								MjControlTaskItem mjControlTaskItem16 = new MjControlTaskItem();
								mjControlTaskItem16.ymdStart = (DateTime)dbDataReader["f_BeginYMD"];
								mjControlTaskItem16.ymdEnd = (DateTime)dbDataReader["f_EndYMD"];
								mjControlTaskItem16.hms = (DateTime)dbDataReader["f_OperateTime"];
								int num24 = 0;
								num24 = num24 * 2 + (int)((byte)dbDataReader["f_Sunday"]);
								num24 = num24 * 2 + (int)((byte)dbDataReader["f_Saturday"]);
								num24 = num24 * 2 + (int)((byte)dbDataReader["f_Friday"]);
								num24 = num24 * 2 + (int)((byte)dbDataReader["f_Thursday"]);
								num24 = num24 * 2 + (int)((byte)dbDataReader["f_Wednesday"]);
								num24 = num24 * 2 + (int)((byte)dbDataReader["f_Tuesday"]);
								num24 = num24 * 2 + (int)((byte)dbDataReader["f_Monday"]);
								mjControlTaskItem16.weekdayControl = (byte)num24;
								mjControlTaskItem16.paramLoc = 20 + (num21 >> 3);
								switch ((int)dbDataReader["f_DoorControl"])
								{
								case 0:
									mjControlTaskItem16.paramValue = 0;
									if (controlTaskList.AddItem(mjControlTaskItem16) < 0)
									{
										wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
									}
									break;
								case 1:
									mjControlTaskItem16.paramValue = (byte)(num20 & 255);
									if (controlTaskList.AddItem(mjControlTaskItem16) < 0)
									{
										wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
									}
									flag2 = true;
									break;
								default:
									mjControlTaskItem16.paramValue = 0;
									mjControlTaskItem16.paramLoc = 0;
									break;
								}
								num20 = 0;
								flag3 = false;
							}
						}
					}
					dbDataReader.Close();
					if (flag2)
					{
						for (int num25 = 0; num25 < 3; num25++)
						{
							MjControlTaskItem mjControlTaskItem17 = new MjControlTaskItem();
							mjControlTaskItem17.ymdStart = DateTime.Parse("2010-1-1");
							mjControlTaskItem17.ymdEnd = DateTime.Parse("2099-12-31");
							mjControlTaskItem17.hms = DateTime.Parse("2018-07-08");
							int num26 = 0;
							num26 = num26 * 2 + 1;
							num26 = num26 * 2 + 1;
							num26 = num26 * 2 + 1;
							num26 = num26 * 2 + 1;
							num26 = num26 * 2 + 1;
							num26 = num26 * 2 + 1;
							num26 = num26 * 2 + 1;
							mjControlTaskItem17.weekdayControl = (byte)num26;
							mjControlTaskItem17.paramLoc = 27 + num25;
							mjControlTaskItem17.paramValue = 1;
							if (controlTaskList.AddItem(mjControlTaskItem17) < 0)
							{
								wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
							}
						}
					}
				}
			}
			controlConfigure.controlTaskList_enabled = ((controlTaskList.taskCount > 0) ? 1 : 0);
			if (wgAppConfig.getParamValBoolByNO(121))
			{
				text = " SELECT * FROM t_b_ControlHolidays ";
				dbCommand.CommandText = text;
				dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					MjControlHolidayTime mjControlHolidayTime = new MjControlHolidayTime();
					mjControlHolidayTime.dtStart = (DateTime)dbDataReader["f_BeginYMDHMS"];
					mjControlHolidayTime.dtEnd = (DateTime)dbDataReader["f_EndYMDHMS"];
					mjControlHolidayTime.bForceWork = (int)dbDataReader["f_forceWork"] == 1;
					controlHolidayList.AddItem(mjControlHolidayTime);
				}
				dbDataReader.Close();
			}
			if (controlHolidayList.holidayCount > 0)
			{
				controlConfigure.holidayControl = 1;
			}
			else
			{
				controlConfigure.holidayControl = 0;
			}
			if (wgAppConfig.getParamValBoolByNO(144) && wgMjController.IsElevator(num2))
			{
				int num27 = 0;
				text = " SELECT * FROM t_b_Floor WHERE [f_ControllerID] = " + ControllerID.ToString();
				dbCommand.CommandText = text;
				dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					if ((int)dbDataReader["f_FloorNO"] > 0)
					{
						if ((int)dbDataReader["f_FloorNO"] <= 20)
						{
							num27 |= 1;
						}
						else if ((int)dbDataReader["f_FloorNO"] <= 40)
						{
							num27 |= 2;
						}
					}
				}
				dbDataReader.Close();
				try
				{
					int num28 = int.Parse("0" + wgAppConfig.getSystemParamByNO(144));
					if (num28 > 3)
					{
						controlConfigure.elevatorSingleDelay = (float)(((num28 >> 8) & 255) / 10m);
						controlConfigure.elevatorMultioutputDelay = (float)(((num28 >> 16) & 255) / 10m);
					}
					else
					{
						controlConfigure.elevatorSingleDelay = 0.4f;
						controlConfigure.elevatorMultioutputDelay = 5f;
					}
				}
				catch (Exception)
				{
				}
				controlConfigure.DoorControlSet(2, 3);
				controlConfigure.DoorDelaySet(2, 0);
				controlConfigure.DoorControlSet(3, 3);
				controlConfigure.DoorDelaySet(3, 0);
				controlConfigure.DoorControlSet(4, 3);
				controlConfigure.DoorDelaySet(4, 0);
			}
			if (!wgAppConfig.IsAccessControlBlue)
			{
				if (wgAppConfig.getParamValBoolByNO(169))
				{
					text = "  SELECT  t_b_Reader.f_ReaderID , t_b_Reader.[f_ReaderName], t_b_Reader.f_ReaderNO, t_b_Controller.f_ControllerSN, t_b_Controller.f_ControllerID  ";
					text = text + " FROM  t_b_Reader,t_d_Reader4Meal  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  AND t_b_Reader.f_ReaderID = t_d_Reader4Meal.f_ReaderID  AND   t_b_Controller.f_ControllerID =   " + ControllerID.ToString();
					dbCommand.CommandText = text;
					dbDataReader = dbCommand.ExecuteReader();
					if (dbDataReader.Read())
					{
						controlConfigure.pcControlSwipeTimeout = 30;
					}
					else
					{
						controlConfigure.pcControlSwipeTimeout = 0;
					}
					dbDataReader.Close();
				}
				if (wgAppConfig.getParamValBoolByNO(181))
				{
					controlConfigure.pcControlSwipeTimeout = 0;
				}
			}
			return 1;
		}
	}
}
