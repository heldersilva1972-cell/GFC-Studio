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

namespace WG3000_COMM.ExtendFunc.Elevator
{
	// Token: 0x0200024A RID: 586
	public partial class dfrmFloors : frmN3000
	{
		// Token: 0x06001267 RID: 4711 RVA: 0x00162B80 File Offset: 0x00161B80
		public dfrmFloors()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvFloorList);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00162BBC File Offset: 0x00161BBC
		private void addTaskListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.dgvFloorList.SelectedRows.Count > 0)
				{
					string text = "";
					string text2 = "";
					for (int i = 0; i < this.dgvFloorList.SelectedRows.Count; i++)
					{
						if (i == 0)
						{
							text = this.dgvFloorList.SelectedRows[i].Cells[0].Value.ToString();
							text2 = this.dgvFloorList.SelectedRows[i].Cells["f_floorFullName"].Value.ToString().Replace(",", "");
						}
						else
						{
							text = this.dgvFloorList.SelectedRows[i].Cells[0].Value.ToString() + "," + text;
							text2 = this.dgvFloorList.SelectedRows[i].Cells["f_floorFullName"].Value.ToString().Replace(",", "") + ", " + text2;
						}
					}
					using (dfrmControllerTask4Floor dfrmControllerTask4Floor = new dfrmControllerTask4Floor())
					{
						dfrmControllerTask4Floor.txtFloors.Text = text2;
						dfrmControllerTask4Floor.txtTaskIDs.Text = "";
						dfrmControllerTask4Floor.txtNote.Text = text;
						dfrmControllerTask4Floor.ShowDialog();
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00162D70 File Offset: 0x00161D70
		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (this.cboElevator.SelectedIndex >= 0 && this.cboFloorNO.SelectedIndex >= 0 && !string.IsNullOrEmpty(this.textBox1.Text))
			{
				this.textBox1.Text = this.textBox1.Text.Trim();
				this.dvFloorList.RowFilter = "f_floorName = " + wgTools.PrepareStr(this.textBox1.Text) + " AND f_DoorName = " + wgTools.PrepareStr(this.cboElevator.Text);
				if (this.dvFloorList.Count > 0)
				{
					XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				string text = " INSERT INTO t_b_floor(";
				if (wgAppConfig.runUpdateSql(string.Concat(new object[]
				{
					text,
					"  f_floorName, f_DoorID, f_ControllerID, f_floorNO )  VALUES ( ",
					wgTools.PrepareStrNUnicode(this.textBox1.Text.Trim()),
					" , ",
					this.arrDoorID[this.cboElevator.SelectedIndex],
					" , ",
					this.arrControllerID[this.cboElevator.SelectedIndex],
					" , ",
					this.cboFloorNO.Text,
					") "
				})) > 0)
				{
					this.LoadFloorData();
					try
					{
						this.textBox1.Text = ((this.cboFloorNO.Text.Length == 1) ? "_" : "") + this.cboFloorNO.Text + this.newFloorNameShort;
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
					this.textBox1.Focus();
					this.textBox1.SelectAll();
				}
			}
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00162F50 File Offset: 0x00161F50
		private void btnChange_Click(object sender, EventArgs e)
		{
			int num;
			if (this.dgvFloorList.SelectedRows.Count <= 0)
			{
				if (this.dgvFloorList.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dgvFloorList.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvFloorList.SelectedRows[0].Index;
			}
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as Button).Text + ":  " + this.dgvFloorList.Rows[num].Cells[1].Value.ToString();
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					string text = dfrmInputNewName.strNewName.Trim();
					if (!string.IsNullOrEmpty(text) && text != this.dgvFloorList.Rows[num].Cells[1].Value.ToString())
					{
						this.dvFloorList.RowFilter = "f_floorName = " + wgTools.PrepareStr(text) + " AND f_DoorName = " + wgTools.PrepareStr(this.dgvFloorList.Rows[num].Cells["f_DoorName"].Value.ToString());
						if (this.dvFloorList.Count > 0)
						{
							XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						else
						{
							wgAppConfig.runUpdateSql(string.Format(" UPDATE t_b_floor SET f_floorName={0}  WHERE [f_floorId]={1} ", wgTools.PrepareStrNUnicode(text), this.dgvFloorList.Rows[num].Cells[0].Value.ToString()));
							this.LoadFloorData();
						}
					}
				}
			}
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00163130 File Offset: 0x00162130
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00163138 File Offset: 0x00162138
		private void btnDel_Click(object sender, EventArgs e)
		{
			int num;
			if (this.dgvFloorList.SelectedRows.Count <= 0)
			{
				if (this.dgvFloorList.SelectedCells.Count <= 0)
				{
					return;
				}
				num = this.dgvFloorList.SelectedCells[0].RowIndex;
			}
			else
			{
				num = this.dgvFloorList.SelectedRows[0].Index;
			}
			string text = string.Format("{0}\r\n{1}:  {2}", this.btnDel.Text, this.dgvFloorList.Columns[1].HeaderText, this.dgvFloorList.Rows[num].Cells[1].Value.ToString());
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				wgAppConfig.runUpdateSql(" DELETE FROM t_b_floor WHERE [f_floorId]= " + this.dgvFloorList.Rows[num].Cells[0].Value.ToString());
				this.LoadFloorData();
			}
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00163250 File Offset: 0x00162250
		private void btnRemoteControl_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvFloorList.SelectedRows.Count <= 0)
				{
					if (this.dgvFloorList.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvFloorList.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvFloorList.SelectedRows[0].Index;
				}
				using (icController icController = new icController())
				{
					icController.GetInfoFromDBByDoorName(this.dgvFloorList.Rows[num].Cells["f_DoorName"].Value.ToString());
					if (icController.RemoteOpenFoorIP(int.Parse(this.dgvFloorList.Rows[num].Cells["f_floorNO"].Value.ToString()), (uint)icOperator.OperatorID, -1L) > 0)
					{
						string text = string.Concat(new string[]
						{
							this.btnRemoteControl.Text,
							" ",
							this.dgvFloorList.Rows[num].Cells["f_floorName"].Value.ToString(),
							" ",
							CommonStr.strSuccessfully
						});
						wgAppConfig.wgLog(text);
						XMessageBox.Show(text);
					}
					else
					{
						string text = string.Concat(new string[]
						{
							this.btnRemoteControl.Text,
							"  ",
							this.dgvFloorList.Rows[num].Cells["f_floorName"].Value.ToString(),
							" ",
							CommonStr.strFailed
						});
						wgAppConfig.wgLog(text);
						XMessageBox.Show(this, text, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00163474 File Offset: 0x00162474
		private void btnRemoteControlNC_Click(object sender, EventArgs e)
		{
			try
			{
				int num;
				if (this.dgvFloorList.SelectedRows.Count <= 0)
				{
					if (this.dgvFloorList.SelectedCells.Count <= 0)
					{
						return;
					}
					num = this.dgvFloorList.SelectedCells[0].RowIndex;
				}
				else
				{
					num = this.dgvFloorList.SelectedRows[0].Index;
				}
				using (icController icController = new icController())
				{
					icController.GetInfoFromDBByDoorName(this.dgvFloorList.Rows[num].Cells["f_DoorName"].Value.ToString());
					if (icController.RemoteOpenFoorIP(int.Parse(this.dgvFloorList.Rows[num].Cells["f_floorNO"].Value.ToString()) + 40, (uint)icOperator.OperatorID, -1L) > 0)
					{
						string text = string.Concat(new string[]
						{
							this.btnRemoteControlNC.Text,
							" ",
							this.dgvFloorList.Rows[num].Cells["f_floorName"].Value.ToString(),
							" ",
							CommonStr.strSuccessfully
						});
						wgAppConfig.wgLog(text);
						XMessageBox.Show(text);
					}
					else
					{
						string text = string.Concat(new string[]
						{
							this.btnRemoteControlNC.Text,
							"  ",
							this.dgvFloorList.Rows[num].Cells["f_floorName"].Value.ToString(),
							" ",
							CommonStr.strFailed
						});
						wgAppConfig.wgLog(text);
						XMessageBox.Show(this, text, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0016369C File Offset: 0x0016269C
		private void cboElevator_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.updateOptionFloors();
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x001636A4 File Offset: 0x001626A4
		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x001636A8 File Offset: 0x001626A8
		private void dfrmControllerTaskList_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			string text = CommonStr.strFloor;
			this.newFloorNameShort = CommonStr.strFloorShort;
			string text2 = "";
			if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(144)) & 255) == 2)
			{
				text = CommonStr.strFloor2;
				text2 = CommonStr.strFloorController2;
				this.newFloorNameShort = CommonStr.strFloorShort2;
			}
			else if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(144)) & 255) == 3)
			{
				text = CommonStr.strFloor3;
				text2 = CommonStr.strFloorController3;
				this.newFloorNameShort = CommonStr.strFloorShort3;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				this.label1.Text = this.label1.Text.Replace(CommonStr.strFloor, text);
				this.label3.Text = this.label3.Text.Replace(CommonStr.strFloor, text);
				this.btnChange.Text = this.btnChange.Text.Replace(CommonStr.strFloor, text);
				this.f_floorFullName.HeaderText = this.f_floorFullName.HeaderText.Replace(CommonStr.strFloor, text);
				this.f_floorNO.HeaderText = this.f_floorNO.HeaderText.Replace(CommonStr.strFloor, text);
				this.label2.Text = this.label2.Text.Replace(CommonStr.strFloorController, text2);
				this.f_DoorName.HeaderText = this.f_DoorName.HeaderText.Replace(CommonStr.strFloorController, text2);
			}
			this.loadDoorData();
			this.LoadFloorData();
			if (this.cboFloorNO.Items.Count > 0)
			{
				this.cboFloorNO.SelectedIndex = 0;
			}
			try
			{
				this.textBox1.Text = ((this.cboFloorNO.Text.Length == 1) ? "_" : "") + this.cboFloorNO.Text + this.newFloorNameShort;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			bool flag = false;
			string text3 = "mnuElevator";
			if (icOperator.OperatePrivilegeVisible(text3, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnChange.Visible = false;
				this.btnDel.Visible = false;
				this.cboElevator.Enabled = false;
				this.cboFloorNO.Enabled = false;
				this.textBox1.Enabled = false;
			}
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00163930 File Offset: 0x00162930
		private void dfrmFloors_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x00163948 File Offset: 0x00162948
		private void dfrmFloors_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyValue == 81 && e.Control)
				{
					this.btnRemoteControlNC.Visible = true;
				}
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

		// Token: 0x06001274 RID: 4724 RVA: 0x001639D4 File Offset: 0x001629D4
		private void loadDoorData()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID,a.f_ControllerID ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  AND b.f_ControllerSN >= 170000000 AND b.f_ControllerSN <= 179999999  ORDER BY  a.f_DoorName ";
			this.dtDoors = new DataTable();
			this.dvDoors = new DataView(this.dtDoors);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtDoors);
						}
					}
					goto IL_00D7;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtDoors);
					}
				}
			}
			IL_00D7:
			int count = this.dtDoors.Rows.Count;
			new icControllerZone().getAllowedControllers(ref this.dtDoors);
			this.cboElevator.Items.Clear();
			if (this.dvDoors.Count > 0)
			{
				for (int i = 0; i < this.dvDoors.Count; i++)
				{
					this.cboElevator.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
					this.arrDoorID.Add(this.dvDoors[i]["f_DoorID"]);
					this.arrControllerID.Add(this.dvDoors[i]["f_ControllerID"]);
				}
			}
			if (this.cboElevator.Items.Count > 0)
			{
				this.cboElevator.SelectedIndex = 0;
				this.textBox1.Focus();
			}
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00163C04 File Offset: 0x00162C04
		private void LoadFloorData()
		{
			string text = "  SELECT t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
			text += "   t_b_Door.f_DoorName,    t_b_Floor.f_floorNO, t_b_Controller.f_ZoneID, t_b_Floor.f_floorName  FROM (t_b_Floor LEFT JOIN t_b_Door ON t_b_Floor.f_DoorID = t_b_Door.f_DoorID) LEFT JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID ORDER BY  (  t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName ) ";
			this.dt = new DataTable();
			this.dv = new DataView(this.dt);
			this.dvFloorList = new DataView(this.dt);
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
					goto IL_00EC;
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
			IL_00EC:
			icControllerZone icControllerZone = new icControllerZone();
			icControllerZone.getAllowedControllers(ref this.dt);
			this.dgvFloorList.AutoGenerateColumns = false;
			this.dgvFloorList.DataSource = this.dv;
			this.updateOptionFloors();
			int num = 0;
			while (num < this.dv.Table.Columns.Count && num < this.dgvFloorList.ColumnCount)
			{
				this.dgvFloorList.Columns[num].DataPropertyName = this.dv.Table.Columns[num].ColumnName;
				this.dgvFloorList.Columns[num].Name = this.dv.Table.Columns[num].ColumnName;
				num++;
			}
			if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(144)) & 255) == 2)
			{
				this.dv.Sort = "f_ZoneID ASC, f_floorName ASC, f_DoorName ASC";
			}
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00163E50 File Offset: 0x00162E50
		private void queryTaskListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmControllerTaskList4Floor dfrmControllerTaskList4Floor = new dfrmControllerTaskList4Floor())
			{
				dfrmControllerTaskList4Floor.ShowDialog(this);
			}
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00163E88 File Offset: 0x00162E88
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.textBox1.Text))
			{
				this.btnAdd.Enabled = false;
				return;
			}
			this.btnAdd.Enabled = true;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00163EB8 File Offset: 0x00162EB8
		private void updateOptionFloors()
		{
			if (this.dvFloorList != null)
			{
				if (this.cboElevator.SelectedIndex < 0)
				{
					this.cboFloorNO.Items.Clear();
					return;
				}
				this.dvFloorList.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboElevator.Text);
				this.cboFloorNO.Items.Clear();
				for (int i = 1; i <= 40; i++)
				{
					this.dvFloorList.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboElevator.Text) + "AND f_floorNO = " + i.ToString();
					if (this.dvFloorList.Count == 0)
					{
						this.cboFloorNO.Items.Add(i.ToString());
					}
				}
				if (this.cboFloorNO.Items.Count > 0)
				{
					this.cboFloorNO.SelectedIndex = 0;
				}
				try
				{
					this.textBox1.Text = ((this.cboFloorNO.Text.Length == 1) ? "_" : "") + this.cboFloorNO.Text + this.newFloorNameShort;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x040021B5 RID: 8629
		private DataTable dt;

		// Token: 0x040021B6 RID: 8630
		private DataTable dtDoors;

		// Token: 0x040021B7 RID: 8631
		private DataView dv;

		// Token: 0x040021B8 RID: 8632
		private DataView dvDoors;

		// Token: 0x040021B9 RID: 8633
		private DataView dvFloorList;

		// Token: 0x040021BA RID: 8634
		private ArrayList arrControllerID = new ArrayList();

		// Token: 0x040021BB RID: 8635
		private ArrayList arrDoorID = new ArrayList();

		// Token: 0x040021BC RID: 8636
		private string newFloorNameShort = "";
	}
}
