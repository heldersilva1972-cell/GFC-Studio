using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;

namespace WG3000_COMM.Core
{
	// Token: 0x02000002 RID: 2
	public partial class frmN3000 : Form
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00001050
		public frmN3000()
		{
			this.InitializeComponent();
			this.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000207A File Offset: 0x0000107A
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002090 File Offset: 0x00001090
		private void dfrm_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.bFindActive)
			{
				try
				{
					if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
					{
						if (this.dfrmFind1 == null)
						{
							this.dfrmFind1 = new dfrmFind();
						}
						this.dfrmFind1.Show();
						this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
					}
				}
				catch (Exception ex)
				{
					wgTools.WriteLine(ex.ToString());
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002114 File Offset: 0x00001114
		private void frmN3000_Load(object sender, EventArgs e)
		{
			Icon icon = base.Icon;
			wgAppConfig.GetAppIcon(ref icon);
			base.Icon = icon;
			if (!base.IsMdiContainer)
			{
				wgAppRunInfo.ClearAllDisplayedInfo();
			}
		}

		// Token: 0x04000001 RID: 1
		public bool bFindActive = true;

		// Token: 0x04000002 RID: 2
		public dfrmFind dfrmFind1;
	}
}
