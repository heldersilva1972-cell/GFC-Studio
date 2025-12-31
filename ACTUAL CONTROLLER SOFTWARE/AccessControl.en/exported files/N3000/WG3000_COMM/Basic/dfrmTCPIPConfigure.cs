using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200002F RID: 47
	public partial class dfrmTCPIPConfigure : frmN3000
	{
		// Token: 0x06000340 RID: 832 RVA: 0x0005F4B0 File Offset: 0x0005E4B0
		public dfrmTCPIPConfigure()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0005F50C File Offset: 0x0005E50C
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (!this.txtf_ControllerSN.ReadOnly)
			{
				this.txtf_ControllerSN.Text = this.txtf_ControllerSN.Text.Trim();
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
			}
			if (string.IsNullOrEmpty(this.txtf_IP.Text))
			{
				XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.txtf_IP.Text = this.txtf_IP.Text.Replace(" ", "");
			if (!this.isIPAddress(this.txtf_IP.Text) && this.txtf_IP.Text != "0.0.0.0")
			{
				XMessageBox.Show(this, this.txtf_IP.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.txtf_mask.Text = this.txtf_mask.Text.Replace(" ", "");
			if (!this.isIPAddress(this.txtf_mask.Text))
			{
				XMessageBox.Show(this, this.txtf_mask.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.txtf_gateway.Text = this.txtf_gateway.Text.Replace(" ", "");
			if (!string.IsNullOrEmpty(this.txtf_gateway.Text) && !this.isIPAddress(this.txtf_gateway.Text))
			{
				XMessageBox.Show(this, this.txtf_gateway.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.strSN = this.txtf_ControllerSN.Text;
			this.strMac = this.txtf_MACAddr.Text;
			this.strIP = this.txtf_IP.Text;
			this.strMask = this.txtf_mask.Text;
			this.strGateway = this.txtf_gateway.Text;
			this.strTCPPort = this.nudPort.Value.ToString();
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0005F788 File Offset: 0x0005E788
		private void btnOption_Click(object sender, EventArgs e)
		{
			base.Size = new Size(base.Width, this.grpPort.Location.Y + this.grpPort.Height * 2);
			this.btnOption.Enabled = false;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0005F7D4 File Offset: 0x0005E7D4
		private void dfrmTCPIPConfigure_Load(object sender, EventArgs e)
		{
			this.txtf_ControllerSN.Text = this.strSN;
			this.txtf_MACAddr.Text = this.strMac;
			this.txtf_IP.Text = this.strIP;
			this.txtf_mask.Text = this.strMask;
			this.txtf_gateway.Text = this.strGateway;
			if (!string.IsNullOrEmpty(this.strTCPPort))
			{
				if (int.Parse(this.strTCPPort) < this.nudPort.Minimum || int.Parse(this.strTCPPort) >= 65535)
				{
					this.strTCPPort = 60000.ToString();
				}
				this.nudPort.Value = int.Parse(this.strTCPPort);
			}
			if (this.txtf_IP.Text == "255.255.255.255")
			{
				this.txtf_IP.Text = "192.168.0.0";
			}
			if (this.txtf_mask.Text == "255.255.255.255")
			{
				this.txtf_mask.Text = "255.255.255.0";
			}
			if (this.txtf_gateway.Text == "255.255.255.255")
			{
				this.txtf_gateway.Text = "";
			}
			if (this.txtf_gateway.Text == "0.0.0.0")
			{
				this.txtf_gateway.Text = "";
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0005F944 File Offset: 0x0005E944
		public bool isIPAddress(string ipstr)
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

		// Token: 0x04000635 RID: 1589
		public string strGateway = "";

		// Token: 0x04000636 RID: 1590
		public string strIP = "";

		// Token: 0x04000637 RID: 1591
		public string strMac = "";

		// Token: 0x04000638 RID: 1592
		public string strMask = "";

		// Token: 0x04000639 RID: 1593
		public string strSN = "";

		// Token: 0x0400063A RID: 1594
		public string strTCPPort = "";
	}
}
