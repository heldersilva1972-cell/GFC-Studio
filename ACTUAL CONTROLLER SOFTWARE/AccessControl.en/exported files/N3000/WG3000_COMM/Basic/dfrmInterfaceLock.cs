using System;
using System.ComponentModel;
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
	// Token: 0x02000018 RID: 24
	public partial class dfrmInterfaceLock : frmN3000
	{
		// Token: 0x06000120 RID: 288 RVA: 0x000286C4 File Offset: 0x000276C4
		public dfrmInterfaceLock()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000286D2 File Offset: 0x000276D2
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000286E4 File Offset: 0x000276E4
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (icOperator.login(this.txtOperatorName.Text, this.txtPassword.Text))
			{
				base.DialogResult = DialogResult.OK;
				wgAppConfig.IsLogin = true;
				wgAppConfig.wgLog(this.Text, EventLogEntryType.Information, null);
				this.bConfirmClose = true;
				base.Close();
				return;
			}
			SystemSounds.Beep.Play();
			XMessageBox.Show(this, CommonStr.strErrPwdOrName, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00028754 File Offset: 0x00027754
		private void dfrmInterfaceLock_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!wgAppConfig.gRestart)
			{
				if (!this.bConfirmClose)
				{
					SystemSounds.Beep.Play();
					XMessageBox.Show(this, CommonStr.strErrPwdOrName, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					e.Cancel = true;
					return;
				}
				frmADCT3000.iOperCount = 0;
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00028794 File Offset: 0x00027794
		private void dfrmInterfaceLock_SizeChanged(object sender, EventArgs e)
		{
			try
			{
				if (base.WindowState == FormWindowState.Minimized)
				{
					base.Owner.Visible = false;
				}
				else if (base.WindowState == FormWindowState.Maximized)
				{
					base.Owner.Visible = true;
				}
				else
				{
					base.Owner.Visible = true;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000287FC File Offset: 0x000277FC
		private void dfrmSetPassword_Load(object sender, EventArgs e)
		{
			this.txtPassword.MaxLength = wgAppConfig.PasswordMaxLenght;
		}

		// Token: 0x040002DD RID: 733
		private bool bConfirmClose;

		// Token: 0x040002DE RID: 734
		public string newPassword;

		// Token: 0x040002DF RID: 735
		public int operatorID;
	}
}
