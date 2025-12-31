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
	// Token: 0x02000379 RID: 889
	public partial class frmManualSwipeRecords : frmN3000
	{
		// Token: 0x06001D40 RID: 7488 RVA: 0x0026B4B4 File Offset: 0x0026A4B4
		public frmManualSwipeRecords()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x0026B4E4 File Offset: 0x0026A4E4
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

		// Token: 0x06001D42 RID: 7490 RVA: 0x0026B564 File Offset: 0x0026A564
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

		// Token: 0x06001D43 RID: 7491 RVA: 0x0026B614 File Offset: 0x0026A614
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmManualSwipeRecordsAdd dfrmManualSwipeRecordsAdd = new dfrmManualSwipeRecordsAdd())
			{
				dfrmManualSwipeRecordsAdd.ShowDialog();
				this.btnQuery_Click(sender, null);
			}
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0026B654 File Offset: 0x0026A654
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
					wgAppConfig.runUpdateSql(" DELETE FROM t_d_ManualCardRecord WHERE [f_ManualCardRecordID]= " + this.dgvMain[0, num].Value.ToString());
				}
				else
				{
					foreach (object obj in this.dgvMain.SelectedRows)
					{
						DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
						wgAppConfig.runUpdateSql(" DELETE FROM t_d_ManualCardRecord WHERE [f_ManualCardRecordID]= " + dataGridViewRow.Cells[0].Value.ToString());
					}
				}
				this.btnQuery_Click(sender, null);
			}
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x0026B7F4 File Offset: 0x0026A7F4
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
					using (dfrmManualSwipeRecordsEdit dfrmManualSwipeRecordsEdit = new dfrmManualSwipeRecordsEdit())
					{
						dfrmManualSwipeRecordsEdit.txtf_ConsumerName.Text = this.dgvMain.Rows[num].Cells[3].Value.ToString();
						dfrmManualSwipeRecordsEdit.dtpStartDate.Value = DateTime.Parse(this.dgvMain.Rows[num].Cells[4].Value.ToString());
						dfrmManualSwipeRecordsEdit.dateEndHMS2.Value = DateTime.Parse(this.dgvMain.Rows[num].Cells[4].Value.ToString());
						dfrmManualSwipeRecordsEdit.txtf_Notes.Text = this.dgvMain.Rows[num].Cells[5].Value.ToString();
						wgAppConfig.setDisplayFormatDate(dfrmManualSwipeRecordsEdit.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
						dfrmManualSwipeRecordsEdit.dateEndHMS2.CustomFormat = "HH:mm";
						dfrmManualSwipeRecordsEdit.dateEndHMS2.Format = DateTimePickerFormat.Custom;
						if (dfrmManualSwipeRecordsEdit.ShowDialog(this) == DialogResult.OK)
						{
							DataGridViewRow dataGridViewRow = this.dgvMain.Rows[num];
							wgAppConfig.runUpdateSql(string.Concat(new string[]
							{
								" UPDATE t_d_ManualCardRecord SET  f_ReadDate = ",
								wgTools.PrepareStr(dfrmManualSwipeRecordsEdit.dtpStartDate.Value.ToString("yyyy-MM-dd") + dfrmManualSwipeRecordsEdit.dateEndHMS2.Value.ToString(" HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
								" , f_Note = ",
								wgTools.PrepareStrNUnicode(dfrmManualSwipeRecordsEdit.txtf_Notes.Text),
								" WHERE [f_ManualCardRecordID]= ",
								num2.ToString()
							}));
							this.dgvMain.Rows[num].Cells[4].Value = DateTime.Parse(dfrmManualSwipeRecordsEdit.dtpStartDate.Value.ToString("yyyy-MM-dd") + dfrmManualSwipeRecordsEdit.dateEndHMS2.Value.ToString(" HH:mm:ss"));
							this.dgvMain.Rows[num].Cells[5].Value = dfrmManualSwipeRecordsEdit.txtf_Notes.Text;
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

		// Token: 0x06001D46 RID: 7494 RVA: 0x0026BB48 File Offset: 0x0026AB48
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x0026BB7A File Offset: 0x0026AB7A
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvMain);
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x0026BB8E File Offset: 0x0026AB8E
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x0026BBA4 File Offset: 0x0026ABA4
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
			string sqlFindNormal = wgAppConfig.getSqlFindNormal(" SELECT t_d_ManualCardRecord.f_ManualCardRecordID, t_b_Group.f_GroupName,        t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName, f_ReadDate, t_d_ManualCardRecord.f_Note,  t_b_Consumer.f_ConsumerID  ", "t_d_ManualCardRecord", this.getSqlOfDateTime("t_d_ManualCardRecord.f_ReadDate"), num, num2, num3, text, num4, num5);
			this.reloadData(sqlFindNormal);
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x0026BC24 File Offset: 0x0026AC24
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
			string sqlFindNormal = wgAppConfig.getSqlFindNormal(" SELECT t_d_ManualCardRecord.f_ManualCardRecordID, t_b_Group.f_GroupName,        t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName, f_ReadDate, t_d_ManualCardRecord.f_Note,  t_b_Consumer.f_ConsumerID  ", "t_d_ManualCardRecord", this.getSqlOfDateTime("t_d_ManualCardRecord.f_ReadDate"), num, num2, num3, text, num4, num5);
			this.reloadData(sqlFindNormal);
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x0026BC94 File Offset: 0x0026AC94
		private void btnTypeSetup_Click(object sender, EventArgs e)
		{
			using (dfrmHolidayType dfrmHolidayType = new dfrmHolidayType())
			{
				dfrmHolidayType.ShowDialog(this);
				this.btnQuery_Click(sender, null);
			}
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x0026BCD4 File Offset: 0x0026ACD4
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

		// Token: 0x06001D4D RID: 7501 RVA: 0x0026BDD0 File Offset: 0x0026ADD0
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

		// Token: 0x06001D4E RID: 7502 RVA: 0x0026BF10 File Offset: 0x0026AF10
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
					wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_ReadDate", wgTools.DisplayFormat_DateYMDHMSWeek);
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

		// Token: 0x06001D4F RID: 7503 RVA: 0x0026C0A8 File Offset: 0x0026B0A8
		private void frmShiftAttReport_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x0026C0AC File Offset: 0x0026B0AC
		private void frmSwipeRecords_Load(object sender, EventArgs e)
		{
			this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName.HeaderText);
			this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
			this.loadOperatorPrivilege();
			this.dtpDateFrom = new frmManualSwipeRecords.ToolStripDateTime();
			this.dtpDateTo = new frmManualSwipeRecords.ToolStripDateTime();
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

		// Token: 0x06001D51 RID: 7505 RVA: 0x0026C27C File Offset: 0x0026B27C
		private string getSqlOfDateTime(string colNameOfDate)
		{
			string text = string.Concat(new string[]
			{
				"  (",
				colNameOfDate,
				" >= ",
				wgTools.PrepareStr(this.dtpDateFrom.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 00:00:00"),
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
				" <= ",
				wgTools.PrepareStr(this.dtpDateTo.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59"),
				")"
			});
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0026C358 File Offset: 0x0026B358
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
				strSql += string.Format(" AND f_ManualCardRecordID > {0:d}", this.recIdMax);
			}
			else
			{
				strSql += string.Format(" WHERE f_ManualCardRecordID > {0:d}", this.recIdMax);
			}
			strSql += " ORDER BY f_ManualCardRecordID ";
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

		// Token: 0x06001D53 RID: 7507 RVA: 0x0026C5C0 File Offset: 0x0026B5C0
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuManualCardRecord";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnDelete.Visible = false;
				this.btnEdit.Visible = false;
			}
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x0026C606 File Offset: 0x0026B606
		private void loadStyle()
		{
			this.dgvMain.AutoGenerateColumns = false;
			wgAppConfig.ReadGVStyle(this, this.dgvMain);
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x0026C620 File Offset: 0x0026B620
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

		// Token: 0x04003843 RID: 14403
		private bool bLoadedFinished;

		// Token: 0x04003844 RID: 14404
		private int recIdMax;

		// Token: 0x04003845 RID: 14405
		private int startRecordIndex;

		// Token: 0x04003846 RID: 14406
		private DataTable table;

		// Token: 0x04003847 RID: 14407
		private string dgvSql = "";

		// Token: 0x04003848 RID: 14408
		private int MaxRecord = 1000;

		// Token: 0x04003852 RID: 14418
		private frmManualSwipeRecords.ToolStripDateTime dtpDateFrom;

		// Token: 0x04003853 RID: 14419
		private frmManualSwipeRecords.ToolStripDateTime dtpDateTo;

		// Token: 0x0200037A RID: 890
		public class ToolStripDateTime : ToolStripControlHost
		{
			// Token: 0x06001D58 RID: 7512 RVA: 0x0026CF5F File Offset: 0x0026BF5F
			public ToolStripDateTime()
				: base(frmManualSwipeRecords.ToolStripDateTime.dtp = new DateTimePicker())
			{
			}

			// Token: 0x06001D59 RID: 7513 RVA: 0x0026CF72 File Offset: 0x0026BF72
			protected override void Dispose(bool disposing)
			{
				if (disposing && frmManualSwipeRecords.ToolStripDateTime.dtp != null)
				{
					frmManualSwipeRecords.ToolStripDateTime.dtp.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x06001D5A RID: 7514 RVA: 0x0026CF90 File Offset: 0x0026BF90
			public void SetTimeFormat()
			{
				DateTimePicker dateTimePicker = base.Control as DateTimePicker;
				dateTimePicker.CustomFormat = "HH;mm";
				dateTimePicker.Format = DateTimePickerFormat.Custom;
				dateTimePicker.ShowUpDown = true;
			}

			// Token: 0x1700026C RID: 620
			// (get) Token: 0x06001D5B RID: 7515 RVA: 0x0026CFC4 File Offset: 0x0026BFC4
			// (set) Token: 0x06001D5C RID: 7516 RVA: 0x0026CFEC File Offset: 0x0026BFEC
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

			// Token: 0x1700026D RID: 621
			// (get) Token: 0x06001D5D RID: 7517 RVA: 0x0026D050 File Offset: 0x0026C050
			public DateTimePicker DateTimeControl
			{
				get
				{
					return base.Control as DateTimePicker;
				}
			}

			// Token: 0x1700026E RID: 622
			// (get) Token: 0x06001D5E RID: 7518 RVA: 0x0026D05D File Offset: 0x0026C05D
			// (set) Token: 0x06001D5F RID: 7519 RVA: 0x0026D070 File Offset: 0x0026C070
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

			// Token: 0x0400385F RID: 14431
			private static DateTimePicker dtp;
		}
	}
}
