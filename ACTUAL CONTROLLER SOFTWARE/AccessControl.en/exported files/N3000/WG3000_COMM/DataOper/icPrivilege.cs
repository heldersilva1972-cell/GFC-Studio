using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.DataOper
{
	// Token: 0x02000060 RID: 96
	internal class icPrivilege : wgMjControllerPrivilege
	{
		// Token: 0x0600074C RID: 1868 RVA: 0x000CF6D8 File Offset: 0x000CE6D8
		public icPrivilege()
		{
			base.AllowUpload();
			this.bAllowUploadUserName = false;
			int num = 0;
			try
			{
				int.TryParse(wgAppConfig.GetKeyVal("AllowUploadUserName"), out num);
			}
			catch (Exception)
			{
			}
			if (num > 0)
			{
				this.bAllowUploadUserName = true;
			}
			int num2 = 0;
			try
			{
				int.TryParse(wgAppConfig.GetKeyVal("AllowUploadUserMobile"), out num2);
			}
			catch (Exception)
			{
			}
			if (num2 > 0)
			{
				this.bAllowUploadMobile = true;
			}
			if (!this.bAllowUploadUserName)
			{
				this.bAllowUploadMobile = false;
			}
			this.bAllowUploadTwoCardCheck = wgAppConfig.getParamValBoolByNO(200);
			if (wgTools.gWGYTJ)
			{
				this.bAllowUploadUserName = true;
			}
			if (wgAppConfig.IsActivateCard19)
			{
				icPrivilege.CARDNOMAX = long.MaxValue;
			}
			else
			{
				icPrivilege.CARDNOMAX = (long)((ulong)(-2));
			}
			if (this.dtPrivilege == null)
			{
				this.dtPrivilege = new DataTable("Privilege");
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.UInt32");
				this.dc.ColumnName = "f_ConsumerID";
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int64");
				this.dc.ColumnName = "f_CardNO";
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_BeginYMD";
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_EndYMD";
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.String");
				this.dc.ColumnName = "f_PIN";
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_ControlSegID1";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_ControlSegID2";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_ControlSegID3";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_ControlSegID4";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_DoorFirstCard_1";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_DoorFirstCard_2";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_DoorFirstCard_3";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_DoorFirstCard_4";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_MoreCards_GrpID_1";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_MoreCards_GrpID_2";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_MoreCards_GrpID_3";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_MoreCards_GrpID_4";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.UInt32");
				this.dc.ColumnName = "f_MaxSwipe";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Byte");
				this.dc.ColumnName = "f_IsSuperCard";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.UInt64");
				this.dc.ColumnName = "f_AllowFloors";
				this.dc.DefaultValue = 0;
				this.dtPrivilege.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.String");
				this.dc.ColumnName = "f_ConsumerName";
				this.dtPrivilege.Columns.Add(this.dc);
			}
			this.bDisplayProcess = true;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x000CFE9C File Offset: 0x000CEE9C
		public int AddPrivilegeOfOneCardByDB(int ControllerID, int ConsumerID)
		{
			int num = 0;
			string text = "";
			int num2 = 60000;
			int num3 = -1;
			long num4 = -1L;
			MjRegisterCard privilegeOfOneCardByDB = this.GetPrivilegeOfOneCardByDB(ControllerID, ConsumerID, ref num4, ref num, ref text, ref num2);
			if (privilegeOfOneCardByDB == null)
			{
				return -1;
			}
			if (privilegeOfOneCardByDB.CardID > 0L)
			{
				num3 = base.AddPrivilegeOfOneCardIP(num, text, num2, privilegeOfOneCardByDB);
			}
			return num3;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000CFEF0 File Offset: 0x000CEEF0
		public int AddPrivilegeWithUsernameOfOneCardByDB(int ControllerID, int ConsumerID)
		{
			int num = 0;
			string text = "";
			int num2 = 60000;
			int num3 = -1;
			long num4 = -1L;
			MjRegisterCard privilegeOfOneCardByDB = this.GetPrivilegeOfOneCardByDB(ControllerID, ConsumerID, ref num4, ref num, ref text, ref num2);
			if (privilegeOfOneCardByDB == null)
			{
				return -1;
			}
			if (privilegeOfOneCardByDB.CardID > 0L)
			{
				num3 = base.AddPrivilegeWithUsernameOfOneCardIP(num, text, num2, privilegeOfOneCardByDB);
			}
			return num3;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x000CFF44 File Offset: 0x000CEF44
		public int DelPrivilegeOfOneCardByDB(int ControllerID, int ConsumerID)
		{
			int num = 0;
			string text = "";
			int num2 = 60000;
			long num3 = -1L;
			MjRegisterCard privilegeOfOneCardByDB = this.GetPrivilegeOfOneCardByDB(ControllerID, ConsumerID, ref num3, ref num, ref text, ref num2);
			if (privilegeOfOneCardByDB == null)
			{
				return -1;
			}
			if (privilegeOfOneCardByDB.CardID > 0L)
			{
				return base.AddPrivilegeOfOneCardIP(num, text, num2, privilegeOfOneCardByDB);
			}
			return base.DelPrivilegeOfOneCardIP(num, text, num2, num3);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x000CFF9C File Offset: 0x000CEF9C
		protected override void DisplayProcessInfo(string info, int infoCode, int specialInfo)
		{
			if (this.bDisplayProcess)
			{
				if (infoCode == -100001)
				{
					wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", info, wgTools.gADCT ? CommonStr.strUploadFail_200K : (wgTools.gWGYTJ ? CommonStr.strUploadFail_500 : CommonStr.strUploadFail_40K), specialInfo));
					return;
				}
				switch (infoCode)
				{
				case 100001:
					wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", info, CommonStr.strUploadPreparing));
					return;
				case 100002:
					wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", info, CommonStr.strUploadingPrivileges, specialInfo));
					return;
				case 100003:
					wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", info, CommonStr.strUploadedPrivileges, specialInfo));
					return;
				default:
					wgAppRunInfo.raiseAppRunInfoCommStatus(info);
					break;
				}
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x000D0068 File Offset: 0x000CF068
		public int getControllerIDByDoorName(string DoorName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getControllerIDByDoorName_Acc(DoorName);
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
			return num;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x000D0108 File Offset: 0x000CF108
		public int getControllerIDByDoorName_Acc(string DoorName)
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
			return num;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000D0198 File Offset: 0x000CF198
		public int getControllerSNByDoorName(string DoorName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getControllerSNByDoorName_Acc(DoorName);
			}
			int num = 0;
			string text = " SELECT f_ControllerSN ";
			text = text + " FROM t_b_Door a,t_b_Controller b WHERE a.f_ControllerID = b.f_ControllerID AND f_DoorName =  " + wgTools.PrepareStrNUnicode(DoorName);
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					num = int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar()));
				}
			}
			return num;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x000D0238 File Offset: 0x000CF238
		public int getControllerSNByDoorName_Acc(string DoorName)
		{
			int num = 0;
			string text = " SELECT f_ControllerSN ";
			text = text + " FROM t_b_Door a,t_b_Controller b WHERE a.f_ControllerID = b.f_ControllerID AND f_DoorName =  " + wgTools.PrepareStrNUnicode(DoorName);
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					num = int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()));
				}
			}
			return num;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000D02C8 File Offset: 0x000CF2C8
		public int getControllerSNByID(int ControllerID)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getControllerSNByID_Acc(ControllerID);
			}
			int num = 0;
			string text = " SELECT f_ControllerSN ";
			text = text + " FROM t_b_Controller b WHERE  b.f_ControllerID =  " + ControllerID;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					num = int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar()));
				}
			}
			return num;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000D0368 File Offset: 0x000CF368
		public int getControllerSNByID_Acc(int ControllerID)
		{
			int num = 0;
			string text = " SELECT f_ControllerSN ";
			text = text + " FROM t_b_Controller b WHERE  b.f_ControllerID =  " + ControllerID;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					num = int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()));
				}
			}
			return num;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000D03F8 File Offset: 0x000CF3F8
		public int getElevatorPrivilegeByID(int ControllerID)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getElevatorPrivilegeByID_Acc(ControllerID);
			}
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
			}
			int num = int.Parse("0" + wgAppConfig.getSystemParamByNO(208));
			wgTools.WriteLine("getPrivilegeByID Start");
			this.dtPrivilege.Rows.Clear();
			string text = string.Format(" SELECT a.f_ConsumerID, COUNT(a.f_FloorID) as cnt FROM t_b_UserFloor a\r\nWHERE a.f_FloorID IN \r\n(SELECT b.f_floorid from t_b_floor b where b.[f_ControllerID]={0} or b.[f_ControllerID] in \r\n (select c.f_ControllerID from t_b_ElevatorGroup c where c.f_ElevatorGroupNO in \r\n   (select d.f_ElevatorGroupNO from t_b_ElevatorGroup d where d.f_ControllerID = {0})))\r\nGROUP BY a.f_ConsumerID ", ControllerID);
			this.dtUserFloorCnt = new DataTable();
			this.dvUserFloorCnt = new DataView(this.dtUserFloorCnt);
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						sqlDataAdapter.Fill(this.dtUserFloorCnt);
					}
				}
			}
			text = string.Format("SELECT b.f_CardNO, b.f_ConsumerID, b.f_BeginYMD, b.f_EndYMD, b.f_PIN, a.f_ControlSegID, b.f_DoorEnabled, c.f_floorNO,b.f_consumername \r\nFROM [t_b_UserFloor] a\r\nINNER JOIN t_b_Consumer b ON  a.f_ConsumerID = b.f_ConsumerID AND b.f_CardNO IS NOT NULL\r\nINNER JOIN t_b_Floor c ON a.f_FloorID = c.f_FloorID\r\nWHERE f_ControllerID = {0}\r\nORDER BY f_CardNO", ControllerID);
			this.dtUserFloor = new DataTable();
			using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
				{
					using (SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sqlCommand2))
					{
						sqlCommand2.CommandTimeout = wgAppConfig.dbCommandTimeout;
						sqlDataAdapter2.Fill(this.dtUserFloor);
					}
				}
			}
			DataRow dataRow = this.dtPrivilege.NewRow();
			dataRow["f_CardNO"] = 0;
			dataRow["f_ControlSegID1"] = 0;
			dataRow["f_ControlSegID2"] = 0;
			dataRow["f_ControlSegID3"] = 0;
			dataRow["f_ControlSegID4"] = 0;
			dataRow["f_MoreCards_GrpID_3"] = 0;
			dataRow["f_MoreCards_GrpID_4"] = 0;
			dataRow["f_MoreCards_GrpID_2"] = 0;
			dataRow["f_DoorFirstCard_2"] = 0;
			dataRow["f_DoorFirstCard_3"] = 0;
			dataRow["f_DoorFirstCard_4"] = 0;
			dataRow["f_PIN"] = 0;
			int num2 = 0;
			for (int i = 0; i < this.dtUserFloor.Rows.Count; i++)
			{
				if (wgMjControllerPrivilege.bStopUploadPrivilege)
				{
					return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
				}
				DataRow dataRow2 = this.dtUserFloor.Rows[i];
				if ((long)dataRow2["f_CardNO"] >= 0L && (long)dataRow2["f_CardNO"] <= icPrivilege.CARDNOMAX && int.Parse(wgTools.SetObjToStr(dataRow2["f_DoorEnabled"])) == 1)
				{
					if ((long)dataRow["f_CardNO"] != (long)dataRow2["f_CardNO"])
					{
						if ((long)dataRow["f_CardNO"] > 0L)
						{
							this.dtPrivilege.Rows.Add(dataRow);
							dataRow = this.dtPrivilege.NewRow();
							dataRow["f_ControlSegID1"] = 0;
							dataRow["f_ControlSegID2"] = 0;
							dataRow["f_ControlSegID3"] = 0;
							dataRow["f_ControlSegID4"] = 0;
							dataRow["f_MoreCards_GrpID_3"] = 0;
							dataRow["f_MoreCards_GrpID_4"] = 0;
							dataRow["f_MoreCards_GrpID_2"] = 0;
							dataRow["f_DoorFirstCard_2"] = 0;
							dataRow["f_DoorFirstCard_3"] = 0;
							dataRow["f_DoorFirstCard_4"] = 0;
							dataRow["f_PIN"] = 0;
						}
						dataRow["f_CardNO"] = (long)dataRow2["f_CardNO"];
						dataRow["f_ConsumerID"] = (uint)((int)dataRow2["f_ConsumerID"]);
						dataRow["f_BeginYMD"] = dataRow2["f_BeginYMD"];
						dataRow["f_EndYMD"] = dataRow2["f_EndYMD"];
						dataRow["f_PIN"] = dataRow2["f_PIN"];
						this.dvUserFloorCnt.RowFilter = "f_ConsumerID = " + (uint)((int)dataRow2["f_ConsumerID"]);
						if (this.dvUserFloorCnt.Count >= 1 && (int)this.dvUserFloorCnt[0]["cnt"] >= 2)
						{
							dataRow["f_AllowFloors"] = (ulong)dataRow["f_AllowFloors"] | 1099511627776UL;
						}
						dataRow["f_ControlSegID1"] = dataRow2["f_ControlSegID"];
						dataRow["f_ConsumerName"] = dataRow2["f_ConsumerName"];
					}
					int num3 = int.Parse(dataRow2["f_floorNO"].ToString());
					if (num3 > 0 && num3 <= 40)
					{
						dataRow["f_AllowFloors"] = (ulong)dataRow["f_AllowFloors"] | (1UL << num3 - 1);
					}
					if (num > 0 && (ulong)dataRow["f_AllowFloors"] > 0UL)
					{
						dataRow["f_AllowFloors"] = (ulong)dataRow["f_AllowFloors"] | 1099511627776UL;
					}
					num2++;
				}
			}
			if ((long)dataRow["f_CardNO"] > 0L)
			{
				this.dtPrivilege.Rows.Add(dataRow);
			}
			this.dtPrivilege.AcceptChanges();
			this.m_PrivilegeTotal = num2;
			this.m_ValidPrivilegeTotal = num2;
			this.m_ConsumersTotal = this.dtPrivilege.Rows.Count;
			wgTools.WriteLine("getElevatorPrivilegeByID End");
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
			}
			return 1;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000D0A94 File Offset: 0x000CFA94
		public int getElevatorPrivilegeByID_Acc(int ControllerID)
		{
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
			}
			wgTools.WriteLine("getPrivilegeByID Start");
			this.dtPrivilege.Rows.Clear();
			int num = int.Parse("0" + wgAppConfig.getSystemParamByNO(208));
			string text = string.Format(" SELECT a.f_ConsumerID, COUNT(a.f_FloorID) as cnt FROM t_b_UserFloor a\r\nWHERE a.f_FloorID IN \r\n(SELECT b.f_floorid from t_b_floor b where b.[f_ControllerID]={0} or b.[f_ControllerID] in \r\n (select c.f_ControllerID from t_b_ElevatorGroup c where c.f_ElevatorGroupNO in \r\n   (select d.f_ElevatorGroupNO from t_b_ElevatorGroup d where d.f_ControllerID = {0})))\r\nGROUP BY a.f_ConsumerID ", ControllerID);
			this.dtUserFloorCnt = new DataTable();
			this.dvUserFloorCnt = new DataView(this.dtUserFloorCnt);
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
					{
						oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						oleDbDataAdapter.Fill(this.dtUserFloorCnt);
					}
				}
			}
			text = string.Format("SELECT b.f_CardNO, b.f_ConsumerID, b.f_BeginYMD, b.f_EndYMD, b.f_PIN, a.f_ControlSegID, b.f_DoorEnabled, c.f_floorNO, b.f_ConsumerName \r\nFROM (([t_b_UserFloor] a\r\nINNER JOIN t_b_Consumer b ON ( a.f_ConsumerID = b.f_ConsumerID AND b.f_CardNO IS NOT NULL ))\r\nINNER JOIN t_b_Floor c ON a.f_FloorID = c.f_FloorID )\r\nWHERE f_ControllerID = {0}\r\nORDER BY f_CardNO", ControllerID);
			this.dtUserFloor = new DataTable();
			using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2))
				{
					using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(oleDbCommand2))
					{
						oleDbCommand2.CommandTimeout = wgAppConfig.dbCommandTimeout;
						oleDbDataAdapter2.Fill(this.dtUserFloor);
					}
				}
			}
			DataRow dataRow = this.dtPrivilege.NewRow();
			dataRow["f_CardNO"] = 0;
			dataRow["f_ControlSegID1"] = 0;
			dataRow["f_ControlSegID2"] = 0;
			dataRow["f_ControlSegID3"] = 0;
			dataRow["f_ControlSegID4"] = 0;
			dataRow["f_MoreCards_GrpID_3"] = 0;
			dataRow["f_MoreCards_GrpID_4"] = 0;
			dataRow["f_MoreCards_GrpID_2"] = 0;
			dataRow["f_DoorFirstCard_2"] = 0;
			dataRow["f_DoorFirstCard_3"] = 0;
			dataRow["f_DoorFirstCard_4"] = 0;
			dataRow["f_PIN"] = 0;
			int num2 = 0;
			for (int i = 0; i < this.dtUserFloor.Rows.Count; i++)
			{
				if (wgMjControllerPrivilege.bStopUploadPrivilege)
				{
					return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
				}
				DataRow dataRow2 = this.dtUserFloor.Rows[i];
				if (long.Parse(dataRow2["f_CardNO"].ToString()) >= 0L && long.Parse(dataRow2["f_CardNO"].ToString()) <= icPrivilege.CARDNOMAX && int.Parse(wgTools.SetObjToStr(dataRow2["f_DoorEnabled"])) == 1)
				{
					if ((long)dataRow["f_CardNO"] != long.Parse(dataRow2["f_CardNO"].ToString()))
					{
						if ((long)dataRow["f_CardNO"] > 0L)
						{
							this.dtPrivilege.Rows.Add(dataRow);
							dataRow = this.dtPrivilege.NewRow();
							dataRow["f_ControlSegID1"] = 0;
							dataRow["f_ControlSegID2"] = 0;
							dataRow["f_ControlSegID3"] = 0;
							dataRow["f_ControlSegID4"] = 0;
							dataRow["f_MoreCards_GrpID_3"] = 0;
							dataRow["f_MoreCards_GrpID_4"] = 0;
							dataRow["f_MoreCards_GrpID_2"] = 0;
							dataRow["f_DoorFirstCard_2"] = 0;
							dataRow["f_DoorFirstCard_3"] = 0;
							dataRow["f_DoorFirstCard_4"] = 0;
							dataRow["f_PIN"] = 0;
						}
						dataRow["f_CardNO"] = long.Parse(dataRow2["f_CardNO"].ToString());
						dataRow["f_ConsumerID"] = (uint)((int)dataRow2["f_ConsumerID"]);
						dataRow["f_BeginYMD"] = dataRow2["f_BeginYMD"];
						dataRow["f_EndYMD"] = dataRow2["f_EndYMD"];
						dataRow["f_PIN"] = dataRow2["f_PIN"];
						this.dvUserFloorCnt.RowFilter = "f_ConsumerID = " + (uint)((int)dataRow2["f_ConsumerID"]);
						if (this.dvUserFloorCnt.Count >= 1 && (int)this.dvUserFloorCnt[0]["cnt"] >= 2)
						{
							dataRow["f_AllowFloors"] = (ulong)dataRow["f_AllowFloors"] | 1099511627776UL;
						}
						dataRow["f_ControlSegID1"] = dataRow2["f_ControlSegID"];
						dataRow["f_ConsumerName"] = dataRow2["f_ConsumerName"];
					}
					int num3 = int.Parse(dataRow2["f_floorNO"].ToString());
					if (num3 > 0 && num3 <= 40)
					{
						dataRow["f_AllowFloors"] = (ulong)dataRow["f_AllowFloors"] | (1UL << num3 - 1);
					}
					if (num > 0 && (ulong)dataRow["f_AllowFloors"] > 0UL)
					{
						dataRow["f_AllowFloors"] = (ulong)dataRow["f_AllowFloors"] | 1099511627776UL;
					}
					num2++;
				}
			}
			if ((long)dataRow["f_CardNO"] > 0L)
			{
				this.dtPrivilege.Rows.Add(dataRow);
			}
			this.dtPrivilege.AcceptChanges();
			this.m_PrivilegeTotal = num2;
			this.m_ValidPrivilegeTotal = num2;
			this.m_ConsumersTotal = this.dtPrivilege.Rows.Count;
			wgTools.WriteLine("getElevatorPrivilegeByID End");
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
			}
			return 1;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x000D1134 File Offset: 0x000D0134
		public void getPrivilegeByDoorName(string DoorName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.getPrivilegeByDoorName_Acc(DoorName);
				return;
			}
			string text = " SELECT f_ControllerID ";
			text = text + " FROM t_b_Door WHERE f_DoorName =  " + wgTools.PrepareStrNUnicode(DoorName);
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					int num = int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar()));
					if (num > 0)
					{
						this.getPrivilegeByID(num);
					}
				}
			}
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x000D11E0 File Offset: 0x000D01E0
		public void getPrivilegeByDoorName_Acc(string DoorName)
		{
			string text = " SELECT f_ControllerID ";
			text = text + " FROM t_b_Door WHERE f_DoorName =  " + wgTools.PrepareStrNUnicode(DoorName);
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					int num = int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()));
					if (num > 0)
					{
						this.getPrivilegeByID(num);
					}
				}
			}
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x000D127C File Offset: 0x000D027C
		public int getPrivilegeByID(int ControllerID)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getPrivilegeByID_Acc(ControllerID);
			}
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
			}
			if (wgMjController.IsElevator(this.getControllerSNByID(ControllerID)))
			{
				return this.getElevatorPrivilegeByID(ControllerID);
			}
			wgTools.WriteLine("getPrivilegeByID Start");
			this.dtPrivilege.Rows.Clear();
			string text = " SELECT t_b_Consumer.f_CardNO, t_b_Consumer.f_ConsumerID, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID,t_b_Consumer.f_DoorEnabled ";
			text = text + " , t_b_Consumer.f_ConsumerName  FROM t_d_Privilege LEFT OUTER JOIN t_b_Consumer ON  t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID WHERE  f_ControllerID =  " + ControllerID.ToString() + " ORDER BY f_CardNO ";
			if (this.bAllowUploadMobile)
			{
				text = " SELECT t_b_Consumer.f_CardNO, t_b_Consumer.f_ConsumerID, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID,t_b_Consumer.f_DoorEnabled ";
				text = text + " , t_b_Consumer.f_ConsumerName  , t_b_Consumer_Other.f_Mobile  FROM t_d_Privilege LEFT OUTER JOIN t_b_Consumer ON  t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_d_Privilege.f_ConsumerID  WHERE  f_ControllerID =  " + ControllerID.ToString() + " ORDER BY f_CardNO ";
			}
			else if (this.bAllowUploadTwoCardCheck)
			{
				text = " SELECT t_b_Consumer.f_CardNO, t_b_Consumer.f_ConsumerID, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID,t_b_Consumer.f_DoorEnabled ";
				text = text + " , t_b_Consumer.f_ConsumerName  , t_b_Consumer_Other.f_Telephone  FROM t_d_Privilege LEFT OUTER JOIN t_b_Consumer ON  t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_d_Privilege.f_ConsumerID  WHERE  f_ControllerID =  " + ControllerID.ToString() + " ORDER BY f_CardNO ";
			}
			DataRow dataRow = this.dtPrivilege.NewRow();
			dataRow["f_CardNO"] = 0;
			int num = 0;
			int num2 = 0;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (DataView dataView = new DataView(this.dtPrivilege))
					{
						sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							if (wgMjControllerPrivilege.bStopUploadPrivilege)
							{
								return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
							}
							num++;
							if (!(sqlDataReader["f_CardNO"] is DBNull) && (long)sqlDataReader["f_CardNO"] >= 0L && (long)sqlDataReader["f_CardNO"] <= icPrivilege.CARDNOMAX && !(sqlDataReader["f_DoorEnabled"] is DBNull) && int.Parse(wgTools.SetObjToStr(sqlDataReader["f_DoorEnabled"])) == 1)
							{
								num2++;
								if (long.Parse(dataRow["f_CardNO"].ToString()) != long.Parse(sqlDataReader["f_CardNO"].ToString()))
								{
									if (long.Parse(dataRow["f_CardNO"].ToString()) > 0L)
									{
										this.dtPrivilege.Rows.Add(dataRow);
										dataRow = this.dtPrivilege.NewRow();
									}
									dataRow["f_CardNO"] = long.Parse(sqlDataReader["f_CardNO"].ToString());
									dataRow["f_ConsumerID"] = (uint)((int)sqlDataReader["f_ConsumerID"]);
									dataRow["f_BeginYMD"] = sqlDataReader["f_BeginYMD"];
									dataRow["f_EndYMD"] = sqlDataReader["f_EndYMD"];
									dataRow["f_PIN"] = sqlDataReader["f_PIN"];
									dataRow["f_ConsumerName"] = sqlDataReader["f_ConsumerName"];
									if (this.bAllowUploadMobile)
									{
										if (!string.IsNullOrEmpty(wgTools.SetObjToStr(sqlDataReader["f_Mobile"])))
										{
											dataRow["f_ConsumerName"] = wgTools.SetObjToStr(sqlDataReader["f_ConsumerName"]) + this.MobileSperate + wgTools.SetObjToStr(sqlDataReader["f_Mobile"]);
										}
									}
									else if (this.bAllowUploadTwoCardCheck)
									{
										dataRow["f_ConsumerName"] = wgTools.SetObjToStr(sqlDataReader["f_Telephone"]);
									}
								}
								switch (int.Parse(sqlDataReader["f_DoorNO"].ToString()))
								{
								case 1:
									dataRow["f_ControlSegID1"] = sqlDataReader["f_ControlSegID"];
									break;
								case 2:
									dataRow["f_ControlSegID2"] = sqlDataReader["f_ControlSegID"];
									break;
								case 3:
									dataRow["f_ControlSegID3"] = sqlDataReader["f_ControlSegID"];
									break;
								case 4:
									dataRow["f_ControlSegID4"] = sqlDataReader["f_ControlSegID"];
									break;
								}
							}
						}
						if (long.Parse(dataRow["f_CardNO"].ToString()) > 0L)
						{
							this.dtPrivilege.Rows.Add(dataRow);
						}
						sqlDataReader.Close();
						this.dtPrivilege.AcceptChanges();
						this.m_PrivilegeTotal = num;
						this.m_ValidPrivilegeTotal = num;
						this.m_ConsumersTotal = this.dtPrivilege.Rows.Count;
						text = " SELECT a.f_ConsumerID, b.f_DoorNO from t_d_doorFirstCardUsers a inner join t_b_door b on a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString() + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
						sqlCommand.CommandText = text;
						sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							if (wgMjControllerPrivilege.bStopUploadPrivilege)
							{
								return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
							}
							dataView.RowFilter = "f_ConsumerID = " + sqlDataReader["f_ConsumerID"].ToString();
							if (dataView.Count == 1)
							{
								switch ((byte)sqlDataReader["f_DoorNO"])
								{
								case 1:
									dataView[0]["f_DoorFirstCard_1"] = 1;
									break;
								case 2:
									dataView[0]["f_DoorFirstCard_2"] = 1;
									break;
								case 3:
									dataView[0]["f_DoorFirstCard_3"] = 1;
									break;
								case 4:
									dataView[0]["f_DoorFirstCard_4"] = 1;
									break;
								}
							}
						}
						sqlDataReader.Close();
						text = " SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from t_d_doorMoreCardsUsers a inner join t_b_door b on a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString() + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
						sqlCommand.CommandText = text;
						sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							if (wgMjControllerPrivilege.bStopUploadPrivilege)
							{
								return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
							}
							dataView.RowFilter = "f_ConsumerID = " + sqlDataReader["f_ConsumerID"].ToString();
							if (dataView.Count == 1)
							{
								switch ((byte)sqlDataReader["f_DoorNO"])
								{
								case 1:
									dataView[0]["f_MoreCards_GrpID_1"] = sqlDataReader["f_MoreCards_GrpID"];
									break;
								case 2:
									dataView[0]["f_MoreCards_GrpID_2"] = sqlDataReader["f_MoreCards_GrpID"];
									break;
								case 3:
									dataView[0]["f_MoreCards_GrpID_3"] = sqlDataReader["f_MoreCards_GrpID"];
									break;
								case 4:
									dataView[0]["f_MoreCards_GrpID_4"] = sqlDataReader["f_MoreCards_GrpID"];
									break;
								}
							}
						}
						sqlDataReader.Close();
						this.dtPrivilege.AcceptChanges();
						if (!this.bAllowUploadMobile && this.bAllowUploadTwoCardCheck && wgMjController.GetControllerType(this.getControllerSNByID(ControllerID)) < 4)
						{
							for (int i = 0; i < this.dtPrivilege.Rows.Count; i++)
							{
								long num3 = -1L;
								long.TryParse("0" + wgTools.SetObjToStr(this.dtPrivilege.Rows[i]["f_ConsumerName"]), out num3);
								if (num3 >= 0L)
								{
									this.dtPrivilege.Rows[i]["f_MoreCards_GrpID_1"] = (byte)(num3 & 15L);
									this.dtPrivilege.Rows[i]["f_MoreCards_GrpID_2"] = (byte)((num3 >> 4) & 15L);
									this.dtPrivilege.Rows[i]["f_MoreCards_GrpID_3"] = (byte)((num3 >> 8) & 15L);
									this.dtPrivilege.Rows[i]["f_MoreCards_GrpID_4"] = (byte)((num3 >> 12) & 15L);
									this.dtPrivilege.Rows[i]["f_ControlSegID3"] = (byte)((num3 >> 16) & 255L);
									this.dtPrivilege.Rows[i]["f_ControlSegID4"] = (byte)((num3 >> 24) & 255L);
								}
							}
						}
						this.dtPrivilege.AcceptChanges();
					}
				}
			}
			wgTools.WriteLine("getPrivilegeByID End");
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
			}
			return 1;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x000D1B40 File Offset: 0x000D0B40
		public int getPrivilegeByID_Acc(int ControllerID)
		{
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
			}
			if (wgMjController.IsElevator(this.getControllerSNByID(ControllerID)))
			{
				return this.getElevatorPrivilegeByID(ControllerID);
			}
			wgTools.WriteLine("getPrivilegeByID Start");
			this.dtPrivilege.Rows.Clear();
			string text = " SELECT t_b_Consumer.f_CardNO, t_b_Consumer.f_ConsumerID, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID,t_b_Consumer.f_DoorEnabled ";
			text = text + " , t_b_Consumer.f_ConsumerName  FROM t_d_Privilege LEFT OUTER JOIN t_b_Consumer ON  t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID WHERE  f_ControllerID =  " + ControllerID.ToString() + " ORDER BY f_CardNO ";
			if (this.bAllowUploadMobile)
			{
				text = " SELECT t_b_Consumer.f_CardNO, t_b_Consumer.f_ConsumerID, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID,t_b_Consumer.f_DoorEnabled ";
				text = text + " , t_b_Consumer.f_ConsumerName  , t_b_Consumer_Other.f_Mobile  FROM (t_d_Privilege LEFT OUTER JOIN t_b_Consumer ON  t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_d_Privilege.f_ConsumerID  WHERE  f_ControllerID =  " + ControllerID.ToString() + " ORDER BY f_CardNO ";
			}
			else if (this.bAllowUploadTwoCardCheck)
			{
				text = " SELECT t_b_Consumer.f_CardNO, t_b_Consumer.f_ConsumerID, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID,t_b_Consumer.f_DoorEnabled ";
				text = text + " , t_b_Consumer.f_ConsumerName  , t_b_Consumer_Other.f_Telephone  FROM (t_d_Privilege LEFT OUTER JOIN t_b_Consumer ON  t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_d_Privilege.f_ConsumerID  WHERE  f_ControllerID =  " + ControllerID.ToString() + " ORDER BY f_CardNO ";
			}
			DataRow dataRow = this.dtPrivilege.NewRow();
			dataRow["f_CardNO"] = 0;
			int num = 0;
			int num2 = 0;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					using (DataView dataView = new DataView(this.dtPrivilege))
					{
						oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							if (wgMjControllerPrivilege.bStopUploadPrivilege)
							{
								return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
							}
							num++;
							if (!(oleDbDataReader["f_CardNO"] is DBNull) && long.Parse(oleDbDataReader["f_CardNO"].ToString()) >= 0L && long.Parse(oleDbDataReader["f_CardNO"].ToString()) <= icPrivilege.CARDNOMAX && !(oleDbDataReader["f_DoorEnabled"] is DBNull) && int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_DoorEnabled"])) == 1)
							{
								num2++;
								if (long.Parse(dataRow["f_CardNO"].ToString()) != long.Parse(oleDbDataReader["f_CardNO"].ToString()))
								{
									if (long.Parse(dataRow["f_CardNO"].ToString()) > 0L)
									{
										this.dtPrivilege.Rows.Add(dataRow);
										dataRow = this.dtPrivilege.NewRow();
									}
									dataRow["f_CardNO"] = long.Parse(oleDbDataReader["f_CardNO"].ToString());
									dataRow["f_ConsumerID"] = (uint)((int)oleDbDataReader["f_ConsumerID"]);
									dataRow["f_BeginYMD"] = oleDbDataReader["f_BeginYMD"];
									dataRow["f_EndYMD"] = oleDbDataReader["f_EndYMD"];
									dataRow["f_PIN"] = oleDbDataReader["f_PIN"];
									dataRow["f_ConsumerName"] = oleDbDataReader["f_ConsumerName"];
									if (this.bAllowUploadMobile)
									{
										if (!string.IsNullOrEmpty(wgTools.SetObjToStr(oleDbDataReader["f_Mobile"])))
										{
											dataRow["f_ConsumerName"] = wgTools.SetObjToStr(oleDbDataReader["f_ConsumerName"]) + this.MobileSperate + wgTools.SetObjToStr(oleDbDataReader["f_Mobile"]);
										}
									}
									else if (this.bAllowUploadTwoCardCheck)
									{
										dataRow["f_ConsumerName"] = wgTools.SetObjToStr(oleDbDataReader["f_Telephone"]);
									}
								}
								switch (int.Parse(oleDbDataReader["f_DoorNO"].ToString()))
								{
								case 1:
									dataRow["f_ControlSegID1"] = oleDbDataReader["f_ControlSegID"];
									break;
								case 2:
									dataRow["f_ControlSegID2"] = oleDbDataReader["f_ControlSegID"];
									break;
								case 3:
									dataRow["f_ControlSegID3"] = oleDbDataReader["f_ControlSegID"];
									break;
								case 4:
									dataRow["f_ControlSegID4"] = oleDbDataReader["f_ControlSegID"];
									break;
								}
							}
						}
						if (long.Parse(dataRow["f_CardNO"].ToString()) > 0L)
						{
							this.dtPrivilege.Rows.Add(dataRow);
						}
						oleDbDataReader.Close();
						this.dtPrivilege.AcceptChanges();
						this.m_PrivilegeTotal = num;
						this.m_ValidPrivilegeTotal = num;
						this.m_ConsumersTotal = this.dtPrivilege.Rows.Count;
						text = string.Format(" SELECT a.f_ConsumerID, b.f_DoorNO from ( t_d_doorFirstCardUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID = {0} )) ", ControllerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
						oleDbCommand.CommandText = text;
						oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							if (wgMjControllerPrivilege.bStopUploadPrivilege)
							{
								return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
							}
							dataView.RowFilter = "f_ConsumerID = " + oleDbDataReader["f_ConsumerID"].ToString();
							if (dataView.Count == 1)
							{
								switch ((byte)oleDbDataReader["f_DoorNO"])
								{
								case 1:
									dataView[0]["f_DoorFirstCard_1"] = 1;
									break;
								case 2:
									dataView[0]["f_DoorFirstCard_2"] = 1;
									break;
								case 3:
									dataView[0]["f_DoorFirstCard_3"] = 1;
									break;
								case 4:
									dataView[0]["f_DoorFirstCard_4"] = 1;
									break;
								}
							}
						}
						oleDbDataReader.Close();
						text = string.Format(" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from ( t_d_doorMoreCardsUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID =  {0} ))", ControllerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
						oleDbCommand.CommandText = text;
						oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							if (wgMjControllerPrivilege.bStopUploadPrivilege)
							{
								return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
							}
							dataView.RowFilter = "f_ConsumerID = " + oleDbDataReader["f_ConsumerID"].ToString();
							if (dataView.Count == 1)
							{
								switch ((byte)oleDbDataReader["f_DoorNO"])
								{
								case 1:
									dataView[0]["f_MoreCards_GrpID_1"] = oleDbDataReader["f_MoreCards_GrpID"];
									break;
								case 2:
									dataView[0]["f_MoreCards_GrpID_2"] = oleDbDataReader["f_MoreCards_GrpID"];
									break;
								case 3:
									dataView[0]["f_MoreCards_GrpID_3"] = oleDbDataReader["f_MoreCards_GrpID"];
									break;
								case 4:
									dataView[0]["f_MoreCards_GrpID_4"] = oleDbDataReader["f_MoreCards_GrpID"];
									break;
								}
							}
						}
						oleDbDataReader.Close();
						this.dtPrivilege.AcceptChanges();
						if (!this.bAllowUploadMobile && this.bAllowUploadTwoCardCheck && wgMjController.GetControllerType(this.getControllerSNByID(ControllerID)) < 4)
						{
							for (int i = 0; i < this.dtPrivilege.Rows.Count; i++)
							{
								long num3 = -1L;
								long.TryParse("0" + wgTools.SetObjToStr(this.dtPrivilege.Rows[i]["f_ConsumerName"]), out num3);
								if (num3 >= 0L)
								{
									this.dtPrivilege.Rows[i]["f_MoreCards_GrpID_1"] = (byte)(num3 & 15L);
									this.dtPrivilege.Rows[i]["f_MoreCards_GrpID_2"] = (byte)((num3 >> 4) & 15L);
									this.dtPrivilege.Rows[i]["f_MoreCards_GrpID_3"] = (byte)((num3 >> 8) & 15L);
									this.dtPrivilege.Rows[i]["f_MoreCards_GrpID_4"] = (byte)((num3 >> 12) & 15L);
									this.dtPrivilege.Rows[i]["f_ControlSegID3"] = (byte)((num3 >> 16) & 255L);
									this.dtPrivilege.Rows[i]["f_ControlSegID4"] = (byte)((num3 >> 24) & 255L);
								}
							}
						}
						this.dtPrivilege.AcceptChanges();
					}
				}
			}
			wgTools.WriteLine("getPrivilegeByID End");
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
			}
			return 1;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000D2408 File Offset: 0x000D1408
		public void getPrivilegeBySN(int ControllerSN)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.getPrivilegeBySN_Acc(ControllerSN);
				return;
			}
			string text = " SELECT f_ControllerID ";
			text = text + " FROM t_b_Controller WHERE f_ControllerSN =  " + ControllerSN.ToString();
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					int num = int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar()));
					if (num > 0)
					{
						this.getPrivilegeByID(num);
					}
				}
			}
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x000D24B4 File Offset: 0x000D14B4
		public void getPrivilegeBySN_Acc(int ControllerSN)
		{
			string text = " SELECT f_ControllerID ";
			text = text + " FROM t_b_Controller WHERE f_ControllerSN =  " + ControllerSN.ToString();
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					int num = int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()));
					if (num > 0)
					{
						this.getPrivilegeByID(num);
					}
				}
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x000D2550 File Offset: 0x000D1550
		public static int getPrivilegeNumInDBByID(int ControllerID)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return icPrivilege.getPrivilegeNumInDBByID_Acc(ControllerID);
			}
			int num = 0;
			string text = " SELECT COUNT(DISTINCT t_b_Consumer.f_ConsumerID) ";
			text = text + " FROM t_b_Consumer ,t_d_Privilege  WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL  AND f_ControllerID =  " + ControllerID.ToString() + " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						num = int.Parse(sqlDataReader[0].ToString());
					}
					sqlDataReader.Close();
				}
			}
			return num;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x000D2604 File Offset: 0x000D1604
		public static int getPrivilegeNumInDBByID_Acc(int ControllerID)
		{
			int num = 0;
			string text = " SELECT COUNT(DISTINCT t_b_Consumer.f_ConsumerID) ";
			text = text + " FROM t_b_Consumer ,t_d_Privilege  WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL  AND f_ControllerID =  " + ControllerID.ToString() + " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					if (oleDbDataReader.Read())
					{
						num = int.Parse(oleDbDataReader[0].ToString());
					}
					oleDbDataReader.Close();
				}
			}
			return num;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x000D26AC File Offset: 0x000D16AC
		public MjRegisterCard GetPrivilegeOfOneCardByDB(int ControllerID, int ConsumerID, ref long iCardID, ref int sn, ref string ip, ref int port)
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
			string text = " SELECT f_CardNO, f_ConsumerName ";
			text = text + " FROM t_b_Consumer WHERE f_ConsumerID =  " + ConsumerID.ToString();
			if (this.bAllowUploadMobile)
			{
				text = " SELECT f_CardNO, f_ConsumerName  ";
				text = text + " , t_b_Consumer_Other.f_Mobile  FROM t_b_Consumer, t_b_Consumer_Other WHERE t_b_Consumer.f_ConsumerID =  " + ConsumerID.ToString() + " AND t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID ";
			}
			else if (this.bAllowUploadTwoCardCheck)
			{
				text = " SELECT f_CardNO, f_ConsumerName  ";
				text = text + " , t_b_Consumer_Other.f_Telephone  FROM t_b_Consumer, t_b_Consumer_Other WHERE t_b_Consumer.f_ConsumerID =  " + ConsumerID.ToString() + " AND t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID ";
			}
			MjRegisterCard mjRegisterCard = new MjRegisterCard();
			long num = -1L;
			dbConnection.Open();
			dbCommand.CommandText = text;
			DbDataReader dbDataReader = dbCommand.ExecuteReader();
			if (dbDataReader.Read())
			{
				long.TryParse(dbDataReader["f_CardNO"].ToString(), out iCardID);
			}
			byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(wgTools.SetObjToStr(dbDataReader["f_ConsumerName"]));
			mjRegisterCard.userName = bytes;
			if (this.bAllowUploadMobile)
			{
				if (!string.IsNullOrEmpty(wgTools.SetObjToStr(dbDataReader["f_Mobile"])))
				{
					byte[] bytes2 = Encoding.GetEncoding("utf-8").GetBytes(wgTools.SetObjToStr(dbDataReader["f_ConsumerName"]) + this.MobileSperate + wgTools.SetObjToStr(dbDataReader["f_Mobile"]));
					mjRegisterCard.userName = bytes2;
				}
			}
			else if (this.bAllowUploadTwoCardCheck)
			{
				long.TryParse("0" + wgTools.SetObjToStr(dbDataReader["f_Telephone"]), out num);
			}
			dbDataReader.Close();
			if (iCardID <= 0L)
			{
				dbConnection.Close();
				dbConnection.Dispose();
				return null;
			}
			if (iCardID > icPrivilege.CARDNOMAX)
			{
				dbConnection.Close();
				dbConnection.Dispose();
				return null;
			}
			text = " SELECT t_b_Consumer.f_CardNO, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID ";
			text = string.Concat(new string[]
			{
				text,
				" , t_b_Consumer.f_ConsumerName  FROM t_b_Consumer ,t_d_Privilege  WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL  AND f_ControllerID =  ",
				ControllerID.ToString(),
				" AND f_CardNO =  ",
				iCardID.ToString(),
				" AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID "
			});
			dbCommand.CommandText = text;
			dbDataReader = dbCommand.ExecuteReader();
			while (dbDataReader.Read())
			{
				mjRegisterCard.CardID = long.Parse(dbDataReader["f_CardNO"].ToString());
				mjRegisterCard.Password = uint.Parse(dbDataReader["f_PIN"].ToString());
				mjRegisterCard.ymdStart = (DateTime)dbDataReader["f_BeginYMD"];
				mjRegisterCard.ymdEnd = (DateTime)dbDataReader["f_EndYMD"];
				switch (int.Parse(dbDataReader["f_DoorNO"].ToString()))
				{
				case 1:
					mjRegisterCard.ControlSegIndexSet(1, (byte)((int)dbDataReader["f_ControlSegID"] & 255));
					break;
				case 2:
					mjRegisterCard.ControlSegIndexSet(2, (byte)((int)dbDataReader["f_ControlSegID"] & 255));
					break;
				case 3:
					mjRegisterCard.ControlSegIndexSet(3, (byte)((int)dbDataReader["f_ControlSegID"] & 255));
					break;
				case 4:
					mjRegisterCard.ControlSegIndexSet(4, (byte)((int)dbDataReader["f_ControlSegID"] & 255));
					break;
				}
			}
			dbDataReader.Close();
			text = string.Format(" SELECT a.f_ConsumerID, b.f_DoorNO from (t_d_doorFirstCardUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID = {0} )) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString() + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
			dbCommand.CommandText = text;
			dbDataReader = dbCommand.ExecuteReader();
			while (dbDataReader.Read())
			{
				switch ((byte)dbDataReader["f_DoorNO"])
				{
				case 1:
					mjRegisterCard.FirstCardSet(1, true);
					break;
				case 2:
					mjRegisterCard.FirstCardSet(2, true);
					break;
				case 3:
					mjRegisterCard.FirstCardSet(3, true);
					break;
				case 4:
					mjRegisterCard.FirstCardSet(4, true);
					break;
				}
			}
			dbDataReader.Close();
			text = string.Format(" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from ( t_d_doorMoreCardsUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID = {0})) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString() + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
			dbCommand.CommandText = text;
			dbDataReader = dbCommand.ExecuteReader();
			while (dbDataReader.Read())
			{
				switch ((byte)dbDataReader["f_DoorNO"])
				{
				case 1:
					mjRegisterCard.MoreCardGroupIndexSet(1, (byte)((int)dbDataReader["f_MoreCards_GrpID"]));
					break;
				case 2:
					mjRegisterCard.MoreCardGroupIndexSet(2, (byte)((int)dbDataReader["f_MoreCards_GrpID"]));
					break;
				case 3:
					mjRegisterCard.MoreCardGroupIndexSet(3, (byte)((int)dbDataReader["f_MoreCards_GrpID"]));
					break;
				case 4:
					mjRegisterCard.MoreCardGroupIndexSet(4, (byte)((int)dbDataReader["f_MoreCards_GrpID"]));
					break;
				}
			}
			dbDataReader.Close();
			if (num >= 0L && wgMjController.GetControllerType(this.getControllerSNByID(ControllerID)) < 4)
			{
				mjRegisterCard.MoreCardGroupIndexSet(1, (byte)(num & 15L));
				mjRegisterCard.MoreCardGroupIndexSet(2, (byte)((num >> 4) & 15L));
				mjRegisterCard.MoreCardGroupIndexSet(3, (byte)((num >> 8) & 15L));
				mjRegisterCard.MoreCardGroupIndexSet(4, (byte)((num >> 12) & 15L));
				mjRegisterCard.ControlSegIndexSet(3, (byte)((num >> 16) & 255L));
				mjRegisterCard.ControlSegIndexSet(4, (byte)((num >> 24) & 255L));
			}
			text = " SELECT * ";
			text = text + " FROM t_b_Controller WHERE f_ControllerID =  " + ControllerID.ToString();
			dbCommand.CommandText = text;
			dbDataReader = dbCommand.ExecuteReader();
			if (dbDataReader.Read())
			{
				sn = (int)dbDataReader["f_ControllerSN"];
				ip = wgTools.SetObjToStr(dbDataReader["f_IP"]);
				port = (int)dbDataReader["f_PORT"];
			}
			dbDataReader.Close();
			dbConnection.Close();
			dbConnection.Dispose();
			return mjRegisterCard;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000D2CA1 File Offset: 0x000D1CA1
		public int upload(int ControllerSN, string DoorName)
		{
			return base.UploadIP(ControllerSN, "", 60000, DoorName, this.dtPrivilege, -1);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000D2CBC File Offset: 0x000D1CBC
		public int upload(int ControllerSN, string IP, string DoorName)
		{
			return base.UploadIP(ControllerSN, IP, 60000, DoorName, this.dtPrivilege, -1);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x000D2CD3 File Offset: 0x000D1CD3
		public int upload(int ControllerSN, string IP, int Port, string DoorName, int UDPQueue4MultithreadIndex = -1)
		{
			return base.UploadIP(ControllerSN, IP, Port, DoorName, this.dtPrivilege, UDPQueue4MultithreadIndex);
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x000D2CE8 File Offset: 0x000D1CE8
		// (set) Token: 0x06000766 RID: 1894 RVA: 0x000D2CF0 File Offset: 0x000D1CF0
		public bool bDisplayProcess
		{
			get
			{
				return this._bDisplayProcess;
			}
			set
			{
				this._bDisplayProcess = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x000D2CF9 File Offset: 0x000D1CF9
		public int ConsumersTotal
		{
			get
			{
				return this.m_ConsumersTotal;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x000D2D01 File Offset: 0x000D1D01
		public int PrivilegTotal
		{
			get
			{
				return this.m_PrivilegeTotal;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x000D2D09 File Offset: 0x000D1D09
		public int ValidPrivilege
		{
			get
			{
				return this.m_ValidPrivilegeTotal;
			}
		}

		// Token: 0x04000D2A RID: 3370
		private bool _bDisplayProcess;

		// Token: 0x04000D2B RID: 3371
		private bool bAllowUploadMobile;

		// Token: 0x04000D2C RID: 3372
		public bool bAllowUploadTwoCardCheck = true;

		// Token: 0x04000D2D RID: 3373
		private static long CARDNOMAX = (long)((ulong)(-2));

		// Token: 0x04000D2E RID: 3374
		private DataColumn dc;

		// Token: 0x04000D2F RID: 3375
		private DataTable dtPrivilege;

		// Token: 0x04000D30 RID: 3376
		private DataTable dtUserFloor;

		// Token: 0x04000D31 RID: 3377
		private DataTable dtUserFloorCnt;

		// Token: 0x04000D32 RID: 3378
		private DataView dvUserFloorCnt;

		// Token: 0x04000D33 RID: 3379
		private int m_ConsumersTotal;

		// Token: 0x04000D34 RID: 3380
		private int m_PrivilegeTotal;

		// Token: 0x04000D35 RID: 3381
		private int m_ValidPrivilegeTotal;

		// Token: 0x04000D36 RID: 3382
		public string MobileSperate = ",";
	}
}
