using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200000C RID: 12
	public partial class dfrmControllerZoneSelect : frmN3000
	{
		// Token: 0x0600009A RID: 154 RVA: 0x00018ED2 File Offset: 0x00017ED2
		public dfrmControllerZoneSelect()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00018F08 File Offset: 0x00017F08
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00018F18 File Offset: 0x00017F18
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (this.cboZone.Items.Count > 0)
			{
				this.selectZoneId = (int)this.arrZoneID[this.cboZone.SelectedIndex];
				base.DialogResult = DialogResult.OK;
			}
			else
			{
				base.DialogResult = DialogResult.Cancel;
			}
			base.Close();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00018F6F File Offset: 0x00017F6F
		private void dfrmControllerZoneSelect_Load(object sender, EventArgs e)
		{
			this.loadZoneInfo();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00018F78 File Offset: 0x00017F78
		private void loadZoneInfo()
		{
			new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
			int i = this.arrZoneID.Count;
			this.cboZone.Items.Clear();
			for (i = 0; i < this.arrZoneID.Count; i++)
			{
				if (i == 0 && string.IsNullOrEmpty(this.arrZoneName[i].ToString()))
				{
					this.cboZone.Items.Add("");
				}
				else
				{
					this.cboZone.Items.Add(this.arrZoneName[i].ToString());
				}
			}
			if (this.cboZone.Items.Count > 0)
			{
				this.cboZone.SelectedIndex = 0;
			}
			this.cboZone.Visible = true;
		}

		// Token: 0x04000189 RID: 393
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x0400018A RID: 394
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x0400018B RID: 395
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x0400018C RID: 396
		public int selectZoneId = -1;
	}
}
