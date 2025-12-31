using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000050 RID: 80
	public partial class frmLogin : frmN3000
	{
		// Token: 0x060005B7 RID: 1463 RVA: 0x000A112C File Offset: 0x000A012C
		public frmLogin()
		{
			this.InitializeComponent();
			this.txtPassword.MaxLength = wgAppConfig.PasswordMaxLenght;
			if (wgAppConfig.GetKeyVal("autologinName") != "")
			{
				this.txtOperatorName.Text = wgAppConfig.GetKeyVal("autologinName");
				try
				{
					this.txtPassword.Text = Program.Dpt4Database(wgAppConfig.GetKeyVal("autologinPassword"));
					wgAppConfig.IsAutoLogin = true;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			if (wgAppConfig.GetKeyVal("KEY_HideLogo") != "" && wgAppConfig.GetKeyVal("KEY_HideLogo") == "1")
			{
				this.label3.Visible = false;
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x000A1200 File Offset: 0x000A0200
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000A1208 File Offset: 0x000A0208
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (!icOperator.login(this.txtOperatorName.Text, this.txtPassword.Text))
			{
				wgAppConfig.IsAutoLogin = false;
				SystemSounds.Beep.Play();
				XMessageBox.Show(this, CommonStr.strErrPwdOrName, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (!wgAppConfig.getParamValBoolByNO(220) || this.txtPassword.Text.Trim().Length >= 5)
			{
				base.DialogResult = DialogResult.OK;
				wgAppConfig.IsLogin = true;
				wgAppConfig.LoginTitle = this.Text;
				string text = "";
				text += string.Format("Ver: {0},", Application.ProductVersion);
				wgTools.CommPStr = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("CommPCurrent"));
				if (!string.IsNullOrEmpty(wgTools.CommPStr))
				{
					text += string.Format("Communication With Password,", new object[0]);
				}
				if (wgAppConfig.IsAccessDB)
				{
					if (icOperator.OperatorID == 1)
					{
						text += string.Format("{2}:{0}:{1},", icOperator.OperatorName, "MsAccess", CommonStr.strSuper);
					}
					else
					{
						text += string.Format("{0}:{1},", icOperator.OperatorName, "MsAccess");
					}
				}
				else
				{
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						if (icOperator.OperatorID == 1)
						{
							text += string.Format("{2}:{0}:{1},", icOperator.OperatorName, sqlConnection.Database, CommonStr.strSuper);
						}
						else
						{
							text += string.Format("{0}:{1},", icOperator.OperatorName, sqlConnection.Database);
						}
					}
					text += wgAppConfig.GetKeyVal("dbConnection");
				}
				wgAppConfig.wgLog(string.Format("{0},{1}", this.Text, text), EventLogEntryType.Information, null);
				base.Close();
				return;
			}
			using (dfrmOperatorUpdate dfrmOperatorUpdate = new dfrmOperatorUpdate())
			{
				dfrmOperatorUpdate.operateMode = 1;
				dfrmOperatorUpdate.operatorID = icOperator.OperatorID;
				dfrmOperatorUpdate.operatorName = icOperator.OperatorName;
				dfrmOperatorUpdate.txtName.ReadOnly = true;
				dfrmOperatorUpdate.Text = string.Format("{0}--{1}", dfrmOperatorUpdate.Text, icOperator.OperatorName);
				if (dfrmOperatorUpdate.ShowDialog(this) == DialogResult.OK && XMessageBox.Show(this, CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
				{
					wgAppConfig.gRestart = true;
					this.btnExit.PerformClick();
				}
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x000A146C File Offset: 0x000A046C
		private void frmLogin_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r' && (sender.Equals(this.txtOperatorName) || sender.Equals(this.txtPassword) || sender.Equals(this.btnOK)))
			{
				this.btnOK_Click(sender, e);
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x000A14AC File Offset: 0x000A04AC
		private void frmLogin_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.ProductTypeOfApp != "AccessControl")
			{
				XMessageBox.Show("Wrong login file");
				base.Close();
				return;
			}
			wgAppConfig.bFloorRoomManager = wgAppConfig.getParamValBoolByNO(145);
			if (wgAppConfig.bFloorRoomManager)
			{
				this.Text = CommonStr.strTitleHouse;
			}
			this.restoreToolStripMenuItem.Visible = !wgAppConfig.getParamValBoolByNO(224);
			if (wgAppConfig.getSystemParamByName("Custom Title") != "")
			{
				this.Text = wgAppConfig.getSystemParamByName("Custom Title");
			}
			else if (wgAppConfig.GetKeyVal("Custom Title") != "")
			{
				this.Text = wgAppConfig.GetKeyVal("Custom Title");
			}
			this.label3.Text = "Access Control";
			if (wgAppConfig.GetKeyVal("Custom Logo AGACCESS5") != "")
			{
				this.label3.Text = wgAppConfig.GetKeyVal("Custom Logo AGACCESS5");
			}
			if (wgAppConfig.GetKeyVal("Custom Logo") != "")
			{
				this.label3.Text = wgAppConfig.GetKeyVal("Custom Logo");
			}
			wgAppConfig.IsLogin = false;
			this.timer1.Enabled = true;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x000A15DB File Offset: 0x000A05DB
		private void frmLogin_MouseDown(object sender, MouseEventArgs e)
		{
			this.mouse_offset = new Point(-e.X, -e.Y);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000A15F8 File Offset: 0x000A05F8
		private void frmLogin_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Point mousePosition = Control.MousePosition;
				mousePosition.Offset(this.mouse_offset.X, this.mouse_offset.Y);
				base.Location = mousePosition;
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000A163C File Offset: 0x000A063C
		private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.bNeedRestore = true;
			base.Close();
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x000A164A File Offset: 0x000A064A
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			if (wgAppConfig.GetKeyVal("autologinName") != "")
			{
				this.btnOK.PerformClick();
			}
		}

		// Token: 0x04000AD8 RID: 2776
		private Point mouse_offset;
	}
}
