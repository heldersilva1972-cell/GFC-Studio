using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x0200031E RID: 798
	public partial class frmPrivilegeTypeManagement : frmN3000
	{
		// Token: 0x060018BE RID: 6334 RVA: 0x00205C24 File Offset: 0x00204C24
		public frmPrivilegeTypeManagement()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvPrivilegeTypes);
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x00205C44 File Offset: 0x00204C44
		private void _loadData()
		{
			string text = "SELECT  [f_PrivilegeTypeID], [f_PrivilegeTypeName],0 as [f_Active],0 as [f_Doors],0 as [f_Users],[f_Note] FROM t_d_PrivilegeType ORDER BY f_PrivilegeTypeName ";
			this.dt = new DataTable("t_d_PrivilegeType");
			DataSet dataSet = new DataSet();
			using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
			{
				using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
				{
					using (DataAdapter dataAdapter = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand) : new SqlDataAdapter((SqlCommand)dbCommand)))
					{
						dataAdapter.Fill(dataSet);
					}
				}
			}
			this.dt = dataSet.Tables[0];
			DataTable dataTable = new DataTable("doors");
			DataSet dataSet2 = new DataSet();
			text = "SELECT DISTINCT [f_ConsumerID], COUNT(*) as [f_Doors] FROM t_d_Privilege_Of_PrivilegeType GROUP BY f_ConsumerID";
			using (DbConnection dbConnection2 = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
			{
				using (DbCommand dbCommand2 = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection2) : new SqlCommand(text, (SqlConnection)dbConnection2)))
				{
					using (DataAdapter dataAdapter2 = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand2) : new SqlDataAdapter((SqlCommand)dbCommand2)))
					{
						dataAdapter2.Fill(dataSet2);
					}
				}
			}
			dataTable = dataSet2.Tables[0];
			DataTable dataTable2 = new DataTable("Users");
			DataSet dataSet3 = new DataSet();
			text = "SELECT DISTINCT [f_PrivilegeTypeID], COUNT(*) as [f_Users] FROM t_b_Consumer GROUP BY f_PrivilegeTypeID";
			using (DbConnection dbConnection3 = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
			{
				using (DbCommand dbCommand3 = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection3) : new SqlCommand(text, (SqlConnection)dbConnection3)))
				{
					using (DataAdapter dataAdapter3 = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand3) : new SqlDataAdapter((SqlCommand)dbCommand3)))
					{
						dataAdapter3.Fill(dataSet3);
					}
				}
			}
			dataTable2 = dataSet3.Tables[0];
			int num = 0;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				for (int j = 0; j < dataTable.Rows.Count; j++)
				{
					if ((int)this.dt.Rows[i]["f_PrivilegeTypeID"] == (int)dataTable.Rows[j]["f_ConsumerID"])
					{
						this.dt.Rows[i]["f_Doors"] = dataTable.Rows[j]["f_Doors"];
					}
				}
				for (int k = 0; k < dataTable2.Rows.Count; k++)
				{
					if ((int)this.dt.Rows[i]["f_PrivilegeTypeID"] == (int)dataTable2.Rows[k]["f_PrivilegeTypeID"])
					{
						this.dt.Rows[i]["f_Users"] = dataTable2.Rows[k]["f_Users"];
					}
				}
				num += (int)this.dt.Rows[i]["f_Doors"] * (int)this.dt.Rows[i]["f_Users"];
			}
			this.dt.AcceptChanges();
			if (this.dt.Rows.Count > 0)
			{
				this.btnDelete.Enabled = true;
				this.btnEdit.Enabled = true;
				this.btnEditUsers.Enabled = true;
			}
			else
			{
				this.btnDelete.Enabled = false;
				this.btnEdit.Enabled = false;
				this.btnEditUsers.Enabled = false;
			}
			DataView dataView = new DataView(this.dt);
			this.dgvPrivilegeTypes.AutoGenerateColumns = false;
			this.dgvPrivilegeTypes.DataSource = dataView;
			int num2 = 0;
			while (num2 < dataView.Table.Columns.Count && num2 < this.dgvPrivilegeTypes.ColumnCount)
			{
				this.dgvPrivilegeTypes.Columns[num2].DataPropertyName = dataView.Table.Columns[num2].ColumnName;
				num2++;
			}
			this.dgvPrivilegeTypes.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			wgAppRunInfo.raiseAppRunInfoLoadNums(num.ToString());
			int privilegeCount;
			if (num != (privilegeCount = this.getPrivilegeCount()))
			{
				wgAppRunInfo.raiseAppRunInfoLoadNums(num.ToString() + ":" + privilegeCount.ToString());
				this.timer1.Enabled = true;
			}
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x002061E0 File Offset: 0x002051E0
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmPrivilegeTypeEdit dfrmPrivilegeTypeEdit = new dfrmPrivilegeTypeEdit())
			{
				if (dfrmPrivilegeTypeEdit.ShowDialog(this) == DialogResult.OK)
				{
					this._loadData();
				}
			}
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x00206220 File Offset: 0x00205220
		private void btnDelete_Click(object sender, EventArgs e)
		{
			int num;
			if (this.dgvPrivilegeTypes.SelectedRows.Count <= 0)
			{
				if (this.dgvPrivilegeTypes.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dgvPrivilegeTypes.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvPrivilegeTypes.SelectedRows[0].Index;
			}
			string text = string.Format("{0}\r\n\r\n{1}:  {2}", this.btnDelete.Text, this.dgvPrivilegeTypes.Columns[1].HeaderText, this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString());
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				try
				{
					int num2 = (int)this.dgvPrivilegeTypes.Rows[num].Cells[0].Value;
					Cursor.Current = Cursors.WaitCursor;
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
						dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						string text2 = " DELETE FROM t_d_Privilege ";
						text2 += string.Format(" WHERE t_d_Privilege.f_ConsumerID IN ( SELECT f_ConsumerID FROM t_b_Consumer WHERE t_b_Consumer.f_PrivilegeTypeID = {0} ) ", num2.ToString());
						dbCommand.CommandText = text2;
						dbCommand.ExecuteNonQuery();
						text2 = " UPDATE t_b_Consumer SET f_PrivilegeTypeID = 0 ";
						text2 += string.Format("  WHERE f_PrivilegeTypeID = {0}  ", num2.ToString());
						dbCommand.CommandText = text2;
						dbCommand.ExecuteNonQuery();
						text2 = " DELETE FROM t_d_Privilege_Of_PrivilegeType ";
						if (this.dgvPrivilegeTypes.Rows.Count > 1)
						{
							text2 += string.Format(" WHERE f_ConsumerID = {0}  ", num2.ToString());
						}
						dbCommand.CommandText = text2;
						dbCommand.ExecuteNonQuery();
						text2 = " DELETE FROM t_d_PrivilegeType ";
						text2 = text2 + " WHERE f_PrivilegeTypeName = " + wgTools.PrepareStrNUnicode(this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString());
						dbCommand.CommandText = text2;
						dbCommand.ExecuteNonQuery();
						dbConnection.Close();
						this._loadData();
						XMessageBox.Show(string.Format(" {0}  {1}.", this.btnDelete.Text, CommonStr.strSuccessfully));
						this.showUpload();
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
					}
					Cursor.Current = Cursors.Default;
				}
				catch (Exception ex2)
				{
					wgAppConfig.wgLog(ex2.ToString());
				}
			}
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x00206510 File Offset: 0x00205510
		private void btnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvPrivilegeTypes.SelectedRows.Count <= 0)
				{
					if (this.dgvPrivilegeTypes.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvPrivilegeTypes.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvPrivilegeTypes.SelectedRows[0].Index;
				}
				int num2 = (int)this.dgvPrivilegeTypes.Rows[num].Cells[0].Value;
				using (dfrmPrivilegeTypeEdit dfrmPrivilegeTypeEdit = new dfrmPrivilegeTypeEdit())
				{
					dfrmPrivilegeTypeEdit.PrivilegeTypeID = num2;
					dfrmPrivilegeTypeEdit.Text = dfrmPrivilegeTypeEdit.Text + " -- " + this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString();
					if (dfrmPrivilegeTypeEdit.ShowDialog(this) == DialogResult.OK)
					{
						this._loadData();
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x00206634 File Offset: 0x00205634
		private void btnEditDoors_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvPrivilegeTypes.SelectedRows.Count <= 0)
				{
					if (this.dgvPrivilegeTypes.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvPrivilegeTypes.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvPrivilegeTypes.SelectedRows[0].Index;
				}
				int num2 = (int)this.dgvPrivilegeTypes.Rows[num].Cells[0].Value;
				using (dfrmPrivilegeTypeDoors dfrmPrivilegeTypeDoors = new dfrmPrivilegeTypeDoors())
				{
					dfrmPrivilegeTypeDoors.PrivilegeTypeID = num2;
					dfrmPrivilegeTypeDoors.Text = dfrmPrivilegeTypeDoors.Text + " -- " + this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString();
					if (dfrmPrivilegeTypeDoors.ShowDialog(this) == DialogResult.OK)
					{
						this.showUpload();
						this._loadData();
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0020675C File Offset: 0x0020575C
		private void btnEditUser_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvPrivilegeTypes.SelectedRows.Count <= 0)
				{
					if (this.dgvPrivilegeTypes.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvPrivilegeTypes.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvPrivilegeTypes.SelectedRows[0].Index;
				}
				int num2 = (int)this.dgvPrivilegeTypes.Rows[num].Cells[0].Value;
				using (dfrmPrivilegeTypeUser dfrmPrivilegeTypeUser = new dfrmPrivilegeTypeUser())
				{
					dfrmPrivilegeTypeUser.PrivilegeTypeID = num2;
					dfrmPrivilegeTypeUser.Text = dfrmPrivilegeTypeUser.Text + " -- " + this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString();
					dfrmPrivilegeTypeUser.lblSelectedUsers.Text = string.Format("{0}  -- {1}", dfrmPrivilegeTypeUser.lblSelectedUsers.Text, this.dgvPrivilegeTypes.Rows[num].Cells[1].Value.ToString());
					if (dfrmPrivilegeTypeUser.ShowDialog(this) == DialogResult.OK)
					{
						this.showUpload();
						this._loadData();
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x002068E4 File Offset: 0x002058E4
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcel(this.dgvPrivilegeTypes, this.Text);
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x002068F8 File Offset: 0x002058F8
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvPrivilegeTypes);
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x0020690C File Offset: 0x0020590C
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvPrivilegeTypes, this.Text);
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x00206920 File Offset: 0x00205920
		private void btnPrivilege_Click(object sender, EventArgs e)
		{
			using (frmPrivileges frmPrivileges = new frmPrivileges())
			{
				frmPrivileges.ShowDialog();
			}
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x00206958 File Offset: 0x00205958
		private void btnQueryDoors_Click(object sender, EventArgs e)
		{
			using (dfrmPrivilegeTypeDoorsType dfrmPrivilegeTypeDoorsType = new dfrmPrivilegeTypeDoorsType())
			{
				dfrmPrivilegeTypeDoorsType.ShowDialog();
			}
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x00206990 File Offset: 0x00205990
		private void btnSyncPrivileges_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnSyncPrivileges.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				this.SyncPrivilegeByPrivilegeType();
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x002069DD File Offset: 0x002059DD
		private void dgvControlSegs_DoubleClick(object sender, EventArgs e)
		{
			this.btnEditDoors.PerformClick();
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x002069EC File Offset: 0x002059EC
		private void frmControlSegs_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
			{
				wgAppConfig.findCall(this.dfrmFind1, this, this.dgvPrivilegeTypes);
			}
			if (e.Control && e.KeyValue == 81 && e.Shift)
			{
				this.btnPrivilege.Visible = true;
			}
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x00206A4D File Offset: 0x00205A4D
		private void frmControlSegs_Load(object sender, EventArgs e)
		{
			this._loadData();
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x00206A55 File Offset: 0x00205A55
		private void frmPrivilegeTypeManagement_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00206A58 File Offset: 0x00205A58
		private int getPrivilegeCount()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.getValBySql("select  count(*)  from t_d_Privilege");
			}
			if (wgAppConfig.getSystemParamByNO(53) == "1")
			{
				return wgAppConfig.getValBySql(" SELECT SUM(row_count)  FROM sys.dm_db_partition_stats WHERE object_id = OBJECT_ID('t_d_privilege')  AND index_id =1  ");
			}
			int num = wgAppConfig.getValBySql("select rowcnt from sysindexes where id=object_id(N't_d_Privilege') and name = N'PK_t_d_Privilege'");
			if (num <= 2000000)
			{
				num = wgAppConfig.getValBySql("select count(1) from t_d_Privilege");
			}
			return num;
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x00206AB8 File Offset: 0x00205AB8
		private void loadOperatorPrivilege()
		{
			bool flag;
			bool flag2;
			icOperator.getFrmOperatorPrivilege(base.Name.ToString(), out flag, out flag2);
			if (flag || flag2)
			{
				if (!flag2 && flag)
				{
					this.btnAdd.Visible = false;
					this.btnEdit.Visible = false;
					this.btnDelete.Visible = false;
					return;
				}
			}
			else
			{
				base.Close();
			}
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x00206B10 File Offset: 0x00205B10
		private void showUpload()
		{
			if (this.firstShow)
			{
				this.firstShow = false;
				XMessageBox.Show(CommonStr.strNeedUploadPrivilege);
			}
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x00206B2C File Offset: 0x00205B2C
		private void SyncPrivilegeByPrivilegeType()
		{
			using (dfrmPrivilegeSync dfrmPrivilegeSync = new dfrmPrivilegeSync())
			{
				dfrmPrivilegeSync.ShowDialog(this);
			}
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00206B64 File Offset: 0x00205B64
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			if (XMessageBox.Show(this, CommonStr.strNeedSyncPrivilegeTips, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				this.SyncPrivilegeByPrivilegeType();
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x040032AA RID: 12970
		private DataTable dt;

		// Token: 0x040032AB RID: 12971
		private bool firstShow = true;
	}
}
