using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200004D RID: 77
	public partial class frmControllers : frmN3000
	{
		// Token: 0x06000579 RID: 1401 RVA: 0x0009B4EC File Offset: 0x0009A4EC
		public frmControllers()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvControllers);
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0009B53C File Offset: 0x0009A53C
		private void batchUpdateSelectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvControllers.SelectedRows.Count > 0)
			{
				using (dfrmControllerZoneSelect dfrmControllerZoneSelect = new dfrmControllerZoneSelect())
				{
					string text = "";
					for (int i = 0; i < this.dgvControllers.SelectedRows.Count; i++)
					{
						int index = this.dgvControllers.SelectedRows[i].Index;
						int num = int.Parse(this.dgvControllers.Rows[index].Cells[0].Value.ToString());
						if (!string.IsNullOrEmpty(text))
						{
							text += ",";
						}
						text += num.ToString();
					}
					dfrmControllerZoneSelect.Text = string.Format("{0}: [{1}]", sender.ToString(), this.dgvControllers.SelectedRows.Count.ToString());
					if (dfrmControllerZoneSelect.ShowDialog(this) == DialogResult.OK)
					{
						wgAppConfig.runUpdateSql(string.Format(" UPDATE t_b_Controller SET f_ZoneID= {0} WHERE  f_ControllerID IN ({1}) ", dfrmControllerZoneSelect.selectZoneId, text));
						this.loadControllerData();
					}
				}
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0009B664 File Offset: 0x0009A664
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmController dfrmController = new dfrmController())
			{
				int count = this.dv.Table.Rows.Count;
				int count2 = this.dv.Count;
				dfrmController.ShowDialog(this);
				this.loadControllerData();
				this.cboZone_SelectedIndexChanged(null, null);
				if (count != this.dv.Table.Rows.Count && count2 + 1 != this.dv.Count)
				{
					this.loadZoneInfo();
				}
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0009B6FC File Offset: 0x0009A6FC
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (this.dgvControllers.SelectedRows.Count <= 1)
			{
				int num = this.dgvControllers.SelectedRows[0].Index;
				if (XMessageBox.Show(this, string.Concat(new string[]
				{
					CommonStr.strDelete,
					" ",
					this.dgvControllers[1, num].Value.ToString(),
					":",
					this.dgvControllers[2, num].Value.ToString(),
					"?"
				}), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
				{
					return;
				}
			}
			else if (XMessageBox.Show(this, CommonStr.strDeleteSelected + this.dgvControllers.SelectedRows.Count.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
			{
				return;
			}
			int firstDisplayedScrollingRowIndex = this.dgvControllers.FirstDisplayedScrollingRowIndex;
			string text = "";
			if (this.dgvControllers.SelectedRows.Count <= 1)
			{
				int num = this.dgvControllers.SelectedRows[0].Index;
				icController.DeleteControllerFromDB(int.Parse(this.dgvControllers[0, num].Value.ToString()));
				text = string.Concat(new string[]
				{
					text,
					"(",
					this.dgvControllers[1, num].Value.ToString(),
					")",
					this.dgvControllers[2, num].Value.ToString()
				});
			}
			else
			{
				foreach (object obj in this.dgvControllers.SelectedRows)
				{
					DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
					text = string.Concat(new string[]
					{
						text,
						"(",
						dataGridViewRow.Cells[1].Value.ToString(),
						")",
						dataGridViewRow.Cells[2].Value.ToString(),
						","
					});
					icController.DeleteControllerFromDB(int.Parse(dataGridViewRow.Cells[0].Value.ToString()));
				}
			}
			wgAppConfig.wgLog(CommonStr.strDelete + CommonStr.strController + ":" + text);
			this.loadControllerData();
			this.cboZone_SelectedIndexChanged(null, null);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0009B9AC File Offset: 0x0009A9AC
		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (this.dgvControllers.Rows.Count > 0)
			{
				int num = 0;
				if (this.dgvControllers.Rows.Count > 0)
				{
					try
					{
						num = this.dgvControllers.CurrentCell.RowIndex;
					}
					catch
					{
					}
				}
				int rowCount = this.dgvControllers.RowCount;
				using (dfrmController dfrmController = new dfrmController())
				{
					dfrmController.OperateNew = false;
					dfrmController.ControllerID = int.Parse(this.dgvControllers.Rows[num].Cells[0].Value.ToString());
					dfrmController.ShowDialog(this);
					this.loadControllerData();
					if (dfrmController.bEditZone)
					{
						this.loadZoneInfo();
					}
					this.cboZone_SelectedIndexChanged(null, null);
				}
				if (this.dgvControllers.RowCount == 0 || rowCount != this.dgvControllers.RowCount)
				{
					this.loadZoneInfo();
					return;
				}
				if (this.dgvControllers.RowCount > 0)
				{
					if (this.dgvControllers.RowCount > num)
					{
						this.dgvControllers.CurrentCell = this.dgvControllers[1, num];
						return;
					}
					this.dgvControllers.CurrentCell = this.dgvControllers[1, this.dgvControllers.RowCount - 1];
				}
			}
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0009BB0C File Offset: 0x0009AB0C
		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcel(this.dgvControllers, this.Text);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0009BB20 File Offset: 0x0009AB20
		private void btnFind_Click(object sender, EventArgs e)
		{
			wgAppConfig.findCall(this.dfrmFind1, this, this.dgvControllers);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0009BB34 File Offset: 0x0009AB34
		private void btnPrint_Click(object sender, EventArgs e)
		{
			wgAppConfig.printdgv(this.dgvControllers, this.Text);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0009BB48 File Offset: 0x0009AB48
		private void btnSearchController_Click(object sender, EventArgs e)
		{
			using (dfrmNetControllerConfig dfrmNetControllerConfig = new dfrmNetControllerConfig())
			{
				int count = this.dv.Table.Rows.Count;
				int count2 = this.dv.Count;
				dfrmNetControllerConfig.ShowDialog(this);
				this.loadControllerData();
				this.cboZone_SelectedIndexChanged(null, null);
				if (count != this.dv.Table.Rows.Count)
				{
					this.loadZoneInfo();
				}
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0009BBD0 File Offset: 0x0009ABD0
		private void cboZone_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cboZone, this.cboUsers);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0009BBE4 File Offset: 0x0009ABE4
		private void cboZone_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.cboZone.ToolTipText = this.cboZone.Text;
			try
			{
				if (this.dv != null)
				{
					if (this.cboZone.SelectedIndex < 0 || (this.cboZone.SelectedIndex == 0 && (int)this.arrZoneID[0] == 0))
					{
						this.dv.RowFilter = "";
						wgAppRunInfo.raiseAppRunInfoLoadNums(this.dv.Count.ToString());
					}
					else
					{
						this.dv.RowFilter = "f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
						string text = " f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
						int num = (int)this.arrZoneID[this.cboZone.SelectedIndex];
						int num2 = (int)this.arrZoneNO[this.cboZone.SelectedIndex];
						int zoneChildMaxNo = icControllerZone.getZoneChildMaxNo(this.cboZone.Text, this.arrZoneName, this.arrZoneNO);
						if (num2 > 0)
						{
							if (num2 >= zoneChildMaxNo)
							{
								this.dv.RowFilter = string.Format(" f_ZoneID ={0:d} ", num);
								text = string.Format(" f_ZoneID ={0:d} ", num);
							}
							else
							{
								this.dv.RowFilter = "";
								string zoneQuery = icGroup.getZoneQuery(num2, zoneChildMaxNo, this.arrZoneNO, this.arrZoneID);
								this.dv.RowFilter = string.Format("  {0} ", zoneQuery);
								text = string.Format("  {0} ", zoneQuery);
							}
						}
						this.dv.RowFilter = string.Format(" {0} ", text);
						wgTools.WriteLine("foreach (ListViewItem itm in listViewNotDisplay.Items)");
						wgAppRunInfo.raiseAppRunInfoLoadNums(this.dv.Count.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0009BE08 File Offset: 0x0009AE08
		private void dgvControllers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.btnEdit.Enabled)
			{
				this.btnEdit.PerformClick();
			}
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0009BE24 File Offset: 0x0009AE24
		private void doorsExportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = "SELECT t_b_Door.f_DoorID,  t_b_Door.f_DoorName, t_b_Door.f_DoorEnabled,t_b_Door.f_DoorDelay,t_b_Controller.f_ControllerNO, t_b_Controller.f_ControllerSN, t_b_Door.f_DoorNO FROM t_b_Door, t_b_Controller where t_b_door.f_controllerid= t_b_controller.f_controllerid ";
			try
			{
				DataSet dataSet = new DataSet();
				DataTable dataTable = new DataTable();
				using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
				{
					using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
					{
						using (DataAdapter dataAdapter = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand) : new SqlDataAdapter((SqlCommand)dbCommand)))
						{
							dataAdapter.Fill(dataSet);
						}
					}
				}
				dataTable = dataSet.Tables[0];
				this.dataGridView1.AutoGenerateColumns = false;
				this.dataGridView1.DataSource = dataSet.Tables[0];
				int num = 0;
				while (num < dataTable.Columns.Count && num < this.dataGridView1.ColumnCount)
				{
					this.dataGridView1.Columns[num].DataPropertyName = dataTable.Columns[num].ColumnName;
					num++;
				}
				this.dataGridView1.Columns[0].Visible = false;
				wgAppConfig.exportToExcel(this.dataGridView1, CommonStr.strFloor);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0009BFF8 File Offset: 0x0009AFF8
		private void frmControllers_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0009C010 File Offset: 0x0009B010
		public void frmControllers_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x06000588 RID: 1416 RVA: 0x0009C0BC File Offset: 0x0009B0BC
		private void frmControllers_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.loadZoneInfo();
			this.loadOperatorPrivilege();
			this.loadControllerData();
			this.dgvControllers.ContextMenuStrip = this.contextMenuStrip1;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0009C0E8 File Offset: 0x0009B0E8
		private void funcCtrlShiftQ()
		{
			try
			{
				string strNewName;
				using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
				{
					dfrmInputNewName.Text = CommonStr.strControllerBeginNo;
					dfrmInputNewName.label1.Text = CommonStr.strControllerSN;
					dfrmInputNewName.strNewName = "";
					uint num;
					if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || !uint.TryParse(dfrmInputNewName.strNewName, out num))
					{
						return;
					}
					strNewName = dfrmInputNewName.strNewName;
				}
				string text;
				using (dfrmInputNewName dfrmInputNewName2 = new dfrmInputNewName())
				{
					dfrmInputNewName2.Text = CommonStr.strControllerEndNo;
					dfrmInputNewName2.label1.Text = CommonStr.strControllerSN;
					dfrmInputNewName2.strNewName = "";
					if (dfrmInputNewName2.ShowDialog(this) == DialogResult.OK)
					{
						uint num;
						if (!uint.TryParse(dfrmInputNewName2.strNewName, out num))
						{
							text = strNewName;
						}
						else
						{
							text = dfrmInputNewName2.strNewName;
						}
					}
					else
					{
						text = strNewName;
					}
				}
				if (Information.IsNumeric(strNewName) && Information.IsNumeric(text))
				{
					if (int.Parse(strNewName) <= int.Parse(text) && wgMjController.GetControllerType(int.Parse(strNewName)) >= 0 && wgMjController.GetControllerType(int.Parse(text)) >= 0)
					{
						using (dfrmController dfrmController = new dfrmController())
						{
							dfrmController.Show();
							for (long num2 = (long)int.Parse(strNewName); num2 <= (long)int.Parse(text); num2 += 1L)
							{
								dfrmController.Text = num2.ToString();
								dfrmController.mtxtbControllerSN.Text = num2.ToString();
								dfrmController.mtxtbControllerNO.Text = ((long)((int)(num2 / 100000000L) * 10000) + num2 % 10000L).ToString();
								dfrmController.btnNext.PerformClick();
								dfrmController.btnOK_Click(null, null);
								Application.DoEvents();
							}
						}
					}
					XMessageBox.Show(CommonStr.strSNWrong);
				}
				else
				{
					XMessageBox.Show(CommonStr.strSNWrong);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
			this.loadControllerData();
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0009C33C File Offset: 0x0009B33C
		private void loadControllerData()
		{
			this.dtController = new DataTable();
			this.dv = new DataView(this.dtController);
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = " SELECT f_ControllerID, f_ControllerNO, f_ControllerSN, f_Enabled, f_IP, f_PORT, f_ZoneName, f_Note, f_DoorsNames,  t_b_Controller.f_ZoneID ";
				text += " FROM t_b_Controller LEFT OUTER JOIN t_b_Controller_Zone ON t_b_Controller.f_ZoneID = t_b_Controller_Zone.f_ZoneID  ORDER BY [f_ControllerNO]";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtController);
						}
					}
					goto IL_00ED;
				}
			}
			text = " SELECT f_ControllerID, f_ControllerNO, f_ControllerSN, f_Enabled, f_IP, f_PORT, f_ZoneName, f_Note, f_DoorsNames,  t_b_Controller.f_ZoneID ";
			text += " FROM t_b_Controller LEFT OUTER JOIN t_b_Controller_Zone ON t_b_Controller.f_ZoneID = t_b_Controller_Zone.f_ZoneID  ORDER BY f_ControllerNO ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtController);
					}
				}
			}
			IL_00ED:
			icControllerZone icControllerZone = new icControllerZone();
			icControllerZone.getAllowedControllers(ref this.dtController);
			this.dgvControllers.AutoGenerateColumns = false;
			this.dgvControllers.DataSource = this.dv;
			for (int i = 0; i < this.dv.Table.Columns.Count - 1; i++)
			{
				this.dgvControllers.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
			}
			this.dgvControllers.Columns[6].Visible = true;
			if (this.dv.Count > 0)
			{
				this.btnAdd.Enabled = true;
				this.btnEdit.Enabled = true;
				this.btnDelete.Enabled = true;
				this.btnPrint.Enabled = true;
			}
			else
			{
				this.btnAdd.Enabled = true;
				this.btnEdit.Enabled = false;
				this.btnDelete.Enabled = false;
				this.btnPrint.Enabled = false;
			}
			wgAppRunInfo.raiseAppRunInfoLoadNums(this.dv.Count.ToString());
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0009C5AC File Offset: 0x0009B5AC
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuControllers";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnEdit.Visible = false;
				this.btnDelete.Visible = false;
				this.btnSearchController.Visible = false;
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0009C600 File Offset: 0x0009B600
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
			this.cboZone.Visible = true;
		}

		// Token: 0x04000A68 RID: 2664
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x04000A69 RID: 2665
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x04000A6A RID: 2666
		private ArrayList arrZoneNO = new ArrayList();
	}
}
