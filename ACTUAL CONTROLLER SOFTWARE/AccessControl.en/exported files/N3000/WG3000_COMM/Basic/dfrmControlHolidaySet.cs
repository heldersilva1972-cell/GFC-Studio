using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000009 RID: 9
	public partial class dfrmControlHolidaySet : frmN3000
	{
		// Token: 0x06000071 RID: 113 RVA: 0x0000F617 File Offset: 0x0000E617
		public dfrmControlHolidaySet()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvNeedWork);
			wgAppConfig.custDataGridview(ref this.dgvMain);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000F63C File Offset: 0x0000E63C
		private void _dataTableLoad()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this._dataTableLoad_Acc();
				return;
			}
			SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
			try
			{
				string text = "SELECT f_Id, f_BeginYMDHMS, f_EndYMDHMS, f_Notes,f_forcework From t_b_ControlHolidays ";
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						this.dt = new DataTable();
						sqlDataAdapter.Fill(this.dt);
						this.dvHolidays = new DataView(this.dt);
						this.dvHolidays.RowFilter = " f_forcework <> 1";
						this.dgvMain.AutoGenerateColumns = false;
						this.dgvMain.DataSource = this.dvHolidays;
						this.dvHolidays.Sort = "f_BeginYMDHMS ASC";
						for (int i = 0; i < this.dvHolidays.Table.Columns.Count; i++)
						{
							this.dgvMain.Columns[i].DataPropertyName = this.dvHolidays.Table.Columns[i].ColumnName;
							this.dgvMain.Columns[i].Name = this.dvHolidays.Table.Columns[i].ColumnName;
							if (this.dgvMain.ColumnCount == i + 1)
							{
								break;
							}
						}
						this.dvNeedWork = new DataView(this.dt);
						this.dvNeedWork.RowFilter = " f_forcework = 1";
						this.dgvNeedWork.AutoGenerateColumns = false;
						this.dgvNeedWork.DataSource = this.dvNeedWork;
						this.dvNeedWork.Sort = "f_BeginYMDHMS ASC";
						for (int j = 0; j < this.dvNeedWork.Table.Columns.Count; j++)
						{
							this.dgvNeedWork.Columns[j].DataPropertyName = this.dvNeedWork.Table.Columns[j].ColumnName;
							this.dgvNeedWork.Columns[j].Name = this.dvNeedWork.Table.Columns[j].ColumnName;
							if (this.dgvNeedWork.ColumnCount == j + 1)
							{
								break;
							}
						}
					}
				}
				wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_BeginYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
				wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_EndYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
				wgAppConfig.setDisplayFormatDate(this.dgvNeedWork, "f_BeginYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
				wgAppConfig.setDisplayFormatDate(this.dgvNeedWork, "f_EndYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
				this.dgvMain.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.dgvNeedWork.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.dgvMain.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.dgvNeedWork.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				sqlConnection.Dispose();
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000F9B4 File Offset: 0x0000E9B4
		private void _dataTableLoad_Acc()
		{
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			try
			{
				string text = "SELECT f_Id, f_BeginYMDHMS, f_EndYMDHMS, f_Notes,f_forcework From t_b_ControlHolidays ";
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
					{
						this.dt = new DataTable();
						oleDbDataAdapter.Fill(this.dt);
						this.dvHolidays = new DataView(this.dt);
						this.dvHolidays.RowFilter = " f_forcework <> 1";
						this.dgvMain.AutoGenerateColumns = false;
						this.dgvMain.DataSource = this.dvHolidays;
						this.dvHolidays.Sort = "f_BeginYMDHMS ASC";
						for (int i = 0; i < this.dvHolidays.Table.Columns.Count; i++)
						{
							this.dgvMain.Columns[i].DataPropertyName = this.dvHolidays.Table.Columns[i].ColumnName;
							this.dgvMain.Columns[i].Name = this.dvHolidays.Table.Columns[i].ColumnName;
							if (this.dgvMain.ColumnCount == i + 1)
							{
								break;
							}
						}
						this.dvNeedWork = new DataView(this.dt);
						this.dvNeedWork.RowFilter = " f_forcework = 1";
						this.dgvNeedWork.AutoGenerateColumns = false;
						this.dgvNeedWork.DataSource = this.dvNeedWork;
						this.dvNeedWork.Sort = "f_BeginYMDHMS ASC";
						for (int j = 0; j < this.dvNeedWork.Table.Columns.Count; j++)
						{
							this.dgvNeedWork.Columns[j].DataPropertyName = this.dvNeedWork.Table.Columns[j].ColumnName;
							this.dgvNeedWork.Columns[j].Name = this.dvNeedWork.Table.Columns[j].ColumnName;
							if (this.dgvNeedWork.ColumnCount == j + 1)
							{
								break;
							}
						}
					}
				}
				wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_BeginYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
				wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_EndYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
				wgAppConfig.setDisplayFormatDate(this.dgvNeedWork, "f_BeginYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
				wgAppConfig.setDisplayFormatDate(this.dgvNeedWork, "f_EndYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
				this.dgvMain.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.dgvNeedWork.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.dgvMain.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.dgvNeedWork.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				oleDbConnection.Dispose();
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000FD1C File Offset: 0x0000ED1C
		private void btnAddHoliday_Click(object sender, EventArgs e)
		{
			using (dfrmControlHolidayAdd dfrmControlHolidayAdd = new dfrmControlHolidayAdd())
			{
				dfrmControlHolidayAdd.ShowDialog(this);
			}
			this._dataTableLoad();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000FD5C File Offset: 0x0000ED5C
		private void btnAddNeedWorkDay_Click(object sender, EventArgs e)
		{
			using (dfrmControlHolidayAdd dfrmControlHolidayAdd = new dfrmControlHolidayAdd())
			{
				dfrmControlHolidayAdd.Text = this.groupBox2.Text;
				dfrmControlHolidayAdd.bHoliday = false;
				dfrmControlHolidayAdd.ShowDialog(this);
			}
			this._dataTableLoad();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000FDB4 File Offset: 0x0000EDB4
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000FDC4 File Offset: 0x0000EDC4
		private void btnDelHoliday_Click(object sender, EventArgs e)
		{
			if (this.dgvMain.Rows.Count > 0)
			{
				int index = this.dgvMain.SelectedRows[0].Index;
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_ControlHolidays WHERE [f_Id]= " + this.dgvMain.SelectedRows[0].Cells["f_Id"].Value.ToString());
				this._dataTableLoad();
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000FE3C File Offset: 0x0000EE3C
		private void btnDelNeedWorkDay_Click(object sender, EventArgs e)
		{
			if (this.dgvNeedWork.Rows.Count > 0)
			{
				int index = this.dgvNeedWork.SelectedRows[0].Index;
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_ControlHolidays WHERE [f_Id]= " + this.dgvNeedWork.SelectedRows[0].Cells["f_Id"].Value.ToString());
				this._dataTableLoad();
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000FEB4 File Offset: 0x0000EEB4
		private void dfrmHolidaySet_Load(object sender, EventArgs e)
		{
			this._dataTableLoad();
			bool flag = false;
			string text = "mnuHolidaySet";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAddHoliday.Visible = false;
				this.btnDelHoliday.Visible = false;
			}
		}

		// Token: 0x040000B0 RID: 176
		private DataTable dt;

		// Token: 0x040000B1 RID: 177
		private DataView dvHolidays;

		// Token: 0x040000B2 RID: 178
		private DataView dvNeedWork;
	}
}
