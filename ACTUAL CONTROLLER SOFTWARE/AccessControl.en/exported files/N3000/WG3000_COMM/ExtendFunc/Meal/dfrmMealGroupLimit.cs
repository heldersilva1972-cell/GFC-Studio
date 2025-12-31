using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002F6 RID: 758
	public partial class dfrmMealGroupLimit : frmN3000
	{
		// Token: 0x06001611 RID: 5649 RVA: 0x001BE170 File Offset: 0x001BD170
		public dfrmMealGroupLimit()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x001BE1D0 File Offset: 0x001BD1D0
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x001BE1E0 File Offset: 0x001BD1E0
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.morning = this.nudMorning.Value;
			this.lunch = this.nudLunch.Value;
			this.evening = this.nudEvening.Value;
			this.other = this.nudOther.Value;
			this.enable = (this.chkActive.Checked ? 1 : 0);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x001BE258 File Offset: 0x001BD258
		private void dfrmMealGroup_Load(object sender, EventArgs e)
		{
			this.nudMorning.Value = this.morning;
			this.nudLunch.Value = this.lunch;
			this.nudEvening.Value = this.evening;
			this.nudOther.Value = this.other;
			this.chkActive.Checked = this.enable == 1;
		}

		// Token: 0x04002DBF RID: 11711
		public int enable = 1;

		// Token: 0x04002DC0 RID: 11712
		public decimal evening = 0.0m;

		// Token: 0x04002DC1 RID: 11713
		public decimal lunch = 0.0m;

		// Token: 0x04002DC2 RID: 11714
		public decimal morning = 0.0m;

		// Token: 0x04002DC3 RID: 11715
		public decimal other = 0.0m;
	}
}
