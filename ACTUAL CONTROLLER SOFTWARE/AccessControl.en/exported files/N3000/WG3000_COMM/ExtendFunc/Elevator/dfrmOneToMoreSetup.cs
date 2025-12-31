using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Elevator
{
	// Token: 0x0200024B RID: 587
	public partial class dfrmOneToMoreSetup : frmN3000
	{
		// Token: 0x0600127B RID: 4731 RVA: 0x00164AB8 File Offset: 0x00163AB8
		public dfrmOneToMoreSetup()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00164AC6 File Offset: 0x00163AC6
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00164ACE File Offset: 0x00163ACE
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00164ADD File Offset: 0x00163ADD
		private void dfrmOneToMoreSetup_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.funcCtrlShiftQ();
			}
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00164B1C File Offset: 0x00163B1C
		private void dfrmOneToMoreSetup_Load(object sender, EventArgs e)
		{
			this.radioButton0_CheckedChanged(null, null);
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00164B28 File Offset: 0x00163B28
		private void funcCtrlShiftQ()
		{
			base.Size = new Size(Math.Max(554, this.numericUpDown21.Location.X + this.numericUpDown21.Width * 2), base.Height);
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00164B71 File Offset: 0x00163B71
		private void label3_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00164B74 File Offset: 0x00163B74
		private void radioButton0_CheckedChanged(object sender, EventArgs e)
		{
			this.textBox0.Visible = false;
			this.textBox1.Visible = false;
			this.textBox2.Visible = false;
			this.textBox3.Visible = false;
			this.textBox4.Visible = false;
			this.textBox5.Visible = false;
			if (this.radioButton1.Checked)
			{
				this.textBox2.Visible = true;
				this.textBox3.Visible = true;
				return;
			}
			if (this.radioButton2.Checked)
			{
				this.textBox4.Visible = true;
				this.textBox5.Visible = true;
				return;
			}
			this.textBox0.Visible = true;
			this.textBox1.Visible = true;
		}
	}
}
