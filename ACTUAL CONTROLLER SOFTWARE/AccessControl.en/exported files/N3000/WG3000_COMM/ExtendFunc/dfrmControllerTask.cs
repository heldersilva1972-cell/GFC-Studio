using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x0200023C RID: 572
	public partial class dfrmControllerTask : frmN3000
	{
		// Token: 0x0600115C RID: 4444 RVA: 0x0014143C File Offset: 0x0014043C
		public dfrmControllerTask()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00141455 File Offset: 0x00140455
		private void chk1_CheckedChanged(object sender, EventArgs e)
		{
			this.groupBox1.Enabled = !this.chk1.Visible || this.chk1.Checked;
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x0014147D File Offset: 0x0014047D
		private void chk2_CheckedChanged(object sender, EventArgs e)
		{
			this.groupBox2.Enabled = !this.chk2.Visible || this.chk2.Checked;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x001414A5 File Offset: 0x001404A5
		private void chk3_CheckedChanged(object sender, EventArgs e)
		{
			this.groupBox3.Enabled = !this.chk3.Visible || this.chk3.Checked;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x001414CD File Offset: 0x001404CD
		private void chk4_CheckedChanged(object sender, EventArgs e)
		{
			this.groupBox4.Enabled = !this.chk4.Visible || this.chk4.Checked;
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x001414F5 File Offset: 0x001404F5
		private void chk5_CheckedChanged(object sender, EventArgs e)
		{
			this.groupBox5.Enabled = !this.chk5.Visible || this.chk5.Checked;
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0014151D File Offset: 0x0014051D
		private void chk6_CheckedChanged(object sender, EventArgs e)
		{
			this.groupBox6.Enabled = !this.chk6.Visible || this.chk6.Checked;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00141545 File Offset: 0x00140545
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00141554 File Offset: 0x00140554
		private void cmdOK_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.groupBox1.Enabled && this.dtpBegin.Value.Date > this.dtpEnd.Value.Date)
				{
					XMessageBox.Show(CommonStr.strTimeInvalidParm, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else if (this.groupBox3.Enabled && !this.checkBox43.Checked && !this.checkBox44.Checked && !this.checkBox45.Checked && !this.checkBox46.Checked && !this.checkBox47.Checked && !this.checkBox48.Checked && !this.checkBox49.Checked)
				{
					XMessageBox.Show(CommonStr.strTimeInvalidParm, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else if (this.groupBox4.Enabled && this.cboDoors.SelectedIndex < 0)
				{
					XMessageBox.Show(this.label2.Text + "...?", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					if (this.txtTaskIDs.Text.IndexOf("(") < 0)
					{
						string text = " UPDATE t_b_ControllerTaskList SET ";
						text = string.Concat(new object[]
						{
							text,
							" f_BeginYMD =",
							this.getDateString(this.dtpBegin),
							" , f_EndYMD =",
							this.getDateString(this.dtpEnd),
							" , f_OperateTime = ",
							this.getDateString(this.dtpTime),
							" , f_Monday =",
							this.checkBox43.Checked ? "1" : "0",
							" , f_Tuesday = ",
							this.checkBox44.Checked ? "1" : "0",
							" , f_Wednesday = ",
							this.checkBox45.Checked ? "1" : "0",
							" , f_Thursday = ",
							this.checkBox46.Checked ? "1" : "0",
							" , f_Friday = ",
							this.checkBox47.Checked ? "1" : "0",
							" , f_Saturday = ",
							this.checkBox48.Checked ? "1" : "0",
							" , f_Sunday = ",
							this.checkBox49.Checked ? "1" : "0",
							" , f_DoorID = ",
							this.arrDoorID[this.cboDoors.SelectedIndex]
						});
						if (this.cboAccessMethod.SelectedIndex < 0)
						{
							this.cboAccessMethod.SelectedIndex = 0;
						}
						text = string.Concat(new object[]
						{
							text,
							" , f_DoorControl = ",
							this.cboAccessMethod.SelectedIndex,
							" , f_Notes = ",
							wgTools.PrepareStrNUnicode(this.txtNote.Text.Trim()),
							" WHERE [f_Id]= ",
							this.txtTaskIDs.Text
						});
						if (wgAppConfig.runUpdateSql(text) <= 0)
						{
							return;
						}
						wgAppConfig.wgLog(string.Format("{0} {1}:{2} [{3}]", new object[]
						{
							this.Text,
							this.lblTaskID.Text,
							this.txtTaskIDs.Text,
							text
						}));
					}
					else
					{
						string text2 = "";
						string text3 = " UPDATE t_b_ControllerTaskList SET ";
						text2 += ((!this.chk1.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_BeginYMD =" + this.getDateString(this.dtpBegin)));
						text2 += ((!this.chk1.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_EndYMD =" + this.getDateString(this.dtpEnd)));
						text2 += ((!this.chk2.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_OperateTime = " + this.getDateString(this.dtpTime)));
						text2 += ((!this.chk3.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_Monday =" + (this.checkBox43.Checked ? "1" : "0")));
						text2 += ((!this.chk3.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_Tuesday = " + (this.checkBox44.Checked ? "1" : "0")));
						text2 += ((!this.chk3.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_Wednesday = " + (this.checkBox45.Checked ? "1" : "0")));
						text2 += ((!this.chk3.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_Thursday = " + (this.checkBox46.Checked ? "1" : "0")));
						text2 += ((!this.chk3.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_Friday = " + (this.checkBox47.Checked ? "1" : "0")));
						text2 += ((!this.chk3.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_Saturday = " + (this.checkBox48.Checked ? "1" : "0")));
						text2 += ((!this.chk3.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_Sunday = " + (this.checkBox49.Checked ? "1" : "0")));
						text2 += ((!this.chk4.Checked) ? "" : (((text2 == "") ? " " : " , ") + " f_DoorID = " + this.arrDoorID[this.cboDoors.SelectedIndex]));
						if (this.cboAccessMethod.SelectedIndex < 0)
						{
							this.cboAccessMethod.SelectedIndex = 0;
						}
						text2 += ((!this.chk5.Checked) ? "" : (((text2 == "") ? " " : " , ") + "  f_DoorControl = " + this.cboAccessMethod.SelectedIndex));
						text2 += ((!this.chk6.Checked) ? "" : (((text2 == "") ? " " : " , ") + "  f_Notes = " + wgTools.PrepareStrNUnicode(this.txtNote.Text.Trim())));
						if (text2 != "")
						{
							text3 = text3 + text2 + " WHERE [f_Id] IN " + this.txtTaskIDs.Text;
							if (wgAppConfig.runUpdateSql(text3) <= 0)
							{
								return;
							}
							wgAppConfig.wgLog(string.Format("{0} {1}:{2} [{3}]", new object[]
							{
								this.Text,
								this.lblTaskID.Text,
								this.txtTaskIDs.Text,
								text3
							}));
						}
					}
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00141E20 File Offset: 0x00140E20
		private void dfrmControllerTask_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.dfrmControllerTask_Load_Acc(sender, e);
				return;
			}
			this.dtpBegin.Value = DateTime.Now.Date;
			this.dtpTime.CustomFormat = "HH:mm";
			this.dtpTime.Format = DateTimePickerFormat.Custom;
			this.dtpTime.Value = DateTime.Parse("00:00:00");
			this.loadDoorData();
			if (!wgAppConfig.IsAccessControlBlue && this.cboAccessMethod.Items.Count < 14)
			{
				this.cboAccessMethod.Items.Add(CommonStr.strTaskList13_AlarmDisable);
				this.cboAccessMethod.Items.Add(CommonStr.strTaskList14_AlarmEnable);
			}
			if (this.cboAccessMethod.Items.Count > 0)
			{
				this.cboAccessMethod.SelectedIndex = 0;
			}
			if (!string.IsNullOrEmpty(this.txtTaskIDs.Text))
			{
				if (this.txtTaskIDs.Text.IndexOf("(") < 0)
				{
					this.groupBox6.Visible = true;
					string text = " SELECT * FROM t_b_ControllerTaskList WHERE [f_Id]= " + this.txtTaskIDs.Text;
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
						{
							sqlConnection.Open();
							SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
							if (sqlDataReader.Read())
							{
								this.dtpBegin.Value = wgTools.wgDateTimeParse(sqlDataReader["f_BeginYMD"]);
								this.dtpEnd.Value = wgTools.wgDateTimeParse(sqlDataReader["f_EndYMD"]);
								this.dtpTime.Value = wgTools.wgDateTimeParse(sqlDataReader["f_OperateTime"]);
								this.checkBox43.Checked = wgTools.SetObjToStr(sqlDataReader["f_Monday"]) == "1";
								this.checkBox44.Checked = wgTools.SetObjToStr(sqlDataReader["f_Tuesday"]) == "1";
								this.checkBox45.Checked = wgTools.SetObjToStr(sqlDataReader["f_Wednesday"]) == "1";
								this.checkBox46.Checked = wgTools.SetObjToStr(sqlDataReader["f_Thursday"]) == "1";
								this.checkBox47.Checked = wgTools.SetObjToStr(sqlDataReader["f_Friday"]) == "1";
								this.checkBox48.Checked = wgTools.SetObjToStr(sqlDataReader["f_Saturday"]) == "1";
								this.checkBox49.Checked = wgTools.SetObjToStr(sqlDataReader["f_Sunday"]) == "1";
								this.cboDoors.SelectedIndex = this.arrDoorID.IndexOf(int.Parse(wgTools.SetObjToStr(sqlDataReader["f_DoorID"])));
								this.cboAccessMethod.SelectedIndex = int.Parse(wgTools.SetObjToStr(sqlDataReader["f_DoorControl"]));
								this.txtNote.Text = wgTools.SetObjToStr(sqlDataReader["f_Notes"]);
							}
							sqlDataReader.Close();
						}
						return;
					}
				}
				this.chk1.Visible = true;
				this.chk2.Visible = true;
				this.chk3.Visible = true;
				this.chk4.Visible = true;
				this.chk5.Visible = true;
				this.chk6.Visible = true;
				this.groupBox1.Enabled = false;
				this.groupBox2.Enabled = false;
				this.groupBox3.Enabled = false;
				this.groupBox4.Enabled = false;
				this.groupBox5.Enabled = false;
				this.groupBox6.Enabled = false;
			}
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00142214 File Offset: 0x00141214
		private void dfrmControllerTask_Load_Acc(object sender, EventArgs e)
		{
			bool isAccessDB = wgAppConfig.IsAccessDB;
			this.dtpBegin.Value = DateTime.Now.Date;
			this.dtpTime.CustomFormat = "HH:mm";
			this.dtpTime.Format = DateTimePickerFormat.Custom;
			this.dtpTime.Value = DateTime.Parse("00:00:00");
			this.loadDoorData();
			if (!wgAppConfig.IsAccessControlBlue && this.cboAccessMethod.Items.Count < 14)
			{
				this.cboAccessMethod.Items.Add(CommonStr.strTaskList13_AlarmDisable);
				this.cboAccessMethod.Items.Add(CommonStr.strTaskList14_AlarmEnable);
			}
			if (this.cboAccessMethod.Items.Count > 0)
			{
				this.cboAccessMethod.SelectedIndex = 0;
			}
			if (!string.IsNullOrEmpty(this.txtTaskIDs.Text))
			{
				if (this.txtTaskIDs.Text.IndexOf("(") < 0)
				{
					this.groupBox6.Visible = true;
					string text = " SELECT * FROM t_b_ControllerTaskList WHERE [f_Id]= " + this.txtTaskIDs.Text;
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							oleDbConnection.Open();
							OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
							if (oleDbDataReader.Read())
							{
								this.dtpBegin.Value = wgTools.wgDateTimeParse(oleDbDataReader["f_BeginYMD"]);
								this.dtpEnd.Value = wgTools.wgDateTimeParse(oleDbDataReader["f_EndYMD"]);
								this.dtpTime.Value = wgTools.wgDateTimeParse(oleDbDataReader["f_OperateTime"]);
								this.checkBox43.Checked = wgTools.SetObjToStr(oleDbDataReader["f_Monday"]) == "1";
								this.checkBox44.Checked = wgTools.SetObjToStr(oleDbDataReader["f_Tuesday"]) == "1";
								this.checkBox45.Checked = wgTools.SetObjToStr(oleDbDataReader["f_Wednesday"]) == "1";
								this.checkBox46.Checked = wgTools.SetObjToStr(oleDbDataReader["f_Thursday"]) == "1";
								this.checkBox47.Checked = wgTools.SetObjToStr(oleDbDataReader["f_Friday"]) == "1";
								this.checkBox48.Checked = wgTools.SetObjToStr(oleDbDataReader["f_Saturday"]) == "1";
								this.checkBox49.Checked = wgTools.SetObjToStr(oleDbDataReader["f_Sunday"]) == "1";
								this.cboDoors.SelectedIndex = this.arrDoorID.IndexOf(int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_DoorID"])));
								this.cboAccessMethod.SelectedIndex = int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_DoorControl"]));
								this.txtNote.Text = wgTools.SetObjToStr(oleDbDataReader["f_Notes"]);
							}
							oleDbDataReader.Close();
						}
						return;
					}
				}
				this.chk1.Visible = true;
				this.chk2.Visible = true;
				this.chk3.Visible = true;
				this.chk4.Visible = true;
				this.chk5.Visible = true;
				this.chk6.Visible = true;
				this.groupBox1.Enabled = false;
				this.groupBox2.Enabled = false;
				this.groupBox3.Enabled = false;
				this.groupBox4.Enabled = false;
				this.groupBox5.Enabled = false;
				this.groupBox6.Enabled = false;
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00142600 File Offset: 0x00141600
		private string getDateString(DateTimePicker dtp)
		{
			if (dtp == null)
			{
				return "NULL";
			}
			return wgTools.PrepareStr(dtp.Value.ToString(wgTools.YMDHMSFormat), true, wgTools.YMDHMSFormat);
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00142634 File Offset: 0x00141634
		private void loadDoorData()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
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
					goto IL_00DB;
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
			IL_00DB:
			int count = this.dtDoors.Rows.Count;
			new icControllerZone().getAllowedControllers(ref this.dtDoors);
			this.cboDoors.Items.Clear();
			if (count == this.dtDoors.Rows.Count)
			{
				this.cboDoors.Items.Add(CommonStr.strAll);
				this.arrDoorID.Add(0);
			}
			if (this.dvDoors.Count > 0)
			{
				for (int i = 0; i < this.dvDoors.Count; i++)
				{
					this.cboDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
					this.arrDoorID.Add(this.dvDoors[i]["f_DoorID"]);
				}
			}
			if (this.cboDoors.Items.Count > 0)
			{
				this.cboDoors.SelectedIndex = 0;
			}
		}

		// Token: 0x04001EFA RID: 7930
		private DataTable dtDoors;

		// Token: 0x04001EFB RID: 7931
		private DataView dvDoors;

		// Token: 0x04001EFC RID: 7932
		private ArrayList arrDoorID = new ArrayList();
	}
}
