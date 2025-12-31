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
	// Token: 0x0200004E RID: 78
	public partial class frmControlSegs : frmN3000
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x0009D4BC File Offset: 0x0009C4BC
		public frmControlSegs()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvControlSegs);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0009D4DC File Offset: 0x0009C4DC
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmControlSeg dfrmControlSeg = new dfrmControlSeg())
			{
				dfrmControlSeg.operateMode = "New";
				if (dfrmControlSeg.ShowDialog(this) == DialogResult.OK)
				{
					this.loadControlSegData();
					this.showUpload();
				}
			}
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0009D52C File Offset: 0x0009C52C
		private void btnDelete_Click(object sender, EventArgs e)
		{
			int num;
			if (this.dgvControlSegs.SelectedRows.Count <= 0)
			{
				if (this.dgvControlSegs.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dgvControlSegs.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvControlSegs.SelectedRows[0].Index;
			}
			string text = string.Format("{0}\r\n\r\n{1}:  {2}", this.btnDelete.Text, this.dgvControlSegs.Columns[1].HeaderText, this.dgvControlSegs.Rows[num].Cells[0].Value.ToString());
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_ControlSeg WHERE [f_ControlSegID]= " + this.dgvControlSegs.Rows[num].Cells[0].Value.ToString());
				this.loadControlSegData();
				this.showUpload();
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0009D648 File Offset: 0x0009C648
		private void btnEdit_Click(object sender, EventArgs e)
		{
			int num;
			if (this.dgvControlSegs.SelectedRows.Count <= 0)
			{
				if (this.dgvControlSegs.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dgvControlSegs.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvControlSegs.SelectedRows[0].Index;
			}
			using (dfrmControlSeg dfrmControlSeg = new dfrmControlSeg())
			{
				dfrmControlSeg.curControlSegID = int.Parse(this.dgvControlSegs.Rows[num].Cells[0].Value.ToString());
				if (dfrmControlSeg.ShowDialog(this) == DialogResult.OK)
				{
					this.loadControlSegData();
					this.showUpload();
				}
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0009D718 File Offset: 0x0009C718
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcel(this.dgvControlSegs, this.Text);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0009D72C File Offset: 0x0009C72C
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvControlSegs);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0009D740 File Offset: 0x0009C740
		private void btnHolidayControl_Click(object sender, EventArgs e)
		{
			using (dfrmControlHolidaySet dfrmControlHolidaySet = new dfrmControlHolidaySet())
			{
				dfrmControlHolidaySet.ShowDialog();
			}
			this.showUpload();
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0009D77C File Offset: 0x0009C77C
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvControlSegs, this.Text);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0009D78F File Offset: 0x0009C78F
		private void dgvControlSegs_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0009D79C File Offset: 0x0009C79C
		private void frmControlSegs_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
			{
				wgAppConfig.findCall(this.dfrmFind1, this, this.dgvControlSegs);
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0009D7CC File Offset: 0x0009C7CC
		private void frmControlSegs_Load(object sender, EventArgs e)
		{
			this.loadOperatorPrivilege();
			this.loadControlSegData();
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0009D7DC File Offset: 0x0009C7DC
		private void loadControlSegData()
		{
			string text = " SELECT ";
			text += " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak, ";
			if (wgAppConfig.IsAccessDB)
			{
				text += "  IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID,  [f_Monday], [f_Tuesday], [f_Wednesday] , [f_Thursday], [f_Friday], [f_Saturday], [f_Sunday]  , Format([f_BeginHMS1],'Short Time')  as [f_BeginHMS1A],Format([f_EndHMS1],'Short Time')  as [f_EndHMS1A], Format([f_BeginHMS2],'Short Time')  as [f_BeginHMS2A],Format([f_EndHMS2],'Short Time')  as [f_EndHMS2A], Format([f_BeginHMS3],'Short Time')  as [f_BeginHMS3A],Format([f_EndHMS3],'Short Time')  as [f_EndHMS3A]  ,f_ControlSegIDLinked,f_BeginYMD, f_EndYMD  ,   f_ControlByHoliday     FROM [t_b_ControlSeg] ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
			}
			else
			{
				text += "   CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),       ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50),      ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']')     END AS f_ControlSegID, [f_Monday], [f_Tuesday], [f_Wednesday], [f_Thursday],    [f_Friday], [f_Saturday], [f_Sunday],  ISNULL(CONVERT(char(5), f_BeginHMS1,108) , '00:00') AS [f_BeginHMS1A],  ISNULL(CONVERT(char(5), f_EndHMS1,108) , '00:00')  AS [f_EndHMS1A],  ISNULL(CONVERT(char(5), f_BeginHMS2,108) , '00:00')  AS [f_BeginHMS2A],  ISNULL(CONVERT(char(5), f_EndHMS2,108) , '00:00')  AS [f_EndHMS2A],  ISNULL(CONVERT(char(5), f_BeginHMS3,108) , '00:00')  AS [f_BeginHMS3A],  ISNULL(CONVERT(char(5), f_EndHMS3,108) , '00:00')  AS [f_EndHMS3A]   ,f_ControlSegIDLinked,    f_BeginYMD,    f_EndYMD  ,   f_ControlByHoliday    FROM [t_b_ControlSeg] ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
			}
			this.dt = new DataTable();
			this.dv = new DataView(this.dt);
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
					goto IL_00FC;
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
			IL_00FC:
			DataGridView dataGridView = this.dgvControlSegs;
			dataGridView.AutoGenerateColumns = false;
			dataGridView.DataSource = this.dv;
			for (int i = 0; i < this.dv.Table.Columns.Count; i++)
			{
				dataGridView.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
				dataGridView.Columns[i].Name = this.dv.Table.Columns[i].ColumnName;
			}
			wgAppConfig.setDisplayFormatDate(dataGridView, "f_BeginYMD", wgTools.DisplayFormat_DateYMD);
			wgAppConfig.setDisplayFormatDate(dataGridView, "f_EndYMD", wgTools.DisplayFormat_DateYMD);
			using (DataView dataView = new DataView(this.dt))
			{
				dataView.RowFilter = "f_ControlByHoliday = 0";
				if (dataView.Count == 0)
				{
					dataGridView.Columns["f_ControlByHoliday"].Visible = false;
				}
				else
				{
					dataGridView.Columns["f_ControlByHoliday"].Visible = true;
				}
			}
			if (this.dv.Count > 0)
			{
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

		// Token: 0x0600059B RID: 1435 RVA: 0x0009DAC4 File Offset: 0x0009CAC4
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

		// Token: 0x0600059C RID: 1436 RVA: 0x0009DB1C File Offset: 0x0009CB1C
		private void showUpload()
		{
			if (this.firstShow)
			{
				this.firstShow = false;
				XMessageBox.Show(CommonStr.strNeedUploadControlTimeSeg);
			}
		}

		// Token: 0x04000A8D RID: 2701
		private bool firstShow = true;

		// Token: 0x04000A8E RID: 2702
		private DataTable dt;

		// Token: 0x04000A8F RID: 2703
		private DataView dv;
	}
}
