using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000029 RID: 41
	public partial class dfrmRegister : frmN3000
	{
		// Token: 0x060002F8 RID: 760 RVA: 0x0005A8D5 File Offset: 0x000598D5
		public dfrmRegister()
		{
			this.InitializeComponent();
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0005A8E4 File Offset: 0x000598E4
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				string text = this.txtRegisterCode.Text.Trim();
				if (string.IsNullOrEmpty(text))
				{
					XMessageBox.Show(CommonStr.strInputRegisterSN, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else if (string.IsNullOrEmpty(this.txtCompanyName.Text.Trim()))
				{
					XMessageBox.Show(CommonStr.strInputCompanyName, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else if (string.IsNullOrEmpty(this.txtBuildingCompanyName.Text.Trim()))
				{
					XMessageBox.Show(CommonStr.strInputBuildingCompanyName, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					string text2 = "";
					string text3 = "";
					string text4 = "";
					text = text.Replace("－", "-");
					string text5;
					if (wgAppConfig.getSystemParamValue(12, out text3, out text2, out text5) > 0 && text2.Replace("－", "-").IndexOf("-") > 0)
					{
						string text6 = text2.Substring(text2.Replace("－", "-").IndexOf("-"));
						if (text.Replace("－", "-").IndexOf("-") <= 0)
						{
							XMessageBox.Show(CommonStr.strRegisterSNPasswordWrong);
							return;
						}
						if (text6 != text.Substring(text.Replace("－", "-").IndexOf("-")))
						{
							XMessageBox.Show(CommonStr.strRegisterSNPasswordWrong);
							return;
						}
					}
					if (text.Replace("－", "-").IndexOf("-") >= 0)
					{
						text4 = text.Substring(text.Replace("－", "-").IndexOf("-"));
						text = text.Substring(0, text.Replace("－", "-").IndexOf("-"));
					}
					if (text == "2004")
					{
						string text7;
						string text8;
						wgAppConfig.getSystemParamValue(12, out text7, out text8, out text5);
						wgAppConfig.setSystemParamValue(12, text7, "200405", text5);
						wgAppConfig.setSystemParamValue(36, "", this.txtCompanyName.Text, this.txtBuildingCompanyName.Text);
						if (wgAppConfig.GetKeyVal("rgtries") != "")
						{
							wgAppConfig.UpdateKeyVal("rgtries", 1.ToString());
						}
						this.sendRegisterInfo();
						XMessageBox.Show(CommonStr.strRegisterSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						base.DialogResult = DialogResult.OK;
						base.Close();
					}
					else
					{
						string text9 = "";
						if (text.Length == 6 && text.Substring(0, 4) == "2006")
						{
							text9 = text.Substring(4, 2);
						}
						if (text.Length == 6 && text.Substring(0, 4) == "2014")
						{
							text9 = text.Substring(4, 2);
						}
						int num = 0;
						int.TryParse(text9, out num);
						if (num >= 1)
						{
							if (text.Length >= 6 && text.Substring(0, 4) == "2006")
							{
								num *= 30;
							}
							string text7;
							string text8;
							wgAppConfig.getSystemParamValue(12, out text7, out text8, out text5);
							text8 = (num + 1).ToString() + text4;
							text7 = DateTime.Now.ToString("yyyy-MM-dd");
							wgAppConfig.setSystemParamValue(12, text7, text8, text5);
							wgAppConfig.setSystemParamValue(36, "", this.txtCompanyName.Text, this.txtBuildingCompanyName.Text);
							if (wgAppConfig.GetKeyVal("rgtries") != "")
							{
								wgAppConfig.UpdateKeyVal("rgtries", 1.ToString());
							}
							this.sendRegisterInfo();
							XMessageBox.Show(CommonStr.strRegisterSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							base.DialogResult = DialogResult.OK;
							base.Close();
						}
						else
						{
							XMessageBox.Show(CommonStr.strRegisterSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0005AD00 File Offset: 0x00059D00
		private void dfrmRegister_Load(object sender, EventArgs e)
		{
			if (wgTools.gWGYTJ)
			{
				this.txtRegisterCode.Text = "2004";
				this.txtRegisterCode.Visible = false;
				this.label3.Visible = false;
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0005AD31 File Offset: 0x00059D31
		private void Exit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0005AD39 File Offset: 0x00059D39
		private void sendRegisterInfo()
		{
			new Thread(new ThreadStart(wgMail.sendMailOnce)).Start();
		}
	}
}
