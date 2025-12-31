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

namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x0200030B RID: 779
	public partial class dfrmPatrolTaskEdit : frmN3000
	{
		// Token: 0x06001793 RID: 6035 RVA: 0x001EACB8 File Offset: 0x001E9CB8
		public dfrmPatrolTaskEdit()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x001EAD04 File Offset: 0x001E9D04
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x001EAD13 File Offset: 0x001E9D13
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.routeID = int.Parse(this.arrRouteID[this.cbof_route.SelectedIndex].ToString());
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x001EAD48 File Offset: 0x001E9D48
		private void dfrmShiftEdit_Load(object sender, EventArgs e)
		{
			this.dsConsumers = new DataSet("Users");
			string text = "";
			if (wgAppConfig.IsAccessDB)
			{
				text += " SELECT    IIF( [f_RouteName] IS NULL , CSTR([f_RouteID])     , CSTR([f_RouteID]) + '-' + [f_RouteName]     ) AS f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC  ";
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
			text += " SELECT    CASE WHEN [f_RouteName] IS NULL THEN CONVERT(nvarchar(50),[f_RouteID])     ELSE CONVERT(nvarchar(50),[f_RouteID]) + '-' + [f_RouteName]     END AS f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC  ";
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
			this.arrRouteID.Clear();
			this.cbof_route.Items.Clear();
			this.arrRouteID.Add("0");
			this.cbof_route.Items.Add("0*-" + CommonStr.strPatrolEventRest);
			if (this.dtOptionalShift.Rows.Count > 0)
			{
				for (int i = 0; i <= this.dtOptionalShift.Rows.Count - 1; i++)
				{
					this.cbof_route.Items.Add(this.dtOptionalShift.Rows[i][0]);
					this.arrRouteID.Add(this.dtOptionalShift.Rows[i][1]);
				}
			}
			if (this.cbof_route.Items.Count > 0)
			{
				this.cbof_route.SelectedIndex = 0;
			}
		}

		// Token: 0x04003094 RID: 12436
		private DataSet dsConsumers;

		// Token: 0x04003095 RID: 12437
		private DataTable dtOptionalShift;

		// Token: 0x04003096 RID: 12438
		private ArrayList arrConsumerCMIndex = new ArrayList();

		// Token: 0x04003097 RID: 12439
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04003098 RID: 12440
		private ArrayList arrRouteID = new ArrayList();

		// Token: 0x04003099 RID: 12441
		private ArrayList arrSelectedRouteID = new ArrayList();

		// Token: 0x0400309A RID: 12442
		public int routeID = -1;
	}
}
