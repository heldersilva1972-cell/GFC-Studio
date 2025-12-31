using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000367 RID: 871
	public partial class dfrmFullAttendanceSet : frmN3000
	{
		// Token: 0x06001C53 RID: 7251 RVA: 0x0025548E File Offset: 0x0025448E
		public dfrmFullAttendanceSet()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvHolidayType);
			wgAppConfig.custDataGridview(ref this.dgvSelected);
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x002554B4 File Offset: 0x002544B4
		private void _loadData()
		{
			string text = "SELECT [f_No]      ,[f_HolidayType],0 as f_selected FROM t_a_HolidayType ORDER BY f_NO ";
			this.dt = new DataTable("t_a_HolidayType");
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dt);
						}
					}
				}
				comShift_Acc.localizedHolidayType(this.dt);
			}
			else
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlDataAdapter.Fill(this.dt);
						}
					}
				}
				comShift.localizedHolidayType(this.dt);
			}
			string text2 = "";
			string text3 = "";
			string text4 = "";
			wgAppConfig.getSystemParamValue(174, out text2, out text3, out text4);
			string text5 = text4;
			this.dv = new DataView(this.dt);
			this.dvSelected = new DataView(this.dt);
			if (!string.IsNullOrEmpty(text5))
			{
				this.dvSelected.RowFilter = string.Format("f_NO IN ({0})", text5);
				for (int i = 0; i < this.dvSelected.Count; i++)
				{
					this.dvSelected[i]["f_Selected"] = 1;
				}
			}
			this.dv.RowFilter = "f_Selected = 0";
			this.dvSelected.RowFilter = "f_Selected > 0";
			this.dgvHolidayType.AutoGenerateColumns = false;
			this.dgvHolidayType.DataSource = this.dv;
			this.dgvSelected.AutoGenerateColumns = false;
			this.dgvSelected.DataSource = this.dvSelected;
			for (int j = 0; j < this.dgvHolidayType.Columns.Count; j++)
			{
				this.dgvHolidayType.Columns[j].DataPropertyName = this.dt.Columns[j].ColumnName;
				this.dgvSelected.Columns[j].DataPropertyName = this.dt.Columns[j].ColumnName;
			}
			text3 = "1";
			wgAppConfig.getSystemParamValue(175, out text2, out text3, out text4);
			if (text3 == "0")
			{
				this.chkManualRecordAsFullAttendance.Checked = false;
				return;
			}
			this.chkManualRecordAsFullAttendance.Checked = true;
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x002557A4 File Offset: 0x002547A4
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			this.dvSelected.RowFilter = "";
			for (int i = 0; i < this.dvSelected.Count; i++)
			{
				this.dvSelected[i]["f_Selected"] = 1;
			}
			this.dvSelected.Table.AcceptChanges();
			this.dvSelected.RowFilter = "f_Selected > 0";
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x00255814 File Offset: 0x00254814
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			if (this.dgvHolidayType.SelectedRows.Count > 0)
			{
				this.dvSelected.RowFilter = "f_NO = " + ((int)this.dgvHolidayType.SelectedRows[0].Cells[0].Value).ToString();
				if (this.dvSelected.Count > 0)
				{
					this.dvSelected[0]["f_Selected"] = 1;
				}
				this.dvSelected.Table.AcceptChanges();
				this.dvSelected.RowFilter = "f_Selected > 0";
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x002558C4 File Offset: 0x002548C4
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			this.dvSelected.RowFilter = "";
			for (int i = 0; i < this.dvSelected.Count; i++)
			{
				this.dvSelected[i]["f_Selected"] = 0;
			}
			this.dvSelected.Table.AcceptChanges();
			this.dvSelected.RowFilter = "f_Selected > 0";
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00255934 File Offset: 0x00254934
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			if (this.dgvSelected.SelectedRows.Count > 0)
			{
				this.dvSelected.RowFilter = "f_NO = " + ((int)this.dgvSelected.SelectedRows[0].Cells[0].Value).ToString();
				if (this.dvSelected.Count > 0)
				{
					this.dvSelected[0]["f_Selected"] = 0;
				}
				this.dvSelected.Table.AcceptChanges();
				this.dvSelected.RowFilter = "f_Selected > 0";
			}
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x002559E4 File Offset: 0x002549E4
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x002559EC File Offset: 0x002549EC
		private void btnOK_Click(object sender, EventArgs e)
		{
			string text = "0";
			for (int i = 0; i < this.dvSelected.Count; i++)
			{
				text = text + "," + this.dvSelected[i]["f_No"].ToString();
			}
			wgAppConfig.setSystemParamValue(174, "Holiday Type As Full Attendance", "Holiday Type", text);
			wgAppConfig.setSystemParamValue(175, "Manual Records As Full Attendance", this.chkManualRecordAsFullAttendance.Checked ? "1" : "0", "2015-04-07 12:39:15");
			base.Close();
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x00255A8C File Offset: 0x00254A8C
		private void dfrmFullAttendanceSet_Load(object sender, EventArgs e)
		{
			this._loadData();
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00255A94 File Offset: 0x00254A94
		private void dgvHolidayType_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneUser.PerformClick();
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00255AA1 File Offset: 0x00254AA1
		private void dgvSelected_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x0400368C RID: 13964
		private DataTable dt;

		// Token: 0x0400368D RID: 13965
		private DataView dv;

		// Token: 0x0400368E RID: 13966
		private DataView dvSelected;
	}
}
