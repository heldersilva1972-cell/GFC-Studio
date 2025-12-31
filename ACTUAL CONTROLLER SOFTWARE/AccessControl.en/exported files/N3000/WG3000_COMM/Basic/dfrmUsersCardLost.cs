using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200003A RID: 58
	public partial class dfrmUsersCardLost : frmN3000
	{
		// Token: 0x06000401 RID: 1025 RVA: 0x00071D03 File Offset: 0x00070D03
		public dfrmUsersCardLost()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00071D1C File Offset: 0x00070D1C
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00071D2C File Offset: 0x00070D2C
		private void btnOK_Click(object sender, EventArgs e)
		{
			icConsumer icConsumer = new icConsumer();
			if (!string.IsNullOrEmpty(this.txtf_CardNONew.Text) && icConsumer.isExisted(long.Parse(this.txtf_CardNONew.Text)))
			{
				XMessageBox.Show(this, CommonStr.strCardAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			icConsumerShare.setUpdateLog();
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00071D90 File Offset: 0x00070D90
		private void dfrmUsersCardLost_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.IsActivateCard19)
			{
				this.txtf_CardNO.Mask = "9999999999999999999";
				this.txtf_CardNONew.Mask = "9999999999999999999";
			}
			else
			{
				this.txtf_CardNO.Mask = "9999999999";
				this.txtf_CardNONew.Mask = "9999999999";
			}
			if (wgTools.gbHideCardNO)
			{
				this.txtf_CardNO.PasswordChar = '#';
				this.txtf_CardNONew.PasswordChar = '#';
			}
			if (!(wgAppConfig.getSystemParamByNO(205) == "0.0.0.0") && !string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(205)) && icOperator.OperatorID != 1)
			{
				this.txtf_CardNONew.Enabled = false;
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00071E44 File Offset: 0x00070E44
		private void timer2_Tick(object sender, EventArgs e)
		{
			this.timer2.Enabled = false;
			if (this.inputCard.Length >= 8)
			{
				try
				{
					long num;
					if (long.TryParse(this.inputCard, out num))
					{
						this.inputCard = "";
						this.txtf_CardNONew.Text = num.ToString();
						this.txtf_CardNONew.Focus();
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			this.inputCard = "";
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00071ED4 File Offset: 0x00070ED4
		private void txtf_CardNONew_KeyPress(object sender, KeyPressEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtf_CardNONew);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00071EE4 File Offset: 0x00070EE4
		private void txtf_CardNONew_KeyUp(object sender, KeyEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtf_CardNONew);
			if (!e.Control && !e.Alt && !e.Shift && e.KeyValue >= 48 && e.KeyValue <= 57)
			{
				if (this.inputCard.Length == 0)
				{
					this.timer2.Interval = 500;
					this.timer2.Enabled = true;
				}
				this.inputCard += (e.KeyValue - 48).ToString();
			}
		}

		// Token: 0x040007AB RID: 1963
		private string inputCard = "";
	}
}
