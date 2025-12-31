using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.WIFI2019
{
	// Token: 0x0200032A RID: 810
	public partial class dfrmWIFIConfig : frmN3000
	{
		// Token: 0x06001989 RID: 6537 RVA: 0x002168D3 File Offset: 0x002158D3
		public dfrmWIFIConfig()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x002168E1 File Offset: 0x002158E1
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x002168EC File Offset: 0x002158EC
		private void btnOK_Click(object sender, EventArgs e)
		{
			dfrmWIFIConfig.strSSID = this.txtSSID.Text.Trim();
			dfrmWIFIConfig.strPassword = this.txtPassword.Text.Trim();
			wgAppConfig.UpdateKeyVal("KEY_SSID", dfrmWIFIConfig.strSSID);
			wgAppConfig.UpdateKeyVal("KEY_SSIDPWD", Program.Ept4Database(dfrmWIFIConfig.strPassword));
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00216953 File Offset: 0x00215953
		private void chkDisplayPwd_CheckedChanged(object sender, EventArgs e)
		{
			dfrmWIFIConfig.bDisplay = this.chkDisplayPwd.Checked;
			if (!dfrmWIFIConfig.bDisplay)
			{
				this.txtPassword.PasswordChar = '*';
				return;
			}
			this.txtPassword.PasswordChar = '\0';
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00216988 File Offset: 0x00215988
		private void createShareSSIDNameAndCopyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dfrmWIFIConfig.strSSID = this.txtSSID.Text.Trim();
			dfrmWIFIConfig.strPassword = this.txtPassword.Text.Trim();
			wgAppConfig.UpdateKeyVal("KEY_SSID", dfrmWIFIConfig.strSSID);
			wgAppConfig.UpdateKeyVal("KEY_SSIDPWD", Program.Ept4Database(dfrmWIFIConfig.strPassword));
			this.txtSharePoint.Visible = true;
			this.txtSharePoint.Text = "";
			string text = this.txtSSID.Text.Trim();
			string text2 = this.txtPassword.Text.Trim();
			string text3 = text + "888";
			char c = (char)(97 + DateTime.Now.Second % 10);
			int num = (int)text.ToCharArray()[text.Length - 1];
			int num2 = (int)text.ToCharArray()[0];
			int num3 = (num2 + num) & 15;
			if (num3 == 0)
			{
				num3 = 1;
			}
			char[] array = text2.ToCharArray();
			text3 += c;
			int num4 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				int num5 = num3 + i + (int)c;
				num5 &= 15;
				if (num5 == 0)
				{
					num5 = 1;
				}
				text3 += this.getEnc(array[i], num5);
				num4 += (int)this.getEnc(array[i], num5);
			}
			text3 += (num4 % 10).ToString();
			this.txtSharePoint.Text = text3;
			Clipboard.SetText(this.txtSharePoint.Text);
			wgAppConfig.wgLog("ShareHotPoint=" + text3);
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00216B1C File Offset: 0x00215B1C
		private void deactiveWIFINeedRebootToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.deactiveWIFINeedRebootToolStripMenuItem.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				this.bWIFIDisable = true;
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x00216B70 File Offset: 0x00215B70
		private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.txtSharePoint.Visible = true;
			string text = this.txtSharePoint.Text.Substring(0, this.txtSharePoint.Text.IndexOf("888"));
			if (string.IsNullOrEmpty(text))
			{
				XMessageBox.Show("Invalid Information.  无效热点共享名称");
				return;
			}
			char c = this.txtSharePoint.Text.ToCharArray()[text.Length + 3];
			string text2 = this.txtSharePoint.Text.Substring(text.Length + 4);
			string text3 = "";
			int num = (int)text.ToCharArray()[text.Length - 1];
			int num2 = (int)text.ToCharArray()[0];
			int num3 = (num2 + num) & 15;
			if (num3 == 0)
			{
				num3 = 1;
			}
			char[] array = text2.ToCharArray();
			int num4 = 0;
			for (int i = 0; i < array.Length - 1; i++)
			{
				int num5 = num3 + i + (int)c;
				num5 &= 15;
				if (num5 == 0)
				{
					num5 = 1;
				}
				num4 += (int)array[i];
				text3 += this.getDecryt(array[i], num5);
			}
			if (num4 % 10 + 48 != (int)array[array.Length - 1])
			{
				XMessageBox.Show("Invalid Information.  无效热点共享名称");
				return;
			}
			this.txtSSID.Text = text;
			this.txtPassword.Text = text3;
			XMessageBox.Show(string.Format("解码成功\r\n\r\n{0}={1}\r\n\r\n{2}={3}", new object[]
			{
				this.lblSSID.Text,
				text,
				this.lblSSIDpwd.Text,
				text3
			}));
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x00216CFC File Offset: 0x00215CFC
		private void dfrmWIFIConfig_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.decryptToolStripMenuItem.Visible = true;
				this.txtSharePoint.Visible = true;
				this.txtSharePoint.ReadOnly = false;
			}
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x00216D64 File Offset: 0x00215D64
		private void dfrmWIFIConfig_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(dfrmWIFIConfig.strSSID))
			{
				dfrmWIFIConfig.strSSID = wgAppConfig.GetKeyVal("KEY_SSID");
				if (!string.IsNullOrEmpty(dfrmWIFIConfig.strSSID))
				{
					dfrmWIFIConfig.strPassword = Program.Dpt4Database(wgAppConfig.GetKeyVal("KEY_SSIDPWD"));
				}
			}
			this.txtSSID.Text = dfrmWIFIConfig.strSSID;
			this.txtPassword.Text = dfrmWIFIConfig.strPassword;
			this.chkDisplayPwd.Checked = dfrmWIFIConfig.bDisplay;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x00216DDC File Offset: 0x00215DDC
		private char getDecryt(char val, int cryt)
		{
			int num = (int)val;
			if ((val >= '0') & (val <= '9'))
			{
				num = (int)(val - '0');
				num = 20 - cryt + num;
				num %= 10;
				num += 48;
			}
			else if ((val >= 'a') & (val <= 'z'))
			{
				num = (int)(val - 'a');
				num = 26 - cryt + num;
				num %= 26;
				num += 97;
			}
			else if ((val >= 'A') & (val <= 'Z'))
			{
				num = (int)(val - 'A');
				num = 26 - cryt + num;
				num %= 26;
				num += 65;
			}
			return (char)num;
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x00216E6C File Offset: 0x00215E6C
		private char getEnc(char val, int cryt)
		{
			int num = (int)val;
			if ((val >= '0') & (val <= '9'))
			{
				num = (int)(val - '0');
				num = cryt + num;
				num %= 10;
				num += 48;
			}
			else if ((val >= 'a') & (val <= 'z'))
			{
				num = (int)(val - 'a');
				num = cryt + num;
				num %= 26;
				num += 97;
			}
			else if ((val >= 'A') & (val <= 'Z'))
			{
				num = (int)(val - 'A');
				num = cryt + num;
				num %= 26;
				num += 65;
			}
			return (char)num;
		}

		// Token: 0x04003441 RID: 13377
		public static bool bDisplay = false;

		// Token: 0x04003442 RID: 13378
		public bool bWIFIDisable;

		// Token: 0x04003443 RID: 13379
		public static string strPassword = "";

		// Token: 0x04003444 RID: 13380
		public static string strSSID = "";
	}
}
