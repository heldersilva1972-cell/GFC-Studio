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

namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x0200031A RID: 794
	public partial class dfrmPrivilegeTypeDoorsType : frmN3000
	{
		// Token: 0x06001882 RID: 6274 RVA: 0x00200416 File Offset: 0x001FF416
		public dfrmPrivilegeTypeDoorsType()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvPrivilegeTypes);
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00200430 File Offset: 0x001FF430
		private void _loadData()
		{
			string text = " SELECT t_d_Privilege_Of_PrivilegeType.f_PrivilegeRecID,  [f_PrivilegeTypeName],  f_DoorID ";
			text += " FROM t_d_Privilege_Of_PrivilegeType,t_d_PrivilegeType   WHERE t_d_Privilege_Of_PrivilegeType.f_ConsumerID=  t_d_PrivilegeType.f_PrivilegeTypeID   ORDER BY f_PrivilegeTypeName  ";
			this.dt = new DataTable("t_d_PrivilegeType");
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
					goto IL_00CF;
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
			IL_00CF:
			DataView dataView = new DataView(this.dt);
			this.dvtemp = new DataView(this.dt);
			this.dgvPrivilegeTypes.AutoGenerateColumns = false;
			this.dgvPrivilegeTypes.DataSource = dataView;
			int num = 0;
			while (num < dataView.Table.Columns.Count && num < this.dgvPrivilegeTypes.ColumnCount)
			{
				this.dgvPrivilegeTypes.Columns[num].DataPropertyName = dataView.Table.Columns[num].ColumnName;
				num++;
			}
			this.dgvPrivilegeTypes.DefaultCellStyle.ForeColor = SystemColors.WindowText;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x00200600 File Offset: 0x001FF600
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00200608 File Offset: 0x001FF608
		private void cboDoor_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.cboDoor.Text != "")
				{
					this.dvDoors4Watching.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboDoor.Text);
					if (this.dvDoors4Watching.Count > 0)
					{
						(this.dgvPrivilegeTypes.DataSource as DataView).RowFilter = " f_DoorID =" + this.dvDoors4Watching[0]["f_DoorID"].ToString();
					}
					else
					{
						(this.dgvPrivilegeTypes.DataSource as DataView).RowFilter = " f_DoorID = 0";
					}
				}
				else
				{
					(this.dgvPrivilegeTypes.DataSource as DataView).RowFilter = " f_DoorID = 0";
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x002006F4 File Offset: 0x001FF6F4
		private void dfrmPrivilegeTypeDoorsType_Load(object sender, EventArgs e)
		{
			this._loadData();
			(this.dgvPrivilegeTypes.DataSource as DataView).RowFilter = " f_DoorID = 0";
			this.loadDoorData();
			if (this.cboDoor.Items.Count > 0)
			{
				this.cboDoor.SelectedIndex = 0;
			}
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00200748 File Offset: 0x001FF748
		private void loadDoorData()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, a.f_ControllerID , b.f_ZoneID ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
			DataTable dataTable = new DataTable();
			this.dvDoors = new DataView(dataTable);
			this.dvDoors4Watching = new DataView(dataTable);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(dataTable);
						}
					}
					goto IL_00D7;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(dataTable);
					}
				}
			}
			IL_00D7:
			icControllerZone icControllerZone = new icControllerZone();
			icControllerZone.getAllowedControllers(ref dataTable);
			try
			{
				dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[0] };
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.cboDoor.Items.Clear();
			if (this.dvDoors.Count > 0)
			{
				for (int i = 0; i < this.dvDoors.Count; i++)
				{
					this.cboDoor.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
				}
			}
		}

		// Token: 0x04003248 RID: 12872
		private DataTable dt;

		// Token: 0x04003249 RID: 12873
		private DataView dvDoors;

		// Token: 0x0400324A RID: 12874
		private DataView dvDoors4Watching;

		// Token: 0x0400324B RID: 12875
		private DataView dvtemp;
	}
}
