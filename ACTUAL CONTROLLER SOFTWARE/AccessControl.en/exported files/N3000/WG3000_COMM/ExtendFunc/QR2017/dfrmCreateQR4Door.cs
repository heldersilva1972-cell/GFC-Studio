using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace WG3000_COMM.ExtendFunc.QR2017
{
	// Token: 0x02000320 RID: 800
	public partial class dfrmCreateQR4Door : frmN3000
	{
		// Token: 0x060018F8 RID: 6392 RVA: 0x0020AC5A File Offset: 0x00209C5A
		public dfrmCreateQR4Door()
		{
			this.InitializeComponent();
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x0020AC95 File Offset: 0x00209C95
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x0020ACA0 File Offset: 0x00209CA0
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

		// Token: 0x060018FB RID: 6395 RVA: 0x0020AD20 File Offset: 0x00209D20
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

		// Token: 0x060018FC RID: 6396 RVA: 0x0020ADC0 File Offset: 0x00209DC0
		private void createNewQR()
		{
			if (!this.bCreating)
			{
				try
				{
					this.bCreating = true;
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
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i] = 0;
					}
					array2[0] = (byte)(this.controllerSN & 255L);
					array2[1] = (byte)((this.controllerSN >> 8) & 255L);
					array2[2] = (byte)((this.controllerSN >> 16) & 255L);
					array2[3] = (byte)((this.controllerSN >> 24) & 255L);
					array2[4] = (byte)(this.doorNO & 255L);
					for (int j = 0; j < 5; j++)
					{
						array2[5 + j] = array2[j];
					}
					dfrmCreateQR4Door.EncryptSM4_ECB(ref array2, array);
					string text = BitConverter.ToString(array2).Replace("-", "") + ":" + this.doorName;
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
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				wgAppConfig.wgLogWithoutDB(DateTime.Now.ToString());
				this.bCreating = false;
			}
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x0020AF8C File Offset: 0x00209F8C
		private void dfrmCreateQR_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0020AF90 File Offset: 0x00209F90
		private void dfrmCreateQR_Load(object sender, EventArgs e)
		{
			this.lblUser.Text = this.doorName;
			this.lblInfo.Text = "";
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE_4DOOR"))))
			{
				try
				{
					if (wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE_4DOOR")).IndexOf("%") > 0)
					{
						int num = int.Parse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE_4DOOR").Replace("%", "")));
						if (num > 1)
						{
							this.valueToolStripMenuItem.Text = num.ToString() + "%";
							this.printscale = (double)num;
							this.printscale /= 100.0;
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			if (!this.bCreating)
			{
				this.createNewQR();
			}
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x0020B08C File Offset: 0x0020A08C
		public static int EncryptSM4_ECB(ref byte[] command, byte[] password)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(64);
			Marshal.Copy(command, 0, intPtr, 16);
			IntPtr intPtr2 = Marshal.AllocHGlobal(16);
			Marshal.Copy(password, 0, intPtr2, 16);
			int num = dfrmCreateQR4Door.ShortEncryptSM4_ECB(intPtr, 64, intPtr2);
			if (num > 0)
			{
				Marshal.Copy(intPtr, command, 0, 16);
			}
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			return num;
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x0020B0E4 File Offset: 0x0020A0E4
		public static byte GetHex(int val)
		{
			return (byte)(val % 10 + (val - val % 10) / 10 % 10 * 16);
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0020B0FC File Offset: 0x0020A0FC
		public void InitPrint()
		{
			this._NewBitmap = new Bitmap(this.panel1.Width, this.panel1.Height);
			this.panel1.DrawToBitmap(this._NewBitmap, new Rectangle(0, 0, this._NewBitmap.Width, this._NewBitmap.Height));
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0020B158 File Offset: 0x0020A158
		private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
		{
			Image thumbnailImage = this._NewBitmap.GetThumbnailImage((int)((double)this._NewBitmap.Width * this.printscale), (int)((double)this._NewBitmap.Height * this.printscale), null, IntPtr.Zero);
			e.Graphics.DrawImage(thumbnailImage, (this.printDocument1.DefaultPageSettings.PaperSize.Width - thumbnailImage.Width) / 2, 60, thumbnailImage.Width, thumbnailImage.Height);
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0020B1D8 File Offset: 0x0020A1D8
		private void saveAsFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
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

		// Token: 0x06001904 RID: 6404
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern int ShortEncryptSM4_ECB(IntPtr command, int cmdLen, IntPtr password);

		// Token: 0x06001905 RID: 6405 RVA: 0x0020B298 File Offset: 0x0020A298
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.statTimeDate.Text = DateTime.Now.ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x0020B2C4 File Offset: 0x0020A2C4
		private void valueToolStripMenuItem_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.valueToolStripMenuItem.Text))
				{
					wgAppConfig.UpdateKeyVal("KEY_QR_PRINT_SCALE_4DOOR", "");
					this.printscale = 1.0;
				}
				else if (this.valueToolStripMenuItem.Text.IndexOf("%") > 0)
				{
					wgAppConfig.UpdateKeyVal("KEY_QR_PRINT_SCALE_4DOOR", this.valueToolStripMenuItem.Text);
					if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE_4DOOR"))))
					{
						try
						{
							if (wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE_4DOOR")).IndexOf("%") > 0)
							{
								int num = int.Parse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_QR_PRINT_SCALE_4DOOR").Replace("%", "")));
								if (num > 1)
								{
									this.printscale = (double)num;
									this.printscale /= 100.0;
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

		// Token: 0x040032F0 RID: 13040
		private Bitmap _NewBitmap;

		// Token: 0x040032F1 RID: 13041
		private bool bCreating;

		// Token: 0x040032F2 RID: 13042
		public long consumerCardNO;

		// Token: 0x040032F3 RID: 13043
		public long controllerSN;

		// Token: 0x040032F4 RID: 13044
		public string doorInfo = "";

		// Token: 0x040032F5 RID: 13045
		public string doorName = "";

		// Token: 0x040032F6 RID: 13046
		public long doorNO = 1L;

		// Token: 0x040032F7 RID: 13047
		private double printscale = 1.0;
	}
}
