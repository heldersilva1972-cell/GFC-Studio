using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Reports
{
	// Token: 0x0200035C RID: 860
	public partial class dfrmManualSwipeRecordsEdit : frmN3000
	{
		// Token: 0x06001B97 RID: 7063 RVA: 0x002251D7 File Offset: 0x002241D7
		public dfrmManualSwipeRecordsEdit()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x002251E5 File Offset: 0x002241E5
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x002251F4 File Offset: 0x002241F4
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x00225203 File Offset: 0x00224203
		private void dfrmManualSwipeRecordsEdit_Load(object sender, EventArgs e)
		{
		}
	}
}
