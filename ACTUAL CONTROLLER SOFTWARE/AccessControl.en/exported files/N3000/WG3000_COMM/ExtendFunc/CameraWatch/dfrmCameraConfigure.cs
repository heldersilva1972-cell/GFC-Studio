using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.CameraWatch
{
	// Token: 0x0200022D RID: 557
	public partial class dfrmCameraConfigure : frmN3000
	{
		// Token: 0x0600109A RID: 4250 RVA: 0x0012DA7E File Offset: 0x0012CA7E
		public dfrmCameraConfigure()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0012DA8C File Offset: 0x0012CA8C
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0012DA9C File Offset: 0x0012CA9C
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.txtCameraName.Text = this.txtCameraName.Text.Trim();
			if (string.IsNullOrEmpty(this.txtCameraName.Text))
			{
				XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.txtCameraIP.Text = this.txtCameraIP.Text.Trim();
			if (string.IsNullOrEmpty(this.txtCameraIP.Text))
			{
				XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.UpdateRecord())
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
				return;
			}
			XMessageBox.Show(this, CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0012DB57 File Offset: 0x0012CB57
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			this.txtPassword.UseSystemPasswordChar = !this.checkBox1.Checked;
			wgAppConfig.UpdateKeyVal("KEY_CameraDisplayPassword", this.checkBox1.Checked ? "1" : "0");
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0012DB98 File Offset: 0x0012CB98
		private void dfrmCameraConfigure_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.GetKeyVal("KEY_CameraDisplayPassword") == "0")
			{
				this.checkBox1.Checked = false;
			}
			else
			{
				this.checkBox1.Checked = true;
			}
			this.checkBox1_CheckedChanged(null, null);
			this.cboChannel.Visible = false;
			if (this.cameraID > 0)
			{
				try
				{
					string text = " SELECT ";
					text = text + " t_b_Camera.f_CameraId , t_b_Camera.f_CameraName , t_b_Camera.f_CameraIP , t_b_Camera.f_CameraPort , t_b_Camera.f_CameraChannel, t_b_Camera.f_CameraUser, t_b_Camera.f_CameraPassword, t_b_Camera.f_Enabled , t_b_Camera.f_Notes   from t_b_Camera   Where f_CameraID = " + this.cameraID.ToString();
					DataTable dataTable = new DataTable();
					new DataView(dataTable);
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
							goto IL_011D;
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
					IL_011D:
					if (dataTable.Rows.Count > 0)
					{
						this.txtCameraName.Text = (string)dataTable.Rows[0]["f_CameraName"];
						this.txtCameraIP.Text = wgTools.SetObjToStr(dataTable.Rows[0]["f_CameraIP"]);
						this.nudCameraPort.Value = (int)dataTable.Rows[0]["f_CameraPort"];
						this.nudChannel.Value = (int)dataTable.Rows[0]["f_CameraChannel"] % 100000;
						switch (((int)dataTable.Rows[0]["f_CameraChannel"] - (int)this.nudChannel.Value) / 100000)
						{
						case 1:
							this.radioButton1.Checked = true;
							if (this.nudChannel.Value > 32m)
							{
								this.nudChannel.Value -= 32m;
							}
							break;
						case 2:
							this.radioButton2.Checked = true;
							break;
						case 3:
							this.radioButton3.Checked = true;
							break;
						case 4:
							this.radioButton4.Checked = true;
							break;
						default:
							this.radioButton0.Checked = true;
							break;
						}
						this.txtNote.Text = wgTools.SetObjToStr(dataTable.Rows[0]["f_Notes"]);
						this.txtUser.Text = wgTools.SetObjToStr(dataTable.Rows[0]["f_CameraUser"]);
						this.txtPassword.Text = wgTools.SetObjToStr(dataTable.Rows[0]["f_CameraPassword"]);
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
				}
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0012DF78 File Offset: 0x0012CF78
		private void groupBox1_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0012DF7C File Offset: 0x0012CF7C
		private bool UpdateRecord()
		{
			DbConnection dbConnection = null;
			bool flag = false;
			try
			{
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
				string text = string.Format("Select * from t_b_camera where f_cameraname ={0} and f_cameraid <>{1}", wgTools.PrepareStrNUnicode(this.txtCameraName.Text), this.cameraID);
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				int num = 0;
				if (dbDataReader.Read())
				{
					num = 1;
				}
				dbDataReader.Close();
				if (num > 0)
				{
					XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return flag;
				}
				int num2 = (int)this.nudChannel.Value;
				if (this.radioButton1.Checked)
				{
					num2 += 100000;
					num2 += 32;
				}
				else if (this.radioButton2.Checked)
				{
					num2 += 200000;
				}
				else if (this.radioButton3.Checked)
				{
					num2 += 300000;
				}
				else if (this.radioButton4.Checked)
				{
					num2 += 400000;
				}
				if (this.cameraID > 0)
				{
					text = "UPDATE t_b_Camera  SET  ";
					text = string.Concat(new string[]
					{
						text,
						" f_CameraName =",
						wgTools.PrepareStrNUnicode(this.txtCameraName.Text),
						",f_CameraIP = ",
						wgTools.PrepareStrNUnicode(this.txtCameraIP.Text),
						",f_CameraPort = ",
						this.nudCameraPort.Value.ToString(),
						",f_CameraChannel = ",
						num2.ToString(),
						",f_Enabled = ",
						(this.chkActive.Checked ? 1 : 0).ToString(),
						",f_CameraUser = ",
						wgTools.PrepareStrNUnicode(this.txtUser.Text),
						",f_CameraPassword = ",
						wgTools.PrepareStrNUnicode(this.txtPassword.Text),
						",f_Notes = ",
						wgTools.PrepareStrNUnicode(this.txtNote.Text),
						" WHERE f_CameraID = ",
						this.cameraID.ToString()
					});
				}
				else
				{
					text = "Insert Into t_b_Camera(f_CameraName,f_CameraIP,f_CameraPort,f_CameraChannel,f_Enabled,f_Notes, f_CameraUser, f_CameraPassword) ";
					text = string.Concat(new object[]
					{
						text,
						" Values(",
						wgTools.PrepareStrNUnicode(this.txtCameraName.Text),
						",",
						wgTools.PrepareStrNUnicode(this.txtCameraIP.Text),
						",",
						this.nudCameraPort.Value.ToString(),
						",",
						num2.ToString(),
						",",
						this.chkActive.Checked ? 1 : 0,
						",",
						wgTools.PrepareStrNUnicode(this.txtNote.Text),
						", ",
						wgTools.PrepareStrNUnicode(this.txtUser.Text),
						", ",
						wgTools.PrepareStrNUnicode(this.txtPassword.Text),
						")"
					});
				}
				dbCommand.CommandText = text;
				dbCommand.ExecuteNonQuery();
				dbConnection.Close();
				flag = true;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			finally
			{
				if (dbConnection != null && dbConnection.State == ConnectionState.Open)
				{
					dbConnection.Close();
				}
			}
			return flag;
		}

		// Token: 0x04001D81 RID: 7553
		public int cameraID;
	}
}
