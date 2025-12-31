using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x020002D4 RID: 724
	public partial class dfrmFaceManagement4Hikvision : frmN3000
	{
		// Token: 0x060013A6 RID: 5030 RVA: 0x00178974 File Offset: 0x00177974
		public dfrmFaceManagement4Hikvision()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			wgAppConfig.custDataGridview(ref this.dgvRunInfo);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00178A90 File Offset: 0x00177A90
		private void _fillDeviceGrid()
		{
			try
			{
				string text = " SELECT ";
				text = text + " f_DeviceId , f_DeviceName  , 0 as f_Selected, f_DeviceIP , f_DevicePort , f_ReaderName , f_CardNOWorkNODiff , f_DevicePassword , f_Notes   from t_b_ThirdPartyNetDevice LEFT JOIN t_b_Reader ON t_b_Reader.f_ReaderID = t_b_ThirdPartyNetDevice.f_ReaderID  WHERE f_factory =   " + wgTools.PrepareStrNUnicode(this.factoryName) + " ORDER BY f_DeviceName  ";
				this.dt = new DataTable();
				this.dv = new DataView(this.dt);
				this.dvSelected = new DataView(this.dt);
				DataTable dataTable = new DataTable();
				new DataView(dataTable);
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
							{
								oleDbDataAdapter.Fill(this.dt);
							}
						}
						goto IL_0109;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlDataAdapter.Fill(this.dt);
						}
					}
				}
				try
				{
					IL_0109:
					this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				this.dv.RowFilter = "f_Selected = 0";
				this.dvSelected.RowFilter = "f_Selected > 0";
				this.dgvDoors.AutoGenerateColumns = false;
				this.dgvDoors.DataSource = this.dv;
				this.dgvSelectedDoors.AutoGenerateColumns = false;
				this.dgvSelectedDoors.DataSource = this.dvSelected;
				for (int i = 0; i < this.dgvDoors.Columns.Count; i++)
				{
					this.dgvDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvSelectedDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgDebugWrite(ex2.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00178D90 File Offset: 0x00177D90
		private void addLogEvent(string desc, string info)
		{
			this.addLogEvent(desc, info, 100);
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00178D9C File Offset: 0x00177D9C
		private void addLogEvent(string desc, string info, int category)
		{
			InfoRow infoRow = new InfoRow();
			infoRow.category = category;
			infoRow.desc = desc;
			infoRow.information = info;
			if (this.tbRunInfoLog == null)
			{
				wgAppConfig.wgLog(string.Format("{0},{1},{2},{3},{4}", new object[] { "0", infoRow.desc, infoRow.information, infoRow.detail, infoRow.MjRecStr }));
				return;
			}
			lock (this.tbRunInfoLog)
			{
				DataRow dataRow = this.tbRunInfoLog.NewRow();
				dataRow[0] = infoRow.category;
				this.eventRecID++;
				dataRow[1] = this.eventRecID.ToString();
				dataRow[2] = DateTime.Now.ToString("HH:mm:ss");
				dataRow[3] = infoRow.desc;
				dataRow[4] = infoRow.information;
				dataRow[5] = infoRow.detail;
				dataRow[6] = infoRow.MjRecStr;
				object[] array = new object[]
				{
					(this.tbRunInfoLog.Rows.Count + 1).ToString(),
					infoRow.desc,
					infoRow.information,
					infoRow.detail,
					infoRow.MjRecStr
				};
				wgAppConfig.wgLog(string.Format("{0},{1},{2},{3},{4}", array));
				this.tbRunInfoLog.Rows.Add(dataRow);
				this.tbRunInfoLog.AcceptChanges();
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00178F50 File Offset: 0x00177F50
		public void AdjustTimeAllDevice()
		{
			object obj = this.btnAdjustTime;
			this._fillDeviceGrid();
			this.btnAddAllDoors_Click(null, null);
			wgAppConfig.wgLog(string.Concat(new string[]
			{
				(obj as Button).Text,
				" ,",
				CommonStr.strDevicesNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString()
			}));
			this.logOperate(obj);
			try
			{
				new ArrayList();
				new ArrayList();
				DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
				dataView.RowFilter = "f_Selected > 0";
				DateTime dateTime = DateTime.Now.AddSeconds(3.0);
				this.arrPingInfoReset();
				foreach (object obj2 in dataView)
				{
					DataRowView dataRowView = (DataRowView)obj2;
					this.pingControllerIP(wgTools.SetObjToStr(dataRowView["f_DeviceIP"]));
				}
				wgTools.WriteLine("bExistIPNet");
				Thread.Sleep(100);
				while (DateTime.Now < dateTime && this.arrIPARPOK.Count == 0 && this.arrIPPingOK.Count == 0)
				{
					Thread.Sleep(100);
				}
				wgTools.WriteLine("(DateTime.Now < dtPingEnd)");
				ArrayList arrayList = new ArrayList();
				if (this.arrIPARPOK.Count != 0 || this.arrIPPingOK.Count != 0)
				{
					for (int i = 0; i < 2; i++)
					{
						foreach (object obj3 in dataView)
						{
							DataRowView dataRowView2 = (DataRowView)obj3;
							if (this.IsIPCanConnect(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"])) && arrayList.IndexOf(dataRowView2["f_DeviceName"]) < 0)
							{
								arrayList.Add(dataRowView2["f_DeviceName"]);
								this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (obj as Button).Text), CommonStr.strStart);
								string text = "";
								byte[] array = null;
								this.tcpClose();
								DateTime now = DateTime.Now;
								string text2 = now.ToString("yyyy-MM-dd");
								if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), string.Format("SetDateTime(date=\"{0}\" time=\"{1}\")", text2, now.ToString("HH:mm:ss")), ref text, ref array) > 0)
								{
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (obj as Button).Text), string.Format("{0} {1}. ", (obj as Button).Text, CommonStr.strSuccessfully));
								}
								else
								{
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (obj as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
									{
										CommonStr.strCommFail,
										wgTools.SetObjToStr(dataRowView2["f_DeviceName"]),
										wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
										wgTools.SetObjToStr(dataRowView2["f_DevicePort"])
									}), 101);
								}
								this.displayNewestLog();
							}
						}
						if (arrayList.Count == dataView.Count)
						{
							break;
						}
						Thread.Sleep(300);
					}
				}
				if (arrayList.Count != dataView.Count)
				{
					foreach (object obj4 in dataView)
					{
						DataRowView dataRowView3 = (DataRowView)obj4;
						if (arrayList.IndexOf(dataRowView3["f_DeviceName"]) < 0)
						{
							this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView3["f_DeviceName"]), (obj as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
							{
								CommonStr.strCommFail,
								wgTools.SetObjToStr(dataRowView3["f_DeviceName"]),
								wgTools.SetObjToStr(dataRowView3["f_DeviceIP"]),
								wgTools.SetObjToStr(dataRowView3["f_DevicePort"])
							}), 101);
						}
					}
				}
				this.tcpClose();
				icConsumerShare.setUpdateLog();
				this.displayNewestLog();
				this.dfrmWait1.Hide();
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(obj as Button).Text.Replace("\r\n", ""),
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString(),
					",",
					CommonStr.strOperationComplete
				}), EventLogEntryType.Information, null);
				Cursor.Current = Cursors.Default;
				this.progressBar1.Value = this.progressBar1.Maximum;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				return;
			}
			this.displayNewestLog();
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00179538 File Offset: 0x00178538
		public int adjusttimeByHTTP(string strIP, int PORT, string devicePassword)
		{
			CHCNetSDK.NET_DVR_USER_LOGIN_INFO net_DVR_USER_LOGIN_INFO = default(CHCNetSDK.NET_DVR_USER_LOGIN_INFO);
			CHCNetSDK.NET_DVR_DEVICEINFO_V40 net_DVR_DEVICEINFO_V = default(CHCNetSDK.NET_DVR_DEVICEINFO_V40);
			net_DVR_DEVICEINFO_V.struDeviceV30.sSerialNumber = new byte[48];
			net_DVR_USER_LOGIN_INFO.sDeviceAddress = strIP;
			net_DVR_USER_LOGIN_INFO.sUserName = this.deviceDefaultUser;
			net_DVR_USER_LOGIN_INFO.sPassword = this.deviceDefaultPassword;
			net_DVR_USER_LOGIN_INFO.wPort = (ushort)PORT;
			int num = CHCNetSDK.NET_DVR_Login_V40(ref net_DVR_USER_LOGIN_INFO, ref net_DVR_DEVICEINFO_V);
			if (num < 0)
			{
				wgAppConfig.wgLog("check  failed=" + CHCNetSDK.NET_DVR_GetLastError().ToString());
				return -1;
			}
			CHCNetSDK.NET_DVR_TIME net_DVR_TIME = default(CHCNetSDK.NET_DVR_TIME);
			DateTime now = DateTime.Now;
			net_DVR_TIME.dwYear = (int)uint.Parse(now.Year.ToString());
			net_DVR_TIME.dwMonth = (int)uint.Parse(now.Month.ToString());
			net_DVR_TIME.dwDay = (int)uint.Parse(now.Day.ToString());
			net_DVR_TIME.dwHour = (int)uint.Parse(now.Hour.ToString());
			net_DVR_TIME.dwMinute = (int)uint.Parse(now.Minute.ToString());
			net_DVR_TIME.dwSecond = (int)uint.Parse(now.Second.ToString());
			uint num2 = (uint)Marshal.SizeOf(net_DVR_TIME);
			IntPtr intPtr = Marshal.AllocHGlobal((int)num2);
			Marshal.StructureToPtr(net_DVR_TIME, intPtr, false);
			bool flag = CHCNetSDK.NET_DVR_SetDVRConfig(num, 119U, 0, intPtr, num2);
			Marshal.FreeHGlobal(intPtr);
			CHCNetSDK.NET_DVR_Logout_V30(num);
			if (flag)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x001796BF File Offset: 0x001786BF
		private void arrPingInfoReset()
		{
			this.arrIPPingOK.Clear();
			this.arrIPPingFailed.Clear();
			this.arrIPARPOK.Clear();
			this.arrIPARPFailed.Clear();
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x001796F0 File Offset: 0x001786F0
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			e.Result = this.loadUserData4BackWork();
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00179734 File Offset: 0x00178734
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				wgTools.WgDebugWrite("Users Operation was canceled", new object[0]);
				return;
			}
			if (e.Error != null)
			{
				wgTools.WgDebugWrite(string.Format("An error occurred: {0}", e.Error.Message), new object[0]);
				return;
			}
			this.loadUserData4BackWorkComplete(e.Result as DataTable);
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString());
			this.updateUserPhoto();
			this.defaultFindControl = this.dgvUsers;
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x001797C4 File Offset: 0x001787C4
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 1;
			}
			this.updateButtonDisplay();
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00179830 File Offset: 0x00178830
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			if (this.strGroupFilter == "")
			{
				icConsumerShare.selectAllUsers();
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
				this.updateButtonDisplay();
				return;
			}
			wgTools.WriteLine("btnAddAllUsers_Click Start");
			this.dt = ((DataView)this.dgvUsers.DataSource).Table;
			this.dv1 = (DataView)this.dgvUsers.DataSource;
			this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
			this.dgvUsers.DataSource = null;
			this.dgvSelectedUsers.DataSource = null;
			if (this.strGroupFilter == "")
			{
				this.updateButtonDisplay();
				return;
			}
			string rowFilter = this.dv1.RowFilter;
			string rowFilter2 = this.dv2.RowFilter;
			this.dv1.Dispose();
			this.dv2.Dispose();
			this.dv1 = null;
			this.dv2 = null;
			this.dt.BeginLoadData();
			this.dv = new DataView(this.dt);
			this.dv.RowFilter = this.strGroupFilter;
			for (int i = 0; i < this.dv.Count; i++)
			{
				this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
			}
			this.dt.EndLoadData();
			this.dv1 = new DataView(this.dt);
			this.dv1.RowFilter = rowFilter;
			this.dv2 = new DataView(this.dt);
			this.dv2.RowFilter = rowFilter2;
			this.dgvUsers.DataSource = this.dv1;
			this.dgvSelectedUsers.DataSource = this.dv2;
			wgTools.WriteLine("btnAddAllUsers_Click End");
			this.updateButtonDisplay();
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00179A4F File Offset: 0x00178A4F
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvDoors);
			this.updateButtonDisplay();
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00179A62 File Offset: 0x00178A62
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
			this.updateButtonDisplay();
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00179A7C File Offset: 0x00178A7C
		private void btnAdjustTime_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(string.Concat(new string[]
			{
				CommonStr.strAreYouSure,
				" {0}?\r\n\r\n",
				CommonStr.strDevicesNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString()
			}), (sender as Button).Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(sender as Button).Text,
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString()
				}));
				this.logOperate(sender);
				this.progressBar1.Value = 0;
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				try
				{
					new ArrayList();
					new ArrayList();
					DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
					dataView.RowFilter = "f_Selected > 0";
					DateTime dateTime = DateTime.Now.AddSeconds(3.0);
					this.arrPingInfoReset();
					foreach (object obj in dataView)
					{
						DataRowView dataRowView = (DataRowView)obj;
						this.pingControllerIP(wgTools.SetObjToStr(dataRowView["f_DeviceIP"]));
					}
					wgTools.WriteLine("bExistIPNet");
					Thread.Sleep(100);
					while (DateTime.Now < dateTime && this.arrIPARPOK.Count == 0 && this.arrIPPingOK.Count == 0)
					{
						Thread.Sleep(100);
					}
					wgTools.WriteLine("(DateTime.Now < dtPingEnd)");
					ArrayList arrayList = new ArrayList();
					if (this.arrIPARPOK.Count != 0 || this.arrIPPingOK.Count != 0)
					{
						for (int i = 0; i < 2; i++)
						{
							foreach (object obj2 in dataView)
							{
								DataRowView dataRowView2 = (DataRowView)obj2;
								if (this.IsIPCanConnect(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"])) && arrayList.IndexOf(dataRowView2["f_DeviceName"]) < 0)
								{
									arrayList.Add(dataRowView2["f_DeviceName"]);
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), CommonStr.strStart);
									if (this.adjusttimeByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"])) > 0)
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}. ", (sender as Button).Text, CommonStr.strSuccessfully));
									}
									else
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
										{
											CommonStr.strCommFail,
											wgTools.SetObjToStr(dataRowView2["f_DeviceName"]),
											wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
											wgTools.SetObjToStr(dataRowView2["f_DevicePort"])
										}), 101);
									}
									this.displayNewestLog();
								}
							}
							if (arrayList.Count == dataView.Count)
							{
								break;
							}
							Thread.Sleep(300);
						}
					}
					if (arrayList.Count != dataView.Count)
					{
						foreach (object obj3 in dataView)
						{
							DataRowView dataRowView3 = (DataRowView)obj3;
							if (arrayList.IndexOf(dataRowView3["f_DeviceName"]) < 0)
							{
								this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView3["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
								{
									CommonStr.strCommFail,
									wgTools.SetObjToStr(dataRowView3["f_DeviceName"]),
									wgTools.SetObjToStr(dataRowView3["f_DeviceIP"]),
									wgTools.SetObjToStr(dataRowView3["f_DevicePort"])
								}), 101);
							}
						}
					}
					this.tcpClose();
					icConsumerShare.setUpdateLog();
					this.displayNewestLog();
					this.dfrmWait1.Hide();
					wgAppConfig.wgLog(string.Concat(new string[]
					{
						(sender as Button).Text.Replace("\r\n", ""),
						" ,",
						CommonStr.strDevicesNum,
						" = ",
						this.dgvSelectedDoors.RowCount.ToString(),
						",",
						CommonStr.strOperationComplete
					}), EventLogEntryType.Information, null);
					Cursor.Current = Cursors.Default;
					this.progressBar1.Value = this.progressBar1.Maximum;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					return;
				}
				finally
				{
					this.dfrmWait1.Hide();
				}
				this.displayNewestLog();
			}
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0017A0D0 File Offset: 0x001790D0
		private void btnCheck_Click(object sender, EventArgs e)
		{
			this.progressBar1.Value = 0;
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			try
			{
				new ArrayList();
				new ArrayList();
				DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
				dataView.RowFilter = "f_Selected > 0";
				DateTime dateTime = DateTime.Now.AddSeconds(3.0);
				this.arrPingInfoReset();
				foreach (object obj in dataView)
				{
					DataRowView dataRowView = (DataRowView)obj;
					this.pingControllerIP(wgTools.SetObjToStr(dataRowView["f_DeviceIP"]));
				}
				wgTools.WriteLine("bExistIPNet");
				Thread.Sleep(100);
				while (DateTime.Now < dateTime && this.arrIPARPOK.Count == 0 && this.arrIPPingOK.Count == 0)
				{
					Thread.Sleep(100);
				}
				wgTools.WriteLine("(DateTime.Now < dtPingEnd)");
				ArrayList arrayList = new ArrayList();
				if (this.arrIPARPOK.Count != 0 || this.arrIPPingOK.Count != 0)
				{
					for (int i = 0; i < 2; i++)
					{
						foreach (object obj2 in dataView)
						{
							DataRowView dataRowView2 = (DataRowView)obj2;
							if (this.IsIPCanConnect(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"])) && arrayList.IndexOf(dataRowView2["f_DeviceName"]) < 0)
							{
								arrayList.Add(dataRowView2["f_DeviceName"]);
								string text = "";
								int num = this.checkByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"]));
								if (num > 0)
								{
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}. ", (sender as Button).Text, CommonStr.strSuccessfully));
									wgAppConfig.wgLog(string.Format("[{0}]{1}:{2}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text, text));
								}
								else if (num == 0)
								{
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
									{
										CommonStr.strCommFail + CommonStr.strWrongDevicePassword,
										wgTools.SetObjToStr(dataRowView2["f_DeviceName"]),
										wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
										wgTools.SetObjToStr(dataRowView2["f_DevicePort"])
									}), 101);
								}
								this.displayNewestLog();
							}
						}
						if (arrayList.Count == dataView.Count)
						{
							break;
						}
						Thread.Sleep(300);
					}
				}
				if (arrayList.Count != dataView.Count)
				{
					foreach (object obj3 in dataView)
					{
						DataRowView dataRowView3 = (DataRowView)obj3;
						if (arrayList.IndexOf(dataRowView3["f_DeviceName"]) < 0)
						{
							this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView3["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
							{
								CommonStr.strCommFail,
								wgTools.SetObjToStr(dataRowView3["f_DeviceName"]),
								wgTools.SetObjToStr(dataRowView3["f_DeviceIP"]),
								wgTools.SetObjToStr(dataRowView3["f_DevicePort"])
							}), 101);
						}
					}
				}
				icConsumerShare.setUpdateLog();
				this.displayNewestLog();
				this.dfrmWait1.Hide();
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(sender as Button).Text.Replace("\r\n", ""),
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString(),
					",",
					CommonStr.strOperationComplete
				}), EventLogEntryType.Information, null);
				Cursor.Current = Cursors.Default;
				this.progressBar1.Value = this.progressBar1.Maximum;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				return;
			}
			this.displayNewestLog();
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0017A644 File Offset: 0x00179644
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 0;
			}
			this.updateButtonDisplay();
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0017A6B0 File Offset: 0x001796B0
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnDelAllUsers_Click Start");
				icConsumerShare.selectNoneUsers();
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
					((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
					((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
				}
				this.updateButtonDisplay();
			}
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0017A798 File Offset: 0x00179798
		private void btnDeleteAll_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(string.Concat(new string[]
			{
				CommonStr.strAreYouSure,
				" {0}?\r\n\r\n",
				CommonStr.strDevicesClearFace,
				"\r\n\r\n",
				CommonStr.strDevicesNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString()
			}), (sender as Button).Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(sender as Button).Text,
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString()
				}));
				this.logOperate(sender);
				this.progressBar1.Value = 0;
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				try
				{
					new ArrayList();
					new ArrayList();
					DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
					dataView.RowFilter = "f_Selected > 0";
					DateTime dateTime = DateTime.Now.AddSeconds(3.0);
					this.arrPingInfoReset();
					foreach (object obj in dataView)
					{
						DataRowView dataRowView = (DataRowView)obj;
						this.pingControllerIP(wgTools.SetObjToStr(dataRowView["f_DeviceIP"]));
					}
					wgTools.WriteLine("bExistIPNet");
					Thread.Sleep(100);
					while (DateTime.Now < dateTime && this.arrIPARPOK.Count == 0 && this.arrIPPingOK.Count == 0)
					{
						Thread.Sleep(100);
					}
					wgTools.WriteLine("(DateTime.Now < dtPingEnd)");
					ArrayList arrayList = new ArrayList();
					if (this.arrIPARPOK.Count != 0 || this.arrIPPingOK.Count != 0)
					{
						for (int i = 0; i < 2; i++)
						{
							foreach (object obj2 in dataView)
							{
								DataRowView dataRowView2 = (DataRowView)obj2;
								if (this.IsIPCanConnect(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"])) && arrayList.IndexOf(dataRowView2["f_DeviceName"]) < 0)
								{
									arrayList.Add(dataRowView2["f_DeviceName"]);
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), CommonStr.strStart);
									string text = "";
									byte[] array = null;
									this.tcpClose();
									DateTime now = DateTime.Now;
									int num = this.updateOneUserByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"]), -1L, 0L, "", "", ref text, ref array);
									if (this.m_lUserID4updateOneUser != -1)
									{
										CHCNetSDK.NET_DVR_Logout_V30(this.m_lUserID4updateOneUser);
										this.m_lUserID4updateOneUser = -1;
									}
									if (num > 0)
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}. ", (sender as Button).Text, CommonStr.strSuccessfully));
									}
									else
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
										{
											CommonStr.strCommFail,
											wgTools.SetObjToStr(dataRowView2["f_DeviceName"]),
											wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
											wgTools.SetObjToStr(dataRowView2["f_DevicePort"])
										}), 101);
									}
									this.displayNewestLog();
									if (this.bExit)
									{
										return;
									}
								}
							}
							if (arrayList.Count == dataView.Count)
							{
								break;
							}
							Thread.Sleep(300);
						}
					}
					if (arrayList.Count != dataView.Count)
					{
						foreach (object obj3 in dataView)
						{
							DataRowView dataRowView3 = (DataRowView)obj3;
							if (arrayList.IndexOf(dataRowView3["f_DeviceName"]) < 0)
							{
								this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView3["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
								{
									CommonStr.strCommFail,
									wgTools.SetObjToStr(dataRowView3["f_DeviceName"]),
									wgTools.SetObjToStr(dataRowView3["f_DeviceIP"]),
									wgTools.SetObjToStr(dataRowView3["f_DevicePort"])
								}), 101);
							}
						}
					}
					this.tcpClose();
					icConsumerShare.setUpdateLog();
					this.displayNewestLog();
					this.dfrmWait1.Hide();
					wgAppConfig.wgLog(string.Concat(new string[]
					{
						(sender as Button).Text.Replace("\r\n", ""),
						" ,",
						CommonStr.strDevicesNum,
						" = ",
						this.dgvSelectedDoors.RowCount.ToString(),
						",",
						CommonStr.strOperationComplete
					}), EventLogEntryType.Information, null);
					Cursor.Current = Cursors.Default;
					this.progressBar1.Value = this.progressBar1.Maximum;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					return;
				}
				finally
				{
					this.dfrmWait1.Hide();
				}
				this.displayNewestLog();
			}
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x0017AE54 File Offset: 0x00179E54
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.updateButtonDisplay();
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0017AE67 File Offset: 0x00179E67
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
			this.updateButtonDisplay();
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0017AE80 File Offset: 0x00179E80
		private void btnDeviceManage_Click(object sender, EventArgs e)
		{
			using (dfrmFaceDeviceManagement dfrmFaceDeviceManagement = new dfrmFaceDeviceManagement())
			{
				dfrmFaceDeviceManagement.ShowDialog(this);
			}
			this._fillDeviceGrid();
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0017AEC0 File Offset: 0x00179EC0
		private void btnDownloadAllUsers_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Concat(new string[]
			{
				string.Format(CommonStr.strAreYouSure + " {0}?", (sender as Button).Text),
				"\r\n\r\n",
				CommonStr.strDevicesNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString()
			}), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(sender as Button).Text,
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString()
				}));
				this.logOperate(sender);
				this.progressBar1.Value = 0;
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				try
				{
					new ArrayList();
					new ArrayList();
					DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
					dataView.RowFilter = "f_Selected > 0";
					DateTime dateTime = DateTime.Now.AddSeconds(3.0);
					this.arrPingInfoReset();
					foreach (object obj in dataView)
					{
						DataRowView dataRowView = (DataRowView)obj;
						this.pingControllerIP(wgTools.SetObjToStr(dataRowView["f_DeviceIP"]));
					}
					wgTools.WriteLine("bExistIPNet");
					Thread.Sleep(100);
					while (DateTime.Now < dateTime && this.arrIPARPOK.Count == 0 && this.arrIPPingOK.Count == 0)
					{
						Thread.Sleep(100);
					}
					wgTools.WriteLine("(DateTime.Now < dtPingEnd)");
					ArrayList arrayList = new ArrayList();
					if (this.arrIPARPOK.Count != 0 || this.arrIPPingOK.Count != 0)
					{
						for (int i = 0; i < 2; i++)
						{
							foreach (object obj2 in dataView)
							{
								DataRowView dataRowView2 = (DataRowView)obj2;
								if (this.IsIPCanConnect(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"])) && arrayList.IndexOf(dataRowView2["f_DeviceName"]) < 0)
								{
									arrayList.Add(dataRowView2["f_DeviceName"]);
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), CommonStr.strStart);
									string text = "";
									byte[] array = null;
									bool flag = false;
									this.tcpClose();
									if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), "GetEmployeeID()", ref text, ref array) > 0)
									{
										dfrmFaceManagement4Hikvision.FaceId_Item[] allItems = dfrmFaceManagement4Hikvision.FaceId_Item.GetAllItems(text);
										if (allItems != null)
										{
											int num = 0;
											ArrayList arrayList2 = new ArrayList();
											for (int j = 0; j < allItems.Length; j++)
											{
												if (allItems[j].Name == "id")
												{
													arrayList2.Add(allItems[j].Value);
												}
											}
											this.progressBar1.Maximum += arrayList2.Count;
											for (int k = 0; k < arrayList2.Count; k++)
											{
												if (this.progressBar1.Value < this.progressBar1.Maximum)
												{
													this.progressBar1.Value++;
													Application.DoEvents();
												}
												string text2 = wgTools.SetObjToStr(arrayList2[k]);
												if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), "GetEmployee(id=\"" + text2 + "\")", ref text, ref array) > 0 && text.IndexOf("face_data=\"") > 0)
												{
													dfrmFaceManagement4Hikvision.FaceId_Item[] allItems2 = dfrmFaceManagement4Hikvision.FaceId_Item.GetAllItems(text);
													ArrayList arrayList3 = new ArrayList();
													for (int l = 0; l < allItems2.Length; l++)
													{
														if (allItems2[l].Name == "face_data")
														{
															arrayList3.Add(allItems2[l].Value);
														}
													}
													bool flag2 = false;
													if (arrayList3.Count > 1)
													{
														for (int m = 0; m < arrayList3.Count - 2; m++)
														{
															if (!arrayList3[m].Equals(arrayList3[m + 1]))
															{
																flag2 = true;
																break;
															}
														}
													}
													if (flag2 && this.updateDBFaceInfo(long.Parse(text2), text, long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"]))) > 0)
													{
														num++;
													}
												}
											}
											flag = true;
											this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[{2}/{3}] ", new object[]
											{
												(sender as Button).Text,
												CommonStr.strSuccessfully,
												num,
												arrayList2.Count
											}));
										}
									}
									if (!flag)
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
										{
											CommonStr.strCommFail,
											wgTools.SetObjToStr(dataRowView2["f_DeviceName"]),
											wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
											wgTools.SetObjToStr(dataRowView2["f_DevicePort"])
										}), 101);
									}
									this.displayNewestLog();
								}
							}
							if (arrayList.Count == dataView.Count)
							{
								break;
							}
							Thread.Sleep(300);
						}
					}
					if (arrayList.Count != dataView.Count)
					{
						foreach (object obj3 in dataView)
						{
							DataRowView dataRowView3 = (DataRowView)obj3;
							if (arrayList.IndexOf(dataRowView3["f_DeviceName"]) < 0)
							{
								this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView3["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
								{
									CommonStr.strCommFail,
									wgTools.SetObjToStr(dataRowView3["f_DeviceName"]),
									wgTools.SetObjToStr(dataRowView3["f_DeviceIP"]),
									wgTools.SetObjToStr(dataRowView3["f_DevicePort"])
								}), 101);
							}
						}
					}
					this.tcpClose();
					icConsumerShare.setUpdateLog();
					this.displayNewestLog();
					if (!this.backgroundWorker1.IsBusy)
					{
						this.backgroundWorker1.RunWorkerAsync();
					}
					this.dfrmWait1.Hide();
					wgAppConfig.wgLog(string.Concat(new string[]
					{
						(sender as Button).Text.Replace("\r\n", ""),
						" ,",
						CommonStr.strDevicesNum,
						" = ",
						this.dgvSelectedDoors.RowCount.ToString(),
						",",
						CommonStr.strOperationComplete
					}), EventLogEntryType.Information, null);
					Cursor.Current = Cursors.Default;
					this.progressBar1.Value = this.progressBar1.Maximum;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					return;
				}
				finally
				{
					this.dfrmWait1.Hide();
				}
				this.displayNewestLog();
			}
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0017B75C File Offset: 0x0017A75C
		private void btnDownloadSelectedUsers_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(string.Concat(new string[]
			{
				CommonStr.strAreYouSure,
				" {0}?\r\n\r\n",
				CommonStr.strUsersNum,
				" = ",
				this.dgvSelectedUsers.RowCount.ToString(),
				"\r\n\r\n",
				CommonStr.strDevicesNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString()
			}), (sender as Button).Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(sender as Button).Text,
					" ,",
					CommonStr.strUsersNum,
					" = ",
					this.dgvSelectedUsers.RowCount.ToString(),
					",",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString()
				}));
				this.logOperate(sender);
				this.progressBar1.Value = 0;
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				try
				{
					ArrayList arrayList = new ArrayList();
					int num = 0;
					while (num < this.dgvSelectedUsers.Rows.Count && ((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count >= this.dgvSelectedUsers.Rows.Count)
					{
						if (!string.IsNullOrEmpty(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_CardNO"])))
						{
							arrayList.Add(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_CardNO"]));
						}
						num++;
					}
					new ArrayList();
					new ArrayList();
					DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
					dataView.RowFilter = "f_Selected > 0";
					DateTime dateTime = DateTime.Now.AddSeconds(3.0);
					this.arrPingInfoReset();
					foreach (object obj in dataView)
					{
						DataRowView dataRowView = (DataRowView)obj;
						this.pingControllerIP(wgTools.SetObjToStr(dataRowView["f_DeviceIP"]));
					}
					wgTools.WriteLine("bExistIPNet");
					Thread.Sleep(100);
					while (DateTime.Now < dateTime && this.arrIPARPOK.Count == 0 && this.arrIPPingOK.Count == 0)
					{
						Thread.Sleep(100);
					}
					wgTools.WriteLine("(DateTime.Now < dtPingEnd)");
					ArrayList arrayList2 = new ArrayList();
					if (this.arrIPARPOK.Count != 0 || this.arrIPPingOK.Count != 0)
					{
						for (int i = 0; i < 2; i++)
						{
							foreach (object obj2 in dataView)
							{
								DataRowView dataRowView2 = (DataRowView)obj2;
								if (this.IsIPCanConnect(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"])) && arrayList2.IndexOf(dataRowView2["f_DeviceName"]) < 0)
								{
									arrayList2.Add(dataRowView2["f_DeviceName"]);
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), CommonStr.strStart);
									string text = "";
									byte[] array = null;
									bool flag = false;
									this.tcpClose();
									if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), "GetEmployeeID()", ref text, ref array) > 0)
									{
										dfrmFaceManagement4Hikvision.FaceId_Item[] allItems = dfrmFaceManagement4Hikvision.FaceId_Item.GetAllItems(text);
										if (allItems != null)
										{
											int num2 = 0;
											ArrayList arrayList3 = new ArrayList();
											for (int j = 0; j < allItems.Length; j++)
											{
												if (allItems[j].Name == "id")
												{
													arrayList3.Add(allItems[j].Value);
												}
											}
											this.progressBar1.Maximum += arrayList3.Count;
											for (int k = 0; k < arrayList3.Count; k++)
											{
												if (this.progressBar1.Value < this.progressBar1.Maximum)
												{
													this.progressBar1.Value++;
													Application.DoEvents();
												}
												string text2 = wgTools.SetObjToStr(arrayList3[k]);
												string text3 = (long.Parse(text2) + long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"]))).ToString();
												if (arrayList.IndexOf(text3) >= 0 && this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), "GetEmployee(id=\"" + text2 + "\")", ref text, ref array) > 0 && text.IndexOf("face_data=\"") > 0 && this.updateDBFaceInfo(long.Parse(text2), text, long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"]))) > 0)
												{
													num2++;
												}
											}
											flag = true;
											this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[{2}/{3}] ", new object[]
											{
												(sender as Button).Text,
												CommonStr.strSuccessfully,
												num2,
												arrayList3.Count
											}));
										}
									}
									if (!flag)
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
										{
											CommonStr.strCommFail,
											wgTools.SetObjToStr(dataRowView2["f_DeviceName"]),
											wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
											wgTools.SetObjToStr(dataRowView2["f_DevicePort"])
										}), 101);
									}
									this.displayNewestLog();
								}
							}
							if (arrayList2.Count == dataView.Count)
							{
								break;
							}
							Thread.Sleep(300);
						}
					}
					if (arrayList2.Count != dataView.Count)
					{
						foreach (object obj3 in dataView)
						{
							DataRowView dataRowView3 = (DataRowView)obj3;
							if (arrayList2.IndexOf(dataRowView3["f_DeviceName"]) < 0)
							{
								this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView3["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
								{
									CommonStr.strCommFail,
									wgTools.SetObjToStr(dataRowView3["f_DeviceName"]),
									wgTools.SetObjToStr(dataRowView3["f_DeviceIP"]),
									wgTools.SetObjToStr(dataRowView3["f_DevicePort"])
								}), 101);
							}
						}
					}
					this.tcpClose();
					icConsumerShare.setUpdateLog();
					this.displayNewestLog();
					this.updateUserPhoto();
					this.dfrmWait1.Hide();
					wgAppConfig.wgLog(string.Concat(new string[]
					{
						(sender as Button).Text.Replace("\r\n", ""),
						" ,",
						CommonStr.strUsersNum,
						" = ",
						this.dgvSelectedUsers.RowCount.ToString(),
						",",
						CommonStr.strDevicesNum,
						" = ",
						this.dgvSelectedDoors.RowCount.ToString(),
						",",
						CommonStr.strOperationComplete
					}), EventLogEntryType.Information, null);
					Cursor.Current = Cursors.Default;
					this.progressBar1.Value = this.progressBar1.Maximum;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					return;
				}
				finally
				{
					this.dfrmWait1.Hide();
				}
				this.displayNewestLog();
			}
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0017C0D0 File Offset: 0x0017B0D0
		private void btnExit_Click(object sender, EventArgs e)
		{
			this.bExit = true;
			base.Close();
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0017C0E0 File Offset: 0x0017B0E0
		private void btnHaveFace_Click(object sender, EventArgs e)
		{
			this.btnDelAllUsers_Click(null, null);
			using (DataTable table = ((DataView)this.dgvUsers.DataSource).Table)
			{
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (!string.IsNullOrEmpty(wgTools.SetObjToStr(table.Rows[i]["f_CardNO"])))
					{
						if (sender == this.btnHaveFace)
						{
							if ((int)table.Rows[i]["f_Other"] > 0)
							{
								table.Rows[i]["f_Selected"] = icConsumerShare.iSelectedCurrentNoneMax + 1;
							}
						}
						else if (sender == this.btnNoFace && (int)table.Rows[i]["f_Other"] == 0)
						{
							table.Rows[i]["f_Selected"] = icConsumerShare.iSelectedCurrentNoneMax + 1;
						}
					}
				}
			}
			this.updateButtonDisplay();
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x0017C204 File Offset: 0x0017B204
		private void btnUploadAllUsers_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0017C208 File Offset: 0x0017B208
		private void btnUploadSelectedUsers_Click(object sender, EventArgs e)
		{
			bool flag = false;
			if (this.btnUploadSelectedUsers.Enabled && XMessageBox.Show(this, string.Format(string.Concat(new string[]
			{
				CommonStr.strAreYouSure,
				" {0}?\r\n\r\n",
				CommonStr.strUsersNum,
				" = ",
				this.dgvSelectedUsers.RowCount.ToString(),
				"\r\n\r\n",
				CommonStr.strDevicesNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString()
			}), (sender as Button).Text.Replace("\r\n", "")), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				if (sender == this.btnDeleteSelectedUsersFromSelectedDevice)
				{
					flag = true;
				}
				try
				{
					DataTable dataTable = new DataTable("face");
					string text = string.Format(" SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, {0:d} as f_Selected, f_GroupID, f_DoorEnabled, f_AttendEnabled  ", 0) + " FROM t_b_Consumer WHERE f_CardNO>0  ORDER BY f_ConsumerNO ASC ";
					if (wgAppConfig.IsAccessDB)
					{
						using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
						{
							using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
							{
								using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
								{
									oleDbDataAdapter.Fill(dataTable);
								}
							}
							goto IL_01A6;
						}
					}
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
						{
							using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
							{
								sqlDataAdapter.Fill(dataTable);
							}
						}
					}
					IL_01A6:
					ArrayList arrayList = new ArrayList();
					ArrayList arrayList2 = new ArrayList();
					ArrayList arrayList3 = new ArrayList();
					ArrayList arrayList4 = new ArrayList();
					ArrayList arrayList5 = new ArrayList();
					int num = 0;
					int num2 = 0;
					string text2 = "";
					if (this.dgvSelectedUsers.Rows.Count > 10)
					{
						Application.DoEvents();
						Thread.Sleep(10);
					}
					this.progressBar1.Value = 0;
					this.dfrmWait1.Show();
					this.dfrmWait1.Refresh();
					this.progressBar1.Maximum = this.dgvSelectedUsers.Rows.Count;
					long num3 = 0L;
					while (num < this.dgvSelectedUsers.Rows.Count && ((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count >= this.dgvSelectedUsers.Rows.Count)
					{
						if (!long.TryParse(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_ConsumerNO"]), out num3))
						{
							XMessageBox.Show("海康设备要求 工号必须是不超过8位的数字!\r\n\r\n" + wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_ConsumerNO"]), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
						if (num3 > 99999999L)
						{
							XMessageBox.Show("海康设备要求 工号必须是不超过8位的数字!\r\n\r\n" + wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_ConsumerNO"]), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
						if (long.TryParse(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_CardNO"]), out num3) && num3 > (long)((ulong)(-1)))
						{
							XMessageBox.Show("海康设备要求 卡号必须为不超过10位的数字!\r\n\r\n" + wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_CardNO"]), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
						if (this.checkValidFaceId(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_CardNO"]), this.getValidUserName(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_ConsumerName"]))) == 0)
						{
							if (num2 > 0)
							{
								text2 += ",";
							}
							text2 += wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_ConsumerName"]);
							num2++;
						}
						else if (!string.IsNullOrEmpty(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_CardNO"])))
						{
							arrayList.Add(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_CardNO"]));
							arrayList2.Add(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_ConsumerNO"]));
							arrayList3.Add(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_ConsumerID"]));
							arrayList4.Add(this.getValidUserName(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_ConsumerName"])));
							arrayList5.Add(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_Other"]));
						}
						num++;
						this.progressBar1.Value = num;
					}
					wgAppConfig.wgLog(string.Concat(new string[]
					{
						(sender as Button).Text,
						" ,",
						CommonStr.strUsersNum,
						" = ",
						this.dgvSelectedUsers.RowCount.ToString(),
						",",
						CommonStr.strDevicesNum,
						" = ",
						this.dgvSelectedDoors.RowCount.ToString()
					}));
					this.logOperate(sender);
					this.progressBar1.Value = 0;
					this.dfrmWait1.Show();
					this.dfrmWait1.Refresh();
					this.progressBar1.Maximum = this.dgvSelectedDoors.Rows.Count * Math.Max(arrayList.Count, 1);
					Application.DoEvents();
					new ArrayList();
					new ArrayList();
					DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
					dataView.RowFilter = "f_Selected > 0";
					DateTime dateTime = DateTime.Now.AddSeconds(3.0);
					this.arrPingInfoReset();
					foreach (object obj in dataView)
					{
						DataRowView dataRowView = (DataRowView)obj;
						this.pingControllerIP(wgTools.SetObjToStr(dataRowView["f_DeviceIP"]));
					}
					wgTools.WriteLine("bExistIPNet");
					Thread.Sleep(100);
					while (DateTime.Now < dateTime && this.arrIPARPOK.Count == 0 && this.arrIPPingOK.Count == 0)
					{
						Thread.Sleep(100);
					}
					wgTools.WriteLine("(DateTime.Now < dtPingEnd)");
					ArrayList arrayList6 = new ArrayList();
					if (this.arrIPARPOK.Count != 0 || this.arrIPPingOK.Count != 0)
					{
						for (int i = 0; i < 2; i++)
						{
							foreach (object obj2 in dataView)
							{
								DataRowView dataRowView2 = (DataRowView)obj2;
								if (this.IsIPCanConnect(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"])) && arrayList6.IndexOf(dataRowView2["f_DeviceName"]) < 0)
								{
									arrayList6.Add(dataRowView2["f_DeviceName"]);
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), CommonStr.strStart);
									if (this.progressBar1.Value < this.progressBar1.Maximum)
									{
										this.progressBar1.Value++;
										Application.DoEvents();
									}
									string text3 = "";
									byte[] array = null;
									int num4 = 0;
									int num5 = 0;
									bool flag2 = false;
									string text4 = "";
									for (int j = 0; j < arrayList.Count; j++)
									{
										long workNOOnDevice = this.getWorkNOOnDevice(long.Parse(wgTools.SetObjToStr(arrayList[j])), long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"])));
										int num6 = int.Parse(wgTools.SetObjToStr(arrayList2[j]));
										text4 = wgTools.SetObjToStr(arrayList4[j]);
										wgTools.SetObjToStr(workNOOnDevice);
										string text5 = "";
										if (this.progressBar1.Value < this.progressBar1.Maximum)
										{
											this.progressBar1.Value++;
											Application.DoEvents();
										}
										string text6 = "";
										if (arrayList5[j].ToString().Equals("1"))
										{
											text6 = workNOOnDevice.ToString();
										}
										if (flag)
										{
											text6 = "";
										}
										int num7 = this.updateOneUserByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"]), (long)num6, workNOOnDevice, text4, text6, ref text3, ref array);
										if (num7 < 0)
										{
											Thread.Sleep(100);
											wgAppConfig.wgDebugWrite("updateOneUserByHTTP  again");
											num7 = this.updateOneUserByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"]), (long)num6, workNOOnDevice, text4, text6, ref text3, ref array);
										}
										if (num7 <= 0)
										{
											flag2 = true;
											wgAppConfig.wgLog(string.Format("DeviceIP={0},DevicePort={1},Answer={2},recvInfo={3}, Command={4}", new object[]
											{
												wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
												int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])),
												wgTools.SetObjToStr(text3),
												(array == null) ? "" : Encoding.GetEncoding(936).GetString(array),
												text5
											}));
											break;
										}
										num4++;
										Thread.Sleep(50);
										if (this.bExit)
										{
											if (this.m_lUserID4updateOneUser != -1)
											{
												CHCNetSDK.NET_DVR_Logout_V30(this.m_lUserID4updateOneUser);
												this.m_lUserID4updateOneUser = -1;
											}
											return;
										}
									}
									if (this.m_lUserID4updateOneUser != -1)
									{
										CHCNetSDK.NET_DVR_Logout_V30(this.m_lUserID4updateOneUser);
										this.m_lUserID4updateOneUser = -1;
									}
									bool flag3 = true;
									if (flag2)
									{
										if (!string.IsNullOrEmpty(text4))
										{
											this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}. ({2}:{3})", new object[]
											{
												(sender as Button).Text,
												CommonStr.strFailed,
												CommonStr.strPhotoCheckAgain,
												text4
											}), 101);
										}
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[U{2}](T{4})", new object[]
										{
											(sender as Button).Text,
											CommonStr.strFailed,
											num4,
											num5,
											arrayList.Count
										}), 101);
									}
									else
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[U{2}] ", new object[]
										{
											(sender as Button).Text,
											CommonStr.strSuccessfully,
											num4,
											num5
										}));
									}
									if (!flag3)
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
										{
											CommonStr.strCommFail,
											wgTools.SetObjToStr(dataRowView2["f_DeviceName"]),
											wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
											wgTools.SetObjToStr(dataRowView2["f_DevicePort"])
										}), 101);
									}
									this.displayNewestLog();
								}
							}
							if (arrayList6.Count == dataView.Count)
							{
								break;
							}
							Thread.Sleep(300);
						}
					}
					if (arrayList6.Count != dataView.Count)
					{
						foreach (object obj3 in dataView)
						{
							DataRowView dataRowView3 = (DataRowView)obj3;
							if (arrayList6.IndexOf(dataRowView3["f_DeviceName"]) < 0)
							{
								this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView3["f_DeviceName"]), (sender as Button).Text), string.Format("{0}--{1}:{2}:{3}", new object[]
								{
									CommonStr.strCommFail,
									wgTools.SetObjToStr(dataRowView3["f_DeviceName"]),
									wgTools.SetObjToStr(dataRowView3["f_DeviceIP"]),
									wgTools.SetObjToStr(dataRowView3["f_DevicePort"])
								}), 101);
							}
						}
					}
					icConsumerShare.setUpdateLog();
					this.displayNewestLog();
					this.dfrmWait1.Hide();
					wgAppConfig.wgLog(string.Concat(new string[]
					{
						(sender as Button).Text.Replace("\r\n", ""),
						" ,",
						CommonStr.strUsersNum,
						" = ",
						this.dgvSelectedUsers.RowCount.ToString(),
						",",
						CommonStr.strDevicesNum,
						" = ",
						this.dgvSelectedDoors.RowCount.ToString(),
						",",
						CommonStr.strOperationComplete
					}), EventLogEntryType.Information, null);
					Cursor.Current = Cursors.Default;
					this.progressBar1.Value = this.progressBar1.Maximum;
					if (num2 > 0)
					{
						wgAppConfig.wgLog(string.Concat(new string[]
						{
							CommonStr.strDevicesFaceOver60000Check,
							text2,
							"!!!(",
							num2.ToString(),
							") "
						}));
						XMessageBox.Show(string.Concat(new string[]
						{
							CommonStr.strDevicesFaceOver60000Check,
							text2,
							"!!!(",
							num2.ToString(),
							") "
						}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					return;
				}
				finally
				{
					this.dfrmWait1.Hide();
				}
				this.displayNewestLog();
			}
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0017D2F8 File Offset: 0x0017C2F8
		private void cbof_GroupID_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0017D328 File Offset: 0x0017C328
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.dgvUsers.DataSource != null)
			{
				this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
				DataView dataView = (DataView)this.dgvUsers.DataSource;
				if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					dataView.RowFilter = icConsumerShare.getOptionalRowfilter();
					this.strGroupFilter = "";
				}
				else
				{
					dataView.RowFilter = "f_Selected = 0 AND f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
					int groupChildMaxNo = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
					if (num2 > 0)
					{
						if (num2 >= groupChildMaxNo)
						{
							dataView.RowFilter = string.Format("f_Selected = 0 AND f_GroupID ={0:d} ", num);
							this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num);
						}
						else
						{
							dataView.RowFilter = "f_Selected = 0 ";
							string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
							dataView.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", groupQuery);
							this.strGroupFilter = string.Format("  {0} ", groupQuery);
						}
					}
					dataView.RowFilter = string.Format("(f_DoorEnabled > 0) AND {0} AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				}
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				}
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
			}
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0017D578 File Offset: 0x0017C578
		public int checkByHTTP(string strIP, int PORT, string devicePassword)
		{
			CHCNetSDK.NET_DVR_USER_LOGIN_INFO net_DVR_USER_LOGIN_INFO = default(CHCNetSDK.NET_DVR_USER_LOGIN_INFO);
			CHCNetSDK.NET_DVR_DEVICEINFO_V40 net_DVR_DEVICEINFO_V = default(CHCNetSDK.NET_DVR_DEVICEINFO_V40);
			net_DVR_DEVICEINFO_V.struDeviceV30.sSerialNumber = new byte[48];
			net_DVR_USER_LOGIN_INFO.sDeviceAddress = strIP;
			net_DVR_USER_LOGIN_INFO.sUserName = this.deviceDefaultUser;
			net_DVR_USER_LOGIN_INFO.sPassword = this.deviceDefaultPassword;
			net_DVR_USER_LOGIN_INFO.wPort = (ushort)PORT;
			int num = CHCNetSDK.NET_DVR_Login_V40(ref net_DVR_USER_LOGIN_INFO, ref net_DVR_DEVICEINFO_V);
			if (num < 0)
			{
				wgAppConfig.wgLog("check  failed=" + CHCNetSDK.NET_DVR_GetLastError().ToString());
				return 0;
			}
			CHCNetSDK.NET_DVR_Logout_V30(num);
			return 1;
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0017D608 File Offset: 0x0017C608
		private int checkValidFaceId(string cardno, string consumername)
		{
			long num = 0L;
			if (!string.IsNullOrEmpty(cardno))
			{
				long.TryParse(cardno, out num);
			}
			if (num > 0L && num < (long)((ulong)(-1)))
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x0017D636 File Offset: 0x0017C636
		private void chkDeleteUsersNotInDB_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x0017D638 File Offset: 0x0017C638
		private void deleteSelectedUsersFromSelectedDevicesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnUploadSelectedUsers_Click(this.btnDeleteSelectedUsersFromSelectedDevice, e);
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0017D647 File Offset: 0x0017C647
		private void dfrmFaceManage_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0017D65C File Offset: 0x0017C65C
		private void dfrmFaceManage_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.defaultFindControl == null)
					{
						this.defaultFindControl = this.dgvUsers;
						base.ActiveControl = this.dgvUsers;
					}
					wgAppConfig.findCall(this.dfrmFind1, this, this.defaultFindControl);
				}
				if (e.Control && e.KeyValue == 81)
				{
					this.btnDeleteAll.Visible = true;
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0017D6F8 File Offset: 0x0017C6F8
		private void dfrmFaceManage_Load(object sender, EventArgs e)
		{
			try
			{
				CHCNetSDK.NET_DVR_Init();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.bFindActive = false;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			if (int.Parse("0" + wgAppConfig.getSystemParamByNO(209)) != 0)
			{
				string systemParamNotes = wgAppConfig.getSystemParamNotes(209);
				if (!string.IsNullOrEmpty(systemParamNotes))
				{
					this.deviceDefaultPassword = systemParamNotes.Substring(systemParamNotes.IndexOf(",") + 1);
				}
			}
			try
			{
				this.tbRunInfoLog = new DataTable();
				this.tbRunInfoLog.TableName = "runInfolog";
				this.tbRunInfoLog.Columns.Add("f_Category");
				this.tbRunInfoLog.Columns.Add("f_RecID");
				this.tbRunInfoLog.Columns.Add("f_Time");
				this.tbRunInfoLog.Columns.Add("f_Desc");
				this.tbRunInfoLog.Columns.Add("f_Info");
				this.tbRunInfoLog.Columns.Add("f_Detail");
				this.tbRunInfoLog.Columns.Add("f_MjRecStr");
				DataView dataView = new DataView(this.tbRunInfoLog);
				this.dgvRunInfo.AutoGenerateColumns = false;
				this.dgvRunInfo.DataSource = dataView;
				this.dgvRunInfo.Columns[0].DataPropertyName = "f_Category";
				this.dgvRunInfo.Columns[1].DataPropertyName = "f_RecID";
				this.dgvRunInfo.Columns[2].DataPropertyName = "f_Time";
				this.dgvRunInfo.Columns[3].DataPropertyName = "f_Desc";
				this.dgvRunInfo.Columns[4].DataPropertyName = "f_Info";
				this.dgvRunInfo.Columns[5].DataPropertyName = "f_Detail";
				this.dgvRunInfo.Columns[6].DataPropertyName = "f_MjRecStr";
				for (int i = 0; i < this.dgvRunInfo.ColumnCount; i++)
				{
					this.dgvRunInfo.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				}
				new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
				for (int j = 0; j < this.arrGroupID.Count; j++)
				{
					if (j == 0 && string.IsNullOrEmpty(this.arrGroupName[j].ToString()))
					{
						this.cbof_GroupID.Items.Add(CommonStr.strAll);
					}
					else
					{
						this.cbof_GroupID.Items.Add(this.arrGroupName[j].ToString());
					}
				}
				if (this.cbof_GroupID.Items.Count > 0)
				{
					this.cbof_GroupID.SelectedIndex = 0;
				}
				this._fillDeviceGrid();
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
			if (!this.backgroundWorker1.IsBusy)
			{
				this.backgroundWorker1.RunWorkerAsync();
			}
			new ToolStripMenuItem("C");
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			this.updateButtonDisplay();
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0017DB2C File Offset: 0x0017CB2C
		private void dfrmFaceManagement_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.dfrmWait1 != null)
				{
					this.dfrmWait1.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0017DB64 File Offset: 0x0017CB64
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0017DB74 File Offset: 0x0017CB74
		private void dgvRunInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && this.dgvRunInfo.Columns[e.ColumnIndex].Name.Equals("f_Category"))
				{
					string text = e.Value as string;
					if (text != null)
					{
						DataGridViewCell dataGridViewCell = this.dgvRunInfo[e.ColumnIndex, e.RowIndex];
						dataGridViewCell.ToolTipText = text;
						DataGridViewRow dataGridViewRow = this.dgvRunInfo.Rows[e.RowIndex];
						e.Value = InfoRow.getImage(text, ref dataGridViewRow);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0017DC24 File Offset: 0x0017CC24
		private void dgvRunInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			if (!this.bDataErrorExist)
			{
				this.bDataErrorExist = true;
				wgAppConfig.wgLog(string.Format("dgvRunInfo_DataError  ColumnIndex ={0}, RowIndex ={1}, exception={2}, Context ={3} ", new object[]
				{
					e.ColumnIndex,
					e.RowIndex,
					e.Exception.ToString(),
					e.Context
				}));
			}
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0017DC90 File Offset: 0x0017CC90
		private void dgvSelectedDoors_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0017DCC0 File Offset: 0x0017CCC0
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0017DCD0 File Offset: 0x0017CCD0
		private void dgvSelectedUsers_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0017DD00 File Offset: 0x0017CD00
		private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0017DD10 File Offset: 0x0017CD10
		private void dgvUsers_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0017DD40 File Offset: 0x0017CD40
		private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneUser.PerformClick();
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0017DD50 File Offset: 0x0017CD50
		private void displayNewestLog()
		{
			if (this.dgvRunInfo.Rows.Count > 0)
			{
				this.dgvRunInfo.FirstDisplayedScrollingRowIndex = this.dgvRunInfo.Rows.Count - 1;
				this.dgvRunInfo.Rows[this.dgvRunInfo.Rows.Count - 1].Selected = true;
				this.dgvRunInfo.Rows[this.dgvRunInfo.Rows.Count - 1].Selected = false;
				Application.DoEvents();
			}
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0017DDE2 File Offset: 0x0017CDE2
		private long getCardNOonDB(long WorkNO, long diff)
		{
			return WorkNO + diff;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0017DDE8 File Offset: 0x0017CDE8
		private string getConsumerNameInFace(string faceInfo)
		{
			string text = "name=\"";
			int num = faceInfo.IndexOf(text);
			if (num > 0)
			{
				num += text.Length;
				int num2 = faceInfo.IndexOf("\"", num);
				if (num2 > 0)
				{
					string text2 = faceInfo.Substring(num, num2 - num);
					if (!string.IsNullOrEmpty(text2))
					{
						return text2;
					}
				}
			}
			return "";
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0017DE3C File Offset: 0x0017CE3C
		private string getValidUserName(string user)
		{
			string text = "";
			try
			{
				string text2 = user.Replace("(", ".").Replace(")", ".");
				for (int i = 0; i < 8; i++)
				{
					text += text2.Substring(i, 1);
				}
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0017DEA4 File Offset: 0x0017CEA4
		private long getWorkNOOnDevice(long cardno, long diff)
		{
			if (cardno > diff)
			{
				return cardno - diff;
			}
			return cardno;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0017DEB0 File Offset: 0x0017CEB0
		private bool IsIPCanConnect(string ip)
		{
			return (this.arrIPPingOK.Count > 0 && this.arrIPPingOK.IndexOf(ip) >= 0) || (this.arrIPARPOK.Count > 0 && this.arrIPARPOK.IndexOf(ip) >= 0);
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0017DEFE File Offset: 0x0017CEFE
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0017DF20 File Offset: 0x0017CF20
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			try
			{
				this.dv.RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
				this.dvSelected.RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
				this.dgvUsers.AutoGenerateColumns = false;
				this.dgvUsers.DataSource = this.dv;
				this.dgvSelectedUsers.AutoGenerateColumns = false;
				this.dgvSelectedUsers.DataSource = this.dvSelected;
				int num = 0;
				while (num < this.dv.Table.Columns.Count && num < this.dgvUsers.ColumnCount)
				{
					this.dgvUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
					this.dgvSelectedUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
					num++;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				wgAppConfig.wgLog(ex.ToString());
			}
			wgTools.WriteLine("loadUserData End");
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0017E07C File Offset: 0x0017D07C
		private void logOperate(object sender)
		{
			string text = "";
			for (int i = 0; i <= Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedUsers.RowCount) - 1; i++)
			{
				text = text + ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerName"] + ",";
			}
			if (this.dgvSelectedUsers.RowCount > wgAppConfig.LogEventMaxCount)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"......(",
					this.dgvSelectedUsers.RowCount,
					")"
				});
			}
			else
			{
				object obj2 = text;
				text = string.Concat(new object[]
				{
					obj2,
					"(",
					this.dgvSelectedUsers.RowCount,
					")"
				});
			}
			string text2 = "";
			for (int i = 0; i <= Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedDoors.RowCount) - 1; i++)
			{
				text2 = text2 + ((DataView)this.dgvSelectedDoors.DataSource)[i]["f_DeviceName"] + ",";
			}
			if (this.dgvSelectedDoors.RowCount > wgAppConfig.LogEventMaxCount)
			{
				object obj3 = text2;
				text2 = string.Concat(new object[]
				{
					obj3,
					"......(",
					this.dgvSelectedDoors.RowCount,
					")"
				});
			}
			else
			{
				object obj4 = text2;
				text2 = string.Concat(new object[]
				{
					obj4,
					"(",
					this.dgvSelectedDoors.RowCount,
					")"
				});
			}
			wgAppConfig.wgLog(string.Format("{0}:[{1} => {2}]:{3} => {4}", new object[]
			{
				(sender as Button).Text.Replace("\r\n", ""),
				this.dgvSelectedUsers.RowCount.ToString(),
				this.dgvSelectedDoors.RowCount.ToString(),
				text,
				text2
			}), EventLogEntryType.Information, null);
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0017E2CC File Offset: 0x0017D2CC
		public bool pingControllerIP(string ip)
		{
			if (this.IsIPCanConnect(ip))
			{
				return true;
			}
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.pingControllerIPThread), ip);
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.pingControllerIPARPThread), ip);
			return false;
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0017E300 File Offset: 0x0017D300
		public void pingControllerIPARPThread(object ip)
		{
			byte[] array = new byte[6];
			uint num = (uint)array.Length;
			try
			{
				if (wgGlobal.SafeNativeMethods.SendARP(wgTools.IPLng(IPAddress.Parse(ip as string)), 0, array, ref num) == 0)
				{
					if (this.arrIPARPOK.IndexOf(ip as string) >= 0)
					{
						return;
					}
					lock (this.arrIPARPOK)
					{
						this.arrIPARPOK.Add(ip as string);
						return;
					}
				}
				this.arrIPARPFailed.Add(ip as string);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0017E3B8 File Offset: 0x0017D3B8
		public void pingControllerIPThread(object ip)
		{
			bool flag = false;
			try
			{
				Ping ping = new Ping();
				PingOptions pingOptions = new PingOptions();
				pingOptions.DontFragment = true;
				string text = "12345678901234567890123456789012";
				byte[] bytes = Encoding.ASCII.GetBytes(text);
				int num = 300;
				int num2 = 0;
				while (num2++ < 3)
				{
					if (ping.Send(ip as string, num, bytes, pingOptions).Status == IPStatus.Success)
					{
						flag = true;
						if (this.arrIPPingOK.IndexOf(ip as string) >= 0)
						{
							break;
						}
						lock (this.arrIPPingOK)
						{
							this.arrIPPingOK.Add(ip as string);
							break;
						}
					}
					num = 1000;
				}
				if (!flag)
				{
					this.arrIPPingFailed.Add(ip as string);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0017E4AC File Offset: 0x0017D4AC
		private void ProcessGetFaceParamCallback(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData)
		{
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0017E4AE File Offset: 0x0017D4AE
		private void ProcessSetFaceParamCallback(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData)
		{
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0017E4B0 File Offset: 0x0017D4B0
		private void ProcessSetGatewayCardCallback(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData)
		{
			uint num = (uint)Marshal.ReadInt32(lpBuffer);
			wgAppConfig.wgDebugWrite(num.ToString());
			switch (num)
			{
			case 1000U:
				CHCNetSDK.PostMessage(pUserData, 1001, 0, 0);
				return;
			case 1001U:
				CHCNetSDK.PostMessage(pUserData, 1002, 0, 0);
				return;
			case 1002U:
				CHCNetSDK.PostMessage(pUserData, 1002, 0, 0);
				return;
			case 1003U:
				CHCNetSDK.PostMessage(pUserData, 1001, 0, 0);
				return;
			default:
				return;
			}
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0017E530 File Offset: 0x0017D530
		public string PushToWeb(string weburl, string data, Encoding encode)
		{
			byte[] bytes = encode.GetBytes(data);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
			httpWebRequest.SendChunked = false;
			httpWebRequest.KeepAlive = false;
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.ContentLength = (long)bytes.Length;
			Stream requestStream = httpWebRequest.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			string text = new StreamReader(httpWebResponse.GetResponseStream(), encode).ReadToEnd();
			if (text.IndexOf("\"result\":1,\"success\":true") > 0)
			{
				return "1";
			}
			if (text.IndexOf("\"result\":1,\"success\":false") > 0)
			{
				text = "0";
			}
			return text;
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0017E5E8 File Offset: 0x0017D5E8
		private bool SendCardData(uint dwDiffTime = 0U, dfrmFaceManagement4Hikvision.Person person = null)
		{
			if (-1 != this.m_lSetCardCfgHandle)
			{
				CHCNetSDK.NET_DVR_CARD_CFG_V50 net_DVR_CARD_CFG_V = default(CHCNetSDK.NET_DVR_CARD_CFG_V50);
				uint num = (uint)Marshal.SizeOf(net_DVR_CARD_CFG_V);
				net_DVR_CARD_CFG_V.dwSize = (uint)Marshal.SizeOf(net_DVR_CARD_CFG_V);
				IntPtr intPtr = Marshal.AllocHGlobal((int)num);
				net_DVR_CARD_CFG_V.Init();
				byte[] array = new byte[32];
				string text = string.Empty;
				if (person != null)
				{
					text = person.name;
					array = Encoding.Default.GetBytes(text);
					for (int i = 0; i < array.Length; i++)
					{
						net_DVR_CARD_CFG_V.byName[i] = array[i];
					}
					string text2 = string.Empty;
					if (person == null)
					{
						return false;
					}
					text2 = person.cardId;
					byte[] array2 = new byte[32];
					array2 = Encoding.UTF8.GetBytes(text2);
					for (int j = 0; j < array2.Length; j++)
					{
						net_DVR_CARD_CFG_V.byCardNo[j] = array2[j];
					}
					string text3 = string.Empty;
					if (person == null)
					{
						return false;
					}
					text3 = person.employeeId;
					net_DVR_CARD_CFG_V.dwModifyParamType = uint.MaxValue;
					net_DVR_CARD_CFG_V.dwEmployeeNo = Convert.ToUInt32(text3);
					net_DVR_CARD_CFG_V.byCardValid = 1;
					if (person.bDeleted)
					{
						net_DVR_CARD_CFG_V.byCardValid = 0;
					}
					net_DVR_CARD_CFG_V.dwCardRight = 1U;
					net_DVR_CARD_CFG_V.byCardType = 1;
					net_DVR_CARD_CFG_V.byDoorRight[0] = 1;
					net_DVR_CARD_CFG_V.dwPlanTemplate = 1U;
					net_DVR_CARD_CFG_V.struValid.byEnable = 1;
					net_DVR_CARD_CFG_V.struValid.struBeginTime.wYear = 2000;
					net_DVR_CARD_CFG_V.struValid.struBeginTime.byMonth = 1;
					net_DVR_CARD_CFG_V.struValid.struBeginTime.byDay = 1;
					net_DVR_CARD_CFG_V.struValid.struBeginTime.byHour = 0;
					net_DVR_CARD_CFG_V.struValid.struBeginTime.byMinute = 0;
					net_DVR_CARD_CFG_V.struValid.struBeginTime.bySecond = 0;
					net_DVR_CARD_CFG_V.struValid.struEndTime.wYear = 2037;
					net_DVR_CARD_CFG_V.struValid.struEndTime.byMonth = 12;
					net_DVR_CARD_CFG_V.struValid.struEndTime.byDay = 31;
					net_DVR_CARD_CFG_V.struValid.struEndTime.byHour = 0;
					net_DVR_CARD_CFG_V.struValid.struEndTime.byMinute = 0;
					net_DVR_CARD_CFG_V.struValid.struEndTime.bySecond = 0;
					net_DVR_CARD_CFG_V.wCardRightPlan[0] = 1;
					Marshal.StructureToPtr(net_DVR_CARD_CFG_V, intPtr, false);
					if (!CHCNetSDK.NET_DVR_SendRemoteConfig(this.m_lSetCardCfgHandle, 3U, intPtr, num))
					{
						Marshal.FreeHGlobal(intPtr);
						return false;
					}
					Marshal.FreeHGlobal(intPtr);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0017E85C File Offset: 0x0017D85C
		private int SendCardData(long cardNO, string username, int m_lSetCardCfgHandle, string jpgName, uint dwDiffTime = 0U)
		{
			CHCNetSDK.NET_DVR_CARD_CFG_V50 net_DVR_CARD_CFG_V = default(CHCNetSDK.NET_DVR_CARD_CFG_V50);
			uint num = (uint)Marshal.SizeOf(net_DVR_CARD_CFG_V);
			net_DVR_CARD_CFG_V.dwSize = (uint)Marshal.SizeOf(net_DVR_CARD_CFG_V);
			IntPtr intPtr = Marshal.AllocHGlobal((int)num);
			net_DVR_CARD_CFG_V.Init();
			byte[] array = new byte[32];
			array = Encoding.Default.GetBytes(username);
			for (int i = 0; i < array.Length; i++)
			{
				net_DVR_CARD_CFG_V.byName[i] = array[i];
			}
			byte[] array2 = new byte[32];
			array2 = Encoding.UTF8.GetBytes(cardNO.ToString());
			for (int j = 0; j < array2.Length; j++)
			{
				net_DVR_CARD_CFG_V.byCardNo[j] = array2[j];
			}
			net_DVR_CARD_CFG_V.dwModifyParamType = uint.MaxValue;
			net_DVR_CARD_CFG_V.dwEmployeeNo = Convert.ToUInt32(cardNO);
			net_DVR_CARD_CFG_V.byCardValid = 1;
			net_DVR_CARD_CFG_V.dwCardRight = 1U;
			net_DVR_CARD_CFG_V.byCardType = 1;
			net_DVR_CARD_CFG_V.byDoorRight[0] = 1;
			net_DVR_CARD_CFG_V.dwPlanTemplate = 1U;
			net_DVR_CARD_CFG_V.struValid.byEnable = 1;
			net_DVR_CARD_CFG_V.struValid.struBeginTime.wYear = 2000;
			net_DVR_CARD_CFG_V.struValid.struBeginTime.byMonth = 1;
			net_DVR_CARD_CFG_V.struValid.struBeginTime.byDay = 1;
			net_DVR_CARD_CFG_V.struValid.struBeginTime.byHour = 0;
			net_DVR_CARD_CFG_V.struValid.struBeginTime.byMinute = 0;
			net_DVR_CARD_CFG_V.struValid.struBeginTime.bySecond = 0;
			net_DVR_CARD_CFG_V.struValid.struEndTime.wYear = 2037;
			net_DVR_CARD_CFG_V.struValid.struEndTime.byMonth = 12;
			net_DVR_CARD_CFG_V.struValid.struEndTime.byDay = 31;
			net_DVR_CARD_CFG_V.struValid.struEndTime.byHour = 0;
			net_DVR_CARD_CFG_V.struValid.struEndTime.byMinute = 0;
			net_DVR_CARD_CFG_V.struValid.struEndTime.bySecond = 0;
			net_DVR_CARD_CFG_V.wCardRightPlan[0] = 1;
			if (string.IsNullOrEmpty(jpgName))
			{
				net_DVR_CARD_CFG_V.byCardValid = 0;
			}
			Marshal.StructureToPtr(net_DVR_CARD_CFG_V, intPtr, false);
			if (!CHCNetSDK.NET_DVR_SendRemoteConfig(m_lSetCardCfgHandle, 3U, intPtr, num))
			{
				Marshal.FreeHGlobal(intPtr);
				return -1;
			}
			Marshal.FreeHGlobal(intPtr);
			return 1;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0017EA80 File Offset: 0x0017DA80
		private bool SendCardInfo(dfrmFaceManagement4Hikvision.Person person)
		{
			if (-1 != this.m_lSetCardCfgHandle && CHCNetSDK.NET_DVR_StopRemoteConfig(this.m_lSetCardCfgHandle))
			{
				this.m_lSetCardCfgHandle = -1;
			}
			CHCNetSDK.NET_DVR_CARD_CFG_COND net_DVR_CARD_CFG_COND = default(CHCNetSDK.NET_DVR_CARD_CFG_COND);
			net_DVR_CARD_CFG_COND.dwSize = (uint)Marshal.SizeOf(net_DVR_CARD_CFG_COND);
			net_DVR_CARD_CFG_COND.dwCardNum = 1U;
			net_DVR_CARD_CFG_COND.byCheckCardNo = 1;
			net_DVR_CARD_CFG_COND.wLocalControllerID = 0;
			int num = Marshal.SizeOf(net_DVR_CARD_CFG_COND);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr(net_DVR_CARD_CFG_COND, intPtr, false);
			this.g_fSetGatewayCardCallback = new CHCNetSDK.RemoteConfigCallback(this.ProcessSetGatewayCardCallback);
			this.m_lSetCardCfgHandle = CHCNetSDK.NET_DVR_StartRemoteConfig(this.m_lUserID, 2179U, intPtr, num, this.g_fSetGatewayCardCallback, IntPtr.Zero);
			if (-1 == this.m_lSetCardCfgHandle)
			{
				wgAppConfig.wgLog("NET_DVR_StartRemoteConfig Failed 111");
				Marshal.FreeHGlobal(intPtr);
				return false;
			}
			Marshal.FreeHGlobal(intPtr);
			this.SendCardData(0U, person);
			return true;
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0017EB5C File Offset: 0x0017DB5C
		private int SendFaceData(long cardNO, string jpgName, int m_lUserID, uint dwDiffTime = 0U)
		{
			CHCNetSDK.NET_DVR_FACE_PARAM_COND net_DVR_FACE_PARAM_COND = default(CHCNetSDK.NET_DVR_FACE_PARAM_COND);
			net_DVR_FACE_PARAM_COND.dwSize = (uint)Marshal.SizeOf(net_DVR_FACE_PARAM_COND);
			net_DVR_FACE_PARAM_COND.dwFaceNum = 1U;
			net_DVR_FACE_PARAM_COND.byFaceID = 1;
			net_DVR_FACE_PARAM_COND.byEnableCardReader = new byte[512];
			net_DVR_FACE_PARAM_COND.byEnableCardReader[0] = 1;
			net_DVR_FACE_PARAM_COND.byCardNo = new byte[32];
			byte[] bytes = Encoding.UTF8.GetBytes(cardNO.ToString());
			for (int i = 0; i < bytes.Length; i++)
			{
				if (i > net_DVR_FACE_PARAM_COND.byCardNo.Length)
				{
					MessageBox.Show("card number length too long!");
					return -1;
				}
				net_DVR_FACE_PARAM_COND.byCardNo[i] = bytes[i];
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(net_DVR_FACE_PARAM_COND));
			Marshal.StructureToPtr(net_DVR_FACE_PARAM_COND, intPtr, false);
			net_DVR_FACE_PARAM_COND = default(CHCNetSDK.NET_DVR_FACE_PARAM_COND);
			net_DVR_FACE_PARAM_COND.dwSize = (uint)Marshal.SizeOf(net_DVR_FACE_PARAM_COND);
			net_DVR_FACE_PARAM_COND.dwFaceNum = 1U;
			net_DVR_FACE_PARAM_COND.byFaceID = 1;
			net_DVR_FACE_PARAM_COND.byEnableCardReader = new byte[512];
			net_DVR_FACE_PARAM_COND.byEnableCardReader[0] = 1;
			net_DVR_FACE_PARAM_COND.byCardNo = new byte[32];
			for (int j = 0; j < bytes.Length; j++)
			{
				if (j > net_DVR_FACE_PARAM_COND.byCardNo.Length)
				{
					MessageBox.Show("card number length too long!");
					return -1;
				}
				net_DVR_FACE_PARAM_COND.byCardNo[j] = bytes[j];
			}
			int num = Marshal.SizeOf(net_DVR_FACE_PARAM_COND);
			intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr(net_DVR_FACE_PARAM_COND, intPtr, false);
			this.g_fSetFaceParamCallback = new CHCNetSDK.RemoteConfigCallback(this.ProcessSetFaceParamCallback);
			int num2 = CHCNetSDK.NET_DVR_StartRemoteConfig(m_lUserID, 2508U, intPtr, num, this.g_fSetFaceParamCallback, base.Handle);
			int num3;
			if (-1 == num2)
			{
				num3 = -1;
			}
			else
			{
				num3 = 1;
			}
			Marshal.FreeHGlobal(intPtr);
			if (num3 > 0)
			{
				CHCNetSDK.NET_DVR_FACE_PARAM_CFG net_DVR_FACE_PARAM_CFG = default(CHCNetSDK.NET_DVR_FACE_PARAM_CFG);
				net_DVR_FACE_PARAM_CFG.dwSize = (uint)Marshal.SizeOf(net_DVR_FACE_PARAM_CFG);
				net_DVR_FACE_PARAM_CFG.byFaceID = 1;
				net_DVR_FACE_PARAM_CFG.byFaceDataType = 1;
				net_DVR_FACE_PARAM_CFG.byEnableCardReader = new byte[512];
				net_DVR_FACE_PARAM_CFG.byEnableCardReader[0] = 1;
				net_DVR_FACE_PARAM_CFG.byCardNo = new byte[32];
				byte[] bytes2 = Encoding.UTF8.GetBytes(cardNO.ToString());
				for (int k = 0; k < bytes2.Length; k++)
				{
					if (k > net_DVR_FACE_PARAM_CFG.byCardNo.Length)
					{
						MessageBox.Show("card number length too long!");
						return -1;
					}
					net_DVR_FACE_PARAM_CFG.byCardNo[k] = bytes2[k];
				}
				if (!File.Exists(wgAppConfig.Path4Photo() + jpgName + ".jpg"))
				{
					MessageBox.Show("The face picture does not exist!");
					return -1;
				}
				FileStream fileStream = new FileStream(wgAppConfig.Path4Photo() + jpgName + ".jpg", FileMode.OpenOrCreate);
				if (0L == fileStream.Length)
				{
					MessageBox.Show("The face picture is 0k,please input another picture!");
					return -1;
				}
				if (204800L < fileStream.Length)
				{
					MessageBox.Show("The face picture is larger than 200k,please input another picture!");
					return -1;
				}
				net_DVR_FACE_PARAM_CFG.dwFaceLen = (uint)fileStream.Length;
				int dwFaceLen = (int)net_DVR_FACE_PARAM_CFG.dwFaceLen;
				byte[] array = new byte[dwFaceLen];
				net_DVR_FACE_PARAM_CFG.pFaceBuffer = Marshal.AllocHGlobal(dwFaceLen);
				fileStream.Read(array, 0, dwFaceLen);
				Marshal.Copy(array, 0, net_DVR_FACE_PARAM_CFG.pFaceBuffer, dwFaceLen);
				fileStream.Close();
				uint num4 = (uint)Marshal.SizeOf(net_DVR_FACE_PARAM_CFG);
				IntPtr intPtr2 = Marshal.AllocHGlobal((int)num4);
				Marshal.StructureToPtr(net_DVR_FACE_PARAM_CFG, intPtr2, false);
				if (!CHCNetSDK.NET_DVR_SendRemoteConfig(num2, 9U, intPtr2, num4))
				{
					num3 = -1;
				}
				Marshal.FreeHGlobal(net_DVR_FACE_PARAM_CFG.pFaceBuffer);
				Marshal.FreeHGlobal(intPtr2);
				CHCNetSDK.NET_DVR_StopRemoteConfig(num2);
			}
			return num3;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0017EED8 File Offset: 0x0017DED8
		private bool SendPhotoInfo(dfrmFaceManagement4Hikvision.Person person, FileInfo photo)
		{
			if (-1 != this.m_lSetFaceCfgHandle && CHCNetSDK.NET_DVR_StopRemoteConfig(this.m_lSetFaceCfgHandle))
			{
				this.m_lSetFaceCfgHandle = -1;
			}
			CHCNetSDK.NET_DVR_FACE_PARAM_COND net_DVR_FACE_PARAM_COND = default(CHCNetSDK.NET_DVR_FACE_PARAM_COND);
			net_DVR_FACE_PARAM_COND.dwSize = (uint)Marshal.SizeOf(net_DVR_FACE_PARAM_COND);
			net_DVR_FACE_PARAM_COND.dwFaceNum = 1U;
			net_DVR_FACE_PARAM_COND.byFaceID = 1;
			net_DVR_FACE_PARAM_COND.byEnableCardReader = new byte[512];
			net_DVR_FACE_PARAM_COND.byEnableCardReader[0] = 1;
			if ("" == person.cardId)
			{
				return false;
			}
			net_DVR_FACE_PARAM_COND.byCardNo = new byte[32];
			byte[] bytes = Encoding.UTF8.GetBytes(person.cardId);
			for (int i = 0; i < bytes.Length; i++)
			{
				if (i > net_DVR_FACE_PARAM_COND.byCardNo.Length)
				{
					wgAppConfig.wgLog("card number length too long!");
					return false;
				}
				net_DVR_FACE_PARAM_COND.byCardNo[i] = bytes[i];
			}
			int num = Marshal.SizeOf(net_DVR_FACE_PARAM_COND);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr(net_DVR_FACE_PARAM_COND, intPtr, false);
			this.g_fSetFaceParamCallback = new CHCNetSDK.RemoteConfigCallback(this.ProcessSetFaceParamCallback);
			this.m_lSetFaceCfgHandle = CHCNetSDK.NET_DVR_StartRemoteConfig(this.m_lUserID, 2508U, intPtr, num, this.g_fSetFaceParamCallback, IntPtr.Zero);
			if (-1 == this.m_lSetFaceCfgHandle)
			{
				return false;
			}
			Marshal.FreeHGlobal(intPtr);
			CHCNetSDK.NET_DVR_FACE_PARAM_CFG net_DVR_FACE_PARAM_CFG = default(CHCNetSDK.NET_DVR_FACE_PARAM_CFG);
			net_DVR_FACE_PARAM_CFG.dwSize = (uint)Marshal.SizeOf(net_DVR_FACE_PARAM_CFG);
			net_DVR_FACE_PARAM_CFG.byFaceID = 1;
			net_DVR_FACE_PARAM_CFG.byFaceDataType = 1;
			net_DVR_FACE_PARAM_CFG.byEnableCardReader = new byte[512];
			net_DVR_FACE_PARAM_CFG.byEnableCardReader[0] = 1;
			net_DVR_FACE_PARAM_CFG.byCardNo = new byte[32];
			byte[] bytes2 = Encoding.UTF8.GetBytes(person.cardId);
			for (int j = 0; j < bytes2.Length; j++)
			{
				if (j > net_DVR_FACE_PARAM_CFG.byCardNo.Length)
				{
					return false;
				}
				net_DVR_FACE_PARAM_CFG.byCardNo[j] = bytes2[j];
			}
			FileStream fileStream = photo.OpenRead();
			if (0L == fileStream.Length)
			{
				wgAppConfig.wgLog("The face picture is 0k,please input another picture!");
				return false;
			}
			if (204800L < fileStream.Length)
			{
				wgAppConfig.wgLog("The face picture is larger than 200k,please input another picture!");
				return false;
			}
			net_DVR_FACE_PARAM_CFG.dwFaceLen = (uint)fileStream.Length;
			int dwFaceLen = (int)net_DVR_FACE_PARAM_CFG.dwFaceLen;
			byte[] array = new byte[dwFaceLen];
			net_DVR_FACE_PARAM_CFG.pFaceBuffer = Marshal.AllocHGlobal(dwFaceLen);
			fileStream.Read(array, 0, dwFaceLen);
			Marshal.Copy(array, 0, net_DVR_FACE_PARAM_CFG.pFaceBuffer, dwFaceLen);
			fileStream.Close();
			uint num2 = (uint)Marshal.SizeOf(net_DVR_FACE_PARAM_CFG);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)num2);
			Marshal.StructureToPtr(net_DVR_FACE_PARAM_CFG, intPtr2, false);
			if (!CHCNetSDK.NET_DVR_SendRemoteConfig(this.m_lSetFaceCfgHandle, 9U, intPtr2, num2))
			{
				wgAppConfig.wgLog("set face picture  Failed 113");
			}
			Marshal.FreeHGlobal(net_DVR_FACE_PARAM_CFG.pFaceBuffer);
			Marshal.FreeHGlobal(intPtr2);
			return true;
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0017F192 File Offset: 0x0017E192
		private void tcpClose()
		{
			if (this.tcp != null)
			{
				this.tcp.Close();
				this.tcp = null;
			}
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0017F1B0 File Offset: 0x0017E1B0
		public int ThirdPartyDeviceFaceCommIP_TCP(string strIP, int PORT, string command, ref string strRecvInfo, ref byte[] recvInfo)
		{
			recvInfo = null;
			strRecvInfo = "";
			if (this.tcp == null)
			{
				try
				{
					this.tcp = new TcpClient();
					this.tcp.Connect(IPAddress.Parse(strIP), PORT);
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					this.tcp = null;
				}
			}
			if (this.tcp != null)
			{
				byte[] array = null;
				this.tcp.ReceiveBufferSize = this.tcpBuffSize;
				this.tcp.SendBufferSize = this.tcpBuffSize;
				NetworkStream stream = this.tcp.GetStream();
				if (stream.CanWrite)
				{
					byte[] bytes = Encoding.GetEncoding(936).GetBytes(command);
					stream.Write(bytes, 0, bytes.Length);
					DateTime dateTime = DateTime.Now.AddSeconds(5.0);
					if (this.nametablecnt >= 10)
					{
						dateTime = DateTime.Now.AddSeconds((double)(5 + 3 * this.nametablecnt / 10));
						this.nametablecnt = 0;
					}
					byte[] array2 = new byte[this.tcpBuffSize];
					int num = 0;
					int num2 = 0;
					while (dateTime > DateTime.Now)
					{
						if (stream.CanRead)
						{
							if (stream.DataAvailable)
							{
								int num3 = stream.Read(array2, num, array2.Length - num);
								if (Array.IndexOf<byte>(array2, 41, num, num3) >= 0)
								{
									num = Array.IndexOf<byte>(array2, 41, num, num3) + 1;
									break;
								}
								num += num3;
							}
							else
							{
								num2++;
								if (num2 > 10000)
								{
									num2 = 0;
									Application.DoEvents();
								}
							}
						}
						else
						{
							num2++;
							if (num2 > 10000)
							{
								num2 = 0;
								Application.DoEvents();
							}
						}
					}
					if (num > 0)
					{
						array = new byte[num];
						for (int i = 0; i < num; i++)
						{
							array[i] = array2[i];
						}
						strRecvInfo = ((array != null) ? Encoding.GetEncoding(936).GetString(array) : null);
						if (!string.IsNullOrEmpty(strRecvInfo) && strRecvInfo.StartsWith("Wait"))
						{
							num = 0;
							strRecvInfo = null;
							array = null;
							while (dateTime > DateTime.Now)
							{
								if (stream.CanRead && stream.DataAvailable)
								{
									num = stream.Read(array2, 0, array2.Length);
									break;
								}
							}
							if (num > 0)
							{
								array = new byte[num];
								for (int j = 0; j < num; j++)
								{
									array[j] = array2[j];
								}
								strRecvInfo = ((array != null) ? Encoding.GetEncoding(936).GetString(array) : null);
							}
						}
						if (string.IsNullOrEmpty(strRecvInfo) || !strRecvInfo.StartsWith("Return"))
						{
							wgAppConfig.wgLog(string.Format("Recv={0},Command={1}", wgTools.SetObjToStr(strRecvInfo), command));
							wgAppConfig.wgLog("FaceId_ErrorCode.NotSupportedException");
							wgTools.WgDebugWrite("FaceId_ErrorCode.NotSupportedException", new object[0]);
							return -4;
						}
						string keyValue = dfrmFaceManagement4Hikvision.FaceId_Item.GetKeyValue(strRecvInfo, "result");
						if (!keyValue.Equals("success"))
						{
							if (keyValue.Equals("busy"))
							{
								wgAppConfig.wgLog(string.Format("Recv={0},Command={1}", strRecvInfo, command));
								wgAppConfig.wgLog("FaceId_ErrorCode.Busy");
								wgTools.WgDebugWrite("FaceId_ErrorCode.Busy", new object[0]);
								return -2;
							}
							wgAppConfig.wgLog(string.Format("Recv={0},Command={1}", strRecvInfo, command));
							wgAppConfig.wgLog("FaceId_ErrorCode.Failed");
							wgTools.WgDebugWrite("FaceId_ErrorCode.Failed", new object[0]);
							return -3;
						}
					}
					if (array != null)
					{
						recvInfo = array;
						return 1;
					}
					wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
					return -1;
				}
			}
			return -1;
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0017F548 File Offset: 0x0017E548
		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				if (!this.bStarting)
				{
					if (this.progressBar1.Value != 0 && this.progressBar1.Value != this.progressBar1.Maximum)
					{
						Cursor.Current = Cursors.WaitCursor;
					}
				}
				else if (this.dgvUsers.DataSource == null)
				{
					Cursor.Current = Cursors.WaitCursor;
				}
				else
				{
					Cursor.Current = Cursors.Default;
					this.lblWait.Visible = false;
					this.groupBox1.Enabled = true;
					this.bStarting = false;
					this.timer1.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0017F5FC File Offset: 0x0017E5FC
		private void updateButtonDisplay()
		{
			this.btnUploadSelectedUsers.Enabled = false;
			this.btnDeleteSelectedUsersFromSelectedDevice.Enabled = false;
			this.btnDownloadSelectedUsers.Enabled = false;
			if (this.dgvSelectedDoors.RowCount > 0)
			{
				this.btnCheck.Enabled = true;
				this.btnAdjustTime.Enabled = true;
				this.btnDownloadAllUsers.Enabled = true;
				this.btnUploadAllUsers.Enabled = true;
				this.btnDeleteAll.Enabled = true;
				if (this.dgvSelectedUsers.RowCount > 0)
				{
					this.btnUploadSelectedUsers.Enabled = true;
					this.btnDeleteSelectedUsersFromSelectedDevice.Enabled = true;
					this.btnDownloadSelectedUsers.Enabled = true;
					return;
				}
			}
			else
			{
				this.btnCheck.Enabled = false;
				this.btnAdjustTime.Enabled = false;
				this.btnDownloadAllUsers.Enabled = false;
				this.btnUploadAllUsers.Enabled = false;
				this.btnDeleteAll.Enabled = false;
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0017F6E8 File Offset: 0x0017E6E8
		private int updateDBFaceInfo(long workNOonDevice, string faceInfo, long diff)
		{
			if (workNOonDevice >= 0L)
			{
				long num = workNOonDevice + diff;
				if (num <= 0L)
				{
					return -1;
				}
				if (string.IsNullOrEmpty(faceInfo))
				{
					return -1;
				}
				string text = "";
				try
				{
					text = string.Format("SELECT f_ConsumerID FROM t_b_Consumer WHERE f_CardNO ={0}", num);
					if (wgAppConfig.getValBySql(text) <= 0)
					{
						icConsumer icConsumer = new icConsumer();
						string text2 = "";
						long num2 = icConsumer.ConsumerNONext(text2);
						if (num2 < 0L)
						{
							num2 = 1L;
						}
						string text3 = this.getConsumerNameInFace(faceInfo);
						if (string.IsNullOrEmpty(text3))
						{
							text3 = "N" + num.ToString();
						}
						icConsumer.addNew(num2.ToString().ToString(), text3, 0, 1, 0, 1, DateTime.Now, DateTime.Parse("2099-12-31 23:59:59"), 345678, num);
					}
					text = string.Format("SELECT f_ConsumerID FROM t_b_Consumer WHERE f_CardNO ={0}", num);
					int valBySql = wgAppConfig.getValBySql(text);
					if (valBySql <= 0)
					{
						return -1;
					}
					text = string.Format("SELECT f_ConsumerID FROM t_b_Consumer_Face WHERE f_ConsumerID ={0}", valBySql);
					if (wgAppConfig.getValBySql(text) <= 0)
					{
						text = string.Format("INSERT INTO t_b_Consumer_Face ( f_ConsumerID, f_FaceInfo) Values({0},'{1}')", valBySql, faceInfo);
						if (wgAppConfig.runUpdateSql(text) > 0)
						{
							return 1;
						}
					}
					else
					{
						text = string.Format("UPDATE t_b_Consumer_Face SET f_FaceInfo = '{0}' WHERE f_ConsumerID ={1}", faceInfo, valBySql);
						if (wgAppConfig.runUpdateSql(text) > 0)
						{
							return 1;
						}
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog("strSql=" + text + "\r\n" + ex.ToString());
				}
				return -1;
			}
			return -1;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0017F870 File Offset: 0x0017E870
		private void UpdateListBoxResult(string info)
		{
			XMessageBox.Show(info);
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0017F87C File Offset: 0x0017E87C
		public int updateOneUserByHTTP(string strIP, int PORT, string devicePassword, long userID, long cardNO, string username, string jpgName, ref string strRecvInfo, ref byte[] recvInfo)
		{
			if (this.m_lUserID4updateOneUser == -1)
			{
				CHCNetSDK.NET_DVR_USER_LOGIN_INFO net_DVR_USER_LOGIN_INFO = default(CHCNetSDK.NET_DVR_USER_LOGIN_INFO);
				CHCNetSDK.NET_DVR_DEVICEINFO_V40 net_DVR_DEVICEINFO_V = default(CHCNetSDK.NET_DVR_DEVICEINFO_V40);
				net_DVR_DEVICEINFO_V.struDeviceV30.sSerialNumber = new byte[48];
				net_DVR_USER_LOGIN_INFO.sDeviceAddress = strIP;
				net_DVR_USER_LOGIN_INFO.sUserName = this.deviceDefaultUser;
				net_DVR_USER_LOGIN_INFO.sPassword = this.deviceDefaultPassword;
				net_DVR_USER_LOGIN_INFO.wPort = (ushort)PORT;
				this.m_lUserID4updateOneUser = CHCNetSDK.NET_DVR_Login_V40(ref net_DVR_USER_LOGIN_INFO, ref net_DVR_DEVICEINFO_V);
			}
			this.m_lUserID = this.m_lUserID4updateOneUser;
			if (this.m_lUserID < 0)
			{
				wgAppConfig.wgLog("login  failed=" + CHCNetSDK.NET_DVR_GetLastError().ToString());
				return -1;
			}
			int num2;
			if (userID == -1L)
			{
				CHCNetSDK.NET_DVR_ACS_PARAM_TYPE net_DVR_ACS_PARAM_TYPE = default(CHCNetSDK.NET_DVR_ACS_PARAM_TYPE);
				net_DVR_ACS_PARAM_TYPE.dwSize = (uint)Marshal.SizeOf(net_DVR_ACS_PARAM_TYPE);
				net_DVR_ACS_PARAM_TYPE.dwParamType |= 4096U;
				uint num = (uint)Marshal.SizeOf(net_DVR_ACS_PARAM_TYPE);
				IntPtr intPtr = Marshal.AllocHGlobal((int)num);
				Marshal.StructureToPtr(net_DVR_ACS_PARAM_TYPE, intPtr, false);
				if (!CHCNetSDK.NET_DVR_RemoteControl(this.m_lUserID, 2118U, intPtr, num))
				{
					num2 = -1;
				}
				else
				{
					wgAppConfig.wgLog("clear faces users OK");
					num2 = 1;
				}
				Marshal.FreeHGlobal(intPtr);
				return num2;
			}
			dfrmFaceManagement4Hikvision.Person person = new dfrmFaceManagement4Hikvision.Person();
			person.cardId = cardNO.ToString();
			person.name = username;
			person.employeeId = userID.ToString();
			if (string.IsNullOrEmpty(jpgName))
			{
				person.bDeleted = true;
			}
			num2 = (this.SendCardInfo(person) ? 1 : (-1));
			wgAppConfig.wgDebugWrite("SendCardData : " + num2.ToString());
			if (num2 >= 0)
			{
				if (string.IsNullOrEmpty(jpgName))
				{
					return num2;
				}
				FileInfo fileInfo = new FileInfo(wgAppConfig.Path4Photo() + jpgName + ".jpg");
				num2 = (this.SendPhotoInfo(person, fileInfo) ? 1 : (-1));
				wgAppConfig.wgDebugWrite("SendFaceData : " + num2.ToString());
			}
			return num2;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0017FA60 File Offset: 0x0017EA60
		private void updateUserPhoto()
		{
			using (DataTable table = ((DataView)this.dgvUsers.DataSource).Table)
			{
				for (int i = 0; i < table.Rows.Count; i++)
				{
					string text = table.Rows[i]["f_CardNO"].ToString();
					if (!string.IsNullOrEmpty(text) && wgAppConfig.FileIsExisted(wgAppConfig.Path4Photo() + text + ".jpg"))
					{
						table.Rows[i]["f_other"] = 1;
					}
					else
					{
						table.Rows[i]["f_other"] = 0;
					}
				}
				table.AcceptChanges();
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0017FB34 File Offset: 0x0017EB34
		private void updateUserRegister()
		{
			DataTable dataTable = new DataTable("face");
			string text = string.Format(" SELECT f_ConsumerID  ", new object[0]) + " FROM t_b_Consumer_face ";
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(dataTable);
						}
					}
					goto IL_00D3;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(dataTable);
					}
				}
			}
			IL_00D3:
			DataView dataView = new DataView(dataTable);
			if (dataView.Count > 0)
			{
				using (DataTable table = ((DataView)this.dgvUsers.DataSource).Table)
				{
					for (int i = 0; i < table.Rows.Count; i++)
					{
						dataView.RowFilter = "f_ConsumerID = " + table.Rows[i]["f_ConsumerID"];
						if (dataView.Count > 0)
						{
							table.Rows[i]["f_other"] = 1;
						}
						else
						{
							table.Rows[i]["f_other"] = 0;
						}
					}
					table.AcceptChanges();
					return;
				}
			}
			using (DataTable table2 = ((DataView)this.dgvUsers.DataSource).Table)
			{
				for (int j = 0; j < table2.Rows.Count; j++)
				{
					table2.Rows[j]["f_other"] = 0;
				}
				table2.AcceptChanges();
			}
		}

		// Token: 0x040029ED RID: 10733
		public const int DeviceCodePage = 936;

		// Token: 0x040029EE RID: 10734
		private bool bDataErrorExist;

		// Token: 0x040029EF RID: 10735
		private bool bExit;

		// Token: 0x040029F0 RID: 10736
		private Control defaultFindControl;

		// Token: 0x040029F1 RID: 10737
		private DataTable dt;

		// Token: 0x040029F2 RID: 10738
		private DataView dv;

		// Token: 0x040029F3 RID: 10739
		private DataView dv1;

		// Token: 0x040029F4 RID: 10740
		private DataView dv2;

		// Token: 0x040029F5 RID: 10741
		private DataView dvSelected;

		// Token: 0x040029F6 RID: 10742
		private int eventRecID;

		// Token: 0x040029F7 RID: 10743
		private CHCNetSDK.RemoteConfigCallback g_fSetFaceParamCallback;

		// Token: 0x040029F8 RID: 10744
		private CHCNetSDK.RemoteConfigCallback g_fSetGatewayCardCallback;

		// Token: 0x040029F9 RID: 10745
		private int nametablecnt;

		// Token: 0x040029FA RID: 10746
		private DataTable tbRunInfoLog;

		// Token: 0x040029FB RID: 10747
		private TcpClient tcp;

		// Token: 0x040029FC RID: 10748
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x040029FD RID: 10749
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x040029FE RID: 10750
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x040029FF RID: 10751
		private ArrayList arrIPARPFailed = new ArrayList();

		// Token: 0x04002A00 RID: 10752
		private ArrayList arrIPARPOK = new ArrayList();

		// Token: 0x04002A01 RID: 10753
		private ArrayList arrIPPingFailed = new ArrayList();

		// Token: 0x04002A02 RID: 10754
		private ArrayList arrIPPingOK = new ArrayList();

		// Token: 0x04002A03 RID: 10755
		private bool bStarting = true;

		// Token: 0x04002A04 RID: 10756
		private string deviceDefaultPassword = "w1234567";

		// Token: 0x04002A05 RID: 10757
		private string deviceDefaultUser = "admin";

		// Token: 0x04002A06 RID: 10758
		private dfrmWait dfrmWait1 = new dfrmWait();

		// Token: 0x04002A07 RID: 10759
		public string factoryName = "Hanvon";

		// Token: 0x04002A08 RID: 10760
		public int m_lSetCardCfgHandle = -1;

		// Token: 0x04002A09 RID: 10761
		private int m_lSetFaceCfgHandle = -1;

		// Token: 0x04002A0A RID: 10762
		private int m_lUserID = -1;

		// Token: 0x04002A0B RID: 10763
		private int m_lUserID4updateOneUser = -1;

		// Token: 0x04002A0C RID: 10764
		private string strGroupFilter = "";

		// Token: 0x04002A0D RID: 10765
		private int tcpBuffSize = 204800;

		// Token: 0x020002D5 RID: 725
		public class FaceId_Item
		{
			// Token: 0x060013F4 RID: 5108 RVA: 0x001826C8 File Offset: 0x001816C8
			public FaceId_Item(string keyName, string keyValue)
			{
				this.Name = keyName;
				this.Value = keyValue;
			}

			// Token: 0x060013F5 RID: 5109 RVA: 0x001826E0 File Offset: 0x001816E0
			public static dfrmFaceManagement4Hikvision.FaceId_Item[] GetAllItems(string Answer)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					List<dfrmFaceManagement4Hikvision.FaceId_Item> list = new List<dfrmFaceManagement4Hikvision.FaceId_Item>();
					string text = "\\b([a-z|A-Z|_]+)\\s*=\\s*\"([^\"]+)\"";
					MatchCollection matchCollection = Regex.Matches(Answer, text);
					if (matchCollection != null)
					{
						foreach (object obj in matchCollection)
						{
							Match match = (Match)obj;
							list.Add(new dfrmFaceManagement4Hikvision.FaceId_Item(match.Groups[1].Value, match.Groups[2].Value));
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
				return null;
			}

			// Token: 0x060013F6 RID: 5110 RVA: 0x00182794 File Offset: 0x00181794
			public static dfrmFaceManagement4Hikvision.FaceId_Item[] GetAllPairs(string Answer, string LeftKeyName, string RightKeyName)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					List<dfrmFaceManagement4Hikvision.FaceId_Item> list = new List<dfrmFaceManagement4Hikvision.FaceId_Item>();
					string text = string.Concat(new string[] { "\\b", LeftKeyName, "=\"([^\"]+)\"\\s*", RightKeyName, "=\"([^\"]+)\"" });
					MatchCollection matchCollection = Regex.Matches(Answer, text);
					if (matchCollection != null)
					{
						foreach (object obj in matchCollection)
						{
							Match match = (Match)obj;
							list.Add(new dfrmFaceManagement4Hikvision.FaceId_Item(match.Groups[1].Value, match.Groups[2].Value));
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
				return null;
			}

			// Token: 0x060013F7 RID: 5111 RVA: 0x00182878 File Offset: 0x00181878
			public static string GetKeyValue(string Answer, string KeyName)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					string text = "\\b" + KeyName + "\\s*=\\s*\"([^\"]+)\"";
					Match match = Regex.Match(Answer, text);
					if (match.Success)
					{
						return match.Groups[1].Value;
					}
				}
				return null;
			}

			// Token: 0x060013F8 RID: 5112 RVA: 0x001828C4 File Offset: 0x001818C4
			public static string GetPairValue(string Answer, string LeftKeyName, string LeftKeyValue, string RightKeyName)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					string text = string.Concat(new string[] { "\\b", LeftKeyName, "=\"", LeftKeyValue, "\"\\s*", RightKeyName, "=\"([^\"]+)\"" });
					Match match = Regex.Match(Answer, text);
					if (match.Success)
					{
						return match.Groups[1].Value;
					}
				}
				return null;
			}

			// Token: 0x04002A5B RID: 10843
			public readonly string Name;

			// Token: 0x04002A5C RID: 10844
			public readonly string Value;
		}

		// Token: 0x020002D6 RID: 726
		public class Person
		{
			// Token: 0x060013F9 RID: 5113 RVA: 0x00182936 File Offset: 0x00181936
			public Person()
			{
				this.name = null;
				this.gender = null;
				this.cardId = null;
				this.employeeId = null;
				this.department = null;
				this.bDeleted = false;
			}

			// Token: 0x060013FA RID: 5114 RVA: 0x00182968 File Offset: 0x00181968
			public Person(string name, string cardId, string employeeId, string gender, string department)
			{
				this.name = name;
				this.cardId = cardId;
				this.employeeId = employeeId;
				this.gender = gender;
				this.department = department;
			}

			// Token: 0x060013FB RID: 5115 RVA: 0x00182995 File Offset: 0x00181995
			public override string ToString()
			{
				return string.Format("Person{name:'{0}', cardId:'{1}', employeeId:'{2}'}", this.name, this.cardId, this.employeeId);
			}

			// Token: 0x170001B9 RID: 441
			// (get) Token: 0x060013FC RID: 5116 RVA: 0x001829B3 File Offset: 0x001819B3
			// (set) Token: 0x060013FD RID: 5117 RVA: 0x001829BB File Offset: 0x001819BB
			public bool bDeleted
			{
				get
				{
					return this._bDeleted;
				}
				set
				{
					this._bDeleted = value;
				}
			}

			// Token: 0x170001BA RID: 442
			// (get) Token: 0x060013FE RID: 5118 RVA: 0x001829C4 File Offset: 0x001819C4
			// (set) Token: 0x060013FF RID: 5119 RVA: 0x001829CC File Offset: 0x001819CC
			public string cardId
			{
				get
				{
					return this._cardId;
				}
				set
				{
					this._cardId = value;
				}
			}

			// Token: 0x170001BB RID: 443
			// (get) Token: 0x06001400 RID: 5120 RVA: 0x001829D5 File Offset: 0x001819D5
			// (set) Token: 0x06001401 RID: 5121 RVA: 0x001829DD File Offset: 0x001819DD
			public string department
			{
				get
				{
					return this._department;
				}
				set
				{
					this._department = value;
				}
			}

			// Token: 0x170001BC RID: 444
			// (get) Token: 0x06001402 RID: 5122 RVA: 0x001829E6 File Offset: 0x001819E6
			// (set) Token: 0x06001403 RID: 5123 RVA: 0x001829EE File Offset: 0x001819EE
			public string employeeId
			{
				get
				{
					return this._employeeId;
				}
				set
				{
					this._employeeId = value;
				}
			}

			// Token: 0x170001BD RID: 445
			// (get) Token: 0x06001404 RID: 5124 RVA: 0x001829F7 File Offset: 0x001819F7
			// (set) Token: 0x06001405 RID: 5125 RVA: 0x001829FF File Offset: 0x001819FF
			public string gender
			{
				get
				{
					return this._gender;
				}
				set
				{
					this._gender = value;
				}
			}

			// Token: 0x170001BE RID: 446
			// (get) Token: 0x06001406 RID: 5126 RVA: 0x00182A08 File Offset: 0x00181A08
			// (set) Token: 0x06001407 RID: 5127 RVA: 0x00182A10 File Offset: 0x00181A10
			public string name
			{
				get
				{
					return this._name;
				}
				set
				{
					this._name = value;
				}
			}

			// Token: 0x04002A5D RID: 10845
			private bool _bDeleted;

			// Token: 0x04002A5E RID: 10846
			private string _cardId;

			// Token: 0x04002A5F RID: 10847
			private string _department;

			// Token: 0x04002A60 RID: 10848
			private string _employeeId;

			// Token: 0x04002A61 RID: 10849
			private string _gender;

			// Token: 0x04002A62 RID: 10850
			private string _name;
		}

		// Token: 0x020002D7 RID: 727
		public class PersonHelper
		{
			// Token: 0x06001408 RID: 5128 RVA: 0x00182A1C File Offset: 0x00181A1C
			public static dfrmFaceManagement4Hikvision.Person ParseFromString(string value)
			{
				try
				{
					if (string.IsNullOrEmpty(value))
					{
						return null;
					}
					string[] array = value.Split(new char[] { '_' });
					if (array.Length != 5)
					{
						return null;
					}
					string text = array[0];
					string text2 = array[1];
					string text3 = array[2];
					string text4 = array[3];
					return new dfrmFaceManagement4Hikvision.Person(text2, text, text4, array[4], text3);
				}
				catch
				{
				}
				return null;
			}
		}
	}
}
