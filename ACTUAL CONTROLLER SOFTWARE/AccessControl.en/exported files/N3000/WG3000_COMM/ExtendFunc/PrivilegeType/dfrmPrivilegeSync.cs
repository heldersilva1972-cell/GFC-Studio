using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x02000318 RID: 792
	public partial class dfrmPrivilegeSync : frmN3000
	{
		// Token: 0x06001858 RID: 6232 RVA: 0x001FC004 File Offset: 0x001FB004
		public dfrmPrivilegeSync()
		{
			this.InitializeComponent();
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x001FC058 File Offset: 0x001FB058
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

		// Token: 0x0600185A RID: 6234 RVA: 0x001FC0E0 File Offset: 0x001FB0E0
		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBar1.Value = e.ProgressPercentage;
			this.lblInfo.Text = e.ProgressPercentage.ToString();
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x001FC118 File Offset: 0x001FB118
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
			wgAppConfig.wgLog(this.Text + "  " + CommonStr.strSuccessfully);
			XMessageBox.Show(this, string.Format("{0} {1}", this.Text, CommonStr.strSuccessfully), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x001FC1BC File Offset: 0x001FB1BC
		private void btnStop_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnStop.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				this.backgroundWorker1.CancelAsync();
				base.DialogResult = DialogResult.Cancel;
				base.Close();
			}
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x001FC211 File Offset: 0x001FB211
		private void dfrmShiftAttReportCreate_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.backgroundWorker1.IsBusy)
			{
				this.backgroundWorker1.CancelAsync();
			}
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x001FC22C File Offset: 0x001FB22C
		private void dfrmShiftAttReportCreate_Load(object sender, EventArgs e)
		{
			string text = "SELECT TOP 1 f_ConsumerID From t_b_Consumer order by f_ConsumerID ASC";
			int valBySql = wgAppConfig.getValBySql(text);
			text = "SELECT TOP 1 f_ConsumerID From t_b_Consumer order by f_ConsumerID Desc";
			int valBySql2 = wgAppConfig.getValBySql(text);
			this.totalConsumer = valBySql2 - valBySql + 1;
			this.progressBar1.Maximum = this.totalConsumer;
			this.label1.Text = ("[ " + this.totalConsumer.ToString() + " ]").PadLeft("[ 200000 ]".Length, ' ');
			wgAppConfig.wgLog(this.Text);
			this.StartCreate();
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x001FC2B8 File Offset: 0x001FB2B8
		private int ReportCreate(DateTime dateStart, DateTime dateEnd, string strSql2)
		{
			if (wgAppConfig.IsAccessDB)
			{
				wgAppConfig.runUpdateSql("Delete From t_d_Privilege");
			}
			else
			{
				wgAppConfig.runUpdateSql("TRUNCATE TABLE t_d_Privilege");
			}
			try
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				dbConnection.Open();
				string text = "SELECT TOP 1 f_ConsumerID From t_b_Consumer order by f_ConsumerID ASC";
				dbCommand.CommandText = text;
				int num = (int)dbCommand.ExecuteScalar();
				text = "SELECT TOP 1 f_ConsumerID From t_b_Consumer order by f_ConsumerID Desc";
				dbCommand.CommandText = text;
				int num2 = (int)dbCommand.ExecuteScalar();
				int i = num;
				int num3 = 500;
				while (i <= num2)
				{
					this.backgroundWorker1.ReportProgress(i - num);
					dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					text = "INSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])";
					text = text + " SELECT t_b_Consumer.f_ConsumerID, t_d_Privilege_Of_PrivilegeType.f_DoorID,t_d_Privilege_Of_PrivilegeType.[f_ControlSegID] , t_d_Privilege_Of_PrivilegeType.[f_ControllerID], t_d_Privilege_Of_PrivilegeType.[f_DoorNO]  FROM t_d_Privilege_Of_PrivilegeType, t_b_Consumer  WHERE  (t_d_Privilege_Of_PrivilegeType.f_ConsumerID)= t_b_Consumer.f_PrivilegeTypeID " + string.Format(" AND t_b_Consumer.f_ConsumerID >={0} AND t_b_Consumer.f_ConsumerID < {1}", i, i + num3);
					dbCommand.CommandText = text;
					if (dbCommand.ExecuteNonQuery() <= 0)
					{
						if (i + num3 > num2)
						{
							break;
						}
						text = string.Format("SELECT TOP 1 f_ConsumerID From t_b_Consumer WHERE t_b_Consumer.f_ConsumerID >={0} order by f_ConsumerID ASC ", i + num3);
						dbCommand.CommandText = text;
						i = int.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar()));
						if (i <= 0)
						{
							break;
						}
					}
					else
					{
						i += num3;
					}
				}
				dbConnection.Close();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return 0;
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x001FC454 File Offset: 0x001FB454
		private void StartCreate()
		{
			if (!this.backgroundWorker1.IsBusy)
			{
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.dtBegin, this.dtEnd, this.strConsumerSql });
			}
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x001FC4A8 File Offset: 0x001FB4A8
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

		// Token: 0x04003200 RID: 12800
		public DateTime dtBegin;

		// Token: 0x04003201 RID: 12801
		public DateTime dtEnd;

		// Token: 0x04003202 RID: 12802
		public string groupName = "";

		// Token: 0x04003203 RID: 12803
		private DateTime startTime = DateTime.Now;

		// Token: 0x04003204 RID: 12804
		public string strConsumerSql = "";

		// Token: 0x04003205 RID: 12805
		public int totalConsumer;
	}
}
