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
	// Token: 0x02000243 RID: 579
	public partial class dfrmLocate : frmN3000
	{
		// Token: 0x060011DC RID: 4572 RVA: 0x00150FA4 File Offset: 0x0014FFA4
		public dfrmLocate()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0015103C File Offset: 0x0015003C
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

		// Token: 0x060011DE RID: 4574 RVA: 0x00151080 File Offset: 0x00150080
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

		// Token: 0x060011DF RID: 4575 RVA: 0x00151100 File Offset: 0x00150100
		private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			this.strInOutInfo = "";
			try
			{
				int num = 0;
				DataRow[] array = new DataRow[]
				{
					this.ds.Tables["cardrecord"].NewRow(),
					this.ds.Tables["cardrecord"].NewRow()
				};
				string text = " SELECT * FROM t_d_SwipeRecord WHERE f_Character >0 AND f_ConsumerID = " + this.strUserId + " ORDER BY f_ReadDate DESC ";
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							oleDbConnection.Open();
							oleDbCommand.CommandTimeout = 180;
							OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
							while (oleDbDataReader.Read())
							{
								if ((DateTime)oleDbDataReader["f_ReadDate"] <= DateTime.Now.AddDays(2.0))
								{
									for (int i = 0; i <= oleDbDataReader.FieldCount - 1; i++)
									{
										array[num][i] = oleDbDataReader[i];
									}
									num++;
									if (num >= 2)
									{
										break;
									}
								}
							}
							oleDbDataReader.Close();
						}
						goto IL_0207;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.CommandTimeout = 180;
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							if ((DateTime)sqlDataReader["f_ReadDate"] <= DateTime.Now.AddDays(2.0))
							{
								for (int i = 0; i <= sqlDataReader.FieldCount - 1; i++)
								{
									array[num][i] = sqlDataReader[i];
								}
								num++;
								if (num >= 2)
								{
									break;
								}
							}
						}
						sqlDataReader.Close();
					}
				}
				IL_0207:
				string text2 = this.m_strUsers + "\r\n";
				if (num > 0)
				{
					if (num == 2 && (int)array[0]["f_ControllerSN"] == (int)array[1]["f_ControllerSN"] && (DateTime)array[0]["f_ReadDate"] > (DateTime)array[1]["f_ReadDate"] && array[0]["f_InOut"].ToString() == "0" && array[1]["f_InOut"].ToString() == "1")
					{
						this.controller4Locate.GetInfoFromDBByControllerSN((int)array[1]["f_ControllerSN"]);
						text2 = string.Concat(new string[]
						{
							text2,
							((DateTime)array[1]["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek),
							"   {0}  ",
							this.controller4Locate.GetDoorNameByReaderNO((int)((byte)array[1]["f_ReaderNO"])),
							"\r\n"
						});
						this.controller4Locate.GetInfoFromDBByControllerSN((int)array[0]["f_ControllerSN"]);
						text2 = string.Concat(new string[]
						{
							text2,
							((DateTime)array[0]["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek),
							"   {1}  ",
							this.controller4Locate.GetDoorNameByReaderNO((int)((byte)array[0]["f_ReaderNO"])),
							"\r\n{2}:  "
						});
						TimeSpan timeSpan = ((DateTime)array[0]["f_ReadDate"]).Subtract((DateTime)array[1]["f_ReadDate"]);
						if (timeSpan.TotalDays >= 1.0)
						{
							text2 = text2 + (int)timeSpan.TotalDays + " {9}, ";
						}
						if (timeSpan.Hours > 0)
						{
							text2 = text2 + timeSpan.Hours + " {3}, ";
						}
						if (timeSpan.Minutes > 0)
						{
							text2 = text2 + timeSpan.Minutes + " {4} ";
						}
					}
					else
					{
						this.controller4Locate.GetInfoFromDBByControllerSN((int)array[0]["f_ControllerSN"]);
						if (array[0]["f_InOut"].ToString() == "0")
						{
							text2 = string.Concat(new string[]
							{
								text2,
								"{5}\r\n",
								((DateTime)array[0]["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek),
								"   {1}    ",
								this.controller4Locate.GetDoorNameByReaderNO((int)((byte)array[0]["f_ReaderNO"])),
								"\r\n{2}:  "
							});
						}
						else
						{
							text2 = string.Concat(new string[]
							{
								text2,
								((DateTime)array[0]["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek),
								"   {0}    ",
								this.controller4Locate.GetDoorNameByReaderNO((int)((byte)array[0]["f_ReaderNO"])),
								"\r\n{6}\r\n{2}:  "
							});
							TimeSpan timeSpan = DateTime.Now.Subtract((DateTime)array[0]["f_ReadDate"]);
							if (timeSpan.TotalSeconds < 0.0)
							{
								text2 += "[{7}] ";
							}
							else
							{
								if (timeSpan.TotalDays >= 1.0)
								{
									text2 = text2 + (int)timeSpan.TotalDays + " {9}, ";
								}
								if (timeSpan.Hours > 0)
								{
									text2 = text2 + timeSpan.Hours + " {3}, ";
								}
								if (timeSpan.Minutes > 0)
								{
									text2 = text2 + timeSpan.Minutes + " {4} ";
								}
							}
						}
					}
				}
				else
				{
					text2 += "{8}";
				}
				this.strInOutInfo = text2;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00151804 File Offset: 0x00150804
		private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.txtLocate.Text = string.Format(this.strInOutInfo, new object[]
			{
				CommonStr.strEnterInto,
				CommonStr.strGoOff,
				CommonStr.strStay,
				CommonStr.strHour,
				CommonStr.strMinutes,
				CommonStr.strEnterWithoutSwiping,
				CommonStr.strDontGoOff,
				CommonStr.strLaterThanNow,
				CommonStr.strNoSwiping,
				CommonStr.strDay
			});
			this.timer1.Enabled = false;
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00151897 File Offset: 0x00150897
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

		// Token: 0x060011E2 RID: 4578 RVA: 0x001518B8 File Offset: 0x001508B8
		private void btnQuery_Click(object sender, EventArgs e)
		{
			DataGridView dataGridView = this.dgvUsers;
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
				this.strUserId = dataRow["f_ConsumerID"].ToString();
				this.m_strGroupName = this.cbof_GroupID.Text;
				if (this.m_strGroupName == CommonStr.strAll)
				{
					this.m_strGroupName = "";
				}
				this.m_strUsers = dataRow["f_ConsumerName"].ToString();
				if (!this.backgroundWorker2.IsBusy)
				{
					this.backgroundWorker2.RunWorkerAsync();
					this.timer1.Enabled = true;
				}
			}
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x001519DC File Offset: 0x001509DC
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x001519EC File Offset: 0x001509EC
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
			if (this.dgvUsers.DataSource != null)
			{
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
					return;
				}
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
			}
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x00151C18 File Offset: 0x00150C18
		private void dfrmLocate_KeyDown(object sender, KeyEventArgs e)
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

		// Token: 0x060011E6 RID: 4582 RVA: 0x00151C88 File Offset: 0x00150C88
		private void dfrmPrivilegeCopy_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00151CA0 File Offset: 0x00150CA0
		private void dfrmPrivilegeCopy_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			try
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
				this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
				this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			if (!this.backgroundWorker1.IsBusy)
			{
				this.backgroundWorker1.RunWorkerAsync();
			}
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00151E00 File Offset: 0x00150E00
		private void dgvUsers_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrmLocate_KeyDown(this.dgvUsers, e);
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x00151E10 File Offset: 0x00150E10
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			this.ds = new DataSet("ReaderAndCardRecordtable");
			string text = " SELECT * FROM t_d_SwipeRecord WHERE 1<0 ";
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.ds, "cardrecord");
						}
					}
					goto IL_00E2;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.ds, "cardrecord");
					}
				}
			}
			IL_00E2:
			return icConsumerShare.getDt();
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00151F50 File Offset: 0x00150F50
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			this.dv.RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
			this.dvSelected.RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
			this.dgvUsers.AutoGenerateColumns = false;
			this.dgvUsers.DataSource = this.dv;
			int num = 0;
			while (num < this.dv.Table.Columns.Count && num < this.dgvUsers.ColumnCount)
			{
				this.dgvUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
				num++;
			}
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			wgTools.WriteLine("loadUserData End");
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x00152038 File Offset: 0x00151038
		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				if (!this.bStarting)
				{
					if (this.backgroundWorker2.IsBusy)
					{
						Cursor.Current = Cursors.WaitCursor;
					}
					else if (this.progressBar1.Value != 0 && this.progressBar1.Value != this.progressBar1.Maximum)
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
					this.timer1.Enabled = false;
					Cursor.Current = Cursors.Default;
					this.lblWait.Visible = false;
					this.btnQuery.Enabled = true;
					this.cbof_GroupID.Enabled = true;
					this.bStarting = false;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0015210C File Offset: 0x0015110C
		private void txtLocate_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0400202D RID: 8237
		private bool bEdit;

		// Token: 0x0400202E RID: 8238
		private DataView dv;

		// Token: 0x0400202F RID: 8239
		private DataView dvSelected;

		// Token: 0x04002030 RID: 8240
		private string m_strGroupName;

		// Token: 0x04002031 RID: 8241
		private string m_strUsers;

		// Token: 0x04002032 RID: 8242
		private string strInOutInfo;

		// Token: 0x04002033 RID: 8243
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04002034 RID: 8244
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04002035 RID: 8245
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04002036 RID: 8246
		private bool bStarting = true;

		// Token: 0x04002038 RID: 8248
		private DataSet ds = new DataSet("ReaderAndCardRecordtable");

		// Token: 0x04002039 RID: 8249
		private string strGroupFilter = "";

		// Token: 0x0400203A RID: 8250
		private string strUserId = "000";
	}
}
