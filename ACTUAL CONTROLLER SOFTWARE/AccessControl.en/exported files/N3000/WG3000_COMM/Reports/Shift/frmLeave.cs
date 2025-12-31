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

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000377 RID: 887
	public partial class frmLeave : frmN3000
	{
		// Token: 0x06001D20 RID: 7456 RVA: 0x002695D1 File Offset: 0x002685D1
		public frmLeave()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x00269600 File Offset: 0x00268600
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

		// Token: 0x06001D22 RID: 7458 RVA: 0x00269680 File Offset: 0x00268680
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

		// Token: 0x06001D23 RID: 7459 RVA: 0x00269730 File Offset: 0x00268730
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmLeaveAdd dfrmLeaveAdd = new dfrmLeaveAdd())
			{
				dfrmLeaveAdd.ShowDialog();
				this.btnQuery_Click(sender, null);
			}
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x00269770 File Offset: 0x00268770
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (this.dgvMain.RowCount > 0)
			{
				if (this.dgvMain.SelectedRows.Count <= 1)
				{
					int num = this.dgvMain.SelectedRows[0].Index;
					if (XMessageBox.Show(this, CommonStr.strDelete + " " + this.dgvMain[0, num].Value.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
					{
						return;
					}
				}
				else if (XMessageBox.Show(this, CommonStr.strDeleteSelected + " " + this.dgvMain.SelectedRows.Count.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
				{
					return;
				}
				int firstDisplayedScrollingRowIndex = this.dgvMain.FirstDisplayedScrollingRowIndex;
				if (this.dgvMain.SelectedRows.Count <= 1)
				{
					int num = this.dgvMain.SelectedRows[0].Index;
					wgAppConfig.runUpdateSql(" DELETE FROM t_d_Leave WHERE [f_NO]= " + this.dgvMain[0, num].Value.ToString());
				}
				else
				{
					foreach (object obj in this.dgvMain.SelectedRows)
					{
						DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
						wgAppConfig.runUpdateSql(" DELETE FROM t_d_Leave WHERE [f_NO]= " + dataGridViewRow.Cells[0].Value.ToString());
					}
				}
				this.btnQuery_Click(sender, null);
			}
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x00269910 File Offset: 0x00268910
		private void btnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvMain.SelectedRows.Count <= 0)
				{
					if (this.dgvMain.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvMain.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvMain.SelectedRows[0].Index;
				}
				int num2 = int.Parse(this.dgvMain.Rows[num].Cells[0].Value.ToString());
				if (num2 > 0)
				{
					using (dfrmLeaveEdit dfrmLeaveEdit = new dfrmLeaveEdit())
					{
						dfrmLeaveEdit.txtf_ConsumerName.Text = this.dgvMain.Rows[num].Cells[3].Value.ToString();
						dfrmLeaveEdit.dtpStartDate.Value = DateTime.Parse(this.dgvMain.Rows[num].Cells[4].Value.ToString());
						dfrmLeaveEdit.cboStart.Text = this.dgvMain.Rows[num].Cells[5].Value.ToString();
						dfrmLeaveEdit.dtpEndDate.Value = DateTime.Parse(this.dgvMain.Rows[num].Cells[6].Value.ToString());
						dfrmLeaveEdit.cboEnd.Text = this.dgvMain.Rows[num].Cells[7].Value.ToString();
						dfrmLeaveEdit.cboHolidayType.Text = this.dgvMain.Rows[num].Cells[8].Value.ToString();
						dfrmLeaveEdit.txtf_Notes.Text = this.dgvMain.Rows[num].Cells[9].Value.ToString();
						wgAppConfig.setDisplayFormatDate(dfrmLeaveEdit.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
						wgAppConfig.setDisplayFormatDate(dfrmLeaveEdit.dtpEndDate, wgTools.DisplayFormat_DateYMDWeek);
						if (dfrmLeaveEdit.ShowDialog(this) == DialogResult.OK)
						{
							DataGridViewRow dataGridViewRow = this.dgvMain.Rows[num];
							wgAppConfig.runUpdateSql(string.Concat(new string[]
							{
								" UPDATE t_d_Leave SET  f_Value = ",
								wgTools.PrepareStrNUnicode(dfrmLeaveEdit.dtpStartDate.Value.ToString("yyyy-MM-dd")),
								", f_Value1 = ",
								wgTools.PrepareStrNUnicode(dfrmLeaveEdit.cboStart.Text),
								", [f_Value2] =",
								wgTools.PrepareStrNUnicode(dfrmLeaveEdit.dtpEndDate.Value.ToString("yyyy-MM-dd")),
								",   [f_Value3] = ",
								wgTools.PrepareStrNUnicode(dfrmLeaveEdit.cboEnd.Text),
								" ,  [f_HolidayType] = ",
								wgTools.PrepareStrNUnicode(dfrmLeaveEdit.cboHolidayType.Text),
								" ,  [f_Notes] = ",
								wgTools.PrepareStrNUnicode(dfrmLeaveEdit.txtf_Notes.Text),
								"   WHERE [f_NO]= ",
								num2.ToString()
							}));
							this.dgvMain.Rows[num].Cells[4].Value = dfrmLeaveEdit.dtpStartDate.Value;
							this.dgvMain.Rows[num].Cells[5].Value = dfrmLeaveEdit.cboStart.Text;
							this.dgvMain.Rows[num].Cells[6].Value = dfrmLeaveEdit.dtpEndDate.Value;
							this.dgvMain.Rows[num].Cells[7].Value = dfrmLeaveEdit.cboEnd.Text;
							this.dgvMain.Rows[num].Cells[8].Value = dfrmLeaveEdit.cboHolidayType.Text;
							this.dgvMain.Rows[num].Cells[9].Value = dfrmLeaveEdit.txtf_Notes.Text;
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x00269DD4 File Offset: 0x00268DD4
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x00269E06 File Offset: 0x00268E06
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvMain);
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00269E1A File Offset: 0x00268E1A
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x00269E30 File Offset: 0x00268E30
		private void btnQuery_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnQuery_Click_Acc(sender, e);
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string sqlFindNormal = wgAppConfig.getSqlFindNormal(" SELECT t_d_Leave.f_NO, t_b_Group.f_GroupName,        t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName,  CONVERT(DateTime , [f_Value]) AS f_from,  [f_Value1] AS f_from1,  CONVERT(DateTime , [f_Value2]) AS f_to,  [f_Value3] AS f_to1,  t_d_Leave.[f_HolidayType],  t_d_Leave.f_Notes,  t_b_Consumer.f_ConsumerID  ", "t_d_Leave", this.getSqlOfDateTime("t_d_Leave.f_DateYM"), num, num2, num3, text, num4, num5);
			this.reloadData(sqlFindNormal);
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x00269EB0 File Offset: 0x00268EB0
		private void btnQuery_Click_Acc(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string sqlFindNormal = wgAppConfig.getSqlFindNormal(" SELECT t_d_Leave.f_NO, t_b_Group.f_GroupName,        t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName,  CDate([f_Value]) AS f_from,  [f_Value1] AS f_from1,  CDate([f_Value2]) AS f_to,  [f_Value3] AS f_to1,  t_d_Leave.[f_HolidayType],  t_d_Leave.f_Notes,  t_b_Consumer.f_ConsumerID  ", "t_d_Leave", this.getSqlOfDateTime("t_d_Leave.f_DateYM"), num, num2, num3, text, num4, num5);
			this.reloadData(sqlFindNormal);
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x00269F20 File Offset: 0x00268F20
		private void btnTypeSetup_Click(object sender, EventArgs e)
		{
			using (dfrmHolidayType dfrmHolidayType = new dfrmHolidayType())
			{
				dfrmHolidayType.ShowDialog(this);
				this.btnQuery_Click(sender, null);
			}
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x00269F60 File Offset: 0x00268F60
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
					}
				}
			}
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x0026A05C File Offset: 0x0026905C
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

		// Token: 0x06001D2E RID: 7470 RVA: 0x0026A19C File Offset: 0x0026919C
		private void fillDgv(DataTable dt)
		{
			try
			{
				if (this.dgvMain.DataSource == null)
				{
					this.dgvMain.DataSource = dt;
					for (int i = 0; i < this.dgvMain.ColumnCount; i++)
					{
						this.dgvMain.Columns[i].DataPropertyName = dt.Columns[i].ColumnName;
						this.dgvMain.Columns[i].Name = dt.Columns[i].ColumnName;
					}
					wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_From", wgTools.DisplayFormat_DateYMDWeek);
					wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_To", wgTools.DisplayFormat_DateYMDWeek);
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

		// Token: 0x06001D2F RID: 7471 RVA: 0x0026A34C File Offset: 0x0026934C
		private void frmLeave_Load(object sender, EventArgs e)
		{
			this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName.HeaderText);
			this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
			this.loadOperatorPrivilege();
			this.dtpDateFrom = new frmLeave.ToolStripDateTime();
			this.dtpDateTo = new frmLeave.ToolStripDateTime();
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

		// Token: 0x06001D30 RID: 7472 RVA: 0x0026A51B File Offset: 0x0026951B
		private void frmShiftAttReport_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x0026A520 File Offset: 0x00269520
		private string getSqlOfDateTime(string colNameOfDate)
		{
			string text = "  ([f_Value2]>= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString("yyyy-MM-dd")) + ")";
			if (text != "")
			{
				text += " AND ";
			}
			return text + "  ( [f_Value] <= " + wgTools.PrepareStr(this.dtpDateTo.Value.ToString("yyyy-MM-dd  23:59:59")) + ")";
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x0026A5A8 File Offset: 0x002695A8
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
				strSql += string.Format(" AND t_d_Leave.f_NO > {0:d}", this.recIdMax);
			}
			else
			{
				strSql += string.Format(" WHERE t_d_Leave.f_NO > {0:d}", this.recIdMax);
			}
			strSql += " ORDER BY f_NO ";
			this.table = new DataTable();
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
					goto IL_0186;
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
			IL_0186:
			if (this.table.Rows.Count > 0)
			{
				this.recIdMax = int.Parse(this.table.Rows[this.table.Rows.Count - 1][0].ToString());
			}
			wgTools.WriteLine("da.Fill End " + startIndex.ToString());
			Cursor.Current = Cursors.Default;
			wgTools.WriteLine(this.Text + "  loadRecords End");
			return this.table;
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x0026A810 File Offset: 0x00269810
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuLeave";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnDelete.Visible = false;
				this.btnEdit.Visible = false;
				this.btnTypeSetup.Visible = false;
			}
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x0026A862 File Offset: 0x00269862
		private void loadStyle()
		{
			this.dgvMain.AutoGenerateColumns = false;
			wgAppConfig.ReadGVStyle(this, this.dgvMain);
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x0026A87C File Offset: 0x0026987C
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

		// Token: 0x04003821 RID: 14369
		private bool bLoadedFinished;

		// Token: 0x04003822 RID: 14370
		private int recIdMax;

		// Token: 0x04003823 RID: 14371
		private int startRecordIndex;

		// Token: 0x04003824 RID: 14372
		private DataTable table;

		// Token: 0x04003825 RID: 14373
		private string dgvSql = "";

		// Token: 0x04003826 RID: 14374
		private int MaxRecord = 1000;

		// Token: 0x04003831 RID: 14385
		private frmLeave.ToolStripDateTime dtpDateFrom;

		// Token: 0x04003832 RID: 14386
		private frmLeave.ToolStripDateTime dtpDateTo;

		// Token: 0x02000378 RID: 888
		public class ToolStripDateTime : ToolStripControlHost
		{
			// Token: 0x06001D38 RID: 7480 RVA: 0x0026B33E File Offset: 0x0026A33E
			public ToolStripDateTime()
				: base(frmLeave.ToolStripDateTime.dtp = new DateTimePicker())
			{
			}

			// Token: 0x06001D39 RID: 7481 RVA: 0x0026B351 File Offset: 0x0026A351
			protected override void Dispose(bool disposing)
			{
				if (disposing && frmLeave.ToolStripDateTime.dtp != null)
				{
					frmLeave.ToolStripDateTime.dtp.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x06001D3A RID: 7482 RVA: 0x0026B370 File Offset: 0x0026A370
			public void SetTimeFormat()
			{
				DateTimePicker dateTimePicker = base.Control as DateTimePicker;
				dateTimePicker.CustomFormat = "HH;mm";
				dateTimePicker.Format = DateTimePickerFormat.Custom;
				dateTimePicker.ShowUpDown = true;
			}

			// Token: 0x17000269 RID: 617
			// (get) Token: 0x06001D3B RID: 7483 RVA: 0x0026B3A4 File Offset: 0x0026A3A4
			// (set) Token: 0x06001D3C RID: 7484 RVA: 0x0026B3CC File Offset: 0x0026A3CC
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

			// Token: 0x1700026A RID: 618
			// (get) Token: 0x06001D3D RID: 7485 RVA: 0x0026B430 File Offset: 0x0026A430
			public DateTimePicker DateTimeControl
			{
				get
				{
					return base.Control as DateTimePicker;
				}
			}

			// Token: 0x1700026B RID: 619
			// (get) Token: 0x06001D3E RID: 7486 RVA: 0x0026B43D File Offset: 0x0026A43D
			// (set) Token: 0x06001D3F RID: 7487 RVA: 0x0026B450 File Offset: 0x0026A450
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

			// Token: 0x04003842 RID: 14402
			private static DateTimePicker dtp;
		}
	}
}
