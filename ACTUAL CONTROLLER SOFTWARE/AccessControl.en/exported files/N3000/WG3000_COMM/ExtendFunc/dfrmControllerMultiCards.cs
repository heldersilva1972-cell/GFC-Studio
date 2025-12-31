using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000239 RID: 569
	public partial class dfrmControllerMultiCards : frmN3000
	{
		// Token: 0x06001131 RID: 4401 RVA: 0x0013C107 File Offset: 0x0013B107
		public dfrmControllerMultiCards()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x0013C120 File Offset: 0x0013B120
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0013C128 File Offset: 0x0013B128
		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (this.dataGridView1.SelectedRows.Count <= 0)
			{
				if (this.dataGridView1.SelectedCells.Count <= 0)
				{
					return;
				}
				int rowIndex = this.dataGridView1.SelectedCells[0].RowIndex;
			}
			else
			{
				int index = this.dataGridView1.SelectedRows[0].Index;
			}
			int num = 0;
			DataGridView dataGridView = this.dataGridView1;
			if (dataGridView.Rows.Count > 0)
			{
				try
				{
					num = dataGridView.CurrentCell.RowIndex;
				}
				catch
				{
				}
			}
			using (dfrmMultiCards dfrmMultiCards = new dfrmMultiCards())
			{
				dfrmMultiCards.DoorID = int.Parse(dataGridView.Rows[num].Cells[0].Value.ToString());
				dfrmMultiCards.Text = string.Concat(new string[]
				{
					dfrmMultiCards.Text,
					"[",
					dataGridView.Rows[num].Cells[2].Value.ToString(),
					"   ",
					dataGridView.Rows[num].Cells[3].Value.ToString(),
					"]"
				});
				if (dfrmMultiCards.ShowDialog(this) == DialogResult.OK)
				{
					dataGridView.Rows[num].Cells["f_MoreCards_Total"].Value = dfrmMultiCards.retValue;
				}
			}
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x0013C2BC File Offset: 0x0013B2BC
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0013C2BE File Offset: 0x0013B2BE
		private void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x0013C2CB File Offset: 0x0013B2CB
		private void dfrmControllerMultiCards_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0013C2E0 File Offset: 0x0013B2E0
		private void dfrmControllerMultiCards_KeyDown(object sender, KeyEventArgs e)
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
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0013C354 File Offset: 0x0013B354
		private void dfrmControllerMultiCards_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			string text = " SELECT ";
			text += " t_b_Door.f_DoorID , t_b_Controller.f_ControllerSN , t_b_Door.f_DoorNO , t_b_Door.f_DoorName , t_b_Door.f_MoreCards_Total , t_b_Controller.f_ZoneID  from t_b_Controller,t_b_Door WHERE t_b_Controller.f_ControllerID=t_b_Door.f_ControllerID  ORDER BY t_b_Door.f_DoorID ";
			wgAppConfig.fillDGVData(ref this.dataGridView1, text);
			DataTable table = ((DataView)this.dataGridView1.DataSource).Table;
			new icControllerZone().getAllowedControllers(ref table);
			this.loadOperatorPrivilege();
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0013C3B0 File Offset: 0x0013B3B0
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuMoreCards";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnEdit.Visible = false;
				this.dataGridView1.ReadOnly = true;
			}
		}
	}
}
