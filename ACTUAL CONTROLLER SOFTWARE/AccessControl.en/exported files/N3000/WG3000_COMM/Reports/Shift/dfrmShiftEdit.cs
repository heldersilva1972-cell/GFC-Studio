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
	// Token: 0x02000371 RID: 881
	public partial class dfrmShiftEdit : frmN3000
	{
		// Token: 0x06001CDB RID: 7387 RVA: 0x00261514 File Offset: 0x00260514
		public dfrmShiftEdit()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x00261560 File Offset: 0x00260560
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x0026156F File Offset: 0x0026056F
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.shiftid = int.Parse(this.arrShiftID[this.cbof_shift.SelectedIndex].ToString());
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x002615A4 File Offset: 0x002605A4
		private void dfrmShiftEdit_Load(object sender, EventArgs e)
		{
			this.dsConsumers = new DataSet("Users");
			string text = "";
			if (wgAppConfig.IsAccessDB)
			{
				text += " SELECT    IIF( [f_ShiftName] IS NULL , CSTR([f_ShiftID])     , CSTR([f_ShiftID]) + '-' + [f_ShiftName]     ) AS f_ShiftFullName, [f_ShiftID] from t_b_ShiftSet order by f_ShiftID ASC  ";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dsConsumers, "OptionalShift");
						}
					}
					goto IL_00E1;
				}
			}
			text += " SELECT    CASE WHEN [f_ShiftName] IS NULL THEN CONVERT(nvarchar(50),[f_ShiftID])     ELSE CONVERT(nvarchar(50),[f_ShiftID]) + '-' + [f_ShiftName]     END AS f_ShiftFullName, [f_ShiftID] from t_b_ShiftSet order by f_ShiftID ASC  ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dsConsumers, "OptionalShift");
					}
				}
			}
			IL_00E1:
			this.dtOptionalShift = this.dsConsumers.Tables["OptionalShift"];
			this.arrShiftID.Clear();
			this.cbof_shift.Items.Clear();
			this.arrShiftID.Add("0");
			this.cbof_shift.Items.Add("0*-" + CommonStr.strRest);
			if (this.dtOptionalShift.Rows.Count > 0)
			{
				for (int i = 0; i <= this.dtOptionalShift.Rows.Count - 1; i++)
				{
					this.cbof_shift.Items.Add(this.dtOptionalShift.Rows[i][0]);
					this.arrShiftID.Add(this.dtOptionalShift.Rows[i][1]);
				}
			}
			if (this.cbof_shift.Items.Count > 0)
			{
				this.cbof_shift.SelectedIndex = 0;
			}
		}

		// Token: 0x04003779 RID: 14201
		private DataSet dsConsumers;

		// Token: 0x0400377A RID: 14202
		private DataTable dtOptionalShift;

		// Token: 0x0400377B RID: 14203
		private ArrayList arrConsumerCMIndex = new ArrayList();

		// Token: 0x0400377C RID: 14204
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x0400377D RID: 14205
		private ArrayList arrSelectedShiftID = new ArrayList();

		// Token: 0x0400377E RID: 14206
		private ArrayList arrShiftID = new ArrayList();

		// Token: 0x0400377F RID: 14207
		public int shiftid = -1;
	}
}
