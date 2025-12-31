using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic.MultiThread;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000025 RID: 37
	public partial class dfrmPrivilege : frmN3000
	{
		// Token: 0x06000262 RID: 610 RVA: 0x00047DAC File Offset: 0x00046DAC
		public dfrmPrivilege()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00047E98 File Offset: 0x00046E98
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			e.Result = this.loadUserData4BackWork();
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00047EDC File Offset: 0x00046EDC
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				wgTools.WgDebugWrite("Users Operation was canceled", new object[0]);
				return;
			}
			if (e.Error != null)
			{
				wgTools.WgDebugWrite(string.Format("An error occurred: {0}", e.Error.Message), new object[0]);
				return;
			}
			this.loadUserData4BackWorkComplete(e.Result as DataTable);
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString());
			this.defaultFindControl = this.dgvUsers;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00047F68 File Offset: 0x00046F68
		private bool bAllowedDoor(int doorID, int doorZoneID)
		{
			if (this.arrZoneID.Count <= 0)
			{
				return true;
			}
			if (string.IsNullOrEmpty(this.arrZoneName[0].ToString()))
			{
				return true;
			}
			for (int i = 0; i < this.arrZoneID.Count; i++)
			{
				if (doorZoneID == (int)this.arrZoneID[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00047FD0 File Offset: 0x00046FD0
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

		// Token: 0x06000267 RID: 615 RVA: 0x000480D8 File Offset: 0x000470D8
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			if (this.strGroupFilter == "")
			{
				icConsumerShare.selectAllUsers();
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
				if (this.dgvSelectedUsers.RowCount > this.allowUpdateMax)
				{
					this.lblOver1000.Visible = true;
					return;
				}
			}
			else
			{
				wgTools.WriteLine("btnAddAllUsers_Click Start");
				this.dt = ((DataView)this.dgvUsers.DataSource).Table;
				this.dv1 = (DataView)this.dgvUsers.DataSource;
				this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
				this.dgvUsers.DataSource = null;
				this.dgvSelectedUsers.DataSource = null;
				if (this.strGroupFilter != "")
				{
					string rowFilter = this.dv1.RowFilter;
					string rowFilter2 = this.dv2.RowFilter;
					this.dv1.Dispose();
					this.dv2.Dispose();
					this.dv1 = null;
					this.dv2 = null;
					this.dt.BeginLoadData();
					this.dv = new DataView(this.dt);
					this.dv.RowFilter = this.strGroupFilter;
					for (int i = 0; i < this.dv.Count; i++)
					{
						this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
					}
					this.dt.EndLoadData();
					this.dv1 = new DataView(this.dt);
					this.dv1.RowFilter = rowFilter;
					this.dv2 = new DataView(this.dt);
					this.dv2.RowFilter = rowFilter2;
					this.dgvUsers.DataSource = this.dv1;
					this.dgvSelectedUsers.DataSource = this.dv2;
					wgTools.WriteLine("btnAddAllUsers_Click End");
					if (this.dv2.Count > this.allowUpdateMax)
					{
						this.lblOver1000.Visible = true;
					}
					Cursor.Current = Cursors.Default;
				}
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00048328 File Offset: 0x00047328
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvDoors);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00048335 File Offset: 0x00047335
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
			if (this.dgvSelectedUsers.RowCount > this.allowUpdateMax)
			{
				this.lblOver1000.Visible = true;
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00048366 File Offset: 0x00047366
		private void btnAddPass_Click(object sender, EventArgs e)
		{
			this.btnAddPassAndUpload_Click(sender, e);
			this.bEdit = true;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00048378 File Offset: 0x00047378
		private void btnAddPassAndUpload_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnAddPassAndUpload_Click_Acc(sender, e);
				return;
			}
			if (XMessageBox.Show(string.Concat(new string[]
			{
				(sender as Button).Text,
				" \r\n\r\n",
				CommonStr.strUsersNum,
				" = ",
				this.dgvSelectedUsers.RowCount.ToString(),
				"\r\n\r\n",
				CommonStr.strDoorsNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString(),
				"? "
			}), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK && this.dgvSelectedDoors.Rows.Count > 0 && this.dgvSelectedUsers.Rows.Count > 0)
			{
				this.bEdit = true;
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(sender as Button).Text,
					" ,",
					CommonStr.strUsersNum,
					" = ",
					this.dgvSelectedUsers.RowCount.ToString(),
					",",
					CommonStr.strDoorsNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString()
				}));
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnAddPass_Click Start");
				this.cn = new SqlConnection(wgAppConfig.dbConString);
				try
				{
					this.cn.Open();
					this.cmd = new SqlCommand("", this.cn);
					try
					{
						int i = 0;
						i = 0;
						this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount * this.dgvSelectedUsers.RowCount;
						bool flag = true;
						flag = true;
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
						int num2 = 0;
						while (i < this.dgvSelectedDoors.Rows.Count)
						{
							string text = "";
							int num3 = 0;
							if (this.cbof_ZoneID.Items[0].ToString() == CommonStr.strAllZones && this.dtDoorTmpSelected.Rows.Count == this.dgvSelectedDoors.RowCount)
							{
								i = this.dgvSelectedDoors.Rows.Count;
								this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
							}
							else
							{
								this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
								if (arrayList.Count > 0 && num < arrayList.Count)
								{
									text = text + " [f_ControllerID] = ( " + arrayList[num].ToString() + ")";
									this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", arrayList[num].ToString());
									num++;
									num3 = this.dvSelectedControllerID.Count;
									i += this.dvSelectedControllerID.Count;
								}
								else
								{
									if (this.dvDoorTmpSelected.Count <= num2)
									{
										break;
									}
									text = text + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num2]["f_DoorID"].ToString() + ")";
									num2++;
									num3 = 1;
									i++;
								}
							}
							int num4 = 2000;
							int j = 0;
							while (j < this.dgvSelectedUsers.Rows.Count)
							{
								string text2 = "";
								if (((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
								{
									while (j < this.dgvSelectedUsers.Rows.Count)
									{
										text2 = text2 + ((DataView)this.dgvSelectedUsers.DataSource)[j]["f_ConsumerID"] + ",";
										j++;
										if (text2.Length > num4)
										{
											break;
										}
									}
									text2 += "0";
								}
								else
								{
									j = this.dgvSelectedUsers.Rows.Count;
								}
								if (flag)
								{
									string text3 = "DELETE FROM  [t_d_Privilege]  WHERE  ";
									if (!string.IsNullOrEmpty(text))
									{
										text3 = text3 + "  ( " + text + ")";
									}
									else
									{
										text3 += "  1>0 ";
									}
									if (text2 != "")
									{
										text3 = text3 + " AND [f_ConsumerID] IN (" + text2 + " ) ";
									}
									if (!wgAppConfig.IsAccessDB && string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2))
									{
										text3 = "TRUNCATE TABLE [t_d_Privilege] ";
									}
									this.cmd.CommandText = text3;
									wgTools.WriteLine(text3);
									this.cmd.ExecuteNonQuery();
									wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
								}
								this.progressBar1.Value = j * num3 + this.dgvSelectedUsers.Rows.Count * (i - num3);
								Application.DoEvents();
							}
						}
						flag = true;
						i = 0;
						num = 0;
						num2 = 0;
						while (i < this.dgvSelectedDoors.Rows.Count)
						{
							string text = "";
							int num5;
							if (arrayList.Count > 0 && num < arrayList.Count)
							{
								text = text + " [f_ControllerID] = ( " + arrayList[num].ToString() + ")";
								this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", arrayList[num].ToString());
								num++;
								num5 = this.dvSelectedControllerID.Count;
								i += this.dvSelectedControllerID.Count;
							}
							else
							{
								if (this.dvDoorTmpSelected.Count <= num2)
								{
									break;
								}
								text = text + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num2]["f_DoorID"].ToString() + ")";
								num2++;
								num5 = 1;
								i++;
							}
							int num6 = 2000;
							int k = 0;
							while (k < this.dgvSelectedUsers.Rows.Count)
							{
								string text4 = "";
								if (((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
								{
									while (k < this.dgvSelectedUsers.Rows.Count)
									{
										text4 = text4 + ((DataView)this.dgvSelectedUsers.DataSource)[k]["f_ConsumerID"] + ",";
										k++;
										if (text4.Length > num6)
										{
											break;
										}
									}
									text4 += "0";
								}
								else
								{
									k = this.dgvSelectedUsers.Rows.Count;
								}
								string text3 = "INSERT INTO [t_d_Privilege] (f_ConsumerID, f_DoorID ,f_ControllerID, f_DoorNO, f_ControlSegID)";
								object obj2 = text3;
								text3 = string.Concat(new object[]
								{
									obj2,
									" SELECT t_b_Consumer.f_ConsumerID, t_b_Door.f_DoorID, t_b_Door.f_ControllerID ,t_b_Door.f_DoorNO, ",
									this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex],
									" AS [f_ControlSegID]  "
								}) + " FROM t_b_Consumer, t_b_Door  WHERE  ((t_b_Consumer.f_DoorEnabled)=1) ";
								if (text4 != "")
								{
									text3 = text3 + " AND [f_ConsumerID] IN (" + text4 + " ) ";
								}
								text3 = text3 + " AND  ( " + text + ")";
								this.cmd.CommandText = text3;
								wgTools.WriteLine(text3);
								this.cmd.ExecuteNonQuery();
								wgTools.WriteLine("INSERT INTO [t_d_Privilege] End");
								this.progressBar1.Value = k * num5 + this.dgvSelectedUsers.Rows.Count * (i - num5);
								Application.DoEvents();
							}
						}
						string text5;
						if (sender.Equals(this.btnAddPass))
						{
							text5 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
						}
						else
						{
							text5 = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
						}
						for (int l = 0; l < this.dgvSelectedDoors.Rows.Count; l++)
						{
							string text3 = string.Format(text5, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, (int)((DataView)this.dgvSelectedDoors.DataSource)[l]["f_ControllerID"]);
							this.cmd.CommandText = text3;
							this.cmd.ExecuteNonQuery();
						}
						wgTools.WriteLine("btnAddPass_Click End");
						Cursor.Current = Cursors.Default;
						this.progressBar1.Value = this.progressBar1.Maximum;
						if (sender.Equals(this.btnAddPass))
						{
							this.dfrmWait1.Hide();
							this.logOperate(this.btnAddPass);
							XMessageBox.Show(string.Concat(new string[]
							{
								(sender as Button).Text,
								" \r\n\r\n",
								CommonStr.strUsersNum,
								" = ",
								this.dgvSelectedUsers.RowCount.ToString(),
								"\r\n\r\n",
								CommonStr.strDoorsNum,
								" = ",
								this.dgvSelectedDoors.RowCount.ToString(),
								"\r\n\r\n",
								CommonStr.strSuccessfully
							}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							this.progressBar1.Value = 0;
						}
						else if (this.lblOver1000.Visible)
						{
							this.dfrmWait1.Hide();
							this.logOperate(this.btnAddPass);
							if (XMessageBox.Show(CommonStr.strNeedUploadPrivilegeOver50User, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
							{
								this.btnConsoleUpload_Click(null, null);
							}
							this.progressBar1.Value = 0;
						}
						else
						{
							this.logOperate(this.btnAddPassAndUpload);
							this.progressBar1.Value = 0;
							int num7;
							if (!wgTools.gWGYTJ && (num7 = this.multithreadUpdate(true)) >= 0)
							{
								if (num7 == 1)
								{
									this.dfrmWait1.Hide();
									wgAppConfig.wgLog(string.Concat(new string[]
									{
										(sender as Button).Text.Replace("\r\n", ""),
										" ,",
										CommonStr.strUsersNum,
										" = ",
										this.dgvSelectedUsers.RowCount.ToString(),
										",",
										CommonStr.strDoorsNum,
										" = ",
										this.dgvSelectedDoors.RowCount.ToString(),
										",",
										CommonStr.strSuccessfully
									}), EventLogEntryType.Information, null);
									Cursor.Current = Cursors.Default;
									this.progressBar1.Value = this.progressBar1.Maximum;
									XMessageBox.Show(string.Concat(new string[]
									{
										(sender as Button).Text,
										" \r\n\r\n",
										CommonStr.strUsersNum,
										" = ",
										this.dgvSelectedUsers.RowCount.ToString(),
										"\r\n\r\n",
										CommonStr.strDoorsNum,
										" = ",
										this.dgvSelectedDoors.RowCount.ToString(),
										"\r\n\r\n",
										CommonStr.strSuccessfully
									}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
									this.progressBar1.Value = 0;
								}
							}
							else
							{
								ArrayList arrayList3 = new ArrayList();
								this.progressBar1.Maximum = this.dgvSelectedDoors.Rows.Count;
								if (this.dgvSelectedUsers.Rows.Count > 0)
								{
									using (icPrivilege icPrivilege = new icPrivilege())
									{
										using (icController icController = new icController())
										{
											for (int m = 0; m < this.dgvSelectedDoors.Rows.Count; m++)
											{
												int num8 = (int)((DataView)this.dgvSelectedDoors.DataSource)[m]["f_ControllerID"];
												string text6 = (string)((DataView)this.dgvSelectedDoors.DataSource)[m]["f_DoorName"];
												if (arrayList3.IndexOf(num8) < 0)
												{
													icController.GetInfoFromDBByControllerID(num8);
													if (icController.GetControllerRunInformationIP(-1) <= 0)
													{
														this.dfrmWait1.Hide();
														wgAppConfig.wgLog(string.Concat(new string[]
														{
															this.btnAddPassAndUpload.Text.Replace("\r\n", ""),
															" ,",
															CommonStr.strUsersNum,
															" = ",
															this.dgvSelectedUsers.RowCount.ToString(),
															",",
															CommonStr.strDoorsNum,
															" = ",
															this.dgvSelectedDoors.RowCount.ToString(),
															",",
															CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
														}), EventLogEntryType.Information, null);
														XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
														this.progressBar1.Value = 0;
														return;
													}
													if (icController.runinfo.registerCardNum == 0U && icPrivilege.ClearAllPrivilegeIP(icController.ControllerSN, icController.IP, icController.PORT) < 0)
													{
														this.dfrmWait1.Hide();
														wgAppConfig.wgLog(string.Concat(new string[]
														{
															this.btnAddPassAndUpload.Text.Replace("\r\n", ""),
															" ,",
															CommonStr.strUsersNum,
															" = ",
															this.dgvSelectedUsers.RowCount.ToString(),
															",",
															CommonStr.strDoorsNum,
															" = ",
															this.dgvSelectedDoors.RowCount.ToString(),
															",",
															CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
														}), EventLogEntryType.Information, null);
														XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
														this.progressBar1.Value = 0;
														return;
													}
													if (wgAppConfig.IsAccessControlBlue)
													{
														icController icController2 = icController;
														if (!wgAppConfig.checkRSAController((long)icController2.ControllerSN) && wgTools.doubleParse(icController2.runinfo.driverVersion.Substring(1)) == 6.62)
														{
															bool flag2 = true;
															string systemParamByNO = wgAppConfig.getSystemParamByNO(49);
															DateTime now = DateTime.Now;
															if (!string.IsNullOrEmpty(systemParamByNO))
															{
																DateTime.TryParse(systemParamByNO, out now);
																if (now < DateTime.Parse("2018-01-01 00:00:00"))
																{
																	flag2 = false;
																}
															}
															if (((icController2.runinfo.swipeEndIndex < 2000U && icController2.runinfo.swipeStartIndex < 1000U) || flag2) && wgAppConfig.getParamValBoolByNO(64))
															{
																string text7 = CommonStr.strDelAddAndUploadFail4False + "\r\n\r\n" + string.Format("!!!6.62--{0}: {1}", icController2.ControllerSN.ToString(), CommonStr.strSupposeUpgradeDriver);
																wgAppConfig.wgLog(text7);
																XMessageBox.Show(text7, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
																this.progressBar1.Value = 0;
																return;
															}
														}
													}
													for (int n = 0; n < this.dgvSelectedUsers.Rows.Count; n++)
													{
														int num9;
														if (wgTools.doubleParse(icController.runinfo.driverVersion.Substring(1)) >= 5.52 && (wgTools.gWGYTJ || icPrivilege.bAllowUploadUserName))
														{
															num9 = icPrivilege.AddPrivilegeWithUsernameOfOneCardByDB(num8, (int)((DataView)this.dgvSelectedUsers.DataSource)[n]["f_ConsumerID"]);
														}
														else
														{
															num9 = icPrivilege.AddPrivilegeOfOneCardByDB(num8, (int)((DataView)this.dgvSelectedUsers.DataSource)[n]["f_ConsumerID"]);
														}
														if (num9 < 0)
														{
															text5 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
															if (this.cn.State != ConnectionState.Open)
															{
																this.cn.Open();
															}
															for (int num10 = 0; num10 < this.dgvSelectedDoors.Rows.Count; num10++)
															{
																num8 = (int)((DataView)this.dgvSelectedDoors.DataSource)[num10]["f_ControllerID"];
																if (arrayList3.IndexOf(num8) < 0)
																{
																	string text3 = string.Format(text5, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, num8);
																	this.cmd.CommandText = text3;
																	this.cmd.ExecuteNonQuery();
																}
															}
															this.dfrmWait1.Hide();
															wgAppConfig.wgLog(string.Concat(new string[]
															{
																this.btnAddPassAndUpload.Text.Replace("\r\n", ""),
																" ,",
																CommonStr.strUsersNum,
																" = ",
																this.dgvSelectedUsers.RowCount.ToString(),
																",",
																CommonStr.strDoorsNum,
																" = ",
																this.dgvSelectedDoors.RowCount.ToString(),
																",",
																CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
															}), EventLogEntryType.Information, null);
															XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
															this.progressBar1.Value = 0;
															return;
														}
													}
													arrayList3.Add(num8);
												}
												this.progressBar1.Value = m + 1;
											}
										}
									}
								}
								this.dfrmWait1.Hide();
								wgAppConfig.wgLog(string.Concat(new string[]
								{
									(sender as Button).Text.Replace("\r\n", ""),
									" ,",
									CommonStr.strUsersNum,
									" = ",
									this.dgvSelectedUsers.RowCount.ToString(),
									",",
									CommonStr.strDoorsNum,
									" = ",
									this.dgvSelectedDoors.RowCount.ToString(),
									",",
									CommonStr.strSuccessfully
								}), EventLogEntryType.Information, null);
								Cursor.Current = Cursors.Default;
								this.progressBar1.Value = this.progressBar1.Maximum;
								XMessageBox.Show(string.Concat(new string[]
								{
									(sender as Button).Text,
									" \r\n\r\n",
									CommonStr.strUsersNum,
									" = ",
									this.dgvSelectedUsers.RowCount.ToString(),
									"\r\n\r\n",
									CommonStr.strDoorsNum,
									" = ",
									this.dgvSelectedDoors.RowCount.ToString(),
									"\r\n\r\n",
									CommonStr.strSuccessfully
								}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								this.progressBar1.Value = 0;
							}
						}
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
						wgAppConfig.wgLog(ex.ToString());
					}
					finally
					{
						if (this.cmd != null)
						{
							this.cmd.Dispose();
						}
					}
				}
				catch (Exception ex2)
				{
					wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
					wgAppConfig.wgLog(ex2.ToString());
				}
				finally
				{
					if (this.cn != null)
					{
						this.cn.Dispose();
					}
					this.dfrmWait1.Hide();
				}
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00049A08 File Offset: 0x00048A08
		private void btnAddPassAndUpload_Click_Acc(object sender, EventArgs e)
		{
			OleDbCommand oleDbCommand = null;
			OleDbConnection oleDbConnection = null;
			if (XMessageBox.Show(string.Concat(new string[]
			{
				(sender as Button).Text,
				" \r\n\r\n",
				CommonStr.strUsersNum,
				" = ",
				this.dgvSelectedUsers.RowCount.ToString(),
				"\r\n\r\n",
				CommonStr.strDoorsNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString(),
				"? "
			}), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK && this.dgvSelectedDoors.Rows.Count > 0 && this.dgvSelectedUsers.Rows.Count > 0)
			{
				this.bEdit = true;
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(sender as Button).Text,
					" ,",
					CommonStr.strUsersNum,
					" = ",
					this.dgvSelectedUsers.RowCount.ToString(),
					",",
					CommonStr.strDoorsNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString()
				}));
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnAddPass_Click Start");
				oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				try
				{
					oleDbConnection.Open();
					OleDbCommand oleDbCommand2;
					oleDbCommand = (oleDbCommand2 = new OleDbCommand("", oleDbConnection));
					try
					{
						int i = 0;
						i = 0;
						this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount * this.dgvSelectedUsers.RowCount;
						bool flag = true;
						flag = true;
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
						int num2 = 0;
						while (i < this.dgvSelectedDoors.Rows.Count)
						{
							string text = "";
							int num3 = 0;
							if (this.cbof_ZoneID.Items[0].ToString() == CommonStr.strAllZones && this.dtDoorTmpSelected.Rows.Count == this.dgvSelectedDoors.RowCount)
							{
								i = this.dgvSelectedDoors.Rows.Count;
								oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							}
							else
							{
								oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
								if (arrayList.Count > 0 && num < arrayList.Count)
								{
									text = text + " [f_ControllerID] = ( " + arrayList[num].ToString() + ")";
									this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", arrayList[num].ToString());
									num++;
									num3 = this.dvSelectedControllerID.Count;
									i += this.dvSelectedControllerID.Count;
								}
								else
								{
									if (this.dvDoorTmpSelected.Count <= num2)
									{
										break;
									}
									text = text + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num2]["f_DoorID"].ToString() + ")";
									num2++;
									num3 = 1;
									i++;
								}
							}
							int num4 = 2000;
							int j = 0;
							while (j < this.dgvSelectedUsers.Rows.Count)
							{
								string text2 = "";
								if (((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
								{
									while (j < this.dgvSelectedUsers.Rows.Count)
									{
										text2 = text2 + ((DataView)this.dgvSelectedUsers.DataSource)[j]["f_ConsumerID"] + ",";
										j++;
										if (text2.Length > num4)
										{
											break;
										}
									}
									text2 += "0";
								}
								else
								{
									j = this.dgvSelectedUsers.Rows.Count;
								}
								if (flag)
								{
									string text3 = "DELETE FROM  [t_d_Privilege]  WHERE  ";
									if (!string.IsNullOrEmpty(text))
									{
										text3 = text3 + "  ( " + text + ")";
									}
									else
									{
										text3 += "  1>0 ";
									}
									if (text2 != "")
									{
										text3 = text3 + " AND [f_ConsumerID] IN (" + text2 + " ) ";
									}
									oleDbCommand.CommandText = text3;
									wgTools.WriteLine(text3);
									oleDbCommand.ExecuteNonQuery();
									wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
								}
								this.progressBar1.Value = j * num3 + this.dgvSelectedUsers.Rows.Count * (i - num3);
								Application.DoEvents();
							}
						}
						flag = true;
						i = 0;
						num = 0;
						num2 = 0;
						while (i < this.dgvSelectedDoors.Rows.Count)
						{
							string text = "";
							int num5;
							if (arrayList.Count > 0 && num < arrayList.Count)
							{
								text = text + " [f_ControllerID] = ( " + arrayList[num].ToString() + ")";
								this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", arrayList[num].ToString());
								num++;
								num5 = this.dvSelectedControllerID.Count;
								i += this.dvSelectedControllerID.Count;
							}
							else
							{
								if (this.dvDoorTmpSelected.Count <= num2)
								{
									break;
								}
								text = text + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num2]["f_DoorID"].ToString() + ")";
								num2++;
								num5 = 1;
								i++;
							}
							int num6 = 2000;
							int k = 0;
							while (k < this.dgvSelectedUsers.Rows.Count)
							{
								string text4 = "";
								if (((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
								{
									while (k < this.dgvSelectedUsers.Rows.Count)
									{
										text4 = text4 + ((DataView)this.dgvSelectedUsers.DataSource)[k]["f_ConsumerID"] + ",";
										k++;
										if (text4.Length > num6)
										{
											break;
										}
									}
									text4 += "0";
								}
								else
								{
									k = this.dgvSelectedUsers.Rows.Count;
								}
								string text3 = "INSERT INTO [t_d_Privilege] (f_ConsumerID, f_DoorID ,f_ControllerID, f_DoorNO, f_ControlSegID)";
								object obj2 = text3;
								text3 = string.Concat(new object[]
								{
									obj2,
									" SELECT t_b_Consumer.f_ConsumerID, t_b_Door.f_DoorID, t_b_Door.f_ControllerID ,t_b_Door.f_DoorNO, ",
									this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex],
									" AS [f_ControlSegID]  "
								}) + " FROM t_b_Consumer, t_b_Door  WHERE  ((t_b_Consumer.f_DoorEnabled)=1) ";
								if (text4 != "")
								{
									text3 = text3 + " AND [f_ConsumerID] IN (" + text4 + " ) ";
								}
								text3 = text3 + " AND  ( " + text + ")";
								oleDbCommand.CommandText = text3;
								wgTools.WriteLine(text3);
								oleDbCommand.ExecuteNonQuery();
								wgTools.WriteLine("INSERT INTO [t_d_Privilege] End");
								this.progressBar1.Value = k * num5 + this.dgvSelectedUsers.Rows.Count * (i - num5);
								Application.DoEvents();
							}
						}
						string text5;
						if (sender.Equals(this.btnAddPass))
						{
							text5 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
						}
						else
						{
							text5 = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
						}
						for (int l = 0; l < this.dgvSelectedDoors.Rows.Count; l++)
						{
							string text3 = string.Format(text5, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, (int)((DataView)this.dgvSelectedDoors.DataSource)[l]["f_ControllerID"]);
							oleDbCommand.CommandText = text3;
							oleDbCommand.ExecuteNonQuery();
						}
						wgTools.WriteLine("btnAddPass_Click End");
						Cursor.Current = Cursors.Default;
						this.progressBar1.Value = this.progressBar1.Maximum;
						if (sender.Equals(this.btnAddPass))
						{
							this.dfrmWait1.Hide();
							this.logOperate(this.btnAddPass);
							XMessageBox.Show(string.Concat(new string[]
							{
								(sender as Button).Text,
								" \r\n\r\n",
								CommonStr.strUsersNum,
								" = ",
								this.dgvSelectedUsers.RowCount.ToString(),
								"\r\n\r\n",
								CommonStr.strDoorsNum,
								" = ",
								this.dgvSelectedDoors.RowCount.ToString(),
								"\r\n\r\n",
								CommonStr.strSuccessfully
							}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							this.progressBar1.Value = 0;
						}
						else if (this.lblOver1000.Visible)
						{
							this.dfrmWait1.Hide();
							this.logOperate(this.btnAddPass);
							if (XMessageBox.Show(CommonStr.strNeedUploadPrivilegeOver50User, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
							{
								this.btnConsoleUpload_Click(null, null);
							}
							this.progressBar1.Value = 0;
						}
						else
						{
							this.logOperate(this.btnAddPassAndUpload);
							this.waitCloudServerConnect();
							this.progressBar1.Value = 0;
							int num7;
							if (!wgTools.gWGYTJ && (num7 = this.multithreadUpdate(true)) >= 0)
							{
								if (num7 == 1)
								{
									this.dfrmWait1.Hide();
									wgAppConfig.wgLog(string.Concat(new string[]
									{
										(sender as Button).Text.Replace("\r\n", ""),
										" ,",
										CommonStr.strUsersNum,
										" = ",
										this.dgvSelectedUsers.RowCount.ToString(),
										",",
										CommonStr.strDoorsNum,
										" = ",
										this.dgvSelectedDoors.RowCount.ToString(),
										",",
										CommonStr.strSuccessfully
									}), EventLogEntryType.Information, null);
									Cursor.Current = Cursors.Default;
									this.progressBar1.Value = this.progressBar1.Maximum;
									XMessageBox.Show(string.Concat(new string[]
									{
										(sender as Button).Text,
										" \r\n\r\n",
										CommonStr.strUsersNum,
										" = ",
										this.dgvSelectedUsers.RowCount.ToString(),
										"\r\n\r\n",
										CommonStr.strDoorsNum,
										" = ",
										this.dgvSelectedDoors.RowCount.ToString(),
										"\r\n\r\n",
										CommonStr.strSuccessfully
									}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
									this.progressBar1.Value = 0;
								}
							}
							else
							{
								ArrayList arrayList3 = new ArrayList();
								this.progressBar1.Maximum = this.dgvSelectedDoors.Rows.Count;
								if (this.dgvSelectedUsers.Rows.Count > 0)
								{
									using (icPrivilege icPrivilege = new icPrivilege())
									{
										using (icController icController = new icController())
										{
											for (int m = 0; m < this.dgvSelectedDoors.Rows.Count; m++)
											{
												int num8 = (int)((DataView)this.dgvSelectedDoors.DataSource)[m]["f_ControllerID"];
												string text6 = (string)((DataView)this.dgvSelectedDoors.DataSource)[m]["f_DoorName"];
												if (arrayList3.IndexOf(num8) < 0)
												{
													icController.GetInfoFromDBByControllerID(num8);
													if (icController.GetControllerRunInformationIP(-1) <= 0)
													{
														this.dfrmWait1.Hide();
														wgAppConfig.wgLog(string.Concat(new string[]
														{
															this.btnAddPassAndUpload.Text.Replace("\r\n", ""),
															" ,",
															CommonStr.strUsersNum,
															" = ",
															this.dgvSelectedUsers.RowCount.ToString(),
															",",
															CommonStr.strDoorsNum,
															" = ",
															this.dgvSelectedDoors.RowCount.ToString(),
															",",
															CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
														}), EventLogEntryType.Information, null);
														XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
														this.progressBar1.Value = 0;
														return;
													}
													if (icController.runinfo.registerCardNum == 0U && icPrivilege.ClearAllPrivilegeIP(icController.ControllerSN, icController.IP, icController.PORT) < 0)
													{
														this.dfrmWait1.Hide();
														wgAppConfig.wgLog(string.Concat(new string[]
														{
															this.btnAddPassAndUpload.Text.Replace("\r\n", ""),
															" ,",
															CommonStr.strUsersNum,
															" = ",
															this.dgvSelectedUsers.RowCount.ToString(),
															",",
															CommonStr.strDoorsNum,
															" = ",
															this.dgvSelectedDoors.RowCount.ToString(),
															",",
															CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
														}), EventLogEntryType.Information, null);
														XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
														this.progressBar1.Value = 0;
														return;
													}
													if (wgAppConfig.IsAccessControlBlue)
													{
														icController icController2 = icController;
														if (!wgAppConfig.checkRSAController((long)icController2.ControllerSN) && wgTools.doubleParse(icController2.runinfo.driverVersion.Substring(1)) == 6.62)
														{
															bool flag2 = true;
															string systemParamByNO = wgAppConfig.getSystemParamByNO(49);
															DateTime now = DateTime.Now;
															if (!string.IsNullOrEmpty(systemParamByNO))
															{
																DateTime.TryParse(systemParamByNO, out now);
																if (now < DateTime.Parse("2018-01-01 00:00:00"))
																{
																	flag2 = false;
																}
															}
															if (((icController2.runinfo.swipeEndIndex < 2000U && icController2.runinfo.swipeStartIndex < 1000U) || flag2) && wgAppConfig.getParamValBoolByNO(64))
															{
																string text7 = CommonStr.strDelAddAndUploadFail4False + "\r\n\r\n" + string.Format("!!!6.62--{0}: {1}", icController2.ControllerSN.ToString(), CommonStr.strSupposeUpgradeDriver);
																wgAppConfig.wgLog(text7);
																XMessageBox.Show(text7, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
																this.progressBar1.Value = 0;
																return;
															}
														}
													}
													for (int n = 0; n < this.dgvSelectedUsers.Rows.Count; n++)
													{
														int num9;
														if (wgTools.doubleParse(icController.runinfo.driverVersion.Substring(1)) >= 5.52 && (wgTools.gWGYTJ || icPrivilege.bAllowUploadUserName))
														{
															num9 = icPrivilege.AddPrivilegeWithUsernameOfOneCardByDB(num8, (int)((DataView)this.dgvSelectedUsers.DataSource)[n]["f_ConsumerID"]);
														}
														else
														{
															num9 = icPrivilege.AddPrivilegeOfOneCardByDB(num8, (int)((DataView)this.dgvSelectedUsers.DataSource)[n]["f_ConsumerID"]);
														}
														if (num9 < 0)
														{
															text5 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
															if (oleDbConnection.State != ConnectionState.Open)
															{
																oleDbConnection.Open();
															}
															for (int num10 = 0; num10 < this.dgvSelectedDoors.Rows.Count; num10++)
															{
																num8 = (int)((DataView)this.dgvSelectedDoors.DataSource)[num10]["f_ControllerID"];
																if (arrayList3.IndexOf(num8) < 0)
																{
																	string text3 = string.Format(text5, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, num8);
																	oleDbCommand.CommandText = text3;
																	oleDbCommand.ExecuteNonQuery();
																}
															}
															this.dfrmWait1.Hide();
															wgAppConfig.wgLog(string.Concat(new string[]
															{
																this.btnAddPassAndUpload.Text.Replace("\r\n", ""),
																" ,",
																CommonStr.strUsersNum,
																" = ",
																this.dgvSelectedUsers.RowCount.ToString(),
																",",
																CommonStr.strDoorsNum,
																" = ",
																this.dgvSelectedDoors.RowCount.ToString(),
																",",
																CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
															}), EventLogEntryType.Information, null);
															XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
															this.progressBar1.Value = 0;
															return;
														}
													}
													arrayList3.Add(num8);
												}
												this.progressBar1.Value = m + 1;
											}
										}
									}
								}
								this.dfrmWait1.Hide();
								wgAppConfig.wgLog(string.Concat(new string[]
								{
									(sender as Button).Text.Replace("\r\n", ""),
									" ,",
									CommonStr.strUsersNum,
									" = ",
									this.dgvSelectedUsers.RowCount.ToString(),
									",",
									CommonStr.strDoorsNum,
									" = ",
									this.dgvSelectedDoors.RowCount.ToString(),
									",",
									CommonStr.strSuccessfully
								}), EventLogEntryType.Information, null);
								Cursor.Current = Cursors.Default;
								this.progressBar1.Value = this.progressBar1.Maximum;
								XMessageBox.Show(string.Concat(new string[]
								{
									(sender as Button).Text,
									" \r\n\r\n",
									CommonStr.strUsersNum,
									" = ",
									this.dgvSelectedUsers.RowCount.ToString(),
									"\r\n\r\n",
									CommonStr.strDoorsNum,
									" = ",
									this.dgvSelectedDoors.RowCount.ToString(),
									"\r\n\r\n",
									CommonStr.strSuccessfully
								}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								this.progressBar1.Value = 0;
							}
						}
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
						wgAppConfig.wgLog(ex.ToString());
					}
					finally
					{
						if (oleDbCommand2 != null)
						{
							((IDisposable)oleDbCommand2).Dispose();
						}
					}
				}
				catch (Exception ex2)
				{
					wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
					wgAppConfig.wgLog(ex2.ToString());
				}
				finally
				{
					if (oleDbConnection != null)
					{
						oleDbConnection.Dispose();
					}
					this.dfrmWait1.Hide();
				}
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0004B028 File Offset: 0x0004A028
		private void btnConsoleUpload_Click(object sender, EventArgs e)
		{
			this.bLoadConsole = true;
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0004B040 File Offset: 0x0004A040
		private void btnDelAllDoors_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 0;
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0004B0A4 File Offset: 0x0004A0A4
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnDelAllUsers_Click Start");
				icConsumerShare.selectNoneUsers();
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
					((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
					return;
				}
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0004B184 File Offset: 0x0004A184
		private void btnDeletePass_Click(object sender, EventArgs e)
		{
			this.btnDeletePassAndUpload_Click(sender, e);
			this.bEdit = true;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0004B198 File Offset: 0x0004A198
		private void btnDeletePassAndUpload_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnDeletePassAndUpload_Click_Acc(sender, e);
				return;
			}
			if (XMessageBox.Show(string.Concat(new string[]
			{
				(sender as Button).Text,
				" \r\n\r\n",
				CommonStr.strUsersNum,
				" = ",
				this.dgvSelectedUsers.RowCount.ToString(),
				"\r\n\r\n",
				CommonStr.strDoorsNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString(),
				"? "
			}), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK && this.dgvSelectedDoors.Rows.Count > 0 && this.dgvSelectedUsers.Rows.Count > 0)
			{
				this.bEdit = true;
				Cursor.Current = Cursors.WaitCursor;
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(sender as Button).Text,
					" ,",
					CommonStr.strUsersNum,
					" = ",
					this.dgvSelectedUsers.RowCount.ToString(),
					",",
					CommonStr.strDoorsNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString()
				}));
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				wgTools.WriteLine("btnDelete_Click Start");
				this.cn = new SqlConnection(wgAppConfig.dbConString);
				this.cmd = new SqlCommand("", this.cn);
				try
				{
					this.cn.Open();
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
					int i = 0;
					this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount * this.dgvSelectedUsers.RowCount;
					int num = 0;
					int num2 = 0;
					this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
					while (i < this.dgvSelectedDoors.Rows.Count)
					{
						string text = "";
						int num3 = 0;
						if (this.cbof_ZoneID.Items[0].ToString() == CommonStr.strAllZones && this.dtDoorTmpSelected.Rows.Count == this.dgvSelectedDoors.RowCount)
						{
							i = this.dgvSelectedDoors.Rows.Count;
							this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
						}
						else if (arrayList.Count > 0 && num < arrayList.Count)
						{
							text = text + " [f_ControllerID] = ( " + arrayList[num].ToString() + ")";
							this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", arrayList[num].ToString());
							num++;
							num3 = this.dvSelectedControllerID.Count;
							i += this.dvSelectedControllerID.Count;
						}
						else
						{
							if (this.dvDoorTmpSelected.Count <= num2)
							{
								break;
							}
							text = text + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num2]["f_DoorID"].ToString() + ")";
							num2++;
							num3 = 1;
							i++;
						}
						int num4 = 2000;
						int j = 0;
						while (j < this.dgvSelectedUsers.Rows.Count)
						{
							string text2 = "";
							if (((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count <= this.dgvSelectedUsers.Rows.Count)
							{
								if (!(this.cbof_GroupID.Items[0].ToString() != CommonStr.strAll))
								{
									j = this.dgvSelectedUsers.Rows.Count;
									goto IL_05B0;
								}
							}
							while (j < this.dgvSelectedUsers.Rows.Count)
							{
								text2 = text2 + ((DataView)this.dgvSelectedUsers.DataSource)[j]["f_ConsumerID"] + ",";
								if (text2.Length > num4)
								{
									break;
								}
								j++;
							}
							text2 += "0";
							IL_05B0:
							string text3 = "DELETE FROM  [t_d_Privilege]  WHERE 1>0  ";
							if (!string.IsNullOrEmpty(text))
							{
								text3 = text3 + " AND  ( " + text + ")";
							}
							if (text2 != "")
							{
								text3 = text3 + " AND [f_ConsumerID] IN (" + text2 + " ) ";
							}
							if (!wgAppConfig.IsAccessDB && string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2))
							{
								text3 = "TRUNCATE TABLE [t_d_Privilege] ";
							}
							this.cmd.CommandText = text3;
							wgTools.WriteLine(text3);
							this.cmd.ExecuteNonQuery();
							wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
							this.progressBar1.Value = j * num3 + this.dgvSelectedUsers.Rows.Count * (i - num3);
							Application.DoEvents();
						}
					}
					string text4;
					if (sender.Equals(this.btnDeletePass))
					{
						text4 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
					}
					else
					{
						text4 = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
					}
					for (int k = 0; k < this.dgvSelectedDoors.Rows.Count; k++)
					{
						string text3 = string.Format(text4, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, (int)((DataView)this.dgvSelectedDoors.DataSource)[k]["f_ControllerID"]);
						this.cmd.CommandText = text3;
						this.cmd.ExecuteNonQuery();
					}
					wgTools.WriteLine("btnDelete_Click End");
					this.progressBar1.Value = this.progressBar1.Maximum;
					Cursor.Current = Cursors.Default;
					if (sender.Equals(this.btnDeletePass))
					{
						this.dfrmWait1.Hide();
						this.logOperate(this.btnDeletePass);
						XMessageBox.Show(string.Concat(new string[]
						{
							(sender as Button).Text,
							" \r\n\r\n",
							CommonStr.strUsersNum,
							" = ",
							this.dgvSelectedUsers.RowCount.ToString(),
							"\r\n\r\n",
							CommonStr.strDoorsNum,
							" = ",
							this.dgvSelectedDoors.RowCount.ToString(),
							"\r\n\r\n",
							CommonStr.strSuccessfully
						}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						this.progressBar1.Value = 0;
					}
					else if (this.lblOver1000.Visible)
					{
						this.dfrmWait1.Hide();
						this.logOperate(this.btnDeletePass);
						if (XMessageBox.Show(CommonStr.strNeedUploadPrivilegeOver50User, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
						{
							this.btnConsoleUpload_Click(null, null);
						}
						this.progressBar1.Value = 0;
					}
					else
					{
						this.logOperate(this.btnDeletePassAndUpload);
						this.waitCloudServerConnect();
						int num5 = this.multithreadUpdate(false);
						if (num5 >= 0)
						{
							if (num5 == 1)
							{
								this.dfrmWait1.Hide();
								wgAppConfig.wgLog(string.Concat(new string[]
								{
									(sender as Button).Text.Replace("\r\n", ""),
									" ,",
									CommonStr.strUsersNum,
									" = ",
									this.dgvSelectedUsers.RowCount.ToString(),
									",",
									CommonStr.strDoorsNum,
									" = ",
									this.dgvSelectedDoors.RowCount.ToString(),
									",",
									CommonStr.strSuccessfully
								}), EventLogEntryType.Information, null);
								Cursor.Current = Cursors.Default;
								this.progressBar1.Value = this.progressBar1.Maximum;
								XMessageBox.Show(string.Concat(new string[]
								{
									(sender as Button).Text,
									" \r\n\r\n",
									CommonStr.strUsersNum,
									" = ",
									this.dgvSelectedUsers.RowCount.ToString(),
									"\r\n\r\n",
									CommonStr.strDoorsNum,
									" = ",
									this.dgvSelectedDoors.RowCount.ToString(),
									"\r\n\r\n",
									CommonStr.strSuccessfully
								}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								this.progressBar1.Value = 0;
							}
						}
						else
						{
							ArrayList arrayList3 = new ArrayList();
							if (this.dgvSelectedUsers.Rows.Count > 0)
							{
								using (icPrivilege icPrivilege = new icPrivilege())
								{
									for (int l = 0; l < this.dgvSelectedDoors.Rows.Count; l++)
									{
										int num6 = (int)((DataView)this.dgvSelectedDoors.DataSource)[l]["f_ControllerID"];
										if (arrayList3.IndexOf(num6) < 0)
										{
											for (int m = 0; m < this.dgvSelectedUsers.Rows.Count; m++)
											{
												if (icPrivilege.DelPrivilegeOfOneCardByDB(num6, (int)((DataView)this.dgvSelectedUsers.DataSource)[m]["f_ConsumerID"]) < 0)
												{
													text4 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
													for (int n = 0; n < this.dgvSelectedDoors.Rows.Count; n++)
													{
														num6 = (int)((DataView)this.dgvSelectedDoors.DataSource)[n]["f_ControllerID"];
														if (arrayList3.IndexOf(num6) < 0)
														{
															string text3 = string.Format(text4, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, num6);
															this.cmd.CommandText = text3;
															this.cmd.ExecuteNonQuery();
														}
													}
													this.dfrmWait1.Hide();
													wgAppConfig.wgLog(string.Concat(new string[]
													{
														this.btnDeletePassAndUpload.Text.Replace("\r\n", ""),
														" ,",
														CommonStr.strUsersNum,
														" = ",
														this.dgvSelectedUsers.RowCount.ToString(),
														",",
														CommonStr.strDoorsNum,
														" = ",
														this.dgvSelectedDoors.RowCount.ToString(),
														",",
														CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
													}), EventLogEntryType.Information, null);
													XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
													this.progressBar1.Value = 0;
													return;
												}
											}
											arrayList3.Add(num6);
										}
									}
								}
							}
							this.dfrmWait1.Hide();
							wgAppConfig.wgLog(string.Concat(new string[]
							{
								(sender as Button).Text.Replace("\r\n", ""),
								" ,",
								CommonStr.strUsersNum,
								" = ",
								this.dgvSelectedUsers.RowCount.ToString(),
								",",
								CommonStr.strDoorsNum,
								" = ",
								this.dgvSelectedDoors.RowCount.ToString(),
								",",
								CommonStr.strSuccessfully
							}), EventLogEntryType.Information, null);
							this.progressBar1.Value = this.progressBar1.Maximum;
							Cursor.Current = Cursors.Default;
							XMessageBox.Show((sender as Button).Text + " " + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							this.progressBar1.Value = 0;
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					wgAppConfig.wgLog(ex.ToString());
				}
				finally
				{
					if (this.cmd != null)
					{
						this.cmd.Dispose();
					}
					if (this.cn != null)
					{
						this.cn.Dispose();
					}
					this.dfrmWait1.Hide();
				}
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0004C074 File Offset: 0x0004B074
		private void btnDeletePassAndUpload_Click_Acc(object sender, EventArgs e)
		{
			OleDbCommand oleDbCommand = null;
			OleDbConnection oleDbConnection = null;
			if (XMessageBox.Show(string.Concat(new string[]
			{
				(sender as Button).Text,
				" \r\n\r\n",
				CommonStr.strUsersNum,
				" = ",
				this.dgvSelectedUsers.RowCount.ToString(),
				"\r\n\r\n",
				CommonStr.strDoorsNum,
				" = ",
				this.dgvSelectedDoors.RowCount.ToString(),
				"? "
			}), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK && this.dgvSelectedDoors.Rows.Count > 0 && this.dgvSelectedUsers.Rows.Count > 0)
			{
				this.bEdit = true;
				Cursor.Current = Cursors.WaitCursor;
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					(sender as Button).Text,
					" ,",
					CommonStr.strUsersNum,
					" = ",
					this.dgvSelectedUsers.RowCount.ToString(),
					",",
					CommonStr.strDoorsNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString()
				}));
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				wgTools.WriteLine("btnDelete_Click Start");
				oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				oleDbCommand = new OleDbCommand("", oleDbConnection);
				try
				{
					oleDbConnection.Open();
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
					int i = 0;
					this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount * this.dgvSelectedUsers.RowCount;
					int num = 0;
					int num2 = 0;
					oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
					while (i < this.dgvSelectedDoors.Rows.Count)
					{
						string text = "";
						int num3 = 0;
						if (this.cbof_ZoneID.Items[0].ToString() == CommonStr.strAllZones && this.dtDoorTmpSelected.Rows.Count == this.dgvSelectedDoors.RowCount)
						{
							i = this.dgvSelectedDoors.Rows.Count;
							oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						}
						else if (arrayList.Count > 0 && num < arrayList.Count)
						{
							text = text + " [f_ControllerID] = ( " + arrayList[num].ToString() + ")";
							this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", arrayList[num].ToString());
							num++;
							num3 = this.dvSelectedControllerID.Count;
							i += this.dvSelectedControllerID.Count;
						}
						else
						{
							if (this.dvDoorTmpSelected.Count <= num2)
							{
								break;
							}
							text = text + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num2]["f_DoorID"].ToString() + ")";
							num2++;
							num3 = 1;
							i++;
						}
						int num4 = 2000;
						int j = 0;
						while (j < this.dgvSelectedUsers.Rows.Count)
						{
							string text2 = "";
							if (((DataView)this.dgvSelectedUsers.DataSource).Table.Rows.Count <= this.dgvSelectedUsers.Rows.Count)
							{
								if (!(this.cbof_GroupID.Items[0].ToString() != CommonStr.strAll))
								{
									j = this.dgvSelectedUsers.Rows.Count;
									goto IL_058F;
								}
							}
							while (j < this.dgvSelectedUsers.Rows.Count)
							{
								text2 = text2 + ((DataView)this.dgvSelectedUsers.DataSource)[j]["f_ConsumerID"] + ",";
								if (text2.Length > num4)
								{
									break;
								}
								j++;
							}
							text2 += "0";
							IL_058F:
							string text3 = "DELETE FROM  [t_d_Privilege]  WHERE  1>0 ";
							if (!string.IsNullOrEmpty(text))
							{
								text3 = text3 + " AND  ( " + text + ")";
							}
							if (text2 != "")
							{
								text3 = text3 + " AND [f_ConsumerID] IN (" + text2 + " ) ";
							}
							oleDbCommand.CommandText = text3;
							wgTools.WriteLine(text3);
							oleDbCommand.ExecuteNonQuery();
							wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
							this.progressBar1.Value = j * num3 + this.dgvSelectedUsers.Rows.Count * (i - num3);
							Application.DoEvents();
						}
					}
					string text4;
					if (sender.Equals(this.btnDeletePass))
					{
						text4 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
					}
					else
					{
						text4 = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
					}
					for (int k = 0; k < this.dgvSelectedDoors.Rows.Count; k++)
					{
						string text3 = string.Format(text4, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, (int)((DataView)this.dgvSelectedDoors.DataSource)[k]["f_ControllerID"]);
						oleDbCommand.CommandText = text3;
						oleDbCommand.ExecuteNonQuery();
					}
					wgTools.WriteLine("btnDelete_Click End");
					this.progressBar1.Value = this.progressBar1.Maximum;
					Cursor.Current = Cursors.Default;
					if (sender.Equals(this.btnDeletePass))
					{
						this.dfrmWait1.Hide();
						this.logOperate(this.btnDeletePass);
						XMessageBox.Show(string.Concat(new string[]
						{
							(sender as Button).Text,
							" \r\n\r\n",
							CommonStr.strUsersNum,
							" = ",
							this.dgvSelectedUsers.RowCount.ToString(),
							"\r\n\r\n",
							CommonStr.strDoorsNum,
							" = ",
							this.dgvSelectedDoors.RowCount.ToString(),
							"\r\n\r\n",
							CommonStr.strSuccessfully
						}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						this.progressBar1.Value = 0;
					}
					else if (this.lblOver1000.Visible)
					{
						this.dfrmWait1.Hide();
						this.logOperate(this.btnDeletePass);
						if (XMessageBox.Show(CommonStr.strNeedUploadPrivilegeOver50User, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
						{
							this.btnConsoleUpload_Click(null, null);
						}
						this.progressBar1.Value = 0;
					}
					else
					{
						this.logOperate(this.btnDeletePassAndUpload);
						int num5 = this.multithreadUpdate(false);
						if (num5 >= 0)
						{
							if (num5 == 1)
							{
								this.dfrmWait1.Hide();
								wgAppConfig.wgLog(string.Concat(new string[]
								{
									(sender as Button).Text.Replace("\r\n", ""),
									" ,",
									CommonStr.strUsersNum,
									" = ",
									this.dgvSelectedUsers.RowCount.ToString(),
									",",
									CommonStr.strDoorsNum,
									" = ",
									this.dgvSelectedDoors.RowCount.ToString(),
									",",
									CommonStr.strSuccessfully
								}), EventLogEntryType.Information, null);
								Cursor.Current = Cursors.Default;
								this.progressBar1.Value = this.progressBar1.Maximum;
								XMessageBox.Show(string.Concat(new string[]
								{
									(sender as Button).Text,
									" \r\n\r\n",
									CommonStr.strUsersNum,
									" = ",
									this.dgvSelectedUsers.RowCount.ToString(),
									"\r\n\r\n",
									CommonStr.strDoorsNum,
									" = ",
									this.dgvSelectedDoors.RowCount.ToString(),
									"\r\n\r\n",
									CommonStr.strSuccessfully
								}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								this.progressBar1.Value = 0;
							}
						}
						else
						{
							ArrayList arrayList3 = new ArrayList();
							if (this.dgvSelectedUsers.Rows.Count > 0)
							{
								using (icPrivilege icPrivilege = new icPrivilege())
								{
									for (int l = 0; l < this.dgvSelectedDoors.Rows.Count; l++)
									{
										int num6 = (int)((DataView)this.dgvSelectedDoors.DataSource)[l]["f_ControllerID"];
										if (arrayList3.IndexOf(num6) < 0)
										{
											for (int m = 0; m < this.dgvSelectedUsers.Rows.Count; m++)
											{
												if (icPrivilege.DelPrivilegeOfOneCardByDB(num6, (int)((DataView)this.dgvSelectedUsers.DataSource)[m]["f_ConsumerID"]) < 0)
												{
													text4 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
													for (int n = 0; n < this.dgvSelectedDoors.Rows.Count; n++)
													{
														num6 = (int)((DataView)this.dgvSelectedDoors.DataSource)[n]["f_ControllerID"];
														if (arrayList3.IndexOf(num6) < 0)
														{
															string text3 = string.Format(text4, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, num6);
															oleDbCommand.CommandText = text3;
															oleDbCommand.ExecuteNonQuery();
														}
													}
													this.dfrmWait1.Hide();
													wgAppConfig.wgLog(string.Concat(new string[]
													{
														this.btnDeletePassAndUpload.Text.Replace("\r\n", ""),
														" ,",
														CommonStr.strUsersNum,
														" = ",
														this.dgvSelectedUsers.RowCount.ToString(),
														",",
														CommonStr.strDoorsNum,
														" = ",
														this.dgvSelectedDoors.RowCount.ToString(),
														",",
														CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
													}), EventLogEntryType.Information, null);
													XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
													this.progressBar1.Value = 0;
													return;
												}
											}
											arrayList3.Add(num6);
										}
									}
								}
							}
							this.dfrmWait1.Hide();
							wgAppConfig.wgLog(string.Concat(new string[]
							{
								(sender as Button).Text.Replace("\r\n", ""),
								" ,",
								CommonStr.strUsersNum,
								" = ",
								this.dgvSelectedUsers.RowCount.ToString(),
								",",
								CommonStr.strDoorsNum,
								" = ",
								this.dgvSelectedDoors.RowCount.ToString(),
								",",
								CommonStr.strSuccessfully
							}), EventLogEntryType.Information, null);
							this.progressBar1.Value = this.progressBar1.Maximum;
							Cursor.Current = Cursors.Default;
							XMessageBox.Show((sender as Button).Text + " " + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							this.progressBar1.Value = 0;
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					wgAppConfig.wgLog(ex.ToString());
				}
				finally
				{
					if (oleDbCommand != null)
					{
						oleDbCommand.Dispose();
					}
					if (oleDbConnection != null)
					{
						oleDbConnection.Dispose();
					}
					this.dfrmWait1.Hide();
				}
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0004CED8 File Offset: 0x0004BED8
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0004CEE5 File Offset: 0x0004BEE5
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0004CEF7 File Offset: 0x0004BEF7
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

		// Token: 0x06000276 RID: 630 RVA: 0x0004CF17 File Offset: 0x0004BF17
		private void btnFind_Click(object sender, EventArgs e)
		{
			if (this.defaultFindControl == null)
			{
				this.defaultFindControl = this.dgvUsers;
				base.ActiveControl = this.dgvUsers;
			}
			wgAppConfig.findCall(this.dfrmFind1, this, this.defaultFindControl);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0004CF4B File Offset: 0x0004BF4B
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0004CF58 File Offset: 0x0004BF58
		private void cbof_GroupID_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0004CF88 File Offset: 0x0004BF88
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.dgvUsers.DataSource != null)
			{
				this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
				DataView dataView = (DataView)this.dgvUsers.DataSource;
				if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					dataView.RowFilter = icConsumerShare.getOptionalRowfilter();
					this.strGroupFilter = "";
				}
				else
				{
					dataView.RowFilter = "f_Selected = 0 AND f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					int num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
					int groupChildMaxNo = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
					if (num2 > 0)
					{
						if (num2 >= groupChildMaxNo)
						{
							dataView.RowFilter = string.Format("f_Selected = 0 AND f_GroupID ={0:d} ", num);
							this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num);
						}
						else
						{
							dataView.RowFilter = "f_Selected = 0 ";
							string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
							dataView.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", groupQuery);
							this.strGroupFilter = string.Format("  {0} ", groupQuery);
						}
					}
					dataView.RowFilter = string.Format("(f_DoorEnabled > 0) AND {0} AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				}
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				}
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0004D1D8 File Offset: 0x0004C1D8
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

		// Token: 0x0600027B RID: 635 RVA: 0x0004D39E File Offset: 0x0004C39E
		private void cbof_ZoneID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ZoneID);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0004D3AC File Offset: 0x0004C3AC
		private void cloudServerStart()
		{
			try
			{
				if (wgTools.bUDPCloud != 0)
				{
					if (this.watching == null)
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
					this.watching.EventHandler += this.evtNewInfoCallBack;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0004D420 File Offset: 0x0004C420
		private void dfrmPrivilege_FormClosed(object sender, FormClosedEventArgs e)
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

		// Token: 0x0600027E RID: 638 RVA: 0x0004D458 File Offset: 0x0004C458
		private void dfrmPrivilege_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
			if (this.watching != null)
			{
				this.watching.EventHandler -= this.evtNewInfoCallBack;
				this.watching.StopWatch();
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0004D498 File Offset: 0x0004C498
		private void dfrmPrivilege_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.defaultFindControl == null)
					{
						this.defaultFindControl = this.dgvUsers;
						base.ActiveControl = this.dgvUsers;
					}
					wgAppConfig.findCall(this.dfrmFind1, this, this.defaultFindControl);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0004D514 File Offset: 0x0004C514
		private void dfrmPrivilege_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.cloudServerStart();
			try
			{
				this.label1.Visible = wgAppConfig.getParamValBoolByNO(121);
				this.cbof_ControlSegID.Visible = wgAppConfig.getParamValBoolByNO(121);
				this.loadControlSegData();
				new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
				for (int i = 0; i < this.arrGroupID.Count; i++)
				{
					if (i == 0 && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
					{
						this.cbof_GroupID.Items.Add(CommonStr.strAll);
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
				this.loadZoneInfo();
				this.loadDoorData();
				this.cbof_Zone_SelectedIndexChanged(null, null);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			if (!this.backgroundWorker1.IsBusy)
			{
				this.backgroundWorker1.RunWorkerAsync();
			}
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("C");
			this.mnuDoorTypeSelect.Items.Add(toolStripMenuItem);
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvUsers.Select();
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0004D750 File Offset: 0x0004C750
		private void dgvDoors_KeyDown(object sender, KeyEventArgs e)
		{
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0004D752 File Offset: 0x0004C752
		private void dgvDoors_MouseClick(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0004D754 File Offset: 0x0004C754
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0004D761 File Offset: 0x0004C761
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0004D76E File Offset: 0x0004C76E
		private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0004D77B File Offset: 0x0004C77B
		private void dgvUsers_KeyDown(object sender, KeyEventArgs e)
		{
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0004D77D File Offset: 0x0004C77D
		private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneUser.PerformClick();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0004D78C File Offset: 0x0004C78C
		private void evtNewInfoCallBack(string text)
		{
			wgTools.WgDebugWrite("Got text through callback! {0}", new object[] { text });
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0004D7AF File Offset: 0x0004C7AF
		private void groupBox2_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0004D7B4 File Offset: 0x0004C7B4
		private void loadControlSegData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadControlSegData_Acc();
				return;
			}
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
			this.controlSegIDList[0] = 1;
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			try
			{
				string text = " SELECT ";
				using (SqlCommand sqlCommand = new SqlCommand(text + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,    CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),       ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50),      ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']')     END AS f_ControlSegID    FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ", this.cn))
				{
					this.cn.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					int num = 1;
					while (sqlDataReader.Read())
					{
						this.cbof_ControlSegID.Items.Add(sqlDataReader["f_ControlSegID"]);
						this.controlSegIDList[num] = (int)sqlDataReader["f_ControlSegIDBak"];
						num++;
					}
					sqlDataReader.Close();
					if (this.cbof_ControlSegID.Items.Count > 0)
					{
						this.cbof_ControlSegID.SelectedIndex = 0;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				if (this.cn != null)
				{
					this.cn.Dispose();
				}
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0004D908 File Offset: 0x0004C908
		private void loadControlSegData_Acc()
		{
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
			this.controlSegIDList[0] = 1;
			OleDbConnection oleDbConnection = null;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				try
				{
					string text = " SELECT ";
					using (OleDbCommand oleDbCommand = new OleDbCommand(text + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,   IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID   FROM [t_b_ControlSeg]  WHERE  [t_b_ControlSeg].[f_ControlSegID]>1  ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ", oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						int num = 1;
						while (oleDbDataReader.Read())
						{
							this.cbof_ControlSegID.Items.Add(oleDbDataReader["f_ControlSegID"]);
							this.controlSegIDList[num] = (int)oleDbDataReader["f_ControlSegIDBak"];
							num++;
						}
						oleDbDataReader.Close();
						if (this.cbof_ControlSegID.Items.Count > 0)
						{
							this.cbof_ControlSegID.SelectedIndex = 0;
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0004DA3C File Offset: 0x0004CA3C
		private void loadDoorData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadDoorData_Acc();
				return;
			}
			string text = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
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

		// Token: 0x0600028D RID: 653 RVA: 0x0004DC58 File Offset: 0x0004CC58
		private void loadDoorData_Acc()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
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

		// Token: 0x0600028E RID: 654 RVA: 0x0004DE64 File Offset: 0x0004CE64
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

		// Token: 0x0600028F RID: 655 RVA: 0x0004DF33 File Offset: 0x0004CF33
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0004DF54 File Offset: 0x0004CF54
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			try
			{
				this.dv.RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
				this.dvSelected.RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
				this.dgvUsers.AutoGenerateColumns = false;
				this.dgvUsers.DataSource = this.dv;
				this.dgvSelectedUsers.AutoGenerateColumns = false;
				this.dgvSelectedUsers.DataSource = this.dvSelected;
				int num = 0;
				while (num < this.dv.Table.Columns.Count && num < this.dgvUsers.ColumnCount)
				{
					this.dgvUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
					this.dgvSelectedUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
					num++;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				wgAppConfig.wgLog(ex.ToString());
			}
			wgTools.WriteLine("loadUserData End");
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			this.mnuDoorTypeSelect_loadData();
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0004E0B8 File Offset: 0x0004D0B8
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

		// Token: 0x06000292 RID: 658 RVA: 0x0004E1A4 File Offset: 0x0004D1A4
		private void logOperate(object sender)
		{
			string text = "";
			for (int i = 0; i <= Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedUsers.RowCount) - 1; i++)
			{
				text = text + ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerName"] + ",";
			}
			if (this.dgvSelectedUsers.RowCount > wgAppConfig.LogEventMaxCount)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"......(",
					this.dgvSelectedUsers.RowCount,
					")"
				});
			}
			else
			{
				object obj2 = text;
				text = string.Concat(new object[]
				{
					obj2,
					"(",
					this.dgvSelectedUsers.RowCount,
					")"
				});
			}
			string text2 = "";
			for (int i = 0; i <= Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedDoors.RowCount) - 1; i++)
			{
				text2 = text2 + ((DataView)this.dgvSelectedDoors.DataSource)[i]["f_DoorName"] + ",";
			}
			if (this.dgvSelectedDoors.RowCount > wgAppConfig.LogEventMaxCount)
			{
				object obj3 = text2;
				text2 = string.Concat(new object[]
				{
					obj3,
					"......(",
					this.dgvSelectedDoors.RowCount,
					")"
				});
			}
			else
			{
				object obj4 = text2;
				text2 = string.Concat(new object[]
				{
					obj4,
					"(",
					this.dgvSelectedDoors.RowCount,
					")"
				});
			}
			wgAppConfig.wgLog(string.Format("{0}:[{1} => {2}]:{3} => {4}", new object[]
			{
				(sender as Button).Text.Replace("\r\n", ""),
				this.dgvSelectedUsers.RowCount.ToString(),
				this.dgvSelectedDoors.RowCount.ToString(),
				text,
				text2
			}), EventLogEntryType.Information, null);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0004E3F4 File Offset: 0x0004D3F4
		private void manageCommonDoorControlGroup_Click(object sender, EventArgs e)
		{
			dfrmUserSelected dfrmUserSelected = new dfrmUserSelected();
			dfrmUserSelected.Text = this.manageCommonDoorControlGroup.Text;
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			wgAppConfig.getSystemParamValue(154, out text, out text2, out text3, out text4);
			string text5 = text3;
			if (!string.IsNullOrEmpty(text5))
			{
				dfrmUserSelected.selectedUsers = text5;
			}
			if (dfrmUserSelected.ShowDialog(this) == DialogResult.OK)
			{
				if (!string.IsNullOrEmpty(dfrmUserSelected.selectedUsers) && dfrmUserSelected.selectedUsers.Split(new char[] { ',' }).Length > 31)
				{
					XMessageBox.Show(CommonStr.strDoorControlGroupOver30);
					return;
				}
				wgAppConfig.setSystemParamValue(154, "Common DoorControl Consumers", "", dfrmUserSelected.selectedUsers);
				this.mnuDoorTypeSelect_loadData();
				XMessageBox.Show(this.manageCommonDoorControlGroup.Text + "\r\n" + CommonStr.strSuccessfully);
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0004E4E0 File Offset: 0x0004D4E0
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
										if ((int)table.Rows[k]["f_DoorID"] == (int)this.dvCommonUserPrivilege[j]["f_DoorID"] && this.bAllowedDoor((int)table.Rows[k]["f_DoorID"], (int)table.Rows[k]["f_ZoneID"]))
										{
											table.Rows[k]["f_Selected"] = 1;
										}
									}
								}
							}
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0004E6CC File Offset: 0x0004D6CC
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
					wgAppConfig.wgLog(ex.ToString());
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0004E8C4 File Offset: 0x0004D8C4
		private int multithreadUpdate(bool bAddNotDel)
		{
			if (!wgAppConfig.IsAcceleratorActive)
			{
				return -1;
			}
			bool flag = false;
			dfrmMultiThreadOperation dfrmMultiThreadOperation = new dfrmMultiThreadOperation();
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
			{
				int num = (int)((DataView)this.dgvSelectedDoors.DataSource)[i]["f_ControllerID"];
				if (arrayList.IndexOf(num) < 0)
				{
					arrayList.Add(num);
				}
			}
			dfrmMultiThreadOperation.arrSelectedDoorsOnConsole = arrayList;
			ArrayList arrayList2 = new ArrayList();
			for (int j = 0; j < this.dgvSelectedUsers.Rows.Count; j++)
			{
				int num2 = (int)((DataView)this.dgvSelectedUsers.DataSource)[j]["f_ConsumerID"];
				if (arrayList2.IndexOf(num2) < 0)
				{
					arrayList2.Add(num2);
				}
			}
			dfrmMultiThreadOperation.arrConsumerID = arrayList2;
			dfrmMultiThreadOperation.startDownloadAddDelPri(bAddNotDel);
			DateTime.Now.AddSeconds(20.0);
			while (!dfrmMultiThreadOperation.bComplete4AddDelPri)
			{
				Application.DoEvents();
				Thread.Sleep(100);
			}
			if (dfrmMultiThreadOperation.bCompleteFullOK4AddDelPri)
			{
				flag = true;
			}
			dfrmMultiThreadOperation.Close();
			this.dfrmWait1.Hide();
			this.dfrmWait1.Location = new Point(this.dfrmWait1.Location.X, 0);
			this.Refresh();
			Application.DoEvents();
			if (!flag)
			{
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					bAddNotDel ? this.btnAddPassAndUpload.Text.Replace("\r\n", "") : this.btnDeletePassAndUpload.Text.Replace("\r\n", ""),
					" ,",
					CommonStr.strUsersNum,
					" = ",
					this.dgvSelectedUsers.RowCount.ToString(),
					",",
					CommonStr.strDoorsNum,
					" = ",
					this.dgvSelectedDoors.RowCount.ToString(),
					",",
					CommonStr.strDelAddAndUploadFail.Replace("\r\n", "")
				}), EventLogEntryType.Information, null);
				XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.progressBar1.Value = 0;
				return 0;
			}
			return 1;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0004EB4C File Offset: 0x0004DB4C
		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				if (!this.bStarting)
				{
					if (this.progressBar1.Value != 0 && this.progressBar1.Value != this.progressBar1.Maximum)
					{
						Cursor.Current = Cursors.WaitCursor;
					}
				}
				else if (this.dgvUsers.DataSource == null)
				{
					Cursor.Current = Cursors.WaitCursor;
				}
				else
				{
					Cursor.Current = Cursors.Default;
					this.lblWait.Visible = false;
					this.groupBox1.Enabled = true;
					this.bStarting = false;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0004EBF4 File Offset: 0x0004DBF4
		private void udpserver_evNewRecord(string info)
		{
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0004EBF8 File Offset: 0x0004DBF8
		private void waitCloudServerConnect()
		{
			if (wgTools.bUDPCloud != 0 && this.dtStart.AddSeconds(5.0) >= DateTime.Now)
			{
				DateTime dateTime = DateTime.Now.AddSeconds(5.0);
				while (dateTime > DateTime.Now)
				{
					if (wgTools.arrSNIP.Count > 0)
					{
						return;
					}
					Thread.Sleep(300);
				}
			}
		}

		// Token: 0x040004C7 RID: 1223
		private ArrayList arrCommonUsersID;

		// Token: 0x040004C8 RID: 1224
		private ArrayList arrCommonUsersLoad;

		// Token: 0x040004C9 RID: 1225
		private ArrayList arrCommonUsersName;

		// Token: 0x040004CA RID: 1226
		private bool bEdit;

		// Token: 0x040004CB RID: 1227
		private SqlCommand cmd;

		// Token: 0x040004CC RID: 1228
		private SqlConnection cn;

		// Token: 0x040004CD RID: 1229
		private Control defaultFindControl;

		// Token: 0x040004CE RID: 1230
		private DataTable dt;

		// Token: 0x040004CF RID: 1231
		private DataTable dtCommonUserPrivilege;

		// Token: 0x040004D0 RID: 1232
		private DataTable dtDoorTmpSelected;

		// Token: 0x040004D1 RID: 1233
		private DataView dv;

		// Token: 0x040004D2 RID: 1234
		private DataView dv1;

		// Token: 0x040004D3 RID: 1235
		private DataView dv2;

		// Token: 0x040004D4 RID: 1236
		private DataView dvCommonUserPrivilege;

		// Token: 0x040004D5 RID: 1237
		private DataView dvDoorTmpSelected;

		// Token: 0x040004D6 RID: 1238
		private DataView dvSelected;

		// Token: 0x040004D7 RID: 1239
		private DataView dvSelectedControllerID;

		// Token: 0x040004D8 RID: 1240
		private DataView dvtmp;

		// Token: 0x040004D9 RID: 1241
		private int allowUpdateMax = 50;

		// Token: 0x040004DA RID: 1242
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x040004DB RID: 1243
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x040004DC RID: 1244
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x040004DD RID: 1245
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x040004DE RID: 1246
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x040004DF RID: 1247
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x040004E0 RID: 1248
		public bool bLoadConsole;

		// Token: 0x040004E1 RID: 1249
		private bool bStarting = true;

		// Token: 0x040004E2 RID: 1250
		private int[] controlSegIDList = new int[256];

		// Token: 0x040004E4 RID: 1252
		private DateTime dtStart = DateTime.Now;

		// Token: 0x040004E5 RID: 1253
		private string strGroupFilter = "";

		// Token: 0x040004E6 RID: 1254
		private string strZoneFilter = "";

		// Token: 0x040004E7 RID: 1255
		public WatchingService watching;
	}
}
