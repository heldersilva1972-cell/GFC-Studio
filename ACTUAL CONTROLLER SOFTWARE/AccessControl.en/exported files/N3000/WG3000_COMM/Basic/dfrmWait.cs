using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200003C RID: 60
	public partial class dfrmWait : Form
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x00074062 File Offset: 0x00073062
		public dfrmWait()
		{
			this.InitializeComponent();
			this.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00074090 File Offset: 0x00073090
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Hide();
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00074098 File Offset: 0x00073098
		private void dfrmWait_Load(object sender, EventArgs e)
		{
			Icon icon = base.Icon;
			wgAppConfig.GetAppIcon(ref icon);
			base.Icon = icon;
			this.Refresh();
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x000740CC File Offset: 0x000730CC
		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				if (!base.Visible)
				{
					this.startTime = DateTime.Now;
				}
				TimeSpan timeSpan = DateTime.Now.Subtract(this.startTime);
				string text = string.Concat(new object[] { timeSpan.Hours, ":", timeSpan.Minutes, ":", timeSpan.Seconds });
				this.lblRuntime.Text = text;
				this.lblRuntime.Visible = true;
			}
			catch
			{
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00074178 File Offset: 0x00073178
		private void xToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.Hide();
		}

		// Token: 0x040007E1 RID: 2017
		private DateTime startTime = DateTime.Now;
	}
}
