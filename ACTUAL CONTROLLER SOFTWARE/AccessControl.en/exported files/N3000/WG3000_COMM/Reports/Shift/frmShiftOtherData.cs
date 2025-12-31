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
	// Token: 0x0200037F RID: 895
	public partial class frmShiftOtherData : frmN3000
	{
		// Token: 0x06001DBF RID: 7615 RVA: 0x002751B8 File Offset: 0x002741B8
		public frmShiftOtherData()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x002751E8 File Offset: 0x002741E8
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

		// Token: 0x06001DC1 RID: 7617 RVA: 0x00275268 File Offset: 0x00274268
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

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00275318 File Offset: 0x00274318
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmAutoShift dfrmAutoShift = new dfrmAutoShift())
			{
				dfrmAutoShift.ShowDialog();
				this.btnQuery_Click(sender, null);
			}
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00275358 File Offset: 0x00274358
		private void btnClear_Click(object sender, EventArgs e)
		{
			string text = string.Format("{0}", this.btnClear.Text);
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				if (wgAppConfig.IsAccessDB)
				{
					using (comShift_Acc comShift_Acc = new comShift_Acc())
					{
						int num = comShift_Acc.shift_arrange_delete(0, DateTime.Parse("2000-1-1"), DateTime.Parse("2050-12-31"));
						if (num == 0)
						{
							XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							this.btnQuery_Click(sender, null);
						}
						else
						{
							XMessageBox.Show(this, comShift_Acc.errDesc(num) + "\r\n\r\n" + comShift_Acc.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						return;
					}
				}
				using (comShift comShift = new comShift())
				{
					int num = comShift.shift_arrange_delete(0, DateTime.Parse("2000-1-1"), DateTime.Parse("2050-12-31"));
					if (num == 0)
					{
						XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						this.btnQuery_Click(sender, null);
					}
					else
					{
						XMessageBox.Show(this, comShift.errDesc(num) + "\r\n\r\n" + comShift.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x002754B0 File Offset: 0x002744B0
		private void btnDelete_Click(object sender, EventArgs e)
		{
			using (dfrmShiftDelete dfrmShiftDelete = new dfrmShiftDelete())
			{
				dfrmShiftDelete.ShowDialog(this);
				this.btnQuery_Click(sender, null);
			}
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x002754F0 File Offset: 0x002744F0
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
						using (dfrmShiftEdit dfrmShiftEdit = new dfrmShiftEdit())
						{
							if (dfrmShiftEdit.ShowDialog() == DialogResult.OK)
							{
								int shiftid = dfrmShiftEdit.shiftid;
								if (wgAppConfig.IsAccessDB)
								{
									using (comShift_Acc comShift_Acc = new comShift_Acc())
									{
										DateTime dateTime = Convert.ToDateTime(string.Concat(new object[]
										{
											dataGridViewRow.Cells["f_DateYM"].Value,
											"-",
											dataGridViewCell.ColumnIndex - 4,
											" 12:00:00"
										}));
										long num = (long)comShift_Acc.shift_arrange_update(Convert.ToInt32(dataGridViewRow.Cells["f_ConsumerID"].Value), dateTime, shiftid);
										if (num == 0L)
										{
											if (shiftid == 0)
											{
												dataGridViewCell.Value = "*";
											}
											else
											{
												dataGridViewCell.Value = shiftid;
											}
										}
										return;
									}
								}
								using (comShift comShift = new comShift())
								{
									DateTime dateTime2 = Convert.ToDateTime(string.Concat(new object[]
									{
										dataGridViewRow.Cells["f_DateYM"].Value,
										"-",
										dataGridViewCell.ColumnIndex - 4,
										" 12:00:00"
									}));
									long num2 = (long)comShift.shift_arrange_update(Convert.ToInt32(dataGridViewRow.Cells["f_ConsumerID"].Value), dateTime2, shiftid);
									if (num2 == 0L)
									{
										if (shiftid == 0)
										{
											dataGridViewCell.Value = "*";
										}
										else
										{
											dataGridViewCell.Value = shiftid;
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

		// Token: 0x06001DC6 RID: 7622 RVA: 0x002757A4 File Offset: 0x002747A4
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x002757D6 File Offset: 0x002747D6
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvMain);
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x002757EA File Offset: 0x002747EA
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x00275800 File Offset: 0x00274800
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
				text2 = " SELECT t_d_ShiftData.f_RecID, t_b_Group.f_GroupName, ";
				text2 += "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName,  t_d_ShiftData.f_DateYM, ";
				for (int i = 1; i < 31; i++)
				{
					text2 += string.Format(" CSTR(t_d_ShiftData.f_ShiftID_{0:d2}) as f_ShiftID_{0:d2}, ", i);
				}
				text2 += "CSTR(t_d_ShiftData.f_ShiftID_31) as f_ShiftID_31  ,t_b_Consumer.f_ConsumerID  ";
			}
			else
			{
				text2 = " SELECT t_d_ShiftData.f_RecID, t_b_Group.f_GroupName, ";
				text2 += "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName,  t_d_ShiftData.f_DateYM, ";
				for (int j = 1; j < 31; j++)
				{
					text2 += string.Format(" CONVERT(nvarchar(3),  t_d_ShiftData.f_ShiftID_{0:d2}) as f_ShiftID_{0:d2}, ", j);
				}
				text2 += "CONVERT(nvarchar(3),  t_d_ShiftData.f_ShiftID_31) as f_ShiftID_31  ,t_b_Consumer.f_ConsumerID  ";
			}
			string sqlFindNormal = wgAppConfig.getSqlFindNormal(text2, "t_d_ShiftData", this.getSqlOfDateTime("t_d_ShiftData.f_DateYM"), num, num2, num3, text, num4, num5);
			this.reloadData(sqlFindNormal);
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x00275902 File Offset: 0x00274902
		private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (this.btnEdit.Visible)
			{
				this.btnEdit.PerformClick();
			}
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x0027591C File Offset: 0x0027491C
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

		// Token: 0x06001DCC RID: 7628 RVA: 0x00275A34 File Offset: 0x00274A34
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

		// Token: 0x06001DCD RID: 7629 RVA: 0x00275B74 File Offset: 0x00274B74
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

		// Token: 0x06001DCE RID: 7630 RVA: 0x00275CD0 File Offset: 0x00274CD0
		private void frmShiftAttReport_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x00275CD4 File Offset: 0x00274CD4
		private void frmShiftOtherData_Load(object sender, EventArgs e)
		{
			this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName.HeaderText);
			this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
			this.loadOperatorPrivilege();
			this.dtpDateFrom = new frmShiftOtherData.ToolStripDateTime();
			this.dtpDateTo = new frmShiftOtherData.ToolStripDateTime();
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
			if (!string.IsNullOrEmpty(this.userControlFind1.cboFindDept.Text))
			{
				this.btnClear.Enabled = false;
			}
			this.Refresh();
			this.userControlFind1.btnQuery.PerformClick();
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x00275EC8 File Offset: 0x00274EC8
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

		// Token: 0x06001DD1 RID: 7633 RVA: 0x00275FA0 File Offset: 0x00274FA0
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
				strSql += string.Format(" AND t_d_ShiftData.f_RecID > {0:d}", this.recIdMax);
			}
			else
			{
				strSql += string.Format(" WHERE t_d_ShiftData.f_RecID > {0:d}", this.recIdMax);
			}
			strSql += " ORDER BY t_d_ShiftData.f_RecID ";
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

		// Token: 0x06001DD2 RID: 7634 RVA: 0x00276210 File Offset: 0x00275210
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuShiftArrange";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnClear.Visible = false;
				this.btnDelete.Visible = false;
				this.btnEdit.Visible = false;
			}
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x00276262 File Offset: 0x00275262
		private void loadStyle()
		{
			this.dgvMain.AutoGenerateColumns = false;
			wgAppConfig.ReadGVStyle(this, this.dgvMain);
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x0027627C File Offset: 0x0027527C
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

		// Token: 0x040038DF RID: 14559
		private bool bLoadedFinished;

		// Token: 0x040038E0 RID: 14560
		private int recIdMax;

		// Token: 0x040038E1 RID: 14561
		private int startRecordIndex;

		// Token: 0x040038E2 RID: 14562
		private DataTable table;

		// Token: 0x040038E3 RID: 14563
		private string dgvSql = "";

		// Token: 0x040038E4 RID: 14564
		private int MaxRecord = 1000;

		// Token: 0x040038EF RID: 14575
		private frmShiftOtherData.ToolStripDateTime dtpDateFrom;

		// Token: 0x040038F0 RID: 14576
		private frmShiftOtherData.ToolStripDateTime dtpDateTo;

		// Token: 0x02000380 RID: 896
		public class ToolStripDateTime : ToolStripControlHost
		{
			// Token: 0x06001DD7 RID: 7639 RVA: 0x0027746A File Offset: 0x0027646A
			public ToolStripDateTime()
				: base(frmShiftOtherData.ToolStripDateTime.dtp = new DateTimePicker())
			{
			}

			// Token: 0x06001DD8 RID: 7640 RVA: 0x0027747D File Offset: 0x0027647D
			protected override void Dispose(bool disposing)
			{
				if (disposing && frmShiftOtherData.ToolStripDateTime.dtp != null)
				{
					frmShiftOtherData.ToolStripDateTime.dtp.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x06001DD9 RID: 7641 RVA: 0x0027749C File Offset: 0x0027649C
			public void SetTimeFormat()
			{
				DateTimePicker dateTimePicker = base.Control as DateTimePicker;
				dateTimePicker.CustomFormat = "HH;mm";
				dateTimePicker.Format = DateTimePickerFormat.Custom;
				dateTimePicker.ShowUpDown = true;
			}

			// Token: 0x17000275 RID: 629
			// (get) Token: 0x06001DDA RID: 7642 RVA: 0x002774D0 File Offset: 0x002764D0
			// (set) Token: 0x06001DDB RID: 7643 RVA: 0x002774F8 File Offset: 0x002764F8
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

			// Token: 0x17000276 RID: 630
			// (get) Token: 0x06001DDC RID: 7644 RVA: 0x0027755C File Offset: 0x0027655C
			public DateTimePicker DateTimeControl
			{
				get
				{
					return base.Control as DateTimePicker;
				}
			}

			// Token: 0x17000277 RID: 631
			// (get) Token: 0x06001DDD RID: 7645 RVA: 0x00277569 File Offset: 0x00276569
			// (set) Token: 0x06001DDE RID: 7646 RVA: 0x0027757C File Offset: 0x0027657C
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

			// Token: 0x0400391B RID: 14619
			private static DateTimePicker dtp;
		}
	}
}
