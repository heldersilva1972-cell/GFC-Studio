using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000024 RID: 36
	public partial class dfrmOptionAdvanced : frmN3000
	{
		// Token: 0x06000259 RID: 601 RVA: 0x00047570 File Offset: 0x00046570
		public dfrmOptionAdvanced()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0004757E File Offset: 0x0004657E
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				this.txtPhotoDirectory.Text = this.folderBrowserDialog1.SelectedPath;
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000475A4 File Offset: 0x000465A4
		private void chkValidSwipeGap_CheckedChanged(object sender, EventArgs e)
		{
			this.nudValidSwipeGap.Enabled = this.chkValidSwipeGap.Checked;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000475BC File Offset: 0x000465BC
		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.txtPhotoDirectory.Text.Trim()) && this.txtPhotoDirectory.Text.Trim().Length != Encoding.GetEncoding("utf-8").GetBytes(this.txtPhotoDirectory.Text.Trim()).Length)
			{
				XMessageBox.Show(this, CommonStr.strInvalidPhotoDirectory, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			wgAppConfig.UpdateKeyVal("AllowUploadUserName", this.chkAllowUploadUserName.Checked ? "1" : "0");
			wgAppConfig.setSystemParamNotes(41, this.txtPhotoDirectory.Text.Trim());
			wgAppConfig.Path4PhotoRefresh();
			if (this.chkValidSwipeGap.Visible)
			{
				int num = 0;
				if (this.chkValidSwipeGap.Checked)
				{
					num = (int)this.nudValidSwipeGap.Value;
				}
				if ((num & 1) > 0)
				{
					num++;
				}
				wgAppConfig.setSystemParamValue(147, num.ToString());
			}
			base.Close();
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000476BC File Offset: 0x000466BC
		private void dfrmOptionAdvanced_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x0600025E RID: 606 RVA: 0x000476FC File Offset: 0x000466FC
		private void dfrmOptionAdvanced_Load(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.chkAllowUploadUserName.Checked = wgAppConfig.GetKeyVal("AllowUploadUserName") == "1";
			this.txtPhotoDirectory.Text = wgAppConfig.getSystemParamByNO(41);
			this.chkValidSwipeGap.Visible = wgAppConfig.getParamValBoolByNO(147);
			this.chkValidSwipeGap.Checked = wgAppConfig.getParamValBoolByNO(147);
			this.nudValidSwipeGap.Visible = wgAppConfig.getParamValBoolByNO(147);
			this.nudValidSwipeGap.Enabled = false;
			if (this.chkValidSwipeGap.Checked)
			{
				this.nudValidSwipeGap.Value = int.Parse(wgAppConfig.getSystemParamByNO(147));
				this.nudValidSwipeGap.Enabled = true;
			}
			this.chkValidSwipeGap.Visible = true;
			this.nudValidSwipeGap.Visible = true;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000477F2 File Offset: 0x000467F2
		private void funcCtrlShiftQ()
		{
			this.txtPhotoDirectory.ReadOnly = false;
			this.chkValidSwipeGap.Visible = true;
			this.nudValidSwipeGap.Visible = true;
		}
	}
}
