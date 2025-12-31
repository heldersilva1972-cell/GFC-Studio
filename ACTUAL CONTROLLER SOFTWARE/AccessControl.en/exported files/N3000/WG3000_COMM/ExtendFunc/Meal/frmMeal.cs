using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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

namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002FA RID: 762
	public partial class frmMeal : frmN3000
	{
		// Token: 0x06001641 RID: 5697 RVA: 0x001C50C0 File Offset: 0x001C40C0
		public frmMeal()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvGroupSubTotal);
			wgAppConfig.custDataGridview(ref this.dgvStatistics);
			wgAppConfig.custDataGridview(ref this.dgvSubtotal);
			wgAppConfig.custDataGridview(ref this.dgvSwipeRecords);
			this.tabPage1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage2.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage3.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.ProgressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x001C51D8 File Offset: 0x001C41D8
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			int num = (int)((object[])e.Argument)[0];
			int num2 = (int)((object[])e.Argument)[1];
			string text = (string)((object[])e.Argument)[2];
			e.Result = this.loadSwipeRecords(num, num2, text);
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x001C5258 File Offset: 0x001C4258
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
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvSwipeRecords.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x001C5308 File Offset: 0x001C4308
		private void btnCreateReport_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			this.startDeal();
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x001C5324 File Offset: 0x001C4324
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x001C532C File Offset: 0x001C432C
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			if (this.tabControl1.SelectedIndex == 1)
			{
				wgAppConfig.exportToExcel(this.dgvSubtotal, string.Concat(new string[]
				{
					this.Text,
					" [",
					this.tabPage2.Text,
					this.strDateTitle,
					"]"
				}));
				return;
			}
			if (this.tabControl1.SelectedIndex == 2)
			{
				wgAppConfig.exportToExcel(this.dgvGroupSubTotal, string.Concat(new string[]
				{
					this.Text,
					" [",
					this.tabPage4.Text,
					this.strDateTitle,
					"]"
				}));
				return;
			}
			if (this.tabControl1.SelectedIndex == 3)
			{
				wgAppConfig.exportToExcel(this.dgvStatistics, this.Text + " [" + this.tabPage3.Text + "]");
				return;
			}
			wgAppConfig.exportToExcel(this.dgvSwipeRecords, string.Concat(new string[]
			{
				this.Text,
				" [",
				this.tabPage1.Text,
				this.strDateTitle,
				"]"
			}));
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x001C5470 File Offset: 0x001C4470
		private void btnFind_Click(object sender, EventArgs e)
		{
			if (!(base.ActiveControl is TabPage) && !(base.ActiveControl is TabControl))
			{
				wgAppConfig.findCall(this.dfrmFind1, this, this.dgvSwipeRecords);
				return;
			}
			if (this.tabControl1.SelectedIndex == 1)
			{
				wgAppConfig.findCall(this.dfrmFind1, this, this.dgvSubtotal);
				return;
			}
			if (this.tabControl1.SelectedIndex == 2)
			{
				wgAppConfig.findCall(this.dfrmFind1, this, this.dgvGroupSubTotal);
				return;
			}
			if (this.tabControl1.SelectedIndex == 3)
			{
				wgAppConfig.findCall(this.dfrmFind1, this, this.dgvStatistics);
				return;
			}
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvSwipeRecords);
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x001C5520 File Offset: 0x001C4520
		private void btnMealSetup_Click(object sender, EventArgs e)
		{
			using (dfrmMealSetup dfrmMealSetup = new dfrmMealSetup())
			{
				dfrmMealSetup.ShowDialog();
			}
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x001C5558 File Offset: 0x001C4558
		private void btnPrint_Click(object sender, EventArgs e)
		{
			if (this.tabControl1.SelectedIndex == 1)
			{
				wgAppConfig.printdgv(this.dgvSubtotal, string.Concat(new string[]
				{
					this.Text,
					" [",
					this.tabPage2.Text,
					this.strDateTitle,
					"]"
				}), wgAppConfig.GetKeyVal("KEY_Meal_Printer_Footer"));
				return;
			}
			if (this.tabControl1.SelectedIndex == 2)
			{
				wgAppConfig.printdgv(this.dgvGroupSubTotal, string.Concat(new string[]
				{
					this.Text,
					" [",
					this.tabPage4.Text,
					this.strDateTitle,
					"]"
				}), wgAppConfig.GetKeyVal("KEY_Meal_Printer_Footer"));
				return;
			}
			if (this.tabControl1.SelectedIndex == 3)
			{
				wgAppConfig.printdgv(this.dgvStatistics, this.Text + " [" + this.tabPage3.Text + "]", wgAppConfig.GetKeyVal("KEY_Meal_Printer_Footer"));
				return;
			}
			wgAppConfig.printdgv(this.dgvSwipeRecords, string.Concat(new string[]
			{
				this.Text,
				" [",
				this.tabPage1.Text,
				this.strDateTitle,
				"]"
			}), wgAppConfig.GetKeyVal("KEY_Meal_Printer_Footer"));
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x001C56C0 File Offset: 0x001C46C0
		public void btnQuery_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			string text2 = "";
			bool flag = false;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text3 = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
			text3 += "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_ReaderName,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll ";
			string text4 = " ( 1>0 ) ";
			if (this.getSqlOfDateTime() != "")
			{
				text4 += string.Format(" AND {0} ", this.getSqlOfDateTime());
			}
			if (flag)
			{
				text4 += string.Format(" AND {0} ", text2);
			}
			string sqlFindSwipeRecord = wgAppConfig.getSqlFindSwipeRecord(text3, "t_d_SwipeRecord", text4, num, num2, num3, text, num4, num5);
			this.reloadData(sqlFindSwipeRecord);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x001C5784 File Offset: 0x001C4784
		public void btnQuery_Click_Acc(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			string text2 = "";
			bool flag = false;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text3 = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
			text3 += "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_ReaderName,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll ";
			string text4 = " ( 1>0 ) ";
			if (this.getSqlOfDateTime() != "")
			{
				text4 += string.Format(" AND {0} ", this.getSqlOfDateTime());
			}
			if (flag)
			{
				text4 += string.Format(" AND {0} ", text2);
			}
			string sqlFindSwipeRecord = wgAppConfig.getSqlFindSwipeRecord(text3, "t_d_SwipeRecord", text4, num, num2, num3, text, num4, num5);
			this.reloadData(sqlFindSwipeRecord);
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x001C5848 File Offset: 0x001C4848
		private void columnsConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataGridView dataGridView;
			string text;
			if (this.tabControl1.SelectedIndex == 1)
			{
				dataGridView = this.dgvSubtotal;
				text = this.Text + " [" + this.tabPage2.Text + "]";
			}
			else if (this.tabControl1.SelectedIndex == 2)
			{
				dataGridView = this.dgvGroupSubTotal;
				text = this.Text + " [" + this.tabPage4.Text + "]";
			}
			else if (this.tabControl1.SelectedIndex == 3)
			{
				dataGridView = this.dgvStatistics;
				text = this.Text + " [" + this.tabPage3.Text + "]";
			}
			else
			{
				dataGridView = this.dgvSwipeRecords;
				text = this.Text + " [" + this.tabPage1.Text + "]";
			}
			dataGridView.Tag = text;
			using (dfrmSelectColumnsShow dfrmSelectColumnsShow = new dfrmSelectColumnsShow())
			{
				for (int i = 0; i < dataGridView.ColumnCount; i++)
				{
					dfrmSelectColumnsShow.chkListColumns.Items.Add(dataGridView.Columns[i].HeaderText);
					dfrmSelectColumnsShow.chkListColumns.SetItemChecked(i, dataGridView.Columns[i].Visible);
				}
				if (dfrmSelectColumnsShow.ShowDialog(this) == DialogResult.OK)
				{
					for (int j = 0; j < dataGridView.ColumnCount; j++)
					{
						dataGridView.Columns[j].Visible = dfrmSelectColumnsShow.chkListColumns.GetItemChecked(j);
					}
					this.saveColumns(dataGridView);
					if (wgAppConfig.ReadGVStyleFileExisted(this, dataGridView))
					{
						wgAppConfig.SaveDGVStyle(this, dataGridView);
					}
				}
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x001C59F8 File Offset: 0x001C49F8
		private void dgvSwipeRecords_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvSwipeRecords.Columns.Count && this.dgvSwipeRecords.Columns[e.ColumnIndex].Name.Equals("f_Desc"))
			{
				string text;
				if ((text = e.Value as string) != null && !(text == " "))
				{
					return;
				}
				DataGridViewCell dataGridViewCell = this.dgvSwipeRecords[e.ColumnIndex, e.RowIndex];
				string text2 = this.dgvSwipeRecords[e.ColumnIndex + 1, e.RowIndex].Value as string;
				if (string.IsNullOrEmpty(text2))
				{
					e.Value = "";
					dataGridViewCell.Value = "";
					return;
				}
				e.Value = new MjRec(text2.PadLeft(48, '0')).GetDetailedRecord(null, 0U);
				dataGridViewCell.Value = e.Value;
			}
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x001C5B04 File Offset: 0x001C4B04
		private void dgvSwipeRecords_Scroll(object sender, ScrollEventArgs e)
		{
			if (!this.bLoadedFinished && e.ScrollOrientation == ScrollOrientation.VerticalScroll)
			{
				wgTools.WriteLine(e.OldValue.ToString());
				wgTools.WriteLine(e.NewValue.ToString());
				if (e.NewValue > e.OldValue && (e.NewValue + 100 > this.dgvSwipeRecords.Rows.Count || e.NewValue + this.dgvSwipeRecords.Rows.Count / 10 > this.dgvSwipeRecords.Rows.Count))
				{
					if (this.startRecordIndex <= this.dgvSwipeRecords.Rows.Count)
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
						wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvSwipeRecords.Rows.Count.ToString() + "#");
					}
				}
			}
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x001C5C44 File Offset: 0x001C4C44
		private void editPrintersFooterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripMenuItem).Text;
				dfrmInputNewName.strNewName = wgAppConfig.GetKeyVal("KEY_Meal_Printer_Footer");
				dfrmInputNewName.bNotAllowNull = false;
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					wgAppConfig.UpdateKeyVal("KEY_Meal_Printer_Footer", wgTools.SetObjToStr(dfrmInputNewName.strNewName).Trim());
				}
			}
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x001C5CC0 File Offset: 0x001C4CC0
		private void fillDgv(DataTable dt)
		{
			try
			{
				if (this.dgvSwipeRecords.DataSource == null)
				{
					this.dgvSwipeRecords.DataSource = dt;
					for (int i = 0; i < dt.Columns.Count; i++)
					{
						this.dgvSwipeRecords.Columns[i].DataPropertyName = dt.Columns[i].ColumnName;
						this.dgvSwipeRecords.Columns[i].Name = dt.Columns[i].ColumnName;
					}
					wgAppConfig.setDisplayFormatDate(this.dgvSwipeRecords, "f_ReadDate", wgTools.DisplayFormat_DateYMDHMSWeek);
					wgAppConfig.ReadGVStyle(this, this.dgvSwipeRecords);
					if (this.startRecordIndex == 0 && dt.Rows.Count >= this.MaxRecord)
					{
						this.startRecordIndex += this.MaxRecord;
						wgTools.WgDebugWrite("First 1000", new object[0]);
						this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
					}
				}
				else if (dt.Rows.Count > 0)
				{
					int firstDisplayedScrollingRowIndex = this.dgvSwipeRecords.FirstDisplayedScrollingRowIndex;
					(this.dgvSwipeRecords.DataSource as DataTable).Merge(dt);
					if (firstDisplayedScrollingRowIndex > 0)
					{
						this.dgvSwipeRecords.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x001C5E68 File Offset: 0x001C4E68
		private void frmSwipeRecords_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x001C5E6A File Offset: 0x001C4E6A
		private void frmSwipeRecords_KeyDown(object sender, KeyEventArgs e)
		{
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x001C5E6C File Offset: 0x001C4E6C
		private void frmSwipeRecords_Load(object sender, EventArgs e)
		{
			this.oldStatTitle3 = this.tabPage3.Text;
			this.oldStatTitle1 = this.tabPage1.Text;
			this.oldStatTitle2 = this.tabPage2.Text;
			this.oldStatTitle4 = this.tabPage4.Text;
			this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName.HeaderText);
			this.f_DepartmentName2.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName2.HeaderText);
			this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
			this.f_ConsumerNO2.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO2.HeaderText);
			this.dataGridViewTextBoxColumn3.HeaderText = wgAppConfig.ReplaceFloorRoom(this.dataGridViewTextBoxColumn3.HeaderText);
			this.tabPage4.Text = wgAppConfig.ReplaceFloorRoom(this.tabPage4.Text);
			this.loadOperatorPrivilege();
			this.saveDefaultStyle();
			this.loadStyle();
			this.dtpDateFrom = new frmMeal.ToolStripDateTime();
			this.dtpDateTo = new frmMeal.ToolStripDateTime();
			this.dtpDateTo.Value = DateTime.Now.Date;
			this.dtpDateFrom.Value = DateTime.Now.Date;
			this.toolStrip3.Items.Clear();
			this.toolStrip3.Items.AddRange(new ToolStripItem[] { this.toolStripLabel2, this.dtpDateFrom, this.toolStripLabel3, this.dtpDateTo });
			this.userControlFind1.toolStripLabel2.Visible = false;
			this.userControlFind1.txtFindCardID.Visible = false;
			this.dtpDateFrom.BoxWidth = 120;
			this.dtpDateTo.BoxWidth = 120;
			this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName.HeaderText);
			this.userControlFind1.btnQuery.Click += this.btnQuery_Click;
			this.dtpDateFrom.BoxWidth = 150;
			this.dtpDateTo.BoxWidth = 150;
			wgAppConfig.setDisplayFormatDate(this.dtpDateFrom.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dtpDateTo.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
			this.userControlFind1.btnQuery.Visible = false;
			this.bLoadedFinished = true;
			this.btnPrint.Enabled = false;
			this.btnExportToExcel.Enabled = false;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x001C60F0 File Offset: 0x001C50F0
		private string getQueryConsumerConditionStr(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
		{
			string text = "";
			try
			{
				string text2 = " WHERE (1>0) ";
				if (findConsumerID > 0)
				{
					return strBaseInfo + string.Format("  FROM t_b_Consumer  LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) WHERE   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
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
						text = strBaseInfo + string.Format("  FROM t_b_Consumer  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {0} )  ", string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO));
					}
					else
					{
						text = strBaseInfo + string.Format("  FROM t_b_Consumer  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {0} )  ", string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO));
					}
				}
				else
				{
					text = strBaseInfo + string.Format("  FROM t_b_Consumer  LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  )  ", new object[0]);
				}
				text += text2;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return text;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x001C6228 File Offset: 0x001C5228
		private string getSqlFindSwipeRecord4Meal(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
		{
			string text = "";
			try
			{
				string text2 = "";
				string text3 = " WHERE (1>0) ";
				if (!string.IsNullOrEmpty(strTimeCon))
				{
					text3 += string.Format("AND {0}", strTimeCon);
				}
				if (findConsumerID > 0)
				{
					text2 += string.Format("AND   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
					text = strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN  t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID))  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, text2);
					return text + text3;
				}
				if (!string.IsNullOrEmpty(findName))
				{
					text3 += string.Format(" AND t_b_Consumer.f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", findName)));
				}
				if (findCard > 0L)
				{
					text3 += string.Format(" AND {0}.f_CardNO ={1:d} ", fromMainDt, findCard);
				}
				if (groupMinNO > 0)
				{
					if (groupMinNO >= groupMaxNO)
					{
						text = strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  INNER JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID))  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) )  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, text2, string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO));
					}
					else
					{
						text = strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  INNER JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID))  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) )  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, text2, string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO));
					}
				}
				else
				{
					text = strBaseInfo + string.Format(" FROM ((({0} INNER JOIN t_b_Consumer ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  INNER JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID))  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) )  LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, text2);
				}
				text += text3;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return text;
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x001C63A8 File Offset: 0x001C53A8
		private string getSqlOfDateTime()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getSqlOfDateTime_Acc();
			}
			string text = "";
			text = "  ([f_ReadDate]>= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 00:00:00") + ")";
			if (text != "")
			{
				text += " AND ";
			}
			text = text + "  ([f_ReadDate]<= " + wgTools.PrepareStr(this.dtpDateTo.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")";
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
				dbCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID = 5 ORDER BY f_ID ASC";
				dbConnection.Open();
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.Read() && (int)dbDataReader["f_Value"] > 0)
				{
					DateTime dateTime = (DateTime)dbDataReader["f_BeginHMS"];
					dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
					DateTime dateTime2 = (DateTime)dbDataReader["f_EndHMS"];
					dateTime2 = dateTime2.AddSeconds((double)(59 - dateTime2.Second));
					if (string.Compare(dateTime.ToString("HH:mm"), dateTime2.ToString("HH:mm")) >= 0)
					{
						dateTime = DateTime.Parse(this.dtpDateFrom.Value.ToString("yyyy-MM-dd ") + dateTime2.AddSeconds(1.0).ToString(" HH:mm:ss"));
						string text2 = "  ([f_ReadDate]>= " + wgTools.PrepareStr(dateTime.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd HH:mm:ss") + ")";
						if (text2 != "")
						{
							text2 += " AND ";
						}
						dateTime = DateTime.Parse(this.dtpDateTo.Value.AddDays(1.0).ToString("yyyy-MM-dd ") + dateTime2.ToString(" HH:mm:ss"));
						text = text2 + "  ([f_ReadDate]<= " + wgTools.PrepareStr(dateTime.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd HH:mm:ss") + ")";
					}
				}
				dbConnection.Close();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
			return text;
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x001C6664 File Offset: 0x001C5664
		private string getSqlOfDateTime_Acc()
		{
			string text = "";
			text = "  ([f_ReadDate]>= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 00:00:00") + ")";
			if (text != "")
			{
				text += " AND ";
			}
			text = text + "  ([f_ReadDate]<= " + wgTools.PrepareStr(this.dtpDateTo.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")";
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
				dbCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID = 5 ORDER BY f_ID ASC";
				dbConnection.Open();
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.Read() && (int)dbDataReader["f_Value"] > 0)
				{
					DateTime dateTime = (DateTime)dbDataReader["f_BeginHMS"];
					dateTime = dateTime.AddSeconds((double)(-(double)dateTime.Second));
					DateTime dateTime2 = (DateTime)dbDataReader["f_EndHMS"];
					dateTime2 = dateTime2.AddSeconds((double)(59 - dateTime2.Second));
					if (string.Compare(dateTime.ToString("HH:mm"), dateTime2.ToString("HH:mm")) >= 0)
					{
						dateTime = DateTime.Parse(this.dtpDateFrom.Value.ToString("yyyy-MM-dd ") + dateTime2.ToString(" HH:mm:ss"));
						string text2 = "  ([f_ReadDate]>= " + wgTools.PrepareStr(dateTime.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd HH:mm:ss") + ")";
						if (text2 != "")
						{
							text2 += " AND ";
						}
						dateTime = DateTime.Parse(this.dtpDateTo.Value.AddDays(1.0).ToString("yyyy-MM-dd ") + dateTime2.ToString(" HH:mm:ss"));
						text = text2 + "  ([f_ReadDate]<= " + wgTools.PrepareStr(dateTime.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd HH:mm:ss") + ")";
					}
				}
				dbConnection.Close();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
			return text;
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x001C6900 File Offset: 0x001C5900
		private void loadDefaultStyle()
		{
			for (int i = 0; i < 4; i++)
			{
				DataGridView dataGridView;
				string text;
				switch (i)
				{
				case 1:
					dataGridView = this.dgvSubtotal;
					text = this.Text + " [" + this.tabPage2.Text + "]";
					break;
				case 2:
					dataGridView = this.dgvGroupSubTotal;
					text = this.Text + " [" + this.tabPage4.Text + "]";
					break;
				case 3:
					dataGridView = this.dgvStatistics;
					text = this.Text + " [" + this.tabPage3.Text + "]";
					break;
				default:
					dataGridView = this.dgvSwipeRecords;
					text = this.Text + " [" + this.tabPage1.Text + "]";
					break;
				}
				dataGridView.Tag = text;
				DataTable dataTable = this.dsDefaultStyle.Tables[dataGridView.Name];
				for (int j = 0; j < dataTable.Rows.Count; j++)
				{
					dataGridView.Columns[j].Name = dataTable.Rows[j]["colName"].ToString();
					dataGridView.Columns[j].HeaderText = dataTable.Rows[j]["colHeader"].ToString();
					dataGridView.Columns[j].Width = int.Parse(dataTable.Rows[j]["colWidth"].ToString());
					dataGridView.Columns[j].Visible = bool.Parse(dataTable.Rows[j]["colVisable"].ToString());
					dataGridView.Columns[j].DisplayIndex = int.Parse(dataTable.Rows[j]["colDisplayIndex"].ToString());
				}
			}
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x001C6B14 File Offset: 0x001C5B14
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuConstMeal";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnMealSetup.Visible = false;
			}
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x001C6B44 File Offset: 0x001C5B44
		private void loadStyle()
		{
			for (int i = 0; i < 4; i++)
			{
				DataGridView dataGridView;
				string text;
				switch (i)
				{
				case 1:
					dataGridView = this.dgvSubtotal;
					text = this.Text + " [" + this.tabPage2.Text + "]";
					break;
				case 2:
					dataGridView = this.dgvGroupSubTotal;
					text = this.Text + " [" + this.tabPage4.Text + "]";
					break;
				case 3:
					dataGridView = this.dgvStatistics;
					text = this.Text + " [" + this.tabPage3.Text + "]";
					break;
				default:
					dataGridView = this.dgvSwipeRecords;
					text = this.Text + " [" + this.tabPage1.Text + "]";
					break;
				}
				dataGridView.Tag = text;
				string keyVal = wgAppConfig.GetKeyVal(base.Name + "-" + dataGridView.Tag);
				if (keyVal != "")
				{
					string[] array = keyVal.Split(new char[] { ';' });
					if (array.Length == 2)
					{
						string[] array2 = array[1].Split(new char[] { ',' });
						if (array2.Length == dataGridView.Columns.Count)
						{
							for (int j = 0; j < dataGridView.ColumnCount; j++)
							{
								dataGridView.Columns[j].Visible = bool.Parse(array2[j]);
							}
						}
					}
				}
				wgAppConfig.ReadGVStyle(this, dataGridView);
			}
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x001C6CD8 File Offset: 0x001C5CD8
		private DataTable loadSwipeRecords(int startIndex, int maxRecords, string strSql)
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadSwipeRecords Start");
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
					goto IL_017B;
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
			IL_017B:
			if (this.table.Rows.Count > 0)
			{
				this.recIdMin = int.Parse(this.table.Rows[this.table.Rows.Count - 1][0].ToString());
			}
			wgTools.WriteLine("da.Fill End " + startIndex.ToString());
			Cursor.Current = Cursors.Default;
			return this.table;
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x001C6F20 File Offset: 0x001C5F20
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
				this.dgvSwipeRecords.DataSource = null;
				this.timer1.Enabled = true;
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
			}
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x001C6FB2 File Offset: 0x001C5FB2
		private void restoreDefaultLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.RestoreGVStyle(this, this.dgvSwipeRecords);
			this.loadDefaultStyle();
			this.loadStyle();
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x001C6FCC File Offset: 0x001C5FCC
		private void restoreDefaultLayoutToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			DataGridView dataGridView;
			string text;
			if (this.tabControl1.SelectedIndex == 1)
			{
				dataGridView = this.dgvSubtotal;
				text = this.Text + " [" + this.tabPage2.Text + "]";
			}
			else if (this.tabControl1.SelectedIndex == 2)
			{
				dataGridView = this.dgvGroupSubTotal;
				text = this.Text + " [" + this.tabPage4.Text + "]";
			}
			else if (this.tabControl1.SelectedIndex == 3)
			{
				dataGridView = this.dgvStatistics;
				text = this.Text + " [" + this.tabPage3.Text + "]";
			}
			else
			{
				dataGridView = this.dgvSwipeRecords;
				text = this.Text + " [" + this.tabPage1.Text + "]";
			}
			dataGridView.Tag = text;
			wgAppConfig.RestoreGVStyle(this, dataGridView);
			wgAppConfig.UpdateKeyVal(base.Name + "-" + dataGridView.Tag, "");
			this.loadDefaultStyle();
			this.loadStyle();
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x001C70E6 File Offset: 0x001C60E6
		private void restoreOldTitle()
		{
			this.tabPage1.Text = this.oldStatTitle1;
			this.tabPage2.Text = this.oldStatTitle2;
			this.tabPage4.Text = this.oldStatTitle4;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x001C711C File Offset: 0x001C611C
		private void saveColumns(DataGridView dgv)
		{
			string text = "";
			string text2 = "";
			for (int i = 0; i < dgv.ColumnCount; i++)
			{
				if (text != "")
				{
					text += ",";
					text2 += ",";
				}
				text += dgv.Columns[i].HeaderText;
				text2 += dgv.Columns[i].Visible.ToString();
			}
			wgAppConfig.InsertKeyVal(base.Name + "-" + dgv.Tag, text + ";" + text2);
			wgAppConfig.UpdateKeyVal(base.Name + "-" + dgv.Tag, text + ";" + text2);
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x001C71F4 File Offset: 0x001C61F4
		private void saveDefaultStyle()
		{
			for (int i = 0; i < 4; i++)
			{
				DataGridView dataGridView;
				string text;
				switch (i)
				{
				case 1:
					dataGridView = this.dgvSubtotal;
					text = this.Text + " [" + this.tabPage2.Text + "]";
					break;
				case 2:
					dataGridView = this.dgvGroupSubTotal;
					text = this.Text + " [" + this.tabPage4.Text + "]";
					break;
				case 3:
					dataGridView = this.dgvStatistics;
					text = this.Text + " [" + this.tabPage3.Text + "]";
					break;
				default:
					dataGridView = this.dgvSwipeRecords;
					text = this.Text + " [" + this.tabPage1.Text + "]";
					break;
				}
				dataGridView.Tag = text;
				DataTable dataTable = new DataTable();
				this.dsDefaultStyle.Tables.Add(dataTable);
				dataTable.TableName = dataGridView.Name;
				dataTable.Columns.Add("colName");
				dataTable.Columns.Add("colHeader");
				dataTable.Columns.Add("colWidth");
				dataTable.Columns.Add("colVisable");
				dataTable.Columns.Add("colDisplayIndex");
				for (int j = 0; j < dataGridView.ColumnCount; j++)
				{
					DataGridViewColumn dataGridViewColumn = dataGridView.Columns[j];
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
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x001C7405 File Offset: 0x001C6405
		private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.SaveDGVStyle(this, this.dgvSwipeRecords);
			XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x001C7430 File Offset: 0x001C6430
		private void saveLayoutToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			DataGridView dataGridView;
			string text;
			if (this.tabControl1.SelectedIndex == 1)
			{
				dataGridView = this.dgvSubtotal;
				text = this.Text + " [" + this.tabPage2.Text + "]";
			}
			else if (this.tabControl1.SelectedIndex == 2)
			{
				dataGridView = this.dgvGroupSubTotal;
				text = this.Text + " [" + this.tabPage4.Text + "]";
			}
			else if (this.tabControl1.SelectedIndex == 3)
			{
				dataGridView = this.dgvStatistics;
				text = this.Text + " [" + this.tabPage3.Text + "]";
			}
			else
			{
				dataGridView = this.dgvSwipeRecords;
				text = this.Text + " [" + this.tabPage1.Text + "]";
			}
			dataGridView.Tag = text;
			wgAppConfig.SaveDGVStyle(this, dataGridView);
			XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x001C753C File Offset: 0x001C653C
		public void startDeal()
		{
			this.btnCreateReport.Enabled = false;
			if (XMessageBox.Show(string.Format(CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strAreYouSure + " {0} ?", this.btnCreateReport.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				Cursor cursor = Cursor.Current;
				string text = "";
				DbConnection dbConnection;
				DbCommand dbCommand;
				DbCommand dbCommand2;
				DbDataAdapter dbDataAdapter;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
					dbCommand2 = new OleDbCommand("", dbConnection as OleDbConnection);
					dbDataAdapter = new OleDbDataAdapter();
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
					dbCommand2 = new SqlCommand("", dbConnection as SqlConnection);
					dbDataAdapter = new SqlDataAdapter();
				}
				Cursor.Current = Cursors.WaitCursor;
				this.btnCreateReport.Enabled = false;
				try
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					string text2 = "";
					long num4 = 0L;
					int num5 = 0;
					this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text2, ref num4, ref num5);
					string text3 = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
					text3 += "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_ReaderName, t_d_SwipeRecord.f_ReaderID, t_b_Consumer.f_ConsumerID,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll ";
					string text4 = " ( 1>0 ) ";
					if (this.getSqlOfDateTime() != "")
					{
						text4 += string.Format(" AND {0} ", this.getSqlOfDateTime());
					}
					string text5 = this.getSqlFindSwipeRecord4Meal(text3, "t_d_SwipeRecord", text4, num, num2, num3, text2, num4, num5);
					this.strDateTitle = string.Format("({0} {1} {2})", this.dtpDateFrom.Value.ToString(wgTools.DisplayFormat_DateYMD), this.toolStripLabel3.Text.Replace(":", ""), this.dtpDateTo.Value.ToString(wgTools.DisplayFormat_DateYMD));
					this.tabPage3.Text = this.oldStatTitle3 + this.strDateTitle;
					this.ProgressBar1.Value = 0;
					this.ProgressBar1.Maximum = 100;
					this.ProgressBar1.Value = 30;
					this.ds = new DataSet("inout");
					dbCommand2.CommandText = text5;
					dbCommand2.CommandTimeout = 180;
					dbDataAdapter.SelectCommand = dbCommand2;
					this.ProgressBar1.Value = 40;
					dbDataAdapter.Fill(this.ds, "t_d_SwipeRecord");
					this.ProgressBar1.Value = 60;
					text5 = " SELECT a.f_GroupID,a.f_GroupName,  0 as f_MorningCount, 0 as f_LunchCount, 0 as f_EveningCount ,0 as f_OtherCount, 0 as f_groupTotalTimes, 0.01 as f_groupTotal, b.f_Enabled, b.f_Morning,b.f_Lunch, b.f_Evening,b.f_Other from t_b_Group a LEFT JOIN t_b_group4Meal b ON a.f_GroupID = b.f_GroupID ";
					if (num > 0)
					{
						if (num >= num3)
						{
							text5 += string.Format(" WHERE  a.f_GroupID ={0:d} ", num2);
						}
						else
						{
							text5 = text5 + string.Format(" WHERE  a.f_GroupID ={0:d} ", num2) + string.Format(" AND  a.f_GroupNO <={0:d} ", num3);
						}
					}
					text5 += " order by f_GroupName  + '\\' ASC";
					dbCommand2.CommandText = text5;
					dbDataAdapter.Fill(this.ds, "t_b_group4Meal");
					for (int i = 0; i <= this.ds.Tables["t_b_group4Meal"].Rows.Count - 1; i++)
					{
						if (string.IsNullOrEmpty(wgTools.SetObjToStr(this.ds.Tables["t_b_group4Meal"].Rows[i]["f_Morning"])))
						{
							this.ds.Tables["t_b_group4Meal"].Rows[i]["f_MorningCount"] = 0;
							this.ds.Tables["t_b_group4Meal"].Rows[i]["f_LunchCount"] = 0;
							this.ds.Tables["t_b_group4Meal"].Rows[i]["f_EveningCount"] = 0;
							this.ds.Tables["t_b_group4Meal"].Rows[i]["f_OtherCount"] = 0;
							this.ds.Tables["t_b_group4Meal"].Rows[i]["f_Morning"] = 0.00m;
							this.ds.Tables["t_b_group4Meal"].Rows[i]["f_Lunch"] = 0.00m;
							this.ds.Tables["t_b_group4Meal"].Rows[i]["f_Evening"] = 0.00m;
							this.ds.Tables["t_b_group4Meal"].Rows[i]["f_Other"] = 0.00m;
							this.ds.Tables["t_b_group4Meal"].Rows[i]["f_Enabled"] = 1;
						}
						this.ds.Tables["t_b_group4Meal"].Rows[i]["f_groupTotal"] = 0.00m;
						this.ds.Tables["t_b_group4Meal"].Rows[i]["f_groupTotalTimes"] = 0;
					}
					this.ds.Tables["t_b_group4Meal"].AcceptChanges();
					if (num <= 0)
					{
						DataRow dataRow = this.ds.Tables["t_b_group4Meal"].NewRow();
						dataRow["f_GroupID"] = 0;
						dataRow["f_GroupName"] = CommonStr.strDepartmentIsEmpty;
						dataRow["f_MorningCount"] = 0;
						dataRow["f_LunchCount"] = 0;
						dataRow["f_EveningCount"] = 0;
						dataRow["f_OtherCount"] = 0;
						dataRow["f_Morning"] = 0.00m;
						dataRow["f_Lunch"] = 0.00m;
						dataRow["f_Evening"] = 0.00m;
						dataRow["f_Other"] = 0.00m;
						dataRow["f_Enabled"] = 1;
						dataRow["f_groupTotalTimes"] = 0;
						dataRow["f_groupTotal"] = 0.00m;
						this.ds.Tables["t_b_group4Meal"].Rows.Add(dataRow);
						this.ds.Tables["t_b_group4Meal"].AcceptChanges();
					}
					DataView dataView = new DataView(this.ds.Tables["t_b_group4Meal"]);
					text5 = " SELECT  f_RecID, 0 as f_ConsumerID, '' AS f_GroupName, '' as f_ConsumerNO,  '' AS f_ConsumerName,t_d_SwipeRecord.f_ReadDate , '' as f_MealName, 0.01 as f_Cost, '' as [f_ReaderName],f_ReaderID  ";
					text5 += ", 0 as f_GroupID FROM t_d_SwipeRecord  WHERE 1<0 ";
					dbCommand.CommandText = text5;
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(this.ds, "MealReport");
					text5 = "  SELECT  t_d_Reader4Meal.*  ";
					text5 += " FROM  t_b_Reader,t_d_Reader4Meal, t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  AND t_b_Reader.f_ReaderID = t_d_Reader4Meal.f_ReaderID  ORDER BY  t_b_Reader.f_ReaderID  ";
					dbCommand.CommandText = text5;
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(this.ds, "t_d_Reader4Meal");
					DataView dataView2 = new DataView(this.ds.Tables["t_d_Reader4Meal"]);
					text5 = "  SELECT  t_b_Reader.f_ReaderID , t_b_Reader.[f_ReaderName], 0 as f_CostCount, 0.01 as f_CostTotal4Reader  ";
					text5 += " FROM  t_b_Reader,t_d_Reader4Meal  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  AND t_b_Reader.f_ReaderID = t_d_Reader4Meal.f_ReaderID  ORDER BY  t_b_Reader.f_ReaderID  ";
					dbCommand.CommandText = text5;
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(this.ds, "ReaderStatistics");
					DataTable dataTable = this.ds.Tables["ReaderStatistics"];
					this.dvReaderStatistics = new DataView(this.ds.Tables["ReaderStatistics"]);
					int j;
					for (j = 0; j <= this.dvReaderStatistics.Count - 1; j++)
					{
						this.dvReaderStatistics[j]["f_CostTotal4Reader"] = 0;
					}
					dataTable.AcceptChanges();
					this.ProgressBar1.Value = 70;
					if (this.dvReaderStatistics.Count > 0)
					{
						text = text + "  f_ReaderID IN ( " + this.dvReaderStatistics[0]["f_ReaderID"];
						for (int k = 1; k <= this.dvReaderStatistics.Count - 1; k++)
						{
							text = text + "," + this.dvReaderStatistics[k]["f_ReaderID"];
						}
						text += ")";
					}
					else
					{
						text += " 1<0 ";
					}
					text3 = " SELECT    f_ConsumerID, f_GroupName,  f_ConsumerNO, f_ConsumerName ";
					text3 += " , 0 as f_CostMorningCount, 0 as f_CostLunchCount, 0 as f_CostEveningCount ,0 as f_CostOtherCount, 0 as f_CostTotalCount, 0.01 as f_CostTotal,  0.01 as f_CostMorning, 0.01 as f_CostLunch, 0.01 as f_CostEvening ,0.01 as f_CostOther  , t_b_Consumer.f_GroupID ";
					text5 = this.getQueryConsumerConditionStr(text3, "", text4, num, num2, num3, text2, num4, num5);
					dbCommand.CommandText = text5;
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(this.ds, "ConsumerStatistics");
					DataTable dataTable2 = this.ds.Tables["ConsumerStatistics"];
					this.dvConsumerStatistics = new DataView(this.ds.Tables["ConsumerStatistics"]);
					for (j = 0; j <= this.dvConsumerStatistics.Count - 1; j++)
					{
						dataTable2.Rows[j]["f_CostMorning"] = 0;
						dataTable2.Rows[j]["f_CostLunch"] = 0;
						dataTable2.Rows[j]["f_CostEvening"] = 0;
						dataTable2.Rows[j]["f_CostOther"] = 0;
						dataTable2.Rows[j]["f_CostTotal"] = 0;
					}
					dataTable2.AcceptChanges();
					this.ProgressBar1.Value = 80;
					DataTable dataTable3 = this.ds.Tables["MealReport"];
					DataView dataView3 = new DataView(this.ds.Tables["t_d_SwipeRecord"]);
					text5 = "SELECT * from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) ";
					dbCommand.CommandText = text5;
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(this.ds, "t_b_reader");
					DataTable dataTable4 = this.ds.Tables["t_b_reader"];
					new DataView(dataTable4);
					int num6 = 0;
					int num7 = 0;
					bool flag = false;
					if (dbConnection.State != ConnectionState.Open)
					{
						dbConnection.Open();
					}
					dbCommand2.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID=1 ";
					DbDataReader dbDataReader = dbCommand2.ExecuteReader();
					if (dbDataReader.Read())
					{
						if (int.Parse(wgTools.SetObjToStr(dbDataReader["f_Value"])) == 1)
						{
							num7 = 1;
						}
						else
						{
							if (int.Parse(wgTools.SetObjToStr(dbDataReader["f_Value"])) == 2)
							{
								num7 = 2;
								try
								{
									num6 = (int)decimal.Parse(wgTools.SetObjToStr(dbDataReader["f_ParamVal"]), CultureInfo.InvariantCulture);
									goto IL_0B94;
								}
								catch (Exception ex)
								{
									wgTools.WgDebugWrite(ex.ToString(), new object[0]);
									goto IL_0B94;
								}
							}
							num7 = 0;
							num6 = 0;
						}
					}
					IL_0B94:
					dbDataReader.Close();
					dbCommand2.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID > 1 ORDER BY f_ID ASC";
					dbDataReader = dbCommand2.ExecuteReader();
					int[] array = new int[4];
					string[] array2 = new string[4];
					string[] array3 = new string[4];
					DateTime[] array4 = new DateTime[4];
					DateTime[] array5 = new DateTime[4];
					decimal[] array6 = new decimal[4];
					for (int l = 0; l <= array.Length - 1; l++)
					{
						array[l] = 0;
						array4[l] = DateTime.Parse("00:00");
						array5[l] = DateTime.Parse("00:00");
						array6[l] = 0m;
					}
					array2[0] = CommonStr.strMealName0;
					array2[1] = CommonStr.strMealName1;
					array2[2] = CommonStr.strMealName2;
					array2[3] = CommonStr.strMealName3;
					array3[0] = "f_CostMorning";
					array3[1] = "f_CostLunch";
					array3[2] = "f_CostEvening";
					array3[3] = "f_CostOther";
					while (dbDataReader.Read())
					{
						if ((int)dbDataReader["f_ID"] == 2 || (int)dbDataReader["f_ID"] == 3 || (int)dbDataReader["f_ID"] == 4 || (int)dbDataReader["f_ID"] == 5)
						{
							if ((int)dbDataReader["f_Value"] > 0)
							{
								int l = (int)dbDataReader["f_ID"] - 2;
								array[l] = 1;
								array4[l] = (DateTime)dbDataReader["f_BeginHMS"];
								array4[l] = array4[l].AddSeconds((double)(-(double)array4[l].Second));
								array5[l] = (DateTime)dbDataReader["f_EndHMS"];
								array5[l] = array5[l].AddSeconds((double)(59 - array5[l].Second));
								array6[l] = decimal.Parse(wgTools.SetObjToStr(dbDataReader["f_ParamVal"]), CultureInfo.InvariantCulture);
							}
						}
						else if ((int)dbDataReader["f_ID"] == 6)
						{
							if ((int)dbDataReader["f_Value"] > 0)
							{
								text += " AND f_Character=1 ";
							}
						}
						else if ((int)dbDataReader["f_ID"] == 7 && (int)dbDataReader["f_Value"] > 0)
						{
							flag = true;
						}
					}
					dbDataReader.Close();
					for (int m = 0; m < 4; m++)
					{
						this.dgvStatistics.Columns[4 + m].Visible = array[m] == 1;
					}
					int num8 = 12;
					for (int l = 0; l <= array.Length - 1; l++)
					{
						if (array[l] > 0 && l < 3)
						{
							double totalHours = DateTime.Parse(string.Format("2015-10-20 {0}", array5[l].ToString("HH:mm"))).Subtract(DateTime.Parse(string.Format("2015-10-20 {0}", array4[l].ToString("HH:mm")))).TotalHours;
							if (totalHours > (double)num8)
							{
								num8 = (int)totalHours;
								num8++;
								if (num8 > 24)
								{
									num8 = 24;
								}
							}
						}
					}
					dataView3.Sort = "f_ReadDate ASC";
					this.ProgressBar1.Value = 0;
					this.dvConsumerStatistics = new DataView(this.ds.Tables["ConsumerStatistics"]);
					this.ProgressBar1.Maximum = Math.Max(0, this.dvConsumerStatistics.Count);
					int num9 = 0;
					int num10 = 0;
					int num11 = 0;
					int num12 = 0;
					for (int n = 0; n <= this.dvConsumerStatistics.Count - 1; n++)
					{
						this.ProgressBar1.Value = n;
						dataView3.RowFilter = text + " AND  f_ConsumerID = " + this.dvConsumerStatistics[n]["f_ConsumerID"];
						if (dataView3.Count > 0)
						{
							string text6 = "";
							DateTime dateTime = DateTime.Parse("2100-1-1");
							string text7 = "";
							int num13 = -1;
							string text8 = "";
							for (int num14 = 0; num14 <= dataView3.Count - 1; num14++)
							{
								DateTime dateTime2 = (DateTime)dataView3[num14]["f_ReadDate"];
								bool flag2 = false;
								int l;
								for (l = 0; l <= array.Length - 1; l++)
								{
									if (array[l] > 0)
									{
										if (l < 3)
										{
											if (string.Compare(dateTime2.ToString("HH:mm"), array4[l].ToString("HH:mm")) >= 0 && string.Compare(dateTime2.ToString("HH:mm"), array5[l].ToString("HH:mm")) <= 0)
											{
												flag2 = true;
												text6 = array2[l];
												text8 = array3[l];
												break;
											}
										}
										else if (string.Compare(array4[l].ToString("HH:mm"), array5[l].ToString("HH:mm")) < 0)
										{
											if (string.Compare(dateTime2.ToString("HH:mm"), array4[l].ToString("HH:mm")) >= 0 && string.Compare(dateTime2.ToString("HH:mm"), array5[l].ToString("HH:mm")) <= 0)
											{
												flag2 = true;
												text6 = array2[l];
												text8 = array3[l];
												break;
											}
										}
										else if (string.Compare(dateTime2.ToString("HH:mm"), array4[l].ToString("HH:mm")) >= 0 || string.Compare(dateTime2.ToString("HH:mm"), array5[l].ToString("HH:mm")) <= 0)
										{
											flag2 = true;
											text6 = array2[l];
											text8 = array3[l];
											break;
										}
									}
								}
								if (flag2)
								{
									bool flag3 = true;
									TimeSpan timeSpan = Convert.ToDateTime(dataView3[num14]["f_ReadDate"]).Subtract(dateTime);
									if (num7 == 1 && text7 == text6 && Math.Abs(timeSpan.TotalHours) < (double)num8)
									{
										flag3 = false;
									}
									if (num7 == 2 && text7 == text6 && Math.Abs(timeSpan.TotalSeconds) < (double)num6 && num13 == (int)dataView3[num14]["f_ReaderID"])
									{
										flag3 = false;
									}
									if (flag3)
									{
										text7 = text6;
										dateTime = (DateTime)dataView3[num14]["f_ReadDate"];
										num13 = (int)dataView3[num14]["f_ReaderID"];
										DataRow dataRow2 = dataTable3.NewRow();
										dataRow2["f_RecID"] = dataView3[num14]["f_RecID"];
										dataRow2["f_GroupName"] = dataView3[num14]["f_GroupName"];
										dataRow2["f_ConsumerNO"] = dataView3[num14]["f_ConsumerNO"];
										dataRow2["f_ConsumerID"] = dataView3[num14]["f_ConsumerID"];
										dataRow2["f_ConsumerName"] = dataView3[num14]["f_ConsumerName"];
										dataRow2["f_ReaderName"] = dataView3[num14]["f_ReaderName"];
										dataRow2["f_ReaderID"] = dataView3[num14]["f_ReaderID"];
										dataRow2["f_ReadDate"] = dataView3[num14]["f_ReadDate"];
										dataRow2["f_MealName"] = text6;
										dataRow2["f_Cost"] = array6[l];
										if (!string.IsNullOrEmpty(text8))
										{
											dataView2.RowFilter = string.Format("{0}>=0 AND f_ReaderID ={1} ", text8, dataRow2["f_ReaderID"].ToString());
											if (dataView2.Count > 0)
											{
												dataRow2["f_Cost"] = decimal.Parse(wgTools.SetObjToStr(dataView2[0][text8]), CultureInfo.InvariantCulture);
											}
										}
										dataView.RowFilter = string.Format("f_GroupID = {0} ", (int)this.dvConsumerStatistics[n]["f_GroupID"]);
										if (flag && (int)this.dvConsumerStatistics[n]["f_GroupID"] > 0 && dataView.Count > 0 && (int)dataView[0]["f_Enabled"] == 1)
										{
											switch (l)
											{
											case 0:
												dataRow2["f_Cost"] = decimal.Parse(wgTools.SetObjToStr(dataView[0]["f_Morning"]), CultureInfo.InvariantCulture);
												break;
											case 1:
												dataRow2["f_Cost"] = decimal.Parse(wgTools.SetObjToStr(dataView[0]["f_Lunch"]), CultureInfo.InvariantCulture);
												break;
											case 2:
												dataRow2["f_Cost"] = decimal.Parse(wgTools.SetObjToStr(dataView[0]["f_Evening"]), CultureInfo.InvariantCulture);
												break;
											case 3:
												dataRow2["f_Cost"] = decimal.Parse(wgTools.SetObjToStr(dataView[0]["f_Other"]), CultureInfo.InvariantCulture);
												break;
											}
										}
										if (dataView.Count > 0)
										{
											if (text6 == array2[0])
											{
												dataView[0]["f_MorningCount"] = (int)dataView[0]["f_MorningCount"] + 1;
												num9++;
											}
											else if (text6 == array2[1])
											{
												dataView[0]["f_LunchCount"] = (int)dataView[0]["f_LunchCount"] + 1;
												num10++;
											}
											else if (text6 == array2[2])
											{
												dataView[0]["f_EveningCount"] = (int)dataView[0]["f_EveningCount"] + 1;
												num11++;
											}
											else if (text6 == array2[3])
											{
												dataView[0]["f_OtherCount"] = (int)dataView[0]["f_OtherCount"] + 1;
												num12++;
											}
											dataView[0]["f_groupTotalTimes"] = (int)dataView[0]["f_groupTotalTimes"] + 1;
											dataView[0]["f_groupTotal"] = (decimal)dataView[0]["f_groupTotal"] + (decimal)dataRow2["f_Cost"];
										}
										dataTable3.Rows.Add(dataRow2);
									}
								}
							}
							dataView.Table.AcceptChanges();
							dataTable3.AcceptChanges();
						}
					}
					this.dv = new DataView(this.ds.Tables["MealReport"]);
					int num15 = 0;
					decimal num16 = 0m;
					this.dv.RowFilter = "";
					if (this.dv.Count > 0)
					{
						this.ProgressBar1.Value = 0;
						this.ProgressBar1.Maximum = Math.Max(0, dataTable.Rows.Count);
						for (int num17 = 0; num17 <= dataTable.Rows.Count - 1; num17++)
						{
							this.ProgressBar1.Value = num17;
							string text9 = "f_ReaderID = " + (int)dataTable.Rows[num17]["f_ReaderID"];
							this.dv.RowFilter = text9;
							if (this.dv.Count > 0)
							{
								if (this.dv.Count > 0)
								{
									dataTable.Rows[num17]["f_CostCount"] = (int)dataTable.Rows[num17]["f_CostCount"] + this.dv.Count;
									num15 += this.dv.Count;
								}
								for (int num18 = 0; num18 <= this.dv.Count - 1; num18++)
								{
									dataTable.Rows[num17]["f_CostTotal4Reader"] = (decimal)dataTable.Rows[num17]["f_CostTotal4Reader"] + (decimal)this.dv[num18]["f_Cost"];
									num16 += (decimal)this.dv[num18]["f_Cost"];
								}
							}
						}
					}
					DataRow dataRow3 = dataTable.NewRow();
					dataRow3["f_ReaderName"] = CommonStr.strMealTotal;
					dataRow3["f_CostCount"] = num15;
					dataRow3["f_CostTotal4Reader"] = num16;
					dataTable.Rows.Add(dataRow3);
					dataTable.AcceptChanges();
					dataRow3 = this.ds.Tables["t_b_group4Meal"].NewRow();
					dataRow3["f_GroupID"] = -1;
					dataRow3["f_GroupName"] = CommonStr.strMealTotal;
					dataRow3["f_Morning"] = 0.00m;
					dataRow3["f_Lunch"] = 0.00m;
					dataRow3["f_Evening"] = 0.00m;
					dataRow3["f_Other"] = 0.00m;
					dataRow3["f_MorningCount"] = num9;
					dataRow3["f_LunchCount"] = num10;
					dataRow3["f_EveningCount"] = num11;
					dataRow3["f_OtherCount"] = num12;
					dataRow3["f_groupTotalTimes"] = num15;
					dataRow3["f_groupTotal"] = num16;
					this.ds.Tables["t_b_group4Meal"].Rows.Add(dataRow3);
					this.ds.Tables["t_b_group4Meal"].AcceptChanges();
					this.dv.RowFilter = "";
					this.dv.RowFilter = "";
					if (this.dv.Count > 0)
					{
						this.ProgressBar1.Value = 0;
						this.ProgressBar1.Maximum = Math.Max(0, dataTable2.Rows.Count);
						for (int n = 0; n <= dataTable2.Rows.Count - 1; n++)
						{
							this.ProgressBar1.Value = n;
							string text9 = "f_ConsumerID = " + dataTable2.Rows[n]["f_ConsumerID"];
							this.dv.RowFilter = text9;
							if (this.dv.Count > 0)
							{
								for (int l = 0; l <= array.Length - 1; l++)
								{
									if (array[l] > 0)
									{
										this.dv.RowFilter = text9 + " AND f_MealName= " + wgTools.PrepareStr(array2[l]);
										dataTable2.Rows[n][array3[l] + "Count"] = (int)dataTable2.Rows[n][array3[l] + "Count"] + this.dv.Count;
										dataTable2.Rows[n]["f_CostTotalCount"] = (int)dataTable2.Rows[n]["f_CostTotalCount"] + this.dv.Count;
										for (int num19 = 0; num19 <= this.dv.Count - 1; num19++)
										{
											dataTable2.Rows[n][array3[l]] = (decimal)dataTable2.Rows[n][array3[l]] + (decimal)this.dv[num19]["f_Cost"];
											dataTable2.Rows[n]["f_CostTotal"] = (decimal)dataTable2.Rows[n]["f_CostTotal"] + (decimal)this.dv[num19]["f_Cost"];
										}
									}
								}
							}
						}
					}
					dataTable2.AcceptChanges();
					DataRow dataRow4 = dataTable2.NewRow();
					dataRow4["f_GroupName"] = "==========";
					dataRow4["f_ConsumerNO"] = "==========";
					dataRow4["f_ConsumerName"] = CommonStr.strMealTotal;
					for (int l = 4; l <= 13; l++)
					{
						dataRow4[l] = 0;
					}
					for (int n = 0; n <= dataTable2.Rows.Count - 1; n++)
					{
						this.ProgressBar1.Value = n;
						for (int l = 4; l <= 13; l++)
						{
							if (dataRow4[l].GetType().Name.ToString() == "Decimal")
							{
								dataRow4[l] = (decimal)dataRow4[l] + (decimal)dataTable2.Rows[n][l];
							}
							else
							{
								dataRow4[l] = (int)dataRow4[l] + (int)dataTable2.Rows[n][l];
							}
						}
					}
					dataTable2.Rows.Add(dataRow4);
					this.ProgressBar1.Value = 0;
					this.dgvSwipeRecords.AutoGenerateColumns = false;
					this.dgvSubtotal.AutoGenerateColumns = false;
					this.dgvStatistics.AutoGenerateColumns = false;
					this.dgvSwipeRecords.DataSource = this.ds.Tables["MealReport"];
					DataTable dataTable5 = this.ds.Tables["MealReport"];
					j = 0;
					while (j < this.dgvSwipeRecords.ColumnCount && j < dataTable5.Columns.Count)
					{
						this.dgvSwipeRecords.Columns[j].DataPropertyName = dataTable5.Columns[j].ColumnName;
						j++;
					}
					wgAppConfig.setDisplayFormatDate(this.dgvSwipeRecords, "f_ReadDate", wgTools.DisplayFormat_DateYMDHMSWeek);
					this.dgvSubtotal.DataSource = dataTable;
					dataTable5 = dataTable;
					j = 0;
					while (j < this.dgvSubtotal.ColumnCount && j < dataTable5.Columns.Count)
					{
						this.dgvSubtotal.Columns[j].DataPropertyName = dataTable5.Columns[j].ColumnName;
						j++;
					}
					this.dgvStatistics.DataSource = dataTable2;
					dataTable5 = dataTable2;
					j = 0;
					while (j < this.dgvStatistics.ColumnCount && j < dataTable5.Columns.Count)
					{
						this.dgvStatistics.Columns[j].DataPropertyName = dataTable5.Columns[j].ColumnName;
						j++;
					}
					dataView.RowFilter = "";
					this.dgvGroupSubTotal.AutoGenerateColumns = false;
					this.dgvGroupSubTotal.DataSource = dataView;
					j = 0;
					while (j < this.dgvGroupSubTotal.ColumnCount && j < dataView.Table.Columns.Count)
					{
						this.dgvGroupSubTotal.Columns[j].DataPropertyName = dataView.Table.Columns[j].ColumnName;
						j++;
					}
					this.dgvSwipeRecords.DefaultCellStyle.ForeColor = Color.Black;
					this.dgvSubtotal.DefaultCellStyle.ForeColor = Color.Black;
					this.dgvStatistics.DefaultCellStyle.ForeColor = Color.Black;
					if (this.dgvSwipeRecords.Rows.Count <= 0)
					{
						this.btnPrint.Enabled = false;
						this.btnExportToExcel.Enabled = false;
						XMessageBox.Show(CommonStr.strMealNoRecords);
					}
					else
					{
						this.btnPrint.Enabled = true;
						this.btnExportToExcel.Enabled = true;
					}
				}
				catch (Exception ex2)
				{
					wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
				}
				finally
				{
					this.btnCreateReport.Enabled = true;
					Cursor.Current = cursor;
				}
			}
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x001C9750 File Offset: 0x001C8750
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.dgvSwipeRecords.DataSource == null)
			{
				Cursor.Current = Cursors.WaitCursor;
				return;
			}
			Cursor.Current = Cursors.Default;
			this.timer1.Enabled = false;
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x001C9780 File Offset: 0x001C8780
		private void updateNewTitle()
		{
			this.tabPage1.Text = this.oldStatTitle1 + this.strDateTitle;
			this.tabPage2.Text = this.oldStatTitle2 + this.strDateTitle;
			this.tabPage4.Text = this.oldStatTitle4 + this.strDateTitle;
		}

		// Token: 0x04002E3D RID: 11837
		private bool bLoadedFinished;

		// Token: 0x04002E3E RID: 11838
		private DataSet ds;

		// Token: 0x04002E3F RID: 11839
		private DataView dv;

		// Token: 0x04002E40 RID: 11840
		private DataView dvReaderStatistics;

		// Token: 0x04002E41 RID: 11841
		private int recIdMin;

		// Token: 0x04002E42 RID: 11842
		private int startRecordIndex;

		// Token: 0x04002E43 RID: 11843
		private string dgvSql = "";

		// Token: 0x04002E44 RID: 11844
		private DataSet dsDefaultStyle = new DataSet("DGV_STILE");

		// Token: 0x04002E45 RID: 11845
		private DataView dvConsumerStatistics = new DataView();

		// Token: 0x04002E46 RID: 11846
		private int MaxRecord = 1000;

		// Token: 0x04002E47 RID: 11847
		private string oldStatTitle1 = "";

		// Token: 0x04002E48 RID: 11848
		private string oldStatTitle2 = "";

		// Token: 0x04002E49 RID: 11849
		private string oldStatTitle3 = "";

		// Token: 0x04002E4A RID: 11850
		private string oldStatTitle4 = "";

		// Token: 0x04002E4B RID: 11851
		private string strDateTitle = "";

		// Token: 0x04002E69 RID: 11881
		private frmMeal.ToolStripDateTime dtpDateFrom;

		// Token: 0x04002E6A RID: 11882
		private frmMeal.ToolStripDateTime dtpDateTo;

		// Token: 0x04002E81 RID: 11905
		private DataTable table;

		// Token: 0x020002FB RID: 763
		public class ToolStripDateTime : ToolStripControlHost
		{
			// Token: 0x06001669 RID: 5737 RVA: 0x001CB296 File Offset: 0x001CA296
			public ToolStripDateTime()
				: base(frmMeal.ToolStripDateTime.dtp = new DateTimePicker())
			{
			}

			// Token: 0x0600166A RID: 5738 RVA: 0x001CB2A9 File Offset: 0x001CA2A9
			protected override void Dispose(bool disposing)
			{
				if (disposing && frmMeal.ToolStripDateTime.dtp != null)
				{
					frmMeal.ToolStripDateTime.dtp.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x0600166B RID: 5739 RVA: 0x001CB2C8 File Offset: 0x001CA2C8
			public void SetTimeFormat()
			{
				DateTimePicker dateTimePicker = base.Control as DateTimePicker;
				dateTimePicker.CustomFormat = "HH;mm";
				dateTimePicker.Format = DateTimePickerFormat.Custom;
				dateTimePicker.ShowUpDown = true;
			}

			// Token: 0x170001C5 RID: 453
			// (get) Token: 0x0600166C RID: 5740 RVA: 0x001CB2FC File Offset: 0x001CA2FC
			// (set) Token: 0x0600166D RID: 5741 RVA: 0x001CB324 File Offset: 0x001CA324
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

			// Token: 0x170001C6 RID: 454
			// (get) Token: 0x0600166E RID: 5742 RVA: 0x001CB388 File Offset: 0x001CA388
			public DateTimePicker DateTimeControl
			{
				get
				{
					return base.Control as DateTimePicker;
				}
			}

			// Token: 0x170001C7 RID: 455
			// (get) Token: 0x0600166F RID: 5743 RVA: 0x001CB395 File Offset: 0x001CA395
			// (set) Token: 0x06001670 RID: 5744 RVA: 0x001CB3A7 File Offset: 0x001CA3A7
			public DateTime Value
			{
				get
				{
					return (base.Control as DateTimePicker).Value;
				}
				set
				{
					(base.Control as DateTimePicker).Value = value;
				}
			}

			// Token: 0x04002E8C RID: 11916
			private static DateTimePicker dtp;
		}
	}
}
