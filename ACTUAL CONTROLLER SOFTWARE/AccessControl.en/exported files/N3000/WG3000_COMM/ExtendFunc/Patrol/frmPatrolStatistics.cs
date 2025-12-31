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
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000310 RID: 784
	public partial class frmPatrolStatistics : frmN3000
	{
		// Token: 0x060017E1 RID: 6113 RVA: 0x001F1BC1 File Offset: 0x001F0BC1
		public frmPatrolStatistics()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x001F1C00 File Offset: 0x001F0C00
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

		// Token: 0x060017E3 RID: 6115 RVA: 0x001F1C80 File Offset: 0x001F0C80
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

		// Token: 0x060017E4 RID: 6116 RVA: 0x001F1D30 File Offset: 0x001F0D30
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x001F1D38 File Offset: 0x001F0D38
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x001F1D6A File Offset: 0x001F0D6A
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x001F1D80 File Offset: 0x001F0D80
		private void btnQuery_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			this.getLogCreateReport();
			if (!this.bLogCreateReport)
			{
				XMessageBox.Show(this, CommonStr.strCreateInAdvance, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text2 = " SELECT t_d_PatrolStatistic.f_RecID, t_b_Group.f_GroupName, ";
			text2 += "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName, ";
			if (wgAppConfig.IsAccessDB)
			{
				text2 += "IIF((IIF(ISNULL([f_TotalNormal]           ),0,[f_TotalNormal]            )) >0 ,IIF((IIF(ISNULL([f_TotalNormal]           ),0,[f_TotalNormal]            )) <1 , '0.5', CSTR(t_d_PatrolStatistic.[f_TotalNormal]            ) ) , ' ') AS  [f_TotalNormal]           ,  IIF((IIF(ISNULL([f_TotalEarly]           ),0,[f_TotalEarly]            )) >0 ,IIF((IIF(ISNULL([f_TotalEarly]           ),0,[f_TotalEarly]            )) <1 , '0.5', CSTR(t_d_PatrolStatistic.[f_TotalEarly]            ) ) , ' ') AS  [f_TotalEarly]           ,  IIF((IIF(ISNULL([f_TotalLate]           ),0,[f_TotalLate]            )) >0 ,IIF((IIF(ISNULL([f_TotalLate]           ),0,[f_TotalLate]            )) <1 , '0.5', CSTR(t_d_PatrolStatistic.[f_TotalLate]            ) ) , ' ') AS  [f_TotalLate]           ,  IIF((IIF(ISNULL([f_TotalAbsence]           ),0,[f_TotalAbsence]            )) >0 ,IIF((IIF(ISNULL([f_TotalAbsence]           ),0,[f_TotalAbsence]            )) <1 , '0.5', CSTR(t_d_PatrolStatistic.[f_TotalAbsence]            ) ) , ' ') AS  [f_TotalAbsence]           ,  ";
			}
			else
			{
				text2 += "CASE WHEN CONVERT(decimal(10,1),[f_TotalNormal]     ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_TotalNormal]     ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_TotalNormal]      ) END) ELSE ' ' END  [f_TotalNormal]     ,  CASE WHEN CONVERT(decimal(10,1),[f_TotalEarly]     ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_TotalEarly]     ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_TotalEarly]      ) END) ELSE ' ' END  [f_TotalEarly]     ,  CASE WHEN CONVERT(decimal(10,1),[f_TotalLate]     ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_TotalLate]     ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_TotalLate]      ) END) ELSE ' ' END  [f_TotalLate]     ,  CASE WHEN CONVERT(decimal(10,1),[f_TotalAbsence]     ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_TotalAbsence]     ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_TotalAbsence]      ) END) ELSE ' ' END  [f_TotalAbsence]     ,  ";
			}
			string sqlFindNormal = wgAppConfig.getSqlFindNormal(text2 + " f_PatrolDateStart,  f_PatrolDateEnd ", "t_d_PatrolStatistic", "", num, num2, num3, text, num4, num5);
			this.reloadData(sqlFindNormal);
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x001F1E4C File Offset: 0x001F0E4C
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

		// Token: 0x060017E9 RID: 6121 RVA: 0x001F1F8C File Offset: 0x001F0F8C
		private void fillDgv(DataTable dt)
		{
			try
			{
				if (this.dgvMain.DataSource == null)
				{
					this.dgvMain.DataSource = dt;
					int num = 0;
					while (num < dt.Columns.Count && num < this.dgvMain.Columns.Count)
					{
						this.dgvMain.Columns[num].DataPropertyName = dt.Columns[num].ColumnName;
						num++;
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

		// Token: 0x060017EA RID: 6122 RVA: 0x001F20FC File Offset: 0x001F10FC
		private void frmShiftAttReport_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x001F2100 File Offset: 0x001F1100
		private void frmShiftAttStatistics_Load(object sender, EventArgs e)
		{
			this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName.HeaderText);
			this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
			this.userControlFind1.btnQuery.Click += this.btnQuery_Click;
			this.userControlFind1.toolStripLabel2.Visible = false;
			this.userControlFind1.txtFindCardID.Visible = false;
			this.saveDefaultStyle();
			this.loadStyle();
			Cursor.Current = Cursors.WaitCursor;
			this.getLogCreateReport();
			this.Refresh();
			this.userControlFind1.btnQuery.PerformClick();
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x001F21B4 File Offset: 0x001F11B4
		public void getLogCreateReport()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.getLogCreateReport_Acc();
				return;
			}
			this.bLogCreateReport = false;
			string text = "SELECT * FROM  t_a_SystemParam WHERE [f_NO]=29 ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (new SqlDataAdapter(sqlCommand))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						if (sqlDataReader.Read() && wgTools.SetObjToStr(sqlDataReader["f_Notes"]) != "")
						{
							this.bLogCreateReport = true;
							this.logDateStart = DateTime.Parse(wgTools.SetObjToStr(sqlDataReader["f_Value"]).Substring(0, 10));
							this.logDateEnd = DateTime.Parse(wgTools.SetObjToStr(sqlDataReader["f_Value"]).Substring(12, 10));
							this.lblLog.Text = sqlDataReader["f_Notes"].ToString();
						}
						sqlDataReader.Close();
					}
				}
			}
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x001F22E8 File Offset: 0x001F12E8
		public void getLogCreateReport_Acc()
		{
			this.bLogCreateReport = false;
			string text = "SELECT * FROM  t_a_SystemParam WHERE [f_NO]=29 ";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					using (new OleDbDataAdapter(oleDbCommand))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						if (oleDbDataReader.Read() && wgTools.SetObjToStr(oleDbDataReader["f_Notes"]) != "")
						{
							this.bLogCreateReport = true;
							this.logDateStart = DateTime.Parse(wgTools.SetObjToStr(oleDbDataReader["f_Value"]).Substring(0, 10));
							this.logDateEnd = DateTime.Parse(wgTools.SetObjToStr(oleDbDataReader["f_Value"]).Substring(12, 10));
							this.lblLog.Text = oleDbDataReader["f_Notes"].ToString();
						}
						oleDbDataReader.Close();
					}
				}
			}
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x001F2410 File Offset: 0x001F1410
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
				strSql += string.Format(" AND t_d_PatrolStatistic.f_RecID > {0:d}", this.recIdMax);
			}
			else
			{
				strSql += string.Format(" WHERE t_d_PatrolStatistic.f_RecID > {0:d}", this.recIdMax);
			}
			strSql += " ORDER BY t_d_PatrolStatistic.f_RecID ";
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

		// Token: 0x060017EF RID: 6127 RVA: 0x001F2678 File Offset: 0x001F1678
		private void loadDefaultStyle()
		{
			DataTable dataTable = this.dsDefaultStyle.Tables[this.dgvMain.Name];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				this.dgvMain.Columns[i].Name = dataTable.Rows[i]["colName"].ToString();
				this.dgvMain.Columns[i].HeaderText = dataTable.Rows[i]["colHeader"].ToString();
				this.dgvMain.Columns[i].Width = int.Parse(dataTable.Rows[i]["colWidth"].ToString());
				this.dgvMain.Columns[i].Visible = bool.Parse(dataTable.Rows[i]["colVisable"].ToString());
				this.dgvMain.Columns[i].DisplayIndex = int.Parse(dataTable.Rows[i]["colDisplayIndex"].ToString());
			}
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x001F27C1 File Offset: 0x001F17C1
		private void loadStyle()
		{
			this.dgvMain.AutoGenerateColumns = false;
			wgAppConfig.ReadGVStyle(this, this.dgvMain);
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x001F27DC File Offset: 0x001F17DC
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

		// Token: 0x060017F2 RID: 6130 RVA: 0x001F2862 File Offset: 0x001F1862
		private void restoreDefaultLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.RestoreGVStyle(this, this.dgvMain);
			this.loadDefaultStyle();
			this.loadStyle();
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x001F287C File Offset: 0x001F187C
		private void saveDefaultStyle()
		{
			DataTable dataTable = new DataTable();
			this.dsDefaultStyle.Tables.Add(dataTable);
			dataTable.TableName = this.dgvMain.Name;
			dataTable.Columns.Add("colName");
			dataTable.Columns.Add("colHeader");
			dataTable.Columns.Add("colWidth");
			dataTable.Columns.Add("colVisable");
			dataTable.Columns.Add("colDisplayIndex");
			for (int i = 0; i < this.dgvMain.ColumnCount; i++)
			{
				DataGridViewColumn dataGridViewColumn = this.dgvMain.Columns[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["colName"] = dataGridViewColumn.Name;
				dataRow["colHeader"] = dataGridViewColumn.HeaderText;
				dataRow["colWidth"] = dataGridViewColumn.Width;
				dataRow["colVisable"] = dataGridViewColumn.Visible;
				dataRow["colDisplayIndex"] = dataGridViewColumn.DisplayIndex;
				dataTable.Rows.Add(dataRow);
				dataTable.AcceptChanges();
			}
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x001F29B1 File Offset: 0x001F19B1
		private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.SaveDGVStyle(this, this.dgvMain);
			XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
		}

		// Token: 0x04003112 RID: 12562
		private bool bLoadedFinished;

		// Token: 0x04003113 RID: 12563
		private bool bLogCreateReport;

		// Token: 0x04003114 RID: 12564
		private DateTime logDateEnd;

		// Token: 0x04003115 RID: 12565
		private DateTime logDateStart;

		// Token: 0x04003116 RID: 12566
		private int recIdMax;

		// Token: 0x04003117 RID: 12567
		private int startRecordIndex;

		// Token: 0x04003118 RID: 12568
		private DataTable table;

		// Token: 0x04003119 RID: 12569
		private string dgvSql = "";

		// Token: 0x0400311A RID: 12570
		private DataSet dsDefaultStyle = new DataSet("DGV_STILE");

		// Token: 0x0400311B RID: 12571
		private int MaxRecord = 1000;

		// Token: 0x02000311 RID: 785
		public class ToolStripDateTime : ToolStripControlHost
		{
			// Token: 0x060017F7 RID: 6135 RVA: 0x001F3385 File Offset: 0x001F2385
			public ToolStripDateTime()
				: base(frmPatrolStatistics.ToolStripDateTime.dtp = new DateTimePicker())
			{
			}

			// Token: 0x060017F8 RID: 6136 RVA: 0x001F3398 File Offset: 0x001F2398
			protected override void Dispose(bool disposing)
			{
				if (disposing && frmPatrolStatistics.ToolStripDateTime.dtp != null)
				{
					frmPatrolStatistics.ToolStripDateTime.dtp.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x060017F9 RID: 6137 RVA: 0x001F33B8 File Offset: 0x001F23B8
			public void SetTimeFormat()
			{
				DateTimePicker dateTimePicker = base.Control as DateTimePicker;
				dateTimePicker.CustomFormat = "HH;mm";
				dateTimePicker.Format = DateTimePickerFormat.Custom;
				dateTimePicker.ShowUpDown = true;
			}

			// Token: 0x170001CB RID: 459
			// (get) Token: 0x060017FA RID: 6138 RVA: 0x001F33EC File Offset: 0x001F23EC
			// (set) Token: 0x060017FB RID: 6139 RVA: 0x001F3414 File Offset: 0x001F2414
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

			// Token: 0x170001CC RID: 460
			// (get) Token: 0x060017FC RID: 6140 RVA: 0x001F3478 File Offset: 0x001F2478
			public DateTimePicker DateTimeControl
			{
				get
				{
					return base.Control as DateTimePicker;
				}
			}

			// Token: 0x170001CD RID: 461
			// (get) Token: 0x060017FD RID: 6141 RVA: 0x001F3485 File Offset: 0x001F2485
			// (set) Token: 0x060017FE RID: 6142 RVA: 0x001F3498 File Offset: 0x001F2498
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

			// Token: 0x04003134 RID: 12596
			private static DateTimePicker dtp;
		}
	}
}
