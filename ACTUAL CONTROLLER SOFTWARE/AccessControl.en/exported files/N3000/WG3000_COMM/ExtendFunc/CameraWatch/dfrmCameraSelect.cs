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

namespace WG3000_COMM.ExtendFunc.CameraWatch
{
	// Token: 0x0200022F RID: 559
	public partial class dfrmCameraSelect : frmN3000
	{
		// Token: 0x060010C3 RID: 4291 RVA: 0x0013203E File Offset: 0x0013103E
		public dfrmCameraSelect()
		{
			this.InitializeComponent();
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0013204C File Offset: 0x0013104C
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
				DataRow dataRow = dataTable.NewRow();
				dataRow["f_CameraID"] = 0;
				dataRow["f_CameraName"] = " ";
				dataTable.Rows.Add(dataRow);
				dataTable.AcceptChanges();
				dataView.Sort = "f_CameraName ASC";
				this.cboCamera.DataSource = dataView;
				this.cboCamera.DisplayMember = "f_CameraName";
				string text2 = "";
				if (this.cameraID > 0)
				{
					dataView.RowFilter = "f_CameraID = " + this.cameraID.ToString();
					if (dataView.Count > 0)
					{
						this.cboCamera.SelectedItem = dataView[0];
						text2 = (string)dataView[0]["f_CameraName"];
					}
				}
				dataView.RowFilter = "";
				if (!string.IsNullOrEmpty(text2))
				{
					this.cboCamera.Text = text2;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x001322C4 File Offset: 0x001312C4
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x001322D4 File Offset: 0x001312D4
		private void cmdOK_Click(object sender, EventArgs e)
		{
			try
			{
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_CameraTriggerSource WHERE [f_ReaderId]= " + this.readerID.ToString());
				if (!string.IsNullOrEmpty(this.cboCamera.Text.Trim()))
				{
					wgAppConfig.runUpdateSql(string.Concat(new string[]
					{
						" INSERT INTO t_b_CameraTriggerSource( [f_ReaderId],[f_CameraID])  VALUES (",
						this.readerID.ToString(),
						" ,  ",
						(this.cboCamera.SelectedItem as DataRowView)["f_CameraID"].ToString(),
						")"
					}));
				}
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0013239C File Offset: 0x0013139C
		private void dfrmCameraSelect_Load(object sender, EventArgs e)
		{
			this._fillCameraGrid();
		}

		// Token: 0x04001DD4 RID: 7636
		public int cameraID;

		// Token: 0x04001DD5 RID: 7637
		public int readerID;
	}
}
