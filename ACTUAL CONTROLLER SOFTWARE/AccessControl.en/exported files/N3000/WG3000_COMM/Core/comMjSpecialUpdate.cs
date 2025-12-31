using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Core
{
	// Token: 0x020001C2 RID: 450
	public class comMjSpecialUpdate : Component
	{
		// Token: 0x0600095B RID: 2395 RVA: 0x000DD4BE File Offset: 0x000DC4BE
		public comMjSpecialUpdate()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x000DD4CC File Offset: 0x000DC4CC
		public comMjSpecialUpdate(IContainer Container)
			: this()
		{
			Container.Add(this);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x000DD4DB File Offset: 0x000DC4DB
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x000DD4FC File Offset: 0x000DC4FC
		private static string getSoftwareRar(string newfilename)
		{
			string text = "";
			try
			{
				using (WebClient webClient = new WebClient())
				{
					string text2 = Application.StartupPath + "\\PHOTO\\mj3ksp" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".gz";
					if (!comMjSpecialUpdate.downweb.Equals(Program.Dpt4Database("7y5zXyIml8RfT71526QmkgEc0qv5/Kxm+gF6JL1BtDE=")))
					{
						Application.Exit();
					}
					webClient.DownloadFile(comMjSpecialUpdate.downweb + newfilename, text2);
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

		// Token: 0x0600095F RID: 2399 RVA: 0x000DD5AC File Offset: 0x000DC5AC
		private static string GetSpSN()
		{
			string text = "";
			try
			{
				using (WebClient webClient = new WebClient())
				{
					string text2 = Application.StartupPath + "\\PHOTO\\mj3kSpSN" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt";
					if (!comMjSpecialUpdate.downweb.Equals(Program.Dpt4Database("+2XirTYKVg8XcQPFCm+nBcMmBmPy1hAGz3QxwXFEmfU=")))
					{
						Application.Exit();
					}
					webClient.DownloadFile(comMjSpecialUpdate.downweb + "mj3kSpSN.txt", text2);
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

		// Token: 0x06000960 RID: 2400 RVA: 0x000DD690 File Offset: 0x000DC690
		private static string GetVersionFile()
		{
			string text = "";
			try
			{
				using (WebClient webClient = new WebClient())
				{
					string text2 = Application.StartupPath + "\\PHOTO\\mj3kver" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt";
					if (!comMjSpecialUpdate.downweb.Equals(Program.Dpt4Database("+2XirTYKVg8XcQPFCm+nBcMmBmPy1hAGz3QxwXFEmfU=")))
					{
						Application.Exit();
					}
					webClient.DownloadFile(comMjSpecialUpdate.downweb + "mj3kver2.txt", text2);
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

		// Token: 0x06000961 RID: 2401 RVA: 0x000DD774 File Offset: 0x000DC774
		private void InitializeComponent()
		{
			this.components = new Container();
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000DD784 File Offset: 0x000DC784
		public static bool updateMjSpecialSoftware()
		{
			string text = "";
			try
			{
				comMjSpecialUpdate.downweb = "http://www.znsmart.com/down/";
				text = comMjSpecialUpdate.GetVersionFile();
				text == "";
				try
				{
					string spSN = comMjSpecialUpdate.GetSpSN();
					if (!string.IsNullOrEmpty(spSN))
					{
						string text2 = spSN;
						string[] array = Program.Dpt4Database(spSN).Split(new char[] { ';' });
						if (array.Length >= 2)
						{
							byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(array[0]);
							byte[] bytes2 = Encoding.GetEncoding("utf-8").GetBytes(array[1]);
							RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
							rsacryptoServiceProvider.FromXmlString(Program.Dpt4Database("luSWeZHP2nqo3pdFCebDHs+68zQSy1zySUyIfugXbxng9FC9VtDjhCEUGq8FtduQjsx4RI6mijLelsGsiZzuJK5WdhdlaaXFgow2i66loEpn6rzdMYIbRX4EsyukBnxXVVQSPxt4Fg7bfrPW7nxT5RpaidSUAreDTibTYomjbifQDpBu47hGJ1+xuQrs8tp7C/kK+v2+Gt3F+eKngXG6WMS6r6cbggTaiKvTKVqERVNVcmuCJFksW7GhN2ci03fz6NdF+MoF8gGKQDYEhOv60LNgkc1Z9EEQoq2jhIHdpaUmggm9hWUP1IbQ31EuGrpuc3R9IzlACjnBAZi057LSpOyazjLn0AryKJPWXuO2VjGyXIxBF9IurWwiYhwzc8tuO7Nb1H0tnQyFapnfoM0huHkZzY6GAhB/B52BYe47b0vnvodVMZBU/3aG+Ry2IKxnE3bzsL5CiUuhXxIPNCUj2Xu1n1PL+J6JXFWCqj0liohQ7wg4tY2SujBm17+wsyJVPMAoaMPDbZA0WE19+VE5VvHmAoHAuo6nHFePaFE46Rk="));
							if (rsacryptoServiceProvider.VerifyData(bytes, Program.Dpt4Database("7TbkKQVZ4Al3BOP7opHTFQ=="), bytes2))
							{
								string systemParamNotes = wgAppConfig.getSystemParamNotes(190);
								if (string.IsNullOrEmpty(systemParamNotes) || systemParamNotes.IndexOf(text2) < 0)
								{
									wgAppConfig.setSystemParamValue(190, "Activate SpSN", "1", text2);
								}
							}
						}
					}
				}
				catch
				{
				}
				DateTime dateTime = DateTime.Parse("2012-12-1");
				wgAppConfig.UpdateKeyVal("RunTimeAt", (DateTime.Now.Subtract(dateTime).Days + 31).ToString());
				wgAppConfig.wgLog("GetNewSpecialSoft: " + text, EventLogEntryType.Information, null);
				if (!string.IsNullOrEmpty(text))
				{
					text = Program.Dpt4Database(text);
					string[] array2 = text.Split(new char[] { ';' });
					bool flag = false;
					if (wgTools.CmpProductVersion(array2[1], wgAppConfig.GetKeyVal("NewSoftwareSpecialVersionInfo")) > 0)
					{
						flag = true;
					}
					if (!flag)
					{
						return false;
					}
					if (text.ToUpper().IndexOf("FORCEUPGRADE") <= 0)
					{
						Thread.Sleep(3000);
						if (XMessageBox.Show(CommonStr.strNewSoftwareNeedUpgrade, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
						{
							return false;
						}
					}
					string text3;
					if (array2[2].IndexOf("2") > 0)
					{
						text3 = comMjSpecialUpdate.getSoftwareRar(array2[2].Replace("2", wgTools.gate.ToString()));
					}
					else
					{
						text3 = comMjSpecialUpdate.getSoftwareRar(array2[2]);
					}
					if (text3 == "")
					{
						return false;
					}
					FileInfo fileInfo = new FileInfo(text3);
					if (!fileInfo.Exists)
					{
						return false;
					}
					wgAppConfig.UpdateKeyVal("NewSoftwareSpecialVersionInfo", array2[1]);
					GZip.Decompress(fileInfo.Directory.FullName, Application.StartupPath, fileInfo.Name);
					Interaction.Shell(array2[3], AppWinStyle.Hide, false, -1);
					Thread.Sleep(5000);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return false;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x000DDA6C File Offset: 0x000DCA6C
		public static bool updateMjSpecialSoftwareOldV5xx()
		{
			try
			{
				comMjSpecialUpdate.downweb = "http://www.znsmart.com/down/";
				string text = comMjSpecialUpdate.GetVersionFile();
				text == "";
				DateTime dateTime = DateTime.Parse("2012-12-1");
				wgAppConfig.UpdateKeyVal("RunTimeAt", (DateTime.Now.Subtract(dateTime).Days + 31).ToString());
				wgAppConfig.wgLog("GetNewSpecialSoft: " + text, EventLogEntryType.Information, null);
				if (!string.IsNullOrEmpty(text))
				{
					text = Program.Dpt4Database(text);
					string[] array = text.Split(new char[] { ';' });
					bool flag = false;
					if (wgTools.CmpProductVersion(array[1], wgAppConfig.GetKeyVal("NewSoftwareSpecialVersionInfo")) != 0)
					{
						flag = true;
					}
					if (!flag)
					{
						return false;
					}
					if (text.ToUpper().IndexOf("FORCEUPGRADE") <= 0)
					{
						Thread.Sleep(3000);
						if (XMessageBox.Show(CommonStr.strNewSoftwareNeedUpgrade, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
						{
							return false;
						}
					}
					string text2;
					if (array[2].IndexOf("2") > 0)
					{
						text2 = comMjSpecialUpdate.getSoftwareRar(array[2].Replace("2", wgTools.gate.ToString()));
					}
					else
					{
						text2 = comMjSpecialUpdate.getSoftwareRar(array[2]);
					}
					if (text2 == "")
					{
						return false;
					}
					wgAppConfig.UpdateKeyVal("NewSoftwareSpecialVersionInfo", array[1]);
					Interaction.Shell(text2, AppWinStyle.Hide, false, -1);
					Thread.Sleep(5000);
					Interaction.Shell(array[3], AppWinStyle.Hide, false, -1);
					Thread.Sleep(5000);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return false;
		}

		// Token: 0x0400184B RID: 6219
		private Container components;

		// Token: 0x0400184C RID: 6220
		private static string downweb = "http://www.znsmart.com/down/";
	}
}
