using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000240 RID: 576
	public partial class dfrmControllerWarnSet : frmN3000
	{
		// Token: 0x0600119B RID: 4507 RVA: 0x0014A4EE File Offset: 0x001494EE
		public dfrmControllerWarnSet()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0014A507 File Offset: 0x00149507
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0014A510 File Offset: 0x00149510
		private void btnChangeThreatPassword_Click(object sender, EventArgs e)
		{
			using (dfrmSetPassword dfrmSetPassword = new dfrmSetPassword())
			{
				dfrmSetPassword.operatorID = 0;
				dfrmSetPassword.Text = this.btnChangeThreatPassword.Text;
				if (dfrmSetPassword.ShowDialog(this) == DialogResult.OK)
				{
					if (int.Parse(dfrmSetPassword.newPassword) >= 999999 || int.Parse(dfrmSetPassword.newPassword) <= 0)
					{
						XMessageBox.Show(this, CommonStr.strFailedNumeric999999, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else if (wgAppConfig.setSystemParamValue(24, "Threat Password", dfrmSetPassword.newPassword, "") == 1)
					{
						this.lblThreatPassword.Text = dfrmSetPassword.newPassword;
						XMessageBox.Show(this, "OK", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
					else
					{
						XMessageBox.Show(this, CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0014A5F0 File Offset: 0x001495F0
		private void btnExtension_Click(object sender, EventArgs e)
		{
			if (this.dataGridView1.SelectedRows.Count <= 0)
			{
				if (this.dataGridView1.SelectedCells.Count <= 0)
				{
					return;
				}
				int rowIndex = this.dataGridView1.SelectedCells[0].RowIndex;
			}
			else
			{
				int index = this.dataGridView1.SelectedRows[0].Index;
			}
			int num = 0;
			DataGridView dataGridView = this.dataGridView1;
			if (dataGridView.Rows.Count > 0)
			{
				try
				{
					num = dataGridView.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			using (dfrmPeripheralControlBoard dfrmPeripheralControlBoard = new dfrmPeripheralControlBoard())
			{
				dfrmPeripheralControlBoard.ControllerNO = int.Parse(dataGridView.Rows[num].Cells[0].Value.ToString());
				dfrmPeripheralControlBoard.ControllerSN = int.Parse(dataGridView.Rows[num].Cells[1].Value.ToString());
				if (dfrmPeripheralControlBoard.ShowDialog(this) == DialogResult.OK)
				{
					this.loadData();
				}
			}
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0014A714 File Offset: 0x00149714
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.btnOK.Enabled = false;
			wgAppConfig.setSystemParamValue(60, this.chkActiveFireSignalShare.Checked ? (this.chkGrouped.Checked ? "2" : "1") : "0");
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOK_Click_Acc(sender, e);
				return;
			}
			DataTable table = (this.dataGridView1.DataSource as DataView).Table;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					for (int i = 0; i <= table.Rows.Count - 1; i++)
					{
						string text = " UPDATE t_b_Controller SET ";
						text = string.Concat(new string[]
						{
							text,
							" f_DoorInvalidOpen = 0",
							table.Rows[i]["f_DoorInvalidOpen"].ToString(),
							", f_DoorOpenTooLong = 0",
							table.Rows[i]["f_DoorOpenTooLong"].ToString(),
							", f_ForceWarn  = 0",
							table.Rows[i]["f_ForceWarn"].ToString(),
							", f_InvalidCardWarn = 0",
							table.Rows[i]["f_InvalidCardWarn"].ToString(),
							" WHERE f_ControllerNo = ",
							table.Rows[i]["f_ControllerNo"].ToString()
						});
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
					}
				}
			}
			wgAppConfig.setSystemParamValue(40, "", this.nudOpenDoorTimeout.Value.ToString(), "");
			base.Close();
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0014A934 File Offset: 0x00149934
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			DataTable table = (this.dataGridView1.DataSource as DataView).Table;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
				{
					oleDbConnection.Open();
					for (int i = 0; i <= table.Rows.Count - 1; i++)
					{
						string text = " UPDATE t_b_Controller SET ";
						text = string.Concat(new string[]
						{
							text,
							" f_DoorInvalidOpen = 0",
							table.Rows[i]["f_DoorInvalidOpen"].ToString(),
							", f_DoorOpenTooLong = 0",
							table.Rows[i]["f_DoorOpenTooLong"].ToString(),
							", f_ForceWarn  = 0",
							table.Rows[i]["f_ForceWarn"].ToString(),
							", f_InvalidCardWarn = 0",
							table.Rows[i]["f_InvalidCardWarn"].ToString(),
							" WHERE f_ControllerNo = ",
							table.Rows[i]["f_ControllerNo"].ToString()
						});
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
					}
				}
			}
			wgAppConfig.setSystemParamValue(40, "", this.nudOpenDoorTimeout.Value.ToString(), "");
			base.Close();
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0014AB04 File Offset: 0x00149B04
		private void chkActiveFireSignalShare_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkActiveFireSignalShare.Checked)
			{
				this.chkGrouped.Enabled = true;
				return;
			}
			this.chkGrouped.Enabled = false;
			this.chkGrouped.Checked = false;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0014AB38 File Offset: 0x00149B38
		private void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			this.btnExtension.PerformClick();
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0014AB48 File Offset: 0x00149B48
		private void dfrmControllerInterLock_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.lblThreatPassword.Text = "889988";
			this.loadData();
			this.lblThreatPassword.Text = wgAppConfig.getSystemParamByNO(24);
			this.chkActiveFireSignalShare.Checked = wgAppConfig.getParamValBoolByNO(60);
			this.chkActiveFireSignalShare.Visible = this.chkActiveFireSignalShare.Checked;
			if (this.chkActiveFireSignalShare.Visible && wgAppConfig.getSystemParamByNO(60) == "2")
			{
				this.chkGrouped.Checked = true;
				this.chkGrouped.Visible = true;
			}
			this.nudOpenDoorTimeout.Value = decimal.Parse(wgAppConfig.getSystemParamByNO(40), CultureInfo.InvariantCulture);
			this.loadOperatorPrivilege();
			if (wgAppConfig.IsActivateOpenTooLongWarn)
			{
				this.dataGridView1.Columns["f_InterLock34"].HeaderText = CommonStr.strOpenTooLongWarn2015;
			}
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0014AC2C File Offset: 0x00149C2C
		private void dfrmControllerWarnSet_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0014AC44 File Offset: 0x00149C44
		private void dfrmControllerWarnSet_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.KeyValue == 81 && e.Shift)
				{
					if (this.chkActiveFireSignalShare.Visible)
					{
						this.chkGrouped.Visible = true;
					}
					this.chkActiveFireSignalShare.Visible = true;
					this.chkActiveFireSignalShare_CheckedChanged(null, null);
				}
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.dfrmFind1 == null)
					{
						this.dfrmFind1 = new dfrmFind();
					}
					this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0014AD00 File Offset: 0x00149D00
		private void loadData()
		{
			string text = " SELECT ";
			text += " f_ControllerNO , f_ControllerSN , f_ForceWarn , f_DoorOpenTooLong , f_DoorInvalidOpen , f_InvalidCardWarn , f_DoorsNames , f_ZoneID  from t_b_Controller   ORDER BY f_ControllerNO ";
			wgAppConfig.fillDGVData(ref this.dataGridView1, text);
			DataTable table = ((DataView)this.dataGridView1.DataSource).Table;
			new icControllerZone().getAllowedControllers(ref table);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0014AD50 File Offset: 0x00149D50
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuExtendedFunction";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnExtension.Visible = false;
				this.btnChangeThreatPassword.Enabled = false;
				this.btnOK.Visible = false;
				this.nudOpenDoorTimeout.ReadOnly = true;
				this.nudOpenDoorTimeout.Enabled = false;
				this.dataGridView1.ReadOnly = true;
			}
		}
	}
}
