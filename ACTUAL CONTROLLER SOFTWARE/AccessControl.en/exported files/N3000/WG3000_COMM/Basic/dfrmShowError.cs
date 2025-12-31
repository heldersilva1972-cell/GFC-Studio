using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200002C RID: 44
	public partial class dfrmShowError : frmN3000
	{
		// Token: 0x0600031F RID: 799 RVA: 0x0005D49A File Offset: 0x0005C49A
		public dfrmShowError()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0005D4B4 File Offset: 0x0005C4B4
		private void btnCopyDetail_Click(object sender, EventArgs e)
		{
			try
			{
				this.txtErrorDetail.Text = this.errInfo;
				string text = this.txtErrorDetail.Text;
				try
				{
					Clipboard.SetText(text);
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					wgAppConfig.wgLogWithoutDB(text, EventLogEntryType.Information, null);
				}
				this.btnDetail.Enabled = false;
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0005D53C File Offset: 0x0005C53C
		private void btnDetail_Click(object sender, EventArgs e)
		{
			base.Height = 320;
			this.btnDetail.Visible = false;
			this.btnCopyDetail.Visible = true;
			try
			{
				this.txtErrorDetail.Visible = true;
				this.txtErrorDetail.Text = this.errInfo;
				string text = this.txtErrorDetail.Text;
				try
				{
					Clipboard.SetText(text);
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					wgAppConfig.wgLogWithoutDB(text, EventLogEntryType.Information, null);
				}
				this.btnDetail.Enabled = false;
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0005D5F4 File Offset: 0x0005C5F4
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0005D603 File Offset: 0x0005C603
		private void dfrmShowError_Load(object sender, EventArgs e)
		{
			this.errInfo != "";
		}

		// Token: 0x0400060B RID: 1547
		public string errInfo = "";
	}
}
