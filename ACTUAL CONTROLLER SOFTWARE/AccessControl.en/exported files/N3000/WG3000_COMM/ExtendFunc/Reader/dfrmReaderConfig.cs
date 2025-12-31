using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc.Reader
{
	// Token: 0x02000321 RID: 801
	public partial class dfrmReaderConfig : frmN3000
	{
		// Token: 0x06001909 RID: 6409 RVA: 0x0020BB12 File Offset: 0x0020AB12
		public dfrmReaderConfig()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0020BB2B File Offset: 0x0020AB2B
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0020BB3C File Offset: 0x0020AB3C
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.readerFormat = "";
			if (this.radioButton1.Checked)
			{
				if (!string.IsNullOrEmpty(this.textBox1.Text) && this.textBox1.Text.Split(new char[] { ',' }).Length >= 3)
				{
					this.readerFormat = this.textBox1.Text;
				}
				else
				{
					this.custom_cardformat_totalbits = (int)this.nudTotal.Value;
					this.custom_cardformat_startloc = (int)this.nudStart.Value;
					this.custom_cardformat_validbits = (int)this.nudValidLength.Value;
					if (this.custom_cardformat_validbits <= 24)
					{
						this.custom_cardformat_validbits += this.custom_cardformat_totalbits;
					}
					if (this.checkBox1.Checked)
					{
						this.custom_cardformat_validbits |= 128;
					}
					this.readerFormat = string.Format("{0},{1},{2}", this.custom_cardformat_totalbits, this.custom_cardformat_startloc, this.custom_cardformat_validbits);
				}
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x0020BC6C File Offset: 0x0020AC6C
		private void dfrmReaderConfig_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.readerFormat))
			{
				string[] array = this.readerFormat.Split(new char[] { ',' });
				bool flag = true;
				if (array.Length != 3)
				{
					flag = false;
				}
				if (flag)
				{
					if (!int.TryParse(array[0].Trim(), out this.custom_cardformat_totalbits))
					{
						flag = false;
					}
					if (!int.TryParse(array[1].Trim(), out this.custom_cardformat_startloc))
					{
						flag = false;
					}
					if (!int.TryParse(array[2].Trim(), out this.custom_cardformat_validbits))
					{
						flag = false;
					}
				}
				if (flag && this.custom_cardformat_totalbits <= 128 && this.custom_cardformat_startloc < this.custom_cardformat_totalbits && this.custom_cardformat_validbits < 2 * this.custom_cardformat_totalbits)
				{
					this.radioButton1.Checked = true;
					this.nudTotal.Value = this.custom_cardformat_totalbits;
					this.nudStart.Value = this.custom_cardformat_startloc;
					this.nudValidLength.Value = this.custom_cardformat_validbits & 127;
					if ((this.custom_cardformat_validbits & 127) > this.custom_cardformat_totalbits)
					{
						this.nudValidLength.Value = (this.custom_cardformat_validbits & 127) - this.custom_cardformat_totalbits;
					}
					if ((this.custom_cardformat_validbits & 128) > 0)
					{
						this.checkBox1.Checked = true;
					}
				}
			}
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0020BDCB File Offset: 0x0020ADCB
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			this.groupBox1.Enabled = this.radioButton1.Checked;
		}

		// Token: 0x0400330A RID: 13066
		private int custom_cardformat_startloc;

		// Token: 0x0400330B RID: 13067
		private int custom_cardformat_totalbits;

		// Token: 0x0400330C RID: 13068
		private int custom_cardformat_validbits;

		// Token: 0x0400330D RID: 13069
		public string readerFormat = "";
	}
}
