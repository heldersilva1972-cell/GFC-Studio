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
	// Token: 0x020002DA RID: 730
	public partial class dfrmFaceManagement4uniubi : frmN3000
	{
		// Token: 0x0600145A RID: 5210 RVA: 0x0018D688 File Offset: 0x0018C688
		public dfrmFaceManagement4uniubi()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			wgAppConfig.custDataGridview(ref this.dgvRunInfo);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0018D780 File Offset: 0x0018C780
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

		// Token: 0x0600145C RID: 5212 RVA: 0x0018DA80 File Offset: 0x0018CA80
		private void addLogEvent(string desc, string info)
		{
			this.addLogEvent(desc, info, 100);
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0018DA8C File Offset: 0x0018CA8C
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

		// Token: 0x0600145E RID: 5214 RVA: 0x0018DC40 File Offset: 0x0018CC40
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

		// Token: 0x0600145F RID: 5215 RVA: 0x0018E228 File Offset: 0x0018D228
		public int adjusttimeByHTTP(string strIP, int PORT, string devicePassword)
		{
			string text = string.Format("http://{0}:{1}", strIP, PORT.ToString());
			string text2 = "/setTime";
			long num = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds * 1000.0);
			string text3 = text + text2;
			string text4 = string.Format("pass={0}&timestamp={1}", this.deviceDefaultPassword, num.ToString());
			string text5 = this.PushToWeb(text3, text4, Encoding.UTF8);
			string text6;
			if ((text6 = text5) != null && (text6 == "0" || text6 == "1"))
			{
				return 1;
			}
			wgAppConfig.wgLog(text5);
			return -1;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0018E2ED File Offset: 0x0018D2ED
		private void arrPingInfoReset()
		{
			this.arrIPPingOK.Clear();
			this.arrIPPingFailed.Clear();
			this.arrIPARPOK.Clear();
			this.arrIPARPFailed.Clear();
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0018E31C File Offset: 0x0018D31C
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

		// Token: 0x06001462 RID: 5218 RVA: 0x0018E360 File Offset: 0x0018D360
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

		// Token: 0x06001463 RID: 5219 RVA: 0x0018E3F0 File Offset: 0x0018D3F0
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 1;
			}
			this.updateButtonDisplay();
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0018E45C File Offset: 0x0018D45C
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

		// Token: 0x06001465 RID: 5221 RVA: 0x0018E67B File Offset: 0x0018D67B
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvDoors);
			this.updateButtonDisplay();
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0018E68E File Offset: 0x0018D68E
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
			this.updateButtonDisplay();
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0018E6A8 File Offset: 0x0018D6A8
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

		// Token: 0x06001468 RID: 5224 RVA: 0x0018ECFC File Offset: 0x0018DCFC
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

		// Token: 0x06001469 RID: 5225 RVA: 0x0018F300 File Offset: 0x0018E300
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 0;
			}
			this.updateButtonDisplay();
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0018F36C File Offset: 0x0018E36C
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

		// Token: 0x0600146B RID: 5227 RVA: 0x0018F454 File Offset: 0x0018E454
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
									if (this.updateOneUserByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"]), -1L, 0L, "", "", ref text, ref array) > 0)
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

		// Token: 0x0600146C RID: 5228 RVA: 0x0018FAF0 File Offset: 0x0018EAF0
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.updateButtonDisplay();
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0018FB03 File Offset: 0x0018EB03
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
			this.updateButtonDisplay();
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0018FB1C File Offset: 0x0018EB1C
		private void btnDeviceManage_Click(object sender, EventArgs e)
		{
			using (dfrmFaceDeviceManagement dfrmFaceDeviceManagement = new dfrmFaceDeviceManagement())
			{
				dfrmFaceDeviceManagement.ShowDialog(this);
			}
			this._fillDeviceGrid();
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0018FB5C File Offset: 0x0018EB5C
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
										dfrmFaceManagement4uniubi.FaceId_Item[] allItems = dfrmFaceManagement4uniubi.FaceId_Item.GetAllItems(text);
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
													dfrmFaceManagement4uniubi.FaceId_Item[] allItems2 = dfrmFaceManagement4uniubi.FaceId_Item.GetAllItems(text);
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

		// Token: 0x06001470 RID: 5232 RVA: 0x001903F8 File Offset: 0x0018F3F8
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
										dfrmFaceManagement4uniubi.FaceId_Item[] allItems = dfrmFaceManagement4uniubi.FaceId_Item.GetAllItems(text);
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

		// Token: 0x06001471 RID: 5233 RVA: 0x00190D6C File Offset: 0x0018FD6C
		private void btnExit_Click(object sender, EventArgs e)
		{
			this.bExit = true;
			base.Close();
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x00190D7C File Offset: 0x0018FD7C
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

		// Token: 0x06001473 RID: 5235 RVA: 0x00190EA0 File Offset: 0x0018FEA0
		private void btnUploadAllUsers_Click(object sender, EventArgs e)
		{
			bool @checked = this.chkDeleteUsersNotInDB.Checked;
			if (XMessageBox.Show(this, string.Concat(new string[]
			{
				string.Format(CommonStr.strAreYouSure + " {0}?\r\n\r\n" + (@checked ? CommonStr.strDevicesReplaceFace : ""), (sender as Button).Text),
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
							goto IL_01E6;
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
					IL_01E6:
					this.progressBar1.Maximum = this.dgvSelectedDoors.Rows.Count * Math.Max(dataTable.Rows.Count, 1);
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
									string text2 = "";
									byte[] array = null;
									if (@checked)
									{
										this.updateOneUserByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"]), -1L, 0L, "", "", ref text2, ref array);
										if (this.bExit)
										{
											return;
										}
									}
									int num = 0;
									int num2 = 0;
									bool flag = false;
									string text3 = "";
									for (int j = 0; j < dataTable.Rows.Count; j++)
									{
										long workNOOnDevice = this.getWorkNOOnDevice(long.Parse(wgTools.SetObjToStr(dataTable.Rows[j]["f_CardNO"])), long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"])));
										int num3 = int.Parse(wgTools.SetObjToStr(dataTable.Rows[j]["f_ConsumerID"]));
										text3 = this.getValidUserName(wgTools.SetObjToStr(dataTable.Rows[j]["f_ConsumerName"]));
										wgTools.SetObjToStr(workNOOnDevice);
										string text4 = "";
										if (this.progressBar1.Value < this.progressBar1.Maximum)
										{
											this.progressBar1.Value++;
											Application.DoEvents();
										}
										string text5 = "";
										if (wgAppConfig.FileIsExisted(wgAppConfig.Path4Photo() + workNOOnDevice.ToString() + ".jpg"))
										{
											text5 = workNOOnDevice.ToString();
										}
										if (this.updateOneUserByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"]), (long)num3, workNOOnDevice, text3, text5, ref text2, ref array) <= 0)
										{
											flag = true;
											wgAppConfig.wgLog(string.Format("DeviceIP={0},DevicePort={1},Answer={2},recvInfo={3}, Command={4}", new object[]
											{
												wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
												int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])),
												wgTools.SetObjToStr(text2),
												(array == null) ? "" : Encoding.GetEncoding(936).GetString(array),
												text4
											}));
											break;
										}
										num++;
										if (this.bExit)
										{
											return;
										}
									}
									bool flag2 = true;
									if (flag)
									{
										if (!string.IsNullOrEmpty(text3))
										{
											this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}. ({2}:{3})", new object[]
											{
												(sender as Button).Text,
												CommonStr.strFailed,
												CommonStr.strPhotoCheckAgain,
												text3
											}), 101);
										}
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[U{2}](T{4})", new object[]
										{
											(sender as Button).Text,
											CommonStr.strFailed,
											num,
											num2,
											dataTable.Rows.Count
										}), 101);
									}
									else
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[U{2}] ", new object[]
										{
											(sender as Button).Text,
											CommonStr.strSuccessfully,
											num,
											num2
										}));
									}
									if (!flag2)
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

		// Token: 0x06001474 RID: 5236 RVA: 0x00191A24 File Offset: 0x00190A24
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
					while (num < this.dgvSelectedUsers.Rows.Count && ((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count >= this.dgvSelectedUsers.Rows.Count)
					{
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
							arrayList2.Add(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_ConsumerID"]));
							arrayList3.Add(this.getValidUserName(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_ConsumerName"])));
							arrayList4.Add(wgTools.SetObjToStr(((DataView)this.dgvSelectedUsers.DataSource)[num]["f_Other"]));
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
					ArrayList arrayList5 = new ArrayList();
					if (this.arrIPARPOK.Count != 0 || this.arrIPPingOK.Count != 0)
					{
						for (int i = 0; i < 2; i++)
						{
							foreach (object obj2 in dataView)
							{
								DataRowView dataRowView2 = (DataRowView)obj2;
								if (this.IsIPCanConnect(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"])) && arrayList5.IndexOf(dataRowView2["f_DeviceName"]) < 0)
								{
									arrayList5.Add(dataRowView2["f_DeviceName"]);
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), CommonStr.strStart);
									if (this.progressBar1.Value < this.progressBar1.Maximum)
									{
										this.progressBar1.Value++;
										Application.DoEvents();
									}
									string text3 = "";
									byte[] array = null;
									int num3 = 0;
									int num4 = 0;
									bool flag2 = false;
									string text4 = "";
									for (int j = 0; j < arrayList.Count; j++)
									{
										long workNOOnDevice = this.getWorkNOOnDevice(long.Parse(wgTools.SetObjToStr(arrayList[j])), long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"])));
										int num5 = int.Parse(wgTools.SetObjToStr(arrayList2[j]));
										text4 = wgTools.SetObjToStr(arrayList3[j]);
										wgTools.SetObjToStr(workNOOnDevice);
										string text5 = "";
										if (this.progressBar1.Value < this.progressBar1.Maximum)
										{
											this.progressBar1.Value++;
											Application.DoEvents();
										}
										string text6 = "";
										if (arrayList4[j].ToString().Equals("1"))
										{
											text6 = workNOOnDevice.ToString();
										}
										if (flag)
										{
											text6 = "";
										}
										if (this.updateOneUserByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"]), (long)num5, workNOOnDevice, text4, text6, ref text3, ref array) <= 0)
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
										num3++;
										if (this.bExit)
										{
											return;
										}
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
											num3,
											num4,
											arrayList.Count
										}), 101);
									}
									else
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[U{2}] ", new object[]
										{
											(sender as Button).Text,
											CommonStr.strSuccessfully,
											num3,
											num4
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
							if (arrayList5.Count == dataView.Count)
							{
								break;
							}
							Thread.Sleep(300);
						}
					}
					if (arrayList5.Count != dataView.Count)
					{
						foreach (object obj3 in dataView)
						{
							DataRowView dataRowView3 = (DataRowView)obj3;
							if (arrayList5.IndexOf(dataRowView3["f_DeviceName"]) < 0)
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

		// Token: 0x06001475 RID: 5237 RVA: 0x001928F4 File Offset: 0x001918F4
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

		// Token: 0x06001476 RID: 5238 RVA: 0x00192924 File Offset: 0x00191924
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

		// Token: 0x06001477 RID: 5239 RVA: 0x00192B74 File Offset: 0x00191B74
		public int checkByHTTP(string strIP, int PORT, string devicePassword)
		{
			string text = string.Format("http://{0}:{1}", strIP, PORT.ToString());
			string text2 = "/getDeviceKey";
			Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds * 1000.0);
			string text3 = text + text2;
			string text4 = "";
			string text5 = this.PushToWeb(text3, text4, Encoding.UTF8);
			string text6;
			if ((text6 = text5) == null || (!(text6 == "0") && !(text6 == "1")))
			{
				wgAppConfig.wgLog("updateOneUserByHTTP  failed");
				return -1;
			}
			string text7 = "/device/openDoorControl";
			text3 = text + text7;
			text4 = string.Format("pass={0}", this.deviceDefaultPassword);
			text5 = this.PushToWeb(text3, text4, Encoding.UTF8);
			if (text5 == "1")
			{
				return 1;
			}
			wgAppConfig.wgLog(text5);
			return 0;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00192C74 File Offset: 0x00191C74
		private int checkValidFaceId(string cardno, string consumername)
		{
			long num = 0L;
			if (!string.IsNullOrEmpty(cardno))
			{
				long.TryParse(cardno, out num);
			}
			if (num > 0L)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x00192C9D File Offset: 0x00191C9D
		private void chkDeleteUsersNotInDB_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00192C9F File Offset: 0x00191C9F
		private void dfrmFaceManage_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00192CB4 File Offset: 0x00191CB4
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

		// Token: 0x0600147C RID: 5244 RVA: 0x00192D50 File Offset: 0x00191D50
		private void dfrmFaceManage_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			int num = int.Parse("0" + wgAppConfig.getSystemParamByNO(209));
			if (num != 0 && num == 1)
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
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
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

		// Token: 0x0600147D RID: 5245 RVA: 0x0019315C File Offset: 0x0019215C
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

		// Token: 0x0600147E RID: 5246 RVA: 0x00193194 File Offset: 0x00192194
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x001931A4 File Offset: 0x001921A4
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

		// Token: 0x06001480 RID: 5248 RVA: 0x00193254 File Offset: 0x00192254
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

		// Token: 0x06001481 RID: 5249 RVA: 0x001932C0 File Offset: 0x001922C0
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

		// Token: 0x06001482 RID: 5250 RVA: 0x001932F0 File Offset: 0x001922F0
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00193300 File Offset: 0x00192300
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

		// Token: 0x06001484 RID: 5252 RVA: 0x00193330 File Offset: 0x00192330
		private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x00193340 File Offset: 0x00192340
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

		// Token: 0x06001486 RID: 5254 RVA: 0x00193370 File Offset: 0x00192370
		private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneUser.PerformClick();
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00193380 File Offset: 0x00192380
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

		// Token: 0x06001488 RID: 5256 RVA: 0x00193412 File Offset: 0x00192412
		private long getCardNOonDB(long WorkNO, long diff)
		{
			return WorkNO + diff;
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00193418 File Offset: 0x00192418
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

		// Token: 0x0600148A RID: 5258 RVA: 0x0019346C File Offset: 0x0019246C
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

		// Token: 0x0600148B RID: 5259 RVA: 0x001934D4 File Offset: 0x001924D4
		private long getWorkNOOnDevice(long cardno, long diff)
		{
			if (cardno > diff)
			{
				return cardno - diff;
			}
			return cardno;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x001934E0 File Offset: 0x001924E0
		private bool IsIPCanConnect(string ip)
		{
			return (this.arrIPPingOK.Count > 0 && this.arrIPPingOK.IndexOf(ip) >= 0) || (this.arrIPARPOK.Count > 0 && this.arrIPARPOK.IndexOf(ip) >= 0);
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0019352E File Offset: 0x0019252E
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00193550 File Offset: 0x00192550
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

		// Token: 0x0600148F RID: 5263 RVA: 0x001936AC File Offset: 0x001926AC
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

		// Token: 0x06001490 RID: 5264 RVA: 0x001938FC File Offset: 0x001928FC
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

		// Token: 0x06001491 RID: 5265 RVA: 0x00193930 File Offset: 0x00192930
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

		// Token: 0x06001492 RID: 5266 RVA: 0x001939E8 File Offset: 0x001929E8
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

		// Token: 0x06001493 RID: 5267 RVA: 0x00193ADC File Offset: 0x00192ADC
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

		// Token: 0x06001494 RID: 5268 RVA: 0x00193B94 File Offset: 0x00192B94
		private void tcpClose()
		{
			if (this.tcp != null)
			{
				this.tcp.Close();
				this.tcp = null;
			}
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x00193BB0 File Offset: 0x00192BB0
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
						string keyValue = dfrmFaceManagement4uniubi.FaceId_Item.GetKeyValue(strRecvInfo, "result");
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

		// Token: 0x06001496 RID: 5270 RVA: 0x00193F48 File Offset: 0x00192F48
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

		// Token: 0x06001497 RID: 5271 RVA: 0x00193FFC File Offset: 0x00192FFC
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

		// Token: 0x06001498 RID: 5272 RVA: 0x001940E8 File Offset: 0x001930E8
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

		// Token: 0x06001499 RID: 5273 RVA: 0x00194270 File Offset: 0x00193270
		public int updateOneUserByHTTP(string strIP, int PORT, string devicePassword, long userID, long cardNO, string username, string jpgName, ref string strRecvInfo, ref byte[] recvInfo)
		{
			string text = string.Format("http://{0}:{1}", strIP, PORT.ToString());
			string text2 = "/person/create";
			string text3 = "/face/create";
			string text4 = "/person/delete";
			string text5 = "";
			text5 = text + text4;
			string text6 = string.Format("pass={0}&id={1}", this.deviceDefaultPassword, userID);
			string text7 = this.PushToWeb(text5, text6, Encoding.UTF8);
			if (!(text7 == "0") && !(text7 == "1"))
			{
				wgAppConfig.wgLog(text7);
				return -1;
			}
			if (userID == -1L)
			{
				wgAppConfig.wgLog("clear faces users OK");
				return 1;
			}
			if (string.IsNullOrEmpty(jpgName))
			{
				return 1;
			}
			text5 = text + text2;
			text6 = string.Format("pass={0}&person={1}", this.deviceDefaultPassword, string.Format("{{\"id\":\"{0}\",\"idcardNum\":\"{1}\",\"name\":\"{2}\"}}", userID, cardNO, username));
			text7 = this.PushToWeb(text5, text6, Encoding.UTF8);
			if (text7 == "1" && !string.IsNullOrEmpty(jpgName))
			{
				text5 = text + text3;
				string text8 = "";
				using (FileStream fileStream = new FileStream(wgAppConfig.Path4Photo() + jpgName + ".jpg", FileMode.Open, FileAccess.Read))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					text8 = Convert.ToBase64String(array);
					text8 = text8.Replace("/", "%2F");
					text8 = text8.Replace("+", "%2B");
				}
				text6 = string.Format("pass={0}&personId={1}&faceId=&imgBase64={2}", this.deviceDefaultPassword, userID, text8);
				text7 = this.PushToWeb(text5, text6, Encoding.UTF8);
			}
			if (text7 == "1")
			{
				return 1;
			}
			if (text7 == "-1")
			{
				wgAppConfig.wgLog("updateOneUserByHTTP  failed");
			}
			else
			{
				wgAppConfig.wgLog(text7);
			}
			recvInfo = null;
			strRecvInfo = "";
			return -1;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0019448C File Offset: 0x0019348C
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

		// Token: 0x0600149B RID: 5275 RVA: 0x00194560 File Offset: 0x00193560
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

		// Token: 0x04002AD5 RID: 10965
		public const int DeviceCodePage = 936;

		// Token: 0x04002AD6 RID: 10966
		private bool bDataErrorExist;

		// Token: 0x04002AD7 RID: 10967
		private bool bExit;

		// Token: 0x04002AD8 RID: 10968
		private Control defaultFindControl;

		// Token: 0x04002AD9 RID: 10969
		private DataTable dt;

		// Token: 0x04002ADA RID: 10970
		private DataView dv;

		// Token: 0x04002ADB RID: 10971
		private DataView dv1;

		// Token: 0x04002ADC RID: 10972
		private DataView dv2;

		// Token: 0x04002ADD RID: 10973
		private DataView dvSelected;

		// Token: 0x04002ADE RID: 10974
		private int eventRecID;

		// Token: 0x04002ADF RID: 10975
		private int nametablecnt;

		// Token: 0x04002AE0 RID: 10976
		private DataTable tbRunInfoLog;

		// Token: 0x04002AE1 RID: 10977
		private TcpClient tcp;

		// Token: 0x04002AE2 RID: 10978
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04002AE3 RID: 10979
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04002AE4 RID: 10980
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04002AE5 RID: 10981
		private ArrayList arrIPARPFailed = new ArrayList();

		// Token: 0x04002AE6 RID: 10982
		private ArrayList arrIPARPOK = new ArrayList();

		// Token: 0x04002AE7 RID: 10983
		private ArrayList arrIPPingFailed = new ArrayList();

		// Token: 0x04002AE8 RID: 10984
		private ArrayList arrIPPingOK = new ArrayList();

		// Token: 0x04002AE9 RID: 10985
		private bool bStarting = true;

		// Token: 0x04002AEA RID: 10986
		private string deviceDefaultPassword = "12345678";

		// Token: 0x04002AEB RID: 10987
		private dfrmWait dfrmWait1 = new dfrmWait();

		// Token: 0x04002AEC RID: 10988
		public string factoryName = "Hanvon";

		// Token: 0x04002AED RID: 10989
		private string strGroupFilter = "";

		// Token: 0x04002AEE RID: 10990
		private int tcpBuffSize = 204800;

		// Token: 0x020002DB RID: 731
		public class FaceId_Item
		{
			// Token: 0x0600149E RID: 5278 RVA: 0x0019701C File Offset: 0x0019601C
			public FaceId_Item(string keyName, string keyValue)
			{
				this.Name = keyName;
				this.Value = keyValue;
			}

			// Token: 0x0600149F RID: 5279 RVA: 0x00197034 File Offset: 0x00196034
			public static dfrmFaceManagement4uniubi.FaceId_Item[] GetAllItems(string Answer)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					List<dfrmFaceManagement4uniubi.FaceId_Item> list = new List<dfrmFaceManagement4uniubi.FaceId_Item>();
					string text = "\\b([a-z|A-Z|_]+)\\s*=\\s*\"([^\"]+)\"";
					MatchCollection matchCollection = Regex.Matches(Answer, text);
					if (matchCollection != null)
					{
						foreach (object obj in matchCollection)
						{
							Match match = (Match)obj;
							list.Add(new dfrmFaceManagement4uniubi.FaceId_Item(match.Groups[1].Value, match.Groups[2].Value));
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
				return null;
			}

			// Token: 0x060014A0 RID: 5280 RVA: 0x001970E8 File Offset: 0x001960E8
			public static dfrmFaceManagement4uniubi.FaceId_Item[] GetAllPairs(string Answer, string LeftKeyName, string RightKeyName)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					List<dfrmFaceManagement4uniubi.FaceId_Item> list = new List<dfrmFaceManagement4uniubi.FaceId_Item>();
					string text = string.Concat(new string[] { "\\b", LeftKeyName, "=\"([^\"]+)\"\\s*", RightKeyName, "=\"([^\"]+)\"" });
					MatchCollection matchCollection = Regex.Matches(Answer, text);
					if (matchCollection != null)
					{
						foreach (object obj in matchCollection)
						{
							Match match = (Match)obj;
							list.Add(new dfrmFaceManagement4uniubi.FaceId_Item(match.Groups[1].Value, match.Groups[2].Value));
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
				return null;
			}

			// Token: 0x060014A1 RID: 5281 RVA: 0x001971CC File Offset: 0x001961CC
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

			// Token: 0x060014A2 RID: 5282 RVA: 0x00197218 File Offset: 0x00196218
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

			// Token: 0x04002B3A RID: 11066
			public readonly string Name;

			// Token: 0x04002B3B RID: 11067
			public readonly string Value;
		}
	}
}
