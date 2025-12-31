using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200000D RID: 13
	public partial class dfrmControlSeg : frmN3000
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x000192B5 File Offset: 0x000182B5
		public dfrmControlSeg()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000192CE File Offset: 0x000182CE
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000192D8 File Offset: 0x000182D8
		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.cmdOK_Click_Acc(sender, e);
				return;
			}
			SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
			try
			{
				string text;
				if (this.operateMode == "New")
				{
					text = " SELECT * FROM t_b_ControlSeg WHERE [f_ControlSegID]= " + this.cbof_ControlSegID.Text;
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.Default);
						if (sqlDataReader.Read())
						{
							sqlDataReader.Close();
							XMessageBox.Show(this, CommonStr.strIDIsDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						else
						{
							sqlDataReader.Close();
							text = " INSERT INTO t_b_ControlSeg([f_ControlSegID], [f_Monday], [f_Tuesday], [f_Wednesday]";
							decimal num = this.nudf_LimitedTimesOfDay.Value + ((int)this.nudf_LimitedTimesOfMonth.Value << 8);
							using (SqlCommand sqlCommand2 = new SqlCommand(string.Concat(new string[]
							{
								text,
								" , [f_Thursday], [f_Friday], [f_Saturday], [f_Sunday]  , [f_BeginHMS1],[f_EndHMS1], [f_BeginHMS2], [f_EndHMS2], [f_BeginHMS3], [f_EndHMS3] , [f_BeginYMD],[f_EndYMD], [f_ControlSegName], [f_ControlSegIDLinked] , [f_ReaderCount],[f_LimitedTimesOfDay], [f_LimitedTimesOfHMS1], [f_LimitedTimesOfHMS2], [f_LimitedTimesOfHMS3] , [f_ControlByHoliday] )  VALUES ( ",
								this.cbof_ControlSegID.Text,
								" , ",
								this.chkMonday.Checked ? "1" : "0",
								" , ",
								this.chkTuesday.Checked ? "1" : "0",
								" , ",
								this.chkWednesday.Checked ? "1" : "0",
								" , ",
								this.chkThursday.Checked ? "1" : "0",
								" , ",
								this.chkFriday.Checked ? "1" : "0",
								" , ",
								this.chkSaturday.Checked ? "1" : "0",
								" , ",
								this.chkSunday.Checked ? "1" : "0",
								" , ",
								this.getDateString(this.dateBeginHMS1),
								" , ",
								this.getDateString(this.dateEndHMS1),
								" , ",
								this.getDateString(this.dateBeginHMS2),
								" , ",
								this.getDateString(this.dateEndHMS2),
								" , ",
								this.getDateString(this.dateBeginHMS3),
								" , ",
								this.getDateString(this.dateEndHMS3),
								" , ",
								this.getDateString(this.dtpBegin),
								" , ",
								this.getDateString(this.dtpEnd),
								" , ",
								wgTools.PrepareStrNUnicode(this.txtf_ControlSegName.Text),
								" , ",
								this.cbof_ControlSegIDLinked.Text,
								" , ",
								this.optReaderCount.Checked ? "1" : "0",
								" , ",
								num.ToString(),
								" , ",
								this.nudf_LimitedTimesOfHMS1.Value.ToString(),
								" , ",
								this.nudf_LimitedTimesOfHMS2.Value.ToString(),
								" , ",
								this.nudf_LimitedTimesOfHMS3.Value.ToString(),
								" , ",
								this.chkNotAllowInHolidays.Checked ? "1" : "0",
								")"
							}), sqlConnection))
							{
								sqlCommand2.ExecuteNonQuery();
							}
							base.DialogResult = DialogResult.OK;
							base.Close();
						}
						return;
					}
				}
				sqlConnection.Open();
				text = " UPDATE t_b_ControlSeg ";
				decimal num2 = this.nudf_LimitedTimesOfDay.Value + ((int)this.nudf_LimitedTimesOfMonth.Value << 8);
				using (SqlCommand sqlCommand3 = new SqlCommand(string.Concat(new string[]
				{
					text,
					" SET  [f_Monday]= ",
					this.chkMonday.Checked ? "1" : "0",
					", [f_Tuesday]=",
					this.chkTuesday.Checked ? "1" : "0",
					", [f_Wednesday]=",
					this.chkWednesday.Checked ? "1" : "0",
					" , [f_Thursday]= ",
					this.chkThursday.Checked ? "1" : "0",
					" ,[f_Friday]=",
					this.chkFriday.Checked ? "1" : "0",
					" , [f_Saturday]=",
					this.chkSaturday.Checked ? "1" : "0",
					" , [f_Sunday] =",
					this.chkSunday.Checked ? "1" : "0",
					" , [f_BeginHMS1]=",
					this.getDateString(this.dateBeginHMS1),
					" ,[f_EndHMS1]=",
					this.getDateString(this.dateEndHMS1),
					" , [f_BeginHMS2]=",
					this.getDateString(this.dateBeginHMS2),
					" ,[f_EndHMS2]=",
					this.getDateString(this.dateEndHMS2),
					" , [f_BeginHMS3]=",
					this.getDateString(this.dateBeginHMS3),
					" ,[f_EndHMS3]=",
					this.getDateString(this.dateEndHMS3),
					" , [f_BeginYMD]=",
					this.getDateString(this.dtpBegin),
					" , [f_EndYMD]=",
					this.getDateString(this.dtpEnd),
					" , [f_ControlSegName]= ",
					wgTools.PrepareStrNUnicode(this.txtf_ControlSegName.Text),
					" , [f_ControlSegIDLinked]= ",
					this.cbof_ControlSegIDLinked.Text,
					" , [f_ReaderCount]=  ",
					this.optReaderCount.Checked ? "1" : "0",
					" , [f_LimitedTimesOfDay]=  ",
					num2.ToString(),
					" , [f_LimitedTimesOfHMS1]=  ",
					this.nudf_LimitedTimesOfHMS1.Value.ToString(),
					" , [f_LimitedTimesOfHMS2]=  ",
					this.nudf_LimitedTimesOfHMS2.Value.ToString(),
					" , [f_LimitedTimesOfHMS3]=  ",
					this.nudf_LimitedTimesOfHMS3.Value.ToString(),
					" , [f_ControlByHoliday] =",
					this.chkNotAllowInHolidays.Checked ? "1" : "0",
					" WHERE [f_ControlSegID]= ",
					this.cbof_ControlSegID.Text
				}), sqlConnection))
				{
					sqlCommand3.ExecuteNonQuery();
				}
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				sqlConnection.Dispose();
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00019B1C File Offset: 0x00018B1C
		private void cmdOK_Click_Acc(object sender, EventArgs e)
		{
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			try
			{
				string text;
				if (this.operateMode == "New")
				{
					text = " SELECT * FROM t_b_ControlSeg WHERE [f_ControlSegID]= " + this.cbof_ControlSegID.Text;
					oleDbConnection.Open();
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader(CommandBehavior.Default);
						if (oleDbDataReader.Read())
						{
							oleDbDataReader.Close();
							XMessageBox.Show(this, CommonStr.strIDIsDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						else
						{
							oleDbDataReader.Close();
							text = " INSERT INTO t_b_ControlSeg([f_ControlSegID], [f_Monday], [f_Tuesday], [f_Wednesday]";
							decimal num = this.nudf_LimitedTimesOfDay.Value + ((int)this.nudf_LimitedTimesOfMonth.Value << 8);
							using (OleDbCommand oleDbCommand2 = new OleDbCommand(string.Concat(new string[]
							{
								text,
								" , [f_Thursday], [f_Friday], [f_Saturday], [f_Sunday]  , [f_BeginHMS1],[f_EndHMS1], [f_BeginHMS2], [f_EndHMS2], [f_BeginHMS3], [f_EndHMS3] , [f_BeginYMD],[f_EndYMD], [f_ControlSegName], [f_ControlSegIDLinked] , [f_ReaderCount],[f_LimitedTimesOfDay], [f_LimitedTimesOfHMS1], [f_LimitedTimesOfHMS2], [f_LimitedTimesOfHMS3] , [f_ControlByHoliday] )  VALUES ( ",
								this.cbof_ControlSegID.Text,
								" , ",
								this.chkMonday.Checked ? "1" : "0",
								" , ",
								this.chkTuesday.Checked ? "1" : "0",
								" , ",
								this.chkWednesday.Checked ? "1" : "0",
								" , ",
								this.chkThursday.Checked ? "1" : "0",
								" , ",
								this.chkFriday.Checked ? "1" : "0",
								" , ",
								this.chkSaturday.Checked ? "1" : "0",
								" , ",
								this.chkSunday.Checked ? "1" : "0",
								" , ",
								this.getDateString(this.dateBeginHMS1),
								" , ",
								this.getDateString(this.dateEndHMS1),
								" , ",
								this.getDateString(this.dateBeginHMS2),
								" , ",
								this.getDateString(this.dateEndHMS2),
								" , ",
								this.getDateString(this.dateBeginHMS3),
								" , ",
								this.getDateString(this.dateEndHMS3),
								" , ",
								this.getDateString(this.dtpBegin),
								" , ",
								this.getDateString(this.dtpEnd),
								" , ",
								wgTools.PrepareStrNUnicode(this.txtf_ControlSegName.Text),
								" , ",
								this.cbof_ControlSegIDLinked.Text,
								" , ",
								this.optReaderCount.Checked ? "1" : "0",
								" , ",
								num.ToString(),
								" , ",
								this.nudf_LimitedTimesOfHMS1.Value.ToString(),
								" , ",
								this.nudf_LimitedTimesOfHMS2.Value.ToString(),
								" , ",
								this.nudf_LimitedTimesOfHMS3.Value.ToString(),
								" , ",
								this.chkNotAllowInHolidays.Checked ? "1" : "0",
								")"
							}), oleDbConnection))
							{
								oleDbCommand2.ExecuteNonQuery();
							}
							base.DialogResult = DialogResult.OK;
							base.Close();
						}
						return;
					}
				}
				oleDbConnection.Open();
				text = " UPDATE t_b_ControlSeg ";
				decimal num2 = this.nudf_LimitedTimesOfDay.Value + ((int)this.nudf_LimitedTimesOfMonth.Value << 8);
				using (OleDbCommand oleDbCommand3 = new OleDbCommand(string.Concat(new string[]
				{
					text,
					" SET  [f_Monday]= ",
					this.chkMonday.Checked ? "1" : "0",
					", [f_Tuesday]=",
					this.chkTuesday.Checked ? "1" : "0",
					", [f_Wednesday]=",
					this.chkWednesday.Checked ? "1" : "0",
					" , [f_Thursday]= ",
					this.chkThursday.Checked ? "1" : "0",
					" ,[f_Friday]=",
					this.chkFriday.Checked ? "1" : "0",
					" , [f_Saturday]=",
					this.chkSaturday.Checked ? "1" : "0",
					" , [f_Sunday] =",
					this.chkSunday.Checked ? "1" : "0",
					" , [f_BeginHMS1]=",
					this.getDateString(this.dateBeginHMS1),
					" ,[f_EndHMS1]=",
					this.getDateString(this.dateEndHMS1),
					" , [f_BeginHMS2]=",
					this.getDateString(this.dateBeginHMS2),
					" ,[f_EndHMS2]=",
					this.getDateString(this.dateEndHMS2),
					" , [f_BeginHMS3]=",
					this.getDateString(this.dateBeginHMS3),
					" ,[f_EndHMS3]=",
					this.getDateString(this.dateEndHMS3),
					" , [f_BeginYMD]=",
					this.getDateString(this.dtpBegin),
					" , [f_EndYMD]=",
					this.getDateString(this.dtpEnd),
					" , [f_ControlSegName]= ",
					wgTools.PrepareStrNUnicode(this.txtf_ControlSegName.Text),
					" , [f_ControlSegIDLinked]= ",
					this.cbof_ControlSegIDLinked.Text,
					" , [f_ReaderCount]=  ",
					this.optReaderCount.Checked ? "1" : "0",
					" , [f_LimitedTimesOfDay]=  ",
					num2.ToString(),
					" , [f_LimitedTimesOfHMS1]=  ",
					this.nudf_LimitedTimesOfHMS1.Value.ToString(),
					" , [f_LimitedTimesOfHMS2]=  ",
					this.nudf_LimitedTimesOfHMS2.Value.ToString(),
					" , [f_LimitedTimesOfHMS3]=  ",
					this.nudf_LimitedTimesOfHMS3.Value.ToString(),
					" , [f_ControlByHoliday] =",
					this.chkNotAllowInHolidays.Checked ? "1" : "0",
					" WHERE [f_ControlSegID]= ",
					this.cbof_ControlSegID.Text
				}), oleDbConnection))
				{
					oleDbCommand3.ExecuteNonQuery();
				}
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				oleDbConnection.Dispose();
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0001A350 File Offset: 0x00019350
		private void dfrmControlSeg_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				this.chkNotAllowInHolidays.Visible = true;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0001A378 File Offset: 0x00019378
		private void dfrmControlSeg_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.dfrmControlSeg_Load_Acc(sender, e);
				return;
			}
			if (wgAppConfig.getParamValBoolByNO(136))
			{
				base.Size = new Size(new Point(this.groupBox3.Location.X + this.groupBox3.Width + 20, base.Size.Height));
			}
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegIDLinked.Items.Clear();
			for (int i = 2; i <= 255; i++)
			{
				this.cbof_ControlSegID.Items.Add(i);
			}
			for (int i = 0; i <= 255; i++)
			{
				this.cbof_ControlSegIDLinked.Items.Add(i);
			}
			this.cbof_ControlSegIDLinked.Text = "0";
			this.dateBeginHMS1.CustomFormat = "HH:mm";
			this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS1.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS1.CustomFormat = "HH:mm";
			this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS1.Value = DateTime.Parse("23:59:59");
			this.dateBeginHMS2.CustomFormat = "HH:mm";
			this.dateBeginHMS2.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS2.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS2.CustomFormat = "HH:mm";
			this.dateEndHMS2.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS2.Value = DateTime.Parse("00:00:00");
			this.dateBeginHMS3.CustomFormat = "HH:mm";
			this.dateBeginHMS3.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS3.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS3.CustomFormat = "HH:mm";
			this.dateEndHMS3.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS3.Value = DateTime.Parse("00:00:00");
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				if (this.operateMode == "New")
				{
					this.cbof_ControlSegID.Enabled = true;
					string text = " SELECT * FROM t_b_ControlSeg ORDER BY [f_ControlSegID] DESC ";
					if (sqlConnection.State == ConnectionState.Closed)
					{
						sqlConnection.Open();
					}
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						if (sqlDataReader.Read())
						{
							this.curControlSegID = (int)sqlDataReader["f_ControlSegID"] + 1;
						}
						else
						{
							this.curControlSegID = 2;
						}
						sqlDataReader.Close();
					}
					this.cbof_ControlSegID.Text = this.curControlSegID.ToString();
				}
				else
				{
					this.cbof_ControlSegID.Enabled = false;
					this.cbof_ControlSegID.Text = this.curControlSegID.ToString();
					string text2 = " SELECT * FROM t_b_ControlSeg WHERE [f_ControlSegID]= " + this.curControlSegID.ToString();
					if (sqlConnection.State == ConnectionState.Closed)
					{
						sqlConnection.Open();
					}
					using (SqlCommand sqlCommand2 = new SqlCommand(text2, sqlConnection))
					{
						SqlDataReader sqlDataReader2 = sqlCommand2.ExecuteReader();
						if (sqlDataReader2.Read())
						{
							try
							{
								this.chkMonday.Checked = sqlDataReader2["f_Monday"].ToString() == "1";
								this.chkTuesday.Checked = sqlDataReader2["f_Tuesday"].ToString() == "1";
								this.chkWednesday.Checked = sqlDataReader2["f_Wednesday"].ToString() == "1";
								this.chkThursday.Checked = sqlDataReader2["f_Thursday"].ToString() == "1";
								this.chkFriday.Checked = sqlDataReader2["f_Friday"].ToString() == "1";
								this.chkSaturday.Checked = sqlDataReader2["f_Saturday"].ToString() == "1";
								this.chkSunday.Checked = sqlDataReader2["f_Sunday"].ToString() == "1";
								this.dateBeginHMS1.Value = wgTools.wgDateTimeParse(sqlDataReader2["f_BeginHMS1"]);
								this.dateBeginHMS2.Value = wgTools.wgDateTimeParse(sqlDataReader2["f_BeginHMS2"]);
								this.dateBeginHMS3.Value = wgTools.wgDateTimeParse(sqlDataReader2["f_BeginHMS3"]);
								this.dateEndHMS1.Value = wgTools.wgDateTimeParse(sqlDataReader2["f_EndHMS1"]);
								this.dateEndHMS2.Value = wgTools.wgDateTimeParse(sqlDataReader2["f_EndHMS2"]);
								this.dateEndHMS3.Value = wgTools.wgDateTimeParse(sqlDataReader2["f_EndHMS3"]);
								this.dtpBegin.Value = wgTools.wgDateTimeParse(sqlDataReader2["f_BeginYMD"]);
								this.dtpEnd.Value = wgTools.wgDateTimeParse(sqlDataReader2["f_EndYMD"]);
								this.txtf_ControlSegName.Text = wgTools.SetObjToStr(sqlDataReader2["f_ControlSegName"]);
								this.cbof_ControlSegIDLinked.Text = sqlDataReader2["f_ControlSegIDLinked"].ToString();
								this.chkf_ReaderCount.Checked = (int.Parse(sqlDataReader2["f_ReaderCount"].ToString()) & 1) > 0;
								this.optControllerCount.Checked = (int.Parse(sqlDataReader2["f_ReaderCount"].ToString()) & 1) == 0;
								this.optReaderCount.Checked = (int.Parse(sqlDataReader2["f_ReaderCount"].ToString()) & 1) > 0;
								this.nudf_LimitedTimesOfDay.Value = (int)sqlDataReader2["f_LimitedTimesOfDay"] & 255;
								this.nudf_LimitedTimesOfMonth.Value = ((int)sqlDataReader2["f_LimitedTimesOfDay"] >> 8) & 255;
								this.nudf_LimitedTimesOfHMS1.Value = (int)sqlDataReader2["f_LimitedTimesOfHMS1"];
								this.nudf_LimitedTimesOfHMS2.Value = (int)sqlDataReader2["f_LimitedTimesOfHMS2"];
								this.nudf_LimitedTimesOfHMS3.Value = (int)sqlDataReader2["f_LimitedTimesOfHMS3"];
								this.chkNotAllowInHolidays.Checked = sqlDataReader2["f_ControlByHoliday"].ToString() == "1";
								if (!this.chkNotAllowInHolidays.Checked)
								{
									this.chkNotAllowInHolidays.Visible = true;
								}
							}
							catch (Exception)
							{
							}
						}
						sqlDataReader2.Close();
					}
				}
			}
			wgAppConfig.setDisplayFormatDate(this.dtpBegin, wgTools.DisplayFormat_DateYMD);
			wgAppConfig.setDisplayFormatDate(this.dtpEnd, wgTools.DisplayFormat_DateYMD);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0001AAC8 File Offset: 0x00019AC8
		private void dfrmControlSeg_Load_Acc(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(136))
			{
				base.Size = new Size(new Point(this.groupBox3.Location.X + this.groupBox3.Width + 20, base.Size.Height));
			}
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegIDLinked.Items.Clear();
			for (int i = 2; i <= 255; i++)
			{
				this.cbof_ControlSegID.Items.Add(i);
			}
			for (int i = 0; i <= 255; i++)
			{
				this.cbof_ControlSegIDLinked.Items.Add(i);
			}
			this.cbof_ControlSegIDLinked.Text = "0";
			this.dateBeginHMS1.CustomFormat = "HH:mm";
			this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS1.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS1.CustomFormat = "HH:mm";
			this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS1.Value = DateTime.Parse("23:59:59");
			this.dateBeginHMS2.CustomFormat = "HH:mm";
			this.dateBeginHMS2.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS2.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS2.CustomFormat = "HH:mm";
			this.dateEndHMS2.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS2.Value = DateTime.Parse("00:00:00");
			this.dateBeginHMS3.CustomFormat = "HH:mm";
			this.dateBeginHMS3.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS3.Value = DateTime.Parse("00:00:00");
			this.dateEndHMS3.CustomFormat = "HH:mm";
			this.dateEndHMS3.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS3.Value = DateTime.Parse("00:00:00");
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				if (this.operateMode == "New")
				{
					this.cbof_ControlSegID.Enabled = true;
					string text = " SELECT * FROM t_b_ControlSeg ORDER BY [f_ControlSegID] DESC ";
					if (oleDbConnection.State == ConnectionState.Closed)
					{
						oleDbConnection.Open();
					}
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							this.curControlSegID = (int)oleDbDataReader["f_ControlSegID"] + 1;
						}
						else
						{
							this.curControlSegID = 2;
						}
						oleDbDataReader.Close();
					}
					this.cbof_ControlSegID.Text = this.curControlSegID.ToString();
				}
				else
				{
					this.cbof_ControlSegID.Enabled = false;
					this.cbof_ControlSegID.Text = this.curControlSegID.ToString();
					string text2 = " SELECT * FROM t_b_ControlSeg WHERE [f_ControlSegID]= " + this.curControlSegID.ToString();
					if (oleDbConnection.State == ConnectionState.Closed)
					{
						oleDbConnection.Open();
					}
					using (OleDbCommand oleDbCommand2 = new OleDbCommand(text2, oleDbConnection))
					{
						OleDbDataReader oleDbDataReader2 = oleDbCommand2.ExecuteReader();
						if (oleDbDataReader2.Read())
						{
							try
							{
								this.chkMonday.Checked = oleDbDataReader2["f_Monday"].ToString() == "1";
								this.chkTuesday.Checked = oleDbDataReader2["f_Tuesday"].ToString() == "1";
								this.chkWednesday.Checked = oleDbDataReader2["f_Wednesday"].ToString() == "1";
								this.chkThursday.Checked = oleDbDataReader2["f_Thursday"].ToString() == "1";
								this.chkFriday.Checked = oleDbDataReader2["f_Friday"].ToString() == "1";
								this.chkSaturday.Checked = oleDbDataReader2["f_Saturday"].ToString() == "1";
								this.chkSunday.Checked = oleDbDataReader2["f_Sunday"].ToString() == "1";
								this.dateBeginHMS1.Value = wgTools.wgDateTimeParse(oleDbDataReader2["f_BeginHMS1"]);
								this.dateBeginHMS2.Value = wgTools.wgDateTimeParse(oleDbDataReader2["f_BeginHMS2"]);
								this.dateBeginHMS3.Value = wgTools.wgDateTimeParse(oleDbDataReader2["f_BeginHMS3"]);
								this.dateEndHMS1.Value = wgTools.wgDateTimeParse(oleDbDataReader2["f_EndHMS1"]);
								this.dateEndHMS2.Value = wgTools.wgDateTimeParse(oleDbDataReader2["f_EndHMS2"]);
								this.dateEndHMS3.Value = wgTools.wgDateTimeParse(oleDbDataReader2["f_EndHMS3"]);
								this.dtpBegin.Value = wgTools.wgDateTimeParse(oleDbDataReader2["f_BeginYMD"]);
								this.dtpEnd.Value = wgTools.wgDateTimeParse(oleDbDataReader2["f_EndYMD"]);
								this.txtf_ControlSegName.Text = wgTools.SetObjToStr(oleDbDataReader2["f_ControlSegName"]);
								this.cbof_ControlSegIDLinked.Text = oleDbDataReader2["f_ControlSegIDLinked"].ToString();
								this.chkf_ReaderCount.Checked = (int.Parse(oleDbDataReader2["f_ReaderCount"].ToString()) & 1) > 0;
								this.optControllerCount.Checked = (int.Parse(oleDbDataReader2["f_ReaderCount"].ToString()) & 1) == 0;
								this.optReaderCount.Checked = (int.Parse(oleDbDataReader2["f_ReaderCount"].ToString()) & 1) > 0;
								this.nudf_LimitedTimesOfDay.Value = (int)oleDbDataReader2["f_LimitedTimesOfDay"] & 255;
								this.nudf_LimitedTimesOfMonth.Value = ((int)oleDbDataReader2["f_LimitedTimesOfDay"] >> 8) & 255;
								this.nudf_LimitedTimesOfHMS1.Value = (int)oleDbDataReader2["f_LimitedTimesOfHMS1"];
								this.nudf_LimitedTimesOfHMS2.Value = (int)oleDbDataReader2["f_LimitedTimesOfHMS2"];
								this.nudf_LimitedTimesOfHMS3.Value = (int)oleDbDataReader2["f_LimitedTimesOfHMS3"];
								this.chkNotAllowInHolidays.Checked = oleDbDataReader2["f_ControlByHoliday"].ToString() == "1";
								if (!this.chkNotAllowInHolidays.Checked)
								{
									this.chkNotAllowInHolidays.Visible = true;
								}
							}
							catch (Exception)
							{
							}
						}
						oleDbDataReader2.Close();
					}
				}
			}
			wgAppConfig.setDisplayFormatDate(this.dtpBegin, wgTools.DisplayFormat_DateYMD);
			wgAppConfig.setDisplayFormatDate(this.dtpEnd, wgTools.DisplayFormat_DateYMD);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0001B208 File Offset: 0x0001A208
		private string getDateString(DateTimePicker dtp)
		{
			if (dtp == null)
			{
				return "NULL";
			}
			return wgTools.PrepareStr(dtp.Value.ToString(wgTools.YMDHMSFormat), true, wgTools.YMDHMSFormat);
		}

		// Token: 0x04000192 RID: 402
		public int curControlSegID;

		// Token: 0x04000193 RID: 403
		public string operateMode = "";
	}
}
