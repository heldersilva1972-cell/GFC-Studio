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
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000027 RID: 39
	public partial class dfrmPrivilegeDoorCopy : frmN3000
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x00054148 File Offset: 0x00053148
		public dfrmPrivilegeDoorCopy()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors);
			wgAppConfig.custDataGridview(ref this.dgvSelectedDoors4Copy);
			wgAppConfig.custDataGridview(ref this.dgvDoors);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000541E4 File Offset: 0x000531E4
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

		// Token: 0x060002BB RID: 699 RVA: 0x00054228 File Offset: 0x00053228
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvDoors.DataSource).Table;
			if (this.cbof_ZoneID.SelectedIndex <= 0 && this.cbof_ZoneID.Text == CommonStr.strAllZones)
			{
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					this.dt.Rows[i]["f_Selected"] = 1;
				}
			}
			else if (this.cbof_ZoneID.SelectedIndex >= 0)
			{
				this.dvtmp = new DataView((this.dgvDoors.DataSource as DataView).Table);
				this.dvtmp.RowFilter = string.Format("  {0} ", this.strZoneFilter);
				for (int j = 0; j < this.dvtmp.Count; j++)
				{
					this.dvtmp[j]["f_Selected"] = 1;
				}
			}
			this.toolStripStatusLabel1.Text = this.dgvSelectedDoors.Rows.Count.ToString();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00054354 File Offset: 0x00053354
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvDoors);
			this.toolStripStatusLabel1.Text = this.dgvSelectedDoors.Rows.Count.ToString();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00054390 File Offset: 0x00053390
		private void btnAddOneUser4Copy_Click(object sender, EventArgs e)
		{
			if (this.dt4copy.Rows.Count <= 0)
			{
				try
				{
					DataGridView dataGridView = this.dgvDoors;
					int num;
					if (dataGridView.SelectedRows.Count <= 0)
					{
						if (dataGridView.SelectedCells.Count <= 0)
						{
							return;
						}
						num = dataGridView.SelectedCells[0].RowIndex;
					}
					else
					{
						num = dataGridView.SelectedRows[0].Index;
					}
					DataTable table = ((DataView)dataGridView.DataSource).Table;
					int num2 = (int)dataGridView.Rows[num].Cells[0].Value;
					DataRow dataRow = table.Rows.Find(num2);
					if (dataRow != null)
					{
						DataRow dataRow2 = this.dt4copy.NewRow();
						for (int i = 0; i < table.Columns.Count; i++)
						{
							dataRow2[i] = dataRow[i];
						}
						table.Rows.Remove(dataRow);
						table.AcceptChanges();
						this.dt4copy.Rows.Add(dataRow2);
						this.dt4copy.AcceptChanges();
					}
				}
				catch (Exception ex)
				{
					wgTools.WriteLine(ex.ToString());
				}
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000544E4 File Offset: 0x000534E4
		private void btnAddPass_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedDoors.RowCount > 0 && this.dgvSelectedDoors4Copy.RowCount > 0 && XMessageBox.Show(this.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.Cancel)
			{
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
				dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
				string text = "";
				bool flag = false;
				this.timer1.Enabled = true;
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				try
				{
					DataView dataView = (DataView)this.dgvSelectedDoors.DataSource;
					DataView dataView2 = (DataView)this.dgvSelectedDoors4Copy.DataSource;
					if (dbConnection.State != ConnectionState.Open)
					{
						dbConnection.Open();
					}
					try
					{
						int i = 0;
						this.progressBar1.Maximum = 2 * this.dgvSelectedDoors.RowCount;
						while (i < this.dgvSelectedDoors.Rows.Count)
						{
							text = "DELETE FROM  [t_d_Privilege] ";
							text = text + " WHERE f_DoorID = " + ((DataView)this.dgvSelectedDoors.DataSource)[i]["f_DoorID"];
							dbCommand.CommandText = text;
							i++;
							dbCommand.ExecuteNonQuery();
							int count = this.dgvSelectedDoors.Rows.Count;
							this.progressBar1.Value = i;
							this.toolStripStatusLabel2.Text = "(x)" + i.ToString() + "." + ((DataView)this.dgvSelectedDoors.DataSource)[i - 1]["f_DoorName"].ToString();
							Application.DoEvents();
						}
						dbConnection.Close();
						if (dbConnection.State != ConnectionState.Open)
						{
							dbConnection.Open();
						}
						i = 0;
						text = " SELECT count(t_d_Privilege.f_ConsumerID) ";
						text += " FROM t_d_Privilege ";
						text = text + " WHERE (t_d_Privilege.f_DoorID)= " + dataView2[0]["f_DoorID"];
						dbCommand.CommandText = text;
						int num = int.Parse(wgTools.SetObjToStr(dbCommand.ExecuteScalar()));
						while (i < this.dgvSelectedDoors.Rows.Count)
						{
							text = "INSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])";
							text += " SELECT t_d_Privilege.f_ConsumerID, t_b_Door.f_DoorID,t_d_Privilege.[f_ControlSegID] , t_b_Controller.[f_ControllerID], t_b_Door.[f_DoorNO] ";
							text += " FROM t_d_Privilege, t_b_Door, t_b_Controller ";
							text = text + " WHERE t_b_Door.f_DoorID = " + ((DataView)this.dgvSelectedDoors.DataSource)[i]["f_DoorID"];
							text = text + " AND (t_d_Privilege.f_DoorID)= " + dataView2[0]["f_DoorID"];
							text += " AND (t_b_Controller.f_ControllerID)= (t_b_Door.f_ControllerID ) ";
							i++;
							dbCommand.CommandText = text;
							dbCommand.ExecuteNonQuery();
							this.progressBar1.Value = i + this.dgvSelectedDoors.Rows.Count;
							this.toolStripStatusLabel2.Text = "(+)" + i.ToString() + "." + ((DataView)this.dgvSelectedDoors.DataSource)[i - 1]["f_DoorName"].ToString();
							Application.DoEvents();
						}
						dbConnection.Close();
						if (dbConnection.State != ConnectionState.Open)
						{
							dbConnection.Open();
						}
						flag = true;
						string text2 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} ";
						text = string.Format(text2, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), num);
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						this.logOperate(null);
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
						wgAppConfig.wgLog(ex.ToString() + "\r\nstrSql = " + text);
					}
				}
				catch (Exception ex2)
				{
					this.dfrmWait1.Hide();
					wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
					wgAppConfig.wgLog(ex2.ToString() + "\r\nstrSql2 = " + text);
					XMessageBox.Show(ex2.Message);
				}
				finally
				{
					if (dbCommand != null)
					{
						dbCommand.Dispose();
					}
					if (dbConnection.State != ConnectionState.Closed)
					{
						dbConnection.Close();
					}
					this.dfrmWait1.Hide();
				}
				this.progressBar1.Value = this.progressBar1.Maximum;
				Cursor.Current = Cursors.Default;
				if (flag)
				{
					wgAppConfig.wgLog(this.Text + CommonStr.strSuccessfully);
					XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
					base.DialogResult = DialogResult.OK;
					this.bEdit = true;
					base.Close();
					return;
				}
				this.progressBar1.Value = 0;
				this.bEdit = true;
				wgAppConfig.wgLog(this.Text + CommonStr.strOperateFailed);
				XMessageBox.Show(this.Text + CommonStr.strOperateFailed);
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00054A24 File Offset: 0x00053A24
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			this.dt = ((DataView)this.dgvSelectedDoors.DataSource).Table;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 0;
			}
			this.toolStripStatusLabel1.Text = this.dgvSelectedDoors.Rows.Count.ToString();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00054AAC File Offset: 0x00053AAC
		private void btnDeleteOneUser4Copy_Click(object sender, EventArgs e)
		{
			if (this.dt4copy.Rows.Count != 0)
			{
				try
				{
					DataTable table = ((DataView)this.dgvDoors.DataSource).Table;
					DataRow dataRow = table.NewRow();
					if (dataRow != null)
					{
						DataRow dataRow2 = this.dt4copy.Rows[0];
						for (int i = 0; i < table.Columns.Count; i++)
						{
							dataRow[i] = dataRow2[i];
						}
						dataRow["f_Selected"] = 0;
						table.Rows.Add(dataRow);
						table.AcceptChanges();
						this.dt4copy.Rows.Remove(dataRow2);
						this.dt4copy.AcceptChanges();
					}
				}
				catch (Exception ex)
				{
					wgTools.WriteLine(ex.ToString());
				}
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00054B88 File Offset: 0x00053B88
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedDoors);
			this.toolStripStatusLabel1.Text = this.dgvSelectedDoors.Rows.Count.ToString();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00054BC3 File Offset: 0x00053BC3
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

		// Token: 0x060002C3 RID: 707 RVA: 0x00054BE3 File Offset: 0x00053BE3
		private void btnFind_Click(object sender, EventArgs e)
		{
			if (this.defaultFindControl == null)
			{
				this.defaultFindControl = this.dgvDoors;
			}
			wgAppConfig.findCall(this.dfrmFind1, this, this.defaultFindControl);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00054C0B File Offset: 0x00053C0B
		private void cbof_ZoneID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_ZoneID);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00054C18 File Offset: 0x00053C18
		private void cbof_ZoneID_SelectedIndexChanged(object sender, EventArgs e)
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

		// Token: 0x060002C6 RID: 710 RVA: 0x00054DDE File Offset: 0x00053DDE
		private void dfrmPrivilegeCopy_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00054DF4 File Offset: 0x00053DF4
		private void dfrmPrivilegeCopy_KeyDown(object sender, KeyEventArgs e)
		{
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

		// Token: 0x060002C8 RID: 712 RVA: 0x00054E64 File Offset: 0x00053E64
		private void dfrmPrivilegeCopy_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.loadZoneInfo();
			string text = " SELECT ";
			text += " t_b_Door.f_DoorID , t_b_Controller.f_ControllerSN , t_b_Door.f_DoorNO , t_b_Door.f_DoorName , t_b_Controller.f_ZoneID  , 0 as f_Selected FROM t_b_Door , t_b_Controller  WHERE t_b_Door.f_DoorEnabled > 0 and t_b_Controller.f_Enabled >0 and t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID  AND ( t_b_Controller.f_ControllerSN < 160000000 OR t_b_Controller.f_ControllerSN > 180000000 )  ORDER BY t_b_Door.f_DoorName ";
			wgAppConfig.fillDGVData(ref this.dgvDoors, text);
			this.dt = (this.dgvDoors.DataSource as DataView).Table;
			try
			{
				this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.dv = new DataView(this.dt);
			this.dvSelected = new DataView(this.dt);
			this.dt4copy = this.dt.Clone();
			this.dvSelectedNone = new DataView(this.dt4copy);
			this.dv.RowFilter = "f_Selected = 0";
			this.dv.Sort = "f_DoorName";
			this.dvSelected.RowFilter = "f_Selected > 0";
			this.dvSelected.Sort = "f_DoorName";
			this.dgvDoors.AutoGenerateColumns = false;
			this.dgvDoors.DataSource = this.dv;
			this.dgvSelectedDoors.AutoGenerateColumns = false;
			this.dgvSelectedDoors.DataSource = this.dvSelected;
			this.dgvSelectedDoors4Copy.AutoGenerateColumns = false;
			this.dgvSelectedDoors4Copy.DataSource = this.dvSelectedNone;
			for (int i = 0; i < this.dgvDoors.Columns.Count; i++)
			{
				this.dgvDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
				this.dgvSelectedDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
				this.dgvSelectedDoors4Copy.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
			}
			this.cbof_ZoneID_SelectedIndexChanged(null, null);
			this.dgvDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedDoors4Copy.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000550D0 File Offset: 0x000540D0
		private void dfrmPrivilegeDoorCopy_FormClosed(object sender, FormClosedEventArgs e)
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

		// Token: 0x060002CA RID: 714 RVA: 0x00055108 File Offset: 0x00054108
		private void dgvDoors_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00055138 File Offset: 0x00054138
		private void dgvDoors_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrmPrivilegeCopy_KeyDown(this.dgvDoors, e);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00055147 File Offset: 0x00054147
		private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.dgvSelectedDoors4Copy.Rows.Count == 0)
			{
				this.btnAddOneUser4Copy.PerformClick();
				return;
			}
			this.btnAddOneUser.PerformClick();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00055172 File Offset: 0x00054172
		private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0005517F File Offset: 0x0005417F
		private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0005518C File Offset: 0x0005418C
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000551AC File Offset: 0x000541AC
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

		// Token: 0x060002D1 RID: 721 RVA: 0x00055298 File Offset: 0x00054298
		private void logOperate(object sender)
		{
			string text = "";
			for (int i = 0; i <= Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedDoors.RowCount) - 1; i++)
			{
				text = text + ((DataView)this.dgvSelectedDoors.DataSource)[i]["f_DoorName"] + ",";
			}
			if (this.dgvSelectedDoors.RowCount > wgAppConfig.LogEventMaxCount)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"......(",
					this.dgvSelectedDoors.RowCount,
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
					this.dgvSelectedDoors.RowCount,
					")"
				});
			}
			wgAppConfig.wgLog(string.Format("{0}: {1} => {2}", this.Text.Replace("\r\n", ""), ((DataView)this.dgvSelectedDoors4Copy.DataSource)[0]["f_DoorName"].ToString(), text), EventLogEntryType.Information, null);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x000553D4 File Offset: 0x000543D4
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
				else if (this.dgvDoors.DataSource == null)
				{
					Cursor.Current = Cursors.WaitCursor;
				}
				else
				{
					this.timer1.Enabled = false;
					Cursor.Current = Cursors.Default;
					this.lblWait.Visible = false;
					this.btnAddAllUsers.Enabled = true;
					this.btnAddOneUser.Enabled = true;
					this.btnAddOneUser4Copy.Enabled = true;
					this.btnAddPass.Enabled = true;
					this.btnDelAllUsers.Enabled = true;
					this.btnDeleteOneUser4Copy.Enabled = true;
					this.btnDelOneUser.Enabled = true;
					this.bStarting = false;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0400055B RID: 1371
		private bool bEdit;

		// Token: 0x0400055C RID: 1372
		private Control defaultFindControl;

		// Token: 0x0400055D RID: 1373
		private DataTable dt;

		// Token: 0x0400055E RID: 1374
		private DataTable dt4copy;

		// Token: 0x0400055F RID: 1375
		private DataView dv;

		// Token: 0x04000560 RID: 1376
		private DataView dvSelected;

		// Token: 0x04000561 RID: 1377
		private DataView dvSelectedNone;

		// Token: 0x04000562 RID: 1378
		private DataView dvtmp;

		// Token: 0x04000563 RID: 1379
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04000564 RID: 1380
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04000565 RID: 1381
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04000566 RID: 1382
		private ArrayList arrZoneID = new ArrayList();

		// Token: 0x04000567 RID: 1383
		private ArrayList arrZoneName = new ArrayList();

		// Token: 0x04000568 RID: 1384
		private ArrayList arrZoneNO = new ArrayList();

		// Token: 0x04000569 RID: 1385
		private bool bStarting = true;

		// Token: 0x0400056B RID: 1387
		private string strZoneFilter = "";
	}
}
