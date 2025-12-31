using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Elevator
{
	// Token: 0x0200024C RID: 588
	public partial class dfrmUserFloor : frmN3000
	{
		// Token: 0x06001285 RID: 4741 RVA: 0x0016557C File Offset: 0x0016457C
		public dfrmUserFloor()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvFloors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00165618 File Offset: 0x00164618
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			DataTable table = ((DataView)this.dgvFloors.DataSource).Table;
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
				using (DataView dataView = new DataView((this.dgvFloors.DataSource as DataView).Table))
				{
					dataView.RowFilter = string.Format("  {0} ", this.strZoneFilter);
					for (int j = 0; j < dataView.Count; j++)
					{
						dataView[j]["f_Selected"] = 1;
					}
				}
			}
			this.updateCount();
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00165730 File Offset: 0x00164730
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			this.selectObject(this.dgvFloors);
			this.updateCount();
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00165744 File Offset: 0x00164744
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			DataTable table = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				table.Rows[i]["f_Selected"] = 0;
			}
			this.updateCount();
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0016579F File Offset: 0x0016479F
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.updateCount();
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x001657B2 File Offset: 0x001647B2
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

		// Token: 0x0600128B RID: 4747 RVA: 0x001657D4 File Offset: 0x001647D4
		private void btnOK_Click(object sender, EventArgs e)
		{
			if ((sender != this.btnAdd || this.dgvSelectedDoors.Rows.Count != 0) && (sender != this.btnUpdate || XMessageBox.Show(this, CommonStr.strUpdateFloorPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK))
			{
				if (wgAppConfig.IsAccessDB)
				{
					this.btnOK_Click_Acc(sender, e);
					return;
				}
				this.bEdit = true;
				Cursor.Current = Cursors.WaitCursor;
				this.cn = new SqlConnection(wgAppConfig.dbConString);
				this.dtDoorTmpSelected = ((DataView)this.dgvSelectedDoors.DataSource).Table.Copy();
				this.dvDoorTmpSelected = new DataView(this.dtDoorTmpSelected);
				this.dvSelectedControllerID = new DataView(this.dtDoorTmpSelected);
				ArrayList arrayList = new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				this.dvDoorTmpSelected.RowFilter = "f_Selected > 0";
				this.dvSelectedControllerID.RowFilter = "f_Selected = 2";
				foreach (object obj in this.dvDoorTmpSelected)
				{
					DataRowView dataRowView = (DataRowView)obj;
					this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", dataRowView["f_ControllerID"].ToString());
					if (this.dvSelectedControllerID.Count == wgMjController.GetControllerType(int.Parse(dataRowView["f_ControllerSN"].ToString())))
					{
						if (arrayList.IndexOf(int.Parse(dataRowView["f_ControllerID"].ToString())) < 0)
						{
							arrayList.Add(int.Parse(dataRowView["f_ControllerID"].ToString()));
							arrayList2.Add(int.Parse(dataRowView["f_ControllerSN"].ToString()));
						}
					}
					else
					{
						dataRowView["f_Selected"] = 2;
					}
				}
				this.dvDoorTmpSelected.RowFilter = "f_Selected = 2";
				this.cn.Open();
				this.cmd = new SqlCommand("", this.cn);
				this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
				string text = "DELETE FROM  [t_b_UserFloor]    ";
				if (string.IsNullOrEmpty(this.strSqlSelected))
				{
					object obj2 = text;
					text = string.Concat(new object[] { obj2, "WHERE [f_ConsumerID] = (", this.consumerID, " ) " });
				}
				else
				{
					text += string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected);
				}
				if (this.arrRecIDOfUserFloor.Count > 0 && this.arrZoneID.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < this.arrZoneID.Count; i++)
					{
						if (stringBuilder.Length == 0)
						{
							stringBuilder.Append(this.arrZoneID[i].ToString());
						}
						else
						{
							stringBuilder.Append("," + this.arrZoneID[i].ToString());
						}
					}
					if (stringBuilder.Length > 0)
					{
						text = text + " AND f_FloorID IN  (SELECT f_floorID FROM t_b_Floor, t_b_Controller WHERE t_b_Floor.f_ControllerID = t_b_Controller.f_ControllerID AND t_b_Controller.f_ZoneID IN ( " + stringBuilder.ToString() + "))";
					}
				}
				int j;
				if (sender == this.btnAdd)
				{
					j = 0;
					string text2 = "";
					while (j < this.dgvSelectedDoors.Rows.Count)
					{
						text2 = text2 + this.dgvSelectedDoors.Rows[j].Cells[0].Value.ToString() + ",";
						j++;
					}
					text = text + " AND f_FloorID IN (" + text2 + "0)";
				}
				this.cmd.CommandText = text;
				wgTools.WriteLine(text);
				this.cmd.ExecuteNonQuery();
				wgTools.WriteLine("DELETE FROM  [t_b_UserFloor] End");
				j = 0;
				if (!string.IsNullOrEmpty(this.strSqlSelected))
				{
					while (j < this.dgvSelectedDoors.Rows.Count)
					{
						text = "INSERT INTO [t_b_UserFloor] (f_ConsumerID, f_floorID , f_ControlSegID, f_MoreFloorNum)";
						text = string.Concat(new string[]
						{
							text,
							" SELECT  f_ConsumerID,  ",
							this.dgvSelectedDoors.Rows[j].Cells[0].Value.ToString(),
							" AS f_floorID,",
							this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex].ToString(),
							" AS f_ControlSegID,",
							this.dgvSelectedDoors.Rows.Count.ToString(),
							" AS f_MoreFloorNum  from t_b_consumer ",
							string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected)
						});
						this.cmd.CommandText = text;
						this.cmd.ExecuteNonQuery();
						j++;
					}
				}
				else
				{
					while (j < this.dgvSelectedDoors.Rows.Count)
					{
						text = "INSERT INTO [t_b_UserFloor] (f_ConsumerID, f_floorID , f_ControlSegID, f_MoreFloorNum)";
						text = string.Concat(new string[]
						{
							text,
							" VALUES ( ",
							this.consumerID.ToString(),
							",",
							this.dgvSelectedDoors.Rows[j].Cells[0].Value.ToString(),
							",",
							this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex].ToString(),
							",",
							this.dgvSelectedDoors.Rows.Count.ToString(),
							")"
						});
						this.cmd.CommandText = text;
						this.cmd.ExecuteNonQuery();
						j++;
					}
				}
				wgTools.WriteLine("INSERT INTO [t_b_UserFloor] End");
				if (string.IsNullOrEmpty(this.strSqlSelected))
				{
					text = "update [t_b_UserFloor] set f_MoreFloorNum = " + (this.lastRecordTotalCnt + this.dgvSelectedDoors.RowCount - this.lastRecordCurrentCnt).ToString() + " where  f_ConsumerID =  " + this.consumerID.ToString();
					this.cmd.CommandText = text;
					this.cmd.ExecuteNonQuery();
				}
				string text3 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
				for (int k = 0; k < this.dgvSelectedDoors.Rows.Count; k++)
				{
					text = string.Format(text3, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, (int)((DataView)this.dgvSelectedDoors.DataSource)[k]["f_ControllerID"]);
					this.cmd.CommandText = text;
					this.cmd.ExecuteNonQuery();
				}
				this.cn.Close();
				Cursor.Current = Cursors.Default;
				this.logOperate(sender);
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00165F10 File Offset: 0x00164F10
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			OleDbConnection oleDbConnection = null;
			this.bEdit = true;
			Cursor.Current = Cursors.WaitCursor;
			oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			this.dtDoorTmpSelected = ((DataView)this.dgvSelectedDoors.DataSource).Table.Copy();
			this.dvDoorTmpSelected = new DataView(this.dtDoorTmpSelected);
			this.dvSelectedControllerID = new DataView(this.dtDoorTmpSelected);
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			this.dvDoorTmpSelected.RowFilter = "f_Selected > 0";
			this.dvSelectedControllerID.RowFilter = "f_Selected = 2";
			foreach (object obj in this.dvDoorTmpSelected)
			{
				DataRowView dataRowView = (DataRowView)obj;
				this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", dataRowView["f_ControllerID"].ToString());
				if (this.dvSelectedControllerID.Count == wgMjController.GetControllerType(int.Parse(dataRowView["f_ControllerSN"].ToString())))
				{
					if (arrayList.IndexOf(int.Parse(dataRowView["f_ControllerID"].ToString())) < 0)
					{
						arrayList.Add(int.Parse(dataRowView["f_ControllerID"].ToString()));
						arrayList2.Add(int.Parse(dataRowView["f_ControllerSN"].ToString()));
					}
				}
				else
				{
					dataRowView["f_Selected"] = 2;
				}
			}
			this.dvDoorTmpSelected.RowFilter = "f_Selected = 2";
			oleDbConnection.Open();
			OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection);
			oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
			string text = "DELETE FROM  [t_b_UserFloor]    ";
			if (string.IsNullOrEmpty(this.strSqlSelected))
			{
				object obj2 = text;
				text = string.Concat(new object[] { obj2, "WHERE [f_ConsumerID] = (", this.consumerID, " ) " });
			}
			else
			{
				text += string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected);
			}
			if (this.arrRecIDOfUserFloor.Count > 0 && this.arrZoneID.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.arrZoneID.Count; i++)
				{
					if (stringBuilder.Length == 0)
					{
						stringBuilder.Append(this.arrZoneID[i].ToString());
					}
					else
					{
						stringBuilder.Append("," + this.arrZoneID[i].ToString());
					}
				}
				if (stringBuilder.Length > 0)
				{
					text = text + " AND f_FloorID IN  (SELECT f_floorID FROM t_b_Floor, t_b_Controller WHERE t_b_Floor.f_ControllerID = t_b_Controller.f_ControllerID AND t_b_Controller.f_ZoneID IN ( " + stringBuilder.ToString() + "))";
				}
			}
			int j;
			if (sender == this.btnAdd)
			{
				j = 0;
				string text2 = "";
				while (j < this.dgvSelectedDoors.Rows.Count)
				{
					text2 = text2 + this.dgvSelectedDoors.Rows[j].Cells[0].Value.ToString() + ",";
					j++;
				}
				text = text + " AND f_FloorID IN (" + text2 + "0)";
			}
			oleDbCommand.CommandText = text;
			wgTools.WriteLine(text);
			oleDbCommand.ExecuteNonQuery();
			wgTools.WriteLine("DELETE FROM  [t_b_UserFloor] End");
			j = 0;
			if (!string.IsNullOrEmpty(this.strSqlSelected))
			{
				while (j < this.dgvSelectedDoors.Rows.Count)
				{
					text = "INSERT INTO [t_b_UserFloor] (f_ConsumerID, f_floorID , f_ControlSegID, f_MoreFloorNum)";
					text = string.Concat(new string[]
					{
						text,
						" SELECT  f_ConsumerID,  ",
						this.dgvSelectedDoors.Rows[j].Cells[0].Value.ToString(),
						" AS f_floorID,",
						this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex].ToString(),
						" AS f_ControlSegID,",
						this.dgvSelectedDoors.Rows.Count.ToString(),
						" AS f_MoreFloorNum  from t_b_consumer ",
						string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected)
					});
					oleDbCommand.CommandText = text;
					oleDbCommand.ExecuteNonQuery();
					j++;
				}
			}
			else
			{
				while (j < this.dgvSelectedDoors.Rows.Count)
				{
					text = "INSERT INTO [t_b_UserFloor] (f_ConsumerID, f_floorID , f_ControlSegID, f_MoreFloorNum)";
					text = string.Concat(new string[]
					{
						text,
						" VALUES ( ",
						this.consumerID.ToString(),
						",",
						this.dgvSelectedDoors.Rows[j].Cells[0].Value.ToString(),
						",",
						this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex].ToString(),
						",",
						this.dgvSelectedDoors.Rows.Count.ToString(),
						")"
					});
					oleDbCommand.CommandText = text;
					oleDbCommand.ExecuteNonQuery();
					j++;
				}
			}
			wgTools.WriteLine("INSERT INTO [t_b_UserFloor] End");
			if (string.IsNullOrEmpty(this.strSqlSelected))
			{
				text = "update [t_b_UserFloor] set f_MoreFloorNum = " + (this.lastRecordTotalCnt + this.dgvSelectedDoors.RowCount - this.lastRecordCurrentCnt).ToString() + " where  f_ConsumerID =  " + this.consumerID.ToString();
				oleDbCommand.CommandText = text;
				oleDbCommand.ExecuteNonQuery();
			}
			string text3 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
			for (int k = 0; k < this.dgvSelectedDoors.Rows.Count; k++)
			{
				text = string.Format(text3, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, (int)((DataView)this.dgvSelectedDoors.DataSource)[k]["f_ControllerID"]);
				oleDbCommand.CommandText = text;
				oleDbCommand.ExecuteNonQuery();
			}
			oleDbConnection.Close();
			Cursor.Current = Cursors.Default;
			this.logOperate(sender);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x001665C0 File Offset: 0x001655C0
		private void cbof_ControlSegID_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x001665C4 File Offset: 0x001655C4
		private void cbof_Zone_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_ZoneID, this.cbof_ZoneID.Text);
			if (this.dgvFloors.DataSource != null)
			{
				DataView dataView = (DataView)this.dgvFloors.DataSource;
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

		// Token: 0x0600128F RID: 4751 RVA: 0x00166794 File Offset: 0x00165794
		private void cbof_ZoneID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ZoneID);
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x001667A4 File Offset: 0x001657A4
		private void dfrmPrivilegeSingle_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			if (!string.IsNullOrEmpty(this.strSqlSelected))
			{
				this.btnOK.Visible = false;
				this.btnAdd.Visible = true;
				this.btnUpdate.Visible = true;
			}
			try
			{
				this.label1.Visible = wgAppConfig.getParamValBoolByNO(121);
				this.cbof_ControlSegID.Visible = wgAppConfig.getParamValBoolByNO(121);
				this.loadControlSegData();
				this.loadZoneInfo();
				this.loadDoorData();
				this.loadPrivilegeData();
				this.lastRecordTotalCnt = this.dgvSelectedDoors.RowCount;
				if (this.dgvFloors.DataSource != null)
				{
					DataView dataView = (DataView)this.dgvFloors.DataSource;
					if ((int)this.arrZoneID[0] != 0)
					{
						for (int i = 0; i < this.arrZoneID.Count; i++)
						{
							dataView.RowFilter = " f_ZoneID =" + this.arrZoneID[i];
							this.strZoneFilter = " f_ZoneID =" + this.arrZoneID[i];
							int num = (int)this.arrZoneID[i];
							int num2 = (int)this.arrZoneNO[i];
							int num3 = (int)this.arrZoneNO[i];
							dataView.RowFilter = string.Format(" f_ZoneID ={0:d} ", num);
							this.strZoneFilter = string.Format(" f_ZoneID ={0:d} ", num);
							if (dataView.Count > 0)
							{
								for (int j = 0; j < dataView.Count; j++)
								{
									this.arrRecIDOfUserFloor.Add((int)dataView[j]["f_floorID"]);
								}
							}
						}
						DataTable table = dataView.Table;
						for (int k = table.Rows.Count - 1; k >= 0; k--)
						{
							DataRow dataRow = table.Rows[k];
							if (this.arrRecIDOfUserFloor.IndexOf((int)dataRow["f_floorID"]) < 0)
							{
								table.Rows.Remove(dataRow);
							}
						}
						table.AcceptChanges();
					}
				}
				this.cbof_Zone_SelectedIndexChanged(null, null);
				this.updateCount();
				this.lastRecordCurrentCnt = this.dgvSelectedDoors.RowCount;
				bool flag = false;
				string text = "mnuElevator";
				if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
				{
					this.btnAddAllDoors.Visible = false;
					this.btnAddOneDoor.Visible = false;
					this.btnDelAllDoors.Visible = false;
					this.btnDelOneDoor.Visible = false;
					this.btnOK.Visible = false;
					this.btnAdd.Visible = false;
					this.btnUpdate.Visible = false;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00166A98 File Offset: 0x00165A98
		private void dfrmUserFloor_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00166AB0 File Offset: 0x00165AB0
		private void dfrmUserFloor_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.dfrmFind1 == null)
					{
						this.dfrmFind1 = new dfrmFind();
					}
					this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00166B20 File Offset: 0x00165B20
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00166B2D File Offset: 0x00165B2D
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00166B3A File Offset: 0x00165B3A
		private void label1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00166B3C File Offset: 0x00165B3C
		private void loadControlSegData()
		{
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
			this.controlSegNameList[0] = CommonStr.strFreeTime;
			this.controlSegIDList[0] = 1;
			int num = 1;
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = " SELECT ";
				text += " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,   IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID   FROM [t_b_ControlSeg]  WHERE  [t_b_ControlSeg].[f_ControlSegID]>1  ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							this.cbof_ControlSegID.Items.Add(oleDbDataReader["f_ControlSegID"]);
							this.controlSegNameList[num] = (string)oleDbDataReader["f_ControlSegID"];
							this.controlSegIDList[num] = (int)oleDbDataReader["f_ControlSegIDBak"];
							num++;
						}
						oleDbDataReader.Close();
					}
					goto IL_01B4;
				}
			}
			text = " SELECT ";
			text += " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,    CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),       ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50),      ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']')     END AS f_ControlSegID    FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
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
			IL_01B4:
			if (this.cbof_ControlSegID.Items.Count > 0)
			{
				this.cbof_ControlSegID.SelectedIndex = 0;
			}
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00166D50 File Offset: 0x00165D50
		private void loadDoorData()
		{
			string text = " SELECT a.f_floorID,  c.f_DoorName + '.' + a.f_floorName as f_floorFullName , 0 as f_Selected, b.f_ZoneID, 0 as f_TimeProfile, b.f_ControllerID, b.f_ControllerSN, a.f_floorName,c.f_DoorName ";
			text += " FROM t_b_floor a, t_b_Controller b,t_b_Door c WHERE c.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID and a.f_DoorID = c.f_DoorID  ORDER BY  (  c.f_DoorName + '.' + a.f_floorName ) ";
			this.dt = new DataTable();
			this.dv = new DataView(this.dt);
			this.dvSelected = new DataView(this.dt);
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
					goto IL_00E8;
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
			try
			{
				IL_00E8:
				this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.dv.RowFilter = "f_Selected = 0";
			this.dvSelected.RowFilter = "f_Selected > 0";
			this.dgvFloors.AutoGenerateColumns = false;
			this.dgvFloors.DataSource = this.dv;
			this.dgvSelectedDoors.AutoGenerateColumns = false;
			this.dgvSelectedDoors.DataSource = this.dvSelected;
			for (int i = 0; i < this.dgvFloors.Columns.Count; i++)
			{
				this.dgvFloors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
				this.dgvSelectedDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
			}
			if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(144)) & 255) == 2)
			{
				this.dv.Sort = "f_ZoneID ASC, f_floorName ASC, f_DoorName ASC";
				this.dvSelected.Sort = "f_ZoneID ASC, f_floorName ASC, f_DoorName ASC";
			}
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00166FF0 File Offset: 0x00165FF0
		private void loadPrivilegeData()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadPrivilegeData Start");
			string text = " SELECT f_RecID, f_FloorId, f_ControlSegID ";
			text = text + " FROM t_b_UserFloor  WHERE f_ConsumerID=  " + this.m_consumerID.ToString();
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
					goto IL_00FC;
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
			IL_00FC:
			wgTools.WriteLine("da.Fill End");
			this.dv = new DataView(this.tbPrivilege);
			this.oldTbPrivilege = this.tbPrivilege;
			int num = 1;
			if (this.dv.Count > 0)
			{
				DataTable table = ((DataView)this.dgvFloors.DataSource).Table;
				for (int i = 0; i < this.dv.Count; i++)
				{
					for (int j = 0; j < table.Rows.Count; j++)
					{
						if ((int)this.dv[i]["f_floorID"] == (int)table.Rows[j]["f_floorID"])
						{
							table.Rows[j]["f_Selected"] = 1;
							num = int.Parse(this.dv[i]["f_ControlSegID"].ToString());
							break;
						}
					}
				}
			}
			for (int k = 0; k < this.controlSegIDList.Length; k++)
			{
				if (this.controlSegIDList[k] == num)
				{
					this.cbof_ControlSegID.SelectedIndex = k;
				}
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00167294 File Offset: 0x00166294
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

		// Token: 0x0600129A RID: 4762 RVA: 0x00167380 File Offset: 0x00166380
		private void logOperate(object sender)
		{
			string text = this.Text;
			if (!string.IsNullOrEmpty(this.strSelectedUsers))
			{
				text = text + ":" + this.strSelectedUsers;
			}
			string text2 = "";
			for (int i = 0; i <= Math.Min(10, this.dgvSelectedDoors.RowCount) - 1; i++)
			{
				text2 = text2 + ((DataView)this.dgvSelectedDoors.DataSource)[i]["f_floorFullName"] + ",";
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

		// Token: 0x0600129B RID: 4763 RVA: 0x001674F8 File Offset: 0x001664F8
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
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0016767C File Offset: 0x0016667C
		private void updateCount()
		{
			this.lblOptional.Text = this.dgvFloors.RowCount.ToString();
			this.lblSeleted.Text = this.dgvSelectedDoors.RowCount.ToString();
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x001676C5 File Offset: 0x001666C5
		// (set) Token: 0x0600129E RID: 4766 RVA: 0x001676CD File Offset: 0x001666CD
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

		// Token: 0x040021EA RID: 8682
		private bool bEdit;

		// Token: 0x040021EB RID: 8683
		private SqlCommand cmd;

		// Token: 0x040021EC RID: 8684
		private SqlConnection cn;

		// Token: 0x040021ED RID: 8685
		private DataTable dt;

		// Token: 0x040021EE RID: 8686
		private DataTable dtDoorTmpSelected;

		// Token: 0x040021EF RID: 8687
		private DataView dv;

		// Token: 0x040021F0 RID: 8688
		private DataView dvDoorTmpSelected;

		// Token: 0x040021F1 RID: 8689
		private DataView dvSelected;

		// Token: 0x040021F2 RID: 8690
		private DataView dvSelectedControllerID;

		// Token: 0x040021F3 RID: 8691
		private int lastRecordCurrentCnt;

		// Token: 0x040021F4 RID: 8692
		private int lastRecordTotalCnt;

		// Token: 0x040021F5 RID: 8693
		private int m_consumerID;

		// Token: 0x040021F6 RID: 8694
		private DataTable oldTbPrivilege;

		// Token: 0x040021F7 RID: 8695
		private DataTable tbPrivilege;

		// Token: 0x040021F8 RID: 8696
		private ArrayList arrRecIDOfUserFloor = new ArrayList();

		// Token: 0x040021F9 RID: 8697
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x040021FA RID: 8698
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x040021FB RID: 8699
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x040021FC RID: 8700
		private int[] controlSegIDList = new int[256];

		// Token: 0x040021FD RID: 8701
		private string[] controlSegNameList = new string[256];

		// Token: 0x040021FE RID: 8702
		public string strSelectedUsers = "";

		// Token: 0x040021FF RID: 8703
		public string strSqlSelected = "";

		// Token: 0x04002200 RID: 8704
		private string strZoneFilter = "";
	}
}
