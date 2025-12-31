using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000019 RID: 25
	public partial class dfrmLogQuery : frmN3000
	{
		// Token: 0x06000128 RID: 296 RVA: 0x00028AA3 File Offset: 0x00027AA3
		public dfrmLogQuery()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00028AE0 File Offset: 0x00027AE0
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

		// Token: 0x0600012A RID: 298 RVA: 0x00028B60 File Offset: 0x00027B60
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

		// Token: 0x0600012B RID: 299 RVA: 0x00028C10 File Offset: 0x00027C10
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00028C18 File Offset: 0x00027C18
		private void btnExportExcel_Click(object sender, EventArgs e)
		{
			try
			{
				ExcelObject.bOnly300ExcelData = true;
				wgAppConfig.exportToExcel(this.dgvMain, this.Text);
				ExcelObject.bOnly300ExcelData = false;
			}
			catch (Exception)
			{
			}
			finally
			{
				ExcelObject.bOnly300ExcelData = false;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00028C70 File Offset: 0x00027C70
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvMain);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00028C84 File Offset: 0x00027C84
		private void button1_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00028C97 File Offset: 0x00027C97
		private void dfrmLogQuery_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00028CAC File Offset: 0x00027CAC
		private void dfrmLogQuery_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					wgAppConfig.findCall(this.dfrmFind1, this, this.dgvMain);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00028D08 File Offset: 0x00027D08
		private void dfrmLogQuery_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			string text = " SELECT f_RecID,f_LogDateTime,  f_EventType, f_EventDesc From t_s_wgLog  ";
			if (!string.IsNullOrEmpty(this.specialLog))
			{
				this.dgvMain.Columns[2].Width = 150;
				text = " SELECT f_RecID,f_LogDateTime,  f_EventType, f_EventDesc From   " + this.specialLog;
			}
			this.dgvMain.AutoGenerateColumns = false;
			this.reloadData(text);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00028D6E File Offset: 0x00027D6E
		private void dgvMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00028D70 File Offset: 0x00027D70
		private void dgvMain_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00028D74 File Offset: 0x00027D74
		private void dgvMain_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if (this.dgvMain.SelectedRows.Count <= 0)
				{
					if (this.dgvMain.SelectedCells.Count <= 0)
					{
						return;
					}
					int rowIndex = this.dgvMain.SelectedCells[0].RowIndex;
				}
				else
				{
					int index = this.dgvMain.SelectedRows[0].Index;
				}
				int num = 0;
				DataGridView dataGridView = this.dgvMain;
				if (dataGridView.Rows.Count > 0)
				{
					try
					{
						num = dataGridView.CurrentCell.RowIndex;
					}
					catch
					{
					}
				}
				string text = dataGridView.Rows[num].Cells["f_EventDesc"].Value.ToString();
				try
				{
					Clipboard.SetText(text);
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					wgAppConfig.wgLogWithoutDB(text, EventLogEntryType.Information, null);
				}
				XMessageBox.Show(text, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00028E94 File Offset: 0x00027E94
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

		// Token: 0x06000136 RID: 310 RVA: 0x00028FD4 File Offset: 0x00027FD4
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
					wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_LogDateTime", wgTools.DisplayFormat_DateYMDHMSWeek);
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

		// Token: 0x06000137 RID: 311 RVA: 0x0002916C File Offset: 0x0002816C
		private DataTable loadDataRecords(int startIndex, int maxRecords, string strSql)
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("load LogQuery Start");
			if (strSql.ToUpper().IndexOf("SELECT ") > 0)
			{
				strSql = string.Format("SELECT TOP {0:d} ", maxRecords) + strSql.Substring(strSql.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
			}
			if (startIndex == 0)
			{
				this.recIdMin = int.MaxValue;
			}
			else if (strSql.ToUpper().IndexOf(" WHERE ") > 0)
			{
				strSql += string.Format(" AND f_RecID < {0:d}", this.recIdMin);
			}
			else
			{
				strSql += string.Format(" WHERE f_RecID < {0:d}", this.recIdMin);
			}
			strSql += " ORDER BY f_RecID DESC ";
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							this.dt = new DataTable();
							wgTools.WriteLine("da.Fill start");
							oleDbDataAdapter.Fill(this.dt);
							if (this.dt.Rows.Count > 0)
							{
								this.recIdMin = int.Parse(this.dt.Rows[this.dt.Rows.Count - 1][0].ToString());
							}
						}
					}
					goto IL_0236;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						this.dt = new DataTable();
						wgTools.WriteLine("da.Fill start");
						sqlDataAdapter.Fill(this.dt);
						if (this.dt.Rows.Count > 0)
						{
							this.recIdMin = int.Parse(this.dt.Rows[this.dt.Rows.Count - 1][0].ToString());
						}
					}
				}
			}
			IL_0236:
			wgTools.WriteLine("da.Fill End " + startIndex.ToString());
			wgTools.WriteLine(this.Text + "  load LogQuery End");
			Cursor.Current = Cursors.Default;
			return this.dt;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00029438 File Offset: 0x00028438
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

		// Token: 0x040002E6 RID: 742
		private bool bLoadedFinished;

		// Token: 0x040002E7 RID: 743
		private DataTable dt;

		// Token: 0x040002E8 RID: 744
		private int recIdMin;

		// Token: 0x040002E9 RID: 745
		private int startRecordIndex;

		// Token: 0x040002EA RID: 746
		private string dgvSql = "";

		// Token: 0x040002EB RID: 747
		private int MaxRecord = 1000;

		// Token: 0x040002EC RID: 748
		public string specialLog = "";
	}
}
