using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000032 RID: 50
	public partial class dfrmUpdateOnline : frmN3000
	{
		// Token: 0x06000370 RID: 880 RVA: 0x000653FA File Offset: 0x000643FA
		public dfrmUpdateOnline()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00065430 File Offset: 0x00064430
		private void btnExit_Click(object sender, EventArgs e)
		{
			try
			{
				this.startSlowThread.Interrupt();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			base.Close();
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00065480 File Offset: 0x00064480
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00065488 File Offset: 0x00064488
		private void dfrmUpdateOnline_Load(object sender, EventArgs e)
		{
			this.txtInfo.Text = CommonStr.strCheckingNewSoftwareVersion;
			this.startSlowThread = new Thread(new ThreadStart(this.getNewSoftware));
			this.startSlowThread.IsBackground = true;
			this.startSlowThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			this.startSlowThread.Start();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000654E9 File Offset: 0x000644E9
		public void displayInfo()
		{
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000654EC File Offset: 0x000644EC
		public void getNewSoftware()
		{
			try
			{
				string text = dfrmUpdateOnline.GetVersionFile();
				if (string.IsNullOrEmpty(text))
				{
					this.txtInfoText = CommonStr.strCheckingNewSoftwareCurrentSoftwareNewest;
					this.btnExitText = CommonStr.strCheckingNewSoftwareExit;
					this.pictVisible = false;
				}
				else if (text == "NOTCONNECT")
				{
					this.txtInfoText = CommonStr.strCheckingNewSoftwareServerNotConnected;
					this.btnExitText = CommonStr.strCheckingNewSoftwareExit;
					this.pictVisible = false;
				}
				else if (!string.IsNullOrEmpty(text))
				{
					text = Program.Dpt4Database(text);
					string[] array = text.Split(new char[] { ';' });
					if (wgTools.CmpProductVersion(array[1], wgAppConfig.GetKeyVal("NewSoftwareSpecialVersionInfo")) <= 0)
					{
						this.txtInfoText = CommonStr.strCheckingNewSoftwareCurrentSoftwareNewest;
						this.btnExitText = CommonStr.strCheckingNewSoftwareExit;
						this.pictVisible = false;
					}
					else if (text.ToUpper().IndexOf("FORCEUPGRADE") <= 0 && XMessageBox.Show(CommonStr.strNewSoftwareNeedUpgrade + "\r\n\r\n V" + array[1], wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
					{
						this.bCloseWindow = true;
						this.btnExitText = CommonStr.strCheckingNewSoftwareExit;
						this.pictVisible = false;
					}
					else
					{
						this.txtInfoText = CommonStr.strCheckingNewSoftwareDownloadingSoftware + "V" + array[1];
						if (array[2].IndexOf("2") > 0)
						{
							this.newsoftFilename = dfrmUpdateOnline.getSoftwareRar(array[2].Replace("2", wgTools.gate.ToString()));
						}
						else
						{
							this.newsoftFilename = dfrmUpdateOnline.getSoftwareRar(array[2]);
						}
						if (this.newsoftFilename == "")
						{
							this.btnExitText = CommonStr.strCheckingNewSoftwareExit;
							this.pictVisible = false;
						}
						else
						{
							FileInfo fileInfo = new FileInfo(this.newsoftFilename);
							if (!fileInfo.Exists)
							{
								this.btnExitText = CommonStr.strCheckingNewSoftwareExit;
								this.pictVisible = false;
							}
							else
							{
								wgAppConfig.UpdateKeyVal("NewSoftwareSpecialVersionInfo", array[1]);
								GZip.Decompress(fileInfo.Directory.FullName, Application.StartupPath, fileInfo.Name);
								Interaction.Shell(array[3], AppWinStyle.Hide, false, -1);
								Thread.Sleep(5000);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00065738 File Offset: 0x00064738
		private static string getSoftwareRar(string newfilename)
		{
			string text = "";
			try
			{
				using (WebClient webClient = new WebClient())
				{
					string text2 = Application.StartupPath + "\\PHOTO\\mj3ksp" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".gz";
					webClient.DownloadFile(dfrmUpdateOnline.downweb + newfilename, text2);
					FileInfo fileInfo = new FileInfo(text2);
					if (fileInfo.Exists)
					{
						text = text2;
					}
				}
			}
			catch (Exception)
			{
			}
			return text;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000657CC File Offset: 0x000647CC
		private static string GetVersionFile()
		{
			string text = "NOTCONNECT";
			try
			{
				using (WebClient webClient = new WebClient())
				{
					string text2 = Application.StartupPath + "\\PHOTO\\mj3kver" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt";
					webClient.DownloadFile(dfrmUpdateOnline.downweb + "mj3kverM.txt", text2);
					FileInfo fileInfo = new FileInfo(text2);
					if (fileInfo.Exists)
					{
						using (StreamReader streamReader = new StreamReader(text2))
						{
							text = streamReader.ReadToEnd();
						}
						fileInfo.Delete();
					}
					return text;
				}
			}
			catch (Exception)
			{
			}
			return text;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00065894 File Offset: 0x00064894
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.txtInfo.Text = this.txtInfoText;
			if (this.btnExitText != "")
			{
				this.btnExit.Text = this.btnExitText;
			}
			this.PictureBox1.Visible = this.pictVisible;
			if (this.txtInfoText == CommonStr.strCheckingNewSoftwareDownloadSuccessAndRestart)
			{
				this.btnOK.Enabled = true;
				this.btnOK.Select();
				this.btnExitText = CommonStr.strExit;
			}
			this.PictureBox1.Visible = this.pictVisible;
			if (this.bCloseWindow)
			{
				base.Close();
			}
		}

		// Token: 0x040006A9 RID: 1705
		private bool bCloseWindow;

		// Token: 0x040006AA RID: 1706
		private Thread startSlowThread;

		// Token: 0x040006AB RID: 1707
		private string btnExitText = "";

		// Token: 0x040006AC RID: 1708
		private static string downweb = "http://www.znsmart.com/down/";

		// Token: 0x040006AD RID: 1709
		private string newsoftFilename = "";

		// Token: 0x040006AE RID: 1710
		private bool pictVisible = true;

		// Token: 0x040006AF RID: 1711
		private string txtInfoText = "";
	}
}
