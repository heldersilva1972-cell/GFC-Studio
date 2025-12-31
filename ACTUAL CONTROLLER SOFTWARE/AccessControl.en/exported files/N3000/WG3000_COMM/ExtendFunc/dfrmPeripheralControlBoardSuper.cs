using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000246 RID: 582
	public partial class dfrmPeripheralControlBoardSuper : frmN3000
	{
		// Token: 0x06001215 RID: 4629 RVA: 0x0015890A File Offset: 0x0015790A
		public dfrmPeripheralControlBoardSuper()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00158918 File Offset: 0x00157918
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00158928 File Offset: 0x00157928
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			this.extControl = 0;
			if (this.radioButton14.Checked)
			{
				this.extControl = 1;
			}
			if (this.radioButton15.Checked)
			{
				this.extControl = 2;
			}
			if (this.radioButton16.Checked)
			{
				this.extControl = 3;
			}
			if (this.radioButton17.Checked)
			{
				this.extControl = (this.chkForceOutputTimeRemains.Checked ? 6 : 4);
			}
			if (this.radioButton18.Checked)
			{
				this.extControl = (this.chkForceOutputTimeRemains.Checked ? 7 : 5);
			}
			this.ext_warnSignalEnabled2 = 0;
			if (this.checkBox76.Checked)
			{
				this.ext_warnSignalEnabled2 |= 1;
			}
			if (this.checkBox77.Checked)
			{
				this.ext_warnSignalEnabled2 |= 2;
			}
			if (this.checkBox78.Checked)
			{
				this.ext_warnSignalEnabled2 |= 4;
			}
			if (this.checkBox79.Checked)
			{
				this.ext_warnSignalEnabled2 |= 8;
			}
			if (this.checkBox80.Checked)
			{
				this.ext_warnSignalEnabled2 |= 16;
			}
			if (this.checkBox81.Checked)
			{
				this.ext_warnSignalEnabled2 |= 32;
			}
			if (this.checkBox82.Checked)
			{
				this.ext_warnSignalEnabled2 |= 64;
			}
			if (this.checkBox83.Checked)
			{
				this.ext_warnSignalEnabled2 |= 128;
			}
			base.Close();
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00158AB3 File Offset: 0x00157AB3
		private void dfrmPeripheralControlBoardSuper_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				this.bVisibleForce = true;
				this.radioButton14_CheckedChanged(null, null);
			}
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x00158AE0 File Offset: 0x00157AE0
		private void dfrmPeripheralControlBoardSuper_Load(object sender, EventArgs e)
		{
			this.radioButton14.Checked = this.extControl == 1 || this.extControl == 0;
			this.radioButton15.Checked = this.extControl == 2;
			this.radioButton16.Checked = this.extControl == 3;
			this.radioButton17.Checked = this.extControl == 4 || this.extControl == 6;
			this.radioButton18.Checked = this.extControl == 5 || this.extControl == 7;
			this.bVisibleForce = this.extControl == 7 || this.extControl == 6;
			this.chkForceOutputTimeRemains.Visible = this.bVisibleForce;
			this.chkForceOutputTimeRemains.Checked = this.bVisibleForce;
			this.checkBox76.Checked = (this.ext_warnSignalEnabled2 & 1) > 0;
			this.checkBox77.Checked = (this.ext_warnSignalEnabled2 & 2) > 0;
			this.checkBox78.Checked = (this.ext_warnSignalEnabled2 & 4) > 0;
			this.checkBox79.Checked = (this.ext_warnSignalEnabled2 & 8) > 0;
			this.checkBox80.Checked = (this.ext_warnSignalEnabled2 & 16) > 0;
			this.checkBox81.Checked = (this.ext_warnSignalEnabled2 & 32) > 0;
			this.checkBox82.Checked = (this.ext_warnSignalEnabled2 & 64) > 0;
			this.checkBox83.Checked = (this.ext_warnSignalEnabled2 & 128) > 0;
			if (this.radioButton17.Checked || this.radioButton18.Checked)
			{
				this.diplayChkbox();
				return;
			}
			this.hideChkbox();
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00158C90 File Offset: 0x00157C90
		private void diplayChkbox()
		{
			this.checkBox76.Visible = true;
			this.checkBox77.Visible = true;
			this.checkBox78.Visible = true;
			this.checkBox79.Visible = true;
			this.checkBox80.Visible = true;
			this.checkBox81.Visible = true;
			this.checkBox82.Visible = true;
			this.checkBox83.Visible = true;
			this.chkForceOutputTimeRemains.Visible = this.bVisibleForce;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00158D10 File Offset: 0x00157D10
		private void hideChkbox()
		{
			this.checkBox76.Visible = false;
			this.checkBox77.Visible = false;
			this.checkBox78.Visible = false;
			this.checkBox79.Visible = false;
			this.checkBox80.Visible = false;
			this.checkBox81.Visible = false;
			this.checkBox82.Visible = false;
			this.checkBox83.Visible = false;
			this.chkForceOutputTimeRemains.Visible = false;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00158D89 File Offset: 0x00157D89
		private void radioButton14_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioButton17.Checked || this.radioButton18.Checked)
			{
				this.diplayChkbox();
				return;
			}
			this.hideChkbox();
		}

		// Token: 0x040020D2 RID: 8402
		private bool bVisibleForce;

		// Token: 0x040020D3 RID: 8403
		public int ext_warnSignalEnabled2;

		// Token: 0x040020D4 RID: 8404
		public int extControl;
	}
}
