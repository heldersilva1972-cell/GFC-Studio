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
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200002E RID: 46
	public partial class dfrmSystemParam : frmN3000
	{
		// Token: 0x06000337 RID: 823 RVA: 0x0005EAC0 File Offset: 0x0005DAC0
		public dfrmSystemParam()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0005EAD9 File Offset: 0x0005DAD9
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0005EAE8 File Offset: 0x0005DAE8
		private void btnOK_Click(object sender, EventArgs e)
		{
			DataTable table = (this.dataGridView1.DataSource as DataView).Table;
			for (int i = 0; i <= table.Rows.Count - 1; i++)
			{
				if (wgTools.SetObjToStr(table.Rows[i]["f_Value"]) != wgTools.SetObjToStr(table.Rows[i]["f_OldValue"]))
				{
					wgAppConfig.runUpdateSql(string.Concat(new string[]
					{
						" UPDATE t_a_SystemParam SET  f_Value = ",
						wgTools.PrepareStrNUnicode(table.Rows[i]["f_Value"].ToString()),
						" , f_Modified = ",
						wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
						" WHERE f_NO = ",
						table.Rows[i]["f_NO"].ToString()
					}));
				}
			}
			if (XMessageBox.Show(CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				base.DialogResult = DialogResult.OK;
			}
			else
			{
				base.DialogResult = DialogResult.Cancel;
			}
			base.Close();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0005EC18 File Offset: 0x0005DC18
		private void dfrmSystemParam_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				for (int i = 0; i < this.dataGridView1.ColumnCount; i++)
				{
					this.dataGridView1.Columns[i].Visible = true;
				}
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0005EC89 File Offset: 0x0005DC89
		private void dfrmSystemParam_Load(object sender, EventArgs e)
		{
			this.fillSystemParam();
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0005EC94 File Offset: 0x0005DC94
		private void fillSystemParam()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.fillSystemParam_Acc();
				return;
			}
			string text = " SELECT ";
			text += " f_NO, f_Name, f_Value, f_EName, f_Notes, f_Modified, f_Value as f_OldValue  FROM t_a_SystemParam  ORDER BY [f_NO] ";
			this.dt = new DataTable();
			this.dv = new DataView(this.dt);
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
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.DataSource = this.dv;
			int num = 0;
			while (num < this.dv.Table.Columns.Count && num < this.dataGridView1.ColumnCount)
			{
				this.dataGridView1.Columns[num].DataPropertyName = this.dv.Table.Columns[num].ColumnName;
				num++;
			}
			this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0005EDE8 File Offset: 0x0005DDE8
		private void fillSystemParam_Acc()
		{
			string text = " SELECT ";
			text += " f_NO, f_Name, f_Value, f_EName, f_Notes, f_Modified, f_Value as f_OldValue  FROM t_a_SystemParam  ORDER BY [f_NO] ";
			this.dt = new DataTable();
			this.dv = new DataView(this.dt);
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
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.DataSource = this.dv;
			int num = 0;
			while (num < this.dv.Table.Columns.Count && num < this.dataGridView1.ColumnCount)
			{
				this.dataGridView1.Columns[num].DataPropertyName = this.dv.Table.Columns[num].ColumnName;
				num++;
			}
			this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
		}

		// Token: 0x04000628 RID: 1576
		private DataTable dt;

		// Token: 0x04000629 RID: 1577
		private DataView dv;
	}
}
