using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic.MultiThread;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000028 RID: 40
	public partial class dfrmPrivilegeSingle : frmN3000
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x0005682C File Offset: 0x0005582C
		public dfrmPrivilegeSingle()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
			wgAppConfig.custDataGridview(ref this.dgvDoors);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000568B4 File Offset: 0x000558B4
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

		// Token: 0x060002D7 RID: 727 RVA: 0x00056AF0 File Offset: 0x00055AF0
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			this.bNeedAdd = true;
			this.selectObject(this.dgvDoors);
			this.updateCount();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00056B0C File Offset: 0x00055B0C
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

		// Token: 0x060002D9 RID: 729 RVA: 0x00056B6E File Offset: 0x00055B6E
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			this.bNeedDelete = true;
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.updateCount();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00056B88 File Offset: 0x00055B88
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

		// Token: 0x060002DB RID: 731 RVA: 0x00056BA8 File Offset: 0x00055BA8
		private void btnFind_Click(object sender, EventArgs e)
		{
			if (this.defaultFindControl == null)
			{
				this.defaultFindControl = this.dgvDoors;
			}
			wgAppConfig.findCall(this.dfrmFind1, this, this.defaultFindControl);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00056BD0 File Offset: 0x00055BD0
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (!this.bNeedAdd && !this.bNeedDelete && !sender.Equals(this.btnOKAndUpload) && !sender.Equals(this.btnLimitReset))
			{
				base.DialogResult = DialogResult.Cancel;
				base.Close();
				return;
			}
			if (!sender.Equals(this.btnOKAndUpload) || XMessageBox.Show(string.Format("{0} {1} ({2}) ?", CommonStr.strAreYouSureUpdate, this.Text, CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString()), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				if (sender.Equals(this.btnLimitReset))
				{
					if (XMessageBox.Show(string.Format("{0} {1}  ?", CommonStr.strAreYouSure, this.btnLimitReset.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
					{
						return;
					}
					this.bEdit = true;
				}
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				try
				{
					if (sender.Equals(this.btnLimitReset))
					{
						wgAppConfig.runSql(" UPDATE t_b_Consumer  SET  f_BeginYMD=" + wgTools.PrepareStr(DateTime.Now, true, icConsumer.gTimeSecondEnabled ? "yyyy-MM-dd HH:mm:ss" : "yyyy-MM-dd") + " WHERE f_ConsumerID =" + this.consumerID.ToString());
					}
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
					this.bEdit = true;
					Cursor.Current = Cursors.WaitCursor;
					wgTools.WriteLine("btnDelete_Click Start");
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
					dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					if (this.bNeedAdd || this.bNeedDelete)
					{
						dbConnection.Open();
						string text = "DELETE FROM  [t_d_Privilege]  WHERE  ";
						object obj2 = text;
						text = string.Concat(new object[] { obj2, " [f_ConsumerID] = (", this.consumerID, " ) " });
						dbCommand.CommandText = text;
						wgTools.WriteLine(text);
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
						for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
						{
							text = "INSERT INTO [t_d_Privilege] (f_ConsumerID, f_DoorID ,f_ControllerID, f_DoorNO, f_ControlSegID)";
							object obj3 = text + " SELECT t_b_Consumer.f_ConsumerID, t_b_Door.f_DoorID, t_b_Door.f_ControllerID ,t_b_Door.f_DoorNO, " + this.dgvSelectedDoors.Rows[i].Cells[4].Value.ToString() + " AS [f_ControlSegID]   FROM t_b_Consumer, t_b_Door  WHERE  ";
							text = string.Concat(new object[] { obj3, " [f_ConsumerID] = (", this.consumerID, " ) " }) + " AND  ( t_b_Door.f_DoorID= " + this.dgvSelectedDoors.Rows[i].Cells[0].Value.ToString() + ")";
							dbCommand.CommandText = text;
							dbCommand.ExecuteNonQuery();
						}
						wgTools.WriteLine("INSERT INTO [t_d_Privilege] End");
						string text2 = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
						for (int j = 0; j < this.dgvSelectedDoors.Rows.Count; j++)
						{
							text = string.Format(text2, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, (int)((DataView)this.dgvSelectedDoors.DataSource)[j]["f_ControllerID"]);
							dbCommand.CommandText = text;
							dbCommand.ExecuteNonQuery();
						}
						dbConnection.Close();
					}
					wgTools.WriteLine("btnDelete_Click End");
					Cursor.Current = Cursors.Default;
					if (sender.Equals(this.btnOK))
					{
						this.dfrmWait1.Hide();
						this.logOperate(this.btnOK);
						base.DialogResult = DialogResult.OK;
						base.Close();
					}
					else
					{
						this.logOperate(this.btnOKAndUpload);
						int num;
						if (!wgTools.gWGYTJ && (num = this.multithreadUpdate()) >= 0)
						{
							if (num == 1)
							{
								this.dfrmWait1.Hide();
								wgAppConfig.wgLog(string.Concat(new string[]
								{
									(sender as Button).Text.Replace("\r\n", ""),
									" ,",
									this.Text,
									",",
									CommonStr.strDoorsNum,
									" = ",
									this.dgvSelectedDoors.RowCount.ToString(),
									",",
									CommonStr.strSuccessfully
								}), EventLogEntryType.Information, null);
								XMessageBox.Show((sender as Button).Text + " " + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								base.DialogResult = DialogResult.OK;
								base.Close();
							}
						}
						else
						{
							ArrayList arrayList3 = new ArrayList();
							using (icController icController = new icController())
							{
								using (icPrivilege icPrivilege = new icPrivilege())
								{
									for (int k = 0; k < this.dgvSelectedDoors.Rows.Count; k++)
									{
										int num2 = (int)((DataView)this.dgvSelectedDoors.DataSource)[k]["f_ControllerID"];
										if (arrayList3.IndexOf(num2) < 0)
										{
											icController.GetInfoFromDBByControllerID(num2);
											if (icController.GetControllerRunInformationIP(-1) <= 0)
											{
												this.dfrmWait1.Hide();
												wgAppConfig.wgLog(string.Concat(new string[]
												{
													(sender as Button).Text.Replace("\r\n", ""),
													" ,",
													this.Text,
													",",
													CommonStr.strDoorsNum,
													" = ",
													this.dgvSelectedDoors.RowCount.ToString(),
													",",
													CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
												}), EventLogEntryType.Information, null);
												XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
												return;
											}
											if (icController.runinfo.registerCardNum == 0U && icPrivilege.ClearAllPrivilegeIP(icController.ControllerSN, icController.IP, icController.PORT) < 0)
											{
												this.dfrmWait1.Hide();
												wgAppConfig.wgLog(string.Concat(new string[]
												{
													(sender as Button).Text.Replace("\r\n", ""),
													" ,",
													this.Text,
													",",
													CommonStr.strDoorsNum,
													" = ",
													this.dgvSelectedDoors.RowCount.ToString(),
													",",
													CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
												}), EventLogEntryType.Information, null);
												XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
												return;
											}
											int num3;
											if (wgTools.doubleParse(icController.runinfo.driverVersion.Substring(1)) >= 5.52 && (wgTools.gWGYTJ || icPrivilege.bAllowUploadUserName))
											{
												num3 = icPrivilege.AddPrivilegeWithUsernameOfOneCardByDB(num2, this.consumerID);
											}
											else
											{
												num3 = icPrivilege.AddPrivilegeOfOneCardByDB(num2, this.consumerID);
											}
											if (num3 < 0)
											{
												string text2 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
												dbConnection.Open();
												for (int l = 0; l < this.dgvSelectedDoors.Rows.Count; l++)
												{
													num2 = (int)((DataView)this.dgvSelectedDoors.DataSource)[l]["f_ControllerID"];
													if (arrayList3.IndexOf(num2) < 0)
													{
														string text = string.Format(text2, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, num2);
														dbCommand.CommandText = text;
														dbCommand.ExecuteNonQuery();
													}
												}
												dbConnection.Close();
												this.dfrmWait1.Hide();
												wgAppConfig.wgLog(string.Concat(new string[]
												{
													(sender as Button).Text.Replace("\r\n", ""),
													" ,",
													this.Text,
													",",
													CommonStr.strDoorsNum,
													" = ",
													this.dgvSelectedDoors.RowCount.ToString(),
													",",
													CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
												}), EventLogEntryType.Information, null);
												XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
												return;
											}
											arrayList3.Add(num2);
										}
									}
									this.dv = new DataView(this.oldTbPrivilege);
									for (int m = 0; m < this.dv.Count; m++)
									{
										int num2 = (int)this.dv[m]["f_ControllerID"];
										if (arrayList3.IndexOf(num2) < 0)
										{
											if (icPrivilege.DelPrivilegeOfOneCardByDB(num2, this.consumerID) < 0)
											{
												string text2 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
												dbConnection.Open();
												for (int n = 0; n < this.dgvSelectedDoors.Rows.Count; n++)
												{
													num2 = (int)((DataView)this.dgvSelectedDoors.DataSource)[n]["f_ControllerID"];
													if (arrayList3.IndexOf(num2) < 0)
													{
														string text = string.Format(text2, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, num2);
														dbCommand.CommandText = text;
														dbCommand.ExecuteNonQuery();
													}
												}
												dbConnection.Close();
												this.dfrmWait1.Hide();
												wgAppConfig.wgLog(string.Concat(new string[]
												{
													(sender as Button).Text.Replace("\r\n", ""),
													" ,",
													this.Text,
													",",
													CommonStr.strDoorsNum,
													" = ",
													this.dgvSelectedDoors.RowCount.ToString(),
													",",
													CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
												}), EventLogEntryType.Information, null);
												XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
												return;
											}
											arrayList3.Add(num2);
										}
									}
								}
							}
							this.dfrmWait1.Hide();
							wgAppConfig.wgLog(string.Concat(new string[]
							{
								(sender as Button).Text.Replace("\r\n", ""),
								" ,",
								this.Text,
								",",
								CommonStr.strDoorsNum,
								" = ",
								this.dgvSelectedDoors.RowCount.ToString(),
								",",
								CommonStr.strSuccessfully
							}), EventLogEntryType.Information, null);
							XMessageBox.Show((sender as Button).Text + " " + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							base.DialogResult = DialogResult.OK;
							base.Close();
						}
					}
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

		// Token: 0x060002DD RID: 733 RVA: 0x000579DC File Offset: 0x000569DC
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

		// Token: 0x060002DE RID: 734 RVA: 0x00057BAC File Offset: 0x00056BAC
		private void cbof_ZoneID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ZoneID);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00057BBC File Offset: 0x00056BBC
		private void cbof_ZoneID_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00057BEC File Offset: 0x00056BEC
		private void dfrmPrivilegeSingle_FormClosed(object sender, FormClosedEventArgs e)
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

		// Token: 0x060002E1 RID: 737 RVA: 0x00057C24 File Offset: 0x00056C24
		private void dfrmPrivilegeSingle_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.watching != null && this.frmCall == null)
			{
				this.watching.StopWatch();
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00057C44 File Offset: 0x00056C44
		private void dfrmPrivilegeSingle_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.Shift && e.KeyValue == 81)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						this.btnLimitReset.Visible = true;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00057CB8 File Offset: 0x00056CB8
		private void dfrmPrivilegeSingle_Load(object sender, EventArgs e)
		{
			try
			{
				if (this.watching == null)
				{
					if (this.frmCall == null)
					{
						if (wgTools.bUDPOnly64 > 0)
						{
							this.watching = frmADCT3000.watchingP64;
						}
						else
						{
							this.watching = new WatchingService();
						}
					}
					else
					{
						(this.frmCall as frmUsers).startWatch();
						this.watching = (this.frmCall as frmUsers).watching;
					}
				}
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
				if (icConsumer.gTimeSecondEnabled && wgAppConfig.getParamValBoolByNO(136))
				{
					this.btnLimitReset.Visible = true;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00057DE8 File Offset: 0x00056DE8
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00057DF5 File Offset: 0x00056DF5
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00057E04 File Offset: 0x00056E04
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

		// Token: 0x060002E7 RID: 743 RVA: 0x00057F48 File Offset: 0x00056F48
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

		// Token: 0x060002E8 RID: 744 RVA: 0x000580A0 File Offset: 0x000570A0
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

		// Token: 0x060002E9 RID: 745 RVA: 0x000582BC File Offset: 0x000572BC
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

		// Token: 0x060002EA RID: 746 RVA: 0x000584C8 File Offset: 0x000574C8
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

		// Token: 0x060002EB RID: 747 RVA: 0x00058598 File Offset: 0x00057598
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
			text = text + " FROM t_d_Privilege  WHERE f_ConsumerID=  " + this.m_consumerID.ToString();
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

		// Token: 0x060002EC RID: 748 RVA: 0x0005881C File Offset: 0x0005781C
		private void loadPrivilegeData_Acc()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadPrivilegeData Start");
			string text = " SELECT f_PrivilegeRecID, f_ControllerID, f_DoorID, f_ControlSegID,' ' as  f_ControlSegName ";
			text = text + " FROM t_d_Privilege  WHERE f_ConsumerID=  " + this.m_consumerID.ToString();
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

		// Token: 0x060002ED RID: 749 RVA: 0x00058A90 File Offset: 0x00057A90
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

		// Token: 0x060002EE RID: 750 RVA: 0x00058B7C File Offset: 0x00057B7C
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

		// Token: 0x060002EF RID: 751 RVA: 0x00058CD4 File Offset: 0x00057CD4
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

		// Token: 0x060002F0 RID: 752 RVA: 0x00058EB0 File Offset: 0x00057EB0
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

		// Token: 0x060002F1 RID: 753 RVA: 0x0005909C File Offset: 0x0005809C
		private int multithreadUpdate()
		{
			if (!wgAppConfig.IsAcceleratorActive)
			{
				return -1;
			}
			bool flag = false;
			dfrmMultiThreadOperation dfrmMultiThreadOperation = new dfrmMultiThreadOperation();
			dfrmMultiThreadOperation.newCardNO4Cardlost = this.consumerID.ToString();
			dfrmMultiThreadOperation.oldCardNO4Cardlost = "";
			dfrmMultiThreadOperation.ConsumerID4Cardlost = this.consumerID;
			ArrayList arrayList = new ArrayList();
			this.dv = new DataView(this.oldTbPrivilege);
			for (int i = 0; i < this.dv.Count; i++)
			{
				int num = (int)this.dv[i]["f_ControllerID"];
				if (arrayList.IndexOf(num) < 0)
				{
					arrayList.Add(num);
				}
			}
			if (this.dv.Count == 0 && this.dgvSelectedDoors.RowCount == 0)
			{
				this.dtDoorTmpSelected = ((DataView)this.dgvSelectedDoors.DataSource).Table.Copy();
				this.dvDoorTmpSelected = new DataView(this.dtDoorTmpSelected);
				this.dvDoorTmpSelected.RowFilter = "";
				foreach (object obj in this.dvDoorTmpSelected)
				{
					DataRowView dataRowView = (DataRowView)obj;
					if (arrayList.IndexOf(int.Parse(dataRowView["f_ControllerID"].ToString())) < 0)
					{
						arrayList.Add(int.Parse(dataRowView["f_ControllerID"].ToString()));
					}
				}
			}
			dfrmMultiThreadOperation.delPrivilegeController = arrayList;
			dfrmMultiThreadOperation.startDownloadCardLostUser();
			DateTime.Now.AddSeconds(20.0);
			while (!dfrmMultiThreadOperation.bComplete4Cardlost)
			{
				Application.DoEvents();
				Thread.Sleep(100);
			}
			if (dfrmMultiThreadOperation.bCompleteFullOK4Cardlost)
			{
				flag = true;
			}
			dfrmMultiThreadOperation.Close();
			this.dfrmWait1.Location = new Point(this.dfrmWait1.Location.X, 0);
			this.dfrmWait1.Hide();
			this.Refresh();
			Application.DoEvents();
			if (!flag)
			{
				this.dfrmWait1.Hide();
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					this.btnOKAndUpload.Text.Replace("\r\n", ""),
					" ,",
					this.Text,
					",",
					CommonStr.strDoorsNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString(),
					",",
					CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
				}), EventLogEntryType.Information, null);
				XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return 0;
			}
			return 1;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0005938C File Offset: 0x0005838C
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

		// Token: 0x060002F3 RID: 755 RVA: 0x00059590 File Offset: 0x00058590
		private void updateCount()
		{
			this.lblOptional.Text = this.dgvDoors.RowCount.ToString();
			this.lblSeleted.Text = this.dgvSelectedDoors.RowCount.ToString();
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x000595D9 File Offset: 0x000585D9
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x000595E1 File Offset: 0x000585E1
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

		// Token: 0x04000590 RID: 1424
		private ArrayList arrCommonUsersID;

		// Token: 0x04000591 RID: 1425
		private ArrayList arrCommonUsersLoad;

		// Token: 0x04000592 RID: 1426
		private ArrayList arrCommonUsersName;

		// Token: 0x04000593 RID: 1427
		private bool bEdit;

		// Token: 0x04000594 RID: 1428
		private bool bNeedAdd;

		// Token: 0x04000595 RID: 1429
		private bool bNeedDelete;

		// Token: 0x04000596 RID: 1430
		private Control defaultFindControl;

		// Token: 0x04000597 RID: 1431
		private DataTable dt;

		// Token: 0x04000598 RID: 1432
		private DataTable dtCommonUserPrivilege;

		// Token: 0x04000599 RID: 1433
		private DataTable dtDoorTmpSelected;

		// Token: 0x0400059A RID: 1434
		private DataView dv;

		// Token: 0x0400059B RID: 1435
		private DataView dvCommonUserPrivilege;

		// Token: 0x0400059C RID: 1436
		private DataView dvDoorTmpSelected;

		// Token: 0x0400059D RID: 1437
		private DataView dvSelected;

		// Token: 0x0400059E RID: 1438
		private DataView dvSelectedControllerID;

		// Token: 0x0400059F RID: 1439
		private DataView dvtmp;

		// Token: 0x040005A0 RID: 1440
		private int m_consumerID;

		// Token: 0x040005A1 RID: 1441
		private DataTable oldTbPrivilege;

		// Token: 0x040005A2 RID: 1442
		private DataTable tbPrivilege;

		// Token: 0x040005A3 RID: 1443
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x040005A4 RID: 1444
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x040005A5 RID: 1445
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x040005A6 RID: 1446
		private int[] controlSegIDList = new int[256];

		// Token: 0x040005A7 RID: 1447
		private string[] controlSegNameList = new string[256];

		// Token: 0x040005A9 RID: 1449
		public Form frmCall;

		// Token: 0x040005AA RID: 1450
		private string strZoneFilter = "";

		// Token: 0x040005AB RID: 1451
		public WatchingService watching;
	}
}
