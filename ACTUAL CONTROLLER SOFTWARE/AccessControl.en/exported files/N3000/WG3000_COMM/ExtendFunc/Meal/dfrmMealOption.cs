using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002F8 RID: 760
	public partial class dfrmMealOption : frmN3000
	{
		// Token: 0x0600161F RID: 5663 RVA: 0x001BF7E8 File Offset: 0x001BE7E8
		public dfrmMealOption()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvOptional);
			wgAppConfig.custDataGridview(ref this.dgvSelected);
			this.tabPage1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x001BF854 File Offset: 0x001BE854
		private void btnAddAllReaders_Click(object sender, EventArgs e)
		{
			this.dgvOptional.SelectAll();
			wgAppConfig.selectObject(this.dgvOptional, "f_cost", this.nudCost.Value.ToString());
			this.dgvOptional.ClearSelection();
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x001BF89C File Offset: 0x001BE89C
		private void btnAddOneReader_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvOptional, "f_cost", this.nudCost.Value.ToString());
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x001BF8CC File Offset: 0x001BE8CC
		private void btnDeleteAllReaders_Click(object sender, EventArgs e)
		{
			this.dgvSelected.SelectAll();
			wgAppConfig.deselectObject(this.dgvSelected);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x001BF8E4 File Offset: 0x001BE8E4
		private void btnDeleteOneReader_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelected);
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x001BF8F1 File Offset: 0x001BE8F1
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x001BF8F9 File Offset: 0x001BE8F9
		private void cmdOK_Click(object sender, EventArgs e)
		{
			this.cmdOK_Click_Acc(sender, e);
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x001BF904 File Offset: 0x001BE904
		private void cmdOK_Click_Acc(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				Cursor cursor = Cursor.Current;
				wgAppConfig.runUpdateSql(string.Format(" Update t_d_Reader4Meal SET {0} = -1", this.strMealCon));
				if (this.dvSelected.Count > 0)
				{
					for (int i = 0; i <= this.dvSelected.Count - 1; i++)
					{
						wgAppConfig.runUpdateSql(string.Format(" Update t_d_Reader4Meal SET {0} = {1} WHERE f_ReaderID ={2}", this.strMealCon, this.dvSelected[i]["f_Cost"].ToString(), this.dvSelected[i]["f_ReaderID"].ToString()));
					}
				}
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
			this.Cursor = Cursors.Default;
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x001BF9FC File Offset: 0x001BE9FC
		private void dfrmMealOption_Load(object sender, EventArgs e)
		{
			switch (this.mealNo)
			{
			case 0:
				this.strMealCon = "f_CostMorning";
				break;
			case 1:
				this.strMealCon = "f_CostLunch";
				break;
			case 2:
				this.strMealCon = "f_CostEvening";
				break;
			case 3:
				this.strMealCon = "f_CostOther";
				break;
			default:
				return;
			}
			this.loadData();
			if (this.dgvOptional.Rows.Count == 0 && this.dgvSelected.Rows.Count == 0)
			{
				XMessageBox.Show(CommonStr.strMealPremote);
				base.Close();
			}
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x001BFA94 File Offset: 0x001BEA94
		public void loadData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadData_Acc();
				return;
			}
			SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
			try
			{
				SqlCommand sqlCommand = new SqlCommand(string.Format("Select t_d_Reader4Meal.f_ReaderID, f_ReaderName, CASE WHEN {0}  IS NULL  THEN 0 ELSE (CASE WHEN {0} >=0 THEN 1 ELSE 0 END ) END  as f_Selected,{0} as f_Cost from t_b_reader,t_d_Reader4Meal  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_Reader4Meal.f_ReaderID ", this.strMealCon), sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(this.ds, "optionalReader");
				this.dv = new DataView(this.ds.Tables["optionalReader"]);
				this.dv.RowFilter = " f_Selected = 0";
				this.dvSelected = new DataView(this.ds.Tables["optionalReader"]);
				this.dvSelected.RowFilter = " f_Selected = 1";
				this.dt = this.ds.Tables["optionalReader"];
				try
				{
					this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
				}
				catch (Exception)
				{
					throw;
				}
				for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
				{
					this.dgvOptional.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvSelected.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
				}
				this.dv.RowFilter = "f_Selected = 0";
				this.dvSelected.RowFilter = "f_Selected > 0";
				this.dgvOptional.AutoGenerateColumns = false;
				this.dgvOptional.DataSource = this.dv;
				this.dgvSelected.AutoGenerateColumns = false;
				this.dgvSelected.DataSource = this.dvSelected;
				this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
				this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x001BFCD4 File Offset: 0x001BECD4
		public void loadData_Acc()
		{
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			try
			{
				OleDbCommand oleDbCommand = new OleDbCommand(string.Format("Select t_d_Reader4Meal.f_ReaderID, f_ReaderName, IIF(ISNULL({0}),0, IIF({0} >=0,1,0)) as f_Selected,{0} as f_Cost from t_b_reader,t_d_Reader4Meal  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_Reader4Meal.f_ReaderID ", this.strMealCon), oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "optionalReader");
				this.dv = new DataView(this.ds.Tables["optionalReader"]);
				this.dv.RowFilter = " f_Selected = 0";
				this.dvSelected = new DataView(this.ds.Tables["optionalReader"]);
				this.dvSelected.RowFilter = " f_Selected = 1";
				this.dt = this.ds.Tables["optionalReader"];
				try
				{
					this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
				}
				catch (Exception)
				{
					throw;
				}
				for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
				{
					this.dgvOptional.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvSelected.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
				}
				this.dv.RowFilter = "f_Selected = 0";
				this.dvSelected.RowFilter = "f_Selected > 0";
				this.dgvOptional.AutoGenerateColumns = false;
				this.dgvOptional.DataSource = this.dv;
				this.dgvSelected.AutoGenerateColumns = false;
				this.dgvSelected.DataSource = this.dvSelected;
				this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
				this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x04002DE0 RID: 11744
		private DataTable dt;

		// Token: 0x04002DE1 RID: 11745
		private DataView dv;

		// Token: 0x04002DE2 RID: 11746
		private DataView dvSelected;

		// Token: 0x04002DE3 RID: 11747
		private DataSet ds = new DataSet("dsMeal");

		// Token: 0x04002DE4 RID: 11748
		public int mealNo = -1;

		// Token: 0x04002DE5 RID: 11749
		private string strMealCon = "";
	}
}
