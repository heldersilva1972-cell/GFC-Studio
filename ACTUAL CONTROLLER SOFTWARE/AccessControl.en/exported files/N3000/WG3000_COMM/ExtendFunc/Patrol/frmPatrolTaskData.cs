using System;
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

namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000312 RID: 786
	public partial class frmPatrolTaskData : frmN3000
	{
		// Token: 0x060017FF RID: 6143 RVA: 0x001F34FC File Offset: 0x001F24FC
		public frmPatrolTaskData()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x001F352C File Offset: 0x001F252C
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			int num = (int)((object[])e.Argument)[0];
			int num2 = (int)((object[])e.Argument)[1];
			string text = (string)((object[])e.Argument)[2];
			e.Result = this.loadDataRecords(num, num2, text);
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x001F35AC File Offset: 0x001F25AC
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				XMessageBox.Show(CommonStr.strOperationCanceled);
				return;
			}
			if (e.Error != null)
			{
				XMessageBox.Show(string.Format("An error occurred: {0}", e.Error.Message));
				return;
			}
			if ((e.Result as DataTable).Rows.Count < this.MaxRecord)
			{
				this.bLoadedFinished = true;
			}
			this.fillDgv(e.Result as DataTable);
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvMain.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x001F365C File Offset: 0x001F265C
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmPatrolTaskAutoPlan dfrmPatrolTaskAutoPlan = new dfrmPatrolTaskAutoPlan())
			{
				dfrmPatrolTaskAutoPlan.ShowDialog();
				this.btnQuery_Click(sender, null);
			}
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x001F369C File Offset: 0x001F269C
		private void btnClear_Click(object sender, EventArgs e)
		{
			string text = string.Format("{0}", this.btnClear.Text);
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				if (wgAppConfig.IsAccessDB)
				{
					using (comPatrol_Acc comPatrol_Acc = new comPatrol_Acc())
					{
						int num = comPatrol_Acc.shift_arrange_delete(0, DateTime.Parse("2000-1-1"), DateTime.Parse("2050-12-31"));
						if (num == 0)
						{
							XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							this.btnQuery_Click(sender, null);
						}
						else
						{
							XMessageBox.Show(this, comPatrol_Acc.errDesc(num) + "\r\n\r\n" + comPatrol_Acc.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						return;
					}
				}
				using (comPatrol comPatrol = new comPatrol())
				{
					int num = comPatrol.shift_arrange_delete(0, DateTime.Parse("2000-1-1"), DateTime.Parse("2050-12-31"));
					if (num == 0)
					{
						XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						this.btnQuery_Click(sender, null);
					}
					else
					{
						XMessageBox.Show(this, comPatrol.errDesc(num) + "\r\n\r\n" + comPatrol.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x001F37F4 File Offset: 0x001F27F4
		private void btnDelete_Click(object sender, EventArgs e)
		{
			using (dfrmPatrolTaskDelete dfrmPatrolTaskDelete = new dfrmPatrolTaskDelete())
			{
				dfrmPatrolTaskDelete.ShowDialog(this);
				this.btnQuery_Click(sender, null);
			}
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x001F3834 File Offset: 0x001F2834
		private void btnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.dgvMain.SelectedCells.Count > 0 && this.dgvMain.SelectedCells[0].ColumnIndex >= 5 && this.dgvMain.SelectedCells[0].RowIndex >= 0)
				{
					DataGridViewCell dataGridViewCell = this.dgvMain.SelectedCells[0];
					DataGridViewRow dataGridViewRow = this.dgvMain.Rows[this.dgvMain.SelectedCells[0].RowIndex];
					if (dataGridViewCell.Value != DBNull.Value)
					{
						using (dfrmPatrolTaskEdit dfrmPatrolTaskEdit = new dfrmPatrolTaskEdit())
						{
							if (dfrmPatrolTaskEdit.ShowDialog() == DialogResult.OK)
							{
								int routeID = dfrmPatrolTaskEdit.routeID;
								if (wgAppConfig.IsAccessDB)
								{
									using (comPatrol_Acc comPatrol_Acc = new comPatrol_Acc())
									{
										DateTime dateTime = Convert.ToDateTime(string.Concat(new object[]
										{
											dataGridViewRow.Cells["f_DateYM"].Value,
											"-",
											dataGridViewCell.ColumnIndex - 4,
											" 12:00:00"
										}));
										long num = (long)comPatrol_Acc.shift_arrange_update(Convert.ToInt32(dataGridViewRow.Cells["f_ConsumerID"].Value), dateTime, routeID);
										if (num == 0L)
										{
											if (routeID == 0)
											{
												dataGridViewCell.Value = "*";
											}
											else
											{
												dataGridViewCell.Value = routeID;
											}
										}
										return;
									}
								}
								using (comPatrol comPatrol = new comPatrol())
								{
									DateTime dateTime2 = Convert.ToDateTime(string.Concat(new object[]
									{
										dataGridViewRow.Cells["f_DateYM"].Value,
										"-",
										dataGridViewCell.ColumnIndex - 4,
										" 12:00:00"
									}));
									long num2 = (long)comPatrol.shift_arrange_update(Convert.ToInt32(dataGridViewRow.Cells["f_ConsumerID"].Value), dateTime2, routeID);
									if (num2 == 0L)
									{
										if (routeID == 0)
										{
											dataGridViewCell.Value = "*";
										}
										else
										{
											dataGridViewCell.Value = routeID;
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x001F3AE8 File Offset: 0x001F2AE8
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x001F3AF0 File Offset: 0x001F2AF0
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x001F3B22 File Offset: 0x001F2B22
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x001F3B38 File Offset: 0x001F2B38
		private void btnQuery_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text2;
			if (wgAppConfig.IsAccessDB)
			{
				text2 = " SELECT t_d_PatrolPlanData.f_RecID, t_b_Group.f_GroupName, ";
				text2 += "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName,  t_d_PatrolPlanData.f_DateYM, ";
				for (int i = 1; i < 31; i++)
				{
					text2 += string.Format(" CSTR(t_d_PatrolPlanData.f_RouteID_{0:d2}) as f_RouteID_{0:d2}, ", i);
				}
				text2 += "CSTR(t_d_PatrolPlanData.f_RouteID_31) as f_RouteID_31  ,t_b_Consumer.f_ConsumerID  ";
			}
			else
			{
				text2 = " SELECT t_d_PatrolPlanData.f_RecID, t_b_Group.f_GroupName, ";
				text2 += "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName,  t_d_PatrolPlanData.f_DateYM, ";
				for (int j = 1; j < 31; j++)
				{
					text2 += string.Format(" CONVERT(nvarchar(3),  t_d_PatrolPlanData.f_RouteID_{0:d2}) as f_RouteID_{0:d2}, ", j);
				}
				text2 += "CONVERT(nvarchar(3),  t_d_PatrolPlanData.f_RouteID_31) as f_RouteID_31  ,t_b_Consumer.f_ConsumerID  ";
			}
			string sqlFindNormal = wgAppConfig.getSqlFindNormal(text2, "t_d_PatrolPlanData", this.getSqlOfDateTime("t_d_PatrolPlanData.f_DateYM"), num, num2, num3, text, num4, num5);
			this.reloadData(sqlFindNormal);
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x001F3C3A File Offset: 0x001F2C3A
		private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (this.btnEdit.Visible)
			{
				this.btnEdit.PerformClick();
			}
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x001F3C54 File Offset: 0x001F2C54
		private void dgvMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 5 && e.ColumnIndex < this.dgvMain.Columns.Count)
			{
				object value = e.Value;
				DataGridViewCell dataGridViewCell = this.dgvMain[e.ColumnIndex, e.RowIndex];
				string text = this.dgvMain[e.ColumnIndex, e.RowIndex].Value.ToString();
				if (!string.IsNullOrEmpty(text))
				{
					if (text == "0")
					{
						text = "*";
						e.Value = text;
						dataGridViewCell.Value = e.Value;
						return;
					}
					if (text == "-1")
					{
						e.Value = "-";
						dataGridViewCell.Value = e.Value;
						return;
					}
					if (text == "-2")
					{
						e.Value = DBNull.Value;
						dataGridViewCell.Value = e.Value;
						dataGridViewCell.ReadOnly = true;
						dataGridViewCell.Style.BackColor = SystemPens.InactiveBorder.Color;
					}
				}
			}
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x001F3D6C File Offset: 0x001F2D6C
		private void dgvMain_Scroll(object sender, ScrollEventArgs e)
		{
			if (!this.bLoadedFinished && e.ScrollOrientation == ScrollOrientation.VerticalScroll)
			{
				wgTools.WriteLine(e.OldValue.ToString());
				wgTools.WriteLine(e.NewValue.ToString());
				if (e.NewValue > e.OldValue && (e.NewValue + 100 > this.dgvMain.Rows.Count || e.NewValue + this.dgvMain.Rows.Count / 10 > this.dgvMain.Rows.Count))
				{
					if (this.startRecordIndex <= this.dgvMain.Rows.Count)
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
						wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvMain.Rows.Count.ToString() + "#");
					}
				}
			}
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x001F3EAC File Offset: 0x001F2EAC
		private void fillDgv(DataTable dt)
		{
			try
			{
				if (this.dgvMain.DataSource == null)
				{
					this.dgvMain.DataSource = dt;
					for (int i = 0; i < dt.Columns.Count; i++)
					{
						this.dgvMain.Columns[i].DataPropertyName = dt.Columns[i].ColumnName;
					}
					wgAppConfig.ReadGVStyle(this, this.dgvMain);
					if (this.startRecordIndex == 0 && dt.Rows.Count >= this.MaxRecord)
					{
						this.startRecordIndex += this.MaxRecord;
						this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
					}
				}
				else if (dt.Rows.Count > 0)
				{
					int firstDisplayedScrollingRowIndex = this.dgvMain.FirstDisplayedScrollingRowIndex;
					(this.dgvMain.DataSource as DataTable).Merge(dt);
					if (firstDisplayedScrollingRowIndex >= 0)
					{
						this.dgvMain.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x001F4008 File Offset: 0x001F3008
		private void frmShiftAttReport_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x001F400C File Offset: 0x001F300C
		private void frmShiftOtherData_Load(object sender, EventArgs e)
		{
			this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName.HeaderText);
			this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
			this.loadOperatorPrivilege();
			this.dtpDateFrom = new frmPatrolTaskData.ToolStripDateTime();
			this.dtpDateTo = new frmPatrolTaskData.ToolStripDateTime();
			this.toolStrip3.Items.Clear();
			this.toolStrip3.Items.AddRange(new ToolStripItem[] { this.toolStripLabel2, this.dtpDateFrom, this.toolStripLabel3, this.dtpDateTo });
			this.dtpDateFrom.BoxWidth = 120;
			this.dtpDateTo.BoxWidth = 120;
			this.userControlFind1.btnQuery.Click += this.btnQuery_Click;
			this.userControlFind1.toolStripLabel2.Visible = false;
			this.userControlFind1.txtFindCardID.Visible = false;
			this.dtpDateFrom.Enabled = true;
			this.dtpDateTo.Enabled = true;
			this.loadStyle();
			Cursor.Current = Cursors.WaitCursor;
			this.dtpDateTo.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-12-31"));
			this.dtpDateFrom.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01"));
			this.dtpDateFrom.BoxWidth = 150;
			this.dtpDateTo.BoxWidth = 150;
			wgAppConfig.setDisplayFormatDate(this.dtpDateFrom.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dtpDateTo.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
			this.Refresh();
			this.userControlFind1.btnQuery.PerformClick();
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x001F41DC File Offset: 0x001F31DC
		private string getSqlOfDateTime(string colNameOfDate)
		{
			string text = string.Concat(new string[]
			{
				"  (",
				colNameOfDate,
				" >= ",
				wgTools.PrepareStr(this.dtpDateFrom.Value.ToString("yyyy-MM")),
				")"
			});
			if (text != "")
			{
				text += " AND ";
			}
			string text2 = text;
			return string.Concat(new string[]
			{
				text2,
				"  (",
				colNameOfDate,
				" < ",
				wgTools.PrepareStr(this.dtpDateTo.Value.AddMonths(1).ToString("yyyy-MM")),
				")"
			});
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x001F42B4 File Offset: 0x001F32B4
		private DataTable loadDataRecords(int startIndex, int maxRecords, string strSql)
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine(this.Text + " loadDataRecords Start");
			if (strSql.ToUpper().IndexOf("SELECT ") > 0)
			{
				strSql = string.Format("SELECT TOP {0:d} ", maxRecords) + strSql.Substring(strSql.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
			}
			if (startIndex == 0)
			{
				this.recIdMax = int.MinValue;
			}
			else if (strSql.ToUpper().IndexOf(" WHERE ") > 0)
			{
				strSql += string.Format(" AND t_d_PatrolPlanData.f_RecID > {0:d}", this.recIdMax);
			}
			else
			{
				strSql += string.Format(" WHERE t_d_PatrolPlanData.f_RecID > {0:d}", this.recIdMax);
			}
			strSql += " ORDER BY t_d_PatrolPlanData.f_RecID ";
			this.table = new DataTable();
			wgTools.WriteLine("da.Fill start");
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.table);
						}
					}
					goto IL_0190;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.table);
					}
				}
			}
			IL_0190:
			if (this.table.Rows.Count > 0)
			{
				this.recIdMax = int.Parse(this.table.Rows[this.table.Rows.Count - 1][0].ToString());
			}
			wgTools.WriteLine("da.Fill End " + startIndex.ToString());
			Cursor.Current = Cursors.Default;
			wgTools.WriteLine(this.Text + "  loadRecords End");
			return this.table;
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x001F4524 File Offset: 0x001F3524
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuPatrolDetailData";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnClear.Visible = false;
				this.btnDelete.Visible = false;
				this.btnEdit.Visible = false;
			}
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x001F4576 File Offset: 0x001F3576
		private void loadStyle()
		{
			this.dgvMain.AutoGenerateColumns = false;
			wgAppConfig.ReadGVStyle(this, this.dgvMain);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x001F4590 File Offset: 0x001F3590
		private void reloadData(string strsql)
		{
			if (!this.backgroundWorker1.IsBusy)
			{
				this.bLoadedFinished = false;
				this.startRecordIndex = 0;
				this.MaxRecord = 1000;
				if (!string.IsNullOrEmpty(strsql))
				{
					this.dgvSql = strsql;
				}
				this.dgvMain.DataSource = null;
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
			}
		}

		// Token: 0x04003135 RID: 12597
		private bool bLoadedFinished;

		// Token: 0x04003136 RID: 12598
		private int recIdMax;

		// Token: 0x04003137 RID: 12599
		private int startRecordIndex;

		// Token: 0x04003138 RID: 12600
		private DataTable table;

		// Token: 0x04003139 RID: 12601
		private string dgvSql = "";

		// Token: 0x0400313A RID: 12602
		private int MaxRecord = 1000;

		// Token: 0x04003145 RID: 12613
		private frmPatrolTaskData.ToolStripDateTime dtpDateFrom;

		// Token: 0x04003146 RID: 12614
		private frmPatrolTaskData.ToolStripDateTime dtpDateTo;

		// Token: 0x02000313 RID: 787
		public class ToolStripDateTime : ToolStripControlHost
		{
			// Token: 0x06001817 RID: 6167 RVA: 0x001F577E File Offset: 0x001F477E
			public ToolStripDateTime()
				: base(frmPatrolTaskData.ToolStripDateTime.dtp = new DateTimePicker())
			{
			}

			// Token: 0x06001818 RID: 6168 RVA: 0x001F5791 File Offset: 0x001F4791
			protected override void Dispose(bool disposing)
			{
				if (disposing && frmPatrolTaskData.ToolStripDateTime.dtp != null)
				{
					frmPatrolTaskData.ToolStripDateTime.dtp.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x06001819 RID: 6169 RVA: 0x001F57B0 File Offset: 0x001F47B0
			public void SetTimeFormat()
			{
				DateTimePicker dateTimePicker = base.Control as DateTimePicker;
				dateTimePicker.CustomFormat = "HH;mm";
				dateTimePicker.Format = DateTimePickerFormat.Custom;
				dateTimePicker.ShowUpDown = true;
			}

			// Token: 0x170001CE RID: 462
			// (get) Token: 0x0600181A RID: 6170 RVA: 0x001F57E4 File Offset: 0x001F47E4
			// (set) Token: 0x0600181B RID: 6171 RVA: 0x001F580C File Offset: 0x001F480C
			public int BoxWidth
			{
				get
				{
					return (base.Control as DateTimePicker).Size.Width;
				}
				set
				{
					base.Control.Size = new Size(new Point(value, base.Control.Size.Height));
					(base.Control as DateTimePicker).Size = new Size(new Point(value, base.Control.Size.Height));
				}
			}

			// Token: 0x170001CF RID: 463
			// (get) Token: 0x0600181C RID: 6172 RVA: 0x001F5870 File Offset: 0x001F4870
			public DateTimePicker DateTimeControl
			{
				get
				{
					return base.Control as DateTimePicker;
				}
			}

			// Token: 0x170001D0 RID: 464
			// (get) Token: 0x0600181D RID: 6173 RVA: 0x001F587D File Offset: 0x001F487D
			// (set) Token: 0x0600181E RID: 6174 RVA: 0x001F5890 File Offset: 0x001F4890
			public DateTime Value
			{
				get
				{
					return (base.Control as DateTimePicker).Value;
				}
				set
				{
					DateTime dateTime;
					if (DateTime.TryParse(value.ToString(), out dateTime) && dateTime >= (base.Control as DateTimePicker).MinDate && dateTime <= (base.Control as DateTimePicker).MaxDate)
					{
						(base.Control as DateTimePicker).Value = dateTime;
					}
				}
			}

			// Token: 0x04003171 RID: 12657
			private static DateTimePicker dtp;
		}
	}
}
