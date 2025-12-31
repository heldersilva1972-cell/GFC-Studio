using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000234 RID: 564
	public partial class dfrmCommPSet : frmN3000
	{
		// Token: 0x060010E6 RID: 4326 RVA: 0x00135F60 File Offset: 0x00134F60
		public dfrmCommPSet()
		{
			this.InitializeComponent();
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00135F84 File Offset: 0x00134F84
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00135F94 File Offset: 0x00134F94
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this.txtPasswordNew.Text != this.txtPasswordNewConfirm.Text)
			{
				XMessageBox.Show(this, CommonStr.strPwdNotSame, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.txtPasswordPrev.Text != this.txtPasswordPrevConfirm.Text)
			{
				XMessageBox.Show(this, this.label1.Text + "\r\n\r\n" + CommonStr.strPwdNotSame, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.CurrentPwd = this.txtPasswordNew.Text.Trim();
			if (this.bChangedPwd)
			{
				if (string.IsNullOrEmpty(this.txtPasswordPrev.Text.Trim()))
				{
					this.oldPwd = "";
				}
				else
				{
					this.oldPwd = WGPacket.Ept(this.txtPasswordPrev.Text.Trim());
				}
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00136084 File Offset: 0x00135084
		private void dfrmCommPSet_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && !e.Shift && e.KeyValue == 120 && !this.bChangedPwd)
			{
				base.Size = new Size(base.Size.Width, Math.Max(310, this.groupBox1.Location.Y + this.groupBox1.Height * 2));
				this.groupBox1.Visible = true;
				this.bChangedPwd = true;
			}
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0013610A File Offset: 0x0013510A
		private void dfrmCommPSet_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x04001E22 RID: 7714
		public bool bChangedPwd;

		// Token: 0x04001E23 RID: 7715
		public string CurrentPwd = "";

		// Token: 0x04001E24 RID: 7716
		public string oldPwd = "";
	}
}
