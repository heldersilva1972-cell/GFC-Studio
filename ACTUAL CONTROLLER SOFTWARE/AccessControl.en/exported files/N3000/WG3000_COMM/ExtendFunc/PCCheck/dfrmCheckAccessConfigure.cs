using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.PCCheck
{
	// Token: 0x02000314 RID: 788
	public partial class dfrmCheckAccessConfigure : frmN3000
	{
		// Token: 0x0600181F RID: 6175 RVA: 0x001F58F4 File Offset: 0x001F48F4
		public dfrmCheckAccessConfigure()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvGroups);
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x001F5994 File Offset: 0x001F4994
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvDoors.DataSource).Table;
			if (this.cbof_ZoneID.SelectedIndex <= 0 && this.cbof_ZoneID.Text == CommonStr.strAllZones)
			{
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					this.dt.Rows[i]["f_Selected"] = 1;
				}
				return;
			}
			if (this.cbof_ZoneID.SelectedIndex >= 0)
			{
				this.dvtmp = new DataView((this.dgvDoors.DataSource as DataView).Table);
				this.dvtmp.RowFilter = string.Format("  {0} ", this.strZoneFilter);
				for (int j = 0; j < this.dvtmp.Count; j++)
				{
					this.dvtmp[j]["f_Selected"] = 1;
				}
			}
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x001F5A99 File Offset: 0x001F4A99
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvDoors);
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x001F5AA6 File Offset: 0x001F4AA6
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x001F5AB0 File Offset: 0x001F4AB0
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 0;
			}
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x001F5B14 File Offset: 0x001F4B14
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x001F5B24 File Offset: 0x001F4B24
		private void btnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridView dataGridView = this.dgvGroups;
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
				int num2 = (int)dataGridView.Rows[num].Cells[0].Value;
				DataRow dataRow = table.Rows.Find(num2);
				if (dataRow != null)
				{
					using (dfrmCheckAccessSetup dfrmCheckAccessSetup = new dfrmCheckAccessSetup())
					{
						dfrmCheckAccessSetup.groupname = dataRow["f_GroupName"].ToString();
						dfrmCheckAccessSetup.soundfilename = dataRow["f_SoundFileName"].ToString();
						dfrmCheckAccessSetup.active = (int)dataRow["f_CheckAccessActive"];
						dfrmCheckAccessSetup.morecards = (int)dataRow["f_MoreCards"];
						if (dfrmCheckAccessSetup.ShowDialog(this) == DialogResult.OK)
						{
							dataRow["f_SoundFileName"] = dfrmCheckAccessSetup.soundfilename;
							dataRow["f_CheckAccessActive"] = dfrmCheckAccessSetup.active;
							dataRow["f_MoreCards"] = dfrmCheckAccessSetup.morecards;
							this.dsCheckAccess.Tables["groups"].AcceptChanges();
							this.Refresh();
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x001F5CFC File Offset: 0x001F4CFC
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				string text = "";
				if (this.dvSelected.Count > 0)
				{
					for (int i = 0; i < this.dvSelected.Count; i++)
					{
						if (i != 0)
						{
							text += ",";
						}
						text += string.Format("{0:d}", this.dvSelected[i]["f_DoorID"]);
					}
				}
				string text2 = "DELETE from t_b_group4PCCheckAccess";
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text2, oleDbConnection))
						{
							oleDbConnection.Open();
							oleDbCommand.ExecuteNonQuery();
							for (int j = 0; j <= this.dvCheckAccess.Count - 1; j++)
							{
								text2 = "INSERT INTO t_b_group4PCCheckAccess (f_GroupID,f_GroupType,f_CheckAccessActive,f_MoreCards,f_SoundFileName)";
								text2 = text2 + " VALUES( " + this.dvCheckAccess[j]["f_GroupID"].ToString();
								text2 = text2 + " ," + 0;
								text2 = text2 + " ," + this.dvCheckAccess[j]["f_CheckAccessActive"].ToString();
								text2 = text2 + " ," + this.dvCheckAccess[j]["f_MoreCards"].ToString();
								text2 = text2 + " ," + wgTools.PrepareStrNUnicode(this.dvCheckAccess[j]["f_SoundFileName"].ToString());
								text2 += ")";
								oleDbCommand.CommandText = text2;
								oleDbCommand.ExecuteNonQuery();
							}
							if (!string.IsNullOrEmpty(this.cbof_GroupID.Text))
							{
								text2 = "INSERT INTO t_b_group4PCCheckAccess (f_GroupID,f_GroupType,f_CheckAccessActive,f_MoreCards,f_SoundFileName)";
								text2 = text2 + " VALUES( " + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
								text2 = text2 + " ," + 1;
								text2 = text2 + " ," + this.MoreCardsInsidetoolStripComboBox2.Text;
								if (this.toolStripComboBox1.SelectedIndex == 0)
								{
									text2 = text2 + " ," + 1;
								}
								else
								{
									text2 = text2 + " ," + 0;
								}
								text2 = text2 + " ," + wgTools.PrepareStrNUnicode(text);
								text2 += ")";
								oleDbCommand.CommandText = text2;
								oleDbCommand.ExecuteNonQuery();
							}
						}
						goto IL_049B;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.ExecuteNonQuery();
						for (int k = 0; k <= this.dvCheckAccess.Count - 1; k++)
						{
							text2 = "INSERT INTO t_b_group4PCCheckAccess (f_GroupID,f_GroupType,f_CheckAccessActive,f_MoreCards,f_SoundFileName)";
							text2 = string.Concat(new object[]
							{
								text2,
								" VALUES( ",
								this.dvCheckAccess[k]["f_GroupID"].ToString(),
								" ,",
								0,
								" ,",
								this.dvCheckAccess[k]["f_CheckAccessActive"].ToString(),
								" ,",
								this.dvCheckAccess[k]["f_MoreCards"].ToString(),
								" ,",
								wgTools.PrepareStrNUnicode(this.dvCheckAccess[k]["f_SoundFileName"].ToString()),
								")"
							});
							sqlCommand.CommandText = text2;
							sqlCommand.ExecuteNonQuery();
						}
						if (!string.IsNullOrEmpty(this.cbof_GroupID.Text))
						{
							text2 = "INSERT INTO t_b_group4PCCheckAccess (f_GroupID,f_GroupType,f_CheckAccessActive,f_MoreCards,f_SoundFileName)";
							text2 = string.Concat(new object[]
							{
								text2,
								" VALUES( ",
								this.arrGroupID[this.cbof_GroupID.SelectedIndex],
								" ,",
								1,
								" ,",
								0
							});
							if (this.toolStripComboBox1.SelectedIndex == 0)
							{
								text2 = text2 + " ," + 1;
							}
							else
							{
								text2 = text2 + " ," + 0;
							}
							text2 = text2 + " ," + wgTools.PrepareStrNUnicode(text) + ")";
							sqlCommand.CommandText = text2;
							sqlCommand.ExecuteNonQuery();
						}
					}
				}
				IL_049B:
				wgAppConfig.setSystemParamValue(172, "Video_OnlyDisplaySingle", this.toolStripComboBox2.SelectedIndex.ToString(), "Camera View-2014-12-10 09:28:59");
				int num = 0;
				int.TryParse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(172)), out num);
				if (num == 1)
				{
					wgAppConfig.setSystemParamValue(157, "Video_OnlyCapturePhoto", "0", "Camera View-20131019");
				}
				wgAppConfig.setSystemParamValue(158, "Video_AutoAdjustTimeOfVideo", (num != 1) ? "1" : "0", "Camera View-20131019");
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x001F62D8 File Offset: 0x001F52D8
		private void btnOption_Click(object sender, EventArgs e)
		{
			base.Size = new Size(base.Width, Math.Max(604, this.groupBox2.Location.Y + this.groupBox2.Height + 60));
			this.btnOption.Enabled = false;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x001F632E File Offset: 0x001F532E
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x001F633C File Offset: 0x001F533C
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
			try
			{
				if (this.dvCheckAccess != null)
				{
					if (string.IsNullOrEmpty(this.cbof_GroupID.Text))
					{
						this.dvCheckAccess.RowFilter = "";
					}
					else
					{
						this.dvCheckAccess.RowFilter = string.Format("f_GroupName <> '{0}'", this.cbof_GroupID.Text);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x001F63D8 File Offset: 0x001F53D8
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
					return;
				}
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
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x001F659E File Offset: 0x001F559E
		private void dfrmCheckAccessConfigure_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x001F65B4 File Offset: 0x001F55B4
		private void dfrmCheckAccessConfigure_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x0600182D RID: 6189 RVA: 0x001F6624 File Offset: 0x001F5624
		private void dfrmCheckAccessSetup_Load(object sender, EventArgs e)
		{
			this.toolStripComboBox1.SelectedIndex = 0;
			this.toolStripComboBox2.SelectedIndex = 0;
			int num = 0;
			int.TryParse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(172)), out num);
			if (num == 1)
			{
				this.toolStripComboBox2.SelectedIndex = 1;
			}
			this.bFindActive = false;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.GroupName.HeaderText = wgAppConfig.ReplaceFloorRoom(this.GroupName.HeaderText);
			this.label1.Text = wgAppConfig.ReplaceFloorRoom(this.label1.Text);
			try
			{
				new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
				for (int i = 0; i < this.arrGroupID.Count; i++)
				{
					if (i == 0 && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
					{
						this.cbof_GroupID.Items.Add("");
					}
					else
					{
						this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
					}
				}
				if (this.cbof_GroupID.Items.Count > 0)
				{
					this.cbof_GroupID.SelectedIndex = 0;
				}
				string text = " SELECT a.f_GroupID,a.f_GroupName,b.f_CheckAccessActive,b.f_MoreCards, b.f_SoundFileName,b.f_GroupType from t_b_Group a LEFT JOIN t_b_group4PCCheckAccess b ON a.f_GroupID = b.f_GroupID order by f_GroupName  + '\\' ASC";
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
							{
								oleDbDataAdapter.Fill(this.dsCheckAccess, "groups");
							}
						}
						goto IL_020B;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlDataAdapter.Fill(this.dsCheckAccess, "groups");
						}
					}
				}
				IL_020B:
				this.dvCheckAccess = new DataView(this.dsCheckAccess.Tables["groups"]);
				for (int j = 0; j <= this.dvCheckAccess.Count - 1; j++)
				{
					if (string.IsNullOrEmpty(this.dvCheckAccess[j]["f_GroupType"].ToString()))
					{
						this.dvCheckAccess[j]["f_GroupType"] = 0;
						this.dvCheckAccess[j]["f_CheckAccessActive"] = 0;
						this.dvCheckAccess[j]["f_MoreCards"] = 1;
						this.dvCheckAccess[j]["f_SoundFileName"] = "";
					}
				}
				this.dvCheckAccess.RowFilter = "f_GroupType = 1";
				if (this.dvCheckAccess.Count > 0 && this.cbof_GroupID.Items.IndexOf(this.dvCheckAccess[0]["f_GroupName"].ToString()) > 0)
				{
					if (wgTools.SetObjToStr(this.dvCheckAccess[0]["f_MoreCards"]) == "1")
					{
						this.toolStripComboBox1.SelectedIndex = 0;
					}
					else
					{
						this.toolStripComboBox1.SelectedIndex = 1;
					}
					if (wgTools.SetObjToStr(this.dvCheckAccess[0]["f_CheckAccessActive"]) == "0")
					{
						this.MoreCardsInsidetoolStripComboBox2.SelectedIndex = 0;
					}
					else if ((int)this.dvCheckAccess[0]["f_CheckAccessActive"] <= this.MoreCardsInsidetoolStripComboBox2.Items.Count)
					{
						this.MoreCardsInsidetoolStripComboBox2.SelectedIndex = (int)this.dvCheckAccess[0]["f_CheckAccessActive"] - 1;
					}
					this.strSelectedDoors = wgTools.SetObjToStr(this.dvCheckAccess[0]["f_SoundFileName"]);
					this.dvCheckAccess[0]["f_SoundFileName"] = "";
					this.cbof_GroupID.Text = this.dvCheckAccess[0]["f_GroupName"].ToString();
					if (!string.IsNullOrEmpty(this.strSelectedDoors))
					{
						this.btnOption.Enabled = false;
						base.Size = new Size(base.Width, Math.Max(604, this.groupBox2.Location.Y + this.groupBox2.Height + 60));
					}
				}
				try
				{
					this.dsCheckAccess.Tables["groups"].PrimaryKey = new DataColumn[] { this.dsCheckAccess.Tables["groups"].Columns[0] };
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				this.cbof_GroupID_SelectedIndexChanged(null, null);
				this.dgvGroups.AutoGenerateColumns = false;
				this.dgvGroups.DataSource = this.dvCheckAccess;
				int num2 = 0;
				while (num2 < this.dvCheckAccess.Table.Columns.Count && num2 < this.dgvGroups.ColumnCount)
				{
					this.dgvGroups.Columns[num2].DataPropertyName = this.dvCheckAccess.Table.Columns[num2].ColumnName;
					num2++;
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
			this.dgvGroups.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.loadZoneInfo();
			this.loadDoorData();
			this.loadPrivilegeData();
			this.dgvDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.toolStripComboBox1.Visible = wgAppConfig.IsAllowAccess("MoreCardAllGroups");
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x001F6D0C File Offset: 0x001F5D0C
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x001F6D19 File Offset: 0x001F5D19
		private void dgvGroups_DoubleClick(object sender, EventArgs e)
		{
			this.btnEdit.PerformClick();
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x001F6D26 File Offset: 0x001F5D26
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x001F6D34 File Offset: 0x001F5D34
		private void displayConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				string systemParamNotes = wgAppConfig.getSystemParamNotes(202);
				new DataTable("DataRow");
				if (!string.IsNullOrEmpty(systemParamNotes))
				{
					DataView dataView = new DataView(DatatableToJson.JsonToDataTable(systemParamNotes));
					string text = "";
					dataView.Sort = "f_MoreCards";
					for (int i = 0; i < dataView.Count; i++)
					{
						text += string.Format("{0}: {1}\r\n", dataView[i][2], dataView[i][1]);
					}
					XMessageBox.Show(text);
				}
				else
				{
					XMessageBox.Show("----");
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x001F6DF0 File Offset: 0x001F5DF0
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

		// Token: 0x06001833 RID: 6195 RVA: 0x001F700C File Offset: 0x001F600C
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

		// Token: 0x06001834 RID: 6196 RVA: 0x001F7218 File Offset: 0x001F6218
		private void loadPrivilegeData()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadPrivilegeData Start");
			if (!string.IsNullOrEmpty(this.strSelectedDoors))
			{
				string[] array = this.strSelectedDoors.Split(new char[] { ',' });
				if (array.Length > 0)
				{
					DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
					for (int i = 0; i < array.Length; i++)
					{
						for (int j = 0; j < table.Rows.Count; j++)
						{
							if (int.Parse(array[i]) == (int)table.Rows[j]["f_DoorID"])
							{
								table.Rows[j]["f_Selected"] = 1;
								break;
							}
						}
					}
				}
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x001F72F4 File Offset: 0x001F62F4
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

		// Token: 0x06001836 RID: 6198 RVA: 0x001F73E0 File Offset: 0x001F63E0
		private void oneCardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int num = -1;
			sender.ToString();
			if (sender == this.oneCardToolStripMenuItem)
			{
				num = 1;
			}
			if (sender == this.twoCardToolStripMenuItem)
			{
				num = 2;
			}
			if (sender == this.threeCardToolStripMenuItem)
			{
				num = 3;
			}
			if (sender == this.fourCardsToolStripMenuItem)
			{
				num = 4;
			}
			if (sender == this.fiveCardsToolStripMenuItem)
			{
				num = 5;
			}
			if (sender == this.sixCardsToolStripMenuItem)
			{
				num = 6;
			}
			if (sender == this.clearAllCardsConfigureToolStripMenuItem)
			{
				num = 0;
			}
			DataTable dataTable = new DataTable("DataRow");
			dataTable.Columns.Add("f_DoorID", Type.GetType("System.UInt32"));
			dataTable.Columns.Add("f_MoreCards", Type.GetType("System.UInt32"));
			dataTable.Columns.Add("f_DoorName", Type.GetType("System.String"));
			dataTable.AcceptChanges();
			if (num >= 0)
			{
				if (num == 0)
				{
					if (XMessageBox.Show(CommonStr.strAreYouSure + sender.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						wgAppConfig.setSystemParamValueWithNotes(202, "CheckAccess Door MoreCards 2018-06-08 16:52:15", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
						return;
					}
				}
				else if (XMessageBox.Show(CommonStr.strAreYouSure + sender.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					string text = wgAppConfig.getSystemParamNotes(202);
					if (!string.IsNullOrEmpty(text))
					{
						dataTable = DatatableToJson.JsonToDataTable(text);
					}
					DataView dataView = new DataView(dataTable);
					DataTable dataTable2 = dataTable.Clone();
					if (this.dvSelected.Count > 0)
					{
						for (int i = 0; i < this.dvSelected.Count; i++)
						{
							dataView.RowFilter = string.Format("f_DoorID = {0}", this.dvSelected[i]["f_DoorID"]);
							for (int j = 0; j < dataView.Count; j++)
							{
								dataView[j]["f_MoreCards"] = 0;
							}
							DataRow dataRow = dataTable2.NewRow();
							dataRow[0] = this.dvSelected[i]["f_DoorID"];
							dataRow[1] = num;
							dataRow[2] = this.dvSelected[i]["f_DoorName"];
							dataTable2.Rows.Add(dataRow);
						}
						dataTable.AcceptChanges();
						dataTable2.AcceptChanges();
					}
					dataView.RowFilter = string.Format("f_MoreCards > 0", new object[0]);
					for (int k = 0; k < dataView.Count; k++)
					{
						DataRow dataRow2 = dataTable2.NewRow();
						dataRow2[0] = dataView[k][0];
						dataRow2[1] = dataView[k][1];
						dataRow2[2] = dataView[k][2];
						dataTable2.Rows.Add(dataRow2);
					}
					dataTable2.AcceptChanges();
					text = DatatableToJson.DataTableToJson(dataTable2);
					wgAppConfig.setSystemParamValue(202, "CheckAccess Door MoreCards 2018-06-08 16:52:15", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text);
				}
			}
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x001F76F9 File Offset: 0x001F66F9
		private void toolStripComboBox1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x04003172 RID: 12658
		private DataTable dt;

		// Token: 0x04003173 RID: 12659
		private DataView dv;

		// Token: 0x04003174 RID: 12660
		private DataView dvCheckAccess;

		// Token: 0x04003175 RID: 12661
		private DataView dvSelected;

		// Token: 0x04003176 RID: 12662
		private DataView dvtmp;

		// Token: 0x04003177 RID: 12663
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04003178 RID: 12664
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04003179 RID: 12665
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x0400317A RID: 12666
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x0400317B RID: 12667
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x0400317C RID: 12668
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x0400317D RID: 12669
		private DataSet dsCheckAccess = new DataSet();

		// Token: 0x0400317E RID: 12670
		private string strSelectedDoors = "";

		// Token: 0x0400317F RID: 12671
		private string strZoneFilter = "";
	}
}
