using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002EA RID: 746
	public partial class dfrmFingerPrintDeviceManagement : frmN3000
	{
		// Token: 0x0600155A RID: 5466 RVA: 0x001A7BE8 File Offset: 0x001A6BE8
		public dfrmFingerPrintDeviceManagement()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x001A7BF8 File Offset: 0x001A6BF8
		private void _fillDeviceGrid()
		{
			try
			{
				string text = " SELECT ";
				text += " t_b_Controller_FingerPrint.f_ControllerID , f_FingerPrintName , f_ControllerSN , f_IP , f_Port , f_ReaderName , f_Notes   from t_b_Controller_FingerPrint LEFT JOIN t_b_Reader ON t_b_Reader.f_ReaderID = t_b_Controller_FingerPrint.f_ReaderID  ORDER BY f_FingerPrintName  ";
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
				DataGridView dataGridView = this.dgvDevices;
				dataGridView.AutoGenerateColumns = false;
				dataGridView.DataSource = dataView;
				int num = 0;
				while (num < dataView.Table.Columns.Count && num < dataGridView.ColumnCount)
				{
					dataGridView.Columns[num].DataPropertyName = dataView.Table.Columns[num].ColumnName;
					dataGridView.Columns[num].Name = dataView.Table.Columns[num].ColumnName;
					num++;
				}
				dataGridView.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x001A7E30 File Offset: 0x001A6E30
		private void btnAdd_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvDevices.Rows.Count > 0)
			{
				try
				{
					num = this.dgvDevices.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			using (dfrmFingerPrintConfigure dfrmFingerPrintConfigure = new dfrmFingerPrintConfigure())
			{
				if (dfrmFingerPrintConfigure.ShowDialog() == DialogResult.OK)
				{
					this._fillDeviceGrid();
				}
			}
			if (this.dgvDevices.RowCount > 0)
			{
				if (this.dgvDevices.RowCount > num)
				{
					this.dgvDevices.CurrentCell = this.dgvDevices[1, num];
					return;
				}
				this.dgvDevices.CurrentCell = this.dgvDevices[1, this.dgvDevices.RowCount - 1];
			}
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x001A7F00 File Offset: 0x001A6F00
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x001A7F08 File Offset: 0x001A6F08
		private void btnDel_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvDevices.Rows.Count > 0)
			{
				try
				{
					num = this.dgvDevices.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num2;
			if (this.dgvDevices.SelectedRows.Count <= 0)
			{
				if (this.dgvDevices.SelectedCells.Count <= 0)
				{
					return;
				}
				num2 = this.dgvDevices.SelectedCells[0].RowIndex;
			}
			else
			{
				num2 = this.dgvDevices.SelectedRows[0].Index;
			}
			string text = string.Format("{0}  \"{1}\"", this.btnDel.Text, this.dgvDevices.Rows[num2].Cells[1].Value.ToString());
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_Controller_FingerPrint WHERE [f_ControllerId]= " + this.dgvDevices.Rows[num2].Cells[0].Value.ToString());
				this._fillDeviceGrid();
				if (this.dgvDevices.RowCount > 0)
				{
					if (this.dgvDevices.RowCount > num)
					{
						this.dgvDevices.CurrentCell = this.dgvDevices[1, num];
						return;
					}
					this.dgvDevices.CurrentCell = this.dgvDevices[1, this.dgvDevices.RowCount - 1];
				}
			}
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x001A80A0 File Offset: 0x001A70A0
		private void btnEdit_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvDevices.Rows.Count > 0)
			{
				try
				{
					num = this.dgvDevices.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num2;
			if (this.dgvDevices.SelectedRows.Count <= 0)
			{
				if (this.dgvDevices.SelectedCells.Count <= 0)
				{
					return;
				}
				num2 = this.dgvDevices.SelectedCells[0].RowIndex;
			}
			else
			{
				num2 = this.dgvDevices.SelectedRows[0].Index;
			}
			using (dfrmFingerPrintConfigure dfrmFingerPrintConfigure = new dfrmFingerPrintConfigure())
			{
				dfrmFingerPrintConfigure.cameraID = (int)this.dgvDevices.Rows[num2].Cells["f_ControllerID"].Value;
				if (dfrmFingerPrintConfigure.ShowDialog() == DialogResult.OK)
				{
					this._fillDeviceGrid();
				}
			}
			if (this.dgvDevices.RowCount > 0)
			{
				if (this.dgvDevices.RowCount > num)
				{
					this.dgvDevices.CurrentCell = this.dgvDevices[1, num];
					return;
				}
				this.dgvDevices.CurrentCell = this.dgvDevices[1, this.dgvDevices.RowCount - 1];
			}
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x001A81F8 File Offset: 0x001A71F8
		private void btnSearch_Click(object sender, EventArgs e)
		{
			using (dfrmNetControllerConfig4FingerPrint dfrmNetControllerConfig4FingerPrint = new dfrmNetControllerConfig4FingerPrint())
			{
				dfrmNetControllerConfig4FingerPrint.ShowDialog(this);
				this._fillDeviceGrid();
			}
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x001A8238 File Offset: 0x001A7238
		private void dfrmFaceDeviceManage_Load(object sender, EventArgs e)
		{
			this._fillDeviceGrid();
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x001A8240 File Offset: 0x001A7240
		private void dgvDevices_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}
	}
}
