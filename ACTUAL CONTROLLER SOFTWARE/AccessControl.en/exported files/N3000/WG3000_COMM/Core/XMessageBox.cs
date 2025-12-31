using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Core
{
	// Token: 0x02000218 RID: 536
	public partial class XMessageBox : Form
	{
		// Token: 0x06000F25 RID: 3877 RVA: 0x0010DFDE File Offset: 0x0010CFDE
		private XMessageBox()
		{
			this.InitializeComponent();
			this.TextBox1.Cursor = Cursors.Default;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0010DFFC File Offset: 0x0010CFFC
		private void SetButtons(XMessageBoxButtons buttons)
		{
			if (buttons == XMessageBoxButtons.OK)
			{
				this.button2.Visible = true;
				this.button2.Enabled = true;
				this.button2.Text = CommonStr.strMsgOK;
				this.button2.DialogResult = DialogResult.OK;
				base.ControlBox = true;
				return;
			}
			if (buttons == XMessageBoxButtons.AbortRetryIgnore)
			{
				this.button1.Visible = true;
				this.button1.Enabled = true;
				this.button1.Text = CommonStr.strMsgAbort;
				this.button1.DialogResult = DialogResult.Abort;
				this.button2.Visible = true;
				this.button2.Enabled = true;
				this.button2.Text = CommonStr.strMsgRetry;
				this.button2.DialogResult = DialogResult.Retry;
				this.button3.Visible = true;
				this.button3.Enabled = true;
				this.button3.Text = CommonStr.strMsgIgnore;
				this.button3.DialogResult = DialogResult.Ignore;
				if (base.Width < 180)
				{
					base.Width = 180;
					return;
				}
			}
			else if (buttons == XMessageBoxButtons.OKCancel)
			{
				this.button1.Visible = true;
				this.button1.Enabled = true;
				this.button1.Text = CommonStr.strMsgOK;
				this.button1.DialogResult = DialogResult.OK;
				this.button2.Visible = true;
				this.button2.Enabled = true;
				this.button2.Text = CommonStr.strMsgCancel;
				this.button2.DialogResult = DialogResult.Cancel;
				base.ControlBox = true;
				if (base.Width < 180)
				{
					base.Width = 180;
					return;
				}
			}
			else if (buttons == XMessageBoxButtons.YesNo)
			{
				this.button1.Visible = true;
				this.button1.Enabled = true;
				this.button1.Text = CommonStr.strMsgYes;
				this.button1.DialogResult = DialogResult.Yes;
				this.button2.Visible = true;
				this.button2.Enabled = true;
				this.button2.Text = CommonStr.strMsgNo;
				this.button2.DialogResult = DialogResult.No;
				if (base.Width < 180)
				{
					base.Width = 180;
					return;
				}
			}
			else if (buttons == XMessageBoxButtons.YesNoCancel)
			{
				this.button1.Visible = true;
				this.button1.Enabled = true;
				this.button1.DialogResult = DialogResult.Yes;
				this.button1.Text = CommonStr.strMsgYes;
				this.button2.Visible = true;
				this.button2.Enabled = true;
				this.button2.DialogResult = DialogResult.No;
				this.button2.Text = CommonStr.strMsgNo;
				this.button3.Visible = true;
				this.button3.Enabled = true;
				this.button3.DialogResult = DialogResult.Cancel;
				this.button3.Text = CommonStr.strMsgCancel;
				if (base.Width < 180)
				{
					base.Width = 180;
				}
			}
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0010E2E0 File Offset: 0x0010D2E0
		private void SetIcon(XMessageBoxIcon icon)
		{
			if (base.Height < 104)
			{
				base.Height = 104;
			}
			if (base.Width <= 600)
			{
				base.Width += 55;
			}
			this.leftPanel.Visible = true;
			if (icon == XMessageBoxIcon.Information)
			{
				this.pictureBox1.BackgroundImage = SystemIcons.Information.ToBitmap();
				return;
			}
			if (icon == XMessageBoxIcon.Warning)
			{
				this.pictureBox1.BackgroundImage = SystemIcons.Warning.ToBitmap();
				return;
			}
			if (icon == XMessageBoxIcon.Error)
			{
				this.pictureBox1.BackgroundImage = SystemIcons.Error.ToBitmap();
				return;
			}
			if (icon == XMessageBoxIcon.Question)
			{
				this.pictureBox1.BackgroundImage = SystemIcons.Question.ToBitmap();
				return;
			}
			if (icon == XMessageBoxIcon.Exclamation)
			{
				this.pictureBox1.BackgroundImage = SystemIcons.Exclamation.ToBitmap();
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0010E3A8 File Offset: 0x0010D3A8
		private static DialogResult show(string text)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, "", xmessageBox.Size, xmessageBox.Font, xmessageBox.BackColor, xmessageBox.ForeColor, false);
				xmessageBox.SetButtons(XMessageBoxButtons.OK);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0010E40C File Offset: 0x0010D40C
		private static DialogResult show(string text, string title)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, xmessageBox.Size, xmessageBox.Font, xmessageBox.BackColor, xmessageBox.ForeColor, false);
				xmessageBox.SetButtons(XMessageBoxButtons.OK);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0010E46C File Offset: 0x0010D46C
		private static DialogResult show(string text, string title, XMessageBoxButtons buttons)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, xmessageBox.Size, xmessageBox.Font, xmessageBox.BackColor, xmessageBox.ForeColor, false);
				xmessageBox.SetButtons(buttons);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0010E4CC File Offset: 0x0010D4CC
		private static DialogResult show(string text, string title, Color backColor, Color foreColor)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, xmessageBox.Size, xmessageBox.Font, backColor, foreColor, false);
				xmessageBox.SetButtons(XMessageBoxButtons.OK);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0010E524 File Offset: 0x0010D524
		private static DialogResult show(string text, string title, XMessageBoxButtons buttons, Size size)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, size, xmessageBox.Font, xmessageBox.BackColor, xmessageBox.ForeColor, true);
				xmessageBox.SetButtons(buttons);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0010E580 File Offset: 0x0010D580
		private static DialogResult show(string text, string title, XMessageBoxButtons buttons, XMessageBoxIcon icon)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, xmessageBox.Size, xmessageBox.Font, xmessageBox.BackColor, xmessageBox.ForeColor, false);
				xmessageBox.SetButtons(buttons);
				xmessageBox.SetIcon(icon);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0010E5E8 File Offset: 0x0010D5E8
		private static DialogResult show(string text, string title, Color backColor, Color foreColor, XMessageBoxButtons buttons)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, xmessageBox.Size, xmessageBox.Font, backColor, foreColor, false);
				xmessageBox.SetButtons(buttons);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0010E640 File Offset: 0x0010D640
		private static DialogResult show(string text, string title, XMessageBoxButtons buttons, Size size, Font font)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, size, font, xmessageBox.BackColor, xmessageBox.ForeColor, true);
				xmessageBox.SetButtons(buttons);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0010E698 File Offset: 0x0010D698
		private static DialogResult show(string text, string title, XMessageBoxButtons buttons, XMessageBoxIcon icon, Size size)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, size, xmessageBox.Font, xmessageBox.BackColor, xmessageBox.ForeColor, true);
				xmessageBox.SetButtons(buttons);
				xmessageBox.SetIcon(icon);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0010E6FC File Offset: 0x0010D6FC
		private static DialogResult show(string text, string title, Color backColor, Color foreColor, XMessageBoxButtons buttons, XMessageBoxIcon icon)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, xmessageBox.Size, xmessageBox.Font, backColor, foreColor, false);
				xmessageBox.SetButtons(buttons);
				xmessageBox.SetIcon(icon);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0010E75C File Offset: 0x0010D75C
		private static DialogResult show(string text, string title, XMessageBoxButtons buttons, XMessageBoxIcon icon, Size size, Font font)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, size, font, xmessageBox.BackColor, xmessageBox.ForeColor, true);
				xmessageBox.SetButtons(buttons);
				xmessageBox.SetIcon(icon);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0010E7BC File Offset: 0x0010D7BC
		private static DialogResult show(string text, string title, Color backColor, Color foreColor, XMessageBoxButtons buttons, Size size, Font font)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, size, font, backColor, foreColor, true);
				xmessageBox.SetButtons(buttons);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x0010E80C File Offset: 0x0010D80C
		private static DialogResult show(string text, string title, Color backColor, Color foreColor, XMessageBoxButtons buttons, XMessageBoxIcon icon, Size size, Font font)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				xmessageBox.XmessageBox(text, title, size, font, backColor, foreColor, true);
				xmessageBox.SetButtons(buttons);
				xmessageBox.SetIcon(icon);
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0010E864 File Offset: 0x0010D864
		public static DialogResult Show(string text)
		{
			return XMessageBox.Show(null, text, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x0010E875 File Offset: 0x0010D875
		public static DialogResult Show(string text, string title, MessageBoxButtons buttons)
		{
			return XMessageBox.Show(null, text, title, buttons, MessageBoxIcon.Asterisk);
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0010E882 File Offset: 0x0010D882
		public static DialogResult Show(string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return XMessageBox.Show(null, text, title, buttons, icon);
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0010E88E File Offset: 0x0010D88E
		public static DialogResult Show(IWin32Window owner, string text, string title, MessageBoxButtons buttons)
		{
			return XMessageBox.Show(owner, text, title, buttons, MessageBoxIcon.Asterisk);
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0010E89C File Offset: 0x0010D89C
		public static DialogResult Show(IWin32Window owner, string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			DialogResult dialogResult;
			using (XMessageBox xmessageBox = new XMessageBox())
			{
				Color keyColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor5", "147, 150, 177");
				Color white = Color.White;
				xmessageBox.XmessageBox(text, title, xmessageBox.Size, xmessageBox.Font, keyColor, white, false);
				if (buttons == MessageBoxButtons.OK)
				{
					xmessageBox.SetButtons(XMessageBoxButtons.OK);
				}
				if (buttons == MessageBoxButtons.OKCancel)
				{
					xmessageBox.SetButtons(XMessageBoxButtons.OKCancel);
				}
				if (icon == MessageBoxIcon.Exclamation)
				{
					xmessageBox.SetIcon(XMessageBoxIcon.Exclamation);
				}
				if (icon == MessageBoxIcon.Asterisk)
				{
					xmessageBox.SetIcon(XMessageBoxIcon.Information);
				}
				if (icon == MessageBoxIcon.Hand)
				{
					xmessageBox.SetIcon(XMessageBoxIcon.Error);
				}
				if (icon == MessageBoxIcon.Exclamation)
				{
					xmessageBox.SetIcon(XMessageBoxIcon.Warning);
				}
				int num = xmessageBox.leftPanel.Location.Y + xmessageBox.pictureBox1.Location.Y + xmessageBox.pictureBox1.Size.Height + 10 - xmessageBox.button1.Location.Y;
				if (num > 0)
				{
					xmessageBox.Size = new Size(xmessageBox.Size.Width, xmessageBox.Size.Height + num);
				}
				int num2 = 0;
				int num3 = 0;
				if (xmessageBox.button1.Enabled)
				{
					num2 += xmessageBox.button1.Width;
					num3++;
				}
				if (xmessageBox.button2.Enabled)
				{
					num2 += xmessageBox.button2.Width;
					num3++;
				}
				if (xmessageBox.button3.Enabled)
				{
					num2 += xmessageBox.button3.Width;
					num3++;
				}
				if (num3 > 0)
				{
					num = xmessageBox.Size.Width - num2 - (num3 + 1) * 3;
					if (num < 0)
					{
						xmessageBox.Size = new Size(xmessageBox.Size.Width - num, xmessageBox.Size.Height);
					}
					num = xmessageBox.Size.Width - num2;
					int num4 = 0;
					if (xmessageBox.button1.Enabled)
					{
						xmessageBox.button1.Location = new Point(num4 + num / (num3 + 1), xmessageBox.button1.Location.Y);
						num4 = xmessageBox.button1.Location.X + xmessageBox.button1.Size.Width;
					}
					if (xmessageBox.button2.Enabled)
					{
						xmessageBox.button2.Location = new Point(num4 + num / (num3 + 1), xmessageBox.button2.Location.Y);
						num4 = xmessageBox.button2.Location.X + xmessageBox.button2.Size.Width;
					}
					if (xmessageBox.button3.Enabled)
					{
						xmessageBox.button3.Location = new Point(num4 + num / (num3 + 1), xmessageBox.button3.Location.Y);
					}
				}
				dialogResult = xmessageBox.ShowDialog();
			}
			return dialogResult;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0010EBB8 File Offset: 0x0010DBB8
		private void XmessageBox(string text, string title, Size size, Font font, Color backColor, Color foreColor, bool customSize)
		{
			if (!string.IsNullOrEmpty(title))
			{
				this.Text = title;
			}
			this.TextBox1.Text = text;
			this.Font = font;
			this.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.ForeColor = foreColor;
			Graphics graphics = base.CreateGraphics();
			SizeF sizeF;
			if (text.Length > 1000)
			{
				sizeF = graphics.MeasureString(text.Substring(0, 1000), font);
			}
			else
			{
				sizeF = graphics.MeasureString(text, font);
			}
			graphics.Dispose();
			int num;
			if (sizeF.Width <= 600f)
			{
				num = (int)sizeF.Width + 15;
			}
			else
			{
				num = 600;
			}
			num = num * 5 / 4;
			int num2 = this.TextBox1.LinesCount() * this.TextBox1.Font.Height + 40 + 60;
			if (num2 > 600)
			{
				num2 = 600;
			}
			if (customSize)
			{
				base.Size = size;
				return;
			}
			base.Size = new Size(num, num2);
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0010ECB6 File Offset: 0x0010DCB6
		private void XMessageBox_BackColorChanged(object sender, EventArgs e)
		{
			this.TextBox1.BackColor = this.BackColor;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0010ECC9 File Offset: 0x0010DCC9
		private void XMessageBox_ForeColorChanged(object sender, EventArgs e)
		{
			this.TextBox1.ForeColor = this.ForeColor;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0010ECDC File Offset: 0x0010DCDC
		private void XMessageBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 27)
			{
				base.DialogResult = DialogResult.Cancel;
				base.Close();
			}
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0010ECF8 File Offset: 0x0010DCF8
		private void XMessageBox_Load(object sender, EventArgs e)
		{
			Icon icon = base.Icon;
			wgAppConfig.GetAppIcon(ref icon);
			base.Icon = icon;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0010ED1A File Offset: 0x0010DD1A
		private void XMessageBox_Resize(object sender, EventArgs e)
		{
		}

		// Token: 0x04001C3C RID: 7228
		private const int maxHeight = 600;

		// Token: 0x04001C3D RID: 7229
		private const int maxTextLength = 1000;

		// Token: 0x04001C3E RID: 7230
		private const int maxWidth = 600;

		// Token: 0x04001C3F RID: 7231
		private const int minWidth = 180;
	}
}
