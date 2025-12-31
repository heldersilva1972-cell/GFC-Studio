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

namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002E9 RID: 745
	public partial class dfrmFingerPrintConfigure : frmN3000
	{
		// Token: 0x0600154F RID: 5455 RVA: 0x001A6726 File Offset: 0x001A5726
		public dfrmFingerPrintConfigure()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x001A674C File Offset: 0x001A574C
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.mtxtbControllerSN.Text = this.mtxtbControllerSN.Text.Replace(" ", "");
			int num;
			if (!int.TryParse(this.mtxtbControllerSN.Text, out num))
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (!wgMjController.IsFingerController(int.Parse(this.mtxtbControllerSN.Text)))
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.optIPLarge.Checked && string.IsNullOrEmpty(this.txtControllerIP.Text))
			{
				XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.txtCameraName.Text = this.txtCameraName.Text.Trim();
			if (string.IsNullOrEmpty(this.txtCameraName.Text))
			{
				XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.txtControllerIP.Text = this.txtControllerIP.Text.Trim();
			if (this.UpdateRecord())
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
				return;
			}
			XMessageBox.Show(this, CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x001A6890 File Offset: 0x001A5890
		private void dfrmDeviceConfigure_Load(object sender, EventArgs e)
		{
			this.mtxtbControllerSN.Mask = "000000000";
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
						goto IL_00CB;
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
				IL_00CB:
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
			string text2 = "SELECT MAX(f_ControllerID) from t_b_Controller_FingerPrint";
			int num = wgAppConfig.getValBySql(text2) + 1;
			this.txtCameraName.Text = "z" + num.ToString().PadLeft(3, '0');
			if (this.cameraID > 0)
			{
				try
				{
					string text = " SELECT ";
					text = text + " f_ControllerID , f_FingerPrintName , f_ControllerSN , f_IP , f_Port , f_Enabled , f_ReaderID , f_Notes   from t_b_Controller_FingerPrint   Where f_ControllerID = " + this.cameraID.ToString();
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
							goto IL_031A;
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
					IL_031A:
					if (dataTable2.Rows.Count > 0)
					{
						this.txtCameraName.Text = (string)dataTable2.Rows[0]["f_FingerPrintName"];
						this.txtControllerIP.Text = wgTools.SetObjToStr(dataTable2.Rows[0]["f_IP"]);
						if (this.txtControllerIP.Text == "")
						{
							this.optIPSmall.Checked = true;
						}
						else
						{
							this.optIPLarge.Checked = true;
							this.nudPort.Value = (int)dataTable2.Rows[0]["f_Port"];
						}
						this.txtNote.Text = wgTools.SetObjToStr(dataTable2.Rows[0]["f_Notes"]);
						this.mtxtbControllerSN.Text = wgTools.SetObjToStr(dataTable2.Rows[0]["f_ControllerSN"]);
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
				}
				catch (Exception ex2)
				{
					wgAppConfig.wgDebugWrite(ex2.ToString(), EventLogEntryType.Error);
				}
			}
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x001A6EA4 File Offset: 0x001A5EA4
		private void dfrmFaceDeviceConfigure_KeyDown(object sender, KeyEventArgs e)
		{
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x001A6EA6 File Offset: 0x001A5EA6
		private void mtxtbControllerSN_KeyPress(object sender, KeyPressEventArgs e)
		{
			dfrmFingerPrintConfigure.SNInput(ref this.mtxtbControllerSN);
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x001A6EB3 File Offset: 0x001A5EB3
		private void mtxtbControllerSN_KeyUp(object sender, KeyEventArgs e)
		{
			dfrmFingerPrintConfigure.SNInput(ref this.mtxtbControllerSN);
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x001A6EC0 File Offset: 0x001A5EC0
		private void optIPLarge_CheckedChanged(object sender, EventArgs e)
		{
			this.grpbIP.Visible = this.optIPLarge.Checked;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x001A6ED8 File Offset: 0x001A5ED8
		public static void SNInput(ref MaskedTextBox mtb)
		{
			if (mtb.Text.Length != mtb.Text.Trim().Length)
			{
				mtb.Text = mtb.Text.Trim();
			}
			else if (mtb.Text.Length == 0 && mtb.SelectionStart != 0)
			{
				mtb.SelectionStart = 0;
			}
			if (mtb.Text.Length > 0)
			{
				if (mtb.Text.IndexOf(" ") > 0)
				{
					mtb.Text = mtb.Text.Replace(" ", "");
				}
				if (mtb.Text.Length > 9 && long.Parse(mtb.Text) >= (long)((ulong)(-1)))
				{
					mtb.Text = mtb.Text.Substring(0, mtb.Text.Length - 1);
				}
			}
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x001A6FBC File Offset: 0x001A5FBC
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
				string text = string.Format("Select * from t_b_Controller_FingerPrint where f_Controllerid <>{0} and (f_FingerPrintName ={1} )", this.cameraID, wgTools.PrepareStrNUnicode(this.txtCameraName.Text), this.mtxtbControllerSN.Text);
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
				text = string.Format("Select * from t_b_Controller_FingerPrint where f_Controllerid <>{0} and ( f_ControllerSN = {2})", this.cameraID, wgTools.PrepareStrNUnicode(this.txtCameraName.Text), this.mtxtbControllerSN.Text);
				dbCommand.CommandText = text;
				dbDataReader = dbCommand.ExecuteReader();
				num = 0;
				if (dbDataReader.Read())
				{
					num = 1;
				}
				dbDataReader.Close();
				if (num > 0)
				{
					XMessageBox.Show(this, CommonStr.strSNAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return flag;
				}
				int num2 = 1;
				if ((this.cboReader.DataSource as DataTable).Rows.Count > 0 && this.cboReader.SelectedIndex >= 0)
				{
					num2 = (int)(this.cboReader.DataSource as DataTable).Rows[this.cboReader.SelectedIndex]["f_ReaderID"];
				}
				string text2 = "";
				int num3 = 60000;
				if (this.optIPLarge.Checked)
				{
					text2 = this.txtControllerIP.Text.Trim();
					num3 = (int)this.nudPort.Value;
				}
				if (this.cameraID > 0)
				{
					text = "UPDATE t_b_Controller_FingerPrint  SET  ";
					text = string.Concat(new string[]
					{
						text,
						" f_FingerPrintName =",
						wgTools.PrepareStrNUnicode(this.txtCameraName.Text),
						",f_IP = ",
						wgTools.PrepareStrNUnicode(text2),
						",f_Port = ",
						num3.ToString(),
						",f_Enabled = ",
						(this.chkActive.Checked ? 1 : 0).ToString(),
						",f_ControllerSN = ",
						this.mtxtbControllerSN.Text,
						",f_ReaderID = ",
						num2.ToString(),
						",f_Notes = ",
						wgTools.PrepareStrNUnicode(this.txtNote.Text),
						" WHERE f_Controllerid = ",
						this.cameraID.ToString()
					});
				}
				else
				{
					text = "Insert Into t_b_Controller_FingerPrint(f_FingerPrintName,f_IP,f_Port,f_Enabled,f_Notes, f_ControllerSN, f_ReaderID) ";
					text = string.Concat(new object[]
					{
						text,
						" Values(",
						wgTools.PrepareStrNUnicode(this.txtCameraName.Text),
						",",
						wgTools.PrepareStrNUnicode(text2),
						",",
						num3.ToString(),
						",",
						this.chkActive.Checked ? 1 : 0,
						",",
						wgTools.PrepareStrNUnicode(this.txtNote.Text),
						", ",
						this.mtxtbControllerSN.Text,
						", ",
						num2.ToString(),
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

		// Token: 0x04002C1E RID: 11294
		public int cameraID;

		// Token: 0x04002C1F RID: 11295
		private Dictionary<string, string> ReaderID = new Dictionary<string, string>();

		// Token: 0x04002C20 RID: 11296
		private Dictionary<string, string> ReaderName = new Dictionary<string, string>();
	}
}
