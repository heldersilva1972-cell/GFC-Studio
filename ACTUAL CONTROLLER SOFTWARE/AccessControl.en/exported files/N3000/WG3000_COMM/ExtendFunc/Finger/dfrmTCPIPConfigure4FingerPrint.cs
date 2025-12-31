using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002EF RID: 751
	public partial class dfrmTCPIPConfigure4FingerPrint : frmN3000
	{
		// Token: 0x060015A8 RID: 5544 RVA: 0x001B3474 File Offset: 0x001B2474
		public dfrmTCPIPConfigure4FingerPrint()
		{
			this.InitializeComponent();
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x001B34D0 File Offset: 0x001B24D0
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

		// Token: 0x060015AA RID: 5546 RVA: 0x001B374C File Offset: 0x001B274C
		private void btnOption_Click(object sender, EventArgs e)
		{
			base.Size = new Size(base.Width, this.grpPort.Location.Y + this.grpPort.Height * 2);
			this.btnOption.Enabled = false;
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x001B3798 File Offset: 0x001B2798
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

		// Token: 0x060015AC RID: 5548 RVA: 0x001B3908 File Offset: 0x001B2908
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

		// Token: 0x04002CC7 RID: 11463
		public string strGateway = "";

		// Token: 0x04002CC8 RID: 11464
		public string strIP = "";

		// Token: 0x04002CC9 RID: 11465
		public string strMac = "";

		// Token: 0x04002CCA RID: 11466
		public string strMask = "";

		// Token: 0x04002CCB RID: 11467
		public string strSN = "";

		// Token: 0x04002CCC RID: 11468
		public string strTCPPort = "";
	}
}
