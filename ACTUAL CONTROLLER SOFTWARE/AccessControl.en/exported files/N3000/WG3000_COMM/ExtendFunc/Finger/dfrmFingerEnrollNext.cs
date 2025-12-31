using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002E1 RID: 737
	public partial class dfrmFingerEnrollNext : frmN3000
	{
		// Token: 0x060014E0 RID: 5344 RVA: 0x0019CE5A File Offset: 0x0019BE5A
		public dfrmFingerEnrollNext()
		{
			this.InitializeComponent();
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0019CE68 File Offset: 0x0019BE68
		private void btnAddNext_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Retry;
			base.Close();
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0019CE77 File Offset: 0x0019BE77
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}
	}
}
