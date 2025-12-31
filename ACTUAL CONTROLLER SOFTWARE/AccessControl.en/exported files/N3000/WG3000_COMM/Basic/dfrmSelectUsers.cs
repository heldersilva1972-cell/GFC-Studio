using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200002A RID: 42
	public partial class dfrmSelectUsers : frmN3000
	{
		// Token: 0x060002FF RID: 767 RVA: 0x0005B0C8 File Offset: 0x0005A0C8
		public dfrmSelectUsers()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0005B168 File Offset: 0x0005A168
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

		// Token: 0x06000301 RID: 769 RVA: 0x0005B1AC File Offset: 0x0005A1AC
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

		// Token: 0x06000302 RID: 770 RVA: 0x0005B22C File Offset: 0x0005A22C
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			if (this.strGroupFilter == "")
			{
				icConsumerShare.selectAllUsers();
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
				int rowCount = this.dgvSelectedUsers.RowCount;
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
				string rowFilter = this.dv1.RowFilter;
				string rowFilter2 = this.dv2.RowFilter;
				this.dv1.Dispose();
				this.dv2.Dispose();
				this.dv1 = null;
				this.dv2 = null;
				this.dt.BeginLoadData();
				this.dv = new DataView(this.dt);
				this.dv.RowFilter = this.strGroupFilter;
				for (int i = 0; i < this.dv.Count; i++)
				{
					this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
				}
				this.dt.EndLoadData();
				this.dv1 = new DataView(this.dt);
				this.dv1.RowFilter = rowFilter;
				this.dv2 = new DataView(this.dt);
				this.dv2.RowFilter = rowFilter2;
				this.dgvUsers.DataSource = this.dv1;
				this.dgvSelectedUsers.DataSource = this.dv2;
				wgTools.WriteLine("btnAddAllUsers_Click End");
				int count = this.dv2.Count;
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0005B453 File Offset: 0x0005A453
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
			int rowCount = this.dgvSelectedUsers.RowCount;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0005B474 File Offset: 0x0005A474
		private void btnAddPass_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.RowCount > 0)
			{
				string text = "";
				string text2 = "";
				int i = 0;
				while (i < this.dgvSelectedUsers.Rows.Count)
				{
					text = "";
					while (i < this.dgvSelectedUsers.Rows.Count)
					{
						if (!string.IsNullOrEmpty(text))
						{
							text += ",";
							text2 += ",";
						}
						text += ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerID"];
						text2 += ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerName"];
						i++;
					}
				}
				this.selectedUserID = text;
				this.selectedUserName = text2;
				this.selectedUsersNum = i;
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0005B570 File Offset: 0x0005A570
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnDelAllUsers_Click Start");
				icConsumerShare.selectNoneUsers();
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
					((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
					return;
				}
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0005B650 File Offset: 0x0005A650
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0005B662 File Offset: 0x0005A662
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0005B671 File Offset: 0x0005A671
		private void btnFind_Click(object sender, EventArgs e)
		{
			if (this.defaultFindControl == null)
			{
				this.defaultFindControl = this.dgvUsers;
			}
			wgAppConfig.findCall(this.dfrmFind1, this, this.defaultFindControl);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0005B699 File Offset: 0x0005A699
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0005B6A8 File Offset: 0x0005A6A8
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
					dataView.RowFilter = "f_Selected = 0 AND f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
					int groupChildMaxNo = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
					if (num2 > 0)
					{
						if (num2 >= groupChildMaxNo)
						{
							dataView.RowFilter = string.Format("f_Selected = 0 AND f_GroupID ={0:d} ", num);
							this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num);
						}
						else
						{
							dataView.RowFilter = "f_Selected = 0 ";
							string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
							dataView.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", groupQuery);
							this.strGroupFilter = string.Format("  {0} ", groupQuery);
						}
					}
					dataView.RowFilter = string.Format("(f_DoorEnabled > 0) AND {0} AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				}
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				}
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0005B8F8 File Offset: 0x0005A8F8
		private void dfrmPrivilegeCopy_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.dfrmWait1 != null)
				{
					this.dfrmWait1.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0005B930 File Offset: 0x0005A930
		private void dfrmPrivilegeCopy_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0005B948 File Offset: 0x0005A948
		private void dfrmPrivilegeCopy_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x0600030E RID: 782 RVA: 0x0005B9A4 File Offset: 0x0005A9A4
		private void dfrmPrivilegeCopy_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			try
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
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			if (!this.backgroundWorker1.IsBusy)
			{
				this.backgroundWorker1.RunWorkerAsync();
			}
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0005BB48 File Offset: 0x0005AB48
		private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0005BB58 File Offset: 0x0005AB58
		private void dgvUsers_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0005BB88 File Offset: 0x0005AB88
		private void dgvUsers_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrmPrivilegeCopy_KeyDown(this.dgvUsers, e);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0005BB97 File Offset: 0x0005AB97
		private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneUser.PerformClick();
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0005BBA4 File Offset: 0x0005ABA4
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0005BBC4 File Offset: 0x0005ABC4
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			this.dv.RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
			this.dvSelected.RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
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
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0005BCF0 File Offset: 0x0005ACF0
		private void logOperate(object sender)
		{
			string text = "";
			for (int i = 0; i <= Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedUsers.RowCount) - 1; i++)
			{
				text = text + ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerName"] + ",";
			}
			if (this.dgvSelectedUsers.RowCount > wgAppConfig.LogEventMaxCount)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"......(",
					this.dgvSelectedUsers.RowCount,
					")"
				});
				return;
			}
			object obj2 = text;
			text = string.Concat(new object[]
			{
				obj2,
				"(",
				this.dgvSelectedUsers.RowCount,
				")"
			});
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0005BDE0 File Offset: 0x0005ADE0
		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				if (!this.bStarting)
				{
					if (this.progressBar1.Value != 0 && this.progressBar1.Value != this.progressBar1.Maximum)
					{
						Cursor.Current = Cursors.WaitCursor;
					}
				}
				else if (this.dgvUsers.DataSource == null)
				{
					Cursor.Current = Cursors.WaitCursor;
				}
				else
				{
					this.timer1.Enabled = false;
					Cursor.Current = Cursors.Default;
					this.lblWait.Visible = false;
					this.btnAddAllUsers.Enabled = true;
					this.btnAddOneUser.Enabled = true;
					this.btnAddPass.Enabled = true;
					this.btnDelAllUsers.Enabled = true;
					this.btnDelOneUser.Enabled = true;
					this.cbof_GroupID.Enabled = true;
					this.bStarting = false;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x040005D5 RID: 1493
		private Control defaultFindControl;

		// Token: 0x040005D6 RID: 1494
		private DataTable dt;

		// Token: 0x040005D7 RID: 1495
		private DataView dv;

		// Token: 0x040005D8 RID: 1496
		private DataView dv1;

		// Token: 0x040005D9 RID: 1497
		private DataView dv2;

		// Token: 0x040005DA RID: 1498
		private DataView dvSelected;

		// Token: 0x040005DB RID: 1499
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x040005DC RID: 1500
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x040005DD RID: 1501
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x040005DE RID: 1502
		private bool bStarting = true;

		// Token: 0x040005E0 RID: 1504
		public string selectedUserID = "";

		// Token: 0x040005E1 RID: 1505
		public string selectedUserName = "";

		// Token: 0x040005E2 RID: 1506
		public int selectedUsersNum;

		// Token: 0x040005E3 RID: 1507
		private string strGroupFilter = "";
	}
}
