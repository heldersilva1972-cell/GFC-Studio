using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x020002FF RID: 767
	public partial class dfrmMeetingSet : frmN3000
	{
		// Token: 0x06001695 RID: 5781 RVA: 0x001CEAB4 File Offset: 0x001CDAB4
		public dfrmMeetingSet()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x001CEB28 File Offset: 0x001CDB28
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

		// Token: 0x06001697 RID: 5783 RVA: 0x001CEB6C File Offset: 0x001CDB6C
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

		// Token: 0x06001698 RID: 5784 RVA: 0x001CEBEC File Offset: 0x001CDBEC
		private void btnAdd_Click(object sender, EventArgs e)
		{
			int selectedIndex = this.cboIdentity.SelectedIndex;
			dfrmMeetingSet.selectObject(this.dgvUsers, "f_MeetingIdentity", selectedIndex, "f_Seat", this.txtSeat.Text.ToString(), "f_MeetingIdentityStr", this.cboIdentity.Items[selectedIndex].ToString());
			this.bNeedUpdateMeetingConsumer = true;
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x001CEC50 File Offset: 0x001CDC50
		private void btnAddAll_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			wgTools.WriteLine("btnAddAllUsers_Click Start");
			DataTable table = ((DataView)this.dgvUsers.DataSource).Table;
			DataView dataView = (DataView)this.dgvUsers.DataSource;
			DataView dataView2 = (DataView)this.dgvSelectedUsers.DataSource;
			this.dgvUsers.DataSource = null;
			this.dgvSelectedUsers.DataSource = null;
			int selectedIndex = this.cboIdentity.SelectedIndex;
			if (this.strGroupFilter == "")
			{
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if ((int)table.Rows[i]["f_Selected"] != 1)
					{
						table.Rows[i]["f_Selected"] = 1;
						table.Rows[i]["f_Seat"] = this.txtSeat.Text;
						table.Rows[i]["f_MeetingIdentity"] = selectedIndex;
						table.Rows[i]["f_MeetingIdentityStr"] = this.cboIdentity.Items[selectedIndex];
					}
				}
			}
			else
			{
				this.dv = new DataView(table);
				this.dv.RowFilter = this.strGroupFilter;
				for (int j = 0; j < this.dv.Count; j++)
				{
					if ((int)this.dv[j]["f_Selected"] != 1)
					{
						this.dv[j]["f_Selected"] = 1;
						this.dv[j]["f_Seat"] = this.txtSeat.Text;
						this.dv[j]["f_MeetingIdentity"] = selectedIndex;
						this.dv[j]["f_MeetingIdentityStr"] = this.cboIdentity.Items[selectedIndex];
					}
				}
			}
			this.dgvUsers.DataSource = dataView;
			this.dgvSelectedUsers.DataSource = dataView2;
			this.bNeedUpdateMeetingConsumer = true;
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x001CEEBC File Offset: 0x001CDEBC
		private void btnAddMeetingAdr_Click(object sender, EventArgs e)
		{
			try
			{
				string text = this.cbof_MeetingAdr.Text;
				using (dfrmMeetingAdr dfrmMeetingAdr = new dfrmMeetingAdr())
				{
					dfrmMeetingAdr.ShowDialog();
				}
				this.loadMeetingAdr();
				if (string.IsNullOrEmpty(text))
				{
					this.cbof_MeetingAdr.Text = text;
				}
				else
				{
					foreach (object obj in this.cbof_MeetingAdr.Items)
					{
						if (obj.ToString() == text)
						{
							this.cbof_MeetingAdr.Text = text;
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x001CEFAC File Offset: 0x001CDFAC
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x001CEFBC File Offset: 0x001CDFBC
		private void btnCreateInfo_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				string text = "";
				string text2 = text;
				string text3 = string.Concat(new string[]
				{
					text2,
					this.lblMeetingName.Text,
					"\t",
					this.txtf_MeetingName.Text,
					"\r\n"
				});
				string text4 = string.Concat(new string[]
				{
					text3,
					this.lblMeetingAddr.Text,
					"\t",
					this.cbof_MeetingAdr.Text,
					"\r\n"
				});
				string text5 = string.Concat(new string[]
				{
					text4,
					this.lblMeetingDateTime.Text,
					"\t",
					this.dtpMeetingDate.Text,
					" ",
					this.dtpMeetingTime.Text,
					"\r\n"
				});
				string text6 = string.Concat(new string[]
				{
					text5,
					this.lblSignBegin.Text,
					"\t",
					this.dtpStartTime.Text,
					"\r\n"
				});
				text = string.Concat(new string[]
				{
					text6,
					this.lblSignEnd.Text,
					"\t",
					this.dtpEndTime.Text,
					"\r\n"
				});
				if (this.txtf_Content.Text.Length > 0)
				{
					text = string.Concat(new string[]
					{
						text,
						this.lblContent.Text,
						"\r\n",
						this.txtf_Content.Text,
						"\r\n"
					});
				}
				if (this.txtf_Notes.Text.Length > 0)
				{
					text = string.Concat(new string[]
					{
						text,
						this.lblNotes.Text,
						"\r\n",
						this.txtf_Notes.Text,
						"\r\n"
					});
				}
				text = text + "\r\n\r\n" + Strings.Format(DateTime.Now, wgTools.DisplayFormat_DateYMD);
				string text7 = this.txtf_MeetingNo.Text + "-" + Strings.Format(DateTime.Now, "yyyy-MM-dd_HHmmss_ff") + ".txt";
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.FileName = text7;
					saveFileDialog.Filter = " (*.txt)|*.txt";
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						string fileName = saveFileDialog.FileName;
						using (StreamWriter streamWriter = new StreamWriter(fileName, true))
						{
							streamWriter.WriteLine(text);
						}
						text7 = fileName;
					}
				}
				Process.Start(new ProcessStartInfo
				{
					FileName = text7,
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			this.Cursor = Cursors.Default;
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x001CF350 File Offset: 0x001CE350
		private void btnDelAllUsers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			this.dt = ((DataView)this.dgvUsers.DataSource).Table;
			this.dv1 = (DataView)this.dgvUsers.DataSource;
			this.dv2 = (DataView)this.dgvSelectedUsers.DataSource;
			this.dgvUsers.DataSource = null;
			this.dgvSelectedUsers.DataSource = null;
			for (int i = 0; i < this.dt.Rows.Count; i++)
			{
				this.dt.Rows[i]["f_Selected"] = 0;
			}
			this.dgvUsers.DataSource = this.dv1;
			this.dgvSelectedUsers.DataSource = this.dv2;
			this.bNeedUpdateMeetingConsumer = true;
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x001CF435 File Offset: 0x001CE435
		private void btnDelOneUser_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelectedUsers);
			this.bNeedUpdateMeetingConsumer = true;
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x001CF449 File Offset: 0x001CE449
		private void btnFind_Click(object sender, EventArgs e)
		{
			if (this.defaultFindControl == null)
			{
				this.defaultFindControl = this.dgvUsers;
			}
			wgAppConfig.findCall(this.dfrmFind1, this, this.defaultFindControl);
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x001CF474 File Offset: 0x001CE474
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOk_Click_Acc(sender, e);
				return;
			}
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				if (this.txtf_MeetingName.Text.Trim() == "")
				{
					XMessageBox.Show(CommonStr.strMeetingNameIsEmpty);
					return;
				}
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				SqlCommand sqlCommand = new SqlCommand("", sqlConnection);
				if (sqlConnection.State == ConnectionState.Closed)
				{
					sqlConnection.Open();
				}
				if (this.curMeetingNo == "")
				{
					sqlCommand = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text), sqlConnection);
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						this.curMeetingNo = this.txtf_MeetingNo.Text;
					}
					sqlDataReader.Close();
				}
				if (this.curMeetingNo == "")
				{
					string text = "INSERT INTO t_d_Meeting ([f_MeetingNO], [f_MeetingName], [f_MeetingAdr], [f_MeetingDateTime], [f_SignStartTime], [f_SignEndTime], [f_Content], [f_Notes]) ";
					sqlCommand = new SqlCommand(string.Concat(new string[]
					{
						text,
						"VALUES(",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text),
						" , ",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingName.Text),
						" , ",
						wgTools.PrepareStrNUnicode(this.cbof_MeetingAdr.Text),
						" , ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpStartTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpEndTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						wgTools.PrepareStrNUnicode(this.txtf_Content.Text),
						" , ",
						wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
						")"
					}), sqlConnection);
					if (sqlCommand.ExecuteNonQuery() <= 0)
					{
					}
				}
				else
				{
					string text = "   Update [t_d_Meeting] ";
					sqlCommand = new SqlCommand(string.Concat(new string[]
					{
						text,
						" SET [f_MeetingName]=",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingName.Text),
						" , [f_MeetingAdr]=",
						wgTools.PrepareStrNUnicode(this.cbof_MeetingAdr.Text),
						" , [f_MeetingDateTime]=",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , [f_SignStartTime]=",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpStartTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , [f_SignEndTime]=",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpEndTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , [f_Content]=",
						wgTools.PrepareStrNUnicode(this.txtf_Content.Text),
						" , [f_Notes]=",
						wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
						" WHERE  f_MeetingNO= ",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text)
					}), sqlConnection);
					sqlCommand.ExecuteNonQuery();
				}
				if (this.bNeedUpdateMeetingConsumer)
				{
					this.dvGroupedConsumers = (DataView)this.dgvSelectedUsers.DataSource;
					if (this.dvGroupedConsumers.Count > 0)
					{
						string text = " Delete  FROM t_d_MeetingConsumer  WHERE f_MeetingNO= " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text);
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
						DataTable dataTable = new DataTable("t_d_Swipe");
						dataTable.Columns.Add("f_ConsumerID", Type.GetType("System.UInt32"));
						dataTable.Columns.Add("f_MeetingIdentity", Type.GetType("System.UInt32"));
						dataTable.Columns.Add("f_MeetingNO", Type.GetType("System.String"));
						dataTable.Columns.Add("f_Seat", Type.GetType("System.String"));
						SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString);
						SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection2);
						sqlBulkCopy.DestinationTableName = "t_d_MeetingConsumer";
						sqlBulkCopy.BulkCopyTimeout = wgAppConfig.dbCommandTimeout;
						sqlBulkCopy.ColumnMappings.Add("f_ConsumerID", "f_ConsumerID");
						sqlBulkCopy.ColumnMappings.Add("f_MeetingIdentity", "f_MeetingIdentity");
						sqlBulkCopy.ColumnMappings.Add("f_MeetingNO", "f_MeetingNO");
						sqlBulkCopy.ColumnMappings.Add("f_Seat", "f_Seat");
						for (int i = 0; i <= this.dvGroupedConsumers.Count - 1; i++)
						{
							DataRow dataRow = dataTable.NewRow();
							dataRow["f_MeetingNO"] = this.txtf_MeetingNo.Text;
							dataRow["f_ConsumerID"] = this.dvGroupedConsumers[i]["f_ConsumerID"];
							dataRow["f_MeetingIdentity"] = this.dvGroupedConsumers[i]["f_MeetingIdentity"];
							dataRow["f_Seat"] = this.dvGroupedConsumers[i]["f_Seat"];
							dataTable.Rows.Add(dataRow);
						}
						dataTable.AcceptChanges();
						sqlBulkCopy.BatchSize = dataTable.Rows.Count;
						sqlConnection2.Open();
						sqlBulkCopy.WriteToServer(dataTable);
						sqlBulkCopy.Close();
						dataTable.Dispose();
						sqlConnection2.Close();
					}
					else
					{
						new SqlCommand(" Delete  FROM t_d_MeetingConsumer  WHERE f_MeetingNO= " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text), sqlConnection).ExecuteNonQuery();
					}
				}
				if (sqlConnection.State == ConnectionState.Open)
				{
					sqlConnection.Close();
				}
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			Cursor.Current = cursor;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x001CFBDC File Offset: 0x001CEBDC
		private void btnOk_Click_Acc(object sender, EventArgs e)
		{
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				if (this.txtf_MeetingName.Text.Trim() == "")
				{
					XMessageBox.Show(CommonStr.strMeetingNameIsEmpty);
					return;
				}
				OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				if (this.curMeetingNo == "")
				{
					OleDbCommand oleDbCommand = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text), oleDbConnection);
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					if (oleDbDataReader.Read())
					{
						this.curMeetingNo = this.txtf_MeetingNo.Text;
					}
					oleDbDataReader.Close();
				}
				if (this.curMeetingNo == "")
				{
					string text = "INSERT INTO t_d_Meeting ([f_MeetingNO], [f_MeetingName], [f_MeetingAdr], [f_MeetingDateTime], [f_SignStartTime], [f_SignEndTime], [f_Content], [f_Notes]) ";
					OleDbCommand oleDbCommand = new OleDbCommand(string.Concat(new string[]
					{
						text,
						"VALUES(",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text),
						" , ",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingName.Text),
						" , ",
						wgTools.PrepareStrNUnicode(this.cbof_MeetingAdr.Text),
						" , ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpStartTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpEndTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						wgTools.PrepareStrNUnicode(this.txtf_Content.Text),
						" , ",
						wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
						")"
					}), oleDbConnection);
					if (oleDbCommand.ExecuteNonQuery() <= 0)
					{
					}
				}
				else
				{
					string text = "   Update [t_d_Meeting] ";
					OleDbCommand oleDbCommand = new OleDbCommand(string.Concat(new string[]
					{
						text,
						" SET [f_MeetingName]=",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingName.Text),
						" , [f_MeetingAdr]=",
						wgTools.PrepareStrNUnicode(this.cbof_MeetingAdr.Text),
						" , [f_MeetingDateTime]=",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , [f_SignStartTime]=",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpStartTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , [f_SignEndTime]=",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpEndTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , [f_Content]=",
						wgTools.PrepareStrNUnicode(this.txtf_Content.Text),
						" , [f_Notes]=",
						wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
						" WHERE  f_MeetingNO= ",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text)
					}), oleDbConnection);
					oleDbCommand.ExecuteNonQuery();
				}
				if (this.bNeedUpdateMeetingConsumer)
				{
					string text2 = "";
					this.dvGroupedConsumers = (DataView)this.dgvSelectedUsers.DataSource;
					if (this.dvGroupedConsumers.Count > 0)
					{
						for (int i = 0; i <= this.dvGroupedConsumers.Count - 1; i++)
						{
							text2 = text2 + this.dvGroupedConsumers[i]["f_ConsumerID"] + ",";
						}
						text2 += 0;
						string text = " Delete  FROM t_d_MeetingConsumer  WHERE f_MeetingNO= " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text);
						if (text2 != "")
						{
							text = text + " AND f_ConsumerID NOT IN (" + text2 + ")";
						}
						OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection);
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand = new OleDbCommand(text, oleDbConnection);
						text = " SELECT COUNT(*) FROM t_d_MeetingConsumer ";
						text = text + " WHERE  f_MeetingNO= " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text);
						oleDbCommand.CommandText = text;
						int num = (int)oleDbCommand.ExecuteScalar();
						int num2 = 0;
						for (int i = 0; i <= this.dvGroupedConsumers.Count - 1; i++)
						{
							if (num > 0)
							{
								text = " Update t_d_MeetingConsumer ";
								text = string.Concat(new object[]
								{
									text,
									" SET f_MeetingNO = ",
									wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text),
									", f_MeetingIdentity = ",
									this.dvGroupedConsumers[i]["f_MeetingIdentity"],
									", f_Seat = ",
									wgTools.PrepareStrNUnicode(this.dvGroupedConsumers[i]["f_Seat"]),
									" WHERE  f_MeetingNO= ",
									wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text),
									" AND f_ConsumerID = ",
									this.dvGroupedConsumers[i]["f_ConsumerID"]
								});
								oleDbCommand.CommandText = text;
								num2 = oleDbCommand.ExecuteNonQuery();
							}
							if (num2 <= 0)
							{
								text = " INSERT INTO t_d_MeetingConsumer ( [f_MeetingNO], [f_ConsumerID], [f_MeetingIdentity], [f_Seat])";
								text = string.Concat(new object[]
								{
									text,
									" VALUES( ",
									wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text),
									",  ",
									this.dvGroupedConsumers[i]["f_ConsumerID"],
									", ",
									this.dvGroupedConsumers[i]["f_MeetingIdentity"],
									", ",
									wgTools.PrepareStrNUnicode(this.dvGroupedConsumers[i]["f_Seat"]),
									" ) "
								});
								oleDbCommand.CommandText = text;
								oleDbCommand.ExecuteNonQuery();
							}
						}
					}
					else
					{
						string text = " Delete  FROM t_d_MeetingConsumer  WHERE f_MeetingNO= " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text);
						if (text2 != "")
						{
							text = text + " AND f_ConsumerID NOT IN (" + text2 + ")";
						}
						new OleDbCommand(text, oleDbConnection).ExecuteNonQuery();
					}
				}
				if (oleDbConnection.State == ConnectionState.Open)
				{
					oleDbConnection.Close();
				}
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			Cursor.Current = cursor;
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x001D03D0 File Offset: 0x001CF3D0
		private void btnOk_Click22(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.btnOk_Click_Acc(sender, e);
				return;
			}
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				if (this.txtf_MeetingName.Text.Trim() == "")
				{
					XMessageBox.Show(CommonStr.strMeetingNameIsEmpty);
					return;
				}
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				if (sqlConnection.State == ConnectionState.Closed)
				{
					sqlConnection.Open();
				}
				if (this.curMeetingNo == "")
				{
					SqlCommand sqlCommand = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text), sqlConnection);
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						this.curMeetingNo = this.txtf_MeetingNo.Text;
					}
					sqlDataReader.Close();
				}
				if (this.curMeetingNo == "")
				{
					string text = "INSERT INTO t_d_Meeting ([f_MeetingNO], [f_MeetingName], [f_MeetingAdr], [f_MeetingDateTime], [f_SignStartTime], [f_SignEndTime], [f_Content], [f_Notes]) ";
					SqlCommand sqlCommand = new SqlCommand(string.Concat(new string[]
					{
						text,
						"VALUES(",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text),
						" , ",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingName.Text),
						" , ",
						wgTools.PrepareStrNUnicode(this.cbof_MeetingAdr.Text),
						" , ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpStartTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpEndTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						wgTools.PrepareStrNUnicode(this.txtf_Content.Text),
						" , ",
						wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
						")"
					}), sqlConnection);
					if (sqlCommand.ExecuteNonQuery() <= 0)
					{
					}
				}
				else
				{
					string text = "   Update [t_d_Meeting] ";
					SqlCommand sqlCommand = new SqlCommand(string.Concat(new string[]
					{
						text,
						" SET [f_MeetingName]=",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingName.Text),
						" , [f_MeetingAdr]=",
						wgTools.PrepareStrNUnicode(this.cbof_MeetingAdr.Text),
						" , [f_MeetingDateTime]=",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , [f_SignStartTime]=",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpStartTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , [f_SignEndTime]=",
						wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpEndTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss"),
						" , [f_Content]=",
						wgTools.PrepareStrNUnicode(this.txtf_Content.Text),
						" , [f_Notes]=",
						wgTools.PrepareStrNUnicode(this.txtf_Notes.Text),
						" WHERE  f_MeetingNO= ",
						wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text)
					}), sqlConnection);
					sqlCommand.ExecuteNonQuery();
				}
				if (this.bNeedUpdateMeetingConsumer)
				{
					string text2 = "";
					this.dvGroupedConsumers = (DataView)this.dgvSelectedUsers.DataSource;
					if (this.dvGroupedConsumers.Count > 0)
					{
						for (int i = 0; i <= this.dvGroupedConsumers.Count - 1; i++)
						{
							text2 = text2 + this.dvGroupedConsumers[i]["f_ConsumerID"] + ",";
						}
						text2 += 0;
						string text = " Delete  FROM t_d_MeetingConsumer  WHERE f_MeetingNO= " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text);
						if (text2 != "")
						{
							text = text + " AND f_ConsumerID NOT IN (" + text2 + ")";
						}
						SqlCommand sqlCommand = new SqlCommand(text, sqlConnection);
						sqlCommand.ExecuteNonQuery();
						sqlCommand = new SqlCommand(text, sqlConnection);
						text = " SELECT COUNT(*) FROM t_d_MeetingConsumer ";
						text = text + " WHERE  f_MeetingNO= " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text);
						sqlCommand.CommandText = text;
						int num = (int)sqlCommand.ExecuteScalar();
						int num2 = 0;
						for (int i = 0; i <= this.dvGroupedConsumers.Count - 1; i++)
						{
							if (num > 0)
							{
								text = " Update t_d_MeetingConsumer ";
								text = string.Concat(new object[]
								{
									text,
									" SET f_MeetingNO = ",
									wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text),
									", f_MeetingIdentity = ",
									this.dvGroupedConsumers[i]["f_MeetingIdentity"],
									", f_Seat = ",
									wgTools.PrepareStrNUnicode(this.dvGroupedConsumers[i]["f_Seat"]),
									" WHERE  f_MeetingNO= ",
									wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text),
									" AND f_ConsumerID = ",
									this.dvGroupedConsumers[i]["f_ConsumerID"]
								});
								sqlCommand.CommandText = text;
								num2 = sqlCommand.ExecuteNonQuery();
							}
							if (num2 <= 0)
							{
								text = " INSERT INTO t_d_MeetingConsumer ( [f_MeetingNO], [f_ConsumerID], [f_MeetingIdentity], [f_Seat])";
								text = string.Concat(new object[]
								{
									text,
									" VALUES( ",
									wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text),
									",  ",
									this.dvGroupedConsumers[i]["f_ConsumerID"],
									", ",
									this.dvGroupedConsumers[i]["f_MeetingIdentity"],
									", ",
									wgTools.PrepareStrNUnicode(this.dvGroupedConsumers[i]["f_Seat"]),
									" ) "
								});
								sqlCommand.CommandText = text;
								sqlCommand.ExecuteNonQuery();
							}
						}
					}
					else
					{
						string text = " Delete  FROM t_d_MeetingConsumer  WHERE f_MeetingNO= " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text);
						if (text2 != "")
						{
							text = text + " AND f_ConsumerID NOT IN (" + text2 + ")";
						}
						new SqlCommand(text, sqlConnection).ExecuteNonQuery();
					}
				}
				if (sqlConnection.State == ConnectionState.Open)
				{
					sqlConnection.Close();
				}
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			Cursor.Current = cursor;
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x001D0BD4 File Offset: 0x001CFBD4
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x001D0BE4 File Offset: 0x001CFBE4
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
			if (this.dgvUsers.DataSource != null)
			{
				DataView dataView = (DataView)this.dgvUsers.DataSource;
				if (this.cbof_GroupID.SelectedIndex < 0 || (this.cbof_GroupID.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					dataView.RowFilter = "f_Selected = 0";
					this.strGroupFilter = "";
					return;
				}
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
						return;
					}
					dataView.RowFilter = "f_Selected = 0 ";
					string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
					dataView.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", groupQuery);
					this.strGroupFilter = string.Format("  {0} ", groupQuery);
				}
			}
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x001D0D93 File Offset: 0x001CFD93
		private void cbof_MeetingAdr_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_MeetingAdr);
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x001D0DA0 File Offset: 0x001CFDA0
		private void cbof_MeetingAdr_Enter(object sender, EventArgs e)
		{
			try
			{
				this.defaultFindControl = sender as Control;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x001D0DD0 File Offset: 0x001CFDD0
		private void cbof_MeetingAdr_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_MeetingAdr, this.cbof_MeetingAdr.Text);
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x001D0DEE File Offset: 0x001CFDEE
		private void dfrmMeetingSet_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x001D0DF0 File Offset: 0x001CFDF0
		private void dfrmMeetingSet_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.defaultFindControl == null)
					{
						this.defaultFindControl = this.dgvUsers;
					}
					wgAppConfig.findCall(this.dfrmFind1, this, this.defaultFindControl);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x001D0E70 File Offset: 0x001CFE70
		private void dfrmMeetingSet_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			if (this.cboIdentity.Items.Count >= 4)
			{
				this.cboIdentity.Items[0] = wgAppConfig.ReplaceSpecialWord((string)this.cboIdentity.Items[0], "KEY_strDelegate");
				this.cboIdentity.Items[1] = wgAppConfig.ReplaceSpecialWord((string)this.cboIdentity.Items[1], "KEY_strNonvotingDelegate");
				this.cboIdentity.Items[2] = wgAppConfig.ReplaceSpecialWord((string)this.cboIdentity.Items[2], "KEY_strInvitational");
				this.cboIdentity.Items[3] = wgAppConfig.ReplaceSpecialWord((string)this.cboIdentity.Items[3], "KEY_strAudit");
			}
			this.Text = wgAppConfig.ReplaceMeeting(this.Text);
			this.Label1.Text = wgAppConfig.ReplaceMeeting(this.Label1.Text);
			this.lblMeetingName.Text = wgAppConfig.ReplaceMeeting(this.lblMeetingName.Text);
			this.lblMeetingAddr.Text = wgAppConfig.ReplaceMeeting(this.lblMeetingAddr.Text);
			this.lblMeetingDateTime.Text = wgAppConfig.ReplaceMeeting(this.lblMeetingDateTime.Text);
			this.label3.Text = wgAppConfig.ReplaceMeeting(this.label3.Text);
			this.btnCreateInfo.Text = wgAppConfig.ReplaceMeeting(this.btnCreateInfo.Text);
			if (wgAppConfig.IsAccessDB)
			{
				this.dfrmMeetingSet_Load_Acc(sender, e);
				return;
			}
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			base.KeyPreview = true;
			try
			{
				this.cboIdentity.SelectedIndex = 0;
				this.loadGroupData();
				this.loadMeetingAdr();
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				if (sqlConnection.State == ConnectionState.Closed)
				{
					sqlConnection.Open();
				}
				if (this.curMeetingNo == "")
				{
					this.txtf_MeetingNo.Text = Strings.Format(DateTime.Now, "yyyyMMdd_HHmmss");
					this.dtpMeetingDate.Value = DateTime.Now.Date;
					this.dtpMeetingTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 9:00:00"));
					this.dtpStartTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 8:00:00"));
					this.dtpEndTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 17:30:00"));
				}
				else
				{
					this.txtf_MeetingNo.Text = this.curMeetingNo;
					SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text), sqlConnection).ExecuteReader();
					if (sqlDataReader.Read())
					{
						this.txtf_MeetingName.Text = wgTools.SetObjToStr(sqlDataReader["f_MeetingName"]);
						this.cbof_MeetingAdr.Text = wgTools.SetObjToStr(sqlDataReader["f_MeetingAdr"]);
						this.dtpMeetingDate.Value = DateTime.Parse(Strings.Format(sqlDataReader["f_MeetingDateTime"], "yyyy-MM-dd"));
						this.dtpMeetingTime.Value = DateTime.Parse(Strings.Format(sqlDataReader["f_MeetingDateTime"], "yyyy-MM-dd HH:mm:ss"));
						this.dtpStartTime.Value = DateTime.Parse(Strings.Format(sqlDataReader["f_SignStartTime"], "yyyy-MM-dd HH:mm:ss"));
						this.dtpEndTime.Value = DateTime.Parse(Strings.Format(sqlDataReader["f_SignEndTime"], "yyyy-MM-dd HH:mm:ss"));
						this.txtf_Content.Text = wgTools.SetObjToStr(sqlDataReader["f_Content"]);
						this.txtf_Notes.Text = wgTools.SetObjToStr(sqlDataReader["f_Notes"]);
					}
					sqlDataReader.Close();
				}
				this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				wgAppConfig.HideCardNOColumn(this.dgvUsers.Columns[5]);
				wgAppConfig.HideCardNOColumn(this.dgvSelectedUsers.Columns[5]);
				this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
				this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
				this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
				this.dtpMeetingTime.CustomFormat = "HH:mm";
				this.dtpMeetingTime.Format = DateTimePickerFormat.Custom;
				this.dtpStartTime.CustomFormat = "HH:mm";
				this.dtpStartTime.Format = DateTimePickerFormat.Custom;
				this.dtpEndTime.CustomFormat = "HH:mm";
				this.dtpEndTime.Format = DateTimePickerFormat.Custom;
				wgAppConfig.setDisplayFormatDate(this.dtpMeetingDate, wgTools.DisplayFormat_DateYMDWeek);
				this.backgroundWorker1.RunWorkerAsync();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			Cursor.Current = cursor;
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x001D13CC File Offset: 0x001D03CC
		private void dfrmMeetingSet_Load_Acc(object sender, EventArgs e)
		{
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			base.KeyPreview = true;
			try
			{
				this.cboIdentity.SelectedIndex = 0;
				this.loadGroupData();
				this.loadMeetingAdr();
				OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				if (this.curMeetingNo == "")
				{
					this.txtf_MeetingNo.Text = Strings.Format(DateTime.Now, "yyyyMMdd_HHmmss");
					this.dtpMeetingDate.Value = DateTime.Now.Date;
					this.dtpMeetingTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 9:00:00"));
					this.dtpStartTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 8:00:00"));
					this.dtpEndTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 17:30:00"));
				}
				else
				{
					this.txtf_MeetingNo.Text = this.curMeetingNo;
					OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text), oleDbConnection).ExecuteReader();
					if (oleDbDataReader.Read())
					{
						this.txtf_MeetingName.Text = wgTools.SetObjToStr(oleDbDataReader["f_MeetingName"]);
						this.cbof_MeetingAdr.Text = wgTools.SetObjToStr(oleDbDataReader["f_MeetingAdr"]);
						this.dtpMeetingDate.Value = DateTime.Parse(Strings.Format(oleDbDataReader["f_MeetingDateTime"], "yyyy-MM-dd"));
						this.dtpMeetingTime.Value = DateTime.Parse(Strings.Format(oleDbDataReader["f_MeetingDateTime"], "yyyy-MM-dd HH:mm:ss"));
						this.dtpStartTime.Value = DateTime.Parse(Strings.Format(oleDbDataReader["f_SignStartTime"], "yyyy-MM-dd HH:mm:ss"));
						this.dtpEndTime.Value = DateTime.Parse(Strings.Format(oleDbDataReader["f_SignEndTime"], "yyyy-MM-dd HH:mm:ss"));
						this.txtf_Content.Text = wgTools.SetObjToStr(oleDbDataReader["f_Content"]);
						this.txtf_Notes.Text = wgTools.SetObjToStr(oleDbDataReader["f_Notes"]);
					}
					oleDbDataReader.Close();
				}
				this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
				this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
				this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
				this.dtpMeetingTime.CustomFormat = "HH:mm";
				this.dtpMeetingTime.Format = DateTimePickerFormat.Custom;
				this.dtpStartTime.CustomFormat = "HH:mm";
				this.dtpStartTime.Format = DateTimePickerFormat.Custom;
				this.dtpEndTime.CustomFormat = "HH:mm";
				this.dtpEndTime.Format = DateTimePickerFormat.Custom;
				wgAppConfig.setDisplayFormatDate(this.dtpMeetingDate, wgTools.DisplayFormat_DateYMDWeek);
				this.backgroundWorker1.RunWorkerAsync();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			Cursor.Current = cursor;
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x001D1758 File Offset: 0x001D0758
		private void loadGroupData()
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

		// Token: 0x060016AD RID: 5805 RVA: 0x001D180C File Offset: 0x001D080C
		public void loadMeetingAdr()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadMeetingAdr_Acc();
				return;
			}
			try
			{
				this.cbof_MeetingAdr.Items.Clear();
				DataSet dataSet = new DataSet();
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				try
				{
					dataSet.Clear();
					SqlCommand sqlCommand = new SqlCommand("Select DISTINCT f_MeetingAdr  from t_d_MeetingAdr ", sqlConnection);
					new SqlDataAdapter(sqlCommand).Fill(dataSet, "t_d_MeetingAdr");
					if (dataSet.Tables["t_d_MeetingAdr"].Rows.Count > 0)
					{
						for (int i = 0; i <= dataSet.Tables["t_d_MeetingAdr"].Rows.Count - 1; i++)
						{
							this.cbof_MeetingAdr.Items.Add(dataSet.Tables["t_d_MeetingAdr"].Rows[i][0]);
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x001D194C File Offset: 0x001D094C
		public void loadMeetingAdr_Acc()
		{
			try
			{
				this.cbof_MeetingAdr.Items.Clear();
				DataSet dataSet = new DataSet();
				OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				try
				{
					dataSet.Clear();
					OleDbCommand oleDbCommand = new OleDbCommand("Select DISTINCT f_MeetingAdr  from t_d_MeetingAdr ", oleDbConnection);
					new OleDbDataAdapter(oleDbCommand).Fill(dataSet, "t_d_MeetingAdr");
					if (dataSet.Tables["t_d_MeetingAdr"].Rows.Count > 0)
					{
						for (int i = 0; i <= dataSet.Tables["t_d_MeetingAdr"].Rows.Count - 1; i++)
						{
							this.cbof_MeetingAdr.Items.Add(dataSet.Tables["t_d_MeetingAdr"].Rows[i][0]);
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x001D1A7C File Offset: 0x001D0A7C
		private DataTable loadUserData4BackWork()
		{
			Thread.Sleep(100);
			wgTools.WriteLine("loadUserData Start");
			this.dtUser1 = new DataTable();
			string text;
			if (wgAppConfig.IsAccessDB)
			{
				text = " SELECT  t_b_Consumer.f_ConsumerID ";
				text = text + " , f_MeetingIdentity,' ' as  f_MeetingIdentityStr, f_ConsumerNO, f_ConsumerName, f_CardNO  , f_Seat  ,IIF (t_d_MeetingConsumer.f_MeetingIdentity IS NULL, 0,  IIF (  t_d_MeetingConsumer.f_MeetingIdentity <0 , 0 , 1 )) AS f_Selected  , f_GroupID  FROM t_b_Consumer  LEFT OUTER JOIN t_d_MeetingConsumer ON ( t_b_Consumer.f_ConsumerID = t_d_MeetingConsumer.f_ConsumerID AND f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text) + ") ORDER BY f_ConsumerNO ASC ";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dtUser1);
						}
					}
					goto IL_011A;
				}
			}
			text = " SELECT  t_b_Consumer.f_ConsumerID ";
			text = text + " , f_MeetingIdentity,' ' as f_MeetingIdentityStr, f_ConsumerNO, f_ConsumerName, f_CardNO  , f_Seat  , CASE WHEN t_d_MeetingConsumer.f_MeetingIdentity IS NULL THEN 0 ELSE CASE WHEN t_d_MeetingConsumer.f_MeetingIdentity < 0 THEN 0 ELSE 1 END END AS f_Selected  , f_GroupID  FROM t_b_Consumer  LEFT OUTER JOIN t_d_MeetingConsumer ON ( t_b_Consumer.f_ConsumerID = t_d_MeetingConsumer.f_ConsumerID AND f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.txtf_MeetingNo.Text) + ") ORDER BY f_ConsumerNO ASC ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtUser1);
					}
				}
			}
			IL_011A:
			for (int i = 0; i < this.dtUser1.Rows.Count; i++)
			{
				DataRow dataRow = this.dtUser1.Rows[i];
				if (!string.IsNullOrEmpty(dataRow["f_MeetingIdentity"].ToString()) && (int)dataRow["f_MeetingIdentity"] >= 0)
				{
					dataRow["f_MeetingIdentityStr"] = this.cboIdentity.Items[(int)dataRow["f_MeetingIdentity"]].ToString();
				}
			}
			this.dtUser1.AcceptChanges();
			wgTools.WriteLine("da.Fill End");
			try
			{
				this.dtUser1.PrimaryKey = new DataColumn[] { this.dtUser1.Columns[0] };
			}
			catch (Exception)
			{
				throw;
			}
			dfrmMeetingSet.lastLoadUsers = icConsumerShare.getUpdateLog();
			dfrmMeetingSet.dtLastLoad = this.dtUser1;
			return this.dtUser1;
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x001D1CEC File Offset: 0x001D0CEC
		private void loadUserData4BackWorkComplete(DataTable dtUser)
		{
			this.dv = new DataView(dtUser);
			this.dvSelected = new DataView(dtUser);
			this.dv.RowFilter = "f_Selected = 0";
			this.dvSelected.RowFilter = "f_Selected > 0";
			this.dvSelected.Sort = "f_MeetingIdentity ASC, f_ConsumerNo ASC ";
			this.dgvUsers.AutoGenerateColumns = false;
			this.dgvUsers.DataSource = this.dv;
			this.dgvSelectedUsers.AutoGenerateColumns = false;
			this.dgvSelectedUsers.DataSource = this.dvSelected;
			for (int i = 0; i < this.dv.Table.Columns.Count; i++)
			{
				this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
				this.dgvSelectedUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
			}
			this.cbof_GroupID_SelectedIndexChanged(null, null);
			wgTools.WriteLine("loadUserData End");
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x001D1DFC File Offset: 0x001D0DFC
		public static void selectObject(DataGridView dgv, string secondField, int val, string secondField2, string val2, string secondField3, string val3)
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
				using (DataTable table = ((DataView)dgv.DataSource).Table)
				{
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
								if (secondField != "")
								{
									dataRow[secondField] = val;
								}
								if (secondField2 != "")
								{
									dataRow[secondField2] = val2;
								}
								if (secondField3 != "")
								{
									dataRow[secondField3] = val3;
								}
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
							if (secondField != "")
							{
								dataRow[secondField] = val;
							}
							if (secondField2 != "")
							{
								dataRow[secondField2] = val2;
							}
							if (secondField3 != "")
							{
								dataRow[secondField3] = val3;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x001D202C File Offset: 0x001D102C
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.dgvUsers.DataSource == null)
			{
				Cursor.Current = Cursors.WaitCursor;
				return;
			}
			this.timer1.Enabled = false;
			Cursor.Current = Cursors.Default;
			this.lblWait.Visible = false;
			this.groupBox1.Enabled = true;
			this.btnOK.Enabled = true;
		}

		// Token: 0x04002ECA RID: 11978
		private bool bNeedUpdateMeetingConsumer;

		// Token: 0x04002ECB RID: 11979
		private Control defaultFindControl;

		// Token: 0x04002ECC RID: 11980
		private DataTable dt;

		// Token: 0x04002ECD RID: 11981
		private DataTable dtUser1;

		// Token: 0x04002ECE RID: 11982
		private DataView dv;

		// Token: 0x04002ECF RID: 11983
		private DataView dv1;

		// Token: 0x04002ED0 RID: 11984
		private DataView dv2;

		// Token: 0x04002ED1 RID: 11985
		private DataView dvGroupedConsumers;

		// Token: 0x04002ED2 RID: 11986
		private DataView dvSelected;

		// Token: 0x04002ED3 RID: 11987
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04002ED4 RID: 11988
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04002ED5 RID: 11989
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04002ED6 RID: 11990
		public string curMeetingNo = "";

		// Token: 0x04002ED7 RID: 11991
		private DataSet ds = new DataSet();

		// Token: 0x04002ED8 RID: 11992
		private static DataTable dtLastLoad;

		// Token: 0x04002ED9 RID: 11993
		private static string lastLoadUsers = "";

		// Token: 0x04002EDA RID: 11994
		private string strGroupFilter = "";
	}
}
