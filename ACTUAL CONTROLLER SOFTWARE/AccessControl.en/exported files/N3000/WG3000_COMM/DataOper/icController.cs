using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using WG3000_COMM.Core;

namespace WG3000_COMM.DataOper
{
	// Token: 0x0200021F RID: 543
	public class icController : wgMjController
	{
		// Token: 0x06000F7C RID: 3964 RVA: 0x00113F74 File Offset: 0x00112F74
		public int AddIntoDB()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.AddIntoDB_Acc();
			}
			int num = -9;
			try
			{
				string text = "BEGIN TRANSACTION";
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (this.cm = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						this.cm.ExecuteNonQuery();
						try
						{
							string text2 = "";
							for (int i = 0; i < wgMjController.GetControllerType(base.ControllerSN); i++)
							{
								text2 = text2 + this.m_doorName[i] + ";   ";
							}
							text = " INSERT INTO t_b_Controller (f_ControllerNO, f_ControllerSN, f_Enabled, f_IP, f_PORT, f_Note, f_DoorsNames,f_ZoneID) values (";
							text = string.Concat(new string[]
							{
								text,
								this.m_ControllerNO.ToString(),
								" , ",
								base.ControllerSN.ToString(),
								" , ",
								this.m_Active ? "1" : "0",
								" , ",
								wgTools.PrepareStrNUnicode(base.IP),
								" , ",
								base.PORT.ToString(),
								" , ",
								wgTools.PrepareStrNUnicode(this.m_Note),
								" , ",
								wgTools.PrepareStrNUnicode(text2),
								" , ",
								this.m_ZoneID.ToString(),
								")"
							});
							this.cm.CommandText = text;
							this.cm.ExecuteNonQuery();
							text = "SELECT f_ControllerID from [t_b_Controller] where f_ControllerNo =" + this.m_ControllerNO.ToString();
							this.cm.CommandText = text;
							this.m_ControllerID = int.Parse("0" + wgTools.SetObjToStr(this.cm.ExecuteScalar()));
							text = " DELETE FROM [t_b_Door] ";
							text = text + " WHERE [f_ControllerID] = " + this.m_ControllerID.ToString();
							this.cm.CommandText = text;
							this.cm.ExecuteNonQuery();
							for (int i = 0; i < wgMjController.GetControllerType(base.ControllerSN); i++)
							{
								text = " DELETE FROM [t_b_Door] ";
								text = text + " WHERE [f_DoorName] = " + wgTools.PrepareStrNUnicode(this.m_doorName[i]) + " AND [t_b_Door].f_ControllerID NOT IN (SELECT t_b_Controller.f_ControllerID FROM t_b_Controller) ";
								this.cm.CommandText = text;
								this.cm.ExecuteNonQuery();
								text = " INSERT INTO [t_b_Door] ";
								text = string.Concat(new string[]
								{
									text,
									"([f_ControllerID], [f_DoorNO], [f_DoorName], [f_DoorControl], [f_DoorDelay], [f_DoorEnabled]) Values(",
									this.m_ControllerID.ToString(),
									" , ",
									(i + 1).ToString(),
									" , ",
									wgTools.PrepareStrNUnicode(this.m_doorName[i]),
									" , ",
									this.m_doorControl[i].ToString(),
									" , ",
									this.m_doorDelay[i].ToString(),
									" , ",
									this.m_doorActive[i] ? "1" : "0",
									")"
								});
								this.cm.CommandText = text;
								this.cm.ExecuteNonQuery();
							}
							text = " DELETE FROM [t_b_Reader] ";
							text = text + " WHERE [f_ControllerID] = " + this.m_ControllerID.ToString();
							this.cm.CommandText = text;
							this.cm.ExecuteNonQuery();
							for (int i = 0; i < wgMjController.GetControllerReaderNum(base.ControllerSN); i++)
							{
								text = " INSERT INTO [t_b_Reader] ";
								text = string.Concat(new string[]
								{
									text,
									"([f_ControllerID], [f_ReaderNo], [f_ReaderName], [f_PasswordEnabled], [f_Attend],[f_DutyOnOff]) Values(",
									this.m_ControllerID.ToString(),
									" , ",
									(i + 1).ToString(),
									" , ",
									wgTools.PrepareStrNUnicode(this.m_readerName[i]),
									" , 0 , ",
									this.m_readerAsAttendActive[i] ? "1" : "0",
									" , ",
									this.m_readerAsAttendControl[i].ToString(),
									")"
								});
								this.cm.CommandText = text;
								this.cm.ExecuteNonQuery();
								if (wgMjController.IsElevator(base.ControllerSN))
								{
									break;
								}
							}
							text = "COMMIT TRANSACTION";
							this.cm.CommandText = text;
							this.cm.ExecuteNonQuery();
							num = 1;
						}
						catch (Exception)
						{
							text = "ROLLBACK TRANSACTION";
							this.cm.CommandText = text;
							this.cm.ExecuteNonQuery();
						}
						return num;
					}
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x001144B8 File Offset: 0x001134B8
		public int AddIntoDB_Acc()
		{
			int num = -9;
			try
			{
				string text = "BEGIN TRANSACTION";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (this.cm_Acc = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						this.cm_Acc.ExecuteNonQuery();
						try
						{
							string text2 = "";
							for (int i = 0; i < wgMjController.GetControllerType(base.ControllerSN); i++)
							{
								text2 = text2 + this.m_doorName[i] + ";   ";
							}
							text = " INSERT INTO t_b_Controller (f_ControllerNO, f_ControllerSN, f_Enabled, f_IP, f_PORT, f_Note, f_DoorsNames,f_ZoneID) values (";
							text = string.Concat(new string[]
							{
								text,
								this.m_ControllerNO.ToString(),
								" , ",
								base.ControllerSN.ToString(),
								" , ",
								this.m_Active ? "1" : "0",
								" , ",
								wgTools.PrepareStrNUnicode(base.IP),
								" , ",
								base.PORT.ToString(),
								" , ",
								wgTools.PrepareStrNUnicode(this.m_Note),
								" , ",
								wgTools.PrepareStrNUnicode(text2),
								" , ",
								this.m_ZoneID.ToString(),
								")"
							});
							this.cm_Acc.CommandText = text;
							this.cm_Acc.ExecuteNonQuery();
							text = "SELECT f_ControllerID from [t_b_Controller] where f_ControllerNo =" + this.m_ControllerNO.ToString();
							this.cm_Acc.CommandText = text;
							this.m_ControllerID = int.Parse("0" + wgTools.SetObjToStr(this.cm_Acc.ExecuteScalar()));
							text = " DELETE FROM [t_b_Door] ";
							text = text + " WHERE [f_ControllerID] = " + this.m_ControllerID.ToString();
							this.cm_Acc.CommandText = text;
							this.cm_Acc.ExecuteNonQuery();
							for (int i = 0; i < wgMjController.GetControllerType(base.ControllerSN); i++)
							{
								text = " DELETE FROM [t_b_Door] ";
								text = text + " WHERE [f_DoorName] = " + wgTools.PrepareStrNUnicode(this.m_doorName[i]) + " AND [t_b_Door].f_ControllerID NOT IN (SELECT t_b_Controller.f_ControllerID FROM t_b_Controller) ";
								this.cm_Acc.CommandText = text;
								this.cm_Acc.ExecuteNonQuery();
								text = " INSERT INTO [t_b_Door] ";
								text = string.Concat(new string[]
								{
									text,
									"([f_ControllerID], [f_DoorNO], [f_DoorName], [f_DoorControl], [f_DoorDelay], [f_DoorEnabled]) Values(",
									this.m_ControllerID.ToString(),
									" , ",
									(i + 1).ToString(),
									" , ",
									wgTools.PrepareStrNUnicode(this.m_doorName[i]),
									" , ",
									this.m_doorControl[i].ToString(),
									" , ",
									this.m_doorDelay[i].ToString(),
									" , ",
									this.m_doorActive[i] ? "1" : "0",
									")"
								});
								this.cm_Acc.CommandText = text;
								this.cm_Acc.ExecuteNonQuery();
							}
							text = " DELETE FROM [t_b_Reader] ";
							text = text + " WHERE [f_ControllerID] = " + this.m_ControllerID.ToString();
							this.cm_Acc.CommandText = text;
							this.cm_Acc.ExecuteNonQuery();
							for (int i = 0; i < wgMjController.GetControllerReaderNum(base.ControllerSN); i++)
							{
								text = " INSERT INTO [t_b_Reader] ";
								text = string.Concat(new string[]
								{
									text,
									"([f_ControllerID], [f_ReaderNo], [f_ReaderName], [f_PasswordEnabled], [f_Attend],[f_DutyOnOff]) Values(",
									this.m_ControllerID.ToString(),
									" , ",
									(i + 1).ToString(),
									" , ",
									wgTools.PrepareStrNUnicode(this.m_readerName[i]),
									" , 0 , ",
									this.m_readerAsAttendActive[i] ? "1" : "0",
									" , ",
									this.m_readerAsAttendControl[i].ToString(),
									")"
								});
								this.cm_Acc.CommandText = text;
								this.cm_Acc.ExecuteNonQuery();
								if (wgMjController.IsElevator(base.ControllerSN))
								{
									break;
								}
							}
							text = "COMMIT TRANSACTION";
							this.cm_Acc.CommandText = text;
							this.cm_Acc.ExecuteNonQuery();
							num = 1;
						}
						catch (Exception)
						{
							text = "ROLLBACK TRANSACTION";
							this.cm_Acc.CommandText = text;
							this.cm_Acc.ExecuteNonQuery();
						}
						return num;
					}
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x001149F0 File Offset: 0x001139F0
		public static int DeleteControllerFromDB(int ControllerID)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return icController.DeleteControllerFromDB_Acc(ControllerID);
			}
			int num = ControllerID;
			if (num > 0)
			{
				string text = "BEGIN TRANSACTION";
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.ExecuteNonQuery();
						try
						{
							text = " DELETE FROM t_b_ElevatorGroup ";
							text = text + " WHERE  t_b_ElevatorGroup.f_ControllerID =  " + num.ToString();
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							text = " DELETE  FROM t_b_UserFloor ";
							text = text + " WHERE f_floorID IN   (SELECT f_floorID FROM t_b_Floor WHERE t_b_Floor.f_ControllerID =  " + num.ToString() + ")";
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							text = " DELETE FROM t_b_Floor ";
							text = text + " WHERE t_b_Floor.f_ControllerID =  " + num.ToString();
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							text = "DELETE FROM t_b_Controller WHERE f_ControllerID =  " + num.ToString();
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							text = "DELETE  FROM t_b_Door WHERE f_ControllerID =  " + num.ToString();
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							text = "DELETE  FROM t_d_Privilege_Of_PrivilegeType WHERE f_ControllerID =  " + num.ToString();
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							text = "DELETE  FROM t_d_Privilege WHERE f_ControllerID =  " + num.ToString();
							sqlCommand.CommandText = text;
							sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							sqlCommand.ExecuteNonQuery();
							text = "COMMIT TRANSACTION";
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
							text = "ROLLBACK TRANSACTION";
							if (sqlCommand.Connection.State != ConnectionState.Open)
							{
								sqlCommand.Connection.Open();
							}
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							throw;
						}
					}
				}
			}
			return 1;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00114BF8 File Offset: 0x00113BF8
		public static int DeleteControllerFromDB_Acc(int ControllerID)
		{
			int num = ControllerID;
			if (num > 0)
			{
				string text = "BEGIN TRANSACTION";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.ExecuteNonQuery();
						try
						{
							text = " DELETE FROM t_b_ElevatorGroup ";
							text = text + " WHERE  t_b_ElevatorGroup.f_ControllerID =  " + num.ToString();
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							text = " DELETE  FROM t_b_UserFloor ";
							text = text + " WHERE f_floorID IN   (SELECT f_floorID FROM t_b_Floor WHERE t_b_Floor.f_ControllerID =  " + num.ToString() + ")";
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							text = " DELETE FROM t_b_Floor ";
							text = text + " WHERE t_b_Floor.f_ControllerID =  " + num.ToString();
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							text = "DELETE FROM t_b_Controller WHERE f_ControllerID =  " + num.ToString();
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							text = "DELETE  FROM t_b_Door WHERE f_ControllerID =  " + num.ToString();
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							text = "DELETE  FROM t_d_Privilege_Of_PrivilegeType WHERE f_ControllerID =  " + num.ToString();
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							text = "DELETE  FROM t_d_Privilege WHERE f_ControllerID =  " + num.ToString();
							oleDbCommand.CommandText = text;
							oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							oleDbCommand.ExecuteNonQuery();
							text = "COMMIT TRANSACTION";
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
							text = "ROLLBACK TRANSACTION";
							if (oleDbCommand.Connection.State != ConnectionState.Open)
							{
								oleDbCommand.Connection.Open();
							}
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							throw;
						}
					}
				}
			}
			return 1;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00114DF4 File Offset: 0x00113DF4
		public int DirectSetDoorControlIP(string DoorName, int doorControl)
		{
			int num = 0;
			while (num < 4 && !(this.m_doorName[num] == DoorName))
			{
				num++;
			}
			if (num >= 4)
			{
				return -1;
			}
			wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
			wgMjControllerConfigure.DoorControlSet(num + 1, doorControl);
			return base.UpdateConfigureIP(wgMjControllerConfigure, -1);
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00114E3C File Offset: 0x00113E3C
		public int DirectSetDoorOpenDelayIP(string DoorName, int doorOpenDelay)
		{
			int num = 0;
			while (num < 4 && !(this.m_doorName[num] == DoorName))
			{
				num++;
			}
			if (num >= 4)
			{
				return -1;
			}
			wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
			wgMjControllerConfigure.DoorDelaySet(num + 1, doorOpenDelay);
			return base.UpdateConfigureIP(wgMjControllerConfigure, -1);
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x00114E83 File Offset: 0x00113E83
		public int GetControllerRunInformationIP(int UDPQueue4MultithreadIndex = -1)
		{
			return this.GetControllerRunInformationIP("", UDPQueue4MultithreadIndex);
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00114E94 File Offset: 0x00113E94
		public int GetControllerRunInformationIP(string PCIPAddr, int UDPQueue4MultithreadIndex = -1)
		{
			byte[] array = null;
			if (wgTools.bUDPOnly64 > 0)
			{
				return base.GetControllerRunInformationIP64(ref array, PCIPAddr, ref this.m_runinfo, UDPQueue4MultithreadIndex);
			}
			if (base.GetMjControllerRunInformationIP(ref array, PCIPAddr, UDPQueue4MultithreadIndex) != 1 || array == null)
			{
				return -1;
			}
			if (base.ControllerSN != -1)
			{
				this.m_runinfo.update(array, 20, (uint)base.ControllerSN);
			}
			else
			{
				uint num = (uint)((int)array[8] + ((int)array[9] << 8) + ((int)array[10] << 16) + ((int)array[11] << 24));
				this.m_runinfo.update(array, 20, num);
			}
			int num2 = 0;
			while (num2 < 10 && this.m_runinfo.newSwipes[num2].IndexInDataFlash != 4294967295U)
			{
				num2++;
			}
			return 1;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00114F3C File Offset: 0x00113F3C
		public int GetControllerRunInformationIP(string PCIPAddr, ref byte[] bytData, int UDPQueue4MultithreadIndex = -1)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return base.GetControllerRunInformationIP64(ref bytData, PCIPAddr, ref this.m_runinfo, UDPQueue4MultithreadIndex);
			}
			if (base.GetMjControllerRunInformationIP(ref bytData, PCIPAddr, UDPQueue4MultithreadIndex) != 1 || bytData == null)
			{
				return -1;
			}
			if (base.ControllerSN != -1)
			{
				this.m_runinfo.update(bytData, 20, (uint)base.ControllerSN);
			}
			else
			{
				uint num = (uint)((int)bytData[8] + ((int)bytData[9] << 8) + ((int)bytData[10] << 16) + ((int)bytData[11] << 24));
				this.m_runinfo.update(bytData, 20, num);
			}
			return 1;
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00114FC4 File Offset: 0x00113FC4
		public int GetControllerRunInformationIP_TCP(string strIP)
		{
			byte[] array = null;
			if (base.GetMjControllerRunInformationIP_TCP(strIP, ref array) != 1 || array == null)
			{
				return -1;
			}
			if (base.ControllerSN != -1)
			{
				this.m_runinfo.update(array, 20, (uint)base.ControllerSN);
			}
			else
			{
				uint num = (uint)((int)array[8] + ((int)array[9] << 8) + ((int)array[10] << 16) + ((int)array[11] << 24));
				this.m_runinfo.update(array, 20, num);
			}
			int num2 = 0;
			while (num2 < 10 && this.m_runinfo.newSwipes[num2].IndexInDataFlash != 4294967295U)
			{
				num2++;
			}
			return 1;
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00115050 File Offset: 0x00114050
		public int GetControllerRunInformationIPNoTries()
		{
			byte[] array = null;
			if (base.GetMjControllerRunInformationIPNoTries(ref array) != 1 || array == null)
			{
				return -1;
			}
			if (base.ControllerSN != -1)
			{
				this.m_runinfo.update(array, 20, (uint)base.ControllerSN);
			}
			else
			{
				uint num = (uint)((int)array[8] + ((int)array[9] << 8) + ((int)array[10] << 16) + ((int)array[11] << 24));
				this.m_runinfo.update(array, 20, num);
			}
			int num2 = 0;
			while (num2 < 10 && this.m_runinfo.newSwipes[num2].IndexInDataFlash != 4294967295U)
			{
				num2++;
			}
			return 1;
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x001150DC File Offset: 0x001140DC
		public int GetControlTaskListIP(ref wgMjControllerTaskList controlTaskList)
		{
			try
			{
				byte[] array = null;
				if (base.GetControlTaskListIP(ref array) == 1 && array != null)
				{
					controlTaskList = new wgMjControllerTaskList(array);
					return 1;
				}
			}
			catch (Exception)
			{
			}
			return -1;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x00115120 File Offset: 0x00114120
		public bool GetDoorActive(int doorNO)
		{
			return doorNO > 0 && doorNO <= 4 && this.m_doorActive[doorNO - 1];
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00115136 File Offset: 0x00114136
		public int GetDoorControl(int doorNO)
		{
			if (doorNO > 0 && doorNO <= 4)
			{
				return this.m_doorControl[doorNO - 1];
			}
			return 0;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0011514C File Offset: 0x0011414C
		public int GetDoorDelay(int doorNO)
		{
			if (doorNO > 0 && doorNO <= 4)
			{
				return this.m_doorDelay[doorNO - 1];
			}
			return 0;
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00115162 File Offset: 0x00114162
		public string GetDoorName(int doorNO)
		{
			if (doorNO > 0 && doorNO <= 4)
			{
				return wgTools.SetObjToStr(this.m_doorName[doorNO - 1]);
			}
			return "";
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x00115184 File Offset: 0x00114184
		public string GetDoorNameByReaderNO(int readerNO)
		{
			int num;
			if (wgMjController.GetControllerType(base.ControllerSN) == 4)
			{
				num = readerNO;
			}
			else
			{
				num = readerNO + 1 >> 1;
			}
			if (num > 0 && num <= 4)
			{
				return wgTools.SetObjToStr(this.m_doorName[num - 1]);
			}
			return "";
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x001151C8 File Offset: 0x001141C8
		public int GetDoorNO(string doorName)
		{
			int num = 0;
			while (num < 4 && !(this.m_doorName[num] == doorName))
			{
				num++;
			}
			if (num == 4)
			{
				return 1;
			}
			return num + 1;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x001151FE File Offset: 0x001141FE
		public string GetFloorName(int floorNO)
		{
			if (floorNO > 0 && floorNO <= this.m_floorName.Length)
			{
				return wgTools.SetObjToStr(this.m_floorName[floorNO - 1]);
			}
			return "";
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x00115224 File Offset: 0x00114224
		public string[] GetFloorNames()
		{
			return this.m_floorName;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0011522C File Offset: 0x0011422C
		public int GetInfoFromDBByControllerID(int ControllerID)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.GetInfoFromDBByControllerID_Acc(ControllerID);
			}
			int num = ControllerID;
			if (num > 0)
			{
				this.m_ControllerID = num;
				string text = " SELECT * ";
				text = text + " FROM t_b_Controller WHERE f_ControllerID =  " + num.ToString();
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						if (sqlDataReader.Read())
						{
							this.m_ControllerNO = (int)sqlDataReader["f_ControllerNO"];
							base.ControllerSN = (int)sqlDataReader["f_ControllerSN"];
							this.m_Active = int.Parse(sqlDataReader["f_Enabled"].ToString()) > 0;
							base.IP = wgTools.SetObjToStr(sqlDataReader["f_IP"]);
							base.PORT = (int)sqlDataReader["f_PORT"];
							this.m_Note = wgTools.SetObjToStr(sqlDataReader["f_Note"]);
							this.m_ZoneID = (int)sqlDataReader["f_ZoneID"];
						}
						sqlDataReader.Close();
						text = " SELECT * ";
						text = text + " FROM t_b_Door WHERE f_ControllerID =  " + num.ToString();
						sqlCommand.CommandText = text;
						sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							int num2 = int.Parse(sqlDataReader["f_DoorNO"].ToString()) - 1;
							this.m_doorName[num2] = wgTools.SetObjToStr(sqlDataReader["f_DoorName"]);
							this.m_doorControl[num2] = int.Parse(sqlDataReader["f_DoorControl"].ToString());
							this.m_doorDelay[num2] = int.Parse(sqlDataReader["f_DoorDelay"].ToString());
							this.m_doorActive[num2] = int.Parse(sqlDataReader["f_DoorEnabled"].ToString()) > 0;
						}
						sqlDataReader.Close();
						text = " SELECT * ";
						text = text + " FROM t_b_Reader WHERE f_ControllerID =  " + num.ToString();
						sqlCommand.CommandText = text;
						sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							int num3 = int.Parse(sqlDataReader["f_ReaderNo"].ToString()) - 1;
							this.m_readerName[num3] = wgTools.SetObjToStr(sqlDataReader["f_ReaderName"]);
							this.m_readerPasswordActive[num3] = int.Parse(sqlDataReader["f_PasswordEnabled"].ToString()) > 0;
							this.m_readerAsAttendActive[num3] = int.Parse(sqlDataReader["f_Attend"].ToString()) > 0;
							this.m_readerAsAttendControl[num3] = int.Parse(sqlDataReader["f_DutyOnOff"].ToString());
						}
						sqlDataReader.Close();
						text = "  SELECT t_b_Reader.f_ReaderName, t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
						text = text + "   t_b_Door.f_DoorName,    t_b_Floor.f_floorNO, t_b_Controller.f_ZoneID, t_b_Floor.f_floorName, t_b_Door.f_ControllerID      FROM t_b_Floor , t_b_Door, t_b_Controller, t_b_Reader    where t_b_Floor.f_DoorID = t_b_Door.f_DoorID and t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID and t_b_Reader.f_ControllerID = t_b_Floor.f_ControllerID  AND  t_b_Reader.f_ReaderNO =1  and  t_b_Floor.f_ControllerID =  " + num.ToString();
						sqlCommand.CommandText = text;
						sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							int num4 = int.Parse(sqlDataReader["f_floorNO"].ToString()) - 1;
							this.m_floorName[num4] = (string)sqlDataReader["f_floorFullName"];
						}
						sqlDataReader.Close();
					}
				}
			}
			return 1;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x001155B4 File Offset: 0x001145B4
		public int GetInfoFromDBByControllerID_Acc(int ControllerID)
		{
			int num = ControllerID;
			if (num > 0)
			{
				this.m_ControllerID = num;
				string text = " SELECT * ";
				text = text + " FROM t_b_Controller WHERE f_ControllerID =  " + num.ToString();
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							this.m_ControllerNO = (int)oleDbDataReader["f_ControllerNO"];
							base.ControllerSN = (int)oleDbDataReader["f_ControllerSN"];
							this.m_Active = int.Parse(oleDbDataReader["f_Enabled"].ToString()) > 0;
							base.IP = wgTools.SetObjToStr(oleDbDataReader["f_IP"]);
							base.PORT = (int)oleDbDataReader["f_PORT"];
							this.m_Note = wgTools.SetObjToStr(oleDbDataReader["f_Note"]);
							this.m_ZoneID = (int)oleDbDataReader["f_ZoneID"];
						}
						oleDbDataReader.Close();
						text = " SELECT * ";
						text = text + " FROM t_b_Door WHERE f_ControllerID =  " + num.ToString();
						oleDbCommand.CommandText = text;
						oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							int num2 = int.Parse(oleDbDataReader["f_DoorNO"].ToString()) - 1;
							this.m_doorName[num2] = (string)oleDbDataReader["f_DoorName"];
							this.m_doorControl[num2] = int.Parse(oleDbDataReader["f_DoorControl"].ToString());
							this.m_doorDelay[num2] = int.Parse(oleDbDataReader["f_DoorDelay"].ToString());
							this.m_doorActive[num2] = int.Parse(oleDbDataReader["f_DoorEnabled"].ToString()) > 0;
						}
						oleDbDataReader.Close();
						text = " SELECT * ";
						text = text + " FROM t_b_Reader WHERE f_ControllerID =  " + num.ToString();
						oleDbCommand.CommandText = text;
						oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							int num3 = int.Parse(oleDbDataReader["f_ReaderNo"].ToString()) - 1;
							this.m_readerName[num3] = wgTools.SetObjToStr(oleDbDataReader["f_ReaderName"]);
							this.m_readerPasswordActive[num3] = int.Parse(oleDbDataReader["f_PasswordEnabled"].ToString()) > 0;
							this.m_readerAsAttendActive[num3] = int.Parse(oleDbDataReader["f_Attend"].ToString()) > 0;
							this.m_readerAsAttendControl[num3] = int.Parse(oleDbDataReader["f_DutyOnOff"].ToString());
						}
						oleDbDataReader.Close();
					}
				}
			}
			return 1;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x001158C0 File Offset: 0x001148C0
		public int GetInfoFromDBByControllerSN(int parControllerSN)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.GetInfoFromDBByControllerSN_Acc(parControllerSN);
			}
			int num = parControllerSN;
			if (num > 0)
			{
				parControllerSN = num;
				string text = " SELECT * ";
				text = text + " FROM t_b_Controller WHERE f_ControllerSN =  " + num.ToString();
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						if (sqlDataReader.Read())
						{
							this.m_ControllerNO = (int)sqlDataReader["f_ControllerNO"];
							base.ControllerSN = (int)sqlDataReader["f_ControllerSN"];
							this.m_ControllerID = (int)sqlDataReader["f_ControllerID"];
							this.m_Active = int.Parse(sqlDataReader["f_Enabled"].ToString()) > 0;
							base.IP = wgTools.SetObjToStr(sqlDataReader["f_IP"]);
							base.PORT = (int)sqlDataReader["f_PORT"];
							this.m_Note = wgTools.SetObjToStr(sqlDataReader["f_Note"]);
							this.m_ZoneID = (int)sqlDataReader["f_ZoneID"];
						}
						sqlDataReader.Close();
						text = " SELECT * ";
						text = text + " FROM t_b_Door WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
						sqlCommand.CommandText = text;
						sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							int num2 = int.Parse(sqlDataReader["f_DoorNO"].ToString()) - 1;
							this.m_doorName[num2] = (string)sqlDataReader["f_DoorName"];
							this.m_doorControl[num2] = int.Parse(sqlDataReader["f_DoorControl"].ToString());
							this.m_doorDelay[num2] = int.Parse(sqlDataReader["f_DoorDelay"].ToString());
							this.m_doorActive[num2] = int.Parse(sqlDataReader["f_DoorEnabled"].ToString()) > 0;
						}
						sqlDataReader.Close();
						text = " SELECT * ";
						text = text + " FROM t_b_Reader WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
						sqlCommand.CommandText = text;
						sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							int num3 = int.Parse(sqlDataReader["f_ReaderNo"].ToString()) - 1;
							this.m_readerName[num3] = (string)sqlDataReader["f_ReaderName"];
							this.m_readerPasswordActive[num3] = int.Parse(sqlDataReader["f_PasswordEnabled"].ToString()) > 0;
							this.m_readerAsAttendActive[num3] = int.Parse(sqlDataReader["f_Attend"].ToString()) > 0;
							this.m_readerAsAttendControl[num3] = int.Parse(sqlDataReader["f_DutyOnOff"].ToString());
						}
						sqlDataReader.Close();
					}
				}
			}
			return 1;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00115BF8 File Offset: 0x00114BF8
		public int GetInfoFromDBByControllerSN_Acc(int parControllerSN)
		{
			int num = parControllerSN;
			if (num > 0)
			{
				parControllerSN = num;
				string text = " SELECT * ";
				text = text + " FROM t_b_Controller WHERE f_ControllerSN =  " + num.ToString();
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							this.m_ControllerNO = (int)oleDbDataReader["f_ControllerNO"];
							base.ControllerSN = (int)oleDbDataReader["f_ControllerSN"];
							this.m_ControllerID = (int)oleDbDataReader["f_ControllerID"];
							this.m_Active = int.Parse(oleDbDataReader["f_Enabled"].ToString()) > 0;
							base.IP = wgTools.SetObjToStr(oleDbDataReader["f_IP"]);
							base.PORT = (int)oleDbDataReader["f_PORT"];
							this.m_Note = wgTools.SetObjToStr(oleDbDataReader["f_Note"]);
							this.m_ZoneID = (int)oleDbDataReader["f_ZoneID"];
						}
						oleDbDataReader.Close();
						text = " SELECT * ";
						text = text + " FROM t_b_Door WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
						oleDbCommand.CommandText = text;
						oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							int num2 = int.Parse(oleDbDataReader["f_DoorNO"].ToString()) - 1;
							this.m_doorName[num2] = (string)oleDbDataReader["f_DoorName"];
							this.m_doorControl[num2] = int.Parse(oleDbDataReader["f_DoorControl"].ToString());
							this.m_doorDelay[num2] = int.Parse(oleDbDataReader["f_DoorDelay"].ToString());
							this.m_doorActive[num2] = int.Parse(oleDbDataReader["f_DoorEnabled"].ToString()) > 0;
						}
						oleDbDataReader.Close();
						text = " SELECT * ";
						text = text + " FROM t_b_Reader WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
						oleDbCommand.CommandText = text;
						oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							int num3 = int.Parse(oleDbDataReader["f_ReaderNo"].ToString()) - 1;
							this.m_readerName[num3] = (string)oleDbDataReader["f_ReaderName"];
							this.m_readerPasswordActive[num3] = int.Parse(oleDbDataReader["f_PasswordEnabled"].ToString()) > 0;
							this.m_readerAsAttendActive[num3] = int.Parse(oleDbDataReader["f_Attend"].ToString()) > 0;
							this.m_readerAsAttendControl[num3] = int.Parse(oleDbDataReader["f_DutyOnOff"].ToString());
						}
						oleDbDataReader.Close();
					}
				}
			}
			return 1;
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00115F20 File Offset: 0x00114F20
		public int GetInfoFromDBByDoorName(string DoorName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.GetInfoFromDBByDoorName_Acc(DoorName);
			}
			int num = 0;
			string text = " SELECT f_ControllerID ";
			text = text + " FROM t_b_Door WHERE f_DoorName =  " + wgTools.PrepareStrNUnicode(DoorName);
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					num = int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar()));
				}
			}
			if (num > 0)
			{
				this.GetInfoFromDBByControllerID(num);
			}
			return 1;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00115FCC File Offset: 0x00114FCC
		public int GetInfoFromDBByDoorName_Acc(string DoorName)
		{
			int num = 0;
			string text = " SELECT f_ControllerID ";
			text = text + " FROM t_b_Door WHERE f_DoorName =  " + wgTools.PrepareStrNUnicode(DoorName);
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					num = int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()));
				}
			}
			if (num > 0)
			{
				this.GetInfoFromDBByControllerID(num);
			}
			return 1;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00116068 File Offset: 0x00115068
		public int GetInfoFromDBByDoorNameSpecial(string DoorName, DataSet ds)
		{
			using (DataView dataView = new DataView(ds.Tables["t_b_Door"]))
			{
				using (DataView dataView2 = new DataView(ds.Tables["t_b_Controller"]))
				{
					using (DataView dataView3 = new DataView(ds.Tables["t_b_Reader"]))
					{
						using (DataView dataView4 = new DataView(ds.Tables["t_b_Floor"]))
						{
							dataView.RowFilter = "f_DoorName =  " + wgTools.PrepareStr(DoorName);
							if (dataView.Count <= 0)
							{
								return 0;
							}
							int num = (int)dataView[0]["f_ControllerID"];
							if (num > 0)
							{
								this.m_ControllerID = num;
								dataView2.RowFilter = " f_ControllerID =  " + num.ToString();
								if (dataView2.Count > 0)
								{
									this.m_ControllerNO = (int)dataView2[0]["f_ControllerNO"];
									base.ControllerSN = (int)dataView2[0]["f_ControllerSN"];
									this.m_Active = int.Parse(dataView2[0]["f_Enabled"].ToString()) > 0;
									base.IP = wgTools.SetObjToStr(dataView2[0]["f_IP"]);
									base.PORT = (int)dataView2[0]["f_PORT"];
									this.m_Note = wgTools.SetObjToStr(dataView2[0]["f_Note"]);
									this.m_ZoneID = (int)dataView2[0]["f_ZoneID"];
								}
								dataView.RowFilter = " f_ControllerID =  " + num.ToString();
								for (int i = 0; i < dataView.Count; i++)
								{
									int num2 = int.Parse(dataView[i]["f_DoorNO"].ToString()) - 1;
									this.m_doorName[num2] = (string)dataView[i]["f_DoorName"];
									this.m_doorControl[num2] = int.Parse(dataView[i]["f_DoorControl"].ToString());
									this.m_doorDelay[num2] = int.Parse(dataView[i]["f_DoorDelay"].ToString());
									this.m_doorActive[num2] = int.Parse(dataView[i]["f_DoorEnabled"].ToString()) > 0;
								}
								dataView3.RowFilter = "  f_ControllerID =  " + num.ToString();
								for (int j = 0; j < dataView3.Count; j++)
								{
									int num3 = int.Parse(dataView3[j]["f_ReaderNo"].ToString()) - 1;
									this.m_readerName[num3] = (string)dataView3[j]["f_ReaderName"];
									this.m_readerPasswordActive[num3] = int.Parse(dataView3[j]["f_PasswordEnabled"].ToString()) > 0;
									this.m_readerAsAttendActive[num3] = int.Parse(dataView3[j]["f_Attend"].ToString()) > 0;
									this.m_readerAsAttendControl[num3] = int.Parse(dataView3[j]["f_DutyOnOff"].ToString());
								}
								dataView4.RowFilter = "  f_ControllerID =  " + num.ToString();
								for (int k = 0; k < dataView4.Count; k++)
								{
									int num4 = int.Parse(dataView4[k]["f_floorNO"].ToString()) - 1;
									this.m_floorName[num4] = (string)dataView4[k]["f_floorFullName"];
								}
							}
						}
					}
				}
			}
			return 1;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x001164E0 File Offset: 0x001154E0
		public static int GetMaxControllerNO()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return icController.GetMaxControllerNO_Acc();
			}
			try
			{
				string text = "SELECT MAX(f_ControllerNO) from t_b_Controller";
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						return int.Parse(sqlCommand.ExecuteScalar().ToString());
					}
				}
			}
			catch
			{
			}
			return 0;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00116570 File Offset: 0x00115570
		public static int GetMaxControllerNO_Acc()
		{
			try
			{
				string text = "select max(CLNG(0 & [f_ControllerNO])) from t_b_Controller";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						return int.Parse(oleDbCommand.ExecuteScalar().ToString());
					}
				}
			}
			catch
			{
			}
			return 0;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x001165F4 File Offset: 0x001155F4
		public bool GetReaderAsAttendActive(int readerNO)
		{
			return readerNO > 0 && readerNO <= 4 && this.m_readerAsAttendActive[readerNO - 1];
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0011660A File Offset: 0x0011560A
		public int GetReaderAsAttendControl(int readerNO)
		{
			if (readerNO > 0 && readerNO <= 4)
			{
				return this.m_readerAsAttendControl[readerNO - 1];
			}
			return 0;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x00116620 File Offset: 0x00115620
		public string GetReaderName(int readerNO)
		{
			if (readerNO > 0 && readerNO <= 4)
			{
				return wgTools.SetObjToStr(this.m_readerName[readerNO - 1]);
			}
			return "";
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00116640 File Offset: 0x00115640
		public static bool IsExisted2NO(int ControllerNO, int ControllerIDExclude)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return icController.IsExisted2NO_Acc(ControllerNO, ControllerIDExclude);
			}
			bool flag = false;
			try
			{
				string text = string.Format("SELECT count(*) from [t_b_Controller] WHERE   [f_ControllerID]<> {0:d} AND [f_ControllerNO] ={1:d} ", ControllerIDExclude, ControllerNO);
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						if (int.Parse(sqlCommand.ExecuteScalar().ToString()) > 0)
						{
							flag = true;
						}
					}
					return flag;
				}
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x001166F0 File Offset: 0x001156F0
		public static bool IsExisted2NO_Acc(int ControllerNO, int ControllerIDExclude)
		{
			bool flag = false;
			try
			{
				string text = string.Format("SELECT count(*) from [t_b_Controller] WHERE   [f_ControllerID]<> {0:d} AND [f_ControllerNO] ={1:d} ", ControllerIDExclude, ControllerNO);
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						if (int.Parse(oleDbCommand.ExecuteScalar().ToString()) > 0)
						{
							flag = true;
						}
					}
					return flag;
				}
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00116790 File Offset: 0x00115790
		public static bool IsExisted2SN(int SN, int ControllerIDExclude)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return icController.IsExisted2SN_Acc(SN, ControllerIDExclude);
			}
			bool flag = false;
			try
			{
				string text = string.Format("SELECT count(*) from [t_b_Controller] WHERE [f_ControllerID]<> {0:d} AND [f_ControllerSN] ={1:d} ", ControllerIDExclude, SN);
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						if (int.Parse(sqlCommand.ExecuteScalar().ToString()) > 0)
						{
							flag = true;
						}
					}
					return flag;
				}
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00116840 File Offset: 0x00115840
		public static bool IsExisted2SN_Acc(int SN, int ControllerIDExclude)
		{
			bool flag = false;
			try
			{
				string text = string.Format("SELECT count(*) from [t_b_Controller] WHERE [f_ControllerID]<> {0:d} AND [f_ControllerSN] ={1:d} ", ControllerIDExclude, SN);
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						if (int.Parse(oleDbCommand.ExecuteScalar().ToString()) > 0)
						{
							flag = true;
						}
					}
					return flag;
				}
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x001168E0 File Offset: 0x001158E0
		public int RemoteOpenDoorIP(string DoorName)
		{
			int num = 0;
			while (num < 4 && !(this.m_doorName[num] == DoorName))
			{
				num++;
			}
			if (num >= 4)
			{
				return -1;
			}
			return base.RemoteOpenDoorIP(num + 1, (uint)icOperator.OperatorID, -1L);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00116920 File Offset: 0x00115920
		public int RemoteOpenDoorIP(string DoorName, uint operatorId, long operatorCardNO)
		{
			int num = 0;
			while (num < 4 && !(this.m_doorName[num] == DoorName))
			{
				num++;
			}
			if (num >= 4)
			{
				return -1;
			}
			return base.RemoteOpenDoorIP(num + 1, operatorId, operatorCardNO);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0011695A File Offset: 0x0011595A
		public void SetDoorActive(int doorNO, bool active)
		{
			if (doorNO > 0 && doorNO <= 4)
			{
				this.m_doorActive[doorNO - 1] = active;
			}
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0011696F File Offset: 0x0011596F
		public void SetDoorControl(int doorNO, int doorControl)
		{
			if (doorNO > 0 && doorNO <= 4 && doorControl >= 0 && doorControl <= 3)
			{
				this.m_doorControl[doorNO - 1] = doorControl;
			}
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0011698C File Offset: 0x0011598C
		public int SetdoorControlDelalyInfoFromDBByDoorNameSpecial(string DoorName, DataSet ds, int doorControl, int doorDelay)
		{
			using (DataView dataView = new DataView(ds.Tables["t_b_Door"]))
			{
				using (DataView dataView2 = new DataView(ds.Tables["t_b_Controller"]))
				{
					using (DataView dataView3 = new DataView(ds.Tables["t_b_Reader"]))
					{
						using (DataView dataView4 = new DataView(ds.Tables["t_b_Floor"]))
						{
							dataView.RowFilter = "f_DoorName =  " + wgTools.PrepareStr(DoorName);
							if (dataView.Count <= 0)
							{
								return 0;
							}
							int num = (int)dataView[0]["f_ControllerID"];
							if (num > 0)
							{
								this.m_ControllerID = num;
								dataView2.RowFilter = " f_ControllerID =  " + num.ToString();
								if (dataView2.Count > 0)
								{
									this.m_ControllerNO = (int)dataView2[0]["f_ControllerNO"];
									base.ControllerSN = (int)dataView2[0]["f_ControllerSN"];
									this.m_Active = int.Parse(dataView2[0]["f_Enabled"].ToString()) > 0;
									base.IP = wgTools.SetObjToStr(dataView2[0]["f_IP"]);
									base.PORT = (int)dataView2[0]["f_PORT"];
									this.m_Note = wgTools.SetObjToStr(dataView2[0]["f_Note"]);
									this.m_ZoneID = (int)dataView2[0]["f_ZoneID"];
								}
								dataView.RowFilter = " f_ControllerID =  " + num.ToString();
								for (int i = 0; i < dataView.Count; i++)
								{
									int num2 = int.Parse(dataView[i]["f_DoorNO"].ToString()) - 1;
									this.m_doorName[num2] = (string)dataView[i]["f_DoorName"];
									this.m_doorControl[num2] = int.Parse(dataView[i]["f_DoorControl"].ToString());
									this.m_doorDelay[num2] = int.Parse(dataView[i]["f_DoorDelay"].ToString());
									if (this.m_doorName[num2] == DoorName)
									{
										if (doorControl >= 0)
										{
											this.m_doorControl[num2] = doorControl;
											dataView[i]["f_DoorControl"] = doorControl;
										}
										if (doorDelay >= 0)
										{
											this.m_doorDelay[num2] = doorDelay;
											dataView[i]["f_DoorDelay"] = doorDelay;
										}
									}
									this.m_doorActive[num2] = int.Parse(dataView[i]["f_DoorEnabled"].ToString()) > 0;
								}
								dataView3.RowFilter = "  f_ControllerID =  " + num.ToString();
								for (int j = 0; j < dataView3.Count; j++)
								{
									int num3 = int.Parse(dataView3[j]["f_ReaderNo"].ToString()) - 1;
									this.m_readerName[num3] = (string)dataView3[j]["f_ReaderName"];
									this.m_readerPasswordActive[num3] = int.Parse(dataView3[j]["f_PasswordEnabled"].ToString()) > 0;
									this.m_readerAsAttendActive[num3] = int.Parse(dataView3[j]["f_Attend"].ToString()) > 0;
									this.m_readerAsAttendControl[num3] = int.Parse(dataView3[j]["f_DutyOnOff"].ToString());
								}
								dataView4.RowFilter = "  f_ControllerID =  " + num.ToString();
								for (int k = 0; k < dataView4.Count; k++)
								{
									int num4 = int.Parse(dataView4[k]["f_floorNO"].ToString()) - 1;
									this.m_floorName[num4] = (string)dataView4[k]["f_floorFullName"];
								}
							}
						}
					}
				}
			}
			return 1;
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00116E64 File Offset: 0x00115E64
		public void SetDoorControlToDB(int doorNO, int doorControl)
		{
			if (doorNO > 0 && doorNO <= 4 && doorControl >= 0 && doorControl <= 3)
			{
				this.m_doorControl[doorNO - 1] = doorControl;
			}
			wgAppConfig.runSql(string.Concat(new string[]
			{
				" UPDATE [t_b_Door] SET ",
				string.Format(" [f_DoorControl]={0} ", this.m_doorControl[doorNO - 1].ToString()),
				"  WHERE [f_ControllerID]=",
				this.m_ControllerID.ToString(),
				"  AND [f_DoorNO]=",
				(doorNO - 1 + 1).ToString()
			}));
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00116EF5 File Offset: 0x00115EF5
		public void SetDoorDelay(int doorNO, int doorDelay)
		{
			if (doorNO > 0 && doorNO <= 4)
			{
				this.m_doorDelay[doorNO - 1] = doorDelay;
			}
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00116F0C File Offset: 0x00115F0C
		public void SetDoorDelayToDB(int doorNO, int doorDelay)
		{
			if (doorNO > 0 && doorNO <= 4)
			{
				this.m_doorDelay[doorNO - 1] = doorDelay;
			}
			wgAppConfig.runSql(" UPDATE [t_b_Door] SET " + string.Format(" [f_DoorDelay]={0} ", this.m_doorDelay[doorNO - 1].ToString()));
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00116F59 File Offset: 0x00115F59
		public void SetDoorName(int doorNO, string doorName)
		{
			if (doorNO > 0 && doorNO <= 4)
			{
				this.m_doorName[doorNO - 1] = doorName;
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00116F6E File Offset: 0x00115F6E
		public void SetReaderAsAttendActive(int readerNO, bool active)
		{
			if (readerNO > 0 && readerNO <= 4)
			{
				this.m_readerAsAttendActive[readerNO - 1] = active;
			}
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00116F83 File Offset: 0x00115F83
		public void SetReaderAsAttendControl(int readerNO, int AttendControl)
		{
			if (readerNO > 0 && readerNO <= 4 && AttendControl >= 0 && AttendControl <= 3)
			{
				this.m_readerAsAttendControl[readerNO - 1] = AttendControl;
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00116FA0 File Offset: 0x00115FA0
		public void SetReaderName(int readerNO, string readerName)
		{
			if (readerNO > 0 && readerNO <= 4)
			{
				this.m_readerName[readerNO - 1] = readerName;
			}
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00116FB5 File Offset: 0x00115FB5
		public static string StrDelFirstSame(string mainInfo, string deletedInfo)
		{
			if (!string.IsNullOrEmpty(mainInfo) && !string.IsNullOrEmpty(deletedInfo) && mainInfo.IndexOf(deletedInfo) == 0)
			{
				return mainInfo.Substring(deletedInfo.Length);
			}
			return mainInfo;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00116FDE File Offset: 0x00115FDE
		public static string StrReplaceFirstSame(string mainInfo, string oldInfo, string newInfo)
		{
			if (!string.IsNullOrEmpty(mainInfo) && !string.IsNullOrEmpty(oldInfo) && !string.IsNullOrEmpty(newInfo) && mainInfo.IndexOf(oldInfo) == 0)
			{
				return newInfo + mainInfo.Substring(oldInfo.Length);
			}
			return mainInfo;
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x00117015 File Offset: 0x00116015
		public int UpdateControlTaskListIP(wgMjControllerTaskList controlTaskList, int UDPQueue4MultithreadIndex = -1)
		{
			return base.UpdateControlTaskListIP(controlTaskList.ToByte(), UDPQueue4MultithreadIndex);
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00117024 File Offset: 0x00116024
		public int UpdateControlTimeSegListIP(icControllerTimeSegList controlTimeSegList, int UDPQueue4MultithreadIndex = -1)
		{
			return base.UpdateControlTimeSegListIP(controlTimeSegList.ToByte(), UDPQueue4MultithreadIndex);
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00117034 File Offset: 0x00116034
		public int updateFloorNames(string[] floorName40)
		{
			if (floorName40.Length != 40)
			{
				return 0;
			}
			for (int i = 0; i < 40; i++)
			{
				this.m_floorName[i] = floorName40[i];
			}
			return 1;
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00117064 File Offset: 0x00116064
		public int UpdateIntoDB(bool ControllerTypeChanged)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.UpdateIntoDB_Acc(ControllerTypeChanged);
			}
			int num = -9;
			try
			{
				string text = "BEGIN TRANSACTION";
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (this.cm = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						this.cm.ExecuteNonQuery();
						try
						{
							string text2 = "";
							for (int i = 0; i < wgMjController.GetControllerType(base.ControllerSN); i++)
							{
								text2 = text2 + this.m_doorName[i] + ";   ";
							}
							text = " UPDATE t_b_Controller ";
							text = text + string.Format(" SET  f_ControllerNO={0}, f_ControllerSN={1}, f_Enabled={2}, f_IP={3}, f_PORT={4}, f_Note={5}, f_DoorsNames={6}, f_ZoneID={7} ", new object[]
							{
								this.m_ControllerNO.ToString(),
								base.ControllerSN.ToString(),
								this.m_Active ? "1" : "0",
								wgTools.PrepareStrNUnicode(base.IP),
								base.PORT.ToString(),
								wgTools.PrepareStrNUnicode(this.m_Note),
								wgTools.PrepareStrNUnicode(text2),
								this.m_ZoneID.ToString()
							}) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString();
							this.cm.CommandText = text;
							this.cm.ExecuteNonQuery();
							if (ControllerTypeChanged)
							{
								text = "DELETE FROM t_b_Reader WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
								this.cm.CommandText = text;
								this.cm.ExecuteNonQuery();
								text = "DELETE  FROM t_b_Door WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
								this.cm.CommandText = text;
								this.cm.ExecuteNonQuery();
								for (int i = 0; i < wgMjController.GetControllerType(base.ControllerSN); i++)
								{
									text = " INSERT INTO [t_b_Door] ";
									text = string.Concat(new string[]
									{
										text,
										"([f_ControllerID], [f_DoorNO], [f_DoorName], [f_DoorControl], [f_DoorDelay], [f_DoorEnabled]) Values(",
										this.m_ControllerID.ToString(),
										" , ",
										(i + 1).ToString(),
										" , ",
										wgTools.PrepareStrNUnicode(this.m_doorName[i]),
										" , ",
										this.m_doorControl[i].ToString(),
										" , ",
										this.m_doorDelay[i].ToString(),
										" , ",
										this.m_doorActive[i] ? "1" : "0",
										")"
									});
									this.cm.CommandText = text;
									this.cm.ExecuteNonQuery();
								}
								for (int i = 0; i < wgMjController.GetControllerReaderNum(base.ControllerSN); i++)
								{
									text = " INSERT INTO [t_b_Reader] ";
									text = string.Concat(new string[]
									{
										text,
										"([f_ControllerID], [f_ReaderNo], [f_ReaderName], [f_PasswordEnabled], [f_Attend],[f_DutyOnOff]) Values(",
										this.m_ControllerID.ToString(),
										" , ",
										(i + 1).ToString(),
										" , ",
										wgTools.PrepareStrNUnicode(this.m_readerName[i]),
										" , 0 , ",
										this.m_readerAsAttendActive[i] ? "1" : "0",
										" , ",
										this.m_readerAsAttendControl[i].ToString(),
										")"
									});
									this.cm.CommandText = text;
									this.cm.ExecuteNonQuery();
								}
							}
							else
							{
								text = " UPDATE [t_b_Door] SET ";
								text = text + string.Format(" [f_DoorName]= CONVERT(nvarchar(50),[f_DoorID]) + CONVERT(nvarchar(50),[f_DoorNO]) + {0} ", wgTools.PrepareStr(DateTime.Now.ToString("_yyyyMMddHHmmssffff"))) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString();
								this.cm.CommandText = text;
								this.cm.ExecuteNonQuery();
								for (int i = 0; i < wgMjController.GetControllerType(base.ControllerSN); i++)
								{
									text = " UPDATE [t_b_Door] SET ";
									text = string.Concat(new string[]
									{
										text,
										string.Format(" [f_DoorName]={0}, [f_DoorControl]={1}, [f_DoorDelay]={2}, [f_DoorEnabled]={3} ", new object[]
										{
											wgTools.PrepareStrNUnicode(this.m_doorName[i]),
											this.m_doorControl[i].ToString(),
											this.m_doorDelay[i].ToString(),
											this.m_doorActive[i] ? "1" : "0"
										}),
										"  WHERE [f_ControllerID]=",
										this.m_ControllerID.ToString(),
										"  AND [f_DoorNO]=",
										(i + 1).ToString()
									});
									this.cm.CommandText = text;
									this.cm.ExecuteNonQuery();
									text = " SELECT [f_ControllerID] FROM [t_b_Door]  ";
									text = string.Concat(new string[]
									{
										text,
										"  WHERE [f_ControllerID]=",
										this.m_ControllerID.ToString(),
										"  AND [f_DoorNO]=",
										(i + 1).ToString()
									});
									this.cm.CommandText = text;
									if (int.Parse("0" + wgTools.SetObjToStr(this.cm.ExecuteScalar())) <= 0)
									{
										text = " INSERT INTO [t_b_Door] ";
										text = string.Concat(new string[]
										{
											text,
											"([f_ControllerID], [f_DoorNO], [f_DoorName], [f_DoorControl], [f_DoorDelay], [f_DoorEnabled]) Values(",
											this.m_ControllerID.ToString(),
											" , ",
											(i + 1).ToString(),
											" , ",
											wgTools.PrepareStrNUnicode(this.m_doorName[i]),
											" , ",
											this.m_doorControl[i].ToString(),
											" , ",
											this.m_doorDelay[i].ToString(),
											" , ",
											this.m_doorActive[i] ? "1" : "0",
											")"
										});
										this.cm.CommandText = text;
										this.cm.ExecuteNonQuery();
									}
								}
								text = " UPDATE [t_b_Reader] SET ";
								text = text + string.Format(" [f_ReaderName]= CONVERT(nvarchar(50),[f_ReaderID]) + CONVERT(nvarchar(50),[f_ReaderNo]) + {0} ", wgTools.PrepareStrNUnicode(DateTime.Now.ToString("_yyyyMMddHHmmssffff"))) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString();
								this.cm.CommandText = text;
								this.cm.ExecuteNonQuery();
								for (int i = 0; i < wgMjController.GetControllerReaderNum(base.ControllerSN); i++)
								{
									text = " UPDATE [t_b_Reader] SET ";
									text = string.Concat(new string[]
									{
										text,
										string.Format(" [f_ReaderName]={0}, [f_Attend]={1},[f_DutyOnOff]={2}", wgTools.PrepareStrNUnicode(this.m_readerName[i]), this.m_readerAsAttendActive[i] ? "1" : "0", this.m_readerAsAttendControl[i].ToString()),
										"  WHERE [f_ControllerID]=",
										this.m_ControllerID.ToString(),
										"  AND [f_ReaderNo]=",
										(i + 1).ToString()
									});
									this.cm.CommandText = text;
									this.cm.ExecuteNonQuery();
									text = " SELECT [f_ControllerID] FROM [t_b_Reader]  ";
									text = string.Concat(new string[]
									{
										text,
										"  WHERE [f_ControllerID]=",
										this.m_ControllerID.ToString(),
										"  AND [f_ReaderNo]=",
										(i + 1).ToString()
									});
									this.cm.CommandText = text;
									if (int.Parse("0" + wgTools.SetObjToStr(this.cm.ExecuteScalar())) <= 0)
									{
										text = " INSERT INTO [t_b_Reader] ";
										text = string.Concat(new string[]
										{
											text,
											"([f_ControllerID], [f_ReaderNo], [f_ReaderName], [f_PasswordEnabled], [f_Attend],[f_DutyOnOff]) Values(",
											this.m_ControllerID.ToString(),
											" , ",
											(i + 1).ToString(),
											" , ",
											wgTools.PrepareStrNUnicode(this.m_readerName[i]),
											" , 0 , ",
											this.m_readerAsAttendActive[i] ? "1" : "0",
											" , ",
											this.m_readerAsAttendControl[i].ToString(),
											")"
										});
										this.cm.CommandText = text;
										this.cm.ExecuteNonQuery();
									}
								}
							}
							text = "COMMIT TRANSACTION";
							this.cm.CommandText = text;
							this.cm.ExecuteNonQuery();
							num = 1;
						}
						catch (Exception)
						{
							text = "ROLLBACK TRANSACTION";
							this.cm.CommandText = text;
							this.cm.ExecuteNonQuery();
						}
						return num;
					}
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x001179EC File Offset: 0x001169EC
		public int UpdateIntoDB_Acc(bool ControllerTypeChanged)
		{
			int num = -9;
			try
			{
				string text = "BEGIN TRANSACTION";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (this.cm_Acc = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						this.cm_Acc.ExecuteNonQuery();
						try
						{
							string text2 = "";
							for (int i = 0; i < wgMjController.GetControllerType(base.ControllerSN); i++)
							{
								text2 = text2 + this.m_doorName[i] + ";   ";
							}
							text = " UPDATE t_b_Controller ";
							text = text + string.Format(" SET  f_ControllerNO={0}, f_ControllerSN={1}, f_Enabled={2}, f_IP={3}, f_PORT={4}, f_Note={5}, f_DoorsNames={6}, f_ZoneID={7} ", new object[]
							{
								this.m_ControllerNO.ToString(),
								base.ControllerSN.ToString(),
								this.m_Active ? "1" : "0",
								wgTools.PrepareStrNUnicode(base.IP),
								base.PORT.ToString(),
								wgTools.PrepareStrNUnicode(this.m_Note),
								wgTools.PrepareStrNUnicode(text2),
								this.m_ZoneID.ToString()
							}) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString();
							this.cm_Acc.CommandText = text;
							this.cm_Acc.ExecuteNonQuery();
							if (ControllerTypeChanged)
							{
								text = "DELETE FROM t_b_Reader WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
								this.cm_Acc.CommandText = text;
								this.cm_Acc.ExecuteNonQuery();
								text = "DELETE  FROM t_b_Door WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
								this.cm_Acc.CommandText = text;
								this.cm_Acc.ExecuteNonQuery();
								for (int i = 0; i < wgMjController.GetControllerType(base.ControllerSN); i++)
								{
									text = " INSERT INTO [t_b_Door] ";
									text = string.Concat(new string[]
									{
										text,
										"([f_ControllerID], [f_DoorNO], [f_DoorName], [f_DoorControl], [f_DoorDelay], [f_DoorEnabled]) Values(",
										this.m_ControllerID.ToString(),
										" , ",
										(i + 1).ToString(),
										" , ",
										wgTools.PrepareStrNUnicode(this.m_doorName[i]),
										" , ",
										this.m_doorControl[i].ToString(),
										" , ",
										this.m_doorDelay[i].ToString(),
										" , ",
										this.m_doorActive[i] ? "1" : "0",
										")"
									});
									this.cm_Acc.CommandText = text;
									this.cm_Acc.ExecuteNonQuery();
								}
								for (int i = 0; i < wgMjController.GetControllerReaderNum(base.ControllerSN); i++)
								{
									text = " INSERT INTO [t_b_Reader] ";
									text = string.Concat(new string[]
									{
										text,
										"([f_ControllerID], [f_ReaderNo], [f_ReaderName], [f_PasswordEnabled], [f_Attend],[f_DutyOnOff]) Values(",
										this.m_ControllerID.ToString(),
										" , ",
										(i + 1).ToString(),
										" , ",
										wgTools.PrepareStrNUnicode(this.m_readerName[i]),
										" , 0 , ",
										this.m_readerAsAttendActive[i] ? "1" : "0",
										" , ",
										this.m_readerAsAttendControl[i].ToString(),
										")"
									});
									this.cm_Acc.CommandText = text;
									this.cm_Acc.ExecuteNonQuery();
								}
							}
							else
							{
								text = " UPDATE [t_b_Door] SET ";
								text = text + string.Format(" [f_DoorName]= CSTR([f_DoorID]) & CSTR([f_DoorNO]) & {0} ", wgTools.PrepareStrNUnicode(DateTime.Now.ToString("_yyyyMMddHHmmssffff"))) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString();
								this.cm_Acc.CommandText = text;
								this.cm_Acc.ExecuteNonQuery();
								for (int i = 0; i < wgMjController.GetControllerType(base.ControllerSN); i++)
								{
									text = " UPDATE [t_b_Door] SET ";
									text = string.Concat(new string[]
									{
										text,
										string.Format(" [f_DoorName]={0}, [f_DoorControl]={1}, [f_DoorDelay]={2}, [f_DoorEnabled]={3} ", new object[]
										{
											wgTools.PrepareStrNUnicode(this.m_doorName[i]),
											this.m_doorControl[i].ToString(),
											this.m_doorDelay[i].ToString(),
											this.m_doorActive[i] ? "1" : "0"
										}),
										"  WHERE [f_ControllerID]=",
										this.m_ControllerID.ToString(),
										"  AND [f_DoorNO]=",
										(i + 1).ToString()
									});
									this.cm_Acc.CommandText = text;
									this.cm_Acc.ExecuteNonQuery();
									text = " SELECT [f_ControllerID] FROM [t_b_Door]  ";
									text = string.Concat(new string[]
									{
										text,
										"  WHERE [f_ControllerID]=",
										this.m_ControllerID.ToString(),
										"  AND [f_DoorNO]=",
										(i + 1).ToString()
									});
									this.cm_Acc.CommandText = text;
									if (int.Parse("0" + wgTools.SetObjToStr(this.cm_Acc.ExecuteScalar())) <= 0)
									{
										text = " INSERT INTO [t_b_Door] ";
										text = string.Concat(new string[]
										{
											text,
											"([f_ControllerID], [f_DoorNO], [f_DoorName], [f_DoorControl], [f_DoorDelay], [f_DoorEnabled]) Values(",
											this.m_ControllerID.ToString(),
											" , ",
											(i + 1).ToString(),
											" , ",
											wgTools.PrepareStrNUnicode(this.m_doorName[i]),
											" , ",
											this.m_doorControl[i].ToString(),
											" , ",
											this.m_doorDelay[i].ToString(),
											" , ",
											this.m_doorActive[i] ? "1" : "0",
											")"
										});
										this.cm_Acc.CommandText = text;
										this.cm_Acc.ExecuteNonQuery();
									}
								}
								text = " UPDATE [t_b_Reader] SET ";
								text = text + string.Format(" [f_ReaderName]= CSTR([f_ReaderID]) & CSTR([f_ReaderNo]) & {0} ", wgTools.PrepareStr(DateTime.Now.ToString("_yyyyMMddHHmmssffff"))) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString();
								this.cm_Acc.CommandText = text;
								this.cm_Acc.ExecuteNonQuery();
								for (int i = 0; i < wgMjController.GetControllerReaderNum(base.ControllerSN); i++)
								{
									text = " UPDATE [t_b_Reader] SET ";
									text = string.Concat(new string[]
									{
										text,
										string.Format(" [f_ReaderName]={0}, [f_Attend]={1},[f_DutyOnOff]={2}", wgTools.PrepareStrNUnicode(this.m_readerName[i]), this.m_readerAsAttendActive[i] ? "1" : "0", this.m_readerAsAttendControl[i].ToString()),
										"  WHERE [f_ControllerID]=",
										this.m_ControllerID.ToString(),
										"  AND [f_ReaderNo]=",
										(i + 1).ToString()
									});
									this.cm_Acc.CommandText = text;
									this.cm_Acc.ExecuteNonQuery();
									text = " SELECT [f_ControllerID] FROM [t_b_Reader]  ";
									text = string.Concat(new string[]
									{
										text,
										"  WHERE [f_ControllerID]=",
										this.m_ControllerID.ToString(),
										"  AND [f_ReaderNo]=",
										(i + 1).ToString()
									});
									this.cm_Acc.CommandText = text;
									if (int.Parse("0" + wgTools.SetObjToStr(this.cm_Acc.ExecuteScalar())) <= 0)
									{
										text = " INSERT INTO [t_b_Reader] ";
										text = string.Concat(new string[]
										{
											text,
											"([f_ControllerID], [f_ReaderNo], [f_ReaderName], [f_PasswordEnabled], [f_Attend],[f_DutyOnOff]) Values(",
											this.m_ControllerID.ToString(),
											" , ",
											(i + 1).ToString(),
											" , ",
											wgTools.PrepareStrNUnicode(this.m_readerName[i]),
											" , 0 , ",
											this.m_readerAsAttendActive[i] ? "1" : "0",
											" , ",
											this.m_readerAsAttendControl[i].ToString(),
											")"
										});
										this.cm_Acc.CommandText = text;
										this.cm_Acc.ExecuteNonQuery();
									}
								}
							}
							text = "COMMIT TRANSACTION";
							this.cm_Acc.CommandText = text;
							this.cm_Acc.ExecuteNonQuery();
							num = 1;
						}
						catch (Exception)
						{
							text = "ROLLBACK TRANSACTION";
							this.cm_Acc.CommandText = text;
							this.cm_Acc.ExecuteNonQuery();
						}
						return num;
					}
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x00118364 File Offset: 0x00117364
		// (set) Token: 0x06000FB4 RID: 4020 RVA: 0x0011836C File Offset: 0x0011736C
		public bool Active
		{
			get
			{
				return this.m_Active;
			}
			set
			{
				this.m_Active = value;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00118375 File Offset: 0x00117375
		// (set) Token: 0x06000FB6 RID: 4022 RVA: 0x0011837D File Offset: 0x0011737D
		public int ControllerID
		{
			get
			{
				return this.m_ControllerID;
			}
			set
			{
				if (this.m_ControllerID >= 0)
				{
					this.m_ControllerID = value;
				}
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0011838F File Offset: 0x0011738F
		// (set) Token: 0x06000FB8 RID: 4024 RVA: 0x00118397 File Offset: 0x00117397
		public int ControllerNO
		{
			get
			{
				return this.m_ControllerNO;
			}
			set
			{
				if (this.m_ControllerNO >= 0)
				{
					this.m_ControllerNO = value;
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x001183A9 File Offset: 0x001173A9
		// (set) Token: 0x06000FBA RID: 4026 RVA: 0x001183B1 File Offset: 0x001173B1
		public string Note
		{
			get
			{
				return this.m_Note;
			}
			set
			{
				this.m_Note = value;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x001183BA File Offset: 0x001173BA
		public ControllerRunInformation runinfo
		{
			get
			{
				return this.m_runinfo;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x001183C2 File Offset: 0x001173C2
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x001183CA File Offset: 0x001173CA
		public int ZoneID
		{
			get
			{
				return this.m_ZoneID;
			}
			set
			{
				this.m_ZoneID = value;
			}
		}

		// Token: 0x04001C60 RID: 7264
		private const int DoorMax = 4;

		// Token: 0x04001C61 RID: 7265
		public const int DUTY_ON = 1;

		// Token: 0x04001C62 RID: 7266
		public const int DUTY_OFF = 2;

		// Token: 0x04001C63 RID: 7267
		public const int DUTY_ONOFF = 3;

		// Token: 0x04001C64 RID: 7268
		private const int ReaderMax = 4;

		// Token: 0x04001C65 RID: 7269
		private SqlCommand cm;

		// Token: 0x04001C66 RID: 7270
		private OleDbCommand cm_Acc;

		// Token: 0x04001C67 RID: 7271
		private bool m_Active = true;

		// Token: 0x04001C68 RID: 7272
		private int m_ControllerID;

		// Token: 0x04001C69 RID: 7273
		private int m_ControllerNO;

		// Token: 0x04001C6A RID: 7274
		private bool[] m_doorActive = new bool[] { true, true, true, true };

		// Token: 0x04001C6B RID: 7275
		private int[] m_doorControl = new int[] { 3, 3, 3, 3 };

		// Token: 0x04001C6C RID: 7276
		private int[] m_doorDelay = new int[] { 3, 3, 3, 3 };

		// Token: 0x04001C6D RID: 7277
		private string[] m_doorName = new string[] { "", "", "", "" };

		// Token: 0x04001C6E RID: 7278
		private string[] m_floorName = new string[]
		{
			"", "", "", "", "", "", "", "", "", "",
			"", "", "", "", "", "", "", "", "", "",
			"", "", "", "", "", "", "", "", "", "",
			"", "", "", "", "", "", "", "", "", ""
		};

		// Token: 0x04001C6F RID: 7279
		private string m_Note = "";

		// Token: 0x04001C70 RID: 7280
		private bool[] m_readerAsAttendActive = new bool[] { true, true, true, true };

		// Token: 0x04001C71 RID: 7281
		private int[] m_readerAsAttendControl = new int[] { 3, 3, 3, 3 };

		// Token: 0x04001C72 RID: 7282
		private string[] m_readerName = new string[] { "", "", "", "" };

		// Token: 0x04001C73 RID: 7283
		private bool[] m_readerPasswordActive = new bool[4];

		// Token: 0x04001C74 RID: 7284
		private ControllerRunInformation m_runinfo = new ControllerRunInformation();

		// Token: 0x04001C75 RID: 7285
		private int m_ZoneID;
	}
}
