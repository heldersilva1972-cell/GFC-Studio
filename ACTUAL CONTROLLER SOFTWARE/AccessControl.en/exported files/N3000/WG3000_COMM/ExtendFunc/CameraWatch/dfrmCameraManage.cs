using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.CameraWatch
{
	// Token: 0x0200022E RID: 558
	public partial class dfrmCameraManage : frmN3000
	{
		// Token: 0x060010A3 RID: 4259 RVA: 0x0012EF72 File Offset: 0x0012DF72
		public dfrmCameraManage()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvCameras);
			wgAppConfig.custDataGridview(ref this.dgvReaderCamera);
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0012EF98 File Offset: 0x0012DF98
		private void _fillCameraGrid()
		{
			try
			{
				string text = " SELECT ";
				text += " t_b_Camera.f_CameraId , t_b_Camera.f_CameraName , t_b_Camera.f_CameraIP , t_b_Camera.f_CameraPort , t_b_Camera.f_CameraChannel, t_b_Camera.f_Notes   from t_b_Camera   ORDER BY t_b_Camera.f_CameraName  ";
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
				DataGridView dataGridView = this.dgvCameras;
				dataGridView.AutoGenerateColumns = false;
				dataGridView.DataSource = dataView;
				int num = 0;
				while (num < dataView.Table.Columns.Count && num < dataGridView.ColumnCount)
				{
					dataGridView.Columns[num].DataPropertyName = dataView.Table.Columns[num].ColumnName;
					dataGridView.Columns[num].Name = dataView.Table.Columns[num].ColumnName;
					num++;
				}
				if (dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						int num2 = (int)dataTable.Rows[i]["f_CameraChannel"] % 100000;
						int num3 = ((int)dataTable.Rows[i]["f_CameraChannel"] - num2) / 100000;
						if (num3 == 1 && num2 > 32)
						{
							num2 -= 32;
						}
						dataTable.Rows[i]["f_CameraChannel"] = num2;
					}
				}
				dataGridView.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0012F27C File Offset: 0x0012E27C
		private void _fillReaderCameraGrid()
		{
			try
			{
				string text = "  Select t_b_Reader.f_ReaderID, t_b_Reader.f_ReaderName, t_b_CameraTriggerSource.f_CameraId,  t_b_Camera.f_CameraName  ";
				text += " from ((t_b_reader  LEFT JOIN t_b_CameraTriggerSource ON (t_b_Reader.f_ReaderID = t_b_CameraTriggerSource.f_ReaderID ))  LEFT JOIN t_b_Camera on (t_b_CameraTriggerSource.f_CameraID=t_b_Camera.f_CameraID)) INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  ";
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
				DataGridView dataGridView = this.dgvReaderCamera;
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

		// Token: 0x060010A6 RID: 4262 RVA: 0x0012F4B4 File Offset: 0x0012E4B4
		private void autoSelectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.setSystemParamValue(176, "Video_Disable JPEGCaputure", "0", "Camera View-2015-04-16 13:50:13");
			this.updateJPEGCaptureMode();
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0012F4D8 File Offset: 0x0012E4D8
		private void btnAdd_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvCameras.Rows.Count > 0)
			{
				try
				{
					num = this.dgvCameras.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			using (dfrmCameraConfigure dfrmCameraConfigure = new dfrmCameraConfigure())
			{
				if (dfrmCameraConfigure.ShowDialog() == DialogResult.OK)
				{
					this._fillCameraGrid();
				}
			}
			if (this.dgvCameras.RowCount > 0)
			{
				if (this.dgvCameras.RowCount > num)
				{
					this.dgvCameras.CurrentCell = this.dgvCameras[1, num];
					return;
				}
				this.dgvCameras.CurrentCell = this.dgvCameras[1, this.dgvCameras.RowCount - 1];
			}
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0012F5A8 File Offset: 0x0012E5A8
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0012F5B0 File Offset: 0x0012E5B0
		private void btnDel_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvCameras.Rows.Count > 0)
			{
				try
				{
					num = this.dgvCameras.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num2;
			if (this.dgvCameras.SelectedRows.Count <= 0)
			{
				if (this.dgvCameras.SelectedCells.Count <= 0)
				{
					return;
				}
				num2 = this.dgvCameras.SelectedCells[0].RowIndex;
			}
			else
			{
				num2 = this.dgvCameras.SelectedRows[0].Index;
			}
			string text = string.Format("{0}  \"{1}\"", this.btnDel.Text, this.dgvCameras.Rows[num2].Cells[1].Value.ToString());
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_Camera WHERE [f_CameraId]= " + this.dgvCameras.Rows[num2].Cells[0].Value.ToString());
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_CameraTriggerSource WHERE [f_CameraId]= " + this.dgvCameras.Rows[num2].Cells[0].Value.ToString());
				this._fillCameraGrid();
				this._fillReaderCameraGrid();
				if (this.dgvCameras.RowCount > 0)
				{
					if (this.dgvCameras.RowCount > num)
					{
						this.dgvCameras.CurrentCell = this.dgvCameras[1, num];
						return;
					}
					this.dgvCameras.CurrentCell = this.dgvCameras[1, this.dgvCameras.RowCount - 1];
				}
			}
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0012F784 File Offset: 0x0012E784
		private void btnEdit_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvCameras.Rows.Count > 0)
			{
				try
				{
					num = this.dgvCameras.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num2;
			if (this.dgvCameras.SelectedRows.Count <= 0)
			{
				if (this.dgvCameras.SelectedCells.Count <= 0)
				{
					return;
				}
				num2 = this.dgvCameras.SelectedCells[0].RowIndex;
			}
			else
			{
				num2 = this.dgvCameras.SelectedRows[0].Index;
			}
			using (dfrmCameraConfigure dfrmCameraConfigure = new dfrmCameraConfigure())
			{
				dfrmCameraConfigure.cameraID = (int)this.dgvCameras.Rows[num2].Cells["f_CameraID"].Value;
				if (dfrmCameraConfigure.ShowDialog() == DialogResult.OK)
				{
					this._fillCameraGrid();
					this._fillReaderCameraGrid();
				}
			}
			if (this.dgvCameras.RowCount > 0)
			{
				if (this.dgvCameras.RowCount > num)
				{
					this.dgvCameras.CurrentCell = this.dgvCameras[1, num];
					return;
				}
				this.dgvCameras.CurrentCell = this.dgvCameras[1, this.dgvCameras.RowCount - 1];
			}
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0012F8E0 File Offset: 0x0012E8E0
		private void btnEditReaderCamera_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.dgvReaderCamera.Rows.Count > 0)
			{
				try
				{
					num = this.dgvReaderCamera.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num2;
			if (this.dgvReaderCamera.SelectedRows.Count <= 0)
			{
				if (this.dgvReaderCamera.SelectedCells.Count <= 0)
				{
					return;
				}
				num2 = this.dgvReaderCamera.SelectedCells[0].RowIndex;
			}
			else
			{
				num2 = this.dgvReaderCamera.SelectedRows[0].Index;
			}
			using (dfrmCameraSelect dfrmCameraSelect = new dfrmCameraSelect())
			{
				if (!string.IsNullOrEmpty(wgTools.SetObjToStr(this.dgvReaderCamera.Rows[num2].Cells["f_CameraID"].Value)))
				{
					dfrmCameraSelect.cameraID = (int)this.dgvReaderCamera.Rows[num2].Cells["f_CameraID"].Value;
				}
				dfrmCameraSelect.readerID = (int)this.dgvReaderCamera.Rows[num2].Cells["f_ReaderID"].Value;
				if (dfrmCameraSelect.ShowDialog() == DialogResult.OK)
				{
					this._fillCameraGrid();
					this._fillReaderCameraGrid();
				}
			}
			if (this.dgvReaderCamera.RowCount > 0)
			{
				if (this.dgvReaderCamera.RowCount > num)
				{
					this.dgvReaderCamera.CurrentCell = this.dgvReaderCamera[1, num];
					return;
				}
				this.dgvReaderCamera.CurrentCell = this.dgvReaderCamera[1, this.dgvReaderCamera.RowCount - 1];
			}
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0012FA9C File Offset: 0x0012EA9C
		private void chkAutoSyncTime_CheckedChanged(object sender, EventArgs e)
		{
			wgAppConfig.setSystemParamValue(158, "Video_AutoAdjustTimeOfVideo", this.chkAutoSyncTime.Checked ? "1" : "0", "Camera View-20131019");
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0012FACC File Offset: 0x0012EACC
		private void chkDeleteOldAviJpg_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkDeleteOldAviJpg.Checked)
			{
				wgAppConfig.UpdateKeyVal("KEY_Video_DELETE_AVI_TIME_FREQ_MONTH", this.comboBox1.Text);
				this.comboBox1.Visible = true;
				this.label1.Visible = true;
				return;
			}
			wgAppConfig.UpdateKeyVal("KEY_Video_DELETE_AVI_TIME_FREQ_MONTH", "0");
			this.comboBox1.Visible = false;
			this.label1.Visible = false;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0012FB3C File Offset: 0x0012EB3C
		private void clientDemoToolToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				string startupPath = Application.StartupPath;
				Process.Start("ClientDemo.exe", startupPath);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0012FB7C File Offset: 0x0012EB7C
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.chkDeleteOldAviJpg.Checked)
			{
				wgAppConfig.UpdateKeyVal("KEY_Video_DELETE_AVI_TIME_FREQ_MONTH", this.comboBox1.Text);
				return;
			}
			wgAppConfig.UpdateKeyVal("KEY_Video_DELETE_AVI_TIME_FREQ_MONTH", "0");
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0012FBB0 File Offset: 0x0012EBB0
		private void dfrmCameraManage_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.chkAutoSyncTime.Visible = true;
				this.groupBox2.Visible = true;
				this.groupBox3.Visible = true;
				this.groupBox4.Visible = true;
				base.Size = new Size(base.Width, Math.Max(720, this.groupBox3.Location.Y + this.groupBox3.Height + 60));
				this.ContextMenuStrip = this.contextMenuStrip1;
			}
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0012FC78 File Offset: 0x0012EC78
		private void dfrmCameraManage_Load(object sender, EventArgs e)
		{
			this._fillCameraGrid();
			this._fillReaderCameraGrid();
			if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(157)) == "1")
			{
				this.optOnlyCapture.Checked = true;
			}
			else if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(157)) == "0")
			{
				this.optBoth.Checked = true;
			}
			else
			{
				this.optNotPreview.Checked = true;
				if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(157)) != "2")
				{
					wgAppConfig.setSystemParamValue(157, "Video_OnlyCapturePhoto", "2", "Camera View-20131019");
				}
			}
			if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(158)) == "0")
			{
				this.chkAutoSyncTime.Checked = false;
				this.chkAutoSyncTime.Visible = true;
			}
			else
			{
				this.chkAutoSyncTime.Checked = true;
				this.chkAutoSyncTime.Visible = false;
			}
			if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(159)) == "0")
			{
				this.optResolution0.Checked = true;
			}
			else if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(159)) == "1")
			{
				this.optResolution1.Checked = true;
			}
			else
			{
				this.optResolution2.Checked = true;
				if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(159)) != "2" && !string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(159))))
				{
					this.optResolution2.Checked = false;
					this.specialSetToolStripMenuItem.Checked = true;
				}
			}
			if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(160)) == "2")
			{
				this.optQuality2.Checked = true;
			}
			else if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(160)) == "1")
			{
				this.optQuality1.Checked = true;
			}
			else
			{
				this.optQuality0.Checked = true;
			}
			this.dontCaputreOnThisPCToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_Video_DontCaputreOnThisPC") == "1";
			if (!string.IsNullOrEmpty(wgAppConfig.getSystemParamNotes(42)) || !string.IsNullOrEmpty(wgAppConfig.getSystemParamNotes(41)) || wgAppConfig.GetKeyVal("KEY_Video_DontCaputreOnThisPC") == "1")
			{
				this.ContextMenuStrip = this.contextMenuStrip1;
			}
			if (!this.optQuality0.Checked || !this.optResolution2.Checked)
			{
				base.Size = new Size(base.Width, Math.Max(720, this.groupBox3.Location.Y + this.groupBox3.Height + 60));
				this.groupBox2.Visible = true;
				this.groupBox3.Visible = true;
				this.groupBox4.Visible = true;
			}
			this.updateJPEGCaptureMode();
			int num = 0;
			try
			{
				int.TryParse(wgAppConfig.GetKeyVal("KEY_Video_DELETE_AVI_TIME_FREQ_MONTH"), out num);
				if (num > 0)
				{
					this.chkDeleteOldAviJpg.Checked = true;
					this.comboBox1.Text = num.ToString();
					this.comboBox1.Visible = true;
					this.label1.Visible = true;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x0012FFC4 File Offset: 0x0012EFC4
		private void dgvCameras_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x0012FFD1 File Offset: 0x0012EFD1
		private void dgvReaderCamera_DoubleClick(object sender, EventArgs e)
		{
			this.btnEditReaderCamera.PerformClick();
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0012FFE0 File Offset: 0x0012EFE0
		private void directoryInformationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			XMessageBox.Show(string.Concat(new string[]
			{
				this.setAVIJPGSaveDirectoryToolStripMenuItem.Text,
				"\r\n\r\n",
				wgAppConfig.Path4AviJpg(),
				"\r\n\r\n",
				this.setAVIJPGViewDirectoryToolStripMenuItem.Text,
				"\r\n\r\n",
				wgAppConfig.Path4AviJpgOnlyView(),
				"\r\n\r\n"
			}));
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0013004C File Offset: 0x0012F04C
		private void dontCaputreOnThisPCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.dontCaputreOnThisPCToolStripMenuItem.Checked = !this.dontCaputreOnThisPCToolStripMenuItem.Checked;
			wgAppConfig.UpdateKeyVal("KEY_Video_DontCaputreOnThisPC", this.dontCaputreOnThisPCToolStripMenuItem.Checked ? "1" : "0");
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0013008A File Offset: 0x0012F08A
		private void forcedDisableToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.setSystemParamValue(176, "Video_Disable JPEGCaputure", "1", "Camera View-2015-04-16 13:50:13");
			this.updateJPEGCaptureMode();
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x001300AC File Offset: 0x0012F0AC
		private void optBoth_CheckedChanged(object sender, EventArgs e)
		{
			if (this.optBoth.Checked)
			{
				wgAppConfig.setSystemParamValue(157, "Video_OnlyCapturePhoto", "0", "Camera View-20131019");
				return;
			}
			if (this.optOnlyCapture.Checked)
			{
				wgAppConfig.setSystemParamValue(157, "Video_OnlyCapturePhoto", "1", "Camera View-20131019");
				return;
			}
			wgAppConfig.setSystemParamValue(157, "Video_OnlyCapturePhoto", "2", "Camera View-20131019");
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00130124 File Offset: 0x0012F124
		private void optQuality0_CheckedChanged(object sender, EventArgs e)
		{
			if (this.optQuality2.Checked)
			{
				wgAppConfig.setSystemParamValue(160, "Video_JPEGQuality", "2", "Camera View-20131019");
				return;
			}
			if (this.optQuality1.Checked)
			{
				wgAppConfig.setSystemParamValue(160, "Video_JPEGQuality", "1", "Camera View-20131019");
				return;
			}
			wgAppConfig.setSystemParamValue(160, "Video_JPEGQuality", "0", "Camera View-20131019");
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0013019C File Offset: 0x0012F19C
		private void optResolution2_CheckedChanged(object sender, EventArgs e)
		{
			this.specialSetToolStripMenuItem.Checked = false;
			if (this.optResolution0.Checked)
			{
				wgAppConfig.setSystemParamValue(159, "Video_JPEGResolution", "0", "Camera View-20131019");
				return;
			}
			if (this.optResolution1.Checked)
			{
				wgAppConfig.setSystemParamValue(159, "Video_JPEGResolution", "1", "Camera View-20131019");
				return;
			}
			if (this.optResolution2.Checked)
			{
				wgAppConfig.setSystemParamValue(159, "Video_JPEGResolution", "2", "Camera View-20131019");
			}
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0013022C File Offset: 0x0012F22C
		private void restoreAVIJPGSaveDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.setSystemParamNotes(42, "");
			wgAppConfig.Path4AviJpgRefresh();
			wgAppConfig.Path4AviJpgOnlyViewRefresh();
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00130245 File Offset: 0x0012F245
		private void restoreAVIJPGViewDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.setSystemParamNotes(43, "");
			wgAppConfig.Path4AviJpgRefresh();
			wgAppConfig.Path4AviJpgOnlyViewRefresh();
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00130260 File Offset: 0x0012F260
		private void setAVIJPGSaveDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				this.folderBrowserDialog1.SelectedPath = wgAppConfig.Path4AviJpg();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				string selectedPath = this.folderBrowserDialog1.SelectedPath;
				if (!string.IsNullOrEmpty(selectedPath) && selectedPath.Trim().Length != Encoding.GetEncoding("utf-8").GetBytes(selectedPath.Trim()).Length)
				{
					XMessageBox.Show(this, CommonStr.strInvalidAviJpgDirectory, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				wgAppConfig.setSystemParamNotes(42, selectedPath.Trim());
			}
			wgAppConfig.Path4AviJpgRefresh();
			wgAppConfig.Path4AviJpgOnlyViewRefresh();
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00130310 File Offset: 0x0012F310
		private void setAVIJPGViewDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				this.folderBrowserDialog1.SelectedPath = wgAppConfig.Path4AviJpgOnlyView();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				string selectedPath = this.folderBrowserDialog1.SelectedPath;
				if (!string.IsNullOrEmpty(selectedPath) && selectedPath.Trim().Length != Encoding.GetEncoding("utf-8").GetBytes(selectedPath.Trim()).Length)
				{
					XMessageBox.Show(this, CommonStr.strInvalidAviJpgDirectory, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				wgAppConfig.setSystemParamNotes(43, selectedPath.Trim());
			}
			wgAppConfig.Path4AviJpgRefresh();
			wgAppConfig.Path4AviJpgOnlyViewRefresh();
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x001303C0 File Offset: 0x0012F3C0
		private void specialSetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripMenuItem).Text + ":  " + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(159));
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					string text = dfrmInputNewName.strNewName.Trim();
					if (!string.IsNullOrEmpty(text))
					{
						int num = -1;
						int.TryParse(text, out num);
						if (num > 0 && num <= 65535)
						{
							wgAppConfig.setSystemParamValue(159, "Video_JPEGResolution", num.ToString(), "Camera View-20131019");
							this.optResolution0.Checked = false;
							this.optResolution1.Checked = false;
							this.optResolution2.Checked = false;
							wgAppConfig.setSystemParamValue(159, "Video_JPEGResolution", num.ToString(), "Camera View-20131019");
							if (num <= 2)
							{
								this.specialSetToolStripMenuItem.Checked = false;
								if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(159)) == "0")
								{
									this.optResolution0.Checked = true;
								}
								else if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(159)) == "1")
								{
									this.optResolution1.Checked = true;
								}
								else
								{
									this.optResolution2.Checked = true;
									if (wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(159)) != "2")
									{
										this.optResolution2.Checked = false;
										this.specialSetToolStripMenuItem.Checked = true;
									}
								}
							}
							else
							{
								this.specialSetToolStripMenuItem.Checked = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x00130574 File Offset: 0x0012F574
		private void updateJPEGCaptureMode()
		{
			this.forcedDisableToolStripMenuItem.Checked = false;
			this.autoSelectToolStripMenuItem.Checked = false;
			if (wgAppConfig.getSystemParamByNO(176) == "1")
			{
				this.forcedDisableToolStripMenuItem.Checked = true;
				return;
			}
			this.autoSelectToolStripMenuItem.Checked = true;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x001305C8 File Offset: 0x0012F5C8
		private void videoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnView.Enabled = false;
			if (this.dgvCameras.Rows.Count > 0)
			{
				try
				{
					int rowIndex = this.dgvCameras.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			int num;
			if (this.dgvCameras.SelectedRows.Count <= 0)
			{
				if (this.dgvCameras.SelectedCells.Count <= 0)
				{
					this.btnView.Enabled = true;
					return;
				}
				num = this.dgvCameras.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvCameras.SelectedRows[0].Index;
			}
			int num2 = int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(157)));
			try
			{
				wgAppConfig.setSystemParamValue(157, "Video_OnlyCapturePhoto", "0", "Camera View-20131019");
				using (dfrmPhotoAviSingle dfrmPhotoAviSingle = new dfrmPhotoAviSingle())
				{
					dfrmPhotoAviSingle.selectedCameraID = ((int)this.dgvCameras.Rows[num].Cells["f_CameraID"].Value).ToString();
					dfrmPhotoAviSingle.ShowDialog(this);
					dfrmPhotoAviSingle.bWatching = false;
					dfrmPhotoAviSingle.stopVideo();
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				wgAppConfig.setSystemParamValue(157, "Video_OnlyCapturePhoto", num2.ToString(), "Camera View-20131019");
			}
			this.btnView.Enabled = true;
		}
	}
}
