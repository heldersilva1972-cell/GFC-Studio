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

namespace WG3000_COMM.ExtendFunc.Elevator
{
	// Token: 0x0200024D RID: 589
	public partial class frmUsers4Elevator : Form
	{
		// Token: 0x060012A1 RID: 4769 RVA: 0x001688C8 File Offset: 0x001678C8
		public frmUsers4Elevator()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			wgAppConfig.custDataGridview(ref this.dgvFloorPrivileges);
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00168924 File Offset: 0x00167924
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

		// Token: 0x060012A3 RID: 4771 RVA: 0x001689A4 File Offset: 0x001679A4
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
			}
			this.fillDgv(e.Result as DataView);
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00168A5C File Offset: 0x00167A5C
		private void batchUpdateSelectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvUsers.SelectedRows.Count > 0)
			{
				using (dfrmUserFloor dfrmUserFloor = new dfrmUserFloor())
				{
					string text = "";
					string text2 = "";
					for (int i = 0; i < this.dgvUsers.SelectedRows.Count; i++)
					{
						int index = this.dgvUsers.SelectedRows[i].Index;
						int num = int.Parse(this.dgvUsers.Rows[index].Cells[0].Value.ToString());
						if (!string.IsNullOrEmpty(text))
						{
							text += ",";
							text2 += ",";
						}
						text += num.ToString();
						text2 += this.dgvUsers.Rows[index].Cells["f_ConsumerName"].Value.ToString();
					}
					dfrmUserFloor.strSqlSelected = text;
					dfrmUserFloor.strSelectedUsers = text2;
					dfrmUserFloor.Text = string.Format("{0}: [{1}]", sender.ToString(), this.dgvUsers.SelectedRows.Count.ToString());
					if (dfrmUserFloor.ShowDialog(this) == DialogResult.OK)
					{
						this.showUpload();
						this.btnQuery_Click(null, null);
					}
				}
			}
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00168BD8 File Offset: 0x00167BD8
		private void batchUpdateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvUsers.SelectedRows.Count > 0)
			{
				string text = "";
				string text2 = "";
				int num = 0;
				using (dfrmSelectUsers dfrmSelectUsers = new dfrmSelectUsers())
				{
					if (dfrmSelectUsers.ShowDialog() == DialogResult.OK)
					{
						text = dfrmSelectUsers.selectedUserID;
						text2 = dfrmSelectUsers.selectedUserName;
						num = dfrmSelectUsers.selectedUsersNum;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					using (dfrmUserFloor dfrmUserFloor = new dfrmUserFloor())
					{
						dfrmUserFloor.strSqlSelected = text;
						dfrmUserFloor.strSelectedUsers = text2;
						dfrmUserFloor.Text = string.Format("{0}: [{1}]", sender.ToString(), num);
						if (dfrmUserFloor.ShowDialog(this) == DialogResult.OK)
						{
							this.showUpload();
							this.btnQuery_Click(null, null);
						}
					}
				}
			}
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00168CB8 File Offset: 0x00167CB8
		private void btnAutoAdd_Click(object sender, EventArgs e)
		{
			try
			{
				using (dfrmElevatorGroup dfrmElevatorGroup = new dfrmElevatorGroup())
				{
					dfrmElevatorGroup.ShowDialog(this);
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00168D0C File Offset: 0x00167D0C
		private void btnBatchUpdate_Click(object sender, EventArgs e)
		{
			using (dfrmFloors dfrmFloors = new dfrmFloors())
			{
				dfrmFloors.Text = this.btnBatchUpdate.Text;
				if (dfrmFloors.ShowDialog(this) == DialogResult.OK)
				{
					this.reloadUserData("");
				}
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00168D64 File Offset: 0x00167D64
		private void btnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				using (dfrmElevatorGroup dfrmElevatorGroup = new dfrmElevatorGroup())
				{
					dfrmElevatorGroup.ShowDialog(this);
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00168DB8 File Offset: 0x00167DB8
		private void btnEditPrivilege_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvUsers.SelectedRows.Count <= 0)
				{
					if (this.dgvUsers.SelectedCells.Count <= 0)
					{
						XMessageBox.Show(this, CommonStr.strSelectUser, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					num = this.dgvUsers.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvUsers.SelectedRows[0].Index;
				}
				if (this.dgvUsers.Rows.Count > 0)
				{
					try
					{
						this.currentRowIndex = this.dgvUsers.CurrentCell.RowIndex;
					}
					catch
					{
					}
				}
				using (dfrmUserFloor dfrmUserFloor = new dfrmUserFloor())
				{
					dfrmUserFloor.consumerID = int.Parse(this.dgvUsers.Rows[num].Cells[0].Value.ToString());
					dfrmUserFloor.Text = string.Concat(new string[]
					{
						this.dgvUsers.Rows[num].Cells[1].Value.ToString().Trim(),
						".",
						this.dgvUsers.Rows[num].Cells[2].Value.ToString().Trim(),
						" -- ",
						dfrmUserFloor.Text
					});
					if (dfrmUserFloor.ShowDialog(this) == DialogResult.OK)
					{
						this.showUpload();
						this.btnQuery_Click(null, null);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00168FA4 File Offset: 0x00167FA4
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00168FAC File Offset: 0x00167FAC
		private void btnExport_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcelSpecial(ref this.dgvUsers, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00168FDE File Offset: 0x00167FDE
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvUsers);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00168FF2 File Offset: 0x00167FF2
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvUsers, this.Text);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00169008 File Offset: 0x00168008
		private void btnQuery_Click(object sender, EventArgs e)
		{
			this.loadUserFloor();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			long num4 = 0L;
			int num5 = 0;
			this.userControlFind1.getSqlInfo(ref num, ref num2, ref num3, ref text, ref num4, ref num5);
			string text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName, ' ' as f_FloorNameDesc,' ' as f_ControlSegID,' ' as f_MoreFloorNum, ' ' as f_FloorID  ";
			text2 += " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ";
			if (num5 > 0)
			{
				text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName, ' ' as f_FloorNameDesc,' ' as f_ControlSegID,' ' as f_MoreFloorNum, ' ' as f_FloorID  ";
				text2 = text2 + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  WHERE  t_b_Consumer.f_ConsumerID = " + num5.ToString();
			}
			else if (num > 0)
			{
				text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName, ' ' as f_FloorNameDesc,' ' as f_ControlSegID,' ' as f_MoreFloorNum, ' ' as f_FloorID  ";
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
				text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName, ' ' as f_FloorNameDesc,' ' as f_ControlSegID,' ' as f_MoreFloorNum, ' ' as f_FloorID  ";
				text2 = text2 + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", text)));
			}
			else if (num4 > 0L)
			{
				text2 = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName, ' ' as f_FloorNameDesc,' ' as f_ControlSegID,' ' as f_MoreFloorNum, ' ' as f_FloorID  ";
				text2 = text2 + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID " + string.Format(" WHERE f_CardNO ={0:d} ", num4);
			}
			this.reloadUserData(text2);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x001691C8 File Offset: 0x001681C8
		private void btnUpload_Click(object sender, EventArgs e)
		{
			this.bLoadConsole = true;
			base.Close();
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x001691D8 File Offset: 0x001681D8
		private void dgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvUsers.Columns.Count && this.dgvUsers.Columns[e.ColumnIndex].Name.Equals("f_FloorNameDesc"))
			{
				string text;
				if ((text = e.Value as string) != null && !(text == " "))
				{
					return;
				}
				DataGridViewCell dataGridViewCell = this.dgvUsers[e.ColumnIndex, e.RowIndex];
				string text2 = this.dgvUsers[0, e.RowIndex].Value.ToString();
				if (this.dvUserFloor == null || string.IsNullOrEmpty(text2))
				{
					e.Value = "";
					dataGridViewCell.Value = "";
					return;
				}
				this.dvUserFloor.RowFilter = "f_ConsumerID = " + text2;
				if (this.dvUserFloor.Count == 0)
				{
					e.Value = "";
					dataGridViewCell.Value = "";
					return;
				}
				if (this.dvUserFloor.Count == 1)
				{
					e.Value = this.dvUserFloor[0]["f_floorName"];
					dataGridViewCell.Value = this.dvUserFloor[0]["f_floorName"];
					this.dgvUsers[e.ColumnIndex + 1, e.RowIndex].Value = this.dvUserFloor[0]["f_ControlSegID"];
					return;
				}
				if (this.dvUserFloor.Count > 1)
				{
					if (this.batchUpdateSelectToolStripMenuItem.Enabled)
					{
						e.Value = CommonStr.strElevatorMoreFloors + string.Format("({0})", this.dvUserFloor.Count.ToString());
						dataGridViewCell.Value = CommonStr.strElevatorMoreFloors + string.Format("({0})", this.dvUserFloor.Count.ToString());
						this.dgvUsers[e.ColumnIndex + 1, e.RowIndex].Value = this.dvUserFloor[0]["f_ControlSegID"];
						return;
					}
					e.Value = CommonStr.strElevatorMoreFloors;
					dataGridViewCell.Value = CommonStr.strElevatorMoreFloors;
					this.dgvUsers[e.ColumnIndex + 1, e.RowIndex].Value = this.dvUserFloor[0]["f_ControlSegID"];
					return;
				}
				else
				{
					e.Value = "";
					dataGridViewCell.Value = "";
				}
			}
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00169482 File Offset: 0x00168482
		private void dgvUsers_DoubleClick(object sender, EventArgs e)
		{
			this.btnEditPrivilege.PerformClick();
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00169490 File Offset: 0x00168490
		private void dgvUsers_Scroll(object sender, ScrollEventArgs e)
		{
			if (!this.bLoadedFinished && e.ScrollOrientation == ScrollOrientation.VerticalScroll)
			{
				wgTools.WriteLine(e.OldValue.ToString());
				wgTools.WriteLine(e.NewValue.ToString());
				DataGridView dataGridView = this.dgvUsers;
				if (e.NewValue > e.OldValue && (e.NewValue + 100 > dataGridView.Rows.Count || e.NewValue + dataGridView.Rows.Count / 10 > dataGridView.Rows.Count))
				{
					if (this.startRecordIndex <= dataGridView.Rows.Count)
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

		// Token: 0x060012B3 RID: 4787 RVA: 0x001695C4 File Offset: 0x001685C4
		private void exportAllPrivilegeToExcelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			try
			{
				string text = "SELECT t_b_Consumer.f_ConsumerID,t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, t_b_Group.f_GroupName, t_b_Door.f_DoorName+'.'+t_b_floor.f_floorName AS f_floorNameFull,t_b_UserFloor.f_ControlSegId, t_b_Door.f_DoorName, t_b_floor.f_floorName\r\n                FROM (((t_b_UserFloor INNER JOIN t_b_Floor ON t_b_UserFloor.f_floorID = t_b_Floor.f_floorID) LEFT JOIN (t_b_Controller RIGHT JOIN t_b_Door ON t_b_Controller.f_ControllerID = t_b_Door.f_ControllerID) ON t_b_Floor.f_DoorID = t_b_Door.f_DoorID) INNER JOIN t_b_Consumer ON t_b_UserFloor.f_ConsumerID = t_b_Consumer.f_ConsumerID) LEFT JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID order by t_b_Consumer.f_ConsumerNO";
				DataTable dataTable = new DataTable();
				DataView dataView = new DataView(dataTable);
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
						goto IL_00E1;
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
				IL_00E1:
				DataGridView dataGridView = this.dgvFloorPrivileges;
				bool paramValBoolByNO = wgAppConfig.getParamValBoolByNO(121);
				this.dgvFloorPrivileges.AutoGenerateColumns = false;
				this.dgvFloorPrivileges.DataSource = dataView;
				int num = 0;
				while (num < dataView.Table.Columns.Count && num < dataGridView.ColumnCount)
				{
					dataGridView.Columns[num].DataPropertyName = dataView.Table.Columns[num].ColumnName;
					dataGridView.Columns[num].Name = dataView.Table.Columns[num].ColumnName;
					num++;
				}
				dataGridView.Columns["f_ControlSegId"].Visible = paramValBoolByNO;
				wgAppConfig.exportToExcelSpecial(ref this.dgvFloorPrivileges, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
				this.dgvFloorPrivileges.DataSource = null;
				dataView.Dispose();
				dataTable.Dispose();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00169874 File Offset: 0x00168874
		private void fillDgv(DataView dvUser4Elevator)
		{
			try
			{
				DataGridView dataGridView = this.dgvUsers;
				if (dataGridView.DataSource == null)
				{
					dataGridView.DataSource = dvUser4Elevator;
					for (int i = 0; i < dvUser4Elevator.Table.Columns.Count; i++)
					{
						dataGridView.Columns[i].DataPropertyName = dvUser4Elevator.Table.Columns[i].ColumnName;
						dataGridView.Columns[i].Name = dvUser4Elevator.Table.Columns[i].ColumnName;
					}
					wgAppConfig.setDisplayFormatDate(dataGridView, "f_BeginYMD", wgTools.DisplayFormat_DateYMD);
					wgAppConfig.setDisplayFormatDate(dataGridView, "f_EndYMD", wgTools.DisplayFormat_DateYMD);
					wgAppConfig.ReadGVStyle(this, dataGridView);
					if (this.startRecordIndex == 0 && dvUser4Elevator.Count >= this.MaxRecord)
					{
						this.startRecordIndex += this.MaxRecord;
						this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
					}
				}
				else if (dvUser4Elevator.Count > 0)
				{
					int firstDisplayedScrollingRowIndex = dataGridView.FirstDisplayedScrollingRowIndex;
					DataView dataView = dataGridView.DataSource as DataView;
					dataView.Table.Merge(dvUser4Elevator.Table);
					if (firstDisplayedScrollingRowIndex >= 0)
					{
						dataGridView.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
					}
				}
				if (this.dgvUsers.RowCount > 0 && this.dgvUsers.RowCount > this.currentRowIndex)
				{
					this.dgvUsers.CurrentCell = this.dgvUsers[1, this.currentRowIndex];
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00169A4C File Offset: 0x00168A4C
		private void frmUsers_FormClosed(object sender, FormClosedEventArgs e)
		{
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00169A4E File Offset: 0x00168A4E
		private void frmUsers_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00169A50 File Offset: 0x00168A50
		private void frmUsers_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
				{
					dfrmInputNewName.setPasswordChar('*');
					if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || dfrmInputNewName.strNewName != "5678")
					{
						return;
					}
					this.funcCtrlShiftQ();
				}
			}
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

		// Token: 0x060012B8 RID: 4792 RVA: 0x00169B40 File Offset: 0x00168B40
		private void frmUsers_Load(object sender, EventArgs e)
		{
			if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(144)) & 255) == 2)
			{
				this.btnEditPrivilege.Text = CommonStr.strFloorPrivilege2;
				this.btnBatchUpdate.Text = CommonStr.strFloorConfigure2;
			}
			else if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(144)) & 255) == 3)
			{
				this.btnEditPrivilege.Text = CommonStr.strFloorPrivilege3;
				this.btnBatchUpdate.Text = CommonStr.strFloorConfigure3;
			}
			this.ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.ConsumerNO.HeaderText);
			Icon icon = base.Icon;
			wgAppConfig.GetAppIcon(ref icon);
			base.Icon = icon;
			this.Deptname.HeaderText = wgAppConfig.ReplaceFloorRoom(this.Deptname.HeaderText);
			this.loadOperatorPrivilege();
			this.loadUserFloor();
			this.userControlFind1.btnQuery.Click += this.btnQuery_Click;
			this.saveDefaultStyle();
			this.loadStyle();
			Cursor.Current = Cursors.WaitCursor;
			this.btnAutoAdd.Visible = wgAppConfig.GetKeyVal("ElevatorGroupVisible") == "1";
			this.dgvUsers.ContextMenuStrip = this.contextMenuStrip1;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			this.userControlFind1.btnQuery.PerformClick();
			icControllerZone icControllerZone = new icControllerZone();
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			icControllerZone.getZone(ref arrayList, ref arrayList2, ref arrayList3);
			if (arrayList2.Count > 0 && (int)arrayList2[0] != 0)
			{
				this.batchUpdateSelectToolStripMenuItem.Enabled = false;
			}
			this.Department2.HeaderText = wgAppConfig.ReplaceFloorRoom(this.Department2.HeaderText);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00169D16 File Offset: 0x00168D16
		private void frmUsers4Elevator_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00169D2B File Offset: 0x00168D2B
		private void funcCtrlShiftQ()
		{
			this.btnAutoAdd.Visible = true;
			wgAppConfig.UpdateKeyVal("ElevatorGroupVisible", "1");
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00169D48 File Offset: 0x00168D48
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

		// Token: 0x060012BC RID: 4796 RVA: 0x00169E94 File Offset: 0x00168E94
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuConsumers";
			if (icOperator.OperatePrivilegeVisible(text, ref flag))
			{
				if (flag)
				{
					this.btnAutoAdd.Visible = false;
					this.btnBatchUpdate.Visible = false;
					return;
				}
			}
			else
			{
				base.Close();
			}
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00169ED8 File Offset: 0x00168ED8
		private void loadStyle()
		{
			this.dgvUsers.AutoGenerateColumns = false;
			bool paramValBoolByNO = wgAppConfig.getParamValBoolByNO(121);
			this.dgvUsers.Columns[11].Visible = paramValBoolByNO;
			wgAppConfig.ReadGVStyle(this, this.dgvUsers);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00169F20 File Offset: 0x00168F20
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
			this.tb = new DataTable("users");
			this.dv = new DataView(this.tb);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.tb);
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
						sqlDataAdapter.Fill(this.tb);
					}
				}
			}
			IL_0187:
			if (this.tb.Rows.Count > 0)
			{
				this.recNOMax = this.tb.Rows[this.tb.Rows.Count - 1]["f_ConsumerNO"].ToString();
			}
			wgTools.WriteLine("loadUserData End");
			return this.dv;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0016A15C File Offset: 0x0016915C
		private void loadUserFloor()
		{
			string text = " SELECT  t_b_UserFloor .*,  t_b_Door.f_DoorName + '.' +  t_b_floor.f_floorName as f_floorName  FROM (t_b_UserFloor INNER JOIN t_b_Floor ON t_b_UserFloor.f_floorID = t_b_Floor.f_floorID) LEFT JOIN (t_b_Controller RIGHT JOIN t_b_Door ON t_b_Controller.f_ControllerID = t_b_Door.f_ControllerID) ON t_b_Floor.f_DoorID = t_b_Door.f_DoorID ";
			this.dtUserFloor = new DataTable();
			this.dvUserFloor = new DataView(this.dtUserFloor);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtUserFloor);
						}
					}
					return;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtUserFloor);
					}
				}
			}
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0016A280 File Offset: 0x00169280
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
				this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
			}
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0016A306 File Offset: 0x00169306
		private void restoreDefaultLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.RestoreGVStyle(this, this.dgvUsers);
			this.loadDefaultStyle();
			this.loadStyle();
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0016A320 File Offset: 0x00169320
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

		// Token: 0x060012C3 RID: 4803 RVA: 0x0016A455 File Offset: 0x00169455
		private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.SaveDGVStyle(this, this.dgvUsers);
			XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0016A47E File Offset: 0x0016947E
		private void showUpload()
		{
			if (this.firstShow)
			{
				this.firstShow = false;
				XMessageBox.Show(CommonStr.strNeedUploadFloor);
			}
		}

		// Token: 0x04002221 RID: 8737
		private bool bLoadedFinished;

		// Token: 0x04002222 RID: 8738
		private int currentRowIndex;

		// Token: 0x04002223 RID: 8739
		private dfrmFind dfrmFind1;

		// Token: 0x04002224 RID: 8740
		private string dgvSql;

		// Token: 0x04002225 RID: 8741
		private DataTable dtUserFloor;

		// Token: 0x04002226 RID: 8742
		private DataView dv;

		// Token: 0x04002227 RID: 8743
		private DataView dvUserFloor;

		// Token: 0x04002228 RID: 8744
		private int startRecordIndex;

		// Token: 0x04002229 RID: 8745
		private DataTable tb;

		// Token: 0x0400222A RID: 8746
		public bool bLoadConsole;

		// Token: 0x0400222B RID: 8747
		private DataSet dsDefaultStyle = new DataSet("DGV_STILE");

		// Token: 0x0400222C RID: 8748
		private bool firstShow = true;

		// Token: 0x0400222D RID: 8749
		private int MaxRecord = 1000;

		// Token: 0x0400222E RID: 8750
		private string recNOMax = "";
	}
}
