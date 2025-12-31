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
	// Token: 0x020002D8 RID: 728
	public partial class dfrmFaceManagement4Syd : frmN3000
	{
		// Token: 0x0600140A RID: 5130 RVA: 0x00182A9C File Offset: 0x00181A9C
		public dfrmFaceManagement4Syd()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			wgAppConfig.custDataGridview(ref this.dgvRunInfo);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00182B94 File Offset: 0x00181B94
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

		// Token: 0x0600140C RID: 5132 RVA: 0x00182E94 File Offset: 0x00181E94
		private void addLogEvent(string desc, string info)
		{
			this.addLogEvent(desc, info, 100);
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00182EA0 File Offset: 0x00181EA0
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

		// Token: 0x0600140E RID: 5134 RVA: 0x00183054 File Offset: 0x00182054
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

		// Token: 0x0600140F RID: 5135 RVA: 0x0018363C File Offset: 0x0018263C
		public int adjusttimeByHTTP(string strIP, int PORT, string devicePassword)
		{
			string text = string.Format("http://{0}:{1}", strIP, PORT.ToString());
			string text2 = "/setTime";
			long num = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
			string text3 = text + text2;
			string text4 = string.Format("key={0}&ts={1}", this.deviceDefaultPassword, num.ToString());
			string text5 = this.PushToWeb(text3, text4, Encoding.UTF8);
			string text6;
			if ((text6 = text5) != null && (text6 == "0" || text6 == "1"))
			{
				return 1;
			}
			wgAppConfig.wgLog(text5);
			return -1;
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x001836F6 File Offset: 0x001826F6
		private void arrPingInfoReset()
		{
			this.arrIPPingOK.Clear();
			this.arrIPPingFailed.Clear();
			this.arrIPARPOK.Clear();
			this.arrIPARPFailed.Clear();
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x00183724 File Offset: 0x00182724
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

		// Token: 0x06001412 RID: 5138 RVA: 0x00183768 File Offset: 0x00182768
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

		// Token: 0x06001413 RID: 5139 RVA: 0x001837F8 File Offset: 0x001827F8
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 1;
			}
			this.updateButtonDisplay();
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x00183864 File Offset: 0x00182864
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

		// Token: 0x06001415 RID: 5141 RVA: 0x00183A83 File Offset: 0x00182A83
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvDoors);
			this.updateButtonDisplay();
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x00183A96 File Offset: 0x00182A96
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
			this.updateButtonDisplay();
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x00183AB0 File Offset: 0x00182AB0
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

		// Token: 0x06001418 RID: 5144 RVA: 0x00184104 File Offset: 0x00183104
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
				this.dfrmWait1.Hide();
				MessageBox.Show(ex.Message, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			this.dfrmWait1.Hide();
			this.displayNewestLog();
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x00184734 File Offset: 0x00183734
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 0;
			}
			this.updateButtonDisplay();
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x001847A0 File Offset: 0x001837A0
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

		// Token: 0x0600141B RID: 5147 RVA: 0x00184888 File Offset: 0x00183888
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

		// Token: 0x0600141C RID: 5148 RVA: 0x00184F24 File Offset: 0x00183F24
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.updateButtonDisplay();
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x00184F37 File Offset: 0x00183F37
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
			this.updateButtonDisplay();
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x00184F50 File Offset: 0x00183F50
		private void btnDeviceManage_Click(object sender, EventArgs e)
		{
			using (dfrmFaceDeviceManagement dfrmFaceDeviceManagement = new dfrmFaceDeviceManagement())
			{
				dfrmFaceDeviceManagement.ShowDialog(this);
			}
			this._fillDeviceGrid();
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x00184F90 File Offset: 0x00183F90
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
										dfrmFaceManagement4Syd.FaceId_Item[] allItems = dfrmFaceManagement4Syd.FaceId_Item.GetAllItems(text);
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
													dfrmFaceManagement4Syd.FaceId_Item[] allItems2 = dfrmFaceManagement4Syd.FaceId_Item.GetAllItems(text);
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

		// Token: 0x06001420 RID: 5152 RVA: 0x0018582C File Offset: 0x0018482C
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
										dfrmFaceManagement4Syd.FaceId_Item[] allItems = dfrmFaceManagement4Syd.FaceId_Item.GetAllItems(text);
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

		// Token: 0x06001421 RID: 5153 RVA: 0x001861A0 File Offset: 0x001851A0
		private void btnExit_Click(object sender, EventArgs e)
		{
			this.bExit = true;
			base.Close();
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x001861B0 File Offset: 0x001851B0
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

		// Token: 0x06001423 RID: 5155 RVA: 0x001862D4 File Offset: 0x001852D4
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
										if (this.progressBar1.Value < this.progressBar1.Maximum)
										{
											this.progressBar1.Value++;
											Application.DoEvents();
										}
										string text4 = "";
										if (wgAppConfig.FileIsExisted(wgAppConfig.Path4Photo() + workNOOnDevice.ToString() + ".jpg"))
										{
											text4 = workNOOnDevice.ToString();
										}
										if (this.updateOneUserByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"]), (long)num3, workNOOnDevice, text3, text4, ref text2, ref array) > 0)
										{
											num++;
										}
										else
										{
											flag = true;
										}
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

		// Token: 0x06001424 RID: 5156 RVA: 0x00186DCC File Offset: 0x00185DCC
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
						if (this.bExit)
						{
							return;
						}
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
					this.dfrmWait1.Hide();
					MessageBox.Show(ex.Message, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				finally
				{
					this.dfrmWait1.Hide();
				}
				this.displayNewestLog();
			}
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00187CC8 File Offset: 0x00186CC8
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

		// Token: 0x06001426 RID: 5158 RVA: 0x00187CF8 File Offset: 0x00186CF8
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

		// Token: 0x06001427 RID: 5159 RVA: 0x00187F48 File Offset: 0x00186F48
		public int checkByHTTP(string strIP, int PORT, string devicePassword)
		{
			string text = string.Format("http://{0}:{1}", strIP, PORT.ToString());
			string text2 = "/getDeviceKey";
			Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds * 1000.0);
			string text3 = text + text2;
			string text4 = "/open";
			text3 = text + text4;
			string text5 = string.Format("key={0}", this.deviceDefaultPassword);
			string text6;
			if ((text6 = this.PushToWeb4Get(text3, text5, Encoding.UTF8)) != null)
			{
				if (text6 == "1")
				{
					return 1;
				}
				if (text6 == "0")
				{
					return 0;
				}
			}
			wgAppConfig.wgLog("updateOneUserByHTTP  failed");
			return -1;
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00188018 File Offset: 0x00187018
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

		// Token: 0x06001429 RID: 5161 RVA: 0x00188041 File Offset: 0x00187041
		private void chkDeleteUsersNotInDB_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00188043 File Offset: 0x00187043
		private void dfrmFaceManage_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00188058 File Offset: 0x00187058
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

		// Token: 0x0600142C RID: 5164 RVA: 0x001880F4 File Offset: 0x001870F4
		private void dfrmFaceManage_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			switch (int.Parse("0" + wgAppConfig.getSystemParamByNO(209)))
			{
			case 1:
			{
				string systemParamNotes = wgAppConfig.getSystemParamNotes(209);
				if (!string.IsNullOrEmpty(systemParamNotes))
				{
					this.deviceDefaultPassword = systemParamNotes.Substring(systemParamNotes.IndexOf(",") + 1);
				}
				break;
			}
			case 2:
			{
				string systemParamNotes2 = wgAppConfig.getSystemParamNotes(209);
				if (!string.IsNullOrEmpty(systemParamNotes2))
				{
					this.deviceDefaultPassword = systemParamNotes2.Substring(systemParamNotes2.IndexOf(",") + 1);
				}
				break;
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
			this.toolStripComboBox1.SelectedIndex = 0;
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_SYD_1")))
			{
				int num = 0;
				if (int.TryParse(wgAppConfig.GetKeyVal("KEY_SYD_1"), out num))
				{
					this.toolStripComboBox1.SelectedIndex = num;
				}
			}
			this.toolStripComboBox2.SelectedIndex = 2;
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_SYD_2")))
			{
				int num2 = 0;
				if (int.TryParse(wgAppConfig.GetKeyVal("KEY_SYD_2"), out num2))
				{
					this.toolStripComboBox2.SelectedIndex = num2;
				}
			}
			this.toolStripComboBox3.SelectedIndex = this.toolStripComboBox3.Items.Count - 1;
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_SYD_3")))
			{
				int num3 = 0;
				if (int.TryParse(wgAppConfig.GetKeyVal("KEY_SYD_3"), out num3))
				{
					this.toolStripComboBox3.SelectedIndex = num3;
				}
			}
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0018860C File Offset: 0x0018760C
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

		// Token: 0x0600142E RID: 5166 RVA: 0x00188644 File Offset: 0x00187644
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x00188654 File Offset: 0x00187654
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

		// Token: 0x06001430 RID: 5168 RVA: 0x00188704 File Offset: 0x00187704
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

		// Token: 0x06001431 RID: 5169 RVA: 0x00188770 File Offset: 0x00187770
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

		// Token: 0x06001432 RID: 5170 RVA: 0x001887A0 File Offset: 0x001877A0
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x001887B0 File Offset: 0x001877B0
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

		// Token: 0x06001434 RID: 5172 RVA: 0x001887E0 File Offset: 0x001877E0
		private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x001887F0 File Offset: 0x001877F0
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

		// Token: 0x06001436 RID: 5174 RVA: 0x00188820 File Offset: 0x00187820
		private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneUser.PerformClick();
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00188830 File Offset: 0x00187830
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

		// Token: 0x06001438 RID: 5176 RVA: 0x001888C2 File Offset: 0x001878C2
		private long getCardNOonDB(long WorkNO, long diff)
		{
			return WorkNO + diff;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x001888C8 File Offset: 0x001878C8
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

		// Token: 0x0600143A RID: 5178 RVA: 0x0018891C File Offset: 0x0018791C
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

		// Token: 0x0600143B RID: 5179 RVA: 0x00188984 File Offset: 0x00187984
		private long getWorkNOOnDevice(long cardno, long diff)
		{
			if (cardno > diff)
			{
				return cardno - diff;
			}
			return cardno;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x00188990 File Offset: 0x00187990
		private bool IsIPCanConnect(string ip)
		{
			return (this.arrIPPingOK.Count > 0 && this.arrIPPingOK.IndexOf(ip) >= 0) || (this.arrIPARPOK.Count > 0 && this.arrIPARPOK.IndexOf(ip) >= 0);
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x001889DE File Offset: 0x001879DE
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x00188A00 File Offset: 0x00187A00
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

		// Token: 0x0600143F RID: 5183 RVA: 0x00188B5C File Offset: 0x00187B5C
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

		// Token: 0x06001440 RID: 5184 RVA: 0x00188DAC File Offset: 0x00187DAC
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

		// Token: 0x06001441 RID: 5185 RVA: 0x00188DE0 File Offset: 0x00187DE0
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

		// Token: 0x06001442 RID: 5186 RVA: 0x00188E98 File Offset: 0x00187E98
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

		// Token: 0x06001443 RID: 5187 RVA: 0x00188F8C File Offset: 0x00187F8C
		public string PushToWeb(string weburl, string data, Encoding encode)
		{
			byte[] bytes = encode.GetBytes(data);
			string text = "";
			try
			{
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
				text = new StreamReader(httpWebResponse.GetResponseStream(), encode).ReadToEnd();
				if (text.IndexOf("\"status\":0") > 0)
				{
					return "1";
				}
				if (text.IndexOf("\"status\":") > 0)
				{
					text = "0";
				}
			}
			catch (Exception)
			{
				throw;
			}
			return text;
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00189064 File Offset: 0x00188064
		public string PushToWeb4Get(string weburl, string data, Encoding encode)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl + "?" + data));
			httpWebRequest.SendChunked = false;
			httpWebRequest.KeepAlive = false;
			httpWebRequest.Method = "GET";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			string text = new StreamReader(httpWebResponse.GetResponseStream(), encode).ReadToEnd();
			if (text.IndexOf("\"status\":0") > 0)
			{
				return "1";
			}
			if (text.IndexOf("\"status\":") > 0)
			{
				text = "0";
			}
			return text;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x001890F8 File Offset: 0x001880F8
		public int setDeviceByHTTP(string strIP, int PORT, string devicePassword)
		{
			string text = string.Format("http://{0}:{1}", strIP, PORT.ToString());
			string text2 = "";
			long num = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds * 1000.0);
			text2 = text + "/setDeviceInfo";
			string text3 = string.Format("key={0}&ts={1}", this.deviceDefaultPassword, num.ToString());
			string text4 = null;
			try
			{
				text4 = "cameraDetectType=0&faceFeaturePairNumber=0.92&faceFeaturePairSuccessOrFailWaitTime=2000&openDoorType=0&openDoorContinueTime=2000&doorType=34&idCardFaceFeaturePairNumber=0.8&appWelcomeMsg=识别成功,@&deviceSoundSize=5&deviceDefendTime=21:50&deviceName=1号机";
			}
			catch
			{
			}
			text3 = string.Format("key={0}&{1}", this.deviceDefaultPassword, text4);
			string text5 = this.PushToWeb(text2, text3, Encoding.UTF8);
			string text6;
			if ((text6 = text5) != null && (text6 == "0" || text6 == "1"))
			{
				return 1;
			}
			wgAppConfig.wgLog(text5);
			return -1;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x001891F0 File Offset: 0x001881F0
		public int setDeviceParamByHTTP(string strIP, int PORT, string devicePassword, string paramInfo)
		{
			string text = string.Format("http://{0}:{1}", strIP, PORT.ToString());
			string text2 = "";
			long num = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds * 1000.0);
			text2 = text + "/setDeviceInfo";
			string text3 = string.Format("key={0}&ts={1}", this.deviceDefaultPassword, num.ToString());
			string text4 = null;
			try
			{
				text4 = paramInfo;
			}
			catch
			{
			}
			text3 = string.Format("key={0}&{1}", this.deviceDefaultPassword, text4);
			string text5 = this.PushToWeb(text2, text3, Encoding.UTF8);
			string text6;
			if ((text6 = text5) != null && (text6 == "0" || text6 == "1"))
			{
				return 1;
			}
			wgAppConfig.wgLog(text5);
			return -1;
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x001892E4 File Offset: 0x001882E4
		private void tcpClose()
		{
			if (this.tcp != null)
			{
				this.tcp.Close();
				this.tcp = null;
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00189300 File Offset: 0x00188300
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
						string keyValue = dfrmFaceManagement4Syd.FaceId_Item.GetKeyValue(strRecvInfo, "result");
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

		// Token: 0x06001449 RID: 5193 RVA: 0x00189698 File Offset: 0x00188698
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

		// Token: 0x0600144A RID: 5194 RVA: 0x0018974C File Offset: 0x0018874C
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
					this.btnDownloadSelectedUsers.Enabled = true;
					this.btnDeleteSelectedUsersFromSelectedDevice.Enabled = true;
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

		// Token: 0x0600144B RID: 5195 RVA: 0x00189838 File Offset: 0x00188838
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

		// Token: 0x0600144C RID: 5196 RVA: 0x001899C0 File Offset: 0x001889C0
		public int updateOneUserByHTTP(string strIP, int PORT, string devicePassword, long userID, long cardNO, string username, string jpgName, ref string strRecvInfo, ref byte[] recvInfo)
		{
			string text = string.Format("http://{0}:{1}", strIP, PORT.ToString());
			string text2 = "/setPerson";
			string text3 = "/removePerson";
			string text4 = "";
			text4 = text + text3;
			string text5 = string.Format("key={0}&id=[{1}]", this.deviceDefaultPassword, (userID == -1L) ? "" : userID.ToString());
			string text6;
			try
			{
				text6 = this.PushToWeb(text4, text5, Encoding.UTF8);
				if (!(text6 == "0") && !(text6 == "1"))
				{
					wgAppConfig.wgLog(text6);
					return -1;
				}
			}
			catch (Exception ex)
			{
				if (dfrmFaceManagement4Syd.errDeleteUserCnt < 3)
				{
					dfrmFaceManagement4Syd.errDeleteUserCnt++;
					wgAppConfig.wgLogWithoutDB(ex.ToString());
				}
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
			text4 = text + text2;
			text5 = string.Format("key={0}&{1}", this.deviceDefaultPassword, string.Format("id={0}&name={2}&IC_NO={1}&ID_NO=", userID, cardNO, username));
			string text7 = "";
			if (!string.IsNullOrEmpty(jpgName))
			{
				using (FileStream fileStream = new FileStream(wgAppConfig.Path4Photo() + jpgName + ".jpg", FileMode.Open, FileAccess.Read))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					text7 = Convert.ToBase64String(array);
					text7 = text7.Replace("/", "%2F");
					text7 = text7.Replace("+", "%2B");
				}
			}
			text5 += string.Format("&photo={0}&passCount=10000&startTs=1&endTs=99999999999", text7);
			text6 = this.PushToWeb(text4, text5, Encoding.UTF8);
			if (text6 == "1")
			{
				return 1;
			}
			if (text6 == "-1")
			{
				wgAppConfig.wgLog("updateOneUserByHTTP  failed");
			}
			else
			{
				wgAppConfig.wgLog(text6);
			}
			recvInfo = null;
			strRecvInfo = "";
			return -1;
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00189BE4 File Offset: 0x00188BE4
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

		// Token: 0x0600144E RID: 5198 RVA: 0x00189CB8 File Offset: 0x00188CB8
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

		// Token: 0x0600144F RID: 5199 RVA: 0x00189F30 File Offset: 0x00188F30
		private void uploadConfigure(string sendername, string config)
		{
			if (XMessageBox.Show(this, string.Format(string.Concat(new string[]
			{
				CommonStr.strAreYouSure,
				" {0}?\r\n\r\n",
				CommonStr.strDevicesNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString()
			}), sendername), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					sendername,
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString()
				}));
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
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), sendername), CommonStr.strStart);
									if (this.setDeviceParamByHTTP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), wgTools.SetObjToStr(dataRowView2["f_DevicePassword"]), config) > 0)
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), sendername), string.Format("{0} {1}. ", sendername, CommonStr.strSuccessfully));
									}
									else
									{
										this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), sendername), string.Format("{0}--{1}:{2}:{3}", new object[]
										{
											CommonStr.strCommFail,
											wgTools.SetObjToStr(dataRowView2["f_DeviceName"]),
											wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
											wgTools.SetObjToStr(dataRowView2["f_DevicePort"])
										}), 101);
									}
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), sendername), string.Format("{0} {1}. ", sendername, CommonStr.strSuccessfully));
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
								this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView3["f_DeviceName"]), sendername), string.Format("{0}--{1}:{2}:{3}", new object[]
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
						sendername.Replace("\r\n", ""),
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

		// Token: 0x06001450 RID: 5200 RVA: 0x0018A560 File Offset: 0x00189560
		private void uploadConfigure1ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = this.toolStripComboBox1.Items[this.toolStripComboBox1.SelectedIndex].ToString();
			text = text.Substring(0, text.IndexOf(":"));
			text = "openDoorType=" + text.Trim();
			wgAppConfig.UpdateKeyVal("KEY_SYD_1", this.toolStripComboBox1.SelectedIndex.ToString());
			wgAppConfig.wgLog("设置开门条件:" + text);
			this.uploadConfigure("设置开门条件", text);
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0018A5F4 File Offset: 0x001895F4
		private void uploadConfigure2ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = this.toolStripComboBox2.Items[this.toolStripComboBox2.SelectedIndex].ToString();
			text = text.Substring(0, text.IndexOf(":"));
			text = "doorType=" + text.Trim();
			wgAppConfig.UpdateKeyVal("KEY_SYD_2", this.toolStripComboBox2.SelectedIndex.ToString());
			wgAppConfig.wgLog("设置开门类型(输出信号):" + text);
			this.uploadConfigure("设置开门类型(输出信号)", text);
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0018A688 File Offset: 0x00189688
		private void uploadConfigure3ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = this.toolStripComboBox3.Items[this.toolStripComboBox3.SelectedIndex].ToString();
			text = "deviceSoundSize=" + text.Trim();
			wgAppConfig.UpdateKeyVal("KEY_SYD_3", this.toolStripComboBox3.SelectedIndex.ToString());
			wgAppConfig.wgLog("设置音量: " + text);
			this.uploadConfigure("设置音量", text);
		}

		// Token: 0x04002A63 RID: 10851
		public const int DeviceCodePage = 936;

		// Token: 0x04002A64 RID: 10852
		private bool bDataErrorExist;

		// Token: 0x04002A65 RID: 10853
		private bool bExit;

		// Token: 0x04002A66 RID: 10854
		private Control defaultFindControl;

		// Token: 0x04002A67 RID: 10855
		private DataTable dt;

		// Token: 0x04002A68 RID: 10856
		private DataView dv;

		// Token: 0x04002A69 RID: 10857
		private DataView dv1;

		// Token: 0x04002A6A RID: 10858
		private DataView dv2;

		// Token: 0x04002A6B RID: 10859
		private DataView dvSelected;

		// Token: 0x04002A6C RID: 10860
		private int eventRecID;

		// Token: 0x04002A6D RID: 10861
		private int nametablecnt;

		// Token: 0x04002A6E RID: 10862
		private DataTable tbRunInfoLog;

		// Token: 0x04002A6F RID: 10863
		private TcpClient tcp;

		// Token: 0x04002A70 RID: 10864
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04002A71 RID: 10865
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04002A72 RID: 10866
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04002A73 RID: 10867
		private ArrayList arrIPARPFailed = new ArrayList();

		// Token: 0x04002A74 RID: 10868
		private ArrayList arrIPARPOK = new ArrayList();

		// Token: 0x04002A75 RID: 10869
		private ArrayList arrIPPingFailed = new ArrayList();

		// Token: 0x04002A76 RID: 10870
		private ArrayList arrIPPingOK = new ArrayList();

		// Token: 0x04002A77 RID: 10871
		private bool bStarting = true;

		// Token: 0x04002A78 RID: 10872
		private string deviceDefaultPassword = "abc";

		// Token: 0x04002A79 RID: 10873
		private dfrmWait dfrmWait1 = new dfrmWait();

		// Token: 0x04002A7A RID: 10874
		private static int errDeleteUserCnt;

		// Token: 0x04002A7B RID: 10875
		public string factoryName = "Hanvon";

		// Token: 0x04002A7C RID: 10876
		private string strGroupFilter = "";

		// Token: 0x04002A7D RID: 10877
		private int tcpBuffSize = 204800;

		// Token: 0x020002D9 RID: 729
		public class FaceId_Item
		{
			// Token: 0x06001455 RID: 5205 RVA: 0x0018D41A File Offset: 0x0018C41A
			public FaceId_Item(string keyName, string keyValue)
			{
				this.Name = keyName;
				this.Value = keyValue;
			}

			// Token: 0x06001456 RID: 5206 RVA: 0x0018D430 File Offset: 0x0018C430
			public static dfrmFaceManagement4Syd.FaceId_Item[] GetAllItems(string Answer)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					List<dfrmFaceManagement4Syd.FaceId_Item> list = new List<dfrmFaceManagement4Syd.FaceId_Item>();
					string text = "\\b([a-z|A-Z|_]+)\\s*=\\s*\"([^\"]+)\"";
					MatchCollection matchCollection = Regex.Matches(Answer, text);
					if (matchCollection != null)
					{
						foreach (object obj in matchCollection)
						{
							Match match = (Match)obj;
							list.Add(new dfrmFaceManagement4Syd.FaceId_Item(match.Groups[1].Value, match.Groups[2].Value));
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
				return null;
			}

			// Token: 0x06001457 RID: 5207 RVA: 0x0018D4E4 File Offset: 0x0018C4E4
			public static dfrmFaceManagement4Syd.FaceId_Item[] GetAllPairs(string Answer, string LeftKeyName, string RightKeyName)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					List<dfrmFaceManagement4Syd.FaceId_Item> list = new List<dfrmFaceManagement4Syd.FaceId_Item>();
					string text = string.Concat(new string[] { "\\b", LeftKeyName, "=\"([^\"]+)\"\\s*", RightKeyName, "=\"([^\"]+)\"" });
					MatchCollection matchCollection = Regex.Matches(Answer, text);
					if (matchCollection != null)
					{
						foreach (object obj in matchCollection)
						{
							Match match = (Match)obj;
							list.Add(new dfrmFaceManagement4Syd.FaceId_Item(match.Groups[1].Value, match.Groups[2].Value));
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
				return null;
			}

			// Token: 0x06001458 RID: 5208 RVA: 0x0018D5C8 File Offset: 0x0018C5C8
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

			// Token: 0x06001459 RID: 5209 RVA: 0x0018D614 File Offset: 0x0018C614
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

			// Token: 0x04002AD3 RID: 10963
			public readonly string Name;

			// Token: 0x04002AD4 RID: 10964
			public readonly string Value;
		}
	}
}
