using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000003 RID: 3
	public partial class dfrmAbout : frmN3000
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002220 File Offset: 0x00001220
		public dfrmAbout()
		{
			this.InitializeComponent();
			this.label1.Text = this.AssemblyProduct;
			this.label2.Text = string.Format("Version {0}", this.AssemblyVersion);
			this.label3.Text = this.AssemblyCopyright;
			this.label4.Text = "";
			this.textBoxDescription.Text = this.AssemblyDescription;
			this.textBoxDescription.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022B4 File Offset: 0x000012B4
		private void btnClose_Click(object sender, EventArgs e)
		{
			if (this.bNeedReboot)
			{
				Thread.Sleep(300);
				XMessageBox.Show(CommonStr.strRegisterAndRestart);
				Cursor.Current = Cursors.WaitCursor;
				Thread.Sleep(3000);
				try
				{
					wgAppConfig.gRestart = true;
					(base.Owner as frmADCT3000).mnuExit.PerformClick();
				}
				catch (Exception)
				{
				}
			}
			base.Close();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002328 File Offset: 0x00001328
		private void btnRegister_Click(object sender, EventArgs e)
		{
			using (dfrmRegister dfrmRegister = new dfrmRegister())
			{
				if (dfrmRegister.ShowDialog(this) == DialogResult.OK)
				{
					this.bNeedReboot = true;
					if (this.bNeedReboot)
					{
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							wgAppConfig.gRestart = true;
							(base.Owner as frmADCT3000).mnuExit.PerformClick();
						}
						catch (Exception)
						{
						}
						base.Close();
					}
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023AC File Offset: 0x000013AC
		private void checkAndUpradeSoftwareToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.checkAndUpradeSoftwareToolStripMenuItem.Enabled = false;
			Program.startSlowThreadUpg = new Thread(new ThreadStart(Program.getNewSoftware));
			Program.startSlowThreadUpg.IsBackground = true;
			Program.startSlowThreadUpg.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			Program.startSlowThreadUpg.Start();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002408 File Offset: 0x00001408
		private void dfrmAbout_KeyDown(object sender, KeyEventArgs e)
		{
			if (!this.bDispSpecial && e.Control && e.Shift && e.KeyValue == 87)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.bDispSpecial = true;
				this.textBoxDescription.Text = this.textBoxDescription.Text + "\r\n2010 版权所有(C) 深圳市门禁实业有限公司\r\n保留所有权利";
				this.label5.Text = this.textBoxDescription.Text.Replace("\r\n", "\r\n\r\n");
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024A4 File Offset: 0x000014A4
		private void dfrmAbout_Load(object sender, EventArgs e)
		{
			this.textBoxDescription.ForeColor = Color.White;
			this.label1.Text = base.Owner.Text;
			this.label2.Text = string.Format("Software Version: {0}", this.AssemblyVersion);
			this.label3.Text = "Database Version: " + wgAppConfig.getSystemParamByNO(9) + (wgAppConfig.IsAccessDB ? "  [Microsoft Access]" : "[MS Sql Server]");
			this.label4.Text = string.Format(".Net Framework {0} ", Environment.Version.ToString());
			this.label5.Text = "";
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(12)) && wgAppConfig.getSystemParamByNO(12) == "200405")
				{
					this.textBoxDescription.Text = this.textBoxDescription.Text + "\r\n" + CommonStr.strRegisterAlready;
					if (!string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(36)))
					{
						this.textBoxDescription.Text = this.textBoxDescription.Text + "\r\n" + wgAppConfig.getSystemParamByNO(36);
					}
					this.textBoxDescription.Text = this.textBoxDescription.Text + "\r\n" + CommonStr.strWelcomeToUse;
					this.label5.Text = this.textBoxDescription.Text.Replace("\r\n", "\r\n\r\n");
					this.btnRegister.Text = CommonStr.strRegisterAgain;
				}
			}
			catch (Exception)
			{
			}
			if (!string.IsNullOrEmpty(wgAppConfig.getTriggerName()))
			{
				wgAppConfig.wgLog(wgAppConfig.getTriggerName());
				this.textBoxDescription.Text = this.textBoxDescription.Text + "\r\n" + wgAppConfig.getTriggerName();
				this.label5.Text = this.textBoxDescription.Text.Replace("\r\n", "\r\n\r\n");
				this.label6.Visible = false;
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000026AC File Offset: 0x000016AC
		public string AssemblyCompany
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)customAttributes[0]).Company;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000026E8 File Offset: 0x000016E8
		public string AssemblyCopyright
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002724 File Offset: 0x00001724
		public string AssemblyDescription
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002760 File Offset: 0x00001760
		public string AssemblyProduct
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)customAttributes[0]).Product;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000279C File Offset: 0x0000179C
		public string AssemblyTitle
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (customAttributes.Length > 0)
				{
					AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)customAttributes[0];
					if (assemblyTitleAttribute.Title != "")
					{
						return assemblyTitleAttribute.Title;
					}
				}
				return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000027F6 File Offset: 0x000017F6
		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		// Token: 0x04000004 RID: 4
		private bool bDispSpecial;

		// Token: 0x04000005 RID: 5
		private bool bNeedReboot;
	}
}
