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

namespace WG3000_COMM.ExtendFunc.GlobalAntiBack2015
{
	// Token: 0x020002F0 RID: 752
	public partial class dfrmGlobalAntiBackManagement : frmN3000
	{
		// Token: 0x060015AF RID: 5551 RVA: 0x001B40E0 File Offset: 0x001B30E0
		public dfrmGlobalAntiBackManagement()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x001B4174 File Offset: 0x001B3174
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
					}
				}
			}
			else
			{
				this.dvtmp = new DataView((this.dgvDoors.DataSource as DataView).Table);
				this.dvtmp.RowFilter = string.Format("  {0} ", this.strZoneFilter);
				for (int j = 0; j < this.dvtmp.Count; j++)
				{
					this.dvtmp[j]["f_Selected"] = 1;
				}
			}
			this.updateCount();
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x001B4288 File Offset: 0x001B3288
		private void btnAddOneDoor_Click(object sender, EventArgs e)
		{
			this.bNeedAdd = true;
			this.selectObject(this.dgvDoors);
			this.updateCount();
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x001B42A4 File Offset: 0x001B32A4
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

		// Token: 0x060015B3 RID: 5555 RVA: 0x001B4306 File Offset: 0x001B3306
		private void btnDelOneDoor_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.bNeedDelete = true;
			this.updateCount();
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x001B4320 File Offset: 0x001B3320
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x001B4328 File Offset: 0x001B3328
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (this.bNeedAdd || this.bNeedDelete)
			{
				string text = "";
				DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if ((int)table.Rows[i]["f_Selected"] == 1)
					{
						if (!string.IsNullOrEmpty(text))
						{
							text += ",";
						}
						text += ((int)table.Rows[i]["f_DoorID"]).ToString();
					}
				}
				string text2 = "";
				string text3 = "";
				string text4 = "";
				string text5 = "";
				wgAppConfig.getSystemParamValue(183, out text2, out text3, out text4, out text5);
				wgAppConfig.setSystemParamValueWithNotes(183, "AntiBack_Door4Exit", "", text);
				wgAppConfig.wgLog(string.Format("{0}   {1} => {2}", this.Text, wgTools.SetObjToStr(text4), wgTools.SetObjToStr(text)));
			}
			wgAppConfig.setSystemParamValue(184, "AntiBack_FirstInThenOut", this.chkFirstInThenOut.Checked ? "1" : "0", "2015-11-16 15:42:01 ");
			this.saveDateHMS();
			if (this.chkActiveAntibackShare.Checked && this.nudTotal.Value > 0m)
			{
				wgAppConfig.setSystemParamValue(213, "AntiBack_LimitedPersons", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this.nudTotal.Value.ToString());
			}
			else
			{
				wgAppConfig.setSystemParamValue(213, "AntiBack_LimitedPersons", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "0");
			}
			base.DialogResult = DialogResult.OK;
			wgAppConfig.wgLog(this.Text + " " + CommonStr.strSuccessfully);
			XMessageBox.Show(this.Text + " " + CommonStr.strSuccessfully);
			base.Close();
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x001B453C File Offset: 0x001B353C
		private void btnRestartAntiBack_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(CommonStr.strGlobalAntiPassbackGetRecordsFirst2015) == DialogResult.OK && XMessageBox.Show(CommonStr.strGlobalAntiPassbackAreYouSure2015, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				string text = "DELETE  FROM [t_b_Consumer_Location] ";
				wgAppConfig.runSql(text);
				wgAppConfig.setSystemParamValue(182, "AntiBack_LastQueryRecID", "0", "2015-11-13 22:49:07");
				wgAppConfig.wgLog(this.btnRestartAntiBack.Text + "  " + CommonStr.strSuccessfully);
			}
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x001B45B0 File Offset: 0x001B35B0
		private void cbof_Zone_SelectedIndexChanged(object sender, EventArgs e)
		{
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

		// Token: 0x060015B8 RID: 5560 RVA: 0x001B4764 File Offset: 0x001B3764
		private void cbof_ZoneID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ZoneID);
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x001B4771 File Offset: 0x001B3771
		private void chkActiveAntibackShare_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkActiveAntibackShare.Checked)
			{
				this.nudTotal.Visible = true;
				return;
			}
			this.nudTotal.Visible = false;
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x001B4799 File Offset: 0x001B3799
		private void chkActiveTimeSegments_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkActiveTimeSegments.Checked)
			{
				this.groupBoxIn.Enabled = true;
				this.groupBoxOut.Enabled = true;
				return;
			}
			this.groupBoxIn.Enabled = false;
			this.groupBoxOut.Enabled = false;
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x001B47D9 File Offset: 0x001B37D9
		private void dfrmGlobalAntiBackManagement_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				this.chkFirstInThenOut.Visible = true;
			}
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x001B4804 File Offset: 0x001B3804
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

		// Token: 0x060015BD RID: 5565 RVA: 0x001B483C File Offset: 0x001B383C
		private void dfrmPrivilegeTypeDoors_Load(object sender, EventArgs e)
		{
			try
			{
				this.loadZoneInfo();
				this.loadDoorData();
				this.mnuDoorTypeSelect_loadData();
				this.updateCount();
				this.chkFirstInThenOut.Checked = wgAppConfig.getParamValBoolByNO(184);
				this.chkFirstInThenOut.Visible = wgAppConfig.getParamValBoolByNO(184);
				for (int i = 1; i <= 6; i++)
				{
					for (int j = 0; j < this.arrHMStype.Length; j++)
					{
						Control[] array = base.Controls.Find(this.arrHMStype[j] + i.ToString(), true);
						if (array.Length > 0)
						{
							foreach (Control control in array)
							{
								DateTimePicker dateTimePicker = (DateTimePicker)control;
								dateTimePicker.CustomFormat = "HH:mm";
								dateTimePicker.Format = DateTimePickerFormat.Custom;
								dateTimePicker.Value = DateTime.Parse("00:00:00");
							}
						}
					}
				}
				this.dateEndHMS1.CustomFormat = "HH:mm";
				this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
				this.dateEndHMS1.Value = DateTime.Parse("23:59:59");
				this.dateEndHMSExit1.CustomFormat = "HH:mm";
				this.dateEndHMSExit1.Format = DateTimePickerFormat.Custom;
				this.dateEndHMSExit1.Value = DateTime.Parse("23:59:59");
				this.loadDateHMS();
				int num = int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamNotes(213)));
				if (num > 0)
				{
					this.chkActiveAntibackShare.Checked = true;
					this.nudTotal.Visible = true;
					this.nudTotal.Value = num;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x001B4A14 File Offset: 0x001B3A14
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnAddOneDoor.PerformClick();
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x001B4A21 File Offset: 0x001B3A21
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneDoor.PerformClick();
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x001B4A30 File Offset: 0x001B3A30
		private void loadDateHMS()
		{
			string systemParamNotes = wgAppConfig.getSystemParamNotes(212);
			if (!string.IsNullOrEmpty(systemParamNotes))
			{
				this.chkActiveTimeSegments.Checked = true;
				this.groupBoxIn.Enabled = true;
				this.groupBoxOut.Enabled = true;
				string[] array = systemParamNotes.Split(new char[] { ',' });
				int num = 0;
				for (int i = 1; i <= 6; i++)
				{
					for (int j = 0; j < this.arrHMStype.Length; j++)
					{
						Control[] array2 = base.Controls.Find(this.arrHMStype[j] + i.ToString(), true);
						if (array2.Length > 0)
						{
							foreach (Control control in array2)
							{
								DateTimePicker dateTimePicker = (DateTimePicker)control;
								dateTimePicker.CustomFormat = "HH:mm";
								dateTimePicker.Format = DateTimePickerFormat.Custom;
								if (num < array.Length)
								{
									dateTimePicker.Value = DateTime.Parse(array[num] + ":00");
									num++;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x001B4B48 File Offset: 0x001B3B48
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

		// Token: 0x060015C2 RID: 5570 RVA: 0x001B4D64 File Offset: 0x001B3D64
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

		// Token: 0x060015C3 RID: 5571 RVA: 0x001B4F70 File Offset: 0x001B3F70
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

		// Token: 0x060015C4 RID: 5572 RVA: 0x001B505C File Offset: 0x001B405C
		private void mnuDoorTypeSelect_loadData()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			wgAppConfig.getSystemParamValue(183, out text, out text2, out text3, out text4);
			string text5 = text3;
			if (!string.IsNullOrEmpty(text5))
			{
				string[] array = text5.Split(new char[] { ',' });
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
			}
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x001B5154 File Offset: 0x001B4154
		private void saveDateHMS()
		{
			if (!this.chkActiveTimeSegments.Checked)
			{
				wgAppConfig.setSystemParamValue(212, "AntiBack_TimeSegment", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), " ");
				return;
			}
			string text = "";
			for (int i = 1; i <= 6; i++)
			{
				for (int j = 0; j < this.arrHMStype.Length; j++)
				{
					Control[] array = base.Controls.Find(this.arrHMStype[j] + i.ToString(), true);
					if (array.Length > 0)
					{
						foreach (Control control in array)
						{
							DateTimePicker dateTimePicker = (DateTimePicker)control;
							if (string.IsNullOrEmpty(text))
							{
								text = dateTimePicker.Value.ToString("HH:mm");
							}
							else
							{
								text = text + "," + dateTimePicker.Value.ToString("HH:mm");
							}
						}
					}
				}
			}
			wgAppConfig.setSystemParamValue(212, "AntiBack_TimeSegment", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text);
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x001B527C File Offset: 0x001B427C
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

		// Token: 0x060015C7 RID: 5575 RVA: 0x001B5400 File Offset: 0x001B4400
		private void updateCount()
		{
			this.lblOptional.Text = this.dgvDoors.RowCount.ToString();
			this.lblSeleted.Text = this.dgvSelectedDoors.RowCount.ToString();
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x001B5449 File Offset: 0x001B4449
		// (set) Token: 0x060015C9 RID: 5577 RVA: 0x001B5451 File Offset: 0x001B4451
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

		// Token: 0x04002CDF RID: 11487
		private bool bNeedAdd;

		// Token: 0x04002CE0 RID: 11488
		private bool bNeedDelete;

		// Token: 0x04002CE1 RID: 11489
		private DataTable dt;

		// Token: 0x04002CE2 RID: 11490
		private DataView dv;

		// Token: 0x04002CE3 RID: 11491
		private DataView dvSelected;

		// Token: 0x04002CE4 RID: 11492
		private DataView dvtmp;

		// Token: 0x04002CE5 RID: 11493
		private int m_privilegeTypeID;

		// Token: 0x04002CE6 RID: 11494
		private string[] arrHMStype = new string[] { "dateBeginHMS", "dateEndHMS", "dateBeginHMSExit", "dateEndHMSExit" };

		// Token: 0x04002CE7 RID: 11495
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x04002CE8 RID: 11496
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x04002CE9 RID: 11497
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x04002CEB RID: 11499
		private string strZoneFilter = "";
	}
}
