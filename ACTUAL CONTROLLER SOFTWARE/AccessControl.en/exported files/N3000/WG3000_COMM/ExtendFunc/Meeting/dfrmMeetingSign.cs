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
using Microsoft.VisualBasic;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x02000300 RID: 768
	public partial class dfrmMeetingSign : frmN3000
	{
		// Token: 0x060016B6 RID: 5814 RVA: 0x001D3D94 File Offset: 0x001D2D94
		public dfrmMeetingSign()
		{
			this.InitializeComponent();
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x001D3E1C File Offset: 0x001D2E1C
		private void _loadPhoto(string strCardno, ref PictureBox pic)
		{
			try
			{
				string text;
				if (wgAppConfig.IsPhotoNameFromConsumerNO)
				{
					if (strCardno.Trim() == "")
					{
						text = null;
					}
					else
					{
						text = wgAppConfig.getPhotoFileNameByConsumerNO(strCardno.Trim());
					}
				}
				else if (strCardno.Trim() == "")
				{
					text = null;
				}
				else
				{
					text = wgAppConfig.getPhotoFileName(long.Parse(strCardno.Trim()));
				}
				Image image = pic.Image;
				wgAppConfig.ShowMyImage(text, ref image);
				pic.Image = image;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x001D3EC4 File Offset: 0x001D2EC4
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x001D3ECC File Offset: 0x001D2ECC
		private void Button6_Click(object sender, EventArgs e)
		{
			try
			{
				dfrmManualSign dfrmManualSign = new dfrmManualSign();
				MethodInvoker methodInvoker = new MethodInvoker(this.updateDisplay_Invoker);
				dfrmManualSign.curMeetingNo = this.curMeetingNo;
				if (dfrmManualSign.ShowDialog(this) == DialogResult.OK)
				{
					this.fillMeetingNum();
					base.BeginInvoke(methodInvoker);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x001D3F44 File Offset: 0x001D2F44
		private void Button7_Click(object sender, EventArgs e)
		{
			try
			{
				base.TopMost = false;
				dfrmMeetingStatDetail dfrmMeetingStatDetail = new dfrmMeetingStatDetail();
				MethodInvoker methodInvoker = new MethodInvoker(this.updateDisplay_Invoker);
				dfrmMeetingStatDetail.curMeetingNo = this.curMeetingNo;
				if (sender == this.Button1)
				{
					dfrmMeetingStatDetail.tabControl1.SelectedTab = dfrmMeetingStatDetail.tabControl1.TabPages[0];
				}
				if (sender == this.Button2)
				{
					dfrmMeetingStatDetail.tabControl1.SelectedTab = dfrmMeetingStatDetail.tabControl1.TabPages[1];
				}
				if (sender == this.Button3)
				{
					dfrmMeetingStatDetail.tabControl1.SelectedTab = dfrmMeetingStatDetail.tabControl1.TabPages[2];
				}
				if (sender == this.Button4)
				{
					dfrmMeetingStatDetail.tabControl1.SelectedTab = dfrmMeetingStatDetail.tabControl1.TabPages[3];
				}
				if (sender == this.Button5)
				{
					dfrmMeetingStatDetail.tabControl1.SelectedTab = dfrmMeetingStatDetail.tabControl1.TabPages[5];
				}
				if (sender == this.Button7)
				{
					dfrmMeetingStatDetail.tabControl1.SelectedTab = dfrmMeetingStatDetail.tabControl1.TabPages[5];
				}
				dfrmMeetingStatDetail.ShowDialog(this);
				this.fillMeetingNum();
				base.BeginInvoke(methodInvoker);
				base.TopMost = true;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x001D40B0 File Offset: 0x001D30B0
		private void button8_Click(object sender, EventArgs e)
		{
			using (dfrmInterfaceLock dfrmInterfaceLock = new dfrmInterfaceLock())
			{
				dfrmInterfaceLock.txtOperatorName.Text = icOperator.OperatorName;
				dfrmInterfaceLock.StartPosition = FormStartPosition.CenterScreen;
				dfrmInterfaceLock.ShowDialog(this);
			}
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x001D4100 File Offset: 0x001D3100
		private void dfrmMeetingSign_Closing(object sender, CancelEventArgs e)
		{
			try
			{
				if (this.frmWatch != null)
				{
					this.frmWatch.Close();
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x001D4154 File Offset: 0x001D3154
		private void dfrmMeetingSign_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.frmWatch != null)
				{
					this.frmWatch.Close();
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			try
			{
				if (this.startSlowThread != null && this.startSlowThread.IsAlive)
				{
					this.startSlowThread.Interrupt();
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			wgAppConfig.DisposeImage(this.picSwipe1.Image);
			wgAppConfig.DisposeImage(this.picSwipe2.Image);
			wgAppConfig.DisposeImage(this.picSwipe3.Image);
			wgAppConfig.DisposeImage(this.picSwipe4.Image);
			wgAppConfig.DisposeImage(this.picSwipe5.Image);
			wgAppConfig.DisposeImage(this.picSwipe6.Image);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x001D4248 File Offset: 0x001D3248
		private void dfrmMeetingSign_Load(object sender, EventArgs e)
		{
			this.Text = wgAppConfig.ReplaceMeeting(this.Text);
			this.Button6.Text = wgAppConfig.ReplaceMeeting(this.Button6.Text);
			this.Button7.Text = wgAppConfig.ReplaceMeeting(this.Button7.Text);
			this.Text = wgAppConfig.ReplaceMeeting(this.Text);
			this.Label2.Text = wgAppConfig.ReplaceSpecialWord(this.Label2.Text, "KEY_strDelegate");
			this.Label3.Text = wgAppConfig.ReplaceSpecialWord(this.Label3.Text, "KEY_strNonvotingDelegate");
			this.Label4.Text = wgAppConfig.ReplaceSpecialWord(this.Label4.Text, "KEY_strInvitational");
			this.Label5.Text = wgAppConfig.ReplaceSpecialWord(this.Label5.Text, "KEY_strAudit");
			if (wgAppConfig.IsAccessDB)
			{
				this.dfrmMeetingSign_Load_Acc(sender, e);
				return;
			}
			try
			{
				if (this.curMeetingNo == "")
				{
					base.Close();
				}
				else
				{
					SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
					if (sqlConnection.State == ConnectionState.Closed)
					{
						sqlConnection.Open();
					}
					SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo), sqlConnection).ExecuteReader();
					if (sqlDataReader.Read())
					{
						this.lblMeetingName.Text = wgTools.SetObjToStr(sqlDataReader["f_MeetingName"]);
						this.signStarttime = DateTime.Parse(Strings.Format(sqlDataReader["f_SignStartTime"], "yyyy-MM-dd HH:mm:ss"));
						this.signEndtime = DateTime.Parse(Strings.Format(sqlDataReader["f_SignEndTime"], "yyyy-MM-dd HH:mm:ss"));
						this.meetingAdr = wgTools.SetObjToStr(sqlDataReader["f_MeetingAdr"]);
					}
					sqlDataReader.Close();
					sqlConnection.Close();
					if (this.lblMeetingName.Text == "")
					{
						base.Close();
					}
					else
					{
						Application.DoEvents();
						this.startSlowThread = new Thread(new ThreadStart(this.startSlow));
						this.startSlowThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
						this.startSlowThread.IsBackground = true;
						this.startSlowThread.Start();
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x001D44C0 File Offset: 0x001D34C0
		private void dfrmMeetingSign_Load_Acc(object sender, EventArgs e)
		{
			try
			{
				if (this.curMeetingNo == "")
				{
					base.Close();
				}
				else
				{
					OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
					if (oleDbConnection.State == ConnectionState.Closed)
					{
						oleDbConnection.Open();
					}
					OleDbDataReader oleDbDataReader = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo), oleDbConnection).ExecuteReader();
					if (oleDbDataReader.Read())
					{
						this.lblMeetingName.Text = wgTools.SetObjToStr(oleDbDataReader["f_MeetingName"]);
						this.signStarttime = DateTime.Parse(Strings.Format(oleDbDataReader["f_SignStartTime"], "yyyy-MM-dd HH:mm:ss"));
						this.signEndtime = DateTime.Parse(Strings.Format(oleDbDataReader["f_SignEndTime"], "yyyy-MM-dd HH:mm:ss"));
						this.meetingAdr = wgTools.SetObjToStr(oleDbDataReader["f_MeetingAdr"]);
					}
					oleDbDataReader.Close();
					oleDbConnection.Close();
					if (this.lblMeetingName.Text == "")
					{
						base.Close();
					}
					else
					{
						Application.DoEvents();
						this.startSlowThread = new Thread(new ThreadStart(this.startSlow));
						this.startSlowThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
						this.startSlowThread.IsBackground = true;
						this.startSlowThread.Start();
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x001D4650 File Offset: 0x001D3650
		private void dfrmMeetingSign_SizeChanged(object sender, EventArgs e)
		{
			GroupBox[] array = new GroupBox[] { this.GroupBox2, this.GroupBox3, this.GroupBox4, this.GroupBox6, this.GroupBox7 };
			for (int i = 0; i < 5; i++)
			{
				array[i].Size = new Size(this.flowLayoutPanel1.Width / 5 - 8, this.flowLayoutPanel1.Height - 18);
			}
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x001D46CC File Offset: 0x001D36CC
		public void fillMeetingNum()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.fillMeetingNum_Acc();
				return;
			}
			try
			{
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				if (sqlConnection.State == ConnectionState.Closed)
				{
					sqlConnection.Open();
				}
				for (int i = 0; i <= 4; i++)
				{
					this.arrMeetingNum[0, i] = 0L;
					this.arrMeetingNum[1, i] = 0L;
					this.arrMeetingNum[2, i] = 0L;
					this.arrMeetingNum[3, i] = 0L;
					this.arrMeetingNum[4, i] = 0L;
				}
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT  a.*  FROM t_d_MeetingConsumer a, t_b_Consumer b WHERE a.f_ConsumerID=b.f_ConsumerID and a.f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo), sqlConnection);
				this.ds.Clear();
				sqlDataAdapter.Fill(this.ds, "t_d_MeetingConsumer");
				DataView dataView = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				for (int j = 0; j <= 3; j++)
				{
					dataView.RowFilter = " f_MeetingIdentity = " + j;
					if (dataView.Count > 0)
					{
						this.arrMeetingNum[j, 0] = (long)dataView.Count;
						dataView.RowFilter = " f_MeetingIdentity = " + j + " AND ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) ";
						this.arrMeetingNum[j, 1] = (long)dataView.Count;
						dataView.RowFilter = " f_MeetingIdentity = " + j + " AND (f_SignWay = 2) ";
						this.arrMeetingNum[j, 2] = (long)dataView.Count;
						this.arrMeetingNum[j, 3] = Math.Max(0L, this.arrMeetingNum[j, 0] - this.arrMeetingNum[j, 1] - this.arrMeetingNum[j, 2]);
						if (this.arrMeetingNum[j, 0] > 0L)
						{
							this.arrMeetingNum[j, 4] = this.arrMeetingNum[j, 1] * 1000L / this.arrMeetingNum[j, 0];
						}
					}
					this.arrMeetingNum[4, 0] += this.arrMeetingNum[j, 0];
					this.arrMeetingNum[4, 1] += this.arrMeetingNum[j, 1];
					this.arrMeetingNum[4, 2] += this.arrMeetingNum[j, 2];
					this.arrMeetingNum[4, 3] += this.arrMeetingNum[j, 3];
				}
				if (this.arrMeetingNum[4, 0] > 0L)
				{
					this.arrMeetingNum[4, 4] = this.arrMeetingNum[4, 1] * 1000L / this.arrMeetingNum[4, 0];
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x001D4A00 File Offset: 0x001D3A00
		public void fillMeetingNum_Acc()
		{
			try
			{
				OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				for (int i = 0; i <= 4; i++)
				{
					this.arrMeetingNum[0, i] = 0L;
					this.arrMeetingNum[1, i] = 0L;
					this.arrMeetingNum[2, i] = 0L;
					this.arrMeetingNum[3, i] = 0L;
					this.arrMeetingNum[4, i] = 0L;
				}
				OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter("SELECT  a.*  FROM t_d_MeetingConsumer a, t_b_Consumer b WHERE a.f_ConsumerID=b.f_ConsumerID and a.f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo), oleDbConnection);
				this.ds.Clear();
				oleDbDataAdapter.Fill(this.ds, "t_d_MeetingConsumer");
				DataView dataView = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
				for (int j = 0; j <= 3; j++)
				{
					dataView.RowFilter = " f_MeetingIdentity = " + j;
					if (dataView.Count > 0)
					{
						this.arrMeetingNum[j, 0] = (long)dataView.Count;
						dataView.RowFilter = " f_MeetingIdentity = " + j + " AND ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) ";
						this.arrMeetingNum[j, 1] = (long)dataView.Count;
						dataView.RowFilter = " f_MeetingIdentity = " + j + " AND (f_SignWay = 2) ";
						this.arrMeetingNum[j, 2] = (long)dataView.Count;
						this.arrMeetingNum[j, 3] = Math.Max(0L, this.arrMeetingNum[j, 0] - this.arrMeetingNum[j, 1] - this.arrMeetingNum[j, 2]);
						if (this.arrMeetingNum[j, 0] > 0L)
						{
							this.arrMeetingNum[j, 4] = this.arrMeetingNum[j, 1] * 1000L / this.arrMeetingNum[j, 0];
						}
					}
					this.arrMeetingNum[4, 0] += this.arrMeetingNum[j, 0];
					this.arrMeetingNum[4, 1] += this.arrMeetingNum[j, 1];
					this.arrMeetingNum[4, 2] += this.arrMeetingNum[j, 2];
					this.arrMeetingNum[4, 3] += this.arrMeetingNum[j, 3];
				}
				if (this.arrMeetingNum[4, 0] > 0L)
				{
					this.arrMeetingNum[4, 4] = this.arrMeetingNum[4, 1] * 1000L / this.arrMeetingNum[4, 0];
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x001D4D24 File Offset: 0x001D3D24
		public void fillMeetingRecord(string MeetingNo)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.fillMeetingRecord_Acc(MeetingNo);
				return;
			}
			try
			{
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				if (sqlConnection.State == ConnectionState.Closed)
				{
					sqlConnection.Open();
				}
				SqlCommand sqlCommand = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(MeetingNo), sqlConnection);
				SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
				if (sqlDataReader.Read())
				{
					this.signStarttime = (DateTime)sqlDataReader["f_SignStartTime"];
					this.signEndtime = (DateTime)sqlDataReader["f_SignEndTime"];
					this.meetingAdr = wgTools.SetObjToStr(sqlDataReader["f_MeetingAdr"]);
				}
				sqlDataReader.Close();
				if (this.lngDealtRecordID == -1L && this.meetingAdr != "")
				{
					this.queryReaderStr = "";
					string text = "Select t_b_reader.* from t_b_reader,t_d_MeetingAdr  ";
					sqlCommand = new SqlCommand(text + " , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  t_b_Reader.f_ReaderID  = t_d_MeetingAdr.f_ReaderID  AND t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.meetingAdr), sqlConnection);
					sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.HasRows)
					{
						while (sqlDataReader.Read())
						{
							if (this.queryReaderStr == "")
							{
								this.queryReaderStr = " f_ReaderID IN ( " + sqlDataReader["f_ReaderID"];
							}
							else
							{
								this.queryReaderStr = this.queryReaderStr + " , " + sqlDataReader["f_ReaderID"];
							}
							if (this.arrControllerID.IndexOf(sqlDataReader["f_ControllerID"]) < 0)
							{
								this.arrControllerID.Add(sqlDataReader["f_ControllerID"]);
							}
						}
						this.queryReaderStr += ")";
					}
					sqlDataReader.Close();
				}
				if (this.lngDealtRecordID == -1L)
				{
					this.lngDealtRecordID = 0L;
				}
				string text2 = "";
				text2 = string.Concat(new string[]
				{
					text2,
					" ([f_ReadDate]>= ",
					wgTools.PrepareStr(this.signStarttime, true, "yyyy-MM-dd H:mm:ss"),
					") AND ([f_ReadDate]<= ",
					wgTools.PrepareStr(this.signEndtime, true, "yyyy-MM-dd H:mm:ss"),
					")"
				});
				string text3 = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
				text3 = string.Concat(new string[]
				{
					text3,
					"        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll, t_d_MeetingConsumer.f_Seat, t_d_MeetingConsumer.f_MeetingIdentity, t_d_SwipeRecord.f_ConsumerID ",
					string.Format(" FROM ((t_b_Consumer INNER JOIN t_d_SwipeRecord ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) LEFT JOIN  t_d_MeetingConsumer on ( t_d_MeetingConsumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID AND  f_MeetingNO = " + wgTools.PrepareStrNUnicode(MeetingNo) + ") ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", new object[0]),
					" WHERE t_d_SwipeRecord.f_RecID > ",
					this.lngDealtRecordID.ToString(),
					" AND  t_d_SwipeRecord.f_ConsumerID IN (SELECT f_ConsumerID FROM t_d_MeetingConsumer WHERE f_SignWay=0 AND f_RecID =0 AND  f_MeetingNO = ",
					wgTools.PrepareStrNUnicode(MeetingNo),
					" )  "
				});
				if (this.queryReaderStr != "")
				{
					text3 = text3 + " AND " + this.queryReaderStr;
				}
				sqlCommand = new SqlCommand(text3 + " AND " + text2, sqlConnection);
				sqlDataReader = sqlCommand.ExecuteReader();
				ArrayList arrayList = new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				ArrayList arrayList3 = new ArrayList();
				if (sqlDataReader.HasRows)
				{
					while (sqlDataReader.Read())
					{
						int num = arrayList.IndexOf(sqlDataReader["f_ConsumerID"]);
						if (num < 0)
						{
							arrayList.Add(sqlDataReader["f_ConsumerID"]);
							arrayList2.Add((DateTime)sqlDataReader["f_ReadDate"]);
							arrayList3.Add(sqlDataReader["f_RecID"]);
						}
						else if ((DateTime)arrayList2[num] > (DateTime)sqlDataReader["f_ReadDate"])
						{
							arrayList2[num] = (DateTime)sqlDataReader["f_ReadDate"];
							arrayList3[num] = sqlDataReader["f_RecID"];
						}
					}
				}
				sqlDataReader.Close();
				if (arrayList.Count > 0)
				{
					for (int i = 0; i < arrayList.Count; i++)
					{
						string text = " UPDATE t_d_MeetingConsumer ";
						object obj = string.Concat(new object[]
						{
							text,
							" SET [f_SignRealTime] = ",
							wgTools.PrepareStr((DateTime)arrayList2[i], true, "yyyy-MM-dd H:mm:ss"),
							" ,[f_RecID] = ",
							arrayList3[i]
						});
						text = string.Concat(new object[]
						{
							obj,
							" WHERE f_ConsumerID = ",
							arrayList[i],
							" AND  f_MeetingNO = ",
							wgTools.PrepareStrNUnicode(MeetingNo)
						});
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
					}
				}
				sqlConnection.Close();
				this.lngDealtRecordID = (long)wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x001D522C File Offset: 0x001D422C
		public void fillMeetingRecord_Acc(string MeetingNo)
		{
			try
			{
				OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				OleDbCommand oleDbCommand = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStrNUnicode(MeetingNo), oleDbConnection);
				OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
				if (oleDbDataReader.Read())
				{
					this.signStarttime = (DateTime)oleDbDataReader["f_SignStartTime"];
					this.signEndtime = (DateTime)oleDbDataReader["f_SignEndTime"];
					this.meetingAdr = wgTools.SetObjToStr(oleDbDataReader["f_MeetingAdr"]);
				}
				oleDbDataReader.Close();
				if (this.lngDealtRecordID == -1L && this.meetingAdr != "")
				{
					this.queryReaderStr = "";
					string text = "Select t_b_reader.* from t_b_reader,t_d_MeetingAdr  ";
					oleDbDataReader = new OleDbCommand(text + " , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  t_b_Reader.f_ReaderID  = t_d_MeetingAdr.f_ReaderID  AND t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStrNUnicode(this.meetingAdr), oleDbConnection).ExecuteReader();
					if (oleDbDataReader.HasRows)
					{
						while (oleDbDataReader.Read())
						{
							if (this.queryReaderStr == "")
							{
								this.queryReaderStr = " f_ReaderID IN ( " + oleDbDataReader["f_ReaderID"];
							}
							else
							{
								this.queryReaderStr = this.queryReaderStr + " , " + oleDbDataReader["f_ReaderID"];
							}
							if (this.arrControllerID.IndexOf(oleDbDataReader["f_ControllerID"]) < 0)
							{
								this.arrControllerID.Add(oleDbDataReader["f_ControllerID"]);
							}
						}
						this.queryReaderStr += ")";
					}
					oleDbDataReader.Close();
				}
				if (this.lngDealtRecordID == -1L)
				{
					this.lngDealtRecordID = 0L;
				}
				string text2 = "";
				text2 = string.Concat(new string[]
				{
					text2,
					" ([f_ReadDate]>= ",
					wgTools.PrepareStr(this.signStarttime, true, "yyyy-MM-dd H:mm:ss"),
					") AND ([f_ReadDate]<= ",
					wgTools.PrepareStr(this.signEndtime, true, "yyyy-MM-dd H:mm:ss"),
					")"
				});
				string text3 = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
				text3 = string.Concat(new string[]
				{
					text3,
					"        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll, t_d_MeetingConsumer.f_Seat, t_d_MeetingConsumer.f_MeetingIdentity, t_d_SwipeRecord.f_ConsumerID ",
					string.Format(" FROM ((t_b_Consumer INNER JOIN t_d_SwipeRecord ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) LEFT JOIN  t_d_MeetingConsumer on ( t_d_MeetingConsumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID AND  f_MeetingNO = " + wgTools.PrepareStrNUnicode(MeetingNo) + ") ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", new object[0]),
					" WHERE t_d_SwipeRecord.f_RecID > ",
					this.lngDealtRecordID.ToString(),
					" AND  t_d_SwipeRecord.f_ConsumerID IN (SELECT f_ConsumerID FROM t_d_MeetingConsumer WHERE f_SignWay=0 AND f_RecID =0 AND  f_MeetingNO = ",
					wgTools.PrepareStrNUnicode(MeetingNo),
					" )  "
				});
				if (this.queryReaderStr != "")
				{
					text3 = text3 + " AND " + this.queryReaderStr;
				}
				oleDbCommand = new OleDbCommand(text3 + " AND " + text2, oleDbConnection);
				oleDbDataReader = oleDbCommand.ExecuteReader();
				ArrayList arrayList = new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				ArrayList arrayList3 = new ArrayList();
				if (oleDbDataReader.HasRows)
				{
					while (oleDbDataReader.Read())
					{
						int num = arrayList.IndexOf(oleDbDataReader["f_ConsumerID"]);
						if (num < 0)
						{
							arrayList.Add(oleDbDataReader["f_ConsumerID"]);
							arrayList2.Add((DateTime)oleDbDataReader["f_ReadDate"]);
							arrayList3.Add(oleDbDataReader["f_RecID"]);
						}
						else if ((DateTime)arrayList2[num] > (DateTime)oleDbDataReader["f_ReadDate"])
						{
							arrayList2[num] = (DateTime)oleDbDataReader["f_ReadDate"];
							arrayList3[num] = oleDbDataReader["f_RecID"];
						}
					}
				}
				oleDbDataReader.Close();
				if (arrayList.Count > 0)
				{
					for (int i = 0; i < arrayList.Count; i++)
					{
						string text = " UPDATE t_d_MeetingConsumer ";
						object obj = string.Concat(new object[]
						{
							text,
							" SET [f_SignRealTime] = ",
							wgTools.PrepareStr((DateTime)arrayList2[i], true, "yyyy-MM-dd H:mm:ss"),
							" ,[f_RecID] = ",
							arrayList3[i]
						});
						text = string.Concat(new object[]
						{
							obj,
							" WHERE f_ConsumerID = ",
							arrayList[i],
							" AND  f_MeetingNO = ",
							wgTools.PrepareStrNUnicode(MeetingNo)
						});
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
					}
				}
				oleDbConnection.Close();
				this.lngDealtRecordID = (long)wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x001D5724 File Offset: 0x001D4724
		public void getNewMeetingRecord()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.getNewMeetingRecord_Acc();
				return;
			}
			try
			{
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				if (sqlConnection.State == ConnectionState.Closed)
				{
					sqlConnection.Open();
				}
				string text = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
				text = string.Concat(new string[]
				{
					text,
					"        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll, t_d_MeetingConsumer.f_Seat, t_d_MeetingConsumer.f_MeetingIdentity ",
					string.Format(" FROM ((t_b_Consumer INNER JOIN t_d_SwipeRecord ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) LEFT JOIN  t_d_MeetingConsumer on ( t_d_MeetingConsumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID AND  f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo) + ") ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", new object[0]),
					" WHERE t_d_SwipeRecord.f_RecID > ",
					this.lngDealtRecordID.ToString()
				});
				if (this.queryReaderStr != "")
				{
					text = text + " AND " + this.queryReaderStr;
				}
				string text2 = text;
				SqlDataReader sqlDataReader = new SqlCommand(text2, sqlConnection).ExecuteReader();
				bool flag = false;
				if (sqlDataReader.HasRows)
				{
					while (sqlDataReader.Read())
					{
						string text3 = wgTools.SetObjToStr(sqlDataReader["f_ConsumerName"]);
						MjRec mjRec = new MjRec(sqlDataReader["f_RecordAll"].ToString());
						if (mjRec.IsSwipeRecord)
						{
							flag = true;
							if (text3 != "")
							{
								if (wgTools.SetObjToStr(sqlDataReader["f_MeetingIdentity"]) != "")
								{
									text3 = text3 + "." + frmMeetings.getStrMeetingIdentity(long.Parse(sqlDataReader["f_MeetingIdentity"].ToString()));
								}
								this.arrSignedUser.Add(text3);
								this.arrSignedSeat.Add(wgTools.SetObjToStr(sqlDataReader["f_Seat"]));
								if (wgAppConfig.IsPhotoNameFromConsumerNO)
								{
									this.arrSignedCardNo.Add(wgTools.SetObjToStr(sqlDataReader["f_ConsumerNO"]));
								}
								else
								{
									this.arrSignedCardNo.Add(wgTools.SetObjToStr(sqlDataReader["f_CardNO"]));
								}
							}
							else
							{
								text3 = wgTools.SetObjToStr(sqlDataReader["f_CardNO"]);
								this.arrSignedUser.Add(text3);
								this.arrSignedSeat.Add("!!!");
								if (wgAppConfig.IsPhotoNameFromConsumerNO)
								{
									this.arrSignedCardNo.Add(wgTools.SetObjToStr(sqlDataReader["f_ConsumerNO"]));
								}
								else
								{
									this.arrSignedCardNo.Add(wgTools.SetObjToStr(sqlDataReader["f_CardNO"]));
								}
							}
						}
					}
					if (this.arrSignedUser.Count > 100)
					{
						while (this.arrSignedUser.Count > 50)
						{
							this.arrSignedUser.RemoveAt(0);
							this.arrSignedSeat.RemoveAt(0);
							this.arrSignedCardNo.RemoveAt(0);
						}
					}
				}
				sqlDataReader.Close();
				sqlConnection.Close();
				if (flag)
				{
					if (this.arrSignedUser.Count > 0)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser1.Text = this.arrSignedUser[num - 1].ToString();
						this.txtSwipeSeat1.Text = this.arrSignedSeat[num - 1].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 1].ToString(), ref this.picSwipe1);
					}
					if (this.arrSignedUser.Count > 1)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser2.Text = this.arrSignedUser[num - 2].ToString();
						this.txtSwipeSeat2.Text = this.arrSignedSeat[num - 2].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 2].ToString(), ref this.picSwipe2);
					}
					if (this.arrSignedUser.Count > 2)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser3.Text = this.arrSignedUser[num - 3].ToString();
						this.txtSwipeSeat3.Text = this.arrSignedSeat[num - 3].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 3].ToString(), ref this.picSwipe3);
					}
					if (this.arrSignedUser.Count > 3)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser4.Text = this.arrSignedUser[num - 4].ToString();
						this.txtSwipeSeat4.Text = this.arrSignedSeat[num - 4].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 4].ToString(), ref this.picSwipe4);
					}
					if (this.arrSignedUser.Count > 4)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser5.Text = this.arrSignedUser[num - 5].ToString();
						this.txtSwipeSeat5.Text = this.arrSignedSeat[num - 5].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 5].ToString(), ref this.picSwipe5);
					}
					if (this.arrSignedUser.Count > 5)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser6.Text = this.arrSignedUser[num - 6].ToString();
						this.txtSwipeSeat6.Text = this.arrSignedSeat[num - 6].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 6].ToString(), ref this.picSwipe6);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x001D5CEC File Offset: 0x001D4CEC
		public void getNewMeetingRecord_Acc()
		{
			try
			{
				OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				string text = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_CardNO,  ";
				text = string.Concat(new string[]
				{
					text,
					"        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName,         t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate,         t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll, t_d_MeetingConsumer.f_Seat, t_d_MeetingConsumer.f_MeetingIdentity ",
					string.Format(" FROM ((t_b_Consumer INNER JOIN t_d_SwipeRecord ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) LEFT JOIN  t_d_MeetingConsumer on ( t_d_MeetingConsumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID AND  f_MeetingNO = " + wgTools.PrepareStrNUnicode(this.curMeetingNo) + ") ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", new object[0]),
					" WHERE t_d_SwipeRecord.f_RecID > ",
					this.lngDealtRecordID.ToString()
				});
				if (this.queryReaderStr != "")
				{
					text = text + " AND " + this.queryReaderStr;
				}
				string text2 = text;
				OleDbDataReader oleDbDataReader = new OleDbCommand(text2, oleDbConnection).ExecuteReader();
				bool flag = false;
				if (oleDbDataReader.HasRows)
				{
					while (oleDbDataReader.Read())
					{
						string text3 = wgTools.SetObjToStr(oleDbDataReader["f_ConsumerName"]);
						MjRec mjRec = new MjRec(oleDbDataReader["f_RecordAll"].ToString());
						if (mjRec.IsSwipeRecord)
						{
							flag = true;
							if (text3 != "")
							{
								if (wgTools.SetObjToStr(oleDbDataReader["f_MeetingIdentity"]) != "")
								{
									text3 = text3 + "." + frmMeetings.getStrMeetingIdentity(long.Parse(oleDbDataReader["f_MeetingIdentity"].ToString()));
								}
								this.arrSignedUser.Add(text3);
								this.arrSignedSeat.Add(wgTools.SetObjToStr(oleDbDataReader["f_Seat"]));
								if (wgAppConfig.IsPhotoNameFromConsumerNO)
								{
									this.arrSignedCardNo.Add(wgTools.SetObjToStr(oleDbDataReader["f_ConsumerNO"]));
								}
								else
								{
									this.arrSignedCardNo.Add(wgTools.SetObjToStr(oleDbDataReader["f_CardNO"]));
								}
							}
							else
							{
								text3 = wgTools.SetObjToStr(oleDbDataReader["f_CardNO"]);
								this.arrSignedUser.Add(text3);
								this.arrSignedSeat.Add("!!!");
								if (wgAppConfig.IsPhotoNameFromConsumerNO)
								{
									this.arrSignedCardNo.Add(wgTools.SetObjToStr(oleDbDataReader["f_ConsumerNO"]));
								}
								else
								{
									this.arrSignedCardNo.Add(wgTools.SetObjToStr(oleDbDataReader["f_CardNO"]));
								}
							}
						}
					}
					if (this.arrSignedUser.Count > 100)
					{
						while (this.arrSignedUser.Count > 50)
						{
							this.arrSignedUser.RemoveAt(0);
							this.arrSignedSeat.RemoveAt(0);
							this.arrSignedCardNo.RemoveAt(0);
						}
					}
				}
				oleDbDataReader.Close();
				oleDbConnection.Close();
				if (flag)
				{
					if (this.arrSignedUser.Count > 0)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser1.Text = this.arrSignedUser[num - 1].ToString();
						this.txtSwipeSeat1.Text = this.arrSignedSeat[num - 1].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 1].ToString(), ref this.picSwipe1);
					}
					if (this.arrSignedUser.Count > 1)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser2.Text = this.arrSignedUser[num - 2].ToString();
						this.txtSwipeSeat2.Text = this.arrSignedSeat[num - 2].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 2].ToString(), ref this.picSwipe2);
					}
					if (this.arrSignedUser.Count > 2)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser3.Text = this.arrSignedUser[num - 3].ToString();
						this.txtSwipeSeat3.Text = this.arrSignedSeat[num - 3].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 3].ToString(), ref this.picSwipe3);
					}
					if (this.arrSignedUser.Count > 3)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser4.Text = this.arrSignedUser[num - 4].ToString();
						this.txtSwipeSeat4.Text = this.arrSignedSeat[num - 4].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 4].ToString(), ref this.picSwipe4);
					}
					if (this.arrSignedUser.Count > 4)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser5.Text = this.arrSignedUser[num - 5].ToString();
						this.txtSwipeSeat5.Text = this.arrSignedSeat[num - 5].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 5].ToString(), ref this.picSwipe5);
					}
					if (this.arrSignedUser.Count > 5)
					{
						int num = this.arrSignedUser.Count;
						this.txtSwipeUser6.Text = this.arrSignedUser[num - 6].ToString();
						this.txtSwipeSeat6.Text = this.arrSignedSeat[num - 6].ToString();
						this._loadPhoto(this.arrSignedCardNo[num - 6].ToString(), ref this.picSwipe6);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x001D62A8 File Offset: 0x001D52A8
		private void GroupBox1_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x001D62AC File Offset: 0x001D52AC
		public void startSlow()
		{
			MethodInvoker methodInvoker = new MethodInvoker(this.startSlow_Invoker);
			try
			{
				Application.DoEvents();
				Thread.Sleep(1000);
				this.fillMeetingRecord(this.curMeetingNo);
				this.fillMeetingNum();
				base.BeginInvoke(methodInvoker);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x001D6320 File Offset: 0x001D5320
		public void startSlow_Invoker()
		{
			try
			{
				int num = 0;
				this.txtA0.Text = this.arrMeetingNum[num, 0].ToString();
				this.txtA1.Text = this.arrMeetingNum[num, 1].ToString();
				this.txtA2.Text = this.arrMeetingNum[num, 2].ToString();
				this.txtA3.Text = this.arrMeetingNum[num, 3].ToString();
				this.txtA4.Text = (this.arrMeetingNum[num, 4] / 10L).ToString() + "%";
				num = 1;
				this.txtB0.Text = this.arrMeetingNum[num, 0].ToString();
				this.txtB1.Text = this.arrMeetingNum[num, 1].ToString();
				this.txtB2.Text = this.arrMeetingNum[num, 2].ToString();
				this.txtB3.Text = this.arrMeetingNum[num, 3].ToString();
				this.txtB4.Text = (this.arrMeetingNum[num, 4] / 10L).ToString() + "%";
				num = 2;
				this.txtC0.Text = this.arrMeetingNum[num, 0].ToString();
				this.txtC1.Text = this.arrMeetingNum[num, 1].ToString();
				this.txtC2.Text = this.arrMeetingNum[num, 2].ToString();
				this.txtC3.Text = this.arrMeetingNum[num, 3].ToString();
				this.txtC4.Text = (this.arrMeetingNum[num, 4] / 10L).ToString() + "%";
				num = 3;
				this.txtD0.Text = this.arrMeetingNum[num, 0].ToString();
				this.txtD1.Text = this.arrMeetingNum[num, 1].ToString();
				this.txtD2.Text = this.arrMeetingNum[num, 2].ToString();
				this.txtD3.Text = this.arrMeetingNum[num, 3].ToString();
				this.txtD4.Text = (this.arrMeetingNum[num, 4] / 10L).ToString() + "%";
				num = 4;
				this.txtE0.Text = this.arrMeetingNum[num, 0].ToString();
				this.txtE1.Text = this.arrMeetingNum[num, 1].ToString();
				this.txtE2.Text = this.arrMeetingNum[num, 2].ToString();
				this.txtE3.Text = this.arrMeetingNum[num, 3].ToString();
				this.txtE4.Text = (this.arrMeetingNum[num, 4] / 10L).ToString() + "%";
				if (this.frmWatch == null)
				{
					this.frmWatch = new frmConsole();
					this.frmWatch.arrSelectDoors4Sign = (ArrayList)this.arrControllerID.Clone();
					this.frmWatch.WindowState = FormWindowState.Minimized;
					this.frmWatch.Show();
					this.frmWatch.Visible = false;
					this.frmWatch.directToRealtimeGet();
				}
				foreach (object obj in this.GroupBox5.Controls)
				{
					if (obj.GetType().Name.ToString() == "TextBox" && ((TextBox)obj).Text == "0")
					{
						((TextBox)obj).Text = "";
					}
				}
				if (string.IsNullOrEmpty(this.txtA0.Text))
				{
					this.txtA4.Text = "";
				}
				if (string.IsNullOrEmpty(this.txtB0.Text))
				{
					this.txtB4.Text = "";
				}
				if (string.IsNullOrEmpty(this.txtC0.Text))
				{
					this.txtC4.Text = "";
				}
				if (string.IsNullOrEmpty(this.txtD0.Text))
				{
					this.txtD4.Text = "";
				}
				if (string.IsNullOrEmpty(this.txtE0.Text))
				{
					this.txtE4.Text = "";
				}
				this.Timer2.Enabled = true;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			this.TimerStartSlow.Enabled = false;
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x001D6864 File Offset: 0x001D5864
		private void Timer1_Tick(object sender, EventArgs e)
		{
			this.Timer1.Enabled = false;
			try
			{
				this.lblTime.Text = Strings.Format(DateTime.Now, "HH:mm:ss");
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			this.Timer1.Enabled = true;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x001D68DC File Offset: 0x001D58DC
		private void Timer2_Tick(object sender, EventArgs e)
		{
			this.Timer2.Enabled = false;
			try
			{
				this.cntTimer2++;
				if (this.frmWatch.getAllInfoRowsCount() != this.lastinfoRowsCount || this.cntTimer2 > 600 / this.Timer2.Interval)
				{
					this.cntTimer2 = 0;
					this.lastinfoRowsCount = this.frmWatch.getAllInfoRowsCount();
					MethodInvoker methodInvoker = new MethodInvoker(this.updateDisplay_Invoker);
					object obj = null;
					string text = "SELECT f_RecID from t_d_SwipeRecord WHERE t_d_SwipeRecord.f_RecID > " + this.lngDealtRecordID;
					if (wgAppConfig.IsAccessDB)
					{
						using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
						{
							using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
							{
								oleDbConnection.Open();
								obj = oleDbCommand.ExecuteScalar();
								oleDbConnection.Close();
							}
							goto IL_0117;
						}
					}
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
						{
							sqlConnection.Open();
							obj = sqlCommand.ExecuteScalar();
							sqlConnection.Close();
						}
					}
					IL_0117:
					if (wgTools.SetObjToStr(obj) != "")
					{
						this.getNewMeetingRecord();
						this.fillMeetingRecord(this.curMeetingNo);
						this.fillMeetingNum();
						base.BeginInvoke(methodInvoker);
					}
					else
					{
						try
						{
							if (this.frmWatch != null)
							{
								ListView lstDoors = this.frmWatch.lstDoors;
								bool flag = false;
								for (int i = 0; i <= lstDoors.Items.Count - 1; i++)
								{
									if (lstDoors.Items[i].ImageIndex == 3)
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									this.btnErrConnect.Visible = flag ^ this.btnErrConnect.Visible;
								}
								else
								{
									this.btnErrConnect.Visible = flag;
								}
							}
						}
						catch (Exception)
						{
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			this.Timer2.Enabled = true;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x001D6B80 File Offset: 0x001D5B80
		private void TimerStartSlow_Tick(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x001D6B8C File Offset: 0x001D5B8C
		public void updateDisplay_Invoker()
		{
			try
			{
				int num = 0;
				this.txtA0.Text = this.arrMeetingNum[num, 0].ToString();
				this.txtA1.Text = this.arrMeetingNum[num, 1].ToString();
				this.txtA2.Text = this.arrMeetingNum[num, 2].ToString();
				this.txtA3.Text = this.arrMeetingNum[num, 3].ToString();
				this.txtA4.Text = (this.arrMeetingNum[num, 4] / 10L).ToString() + "%";
				num = 1;
				this.txtB0.Text = this.arrMeetingNum[num, 0].ToString();
				this.txtB1.Text = this.arrMeetingNum[num, 1].ToString();
				this.txtB2.Text = this.arrMeetingNum[num, 2].ToString();
				this.txtB3.Text = this.arrMeetingNum[num, 3].ToString();
				this.txtB4.Text = (this.arrMeetingNum[num, 4] / 10L).ToString() + "%";
				num = 2;
				this.txtC0.Text = this.arrMeetingNum[num, 0].ToString();
				this.txtC1.Text = this.arrMeetingNum[num, 1].ToString();
				this.txtC2.Text = this.arrMeetingNum[num, 2].ToString();
				this.txtC3.Text = this.arrMeetingNum[num, 3].ToString();
				this.txtC4.Text = (this.arrMeetingNum[num, 4] / 10L).ToString() + "%";
				num = 3;
				this.txtD0.Text = this.arrMeetingNum[num, 0].ToString();
				this.txtD1.Text = this.arrMeetingNum[num, 1].ToString();
				this.txtD2.Text = this.arrMeetingNum[num, 2].ToString();
				this.txtD3.Text = this.arrMeetingNum[num, 3].ToString();
				this.txtD4.Text = (this.arrMeetingNum[num, 4] / 10L).ToString() + "%";
				num = 4;
				this.txtE0.Text = this.arrMeetingNum[num, 0].ToString();
				this.txtE1.Text = this.arrMeetingNum[num, 1].ToString();
				this.txtE2.Text = this.arrMeetingNum[num, 2].ToString();
				this.txtE3.Text = this.arrMeetingNum[num, 3].ToString();
				this.txtE4.Text = (this.arrMeetingNum[num, 4] / 10L).ToString() + "%";
				foreach (object obj in this.GroupBox5.Controls)
				{
					if (obj.GetType().Name.ToString() == "TextBox" && ((TextBox)obj).Text == "0")
					{
						((TextBox)obj).Text = "";
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x04002F16 RID: 12054
		private int cntTimer2;

		// Token: 0x04002F17 RID: 12055
		private frmConsole frmWatch;

		// Token: 0x04002F18 RID: 12056
		private int lastinfoRowsCount;

		// Token: 0x04002F19 RID: 12057
		private DateTime signEndtime;

		// Token: 0x04002F1A RID: 12058
		private DateTime signStarttime;

		// Token: 0x04002F1B RID: 12059
		private Thread startSlowThread;

		// Token: 0x04002F1C RID: 12060
		private ArrayList arrControllerID = new ArrayList();

		// Token: 0x04002F1D RID: 12061
		private long[,] arrMeetingNum = new long[5, 5];

		// Token: 0x04002F1E RID: 12062
		private ArrayList arrSignedCardNo = new ArrayList();

		// Token: 0x04002F1F RID: 12063
		private ArrayList arrSignedSeat = new ArrayList();

		// Token: 0x04002F20 RID: 12064
		private ArrayList arrSignedUser = new ArrayList();

		// Token: 0x04002F21 RID: 12065
		public string curMeetingNo = "";

		// Token: 0x04002F22 RID: 12066
		private DataSet ds = new DataSet();

		// Token: 0x04002F23 RID: 12067
		public long lngDealtRecordID = -1L;

		// Token: 0x04002F24 RID: 12068
		private string meetingAdr = "";

		// Token: 0x04002F25 RID: 12069
		private string queryReaderStr = "";
	}
}
