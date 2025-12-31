using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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

namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002E2 RID: 738
	public partial class dfrmFingerManagement : frmN3000
	{
		// Token: 0x060014E5 RID: 5349 RVA: 0x0019D088 File Offset: 0x0019C088
		public dfrmFingerManagement()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			wgAppConfig.custDataGridview(ref this.dgvRunInfo);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0019D180 File Offset: 0x0019C180
		private void _fillDeviceGrid()
		{
			try
			{
				string text = " SELECT ";
				text += " t_b_Controller_FingerPrint.f_ControllerID , f_FingerPrintName , 0 as f_Selected , f_ControllerSN , f_IP , f_Port , f_ReaderName , f_Notes   from t_b_Controller_FingerPrint LEFT JOIN t_b_Reader ON t_b_Reader.f_ReaderID = t_b_Controller_FingerPrint.f_ReaderID  ORDER BY f_FingerPrintName  ";
				DataTable dataTable = new DataTable();
				this.dv = new DataView(dataTable);
				this.dvSelected = new DataView(dataTable);
				DataTable dataTable2 = new DataTable();
				new DataView(dataTable2);
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
						goto IL_00E4;
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
				try
				{
					IL_00E4:
					dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[0] };
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
					this.dgvDoors.Columns[i].DataPropertyName = dataTable.Columns[i].ColumnName;
					this.dgvSelectedDoors.Columns[i].DataPropertyName = dataTable.Columns[i].ColumnName;
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgDebugWrite(ex2.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0019D448 File Offset: 0x0019C448
		private void addLogEvent(string desc, string info)
		{
			this.addLogEvent(desc, info, 100);
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0019D454 File Offset: 0x0019C454
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
			this.displayNewestLog();
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0019D60C File Offset: 0x0019C60C
		private void arrPingInfoReset()
		{
			this.arrIPPingOK.Clear();
			this.arrIPPingFailed.Clear();
			this.arrIPARPOK.Clear();
			this.arrIPARPFailed.Clear();
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0019D63C File Offset: 0x0019C63C
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

		// Token: 0x060014EB RID: 5355 RVA: 0x0019D680 File Offset: 0x0019C680
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
			this.dgvUsers_MouseClick(null, null);
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0019D718 File Offset: 0x0019C718
		private void bkCheck_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			e.Result = this.checkFingerExist();
			try
			{
				if (!this.control4uploadPrivilege.bStopFingerprintComm)
				{
					base.Invoke(new dfrmFingerManagement.updateButtonDisplayDlg(this.updateButtonDisplay));
				}
			}
			catch
			{
			}
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0019D798 File Offset: 0x0019C798
		private void bkUploadAndGetRecords_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			if (this.uploadconsumerID > 0)
			{
				e.Result = this.uploadOneUserFingerprints();
			}
			else
			{
				e.Result = this.uploadAllFingerprint();
			}
			try
			{
				if (!this.control4uploadPrivilege.bStopFingerprintComm)
				{
					base.Invoke(new dfrmFingerManagement.updateButtonDisplayDlg(this.updateButtonDisplay));
				}
			}
			catch
			{
			}
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0019D834 File Offset: 0x0019C834
		private void bkUploadConfigure_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			e.Result = this.uploadConfigure();
			try
			{
				if (!this.control4uploadPrivilege.bStopFingerprintComm)
				{
					base.Invoke(new dfrmFingerManagement.updateButtonDisplayDlg(this.updateButtonDisplay));
				}
			}
			catch
			{
			}
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0019D8B4 File Offset: 0x0019C8B4
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				table.Rows[i]["f_Selected"] = 1;
			}
			this.updateButtonDisplay();
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0019D910 File Offset: 0x0019C910
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
			DataTable table = ((DataView)this.dgvUsers.DataSource).Table;
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
			table.BeginLoadData();
			this.dv = new DataView(table);
			this.dv.RowFilter = this.strGroupFilter;
			for (int i = 0; i < this.dv.Count; i++)
			{
				this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
			}
			table.EndLoadData();
			this.dv1 = new DataView(table);
			this.dv1.RowFilter = rowFilter;
			this.dv2 = new DataView(table);
			this.dv2.RowFilter = rowFilter2;
			this.dgvUsers.DataSource = this.dv1;
			this.dgvSelectedUsers.DataSource = this.dv2;
			wgTools.WriteLine("btnAddAllUsers_Click End");
			this.updateButtonDisplay();
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0019DB14 File Offset: 0x0019CB14
		private void btnAddCardNO_Click(object sender, EventArgs e)
		{
			string text = "";
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as Button).Text;
				dfrmInputNewName.label1.Text = this.label2.Text;
				if (wgAppConfig.IsActivateCard19)
				{
					dfrmInputNewName.txtNewName.MaxLength = 19;
				}
				else
				{
					dfrmInputNewName.txtNewName.MaxLength = 10;
				}
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				text = dfrmInputNewName.strNewName;
			}
			long num = 0L;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			long.TryParse(text.Trim(), out num);
			if (num > 0L)
			{
				new icConsumer().registerLostCard(this.consumerID, num);
				DataTable table = ((DataView)this.dgvUsers.DataSource).Table;
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if ((int)table.Rows[i]["f_ConsumerID"] == this.consumerID)
					{
						table.Rows[i]["f_CardNO"] = num;
						IL_0121:
						table.AcceptChanges();
						this.consumerID = 0;
						this.dgvUsers_MouseClick(null, null);
						return;
					}
				}
				goto IL_0121;
			}
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0019DC68 File Offset: 0x0019CC68
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvDoors);
			this.updateButtonDisplay();
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0019DC7B File Offset: 0x0019CC7B
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
			this.updateButtonDisplay();
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0019DC94 File Offset: 0x0019CC94
		private void btnCheck_Click(object sender, EventArgs e)
		{
			if (!this.bkCheck.IsBusy)
			{
				this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount;
				this.progressBar1.Value = 0;
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount;
				this.CommButtonDisable();
				this.bkCheck.RunWorkerAsync();
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0019DD10 File Offset: 0x0019CD10
		private void btnConfigure_Click(object sender, EventArgs e)
		{
			if (!this.bkUploadConfigure.IsBusy)
			{
				using (dfrmFingerSupercard dfrmFingerSupercard = new dfrmFingerSupercard())
				{
					if (wgAppConfig.IsActivateCard19)
					{
						dfrmFingerSupercard.txtSuperCard1.Mask = "9999999999999999999";
						dfrmFingerSupercard.txtSuperCard2.Mask = "9999999999999999999";
					}
					else
					{
						dfrmFingerSupercard.txtSuperCard1.Mask = "9999999999";
						dfrmFingerSupercard.txtSuperCard2.Mask = "9999999999";
					}
					if (dfrmFingerSupercard.ShowDialog(this) != DialogResult.OK)
					{
						return;
					}
					this.superCard1_IPWEB = dfrmFingerSupercard.txtSuperCard1.Text;
					this.superCard2_IPWEB = dfrmFingerSupercard.txtSuperCard2.Text;
				}
				this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount;
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(sender as Button).Text,
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString(),
					",superCard1_IPWEB = ",
					this.superCard1_IPWEB,
					",superCard2_IPWEB = ",
					this.superCard2_IPWEB
				}));
				this.addLogEvent(string.Format("{0}", (sender as Button).Text), CommonStr.strStart);
				this.logOperate(sender);
				this.progressBar1.Value = 0;
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount;
				this.CommButtonDisable();
				this.bkUploadConfigure.RunWorkerAsync();
			}
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0019DEC4 File Offset: 0x0019CEC4
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			DataTable table = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				table.Rows[i]["f_Selected"] = 0;
			}
			this.updateButtonDisplay();
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0019DF20 File Offset: 0x0019CF20
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

		// Token: 0x060014F8 RID: 5368 RVA: 0x0019E007 File Offset: 0x0019D007
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.updateButtonDisplay();
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0019E01A File Offset: 0x0019D01A
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
			this.updateButtonDisplay();
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0019E034 File Offset: 0x0019D034
		private void btnDeviceManage_Click(object sender, EventArgs e)
		{
			using (dfrmFingerPrintDeviceManagement dfrmFingerPrintDeviceManagement = new dfrmFingerPrintDeviceManagement())
			{
				dfrmFingerPrintDeviceManagement.ShowDialog(this);
			}
			this._fillDeviceGrid();
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0019E074 File Offset: 0x0019D074
		private void btnDownloadAllUsers_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0019E076 File Offset: 0x0019D076
		private void btnDownloadSelectedUsers_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0019E078 File Offset: 0x0019D078
		private void btnEditDescription_Click(object sender, EventArgs e)
		{
			string text = "";
			string text2 = "";
			if (this.dataGridView1.Rows.Count > 0)
			{
				try
				{
					int rowIndex = this.dataGridView1.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num;
			if (this.dataGridView1.SelectedRows.Count <= 0)
			{
				if (this.dataGridView1.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dataGridView1.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dataGridView1.SelectedRows[0].Index;
			}
			text2 = wgTools.SetObjToStr(this.dataGridView1.Rows[num].Cells[2].Value);
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as Button).Text;
				dfrmInputNewName.strNewName = text2;
				dfrmInputNewName.label1.Text = this.label5.Text;
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				text = dfrmInputNewName.strNewName;
			}
			if (!string.IsNullOrEmpty(text) && !text2.Equals(text) && wgAppConfig.runUpdateSql(string.Format(" UPDATE t_b_Consumer_Fingerprint SET f_Description={0} WHERE [f_FingerNO]= " + this.dataGridView1.Rows[num].Cells[0].Value.ToString(), wgTools.PrepareStrNUnicode(text))) > 0)
			{
				this.dataGridView1.Rows[num].Cells[2].Value = text;
			}
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0019E224 File Offset: 0x0019D224
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0019E22C File Offset: 0x0019D22C
		private void btnFingerAdd_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.txtf_CardNO.Text))
				{
					XMessageBox.Show(this, CommonStr.strSetCardNO, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					using (dfrmFingerEnroll dfrmFingerEnroll = new dfrmFingerEnroll())
					{
						dfrmFingerEnroll.consumerID = this.consumerID;
						dfrmFingerEnroll.consumerCardNO = long.Parse(this.txtf_CardNO.Text);
						if (dfrmFingerEnroll.ShowDialog() == DialogResult.OK)
						{
							this.consumerID = 0;
							this.dgvUsers_MouseClick(null, null);
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0019E2DC File Offset: 0x0019D2DC
		private void btnFingerClear_Click(object sender, EventArgs e)
		{
			string text = string.Format("{0}  \"{1}\"", (sender as Button).Text, wgTools.SetObjToStr(this.txtf_ConsumerName.Text));
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				int count = this.dataGridView1.Rows.Count;
				new icConsumer().fingerprintClear(this.consumerID);
				this.consumerID = 0;
				this.dgvUsers_MouseClick(null, null);
				wgAppConfig.wgLog(string.Format("{0}-{1}-{2}", this.btnFingerClear.Text, this.txtf_ConsumerName.Text, count));
				byte[] array = new byte[1152];
				byte[] array2 = new byte[1152];
				for (int i = 0; i < 1152; i++)
				{
					array[i] = 0;
				}
				array[0] = 241;
				array[2] = 16;
				long num = long.Parse(this.txtf_CardNO.Text);
				array[5] = (byte)(num & 255L);
				array[6] = (byte)((num >> 8) & 255L);
				array[7] = (byte)((num >> 16) & 255L);
				array[8] = (byte)((num >> 24) & 255L);
				for (int j = 128; j < 640; j++)
				{
					array[j] = array2[j];
				}
				for (int k = 0; k < 1152; k++)
				{
					array2[k] = 0;
				}
				ArrayList arrayList = new ArrayList();
				DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
				for (int l = 0; l < table.Rows.Count; l++)
				{
					arrayList.Add(table.Rows[l]["f_FingerprintName"]);
				}
				if (arrayList.Count != 0)
				{
					lock (frmADCT3000.qfingerEnrollInfo.SyncRoot)
					{
						frmADCT3000.qfingerEnrollInfo.Enqueue(array);
						frmADCT3000.qfingerEnrollarrController.Enqueue(arrayList);
					}
				}
			}
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0019E4FC File Offset: 0x0019D4FC
		private void btnFingerDel_Click(object sender, EventArgs e)
		{
			int num = 0;
			try
			{
				if (this.dataGridView1.Rows.Count > 0)
				{
					try
					{
						num = this.dataGridView1.CurrentCell.RowIndex;
					}
					catch
					{
					}
				}
				int num2;
				if (this.dataGridView1.SelectedRows.Count <= 0)
				{
					if (this.dataGridView1.SelectedCells.Count <= 0)
					{
						return;
					}
					num2 = this.dataGridView1.SelectedCells[0].RowIndex;
				}
				else
				{
					num2 = this.dataGridView1.SelectedRows[0].Index;
				}
				string text = string.Format("{0}  \"{1}:{2}:{3}\"", new object[]
				{
					(sender as Button).Text,
					wgTools.SetObjToStr(this.dataGridView1.Rows[num2].Cells[1].Value.ToString()),
					wgTools.SetObjToStr(this.dataGridView1.Rows[num2].Cells[2].Value.ToString()),
					wgTools.SetObjToStr(this.dataGridView1.Rows[num2].Cells[3].Value.ToString())
				});
				if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					long num3 = long.Parse(this.dataGridView1.Rows[num2].Cells[0].Value.ToString());
					wgAppConfig.runUpdateSql(" DELETE FROM t_b_Consumer_Fingerprint WHERE [f_FingerNO]= " + this.dataGridView1.Rows[num2].Cells[0].Value.ToString());
					this.consumerID = 0;
					this.dgvUsers_MouseClick(null, null);
					if (this.dataGridView1.RowCount > 0)
					{
						if (this.dataGridView1.RowCount > num)
						{
							this.dataGridView1.CurrentCell = this.dataGridView1[2, num];
						}
						else
						{
							this.dataGridView1.CurrentCell = this.dataGridView1[2, this.dataGridView1.RowCount - 1];
						}
					}
					wgAppConfig.wgLog(string.Format("{0}-{1}-{2}", this.btnFingerDel.Text, this.txtf_ConsumerName.Text, num3));
					byte[] array = new byte[1152];
					byte[] array2 = new byte[1152];
					for (int i = 0; i < 1152; i++)
					{
						array[i] = 0;
					}
					array[0] = 241;
					array[2] = 17;
					if (num3 > 0L)
					{
						num3 -= 1L;
					}
					array[3] = (byte)(num3 & 255L);
					array[4] = (byte)((num3 >> 8) & 255L);
					for (int j = 128; j < 640; j++)
					{
						array[j] = array2[j];
					}
					for (int k = 0; k < 1152; k++)
					{
						array2[k] = 0;
					}
					ArrayList arrayList = new ArrayList();
					DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
					for (int l = 0; l < table.Rows.Count; l++)
					{
						arrayList.Add(table.Rows[l]["f_FingerprintName"]);
					}
					if (arrayList.Count != 0)
					{
						lock (frmADCT3000.qfingerEnrollInfo.SyncRoot)
						{
							frmADCT3000.qfingerEnrollInfo.Enqueue(array);
							frmADCT3000.qfingerEnrollarrController.Enqueue(arrayList);
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0019E8F4 File Offset: 0x0019D8F4
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

		// Token: 0x06001503 RID: 5379 RVA: 0x0019EBF8 File Offset: 0x0019DBF8
		private void btnUploadAllUsers_Click(object sender, EventArgs e)
		{
			if (!this.bkUploadAndGetRecords.IsBusy && XMessageBox.Show(this, string.Concat(new string[]
			{
				string.Format(CommonStr.strAreYouSure + " {0}?\r\n\r\n", (sender as Button).Text),
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
				this.addLogEvent(string.Format("{0}", (sender as Button).Text), CommonStr.strStart);
				this.logOperate(sender);
				this.progressBar1.Value = 0;
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount;
				this.CommButtonDisable();
				this.uploadconsumerID = 0;
				this.bkUploadAndGetRecords.RunWorkerAsync();
			}
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0019ED4C File Offset: 0x0019DD4C
		private void btnUploadOneuserFingerprints_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(string.Concat(new string[]
			{
				CommonStr.strAreYouSure,
				" {0}?\r\n\r\n",
				this.txtf_ConsumerName.Text,
				"\r\n\r\n",
				CommonStr.strDevicesNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString()
			}), (sender as Button).Text.Replace("\r\n", "")), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				this.uploadconsumerID = this.consumerID;
			}
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x0019EDEE File Offset: 0x0019DDEE
		private void btnUploadSelectedUsers_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x0019EDF0 File Offset: 0x0019DDF0
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

		// Token: 0x06001507 RID: 5383 RVA: 0x0019EE20 File Offset: 0x0019DE20
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
				this.consumerID = 0;
				this.dgvUsers_MouseClick(null, null);
			}
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x0019F080 File Offset: 0x0019E080
		private int checkFingerExist()
		{
			try
			{
				Button button = this.btnCheck;
				DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
				dataView.RowFilter = "f_Selected > 0";
				if (this.control4uploadPrivilege != null)
				{
					this.control4uploadPrivilege = null;
				}
				this.control4uploadPrivilege = new icController();
				this.control4uploadPrivilege.bStopFingerprintComm = false;
				int num = 0;
				foreach (object obj in dataView)
				{
					DataRowView dataRowView = (DataRowView)obj;
					if (this.control4uploadPrivilege.bStopFingerprintComm)
					{
						break;
					}
					if (wgMjController.IsFingerController((int)dataRowView["f_ControllerSN"]))
					{
						try
						{
							wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]);
							this.control4uploadPrivilege.ControllerSN = (int)dataRowView["f_ControllerSN"];
							this.control4uploadPrivilege.IP = wgTools.SetObjToStr(dataRowView["f_IP"]);
							this.control4uploadPrivilege.PORT = (int)dataRowView["f_PORT"];
							int num2 = this.control4uploadPrivilege.GetControllerRunInformationIP(-1);
							if (num2 <= 0)
							{
								if (this.control4uploadPrivilege.bStopFingerprintComm)
								{
									break;
								}
								wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " Upload Configure Failed num =" + num2.ToString(), new object[0]);
								base.Invoke(new dfrmFingerManagement.addLogEvent3BK(this.addLogEvent), new object[]
								{
									string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
									string.Format("{0}--{1}[{2}]:{3}:{4}", new object[]
									{
										CommonStr.strCommFail,
										wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]),
										wgTools.SetObjToStr(dataRowView["f_ControllerSN"]),
										wgTools.SetObjToStr(dataRowView["f_IP"]),
										wgTools.SetObjToStr(dataRowView["f_Port"])
									}),
									101
								});
							}
							else
							{
								num2 = this.control4uploadPrivilege.FingerGetCountIP();
								if (num2 >= 0)
								{
									string text = "";
									string text2 = "";
									try
									{
										if (!string.IsNullOrEmpty(this.control4uploadPrivilege.GetProductInfoIP(ref text, ref text2, -1)))
										{
											text2 = string.Format("{0}", text.Substring(text.IndexOf("DATE=") + 5, 10));
										}
									}
									catch (Exception)
									{
									}
									string text3 = string.Format("[{0}:{1} ({2})]", CommonStr.strFirmware, this.control4uploadPrivilege.runinfo.driverVersion, text2) + " " + string.Format("[{0}:{1}]", CommonStr.strFingerprintCount, this.control4uploadPrivilege.fingerTotalValid);
									wgAppConfig.wgLog(string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text) + " " + string.Format("{0} {1}. {2}", button.Text, CommonStr.strSuccessfully, text3));
									base.Invoke(new dfrmFingerManagement.addLogEventBK(this.addLogEvent), new object[]
									{
										string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
										string.Format("{0} {1}. {2}", button.Text, CommonStr.strSuccessfully, text3)
									});
								}
								else
								{
									if (this.control4uploadPrivilege.bStopFingerprintComm)
									{
										break;
									}
									wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " Upload Configure Failed num =" + num2.ToString(), new object[0]);
									base.Invoke(new dfrmFingerManagement.addLogEvent3BK(this.addLogEvent), new object[]
									{
										string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
										string.Format("{0}--{1}[{2}]:{3}:{4}", new object[]
										{
											CommonStr.strCommFail,
											wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]),
											wgTools.SetObjToStr(dataRowView["f_ControllerSN"]),
											wgTools.SetObjToStr(dataRowView["f_IP"]),
											wgTools.SetObjToStr(dataRowView["f_Port"])
										}),
										101
									});
								}
							}
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
					}
					num++;
					base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { num });
				}
				if (this.control4uploadPrivilege.bStopFingerprintComm)
				{
					return 1;
				}
				base.Invoke(new dfrmFingerManagement.displayNewestLogBK(this.displayNewestLog));
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					button.Text.Replace("\r\n", ""),
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString(),
					",",
					CommonStr.strOperationComplete
				}), EventLogEntryType.Information, null);
				Cursor.Current = Cursors.Default;
				base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { -1 });
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
			if (!this.control4uploadPrivilege.bStopFingerprintComm)
			{
				try
				{
					base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { -1 });
					base.Invoke(new dfrmFingerManagement.displayNewestLogBK(this.displayNewestLog));
				}
				catch (Exception ex3)
				{
					wgAppConfig.wgLog(ex3.ToString());
				}
			}
			return 1;
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x0019F770 File Offset: 0x0019E770
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

		// Token: 0x0600150A RID: 5386 RVA: 0x0019F79C File Offset: 0x0019E79C
		private void CommButtonDisable()
		{
			this.btnCheck.Enabled = false;
			this.btnUploadAllUsers.Enabled = false;
			this.btnConfigure.Enabled = false;
			this.btnFingerAdd.Enabled = false;
			this.btnFingerClear.Enabled = false;
			this.btnFingerDel.Enabled = false;
			this.btnEditDescription.Enabled = false;
			this.btnAddCardNO.Enabled = false;
			this.btnDeviceManage.Enabled = false;
			if (!string.IsNullOrEmpty(this.txtFingerDescription.Text))
			{
				this.btnEditDescription.Enabled = true;
			}
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x0019F833 File Offset: 0x0019E833
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0019F835 File Offset: 0x0019E835
		private void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			this.btnEditDescription.PerformClick();
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0019F842 File Offset: 0x0019E842
		private void dfrmFaceManage_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0019F858 File Offset: 0x0019E858
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
				if (e.Control)
				{
					int keyValue = e.KeyValue;
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0019F8E4 File Offset: 0x0019E8E4
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
				this.updateTotalFinger();
				string systemParamNotes = wgAppConfig.getSystemParamNotes(189);
				try
				{
					if (!string.IsNullOrEmpty(systemParamNotes) && systemParamNotes != ",")
					{
						this.btnConfigure.Visible = true;
					}
				}
				catch
				{
				}
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
			if (wgTools.gbHideCardNO)
			{
				this.txtf_CardNO.PasswordChar = '#';
			}
			this.updateButtonDisplay();
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x0019FD00 File Offset: 0x0019ED00
		private void dfrmFingerManagement_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.bkCheck.IsBusy || this.bkUploadAndGetRecords.IsBusy || this.bkUploadConfigure.IsBusy)
				{
					wgAppConfig.wgLog(this.Text + " " + CommonStr.strForceClose + "!!!");
				}
				if (this.dfrmWait1 != null)
				{
					this.dfrmWait1.Close();
				}
				if (this.control4uploadPrivilege != null)
				{
					this.control4uploadPrivilege.bStopFingerprintComm = true;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0019FD90 File Offset: 0x0019ED90
		private void dgvDoors_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0019FD92 File Offset: 0x0019ED92
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0019FDA0 File Offset: 0x0019EDA0
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

		// Token: 0x06001514 RID: 5396 RVA: 0x0019FE50 File Offset: 0x0019EE50
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

		// Token: 0x06001515 RID: 5397 RVA: 0x0019FEBC File Offset: 0x0019EEBC
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

		// Token: 0x06001516 RID: 5398 RVA: 0x0019FEEC File Offset: 0x0019EEEC
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0019FEFC File Offset: 0x0019EEFC
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

		// Token: 0x06001518 RID: 5400 RVA: 0x0019FF2C File Offset: 0x0019EF2C
		private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0019FF39 File Offset: 0x0019EF39
		private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0019FF3C File Offset: 0x0019EF3C
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

		// Token: 0x0600151B RID: 5403 RVA: 0x0019FF6C File Offset: 0x0019EF6C
		private void dgvUsers_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				int num = this.consumerID;
				this.consumerID = 0;
				DataGridView dataGridView = this.dgvUsers;
				int num2;
				if (dataGridView.SelectedRows.Count <= 0)
				{
					if (dataGridView.SelectedCells.Count <= 0)
					{
						return;
					}
					num2 = dataGridView.SelectedCells[0].RowIndex;
				}
				else
				{
					num2 = dataGridView.SelectedRows[0].Index;
				}
				this.consumerID = (int)dataGridView.Rows[num2].Cells[0].Value;
				if (num != this.consumerID)
				{
					this.updateTotalFinger();
					this.txtf_ConsumerName.Text = (string)dataGridView.Rows[num2].Cells[2].Value;
					this.txtf_CardNO.Text = wgTools.SetObjToStr(dataGridView.Rows[num2].Cells[3].Value);
					if (string.IsNullOrEmpty(this.txtf_CardNO.Text))
					{
						this.btnAddCardNO.Enabled = true;
					}
					else
					{
						this.btnAddCardNO.Enabled = false;
					}
					try
					{
						string text = string.Format("SELECT t_b_Consumer_Fingerprint.f_FingerNO, 0 as ID ,t_b_Consumer_Fingerprint.f_Description,t_b_Consumer_Fingerprint.f_RegisterTime,t_b_Consumer_Fingerprint.f_FingerID  From t_b_Consumer_Fingerprint WHERE  t_b_Consumer_Fingerprint.f_ConsumerID = {0}  ORDER BY f_FingerID ASC", this.consumerID);
						DataSet dataSet = new DataSet();
						DataTable dataTable = new DataTable();
						using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
						{
							using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
							{
								using (DataAdapter dataAdapter = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand) : new SqlDataAdapter((SqlCommand)dbCommand)))
								{
									dataAdapter.Fill(dataSet);
								}
							}
						}
						dataTable = dataSet.Tables[0];
						for (int i = 0; i < dataTable.Rows.Count; i++)
						{
							dataTable.Rows[i]["ID"] = i + 1;
							if (string.IsNullOrEmpty(wgTools.SetObjToStr(dataTable.Rows[i]["f_Description"])))
							{
								dataTable.Rows[i]["f_Description"] = "ZW-" + wgTools.SetObjToStr(dataTable.Rows[i]["f_FingerID"]).PadLeft(4, '0');
							}
						}
						dataTable.AcceptChanges();
						this.dataGridView1.AutoGenerateColumns = false;
						DataView dataView = new DataView(dataTable);
						this.dataGridView1.DataSource = dataView;
						dataView.Sort = "f_Description ASC";
						int num3 = 0;
						while (num3 < dataTable.Columns.Count && num3 < this.dataGridView1.ColumnCount)
						{
							this.dataGridView1.Columns[num3].DataPropertyName = dataTable.Columns[num3].ColumnName;
							num3++;
						}
						wgAppConfig.setDisplayFormatDate(this.dataGridView1, "f_RegisterTime", wgTools.DisplayFormat_DateYMDHMSWeek);
						this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
						if (dataTable.Rows.Count > 0)
						{
							this.txtFingerDescription.Text = wgTools.SetObjToStr(dataTable.Rows[0][2]);
							this.btnEditDescription.Enabled = true;
						}
						else
						{
							this.txtFingerDescription.Text = "";
							this.btnEditDescription.Enabled = false;
						}
						DataTable table = ((DataView)this.dgvUsers.DataSource).Table;
						for (int j = 0; j < table.Rows.Count; j++)
						{
							if ((int)table.Rows[j]["f_ConsumerID"] == this.consumerID)
							{
								table.Rows[j]["f_Other"] = ((dataTable.Rows.Count > 0) ? 1 : 0);
								break;
							}
						}
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
					}
					this.updateButtonDisplay();
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x001A0454 File Offset: 0x0019F454
		private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x001A0456 File Offset: 0x0019F456
		private void dgvUsers_SelectionChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x001A0458 File Offset: 0x0019F458
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

		// Token: 0x0600151F RID: 5407 RVA: 0x001A04EC File Offset: 0x0019F4EC
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

		// Token: 0x06001520 RID: 5408 RVA: 0x001A053F File Offset: 0x0019F53F
		private long getWorkNOOnDevice(long cardno, long diff)
		{
			if (cardno > diff)
			{
				return cardno - diff;
			}
			return cardno;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x001A054A File Offset: 0x0019F54A
		private void groupBox1_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x001A054C File Offset: 0x0019F54C
		private bool IsIPCanConnect(string ip)
		{
			return (this.arrIPPingOK.Count > 0 && this.arrIPPingOK.IndexOf(ip) >= 0) || (this.arrIPARPOK.Count > 0 && this.arrIPARPOK.IndexOf(ip) >= 0);
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x001A059A File Offset: 0x0019F59A
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x001A05BC File Offset: 0x0019F5BC
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

		// Token: 0x06001525 RID: 5413 RVA: 0x001A0718 File Offset: 0x0019F718
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
				text2 = text2 + ((DataView)this.dgvSelectedDoors.DataSource)[i]["f_FingerPrintName"] + ",";
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
				this.txtTotalFingerprint.Text,
				this.dgvSelectedDoors.RowCount.ToString(),
				this.txtTotalFingerprint.Text,
				text2
			}), EventLogEntryType.Information, null);
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x001A0969 File Offset: 0x0019F969
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

		// Token: 0x06001527 RID: 5415 RVA: 0x001A09A0 File Offset: 0x0019F9A0
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

		// Token: 0x06001528 RID: 5416 RVA: 0x001A0A58 File Offset: 0x0019FA58
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

		// Token: 0x06001529 RID: 5417 RVA: 0x001A0B4C File Offset: 0x0019FB4C
		private void tcpClose()
		{
			if (this.tcp != null)
			{
				this.tcp.Close();
				this.tcp = null;
			}
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x001A0B68 File Offset: 0x0019FB68
		public int ThirdPartyDeviceFaceCommIP_TCP(string strIP, int PORT, string command, ref string strRecvInfo, ref byte[] recvInfo)
		{
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
					byte[] array2 = new byte[this.tcpBuffSize];
					int num = 0;
					while (dateTime > DateTime.Now)
					{
						if (stream.CanRead && stream.DataAvailable)
						{
							int num2 = stream.Read(array2, num, array2.Length - num);
							if (Array.IndexOf<byte>(array2, 41, num, num2) >= 0)
							{
								num = Array.IndexOf<byte>(array2, 41, num, num2) + 1;
								break;
							}
							num += num2;
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
						if (strRecvInfo.StartsWith("Wait"))
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
						if (!strRecvInfo.StartsWith("Return"))
						{
							wgTools.WgDebugWrite("FaceId_ErrorCode.NotSupportedException", new object[0]);
							return -4;
						}
						string keyValue = dfrmFingerManagement.FaceId_Item.GetKeyValue(strRecvInfo, "result");
						if (!keyValue.Equals("success"))
						{
							if (keyValue.Equals("busy"))
							{
								wgTools.WgDebugWrite("FaceId_ErrorCode.Busy", new object[0]);
								return -2;
							}
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

		// Token: 0x0600152B RID: 5419 RVA: 0x001A0E10 File Offset: 0x0019FE10
		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				if (this.bStarting)
				{
					if (this.dgvUsers.DataSource == null)
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
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x001A0E94 File Offset: 0x0019FE94
		private void txtInfoUpdateEntry(object info)
		{
			if (wgTools.SetObjToStr(info) == "-1")
			{
				this.progressBar1.Value = this.progressBar1.Maximum;
				this.dfrmWait1.Hide();
			}
			if (this.progressBar1.Value < this.progressBar1.Maximum)
			{
				this.progressBar1.Value++;
				Application.DoEvents();
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x001A0F04 File Offset: 0x0019FF04
		private void updateButtonDisplay()
		{
			try
			{
				this.btnDeviceManage.Enabled = true;
				this.btnUploadSelectedUsers.Enabled = false;
				this.btnDownloadSelectedUsers.Enabled = false;
				if (this.dgvSelectedDoors.RowCount > 0)
				{
					this.btnDownloadAllUsers.Enabled = true;
					this.btnUploadAllUsers.Enabled = true;
					this.btnConfigure.Enabled = true;
					this.btnCheck.Enabled = true;
					if (this.dgvSelectedUsers.RowCount > 0)
					{
						this.btnUploadSelectedUsers.Enabled = true;
						this.btnDownloadSelectedUsers.Enabled = true;
					}
				}
				else
				{
					this.btnDownloadAllUsers.Enabled = false;
					this.btnUploadAllUsers.Enabled = false;
					this.btnConfigure.Enabled = false;
					this.btnCheck.Enabled = false;
				}
				if (string.IsNullOrEmpty(this.txtf_ConsumerName.Text))
				{
					this.btnFingerAdd.Enabled = false;
					this.btnFingerClear.Enabled = false;
					this.btnFingerDel.Enabled = false;
					this.btnEditDescription.Enabled = false;
					this.btnAddCardNO.Enabled = false;
				}
				else
				{
					this.btnFingerAdd.Enabled = true;
					this.btnFingerClear.Enabled = true;
					this.btnFingerDel.Enabled = true;
					if (this.dataGridView1.Rows.Count == 0)
					{
						this.btnFingerClear.Enabled = false;
						this.btnFingerDel.Enabled = false;
					}
					if (this.dataGridView1.Rows.Count >= 10)
					{
						this.btnFingerAdd.Enabled = false;
					}
					if (this.txtTotalFingerprint.Text.Equals("1000"))
					{
						this.btnFingerAdd.Enabled = false;
					}
					if (wgAppConfig.getValBySql("SELECT count(t_b_Consumer_Fingerprint.f_fingerNO) from t_b_Consumer_Fingerprint,t_b_consumer where t_b_Consumer_Fingerprint.f_consumerid = t_b_consumer.f_consumerid") >= 1000)
					{
						this.btnFingerAdd.Enabled = false;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x001A10F8 File Offset: 0x001A00F8
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

		// Token: 0x0600152F RID: 5423 RVA: 0x001A1280 File Offset: 0x001A0280
		private void updateTotalFinger()
		{
			try
			{
				this.txtTotalFingerprint.Text = wgAppConfig.getValBySql("SELECT count(t_b_Consumer_Fingerprint.f_fingerNO) from t_b_Consumer_Fingerprint,t_b_consumer where t_b_Consumer_Fingerprint.f_consumerid = t_b_consumer.f_consumerid").ToString();
				if (wgAppConfig.getValBySql("SELECT count(t_b_Consumer_Fingerprint.f_fingerNO) from t_b_Consumer_Fingerprint") > int.Parse(this.txtTotalFingerprint.Text))
				{
					wgAppConfig.runSql(string.Format("DELETE  From t_b_Consumer_Fingerprint WHERE t_b_Consumer_Fingerprint.f_ConsumerID IS NULL", new object[0]));
					ArrayList arrayList = new ArrayList();
					string text = "SELECT t_b_Consumer_Fingerprint.f_consumerid  from t_b_Consumer_Fingerprint WHERE t_b_Consumer_Fingerprint.f_consumerid  not in (select  t_b_consumer.f_consumerid from t_b_consumer) ";
					using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
					{
						using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
						{
							dbConnection.Open();
							dbCommand.CommandText = text;
							DbDataReader dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								arrayList.Add(dbDataReader["f_ConsumerID"]);
							}
							dbDataReader.Close();
						}
					}
					icConsumer icConsumer = new icConsumer();
					for (int i = 0; i < arrayList.Count; i++)
					{
						icConsumer.fingerprintClear((int)arrayList[i]);
					}
					this.txtTotalFingerprint.Text = wgAppConfig.getValBySql("SELECT count(t_b_Consumer_Fingerprint.f_fingerNO) from t_b_Consumer_Fingerprint,t_b_consumer where t_b_Consumer_Fingerprint.f_consumerid = t_b_consumer.f_consumerid").ToString();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x001A142C File Offset: 0x001A042C
		private void updateUserRegister()
		{
			try
			{
				string text = string.Format("SELECT DISTINCT f_ConsumerID  From t_b_Consumer_Fingerprint  ORDER BY f_ConsumerID ASC", new object[0]);
				DataSet dataSet = new DataSet();
				DataTable dataTable = new DataTable();
				using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
				{
					using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
					{
						using (DataAdapter dataAdapter = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand) : new SqlDataAdapter((SqlCommand)dbCommand)))
						{
							dataAdapter.Fill(dataSet);
						}
					}
				}
				dataTable = dataSet.Tables[0];
				DataView dataView = new DataView(dataTable);
				DataTable table = ((DataView)this.dgvUsers.DataSource).Table;
				for (int i = 0; i < table.Rows.Count; i++)
				{
					dataView.RowFilter = "f_ConsumerID = " + wgTools.PrepareStr(table.Rows[i]["f_ConsumerID"]);
					if (dataView.Count > 0)
					{
						table.Rows[i]["f_Other"] = 1;
					}
					else
					{
						table.Rows[i]["f_Other"] = 0;
					}
				}
				table.AcceptChanges();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x001A1630 File Offset: 0x001A0630
		private int uploadAllFingerprint()
		{
			try
			{
				Button button = this.btnUploadAllUsers;
				DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
				dataView.RowFilter = "f_Selected > 0";
				if (this.control4uploadPrivilege != null)
				{
					if (this.control4uploadPrivilege.bStopFingerprintComm)
					{
						return 1;
					}
					this.control4uploadPrivilege = null;
				}
				this.control4uploadPrivilege = new icController();
				this.control4uploadPrivilege.bStopFingerprintComm = false;
				byte[] array = new byte[4096];
				byte[] array2 = new byte[524288];
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = byte.MaxValue;
				}
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = byte.MaxValue;
				}
				string text = "";
				string text2 = "";
				int num3 = 0;
				try
				{
					using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
					{
						using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
						{
							dbConnection.Open();
							text = "SELECT t_b_Consumer_Fingerprint.*, t_b_Consumer.f_CardNO  From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer.f_CardNO>0 AND t_b_Consumer_Fingerprint.f_ConsumerID = t_b_Consumer.f_ConsumerID  ORDER BY f_FingerNO ASC";
							dbCommand.CommandText = text;
							DbDataReader dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								num3 = (int)dbDataReader["f_FingerNO"];
								if (num3 <= 0 || num3 >= 1024)
								{
									break;
								}
								num2++;
								Array.Copy(BitConverter.GetBytes(long.Parse(dbDataReader["f_CardNO"].ToString())), 0, array, (num3 - 1) * 4, 4);
								text2 = dbDataReader["f_FingerInfo"] as string;
								if (!string.IsNullOrEmpty(text2))
								{
									text2 = text2.Replace("\r\n", "");
									for (int k = 0; k < text2.Length; k += 2)
									{
										try
										{
											array2[(num3 - 1) * 512 + k / 2] = byte.Parse(text2.Substring(k, 2), NumberStyles.AllowHexSpecifier);
										}
										catch (Exception ex)
										{
											wgAppConfig.wgLog(string.Concat(new object[]
											{
												ex.ToString(),
												"\r\nfingerNO=",
												num3,
												",strFinger=",
												text2
											}));
										}
									}
								}
								if (num < num3)
								{
									num = num3;
								}
							}
							dbDataReader.Close();
						}
					}
				}
				catch (Exception ex2)
				{
					wgAppConfig.wgLog(string.Concat(new object[]
					{
						ex2.ToString(),
						"\r\nfingerNO=",
						num3,
						",strFinger=",
						text2
					}));
				}
				int num4 = 0;
				foreach (object obj in dataView)
				{
					DataRowView dataRowView = (DataRowView)obj;
					if (wgMjController.IsFingerController((int)dataRowView["f_ControllerSN"]))
					{
						base.Invoke(new dfrmFingerManagement.addLogEventBK(this.addLogEvent), new object[]
						{
							string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
							CommonStr.strStart
						});
						try
						{
							if (this.control4uploadPrivilege.bStopFingerprintComm)
							{
								break;
							}
							string text3 = wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]);
							this.control4uploadPrivilege.ControllerSN = (int)dataRowView["f_ControllerSN"];
							this.control4uploadPrivilege.IP = wgTools.SetObjToStr(dataRowView["f_IP"]);
							this.control4uploadPrivilege.PORT = (int)dataRowView["f_PORT"];
							int num5 = this.control4uploadPrivilege.UpdateFingerprintListIP(num, array, array2, text3, -1);
							if (num5 <= 0)
							{
								if (this.control4uploadPrivilege.bStopFingerprintComm)
								{
									break;
								}
								wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " UpdateFingerprintListIP Failed num =" + num5.ToString(), new object[0]);
								if (num5 == -1999)
								{
									base.Invoke(new dfrmFingerManagement.addLogEvent3BK(this.addLogEvent), new object[]
									{
										string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
										string.Format("{0}--{1}[{2}]:{3}:{4}", new object[]
										{
											CommonStr.strSupposeUpgradeDriver,
											wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]),
											wgTools.SetObjToStr(dataRowView["f_ControllerSN"]),
											wgTools.SetObjToStr(dataRowView["f_IP"]),
											wgTools.SetObjToStr(dataRowView["f_Port"])
										}),
										5
									});
								}
								else
								{
									base.Invoke(new dfrmFingerManagement.addLogEvent3BK(this.addLogEvent), new object[]
									{
										string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
										string.Format("{0}--{1}[{2}]:{3}:{4}", new object[]
										{
											CommonStr.strCommFail,
											wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]),
											wgTools.SetObjToStr(dataRowView["f_ControllerSN"]),
											wgTools.SetObjToStr(dataRowView["f_IP"]),
											wgTools.SetObjToStr(dataRowView["f_Port"])
										}),
										101
									});
								}
							}
							else
							{
								wgAppConfig.wgLog(string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text) + " " + string.Format("{0} {1}.[Total:{2}/MaxNO:{3}] ", new object[]
								{
									button.Text,
									CommonStr.strSuccessfully,
									num2,
									num
								}));
								base.Invoke(new dfrmFingerManagement.addLogEventBK(this.addLogEvent), new object[]
								{
									string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
									string.Format("{0} {1}.[{2}] ", button.Text, CommonStr.strSuccessfully, num2)
								});
							}
						}
						catch (Exception ex3)
						{
							wgAppConfig.wgLog(ex3.ToString());
						}
					}
					num4++;
					base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { num4 });
				}
				if (this.control4uploadPrivilege.bStopFingerprintComm)
				{
					return 1;
				}
				icConsumerShare.setUpdateLog();
				base.Invoke(new dfrmFingerManagement.displayNewestLogBK(this.displayNewestLog));
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					button.Text.Replace("\r\n", ""),
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString(),
					",",
					CommonStr.strOperationComplete
				}), EventLogEntryType.Information, null);
				Cursor.Current = Cursors.Default;
				base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { -1 });
			}
			catch (Exception ex4)
			{
				wgAppConfig.wgLog(ex4.ToString());
			}
			if (!this.control4uploadPrivilege.bStopFingerprintComm)
			{
				try
				{
					base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { -1 });
					base.Invoke(new dfrmFingerManagement.displayNewestLogBK(this.displayNewestLog));
				}
				catch
				{
				}
			}
			return 1;
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x001A1F80 File Offset: 0x001A0F80
		private int uploadConfigure()
		{
			try
			{
				Button button = this.btnConfigure;
				DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
				dataView.RowFilter = "f_Selected > 0";
				if (this.control4uploadPrivilege != null)
				{
					this.control4uploadPrivilege = null;
				}
				this.control4uploadPrivilege = new icController();
				byte[] array = new byte[1152];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 0;
				}
				long num = -1L;
				long num2 = -1L;
				long.TryParse(this.superCard1_IPWEB, out num);
				long.TryParse(this.superCard2_IPWEB, out num2);
				if (num <= 0L)
				{
					num = -1L;
				}
				if (num2 <= 0L)
				{
					num2 = -1L;
				}
				wgAppConfig.setSystemParamValue(189, "Activate Fingerprint SuperCard", "1", this.superCard1_IPWEB + "," + this.superCard2_IPWEB);
				int num3 = 144;
				array[num3] = (byte)(num & 255L);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num >> 8);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num >> 16);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num >> 24);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num >> 32);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num >> 40);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num >> 48);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num >> 56);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num2 & 255L);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num2 >> 8);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num2 >> 16);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num2 >> 24);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num2 >> 32);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num2 >> 40);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num2 >> 48);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				array[num3] = (byte)(num2 >> 56);
				array[1024 + (num3 >> 3)] = array[1024 + (num3 >> 3)] | (byte)(1 << (num3 & 7));
				num3++;
				int num4 = 0;
				foreach (object obj in dataView)
				{
					DataRowView dataRowView = (DataRowView)obj;
					if (this.control4uploadPrivilege.bStopFingerprintComm)
					{
						break;
					}
					if (wgMjController.IsFingerController((int)dataRowView["f_ControllerSN"]))
					{
						try
						{
							wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]);
							this.control4uploadPrivilege.ControllerSN = (int)dataRowView["f_ControllerSN"];
							this.control4uploadPrivilege.IP = wgTools.SetObjToStr(dataRowView["f_IP"]);
							this.control4uploadPrivilege.PORT = (int)dataRowView["f_PORT"];
							int num5 = this.control4uploadPrivilege.UpdateConfigureCPUSuperIP(array, "", "");
							if (num5 <= 0)
							{
								if (this.control4uploadPrivilege.bStopFingerprintComm)
								{
									break;
								}
								wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " Upload Configure Failed num =" + num5.ToString(), new object[0]);
								base.Invoke(new dfrmFingerManagement.addLogEvent3BK(this.addLogEvent), new object[]
								{
									string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
									string.Format("{0}--{1}[{2}]:{3}:{4}", new object[]
									{
										CommonStr.strCommFail,
										wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]),
										wgTools.SetObjToStr(dataRowView["f_ControllerSN"]),
										wgTools.SetObjToStr(dataRowView["f_IP"]),
										wgTools.SetObjToStr(dataRowView["f_Port"])
									}),
									101
								});
							}
							else
							{
								wgAppConfig.wgLog(string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text) + " " + string.Format("{0} {1}.[{2},{3}] ", new object[]
								{
									button.Text,
									CommonStr.strSuccessfully,
									this.superCard1_IPWEB,
									this.superCard2_IPWEB
								}));
								base.Invoke(new dfrmFingerManagement.addLogEventBK(this.addLogEvent), new object[]
								{
									string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
									string.Format("{0} {1}.[{2},{3}] ", new object[]
									{
										button.Text,
										CommonStr.strSuccessfully,
										this.superCard1_IPWEB,
										this.superCard2_IPWEB
									})
								});
							}
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
					}
					num4++;
					base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { num4 });
				}
				if (this.control4uploadPrivilege.bStopFingerprintComm)
				{
					return 1;
				}
				base.Invoke(new dfrmFingerManagement.displayNewestLogBK(this.displayNewestLog));
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					button.Text.Replace("\r\n", ""),
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString(),
					",",
					CommonStr.strOperationComplete
				}), EventLogEntryType.Information, null);
				Cursor.Current = Cursors.Default;
				base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { -1 });
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
			if (!this.control4uploadPrivilege.bStopFingerprintComm)
			{
				try
				{
					base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { -1 });
					base.Invoke(new dfrmFingerManagement.displayNewestLogBK(this.displayNewestLog));
				}
				catch (Exception ex3)
				{
					wgAppConfig.wgLog(ex3.ToString());
				}
			}
			return 1;
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x001A28D0 File Offset: 0x001A18D0
		private int uploadOneUserFingerprints()
		{
			try
			{
				int num = this.uploadconsumerID;
				this.uploadconsumerID = 0;
				Button button = this.btnUploadAllUsers;
				DataView dataView = new DataView(((DataView)this.dgvSelectedDoors.DataSource).Table.Copy());
				dataView.RowFilter = "f_Selected > 0";
				if (this.control4uploadPrivilege != null)
				{
					if (this.control4uploadPrivilege.bStopFingerprintComm)
					{
						return 1;
					}
					this.control4uploadPrivilege = null;
				}
				this.control4uploadPrivilege = new icController();
				this.control4uploadPrivilege.bStopFingerprintComm = false;
				byte[] array = new byte[4096];
				byte[] array2 = new byte[524288];
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = byte.MaxValue;
				}
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = byte.MaxValue;
				}
				string text = "";
				string text2 = "";
				int num4 = 0;
				try
				{
					using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
					{
						using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
						{
							dbConnection.Open();
							text = string.Format("SELECT t_b_Consumer_Fingerprint.*, t_b_Consumer.f_CardNO  From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer.f_CardNO>0 AND t_b_Consumer_Fingerprint.f_ConsumerID = t_b_Consumer.f_ConsumerID AND t_b_Consumer_Fingerprint.f_ConsumerID = {0} ORDER BY f_FingerNO ASC", num);
							dbCommand.CommandText = text;
							DbDataReader dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								num4 = (int)dbDataReader["f_FingerNO"];
								if (num4 <= 0 || num4 >= 1024)
								{
									break;
								}
								num3++;
								Array.Copy(BitConverter.GetBytes(long.Parse(dbDataReader["f_CardNO"].ToString())), 0, array, (num4 - 1) * 4, 4);
								text2 = dbDataReader["f_FingerInfo"] as string;
								if (!string.IsNullOrEmpty(text2))
								{
									text2 = text2.Replace("\r\n", "");
									for (int k = 0; k < text2.Length; k += 2)
									{
										try
										{
											array2[(num4 - 1) * 512 + k / 2] = byte.Parse(text2.Substring(k, 2), NumberStyles.AllowHexSpecifier);
										}
										catch (Exception ex)
										{
											wgAppConfig.wgLog(string.Concat(new object[]
											{
												ex.ToString(),
												"\r\nfingerNO=",
												num4,
												",strFinger=",
												text2
											}));
										}
									}
								}
								if (num2 < num4)
								{
									num2 = num4;
								}
							}
							dbDataReader.Close();
						}
					}
				}
				catch (Exception ex2)
				{
					wgAppConfig.wgLog(string.Concat(new object[]
					{
						ex2.ToString(),
						"\r\nfingerNO=",
						num4,
						",strFinger=",
						text2
					}));
				}
				int num5 = 0;
				foreach (object obj in dataView)
				{
					DataRowView dataRowView = (DataRowView)obj;
					if (wgMjController.IsFingerController((int)dataRowView["f_ControllerSN"]))
					{
						base.Invoke(new dfrmFingerManagement.addLogEventBK(this.addLogEvent), new object[]
						{
							string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
							CommonStr.strStart
						});
						try
						{
							if (this.control4uploadPrivilege.bStopFingerprintComm)
							{
								break;
							}
							string text3 = wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]);
							this.control4uploadPrivilege.ControllerSN = (int)dataRowView["f_ControllerSN"];
							this.control4uploadPrivilege.IP = wgTools.SetObjToStr(dataRowView["f_IP"]);
							this.control4uploadPrivilege.PORT = (int)dataRowView["f_PORT"];
							int num6 = this.control4uploadPrivilege.UpdateFingerprintListIP(num2, array, array2, text3, -1);
							if (num6 <= 0)
							{
								if (this.control4uploadPrivilege.bStopFingerprintComm)
								{
									break;
								}
								wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " UpdateFingerprintListIP Failed num =" + num6.ToString(), new object[0]);
								if (num6 == -1999)
								{
									base.Invoke(new dfrmFingerManagement.addLogEvent3BK(this.addLogEvent), new object[]
									{
										string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
										string.Format("{0}--{1}[{2}]:{3}:{4}", new object[]
										{
											CommonStr.strSupposeUpgradeDriver,
											wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]),
											wgTools.SetObjToStr(dataRowView["f_ControllerSN"]),
											wgTools.SetObjToStr(dataRowView["f_IP"]),
											wgTools.SetObjToStr(dataRowView["f_Port"])
										}),
										5
									});
								}
								else
								{
									base.Invoke(new dfrmFingerManagement.addLogEvent3BK(this.addLogEvent), new object[]
									{
										string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
										string.Format("{0}--{1}[{2}]:{3}:{4}", new object[]
										{
											CommonStr.strCommFail,
											wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]),
											wgTools.SetObjToStr(dataRowView["f_ControllerSN"]),
											wgTools.SetObjToStr(dataRowView["f_IP"]),
											wgTools.SetObjToStr(dataRowView["f_Port"])
										}),
										101
									});
								}
							}
							else
							{
								wgAppConfig.wgLog(string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text) + " " + string.Format("{0} {1}.[Total:{2}/MaxNO:{3}] ", new object[]
								{
									button.Text,
									CommonStr.strSuccessfully,
									num3,
									num2
								}));
								base.Invoke(new dfrmFingerManagement.addLogEventBK(this.addLogEvent), new object[]
								{
									string.Format("{0}[{1}]{2}", wgTools.SetObjToStr(dataRowView["f_FingerPrintName"]), wgTools.SetObjToStr(dataRowView["f_ControllerSN"]), button.Text),
									string.Format("{0} {1}.[{2}] ", button.Text, CommonStr.strSuccessfully, num3)
								});
							}
						}
						catch (Exception ex3)
						{
							wgAppConfig.wgLog(ex3.ToString());
						}
					}
					num5++;
					base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { num5 });
				}
				if (this.control4uploadPrivilege.bStopFingerprintComm)
				{
					return 1;
				}
				icConsumerShare.setUpdateLog();
				base.Invoke(new dfrmFingerManagement.displayNewestLogBK(this.displayNewestLog));
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					button.Text.Replace("\r\n", ""),
					" ,",
					CommonStr.strDevicesNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString(),
					",",
					CommonStr.strOperationComplete
				}), EventLogEntryType.Information, null);
				Cursor.Current = Cursors.Default;
				base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { -1 });
			}
			catch (Exception ex4)
			{
				wgAppConfig.wgLog(ex4.ToString());
			}
			if (!this.control4uploadPrivilege.bStopFingerprintComm)
			{
				try
				{
					base.Invoke(new dfrmFingerManagement.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { -1 });
					base.Invoke(new dfrmFingerManagement.displayNewestLogBK(this.displayNewestLog));
				}
				catch
				{
				}
			}
			return 1;
		}

		// Token: 0x04002BA3 RID: 11171
		public const int DeviceCodePage = 936;

		// Token: 0x04002BA4 RID: 11172
		private bool bDataErrorExist;

		// Token: 0x04002BA5 RID: 11173
		private int consumerID;

		// Token: 0x04002BA6 RID: 11174
		private icController control4uploadPrivilege;

		// Token: 0x04002BA7 RID: 11175
		private Control defaultFindControl;

		// Token: 0x04002BA8 RID: 11176
		private DataView dv;

		// Token: 0x04002BA9 RID: 11177
		private DataView dv1;

		// Token: 0x04002BAA RID: 11178
		private DataView dv2;

		// Token: 0x04002BAB RID: 11179
		private DataView dvSelected;

		// Token: 0x04002BAC RID: 11180
		private int eventRecID;

		// Token: 0x04002BAD RID: 11181
		private DataTable tbRunInfoLog;

		// Token: 0x04002BAE RID: 11182
		private TcpClient tcp;

		// Token: 0x04002BAF RID: 11183
		private int uploadconsumerID;

		// Token: 0x04002BB0 RID: 11184
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04002BB1 RID: 11185
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04002BB2 RID: 11186
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04002BB3 RID: 11187
		private ArrayList arrIPARPFailed = new ArrayList();

		// Token: 0x04002BB4 RID: 11188
		private ArrayList arrIPARPOK = new ArrayList();

		// Token: 0x04002BB5 RID: 11189
		private ArrayList arrIPPingFailed = new ArrayList();

		// Token: 0x04002BB6 RID: 11190
		private ArrayList arrIPPingOK = new ArrayList();

		// Token: 0x04002BB7 RID: 11191
		private bool bStarting = true;

		// Token: 0x04002BB8 RID: 11192
		private dfrmWait dfrmWait1 = new dfrmWait();

		// Token: 0x04002BB9 RID: 11193
		private string strGroupFilter = "";

		// Token: 0x04002BBA RID: 11194
		private string superCard1_IPWEB = "";

		// Token: 0x04002BBB RID: 11195
		private string superCard2_IPWEB = "";

		// Token: 0x04002BBC RID: 11196
		private int tcpBuffSize = 20480;

		// Token: 0x020002E3 RID: 739
		// (Invoke) Token: 0x06001537 RID: 5431
		private delegate void addLogEvent3BK(string desc, string info, int category);

		// Token: 0x020002E4 RID: 740
		// (Invoke) Token: 0x0600153B RID: 5435
		private delegate void addLogEventBK(string desc, string info);

		// Token: 0x020002E5 RID: 741
		// (Invoke) Token: 0x0600153F RID: 5439
		private delegate void displayNewestLogBK();

		// Token: 0x020002E6 RID: 742
		public class FaceId_Item
		{
			// Token: 0x06001542 RID: 5442 RVA: 0x001A64B8 File Offset: 0x001A54B8
			public FaceId_Item(string keyName, string keyValue)
			{
				this.Name = keyName;
				this.Value = keyValue;
			}

			// Token: 0x06001543 RID: 5443 RVA: 0x001A64D0 File Offset: 0x001A54D0
			public static dfrmFingerManagement.FaceId_Item[] GetAllItems(string Answer)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					List<dfrmFingerManagement.FaceId_Item> list = new List<dfrmFingerManagement.FaceId_Item>();
					string text = "\\b([a-z|A-Z|_]+)\\s*=\\s*\"([^\"]+)\"";
					MatchCollection matchCollection = Regex.Matches(Answer, text);
					if (matchCollection != null)
					{
						foreach (object obj in matchCollection)
						{
							Match match = (Match)obj;
							list.Add(new dfrmFingerManagement.FaceId_Item(match.Groups[1].Value, match.Groups[2].Value));
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
				return null;
			}

			// Token: 0x06001544 RID: 5444 RVA: 0x001A6584 File Offset: 0x001A5584
			public static dfrmFingerManagement.FaceId_Item[] GetAllPairs(string Answer, string LeftKeyName, string RightKeyName)
			{
				if (!string.IsNullOrEmpty(Answer))
				{
					List<dfrmFingerManagement.FaceId_Item> list = new List<dfrmFingerManagement.FaceId_Item>();
					string text = string.Concat(new string[] { "\\b", LeftKeyName, "=\"([^\"]+)\"\\s*", RightKeyName, "=\"([^\"]+)\"" });
					MatchCollection matchCollection = Regex.Matches(Answer, text);
					if (matchCollection != null)
					{
						foreach (object obj in matchCollection)
						{
							Match match = (Match)obj;
							list.Add(new dfrmFingerManagement.FaceId_Item(match.Groups[1].Value, match.Groups[2].Value));
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
				return null;
			}

			// Token: 0x06001545 RID: 5445 RVA: 0x001A6668 File Offset: 0x001A5668
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

			// Token: 0x06001546 RID: 5446 RVA: 0x001A66B4 File Offset: 0x001A56B4
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

			// Token: 0x04002C1C RID: 11292
			public readonly string Name;

			// Token: 0x04002C1D RID: 11293
			public readonly string Value;
		}

		// Token: 0x020002E7 RID: 743
		// (Invoke) Token: 0x06001548 RID: 5448
		private delegate void txtInfoUpdate(object info);

		// Token: 0x020002E8 RID: 744
		// (Invoke) Token: 0x0600154C RID: 5452
		private delegate void updateButtonDisplayDlg();
	}
}
