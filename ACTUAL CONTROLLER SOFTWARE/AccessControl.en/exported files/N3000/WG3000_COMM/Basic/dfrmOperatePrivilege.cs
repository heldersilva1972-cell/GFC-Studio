using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200001E RID: 30
	public partial class dfrmOperatePrivilege : frmN3000
	{
		// Token: 0x060001AB RID: 427 RVA: 0x0003BAE1 File Offset: 0x0003AAE1
		public dfrmOperatePrivilege()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvOperatePrivilege);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0003BB01 File Offset: 0x0003AB01
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0003BB0C File Offset: 0x0003AB0C
		private void btnFullControlAllOn_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.dgvOperatePrivilege.Columns.Count; i++)
			{
				if (this.dgvOperatePrivilege.Columns[i].Name.Equals("f_FullControl"))
				{
					for (int j = 0; j < this.dgvOperatePrivilege.Rows.Count; j++)
					{
						this.dgvOperatePrivilege[i, j].Value = true;
					}
				}
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0003BB8C File Offset: 0x0003AB8C
		private void btnFullControlOff_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.dgvOperatePrivilege.Columns.Count; i++)
			{
				if (this.dgvOperatePrivilege.Columns[i].Name.Equals("f_FullControl"))
				{
					for (int j = 0; j < this.dgvOperatePrivilege.Rows.Count; j++)
					{
						this.dgvOperatePrivilege[i, j].Value = false;
					}
				}
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0003BC09 File Offset: 0x0003AC09
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (icOperator.setOperatorPrivilege(this.operatorID, (this.dgvOperatePrivilege.DataSource as DataView).Table) > 0)
			{
				base.Close();
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0003BC34 File Offset: 0x0003AC34
		private void btnReadAllOff_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.dgvOperatePrivilege.Columns.Count; i++)
			{
				if (this.dgvOperatePrivilege.Columns[i].Name.Equals("f_ReadOnly"))
				{
					for (int j = 0; j < this.dgvOperatePrivilege.Rows.Count; j++)
					{
						this.dgvOperatePrivilege[i, j].Value = false;
					}
				}
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0003BCB4 File Offset: 0x0003ACB4
		private void btnReadAllOn_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.dgvOperatePrivilege.Columns.Count; i++)
			{
				if (this.dgvOperatePrivilege.Columns[i].Name.Equals("f_ReadOnly"))
				{
					for (int j = 0; j < this.dgvOperatePrivilege.Rows.Count; j++)
					{
						this.dgvOperatePrivilege[i, j].Value = true;
					}
				}
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0003BD34 File Offset: 0x0003AD34
		private void dfrmOperatePrivilege_Load(object sender, EventArgs e)
		{
			this.dgvOperatePrivilege.AutoGenerateColumns = false;
			this.tb = new DataTable();
			this.tb.TableName = "OperatePrivilege";
			this.tb.Columns.Add("f_FunctionID");
			this.tb.Columns.Add("f_FunctionName");
			this.tb.Columns.Add("f_FunctionDisplayName");
			this.tb.Columns.Add("f_ReadOnly");
			this.tb.Columns.Add("f_FullControl");
			this.tb.Columns.Add("f_DisplayID");
			DataRow dataRow = this.tb.NewRow();
			this.updateDr(ref dataRow, 1, "frmControllers", "Controllers", false, true);
			this.tb.Rows.Add(dataRow);
			this.tb.AcceptChanges();
			this.dgvOperatePrivilege.DataSource = this.tb;
			this.dgvOperatePrivilege.Columns[0].DataPropertyName = "f_FunctionID";
			this.dgvOperatePrivilege.Columns[1].DataPropertyName = "f_FunctionName";
			this.dgvOperatePrivilege.Columns[2].DataPropertyName = "f_FunctionDisplayName";
			this.dgvOperatePrivilege.Columns[3].DataPropertyName = "f_ReadOnly";
			this.dgvOperatePrivilege.Columns[4].DataPropertyName = "f_FullControl";
			this.dgvOperatePrivilege.Columns[5].DataPropertyName = "f_DisplayID";
			string text = "";
			DataTable operatorPrivilege = icOperator.getOperatorPrivilege(this.operatorID);
			if (operatorPrivilege != null)
			{
				operatorPrivilege.Columns.Add("f_DisplayID");
				string[] array = new string[]
				{
					"090", "mnu1File", "091", "mnuLogQuery", "092", "mnuDBBackup", "100", "mnu1BasicConfigure", "110", "mnuControllers",
					"120", "btnZoneManage", "130", "mnuGroups", "140", "mnuConsumers", "141", "mnuCardLost", "210", "mnuPrivilege",
					"220", "mnuControlSeg", "230", "mnuPeripheral", "240", "mnuPasswordManagement", "250", "mnuAntiBack", "260", "mnuInterLock",
					"270", "mnuMoreCards", "280", "mnuFirstCard", "290", "mnuTaskList", "295", "mnuCameraManage", "300", "mnu1BasicOperate",
					"310", "mnuTotalControl", "311", "mnuMonitor", "312", "mnuCheckController", "313", "mnuAdjustTime", "314", "mnuUpload",
					"315", "mnuGetCardRecords", "316", "mnuRealtimeGetRecords", "317", "TotalControl_RemoteOpen", "318", "btnMaps", "319", "mnuCameraMonitor",
					"320", "mnuCardRecords", "400", "mnu1Attendence", "401", "mnuAttendenceData", "410", "mnuShiftNormalConfigure", "420", "mnuShiftRule",
					"430", "mnuShiftSet", "440", "mnuShiftArrange", "450", "mnuHolidaySet", "460", "mnuLeave", "470", "mnuManualCardRecord",
					"481", "mnuPatrolDetailData", "482", "mnuConstMeal", "483", "mnuMeeting", "484", "mnuElevator", "500", "mnu1Tool",
					"510", "cmdOperatorManage", "520", "cmdChangePasswor", "540", "mnuExtendedFunction", "550", "mnuOption", "600", "mnu1Help",
					"610", "mnuAbout", "620", "mnuManual", "630", "mnuSystemCharacteristic", "", ""
				};
				for (int i = 0; i < operatorPrivilege.Rows.Count; i++)
				{
					operatorPrivilege.Rows[i]["f_FunctionDisplayName"] = CommonStr.ResourceManager.GetString("strFunctionDisplayName_" + operatorPrivilege.Rows[i]["f_FunctionName"].ToString());
					operatorPrivilege.Rows[i]["f_FunctionDisplayName"] = wgAppConfig.ReplaceFloorRoom(operatorPrivilege.Rows[i]["f_FunctionDisplayName"] as string);
					for (int j = 0; j < array.Length; j += 2)
					{
						if (array[j + 1] == operatorPrivilege.Rows[i]["f_FunctionName"].ToString())
						{
							operatorPrivilege.Rows[i]["f_DisplayID"] = array[j];
						}
					}
				}
				if (wgAppConfig.IsAccessControlBlue)
				{
					for (int k = 0; k < operatorPrivilege.Rows.Count; k++)
					{
						if (operatorPrivilege.Rows[k]["f_FunctionName"].ToString().CompareTo("mnuCameraManage") == 0)
						{
							operatorPrivilege.Rows.RemoveAt(k);
						}
					}
					operatorPrivilege.AcceptChanges();
					for (int l = 0; l < operatorPrivilege.Rows.Count; l++)
					{
						if (operatorPrivilege.Rows[l]["f_FunctionName"].ToString().CompareTo("mnuCameraMonitor") == 0)
						{
							operatorPrivilege.Rows.RemoveAt(l);
						}
					}
					operatorPrivilege.AcceptChanges();
				}
				for (int m = 0; m < operatorPrivilege.Rows.Count; m++)
				{
					if (operatorPrivilege.Rows[m]["f_FunctionName"].ToString().CompareTo("mnu1DoorControl") == 0)
					{
						operatorPrivilege.Rows.RemoveAt(m);
					}
				}
				operatorPrivilege.AcceptChanges();
				this.dgvOperatePrivilege.DataSource = operatorPrivilege;
				this.dv = new DataView(operatorPrivilege);
				this.dv.Sort = "f_DisplayID";
				this.dgvOperatePrivilege.DataSource = this.dv;
			}
			wgTools.WgDebugWrite(text, new object[0]);
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			dfrmOperatePrivilege.styleYellow = new DataGridViewCellStyle
			{
				Alignment = DataGridViewContentAlignment.MiddleLeft,
				BackColor = Color.Yellow,
				Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Pixel, 134),
				ForeColor = Color.Blue,
				SelectionBackColor = SystemColors.Highlight,
				SelectionForeColor = SystemColors.HighlightText,
				WrapMode = DataGridViewTriState.False
			};
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0003C5F8 File Offset: 0x0003B5F8
		private void dgvOperatePrivilege_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DataGridViewCell dataGridViewCell = this.dgvOperatePrivilege[1, e.RowIndex];
				if (e.Value != null)
				{
					ArrayList arrayList = new ArrayList { "mnu1BasicConfigure", "mnu1DoorControl", "mnu1BasicOperate", "mnu1Attendence", "mnuPatrolDetailData", "mnuConstMeal", "mnuMeeting", "mnuElevator", "mnu1Tool", "mnu1Help" };
					if (arrayList.IndexOf(dataGridViewCell.Value.ToString()) >= 0)
					{
						DataGridViewRow dataGridViewRow = this.dgvOperatePrivilege.Rows[e.RowIndex];
						dataGridViewRow.DefaultCellStyle = dfrmOperatePrivilege.styleYellow;
					}
				}
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0003C6E8 File Offset: 0x0003B6E8
		private void updateDr(ref DataRow dr, int id, string name, string display, bool read, bool fullControl)
		{
			dr[0] = id;
			dr[1] = name;
			dr[2] = display;
			dr[3] = read;
			dr[4] = fullControl;
		}

		// Token: 0x040003CA RID: 970
		private DataView dv;

		// Token: 0x040003CB RID: 971
		private DataTable tb;

		// Token: 0x040003CC RID: 972
		public int operatorID = -1;

		// Token: 0x040003CD RID: 973
		private static DataGridViewCellStyle styleYellow = new DataGridViewCellStyle();
	}
}
