using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002F4 RID: 756
	public partial class dfrmMealGroup : frmN3000
	{
		// Token: 0x06001603 RID: 5635 RVA: 0x001BCAB8 File Offset: 0x001BBAB8
		public dfrmMealGroup()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x001BCB18 File Offset: 0x001BBB18
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x001BCB28 File Offset: 0x001BBB28
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

		// Token: 0x06001606 RID: 5638 RVA: 0x001BCBA0 File Offset: 0x001BBBA0
		private void dfrmMealGroup_Load(object sender, EventArgs e)
		{
			this.nudMorning.Value = this.morning;
			this.nudLunch.Value = this.lunch;
			this.nudEvening.Value = this.evening;
			this.nudOther.Value = this.other;
			this.chkActive.Checked = this.enable == 1;
		}

		// Token: 0x04002D9D RID: 11677
		public int enable = 1;

		// Token: 0x04002D9E RID: 11678
		public decimal evening = 0.0m;

		// Token: 0x04002D9F RID: 11679
		public decimal lunch = 0.0m;

		// Token: 0x04002DA0 RID: 11680
		public decimal morning = 0.0m;

		// Token: 0x04002DA1 RID: 11681
		public decimal other = 0.0m;
	}
}
