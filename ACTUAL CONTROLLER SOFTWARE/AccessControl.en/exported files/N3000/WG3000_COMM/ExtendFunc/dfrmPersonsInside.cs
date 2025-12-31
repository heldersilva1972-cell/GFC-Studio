using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000247 RID: 583
	public partial class dfrmPersonsInside : frmN3000
	{
		// Token: 0x0600121F RID: 4639 RVA: 0x00159528 File Offset: 0x00158528
		public dfrmPersonsInside()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvEnterIn);
			wgAppConfig.custDataGridview(ref this.dgvOutSide);
			wgAppConfig.custDataGridview(ref this.dgvGroupSubTotal);
			wgAppConfig.custDataGridview(ref this.dgvSwipe);
			wgAppConfig.custDataGridview(ref this.dgvNotSwipe);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0015967F File Offset: 0x0015867F
		private void activeExportToXMLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.bAllowExport = 1;
			wgAppConfig.UpdateKeyVal("KEY_PersonsInsideAllowExport", this.bAllowExport.ToString());
			this.activeExportToXMLToolStripMenuItem.Checked = true;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x001596AC File Offset: 0x001586AC
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			e.Result = this.dealPersonInside();
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x001596F8 File Offset: 0x001586F8
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				wgTools.WgDebugWrite("Users Operation was canceled", new object[0]);
			}
			else if (e.Error != null)
			{
				wgTools.WgDebugWrite(string.Format("An error occurred: {0}", e.Error.Message), new object[0]);
			}
			else
			{
				int num = (int)e.Result;
				if (num == 0)
				{
					this.dgvEnterIn.DataSource = null;
					this.dgvOutSide.DataSource = null;
					this.dgvGroupSubTotal.DataSource = null;
					this.dgvSwipe.DataSource = null;
					this.dgvNotSwipe.DataSource = null;
					this.txtPersons.Text = "0";
					this.txtPersonsOutSide.Text = "0";
					if (!this.chkAutoRefresh.Checked)
					{
						XMessageBox.Show(CommonStr.strNoAccessPrivilege4SelectedDoors);
					}
				}
				else if (num > 0)
				{
					this.dvIn = new DataView(this.dtUsers);
					this.dvOut = new DataView(this.dtUsers);
					this.dvGroup = new DataView(this.dtGroups);
					this.dvIn.RowFilter = "f_bHave =2";
					this.dvOut.RowFilter = "f_bHave < 2";
					this.dvSwipe = new DataView(this.dtUsers);
					this.dvSwipe.RowFilter = "f_bHave = 1 OR f_bHave = 2 ";
					this.dvNotSwipe = new DataView(this.dtUsers);
					this.dvNotSwipe.RowFilter = "f_bHave = 0";
					int num2 = 0;
					for (int i = 0; i < this.dtGroups.Rows.Count; i++)
					{
						this.dvIn.RowFilter = "f_bHave = 2 And f_GroupID = " + this.dvGroup.Table.Rows[i]["f_GroupID"].ToString();
						this.dvOut.RowFilter = "f_bHave < 2 And f_GroupID = " + this.dvGroup.Table.Rows[i]["f_GroupID"].ToString();
						this.dvGroup.RowFilter = "f_GroupID = " + this.dvGroup.Table.Rows[i]["f_GroupID"].ToString();
						if (this.dvGroup.Count > 0)
						{
							this.dvGroup[0]["f_Inside"] = this.dvIn.Count;
							this.dvGroup[0]["f_Outside"] = this.dvOut.Count;
							if (this.dvOut.Count > 0)
							{
								this.dvOut.RowFilter = "f_bHave < 2 And f_GroupID = " + this.dvGroup.Table.Rows[i]["f_GroupID"].ToString() + " AND f_bHave = 1 ";
								this.dvGroup[0]["f_SwipeOut"] = this.dvOut.Count;
								num2 += this.dvOut.Count;
							}
						}
					}
					this.dtGroups.AcceptChanges();
					this.dvIn.RowFilter = "f_bHave =2";
					this.dvOut.RowFilter = "f_bHave < 2";
					this.dvGroup.RowFilter = "";
					DataRow dataRow = this.dtGroups.NewRow();
					dataRow["f_GroupID"] = -1;
					dataRow["f_GroupName"] = CommonStr.strPersonTotal;
					dataRow["f_Inside"] = this.dvIn.Count;
					dataRow["f_Outside"] = this.dvOut.Count;
					dataRow["f_SwipeOut"] = num2;
					this.dtGroups.Rows.Add(dataRow);
					this.dtGroups.AcceptChanges();
					this.dgvEnterIn.DataSource = this.dvIn;
					this.dgvOutSide.DataSource = this.dvOut;
					int num3 = 0;
					while (num3 < this.dvIn.Table.Columns.Count && num3 < this.dgvEnterIn.ColumnCount)
					{
						this.dgvEnterIn.Columns[num3].DataPropertyName = this.dvIn.Table.Columns[num3].ColumnName;
						this.dgvEnterIn.Columns[num3].Name = this.dvIn.Table.Columns[num3].ColumnName;
						num3++;
					}
					int num4 = 0;
					while (num4 < this.dvOut.Table.Columns.Count && num4 < this.dgvOutSide.ColumnCount)
					{
						this.dgvOutSide.Columns[num4].DataPropertyName = this.dvOut.Table.Columns[num4].ColumnName;
						this.dgvOutSide.Columns[num4].Name = this.dvOut.Table.Columns[num4].ColumnName;
						num4++;
					}
					this.dgvGroupSubTotal.DataSource = this.dvGroup;
					int num5 = 0;
					while (num5 < this.dgvGroupSubTotal.ColumnCount && num5 < this.dvGroup.Table.Columns.Count)
					{
						this.dgvGroupSubTotal.Columns[num5].DataPropertyName = this.dvGroup.Table.Columns[num5].ColumnName;
						this.dgvGroupSubTotal.Columns[num5].Name = this.dvGroup.Table.Columns[num5].ColumnName;
						num5++;
					}
					this.dgvSwipe.DataSource = this.dvSwipe;
					this.dgvNotSwipe.DataSource = this.dvNotSwipe;
					int num6 = 0;
					while (num6 < this.dvSwipe.Table.Columns.Count && num6 < this.dgvSwipe.ColumnCount)
					{
						this.dgvSwipe.Columns[num6].DataPropertyName = this.dvSwipe.Table.Columns[num6].ColumnName;
						this.dgvSwipe.Columns[num6].Name = this.dvSwipe.Table.Columns[num6].ColumnName;
						num6++;
					}
					int num7 = 0;
					while (num7 < this.dvNotSwipe.Table.Columns.Count && num7 < this.dgvNotSwipe.ColumnCount)
					{
						this.dgvNotSwipe.Columns[num7].DataPropertyName = this.dvNotSwipe.Table.Columns[num7].ColumnName;
						this.dgvNotSwipe.Columns[num7].Name = this.dvNotSwipe.Table.Columns[num7].ColumnName;
						num7++;
					}
					wgAppConfig.setDisplayFormatDate(this.dgvEnterIn, "f_EndYMD", wgTools.DisplayFormat_DateYMDHMS.Replace(":ss", ""));
					wgAppConfig.setDisplayFormatDate(this.dgvOutSide, "f_EndYMD", wgTools.DisplayFormat_DateYMDHMS.Replace(":ss", ""));
					wgAppConfig.setDisplayFormatDate(this.dgvSwipe, "f_EndYMD", wgTools.DisplayFormat_DateYMDHMS.Replace(":ss", ""));
					wgAppConfig.setDisplayFormatDate(this.dgvNotSwipe, "f_EndYMD", wgTools.DisplayFormat_DateYMDHMS.Replace(":ss", ""));
					this.txtPersons.Text = this.dvIn.Count.ToString();
					this.txtPersonsOutSide.Text = this.dvOut.Count.ToString();
					this.outputFile4InsideUsers(this.dvIn.Count);
					lock (this.dtGroupsShare)
					{
						this.dtGroupsShare = this.dtGroups.Copy();
						this.dataNeedRefresh4Lcd = true;
					}
					frmWatchingLED.sendLEDNumber(this.dtGroupsShare);
					if (this.bActiveLEDSwitch)
					{
						frmWatchingLED.sendPersonsIndoor(this.dtUsers);
					}
				}
				this.btnQuery2.Enabled = true;
				this.timer1.Enabled = true;
				this.NextRefreshTime = DateTime.Now.AddSeconds((double)this.nudCycleSecs.Value);
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00159FD8 File Offset: 0x00158FD8
		private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			e.Result = this.loadUserData4BackWork();
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0015A01C File Offset: 0x0015901C
		private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				this.frmCall.dvConsumer4Access = new DataView(e.Result as DataTable);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0015A064 File Offset: 0x00159064
		private void BatchUpdateSelectedtoolStripMenuItem4_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = null;
				int num = 0;
				string text = "";
				string text2 = "";
				int num2 = 0;
				if (this.warnInfoShow(sender, ref dataGridView, ref num, ref text, ref text2, ref num2) != 0)
				{
					using (dfrmUserBatchUpdate dfrmUserBatchUpdate = new dfrmUserBatchUpdate())
					{
						string text3 = "";
						string text4 = "";
						for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
						{
							num = dataGridView.SelectedRows[i].Index;
							text4 = text4 + dataGridView.Rows[num].Cells[1].Value.ToString() + ",";
							int num3 = int.Parse(dataGridView.Rows[num].Cells[0].Value.ToString());
							if (!string.IsNullOrEmpty(text3))
							{
								text3 += ",";
							}
							text3 += num3.ToString();
						}
						dfrmUserBatchUpdate.strSqlSelected = text3;
						dfrmUserBatchUpdate.Text = string.Format("{0}: [{1}]", (sender as ToolStripMenuItem).Text, dataGridView.SelectedRows.Count.ToString());
						if (dfrmUserBatchUpdate.ShowDialog(this) == DialogResult.OK)
						{
							if (dfrmUserBatchUpdate.groupIdUpdated >= 0)
							{
								icConsumerShare.setUpdateLog();
								if (!this.backgroundWorker2.IsBusy)
								{
									this.backgroundWorker2.RunWorkerAsync();
								}
								string text5 = "";
								if (dfrmUserBatchUpdate.groupIdUpdated > 0 && dfrmUserBatchUpdate.groupIdUpdated < this.arrGroupName.Count)
								{
									text5 = this.arrGroupName[dfrmUserBatchUpdate.groupIdUpdated].ToString();
								}
								foreach (object obj in dataGridView.SelectedRows)
								{
									DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
									dataGridViewRow.Cells["f_GroupName"].Value = text5;
								}
								dataGridView.Refresh();
							}
							if (dfrmUserBatchUpdate.bNewDate)
							{
								foreach (object obj2 in dataGridView.SelectedRows)
								{
									DataGridViewRow dataGridViewRow2 = (DataGridViewRow)obj2;
									dataGridViewRow2.Cells["f_EndYMD"].Value = dfrmUserBatchUpdate.deactivateDateNew;
								}
							}
							wgAppConfig.wgLog(text2);
							if (!this.bShowNeedUploadFloor)
							{
								this.bShowNeedUploadFloor = true;
								XMessageBox.Show(CommonStr.strNeedUploadFloor);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0015A378 File Offset: 0x00159378
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			if (this.tabControl1.SelectedTab == this.tabPage1)
			{
				wgAppConfig.exportToExcel(this.dgvEnterIn, this.tabPage1.Text);
			}
			if (this.tabControl1.SelectedTab == this.tabPage2)
			{
				wgAppConfig.exportToExcel(this.dgvOutSide, this.tabPage2.Text);
			}
			if (this.tabControl1.SelectedTab == this.tabPage3)
			{
				wgAppConfig.exportToExcel(this.dgvGroupSubTotal, this.tabPage3.Text);
			}
			if (this.tabControl1.SelectedTab == this.tabPage4)
			{
				wgAppConfig.exportToExcel(this.dgvSwipe, this.tabPage4.Text);
			}
			if (this.tabControl1.SelectedTab == this.tabPage5)
			{
				wgAppConfig.exportToExcel(this.dgvNotSwipe, this.tabPage5.Text);
			}
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0015A458 File Offset: 0x00159458
		private void btnPrint_Click(object sender, EventArgs e)
		{
			if (this.tabControl1.SelectedTab == this.tabPage1)
			{
				wgAppConfig.printdgv(this.dgvEnterIn, this.tabPage1.Text);
			}
			if (this.tabControl1.SelectedTab == this.tabPage2)
			{
				wgAppConfig.printdgv(this.dgvOutSide, this.tabPage2.Text);
			}
			if (this.tabControl1.SelectedTab == this.tabPage3)
			{
				wgAppConfig.printdgv(this.dgvGroupSubTotal, this.tabPage3.Text);
			}
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0015A4E0 File Offset: 0x001594E0
		private void btnQuery_Click(object sender, EventArgs e)
		{
			this.strSqlReaders = this.getStrSql();
			if (string.IsNullOrEmpty(this.strSqlReaders))
			{
				if (!this.chkAutoRefresh.Checked)
				{
					XMessageBox.Show(CommonStr.strSelectDoor4Query);
					return;
				}
			}
			else
			{
				this.tmStop = DateTime.Now.Date.AddDays(-(double)this.nudDays.Value).Date;
				wgAppConfig.UpdateKeyVal("KEY_PersonsInsideDayQuery", this.nudDays.Value.ToString());
				wgAppConfig.UpdateKeyVal("KEY_PersonsInsideCycleSecs", this.nudCycleSecs.Value.ToString());
				wgAppConfig.UpdateKeyVal("KEY_PersonsInsideAutoRefresh", this.chkAutoRefresh.Checked ? "1" : "0");
				if (!this.backgroundWorker1.IsBusy)
				{
					this.btnQuery2.Enabled = false;
					this.timer1.Enabled = false;
					this.bRecordIn = this.chkReaderIn.Checked;
					this.bRecordOut = this.chkReaderOut.Checked;
					wgAppConfig.UpdateKeyVal("KEY_PersonsInsideReaderIn", this.bRecordIn ? "0" : "1");
					wgAppConfig.UpdateKeyVal("KEY_PersonsInsideReaderOut", this.bRecordOut ? "0" : "1");
					wgAppConfig.UpdateKeyVal("KEY_PersonsInsideZone", this.cboZone.SelectedIndex.ToString());
					wgAppConfig.UpdateKeyVal("KEY_PersonsInsideGroup", this.cbof_GroupID.SelectedIndex.ToString());
					if (this.chkListDoors.SelectedItems.Count == this.chkListDoors.Items.Count)
					{
						wgAppConfig.UpdateKeyVal("KEY_PersonsInsideSqlReaders", "");
					}
					else
					{
						wgAppConfig.UpdateKeyVal("KEY_PersonsInsideSqlReaders", this.strSqlReaders);
					}
					if (this.nudCycleSecs.Value >= 3m)
					{
						Cursor.Current = Cursors.WaitCursor;
					}
					int swipeRecordMaxRecIdOfDB = wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
					if (this.laststrSqlReaders.Equals(this.strSqlReaders) && this.laststrKEY_PersonsInsideDayQuery.Equals(wgAppConfig.GetKeyVal("KEY_PersonsInsideDayQuery")) && this.laststrKEY_PersonsInsideCycleSecs.Equals(wgAppConfig.GetKeyVal("KEY_PersonsInsideCycleSecs")) && this.laststrKEY_PersonsInsideAutoRefresh.Equals(wgAppConfig.GetKeyVal("KEY_PersonsInsideCycleSecs")) && this.laststrKEY_PersonsInsideReaderIn.Equals(wgAppConfig.GetKeyVal("KEY_PersonsInsideReaderIn")) && this.laststrKEY_PersonsInsideReaderOut.Equals(wgAppConfig.GetKeyVal("KEY_PersonsInsideReaderOut")) && this.laststrKEY_PersonsInsideZone.Equals(wgAppConfig.GetKeyVal("KEY_PersonsInsideZone")) && this.laststrKEY_PersonsInsideGroup.Equals(wgAppConfig.GetKeyVal("KEY_PersonsInsideGroup")) && this.laststrKEY_PersonsInsideSqlReaders.Equals(wgAppConfig.GetKeyVal("KEY_PersonsInsideSqlReaders")))
					{
						if (swipeRecordMaxRecIdOfDB == this.lastRecID)
						{
							this.btnQuery2.Enabled = true;
							this.timer1.Enabled = true;
							return;
						}
						this.lastRecID = swipeRecordMaxRecIdOfDB;
						this.bOnlyCheckNewRecord = true;
					}
					else
					{
						this.bOnlyCheckNewRecord = false;
					}
					this.lastRecID = swipeRecordMaxRecIdOfDB;
					this.laststrSqlReaders = this.strSqlReaders;
					this.laststrKEY_PersonsInsideDayQuery = wgAppConfig.GetKeyVal("KEY_PersonsInsideDayQuery");
					this.laststrKEY_PersonsInsideCycleSecs = wgAppConfig.GetKeyVal("KEY_PersonsInsideCycleSecs");
					this.laststrKEY_PersonsInsideAutoRefresh = wgAppConfig.GetKeyVal("KEY_PersonsInsideCycleSecs");
					this.laststrKEY_PersonsInsideReaderIn = wgAppConfig.GetKeyVal("KEY_PersonsInsideReaderIn");
					this.laststrKEY_PersonsInsideReaderOut = wgAppConfig.GetKeyVal("KEY_PersonsInsideReaderOut");
					this.laststrKEY_PersonsInsideZone = wgAppConfig.GetKeyVal("KEY_PersonsInsideZone");
					this.laststrKEY_PersonsInsideGroup = wgAppConfig.GetKeyVal("KEY_PersonsInsideGroup");
					this.laststrKEY_PersonsInsideSqlReaders = wgAppConfig.GetKeyVal("KEY_PersonsInsideSqlReaders");
					this.backgroundWorker1.RunWorkerAsync();
				}
			}
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0015A894 File Offset: 0x00159894
		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			if (this.chkListDoors.Items.Count > 0)
			{
				for (int i = 0; i < this.chkListDoors.Items.Count; i++)
				{
					this.chkListDoors.SetItemChecked(i, true);
				}
			}
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0015A8DC File Offset: 0x001598DC
		private void btnSelectNone_Click(object sender, EventArgs e)
		{
			if (this.chkListDoors.Items.Count > 0)
			{
				for (int i = 0; i < this.chkListDoors.Items.Count; i++)
				{
					this.chkListDoors.SetItemChecked(i, false);
				}
			}
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0015A924 File Offset: 0x00159924
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
			{
				this.strGroupFilter = "";
				return;
			}
			this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
			int num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex];
			int num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
			int groupChildMaxNo = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
			if (num2 > 0)
			{
				if (num2 >= groupChildMaxNo)
				{
					this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num);
					return;
				}
				string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
				this.strGroupFilter = string.Format("  {0} ", groupQuery);
			}
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0015AA30 File Offset: 0x00159A30
		private void cbof_Zone_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.dvDoors != null)
			{
				this.chkListDoors.Items.Clear();
				this.dv = this.dvDoors;
				if (this.cboZone.SelectedIndex < 0 || (this.cboZone.SelectedIndex == 0 && (int)this.arrZoneID[0] == 0))
				{
					this.dv.RowFilter = "";
				}
				else
				{
					this.dv.RowFilter = "f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
					string text = " f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
					int num = (int)this.arrZoneID[this.cboZone.SelectedIndex];
					int num2 = (int)this.arrZoneNO[this.cboZone.SelectedIndex];
					int zoneChildMaxNo = icControllerZone.getZoneChildMaxNo(this.cboZone.Text, this.arrZoneName, this.arrZoneNO);
					if (num2 > 0)
					{
						if (num2 >= zoneChildMaxNo)
						{
							this.dv.RowFilter = string.Format(" f_ZoneID ={0:d} ", num);
							text = string.Format(" f_ZoneID ={0:d} ", num);
						}
						else
						{
							this.dv.RowFilter = "";
							string zoneQuery = icGroup.getZoneQuery(num2, zoneChildMaxNo, this.arrZoneNO, this.arrZoneID);
							this.dv.RowFilter = string.Format("  {0} ", zoneQuery);
							text = string.Format("  {0} ", zoneQuery);
						}
					}
					this.dv.RowFilter = string.Format(" {0} ", text);
				}
				this.chkListDoors.Items.Clear();
				if (this.dvDoors.Count > 0)
				{
					for (int i = 0; i < this.dvDoors.Count; i++)
					{
						this.chkListDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
						this.arrAddrDoorID[i] = (int)this.dvDoors[i]["f_DoorID"];
						this.arrAddrDoorName[i] = (string)this.dvDoors[i]["f_DoorName"];
						this.arrAddr[i] = 0;
						this.arrAddrOut[i] = 0;
						if (wgMjController.GetControllerType((int)this.dvDoors[i]["f_ControllerSN"]) == 4)
						{
							this.dvReader.RowFilter = "f_ControllerSN = " + this.dvDoors[i]["f_ControllerSN"].ToString() + " AND f_ReaderNO=" + this.dvDoors[i]["f_DoorNO"].ToString();
							if (this.dvReader.Count > 0)
							{
								this.arrAddr[i] = (int)this.dvReader[0]["f_ReaderID"];
							}
						}
						else
						{
							string[] array = new string[]
							{
								"f_ControllerSN = ",
								this.dvDoors[i]["f_ControllerSN"].ToString(),
								" AND (f_ReaderNO=",
								((int)(((byte)this.dvDoors[i]["f_DoorNO"] - 1) * 2 + 1)).ToString(),
								" OR f_ReaderNO=",
								((int)(((byte)this.dvDoors[i]["f_DoorNO"] - 1) * 2 + 2)).ToString(),
								" )"
							};
							this.dvReader.RowFilter = string.Concat(array);
							if (this.dvReader.Count > 0)
							{
								this.arrAddr[i] = (int)this.dvReader[0]["f_ReaderID"];
								if (this.dvReader.Count > 1)
								{
									this.arrAddrOut[i] = (int)this.dvReader[1]["f_ReaderID"];
								}
							}
						}
					}
					return;
				}
			}
			else
			{
				this.chkListDoors.Items.Clear();
			}
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0015AEA3 File Offset: 0x00159EA3
		private void chkListDoors_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrmPersonsInside_KeyDown(this.chkListDoors, e);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0015AEB4 File Offset: 0x00159EB4
		private int dealPersonInside()
		{
			if (this.bOnlyCheckNewRecord)
			{
				this.bOnlyCheckNewRecord = false;
				return this.dealPersonInsideOnlyNewRecords();
			}
			int num = -1;
			try
			{
				string text;
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					text = " SELECT t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_GroupName,  '' as  f_ReadDate, '' as f_DoorName, 0 as f_bHave ";
					text = text + ", t_b_Consumer.f_EndYMD  , t_b_Consumer.f_GroupID  FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON ( t_b_Consumer.f_GroupID = t_b_Group.f_GroupID )  WHERE f_ConsumerID IN (SELECT DISTINCT f_ConsumerID FROM t_d_Privilege WHERE f_DoorID IN (" + this.strSqlDoorID + "))";
				}
				else
				{
					text = " SELECT  t_b_Consumer.f_ConsumerID,f_ConsumerNO, f_ConsumerName, f_GroupName,  '' as  f_ReadDate, '' as f_DoorName, 0 as f_bHave ";
					text = string.Concat(new string[]
					{
						text,
						" , t_b_Consumer.f_EndYMD, t_b_Consumer.f_GroupID   FROM t_b_Consumer , t_b_Group WHERE ( t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) AND  ",
						this.strGroupFilter.Replace("f_GroupID", "t_b_Group.f_GroupID"),
						" AND f_ConsumerID IN (SELECT DISTINCT f_ConsumerID FROM t_d_Privilege WHERE f_DoorID IN (",
						this.strSqlDoorID,
						"))"
					});
				}
				this.dtUsers = new DataTable("PersonsInside");
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
							{
								oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
								oleDbDataAdapter.Fill(this.dtUsers);
							}
						}
						goto IL_0171;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							sqlDataAdapter.Fill(this.dtUsers);
						}
					}
				}
				IL_0171:
				if (this.dtUsers.Rows.Count <= 0)
				{
					return 0;
				}
				text = " SELECT f_GroupID,f_GroupName, 0 as f_Inside, 0 as f_Outside, 0 as f_SwipeOut from t_b_Group";
				if (!string.IsNullOrEmpty(this.strGroupFilter))
				{
					text = text + " WHERE " + this.strGroupFilter;
				}
				text += " order by f_GroupName  + '\\' ASC";
				this.dtGroups = new DataTable("GroupsInside");
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2))
						{
							using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(oleDbCommand2))
							{
								oleDbCommand2.CommandTimeout = wgAppConfig.dbCommandTimeout;
								oleDbDataAdapter2.Fill(this.dtGroups);
							}
						}
						goto IL_029A;
					}
				}
				using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
					{
						using (SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sqlCommand2))
						{
							sqlCommand2.CommandTimeout = wgAppConfig.dbCommandTimeout;
							sqlDataAdapter2.Fill(this.dtGroups);
						}
					}
				}
				IL_029A:
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					DataRow dataRow = this.dtGroups.NewRow();
					dataRow["f_GroupID"] = 0;
					dataRow["f_GroupName"] = CommonStr.strDepartmentIsEmpty;
					dataRow["f_Inside"] = 0;
					dataRow["f_Outside"] = 0;
					dataRow["f_SwipeOut"] = 0;
					this.dtGroups.Rows.Add(dataRow);
					this.dtGroups.AcceptChanges();
				}
				new DataView(this.dtGroups);
				text = " SELECT * FROM t_d_SwipeRecord WHERE 1>0 AND ";
				text = text + " f_ReaderID IN (" + this.strSqlReaders + ")  AND NOT ( f_ConsumerID IS NULL)  AND f_Character >0   ORDER BY f_ReadDate DESC ";
				using (DataView dataView = new DataView(this.dtUsers))
				{
					using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
					{
						using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
						{
							dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							dbConnection.Open();
							this.recIndexDealt = 0;
							DbDataReader dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								if ((int)dbDataReader["f_RecID"] > this.recIndexDealt)
								{
									this.recIndexDealt = (int)dbDataReader["f_RecID"];
								}
								if ((DateTime)dbDataReader["f_ReadDate"] <= DateTime.Now.AddDays(2.0))
								{
									if (((DateTime)dbDataReader["f_ReadDate"]).Date <= this.tmStop.Date)
									{
										break;
									}
									dataView.RowFilter = "f_ConsumerID = " + dbDataReader["f_ConsumerID"];
									if (dataView.Count > 0 && (int)dataView[0]["f_bHave"] == 0)
									{
										if ((byte)dbDataReader["f_InOut"] == 1 && this.bRecordIn)
										{
											dataView[0]["f_bHave"] = 2;
											dataView[0]["f_ReadDate"] = ((DateTime)dbDataReader["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
											for (int i = 0; i < this.arrAddr.Length; i++)
											{
												if (this.arrAddr[i] == (int)dbDataReader["f_ReaderID"])
												{
													dataView[0]["f_DoorName"] = this.arrAddrDoorName[i] + "[" + CommonStr.strInDoor + "]";
												}
											}
										}
										else if ((byte)dbDataReader["f_InOut"] == 0 && this.bRecordOut)
										{
											dataView[0]["f_bHave"] = 1;
											dataView[0]["f_ReadDate"] = ((DateTime)dbDataReader["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
											for (int j = 0; j < this.arrAddr.Length; j++)
											{
												if (this.arrAddrOut[j] == (int)dbDataReader["f_ReaderID"])
												{
													dataView[0]["f_DoorName"] = this.arrAddrDoorName[j] + "[" + CommonStr.strExitDoor + "]";
												}
											}
										}
									}
								}
							}
							dbDataReader.Close();
						}
					}
				}
				num = this.dtUsers.Rows.Count;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0015B6F4 File Offset: 0x0015A6F4
		private int dealPersonInsideOnlyNewRecords()
		{
			int num = -1;
			try
			{
				string text = " SELECT * FROM t_d_SwipeRecord WHERE 1>0 AND ";
				text = string.Concat(new string[]
				{
					text,
					" f_ReaderID IN (",
					this.strSqlReaders,
					")  AND NOT ( f_ConsumerID IS NULL)  AND f_Character >0   AND f_RecID > ",
					this.recIndexDealt.ToString(),
					" ORDER BY f_ReadDate ASC "
				});
				using (DataView dataView = new DataView(this.dtUsers))
				{
					using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
					{
						using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
						{
							dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							dbConnection.Open();
							DbDataReader dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								if ((int)dbDataReader["f_RecID"] > this.recIndexDealt)
								{
									this.recIndexDealt = (int)dbDataReader["f_RecID"];
								}
								if ((DateTime)dbDataReader["f_ReadDate"] <= DateTime.Now.AddDays(2.0))
								{
									if (((DateTime)dbDataReader["f_ReadDate"]).Date <= this.tmStop.Date)
									{
										break;
									}
									dataView.RowFilter = "f_ConsumerID = " + dbDataReader["f_ConsumerID"];
									if (dataView.Count > 0)
									{
										if ((byte)dbDataReader["f_InOut"] == 1 && this.bRecordIn)
										{
											dataView[0]["f_bHave"] = 2;
											dataView[0]["f_ReadDate"] = ((DateTime)dbDataReader["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
											for (int i = 0; i < this.arrAddr.Length; i++)
											{
												if (this.arrAddr[i] == (int)dbDataReader["f_ReaderID"])
												{
													dataView[0]["f_DoorName"] = this.arrAddrDoorName[i] + "[" + CommonStr.strInDoor + "]";
												}
											}
										}
										else if ((byte)dbDataReader["f_InOut"] == 0 && this.bRecordOut)
										{
											dataView[0]["f_bHave"] = 1;
											dataView[0]["f_ReadDate"] = ((DateTime)dbDataReader["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
											for (int j = 0; j < this.arrAddr.Length; j++)
											{
												if (this.arrAddrOut[j] == (int)dbDataReader["f_ReaderID"])
												{
													dataView[0]["f_DoorName"] = this.arrAddrDoorName[j] + "[" + CommonStr.strExitDoor + "]";
												}
											}
										}
									}
								}
							}
							dbDataReader.Close();
						}
					}
				}
				this.dtUsers.AcceptChanges();
				num = this.dtUsers.Rows.Count;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0015BAE4 File Offset: 0x0015AAE4
		private void DelayValidDateStripMenuItem3_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = null;
				int num = 0;
				string text = "";
				string text2 = "";
				int num2 = 0;
				if (this.warnInfoShow(sender, ref dataGridView, ref num, ref text, ref text2, ref num2) != 0)
				{
					string text3 = DateTime.Now.AddDays((double)(int)this.nudDays.Value).ToString(wgTools.DisplayFormat_DateYMD);
					string text4 = "";
					DateTime dateTime = DateTime.Now;
					using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
					{
						dfrmInputNewName.strNewName = text3;
						dfrmInputNewName.label1.Text = CommonStr.strDeactivateDateNew;
						dfrmInputNewName.Text = CommonStr.strDeactivateDateNewTitle;
						if (dfrmInputNewName.ShowDialog() == DialogResult.OK)
						{
							text4 = dfrmInputNewName.strNewName;
							if (!DateTime.TryParse(text4, out dateTime))
							{
								XMessageBox.Show(CommonStr.strDeactivateDateNewInvalid);
								return;
							}
							if (!(dateTime >= DateTime.Now))
							{
								XMessageBox.Show(CommonStr.strDeactivateDateNewTooSmall);
								return;
							}
							if (dateTime.Hour == 0)
							{
								text4 = dateTime.ToString("yyyy-MM-dd 23:59:59");
								dateTime = DateTime.Parse(text4);
							}
							else
							{
								text4 = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
							}
						}
					}
					if (!string.IsNullOrEmpty(text4))
					{
						string text5 = "";
						string text6 = "";
						int num3 = 2000;
						for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
						{
							num = dataGridView.SelectedRows[i].Index;
							if (dateTime > (DateTime)dataGridView.Rows[num].Cells["f_EndYMD"].Value)
							{
								text5 = text5 + dataGridView.Rows[num].Cells[0].Value.ToString() + ",";
								text6 = text6 + dataGridView.Rows[num].Cells[1].Value.ToString() + ",";
								dataGridView.Rows[num].Cells["f_EndYMD"].Value = dateTime;
								if (text5.Length > num3 - 100)
								{
									wgAppConfig.runSql(string.Format(" UPDATE t_b_Consumer SET f_EndYMD={0}  WHERE f_ConsumerID IN ({1} 0) AND (f_EndYMD < {2})", wgTools.PrepareStr(text4, true, "yyyy-MM-dd HH:mm:ss"), text5, wgTools.PrepareStr(text4, true, "yyyy-MM-dd HH:mm:ss")));
									text5 = "";
								}
							}
						}
						if (!string.IsNullOrEmpty(text5))
						{
							wgAppConfig.runSql(string.Format(" UPDATE t_b_Consumer SET f_EndYMD={0}  WHERE f_ConsumerID IN ({1} 0) AND (f_EndYMD < {2})", wgTools.PrepareStr(text4, true, "yyyy-MM-dd HH:mm:ss"), text5, wgTools.PrepareStr(text4, true, "yyyy-MM-dd HH:mm:ss")));
						}
						wgAppConfig.wgLog(text2 + "\r\nNew Date" + text4);
						if (!this.bShowNeedUploadFloor)
						{
							this.bShowNeedUploadFloor = true;
							XMessageBox.Show(CommonStr.strNeedUploadFloor);
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x0015BE04 File Offset: 0x0015AE04
		private void DeletePrivilegetoolStripMenuItem2_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = null;
				int num = 0;
				string text = "";
				string text2 = "";
				int num2 = 0;
				if (this.warnInfoShow(sender, ref dataGridView, ref num, ref text, ref text2, ref num2) != 0)
				{
					string text3 = "";
					string text4 = "";
					int num3 = 2000;
					for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
					{
						num = dataGridView.SelectedRows[i].Index;
						text3 = text3 + dataGridView.Rows[num].Cells[0].Value.ToString() + ",";
						text4 = text4 + dataGridView.Rows[num].Cells[1].Value.ToString() + ",";
						if (text3.Length > num3 - 100)
						{
							wgAppConfig.runSql(string.Format(" DELETE FROM [t_d_Privilege]  WHERE f_ConsumerID IN ({0} 0)", text3));
							text3 = "";
						}
					}
					if (!string.IsNullOrEmpty(text3))
					{
						wgAppConfig.runSql(string.Format(" DELETE FROM [t_d_Privilege]  WHERE f_ConsumerID IN ({0} 0)", text3));
					}
					foreach (object obj in dataGridView.SelectedRows)
					{
						DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
						dataGridView.Rows.Remove(dataGridViewRow);
					}
					wgAppConfig.wgLog(text2, EventLogEntryType.Information, null);
					if (!this.bShowNeedUploadFloor)
					{
						this.bShowNeedUploadFloor = true;
						XMessageBox.Show(CommonStr.strNeedUploadFloor);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0015BFD4 File Offset: 0x0015AFD4
		private void deleteSelectedPersonsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = null;
				int num = 0;
				string text = "";
				string text2 = "";
				int num2 = 0;
				if (this.warnInfoShow(sender, ref dataGridView, ref num, ref text, ref text2, ref num2) != 0)
				{
					icConsumer icConsumer = new icConsumer();
					if (dataGridView.SelectedRows.Count == 1)
					{
						int num3 = int.Parse(dataGridView.Rows[num].Cells[0].Value.ToString());
						icConsumer.dimissionUser(num3);
					}
					else
					{
						for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
						{
							num = dataGridView.SelectedRows[i].Index;
							int num4 = int.Parse(dataGridView.Rows[num].Cells[0].Value.ToString());
							icConsumer.dimissionUser(num4);
						}
					}
					foreach (object obj in dataGridView.SelectedRows)
					{
						DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
						dataGridView.Rows.Remove(dataGridViewRow);
					}
					wgAppConfig.wgLog(text2, EventLogEntryType.Information, null);
					if (!this.bShowNeedUploadFloor)
					{
						this.bShowNeedUploadFloor = true;
						XMessageBox.Show(CommonStr.strNeedUploadFloor);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0015C164 File Offset: 0x0015B164
		private void dfrmPersonsInside_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0015C17C File Offset: 0x0015B17C
		private void dfrmPersonsInside_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.dfrmFind1 == null)
					{
						this.dfrmFind1 = new dfrmFind();
					}
					this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0015C1EC File Offset: 0x0015B1EC
		private void dfrmPersonsInside_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.loadZoneInfo();
			this.tabPage1.BackColor = this.BackColor;
			this.tabPage2.BackColor = this.BackColor;
			this.tabPage3.BackColor = this.BackColor;
			this.tabPage4.BackColor = this.BackColor;
			this.tabPage5.BackColor = this.BackColor;
			this.loadGroupData();
			this.loadDoorData();
			this.dgvEnterIn.AutoGenerateColumns = false;
			this.dgvOutSide.AutoGenerateColumns = false;
			this.dgvGroupSubTotal.AutoGenerateColumns = false;
			this.dgvSwipe.AutoGenerateColumns = false;
			this.dgvNotSwipe.AutoGenerateColumns = false;
			this.dgvEnterIn.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvOutSide.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvGroupSubTotal.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSwipe.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvNotSwipe.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.NextRefreshTime = DateTime.Now.AddSeconds((double)this.nudCycleSecs.Value);
			this.dataGridViewTextBoxColumn5.HeaderText = wgAppConfig.ReplaceFloorRoom(this.dataGridViewTextBoxColumn5.HeaderText);
			this.dataGridViewTextBoxColumn14.HeaderText = wgAppConfig.ReplaceFloorRoom(this.dataGridViewTextBoxColumn14.HeaderText);
			this.dataGridViewTextBoxColumn1.HeaderText = wgAppConfig.ReplaceFloorRoom(this.dataGridViewTextBoxColumn1.HeaderText);
			this.tabPage3.Text = wgAppConfig.ReplaceFloorRoom(this.tabPage3.Text);
			this.dataGridViewTextBoxColumn3.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn3.HeaderText);
			this.dataGridViewTextBoxColumn12.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn12.HeaderText);
			string text = "mnuPrivilege";
			bool flag = false;
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.contextMenuStrip3.Visible = false;
			}
			flag = false;
			text = "mnuConsumers";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.contextMenuStrip3.Visible = false;
			}
			int num = 0;
			int.TryParse(wgAppConfig.GetKeyVal("KEY_PersonsInsideDayQuery"), out num);
			if (num > 0)
			{
				this.nudDays.Value = num;
			}
			num = 0;
			int.TryParse(wgAppConfig.GetKeyVal("KEY_PersonsInsideCycleSecs"), out num);
			if (num > 0)
			{
				this.nudCycleSecs.Value = num;
			}
			num = 0;
			int.TryParse(wgAppConfig.GetKeyVal("KEY_PersonsInsideAutoRefresh"), out num);
			if (num > 0)
			{
				this.chkAutoRefresh.Checked = true;
			}
			int.TryParse(wgAppConfig.GetKeyVal("KEY_PersonsInsideAllowExport"), out this.bAllowExport);
			this.activeExportToXMLToolStripMenuItem.Checked = this.bAllowExport > 0;
			int num2 = 0;
			int.TryParse(wgAppConfig.GetKeyVal("KEY_PersonsInsideReaderIn"), out num2);
			this.chkReaderIn.Checked = num2 == 0;
			num2 = 0;
			int.TryParse(wgAppConfig.GetKeyVal("KEY_PersonsInsideReaderOut"), out num2);
			this.chkReaderOut.Checked = num2 == 0;
			num2 = 0;
			int.TryParse(wgAppConfig.GetKeyVal("KEY_PersonsInsideZone"), out num2);
			if (num2 < this.cboZone.Items.Count)
			{
				this.cboZone.SelectedIndex = num2;
			}
			int.TryParse(wgAppConfig.GetKeyVal("KEY_PersonsInsideGroup"), out num2);
			if (num2 < this.cbof_GroupID.Items.Count)
			{
				this.cbof_GroupID.SelectedIndex = num2;
			}
			this.tabPage3.Text = wgAppConfig.ReplaceFloorRoom(this.tabPage3.Text);
			this.label6.Text = wgAppConfig.ReplaceFloorRoom(this.label6.Text);
			this.dataGridViewTextBoxColumn1.HeaderText = wgAppConfig.ReplaceFloorRoom(this.dataGridViewTextBoxColumn1.HeaderText);
			this.dataGridViewTextBoxColumn5.HeaderText = wgAppConfig.ReplaceFloorRoom(this.dataGridViewTextBoxColumn5.HeaderText);
			this.dataGridViewTextBoxColumn14.HeaderText = wgAppConfig.ReplaceFloorRoom(this.dataGridViewTextBoxColumn14.HeaderText);
			this.dataGridViewTextBoxColumn17.HeaderText = wgAppConfig.ReplaceFloorRoom(this.dataGridViewTextBoxColumn17.HeaderText);
			this.dataGridViewTextBoxColumn22.HeaderText = wgAppConfig.ReplaceFloorRoom(this.dataGridViewTextBoxColumn22.HeaderText);
			this.dataGridViewTextBoxColumn3.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn3.HeaderText);
			this.dataGridViewTextBoxColumn12.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn12.HeaderText);
			this.dataGridViewTextBoxColumn10.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn10.HeaderText);
			this.dataGridViewTextBoxColumn20.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn20.HeaderText);
			frmWatchingLED.getLedConfig();
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_SWITCH_CYCLE")))
			{
				int num3 = 0;
				int.TryParse(wgAppConfig.GetKeyVal("KEY_LED_SWITCH_CYCLE"), out num3);
				if (num3 > 0 && num3 < 5)
				{
					this.bActiveLEDSwitch = true;
				}
			}
			if (string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_PersonsInsideSqlReaders")))
			{
				this.btnSelectAll_Click(null, null);
			}
			else
			{
				string[] array = wgAppConfig.GetKeyVal("KEY_PersonsInsideSqlReaders").Split(new char[] { ',' });
				for (int i = 0; i < this.chkListDoors.Items.Count; i++)
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (this.arrAddr[i].ToString().Equals(array[j]))
						{
							this.chkListDoors.SetItemChecked(i, true);
						}
						if (this.arrAddrOut[i].ToString().Equals(array[j]))
						{
							this.chkListDoors.SetItemChecked(i, true);
						}
					}
				}
			}
			if (this.chkAutoRefresh.Checked)
			{
				this.btnQuery_Click(null, null);
			}
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0015C7A5 File Offset: 0x0015B7A5
		private void dgvEnterIn_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrmPersonsInside_KeyDown(this.dgvEnterIn, e);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0015C7B4 File Offset: 0x0015B7B4
		private void dgvGroupSubTotal_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrmPersonsInside_KeyDown(this.dgvGroupSubTotal, e);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0015C7C3 File Offset: 0x0015B7C3
		private void dgvNotSwipe_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrmPersonsInside_KeyDown(this.dgvNotSwipe, e);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0015C7D2 File Offset: 0x0015B7D2
		private void dgvOutSide_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrmPersonsInside_KeyDown(this.dgvOutSide, e);
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0015C7E1 File Offset: 0x0015B7E1
		private void dgvSwipe_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrmPersonsInside_KeyDown(this.dgvSwipe, e);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0015C7F0 File Offset: 0x0015B7F0
		public string getStrSql()
		{
			string text = "";
			this.strSqlDoorID = "";
			if (this.chkListDoors.CheckedItems.Count != 0)
			{
				for (int i = 0; i < this.chkListDoors.Items.Count; i++)
				{
					if (this.chkListDoors.GetItemChecked(i))
					{
						if (text == "")
						{
							this.strSqlDoorID += this.arrAddrDoorID[i].ToString();
							text += this.arrAddr[i].ToString();
						}
						else
						{
							this.strSqlDoorID = this.strSqlDoorID + "," + this.arrAddrDoorID[i].ToString();
							text = text + "," + this.arrAddr[i].ToString();
						}
						if (this.arrAddrOut[i] != 0)
						{
							text = text + "," + this.arrAddrOut[i].ToString();
						}
					}
				}
			}
			return text;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0015C90C File Offset: 0x0015B90C
		private void loadDoorData()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
			this.dt = new DataTable();
			this.dvDoors = new DataView(this.dt);
			this.dvDoors4Watching = new DataView(this.dt);
			this.dvSelected = new DataView(this.dt);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dt);
						}
					}
					goto IL_00FD;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dt);
					}
				}
			}
			IL_00FD:
			text = " SELECT c.*,b.f_ControllerSN ,  b.f_ZoneID ";
			text += " FROM t_b_Controller b, t_b_reader c WHERE c.f_ControllerID = b.f_ControllerID  ORDER BY  c.f_ReaderID ";
			this.dtReader = new DataTable();
			this.dvReader = new DataView(this.dtReader);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2))
					{
						using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(oleDbCommand2))
						{
							oleDbDataAdapter2.Fill(this.dtReader);
						}
					}
					goto IL_01E0;
				}
			}
			using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
				{
					using (SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sqlCommand2))
					{
						sqlDataAdapter2.Fill(this.dtReader);
					}
				}
			}
			IL_01E0:
			icControllerZone icControllerZone = new icControllerZone();
			icControllerZone.getAllowedControllers(ref this.dt);
			if (this.dvDoors.Count > 0)
			{
				this.arrAddr = new int[this.dvDoors.Count + 1];
				this.arrAddrOut = new int[this.dvDoors.Count + 1];
				this.arrAddrDoorID = new int[this.dvDoors.Count + 1];
				this.arrAddrDoorName = new string[this.dvDoors.Count + 1];
				for (int i = 0; i < this.dvDoors.Count; i++)
				{
					string text2 = wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]);
					this.chkListDoors.Items.Add(text2);
					this.arrAddrDoorID[i] = (int)this.dvDoors[i]["f_DoorID"];
					this.arrAddrDoorName[i] = (string)this.dvDoors[i]["f_DoorName"];
					this.arrAddr[i] = 0;
					this.arrAddrOut[i] = 0;
					if (wgMjController.GetControllerType((int)this.dvDoors[i]["f_ControllerSN"]) == 4)
					{
						this.dvReader.RowFilter = "f_ControllerSN = " + this.dvDoors[i]["f_ControllerSN"].ToString() + " AND f_ReaderNO=" + this.dvDoors[i]["f_DoorNO"].ToString();
						if (this.dvReader.Count > 0)
						{
							this.arrAddr[i] = (int)this.dvReader[0]["f_ReaderID"];
						}
					}
					else
					{
						string[] array = new string[]
						{
							"f_ControllerSN = ",
							this.dvDoors[i]["f_ControllerSN"].ToString(),
							" AND (f_ReaderNO=",
							((int)(((byte)this.dvDoors[i]["f_DoorNO"] - 1) * 2 + 1)).ToString(),
							" OR f_ReaderNO=",
							((int)(((byte)this.dvDoors[i]["f_DoorNO"] - 1) * 2 + 2)).ToString(),
							" )"
						};
						this.dvReader.RowFilter = string.Concat(array);
						this.dvReader.Sort = "f_ReaderNO ASC";
						if (this.dvReader.Count > 0)
						{
							this.arrAddr[i] = (int)this.dvReader[0]["f_ReaderID"];
							if (this.dvReader.Count > 1)
							{
								this.arrAddrOut[i] = (int)this.dvReader[1]["f_ReaderID"];
							}
						}
					}
				}
			}
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0015CEA8 File Offset: 0x0015BEA8
		private void loadGroupData()
		{
			new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
			for (int i = 0; i < this.arrGroupID.Count; i++)
			{
				if (i == 0 && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
				{
					this.cbof_GroupID.Items.Add(CommonStr.strAll);
				}
				else
				{
					this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
				}
			}
			if (this.cbof_GroupID.Items.Count > 0)
			{
				this.cbof_GroupID.SelectedIndex = 0;
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0015CF5C File Offset: 0x0015BF5C
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0015CF7C File Offset: 0x0015BF7C
		private void loadZoneInfo()
		{
			new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
			int i = this.arrZoneID.Count;
			this.cboZone.Items.Clear();
			for (i = 0; i < this.arrZoneID.Count; i++)
			{
				if (i == 0 && string.IsNullOrEmpty(this.arrZoneName[i].ToString()))
				{
					this.cboZone.Items.Add(CommonStr.strAllZones);
				}
				else
				{
					this.cboZone.Items.Add(this.arrZoneName[i].ToString());
				}
			}
			if (this.cboZone.Items.Count > 0)
			{
				this.cboZone.SelectedIndex = 0;
			}
			bool flag = true;
			this.label25.Visible = flag;
			this.cboZone.Visible = flag;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0015D068 File Offset: 0x0015C068
		private void outputFile4InsideUsers(int cnt)
		{
			try
			{
				if (this.lastIn != cnt)
				{
					this.lastIn = cnt;
					using (StreamWriter streamWriter = new StreamWriter(wgAppConfig.Path4Doc() + "n3k_in.txt", false))
					{
						streamWriter.Write(this.lastIn.ToString());
					}
				}
				try
				{
					if (this.bAllowExport > 0)
					{
						DatatableToJson.CDataToXmlFile(this.dvIn, wgAppConfig.Path4Doc() + "PersonsInside_In.json");
						DatatableToJson.CDataToXmlFile(this.dvOut, wgAppConfig.Path4Doc() + "PersonsInside_Out.json");
						DatatableToJson.CDataToXmlFile(this.dvGroup, wgAppConfig.Path4Doc() + "PersonsInside_Group.json");
						DatatableToJson.CDataToXmlFile(this.dvSwipe, wgAppConfig.Path4Doc() + "PersonsInside_Swipe.json");
						DatatableToJson.CDataToXmlFile(this.dvNotSwipe, wgAppConfig.Path4Doc() + "PersonsInside_NotSwipe.json");
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0015D1A0 File Offset: 0x0015C1A0
		private void SelectAlltoolStripMenuItem1_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView;
				if (this.tabControl1.SelectedTab == this.tabPage1)
				{
					dataGridView = this.dgvEnterIn;
				}
				else if (this.tabControl1.SelectedTab == this.tabPage2)
				{
					dataGridView = this.dgvOutSide;
				}
				else if (this.tabControl1.SelectedTab == this.tabPage4)
				{
					dataGridView = this.dgvSwipe;
				}
				else
				{
					if (this.tabControl1.SelectedTab != this.tabPage5)
					{
						return;
					}
					dataGridView = this.dgvNotSwipe;
				}
				dataGridView.SelectAll();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0015D248 File Offset: 0x0015C248
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.chkAutoRefresh.Checked && this.btnQuery2.Enabled && DateTime.Now > this.NextRefreshTime)
			{
				this.NextRefreshTime = DateTime.Now.AddSeconds((double)this.nudCycleSecs.Value);
				this.btnQuery_Click(null, null);
			}
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0015D2B0 File Offset: 0x0015C2B0
		private int warnInfoShow(object sender, ref DataGridView dgv, ref int selectRowId, ref string msgInfo, ref string detailInfo, ref int selectedUser)
		{
			if (this.tabControl1.SelectedTab == this.tabPage1)
			{
				dgv = this.dgvEnterIn;
			}
			else if (this.tabControl1.SelectedTab == this.tabPage2)
			{
				dgv = this.dgvOutSide;
			}
			else if (this.tabControl1.SelectedTab == this.tabPage4)
			{
				dgv = this.dgvSwipe;
			}
			else
			{
				if (this.tabControl1.SelectedTab != this.tabPage5)
				{
					return 0;
				}
				dgv = this.dgvNotSwipe;
			}
			if (dgv.SelectedRows.Count <= 0)
			{
				if (dgv.SelectedCells.Count <= 0)
				{
					return 0;
				}
				selectRowId = dgv.SelectedCells[0].RowIndex;
			}
			else
			{
				selectRowId = dgv.SelectedRows[0].Index;
			}
			if (dgv.SelectedRows.Count <= 0)
			{
				return 0;
			}
			string text;
			if (dgv.SelectedRows.Count == 1)
			{
				text = string.Format("{0}\r\n\r\n{1}:  {2}", (sender as ToolStripMenuItem).Text, dgv.Columns[2].HeaderText, dgv.Rows[selectRowId].Cells[2].Value.ToString());
			}
			else
			{
				text = string.Format("{0}\r\n\r\n{1}=  {2}", (sender as ToolStripMenuItem).Text, CommonStr.strUsersNum, dgv.SelectedRows.Count);
			}
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				return 0;
			}
			string text2 = (sender as ToolStripMenuItem).Text;
			if (dgv.SelectedRows.Count == 1)
			{
				msgInfo = string.Format("{0} {1}:  {2}", text2, dgv.Columns[2].HeaderText, dgv.Rows[selectRowId].Cells[2].Value.ToString());
				detailInfo = string.Format("{0}-{1}-{2}-{3}:  {4}", new object[]
				{
					text2,
					dgv.Rows[selectRowId].Cells[0].Value.ToString(),
					dgv.Rows[selectRowId].Cells[1].Value.ToString(),
					dgv.Columns[2].HeaderText,
					dgv.Rows[selectRowId].Cells[2].Value.ToString()
				});
			}
			else
			{
				msgInfo = string.Format("{0} {1}=  {2}", text2, CommonStr.strUsersNum, dgv.SelectedRows.Count);
				msgInfo += string.Format("From {0}...", dgv.Rows[selectRowId].Cells[2].Value.ToString());
				detailInfo = msgInfo;
			}
			selectedUser = dgv.SelectedRows.Count;
			return 1;
		}

		// Token: 0x040020E7 RID: 8423
		private bool bActiveLEDSwitch;

		// Token: 0x040020E8 RID: 8424
		private int bAllowExport;

		// Token: 0x040020E9 RID: 8425
		private bool bOnlyCheckNewRecord;

		// Token: 0x040020EA RID: 8426
		private bool bShowNeedUploadFloor;

		// Token: 0x040020EB RID: 8427
		private DataTable dt;

		// Token: 0x040020EC RID: 8428
		private DataTable dtGroups;

		// Token: 0x040020ED RID: 8429
		private DataTable dtReader;

		// Token: 0x040020EE RID: 8430
		private DataTable dtUsers;

		// Token: 0x040020EF RID: 8431
		private DataView dv;

		// Token: 0x040020F0 RID: 8432
		private DataView dvDoors;

		// Token: 0x040020F1 RID: 8433
		private DataView dvDoors4Watching;

		// Token: 0x040020F2 RID: 8434
		private DataView dvGroup;

		// Token: 0x040020F3 RID: 8435
		private DataView dvIn;

		// Token: 0x040020F4 RID: 8436
		private DataView dvNotSwipe;

		// Token: 0x040020F5 RID: 8437
		private DataView dvOut;

		// Token: 0x040020F6 RID: 8438
		private DataView dvReader;

		// Token: 0x040020F7 RID: 8439
		private DataView dvSelected;

		// Token: 0x040020F8 RID: 8440
		private DataView dvSwipe;

		// Token: 0x040020F9 RID: 8441
		private int recIndexDealt;

		// Token: 0x040020FA RID: 8442
		private string strSqlDoorID;

		// Token: 0x040020FB RID: 8443
		private string strSqlReaders;

		// Token: 0x040020FC RID: 8444
		private int[] arrAddr;

		// Token: 0x040020FD RID: 8445
		private int[] arrAddrDoorID;

		// Token: 0x040020FE RID: 8446
		private string[] arrAddrDoorName;

		// Token: 0x040020FF RID: 8447
		private int[] arrAddrOut;

		// Token: 0x04002100 RID: 8448
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04002101 RID: 8449
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04002102 RID: 8450
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04002103 RID: 8451
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x04002104 RID: 8452
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x04002105 RID: 8453
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x04002106 RID: 8454
		private bool bRecordIn = true;

		// Token: 0x04002107 RID: 8455
		private bool bRecordOut = true;

		// Token: 0x04002109 RID: 8457
		public bool dataNeedRefresh4Lcd;

		// Token: 0x0400210A RID: 8458
		public DataTable dtEnterInShare = new DataTable();

		// Token: 0x0400210B RID: 8459
		public DataTable dtGroupsShare = new DataTable();

		// Token: 0x0400210C RID: 8460
		public frmConsole frmCall;

		// Token: 0x0400210D RID: 8461
		private int lastIn = -1;

		// Token: 0x0400210E RID: 8462
		private int lastRecID = -1;

		// Token: 0x0400210F RID: 8463
		private string laststrKEY_PersonsInsideAutoRefresh = "";

		// Token: 0x04002110 RID: 8464
		private string laststrKEY_PersonsInsideCycleSecs = "";

		// Token: 0x04002111 RID: 8465
		private string laststrKEY_PersonsInsideDayQuery = "";

		// Token: 0x04002112 RID: 8466
		private string laststrKEY_PersonsInsideGroup = "";

		// Token: 0x04002113 RID: 8467
		private string laststrKEY_PersonsInsideReaderIn = "";

		// Token: 0x04002114 RID: 8468
		private string laststrKEY_PersonsInsideReaderOut = "";

		// Token: 0x04002115 RID: 8469
		private string laststrKEY_PersonsInsideSqlReaders = "";

		// Token: 0x04002116 RID: 8470
		private string laststrKEY_PersonsInsideZone = "";

		// Token: 0x04002117 RID: 8471
		private string laststrSqlReaders = "";

		// Token: 0x04002118 RID: 8472
		private CheckedListBox listViewNotDisplay = new CheckedListBox();

		// Token: 0x04002119 RID: 8473
		public DateTime NextRefreshTime;

		// Token: 0x0400211A RID: 8474
		private string strGroupFilter = "";

		// Token: 0x0400216A RID: 8554
		private DateTime tmStop;
	}
}
