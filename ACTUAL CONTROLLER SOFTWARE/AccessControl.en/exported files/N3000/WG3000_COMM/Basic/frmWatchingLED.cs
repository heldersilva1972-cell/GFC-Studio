using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000057 RID: 87
	public partial class frmWatchingLED : frmN3000
	{
		// Token: 0x06000688 RID: 1672 RVA: 0x000B681B File Offset: 0x000B581B
		public frmWatchingLED()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000B683C File Offset: 0x000B583C
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			bool flag = false;
			for (int i = 0; i < this.dtDealingSelectedGroup.Rows.Count; i++)
			{
				if ((int)this.dtDealingSelectedGroup.Rows[i][1] == -1)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				int num = this.dtDealingSelectedGroup.Rows.Count + 1;
				DataRow dataRow = this.dtDealingSelectedGroup.NewRow();
				dataRow[0] = num;
				dataRow[1] = -1;
				dataRow[2] = "显示当前在场人数";
				dataRow[4] = "显示当前在场人数";
				dataRow[this.dtDealingSelectedGroup.Columns.Count - 1] = 2;
				this.dtDealingSelectedGroup.Rows.Add(dataRow);
				this.dtDealingSelectedGroup.AcceptChanges();
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x000B691C File Offset: 0x000B591C
		private void btnAddTempCard_Click(object sender, EventArgs e)
		{
			bool flag = false;
			for (int i = 0; i < this.dtDealingSelectedGroup.Rows.Count; i++)
			{
				if ((int)this.dtDealingSelectedGroup.Rows[i][1] == -2)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				int num = this.dtDealingSelectedGroup.Rows.Count + 1;
				DataRow dataRow = this.dtDealingSelectedGroup.NewRow();
				dataRow[0] = num;
				dataRow[1] = -2;
				dataRow[2] = "显示临时卡人数";
				dataRow[4] = "显示临时卡人数";
				dataRow[this.dtDealingSelectedGroup.Columns.Count - 1] = 2;
				this.dtDealingSelectedGroup.Rows.Add(dataRow);
				this.dtDealingSelectedGroup.AcceptChanges();
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x000B69FC File Offset: 0x000B59FC
		private void btnGroupDown_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = this.dataGridViewSelected;
				if (dataGridView.RowCount != 0)
				{
					int num;
					if (dataGridView.SelectedRows.Count <= 0)
					{
						if (dataGridView.SelectedCells.Count <= 0)
						{
							return;
						}
						num = dataGridView.SelectedCells[0].RowIndex;
					}
					else
					{
						num = dataGridView.SelectedRows[0].Index;
					}
					if ((int)dataGridView.Rows[num].Cells[0].Value != dataGridView.RowCount)
					{
						DataTable table = ((DataView)dataGridView.DataSource).Table;
						DataRow dataRow = null;
						DataRow dataRow2 = null;
						if (dataGridView.SelectedRows.Count > 0)
						{
							int count = dataGridView.SelectedRows.Count;
							for (int i = 0; i < table.Rows.Count; i++)
							{
								if ((int)dataGridView.Rows[num].Cells[0].Value == (int)table.Rows[i][0])
								{
									dataRow = table.Rows[i];
									break;
								}
							}
							for (int j = 0; j < table.Rows.Count; j++)
							{
								if ((int)dataGridView.Rows[num].Cells[0].Value + 1 == (int)table.Rows[j][0])
								{
									dataRow2 = table.Rows[j];
									break;
								}
							}
							dataRow2[0] = dataRow[0];
							dataRow[0] = (int)dataRow[0] + 1;
							this.dtDealingSelectedGroup.AcceptChanges();
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000B6BF0 File Offset: 0x000B5BF0
		private void btnGroupUp_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = this.dataGridViewSelected;
				if (dataGridView.RowCount != 0)
				{
					int num;
					if (dataGridView.SelectedRows.Count <= 0)
					{
						if (dataGridView.SelectedCells.Count <= 0)
						{
							return;
						}
						num = dataGridView.SelectedCells[0].RowIndex;
					}
					else
					{
						num = dataGridView.SelectedRows[0].Index;
					}
					if ((int)dataGridView.Rows[num].Cells[0].Value != 1)
					{
						DataTable table = ((DataView)dataGridView.DataSource).Table;
						DataRow dataRow = null;
						DataRow dataRow2 = null;
						if (dataGridView.SelectedRows.Count > 0)
						{
							int count = dataGridView.SelectedRows.Count;
							for (int i = 0; i < table.Rows.Count; i++)
							{
								if ((int)dataGridView.Rows[num].Cells[0].Value == (int)table.Rows[i][0])
								{
									dataRow = table.Rows[i];
									break;
								}
							}
							for (int j = 0; j < table.Rows.Count; j++)
							{
								if ((int)dataGridView.Rows[num].Cells[0].Value - 1 == (int)table.Rows[j][0])
								{
									dataRow2 = table.Rows[j];
									break;
								}
							}
							dataRow2[0] = dataRow[0];
							dataRow[0] = (int)dataRow[0] - 1;
							this.dtDealingSelectedGroup.AcceptChanges();
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x000B6DE0 File Offset: 0x000B5DE0
		private void btnReset_Click(object sender, EventArgs e)
		{
			this.getGroupInfo();
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000B6DE8 File Offset: 0x000B5DE8
		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (this.optWEBEnabled.Checked)
			{
				wgAppConfig.UpdateKeyVal("KEY_LED_ENABLE", "1");
				wgAppConfig.UpdateKeyVal("KEY_LED_CONFIG_CONTROLLER_IP", this.textBox1.Text);
				wgAppConfig.wgLog("KEY_LED_ENABLE = 1");
				string text = "";
				string text2 = "";
				string text3 = "";
				string text4 = "";
				string text5 = "";
				string text6 = "";
				for (int i = 0; i < this.dvDealingSelectedGroup.Count; i++)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += ";";
						text2 += ";";
						text3 += ";";
						text4 += ";";
						text5 += ";";
						text6 += ";";
					}
					text += string.Format("{0}", this.dvDealingSelectedGroup[i][0], this.dvDealingSelectedGroup[i][1]);
					text2 += string.Format("{1}", this.dvDealingSelectedGroup[i][0], this.dvDealingSelectedGroup[i][1]);
					text3 += string.Format("{0}", this.dvDealingSelectedGroup[i][this.dtDealingSelectedGroup.Columns.Count - 1]);
					text4 += string.Format("{0}", this.dvDealingSelectedGroup[i][2]);
					text5 += string.Format("{0}", this.dvDealingSelectedGroup[i]["f_GroupIDs"]);
					text6 += string.Format("{0},{1},{2}", this.dvDealingSelectedGroup[i][0], this.dvDealingSelectedGroup[i][1], this.dvDealingSelectedGroup[i][2]);
				}
				wgAppConfig.UpdateKeyVal("KEY_LED_CONFIG", text);
				wgAppConfig.UpdateKeyVal("KEY_LED_CONFIG_GROUPID", text2);
				wgAppConfig.UpdateKeyVal("KEY_LED_CONFIG_GROUPTYPE", text3);
				wgAppConfig.UpdateKeyVal("KEY_LED_CONFIG_GROUPNAME", text4);
				wgAppConfig.UpdateKeyVal("KEY_LED_CONFIG_GROUPIDS", text5);
				wgAppConfig.wgLog("KEY_LED_CONFIG = " + text);
				wgAppConfig.wgLog("KEY_LED_CONFIG_GROUPID = " + text2);
				wgAppConfig.wgLog("KEY_LED_CONFIG_GROUPTYPE = " + text3);
				wgAppConfig.wgLog("KEY_LED_CONFIG_STR = " + text6);
			}
			else
			{
				wgAppConfig.UpdateKeyVal("KEY_LED_ENABLE", "0");
				wgAppConfig.wgLog("KEY_LED_ENABLE = 0");
			}
			frmWatchingLED.getLedConfig();
			base.Close();
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x000B70AC File Offset: 0x000B60AC
		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				DataGridView dataGridView = this.dataGridView1;
				int num;
				if (dataGridView.SelectedRows.Count <= 0)
				{
					if (dataGridView.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dataGridView.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dataGridView.SelectedRows[0].Index;
				}
				DataTable table = ((DataView)dataGridView.DataSource).Table;
				DataRow dataRow = null;
				if (dataGridView.SelectedRows.Count > 0)
				{
					int count = dataGridView.SelectedRows.Count;
					for (int i = 0; i < table.Rows.Count; i++)
					{
						if ((int)dataGridView.Rows[num].Cells[0].Value == (int)table.Rows[i][0])
						{
							dataRow = table.Rows[i];
							break;
						}
					}
					int num2 = this.dtDealingSelectedGroup.Rows.Count + 1;
					DataRow dataRow2 = this.dtDealingSelectedGroup.NewRow();
					for (int j = 0; j < table.Columns.Count; j++)
					{
						dataRow2[j + 1] = dataRow[j];
					}
					dataRow2[0] = num2;
					dataRow2[this.dtDealingSelectedGroup.Columns.Count - 1] = 1;
					this.dtDealingSelectedGroup.Rows.Add(dataRow2);
					this.dtDealingSelectedGroup.AcceptChanges();
					table.Rows.Remove(dataRow);
					table.AcceptChanges();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000B7278 File Offset: 0x000B6278
		private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_LED_RECORD_OUT_MODE", "0");
			wgAppConfig.wgLog("KEY_LED_RECORD_OUT_MODE = 0");
			this.refreshRecordMode();
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x000B729C File Offset: 0x000B629C
		private void dgvMainGroups_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				DataGridView dataGridView = this.dgvMainGroups;
				int num;
				if (dataGridView.SelectedRows.Count <= 0)
				{
					if (dataGridView.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dataGridView.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dataGridView.SelectedRows[0].Index;
				}
				DataTable table = ((DataView)dataGridView.DataSource).Table;
				DataRow dataRow = null;
				if (dataGridView.SelectedRows.Count > 0)
				{
					int count = dataGridView.SelectedRows.Count;
					for (int i = 0; i < table.Rows.Count; i++)
					{
						if ((int)dataGridView.Rows[num].Cells[0].Value == (int)table.Rows[i][0])
						{
							dataRow = table.Rows[i];
							break;
						}
					}
					int num2 = this.dtDealingSelectedGroup.Rows.Count + 1;
					DataRow dataRow2 = this.dtDealingSelectedGroup.NewRow();
					for (int j = 0; j < table.Columns.Count; j++)
					{
						dataRow2[j + 1] = dataRow[j];
					}
					dataRow2[0] = num2;
					dataRow2[this.dtDealingSelectedGroup.Columns.Count - 1] = 0;
					this.dtDealingSelectedGroup.Rows.Add(dataRow2);
					this.dtDealingSelectedGroup.AcceptChanges();
					table.Rows.Remove(dataRow);
					table.AcceptChanges();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000B7468 File Offset: 0x000B6468
		private void displaySet(bool bHide)
		{
			this.label1.Visible = bHide;
			this.label3.Visible = bHide;
			this.textBox1.Visible = bHide;
			this.textBox2.Visible = bHide;
			this.btnAddAllDoors.Visible = bHide;
			this.btnAddTempCard.Visible = bHide;
			this.btnReset.Visible = bHide;
			this.btnGroupUp.Visible = bHide;
			this.btnGroupDown.Visible = bHide;
			this.dataGridView1.Visible = bHide;
			this.dataGridViewSelected.Visible = bHide;
			this.dgvMainGroups.Visible = bHide;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000B7505 File Offset: 0x000B6505
		private void frmWatchingMoreRecords_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x000B7508 File Offset: 0x000B6508
		private void frmWatchingMoreRecords_Load(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_ENABLE")))
				{
					this.bSpecialStyleA = wgAppConfig.GetKeyVal("KEY_LED_ENABLE") == "1";
					this.optWEBEnabled.Checked = true;
				}
				else
				{
					this.optWEBDisable.Checked = true;
					this.displaySet(false);
				}
				this.refreshRecordMode();
				this.getGroupInfo();
				this.loadLedConfig();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x000B7594 File Offset: 0x000B6594
		private void getGroupInfo()
		{
			try
			{
				string text = "";
				string text2 = " SELECT f_GroupID,f_GroupName, 0 as f_Inside, 0 as f_Outside, 0 as f_SwipeOut from t_b_Group";
				if (!string.IsNullOrEmpty(text))
				{
					text2 = text2 + " WHERE " + text;
				}
				text2 += " order by f_GroupName  + '\\' ASC";
				this.dtGroups = new DataTable("GroupsInside");
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text2, oleDbConnection))
						{
							using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
							{
								oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
								oleDbDataAdapter.Fill(this.dtGroups);
							}
						}
						goto IL_0106;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							sqlDataAdapter.Fill(this.dtGroups);
						}
					}
				}
				IL_0106:
				this.dtDealingMainGroup = new DataTable("MainGroup");
				DataColumn dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.Int32");
				dataColumn.ColumnName = "f_GroupID";
				this.dtDealingMainGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "f_GroupName";
				this.dtDealingMainGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.Int32");
				dataColumn.ColumnName = "f_Inside";
				this.dtDealingMainGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "f_GroupNameDisplay";
				this.dtDealingMainGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "f_GroupIDs";
				this.dtDealingMainGroup.Columns.Add(dataColumn);
				this.dtDealingMainGroup.AcceptChanges();
				this.dtDealingSubGroup = new DataTable("SubGroup");
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.Int32");
				dataColumn.ColumnName = "f_GroupID";
				this.dtDealingSubGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "f_GroupName";
				this.dtDealingSubGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.Int32");
				dataColumn.ColumnName = "f_Inside";
				this.dtDealingSubGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "f_GroupNameDisplay";
				this.dtDealingSubGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "f_GroupIDs";
				this.dtDealingSubGroup.Columns.Add(dataColumn);
				this.dtDealingSubGroup.AcceptChanges();
				this.dtDealingSelectedGroup = new DataTable("SelectedGroup");
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.Int32");
				dataColumn.ColumnName = "f_LedMachineID";
				this.dtDealingSelectedGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.Int32");
				dataColumn.ColumnName = "f_GroupID";
				this.dtDealingSelectedGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "f_GroupName";
				this.dtDealingSelectedGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.Int32");
				dataColumn.ColumnName = "f_Inside";
				this.dtDealingSelectedGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "f_GroupNameDisplay";
				this.dtDealingSelectedGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.String");
				dataColumn.ColumnName = "f_GroupIDs";
				this.dtDealingSelectedGroup.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = Type.GetType("System.Int32");
				dataColumn.ColumnName = "f_GroupType";
				this.dtDealingSelectedGroup.Columns.Add(dataColumn);
				this.dtDealingSelectedGroup.AcceptChanges();
				DataView dataView = new DataView(this.dtDealingSubGroup);
				this.dvDealingMainGroup = new DataView(this.dtDealingMainGroup);
				this.dvDealingSubGroup = new DataView(this.dtDealingSubGroup);
				this.dvDealingSelectedGroup = new DataView(this.dtDealingSelectedGroup);
				this.dvDealingSelectedGroup.Sort = "f_LedMachineID ASC";
				this.dgvMainGroups.AutoGenerateColumns = false;
				this.dgvMainGroups.DataSource = this.dvDealingMainGroup;
				this.dgvMainGroups.Columns[0].DataPropertyName = "f_GroupID";
				this.dgvMainGroups.Columns[1].DataPropertyName = "f_GroupNameDisplay";
				this.dgvMainGroups.Columns[2].DataPropertyName = "f_Inside";
				this.dgvMainGroups.Columns[3].DataPropertyName = "f_GroupName";
				this.dgvMainGroups.Columns[4].DataPropertyName = "f_GroupIDs";
				this.dataGridView1.AutoGenerateColumns = false;
				this.dataGridView1.DataSource = this.dvDealingSubGroup;
				this.dataGridView1.Columns[0].DataPropertyName = "f_GroupID";
				this.dataGridView1.Columns[1].DataPropertyName = "f_GroupNameDisplay";
				this.dataGridView1.Columns[2].DataPropertyName = "f_Inside";
				this.dataGridView1.Columns[3].DataPropertyName = "f_GroupName";
				this.dataGridView1.Columns[4].DataPropertyName = "f_GroupIDs";
				this.dataGridViewSelected.AutoGenerateColumns = false;
				this.dataGridViewSelected.DataSource = this.dvDealingSelectedGroup;
				this.dataGridViewSelected.Columns[0].DataPropertyName = "f_LedMachineID";
				this.dataGridViewSelected.Columns[1].DataPropertyName = "f_GroupID";
				this.dataGridViewSelected.Columns[2].DataPropertyName = "f_GroupNameDisplay";
				this.dataGridViewSelected.Columns[3].DataPropertyName = "f_Inside";
				this.dataGridViewSelected.Columns[4].DataPropertyName = "f_GroupName";
				this.dataGridViewSelected.Columns[5].DataPropertyName = "f_GroupIDs";
				this.dataGridViewSelected.Columns[6].DataPropertyName = "f_GroupType";
				this.dtDealing = this.dtGroups;
				this.dvDealingMainGroup = new DataView(this.dtDealingMainGroup);
				this.dvDealingSubGroup = new DataView(this.dtDealingSubGroup);
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < this.dtDealing.Rows.Count; i++)
				{
					if ((int)this.dtDealing.Rows[i]["f_GroupID"] > 0 && ((string)this.dtDealing.Rows[i]["f_GroupName"]).IndexOf(CommonStr.strDepartmentTempCard) < 0)
					{
						string text3 = (string)this.dtDealing.Rows[i]["f_GroupName"];
						int num3 = text3.Length - text3.Replace("\\", "").Length;
						if (num2 < num3)
						{
							num2 = num3;
						}
					}
				}
				if (num2 > 0)
				{
					num = num2 - 1;
				}
				for (int j = 0; j < this.dtDealing.Rows.Count; j++)
				{
					if ((int)this.dtDealing.Rows[j]["f_GroupID"] > 0 && ((string)this.dtDealing.Rows[j]["f_GroupName"]).IndexOf(CommonStr.strDepartmentTempCard) < 0)
					{
						string text4 = "-2";
						string text3 = (string)this.dtDealing.Rows[j]["f_GroupName"];
						int num4 = text3.Length - text3.Replace("\\", "").Length;
						if (num4 == num)
						{
							DataRow dataRow = this.dtDealingMainGroup.NewRow();
							dataRow["f_GroupID"] = this.dtDealing.Rows[j]["f_GroupID"];
							dataRow["f_GroupName"] = text3;
							dataRow["f_GroupNameDisplay"] = text3;
							if (num > 0)
							{
								dataRow["f_GroupNameDisplay"] = text3.Substring(text3.LastIndexOf("\\") + 1);
							}
							dataRow["f_GroupIDs"] = dataRow["f_GroupID"];
							this.dtDealingMainGroup.Rows.Add(dataRow);
							this.dtDealingMainGroup.AcceptChanges();
						}
						else if (num4 == num + 1)
						{
							DataRow dataRow2 = this.dtDealingMainGroup.Rows[this.dtDealingMainGroup.Rows.Count - 1];
							dataRow2["f_GroupIDs"] = string.Format("{0},{1}", (string)dataRow2["f_GroupIDs"], ((int)this.dtDealing.Rows[j]["f_GroupID"]).ToString());
							this.dtDealingMainGroup.AcceptChanges();
							string text5 = text3.Substring(text3.LastIndexOf("\\") + 1);
							dataView.RowFilter = "f_GroupName = " + wgTools.PrepareStr(text5);
							if (dataView.Count == 0)
							{
								dataRow2 = this.dtDealingSubGroup.NewRow();
								dataRow2["f_GroupID"] = this.dtDealing.Rows[j]["f_GroupID"];
								dataRow2["f_GroupName"] = text5;
								dataRow2["f_GroupNameDisplay"] = text5;
								dataRow2["f_GroupIDs"] = text4;
								this.dtDealingSubGroup.Rows.Add(dataRow2);
								this.dtDealingSubGroup.AcceptChanges();
							}
							if (dataView.Count > 0)
							{
								dataView[0]["f_GroupIDs"] = string.Format("{0},{1}", dataView[0]["f_GroupIDs"], ((int)this.dtDealing.Rows[j]["f_GroupID"]).ToString());
								this.dtDealingSubGroup.AcceptChanges();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000B81F4 File Offset: 0x000B71F4
		public static void getLedConfig()
		{
			try
			{
				frmWatchingLED.bSendTempCard = false;
				frmWatchingLED.bSendTotalUsers = false;
				frmWatchingLED.arrGroupNames4Send = new ArrayList();
				frmWatchingLED.arrLoc4Send = new ArrayList();
				frmWatchingLED.arrGroupIDs4Send = new ArrayList();
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_CONFIG_CONTROLLER_IP")))
				{
					frmWatchingLED.ledControllerIP = wgAppConfig.GetKeyVal("KEY_LED_CONFIG_CONTROLLER_IP");
				}
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_ENABLE")))
				{
					frmWatchingLED.bLedActive = wgAppConfig.GetKeyVal("KEY_LED_ENABLE") == "1";
				}
				else
				{
					frmWatchingLED.bLedActive = false;
				}
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_RECORD_OUT_MODE")))
				{
					int.TryParse(wgAppConfig.GetKeyVal("KEY_LED_RECORD_OUT_MODE"), out frmWatchingLED.recordOutMode);
				}
				frmWatchingLED.ledPageSwipeMax = int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_LED_PAGE_SWIPE")));
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_CONFIG")))
				{
					string[] array = wgAppConfig.GetKeyVal("KEY_LED_CONFIG").Split(new char[] { ';' });
					string[] array2 = wgAppConfig.GetKeyVal("KEY_LED_CONFIG_GROUPID").Split(new char[] { ';' });
					string[] array3 = wgAppConfig.GetKeyVal("KEY_LED_CONFIG_GROUPTYPE").Split(new char[] { ';' });
					string[] array4 = wgAppConfig.GetKeyVal("KEY_LED_CONFIG_GROUPNAME").Split(new char[] { ';' });
					string[] array5 = wgAppConfig.GetKeyVal("KEY_LED_CONFIG_GROUPIDS").Split(new char[] { ';' });
					if (array.Length == array2.Length)
					{
						for (int i = 0; i < array.Length; i++)
						{
							if (int.Parse(array2[i]) == -2)
							{
								frmWatchingLED.bSendTempCard = true;
								frmWatchingLED.arrGroupNames4Send.Add("显示临时卡人数");
								frmWatchingLED.arrGroupIDs4Send.Add("");
								frmWatchingLED.arrLoc4Send.Add(int.Parse(array[i]));
							}
							else if (int.Parse(array2[i]) == -1)
							{
								frmWatchingLED.bSendTotalUsers = true;
								frmWatchingLED.arrGroupNames4Send.Add("显示当前在场人数");
								frmWatchingLED.arrGroupIDs4Send.Add("");
								frmWatchingLED.arrLoc4Send.Add(int.Parse(array[i]));
							}
							else if (int.Parse(array3[i]) == 0)
							{
								frmWatchingLED.arrGroupNames4Send.Add(array4[i]);
								frmWatchingLED.arrLoc4Send.Add(int.Parse(array[i]));
								frmWatchingLED.arrGroupIDs4Send.Add(array5[i]);
							}
							else if (int.Parse(array3[i]) == 1)
							{
								frmWatchingLED.arrGroupNames4Send.Add(array4[i]);
								frmWatchingLED.arrLoc4Send.Add(int.Parse(array[i]));
								frmWatchingLED.arrGroupIDs4Send.Add(array5[i]);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000B84F8 File Offset: 0x000B74F8
		private void ledSwitchCycleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (sender == this.ledSwitchCycleToolStripMenuItem1)
			{
				num = 1;
			}
			if (sender == this.ledSwitchCycleToolStripMenuItem2)
			{
				num = 2;
			}
			if (sender == this.ledSwitchCycleToolStripMenuItem3)
			{
				num = 3;
			}
			if (sender == this.ledSwitchCycleToolStripMenuItem4)
			{
				num = 4;
			}
			if (sender == this.ledSwitchCycleToolStripMenuItem5)
			{
				num = 5;
			}
			wgAppConfig.UpdateKeyVal("KEY_LED_SWITCH_CYCLE", num.ToString());
			wgAppConfig.wgLog("KEY_LED_SWITCH_CYCLE =" + num.ToString());
			this.refreshRecordMode();
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000B856C File Offset: 0x000B756C
		private void loadLedConfig()
		{
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_CONFIG_CONTROLLER_IP")))
				{
					this.textBox1.Text = wgAppConfig.GetKeyVal("KEY_LED_CONFIG_CONTROLLER_IP");
				}
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_CONFIG")))
				{
					string[] array = wgAppConfig.GetKeyVal("KEY_LED_CONFIG").Split(new char[] { ';' });
					string[] array2 = wgAppConfig.GetKeyVal("KEY_LED_CONFIG_GROUPID").Split(new char[] { ';' });
					string[] array3 = wgAppConfig.GetKeyVal("KEY_LED_CONFIG_GROUPTYPE").Split(new char[] { ';' });
					string[] array4 = wgAppConfig.GetKeyVal("KEY_LED_CONFIG_GROUPNAME").Split(new char[] { ';' });
					string[] array5 = wgAppConfig.GetKeyVal("KEY_LED_CONFIG_GROUPIDS").Split(new char[] { ';' });
					this.dtDealingSelectedGroup.Clear();
					if (array.Length == array2.Length)
					{
						for (int i = 0; i < array.Length; i++)
						{
							int count = this.dtDealingSelectedGroup.Rows.Count;
							DataRow dataRow = this.dtDealingSelectedGroup.NewRow();
							dataRow[0] = int.Parse(array[i]);
							dataRow[1] = int.Parse(array2[i]);
							dataRow[2] = array4[i];
							dataRow["f_GroupIDs"] = array5[i];
							dataRow[this.dtDealingSelectedGroup.Columns.Count - 1] = int.Parse(array3[i]);
							if ((int)dataRow[1] == -2)
							{
								dataRow[2] = "显示临时卡人数";
								dataRow[4] = "显示临时卡人数";
								dataRow[this.dtDealingSelectedGroup.Columns.Count - 1] = 2;
							}
							else if ((int)dataRow[1] == -1)
							{
								dataRow[2] = "显示当前在场人数";
								dataRow[4] = "显示当前在场人数";
								dataRow[this.dtDealingSelectedGroup.Columns.Count - 1] = 2;
							}
							else if (int.Parse(array3[i]) == 0)
							{
								for (int j = 0; j < this.dtDealingMainGroup.Rows.Count; j++)
								{
									if (((string)this.dtDealingMainGroup.Rows[j][1]).Equals((string)dataRow[2]))
									{
										DataRow dataRow2 = this.dtDealingMainGroup.Rows[j];
										for (int k = 1; k < this.dtDealingMainGroup.Columns.Count; k++)
										{
											dataRow[k + 1] = dataRow2[k];
										}
										this.dtDealingMainGroup.Rows.Remove(dataRow2);
										this.dtDealingMainGroup.AcceptChanges();
										break;
									}
								}
							}
							else if (int.Parse(array3[i]) == 1)
							{
								for (int l = 0; l < this.dtDealingSubGroup.Rows.Count; l++)
								{
									if (((string)this.dtDealingSubGroup.Rows[l][1]).Equals((string)dataRow[2]))
									{
										DataRow dataRow2 = this.dtDealingSubGroup.Rows[l];
										for (int m = 1; m < this.dtDealingSubGroup.Columns.Count; m++)
										{
											dataRow[m + 1] = dataRow2[m];
										}
										this.dtDealingSubGroup.Rows.Remove(dataRow2);
										this.dtDealingSubGroup.AcceptChanges();
										break;
									}
								}
							}
							this.dtDealingSelectedGroup.Rows.Add(dataRow);
							this.dtDealingSelectedGroup.AcceptChanges();
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000B8998 File Offset: 0x000B7998
		private void optWEBDisable_CheckedChanged(object sender, EventArgs e)
		{
			this.displaySet(false);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000B89A1 File Offset: 0x000B79A1
		private void optWEBEnabled_CheckedChanged(object sender, EventArgs e)
		{
			this.displaySet(true);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x000B89AA File Offset: 0x000B79AA
		public void ReallyCloseForm()
		{
			base.Close();
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000B89B4 File Offset: 0x000B79B4
		private void refreshRecordMode()
		{
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_RECORD_OUT_MODE")))
			{
				this.userGongzhongTimeToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_LED_RECORD_OUT_MODE") == "1";
				this.defaultToolStripMenuItem.Checked = false;
			}
			else
			{
				this.defaultToolStripMenuItem.Checked = true;
				this.userGongzhongTimeToolStripMenuItem.Checked = false;
			}
			this.deactiveDefaultToolStripMenuItem.Checked = true;
			this.ledSwitchCycleToolStripMenuItem1.Checked = false;
			this.ledSwitchCycleToolStripMenuItem2.Checked = false;
			this.ledSwitchCycleToolStripMenuItem3.Checked = false;
			this.ledSwitchCycleToolStripMenuItem4.Checked = false;
			this.ledSwitchCycleToolStripMenuItem5.Checked = false;
			frmWatchingLED.ledSwitchCyle = 0;
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_SWITCH_CYCLE")))
			{
				int num = 0;
				int.TryParse(wgAppConfig.GetKeyVal("KEY_LED_SWITCH_CYCLE"), out num);
				if (num > 0 && num <= 5)
				{
					frmWatchingLED.ledSwitchCyle = num;
					this.deactiveDefaultToolStripMenuItem.Checked = false;
					this.ledSwitchCycleToolStripMenuItem1.Checked = num == 1;
					this.ledSwitchCycleToolStripMenuItem2.Checked = num == 2;
					this.ledSwitchCycleToolStripMenuItem3.Checked = num == 3;
					this.ledSwitchCycleToolStripMenuItem4.Checked = num == 4;
					this.ledSwitchCycleToolStripMenuItem5.Checked = num == 5;
				}
			}
			this.toolStripMenuItemPageSwipe0.Checked = true;
			this.toolStripMenuItemPageSwipe1.Checked = false;
			this.toolStripMenuItemPageSwipe2.Checked = false;
			this.toolStripMenuItemPageSwipe3.Checked = false;
			this.toolStripMenuItemPageSwipe4.Checked = false;
			this.toolStripMenuItemPageSwipe5.Checked = false;
			frmWatchingLED.ledPageSwipeMax = 0;
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_PAGE_SWIPE")))
			{
				int num2 = 0;
				int.TryParse(wgAppConfig.GetKeyVal("KEY_LED_PAGE_SWIPE"), out num2);
				if (num2 > 0 && num2 <= 5)
				{
					frmWatchingLED.ledPageSwipeMax = num2;
					this.toolStripMenuItemPageSwipe0.Checked = false;
					this.toolStripMenuItemPageSwipe1.Checked = num2 == 1;
					this.toolStripMenuItemPageSwipe2.Checked = num2 == 2;
					this.toolStripMenuItemPageSwipe3.Checked = num2 == 3;
					this.toolStripMenuItemPageSwipe4.Checked = num2 == 4;
					this.toolStripMenuItemPageSwipe5.Checked = num2 == 5;
				}
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x000B8BC7 File Offset: 0x000B7BC7
		private void restoreDefaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("WatchingMoreRecords_Display", "");
			base.Close();
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000B8BE0 File Offset: 0x000B7BE0
		public static void sendLEDNumber(DataTable dtGroupsShare)
		{
			try
			{
				if (frmWatchingLED.bLedActive)
				{
					string text = frmWatchingLED.ledControllerIP;
					DataTable dataTable;
					lock (dtGroupsShare)
					{
						dataTable = dtGroupsShare.Copy();
					}
					DataView dataView = new DataView(dataTable);
					int num = 0;
					if (frmWatchingLED.bSendTotalUsers)
					{
						dataView.RowFilter = "f_GroupID = -1";
						if (dataView.Count > 0)
						{
							num = (int)dataView[0]["f_Inside"];
						}
					}
					int num2 = 0;
					if (frmWatchingLED.bSendTempCard)
					{
						dataView.RowFilter = string.Format("f_GroupName like '%{0}%'", CommonStr.strDepartmentTempCard);
						if (dataView.Count > 0)
						{
							num2 = (int)dataView[0]["f_Inside"];
						}
					}
					string text2 = "!0/";
					for (int i = 0; i < frmWatchingLED.arrLoc4Send.Count; i++)
					{
						if (frmWatchingLED.arrGroupNames4Send[i].Equals("显示临时卡人数"))
						{
							text2 = text2 + num2.ToString() + "/";
						}
						else if (frmWatchingLED.arrGroupNames4Send[i].Equals("显示当前在场人数"))
						{
							text2 = text2 + num.ToString() + "/";
						}
						else
						{
							dataView.RowFilter = string.Format("f_GroupID IN ({0})", frmWatchingLED.arrGroupIDs4Send[i]);
							int num3 = 0;
							for (int j = 0; j < dataView.Count; j++)
							{
								num3 += (int)dataView[j]["f_Inside"];
							}
							text2 = text2 + num3.ToString() + "/";
						}
					}
					text2 += "0#";
					string text3 = text2;
					if (!string.IsNullOrEmpty(text3))
					{
						using (UdpClient udpClient = new UdpClient())
						{
							byte[] bytes = Encoding.ASCII.GetBytes(text2);
							byte[] array = new byte[bytes.Length + 2];
							array[0] = 85;
							for (int k = 1; k < array.Length - 1; k++)
							{
								array[k] = bytes[k - 1];
							}
							array[array.Length - 1] = 170;
							string[] array2 = text.Replace("，", ",").Split(new char[] { ',' });
							for (int l = 0; l < array2.Length; l++)
							{
								try
								{
									IPAddress ipaddress;
									if (IPAddress.TryParse(array2[l], out ipaddress))
									{
										udpClient.Send(array, array.Length, new IPEndPoint(IPAddress.Parse(array2[l]), 6666));
									}
								}
								catch (Exception ex)
								{
									wgAppConfig.wgLogWithoutDB(ex.ToString());
								}
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x000B8EF4 File Offset: 0x000B7EF4
		public static void sendLEDSwipeInfo(string Info)
		{
			if (frmWatchingLED.bLedActive && !string.IsNullOrEmpty(Info))
			{
				if (frmWatchingLED.ledSwitchCyle > 0)
				{
					frmWatchingLED.datetimeRefresh = DateTime.Now.AddSeconds((double)frmWatchingLED.ledSwitchCyle);
				}
				string text = frmWatchingLED.ledControllerIP;
				using (UdpClient udpClient = new UdpClient())
				{
					string[] array = Info.Split(new char[] { '/' });
					byte[] array2 = new byte[1024];
					int num = 0;
					array2[0] = 85;
					array2[1] = 37;
					num += 2;
					for (int i = 0; i < array.Length; i++)
					{
						string text2 = array[i];
						string text3 = BitConverter.ToString(Encoding.GetEncoding("gb2312").GetBytes(text2)).Replace("-", "").ToLower();
						byte[] bytes = Encoding.ASCII.GetBytes(text3);
						for (int j = 0; j < bytes.Length; j++)
						{
							array2[num + j] = bytes[j];
						}
						if (i < array.Length - 1)
						{
							array2[num + bytes.Length] = 47;
							num = num + bytes.Length + 1;
						}
						else
						{
							num += bytes.Length;
						}
					}
					num += 2;
					array2[num - 2] = 35;
					array2[num - 1] = 170;
					string[] array3 = text.Replace("，", ",").Split(new char[] { ',' });
					for (int k = 0; k < array3.Length; k++)
					{
						try
						{
							IPAddress ipaddress;
							if (IPAddress.TryParse(array3[k], out ipaddress))
							{
								udpClient.Send(array2, num, new IPEndPoint(IPAddress.Parse(array3[k]), 6666));
							}
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLogWithoutDB(ex.ToString());
						}
					}
					Thread.Sleep(5);
				}
			}
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x000B90F0 File Offset: 0x000B80F0
		public static void sendPersonsIndoor(DataTable dtShare)
		{
			try
			{
				if (frmWatchingLED.bLedActive && frmWatchingLED.ledSwitchCyle != 0)
				{
					if (frmWatchingLED.ledSwitchCyle == -1)
					{
						frmWatchingLED.ledSwitchCyle = 0;
						if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_LED_SWITCH_CYCLE")))
						{
							int num = 0;
							int.TryParse(wgAppConfig.GetKeyVal("KEY_LED_SWITCH_CYCLE"), out num);
							if (num > 0 && num < 5)
							{
								frmWatchingLED.ledSwitchCyle = num;
							}
						}
						if (frmWatchingLED.ledSwitchCyle == 0)
						{
							return;
						}
					}
					if (DateTime.Compare(frmWatchingLED.datetimeRefresh, DateTime.Now) < 0)
					{
						frmWatchingLED.datetimeRefresh = DateTime.Now.AddSeconds((double)frmWatchingLED.ledSwitchCyle);
						string text = frmWatchingLED.ledControllerIP;
						DataView dataView = new DataView(dtShare);
						dataView.RowFilter = "f_bHave =2";
						string text2 = " ";
						int num2 = frmWatchingLED.ledPageSwipeMax;
						if (frmWatchingLED.lastSendIndex >= dataView.Count)
						{
							frmWatchingLED.lastSendIndex = 0;
						}
						bool flag = false;
						for (int i = 0; i < num2; i++)
						{
							text2 += "/";
							if (flag)
							{
								text2 += " / / ";
							}
							else if (frmWatchingLED.lastSendIndex >= dataView.Count)
							{
								frmWatchingLED.lastSendIndex = 0;
								flag = true;
								text2 += " / / ";
							}
							else
							{
								if (wgTools.SetObjToStr(dataView[frmWatchingLED.lastSendIndex]["f_GroupName"]).LastIndexOf("\\") > 0)
								{
									object obj = text2;
									text2 = string.Concat(new object[]
									{
										obj,
										dataView[frmWatchingLED.lastSendIndex]["f_ConsumerName"],
										"/",
										((string)dataView[frmWatchingLED.lastSendIndex]["f_GroupName"]).Substring(((string)dataView[frmWatchingLED.lastSendIndex]["f_GroupName"]).LastIndexOf("\\") + 1),
										"/",
										((string)dataView[frmWatchingLED.lastSendIndex]["f_ReadDate"]).Substring(((string)dataView[frmWatchingLED.lastSendIndex]["f_ReadDate"]).IndexOf(" ") + 1, 8)
									});
								}
								else
								{
									object obj2 = text2;
									text2 = string.Concat(new object[]
									{
										obj2,
										dataView[frmWatchingLED.lastSendIndex]["f_ConsumerName"],
										"/",
										wgTools.SetObjToStr(dataView[frmWatchingLED.lastSendIndex]["f_GroupName"]),
										"/",
										((string)dataView[frmWatchingLED.lastSendIndex]["f_ReadDate"]).Substring(((string)dataView[frmWatchingLED.lastSendIndex]["f_ReadDate"]).IndexOf(" ") + 1, 8)
									});
								}
								frmWatchingLED.lastSendIndex++;
							}
						}
						frmWatchingLED.sendLEDSwipeInfo(text2);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x000B9418 File Offset: 0x000B8418
		private void toolStripMenuItemPageSwipe0_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (sender == this.toolStripMenuItemPageSwipe1)
			{
				num = 1;
			}
			if (sender == this.toolStripMenuItemPageSwipe2)
			{
				num = 2;
			}
			if (sender == this.toolStripMenuItemPageSwipe3)
			{
				num = 3;
			}
			if (sender == this.toolStripMenuItemPageSwipe4)
			{
				num = 4;
			}
			if (sender == this.toolStripMenuItemPageSwipe5)
			{
				num = 5;
			}
			wgAppConfig.UpdateKeyVal("KEY_LED_PAGE_SWIPE", num.ToString());
			wgAppConfig.wgLog("KEY_LED_PAGE_SWIPE =" + num.ToString());
			this.refreshRecordMode();
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x000B948B File Offset: 0x000B848B
		private void userGongzhongTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_LED_RECORD_OUT_MODE", "1");
			wgAppConfig.wgLog("KEY_LED_RECORD_OUT_MODE = 1");
			this.refreshRecordMode();
		}

		// Token: 0x04000BFF RID: 3071
		private bool bSpecialStyleA;

		// Token: 0x04000C00 RID: 3072
		private DataTable dtDealing;

		// Token: 0x04000C01 RID: 3073
		private DataTable dtDealingMainGroup;

		// Token: 0x04000C02 RID: 3074
		private DataTable dtDealingSelectedGroup;

		// Token: 0x04000C03 RID: 3075
		private DataTable dtDealingSubGroup;

		// Token: 0x04000C04 RID: 3076
		private DataTable dtGroups;

		// Token: 0x04000C05 RID: 3077
		private DataView dvDealingMainGroup;

		// Token: 0x04000C06 RID: 3078
		private DataView dvDealingSelectedGroup;

		// Token: 0x04000C07 RID: 3079
		private DataView dvDealingSubGroup;

		// Token: 0x04000C08 RID: 3080
		private static ArrayList arrGroupIDs4Send = new ArrayList();

		// Token: 0x04000C09 RID: 3081
		private static ArrayList arrGroupNames4Send = new ArrayList();

		// Token: 0x04000C0A RID: 3082
		private static ArrayList arrLoc4Send = new ArrayList();

		// Token: 0x04000C0B RID: 3083
		public bool bDisplayCapturedPhoto;

		// Token: 0x04000C0C RID: 3084
		private static bool bLedActive = false;

		// Token: 0x04000C0D RID: 3085
		private static bool bSendTempCard = false;

		// Token: 0x04000C0E RID: 3086
		private static bool bSendTotalUsers = false;

		// Token: 0x04000C0F RID: 3087
		private static DateTime datetimeRefresh = DateTime.Now;

		// Token: 0x04000C10 RID: 3088
		public frmConsole frmCall;

		// Token: 0x04000C11 RID: 3089
		public int groupMax = 3;

		// Token: 0x04000C12 RID: 3090
		public float InfoFontSize = 15f;

		// Token: 0x04000C13 RID: 3091
		private static int lastSendIndex = 0;

		// Token: 0x04000C14 RID: 3092
		private static string ledControllerIP = "192.168.0.99";

		// Token: 0x04000C15 RID: 3093
		public static int ledPageSwipeMax = 0;

		// Token: 0x04000C16 RID: 3094
		private static int ledSwitchCyle = -1;

		// Token: 0x04000C17 RID: 3095
		public static int recordOutMode = 0;

		// Token: 0x04000C18 RID: 3096
		public DataTable tbRunInfoLog;
	}
}
