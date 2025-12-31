using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200000B RID: 11
	public partial class dfrmControllerDoorControlSet : frmN3000
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00018911 File Offset: 0x00017911
		public dfrmControllerDoorControlSet()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0001892D File Offset: 0x0001792D
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0001893C File Offset: 0x0001793C
		private void btnOnline_Click(object sender, EventArgs e)
		{
			if (sender == this.btnOnline)
			{
				this.doorControl = 3;
			}
			else if (sender == this.btnNormalClose)
			{
				this.doorControl = 2;
			}
			else
			{
				if (sender != this.btnNormalOpen)
				{
					this.btnCancel.PerformClick();
					return;
				}
				this.doorControl = 1;
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00018998 File Offset: 0x00017998
		private void btnUpdateDoorDelay_Click(object sender, EventArgs e)
		{
			this.doorOpenDelay = (int)this.nudDoorDelay1D.Value;
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0400017D RID: 381
		public int doorControl = -1;

		// Token: 0x0400017E RID: 382
		public int doorOpenDelay = -1;
	}
}
