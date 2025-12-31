using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002F5 RID: 757
	public partial class dfrmMealGroupConfigure : frmN3000
	{
		// Token: 0x06001609 RID: 5641 RVA: 0x001BD117 File Offset: 0x001BC117
		public dfrmMealGroupConfigure()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvGroups);
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x001BD13B File Offset: 0x001BC13B
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x001BD144 File Offset: 0x001BC144
		private void btnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = this.dgvGroups;
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
				using (dfrmMealGroup dfrmMealGroup = new dfrmMealGroup())
				{
					DataTable table = ((DataView)dataGridView.DataSource).Table;
					if (dataGridView.SelectedRows.Count == 1)
					{
						int num2 = (int)dataGridView.Rows[num].Cells[0].Value;
						DataRow dataRow = table.Rows.Find(num2);
						if (dataRow != null)
						{
							dfrmMealGroup.morning = decimal.Parse(dataRow["f_morning"].ToString(), CultureInfo.InvariantCulture);
							dfrmMealGroup.lunch = decimal.Parse(dataRow["f_lunch"].ToString(), CultureInfo.InvariantCulture);
							dfrmMealGroup.evening = decimal.Parse(dataRow["f_evening"].ToString(), CultureInfo.InvariantCulture);
							dfrmMealGroup.other = decimal.Parse(dataRow["f_other"].ToString(), CultureInfo.InvariantCulture);
							dfrmMealGroup.morning = decimal.Parse(dataRow["f_morning"].ToString(), CultureInfo.InvariantCulture);
							dfrmMealGroup.enable = (int)dataRow["f_Enabled"];
						}
					}
					if (dfrmMealGroup.ShowDialog(this) == DialogResult.OK)
					{
						for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
						{
							num = dataGridView.SelectedRows[i].Index;
							int num3 = (int)dataGridView.Rows[num].Cells[0].Value;
							DataRow dataRow = table.Rows.Find(num3);
							if (dataRow != null)
							{
								dataRow["f_morning"] = string.Format("{0}", dfrmMealGroup.morning);
								dataRow["f_lunch"] = string.Format("{0}", dfrmMealGroup.lunch);
								dataRow["f_evening"] = string.Format("{0}", dfrmMealGroup.evening);
								dataRow["f_other"] = string.Format("{0}", dfrmMealGroup.other);
								dataRow["f_Enabled"] = dfrmMealGroup.enable;
							}
						}
						table.AcceptChanges();
						this.Refresh();
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x001BD434 File Offset: 0x001BC434
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				string text = "DELETE from t_b_group4Meal";
				dbConnection.Open();
				dbCommand.CommandText = text;
				dbCommand.ExecuteNonQuery();
				for (int i = 0; i <= this.dvCheckAccess.Count - 1; i++)
				{
					text = "INSERT INTO t_b_group4Meal (f_GroupID,f_Enabled, f_Morning,f_Lunch,f_Evening,f_Other)";
					text = string.Concat(new string[]
					{
						text,
						" VALUES( ",
						this.dvCheckAccess[i]["f_GroupID"].ToString(),
						" ,",
						this.dvCheckAccess[i]["f_Enabled"].ToString(),
						" ,",
						this.dvCheckAccess[i]["f_Morning"].ToString(),
						" ,",
						this.dvCheckAccess[i]["f_Lunch"].ToString(),
						" ,",
						this.dvCheckAccess[i]["f_Evening"].ToString(),
						" ,",
						this.dvCheckAccess[i]["f_Other"].ToString(),
						")"
					});
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
				}
				dbConnection.Close();
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x001BD62C File Offset: 0x001BC62C
		private void dfrmCheckAccessSetup_Load(object sender, EventArgs e)
		{
			this.GroupName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.GroupName.HeaderText);
			try
			{
				string text = " SELECT a.f_GroupID,a.f_GroupName,b.f_Enabled, b.f_Morning,b.f_Lunch, b.f_Evening,b.f_Other from t_b_Group a LEFT JOIN t_b_group4Meal b ON a.f_GroupID = b.f_GroupID order by f_GroupName  + '\\' ASC";
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
							{
								oleDbDataAdapter.Fill(this.dsCheckAccess, "groups");
							}
						}
						goto IL_00D4;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlDataAdapter.Fill(this.dsCheckAccess, "groups");
						}
					}
				}
				try
				{
					IL_00D4:
					this.dsCheckAccess.Tables["groups"].PrimaryKey = new DataColumn[] { this.dsCheckAccess.Tables["groups"].Columns[0] };
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				this.dvCheckAccess = new DataView(this.dsCheckAccess.Tables["groups"]);
				for (int i = 0; i <= this.dvCheckAccess.Count - 1; i++)
				{
					if (string.IsNullOrEmpty(wgTools.SetObjToStr(this.dvCheckAccess[i]["f_Morning"])))
					{
						this.dvCheckAccess[i]["f_Morning"] = 0;
						this.dvCheckAccess[i]["f_Lunch"] = 0;
						this.dvCheckAccess[i]["f_Evening"] = 0;
						this.dvCheckAccess[i]["f_Other"] = 0;
						this.dvCheckAccess[i]["f_Enabled"] = 1;
					}
				}
				this.dgvGroups.AutoGenerateColumns = false;
				this.dgvGroups.DataSource = this.dvCheckAccess;
				int num = 0;
				while (num < this.dvCheckAccess.Table.Columns.Count && num < this.dgvGroups.ColumnCount)
				{
					this.dgvGroups.Columns[num].DataPropertyName = this.dvCheckAccess.Table.Columns[num].ColumnName;
					num++;
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
			this.dgvGroups.DefaultCellStyle.ForeColor = SystemColors.WindowText;
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x001BD9D0 File Offset: 0x001BC9D0
		private void dgvGroups_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x04002DAF RID: 11695
		private DataSet dsCheckAccess = new DataSet();

		// Token: 0x04002DB5 RID: 11701
		private DataView dvCheckAccess;
	}
}
