using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.ExtendFunc;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000052 RID: 82
	public partial class frmSwipeRecords : frmN3000
	{
		// Token: 0x060005E8 RID: 1512 RVA: 0x000A4E68 File Offset: 0x000A3E68
		public frmSwipeRecords()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSwipeRecords);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000A4EC8 File Offset: 0x000A3EC8
		private string addTextFile(DataRow dr, string Field, bool bFixed, string Info, int typeA, string dispVal)
		{
			string text;
			if (string.IsNullOrEmpty(Field))
			{
				text = Info;
			}
			else
			{
				text = dr[Field].ToString();
			}
			if (!bFixed)
			{
				if (3 == typeA && !string.IsNullOrEmpty(Info))
				{
					text = DateTime.Parse(text).ToString(Info);
				}
				if (4 == typeA && !string.IsNullOrEmpty(Info))
				{
					DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
					dateTimeFormatInfo.ShortDatePattern = Info;
					text = DateTime.Parse(text).ToString(Info, dateTimeFormatInfo);
				}
				return text;
			}
			int num = int.Parse(Info);
			if (1 == typeA)
			{
				text = text.Trim().PadLeft(num, '0');
				return text.Substring(text.Length - num, num);
			}
			return text.PadRight(num, ' ').Substring(0, num);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000A4F84 File Offset: 0x000A3F84
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

		// Token: 0x060005EB RID: 1515 RVA: 0x000A5004 File Offset: 0x000A4004
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
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvSwipeRecords.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000A50BC File Offset: 0x000A40BC
		private void btnCameraView_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.photoavi == null || this.photoavi.IsDisposed)
				{
					this.photoavi = null;
					this.photoavi = new dfrmPhotoAvi();
					this.photoavi.frmCaller = this;
				}
				this.photoavi.FormBorderStyle = FormBorderStyle.FixedDialog;
				this.photoavi.btnEnd.Visible = false;
				this.photoavi.btnFirst.Visible = false;
				this.photoavi.bWatching = false;
				this.photoavi.MaximizeBox = false;
				this.photoavi.StartPosition = FormStartPosition.CenterParent;
				if (this.photoavi.Visible)
				{
					this.photoavi.Hide();
				}
				this.photoavi.Show(this);
				this.dgvSwipeRecords_SelectionChanged(null, null);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x000A519C File Offset: 0x000A419C
		private void btnDelete_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripButton).Text;
				dfrmInputNewName.label1.Text = CommonStr.strSelectMaxRecID;
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					int num;
					if (int.TryParse(dfrmInputNewName.strNewName, out num))
					{
						string text = "DELETE FROM t_d_SwipeRecord  WHERE f_RecID < " + num.ToString();
						int num2 = wgAppConfig.runUpdateSql(text);
						wgAppConfig.wgLog("strsql =" + text);
						wgAppConfig.wgLog("Deleted Records' Count =" + num2.ToString());
						XMessageBox.Show(CommonStr.strDeletedSwipeRecordCount + num2.ToString());
					}
					else
					{
						XMessageBox.Show(CommonStr.strNumericWrong);
					}
				}
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000A5268 File Offset: 0x000A4268
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			DataGridView dataGridView = this.dgvSwipeRecords;
			if (dataGridView.Rows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strNoDataToExport);
				return;
			}
			if (dataGridView.Rows.Count <= 65535 && !this.bLoadedFinished)
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
						this.backgroundWorker1.RunWorkerAsync(new object[]
						{
							this.startRecordIndex,
							66000 - dataGridView.Rows.Count,
							this.dgvSql
						});
						while (this.backgroundWorker1.IsBusy)
						{
							Thread.Sleep(500);
							Application.DoEvents();
						}
						this.startRecordIndex = this.startRecordIndex + 66000 - dataGridView.Rows.Count - this.MaxRecord;
						if (dataGridView.Rows.Count > 65535)
						{
							IL_0161:
							dfrmWait.Hide();
							goto IL_0173;
						}
					}
					wgAppRunInfo.raiseAppRunInfoLoadNums(dataGridView.Rows.Count.ToString() + "#");
					goto IL_0161;
				}
			}
			IL_0173:
			foreach (object obj in this.dgvSwipeRecords.Columns)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj;
				if (dataGridViewColumn.Name.Equals("f_Desc"))
				{
					foreach (object obj2 in ((IEnumerable)this.dgvSwipeRecords.Rows))
					{
						DataGridViewRow dataGridViewRow = (DataGridViewRow)obj2;
						DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[dataGridViewColumn.Index];
						if (dataGridViewCell.Value != null && dataGridViewCell.Value as string == " ")
						{
							string text = dataGridViewRow.Cells[dataGridViewColumn.Index + 1].Value as string;
							MjRec mjRec = new MjRec(text.PadLeft(48, '0'));
							dataGridViewCell.Value = mjRec.GetDetailedRecord(null, 0U);
							if (wgMjController.IsElevator((int)mjRec.ControllerSN))
							{
								if (mjRec.IsRemoteOpen)
								{
									if (mjRec.floorNo == 0)
									{
										dataGridViewCell.Value += string.Format("{0}", CommonStr.strRecordRemoteOpenDoor4MultiFloor1);
									}
									else
									{
										this.dvFloor.RowFilter = string.Format("f_ReaderName = '{0}' AND f_floorNO = {1} ", dataGridViewRow.Cells["f_ReaderName"].Value, mjRec.floorNo);
										if (this.dvFloor.Count >= 1)
										{
											dataGridViewCell.Value += string.Format("{0}{1}", CommonStr.strRecordRemoteOpenDoor4Floor, " [" + this.dvFloor[0]["f_floorFullName"].ToString() + "]");
										}
										else
										{
											dataGridViewCell.Value += string.Format("{0}{1}", CommonStr.strRecordRemoteOpenDoor4Floor, mjRec.floorNo);
										}
									}
								}
								else if (mjRec.floorNo > 0)
								{
									this.dvFloor.RowFilter = string.Format("f_ReaderName = '{0}' AND f_floorNO = {1} ", dataGridViewRow.Cells["f_ReaderName"].Value, mjRec.floorNo);
									if (this.dvFloor.Count >= 1)
									{
										object value = dataGridViewCell.Value;
										dataGridViewCell.Value = string.Concat(new object[]
										{
											value,
											" [",
											this.dvFloor[0]["f_floorFullName"].ToString(),
											"]"
										});
									}
								}
							}
							else if (!mjRec.IsSwipeRecord && (mjRec.ReasonNo == 20 || mjRec.ReasonNo == 21 || mjRec.ReasonNo == 22 || mjRec.ReasonNo == 23 || mjRec.ReasonNo == 24))
							{
								string text2 = dataGridViewRow.Cells["f_ReaderName"].Value as string;
								if (text2.LastIndexOf("-") > 0)
								{
									text2 = text2.Substring(0, text2.LastIndexOf("-"));
									dataGridViewRow.Cells["f_ReaderName"].Value = text2;
								}
							}
						}
					}
				}
			}
			wgAppConfig.exportToExcel(this.dgvSwipeRecords, this.Text);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000A57C4 File Offset: 0x000A47C4
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvSwipeRecords);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000A57D8 File Offset: 0x000A47D8
		private void btnFindOption_Click(object sender, EventArgs e)
		{
			if (this.dfrmFindOption == null)
			{
				this.dfrmFindOption = new dfrmSwipeRecordsFindOption();
				this.dfrmFindOption.Owner = this;
			}
			this.dfrmFindOption.Show();
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000A5804 File Offset: 0x000A4804
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvSwipeRecords, this.Text);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000A5818 File Offset: 0x000A4818
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
			if (this.dfrmFindOption != null && this.dfrmFindOption.Visible)
			{
				flag = true;
				text2 = this.dfrmFindOption.getStrSql();
			}
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text3 = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
			text3 += "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_ReaderName,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll         ,0 as f_CaptureFileExisted ";
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

		// Token: 0x060005F3 RID: 1523 RVA: 0x000A5901 File Offset: 0x000A4901
		private void cboEnd_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cboEnd.SelectedIndex == 1)
			{
				this.dtpDateTo.Enabled = true;
				return;
			}
			this.dtpDateTo.Enabled = false;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000A592A File Offset: 0x000A492A
		private void cboStart_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cboStart.SelectedIndex == 1)
			{
				this.dtpDateFrom.Enabled = true;
				return;
			}
			this.dtpDateFrom.Enabled = false;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000A5954 File Offset: 0x000A4954
		private void deletedUsersSwipeQueryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dfrmDeletedUsersManage dfrmDeletedUsersManage = new dfrmDeletedUsersManage();
			int num;
			if (dfrmDeletedUsersManage.ShowDialog() == DialogResult.OK && int.TryParse(dfrmDeletedUsersManage.selectedUsers, out num))
			{
				string text = "";
				bool flag = false;
				if (this.dfrmFindOption != null && this.dfrmFindOption.Visible)
				{
					flag = true;
					text = this.dfrmFindOption.getStrSql();
				}
				string text2 = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
				text2 += "         t_b_Consumer_delete.f_ConsumerNO, t_b_Consumer_delete.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_ReaderName,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll         ,0 as f_CaptureFileExisted ";
				string text3 = " ( 1>0 ) ";
				if (this.getSqlOfDateTime() != "")
				{
					text3 += string.Format(" AND {0} ", this.getSqlOfDateTime());
				}
				if (flag)
				{
					text3 += string.Format(" AND {0} ", text);
				}
				int num2 = num;
				string text4 = "t_d_SwipeRecord";
				string text5 = " WHERE (1>0) ";
				if (!string.IsNullOrEmpty(text3))
				{
					text5 += string.Format("AND {0}", text3);
				}
				string text6 = string.Format("AND   t_b_Consumer_delete.f_ConsumerID ={0:d} ", num2);
				string text7 = text2 + string.Format(" FROM ((t_b_Consumer_delete INNER JOIN {0} ON ( t_b_Consumer_delete.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN  t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID) ) LEFT JOIN t_b_Group ON (t_b_Consumer_delete.f_GroupID = t_b_Group.f_GroupID  ) ", text4, text6) + text5;
				this.reloadData(text7);
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000A5A77 File Offset: 0x000A4A77
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000A5A8C File Offset: 0x000A4A8C
		private void dfrm_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					wgAppConfig.findCall(this.dfrmFind1, this, this.dgvSwipeRecords);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000A5AE8 File Offset: 0x000A4AE8
		private void dgvSwipeRecords_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvSwipeRecords.Columns.Count && this.dgvSwipeRecords.Columns[e.ColumnIndex].Name.Equals("f_Desc"))
			{
				string text = e.Value as string;
				if (!string.IsNullOrEmpty(text) && text != " ")
				{
					if (this.bCameraView && (int)this.dgvSwipeRecords[e.ColumnIndex + 2, e.RowIndex].Value == 1)
					{
						this.dgvSwipeRecords[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LightBlue;
						return;
					}
				}
				else
				{
					DataGridViewCell dataGridViewCell = this.dgvSwipeRecords[e.ColumnIndex, e.RowIndex];
					string text2 = this.dgvSwipeRecords[e.ColumnIndex + 1, e.RowIndex].Value as string;
					if (string.IsNullOrEmpty(text2))
					{
						e.Value = "  ";
						dataGridViewCell.Value = "  ";
						return;
					}
					MjRec mjRec = new MjRec(text2.PadLeft(48, '0'));
					e.Value = mjRec.GetDetailedRecord(null, 0U);
					string sfzname = mjRec.getSFZName();
					if (!string.IsNullOrEmpty(sfzname) && string.IsNullOrEmpty(wgTools.SetObjToStr(this.dgvSwipeRecords.Rows[e.RowIndex].Cells["f_ConsumerName"].Value)))
					{
						this.dgvSwipeRecords.Rows[e.RowIndex].Cells["f_ConsumerName"].Value = sfzname;
					}
					if (wgMjController.IsElevator((int)mjRec.ControllerSN))
					{
						if (mjRec.IsRemoteOpen)
						{
							if (mjRec.floorNo == 0)
							{
								e.Value += string.Format("{0}", CommonStr.strRecordRemoteOpenDoor4MultiFloor1);
							}
							else
							{
								this.dvFloor.RowFilter = string.Format("f_ReaderName = '{0}' AND f_floorNO = {1} ", this.dgvSwipeRecords["f_ReaderName", e.RowIndex].Value, mjRec.floorNo);
								if (this.dvFloor.Count >= 1)
								{
									e.Value += string.Format("{0}{1}", CommonStr.strRecordRemoteOpenDoor4Floor, " [" + this.dvFloor[0]["f_floorFullName"].ToString() + "]");
								}
								else
								{
									e.Value += string.Format("{0}{1}", CommonStr.strRecordRemoteOpenDoor4Floor, mjRec.floorNo);
								}
							}
						}
						else if (mjRec.floorNo > 0)
						{
							this.dvFloor.RowFilter = string.Format("f_ReaderName = '{0}' AND f_floorNO = {1} ", this.dgvSwipeRecords["f_ReaderName", e.RowIndex].Value, mjRec.floorNo);
							if (this.dvFloor.Count >= 1)
							{
								object value = e.Value;
								e.Value = string.Concat(new object[]
								{
									value,
									" [",
									this.dvFloor[0]["f_floorFullName"].ToString(),
									"]"
								});
							}
						}
					}
					else if (!mjRec.IsSwipeRecord && (mjRec.ReasonNo == 20 || mjRec.ReasonNo == 21 || mjRec.ReasonNo == 22 || mjRec.ReasonNo == 23 || mjRec.ReasonNo == 24))
					{
						string text3 = this.dgvSwipeRecords.Rows[e.RowIndex].Cells["f_ReaderName"].Value as string;
						if (text3.LastIndexOf("-") > 0)
						{
							text3 = text3.Substring(0, text3.LastIndexOf("-"));
							this.dgvSwipeRecords.Rows[e.RowIndex].Cells["f_ReaderName"].Value = text3;
						}
					}
					if (string.IsNullOrEmpty(wgTools.SetObjToStr(e.Value)))
					{
						e.Value = "  ";
					}
					dataGridViewCell.Value = e.Value;
					if (this.bCameraView)
					{
						string text4 = mjRec.ReadDate.ToString("yyyyMMdd_HHmmss_") + mjRec.CardID.ToString() + "_" + mjRec.ToStringRaw();
						if (wgAppConfig.FileIsExisted(this.avijpgPath + text4 + ".jpg"))
						{
							dataGridViewCell.Style.BackColor = Color.LightBlue;
							this.dgvSwipeRecords[e.ColumnIndex + 2, e.RowIndex].Value = 1;
						}
					}
				}
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000A5FF4 File Offset: 0x000A4FF4
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

		// Token: 0x060005FA RID: 1530 RVA: 0x000A6134 File Offset: 0x000A5134
		private void dgvSwipeRecords_SelectionChanged(object sender, EventArgs e)
		{
			if (this.dgvSwipeRecords.SelectedRows.Count > 0 && this.photoavi != null)
			{
				if (this.photoavi.IsDisposed)
				{
					this.photoavi = null;
					return;
				}
				if (!string.IsNullOrEmpty(this.dgvSwipeRecords.SelectedRows[0].Cells["f_RecordAll"].Value as string))
				{
					this.photoavi.Text = this.dgvSwipeRecords.SelectedRows[0].Cells[0].Value.ToString();
					MjRec mjRec = new MjRec(this.dgvSwipeRecords.SelectedRows[0].Cells["f_RecordAll"].Value as string);
					mjRec.address = this.dgvSwipeRecords.SelectedRows[0].Cells["f_ReaderName"].Value as string;
					this.photoavi.newCardNo = mjRec.CardID;
					if (!mjRec.IsUserCardNO)
					{
						this.photoavi.newCardNo = (long)((ulong)(-1));
					}
					this.photoavi.fileName = mjRec.ReadDate.ToString("yyyyMMdd_HHmmss_") + mjRec.CardID.ToString() + "_" + mjRec.ToStringRaw();
					this.photoavi.newCardInfo = mjRec.ToDisplayDetail().Replace("\t", " ");
					this.photoavi.lblCount.Text = this.dgvSwipeRecords.SelectedRows[0].Cells["f_RecID"].Value.ToString();
					this.photoavi.reload();
				}
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000A6304 File Offset: 0x000A5304
		private void exportOver65535ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataGridView dataGridView = this.dgvSwipeRecords;
			if (dataGridView.Rows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strNoDataToExport);
				return;
			}
			if (!this.bLoadedFinished)
			{
				if (XMessageBox.Show(this.exportOver65535ToolStripMenuItem.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
				{
					return;
				}
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
					dfrmWait.Text = dataGridView.Rows.Count.ToString();
					wgAppRunInfo.raiseAppRunInfoLoadNums(dataGridView.Rows.Count.ToString() + "#");
					this.MaxRecord = 1000;
					dfrmWait.Hide();
				}
			}
			int num = -1;
			foreach (object obj in this.dgvSwipeRecords.Columns)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj;
				if (dataGridViewColumn.Name.Equals("f_Desc"))
				{
					num = dataGridViewColumn.Index;
					break;
				}
			}
			wgAppConfig.exportToExcel100W4SwipeRecord(dataGridView, this.Text, this.dvFloor, num);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000A6540 File Offset: 0x000A5540
		private void exportTextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataGridView dataGridView = this.dgvSwipeRecords;
			if (dataGridView.Rows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strNoDataToExport);
				return;
			}
			if (string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_ExportTextStyle")))
			{
				XMessageBox.Show(CommonStr.strExportTextStyleNoSet);
				return;
			}
			if (!this.bLoadedFinished)
			{
				if (XMessageBox.Show(this.exportTextToolStripMenuItem.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
				{
					return;
				}
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
					dfrmWait.Text = dataGridView.Rows.Count.ToString();
					wgAppRunInfo.raiseAppRunInfoLoadNums(dataGridView.Rows.Count.ToString() + "#");
					this.MaxRecord = 1000;
					dfrmWait.Hide();
				}
			}
			string text = "";
			string text2 = this.Text;
			try
			{
				string text3;
				if (string.IsNullOrEmpty(text2))
				{
					text3 = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_ff") + ".txt";
				}
				else
				{
					text3 = text2 + DateTime.Now.ToString("-yyyy-MM-dd_HHmmss_ff") + ".txt";
				}
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.FileName = text3;
					saveFileDialog.Filter = " (*.txt)|*.txt";
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						text = saveFileDialog.FileName;
						text3 = text;
						using (dfrmWait dfrmWait2 = new dfrmWait())
						{
							dfrmWait2.Show();
							dfrmWait2.Refresh();
							StreamWriter streamWriter = new StreamWriter(text3);
							int num = 0;
							string keyVal = wgAppConfig.GetKeyVal("KEY_ExportTextStyle");
							ArrayList arrayList = new ArrayList();
							ArrayList arrayList2 = new ArrayList();
							ArrayList arrayList3 = new ArrayList();
							ArrayList arrayList4 = new ArrayList();
							new ArrayList();
							if (!string.IsNullOrEmpty(keyVal))
							{
								string[] array = keyVal.Split(new char[] { '"' });
								if (array.Length > 0)
								{
									for (int i = 0; i < array.Length; i++)
									{
										string[] array2 = array[i].Split(new char[] { '\'' });
										if (array2.Length == 5)
										{
											int num2 = 0;
											string text4 = array2[3].ToString();
											if (text4 != null)
											{
												if (!(text4 == "PadLeft"))
												{
													if (!(text4 == "PadRight"))
													{
														if (!(text4 == "DateTime"))
														{
															if (text4 == "Date")
															{
																num2 = 4;
															}
														}
														else
														{
															num2 = 3;
														}
													}
													else
													{
														num2 = 2;
													}
												}
												else
												{
													num2 = 1;
												}
											}
											arrayList.Add(array2[0]);
											arrayList2.Add(bool.Parse(array2[1]));
											arrayList3.Add(array2[2]);
											arrayList4.Add(num2);
										}
									}
								}
							}
							int num3 = -1;
							for (int j = 0; j < arrayList.Count; j++)
							{
								if ((string)arrayList[j] == "f_Desc")
								{
									using (IEnumerator enumerator = this.dgvSwipeRecords.Columns.GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											object obj = enumerator.Current;
											DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj;
											if (dataGridViewColumn.Name.Equals("f_Desc"))
											{
												num3 = dataGridViewColumn.Index;
												break;
											}
										}
										break;
									}
								}
							}
							DataTable dataTable = this.dgvSwipeRecords.DataSource as DataTable;
							for (int k = 0; k < dataTable.Rows.Count; k++)
							{
								string text5 = "";
								if (num3 >= 0)
								{
									string text6 = dataTable.Rows[k][num3].ToString();
									if (text6 != null && text6 == " ")
									{
										string text7 = dataTable.Rows[k][num3 + 1] as string;
										MjRec mjRec = new MjRec(text7.PadLeft(48, '0'));
										text6 = mjRec.GetDetailedRecord(null, 0U);
										if (wgMjController.IsElevator((int)mjRec.ControllerSN))
										{
											if (mjRec.IsRemoteOpen)
											{
												if (mjRec.floorNo == 0)
												{
													text6 += string.Format("{0}", CommonStr.strRecordRemoteOpenDoor4MultiFloor1);
												}
												else
												{
													this.dvFloor.RowFilter = string.Format("f_ReaderName = '{0}' AND f_floorNO = {1} ", dataTable.Rows[k]["f_ReaderName"], mjRec.floorNo);
													if (this.dvFloor.Count >= 1)
													{
														text6 += string.Format("{0}{1}", CommonStr.strRecordRemoteOpenDoor4Floor, " [" + this.dvFloor[0]["f_floorFullName"].ToString() + "]");
													}
													else
													{
														text6 += string.Format("{0}{1}", CommonStr.strRecordRemoteOpenDoor4Floor, mjRec.floorNo);
													}
												}
											}
											else if (mjRec.floorNo > 0)
											{
												this.dvFloor.RowFilter = string.Format("f_ReaderName = '{0}' AND f_floorNO = {1} ", dataTable.Rows[k]["f_ReaderName"], mjRec.floorNo);
												if (this.dvFloor.Count >= 1)
												{
													text6 = text6 + " [" + this.dvFloor[0]["f_floorFullName"].ToString() + "]";
												}
											}
										}
										else if (!mjRec.IsSwipeRecord && (mjRec.ReasonNo == 20 || mjRec.ReasonNo == 21 || mjRec.ReasonNo == 22 || mjRec.ReasonNo == 23 || mjRec.ReasonNo == 24))
										{
											string text8 = dataTable.Rows[k]["f_ReaderName"] as string;
											if (text8.LastIndexOf("-") > 0)
											{
												text8 = text8.Substring(0, text8.LastIndexOf("-"));
												dataTable.Rows[k]["f_ReaderName"] = text8;
											}
										}
										dataTable.Rows[k][num3] = text6;
									}
								}
								for (int l = 0; l < arrayList.Count; l++)
								{
									text5 += this.addTextFile(dataTable.Rows[k], (string)arrayList[l], (bool)arrayList2[l], (string)arrayList3[l], (int)arrayList4[l], "");
								}
								streamWriter.Write(text5);
								streamWriter.Write("\r\n");
								if (num % 1000 == 0)
								{
									dfrmWait2.Text = num.ToString();
									wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num));
									Application.DoEvents();
								}
								num++;
							}
							streamWriter.Close();
							dfrmWait2.Text = num.ToString();
							wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num));
							dfrmWait2.Hide();
							XMessageBox.Show(string.Concat(new string[]
							{
								CommonStr.strExportRecords,
								" = ",
								num.ToString(),
								"\t\r\n\r\n",
								this.exportTextFileToolStripMenuItem.Text,
								" ",
								text
							}));
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine("ExportToExcel" + text + ex.ToString());
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000A6E90 File Offset: 0x000A5E90
		private void fillDgv(DataTable dt)
		{
			try
			{
				if (this.bLoad4Export && this.dt4Dgv != null)
				{
					this.dt4Dgv.Merge(dt);
					return;
				}
				if (this.dgvSwipeRecords.DataSource == null)
				{
					this.dt4Dgv = dt;
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
					this.dt4Dgv.Merge(dt);
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

		// Token: 0x060005FE RID: 1534 RVA: 0x000A7058 File Offset: 0x000A6058
		private void frmSwipeRecords_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.backgroundWorker1.IsBusy)
				{
					this.backgroundWorker1.Dispose();
				}
			}
			catch
			{
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x000A7094 File Offset: 0x000A6094
		private void frmSwipeRecords_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFindOption != null)
			{
				this.dfrmFindOption.Close();
			}
			try
			{
				if (this.photoavi != null)
				{
					if (!this.photoavi.IsDisposed)
					{
						this.photoavi.bWatching = false;
						this.photoavi.stopVideo();
						this.photoavi.Close();
					}
					this.photoavi = null;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.dfrm_FormClosing(sender, e);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000A711C File Offset: 0x000A611C
		private void frmSwipeRecords_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrm_KeyDown(sender, e);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000A7128 File Offset: 0x000A6128
		private void frmSwipeRecords_Load(object sender, EventArgs e)
		{
			if (!wgAppConfig.IsActivateCameraManage)
			{
				this.bCameraView = false;
			}
			else
			{
				this.bCameraView = true;
			}
			if (wgAppConfig.getParamValBoolByNO(185))
			{
				wgMjControllerSwipeRecord.bRemoteOpenBiDirection = 1;
			}
			else if (wgMjControllerSwipeRecord.bRemoteOpenBiDirection > 0)
			{
				wgAppConfig.setSystemParamValue(185, "Activate Remote Open Direction", "1", "Remote Open Direction V6.62 2015-12-04 12:43:32");
			}
			this.bFindActive = false;
			this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
			this.saveDefaultStyle();
			this.loadStyle();
			this.loadFloorInfo();
			wgAppConfig.HideCardNOColumn(this.dgvSwipeRecords.Columns["f_CardNO"]);
			this.dtpDateFrom = new frmSwipeRecords.ToolStripDateTime();
			this.dtpDateTo = new frmSwipeRecords.ToolStripDateTime();
			this.dtpTimeFrom = new frmSwipeRecords.ToolStripDateTime();
			this.dtpTimeFrom.SetTimeFormat();
			this.dtpTimeFrom.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
			this.dtpTimeTo = new frmSwipeRecords.ToolStripDateTime();
			this.dtpTimeTo.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
			this.dtpTimeTo.SetTimeFormat();
			this.toolStrip3.Items.Clear();
			this.toolStrip3.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripLabel2, this.cboStart, this.dtpDateFrom, this.toolStripLabel3, this.cboEnd, this.dtpDateTo, this.toolStripSeparator1, this.toolStripLabel4, this.dtpTimeFrom, this.toolStripLabel5,
				this.dtpTimeTo
			});
			this.dtpDateFrom.BoxWidth = 120;
			this.dtpDateTo.BoxWidth = 120;
			this.dtpTimeFrom.BoxWidth = 62;
			this.dtpTimeTo.BoxWidth = 62;
			this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.f_DepartmentName.HeaderText);
			this.userControlFind1.btnQuery.Click += this.btnQuery_Click;
			if (this.cboStart.Items.Count > 0)
			{
				this.cboStart.SelectedIndex = 0;
			}
			this.dtpDateFrom.Enabled = false;
			if (this.cboEnd.Items.Count > 0)
			{
				this.cboEnd.SelectedIndex = 0;
			}
			this.dtpDateTo.Enabled = false;
			this.dtpDateFrom.BoxWidth = 150;
			this.dtpDateTo.BoxWidth = 150;
			wgAppConfig.setDisplayFormatDate(this.dtpDateFrom.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
			wgAppConfig.setDisplayFormatDate(this.dtpDateTo.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
			if (!wgAppConfig.getParamValBoolByNO(143))
			{
				Cursor.Current = Cursors.WaitCursor;
				this.timer1.Enabled = true;
				this.userControlFind1.btnQuery.PerformClick();
			}
			this.btnCameraView.Visible = this.bCameraView;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000A7434 File Offset: 0x000A6434
		private string getSqlOfDateTime()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getSqlOfDateTime_Acc();
			}
			string text = "";
			if (this.cboStart.SelectedIndex == 1)
			{
				text = "  ([f_ReadDate]>= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 00:00:00") + ")";
			}
			if (this.cboEnd.SelectedIndex == 1)
			{
				if (text != "")
				{
					text += " AND ";
				}
				text = text + "  ([f_ReadDate]<= " + wgTools.PrepareStr(this.dtpDateTo.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")";
			}
			if (this.dtpTimeFrom.Value.ToString("HH:mm") != "00:00")
			{
				if (text != "")
				{
					text += " AND ";
				}
				if (this.dtpTimeFrom.Value.ToString("mm") == "00")
				{
					text = text + " DATEPART(hh, [f_ReadDate]) >= " + this.dtpTimeFrom.Value.ToString("HH");
				}
				else
				{
					string text2 = text + " (  DATEPART(hh, [f_ReadDate]) > " + this.dtpTimeFrom.Value.ToString("HH");
					text = string.Concat(new string[]
					{
						text2,
						" OR (DATEPART(hh, [f_ReadDate]) = ",
						this.dtpTimeFrom.Value.ToString("HH"),
						" AND (DATEPART(mi, [f_ReadDate]) >= ",
						this.dtpTimeFrom.Value.ToString("mm"),
						")) ) "
					});
				}
			}
			if (!(this.dtpTimeTo.Value.ToString("HH:mm") != "23:59"))
			{
				return text;
			}
			if (text != "")
			{
				text += " AND ";
			}
			if (this.dtpTimeTo.Value.ToString("mm") == "59")
			{
				return text + " DATEPART(hh, [f_ReadDate]) <= " + this.dtpTimeTo.Value.ToString("HH");
			}
			string text3 = text + " (  DATEPART(hh, [f_ReadDate]) < " + this.dtpTimeTo.Value.ToString("HH");
			return string.Concat(new string[]
			{
				text3,
				" OR (DATEPART(hh, [f_ReadDate]) = ",
				this.dtpTimeTo.Value.ToString("HH"),
				" AND (DATEPART(mi, [f_ReadDate]) <= ",
				this.dtpTimeTo.Value.ToString("mm"),
				")) ) "
			});
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000A772C File Offset: 0x000A672C
		private string getSqlOfDateTime_Acc()
		{
			string text = "";
			if (this.cboStart.SelectedIndex == 1)
			{
				text = "  ([f_ReadDate]>= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 00:00:00") + ")";
			}
			if (this.cboEnd.SelectedIndex == 1)
			{
				if (text != "")
				{
					text += " AND ";
				}
				text = text + "  ([f_ReadDate]<= " + wgTools.PrepareStr(this.dtpDateTo.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")";
			}
			if (this.dtpTimeFrom.Value.ToString("HH:mm") != "00:00")
			{
				if (text != "")
				{
					text += " AND ";
				}
				if (this.dtpTimeFrom.Value.ToString("mm") == "00")
				{
					text = text + " Hour([f_ReadDate]) >= " + this.dtpTimeFrom.Value.ToString("HH");
				}
				else
				{
					string text2 = text + " (  HOUR( [f_ReadDate]) > " + this.dtpTimeFrom.Value.ToString("HH");
					text = string.Concat(new string[]
					{
						text2,
						" OR (HOUR([f_ReadDate]) = ",
						this.dtpTimeFrom.Value.ToString("HH"),
						" AND (Minute( [f_ReadDate]) >= ",
						this.dtpTimeFrom.Value.ToString("mm"),
						")) ) "
					});
				}
			}
			if (!(this.dtpTimeTo.Value.ToString("HH:mm") != "23:59"))
			{
				return text;
			}
			if (text != "")
			{
				text += " AND ";
			}
			if (this.dtpTimeTo.Value.ToString("mm") == "59")
			{
				return text + " HOUR( [f_ReadDate]) <= " + this.dtpTimeTo.Value.ToString("HH");
			}
			string text3 = text + " (  HOUR([f_ReadDate]) < " + this.dtpTimeTo.Value.ToString("HH");
			return string.Concat(new string[]
			{
				text3,
				" OR (Hour( [f_ReadDate]) = ",
				this.dtpTimeTo.Value.ToString("HH"),
				" AND (Minute( [f_ReadDate]) <= ",
				this.dtpTimeTo.Value.ToString("mm"),
				")) ) "
			});
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000A7A13 File Offset: 0x000A6A13
		private void loadAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!this.bLoadAll)
			{
				this.bLoadAll = true;
				this.btnQuery_Click(null, null);
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000A7A2C File Offset: 0x000A6A2C
		private void loadDefaultStyle()
		{
			DataTable dataTable = this.dsDefaultStyle.Tables[this.dgvSwipeRecords.Name];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				this.dgvSwipeRecords.Columns[i].Name = dataTable.Rows[i]["colName"].ToString();
				this.dgvSwipeRecords.Columns[i].HeaderText = dataTable.Rows[i]["colHeader"].ToString();
				this.dgvSwipeRecords.Columns[i].Width = int.Parse(dataTable.Rows[i]["colWidth"].ToString());
				this.dgvSwipeRecords.Columns[i].Visible = bool.Parse(dataTable.Rows[i]["colVisable"].ToString());
				this.dgvSwipeRecords.Columns[i].DisplayIndex = int.Parse(dataTable.Rows[i]["colDisplayIndex"].ToString());
			}
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000A7B78 File Offset: 0x000A6B78
		private void loadFloorInfo()
		{
			string text = "  SELECT t_b_Floor.f_floorID, t_b_Reader.f_ReaderName, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
			text += "   t_b_Door.f_DoorName,    t_b_Floor.f_floorNO, t_b_Controller.f_ZoneID, t_b_Floor.f_floorName, t_b_Door.f_ControllerID      FROM t_b_Floor , t_b_Door, t_b_Controller, t_b_Reader    where t_b_Floor.f_DoorID = t_b_Door.f_DoorID and t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID and t_b_Reader.f_ControllerID = t_b_Floor.f_ControllerID  AND  t_b_Reader.f_ReaderNO =1 ";
			DataTable dataTable = new DataTable();
			this.dvFloor = new DataView(dataTable);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(dataTable);
						}
					}
					goto IL_00C7;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(dataTable);
					}
				}
			}
			try
			{
				IL_00C7:
				dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[0] };
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000A7CD8 File Offset: 0x000A6CD8
		private void loadStyle()
		{
			this.dgvSwipeRecords.AutoGenerateColumns = false;
			wgAppConfig.ReadGVStyle(this, this.dgvSwipeRecords);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000A7CF4 File Offset: 0x000A6CF4
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
							oleDbCommand.CommandTimeout = 90;
							if (this.bLoadAll)
							{
								oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							}
							oleDbDataAdapter.Fill(this.table);
						}
					}
					goto IL_01B3;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlCommand.CommandTimeout = 90;
						if (this.bLoadAll)
						{
							sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						}
						sqlDataAdapter.Fill(this.table);
					}
				}
			}
			IL_01B3:
			if (this.table.Rows.Count > 0)
			{
				this.recIdMin = int.Parse(this.table.Rows[this.table.Rows.Count - 1][0].ToString());
			}
			wgTools.WriteLine("da.Fill End " + startIndex.ToString());
			Cursor.Current = Cursors.Default;
			return this.table;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000A7F74 File Offset: 0x000A6F74
		public void nextRecord()
		{
			int num = this.dgvSwipeRecords.CurrentCell.RowIndex + 1;
			if (this.dgvSwipeRecords.RowCount > 0)
			{
				if (this.dgvSwipeRecords.RowCount > num)
				{
					this.dgvSwipeRecords.CurrentCell = this.dgvSwipeRecords[1, num];
					return;
				}
				this.dgvSwipeRecords.CurrentCell = this.dgvSwipeRecords[1, this.dgvSwipeRecords.RowCount - 1];
			}
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x000A7FF0 File Offset: 0x000A6FF0
		public void prevRecord()
		{
			int num = this.dgvSwipeRecords.CurrentCell.RowIndex - 1;
			if (this.dgvSwipeRecords.RowCount > 0 && num >= 0)
			{
				if (this.dgvSwipeRecords.RowCount > num)
				{
					this.dgvSwipeRecords.CurrentCell = this.dgvSwipeRecords[1, num];
					return;
				}
				this.dgvSwipeRecords.CurrentCell = this.dgvSwipeRecords[1, this.dgvSwipeRecords.RowCount - 1];
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x000A8070 File Offset: 0x000A7070
		private void reloadData(string strsql)
		{
			if (!this.backgroundWorker1.IsBusy)
			{
				this.bLoadedFinished = false;
				this.startRecordIndex = 0;
				if (!string.IsNullOrEmpty(strsql))
				{
					this.dgvSql = strsql;
				}
				this.dgvSwipeRecords.DataSource = null;
				this.timer1.Enabled = true;
				if (this.bLoadAll)
				{
					this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, 100000000, this.dgvSql });
					return;
				}
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x000A813A File Offset: 0x000A713A
		private void restoreDefaultLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.RestoreGVStyle(this, this.dgvSwipeRecords);
			this.loadDefaultStyle();
			this.loadStyle();
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x000A8154 File Offset: 0x000A7154
		private void saveDefaultStyle()
		{
			DataTable dataTable = new DataTable();
			this.dsDefaultStyle.Tables.Add(dataTable);
			dataTable.TableName = this.dgvSwipeRecords.Name;
			dataTable.Columns.Add("colName");
			dataTable.Columns.Add("colHeader");
			dataTable.Columns.Add("colWidth");
			dataTable.Columns.Add("colVisable");
			dataTable.Columns.Add("colDisplayIndex");
			for (int i = 0; i < this.dgvSwipeRecords.ColumnCount; i++)
			{
				DataGridViewColumn dataGridViewColumn = this.dgvSwipeRecords.Columns[i];
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

		// Token: 0x0600060E RID: 1550 RVA: 0x000A8289 File Offset: 0x000A7289
		private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.SaveDGVStyle(this, this.dgvSwipeRecords);
			XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x000A82B4 File Offset: 0x000A72B4
		private void setOutputFormatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvSwipeRecords.DataSource != null)
			{
				DataTable dataTable = this.dgvSwipeRecords.DataSource as DataTable;
				if (dataTable.Rows.Count <= 0)
				{
					XMessageBox.Show(CommonStr.strExportTextStyleNoData);
					return;
				}
				using (dfrmExportTextStyle4SwipeRecord dfrmExportTextStyle4SwipeRecord = new dfrmExportTextStyle4SwipeRecord())
				{
					dfrmExportTextStyle4SwipeRecord.dr = dataTable.Rows[0];
					dfrmExportTextStyle4SwipeRecord.ShowDialog();
				}
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000A8334 File Offset: 0x000A7334
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

		// Token: 0x06000611 RID: 1553 RVA: 0x000A8364 File Offset: 0x000A7364
		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			string text = "";
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = sender.ToString();
				dfrmInputNewName.label1.Text = CommonStr.strCardID;
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				text = dfrmInputNewName.strNewName;
			}
			if (!string.IsNullOrEmpty(text))
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				string text2 = "";
				long num4 = 0L;
				int num5 = 0;
				string text3 = "";
				bool flag = false;
				this.userControlFind1.txtFindCardID.Text = "";
				this.userControlFind1.txtFindName.Text = "";
				if (this.dfrmFindOption != null && this.dfrmFindOption.Visible)
				{
					flag = true;
					text3 = this.dfrmFindOption.getStrSql();
				}
				this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text2, ref num4, ref num5);
				string text4 = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
				text4 += "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_ReaderName,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll         ,0 as f_CaptureFileExisted ";
				string text5 = " ( 1>0 ) ";
				if (this.getSqlOfDateTime() != "")
				{
					text5 += string.Format(" AND {0} ", this.getSqlOfDateTime());
				}
				if (flag)
				{
					text5 += string.Format(" AND {0} ", text3);
				}
				if (text.IndexOf("%") < 0)
				{
					text = string.Format("%{0}%", text);
				}
				if (wgAppConfig.IsAccessDB)
				{
					text5 += string.Format(" AND CSTR(t_d_SwipeRecord.f_CardNO) like {0} ", wgTools.PrepareStrNUnicode(text));
				}
				else
				{
					text5 += string.Format(" AND t_d_SwipeRecord.f_CardNO like {0} ", wgTools.PrepareStrNUnicode(text));
				}
				string sqlFindSwipeRecord = wgAppConfig.getSqlFindSwipeRecord(text4, "t_d_SwipeRecord", text5, num, num2, num3, text2, num4, num5);
				this.reloadData(sqlFindSwipeRecord);
			}
		}

		// Token: 0x04000B12 RID: 2834
		private bool bCameraView;

		// Token: 0x04000B13 RID: 2835
		private bool bLoad4Export;

		// Token: 0x04000B14 RID: 2836
		private bool bLoadAll;

		// Token: 0x04000B15 RID: 2837
		private bool bLoadedFinished;

		// Token: 0x04000B16 RID: 2838
		private dfrmSwipeRecordsFindOption dfrmFindOption;

		// Token: 0x04000B17 RID: 2839
		private DataTable dt4Dgv;

		// Token: 0x04000B18 RID: 2840
		private DataView dvFloor;

		// Token: 0x04000B19 RID: 2841
		private dfrmPhotoAvi photoavi;

		// Token: 0x04000B1A RID: 2842
		private int recIdMin;

		// Token: 0x04000B1B RID: 2843
		private int startRecordIndex;

		// Token: 0x04000B1C RID: 2844
		private DataTable table;

		// Token: 0x04000B1D RID: 2845
		private string avijpgPath = wgAppConfig.Path4AviJpgOnlyView();

		// Token: 0x04000B1E RID: 2846
		private string dgvSql = "";

		// Token: 0x04000B1F RID: 2847
		private DataSet dsDefaultStyle = new DataSet("DGV_STILE");

		// Token: 0x04000B20 RID: 2848
		private int MaxRecord = 1000;

		// Token: 0x04000B21 RID: 2849
		public string strFindOption = "";

		// Token: 0x04000B2F RID: 2863
		private frmSwipeRecords.ToolStripDateTime dtpDateFrom;

		// Token: 0x04000B30 RID: 2864
		private frmSwipeRecords.ToolStripDateTime dtpDateTo;

		// Token: 0x04000B31 RID: 2865
		private frmSwipeRecords.ToolStripDateTime dtpTimeFrom;

		// Token: 0x04000B32 RID: 2866
		private frmSwipeRecords.ToolStripDateTime dtpTimeTo;

		// Token: 0x02000053 RID: 83
		private enum fieldType
		{
			// Token: 0x04000B50 RID: 2896
			None,
			// Token: 0x04000B51 RID: 2897
			PadLeft,
			// Token: 0x04000B52 RID: 2898
			PadRight,
			// Token: 0x04000B53 RID: 2899
			DateTime,
			// Token: 0x04000B54 RID: 2900
			Date
		}

		// Token: 0x02000054 RID: 84
		public class ToolStripDateTime : ToolStripControlHost
		{
			// Token: 0x06000614 RID: 1556 RVA: 0x000A9600 File Offset: 0x000A8600
			public ToolStripDateTime()
				: base(frmSwipeRecords.ToolStripDateTime.dtp = new DateTimePicker())
			{
			}

			// Token: 0x06000615 RID: 1557 RVA: 0x000A9613 File Offset: 0x000A8613
			protected override void Dispose(bool disposing)
			{
				if (disposing && frmSwipeRecords.ToolStripDateTime.dtp != null)
				{
					frmSwipeRecords.ToolStripDateTime.dtp.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x06000616 RID: 1558 RVA: 0x000A9630 File Offset: 0x000A8630
			public void SetTimeFormat()
			{
				DateTimePicker dateTimePicker = base.Control as DateTimePicker;
				dateTimePicker.CustomFormat = "HH;mm";
				dateTimePicker.Format = DateTimePickerFormat.Custom;
				dateTimePicker.ShowUpDown = true;
			}

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x06000617 RID: 1559 RVA: 0x000A9664 File Offset: 0x000A8664
			// (set) Token: 0x06000618 RID: 1560 RVA: 0x000A968C File Offset: 0x000A868C
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

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x06000619 RID: 1561 RVA: 0x000A96F0 File Offset: 0x000A86F0
			public DateTimePicker DateTimeControl
			{
				get
				{
					return base.Control as DateTimePicker;
				}
			}

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x0600061A RID: 1562 RVA: 0x000A96FD File Offset: 0x000A86FD
			// (set) Token: 0x0600061B RID: 1563 RVA: 0x000A970F File Offset: 0x000A870F
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

			// Token: 0x04000B55 RID: 2901
			private static DateTimePicker dtp;
		}
	}
}
