using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.DataOper;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Core
{
	// Token: 0x020001EA RID: 490
	internal class wgAppConfig
	{
		// Token: 0x06000BAE RID: 2990 RVA: 0x000F1CFF File Offset: 0x000F0CFF
		private wgAppConfig()
		{
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x000F1D08 File Offset: 0x000F0D08
		public static void AdjustComboBoxDropDownListWidth(ComboBox cboGrp)
		{
			Graphics graphics = null;
			try
			{
				int num = cboGrp.Width;
				graphics = cboGrp.CreateGraphics();
				Font font = cboGrp.Font;
				int num2 = ((cboGrp.Items.Count > cboGrp.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0);
				foreach (object obj in cboGrp.Items)
				{
					if (obj != null)
					{
						int num3 = (int)graphics.MeasureString((string)obj, font).Width + num2;
						if (num < num3)
						{
							num = num3;
						}
					}
				}
				cboGrp.DropDownWidth = num;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x000F1DF8 File Offset: 0x000F0DF8
		public static void AdjustComboBoxDropDownListWidth(ToolStripComboBox cboGrp, ComboBox cboUsers)
		{
			Graphics graphics = null;
			try
			{
				int num = cboGrp.Width;
				graphics = cboUsers.CreateGraphics();
				Font font = cboGrp.Font;
				int num2 = ((cboGrp.Items.Count > cboGrp.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0);
				foreach (object obj in cboGrp.Items)
				{
					if (obj != null)
					{
						int num3 = (int)graphics.MeasureString((string)obj, font).Width + num2;
						if (num < num3)
						{
							num = num3;
						}
					}
				}
				cboGrp.DropDownWidth = num;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x000F1EE8 File Offset: 0x000F0EE8
		public static void backupBeforeExitByJustCopy()
		{
			try
			{
				if (wgAppConfig.IsAccessDB)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(wgAppConfig.BackupDir);
					if (!directoryInfo.Exists)
					{
						directoryInfo.Create();
						directoryInfo = new DirectoryInfo(wgAppConfig.BackupDir);
					}
					if (directoryInfo.Exists)
					{
						Cursor cursor = Cursor.Current;
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							string text = wgAppConfig.accessDbName + "_000.bak";
							FileInfo fileInfo = new FileInfo(wgAppConfig.BackupDir + text);
							FileInfo fileInfo2 = new FileInfo(wgAppConfig.BackupDir + wgAppConfig.accessDbName + "_001.bak");
							FileInfo fileInfo3 = new FileInfo(wgAppConfig.BackupDir + wgAppConfig.accessDbName + "_LASTDAY0.bak");
							FileInfo fileInfo4 = new FileInfo(wgAppConfig.BackupDir + wgAppConfig.accessDbName + "_LASTDAY1.bak");
							FileInfo fileInfo5 = new FileInfo(string.Format(Application.StartupPath + "\\t{0}.bak", wgAppConfig.accessDbName));
							try
							{
								if (fileInfo.Exists)
								{
									fileInfo.Attributes = FileAttributes.Archive;
								}
							}
							catch (Exception ex)
							{
								wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
							}
							try
							{
								if (fileInfo2.Exists)
								{
									fileInfo2.Attributes = FileAttributes.Archive;
								}
							}
							catch (Exception ex2)
							{
								wgAppConfig.wgDebugWrite(ex2.ToString(), EventLogEntryType.Error);
							}
							try
							{
								if (fileInfo.Exists)
								{
									fileInfo.Attributes = FileAttributes.Archive;
								}
							}
							catch (Exception ex3)
							{
								wgAppConfig.wgDebugWrite(ex3.ToString(), EventLogEntryType.Error);
							}
							try
							{
								if (fileInfo3.Exists)
								{
									fileInfo3.Attributes = FileAttributes.Archive;
								}
							}
							catch (Exception ex4)
							{
								wgAppConfig.wgDebugWrite(ex4.ToString(), EventLogEntryType.Error);
							}
							try
							{
								if (fileInfo4.Exists)
								{
									fileInfo4.Attributes = FileAttributes.Archive;
								}
							}
							catch (Exception ex5)
							{
								wgAppConfig.wgDebugWrite(ex5.ToString(), EventLogEntryType.Error);
							}
							try
							{
								if (fileInfo5.Exists)
								{
									fileInfo5.Attributes = FileAttributes.Archive;
								}
							}
							catch (Exception ex6)
							{
								wgAppConfig.wgDebugWrite(ex6.ToString(), EventLogEntryType.Error);
							}
							if (fileInfo.Exists)
							{
								if (fileInfo2.Exists && fileInfo.LastWriteTime.ToString("yyyyMMdd") != fileInfo2.LastWriteTime.ToString("yyyyMMdd"))
								{
									if (fileInfo3.Exists)
									{
										if (fileInfo4.Exists)
										{
											fileInfo4.Delete();
										}
										fileInfo3.MoveTo(wgAppConfig.BackupDir + wgAppConfig.accessDbName + "_LASTDAY1.bak");
									}
									fileInfo2.MoveTo(wgAppConfig.BackupDir + wgAppConfig.accessDbName + "_LASTDAY0.bak");
								}
								if (fileInfo2.FullName == wgAppConfig.BackupDir + wgAppConfig.accessDbName + "_001.bak" && fileInfo2.Exists)
								{
									fileInfo2.Delete();
								}
								fileInfo.MoveTo(wgAppConfig.BackupDir + wgAppConfig.accessDbName + "_001.bak");
							}
							fileInfo = new FileInfo(Application.StartupPath + string.Format("\\{0}.mdb", wgAppConfig.accessDbName));
							fileInfo.CopyTo(Application.StartupPath + string.Format("\\t{0}.bak", wgAppConfig.accessDbName), true);
							fileInfo.CopyTo(wgAppConfig.BackupDir + text, true);
						}
						catch (Exception ex7)
						{
							wgAppConfig.wgDebugWrite(ex7.ToString(), EventLogEntryType.Error);
						}
					}
				}
			}
			catch (Exception ex8)
			{
				wgAppConfig.wgDebugWrite(ex8.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x000F22C0 File Offset: 0x000F12C0
		public static void CardIDInput(ref MaskedTextBox mtb)
		{
			if (mtb.Text.Length != mtb.Text.Trim().Length)
			{
				mtb.Text = mtb.Text.Trim();
			}
			else if (mtb.Text.Length == 0 && mtb.SelectionStart != 0)
			{
				mtb.SelectionStart = 0;
			}
			if (mtb.Text.Length > 0)
			{
				if (mtb.Text.IndexOf(" ") > 0)
				{
					mtb.Text = mtb.Text.Replace(" ", "");
				}
				if (wgAppConfig.IsActivateCard19)
				{
					if (mtb.Text.Length != 19)
					{
						if (mtb.Text.Length == 20)
						{
							mtb.Text = mtb.Text.Substring(0, mtb.Text.Length - 1);
							return;
						}
					}
					else if (ulong.Parse(mtb.Text) >= 9223372036854775807UL)
					{
						mtb.Text = mtb.Text.Substring(0, mtb.Text.Length - 1);
						return;
					}
				}
				else if (mtb.Text.Length > 9 && long.Parse(mtb.Text) >= (long)((ulong)(-1)))
				{
					mtb.Text = mtb.Text.Substring(0, mtb.Text.Length - 1);
				}
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x000F2430 File Offset: 0x000F1430
		public static bool checkRSAController(long controllerSN)
		{
			bool flag = false;
			string text;
			string text2;
			wgAppConfig.getSystemParamValue(221, out text, out text, out text2);
			string text3;
			wgAppConfig.getSystemParamValue(222, out text, out text, out text3);
			if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3) && text2.IndexOf("SN=" + controllerSN) >= 0)
			{
				UTF8Encoding utf8Encoding = new UTF8Encoding();
				RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
				rsacryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>x9P3JYYMphmIFo5l1qCjU4wWogP1ORtuNrK+8mk9Z0aCljY/3eJP86gqcWdqnfiN4iTwSWoKdSYy2+YwMmLV1cZ1Ma0j6bRQLtQgFTcv2gpWkGomLYKCF3Ok1huyCdxNs6TDXdcGxOGJpQdL4TLDHRpfIKMcoLBGfiO/KZ5TI/2CPgc8TJfx9SCFf4C/07rnAq9CoTjK64ruhDgdOWBePcNNsz687eb1j5LUzr7jhl+mpuddk3bL8TZWDks48ueBIsdxhgEGlMmbFXQvrell0n9e7S8AYzVaVR4wrqAnU9TJje4B/vDL1de1qbKD+jYI5zIcNQjGVjXZro8mCI72fQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				byte[] array = Convert.FromBase64String(text3);
				byte[] bytes = utf8Encoding.GetBytes(text2);
				if (rsacryptoServiceProvider.VerifyData(bytes, "SHA1", array))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				text3 = "";
				string text4 = "";
				wgAppConfig.getSystemParamValue(223, out text, out text, out text4);
				if (!string.IsNullOrEmpty(text4))
				{
					string[] array2 = text4.Split(new char[] { ';' });
					if (array2.Length == 2)
					{
						text2 = array2[0];
						text3 = array2[1];
						if (text2.IndexOf("SN=" + controllerSN) >= 0)
						{
							UTF8Encoding utf8Encoding2 = new UTF8Encoding();
							RSACryptoServiceProvider rsacryptoServiceProvider2 = new RSACryptoServiceProvider();
							rsacryptoServiceProvider2.FromXmlString("<RSAKeyValue><Modulus>x9P3JYYMphmIFo5l1qCjU4wWogP1ORtuNrK+8mk9Z0aCljY/3eJP86gqcWdqnfiN4iTwSWoKdSYy2+YwMmLV1cZ1Ma0j6bRQLtQgFTcv2gpWkGomLYKCF3Ok1huyCdxNs6TDXdcGxOGJpQdL4TLDHRpfIKMcoLBGfiO/KZ5TI/2CPgc8TJfx9SCFf4C/07rnAq9CoTjK64ruhDgdOWBePcNNsz687eb1j5LUzr7jhl+mpuddk3bL8TZWDks48ueBIsdxhgEGlMmbFXQvrell0n9e7S8AYzVaVR4wrqAnU9TJje4B/vDL1de1qbKD+jYI5zIcNQjGVjXZro8mCI72fQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
							byte[] array3 = Convert.FromBase64String(text3);
							byte[] bytes2 = utf8Encoding2.GetBytes(text2);
							if (rsacryptoServiceProvider2.VerifyData(bytes2, "SHA1", array3))
							{
								flag = true;
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x000F2580 File Offset: 0x000F1580
		public static bool CreateCustXml()
		{
			string startupPath = Application.StartupPath;
			string text = startupPath + "\\n3k_cust.xml";
			if (File.Exists(text))
			{
				wgAppConfig.tryCreateCnt = 0;
				return true;
			}
			if (wgAppConfig.tryCreateCnt <= 5)
			{
				string text2 = startupPath + "\\photo\\n3k_cust.xmlAA";
				string text3 = wgAppConfig.defaultCustConfigzhCHS;
				if (!wgAppConfig.IsChineseSet(Thread.CurrentThread.CurrentUICulture.Name))
				{
					text3 = text3.Replace("zh-CHS", "en");
				}
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
				wgAppConfig.tryCreateCnt++;
				if (File.Exists(text))
				{
					wgAppConfig.tryCreateCnt = 0;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x000F2684 File Offset: 0x000F1684
		public static bool CreatePhotoDirectory(string strFileName)
		{
			try
			{
				wgAppConfig.m_PhotoDiriectyName = strFileName;
				wgAppConfig.m_bCreatePhotoDirectory = false;
				Thread thread = new Thread(new ThreadStart(wgAppConfig.CreatePhotoDirectoryRealize));
				thread.Name = "CreatePhotoDirectoryRealize";
				thread.Start();
				long ticks = DateTime.Now.Ticks;
				while (DateTime.Now.Ticks - ticks < 1000000L && !wgAppConfig.m_bCreatePhotoDirectory)
				{
				}
				if (thread.IsAlive)
				{
					thread.Abort();
				}
			}
			catch (Exception)
			{
			}
			return wgAppConfig.m_bCreatePhotoDirectory;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x000F2718 File Offset: 0x000F1718
		public static void CreatePhotoDirectoryRealize()
		{
			try
			{
				Directory.CreateDirectory(wgAppConfig.m_PhotoDiriectyName);
				wgAppConfig.m_bCreatePhotoDirectory = true;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000F274C File Offset: 0x000F174C
		public static void CustConfigureInit()
		{
			wgAppConfig.InsertKeyVal("autologinName", "");
			wgAppConfig.InsertKeyVal("autologinPassword", "");
			wgAppConfig.InsertKeyVal("rgtries", "1234");
			wgAppConfig.InsertKeyVal("CommPCurrent", "");
			wgAppConfig.InsertKeyVal("EMapZoomInfo", "");
			wgAppConfig.InsertKeyVal("EMapLocInfo", "");
			wgAppConfig.InsertKeyVal("NewSoftwareVersionInfo", Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf(".")));
			if (wgAppConfig.GetKeyVal("NewSoftwareVersionInfo") != Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf(".")))
			{
				wgAppConfig.UpdateKeyVal("NewSoftwareVersionInfo", Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf(".")));
			}
			wgAppConfig.InsertKeyVal("RunTimes", "0");
			try
			{
				string text = wgAppConfig.GetKeyVal("RunTimes") + "1";
				wgAppConfig.UpdateKeyVal("RunTimes", text);
				if (text.CompareTo(wgAppConfig.GetKeyVal("RunTimes")) != 0)
				{
					FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\n3k_cust.xml");
					if (fileInfo.Exists)
					{
						try
						{
							if ((fileInfo.Attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
							{
								fileInfo.Attributes &= ~FileAttributes.ReadOnly;
								wgAppConfig.UpdateKeyVal("RunTimes", text);
							}
						}
						catch (Exception)
						{
						}
					}
					if (text.CompareTo(wgAppConfig.GetKeyVal("RunTimes")) != 0)
					{
						XMessageBox.Show(CommonStr.strAccessDatabaseOnlyReadNotWrite, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						Thread.Sleep(500);
						Environment.Exit(0);
						Process.GetCurrentProcess().Kill();
					}
				}
			}
			catch
			{
			}
			wgAppConfig.InsertKeyVal("RunTimeAt", "0");
			wgAppConfig.InsertKeyVal("NewSoftwareSpecialVersionInfo", "");
			if (wgAppConfig.GetKeyVal("NewSoftwareSpecialVersionInfo") != Application.ProductVersion)
			{
				wgAppConfig.UpdateKeyVal("NewSoftwareSpecialVersionInfo", Application.ProductVersion);
			}
			try
			{
				wgUdpComm.defaultNetworkIP = null;
				wgAppConfig.UpdateKeyVal("DefaultNetworkIP", "");
				wgAppConfig.UpdateKeyVal("DefaultNetworkMac", "");
			}
			catch (Exception)
			{
			}
			try
			{
				if (int.Parse(wgAppConfig.GetKeyVal("RunTimeAt")) >= 0)
				{
					DateTime dateTime = DateTime.Parse("2010-5-1");
					if (!(DateTime.Now.Date < dateTime.Date) && int.Parse(wgAppConfig.GetKeyVal("RunTimeAt")) != 0 && !(DateTime.Now.Date >= dateTime.AddDays((double)int.Parse(wgAppConfig.GetKeyVal("RunTimeAt"))).Date) && DateTime.Now.AddDays(32.0).Date > dateTime.AddDays((double)int.Parse(wgAppConfig.GetKeyVal("RunTimeAt"))).Date)
					{
					}
				}
				else
				{
					wgAppConfig.UpdateKeyVal("RunTimeAt", "0");
				}
			}
			catch (Exception)
			{
				wgAppConfig.UpdateKeyVal("RunTimeAt", "0");
			}
			wgAppConfig.InsertKeyVal("DisplayFormat_DateYMD", "");
			wgAppConfig.InsertKeyVal("DisplayFormat_DateYMDWeek", "");
			wgAppConfig.InsertKeyVal("DisplayFormat_DateYMDHMS", "");
			wgAppConfig.InsertKeyVal("DisplayFormat_DateYMDHMSWeek", "");
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x000F2AC4 File Offset: 0x000F1AC4
		public static void custDataGridview(ref DataGridView dgv)
		{
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KeyWindows_Backcolor13")))
				{
					dgv.ColumnHeadersDefaultCellStyle.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor13", "124, 125, 156");
				}
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KeyWindows_Backcolor14")))
				{
					dgv.DefaultCellStyle.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor14", "255, 255, 255");
				}
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KeyWindows_DataSheet_TitleFontsize")))
				{
					DataGridViewCellStyle columnHeadersDefaultCellStyle = dgv.ColumnHeadersDefaultCellStyle;
					try
					{
						float num = float.Parse(wgAppConfig.GetKeyVal("KeyWindows_DataSheet_TitleFontsize"));
						columnHeadersDefaultCellStyle.Font = new Font("宋体", num, FontStyle.Regular, GraphicsUnit.Point, 134);
						if (num > 9f)
						{
							dgv.ColumnHeadersHeight = (int)((float)dgv.ColumnHeadersHeight * (num / 9f));
						}
					}
					catch (Exception)
					{
					}
				}
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KeyWindows_DataSheet_ContentFontsize")))
				{
					DataGridViewCellStyle defaultCellStyle = dgv.DefaultCellStyle;
					try
					{
						float num2 = float.Parse(wgAppConfig.GetKeyVal("KeyWindows_DataSheet_ContentFontsize"));
						defaultCellStyle.Font = new Font("宋体", num2, FontStyle.Regular, GraphicsUnit.Point, 134);
						if (num2 > 9f)
						{
							dgv.RowTemplate.Height = (int)((float)dgv.RowTemplate.Height * (num2 / 9f));
						}
					}
					catch (Exception)
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x000F2C50 File Offset: 0x000F1C50
		public static void deselectObject(DataGridView dgv)
		{
			try
			{
				int num;
				if (dgv.SelectedRows.Count <= 0)
				{
					if (dgv.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dgv.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dgv.SelectedRows[0].Index;
				}
				using (DataTable table = ((DataView)dgv.DataSource).Table)
				{
					if (dgv.SelectedRows.Count > 0)
					{
						int count = dgv.SelectedRows.Count;
						int[] array = new int[count];
						for (int i = 0; i < dgv.SelectedRows.Count; i++)
						{
							array[i] = (int)dgv.SelectedRows[i].Cells[0].Value;
						}
						for (int j = 0; j < count; j++)
						{
							int num2 = array[j];
							DataRow dataRow = table.Rows.Find(num2);
							if (dataRow != null)
							{
								dataRow["f_Selected"] = 0;
							}
						}
					}
					else
					{
						int num3 = (int)dgv.Rows[num].Cells[0].Value;
						DataRow dataRow = table.Rows.Find(num3);
						if (dataRow != null)
						{
							dataRow["f_Selected"] = 0;
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x000F2DEC File Offset: 0x000F1DEC
		public static void deselectObject(DataGridView dgv, int iSelectedCurrentNoneMax)
		{
			try
			{
				int num;
				if (dgv.SelectedRows.Count <= 0)
				{
					if (dgv.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dgv.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dgv.SelectedRows[0].Index;
				}
				using (DataTable table = ((DataView)dgv.DataSource).Table)
				{
					if (dgv.SelectedRows.Count > 0)
					{
						int count = dgv.SelectedRows.Count;
						int[] array = new int[count];
						for (int i = 0; i < dgv.SelectedRows.Count; i++)
						{
							array[i] = (int)dgv.SelectedRows[i].Cells[0].Value;
						}
						for (int j = 0; j < count; j++)
						{
							int num2 = array[j];
							DataRow dataRow = table.Rows.Find(num2);
							if (dataRow != null)
							{
								dataRow["f_Selected"] = iSelectedCurrentNoneMax;
							}
						}
					}
					else
					{
						int num3 = (int)dgv.Rows[num].Cells[0].Value;
						DataRow dataRow = table.Rows.Find(num3);
						if (dataRow != null)
						{
							dataRow["f_Selected"] = iSelectedCurrentNoneMax;
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x000F2F88 File Offset: 0x000F1F88
		public static bool DirectoryIsExisted(string strFileName)
		{
			bool flag = false;
			try
			{
				if (string.IsNullOrEmpty(strFileName))
				{
					return flag;
				}
				if (string.IsNullOrEmpty(strFileName.Trim()))
				{
					return flag;
				}
				string text = strFileName.Trim();
				if (text.Length > 2)
				{
					if (text.Substring(0, 2) == "\\\\")
					{
						return wgAppConfig.DirectoryIsExistedWithNetShare(text);
					}
					if (text.IndexOf(":") <= 0)
					{
						text = Application.StartupPath + "\\" + text;
					}
				}
				if (Directory.Exists(text))
				{
					flag = true;
				}
			}
			catch (Exception)
			{
			}
			return flag;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000F3024 File Offset: 0x000F2024
		private static void DirectoryIsExistedNetShare()
		{
			try
			{
				if (Directory.Exists(wgAppConfig.m_DirectoryNetShare))
				{
					wgAppConfig.m_bFindDirectoryNetShare = true;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x000F3058 File Offset: 0x000F2058
		public static bool DirectoryIsExistedWithNetShare(string strFileName)
		{
			bool flag = false;
			try
			{
				string text = strFileName.Trim();
				wgAppConfig.m_bFindDirectoryNetShare = false;
				wgAppConfig.m_DirectoryNetShare = text;
				try
				{
					Thread thread = new Thread(new ThreadStart(wgAppConfig.DirectoryIsExistedNetShare));
					thread.Name = "DirectoryIsExistedNetShare";
					thread.Start();
					long ticks = DateTime.Now.Ticks;
					while (DateTime.Now.Ticks - ticks < 1000000L)
					{
						if (wgAppConfig.m_bFindDirectoryNetShare)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						wgTools.WriteLine("DirectoryIsExistedNetShare  Not Found");
					}
					if (thread.IsAlive)
					{
						thread.Abort();
					}
				}
				catch (Exception)
				{
				}
			}
			catch (Exception)
			{
			}
			return flag;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000F3114 File Offset: 0x000F2114
		public static string displayPartCardNO(long cardid)
		{
			string text = cardid.ToString();
			if (wgTools.gbHideCardNO)
			{
				try
				{
					if (text.Length > 3)
					{
						return "***" + text.Substring(3);
					}
					text = "***";
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				return text;
			}
			return text;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x000F3178 File Offset: 0x000F2178
		public static string displayPartCardNO(string cardid)
		{
			string text = cardid;
			if (wgTools.gbHideCardNO)
			{
				try
				{
					if (text.Length > 3)
					{
						return "***" + text.Substring(3);
					}
					text = "***";
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				return text;
			}
			return text;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x000F31D4 File Offset: 0x000F21D4
		public static void DisposeImage(Image img)
		{
			try
			{
				if (img != null)
				{
					img.Dispose();
					img = null;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x000F3214 File Offset: 0x000F2214
		public static void DisposeImageRef(ref Image img)
		{
			try
			{
				if (img != null)
				{
					img.Dispose();
					img = null;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x000F3258 File Offset: 0x000F2258
		public static bool exportToExcel(DataGridView dgv, string formText)
		{
			string text = "";
			try
			{
				if (dgv.Rows.Count == 0)
				{
					XMessageBox.Show(CommonStr.strNoDataToExport);
					return false;
				}
				string text2;
				if (string.IsNullOrEmpty(formText))
				{
					text2 = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_ff") + ".xls";
				}
				else
				{
					text2 = formText + DateTime.Now.ToString("-yyyy-MM-dd_HHmmss_ff") + ".xls";
				}
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.FileName = text2;
					saveFileDialog.Filter = " (*.xls)|*.xls";
					if (saveFileDialog.ShowDialog() != DialogResult.OK)
					{
						return false;
					}
					text = saveFileDialog.FileName;
					int num = -1;
					if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_EXCELMODE")))
					{
						int.TryParse(wgAppConfig.GetKeyVal("KEY_EXCELMODE"), out num);
					}
					int i = 0;
					while (i < 3)
					{
						if (num >= 0)
						{
							i = num;
						}
						FileInfo fileInfo = new FileInfo(text);
						try
						{
							if (fileInfo.Exists)
							{
								fileInfo.Delete();
							}
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
						try
						{
							if (i <= 1)
							{
								using (ExcelObject excelObject = new ExcelObject(text, i == 1))
								{
									int num2 = 0;
									using (dfrmWait dfrmWait = new dfrmWait())
									{
										dfrmWait.Show();
										dfrmWait.Refresh();
										excelObject.WriteTable(dgv);
										foreach (object obj in ((IEnumerable)dgv.Rows))
										{
											DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
											try
											{
												excelObject.AddNewRow(dataGridViewRow, dgv);
												num2++;
												if (num2 >= 65535)
												{
													break;
												}
												if (num2 % 1000 == 0)
												{
													wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num2));
													Application.DoEvents();
												}
											}
											catch
											{
											}
										}
										wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num2));
										dfrmWait.Hide();
									}
									XMessageBox.Show(string.Concat(new string[]
									{
										CommonStr.strExportRecords,
										" = ",
										num2.ToString(),
										"\t",
										(num2 >= 65535) ? CommonStr.strExportRecordsMax : "",
										"\r\n\r\n",
										CommonStr.strExportToExcel,
										" ",
										text
									}));
									return true;
								}
							}
							if (i == 2)
							{
								return wgAppConfig.exportToExcel100W(dgv, formText, text);
							}
						}
						catch (Exception ex2)
						{
							wgAppConfig.wgLogWithoutDB("ExportToExcel" + text + ex2.ToString());
						}
						i++;
						if (num >= 0)
						{
							break;
						}
					}
				}
			}
			catch (Exception ex3)
			{
				wgAppConfig.wgLogWithoutDB(ex3.ToString(), EventLogEntryType.Information, null);
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
			}
			return false;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x000F362C File Offset: 0x000F262C
		public static bool exportToExcel100W(DataGridView dgv, string formText)
		{
			string text = "";
			try
			{
				if (dgv.Rows.Count == 0)
				{
					XMessageBox.Show(CommonStr.strNoDataToExport);
					return false;
				}
				string text2;
				if (string.IsNullOrEmpty(formText))
				{
					text2 = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_ff") + "_100W.xls";
				}
				else
				{
					text2 = formText + DateTime.Now.ToString("-yyyy-MM-dd_HHmmss_ff") + "_100W.xls";
				}
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.FileName = text2;
					saveFileDialog.Filter = " (*.xls)|*.xls";
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						text = saveFileDialog.FileName;
						return wgAppConfig.exportToExcel100W(dgv, formText, text);
					}
					return false;
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine("ExportToExcel" + text + ex.ToString());
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
			}
			return false;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000F3744 File Offset: 0x000F2744
		private static bool exportToExcel100W(DataGridView dgv, string formText, string newPathFile)
		{
			newPathFile.LastIndexOf(".");
			StreamWriter streamWriter = new StreamWriter(newPathFile);
			string text = "<xml version>\r\n<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n xmlns:x=\"urn:schemas-    microsoft-com:office:excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">\r\n <Styles>\r\n <Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n <Alignment ss:Vertical=\"Bottom\"/><Borders/>\r\n\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>\r\n <Protection/>\r\n </Style>\r\n <Style ss:ID=\"BoldColumn\">\r\n <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\" ss:WrapText=\"1\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"StringLiteral\"><Alignment ss:Vertical=\"Bottom\" ss:WrapText=\"1\"/>\r\n<NumberFormat/> <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"Decimal\">\r\n <NumberFormat ss:Format=\"0.00\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"Integer\">\r\n <NumberFormat ss:Format=\"0\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"DateLiteral\">\r\n <NumberFormat ss:Format=\"mm/dd/yyyy;@\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"s69\"><Alignment ss:Horizontal=\"CenterAcrossSelection\" ss:Vertical=\"Bottom\"/> <Borders/><Font ss:FontName=\"Arial\" x:Family=\"Swiss\" ss:Size=\"16\" ss:Bold=\"1\"/><Interior/><NumberFormat/><Protection/></Style>\r\n<Style ss:ID=\"s70\"><Alignment ss:Horizontal=\"CenterAcrossSelection\" ss:Vertical=\"Bottom\"/> <Borders/> <Font ss:FontName=\"Arial\" x:Family=\"Swiss\"/><Interior/><NumberFormat/><Protection/></Style>\r\n<Style ss:ID=\"s71\"><Alignment ss:Vertical=\"Bottom\"/> <Borders/><Font ss:FontName=\"Arial\" x:Family=\"Swiss\" ss:Size=\"12\" ss:Bold=\"0\"/><Interior/><NumberFormat/><Protection/></Style>\r\n</Styles>\r\n ";
			if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
			{
				text = text.Replace("\"Arial\"", "\"宋体\"");
			}
			int num = 0;
			streamWriter.Write(text);
			streamWriter.Write("<Worksheet ss:Name=\"ExcelData\">");
			streamWriter.Write("<Table>");
			int num2 = 0;
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = dgv.Columns[i];
				if (dataGridViewColumn.Visible)
				{
					num2++;
					streamWriter.Write(string.Format("<Column ss:Index=\"{0}\" ss:StyleID=\"Default\" ss:AutoFitWidth=\"0\" ss:Width=\"{1}\"/>", num2, dataGridViewColumn.Width));
				}
			}
			streamWriter.Write("<Row>");
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			string[] array = new string[dgv.Columns.Count];
			for (int j = 0; j < dgv.Columns.Count; j++)
			{
				DataGridViewColumn dataGridViewColumn2 = dgv.Columns[j];
				if (dataGridViewColumn2.Visible)
				{
					streamWriter.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
					streamWriter.Write(dataGridViewColumn2.HeaderText);
					streamWriter.Write("</Data></Cell>");
					dataGridViewColumn2.HeaderText.ToString().Replace("[", "(").Replace("]", ")")
						.Replace(".", " ")
						.Replace("\r\n", "&#10;")
						.Replace("\n", "&#10;");
					if (!flag)
					{
						stringBuilder.Append(",");
					}
					flag = false;
					if (dataGridViewColumn2.ValueType.Name.ToString().IndexOf("Int") >= 0)
					{
						array[j] = "System.Int64";
					}
					else if (dataGridViewColumn2.ValueType.Name.ToString().IndexOf("DateTime") >= 0)
					{
						array[j] = "System.String";
					}
					else if (dataGridViewColumn2.ValueType.Name.ToString().IndexOf("Decimal") >= 0)
					{
						array[j] = "System.Decimal";
						if (dataGridViewColumn2.Name.IndexOf("CardNO") > 0)
						{
							array[j] = "System.Int64";
						}
					}
					else
					{
						array[j] = dataGridViewColumn2.ValueType.Name.ToString();
					}
				}
			}
			stringBuilder.Append(")");
			streamWriter.Write("</Row>");
			int num3 = 0;
			using (dfrmWait dfrmWait = new dfrmWait())
			{
				dfrmWait.Show();
				dfrmWait.Refresh();
				foreach (object obj in ((IEnumerable)dgv.Rows))
				{
					DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
					num++;
					num3++;
					streamWriter.Write("<Row>");
					for (int k = 0; k < dgv.Columns.Count; k++)
					{
						if (dgv.Columns[k].Visible)
						{
							string text2;
							switch (text2 = array[k])
							{
							case "System.String":
							case "String":
							case "System.DateTime":
							case "DateTime":
							{
								string text3 = dataGridViewRow.Cells[k].FormattedValue.ToString().Trim().Replace("&", "&")
									.Replace(">", ">")
									.Replace("<", "<")
									.Replace("\r\n", "&#10;")
									.Replace("\n", "&#10;");
								streamWriter.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss:Type=\"String\">");
								streamWriter.Write(text3);
								streamWriter.Write("</Data></Cell>");
								goto IL_05A6;
							}
							case "System.Boolean":
								streamWriter.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss:Type=\"String\">");
								streamWriter.Write(dataGridViewRow.Cells[k].Value.ToString());
								streamWriter.Write("</Data></Cell>");
								goto IL_05A6;
							case "System.Int16":
							case "System.Int32":
							case "System.Int64":
							case "System.Byte":
							case "Byte":
								streamWriter.Write("<Cell ss:StyleID=\"Integer\"><Data ss:Type=\"Number\">");
								streamWriter.Write(dataGridViewRow.Cells[k].Value.ToString());
								streamWriter.Write("</Data></Cell>");
								goto IL_05A6;
							case "System.Decimal":
							case "System.Double":
							case "Decimal":
							case "Double":
								streamWriter.Write("<Cell ss:StyleID=\"Decimal\"><Data ss:Type=\"Number\">");
								streamWriter.Write(dataGridViewRow.Cells[k].Value.ToString());
								streamWriter.Write("</Data></Cell>");
								goto IL_05A6;
							case "System.DBNull":
							case "null":
								streamWriter.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss:Type=\"String\">");
								streamWriter.Write("");
								streamWriter.Write("</Data></Cell>");
								goto IL_05A6;
							}
							throw new Exception(array[k].ToString() + " not handled.");
						}
						IL_05A6:;
					}
					streamWriter.Write("</Row>");
					if (num3 % 1000 == 0)
					{
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num3));
						Application.DoEvents();
					}
				}
				streamWriter.Write("</Table>");
				streamWriter.Write(" </Worksheet>");
				streamWriter.Write("</Workbook>");
				streamWriter.Close();
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num3));
				dfrmWait.Hide();
			}
			XMessageBox.Show(string.Concat(new string[]
			{
				CommonStr.strExportRecords,
				" = ",
				num3.ToString(),
				"\t\r\n\r\n",
				CommonStr.strExportToExcel,
				" ",
				newPathFile
			}));
			return true;
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000F3E48 File Offset: 0x000F2E48
		public static bool exportToExcel100W_WithTitle(DataGridView dgv, string formText, string title, string titleBottom)
		{
			string text = "";
			try
			{
				if (dgv.Rows.Count == 0)
				{
					XMessageBox.Show(CommonStr.strNoDataToExport);
					return false;
				}
				string text2;
				if (string.IsNullOrEmpty(formText))
				{
					text2 = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_ff") + "_100W.xls";
				}
				else
				{
					text2 = formText + DateTime.Now.ToString("-yyyy-MM-dd_HHmmss_ff") + "_100W.xls";
				}
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.FileName = text2;
					saveFileDialog.Filter = " (*.xls)|*.xls";
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						text = saveFileDialog.FileName;
						return wgAppConfig.exportToExcel100W_WithTitle(dgv, formText, text, title, titleBottom);
					}
					return false;
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine("ExportToExcel" + text + ex.ToString());
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
			}
			return false;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000F3F60 File Offset: 0x000F2F60
		private static bool exportToExcel100W_WithTitle(DataGridView dgv, string formText, string newPathFile, string title, string titleBottom)
		{
			newPathFile.LastIndexOf(".");
			StreamWriter streamWriter = new StreamWriter(newPathFile);
			string text = "<xml version>\r\n<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n xmlns:x=\"urn:schemas-    microsoft-com:office:excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">\r\n <Styles>\r\n <Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n <Alignment ss:Vertical=\"Bottom\"/><Borders/>\r\n\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>\r\n <Protection/>\r\n </Style>\r\n <Style ss:ID=\"BoldColumn\">\r\n <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\" ss:WrapText=\"1\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"StringLiteral\"><Alignment ss:Vertical=\"Bottom\" ss:WrapText=\"1\"/>\r\n<NumberFormat/> <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"Decimal\">\r\n <NumberFormat ss:Format=\"0.00\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"Integer\">\r\n <NumberFormat ss:Format=\"0\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"DateLiteral\">\r\n <NumberFormat ss:Format=\"mm/dd/yyyy;@\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"s69\"><Alignment ss:Horizontal=\"CenterAcrossSelection\" ss:Vertical=\"Bottom\"/> <Borders/><Font ss:FontName=\"Arial\" x:Family=\"Swiss\" ss:Size=\"16\" ss:Bold=\"1\"/><Interior/><NumberFormat/><Protection/></Style>\r\n<Style ss:ID=\"s70\"><Alignment ss:Horizontal=\"CenterAcrossSelection\" ss:Vertical=\"Bottom\"/> <Borders/> <Font ss:FontName=\"Arial\" x:Family=\"Swiss\"/><Interior/><NumberFormat/><Protection/></Style>\r\n<Style ss:ID=\"s71\"><Alignment ss:Vertical=\"Bottom\"/> <Borders/><Font ss:FontName=\"Arial\" x:Family=\"Swiss\" ss:Size=\"12\" ss:Bold=\"0\"/><Interior/><NumberFormat/><Protection/></Style>\r\n</Styles>\r\n ";
			if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
			{
				text = text.Replace("\"Arial\"", "\"宋体\"");
			}
			int num = 0;
			streamWriter.Write(text);
			streamWriter.Write("<Worksheet ss:Name=\"ExcelData\">");
			streamWriter.Write("<Table>");
			int num2 = 0;
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = dgv.Columns[i];
				if (dataGridViewColumn.Visible)
				{
					num2++;
					streamWriter.Write(string.Format("<Column ss:Index=\"{0}\" ss:StyleID=\"Default\" ss:AutoFitWidth=\"0\" ss:Width=\"{1}\"/>", num2, dataGridViewColumn.Width));
				}
			}
			if (!string.IsNullOrEmpty(title))
			{
				streamWriter.Write("<Row>");
				bool flag = true;
				for (int j = 0; j < dgv.Columns.Count; j++)
				{
					DataGridViewColumn dataGridViewColumn2 = dgv.Columns[j];
					if (dataGridViewColumn2.Visible)
					{
						if (flag)
						{
							streamWriter.Write(string.Format("<Cell ss:StyleID=\"s69\"><Data ss:Type=\"String\">{0}</Data></Cell>", title));
						}
						else
						{
							streamWriter.Write("<Cell ss:StyleID=\"s70\"/>");
						}
						flag = false;
					}
				}
				streamWriter.Write("</Row>");
			}
			streamWriter.Write("<Row>");
			StringBuilder stringBuilder = new StringBuilder();
			bool flag2 = true;
			string[] array = new string[dgv.Columns.Count];
			for (int k = 0; k < dgv.Columns.Count; k++)
			{
				DataGridViewColumn dataGridViewColumn3 = dgv.Columns[k];
				if (dataGridViewColumn3.Visible)
				{
					streamWriter.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
					streamWriter.Write(dataGridViewColumn3.HeaderText);
					streamWriter.Write("</Data></Cell>");
					dataGridViewColumn3.HeaderText.ToString().Replace("[", "(").Replace("]", ")")
						.Replace(".", " ")
						.Replace("\r\n", "&#10;")
						.Replace("\n", "&#10;");
					if (!flag2)
					{
						stringBuilder.Append(",");
					}
					flag2 = false;
					if (dataGridViewColumn3.ValueType.Name.ToString().IndexOf("Int") >= 0)
					{
						array[k] = "System.Int64";
					}
					else if (dataGridViewColumn3.ValueType.Name.ToString().IndexOf("DateTime") >= 0)
					{
						array[k] = "System.String";
					}
					else if (dataGridViewColumn3.ValueType.Name.ToString().IndexOf("Decimal") >= 0)
					{
						array[k] = "System.Decimal";
						if (dataGridViewColumn3.Name.IndexOf("CardNO") > 0)
						{
							array[k] = "System.Int64";
						}
					}
					else
					{
						array[k] = dataGridViewColumn3.ValueType.Name.ToString();
					}
				}
			}
			stringBuilder.Append(")");
			streamWriter.Write("</Row>");
			int num3 = 0;
			using (dfrmWait dfrmWait = new dfrmWait())
			{
				dfrmWait.Show();
				dfrmWait.Refresh();
				foreach (object obj in ((IEnumerable)dgv.Rows))
				{
					DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
					num++;
					num3++;
					streamWriter.Write("<Row>");
					for (int l = 0; l < dgv.Columns.Count; l++)
					{
						if (dgv.Columns[l].Visible)
						{
							string text2;
							switch (text2 = array[l])
							{
							case "System.String":
							case "String":
							case "System.DateTime":
							case "DateTime":
							{
								string text3 = dataGridViewRow.Cells[l].FormattedValue.ToString().Trim().Replace("&", "&")
									.Replace(">", ">")
									.Replace("<", "<")
									.Replace("\r\n", "&#10;")
									.Replace("\n", "&#10;");
								streamWriter.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss:Type=\"String\">");
								streamWriter.Write(text3);
								streamWriter.Write("</Data></Cell>");
								goto IL_0621;
							}
							case "System.Boolean":
								streamWriter.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss:Type=\"String\">");
								streamWriter.Write(dataGridViewRow.Cells[l].Value.ToString());
								streamWriter.Write("</Data></Cell>");
								goto IL_0621;
							case "System.Int16":
							case "System.Int32":
							case "System.Int64":
							case "System.Byte":
							case "Byte":
								streamWriter.Write("<Cell ss:StyleID=\"Integer\"><Data ss:Type=\"Number\">");
								streamWriter.Write(dataGridViewRow.Cells[l].Value.ToString());
								streamWriter.Write("</Data></Cell>");
								goto IL_0621;
							case "System.Decimal":
							case "System.Double":
							case "Decimal":
							case "Double":
								streamWriter.Write("<Cell ss:StyleID=\"Decimal\"><Data ss:Type=\"Number\">");
								streamWriter.Write(dataGridViewRow.Cells[l].Value.ToString());
								streamWriter.Write("</Data></Cell>");
								goto IL_0621;
							case "System.DBNull":
							case "null":
								streamWriter.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss:Type=\"String\">");
								streamWriter.Write("");
								streamWriter.Write("</Data></Cell>");
								goto IL_0621;
							}
							throw new Exception(array[l].ToString() + " not handled.");
						}
						IL_0621:;
					}
					streamWriter.Write("</Row>");
					if (num3 % 1000 == 0)
					{
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num3));
						Application.DoEvents();
					}
				}
				if (!string.IsNullOrEmpty(titleBottom))
				{
					streamWriter.Write("<Row>");
					bool flag3 = true;
					for (int m = 0; m < dgv.Columns.Count; m++)
					{
						DataGridViewColumn dataGridViewColumn4 = dgv.Columns[m];
						if (dataGridViewColumn4.Visible)
						{
							streamWriter.Write("<Cell ss:StyleID=\"s70\"/>");
						}
					}
					streamWriter.Write("</Row>");
					streamWriter.Write("<Row>");
					for (int n = 0; n < dgv.Columns.Count; n++)
					{
						DataGridViewColumn dataGridViewColumn5 = dgv.Columns[n];
						if (dataGridViewColumn5.Visible)
						{
							if (flag3)
							{
								streamWriter.Write(string.Format("<Cell ss:StyleID=\"s71\"><Data ss:Type=\"String\">{0}</Data></Cell>", titleBottom));
							}
							else
							{
								streamWriter.Write("<Cell ss:StyleID=\"s70\"/>");
							}
							flag3 = false;
						}
					}
					streamWriter.Write("</Row>");
				}
				streamWriter.Write("</Table>");
				streamWriter.Write(" </Worksheet>");
				streamWriter.Write("</Workbook>");
				streamWriter.Close();
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num3));
				dfrmWait.Hide();
			}
			XMessageBox.Show(string.Concat(new string[]
			{
				CommonStr.strExportRecords,
				" = ",
				num3.ToString(),
				"\t\r\n\r\n",
				CommonStr.strExportToExcel,
				" ",
				newPathFile
			}));
			return true;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x000F47B0 File Offset: 0x000F37B0
		public static bool exportToExcel100W4SwipeRecord(DataGridView dgv, string formText, DataView dvFloor, int desc)
		{
			string text = "";
			try
			{
				if (dgv.Rows.Count == 0)
				{
					XMessageBox.Show(CommonStr.strNoDataToExport);
					return false;
				}
				string text2;
				if (string.IsNullOrEmpty(formText))
				{
					text2 = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_ff") + "_100W.xls";
				}
				else
				{
					text2 = formText + DateTime.Now.ToString("-yyyy-MM-dd_HHmmss_ff") + "_100W.xls";
				}
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.FileName = text2;
					saveFileDialog.Filter = " (*.xls)|*.xls";
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						text = saveFileDialog.FileName;
						return wgAppConfig.exportToExcel100W4SwipeRecord(dgv, formText, text, dvFloor, desc);
					}
					return false;
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine("ExportToExcel" + text + ex.ToString());
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
			}
			return false;
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x000F48C8 File Offset: 0x000F38C8
		private static bool exportToExcel100W4SwipeRecord(DataGridView dgv, string formText, string newPathFile, DataView dvFloor, int desc)
		{
			newPathFile.LastIndexOf(".");
			StreamWriter streamWriter = new StreamWriter(newPathFile);
			streamWriter.AutoFlush = true;
			string text = "<xml version>\r\n<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n xmlns:x=\"urn:schemas-    microsoft-com:office:excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">\r\n <Styles>\r\n <Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n <Alignment ss:Vertical=\"Bottom\"/><Borders/>\r\n\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>\r\n <Protection/>\r\n </Style>\r\n <Style ss:ID=\"BoldColumn\">\r\n <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\" ss:WrapText=\"1\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"StringLiteral\"><Alignment ss:Vertical=\"Bottom\" ss:WrapText=\"1\"/>\r\n<NumberFormat/> <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"Decimal\">\r\n <NumberFormat ss:Format=\"0.00\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"Integer\">\r\n <NumberFormat ss:Format=\"0\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"DateLiteral\">\r\n <NumberFormat ss:Format=\"mm/dd/yyyy;@\"/>\r\n  <Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>\r\n <Style ss:ID=\"s69\"><Alignment ss:Horizontal=\"CenterAcrossSelection\" ss:Vertical=\"Bottom\"/> <Borders/><Font ss:FontName=\"Arial\" x:Family=\"Swiss\" ss:Size=\"16\" ss:Bold=\"1\"/><Interior/><NumberFormat/><Protection/></Style>\r\n<Style ss:ID=\"s70\"><Alignment ss:Horizontal=\"CenterAcrossSelection\" ss:Vertical=\"Bottom\"/> <Borders/> <Font ss:FontName=\"Arial\" x:Family=\"Swiss\"/><Interior/><NumberFormat/><Protection/></Style>\r\n<Style ss:ID=\"s71\"><Alignment ss:Vertical=\"Bottom\"/> <Borders/><Font ss:FontName=\"Arial\" x:Family=\"Swiss\" ss:Size=\"12\" ss:Bold=\"0\"/><Interior/><NumberFormat/><Protection/></Style>\r\n</Styles>\r\n ";
			if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
			{
				text = text.Replace("\"Arial\"", "\"宋体\"");
			}
			int num = 0;
			streamWriter.Write(text);
			streamWriter.Write("<Worksheet ss:Name=\"ExcelData\">");
			streamWriter.Write("<Table>");
			int num2 = 0;
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = dgv.Columns[i];
				if (dataGridViewColumn.Visible)
				{
					num2++;
					streamWriter.Write(string.Format("<Column ss:Index=\"{0}\" ss:StyleID=\"Default\" ss:AutoFitWidth=\"0\" ss:Width=\"{1}\"/>", num2, dataGridViewColumn.Width));
				}
			}
			streamWriter.Write("<Row>");
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			string[] array = new string[dgv.Columns.Count];
			for (int j = 0; j < dgv.Columns.Count; j++)
			{
				DataGridViewColumn dataGridViewColumn2 = dgv.Columns[j];
				if (dataGridViewColumn2.Visible)
				{
					streamWriter.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
					streamWriter.Write(dataGridViewColumn2.HeaderText);
					streamWriter.Write("</Data></Cell>");
					dataGridViewColumn2.HeaderText.ToString().Replace("[", "(").Replace("]", ")")
						.Replace(".", " ")
						.Replace("\r\n", "&#10;")
						.Replace("\n", "&#10;");
					if (!flag)
					{
						stringBuilder.Append(",");
					}
					flag = false;
					if (dataGridViewColumn2.ValueType.Name.ToString().IndexOf("Int") >= 0)
					{
						array[j] = "System.Int64";
					}
					else if (dataGridViewColumn2.ValueType.Name.ToString().IndexOf("DateTime") >= 0)
					{
						array[j] = "System.String";
					}
					else if (dataGridViewColumn2.ValueType.Name.ToString().IndexOf("Decimal") >= 0)
					{
						array[j] = "System.Decimal";
						if (dataGridViewColumn2.Name.IndexOf("CardNO") > 0)
						{
							array[j] = "System.Int64";
						}
					}
					else
					{
						array[j] = dataGridViewColumn2.ValueType.Name.ToString();
					}
				}
			}
			stringBuilder.Append(")");
			streamWriter.Write("</Row>");
			int num3 = 0;
			using (dfrmWait dfrmWait = new dfrmWait())
			{
				dfrmWait.Show();
				dfrmWait.Refresh();
				DataTable dataTable = dgv.DataSource as DataTable;
				for (int k = 0; k < dgv.Rows.Count; k++)
				{
					num++;
					num3++;
					streamWriter.Write("<Row>");
					for (int l = 0; l < dgv.Columns.Count; l++)
					{
						if (dgv.Columns[l].Visible)
						{
							string text2 = "";
							if (dataTable.Rows[k][l] != null)
							{
								text2 = dataTable.Rows[k][l].ToString();
							}
							string text3;
							switch (text3 = array[l])
							{
							case "System.String":
							case "String":
							case "System.DateTime":
							case "DateTime":
							{
								text2 = dataTable.Rows[k][l].ToString();
								if (desc == l && text2 != null && text2 == " ")
								{
									string text4 = dataTable.Rows[k][l + 1] as string;
									MjRec mjRec = new MjRec(text4.PadLeft(48, '0'));
									text2 = mjRec.GetDetailedRecord(null, 0U);
									if (mjRec.floorNo > 0)
									{
										dvFloor.RowFilter = string.Format("f_ReaderName = '{0}' AND f_floorNO = {1} ", dataTable.Rows[k]["f_ReaderName"], mjRec.floorNo);
										if (dvFloor.Count >= 1)
										{
											text2 = text2 + " [" + dvFloor[0]["f_floorFullName"].ToString() + "]";
										}
									}
								}
								string text5 = text2;
								text5 = text5.Trim().Replace("&", "&").Replace(">", ">")
									.Replace("<", "<")
									.Replace("\r\n", "&#10;")
									.Replace("\n", "&#10;");
								streamWriter.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss:Type=\"String\">");
								streamWriter.Write(text5);
								streamWriter.Write("</Data></Cell>");
								goto IL_0676;
							}
							case "System.Boolean":
								streamWriter.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss:Type=\"String\">");
								streamWriter.Write(text2);
								streamWriter.Write("</Data></Cell>");
								goto IL_0676;
							case "System.Int16":
							case "System.Int32":
							case "System.Int64":
							case "System.Byte":
							case "Byte":
								streamWriter.Write("<Cell ss:StyleID=\"Integer\"><Data ss:Type=\"Number\">");
								streamWriter.Write(text2);
								streamWriter.Write("</Data></Cell>");
								goto IL_0676;
							case "System.Decimal":
							case "System.Double":
							case "Decimal":
							case "Double":
								streamWriter.Write("<Cell ss:StyleID=\"Decimal\"><Data ss:Type=\"Number\">");
								streamWriter.Write(text2);
								streamWriter.Write("</Data></Cell>");
								goto IL_0676;
							case "System.DBNull":
							case "null":
								streamWriter.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss:Type=\"String\">");
								streamWriter.Write("");
								streamWriter.Write("</Data></Cell>");
								goto IL_0676;
							}
							throw new Exception(array[l].ToString() + " not handled.");
						}
						IL_0676:;
					}
					streamWriter.Write("</Row>");
					if (num3 % 1000 == 0)
					{
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num3));
						dfrmWait.Text = num3.ToString();
						Application.DoEvents();
					}
				}
				streamWriter.Write("</Table>");
				streamWriter.Write(" </Worksheet>");
				streamWriter.Write("</Workbook>");
				streamWriter.Close();
				dfrmWait.Text = num3.ToString();
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num3));
				dfrmWait.Hide();
			}
			XMessageBox.Show(string.Concat(new string[]
			{
				CommonStr.strExportRecords,
				" = ",
				num3.ToString(),
				"\t\r\n\r\n",
				CommonStr.strExportToExcel,
				" ",
				newPathFile
			}));
			return true;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000F5094 File Offset: 0x000F4094
		public static bool exportToExcelSpecial(ref DataGridView dgv, string formText, bool bLoadedFinished, ref BackgroundWorker bk1, ref int startRecordIndex, int MaxRecord, string dgvSql)
		{
			DataGridView dataGridView = dgv;
			if (dataGridView.Rows.Count <= 65535 && !bLoadedFinished)
			{
				using (dfrmWait dfrmWait = new dfrmWait())
				{
					dfrmWait.Show();
					dfrmWait.Refresh();
					while (bk1.IsBusy)
					{
						Thread.Sleep(500);
						Application.DoEvents();
					}
					while (startRecordIndex <= dataGridView.Rows.Count && dataGridView.Rows.Count > 0)
					{
						startRecordIndex += MaxRecord;
						bk1.RunWorkerAsync(new object[]
						{
							startRecordIndex,
							66000 - dataGridView.Rows.Count,
							dgvSql
						});
						while (bk1.IsBusy)
						{
							Thread.Sleep(500);
							Application.DoEvents();
						}
						startRecordIndex = startRecordIndex + 66000 - dataGridView.Rows.Count - MaxRecord;
						if (dataGridView.Rows.Count > 65535)
						{
							IL_011B:
							dfrmWait.Hide();
							goto IL_012D;
						}
					}
					wgAppRunInfo.raiseAppRunInfoLoadNums(dataGridView.Rows.Count.ToString() + "#");
					goto IL_011B;
				}
			}
			IL_012D:
			wgAppConfig.exportToExcel(dataGridView, formText);
			return true;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000F51E8 File Offset: 0x000F41E8
		public static bool FileIsExisted(string strFileName)
		{
			bool flag = false;
			try
			{
				if (string.IsNullOrEmpty(strFileName))
				{
					return flag;
				}
				if (string.IsNullOrEmpty(strFileName.Trim()))
				{
					return flag;
				}
				string text = strFileName.Trim();
				if (text.Length > 2 && !(text.Substring(0, 2) == "\\\\") && text.IndexOf(":") <= 0)
				{
					text = Application.StartupPath + "\\" + text;
				}
				FileInfo fileInfo = new FileInfo(text);
				if (fileInfo.Exists)
				{
					if (fileInfo.Extension.ToUpper() == ".JPG" || fileInfo.Extension.ToUpper() == ".BMP")
					{
						if (fileInfo.Length > 1024L)
						{
							flag = true;
						}
					}
					else if (fileInfo.Extension.ToUpper() == ".MP4")
					{
						if (fileInfo.Length > 10240L)
						{
							flag = true;
						}
					}
					else if (fileInfo.Length > 0L)
					{
						flag = true;
					}
				}
			}
			catch (Exception)
			{
			}
			return flag;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x000F52FC File Offset: 0x000F42FC
		public static void fillDGVData(ref DataGridView dgv, string strSql)
		{
			if (wgAppConfig.IsAccessDB)
			{
				wgAppConfig.fillDGVData_Acc(ref dgv, strSql);
				return;
			}
			wgAppConfig.tb = new DataTable();
			wgAppConfig.dv = new DataView(wgAppConfig.tb);
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(wgAppConfig.tb);
					}
				}
			}
			dgv.AutoGenerateColumns = false;
			dgv.DataSource = wgAppConfig.dv;
			int num = 0;
			while (num < wgAppConfig.dv.Table.Columns.Count && num < dgv.ColumnCount)
			{
				dgv.Columns[num].DataPropertyName = wgAppConfig.dv.Table.Columns[num].ColumnName;
				num++;
			}
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x000F540C File Offset: 0x000F440C
		public static void fillDGVData_Acc(ref DataGridView dgv, string strSql)
		{
			wgAppConfig.tb = new DataTable();
			wgAppConfig.dv = new DataView(wgAppConfig.tb);
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
				{
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
					{
						oleDbDataAdapter.Fill(wgAppConfig.tb);
					}
				}
			}
			dgv.AutoGenerateColumns = false;
			dgv.DataSource = wgAppConfig.dv;
			int num = 0;
			while (num < wgAppConfig.dv.Table.Columns.Count && num < dgv.ColumnCount)
			{
				dgv.Columns[num].DataPropertyName = wgAppConfig.dv.Table.Columns[num].ColumnName;
				num++;
			}
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x000F5510 File Offset: 0x000F4510
		public static void findCall(dfrmFind dfrm, Form mainfrm, Control defaultControl)
		{
			try
			{
				if (dfrm == null)
				{
					dfrm = new dfrmFind();
				}
				if (mainfrm.ActiveControl == null)
				{
					mainfrm.ActiveControl = defaultControl;
				}
				else if (mainfrm.ActiveControl.GetType().Name == "Button" || mainfrm.ActiveControl is TabPage || mainfrm.ActiveControl is TabControl)
				{
					mainfrm.ActiveControl = defaultControl;
				}
				if (mainfrm.ActiveControl != null)
				{
					dfrm.setObjtoFind(mainfrm.ActiveControl, mainfrm);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000F55AC File Offset: 0x000F45AC
		public static void GetAppIcon(ref Icon appicon)
		{
			try
			{
				if (wgAppConfig.currenAppIcon == null)
				{
					wgAppConfig.currenAppIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
				}
				appicon = wgAppConfig.currenAppIcon;
			}
			catch
			{
			}
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x000F55EC File Offset: 0x000F45EC
		public static object getCustBackImage(string imageName, CultureInfo resourceCulture, ResourceManager ResourceManager)
		{
			bool flag = false;
			object obj = null;
			try
			{
				string text = Application.StartupPath + "\\" + imageName + ".png";
				if (File.Exists(text))
				{
					using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read))
					{
						byte[] array = new byte[fileStream.Length];
						fileStream.Read(array, 0, (int)fileStream.Length);
						using (MemoryStream memoryStream = new MemoryStream(array))
						{
							obj = Image.FromStream(memoryStream);
							flag = true;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			if (!flag)
			{
				obj = ResourceManager.GetObject(imageName, resourceCulture);
			}
			return obj;
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x000F56AC File Offset: 0x000F46AC
		public static Color GetKeyColor(string key, string defaultColor)
		{
			Color empty = Color.Empty;
			try
			{
				string keyVal = wgAppConfig.GetKeyVal(key);
				if (!string.IsNullOrEmpty(keyVal))
				{
					string[] array = keyVal.Split(new char[] { ',' });
					if (array.Length == 3)
					{
						return Color.FromArgb(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()));
					}
				}
			}
			catch
			{
			}
			try
			{
				if (!string.IsNullOrEmpty(defaultColor))
				{
					string[] array2 = defaultColor.Split(new char[] { ',' });
					if (array2.Length == 3)
					{
						return Color.FromArgb(int.Parse(array2[0].Trim()), int.Parse(array2[1].Trim()), int.Parse(array2[2].Trim()));
					}
				}
			}
			catch (Exception)
			{
			}
			return empty;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000F57A0 File Offset: 0x000F47A0
		public static void GetKeyIntVal(string key, ref int value)
		{
			int num = -1;
			if (int.TryParse(wgAppConfig.GetKeyVal(key), out num) && num >= 0)
			{
				value = num;
			}
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000F57C8 File Offset: 0x000F47C8
		public static string GetKeyVal(string key)
		{
			string text = "";
			try
			{
				string text2 = Application.StartupPath + "\\n3k_cust.xml";
				if (!File.Exists(text2))
				{
					wgAppConfig.CreateCustXml();
				}
				if (!File.Exists(text2))
				{
					return text;
				}
				using (DataTable dataTable = new DataTable())
				{
					dataTable.TableName = "appSettings";
					dataTable.Columns.Add("key");
					dataTable.Columns.Add("value");
					dataTable.ReadXml(text2);
					foreach (object obj in dataTable.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						if (dataRow["key"].ToString() == key)
						{
							return dataRow["value"].ToString();
						}
					}
					return text;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
			return text;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x000F58F8 File Offset: 0x000F48F8
		public static string getMoreCardShiftOneUserCondition(int consumerID)
		{
			string text = " WHERE f_ConsumerID=" + consumerID.ToString();
			if (wgAppConfig.getParamValBoolByNO(153))
			{
				string text2 = "SELECT RTRIM(LTRIM(f_ConsumerNO)) from t_b_Consumer   WHERE f_ConsumerID=" + consumerID.ToString();
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand(text2, dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand(text2, dbConnection as SqlConnection);
				}
				try
				{
					if (dbConnection.State != ConnectionState.Open)
					{
						dbConnection.Open();
					}
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					string text3 = "";
					if (dbDataReader.Read())
					{
						text3 = wgTools.SetObjToStr(dbDataReader[0]);
					}
					dbDataReader.Close();
					if (!string.IsNullOrEmpty(text3) && text3.IndexOf("-F") < 0)
					{
						text2 = "SELECT f_ConsumerID from t_b_Consumer   WHERE ";
						for (int i = 1; i <= 9; i++)
						{
							text2 = text2 + " RTRIM(LTRIM(f_ConsumerNO)) =" + wgTools.PrepareStrNUnicode(text3 + "-F" + i.ToString()) + " OR ";
						}
						text2 += " 1< 0 ";
						string text4 = "";
						dbCommand.CommandText = text2;
						dbDataReader = dbCommand.ExecuteReader();
						while (dbDataReader.Read())
						{
							if (string.IsNullOrEmpty(text4))
							{
								text4 = " WHERE ( f_ConsumerID=" + consumerID.ToString();
							}
							text4 = text4 + " OR  f_ConsumerID=" + wgTools.SetObjToStr(dbDataReader[0]);
						}
						if (!string.IsNullOrEmpty(text4))
						{
							text = text4 + " ) ";
						}
					}
					return text;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgDebugWrite(ex.ToString());
				}
				finally
				{
					dbConnection.Close();
				}
				return text;
			}
			return text;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000F5ADC File Offset: 0x000F4ADC
		public static bool getParamValBoolByNO(int NO)
		{
			string systemParamByNO = wgAppConfig.getSystemParamByNO(NO);
			int num;
			return !string.IsNullOrEmpty(systemParamByNO) && int.TryParse(systemParamByNO, out num) && num > 0;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000F5B0C File Offset: 0x000F4B0C
		public static string getPhotoFileName(long cardno)
		{
			string text = "";
			try
			{
				if (cardno == 0L)
				{
					return text;
				}
				if (wgAppConfig.arrPhotoFileFullNames.Count <= 0 || wgAppConfig.lastPhotoDirectoryName != wgAppConfig.Path4Photo())
				{
					if (!wgAppConfig.DirectoryIsExisted(wgAppConfig.Path4Photo()))
					{
						return text;
					}
					wgAppConfig.lastPhotoDirectoryName = wgAppConfig.Path4Photo();
				}
				DirectoryInfo directoryInfo = new DirectoryInfo(wgAppConfig.Path4Photo());
				if (wgAppConfig.photoDirectoryLastWriteTime != directoryInfo.LastWriteTime || wgAppConfig.arrPhotoFileFullNames.Count <= 0 || directoryInfo.GetFiles().Length != wgAppConfig.photoDirectoryLastFileCount)
				{
					wgAppConfig.arrPhotoFileFullNames.Clear();
					foreach (FileInfo fileInfo in directoryInfo.GetFiles())
					{
						wgAppConfig.arrPhotoFileFullNames.Add(fileInfo.FullName);
					}
					wgAppConfig.photoDirectoryLastWriteTime = directoryInfo.LastWriteTime;
					wgAppConfig.photoDirectoryLastFileCount = directoryInfo.GetFiles().Length;
				}
				int num = 10;
				if (wgAppConfig.IsActivateCard19)
				{
					num = 19;
				}
				for (int j = cardno.ToString().Length; j <= num; j++)
				{
					string text2 = wgAppConfig.Path4Photo() + cardno.ToString().PadLeft(j, '0') + ".jpg";
					if (wgAppConfig.arrPhotoFileFullNames.IndexOf(text2) >= 0)
					{
						return text2;
					}
					text2 = text2.ToLower(new CultureInfo("en-US", false)).Replace(".jpg", ".bmp");
					if (wgAppConfig.arrPhotoFileFullNames.IndexOf(text2) >= 0)
					{
						return text2;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.arrPhotoFileFullNames.Clear();
				wgAppConfig.photoDirectoryLastWriteTime = DateTime.Parse("2012-4-10 09:08:50.531");
				wgAppConfig.photoDirectoryLastFileCount = -1;
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return text;
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000F5CE4 File Offset: 0x000F4CE4
		public static string getPhotoFileNameByConsumerNO(string consumerNo)
		{
			string text = "";
			try
			{
				if (string.IsNullOrEmpty(consumerNo))
				{
					return text;
				}
				if (wgAppConfig.arrPhotoFileFullNames.Count <= 0 || wgAppConfig.lastPhotoDirectoryName != wgAppConfig.Path4Photo())
				{
					if (!wgAppConfig.DirectoryIsExisted(wgAppConfig.Path4Photo()))
					{
						return text;
					}
					wgAppConfig.lastPhotoDirectoryName = wgAppConfig.Path4Photo();
				}
				DirectoryInfo directoryInfo = new DirectoryInfo(wgAppConfig.Path4Photo());
				if (wgAppConfig.photoDirectoryLastWriteTime != directoryInfo.LastWriteTime || wgAppConfig.arrPhotoFileFullNames.Count <= 0 || directoryInfo.GetFiles().Length != wgAppConfig.photoDirectoryLastFileCount)
				{
					wgAppConfig.arrPhotoFileFullNames.Clear();
					foreach (FileInfo fileInfo in directoryInfo.GetFiles())
					{
						wgAppConfig.arrPhotoFileFullNames.Add(fileInfo.FullName);
					}
					wgAppConfig.photoDirectoryLastWriteTime = directoryInfo.LastWriteTime;
					wgAppConfig.photoDirectoryLastFileCount = directoryInfo.GetFiles().Length;
				}
				string text2 = wgAppConfig.Path4Photo() + consumerNo + ".jpg";
				if (wgAppConfig.arrPhotoFileFullNames.IndexOf(text2) >= 0)
				{
					return text2;
				}
				text2 = text2.ToLower(new CultureInfo("en-US", false)).Replace(".jpg", ".bmp");
				if (wgAppConfig.arrPhotoFileFullNames.IndexOf(text2) >= 0)
				{
					text = text2;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.arrPhotoFileFullNames.Clear();
				wgAppConfig.photoDirectoryLastWriteTime = DateTime.Parse("2012-4-10 09:08:50.531");
				wgAppConfig.photoDirectoryLastFileCount = -1;
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return text;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000F5E84 File Offset: 0x000F4E84
		public static string getSqlFindNormal(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
		{
			string text = "";
			try
			{
				string text2 = "";
				if (!string.IsNullOrEmpty(strTimeCon))
				{
					text2 += string.Format("AND {0}", strTimeCon);
				}
				if (findConsumerID > 0)
				{
					text2 += string.Format("AND   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
					return strBaseInfo + string.Format(" FROM (t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, text2);
				}
				if (!string.IsNullOrEmpty(findName))
				{
					text2 += string.Format(" AND t_b_Consumer.f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", findName)));
				}
				if (findCard > 0L)
				{
					text2 += string.Format(" AND t_b_Consumer.f_CardNO ={0:d} ", findCard);
				}
				if (groupMinNO > 0)
				{
					if (groupMinNO >= groupMaxNO)
					{
						return strBaseInfo + string.Format(" FROM (t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, text2, string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO));
					}
					return strBaseInfo + string.Format(" FROM (t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, text2, string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO));
				}
				else
				{
					text = strBaseInfo + string.Format(" FROM (t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, text2);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return text;
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x000F5FE8 File Offset: 0x000F4FE8
		public static string getSqlFindPrilivege(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
		{
			string text = "";
			try
			{
				string text2 = "";
				if (!string.IsNullOrEmpty(strTimeCon))
				{
					text2 += string.Format("AND {0}", strTimeCon);
				}
				if (findConsumerID > 0)
				{
					text2 += string.Format("AND   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
					return strBaseInfo + string.Format(" FROM (t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  INNER JOIN t_b_Door  ON {0}.f_DoorID=t_b_Door.f_DoorID ", fromMainDt, text2);
				}
				if (!string.IsNullOrEmpty(findName))
				{
					text2 += string.Format(" AND t_b_Consumer.f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", findName)));
				}
				if (findCard > 0L)
				{
					text2 += string.Format(" AND t_b_Consumer.f_CardNO ={0:d} ", findCard);
				}
				if (groupMinNO > 0)
				{
					if (groupMinNO >= groupMaxNO)
					{
						return strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN t_b_Door ON {0}.f_DoorID=t_b_Door.f_DoorID) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, text2, string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO));
					}
					return strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN t_b_Door ON {0}.f_DoorID=t_b_Door.f_DoorID) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, text2, string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO));
				}
				else
				{
					text = strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN t_b_Door ON {0}.f_DoorID=t_b_Door.f_DoorID ) ", fromMainDt, text2);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return text;
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x000F614C File Offset: 0x000F514C
		public static string getSqlFindSwipeRecord(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
		{
			string text = "";
			try
			{
				string text2 = "";
				string text3 = " WHERE (1>0) ";
				if (!string.IsNullOrEmpty(strTimeCon))
				{
					text3 += string.Format("AND {0}", strTimeCon);
				}
				if (findConsumerID > 0)
				{
					string text4 = string.Format("AND   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
					if (wgAppConfig.getParamValBoolByNO(153))
					{
						text = "SELECT RTRIM(LTRIM(f_ConsumerNO)) from t_b_Consumer   WHERE f_ConsumerID=" + findConsumerID.ToString();
						DbConnection dbConnection;
						DbCommand dbCommand;
						if (wgAppConfig.IsAccessDB)
						{
							dbConnection = new OleDbConnection(wgAppConfig.dbConString);
							dbCommand = new OleDbCommand(text, dbConnection as OleDbConnection);
						}
						else
						{
							dbConnection = new SqlConnection(wgAppConfig.dbConString);
							dbCommand = new SqlCommand(text, dbConnection as SqlConnection);
						}
						try
						{
							if (dbConnection.State != ConnectionState.Open)
							{
								dbConnection.Open();
							}
							DbDataReader dbDataReader = dbCommand.ExecuteReader();
							string text5 = "";
							if (dbDataReader.Read())
							{
								text5 = wgTools.SetObjToStr(dbDataReader[0]);
							}
							dbDataReader.Close();
							if (!string.IsNullOrEmpty(text5) && text5.IndexOf("-F") < 0)
							{
								text = "SELECT f_ConsumerID from t_b_Consumer   WHERE ";
								for (int i = 1; i <= 9; i++)
								{
									text = text + " RTRIM(LTRIM(f_ConsumerNO)) =" + wgTools.PrepareStrNUnicode(text5 + "-F" + i.ToString());
									text += " OR ";
								}
								text += " 1< 0 ";
								string text6 = "";
								dbCommand.CommandText = text;
								dbDataReader = dbCommand.ExecuteReader();
								while (dbDataReader.Read())
								{
									if (string.IsNullOrEmpty(text6))
									{
										text6 = " AND ( t_b_Consumer.f_ConsumerID=" + findConsumerID.ToString();
									}
									text6 = text6 + " OR  t_b_Consumer.f_ConsumerID=" + wgTools.SetObjToStr(dbDataReader[0]);
								}
								if (!string.IsNullOrEmpty(text6))
								{
									text4 = text6 + " ) ";
								}
							}
						}
						catch (Exception ex)
						{
							wgAppConfig.wgDebugWrite(ex.ToString());
						}
						finally
						{
							dbConnection.Close();
						}
					}
					text2 += text4;
					text = strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN  t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID) ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, text2);
					return text + text3;
				}
				if (!string.IsNullOrEmpty(findName))
				{
					text3 += string.Format(" AND t_b_Consumer.f_ConsumerName like {0} ", wgTools.PrepareStrNUnicode(string.Format("%{0}%", findName)));
				}
				if (findCard > 0L)
				{
					text3 += string.Format(" AND {0}.f_CardNO ={1:d} ", fromMainDt, findCard);
				}
				if (groupMinNO > 0)
				{
					if (groupMinNO >= groupMaxNO)
					{
						text = strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  LEFT JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID) )  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, text2, string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO));
					}
					else
					{
						text = strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  LEFT JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID) )  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, text2, string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO));
					}
				}
				else
				{
					text = strBaseInfo + string.Format(" FROM (({0} LEFT JOIN t_b_Consumer ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  LEFT JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID) )  LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, text2);
				}
				text += text3;
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			return text;
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x000F649C File Offset: 0x000F549C
		public static int GetSwipeRecordMaxRecIdOfDB()
		{
			int num = -2;
			string text = "";
			DbConnection dbConnection = null;
			DbCommand dbCommand;
			if (wgAppConfig.IsAccessDB)
			{
				if (dbConnection == null)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
				}
				dbCommand = new OleDbCommand(text, dbConnection as OleDbConnection);
			}
			else
			{
				if (dbConnection == null)
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
				}
				dbCommand = new SqlCommand(text, dbConnection as SqlConnection);
			}
			dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
			try
			{
				dbConnection.Open();
				if (wgAppConfig.IsAccessDB)
				{
					text = "SELECT MAX(f_RecID) FROM t_d_SwipeRecord";
				}
				else
				{
					text = "Select IDENT_CURRENT('t_d_SwipeRecord')";
				}
				dbCommand.CommandText = text;
				num = int.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar()));
				if (num == 1 && !wgAppConfig.IsAccessDB)
				{
					text = "SELECT MAX(f_RecID) FROM t_d_SwipeRecord";
					dbCommand.CommandText = text;
					num = int.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar()));
				}
				dbConnection.Close();
				dbCommand.Dispose();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString() + "\r\nGetSwipeRecords_MaxRecIdOfDB strSql=\r\n" + text);
			}
			return num;
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x000F65A8 File Offset: 0x000F55A8
		private static string getSystemParam(int parNo, string parName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.getSystemParam_Acc(parNo, parName);
			}
			try
			{
				string text;
				if (string.IsNullOrEmpty(parName))
				{
					text = "SELECT f_Value FROM t_a_SystemParam WHERE f_NO=" + parNo.ToString();
				}
				else
				{
					text = "SELECT f_Value FROM t_a_SystemParam WHERE f_Name=" + wgTools.PrepareStrNUnicode(parName);
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					if (sqlConnection.State != ConnectionState.Open)
					{
						sqlConnection.Open();
					}
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						return wgTools.SetObjToStr(sqlCommand.ExecuteScalar());
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return "";
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x000F667C File Offset: 0x000F567C
		private static string getSystemParam_Acc(int parNo, string parName)
		{
			try
			{
				string text;
				if (string.IsNullOrEmpty(parName))
				{
					text = "SELECT f_Value FROM t_a_SystemParam WHERE f_NO=" + parNo.ToString();
				}
				else
				{
					text = "SELECT f_Value FROM t_a_SystemParam WHERE f_Name=" + wgTools.PrepareStrNUnicode(parName);
				}
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						return wgTools.SetObjToStr(oleDbCommand.ExecuteScalar());
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return "";
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x000F6740 File Offset: 0x000F5740
		public static string getSystemParamByName(string parName)
		{
			return wgAppConfig.getSystemParam(-1, parName);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x000F6749 File Offset: 0x000F5749
		public static string getSystemParamByNO(int parNo)
		{
			return wgAppConfig.getSystemParam(parNo, "");
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x000F6758 File Offset: 0x000F5758
		public static string getSystemParamNotes(int NO)
		{
			string text;
			string text2;
			string text3;
			string text4;
			wgAppConfig.getSystemParamValue(NO, out text, out text2, out text3, out text4);
			return wgTools.SetObjToStr(text3);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x000F677C File Offset: 0x000F577C
		public static int getSystemParamValue(int NO, out string EName, out string value, out string notes)
		{
			string text;
			return wgAppConfig.getSystemParamValue(NO, out EName, out value, out notes, out text);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x000F6794 File Offset: 0x000F5794
		public static int getSystemParamValue(int NO, out string EName, out string value, out string notes, out string name)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.getSystemParamValue_Acc(NO, out EName, out value, out notes, out name);
			}
			int num = -9;
			EName = null;
			value = null;
			notes = null;
			name = null;
			string text = "SELECT * FROM t_a_SystemParam WHERE f_NO = " + NO.ToString();
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						name = sqlDataReader["f_Name"] as string;
						EName = sqlDataReader["f_EName"] as string;
						value = sqlDataReader["f_Value"] as string;
						notes = sqlDataReader["f_Notes"] as string;
						num = 1;
					}
					sqlDataReader.Close();
				}
			}
			return num;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x000F688C File Offset: 0x000F588C
		public static int getSystemParamValue_Acc(int NO, out string EName, out string value, out string notes, out string name)
		{
			int num = -9;
			name = null;
			EName = null;
			value = null;
			notes = null;
			string text = "SELECT * FROM t_a_SystemParam WHERE f_NO = " + NO.ToString();
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					if (oleDbDataReader.Read())
					{
						name = oleDbDataReader["f_Name"] as string;
						EName = oleDbDataReader["f_EName"] as string;
						value = oleDbDataReader["f_Value"] as string;
						notes = oleDbDataReader["f_Notes"] as string;
						num = 1;
					}
					oleDbDataReader.Close();
				}
			}
			return num;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x000F6970 File Offset: 0x000F5970
		public static string getTriggerName()
		{
			string text = "";
			if (!wgAppConfig.IsAccessDB)
			{
				try
				{
					string text2 = "select * from sysobjects where xtype='TR'";
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection))
						{
							sqlConnection.Open();
							SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
							while (sqlDataReader.Read())
							{
								if (!string.IsNullOrEmpty(text))
								{
									text += ",";
								}
								text += sqlDataReader["name"];
							}
						}
					}
					if (!string.IsNullOrEmpty(text))
					{
						text = CommonStr.strTrigger2015 + text + ".";
					}
				}
				catch (Exception)
				{
				}
			}
			return text;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x000F6A48 File Offset: 0x000F5A48
		private static string getUpdateStr4Department(string dt, string newNameHead)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return string.Format("UPDATE  t_b_group c INNER JOIN ({0} a INNER JOIN t_b_group b on  (a.f_GroupID= b.f_GroupID))  ON (c.f_GroupName = ({1} + b.f_GroupName)) Set a.f_GroupID = c.f_GroupID", dt, wgTools.PrepareStrNUnicode(newNameHead));
			}
			return string.Format("UPDATE {0} Set {0}.f_GroupID = c.f_GroupID from t_b_group c INNER JOIN ({0} a INNER JOIN t_b_group b on  (a.f_GroupID= b.f_GroupID))  ON (c.f_GroupName = ({1} + b.f_GroupName))         ", dt, wgTools.PrepareStrNUnicode(newNameHead));
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x000F6A74 File Offset: 0x000F5A74
		private static string getUpdateStr4Zone(string dt, string newNameHead)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return string.Format("UPDATE  t_b_Controller_Zone c INNER JOIN ({0} a INNER JOIN t_b_Controller_Zone b on  (a.f_ZoneID= b.f_ZoneID))  ON (c.f_ZoneName = ({1} + b.f_ZoneName)) Set a.f_ZoneID = c.f_ZoneID", dt, wgTools.PrepareStrNUnicode(newNameHead));
			}
			return string.Format("UPDATE {0} Set {0}.f_ZoneID = c.f_ZoneID from t_b_Controller_Zone c INNER JOIN ({0} a INNER JOIN t_b_Controller_Zone b on  (a.f_ZoneID= b.f_ZoneID))  ON (c.f_ZoneName = ({1} + b.f_ZoneName))         ", dt, wgTools.PrepareStrNUnicode(newNameHead));
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x000F6AA0 File Offset: 0x000F5AA0
		public static int getValBySql(string strSql)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.getValBySql_Acc(strSql);
			}
			int num = 0;
			if (!string.IsNullOrEmpty(strSql))
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					if (sqlConnection.State != ConnectionState.Open)
					{
						sqlConnection.Open();
					}
					using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
					{
						int.TryParse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar()).ToString(), out num);
					}
				}
			}
			return num;
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x000F6B40 File Offset: 0x000F5B40
		public static int getValBySql_Acc(string strSql)
		{
			int num = 0;
			if (!string.IsNullOrEmpty(strSql))
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
					{
						int.TryParse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()).ToString(), out num);
					}
				}
			}
			return num;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000F6BD0 File Offset: 0x000F5BD0
		public static string getValStringBySql(string strSql)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.getValStringBySql_Acc(strSql);
			}
			string text = "";
			if (!string.IsNullOrEmpty(strSql))
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					if (sqlConnection.State != ConnectionState.Open)
					{
						sqlConnection.Open();
					}
					using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
					{
						text = wgTools.SetObjToStr(sqlCommand.ExecuteScalar()).ToString();
					}
				}
			}
			return text;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x000F6C64 File Offset: 0x000F5C64
		public static string getValStringBySql_Acc(string strSql)
		{
			string text = "";
			if (!string.IsNullOrEmpty(strSql))
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
					{
						text = wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()).ToString();
					}
				}
			}
			return text;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x000F6CE8 File Offset: 0x000F5CE8
		public static void HideCardNOColumn(DataGridViewColumn vc)
		{
			if (wgTools.gbHideCardNO)
			{
				try
				{
					vc.Visible = false;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x000F6D24 File Offset: 0x000F5D24
		public static void InsertKeyVal(string key, string value)
		{
			bool flag = false;
			try
			{
				string text = Application.StartupPath + "\\n3k_cust.xml";
				if (File.Exists(text))
				{
					using (DataTable dataTable = new DataTable())
					{
						dataTable.TableName = "appSettings";
						dataTable.Columns.Add("key");
						dataTable.Columns.Add("value");
						dataTable.ReadXml(text);
						foreach (object obj in dataTable.Rows)
						{
							DataRow dataRow = (DataRow)obj;
							if (dataRow["key"].ToString() == key)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							DataRow dataRow2 = dataTable.NewRow();
							dataRow2["key"] = key;
							dataRow2["value"] = value;
							dataTable.Rows.Add(dataRow2);
							dataTable.AcceptChanges();
							using (StringWriter stringWriter = new StringWriter())
							{
								using (StreamWriter streamWriter = new StreamWriter(text, false))
								{
									dataTable.WriteXml(stringWriter, XmlWriteMode.WriteSchema, true);
									streamWriter.Write(stringWriter.ToString());
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x000F6EFC File Offset: 0x000F5EFC
		public static bool IsAllowAccess(string funtionName)
		{
			bool flag = false;
			try
			{
				if (funtionName.ToUpper().CompareTo("MoreCardAllGroups".ToUpper()) != 0 || (wgAppConfig.ProductTypeOfApp.CompareTo("CGACCESS") != 0 && DateTime.Now <= DateTime.Parse("2014-12-02 12:19:15").AddMonths(6)))
				{
					return flag;
				}
				flag = true;
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x000F6F70 File Offset: 0x000F5F70
		public static bool IsChineseSet(string cultureInfo)
		{
			bool flag = false;
			try
			{
				if (string.IsNullOrEmpty(cultureInfo) || (!(cultureInfo == "zh") && cultureInfo.IndexOf("zh-") != 0))
				{
					return flag;
				}
				flag = true;
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x000F6FC0 File Offset: 0x000F5FC0
		public static void optimizeDepartment()
		{
			try
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				int num = -1;
				int num2 = -1;
				int num3 = 0;
				int num4 = 0;
				string text = "SELECT f_GroupID, f_GroupName,f_GroupNO FROM t_b_Group ORDER BY f_GroupName  + '\\' ASC";
				dbConnection.Open();
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					if (num2 <= (int)dbDataReader["f_GroupNO"])
					{
						num2 = (int)dbDataReader["f_GroupNO"];
					}
					else
					{
						num3 = 1;
					}
					if (num <= (int)dbDataReader["f_GroupID"])
					{
						num = (int)dbDataReader["f_GroupID"];
					}
					else
					{
						num3 = 1;
					}
					if (num4 < (int)dbDataReader["f_GroupID"])
					{
						num4 = (int)dbDataReader["f_GroupID"];
					}
				}
				dbDataReader.Close();
				if (num3 == 0)
				{
					dbConnection.Close();
				}
				else
				{
					wgAppConfig.wgLog("optimizeDepartment starting...");
					dbDataReader = dbCommand.ExecuteReader();
					string text2 = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_OPTIMUM";
					num2 = 1;
					while (dbDataReader.Read())
					{
						string text3 = text2 + dbDataReader["f_GroupName"];
						wgAppConfig.runUpdateSql(string.Format("INSERT INTO t_b_Group (f_GroupName,f_GroupNO) values ({0},{1})", wgTools.PrepareStrNUnicode(text3), num2));
						num2++;
					}
					dbDataReader.Close();
					wgAppConfig.runUpdateSql(wgAppConfig.getUpdateStr4Department("t_b_group4PCCheckAccess", text2));
					wgAppConfig.runUpdateSql(wgAppConfig.getUpdateStr4Department("t_b_Group4Operator", text2));
					wgAppConfig.runUpdateSql(wgAppConfig.getUpdateStr4Department("t_b_Group4Meal", text2));
					wgAppConfig.runUpdateSql(wgAppConfig.getUpdateStr4Department("t_b_Consumer_Delete", text2));
					wgAppConfig.runUpdateSql(wgAppConfig.getUpdateStr4Department("t_b_Consumer", text2));
					wgAppConfig.runUpdateSql(string.Format("DELETE FROM t_b_Group Where f_GroupID <= {0} ", num4));
					string text4;
					if (wgAppConfig.IsAccessDB)
					{
						text4 = string.Format("update t_b_Group set f_GroupName = mid(f_groupname,{0},255)", text2.Length + 1);
					}
					else
					{
						text4 = string.Format("update t_b_Group set f_GroupName = SUBSTRING(f_groupname,{0},2047)", text2.Length + 1);
					}
					wgAppConfig.runUpdateSql(text4);
					dbConnection.Close();
					dbCommand.Dispose();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x000F724C File Offset: 0x000F624C
		public static void optimizeZone()
		{
			try
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				int num = -1;
				int num2 = -1;
				int num3 = 0;
				int num4 = 0;
				string text = "SELECT f_ZoneID, f_ZoneName,f_ZoneNO FROM t_b_Controller_Zone ORDER BY f_ZoneName  + '\\' ASC";
				dbConnection.Open();
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					if (num2 <= (int)dbDataReader["f_ZoneNO"])
					{
						num2 = (int)dbDataReader["f_ZoneNO"];
					}
					else
					{
						num3 = 1;
					}
					if (num <= (int)dbDataReader["f_ZoneID"])
					{
						num = (int)dbDataReader["f_ZoneID"];
					}
					else
					{
						num3 = 1;
					}
					if (num4 < (int)dbDataReader["f_ZoneID"])
					{
						num4 = (int)dbDataReader["f_ZoneID"];
					}
				}
				dbDataReader.Close();
				if (num3 == 0)
				{
					dbConnection.Close();
				}
				else
				{
					wgAppConfig.wgLog("optimizeZone starting...");
					dbDataReader = dbCommand.ExecuteReader();
					string text2 = DateTime.Now.ToString("ffff") + "_OPTIMUM";
					num2 = 1;
					while (dbDataReader.Read())
					{
						string text3 = text2 + dbDataReader["f_ZoneName"];
						wgAppConfig.runUpdateSql(string.Format("INSERT INTO t_b_Controller_Zone (f_ZoneName,f_ZoneNO) values ({0},{1})", wgTools.PrepareStrNUnicode(text3), num2));
						num2++;
					}
					dbDataReader.Close();
					wgAppConfig.runUpdateSql(wgAppConfig.getUpdateStr4Zone("t_b_Controller_Zone4Operator", text2));
					wgAppConfig.runUpdateSql(wgAppConfig.getUpdateStr4Zone("t_b_Controller", text2));
					wgAppConfig.runUpdateSql(string.Format("DELETE FROM t_b_Controller_Zone Where f_ZoneID <= {0} ", num4));
					string text4;
					if (wgAppConfig.IsAccessDB)
					{
						text4 = string.Format("update t_b_Controller_Zone set f_ZoneName = mid(f_Zonename,{0},255)", text2.Length + 1);
					}
					else
					{
						text4 = string.Format("update t_b_Controller_Zone set f_ZoneName = SUBSTRING(f_Zonename,{0},2047)", text2.Length + 1);
					}
					wgAppConfig.runUpdateSql(text4);
					dbConnection.Close();
					dbCommand.Dispose();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x000F74A0 File Offset: 0x000F64A0
		public static void outPutBackgroundFile()
		{
			string[] array = new string[]
			{
				"pChild_title", "pConsole_Door_NormalOpen", "pLogin_bk", "pMain_Bookmark_bkg", "pMain_Bookmark_focus", "pMain_Bookmark_normal", "pMain_bottom", "pMain_button_normal", "pMain_icon_bkg", "pMain_icon_focus02",
				"pTools_second_title", "pTools_third_title"
			};
			try
			{
				string[] array2 = wgAppConfig.GetKeyVal("KEYS_CustomBackgroud").Split(new char[] { ';' });
				for (int i = 0; i < array2.Length; i++)
				{
					string[] array3 = array2[i].Split(new char[] { ',' });
					if (array3.Length == 2)
					{
						string text = array3[0].Replace("\r\n", "").Trim();
						for (int j = 0; j < array.Length; j++)
						{
							if (array[j] == text)
							{
								FileStream fileStream = new FileStream(Application.StartupPath + "\\" + text + ".png", FileMode.Create);
								BinaryWriter binaryWriter = new BinaryWriter(fileStream);
								array3[1] = array3[1].Replace("\r\n", "");
								array3[1] = array3[1].Trim();
								byte[] array4 = new byte[array3[1].Length / 2];
								wgAppConfig.strToByteArr(array3[1], ref array4, array4.Length);
								for (int k = 0; k < array4.Length; k++)
								{
									binaryWriter.Write(array4[k]);
								}
								binaryWriter.Close();
								break;
							}
						}
					}
				}
				wgAppConfig.UpdateKeyVal("KEYS_CustomBackgroud", "");
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x000F7694 File Offset: 0x000F6694
		public static string Path4AviJpg()
		{
			return wgAppConfig.m_path4AviJpg;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x000F769B File Offset: 0x000F669B
		public static string Path4AviJpgDefault()
		{
			return Application.StartupPath + "\\AVI_JPG\\";
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000F76AC File Offset: 0x000F66AC
		public static string Path4AviJpgOnlyView()
		{
			return wgAppConfig.m_path4AviJpgOnlyView;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x000F76B4 File Offset: 0x000F66B4
		public static void Path4AviJpgOnlyViewRefresh()
		{
			string text = Application.StartupPath + "\\AVI_JPG\\";
			string systemParamNotes = wgAppConfig.getSystemParamNotes(43);
			if (!string.IsNullOrEmpty(systemParamNotes))
			{
				text = systemParamNotes;
				if (text.Substring(text.Length - 1, 1) != "\\")
				{
					text += "\\";
				}
			}
			wgAppConfig.m_path4AviJpgOnlyView = text;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000F7710 File Offset: 0x000F6710
		public static void Path4AviJpgRefresh()
		{
			string text = Application.StartupPath + "\\AVI_JPG\\";
			string systemParamNotes = wgAppConfig.getSystemParamNotes(42);
			if (!string.IsNullOrEmpty(systemParamNotes))
			{
				text = systemParamNotes;
				if (text.Substring(text.Length - 1, 1) != "\\")
				{
					text += "\\";
				}
			}
			wgAppConfig.m_path4AviJpg = text;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x000F776C File Offset: 0x000F676C
		public static string Path4Doc()
		{
			string text = ".\\DOC\\";
			text = Application.StartupPath + "\\DOC\\";
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x000F77BC File Offset: 0x000F67BC
		public static string Path4Photo()
		{
			return wgAppConfig.m_path4Photo;
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x000F77C3 File Offset: 0x000F67C3
		public static string Path4PhotoDefault()
		{
			return Application.StartupPath + "\\PHOTO\\";
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x000F77D4 File Offset: 0x000F67D4
		public static void Path4PhotoRefresh()
		{
			string text = Application.StartupPath + "\\PHOTO\\";
			string systemParamNotes = wgAppConfig.getSystemParamNotes(41);
			if (!string.IsNullOrEmpty(systemParamNotes))
			{
				text = systemParamNotes;
				if (text.Substring(text.Length - 1, 1) != "\\")
				{
					text += "\\";
				}
			}
			wgAppConfig.m_path4Photo = text;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x000F7830 File Offset: 0x000F6830
		public static void printdgv(DataGridView dv, string Title)
		{
			if (dv.Rows.Count == 0)
			{
				XMessageBox.Show(CommonStr.strNoDataToPrint);
				return;
			}
			using (DGVPrinter dgvprinter = new DGVPrinter())
			{
				if (!string.IsNullOrEmpty(Title))
				{
					dgvprinter.Title = Title;
				}
				dgvprinter.PageNumbers = true;
				dgvprinter.PageNumberInHeader = false;
				dgvprinter.PorportionalColumns = true;
				dgvprinter.HeaderCellAlignment = StringAlignment.Near;
				dgvprinter.PrintDataGridView(dv);
			}
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x000F78AC File Offset: 0x000F68AC
		public static void printdgv(DataGridView dv, string Title, string footer)
		{
			using (DGVPrinter dgvprinter = new DGVPrinter())
			{
				if (!string.IsNullOrEmpty(Title))
				{
					dgvprinter.Title = Title;
				}
				if (!string.IsNullOrEmpty(footer))
				{
					dgvprinter.Footer = footer;
				}
				dgvprinter.PageNumbers = true;
				dgvprinter.PageNumberInHeader = false;
				dgvprinter.PorportionalColumns = true;
				dgvprinter.HeaderCellAlignment = StringAlignment.Near;
				dgvprinter.PrintDataGridView(dv);
			}
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x000F791C File Offset: 0x000F691C
		public static int printdgvWithoutPrintDialog(DataGridView dv, string Title)
		{
			if (dv.Rows.Count == 0)
			{
				XMessageBox.Show(CommonStr.strNoDataToPrint);
				return 0;
			}
			int num;
			using (DGVPrinter dgvprinter = new DGVPrinter())
			{
				if (!string.IsNullOrEmpty(Title))
				{
					dgvprinter.Title = Title;
				}
				dgvprinter.PageNumbers = true;
				dgvprinter.PageNumberInHeader = false;
				dgvprinter.PorportionalColumns = true;
				dgvprinter.HeaderCellAlignment = StringAlignment.Near;
				num = dgvprinter.PrintDataGridViewWithoutPrintDialog(dv);
			}
			return num;
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x000F799C File Offset: 0x000F699C
		public static void ReadGVStyle(Form form, DataGridView dgv)
		{
			try
			{
				if (form != null && dgv != null)
				{
					string text = Application.StartupPath + "\\PHOTO\\";
					string text2 = string.Concat(new string[] { text, form.Name, "_", dgv.Name, ".xml" });
					if (wgAppConfig.CultureInfoStr == "")
					{
						text2 = string.Format("{0}{1}_{2}.xml", text, form.Name, dgv.Name);
					}
					else
					{
						text2 = string.Format("{0}{1}_{2}.{3}.xml", new object[]
						{
							text,
							form.Name,
							dgv.Name,
							wgAppConfig.CultureInfoStr
						});
					}
					if (File.Exists(text2))
					{
						using (DataTable dataTable = new DataTable())
						{
							dataTable.TableName = dgv.Name;
							dataTable.Columns.Add("colName");
							dataTable.Columns.Add("colHeader");
							dataTable.Columns.Add("colWidth");
							dataTable.Columns.Add("colVisable");
							dataTable.Columns.Add("colDisplayIndex");
							dataTable.ReadXml(text2);
							foreach (object obj in dataTable.Rows)
							{
								DataRow dataRow = (DataRow)obj;
								dgv.Columns[dataRow["colName"].ToString()].HeaderText = dataRow["colHeader"].ToString();
								dgv.Columns[dataRow["colName"].ToString()].Width = int.Parse(dataRow["colWidth"].ToString());
								dgv.Columns[dataRow["colName"].ToString()].Visible = bool.Parse(dataRow["colVisable"].ToString());
								dgv.Columns[dataRow["colName"].ToString()].DisplayIndex = int.Parse(dataRow["colDisplayIndex"].ToString());
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.writeLine(ex.ToString());
			}
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x000F7C58 File Offset: 0x000F6C58
		public static bool ReadGVStyleFileExisted(Form form, DataGridView dgv)
		{
			try
			{
				if (form == null || dgv == null)
				{
					return false;
				}
				string text = Application.StartupPath + "\\PHOTO\\";
				string text2 = string.Concat(new string[] { text, form.Name, "_", dgv.Name, ".xml" });
				if (wgAppConfig.CultureInfoStr == "")
				{
					text2 = string.Format("{0}{1}_{2}.xml", text, form.Name, dgv.Name);
				}
				else
				{
					text2 = string.Format("{0}{1}_{2}.{3}.xml", new object[]
					{
						text,
						form.Name,
						dgv.Name,
						wgAppConfig.CultureInfoStr
					});
				}
				if (!File.Exists(text2))
				{
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				wgAppConfig.writeLine(ex.ToString());
			}
			return false;
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x000F7D50 File Offset: 0x000F6D50
		public static string ReplaceFloorRoom(string info)
		{
			string text = info;
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_strReplaceDepartment")))
				{
					return info.Replace(CommonStr.strReplaceDepartment, wgAppConfig.GetKeyVal("KEY_strReplaceDepartment"));
				}
				if (wgAppConfig.bFloorRoomManager)
				{
					text = info.Replace(CommonStr.strReplaceDepartment, CommonStr.strReplaceFloorRoom);
					if (text == CommonStr.strReplaceDepartment2)
					{
						text = text.Replace(CommonStr.strReplaceDepartment2, CommonStr.strReplaceFloorRoom);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
			return text;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x000F7DE4 File Offset: 0x000F6DE4
		public static string ReplaceMeeting(string info)
		{
			string text = info;
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_strMeetingSign")))
				{
					text = info.Replace(CommonStr.strMeeting, wgAppConfig.GetKeyVal("KEY_strMeetingSign"));
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
			return text;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x000F7E3C File Offset: 0x000F6E3C
		public static string ReplaceRemoteOpenDoor(string info)
		{
			string text = info;
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_strRemoteOpenDoor")))
				{
					text = info.Replace(CommonStr.strRecordTypeRemoteOpen, wgAppConfig.GetKeyVal("KEY_strRemoteOpenDoor"));
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
			return text;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x000F7E94 File Offset: 0x000F6E94
		public static string ReplaceSpecialWord(string info, string key)
		{
			string text = info;
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal(key)))
				{
					text = wgAppConfig.GetKeyVal(key);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
			return text;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x000F7ED8 File Offset: 0x000F6ED8
		public static string ReplaceWorkNO(string info)
		{
			string text = info;
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_strReplaceWorkNO")))
				{
					return info.Replace(CommonStr.strReplaceWorkNO, wgAppConfig.GetKeyVal("KEY_strReplaceWorkNO"));
				}
				if (wgAppConfig.bFloorRoomManager)
				{
					text = info.Replace(CommonStr.strReplaceWorkNO, CommonStr.strReplaceNO);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
			return text;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x000F7F4C File Offset: 0x000F6F4C
		public static void RestoreGVStyle(Form form, DataGridView dgv)
		{
			try
			{
				string text = Application.StartupPath + "\\PHOTO\\";
				string text2 = string.Concat(new string[] { text, form.Name, "_", dgv.Name, ".xml" });
				if (wgAppConfig.CultureInfoStr == "")
				{
					text2 = string.Format("{0}{1}_{2}.xml", text, form.Name, dgv.Name);
				}
				else
				{
					text2 = string.Format("{0}{1}_{2}.{3}.xml", new object[]
					{
						text,
						form.Name,
						dgv.Name,
						wgAppConfig.CultureInfoStr
					});
				}
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x000F8030 File Offset: 0x000F7030
		public static int runSql(string strSql)
		{
			int num = 0;
			if (!string.IsNullOrEmpty(strSql))
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand(strSql, dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand(strSql, dbConnection as SqlConnection);
				}
				try
				{
					if (dbConnection.State != ConnectionState.Open)
					{
						dbConnection.Open();
					}
					dbCommand.ExecuteNonQuery();
					num = 1;
					dbConnection.Close();
					dbCommand.Dispose();
				}
				catch
				{
				}
				finally
				{
					if (dbConnection.State == ConnectionState.Open)
					{
						dbConnection.Close();
					}
				}
			}
			return num;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x000F80D8 File Offset: 0x000F70D8
		public static int runUpdateSql(string strSql)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.runUpdateSql_Acc(strSql);
			}
			int num = -1;
			if (!string.IsNullOrEmpty(strSql))
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					if (sqlConnection.State != ConnectionState.Open)
					{
						sqlConnection.Open();
					}
					using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
					{
						num = sqlCommand.ExecuteNonQuery();
					}
				}
			}
			return num;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000F815C File Offset: 0x000F715C
		public static int runUpdateSql_Acc(string strSql)
		{
			int num = -1;
			if (!string.IsNullOrEmpty(strSql))
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
					{
						num = oleDbCommand.ExecuteNonQuery();
					}
				}
			}
			return num;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000F81D4 File Offset: 0x000F71D4
		public static void SaveDGVStyle(Form form, DataGridView dgv)
		{
			try
			{
				string text = Application.StartupPath + "\\PHOTO\\";
				string text2;
				if (wgAppConfig.CultureInfoStr == "")
				{
					text2 = string.Format("{0}{1}_{2}.xml", text, form.Name, dgv.Name);
				}
				else
				{
					text2 = string.Format("{0}{1}_{2}.{3}.xml", new object[]
					{
						text,
						form.Name,
						dgv.Name,
						wgAppConfig.CultureInfoStr
					});
				}
				DataSet dataSet = new DataSet("DGV_STILE");
				DataTable dataTable = new DataTable();
				dataSet.Tables.Add(dataTable);
				dataTable.TableName = dgv.Name;
				dataTable.Columns.Add("colName");
				dataTable.Columns.Add("colHeader");
				dataTable.Columns.Add("colWidth");
				dataTable.Columns.Add("colVisable");
				dataTable.Columns.Add("colDisplayIndex");
				foreach (object obj in dgv.Columns)
				{
					DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj;
					DataRow dataRow = dataTable.NewRow();
					dataRow["colName"] = dataGridViewColumn.Name;
					dataRow["colHeader"] = dataGridViewColumn.HeaderText;
					dataRow["colWidth"] = dataGridViewColumn.Width;
					dataRow["colVisable"] = dataGridViewColumn.Visible;
					dataRow["colDisplayIndex"] = dataGridViewColumn.DisplayIndex;
					dataTable.Rows.Add(dataRow);
					dataTable.AcceptChanges();
				}
				StringWriter stringWriter = new StringWriter();
				stringWriter = new StringWriter();
				dataTable.WriteXml(stringWriter, XmlWriteMode.WriteSchema, true);
				using (StreamWriter streamWriter = new StreamWriter(text2, false))
				{
					streamWriter.Write(stringWriter.ToString());
				}
				stringWriter.Dispose();
				dataSet.Dispose();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x000F8448 File Offset: 0x000F7448
		public static void SaveNewXmlFile(string key, string value)
		{
			string startupPath = Application.StartupPath;
			string text = startupPath + "\\n3k_cust.xmlAA";
			string text2 = startupPath + "\\photo\\n3k_cust.xmlAA";
			string text3 = wgAppConfig.defaultCustConfigzhCHS;
			if (!wgAppConfig.IsChineseSet(Thread.CurrentThread.CurrentUICulture.Name))
			{
				text3 = text3.Replace("zh-CHS", "en");
			}
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
			if (File.Exists(text))
			{
				wgAppConfig.UpdateKeyVal(key, value, text);
			}
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000F8524 File Offset: 0x000F7524
		public static void selectObject(DataGridView dgv)
		{
			wgAppConfig.selectObject(dgv, "", "");
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x000F8536 File Offset: 0x000F7536
		public static void selectObject(DataGridView dgv, int iSelectedCurrentNoneMax)
		{
			wgAppConfig.selectObject(dgv, "", "", iSelectedCurrentNoneMax);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x000F854C File Offset: 0x000F754C
		public static void selectObject(DataGridView dgv, string secondField, string val)
		{
			try
			{
				int num;
				if (dgv.SelectedRows.Count <= 0)
				{
					if (dgv.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dgv.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dgv.SelectedRows[0].Index;
				}
				using (DataTable table = ((DataView)dgv.DataSource).Table)
				{
					if (dgv.SelectedRows.Count > 0)
					{
						int count = dgv.SelectedRows.Count;
						int[] array = new int[count];
						for (int i = 0; i < dgv.SelectedRows.Count; i++)
						{
							array[i] = (int)dgv.SelectedRows[i].Cells[0].Value;
						}
						for (int j = 0; j < count; j++)
						{
							int num2 = array[j];
							DataRow dataRow = table.Rows.Find(num2);
							if (dataRow != null)
							{
								dataRow["f_Selected"] = 1;
								if (secondField != "")
								{
									dataRow[secondField] = val;
								}
							}
						}
					}
					else
					{
						int num3 = (int)dgv.Rows[num].Cells[0].Value;
						DataRow dataRow = table.Rows.Find(num3);
						if (dataRow != null)
						{
							dataRow["f_Selected"] = 1;
							if (secondField != "")
							{
								dataRow[secondField] = val;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000F8710 File Offset: 0x000F7710
		public static void selectObject(DataGridView dgv, string secondField, string val, int iSelectedCurrentNoneMax)
		{
			try
			{
				int num;
				if (dgv.SelectedRows.Count <= 0)
				{
					if (dgv.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dgv.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dgv.SelectedRows[0].Index;
				}
				using (DataTable table = ((DataView)dgv.DataSource).Table)
				{
					if (dgv.SelectedRows.Count > 0)
					{
						int count = dgv.SelectedRows.Count;
						int[] array = new int[count];
						for (int i = 0; i < dgv.SelectedRows.Count; i++)
						{
							array[i] = (int)dgv.SelectedRows[i].Cells[0].Value;
						}
						for (int j = 0; j < count; j++)
						{
							int num2 = array[j];
							DataRow dataRow = table.Rows.Find(num2);
							if (dataRow != null)
							{
								dataRow["f_Selected"] = iSelectedCurrentNoneMax + 1;
								if (secondField != "")
								{
									dataRow[secondField] = val;
								}
							}
						}
					}
					else
					{
						int num3 = (int)dgv.Rows[num].Cells[0].Value;
						DataRow dataRow = table.Rows.Find(num3);
						if (dataRow != null)
						{
							dataRow["f_Selected"] = iSelectedCurrentNoneMax + 1;
							if (secondField != "")
							{
								dataRow[secondField] = val;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000F88D8 File Offset: 0x000F78D8
		public static void setDisplayFormatDate(DateTimePicker dtp, string displayformat)
		{
			try
			{
				if (string.IsNullOrEmpty(displayformat))
				{
					dtp.Format = DateTimePickerFormat.Long;
				}
				else
				{
					dtp.Format = DateTimePickerFormat.Custom;
					dtp.CustomFormat = displayformat;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000F891C File Offset: 0x000F791C
		public static void setDisplayFormatDate(DataGridView dgv, string columnname, string displayformat)
		{
			try
			{
				if (!string.IsNullOrEmpty(displayformat) && !string.IsNullOrEmpty(columnname))
				{
					dgv.Columns[columnname].DefaultCellStyle.Format = displayformat;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x000F8968 File Offset: 0x000F7968
		public static int setSystemParamNotes(int NO, string Notes)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.setSystemParamNotes_Acc(NO, Notes);
			}
			try
			{
				string text = "UPDATE t_a_SystemParam SET [f_Notes] = " + wgTools.PrepareStrNUnicode(Notes) + " WHERE f_NO = " + NO.ToString();
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.ExecuteNonQuery();
						return 1;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return -9;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x000F8A1C File Offset: 0x000F7A1C
		public static int setSystemParamNotes_Acc(int NO, string Notes)
		{
			try
			{
				string text = "UPDATE t_a_SystemParam SET [f_Notes] = " + wgTools.PrepareStrNUnicode(Notes) + " WHERE f_NO = " + NO.ToString();
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.ExecuteNonQuery();
						return 1;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return -9;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x000F8AC0 File Offset: 0x000F7AC0
		public static int setSystemParamValue(int NO, string value)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.setSystemParamValue_Acc(NO, value);
			}
			try
			{
				string text = "UPDATE t_a_SystemParam SET [f_Value] = " + wgTools.PrepareStrNUnicode(value) + " WHERE f_NO = " + NO.ToString();
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.ExecuteNonQuery();
						return 1;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return -9;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x000F8B74 File Offset: 0x000F7B74
		public static int setSystemParamValue(int NO, string EName, string value, string notes)
		{
			string text;
			string text2;
			string text3;
			string text4;
			wgAppConfig.getSystemParamValue(NO, out text, out text2, out text3, out text4);
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text4))
			{
				string text5 = "INSERT INTO t_a_SystemParam ([f_NO], [f_Name], [f_EName], [f_Value], [f_Notes]) VALUES ( ";
				text5 = string.Concat(new string[]
				{
					text5,
					"  ",
					NO.ToString(),
					", ",
					wgTools.PrepareStrNUnicode(EName),
					", ",
					wgTools.PrepareStrNUnicode(EName),
					", ",
					wgTools.PrepareStrNUnicode(value),
					", ",
					wgTools.PrepareStrNUnicode(notes),
					")"
				});
				try
				{
					if (wgAppConfig.runUpdateSql(text5) > 0)
					{
						return 1;
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				return -9;
			}
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.setSystemParamValue_Acc(NO, EName, value, notes);
			}
			try
			{
				string text6 = "UPDATE t_a_SystemParam SET [f_Value] = " + wgTools.PrepareStrNUnicode(value);
				if (!string.IsNullOrEmpty(EName))
				{
					text6 = text6 + ", [f_EName] = " + wgTools.PrepareStrNUnicode(EName);
				}
				if (!string.IsNullOrEmpty(notes))
				{
					text6 = text6 + ", [f_Notes] = " + wgTools.PrepareStrNUnicode(notes);
				}
				text6 = text6 + " WHERE f_NO = " + NO.ToString();
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text6, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.ExecuteNonQuery();
						return 1;
					}
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			return -9;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x000F8D64 File Offset: 0x000F7D64
		public static int setSystemParamValue_Acc(int NO, string value)
		{
			try
			{
				string text = "UPDATE t_a_SystemParam SET [f_Value] = " + wgTools.PrepareStrNUnicode(value) + " WHERE f_NO = " + NO.ToString();
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.ExecuteNonQuery();
						return 1;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return -9;
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x000F8E08 File Offset: 0x000F7E08
		public static int setSystemParamValue_Acc(int NO, string EName, string value, string notes)
		{
			if (wgAppConfig.IsAccessDB)
			{
				try
				{
					string text = "UPDATE t_a_SystemParam SET [f_Value] = " + wgTools.PrepareStrNUnicode(value);
					if (!string.IsNullOrEmpty(EName))
					{
						text = text + ", [f_EName] = " + wgTools.PrepareStrNUnicode(EName);
					}
					if (!string.IsNullOrEmpty(notes))
					{
						text = text + ", [f_Notes] = " + wgTools.PrepareStrNUnicode(notes);
					}
					text = text + " WHERE f_NO = " + NO.ToString();
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							oleDbConnection.Open();
							oleDbCommand.ExecuteNonQuery();
							return 1;
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				return -9;
			}
			return -9;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x000F8EF4 File Offset: 0x000F7EF4
		public static int setSystemParamValueBool(int NO, bool value)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.setSystemParamValueBool_Acc(NO, value);
			}
			try
			{
				string text = "UPDATE t_a_SystemParam SET [f_Value] = " + (value ? "1" : "0") + " WHERE f_NO = " + NO.ToString();
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.ExecuteNonQuery();
						return 1;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return -9;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x000F8FB0 File Offset: 0x000F7FB0
		public static int setSystemParamValueBool_Acc(int NO, bool value)
		{
			try
			{
				string text = "UPDATE t_a_SystemParam SET [f_Value] = " + (value ? "1" : "0") + " WHERE f_NO = " + NO.ToString();
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.ExecuteNonQuery();
						return 1;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return -9;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x000F9060 File Offset: 0x000F8060
		public static int setSystemParamValueWithNotes(int NO, string EName, string value, string notes)
		{
			string text;
			string text2;
			string text3;
			string text4;
			wgAppConfig.getSystemParamValue(NO, out text, out text2, out text3, out text4);
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text4))
			{
				string text5 = "INSERT INTO t_a_SystemParam ([f_NO], [f_Name], [f_EName], [f_Value], [f_Notes]) VALUES ( ";
				text5 = string.Concat(new string[]
				{
					text5,
					"  ",
					NO.ToString(),
					", ",
					wgTools.PrepareStrNUnicode(EName),
					", ",
					wgTools.PrepareStrNUnicode(EName),
					", ",
					wgTools.PrepareStrNUnicode(value),
					", ",
					wgTools.PrepareStrNUnicode(notes),
					")"
				});
				try
				{
					if (wgAppConfig.runUpdateSql(text5) > 0)
					{
						return 1;
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				return -9;
			}
			if (wgAppConfig.IsAccessDB)
			{
				return wgAppConfig.setSystemParamValueWithNotes_Acc(NO, EName, value, notes);
			}
			try
			{
				string text6 = "UPDATE t_a_SystemParam SET [f_Value] = " + wgTools.PrepareStrNUnicode(value);
				if (!string.IsNullOrEmpty(EName))
				{
					text6 = text6 + ", [f_EName] = " + wgTools.PrepareStrNUnicode(EName);
				}
				text6 = string.Concat(new string[]
				{
					text6,
					", [f_Notes] = ",
					wgTools.PrepareStrNUnicode(notes),
					" WHERE f_NO = ",
					NO.ToString()
				});
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text6, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.ExecuteNonQuery();
						return 1;
					}
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			return -9;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x000F925C File Offset: 0x000F825C
		public static int setSystemParamValueWithNotes_Acc(int NO, string EName, string value, string notes)
		{
			if (wgAppConfig.IsAccessDB)
			{
				try
				{
					string text = "UPDATE t_a_SystemParam SET [f_Value] = " + wgTools.PrepareStrNUnicode(value);
					if (!string.IsNullOrEmpty(EName))
					{
						text = text + ", [f_EName] = " + wgTools.PrepareStrNUnicode(EName);
					}
					text = string.Concat(new string[]
					{
						text,
						", [f_Notes] = ",
						wgTools.PrepareStrNUnicode(notes),
						" WHERE f_NO = ",
						NO.ToString()
					});
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							oleDbConnection.Open();
							oleDbCommand.ExecuteNonQuery();
							return 1;
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				return -9;
			}
			return -9;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x000F9354 File Offset: 0x000F8354
		public static void ShowMyImage(string fileToDisplay, ref Image img)
		{
			try
			{
				wgAppConfig.DisposeImageRef(ref img);
				if (!string.IsNullOrEmpty(fileToDisplay) && wgAppConfig.FileIsExisted(fileToDisplay))
				{
					using (FileStream fileStream = new FileStream(fileToDisplay, FileMode.Open, FileAccess.Read))
					{
						byte[] array = new byte[fileStream.Length];
						fileStream.Read(array, 0, (int)fileStream.Length);
						using (MemoryStream memoryStream = new MemoryStream(array))
						{
							img = Image.FromStream(memoryStream);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x000F93F4 File Offset: 0x000F83F4
		private static int strToByteArr(string str, ref byte[] bytArr, int len)
		{
			int num = -1;
			try
			{
				int num2 = len;
				if (num2 < str.Length + 1 >> 1)
				{
					num2 = str.Length + 1 >> 1;
				}
				if (num2 < bytArr.Length)
				{
					num2 = bytArr.Length;
				}
				if (num2 <= 0)
				{
					return num;
				}
				for (int i = 0; i < num2; i++)
				{
					bytArr[i] = byte.Parse(str.Substring(2 * i, 2), NumberStyles.AllowHexSpecifier);
				}
				num = 0;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
			return num;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x000F947C File Offset: 0x000F847C
		public static void UpdateKeyVal(string key, string value)
		{
			wgAppConfig.UpdateKeyVal(key, value, "");
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x000F948C File Offset: 0x000F848C
		public static void UpdateKeyVal(string key, string value, string xmlfileName)
		{
			bool flag = false;
			try
			{
				string text = Application.StartupPath + "\\n3k_cust.xml";
				if (!string.IsNullOrEmpty(xmlfileName))
				{
					text = xmlfileName;
				}
				if (!File.Exists(text))
				{
					wgAppConfig.CreateCustXml();
				}
				if (File.Exists(text))
				{
					using (DataTable dataTable = new DataTable())
					{
						dataTable.TableName = "appSettings";
						dataTable.Columns.Add("key");
						dataTable.Columns.Add("value");
						dataTable.ReadXml(text);
						foreach (object obj in dataTable.Rows)
						{
							DataRow dataRow = (DataRow)obj;
							if (dataRow["key"].ToString() == key)
							{
								if (value == dataRow["value"].ToString())
								{
									return;
								}
								dataRow["value"] = value;
								dataTable.AcceptChanges();
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							DataRow dataRow2 = dataTable.NewRow();
							dataRow2["key"] = key;
							dataRow2["value"] = value;
							dataTable.Rows.Add(dataRow2);
							dataTable.AcceptChanges();
						}
						using (StringWriter stringWriter = new StringWriter())
						{
							using (StreamWriter streamWriter = new StreamWriter(text, false))
							{
								dataTable.WriteXml(stringWriter, XmlWriteMode.WriteSchema, true);
								streamWriter.Write(stringWriter.ToString());
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x000F96A8 File Offset: 0x000F86A8
		public static string weekdayToChsName(int weekDay)
		{
			string text = "";
			try
			{
				string[] array = new string[]
				{
					CommonStr.strSunday_Short,
					CommonStr.strMonday_Short,
					CommonStr.strTuesday_Short,
					CommonStr.strWednesday_Short,
					CommonStr.strThursday_Short,
					CommonStr.strFriday_Short,
					CommonStr.strSaturday_Short
				};
				if (weekDay >= 0 && weekDay <= 6)
				{
					text = array[weekDay];
				}
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x000F9720 File Offset: 0x000F8720
		public static void wgDBLog(string strMsg, EventLogEntryType entryType, byte[] rawData)
		{
			if (wgAppConfig.IsAccessDB)
			{
				wgAppConfig.wgDBLog_Acc(strMsg, entryType, rawData);
				return;
			}
			string text = string.Concat(new object[]
			{
				"INSERT INTO [t_s_wglog]( [f_EventType], [f_EventDesc], [f_UserID], [f_UserName])  VALUES( ",
				wgTools.PrepareStrNUnicode(entryType),
				",",
				wgTools.PrepareStrNUnicode(strMsg),
				",",
				icOperator.OperatorID,
				",",
				wgTools.PrepareStrNUnicode(icOperator.OperatorName),
				")"
			});
			try
			{
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(wgAppConfig.dbConString))
				{
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
						{
							if (sqlConnection.State != ConnectionState.Open)
							{
								sqlConnection.Open();
							}
							sqlCommand.ExecuteNonQuery();
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x000F9828 File Offset: 0x000F8828
		public static void wgDBLog_Acc(string strMsg, EventLogEntryType entryType, byte[] rawData)
		{
			if (wgAppConfig.IsAccessDB)
			{
				string text = string.Concat(new object[]
				{
					"INSERT INTO [t_s_wglog]( [f_EventType], [f_EventDesc], [f_UserID], [f_UserName])  VALUES( ",
					wgTools.PrepareStrNUnicode(entryType),
					",",
					wgTools.PrepareStrNUnicode(strMsg),
					",",
					icOperator.OperatorID,
					",",
					wgTools.PrepareStrNUnicode(icOperator.OperatorName),
					")"
				});
				try
				{
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(wgAppConfig.dbConString))
					{
						using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
						{
							using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
							{
								if (oleDbConnection.State != ConnectionState.Open)
								{
									oleDbConnection.Open();
								}
								oleDbCommand.ExecuteNonQuery();
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x000F992C File Offset: 0x000F892C
		public static void wgDebugWrite(string info)
		{
			wgAppConfig.wgLogWithoutDB(info, EventLogEntryType.Information, null);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x000F9936 File Offset: 0x000F8936
		public static void wgDebugWrite(string strMsg, EventLogEntryType entryType)
		{
			wgAppConfig.wgLogWithoutDB(strMsg, entryType, null);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000F9940 File Offset: 0x000F8940
		public static void wgLog(string strMsg)
		{
			wgAppConfig.wgLog(strMsg, EventLogEntryType.Information, null);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x000F994C File Offset: 0x000F894C
		public static void wgLog(string strMsg, EventLogEntryType entryType, byte[] rawData)
		{
			try
			{
				string text = string.Concat(new object[]
				{
					icOperator.OperatorID,
					".",
					icOperator.OperatorName,
					".",
					strMsg
				});
				text = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff") + "\t" + text;
				if (rawData != null)
				{
					text = text + "\t:" + Encoding.ASCII.GetString(rawData);
				}
				using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\n3k_log.log", true))
				{
					streamWriter.WriteLine(text);
				}
			}
			catch (Exception)
			{
			}
			try
			{
				wgAppConfig.wgDBLog(string.Concat(new object[]
				{
					icOperator.OperatorID,
					".",
					icOperator.OperatorName,
					".",
					strMsg
				}), entryType, rawData);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x000F9A68 File Offset: 0x000F8A68
		public static void wgLogRecEventOfController(string strMsg)
		{
			wgAppConfig.wgLogRecEventOfController(strMsg, EventLogEntryType.Information, null);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x000F9A74 File Offset: 0x000F8A74
		public static void wgLogRecEventOfController(string strMsg, EventLogEntryType entryType, byte[] rawData)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(wgAppConfig.Path4Doc() + "n3k_rec.log", true))
				{
					streamWriter.WriteLine(strMsg);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x000F9ACC File Offset: 0x000F8ACC
		public static void wgLogRecEventOfController1(string strMsg)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(wgAppConfig.Path4Doc() + "n3k_rec1.log", true))
				{
					streamWriter.WriteLine(strMsg);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x000F9B24 File Offset: 0x000F8B24
		public static void wgLogRecEventOfController2(string strMsg)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(wgAppConfig.Path4Doc() + "n3k_rec2.log", true))
				{
					streamWriter.WriteLine(strMsg);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x000F9B7C File Offset: 0x000F8B7C
		public static void wgLogRecEventOfController3(string strMsg)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(wgAppConfig.Path4Doc() + "n3k_rec3.log", true))
				{
					streamWriter.WriteLine(strMsg);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x000F9BD4 File Offset: 0x000F8BD4
		public static void wgLogWithoutDB(string strMsg)
		{
			wgAppConfig.wgLogWithoutDB(strMsg, EventLogEntryType.Information, null);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x000F9BE0 File Offset: 0x000F8BE0
		public static void wgLogWithoutDB(string strMsg, EventLogEntryType entryType, byte[] rawData)
		{
			try
			{
				string text = string.Concat(new object[]
				{
					icOperator.OperatorID,
					".",
					icOperator.OperatorName,
					".",
					strMsg
				});
				text = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff") + "\t" + text;
				if (rawData != null)
				{
					text = text + "\t:" + Encoding.ASCII.GetString(rawData);
				}
				using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\n3k_log.log", true))
				{
					streamWriter.WriteLine(text);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000F9CA8 File Offset: 0x000F8CA8
		public static void wgRunningStatusOfController(string strMsg)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(wgAppConfig.Path4Doc() + "n3k_monitor.log", false))
				{
					streamWriter.WriteLine(strMsg);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x000F9D00 File Offset: 0x000F8D00
		public static void writeLine(string info)
		{
			wgAppConfig.dtLast = DateTime.Now;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x000F9D0C File Offset: 0x000F8D0C
		public static string accessDbName
		{
			get
			{
				return "iCCard3000";
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x000F9D13 File Offset: 0x000F8D13
		private static string BackupDir
		{
			get
			{
				return Application.StartupPath + "\\BACKUP\\";
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x000F9D24 File Offset: 0x000F8D24
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x000F9D2B File Offset: 0x000F8D2B
		public static string CultureInfoStr
		{
			get
			{
				return wgAppConfig.m_CultureInfoStr;
			}
			set
			{
				if (value != null)
				{
					wgAppConfig.m_CultureInfoStr = value;
				}
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x000F9D36 File Offset: 0x000F8D36
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x000F9D3D File Offset: 0x000F8D3D
		public static string dbConString
		{
			get
			{
				return wgAppConfig.m_dbConString;
			}
			set
			{
				wgAppConfig.m_dbConString = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x000F9D45 File Offset: 0x000F8D45
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x000F9D4C File Offset: 0x000F8D4C
		public static string dbName
		{
			get
			{
				return wgAppConfig.m_dbName;
			}
			set
			{
				wgAppConfig.m_dbName = value;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x000F9D54 File Offset: 0x000F8D54
		public static string dbWEBUserName
		{
			get
			{
				return "WEBUsers";
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x000F9D5B File Offset: 0x000F8D5B
		public static bool IsAcceleratorActive
		{
			get
			{
				return !wgAppConfig.IsAccessControlBlue && (wgAppConfig.getParamValBoolByNO(166) || wgTools.bUDPOnly64 > 0);
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x000F9D7C File Offset: 0x000F8D7C
		public static bool IsAccessControlBlue
		{
			get
			{
				return wgAppConfig.ProductTypeOfApp == "AccessControl" || wgTools.gWGYTJ;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x000F9D96 File Offset: 0x000F8D96
		// (set) Token: 0x06000C3A RID: 3130 RVA: 0x000F9D9D File Offset: 0x000F8D9D
		public static bool IsAccessDB
		{
			get
			{
				return wgAppConfig.m_IsAccessDB;
			}
			set
			{
				wgAppConfig.m_IsAccessDB = value;
				wgTools.IsSqlServer = !value;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x000F9DAE File Offset: 0x000F8DAE
		public static bool IsActivateCameraManage
		{
			get
			{
				return !wgAppConfig.IsAccessControlBlue && wgAppConfig.getParamValBoolByNO(156);
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x000F9DC3 File Offset: 0x000F8DC3
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x000F9DEA File Offset: 0x000F8DEA
		public static bool IsActivateCard19
		{
			get
			{
				if (wgAppConfig.valActivateCard19 < 0)
				{
					if (wgAppConfig.IsAccessControlBlue)
					{
						wgAppConfig.valActivateCard19 = 0;
					}
					else
					{
						wgAppConfig.valActivateCard19 = 1;
					}
				}
				return wgAppConfig.valActivateCard19 > 0;
			}
			set
			{
				if (value)
				{
					wgAppConfig.valActivateCard19 = 1;
				}
				else
				{
					wgAppConfig.valActivateCard19 = 0;
				}
				if (wgAppConfig.IsAccessControlBlue)
				{
					wgAppConfig.valActivateCard19 = 0;
				}
				wgAppConfig.setSystemParamValue(192, "ActivateCard 19 V7.70", wgAppConfig.valActivateCard19.ToString(), "2016-11-28 15:03:39");
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x000F9E29 File Offset: 0x000F8E29
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x000F9E55 File Offset: 0x000F8E55
		public static bool IsActivateInvalidCardMoreTimesWarn
		{
			get
			{
				if (wgAppConfig.valActivateOpenInvalidCardMoreTimesWarn < 0)
				{
					if (wgAppConfig.getParamValBoolByNO(211))
					{
						wgAppConfig.valActivateOpenInvalidCardMoreTimesWarn = 1;
					}
					else
					{
						wgAppConfig.valActivateOpenInvalidCardMoreTimesWarn = 0;
					}
				}
				return wgAppConfig.valActivateOpenInvalidCardMoreTimesWarn > 0;
			}
			set
			{
				if (value)
				{
					wgAppConfig.valActivateOpenInvalidCardMoreTimesWarn = 1;
					return;
				}
				wgAppConfig.valActivateOpenInvalidCardMoreTimesWarn = 0;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x000F9E67 File Offset: 0x000F8E67
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x000F9E93 File Offset: 0x000F8E93
		public static bool IsActivateOpenTooLongWarn
		{
			get
			{
				if (wgAppConfig.valActivateOpenTooLongWarn < 0)
				{
					if (wgAppConfig.getParamValBoolByNO(180))
					{
						wgAppConfig.valActivateOpenTooLongWarn = 1;
					}
					else
					{
						wgAppConfig.valActivateOpenTooLongWarn = 0;
					}
				}
				return wgAppConfig.valActivateOpenTooLongWarn > 0;
			}
			set
			{
				if (value)
				{
					wgAppConfig.valActivateOpenTooLongWarn = 1;
					return;
				}
				wgAppConfig.valActivateOpenTooLongWarn = 0;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x000F9EA5 File Offset: 0x000F8EA5
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x000F9EE0 File Offset: 0x000F8EE0
		public static bool IsPhotoNameFromConsumerNO
		{
			get
			{
				if (wgAppConfig.valPhotoNameFromConsumerNO < 0)
				{
					if (wgAppConfig.IsAccessControlBlue)
					{
						wgAppConfig.valPhotoNameFromConsumerNO = 0;
					}
					else if (wgAppConfig.getParamValBoolByNO(193))
					{
						wgAppConfig.valPhotoNameFromConsumerNO = 1;
					}
					else
					{
						wgAppConfig.valPhotoNameFromConsumerNO = 0;
					}
				}
				return wgAppConfig.valPhotoNameFromConsumerNO > 0;
			}
			set
			{
				if (value)
				{
					wgAppConfig.valPhotoNameFromConsumerNO = 1;
				}
				else
				{
					wgAppConfig.valPhotoNameFromConsumerNO = 0;
				}
				if (wgAppConfig.IsAccessControlBlue)
				{
					wgAppConfig.valPhotoNameFromConsumerNO = 0;
				}
				wgAppConfig.setSystemParamValue(193, "PhotoNameFromConsumerNO", wgAppConfig.valPhotoNameFromConsumerNO.ToString(), "2017-06-23 21:08:24");
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x000F9F1F File Offset: 0x000F8F1F
		public static bool IsPrivilegeTypeManagementModeActive
		{
			get
			{
				return !wgAppConfig.IsAccessControlBlue && wgAppConfig.getParamValBoolByNO(167);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x000F9F34 File Offset: 0x000F8F34
		public static int LogEventMaxCount
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x000F9F3B File Offset: 0x000F8F3B
		public static int PasswordMaxLenght
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x04001A84 RID: 6788
		public const int dbControllerUserDefaultPassword = 345678;

		// Token: 0x04001A85 RID: 6789
		public const string gc_EventSourceName = "n3k_log";

		// Token: 0x04001A86 RID: 6790
		private const string n3k_cust = "\\n3k_cust.xml";

		// Token: 0x04001A87 RID: 6791
		private static ArrayList arrPhotoFileFullNames = new ArrayList();

		// Token: 0x04001A88 RID: 6792
		public static bool bFloorRoomManager = false;

		// Token: 0x04001A89 RID: 6793
		public static bool bForceRestart = false;

		// Token: 0x04001A8A RID: 6794
		public static bool bNeedRestore = false;

		// Token: 0x04001A8B RID: 6795
		private static Icon currenAppIcon = null;

		// Token: 0x04001A8C RID: 6796
		public static int dbCommandTimeout = 600;

		// Token: 0x04001A8D RID: 6797
		public static bool DBIsConnected = true;

		// Token: 0x04001A8E RID: 6798
		public static string defaultCustConfigzhCHS = "<NewDataSet>\r\n  <xs:schema id=\"NewDataSet\" xmlns=\"\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n    <xs:element name=\"NewDataSet\" msdata:IsDataSet=\"true\" msdata:MainDataTable=\"appSettings\" msdata:UseCurrentLocale=\"true\">\r\n      <xs:complexType>\r\n        <xs:choice minOccurs=\"0\" maxOccurs=\"unbounded\">\r\n          <xs:element name=\"appSettings\">\r\n            <xs:complexType>\r\n              <xs:sequence>\r\n                <xs:element name=\"key\" type=\"xs:string\" minOccurs=\"0\" />\r\n                <xs:element name=\"value\" type=\"xs:string\" minOccurs=\"0\" />\r\n              </xs:sequence>\r\n            </xs:complexType>\r\n          </xs:element>\r\n        </xs:choice>\r\n      </xs:complexType>\r\n    </xs:element>\r\n  </xs:schema>\r\n  <appSettings>\r\n    <key>dbConnection</key>\r\n    <value/>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>Language</key>\r\n    <value>zh-CHS</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>autologinName</key>\r\n    <value />\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>autologinPassword</key>\r\n    <value />\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>rgtries</key>\r\n    <value>1</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>NewSoftwareVersionInfo</key>\r\n    <value>1.0.2</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>RunTimes</key>\r\n    <value></value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>NewSoftwareSpecialVersionInfo</key>\r\n    <value>1.0.2</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>CommCurrent</key>\r\n    <value />\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>RunTimeAt</key>\r\n    <value>0</value>\r\n  </appSettings>\r\n</NewDataSet>";

		// Token: 0x04001A8F RID: 6799
		public static DateTime dtLast = DateTime.Now;

		// Token: 0x04001A90 RID: 6800
		private static DataView dv = null;

		// Token: 0x04001A91 RID: 6801
		public static long glngReceiveAutoUploadCount = 0L;

		// Token: 0x04001A92 RID: 6802
		public static long glngReceiveCount = 0L;

		// Token: 0x04001A93 RID: 6803
		public static bool gRestart = false;

		// Token: 0x04001A94 RID: 6804
		public static bool IsAutoLogin = false;

		// Token: 0x04001A95 RID: 6805
		public static bool IsLogin = false;

		// Token: 0x04001A96 RID: 6806
		private static string lastPhotoDirectoryName = "";

		// Token: 0x04001A97 RID: 6807
		public static string LoginTitle = "";

		// Token: 0x04001A98 RID: 6808
		private static bool m_bCreatePhotoDirectory = false;

		// Token: 0x04001A99 RID: 6809
		private static bool m_bFindDirectoryNetShare = false;

		// Token: 0x04001A9A RID: 6810
		private static string m_CultureInfoStr = "";

		// Token: 0x04001A9B RID: 6811
		private static string m_dbConString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=AccessData;Data Source=(local) ";

		// Token: 0x04001A9C RID: 6812
		private static string m_dbName = "AccessData";

		// Token: 0x04001A9D RID: 6813
		private static string m_DirectoryNetShare = "";

		// Token: 0x04001A9E RID: 6814
		private static bool m_IsAccessDB = false;

		// Token: 0x04001A9F RID: 6815
		private static string m_path4AviJpg = wgAppConfig.Path4AviJpgDefault();

		// Token: 0x04001AA0 RID: 6816
		private static string m_path4AviJpgOnlyView = wgAppConfig.Path4AviJpgDefault();

		// Token: 0x04001AA1 RID: 6817
		private static string m_path4Photo = wgAppConfig.Path4PhotoDefault();

		// Token: 0x04001AA2 RID: 6818
		private static string m_PhotoDiriectyName = "";

		// Token: 0x04001AA3 RID: 6819
		private static int photoDirectoryLastFileCount = -1;

		// Token: 0x04001AA4 RID: 6820
		public static DateTime photoDirectoryLastWriteTime = default(DateTime);

		// Token: 0x04001AA5 RID: 6821
		public static string ProductTypeOfApp = "AccessControl";

		// Token: 0x04001AA6 RID: 6822
		private static DataTable tb = null;

		// Token: 0x04001AA7 RID: 6823
		private static int tryCreateCnt = 0;

		// Token: 0x04001AA8 RID: 6824
		private static int valActivateCard19 = -1;

		// Token: 0x04001AA9 RID: 6825
		private static int valActivateOpenInvalidCardMoreTimesWarn = -1;

		// Token: 0x04001AAA RID: 6826
		private static int valActivateOpenTooLongWarn = -1;

		// Token: 0x04001AAB RID: 6827
		private static int valPhotoNameFromConsumerNO = -1;
	}
}
