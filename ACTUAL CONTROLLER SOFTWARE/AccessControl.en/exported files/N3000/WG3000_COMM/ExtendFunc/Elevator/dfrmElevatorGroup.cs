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

namespace WG3000_COMM.ExtendFunc.Elevator
{
	// Token: 0x02000248 RID: 584
	public partial class dfrmElevatorGroup : frmN3000
	{
		// Token: 0x06001246 RID: 4678 RVA: 0x0015FA04 File Offset: 0x0015EA04
		public dfrmElevatorGroup()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0015FA70 File Offset: 0x0015EA70
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
			if (this.cbof_ZoneID.SelectedIndex <= 0 && this.cbof_ZoneID.Text == CommonStr.strAllZones)
			{
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if ((int)table.Rows[i]["f_Selected"] != 1)
					{
						table.Rows[i]["f_Selected"] = this.nudGroupToAdd.Value;
					}
				}
			}
			else
			{
				using (DataView dataView = new DataView((this.dgvDoors.DataSource as DataView).Table))
				{
					dataView.RowFilter = string.Format("  {0} ", this.strZoneFilter);
					for (int j = 0; j < dataView.Count; j++)
					{
						dataView[j]["f_Selected"] = this.nudGroupToAdd.Value;
					}
				}
			}
			this.updateCount();
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0015FB9C File Offset: 0x0015EB9C
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			this.selectObject(this.dgvDoors);
			this.updateCount();
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0015FBB0 File Offset: 0x0015EBB0
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			DataTable table = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				table.Rows[i]["f_Selected"] = 0;
			}
			this.updateCount();
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0015FC0B File Offset: 0x0015EC0B
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.updateCount();
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0015FC1E File Offset: 0x0015EC1E
		private void btnExit_Click(object sender, EventArgs e)
		{
			if (this.bEdit)
			{
				base.DialogResult = DialogResult.OK;
			}
			else
			{
				base.DialogResult = DialogResult.Cancel;
			}
			base.Close();
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0015FC40 File Offset: 0x0015EC40
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOK_Click_Acc(sender, e);
				return;
			}
			this.bEdit = true;
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("btnDelete_Click Start");
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			this.cn.Open();
			this.cmd = new SqlCommand("", this.cn);
			this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
			string text = "DELETE FROM  [t_b_ElevatorGroup]    ";
			this.cmd.CommandText = text;
			wgTools.WriteLine(text);
			this.cmd.ExecuteNonQuery();
			wgTools.WriteLine("DELETE FROM  [t_b_ElevatorGroup] End");
			for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
			{
				text = "INSERT INTO [t_b_ElevatorGroup] ([f_DoorID], [f_ControllerID], [f_ElevatorGroupNO])";
				text = string.Concat(new string[]
				{
					text,
					" VALUES(  ",
					this.dgvSelectedDoors.Rows[i].Cells[0].Value.ToString(),
					" , ",
					this.dgvSelectedDoors.Rows[i].Cells[4].Value.ToString(),
					" , ",
					this.dgvSelectedDoors.Rows[i].Cells[1].Value.ToString(),
					" ) "
				});
				this.cmd.CommandText = text;
				this.cmd.ExecuteNonQuery();
			}
			wgTools.WriteLine("INSERT INTO [t_b_ElevatorGroup] End");
			string text2 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
			for (int j = 0; j < this.dgvSelectedDoors.Rows.Count; j++)
			{
				text = string.Format(text2, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, (int)((DataView)this.dgvSelectedDoors.DataSource)[j]["f_ControllerID"]);
				this.cmd.CommandText = text;
				this.cmd.ExecuteNonQuery();
			}
			this.cn.Close();
			Cursor.Current = Cursors.Default;
			this.logOperate(this.btnOK);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0015FEA8 File Offset: 0x0015EEA8
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			this.bEdit = true;
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("btnDelete_Click Start");
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			oleDbConnection.Open();
			OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection);
			oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
			string text = "DELETE FROM  [t_b_ElevatorGroup]    ";
			oleDbCommand.CommandText = text;
			wgTools.WriteLine(text);
			oleDbCommand.ExecuteNonQuery();
			wgTools.WriteLine("DELETE FROM  [t_b_ElevatorGroup] End");
			for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
			{
				text = "INSERT INTO [t_b_ElevatorGroup] ([f_DoorID], [f_ControllerID], [f_ElevatorGroupNO])";
				text = string.Concat(new string[]
				{
					text,
					" VALUES(  ",
					this.dgvSelectedDoors.Rows[i].Cells[0].Value.ToString(),
					" , ",
					this.dgvSelectedDoors.Rows[i].Cells[4].Value.ToString(),
					" , ",
					this.dgvSelectedDoors.Rows[i].Cells[1].Value.ToString(),
					" ) "
				});
				oleDbCommand.CommandText = text;
				oleDbCommand.ExecuteNonQuery();
			}
			wgTools.WriteLine("INSERT INTO [t_b_ElevatorGroup] End");
			string text2 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
			for (int j = 0; j < this.dgvSelectedDoors.Rows.Count; j++)
			{
				text = string.Format(text2, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, (int)((DataView)this.dgvSelectedDoors.DataSource)[j]["f_ControllerID"]);
				oleDbCommand.CommandText = text;
				oleDbCommand.ExecuteNonQuery();
			}
			oleDbConnection.Close();
			Cursor.Current = Cursors.Default;
			this.logOperate(this.btnOK);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x001600D0 File Offset: 0x0015F0D0
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

		// Token: 0x0600124F RID: 4687 RVA: 0x001602A0 File Offset: 0x0015F2A0
		private void cbof_ZoneID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ZoneID);
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x001602B0 File Offset: 0x0015F2B0
		private void dfrmPrivilegeSingle_Load(object sender, EventArgs e)
		{
			try
			{
				this.loadZoneInfo();
				this.loadDoorData();
				this.loadElevatorGroupData();
				this.updateCount();
				bool flag = false;
				string text = "mnuElevator";
				if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
				{
					this.btnAddAllDoors.Visible = false;
					this.btnAddOneDoor.Visible = false;
					this.btnDelAllDoors.Visible = false;
					this.btnDelOneDoor.Visible = false;
					this.btnOK.Visible = false;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00160350 File Offset: 0x0015F350
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0016035D File Offset: 0x0015F35D
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0016036C File Offset: 0x0015F36C
		private void loadDoorData()
		{
			string text = " SELECT a.f_DoorID,  0 as f_Selected, a.f_DoorName , b.f_ZoneID, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  AND b.f_ControllerSN >= 170000000 AND b.f_ControllerSN <= 179999999  ORDER BY a.f_DoorName ";
			this.dt = new DataTable();
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
					goto IL_00C6;
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
			IL_00C6:
			this.dv = new DataView(this.dt);
			this.dv.Sort = "f_Selected, f_DoorName";
			this.dvSelected = new DataView(this.dt);
			this.dvSelected.Sort = "f_Selected, f_DoorName";
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

		// Token: 0x06001254 RID: 4692 RVA: 0x001605EC File Offset: 0x0015F5EC
		private void loadElevatorGroupData()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadPrivilegeData Start");
			string text = " SELECT [f_DoorID], [f_ControllerID], [f_ElevatorGroupNO] ";
			text += " FROM t_b_ElevatorGroup  ";
			this.tbPrivilege = new DataTable();
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							oleDbDataAdapter.Fill(this.tbPrivilege);
						}
					}
					goto IL_00F1;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						sqlDataAdapter.Fill(this.tbPrivilege);
					}
				}
			}
			IL_00F1:
			wgTools.WriteLine("da.Fill End");
			this.dv = new DataView(this.tbPrivilege);
			this.oldTbPrivilege = this.tbPrivilege;
			if (this.dv.Count > 0)
			{
				DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
				for (int i = 0; i < this.dv.Count; i++)
				{
					for (int j = 0; j < table.Rows.Count; j++)
					{
						if ((int)this.dv[i]["f_DoorID"] == (int)table.Rows[j]["f_DoorID"])
						{
							table.Rows[j]["f_Selected"] = this.dv[i]["f_ElevatorGroupNO"];
							break;
						}
					}
				}
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0016083C File Offset: 0x0015F83C
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

		// Token: 0x06001256 RID: 4694 RVA: 0x00160928 File Offset: 0x0015F928
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

		// Token: 0x06001257 RID: 4695 RVA: 0x00160A80 File Offset: 0x0015FA80
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
							dataRow["f_Selected"] = this.nudGroupToAdd.Value;
						}
					}
				}
				else
				{
					int num3 = (int)dgv.Rows[num].Cells[0].Value;
					DataRow dataRow = table.Rows.Find(num3);
					if (dataRow != null)
					{
						dataRow["f_Selected"] = this.nudGroupToAdd.Value;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00160C18 File Offset: 0x0015FC18
		private void updateCount()
		{
			this.lblOptional.Text = this.dgvDoors.RowCount.ToString();
			this.lblSeleted.Text = this.dgvSelectedDoors.RowCount.ToString();
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06001259 RID: 4697 RVA: 0x00160C61 File Offset: 0x0015FC61
		// (set) Token: 0x0600125A RID: 4698 RVA: 0x00160C69 File Offset: 0x0015FC69
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

		// Token: 0x04002172 RID: 8562
		private bool bEdit;

		// Token: 0x04002173 RID: 8563
		private SqlCommand cmd;

		// Token: 0x04002174 RID: 8564
		private SqlConnection cn;

		// Token: 0x04002175 RID: 8565
		private DataTable dt;

		// Token: 0x04002176 RID: 8566
		private DataView dv;

		// Token: 0x04002177 RID: 8567
		private DataView dvSelected;

		// Token: 0x04002178 RID: 8568
		private int m_consumerID;

		// Token: 0x04002179 RID: 8569
		private DataTable oldTbPrivilege;

		// Token: 0x0400217A RID: 8570
		private DataTable tbPrivilege;

		// Token: 0x0400217B RID: 8571
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x0400217C RID: 8572
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x0400217D RID: 8573
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x0400217E RID: 8574
		private int[] controlSegIDList = new int[256];

		// Token: 0x0400217F RID: 8575
		private string strZoneFilter = "";
	}
}
