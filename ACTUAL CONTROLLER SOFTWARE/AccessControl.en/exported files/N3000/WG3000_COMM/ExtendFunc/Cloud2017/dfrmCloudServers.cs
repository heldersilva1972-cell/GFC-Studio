using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Cloud2017
{
	// Token: 0x02000230 RID: 560
	public partial class dfrmCloudServers : frmN3000
	{
		// Token: 0x060010CA RID: 4298 RVA: 0x0013266A File Offset: 0x0013166A
		public dfrmCloudServers()
		{
			this.InitializeComponent();
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00132678 File Offset: 0x00131678
		private void btnAddToSystem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				string text = "";
				int num = 0;
				for (int i = 0; i < this.dgvFoundControllers.Rows.Count; i++)
				{
					int num2 = int.Parse(this.dgvFoundControllers.Rows[i].Cells[1].Value.ToString());
					if (num2 != -1)
					{
						if (!wgMjController.validSN(num2))
						{
							XMessageBox.Show(this, num2.ToString() + "\r\n" + CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						if (wgMjController.GetControllerType(num2) == 4 && icConsumer.gTimeSecondEnabled)
						{
							XMessageBox.Show(num2.ToString() + "\r\n" + CommonStr.strTimeSecondAddControllerWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						if (!icController.IsExisted2SN(num2, 0))
						{
							text = text + num2.ToString() + ",";
							num++;
							using (dfrmController dfrmController = new dfrmController())
							{
								dfrmController.OperateNew = true;
								dfrmController.WindowState = FormWindowState.Minimized;
								dfrmController.Show();
								dfrmController.mtxtbControllerSN.Text = num2.ToString();
								dfrmController.btnNext.PerformClick();
								dfrmController.btnOK.PerformClick();
								Application.DoEvents();
							}
						}
					}
				}
				Cursor.Current = Cursors.Default;
				XMessageBox.Show(string.Format("{0}:[{1:d}]\r\n{2}  ", CommonStr.strAutoAddController, num, text), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				if (num > 0)
				{
					base.DialogResult = DialogResult.OK;
				}
			}
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0013282C File Offset: 0x0013182C
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			this.dgvFoundControllers.Rows.Clear();
			for (int i = 0; i < wgTools.arrSNReceived.Count; i++)
			{
				string[] array = new string[]
				{
					(this.dgvFoundControllers.Rows.Count + 1).ToString().PadLeft(4, '0'),
					wgTools.arrSNReceived[i].ToString(),
					wgTools.arrSNIP[i].ToString(),
					wgTools.arrSNPort[i].ToString(),
					((DateTime)wgTools.arrRefreshTime[i]).ToString(wgTools.DisplayFormat_DateYMDHMS),
					((DateTime)wgTools.arrCreateTime[i]).ToString(wgTools.DisplayFormat_DateYMDHMS)
				};
				this.dgvFoundControllers.Rows.Add(array);
			}
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0013291F File Offset: 0x0013191F
		private void dfrmCloudServers_Load(object sender, EventArgs e)
		{
			this.btnRefresh_Click(null, null);
		}
	}
}
