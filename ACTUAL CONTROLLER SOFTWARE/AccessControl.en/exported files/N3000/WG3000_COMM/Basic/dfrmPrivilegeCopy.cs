using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
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
	// Token: 0x02000026 RID: 38
	public partial class dfrmPrivilegeCopy : frmN3000
	{
		// Token: 0x0600029C RID: 668 RVA: 0x00050F14 File Offset: 0x0004FF14
		public dfrmPrivilegeCopy()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers4Copy);
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
			this.progressBar1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00050FA8 File Offset: 0x0004FFA8
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

		// Token: 0x0600029E RID: 670 RVA: 0x00050FEC File Offset: 0x0004FFEC
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

		// Token: 0x0600029F RID: 671 RVA: 0x0005106C File Offset: 0x0005006C
		private void btnAddAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			if (this.strGroupFilter == "")
			{
				icConsumerShare.selectAllUsers();
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
				int rowCount = this.dgvSelectedUsers.RowCount;
				return;
			}
			wgTools.WriteLine("btnAddAllUsers_Click Start");
			this.dt = ((DataView)this.dgvUsers.DataSource).Table;
			this.dv1 = (DataView)this.dgvUsers.DataSource;
			this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
			this.dgvUsers.DataSource = null;
			this.dgvSelectedUsers.DataSource = null;
			if (this.strGroupFilter != "")
			{
				string rowFilter = this.dv1.RowFilter;
				string rowFilter2 = this.dv2.RowFilter;
				this.dv1.Dispose();
				this.dv2.Dispose();
				this.dv1 = null;
				this.dv2 = null;
				this.dt.BeginLoadData();
				this.dv = new DataView(this.dt);
				this.dv.RowFilter = this.strGroupFilter;
				for (int i = 0; i < this.dv.Count; i++)
				{
					this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
				}
				this.dt.EndLoadData();
				this.dv1 = new DataView(this.dt);
				this.dv1.RowFilter = rowFilter;
				this.dv2 = new DataView(this.dt);
				this.dv2.RowFilter = rowFilter2;
				this.dgvUsers.DataSource = this.dv1;
				this.dgvSelectedUsers.DataSource = this.dv2;
				wgTools.WriteLine("btnAddAllUsers_Click End");
				int count = this.dv2.Count;
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00051293 File Offset: 0x00050293
		private void btnAddOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
			int rowCount = this.dgvSelectedUsers.RowCount;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000512B4 File Offset: 0x000502B4
		private void btnAddOneUser4Copy_Click(object sender, EventArgs e)
		{
			if (this.dt4copy.Rows.Count < 10)
			{
				try
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
					icConsumerShare.setUpdateLog();
				}
				catch (Exception ex)
				{
					wgTools.WriteLine(ex.ToString());
				}
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0005140C File Offset: 0x0005040C
		private void btnAddPass_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnAddPass_Click_Acc(sender, e);
				return;
			}
			if (this.dgvSelectedUsers.RowCount > 0 && this.dgvSelectedUsers4Copy.RowCount > 0 && XMessageBox.Show(this.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.Cancel)
			{
				this.cn = new SqlConnection(wgAppConfig.dbConString);
				this.cm = new SqlCommand("", this.cn);
				bool flag = false;
				this.timer1.Enabled = true;
				this.dfrmWait1.Show();
				this.dfrmWait1.Refresh();
				try
				{
					DataView dataView = (DataView)this.dgvSelectedUsers.DataSource;
					DataView dataView2 = (DataView)this.dgvSelectedUsers4Copy.DataSource;
					if (this.cn.State != ConnectionState.Open)
					{
						this.cn.Open();
					}
					if (dataView2.Count > 1)
					{
						string text = "";
						for (int i = 0; i < dataView2.Count; i++)
						{
							text = text + dataView2[i]["f_ConsumerID"] + ",";
						}
						text += "0";
						string text2 = " SELECT  count([f_ConsumerID]) as subtotalUser ";
						text2 = text2 + " FROM t_d_Privilege  WHERE [f_ConsumerID] IN (" + text + " )  GROUP BY f_DoorID having count([f_ConsumerID]) > 1 ";
						this.cm.CommandText = text2;
						int num;
						if (int.TryParse(wgTools.SetObjToStr(this.cm.ExecuteScalar()), out num) && num > 1)
						{
							XMessageBox.Show(CommonStr.strUserprivilegCopyErr1, wgTools.MSGTITLE, MessageBoxButtons.OK);
							this.cn.Close();
							return;
						}
					}
					try
					{
						int num2 = 2000;
						int j = 0;
						this.progressBar1.Maximum = 2 * this.dgvSelectedUsers.RowCount;
						string text2;
						while (j < this.dgvSelectedUsers.Rows.Count)
						{
							string text = "";
							while (j < this.dgvSelectedUsers.Rows.Count)
							{
								text = text + ((DataView)this.dgvSelectedUsers.DataSource)[j]["f_ConsumerID"] + ",";
								j++;
								if (text.Length > num2)
								{
									break;
								}
							}
							text += "0";
							text2 = "DELETE FROM  [t_d_Privilege] ";
							text2 = text2 + " WHERE [f_ConsumerID] IN (" + text + " ) ";
							this.cm.CommandText = text2;
							int num = this.cm.ExecuteNonQuery();
							int count = this.dgvSelectedUsers.Rows.Count;
							this.progressBar1.Value = j;
							Application.DoEvents();
						}
						j = 0;
						while (j < this.dgvSelectedUsers.Rows.Count)
						{
							string text = "";
							while (j < this.dgvSelectedUsers.Rows.Count)
							{
								text = text + ((DataView)this.dgvSelectedUsers.DataSource)[j]["f_ConsumerID"] + ",";
								j++;
								if (text.Length > num2)
								{
									break;
								}
							}
							text += "0";
							for (int k = 0; k < dataView2.Count; k++)
							{
								text2 = "INSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])";
								text2 = string.Concat(new object[]
								{
									text2,
									" SELECT t_b_Consumer.f_ConsumerID, t_d_Privilege.f_DoorID,t_d_Privilege.[f_ControlSegID] , t_d_Privilege.[f_ControllerID], t_d_Privilege.[f_DoorNO]  FROM t_d_Privilege, t_b_Consumer  WHERE [t_b_Consumer].[f_ConsumerID] IN (",
									text,
									" )  AND (t_d_Privilege.f_ConsumerID)= ",
									dataView2[k]["f_ConsumerID"]
								});
								this.cm.CommandText = text2;
								int num = this.cm.ExecuteNonQuery();
							}
							this.progressBar1.Value = j + this.dgvSelectedUsers.Rows.Count;
							Application.DoEvents();
						}
						flag = true;
						string text3 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} ";
						text2 = string.Format(text3, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount);
						this.cm.CommandText = text2;
						this.cm.ExecuteNonQuery();
						this.logOperate(null);
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
				}
				catch (Exception ex2)
				{
					this.dfrmWait1.Hide();
					wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
					XMessageBox.Show(ex2.Message);
				}
				finally
				{
					if (this.cm != null)
					{
						this.cm.Dispose();
					}
					if (this.cn.State != ConnectionState.Closed)
					{
						this.cn.Close();
					}
					this.dfrmWait1.Hide();
				}
				this.progressBar1.Value = this.progressBar1.Maximum;
				Cursor.Current = Cursors.Default;
				if (!flag)
				{
					this.progressBar1.Value = 0;
					this.bEdit = true;
					wgAppConfig.wgLog(this.Text + CommonStr.strOperateFailed);
					XMessageBox.Show(this.Text + CommonStr.strOperateFailed);
					return;
				}
				wgAppConfig.wgLog(this.Text + CommonStr.strSuccessfully);
				XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
				base.DialogResult = DialogResult.OK;
				this.bEdit = true;
				base.Close();
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00051990 File Offset: 0x00050990
		private void btnAddPass_Click_Acc(object sender, EventArgs e)
		{
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
			if (this.dgvSelectedUsers.RowCount > 0 && this.dgvSelectedUsers4Copy.RowCount > 0 && XMessageBox.Show(this.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.Cancel)
			{
				oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				oleDbCommand = new OleDbCommand("", oleDbConnection);
				bool flag = false;
				this.timer1.Enabled = true;
				if (this.dgvSelectedUsers.Rows.Count > 1000)
				{
					this.dfrmWait1.Show();
					this.dfrmWait1.Refresh();
				}
				try
				{
					DataView dataView = (DataView)this.dgvSelectedUsers.DataSource;
					DataView dataView2 = (DataView)this.dgvSelectedUsers4Copy.DataSource;
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					if (dataView2.Count > 1)
					{
						string text = "";
						for (int i = 0; i < dataView2.Count; i++)
						{
							text = text + dataView2[i]["f_ConsumerID"] + ",";
						}
						text += "0";
						string text2 = " SELECT  count([f_ConsumerID]) as subtotalUser ";
						text2 = text2 + " FROM t_d_Privilege  WHERE [f_ConsumerID] IN (" + text + " )  GROUP BY f_DoorID having count([f_ConsumerID]) > 1 ";
						oleDbCommand.CommandText = text2;
						int num;
						if (int.TryParse(wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()), out num) && num > 1)
						{
							XMessageBox.Show(CommonStr.strUserprivilegCopyErr1, wgTools.MSGTITLE, MessageBoxButtons.OK);
							oleDbConnection.Close();
							return;
						}
					}
					try
					{
						int num2 = 2000;
						int j = 0;
						this.progressBar1.Maximum = 2 * this.dgvSelectedUsers.RowCount;
						string text2;
						while (j < this.dgvSelectedUsers.Rows.Count)
						{
							string text = "";
							while (j < this.dgvSelectedUsers.Rows.Count)
							{
								text = text + ((DataView)this.dgvSelectedUsers.DataSource)[j]["f_ConsumerID"] + ",";
								j++;
								if (text.Length > num2)
								{
									break;
								}
							}
							text += "0";
							text2 = "DELETE FROM  [t_d_Privilege] ";
							text2 = text2 + " WHERE [f_ConsumerID] IN (" + text + " ) ";
							oleDbCommand.CommandText = text2;
							int num = oleDbCommand.ExecuteNonQuery();
							int count = this.dgvSelectedUsers.Rows.Count;
							this.progressBar1.Value = j;
							Application.DoEvents();
						}
						j = 0;
						while (j < this.dgvSelectedUsers.Rows.Count)
						{
							string text = "";
							while (j < this.dgvSelectedUsers.Rows.Count)
							{
								text = text + ((DataView)this.dgvSelectedUsers.DataSource)[j]["f_ConsumerID"] + ",";
								j++;
								if (text.Length > num2)
								{
									break;
								}
							}
							text += "0";
							for (int k = 0; k < dataView2.Count; k++)
							{
								text2 = "INSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])";
								text2 = string.Concat(new object[]
								{
									text2,
									" SELECT t_b_Consumer.f_ConsumerID, t_d_Privilege.f_DoorID,t_d_Privilege.[f_ControlSegID] , t_d_Privilege.[f_ControllerID], t_d_Privilege.[f_DoorNO]  FROM t_d_Privilege, t_b_Consumer  WHERE [t_b_Consumer].[f_ConsumerID] IN (",
									text,
									" )  AND (t_d_Privilege.f_ConsumerID)= ",
									dataView2[k]["f_ConsumerID"]
								});
								oleDbCommand.CommandText = text2;
								int num = oleDbCommand.ExecuteNonQuery();
							}
							this.progressBar1.Value = j + this.dgvSelectedUsers.Rows.Count;
							Application.DoEvents();
						}
						flag = true;
						string text3 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} ";
						text2 = string.Format(text3, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount);
						oleDbCommand.CommandText = text2;
						oleDbCommand.ExecuteNonQuery();
						this.logOperate(null);
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
				}
				catch (Exception ex2)
				{
					wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
					XMessageBox.Show(ex2.Message);
				}
				finally
				{
					if (oleDbCommand != null)
					{
						oleDbCommand.Dispose();
					}
					if (oleDbConnection.State != ConnectionState.Closed)
					{
						oleDbConnection.Close();
					}
					this.dfrmWait1.Hide();
				}
				this.progressBar1.Value = this.progressBar1.Maximum;
				Cursor.Current = Cursors.Default;
				if (!flag)
				{
					this.progressBar1.Value = 0;
					this.bEdit = true;
					wgAppConfig.wgLog(this.Text + CommonStr.strOperateFailed);
					XMessageBox.Show(this.Text + CommonStr.strOperateFailed);
					return;
				}
				wgAppConfig.wgLog(this.Text + CommonStr.strSuccessfully);
				XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
				base.DialogResult = DialogResult.OK;
				this.bEdit = true;
				base.Close();
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00051ED4 File Offset: 0x00050ED4
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			if (this.dgvSelectedUsers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				wgTools.WriteLine("btnDelAllUsers_Click Start");
				icConsumerShare.selectNoneUsers();
				if (string.IsNullOrEmpty(this.strGroupFilter))
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
					((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
					return;
				}
				((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00051FB4 File Offset: 0x00050FB4
		private void btnDeleteOneUser4Copy_Click(object sender, EventArgs e)
		{
			if (this.dt4copy.Rows.Count != 0)
			{
				try
				{
					DataGridView dataGridView = this.dgvUsers;
					DataTable table = ((DataView)dataGridView.DataSource).Table;
					for (int i = 0; i < this.dt4copy.Rows.Count; i++)
					{
						DataRow dataRow = table.NewRow();
						if (dataRow != null)
						{
							DataRow dataRow2 = this.dt4copy.Rows[i];
							for (int j = 0; j < table.Columns.Count; j++)
							{
								dataRow[j] = dataRow2[j];
							}
							dataRow["f_Selected"] = icConsumerShare.iSelectedCurrentNoneMax;
							table.Rows.Add(dataRow);
							table.AcceptChanges();
						}
					}
					this.dt4copy.Rows.Clear();
					this.dt4copy.AcceptChanges();
					((DataView)dataGridView.DataSource).Sort = "f_ConsumerNO ASC";
				}
				catch (Exception ex)
				{
					wgTools.WriteLine(ex.ToString());
				}
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000520D0 File Offset: 0x000510D0
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000520E2 File Offset: 0x000510E2
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

		// Token: 0x060002A8 RID: 680 RVA: 0x00052102 File Offset: 0x00051102
		private void btnFind_Click(object sender, EventArgs e)
		{
			if (this.defaultFindControl == null)
			{
				this.defaultFindControl = this.dgvUsers;
			}
			wgAppConfig.findCall(this.dfrmFind1, this, this.defaultFindControl);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0005212A File Offset: 0x0005112A
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00052138 File Offset: 0x00051138
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
				}
				else
				{
					((DataView)this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
				}
				((DataView)this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00052388 File Offset: 0x00051388
		private void dfrmPrivilegeCopy_FormClosed(object sender, FormClosedEventArgs e)
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

		// Token: 0x060002AC RID: 684 RVA: 0x000523C0 File Offset: 0x000513C0
		private void dfrmPrivilegeCopy_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000523D8 File Offset: 0x000513D8
		private void dfrmPrivilegeCopy_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					wgAppConfig.findCall(this.dfrmFind1, this, this.dgvUsers);
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00052434 File Offset: 0x00051434
		private void dfrmPrivilegeCopy_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.dataGridViewTextBoxColumn6.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn6.HeaderText);
			this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
			this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
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
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[3]);
			wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers4Copy.Columns[3]);
			if (!this.backgroundWorker1.IsBusy)
			{
				this.backgroundWorker1.RunWorkerAsync();
			}
			this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			this.dgvSelectedUsers4Copy.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00052620 File Offset: 0x00051620
		private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.btnDelOneUser.PerformClick();
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00052630 File Offset: 0x00051630
		private void dgvUsers_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00052660 File Offset: 0x00051660
		private void dgvUsers_KeyDown(object sender, KeyEventArgs e)
		{
			this.dfrmPrivilegeCopy_KeyDown(this.dgvUsers, e);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0005266F File Offset: 0x0005166F
		private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.dgvSelectedUsers4Copy.Rows.Count == 0)
			{
				this.btnAddOneUser4Copy.PerformClick();
				return;
			}
			this.btnAddOneUser.PerformClick();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0005269A File Offset: 0x0005169A
		private DataTable loadUserData4BackWork()
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("loadUserData Start");
			icConsumerShare.loadUserData();
			return icConsumerShare.getDt();
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000526BC File Offset: 0x000516BC
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			this.dv.RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
			this.dvSelected.RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
			this.dgvUsers.AutoGenerateColumns = false;
			this.dgvUsers.DataSource = this.dv;
			this.dgvSelectedUsers.AutoGenerateColumns = false;
			this.dgvSelectedUsers.DataSource = this.dvSelected;
			this.dt4copy = dtUser.Clone();
			this.dgvSelectedUsers4Copy.AutoGenerateColumns = false;
			this.dgvSelectedUsers4Copy.DataSource = new DataView(this.dt4copy);
			int num = 0;
			while (num < this.dv.Table.Columns.Count && num < this.dgvUsers.ColumnCount)
			{
				this.dgvUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
				this.dgvSelectedUsers.Columns[num].DataPropertyName = dtUser.Columns[num].ColumnName;
				this.dgvSelectedUsers4Copy.Columns[num].DataPropertyName = this.dt4copy.Columns[num].ColumnName;
				num++;
			}
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			wgTools.WriteLine("loadUserData End");
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00052844 File Offset: 0x00051844
		private void logOperate(object sender)
		{
			string text = "";
			for (int i = 0; i <= Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedUsers.RowCount) - 1; i++)
			{
				text = text + ((DataView)this.dgvSelectedUsers.DataSource)[i]["f_ConsumerName"] + ",";
			}
			if (this.dgvSelectedUsers.RowCount > wgAppConfig.LogEventMaxCount)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"......(",
					this.dgvSelectedUsers.RowCount,
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
					this.dgvSelectedUsers.RowCount,
					")"
				});
			}
			wgAppConfig.wgLog(string.Format("{0}: {1} => {2}", this.Text.Replace("\r\n", ""), ((DataView)this.dgvSelectedUsers4Copy.DataSource)[0]["f_ConsumerName"].ToString(), text), EventLogEntryType.Information, null);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00052980 File Offset: 0x00051980
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
				else if (this.dgvUsers.DataSource == null)
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
					this.cbof_GroupID.Enabled = true;
					this.bStarting = false;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x04000523 RID: 1315
		private bool bEdit;

		// Token: 0x04000524 RID: 1316
		private SqlCommand cm;

		// Token: 0x04000525 RID: 1317
		private SqlConnection cn;

		// Token: 0x04000526 RID: 1318
		private Control defaultFindControl;

		// Token: 0x04000527 RID: 1319
		private DataTable dt;

		// Token: 0x04000528 RID: 1320
		private DataTable dt4copy;

		// Token: 0x04000529 RID: 1321
		private DataView dv;

		// Token: 0x0400052A RID: 1322
		private DataView dv1;

		// Token: 0x0400052B RID: 1323
		private DataView dv2;

		// Token: 0x0400052C RID: 1324
		private DataView dvSelected;

		// Token: 0x0400052D RID: 1325
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x0400052E RID: 1326
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x0400052F RID: 1327
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04000530 RID: 1328
		private bool bStarting = true;

		// Token: 0x04000532 RID: 1330
		private string strGroupFilter = "";
	}
}
