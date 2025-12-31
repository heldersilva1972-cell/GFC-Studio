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
	// Token: 0x02000249 RID: 585
	public partial class dfrmFaceDeviceTypeSetup : frmN3000
	{
		// Token: 0x0600125D RID: 4701 RVA: 0x00161CE7 File Offset: 0x00160CE7
		public dfrmFaceDeviceTypeSetup()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00161CF5 File Offset: 0x00160CF5
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00161D00 File Offset: 0x00160D00
		private void btnOK_Click(object sender, EventArgs e)
		{
			int num;
			string text;
			if (this.radioButton3.Checked)
			{
				num = 3;
				text = string.Format("{0},{1}", this.textBox6.Text, this.textBox7.Text);
			}
			else if (this.radioButton2.Checked)
			{
				num = 2;
				text = string.Format("{0},{1}", this.textBox4.Text, this.textBox5.Text);
			}
			else if (this.radioButton1.Checked)
			{
				num = 1;
				text = string.Format("{0},{1}", this.textBox2.Text, this.textBox3.Text);
			}
			else
			{
				num = 0;
				text = string.Format("{0},{1}", this.textBox0.Text, this.textBox1.Text);
			}
			wgAppConfig.setSystemParamValueWithNotes(209, "Face Device Type", num.ToString(), text);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00161DF6 File Offset: 0x00160DF6
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

		// Token: 0x06001261 RID: 4705 RVA: 0x00161E38 File Offset: 0x00160E38
		private void dfrmOneToMoreSetup_Load(object sender, EventArgs e)
		{
			this.radioButton0_CheckedChanged(null, null);
			this.textBox2.Location = this.textBox0.Location;
			this.textBox4.Location = this.textBox0.Location;
			this.textBox6.Location = this.textBox0.Location;
			this.textBox3.Location = this.textBox1.Location;
			this.textBox5.Location = this.textBox1.Location;
			this.textBox7.Location = this.textBox1.Location;
			switch (int.Parse("0" + wgAppConfig.getSystemParamByNO(209)))
			{
			case 0:
				this.radioButton0.Checked = true;
				return;
			case 1:
			{
				this.radioButton1.Checked = true;
				string systemParamNotes = wgAppConfig.getSystemParamNotes(209);
				this.textBox2.Text = systemParamNotes.Substring(0, systemParamNotes.IndexOf(","));
				this.textBox3.Text = systemParamNotes.Substring(systemParamNotes.IndexOf(",") + 1);
				return;
			}
			case 2:
			{
				this.radioButton2.Checked = true;
				string systemParamNotes2 = wgAppConfig.getSystemParamNotes(209);
				this.textBox4.Text = systemParamNotes2.Substring(0, systemParamNotes2.IndexOf(","));
				this.textBox5.Text = systemParamNotes2.Substring(systemParamNotes2.IndexOf(",") + 1);
				return;
			}
			case 3:
			{
				this.radioButton3.Checked = true;
				string systemParamNotes3 = wgAppConfig.getSystemParamNotes(209);
				this.textBox6.Text = systemParamNotes3.Substring(0, systemParamNotes3.IndexOf(","));
				this.textBox7.Text = systemParamNotes3.Substring(systemParamNotes3.IndexOf(",") + 1);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00162008 File Offset: 0x00161008
		private void funcCtrlShiftQ()
		{
			this.textBox0.ReadOnly = false;
			this.textBox2.ReadOnly = false;
			this.textBox4.ReadOnly = false;
			this.textBox0.Enabled = true;
			this.textBox2.Enabled = true;
			this.textBox4.Enabled = true;
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0016205D File Offset: 0x0016105D
		private void label3_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00162060 File Offset: 0x00161060
		private void radioButton0_CheckedChanged(object sender, EventArgs e)
		{
			this.textBox0.Visible = false;
			this.textBox1.Visible = false;
			this.textBox2.Visible = false;
			this.textBox3.Visible = false;
			this.textBox4.Visible = false;
			this.textBox5.Visible = false;
			this.textBox6.Visible = false;
			this.textBox7.Visible = false;
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
			if (this.radioButton3.Checked)
			{
				this.textBox6.Visible = true;
				this.textBox7.Visible = true;
				return;
			}
			this.textBox0.Visible = true;
			this.textBox1.Visible = true;
		}
	}
}
