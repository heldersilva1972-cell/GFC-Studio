using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.DataOper;
using WG3000_COMM.ExtendFunc.FaceReader;
using WG3000_COMM.ExtendFunc._2019SwipeSendToCenter;

namespace WG3000_COMM.Core
{
	// Token: 0x02000067 RID: 103
	public class batchAutoRun
	{
		// Token: 0x06000799 RID: 1945 RVA: 0x000D8540 File Offset: 0x000D7540
		public static int commandSpecialCall(string[] cmdArgsParam)
		{
			bool flag = false;
			if (cmdArgsParam.Length <= 0)
			{
				return -1;
			}
			string[] array = new string[cmdArgsParam.Length];
			for (int i = 0; i < cmdArgsParam.Length; i++)
			{
				array[i] = cmdArgsParam[i];
			}
			try
			{
				if ((array[0].ToUpper().Replace("–", "-") == "-USER" || array[0].ToUpper().Replace("–", "-") == "-AUTOLOGIN") && Program.dbConnectionCheck() > 0)
				{
					if (array[0].ToUpper().Replace("–", "-") == "-USER")
					{
						if (array.Length > 4 && array[2].ToUpper().Replace("–", "-") == "-PASSWORD")
						{
							flag = icOperator.login(array[1].Replace("'", ""), array[3].Replace("'", ""));
							if (array[3].Length > 0)
							{
								array[3] = "*".PadLeft(array[3].Length, '*');
							}
						}
					}
					else if (array[0].ToUpper().Replace("–", "-") == "-AUTOLOGIN" && wgAppConfig.GetKeyVal("autologinName") != "")
					{
						string keyVal = wgAppConfig.GetKeyVal("autologinName");
						try
						{
							string text = Program.Dpt4Database(wgAppConfig.GetKeyVal("autologinPassword"));
							flag = icOperator.login(keyVal, text);
						}
						catch (Exception ex)
						{
							wgTools.WgDebugWrite(ex.ToString(), new object[0]);
						}
					}
				}
				if (!flag)
				{
					return -1;
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
			int num = 0;
			string text2 = "N3000";
			for (int j = 0; j < array.Length; j++)
			{
				text2 = text2 + " " + array[j];
			}
			batchAutoRun.wglogRecEventOfController(text2);
			wgTools.CommPStr = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("CommPCurrent"));
			try
			{
				for (int k = 0; k < array.Length; k++)
				{
					if (array[k].ToUpper().Replace("–", "-") == "-LOGIN".ToUpper())
					{
						frmLogin frmLogin = new frmLogin();
						frmLogin.Show();
						wgAppConfig.LoginTitle = frmLogin.Text;
						frmLogin.Close();
						new frmADCT3000().ShowDialog();
						if (wgAppConfig.IsAccessDB)
						{
							dfrmWait dfrmWait = new dfrmWait();
							dfrmWait.Show();
							dfrmWait.Refresh();
							wgAppConfig.backupBeforeExitByJustCopy();
							dfrmWait.Hide();
							dfrmWait.Close();
						}
						return num;
					}
				}
				long num2 = -1L;
				for (int l = 0; l < array.Length; l++)
				{
					if (array[l].ToUpper().Replace("–", "-") == "-CARDNO".ToUpper())
					{
						try
						{
							num2 = long.Parse(array[l + 1]);
							array = new string[cmdArgsParam.Length - 2];
							int num3 = 0;
							for (int m = 0; m < cmdArgsParam.Length; m++)
							{
								if (m == l)
								{
									m++;
								}
								else
								{
									array[num3] = cmdArgsParam[m];
									num3++;
								}
							}
						}
						catch (Exception ex3)
						{
							wgAppConfig.wgLog(ex3.ToString());
						}
					}
				}
				for (int n = 0; n < array.Length; n++)
				{
					if (array[n].ToUpper().Replace("–", "-") == "-OPEN".ToUpper())
					{
						int num4 = 0;
						if (array.Length == n + 2)
						{
							string text3 = array[n + 1];
							MjRec mjRec = new MjRec(text3);
							int num5 = (int)mjRec.ControllerSN;
							int num6 = (int)mjRec.DoorNo;
							bool flag2 = false;
							if (num5 <= 0)
							{
								int num7 = wgAppConfig.getValBySql(string.Format(" SELECT [f_ControllerSN]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(text3)));
								if (num7 > 0)
								{
									num5 = num7;
									num7 = wgAppConfig.getValBySql(string.Format(" SELECT [f_DoorNO]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(text3)));
									num6 = 1;
									if (num7 > 0)
									{
										num6 = num7;
									}
								}
								else
								{
									string text4 = "  SELECT t_b_Controller.f_ControllerSN, t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
									num7 = wgAppConfig.getValBySql(text4 + "   t_b_Door.f_DoorName,    t_b_Floor.f_floorNO, t_b_Controller.f_ZoneID, t_b_Floor.f_floorName  FROM (t_b_Floor LEFT JOIN t_b_Door ON t_b_Floor.f_DoorID = t_b_Door.f_DoorID) LEFT JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID WHERE  (  t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName ) =  " + wgTools.PrepareStrNUnicode(text3));
									if (num7 > 0)
									{
										flag2 = true;
										num5 = num7;
										text4 = "  SELECT  t_b_Floor.f_floorNO,t_b_Controller.f_ControllerSN, t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
										num7 = wgAppConfig.getValBySql(text4 + "   t_b_Door.f_DoorName,    t_b_Controller.f_ZoneID, t_b_Floor.f_floorName  FROM (t_b_Floor LEFT JOIN t_b_Door ON t_b_Floor.f_DoorID = t_b_Door.f_DoorID) LEFT JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID WHERE  (  t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName ) =  " + wgTools.PrepareStrNUnicode(text3));
										num4 = 1;
										if (num7 > 0)
										{
											num4 = num7;
										}
									}
									else
									{
										text4 = "  SELECT t_b_Controller.f_ControllerSN, t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
										num7 = wgAppConfig.getValBySql(text4 + "   t_b_Door.f_DoorName,    t_b_Floor.f_floorNO, t_b_Controller.f_ZoneID, t_b_Floor.f_floorName  FROM (t_b_Floor LEFT JOIN t_b_Door ON t_b_Floor.f_DoorID = t_b_Door.f_DoorID) LEFT JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID WHERE  (  t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName + '.NC') =  " + wgTools.PrepareStrNUnicode(text3));
										if (num7 > 0)
										{
											flag2 = true;
											num5 = num7;
											num7 = wgAppConfig.getValBySql("  SELECT  t_b_Floor.f_floorNO,t_b_Controller.f_ControllerSN, t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,     t_b_Door.f_DoorName,    t_b_Controller.f_ZoneID, t_b_Floor.f_floorName  FROM (t_b_Floor LEFT JOIN t_b_Door ON t_b_Floor.f_DoorID = t_b_Door.f_DoorID) LEFT JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID WHERE  (  t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName + '.NC' ) =  " + wgTools.PrepareStrNUnicode(text3));
											num4 = 41;
											if (num7 > 0)
											{
												num4 = num7 + 40;
											}
										}
									}
								}
							}
							if (num5 <= 0)
							{
								goto IL_0706;
							}
							if (!flag2)
							{
								using (icController icController = new icController())
								{
									icController.GetInfoFromDBByControllerSN(num5);
									if (mjRec.CardID <= 0L && num2 > 0L)
									{
										num = icController.RemoteOpenDoorIP(num6, 0U, num2);
									}
									else
									{
										num = icController.RemoteOpenDoorIP(num6, 0U, mjRec.CardID);
									}
									if (num > 0)
									{
										batchAutoRun.wglogRecEventOfController(string.Format("OpenDoor OK. SN = {0} ({1})", icController.ControllerSN.ToString(), icController.GetDoorName(num6)));
									}
									else
									{
										batchAutoRun.wglogRecEventOfController(string.Format("OpenDoor Failed. SN = {0} ({1})", icController.ControllerSN.ToString(), icController.GetDoorName(num6)));
									}
									goto IL_0706;
								}
							}
							using (icController icController2 = new icController())
							{
								icController2.GetInfoFromDBByControllerSN(num5);
								if (mjRec.CardID <= 0L && num2 > 0L)
								{
									num = icController2.RemoteOpenDoorIP(num4, 0U, num2);
								}
								else
								{
									num = icController2.RemoteOpenFoorIP(num4, 0U, mjRec.CardID);
								}
								if (num > 0)
								{
									batchAutoRun.wglogRecEventOfController(string.Format("OpenDoor OK. SN = {0} ({1})", icController2.ControllerSN.ToString(), text3));
								}
								else
								{
									batchAutoRun.wglogRecEventOfController(string.Format("OpenDoor Failed. SN = {0} ({1})", icController2.ControllerSN.ToString(), text3));
								}
								goto IL_0706;
							}
						}
						if (array.Length == n + 3)
						{
							int num5 = int.Parse(array[n + 1]);
							int num6 = int.Parse(array[n + 2]);
							if (num5 > 0)
							{
								using (icController icController3 = new icController())
								{
									icController3.GetInfoFromDBByControllerSN(num5);
									if (wgMjController.IsElevator(num5))
									{
										if (num2 > 0L)
										{
											num = icController3.RemoteOpenDoorIP(num6, 0U, num2);
										}
										else
										{
											num = icController3.RemoteOpenFoorIP(num6, 0U, -1L);
										}
									}
									else if (num2 > 0L)
									{
										num = icController3.RemoteOpenDoorIP(num6, 0U, num2);
									}
									else
									{
										num = icController3.RemoteOpenDoorIP(num6);
									}
									if (num > 0)
									{
										batchAutoRun.wglogRecEventOfController(string.Format("OpenDoor OK. SN = {0} ({1})", icController3.ControllerSN.ToString(), icController3.GetDoorName(num6)));
									}
									else
									{
										batchAutoRun.wglogRecEventOfController(string.Format("OpenDoor Failed. SN = {0} ({1})", icController3.ControllerSN.ToString(), icController3.GetDoorName(num6)));
									}
								}
							}
						}
						IL_0706:
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num8 = 0; num8 < array.Length; num8++)
				{
					if (array[num8].ToUpper().Replace("–", "-") == "-UPLOADPRIVILEGE")
					{
						int num9 = 0;
						if (num2 > 0L)
						{
							if (array.Length == num8 + 1)
							{
								num = batchAutoRun.uploadPrivilege(num9, num2);
							}
							else if (array.Length == num8 + 2)
							{
								int valBySql = wgAppConfig.getValBySql(string.Format(" SELECT [f_ControllerSN]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(array[num8 + 1])));
								if (valBySql > 0)
								{
									num9 = valBySql;
								}
								else
								{
									int.TryParse(array[num8 + 1], out valBySql);
									if (valBySql <= 0)
									{
										return valBySql;
									}
									num9 = int.Parse(array[num8 + 1]);
								}
								num = batchAutoRun.uploadPrivilege(num9, num2);
							}
						}
						else if (array.Length == num8 + 1)
						{
							num = batchAutoRun.uploadPrivilege(num9);
						}
						else if (array.Length == num8 + 2)
						{
							int valBySql2 = wgAppConfig.getValBySql(string.Format(" SELECT [f_ControllerSN]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(array[num8 + 1])));
							if (valBySql2 > 0)
							{
								num9 = valBySql2;
							}
							else
							{
								int.TryParse(array[num8 + 1], out valBySql2);
								if (valBySql2 <= 0)
								{
									return valBySql2;
								}
								num9 = int.Parse(array[num8 + 1]);
							}
							num = batchAutoRun.uploadPrivilege(num9);
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num10 = 0; num10 < array.Length; num10++)
				{
					if (array[num10].ToUpper().Replace("–", "-") == "-DELETEALLPRIVILEGE")
					{
						int num11 = 0;
						if (num2 > 0L)
						{
							num = batchAutoRun.deleteAllPrivilege(num11, num2);
						}
						else
						{
							num = -1;
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num12 = 0; num12 < array.Length; num12++)
				{
					if (array[num12].ToUpper().Replace("–", "-") == "-GETRECORD")
					{
						int num13 = 0;
						if (array.Length == num12 + 1)
						{
							num = batchAutoRun.getSwipe(num13);
						}
						else if (array.Length == num12 + 2)
						{
							num = batchAutoRun.getSwipe(int.Parse(array[num12 + 1]));
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num14 = 0; num14 < array.Length; num14++)
				{
					if (array[num14].ToUpper().Replace("–", "-") == "-SETTIME".ToUpper())
					{
						int num15 = 0;
						if (array.Length == num14 + 1)
						{
							num = batchAutoRun.setTime(num15);
							if (!wgAppConfig.getParamValBoolByNO(186))
							{
								goto IL_0A71;
							}
							batchAutoRun.wglogRecEventOfController(string.Format("SetTime Starting... [AdjustTime Face Device]", new object[0]));
							try
							{
								using (dfrmFaceManagement4Hanvon dfrmFaceManagement4Hanvon = new dfrmFaceManagement4Hanvon())
								{
									dfrmFaceManagement4Hanvon.AdjustTimeAllDevice();
								}
								goto IL_0A71;
							}
							catch (Exception)
							{
								goto IL_0A71;
							}
						}
						if (array.Length == num14 + 2)
						{
							int.TryParse(array[num14 + 1], out num15);
							if (num15 <= 0)
							{
								int valBySql3 = wgAppConfig.getValBySql(string.Format(" SELECT [f_ControllerSN]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(array[num14 + 1])));
								if (valBySql3 > 0)
								{
									num15 = valBySql3;
								}
							}
							if (num15 > 0)
							{
								num = batchAutoRun.setTime(num15);
							}
							else
							{
								num = -1;
							}
						}
						IL_0A71:
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num16 = 0; num16 < array.Length; num16++)
				{
					if (array[num16].ToUpper().Replace("–", "-") == "-PCAsCardInput".ToUpper())
					{
						long num17 = 0L;
						if (array.Length == num16 + 4 || array.Length == num16 + 5)
						{
							int num18 = int.Parse(array[num16 + 1]);
							int num19 = int.Parse(array[num16 + 2]);
							if (long.TryParse(array[num16 + 3], out num17) && num18 > 0)
							{
								using (icController icController4 = new icController())
								{
									icController4.GetInfoFromDBByControllerSN(num18);
									if (array.Length == num16 + 5)
									{
										icController4.ShortPacketOptionPwdSet(array[num16 + 4]);
									}
									num = icController4.RemoteOpenDoorIP_V546(num19, num17);
									if (num > 0)
									{
										batchAutoRun.wglogRecEventOfController(string.Format("PCAsCardInput OK. SN = {0} ({1}) [{2}]", icController4.ControllerSN.ToString(), icController4.GetDoorName(num19), num17));
									}
									else
									{
										batchAutoRun.wglogRecEventOfController(string.Format("PCAsCardInput Failed. SN = {0} ({1})", icController4.ControllerSN.ToString(), icController4.GetDoorName(num19)));
									}
								}
							}
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num20 = 0; num20 < array.Length; num20++)
				{
					if (array[num20].ToUpper().Replace("–", "-") == "-CONFIGURE")
					{
						int num21 = 0;
						if (array.Length == num20 + 1)
						{
							num = batchAutoRun.uploadControlConfigure(num21);
						}
						else if (array.Length == num20 + 2)
						{
							num = batchAutoRun.uploadControlConfigure(int.Parse(array[num20 + 1]));
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num22 = 0; num22 < array.Length; num22++)
				{
					if (array[num22].ToUpper().Replace("–", "-") == "-GETPRIVILEGEOFRENTINGHOUSE" && wgAppConfig.getParamValBoolByNO(178))
					{
						int num23 = 0;
						if (array.Length == num22 + 1)
						{
							num = batchAutoRun.getPrivilegeOfRentingHouse(num23);
						}
						else if (array.Length == num22 + 2)
						{
							num = batchAutoRun.getPrivilegeOfRentingHouse(int.Parse(array[num22 + 1]));
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num24 = 0; num24 < array.Length; num24++)
				{
					if (array[num24].ToUpper().Replace("–", "-") == "-GETSN".ToUpper())
					{
						if (array.Length == num24 + 2)
						{
							string text5 = array[num24 + 1];
							MjRec mjRec2 = new MjRec(text5);
							int num25 = (int)mjRec2.ControllerSN;
							byte doorNo = mjRec2.DoorNo;
							if (num25 <= 0)
							{
								int valBySql4 = wgAppConfig.getValBySql(string.Format(" SELECT [f_ControllerSN]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(text5)));
								if (valBySql4 > 0)
								{
									num25 = valBySql4;
									wgAppConfig.getValBySql(string.Format(" SELECT [f_DoorNO]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(text5)));
								}
							}
							if (num25 > 0)
							{
								num = num25;
							}
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num26 = 0; num26 < array.Length; num26++)
				{
					if (array[num26].ToUpper().Replace("–", "-") == "-SETDOORNO".ToUpper() || array[num26].ToUpper().Replace("–", "-") == "-SETDOORNC".ToUpper() || array[num26].ToUpper().Replace("–", "-") == "-SETDOORONLINE".ToUpper())
					{
						if (array.Length == num26 + 2)
						{
							string text6 = array[num26 + 1];
							MjRec mjRec3 = new MjRec(text6);
							int num27 = (int)mjRec3.ControllerSN;
							int num28 = (int)mjRec3.DoorNo;
							if (num27 <= 0)
							{
								int num29 = wgAppConfig.getValBySql(string.Format(" SELECT [f_ControllerSN]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(text6)));
								if (num29 > 0)
								{
									num27 = num29;
									num29 = wgAppConfig.getValBySql(string.Format(" SELECT [f_DoorNO]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(text6)));
									num28 = 1;
									if (num29 > 0)
									{
										num28 = num29;
									}
								}
							}
							if (num27 > 0)
							{
								using (icController icController5 = new icController())
								{
									icController5.GetInfoFromDBByControllerSN(num27);
									int num30 = 3;
									if (array[num26].ToUpper().Replace("–", "-") == "-SETDOORNO".ToUpper())
									{
										num30 = 1;
									}
									if (array[num26].ToUpper().Replace("–", "-") == "-SETDOORNC".ToUpper())
									{
										num30 = 2;
									}
									num = icController5.DirectSetDoorControlIP(text6, num30);
									if (num > 0)
									{
										batchAutoRun.wglogRecEventOfController(string.Format("{0} OK. SN = {1} ({2})", array[num26].ToUpper().Replace("–", "-").Replace("-", ""), icController5.ControllerSN.ToString(), icController5.GetDoorName(num28)));
									}
									else
									{
										batchAutoRun.wglogRecEventOfController(string.Format("{0} Failed. SN = {1} ({2})", array[num26].ToUpper().Replace("–", "-").Replace("-", ""), icController5.ControllerSN.ToString(), icController5.GetDoorName(num28)));
									}
								}
							}
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num31 = 0; num31 < array.Length; num31++)
				{
					if (array[num31].ToUpper().Replace("–", "-") == "-GETDOORCONTROL".ToUpper())
					{
						if (array.Length == num31 + 2)
						{
							string text7 = array[num31 + 1];
							MjRec mjRec4 = new MjRec(text7);
							int num32 = (int)mjRec4.ControllerSN;
							int num33 = (int)mjRec4.DoorNo;
							if (num32 <= 0)
							{
								int num34 = wgAppConfig.getValBySql(string.Format(" SELECT [f_ControllerSN]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(text7)));
								if (num34 > 0)
								{
									num32 = num34;
									num34 = wgAppConfig.getValBySql(string.Format(" SELECT [f_DoorNO]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(text7)));
									num33 = 1;
									if (num34 > 0)
									{
										num33 = num34;
									}
								}
							}
							if (num32 > 0)
							{
								using (icController icController6 = new icController())
								{
									icController6.GetInfoFromDBByControllerSN(num32);
									wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
									num = icController6.GetConfigureIP(ref wgMjControllerConfigure);
									if (num > 0)
									{
										icDesc.doorControlDesc(wgMjControllerConfigure.DoorControlGet(num33));
										num = wgMjControllerConfigure.DoorControlGet(num33);
										batchAutoRun.wglogRecEventOfController(string.Format("{0} OK. SN = {1} ({2})", array[num31].ToUpper().Replace("–", "-").Replace("-", ""), icController6.ControllerSN.ToString(), icController6.GetDoorName(num33)));
									}
									else
									{
										batchAutoRun.wglogRecEventOfController(string.Format("{0} Failed. SN = {1} ({2})", array[num31].ToUpper().Replace("–", "-").Replace("-", ""), icController6.ControllerSN.ToString(), icController6.GetDoorName(num33)));
									}
								}
							}
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num35 = 0; num35 < array.Length; num35++)
				{
					if (array[num35].ToUpper().Replace("–", "-") == "-GETDOORSTATUS".ToUpper())
					{
						if (array.Length == num35 + 2)
						{
							string text8 = array[num35 + 1];
							MjRec mjRec5 = new MjRec(text8);
							int num36 = (int)mjRec5.ControllerSN;
							byte doorNo2 = mjRec5.DoorNo;
							if (num36 <= 0)
							{
								int valBySql5 = wgAppConfig.getValBySql(string.Format(" SELECT [f_ControllerSN]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(text8)));
								if (valBySql5 > 0)
								{
									num36 = valBySql5;
									wgAppConfig.getValBySql(string.Format(" SELECT [f_DoorNO]  FROM t_b_Controller,t_b_Door WHERE [f_DoorName] = {0} AND [t_b_Door].[f_ControllerID]=t_b_Controller.[f_ControllerID] ", wgTools.PrepareStrNUnicode(text8)));
								}
							}
							if (num36 > 0)
							{
								using (icController icController7 = new icController())
								{
									icController7.GetInfoFromDBByControllerSN(num36);
									num = icController7.GetControllerRunInformationIP(-1);
									if (num > 0)
									{
										num = (icController7.runinfo.IsOpen(icController7.GetDoorNO(text8)) ? 1 : 2);
									}
									else
									{
										num = 0;
									}
								}
							}
						}
						batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
						return num;
					}
				}
				for (int num37 = 0; num37 < array.Length; num37++)
				{
					if (array[num37].ToUpper().Replace("–", "-") == "-GETALLDOORSTATUS".ToUpper())
					{
						return 3;
					}
				}
				for (int num38 = 0; num38 < array.Length; num38++)
				{
					if (array[num38].ToUpper().Replace("–", "-") == "-SNAPSHOT".ToUpper())
					{
						try
						{
							Snapshot snapshot = new Snapshot();
							Application.Run(snapshot);
						}
						catch (Exception)
						{
						}
						return 1;
					}
				}
				for (int num39 = 0; num39 < array.Length; num39++)
				{
					if (array[num39].ToUpper().Replace("–", "-") == "-DATACENTER".ToUpper())
					{
						try
						{
							Application.Run(new dfrmSendSwipeToCenterA());
						}
						catch (Exception)
						{
						}
						return 1;
					}
				}
			}
			catch (Exception ex4)
			{
				wgAppConfig.wgLog(ex4.ToString());
			}
			batchAutoRun.wglogRecEventOfController(string.Format("iRet = {0}", num.ToString()));
			return num;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x000D9B28 File Offset: 0x000D8B28
		private static int deleteAllPrivilege(int parControllerSN, long remoteOpenCardNO)
		{
			int num = -1;
			try
			{
				int valBySql = wgAppConfig.getValBySql("SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + remoteOpenCardNO.ToString());
				if (valBySql <= 0)
				{
					if (wgAppConfig.getValBySql("SELECT f_ConsumerID  FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + remoteOpenCardNO.ToString()) <= 0)
					{
						return -103;
					}
				}
				else
				{
					new icConsumer().registerLostCard(valBySql, 0L);
				}
				icPrivilege icPrivilege = new icPrivilege();
				icController icController = new icController();
				string text = " SELECT f_ControllerID, f_DoorsNames, f_ControllerSN, f_IP, f_PORT  ";
				text += " FROM [t_b_Controller] WHERE [f_Enabled] = 1 ";
				if (parControllerSN != 0)
				{
					text = text + " AND f_ControllerSN = " + parControllerSN.ToString();
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
				dbConnection.Open();
				dbCommand.CommandText = text;
				int num2 = 0;
				ArrayList arrayList = new ArrayList();
				do
				{
					num2++;
					int num3 = 0;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						int num4 = (int)dbDataReader[0];
						wgTools.SetObjToStr(dbDataReader[1]);
						if (num4 > 0)
						{
							icController.GetInfoFromDBByControllerID(num4);
							if (arrayList.IndexOf(icController.ControllerSN.ToString()) < 0)
							{
								int num5 = (int)dbDataReader["f_ControllerSN"];
								string text2 = wgTools.SetObjToStr(dbDataReader["f_IP"]);
								int num6 = (int)dbDataReader["f_PORT"];
								num = icPrivilege.DelPrivilegeOfOneCardIP(num5, text2, num6, remoteOpenCardNO);
								if (num < 0)
								{
									batchAutoRun.wglogRecEventOfController(string.Format("DelPrivilegeOfOneCardIP Failed. SN = {0} (Err = {1})", icController.ControllerSN.ToString(), num.ToString()));
									wgTools.WgDebugWrite(icController.ControllerSN.ToString() + " DelPrivilegeOfOneCardIP Failed num =" + num.ToString(), new object[0]);
									num3++;
								}
								else
								{
									arrayList.Add(icController.ControllerSN.ToString());
									batchAutoRun.wglogRecEventOfController(string.Format("UploadPrivilege OK. SN = {0} (Privilege Count ={1})", icController.ControllerSN.ToString(), num.ToString()));
									string text3 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal =0,  f_lastConsoleUploadDateTime ={0}, f_lastConsoleUploadConsuemrsTotal ={1:d}, f_lastConsoleUploadPrivilege ={2:d}, f_lastConsoleUploadValidPrivilege ={3:d} WHERE f_ControllerID ={4:d}";
									wgAppConfig.runUpdateSql(string.Format(text3, new object[]
									{
										wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")),
										icPrivilege.ConsumersTotal,
										icPrivilege.PrivilegTotal,
										icPrivilege.ValidPrivilege,
										num4
									}));
								}
							}
						}
					}
					dbDataReader.Close();
					if (num3 <= 0 || num2 >= 3)
					{
						break;
					}
					batchAutoRun.wglogRecEventOfController(string.Format("UploadPrivilege Retrying... (Try Times ={0}, Failed Controller Count = {1})", num2, num3));
					Thread.Sleep(5000);
				}
				while (num2 < 3);
				dbConnection.Close();
				num = 1;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x000D9E60 File Offset: 0x000D8E60
		private static int getPrivilegeOfRentingHouse(int parControllerSN)
		{
			int num = -1;
			try
			{
				string text = "";
				icConsumer icConsumer = new icConsumer();
				long num2 = 0L;
				long num3 = 0L;
				icController icController = new icController();
				string text2 = "Pri" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
				text = wgAppConfig.Path4Doc() + text2;
				num2 = icConsumer.ConsumerNONext();
				if (num2 < long.Parse(DateTime.Now.ToString("yyyyMMdd000001")))
				{
					num2 = long.Parse(DateTime.Now.ToString("yyyyMMdd000001"));
				}
				num3 = 0L;
				wgMjControllerPrivilege wgMjControllerPrivilege = new wgMjControllerPrivilege();
				DataTable dataTable = null;
				string text3 = " SELECT f_ControllerID, f_DoorsNames ";
				text3 += " FROM [t_b_Controller] WHERE [f_Enabled] = 1 ";
				if (parControllerSN != 0)
				{
					text3 = text3 + " AND f_ControllerSN = " + parControllerSN.ToString();
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
				dbConnection.Open();
				dbCommand.CommandText = text3;
				int num4 = 0;
				int num5 = 0;
				ArrayList arrayList = new ArrayList();
				batchAutoRun.wglogRecEventOfController(string.Format("GetPrivilegeOfRentingHouse File = {0} ", "DOC\\" + text2));
				do
				{
					num4++;
					num5 = 0;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						int num6 = (int)dbDataReader[0];
						string text4 = wgTools.SetObjToStr(dbDataReader[1]).Replace(";", ",");
						if (num6 > 0)
						{
							icController.GetInfoFromDBByControllerID(num6);
							if (arrayList.IndexOf(icController.ControllerSN.ToString()) < 0)
							{
								int controllerSN = icController.ControllerSN;
								wgMjControllerPrivilege.AllowDownload();
								if (dataTable != null)
								{
									dataTable.Rows.Clear();
									dataTable.Dispose();
									dataTable = null;
									GC.Collect();
								}
								dataTable = new DataTable("Privilege");
								dataTable.Columns.Add("f_CardNO", Type.GetType("System.UInt32"));
								dataTable.Columns.Add("f_BeginYMD", Type.GetType("System.DateTime"));
								dataTable.Columns.Add("f_EndYMD", Type.GetType("System.DateTime"));
								dataTable.Columns.Add("f_PIN", Type.GetType("System.String"));
								dataTable.Columns.Add("f_ControlSegID1", Type.GetType("System.Byte"));
								dataTable.Columns["f_ControlSegID1"].DefaultValue = 0;
								dataTable.Columns.Add("f_ControlSegID2", Type.GetType("System.Byte"));
								dataTable.Columns["f_ControlSegID2"].DefaultValue = 0;
								dataTable.Columns.Add("f_ControlSegID3", Type.GetType("System.Byte"));
								dataTable.Columns["f_ControlSegID3"].DefaultValue = 0;
								dataTable.Columns.Add("f_ControlSegID4", Type.GetType("System.Byte"));
								dataTable.Columns["f_ControlSegID4"].DefaultValue = 0;
								dataTable.Columns.Add("f_AllowFloors", Type.GetType("System.UInt64"));
								dataTable.Columns["f_AllowFloors"].DefaultValue = 1099511627775L;
								dataTable.Columns.Add("f_ConsumerName", Type.GetType("System.String"));
								num = wgMjControllerPrivilege.DownloadIP(icController.ControllerSN, icController.IP, icController.PORT, "", ref dataTable);
								using (StreamWriter streamWriter = new StreamWriter(text, true))
								{
									if (num > 0)
									{
										int count = dataTable.Rows.Count;
										if (dataTable.Rows.Count >= 0)
										{
											if (dataTable.Rows.Count > 0)
											{
												for (int i = 0; i < dataTable.Rows.Count; i++)
												{
													if (!string.IsNullOrEmpty(wgTools.SetObjToStr(dataTable.Rows[i]["f_ConsumerName"])) && icConsumer.addNew(num2.ToString(), wgTools.SetObjToStr(dataTable.Rows[i]["f_ConsumerName"]), 0, 1, 0, 1, DateTime.Now, DateTime.Parse("2099-12-31"), 345678, long.Parse(dataTable.Rows[i]["f_CardNO"].ToString())) > 0)
													{
														num2 += 1L;
														num3 += 1L;
													}
													string text5 = string.Format("{0};{1};{2};{3};{4};{5}", new object[]
													{
														text4,
														controllerSN.ToString(),
														count,
														i + 1,
														dataTable.Rows[i]["f_CardNO"].ToString(),
														wgTools.SetObjToStr(dataTable.Rows[i]["f_ConsumerName"])
													});
													streamWriter.WriteLine(text5.ToString());
												}
											}
											else
											{
												string text5 = string.Format("{0};{1};{2};{3};{4};{5}", new object[]
												{
													text4,
													controllerSN.ToString(),
													"0",
													"",
													"",
													""
												});
												streamWriter.WriteLine(text5.ToString());
											}
										}
										arrayList.Add(icController.ControllerSN.ToString());
										batchAutoRun.wglogRecEventOfController(string.Format("GetPrivilegeOfRentingHouse OK. SN = {0} (Record Count ={1})", icController.ControllerSN.ToString(), count));
									}
									else
									{
										streamWriter.WriteLine(string.Format("{0};{1};{2};{3};{4};{5}", new object[]
										{
											text4,
											controllerSN.ToString(),
											"-1",
											"",
											"",
											""
										}).ToString());
										batchAutoRun.wglogRecEventOfController(string.Format("GetPrivilegeOfRentingHouse Failed. SN = {0} (Err = {1})", icController.ControllerSN.ToString(), num.ToString()));
										wgTools.WgDebugWrite(icController.ControllerSN.ToString() + "GetPrivilegeOfRentingHouse  Failed num =" + num.ToString(), new object[0]);
										num5++;
									}
								}
							}
						}
					}
					dbDataReader.Close();
					if (num5 <= 0 || num4 >= 3)
					{
						break;
					}
					batchAutoRun.wglogRecEventOfController(string.Format("GetPrivilegeOfRentingHouse Retrying... (Try Times ={0}, Failed Controller Count = {1})", num4, num5));
					Thread.Sleep(5000);
				}
				while (num4 < 3);
				dbConnection.Close();
				batchAutoRun.wglogRecEventOfController(string.Format("GetPrivilegeOfRentingHouse Add New Users = {0} ", num3.ToString()));
				num = 1;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x000DA5DC File Offset: 0x000D95DC
		private static int getSwipe(int parControllerSN)
		{
			int num = -1;
			try
			{
				icController icController = new icController();
				icSwipeRecord icSwipeRecord = new icSwipeRecord();
				icSwipeRecord.Clear();
				string text = " SELECT f_ControllerID, f_DoorsNames ";
				text += " FROM [t_b_Controller] WHERE [f_Enabled] = 1 ";
				if (parControllerSN != 0)
				{
					text = text + " AND f_ControllerSN = " + parControllerSN.ToString();
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
				dbConnection.Open();
				dbCommand.CommandText = text;
				int num2 = 0;
				ArrayList arrayList = new ArrayList();
				do
				{
					num2++;
					int num3 = 0;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						int num4 = (int)dbDataReader[0];
						string text2 = wgTools.SetObjToStr(dbDataReader[1]);
						if (num4 > 0)
						{
							icController.GetInfoFromDBByControllerID(num4);
							if (arrayList.IndexOf(icController.ControllerSN.ToString()) < 0)
							{
								icSwipeRecord.Clear();
								batchAutoRun.wglogRecEventOfController(string.Format("GetRecord Starting... SN = {0} ({1})", icController.ControllerSN.ToString(), text2));
								num = icSwipeRecord.GetSwipeRecords(icController.ControllerSN, icController.IP, icController.PORT, "Auto Get");
								if (num < 0)
								{
									batchAutoRun.wglogRecEventOfController(string.Format("GetRecord Failed. SN = {0} (Err = {1})", icController.ControllerSN.ToString(), num.ToString()));
									wgTools.WgDebugWrite(icController.ControllerSN.ToString() + "GetRecord  Failed num =" + num.ToString(), new object[0]);
									num3++;
								}
								else
								{
									arrayList.Add(icController.ControllerSN.ToString());
									batchAutoRun.wglogRecEventOfController(string.Format("GetRecord OK. SN = {0} (Record Count ={1})", icController.ControllerSN.ToString(), num.ToString()));
								}
							}
						}
					}
					dbDataReader.Close();
					if (num3 <= 0 || num2 >= 3)
					{
						break;
					}
					batchAutoRun.wglogRecEventOfController(string.Format("GetRecord Retrying... (Try Times ={0}, Failed Controller Count = {1})", num2, num3));
					Thread.Sleep(5000);
				}
				while (num2 < 3);
				dbConnection.Close();
				num = 1;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x000DA854 File Offset: 0x000D9854
		private static int setTime(int parControllerSN)
		{
			int num = -1;
			try
			{
				icController icController = new icController();
				string text = " SELECT f_ControllerID, f_DoorsNames ";
				text += " FROM [t_b_Controller] WHERE [f_Enabled] = 1 ";
				if (parControllerSN != 0)
				{
					text = text + " AND f_ControllerSN = " + parControllerSN.ToString();
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
				dbConnection.Open();
				dbCommand.CommandText = text;
				int num2 = 0;
				ArrayList arrayList = new ArrayList();
				do
				{
					num2++;
					int num3 = 0;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						int num4 = (int)dbDataReader[0];
						string text2 = wgTools.SetObjToStr(dbDataReader[1]);
						if (num4 > 0)
						{
							icController.GetInfoFromDBByControllerID(num4);
							if (arrayList.IndexOf(icController.ControllerSN.ToString()) < 0)
							{
								batchAutoRun.wglogRecEventOfController(string.Format("SetTime Starting... SN = {0} ({1})", icController.ControllerSN.ToString(), text2));
								num = icController.AdjustTimeIP(DateTime.Now);
								if (num < 0)
								{
									batchAutoRun.wglogRecEventOfController(string.Format("SetTime Failed. SN = {0} (Err = {1})", icController.ControllerSN.ToString(), num.ToString()));
									wgTools.WgDebugWrite(icController.ControllerSN.ToString() + " Failed num =" + num.ToString(), new object[0]);
									num3++;
								}
								else
								{
									arrayList.Add(icController.ControllerSN.ToString());
									batchAutoRun.wglogRecEventOfController(string.Format("SetTime OK. SN = {0}", icController.ControllerSN.ToString()));
								}
							}
						}
					}
					dbDataReader.Close();
					if (num3 <= 0 || num2 >= 3)
					{
						break;
					}
					batchAutoRun.wglogRecEventOfController(string.Format("SetTime Retrying... (Try Times ={0}, Failed Controller Count = {1})", num2, num3));
					if (parControllerSN == 0)
					{
						Thread.Sleep(5000);
					}
					else
					{
						Thread.Sleep(500);
					}
				}
				while (num2 < 3);
				dbConnection.Close();
				if (parControllerSN == 0)
				{
					num = 1;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x000DAAAC File Offset: 0x000D9AAC
		private static int uploadControlConfigure(int parControllerSN)
		{
			int num = -1;
			try
			{
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				wgMjControllerTaskList wgMjControllerTaskList = new wgMjControllerTaskList();
				wgMjControllerHolidaysList wgMjControllerHolidaysList = new wgMjControllerHolidaysList();
				new icPrivilege();
				icController icController = new icController();
				string text = " SELECT f_ControllerID, f_DoorsNames ";
				text += " FROM [t_b_Controller] WHERE [f_Enabled] = 1 ";
				if (parControllerSN != 0)
				{
					text = text + " AND f_ControllerSN = " + parControllerSN.ToString();
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
				dbConnection.Open();
				dbCommand.CommandText = text;
				int num2 = 0;
				ArrayList arrayList = new ArrayList();
				do
				{
					num2++;
					int num3 = 0;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						int num4 = (int)dbDataReader[0];
						string text2 = wgTools.SetObjToStr(dbDataReader[1]);
						if (num4 > 0)
						{
							wgMjControllerConfigure.Clear();
							icController.GetInfoFromDBByControllerID(num4);
							if (arrayList.IndexOf(icController.ControllerSN.ToString()) < 0)
							{
								icControllerConfigureFromDB.getControllerConfigureFromDBByControllerID(icController.ControllerID, ref wgMjControllerConfigure, ref wgMjControllerTaskList, ref wgMjControllerHolidaysList);
								num = icController.UpdateConfigureIP(wgMjControllerConfigure, -1);
								if (num < 0)
								{
									batchAutoRun.wglogRecEventOfController(string.Format("updateConfigureIP Failed. SN = {0} (Err = {1})  ({2})  ", icController.ControllerSN.ToString(), num.ToString(), text2));
									wgTools.WgDebugWrite(icController.ControllerSN.ToString() + " updateConfigureIP Failed num =" + num.ToString(), new object[0]);
									num3++;
								}
								else if (wgMjControllerConfigure.controlTaskList_enabled > 0 && (num = icController.UpdateControlTaskListIP(wgMjControllerTaskList, -1)) <= 0)
								{
									batchAutoRun.wglogRecEventOfController(string.Format("UploadPrivilege Failed. SN = {0} (Err = {1})", icController.ControllerSN.ToString(), num.ToString()));
									wgTools.WgDebugWrite(icController.ControllerSN.ToString() + " updateControlTaskListIP Failed num =" + num.ToString(), new object[0]);
									num = -13;
									num3++;
								}
								else
								{
									if (wgAppConfig.getParamValBoolByNO(121))
									{
										icControllerTimeSegList icControllerTimeSegList = new icControllerTimeSegList();
										if (wgAppConfig.getParamValBoolByNO(121))
										{
											icControllerTimeSegList.fillByDB();
										}
										num = icController.UpdateControlTimeSegListIP(icControllerTimeSegList, -1);
										if (num <= 0)
										{
											batchAutoRun.wglogRecEventOfController(string.Format("UpdateControlTimeSegListIP Failed. SN = {0} (Err = {1})", icController.ControllerSN.ToString(), num.ToString()));
											wgTools.WgDebugWrite(icController.ControllerSN.ToString() + " updateControlTimeSegListIP Failed num =" + num.ToString(), new object[0]);
											num = -13;
											num3++;
											continue;
										}
										num = icController.UpdateHolidayListIP(wgMjControllerHolidaysList.ToByte(), -1);
										if (num <= 0)
										{
											batchAutoRun.wglogRecEventOfController(string.Format("UpdateHolidayListIP Failed. SN = {0} (Err = {1})", icController.ControllerSN.ToString(), num.ToString()));
											wgTools.WgDebugWrite(icController.ControllerSN.ToString() + " UpdateHolidayListIP Failed num =" + num.ToString(), new object[0]);
											num = -13;
											num3++;
											continue;
										}
									}
									if (wgMjControllerTaskList.taskCount > 0)
									{
										num = icController.RenewControlTaskListIP(-1);
										if (num < 0)
										{
											batchAutoRun.wglogRecEventOfController(string.Format("RenewControlTaskListIP Failed. SN = {0} (Err = {1})", icController.ControllerSN.ToString(), num.ToString()));
											wgTools.WgDebugWrite(icController.ControllerSN.ToString() + " control4uploadPrivilege.renewControlTaskListIP Failed num =" + num.ToString(), new object[0]);
											num3++;
											continue;
										}
									}
									arrayList.Add(icController.ControllerSN.ToString());
									batchAutoRun.wglogRecEventOfController(string.Format("UpdateConfigureIP OK. SN = {0}", icController.ControllerSN.ToString()));
								}
							}
						}
					}
					dbDataReader.Close();
					if (num3 <= 0 || num2 >= 3)
					{
						break;
					}
					batchAutoRun.wglogRecEventOfController(string.Format("UpdateConfigureIP Retrying... (Try Times ={0}, Failed Controller Count = {1})", num2, num3));
					Thread.Sleep(5000);
				}
				while (num2 < 3);
				dbConnection.Close();
				num = 1;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x000DAF00 File Offset: 0x000D9F00
		private static int uploadPrivilege(int parControllerSN)
		{
			int num = -1;
			try
			{
				icPrivilege icPrivilege = new icPrivilege();
				icController icController = new icController();
				string text = " SELECT f_ControllerID, f_DoorsNames ";
				text += " FROM [t_b_Controller] WHERE [f_Enabled] = 1 ";
				if (parControllerSN != 0)
				{
					text = text + " AND f_ControllerSN = " + parControllerSN.ToString();
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
				dbConnection.Open();
				dbCommand.CommandText = text;
				int num2 = 0;
				ArrayList arrayList = new ArrayList();
				do
				{
					num2++;
					int num3 = 0;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						int num4 = (int)dbDataReader[0];
						string text2 = wgTools.SetObjToStr(dbDataReader[1]);
						if (num4 > 0)
						{
							icController.GetInfoFromDBByControllerID(num4);
							if (arrayList.IndexOf(icController.ControllerSN.ToString()) < 0)
							{
								icPrivilege.AllowUpload();
								num = icPrivilege.getPrivilegeByID(num4);
								if (num < 0)
								{
									batchAutoRun.wglogRecEventOfController(string.Format("UploadPrivilege Failed. SN = {0} (Err = {1})  ({2})  ", icController.ControllerSN.ToString(), num.ToString(), text2));
									wgTools.WgDebugWrite(icController.ControllerSN.ToString() + " UploadPrivilege.GetPrivilegeByID Failed num =" + num.ToString(), new object[0]);
									num3++;
								}
								else
								{
									batchAutoRun.wglogRecEventOfController(string.Format("UploadPrivilege Starting... SN = {0} ({1})", icController.ControllerSN.ToString(), text2));
									num = icPrivilege.upload(icController.ControllerSN, icController.IP, icController.PORT, "Auto Upload", -1);
									if (num < 0)
									{
										batchAutoRun.wglogRecEventOfController(string.Format("UploadPrivilege Failed. SN = {0} (Err = {1})", icController.ControllerSN.ToString(), num.ToString()));
										wgTools.WgDebugWrite(icController.ControllerSN.ToString() + " Upload Failed num =" + num.ToString(), new object[0]);
										num3++;
									}
									else
									{
										arrayList.Add(icController.ControllerSN.ToString());
										batchAutoRun.wglogRecEventOfController(string.Format("UploadPrivilege OK. SN = {0} (Privilege Count ={1})", icController.ControllerSN.ToString(), num.ToString()));
										string text3 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal =0,  f_lastConsoleUploadDateTime ={0}, f_lastConsoleUploadConsuemrsTotal ={1:d}, f_lastConsoleUploadPrivilege ={2:d}, f_lastConsoleUploadValidPrivilege ={3:d} WHERE f_ControllerID ={4:d}";
										wgAppConfig.runUpdateSql(string.Format(text3, new object[]
										{
											wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")),
											icPrivilege.ConsumersTotal,
											icPrivilege.PrivilegTotal,
											icPrivilege.ValidPrivilege,
											num4
										}));
									}
								}
							}
						}
					}
					dbDataReader.Close();
					if (num3 <= 0 || num2 >= 3)
					{
						break;
					}
					batchAutoRun.wglogRecEventOfController(string.Format("UploadPrivilege Retrying... (Try Times ={0}, Failed Controller Count = {1})", num2, num3));
					Thread.Sleep(5000);
				}
				while (num2 < 3);
				dbConnection.Close();
				num = 1;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x000DB258 File Offset: 0x000DA258
		private static int uploadPrivilege(int parControllerSN, long remoteOpenCardNO)
		{
			int num = -1;
			try
			{
				int valBySql = wgAppConfig.getValBySql("SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + remoteOpenCardNO.ToString());
				if (valBySql <= 0)
				{
					return -103;
				}
				icPrivilege icPrivilege = new icPrivilege();
				icController icController = new icController();
				string text = " SELECT f_ControllerID, f_DoorsNames ";
				text += " FROM [t_b_Controller] WHERE [f_Enabled] = 1 ";
				if (parControllerSN != 0)
				{
					text = text + " AND f_ControllerSN = " + parControllerSN.ToString();
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
				dbConnection.Open();
				dbCommand.CommandText = text;
				int num2 = 0;
				ArrayList arrayList = new ArrayList();
				do
				{
					num2++;
					int num3 = 0;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						int num4 = (int)dbDataReader[0];
						wgTools.SetObjToStr(dbDataReader[1]);
						if (num4 > 0)
						{
							icController.GetInfoFromDBByControllerID(num4);
							if (arrayList.IndexOf(icController.ControllerSN.ToString()) < 0)
							{
								num = icPrivilege.DelPrivilegeOfOneCardByDB(num4, valBySql);
								if (num < 0)
								{
									batchAutoRun.wglogRecEventOfController(string.Format("DelPrivilegeOfOneCardByDB Failed. SN = {0} (Err = {1})", icController.ControllerSN.ToString(), num.ToString()));
									wgTools.WgDebugWrite(icController.ControllerSN.ToString() + " DelPrivilegeOfOneCardByDB Failed num =" + num.ToString(), new object[0]);
									num3++;
								}
								else
								{
									arrayList.Add(icController.ControllerSN.ToString());
									batchAutoRun.wglogRecEventOfController(string.Format("UploadPrivilege OK. SN = {0} (Privilege Count ={1})", icController.ControllerSN.ToString(), num.ToString()));
									string text2 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal =0,  f_lastConsoleUploadDateTime ={0}, f_lastConsoleUploadConsuemrsTotal ={1:d}, f_lastConsoleUploadPrivilege ={2:d}, f_lastConsoleUploadValidPrivilege ={3:d} WHERE f_ControllerID ={4:d}";
									wgAppConfig.runUpdateSql(string.Format(text2, new object[]
									{
										wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")),
										icPrivilege.ConsumersTotal,
										icPrivilege.PrivilegTotal,
										icPrivilege.ValidPrivilege,
										num4
									}));
								}
							}
						}
					}
					dbDataReader.Close();
					if (num3 <= 0 || num2 >= 3)
					{
						break;
					}
					batchAutoRun.wglogRecEventOfController(string.Format("UploadPrivilege Retrying... (Try Times ={0}, Failed Controller Count = {1})", num2, num3));
					Thread.Sleep(5000);
				}
				while (num2 < 3);
				dbConnection.Close();
				num = 1;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x000DB52C File Offset: 0x000DA52C
		public static void wglogRecEventOfController(string strMsg)
		{
			string text = "";
			try
			{
				text = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff") + "\t" + strMsg, new object[0]);
				using (StreamWriter streamWriter = new StreamWriter(wgAppConfig.Path4Doc() + "n3k_autorun.log", true))
				{
					streamWriter.WriteLine(text);
				}
			}
			catch (Exception)
			{
			}
			wgAppConfig.wgLogWithoutDB(text);
		}
	}
}
