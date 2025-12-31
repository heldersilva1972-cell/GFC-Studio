using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000376 RID: 886
	public partial class dfrmSortSet : frmN3000
	{
		// Token: 0x06001D15 RID: 7445 RVA: 0x00267EAD File Offset: 0x00266EAD
		public dfrmSortSet()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvColumns);
			wgAppConfig.custDataGridview(ref this.dgvSelectedColumns);
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x00267EE8 File Offset: 0x00266EE8
		private void _loadData()
		{
			string text = "SELECT 0 as f_RecID,' ' as f_ColumnNameDisplayed,0 as f_selected, ' ' as f_Sort, ' ' as f_ColumnName, 0 as f_SortID FROM t_a_HolidayType WHERE 1<0 ";
			this.dt = new DataTable("sort");
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
					goto IL_00C3;
				}
			}
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
			IL_00C3:
			for (int i = 0; i < this.arrColsName.Count; i++)
			{
				DataRow dataRow = this.dt.NewRow();
				dataRow["f_RecID"] = i + 1;
				dataRow["f_ColumnNameDisplayed"] = this.arrColsShow[i];
				dataRow["f_selected"] = 0;
				dataRow["f_ColumnName"] = this.arrColsName[i];
				this.dt.Rows.Add(dataRow);
			}
			this.dt.AcceptChanges();
			if (!string.IsNullOrEmpty(this.strDisplaySort))
			{
				string[] array = this.strDisplaySort.Split(new char[] { ',' });
				for (int j = 0; j < array.Length; j++)
				{
					string[] array2 = array[j].Trim().Split(new char[] { ' ' });
					for (int k = 0; k < this.dt.Rows.Count; k++)
					{
						if (this.dt.Rows[k]["f_ColumnName"].ToString().ToUpper() == array2[0].Trim().ToUpper())
						{
							this.dt.Rows[k]["f_SortID"] = j;
							this.dt.Rows[k]["f_selected"] = 1;
							this.dt.Rows[k]["f_Sort"] = ((array2[1].Trim() == "DESC") ? CommonStr.strDESC : CommonStr.strASC);
							break;
						}
					}
				}
			}
			this.dv = new DataView(this.dt);
			this.dvSelected = new DataView(this.dt);
			this.dv.RowFilter = "f_Selected = 0";
			this.dvSelected.RowFilter = "f_Selected > 0";
			this.dvSelected.Sort = "f_SortID ASC";
			this.dgvColumns.AutoGenerateColumns = false;
			this.dgvColumns.DataSource = this.dv;
			this.dgvSelectedColumns.AutoGenerateColumns = false;
			this.dgvSelectedColumns.DataSource = this.dvSelected;
			try
			{
				this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			for (int l = 0; l < this.dgvColumns.Columns.Count; l++)
			{
				this.dgvColumns.Columns[l].DataPropertyName = this.dt.Columns[l].ColumnName;
				this.dgvSelectedColumns.Columns[l].DataPropertyName = this.dt.Columns[l].ColumnName;
			}
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x0026833C File Offset: 0x0026733C
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			int num = this.dgvSelectedColumns.RowCount;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				if ((int)this.dt.Rows[i]["f_Selected"] != 1)
				{
					this.dt.Rows[i]["f_Selected"] = 1;
					this.dt.Rows[i]["f_Sort"] = this.cbof_Sort.Text;
					this.dt.Rows[i]["f_SortID"] = num;
					num++;
				}
			}
			this.dt.AcceptChanges();
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x00268410 File Offset: 0x00267410
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = this.dgvColumns;
				int num;
				if (dataGridView.SelectedRows.Count <= 0)
				{
					if (dataGridView.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dataGridView.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dataGridView.SelectedRows[0].Index;
				}
				DataTable table = ((DataView)dataGridView.DataSource).Table;
				if (dataGridView.SelectedRows.Count > 0)
				{
					int count = dataGridView.SelectedRows.Count;
					int[] array = new int[count];
					for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
					{
						array[i] = (int)dataGridView.SelectedRows[i].Cells[0].Value;
					}
					int num2 = this.dgvSelectedColumns.RowCount;
					for (int j = 0; j < count; j++)
					{
						int num3 = array[j];
						DataRow dataRow = table.Rows.Find(num3);
						if (dataRow != null)
						{
							dataRow["f_Selected"] = 1;
							dataRow["f_SortID"] = num2;
							num2++;
							dataRow["f_Sort"] = this.cbof_Sort.Text;
						}
					}
				}
				else
				{
					int num4 = (int)dataGridView.Rows[num].Cells[0].Value;
					DataRow dataRow = table.Rows.Find(num4);
					int num5 = this.dgvSelectedColumns.RowCount;
					if (dataRow != null)
					{
						dataRow["f_Selected"] = 1;
						dataRow["f_SortID"] = num5;
						num5++;
						dataRow["f_Sort"] = this.cbof_Sort.Text;
					}
				}
				table.AcceptChanges();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x0026861C File Offset: 0x0026761C
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			DataTable table = ((DataView)this.dgvSelectedColumns.DataSource).Table;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				table.Rows[i]["f_Selected"] = 0;
			}
			table.AcceptChanges();
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x00268678 File Offset: 0x00267678
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = this.dgvSelectedColumns;
				int num;
				if (dataGridView.SelectedRows.Count <= 0)
				{
					if (dataGridView.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dataGridView.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dataGridView.SelectedRows[0].Index;
				}
				using (DataTable table = ((DataView)dataGridView.DataSource).Table)
				{
					if (dataGridView.SelectedRows.Count > 0)
					{
						int count = dataGridView.SelectedRows.Count;
						int[] array = new int[count];
						for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
						{
							array[i] = (int)dataGridView.SelectedRows[i].Cells[0].Value;
						}
						for (int j = 0; j < count; j++)
						{
							int num2 = array[j];
							DataRow dataRow = table.Rows.Find(num2);
							if (dataRow != null)
							{
								dataRow["f_Selected"] = 0;
							}
						}
					}
					else
					{
						int num3 = (int)dataGridView.Rows[num].Cells[0].Value;
						DataRow dataRow = table.Rows.Find(num3);
						if (dataRow != null)
						{
							dataRow["f_Selected"] = 0;
						}
					}
					table.AcceptChanges();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x00268824 File Offset: 0x00267824
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x00268834 File Offset: 0x00267834
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.dvSelected.Sort = "f_SortID ASC";
			this.strDisplaySort = "";
			for (int i = 0; i < this.dvSelected.Count; i++)
			{
				if (!string.IsNullOrEmpty(this.strDisplaySort))
				{
					this.strDisplaySort += ",";
				}
				object obj = this.strDisplaySort;
				this.strDisplaySort = string.Concat(new object[]
				{
					obj,
					this.dvSelected[i]["f_ColumnName"],
					" ",
					(this.dvSelected[i]["f_Sort"].ToString() == CommonStr.strDESC) ? "DESC" : "ASC"
				});
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00268920 File Offset: 0x00267920
		private void dfrmSortSet_Load(object sender, EventArgs e)
		{
			this.cbof_Sort.Items.Clear();
			this.cbof_Sort.Items.Add(CommonStr.strASC);
			this.cbof_Sort.Items.Add(CommonStr.strDESC);
			this.cbof_Sort.Text = CommonStr.strASC;
			this._loadData();
		}

		// Token: 0x04003804 RID: 14340
		private DataTable dt;

		// Token: 0x04003805 RID: 14341
		private DataView dv;

		// Token: 0x04003806 RID: 14342
		private DataView dvSelected;

		// Token: 0x04003807 RID: 14343
		public ArrayList arrColsName = new ArrayList();

		// Token: 0x04003808 RID: 14344
		public ArrayList arrColsShow = new ArrayList();

		// Token: 0x04003809 RID: 14345
		public string strDisplaySort;
	}
}
