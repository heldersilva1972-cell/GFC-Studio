using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000033 RID: 51
	public partial class dfrmUploadOption : frmN3000
	{
		// Token: 0x0600037C RID: 892 RVA: 0x00065C00 File Offset: 0x00064C00
		public dfrmUploadOption()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00065C0E File Offset: 0x00064C0E
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00065C18 File Offset: 0x00064C18
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.checkVal = 0;
			if (this.chkBasicConfiguration.Checked)
			{
				this.checkVal++;
			}
			if (this.chkAccessPrivilege.Checked)
			{
				this.checkVal += 2;
			}
			base.Close();
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00065C68 File Offset: 0x00064C68
		private void dfrmUploadOption_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x040006B6 RID: 1718
		public int checkVal;
	}
}
