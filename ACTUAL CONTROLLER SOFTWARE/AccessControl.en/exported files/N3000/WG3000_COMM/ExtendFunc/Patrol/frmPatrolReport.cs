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

namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x0200030D RID: 781
	public partial class frmPatrolReport : frmN3000
	{
		// Token: 0x060017A8 RID: 6056 RVA: 0x001EDEB4 File Offset: 0x001ECEB4
		public frmPatrolReport()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x001EDF1C File Offset: 0x001ECF1C
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

		// Token: 0x060017AA RID: 6058 RVA: 0x001EDF9C File Offset: 0x001ECF9C
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

		// Token: 0x060017AB RID: 6059 RVA: 0x001EE04C File Offset: 0x001ED04C
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
			string text3 = " SELECT   f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO ";
			string text4 = text3;
			text4 += " FROM t_b_Consumer WHERE (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers )) ";
			if (num5 > 0)
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer WHERE ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) " + string.Format(" AND t_b_Consumer.f_ConsumerID ={0:d} ", num5);
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
				text4 += " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers )) ) ";
			}
			else if (text2 != "")
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer  " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text2))) + " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers )) ) ";
			}
			else if (num4 > 0L)
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer  " + string.Format(" WHERE f_CardNO ={0:d} ", num4) + " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) ";
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
						if (form.Name == "dfrmPatrolReportCreate")
						{
							return;
						}
					}
				}
				using (dfrmPatrolReportCreate dfrmPatrolReportCreate = new dfrmPatrolReportCreate())
				{
					dfrmPatrolReportCreate.totalConsumer = num6;
					dfrmPatrolReportCreate.dtBegin = this.dtpDateFrom.Value;
					dfrmPatrolReportCreate.dtEnd = this.dtpDateTo.Value;
					dfrmPatrolReportCreate.strConsumerSql = text4;
					dfrmPatrolReportCreate.groupName = this.userControlFind1.cboFindDept.Text;
					dfrmPatrolReportCreate.ShowDialog(this);
					this.btnQuery_Click(null, null);
				}
			}
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x001EE4B4 File Offset: 0x001ED4B4
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x001EE4BC File Offset: 0x001ED4BC
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x001EE4EE File Offset: 0x001ED4EE
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvMain);
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x001EE502 File Offset: 0x001ED502
		private void btnFindOption_Click(object sender, EventArgs e)
		{
			if (this.dfrmFindOption == null)
			{
				this.dfrmFindOption = new dfrmPatrolReportFindOption();
				this.dfrmFindOption.Owner = this;
			}
			this.dfrmFindOption.Show();
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x001EE530 File Offset: 0x001ED530
		private void btnPatrolRoute_Click(object sender, EventArgs e)
		{
			using (frmPatrolRoute frmPatrolRoute = new frmPatrolRoute())
			{
				frmPatrolRoute.ShowDialog();
			}
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x001EE568 File Offset: 0x001ED568
		private void btnPatrolSetup_Click(object sender, EventArgs e)
		{
			using (dfrmPatrolSetup dfrmPatrolSetup = new dfrmPatrolSetup())
			{
				dfrmPatrolSetup.ShowDialog();
			}
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x001EE5A0 File Offset: 0x001ED5A0
		private void btnPatrolTask_Click(object sender, EventArgs e)
		{
			using (frmPatrolTaskData frmPatrolTaskData = new frmPatrolTaskData())
			{
				frmPatrolTaskData.ShowDialog();
			}
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x001EE5D8 File Offset: 0x001ED5D8
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x001EE5EC File Offset: 0x001ED5EC
		public void btnQuery_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			this.getLogCreateReport();
			if (!this.bFirstQuery && !this.bLogCreateReport)
			{
				XMessageBox.Show(this, CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strCreateInAdvance, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			this.bFirstQuery = false;
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
			string text3 = " SELECT t_d_PatrolDetailData.f_RecID, t_b_Group.f_GroupName, ";
			text3 += "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO,  t_b_Consumer.f_ConsumerName AS f_ConsumerName,  t_d_PatrolDetailData.f_PlanPatrolTime AS f_patroldate,  t_d_PatrolDetailData.f_PlanPatrolTime,  t_d_PatrolDetailData.f_RealPatrolTime, ";
			if (wgAppConfig.IsAccessDB)
			{
				text3 += string.Format("IIF(f_EventDesc=0, {0} ,IIF(f_EventDesc=1,{1}, IIF(f_EventDesc=2, {2}, IIF(f_EventDesc=3, {3}, IIF(f_EventDesc=4, {4},''))))) AS  [f_EventDesc] ,  ", new object[]
				{
					wgTools.PrepareStrNUnicode(this.getEventDescStr(0)),
					wgTools.PrepareStrNUnicode(this.getEventDescStr(1)),
					wgTools.PrepareStrNUnicode(this.getEventDescStr(2)),
					wgTools.PrepareStrNUnicode(this.getEventDescStr(3)),
					wgTools.PrepareStrNUnicode(this.getEventDescStr(4))
				});
			}
			else
			{
				text3 += string.Format("CASE WHEN f_EventDesc=0 THEN {0} ELSE ( CASE WHEN f_EventDesc=1 THEN {1} ELSE ( CASE WHEN f_EventDesc=2 THEN {2} ELSE (CASE WHEN f_EventDesc=3 THEN {3} ELSE (CASE WHEN f_EventDesc=4 THEN {4} ELSE '' END) END) END) END) END AS  [f_EventDesc] ,  ", new object[]
				{
					wgTools.PrepareStrNUnicode(this.getEventDescStr(0)),
					wgTools.PrepareStrNUnicode(this.getEventDescStr(1)),
					wgTools.PrepareStrNUnicode(this.getEventDescStr(2)),
					wgTools.PrepareStrNUnicode(this.getEventDescStr(3)),
					wgTools.PrepareStrNUnicode(this.getEventDescStr(4))
				});
			}
			text3 += " f_RouteName,  f_ReaderName,  '' as f_Description";
			string text4 = this.getSqlFindNormal(text3, "t_d_PatrolDetailData", this.getSqlOfDateTime("t_d_PatrolDetailData.f_patroldate"), num, num2, num3, text, num4, num5);
			if (flag)
			{
				text4 = text4 + " WHERE " + text2;
			}
			this.reloadData(text4);
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x001EE7E4 File Offset: 0x001ED7E4
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

		// Token: 0x060017B6 RID: 6070 RVA: 0x001EE938 File Offset: 0x001ED938
		private void btnStatistics_Click(object sender, EventArgs e)
		{
			using (frmPatrolStatistics frmPatrolStatistics = new frmPatrolStatistics())
			{
				frmPatrolStatistics.ShowDialog(this);
			}
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x001EE970 File Offset: 0x001ED970
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
			string text3 = " SELECT   f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO ";
			string text4 = text3;
			text4 += " FROM t_b_Consumer WHERE ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) ";
			if (num5 > 0)
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer WHERE ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers )) ) " + string.Format(" AND t_b_Consumer.f_ConsumerID ={0:d} ", num5);
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
				text4 += " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers )) ) ";
			}
			else if (text2 != "")
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer  " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text2))) + " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) ";
			}
			else if (num4 > 0L)
			{
				text4 = text3;
				text4 = text4 + " FROM t_b_Consumer  " + string.Format(" WHERE f_CardNO ={0:d} ", num4) + " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) ";
			}
			dfrmUserSelected dfrmUserSelected = new dfrmUserSelected();
			if (dfrmUserSelected.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}
			if (string.IsNullOrEmpty(dfrmUserSelected.selectedUsers))
			{
				return;
			}
			text4 = text3;
			text4 = text4 + " FROM t_b_Consumer  " + string.Format(" WHERE f_ConsumerID IN ({0}) AND {1} ", dfrmUserSelected.selectedUsers, "   ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) ");
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
					goto IL_03A3;
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
			IL_03A3:
			if (flag)
			{
				if (base.OwnedForms.Length > 0)
				{
					foreach (Form form in base.OwnedForms)
					{
						if (form.Name == "dfrmPatrolReportCreate")
						{
							return;
						}
					}
				}
				using (dfrmPatrolReportCreate dfrmPatrolReportCreate = new dfrmPatrolReportCreate())
				{
					dfrmPatrolReportCreate.totalConsumer = num6;
					dfrmPatrolReportCreate.dtBegin = this.dtpDateFrom.Value;
					dfrmPatrolReportCreate.dtEnd = this.dtpDateTo.Value;
					dfrmPatrolReportCreate.strConsumerSql = text4;
					dfrmPatrolReportCreate.groupName = this.userControlFind1.cboFindDept.Text;
					dfrmPatrolReportCreate.ShowDialog(this);
					this.btnQuery_Click(null, null);
				}
			}
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x001EEE20 File Offset: 0x001EDE20
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

		// Token: 0x060017B9 RID: 6073 RVA: 0x001EEF60 File Offset: 0x001EDF60
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
						this.dgvMain.Columns[num].Name = dt.Columns[num].ColumnName;
						num++;
					}
					wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_PatrolDate", wgTools.DisplayFormat_DateYMDWeek);
					wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_PlanPatrolTime", "HH:mm");
					wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_RealPatrolTime", "HH:mm:ss");
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

		// Token: 0x060017BA RID: 6074 RVA: 0x001EF138 File Offset: 0x001EE138
		private void frmShiftAttReport_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x001EF13C File Offset: 0x001EE13C
		private void frmSwipeRecords_Load(object sender, EventArgs e)
		{
			this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName.HeaderText);
			this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
			this.loadOperatorPrivilege();
			this.dtpDateFrom = new frmPatrolReport.ToolStripDateTime();
			this.dtpDateTo = new frmPatrolReport.ToolStripDateTime();
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
			this.Refresh();
			this.userControlFind1.btnQuery.PerformClick();
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x001EF33C File Offset: 0x001EE33C
		public string getEventDescStr(int code)
		{
			switch (code)
			{
			case 0:
				return CommonStr.strPatrolEventRest;
			case 1:
				return CommonStr.strPatrolEventNormal;
			case 2:
				return CommonStr.strPatrolEventEarly;
			case 3:
				return CommonStr.strPatrolEventLate;
			case 4:
				return CommonStr.strPatrolEventAbsence;
			default:
				return "";
			}
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x001EF38C File Offset: 0x001EE38C
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

		// Token: 0x060017BE RID: 6078 RVA: 0x001EF4A0 File Offset: 0x001EE4A0
		public void getLogCreateReport_Acc()
		{
			this.bLogCreateReport = false;
			string text = "SELECT * FROM  t_a_SystemParam WHERE [f_NO]=29 ";
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

		// Token: 0x060017BF RID: 6079 RVA: 0x001EF5A4 File Offset: 0x001EE5A4
		private string getSqlFindNormal(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
		{
			string text = "";
			try
			{
				string text2 = "";
				if (!string.IsNullOrEmpty(strTimeCon))
				{
					text2 += string.Format("AND {0}", strTimeCon);
				}
				if (findConsumerID > 0)
				{
					text2 += string.Format("AND   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
					return strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  LEFT JOIN t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID)) LEFT JOIN t_d_PatrolRouteList on ( t_d_PatrolRouteList.f_RouteID = {0}.f_RouteID))  LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  )  ", fromMainDt, text2);
				}
				if (!string.IsNullOrEmpty(findName))
				{
					text2 += string.Format(" AND t_b_Consumer.f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", findName)));
				}
				if (findCard > 0L)
				{
					text2 += string.Format(" AND t_b_Consumer.f_CardNO ={0:d} ", findCard);
				}
				if (groupMinNO > 0)
				{
					if (groupMinNO >= groupMaxNO)
					{
						return strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID)) LEFT JOIN t_d_PatrolRouteList on ( t_d_PatrolRouteList.f_RouteID = {0}.f_RouteID)) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, text2, string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO));
					}
					return strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID)) LEFT JOIN t_d_PatrolRouteList on ( t_d_PatrolRouteList.f_RouteID = {0}.f_RouteID)) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, text2, string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO));
				}
				else
				{
					text = strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID)) LEFT JOIN t_d_PatrolRouteList on ( t_d_PatrolRouteList.f_RouteID = {0}.f_RouteID)) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, text2);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return text;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x001EF70C File Offset: 0x001EE70C
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

		// Token: 0x060017C1 RID: 6081 RVA: 0x001EF7E8 File Offset: 0x001EE7E8
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
				strSql += string.Format(" AND t_d_PatrolDetailData.f_RecID > {0:d}", this.recIdMax);
			}
			else
			{
				strSql += string.Format(" WHERE t_d_PatrolDetailData.f_RecID > {0:d}", this.recIdMax);
			}
			strSql += " ORDER BY t_d_PatrolDetailData.f_RecID ";
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

		// Token: 0x060017C2 RID: 6082 RVA: 0x001EFA58 File Offset: 0x001EEA58
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

		// Token: 0x060017C3 RID: 6083 RVA: 0x001EFBA4 File Offset: 0x001EEBA4
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuPatrolDetailData";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnCreateReport.Visible = false;
			}
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x001EFBD4 File Offset: 0x001EEBD4
		private void loadStyle()
		{
			this.dgvMain.AutoGenerateColumns = false;
			this.arrColsName.Clear();
			this.arrColsShow.Clear();
			for (int i = 0; i < this.dgvMain.ColumnCount; i++)
			{
				this.arrColsName.Add(this.dgvMain.Columns[i].HeaderText);
				this.arrColsShow.Add(this.dgvMain.Columns[i].Visible);
			}
			string text = "";
			string text2 = "";
			for (int j = 0; j < this.arrColsName.Count; j++)
			{
				if (text != "")
				{
					text += ",";
					text2 += ",";
				}
				text += this.arrColsName[j];
				text2 += this.arrColsShow[j].ToString();
			}
			string keyVal = wgAppConfig.GetKeyVal(base.Name + "-" + this.dgvMain.Tag);
			if (keyVal != "")
			{
				string[] array = keyVal.Split(new char[] { ';' });
				if (array.Length == 2 && text == array[0] && text2 != array[1])
				{
					string[] array2 = array[1].Split(new char[] { ',' });
					if (array2.Length == this.arrColsName.Count)
					{
						this.arrColsShow.Clear();
						for (int k = 0; k < this.dgvMain.ColumnCount; k++)
						{
							this.dgvMain.Columns[k].Visible = bool.Parse(array2[k]);
							this.arrColsShow.Add(this.dgvMain.Columns[k].Visible);
						}
					}
				}
			}
			wgAppConfig.ReadGVStyle(this, this.dgvMain);
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x001EFDF0 File Offset: 0x001EEDF0
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

		// Token: 0x060017C6 RID: 6086 RVA: 0x001EFE76 File Offset: 0x001EEE76
		private void restoreDefaultLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.RestoreGVStyle(this, this.dgvMain);
			wgAppConfig.UpdateKeyVal(base.Name + "-" + this.dgvMain.Tag, "");
			this.loadDefaultStyle();
			this.loadStyle();
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x001EFEB8 File Offset: 0x001EEEB8
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

		// Token: 0x060017C8 RID: 6088 RVA: 0x001EFF90 File Offset: 0x001EEF90
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

		// Token: 0x060017C9 RID: 6089 RVA: 0x001F00C5 File Offset: 0x001EF0C5
		private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.SaveDGVStyle(this, this.dgvMain);
			XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
		}

		// Token: 0x040030D1 RID: 12497
		private bool bLoadedFinished;

		// Token: 0x040030D2 RID: 12498
		private bool bLogCreateReport;

		// Token: 0x040030D3 RID: 12499
		private dfrmPatrolReportFindOption dfrmFindOption;

		// Token: 0x040030D4 RID: 12500
		private DateTime logDateEnd;

		// Token: 0x040030D5 RID: 12501
		private DateTime logDateStart;

		// Token: 0x040030D6 RID: 12502
		private int recIdMax;

		// Token: 0x040030D7 RID: 12503
		private int startRecordIndex;

		// Token: 0x040030D8 RID: 12504
		private DataTable table;

		// Token: 0x040030D9 RID: 12505
		private ArrayList arrColsName = new ArrayList();

		// Token: 0x040030DA RID: 12506
		private ArrayList arrColsShow = new ArrayList();

		// Token: 0x040030DB RID: 12507
		private bool bFirstQuery = true;

		// Token: 0x040030DC RID: 12508
		private string dgvSql = "";

		// Token: 0x040030DD RID: 12509
		private DataSet dsDefaultStyle = new DataSet("DGV_STILE");

		// Token: 0x040030DE RID: 12510
		private int MaxRecord = 1000;

		// Token: 0x040030EF RID: 12527
		private frmPatrolReport.ToolStripDateTime dtpDateFrom;

		// Token: 0x040030F0 RID: 12528
		private frmPatrolReport.ToolStripDateTime dtpDateTo;

		// Token: 0x0200030E RID: 782
		public class ToolStripDateTime : ToolStripControlHost
		{
			// Token: 0x060017CC RID: 6092 RVA: 0x001F0EFB File Offset: 0x001EFEFB
			public ToolStripDateTime()
				: base(frmPatrolReport.ToolStripDateTime.dtp = new DateTimePicker())
			{
			}

			// Token: 0x060017CD RID: 6093 RVA: 0x001F0F0E File Offset: 0x001EFF0E
			protected override void Dispose(bool disposing)
			{
				if (disposing && frmPatrolReport.ToolStripDateTime.dtp != null)
				{
					frmPatrolReport.ToolStripDateTime.dtp.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x060017CE RID: 6094 RVA: 0x001F0F2C File Offset: 0x001EFF2C
			public void SetTimeFormat()
			{
				DateTimePicker dateTimePicker = base.Control as DateTimePicker;
				dateTimePicker.CustomFormat = "HH;mm";
				dateTimePicker.Format = DateTimePickerFormat.Custom;
				dateTimePicker.ShowUpDown = true;
			}

			// Token: 0x170001C8 RID: 456
			// (get) Token: 0x060017CF RID: 6095 RVA: 0x001F0F60 File Offset: 0x001EFF60
			// (set) Token: 0x060017D0 RID: 6096 RVA: 0x001F0F88 File Offset: 0x001EFF88
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

			// Token: 0x170001C9 RID: 457
			// (get) Token: 0x060017D1 RID: 6097 RVA: 0x001F0FEC File Offset: 0x001EFFEC
			public DateTimePicker DateTimeControl
			{
				get
				{
					return base.Control as DateTimePicker;
				}
			}

			// Token: 0x170001CA RID: 458
			// (get) Token: 0x060017D2 RID: 6098 RVA: 0x001F0FF9 File Offset: 0x001EFFF9
			// (set) Token: 0x060017D3 RID: 6099 RVA: 0x001F100C File Offset: 0x001F000C
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

			// Token: 0x04003104 RID: 12548
			private static DateTimePicker dtp;
		}
	}
}
