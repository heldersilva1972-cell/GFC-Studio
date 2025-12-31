using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000051 RID: 81
	public partial class frmPrivileges : Form
	{
		// Token: 0x060005C2 RID: 1474 RVA: 0x000A1B30 File Offset: 0x000A0B30
		public frmPrivileges()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvPrivileges);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x000A1BA4 File Offset: 0x000A0BA4
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			int num = (int)((object[])e.Argument)[0];
			int num2 = (int)((object[])e.Argument)[1];
			string text = (string)((object[])e.Argument)[2];
			e.Result = this.loadData(num, num2, text);
			if (backgroundWorker.CancellationPending)
			{
				wgTools.WriteLine("bw.CancellationPending");
				e.Cancel = true;
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x000A1C30 File Offset: 0x000A0C30
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
			if ((e.Result as DataView).Count < this.MaxRecord)
			{
				if (this.iSelectedControllerIndex + 1 < this.arrController.Count)
				{
					this.iSelectedControllerIndex++;
					this.startRecordIndex = 0;
				}
				else
				{
					this.bLoadedFinished = true;
					this.bLoadAll = false;
				}
			}
			this.fillDgv(e.Result as DataView);
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvPrivileges.Rows.Count.ToString() + this.strAllPrivilegsNum + (this.bLoadedFinished ? "#" : "..."));
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x000A1D1E File Offset: 0x000A0D1E
		private void btnClear_Click(object sender, EventArgs e)
		{
			this.cboDoor.SelectedIndex = -1;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000A1D2C File Offset: 0x000A0D2C
		private void btnEdit_Click(object sender, EventArgs e)
		{
			bool flag = false;
			if (this.backgroundWorker1.IsBusy)
			{
				flag = true;
				this.backgroundWorker1.CancelAsync();
			}
			using (dfrmPrivilege dfrmPrivilege = new dfrmPrivilege())
			{
				if (dfrmPrivilege.ShowDialog(this) != DialogResult.OK)
				{
					if (flag)
					{
						this.userControlFind1.btnQuery.PerformClick();
					}
					return;
				}
				if (dfrmPrivilege.bLoadConsole)
				{
					(base.ParentForm as frmADCT3000).shortcutConsole_Click(null, null);
					icPrivilegeShare.setNeedRefresh();
					return;
				}
			}
			icPrivilegeShare.setNeedRefresh();
			this.userControlFind1.btnQuery.PerformClick();
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000A1DCC File Offset: 0x000A0DCC
		private void btnEditSinglePrivilege_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = this.dgvPrivileges;
				int num;
				if (dataGridView.SelectedRows.Count <= 0)
				{
					if (dataGridView.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dataGridView.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dataGridView.SelectedRows[0].Index;
				}
				bool flag = false;
				if (this.backgroundWorker1.IsBusy)
				{
					flag = true;
					this.backgroundWorker1.CancelAsync();
				}
				using (dfrmPrivilegeSingle dfrmPrivilegeSingle = new dfrmPrivilegeSingle())
				{
					dfrmPrivilegeSingle.consumerID = int.Parse(dataGridView.Rows[num].Cells[7].Value.ToString());
					dfrmPrivilegeSingle.Text = string.Concat(new string[]
					{
						dataGridView.Rows[num].Cells[2].Value.ToString().Trim(),
						".",
						dataGridView.Rows[num].Cells[3].Value.ToString().Trim(),
						" -- ",
						dfrmPrivilegeSingle.Text
					});
					if (dfrmPrivilegeSingle.ShowDialog(this) != DialogResult.OK)
					{
						if (flag)
						{
							this.userControlFind1.btnQuery.PerformClick();
						}
						return;
					}
				}
				icPrivilegeShare.setNeedRefresh();
				this.userControlFind1.btnQuery.PerformClick();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000A1F88 File Offset: 0x000A0F88
		private void btnExport_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = this.dgvPrivileges;
				if (dataGridView.Rows.Count <= 65535 && !this.bLoadedFinished)
				{
					if (!this.bLoadedFinished && !this.bLoadAll)
					{
						this.bLoadAll = true;
						this.btnQuery_Click(null, null);
					}
					using (dfrmWait dfrmWait = new dfrmWait())
					{
						dfrmWait.Show();
						dfrmWait.Refresh();
						while (this.backgroundWorker1.IsBusy || this.bLoadAll)
						{
							Thread.Sleep(500);
							Application.DoEvents();
						}
						dfrmWait.Hide();
					}
				}
				using (dfrmWait dfrmWait2 = new dfrmWait())
				{
					dfrmWait2.Show();
					dfrmWait2.Refresh();
					foreach (object obj in this.dgvPrivileges.Columns)
					{
						DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj;
						if (dataGridViewColumn.Name.Equals("f_ControlSegName"))
						{
							foreach (object obj2 in ((IEnumerable)this.dgvPrivileges.Rows))
							{
								DataGridViewRow dataGridViewRow = (DataGridViewRow)obj2;
								DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[dataGridViewColumn.Index];
								if (dataGridViewCell.Value != null && dataGridViewCell.Value as string == " ")
								{
									int num = (int)dataGridViewRow.Cells[dataGridViewColumn.Index - 1].Value;
									for (int i = 0; i < this.controlSegIDList.Length; i++)
									{
										if (this.controlSegIDList[i] == num)
										{
											dataGridViewCell.Value = this.controlSegNameList[i].ToString();
											break;
										}
									}
								}
							}
						}
					}
					dfrmWait2.Hide();
				}
				wgAppConfig.exportToExcel(dataGridView, this.Text);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000A2218 File Offset: 0x000A1218
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvPrivileges);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000A222C File Offset: 0x000A122C
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvPrivileges, this.Text);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000A2240 File Offset: 0x000A1240
		private void btnPrivilegeCopy_Click(object sender, EventArgs e)
		{
			try
			{
				bool flag = false;
				if (this.backgroundWorker1.IsBusy)
				{
					flag = true;
					this.backgroundWorker1.CancelAsync();
				}
				using (dfrmPrivilegeCopy dfrmPrivilegeCopy = new dfrmPrivilegeCopy())
				{
					if (dfrmPrivilegeCopy.ShowDialog(this) != DialogResult.OK)
					{
						if (flag)
						{
							this.userControlFind1.btnQuery.PerformClick();
						}
						return;
					}
				}
				icPrivilegeShare.setNeedRefresh();
				this.userControlFind1.btnQuery.PerformClick();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000A22E4 File Offset: 0x000A12E4
		private void btnQuery_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text2 = " SELECT  t_d_Privilege.f_PrivilegeRecID,t_b_Door.f_DoorName, t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, t_b_Consumer.f_CardNO,  t_d_Privilege.f_ControlSegID,' ' as  f_ControlSegName, t_b_Consumer.f_ConsumerID ";
			string text3 = wgAppConfig.getSqlFindPrilivege(text2, "t_d_Privilege", "", num, num2, num3, text, num4, num5);
			if (this.cboDoor.Text != "")
			{
				this.dvDoors4Watching.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboDoor.Text);
				if (text3.ToUpper().IndexOf(" WHERE ") > 0)
				{
					text3 = text3 + " AND t_d_Privilege.f_DoorID =" + this.dvDoors4Watching[0]["f_DoorID"].ToString();
				}
				else
				{
					text3 = text3 + " WHERE t_d_Privilege.f_DoorID =" + this.dvDoors4Watching[0]["f_DoorID"].ToString();
				}
				this.arrController.Clear();
				this.arrController.Add(this.dvDoors4Watching[0]["f_ControllerID"]);
			}
			else
			{
				this.arrController.Clear();
				for (int i = 0; i < this.arrControllerAllEnabled.Count; i++)
				{
					this.arrController.Add(this.arrControllerAllEnabled[i]);
				}
			}
			this.reloadData(text3);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000A2459 File Offset: 0x000A1459
		private void button1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000A245C File Offset: 0x000A145C
		private void cardNOFuzzyQueryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = "";
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = sender.ToString();
				dfrmInputNewName.label1.Text = CommonStr.strCardID;
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				text = dfrmInputNewName.strNewName;
			}
			if (!string.IsNullOrEmpty(text))
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				string text2 = "";
				long num4 = 0L;
				int num5 = 0;
				this.userControlFind1.txtFindCardID.Text = "";
				this.userControlFind1.txtFindName.Text = "";
				this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text2, ref num4, ref num5);
				string text3 = " SELECT  t_d_Privilege.f_PrivilegeRecID,t_b_Door.f_DoorName, t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, t_b_Consumer.f_CardNO,  t_d_Privilege.f_ControlSegID,' ' as  f_ControlSegName, t_b_Consumer.f_ConsumerID ";
				string text4 = wgAppConfig.getSqlFindPrilivege(text3, "t_d_Privilege", "", num, num2, num3, text2, num4, num5);
				if (this.cboDoor.Text != "")
				{
					this.dvDoors4Watching.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboDoor.Text);
					if (text4.ToUpper().IndexOf(" WHERE ") > 0)
					{
						text4 = text4 + " AND t_d_Privilege.f_DoorID =" + this.dvDoors4Watching[0]["f_DoorID"].ToString();
					}
					else
					{
						text4 = text4 + " WHERE t_d_Privilege.f_DoorID =" + this.dvDoors4Watching[0]["f_DoorID"].ToString();
					}
					this.arrController.Clear();
					this.arrController.Add(this.dvDoors4Watching[0]["f_ControllerID"]);
				}
				else
				{
					this.arrController.Clear();
					for (int i = 0; i < this.arrControllerAllEnabled.Count; i++)
					{
						this.arrController.Add(this.arrControllerAllEnabled[i]);
					}
				}
				string text5 = " ( 1>0 ) ";
				if (text.IndexOf("%") < 0)
				{
					text = string.Format("%{0}%", text);
				}
				if (wgAppConfig.IsAccessDB)
				{
					text5 += string.Format(" AND CSTR(f_CardNO) like {0} ", wgTools.PrepareStrNUnicode(text));
				}
				else
				{
					text5 += string.Format(" AND f_CardNO like {0} ", wgTools.PrepareStrNUnicode(text));
				}
				if (text4.ToUpper().IndexOf("WHERE") > 0)
				{
					text4 += string.Format(" AND {0} ", text5);
				}
				else
				{
					text4 += string.Format(" WHERE {0} ", text5);
				}
				this.reloadData(text4);
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000A2704 File Offset: 0x000A1704
		private void cboDoor_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				this.userControlFind1.btnQuery.PerformClick();
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000A2720 File Offset: 0x000A1720
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000A2738 File Offset: 0x000A1738
		private void dfrm_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					wgAppConfig.findCall(this.dfrmFind1, this, this.dgvPrivileges);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000A2794 File Offset: 0x000A1794
		private void dgvPrivileges_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvPrivileges.Columns.Count && this.dgvPrivileges.Columns[e.ColumnIndex].Name.Equals("f_ControlSegName"))
			{
				string text;
				if ((text = e.Value as string) != null && !(text == " "))
				{
					return;
				}
				DataGridViewCell dataGridViewCell = this.dgvPrivileges[e.ColumnIndex, e.RowIndex];
				int num = (int)this.dgvPrivileges[e.ColumnIndex - 1, e.RowIndex].Value;
				for (int i = 0; i < this.controlSegIDList.Length; i++)
				{
					if (this.controlSegIDList[i] == num)
					{
						e.Value = this.controlSegNameList[i].ToString();
						dataGridViewCell.Value = e.Value;
						return;
					}
				}
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000A2898 File Offset: 0x000A1898
		private void dgvPrivileges_Scroll(object sender, ScrollEventArgs e)
		{
			if (!this.bLoadedFinished && e.ScrollOrientation == ScrollOrientation.VerticalScroll)
			{
				wgTools.WriteLine(e.OldValue.ToString());
				wgTools.WriteLine(e.NewValue.ToString());
				DataGridView dataGridView = this.dgvPrivileges;
				if (e.NewValue > e.OldValue && (e.NewValue + 100 > dataGridView.Rows.Count || e.NewValue + dataGridView.Rows.Count / 10 > dataGridView.Rows.Count))
				{
					if (this.iSelectedControllerIndex + 1 < this.arrController.Count || this.startRecordIndex <= dataGridView.Rows.Count)
					{
						if (!this.backgroundWorker1.IsBusy)
						{
							this.startRecordIndex += this.MaxRecord;
							this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
							return;
						}
					}
					else
					{
						wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvPrivileges.Rows.Count.ToString() + "/" + this.dgvPrivileges.Rows.Count.ToString() + "#");
					}
				}
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000A29FF File Offset: 0x000A19FF
		private void displayAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!this.bLoadAll)
			{
				this.bLoadAll = true;
				this.btnQuery_Click(null, null);
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000A2A18 File Offset: 0x000A1A18
		private void exportOver65535ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataGridView dataGridView = this.dgvPrivileges;
			if (dataGridView.Rows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strNoDataToExport);
				return;
			}
			if (!this.bLoadedFinished)
			{
				using (dfrmWait dfrmWait = new dfrmWait())
				{
					dfrmWait.Show();
					dfrmWait.Refresh();
					while (this.backgroundWorker1.IsBusy)
					{
						Thread.Sleep(500);
						Application.DoEvents();
					}
					while (this.startRecordIndex <= dataGridView.Rows.Count)
					{
						this.startRecordIndex += this.MaxRecord;
						this.MaxRecord = 100000;
						this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
						while (this.backgroundWorker1.IsBusy)
						{
							Thread.Sleep(500);
							Application.DoEvents();
						}
						dfrmWait.Text = dataGridView.Rows.Count.ToString();
					}
					wgAppRunInfo.raiseAppRunInfoLoadNums(dataGridView.Rows.Count.ToString() + "#");
					this.MaxRecord = 1000;
					dfrmWait.Hide();
				}
			}
			wgAppConfig.exportToExcel100W(dataGridView, this.Text);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000A2B94 File Offset: 0x000A1B94
		private void fillDgv(DataView dv)
		{
			try
			{
				DataGridView dataGridView = this.dgvPrivileges;
				if (dataGridView.DataSource == null)
				{
					dataGridView.DataSource = dv;
					for (int i = 0; i < dv.Table.Columns.Count; i++)
					{
						dataGridView.Columns[i].DataPropertyName = dv.Table.Columns[i].ColumnName;
					}
					wgAppConfig.ReadGVStyle(this, dataGridView);
					if (this.startRecordIndex == 0 && dv.Count >= this.MaxRecord)
					{
						this.startRecordIndex += this.MaxRecord;
						this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
					}
				}
				else if (dv.Count > 0)
				{
					int firstDisplayedScrollingRowIndex = dataGridView.FirstDisplayedScrollingRowIndex;
					DataView dataView = dataGridView.DataSource as DataView;
					dataView.Table.Merge(dv.Table);
					if (firstDisplayedScrollingRowIndex >= 0)
					{
						dataGridView.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000A2CE8 File Offset: 0x000A1CE8
		private void frmPrivileges_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.dfrm_FormClosing(sender, e);
			this.backgroundWorker1.CancelAsync();
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000A2D00 File Offset: 0x000A1D00
		private void frmPrivileges_Load(object sender, EventArgs e)
		{
			Icon icon = base.Icon;
			wgAppConfig.GetAppIcon(ref icon);
			base.Icon = icon;
			this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
			this.loadOperatorPrivilege();
			this.userControlFind1.btnQuery.Click += this.btnQuery_Click;
			this.userControlFind1.btnClear.Click += this.btnClear_Click;
			this.loadStyle();
			this.loadControlSegData();
			this.loadDoorData();
			this.loadControllerData();
			wgAppConfig.HideCardNOColumn(this.dgvPrivileges.Columns["f_CardNO"]);
			icControllerZone icControllerZone = new icControllerZone();
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			icControllerZone.getZone(ref arrayList, ref arrayList2, ref arrayList3);
			if (arrayList2.Count > 0 && (int)arrayList2[0] != 0)
			{
				this.btnEditSinglePrivilege.Enabled = false;
			}
			if (!wgAppConfig.getParamValBoolByNO(142))
			{
				this.userControlFind1.btnQuery.PerformClick();
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x000A2E14 File Offset: 0x000A1E14
		private void loadControllerData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadControllerData_Acc();
				return;
			}
			string text = " SELECT f_ControllerID   FROM [t_b_Controller] WHERE f_Enabled > 0";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					this.arrControllerAllEnabled = new ArrayList();
					this.arrController = new ArrayList();
					while (sqlDataReader.Read())
					{
						this.arrControllerAllEnabled.Add(sqlDataReader[0]);
						this.arrController.Add(sqlDataReader[0]);
					}
					sqlDataReader.Close();
				}
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000A2ED8 File Offset: 0x000A1ED8
		private void loadControllerData_Acc()
		{
			string text = " SELECT f_ControllerID   FROM [t_b_Controller] WHERE f_Enabled > 0";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					this.arrControllerAllEnabled = new ArrayList();
					this.arrController = new ArrayList();
					while (oleDbDataReader.Read())
					{
						this.arrControllerAllEnabled.Add(oleDbDataReader[0]);
						this.arrController.Add(oleDbDataReader[0]);
					}
					oleDbDataReader.Close();
				}
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x000A2F8C File Offset: 0x000A1F8C
		private void loadControlSegData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadControlSegData_Acc();
				return;
			}
			this.controlSegNameList[0] = CommonStr.strFreeTime;
			this.controlSegIDList[0] = 1;
			string text = " SELECT ";
			text += " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,    CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),       ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50),      ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']')     END AS f_ControlSegID    FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					int num = 1;
					while (sqlDataReader.Read())
					{
						this.controlSegNameList[num] = (string)sqlDataReader["f_ControlSegID"];
						this.controlSegIDList[num] = (int)sqlDataReader["f_ControlSegIDBak"];
						num++;
					}
					sqlDataReader.Close();
				}
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000A3070 File Offset: 0x000A2070
		private void loadControlSegData_Acc()
		{
			this.controlSegNameList[0] = CommonStr.strFreeTime;
			this.controlSegIDList[0] = 1;
			string text = " SELECT ";
			text += " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,   IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID   FROM [t_b_ControlSeg]  WHERE  [t_b_ControlSeg].[f_ControlSegID]>1  ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					int num = 1;
					while (oleDbDataReader.Read())
					{
						this.controlSegNameList[num] = (string)oleDbDataReader["f_ControlSegID"];
						this.controlSegIDList[num] = (int)oleDbDataReader["f_ControlSegIDBak"];
						num++;
					}
					oleDbDataReader.Close();
				}
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000A3144 File Offset: 0x000A2144
		private DataView loadData(int startIndex, int maxRecords, string strSqlpar)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.loadData_Acc(startIndex, maxRecords, strSqlpar);
			}
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("load Privileges Data Start");
			int num = this.iSelectedControllerIndex;
			if (this.cn != null)
			{
				this.cn.Dispose();
			}
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			if (this.cmd != null)
			{
				this.cmd.Dispose();
			}
			this.cmd = new SqlCommand("", this.cn);
			if (this.da != null)
			{
				this.da.Dispose();
			}
			this.da = new SqlDataAdapter();
			this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
			this.dtData = new DataTable();
			for (;;)
			{
				string text;
				if (this.iSelectedControllerIndex == 0 && this.arrController.Count > 1)
				{
					text = "SELECT TOP 1 f_ControllerID FROM t_d_Privilege order by f_ControllerID";
					this.cmd.CommandText = text;
					if (this.cn.State != ConnectionState.Open)
					{
						this.cn.Open();
					}
					object obj = this.cmd.ExecuteScalar();
					if (obj == null)
					{
						break;
					}
					this.iSelectedControllerIndex = this.arrController.IndexOf((int)obj);
					if (this.iSelectedControllerIndex < 0)
					{
						this.iSelectedControllerIndex = 0;
					}
				}
				text = strSqlpar;
				if (!this.bLoadAll && text.ToUpper().IndexOf("SELECT ") > 0)
				{
					text = string.Format("SELECT TOP {0:d} ", maxRecords) + text.Substring(text.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
				}
				if (this.iSelectedControllerIndexLast != this.iSelectedControllerIndex)
				{
					this.recIdMax = int.MinValue;
					this.iSelectedControllerIndexLast = this.iSelectedControllerIndex;
				}
				else if (startIndex == 0)
				{
					this.recIdMax = int.MinValue;
				}
				else if (text.ToUpper().IndexOf(" WHERE ") > 0)
				{
					text += string.Format(" AND f_PrivilegeRecID > {0:d}", this.recIdMax);
				}
				else
				{
					text += string.Format(" WHERE f_PrivilegeRecID > {0:d}", this.recIdMax);
				}
				if (this.iSelectedControllerIndex >= this.arrController.Count)
				{
					goto IL_0435;
				}
				if (this.bLoadAll)
				{
					if (this.iSelectedControllerIndex >= this.arrController.Count)
					{
						goto IL_0435;
					}
					if (text.ToUpper().IndexOf(" WHERE ") > 0)
					{
						text += string.Format(" AND t_d_Privilege.f_ControllerID >= {0:d}", (int)this.arrController[this.iSelectedControllerIndex]);
					}
					else
					{
						text += string.Format(" WHERE t_d_Privilege.f_ControllerID >= {0:d}", (int)this.arrController[this.iSelectedControllerIndex]);
					}
					this.iSelectedControllerIndex = this.arrController.Count;
				}
				else if (text.ToUpper().IndexOf(" WHERE ") > 0)
				{
					text += string.Format(" AND t_d_Privilege.f_ControllerID = {0:d}", (int)this.arrController[this.iSelectedControllerIndex]);
				}
				else
				{
					text += string.Format(" WHERE t_d_Privilege.f_ControllerID = {0:d}", (int)this.arrController[this.iSelectedControllerIndex]);
				}
				text += " ORDER BY f_PrivilegeRecID ";
				this.cmd.CommandText = text;
				this.da.SelectCommand = this.cmd;
				wgTools.WriteLine("da.Fill start");
				this.da.Fill(this.dtData);
				if (this.dtData.Rows.Count > 0 && (num != 0 || this.dtData.Rows.Count >= 100))
				{
					goto IL_03BD;
				}
				this.iSelectedControllerIndex++;
				if (this.iSelectedControllerIndex >= this.arrController.Count)
				{
					goto IL_0435;
				}
			}
			this.iSelectedControllerIndex = this.arrController.Count - 1;
			return new DataView(this.dtData);
			IL_03BD:
			this.recIdMax = int.Parse(this.dtData.Rows[this.dtData.Rows.Count - 1][0].ToString());
			wgTools.WriteLine(string.Format("recIdMax = {0:d}", this.recIdMax));
			IL_0435:
			wgTools.WriteLine("da.Fill End " + startIndex.ToString());
			Cursor.Current = Cursors.Default;
			wgTools.WriteLine("load Privileges Data End");
			return new DataView(this.dtData);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000A35BC File Offset: 0x000A25BC
		private DataView loadData_Acc(int startIndex, int maxRecords, string strSqlpar)
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("load Privileges Data Start");
			int num = this.iSelectedControllerIndex;
			if (this.cn_Acc != null)
			{
				this.cn_Acc.Dispose();
			}
			this.cn_Acc = new OleDbConnection(wgAppConfig.dbConString);
			if (this.cmd_Acc != null)
			{
				this.cmd_Acc.Dispose();
			}
			this.cmd_Acc = new OleDbCommand("", this.cn_Acc);
			if (this.da_Acc != null)
			{
				this.da_Acc.Dispose();
			}
			this.da_Acc = new OleDbDataAdapter();
			this.cmd_Acc.CommandTimeout = wgAppConfig.dbCommandTimeout;
			this.dtData = new DataTable();
			for (;;)
			{
				string text;
				if (this.iSelectedControllerIndex == 0 && this.arrController.Count > 1)
				{
					text = "SELECT TOP 1 f_ControllerID FROM t_d_Privilege order by f_ControllerID";
					this.cmd_Acc.CommandText = text;
					if (this.cn_Acc.State != ConnectionState.Open)
					{
						this.cn_Acc.Open();
					}
					object obj = this.cmd_Acc.ExecuteScalar();
					if (obj == null)
					{
						break;
					}
					this.iSelectedControllerIndex = this.arrController.IndexOf((int)obj);
					if (this.iSelectedControllerIndex < 0)
					{
						this.iSelectedControllerIndex = 0;
					}
				}
				text = strSqlpar;
				if (string.IsNullOrEmpty(text))
				{
					goto Block_9;
				}
				if (!this.bLoadAll && text.ToUpper().IndexOf("SELECT ") > 0)
				{
					text = string.Format("SELECT TOP {0:d} ", maxRecords) + text.Substring(text.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
				}
				if (this.iSelectedControllerIndexLast != this.iSelectedControllerIndex)
				{
					this.recIdMax = int.MinValue;
					this.iSelectedControllerIndexLast = this.iSelectedControllerIndex;
				}
				else if (startIndex == 0)
				{
					this.recIdMax = int.MinValue;
				}
				else if (text.ToUpper().IndexOf(" WHERE ") > 0)
				{
					text += string.Format(" AND f_PrivilegeRecID > {0:d}", this.recIdMax);
				}
				else
				{
					text += string.Format(" WHERE f_PrivilegeRecID > {0:d}", this.recIdMax);
				}
				if (this.bLoadAll)
				{
					if (this.iSelectedControllerIndex >= this.arrController.Count)
					{
						goto IL_0418;
					}
					if (text.ToUpper().IndexOf(" WHERE ") > 0)
					{
						text += string.Format(" AND t_d_Privilege.f_ControllerID >= {0:d}", (int)this.arrController[this.iSelectedControllerIndex]);
					}
					else
					{
						text += string.Format(" WHERE t_d_Privilege.f_ControllerID >= {0:d}", (int)this.arrController[this.iSelectedControllerIndex]);
					}
					this.iSelectedControllerIndex = this.arrController.Count;
				}
				else if (text.ToUpper().IndexOf(" WHERE ") > 0)
				{
					text += string.Format(" AND t_d_Privilege.f_ControllerID = {0:d}", (int)this.arrController[this.iSelectedControllerIndex]);
				}
				else
				{
					text += string.Format(" WHERE t_d_Privilege.f_ControllerID = {0:d}", (int)this.arrController[this.iSelectedControllerIndex]);
				}
				text += " ORDER BY f_PrivilegeRecID ";
				this.cmd_Acc.CommandText = text;
				this.da_Acc.SelectCommand = this.cmd_Acc;
				wgTools.WriteLine("da_Acc.Fill start");
				this.da_Acc.Fill(this.dtData);
				if (this.dtData.Rows.Count > 0 && (num != 0 || this.dtData.Rows.Count >= 100))
				{
					goto IL_03A0;
				}
				this.iSelectedControllerIndex++;
				if (this.iSelectedControllerIndex >= this.arrController.Count)
				{
					goto IL_0418;
				}
			}
			this.iSelectedControllerIndex = this.arrController.Count - 1;
			return new DataView(this.dtData);
			Block_9:
			return null;
			IL_03A0:
			this.recIdMax = int.Parse(this.dtData.Rows[this.dtData.Rows.Count - 1][0].ToString());
			wgTools.WriteLine(string.Format("recIdMax = {0:d}", this.recIdMax));
			IL_0418:
			wgTools.WriteLine("da_Acc.Fill End " + startIndex.ToString());
			Cursor.Current = Cursors.Default;
			wgTools.WriteLine("load Privileges Data End");
			return new DataView(this.dtData);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x000A3A18 File Offset: 0x000A2A18
		private void loadDoorData()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, a.f_ControllerID , b.f_ZoneID ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
			this.dt = new DataTable();
			this.dvDoors = new DataView(this.dt);
			this.dvDoors4Watching = new DataView(this.dt);
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
					goto IL_00EC;
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
			IL_00EC:
			icControllerZone icControllerZone = new icControllerZone();
			icControllerZone.getAllowedControllers(ref this.dt);
			try
			{
				this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.cboDoor.Items.Clear();
			this.cboDoor.Items.Add("");
			if (this.dvDoors.Count > 0)
			{
				for (int i = 0; i < this.dvDoors.Count; i++)
				{
					this.cboDoor.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
				}
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000A3C34 File Offset: 0x000A2C34
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuPrivilege";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnEditSinglePrivilege.Visible = false;
				this.btnEdit.Visible = false;
				this.btnPrivilegeCopy.Visible = false;
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000A3C7C File Offset: 0x000A2C7C
		private void loadStyle()
		{
			this.dgvPrivileges.AutoGenerateColumns = false;
			this.dgvPrivileges.Columns[5].Visible = wgAppConfig.getParamValBoolByNO(121);
			this.dgvPrivileges.Columns[6].Visible = wgAppConfig.getParamValBoolByNO(121);
			wgAppConfig.ReadGVStyle(this, this.dgvPrivileges);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000A3CDC File Offset: 0x000A2CDC
		private void reloadData(string strsql)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.reloadData_Acc(strsql);
				return;
			}
			if (!this.backgroundWorker1.IsBusy)
			{
				if (this.arrController.Count <= 0)
				{
					this.bLoadedFinished = true;
					this.bLoadAll = true;
					return;
				}
				this.bLoadedFinished = false;
				this.iAllPrivilegsNum = 0;
				this.iSelectedControllerIndex = 0;
				this.strAllPrivilegsNum = "";
				if (this.strSqlAllPrivileg == strsql)
				{
					try
					{
						using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
						{
							if (sqlConnection.State != ConnectionState.Open)
							{
								sqlConnection.Open();
							}
							SqlCommand sqlCommand = null;
							try
							{
								if (wgAppConfig.getSystemParamByNO(53) == "1")
								{
									sqlCommand = new SqlCommand("SELECT SUM(row_count)  FROM sys.dm_db_partition_stats WHERE object_id = OBJECT_ID('t_d_privilege') AND index_id =1 ", sqlConnection);
									this.iAllPrivilegsNum = int.Parse(sqlCommand.ExecuteScalar().ToString());
									this.strAllPrivilegsNum = "/" + this.iAllPrivilegsNum;
								}
								else
								{
									sqlCommand = new SqlCommand("select rowcnt from sysindexes where id=object_id(N't_d_Privilege') and name = N'PK_t_d_Privilege'", sqlConnection);
									this.iAllPrivilegsNum = int.Parse(sqlCommand.ExecuteScalar().ToString());
									if (this.iAllPrivilegsNum <= 2000000)
									{
										using (SqlCommand sqlCommand2 = new SqlCommand("select count(1) from t_d_Privilege", sqlConnection))
										{
											this.iAllPrivilegsNum = int.Parse(sqlCommand2.ExecuteScalar().ToString());
											this.strAllPrivilegsNum = "/" + this.iAllPrivilegsNum;
										}
									}
								}
							}
							catch (Exception)
							{
							}
							finally
							{
								if (sqlCommand != null)
								{
									sqlCommand.Dispose();
								}
							}
						}
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
				}
				wgAppRunInfo.raiseAppRunInfoLoadNums(this.strAllPrivilegsNum);
				this.startRecordIndex = 0;
				this.MaxRecord = 1000;
				if (!string.IsNullOrEmpty(strsql))
				{
					this.dgvSql = strsql;
				}
				this.dgvPrivileges.DataSource = null;
				if (this.bLoadAll)
				{
					this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, 100000000, this.dgvSql });
					return;
				}
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
			}
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x000A3FA8 File Offset: 0x000A2FA8
		private void reloadData_Acc(string strsql)
		{
			if (!this.backgroundWorker1.IsBusy)
			{
				if (this.arrController.Count <= 0)
				{
					this.bLoadedFinished = true;
					this.bLoadAll = true;
					return;
				}
				this.bLoadedFinished = false;
				this.iAllPrivilegsNum = 0;
				this.iSelectedControllerIndex = 0;
				this.strAllPrivilegsNum = "";
				if (this.strSqlAllPrivileg == strsql)
				{
					try
					{
						using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
						{
							if (oleDbConnection.State != ConnectionState.Open)
							{
								oleDbConnection.Open();
							}
							object obj = null;
							try
							{
								using (OleDbCommand oleDbCommand = new OleDbCommand("select count(1) from t_d_Privilege", oleDbConnection))
								{
									this.iAllPrivilegsNum = int.Parse(oleDbCommand.ExecuteScalar().ToString());
									this.strAllPrivilegsNum = "/" + this.iAllPrivilegsNum;
								}
							}
							catch (Exception)
							{
							}
							finally
							{
								if (obj != null)
								{
									((IDisposable)((IDisposable)null)).Dispose();
								}
							}
						}
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
				}
				wgAppRunInfo.raiseAppRunInfoLoadNums(this.strAllPrivilegsNum);
				this.startRecordIndex = 0;
				this.MaxRecord = 1000;
				if (!string.IsNullOrEmpty(strsql))
				{
					this.dgvSql = strsql;
				}
				this.dgvPrivileges.DataSource = null;
				if (this.bLoadAll)
				{
					this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, 100000000, this.dgvSql });
					return;
				}
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000A41A8 File Offset: 0x000A31A8
		private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000A41AC File Offset: 0x000A31AC
		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			try
			{
				bool flag = false;
				if (this.backgroundWorker1.IsBusy)
				{
					flag = true;
					this.backgroundWorker1.CancelAsync();
				}
				using (dfrmPrivilegeDoorCopy dfrmPrivilegeDoorCopy = new dfrmPrivilegeDoorCopy())
				{
					if (dfrmPrivilegeDoorCopy.ShowDialog(this) != DialogResult.OK)
					{
						if (flag)
						{
							this.userControlFind1.btnQuery.PerformClick();
						}
						return;
					}
				}
				this.userControlFind1.btnQuery.PerformClick();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x04000ADD RID: 2781
		private bool bLoadAll;

		// Token: 0x04000ADE RID: 2782
		private bool bLoadedFinished;

		// Token: 0x04000ADF RID: 2783
		private ArrayList arrController;

		// Token: 0x04000AE0 RID: 2784
		private ArrayList arrControllerAllEnabled;

		// Token: 0x04000AE1 RID: 2785
		private SqlCommand cmd;

		// Token: 0x04000AE2 RID: 2786
		private OleDbCommand cmd_Acc;

		// Token: 0x04000AE3 RID: 2787
		private SqlConnection cn;

		// Token: 0x04000AE4 RID: 2788
		private OleDbConnection cn_Acc;

		// Token: 0x04000AE5 RID: 2789
		private SqlDataAdapter da;

		// Token: 0x04000AE6 RID: 2790
		private OleDbDataAdapter da_Acc;

		// Token: 0x04000AE7 RID: 2791
		private dfrmFind dfrmFind1;

		// Token: 0x04000AE8 RID: 2792
		private DataTable dt;

		// Token: 0x04000AE9 RID: 2793
		private DataTable dtData;

		// Token: 0x04000AEA RID: 2794
		private DataView dvDoors;

		// Token: 0x04000AEB RID: 2795
		private DataView dvDoors4Watching;

		// Token: 0x04000AEC RID: 2796
		private string dgvSql;

		// Token: 0x04000AED RID: 2797
		private int iAllPrivilegsNum;

		// Token: 0x04000AEE RID: 2798
		private int recIdMax;

		// Token: 0x04000AEF RID: 2799
		private int startRecordIndex;

		// Token: 0x04000AF0 RID: 2800
		private int[] controlSegIDList = new int[256];

		// Token: 0x04000AF1 RID: 2801
		private string[] controlSegNameList = new string[256];

		// Token: 0x04000AF2 RID: 2802
		private int iSelectedControllerIndex = -1;

		// Token: 0x04000AF3 RID: 2803
		private int iSelectedControllerIndexLast = -1;

		// Token: 0x04000AF4 RID: 2804
		private int MaxRecord = 1000;

		// Token: 0x04000AF5 RID: 2805
		private string strAllPrivilegsNum = "";

		// Token: 0x04000AF6 RID: 2806
		private string strSqlAllPrivileg = " SELECT  t_d_Privilege.f_PrivilegeRecID,d.f_DoorName, c.f_ConsumerNO, c.f_ConsumerName, c.f_CardNO,  t_d_Privilege.f_ControlSegID,' ' as  f_ControlSegName, t_d_Privilege.f_ConsumerID  FROM ((t_d_Privilege  INNER JOIN t_b_Consumer c ON t_d_Privilege.f_ConsumerID=c.f_ConsumerID)   INNER JOIN t_b_Door d ON t_d_Privilege.f_DoorID=d.f_DoorID) ";
	}
}
