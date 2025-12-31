using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Core
{
	// Token: 0x020001C7 RID: 455
	public partial class dfrmSelectColumnsShow : frmN3000
	{
		// Token: 0x060009A9 RID: 2473 RVA: 0x000DF505 File Offset: 0x000DE505
		public dfrmSelectColumnsShow()
		{
			this.InitializeComponent();
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x000DF513 File Offset: 0x000DE513
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x000DF522 File Offset: 0x000DE522
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}
	}
}
