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
	// Token: 0x02000306 RID: 774
	public partial class dfrmPatrolReportCreate : frmN3000
	{
		// Token: 0x06001737 RID: 5943 RVA: 0x001E3458 File Offset: 0x001E2458
		public dfrmPatrolReportCreate()
		{
			this.InitializeComponent();
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x001E34C4 File Offset: 0x001E24C4
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			DateTime dateTime = (DateTime)((object[])e.Argument)[0];
			DateTime dateTime2 = (DateTime)((object[])e.Argument)[1];
			string text = (string)((object[])e.Argument)[2];
			e.Result = this.ReportCreate(dateTime, dateTime2, text);
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x001E354C File Offset: 0x001E254C
		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBar1.Value = e.ProgressPercentage;
			this.lblInfo.Text = e.ProgressPercentage.ToString();
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x001E3584 File Offset: 0x001E2584
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
			wgAppRunInfo.raiseAppRunInfoLoadNums(CommonStr.strSuccessfully);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x001E35EC File Offset: 0x001E25EC
		private void btnStop_Click(object sender, EventArgs e)
		{
			if (this.comPatrolWork != null)
			{
				this.comPatrolWork.bStopCreate = true;
			}
			if (this.comPatrolWork_Acc != null)
			{
				this.comPatrolWork_Acc.bStopCreate = true;
			}
			this.backgroundWorker1.CancelAsync();
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x001E363C File Offset: 0x001E263C
		private void dfrmShiftAttReportCreate_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.backgroundWorker1.IsBusy)
			{
				if (this.comPatrolWork != null)
				{
					this.comPatrolWork.bStopCreate = true;
				}
				if (this.comPatrolWork_Acc != null)
				{
					this.comPatrolWork_Acc.bStopCreate = true;
				}
				this.backgroundWorker1.CancelAsync();
			}
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x001E368C File Offset: 0x001E268C
		private void dfrmShiftAttReportCreate_Load(object sender, EventArgs e)
		{
			this.progressBar1.Maximum = this.totalConsumer;
			this.label1.Text = ("[ " + this.totalConsumer.ToString() + " ]").PadLeft("[ 200000 ]".Length, ' ');
			this.StartCreate();
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x001E36E8 File Offset: 0x001E26E8
		private int ReportCreate(DateTime dateStart, DateTime dateEnd, string strSql)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.ReportCreate_Acc(dateStart, dateEnd, strSql);
			}
			int num = -1;
			num = 0;
			try
			{
				if (num == 0)
				{
					num = this.comPatrolWork.shift_work_schedule_cleardb();
				}
				if (num == 0)
				{
					num = this.comPatrolWork.shift_AttStatistic_cleardb();
				}
				if (num == 0)
				{
					this.comPatrolWork.getPatrolParam();
				}
				int num2 = 0;
				int num3 = 0;
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						Cursor.Current = Cursors.WaitCursor;
						while (sqlDataReader.Read())
						{
							if (this.comPatrolWork_Acc.bStopCreate)
							{
								return num;
							}
							int num4 = (int)sqlDataReader["f_ConsumerID"];
							num3++;
							this.backgroundWorker1.ReportProgress(num3);
							num = this.ShiftOtherDeal(num4, this.comPatrolWork, dateStart, dateEnd, ref num2);
						}
						sqlDataReader.Close();
						if (num == 0)
						{
							num = this.comPatrolWork.logCreateReport(dateStart, dateEnd, this.groupName, this.totalConsumer.ToString());
						}
					}
					return num;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x001E387C File Offset: 0x001E287C
		private int ReportCreate_Acc(DateTime dateStart, DateTime dateEnd, string strSql)
		{
			int num = -1;
			num = 0;
			try
			{
				if (num == 0)
				{
					num = this.comPatrolWork_Acc.shift_work_schedule_cleardb();
				}
				if (num == 0)
				{
					num = this.comPatrolWork_Acc.shift_AttStatistic_cleardb();
				}
				if (num == 0)
				{
					this.comPatrolWork_Acc.getPatrolParam();
				}
				int num2 = 0;
				int num3 = 0;
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						Cursor.Current = Cursors.WaitCursor;
						while (oleDbDataReader.Read())
						{
							if (this.comPatrolWork_Acc.bStopCreate)
							{
								return num;
							}
							int num4 = (int)oleDbDataReader["f_ConsumerID"];
							num3++;
							this.backgroundWorker1.ReportProgress(num3);
							num = this.ShiftOtherDeal_Acc(num4, this.comPatrolWork_Acc, dateStart, dateEnd, ref num2);
						}
						oleDbDataReader.Close();
						if (num == 0)
						{
							num = this.comPatrolWork_Acc.logCreateReport(dateStart, dateEnd, this.groupName, this.totalConsumer.ToString());
						}
					}
					return num;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x001E39FC File Offset: 0x001E29FC
		private int ShiftNornalDeal(int ConsumerID, DateTime dateStart, DateTime dateEnd)
		{
			return -1;
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x001E3A00 File Offset: 0x001E2A00
		private int ShiftOtherDeal(int currentConsumerID, comPatrol comPatrolWork, DateTime startDate, DateTime endDate, ref int bNotArrange)
		{
			int num = 0;
			int num2 = 1;
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				if (this.dtShiftWorkSchedule == null)
				{
					num = comPatrolWork.shift_work_schedule_create(out this.dtShiftWorkSchedule);
				}
				else
				{
					this.dtShiftWorkSchedule.Rows.Clear();
				}
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_work_schedule_fill(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate, ref bNotArrange);
			}
			wgTools.WriteLine(num2++.ToString());
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_work_schedule_updatebyReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_work_schedule_analyst(this.dtShiftWorkSchedule);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_work_schedule_writetodb(this.dtShiftWorkSchedule);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				if (this.dtAttReport == null)
				{
					num = comPatrolWork.shift_AttReport_Create(out this.dtAttReport);
				}
				else
				{
					this.dtAttReport.Rows.Clear();
				}
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_AttReport_Fill(this.dtAttReport, this.dtShiftWorkSchedule);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_AttReport_writetodb(this.dtAttReport);
			}
			return num;
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x001E3B80 File Offset: 0x001E2B80
		private int ShiftOtherDeal_Acc(int currentConsumerID, comPatrol_Acc comPatrolWork, DateTime startDate, DateTime endDate, ref int bNotArrange)
		{
			int num = 0;
			int num2 = 1;
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				if (this.dtShiftWorkSchedule == null)
				{
					num = comPatrolWork.shift_work_schedule_create(out this.dtShiftWorkSchedule);
				}
				else
				{
					this.dtShiftWorkSchedule.Rows.Clear();
				}
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_work_schedule_fill(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate, ref bNotArrange);
			}
			wgTools.WriteLine(num2++.ToString());
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_work_schedule_updatebyReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_work_schedule_analyst(this.dtShiftWorkSchedule);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_work_schedule_writetodb(this.dtShiftWorkSchedule);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				if (this.dtAttReport == null)
				{
					num = comPatrolWork.shift_AttReport_Create(out this.dtAttReport);
				}
				else
				{
					this.dtAttReport.Rows.Clear();
				}
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_AttReport_Fill(this.dtAttReport, this.dtShiftWorkSchedule);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comPatrolWork.shift_AttReport_writetodb(this.dtAttReport);
			}
			return num;
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x001E3D00 File Offset: 0x001E2D00
		private void StartCreate()
		{
			if (!this.backgroundWorker1.IsBusy)
			{
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.dtBegin, this.dtEnd, this.strConsumerSql });
			}
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x001E3D54 File Offset: 0x001E2D54
		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				TimeSpan timeSpan = DateTime.Now.Subtract(this.startTime);
				string text = string.Concat(new object[] { timeSpan.Hours, ":", timeSpan.Minutes, ":", timeSpan.Seconds });
				this.lblRuntime.Text = text;
			}
			catch
			{
			}
		}

		// Token: 0x04002FF4 RID: 12276
		private DataTable dtAttReport;

		// Token: 0x04002FF5 RID: 12277
		private DataTable dtShiftWorkSchedule;

		// Token: 0x04002FF6 RID: 12278
		private comPatrol comPatrolWork = new comPatrol();

		// Token: 0x04002FF7 RID: 12279
		private comPatrol_Acc comPatrolWork_Acc = new comPatrol_Acc();

		// Token: 0x04002FF8 RID: 12280
		public DateTime dtBegin;

		// Token: 0x04002FF9 RID: 12281
		public DateTime dtEnd;

		// Token: 0x04002FFA RID: 12282
		public string groupName = "";

		// Token: 0x04002FFB RID: 12283
		private DateTime startTime = DateTime.Now;

		// Token: 0x04002FFC RID: 12284
		public string strConsumerSql = "";

		// Token: 0x04002FFD RID: 12285
		public int totalConsumer;
	}
}
