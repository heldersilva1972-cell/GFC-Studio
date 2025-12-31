using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x02000319 RID: 793
	public partial class dfrmPrivilegeTypeDoors : frmN3000
	{
		// Token: 0x06001864 RID: 6244 RVA: 0x001FC85C File Offset: 0x001FB85C
		public dfrmPrivilegeTypeDoors()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x001FC8FC File Offset: 0x001FB8FC
		private void btnAddAllDoors_Click(object sender, EventArgs e)
		{
			this.bNeedAdd = true;
			DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
			if (this.cbof_ZoneID.SelectedIndex <= 0 && this.cbof_ZoneID.Text == CommonStr.strAllZones)
			{
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if ((int)table.Rows[i]["f_Selected"] != 1)
					{
						table.Rows[i]["f_Selected"] = 1;
						table.Rows[i]["f_ControlSegID"] = this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex];
						for (int j = 0; j < this.controlSegIDList.Length; j++)
						{
							if (this.controlSegIDList[j] == (int)table.Rows[i]["f_ControlSegID"])
							{
								table.Rows[i]["f_ControlSegName"] = this.controlSegNameList[j];
								break;
							}
						}
					}
				}
			}
			else
			{
				this.dvtmp = new DataView((this.dgvDoors.DataSource as DataView).Table);
				this.dvtmp.RowFilter = string.Format("  {0} ", this.strZoneFilter);
				for (int k = 0; k < this.dvtmp.Count; k++)
				{
					this.dvtmp[k]["f_ControlSegID"] = this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex];
					for (int l = 0; l < this.controlSegIDList.Length; l++)
					{
						if (this.controlSegIDList[l] == (int)this.dvtmp[k]["f_ControlSegID"])
						{
							this.dvtmp[k]["f_ControlSegName"] = this.controlSegNameList[l];
							break;
						}
					}
					this.dvtmp[k]["f_Selected"] = 1;
				}
			}
			this.updateCount();
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x001FCB38 File Offset: 0x001FBB38
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			this.bNeedAdd = true;
			this.selectObject(this.dgvDoors);
			this.updateCount();
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x001FCB54 File Offset: 0x001FBB54
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			this.bNeedDelete = true;
			DataTable table = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				table.Rows[i]["f_Selected"] = 0;
			}
			this.updateCount();
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x001FCBB6 File Offset: 0x001FBBB6
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.bNeedDelete = true;
			this.updateCount();
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x001FCBD0 File Offset: 0x001FBBD0
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

		// Token: 0x0600186A RID: 6250 RVA: 0x001FCBF0 File Offset: 0x001FBBF0
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (!this.bNeedAdd && !this.bNeedDelete)
			{
				base.DialogResult = DialogResult.Cancel;
				base.Close();
				return;
			}
			string text = "";
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSureAgain + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK && (!sender.Equals(this.btnOKAndUpload) || XMessageBox.Show(string.Format("{0} {1} ({2}) ?", CommonStr.strAreYouSureUpdate, this.Text, CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString()), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK))
			{
				this.logOperate(sender as Button);
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				try
				{
					this.bEdit = true;
					Cursor.Current = Cursors.WaitCursor;
					wgTools.WriteLine("btnDelete_Click Start");
					DbConnection dbConnection;
					DbCommand dbCommand;
					if (wgAppConfig.IsAccessDB)
					{
						dbConnection = new OleDbConnection(wgAppConfig.dbConString);
						dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
					}
					else
					{
						dbConnection = new SqlConnection(wgAppConfig.dbConString);
						dbCommand = new SqlCommand("", dbConnection as SqlConnection);
					}
					dbConnection.Open();
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
					int num = 0;
					dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					DataTable dataTable = new DataTable("Users");
					DataSet dataSet = new DataSet();
					string text2 = "SELECT * FROM t_b_Consumer WHERE [f_PrivilegeTypeID] = (" + this.m_privilegeTypeID + " ) ";
					using (DbConnection dbConnection2 = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
					{
						using (DbCommand dbCommand2 = (wgAppConfig.IsAccessDB ? new OleDbCommand(text2, (OleDbConnection)dbConnection2) : new SqlCommand(text2, (SqlConnection)dbConnection2)))
						{
							using (DataAdapter dataAdapter = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand2) : new SqlDataAdapter((SqlCommand)dbCommand2)))
							{
								dataAdapter.Fill(dataSet);
							}
						}
					}
					dataTable = dataSet.Tables[0];
					this.dvDoorTmpSelected.RowFilter = "f_Selected > 0";
					if (this.bNeedDelete)
					{
						string text3 = "0";
						ArrayList arrayList3 = new ArrayList();
						for (int i = 0; i < this.oldTbPrivilege.Rows.Count; i++)
						{
							bool flag = false;
							for (int j = 0; j < this.dvDoorTmpSelected.Count; j++)
							{
								if ((int)this.oldTbPrivilege.Rows[i]["f_DoorID"] == (int)this.dvDoorTmpSelected[j]["f_DoorID"] && (int)this.oldTbPrivilege.Rows[i]["f_ControlSegID"] == (int)this.dvDoorTmpSelected[j]["f_ControlSegID"])
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								arrayList3.Add((int)this.oldTbPrivilege.Rows[i]["f_DoorID"]);
								text3 = text3 + "," + (int)this.oldTbPrivilege.Rows[i]["f_DoorID"];
							}
						}
						if (arrayList3.Count > 0)
						{
							this.progressBar1.Value = 0;
							this.progressBar1.Maximum = arrayList3.Count;
							this.progressBar1.Value = 1;
							for (int k = 0; k < arrayList3.Count; k++)
							{
								text2 = "DELETE FROM  [t_d_Privilege_Of_PrivilegeType]  WHERE  ";
								object obj2 = text2;
								object obj3 = string.Concat(new object[] { obj2, " [f_ConsumerID] = (", this.m_privilegeTypeID, " ) " });
								text2 = string.Concat(new object[]
								{
									obj3,
									" AND [f_DoorID] = (",
									arrayList3[k],
									" ) "
								});
								dbCommand.CommandText = text2;
								wgTools.WriteLine(text2);
								dbCommand.ExecuteNonQuery();
								wgTools.WriteLine("DELETE FROM  [t_d_Privilege_Of_PrivilegeType] End");
								if (sender == this.btnOK)
								{
									text2 = "SELECT TOP 1 f_ConsumerID FROM  [t_d_Privilege]  WHERE  ";
									object obj4 = text2;
									text2 = string.Concat(new object[]
									{
										obj4,
										"  [f_DoorID] = (",
										arrayList3[k],
										" ) "
									});
									dbCommand.CommandText = text2;
									wgTools.WriteLine(text2);
									if (!string.IsNullOrEmpty(wgTools.SetObjToStr(dbCommand.ExecuteScalar())))
									{
										int num2 = 2000;
										int l = 0;
										this.progressBar2.Value = 0;
										this.progressBar2.Maximum = dataTable.Rows.Count;
										this.progressBar2.Value = Math.Min(1, this.progressBar2.Maximum);
										while (l < dataTable.Rows.Count)
										{
											string text4 = "";
											while (l < dataTable.Rows.Count)
											{
												text4 = text4 + dataTable.Rows[l]["f_ConsumerID"] + ",";
												l++;
												if (text4.Length > num2)
												{
													break;
												}
											}
											text4 += "0";
											text2 = "DELETE FROM  [t_d_Privilege]  WHERE  ";
											object obj5 = text2;
											text2 = string.Concat(new object[]
											{
												obj5,
												"  [f_DoorID] = (",
												arrayList3[k],
												" ) "
											}) + " AND [f_ConsumerID] IN (" + text4 + " ) ";
											dbCommand.CommandText = text2;
											wgTools.WriteLine(text2);
											dbCommand.ExecuteNonQuery();
											wgTools.WriteLine("INSERT INTO [t_d_Privilege] End");
											this.progressBar2.Value = l;
										}
										this.progressBar1.Value = k + 1;
										Application.DoEvents();
									}
								}
							}
						}
					}
					if (this.bNeedAdd)
					{
						string text5 = "0";
						ArrayList arrayList4 = new ArrayList();
						ArrayList arrayList5 = new ArrayList();
						ArrayList arrayList6 = new ArrayList();
						ArrayList arrayList7 = new ArrayList();
						for (int m = 0; m < this.dvDoorTmpSelected.Count; m++)
						{
							bool flag2 = false;
							for (int n = 0; n < this.oldTbPrivilege.Rows.Count; n++)
							{
								if ((int)this.oldTbPrivilege.Rows[n]["f_DoorID"] == (int)this.dvDoorTmpSelected[m]["f_DoorID"] && (int)this.oldTbPrivilege.Rows[n]["f_ControlSegID"] == (int)this.dvDoorTmpSelected[m]["f_ControlSegID"])
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								arrayList4.Add((int)this.dvDoorTmpSelected[m]["f_DoorID"]);
								arrayList6.Add((byte)this.dvDoorTmpSelected[m]["f_DoorNO"]);
								arrayList7.Add((int)this.dvDoorTmpSelected[m]["f_ControllerID"]);
								arrayList5.Add((int)this.dvDoorTmpSelected[m]["f_ControlSegID"]);
								text5 = text5 + "," + (int)this.dvDoorTmpSelected[m]["f_DoorID"];
							}
						}
						if (arrayList4.Count > 0)
						{
							this.progressBar1.Value = 0;
							this.progressBar1.Maximum = arrayList4.Count;
							this.progressBar1.Value = 1;
							Application.DoEvents();
							for (int num3 = 0; num3 < arrayList4.Count; num3++)
							{
								text2 = "INSERT INTO [t_d_Privilege_Of_PrivilegeType] (f_ConsumerID, f_DoorID ,f_ControllerID, f_DoorNO, f_ControlSegID)";
								object obj6 = string.Concat(new string[]
								{
									text2,
									string.Format(" SELECT  {0} as f_ConsumerID ", this.m_privilegeTypeID),
									" , t_b_Door.f_DoorID, t_b_Door.f_ControllerID ,t_b_Door.f_DoorNO, ",
									arrayList5[num3].ToString(),
									" AS [f_ControlSegID]   FROM  t_b_Door  WHERE  "
								});
								text2 = string.Concat(new object[]
								{
									obj6,
									"   ( t_b_Door.f_DoorID= ",
									arrayList4[num3],
									")"
								});
								dbCommand.CommandText = text2;
								wgTools.WriteLine(text2);
								dbCommand.ExecuteNonQuery();
								wgTools.WriteLine("INSERT INTO   [t_d_Privilege_Of_PrivilegeType] End");
								if (sender == this.btnOK)
								{
									int num4 = 2000;
									int num5 = 0;
									this.progressBar2.Value = 0;
									this.progressBar2.Maximum = dataTable.Rows.Count;
									this.progressBar2.Value = Math.Min(1, this.progressBar2.Maximum);
									while (num5 < dataTable.Rows.Count)
									{
										string text6 = "";
										while (num5 < dataTable.Rows.Count)
										{
											text6 = text6 + dataTable.Rows[num5]["f_ConsumerID"] + ",";
											num5++;
											if (text6.Length > num4)
											{
												break;
											}
										}
										text6 += "0";
										text2 = "INSERT INTO [t_d_Privilege] (f_ConsumerID, f_DoorID ,f_ControllerID, f_DoorNO, f_ControlSegID)";
										text2 = string.Concat(new string[]
										{
											text2,
											" SELECT t_b_Consumer.f_ConsumerID, ",
											arrayList4[num3].ToString(),
											" AS f_DoorID, ",
											arrayList7[num3].ToString(),
											" AS f_ControllerID, ",
											arrayList6[num3].ToString(),
											" AS f_DoorNO, ",
											arrayList5[num3].ToString(),
											" AS [f_ControlSegID]   FROM t_b_Consumer  WHERE   [f_ConsumerID] IN (",
											text6,
											" ) "
										});
										dbCommand.CommandText = text2;
										wgTools.WriteLine(text2);
										dbCommand.ExecuteNonQuery();
										wgTools.WriteLine("INSERT INTO [t_d_Privilege] End");
										this.progressBar2.Value = num5;
									}
								}
								num++;
								this.progressBar1.Value = num3 + 1;
								Application.DoEvents();
							}
						}
					}
					string text7 = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
					for (int num6 = 0; num6 < this.dgvSelectedDoors.Rows.Count; num6++)
					{
						text2 = string.Format(text7, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, (int)((DataView)this.dgvSelectedDoors.DataSource)[num6]["f_ControllerID"]);
						dbCommand.CommandText = text2;
						dbCommand.ExecuteNonQuery();
					}
					dbConnection.Close();
					wgTools.WriteLine("btnDelete_Click End");
					Cursor.Current = Cursors.Default;
					base.DialogResult = DialogResult.OK;
					wgAppConfig.wgLog(this.Text + " " + CommonStr.strSuccessfully);
					XMessageBox.Show(this.Text + " " + CommonStr.strSuccessfully);
					base.Close();
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				finally
				{
					this.dfrmWait1.Hide();
				}
			}
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x001FD9DC File Offset: 0x001FC9DC
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

		// Token: 0x0600186C RID: 6252 RVA: 0x001FDBAC File Offset: 0x001FCBAC
		private void cbof_ZoneID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ZoneID);
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x001FDBBC File Offset: 0x001FCBBC
		private void dfrmPrivilegeTypeDoors_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.dfrmWait1 != null)
				{
					this.dfrmWait1.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x001FDBF4 File Offset: 0x001FCBF4
		private void dfrmPrivilegeTypeDoors_Load(object sender, EventArgs e)
		{
			try
			{
				this.label1.Visible = wgAppConfig.getParamValBoolByNO(121);
				this.cbof_ControlSegID.Visible = wgAppConfig.getParamValBoolByNO(121);
				this.dgvSelectedDoors.Columns[4].Visible = wgAppConfig.getParamValBoolByNO(121);
				this.dgvSelectedDoors.Columns[5].Visible = wgAppConfig.getParamValBoolByNO(121);
				this.loadControlSegData();
				this.loadZoneInfo();
				this.loadDoorData();
				this.loadPrivilegeData();
				this.updateCount();
				this.mnuDoorTypeSelect_loadData();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x001FDCB0 File Offset: 0x001FCCB0
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x001FDCBD File Offset: 0x001FCCBD
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x001FDCCC File Offset: 0x001FCCCC
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

		// Token: 0x06001872 RID: 6258 RVA: 0x001FDE10 File Offset: 0x001FCE10
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

		// Token: 0x06001873 RID: 6259 RVA: 0x001FDF68 File Offset: 0x001FCF68
		private void loadDoorData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadDoorData_Acc();
				return;
			}
			string text = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, 1 as f_ControlSegID,' ' as f_ControlSegName, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  AND ( b.f_ControllerSN < 160000000 OR b.f_ControllerSN > 180000000 )  ORDER BY  a.f_DoorName ";
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

		// Token: 0x06001874 RID: 6260 RVA: 0x001FE184 File Offset: 0x001FD184
		private void loadDoorData_Acc()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, 1 as f_ControlSegID,' ' as f_ControlSegName, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  AND ( b.f_ControllerSN < 160000000 OR b.f_ControllerSN > 180000000 )  ORDER BY  a.f_DoorName ";
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

		// Token: 0x06001875 RID: 6261 RVA: 0x001FE390 File Offset: 0x001FD390
		private void loadDoorType(int consumerID)
		{
			string text = " SELECT f_ControllerID, f_DoorID, f_ConsumerID, f_ControlSegID ";
			object obj = text;
			text = string.Concat(new object[] { obj, " FROM t_d_Privilege  WHERE f_ConsumerID =   ", consumerID, " " });
			if (this.dtCommonUserPrivilege == null)
			{
				this.dtCommonUserPrivilege = new DataTable();
			}
			DbConnection dbConnection;
			DbCommand dbCommand;
			DataAdapter dataAdapter;
			if (wgAppConfig.IsAccessDB)
			{
				dbConnection = new OleDbConnection(wgAppConfig.dbConString);
				dbCommand = new OleDbCommand(text, dbConnection as OleDbConnection);
				dataAdapter = new OleDbDataAdapter(dbCommand as OleDbCommand);
				(dataAdapter as OleDbDataAdapter).Fill(this.dtCommonUserPrivilege);
				return;
			}
			dbConnection = new SqlConnection(wgAppConfig.dbConString);
			dbCommand = new SqlCommand(text, dbConnection as SqlConnection);
			dataAdapter = new SqlDataAdapter(dbCommand as SqlCommand);
			(dataAdapter as SqlDataAdapter).Fill(this.dtCommonUserPrivilege);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x001FE460 File Offset: 0x001FD460
		private void loadPrivilegeData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadPrivilegeData_Acc();
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadPrivilegeData Start");
			string text = " SELECT f_PrivilegeRecID, f_ControllerID, f_DoorID, f_ControlSegID,' ' as  f_ControlSegName ";
			text = text + " FROM t_d_Privilege_Of_PrivilegeType  WHERE f_ConsumerID=  " + this.m_privilegeTypeID.ToString();
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						this.tbPrivilege = new DataTable();
						sqlDataAdapter.Fill(this.tbPrivilege);
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
										table.Rows[j]["f_Selected"] = 1;
										table.Rows[j]["f_ControlSegID"] = this.dv[i]["f_ControlSegID"];
										for (int k = 0; k < this.controlSegIDList.Length; k++)
										{
											if (this.controlSegIDList[k] == (int)table.Rows[j]["f_ControlSegID"])
											{
												table.Rows[j]["f_ControlSegName"] = this.controlSegNameList[k];
												break;
											}
										}
										break;
									}
								}
							}
						}
					}
				}
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x001FE6E4 File Offset: 0x001FD6E4
		private void loadPrivilegeData_Acc()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadPrivilegeData Start");
			string text = " SELECT f_PrivilegeRecID, f_ControllerID, f_DoorID, f_ControlSegID,' ' as  f_ControlSegName ";
			text = text + " FROM t_d_Privilege_Of_PrivilegeType  WHERE f_ConsumerID=  " + this.m_privilegeTypeID.ToString();
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
					{
						this.tbPrivilege = new DataTable();
						oleDbDataAdapter.Fill(this.tbPrivilege);
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
										table.Rows[j]["f_Selected"] = 1;
										table.Rows[j]["f_ControlSegID"] = this.dv[i]["f_ControlSegID"];
										for (int k = 0; k < this.controlSegIDList.Length; k++)
										{
											if (this.controlSegIDList[k] == (int)table.Rows[j]["f_ControlSegID"])
											{
												table.Rows[j]["f_ControlSegName"] = this.controlSegNameList[k];
												break;
											}
										}
										break;
									}
								}
							}
						}
					}
				}
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x001FE958 File Offset: 0x001FD958
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

		// Token: 0x06001879 RID: 6265 RVA: 0x001FEA44 File Offset: 0x001FDA44
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

		// Token: 0x0600187A RID: 6266 RVA: 0x001FEB9C File Offset: 0x001FDB9C
		private void mnuDoorTypeSelect_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				if (this.dvCommonUserPrivilege != null)
				{
					for (int i = 0; i < this.arrCommonUsersName.Count; i++)
					{
						if (this.arrCommonUsersName[i].ToString().Equals(e.ClickedItem.Text))
						{
							if ((int)this.arrCommonUsersLoad[i] == 0)
							{
								this.loadDoorType((int)this.arrCommonUsersID[i]);
								this.arrCommonUsersLoad[i] = 1;
							}
							this.dvCommonUserPrivilege.RowFilter = " f_ConsumerID = " + this.arrCommonUsersID[i].ToString();
							DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
							if (this.dvCommonUserPrivilege.Count > 0)
							{
								for (int j = 0; j < this.dvCommonUserPrivilege.Count; j++)
								{
									for (int k = 0; k < table.Rows.Count; k++)
									{
										if ((int)table.Rows[k]["f_DoorID"] == (int)this.dvCommonUserPrivilege[j]["f_DoorID"])
										{
											table.Rows[k]["f_Selected"] = 1;
											table.Rows[k]["f_ControlSegID"] = this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex];
										}
									}
								}
							}
							this.bNeedAdd = true;
							this.updateCount();
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x001FED78 File Offset: 0x001FDD78
		private void mnuDoorTypeSelect_loadData()
		{
			this.mnuDoorTypeSelect.Items.Clear();
			this.arrCommonUsersID = new ArrayList();
			this.arrCommonUsersName = new ArrayList();
			this.arrCommonUsersLoad = new ArrayList();
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			wgAppConfig.getSystemParamValue(154, out text, out text2, out text3, out text4);
			string text5 = text3;
			if (!string.IsNullOrEmpty(text5))
			{
				string text6 = "";
				this.dtCommonUserPrivilege = new DataTable();
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand(text6, dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand(text6, dbConnection as SqlConnection);
				}
				this.dvCommonUserPrivilege = new DataView(this.dtCommonUserPrivilege);
				try
				{
					if (dbConnection.State != ConnectionState.Open)
					{
						dbConnection.Open();
					}
					text6 = " SELECT * ";
					text6 = text6 + " FROM t_b_Consumer  WHERE f_ConsumerID IN (  " + text5 + ")  ORDER BY f_ConsumerName ASC";
					dbCommand.CommandText = text6;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						this.arrCommonUsersID.Add((int)dbDataReader["f_ConsumerID"]);
						this.arrCommonUsersName.Add(wgTools.SetObjToStr(dbDataReader["f_ConsumerName"]));
						this.arrCommonUsersLoad.Add(0);
						ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(wgTools.SetObjToStr(dbDataReader["f_ConsumerName"]));
						this.mnuDoorTypeSelect.Items.Add(toolStripMenuItem);
					}
					dbDataReader.Close();
					dbConnection.Close();
				}
				catch (Exception ex)
				{
					wgAppConfig.wgDebugWrite(ex.ToString());
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x001FEF64 File Offset: 0x001FDF64
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

		// Token: 0x0600187D RID: 6269 RVA: 0x001FF168 File Offset: 0x001FE168
		private void updateCount()
		{
			this.lblOptional.Text = this.dgvDoors.RowCount.ToString();
			this.lblSeleted.Text = this.dgvSelectedDoors.RowCount.ToString();
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600187E RID: 6270 RVA: 0x001FF1B1 File Offset: 0x001FE1B1
		// (set) Token: 0x0600187F RID: 6271 RVA: 0x001FF1B9 File Offset: 0x001FE1B9
		public int PrivilegeTypeID
		{
			get
			{
				return this.m_privilegeTypeID;
			}
			set
			{
				this.m_privilegeTypeID = value;
			}
		}

		// Token: 0x0400320E RID: 12814
		private ArrayList arrCommonUsersID;

		// Token: 0x0400320F RID: 12815
		private ArrayList arrCommonUsersLoad;

		// Token: 0x04003210 RID: 12816
		private ArrayList arrCommonUsersName;

		// Token: 0x04003211 RID: 12817
		private bool bEdit;

		// Token: 0x04003212 RID: 12818
		private bool bNeedAdd;

		// Token: 0x04003213 RID: 12819
		private bool bNeedDelete;

		// Token: 0x04003214 RID: 12820
		private DataTable dt;

		// Token: 0x04003215 RID: 12821
		private DataTable dtCommonUserPrivilege;

		// Token: 0x04003216 RID: 12822
		private DataTable dtDoorTmpSelected;

		// Token: 0x04003217 RID: 12823
		private DataView dv;

		// Token: 0x04003218 RID: 12824
		private DataView dvCommonUserPrivilege;

		// Token: 0x04003219 RID: 12825
		private DataView dvDoorTmpSelected;

		// Token: 0x0400321A RID: 12826
		private DataView dvSelected;

		// Token: 0x0400321B RID: 12827
		private DataView dvSelectedControllerID;

		// Token: 0x0400321C RID: 12828
		private DataView dvtmp;

		// Token: 0x0400321D RID: 12829
		private int m_privilegeTypeID;

		// Token: 0x0400321E RID: 12830
		private DataTable oldTbPrivilege;

		// Token: 0x0400321F RID: 12831
		private DataTable tbPrivilege;

		// Token: 0x04003220 RID: 12832
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x04003221 RID: 12833
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x04003222 RID: 12834
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x04003223 RID: 12835
		private int[] controlSegIDList = new int[256];

		// Token: 0x04003224 RID: 12836
		private string[] controlSegNameList = new string[256];

		// Token: 0x04003226 RID: 12838
		private string strZoneFilter = "";
	}
}
