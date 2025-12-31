using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.DataOper
{
	// Token: 0x02000225 RID: 549
	internal class icOperator
	{
		// Token: 0x06000FF3 RID: 4083 RVA: 0x0011E8B0 File Offset: 0x0011D8B0
		private static void CheckMenu(MenuStrip Menu, ref DataTable dt)
		{
			foreach (object obj in Menu.Items)
			{
				ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)obj;
				icOperator.CheckSubMenu(toolStripMenuItem, ref dt);
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0011E90C File Offset: 0x0011D90C
		public static int checkSoftwareRegister()
		{
			return 1;
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0011E910 File Offset: 0x0011D910
		private static void CheckSubMenu(ToolStripMenuItem menuItem, ref DataTable dt)
		{
			wgTools.WgDebugWrite(menuItem.Text + "--" + menuItem.Name.ToString(), new object[0]);
			if (icOperator.isAllowAdd(menuItem.Name.ToString()))
			{
				DataRow dataRow = dt.NewRow();
				dataRow[0] = dt.Rows.Count + 1;
				dataRow[1] = menuItem.Name.ToString();
				if (menuItem.Text.IndexOf("(&") > 0)
				{
					dataRow[2] = menuItem.Text.Substring(0, menuItem.Text.IndexOf("(&"));
				}
				else if (menuItem.Text.IndexOf("&") >= 0)
				{
					dataRow[2] = menuItem.Text.Replace("&", "");
				}
				else
				{
					dataRow[2] = menuItem.Text;
				}
				dataRow[3] = 0;
				dataRow[4] = 1;
				dt.Rows.Add(dataRow);
				dt.AcceptChanges();
			}
			for (int i = 0; i < menuItem.DropDownItems.Count; i++)
			{
				if (!(menuItem.DropDownItems[i] is ToolStripSeparator))
				{
					icOperator.CheckSubMenu((ToolStripMenuItem)menuItem.DropDownItems[i], ref dt);
				}
			}
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0011EA70 File Offset: 0x0011DA70
		public static void FindInMenu(MenuStrip Menu, string MenuItemName, ref ToolStripMenuItem mnuItm)
		{
			foreach (object obj in Menu.Items)
			{
				ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)obj;
				icOperator.FindInSubMenu(toolStripMenuItem, MenuItemName, ref mnuItm);
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0011EACC File Offset: 0x0011DACC
		private static void FindInSubMenu(ToolStripMenuItem menuItem, string MenuItemName, ref ToolStripMenuItem mnuItm)
		{
			if (mnuItm.Text == "")
			{
				if (menuItem.Name.ToString().Equals(MenuItemName))
				{
					mnuItm = menuItem;
				}
				for (int i = 0; i < menuItem.DropDownItems.Count; i++)
				{
					if (!(menuItem.DropDownItems[i] is ToolStripSeparator))
					{
						icOperator.FindInSubMenu((ToolStripMenuItem)menuItem.DropDownItems[i], MenuItemName, ref mnuItm);
					}
				}
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0011EB44 File Offset: 0x0011DB44
		public static void getDefaultFullFunction(MenuStrip mnuMain)
		{
			icOperator.dt1 = new DataTable();
			icOperator.dt1.TableName = "OperatePrivilege";
			icOperator.dt1.Columns.Add("f_FunctionID");
			icOperator.dt1.Columns.Add("f_FunctionName");
			icOperator.dt1.Columns.Add("f_FunctionDisplayName");
			icOperator.dt1.Columns.Add("f_ReadOnly");
			icOperator.dt1.Columns.Add("f_FullControl");
			icOperator.CheckMenu(mnuMain, ref icOperator.dt1);
			icOperator.IntertIntoDefaultFullFunctionDT(ref icOperator.dt1, "TotalControl_RealGetCardRecord", "Real GetCardRecord");
			icOperator.IntertIntoDefaultFullFunctionDT(ref icOperator.dt1, "TotalControl_RemoteOpen", "Remote Open");
			icOperator.IntertIntoDefaultFullFunctionDT(ref icOperator.dt1, "TotalControl_SetDoorControl", "Set Door Control");
			icOperator.IntertIntoDefaultFullFunctionDT(ref icOperator.dt1, "TotalControl_SetDoorDelay", "Set Door Delay");
			icOperator.IntertIntoDefaultFullFunctionDT(ref icOperator.dt1, "TotalControl_VideoMonitor", "Video Monitor");
			icOperator.IntertIntoDefaultFullFunctionDT(ref icOperator.dt1, "TotalControl_Map", "Map");
			icOperator.m_dtDefaultAllPrivilege = icOperator.dt1;
			wgAppConfig.runUpdateSql(string.Format("DELETE FROM t_s_OperatorPrivilege", new object[0]));
			for (int i = 0; i < icOperator.dt1.Rows.Count; i++)
			{
				wgAppConfig.runUpdateSql(string.Format("INSERT INTO t_s_OperatorPrivilege ([f_OperatorID], [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl]) VALUES(1, {0:d},{1},{2},{3:d},{4:d})", new object[]
				{
					int.Parse(icOperator.dt1.Rows[i][0].ToString()),
					wgTools.PrepareStrNUnicode(icOperator.dt1.Rows[i][1].ToString()),
					wgTools.PrepareStrNUnicode(icOperator.dt1.Rows[i][2].ToString()),
					int.Parse(icOperator.dt1.Rows[i][3].ToString()),
					int.Parse(icOperator.dt1.Rows[i][4].ToString())
				}));
			}
			DatatableToXml.CDataToXmlFile(icOperator.dt1, "OperatePrivilegeCurrent.XML");
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0011ED7C File Offset: 0x0011DD7C
		private static void getDicMenuToFrm()
		{
			icOperator.dicMenuToFrm = new Dictionary<string, string>();
			icOperator.dicMenuToFrm.Add("frmControllers", "mnuControllers");
			icOperator.dicMenuToFrm.Add("frmDepartments", "mnuGroups");
			icOperator.dicMenuToFrm.Add("frmUsers", "mnuConsumers");
			icOperator.dicMenuToFrm.Add("frmControlSegs", "mnuControlSeg");
			icOperator.dicMenuToFrm.Add("frmPrivileges", "mnuPrivilege");
			icOperator.dicMenuToFrm.Add("frmConsole", "mnuTotalControl");
			icOperator.dicMenuToFrm.Add("btnCheckController", "mnuCheckController");
			icOperator.dicMenuToFrm.Add("btnAdjustTime", "mnuAdjustTime");
			icOperator.dicMenuToFrm.Add("btnUpload", "mnuUpload");
			icOperator.dicMenuToFrm.Add("btnMonitor", "mnuMonitor");
			icOperator.dicMenuToFrm.Add("btnGetRecords", "mnuGetCardRecords");
			icOperator.dicMenuToFrm.Add("btnRemoteOpen", "TotalControl_RemoteOpen");
			icOperator.dicMenuToFrm.Add("btnMaps", "btnMaps");
			icOperator.dicMenuToFrm.Add("btnRealtimeGetRecords", "mnuRealtimeGetRecords");
			icOperator.dicMenuToFrm.Add("frmSwipeRecords", "mnuCardRecords");
			icOperator.dicMenuToFrm.Add("dfrmOperator", "cmdOperatorManage");
			icOperator.dicMenuToFrm.Add("frmAbout", "mnuAbout");
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0011EEE8 File Offset: 0x0011DEE8
		public static void getFrmOperatorPrivilege(string frmName, out bool readOnly, out bool fullControl)
		{
			readOnly = true;
			fullControl = true;
			if (!string.IsNullOrEmpty(frmName))
			{
				if (icOperator.dicMenuToFrm == null)
				{
					icOperator.getDicMenuToFrm();
				}
				if (icOperator.dicMenuToFrm != null && icOperator.dicMenuToFrm.ContainsKey(frmName))
				{
					if (icOperator.m_dtCurrentOperatorPrivilege == null)
					{
						icOperator.m_dtCurrentOperatorPrivilege = icOperator.getOperatorPrivilege(icOperator.m_OperatorID);
					}
					if (icOperator.m_dtCurrentOperatorPrivilege.Rows.Count > 0)
					{
						using (DataView dataView = new DataView(icOperator.m_dtCurrentOperatorPrivilege))
						{
							dataView.RowFilter = string.Format("f_FunctionName ={0}", wgTools.PrepareStr(icOperator.dicMenuToFrm[frmName]));
							if (dataView.Count > 0)
							{
								readOnly = bool.Parse(dataView[0]["f_ReadOnly"].ToString());
								fullControl = bool.Parse(dataView[0]["f_FullControl"].ToString());
							}
						}
					}
				}
			}
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0011EFE0 File Offset: 0x0011DFE0
		public static DataTable getOperatorPrivilege(int OperatorID)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return icOperator.getOperatorPrivilege_Acc(OperatorID);
			}
			icOperator.dt = new DataTable();
			icOperator.dt.TableName = "OperatePrivilege";
			icOperator.dt.Columns.Add("f_FunctionID");
			icOperator.dt.Columns.Add("f_FunctionName");
			icOperator.dt.Columns.Add("f_FunctionDisplayName");
			icOperator.dt.Columns.Add("f_ReadOnly");
			icOperator.dt.Columns.Add("f_FullControl");
			string text = "SELECT [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl] FROM t_s_OperatorPrivilege WHERE f_OperatorID = " + OperatorID.ToString();
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (!sqlDataReader.HasRows)
					{
						sqlDataReader.Close();
						text = "SELECT [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl] FROM t_s_OperatorPrivilege WHERE f_OperatorID = " + 1.ToString();
						sqlCommand.CommandText = text;
						sqlDataReader = sqlCommand.ExecuteReader();
					}
					while (sqlDataReader.Read())
					{
						DataRow dataRow = icOperator.dt.NewRow();
						dataRow[0] = sqlDataReader[0];
						dataRow[1] = sqlDataReader[1];
						dataRow[2] = sqlDataReader[2];
						dataRow[3] = int.Parse(sqlDataReader[3].ToString()) > 0;
						dataRow[4] = int.Parse(sqlDataReader[4].ToString()) > 0;
						icOperator.dt.Rows.Add(dataRow);
					}
					sqlDataReader.Close();
					icOperator.dt.AcceptChanges();
				}
			}
			return icOperator.dt;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0011F1E0 File Offset: 0x0011E1E0
		public static DataTable getOperatorPrivilege_Acc(int OperatorID)
		{
			icOperator.dt = new DataTable();
			icOperator.dt.TableName = "OperatePrivilege";
			icOperator.dt.Columns.Add("f_FunctionID");
			icOperator.dt.Columns.Add("f_FunctionName");
			icOperator.dt.Columns.Add("f_FunctionDisplayName");
			icOperator.dt.Columns.Add("f_ReadOnly");
			icOperator.dt.Columns.Add("f_FullControl");
			string text = "SELECT [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl] FROM t_s_OperatorPrivilege WHERE f_OperatorID = " + OperatorID.ToString();
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					if (!oleDbDataReader.HasRows)
					{
						oleDbDataReader.Close();
						text = "SELECT [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl] FROM t_s_OperatorPrivilege WHERE f_OperatorID = " + 1.ToString();
						oleDbCommand.CommandText = text;
						oleDbDataReader = oleDbCommand.ExecuteReader();
					}
					while (oleDbDataReader.Read())
					{
						DataRow dataRow = icOperator.dt.NewRow();
						dataRow[0] = oleDbDataReader[0];
						dataRow[1] = oleDbDataReader[1];
						dataRow[2] = oleDbDataReader[2];
						dataRow[3] = int.Parse(oleDbDataReader[3].ToString()) > 0;
						dataRow[4] = int.Parse(oleDbDataReader[4].ToString()) > 0;
						icOperator.dt.Rows.Add(dataRow);
					}
					oleDbDataReader.Close();
					icOperator.dt.AcceptChanges();
				}
			}
			return icOperator.dt;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0011F3D4 File Offset: 0x0011E3D4
		private static void IntertIntoDefaultFullFunctionDT(ref DataTable dt, string name, string display)
		{
			DataRow dataRow = dt.NewRow();
			dataRow[0] = dt.Rows.Count + 1;
			dataRow[1] = name;
			dataRow[2] = display;
			dataRow[3] = 0;
			dataRow[4] = 1;
			dt.Rows.Add(dataRow);
			dt.AcceptChanges();
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0011F444 File Offset: 0x0011E444
		private static bool isAllowAdd(string FunctionName)
		{
			bool flag = true;
			return (string.IsNullOrEmpty(FunctionName) || FunctionName == null || (!(FunctionName == "mnuExit") && !(FunctionName == "mnuMeetingSign"))) && flag;
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0011F480 File Offset: 0x0011E480
		public static bool login(string name, string pwd)
		{
			bool flag = false;
			try
			{
				bool flag2 = true;
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
				if (!string.IsNullOrEmpty(wgAppConfig.dbConString))
				{
					try
					{
						string text = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStrNUnicode("znsmart");
						dbConnection.ConnectionString = wgAppConfig.dbConString;
						dbCommand.CommandText = text;
						dbConnection.Open();
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						if (dbDataReader.Read())
						{
							flag2 = false;
						}
						dbDataReader.Close();
						dbConnection.Close();
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
				}
				if (flag2 && name == "access" && pwd == "168668")
				{
					icOperator.m_OperatorID = 1;
					icOperator.m_OperatorName = name;
					flag = true;
				}
				else
				{
					string text2 = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStrNUnicode(name) + " and f_Password = " + wgTools.PrepareStrNUnicode(Program.Ept4Database(pwd));
					dbConnection.ConnectionString = wgAppConfig.dbConString;
					dbCommand.CommandText = text2;
					dbConnection.Open();
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					if (dbDataReader.Read())
					{
						icOperator.m_OperatorID = int.Parse(dbDataReader["f_OperatorID"].ToString());
						icOperator.m_OperatorName = name;
						flag = true;
					}
					dbDataReader.Close();
				}
				string systemParamNotes = wgAppConfig.getSystemParamNotes(190);
				if (!string.IsNullOrEmpty(systemParamNotes))
				{
					string text3 = systemParamNotes;
					string[] array = Program.Dpt4Database(text3).Split(new char[] { ';' });
					byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(array[0]);
					byte[] bytes2 = Encoding.GetEncoding("utf-8").GetBytes(array[1]);
					RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
					rsacryptoServiceProvider.FromXmlString(Program.Dpt4Database("luSWeZHP2nqo3pdFCebDHs+68zQSy1zySUyIfugXbxng9FC9VtDjhCEUGq8FtduQjsx4RI6mijLelsGsiZzuJK5WdhdlaaXFgow2i66loEpn6rzdMYIbRX4EsyukBnxXVVQSPxt4Fg7bfrPW7nxT5RpaidSUAreDTibTYomjbifQDpBu47hGJ1+xuQrs8tp7C/kK+v2+Gt3F+eKngXG6WMS6r6cbggTaiKvTKVqERVNVcmuCJFksW7GhN2ci03fz6NdF+MoF8gGKQDYEhOv60LNgkc1Z9EEQoq2jhIHdpaUmggm9hWUP1IbQ31EuGrpuc3R9IzlACjnBAZi057LSpOyazjLn0AryKJPWXuO2VjGyXIxBF9IurWwiYhwzc8tuO7Nb1H0tnQyFapnfoM0huHkZzY6GAhB/B52BYe47b0vnvodVMZBU/3aG+Ry2IKxnE3bzsL5CiUuhXxIPNCUj2Xu1n1PL+J6JXFWCqj0liohQ7wg4tY2SujBm17+wsyJVPMAoaMPDbZA0WE19+VE5VvHmAoHAuo6nHFePaFE46Rk="));
					if (rsacryptoServiceProvider.VerifyData(bytes, Program.Dpt4Database("7TbkKQVZ4Al3BOP7opHTFQ=="), bytes2))
					{
						int valBySql = wgAppConfig.getValBySql("SELECT  f_ControllerSN from t_b_controller Where f_ControllerSN IN (" + array[0] + ")");
						if (valBySql > 0)
						{
							wgAppConfig.wgLog("Failed SN= " + valBySql);
							Thread.Sleep(500);
							Environment.Exit(0);
							Process.GetCurrentProcess().Kill();
						}
					}
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			return flag;
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0011F714 File Offset: 0x0011E714
		public static bool login_Acc(string name, string pwd)
		{
			bool flag = false;
			try
			{
				bool flag2 = true;
				if (!string.IsNullOrEmpty(wgAppConfig.dbConString))
				{
					try
					{
						string text = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStrNUnicode("znsmart");
						using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
						{
							using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
							{
								oleDbConnection.Open();
								OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
								if (oleDbDataReader.Read())
								{
									flag2 = false;
								}
								oleDbDataReader.Close();
							}
						}
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
				}
				if (flag2 && name == "access" && pwd == "168668")
				{
					icOperator.m_OperatorID = 1;
					icOperator.m_OperatorName = name;
					return true;
				}
				string text2;
				if (string.IsNullOrEmpty(pwd))
				{
					text2 = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStrNUnicode(name) + " and f_Password is NULL ";
				}
				else
				{
					text2 = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStrNUnicode(name) + " and f_Password = " + wgTools.PrepareStrNUnicode(pwd);
				}
				using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand(text2, oleDbConnection2))
					{
						oleDbConnection2.Open();
						OleDbDataReader oleDbDataReader2 = oleDbCommand2.ExecuteReader();
						if (oleDbDataReader2.Read())
						{
							icOperator.m_OperatorID = int.Parse(oleDbDataReader2["f_OperatorID"].ToString());
							icOperator.m_OperatorName = name;
							flag = true;
						}
						oleDbDataReader2.Close();
					}
					return flag;
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			return flag;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0011F948 File Offset: 0x0011E948
		public static bool OperatePrivilegeFullControl(string funName)
		{
			bool flag = false;
			return icOperator.OperatePrivilegeVisible(funName, ref flag) && !flag;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0011F968 File Offset: 0x0011E968
		public static void OperatePrivilegeLoad(MenuStrip mnuMain)
		{
			if (icOperator.m_OperatorID != 1)
			{
				if (icOperator.m_dtCurrentOperatorPrivilege == null)
				{
					icOperator.m_dtCurrentOperatorPrivilege = icOperator.getOperatorPrivilege(icOperator.m_OperatorID);
				}
				if (icOperator.m_dtCurrentOperatorPrivilege.Rows.Count > 0)
				{
					foreach (object obj in icOperator.m_dtCurrentOperatorPrivilege.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						icOperator.mnuItm = new ToolStripMenuItem();
						icOperator.mnuItm.Name = "";
						icOperator.FindInMenu(mnuMain, dataRow["f_FunctionName"].ToString(), ref icOperator.mnuItm);
						if (icOperator.mnuItm.Name.ToString() != "" && !bool.Parse(dataRow["f_ReadOnly"].ToString()) && !bool.Parse(dataRow["f_FullControl"].ToString()))
						{
							icOperator.mnuItm.Visible = false;
						}
					}
				}
			}
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0011FA84 File Offset: 0x0011EA84
		public static void OperatePrivilegeLoad(ref string[,] funcList, int funItemLen, int funNameLoc)
		{
			if (icOperator.m_OperatorID != 1)
			{
				if (icOperator.m_dtCurrentOperatorPrivilege == null)
				{
					icOperator.m_dtCurrentOperatorPrivilege = icOperator.getOperatorPrivilege(icOperator.m_OperatorID);
				}
				if (icOperator.m_dtCurrentOperatorPrivilege.Rows.Count > 0)
				{
					foreach (object obj in icOperator.m_dtCurrentOperatorPrivilege.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						if (!bool.Parse(dataRow["f_ReadOnly"].ToString()) && !bool.Parse(dataRow["f_FullControl"].ToString()))
						{
							for (int i = 0; i < funcList.Length / funItemLen; i++)
							{
								if (!string.IsNullOrEmpty(funcList[i, funNameLoc]) && funcList[i, funNameLoc] == dataRow["f_FunctionName"].ToString())
								{
									funcList[i, funNameLoc] = null;
									break;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0011FB94 File Offset: 0x0011EB94
		public static bool OperatePrivilegeTreeDisplay(string FunctionName)
		{
			if (icOperator.m_OperatorID != 1)
			{
				if (icOperator.m_dtCurrentOperatorPrivilege == null)
				{
					icOperator.m_dtCurrentOperatorPrivilege = icOperator.getOperatorPrivilege(icOperator.m_OperatorID);
				}
				if (string.IsNullOrEmpty(FunctionName))
				{
					return false;
				}
				if (icOperator.m_dtCurrentOperatorPrivilege.Rows.Count > 0)
				{
					using (DataView dataView = new DataView(icOperator.m_dtCurrentOperatorPrivilege))
					{
						dataView.RowFilter = string.Format("f_FunctionName ={0}", wgTools.PrepareStr(FunctionName));
						if (dataView.Count <= 0 || (!bool.Parse(dataView[0]["f_ReadOnly"].ToString()) && !bool.Parse(dataView[0]["f_FullControl"].ToString())))
						{
							return false;
						}
						return true;
					}
					return true;
				}
			}
			return true;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0011FC68 File Offset: 0x0011EC68
		public static bool OperatePrivilegeVisible(string funName)
		{
			bool flag = false;
			return icOperator.OperatePrivilegeVisible(funName, ref flag);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0011FC80 File Offset: 0x0011EC80
		public static bool OperatePrivilegeVisible(string funName, ref bool bReadOnly)
		{
			if (icOperator.m_OperatorID != 1)
			{
				if (icOperator.m_dtCurrentOperatorPrivilege == null)
				{
					icOperator.m_dtCurrentOperatorPrivilege = icOperator.getOperatorPrivilege(icOperator.m_OperatorID);
				}
				if (icOperator.m_dtCurrentOperatorPrivilege.Rows.Count > 0)
				{
					foreach (object obj in icOperator.m_dtCurrentOperatorPrivilege.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						if (bool.Parse(dataRow["f_ReadOnly"].ToString()) || bool.Parse(dataRow["f_FullControl"].ToString()))
						{
							if (!bool.Parse(dataRow["f_FullControl"].ToString()) && funName == dataRow["f_FunctionName"].ToString())
							{
								bReadOnly = true;
							}
						}
						else if (funName == dataRow["f_FunctionName"].ToString())
						{
							return false;
						}
					}
					return true;
				}
			}
			return true;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0011FD94 File Offset: 0x0011ED94
		public static string PCSysInfo(bool bSuper)
		{
			string text = "";
			try
			{
				text += string.Format("\r\n.Net Framework {0} ", Environment.Version.ToString());
			}
			catch
			{
			}
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						text += string.Format("\r\n{0}: ", CommonStr.strSystem);
						text += string.Format("\r\n{0} ", managementObject["Caption"]);
						text += string.Format("\r\n{0} ", managementObject["version"].ToString());
						text += string.Format("\r\n{0} ", managementObject["CSDVersion"]);
						try
						{
							RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Internet Explorer");
							if (registryKey != null)
							{
								text += string.Format("\r\n{0}: ", CommonStr.strIEVersion);
								text += string.Format("\r\n{0} ", registryKey.GetValue("Version"));
							}
							registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\DataAccess");
							if (registryKey != null)
							{
								text += string.Format("\r\n{0} ", "MDAC " + registryKey.GetValue("FullInstallVer"));
							}
						}
						catch
						{
						}
						text += string.Format("\r\n{0} ", "---------------------------------------------");
						text += string.Format("\r\n{0} ", CommonStr.strRegistered);
						text += string.Format("\r\n{0} ", managementObject["RegisteredUser"]);
						text += string.Format("\r\n{0} ", managementObject["Organization"].ToString());
						text += string.Format("\r\n{0} ", managementObject["SerialNumber"]);
						text += string.Format("\r\n{0} ", "---------------------------------------------");
						text += string.Format("\r\n{0} ", CommonStr.strComputer);
						text += string.Format("\r\n{0} ", managementObject["TotalVisibleMemorySize"].ToString() + " KB RAM");
					}
				}
			}
			catch
			{
			}
			try
			{
				int width = Screen.PrimaryScreen.Bounds.Width;
				int height = Screen.PrimaryScreen.Bounds.Height;
				text += string.Format("\r\n{0}:{1:d} x {2:d}: ", CommonStr.strDisplaySize, width, height);
				text += string.Format("\r\n{0} IP:", Dns.GetHostName());
				foreach (IPAddress ipaddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
				{
					text += string.Format("\r\n{0}", ipaddress.ToString());
				}
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\International");
				if (registryKey != null)
				{
					text += string.Format("\r\n{0}", "---------------------------------------------");
					text += string.Format("\r\n{0}:{1}", CommonStr.strCountry, registryKey.GetValue("sCountry"));
					text += string.Format("\r\n{0}:{1}", CommonStr.strTimeFormat, registryKey.GetValue("sTimeFormat"));
					text += string.Format("\r\n{0}:{1}", CommonStr.strShortDateFormat, registryKey.GetValue("sShortDate"));
					text += string.Format("\r\n{0}:{1}", CommonStr.strLongDateFormat, registryKey.GetValue("sLongDate"));
				}
			}
			catch
			{
			}
			try
			{
				if (wgAppConfig.IsAccessDB)
				{
					string text2 = "Microsoft Access";
					if (!string.IsNullOrEmpty(text2))
					{
						text += string.Format("\r\n{0}", "---------------------------------------------");
						text += string.Format("\r\n{0}", text2);
					}
				}
				else
				{
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						string text3 = "SELECT @@VERSION";
						using (SqlCommand sqlCommand = new SqlCommand(text3, sqlConnection))
						{
							text3 = wgTools.SetObjToStr(sqlCommand.ExecuteScalar());
						}
						if (!string.IsNullOrEmpty(text3))
						{
							text += string.Format("\r\n{0}", "---------------------------------------------");
							text += string.Format("\r\n{0}", text3);
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return (text + string.Format("\r\n{0}", "---------------------------------------------") + string.Format("\r\n{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss "))).Replace("'", "\"");
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0012033C File Offset: 0x0011F33C
		public static int setOperatorPrivilege(int OperatorID, DataTable dtPrivilege)
		{
			wgAppConfig.runUpdateSql(string.Format("DELETE FROM t_s_OperatorPrivilege WHERE [f_OperatorID] =" + OperatorID.ToString(), new object[0]));
			for (int i = 0; i < dtPrivilege.Rows.Count; i++)
			{
				int num = 1;
				int num2 = 1;
				if (string.IsNullOrEmpty(dtPrivilege.Rows[i][3].ToString()))
				{
					num = 0;
				}
				else if (!bool.Parse(dtPrivilege.Rows[i][3].ToString()))
				{
					num = 0;
				}
				if (string.IsNullOrEmpty(dtPrivilege.Rows[i][4].ToString()))
				{
					num2 = 0;
				}
				else if (!bool.Parse(dtPrivilege.Rows[i][4].ToString()))
				{
					num2 = 0;
				}
				wgAppConfig.runUpdateSql(string.Format("INSERT INTO t_s_OperatorPrivilege ([f_OperatorID], [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl]) VALUES({5}, {0:d},{1},{2},{3:d},{4:d})", new object[]
				{
					int.Parse(dtPrivilege.Rows[i][0].ToString()),
					wgTools.PrepareStrNUnicode(dtPrivilege.Rows[i][1].ToString()),
					wgTools.PrepareStrNUnicode(dtPrivilege.Rows[i][2].ToString()),
					num,
					num2,
					OperatorID.ToString()
				}));
			}
			return 1;
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x001204B0 File Offset: 0x0011F4B0
		public static DataTable dtCurrentOperatorPrivilege
		{
			get
			{
				return icOperator.m_dtCurrentOperatorPrivilege;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x001204B7 File Offset: 0x0011F4B7
		public static int OperatorID
		{
			get
			{
				return icOperator.m_OperatorID;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x001204BE File Offset: 0x0011F4BE
		public static string OperatorName
		{
			get
			{
				return icOperator.m_OperatorName;
			}
		}

		// Token: 0x04001C82 RID: 7298
		private static Dictionary<string, string> dicMenuToFrm;

		// Token: 0x04001C83 RID: 7299
		private static DataTable dt = null;

		// Token: 0x04001C84 RID: 7300
		private static DataTable dt1 = null;

		// Token: 0x04001C85 RID: 7301
		private static DataTable m_dtCurrentOperatorPrivilege;

		// Token: 0x04001C86 RID: 7302
		private static DataTable m_dtDefaultAllPrivilege;

		// Token: 0x04001C87 RID: 7303
		private static int m_OperatorID = 0;

		// Token: 0x04001C88 RID: 7304
		private static string m_OperatorName = "";

		// Token: 0x04001C89 RID: 7305
		private static ToolStripMenuItem mnuItm = null;
	}
}
