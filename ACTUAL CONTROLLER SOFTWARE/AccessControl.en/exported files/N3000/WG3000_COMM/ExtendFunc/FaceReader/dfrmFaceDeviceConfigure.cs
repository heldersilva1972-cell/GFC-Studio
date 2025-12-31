using System;
using System.Collections.Generic;
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

namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x020002D0 RID: 720
	public partial class dfrmFaceDeviceConfigure : frmN3000
	{
		// Token: 0x0600134F RID: 4943 RVA: 0x0016C5F7 File Offset: 0x0016B5F7
		public dfrmFaceDeviceConfigure()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0016C628 File Offset: 0x0016B628
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

		// Token: 0x06001351 RID: 4945 RVA: 0x0016C6E3 File Offset: 0x0016B6E3
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			this.txtPassword.UseSystemPasswordChar = !this.checkBox1.Checked;
			wgAppConfig.UpdateKeyVal("KEY_FaceDeviceDisplayPassword", this.checkBox1.Checked ? "1" : "0");
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0016C724 File Offset: 0x0016B724
		private void dfrmDeviceConfigure_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.GetKeyVal("KEY_FaceDeviceDisplayPassword") == "0")
			{
				this.checkBox1.Checked = false;
			}
			else
			{
				this.checkBox1.Checked = true;
			}
			this.checkBox1_CheckedChanged(null, null);
			this.nudCameraPort.Value = 9922m;
			switch (int.Parse("0" + wgAppConfig.getSystemParamByNO(209)))
			{
			case 1:
			{
				string systemParamNotes = wgAppConfig.getSystemParamNotes(209);
				this.nudCameraPort.Value = decimal.Parse(systemParamNotes.Substring(0, systemParamNotes.IndexOf(",")));
				this.txtCameraIP.Text = "";
				break;
			}
			case 2:
			{
				string systemParamNotes2 = wgAppConfig.getSystemParamNotes(209);
				this.nudCameraPort.Value = decimal.Parse(systemParamNotes2.Substring(0, systemParamNotes2.IndexOf(",")));
				this.txtCameraIP.Text = "";
				break;
			}
			case 3:
			{
				string systemParamNotes3 = wgAppConfig.getSystemParamNotes(209);
				this.nudCameraPort.Value = decimal.Parse(systemParamNotes3.Substring(0, systemParamNotes3.IndexOf(",")));
				this.txtCameraIP.Text = "";
				break;
			}
			}
			try
			{
				string text = " SELECT a.f_ReaderNO, a.f_ReaderName , b.f_ControllerSN, a.f_ReaderID ";
				text += " FROM t_b_Reader a, t_b_Controller b WHERE  b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID ";
				DataTable dataTable = new DataTable();
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
						goto IL_0203;
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
				IL_0203:
				if (dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						this.ReaderName.Add(string.Format("{0}-{1}", dataTable.Rows[i]["f_ControllerSN"].ToString(), dataTable.Rows[i]["f_ReaderNO"].ToString()), dataTable.Rows[i]["f_ReaderName"].ToString());
						this.ReaderID.Add(string.Format("{0}-{1}", dataTable.Rows[i]["f_ControllerSN"].ToString(), dataTable.Rows[i]["f_ReaderNO"].ToString()), dataTable.Rows[i]["f_ReaderID"].ToString());
					}
				}
				this.cboReader.DataSource = dataTable;
				this.cboReader.DisplayMember = "f_ReaderName";
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			if (this.cameraID > 0)
			{
				try
				{
					string text = " SELECT ";
					text = text + " f_DeviceId , f_DeviceName , f_DeviceIP , f_DevicePort , f_DeviceChannel, f_DeviceUser, f_DevicePassword, f_DeviceType, f_Enabled , f_CardNOWorkNODiff , f_ReaderID , f_Notes   from t_b_ThirdPartyNetDevice   Where f_DeviceID = " + this.cameraID.ToString();
					DataTable dataTable2 = new DataTable();
					new DataView(dataTable2);
					if (wgAppConfig.IsAccessDB)
					{
						using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
						{
							using (OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2))
							{
								using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(oleDbCommand2))
								{
									oleDbDataAdapter2.Fill(dataTable2);
								}
							}
							goto IL_0422;
						}
					}
					using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
						{
							using (SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sqlCommand2))
							{
								sqlDataAdapter2.Fill(dataTable2);
							}
						}
					}
					IL_0422:
					if (dataTable2.Rows.Count > 0)
					{
						this.txtCameraName.Text = (string)dataTable2.Rows[0]["f_DeviceName"];
						this.txtCameraIP.Text = wgTools.SetObjToStr(dataTable2.Rows[0]["f_DeviceIP"]);
						this.nudCameraPort.Value = (int)dataTable2.Rows[0]["f_DevicePort"];
						this.txtNote.Text = wgTools.SetObjToStr(dataTable2.Rows[0]["f_Notes"]);
						this.txtUser.Text = wgTools.SetObjToStr(dataTable2.Rows[0]["f_DeviceUser"]);
						this.txtPassword.Text = wgTools.SetObjToStr(dataTable2.Rows[0]["f_DevicePassword"]);
						this.txtDeviceType.Text = wgTools.SetObjToStr(dataTable2.Rows[0]["f_DeviceType"]);
						if (wgTools.SetObjToStr(dataTable2.Rows[0]["f_CardNOWorkNODiff"]) == "0")
						{
							this.radioButton1.Checked = true;
						}
						else
						{
							this.radioButton2.Checked = true;
							long num = 0L;
							long.TryParse(wgTools.SetObjToStr(dataTable2.Rows[0]["f_CardNOWorkNODiff"]), out num);
							this.numericUpDown1.Value = num;
							this.groupBox1.Visible = true;
						}
						int num2 = (int)dataTable2.Rows[0]["f_ReaderID"];
						DataTable dataTable3 = this.cboReader.DataSource as DataTable;
						for (int j = 0; j < dataTable3.Rows.Count; j++)
						{
							if ((int)dataTable3.Rows[j]["f_ReaderID"] == num2)
							{
								this.cboReader.SelectedIndex = j;
								break;
							}
						}
					}
					this.radioButton2_CheckedChanged(null, null);
				}
				catch (Exception ex2)
				{
					wgAppConfig.wgDebugWrite(ex2.ToString(), EventLogEntryType.Error);
				}
			}
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0016CEF0 File Offset: 0x0016BEF0
		private void dfrmFaceDeviceConfigure_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.KeyValue == 81)
				{
					this.groupBox1.Visible = true;
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0016CF3C File Offset: 0x0016BF3C
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			this.numericUpDown1.Enabled = this.radioButton2.Checked;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0016CF54 File Offset: 0x0016BF54
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
				string text = string.Format("Select * from t_b_ThirdPartyNetDevice where f_Devicename ={0} and f_Deviceid <>{1} and f_factory = {2}", wgTools.PrepareStrNUnicode(this.txtCameraName.Text), this.cameraID, wgTools.PrepareStrNUnicode(this.factoryName));
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
				int num2 = 1;
				int num3 = 1;
				long num4 = 0L;
				if (this.radioButton2.Checked)
				{
					num4 = long.Parse(this.numericUpDown1.Value.ToString());
				}
				if ((this.cboReader.DataSource as DataTable).Rows.Count > 0 && this.cboReader.SelectedIndex >= 0)
				{
					num3 = (int)(this.cboReader.DataSource as DataTable).Rows[this.cboReader.SelectedIndex]["f_ReaderID"];
				}
				if (this.cameraID > 0)
				{
					text = "UPDATE t_b_ThirdPartyNetDevice  SET  ";
					text = string.Concat(new string[]
					{
						text,
						" f_DeviceName =",
						wgTools.PrepareStrNUnicode(this.txtCameraName.Text),
						",f_DeviceIP = ",
						wgTools.PrepareStrNUnicode(this.txtCameraIP.Text),
						",f_DevicePort = ",
						this.nudCameraPort.Value.ToString(),
						",f_DeviceChannel = ",
						num2.ToString(),
						",f_Enabled = ",
						(this.chkActive.Checked ? 1 : 0).ToString(),
						",f_DeviceUser = ",
						wgTools.PrepareStrNUnicode(this.txtUser.Text),
						",f_DevicePassword = ",
						wgTools.PrepareStrNUnicode(this.txtPassword.Text),
						",f_DeviceType = ",
						wgTools.PrepareStrNUnicode(this.txtDeviceType.Text),
						",f_Factory = ",
						wgTools.PrepareStrNUnicode(this.factoryName),
						",f_CardNOWorkNODiff = ",
						wgTools.PrepareStrNUnicode(num4.ToString()),
						",f_ReaderID = ",
						num3.ToString(),
						",f_Notes = ",
						wgTools.PrepareStrNUnicode(this.txtNote.Text),
						" WHERE f_DeviceID = ",
						this.cameraID.ToString()
					});
				}
				else
				{
					text = "Insert Into t_b_ThirdPartyNetDevice(f_DeviceName,f_DeviceIP,f_DevicePort,f_DeviceChannel,f_Enabled,f_Notes, f_DeviceUser, f_DevicePassword,f_DeviceType,f_Factory,f_CardNOWorkNODiff, f_ReaderID) ";
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
						", ",
						wgTools.PrepareStrNUnicode(this.txtDeviceType.Text),
						", ",
						wgTools.PrepareStrNUnicode(this.factoryName),
						", ",
						wgTools.PrepareStrNUnicode(num4.ToString()),
						", ",
						num3.ToString(),
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

		// Token: 0x0400295C RID: 10588
		public int cameraID;

		// Token: 0x0400295D RID: 10589
		public string factoryName = "Hanvon";

		// Token: 0x0400295E RID: 10590
		private Dictionary<string, string> ReaderID = new Dictionary<string, string>();

		// Token: 0x0400295F RID: 10591
		private Dictionary<string, string> ReaderName = new Dictionary<string, string>();
	}
}
