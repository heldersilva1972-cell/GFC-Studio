using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000238 RID: 568
	public partial class dfrmControllerInterLock : frmN3000
	{
		// Token: 0x06001124 RID: 4388 RVA: 0x0013B15E File Offset: 0x0013A15E
		public dfrmControllerInterLock()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0013B177 File Offset: 0x0013A177
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0013B180 File Offset: 0x0013A180
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.btnOK.Enabled = false;
			wgAppConfig.setSystemParamValue(61, this.chkActiveInterlockShare.Checked ? (this.chkGrouped.Checked ? "2" : "1") : "0");
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOK_Click_Acc(sender, e);
				return;
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					for (int i = 0; i <= this.dataGridView1.Rows.Count - 1; i++)
					{
						int num = 0;
						for (int j = 2; j < 6; j++)
						{
							if (this.dataGridView1.Rows[i].Cells[j].Value.ToString() == "1")
							{
								switch (j)
								{
								case 2:
									num = 1;
									break;
								case 3:
									num += 2;
									break;
								case 4:
									num = 4;
									break;
								case 5:
									num = 8;
									break;
								}
							}
						}
						string text = " UPDATE t_b_Controller SET ";
						text = string.Concat(new string[]
						{
							text,
							" f_InterLock = ",
							num.ToString(),
							" WHERE f_ControllerNO = ",
							this.dataGridView1.Rows[i].Cells[0].Value.ToString()
						});
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
					}
				}
			}
			base.Close();
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0013B358 File Offset: 0x0013A358
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
				{
					oleDbConnection.Open();
					for (int i = 0; i <= this.dataGridView1.Rows.Count - 1; i++)
					{
						int num = 0;
						for (int j = 2; j < 6; j++)
						{
							if (this.dataGridView1.Rows[i].Cells[j].Value.ToString() == "1")
							{
								switch (j)
								{
								case 2:
									num = 1;
									break;
								case 3:
									num += 2;
									break;
								case 4:
									num = 4;
									break;
								case 5:
									num = 8;
									break;
								}
							}
						}
						string text = " UPDATE t_b_Controller SET ";
						text = string.Concat(new string[]
						{
							text,
							" f_InterLock = ",
							num.ToString(),
							" WHERE f_ControllerNO = ",
							this.dataGridView1.Rows[i].Cells[0].Value.ToString()
						});
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
					}
				}
			}
			base.Close();
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0013B4E0 File Offset: 0x0013A4E0
		private void chkActiveInterlockShare_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkActiveInterlockShare.Checked)
			{
				for (int i = 0; i < this.dataGridView1.RowCount; i++)
				{
					DataGridViewRow dataGridViewRow = this.dataGridView1.Rows[i];
					if (wgMjController.GetControllerType(int.Parse(dataGridViewRow.Cells[1].Value.ToString())) == 2)
					{
						dataGridViewRow.Cells[3].ReadOnly = true;
						dataGridViewRow.Cells[4].ReadOnly = true;
						dataGridViewRow.Cells[5].ReadOnly = true;
						dataGridViewRow.Cells[3].Style.BackColor = SystemPens.InactiveBorder.Color;
						dataGridViewRow.Cells[4].Style.BackColor = SystemPens.InactiveBorder.Color;
						dataGridViewRow.Cells[5].Style.BackColor = SystemPens.InactiveBorder.Color;
					}
					else
					{
						dataGridViewRow.Cells[2].Value = 0;
						dataGridViewRow.Cells[3].Value = 0;
						dataGridViewRow.Cells[4].Value = 0;
						dataGridViewRow.Cells[2].ReadOnly = true;
						dataGridViewRow.Cells[3].ReadOnly = true;
						dataGridViewRow.Cells[4].ReadOnly = true;
						dataGridViewRow.Cells[2].Style.BackColor = SystemPens.InactiveBorder.Color;
						dataGridViewRow.Cells[3].Style.BackColor = SystemPens.InactiveBorder.Color;
						dataGridViewRow.Cells[4].Style.BackColor = SystemPens.InactiveBorder.Color;
					}
				}
			}
			if (this.chkActiveInterlockShare.Checked)
			{
				this.chkGrouped.Enabled = true;
				return;
			}
			this.chkGrouped.Enabled = false;
			this.chkGrouped.Checked = false;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0013B6F9 File Offset: 0x0013A6F9
		private void chkGrouped_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0013B6FB File Offset: 0x0013A6FB
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0013B6FD File Offset: 0x0013A6FD
		private void dfrmControllerInterLock_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0013B714 File Offset: 0x0013A714
		private void dfrmControllerInterLock_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.KeyValue == 81 && e.Shift)
				{
					if (!this.chkGrouped.Visible)
					{
						if (this.chkActiveInterlockShare.Visible)
						{
							this.chkGrouped.Visible = true;
							this.dataGridView1.Location = new Point(8, 72);
							this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, this.dataGridView1.Size.Height + 40 - this.dataGridView1.Location.Y);
						}
						else
						{
							this.chkActiveInterlockShare.Visible = true;
							this.dataGridView1.Location = new Point(8, 40);
							this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, this.dataGridView1.Size.Height + 8 - this.dataGridView1.Location.Y);
						}
					}
					this.chkActiveInterlockShare_CheckedChanged(null, null);
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

		// Token: 0x0600112D RID: 4397 RVA: 0x0013B8B8 File Offset: 0x0013A8B8
		private void dfrmControllerInterLock_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.chkActiveInterlockShare.Checked = wgAppConfig.getParamValBoolByNO(61);
			this.chkActiveInterlockShare.Visible = this.chkActiveInterlockShare.Checked;
			if (this.chkActiveInterlockShare.Visible)
			{
				this.dataGridView1.Location = new Point(8, 40);
				if (wgAppConfig.getSystemParamByNO(61) == "2")
				{
					this.chkGrouped.Checked = true;
					this.chkGrouped.Visible = true;
					this.dataGridView1.Location = new Point(8, 72);
				}
				this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, this.dataGridView1.Size.Height + 8 - this.dataGridView1.Location.Y);
			}
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = " SELECT ";
				text += " f_ControllerNO , f_ControllerSN ,  IIF ( [f_Interlock]=1 OR [f_Interlock]=3 , 1 , 0) AS f_InterLock12  ,  IIF ( [f_Interlock]=2 OR [f_Interlock]=3 , 1 , 0) AS f_InterLock34  , IIF ( [f_Interlock]=4 , 1 , 0) AS f_InterLock123 , IIF ( [f_Interlock]=8 , 1 , 0) AS f_InterLock1234 , f_DoorsNames , t_b_Controller.f_ZoneID  from t_b_Controller   WHERE f_ControllerSN > 199999999  ORDER BY f_ControllerNO ";
			}
			else
			{
				text = " SELECT ";
				text += " f_ControllerNO , f_ControllerSN ,  CASE WHEN [f_Interlock]=1 OR [f_Interlock]=3  THEN 1 ELSE 0 END AS f_InterLock12  ,  CASE WHEN [f_Interlock]=2 OR [f_Interlock]=3  THEN 1 ELSE 0 END AS f_InterLock34  , CASE WHEN [f_Interlock]=4 THEN 1 ELSE 0 END AS f_InterLock123 , CASE WHEN [f_Interlock]=8 THEN 1 ELSE 0 END AS f_InterLock1234 , f_DoorsNames , t_b_Controller.f_ZoneID  from t_b_Controller   WHERE f_ControllerSN > 199999999  ORDER BY f_ControllerNO ";
			}
			wgAppConfig.fillDGVData(ref this.dataGridView1, text);
			DataTable table = ((DataView)this.dataGridView1.DataSource).Table;
			new icControllerZone().getAllowedControllers(ref table);
			for (int i = 0; i < this.dataGridView1.RowCount; i++)
			{
				DataGridViewRow dataGridViewRow = this.dataGridView1.Rows[i];
				if (wgMjController.GetControllerType(int.Parse(dataGridViewRow.Cells[1].Value.ToString())) == 2)
				{
					dataGridViewRow.Cells[3].ReadOnly = true;
					dataGridViewRow.Cells[4].ReadOnly = true;
					dataGridViewRow.Cells[5].ReadOnly = true;
					dataGridViewRow.Cells[3].Style.BackColor = SystemPens.InactiveBorder.Color;
					dataGridViewRow.Cells[4].Style.BackColor = SystemPens.InactiveBorder.Color;
					dataGridViewRow.Cells[5].Style.BackColor = SystemPens.InactiveBorder.Color;
				}
			}
			this.chkActiveInterlockShare_CheckedChanged(null, null);
			this.loadOperatorPrivilege();
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0013BAF8 File Offset: 0x0013AAF8
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuInterLock";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnOK.Visible = false;
				this.dataGridView1.ReadOnly = true;
			}
		}
	}
}
