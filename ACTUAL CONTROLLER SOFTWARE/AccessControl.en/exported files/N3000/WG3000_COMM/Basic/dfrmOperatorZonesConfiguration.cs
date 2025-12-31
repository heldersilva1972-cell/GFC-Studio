using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000022 RID: 34
	public partial class dfrmOperatorZonesConfiguration : frmN3000
	{
		// Token: 0x060001DE RID: 478 RVA: 0x0003F82D File Offset: 0x0003E82D
		public dfrmOperatorZonesConfiguration()
		{
			this.InitializeComponent();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0003F83C File Offset: 0x0003E83C
		private void _bindGroup()
		{
			try
			{
				this.lstOptionalGroups.DisplayMember = "f_ZoneName";
				this.lstOptionalGroups.DataSource = this.dtOptionalGroups;
				this.lstSelectedGroups.DisplayMember = "f_ZoneName";
				this.lstSelectedGroups.DataSource = this.dtSelectedGroups;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0003F8BC File Offset: 0x0003E8BC
		private void _dataTableLoad()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this._dataTableLoad_Acc();
				return;
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand("Select * from t_b_Controller_Zone where f_ZoneID IN (SELECT f_ZoneID FROM t_b_Controller_Zone4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", sqlConnection))
				{
					using (SqlCommand sqlCommand2 = new SqlCommand("Select * from t_b_Controller_Zone where f_ZoneID NOT IN (SELECT f_ZoneID FROM  t_b_Controller_Zone4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", sqlConnection))
					{
						this.ds = new DataSet("Users-Doors");
						this.daSelectedGroup = new SqlDataAdapter(sqlCommand);
						this.daOptionalGroup = new SqlDataAdapter(sqlCommand2);
						try
						{
							this.ds.Clear();
							this.daOptionalGroup.Fill(this.ds, "OptionalGroups");
							this.daSelectedGroup.Fill(this.ds, "SelectedGroups");
							this.dtOptionalGroups = new DataTable();
							this.dtOptionalGroups = this.ds.Tables["OptionalGroups"].Copy();
							this.dtSelectedGroups = new DataTable();
							this.dtSelectedGroups = this.ds.Tables["SelectedGroups"].Copy();
							this.lstOptionalGroups.DataSource = this.dtOptionalGroups;
							this.lstOptionalGroups.DisplayMember = "f_ZoneName";
							this.lstSelectedGroups.DataSource = this.dtSelectedGroups;
							this.lstSelectedGroups.DisplayMember = "f_ZoneName";
							this.dtSelectedGroups.AcceptChanges();
							this.dtOptionalGroups.AcceptChanges();
						}
						catch (Exception ex)
						{
							wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
						}
					}
				}
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0003FAE0 File Offset: 0x0003EAE0
		private void _dataTableLoad_Acc()
		{
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand("Select * from t_b_Controller_Zone where f_ZoneID IN (SELECT f_ZoneID FROM t_b_Controller_Zone4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", oleDbConnection))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand("Select * from t_b_Controller_Zone where f_ZoneID NOT IN (SELECT f_ZoneID FROM  t_b_Controller_Zone4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", oleDbConnection))
					{
						this.ds = new DataSet("Users-Doors");
						OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand);
						OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(oleDbCommand2);
						try
						{
							this.ds.Clear();
							oleDbDataAdapter2.Fill(this.ds, "OptionalGroups");
							oleDbDataAdapter.Fill(this.ds, "SelectedGroups");
							this.dtOptionalGroups = new DataTable();
							this.dtOptionalGroups = this.ds.Tables["OptionalGroups"].Copy();
							this.dtSelectedGroups = new DataTable();
							this.dtSelectedGroups = this.ds.Tables["SelectedGroups"].Copy();
							this.lstOptionalGroups.DataSource = this.dtOptionalGroups;
							this.lstOptionalGroups.DisplayMember = "f_ZoneName";
							this.lstSelectedGroups.DataSource = this.dtSelectedGroups;
							this.lstSelectedGroups.DisplayMember = "f_ZoneName";
							this.dtSelectedGroups.AcceptChanges();
							this.dtOptionalGroups.AcceptChanges();
						}
						catch (Exception ex)
						{
							wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
						}
					}
				}
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0003FCE8 File Offset: 0x0003ECE8
		private void _unbindGroup()
		{
			try
			{
				this.lstOptionalGroups.DataSource = null;
				this.lstOptionalGroups.DisplayMember = null;
				this.lstSelectedGroups.DataSource = null;
				this.lstSelectedGroups.DisplayMember = null;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0003FD58 File Offset: 0x0003ED58
		private void btnAddAllGroups_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor cursor = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;
				this._unbindGroup();
				DataTable dataTable = this.dtOptionalGroups;
				DataTable dataTable2 = this.dtSelectedGroups;
				for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
				{
					dataTable2.ImportRow(dataTable.Rows[i]);
				}
				dataTable.Clear();
				dataTable2.AcceptChanges();
				dataTable.AcceptChanges();
				this.lstSelectedGroups.Refresh();
				this.lstOptionalGroups.Refresh();
				this._bindGroup();
				Cursor.Current = cursor;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0003FE20 File Offset: 0x0003EE20
		private void btnAddOneGroup_Click(object sender, EventArgs e)
		{
			dfrmOperatorZonesConfiguration.lst_UpdateOne(this.dtOptionalGroups, this.dtSelectedGroups, this.lstOptionalGroups, this.lstSelectedGroups);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0003FE3F File Offset: 0x0003EE3F
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0003FE48 File Offset: 0x0003EE48
		private void btnDeleteAllGroups_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor cursor = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;
				this._unbindGroup();
				DataTable dataTable = this.dtSelectedGroups;
				DataTable dataTable2 = this.dtOptionalGroups;
				for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
				{
					dataTable2.ImportRow(dataTable.Rows[i]);
				}
				dataTable.Clear();
				dataTable.AcceptChanges();
				dataTable2.AcceptChanges();
				this.lstSelectedGroups.Refresh();
				this.lstOptionalGroups.Refresh();
				this._bindGroup();
				Cursor.Current = cursor;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0003FF10 File Offset: 0x0003EF10
		private void btnDeleteOneGroup_Click(object sender, EventArgs e)
		{
			dfrmOperatorZonesConfiguration.lst_UpdateOne(this.dtSelectedGroups, this.dtOptionalGroups, this.lstSelectedGroups, this.lstOptionalGroups);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0003FF30 File Offset: 0x0003EF30
		private void btnOk_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				Cursor cursor = Cursor.Current;
				DataTable dataTable = this.dtSelectedGroups;
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_Controller_Zone4Operator Where f_OperatorId = " + this.operatorId);
				if (dataTable.Rows.Count > 0)
				{
					for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
					{
						wgAppConfig.runUpdateSql(string.Concat(new object[]
						{
							" INSERT INTO t_b_Controller_Zone4Operator (f_ZoneID, f_OperatorID)  Values(",
							dataTable.Rows[i]["f_ZoneID"],
							" ,",
							this.operatorId,
							" )"
						}));
					}
				}
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			this.Cursor = Cursors.Default;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00040030 File Offset: 0x0003F030
		private void btnZoneManage_Click(object sender, EventArgs e)
		{
			using (frmZones frmZones = new frmZones())
			{
				frmZones.ShowDialog(this);
			}
			this._dataTableLoad();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00040070 File Offset: 0x0003F070
		private void dfrmSwitchGroupsConfiguration_Load(object sender, EventArgs e)
		{
			this._dataTableLoad();
			this.btnZoneManage.Visible = false;
			bool flag = false;
			string text = "btnZoneManage";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && !flag)
			{
				this.btnZoneManage.Visible = true;
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000400B0 File Offset: 0x0003F0B0
		public static void lst_UpdateOne(DataTable dtSource, DataTable dtDestine, ListBox lstSrc, ListBox lstDest)
		{
			try
			{
				object dataSource = lstDest.DataSource;
				string displayMember = lstDest.DisplayMember;
				lstDest.DisplayMember = null;
				lstDest.DataSource = null;
				Cursor cursor = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;
				try
				{
					if (lstSrc.SelectedIndices.Count > 0)
					{
						DataTable dataTable = dtDestine.Copy();
						dataTable.Rows.Clear();
						int num = lstSrc.SelectedIndices.Count - 1;
						int[] array = new int[num + 1];
						for (int i = 0; i <= num; i++)
						{
							array[i] = lstSrc.SelectedIndices[num - i];
						}
						for (int i = 0; i <= num; i++)
						{
							int num2 = array[i];
							if (num2 >= 0)
							{
								DataRow dataRow = dtSource.Rows[num2];
								dataTable.ImportRow(dataRow);
								dtSource.Rows.Remove(dataRow);
								dtSource.AcceptChanges();
							}
						}
						dataTable.AcceptChanges();
						for (int i = 0; i <= num; i++)
						{
							dtDestine.ImportRow(dataTable.Rows[num - i]);
						}
						dtSource.AcceptChanges();
						dtDestine.AcceptChanges();
						lstSrc.Refresh();
						lstDest.Refresh();
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				}
				lstDest.DisplayMember = displayMember;
				lstDest.DataSource = dataSource;
				Cursor.Current = cursor;
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x04000407 RID: 1031
		private SqlDataAdapter daOptionalGroup;

		// Token: 0x04000408 RID: 1032
		private SqlDataAdapter daSelectedGroup;

		// Token: 0x04000409 RID: 1033
		private DataSet ds;

		// Token: 0x0400040A RID: 1034
		private DataTable dtOptionalGroups;

		// Token: 0x0400040B RID: 1035
		private DataTable dtSelectedGroups;

		// Token: 0x0400040C RID: 1036
		public int operatorId;
	}
}
