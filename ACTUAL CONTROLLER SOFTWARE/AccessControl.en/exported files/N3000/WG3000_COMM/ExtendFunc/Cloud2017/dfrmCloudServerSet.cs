using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Cloud2017
{
	// Token: 0x02000231 RID: 561
	public partial class dfrmCloudServerSet : frmN3000
	{
		// Token: 0x060010D0 RID: 4304 RVA: 0x00132DD0 File Offset: 0x00131DD0
		public dfrmCloudServerSet()
		{
			this.InitializeComponent();
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00132DDE File Offset: 0x00131DDE
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00132DF0 File Offset: 0x00131DF0
		private void btnDeactivate_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnReset.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				this.controlConfigure = new wgMjControllerConfigure();
				this.controlConfigure.dataServerIP = IPAddress.Parse("0.0.0.0");
				this.controlConfigure.dataServerPort = 0;
				this.controlConfigure.dataServerCycle = 0;
				this.controlConfigure.dataServerShortIP = IPAddress.Parse("0.0.0.0");
				this.controlConfigure.dataServerShortPort = 0;
				this.controlConfigure.dataServerShortOption = 0;
				this.controlConfigure.dataServerShortCycle = 0;
				this.controlConfigure.dataServer3ShortIP = IPAddress.Parse("0.0.0.0");
				this.controlConfigure.dataServer3ShortPort = 0;
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00132ED0 File Offset: 0x00131ED0
		private void btnOK_Click(object sender, EventArgs e)
		{
			int num = wgTools.bUDPCloud;
			int num2 = 0;
			int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_ENABLE")), out num2);
			num = num2;
			wgAppConfig.UpdateKeyVal("KEY_UDPCLOUDSERVER_ENABLE", this.chkActiveCloud.Checked ? "1" : "0");
			int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_ENABLE")), out num2);
			wgAppConfig.UpdateKeyVal("KEY_UDPCLOUDSERVER_IP", this.txtHostIP.Text);
			wgTools.UDPCloudIP = wgTools.validCloudServerIP(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_IP")));
			wgAppConfig.UpdateKeyVal("KEY_UDPCLOUDSERVER_PORT", this.txtPort.Text);
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_PORT"))))
			{
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_PORT")), out wgTools.UDPCloudPort);
				this.txtPort.Text = wgTools.UDPCloudPort.ToString();
			}
			wgAppConfig.UpdateKeyVal("KEY_UDPCLOUDSERVER_CYCLE", this.numericUpDown43.Value.ToString());
			wgAppConfig.UpdateKeyVal("KEY_UDPCLOUDSERVER_DHCP", this.chkDHCP.Checked ? "1" : "0");
			wgAppConfig.UpdateKeyVal("KEY_UDPCLOUDSERVER_SHORTPORT", this.txtPortShort.Text);
			wgAppConfig.UpdateKeyVal("KEY_UDPCLOUDSERVER2_IP", this.txtHostIP2.Text);
			wgAppConfig.UpdateKeyVal("KEY_UDPCLOUDSERVER2_SHORTPORT", this.txtPortShort2.Text);
			this.controlConfigure = new wgMjControllerConfigure();
			if (string.IsNullOrEmpty(this.txtHostIP.Text) || string.IsNullOrEmpty(this.txtPort.Text))
			{
				this.controlConfigure.dataServerIP = IPAddress.Parse("0.0.0.0");
				this.controlConfigure.dataServerPort = 0;
				this.controlConfigure.dataServerCycle = 0;
			}
			else
			{
				this.controlConfigure.dataServerIP = IPAddress.Parse(this.txtHostIP.Text);
				this.controlConfigure.dataServerPort = int.Parse(this.txtPort.Text);
				this.controlConfigure.dataServerCycle = int.Parse(this.numericUpDown43.Value.ToString());
			}
			if (string.IsNullOrEmpty(this.txtHostIP.Text) || string.IsNullOrEmpty(this.txtPortShort.Text))
			{
				this.controlConfigure.dataServerShortIP = IPAddress.Parse("0.0.0.0");
				this.controlConfigure.dataServerShortPort = 0;
				this.controlConfigure.dataServerShortOption = 0;
				this.controlConfigure.dataServerShortCycle = int.Parse(this.numericUpDown43.Value.ToString());
			}
			else
			{
				this.controlConfigure.dataServerShortIP = IPAddress.Parse(this.txtHostIP.Text);
				this.controlConfigure.dataServerShortPort = int.Parse(this.txtPortShort.Text);
				this.controlConfigure.dataServerShortOption = 0;
				this.controlConfigure.dataServerShortCycle = int.Parse(this.numericUpDown43.Value.ToString());
			}
			if (string.IsNullOrEmpty(this.txtHostIP2.Text) || string.IsNullOrEmpty(this.txtPortShort2.Text))
			{
				this.controlConfigure.dataServer3ShortIP = IPAddress.Parse("0.0.0.0");
				this.controlConfigure.dataServer3ShortPort = 0;
			}
			else
			{
				this.controlConfigure.dataServer3ShortIP = IPAddress.Parse(this.txtHostIP2.Text);
				this.controlConfigure.dataServer3ShortPort = int.Parse(this.txtPortShort2.Text);
			}
			if (this.chkDHCP.Checked)
			{
				this.controlConfigure.dhcpEnable = 165;
				this.controlConfigure.mobile_web_autoip_disable = 165;
			}
			base.DialogResult = DialogResult.OK;
			if (num != num2 && XMessageBox.Show(this, CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				wgAppConfig.gRestart = true;
			}
			base.Close();
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x001332AC File Offset: 0x001322AC
		private void dfrmCloudServerSet_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.Shift && e.KeyValue == 81)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						this.label199.Visible = true;
						this.label3.Visible = true;
						this.txtPort.Visible = true;
						this.txtPortShort.Visible = true;
						this.groupBox1.Visible = true;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00133350 File Offset: 0x00132350
		private void dfrmCloudServerSet_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_ENABLE"))))
			{
				int num = 0;
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_ENABLE")), out num);
				this.chkActiveCloud.Checked = num > 0;
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_IP"))))
			{
				this.txtHostIP.Text = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_IP"));
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_CYCLE"))))
			{
				int num2 = 5;
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_CYCLE")), out num2);
				this.numericUpDown43.Value = num2;
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_SHORTPORT"))))
			{
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_SHORTPORT")), out wgTools.UDPCloudShortPort);
				this.txtPortShort.Text = wgTools.UDPCloudShortPort.ToString();
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER2_SHORTPORT"))))
			{
				int num3 = 0;
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER2_SHORTPORT")), out num3);
				this.txtPortShort2.Text = num3.ToString();
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER2_IP"))))
			{
				this.txtHostIP2.Text = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER2_IP"));
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_PORT"))))
			{
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_PORT")), out wgTools.UDPCloudPort);
				this.txtPort.Text = wgTools.UDPCloudPort.ToString();
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_DHCP"))) && wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_DHCP") == "1")
			{
				this.chkDHCP.Checked = true;
			}
		}

		// Token: 0x04001DE6 RID: 7654
		public wgMjControllerConfigure controlConfigure;
	}
}
