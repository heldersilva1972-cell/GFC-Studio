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

namespace WG3000_COMM.Basic
{
	// Token: 0x0200001F RID: 31
	public partial class dfrmOperator : frmN3000
	{
		// Token: 0x060001B8 RID: 440 RVA: 0x0003CDAF File Offset: 0x0003BDAF
		public dfrmOperator()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvOperators);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0003CDC8 File Offset: 0x0003BDC8
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmOperatorUpdate dfrmOperatorUpdate = new dfrmOperatorUpdate())
			{
				dfrmOperatorUpdate.operateMode = 0;
				dfrmOperatorUpdate.Text = this.btnAdd.Text + " " + dfrmOperatorUpdate.Text;
				if (dfrmOperatorUpdate.ShowDialog(this) == DialogResult.OK)
				{
					this.loadOperatorData();
				}
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0003CE30 File Offset: 0x0003BE30
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (this.dgvOperators.Rows.Count > 0)
			{
				int num = 0;
				if (this.dgvOperators.Rows.Count > 0)
				{
					try
					{
						num = this.dgvOperators.CurrentCell.RowIndex;
					}
					catch
					{
					}
				}
				if (int.Parse(this.dgvOperators.Rows[num].Cells[0].Value.ToString()) == 1)
				{
					XMessageBox.Show(this, CommonStr.strDeleteForbidden, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (XMessageBox.Show(this, CommonStr.strDelete + " " + this.dgvOperators[1, num].Value.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
				{
					wgAppConfig.runUpdateSql(" DELETE FROM [t_s_Operator] WHERE  [f_OperatorID]=" + this.dgvOperators.Rows[num].Cells[0].Value.ToString());
					this.loadOperatorData();
				}
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0003CF4C File Offset: 0x0003BF4C
		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (this.dgvOperators.Rows.Count > 0)
			{
				int num = 0;
				if (this.dgvOperators.Rows.Count > 0)
				{
					try
					{
						num = this.dgvOperators.CurrentCell.RowIndex;
					}
					catch
					{
					}
				}
				using (dfrmOperatorUpdate dfrmOperatorUpdate = new dfrmOperatorUpdate())
				{
					dfrmOperatorUpdate.Text = this.btnEdit.Text + " " + dfrmOperatorUpdate.Text;
					dfrmOperatorUpdate.operateMode = 1;
					dfrmOperatorUpdate.operatorID = int.Parse(this.dgvOperators.Rows[num].Cells[0].Value.ToString());
					dfrmOperatorUpdate.operatorName = this.dgvOperators.Rows[num].Cells[1].Value.ToString();
					if (dfrmOperatorUpdate.ShowDialog(this) == DialogResult.OK)
					{
						this.loadOperatorData();
					}
				}
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0003D060 File Offset: 0x0003C060
		private void btnEditDepartment_Click(object sender, EventArgs e)
		{
			if (this.dgvOperators.Rows.Count > 0)
			{
				int num = 0;
				if (this.dgvOperators.Rows.Count > 0)
				{
					try
					{
						num = this.dgvOperators.CurrentCell.RowIndex;
					}
					catch
					{
					}
				}
				if (int.Parse(this.dgvOperators.Rows[num].Cells[0].Value.ToString()) == 1)
				{
					XMessageBox.Show(this, CommonStr.strEditOperatePrivilegeForbidden, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				using (dfrmOperatorDepartmentsConfiguration dfrmOperatorDepartmentsConfiguration = new dfrmOperatorDepartmentsConfiguration())
				{
					dfrmOperatorDepartmentsConfiguration.operatorId = int.Parse(this.dgvOperators.Rows[num].Cells[0].Value.ToString());
					dfrmOperatorDepartmentsConfiguration.ShowDialog(this);
				}
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0003D158 File Offset: 0x0003C158
		private void btnEditPrivilege_Click(object sender, EventArgs e)
		{
			if (this.dgvOperators.Rows.Count > 0)
			{
				int num = 0;
				if (this.dgvOperators.Rows.Count > 0)
				{
					try
					{
						num = this.dgvOperators.CurrentCell.RowIndex;
					}
					catch
					{
					}
				}
				if (int.Parse(this.dgvOperators.Rows[num].Cells[0].Value.ToString()) == 1)
				{
					XMessageBox.Show(this, CommonStr.strEditOperatePrivilegeForbidden, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				using (dfrmOperatePrivilege dfrmOperatePrivilege = new dfrmOperatePrivilege())
				{
					dfrmOperatePrivilege.Text = this.dgvOperators.Rows[num].Cells[1].Value.ToString() + "--" + dfrmOperatePrivilege.Text;
					dfrmOperatePrivilege.operatorID = int.Parse(this.dgvOperators.Rows[num].Cells[0].Value.ToString());
					dfrmOperatePrivilege.ShowDialog(this);
				}
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0003D28C File Offset: 0x0003C28C
		private void btnEditZones_Click(object sender, EventArgs e)
		{
			if (this.dgvOperators.Rows.Count > 0)
			{
				int num = 0;
				if (this.dgvOperators.Rows.Count > 0)
				{
					try
					{
						num = this.dgvOperators.CurrentCell.RowIndex;
					}
					catch
					{
					}
				}
				if (int.Parse(this.dgvOperators.Rows[num].Cells[0].Value.ToString()) == 1)
				{
					XMessageBox.Show(this, CommonStr.strEditOperatePrivilegeForbidden, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				using (dfrmOperatorZonesConfiguration dfrmOperatorZonesConfiguration = new dfrmOperatorZonesConfiguration())
				{
					dfrmOperatorZonesConfiguration.operatorId = int.Parse(this.dgvOperators.Rows[num].Cells[0].Value.ToString());
					dfrmOperatorZonesConfiguration.ShowDialog(this);
				}
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0003D384 File Offset: 0x0003C384
		private void btnSetPassword_Click(object sender, EventArgs e)
		{
			if (this.dgvOperators.Rows.Count > 0)
			{
				int num = 0;
				if (this.dgvOperators.Rows.Count > 0)
				{
					try
					{
						num = this.dgvOperators.CurrentCell.RowIndex;
					}
					catch
					{
					}
				}
				using (dfrmOperatorUpdate dfrmOperatorUpdate = new dfrmOperatorUpdate())
				{
					dfrmOperatorUpdate.Text = this.btnSetPassword.Text;
					dfrmOperatorUpdate.operateMode = 2;
					dfrmOperatorUpdate.operatorID = int.Parse(this.dgvOperators.Rows[num].Cells[0].Value.ToString());
					dfrmOperatorUpdate.operatorName = this.dgvOperators.Rows[num].Cells[1].Value.ToString();
					dfrmOperatorUpdate.ShowDialog(this);
				}
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0003D480 File Offset: 0x0003C480
		private void dfrmOperator_Load(object sender, EventArgs e)
		{
			this.btnEditDepartment.Text = wgAppConfig.ReplaceFloorRoom(this.btnEditDepartment.Text);
			this.loadOperatorPrivilege();
			this.loadOperatorData();
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0003D4AC File Offset: 0x0003C4AC
		private void loadOperatorData()
		{
			string text = " SELECT f_OperatorID, f_OperatorName";
			text += " FROM t_s_Operator   ORDER BY f_OperatorID ";
			this.table = new DataTable();
			this.dv = new DataView(this.table);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.table);
						}
					}
					goto IL_00D7;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.table);
					}
				}
			}
			IL_00D7:
			this.dgvOperators.AutoGenerateColumns = false;
			this.dgvOperators.DataSource = this.dv;
			for (int i = 0; i < this.dv.Table.Columns.Count; i++)
			{
				this.dgvOperators.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
			}
			if (this.dv.Count > 0)
			{
				this.btnAdd.Enabled = true;
				this.btnEdit.Enabled = true;
				this.btnDelete.Enabled = true;
				return;
			}
			this.btnAdd.Enabled = true;
			this.btnEdit.Enabled = false;
			this.btnDelete.Enabled = false;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0003D6A8 File Offset: 0x0003C6A8
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
					this.btnSetPassword.Visible = false;
					this.btnEditPrivilege.Visible = false;
					this.toolStrip1.Visible = false;
					return;
				}
			}
			else
			{
				base.Close();
			}
		}

		// Token: 0x040003DC RID: 988
		private DataView dv;

		// Token: 0x040003DD RID: 989
		private DataTable table;
	}
}
