using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000015 RID: 21
	public partial class dfrmFeedback : frmN3000
	{
		// Token: 0x06000105 RID: 261 RVA: 0x0002647C File Offset: 0x0002547C
		public dfrmFeedback()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000264A0 File Offset: 0x000254A0
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.mailSubject = this.txtTitle.Text.Trim();
			this.strSysInfo = this.txtContents.Text;
			if (!string.IsNullOrEmpty(this.strSysInfo) || !string.IsNullOrEmpty(this.mailSubject))
			{
				if (string.IsNullOrEmpty(this.strSysInfo))
				{
					this.strSysInfo = this.mailSubject;
				}
				this.mailSubject = "意见反馈(Feedback)--" + this.mailSubject;
				new Thread(new ThreadStart(this.sendM)).Start();
				this.sendRegisterInfo();
			}
			XMessageBox.Show(this.Text + " " + CommonStr.strSuccessfully + ".");
			base.Close();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0002655F File Offset: 0x0002555F
		private void sendM()
		{
			wgMail.sendMail2018(this.strSysInfo, this.mailSubject);
			wgMail.sendMail2014(this.strSysInfo, this.mailSubject);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00026585 File Offset: 0x00025585
		private void sendRegisterInfo()
		{
			new Thread(new ThreadStart(wgMail.sendMailOnce)).Start();
		}

		// Token: 0x040002B5 RID: 693
		private string mailSubject = "";

		// Token: 0x040002B6 RID: 694
		private string strSysInfo = "";
	}
}
