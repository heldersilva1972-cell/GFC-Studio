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
	// Token: 0x020002D2 RID: 722
	public partial class dfrmFaceManagement4Hanvon : frmN3000
	{
		// Token: 0x06001362 RID: 4962 RVA: 0x0016EB30 File Offset: 0x0016DB30
		public dfrmFaceManagement4Hanvon()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			wgAppConfig.custDataGridview(ref this.dgvRunInfo);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0016EC1C File Offset: 0x0016DC1C
		private void _fillDeviceGrid()
		{
			try
			{
				string text = " SELECT ";
				text = text + " f_DeviceId , f_DeviceName  , 0 as f_Selected, f_DeviceIP , f_DevicePort , f_ReaderName , f_CardNOWorkNODiff , f_Notes   from t_b_ThirdPartyNetDevice LEFT JOIN t_b_Reader ON t_b_Reader.f_ReaderID = t_b_ThirdPartyNetDevice.f_ReaderID  WHERE f_factory =   " + wgTools.PrepareStrNUnicode(this.factoryName) + " ORDER BY f_DeviceName  ";
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

		// Token: 0x06001364 RID: 4964 RVA: 0x0016EF1C File Offset: 0x0016DF1C
		private void addLogEvent(string desc, string info)
		{
			this.addLogEvent(desc, info, 100);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0016EF28 File Offset: 0x0016DF28
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

		// Token: 0x06001366 RID: 4966 RVA: 0x0016F0DC File Offset: 0x0016E0DC
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

		// Token: 0x06001367 RID: 4967 RVA: 0x0016F6C4 File Offset: 0x0016E6C4
		private void arrPingInfoReset()
		{
			this.arrIPPingOK.Clear();
			this.arrIPPingFailed.Clear();
			this.arrIPARPOK.Clear();
			this.arrIPARPFailed.Clear();
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0016F6F4 File Offset: 0x0016E6F4
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

		// Token: 0x06001369 RID: 4969 RVA: 0x0016F738 File Offset: 0x0016E738
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
			this.updateUserRegister();
			this.defaultFindControl = this.dgvUsers;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0016F7C8 File Offset: 0x0016E7C8
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 1;
			}
			this.updateButtonDisplay();
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0016F834 File Offset: 0x0016E834
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

		// Token: 0x0600136C RID: 4972 RVA: 0x0016FA53 File Offset: 0x0016EA53
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvDoors);
			this.updateButtonDisplay();
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0016FA66 File Offset: 0x0016EA66
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
			this.updateButtonDisplay();
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x0016FA80 File Offset: 0x0016EA80
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
									string text = "";
									byte[] array = null;
									this.tcpClose();
									DateTime now = DateTime.Now;
									string text2 = now.ToString("yyyy-MM-dd");
									if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), string.Format("SetDeviceInfo(time=\"{0} {1}\" )", text2, now.ToString("HH:mm:ss")), ref text, ref array) > 0)
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

		// Token: 0x0600136F RID: 4975 RVA: 0x00170104 File Offset: 0x0016F104
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
								byte[] array = null;
								this.tcpClose();
								DateTime now = DateTime.Now;
								if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), string.Format("GetDeviceInfo()", new object[0]), ref text, ref array) > 0)
								{
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}. ", (sender as Button).Text, CommonStr.strSuccessfully));
									wgAppConfig.wgLog(string.Format("[{0}]{1}:{2}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text, text));
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
			this.displayNewestLog();
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0017067C File Offset: 0x0016F67C
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 0;
			}
			this.updateButtonDisplay();
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x001706E8 File Offset: 0x0016F6E8
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

		// Token: 0x06001372 RID: 4978 RVA: 0x001707D0 File Offset: 0x0016F7D0
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
									if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), string.Format("DeleteAllEmployee()", new object[0]), ref text, ref array) > 0)
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

		// Token: 0x06001373 RID: 4979 RVA: 0x00170E50 File Offset: 0x0016FE50
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.updateButtonDisplay();
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00170E63 File Offset: 0x0016FE63
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
			this.updateButtonDisplay();
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x00170E7C File Offset: 0x0016FE7C
		private void btnDeviceManage_Click(object sender, EventArgs e)
		{
			using (dfrmFaceDeviceManagement dfrmFaceDeviceManagement = new dfrmFaceDeviceManagement())
			{
				dfrmFaceDeviceManagement.ShowDialog(this);
			}
			this._fillDeviceGrid();
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x00170EBC File Offset: 0x0016FEBC
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
										dfrmFaceManagement4Hanvon.FaceId_Item[] allItems = dfrmFaceManagement4Hanvon.FaceId_Item.GetAllItems(text);
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
													dfrmFaceManagement4Hanvon.FaceId_Item[] allItems2 = dfrmFaceManagement4Hanvon.FaceId_Item.GetAllItems(text);
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

		// Token: 0x06001377 RID: 4983 RVA: 0x00171758 File Offset: 0x00170758
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
										dfrmFaceManagement4Hanvon.FaceId_Item[] allItems = dfrmFaceManagement4Hanvon.FaceId_Item.GetAllItems(text);
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
					this.updateUserRegister();
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

		// Token: 0x06001378 RID: 4984 RVA: 0x001720CC File Offset: 0x001710CC
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x001720D4 File Offset: 0x001710D4
		private void btnHaveFace_Click(object sender, EventArgs e)
		{
			this.btnDelAllUsers_Click(null, null);
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
					goto IL_00DB;
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
			IL_00DB:
			DataView dataView = new DataView(dataTable);
			if (dataView.Count > 0)
			{
				using (DataTable table = ((DataView)this.dgvUsers.DataSource).Table)
				{
					for (int i = 0; i < table.Rows.Count; i++)
					{
						if (!string.IsNullOrEmpty(wgTools.SetObjToStr(table.Rows[i]["f_CardNO"])))
						{
							dataView.RowFilter = "f_ConsumerID = " + table.Rows[i]["f_ConsumerID"];
							if (sender == this.btnHaveFace)
							{
								if (dataView.Count > 0)
								{
									table.Rows[i]["f_Selected"] = icConsumerShare.iSelectedCurrentNoneMax + 1;
								}
							}
							else if (sender == this.btnNoFace && dataView.Count == 0)
							{
								table.Rows[i]["f_Selected"] = icConsumerShare.iSelectedCurrentNoneMax + 1;
							}
						}
					}
					goto IL_028C;
				}
			}
			using (DataTable table2 = ((DataView)this.dgvUsers.DataSource).Table)
			{
				for (int j = 0; j < table2.Rows.Count; j++)
				{
					if (!string.IsNullOrEmpty(wgTools.SetObjToStr(table2.Rows[j]["f_CardNO"])) && sender == this.btnNoFace && dataView.Count == 0)
					{
						table2.Rows[j]["f_Selected"] = icConsumerShare.iSelectedCurrentNoneMax + 1;
					}
				}
			}
			IL_028C:
			this.updateButtonDisplay();
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x001723D8 File Offset: 0x001713D8
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
									bool flag = false;
									this.tcpClose();
									if (@checked)
									{
										string text3 = "";
										for (int j = 0; j < dataTable.Rows.Count; j++)
										{
											long workNOOnDevice = this.getWorkNOOnDevice(long.Parse(wgTools.SetObjToStr(dataTable.Rows[j]["f_CardNO"])), long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"])));
											int.Parse(wgTools.SetObjToStr(dataTable.Rows[j]["f_ConsumerID"]));
											string validUserName = this.getValidUserName(wgTools.SetObjToStr(dataTable.Rows[j]["f_ConsumerName"]));
											text3 += string.Format("{0}=\"{1}\" ", workNOOnDevice, validUserName);
											if ((j & 15) == 0)
											{
												Application.DoEvents();
											}
										}
										this.nametablecnt = dataTable.Rows.Count;
										this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), string.Format("SetNameTable({0})", text3), ref text2, ref array);
									}
									if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), "GetEmployeeID()", ref text2, ref array) > 0)
									{
										dfrmFaceManagement4Hanvon.FaceId_Item[] allItems = dfrmFaceManagement4Hanvon.FaceId_Item.GetAllItems(text2);
										if (allItems != null)
										{
											int num = 0;
											int num2 = 0;
											ArrayList arrayList2 = new ArrayList();
											for (int k = 0; k < allItems.Length; k++)
											{
												if (allItems[k].Name == "id")
												{
													arrayList2.Add(allItems[k].Value);
												}
											}
											int num3 = 0;
											bool flag2 = false;
											int l = 0;
											while (l < dataTable.Rows.Count)
											{
												long workNOOnDevice2 = this.getWorkNOOnDevice(long.Parse(wgTools.SetObjToStr(dataTable.Rows[l]["f_CardNO"])), long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"])));
												int num4 = int.Parse(wgTools.SetObjToStr(dataTable.Rows[l]["f_ConsumerID"]));
												string validUserName2 = this.getValidUserName(wgTools.SetObjToStr(dataTable.Rows[l]["f_ConsumerName"]));
												string text4 = "";
												string text5 = "";
												text5 = wgTools.SetObjToStr(workNOOnDevice2);
												string text6 = wgAppConfig.getValStringBySql("SELECT f_FaceInfo From t_b_consumer_face WHERE f_ConsumerID =" + num4.ToString());
												if (arrayList2.IndexOf(workNOOnDevice2.ToString()) < 0)
												{
													goto IL_07CD;
												}
												num3++;
												if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), "GetEmployee(id=\"" + text5 + "\")", ref text4, ref array) <= 0 || string.IsNullOrEmpty(text4))
												{
													goto IL_07CD;
												}
												if (!string.IsNullOrEmpty(text6))
												{
													int num5 = text6.IndexOf("face_data=\"");
													if (num5 <= 0 || !text4.Substring(text4.IndexOf("face_data=\"")).Equals(text6.Substring(num5)) || !validUserName2.Equals(this.getConsumerNameInFace(text4)))
													{
														goto IL_07CD;
													}
													num2++;
													if (this.progressBar1.Value < this.progressBar1.Maximum)
													{
														this.progressBar1.Value++;
														Application.DoEvents();
													}
												}
												else
												{
													if (text6.IndexOf("face_data=\"") >= 0 || !validUserName2.Equals(this.getConsumerNameInFace(text4)))
													{
														goto IL_07CD;
													}
													num2++;
													if (this.progressBar1.Value < this.progressBar1.Maximum)
													{
														this.progressBar1.Value++;
														Application.DoEvents();
													}
												}
												IL_0A12:
												l++;
												continue;
												IL_07CD:
												if (string.IsNullOrEmpty(text6))
												{
													num2++;
													if (this.progressBar1.Value < this.progressBar1.Maximum)
													{
														this.progressBar1.Value++;
														Application.DoEvents();
														goto IL_0A12;
													}
													goto IL_0A12;
												}
												else
												{
													string text7 = "Return[(]result=\"success\"\\s+(id=\"([a-z|A-Z|0-9]+)\"\\s+[^)]+)[)]";
													MatchCollection matchCollection = Regex.Matches(text6, text7);
													if (matchCollection != null)
													{
														using (IEnumerator enumerator3 = matchCollection.GetEnumerator())
														{
															if (enumerator3.MoveNext())
															{
																Match match = (Match)enumerator3.Current;
																text6 = match.Groups[1].Value;
															}
														}
													}
													if (string.IsNullOrEmpty(text6))
													{
														num2++;
														if (this.progressBar1.Value < this.progressBar1.Maximum)
														{
															this.progressBar1.Value++;
															Application.DoEvents();
															goto IL_0A12;
														}
														goto IL_0A12;
													}
													else
													{
														string text8 = "name=\"";
														int num6 = text6.IndexOf(text8);
														if (num6 > 0)
														{
															num6 += text8.Length;
															int num7 = text6.IndexOf("\"", num6);
															if (num7 > 0)
															{
																text6 = text6.Substring(num7 + 2);
															}
														}
														string text9 = string.Format("SetEmployee(id=\"{0}\" name=\"{1}\" {2})", text5, validUserName2, text6);
														if (this.progressBar1.Value < this.progressBar1.Maximum)
														{
															this.progressBar1.Value++;
															Application.DoEvents();
														}
														if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), text9, ref text2, ref array) > 0)
														{
															num++;
															goto IL_0A12;
														}
														flag2 = true;
														wgAppConfig.wgLog(string.Format("DeviceIP={0},DevicePort={1},Answer={2},recvInfo={3}, Command={4}", new object[]
														{
															wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
															int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])),
															wgTools.SetObjToStr(text2),
															(array == null) ? "" : Encoding.GetEncoding(936).GetString(array),
															text9
														}));
														break;
													}
												}
											}
											int num8 = 0;
											if (!flag2 && arrayList2.Count > 0 && @checked)
											{
												this.progressBar1.Maximum += Math.Max(arrayList2.Count - num3, 0);
												DataView dataView2 = new DataView(dataTable);
												for (int m = 0; m < arrayList2.Count; m++)
												{
													dataView2.RowFilter = string.Format("f_CardNO={0}", this.getCardNOonDB(long.Parse(wgTools.SetObjToStr(arrayList2[m])), long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"]))));
													if (dataView2.Count == 0)
													{
														if (this.progressBar1.Value < this.progressBar1.Maximum)
														{
															this.progressBar1.Value++;
															Application.DoEvents();
														}
														string text10 = string.Format("DeleteEmployee(id=\"{0}\")", arrayList2[m]);
														if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), text10, ref text2, ref array) > 0)
														{
															num8++;
														}
													}
												}
											}
											flag = true;
											if (flag2)
											{
												this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[U{2}/S{3}/D{4}] (T{5})", new object[]
												{
													(sender as Button).Text,
													CommonStr.strFailed,
													num,
													num2,
													num8,
													dataTable.Rows.Count
												}), 101);
											}
											else
											{
												this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[U{2}/S{3}/D{4}] ", new object[]
												{
													(sender as Button).Text,
													CommonStr.strSuccessfully,
													num,
													num2,
													num8
												}));
											}
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

		// Token: 0x0600137B RID: 4987 RVA: 0x00173430 File Offset: 0x00172430
		private void btnUploadSelectedUsers_Click(object sender, EventArgs e)
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
			}), (sender as Button).Text.Replace("\r\n", "")), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
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
							goto IL_0185;
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
					IL_0185:
					ArrayList arrayList = new ArrayList();
					ArrayList arrayList2 = new ArrayList();
					ArrayList arrayList3 = new ArrayList();
					int num = 0;
					int num2 = 0;
					string text2 = "";
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
						}
						num++;
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
					ArrayList arrayList4 = new ArrayList();
					if (this.arrIPARPOK.Count != 0 || this.arrIPPingOK.Count != 0)
					{
						for (int i = 0; i < 2; i++)
						{
							foreach (object obj2 in dataView)
							{
								DataRowView dataRowView2 = (DataRowView)obj2;
								if (this.IsIPCanConnect(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"])) && arrayList4.IndexOf(dataRowView2["f_DeviceName"]) < 0)
								{
									arrayList4.Add(dataRowView2["f_DeviceName"]);
									this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), CommonStr.strStart);
									string text3 = "";
									byte[] array = null;
									bool flag = false;
									this.tcpClose();
									string text4 = "";
									for (int j = 0; j < dataTable.Rows.Count; j++)
									{
										long workNOOnDevice = this.getWorkNOOnDevice(long.Parse(wgTools.SetObjToStr(dataTable.Rows[j]["f_CardNO"])), long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"])));
										int.Parse(wgTools.SetObjToStr(dataTable.Rows[j]["f_ConsumerID"]));
										string validUserName = this.getValidUserName(wgTools.SetObjToStr(dataTable.Rows[j]["f_ConsumerName"]));
										text4 += string.Format("{0}=\"{1}\" ", workNOOnDevice, validUserName);
										if ((j & 15) == 0)
										{
											Application.DoEvents();
										}
									}
									if (!string.IsNullOrEmpty(text4))
									{
										this.nametablecnt = dataTable.Rows.Count;
										this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), string.Format("SetNameTable({0})", text4), ref text3, ref array);
									}
									if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), "GetEmployeeID()", ref text3, ref array) > 0)
									{
										dfrmFaceManagement4Hanvon.FaceId_Item[] allItems = dfrmFaceManagement4Hanvon.FaceId_Item.GetAllItems(text3);
										if (allItems != null)
										{
											int num3 = 0;
											int num4 = 0;
											ArrayList arrayList5 = new ArrayList();
											for (int k = 0; k < allItems.Length; k++)
											{
												if (allItems[k].Name == "id")
												{
													arrayList5.Add(allItems[k].Value);
												}
											}
											bool flag2 = false;
											int l = 0;
											while (l < arrayList.Count)
											{
												long workNOOnDevice2 = this.getWorkNOOnDevice(long.Parse(wgTools.SetObjToStr(arrayList[l])), long.Parse(wgTools.SetObjToStr(dataRowView2["f_CardNOWorkNODiff"])));
												int num5 = int.Parse(wgTools.SetObjToStr(arrayList2[l]));
												string text5 = wgTools.SetObjToStr(arrayList3[l]);
												string text6 = "";
												string text7 = "";
												text7 = wgTools.SetObjToStr(workNOOnDevice2);
												string text8 = wgAppConfig.getValStringBySql("SELECT f_FaceInfo From t_b_consumer_face WHERE f_ConsumerID =" + num5.ToString());
												if (arrayList5.IndexOf(workNOOnDevice2.ToString()) < 0 || this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), "GetEmployee(id=\"" + text7 + "\")", ref text6, ref array) <= 0 || string.IsNullOrEmpty(text6))
												{
													goto IL_09C0;
												}
												if (!string.IsNullOrEmpty(text8))
												{
													int num6 = text8.IndexOf("face_data=\"");
													if (num6 <= 0 || !text6.Substring(text6.IndexOf("face_data=\"")).Equals(text8.Substring(num6)) || !text5.Equals(this.getConsumerNameInFace(text6)))
													{
														goto IL_09C0;
													}
													num4++;
													if (this.progressBar1.Value < this.progressBar1.Maximum)
													{
														this.progressBar1.Value++;
														Application.DoEvents();
													}
												}
												else
												{
													if (text8.IndexOf("face_data=\"") >= 0 || !text5.Equals(this.getConsumerNameInFace(text6)))
													{
														goto IL_09C0;
													}
													num4++;
													if (this.progressBar1.Value < this.progressBar1.Maximum)
													{
														this.progressBar1.Value++;
														Application.DoEvents();
													}
												}
												IL_0B9F:
												l++;
												continue;
												IL_09C0:
												if (string.IsNullOrEmpty(text8))
												{
													num3++;
													goto IL_0B9F;
												}
												string text9 = "Return[(]result=\"success\"\\s+(id=\"([a-z|A-Z|0-9]+)\"\\s+[^)]+)[)]";
												MatchCollection matchCollection = Regex.Matches(text8, text9);
												if (matchCollection != null)
												{
													using (IEnumerator enumerator3 = matchCollection.GetEnumerator())
													{
														if (enumerator3.MoveNext())
														{
															Match match = (Match)enumerator3.Current;
															text8 = match.Groups[1].Value;
														}
													}
												}
												if (string.IsNullOrEmpty(text8))
												{
													num3++;
													goto IL_0B9F;
												}
												string text10 = "name=\"";
												int num7 = text8.IndexOf(text10);
												if (num7 > 0)
												{
													num7 += text10.Length;
													int num8 = text8.IndexOf("\"", num7);
													if (num8 > 0)
													{
														text8 = text8.Substring(num8 + 2);
													}
												}
												string text11 = string.Format("SetEmployee(id=\"{0}\" name=\"{1}\" {2})", text7, text5, text8);
												if (this.progressBar1.Value < this.progressBar1.Maximum)
												{
													this.progressBar1.Value++;
													Application.DoEvents();
												}
												if (this.ThirdPartyDeviceFaceCommIP_TCP(wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]), int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])), text11, ref text3, ref array) > 0)
												{
													num3++;
													goto IL_0B9F;
												}
												flag2 = true;
												wgAppConfig.wgLog(string.Format("DeviceIP={0},DevicePort={1},Answer={2},recvInfo={3}, Command={4}", new object[]
												{
													wgTools.SetObjToStr(dataRowView2["f_DeviceIP"]),
													int.Parse(wgTools.SetObjToStr(dataRowView2["f_DevicePort"])),
													wgTools.SetObjToStr(text3),
													(array == null) ? "" : Encoding.GetEncoding(936).GetString(array),
													text11
												}));
												break;
											}
											flag = true;
											if (flag2)
											{
												this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[U{2}/S{3}]  (T{4})", new object[]
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
												this.addLogEvent(string.Format("[{0}]{1}", wgTools.SetObjToStr(dataRowView2["f_DeviceName"]), (sender as Button).Text), string.Format("{0} {1}.[U{2}/S{3}] ", new object[]
												{
													(sender as Button).Text,
													CommonStr.strSuccessfully,
													num3,
													num4
												}));
											}
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
							if (arrayList4.Count == dataView.Count)
							{
								break;
							}
							Thread.Sleep(300);
						}
					}
					if (arrayList4.Count != dataView.Count)
					{
						foreach (object obj3 in dataView)
						{
							DataRowView dataRowView3 = (DataRowView)obj3;
							if (arrayList4.IndexOf(dataRowView3["f_DeviceName"]) < 0)
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

		// Token: 0x0600137C RID: 4988 RVA: 0x00174588 File Offset: 0x00173588
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

		// Token: 0x0600137D RID: 4989 RVA: 0x001745B8 File Offset: 0x001735B8
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

		// Token: 0x0600137E RID: 4990 RVA: 0x00174808 File Offset: 0x00173808
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

		// Token: 0x0600137F RID: 4991 RVA: 0x00174831 File Offset: 0x00173831
		private void chkDeleteUsersNotInDB_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00174833 File Offset: 0x00173833
		private void dfrmFaceManage_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00174848 File Offset: 0x00173848
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

		// Token: 0x06001382 RID: 4994 RVA: 0x001748E4 File Offset: 0x001738E4
		private void dfrmFaceManage_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
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

		// Token: 0x06001383 RID: 4995 RVA: 0x00174C98 File Offset: 0x00173C98
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

		// Token: 0x06001384 RID: 4996 RVA: 0x00174CD0 File Offset: 0x00173CD0
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00174CE0 File Offset: 0x00173CE0
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

		// Token: 0x06001386 RID: 4998 RVA: 0x00174D90 File Offset: 0x00173D90
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

		// Token: 0x06001387 RID: 4999 RVA: 0x00174DFC File Offset: 0x00173DFC
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

		// Token: 0x06001388 RID: 5000 RVA: 0x00174E2C File Offset: 0x00173E2C
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00174E3C File Offset: 0x00173E3C
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

		// Token: 0x0600138A RID: 5002 RVA: 0x00174E6C File Offset: 0x00173E6C
		private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x00174E7C File Offset: 0x00173E7C
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

		// Token: 0x0600138C RID: 5004 RVA: 0x00174EAC File Offset: 0x00173EAC
		private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneUser.PerformClick();
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00174EBC File Offset: 0x00173EBC
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

		// Token: 0x0600138E RID: 5006 RVA: 0x00174F4E File Offset: 0x00173F4E
		private long getCardNOonDB(long WorkNO, long diff)
		{
			return WorkNO + diff;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00174F54 File Offset: 0x00173F54
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

		// Token: 0x06001390 RID: 5008 RVA: 0x00174FA8 File Offset: 0x00173FA8
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

		// Token: 0x06001391 RID: 5009 RVA: 0x00175010 File Offset: 0x00174010
		private long getWorkNOOnDevice(long cardno, long diff)
		{
			if (cardno > diff)
			{
				return cardno - diff;
			}
			return cardno;
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0017501C File Offset: 0x0017401C
		private bool IsIPCanConnect(string ip)
		{
			return (this.arrIPPingOK.Count > 0 && this.arrIPPingOK.IndexOf(ip) >= 0) || (this.arrIPARPOK.Count > 0 && this.arrIPARPOK.IndexOf(ip) >= 0);
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0017506A File Offset: 0x0017406A
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0017508C File Offset: 0x0017408C
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

		// Token: 0x06001395 RID: 5013 RVA: 0x001751E8 File Offset: 0x001741E8
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

		// Token: 0x06001396 RID: 5014 RVA: 0x00175438 File Offset: 0x00174438
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

		// Token: 0x06001397 RID: 5015 RVA: 0x0017546C File Offset: 0x0017446C
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

		// Token: 0x06001398 RID: 5016 RVA: 0x00175524 File Offset: 0x00174524
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

		// Token: 0x06001399 RID: 5017 RVA: 0x00175618 File Offset: 0x00174618
		private void tcpClose()
		{
			if (this.tcp != null)
			{
				this.tcp.Close();
				this.tcp = null;
			}
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00175634 File Offset: 0x00174634
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
						string keyValue = dfrmFaceManagement4Hanvon.FaceId_Item.GetKeyValue(strRecvInfo, "result");
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

		// Token: 0x0600139B RID: 5019 RVA: 0x001759CC File Offset: 0x001749CC
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

		// Token: 0x0600139C RID: 5020 RVA: 0x00175A80 File Offset: 0x00174A80
		private void updateButtonDisplay()
		{
			this.btnUploadSelectedUsers.Enabled = false;
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

		// Token: 0x0600139D RID: 5021 RVA: 0x00175B54 File Offset: 0x00174B54
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

		// Token: 0x0600139E RID: 5022 RVA: 0x00175CDC File Offset: 0x00174CDC
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

		// Token: 0x04002989 RID: 10633
		public const int DeviceCodePage = 936;

		// Token: 0x0400298A RID: 10634
		private bool bDataErrorExist;

		// Token: 0x0400298B RID: 10635
		private Control defaultFindControl;

		// Token: 0x0400298C RID: 10636
		private DataTable dt;

		// Token: 0x0400298D RID: 10637
		private DataView dv;

		// Token: 0x0400298E RID: 10638
		private DataView dv1;

		// Token: 0x0400298F RID: 10639
		private DataView dv2;

		// Token: 0x04002990 RID: 10640
		private DataView dvSelected;

		// Token: 0x04002991 RID: 10641
		private int eventRecID;

		// Token: 0x04002992 RID: 10642
		private int nametablecnt;

		// Token: 0x04002993 RID: 10643
		private DataTable tbRunInfoLog;

		// Token: 0x04002994 RID: 10644
		private TcpClient tcp;

		// Token: 0x04002995 RID: 10645
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04002996 RID: 10646
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04002997 RID: 10647
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04002998 RID: 10648
		private ArrayList arrIPARPFailed = new ArrayList();

		// Token: 0x04002999 RID: 10649
		private ArrayList arrIPARPOK = new ArrayList();

		// Token: 0x0400299A RID: 10650
		private ArrayList arrIPPingFailed = new ArrayList();

		// Token: 0x0400299B RID: 10651
		private ArrayList arrIPPingOK = new ArrayList();

		// Token: 0x0400299C RID: 10652
		private bool bStarting = true;

		// Token: 0x0400299D RID: 10653
		private dfrmWait dfrmWait1 = new dfrmWait();

		// Token: 0x0400299E RID: 10654
		public string factoryName = "Hanvon";

		// Token: 0x0400299F RID: 10655
		private string strGroupFilter = "";

		// Token: 0x040029A0 RID: 10656
		private int tcpBuffSize = 204800;

		// Token: 0x020002D3 RID: 723
		public class FaceId_Item
		{
			// Token: 0x060013A1 RID: 5025 RVA: 0x00178704 File Offset: 0x00177704
			public FaceId_Item(string keyName, string keyValue)
			{
				this.Name = keyName;
				this.Value = keyValue;
			}

			// Token: 0x060013A2 RID: 5026 RVA: 0x0017871C File Offset: 0x0017771C
			public static dfrmFaceManagement4Hanvon.FaceId_Item[] GetAllItems(string Answer)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					List<dfrmFaceManagement4Hanvon.FaceId_Item> list = new List<dfrmFaceManagement4Hanvon.FaceId_Item>();
					string text = "\\b([a-z|A-Z|_]+)\\s*=\\s*\"([^\"]+)\"";
					MatchCollection matchCollection = Regex.Matches(Answer, text);
					if (matchCollection != null)
					{
						foreach (object obj in matchCollection)
						{
							Match match = (Match)obj;
							list.Add(new dfrmFaceManagement4Hanvon.FaceId_Item(match.Groups[1].Value, match.Groups[2].Value));
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
				return null;
			}

			// Token: 0x060013A3 RID: 5027 RVA: 0x001787D0 File Offset: 0x001777D0
			public static dfrmFaceManagement4Hanvon.FaceId_Item[] GetAllPairs(string Answer, string LeftKeyName, string RightKeyName)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					List<dfrmFaceManagement4Hanvon.FaceId_Item> list = new List<dfrmFaceManagement4Hanvon.FaceId_Item>();
					string text = string.Concat(new string[] { "\\b", LeftKeyName, "=\"([^\"]+)\"\\s*", RightKeyName, "=\"([^\"]+)\"" });
					MatchCollection matchCollection = Regex.Matches(Answer, text);
					if (matchCollection != null)
					{
						foreach (object obj in matchCollection)
						{
							Match match = (Match)obj;
							list.Add(new dfrmFaceManagement4Hanvon.FaceId_Item(match.Groups[1].Value, match.Groups[2].Value));
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
				return null;
			}

			// Token: 0x060013A4 RID: 5028 RVA: 0x001788B4 File Offset: 0x001778B4
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

			// Token: 0x060013A5 RID: 5029 RVA: 0x00178900 File Offset: 0x00177900
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

			// Token: 0x040029EB RID: 10731
			public readonly string Name;

			// Token: 0x040029EC RID: 10732
			public readonly string Value;
		}
	}
}
