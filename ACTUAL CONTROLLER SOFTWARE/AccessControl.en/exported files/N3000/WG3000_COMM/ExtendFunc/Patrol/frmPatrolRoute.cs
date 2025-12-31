using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x0200030F RID: 783
	public partial class frmPatrolRoute : frmN3000
	{
		// Token: 0x060017D4 RID: 6100 RVA: 0x001F1070 File Offset: 0x001F0070
		public frmPatrolRoute()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x001F108C File Offset: 0x001F008C
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmRouteEdit dfrmRouteEdit = new dfrmRouteEdit())
			{
				if (dfrmRouteEdit.ShowDialog(this) == DialogResult.OK)
				{
					this.loadData();
				}
			}
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x001F10CC File Offset: 0x001F00CC
		private void btnDelete_Click(object sender, EventArgs e)
		{
			int num;
			if (this.dgvMain.SelectedRows.Count <= 0)
			{
				if (this.dgvMain.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dgvMain.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvMain.SelectedRows[0].Index;
			}
			string text = string.Format("{0}\r\n{1}:  {2}", this.btnDelete.Text, this.dgvMain.Columns[0].HeaderText, this.dgvMain.Rows[num].Cells[0].Value.ToString());
			text = string.Format(CommonStr.strAreYouSure + " {0} ?", text);
			if (XMessageBox.Show(this, text, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				int num2 = int.Parse(this.dgvMain.Rows[num].Cells[0].Value.ToString());
				wgAppConfig.runUpdateSql(" DELETE FROM t_d_PatrolRouteList WHERE f_RouteID = " + num2.ToString());
				wgAppConfig.runUpdateSql(" DELETE FROM t_d_PatrolRouteDetail WHERE f_RouteID = " + num2.ToString());
				this.loadData();
			}
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x001F1208 File Offset: 0x001F0208
		private void btnEdit_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvMain.Rows.Count > 0)
			{
				try
				{
					num = this.dgvMain.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num2;
			if (this.dgvMain.SelectedRows.Count <= 0)
			{
				if (this.dgvMain.SelectedCells.Count <= 0)
				{
					return;
				}
				num2 = this.dgvMain.SelectedCells[0].RowIndex;
			}
			else
			{
				num2 = this.dgvMain.SelectedRows[0].Index;
			}
			using (dfrmRouteEdit dfrmRouteEdit = new dfrmRouteEdit())
			{
				dfrmRouteEdit.currentRouteID = int.Parse(this.dgvMain.Rows[num2].Cells[0].Value.ToString());
				if (dfrmRouteEdit.ShowDialog(this) == DialogResult.OK)
				{
					this.loadData();
				}
			}
			if (this.dgvMain.RowCount > 0)
			{
				if (this.dgvMain.RowCount > num)
				{
					this.dgvMain.CurrentCell = this.dgvMain[1, num];
					return;
				}
				this.dgvMain.CurrentCell = this.dgvMain[1, this.dgvMain.RowCount - 1];
			}
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x001F1360 File Offset: 0x001F0360
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x001F1368 File Offset: 0x001F0368
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcel(this.dgvMain, this.Text);
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x001F137C File Offset: 0x001F037C
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x001F138F File Offset: 0x001F038F
		private void dgvControlSegs_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x001F139C File Offset: 0x001F039C
		private void frmShiftOtherTypes_Load(object sender, EventArgs e)
		{
			this.Refresh();
			this.loadOperatorPrivilege();
			this.loadData();
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x001F13B0 File Offset: 0x001F03B0
		private void loadData()
		{
			this.dt = new DataTable();
			this.dv = new DataView(this.dt);
			string text = " SELECT ";
			text += " [f_RouteID], [f_RouteName]   FROM [t_d_PatrolRouteList] ORDER BY [f_RouteID] ASC  ";
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
					goto IL_00E5;
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
			IL_00E5:
			DataGridView dataGridView = this.dgvMain;
			dataGridView.AutoGenerateColumns = false;
			dataGridView.DataSource = this.dv;
			for (int i = 0; i < this.dv.Table.Columns.Count; i++)
			{
				dataGridView.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
			}
			if (this.dv.Count > 0)
			{
				for (int i = 0; i < this.dv.Count; i++)
				{
				}
				this.btnAdd.Enabled = true;
				this.btnEdit.Enabled = true;
				this.btnDelete.Enabled = true;
				this.btnPrint.Enabled = true;
				return;
			}
			this.btnAdd.Enabled = true;
			this.btnEdit.Enabled = false;
			this.btnDelete.Enabled = false;
			this.btnPrint.Enabled = false;
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x001F15DC File Offset: 0x001F05DC
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuPatrolDetailData";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnEdit.Visible = false;
				this.btnDelete.Visible = false;
			}
		}

		// Token: 0x04003105 RID: 12549
		private DataTable dt;

		// Token: 0x04003106 RID: 12550
		private DataView dv;
	}
}
