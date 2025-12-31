using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200002B RID: 43
	public partial class dfrmSetPassword : frmN3000
	{
		// Token: 0x06000319 RID: 793 RVA: 0x0005CFBF File Offset: 0x0005BFBF
		public dfrmSetPassword()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0005CFCD File Offset: 0x0005BFCD
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0005CFDC File Offset: 0x0005BFDC
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this.txtPasswordNew.Text != this.txtPasswordNewConfirm.Text)
			{
				XMessageBox.Show(this, CommonStr.strPwdNotSame, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.operatorID != 0)
			{
				try
				{
					this.newPassword = this.txtPasswordNew.Text;
					string text = " UPDATE [t_s_Operator] ";
					if (wgAppConfig.runUpdateSql(string.Concat(new object[]
					{
						text,
						"SET [f_Password]=",
						wgTools.PrepareStrNUnicode(Program.Ept4Database(this.txtPasswordNew.Text)),
						" WHERE [f_OperatorID]=",
						this.operatorID
					})) >= 0)
					{
						base.DialogResult = DialogResult.OK;
						base.Close();
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
					XMessageBox.Show(ex.Message);
				}
				return;
			}
			if (this.txtPasswordNew.Text == "")
			{
				this.txtPasswordNew.Text = "0";
				this.txtPasswordNewConfirm.Text = "0";
			}
			long num = -1L;
			if (!long.TryParse(this.txtPasswordNew.Text, out num) || this.txtPasswordNew.Text.Length > 6)
			{
				XMessageBox.Show(this, CommonStr.strPasswordWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (num < 0L)
			{
				XMessageBox.Show(this, CommonStr.strPasswordWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.newPassword = this.txtPasswordNew.Text;
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0005D190 File Offset: 0x0005C190
		private void dfrmSetPassword_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x04000602 RID: 1538
		public string newPassword;

		// Token: 0x04000603 RID: 1539
		public int operatorID;
	}
}
