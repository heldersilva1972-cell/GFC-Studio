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

namespace WG3000_COMM.Basic
{
	// Token: 0x0200002D RID: 45
	public partial class dfrmSwipeRecordsFindOption : frmN3000
	{
		// Token: 0x06000326 RID: 806 RVA: 0x0005D8B0 File Offset: 0x0005C8B0
		public dfrmSwipeRecordsFindOption()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0005D900 File Offset: 0x0005C900
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Hide();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0005D908 File Offset: 0x0005C908
		private void btnQuery_Click(object sender, EventArgs e)
		{
			if (base.Owner != null)
			{
				(base.Owner as frmSwipeRecords).btnQuery_Click(null, null);
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0005D924 File Offset: 0x0005C924
		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			if (this.chkListDoors.Items.Count > 0)
			{
				for (int i = 0; i < this.chkListDoors.Items.Count; i++)
				{
					this.chkListDoors.SetItemChecked(i, true);
				}
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0005D96C File Offset: 0x0005C96C
		private void btnSelectNone_Click(object sender, EventArgs e)
		{
			if (this.chkListDoors.Items.Count > 0)
			{
				for (int i = 0; i < this.chkListDoors.Items.Count; i++)
				{
					this.chkListDoors.SetItemChecked(i, false);
				}
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0005D9B4 File Offset: 0x0005C9B4
		private void cbof_Zone_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.dvDoors != null)
			{
				this.chkListDoors.Items.Clear();
				DataView dataView = this.dvDoors;
				if (this.cboZone.SelectedIndex < 0 || (this.cboZone.SelectedIndex == 0 && (int)this.arrZoneID[0] == 0))
				{
					dataView.RowFilter = "";
				}
				else
				{
					dataView.RowFilter = "f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
					string text = " f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
					int num = (int)this.arrZoneID[this.cboZone.SelectedIndex];
					int num2 = (int)this.arrZoneNO[this.cboZone.SelectedIndex];
					int zoneChildMaxNo = icControllerZone.getZoneChildMaxNo(this.cboZone.Text, this.arrZoneName, this.arrZoneNO);
					if (num2 > 0)
					{
						if (num2 >= zoneChildMaxNo)
						{
							dataView.RowFilter = string.Format(" f_ZoneID ={0:d} ", num);
							text = string.Format(" f_ZoneID ={0:d} ", num);
						}
						else
						{
							dataView.RowFilter = "";
							string zoneQuery = icGroup.getZoneQuery(num2, zoneChildMaxNo, this.arrZoneNO, this.arrZoneID);
							dataView.RowFilter = string.Format("  {0} ", zoneQuery);
							text = string.Format("  {0} ", zoneQuery);
						}
					}
					dataView.RowFilter = string.Format(" {0} ", text);
				}
				this.chkListDoors.Items.Clear();
				if (this.dvDoors.Count > 0)
				{
					for (int i = 0; i < this.dvDoors.Count; i++)
					{
						this.arrAddr[i] = (int)this.dvDoors[i]["f_ReaderID"];
						this.chkListDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_ReaderName"]));
					}
					return;
				}
			}
			else
			{
				this.chkListDoors.Items.Clear();
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0005DBEF File Offset: 0x0005CBEF
		private void chkDoors_CheckedChanged(object sender, EventArgs e)
		{
			this.grpAddr.Enabled = this.chkDoors.Checked;
			if (!this.chkDoors.Checked)
			{
				this.chkListDoors.ClearSelected();
				this.btnSelectNone_Click(null, null);
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0005DC28 File Offset: 0x0005CC28
		private void chkRecordTypes_CheckedChanged(object sender, EventArgs e)
		{
			this.grpRecordType.Enabled = this.chkRecordTypes.Checked;
			if (!this.chkRecordTypes.Checked)
			{
				this.chkRecType.ClearSelected();
				for (int i = 0; i < this.chkRecType.Items.Count; i++)
				{
					this.chkRecType.SetItemChecked(i, false);
				}
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0005DC8C File Offset: 0x0005CC8C
		private void dfrmSwipeRecordsFindOption_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.dfrmFind1 == null)
					{
						this.dfrmFind1 = new dfrmFind();
					}
					this.dfrmFind1.setObjtoFind(this.chkListDoors, this);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0005DCFC File Offset: 0x0005CCFC
		private void dfrmSwipeRecordsFindOption_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.loadZoneInfo();
			this.loadDoorData();
			this.loadRecTypes();
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0005DD18 File Offset: 0x0005CD18
		public string getStrSql()
		{
			string text = "";
			if (this.chkDoors.Checked)
			{
				if (this.chkListDoors.CheckedItems.Count == 0)
				{
					text = " 1<0 ";
				}
				else
				{
					text = "-1";
					for (int i = 0; i < this.chkListDoors.Items.Count; i++)
					{
						if (this.chkListDoors.GetItemChecked(i))
						{
							text = text + "," + this.arrAddr[i].ToString();
						}
					}
					text = " (t_d_SwipeRecord.f_ReaderID IN ( " + text + " )) ";
				}
			}
			string text2 = "";
			if (this.chkRecordTypes.Checked)
			{
				if (this.chkRecType.CheckedItems.Count == 0)
				{
					text2 = "";
				}
				else
				{
					text2 = " (1<0) ";
					for (int j = 0; j < this.chkRecType.Items.Count; j++)
					{
						if (this.chkRecType.GetItemChecked(j))
						{
							text2 = text2 + "  OR " + this.arrRectypesSql[j].ToString();
						}
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				if (!string.IsNullOrEmpty(text2))
				{
					text = string.Format("({0})", text2);
				}
			}
			else if (!string.IsNullOrEmpty(text2))
			{
				text = string.Format("({0}) AND ({1})", text, text2);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "(1>0)";
			}
			text = text.Replace(" (1<0)   OR ", "");
			if (!wgAppConfig.IsAccessDB)
			{
				text = text.ToUpper().Replace("MOD", "%");
			}
			return text;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0005DE98 File Offset: 0x0005CE98
		private void loadDoorData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadDoorData_Acc();
				return;
			}
			string text = " SELECT c.*,b.f_ControllerSN ,  b.f_ZoneID ";
			text += " FROM t_b_Controller b, t_b_reader c WHERE c.f_ControllerID = b.f_ControllerID  ORDER BY  c.f_ReaderID ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						this.dt = new DataTable();
						this.dvDoors = new DataView(this.dt);
						this.dvDoors4Watching = new DataView(this.dt);
						sqlDataAdapter.Fill(this.dt);
						new icControllerZone().getAllowedControllers(ref this.dt);
						if (this.dvDoors.Count > 0)
						{
							this.arrAddr = new int[this.dvDoors.Count + 1];
							for (int i = 0; i < this.dvDoors.Count; i++)
							{
								string text2 = wgTools.SetObjToStr(this.dvDoors[i]["f_ReaderName"]);
								this.chkListDoors.Items.Add(text2);
								this.arrAddr[i] = (int)this.dvDoors[i]["f_ReaderID"];
							}
						}
					}
				}
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0005E034 File Offset: 0x0005D034
		private void loadDoorData_Acc()
		{
			string text = " SELECT c.*,b.f_ControllerSN ,  b.f_ZoneID ";
			text += " FROM t_b_Controller b, t_b_reader c WHERE c.f_ControllerID = b.f_ControllerID  ORDER BY  c.f_ReaderID ";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
					{
						this.dt = new DataTable();
						this.dvDoors = new DataView(this.dt);
						this.dvDoors4Watching = new DataView(this.dt);
						oleDbDataAdapter.Fill(this.dt);
						new icControllerZone().getAllowedControllers(ref this.dt);
						if (this.dvDoors.Count > 0)
						{
							this.arrAddr = new int[this.dvDoors.Count + 1];
							for (int i = 0; i < this.dvDoors.Count; i++)
							{
								string text2 = wgTools.SetObjToStr(this.dvDoors[i]["f_ReaderName"]);
								this.chkListDoors.Items.Add(text2);
								this.arrAddr[i] = (int)this.dvDoors[i]["f_ReaderID"];
							}
						}
					}
				}
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0005E1C4 File Offset: 0x0005D1C4
		private void loadRecTypes()
		{
			this.chkRecType.Items.Clear();
			this.arrRectypesSql.Clear();
			this.chkRecType.Items.Add(CommonStr.strRecordTypeValidSwipe);
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) AND (f_Status < 128) )");
			this.chkRecType.Items.Add(CommonStr.strRecordTypeInvalidSwipe);
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) AND (f_Status >= 128) )");
			this.chkRecType.Items.Add(wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRecordTypeRemoteOpen));
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 4) =3) AND (f_Status < 128))");
			this.chkRecType.Items.Add(CommonStr.strRecordTypePushButton);
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 4) =1) AND (t_d_SwipeRecord.f_CardNO > 0 and t_d_SwipeRecord.f_CardNO < 8 )  )");
			this.chkRecType.Items.Add(CommonStr.strRecordTypeDoorStatus);
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 4) =1) AND (f_Status < 128) AND (t_d_SwipeRecord.f_CardNO = 8 OR t_d_SwipeRecord.f_CardNO =9 ) )");
			this.chkRecType.Items.Add(CommonStr.strRecordTypeSuperpasswordOpen);
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 4) =1) AND (f_Status < 128) AND (t_d_SwipeRecord.f_CardNO = 10 OR t_d_SwipeRecord.f_CardNO = 11 OR t_d_SwipeRecord.f_CardNO = 12) )");
			this.chkRecType.Items.Add(CommonStr.strRecordTypeWarn);
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 4) =1) AND (f_Status >= 128) AND (t_d_SwipeRecord.f_CardNO > 80 and t_d_SwipeRecord.f_CardNO < 91 ) )");
			this.chkRecType.Items.Add(CommonStr.strRecordTypeAttendance);
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) AND (f_Status < 128) AND  f_AttendEnabled >0)");
			this.chkRecType.Items.Add(CommonStr.strRecordTypeAttendanceInvalid);
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) AND (f_Status >= 128) AND  f_AttendEnabled >0 )");
			this.chkRecType.Items.Add(CommonStr.strRecordTypeUnregisteredInvalid);
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) AND (f_Status >= 128) AND  t_d_SwipeRecord.f_ConsumerID =0 )");
			this.chkRecType.Items.Add(CommonStr.strRecordTypeRegisteredInvalid);
			this.arrRectypesSql.Add("( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) AND (f_Status >= 128) AND  t_d_SwipeRecord.f_ConsumerID >0 )");
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0005E3A0 File Offset: 0x0005D3A0
		private void loadZoneInfo()
		{
			new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
			int i = this.arrZoneID.Count;
			this.cboZone.Items.Clear();
			for (i = 0; i < this.arrZoneID.Count; i++)
			{
				if (i == 0 && string.IsNullOrEmpty(this.arrZoneName[i].ToString()))
				{
					this.cboZone.Items.Add(CommonStr.strAllZones);
				}
				else
				{
					this.cboZone.Items.Add(this.arrZoneName[i].ToString());
				}
			}
			if (this.cboZone.Items.Count > 0)
			{
				this.cboZone.SelectedIndex = 0;
			}
			bool flag = true;
			this.label25.Visible = flag;
			this.cboZone.Visible = flag;
		}

		// Token: 0x04000612 RID: 1554
		private DataTable dt;

		// Token: 0x04000613 RID: 1555
		private DataView dvDoors;

		// Token: 0x04000614 RID: 1556
		private DataView dvDoors4Watching;

		// Token: 0x04000615 RID: 1557
		private int[] arrAddr;

		// Token: 0x04000616 RID: 1558
		private ArrayList arrRectypesSql = new ArrayList();

		// Token: 0x04000617 RID: 1559
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x04000618 RID: 1560
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x04000619 RID: 1561
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x0400061A RID: 1562
		private CheckedListBox listViewNotDisplay = new CheckedListBox();
	}
}
