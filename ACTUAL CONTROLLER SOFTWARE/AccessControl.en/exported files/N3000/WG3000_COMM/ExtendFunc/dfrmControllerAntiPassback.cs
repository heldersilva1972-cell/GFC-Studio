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
	// Token: 0x02000235 RID: 565
	public partial class dfrmControllerAntiPassback : frmN3000
	{
		// Token: 0x060010ED RID: 4333 RVA: 0x001365C8 File Offset: 0x001355C8
		public dfrmControllerAntiPassback()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x001365E8 File Offset: 0x001355E8
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x001365F0 File Offset: 0x001355F0
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
			using (dfrmAntiBack dfrmAntiBack = new dfrmAntiBack())
			{
				dfrmAntiBack.ControllerSN = dataGridView.Rows[num].Cells[1].Value.ToString();
				dfrmAntiBack.Text = dfrmAntiBack.Text + "[" + dataGridView.Rows[num].Cells[1].Value.ToString() + "]";
				if (dfrmAntiBack.ShowDialog(this) == DialogResult.OK)
				{
					int retValue = dfrmAntiBack.retValue;
					if (wgAppConfig.runUpdateSql("UPDATE t_b_Controller SET f_AntiBack =" + retValue.ToString() + " Where f_ControllerSN = " + dataGridView.Rows[num].Cells[1].Value.ToString()) > 0)
					{
						if (retValue == 0)
						{
							dataGridView.Rows[num].Cells["f_AntiBack"].Value = 0;
						}
						else
						{
							dataGridView.Rows[num].Cells["f_AntiBack"].Value = 1;
						}
					}
				}
			}
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x001367CC File Offset: 0x001357CC
		private void chkActiveAntibackShare_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.bLoad)
			{
				wgAppConfig.setSystemParamValue(62, this.chkActiveAntibackShare.Checked ? (this.chkGrouped.Checked ? "2" : "1") : "0");
				if (this.chkActiveAntibackShare.Checked)
				{
					this.chkGrouped.Enabled = true;
					return;
				}
				this.chkGrouped.Enabled = false;
				this.chkGrouped.Checked = false;
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00136848 File Offset: 0x00135848
		private void chkGrouped_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.bLoad)
			{
				wgAppConfig.setSystemParamValue(62, this.chkActiveAntibackShare.Checked ? (this.chkGrouped.Checked ? "2" : "1") : "0");
			}
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00136887 File Offset: 0x00135887
		private void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00136894 File Offset: 0x00135894
		private void dfrmControllerAntiPassback_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x001368AC File Offset: 0x001358AC
		private void dfrmControllerAntiPassback_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.KeyValue == 81 && e.Shift)
				{
					if (!this.chkGrouped.Visible)
					{
						if (this.chkActiveAntibackShare.Visible)
						{
							this.chkGrouped.Visible = true;
							this.dataGridView1.Location = new Point(8, 72);
							this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, this.dataGridView1.Size.Height + 40 - this.dataGridView1.Location.Y);
						}
						else
						{
							this.chkActiveAntibackShare.Visible = true;
							this.dataGridView1.Location = new Point(8, 40);
							this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, this.dataGridView1.Size.Height + 8 - this.dataGridView1.Location.Y);
						}
					}
					this.chkActiveAntibackShare_CheckedChanged(null, null);
				}
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

		// Token: 0x060010F5 RID: 4341 RVA: 0x00136A50 File Offset: 0x00135A50
		private void dfrmControllerAntiPassback_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.chkActiveAntibackShare.Checked = wgAppConfig.getParamValBoolByNO(62);
			this.chkActiveAntibackShare.Visible = this.chkActiveAntibackShare.Checked;
			if (this.chkActiveAntibackShare.Visible)
			{
				this.dataGridView1.Location = new Point(8, 40);
				if (wgAppConfig.getSystemParamByNO(62) == "2")
				{
					this.chkGrouped.Checked = true;
					this.chkGrouped.Visible = true;
					this.dataGridView1.Location = new Point(8, 72);
				}
				this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, this.dataGridView1.Size.Height + 8 - this.dataGridView1.Location.Y);
			}
			string text = " SELECT ";
			text += " f_ControllerNO , f_ControllerSN , f_AntiBack , f_DoorsNames , t_b_Controller.f_ZoneID   from t_b_Controller ORDER BY f_ControllerNO ";
			wgAppConfig.fillDGVData(ref this.dataGridView1, text);
			DataTable table = ((DataView)this.dataGridView1.DataSource).Table;
			DataView dataView = new DataView(table);
			if (dataView.Count > 0)
			{
				dataView.RowFilter = " f_AntiBack > 10";
				if (dataView.Count > 0)
				{
					dfrmAntiBack.bDisplayIndoorPersonMax = true;
				}
			}
			new icControllerZone().getAllowedControllers(ref table);
			this.loadOperatorPrivilege();
			this.bLoad = false;
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00136BB4 File Offset: 0x00135BB4
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuAntiBack";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnEdit.Visible = false;
			}
		}

		// Token: 0x04001E31 RID: 7729
		private bool bLoad = true;
	}
}
