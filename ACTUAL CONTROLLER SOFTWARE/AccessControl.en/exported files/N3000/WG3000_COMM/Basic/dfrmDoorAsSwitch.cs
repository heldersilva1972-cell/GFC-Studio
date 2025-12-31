using System;
using System.Collections;
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
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000011 RID: 17
	public partial class dfrmDoorAsSwitch : frmN3000
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x0001E770 File Offset: 0x0001D770
		public dfrmDoorAsSwitch()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0001E7EC File Offset: 0x0001D7EC
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
			if (this.cbof_ZoneID.SelectedIndex <= 0 && this.cbof_ZoneID.Text == CommonStr.strAllZones)
			{
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if ((int)table.Rows[i]["f_Selected"] != 1)
					{
						table.Rows[i]["f_Selected"] = 1;
					}
				}
			}
			else
			{
				this.dvtmp = new DataView((this.dgvDoors.DataSource as DataView).Table);
				this.dvtmp.RowFilter = string.Format("  {0} ", this.strZoneFilter);
				for (int j = 0; j < this.dvtmp.Count; j++)
				{
					this.dvtmp[j]["f_Selected"] = 1;
				}
			}
			this.updateCount();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0001E8F9 File Offset: 0x0001D8F9
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			this.selectObject(this.dgvDoors);
			this.updateCount();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0001E910 File Offset: 0x0001D910
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			DataTable table = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				table.Rows[i]["f_Selected"] = 0;
			}
			this.updateCount();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0001E96B File Offset: 0x0001D96B
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.updateCount();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0001E97E File Offset: 0x0001D97E
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0001E990 File Offset: 0x0001D990
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				this.dtDoorTmpSelected = ((DataView)this.dgvSelectedDoors.DataSource).Table.Copy();
				this.dvDoorTmpSelected = new DataView(this.dtDoorTmpSelected);
				this.dvDoorTmpSelected.RowFilter = "f_Selected > 0";
				string text = "";
				foreach (object obj in this.dvSelected)
				{
					DataRowView dataRowView = (DataRowView)obj;
					if (text != "")
					{
						text += ",";
					}
					text += dataRowView["f_DoorID"].ToString();
				}
				wgAppConfig.runUpdateSql(string.Format(" UPDATE t_a_SystemParam SET f_Notes ={0} Where f_NO = {1}", wgTools.PrepareStrNUnicode(text), 146));
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0001EAA8 File Offset: 0x0001DAA8
		private void cbof_Zone_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_ZoneID, this.cbof_ZoneID.Text);
			if (this.dgvDoors.DataSource != null)
			{
				DataView dataView = (DataView)this.dgvDoors.DataSource;
				if (this.cbof_ZoneID.SelectedIndex < 0 || (this.cbof_ZoneID.SelectedIndex == 0 && (int)this.arrZoneID[0] == 0))
				{
					dataView.RowFilter = "f_Selected = 0";
					this.strZoneFilter = "";
				}
				else
				{
					dataView.RowFilter = "f_Selected = 0 AND f_ZoneID =" + this.arrZoneID[this.cbof_ZoneID.SelectedIndex];
					this.strZoneFilter = " f_ZoneID =" + this.arrZoneID[this.cbof_ZoneID.SelectedIndex];
					int num = (int)this.arrZoneID[this.cbof_ZoneID.SelectedIndex];
					int num2 = (int)this.arrZoneNO[this.cbof_ZoneID.SelectedIndex];
					int zoneChildMaxNo = icControllerZone.getZoneChildMaxNo(this.cbof_ZoneID.Text, this.arrZoneName, this.arrZoneNO);
					if (num2 > 0)
					{
						if (num2 >= zoneChildMaxNo)
						{
							dataView.RowFilter = string.Format("f_Selected = 0 AND f_ZoneID ={0:d} ", num);
							this.strZoneFilter = string.Format(" f_ZoneID ={0:d} ", num);
						}
						else
						{
							dataView.RowFilter = "f_Selected = 0 ";
							string zoneQuery = icGroup.getZoneQuery(num2, zoneChildMaxNo, this.arrZoneNO, this.arrZoneID);
							dataView.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", zoneQuery);
							this.strZoneFilter = string.Format("  {0} ", zoneQuery);
						}
					}
					dataView.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", this.strZoneFilter);
				}
				this.updateCount();
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0001EC78 File Offset: 0x0001DC78
		private void cbof_ZoneID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ZoneID);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0001EC88 File Offset: 0x0001DC88
		private void dfrmDoorAsSwitch_Load(object sender, EventArgs e)
		{
			try
			{
				this.loadZoneInfo();
				this.loadDoorData();
				this.loadPrivilegeData();
				this.updateCount();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0001ECD8 File Offset: 0x0001DCD8
		private void dfrmPrivilegeSingle_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0001ECDA File Offset: 0x0001DCDA
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0001ECE7 File Offset: 0x0001DCE7
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0001ECF4 File Offset: 0x0001DCF4
		private void loadControlSegData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadControlSegData_Acc();
				return;
			}
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
			this.controlSegNameList[0] = CommonStr.strFreeTime;
			this.controlSegIDList[0] = 1;
			string text = " SELECT ";
			text += " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,    CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),       ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50),      ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']')     END AS f_ControlSegID    FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					int num = 1;
					while (sqlDataReader.Read())
					{
						this.cbof_ControlSegID.Items.Add(sqlDataReader["f_ControlSegID"]);
						this.controlSegNameList[num] = (string)sqlDataReader["f_ControlSegID"];
						this.controlSegIDList[num] = (int)sqlDataReader["f_ControlSegIDBak"];
						num++;
					}
					sqlDataReader.Close();
				}
			}
			if (this.cbof_ControlSegID.Items.Count > 0)
			{
				this.cbof_ControlSegID.SelectedIndex = 0;
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0001EE38 File Offset: 0x0001DE38
		private void loadControlSegData_Acc()
		{
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
			this.controlSegNameList[0] = CommonStr.strFreeTime;
			this.controlSegIDList[0] = 1;
			string text = " SELECT ";
			text += " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak, ";
			if (wgAppConfig.IsAccessDB)
			{
				text += "  IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID   FROM [t_b_ControlSeg]  WHERE  [t_b_ControlSeg].[f_ControlSegID]>1  ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
			}
			else
			{
				text += "   CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),       ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50),      ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']')     END AS f_ControlSegID    FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
			}
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					int num = 1;
					while (oleDbDataReader.Read())
					{
						this.cbof_ControlSegID.Items.Add(oleDbDataReader["f_ControlSegID"]);
						this.controlSegNameList[num] = (string)oleDbDataReader["f_ControlSegID"];
						this.controlSegIDList[num] = (int)oleDbDataReader["f_ControlSegIDBak"];
						num++;
					}
					oleDbDataReader.Close();
				}
			}
			if (this.cbof_ControlSegID.Items.Count > 0)
			{
				this.cbof_ControlSegID.SelectedIndex = 0;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0001EF90 File Offset: 0x0001DF90
		private void loadDoorData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadDoorData_Acc();
				return;
			}
			string text = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, 1 as f_ControlSegID,' ' as f_ControlSegName, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						this.dt = new DataTable();
						this.dv = new DataView(this.dt);
						this.dvSelected = new DataView(this.dt);
						sqlDataAdapter.Fill(this.dt);
						try
						{
							this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
						this.dv.RowFilter = "f_Selected = 0";
						this.dvSelected.RowFilter = "f_Selected > 0";
						this.dgvDoors.AutoGenerateColumns = false;
						this.dgvDoors.DataSource = this.dv;
						this.dgvSelectedDoors.AutoGenerateColumns = false;
						this.dgvSelectedDoors.DataSource = this.dvSelected;
						for (int i = 0; i < this.dgvDoors.Columns.Count; i++)
						{
							this.dgvDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
							this.dgvSelectedDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
						}
					}
				}
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0001F1AC File Offset: 0x0001E1AC
		private void loadDoorData_Acc()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, 1 as f_ControlSegID,' ' as f_ControlSegName, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
					{
						this.dt = new DataTable();
						this.dv = new DataView(this.dt);
						this.dvSelected = new DataView(this.dt);
						oleDbDataAdapter.Fill(this.dt);
						try
						{
							this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
						this.dv.RowFilter = "f_Selected = 0";
						this.dvSelected.RowFilter = "f_Selected > 0";
						this.dgvDoors.AutoGenerateColumns = false;
						this.dgvDoors.DataSource = this.dv;
						this.dgvSelectedDoors.AutoGenerateColumns = false;
						this.dgvSelectedDoors.DataSource = this.dvSelected;
						for (int i = 0; i < this.dgvDoors.Columns.Count; i++)
						{
							this.dgvDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
							this.dgvSelectedDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
						}
					}
				}
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0001F3B8 File Offset: 0x0001E3B8
		private void loadPrivilegeData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadPrivilegeData_Acc();
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadPrivilegeData Start");
			string text = string.Format(" SELECT f_Notes FROM t_a_SystemParam Where f_NO = {0}", 146);
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					string text2 = wgTools.SetObjToStr(sqlCommand.ExecuteScalar());
					if (!string.IsNullOrEmpty(text2))
					{
						using (DataView dataView = new DataView(this.dt))
						{
							dataView.RowFilter = string.Format("f_DoorID IN ({0})", text2);
							for (int i = 0; i < dataView.Count; i++)
							{
								dataView[i]["f_Selected"] = 1;
							}
						}
					}
				}
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0001F4D0 File Offset: 0x0001E4D0
		private void loadPrivilegeData_Acc()
		{
			Cursor.Current = Cursors.WaitCursor;
			string text = string.Format(" SELECT f_Notes FROM t_a_SystemParam Where f_NO = {0}", 146);
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					string text2 = wgTools.SetObjToStr(oleDbCommand.ExecuteScalar());
					if (!string.IsNullOrEmpty(text2))
					{
						using (DataView dataView = new DataView(this.dt))
						{
							dataView.RowFilter = string.Format("f_DoorID IN ({0})", text2);
							for (int i = 0; i < dataView.Count; i++)
							{
								dataView[i]["f_Selected"] = 1;
							}
						}
					}
				}
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0001F5C8 File Offset: 0x0001E5C8
		private void loadZoneInfo()
		{
			new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
			int i = this.arrZoneID.Count;
			this.cbof_ZoneID.Items.Clear();
			for (i = 0; i < this.arrZoneID.Count; i++)
			{
				if (i == 0 && string.IsNullOrEmpty(this.arrZoneName[i].ToString()))
				{
					this.cbof_ZoneID.Items.Add(CommonStr.strAllZones);
				}
				else
				{
					this.cbof_ZoneID.Items.Add(this.arrZoneName[i].ToString());
				}
			}
			if (this.cbof_ZoneID.Items.Count > 0)
			{
				this.cbof_ZoneID.SelectedIndex = 0;
			}
			bool flag = true;
			this.label25.Visible = flag;
			this.cbof_ZoneID.Visible = flag;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0001F6B4 File Offset: 0x0001E6B4
		private void logOperate(object sender)
		{
			string text = this.Text;
			string text2 = "";
			for (int i = 0; i <= Math.Min(10, this.dgvSelectedDoors.RowCount) - 1; i++)
			{
				text2 = text2 + ((DataView)this.dgvSelectedDoors.DataSource)[i]["f_DoorName"] + ",";
			}
			if (this.dgvSelectedDoors.RowCount > 10)
			{
				object obj = text2;
				text2 = string.Concat(new object[]
				{
					obj,
					"......(",
					this.dgvSelectedDoors.RowCount,
					")"
				});
			}
			else
			{
				object obj2 = text2;
				text2 = string.Concat(new object[]
				{
					obj2,
					"(",
					this.dgvSelectedDoors.RowCount,
					")"
				});
			}
			wgAppConfig.wgLog(string.Format("{0}:[{1} => {2}]:{3} => {4}", new object[]
			{
				(sender as Button).Text.Replace("\r\n", ""),
				1,
				this.dgvSelectedDoors.RowCount.ToString(),
				text,
				text2
			}), EventLogEntryType.Information, null);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0001F80C File Offset: 0x0001E80C
		private void selectObject(DataGridView dgv)
		{
			try
			{
				int num;
				if (dgv.SelectedRows.Count <= 0)
				{
					if (dgv.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dgv.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dgv.SelectedRows[0].Index;
				}
				DataTable table = ((DataView)dgv.DataSource).Table;
				if (dgv.SelectedRows.Count > 0)
				{
					int count = dgv.SelectedRows.Count;
					int[] array = new int[count];
					for (int i = 0; i < dgv.SelectedRows.Count; i++)
					{
						array[i] = (int)dgv.SelectedRows[i].Cells[0].Value;
					}
					for (int j = 0; j < count; j++)
					{
						int num2 = array[j];
						DataRow dataRow = table.Rows.Find(num2);
						if (dataRow != null)
						{
							dataRow["f_Selected"] = 1;
							dataRow["f_ControlSegID"] = this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex];
							dataRow["f_ControlSegName"] = this.controlSegNameList[this.cbof_ControlSegID.SelectedIndex];
						}
					}
				}
				else
				{
					int num3 = (int)dgv.Rows[num].Cells[0].Value;
					DataRow dataRow = table.Rows.Find(num3);
					if (dataRow != null)
					{
						dataRow["f_Selected"] = 1;
						dataRow["f_ControlSegID"] = this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex];
						dataRow["f_ControlSegName"] = this.controlSegNameList[this.cbof_ControlSegID.SelectedIndex];
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0001FA10 File Offset: 0x0001EA10
		private void updateCount()
		{
			this.lblOptional.Text = this.dgvDoors.RowCount.ToString();
			this.lblSeleted.Text = this.dgvSelectedDoors.RowCount.ToString();
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000DD RID: 221 RVA: 0x0001FA59 File Offset: 0x0001EA59
		// (set) Token: 0x060000DE RID: 222 RVA: 0x0001FA61 File Offset: 0x0001EA61
		public int consumerID
		{
			get
			{
				return this.m_consumerID;
			}
			set
			{
				this.m_consumerID = value;
			}
		}

		// Token: 0x040001F1 RID: 497
		private DataTable dt;

		// Token: 0x040001F2 RID: 498
		private DataTable dtDoorTmpSelected;

		// Token: 0x040001F3 RID: 499
		private DataView dv;

		// Token: 0x040001F4 RID: 500
		private DataView dvDoorTmpSelected;

		// Token: 0x040001F5 RID: 501
		private DataView dvSelected;

		// Token: 0x040001F6 RID: 502
		private DataView dvtmp;

		// Token: 0x040001F7 RID: 503
		private int m_consumerID;

		// Token: 0x040001F8 RID: 504
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x040001F9 RID: 505
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x040001FA RID: 506
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x040001FB RID: 507
		private int[] controlSegIDList = new int[256];

		// Token: 0x040001FC RID: 508
		private string[] controlSegNameList = new string[256];

		// Token: 0x040001FD RID: 509
		private string strZoneFilter = "";
	}
}
