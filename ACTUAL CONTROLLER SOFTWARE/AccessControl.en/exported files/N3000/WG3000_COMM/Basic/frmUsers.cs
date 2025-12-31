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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic.MultiThread;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.ExtendFunc;
using WG3000_COMM.ExtendFunc.DeletedUsers;
using WG3000_COMM.ExtendFunc.PrivilegeType;
using WG3000_COMM.ExtendFunc.QR2017;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000055 RID: 85
	public partial class frmUsers : Form
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x000A9724 File Offset: 0x000A8724
		public frmUsers()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvUsers);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x000A97BC File Offset: 0x000A87BC
		private bool _addConsumer4Import(string no, string name, string strCard, string dept)
		{
			long num = 0L;
			icConsumer icConsumer = new icConsumer();
			if (!string.IsNullOrEmpty(strCard))
			{
				long.TryParse(strCard, out num);
			}
			if (string.IsNullOrEmpty(dept))
			{
				return icConsumer.addNew(no, name, num, 0) >= 0;
			}
			return icConsumer.addNew(no, name, num, this.getDeptId(dept)) >= 0;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000A9814 File Offset: 0x000A8814
		private bool _addConsumer4Import(string no, string name, string strCard, string dept, string dateBegin, string dateEnd)
		{
			long num = 0L;
			DateTime now = DateTime.Now;
			DateTime dateTime = DateTime.Parse("2099-12-31 23:59:59");
			DateTime.TryParse(dateBegin, out now);
			DateTime.TryParse(dateEnd, out dateTime);
			icConsumer icConsumer = new icConsumer();
			if (!string.IsNullOrEmpty(strCard))
			{
				long.TryParse(strCard, out num);
			}
			if (string.IsNullOrEmpty(dept))
			{
				return icConsumer.addNew(no, name, 0, 1, 0, 1, now, dateTime, 345678, num) >= 0;
			}
			return icConsumer.addNew(no, name, this.getDeptId(dept), 1, 0, 1, now, dateTime, 345678, num) >= 0;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x000A98A8 File Offset: 0x000A88A8
		public int addConsumerNew(string no, string name, string strCard, string dept)
		{
			icConsumer icConsumer = new icConsumer();
			if (strCard != "")
			{
				bool flag = false;
				long num;
				if (long.TryParse(strCard, out num))
				{
					if (strCard.ToUpper().IndexOf("E") >= 0)
					{
						return -1;
					}
					long num2 = long.Parse(strCard);
					strCard = num2.ToString();
					if (num2 <= 0L)
					{
						return -1;
					}
					if (wgAppConfig.IsActivateCard19)
					{
						if (num2 == (long)((ulong)(-1)))
						{
							return -1;
						}
					}
					else if (num2 >= (long)((ulong)(-1)))
					{
						return -1;
					}
					if (icConsumer.isExisted(long.Parse(strCard)))
					{
						return -1;
					}
					flag = true;
				}
				if (!flag)
				{
					return -1;
				}
			}
			if (this._addConsumer4Import(no, name, strCard, dept))
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000A993C File Offset: 0x000A893C
		public int addConsumerNew(string no, string name, string strCard, string dept, string dateBegin, string dateEnd)
		{
			icConsumer icConsumer = new icConsumer();
			if (strCard != "")
			{
				bool flag = false;
				long num;
				if (long.TryParse(strCard, out num))
				{
					if (strCard.ToUpper().IndexOf("E") >= 0)
					{
						return -1;
					}
					long num2 = long.Parse(strCard);
					strCard = num2.ToString();
					if (num2 <= 0L)
					{
						return -1;
					}
					if (wgAppConfig.IsActivateCard19)
					{
						if (num2 == (long)((ulong)(-1)))
						{
							return -1;
						}
					}
					else if (num2 >= (long)((ulong)(-1)))
					{
						return -1;
					}
					if (icConsumer.isExisted(long.Parse(strCard)))
					{
						return -1;
					}
					flag = true;
				}
				if (!flag)
				{
					return -1;
				}
			}
			if (this._addConsumer4Import(no, name, strCard, dept, dateBegin, dateEnd))
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000A99D4 File Offset: 0x000A89D4
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			int num = (int)((object[])e.Argument)[0];
			int num2 = (int)((object[])e.Argument)[1];
			string text = (string)((object[])e.Argument)[2];
			e.Result = this.loadUserData(num, num2, text);
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000A9A54 File Offset: 0x000A8A54
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
			if ((e.Result as DataView).Count < this.MaxRecord)
			{
				this.bLoadedFinished = true;
				this.bLoadAll = false;
			}
			this.fillDgv(e.Result as DataView);
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x000A9B10 File Offset: 0x000A8B10
		private void batchUpdateSelectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvUsers.SelectedRows.Count > 0)
			{
				using (dfrmUserBatchUpdate dfrmUserBatchUpdate = new dfrmUserBatchUpdate())
				{
					string text = "";
					for (int i = 0; i < this.dgvUsers.SelectedRows.Count; i++)
					{
						int index = this.dgvUsers.SelectedRows[i].Index;
						int num = int.Parse(this.dgvUsers.Rows[index].Cells[0].Value.ToString());
						if (!string.IsNullOrEmpty(text))
						{
							text += ",";
						}
						text += num.ToString();
					}
					dfrmUserBatchUpdate.strSqlSelected = text;
					dfrmUserBatchUpdate.Text = string.Format("{0}: [{1}]", this.batchUpdateSelectToolStripMenuItem.Text, this.dgvUsers.SelectedRows.Count.ToString());
					if (dfrmUserBatchUpdate.ShowDialog(this) == DialogResult.OK)
					{
						this.reloadUserData("");
					}
				}
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000A9C2C File Offset: 0x000A8C2C
		private void btnAdd_Click(object sender, EventArgs e)
		{
			int num = 0;
			DataGridView dataGridView = this.dgvUsers;
			DataGridViewColumn sortedColumn = dataGridView.SortedColumn;
			ListSortDirection listSortDirection = ListSortDirection.Ascending;
			if (sortedColumn != null && dataGridView.SortOrder == global::System.Windows.Forms.SortOrder.Descending)
			{
				listSortDirection = ListSortDirection.Descending;
			}
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
			using (dfrmUser dfrmUser = new dfrmUser())
			{
				if (dfrmUser.ShowDialog(this) == DialogResult.OK)
				{
					this.reloadUserData("");
					if (dataGridView.RowCount > 0)
					{
						if (dataGridView.RowCount > num)
						{
							dataGridView.CurrentCell = dataGridView[1, num];
						}
						else
						{
							dataGridView.CurrentCell = dataGridView[1, dataGridView.RowCount - 1];
						}
					}
					if (sortedColumn != null)
					{
						dataGridView.Sort(sortedColumn, listSortDirection);
					}
				}
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000A9D04 File Offset: 0x000A8D04
		private void btnAutoAdd_Click(object sender, EventArgs e)
		{
			using (dfrmUserAutoAdd dfrmUserAutoAdd = new dfrmUserAutoAdd())
			{
				dfrmUserAutoAdd.watching = this.watching;
				dfrmUserAutoAdd.frmCall = this;
				if (dfrmUserAutoAdd.ShowDialog(this) == DialogResult.OK)
				{
					this.reloadUserData("");
				}
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000A9D5C File Offset: 0x000A8D5C
		private void btnBatchUpdate_Click(object sender, EventArgs e)
		{
			using (dfrmUserBatchUpdate dfrmUserBatchUpdate = new dfrmUserBatchUpdate())
			{
				if (dfrmUserBatchUpdate.ShowDialog(this) == DialogResult.OK)
				{
					this.reloadUserData("");
				}
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000A9DA0 File Offset: 0x000A8DA0
		private void btnDelete_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvUsers.SelectedRows.Count <= 0)
				{
					if (this.dgvUsers.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvUsers.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvUsers.SelectedRows[0].Index;
				}
				int num2 = int.Parse(this.dgvUsers.Rows[num].Cells[0].Value.ToString());
				if (num2 > 0)
				{
					string text = "";
					string text2;
					if (this.dgvUsers.SelectedRows.Count == 1)
					{
						text2 = string.Format("{0}\r\n\r\n{1}:  {2}", this.btnDelete.Text, this.dgvUsers.Columns[2].HeaderText, this.dgvUsers.Rows[num].Cells[2].Value.ToString());
					}
					else
					{
						text2 = string.Format("{0}\r\n\r\n{1}=  {2}", this.btnDelete.Text, CommonStr.strUsersNum, this.dgvUsers.SelectedRows.Count);
					}
					if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text2), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						icConsumer icConsumer = new icConsumer();
						if (this.dgvUsers.SelectedRows.Count == 1)
						{
							text2 = string.Format("{0} {1}:  {2}", this.btnDelete.Text, this.dgvUsers.Columns[2].HeaderText, this.dgvUsers.Rows[num].Cells[2].Value.ToString());
							text = string.Format("{0}-{1}-{2}-{3}:  {4}", new object[]
							{
								this.btnDelete.Text,
								this.dgvUsers.Rows[num].Cells[0].Value.ToString(),
								this.dgvUsers.Rows[num].Cells[1].Value.ToString(),
								this.dgvUsers.Columns[2].HeaderText,
								this.dgvUsers.Rows[num].Cells[2].Value.ToString()
							});
						}
						else
						{
							text = string.Format("{0} {1}=  {2}", this.btnDelete.Text, CommonStr.strUsersNum, this.dgvUsers.SelectedRows.Count) + string.Format("From {0}...", this.dgvUsers.Rows[num].Cells[2].Value.ToString());
						}
						int count = this.dgvUsers.SelectedRows.Count;
						if (this.dgvUsers.SelectedRows.Count == 1)
						{
							icConsumer.dimissionUser(num2);
						}
						else
						{
							for (int i = 0; i < this.dgvUsers.SelectedRows.Count; i++)
							{
								num = this.dgvUsers.SelectedRows[i].Index;
								num2 = int.Parse(this.dgvUsers.Rows[num].Cells[0].Value.ToString());
								icConsumer.dimissionUser(num2);
							}
						}
						foreach (object obj in this.dgvUsers.SelectedRows)
						{
							DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
							this.dgvUsers.Rows.Remove(dataGridViewRow);
						}
						this.deletedUserCnt += count;
						wgAppConfig.wgLog(text, EventLogEntryType.Information, null);
						if (this.bLoadedFinished)
						{
							wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + "#");
						}
					}
					icConsumerShare.setUpdateLog();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000AA214 File Offset: 0x000A9214
		private void btnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvUsers.SelectedRows.Count <= 0)
				{
					if (this.dgvUsers.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvUsers.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvUsers.SelectedRows[0].Index;
				}
				using (dfrmUser dfrmUser = new dfrmUser())
				{
					dfrmUser.consumerID = int.Parse(this.dgvUsers.Rows[num].Cells[0].Value.ToString());
					dfrmUser.OperateNew = false;
					if (dfrmUser.ShowDialog(this) == DialogResult.OK)
					{
						this.reloadSingleUser(num);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000AA304 File Offset: 0x000A9304
		private void btnEditPrivilege_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvUsers.SelectedRows.Count <= 0)
				{
					if (this.dgvUsers.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvUsers.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvUsers.SelectedRows[0].Index;
				}
				using (dfrmPrivilegeSingle dfrmPrivilegeSingle = new dfrmPrivilegeSingle())
				{
					dfrmPrivilegeSingle.watching = this.watching;
					dfrmPrivilegeSingle.frmCall = this;
					dfrmPrivilegeSingle.consumerID = int.Parse(this.dgvUsers.Rows[num].Cells[0].Value.ToString());
					dfrmPrivilegeSingle.Text = string.Concat(new string[]
					{
						this.dgvUsers.Rows[num].Cells[1].Value.ToString().Trim(),
						".",
						this.dgvUsers.Rows[num].Cells[2].Value.ToString().Trim(),
						" -- ",
						dfrmPrivilegeSingle.Text
					});
					if (dfrmPrivilegeSingle.ShowDialog(this) == DialogResult.OK)
					{
						this.reloadSingleUser(num);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000AA4A0 File Offset: 0x000A94A0
		private void btnExport_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcelSpecial(ref this.dgvUsers, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000AA4D2 File Offset: 0x000A94D2
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvUsers);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000AA4E8 File Offset: 0x000A94E8
		private void btnFindBySFZID_Click(object sender, EventArgs e)
		{
			try
			{
				using (dfrmUser dfrmUser = new dfrmUser())
				{
					if (dfrmUser.getSFZID() > 0)
					{
						dfrmUser.OperateNew = false;
						if (dfrmUser.ShowDialog(this) == DialogResult.OK)
						{
							this.reloadUserData("");
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000AA558 File Offset: 0x000A9558
		private void btnImportFromExcel_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(198))
			{
				this.importFromExcelWithDateToolStripMenuItem_Click(null, null);
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				XMessageBox.Show(wgAppConfig.ReplaceWorkNO(wgAppConfig.ReplaceFloorRoom(CommonStr.strImportInformation)));
				this.openFileDialog1.Filter = " (*.xls)|*.xls| (*.*)|*.*";
				this.openFileDialog1.FilterIndex = 1;
				this.openFileDialog1.RestoreDirectory = true;
				try
				{
					this.openFileDialog1.InitialDirectory = ".\\REPORT";
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				this.openFileDialog1.Title = this.btnImportFromExcel.Text;
				this.openFileDialog1.FileName = "";
				if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					string fileName = this.openFileDialog1.FileName;
					this.dfrmWait1.Show();
					this.dfrmWait1.Refresh();
					wgTools.WriteLine("start");
					int num = 0;
					int num2 = 1;
					int num3 = 2;
					int num4 = 3;
					int num5 = -1;
					int num6 = -1;
					if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_EXCELMODE")))
					{
						int.TryParse(wgAppConfig.GetKeyVal("KEY_EXCELMODE"), out num6);
					}
					bool flag = false;
					bool flag2 = false;
					int i = 0;
					while (i < 2)
					{
						try
						{
							this.MyConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; data source= " + fileName + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE'");
							string text = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE\"";
							if (fileName != string.Empty)
							{
								FileInfo fileInfo = new FileInfo(fileName);
								if (fileInfo.Extension.Equals(".xls"))
								{
									this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
									switch (i)
									{
									case 0:
										this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
										break;
									case 1:
										this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "ACE", "12.0", fileName, "8.0" }));
										break;
									}
								}
								else if (fileInfo.Extension.Equals(".xlsx"))
								{
									this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Ace", "12.0", fileName, "12.0" }));
									i = 1;
								}
								else
								{
									this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
									i = 1;
								}
							}
							try
							{
								this.MyConnection.Open();
								flag2 = false;
								flag = true;
							}
							catch (OleDbException)
							{
								flag2 = true;
								throw;
							}
							catch
							{
								throw;
							}
						}
						catch
						{
							if (i == 1)
							{
								if (flag2)
								{
									XMessageBox.Show(CommonStr.strSaveAsXLSFile);
								}
								throw;
							}
						}
						i++;
						if (flag)
						{
							break;
						}
					}
					this.DS = new DataSet();
					DataTable dataTable = null;
					if (flag)
					{
						dataTable = this.MyConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
						this.MyConnection.Close();
					}
					string text2 = "";
					if (dataTable == null || dataTable.Rows.Count <= 0)
					{
						XMessageBox.Show(this.btnImportFromExcel.Text + ": " + 0);
					}
					else
					{
						text2 = wgTools.SetObjToStr(dataTable.Rows[0][2]);
						for (int j = 0; j <= dataTable.Rows.Count - 1; j++)
						{
							if (wgTools.SetObjToStr(dataTable.Rows[j][2]) == "用户" || wgTools.SetObjToStr(dataTable.Rows[j][2]) == "用戶" || wgTools.SetObjToStr(dataTable.Rows[j][2]) == "Users")
							{
								text2 = wgTools.SetObjToStr(dataTable.Rows[j][2]);
								break;
							}
						}
						num = -1;
						num2 = -1;
						num3 = -1;
						num4 = -1;
						num5 = -1;
						if (text2.IndexOf("$") <= 0)
						{
							text2 += "$";
						}
						try
						{
							this.MyCommand = new OleDbDataAdapter("select * from [" + text2 + "A1:Z1]", this.MyConnection);
							this.MyCommand.Fill(this.DS, "userInfoTitle");
							string columnName = this.DS.Tables["userInfoTitle"].Columns[0].ColumnName;
							for (int k = 0; k <= this.DS.Tables["userInfoTitle"].Columns.Count - 1; k++)
							{
								object columnName2 = this.DS.Tables["userInfoTitle"].Columns[k].ColumnName;
								if (wgTools.SetObjToStr(columnName2) != "")
								{
									string text3;
									if (wgTools.SetObjToStr(columnName2).ToUpper() == "User ID".ToUpper())
									{
										num = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "User Name".ToUpper())
									{
										num2 = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "Card NO".ToUpper())
									{
										num3 = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "Department".ToUpper() || wgTools.SetObjToStr(columnName2).ToUpper() == CommonStr.strReplaceFloorRoom.ToUpper())
									{
										num4 = k;
									}
									else
										switch (text3 = wgTools.SetObjToStr(columnName2).ToUpper().Substring(0, 2))
										{
										case "NO":
										case "用户":
										case "用戶":
										case "编号":
										case "編號":
										case "WO":
										case "工号":
										case "工號":
											num = k;
											break;
										case "NA":
										case "姓名":
											num2 = k;
											break;
										case "CA":
										case "卡号":
										case "卡號":
											num3 = k;
											break;
										case "DE":
										case "部门":
										case "部門":
											num4 = k;
											break;
										}
								}
							}
						}
						catch (Exception ex2)
						{
							wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
							wgAppConfig.wgLog(ex2.ToString());
						}
						if (num2 < 0)
						{
							XMessageBox.Show(CommonStr.strWrongUsersFile);
						}
						else
						{
							string text4 = "";
							int num8 = 0;
							try
							{
								int num9 = Math.Max(Math.Max(Math.Max(num, num2), num3), num4);
								string text5 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
								if (num9 < text5.Length)
								{
									this.MyCommand = new OleDbDataAdapter(string.Concat(new string[]
									{
										"select * from [",
										text2,
										"A1:",
										text5.Substring(num9, 1),
										"65535]"
									}), this.MyConnection);
									this.MyCommand.Fill(this.DS, "userInfo");
								}
							}
							catch (Exception ex3)
							{
								wgTools.WgDebugWrite(ex3.ToString(), new object[0]);
								wgAppConfig.wgLog(ex3.ToString());
							}
							this.dv = new DataView(this.DS.Tables["userInfo"]);
							int num10 = 0;
							long num11 = new icConsumer().ConsumerNONext();
							long num12 = num11;
							if (num11 < 0L)
							{
								num12 = 1L;
							}
							string text6 = "";
							new icConsumer();
							for (int l = 0; l <= this.dv.Count - 1; l++)
							{
								int num13;
								if (wgTools.SetObjToStr(this.dv[l][num2]).Trim() != "" && !(text6 == "") && int.TryParse(text6, out num13))
								{
									num12 = Math.Max(num12, (long)(int.Parse(text6) + 1));
								}
							}
							for (int m = 0; m <= this.dv.Count - 1; m++)
							{
								string text7 = "";
								text6 = "";
								string text8 = "";
								string text9 = "";
								string text10 = wgTools.SetObjToStr(this.dv[m][num2]).Trim();
								if (num >= 0)
								{
									text6 = wgTools.SetObjToStr(this.dv[m][num]).Trim();
								}
								if (num5 >= 0)
								{
									text8 = wgTools.SetObjToStr(this.dv[m][num5]).Trim();
								}
								if (num3 >= 0)
								{
									text7 = wgTools.SetObjToStr(this.dv[m][num3]).Trim();
								}
								if (num4 >= 0)
								{
									text9 = wgTools.SetObjToStr(this.dv[m][num4]).Trim();
								}
								if (text10 != "")
								{
									int num14;
									if (text6 == "")
									{
										text6 = num12.ToString();
									}
									else if (int.TryParse(text6, out num14))
									{
										num12 = Math.Max(num12, (long)int.Parse(text6));
									}
									if (this.addConsumerNew(text6, text10, text7, text9) > 0)
									{
										num10++;
										num12 += 1L;
									}
									else
									{
										text4 = text4 + text10 + ",";
										num8++;
									}
								}
								else
								{
									if (text6 != "" || text10 != "" || text7 != "" || text9 != "" || text8 != "")
									{
										text4 = string.Concat(new object[]
										{
											text4,
											"L",
											m + 1,
											","
										});
										num8++;
									}
									if (text4.Length > 500)
									{
										break;
									}
								}
								if (m >= 65535)
								{
									break;
								}
								wgAppRunInfo.raiseAppRunInfoLoadNums((num8 + num10).ToString() + " / " + this.dv.Count.ToString());
								this.dfrmWait1.Text = (num8 + num10).ToString() + " / " + this.dv.Count.ToString();
								Application.DoEvents();
							}
							wgAppRunInfo.raiseAppRunInfoLoadNums((num8 + num10).ToString() + " / " + this.dv.Count.ToString());
							this.dfrmWait1.Text = (num8 + num10).ToString() + " / " + this.dv.Count.ToString();
							new icGroup().updateGroupNO();
							wgTools.WriteLine("Import end");
							if (!(text4 == ""))
							{
								this.dfrmWait1.Hide();
								wgTools.WgDebugWrite(string.Concat(new string[]
								{
									CommonStr.strImportUsersWrong,
									"\r\n",
									CommonStr.strNotImportedUsers,
									num8.ToString(),
									"\r\n",
									text4
								}), new object[0]);
								XMessageBox.Show(string.Concat(new string[]
								{
									CommonStr.strImportUsersWrong,
									"\r\n\r\n",
									CommonStr.strNotImportedUsers,
									num8.ToString(),
									"\r\n\r\n",
									text4
								}));
								wgAppConfig.wgLog(string.Concat(new string[]
								{
									CommonStr.strImportUsersWrong,
									"\r\n",
									CommonStr.strNotImportedUsers,
									num8.ToString(),
									"\r\n",
									text4
								}));
							}
							this.dfrmWait1.Hide();
							wgAppConfig.wgLog(this.btnImportFromExcel.Text + ": " + num10);
							icConsumerShare.setUpdateLog();
							XMessageBox.Show(this.btnImportFromExcel.Text + ": " + num10);
							this.reloadUserData("");
						}
					}
				}
			}
			catch (Exception ex4)
			{
				wgTools.WgDebugWrite(ex4.ToString(), new object[0]);
				wgAppConfig.wgLog(ex4.ToString());
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
				Cursor.Current = Cursors.Default;
				this.dfrmWait1.Hide();
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000AB410 File Offset: 0x000AA410
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvUsers, this.Text);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000AB424 File Offset: 0x000AA424
		private void btnQuery_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(178) || wgAppConfig.getParamValBoolByNO(168) || wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AllowUploadUserMobile")) == "1" || wgAppConfig.getParamValBoolByNO(197) || wgAppConfig.getParamValBoolByNO(200))
			{
				this.btnQuery_Click_WithOtherInfo(sender, e);
				return;
			}
			this.deletedUserCnt = 0;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text2 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ";
			if (num5 > 0)
			{
				text2 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  WHERE  t_b_Consumer.f_ConsumerID = " + num5.ToString();
			}
			else if (num > 0)
			{
				text2 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType;
				if (num >= num3)
				{
					text2 = text2 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", num2);
				}
				else
				{
					text2 = text2 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", num3);
				}
				if (text != "")
				{
					text2 += string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text)));
				}
				else if (num4 > 0L)
				{
					text2 += string.Format(" AND f_CardNO ={0:d} ", num4);
				}
			}
			else if (text != "")
			{
				text2 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text)));
			}
			else if (num4 > 0L)
			{
				text2 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID " + string.Format(" WHERE f_CardNO ={0:d} ", num4);
			}
			this.reloadUserData(text2);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000AB630 File Offset: 0x000AA630
		private void btnQuery_Click_WithOtherInfo(object sender, EventArgs e)
		{
			this.deletedUserCnt = 0;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text2 = " SELECT     t_b_Consumer.f_ConsumerID , f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName  " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID ";
			if (num5 > 0)
			{
				text2 = " SELECT     t_b_Consumer.f_ConsumerID , f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID  WHERE  t_b_Consumer.f_ConsumerID = " + num5.ToString();
			}
			else if (num > 0)
			{
				text2 = " SELECT     t_b_Consumer.f_ConsumerID , f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone ";
				if (num >= num3)
				{
					text2 = text2 + " FROM t_b_Consumer,t_b_Group,t_b_Consumer_Other  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID AND t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", num2);
				}
				else
				{
					text2 = text2 + " FROM t_b_Consumer,t_b_Group,t_b_Consumer_Other  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  AND t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID  " + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", num3);
				}
				if (text != "")
				{
					text2 += string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text)));
				}
				else if (num4 > 0L)
				{
					text2 += string.Format(" AND f_CardNO ={0:d} ", num4);
				}
			}
			else if (text != "")
			{
				text2 = " SELECT     t_b_Consumer.f_ConsumerID , f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text)));
			}
			else if (num4 > 0L)
			{
				text2 = " SELECT     t_b_Consumer.f_ConsumerID , f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID " + string.Format(" WHERE f_CardNO ={0:d} ", num4);
			}
			this.reloadUserData(text2);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x000AB7F0 File Offset: 0x000AA7F0
		private void btnRegisterLostCard_Click(object sender, EventArgs e)
		{
			if (wgTools.bUDPCloud > 0)
			{
				this.startWatch();
			}
			bool flag = false;
			if (!(wgAppConfig.getSystemParamByNO(205) == "0.0.0.0") && !string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(205)) && icOperator.OperatorID != 1)
			{
				flag = true;
			}
			if (!flag && !wgTools.gWGYTJ && wgAppConfig.IsAcceleratorActive)
			{
				this.btnRegisterLostCardMultithread_Click(sender, e);
				return;
			}
			try
			{
				int num;
				if (this.dgvUsers.SelectedRows.Count <= 0)
				{
					if (this.dgvUsers.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvUsers.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvUsers.SelectedRows[0].Index;
				}
				int num2 = int.Parse(this.dgvUsers.Rows[num].Cells[0].Value.ToString());
				if (num2 > 0)
				{
					using (dfrmUsersCardLost dfrmUsersCardLost = new dfrmUsersCardLost())
					{
						dfrmUsersCardLost.txtf_ConsumerName.Text = this.dgvUsers.Rows[num].Cells[2].Value.ToString();
						dfrmUsersCardLost.txtf_CardNO.Text = this.dgvUsers.Rows[num].Cells[3].Value.ToString();
						string text = this.dgvUsers.Rows[num].Cells[3].Value.ToString();
						string text2 = "";
						if (dfrmUsersCardLost.ShowDialog(this) == DialogResult.OK)
						{
							Cursor.Current = Cursors.WaitCursor;
							if (this.deleteCardFromCloudServer(this.dgvUsers.Rows[num].Cells[3].Value.ToString()) <= 0)
							{
								XMessageBox.Show(CommonStr.strCloudServerNotConnected, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							}
							else
							{
								this.dfrmWait1.Show();
								this.dfrmWait1.Refresh();
								icConsumer icConsumer = new icConsumer();
								text2 = dfrmUsersCardLost.txtf_CardNONew.Text;
								if (string.IsNullOrEmpty(dfrmUsersCardLost.txtf_CardNONew.Text))
								{
									icConsumer.registerLostCard(num2, 0L);
								}
								else
								{
									icConsumer.registerLostCard(num2, long.Parse(dfrmUsersCardLost.txtf_CardNONew.Text.Trim()));
								}
								icConsumerShare.setUpdateLog();
								wgAppConfig.wgLog(string.Format("{0}:{1} [{2} => {3}]", new object[]
								{
									sender.ToString(),
									this.dgvUsers.Rows[num].Cells[2].Value.ToString(),
									this.dgvUsers.Rows[num].Cells[3].Value.ToString(),
									dfrmUsersCardLost.txtf_CardNONew.Text
								}), EventLogEntryType.Information, null);
								DataGridViewRow dataGridViewRow = this.dgvUsers.Rows[num];
								string text3 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  WHERE f_ConsumerID= " + num2.ToString();
								if (wgAppConfig.getParamValBoolByNO(178) || wgAppConfig.getParamValBoolByNO(168) || wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AllowUploadUserMobile")) == "1" || wgAppConfig.getParamValBoolByNO(197) || wgAppConfig.getParamValBoolByNO(200))
								{
									text3 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName  " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID  WHERE t_b_Consumer.f_ConsumerID= " + num2.ToString();
								}
								if (wgAppConfig.IsAccessDB)
								{
									using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
									{
										using (OleDbCommand oleDbCommand = new OleDbCommand(text3, oleDbConnection))
										{
											oleDbConnection.Open();
											OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
											bool flag2 = false;
											if (oleDbDataReader.Read())
											{
												int num3 = 1;
												while (num3 < this.dgvUsers.Columns.Count && num3 < oleDbDataReader.FieldCount)
												{
													dataGridViewRow.Cells[num3].Value = oleDbDataReader[num3];
													num3++;
												}
												if (this.arrPrivilegeTypeID.Count > 0 && (int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value > 0)
												{
													int num4 = this.arrPrivilegeTypeID.IndexOf((int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value);
													if (num4 >= 0)
													{
														dataGridViewRow.Cells["f_PrivilegeTypeName"].Value = this.arrPrivilegeTypeName[num4];
													}
												}
												if (int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_DoorEnabled"])) > 0)
												{
													flag2 = true;
												}
											}
											oleDbDataReader.Close();
											if (flag2)
											{
												text3 = " SELECT a.* ";
												text3 += " FROM t_b_Controller a, t_d_Privilege b";
												text3 = text3 + " WHERE b.f_ConsumerID= " + num2.ToString();
												text3 += " AND  a.f_ControllerID = b.f_ControllerID  ";
												using (icPrivilege icPrivilege = new icPrivilege())
												{
													try
													{
														using (OleDbCommand oleDbCommand2 = new OleDbCommand(text3, oleDbConnection))
														{
															oleDbDataReader = oleDbCommand2.ExecuteReader();
															ArrayList arrayList = new ArrayList();
															while (oleDbDataReader.Read())
															{
																if (arrayList.IndexOf((int)oleDbDataReader["f_ControllerID"]) < 0)
																{
																	arrayList.Add((int)oleDbDataReader["f_ControllerID"]);
																	if (!wgMjController.IsElevator((int)oleDbDataReader["f_ControllerSN"]))
																	{
																		if (!string.IsNullOrEmpty(text) && icPrivilege.DelPrivilegeOfOneCardIP((int)oleDbDataReader["f_ControllerSN"], wgTools.SetObjToStr(oleDbDataReader["f_IP"]), (int)oleDbDataReader["f_PORT"], long.Parse(text)) < 0)
																		{
																			this.dfrmWait1.Hide();
																			wgAppConfig.wgLog(CommonStr.strDelAddAndUploadFail.Replace("\r\n", ""), EventLogEntryType.Information, null);
																			XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
																			break;
																		}
																		if (!string.IsNullOrEmpty(text2))
																		{
																			string text4 = "";
																			using (icController icController = new icController())
																			{
																				icController.GetInfoFromDBByControllerID((int)oleDbDataReader["f_ControllerID"]);
																				if (icController.GetControllerRunInformationIP(-1) <= 0)
																				{
																					this.dfrmWait1.Hide();
																					wgAppConfig.wgLog(CommonStr.strDelAddAndUploadFail.Replace("\r\n", ""), EventLogEntryType.Information, null);
																					XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
																					return;
																				}
																				text4 = icController.runinfo.driverVersion;
																				if (icController.runinfo.registerCardNum == 0U && icPrivilege.ClearAllPrivilegeIP(icController.ControllerSN, icController.IP, icController.PORT) < 0)
																				{
																					Cursor.Current = Cursors.Default;
																					this.dfrmWait1.Hide();
																					wgAppConfig.wgLog(CommonStr.strDelAddAndUploadFail.Replace("\r\n", ""), EventLogEntryType.Information, null);
																					XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
																					return;
																				}
																			}
																			int num5;
																			if (wgTools.doubleParse(text4.Substring(1)) >= 5.52 && (wgTools.gWGYTJ || icPrivilege.bAllowUploadUserName))
																			{
																				num5 = icPrivilege.AddPrivilegeWithUsernameOfOneCardByDB((int)oleDbDataReader["f_ControllerID"], num2);
																			}
																			else
																			{
																				num5 = icPrivilege.AddPrivilegeOfOneCardByDB((int)oleDbDataReader["f_ControllerID"], num2);
																			}
																			if (num5 < 0)
																			{
																				this.dfrmWait1.Hide();
																				wgAppConfig.wgLog(CommonStr.strDelAddAndUploadFail.Replace("\r\n", ""), EventLogEntryType.Information, null);
																				XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
																				break;
																			}
																		}
																	}
																}
															}
															oleDbDataReader.Close();
														}
													}
													catch (Exception ex)
													{
														wgTools.WgDebugWrite(ex.ToString(), new object[0]);
													}
												}
											}
										}
										return;
									}
								}
								using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
								{
									using (SqlCommand sqlCommand = new SqlCommand(text3, sqlConnection))
									{
										sqlConnection.Open();
										SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
										bool flag3 = false;
										if (sqlDataReader.Read())
										{
											int num6 = 1;
											while (num6 < this.dgvUsers.Columns.Count && num6 < sqlDataReader.FieldCount)
											{
												dataGridViewRow.Cells[num6].Value = sqlDataReader[num6];
												num6++;
											}
											if (this.arrPrivilegeTypeID.Count > 0 && (int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value > 0)
											{
												int num7 = this.arrPrivilegeTypeID.IndexOf((int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value);
												if (num7 >= 0)
												{
													dataGridViewRow.Cells["f_PrivilegeTypeName"].Value = this.arrPrivilegeTypeName[num7];
												}
											}
											if (int.Parse(wgTools.SetObjToStr(sqlDataReader["f_DoorEnabled"])) > 0)
											{
												flag3 = true;
											}
										}
										sqlDataReader.Close();
										if (flag3)
										{
											text3 = " SELECT a.* ";
											text3 = text3 + " FROM t_b_Controller a, t_d_Privilege b WHERE b.f_ConsumerID= " + num2.ToString() + " AND  a.f_ControllerID = b.f_ControllerID  ";
											using (icPrivilege icPrivilege2 = new icPrivilege())
											{
												try
												{
													using (SqlCommand sqlCommand2 = new SqlCommand(text3, sqlConnection))
													{
														sqlDataReader = sqlCommand2.ExecuteReader();
														ArrayList arrayList2 = new ArrayList();
														while (sqlDataReader.Read())
														{
															if (arrayList2.IndexOf((int)sqlDataReader["f_ControllerID"]) < 0)
															{
																arrayList2.Add((int)sqlDataReader["f_ControllerID"]);
																if (!wgMjController.IsElevator((int)sqlDataReader["f_ControllerSN"]))
																{
																	if (!string.IsNullOrEmpty(text) && icPrivilege2.DelPrivilegeOfOneCardIP((int)sqlDataReader["f_ControllerSN"], wgTools.SetObjToStr(sqlDataReader["f_IP"]), (int)sqlDataReader["f_PORT"], long.Parse(text)) < 0)
																	{
																		this.dfrmWait1.Hide();
																		wgAppConfig.wgLog(CommonStr.strDelAddAndUploadFail.Replace("\r\n", ""), EventLogEntryType.Information, null);
																		XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
																		break;
																	}
																	if (!string.IsNullOrEmpty(text2))
																	{
																		string text5 = "";
																		using (icController icController2 = new icController())
																		{
																			icController2.GetInfoFromDBByControllerID((int)sqlDataReader["f_ControllerID"]);
																			if (icController2.GetControllerRunInformationIP(-1) <= 0)
																			{
																				this.dfrmWait1.Hide();
																				wgAppConfig.wgLog(CommonStr.strDelAddAndUploadFail.Replace("\r\n", ""), EventLogEntryType.Information, null);
																				XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
																				return;
																			}
																			text5 = icController2.runinfo.driverVersion;
																			if (icController2.runinfo.registerCardNum == 0U && icPrivilege2.ClearAllPrivilegeIP(icController2.ControllerSN, icController2.IP, icController2.PORT) < 0)
																			{
																				this.dfrmWait1.Hide();
																				wgAppConfig.wgLog(CommonStr.strDelAddAndUploadFail.Replace("\r\n", ""), EventLogEntryType.Information, null);
																				XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
																				return;
																			}
																		}
																		int num8;
																		if (wgTools.doubleParse(text5.Substring(1)) >= 5.52 && (wgTools.gWGYTJ || icPrivilege2.bAllowUploadUserName))
																		{
																			num8 = icPrivilege2.AddPrivilegeWithUsernameOfOneCardByDB((int)sqlDataReader["f_ControllerID"], num2);
																		}
																		else
																		{
																			num8 = icPrivilege2.AddPrivilegeOfOneCardByDB((int)sqlDataReader["f_ControllerID"], num2);
																		}
																		if (num8 < 0)
																		{
																			this.dfrmWait1.Hide();
																			wgAppConfig.wgLog(CommonStr.strDelAddAndUploadFail.Replace("\r\n", ""), EventLogEntryType.Information, null);
																			XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
																			break;
																		}
																	}
																}
															}
														}
														sqlDataReader.Close();
													}
												}
												catch (Exception ex2)
												{
													wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex3)
			{
				wgAppConfig.wgLog(ex3.ToString());
			}
			finally
			{
				Cursor.Current = Cursors.Default;
				this.dfrmWait1.Hide();
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x000AC60C File Offset: 0x000AB60C
		private void btnRegisterLostCardMultithread_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvUsers.SelectedRows.Count <= 0)
				{
					if (this.dgvUsers.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvUsers.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvUsers.SelectedRows[0].Index;
				}
				int num2 = int.Parse(this.dgvUsers.Rows[num].Cells[0].Value.ToString());
				bool flag = false;
				if (num2 > 0)
				{
					using (dfrmUsersCardLost dfrmUsersCardLost = new dfrmUsersCardLost())
					{
						dfrmUsersCardLost.txtf_ConsumerName.Text = this.dgvUsers.Rows[num].Cells[2].Value.ToString();
						dfrmUsersCardLost.txtf_CardNO.Text = this.dgvUsers.Rows[num].Cells[3].Value.ToString();
						string text = this.dgvUsers.Rows[num].Cells[3].Value.ToString();
						if (dfrmUsersCardLost.ShowDialog(this) == DialogResult.OK)
						{
							Cursor.Current = Cursors.WaitCursor;
							this.dfrmWait1.Show();
							this.dfrmWait1.Refresh();
							icConsumer icConsumer = new icConsumer();
							string text2 = dfrmUsersCardLost.txtf_CardNONew.Text;
							if (string.IsNullOrEmpty(dfrmUsersCardLost.txtf_CardNONew.Text))
							{
								icConsumer.registerLostCard(num2, 0L);
							}
							else
							{
								icConsumer.registerLostCard(num2, long.Parse(dfrmUsersCardLost.txtf_CardNONew.Text.Trim()));
							}
							icConsumerShare.setUpdateLog();
							wgAppConfig.wgLog(string.Format("{0}:{1} [{2} => {3}]", new object[]
							{
								sender.ToString(),
								this.dgvUsers.Rows[num].Cells[2].Value.ToString(),
								this.dgvUsers.Rows[num].Cells[3].Value.ToString(),
								dfrmUsersCardLost.txtf_CardNONew.Text
							}), EventLogEntryType.Information, null);
							DataGridViewRow dataGridViewRow = this.dgvUsers.Rows[num];
							string text3 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  WHERE f_ConsumerID= " + num2.ToString();
							if (wgAppConfig.getParamValBoolByNO(178) || wgAppConfig.getParamValBoolByNO(168) || wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AllowUploadUserMobile")) == "1" || wgAppConfig.getParamValBoolByNO(197) || wgAppConfig.getParamValBoolByNO(200))
							{
								text3 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName  " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID  WHERE t_b_Consumer.f_ConsumerID= " + num2.ToString();
							}
							if (wgAppConfig.IsAccessDB)
							{
								using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
								{
									using (OleDbCommand oleDbCommand = new OleDbCommand(text3, oleDbConnection))
									{
										oleDbConnection.Open();
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										bool flag2 = false;
										if (oleDbDataReader.Read())
										{
											int num3 = 1;
											while (num3 < this.dgvUsers.Columns.Count && num3 < oleDbDataReader.FieldCount)
											{
												dataGridViewRow.Cells[num3].Value = oleDbDataReader[num3];
												num3++;
											}
											if (this.arrPrivilegeTypeID.Count > 0 && (int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value > 0)
											{
												int num4 = this.arrPrivilegeTypeID.IndexOf((int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value);
												if (num4 >= 0)
												{
													dataGridViewRow.Cells["f_PrivilegeTypeName"].Value = this.arrPrivilegeTypeName[num4];
												}
											}
											if (int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_DoorEnabled"])) > 0)
											{
												flag2 = true;
											}
										}
										oleDbDataReader.Close();
										if (flag2)
										{
											dfrmMultiThreadOperation dfrmMultiThreadOperation = new dfrmMultiThreadOperation();
											dfrmMultiThreadOperation.newCardNO4Cardlost = text2;
											dfrmMultiThreadOperation.oldCardNO4Cardlost = text;
											dfrmMultiThreadOperation.ConsumerID4Cardlost = num2;
											dfrmMultiThreadOperation.startDownloadCardLostUser();
											DateTime.Now.AddSeconds(20.0);
											while (!dfrmMultiThreadOperation.bComplete4Cardlost)
											{
												Application.DoEvents();
												Thread.Sleep(100);
											}
											if (dfrmMultiThreadOperation.bCompleteFullOK4Cardlost)
											{
												flag = true;
											}
											dfrmMultiThreadOperation.Close();
										}
									}
									goto IL_062D;
								}
							}
							using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
							{
								using (SqlCommand sqlCommand = new SqlCommand(text3, sqlConnection))
								{
									sqlConnection.Open();
									SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
									bool flag3 = false;
									if (sqlDataReader.Read())
									{
										int num5 = 1;
										while (num5 < this.dgvUsers.Columns.Count && num5 < sqlDataReader.FieldCount)
										{
											dataGridViewRow.Cells[num5].Value = sqlDataReader[num5];
											num5++;
										}
										if (this.arrPrivilegeTypeID.Count > 0 && (int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value > 0)
										{
											int num6 = this.arrPrivilegeTypeID.IndexOf((int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value);
											if (num6 >= 0)
											{
												dataGridViewRow.Cells["f_PrivilegeTypeName"].Value = this.arrPrivilegeTypeName[num6];
											}
										}
										if (int.Parse(wgTools.SetObjToStr(sqlDataReader["f_DoorEnabled"])) > 0)
										{
											flag3 = true;
										}
									}
									sqlDataReader.Close();
									if (flag3)
									{
										dfrmMultiThreadOperation dfrmMultiThreadOperation2 = new dfrmMultiThreadOperation();
										dfrmMultiThreadOperation2.newCardNO4Cardlost = text2;
										dfrmMultiThreadOperation2.oldCardNO4Cardlost = text;
										dfrmMultiThreadOperation2.ConsumerID4Cardlost = num2;
										dfrmMultiThreadOperation2.startDownloadCardLostUser();
										DateTime.Now.AddSeconds(20.0);
										while (!dfrmMultiThreadOperation2.bComplete4Cardlost)
										{
											Application.DoEvents();
											Thread.Sleep(100);
										}
										if (dfrmMultiThreadOperation2.bCompleteFullOK4Cardlost)
										{
											flag = true;
										}
										dfrmMultiThreadOperation2.Close();
									}
								}
							}
							IL_062D:
							if (!flag)
							{
								this.dfrmWait1.Hide();
								wgAppConfig.wgLog(CommonStr.strDelAddAndUploadFail.Replace("\r\n", ""), EventLogEntryType.Information, null);
								XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			finally
			{
				Cursor.Current = Cursors.Default;
				this.dfrmWait1.Hide();
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000ACD64 File Offset: 0x000ABD64
		private void cloudServerStart()
		{
			try
			{
				if (this.watching == null)
				{
					if (wgTools.bUDPOnly64 > 0)
					{
						this.watching = frmADCT3000.watchingP64;
					}
					else
					{
						this.watching = new WatchingService();
					}
				}
				this.watching.EventHandler += this.evtNewInfoCallBack;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000ACDD0 File Offset: 0x000ABDD0
		private void createQRCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvUsers.SelectedRows.Count > 0)
			{
				int rowIndex = this.dgvUsers.SelectedCells[0].RowIndex;
				int num = int.Parse(this.dgvUsers.Rows[rowIndex].Cells["f_ConsumerID"].Value.ToString());
				string valStringBySql = wgAppConfig.getValStringBySql(string.Format("SELECT f_CardNO FROM t_b_consumer where f_ConsumerID = {0} ", num));
				try
				{
					if (string.IsNullOrEmpty(valStringBySql))
					{
						XMessageBox.Show(this, CommonStr.strSetCardNO4CreateQR, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						using (dfrmCreateQR dfrmCreateQR = new dfrmCreateQR())
						{
							dfrmCreateQR.Text = string.Format("{0}..[{1}]", dfrmCreateQR.Text, this.dgvUsers.Rows[rowIndex].Cells["f_ConsumerName"].Value.ToString());
							dfrmCreateQR.lblUser.Text = this.dgvUsers.Rows[rowIndex].Cells["f_ConsumerName"].Value.ToString();
							string text = wgAppConfig.getValStringBySql(string.Format("SELECT f_BeginYMD FROM t_b_consumer where f_ConsumerID = {0} ", num));
							dfrmCreateQR.dtpActivate.Value = DateTime.Parse(text);
							dfrmCreateQR.dateBeginHMS1.Value = DateTime.Parse(text);
							text = wgAppConfig.getValStringBySql(string.Format("SELECT f_EndYMD FROM t_b_consumer where f_ConsumerID = {0} ", num));
							dfrmCreateQR.dtpDeactivate.Value = DateTime.Parse(text);
							dfrmCreateQR.dateEndHMS1.Value = DateTime.Parse(text);
							dfrmCreateQR.consumerCardNO = long.Parse(valStringBySql);
							dfrmCreateQR.ShowDialog(this);
						}
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000ACFC8 File Offset: 0x000ABFC8
		private int deleteCardFromCloudServer(string strCardNO)
		{
			int num = 1;
			if (!(wgAppConfig.getSystemParamByNO(205) == "0.0.0.0") && !string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(205)) && icOperator.OperatorID != 1)
			{
				num = 0;
			}
			if (num <= 0)
			{
				string text = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
				if (!string.IsNullOrEmpty(text))
				{
					text = Program.Dpt4Database(text);
				}
				try
				{
					string text2 = string.Format("N3000 -USER {0} -PASSWORD '{1}'  -DELETEALLPRIVILEGE  -CardNO {2}", wgTools.PrepareStr(icOperator.OperatorName), text, strCardNO);
					try
					{
						int num2 = 60006;
						string systemParamByNO = wgAppConfig.getSystemParamByNO(205);
						this.tcp = new TcpClient();
						this.tcp.Connect(IPAddress.Parse(systemParamByNO), num2);
						string text3 = this.sendCommand(text2);
						if (!string.IsNullOrEmpty(text3))
						{
							wgAppConfig.wgLog(text3);
							num = 1;
						}
						this.tcp.Close();
						this.tcp = null;
					}
					catch
					{
						this.tcp = null;
					}
					wgAppConfig.wgLog(string.Format("N3000 -USER {0} -PASSWORD '{1}'  -DELETEALLPRIVILEGE  -CardNO {2}", wgTools.PrepareStr(icOperator.OperatorName), "***", strCardNO));
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
			return num;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000AD108 File Offset: 0x000AC108
		private void deletedUserManageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.Hide();
			using (dfrmDeletedUsersManage dfrmDeletedUsersManage = new dfrmDeletedUsersManage())
			{
				dfrmDeletedUsersManage.operateMode = "MANAGE";
				dfrmDeletedUsersManage.ShowDialog(this);
			}
			((frmADCT3000)base.ParentForm).closeChildForm4All();
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000AD160 File Offset: 0x000AC160
		private void deleteUserFromExcelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.Hide();
			using (dfrmDeleteUserFromExcel dfrmDeleteUserFromExcel = new dfrmDeleteUserFromExcel())
			{
				dfrmDeleteUserFromExcel.ShowDialog(this);
			}
			((frmADCT3000)base.ParentForm).closeChildForm4All();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000AD1B0 File Offset: 0x000AC1B0
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x000AD1C8 File Offset: 0x000AC1C8
		private void dfrm_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					wgAppConfig.findCall(this.dfrmFind1, this, this.dgvUsers);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x000AD224 File Offset: 0x000AC224
		private void dgvUsers_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x000AD234 File Offset: 0x000AC234
		private void dgvUsers_Scroll(object sender, ScrollEventArgs e)
		{
			if (!this.bLoadedFinished && e.ScrollOrientation == ScrollOrientation.VerticalScroll)
			{
				wgTools.WriteLine(e.OldValue.ToString());
				wgTools.WriteLine(e.NewValue.ToString());
				DataGridView dataGridView = this.dgvUsers;
				if (e.NewValue > e.OldValue && (e.NewValue + 100 + this.deletedUserCnt > dataGridView.Rows.Count + this.deletedUserCnt || e.NewValue + this.deletedUserCnt + (dataGridView.Rows.Count + this.deletedUserCnt) / 10 > dataGridView.Rows.Count + this.deletedUserCnt))
				{
					if (this.startRecordIndex <= dataGridView.Rows.Count + this.deletedUserCnt)
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
						wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + "#");
					}
				}
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000AD38F File Offset: 0x000AC38F
		private void displayAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!this.bLoadAll)
			{
				this.bLoadAll = true;
				this.btnQuery_Click(null, null);
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x000AD3A8 File Offset: 0x000AC3A8
		private void editPrivilegeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnEditPrivilege.PerformClick();
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x000AD3B8 File Offset: 0x000AC3B8
		private void evtNewInfoCallBack(string text)
		{
			wgTools.WgDebugWrite("Got text through callback! {0}", new object[] { text });
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x000AD3DB File Offset: 0x000AC3DB
		private void extendReaderSettingToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000AD3E0 File Offset: 0x000AC3E0
		private void fillDgv(DataView dv)
		{
			try
			{
				DataGridView dataGridView = this.dgvUsers;
				if (dataGridView.DataSource == null)
				{
					dataGridView.DataSource = dv;
					for (int i = 0; i < dv.Table.Columns.Count; i++)
					{
						dataGridView.Columns[i].DataPropertyName = dv.Table.Columns[i].ColumnName;
						dataGridView.Columns[i].Name = dv.Table.Columns[i].ColumnName;
					}
					wgAppConfig.setDisplayFormatDate(dataGridView, "f_BeginYMD", wgTools.DisplayFormat_DateYMD);
					wgAppConfig.setDisplayFormatDate(dataGridView, "f_EndYMD", wgTools.DisplayFormat_DateYMD);
					if (icConsumer.gTimeSecondEnabled)
					{
						wgAppConfig.setDisplayFormatDate(dataGridView, "f_BeginYMD", wgTools.DisplayFormat_DateYMDHMS);
						wgAppConfig.setDisplayFormatDate(dataGridView, "f_EndYMD", wgTools.DisplayFormat_DateYMDHMS);
					}
					wgAppConfig.ReadGVStyle(this, dataGridView);
					if (this.startRecordIndex == 0 && dv.Count >= this.MaxRecord)
					{
						this.startRecordIndex += this.MaxRecord;
						this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
					}
				}
				else if (dv.Count > 0)
				{
					int firstDisplayedScrollingRowIndex = dataGridView.FirstDisplayedScrollingRowIndex;
					DataView dataView = dataGridView.DataSource as DataView;
					dataView.Table.Merge(dv.Table);
					if (firstDisplayedScrollingRowIndex >= 0)
					{
						dataGridView.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x000AD5A4 File Offset: 0x000AC5A4
		private void frmUsers_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.stopWatch();
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x000AD5AC File Offset: 0x000AC5AC
		private void frmUsers_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.watching != null)
			{
				this.watching.StopWatch();
			}
			this.dfrm_FormClosing(sender, e);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000AD5CC File Offset: 0x000AC5CC
		private void frmUsers_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.funcCtrlShiftQ();
			}
			this.dfrm_KeyDown(sender, e);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x000AD620 File Offset: 0x000AC620
		private void frmUsers_Load(object sender, EventArgs e)
		{
			Icon icon = base.Icon;
			wgAppConfig.GetAppIcon(ref icon);
			base.Icon = icon;
			this.Deptname.HeaderText = wgAppConfig.ReplaceFloorRoom(this.Deptname.HeaderText);
			this.ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.ConsumerNO.HeaderText);
			this.loadOperatorPrivilege();
			this.loadPrivilegeType();
			this.userControlFind1.btnQuery.Click += this.btnQuery_Click;
			this.saveDefaultStyle();
			this.loadStyle();
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns["CardNO"]);
			Cursor.Current = Cursors.WaitCursor;
			if ((wgAppConfig.getParamValBoolByNO(178) && !wgAppConfig.IsAccessControlBlue) || wgAppConfig.getParamValBoolByNO(168) || wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AllowUploadUserMobile")) == "1")
			{
				if (wgAppConfig.getParamValBoolByNO(178) || wgAppConfig.getParamValBoolByNO(168))
				{
					this.dgvUsers.Columns["Cert"].Visible = true;
				}
				if (wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AllowUploadUserMobile")) == "1")
				{
					this.dgvUsers.Columns["Mobile"].Visible = true;
				}
			}
			if (wgAppConfig.getParamValBoolByNO(197))
			{
				this.dgvUsers.Columns["Cert"].Visible = true;
				this.dgvUsers.Columns["Mobile"].Visible = true;
			}
			if (wgAppConfig.getParamValBoolByNO(200))
			{
				this.dgvUsers.Columns["SecondCard"].Visible = true;
			}
			this.userControlFind1.btnQuery.PerformClick();
			bool flag;
			bool flag2;
			icOperator.getFrmOperatorPrivilege("mnuCardLost", out flag, out flag2);
			this.btnRegisterLostCard.Visible = flag2;
			bool flag3 = false;
			string text = "mnuCardLost";
			if (icOperator.OperatePrivilegeVisible(text, ref flag3))
			{
				this.btnRegisterLostCard.Visible = !flag3;
			}
			else
			{
				this.btnRegisterLostCard.Visible = false;
			}
			this.btnEditPrivilege.Visible = false;
			if (!wgAppConfig.getParamValBoolByNO(111))
			{
				flag3 = false;
				text = "mnu1DoorControl";
				if (icOperator.OperatePrivilegeVisible(text, ref flag3) && !flag3)
				{
					text = "mnuPrivilege";
					if (icOperator.OperatePrivilegeVisible(text, ref flag3) && !flag3)
					{
						this.btnEditPrivilege.Visible = true;
					}
				}
			}
			icControllerZone icControllerZone = new icControllerZone();
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			icControllerZone.getZone(ref arrayList, ref arrayList2, ref arrayList3);
			if (arrayList2.Count > 0 && (int)arrayList2[0] != 0)
			{
				this.btnEditPrivilege.Enabled = false;
			}
			if (!string.IsNullOrEmpty(this.userControlFind1.cboFindDept.Text))
			{
				this.queryUsersWithoutPrivilegeToolStripMenuItem.Enabled = false;
			}
			this.dgvUsers.ContextMenuStrip = this.contextMenuStrip1;
			this.editPrivilegeToolStripMenuItem.Visible = this.btnEditPrivilege.Visible;
			this.editPrivilegeToolStripMenuItem.Enabled = this.btnEditPrivilege.Enabled;
			this.mnuPrivilegeTypeManage.Enabled = this.btnEditPrivilege.Enabled;
			this.mnuPrivilegeTypeSet.Enabled = this.btnEditPrivilege.Enabled;
			if (wgAppConfig.IsPrivilegeTypeManagementModeActive)
			{
				if (this.btnEditPrivilege.Enabled)
				{
					this.btnEditPrivilegeType.Visible = this.btnEditPrivilege.Visible;
					this.mnuPrivilegeTypeManage.Visible = this.btnEditPrivilege.Visible;
					this.mnuPrivilegeTypeSet.Visible = this.btnEditPrivilege.Visible;
					if (this.arrPrivilegeTypeID.Count <= 0)
					{
						this.mnuPrivilegeTypeSet.Visible = false;
						this.btnEditPrivilegeType.Visible = false;
					}
				}
				else
				{
					this.btnEditPrivilegeType.Visible = false;
					this.mnuPrivilegeTypeManage.Visible = false;
					this.mnuPrivilegeTypeSet.Visible = false;
				}
				this.btnEditPrivilege.Visible = false;
				this.editPrivilegeToolStripMenuItem.Visible = false;
				this.mnuPrivilegeTypeManage.Visible = false;
			}
			this.btnFindBySFZID.Visible = wgAppConfig.getParamValBoolByNO(178) || wgAppConfig.getParamValBoolByNO(168);
			this.deleteUserFromExcelToolStripMenuItem.Visible = this.btnDelete.Visible;
			if (wgAppConfig.IsAccessControlBlue)
			{
				this.deleteUserFromExcelToolStripMenuItem.Visible = false;
				this.createQRCodeToolStripMenuItem.Visible = false;
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000ADA81 File Offset: 0x000ACA81
		private void funcCtrlShiftQ()
		{
			this.btnImportFromExcel.Visible = true;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000ADA90 File Offset: 0x000ACA90
		private int getDeptId(string deptName)
		{
			icGroup icGroup = new icGroup();
			int num = icGroup.getGroupID(deptName);
			if (num > 0)
			{
				return num;
			}
			string text = deptName;
			while (text.IndexOf("\\\\") >= 0)
			{
				text = text.Replace("\\\\", "\\");
			}
			if (text.Substring(0, 1) == "\\")
			{
				text = text.Substring(1);
			}
			if (text.Substring(text.Length - 1) == "\\")
			{
				text = text.Substring(0, text.Length - 1);
			}
			num = icGroup.getGroupID(text);
			if (num > 0)
			{
				return num;
			}
			string[] array = text.Split(new char[] { '\\' });
			string text2 = "";
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (text2 == "")
				{
					text2 = array[i];
				}
				else
				{
					text2 = text2 + "\\" + array[i];
				}
				if (flag || !icGroup.checkExisted(text2))
				{
					flag = true;
					icGroup.addNew4BatchExcel(text2);
				}
			}
			return icGroup.getGroupID(text);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000ADBA8 File Offset: 0x000ACBA8
		private void importFromExcelWithDateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				this.openFileDialog1.Filter = " (*.xls)|*.xls| (*.*)|*.*";
				this.openFileDialog1.FilterIndex = 1;
				this.openFileDialog1.RestoreDirectory = true;
				try
				{
					this.openFileDialog1.InitialDirectory = ".\\REPORT";
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				this.openFileDialog1.Title = this.btnImportFromExcel.Text;
				this.openFileDialog1.FileName = "";
				if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					string fileName = this.openFileDialog1.FileName;
					this.dfrmWait1.Show();
					this.dfrmWait1.Refresh();
					wgTools.WriteLine("start");
					int num = 0;
					int num2 = 1;
					int num3 = 2;
					int num4 = 3;
					int num5 = -1;
					int num6 = 4;
					int num7 = 5;
					int num8 = 6;
					int num9 = 7;
					int num10 = -1;
					if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_EXCELMODE")))
					{
						int.TryParse(wgAppConfig.GetKeyVal("KEY_EXCELMODE"), out num10);
					}
					bool flag = false;
					bool flag2 = false;
					int i = 0;
					while (i < 2)
					{
						try
						{
							this.MyConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; data source= " + fileName + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE'");
							string text = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE\"";
							if (fileName != string.Empty)
							{
								FileInfo fileInfo = new FileInfo(fileName);
								if (fileInfo.Extension.Equals(".xls"))
								{
									this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
									switch (i)
									{
									case 0:
										this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
										break;
									case 1:
										this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "ACE", "12.0", fileName, "8.0" }));
										break;
									}
								}
								else if (fileInfo.Extension.Equals(".xlsx"))
								{
									this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Ace", "12.0", fileName, "12.0" }));
									i = 1;
								}
								else
								{
									this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
									i = 1;
								}
							}
							try
							{
								this.MyConnection.Open();
								flag2 = false;
								flag = true;
							}
							catch (OleDbException)
							{
								flag2 = true;
								throw;
							}
							catch
							{
								throw;
							}
						}
						catch
						{
							if (i == 1)
							{
								if (flag2)
								{
									XMessageBox.Show(CommonStr.strSaveAsXLSFile);
								}
								throw;
							}
						}
						i++;
						if (flag)
						{
							break;
						}
					}
					this.DS = new DataSet();
					DataTable dataTable = null;
					if (flag)
					{
						dataTable = this.MyConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
						this.MyConnection.Close();
					}
					string text2 = "";
					if (dataTable == null || dataTable.Rows.Count <= 0)
					{
						XMessageBox.Show(this.btnImportFromExcel.Text + ": " + 0);
					}
					else
					{
						text2 = wgTools.SetObjToStr(dataTable.Rows[0][2]);
						for (int j = 0; j <= dataTable.Rows.Count - 1; j++)
						{
							if (wgTools.SetObjToStr(dataTable.Rows[j][2]) == "用户" || wgTools.SetObjToStr(dataTable.Rows[j][2]) == "用戶" || wgTools.SetObjToStr(dataTable.Rows[j][2]) == "Users")
							{
								text2 = wgTools.SetObjToStr(dataTable.Rows[j][2]);
								break;
							}
						}
						num = -1;
						num2 = -1;
						num3 = -1;
						num4 = -1;
						num5 = -1;
						num6 = -1;
						num7 = -1;
						num8 = -1;
						num9 = -1;
						if (text2.IndexOf("$") <= 0)
						{
							text2 += "$";
						}
						try
						{
							this.MyCommand = new OleDbDataAdapter("select * from [" + text2 + "A1:Z1]", this.MyConnection);
							this.MyCommand.Fill(this.DS, "userInfoTitle");
							string columnName = this.DS.Tables["userInfoTitle"].Columns[0].ColumnName;
							for (int k = 0; k <= this.DS.Tables["userInfoTitle"].Columns.Count - 1; k++)
							{
								object columnName2 = this.DS.Tables["userInfoTitle"].Columns[k].ColumnName;
								if (wgTools.SetObjToStr(columnName2) != "")
								{
									string text3;
									if (wgTools.SetObjToStr(columnName2).ToUpper() == "User ID".ToUpper())
									{
										num = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "User Name".ToUpper())
									{
										num2 = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "Card NO".ToUpper())
									{
										num3 = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "Department".ToUpper() || wgTools.SetObjToStr(columnName2).ToUpper() == CommonStr.strReplaceFloorRoom.ToUpper())
									{
										num4 = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "Active Date".ToUpper())
									{
										num6 = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "Deactive Date".ToUpper())
									{
										num7 = k;
									}
									else
										switch (text3 = wgTools.SetObjToStr(columnName2).ToUpper().Substring(0, 2))
										{
										case "NO":
										case "用户":
										case "用戶":
										case "编号":
										case "編號":
										case "WO":
										case "工号":
										case "工號":
											num = k;
											break;
										case "NA":
										case "姓名":
											num2 = k;
											break;
										case "CA":
										case "卡号":
										case "卡號":
											num3 = k;
											break;
										case "DE":
										case "部门":
										case "部門":
											num4 = k;
											break;
										case "起始":
											num6 = k;
											break;
										case "截止":
											num7 = k;
											break;
										case "身份":
											num8 = k;
											break;
										case "手机":
										case "手機":
											num9 = k;
											break;
										}
								}
							}
						}
						catch (Exception ex2)
						{
							wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
							wgAppConfig.wgLog(ex2.ToString());
						}
						if (num2 < 0)
						{
							XMessageBox.Show(CommonStr.strWrongUsersFile);
						}
						else
						{
							string text4 = "";
							int num12 = 0;
							try
							{
								int num13 = Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(num, num2), num3), num4), num6), num7), num8), num9);
								string text5 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
								if (num13 < text5.Length)
								{
									this.MyCommand = new OleDbDataAdapter(string.Concat(new string[]
									{
										"select * from [",
										text2,
										"A1:",
										text5.Substring(num13, 1),
										"65535]"
									}), this.MyConnection);
									this.MyCommand.Fill(this.DS, "userInfo");
								}
							}
							catch (Exception ex3)
							{
								wgTools.WgDebugWrite(ex3.ToString(), new object[0]);
								wgAppConfig.wgLog(ex3.ToString());
							}
							this.dv = new DataView(this.DS.Tables["userInfo"]);
							int num14 = 0;
							long num15 = new icConsumer().ConsumerNONext();
							long num16 = num15;
							if (num15 < 0L)
							{
								num16 = 1L;
							}
							string text6 = "";
							new icConsumer();
							for (int l = 0; l <= this.dv.Count - 1; l++)
							{
								int num17;
								if (wgTools.SetObjToStr(this.dv[l][num2]).Trim() != "" && !(text6 == "") && int.TryParse(text6, out num17))
								{
									num16 = Math.Max(num16, (long)(int.Parse(text6) + 1));
								}
							}
							for (int m = 0; m <= this.dv.Count - 1; m++)
							{
								string text7 = "";
								text6 = "";
								string text8 = "";
								string text9 = "";
								string text10 = "";
								string text11 = "";
								string text12 = "";
								string text13 = "";
								string text14 = wgTools.SetObjToStr(this.dv[m][num2]).Trim();
								if (num >= 0)
								{
									text6 = wgTools.SetObjToStr(this.dv[m][num]).Trim();
								}
								if (num5 >= 0)
								{
									text8 = wgTools.SetObjToStr(this.dv[m][num5]).Trim();
								}
								if (num3 >= 0)
								{
									text7 = wgTools.SetObjToStr(this.dv[m][num3]).Trim();
								}
								if (num4 >= 0)
								{
									text9 = wgTools.SetObjToStr(this.dv[m][num4]).Trim();
								}
								if (num6 >= 0)
								{
									text10 = wgTools.deleInvalid(wgTools.SetObjToStr(this.dv[m][num6])).Trim();
								}
								if (num7 >= 0)
								{
									text11 = wgTools.deleInvalid(wgTools.SetObjToStr(this.dv[m][num7])).Trim();
								}
								if (num8 >= 0)
								{
									text12 = wgTools.SetObjToStr(this.dv[m][num8]).Trim();
								}
								if (num9 >= 0)
								{
									text13 = wgTools.SetObjToStr(this.dv[m][num9]).Trim();
								}
								if (text14 != "")
								{
									int num18;
									if (text6 == "")
									{
										text6 = num16.ToString();
									}
									else if (int.TryParse(text6, out num18))
									{
										num16 = Math.Max(num16, (long)int.Parse(text6));
									}
									if (this.addConsumerNew(text6, text14, text7, text9, text10, text11) > 0)
									{
										if ((num8 >= 0 && !string.IsNullOrEmpty(text12)) || (num9 >= 0 && !string.IsNullOrEmpty(text13)))
										{
											string text15 = " UPDATE t_b_Consumer_Other  SET  ";
											text15 = string.Concat(new string[]
											{
												text15,
												"   f_CertificateID         = ",
												wgTools.PrepareStrNUnicode(text12),
												"  , f_Mobile                = ",
												wgTools.PrepareStrNUnicode(text13)
											});
											if (wgAppConfig.IsAccessDB)
											{
												text15 = " UPDATE t_b_Consumer_Other   ";
												text15 = string.Concat(new string[]
												{
													text15,
													" INNER JOIN t_b_Consumer  ON (  t_b_Consumer_Other.[f_ConsumerID] = t_b_Consumer.[f_ConsumerID]   AND  t_b_Consumer.[f_ConsumerNO] = ",
													wgTools.PrepareStrNUnicode(text6.ToString().PadLeft(10, ' ')),
													") SET     f_CertificateID         = ",
													wgTools.PrepareStrNUnicode(text12),
													"  , f_Mobile                = ",
													wgTools.PrepareStrNUnicode(text13)
												});
											}
											else
											{
												text15 = " UPDATE t_b_Consumer_Other   ";
												text15 = string.Concat(new string[]
												{
													text15,
													" SET     f_CertificateID         = ",
													wgTools.PrepareStrNUnicode(text12),
													"  , f_Mobile                = ",
													wgTools.PrepareStrNUnicode(text13),
													" FROM   t_b_Consumer_Other,t_b_Consumer  WHERE  t_b_Consumer_Other.[f_ConsumerID] = t_b_Consumer.[f_ConsumerID]   AND  t_b_Consumer.[f_ConsumerNO] = ",
													wgTools.PrepareStrNUnicode(text6.ToString().PadLeft(10, ' '))
												});
											}
											wgAppConfig.runUpdateSql(text15);
										}
										num14++;
										num16 += 1L;
									}
									else
									{
										text4 = text4 + text14 + ",";
										num12++;
									}
								}
								else
								{
									if (text6 != "" || text14 != "" || text7 != "" || text9 != "" || text8 != "")
									{
										text4 = string.Concat(new object[]
										{
											text4,
											"L",
											m + 1,
											","
										});
										num12++;
									}
									if (text4.Length > 500)
									{
										break;
									}
								}
								if (m >= 65535)
								{
									break;
								}
								wgAppRunInfo.raiseAppRunInfoLoadNums((num12 + num14).ToString() + " / " + this.dv.Count.ToString());
								this.dfrmWait1.Text = (num12 + num14).ToString() + " / " + this.dv.Count.ToString();
								Application.DoEvents();
							}
							wgAppRunInfo.raiseAppRunInfoLoadNums((num12 + num14).ToString() + " / " + this.dv.Count.ToString());
							this.dfrmWait1.Text = (num12 + num14).ToString() + " / " + this.dv.Count.ToString();
							new icGroup().updateGroupNO();
							wgTools.WriteLine("Import end");
							if (!(text4 == ""))
							{
								this.dfrmWait1.Hide();
								wgTools.WgDebugWrite(string.Concat(new string[]
								{
									CommonStr.strImportUsersWrong,
									"\r\n",
									CommonStr.strNotImportedUsers,
									num12.ToString(),
									"\r\n",
									text4
								}), new object[0]);
								XMessageBox.Show(string.Concat(new string[]
								{
									CommonStr.strImportUsersWrong,
									"\r\n\r\n",
									CommonStr.strNotImportedUsers,
									num12.ToString(),
									"\r\n\r\n",
									text4
								}));
								wgAppConfig.wgLog(string.Concat(new string[]
								{
									CommonStr.strImportUsersWrong,
									"\r\n",
									CommonStr.strNotImportedUsers,
									num12.ToString(),
									"\r\n",
									text4
								}));
							}
							this.dfrmWait1.Hide();
							wgAppConfig.wgLog(this.btnImportFromExcel.Text + ": " + num14);
							icConsumerShare.setUpdateLog();
							XMessageBox.Show(this.btnImportFromExcel.Text + ": " + num14);
							this.reloadUserData("");
						}
					}
				}
			}
			catch (Exception ex4)
			{
				wgTools.WgDebugWrite(ex4.ToString(), new object[0]);
				wgAppConfig.wgLog(ex4.ToString());
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
				Cursor.Current = Cursors.Default;
				this.dfrmWait1.Hide();
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x000AED50 File Offset: 0x000ADD50
		private void loadDefaultStyle()
		{
			DataTable dataTable = this.dsDefaultStyle.Tables[this.dgvUsers.Name];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				this.dgvUsers.Columns[i].Name = dataTable.Rows[i]["colName"].ToString();
				this.dgvUsers.Columns[i].HeaderText = dataTable.Rows[i]["colHeader"].ToString();
				this.dgvUsers.Columns[i].Width = int.Parse(dataTable.Rows[i]["colWidth"].ToString());
				this.dgvUsers.Columns[i].Visible = bool.Parse(dataTable.Rows[i]["colVisable"].ToString());
				this.dgvUsers.Columns[i].DisplayIndex = int.Parse(dataTable.Rows[i]["colDisplayIndex"].ToString());
			}
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000AEE9C File Offset: 0x000ADE9C
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuConsumers";
			if (icOperator.OperatePrivilegeVisible(text, ref flag))
			{
				if (flag)
				{
					this.btnAutoAdd.Visible = false;
					this.btnAdd.Visible = false;
					this.btnEdit.Visible = false;
					this.btnDelete.Visible = false;
					this.btnImportFromExcel.Visible = false;
					this.btnBatchUpdate.Visible = false;
					this.mnuPrivilegeTypeManage.Visible = false;
					this.batchUpdateSelectToolStripMenuItem.Visible = false;
					this.importFromExcelToolStripMenuItem.Visible = false;
					this.deleteUserFromExcelToolStripMenuItem.Visible = false;
					this.deletedUserManageToolStripMenuItem.Visible = false;
					this.importFromExcelWithDateToolStripMenuItem.Visible = false;
					return;
				}
				if (wgAppConfig.getSystemParamByNO(205) != "0.0.0.0" && !string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(205)) && icOperator.OperatorID != 1)
				{
					this.btnAutoAdd.Visible = false;
					this.btnAdd.Visible = false;
					this.btnDelete.Visible = false;
					this.btnImportFromExcel.Visible = false;
					this.btnBatchUpdate.Visible = false;
					this.mnuPrivilegeTypeManage.Visible = false;
					this.batchUpdateSelectToolStripMenuItem.Visible = false;
					this.importFromExcelToolStripMenuItem.Visible = false;
					this.deleteUserFromExcelToolStripMenuItem.Visible = false;
					this.deletedUserManageToolStripMenuItem.Visible = false;
					this.importFromExcelWithDateToolStripMenuItem.Visible = false;
					return;
				}
			}
			else
			{
				base.Close();
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x000AF018 File Offset: 0x000AE018
		private void loadPrivilegeType()
		{
			this.arrPrivilegeTypeName.Clear();
			this.arrPrivilegeTypeID.Clear();
			try
			{
				string text = "SELECT * FROM t_d_PrivilegeType ORDER BY f_PrivilegeTypeID ";
				DataTable dataTable = new DataTable("t_d_PrivilegeType");
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
						goto IL_00CA;
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
				IL_00CA:
				text = "SELECT * FROM t_d_Privilege_Of_PrivilegeType  ";
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2))
						{
							using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(oleDbCommand2))
							{
								oleDbDataAdapter2.Fill(this.dtPrivilege_Of_PrivilegeType);
							}
						}
						goto IL_0185;
					}
				}
				using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
					{
						using (SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sqlCommand2))
						{
							sqlDataAdapter2.Fill(this.dtPrivilege_Of_PrivilegeType);
						}
					}
				}
				IL_0185:
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (object obj in dataTable.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						this.arrPrivilegeTypeID.Add(dataRow["f_PrivilegeTypeID"]);
						this.arrPrivilegeTypeName.Add(dataRow["f_PrivilegeTypeName"]);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			if (this.arrPrivilegeTypeID.Count <= 0)
			{
				this.dgvUsers.Columns["PrivilegeType"].Visible = false;
				this.mnuPrivilegeTypeSet.Visible = false;
				this.btnEditPrivilegeType.Visible = false;
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x000AF3D8 File Offset: 0x000AE3D8
		private void loadStyle()
		{
			this.dgvUsers.AutoGenerateColumns = false;
			bool paramValBoolByNO = wgAppConfig.getParamValBoolByNO(113);
			this.dgvUsers.Columns[5].Visible = paramValBoolByNO;
			if (icConsumer.gTimeSecondEnabled)
			{
				this.dgvUsers.Columns[7].Width = 120;
				this.dgvUsers.Columns[8].Width = 120;
			}
			wgAppConfig.ReadGVStyle(this, this.dgvUsers);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x000AF454 File Offset: 0x000AE454
		private DataView loadUserData(int startIndex, int maxRecords, string strSql)
		{
			wgTools.WriteLine("loadUserData Start");
			if (strSql.ToUpper().IndexOf("SELECT ") > 0)
			{
				strSql = string.Format("SELECT TOP {0:d} ", maxRecords) + strSql.Substring(strSql.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
			}
			if (startIndex == 0)
			{
				this.recNOMax = "";
			}
			else if (strSql.ToUpper().IndexOf(" WHERE ") > 0)
			{
				strSql += string.Format(" AND f_ConsumerNO > {0}", wgTools.PrepareStrNUnicode(this.recNOMax));
			}
			else
			{
				strSql += string.Format(" WHERE f_ConsumerNO > {0}", wgTools.PrepareStrNUnicode(this.recNOMax));
			}
			strSql += " ORDER BY f_ConsumerNO ";
			this.tb4loadUserData = new DataTable("users");
			this.dv4loadUserData = new DataView(this.tb4loadUserData);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.tb4loadUserData);
						}
					}
					goto IL_0187;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.tb4loadUserData);
					}
				}
			}
			IL_0187:
			if (this.tb4loadUserData.Rows.Count > 0)
			{
				this.recNOMax = this.tb4loadUserData.Rows[this.tb4loadUserData.Rows.Count - 1]["f_ConsumerNO"].ToString();
				this.updatePrivilegeTypeName(ref this.tb4loadUserData);
			}
			wgTools.WriteLine("loadUserData End");
			return this.dv4loadUserData;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x000AF69C File Offset: 0x000AE69C
		private void mnuPrivilegeTypeManage_Click(object sender, EventArgs e)
		{
			base.Hide();
			using (dfrmPrivilegeTypeManage dfrmPrivilegeTypeManage = new dfrmPrivilegeTypeManage())
			{
				dfrmPrivilegeTypeManage.operateMode = "MANAGE";
				dfrmPrivilegeTypeManage.ShowDialog(this);
			}
			((frmADCT3000)base.ParentForm).closeChildForm4All();
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x000AF6F4 File Offset: 0x000AE6F4
		private void mnuPrivilegeTypeSet_Click(object sender, EventArgs e)
		{
			if (this.dgvUsers.SelectedRows.Count > 0)
			{
				if (this.dgvUsers.SelectedRows.Count == 1)
				{
					if (wgTools.bUDPCloud > 0)
					{
						this.startWatch();
					}
					int rowIndex = this.dgvUsers.SelectedCells[0].RowIndex;
					using (dfrmPrivilegeTypeManage dfrmPrivilegeTypeManage = new dfrmPrivilegeTypeManage())
					{
						int num = int.Parse(this.dgvUsers.Rows[rowIndex].Cells["f_ConsumerID"].Value.ToString());
						int num2 = int.Parse(this.dgvUsers.Rows[rowIndex].Cells["f_PrivilegeTypeID"].Value.ToString());
						dfrmPrivilegeTypeManage.selectID = int.Parse(this.dgvUsers.Rows[rowIndex].Cells["f_PrivilegeTypeID"].Value.ToString());
						dfrmPrivilegeTypeManage.selectConsumerID = int.Parse(this.dgvUsers.Rows[rowIndex].Cells["f_ConsumerID"].Value.ToString());
						dfrmPrivilegeTypeManage.operateMode = "SELECT";
						dfrmPrivilegeTypeManage.Text = this.btnEditPrivilegeType.Text;
						dfrmPrivilegeTypeManage.Text = string.Format("{0} --{1}", dfrmPrivilegeTypeManage.Text, this.dgvUsers.Rows[rowIndex].Cells["f_ConsumerName"].Value.ToString());
						if (dfrmPrivilegeTypeManage.ShowDialog(this) == DialogResult.OK)
						{
							this.dgvUsers.Rows[rowIndex].Cells["f_PrivilegeTypeID"].Value = dfrmPrivilegeTypeManage.selectID;
							int num3 = this.arrPrivilegeTypeID.IndexOf(dfrmPrivilegeTypeManage.selectID);
							if (num3 >= 0)
							{
								this.dgvUsers.Rows[rowIndex].Cells["f_PrivilegeTypeName"].Value = this.arrPrivilegeTypeName[num3];
							}
							else
							{
								this.dgvUsers.Rows[rowIndex].Cells["f_PrivilegeTypeName"].Value = "";
							}
							if (wgAppConfig.IsAcceleratorActive && !this.bPrivilegTypeDownloadFailFirst)
							{
								bool flag = false;
								dfrmMultiThreadOperation dfrmMultiThreadOperation = new dfrmMultiThreadOperation();
								dfrmMultiThreadOperation.newCardNO4Cardlost = num.ToString();
								dfrmMultiThreadOperation.oldCardNO4Cardlost = "";
								dfrmMultiThreadOperation.ConsumerID4Cardlost = num;
								ArrayList arrayList = new ArrayList();
								DataView dataView = new DataView(this.dtPrivilege_Of_PrivilegeType);
								dataView.RowFilter = "f_ConsumerID = " + num2.ToString();
								for (int i = 0; i < dataView.Count; i++)
								{
									int num4 = (int)dataView[i]["f_ControllerID"];
									if (arrayList.IndexOf(num4) < 0)
									{
										arrayList.Add(num4);
									}
								}
								Cursor.Current = Cursors.WaitCursor;
								dfrmMultiThreadOperation.delPrivilegeController = arrayList;
								dfrmMultiThreadOperation.startDownloadCardLostUser();
								DateTime.Now.AddSeconds(10.0);
								while (!dfrmMultiThreadOperation.bComplete4Cardlost)
								{
									Application.DoEvents();
									Thread.Sleep(100);
								}
								if (dfrmMultiThreadOperation.bCompleteFullOK4Cardlost)
								{
									flag = true;
								}
								Cursor.Current = Cursors.Default;
								if (!flag)
								{
									this.bPrivilegTypeDownloadFailFirst = true;
									XMessageBox.Show(CommonStr.strPrivilegTypeDownloadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
								}
							}
						}
						this.Refresh();
						return;
					}
				}
				using (dfrmPrivilegeTypeManage dfrmPrivilegeTypeManage2 = new dfrmPrivilegeTypeManage())
				{
					string text = "";
					for (int j = 0; j < this.dgvUsers.SelectedRows.Count; j++)
					{
						int index = this.dgvUsers.SelectedRows[j].Index;
						int num5 = int.Parse(this.dgvUsers.Rows[index].Cells[0].Value.ToString());
						if (!string.IsNullOrEmpty(text))
						{
							text += ",";
						}
						text += num5.ToString();
					}
					dfrmPrivilegeTypeManage2.operateMode = "SELECT";
					dfrmPrivilegeTypeManage2.Text = this.btnEditPrivilegeType.Text;
					dfrmPrivilegeTypeManage2.strSqlSelected = text;
					dfrmPrivilegeTypeManage2.selectID = -1;
					dfrmPrivilegeTypeManage2.Text = string.Format("{0}: [{1}]", this.mnuPrivilegeTypeSet.Text, this.dgvUsers.SelectedRows.Count.ToString());
					if (dfrmPrivilegeTypeManage2.ShowDialog(this) == DialogResult.OK)
					{
						this.reloadUserData("");
					}
				}
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x000AFBE0 File Offset: 0x000AEBE0
		private void queryUsersWithoutPrivilegeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(178) || wgAppConfig.getParamValBoolByNO(168) || wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AllowUploadUserMobile")) == "1" || wgAppConfig.getParamValBoolByNO(197) || wgAppConfig.getParamValBoolByNO(200))
			{
				this.queryUsersWithoutPrivilegeToolStripMenuItem_ClickWithOther(sender, e);
				return;
			}
			this.deletedUserCnt = 0;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.txtFindCardID.Text = "";
			this.userControlFind1.txtFindName.Text = "";
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM ( t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID )";
			if (num5 > 0)
			{
				text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM  ( t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID )  WHERE  t_b_Consumer.f_ConsumerID = " + num5.ToString();
			}
			else if (num > 0)
			{
				text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType;
				if (num >= num3)
				{
					text2 = text2 + " FROM (t_b_Consumer INNER JOIN t_b_Group  ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", num2);
				}
				else
				{
					text2 = text2 + " FROM (t_b_Consumer INNER JOIN t_b_Group  ON ( t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", num3);
				}
				if (text != "")
				{
					text2 += string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text)));
				}
				else if (num4 > 0L)
				{
					text2 += string.Format(" AND f_CardNO ={0:d} ", num4);
				}
				text2 += " )) ";
			}
			else if (text != "")
			{
				text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM ( t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text)));
			}
			else if (num4 > 0L)
			{
				text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM ( t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) " + string.Format(" WHERE f_CardNO ={0:d} ", num4);
			}
			string text3 = "  t_d_privilege.f_ConsumerID is null  ";
			if (text2.ToUpper().IndexOf("WHERE") > 0)
			{
				text2 = text2.ToUpper().Replace("WHERE", "  LEFT OUTER JOIN t_d_privilege on t_b_Consumer.f_ConsumerID=t_d_privilege.f_ConsumerID  WHERE ") + string.Format(" AND {0} ", text3);
			}
			else
			{
				text2 = text2 + "  LEFT OUTER JOIN t_d_privilege on t_b_Consumer.f_ConsumerID=t_d_privilege.f_ConsumerID  " + string.Format(" WHERE {0} ", text3);
			}
			this.reloadUserData(text2);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000AFE88 File Offset: 0x000AEE88
		private void queryUsersWithoutPrivilegeToolStripMenuItem_ClickWithOther(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(178) || wgAppConfig.getParamValBoolByNO(168) || wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AllowUploadUserMobile")) == "1" || wgAppConfig.getParamValBoolByNO(197) || wgAppConfig.getParamValBoolByNO(200))
			{
				this.deletedUserCnt = 0;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				string text = "";
				long num4 = 0L;
				int num5 = 0;
				this.userControlFind1.txtFindCardID.Text = "";
				this.userControlFind1.txtFindName.Text = "";
				this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
				string text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName  " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM ((t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID ) ";
				if (num5 > 0)
				{
					text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM  (( t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID )  LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID )  WHERE  t_b_Consumer.f_ConsumerID = " + num5.ToString();
				}
				else if (num > 0)
				{
					text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone ";
					if (num >= num3)
					{
						text2 = text2 + " FROM ((t_b_Consumer INNER JOIN t_b_Group  ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", num2);
					}
					else
					{
						text2 = text2 + " FROM ((t_b_Consumer INNER JOIN t_b_Group  ON ( t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", num3);
					}
					if (text != "")
					{
						text2 += string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text)));
					}
					else if (num4 > 0L)
					{
						text2 += string.Format(" AND f_CardNO ={0:d} ", num4);
					}
					text2 += " ))  LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID ) ";
				}
				else if (text != "")
				{
					text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (( t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID )  LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID ) " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text)));
				}
				else if (num4 > 0L)
				{
					text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (( t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID )  LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID )" + string.Format(" WHERE f_CardNO ={0:d} ", num4);
				}
				string text3 = "  t_d_privilege.f_ConsumerID is null  ";
				if (text2.ToUpper().IndexOf("WHERE") > 0)
				{
					text2 = text2.ToUpper().Replace("WHERE", "  LEFT OUTER JOIN t_d_privilege on t_b_Consumer.f_ConsumerID=t_d_privilege.f_ConsumerID  WHERE ") + string.Format(" AND {0} ", text3);
				}
				else
				{
					text2 = text2 + "  LEFT OUTER JOIN t_d_privilege on t_b_Consumer.f_ConsumerID=t_d_privilege.f_ConsumerID  " + string.Format(" WHERE {0} ", text3);
				}
				this.reloadUserData(text2);
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000B012C File Offset: 0x000AF12C
		private void reloadSingleUser(int selectRowId)
		{
			try
			{
				int num = int.Parse(this.dgvUsers.Rows[selectRowId].Cells[0].Value.ToString());
				DataGridViewRow dataGridViewRow = this.dgvUsers.Rows[selectRowId];
				string text = " SELECT     t_b_Consumer.f_ConsumerID , f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  WHERE f_ConsumerID= " + num.ToString();
				if (wgAppConfig.getParamValBoolByNO(178) || wgAppConfig.getParamValBoolByNO(168) || wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AllowUploadUserMobile")) == "1" || wgAppConfig.getParamValBoolByNO(197) || wgAppConfig.getParamValBoolByNO(200))
				{
					text = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName  " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID  WHERE t_b_Consumer.f_ConsumerID= " + num.ToString();
				}
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							oleDbConnection.Open();
							OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
							if (oleDbDataReader.Read())
							{
								int num2 = 1;
								while (num2 < this.dgvUsers.Columns.Count && num2 < oleDbDataReader.FieldCount)
								{
									dataGridViewRow.Cells[num2].Value = oleDbDataReader[num2];
									num2++;
								}
								if (this.arrPrivilegeTypeID.Count > 0 && (int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value > 0)
								{
									int num3 = this.arrPrivilegeTypeID.IndexOf((int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value);
									if (num3 >= 0)
									{
										dataGridViewRow.Cells["f_PrivilegeTypeName"].Value = this.arrPrivilegeTypeName[num3];
									}
								}
							}
							oleDbDataReader.Close();
						}
						return;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						if (sqlDataReader.Read())
						{
							int num4 = 1;
							while (num4 < this.dgvUsers.Columns.Count && num4 < sqlDataReader.FieldCount)
							{
								dataGridViewRow.Cells[num4].Value = sqlDataReader[num4];
								num4++;
							}
							if (this.arrPrivilegeTypeID.Count > 0 && (int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value > 0)
							{
								int num5 = this.arrPrivilegeTypeID.IndexOf((int)dataGridViewRow.Cells["f_PrivilegeTypeID"].Value);
								if (num5 >= 0)
								{
									dataGridViewRow.Cells["f_PrivilegeTypeName"].Value = this.arrPrivilegeTypeName[num5];
								}
							}
						}
						sqlDataReader.Close();
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x000B04C4 File Offset: 0x000AF4C4
		private void reloadUserData(string strsql)
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
				this.dgvUsers.DataSource = null;
				if (this.bLoadAll)
				{
					this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, 100000000, this.dgvSql });
					return;
				}
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x000B058D File Offset: 0x000AF58D
		private void restoreDefaultLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.RestoreGVStyle(this, this.dgvUsers);
			this.loadDefaultStyle();
			this.loadStyle();
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000B05A8 File Offset: 0x000AF5A8
		private void saveDefaultStyle()
		{
			DataTable dataTable = new DataTable();
			this.dsDefaultStyle.Tables.Add(dataTable);
			dataTable.TableName = this.dgvUsers.Name;
			dataTable.Columns.Add("colName");
			dataTable.Columns.Add("colHeader");
			dataTable.Columns.Add("colWidth");
			dataTable.Columns.Add("colVisable");
			dataTable.Columns.Add("colDisplayIndex");
			for (int i = 0; i < this.dgvUsers.ColumnCount; i++)
			{
				DataGridViewColumn dataGridViewColumn = this.dgvUsers.Columns[i];
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

		// Token: 0x06000655 RID: 1621 RVA: 0x000B06DD File Offset: 0x000AF6DD
		private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.SaveDGVStyle(this, this.dgvUsers);
			XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000B0708 File Offset: 0x000AF708
		private string sendCommand(string strCommand)
		{
			byte[] bytes = Encoding.GetEncoding(this.encodingOfTCP).GetBytes(strCommand);
			NetworkStream stream = this.tcp.GetStream();
			while (stream.CanRead && stream.DataAvailable)
			{
				byte[] array = new byte[2000];
				stream.Read(array, 0, array.Length);
			}
			if (stream.CanWrite)
			{
				stream.Write(bytes, 0, bytes.Length);
			}
			DateTime dateTime = DateTime.Now.AddSeconds(2.0);
			byte[] array2 = new byte[2000];
			while (dateTime > DateTime.Now)
			{
				if (stream.CanRead && stream.DataAvailable)
				{
					stream.Read(array2, 0, array2.Length);
					return Encoding.GetEncoding(this.encodingOfTCP).GetString(array2);
				}
			}
			return "";
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x000B07D8 File Offset: 0x000AF7D8
		public void startWatch()
		{
			if (this.watching == null)
			{
				this.cloudServerStart();
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x000B07E8 File Offset: 0x000AF7E8
		public void stopWatch()
		{
			if (this.watching != null)
			{
				this.watching.StopWatch();
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x000B0800 File Offset: 0x000AF800
		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(178) || wgAppConfig.getParamValBoolByNO(168) || wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AllowUploadUserMobile")) == "1" || wgAppConfig.getParamValBoolByNO(197) || wgAppConfig.getParamValBoolByNO(200))
			{
				this.toolStripMenuItem1_ClickWithOther(sender, e);
				return;
			}
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
				this.deletedUserCnt = 0;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				string text2 = "";
				long num4 = 0L;
				int num5 = 0;
				this.userControlFind1.txtFindCardID.Text = "";
				this.userControlFind1.txtFindName.Text = "";
				this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text2, ref num4, ref num5);
				string text3 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ";
				if (num5 > 0)
				{
					text3 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  WHERE  t_b_Consumer.f_ConsumerID = " + num5.ToString();
				}
				else if (num > 0)
				{
					text3 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType;
					if (num >= num3)
					{
						text3 = text3 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", num2);
					}
					else
					{
						text3 = text3 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", num3);
					}
					if (text2 != "")
					{
						text3 += string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text2)));
					}
					else if (num4 > 0L)
					{
						text3 += string.Format(" AND f_CardNO ={0:d} ", num4);
					}
				}
				else if (text2 != "")
				{
					text3 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text2)));
				}
				else if (num4 > 0L)
				{
					text3 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID " + string.Format(" WHERE f_CardNO ={0:d} ", num4);
				}
				string text4 = " ( 1>0 ) ";
				if (text.IndexOf("%") < 0)
				{
					text = string.Format("%{0}%", text);
				}
				if (wgAppConfig.IsAccessDB)
				{
					text4 += string.Format(" AND CSTR(f_CardNO) like {0} ", wgTools.PrepareStrNUnicode(text));
				}
				else
				{
					text4 += string.Format(" AND f_CardNO like {0} ", wgTools.PrepareStrNUnicode(text));
				}
				if (text3.ToUpper().IndexOf("WHERE") > 0)
				{
					text3 += string.Format(" AND {0} ", text4);
				}
				else
				{
					text3 += string.Format(" WHERE {0} ", text4);
				}
				this.reloadUserData(text3);
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000B0B44 File Offset: 0x000AFB44
		private void toolStripMenuItem1_ClickWithOther(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(178) || wgAppConfig.getParamValBoolByNO(168) || wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AllowUploadUserMobile")) == "1" || wgAppConfig.getParamValBoolByNO(197) || wgAppConfig.getParamValBoolByNO(200))
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
					this.deletedUserCnt = 0;
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					string text2 = "";
					long num4 = 0L;
					int num5 = 0;
					this.userControlFind1.txtFindCardID.Text = "";
					this.userControlFind1.txtFindName.Text = "";
					this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text2, ref num4, ref num5);
					string text3 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName  " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID ";
					if (num5 > 0)
					{
						text3 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName  " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM (t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID  WHERE  t_b_Consumer.f_ConsumerID = " + num5.ToString();
					}
					else if (num > 0)
					{
						text3 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " , t_b_Consumer_Other.f_Mobile  , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone ";
						if (num >= num3)
						{
							text3 = text3 + " FROM t_b_Consumer,t_b_Group, t_b_Consumer_Other  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", num2) + " AND t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID ";
						}
						else
						{
							text3 = string.Concat(new string[]
							{
								text3,
								" FROM t_b_Consumer,t_b_Group, t_b_Consumer_Other  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ",
								string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num),
								string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", num3),
								" AND t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID "
							});
						}
						if (text2 != "")
						{
							text3 += string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text2)));
						}
						else if (num4 > 0L)
						{
							text3 += string.Format(" AND f_CardNO ={0:d} ", num4);
						}
					}
					else if (text2 != "")
					{
						text3 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " , t_b_Consumer_Other.f_CertificateID  , t_b_Consumer_Other.f_Telephone  FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text2)));
					}
					else if (num4 > 0L)
					{
						text3 = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName " + this.strPrivilegeType + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  LEFT OUTER JOIN t_b_Consumer_Other ON  t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID " + string.Format(" WHERE f_CardNO ={0:d} ", num4);
					}
					string text4 = " ( 1>0 ) ";
					if (text.IndexOf("%") < 0)
					{
						text = string.Format("%{0}%", text);
					}
					if (wgAppConfig.IsAccessDB)
					{
						text4 += string.Format(" AND CSTR(f_CardNO) like {0} ", wgTools.PrepareStrNUnicode(text));
					}
					else
					{
						text4 += string.Format(" AND f_CardNO like {0} ", wgTools.PrepareStrNUnicode(text));
					}
					if (text3.ToUpper().IndexOf("WHERE") > 0)
					{
						text3 += string.Format(" AND {0} ", text4);
					}
					else
					{
						text3 += string.Format(" WHERE {0} ", text4);
					}
					this.reloadUserData(text3);
				}
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x000B0EAC File Offset: 0x000AFEAC
		private void udpserver_evNewRecord(string info)
		{
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x000B0EB0 File Offset: 0x000AFEB0
		private void updatePrivilegeTypeName(ref DataTable dtUser)
		{
			if (dtUser != null && dtUser.Rows.Count > 0 && this.arrPrivilegeTypeID.Count > 0)
			{
				foreach (object obj in dtUser.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					if ((int)dataRow["f_PrivilegeTypeID"] > 0)
					{
						int num = this.arrPrivilegeTypeID.IndexOf((int)dataRow["f_PrivilegeTypeID"]);
						if (num >= 0)
						{
							dataRow["f_PrivilegeTypeName"] = this.arrPrivilegeTypeName[num];
						}
					}
				}
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x000B0F80 File Offset: 0x000AFF80
		private void waitCloudServerConnect()
		{
			if (wgTools.bUDPCloud != 0 && this.dtStart.AddSeconds(5.0) >= DateTime.Now)
			{
				DateTime dateTime = DateTime.Now.AddSeconds(5.0);
				while (dateTime > DateTime.Now)
				{
					if (wgTools.arrSNIP.Count > 0)
					{
						return;
					}
					Thread.Sleep(300);
				}
			}
		}

		// Token: 0x04000B56 RID: 2902
		private bool bLoadAll;

		// Token: 0x04000B57 RID: 2903
		private bool bLoadedFinished;

		// Token: 0x04000B58 RID: 2904
		private bool bPrivilegTypeDownloadFailFirst;

		// Token: 0x04000B59 RID: 2905
		private int deletedUserCnt;

		// Token: 0x04000B5A RID: 2906
		private dfrmFind dfrmFind1;

		// Token: 0x04000B5B RID: 2907
		private string dgvSql;

		// Token: 0x04000B5C RID: 2908
		private DataSet DS;

		// Token: 0x04000B5F RID: 2911
		private OleDbDataAdapter MyCommand;

		// Token: 0x04000B60 RID: 2912
		private OleDbConnection MyConnection;

		// Token: 0x04000B61 RID: 2913
		private int startRecordIndex;

		// Token: 0x04000B63 RID: 2915
		private TcpClient tcp;

		// Token: 0x04000B64 RID: 2916
		private ArrayList arrPrivilegeTypeID = new ArrayList();

		// Token: 0x04000B65 RID: 2917
		private ArrayList arrPrivilegeTypeName = new ArrayList();

		// Token: 0x04000B69 RID: 2921
		private DataSet dsDefaultStyle = new DataSet("DGV_STILE");

		// Token: 0x04000B6A RID: 2922
		private DataTable dtPrivilege_Of_PrivilegeType = new DataTable();

		// Token: 0x04000B6B RID: 2923
		private DateTime dtStart = DateTime.Now;

		// Token: 0x04000B6C RID: 2924
		private string encodingOfTCP = "GB2312";

		// Token: 0x04000B6D RID: 2925
		private int MaxRecord = 1000;

		// Token: 0x04000B6E RID: 2926
		private string recNOMax = "";

		// Token: 0x04000B6F RID: 2927
		private string strPrivilegeType = ", '' AS f_PrivilegeTypeName, f_PrivilegeTypeID ";
	}
}
