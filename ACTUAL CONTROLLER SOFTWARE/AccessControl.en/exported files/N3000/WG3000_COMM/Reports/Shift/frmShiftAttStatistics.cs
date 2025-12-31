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
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200037D RID: 893
	public partial class frmShiftAttStatistics : frmN3000
	{
		// Token: 0x06001D90 RID: 7568 RVA: 0x00271988 File Offset: 0x00270988
		public frmShiftAttStatistics()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x00271A00 File Offset: 0x00270A00
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

		// Token: 0x06001D92 RID: 7570 RVA: 0x00271A80 File Offset: 0x00270A80
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

		// Token: 0x06001D93 RID: 7571 RVA: 0x00271B37 File Offset: 0x00270B37
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x00271B3F File Offset: 0x00270B3F
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x00271B71 File Offset: 0x00270B71
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvMain);
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x00271B85 File Offset: 0x00270B85
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x00271B98 File Offset: 0x00270B98
		private void btnQuery_Click(object sender, EventArgs e)
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
			string text2 = wgAppConfig.getSqlFindNormal(" SELECT t_d_shift_AttStatistic.f_RecID, t_b_Group.f_GroupName,        t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName, CASE WHEN CONVERT(decimal(10,1),[f_DayShouldWork]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_DayShouldWork]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_DayShouldWork]        ) END) ELSE ' ' END  [f_DayShouldWork]       ,  CASE WHEN CONVERT(decimal(10,1),[f_DayRealWork]         ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_DayRealWork]         ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_DayRealWork]          ) END) ELSE ' ' END  [f_DayRealWork]         ,  CASE WHEN CONVERT(decimal(10,1),[f_LateMinutes]         ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_LateMinutes]         ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_LateMinutes]          ) END) ELSE ' ' END  [f_LateMinutes]         ,  CASE WHEN CONVERT(decimal(10,1),[f_LateCount]           ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_LateCount]           ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_LateCount]            ) END) ELSE ' ' END  [f_LateCount]           ,  CASE WHEN CONVERT(decimal(10,1),[f_LeaveEarlyMinutes]   ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_LeaveEarlyMinutes]   ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_LeaveEarlyMinutes]    ) END) ELSE ' ' END  [f_LeaveEarlyMinutes]   ,  CASE WHEN CONVERT(decimal(10,1),[f_LeaveEarlyCount]     ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_LeaveEarlyCount]     ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_LeaveEarlyCount]      ) END) ELSE ' ' END  [f_LeaveEarlyCount]     ,  CASE WHEN CONVERT(decimal(10,1),[f_OvertimeHours]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_OvertimeHours]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_OvertimeHours]        ) END) ELSE ' ' END  [f_OvertimeHours]       ,  CASE WHEN CONVERT(decimal(10,1),[f_AbsenceDays]         ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_AbsenceDays]         ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_AbsenceDays]          ) END) ELSE ' ' END  [f_AbsenceDays]         ,  CASE WHEN CONVERT(decimal(10,1),[f_NotReadCardCount]    ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_NotReadCardCount]    ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_NotReadCardCount]     ) END) ELSE ' ' END  [f_NotReadCardCount]    ,  CASE WHEN CONVERT(decimal(10,1),[f_ManualReadTimesCount]) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_ManualReadTimesCount]) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_ManualReadTimesCount] ) END) ELSE ' ' END  [f_ManualReadTimesCount],  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType1]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType1]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType1]         ) END) ELSE ' ' END  [f_SpecialType1]        ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType2]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType2]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType2]         ) END) ELSE ' ' END  [f_SpecialType2]        ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType3]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType3]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType3]         ) END) ELSE ' ' END  [f_SpecialType3]        ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType4]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType4]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType4]         ) END) ELSE ' ' END  [f_SpecialType4]        ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType5]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType5]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType5]         ) END) ELSE ' ' END  [f_SpecialType5]        ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType6]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType6]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType6]         ) END) ELSE ' ' END  [f_SpecialType6]        ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType7]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType7]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType7]         ) END) ELSE ' ' END  [f_SpecialType7]        ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType8]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType8]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType8]         ) END) ELSE ' ' END  [f_SpecialType8]        ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType9]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType9]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType9]         ) END) ELSE ' ' END  [f_SpecialType9]        ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType10]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType10]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType10]        ) END) ELSE ' ' END  [f_SpecialType10]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType11]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType11]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType11]        ) END) ELSE ' ' END  [f_SpecialType11]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType12]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType12]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType12]        ) END) ELSE ' ' END  [f_SpecialType12]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType13]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType13]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType13]        ) END) ELSE ' ' END  [f_SpecialType13]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType14]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType14]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType14]        ) END) ELSE ' ' END  [f_SpecialType14]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType15]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType15]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType15]        ) END) ELSE ' ' END  [f_SpecialType15]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType16]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType16]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType16]        ) END) ELSE ' ' END  [f_SpecialType16]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType17]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType17]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType17]        ) END) ELSE ' ' END  [f_SpecialType17]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType18]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType18]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType18]        ) END) ELSE ' ' END  [f_SpecialType18]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType19]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType19]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType19]        ) END) ELSE ' ' END  [f_SpecialType19]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType20]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType20]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType20]        ) END) ELSE ' ' END  [f_SpecialType20]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType21]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType21]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType21]        ) END) ELSE ' ' END  [f_SpecialType21]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType22]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType22]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType22]        ) END) ELSE ' ' END  [f_SpecialType22]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType23]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType23]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType23]        ) END) ELSE ' ' END  [f_SpecialType23]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType24]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType24]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType24]        ) END) ELSE ' ' END  [f_SpecialType24]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType25]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType25]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType25]        ) END) ELSE ' ' END  [f_SpecialType25]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType26]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType26]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType26]        ) END) ELSE ' ' END  [f_SpecialType26]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType27]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType27]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType27]        ) END) ELSE ' ' END  [f_SpecialType27]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType28]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType28]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType28]        ) END) ELSE ' ' END  [f_SpecialType28]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType29]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType29]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType29]        ) END) ELSE ' ' END  [f_SpecialType29]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType30]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType30]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType30]        ) END) ELSE ' ' END  [f_SpecialType30]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType31]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType31]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType31]        ) END) ELSE ' ' END  [f_SpecialType31]       ,  CASE WHEN CONVERT(decimal(10,1),[f_SpecialType32]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType32]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType32]        ) END) ELSE ' ' END  [f_SpecialType32]          , t_d_shift_AttStatistic.f_Notes ", "t_d_shift_AttStatistic", "", num, num2, num3, text, num4, num5);
			string text3 = "";
			if (sender == this.cmdQueryNormalShift)
			{
				text3 = " AND t_d_shift_AttStatistic.f_ConsumerID IN (SELECT aaa.f_ConsumerID FROM t_b_Consumer aaa WHERE aaa.f_ShiftEnabled =0) ";
			}
			else if (sender == this.cmdQueryOtherShift)
			{
				text3 = " AND t_d_shift_AttStatistic.f_ConsumerID IN (SELECT aaa.f_ConsumerID FROM t_b_Consumer aaa WHERE aaa.f_ShiftEnabled =1) ";
			}
			if (!string.IsNullOrEmpty(text3))
			{
				if (text2.IndexOf(" WHERE ") > 0)
				{
					text2 += text3;
				}
				else
				{
					text2 = text2 + " WHERE (1>0) " + text3;
				}
			}
			this.reloadData(text2);
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x00271C90 File Offset: 0x00270C90
		private void btnQuery_Click_Acc(object sender, EventArgs e)
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
			string text2 = wgAppConfig.getSqlFindNormal(" SELECT t_d_shift_AttStatistic.f_RecID, t_b_Group.f_GroupName,        t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName, IIF((IIF(ISNULL([f_DayShouldWork]),0,[f_DayShouldWork])       ) >0 ,CSTR(t_d_shift_AttStatistic.[f_DayShouldWork]        ) , ' ') AS  [f_DayShouldWork]       ,  IIF((IIF(ISNULL([f_DayRealWork]         ),0,[f_DayRealWork] )         ) >0 ,IIF((IIF(ISNULL([f_DayRealWork]         ),0,[f_DayRealWork]          )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_DayRealWork]          ) ) , ' ') AS  [f_DayRealWork]         ,  IIF((IIF(ISNULL([f_LateMinutes]         ),0,[f_LateMinutes]          )) >0 ,IIF((IIF(ISNULL([f_LateMinutes]         ),0,[f_LateMinutes]          )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_LateMinutes]          ) ) , ' ') AS  [f_LateMinutes]         ,  IIF((IIF(ISNULL([f_LateCount]           ),0,[f_LateCount]            )) >0 ,IIF((IIF(ISNULL([f_LateCount]           ),0,[f_LateCount]            )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_LateCount]            ) ) , ' ') AS  [f_LateCount]           ,  IIF((IIF(ISNULL([f_LeaveEarlyMinutes]   ),0,[f_LeaveEarlyMinutes]    )) >0 ,IIF((IIF(ISNULL([f_LeaveEarlyMinutes]   ),0,[f_LeaveEarlyMinutes]    )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_LeaveEarlyMinutes]    ) ) , ' ') AS  [f_LeaveEarlyMinutes]   ,  IIF((IIF(ISNULL([f_LeaveEarlyCount]     ),0,[f_LeaveEarlyCount]      )) >0 ,IIF((IIF(ISNULL([f_LeaveEarlyCount]     ),0,[f_LeaveEarlyCount]      )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_LeaveEarlyCount]      ) ) , ' ') AS  [f_LeaveEarlyCount]     ,  IIF((IIF(ISNULL([f_OvertimeHours]       ),0,[f_OvertimeHours]        )) >0 ,IIF((IIF(ISNULL([f_OvertimeHours]       ),0,[f_OvertimeHours]        )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_OvertimeHours]        ) ) , ' ') AS  [f_OvertimeHours]       ,  IIF((IIF(ISNULL([f_AbsenceDays]         ),0,[f_AbsenceDays]          )) >0 ,IIF((IIF(ISNULL([f_AbsenceDays]         ),0,[f_AbsenceDays]          )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_AbsenceDays]          ) ) , ' ') AS  [f_AbsenceDays]         ,  IIF((IIF(ISNULL([f_NotReadCardCount]    ),0,[f_NotReadCardCount]     )) >0 ,IIF((IIF(ISNULL([f_NotReadCardCount]    ),0,[f_NotReadCardCount]     )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_NotReadCardCount]     ) ) , ' ') AS  [f_NotReadCardCount]    ,  IIF((IIF(ISNULL([f_ManualReadTimesCount]),0,[f_ManualReadTimesCount] )) >0 ,IIF((IIF(ISNULL([f_ManualReadTimesCount]),0,[f_ManualReadTimesCount] )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_ManualReadTimesCount] ) ) , ' ') AS  [f_ManualReadTimesCount],  IIF(CDbl(IIF(ISNULL([f_SpecialType1]    ),0, [f_SpecialType1] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType1]    ),0, [f_SpecialType1] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType1]         ) )) , ' ') AS  [f_SpecialType1]        ,  IIF(CDbl(IIF(ISNULL([f_SpecialType2]    ),0, [f_SpecialType2] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType2]    ),0, [f_SpecialType2] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType2]         ) )) , ' ') AS  [f_SpecialType2]        ,  IIF(CDbl(IIF(ISNULL([f_SpecialType3]    ),0, [f_SpecialType3] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType3]    ),0, [f_SpecialType3] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType3]         ) )) , ' ') AS  [f_SpecialType3]        ,  IIF(CDbl(IIF(ISNULL([f_SpecialType4]    ),0, [f_SpecialType4] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType4]    ),0, [f_SpecialType4] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType4]         ) )) , ' ') AS  [f_SpecialType4]        ,  IIF(CDbl(IIF(ISNULL([f_SpecialType5]    ),0, [f_SpecialType5] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType5]    ),0, [f_SpecialType5] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType5]         ) )) , ' ') AS  [f_SpecialType5]        ,  IIF(CDbl(IIF(ISNULL([f_SpecialType6]    ),0, [f_SpecialType6] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType6]    ),0, [f_SpecialType6] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType6]         ) )) , ' ') AS  [f_SpecialType6]        ,  IIF(CDbl(IIF(ISNULL([f_SpecialType7]    ),0, [f_SpecialType7] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType7]    ),0, [f_SpecialType7] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType7]         ) )) , ' ') AS  [f_SpecialType7]        ,  IIF(CDbl(IIF(ISNULL([f_SpecialType8]    ),0, [f_SpecialType8] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType8]    ),0, [f_SpecialType8] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType8]         ) )) , ' ') AS  [f_SpecialType8]        ,  IIF(CDbl(IIF(ISNULL([f_SpecialType9]    ),0, [f_SpecialType9] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType9]    ),0, [f_SpecialType9] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType9]         ) )) , ' ') AS  [f_SpecialType9]        ,  IIF(CDbl(IIF(ISNULL([f_SpecialType10]   ),0, [f_SpecialType10])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType10]   ),0, [f_SpecialType10])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType10]        ) )) , ' ') AS  [f_SpecialType10]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType11]   ),0, [f_SpecialType11])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType11]   ),0, [f_SpecialType11])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType11]        ) )) , ' ') AS  [f_SpecialType11]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType12]   ),0, [f_SpecialType12])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType12]   ),0, [f_SpecialType12])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType12]        ) )) , ' ') AS  [f_SpecialType12]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType13]   ),0, [f_SpecialType13])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType13]   ),0, [f_SpecialType13])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType13]        ) )) , ' ') AS  [f_SpecialType13]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType14]   ),0, [f_SpecialType14])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType14]   ),0, [f_SpecialType14])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType14]        ) )) , ' ') AS  [f_SpecialType14]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType15]   ),0, [f_SpecialType15])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType15]   ),0, [f_SpecialType15])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType15]        ) )) , ' ') AS  [f_SpecialType15]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType16]   ),0, [f_SpecialType16])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType16]   ),0, [f_SpecialType16])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType16]        ) )) , ' ') AS  [f_SpecialType16]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType17]   ),0, [f_SpecialType17])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType17]   ),0, [f_SpecialType17])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType17]        ) )) , ' ') AS  [f_SpecialType17]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType18]   ),0, [f_SpecialType18])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType18]   ),0, [f_SpecialType18])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType18]        ) )) , ' ') AS  [f_SpecialType18]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType19]   ),0, [f_SpecialType19])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType19]   ),0, [f_SpecialType19])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType19]        ) )) , ' ') AS  [f_SpecialType19]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType20]   ),0, [f_SpecialType20])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType20]   ),0, [f_SpecialType20])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType20]        ) )) , ' ') AS  [f_SpecialType20]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType21]   ),0, [f_SpecialType21])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType21]   ),0, [f_SpecialType21])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType21]        ) )) , ' ') AS  [f_SpecialType21]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType22]   ),0, [f_SpecialType22])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType22]   ),0, [f_SpecialType22])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType22]        ) )) , ' ') AS  [f_SpecialType22]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType23]   ),0, [f_SpecialType23])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType23]   ),0, [f_SpecialType23])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType23]        ) )) , ' ') AS  [f_SpecialType23]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType24]   ),0, [f_SpecialType24])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType24]   ),0, [f_SpecialType24])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType24]        ) )) , ' ') AS  [f_SpecialType24]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType25]   ),0, [f_SpecialType25])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType25]   ),0, [f_SpecialType25])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType25]        ) )) , ' ') AS  [f_SpecialType25]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType26]   ),0, [f_SpecialType26])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType26]   ),0, [f_SpecialType26])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType26]        ) )) , ' ') AS  [f_SpecialType26]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType27]   ),0, [f_SpecialType27])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType27]   ),0, [f_SpecialType27])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType27]        ) )) , ' ') AS  [f_SpecialType27]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType28]   ),0, [f_SpecialType28])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType28]   ),0, [f_SpecialType28])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType28]        ) )) , ' ') AS  [f_SpecialType28]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType29]   ),0, [f_SpecialType29])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType29]   ),0, [f_SpecialType29])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType29]        ) )) , ' ') AS  [f_SpecialType29]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType30]   ),0, [f_SpecialType30])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType30]   ),0, [f_SpecialType30])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType30]        ) )) , ' ') AS  [f_SpecialType30]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType31]   ),0, [f_SpecialType31])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType31]   ),0, [f_SpecialType31])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType31]        ) )) , ' ') AS  [f_SpecialType31]       ,  IIF(CDbl(IIF(ISNULL([f_SpecialType32]   ),0, [f_SpecialType32])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType32]   ),0, [f_SpecialType32])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType32]        ) )) , ' ') AS  [f_SpecialType32]          , t_d_shift_AttStatistic.f_Notes ", "t_d_shift_AttStatistic", "", num, num2, num3, text, num4, num5);
			string text3 = "";
			if (sender == this.cmdQueryNormalShift)
			{
				text3 = " AND t_d_shift_AttStatistic.f_ConsumerID IN (SELECT aaa.f_ConsumerID FROM t_b_Consumer aaa WHERE aaa.f_ShiftEnabled =0) ";
			}
			else if (sender == this.cmdQueryOtherShift)
			{
				text3 = " AND t_d_shift_AttStatistic.f_ConsumerID IN (SELECT aaa.f_ConsumerID FROM t_b_Consumer aaa WHERE aaa.f_ShiftEnabled =1) ";
			}
			if (!string.IsNullOrEmpty(text3))
			{
				if (text2.IndexOf(" WHERE ") > 0)
				{
					text2 += text3;
				}
				else
				{
					text2 = text2 + " WHERE (1>0) " + text3;
				}
			}
			this.reloadData(text2);
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x00271D78 File Offset: 0x00270D78
		private void columnsConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmSelectColumnsShow dfrmSelectColumnsShow = new dfrmSelectColumnsShow())
			{
				for (int i = 0; i < this.arrColsName.Count; i++)
				{
					if (this.arrColsName[i].ToString().IndexOf("f_SpecialType") < 0)
					{
						dfrmSelectColumnsShow.chkListColumns.Items.Add(this.arrColsName[i].ToString().Replace("\r\n", ""));
						dfrmSelectColumnsShow.chkListColumns.SetItemChecked(dfrmSelectColumnsShow.chkListColumns.Items.Count - 1, bool.Parse(this.arrColsShow[i].ToString()));
					}
				}
				if (dfrmSelectColumnsShow.ShowDialog(this) == DialogResult.OK)
				{
					this.arrColsShow.Clear();
					int num = 1;
					for (int j = 0; j < this.dgvMain.ColumnCount; j++)
					{
						if (this.arrColsName[j].ToString().IndexOf("f_SpecialType") < 0)
						{
							this.dgvMain.Columns[j].Visible = dfrmSelectColumnsShow.chkListColumns.GetItemChecked(num - 1);
							num++;
						}
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

		// Token: 0x06001D9A RID: 7578 RVA: 0x00271F10 File Offset: 0x00270F10
		private void columnsSortToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmSortSet dfrmSortSet = new dfrmSortSet())
			{
				dfrmSortSet.arrColsName = new ArrayList();
				dfrmSortSet.arrColsShow = new ArrayList();
				for (int i = 0; i < this.arrColsName.Count; i++)
				{
					if (this.arrColsName[i].ToString().IndexOf("f_SpecialType") < 0)
					{
						dfrmSortSet.arrColsShow.Add(this.arrColsName[i].ToString());
						dfrmSortSet.arrColsName.Add(this.arrColsNameField[i].ToString());
					}
				}
				dfrmSortSet.strDisplaySort = this.strDisplaySort;
				if (dfrmSortSet.ShowDialog(this) == DialogResult.OK)
				{
					wgAppConfig.UpdateKeyVal("KEY_AttstaticSort", dfrmSortSet.strDisplaySort);
					this.strDisplaySort = dfrmSortSet.strDisplaySort;
					this.userControlFind1.btnQuery.PerformClick();
				}
			}
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x00272008 File Offset: 0x00271008
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

		// Token: 0x06001D9C RID: 7580 RVA: 0x00272148 File Offset: 0x00271148
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

		// Token: 0x06001D9D RID: 7581 RVA: 0x002721A0 File Offset: 0x002711A0
		private void editPrintersFooterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripMenuItem).Text;
				dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_ATTSTATISTICS_Printer_Footer");
				dfrmInputNewName.bNotAllowNull = false;
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					wgAppConfig.UpdateKeyVal("KEY_ATTSTATISTICS_Printer_Footer", wgTools.SetObjToStr(dfrmInputNewName.strNewName));
				}
			}
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x00272218 File Offset: 0x00271218
		private void exportOver65535ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = "SELECT [f_Value] From t_a_Attendence  WHERE [f_NO]= 15 ";
			string text2 = wgAppConfig.getValStringBySql(text);
			if (text2.IndexOf("--") > 0)
			{
				text2 = text2.Substring(0, text2.IndexOf("--"));
			}
			if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
			{
				text2 = DateTime.Parse(text2).ToString("yyyy" + CommonStr.strYYYYMM_YYYY + "M" + CommonStr.strYYYYMM_MM);
			}
			else
			{
				text2 = DateTime.Parse(text2).ToString("yyyy-MM ");
			}
			text2 += this.Text;
			string keyVal = wgAppConfig.GetKeyVal("KEY_ATTSTATISTICS_Printer_Footer");
			wgAppConfig.exportToExcel100W_WithTitle(this.dgvMain, this.Text, text2, keyVal);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x002722D8 File Offset: 0x002712D8
		private void exportOver65535WithRecIDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.dv.Sort))
			{
				this.dv.Sort = this.strDisplaySort;
				for (int i = 0; i < this.dv.Count; i++)
				{
					this.dv[i]["f_RecID"] = i + 1;
				}
			}
			this.dgvMain.Columns[0].Visible = true;
			wgAppConfig.exportToExcel100W(this.dgvMain, this.Text);
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x00272368 File Offset: 0x00271368
		private void fillDgv(DataTable dt)
		{
			try
			{
				try
				{
					ArrayList arrayList = new ArrayList();
					if (!string.IsNullOrEmpty(this.strDisplaySort))
					{
						string[] array = this.strDisplaySort.Split(new char[] { ',' });
						for (int i = 0; i < array.Length; i++)
						{
							string[] array2 = array[i].Trim().Split(new char[] { ' ' });
							if (!("f_RecID".ToUpper() == array2[0].Trim().ToUpper()) && !("f_GroupName".ToUpper() == array2[0].Trim().ToUpper()) && !("f_ConsumerNO".ToUpper() == array2[0].Trim().ToUpper()) && !("f_ConsumerName".ToUpper() == array2[0].Trim().ToUpper()))
							{
								arrayList.Add(array2[0].Trim());
							}
						}
					}
					for (int j = 0; j < dt.Rows.Count; j++)
					{
						for (int k = 0; k < arrayList.Count; k++)
						{
							dt.Rows[j][arrayList[k].ToString()] = dt.Rows[j][arrayList[k].ToString()].ToString().PadLeft(5, ' ');
						}
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				bool flag = true;
				for (int l = 0; l < this.dgvMain.ColumnCount; l++)
				{
					if (this.dgvMain.Columns[l].Visible)
					{
						flag = false;
					}
				}
				if (this.dgvMain.DataSource == null)
				{
					this.dv = new DataView(dt);
					this.dgvMain.DataSource = this.dv;
					this.arrColsNameField.Clear();
					for (int m = 0; m < dt.Columns.Count; m++)
					{
						this.dgvMain.Columns[m].DataPropertyName = dt.Columns[m].ColumnName;
						this.arrColsNameField.Add(this.dgvMain.Columns[m].DataPropertyName);
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
					(this.dgvMain.DataSource as DataView).Table.Merge(dt);
					if (firstDisplayedScrollingRowIndex >= 0)
					{
						this.dgvMain.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
					}
				}
				if (this.bLoadedFinished && this.dv != null && !string.IsNullOrEmpty(this.strDisplaySort))
				{
					this.dv.Sort = this.strDisplaySort;
					for (int n = 0; n < this.dv.Count; n++)
					{
						this.dv[n]["f_RecID"] = n + 1;
					}
				}
				if (flag)
				{
					for (int num = 0; num < this.dgvMain.ColumnCount; num++)
					{
						this.dgvMain.Columns[num].Visible = false;
					}
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x00272780 File Offset: 0x00271780
		private void frmShiftAttReport_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x00272784 File Offset: 0x00271784
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
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				this.cmdQueryNormalShift.Visible = true;
				this.cmdQueryOtherShift.Visible = true;
			}
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_AttstaticSort")))
			{
				this.strDisplaySort = wgAppConfig.GetKeyVal("KEY_AttstaticSort");
			}
			string keyVal = wgAppConfig.GetKeyVal("KEY_strAbsent");
			if (!string.IsNullOrEmpty(keyVal))
			{
				for (int i = 0; i < this.dgvMain.Columns.Count; i++)
				{
					this.dgvMain.Columns[i].HeaderText = this.dgvMain.Columns[i].HeaderText.Replace(CommonStr.strAbsence, keyVal);
				}
			}
			this.Refresh();
			this.timer1.Enabled = true;
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x002728DC File Offset: 0x002718DC
		public string getlocalizedHolidayType(string type)
		{
			string text = "";
			try
			{
				if (string.IsNullOrEmpty(type))
				{
					return type;
				}
				text = type;
				if (type == "出差" || type == "出差" || type == "Business Trip")
				{
					text = CommonStr.strBusinessTrip;
				}
				if (type == "病假" || type == "病假" || type == "Sick Leave")
				{
					text = CommonStr.strSickLeave;
				}
				if (!(type == "事假") && !(type == "事假") && !(type == "Private Leave"))
				{
					return text;
				}
				return CommonStr.strPrivateLeave;
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
			return text;
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x002729B0 File Offset: 0x002719B0
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

		// Token: 0x06001DA5 RID: 7589 RVA: 0x00272AE4 File Offset: 0x00271AE4
		public void getLogCreateReport_Acc()
		{
			this.bLogCreateReport = false;
			string text = "SELECT * FROM t_a_Attendence WHERE [f_NO]=15 ";
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

		// Token: 0x06001DA6 RID: 7590 RVA: 0x00272C0C File Offset: 0x00271C0C
		private string getSqlOfDateTime(string colNameOfDate)
		{
			return " 1>0 ";
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x00272C14 File Offset: 0x00271C14
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
				strSql += string.Format(" AND t_d_shift_AttStatistic.f_RecID > {0:d}", this.recIdMax);
			}
			else
			{
				strSql += string.Format(" WHERE t_d_shift_AttStatistic.f_RecID > {0:d}", this.recIdMax);
			}
			strSql += " ORDER BY t_d_shift_AttStatistic.f_RecID ";
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

		// Token: 0x06001DA8 RID: 7592 RVA: 0x00272E7C File Offset: 0x00271E7C
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
			if (this.dgvMain.ColumnCount > dataTable.Rows.Count)
			{
				int columnCount = this.dgvMain.ColumnCount;
				int num = 1;
				for (int j = dataTable.Rows.Count; j < columnCount; j++)
				{
					this.dgvMain.Columns.RemoveAt(columnCount - num);
					num++;
				}
			}
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x0027301C File Offset: 0x0027201C
		private void loadStyle()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadStyle_Acc();
				return;
			}
			if (this.OnlyTwoTimesSpecial())
			{
				this.dgvMain.Columns[10].HeaderText = CommonStr.strWorkHour;
			}
			if (this.OnlyOnDutySpecial())
			{
				this.dgvMain.Columns[8].Visible = false;
				this.dgvMain.Columns[9].Visible = false;
				this.dgvMain.Columns[10].Visible = false;
				this.dgvMain.Columns[12].Visible = false;
			}
			this.dgvMain.AutoGenerateColumns = false;
			string text = " SELECT * FROM t_a_HolidayType ORDER BY f_NO ";
			int i = 0;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					while (sqlDataReader.Read())
					{
						string text2 = "f_SpecialType" + (i + 1);
						this.dc = new DataGridViewTextBoxColumn();
						this.dc.HeaderText = this.getlocalizedHolidayType(sqlDataReader["f_HolidayType"].ToString()) + "\r\n(" + CommonStr.strDay + ")";
						this.dc.DataPropertyName = text2;
						this.dc.Name = this.dc.DataPropertyName;
						this.dc.Width = 45;
						this.dc.ReadOnly = true;
						this.dc.Visible = true;
						this.dgvMain.Columns.Add(this.dc);
						i++;
					}
					sqlDataReader.Close();
					while (i < 32)
					{
						string text2 = "f_SpecialType" + (i + 1);
						this.dc = new DataGridViewTextBoxColumn();
						this.dc.HeaderText = text2;
						this.dc.DataPropertyName = this.dc.DataPropertyName;
						this.dc.Name = this.dc.DataPropertyName;
						this.dc.Width = 45;
						this.dc.ReadOnly = true;
						this.dc.Visible = false;
						this.dgvMain.Columns.Add(this.dc);
						i++;
					}
				}
			}
			this.dc = new DataGridViewTextBoxColumn();
			this.dc.HeaderText = CommonStr.strNotes;
			this.dc.DataPropertyName = "f_Notes";
			this.dc.Name = this.dc.DataPropertyName;
			this.dc.Width = 200;
			this.dc.ReadOnly = true;
			this.dc.Visible = true;
			this.dgvMain.Columns.Add(this.dc);
			this.arrColsName.Clear();
			this.arrColsShow.Clear();
			for (int j = 0; j < this.dgvMain.ColumnCount; j++)
			{
				this.arrColsName.Add(this.dgvMain.Columns[j].HeaderText);
				this.arrColsNameField.Add(this.dgvMain.Columns[j].DataPropertyName);
				this.arrColsShow.Add(this.dgvMain.Columns[j].Visible);
			}
			string text3 = "";
			string text4 = "";
			for (int k = 0; k < this.arrColsName.Count; k++)
			{
				if (text3 != "")
				{
					text3 += ",";
					text4 += ",";
				}
				text3 += this.arrColsName[k];
				text4 += this.arrColsShow[k].ToString();
			}
			string keyVal = wgAppConfig.GetKeyVal(base.Name + "-" + this.dgvMain.Tag);
			if (keyVal != "")
			{
				string[] array = keyVal.Split(new char[] { ';' });
				if (array.Length == 2 && text3 == array[0] && text4 != array[1])
				{
					string[] array2 = array[1].Split(new char[] { ',' });
					if (array2.Length == this.arrColsName.Count)
					{
						this.arrColsShow.Clear();
						for (int l = 0; l < this.dgvMain.ColumnCount; l++)
						{
							this.dgvMain.Columns[l].Visible = bool.Parse(array2[l]);
							this.arrColsShow.Add(this.dgvMain.Columns[l].Visible);
						}
					}
				}
			}
			wgAppConfig.ReadGVStyle(this, this.dgvMain);
			this.arrColsName.Clear();
			this.arrColsShow.Clear();
			this.arrColsNameField.Clear();
			for (int m = 0; m < this.dgvMain.ColumnCount; m++)
			{
				this.arrColsName.Add(this.dgvMain.Columns[m].HeaderText);
				this.arrColsNameField.Add(this.dgvMain.Columns[m].DataPropertyName);
				this.arrColsShow.Add(this.dgvMain.Columns[m].Visible);
			}
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x00273624 File Offset: 0x00272624
		private void loadStyle_Acc()
		{
			if (this.OnlyTwoTimesSpecial())
			{
				this.dgvMain.Columns[10].HeaderText = CommonStr.strWorkHour;
			}
			if (this.OnlyOnDutySpecial())
			{
				this.dgvMain.Columns[8].Visible = false;
				this.dgvMain.Columns[9].Visible = false;
				this.dgvMain.Columns[10].Visible = false;
				this.dgvMain.Columns[12].Visible = false;
			}
			this.dgvMain.AutoGenerateColumns = false;
			string text = " SELECT * FROM t_a_HolidayType ORDER BY f_NO ";
			int i = 0;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					while (oleDbDataReader.Read())
					{
						string text2 = "f_SpecialType" + (i + 1);
						this.dc = new DataGridViewTextBoxColumn();
						this.dc.HeaderText = this.getlocalizedHolidayType(oleDbDataReader["f_HolidayType"].ToString()) + "\r\n(" + CommonStr.strDay + ")";
						this.dc.DataPropertyName = text2;
						this.dc.Name = this.dc.DataPropertyName;
						this.dc.Width = 45;
						this.dc.ReadOnly = true;
						this.dc.Visible = true;
						this.dgvMain.Columns.Add(this.dc);
						i++;
					}
					oleDbDataReader.Close();
					while (i < 32)
					{
						string text2 = "f_SpecialType" + (i + 1);
						this.dc = new DataGridViewTextBoxColumn();
						this.dc.HeaderText = text2;
						this.dc.DataPropertyName = text2;
						this.dc.Name = this.dc.DataPropertyName;
						this.dc.Width = 45;
						this.dc.ReadOnly = true;
						this.dc.Visible = false;
						this.dgvMain.Columns.Add(this.dc);
						i++;
					}
				}
			}
			this.dc = new DataGridViewTextBoxColumn();
			this.dc.HeaderText = CommonStr.strNotes;
			this.dc.DataPropertyName = "f_Notes";
			this.dc.Name = this.dc.DataPropertyName;
			this.dc.Width = 200;
			this.dc.ReadOnly = true;
			this.dc.Visible = true;
			this.dgvMain.Columns.Add(this.dc);
			this.arrColsName.Clear();
			this.arrColsShow.Clear();
			for (int j = 0; j < this.dgvMain.ColumnCount; j++)
			{
				this.arrColsName.Add(this.dgvMain.Columns[j].HeaderText);
				this.arrColsNameField.Add(this.dgvMain.Columns[j].DataPropertyName);
				this.arrColsShow.Add(this.dgvMain.Columns[j].Visible);
			}
			string text3 = "";
			string text4 = "";
			for (int k = 0; k < this.arrColsName.Count; k++)
			{
				if (text3 != "")
				{
					text3 += ",";
					text4 += ",";
				}
				text3 += this.arrColsName[k];
				text4 += this.arrColsShow[k].ToString();
			}
			string keyVal = wgAppConfig.GetKeyVal(base.Name + "-" + this.dgvMain.Tag);
			if (keyVal != "")
			{
				string[] array = keyVal.Split(new char[] { ';' });
				if (array.Length == 2 && text3 == array[0] && text4 != array[1])
				{
					string[] array2 = array[1].Split(new char[] { ',' });
					if (array2.Length == this.arrColsName.Count)
					{
						this.arrColsShow.Clear();
						for (int l = 0; l < this.dgvMain.ColumnCount; l++)
						{
							this.dgvMain.Columns[l].Visible = bool.Parse(array2[l]);
							this.arrColsShow.Add(this.dgvMain.Columns[l].Visible);
						}
					}
				}
			}
			wgAppConfig.ReadGVStyle(this, this.dgvMain);
			this.arrColsName.Clear();
			this.arrColsShow.Clear();
			this.arrColsNameField.Clear();
			for (int m = 0; m < this.dgvMain.ColumnCount; m++)
			{
				this.arrColsName.Add(this.dgvMain.Columns[m].HeaderText);
				this.arrColsNameField.Add(this.dgvMain.Columns[m].DataPropertyName);
				this.arrColsShow.Add(this.dgvMain.Columns[m].Visible);
			}
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x00273C18 File Offset: 0x00272C18
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

		// Token: 0x06001DAC RID: 7596 RVA: 0x00273CEC File Offset: 0x00272CEC
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

		// Token: 0x06001DAD RID: 7597 RVA: 0x00273DB0 File Offset: 0x00272DB0
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

		// Token: 0x06001DAE RID: 7598 RVA: 0x00273E94 File Offset: 0x00272E94
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

		// Token: 0x06001DAF RID: 7599 RVA: 0x00273F6C File Offset: 0x00272F6C
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

		// Token: 0x06001DB0 RID: 7600 RVA: 0x00274035 File Offset: 0x00273035
		private void restoreDefaultLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.RestoreGVStyle(this, this.dgvMain);
			wgAppConfig.UpdateKeyVal(base.Name + "-" + this.dgvMain.Tag, "");
			this.loadDefaultStyle();
			this.loadStyle();
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x00274074 File Offset: 0x00273074
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

		// Token: 0x06001DB2 RID: 7602 RVA: 0x0027414C File Offset: 0x0027314C
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

		// Token: 0x06001DB3 RID: 7603 RVA: 0x00274281 File Offset: 0x00273281
		private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.SaveDGVStyle(this, this.dgvMain);
			XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x002742AA File Offset: 0x002732AA
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			this.userControlFind1.btnQuery.PerformClick();
		}

		// Token: 0x040038A8 RID: 14504
		private bool bLoadAll;

		// Token: 0x040038A9 RID: 14505
		private bool bLoadedFinished;

		// Token: 0x040038AA RID: 14506
		private bool bLogCreateReport;

		// Token: 0x040038AB RID: 14507
		private DataView dv;

		// Token: 0x040038AC RID: 14508
		private DateTime logDateEnd;

		// Token: 0x040038AD RID: 14509
		private DateTime logDateStart;

		// Token: 0x040038AE RID: 14510
		private int recIdMax;

		// Token: 0x040038AF RID: 14511
		private int startRecordIndex;

		// Token: 0x040038B0 RID: 14512
		private DataTable table;

		// Token: 0x040038B1 RID: 14513
		private ArrayList arrColsName = new ArrayList();

		// Token: 0x040038B2 RID: 14514
		private ArrayList arrColsNameField = new ArrayList();

		// Token: 0x040038B3 RID: 14515
		private ArrayList arrColsShow = new ArrayList();

		// Token: 0x040038B4 RID: 14516
		private string dgvSql = "";

		// Token: 0x040038B5 RID: 14517
		private DataSet dsDefaultStyle = new DataSet("DGV_STILE");

		// Token: 0x040038B6 RID: 14518
		private int MaxRecord = 1000;

		// Token: 0x040038B7 RID: 14519
		private string strDisplaySort = "";

		// Token: 0x040038C3 RID: 14531
		private DataGridViewTextBoxColumn dc;

		// Token: 0x0200037E RID: 894
		public class ToolStripDateTime : ToolStripControlHost
		{
			// Token: 0x06001DB7 RID: 7607 RVA: 0x00275041 File Offset: 0x00274041
			public ToolStripDateTime()
				: base(frmShiftAttStatistics.ToolStripDateTime.dtp = new DateTimePicker())
			{
			}

			// Token: 0x06001DB8 RID: 7608 RVA: 0x00275054 File Offset: 0x00274054
			protected override void Dispose(bool disposing)
			{
				if (disposing && frmShiftAttStatistics.ToolStripDateTime.dtp != null)
				{
					frmShiftAttStatistics.ToolStripDateTime.dtp.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x06001DB9 RID: 7609 RVA: 0x00275074 File Offset: 0x00274074
			public void SetTimeFormat()
			{
				DateTimePicker dateTimePicker = base.Control as DateTimePicker;
				dateTimePicker.CustomFormat = "HH;mm";
				dateTimePicker.Format = DateTimePickerFormat.Custom;
				dateTimePicker.ShowUpDown = true;
			}

			// Token: 0x17000272 RID: 626
			// (get) Token: 0x06001DBA RID: 7610 RVA: 0x002750A8 File Offset: 0x002740A8
			// (set) Token: 0x06001DBB RID: 7611 RVA: 0x002750D0 File Offset: 0x002740D0
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

			// Token: 0x17000273 RID: 627
			// (get) Token: 0x06001DBC RID: 7612 RVA: 0x00275134 File Offset: 0x00274134
			public DateTimePicker DateTimeControl
			{
				get
				{
					return base.Control as DateTimePicker;
				}
			}

			// Token: 0x17000274 RID: 628
			// (get) Token: 0x06001DBD RID: 7613 RVA: 0x00275141 File Offset: 0x00274141
			// (set) Token: 0x06001DBE RID: 7614 RVA: 0x00275154 File Offset: 0x00274154
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

			// Token: 0x040038DE RID: 14558
			private static DateTimePicker dtp;
		}
	}
}
