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

namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x020002D1 RID: 721
	public partial class dfrmFaceDeviceManagement : frmN3000
	{
		// Token: 0x06001358 RID: 4952 RVA: 0x0016DE17 File Offset: 0x0016CE17
		public dfrmFaceDeviceManagement()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0016DE30 File Offset: 0x0016CE30
		private void _fillDeviceGrid()
		{
			try
			{
				string text = " SELECT ";
				text = text + " f_DeviceId , f_DeviceName  , f_DeviceType, f_DeviceIP , f_DevicePort , f_ReaderName , f_CardNOWorkNODiff , f_Notes   from t_b_ThirdPartyNetDevice LEFT JOIN t_b_Reader ON t_b_Reader.f_ReaderID = t_b_ThirdPartyNetDevice.f_ReaderID  WHERE f_factory =   " + wgTools.PrepareStrNUnicode(this.factoryName) + " ORDER BY f_DeviceName  ";
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
						goto IL_00DA;
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
				IL_00DA:
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

		// Token: 0x0600135A RID: 4954 RVA: 0x0016E078 File Offset: 0x0016D078
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
			using (dfrmFaceDeviceConfigure dfrmFaceDeviceConfigure = new dfrmFaceDeviceConfigure())
			{
				if (dfrmFaceDeviceConfigure.ShowDialog() == DialogResult.OK)
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

		// Token: 0x0600135B RID: 4955 RVA: 0x0016E148 File Offset: 0x0016D148
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0016E150 File Offset: 0x0016D150
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
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_ThirdPartyNetDevice WHERE [f_DeviceId]= " + this.dgvDevices.Rows[num2].Cells[0].Value.ToString());
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

		// Token: 0x0600135D RID: 4957 RVA: 0x0016E2E8 File Offset: 0x0016D2E8
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
			using (dfrmFaceDeviceConfigure dfrmFaceDeviceConfigure = new dfrmFaceDeviceConfigure())
			{
				dfrmFaceDeviceConfigure.cameraID = (int)this.dgvDevices.Rows[num2].Cells["f_DeviceID"].Value;
				if (dfrmFaceDeviceConfigure.ShowDialog() == DialogResult.OK)
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

		// Token: 0x0600135E RID: 4958 RVA: 0x0016E440 File Offset: 0x0016D440
		private void dfrmFaceDeviceManage_Load(object sender, EventArgs e)
		{
			this._fillDeviceGrid();
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0016E448 File Offset: 0x0016D448
		private void dgvDevices_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x0400297B RID: 10619
		public string factoryName = "Hanvon";
	}
}
