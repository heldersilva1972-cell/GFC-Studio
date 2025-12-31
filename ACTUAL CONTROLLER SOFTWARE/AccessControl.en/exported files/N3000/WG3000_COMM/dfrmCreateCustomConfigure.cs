using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM
{
	// Token: 0x02000228 RID: 552
	public partial class dfrmCreateCustomConfigure : frmN3000
	{
		// Token: 0x0600101B RID: 4123 RVA: 0x00120DBE File Offset: 0x0011FDBE
		public dfrmCreateCustomConfigure()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00120DCC File Offset: 0x0011FDCC
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00120DD4 File Offset: 0x0011FDD4
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this.txtPasswordNew.Text != this.txtPasswordNewConfirm.Text)
			{
				XMessageBox.Show(this, CommonStr.strPwdNotSame, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.validColor(this.txtBackColor1.Text.Trim()) >= 0 && this.validColor(this.txtBackColor2.Text.Trim()) >= 0 && this.validColor(this.txtBackColor4.Text.Trim()) >= 0 && this.validColor(this.txtBackColor5.Text.Trim()) >= 0 && this.validColor(this.txtBackColor13.Text.Trim()) >= 0 && this.validColor(this.txtBackColor14.Text.Trim()) >= 0 && this.validColor(this.txtBackColor15.Text.Trim()) >= 0)
			{
				string text = Application.StartupPath + "\\n3k_cust.xmlAA";
				if (File.Exists(text))
				{
					new FileInfo(text).Delete();
				}
				if (!string.IsNullOrEmpty(this.txtCustType.Text))
				{
					string text2 = this.txtCustType.Text.Trim();
					this.SaveNewXmlFile4AA("KEY_CUSTOMTYPE", text2);
				}
				if (!string.IsNullOrEmpty(this.txtPasswordNew.Text))
				{
					this.SaveNewXmlFile4AA("CommPCurrent", WGPacket.Ept(WGPacket.Ept(this.txtPasswordNew.Text)));
				}
				this.SaveNewXmlFile4AA("Custom Title", this.txtTitle.Text.Trim());
				this.SaveNewXmlFile4AA("Custom Logo", this.txtLogo.Text.Trim());
				this.SaveNewXmlFile4AA("KeyWindows_Backcolor1", this.txtBackColor1.Text.Trim());
				this.SaveNewXmlFile4AA("KeyWindows_Backcolor2", this.txtBackColor2.Text.Trim());
				this.SaveNewXmlFile4AA("KeyWindows_Backcolor4", this.txtBackColor4.Text.Trim());
				this.SaveNewXmlFile4AA("KeyWindows_Backcolor5", this.txtBackColor5.Text.Trim());
				this.SaveNewXmlFile4AA("KeyWindows_Backcolor13", this.txtBackColor13.Text.Trim());
				this.SaveNewXmlFile4AA("KeyWindows_Backcolor14", this.txtBackColor14.Text.Trim());
				this.SaveNewXmlFile4AA("KeyWindows_Backcolor15", this.txtBackColor15.Text.Trim());
				if (this.chkHideLogo.Checked)
				{
					this.SaveNewXmlFile4AA("KEY_HideLogo", "1");
				}
				else
				{
					this.SaveNewXmlFile4AA("KEY_HideLogo", "");
				}
				if (this.chkHideMainMenu.Checked)
				{
					this.SaveNewXmlFile4AA("KEY_HideMainMenu", "1");
				}
				else
				{
					this.SaveNewXmlFile4AA("KEY_HideMainMenu", "");
				}
				if (this.chkHideStatusBar.Checked)
				{
					this.SaveNewXmlFile4AA("KEY_HideStbRunInfo", "1");
				}
				else
				{
					this.SaveNewXmlFile4AA("KEY_HideStbRunInfo", "");
				}
				this.createBackgroundPNGInfoToolStripMenuItem_Click(null, null);
				if (File.Exists(text))
				{
					XMessageBox.Show(string.Format("{0}: {1}  {2}", this.btnOk.Text, "n3k_cust.xmlAA", CommonStr.strSuccessfully));
				}
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00121110 File Offset: 0x00120110
		private void createBackgroundPNGInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = "";
			string[] array = new string[]
			{
				"pChild_title", "pConsole_Door_NormalOpen", "pLogin_bk", "pMain_Bookmark_bkg", "pMain_Bookmark_focus", "pMain_Bookmark_normal", "pMain_bottom", "pMain_button_normal", "pMain_icon_bkg", "pMain_icon_focus02",
				"pTools_second_title", "pTools_third_title"
			};
			for (int i = 0; i < array.Length; i++)
			{
				string custBackImage = dfrmCreateCustomConfigure.getCustBackImage(array[i]);
				if (!string.IsNullOrEmpty(custBackImage))
				{
					if (string.IsNullOrEmpty(text))
					{
						text = custBackImage;
					}
					else
					{
						text = text + ";\r\n" + custBackImage;
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.SaveNewXmlFile4AA("KEYS_CustomBackgroud", wgTools.SetObjToStr(text).Trim());
			}
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x001211FC File Offset: 0x001201FC
		private void dfrmCreateCustomConfigure_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
				{
					dfrmInputNewName.setPasswordChar('*');
					if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK && dfrmInputNewName.strNewName.ToUpper() == "5678")
					{
						this.chkHideLogo.Visible = true;
						this.txtCustType.Visible = true;
						this.label4.Visible = true;
					}
				}
			}
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x001212B8 File Offset: 0x001202B8
		private void dfrmCreateCustomConfigure_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.GetKeyVal("Custom Title") != "")
			{
				this.txtTitle.Text = wgAppConfig.GetKeyVal("Custom Title");
			}
			if (wgAppConfig.GetKeyVal("Custom Logo") != "")
			{
				this.txtLogo.Text = wgAppConfig.GetKeyVal("Custom Logo");
			}
			if (wgAppConfig.GetKeyVal("KEY_HideLogo") != "" && wgAppConfig.GetKeyVal("KEY_HideLogo") == "1")
			{
				this.chkHideLogo.Checked = true;
			}
			if (wgAppConfig.GetKeyVal("KEY_HideMainMenu") == "1")
			{
				this.chkHideMainMenu.Checked = true;
			}
			if (this.onlyLogo)
			{
				this.groupBox2.Visible = false;
				this.label4.Visible = false;
				this.txtCustType.Visible = false;
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x001213A4 File Offset: 0x001203A4
		public static string getCustBackImage(string imageName)
		{
			string text = null;
			try
			{
				string text2 = Application.StartupPath + "\\" + imageName + ".png";
				if (!File.Exists(text2))
				{
					return text;
				}
				using (FileStream fileStream = new FileStream(text2, FileMode.Open, FileAccess.Read))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					text = BitConverter.ToString(array).Replace("-", "");
					text = imageName + "," + text;
				}
			}
			catch (Exception)
			{
			}
			return text;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00121450 File Offset: 0x00120450
		private void SaveNewXmlFile4AA(string key, string value)
		{
			wgAppConfig.UpdateKeyVal(key, value);
			if (!string.IsNullOrEmpty(value))
			{
				string startupPath = Application.StartupPath;
				string text = startupPath + "\\n3k_cust.xmlAA";
				string text2 = startupPath + "\\photo\\n3k_cust.xmlAA";
				string text3 = wgAppConfig.defaultCustConfigzhCHS;
				if (!wgAppConfig.IsChineseSet(Thread.CurrentThread.CurrentUICulture.Name))
				{
					text3 = text3.Replace("zh-CHS", "en");
				}
				if (!File.Exists(text))
				{
					if (File.Exists(text2))
					{
						using (StreamReader streamReader = new StreamReader(text2))
						{
							string text4 = streamReader.ReadToEnd();
							if (text4.Length > 1000)
							{
								text3 = text4;
							}
						}
					}
					using (StreamWriter streamWriter = new StreamWriter(text, false))
					{
						streamWriter.WriteLine(text3);
					}
				}
				if (File.Exists(text))
				{
					wgAppConfig.UpdateKeyVal(key, value, text);
				}
			}
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x00121548 File Offset: 0x00120548
		private int validColor(string strColor)
		{
			int num = 0;
			try
			{
				if (!string.IsNullOrEmpty(strColor))
				{
					string[] array = strColor.Split(new char[] { ',' });
					if (array.Length == 3)
					{
						Color.FromArgb(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()));
						num = 1;
					}
				}
			}
			catch
			{
			}
			if (num != 1 && !string.IsNullOrEmpty(strColor))
			{
				num = -1;
				XMessageBox.Show(strColor + "  " + CommonStr.strInvalidValue);
			}
			return num;
		}

		// Token: 0x04001C9A RID: 7322
		private const string n3k_cust = "\\n3k_cust.xml";

		// Token: 0x04001C9B RID: 7323
		public bool onlyLogo;
	}
}
