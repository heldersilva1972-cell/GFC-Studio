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

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000241 RID: 577
	public partial class dfrmDeletedUsersManage : frmN3000
	{
		// Token: 0x060011AA RID: 4522 RVA: 0x0014B620 File Offset: 0x0014A620
		public dfrmDeletedUsersManage()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0014B694 File Offset: 0x0014A694
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			e.Result = this.loadUserData4BackWork();
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0014B6D8 File Offset: 0x0014A6D8
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
			this.loadUserData4BackWorkComplete(e.Result as DataTable);
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString());
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0014B758 File Offset: 0x0014A758
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			if (this.strGroupFilter == "")
			{
				icConsumerShare.selectAllUsers();
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter());
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getSelectedRowfilter());
				return;
			}
			wgTools.WriteLine("btnAddAllUsers_Click Start");
			this.dt = ((DataView)this.dgvUsers.DataSource).Table;
			this.dv1 = (DataView)this.dgvUsers.DataSource;
			this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
			this.dgvUsers.DataSource = null;
			this.dgvSelectedUsers.DataSource = null;
			if (this.strGroupFilter != "")
			{
				this.dv = new DataView(this.dt);
				this.dv.RowFilter = this.strGroupFilter;
				for (int i = 0; i < this.dv.Count; i++)
				{
					this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
				}
			}
			this.dgvUsers.DataSource = this.dv1;
			this.dgvSelectedUsers.DataSource = this.dv2;
			wgTools.WriteLine("btnAddAllUsers_Click End");
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0014B8D8 File Offset: 0x0014A8D8
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0014B8EC File Offset: 0x0014A8EC
		private void btnClearAll_Click(object sender, EventArgs e)
		{
			string text = this.btnClearAll.Text;
			if (XMessageBox.Show(text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				try
				{
					DataGridView dataGridView = this.dgvUsers;
					icConsumer icConsumer = new icConsumer();
					for (int i = 0; i < dataGridView.Rows.Count; i++)
					{
						icConsumer.dimissionUserClear((int)dataGridView.Rows[i].Cells[0].Value);
					}
					XMessageBox.Show(text + " " + CommonStr.strSuccessfully);
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
				catch (Exception ex)
				{
					wgTools.WriteLine(ex.ToString());
				}
			}
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0014B9B4 File Offset: 0x0014A9B4
		private void btnClearSelected_Click(object sender, EventArgs e)
		{
			string text = this.btnClearSelected.Text;
			if (XMessageBox.Show(text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				try
				{
					DataGridView dataGridView = this.dgvUsers;
					int num;
					if (dataGridView.SelectedRows.Count <= 0)
					{
						if (dataGridView.SelectedCells.Count <= 0)
						{
							return;
						}
						num = dataGridView.SelectedCells[0].RowIndex;
					}
					else
					{
						num = dataGridView.SelectedRows[0].Index;
					}
					this.selectedUsers = dataGridView.Rows[num].Cells[0].Value.ToString();
					icConsumer icConsumer = new icConsumer();
					if (icConsumer.dimissionUserClear((int)dataGridView.Rows[num].Cells[0].Value) != 1)
					{
						XMessageBox.Show(this, text + " " + CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						XMessageBox.Show(text + " " + CommonStr.strSuccessfully);
						base.DialogResult = DialogResult.OK;
						base.Close();
					}
				}
				catch (Exception ex)
				{
					wgTools.WriteLine(ex.ToString());
				}
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0014BAF4 File Offset: 0x0014AAF4
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0014BB04 File Offset: 0x0014AB04
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnDelAllUsers_Click Start");
				icConsumerShare.selectNoneUsers();
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter());
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				}
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getSelectedRowfilter());
			}
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x0014BBC1 File Offset: 0x0014ABC1
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0014BBD3 File Offset: 0x0014ABD3
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvUsers);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0014BBE8 File Offset: 0x0014ABE8
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (this.btnOK.Enabled && this.btnOK.Visible)
			{
				try
				{
					DataGridView dataGridView = this.dgvUsers;
					int num;
					if (dataGridView.SelectedRows.Count <= 0)
					{
						if (dataGridView.SelectedCells.Count <= 0)
						{
							return;
						}
						num = dataGridView.SelectedCells[0].RowIndex;
					}
					else
					{
						num = dataGridView.SelectedRows[0].Index;
					}
					this.selectedUsers = dataGridView.Rows[num].Cells[0].Value.ToString();
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
				catch (Exception ex)
				{
					wgTools.WriteLine(ex.ToString());
				}
			}
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0014BCB4 File Offset: 0x0014ACB4
		private void btnRestoreSelected_Click(object sender, EventArgs e)
		{
			string text = this.btnRestoreSelected.Text;
			try
			{
				DataGridView dataGridView = this.dgvUsers;
				int num;
				if (dataGridView.SelectedRows.Count <= 0)
				{
					if (dataGridView.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dataGridView.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dataGridView.SelectedRows[0].Index;
				}
				this.selectedUsers = dataGridView.Rows[num].Cells[0].Value.ToString();
				icConsumer icConsumer = new icConsumer();
				if (icConsumer.dimissionUserRestore((int)dataGridView.Rows[num].Cells[0].Value) != 1)
				{
					XMessageBox.Show(this, text + " " + CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					XMessageBox.Show(text + " " + CommonStr.strSuccessfully);
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0014BDD8 File Offset: 0x0014ADD8
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0014BDE8 File Offset: 0x0014ADE8
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
			if (this.dgvUsers.DataSource != null)
			{
				DataView dataView = (DataView)this.dgvUsers.DataSource;
				if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					dataView.RowFilter = icConsumerShare.getOptionalRowfilter();
					this.strGroupFilter = "";
				}
				else
				{
					this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
					int groupChildMaxNo = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
					if (num2 > 0)
					{
						if (num2 >= groupChildMaxNo)
						{
							this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num);
						}
						else
						{
							string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
							this.strGroupFilter = string.Format("  {0} ", groupQuery);
						}
					}
					dataView.RowFilter = string.Format("{0} AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				}
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter());
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				}
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format(" {0}", icConsumerShare.getSelectedRowfilter());
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0014BFDF File Offset: 0x0014AFDF
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0014BFF4 File Offset: 0x0014AFF4
		private void dfrm_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x060011BB RID: 4539 RVA: 0x0014C064 File Offset: 0x0014B064
		private void dfrmUserSelected_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.btnOK.Enabled = false;
			this.btnClearAll.Enabled = false;
			this.btnClearSelected.Enabled = false;
			this.btnRestoreSelected.Enabled = false;
			if (this.operateMode.Equals("MANAGE"))
			{
				this.btnOK.Visible = false;
			}
			else
			{
				this.btnClearAll.Visible = false;
				this.btnClearSelected.Visible = false;
				this.btnRestoreSelected.Visible = false;
			}
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.loadGroupData();
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0014C1A4 File Offset: 0x0014B1A4
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

		// Token: 0x060011BD RID: 4541 RVA: 0x0014C258 File Offset: 0x0014B258
		private void loadUserData()
		{
			wgTools.WriteLine("loadUserData Start");
			string text = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID ";
			text += " FROM t_b_Consumer_delete WHERE f_DoorEnabled > 0 ORDER BY f_ConsumerNO ASC ";
			this.dtUser = new DataTable();
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtUser);
						}
					}
					goto IL_00D0;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtUser);
					}
				}
			}
			IL_00D0:
			wgTools.WriteLine("da.Fill End");
			try
			{
				this.dtUser.PrimaryKey = new DataColumn[] { this.dtUser.Columns[0] };
			}
			catch (Exception)
			{
				throw;
			}
			this.dv = new DataView(this.dtUser);
			this.dvSelected = new DataView(this.dtUser);
			this.dv.RowFilter = "f_Selected = 0";
			this.dvSelected.RowFilter = "f_Selected > 0";
			this.dgvUsers.AutoGenerateColumns = false;
			this.dgvUsers.DataSource = this.dv;
			this.dgvSelectedUsers.AutoGenerateColumns = false;
			this.dgvSelectedUsers.DataSource = this.dvSelected;
			for (int i = 0; i < this.dv.Table.Columns.Count; i++)
			{
				this.dgvUsers.Columns[i].DataPropertyName = this.dtUser.Columns[i].ColumnName;
				this.dgvSelectedUsers.Columns[i].DataPropertyName = this.dtUser.Columns[i].ColumnName;
			}
			wgTools.WriteLine("loadUserData End");
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0014C4CC File Offset: 0x0014B4CC
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			return this.loadUserDataDeleted();
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0014C4E8 File Offset: 0x0014B4E8
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			if (!string.IsNullOrEmpty(this.selectedUsers))
			{
				this.dv.RowFilter = string.Format(" f_ConsumerID IN ({0}) ", this.selectedUsers);
				for (int i = 0; i < this.dv.Count; i++)
				{
					this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
				}
				this.selectedUsers = "";
			}
			this.dv.RowFilter = icConsumerShare.getOptionalRowfilter();
			this.dvSelected.RowFilter = icConsumerShare.getSelectedRowfilter();
			this.dgvUsers.AutoGenerateColumns = false;
			this.dgvUsers.DataSource = this.dv;
			this.dgvSelectedUsers.AutoGenerateColumns = false;
			this.dgvSelectedUsers.DataSource = this.dvSelected;
			int num = 0;
			while (num < this.dv.Table.Columns.Count && num < this.dgvUsers.ColumnCount)
			{
				this.dgvUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
				this.dgvSelectedUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
				num++;
			}
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			wgTools.WriteLine("loadUserData End");
			if (this.dv.Count > 0)
			{
				this.btnOK.Enabled = true;
				this.btnClearAll.Enabled = true;
				this.btnClearSelected.Enabled = true;
				this.btnRestoreSelected.Enabled = true;
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0014C6A8 File Offset: 0x0014B6A8
		private DataTable loadUserDataDeleted()
		{
			wgTools.WriteLine("loadUserData Start");
			Thread.Sleep(100);
			int num = 0;
			string text = string.Format(" SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, {0:d} as f_Selected, f_GroupID, f_DoorEnabled  ", num) + " FROM t_b_Consumer_delete  ORDER BY f_ConsumerNO ASC ";
			DataTable dataTable = new DataTable();
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							oleDbDataAdapter.Fill(dataTable);
						}
					}
					goto IL_00F9;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						sqlDataAdapter.Fill(dataTable);
					}
				}
			}
			IL_00F9:
			wgTools.WriteLine("da.Fill End");
			return dataTable;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0014C808 File Offset: 0x0014B808
		private void restoreAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = this.restoreAllToolStripMenuItem.Text;
			try
			{
				DataGridView dataGridView = this.dgvUsers;
				for (int i = 0; i < dataGridView.Rows.Count; i++)
				{
					this.selectedUsers = dataGridView.Rows[i].Cells[0].Value.ToString();
					icConsumer icConsumer = new icConsumer();
					if (icConsumer.dimissionUserRestore((int)dataGridView.Rows[i].Cells[0].Value) != 1)
					{
						XMessageBox.Show(this, text + " " + CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				icConsumerShare.setUpdateLog();
				XMessageBox.Show(text + " " + CommonStr.strSuccessfully);
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0014C904 File Offset: 0x0014B904
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.dgvUsers.DataSource == null)
			{
				Cursor.Current = Cursors.WaitCursor;
				return;
			}
			this.timer1.Enabled = false;
			Cursor.Current = Cursors.Default;
			this.lblWait.Visible = false;
			this.groupBox1.Enabled = true;
		}

		// Token: 0x04001FBB RID: 8123
		private DataTable dt;

		// Token: 0x04001FBC RID: 8124
		private DataTable dtUser;

		// Token: 0x04001FBD RID: 8125
		private DataView dv;

		// Token: 0x04001FBE RID: 8126
		private DataView dv1;

		// Token: 0x04001FBF RID: 8127
		private DataView dv2;

		// Token: 0x04001FC0 RID: 8128
		private DataView dvSelected;

		// Token: 0x04001FC1 RID: 8129
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04001FC2 RID: 8130
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04001FC3 RID: 8131
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04001FC4 RID: 8132
		public string operateMode = "SELECT";

		// Token: 0x04001FC5 RID: 8133
		public string selectedUsers = "";

		// Token: 0x04001FC6 RID: 8134
		private string strGroupFilter = "";
	}
}
