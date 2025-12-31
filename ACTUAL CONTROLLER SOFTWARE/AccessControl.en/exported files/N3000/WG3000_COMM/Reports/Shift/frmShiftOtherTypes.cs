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

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000381 RID: 897
	public partial class frmShiftOtherTypes : frmN3000
	{
		// Token: 0x06001DDF RID: 7647 RVA: 0x002775E0 File Offset: 0x002765E0
		public frmShiftOtherTypes()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x002775FC File Offset: 0x002765FC
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmShiftOtherTypeSet dfrmShiftOtherTypeSet = new dfrmShiftOtherTypeSet())
			{
				dfrmShiftOtherTypeSet.operateMode = "New";
				if (dfrmShiftOtherTypeSet.ShowDialog(this) == DialogResult.OK)
				{
					this.loadData();
				}
			}
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x00277648 File Offset: 0x00276648
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
			if (XMessageBox.Show(this, text, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				return;
			}
			if (wgAppConfig.IsAccessDB)
			{
				using (comShift_Acc comShift_Acc = new comShift_Acc())
				{
					comShift_Acc.shift_delete(int.Parse(this.dgvMain.Rows[num].Cells[0].Value.ToString()));
					goto IL_015F;
				}
			}
			using (comShift comShift = new comShift())
			{
				comShift.shift_delete(int.Parse(this.dgvMain.Rows[num].Cells[0].Value.ToString()));
			}
			IL_015F:
			this.loadData();
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x002777D8 File Offset: 0x002767D8
		private void btnEdit_Click(object sender, EventArgs e)
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
			using (dfrmShiftOtherTypeSet dfrmShiftOtherTypeSet = new dfrmShiftOtherTypeSet())
			{
				dfrmShiftOtherTypeSet.curShiftID = int.Parse(this.dgvMain.Rows[num].Cells[0].Value.ToString());
				if (dfrmShiftOtherTypeSet.ShowDialog(this) == DialogResult.OK)
				{
					this.loadData();
				}
			}
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x002778A0 File Offset: 0x002768A0
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcel(this.dgvMain, this.Text);
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x002778B4 File Offset: 0x002768B4
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvMain);
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x002778C8 File Offset: 0x002768C8
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvMain, this.Text);
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x002778DB File Offset: 0x002768DB
		private void dgvControlSegs_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x002778E8 File Offset: 0x002768E8
		private void frmShiftOtherTypes_Load(object sender, EventArgs e)
		{
			this.Refresh();
			this.loadOperatorPrivilege();
			this.loadData();
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x002778FC File Offset: 0x002768FC
		private void loadData()
		{
			this.dt = new DataTable();
			this.dv = new DataView(this.dt);
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = " SELECT ";
				text += " [f_ShiftID], [f_ShiftName], [f_ReadTimes], [f_bOvertimeShift] ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OnDuty1],'Short Time') )   AS [f_OnDuty1t]  ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OffDuty1],'Short Time') )  AS [f_OffDuty1t]  ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OnDuty2],'Short Time') )   AS [f_OnDuty2t]  ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OffDuty2],'Short Time') )  AS [f_OffDuty2t]  ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OnDuty3],'Short Time') )   AS [f_OnDuty3t]  ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OffDuty3],'Short Time') )  AS [f_OffDuty3t]  ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OnDuty4],'Short Time') )   AS [f_OnDuty4t]  ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OffDuty4],'Short Time') )  AS [f_OffDuty4t]   FROM [t_b_ShiftSet] ORDER BY [f_ShiftID] ASC  ";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dt);
						}
					}
					goto IL_00F7;
				}
			}
			text = " SELECT ";
			text += " [f_ShiftID], [f_ShiftName], [f_ReadTimes], [f_bOvertimeShift] ,ISNULL(CONVERT(char(5), f_OnDuty1,108) , ' ') AS [f_OnDuty1t]  ,ISNULL(CONVERT(char(5), f_OffDuty1,108) , ' ') AS [f_OffDuty1t]  ,ISNULL(CONVERT(char(5), f_OnDuty2,108) , ' ') AS [f_OnDuty2t]  ,ISNULL(CONVERT(char(5), f_OffDuty2,108) , ' ') AS [f_OffDuty2t]  ,ISNULL(CONVERT(char(5), f_OnDuty3,108) , ' ') AS [f_OnDuty3t]  ,ISNULL(CONVERT(char(5), f_OffDuty3,108) , ' ') AS [f_OffDuty3t]  ,ISNULL(CONVERT(char(5), f_OnDuty4,108) , ' ') AS [f_OnDuty4t]  ,ISNULL(CONVERT(char(5), f_OffDuty4,108) , ' ') AS [f_OffDuty4t]   FROM [t_b_ShiftSet] ORDER BY [f_ShiftID] ASC  ";
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
			IL_00F7:
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

		// Token: 0x06001DE9 RID: 7657 RVA: 0x00277B38 File Offset: 0x00276B38
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuShiftSet";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnEdit.Visible = false;
				this.btnDelete.Visible = false;
			}
		}

		// Token: 0x0400391C RID: 14620
		private DataTable dt;

		// Token: 0x0400391D RID: 14621
		private DataView dv;
	}
}
