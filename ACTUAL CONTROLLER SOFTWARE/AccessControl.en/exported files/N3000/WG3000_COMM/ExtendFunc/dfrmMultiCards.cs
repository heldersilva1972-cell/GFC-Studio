using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000244 RID: 580
	public partial class dfrmMultiCards : frmN3000
	{
		// Token: 0x060011EF RID: 4591 RVA: 0x00152BA0 File Offset: 0x00151BA0
		public dfrmMultiCards()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00152C08 File Offset: 0x00151C08
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

		// Token: 0x060011F1 RID: 4593 RVA: 0x00152C4C File Offset: 0x00151C4C
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
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00152CCC File Offset: 0x00151CCC
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("btnAddAllUsers_Click Start");
			DataTable table = ((DataView)this.dgvUsers.DataSource).Table;
			DataView dataView = (DataView)this.dgvUsers.DataSource;
			DataView dataView2 = (DataView)this.dgvSelectedUsers.DataSource;
			this.dgvUsers.DataSource = null;
			this.dgvSelectedUsers.DataSource = null;
			if (this.strGroupFilter == "")
			{
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if ((int)table.Rows[i]["f_Selected"] != 1)
					{
						table.Rows[i]["f_Selected"] = 1;
						table.Rows[i]["f_MoreCards_GrpID"] = this.nudGroupToAdd.Value;
					}
				}
			}
			else
			{
				this.dv = new DataView(table);
				this.dv.RowFilter = this.strGroupFilter;
				for (int j = 0; j < this.dv.Count; j++)
				{
					if ((int)this.dv[j]["f_Selected"] != 1)
					{
						this.dv[j]["f_Selected"] = 1;
						this.dv[j]["f_MoreCards_GrpID"] = this.nudGroupToAdd.Value;
					}
				}
			}
			this.dgvUsers.DataSource = dataView;
			this.dgvSelectedUsers.DataSource = dataView2;
			wgTools.WriteLine("btnAddAllUsers_Click End");
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00152E90 File Offset: 0x00151E90
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, "f_MoreCards_GrpID", this.nudGroupToAdd.Value.ToString());
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00152EC0 File Offset: 0x00151EC0
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x00152EC8 File Offset: 0x00151EC8
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("btnDelAllUsers_Click Start");
			this.dt = ((DataView)this.dgvUsers.DataSource).Table;
			this.dv1 = (DataView)this.dgvUsers.DataSource;
			this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
			this.dgvUsers.DataSource = null;
			this.dgvSelectedUsers.DataSource = null;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 0;
			}
			this.dgvUsers.DataSource = this.dv1;
			this.dgvSelectedUsers.DataSource = this.dv2;
			wgTools.WriteLine("btnDelAllUsers_Click End");
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00152FBA File Offset: 0x00151FBA
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers);
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00152FC8 File Offset: 0x00151FC8
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOK_Click_Acc(sender, e);
				return;
			}
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			if (this.cn.State == ConnectionState.Closed)
			{
				this.cn.Open();
			}
			if (wgMjController.GetControllerType(this.controllerSN) == 1 || wgMjController.GetControllerType(this.controllerSN) == 2)
			{
				if (this.chkReaderIn.Checked && this.chkActive.Checked)
				{
					this.moreCards_GoInOut |= 1 << (this.doorNo - 1) * 2;
				}
				else
				{
					this.moreCards_GoInOut &= 255 - (1 << (this.doorNo - 1) * 2);
				}
				if (this.chkReaderOut.Checked && this.chkActive.Checked)
				{
					this.moreCards_GoInOut |= 1 << (this.doorNo - 1) * 2 + 1;
				}
				else
				{
					this.moreCards_GoInOut &= 255 - (1 << (this.doorNo - 1) * 2 + 1);
				}
			}
			else if (wgMjController.GetControllerType(this.controllerSN) == 4)
			{
				if (this.chkActive.Checked)
				{
					this.moreCards_GoInOut |= 1 << this.doorNo - 1;
				}
				else
				{
					this.moreCards_GoInOut &= 255 - (1 << this.doorNo - 1);
				}
			}
			else if (this.chkActive.Checked)
			{
				this.moreCards_GoInOut |= 1 << this.doorNo - 1;
			}
			else
			{
				this.moreCards_GoInOut &= 255 - (1 << this.doorNo - 1);
			}
			string text = string.Concat(new object[] { "update t_b_Controller set f_MoreCards_GoInOut =", this.moreCards_GoInOut, " Where f_ControllerID = ", this.controllerID });
			this.cmd = new SqlCommand(text, this.cn);
			this.cmd.ExecuteNonQuery();
			int num = 0;
			if (this.chkReadByOrder.Checked)
			{
				num += 16;
			}
			if (this.chkSingleGroup.Checked)
			{
				num += 8;
				num += (int)decimal.Subtract(this.nudGrpStartOfSingle.Value, 1m);
			}
			if (this.chkActive.Checked)
			{
				text = string.Concat(new object[]
				{
					"update t_b_door set f_MoreCards_Total =",
					this.nudTotal.Value,
					", f_MoreCards_Grp1=",
					this.nudGrp1.Value,
					", f_MoreCards_Grp2=",
					this.nudGrp2.Value,
					", f_MoreCards_Grp3=",
					this.nudGrp3.Value,
					", f_MoreCards_Grp4=",
					this.nudGrp4.Value,
					", f_MoreCards_Grp5=",
					this.nudGrp5.Value,
					", f_MoreCards_Grp6=",
					this.nudGrp6.Value,
					", f_MoreCards_Grp7=",
					this.nudGrp7.Value,
					", f_MoreCards_Grp8=",
					this.nudGrp8.Value,
					", f_MoreCards_Option=",
					num,
					" Where f_DoorID = ",
					this.DoorID
				});
			}
			else
			{
				text = string.Concat(new object[]
				{
					"update t_b_door set f_MoreCards_Total =", 0, ", f_MoreCards_Grp1=", 0, ", f_MoreCards_Grp2=", 0, ", f_MoreCards_Grp3=", 0, ", f_MoreCards_Grp4=", 0,
					", f_MoreCards_Grp5=", 0, ", f_MoreCards_Grp6=", 0, ", f_MoreCards_Grp7=", 0, ", f_MoreCards_Grp8=", 0, ", f_MoreCards_Option=", 0,
					" Where f_DoorID = ", this.DoorID
				});
			}
			this.cmd = new SqlCommand(text, this.cn);
			this.cmd.ExecuteNonQuery();
			text = " Delete  FROM t_d_doorMoreCardsUsers  WHERE f_DoorID= " + this.DoorID;
			this.cmd = new SqlCommand(text, this.cn);
			this.cmd.ExecuteNonQuery();
			this.dvSelected = this.dgvSelectedUsers.DataSource as DataView;
			if (this.chkActive.Checked && this.nudTotal.Value > 0m && this.dvSelected != null)
			{
				for (int i = 1; i <= 9; i++)
				{
					this.dvSelected.RowFilter = "f_Selected > 0 AND f_MoreCards_GrpID = " + i;
					if (this.dvSelected.Count > 0)
					{
						for (int j = 0; j <= this.dvSelected.Count - 1; j++)
						{
							text = "INSERT INTO [t_d_doorMoreCardsUsers] (f_ConsumerID, f_DoorID ,f_MoreCards_GrpID)";
							string text2 = text + " VALUES( " + this.dvSelected[j]["f_ConsumerID"].ToString();
							text = string.Concat(new string[]
							{
								text2,
								", ",
								this.DoorID.ToString(),
								",",
								i.ToString(),
								")"
							});
							this.cmd.CommandText = text;
							this.cmd.ExecuteNonQuery();
						}
					}
				}
			}
			if (this.chkActive.Checked)
			{
				this.retValue = this.nudTotal.Value.ToString();
			}
			else
			{
				this.retValue = "0";
			}
			if (this.cn.State == ConnectionState.Open)
			{
				this.cn.Close();
			}
			base.DialogResult = DialogResult.OK;
			this.Cursor = cursor;
			base.Close();
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00153688 File Offset: 0x00152688
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			if (oleDbConnection.State == ConnectionState.Closed)
			{
				oleDbConnection.Open();
			}
			if (wgMjController.GetControllerType(this.controllerSN) == 1 || wgMjController.GetControllerType(this.controllerSN) == 2)
			{
				if (this.chkReaderIn.Checked && this.chkActive.Checked)
				{
					this.moreCards_GoInOut |= 1 << (this.doorNo - 1) * 2;
				}
				else
				{
					this.moreCards_GoInOut &= 255 - (1 << (this.doorNo - 1) * 2);
				}
				if (this.chkReaderOut.Checked && this.chkActive.Checked)
				{
					this.moreCards_GoInOut |= 1 << (this.doorNo - 1) * 2 + 1;
				}
				else
				{
					this.moreCards_GoInOut &= 255 - (1 << (this.doorNo - 1) * 2 + 1);
				}
			}
			else if (wgMjController.GetControllerType(this.controllerSN) == 4)
			{
				if (this.chkActive.Checked)
				{
					this.moreCards_GoInOut |= 1 << this.doorNo - 1;
				}
				else
				{
					this.moreCards_GoInOut &= 255 - (1 << this.doorNo - 1);
				}
			}
			else if (this.chkActive.Checked)
			{
				this.moreCards_GoInOut |= 1 << this.doorNo - 1;
			}
			else
			{
				this.moreCards_GoInOut &= 255 - (1 << this.doorNo - 1);
			}
			new OleDbCommand(string.Concat(new object[] { "update t_b_Controller set f_MoreCards_GoInOut =", this.moreCards_GoInOut, " Where f_ControllerID = ", this.controllerID }), oleDbConnection).ExecuteNonQuery();
			int num = 0;
			if (this.chkReadByOrder.Checked)
			{
				num += 16;
			}
			if (this.chkSingleGroup.Checked)
			{
				num += 8;
				num += (int)decimal.Subtract(this.nudGrpStartOfSingle.Value, 1m);
			}
			string text;
			if (this.chkActive.Checked)
			{
				text = string.Concat(new object[]
				{
					"update t_b_door set f_MoreCards_Total =",
					this.nudTotal.Value,
					", f_MoreCards_Grp1=",
					this.nudGrp1.Value,
					", f_MoreCards_Grp2=",
					this.nudGrp2.Value,
					", f_MoreCards_Grp3=",
					this.nudGrp3.Value,
					", f_MoreCards_Grp4=",
					this.nudGrp4.Value,
					", f_MoreCards_Grp5=",
					this.nudGrp5.Value,
					", f_MoreCards_Grp6=",
					this.nudGrp6.Value,
					", f_MoreCards_Grp7=",
					this.nudGrp7.Value,
					", f_MoreCards_Grp8=",
					this.nudGrp8.Value,
					", f_MoreCards_Option=",
					num,
					" Where f_DoorID = ",
					this.DoorID
				});
			}
			else
			{
				text = string.Concat(new object[]
				{
					"update t_b_door set f_MoreCards_Total =", 0, ", f_MoreCards_Grp1=", 0, ", f_MoreCards_Grp2=", 0, ", f_MoreCards_Grp3=", 0, ", f_MoreCards_Grp4=", 0,
					", f_MoreCards_Grp5=", 0, ", f_MoreCards_Grp6=", 0, ", f_MoreCards_Grp7=", 0, ", f_MoreCards_Grp8=", 0, ", f_MoreCards_Option=", 0,
					" Where f_DoorID = ", this.DoorID
				});
			}
			new OleDbCommand(text, oleDbConnection).ExecuteNonQuery();
			OleDbCommand oleDbCommand = new OleDbCommand(" Delete  FROM t_d_doorMoreCardsUsers  WHERE f_DoorID= " + this.DoorID, oleDbConnection);
			oleDbCommand.ExecuteNonQuery();
			this.dvSelected = this.dgvSelectedUsers.DataSource as DataView;
			if (this.chkActive.Checked && this.nudTotal.Value > 0m && this.dvSelected != null)
			{
				for (int i = 1; i <= 9; i++)
				{
					this.dvSelected.RowFilter = "f_Selected > 0 AND f_MoreCards_GrpID = " + i;
					if (this.dvSelected.Count > 0)
					{
						for (int j = 0; j <= this.dvSelected.Count - 1; j++)
						{
							text = "INSERT INTO [t_d_doorMoreCardsUsers] (f_ConsumerID, f_DoorID ,f_MoreCards_GrpID)";
							string text2 = text + " VALUES( " + this.dvSelected[j]["f_ConsumerID"].ToString();
							text = string.Concat(new string[]
							{
								text2,
								", ",
								this.DoorID.ToString(),
								",",
								i.ToString(),
								")"
							});
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
						}
					}
				}
			}
			if (this.chkActive.Checked)
			{
				this.retValue = this.nudTotal.Value.ToString();
			}
			else
			{
				this.retValue = "0";
			}
			if (oleDbConnection.State == ConnectionState.Open)
			{
				oleDbConnection.Close();
			}
			base.DialogResult = DialogResult.OK;
			this.Cursor = cursor;
			base.Close();
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00153CF1 File Offset: 0x00152CF1
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00153D00 File Offset: 0x00152D00
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
			if (this.dgvUsers.DataSource != null)
			{
				DataView dataView = (DataView)this.dgvUsers.DataSource;
				if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					dataView.RowFilter = "f_Selected = 0";
					this.strGroupFilter = "";
					return;
				}
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
						return;
					}
					dataView.RowFilter = "f_Selected = 0 ";
					string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
					dataView.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", groupQuery);
					this.strGroupFilter = string.Format("  {0} ", groupQuery);
				}
			}
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00153EB0 File Offset: 0x00152EB0
		private void chkActive_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkActive.Checked)
			{
				this.grpNeeded.Visible = true;
				if (this.grpOptInOut.Enabled)
				{
					this.grpOptInOut.Visible = true;
				}
				this.groupBox1.Visible = true;
				if (this.dgvUsers.DataSource != null)
				{
					DataView dataView = (DataView)this.dgvUsers.DataSource;
					this.dgvUsers.DataSource = null;
					this.dgvUsers.DataSource = dataView;
					return;
				}
			}
			else
			{
				this.grpNeeded.Visible = false;
				this.grpOptInOut.Visible = false;
				this.groupBox1.Visible = false;
			}
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00153F56 File Offset: 0x00152F56
		private void chkSingleGroup_CheckedChanged(object sender, EventArgs e)
		{
			this.nudGrpStartOfSingle.Visible = this.chkSingleGroup.Checked;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00153F6E File Offset: 0x00152F6E
		private void dfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00153F84 File Offset: 0x00152F84
		private void dfrmMultiCards_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.grpOption.Visible = true;
			}
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

		// Token: 0x060011FF RID: 4607 RVA: 0x00154038 File Offset: 0x00153038
		private void dfrmMultiCards_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[4]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[4]);
			if (wgAppConfig.IsAccessDB)
			{
				this.dfrmMultiCards_Load_Acc(sender, e);
				return;
			}
			this.loadGroupData();
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			string text = "SELECT t_b_Door.*,t_b_Controller.f_ControllerSN, t_b_Controller.f_MoreCards_GoInOut  FROM  t_b_Controller,t_b_Door  ";
			text = text + " Where  t_b_Controller.f_ControllerID=t_b_Door.f_ControllerID and t_b_door.f_DoorID = " + this.DoorID;
			if (this.cn.State == ConnectionState.Closed)
			{
				this.cn.Open();
			}
			this.cmd = new SqlCommand(text, this.cn);
			SqlDataReader sqlDataReader = this.cmd.ExecuteReader();
			if (sqlDataReader.Read())
			{
				this.controllerID = (int)sqlDataReader["f_ControllerID"];
				this.controllerSN = (int)sqlDataReader["f_ControllerSN"];
				this.moreCards_GoInOut = (int)sqlDataReader["f_MoreCards_GoInOut"];
				this.doorNo = int.Parse(sqlDataReader["f_DoorNo"].ToString());
				if (wgMjController.GetControllerType(this.controllerSN) == 1 || wgMjController.GetControllerType(this.controllerSN) == 2)
				{
					this.grpOptInOut.Visible = true;
					int num = (this.moreCards_GoInOut >> (this.doorNo - 1) * 2) & 3;
					this.chkReaderIn.Checked = (num & 1) > 0;
					this.chkReaderOut.Checked = (num & 2) > 0;
				}
				else if (wgMjController.GetControllerType(this.controllerSN) == 4)
				{
					this.grpOptInOut.Visible = false;
					this.chkReaderIn.Checked = true;
					this.chkReaderIn.Enabled = false;
					this.chkReaderOut.Visible = false;
					this.grpOptInOut.Enabled = false;
					this.grpOptInOut.Visible = false;
				}
				else
				{
					this.grpOptInOut.Visible = false;
					this.chkReaderIn.Checked = true;
					this.grpOptInOut.Enabled = false;
					this.grpOptInOut.Visible = false;
				}
				if ((int)sqlDataReader["f_MoreCards_Total"] > 0)
				{
					this.chkActive.Checked = true;
					this.chkActive_CheckedChanged(null, null);
				}
				else
				{
					this.chkActive.Checked = false;
					this.chkActive_CheckedChanged(null, null);
				}
				if ((int)sqlDataReader["f_MoreCards_Total"] > 1)
				{
					this.nudTotal.Value = (int)sqlDataReader["f_MoreCards_Total"];
				}
				this.nudGrp1.Value = (int)sqlDataReader["f_MoreCards_Grp1"];
				this.nudGrp2.Value = (int)sqlDataReader["f_MoreCards_Grp2"];
				this.nudGrp3.Value = (int)sqlDataReader["f_MoreCards_Grp3"];
				this.nudGrp4.Value = (int)sqlDataReader["f_MoreCards_Grp4"];
				this.nudGrp5.Value = (int)sqlDataReader["f_MoreCards_Grp5"];
				this.nudGrp6.Value = (int)sqlDataReader["f_MoreCards_Grp6"];
				this.nudGrp7.Value = (int)sqlDataReader["f_MoreCards_Grp7"];
				this.nudGrp8.Value = (int)sqlDataReader["f_MoreCards_Grp8"];
				int num2 = (int)sqlDataReader["f_MoreCards_Option"];
				if ((num2 & 16) > 0)
				{
					this.chkReadByOrder.Checked = true;
				}
				if ((num2 & 8) > 0)
				{
					this.chkSingleGroup.Checked = true;
					this.nudGrpStartOfSingle.Value = 1 + (num2 & 7);
					this.nudGrpStartOfSingle.Visible = true;
				}
				else
				{
					this.chkSingleGroup.Checked = false;
					this.nudGrpStartOfSingle.Visible = false;
				}
				if (this.chkReadByOrder.Checked || this.chkSingleGroup.Checked)
				{
					this.grpOption.Visible = true;
				}
			}
			sqlDataReader.Close();
			this.cn.Close();
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x001544E8 File Offset: 0x001534E8
		private void dfrmMultiCards_Load_Acc(object sender, EventArgs e)
		{
			this.loadGroupData();
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			string text = "SELECT t_b_Door.*,t_b_Controller.f_ControllerSN, t_b_Controller.f_MoreCards_GoInOut  FROM  t_b_Controller,t_b_Door  ";
			text = text + " Where  t_b_Controller.f_ControllerID=t_b_Door.f_ControllerID and t_b_door.f_DoorID = " + this.DoorID;
			if (oleDbConnection.State == ConnectionState.Closed)
			{
				oleDbConnection.Open();
			}
			OleDbDataReader oleDbDataReader = new OleDbCommand(text, oleDbConnection).ExecuteReader();
			if (oleDbDataReader.Read())
			{
				this.controllerID = (int)oleDbDataReader["f_ControllerID"];
				this.controllerSN = (int)oleDbDataReader["f_ControllerSN"];
				this.moreCards_GoInOut = (int)oleDbDataReader["f_MoreCards_GoInOut"];
				this.doorNo = int.Parse(oleDbDataReader["f_DoorNo"].ToString());
				if (wgMjController.GetControllerType(this.controllerSN) == 1 || wgMjController.GetControllerType(this.controllerSN) == 2)
				{
					this.grpOptInOut.Visible = true;
					int num = (this.moreCards_GoInOut >> (this.doorNo - 1) * 2) & 3;
					this.chkReaderIn.Checked = (num & 1) > 0;
					this.chkReaderOut.Checked = (num & 2) > 0;
				}
				else if (wgMjController.GetControllerType(this.controllerSN) == 4)
				{
					this.grpOptInOut.Visible = false;
					this.chkReaderIn.Checked = true;
					this.chkReaderIn.Enabled = false;
					this.chkReaderOut.Visible = false;
					this.grpOptInOut.Enabled = false;
					this.grpOptInOut.Visible = false;
				}
				else
				{
					this.grpOptInOut.Visible = false;
					this.chkReaderIn.Checked = true;
					this.grpOptInOut.Enabled = false;
					this.grpOptInOut.Visible = false;
				}
				if ((int)oleDbDataReader["f_MoreCards_Total"] > 0)
				{
					this.chkActive.Checked = true;
					this.chkActive_CheckedChanged(null, null);
				}
				else
				{
					this.chkActive.Checked = false;
					this.chkActive_CheckedChanged(null, null);
				}
				if ((int)oleDbDataReader["f_MoreCards_Total"] > 1)
				{
					this.nudTotal.Value = (int)oleDbDataReader["f_MoreCards_Total"];
				}
				this.nudGrp1.Value = (int)oleDbDataReader["f_MoreCards_Grp1"];
				this.nudGrp2.Value = (int)oleDbDataReader["f_MoreCards_Grp2"];
				this.nudGrp3.Value = (int)oleDbDataReader["f_MoreCards_Grp3"];
				this.nudGrp4.Value = (int)oleDbDataReader["f_MoreCards_Grp4"];
				this.nudGrp5.Value = (int)oleDbDataReader["f_MoreCards_Grp5"];
				this.nudGrp6.Value = (int)oleDbDataReader["f_MoreCards_Grp6"];
				this.nudGrp7.Value = (int)oleDbDataReader["f_MoreCards_Grp7"];
				this.nudGrp8.Value = (int)oleDbDataReader["f_MoreCards_Grp8"];
				int num2 = (int)oleDbDataReader["f_MoreCards_Option"];
				if ((num2 & 16) > 0)
				{
					this.chkReadByOrder.Checked = true;
				}
				if ((num2 & 8) > 0)
				{
					this.chkSingleGroup.Checked = true;
					this.nudGrpStartOfSingle.Value = 1 + (num2 & 7);
					this.nudGrpStartOfSingle.Visible = true;
				}
				else
				{
					this.chkSingleGroup.Checked = false;
					this.nudGrpStartOfSingle.Visible = false;
				}
				if (this.chkReadByOrder.Checked || this.chkSingleGroup.Checked)
				{
					this.grpOption.Visible = true;
				}
			}
			oleDbDataReader.Close();
			oleDbConnection.Close();
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.backgroundWorker1.RunWorkerAsync();
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00154934 File Offset: 0x00153934
		private void loadGroupData()
		{
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
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x001549E8 File Offset: 0x001539E8
		private DataTable loadUserData4BackWork()
		{
			Thread.Sleep(100);
			wgTools.WriteLine("loadUserData Start");
			this.dtUser1 = new DataTable();
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = " SELECT  t_b_Consumer.f_ConsumerID, ";
				text = text + " IIF ( f_MoreCards_GrpID IS NULL , 0 , f_MoreCards_GrpID ) AS f_MoreCards_GrpID  , f_ConsumerNO, f_ConsumerName, f_CardNO  , IIF (  f_MoreCards_GrpID IS NULL , 0 , 1 ) AS f_Selected  , f_GroupID  FROM t_b_Consumer " + string.Format(" LEFT OUTER JOIN t_d_doorMoreCardsUsers ON ( t_b_Consumer.f_ConsumerID=t_d_doorMoreCardsUsers.f_ConsumerID AND f_DoorID= {0} )", this.DoorID.ToString()) + " WHERE f_DoorEnabled > 0 ORDER BY f_ConsumerNO ASC ";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtUser1);
						}
					}
					goto IL_0130;
				}
			}
			text = " SELECT  t_b_Consumer.f_ConsumerID, ";
			text = string.Concat(new object[] { text, " CASE WHEN f_MoreCards_GrpID IS NULL THEN 0 ELSE f_MoreCards_GrpID END AS f_MoreCards_GrpID  , f_ConsumerNO, f_ConsumerName, f_CardNO  , CASE WHEN f_MoreCards_GrpID IS NULL THEN 0 ELSE 1 END AS f_Selected  , f_GroupID  FROM t_b_Consumer  LEFT OUTER JOIN t_d_doorMoreCardsUsers ON t_b_Consumer.f_ConsumerID=t_d_doorMoreCardsUsers.f_ConsumerID AND f_DoorID= ", this.DoorID, " WHERE f_DoorEnabled > 0 ORDER BY f_ConsumerNO ASC " });
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtUser1);
					}
				}
			}
			IL_0130:
			wgTools.WriteLine("da.Fill End");
			try
			{
				this.dtUser1.PrimaryKey = new DataColumn[] { this.dtUser1.Columns[0] };
			}
			catch (Exception)
			{
				throw;
			}
			dfrmMultiCards.lastLoadUsers = icConsumerShare.getUpdateLog();
			dfrmMultiCards.dtLastLoad = this.dtUser1;
			return this.dtUser1;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x00154BD4 File Offset: 0x00153BD4
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			this.dv.RowFilter = "f_Selected = 0";
			this.dvSelected.RowFilter = "f_Selected > 0";
			this.dvSelected.Sort = "f_MoreCards_GrpID ASC, f_ConsumerNo ASC ";
			this.dgvUsers.AutoGenerateColumns = false;
			this.dgvUsers.DataSource = this.dv;
			this.dgvSelectedUsers.AutoGenerateColumns = false;
			this.dgvSelectedUsers.DataSource = this.dvSelected;
			for (int i = 0; i < this.dv.Table.Columns.Count; i++)
			{
				this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
				this.dgvSelectedUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
			}
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			wgTools.WriteLine("loadUserData End");
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00154CE4 File Offset: 0x00153CE4
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.dgvUsers.DataSource == null)
			{
				Cursor.Current = Cursors.WaitCursor;
				return;
			}
			this.timer1.Enabled = false;
			Cursor.Current = Cursors.Default;
			this.lblWait.Visible = false;
			this.groupBox1.Enabled = true;
			this.btnOK.Enabled = true;
		}

		// Token: 0x04002050 RID: 8272
		private const int MoreCardGroupMaxLen = 9;

		// Token: 0x04002051 RID: 8273
		private SqlCommand cmd;

		// Token: 0x04002052 RID: 8274
		private SqlConnection cn;

		// Token: 0x04002053 RID: 8275
		private int controllerID;

		// Token: 0x04002054 RID: 8276
		private int controllerSN;

		// Token: 0x04002055 RID: 8277
		private int doorNo;

		// Token: 0x04002056 RID: 8278
		private DataTable dt;

		// Token: 0x04002057 RID: 8279
		private DataTable dtUser1;

		// Token: 0x04002058 RID: 8280
		private DataView dv;

		// Token: 0x04002059 RID: 8281
		private DataView dv1;

		// Token: 0x0400205A RID: 8282
		private DataView dv2;

		// Token: 0x0400205B RID: 8283
		private DataView dvSelected;

		// Token: 0x0400205C RID: 8284
		private int moreCards_GoInOut;

		// Token: 0x0400205D RID: 8285
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x0400205E RID: 8286
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x0400205F RID: 8287
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04002060 RID: 8288
		public int DoorID;

		// Token: 0x04002061 RID: 8289
		private static DataTable dtLastLoad;

		// Token: 0x04002062 RID: 8290
		private static string lastLoadUsers = "";

		// Token: 0x04002063 RID: 8291
		public string retValue = "0";

		// Token: 0x04002064 RID: 8292
		private string strGroupFilter = "";
	}
}
