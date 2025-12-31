using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000039 RID: 57
	public partial class dfrmUserBatchUpdate : frmN3000
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x0006F5A4 File Offset: 0x0006E5A4
		public dfrmUserBatchUpdate()
		{
			this.InitializeComponent();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0006F606 File Offset: 0x0006E606
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0006F618 File Offset: 0x0006E618
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (this.chk5.Checked)
			{
				if (icConsumer.gTimeSecondEnabled)
				{
					this.dtpBegin.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpBegin.Value.ToString("yyyy-MM-dd"), this.dateBeginHMS1.Value.ToString("HH:mm:ss")));
					this.dtpEnd.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpEnd.Value.ToString("yyyy-MM-dd"), this.dateEndHMS1.Value.ToString("HH:mm:ss")));
				}
				else
				{
					bool @checked = this.chkStart.Checked;
					if (this.chkEnd.Checked)
					{
						if (this.dateEndHMS1.Value.ToString("HH:mm").Equals("00:00"))
						{
							this.dateEndHMS1.Value = DateTime.Parse(this.dateEndHMS1.Value.ToString("yyyy-MM-dd 23:59:59"));
						}
						this.dtpEnd.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpEnd.Value.ToString("yyyy-MM-dd"), this.dateEndHMS1.Value.ToString("HH:mm:59")));
						this.bNewDate = true;
						this.deactivateDateNew = this.dtpEnd.Value;
					}
				}
			}
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOK_Click_Acc(sender, e);
				return;
			}
			int num = 0;
			string text = "  ";
			int num2;
			int num3;
			if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
			{
				num2 = 0;
				num3 = 0;
			}
			else if (this.bInsertNullDepartment && this.cbof_GroupID.Text == wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartmentIsEmpty))
			{
				num2 = 0;
				num3 = 0;
				text = " WHERE f_GroupID = 0   ";
			}
			else
			{
				if (this.bInsertNullDepartment)
				{
					num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex - 1];
					num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex - 1];
					num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
				}
				else
				{
					num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
					num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
				}
				if (!this.chkIncludeAllBranch.Checked)
				{
					num3 = num2;
				}
			}
			if (num2 > 0)
			{
				if (num2 >= num3)
				{
					text = " FROM   t_b_Consumer,t_b_Group WHERE  t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ";
					text += string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", num);
				}
				else
				{
					text = " FROM   t_b_Consumer,t_b_Group   WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ";
					text = text + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num2) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", num3);
				}
			}
			if (!string.IsNullOrEmpty(this.strSqlSelected))
			{
				text = string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected);
			}
			if (this.chk1.Checked)
			{
				string text2 = "UPDATE t_b_Consumer   SET ";
				wgAppConfig.runUpdateSql(text2 + "  t_b_Consumer.[f_DoorEnabled]=" + (this.opt1a.Checked ? "1" : "0") + text);
			}
			if (this.chk3.Checked)
			{
				string text2 = "UPDATE t_b_Consumer SET ";
				wgAppConfig.runUpdateSql(text2 + "  t_b_Consumer.[f_ShiftEnabled]=" + (this.opt3b.Checked ? "1" : "0") + text);
			}
			if (this.chk2.Checked)
			{
				string text2 = "UPDATE t_b_Consumer SET ";
				wgAppConfig.runUpdateSql(text2 + "  t_b_Consumer.[f_AttendEnabled]=" + (this.opt2a.Checked ? "1" : "0") + text);
				if (!this.opt2a.Checked)
				{
					text2 = "UPDATE t_b_Consumer SET ";
					wgAppConfig.runUpdateSql(text2 + "  t_b_Consumer.[f_ShiftEnabled]=0" + text);
				}
				else if (wgAppConfig.getParamValBoolByNO(215))
				{
					text2 = "UPDATE t_b_Consumer SET ";
					wgAppConfig.runUpdateSql(text2 + "  t_b_Consumer.[f_AttendEnabled]=" + ((this.cmbNormalShift.SelectedIndex == 0) ? "1" : (this.cmbNormalShift.SelectedIndex + 1).ToString()) + text);
				}
			}
			if (this.chk5.Checked && (this.chkStart.Checked || this.chkEnd.Checked))
			{
				string text2 = "UPDATE t_b_Consumer SET ";
				if (icConsumer.gTimeSecondEnabled)
				{
					if (this.chkStart.Checked && this.chkEnd.Checked)
					{
						text2 = string.Concat(new string[]
						{
							text2,
							" t_b_Consumer.[f_BeginYMD]=",
							wgTools.PrepareStr(this.dtpBegin.Value, true, "yyyy-MM-dd HH:mm:ss"),
							"  ,t_b_Consumer.[f_EndYMD]=",
							wgTools.PrepareStr(this.dtpEnd.Value, true, "yyyy-MM-dd HH:mm:ss")
						});
					}
					else if (this.chkStart.Checked)
					{
						text2 = text2 + " t_b_Consumer.[f_BeginYMD]=" + wgTools.PrepareStr(this.dtpBegin.Value, true, "yyyy-MM-dd HH:mm:ss");
					}
					else if (this.chkEnd.Checked)
					{
						text2 = text2 + "  t_b_Consumer.[f_EndYMD]=" + wgTools.PrepareStr(this.dtpEnd.Value, true, "yyyy-MM-dd HH:mm:ss");
					}
				}
				else if (this.chkStart.Checked && this.chkEnd.Checked)
				{
					text2 = string.Concat(new string[]
					{
						text2,
						" t_b_Consumer.[f_BeginYMD]=",
						wgTools.PrepareStr(this.dtpBegin.Value, true, "yyyy-MM-dd"),
						"  ,t_b_Consumer.[f_EndYMD]=",
						wgTools.PrepareStr(this.dtpEnd.Value, true, "yyyy-MM-dd HH:mm:ss")
					});
				}
				else if (this.chkStart.Checked)
				{
					text2 = text2 + " t_b_Consumer.[f_BeginYMD]=" + wgTools.PrepareStr(this.dtpBegin.Value, true, "yyyy-MM-dd");
				}
				else if (this.chkEnd.Checked)
				{
					text2 = text2 + "  t_b_Consumer.[f_EndYMD]=" + wgTools.PrepareStr(this.dtpEnd.Value, true, "yyyy-MM-dd HH:mm:ss");
				}
				wgAppConfig.runUpdateSql(text2 + text);
			}
			if (this.chk6.Checked)
			{
				string text2 = "UPDATE t_b_Consumer SET ";
				wgAppConfig.runUpdateSql(text2 + " t_b_Consumer.[f_PIN] = " + ((this.txtf_PIN.Text == "") ? "0" : this.txtf_PIN.Text) + text);
			}
			if (this.chk4.Checked)
			{
				string text2 = "UPDATE t_b_Consumer SET ";
				if (this.cbof_GroupNew.SelectedIndex == -1)
				{
					text2 += "  t_b_Consumer.[f_GroupID]=0";
					this.groupIdUpdated = 0;
				}
				else
				{
					text2 = text2 + "  t_b_Consumer.[f_GroupID]=" + wgTools.SetObjToStr(this.arrGroupID[this.cbof_GroupNew.SelectedIndex]);
					this.groupIdUpdated = (int)this.arrGroupID[this.cbof_GroupNew.SelectedIndex];
				}
				wgAppConfig.runUpdateSql(text2 + text);
			}
			base.DialogResult = DialogResult.OK;
			icConsumerShare.setUpdateLog();
			base.Close();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0006FDC8 File Offset: 0x0006EDC8
		private void btnOK_Click_Acc(object sender, EventArgs e)
		{
			int num = 0;
			string text = "";
			string text2 = "  ";
			int num2;
			int num3;
			if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
			{
				num2 = 0;
				num3 = 0;
			}
			else if (this.bInsertNullDepartment && this.cbof_GroupID.Text == wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartmentIsEmpty))
			{
				num2 = 0;
				num3 = 0;
				text2 = " WHERE f_GroupID = 0   ";
			}
			else
			{
				if (this.bInsertNullDepartment)
				{
					num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex - 1];
					num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex - 1];
					num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
				}
				else
				{
					num = (int)this.arrGroupID[this.cbof_GroupID.SelectedIndex];
					num2 = (int)this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
					num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
				}
				if (!this.chkIncludeAllBranch.Checked)
				{
					num3 = num2;
				}
			}
			if (num2 > 0)
			{
				if (num2 >= num3)
				{
					text = "    INNER JOIN t_b_Group ON (  t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ";
					text += string.Format(" AND  t_b_Group.f_GroupID ={0:d} ) ", num);
				}
				else
				{
					text = "    INNER JOIN t_b_Group ON  ( t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ";
					text = text + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num2) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ) ", num3);
				}
			}
			if (!string.IsNullOrEmpty(this.strSqlSelected))
			{
				text2 = string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected);
				text = " ";
			}
			if (this.chk1.Checked)
			{
				string text3 = "UPDATE t_b_Consumer  ";
				wgAppConfig.runUpdateSql(string.Concat(new string[]
				{
					text3,
					text,
					"  SET  t_b_Consumer.[f_DoorEnabled]=",
					this.opt1a.Checked ? "1" : "0",
					text2
				}));
			}
			if (this.chk3.Checked)
			{
				string text3 = "UPDATE t_b_Consumer ";
				wgAppConfig.runUpdateSql(string.Concat(new string[]
				{
					text3,
					text,
					" SET  t_b_Consumer.[f_ShiftEnabled]=",
					this.opt3b.Checked ? "1" : "0",
					text2
				}));
			}
			if (this.chk2.Checked)
			{
				string text3 = "UPDATE t_b_Consumer ";
				wgAppConfig.runUpdateSql(string.Concat(new string[]
				{
					text3,
					text,
					" SET  t_b_Consumer.[f_AttendEnabled]=",
					this.opt2a.Checked ? "1" : "0",
					text2
				}));
				if (!this.opt2a.Checked)
				{
					text3 = "UPDATE t_b_Consumer  ";
					wgAppConfig.runUpdateSql(text3 + text + " SET t_b_Consumer.[f_ShiftEnabled]=0" + text2);
				}
				else if (wgAppConfig.getParamValBoolByNO(215))
				{
					text3 = "UPDATE t_b_Consumer  ";
					wgAppConfig.runUpdateSql(string.Concat(new string[]
					{
						text3,
						text,
						"SET  t_b_Consumer.[f_AttendEnabled]=",
						(this.cmbNormalShift.SelectedIndex == 0) ? "1" : (this.cmbNormalShift.SelectedIndex + 1).ToString(),
						text2
					}));
				}
			}
			if (this.chk5.Checked && (this.chkStart.Checked || this.chkEnd.Checked))
			{
				string text3 = "UPDATE t_b_Consumer ";
				text3 += text;
				if (icConsumer.gTimeSecondEnabled)
				{
					if (this.chkStart.Checked && this.chkEnd.Checked)
					{
						text3 = string.Concat(new string[]
						{
							text3,
							" SET t_b_Consumer.[f_BeginYMD]=",
							wgTools.PrepareStr(this.dtpBegin.Value, true, "yyyy-MM-dd HH:mm:ss"),
							"  ,t_b_Consumer.[f_EndYMD]=",
							wgTools.PrepareStr(this.dtpEnd.Value, true, "yyyy-MM-dd HH:mm:ss")
						});
					}
					else if (this.chkStart.Checked)
					{
						text3 = text3 + " SET t_b_Consumer.[f_BeginYMD]=" + wgTools.PrepareStr(this.dtpBegin.Value, true, "yyyy-MM-dd HH:mm:ss");
					}
					else if (this.chkEnd.Checked)
					{
						text3 = text3 + "SET t_b_Consumer.[f_EndYMD]=" + wgTools.PrepareStr(this.dtpEnd.Value, true, "yyyy-MM-dd HH:mm:ss");
					}
				}
				else if (this.chkStart.Checked && this.chkEnd.Checked)
				{
					text3 = string.Concat(new string[]
					{
						text3,
						" SET t_b_Consumer.[f_BeginYMD]=",
						wgTools.PrepareStr(this.dtpBegin.Value, true, "yyyy-MM-dd"),
						"  ,t_b_Consumer.[f_EndYMD]=",
						wgTools.PrepareStr(this.dtpEnd.Value, true, "yyyy-MM-dd HH:mm:ss")
					});
				}
				else if (this.chkStart.Checked)
				{
					text3 = text3 + " SET t_b_Consumer.[f_BeginYMD]=" + wgTools.PrepareStr(this.dtpBegin.Value, true, "yyyy-MM-dd");
				}
				else if (this.chkEnd.Checked)
				{
					text3 = text3 + "SET t_b_Consumer.[f_EndYMD]=" + wgTools.PrepareStr(this.dtpEnd.Value, true, "yyyy-MM-dd HH:mm:ss");
				}
				wgAppConfig.runUpdateSql(text3 + text2);
			}
			if (this.chk6.Checked)
			{
				string text3 = "UPDATE t_b_Consumer ";
				wgAppConfig.runUpdateSql(string.Concat(new string[]
				{
					text3,
					text,
					" SET  t_b_Consumer.[f_PIN] = ",
					(this.txtf_PIN.Text == "") ? "0" : this.txtf_PIN.Text,
					text2
				}));
			}
			if (this.chk4.Checked)
			{
				string text3 = "UPDATE t_b_Consumer ";
				text3 += text;
				if (this.cbof_GroupNew.SelectedIndex == -1)
				{
					text3 += " SET  t_b_Consumer.[f_GroupID]=0";
					this.groupIdUpdated = 0;
				}
				else
				{
					text3 = text3 + " SET  t_b_Consumer.[f_GroupID]=" + wgTools.SetObjToStr(this.arrGroupID[this.cbof_GroupNew.SelectedIndex]);
					this.groupIdUpdated = (int)this.arrGroupID[this.cbof_GroupNew.SelectedIndex];
				}
				wgAppConfig.runUpdateSql(text3 + text2);
			}
			base.DialogResult = DialogResult.OK;
			icConsumerShare.setUpdateLog();
			base.Close();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000704A5 File Offset: 0x0006F4A5
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000704B2 File Offset: 0x0006F4B2
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000704D0 File Offset: 0x0006F4D0
		private void cbof_GroupNew_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupNew);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000704DD File Offset: 0x0006F4DD
		private void cbof_GroupNew_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupNew, this.cbof_GroupNew.Text);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000704FB File Offset: 0x0006F4FB
		private void chk1_CheckedChanged(object sender, EventArgs e)
		{
			this.GroupBox1.Enabled = this.chk1.Checked;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00070513 File Offset: 0x0006F513
		private void chk2_CheckedChanged(object sender, EventArgs e)
		{
			this.GroupBox2.Enabled = this.chk2.Checked;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0007052B File Offset: 0x0006F52B
		private void chk3_CheckedChanged(object sender, EventArgs e)
		{
			this.GroupBox3.Enabled = this.chk3.Checked;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00070543 File Offset: 0x0006F543
		private void chk4_CheckedChanged(object sender, EventArgs e)
		{
			this.cbof_GroupNew.Enabled = this.chk4.Checked;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0007055B File Offset: 0x0006F55B
		private void chk5_CheckedChanged(object sender, EventArgs e)
		{
			this.GroupBox4.Enabled = this.chk5.Checked;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00070574 File Offset: 0x0006F574
		private void dateEndHMS1_ValueChanged(object sender, EventArgs e)
		{
			if (this.dateEndHMS1.Value.ToString("HH:mm").Equals("00:00"))
			{
				this.dateEndHMS1.Value = DateTime.Parse(this.dateEndHMS1.Value.ToString("yyyy-MM-dd 23:59:59"));
			}
			this.dtpEnd.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpEnd.Value.ToString("yyyy-MM-dd"), this.dateEndHMS1.Value.ToString("HH:mm:ss")));
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00070617 File Offset: 0x0006F617
		private void dfrmUserBatchUpdate_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.funcCtrlShiftQ();
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00070658 File Offset: 0x0006F658
		private void dfrmUserBatchUpdate_Load(object sender, EventArgs e)
		{
			this.cmbNormalShift.SelectedIndex = 0;
			if (wgAppConfig.getParamValBoolByNO(215))
			{
				this.cmbNormalShift.Visible = true;
			}
			else
			{
				this.cmbNormalShift.Visible = false;
			}
			this.txtf_PIN.Mask = "999999";
			this.txtf_PIN.Text = 345678.ToString();
			this.Label3.Text = wgAppConfig.ReplaceFloorRoom(this.Label3.Text);
			this.chk4.Text = wgAppConfig.ReplaceFloorRoom(this.chk4.Text);
			this.chkIncludeAllBranch.Text = wgAppConfig.ReplaceFloorRoom(this.chkIncludeAllBranch.Text);
			try
			{
				new icGroup().getGroup(ref this.arrGroupNameWithSpace, ref this.arrGroupID, ref this.arrGroupNO);
				int i = this.arrGroupID.Count;
				for (i = 0; i < this.arrGroupID.Count; i++)
				{
					if (i == 0 && string.IsNullOrEmpty(this.arrGroupNameWithSpace[i].ToString()))
					{
						this.arrGroupName.Add(CommonStr.strAll);
					}
					else
					{
						this.arrGroupName.Add(this.arrGroupNameWithSpace[i].ToString());
					}
					this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
					this.cbof_GroupNew.Items.Add(this.arrGroupNameWithSpace[i].ToString());
				}
				if ((int)this.arrGroupID[0] == 0)
				{
					this.cbof_GroupID.Items.Insert(1, wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartmentIsEmpty));
					this.bInsertNullDepartment = true;
				}
				if (this.cbof_GroupID.Items.Count > 0)
				{
					this.cbof_GroupID.SelectedIndex = 0;
				}
				if (this.cbof_GroupNew.Items.Count > 0)
				{
					this.cbof_GroupNew.SelectedIndex = 0;
				}
				if (!string.IsNullOrEmpty(this.strSqlSelected))
				{
					this.cbof_GroupID.Visible = false;
					this.Label3.Visible = false;
					this.chkIncludeAllBranch.Visible = false;
				}
				if (wgAppConfig.getParamValBoolByNO(113))
				{
					this.chk3.Visible = true;
					this.GroupBox3.Visible = true;
				}
				else
				{
					this.chk3.Visible = false;
					this.GroupBox3.Visible = false;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			wgAppConfig.setDisplayFormatDate(this.dtpBegin, wgTools.DisplayFormat_DateYMD);
			wgAppConfig.setDisplayFormatDate(this.dtpEnd, wgTools.DisplayFormat_DateYMD);
			if (icConsumer.gTimeSecondEnabled)
			{
				this.lblActivateTime.Visible = true;
				this.dateBeginHMS1.Visible = true;
				this.dateEndHMS1.Format = DateTimePickerFormat.Time;
				this.dateEndHMS1.Value = DateTime.Parse("2099-12-31 23:59:59");
				return;
			}
			this.dateEndHMS1.CustomFormat = "HH:mm";
			this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
			if (this.dateEndHMS1.Value.ToString("HH:mm").Equals("00:00"))
			{
				this.dateEndHMS1.Value = DateTime.Parse(this.dateEndHMS1.Value.ToString("yyyy-MM-dd 23:59:59"));
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000709BC File Offset: 0x0006F9BC
		private void funcCtrlShiftQ()
		{
			this.chk6.Visible = true;
			this.txtf_PIN.Visible = true;
		}

		// Token: 0x0400077E RID: 1918
		private bool bInsertNullDepartment;

		// Token: 0x0400077F RID: 1919
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04000780 RID: 1920
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04000781 RID: 1921
		private ArrayList arrGroupNameWithSpace = new ArrayList();

		// Token: 0x04000782 RID: 1922
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04000783 RID: 1923
		public bool bNewDate;

		// Token: 0x04000784 RID: 1924
		public DateTime deactivateDateNew = DateTime.Now;

		// Token: 0x04000785 RID: 1925
		public int groupIdUpdated = -1;

		// Token: 0x04000786 RID: 1926
		public string strSqlSelected = "";
	}
}
