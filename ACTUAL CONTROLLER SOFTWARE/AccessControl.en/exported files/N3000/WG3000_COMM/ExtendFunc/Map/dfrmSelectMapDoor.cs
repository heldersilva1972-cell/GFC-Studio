using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc.Map
{
	// Token: 0x020002F2 RID: 754
	public partial class dfrmSelectMapDoor : frmN3000
	{
		// Token: 0x060015D2 RID: 5586 RVA: 0x001B7E21 File Offset: 0x001B6E21
		public dfrmSelectMapDoor()
		{
			this.InitializeComponent();
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x001B7E38 File Offset: 0x001B6E38
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (this.lstUnMappedDoors.SelectedItems.Count > 0)
			{
				this.doorName = this.lstUnMappedDoors.SelectedItem.ToString();
				base.DialogResult = DialogResult.OK;
				return;
			}
			if (this.lstMappedDoors.SelectedItems.Count > 0)
			{
				this.doorName = this.lstMappedDoors.SelectedItem.ToString();
				this.bAddDoor = false;
				base.DialogResult = DialogResult.OK;
				return;
			}
			base.DialogResult = DialogResult.Cancel;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x001B7EB5 File Offset: 0x001B6EB5
		private void dfrmSelectMapDoor_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x001B7ECC File Offset: 0x001B6ECC
		private void dfrmSelectMapDoor_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.dfrmFind1 == null)
					{
						this.dfrmFind1 = new dfrmFind();
					}
					this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
					this.dfrmFind1.Focus();
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x001B7F4C File Offset: 0x001B6F4C
		private void dfrmSelectMapDoor_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			base.KeyPreview = true;
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x001B7F5C File Offset: 0x001B6F5C
		private void lstMappedDoors_DoubleClick(object sender, EventArgs e)
		{
			this.btnOK.PerformClick();
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x001B7F69 File Offset: 0x001B6F69
		private void lstMappedDoors_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lstMappedDoors.SelectedItems.Count > 0)
			{
				this.lstUnMappedDoors.SelectedIndex = -1;
			}
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x001B7F8A File Offset: 0x001B6F8A
		private void lstUnMappedDoors_DoubleClick(object sender, EventArgs e)
		{
			this.btnOK.PerformClick();
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x001B7F97 File Offset: 0x001B6F97
		private void lstUnMappedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnOK.PerformClick();
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x001B7FA4 File Offset: 0x001B6FA4
		private void lstUnMappedDoors_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lstUnMappedDoors.SelectedItems.Count > 0)
			{
				this.lstMappedDoors.SelectedIndex = -1;
			}
		}

		// Token: 0x04002D4C RID: 11596
		public bool bAddDoor = true;

		// Token: 0x04002D4D RID: 11597
		public string doorName;
	}
}
