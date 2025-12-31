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
	// Token: 0x02000237 RID: 567
	public partial class dfrmControllerFirstCard : frmN3000
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x0013A9E9 File Offset: 0x001399E9
		public dfrmControllerFirstCard()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0013AA02 File Offset: 0x00139A02
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0013AA0C File Offset: 0x00139A0C
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
			using (dfrmFirstCard dfrmFirstCard = new dfrmFirstCard())
			{
				dfrmFirstCard.DoorID = int.Parse(dataGridView.Rows[num].Cells[0].Value.ToString());
				dfrmFirstCard.Text = string.Concat(new string[]
				{
					dfrmFirstCard.Text,
					"[",
					dataGridView.Rows[num].Cells[2].Value.ToString(),
					"   ",
					dataGridView.Rows[num].Cells[3].Value.ToString(),
					"]"
				});
				if (dfrmFirstCard.ShowDialog(this) == DialogResult.OK)
				{
					dataGridView.Rows[num].Cells["f_FirstCard_Enabled"].Value = int.Parse(dfrmFirstCard.retValue);
				}
			}
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0013ABA8 File Offset: 0x00139BA8
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0013ABAA File Offset: 0x00139BAA
		private void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0013ABB7 File Offset: 0x00139BB7
		private void dfrmControllerFirstCard_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0013ABCC File Offset: 0x00139BCC
		private void dfrmControllerFirstCard_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x06001120 RID: 4384 RVA: 0x0013AC40 File Offset: 0x00139C40
		private void dfrmControllerFirstCard_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			string text = " SELECT ";
			text += " t_b_Door.f_DoorID , t_b_Controller.f_ControllerSN , t_b_Door.f_DoorNO , t_b_Door.f_DoorName , t_b_Door.f_FirstCard_Enabled , t_b_Controller.f_ZoneID  from t_b_Controller,t_b_Door WHERE t_b_Controller.f_ControllerID=t_b_Door.f_ControllerID  ORDER BY t_b_Door.f_DoorID ";
			wgAppConfig.fillDGVData(ref this.dataGridView1, text);
			DataTable table = ((DataView)this.dataGridView1.DataSource).Table;
			new icControllerZone().getAllowedControllers(ref table);
			this.loadOperatorPrivilege();
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0013AC9C File Offset: 0x00139C9C
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuFirstCard";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnEdit.Visible = false;
				this.dataGridView1.ReadOnly = true;
			}
		}
	}
}
