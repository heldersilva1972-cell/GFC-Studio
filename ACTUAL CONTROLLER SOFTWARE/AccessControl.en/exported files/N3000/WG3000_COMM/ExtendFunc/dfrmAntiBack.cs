using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000233 RID: 563
	public partial class dfrmAntiBack : frmN3000
	{
		// Token: 0x060010DE RID: 4318 RVA: 0x001352FD File Offset: 0x001342FD
		public dfrmAntiBack()
		{
			this.InitializeComponent();
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00135316 File Offset: 0x00134316
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00135328 File Offset: 0x00134328
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.retValue = 0;
			if (this.radioButton1.Checked)
			{
				this.retValue = 1;
			}
			if (this.radioButton2.Checked)
			{
				this.retValue = 2;
			}
			if (this.radioButton3.Checked)
			{
				this.retValue = 3;
			}
			if (this.radioButton4.Checked)
			{
				this.retValue = 4;
			}
			if (this.chkActiveAntibackShare.Checked)
			{
				this.retValue += (int)this.nudTotal.Value * 10;
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x001353C8 File Offset: 0x001343C8
		private void chkActiveAntibackShare_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkActiveAntibackShare.Checked)
			{
				if (!this.radioButton4.Visible)
				{
					if (this.radioButton2.Visible)
					{
						this.radioButton0.Enabled = false;
						this.radioButton1.Checked = false;
						this.radioButton1.Enabled = false;
						this.radioButton2.Checked = true;
						return;
					}
					if (this.radioButton1.Visible)
					{
						this.radioButton0.Enabled = false;
						this.radioButton1.Checked = true;
						return;
					}
				}
				else
				{
					this.radioButton0.Enabled = false;
					this.radioButton1.Enabled = false;
					this.radioButton2.Enabled = true;
					this.radioButton3.Enabled = false;
					if (!this.radioButton4.Checked)
					{
						this.radioButton2.Checked = true;
						return;
					}
				}
			}
			else if (wgAppConfig.getParamValBoolByNO(62))
			{
				if (this.radioButton0.Visible)
				{
					this.radioButton0.Enabled = true;
				}
				bool visible = this.radioButton1.Visible;
				if (this.radioButton2.Visible)
				{
					this.radioButton1.Checked = false;
					this.radioButton1.Enabled = false;
				}
				if (this.radioButton3.Visible)
				{
					this.radioButton1.Enabled = false;
					this.radioButton2.Enabled = false;
				}
				if (this.radioButton4.Visible)
				{
					this.radioButton1.Enabled = false;
					this.radioButton2.Enabled = true;
					this.radioButton3.Enabled = false;
					return;
				}
			}
			else
			{
				this.radioButton0.Enabled = true;
				this.radioButton1.Enabled = true;
				this.radioButton2.Enabled = true;
				this.radioButton3.Enabled = true;
			}
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00135584 File Offset: 0x00134584
		private void dfrmAntiBack_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyValue == 81 && e.Shift)
			{
				if (this.chkActiveAntibackShare.Visible)
				{
					this.nudTotal.ReadOnly = false;
					this.nudTotal.Maximum = 4000m;
				}
				this.chkActiveAntibackShare.Visible = true;
				this.nudTotal.Visible = true;
			}
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x001355F4 File Offset: 0x001345F4
		private void dfrmAntiBack_Load(object sender, EventArgs e)
		{
			string text = "SELECT * FROM t_b_Controller Where f_ControllerSN = " + this.ControllerSN;
			DbConnection dbConnection;
			DbCommand dbCommand;
			if (wgAppConfig.IsAccessDB)
			{
				dbConnection = new OleDbConnection(wgAppConfig.dbConString);
				dbCommand = new OleDbCommand(text, dbConnection as OleDbConnection);
			}
			else
			{
				dbConnection = new SqlConnection(wgAppConfig.dbConString);
				dbCommand = new SqlCommand(text, dbConnection as SqlConnection);
			}
			dbConnection.Open();
			DbDataReader dbDataReader = dbCommand.ExecuteReader();
			if (dbDataReader.Read())
			{
				switch (wgMjController.GetControllerType(int.Parse(this.ControllerSN)))
				{
				case 1:
					this.radioButton1.Text = this.checkBox11.Text;
					this.radioButton2.Visible = false;
					this.radioButton3.Visible = false;
					this.radioButton4.Visible = false;
					break;
				case 2:
					this.radioButton1.Text = this.checkBox21.Text;
					this.radioButton2.Text = this.checkBox22.Text;
					this.radioButton3.Visible = false;
					this.radioButton4.Visible = false;
					break;
				}
				switch ((int)dbDataReader["f_AntiBack"] % 10)
				{
				case 1:
					this.radioButton1.Checked = true;
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
				if ((int)dbDataReader["f_AntiBack"] > 10)
				{
					this.nudTotal.Visible = true;
					this.chkActiveAntibackShare.Visible = true;
					if (((int)dbDataReader["f_AntiBack"] - (int)dbDataReader["f_AntiBack"] % 10) / 10 > 1000)
					{
						this.nudTotal.Maximum = 4000m;
					}
					this.nudTotal.Value = ((int)dbDataReader["f_AntiBack"] - (int)dbDataReader["f_AntiBack"] % 10) / 10;
					this.chkActiveAntibackShare.Checked = true;
				}
			}
			dbDataReader.Close();
			dbConnection.Close();
			if (dfrmAntiBack.bDisplayIndoorPersonMax)
			{
				this.nudTotal.Visible = true;
				this.chkActiveAntibackShare.Visible = true;
			}
			if (wgAppConfig.getParamValBoolByNO(62))
			{
				bool visible = this.radioButton0.Visible;
				bool visible2 = this.radioButton1.Visible;
				if (this.radioButton2.Visible)
				{
					this.radioButton1.Checked = false;
					this.radioButton1.Enabled = false;
				}
				if (this.radioButton3.Visible)
				{
					this.radioButton1.Enabled = false;
					this.radioButton2.Enabled = false;
				}
				if (this.radioButton4.Visible)
				{
					this.radioButton1.Enabled = false;
					this.radioButton2.Enabled = true;
					this.radioButton3.Enabled = false;
				}
			}
		}

		// Token: 0x04001E12 RID: 7698
		public static bool bDisplayIndoorPersonMax;

		// Token: 0x04001E13 RID: 7699
		public string ControllerSN = "";

		// Token: 0x04001E14 RID: 7700
		public int retValue;
	}
}
