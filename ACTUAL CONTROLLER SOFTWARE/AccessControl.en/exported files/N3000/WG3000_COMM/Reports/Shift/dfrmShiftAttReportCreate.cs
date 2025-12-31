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

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200036E RID: 878
	public partial class dfrmShiftAttReportCreate : frmN3000
	{
		// Token: 0x06001CB1 RID: 7345 RVA: 0x0025E8DC File Offset: 0x0025D8DC
		public dfrmShiftAttReportCreate()
		{
			this.InitializeComponent();
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x0025E948 File Offset: 0x0025D948
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

		// Token: 0x06001CB3 RID: 7347 RVA: 0x0025E9D0 File Offset: 0x0025D9D0
		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBar1.Value = e.ProgressPercentage;
			this.lblInfo.Text = e.ProgressPercentage.ToString();
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x0025EA08 File Offset: 0x0025DA08
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

		// Token: 0x06001CB5 RID: 7349 RVA: 0x0025EA70 File Offset: 0x0025DA70
		private void btnStop_Click(object sender, EventArgs e)
		{
			if (this.comShiftWork != null)
			{
				this.comShiftWork.bStopCreate = true;
			}
			if (this.comShiftWork_Acc != null)
			{
				this.comShiftWork_Acc.bStopCreate = true;
			}
			this.backgroundWorker1.CancelAsync();
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x0025EAC0 File Offset: 0x0025DAC0
		private void dfrmShiftAttReportCreate_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.backgroundWorker1.IsBusy)
			{
				if (this.comShiftWork != null)
				{
					this.comShiftWork.bStopCreate = true;
				}
				if (this.comShiftWork_Acc != null)
				{
					this.comShiftWork_Acc.bStopCreate = true;
				}
				this.backgroundWorker1.CancelAsync();
			}
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x0025EB10 File Offset: 0x0025DB10
		private void dfrmShiftAttReportCreate_Load(object sender, EventArgs e)
		{
			this.progressBar1.Maximum = this.totalConsumer;
			this.label1.Text = ("[ " + this.totalConsumer.ToString() + " ]").PadLeft("[ 200000 ]".Length, ' ');
			this.StartCreate();
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x0025EB6C File Offset: 0x0025DB6C
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
					num = this.comShiftWork.shift_work_schedule_cleardb();
				}
				if (num == 0)
				{
					num = this.comShiftWork.shift_AttReport_cleardb();
				}
				if (num == 0)
				{
					num = this.comShiftWork.shift_AttStatistic_cleardb();
				}
				if (num == 0)
				{
					this.comShiftWork.getAttendenceParam();
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
						bool paramValBoolByNO = wgAppConfig.getParamValBoolByNO(113);
						while (sqlDataReader.Read())
						{
							if (this.comShiftWork.bStopCreate)
							{
								return num;
							}
							int num4 = (int)sqlDataReader["f_ConsumerID"];
							num3++;
							this.backgroundWorker1.ReportProgress(num3);
							if (paramValBoolByNO && (byte)sqlDataReader["f_ShiftEnabled"] > 0)
							{
								num = this.ShiftOtherDeal(num4, this.comShiftWork, dateStart, dateEnd, ref num2);
								if (num != 0)
								{
									if ((num2 & 1) > 0)
									{
										XMessageBox.Show(string.Concat(new object[]
										{
											sqlDataReader["f_ConsumerName"],
											"\r\n\r\n",
											CommonStr.strNotArrange,
											"\r\n",
											CommonStr.strReArrange
										}));
										break;
									}
									if ((num2 & 2) > 0)
									{
										XMessageBox.Show(string.Concat(new object[]
										{
											sqlDataReader["f_ConsumerName"],
											"\r\n\r\n",
											CommonStr.strNotArrange,
											"\r\n",
											CommonStr.strReArrange
										}));
										break;
									}
									if ((num2 & 4) > 0)
									{
										XMessageBox.Show(string.Concat(new object[]
										{
											sqlDataReader["f_ConsumerName"],
											"\r\n\r\n",
											CommonStr.strArrangeShiftIDNotExisted,
											"\r\n",
											CommonStr.strReArrange
										}));
										break;
									}
									break;
								}
							}
							else
							{
								using (comCreateAttendenceData comCreateAttendenceData = new comCreateAttendenceData())
								{
									comCreateAttendenceData.startDateTime = dateStart;
									comCreateAttendenceData.endDateTime = dateEnd;
									comCreateAttendenceData.strConsumerSql = strSql + "AND f_ConsumerID = " + num4.ToString();
									comCreateAttendenceData.make();
								}
							}
						}
						sqlDataReader.Close();
						if (num == 0)
						{
							num = this.comShiftWork.logCreateReport(dateStart, dateEnd, this.groupName, this.totalConsumer.ToString());
						}
					}
					return num;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x0025EEA4 File Offset: 0x0025DEA4
		private int ReportCreate_Acc(DateTime dateStart, DateTime dateEnd, string strSql)
		{
			int num = -1;
			num = 0;
			try
			{
				if (num == 0)
				{
					num = this.comShiftWork_Acc.shift_work_schedule_cleardb();
				}
				if (num == 0)
				{
					num = this.comShiftWork_Acc.shift_AttReport_cleardb();
				}
				if (num == 0)
				{
					num = this.comShiftWork_Acc.shift_AttStatistic_cleardb();
				}
				if (num == 0)
				{
					this.comShiftWork_Acc.getAttendenceParam();
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
						bool paramValBoolByNO = wgAppConfig.getParamValBoolByNO(113);
						while (oleDbDataReader.Read())
						{
							if (this.comShiftWork_Acc.bStopCreate)
							{
								return num;
							}
							int num4 = (int)oleDbDataReader["f_ConsumerID"];
							num3++;
							this.backgroundWorker1.ReportProgress(num3);
							if (paramValBoolByNO && (byte)oleDbDataReader["f_ShiftEnabled"] > 0)
							{
								num = this.ShiftOtherDeal_Acc(num4, this.comShiftWork_Acc, dateStart, dateEnd, ref num2);
								if (num != 0)
								{
									if ((num2 & 1) > 0)
									{
										XMessageBox.Show(string.Concat(new object[]
										{
											oleDbDataReader["f_ConsumerName"],
											"\r\n\r\n",
											CommonStr.strNotArrange,
											"\r\n",
											CommonStr.strReArrange
										}));
										break;
									}
									if ((num2 & 2) > 0)
									{
										XMessageBox.Show(string.Concat(new object[]
										{
											oleDbDataReader["f_ConsumerName"],
											"\r\n\r\n",
											CommonStr.strNotArrange,
											"\r\n",
											CommonStr.strReArrange
										}));
										break;
									}
									if ((num2 & 4) > 0)
									{
										XMessageBox.Show(string.Concat(new object[]
										{
											oleDbDataReader["f_ConsumerName"],
											"\r\n\r\n",
											CommonStr.strArrangeShiftIDNotExisted,
											"\r\n",
											CommonStr.strReArrange
										}));
										break;
									}
									break;
								}
							}
							else
							{
								using (comCreateAttendenceData_Acc comCreateAttendenceData_Acc = new comCreateAttendenceData_Acc())
								{
									comCreateAttendenceData_Acc.startDateTime = dateStart;
									comCreateAttendenceData_Acc.endDateTime = dateEnd;
									comCreateAttendenceData_Acc.strConsumerSql = strSql + "AND f_ConsumerID = " + num4.ToString();
									comCreateAttendenceData_Acc.make();
								}
							}
						}
						oleDbDataReader.Close();
						if (num == 0)
						{
							num = this.comShiftWork_Acc.logCreateReport(dateStart, dateEnd, this.groupName, this.totalConsumer.ToString());
						}
					}
					return num;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x0025F1CC File Offset: 0x0025E1CC
		private int ShiftNornalDeal(int ConsumerID, DateTime dateStart, DateTime dateEnd)
		{
			return -1;
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x0025F1D0 File Offset: 0x0025E1D0
		private int ShiftOtherDeal(int currentConsumerID, comShift comShiftWork, DateTime startDate, DateTime endDate, ref int bNotArrange)
		{
			int num = 0;
			int num2 = 1;
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				if (this.dtShiftWorkSchedule == null)
				{
					num = comShiftWork.shift_work_schedule_create(out this.dtShiftWorkSchedule);
				}
				else
				{
					this.dtShiftWorkSchedule.Rows.Clear();
				}
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_work_schedule_fill(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate, ref bNotArrange);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0 && bNotArrange != 0)
			{
				num = -1;
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_work_schedule_updatebyReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_work_schedule_updatebyManualReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_work_schedule_updatebyLeave(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_work_schedule_analyst(this.dtShiftWorkSchedule);
			}
			wgTools.WriteLine(num2++.ToString());
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				if (this.dtAttReport == null)
				{
					num = comShiftWork.shift_AttReport_Create(out this.dtAttReport);
				}
				else
				{
					this.dtAttReport.Rows.Clear();
				}
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_AttReport_Fill(this.dtAttReport, this.dtShiftWorkSchedule);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_AttReport_writetodb(this.dtAttReport);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				if (this.dtAttStatistic == null)
				{
					num = comShiftWork.shift_AttStatistic_Create(out this.dtAttStatistic);
				}
				else
				{
					this.dtAttStatistic.Rows.Clear();
				}
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_AttStatistic_Fill(this.dtAttStatistic, this.dtAttReport);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_AttStatistic_writetodb(this.dtAttStatistic);
			}
			wgTools.WriteLine(num2++.ToString());
			wgTools.WriteLine(num2++.ToString());
			return num;
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x0025F448 File Offset: 0x0025E448
		private int ShiftOtherDeal_Acc(int currentConsumerID, comShift_Acc comShiftWork, DateTime startDate, DateTime endDate, ref int bNotArrange)
		{
			int num = 0;
			int num2 = 1;
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				if (this.dtShiftWorkSchedule == null)
				{
					num = comShiftWork.shift_work_schedule_create(out this.dtShiftWorkSchedule);
				}
				else
				{
					this.dtShiftWorkSchedule.Rows.Clear();
				}
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_work_schedule_fill(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate, ref bNotArrange);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0 && bNotArrange != 0)
			{
				num = -1;
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_work_schedule_updatebyReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_work_schedule_updatebyManualReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_work_schedule_updatebyLeave(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_work_schedule_analyst(this.dtShiftWorkSchedule);
			}
			wgTools.WriteLine(num2++.ToString());
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				if (this.dtAttReport == null)
				{
					num = comShiftWork.shift_AttReport_Create(out this.dtAttReport);
				}
				else
				{
					this.dtAttReport.Rows.Clear();
				}
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_AttReport_Fill(this.dtAttReport, this.dtShiftWorkSchedule);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_AttReport_writetodb(this.dtAttReport);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				if (this.dtAttStatistic == null)
				{
					num = comShiftWork.shift_AttStatistic_Create(out this.dtAttStatistic);
				}
				else
				{
					this.dtAttStatistic.Rows.Clear();
				}
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_AttStatistic_Fill(this.dtAttStatistic, this.dtAttReport);
			}
			wgTools.WriteLine(num2++.ToString());
			if (num == 0)
			{
				num = comShiftWork.shift_AttStatistic_writetodb(this.dtAttStatistic);
			}
			wgTools.WriteLine(num2++.ToString());
			wgTools.WriteLine(num2++.ToString());
			return num;
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x0025F6C0 File Offset: 0x0025E6C0
		private void StartCreate()
		{
			if (!this.backgroundWorker1.IsBusy)
			{
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.dtBegin, this.dtEnd, this.strConsumerSql });
			}
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x0025F714 File Offset: 0x0025E714
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

		// Token: 0x04003749 RID: 14153
		private DataTable dtAttReport;

		// Token: 0x0400374A RID: 14154
		private DataTable dtAttStatistic;

		// Token: 0x0400374B RID: 14155
		private DataTable dtShiftWorkSchedule;

		// Token: 0x0400374C RID: 14156
		private comShift comShiftWork = new comShift();

		// Token: 0x0400374D RID: 14157
		private comShift_Acc comShiftWork_Acc = new comShift_Acc();

		// Token: 0x0400374E RID: 14158
		public DateTime dtBegin;

		// Token: 0x0400374F RID: 14159
		public DateTime dtEnd;

		// Token: 0x04003750 RID: 14160
		public string groupName = "";

		// Token: 0x04003751 RID: 14161
		private DateTime startTime = DateTime.Now;

		// Token: 0x04003752 RID: 14162
		public string strConsumerSql = "";

		// Token: 0x04003753 RID: 14163
		public int totalConsumer;
	}
}
