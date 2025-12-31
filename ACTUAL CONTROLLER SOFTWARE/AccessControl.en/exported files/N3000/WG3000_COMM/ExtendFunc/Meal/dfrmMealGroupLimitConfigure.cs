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
	// Token: 0x020002F7 RID: 759
	public partial class dfrmMealGroupLimitConfigure : frmN3000
	{
		// Token: 0x06001617 RID: 5655 RVA: 0x001BE79F File Offset: 0x001BD79F
		public dfrmMealGroupLimitConfigure()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvGroups);
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x001BE7C3 File Offset: 0x001BD7C3
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x001BE7CC File Offset: 0x001BD7CC
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
				using (dfrmMealGroupLimit dfrmMealGroupLimit = new dfrmMealGroupLimit())
				{
					DataTable table = ((DataView)dataGridView.DataSource).Table;
					if (dataGridView.SelectedRows.Count == 1)
					{
						int num2 = (int)dataGridView.Rows[num].Cells[0].Value;
						DataRow dataRow = table.Rows.Find(num2);
						if (dataRow != null)
						{
							dfrmMealGroupLimit.morning = decimal.Parse(dataRow["f_morning"].ToString(), CultureInfo.InvariantCulture);
							dfrmMealGroupLimit.lunch = decimal.Parse(dataRow["f_lunch"].ToString(), CultureInfo.InvariantCulture);
							dfrmMealGroupLimit.evening = decimal.Parse(dataRow["f_evening"].ToString(), CultureInfo.InvariantCulture);
							dfrmMealGroupLimit.other = decimal.Parse(dataRow["f_other"].ToString(), CultureInfo.InvariantCulture);
							dfrmMealGroupLimit.morning = decimal.Parse(dataRow["f_morning"].ToString(), CultureInfo.InvariantCulture);
							dfrmMealGroupLimit.enable = (int)dataRow["f_Enabled"];
						}
					}
					if (dfrmMealGroupLimit.ShowDialog(this) == DialogResult.OK)
					{
						for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
						{
							num = dataGridView.SelectedRows[i].Index;
							int num3 = (int)dataGridView.Rows[num].Cells[0].Value;
							DataRow dataRow = table.Rows.Find(num3);
							if (dataRow != null)
							{
								dataRow["f_morning"] = string.Format("{0}", dfrmMealGroupLimit.morning);
								dataRow["f_lunch"] = string.Format("{0}", dfrmMealGroupLimit.lunch);
								dataRow["f_evening"] = string.Format("{0}", dfrmMealGroupLimit.evening);
								dataRow["f_other"] = string.Format("{0}", dfrmMealGroupLimit.other);
								dataRow["f_Enabled"] = dfrmMealGroupLimit.enable;
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

		// Token: 0x0600161A RID: 5658 RVA: 0x001BEABC File Offset: 0x001BDABC
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
				string text = "DELETE from t_b_group4MealLimit";
				dbConnection.Open();
				dbCommand.CommandText = text;
				dbCommand.ExecuteNonQuery();
				for (int i = 0; i <= this.dvCheckAccess.Count - 1; i++)
				{
					text = "INSERT INTO t_b_group4MealLimit (f_GroupID,f_Enabled, f_Morning,f_Lunch,f_Evening,f_Other)";
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

		// Token: 0x0600161B RID: 5659 RVA: 0x001BECB4 File Offset: 0x001BDCB4
		private void dfrmCheckAccessSetup_Load(object sender, EventArgs e)
		{
			this.GroupName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.GroupName.HeaderText);
			try
			{
				string text;
				try
				{
					text = "CREATE TABLE t_b_group4MealLimit ( ";
					if (wgAppConfig.IsAccessDB)
					{
						text += "f_Id AUTOINCREMENT NOT NULL ,f_GroupID int  NOT NULL  DEFAULT 0 ,f_Morning  Numeric(10,0)    NOT NULL  DEFAULT 0 ,f_Lunch  Numeric(10,0)    NOT NULL  DEFAULT 0 ,f_Evening  Numeric(10,0)    NOT NULL  DEFAULT 0 ,f_Other  Numeric(10,0)    NOT NULL  DEFAULT 0 ,f_Enabled    int  NOT NULL Default 0  ,f_Notes MEMO )";
					}
					else
					{
						text += "f_Id         [int] IDENTITY (1, 1) NOT NULL  ,f_GroupID int  NOT NULL  DEFAULT 0 ,f_Morning  Numeric(10,0)    NOT NULL  DEFAULT 0 ,f_Lunch  Numeric(10,0)    NOT NULL  DEFAULT 0 ,f_Evening  Numeric(10,0)    NOT NULL  DEFAULT 0 ,f_Other  Numeric(10,0)    NOT NULL  DEFAULT 0 ,f_Enabled    [int]  NOT NULL Default(0)  ,f_Notes      [ntext]  NULL  )";
					}
					wgAppConfig.runUpdateSql(text);
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				text = " SELECT a.f_GroupID,a.f_GroupName,b.f_Enabled, b.f_Morning,b.f_Lunch, b.f_Evening,b.f_Other from t_b_Group a LEFT JOIN t_b_group4MealLimit b ON a.f_GroupID = b.f_GroupID order by f_GroupName  + '\\' ASC";
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
						goto IL_011C;
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
					IL_011C:
					this.dsCheckAccess.Tables["groups"].PrimaryKey = new DataColumn[] { this.dsCheckAccess.Tables["groups"].Columns[0] };
				}
				catch (Exception ex2)
				{
					wgAppConfig.wgLog(ex2.ToString());
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
						this.dvCheckAccess[i]["f_Enabled"] = 0;
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
			catch (Exception ex3)
			{
				wgAppConfig.wgLog(ex3.ToString());
			}
			this.dgvGroups.DefaultCellStyle.ForeColor = SystemColors.WindowText;
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x001BF0B8 File Offset: 0x001BE0B8
		private void dgvGroups_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x04002DD1 RID: 11729
		private DataSet dsCheckAccess = new DataSet();

		// Token: 0x04002DD7 RID: 11735
		private DataView dvCheckAccess;
	}
}
