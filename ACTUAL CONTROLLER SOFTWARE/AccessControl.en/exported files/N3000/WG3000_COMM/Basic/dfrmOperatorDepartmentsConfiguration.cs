using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000020 RID: 32
	public partial class dfrmOperatorDepartmentsConfiguration : frmN3000
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x0003DD0A File Offset: 0x0003CD0A
		public dfrmOperatorDepartmentsConfiguration()
		{
			this.InitializeComponent();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0003DD18 File Offset: 0x0003CD18
		private void _bindGroup()
		{
			try
			{
				this.lstOptionalGroups.DisplayMember = "f_GroupName";
				this.lstOptionalGroups.DataSource = this.dtOptionalGroups;
				this.lstSelectedGroups.DisplayMember = "f_GroupName";
				this.lstSelectedGroups.DataSource = this.dtSelectedGroups;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0003DD98 File Offset: 0x0003CD98
		private void _dataTableLoad()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this._dataTableLoad_Acc();
				return;
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand("Select * from t_b_Group where f_GroupID IN (SELECT f_GroupID FROM t_b_Group4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", sqlConnection))
				{
					using (SqlCommand sqlCommand2 = new SqlCommand("Select * from t_b_Group where f_GroupID NOT IN (SELECT f_GroupID FROM  t_b_Group4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", sqlConnection))
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
							this.lstOptionalGroups.DisplayMember = "f_GroupName";
							this.lstSelectedGroups.DataSource = this.dtSelectedGroups;
							this.lstSelectedGroups.DisplayMember = "f_GroupName";
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

		// Token: 0x060001C8 RID: 456 RVA: 0x0003DFBC File Offset: 0x0003CFBC
		private void _dataTableLoad_Acc()
		{
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand("Select * from t_b_Group where f_GroupID IN (SELECT f_GroupID FROM t_b_Group4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", oleDbConnection))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand("Select * from t_b_Group where f_GroupID NOT IN (SELECT f_GroupID FROM  t_b_Group4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", oleDbConnection))
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
							this.lstOptionalGroups.DisplayMember = "f_GroupName";
							this.lstSelectedGroups.DataSource = this.dtSelectedGroups;
							this.lstSelectedGroups.DisplayMember = "f_GroupName";
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

		// Token: 0x060001C9 RID: 457 RVA: 0x0003E1C4 File Offset: 0x0003D1C4
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

		// Token: 0x060001CA RID: 458 RVA: 0x0003E234 File Offset: 0x0003D234
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

		// Token: 0x060001CB RID: 459 RVA: 0x0003E2FC File Offset: 0x0003D2FC
		private void btnAddOneGroup_Click(object sender, EventArgs e)
		{
			dfrmOperatorDepartmentsConfiguration.lst_UpdateOne(this.dtOptionalGroups, this.dtSelectedGroups, this.lstOptionalGroups, this.lstSelectedGroups);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0003E31B File Offset: 0x0003D31B
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0003E324 File Offset: 0x0003D324
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

		// Token: 0x060001CE RID: 462 RVA: 0x0003E3EC File Offset: 0x0003D3EC
		private void btnDeleteOneGroup_Click(object sender, EventArgs e)
		{
			dfrmOperatorDepartmentsConfiguration.lst_UpdateOne(this.dtSelectedGroups, this.dtOptionalGroups, this.lstSelectedGroups, this.lstOptionalGroups);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0003E40C File Offset: 0x0003D40C
		private void btnOk_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				Cursor cursor = Cursor.Current;
				DataTable dataTable = this.dtSelectedGroups;
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_Group4Operator Where f_OperatorId = " + this.operatorId);
				if (dataTable.Rows.Count > 0)
				{
					for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
					{
						wgAppConfig.runUpdateSql(string.Concat(new object[]
						{
							" INSERT INTO t_b_Group4Operator (f_GroupID, f_OperatorID)  Values(",
							dataTable.Rows[i]["f_GroupID"],
							" ,",
							this.operatorId,
							" )"
						}));
					}
				}
				base.Close();
				return;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			this.Cursor = Cursors.Default;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0003E50C File Offset: 0x0003D50C
		private void dfrmSwitchGroupsConfiguration_Load(object sender, EventArgs e)
		{
			this.Text = wgAppConfig.ReplaceFloorRoom(this.Text);
			this.Label10.Text = wgAppConfig.ReplaceFloorRoom(this.Label10.Text);
			this.Label11.Text = wgAppConfig.ReplaceFloorRoom(this.Label11.Text);
			this._dataTableLoad();
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0003E568 File Offset: 0x0003D568
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

		// Token: 0x040003EA RID: 1002
		private SqlDataAdapter daOptionalGroup;

		// Token: 0x040003EB RID: 1003
		private SqlDataAdapter daSelectedGroup;

		// Token: 0x040003EC RID: 1004
		private DataSet ds;

		// Token: 0x040003ED RID: 1005
		private DataTable dtOptionalGroups;

		// Token: 0x040003EE RID: 1006
		private DataTable dtSelectedGroups;

		// Token: 0x040003EF RID: 1007
		public int operatorId;
	}
}
