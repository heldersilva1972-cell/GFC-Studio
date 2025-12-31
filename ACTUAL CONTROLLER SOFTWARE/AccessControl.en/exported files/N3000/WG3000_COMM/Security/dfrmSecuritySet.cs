using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Security
{
	// Token: 0x02000383 RID: 899
	public partial class dfrmSecuritySet : frmN3000
	{
		// Token: 0x0600210C RID: 8460 RVA: 0x0027CA1C File Offset: 0x0027BA1C
		public dfrmSecuritySet()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x0027CA78 File Offset: 0x0027BA78
		private void btnAddCurrentPCIP_Click(object sender, EventArgs e)
		{
			if (this.lstIP.Items.Count <= 9)
			{
				if (this.lstIP.Items.IndexOf(this.strPCIP) < 0)
				{
					this.lstIP.Items.Add(this.strPCIP);
				}
				if (this.lstIP.Items.Count >= 9)
				{
					this.btnAddIP.Enabled = false;
				}
				this.btnOk.Enabled = true;
				this.btnChangeAndUpdate.Enabled = true;
			}
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0027CB04 File Offset: 0x0027BB04
		private void btnAddIP_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtf_IP.Text))
			{
				XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.txtf_IP.Text = this.txtf_IP.Text.Replace(" ", "");
			this.txtf_IP.Text = this.txtf_IP.Text.Replace("-", ":");
			if (this.txtf_IP.Text.IndexOf(":") > 0)
			{
				string[] array = this.txtf_IP.Text.Split(new char[] { ':' });
				if (array.Length != 6)
				{
					if (array.Length != 4)
					{
						XMessageBox.Show(this, this.txtf_IP.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					byte[] array2 = new byte[16];
					byte.TryParse(array[0], NumberStyles.AllowHexSpecifier, null, out array2[0]);
					byte.TryParse(array[1], NumberStyles.AllowHexSpecifier, null, out array2[1]);
					byte.TryParse(array[2], NumberStyles.AllowHexSpecifier, null, out array2[2]);
					byte.TryParse(array[3], NumberStyles.AllowHexSpecifier, null, out array2[3]);
					this.txtf_IP.Text = string.Format("{0}.{1}.{2}.{3}", new object[]
					{
						array2[0],
						array2[1],
						array2[2],
						array2[3]
					});
				}
				else
				{
					byte[] array3 = new byte[16];
					byte.TryParse(array[0], NumberStyles.AllowHexSpecifier, null, out array3[0]);
					byte.TryParse(array[1], NumberStyles.AllowHexSpecifier, null, out array3[1]);
					byte.TryParse(array[2], NumberStyles.AllowHexSpecifier, null, out array3[2]);
					byte.TryParse(array[3], NumberStyles.AllowHexSpecifier, null, out array3[3]);
					byte.TryParse(array[4], NumberStyles.AllowHexSpecifier, null, out array3[4]);
					byte.TryParse(array[5], NumberStyles.AllowHexSpecifier, null, out array3[5]);
					this.txtf_IP.Text = string.Format("{0}.{1}.{2}.{3}", new object[]
					{
						array3[2],
						array3[3],
						array3[4],
						array3[5]
					});
				}
			}
			if (!this.isIPAddress(this.txtf_IP.Text))
			{
				XMessageBox.Show(this, this.txtf_IP.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.lstIP.Items.IndexOf(this.txtf_IP.Text) < 0)
			{
				this.lstIP.Items.Add(this.txtf_IP.Text);
			}
			this.txtf_IP.Text = "";
			if (this.lstIP.Items.Count >= 9)
			{
				this.btnAddIP.Enabled = false;
			}
			this.btnOk.Enabled = true;
			this.btnChangeAndUpdate.Enabled = true;
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x0027CE42 File Offset: 0x0027BE42
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x0027CE54 File Offset: 0x0027BE54
		private void btnChangeAndUpdate_Click(object sender, EventArgs e)
		{
			int num;
			if (!int.TryParse(this.txtf_ControllerSN.Text, out num))
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (wgMjController.GetControllerType(int.Parse(this.txtf_ControllerSN.Text)) == 0)
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.txtPasswordNew.Text != this.txtPasswordNewConfirm.Text)
			{
				XMessageBox.Show(this, CommonStr.strPwdNotSame, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (string.IsNullOrEmpty(this.txtPasswordNew.Text))
			{
				XMessageBox.Show(this, this.Label2.Text + "\r\n\r\n" + CommonStr.strErrPwdNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.txtPasswordNew.Text.Equals("654321"))
			{
				XMessageBox.Show(this, this.Label2.Text + "\r\n\r\n" + CommonStr.strErrPwdDefault, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.lstIP.Items.Count == 0)
			{
				XMessageBox.Show(this, CommonStr.strErrIPListNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.strIPListCurrentPassword = this.txtPasswordPrev.Text;
			this.strIPListNewPassword = this.txtPasswordNew.Text;
			this.strIPList = "";
			foreach (object obj in this.lstIP.Items)
			{
				string text = (string)obj;
				if (string.IsNullOrEmpty(this.strIPList))
				{
					this.strIPList = text;
				}
				else
				{
					this.strIPList = this.strIPList + ";" + text;
				}
			}
			this.strControllerSN = this.txtf_ControllerSN.Text;
			this.strOperate = "ChangePassword";
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x0027D05C File Offset: 0x0027C05C
		private void btnClearAll_Click(object sender, EventArgs e)
		{
			this.lstIP.Items.Clear();
			this.btnAddIP.Enabled = true;
			this.btnOk.Enabled = false;
			this.btnChangeAndUpdate.Enabled = false;
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x0027D094 File Offset: 0x0027C094
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtPasswordPrev.Text))
			{
				XMessageBox.Show(this, this.label1.Text + "\r\n\r\n" + CommonStr.strErrPwdNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.txtPasswordPrev.Text.Equals("654321"))
			{
				XMessageBox.Show(this, this.label1.Text + "\r\n\r\n" + CommonStr.strErrPwdDefault, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.lstIP.Items.Count == 0)
			{
				XMessageBox.Show(this, CommonStr.strErrIPListNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			int num;
			if (!int.TryParse(this.txtf_ControllerSN.Text, out num))
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (wgMjController.GetControllerType(int.Parse(this.txtf_ControllerSN.Text)) == 0)
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.strIPListCurrentPassword = this.txtPasswordPrev.Text;
			this.strIPListNewPassword = "";
			this.strIPList = "";
			foreach (object obj in this.lstIP.Items)
			{
				string text = (string)obj;
				if (string.IsNullOrEmpty(this.strIPList))
				{
					this.strIPList = text;
				}
				else
				{
					this.strIPList = this.strIPList + ";" + text;
				}
			}
			this.strControllerSN = this.txtf_ControllerSN.Text;
			this.strOperate = "OnlyUpdate";
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x0027D264 File Offset: 0x0027C264
		private void dfrmCommPSet_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				this.txtf_ControllerSN.Enabled = true;
				this.txtf_ControllerSN.ReadOnly = false;
			}
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x0027D298 File Offset: 0x0027C298
		private void dfrmCommPSet_Load(object sender, EventArgs e)
		{
			this.txtPasswordPrev.Text = this.strIPListCurrentPassword;
			this.txtPasswordNew.Text = this.strIPListNewPassword;
			this.txtPasswordNewConfirm.Text = this.strIPListNewPassword;
			this.txtf_ControllerSN.Text = this.strControllerSN;
			if (string.IsNullOrEmpty(this.strControllerSN))
			{
				this.txtf_ControllerSN.Enabled = true;
				this.txtf_ControllerSN.ReadOnly = false;
			}
			if (!string.IsNullOrEmpty(this.strIPList))
			{
				foreach (string text in this.strIPList.Split(new char[] { ';' }))
				{
					this.lstIP.Items.Add(text);
				}
			}
			if (string.IsNullOrEmpty(this.strPCIP))
			{
				this.btnAddCurrentPCIP.Visible = false;
			}
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x0027D374 File Offset: 0x0027C374
		private bool isIPAddress(string ipstr)
		{
			bool flag = false;
			try
			{
				if (string.IsNullOrEmpty(ipstr))
				{
					return flag;
				}
				string[] array = ipstr.Split(new char[] { '.' });
				if (array.Length != 4)
				{
					return flag;
				}
				flag = true;
				for (int i = 0; i <= 3; i++)
				{
					int num;
					if (!int.TryParse(array[i], out num))
					{
						flag = false;
						break;
					}
					if (num < 0 || num > 255)
					{
						flag = false;
						break;
					}
				}
				if (int.Parse(array[0]) == 0)
				{
					return false;
				}
				if (int.Parse(array[3]) == 255)
				{
					flag = false;
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x0027D41C File Offset: 0x0027C41C
		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.strIPList))
			{
				foreach (string text in this.strIPList.Split(new char[] { ';' }))
				{
					if (this.lstIP.Items.IndexOf(text) < 0)
					{
						this.lstIP.Items.Add(text);
					}
				}
			}
		}

		// Token: 0x04003937 RID: 14647
		public bool bChangedPwd;

		// Token: 0x04003938 RID: 14648
		public string strControllerSN = "";

		// Token: 0x04003939 RID: 14649
		public string strIPList = "";

		// Token: 0x0400393A RID: 14650
		public string strIPListCurrentPassword = "";

		// Token: 0x0400393B RID: 14651
		public string strIPListNewPassword = "";

		// Token: 0x0400393C RID: 14652
		public string strOperate = "";

		// Token: 0x0400393D RID: 14653
		public string strPCIP = "";
	}
}
