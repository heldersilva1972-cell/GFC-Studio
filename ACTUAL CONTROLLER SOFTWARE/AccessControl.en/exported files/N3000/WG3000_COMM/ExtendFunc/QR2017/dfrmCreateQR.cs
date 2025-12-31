using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace WG3000_COMM.ExtendFunc.QR2017
{
	// Token: 0x0200031F RID: 799
	public partial class dfrmCreateQR : frmN3000
	{
		// Token: 0x060018D6 RID: 6358 RVA: 0x002074A7 File Offset: 0x002064A7
		public dfrmCreateQR()
		{
			this.InitializeComponent();
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x002074CC File Offset: 0x002064CC
		private void batchCreateFilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				string strNewName;
				using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
				{
					dfrmInputNewName.Text = CommonStr.strQRStartCardNO;
					dfrmInputNewName.label1.Text = CommonStr.strQRStartCardNO;
					dfrmInputNewName.strNewName = "";
					uint num;
					if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || !uint.TryParse(dfrmInputNewName.strNewName, out num))
					{
						return;
					}
					strNewName = dfrmInputNewName.strNewName;
				}
				string text;
				using (dfrmInputNewName dfrmInputNewName2 = new dfrmInputNewName())
				{
					dfrmInputNewName2.Text = CommonStr.strQREndCardNO;
					dfrmInputNewName2.label1.Text = CommonStr.strQREndCardNO;
					dfrmInputNewName2.strNewName = "";
					if (dfrmInputNewName2.ShowDialog(this) == DialogResult.OK)
					{
						uint num;
						if (!uint.TryParse(dfrmInputNewName2.strNewName, out num))
						{
							text = strNewName;
						}
						else
						{
							text = dfrmInputNewName2.strNewName;
						}
					}
					else
					{
						text = strNewName;
					}
				}
				if (Information.IsNumeric(strNewName) && Information.IsNumeric(text))
				{
					if (int.Parse(strNewName) <= int.Parse(text))
					{
						this.cardStart = (long)int.Parse(strNewName);
						this.cardEnd = (long)int.Parse(text);
						if (this.cardStart <= 0L || this.cardEnd <= 0L)
						{
							XMessageBox.Show(this, CommonStr.strCheckCard, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						else if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSureAutoCreateQR + ": {0:d}--{1:d} [{2:d}] ?", this.cardStart, this.cardEnd, (this.cardEnd - this.cardStart + 1L > 0L) ? (this.cardEnd - this.cardStart + 1L) : 1L), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
						{
							this.bStopSave = false;
							this.saveMoreFiles();
						}
					}
					else
					{
						XMessageBox.Show(CommonStr.strQRWrongCardNO);
					}
				}
				else
				{
					XMessageBox.Show(CommonStr.strQRWrongCardNO);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x002076FC File Offset: 0x002066FC
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00207704 File Offset: 0x00206704
		private void btnCopy_Click(object sender, EventArgs e)
		{
			try
			{
				Bitmap bitmap = new Bitmap(this.panel1.Width, this.panel1.Height);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.CopyFromScreen(this.panel1.PointToScreen(Point.Empty), Point.Empty, this.panel1.Size);
				Clipboard.SetImage(bitmap);
				graphics.Dispose();
			}
			catch
			{
			}
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00207784 File Offset: 0x00206784
		private void btnCreateQR_Click(object sender, EventArgs e)
		{
			if (this.textBoxText.Text == string.Empty)
			{
				MessageBox.Show("输入内容不能为空！");
				return;
			}
			EncodingOptions encodingOptions = new QrCodeEncodingOptions
			{
				DisableECI = true,
				CharacterSet = "UTF-8",
				Width = this.pictureBoxQr.Width,
				Height = this.pictureBoxQr.Height
			};
			Bitmap bitmap = new BarcodeWriter
			{
				Format = 2048,
				Options = encodingOptions
			}.Write(this.textBoxText.Text);
			this.pictureBoxQr.Image = bitmap;
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0020782C File Offset: 0x0020682C
		private void btnCurrentDay_Click(object sender, EventArgs e)
		{
			DateTime dateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
			this.updateTime(dateTime);
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x00207858 File Offset: 0x00206858
		private void btnCurrentYear_Click(object sender, EventArgs e)
		{
			DateTime dateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-12-31 23:59:59"));
			this.updateTime(dateTime);
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x00207884 File Offset: 0x00206884
		private void btnHalfHour_Click(object sender, EventArgs e)
		{
			DateTime dateTime = DateTime.Now.AddHours(0.5);
			this.updateTime(dateTime);
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x002078B0 File Offset: 0x002068B0
		private void btnOneDay_Click(object sender, EventArgs e)
		{
			DateTime dateTime = DateTime.Now.AddDays(1.0);
			this.updateTime(dateTime);
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x002078DC File Offset: 0x002068DC
		private void btnOneHour_Click(object sender, EventArgs e)
		{
			DateTime dateTime = DateTime.Now.AddHours(1.0);
			this.updateTime(dateTime);
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x00207908 File Offset: 0x00206908
		private void btnOneMonth_Click(object sender, EventArgs e)
		{
			DateTime dateTime = DateTime.Parse(DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd 23:59:59"));
			this.updateTime(dateTime);
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x00207940 File Offset: 0x00206940
		private void btnOneWeek_Click(object sender, EventArgs e)
		{
			DateTime dateTime = DateTime.Parse(DateTime.Now.AddDays(7.0).ToString("yyyy-MM-dd 23:59:59"));
			this.updateTime(dateTime);
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x00207980 File Offset: 0x00206980
		private void btnOneYear_Click(object sender, EventArgs e)
		{
			DateTime dateTime = DateTime.Parse(DateTime.Now.AddYears(1).ToString("yyyy-MM-dd 23:59:59"));
			this.updateTime(dateTime);
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x002079B8 File Offset: 0x002069B8
		private void btnPrint_Click(object sender, EventArgs e)
		{
			if (new PrintDialog
			{
				Document = this.printDocument1
			}.ShowDialog() == DialogResult.OK)
			{
				try
				{
					this.InitPrint();
					StandardPrintController standardPrintController = new StandardPrintController();
					this.printDocument1.PrintController = standardPrintController;
					this.printDocument1.Print();
				}
				catch
				{
					this.printDocument1.PrintController.OnEndPrint(this.printDocument1, new PrintEventArgs());
				}
			}
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x00207A58 File Offset: 0x00206A58
		private void createNewQR()
		{
			if (!this.bCreating)
			{
				try
				{
					this.bCreating = true;
					this.dateBeginHMS1.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dtpActivate.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dtpDeactivate.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dateEndHMS1.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dtpActivate.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpActivate.Value.ToString("yyyy-MM-dd"), this.dateBeginHMS1.Value.ToString("HH:mm:ss")));
					this.dtpDeactivate.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpDeactivate.Value.ToString("yyyy-MM-dd"), this.dateEndHMS1.Value.ToString("HH:mm:ss")));
					if (this.dtpDeactivate.Value > this.dtpDeactivateInDB)
					{
						this.dtpDeactivate.Value = this.dtpDeactivateInDB;
					}
					if (this.dtpActivate.Value.Date < DateTime.Now.Date)
					{
						this.dtpActivate.Value = DateTime.Now;
					}
					if (this.dtpDeactivate.Value < DateTime.Now)
					{
						this.dtpDeactivate.Value = DateTime.Now.AddMinutes(5.0);
					}
					if (this.dtpDeactivate.Value < this.dtpActivate.Value)
					{
						this.dtpDeactivate.Value = this.dtpActivate.Value;
					}
					if (this.dtpDeactivate.Value == this.dtpActivate.Value)
					{
						this.dtpDeactivate.Value = this.dtpActivate.Value.AddMinutes(5.0);
					}
					if (this.dtpDeactivate.Value > this.dtpDeactivateInDB)
					{
						this.dtpDeactivate.Value = this.dtpDeactivateInDB;
					}
					this.dateBeginHMS1.Value = this.dtpActivate.Value;
					this.dateEndHMS1.Value = this.dtpDeactivate.Value;
					this.lblInfo.Text = string.Format("{0} .. {1}", this.dtpActivate.Value.ToString("yyyy-MM-dd HH:mm"), this.dtpDeactivate.Value.ToString("yyyy-MM-dd HH:mm"));
					int days = this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Days;
					if (days == 0)
					{
						if (this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Seconds == 0)
						{
							this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0:D2}:{1:D2})", this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Hours, this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Minutes);
						}
						else
						{
							this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0:D2}:{1:D2})", this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Hours, this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Minutes + 1);
						}
					}
					else if (this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Hours > 0)
					{
						this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0})", days + 1);
					}
					else
					{
						this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0})", days);
					}
					this.txtInfo.Text = this.lblInfo.Text;
					byte[] array = new byte[]
					{
						119, 103, 0, 0, 0, 0, 0, 0, 32, 65,
						57, 16, 132, 54, 151, 87
					};
					byte[] array2 = new byte[]
					{
						210, 2, 150, 73, 0, 0, 0, 0, 23, 5,
						4, 8, 54, 153, 0, 0
					};
					array2[15] = 2;
					uint num = wgTools.MsDateToWgDateYMD(this.dtpActivate.Value);
					uint num2 = wgTools.MsDateToWgDateHMS(this.dtpActivate.Value);
					array2[8] = (byte)(num & 255U);
					array2[9] = (byte)((num >> 8) & 255U);
					array2[10] = (byte)((num2 >> 8) & 255U);
					array2[11] = (byte)(num2 & 224U);
					num = wgTools.MsDateToWgDateYMD(this.dtpDeactivate.Value);
					num2 = wgTools.MsDateToWgDateHMS(this.dtpDeactivate.Value);
					array2[12] = (byte)(num & 255U);
					array2[13] = (byte)((num >> 8) & 255U);
					array2[14] = (byte)((num2 >> 8) & 255U);
					array2[11] = (byte)((uint)array2[11] + ((num2 & 224U) >> 4));
					if ((num2 & 224U) == 224U)
					{
						array2[14] = array2[14] + 1;
					}
					long num3 = this.consumerCardNO;
					if (num3 > 0L)
					{
						array2[0] = (byte)(num3 & 255L);
						array2[1] = (byte)((num3 >> 8) & 255L);
						array2[2] = (byte)((num3 >> 16) & 255L);
						array2[3] = (byte)((num3 >> 24) & 255L);
						array2[4] = (byte)((num3 >> 32) & 255L);
						array2[5] = (byte)((num3 >> 40) & 255L);
						array2[6] = (byte)((num3 >> 48) & 255L);
						array2[7] = (byte)((num3 >> 56) & 255L);
					}
					dfrmCreateQR.EncryptSM4_ECB(ref array2, array);
					string text = BitConverter.ToString(array2).Replace("-", "");
					EncodingOptions encodingOptions = new QrCodeEncodingOptions
					{
						DisableECI = true,
						CharacterSet = "UTF-8",
						Margin = 1,
						Width = this.pictureBoxQr.Width,
						Height = this.pictureBoxQr.Height
					};
					Bitmap bitmap = new BarcodeWriter
					{
						Format = 2048,
						Options = encodingOptions
					}.Write(text);
					this.pictureBoxQr.Image = bitmap;
					this.dateBeginHMS1.ValueChanged += this.dtpDeactivate_ValueChanged;
					this.dtpActivate.ValueChanged += this.dtpDeactivate_ValueChanged;
					this.dtpDeactivate.ValueChanged += this.dtpDeactivate_ValueChanged;
					this.dateEndHMS1.ValueChanged += this.dtpDeactivate_ValueChanged;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				wgAppConfig.wgLogWithoutDB(DateTime.Now.ToString());
				this.bCreating = false;
			}
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x00208258 File Offset: 0x00207258
		private void createNewQR4TestCommand64()
		{
			if (!this.bCreating)
			{
				try
				{
					this.bCreating = true;
					this.dateBeginHMS1.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dtpActivate.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dtpDeactivate.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dateEndHMS1.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dtpActivate.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpActivate.Value.ToString("yyyy-MM-dd"), this.dateBeginHMS1.Value.ToString("HH:mm:ss")));
					this.dtpDeactivate.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpDeactivate.Value.ToString("yyyy-MM-dd"), this.dateEndHMS1.Value.ToString("HH:mm:ss")));
					if (this.dtpDeactivate.Value > this.dtpDeactivateInDB)
					{
						this.dtpDeactivate.Value = this.dtpDeactivateInDB;
					}
					if (this.dtpActivate.Value.Date < DateTime.Now.Date)
					{
						this.dtpActivate.Value = DateTime.Now;
					}
					if (this.dtpDeactivate.Value < DateTime.Now)
					{
						this.dtpDeactivate.Value = DateTime.Now.AddMinutes(5.0);
					}
					if (this.dtpDeactivate.Value < this.dtpActivate.Value)
					{
						this.dtpDeactivate.Value = this.dtpActivate.Value;
					}
					if (this.dtpDeactivate.Value == this.dtpActivate.Value)
					{
						this.dtpDeactivate.Value = this.dtpActivate.Value.AddMinutes(5.0);
					}
					if (this.dtpDeactivate.Value > this.dtpDeactivateInDB)
					{
						this.dtpDeactivate.Value = this.dtpDeactivateInDB;
					}
					if (this.dtpDeactivate.Value > DateTime.Now.AddYears(1))
					{
						this.dtpDeactivate.Value = DateTime.Parse(DateTime.Now.AddYears(1).AddDays(-1.0).ToString("yyyy-MM-dd 23:59:59"));
					}
					this.dateBeginHMS1.Value = this.dtpActivate.Value;
					this.dateEndHMS1.Value = this.dtpDeactivate.Value;
					this.lblInfo.Text = string.Format("{0} .. {1}", this.dtpActivate.Value.ToString("yyyy-MM-dd HH:mm"), this.dtpDeactivate.Value.ToString("yyyy-MM-dd HH:mm"));
					int days = this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Days;
					if (days == 0)
					{
						if (this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Seconds == 0)
						{
							this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0:D2}:{1:D2})", this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Hours, this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Minutes);
						}
						else
						{
							this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0:D2}:{1:D2})", this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Hours, this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Minutes + 1);
						}
					}
					else if (this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Hours > 0)
					{
						this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0})", days + 1);
					}
					else
					{
						this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0})", days);
					}
					this.txtInfo.Text = this.lblInfo.Text;
					byte[] array = new byte[]
					{
						119, 103, 0, 0, 0, 0, 0, 0, 32, 65,
						57, 16, 132, 54, 151, 87
					};
					byte[] array2 = new byte[]
					{
						210, 2, 150, 73, 0, 0, 0, 0, 23, 5,
						4, 8, 54, 153, 0, 0
					};
					array2[15] = 2;
					uint num = wgTools.MsDateToWgDateYMD(this.dtpActivate.Value);
					uint num2 = wgTools.MsDateToWgDateHMS(this.dtpActivate.Value);
					array2[8] = (byte)(num & 255U);
					array2[9] = (byte)((num >> 8) & 255U);
					array2[10] = (byte)((num2 >> 8) & 255U);
					array2[11] = (byte)(num2 & 224U);
					num = wgTools.MsDateToWgDateYMD(this.dtpDeactivate.Value);
					num2 = wgTools.MsDateToWgDateHMS(this.dtpDeactivate.Value);
					array2[12] = (byte)(num & 255U);
					array2[13] = (byte)((num >> 8) & 255U);
					array2[14] = (byte)((num2 >> 8) & 255U);
					array2[11] = (byte)((uint)array2[11] + ((num2 & 224U) >> 4));
					if ((num2 & 224U) == 224U)
					{
						array2[14] = array2[14] + 1;
					}
					long num3 = 6172933524171347370L;
					if (num3 > 0L)
					{
						array2[0] = (byte)(num3 & 255L);
						array2[1] = (byte)((num3 >> 8) & 255L);
						array2[2] = (byte)((num3 >> 16) & 255L);
						array2[3] = (byte)((num3 >> 24) & 255L);
						array2[4] = (byte)((num3 >> 32) & 255L);
						array2[5] = (byte)((num3 >> 40) & 255L);
						array2[6] = (byte)((num3 >> 48) & 255L);
						array2[7] = (byte)((num3 >> 56) & 255L);
					}
					dfrmCreateQR.EncryptSM4_ECB(ref array2, array);
					byte[] array3 = new byte[]
					{
						23, 64, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 1, 0,
						0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
						18, 52, 0, 0, 0, 0, 0, 0, 0, 0,
						0, 0, 90, 0, 0, 0, 0, 0, 0, 0,
						0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
						0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
						0, 0, 0, 0
					};
					dfrmCreateQR.EncryptSM4_ECB_64Byte(ref array3, array);
					string text = BitConverter.ToString(array2).Replace("-", "") + BitConverter.ToString(array3).Replace("-", "");
					EncodingOptions encodingOptions = new QrCodeEncodingOptions
					{
						DisableECI = true,
						CharacterSet = "UTF-8",
						Margin = 1,
						Width = this.pictureBoxQr.Width,
						Height = this.pictureBoxQr.Height
					};
					Bitmap bitmap = new BarcodeWriter
					{
						Format = 2048,
						Options = encodingOptions
					}.Write(text);
					this.pictureBoxQr.Image = bitmap;
					this.dateBeginHMS1.ValueChanged += this.dtpDeactivate_ValueChanged;
					this.dtpActivate.ValueChanged += this.dtpDeactivate_ValueChanged;
					this.dtpDeactivate.ValueChanged += this.dtpDeactivate_ValueChanged;
					this.dateEndHMS1.ValueChanged += this.dtpDeactivate_ValueChanged;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				wgAppConfig.wgLogWithoutDB(DateTime.Now.ToString());
				this.bCreating = false;
			}
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x00208AB0 File Offset: 0x00207AB0
		private void createNewQRFile()
		{
			if (!this.bCreating)
			{
				try
				{
					this.bCreating = true;
					this.dateBeginHMS1.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dtpActivate.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dtpDeactivate.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dateEndHMS1.ValueChanged -= this.dtpDeactivate_ValueChanged;
					this.dtpActivate.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpActivate.Value.ToString("yyyy-MM-dd"), this.dateBeginHMS1.Value.ToString("HH:mm:ss")));
					this.dtpDeactivate.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpDeactivate.Value.ToString("yyyy-MM-dd"), this.dateEndHMS1.Value.ToString("HH:mm:ss")));
					if (this.dtpDeactivate.Value > this.dtpDeactivateInDB)
					{
						this.dtpDeactivate.Value = this.dtpDeactivateInDB;
					}
					if (this.dtpActivate.Value.Date < DateTime.Now.Date)
					{
						this.dtpActivate.Value = DateTime.Now;
					}
					if (this.dtpDeactivate.Value < DateTime.Now)
					{
						this.dtpDeactivate.Value = DateTime.Now.AddMinutes(5.0);
					}
					if (this.dtpDeactivate.Value < this.dtpActivate.Value)
					{
						this.dtpDeactivate.Value = this.dtpActivate.Value;
					}
					if (this.dtpDeactivate.Value == this.dtpActivate.Value)
					{
						this.dtpDeactivate.Value = this.dtpActivate.Value.AddMinutes(5.0);
					}
					if (this.dtpDeactivate.Value > this.dtpDeactivateInDB)
					{
						this.dtpDeactivate.Value = this.dtpDeactivateInDB;
					}
					if (this.dtpDeactivate.Value > DateTime.Now.AddYears(1))
					{
						this.dtpDeactivate.Value = DateTime.Parse(DateTime.Now.AddYears(1).AddDays(-1.0).ToString("yyyy-MM-dd 23:59:59"));
					}
					this.dateBeginHMS1.Value = this.dtpActivate.Value;
					this.dateEndHMS1.Value = this.dtpDeactivate.Value;
					this.lblInfo.Text = string.Format("{0} .. {1}", this.dtpActivate.Value.ToString("yyyy-MM-dd HH:mm"), this.dtpDeactivate.Value.ToString("yyyy-MM-dd HH:mm"));
					int days = this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Days;
					if (days == 0)
					{
						if (this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Seconds == 0)
						{
							this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0:D2}:{1:D2})", this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Hours, this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Minutes);
						}
						else
						{
							this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0:D2}:{1:D2})", this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Hours, this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Minutes + 1);
						}
					}
					else if (this.dtpDeactivate.Value.Subtract(this.dtpActivate.Value).Hours > 0)
					{
						this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0})", days + 1);
					}
					else
					{
						this.lblInfo.Text = this.lblInfo.Text + string.Format("  ({0})", days);
					}
					this.txtInfo.Text = this.lblInfo.Text;
					byte[] array = new byte[]
					{
						119, 103, 0, 0, 0, 0, 0, 0, 32, 65,
						57, 16, 132, 54, 151, 87
					};
					byte[] array2 = new byte[]
					{
						210, 2, 150, 73, 0, 0, 0, 0, 23, 5,
						4, 8, 54, 153, 0, 0
					};
					array2[15] = 2;
					uint num = wgTools.MsDateToWgDateYMD(this.dtpActivate.Value);
					uint num2 = wgTools.MsDateToWgDateHMS(this.dtpActivate.Value);
					array2[8] = (byte)(num & 255U);
					array2[9] = (byte)((num >> 8) & 255U);
					array2[10] = (byte)((num2 >> 8) & 255U);
					array2[11] = (byte)(num2 & 224U);
					num = wgTools.MsDateToWgDateYMD(this.dtpDeactivate.Value);
					num2 = wgTools.MsDateToWgDateHMS(this.dtpDeactivate.Value);
					array2[12] = (byte)(num & 255U);
					array2[13] = (byte)((num >> 8) & 255U);
					array2[14] = (byte)((num2 >> 8) & 255U);
					array2[11] = (byte)((uint)array2[11] + ((num2 & 224U) >> 4));
					long num3 = this.consumerCardNO;
					if (num3 > 0L)
					{
						array2[0] = (byte)(num3 & 255L);
						array2[1] = (byte)((num3 >> 8) & 255L);
						array2[2] = (byte)((num3 >> 16) & 255L);
						array2[3] = (byte)((num3 >> 24) & 255L);
						array2[4] = (byte)((num3 >> 32) & 255L);
						array2[5] = (byte)((num3 >> 40) & 255L);
						array2[6] = (byte)((num3 >> 48) & 255L);
						array2[7] = (byte)((num3 >> 56) & 255L);
					}
					dfrmCreateQR.EncryptSM4_ECB(ref array2, array);
					string text = BitConverter.ToString(array2).Replace("-", "");
					EncodingOptions encodingOptions = new QrCodeEncodingOptions
					{
						DisableECI = true,
						CharacterSet = "UTF-8",
						Margin = 1,
						Width = this.pictureBoxQr.Width,
						Height = this.pictureBoxQr.Height
					};
					Bitmap bitmap = new BarcodeWriter
					{
						Format = 2048,
						Options = encodingOptions
					}.Write(text);
					this.pictureBoxQr.Image = bitmap;
					string text2 = wgAppConfig.Path4AviJpgDefault() + this.lblUser.Text + ".png";
					FileInfo fileInfo = new FileInfo(text2);
					if (fileInfo.Exists)
					{
						try
						{
							fileInfo.Delete();
						}
						catch (Exception)
						{
						}
					}
					bitmap.Save(text2);
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				wgAppConfig.wgLogWithoutDB(DateTime.Now.ToString());
				this.bCreating = false;
			}
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x00209290 File Offset: 0x00208290
		private void dfrmCreateQR_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.bStopSave = true;
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x0020929C File Offset: 0x0020829C
		private void dfrmCreateQR_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE"))))
			{
				try
				{
					if (wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE")).IndexOf("%") > 0)
					{
						int num = int.Parse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE").Replace("%", "")));
						if (num > 1)
						{
							this.valueToolStripMenuItem.Text = num.ToString() + "%";
							this.printscale = (double)num;
							this.printscale /= 100.0;
							this.panel1.Size = new Size((int)(382.0 * this.printscale), (int)(440.0 * this.printscale));
							this.panel1.Location = new Point((int)(85.0 + 382.0 * (1.0 - this.printscale) / 2.0), this.panel1.Location.Y);
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			if (this.dtpDeactivate.Value < DateTime.Now)
			{
				XMessageBox.Show(CommonStr.strCreateQRFailed);
				base.Close();
				return;
			}
			this.dtpDeactivateInDB = this.dtpDeactivate.Value;
			this.lblNote.Visible = false;
			this.dtpDeactivate.Value = DateTime.Parse(DateTime.Now.AddYears(1).AddDays(-1.0).ToString("yyyy-MM-dd 23:59:59"));
			wgAppConfig.setDisplayFormatDate(this.dtpActivate, wgTools.DisplayFormat_DateYMD);
			wgAppConfig.setDisplayFormatDate(this.dtpDeactivate, wgTools.DisplayFormat_DateYMD);
			this.dateBeginHMS1.Visible = true;
			this.dateBeginHMS1.CustomFormat = "HH:mm";
			this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS1.CustomFormat = "HH:mm";
			this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
			if (this.dateEndHMS1.Value.ToString("HH:mm").Equals("00:00"))
			{
				this.dateEndHMS1.Value = DateTime.Parse(this.dateEndHMS1.Value.ToString("yyyy-MM-dd 23:59:59"));
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_VALIDTIMEHOURS"))))
			{
				try
				{
					string text = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_VALIDTIMEHOURS"));
					double num2 = 0.0;
					if (double.TryParse(text, out num2))
					{
						this.toolStripValidTimeHours.Text = text;
						DateTime dateTime = DateTime.Now.AddHours(num2);
						this.updateTime(dateTime);
						return;
					}
				}
				catch
				{
				}
			}
			if (!this.bCreating)
			{
				this.createNewQR();
			}
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x002095C0 File Offset: 0x002085C0
		private void dtpDeactivate_ValueChanged(object sender, EventArgs e)
		{
			if (!this.bCreating)
			{
				this.createNewQR();
			}
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x002095D0 File Offset: 0x002085D0
		public static int EncryptSM4_ECB(ref byte[] command, byte[] password)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(64);
			Marshal.Copy(command, 0, intPtr, 16);
			IntPtr intPtr2 = Marshal.AllocHGlobal(16);
			Marshal.Copy(password, 0, intPtr2, 16);
			int num = dfrmCreateQR.ShortEncryptSM4_ECB(intPtr, 64, intPtr2);
			if (num > 0)
			{
				Marshal.Copy(intPtr, command, 0, 16);
			}
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			return num;
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x00209628 File Offset: 0x00208628
		public static int EncryptSM4_ECB_64Byte(ref byte[] command, byte[] password)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(64);
			Marshal.Copy(command, 0, intPtr, 64);
			IntPtr intPtr2 = Marshal.AllocHGlobal(16);
			Marshal.Copy(password, 0, intPtr2, 16);
			int num = dfrmCreateQR.ShortEncryptSM4_ECB(intPtr, 64, intPtr2);
			if (num > 0)
			{
				Marshal.Copy(intPtr, command, 0, 64);
			}
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			return num;
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x00209680 File Offset: 0x00208680
		public static byte GetHex(int val)
		{
			return (byte)(val % 10 + (val - val % 10) / 10 % 10 * 16);
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00209698 File Offset: 0x00208698
		public void InitPrint()
		{
			this._NewBitmap = new Bitmap(this.panel1.Width, this.panel1.Height);
			this.panel1.DrawToBitmap(this._NewBitmap, new Rectangle(0, 0, this._NewBitmap.Width, this._NewBitmap.Height));
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x002096F4 File Offset: 0x002086F4
		private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
		{
			e.Graphics.DrawImage(this._NewBitmap, (this.printDocument1.DefaultPageSettings.PaperSize.Width - this._NewBitmap.Width) / 2, 60, this._NewBitmap.Width, this._NewBitmap.Height);
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x00209750 File Offset: 0x00208750
		private void saveAsFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				Thread.Sleep(300);
				this.panel1.Refresh();
				Bitmap bitmap = new Bitmap(this.panel1.Width, this.panel1.Height);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.CopyFromScreen(this.panel1.PointToScreen(Point.Empty), Point.Empty, this.panel1.Size);
				string text = wgAppConfig.Path4AviJpgDefault() + this.lblUser.Text + ".png";
				FileInfo fileInfo = new FileInfo(text);
				if (fileInfo.Exists)
				{
					try
					{
						fileInfo.Delete();
					}
					catch (Exception)
					{
					}
				}
				bitmap.Save(text);
				graphics.Dispose();
			}
			catch
			{
			}
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00209828 File Offset: 0x00208828
		private void saveMoreFiles()
		{
			try
			{
				string text = wgAppConfig.Path4AviJpgDefault();
				if (!wgAppConfig.DirectoryIsExisted(text))
				{
					Directory.CreateDirectory(text);
				}
				long num = this.cardStart;
				while (num <= this.cardEnd && !this.bStopSave)
				{
					this.consumerCardNO = num;
					this.lblUser.Text = num.ToString();
					this.lblUser.Refresh();
					Application.DoEvents();
					this.createNewQRFile();
					num += 1L;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			try
			{
				string text2 = wgAppConfig.Path4AviJpgDefault();
				Process.Start("explorer.exe", text2);
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x060018F1 RID: 6385
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern int ShortEncryptSM4_ECB(IntPtr command, int cmdLen, IntPtr password);

		// Token: 0x060018F2 RID: 6386 RVA: 0x002098F4 File Offset: 0x002088F4
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.statTimeDate.Text = DateTime.Now.ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x00209920 File Offset: 0x00208920
		private void toolStripValidTimeHours_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.toolStripValidTimeHours.Text))
				{
					wgAppConfig.UpdateKeyVal("KEY_QR_VALIDTIMEHOURS", "");
				}
				else
				{
					double num = 0.0;
					if (double.TryParse(this.toolStripValidTimeHours.Text, out num))
					{
						wgAppConfig.UpdateKeyVal("KEY_QR_VALIDTIMEHOURS", this.toolStripValidTimeHours.Text);
						DateTime dateTime = DateTime.Now.AddHours(num);
						this.updateTime(dateTime);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x002099B0 File Offset: 0x002089B0
		private void updateTime(DateTime dtEnd)
		{
			if (!this.bCreating)
			{
				this.dateEndHMS1.Value = dtEnd;
				this.dtpDeactivate.Value = dtEnd;
				this.createNewQR();
			}
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x002099D8 File Offset: 0x002089D8
		private void valueToolStripMenuItem_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.valueToolStripMenuItem.Text))
				{
					wgAppConfig.UpdateKeyVal("KEY_QR_PRINT_SCALE", "");
					this.printscale = 1.0;
				}
				else if (this.valueToolStripMenuItem.Text.IndexOf("%") > 0)
				{
					wgAppConfig.UpdateKeyVal("KEY_QR_PRINT_SCALE", this.valueToolStripMenuItem.Text);
					if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE"))))
					{
						try
						{
							if (wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE")).IndexOf("%") > 0)
							{
								int num = int.Parse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE").Replace("%", "")));
								if (num > 1)
								{
									this.printscale = (double)num;
									this.printscale /= 100.0;
									this.panel1.Size = new Size((int)(382.0 * this.printscale), (int)(440.0 * this.printscale));
									this.panel1.Location = new Point((int)(85.0 + 382.0 * (1.0 - this.printscale) / 2.0), this.panel1.Location.Y);
									if (!this.bCreating)
									{
										this.createNewQR();
									}
								}
							}
						}
						catch (Exception ex)
						{
							wgTools.WgDebugWrite(ex.ToString(), new object[0]);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x040032C1 RID: 12993
		private Bitmap _NewBitmap;

		// Token: 0x040032C2 RID: 12994
		private bool bCreating;

		// Token: 0x040032C3 RID: 12995
		private bool bStopSave;

		// Token: 0x040032C4 RID: 12996
		private long cardStart;

		// Token: 0x040032C5 RID: 12997
		private DateTime dtpDeactivateInDB;

		// Token: 0x040032C6 RID: 12998
		private long cardEnd = 1L;

		// Token: 0x040032C7 RID: 12999
		public long consumerCardNO;

		// Token: 0x040032C8 RID: 13000
		private double printscale = 1.0;
	}
}
