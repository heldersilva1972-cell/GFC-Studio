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
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200037B RID: 891
	public partial class frmShiftAttReport : frmN3000
	{
		// Token: 0x06001D60 RID: 7520 RVA: 0x0026D0D4 File Offset: 0x0026C0D4
		public frmShiftAttReport()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x0026D140 File Offset: 0x0026C140
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

		// Token: 0x06001D62 RID: 7522 RVA: 0x0026D1C0 File Offset: 0x0026C1C0
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
				this.bLoadAll = false;
			}
			this.fillDgv(e.Result as DataTable);
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvMain.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x0026D278 File Offset: 0x0026C278
		private void btnCreateReport_Click(object sender, EventArgs e)
		{
			if (this.dtpDateFrom.Value > this.dtpDateTo.Value)
			{
				return;
			}
			string text = string.Format("{0}\r\n{1} {2} {3} {4}", new object[]
			{
				this.btnCreateReport.Text,
				this.toolStripLabel2.Text,
				this.dtpDateFrom.Value.ToString(wgTools.DisplayFormat_DateYMD),
				this.toolStripLabel3.Text,
				this.dtpDateTo.Value.ToString(wgTools.DisplayFormat_DateYMD)
			});
			if (XMessageBox.Show(string.Format(CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text2 = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text2, ref num4, ref num5);
			string text3 = " SELECT   f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled ";
			string text4 = text3;
			text4 += " FROM t_b_Consumer WHERE (f_AttendEnabled >0 ) ";
			if (num5 > 0)
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer WHERE (f_AttendEnabled >0 ) " + string.Format(" AND t_b_Consumer.f_ConsumerID ={0:d} ", num5);
			}
			else if (num > 0)
			{
				text4 = text3;
				if (num >= num3)
				{
					text4 = text4 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", num2);
				}
				else
				{
					text4 = text4 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", num3);
				}
				if (text2 != "")
				{
					text4 += string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text2)));
				}
				else if (num4 > 0L)
				{
					text4 += string.Format(" AND f_CardNO ={0:d} ", num4);
				}
				text4 += " AND (f_AttendEnabled >0 ) ";
			}
			else if (text2 != "")
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer  " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text2))) + " AND (f_AttendEnabled >0 ) ";
			}
			else if (num4 > 0L)
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer  " + string.Format(" WHERE f_CardNO ={0:d} ", num4) + " AND (f_AttendEnabled >0 ) ";
			}
			bool flag = false;
			int num6 = 0;
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text4.Replace(text3, " SELECT  COUNT(*) "), oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							flag = true;
							num6 = Convert.ToInt32(oleDbDataReader[0]);
						}
						oleDbDataReader.Close();
					}
					goto IL_0359;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text4.Replace(text3, " SELECT  COUNT(*) "), sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						flag = true;
						num6 = Convert.ToInt32(sqlDataReader[0]);
					}
					sqlDataReader.Close();
				}
			}
			IL_0359:
			if (flag)
			{
				if (base.OwnedForms.Length > 0)
				{
					foreach (Form form in base.OwnedForms)
					{
						if (form.Name == "dfrmShiftAttReportCreate")
						{
							return;
						}
					}
				}
				using (dfrmShiftAttReportCreate dfrmShiftAttReportCreate = new dfrmShiftAttReportCreate())
				{
					dfrmShiftAttReportCreate.totalConsumer = num6;
					dfrmShiftAttReportCreate.dtBegin = this.dtpDateFrom.Value;
					dfrmShiftAttReportCreate.dtEnd = this.dtpDateTo.Value;
					dfrmShiftAttReportCreate.strConsumerSql = text4;
					dfrmShiftAttReportCreate.groupName = this.userControlFind1.cboFindDept.Text;
					dfrmShiftAttReportCreate.ShowDialog(this);
					this.btnQuery_Click(null, null);
				}
			}
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x0026D6E0 File Offset: 0x0026C6E0
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x0026D712 File Offset: 0x0026C712
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvMain);
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x0026D726 File Offset: 0x0026C726
		private void btnFindOption_Click(object sender, EventArgs e)
		{
			if (this.dfrmFindOption == null)
			{
				this.dfrmFindOption = new dfrmShiftAttReportFindOption();
				this.dfrmFindOption.Owner = this;
			}
			this.dfrmFindOption.Show();
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x0026D752 File Offset: 0x0026C752
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x0026D768 File Offset: 0x0026C768
		public void btnQuery_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnQuery_Click_Acc(sender, e);
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			this.getLogCreateReport();
			if (!this.bLogCreateReport)
			{
				XMessageBox.Show(this, CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strCreateInAdvance, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			string text2 = "";
			bool flag = false;
			if (this.dfrmFindOption != null && this.dfrmFindOption.Visible)
			{
				flag = true;
				text2 = this.dfrmFindOption.getStrSql();
			}
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text3 = wgAppConfig.getSqlFindNormal(" SELECT t_d_shift_AttReport.f_RecID, t_b_Group.f_GroupName,        t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName,  t_d_shift_AttReport.f_ShiftDate AS f_ShiftDateShort,  t_d_shift_AttReport.f_ShiftID,  t_d_shift_AttReport.f_ReadTimes,        ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OnDuty1,108) , '') AS f_OnDuty1Short,         ISNULL(t_d_shift_AttReport.f_OnDuty1AttDesc, '') + ISNULL(t_d_shift_AttReport.f_OnDuty1CardRecordDesc, '') AS f_Desc1,         ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OffDuty1,108) , '') AS f_OffDuty1Short,                   ISNULL(t_d_shift_AttReport.f_OffDuty1AttDesc, '') + ISNULL(t_d_shift_AttReport.f_OffDuty1CardRecordDesc, '') AS f_Desc2,                  ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OnDuty2,108) , '') AS f_OnDuty2Short,               ISNULL(t_d_shift_AttReport.f_OnDuty2AttDesc, '') + ISNULL(t_d_shift_AttReport.f_OnDuty2CardRecordDesc, '') AS f_Desc3,                   ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OffDuty2,108) , '') AS f_OffDuty2Short,        ISNULL(t_d_shift_AttReport.f_OffDuty2AttDesc, '')+ ISNULL(t_d_shift_AttReport.f_OffDuty2CardRecordDesc, '') AS f_Desc4,                  ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OnDuty3,108) , '') AS f_OnDuty3Short,                     ISNULL(t_d_shift_AttReport.f_OnDuty3AttDesc, '') + ISNULL(t_d_shift_AttReport.f_OnDuty3CardRecordDesc, '') AS f_Desc5,                   ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OffDuty3,108) , '') AS f_OffDuty3Short,              ISNULL(t_d_shift_AttReport.f_OffDuty3AttDesc, '')+ ISNULL(t_d_shift_AttReport.f_OffDuty3CardRecordDesc, '') AS f_Desc6,                  ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OnDuty4,108) , '') AS f_OnDuty4Short,          ISNULL(t_d_shift_AttReport.f_OnDuty4AttDesc, '') + ISNULL(t_d_shift_AttReport.f_OnDuty4CardRecordDesc, '') AS f_Desc7,                   ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OffDuty4,108) , '') AS f_OffDuty4Short,         ISNULL(t_d_shift_AttReport.f_OffDuty4AttDesc, '')+ ISNULL(t_d_shift_AttReport.f_OffDuty4CardRecordDesc, '') AS f_Desc8, CASE WHEN [f_LateMinutes]>0 THEN (CASE WHEN [f_LateMinutes]<1 THEN '0.5' ELSE CONVERT(varchar(6),[f_LateMinutes]) END ) ELSE ' ' END [f_LateMinutes], CASE WHEN [f_LeaveEarlyMinutes]>0 THEN (CASE WHEN [f_LeaveEarlyMinutes]<1 THEN '0.5' ELSE CONVERT(varchar(6),[f_LeaveEarlyMinutes]) END ) ELSE ' ' END [f_LeaveEarlyMinutes], CASE WHEN [f_OvertimeHours]>0 THEN (CASE WHEN [f_OvertimeHours]<1 THEN '0.5' ELSE CONVERT(varchar(6),[f_OvertimeHours]) END ) ELSE ' ' END [f_OvertimeHours], CASE WHEN [f_AbsenceDays]>0 THEN (CASE WHEN [f_AbsenceDays]<1 THEN '0.5' ELSE CONVERT(varchar(6),[f_AbsenceDays]) END ) ELSE ' ' END [f_AbsenceDays], CASE WHEN [f_NotReadCardCount]>0 THEN (CASE WHEN [f_NotReadCardCount]<1 THEN '0.5' ELSE CONVERT(varchar(6),[f_NotReadCardCount]) END ) ELSE ' ' END [f_NotReadCardCount] ", "t_d_shift_AttReport", this.getSqlOfDateTime("t_d_shift_AttReport.f_ShiftDate"), num, num2, num3, text, num4, num5);
			if (flag)
			{
				text3 = text3 + " WHERE " + text2;
			}
			string text4 = "";
			if (sender == this.cmdQueryNormalShift)
			{
				text4 = " AND t_d_shift_AttReport.f_ShiftID IS NULL ";
			}
			else if (sender == this.cmdQueryOtherShift)
			{
				text4 = " AND t_d_shift_AttReport.f_ShiftID IS NOT NULL ";
			}
			if (!string.IsNullOrEmpty(text4))
			{
				if (text3.IndexOf(" WHERE ") > 0)
				{
					text3 += text4;
				}
				else
				{
					text3 = text3 + " WHERE (1>0) " + text4;
				}
			}
			this.reloadData(text3);
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x0026D8BC File Offset: 0x0026C8BC
		public void btnQuery_Click_Acc(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			this.getLogCreateReport();
			if (!this.bLogCreateReport)
			{
				XMessageBox.Show(this, CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strCreateInAdvance, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			string text2 = "";
			bool flag = false;
			if (this.dfrmFindOption != null && this.dfrmFindOption.Visible)
			{
				flag = true;
				text2 = this.dfrmFindOption.getStrSql();
			}
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text3 = wgAppConfig.getSqlFindNormal(string.Concat(new string[]
			{
				" SELECT t_d_shift_AttReport.f_RecID, t_b_Group.f_GroupName,        t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName,  t_d_shift_AttReport.f_ShiftDate AS f_ShiftDateShort,  t_d_shift_AttReport.f_ShiftID,  t_d_shift_AttReport.f_ReadTimes, ",
				string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OnDuty1", "f_OnDuty1Short"),
				string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OnDuty1AttDesc", "f_OnDuty1CardRecordDesc", "f_Desc1"),
				string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OffDuty1", "f_OffDuty1Short"),
				string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OffDuty1AttDesc", "f_OffDuty1CardRecordDesc", "f_Desc2"),
				string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OnDuty2", "f_OnDuty2Short"),
				string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OnDuty2AttDesc", "f_OnDuty2CardRecordDesc", "f_Desc3"),
				string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OffDuty2", "f_OffDuty2Short"),
				string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OffDuty2AttDesc", "f_OffDuty2CardRecordDesc", "f_Desc4"),
				string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OnDuty3", "f_OnDuty3Short"),
				string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OnDuty3AttDesc", "f_OnDuty3CardRecordDesc", "f_Desc5"),
				string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OffDuty3", "f_OffDuty3Short"),
				string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OffDuty3AttDesc", "f_OffDuty3CardRecordDesc", "f_Desc6"),
				string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OnDuty4", "f_OnDuty4Short"),
				string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OnDuty4AttDesc", "f_OnDuty4CardRecordDesc", "f_Desc7"),
				string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OffDuty4", "f_OffDuty4Short"),
				string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OffDuty4AttDesc", "f_OffDuty4CardRecordDesc", "f_Desc8"),
				"IIF ([f_LateMinutes]>0, IIF ([f_LateMinutes]<1, '0.5', CSTR([f_LateMinutes])), ' ') AS [f_LateMinutes], IIF ([f_LeaveEarlyMinutes]>0 ,  IIF ([f_LeaveEarlyMinutes]<1 , '0.5', CSTR([f_LeaveEarlyMinutes])), ' ') AS [f_LeaveEarlyMinutes], IIF ([f_OvertimeHours]>0,  IIF ([f_OvertimeHours]<1,  '0.5', CSTR([f_OvertimeHours])), ' ') AS [f_OvertimeHours], IIF ([f_AbsenceDays]>0,  IIF ([f_AbsenceDays]<1, '0.5', CSTR([f_AbsenceDays])), ' ') AS [f_AbsenceDays], IIF ([f_NotReadCardCount]>0, IIF ([f_NotReadCardCount]<1, '0.5', CSTR([f_NotReadCardCount])), ' ') AS [f_NotReadCardCount] "
			}), "t_d_shift_AttReport", this.getSqlOfDateTime("t_d_shift_AttReport.f_ShiftDate"), num, num2, num3, text, num4, num5);
			if (flag)
			{
				text3 = text3 + " WHERE " + text2;
			}
			string text4 = "";
			if (sender == this.cmdQueryNormalShift)
			{
				text4 = " AND t_d_shift_AttReport.f_ShiftID IS NULL ";
			}
			else if (sender == this.cmdQueryOtherShift)
			{
				text4 = " AND t_d_shift_AttReport.f_ShiftID IS NOT NULL ";
			}
			if (!string.IsNullOrEmpty(text4))
			{
				if (text3.IndexOf(" WHERE ") > 0)
				{
					text3 += text4;
				}
				else
				{
					text3 = text3 + " WHERE (1>0) " + text4;
				}
			}
			this.reloadData(text3);
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x0026DBD0 File Offset: 0x0026CBD0
		private void btnSelectColumns_Click(object sender, EventArgs e)
		{
			using (dfrmSelectColumnsShow dfrmSelectColumnsShow = new dfrmSelectColumnsShow())
			{
				for (int i = 1; i < this.arrColsName.Count; i++)
				{
					dfrmSelectColumnsShow.chkListColumns.Items.Add(this.arrColsName[i]);
					dfrmSelectColumnsShow.chkListColumns.SetItemChecked(i - 1, bool.Parse(this.arrColsShow[i].ToString()));
				}
				if (dfrmSelectColumnsShow.ShowDialog(this) == DialogResult.OK)
				{
					this.arrColsShow.Clear();
					this.arrColsShow.Add(this.dgvMain.Columns[0].Visible);
					for (int j = 1; j < this.dgvMain.ColumnCount; j++)
					{
						this.dgvMain.Columns[j].Visible = dfrmSelectColumnsShow.chkListColumns.GetItemChecked(j - 1);
						this.arrColsShow.Add(this.dgvMain.Columns[j].Visible);
					}
					this.saveColumns();
					if (wgAppConfig.ReadGVStyleFileExisted(this, this.dgvMain))
					{
						wgAppConfig.SaveDGVStyle(this, this.dgvMain);
					}
				}
			}
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x0026DD24 File Offset: 0x0026CD24
		private void btnStatistics_Click(object sender, EventArgs e)
		{
			using (frmShiftAttStatistics frmShiftAttStatistics = new frmShiftAttStatistics())
			{
				frmShiftAttStatistics.ShowDialog(this);
			}
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x0026DD5C File Offset: 0x0026CD5C
		private void cmdCreateWithSomeConsumer_Click(object sender, EventArgs e)
		{
			if (this.dtpDateFrom.Value > this.dtpDateTo.Value)
			{
				return;
			}
			string text = string.Format("{0}\r\n{1} {2} {3} {4}", new object[]
			{
				this.btnCreateReport.Text,
				this.toolStripLabel2.Text,
				this.dtpDateFrom.Value.ToString(wgTools.DisplayFormat_DateYMD),
				this.toolStripLabel3.Text,
				this.dtpDateTo.Value.ToString(wgTools.DisplayFormat_DateYMD)
			});
			if (XMessageBox.Show(string.Format(CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text2 = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text2, ref num4, ref num5);
			string text3 = " SELECT   f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled ";
			string text4 = text3;
			text4 += " FROM t_b_Consumer WHERE (f_AttendEnabled >0 ) ";
			if (num5 > 0)
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer WHERE (f_AttendEnabled >0 ) " + string.Format(" AND t_b_Consumer.f_ConsumerID ={0:d} ", num5);
			}
			else if (num > 0)
			{
				text4 = text3;
				if (num >= num3)
				{
					text4 = text4 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", num2);
				}
				else
				{
					text4 = text4 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", num3);
				}
				if (text2 != "")
				{
					text4 += string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text2)));
				}
				else if (num4 > 0L)
				{
					text4 += string.Format(" AND f_CardNO ={0:d} ", num4);
				}
				text4 += " AND (f_AttendEnabled >0 ) ";
			}
			else if (text2 != "")
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer  " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text2))) + " AND (f_AttendEnabled >0 ) ";
			}
			else if (num4 > 0L)
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer  " + string.Format(" WHERE f_CardNO ={0:d} ", num4) + " AND (f_AttendEnabled >0 ) ";
			}
			dfrmUserSelected dfrmUserSelected = new dfrmUserSelected();
			dfrmUserSelected.bFilterNotAttend = true;
			if (dfrmUserSelected.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}
			if (string.IsNullOrEmpty(dfrmUserSelected.selectedUsers))
			{
				return;
			}
			text4 = text3;
			text4 = text4 + " FROM t_b_Consumer  " + string.Format(" WHERE f_ConsumerID IN ({0}) AND {1} ", dfrmUserSelected.selectedUsers, "   (f_AttendEnabled >0 ) ");
			bool flag = false;
			int num6 = 0;
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text4.Replace(text3, " SELECT  COUNT(*) "), oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							flag = true;
							num6 = Convert.ToInt32(oleDbDataReader[0]);
						}
						oleDbDataReader.Close();
					}
					goto IL_03AB;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text4.Replace(text3, " SELECT  COUNT(*) "), sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						flag = true;
						num6 = Convert.ToInt32(sqlDataReader[0]);
					}
					sqlDataReader.Close();
				}
			}
			IL_03AB:
			if (flag)
			{
				if (base.OwnedForms.Length > 0)
				{
					foreach (Form form in base.OwnedForms)
					{
						if (form.Name == "dfrmShiftAttReportCreate")
						{
							return;
						}
					}
				}
				using (dfrmShiftAttReportCreate dfrmShiftAttReportCreate = new dfrmShiftAttReportCreate())
				{
					dfrmShiftAttReportCreate.totalConsumer = num6;
					dfrmShiftAttReportCreate.dtBegin = this.dtpDateFrom.Value;
					dfrmShiftAttReportCreate.dtEnd = this.dtpDateTo.Value;
					dfrmShiftAttReportCreate.strConsumerSql = text4;
					dfrmShiftAttReportCreate.groupName = this.userControlFind1.cboFindDept.Text;
					dfrmShiftAttReportCreate.ShowDialog(this);
					this.btnQuery_Click(null, null);
				}
			}
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x0026E214 File Offset: 0x0026D214
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

		// Token: 0x06001D6E RID: 7534 RVA: 0x0026E354 File Offset: 0x0026D354
		private void displayAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!this.bLoadedFinished)
			{
				if (!this.bLogCreateReport)
				{
					XMessageBox.Show(this, CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strCreateInAdvance, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					return;
				}
				if (!this.bLoadAll)
				{
					this.bLoadAll = true;
					this.btnQuery_Click(null, null);
				}
			}
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x0026E3AC File Offset: 0x0026D3AC
		private void exportOver65535ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataGridView dataGridView = this.dgvMain;
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

		// Token: 0x06001D70 RID: 7536 RVA: 0x0026E528 File Offset: 0x0026D528
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
						this.dgvMain.Columns[i].Name = dt.Columns[i].ColumnName;
					}
					wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_ShiftDateShort", wgTools.DisplayFormat_DateYMDWeek);
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

		// Token: 0x06001D71 RID: 7537 RVA: 0x0026E6C0 File Offset: 0x0026D6C0
		private void frmShiftAttReport_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFindOption != null)
			{
				this.dfrmFindOption.Close();
			}
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x0026E6D8 File Offset: 0x0026D6D8
		private void frmShiftAttReport_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x06001D73 RID: 7539 RVA: 0x0026E734 File Offset: 0x0026D734
		private void frmSwipeRecords_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName.HeaderText);
			this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
			this.loadOperatorPrivilege();
			this.dtpDateFrom = new frmShiftAttReport.ToolStripDateTime();
			this.dtpDateTo = new frmShiftAttReport.ToolStripDateTime();
			this.toolStrip3.Items.Clear();
			this.toolStrip3.Items.AddRange(new ToolStripItem[] { this.toolStripLabel2, this.dtpDateFrom, this.toolStripLabel3, this.dtpDateTo });
			this.dtpDateFrom.BoxWidth = 120;
			this.dtpDateTo.BoxWidth = 120;
			this.userControlFind1.btnQuery.Click += this.btnQuery_Click;
			this.dtpDateFrom.Enabled = true;
			this.dtpDateTo.Enabled = true;
			this.userControlFind1.toolStripLabel2.Visible = false;
			this.userControlFind1.txtFindCardID.Visible = false;
			this.saveDefaultStyle();
			this.loadStyle();
			Cursor.Current = Cursors.WaitCursor;
			this.getLogCreateReport();
			if (this.bLogCreateReport)
			{
				this.dtpDateFrom.Value = this.logDateStart;
				this.dtpDateTo.Value = this.logDateEnd;
			}
			else
			{
				this.dtpDateTo.Value = DateTime.Now.Date;
				this.dtpDateFrom.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
			}
			this.dtpDateFrom.BoxWidth = 150;
			this.dtpDateTo.BoxWidth = 150;
			wgAppConfig.setDisplayFormatDate(this.dtpDateFrom.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dtpDateTo.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				this.cmdQueryNormalShift.Visible = true;
				this.cmdQueryOtherShift.Visible = true;
			}
			this.absentNewName = wgAppConfig.GetKeyVal("KEY_strAbsent");
			if (!string.IsNullOrEmpty(this.absentNewName))
			{
				for (int i = 0; i < this.dgvMain.Columns.Count; i++)
				{
					this.dgvMain.Columns[i].HeaderText = this.dgvMain.Columns[i].HeaderText.Replace(CommonStr.strAbsence, this.absentNewName);
				}
			}
			this.Refresh();
			this.userControlFind1.btnQuery.PerformClick();
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x0026E9D0 File Offset: 0x0026D9D0
		public void getLogCreateReport()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.getLogCreateReport_Acc();
				return;
			}
			this.bLogCreateReport = false;
			string text = "SELECT * FROM t_a_Attendence WHERE [f_NO]=15 ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
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

		// Token: 0x06001D75 RID: 7541 RVA: 0x0026EAE4 File Offset: 0x0026DAE4
		public void getLogCreateReport_Acc()
		{
			this.bLogCreateReport = false;
			string text = "SELECT * FROM t_a_Attendence WHERE [f_NO]=15 ";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
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

		// Token: 0x06001D76 RID: 7542 RVA: 0x0026EBE8 File Offset: 0x0026DBE8
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

		// Token: 0x06001D77 RID: 7543 RVA: 0x0026ECC4 File Offset: 0x0026DCC4
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
				strSql += string.Format(" AND t_d_shift_AttReport.f_RecID > {0:d}", this.recIdMax);
			}
			else
			{
				strSql += string.Format(" WHERE t_d_shift_AttReport.f_RecID > {0:d}", this.recIdMax);
			}
			strSql += " ORDER BY t_d_shift_AttReport.f_RecID ";
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
			if (!string.IsNullOrEmpty(this.absentNewName))
			{
				for (int i = 0; i < this.table.Rows.Count; i++)
				{
					for (int j = 0; j < this.table.Columns.Count; j++)
					{
						if (this.table.Rows[i][j].ToString().Equals(CommonStr.strAbsence))
						{
							this.table.Rows[i][j] = this.table.Rows[i][j].ToString().Replace(CommonStr.strAbsence, this.absentNewName);
						}
					}
				}
			}
			wgTools.WriteLine("da.Fill End " + startIndex.ToString());
			Cursor.Current = Cursors.Default;
			wgTools.WriteLine(this.Text + "  loadRecords End");
			return this.table;
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x0026EFFC File Offset: 0x0026DFFC
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

		// Token: 0x06001D79 RID: 7545 RVA: 0x0026F148 File Offset: 0x0026E148
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuAttendenceData";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnCreateReport.Visible = false;
			}
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x0026F178 File Offset: 0x0026E178
		private void loadStyle()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadStyle_Acc();
				return;
			}
			if (this.OnlyTwoTimesSpecial())
			{
				this.dgvMain.Columns[25].HeaderText = CommonStr.strWorkHour;
			}
			if (!wgAppConfig.getParamValBoolByNO(113))
			{
				int num = 2;
				string text = "SELECT f_Value FROM t_a_Attendence WHERE f_No =14";
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						if (sqlDataReader.Read())
						{
							int.TryParse(wgTools.SetObjToStr(sqlDataReader[0]), out num);
						}
						sqlDataReader.Close();
					}
				}
				this.dgvMain.Columns[5].Visible = false;
				this.dgvMain.Columns[6].Visible = false;
				if (num == 4)
				{
					this.dgvMain.Columns[7].HeaderText = CommonStr.strAMOnDuty;
					this.dgvMain.Columns[8].HeaderText = CommonStr.strDutyDesc1;
					this.dgvMain.Columns[9].HeaderText = CommonStr.strAMOffDuty;
					this.dgvMain.Columns[10].HeaderText = CommonStr.strDutyDesc2;
					this.dgvMain.Columns[11].HeaderText = CommonStr.strPMOnDuty;
					this.dgvMain.Columns[12].HeaderText = CommonStr.strDutyDesc3;
					this.dgvMain.Columns[13].HeaderText = CommonStr.strPMOffDuty;
					this.dgvMain.Columns[14].HeaderText = CommonStr.strDutyDesc4;
					for (int i = 15; i < 23; i++)
					{
						this.dgvMain.Columns[i].Visible = false;
					}
					if (this.OnlyOnDutySpecial())
					{
						this.dgvMain.Columns[9].Visible = false;
						this.dgvMain.Columns[10].Visible = false;
						this.dgvMain.Columns[13].Visible = false;
						this.dgvMain.Columns[14].Visible = false;
						this.dgvMain.Columns[24].Visible = false;
						this.dgvMain.Columns[25].Visible = false;
						this.dgvMain.Columns[27].Visible = false;
					}
				}
				else
				{
					this.dgvMain.Columns[7].HeaderText = CommonStr.strAMOnDuty;
					this.dgvMain.Columns[8].HeaderText = CommonStr.strOnDutyDesc;
					this.dgvMain.Columns[9].HeaderText = CommonStr.strPMOffDuty;
					this.dgvMain.Columns[10].HeaderText = CommonStr.strOffDutyDesc;
					for (int j = 11; j < 23; j++)
					{
						this.dgvMain.Columns[j].Visible = false;
					}
					if (this.OnlyOnDutySpecial())
					{
						this.dgvMain.Columns[9].Visible = false;
						this.dgvMain.Columns[10].Visible = false;
						this.dgvMain.Columns[24].Visible = false;
						this.dgvMain.Columns[25].Visible = false;
						this.dgvMain.Columns[27].Visible = false;
					}
				}
			}
			this.dgvMain.AutoGenerateColumns = false;
			this.arrColsName.Clear();
			this.arrColsShow.Clear();
			for (int k = 0; k < this.dgvMain.ColumnCount; k++)
			{
				this.arrColsName.Add(this.dgvMain.Columns[k].HeaderText);
				this.arrColsShow.Add(this.dgvMain.Columns[k].Visible);
			}
			string text2 = "";
			string text3 = "";
			for (int l = 0; l < this.arrColsName.Count; l++)
			{
				if (text2 != "")
				{
					text2 += ",";
					text3 += ",";
				}
				text2 += this.arrColsName[l];
				text3 += this.arrColsShow[l].ToString();
			}
			string keyVal = wgAppConfig.GetKeyVal(base.Name + "-" + this.dgvMain.Tag);
			if (keyVal != "")
			{
				string[] array = keyVal.Split(new char[] { ';' });
				if (array.Length == 2 && text2 == array[0] && text3 != array[1])
				{
					string[] array2 = array[1].Split(new char[] { ',' });
					if (array2.Length == this.arrColsName.Count)
					{
						this.arrColsShow.Clear();
						for (int m = 0; m < this.dgvMain.ColumnCount; m++)
						{
							this.dgvMain.Columns[m].Visible = bool.Parse(array2[m]);
							this.arrColsShow.Add(this.dgvMain.Columns[m].Visible);
						}
					}
				}
			}
			wgAppConfig.ReadGVStyle(this, this.dgvMain);
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x0026F778 File Offset: 0x0026E778
		private void loadStyle_Acc()
		{
			if (this.OnlyTwoTimesSpecial())
			{
				this.dgvMain.Columns[25].HeaderText = CommonStr.strWorkHour;
			}
			if (!wgAppConfig.getParamValBoolByNO(113))
			{
				int num = 2;
				string text = "SELECT f_Value FROM t_a_Attendence WHERE f_No =14";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							int.TryParse(wgTools.SetObjToStr(oleDbDataReader[0]), out num);
						}
						oleDbDataReader.Close();
					}
				}
				this.dgvMain.Columns[5].Visible = false;
				this.dgvMain.Columns[6].Visible = false;
				if (num == 4)
				{
					this.dgvMain.Columns[7].HeaderText = CommonStr.strAMOnDuty;
					this.dgvMain.Columns[8].HeaderText = CommonStr.strDutyDesc1;
					this.dgvMain.Columns[9].HeaderText = CommonStr.strAMOffDuty;
					this.dgvMain.Columns[10].HeaderText = CommonStr.strDutyDesc2;
					this.dgvMain.Columns[11].HeaderText = CommonStr.strPMOnDuty;
					this.dgvMain.Columns[12].HeaderText = CommonStr.strDutyDesc3;
					this.dgvMain.Columns[13].HeaderText = CommonStr.strPMOffDuty;
					this.dgvMain.Columns[14].HeaderText = CommonStr.strDutyDesc4;
					for (int i = 15; i < 23; i++)
					{
						this.dgvMain.Columns[i].Visible = false;
					}
					if (this.OnlyOnDutySpecial())
					{
						this.dgvMain.Columns[9].Visible = false;
						this.dgvMain.Columns[10].Visible = false;
						this.dgvMain.Columns[13].Visible = false;
						this.dgvMain.Columns[14].Visible = false;
						this.dgvMain.Columns[24].Visible = false;
						this.dgvMain.Columns[25].Visible = false;
						this.dgvMain.Columns[27].Visible = false;
					}
				}
				else
				{
					this.dgvMain.Columns[7].HeaderText = CommonStr.strAMOnDuty;
					this.dgvMain.Columns[8].HeaderText = CommonStr.strOnDutyDesc;
					this.dgvMain.Columns[9].HeaderText = CommonStr.strPMOffDuty;
					this.dgvMain.Columns[10].HeaderText = CommonStr.strOffDutyDesc;
					for (int j = 11; j < 23; j++)
					{
						this.dgvMain.Columns[j].Visible = false;
					}
					if (this.OnlyOnDutySpecial())
					{
						this.dgvMain.Columns[9].Visible = false;
						this.dgvMain.Columns[10].Visible = false;
						this.dgvMain.Columns[24].Visible = false;
						this.dgvMain.Columns[25].Visible = false;
						this.dgvMain.Columns[27].Visible = false;
					}
				}
			}
			this.dgvMain.AutoGenerateColumns = false;
			this.arrColsName.Clear();
			this.arrColsShow.Clear();
			for (int k = 0; k < this.dgvMain.ColumnCount; k++)
			{
				this.arrColsName.Add(this.dgvMain.Columns[k].HeaderText);
				this.arrColsShow.Add(this.dgvMain.Columns[k].Visible);
			}
			string text2 = "";
			string text3 = "";
			for (int l = 0; l < this.arrColsName.Count; l++)
			{
				if (text2 != "")
				{
					text2 += ",";
					text3 += ",";
				}
				text2 += this.arrColsName[l];
				text3 += this.arrColsShow[l].ToString();
			}
			string keyVal = wgAppConfig.GetKeyVal(base.Name + "-" + this.dgvMain.Tag);
			if (keyVal != "")
			{
				string[] array = keyVal.Split(new char[] { ';' });
				if (array.Length == 2 && text2 == array[0] && text3 != array[1])
				{
					string[] array2 = array[1].Split(new char[] { ',' });
					if (array2.Length == this.arrColsName.Count)
					{
						this.arrColsShow.Clear();
						for (int m = 0; m < this.dgvMain.ColumnCount; m++)
						{
							this.dgvMain.Columns[m].Visible = bool.Parse(array2[m]);
							this.arrColsShow.Add(this.dgvMain.Columns[m].Visible);
						}
					}
				}
			}
			wgAppConfig.ReadGVStyle(this, this.dgvMain);
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0026FD6C File Offset: 0x0026ED6C
		private bool OnlyOnDutySpecial()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.OnlyOnDutySpecial_Acc();
			}
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				return false;
			}
			string text = "SELECT * FROM t_a_Attendence";
			bool flag;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					while (sqlDataReader.Read())
					{
						if ((int)sqlDataReader["f_No"] == 14)
						{
							Convert.ToInt32(sqlDataReader["f_Value"]);
						}
					}
					sqlDataReader.Close();
					flag = wgAppConfig.getSystemParamByNO(59).ToString() == "1";
				}
			}
			return flag;
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x0026FE40 File Offset: 0x0026EE40
		private bool OnlyOnDutySpecial_Acc()
		{
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				return false;
			}
			string text = "SELECT * FROM t_a_Attendence";
			bool flag;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					while (oleDbDataReader.Read())
					{
						if ((int)oleDbDataReader["f_No"] == 14)
						{
							Convert.ToInt32(oleDbDataReader["f_Value"]);
						}
					}
					oleDbDataReader.Close();
					flag = wgAppConfig.getSystemParamByNO(59).ToString() == "1";
				}
			}
			return flag;
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0026FF04 File Offset: 0x0026EF04
		private bool OnlyTwoTimesSpecial()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.OnlyTwoTimesSpecial_Acc();
			}
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				return false;
			}
			string text = "SELECT * FROM t_a_Attendence";
			bool flag;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					int num = 2;
					while (sqlDataReader.Read())
					{
						if ((int)sqlDataReader["f_No"] == 14)
						{
							num = Convert.ToInt32(sqlDataReader["f_Value"]);
						}
					}
					sqlDataReader.Close();
					if (num == 4)
					{
						return false;
					}
					flag = wgAppConfig.getSystemParamByNO(57).ToString() == "1";
				}
			}
			return flag;
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x0026FFE8 File Offset: 0x0026EFE8
		private bool OnlyTwoTimesSpecial_Acc()
		{
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				return false;
			}
			string text = "SELECT * FROM t_a_Attendence";
			bool flag;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					int num = 2;
					while (oleDbDataReader.Read())
					{
						if ((int)oleDbDataReader["f_No"] == 14)
						{
							num = Convert.ToInt32(oleDbDataReader["f_Value"]);
						}
					}
					oleDbDataReader.Close();
					if (num == 4)
					{
						return false;
					}
					flag = wgAppConfig.getSystemParamByNO(57).ToString() == "1";
				}
			}
			return flag;
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x002700C0 File Offset: 0x0026F0C0
		private void printPageBreakForEveryOneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataTable dataTable = (DataTable)this.dgvMain.DataSource;
			DataView dataView = new DataView(dataTable);
			if (dataView.Count == 0)
			{
				return;
			}
			try
			{
				dataView.Sort = "f_ConsumerNO";
				ArrayList arrayList = new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				string text = "";
				for (int i = 0; i < dataView.Count; i++)
				{
					if (string.IsNullOrEmpty(text) || text != (string)dataView[i]["f_ConsumerNO"])
					{
						text = (string)dataView[i]["f_ConsumerNO"];
						arrayList.Add(text);
						arrayList2.Add((string)dataView[i]["f_ConsumerName"]);
					}
				}
				this.dgvMain.DataSource = dataView;
				for (int j = 0; j < arrayList.Count; j++)
				{
					dataView.RowFilter = "f_ConsumerNO = " + wgTools.PrepareStr(arrayList[j]);
					if (wgAppConfig.printdgvWithoutPrintDialog(this.dgvMain, string.Format("{0}  {1}", (string)arrayList2[j], this.Text)) <= 0)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.dgvMain.DataSource = dataTable;
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x00270230 File Offset: 0x0026F230
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
				if (this.bLoadAll)
				{
					this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, 100000000, this.dgvSql });
					return;
				}
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
			}
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x002702F9 File Offset: 0x0026F2F9
		private void restoreDefaultLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.RestoreGVStyle(this, this.dgvMain);
			wgAppConfig.UpdateKeyVal(base.Name + "-" + this.dgvMain.Tag, "");
			this.loadDefaultStyle();
			this.loadStyle();
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x00270338 File Offset: 0x0026F338
		private void saveColumns()
		{
			string text = "";
			string text2 = "";
			for (int i = 0; i < this.arrColsName.Count; i++)
			{
				if (text != "")
				{
					text += ",";
					text2 += ",";
				}
				text += this.arrColsName[i];
				text2 += this.arrColsShow[i].ToString();
			}
			wgAppConfig.InsertKeyVal(base.Name + "-" + this.dgvMain.Tag, text + ";" + text2);
			wgAppConfig.UpdateKeyVal(base.Name + "-" + this.dgvMain.Tag, text + ";" + text2);
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x00270410 File Offset: 0x0026F410
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

		// Token: 0x06001D85 RID: 7557 RVA: 0x00270545 File Offset: 0x0026F545
		private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.SaveDGVStyle(this, this.dgvMain);
			XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
		}

		// Token: 0x04003860 RID: 14432
		private bool bLoadAll;

		// Token: 0x04003861 RID: 14433
		private bool bLoadedFinished;

		// Token: 0x04003862 RID: 14434
		private bool bLogCreateReport;

		// Token: 0x04003863 RID: 14435
		private dfrmShiftAttReportFindOption dfrmFindOption;

		// Token: 0x04003864 RID: 14436
		private DateTime logDateEnd;

		// Token: 0x04003865 RID: 14437
		private DateTime logDateStart;

		// Token: 0x04003866 RID: 14438
		private int recIdMax;

		// Token: 0x04003867 RID: 14439
		private int startRecordIndex;

		// Token: 0x04003868 RID: 14440
		private DataTable table;

		// Token: 0x04003869 RID: 14441
		private string absentNewName = "";

		// Token: 0x0400386A RID: 14442
		private ArrayList arrColsName = new ArrayList();

		// Token: 0x0400386B RID: 14443
		private ArrayList arrColsShow = new ArrayList();

		// Token: 0x0400386C RID: 14444
		private string dgvSql = "";

		// Token: 0x0400386D RID: 14445
		private DataSet dsDefaultStyle = new DataSet("DGV_STILE");

		// Token: 0x0400386E RID: 14446
		private int MaxRecord = 1000;

		// Token: 0x0400387E RID: 14462
		private frmShiftAttReport.ToolStripDateTime dtpDateFrom;

		// Token: 0x0400387F RID: 14463
		private frmShiftAttReport.ToolStripDateTime dtpDateTo;

		// Token: 0x0200037C RID: 892
		public class ToolStripDateTime : ToolStripControlHost
		{
			// Token: 0x06001D88 RID: 7560 RVA: 0x00271813 File Offset: 0x00270813
			public ToolStripDateTime()
				: base(frmShiftAttReport.ToolStripDateTime.dtp = new DateTimePicker())
			{
			}

			// Token: 0x06001D89 RID: 7561 RVA: 0x00271826 File Offset: 0x00270826
			protected override void Dispose(bool disposing)
			{
				if (disposing && frmShiftAttReport.ToolStripDateTime.dtp != null)
				{
					frmShiftAttReport.ToolStripDateTime.dtp.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x06001D8A RID: 7562 RVA: 0x00271844 File Offset: 0x00270844
			public void SetTimeFormat()
			{
				DateTimePicker dateTimePicker = base.Control as DateTimePicker;
				dateTimePicker.CustomFormat = "HH;mm";
				dateTimePicker.Format = DateTimePickerFormat.Custom;
				dateTimePicker.ShowUpDown = true;
			}

			// Token: 0x1700026F RID: 623
			// (get) Token: 0x06001D8B RID: 7563 RVA: 0x00271878 File Offset: 0x00270878
			// (set) Token: 0x06001D8C RID: 7564 RVA: 0x002718A0 File Offset: 0x002708A0
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

			// Token: 0x17000270 RID: 624
			// (get) Token: 0x06001D8D RID: 7565 RVA: 0x00271904 File Offset: 0x00270904
			public DateTimePicker DateTimeControl
			{
				get
				{
					return base.Control as DateTimePicker;
				}
			}

			// Token: 0x17000271 RID: 625
			// (get) Token: 0x06001D8E RID: 7566 RVA: 0x00271911 File Offset: 0x00270911
			// (set) Token: 0x06001D8F RID: 7567 RVA: 0x00271924 File Offset: 0x00270924
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

			// Token: 0x040038A7 RID: 14503
			private static DateTimePicker dtp;
		}
	}
}
