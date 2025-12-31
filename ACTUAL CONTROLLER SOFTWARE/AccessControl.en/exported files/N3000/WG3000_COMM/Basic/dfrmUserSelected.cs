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
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200003B RID: 59
	public partial class dfrmUserSelected : frmN3000
	{
		// Token: 0x0600040A RID: 1034 RVA: 0x00072388 File Offset: 0x00071388
		public dfrmUserSelected()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000723F0 File Offset: 0x000713F0
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

		// Token: 0x0600040C RID: 1036 RVA: 0x00072434 File Offset: 0x00071434
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

		// Token: 0x0600040D RID: 1037 RVA: 0x000724B4 File Offset: 0x000714B4
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			if (this.strGroupFilter == "")
			{
				icConsumerShare.selectAllUsers();
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter(this.bFilterNotAttend));
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getSelectedRowfilter(this.bFilterNotAttend));
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

		// Token: 0x0600040E RID: 1038 RVA: 0x00072640 File Offset: 0x00071640
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00072652 File Offset: 0x00071652
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00072664 File Offset: 0x00071664
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnDelAllUsers_Click Start");
				icConsumerShare.selectNoneUsers();
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter(this.bFilterNotAttend));
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(this.bFilterNotAttend), this.strGroupFilter);
				}
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getSelectedRowfilter(this.bFilterNotAttend));
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00072733 File Offset: 0x00071733
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00072745 File Offset: 0x00071745
		private void btnFind_Click(object sender, EventArgs e)
		{
			if (this.defaultFindControl == null)
			{
				this.defaultFindControl = this.dgvUsers;
			}
			wgAppConfig.findCall(this.dfrmFind1, this, this.defaultFindControl);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00072770 File Offset: 0x00071770
		private void btnOK_Click(object sender, EventArgs e)
		{
			string text = "";
			for (int i = 0; i < this.dgvSelectedUsers.Rows.Count; i++)
			{
				text = text + ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerID"] + ",";
			}
			text += "0";
			this.selectedUsers = text;
			base.DialogResult = DialogResult.OK;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x000727EA File Offset: 0x000717EA
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000727F8 File Offset: 0x000717F8
		private void cbof_GroupID_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00072828 File Offset: 0x00071828
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
			if (this.dgvUsers.DataSource != null)
			{
				DataView dataView = (DataView)this.dgvUsers.DataSource;
				if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					dataView.RowFilter = icConsumerShare.getOptionalRowfilter(this.bFilterNotAttend);
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
					dataView.RowFilter = string.Format("{0} AND ({1})", icConsumerShare.getOptionalRowfilter(this.bFilterNotAttend), this.strGroupFilter);
				}
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter(this.bFilterNotAttend));
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(this.bFilterNotAttend), this.strGroupFilter);
				}
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format(" {0}", icConsumerShare.getSelectedRowfilter(this.bFilterNotAttend));
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00072A3D File Offset: 0x00071A3D
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00072A54 File Offset: 0x00071A54
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

		// Token: 0x06000419 RID: 1049 RVA: 0x00072AC4 File Offset: 0x00071AC4
		private void dfrmUserSelected_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
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

		// Token: 0x0600041A RID: 1050 RVA: 0x00072B90 File Offset: 0x00071B90
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

		// Token: 0x0600041B RID: 1051 RVA: 0x00072C44 File Offset: 0x00071C44
		private void loadUserData()
		{
			wgTools.WriteLine("loadUserData Start");
			string text = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID ";
			text += " FROM t_b_Consumer WHERE f_DoorEnabled > 0 ORDER BY f_ConsumerNO ASC ";
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

		// Token: 0x0600041C RID: 1052 RVA: 0x00072EB8 File Offset: 0x00071EB8
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00072ED8 File Offset: 0x00071ED8
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
			this.dv.RowFilter = icConsumerShare.getOptionalRowfilter(this.bFilterNotAttend);
			this.dvSelected.RowFilter = icConsumerShare.getSelectedRowfilter(this.bFilterNotAttend);
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

		// Token: 0x0600041E RID: 1054 RVA: 0x00073064 File Offset: 0x00072064
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
			this.btnOK.Enabled = true;
		}

		// Token: 0x040007B6 RID: 1974
		private DataTable dt;

		// Token: 0x040007B7 RID: 1975
		private DataTable dtUser;

		// Token: 0x040007B8 RID: 1976
		private DataView dv;

		// Token: 0x040007B9 RID: 1977
		private DataView dv1;

		// Token: 0x040007BA RID: 1978
		private DataView dv2;

		// Token: 0x040007BB RID: 1979
		private DataView dvSelected;

		// Token: 0x040007BC RID: 1980
		private Control defaultFindControl;

		// Token: 0x040007BD RID: 1981
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x040007BE RID: 1982
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x040007BF RID: 1983
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x040007C0 RID: 1984
		public bool bFilterNotAttend;

		// Token: 0x040007C1 RID: 1985
		public string selectedUsers = "";

		// Token: 0x040007C2 RID: 1986
		private string strGroupFilter = "";
	}
}
