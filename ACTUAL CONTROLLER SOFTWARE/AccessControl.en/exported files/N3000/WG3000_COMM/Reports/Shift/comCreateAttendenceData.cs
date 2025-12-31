using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200035D RID: 861
	public class comCreateAttendenceData : Component
	{
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001B9D RID: 7069 RVA: 0x002255F7 File Offset: 0x002245F7
		// (remove) Token: 0x06001B9E RID: 7070 RVA: 0x00225610 File Offset: 0x00224610
		public event comCreateAttendenceData.CreateCompleteEventHandler CreateComplete;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06001B9F RID: 7071 RVA: 0x00225629 File Offset: 0x00224629
		// (remove) Token: 0x06001BA0 RID: 7072 RVA: 0x00225642 File Offset: 0x00224642
		public event comCreateAttendenceData.DealingNumEventHandler DealingNum;

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0022565C File Offset: 0x0022465C
		public comCreateAttendenceData()
		{
			this.strAllowOndutyTime = "00:00:00";
			this.strAllowOffdutyTime = "23:59:59";
			this.needDutyHour = 8.0m;
			this.strTemp = "";
			this.cnNote = new SqlConnection(wgAppConfig.dbConString);
			this.bFirstGetNotes = true;
			this.tReadCardTimes = 2;
			this.tTwoReadMintime = 60;
			this.bManualRecordAsFullAttendance = true;
			this.InitializeComponent();
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x002256D4 File Offset: 0x002246D4
		public comCreateAttendenceData(IContainer Container)
			: this()
		{
			Container.Add(this);
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x002256E4 File Offset: 0x002246E4
		public void _clearAttendenceData()
		{
			string text = "delete from t_d_AttendenceData";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					sqlCommand.ExecuteNonQuery();
				}
			}
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x0022574C File Offset: 0x0022474C
		public void _clearAttStatistic()
		{
			string text = "delete from t_d_AttStatistic";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					sqlCommand.ExecuteNonQuery();
				}
			}
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x002257B4 File Offset: 0x002247B4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x002257D4 File Offset: 0x002247D4
		private void getAttendenceParam()
		{
			string text = "SELECT * FROM t_a_Attendence";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					while (sqlDataReader.Read())
					{
						if ((int)sqlDataReader["f_No"] == 1)
						{
							this.tLateTimeout = Convert.ToInt32(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 2)
						{
							this.tlateAbsenceTimeout = Convert.ToInt32(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 3)
						{
							this.tLateAbsenceDay = Convert.ToDecimal(sqlDataReader["f_Value"], CultureInfo.InvariantCulture);
						}
						else if ((int)sqlDataReader["f_No"] == 4)
						{
							this.tLeaveTimeout = Convert.ToInt32(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 5)
						{
							this.tLeaveAbsenceTimeout = Convert.ToInt32(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 6)
						{
							this.tLeaveAbsenceDay = Convert.ToDecimal(sqlDataReader["f_Value"], CultureInfo.InvariantCulture);
						}
						else if ((int)sqlDataReader["f_No"] == 7)
						{
							this.tOvertimeTimeout = Convert.ToInt32(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 8)
						{
							this.tOnduty0 = Convert.ToDateTime(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 9)
						{
							this.tOffduty0 = Convert.ToDateTime(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 10)
						{
							this.tOnduty1 = Convert.ToDateTime(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 11)
						{
							this.tOffduty1 = Convert.ToDateTime(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 12)
						{
							this.tOnduty2 = Convert.ToDateTime(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 13)
						{
							this.tOffduty2 = Convert.ToDateTime(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 14)
						{
							this.tReadCardTimes = Convert.ToInt32(sqlDataReader["f_Value"]);
						}
						else if ((int)sqlDataReader["f_No"] == 16)
						{
							this.tTwoReadMintime = Convert.ToInt32(sqlDataReader["f_Value"]);
						}
					}
					sqlDataReader.Close();
					this.strAllowOndutyTime = wgAppConfig.getSystemParamByNO(55).ToString();
					this.bEarliestAsOnDuty = wgAppConfig.getSystemParamByNO(56).ToString() == "1";
					this.bChooseTwoTimes = wgAppConfig.getSystemParamByNO(57).ToString() == "1";
					this.needDutyHour = decimal.Parse(wgAppConfig.getSystemParamByNO(58).ToString(), CultureInfo.InvariantCulture);
					this.bChooseOnlyOnDuty = wgAppConfig.getSystemParamByNO(59).ToString() == "1";
					this.bManualRecordAsFullAttendance = wgAppConfig.getSystemParamByNO(175).ToString() != "0";
					this.bFullAttendanceSpecialA = wgAppConfig.getSystemParamByNO(210).ToString() == "1";
				}
			}
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x00225BE4 File Offset: 0x00224BE4
		public string getDecimalStr(object obj)
		{
			string text = "";
			try
			{
				text = ((decimal)obj).ToString("0.0", CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return text;
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x00225C38 File Offset: 0x00224C38
		private void getDutyTime(int normalshiftID)
		{
			if (normalshiftID > 0 && this.arrNormalabc != null && normalshiftID <= this.arrNormalabc.Length)
			{
				string text = this.arrNormalabc[normalshiftID - 1];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(new char[] { ';' });
					this.tOnduty0 = DateTime.Parse(array[0]);
					this.tOffduty0 = DateTime.Parse(array[1]);
					this.tOnduty1 = DateTime.Parse(array[2]);
					this.tOffduty1 = DateTime.Parse(array[3]);
					this.tOnduty2 = DateTime.Parse(array[4]);
					this.tOffduty2 = DateTime.Parse(array[5]);
				}
			}
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x00225CE0 File Offset: 0x00224CE0
		private string getNotes(int consumerID, DateTime dateStart, DateTime dateEnd)
		{
			string text = "";
			try
			{
				if (this.dsAtt == null)
				{
					return "";
				}
				if (this.dvLeave4Notes == null)
				{
					this.dvLeave4Notes = new DataView(this.dsAtt.Tables["Leave"]);
				}
				string text2 = "SELECT * ";
				text2 = string.Concat(new string[]
				{
					text2,
					string.Format(", {0} as f_Type", wgTools.PrepareStrNUnicode(CommonStr.strSignIn)),
					" FROM t_d_ManualCardRecord   WHERE f_ConsumerID=",
					consumerID.ToString(),
					" AND ([f_ReadDate]>= ",
					this.PrepareStr(dateStart, true, "yyyy-MM-dd 00:00:00"),
					")  AND ([f_ReadDate]<= ",
					this.PrepareStr(dateEnd, true, "yyyy-MM-dd 23:59:59"),
					")  ORDER BY f_ReadDate ASC "
				});
				if (!this.bFirstGetNotes)
				{
					this.dsAtt.Tables["ManualCardRecord"].Clear();
				}
				this.bFirstGetNotes = false;
				new SqlDataAdapter(text2, this.cnNote).Fill(this.dsAtt, "ManualCardRecord");
				this.dvLeave4Notes.RowFilter = string.Format("  f_consumerID = {0} AND NOT (f_Value > '{2}' OR f_Value2 <'{1}') ", consumerID.ToString(), dateStart.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd"));
				int num = 0;
				int num2 = 0;
				DataTable dataTable = this.dsAtt.Tables["ManualCardRecord"];
				while (num < this.dvLeave4Notes.Count || num2 < this.dsAtt.Tables["ManualCardRecord"].Rows.Count)
				{
					if (num2 >= this.dsAtt.Tables["ManualCardRecord"].Rows.Count || (num < this.dvLeave4Notes.Count && DateTime.Parse((string)this.dvLeave4Notes[num]["f_Value"]) < (DateTime)dataTable.Rows[num2]["f_ReadDate"]))
					{
						string text3 = "";
						int num3 = num;
						if (this.dvLeave4Notes[num3]["f_Value"].ToString() == this.dvLeave4Notes[num3]["f_Value2"].ToString())
						{
							if (this.dvLeave4Notes[num3]["f_Value1"].ToString() == this.dvLeave4Notes[num3]["f_Value3"].ToString())
							{
								decimal num4 = 1m;
								text3 = text3 + ":" + (num4 - 0.5m).ToString() + CommonStr.strDay;
							}
						}
						else
						{
							decimal num5 = DateTime.Parse(this.dvLeave4Notes[num3]["f_Value2"].ToString(), CultureInfo.InvariantCulture).Subtract(DateTime.Parse(this.dvLeave4Notes[num3]["f_Value"].ToString())).Days + 1;
							if (this.dvLeave4Notes[num3]["f_Value1"].ToString() == this.dvLeave4Notes[num3]["f_Value3"].ToString())
							{
								num5 -= 0.5m;
							}
							else if (this.dvLeave4Notes[num3]["f_Value1"].ToString() == "P.M." || this.dvLeave4Notes[num3]["f_Value1"].ToString() == "下午" || this.dvLeave4Notes[num3]["f_Value1"].ToString() == "下午")
							{
								num5 = decimal.Subtract(num5, 1m);
							}
							text3 = text3 + ":" + num5.ToString() + CommonStr.strDay;
						}
						if (!Information.IsDBNull(this.dvLeave4Notes[num3]["f_Notes"]) && !string.IsNullOrEmpty((string)this.dvLeave4Notes[num3]["f_Notes"]))
						{
							text3 = text3 + ":" + (string)this.dvLeave4Notes[num3]["f_Notes"];
						}
						if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
						{
							text += string.Format("{0} {1}{2}; ", DateTime.Parse((string)this.dvLeave4Notes[num3]["f_Value"]).ToString(string.Format("M{0}d{1}", CommonStr.strNotesMonth, CommonStr.strNotesDay)), this.dvLeave4Notes[num3]["f_HolidayType"], text3);
						}
						else
						{
							text += string.Format("{0} {1}{2}; ", DateTime.Parse((string)this.dvLeave4Notes[num3]["f_Value"]).ToString("M.d"), this.dvLeave4Notes[num3]["f_HolidayType"], text3);
						}
						num++;
					}
					else
					{
						string text4 = "";
						int num6 = num2;
						if (!Information.IsDBNull(dataTable.Rows[num6]["f_Note"]) && !string.IsNullOrEmpty((string)dataTable.Rows[num6]["f_Note"]))
						{
							text4 = ":" + (string)dataTable.Rows[num6]["f_Note"];
						}
						if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
						{
							text += string.Format("{0} {1}{2}; ", ((DateTime)dataTable.Rows[num6]["f_ReadDate"]).ToString(string.Format("M{0}d{1} H:m", CommonStr.strNotesMonth, CommonStr.strNotesDay)), dataTable.Rows[num6]["f_Type"], text4);
						}
						else
						{
							text += string.Format("{0} {1}{2}; ", ((DateTime)dataTable.Rows[num6]["f_ReadDate"]).ToString("M.d-H:m"), dataTable.Rows[num6]["f_Type"], text4);
						}
						num2++;
					}
					if (num >= this.dvLeave4Notes.Count && num2 >= this.dsAtt.Tables["ManualCardRecord"].Rows.Count)
					{
						return text;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return text;
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x0022641C File Offset: 0x0022541C
		private bool InformationIsDate(string str)
		{
			DateTime dateTime;
			return DateTime.TryParse(str, out dateTime);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x00226431 File Offset: 0x00225431
		private void InitializeComponent()
		{
			this.components = new Container();
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x00226440 File Offset: 0x00225440
		private void loadNormalShiftABC()
		{
			if (wgAppConfig.getParamValBoolByNO(215))
			{
				string text;
				string text2;
				string text3;
				wgAppConfig.getSystemParamValue(215, out text, out text2, out text3);
				if (string.IsNullOrEmpty(text3))
				{
					text3 = "#######";
				}
				this.arrNormalabc = text3.Split(new char[] { '#' });
			}
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x00226494 File Offset: 0x00225494
		public void localizedHoliday(DataTable dt)
		{
			try
			{
				for (int i = 0; i <= dt.Rows.Count - 1; i++)
				{
					if (string.Compare("strHoliday_" + dt.Rows[i]["f_EName"], "strHoliday_Saturday") == 0)
					{
						dt.Rows[i]["f_Name"] = CommonStr.strHoliday_Saturday;
					}
					else if (string.Compare("strHoliday_" + dt.Rows[i]["f_EName"], "strHoliday_Sunday") == 0)
					{
						dt.Rows[i]["f_Name"] = CommonStr.strHoliday_Sunday;
					}
					else if (string.Compare("strHoliday_" + dt.Rows[i]["f_EName"], "strHoliday_AM") == 0)
					{
						dt.Rows[i]["f_Name"] = CommonStr.strHoliday_AM;
					}
					else if (string.Compare("strHoliday_" + dt.Rows[i]["f_EName"], "strHoliday_PM") == 0)
					{
						dt.Rows[i]["f_Name"] = CommonStr.strHoliday_PM;
					}
					if (!Information.IsDBNull(dt.Rows[i]["f_Value1"]) && (dt.Rows[i]["f_Value1"].ToString() == "A.M." || dt.Rows[i]["f_Value1"].ToString() == "上午" || dt.Rows[i]["f_Value1"].ToString() == "上午"))
					{
						dt.Rows[i]["f_Value1"] = CommonStr.strHoliday_AM;
					}
					if (!Information.IsDBNull(dt.Rows[i]["f_Value3"]) && (dt.Rows[i]["f_Value3"].ToString() == "A.M." || dt.Rows[i]["f_Value3"].ToString() == "上午" || dt.Rows[i]["f_Value3"].ToString() == "上午"))
					{
						dt.Rows[i]["f_Value3"] = CommonStr.strHoliday_AM;
					}
					if (!Information.IsDBNull(dt.Rows[i]["f_Value1"]) && (dt.Rows[i]["f_Value1"].ToString() == "P.M." || dt.Rows[i]["f_Value1"].ToString() == "下午" || dt.Rows[i]["f_Value1"].ToString() == "下午"))
					{
						dt.Rows[i]["f_Value1"] = CommonStr.strHoliday_PM;
					}
					if (!Information.IsDBNull(dt.Rows[i]["f_Value3"]) && (dt.Rows[i]["f_Value3"].ToString() == "P.M." || dt.Rows[i]["f_Value3"].ToString() == "下午" || dt.Rows[i]["f_Value3"].ToString() == "下午"))
					{
						dt.Rows[i]["f_Value3"] = CommonStr.strHoliday_PM;
					}
				}
				dt.AcceptChanges();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x002268D0 File Offset: 0x002258D0
		public void localizedHolidayType(DataTable dt)
		{
			try
			{
				for (int i = 0; i <= dt.Rows.Count - 1; i++)
				{
					if (dt.Rows[i]["f_HolidayType"].ToString() == "出差" || dt.Rows[i]["f_HolidayType"].ToString() == "出差" || dt.Rows[i]["f_HolidayType"].ToString() == "Business Trip")
					{
						dt.Rows[i]["f_HolidayType"] = CommonStr.strBusinessTrip;
					}
					if (dt.Rows[i]["f_HolidayType"].ToString() == "病假" || dt.Rows[i]["f_HolidayType"].ToString() == "病假" || dt.Rows[i]["f_HolidayType"].ToString() == "Sick Leave")
					{
						dt.Rows[i]["f_HolidayType"] = CommonStr.strSickLeave;
					}
					if (dt.Rows[i]["f_HolidayType"].ToString() == "事假" || dt.Rows[i]["f_HolidayType"].ToString() == "事假" || dt.Rows[i]["f_HolidayType"].ToString() == "Private Leave")
					{
						dt.Rows[i]["f_HolidayType"] = CommonStr.strPrivateLeave;
					}
				}
				dt.AcceptChanges();
				string text = "";
				string text2 = "";
				string text3 = "";
				wgAppConfig.getSystemParamValue(174, out text, out text2, out text3);
				string text4 = text3;
				if (!string.IsNullOrEmpty(text4))
				{
					DataView dataView = new DataView(dt);
					dataView.RowFilter = string.Format("f_NO IN ({0})", text4);
					for (int j = 0; j < dataView.Count; j++)
					{
						dataView[j]["f_fullAttendance"] = 1;
					}
					dt.AcceptChanges();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x00226B58 File Offset: 0x00225B58
		private void logCreateReport()
		{
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x00226B5C File Offset: 0x00225B5C
		public void make()
		{
			this.getAttendenceParam();
			this.loadNormalShiftABC();
			if ("00:00:00" != this.strAllowOndutyTime && "00:00" != this.strAllowOndutyTime)
			{
				if (this.InformationIsDate("2000-1-1 " + this.strAllowOndutyTime))
				{
					this.strAllowOndutyTime = Strings.Format(DateTime.Parse("2000-1-1 " + this.strAllowOndutyTime), "H:mm:ss");
					this.normalDay = 1;
					DateTime dateTime = Convert.ToDateTime(Strings.Format(DateTime.Now, "yyyy-MM-dd") + " " + this.strAllowOndutyTime).AddMilliseconds(-1.0);
					this.strAllowOffdutyTime = Strings.Format(dateTime, "H:mm:ss");
				}
				else
				{
					this.strAllowOndutyTime = "00:00:00";
				}
			}
			if (this.tReadCardTimes != 4)
			{
				if (this.bChooseOnlyOnDuty)
				{
					this.make4OneTime();
					return;
				}
				this.make4TwoTimes();
				return;
			}
			else
			{
				if (this.bChooseOnlyOnDuty)
				{
					this.make4FourTimesOnlyDuty();
					return;
				}
				this.make4FourTimes();
				return;
			}
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x00226C80 File Offset: 0x00225C80
		private void make4FourTimes()
		{
			this.cnConsumer = new SqlConnection(wgAppConfig.dbConString);
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			this.dtCardRecord1 = new DataTable();
			this.dsAtt = new DataSet("Attendance");
			this.dsAtt.Clear();
			this.daAttendenceData = new SqlDataAdapter("SELECT * FROM t_d_AttendenceData WHERE 1<0", this.cn);
			this.daHoliday = new SqlDataAdapter("SELECT * FROM t_a_Holiday ORDER BY  f_NO ASC", this.cn);
			this.daHolidayType = new SqlDataAdapter("SELECT *,0 as f_fullAttendance FROM t_a_HolidayType", this.cn);
			this.daLeave = new SqlDataAdapter("SELECT * FROM t_d_Leave", this.cn);
			this.daNoCardRecord = new SqlDataAdapter("SELECT f_ReadDate,f_Character,'' as f_Type  FROM t_d_ManualCardRecord Where 1<0 ", this.cn);
			this.daNoCardRecord.Fill(this.dsAtt, "AllCardRecords");
			this.dtCardRecord1 = this.dsAtt.Tables["AllCardRecords"];
			this.dtCardRecord1.Clear();
			this.daAttendenceData.Fill(this.dsAtt, "AttendenceData");
			this.dtAttendenceData = this.dsAtt.Tables["AttendenceData"];
			this.getAttendenceParam();
			this._clearAttendenceData();
			this._clearAttStatistic();
			this.daHoliday.Fill(this.dsAtt, "Holiday");
			this.dtHoliday = this.dsAtt.Tables["Holiday"];
			this.localizedHoliday(this.dtHoliday);
			this.dvHoliday = new DataView(this.dtHoliday);
			this.dvHoliday.RowFilter = "";
			this.dvHoliday.Sort = " f_NO ASC ";
			this.daLeave.Fill(this.dsAtt, "Leave");
			this.dtLeave = this.dsAtt.Tables["Leave"];
			this.dvLeave = new DataView(this.dtLeave);
			this.dvLeave.RowFilter = "";
			this.dvLeave.Sort = " f_NO ASC ";
			this.daHolidayType.Fill(this.dsAtt, "HolidayType");
			this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
			this.localizedHolidayType(this.dtHolidayType);
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 AND f_ShiftEnabled =0) ", this.cnConsumer);
			}
			else
			{
				this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 ) ", this.cnConsumer);
			}
			this.cnConsumer.Open();
			SqlDataReader sqlDataReader = this.cmdConsumer.ExecuteReader();
			int num = 0;
			try
			{
				int num2 = 0;
				while (sqlDataReader.Read())
				{
					num = (int)sqlDataReader["f_ConsumerID"];
					this.getDutyTime((int)((byte)sqlDataReader["f_AttendEnabled"]));
					num2++;
					string text = "SELECT f_ReadDate,f_Character,'' as f_Type  ";
					text = string.Concat(new string[]
					{
						text,
						", t_b_Reader.f_DutyOnOff  FROM t_d_SwipeRecord INNER JOIN t_b_Reader ON ( t_b_Reader.f_Attend=1 AND t_d_SwipeRecord.f_ReaderID =t_b_Reader.f_ReaderID ) ",
						wgAppConfig.getMoreCardShiftOneUserCondition(num),
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  AND t_b_Reader.f_Attend = 1 "
					});
					if (wgAppConfig.getSystemParamByNO(54) == "1")
					{
						text += " AND f_Character >= 1 ";
					}
					text += " ORDER BY f_ReadDate ASC ";
					this.daCardRecord = new SqlDataAdapter(text, this.cn);
					text = "SELECT f_ReadDate,f_Character ";
					text = string.Concat(new string[]
					{
						text,
						string.Format(",{0} as f_Type", wgTools.PrepareStrNUnicode(CommonStr.strSignIn)),
						", 3 as f_DutyOnOff  FROM t_d_ManualCardRecord   WHERE f_ConsumerID=",
						num.ToString(),
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  ORDER BY f_ReadDate ASC "
					});
					this.daManualCardRecord = new SqlDataAdapter(text, this.cn);
					decimal[] array = new decimal[32];
					DataRow dataRow = null;
					if (this.DealingNumEvent != null)
					{
						this.DealingNumEvent(num2);
					}
					this.gProcVal = num2 + 1;
					if (this.bStopCreate)
					{
						return;
					}
					DateTime dateTime = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
					DateTime dateTime2 = DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double)this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime));
					DateTime dateTime3 = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					decimal num7 = 0m;
					decimal num8 = 0m;
					int num9 = 0;
					int num10 = 0;
					int num11 = 0;
					for (int i = 0; i <= array.Length - 1; i++)
					{
						array[i] = 0m;
					}
					this.dtCardRecord1 = this.dsAtt.Tables["AllCardRecords"];
					this.dsAtt.Tables["AllCardRecords"].Clear();
					this.daCardRecord.Fill(this.dsAtt, "AllCardRecords");
					this.daManualCardRecord.Fill(this.dsAtt, "AllCardRecords");
					this.dvCardRecord = new DataView(this.dtCardRecord1);
					this.dvCardRecord.RowFilter = "";
					this.dvCardRecord.Sort = " f_ReadDate ASC ";
					int j = 0;
					while (this.dvCardRecord.Count > j + 1)
					{
						if (((DateTime)this.dvCardRecord[j + 1][0]).Subtract((DateTime)this.dvCardRecord[j][0]).TotalSeconds < (double)this.tTwoReadMintime && Convert.ToInt32(this.dvCardRecord[j]["f_DutyOnOff"]) == Convert.ToInt32(this.dvCardRecord[j + 1]["f_DutyOnOff"]))
						{
							this.dvCardRecord[j + 1].Delete();
						}
						else
						{
							j++;
						}
					}
					while (dateTime3 <= DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double)this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime)))
					{
						dataRow = this.dtAttendenceData.NewRow();
						dataRow["f_ConsumerID"] = num;
						dataRow["f_AttDate"] = dateTime3;
						dataRow["f_LateTime"] = 0;
						dataRow["f_LeaveEarlyTime"] = 0;
						dataRow["f_OvertimeTime"] = 0;
						dataRow["f_AbsenceDay"] = 0;
						bool flag = true;
						bool flag2 = true;
						this.dvCardRecord.RowFilter = " f_ReadDate >= #" + dateTime3.ToString("yyyy-MM-dd HH:mm:ss") + "# and f_ReadDate<= " + Strings.Format(dateTime3.AddDays((double)this.normalDay), "#yyyy-MM-dd " + this.strAllowOffdutyTime + "#");
						this.dvLeave.RowFilter = " f_ConsumerID = " + num.ToString();
						if (this.dvCardRecord.Count > 0)
						{
							int k = 0;
							while (k <= this.dvCardRecord.Count - 1)
							{
								DateTime dateTime4 = Convert.ToDateTime(this.dvCardRecord[k]["f_ReadDate"]);
								if (string.Compare(Strings.Format(dateTime4, "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
								{
									if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty1, "HH:mm:ss")) <= 0)
									{
										if (Convert.ToInt32(this.dvCardRecord[k]["f_DutyOnOff"]) != 2)
										{
											if (this.bEarliestAsOnDuty)
											{
												if (this.SetObjToStr(dataRow["f_Onduty1"]) == "")
												{
													dataRow["f_Onduty1"] = dateTime4;
													dataRow["f_Onduty1Desc"] = this.dvCardRecord[k]["f_Type"];
												}
											}
											else
											{
												dataRow["f_Onduty1"] = dateTime4;
												dataRow["f_Onduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											}
											if (this.dvCardRecord.Count == 4)
											{
												k++;
												break;
											}
										}
										k++;
									}
									else
									{
										if (!(this.SetObjToStr(dataRow["f_Onduty1"]) == ""))
										{
											break;
										}
										if (Convert.ToInt32(this.dvCardRecord[k]["f_DutyOnOff"]) == 2)
										{
											dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
											break;
										}
										if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty1.AddMinutes((double)this.tLateTimeout), "HH:mm:ss")) <= 0)
										{
											dataRow["f_Onduty1"] = dateTime4;
											dataRow["f_Onduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											k++;
											break;
										}
										if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty1.AddMinutes((double)this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
										{
											dataRow["f_Onduty1"] = dateTime4;
											dataRow["f_Onduty1Desc"] = CommonStr.strLateness;
											k++;
											break;
										}
										if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOffduty1.AddMinutes((double)(-(double)this.tLeaveTimeout)), "HH:mm:ss")) > 0)
										{
											dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
										}
										else
										{
											dataRow["f_Onduty1"] = dateTime4;
											dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
										}
										if (this.dvCardRecord.Count == 4)
										{
											k++;
											break;
										}
										break;
									}
								}
								else
								{
									if (!(this.SetObjToStr(dataRow["f_Onduty1"]) != ""))
									{
										dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
										break;
									}
									break;
								}
							}
							int num12 = k;
							int num13 = 0;
							string text2 = "";
							k = num12;
							while (k <= this.dvCardRecord.Count - 1)
							{
								DateTime dateTime4 = Convert.ToDateTime(this.dvCardRecord[k]["f_ReadDate"]);
								if (string.Compare(Strings.Format(dateTime4, "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
								{
									if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) <= 0 || this.dvCardRecord.Count == 4)
									{
										if (Convert.ToInt32(this.dvCardRecord[k]["f_DutyOnOff"]) != 1 || string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) > 0)
										{
											dataRow["f_Offduty1"] = dateTime4;
											dataRow["f_Offduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											if (string.Compare(Strings.Format(dateTime4.AddMinutes((double)this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
											{
												if (string.Compare(Strings.Format(dateTime4.AddMinutes((double)this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
												{
													dataRow["f_Offduty1Desc"] = CommonStr.strAbsence;
												}
												else
												{
													dataRow["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
												}
											}
											if (this.dvCardRecord.Count == 4)
											{
												k++;
												break;
											}
										}
									}
									else if (Convert.ToInt32(this.dvCardRecord[k]["f_DutyOnOff"]) == 1)
									{
										if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2, "HH:mm:ss")) >= 0)
										{
											if (this.SetObjToStr(dataRow["f_Offduty1"]) == "")
											{
												dataRow["f_Offduty1Desc"] = CommonStr.strNotReadCard;
												break;
											}
											break;
										}
										else if (num13 == 0)
										{
											num13 = k;
											text2 = wgTools.SetObjToStr(dataRow["f_Offduty1"]);
										}
									}
									else
									{
										if (this.SetObjToStr(dataRow["f_Offduty1"]) == "")
										{
											if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2, "HH:mm:ss")) >= 0)
											{
												dataRow["f_Offduty1Desc"] = CommonStr.strNotReadCard;
											}
											else if (k + 1 <= this.dvCardRecord.Count - 1 && string.Compare(Strings.Format(this.dvCardRecord[k + 1]["f_ReadDate"], "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double)this.tLateTimeout), "HH:mm:ss")) <= 0)
											{
												dataRow["f_Offduty1"] = dateTime4;
												dataRow["f_Offduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											}
											else if (this.SetObjToStr(dataRow["f_Onduty1"]) == "")
											{
												dataRow["f_Offduty1Desc"] = CommonStr.strNotReadCard;
											}
											else
											{
												dataRow["f_Offduty1"] = dateTime4;
												dataRow["f_Offduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											}
										}
										else if (k + 1 <= this.dvCardRecord.Count - 1 && string.Compare(Strings.Format(this.dvCardRecord[k + 1]["f_ReadDate"], "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double)this.tLateTimeout), "HH:mm:ss")) <= 0)
										{
											if (this.SetObjToStr(dataRow["f_Offduty1Desc"]).IndexOf(CommonStr.strLeaveEarly) >= 0)
											{
												dataRow["f_Offduty1"] = dateTime4;
												dataRow["f_Offduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											}
											else if (this.SetObjToStr(dataRow["f_Offduty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0 && this.SetObjToStr(dataRow["f_Offduty1"]) != "")
											{
												dataRow["f_Offduty1"] = dateTime4;
												dataRow["f_Offduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											}
										}
										if (this.dvCardRecord.Count == 4)
										{
											k++;
											break;
										}
										break;
									}
									k++;
								}
								else
								{
									if (this.SetObjToStr(dataRow["f_Offduty1"]) == "")
									{
										dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
										break;
									}
									break;
								}
							}
							if (this.SetObjToStr(dataRow["f_Offduty1"]) == this.SetObjToStr(dataRow["f_Onduty1"]) && (this.SetObjToStr(dataRow["f_Offduty1Desc"]).IndexOf(CommonStr.strLeaveEarly) >= 0 || this.SetObjToStr(dataRow["f_Offduty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0))
							{
								dataRow["f_Offduty1"] = DBNull.Value;
								dataRow["f_Offduty1Desc"] = CommonStr.strNotReadCard;
							}
							if (this.SetObjToStr(dataRow["f_Offduty1"]) == "" && this.SetObjToStr(dataRow["f_Onduty1"]) == "")
							{
								dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
								dataRow["f_Offduty1Desc"] = CommonStr.strAbsence;
							}
							if (this.SetObjToStr(dataRow["f_Offduty1"]) == "" && this.SetObjToStr(dataRow["f_Offduty1Desc"]) == "")
							{
								dataRow["f_Offduty1Desc"] = CommonStr.strNotReadCard;
							}
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0 && this.SetObjToStr(dataRow["f_OffDuty1Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
							{
								dataRow["f_OnDuty1Desc"] = "";
							}
							num12 = k;
							if (num13 > 0)
							{
								if (text2.Equals(wgTools.SetObjToStr(dataRow["f_Offduty1"])))
								{
									num12 = num13;
								}
							}
							k = num12;
							while (k <= this.dvCardRecord.Count - 1)
							{
								DateTime dateTime4 = Convert.ToDateTime(this.dvCardRecord[k]["f_ReadDate"]);
								if (Strings.Format(dateTime4, "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
								{
									if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2, "HH:mm:ss")) <= 0)
									{
										if (Convert.ToInt32(this.dvCardRecord[k]["f_DutyOnOff"]) != 2)
										{
											dataRow["f_Onduty2"] = dateTime4;
											dataRow["f_Onduty2Desc"] = this.dvCardRecord[k]["f_Type"];
											if (this.dvCardRecord.Count == 4)
											{
												k++;
												break;
											}
										}
										k++;
									}
									else
									{
										if (this.SetObjToStr(dataRow["f_Onduty2"]) != "")
										{
											if (!(this.SetObjToStr(dataRow["f_Offduty1"]) == this.SetObjToStr(dataRow["f_Onduty2"])))
											{
												break;
											}
											dataRow["f_Onduty2"] = DBNull.Value;
											dataRow["f_Onduty2Desc"] = "";
										}
										if (Convert.ToInt32(this.dvCardRecord[k]["f_DutyOnOff"]) == 2)
										{
											dataRow["f_Onduty2Desc"] = CommonStr.strNotReadCard;
											break;
										}
										if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double)this.tLateTimeout), "HH:mm:ss")) <= 0)
										{
											dataRow["f_Onduty2"] = dateTime4;
											dataRow["f_Onduty2Desc"] = this.dvCardRecord[k]["f_Type"];
										}
										else if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double)this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
										{
											dataRow["f_Onduty2"] = dateTime4;
											dataRow["f_Onduty2Desc"] = CommonStr.strLateness;
										}
										else if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOffduty2.AddMinutes((double)(-(double)this.tLeaveTimeout)), "HH:mm:ss")) > 0)
										{
											dataRow["f_Onduty2Desc"] = CommonStr.strNotReadCard;
										}
										else
										{
											dataRow["f_Onduty2"] = dateTime4;
											dataRow["f_Onduty2Desc"] = CommonStr.strAbsence;
										}
										if (this.dvCardRecord.Count == 4)
										{
											k++;
											break;
										}
										break;
									}
								}
								else
								{
									if (this.SetObjToStr(dataRow["f_Onduty2Desc"]) == "" && this.SetObjToStr(dataRow["f_Onduty2"]) == "")
									{
										dataRow["f_Onduty2Desc"] = CommonStr.strNotReadCard;
										break;
									}
									break;
								}
							}
							num12 = k;
							for (k = num12; k <= this.dvCardRecord.Count - 1; k++)
							{
								if (Convert.ToInt32(this.dvCardRecord[k]["f_DutyOnOff"]) != 1)
								{
									DateTime dateTime4 = Convert.ToDateTime(this.dvCardRecord[k]["f_ReadDate"]);
									if (Strings.Format(dateTime4, "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
									{
										if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOffduty2, "HH:mm:ss")) <= 0)
										{
											dataRow["f_Offduty2"] = dateTime4;
											dataRow["f_Offduty2Desc"] = this.dvCardRecord[k]["f_Type"];
											if (string.Compare(Strings.Format(dateTime4.AddMinutes((double)this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty2, "HH:mm:ss")) < 0)
											{
												if (string.Compare(Strings.Format(dateTime4.AddMinutes((double)this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty2, "HH:mm:ss")) < 0)
												{
													dataRow["f_Offduty2Desc"] = CommonStr.strAbsence;
												}
												else
												{
													dataRow["f_Offduty2Desc"] = CommonStr.strLeaveEarly;
												}
											}
										}
										else
										{
											dataRow["f_Offduty2"] = dateTime4;
											dataRow["f_Offduty2Desc"] = this.dvCardRecord[k]["f_Type"];
										}
									}
									else
									{
										dataRow["f_Offduty2"] = dateTime4;
										dataRow["f_Offduty2Desc"] = this.dvCardRecord[k]["f_Type"];
									}
								}
							}
							if (this.SetObjToStr(dataRow["f_Offduty1"]) == this.SetObjToStr(dataRow["f_Onduty2"]) && this.SetObjToStr(dataRow["f_Offduty1"]) != "")
							{
								dataRow["f_Onduty2"] = DBNull.Value;
								dataRow["f_Onduty2Desc"] = "";
							}
							if (this.SetObjToStr(dataRow["f_Offduty2"]) == this.SetObjToStr(dataRow["f_Onduty2"]) && (this.SetObjToStr(dataRow["f_Offduty2Desc"]).IndexOf(CommonStr.strLeaveEarly) >= 0 || this.SetObjToStr(dataRow["f_Offduty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0))
							{
								dataRow["f_Offduty2"] = DBNull.Value;
								dataRow["f_Offduty2Desc"] = CommonStr.strNotReadCard;
							}
							if (this.SetObjToStr(dataRow["f_Offduty2"]) == "" && this.SetObjToStr(dataRow["f_Onduty2"]) == "")
							{
								dataRow["f_Onduty2Desc"] = CommonStr.strAbsence;
								dataRow["f_Offduty2Desc"] = CommonStr.strAbsence;
							}
							if (this.SetObjToStr(dataRow["f_Offduty2"]) == "" && this.SetObjToStr(dataRow["f_Offduty2Desc"]) == "")
							{
								dataRow["f_Offduty2Desc"] = CommonStr.strNotReadCard;
							}
							if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0 && this.SetObjToStr(dataRow["f_OffDuty2Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
							{
								dataRow["f_OnDuty2Desc"] = "";
							}
						}
						else
						{
							dataRow["f_OnDuty1Desc"] = CommonStr.strAbsence;
							dataRow["f_OffDuty1Desc"] = CommonStr.strAbsence;
							dataRow["f_OnDuty2Desc"] = CommonStr.strAbsence;
							dataRow["f_OffDuty2Desc"] = CommonStr.strAbsence;
						}
						int num14 = 3;
						this.dvHoliday.RowFilter = " f_NO =1 ";
						if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
						{
							num14 = 0;
						}
						else
						{
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
							{
								num14 = 1;
							}
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
							{
								num14 = 2;
							}
							this.dvHoliday.RowFilter = " f_NO =2 ";
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
							{
								num14 = 0;
							}
							else
							{
								if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
								{
									num14 = 1;
								}
								if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
								{
									num14 = 2;
								}
								this.dvHoliday.RowFilter = " f_TYPE =2 ";
								for (int l = 0; l <= this.dvHoliday.Count - 1; l++)
								{
									this.strTemp = Convert.ToString(this.dvHoliday[l]["f_Value"]);
									this.strTemp = wgTools.getDate(this.strTemp);
									this.strTemp = this.strTemp + " " + ((this.dvHoliday[l]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
									DateTime dateTime5 = DateTime.Parse(this.strTemp);
									this.strTemp = Convert.ToString(this.dvHoliday[l]["f_Value2"]);
									this.strTemp = wgTools.getDate(this.strTemp);
									this.strTemp = this.strTemp + " " + ((this.dvHoliday[l]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
									DateTime dateTime6 = DateTime.Parse(this.strTemp);
									if (dateTime5 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime6)
									{
										num14 = 0;
										break;
									}
									if (dateTime5 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= dateTime6)
									{
										num14 = 2;
									}
									if (dateTime5 <= DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:01")) && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime6)
									{
										num14 = 1;
									}
								}
							}
						}
						if (num14 != 3)
						{
							this.dvHoliday.RowFilter = " f_TYPE =3 ";
							for (int m = 0; m <= this.dvHoliday.Count - 1; m++)
							{
								this.strTemp = Convert.ToString(this.dvHoliday[m]["f_Value"]);
								this.strTemp = wgTools.getDate(this.strTemp);
								this.strTemp = this.strTemp + " " + ((this.dvHoliday[m]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
								DateTime dateTime7 = DateTime.Parse(this.strTemp);
								this.strTemp = Convert.ToString(this.dvHoliday[m]["f_Value2"]);
								this.strTemp = wgTools.getDate(this.strTemp);
								this.strTemp = this.strTemp + " " + ((this.dvHoliday[m]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
								DateTime dateTime8 = DateTime.Parse(this.strTemp);
								if (dateTime7 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime8)
								{
									num14 = 3;
									break;
								}
								if (dateTime7 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= dateTime8)
								{
									if (num14 == 2)
									{
										num14 = 3;
									}
									else
									{
										num14 = 1;
									}
								}
								if (dateTime7 <= DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:01")) && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime8)
								{
									if (num14 == 1)
									{
										num14 = 3;
									}
									else
									{
										num14 = 2;
									}
								}
							}
						}
						switch (num14)
						{
						case 0:
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OnDuty2Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OffDuty2Desc"] = CommonStr.strRest;
							}
							dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							dataRow["f_OnDuty2Desc"] = CommonStr.strRest;
							dataRow["f_OffDuty2Desc"] = CommonStr.strRest;
							if (this.SetObjToStr(dataRow["f_Onduty1"]) != "" || this.SetObjToStr(dataRow["f_Offduty1"]) != "" || this.SetObjToStr(dataRow["f_Onduty2"]) != "" || this.SetObjToStr(dataRow["f_Offduty2"]) != "")
							{
								if (this.SetObjToStr(dataRow["f_Onduty1"]) != "" && this.SetObjToStr(dataRow["f_Offduty1"]) != "")
								{
									dataRow["f_OnDuty1Desc"] = CommonStr.strOvertime;
									dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
									if (Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
									else
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
								if (this.SetObjToStr(dataRow["f_Onduty2"]) != "" && this.SetObjToStr(dataRow["f_Offduty2"]) != "")
								{
									dataRow["f_OnDuty2Desc"] = CommonStr.strOvertime;
									dataRow["f_OffDuty2Desc"] = CommonStr.strOvertime;
									if (Strings.Format(dataRow["f_Offduty2"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
									else
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty2"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
							}
							flag = false;
							flag2 = false;
							break;
						case 1:
						{
							if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OnDuty2Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OffDuty2Desc"] = CommonStr.strRest;
							}
							bool flag3 = false;
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
							{
								if (this.SetObjToStr(dataRow["f_OffDuty2"]) != "")
								{
									dataRow["f_OffDuty1"] = dataRow["f_OffDuty2"];
									dataRow["f_OffDuty1Desc"] = "";
									dataRow["f_OffDuty2"] = DBNull.Value;
									dataRow["f_OnDuty2"] = DBNull.Value;
									flag3 = true;
								}
								else if (this.SetObjToStr(dataRow["f_OnDuty2"]) != "")
								{
									dataRow["f_OffDuty1"] = dataRow["f_OnDuty2"];
									dataRow["f_OffDuty1Desc"] = "";
									dataRow["f_OnDuty2"] = DBNull.Value;
									dataRow["f_OffDuty2"] = DBNull.Value;
									flag3 = true;
								}
								if (flag3)
								{
									if (this.SetObjToStr(dataRow["f_OnDuty1"]) == "")
									{
										dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
										dataRow["f_Offduty1Desc"] = DBNull.Value;
									}
									else if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_OffDuty1"]).AddMinutes((double)this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
									{
										if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_OffDuty1"]).AddMinutes((double)this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
										{
											dataRow["f_Offduty1Desc"] = CommonStr.strAbsence;
										}
										else
										{
											dataRow["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
										}
									}
									else
									{
										dataRow["f_Offduty1Desc"] = DBNull.Value;
									}
								}
							}
							dataRow["f_OnDuty2Desc"] = CommonStr.strRest;
							dataRow["f_OffDuty2Desc"] = CommonStr.strRest;
							if (this.SetObjToStr(dataRow["f_Onduty2"]) != "" && this.SetObjToStr(dataRow["f_Offduty2"]) != "")
							{
								dataRow["f_OnDuty2Desc"] = CommonStr.strOvertime;
								dataRow["f_OffDuty2Desc"] = CommonStr.strOvertime;
								if (Strings.Format(dataRow["f_Offduty2"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
								{
									dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
								else
								{
									dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty2"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
							}
							else
							{
								dataRow["f_OnDuty2Desc"] = CommonStr.strRest;
								dataRow["f_OffDuty2Desc"] = CommonStr.strRest;
							}
							break;
						}
						case 2:
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							}
							dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
							{
								if (this.SetObjToStr(dataRow["f_OnDuty1"]) != "")
								{
									dataRow["f_OnDuty2"] = dataRow["f_OnDuty1"];
									dataRow["f_OnDuty1"] = DBNull.Value;
									dataRow["f_OffDuty1"] = DBNull.Value;
								}
								else if (this.SetObjToStr(dataRow["f_OffDuty1"]) != "")
								{
									dataRow["f_OnDuty2"] = dataRow["f_OffDuty1"];
									dataRow["f_OffDuty1"] = DBNull.Value;
								}
							}
							if (this.SetObjToStr(dataRow["f_Onduty1"]) != "" && this.SetObjToStr(dataRow["f_Offduty1"]) != "")
							{
								dataRow["f_OnDuty1Desc"] = CommonStr.strOvertime;
								dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
								if (Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
								{
									dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
								else
								{
									dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
							}
							else
							{
								dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
								dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							}
							break;
						default:
							if (num14 == 2)
							{
								if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strAbsence)
								{
									dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
								}
								if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
								{
									dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
								}
								dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
								dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
								if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
								{
									if (this.SetObjToStr(dataRow["f_OnDuty1"]) != "")
									{
										dataRow["f_OnDuty2"] = dataRow["f_OnDuty1"];
										dataRow["f_OnDuty1"] = DBNull.Value;
										dataRow["f_OffDuty1"] = DBNull.Value;
									}
									else if (this.SetObjToStr(dataRow["f_OffDuty1"]) != "")
									{
										dataRow["f_OnDuty2"] = dataRow["f_OffDuty1"];
										dataRow["f_OffDuty1"] = DBNull.Value;
									}
								}
								if (this.SetObjToStr(dataRow["f_Onduty1"]) != "" && this.SetObjToStr(dataRow["f_Offduty1"]) != "")
								{
									dataRow["f_OnDuty1Desc"] = CommonStr.strOvertime;
									dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
									if (Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
									else
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
								else
								{
									dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
									dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
								}
							}
							else if (num14 == 3 && this.SetObjToStr(dataRow["f_Onduty2"]) != "" && this.SetObjToStr(dataRow["f_Offduty2"]) != "")
							{
								if (string.Compare(Strings.Format(dataRow["f_Offduty2"], "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
								{
									if (string.Compare(Strings.Format(dataRow["f_Offduty2"], "HH:mm:ss"), Strings.Format(this.tOffduty2.AddMinutes((double)this.tOvertimeTimeout), "HH:mm:ss")) >= 0 && string.Compare(Strings.Format(this.tOffduty2.AddMinutes((double)this.tOvertimeTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty2, "HH:mm:ss")) >= 0)
									{
										dataRow["f_OffDuty2Desc"] = CommonStr.strOvertime;
										dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty2, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
								else
								{
									dataRow["f_OffDuty2Desc"] = CommonStr.strOvertime;
									dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty2, "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
							}
							break;
						}
						if (this.dvLeave.Count > 0)
						{
							string text3 = "";
							string text4 = "";
							num14 = 3;
							for (int n = 0; n <= this.dvLeave.Count - 1; n++)
							{
								this.strTemp = Convert.ToString(this.dvLeave[n]["f_Value"]);
								this.strTemp = wgTools.getDate(this.strTemp);
								this.strTemp = this.strTemp + " " + ((this.dvLeave[n]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
								DateTime dateTime9 = DateTime.Parse(this.strTemp);
								this.strTemp = Convert.ToString(this.dvLeave[n]["f_Value2"]);
								this.strTemp = wgTools.getDate(this.strTemp);
								this.strTemp = this.strTemp + " " + ((this.dvLeave[n]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
								DateTime dateTime10 = DateTime.Parse(this.strTemp);
								string text5 = Convert.ToString(this.dvLeave[n]["f_HolidayType"]);
								if (dateTime9 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime10)
								{
									text3 = text5;
									text4 = text5;
									num14 = 0;
									break;
								}
								if (dateTime9 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= dateTime10)
								{
									text3 = text5;
									if (num14 == 1)
									{
										num14 = 0;
										break;
									}
									num14 = 2;
								}
								if (dateTime9 <= DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:01")) && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime10)
								{
									text4 = text5;
									if (num14 == 2)
									{
										num14 = 0;
										break;
									}
									num14 = 1;
								}
							}
							bool flag4 = false;
							switch (num14)
							{
							case 0:
								dataRow["f_OnDuty1Desc"] = text3;
								dataRow["f_OnDuty2Desc"] = text4;
								dataRow["f_OffDuty1Desc"] = text3;
								dataRow["f_OffDuty2Desc"] = text4;
								dataRow["f_OnDuty1"] = DBNull.Value;
								dataRow["f_OnDuty2"] = DBNull.Value;
								dataRow["f_OffDuty1"] = DBNull.Value;
								dataRow["f_OffDuty2"] = DBNull.Value;
								break;
							case 1:
								if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
								{
									if (this.SetObjToStr(dataRow["f_OffDuty2"]) != "")
									{
										dataRow["f_OffDuty1"] = dataRow["f_OffDuty2"];
										flag4 = true;
										dataRow["f_OffDuty2"] = DBNull.Value;
										dataRow["f_OnDuty2"] = DBNull.Value;
									}
									else if (this.SetObjToStr(dataRow["f_OnDuty2"]) != "")
									{
										dataRow["f_OffDuty1"] = dataRow["f_OnDuty2"];
										flag4 = true;
										dataRow["f_OnDuty2"] = DBNull.Value;
									}
									if (flag4)
									{
										if (this.SetObjToStr(dataRow["f_OnDuty1"]) == "")
										{
											dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
											dataRow["f_Offduty1Desc"] = DBNull.Value;
										}
										else if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_OffDuty1"]).AddMinutes((double)this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
										{
											if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_OffDuty1"]).AddMinutes((double)this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
											{
												dataRow["f_Offduty1Desc"] = CommonStr.strAbsence;
											}
											else
											{
												dataRow["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
											}
										}
										else
										{
											dataRow["f_Offduty1Desc"] = DBNull.Value;
										}
									}
								}
								dataRow["f_OnDuty2Desc"] = text4;
								dataRow["f_OffDuty2Desc"] = text4;
								dataRow["f_OnDuty2"] = DBNull.Value;
								dataRow["f_OffDuty2"] = DBNull.Value;
								break;
							case 2:
								if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strLateness || this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
								{
									if (this.SetObjToStr(dataRow["f_OnDuty1"]) != "")
									{
										dataRow["f_OnDuty2"] = dataRow["f_OnDuty1"];
										flag4 = true;
										dataRow["f_OnDuty1"] = DBNull.Value;
										dataRow["f_OffDuty1"] = DBNull.Value;
									}
									else if (this.SetObjToStr(dataRow["f_OffDuty1"]) != "")
									{
										flag4 = true;
										dataRow["f_OnDuty2"] = dataRow["f_OffDuty1"];
										dataRow["f_OffDuty1"] = DBNull.Value;
									}
									if (flag4)
									{
										if (this.SetObjToStr(dataRow["f_OffDuty2"]) == "")
										{
											dataRow["f_Offduty2Desc"] = CommonStr.strNotReadCard;
											dataRow["f_Onduty2Desc"] = DBNull.Value;
										}
										else
										{
											DateTime dateTime4 = Convert.ToDateTime(dataRow["f_OnDuty2"]);
											if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double)this.tLateTimeout), "HH:mm:ss")) <= 0)
											{
												dataRow["f_Onduty2"] = dateTime4;
												dataRow["f_Onduty2Desc"] = DBNull.Value;
											}
											else if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double)this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
											{
												dataRow["f_Onduty2"] = dateTime4;
												dataRow["f_Onduty2Desc"] = CommonStr.strLateness;
											}
											else
											{
												dataRow["f_Onduty2"] = dateTime4;
												dataRow["f_Onduty2Desc"] = CommonStr.strAbsence;
											}
										}
									}
								}
								dataRow["f_OnDuty1Desc"] = text3;
								dataRow["f_OffDuty1Desc"] = text3;
								dataRow["f_OnDuty1"] = DBNull.Value;
								dataRow["f_OffDuty1"] = DBNull.Value;
								break;
							}
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strLateness)
						{
							dataRow["f_LateTime"] = (long)Convert.ToInt32(dataRow["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty1, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
						}
						if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strLateness)
						{
							dataRow["f_LateTime"] = (long)Convert.ToInt32(dataRow["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty2, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_OnDuty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
						}
						if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)
						{
							dataRow["f_LeaveEarlyTime"] = (long)Convert.ToInt32(dataRow["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_OffDuty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty1, "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
						}
						if (this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strLeaveEarly)
						{
							dataRow["f_LeaveEarlyTime"] = (long)Convert.ToInt32(dataRow["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_OffDuty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty2, "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0 && this.SetObjToStr(dataRow["f_OffDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0)
						{
							dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
							flag2 = false;
						}
						else
						{
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
								flag2 = false;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
								flag2 = false;
							}
						}
						if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0 && this.SetObjToStr(dataRow["f_OffDuty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0)
						{
							dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
							flag2 = false;
						}
						else
						{
							if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
								flag2 = false;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
								flag2 = false;
							}
						}
						if (Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) >= 1m && num14 != 3)
						{
							dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) / 2.0m;
						}
						text = " INSERT INTO t_d_AttendenceData ";
						using (SqlCommand sqlCommand = new SqlCommand(string.Concat(new object[]
						{
							text,
							" ([f_ConsumerID], [f_AttDate], [f_Onduty1],[f_Onduty1Desc], [f_Offduty1], [f_Offduty1Desc], [f_Onduty2], [f_Onduty2Desc],[f_Offduty2], [f_Offduty2Desc]  , [f_LateTime], [f_LeaveEarlyTime],[f_OvertimeTime], [f_AbsenceDay]   )  VALUES ( ",
							dataRow["f_ConsumerID"],
							" , ",
							this.PrepareStr(dataRow["f_AttDate"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							this.PrepareStr(dataRow["f_Onduty1"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Onduty1Desc"]),
							" , ",
							this.PrepareStr(dataRow["f_Offduty1"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Offduty1Desc"]),
							" , ",
							this.PrepareStr(dataRow["f_Onduty2"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Onduty2Desc"]),
							" , ",
							this.PrepareStr(dataRow["f_Offduty2"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Offduty2Desc"]),
							" , ",
							dataRow["f_LateTime"],
							" , ",
							dataRow["f_LeaveEarlyTime"],
							" , ",
							this.getDecimalStr(dataRow["f_OvertimeTime"]),
							" , ",
							this.getDecimalStr(dataRow["f_AbsenceDay"]),
							" ) "
						}), this.cn))
						{
							if (this.cn.State == ConnectionState.Closed)
							{
								this.cn.Open();
							}
							sqlCommand.ExecuteNonQuery();
						}
						if (flag)
						{
							num3++;
						}
						string text6 = "";
						bool flag5 = true;
						bool flag6 = true;
						for (j = 0; j <= 3; j++)
						{
							switch (j)
							{
							case 0:
								text6 = this.SetObjToStr(dataRow["f_OnDuty1Desc"]);
								break;
							case 1:
								text6 = this.SetObjToStr(dataRow["f_OnDuty2Desc"]);
								break;
							case 2:
								text6 = this.SetObjToStr(dataRow["f_OffDuty1Desc"]);
								break;
							case 3:
								text6 = this.SetObjToStr(dataRow["f_OffDuty2Desc"]);
								break;
							}
							if (text6 == CommonStr.strLateness)
							{
								num5++;
								flag2 = false;
							}
							else if (text6 == CommonStr.strLeaveEarly)
							{
								num6++;
								flag2 = false;
							}
							else if (text6 == CommonStr.strNotReadCard)
							{
								num9++;
								flag2 = false;
							}
							else
							{
								if (text6 == CommonStr.strSignIn)
								{
									flag6 = false;
								}
								text6.IndexOf(CommonStr.strNotReadCard);
								int i = 0;
								while (i <= this.dtHolidayType.Rows.Count - 1 && i < array.Length)
								{
									if (text6 == this.dtHolidayType.Rows[i]["f_HolidayType"].ToString())
									{
										if ((int)this.dtHolidayType.Rows[i]["f_fullAttendance"] < 1)
										{
											flag2 = false;
										}
										else
										{
											flag5 = false;
										}
										array[i] += 0.25m;
										break;
									}
									i++;
								}
							}
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1"]) == "" && this.SetObjToStr(dataRow["f_OffDuty1"]) == "" && this.SetObjToStr(dataRow["f_OnDuty2"]) == "" && this.SetObjToStr(dataRow["f_OffDuty2"]) == "" && flag5)
						{
							flag2 = false;
						}
						if (!flag6 && !this.bManualRecordAsFullAttendance)
						{
							flag2 = false;
						}
						num7 += Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture);
						num8 += Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture);
						num10 += Convert.ToInt32(dataRow["f_LateTime"]);
						num11 += Convert.ToInt32(dataRow["f_LeaveEarlyTime"]);
						if (Convert.ToInt32(dataRow["f_LateTime"]) != 0 || Convert.ToInt32(dataRow["f_LeaveEarlyTime"]) != 0 || Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) != 0m)
						{
							flag2 = false;
						}
						if (this.bFullAttendanceSpecialA && (this.SetObjToStr(dataRow["f_OnDuty1"]) != "" || this.SetObjToStr(dataRow["f_OffDuty1"]) != "" || this.SetObjToStr(dataRow["f_OnDuty2"]) != "" || this.SetObjToStr(dataRow["f_OffDuty2"]) != ""))
						{
							flag2 = true;
						}
						if (flag2)
						{
							num4++;
						}
						dateTime3 = dateTime3.AddDays(1.0);
						Application.DoEvents();
					}
					string notes = this.getNotes((int)dataRow["f_ConsumerID"], dateTime, dateTime2);
					this.dvCardRecord.RowFilter = string.Format("f_Type ={0}", this.PrepareStr(CommonStr.strSignIn));
					text = " Insert Into t_d_AttStatistic ";
					text += " ( [f_ConsumerID], [f_AttDateStart], [f_AttDateEnd]  , [f_DayShouldWork],  [f_DayRealWork] , [f_TotalLate],  [f_TotalLeaveEarly],[f_TotalOvertime], [f_TotalAbsenceDay], [f_TotalNotReadCard]";
					for (int i = 1; i <= 32; i++)
					{
						object obj = text;
						text = string.Concat(new object[] { obj, " , [f_SpecialType", i, "]" });
					}
					text = string.Concat(new object[]
					{
						text,
						", f_LateMinutes, f_LeaveEarlyMinutes, f_ManualReadTimesCount , f_Notes  )  Values( ",
						dataRow["f_ConsumerID"],
						" , ",
						this.PrepareStr(dateTime, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						this.PrepareStr(dateTime2, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						num3,
						" , ",
						num4,
						" , ",
						num5,
						" , ",
						num6,
						" , ",
						this.getDecimalStr(num8),
						" , ",
						this.getDecimalStr(num7),
						" , ",
						num9
					});
					for (int i = 0; i <= 31; i++)
					{
						text = text + " , " + wgTools.PrepareStrNUnicode(this.getDecimalStr(array[i]));
					}
					using (SqlCommand sqlCommand2 = new SqlCommand(string.Concat(new object[]
					{
						text,
						", ",
						num10,
						", ",
						num11,
						", ",
						this.dvCardRecord.Count,
						", ",
						wgTools.PrepareStrNUnicode(notes),
						" )"
					}), this.cn))
					{
						if (this.cn.State == ConnectionState.Closed)
						{
							this.cn.Open();
						}
						sqlCommand2.ExecuteNonQuery();
					}
				}
				sqlDataReader.Close();
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
				this.shiftAttReportImportFromAttendenceData();
				this.shiftAttStatisticImportFromAttStatistic();
				this.logCreateReport();
				if (this.CreateCompleteEvent != null)
				{
					this.CreateCompleteEvent(true, "");
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				try
				{
					if (this.CreateCompleteEvent != null)
					{
						this.CreateCompleteEvent(false, ex.ToString());
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x0022B4AC File Offset: 0x0022A4AC
		private void make4FourTimesOnlyDuty()
		{
			this.cnConsumer = new SqlConnection(wgAppConfig.dbConString);
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			this.dtCardRecord1 = new DataTable();
			this.dsAtt = new DataSet("Attendance");
			this.dsAtt.Clear();
			this.daAttendenceData = new SqlDataAdapter("SELECT * FROM t_d_AttendenceData WHERE 1<0", this.cn);
			this.daHoliday = new SqlDataAdapter("SELECT * FROM t_a_Holiday ORDER BY  f_NO ASC", this.cn);
			this.daHolidayType = new SqlDataAdapter("SELECT *,0 as f_fullAttendance FROM t_a_HolidayType", this.cn);
			this.daLeave = new SqlDataAdapter("SELECT * FROM t_d_Leave", this.cn);
			this.daNoCardRecord = new SqlDataAdapter("SELECT f_ReadDate,f_Character,'' as f_Type  FROM t_d_ManualCardRecord Where 1<0 ", this.cn);
			this.daNoCardRecord.Fill(this.dsAtt, "AllCardRecords");
			this.dtCardRecord1 = this.dsAtt.Tables["AllCardRecords"];
			this.dtCardRecord1.Clear();
			this.daAttendenceData.Fill(this.dsAtt, "AttendenceData");
			this.dtAttendenceData = this.dsAtt.Tables["AttendenceData"];
			this.getAttendenceParam();
			this._clearAttendenceData();
			this._clearAttStatistic();
			this.daHoliday.Fill(this.dsAtt, "Holiday");
			this.dtHoliday = this.dsAtt.Tables["Holiday"];
			this.localizedHoliday(this.dtHoliday);
			this.dvHoliday = new DataView(this.dtHoliday);
			this.dvHoliday.RowFilter = "";
			this.dvHoliday.Sort = " f_NO ASC ";
			this.daLeave.Fill(this.dsAtt, "Leave");
			this.dtLeave = this.dsAtt.Tables["Leave"];
			this.dvLeave = new DataView(this.dtLeave);
			this.dvLeave.RowFilter = "";
			this.dvLeave.Sort = " f_NO ASC ";
			this.daHolidayType.Fill(this.dsAtt, "HolidayType");
			this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
			this.localizedHolidayType(this.dtHolidayType);
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 AND f_ShiftEnabled =0) ", this.cnConsumer);
			}
			else
			{
				this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 ) ", this.cnConsumer);
			}
			this.cnConsumer.Open();
			SqlDataReader sqlDataReader = this.cmdConsumer.ExecuteReader();
			int num = 0;
			try
			{
				int num2 = 0;
				while (sqlDataReader.Read())
				{
					num = (int)sqlDataReader["f_ConsumerID"];
					this.getDutyTime((int)((byte)sqlDataReader["f_AttendEnabled"]));
					num2++;
					string text = "SELECT f_ReadDate,f_Character,'' as f_Type  ";
					text = string.Concat(new string[]
					{
						text,
						" FROM t_d_SwipeRecord INNER JOIN t_b_Reader ON ( t_b_Reader.f_Attend=1 AND t_d_SwipeRecord.f_ReaderID =t_b_Reader.f_ReaderID ) ",
						wgAppConfig.getMoreCardShiftOneUserCondition(num),
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  AND t_b_Reader.f_Attend = 1 "
					});
					if (wgAppConfig.getSystemParamByNO(54) == "1")
					{
						text += " AND f_Character >= 1 ";
					}
					text += " ORDER BY f_ReadDate ASC ";
					this.daCardRecord = new SqlDataAdapter(text, this.cn);
					text = "SELECT f_ReadDate,f_Character ";
					text = string.Concat(new string[]
					{
						text,
						string.Format(",{0} as f_Type", wgTools.PrepareStrNUnicode(CommonStr.strSignIn)),
						" FROM t_d_ManualCardRecord   WHERE f_ConsumerID=",
						num.ToString(),
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  ORDER BY f_ReadDate ASC "
					});
					this.daManualCardRecord = new SqlDataAdapter(text, this.cn);
					decimal[] array = new decimal[32];
					DataRow dataRow = null;
					if (this.DealingNumEvent != null)
					{
						this.DealingNumEvent(num2);
					}
					this.gProcVal = num2 + 1;
					if (this.bStopCreate)
					{
						return;
					}
					DateTime dateTime = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
					DateTime dateTime2 = DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double)this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime));
					DateTime dateTime3 = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					decimal num7 = 0m;
					decimal num8 = 0m;
					int num9 = 0;
					int num10 = 0;
					int num11 = 0;
					for (int i = 0; i <= array.Length - 1; i++)
					{
						array[i] = 0m;
					}
					this.dtCardRecord1 = this.dsAtt.Tables["AllCardRecords"];
					this.dsAtt.Tables["AllCardRecords"].Clear();
					this.daCardRecord.Fill(this.dsAtt, "AllCardRecords");
					this.daManualCardRecord.Fill(this.dsAtt, "AllCardRecords");
					this.dvCardRecord = new DataView(this.dtCardRecord1);
					this.dvCardRecord.RowFilter = "";
					this.dvCardRecord.Sort = " f_ReadDate ASC ";
					int j = 0;
					while (this.dvCardRecord.Count > j + 1)
					{
						if (((DateTime)this.dvCardRecord[j + 1][0]).Subtract((DateTime)this.dvCardRecord[j][0]).TotalSeconds < (double)this.tTwoReadMintime)
						{
							this.dvCardRecord[j + 1].Delete();
						}
						else
						{
							j++;
						}
					}
					while (dateTime3 <= DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double)this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime)))
					{
						dataRow = this.dtAttendenceData.NewRow();
						dataRow["f_ConsumerID"] = num;
						dataRow["f_AttDate"] = dateTime3;
						dataRow["f_LateTime"] = 0;
						dataRow["f_LeaveEarlyTime"] = 0;
						dataRow["f_OvertimeTime"] = 0;
						dataRow["f_AbsenceDay"] = 0;
						bool flag = true;
						bool flag2 = true;
						this.dvCardRecord.RowFilter = " f_ReadDate >= #" + dateTime3.ToString("yyyy-MM-dd HH:mm:ss") + "# and f_ReadDate<= " + Strings.Format(dateTime3.AddDays((double)this.normalDay), "#yyyy-MM-dd " + this.strAllowOffdutyTime + "#");
						this.dvLeave.RowFilter = " f_ConsumerID = " + num.ToString();
						if (this.dvCardRecord.Count > 0)
						{
							int k = 0;
							while (k <= this.dvCardRecord.Count - 1)
							{
								DateTime dateTime4 = Convert.ToDateTime(this.dvCardRecord[k]["f_ReadDate"]);
								if (string.Compare(Strings.Format(dateTime4, "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
								{
									if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty1, "HH:mm:ss")) <= 0)
									{
										if (this.bEarliestAsOnDuty)
										{
											if (this.SetObjToStr(dataRow["f_Onduty1"]) == "")
											{
												dataRow["f_Onduty1"] = dateTime4;
												dataRow["f_Onduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											}
										}
										else
										{
											dataRow["f_Onduty1"] = dateTime4;
											dataRow["f_Onduty1Desc"] = this.dvCardRecord[k]["f_Type"];
										}
										k++;
									}
									else
									{
										if (!(this.SetObjToStr(dataRow["f_Onduty1"]) == ""))
										{
											break;
										}
										if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty1.AddMinutes((double)this.tLateTimeout), "HH:mm:ss")) <= 0)
										{
											dataRow["f_Onduty1"] = dateTime4;
											dataRow["f_Onduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											k++;
											break;
										}
										if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty1.AddMinutes((double)this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
										{
											dataRow["f_Onduty1"] = dateTime4;
											dataRow["f_Onduty1Desc"] = CommonStr.strLateness;
											k++;
											break;
										}
										if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOffduty1.AddMinutes((double)(-(double)this.tLeaveTimeout)), "HH:mm:ss")) > 0)
										{
											dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
											break;
										}
										dataRow["f_Onduty1"] = dateTime4;
										dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
										break;
									}
								}
								else
								{
									if (!(this.SetObjToStr(dataRow["f_Onduty1"]) != ""))
									{
										dataRow["f_Onduty1"] = dateTime4;
										dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
										break;
									}
									break;
								}
							}
							if (this.SetObjToStr(dataRow["f_Offduty1"]) == "" && this.SetObjToStr(dataRow["f_Onduty1"]) == "")
							{
								dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
								dataRow["f_Offduty1Desc"] = CommonStr.strAbsence;
							}
							int num12 = k;
							k = num12;
							while (k <= this.dvCardRecord.Count - 1)
							{
								DateTime dateTime4 = Convert.ToDateTime(this.dvCardRecord[k]["f_ReadDate"]);
								if (Strings.Format(dateTime4, "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
								{
									if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) > 0)
									{
										if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2, "HH:mm:ss")) <= 0)
										{
											dataRow["f_Onduty2"] = dateTime4;
											dataRow["f_Onduty2Desc"] = this.dvCardRecord[k]["f_Type"];
										}
										else
										{
											if (!(this.SetObjToStr(dataRow["f_Onduty2"]) == ""))
											{
												break;
											}
											if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double)this.tLateTimeout), "HH:mm:ss")) <= 0)
											{
												dataRow["f_Onduty2"] = dateTime4;
												dataRow["f_Onduty2Desc"] = this.dvCardRecord[k]["f_Type"];
												break;
											}
											if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double)this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
											{
												dataRow["f_Onduty2"] = dateTime4;
												dataRow["f_Onduty2Desc"] = CommonStr.strLateness;
												break;
											}
											dataRow["f_Onduty2"] = dateTime4;
											dataRow["f_Onduty2Desc"] = CommonStr.strAbsence;
											break;
										}
									}
									k++;
								}
								else
								{
									if (!(this.SetObjToStr(dataRow["f_Onduty2"]) != ""))
									{
										dataRow["f_Onduty2"] = dateTime4;
										dataRow["f_Onduty2Desc"] = CommonStr.strAbsence;
										break;
									}
									break;
								}
							}
							if (this.SetObjToStr(dataRow["f_Offduty2"]) == "" && this.SetObjToStr(dataRow["f_Onduty2"]) == "")
							{
								dataRow["f_Onduty2Desc"] = CommonStr.strAbsence;
								dataRow["f_Offduty2Desc"] = CommonStr.strAbsence;
							}
						}
						else
						{
							dataRow["f_OnDuty1Desc"] = CommonStr.strAbsence;
							dataRow["f_OffDuty1Desc"] = CommonStr.strAbsence;
							dataRow["f_OnDuty2Desc"] = CommonStr.strAbsence;
							dataRow["f_OffDuty2Desc"] = CommonStr.strAbsence;
						}
						int num13 = 3;
						this.dvHoliday.RowFilter = " f_NO =1 ";
						if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
						{
							num13 = 0;
						}
						else
						{
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
							{
								num13 = 1;
							}
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
							{
								num13 = 2;
							}
							this.dvHoliday.RowFilter = " f_NO =2 ";
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
							{
								num13 = 0;
							}
							else
							{
								if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
								{
									num13 = 1;
								}
								if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
								{
									num13 = 2;
								}
								this.dvHoliday.RowFilter = " f_TYPE =2 ";
								for (int l = 0; l <= this.dvHoliday.Count - 1; l++)
								{
									this.strTemp = Convert.ToString(this.dvHoliday[l]["f_Value"]);
									this.strTemp = this.strTemp + " " + ((this.dvHoliday[l]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
									DateTime dateTime5 = DateTime.Parse(this.strTemp);
									this.strTemp = Convert.ToString(this.dvHoliday[l]["f_Value2"]);
									this.strTemp = this.strTemp + " " + ((this.dvHoliday[l]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
									DateTime dateTime6 = DateTime.Parse(this.strTemp);
									if (dateTime5 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime6)
									{
										num13 = 0;
										break;
									}
									if (dateTime5 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= dateTime6)
									{
										num13 = 2;
									}
									if (dateTime5 <= DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:01")) && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime6)
									{
										num13 = 1;
									}
								}
							}
						}
						if (num13 != 3)
						{
							this.dvHoliday.RowFilter = " f_TYPE =3 ";
							for (int m = 0; m <= this.dvHoliday.Count - 1; m++)
							{
								this.strTemp = Convert.ToString(this.dvHoliday[m]["f_Value"]);
								this.strTemp = this.strTemp + " " + ((this.dvHoliday[m]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
								DateTime dateTime7 = DateTime.Parse(this.strTemp);
								this.strTemp = Convert.ToString(this.dvHoliday[m]["f_Value2"]);
								this.strTemp = this.strTemp + " " + ((this.dvHoliday[m]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
								DateTime dateTime8 = DateTime.Parse(this.strTemp);
								if (dateTime7 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime8)
								{
									num13 = 3;
									break;
								}
								if (dateTime7 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= dateTime8)
								{
									if (num13 == 2)
									{
										num13 = 3;
									}
									else
									{
										num13 = 1;
									}
								}
								if (dateTime7 <= DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:01")) && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime8)
								{
									if (num13 == 1)
									{
										num13 = 3;
									}
									else
									{
										num13 = 2;
									}
								}
							}
						}
						switch (num13)
						{
						case 0:
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OnDuty2Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OffDuty2Desc"] = CommonStr.strRest;
							}
							dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							dataRow["f_OnDuty2Desc"] = CommonStr.strRest;
							dataRow["f_OffDuty2Desc"] = CommonStr.strRest;
							if (this.SetObjToStr(dataRow["f_Onduty1"]) != "" || this.SetObjToStr(dataRow["f_Offduty1"]) != "" || this.SetObjToStr(dataRow["f_Onduty2"]) != "" || this.SetObjToStr(dataRow["f_Offduty2"]) != "")
							{
								if (this.SetObjToStr(dataRow["f_Onduty1"]) != "" && this.SetObjToStr(dataRow["f_Offduty1"]) != "")
								{
									dataRow["f_OnDuty1Desc"] = CommonStr.strOvertime;
									dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
									if (Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
									else
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
								if (this.SetObjToStr(dataRow["f_Onduty2"]) != "" && this.SetObjToStr(dataRow["f_Offduty2"]) != "")
								{
									dataRow["f_OnDuty2Desc"] = CommonStr.strOvertime;
									dataRow["f_OffDuty2Desc"] = CommonStr.strOvertime;
									if (Strings.Format(dataRow["f_Offduty2"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
									else
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty2"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
							}
							flag = false;
							flag2 = false;
							break;
						case 1:
						{
							if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OnDuty2Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OffDuty2Desc"] = CommonStr.strRest;
							}
							bool flag3 = false;
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
							{
								if (this.SetObjToStr(dataRow["f_OffDuty2"]) != "")
								{
									dataRow["f_OffDuty1"] = dataRow["f_OffDuty2"];
									dataRow["f_OffDuty1Desc"] = "";
									dataRow["f_OffDuty2"] = DBNull.Value;
									dataRow["f_OnDuty2"] = DBNull.Value;
									flag3 = true;
								}
								else if (this.SetObjToStr(dataRow["f_OnDuty2"]) != "")
								{
									dataRow["f_OffDuty1"] = dataRow["f_OnDuty2"];
									dataRow["f_OffDuty1Desc"] = "";
									dataRow["f_OnDuty2"] = DBNull.Value;
									dataRow["f_OffDuty2"] = DBNull.Value;
									flag3 = true;
								}
								if (flag3)
								{
									if (this.SetObjToStr(dataRow["f_OnDuty1"]) == "")
									{
										dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
										dataRow["f_Offduty1Desc"] = DBNull.Value;
									}
									else if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_OffDuty1"]).AddMinutes((double)this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
									{
										if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_OffDuty1"]).AddMinutes((double)this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
										{
											dataRow["f_Offduty1Desc"] = CommonStr.strAbsence;
										}
										else
										{
											dataRow["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
										}
									}
									else
									{
										dataRow["f_Offduty1Desc"] = DBNull.Value;
									}
								}
							}
							dataRow["f_OnDuty2Desc"] = CommonStr.strRest;
							dataRow["f_OffDuty2Desc"] = CommonStr.strRest;
							if (this.SetObjToStr(dataRow["f_Onduty2"]) != "" && this.SetObjToStr(dataRow["f_Offduty2"]) != "")
							{
								dataRow["f_OnDuty2Desc"] = CommonStr.strOvertime;
								dataRow["f_OffDuty2Desc"] = CommonStr.strOvertime;
								if (Strings.Format(dataRow["f_Offduty2"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
								{
									dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
								else
								{
									dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty2"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
							}
							else
							{
								dataRow["f_OnDuty2Desc"] = CommonStr.strRest;
								dataRow["f_OffDuty2Desc"] = CommonStr.strRest;
							}
							break;
						}
						case 2:
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							}
							dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
							{
								if (this.SetObjToStr(dataRow["f_OnDuty1"]) != "")
								{
									dataRow["f_OnDuty2"] = dataRow["f_OnDuty1"];
									dataRow["f_OnDuty1"] = DBNull.Value;
									dataRow["f_OffDuty1"] = DBNull.Value;
								}
								else if (this.SetObjToStr(dataRow["f_OffDuty1"]) != "")
								{
									dataRow["f_OnDuty2"] = dataRow["f_OffDuty1"];
									dataRow["f_OffDuty1"] = DBNull.Value;
								}
							}
							if (this.SetObjToStr(dataRow["f_Onduty1"]) != "" && this.SetObjToStr(dataRow["f_Offduty1"]) != "")
							{
								dataRow["f_OnDuty1Desc"] = CommonStr.strOvertime;
								dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
								if (Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
								{
									dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
								else
								{
									dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
							}
							else
							{
								dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
								dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							}
							break;
						default:
							if (num13 == 2)
							{
								if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strAbsence)
								{
									dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
								}
								if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
								{
									dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
								}
								dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
								dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
								if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
								{
									if (this.SetObjToStr(dataRow["f_OnDuty1"]) != "")
									{
										dataRow["f_OnDuty2"] = dataRow["f_OnDuty1"];
										dataRow["f_OnDuty1"] = DBNull.Value;
										dataRow["f_OffDuty1"] = DBNull.Value;
									}
									else if (this.SetObjToStr(dataRow["f_OffDuty1"]) != "")
									{
										dataRow["f_OnDuty2"] = dataRow["f_OffDuty1"];
										dataRow["f_OffDuty1"] = DBNull.Value;
									}
								}
								if (this.SetObjToStr(dataRow["f_Onduty1"]) != "" && this.SetObjToStr(dataRow["f_Offduty1"]) != "")
								{
									dataRow["f_OnDuty1Desc"] = CommonStr.strOvertime;
									dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
									if (Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
									else
									{
										dataRow["f_OvertimeTime"] = Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture) + Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
								else
								{
									dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
									dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
								}
							}
							else if (num13 == 3 && this.SetObjToStr(dataRow["f_Onduty2"]) != "" && this.SetObjToStr(dataRow["f_Offduty2"]) != "")
							{
								if (string.Compare(Strings.Format(dataRow["f_Offduty2"], "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
								{
									if (string.Compare(Strings.Format(dataRow["f_Offduty2"], "HH:mm:ss"), Strings.Format(this.tOffduty2.AddMinutes((double)this.tOvertimeTimeout), "HH:mm:ss")) >= 0 && string.Compare(Strings.Format(this.tOffduty2.AddMinutes((double)this.tOvertimeTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty2, "HH:mm:ss")) >= 0)
									{
										dataRow["f_OffDuty2Desc"] = CommonStr.strOvertime;
										dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty2, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
								else
								{
									dataRow["f_OffDuty2Desc"] = CommonStr.strOvertime;
									dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty2, "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
							}
							break;
						}
						if (this.dvLeave.Count > 0)
						{
							string text2 = "";
							string text3 = "";
							num13 = 3;
							for (int n = 0; n <= this.dvLeave.Count - 1; n++)
							{
								this.strTemp = Convert.ToString(this.dvLeave[n]["f_Value"]);
								this.strTemp = this.strTemp + " " + ((this.dvLeave[n]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
								DateTime dateTime9 = DateTime.Parse(this.strTemp);
								this.strTemp = Convert.ToString(this.dvLeave[n]["f_Value2"]);
								this.strTemp = this.strTemp + " " + ((this.dvLeave[n]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
								DateTime dateTime10 = DateTime.Parse(this.strTemp);
								string text4 = Convert.ToString(this.dvLeave[n]["f_HolidayType"]);
								if (dateTime9 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime10)
								{
									text2 = text4;
									text3 = text4;
									num13 = 0;
									break;
								}
								if (dateTime9 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= dateTime10)
								{
									text2 = text4;
									if (num13 == 1)
									{
										num13 = 0;
										break;
									}
									num13 = 2;
								}
								if (dateTime9 <= DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:01")) && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime10)
								{
									text3 = text4;
									if (num13 == 2)
									{
										num13 = 0;
										break;
									}
									num13 = 1;
								}
							}
							bool flag4 = false;
							switch (num13)
							{
							case 0:
								dataRow["f_OnDuty1Desc"] = text2;
								dataRow["f_OnDuty2Desc"] = text3;
								dataRow["f_OffDuty1Desc"] = text2;
								dataRow["f_OffDuty2Desc"] = text3;
								dataRow["f_OnDuty1"] = DBNull.Value;
								dataRow["f_OnDuty2"] = DBNull.Value;
								dataRow["f_OffDuty1"] = DBNull.Value;
								dataRow["f_OffDuty2"] = DBNull.Value;
								break;
							case 1:
								if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
								{
									if (this.SetObjToStr(dataRow["f_OffDuty2"]) != "")
									{
										dataRow["f_OffDuty1"] = dataRow["f_OffDuty2"];
										flag4 = true;
										dataRow["f_OffDuty2"] = DBNull.Value;
										dataRow["f_OnDuty2"] = DBNull.Value;
									}
									else if (this.SetObjToStr(dataRow["f_OnDuty2"]) != "")
									{
										dataRow["f_OffDuty1"] = dataRow["f_OnDuty2"];
										flag4 = true;
										dataRow["f_OnDuty2"] = DBNull.Value;
									}
									if (flag4)
									{
										if (this.SetObjToStr(dataRow["f_OnDuty1"]) == "")
										{
											dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
											dataRow["f_Offduty1Desc"] = DBNull.Value;
										}
										else if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_OffDuty1"]).AddMinutes((double)this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
										{
											if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_OffDuty1"]).AddMinutes((double)this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
											{
												dataRow["f_Offduty1Desc"] = CommonStr.strAbsence;
											}
											else
											{
												dataRow["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
											}
										}
										else
										{
											dataRow["f_Offduty1Desc"] = DBNull.Value;
										}
									}
								}
								dataRow["f_OnDuty2Desc"] = text3;
								dataRow["f_OffDuty2Desc"] = text3;
								dataRow["f_OnDuty2"] = DBNull.Value;
								dataRow["f_OffDuty2"] = DBNull.Value;
								break;
							case 2:
								if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strLateness || this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
								{
									if (this.SetObjToStr(dataRow["f_OnDuty1"]) != "")
									{
										dataRow["f_OnDuty2"] = dataRow["f_OnDuty1"];
										flag4 = true;
										dataRow["f_OnDuty1"] = DBNull.Value;
										dataRow["f_OffDuty1"] = DBNull.Value;
									}
									else if (this.SetObjToStr(dataRow["f_OffDuty1"]) != "")
									{
										flag4 = true;
										dataRow["f_OnDuty2"] = dataRow["f_OffDuty1"];
										dataRow["f_OffDuty1"] = DBNull.Value;
									}
									if (flag4)
									{
										if (this.SetObjToStr(dataRow["f_OffDuty2"]) == "")
										{
											dataRow["f_Offduty2Desc"] = CommonStr.strNotReadCard;
											dataRow["f_Onduty2Desc"] = DBNull.Value;
										}
										else
										{
											DateTime dateTime4 = Convert.ToDateTime(dataRow["f_OnDuty2"]);
											if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double)this.tLateTimeout), "HH:mm:ss")) <= 0)
											{
												dataRow["f_Onduty2"] = dateTime4;
												dataRow["f_Onduty2Desc"] = DBNull.Value;
											}
											else if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double)this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
											{
												dataRow["f_Onduty2"] = dateTime4;
												dataRow["f_Onduty2Desc"] = CommonStr.strLateness;
											}
											else
											{
												dataRow["f_Onduty2"] = dateTime4;
												dataRow["f_Onduty2Desc"] = CommonStr.strAbsence;
											}
										}
									}
								}
								dataRow["f_OnDuty1Desc"] = text2;
								dataRow["f_OffDuty1Desc"] = text2;
								dataRow["f_OnDuty1"] = DBNull.Value;
								dataRow["f_OffDuty1"] = DBNull.Value;
								break;
							}
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strLateness)
						{
							dataRow["f_LateTime"] = (long)Convert.ToInt32(dataRow["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty1, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
						}
						if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strLateness)
						{
							dataRow["f_LateTime"] = (long)Convert.ToInt32(dataRow["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty2, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_OnDuty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
						}
						if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)
						{
							dataRow["f_LeaveEarlyTime"] = (long)Convert.ToInt32(dataRow["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_OffDuty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty1, "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
						}
						if (this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strLeaveEarly)
						{
							dataRow["f_LeaveEarlyTime"] = (long)Convert.ToInt32(dataRow["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_OffDuty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty2, "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0 && this.SetObjToStr(dataRow["f_OffDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0)
						{
							dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
							flag2 = false;
						}
						else
						{
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
								flag2 = false;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
								flag2 = false;
							}
						}
						if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0 && this.SetObjToStr(dataRow["f_OffDuty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0)
						{
							dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
							flag2 = false;
						}
						else
						{
							if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
								flag2 = false;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty2Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
								flag2 = false;
							}
						}
						if (Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) >= 1m && num13 != 3)
						{
							dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) / 2.0m;
						}
						text = " INSERT INTO t_d_AttendenceData ";
						using (SqlCommand sqlCommand = new SqlCommand(string.Concat(new object[]
						{
							text,
							" ([f_ConsumerID], [f_AttDate], [f_Onduty1],[f_Onduty1Desc], [f_Offduty1], [f_Offduty1Desc], [f_Onduty2], [f_Onduty2Desc],[f_Offduty2], [f_Offduty2Desc]  , [f_LateTime], [f_LeaveEarlyTime],[f_OvertimeTime], [f_AbsenceDay]   )  VALUES ( ",
							dataRow["f_ConsumerID"],
							" , ",
							this.PrepareStr(dataRow["f_AttDate"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							this.PrepareStr(dataRow["f_Onduty1"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Onduty1Desc"]),
							" , ",
							this.PrepareStr(dataRow["f_Offduty1"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Offduty1Desc"]),
							" , ",
							this.PrepareStr(dataRow["f_Onduty2"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Onduty2Desc"]),
							" , ",
							this.PrepareStr(dataRow["f_Offduty2"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Offduty2Desc"]),
							" , ",
							dataRow["f_LateTime"],
							" , ",
							dataRow["f_LeaveEarlyTime"],
							" , ",
							this.getDecimalStr(dataRow["f_OvertimeTime"]),
							" , ",
							this.getDecimalStr(dataRow["f_AbsenceDay"]),
							" ) "
						}), this.cn))
						{
							if (this.cn.State == ConnectionState.Closed)
							{
								this.cn.Open();
							}
							sqlCommand.ExecuteNonQuery();
						}
						if (flag)
						{
							num3++;
						}
						string text5 = "";
						bool flag5 = true;
						bool flag6 = true;
						for (j = 0; j <= 3; j++)
						{
							switch (j)
							{
							case 0:
								text5 = this.SetObjToStr(dataRow["f_OnDuty1Desc"]);
								break;
							case 1:
								text5 = this.SetObjToStr(dataRow["f_OnDuty2Desc"]);
								break;
							case 2:
								text5 = this.SetObjToStr(dataRow["f_OffDuty1Desc"]);
								break;
							case 3:
								text5 = this.SetObjToStr(dataRow["f_OffDuty2Desc"]);
								break;
							}
							if (text5 == CommonStr.strLateness)
							{
								num5++;
								flag2 = false;
							}
							else if (text5 == CommonStr.strLeaveEarly)
							{
								num6++;
								flag2 = false;
							}
							else if (text5 == CommonStr.strNotReadCard)
							{
								num9++;
								flag2 = false;
							}
							else
							{
								if (text5 == CommonStr.strSignIn)
								{
									flag6 = false;
								}
								text5.IndexOf(CommonStr.strNotReadCard);
								int i = 0;
								while (i <= this.dtHolidayType.Rows.Count - 1 && i < array.Length)
								{
									if (text5 == this.dtHolidayType.Rows[i]["f_HolidayType"].ToString())
									{
										if ((int)this.dtHolidayType.Rows[i]["f_fullAttendance"] < 1)
										{
											flag2 = false;
										}
										else
										{
											flag5 = false;
										}
										array[i] += 0.25m;
										break;
									}
									i++;
								}
							}
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1"]) == "" && this.SetObjToStr(dataRow["f_OffDuty1"]) == "" && this.SetObjToStr(dataRow["f_OnDuty2"]) == "" && this.SetObjToStr(dataRow["f_OffDuty2"]) == "" && flag5)
						{
							flag2 = false;
						}
						if (!flag6 && !this.bManualRecordAsFullAttendance)
						{
							flag2 = false;
						}
						num7 += Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture);
						num8 += Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture);
						num10 += Convert.ToInt32(dataRow["f_LateTime"]);
						num11 += Convert.ToInt32(dataRow["f_LeaveEarlyTime"]);
						if (Convert.ToInt32(dataRow["f_LateTime"]) != 0 || Convert.ToInt32(dataRow["f_LeaveEarlyTime"]) != 0 || Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) != 0m)
						{
							flag2 = false;
						}
						if (this.bFullAttendanceSpecialA && (this.SetObjToStr(dataRow["f_OnDuty1"]) != "" || this.SetObjToStr(dataRow["f_OffDuty1"]) != "" || this.SetObjToStr(dataRow["f_OnDuty2"]) != "" || this.SetObjToStr(dataRow["f_OffDuty2"]) != ""))
						{
							flag2 = true;
						}
						if (flag2)
						{
							num4++;
						}
						dateTime3 = dateTime3.AddDays(1.0);
						Application.DoEvents();
					}
					string notes = this.getNotes((int)dataRow["f_ConsumerID"], dateTime, dateTime2);
					this.dvCardRecord.RowFilter = string.Format("f_Type ={0}", this.PrepareStr(CommonStr.strSignIn));
					text = " Insert Into t_d_AttStatistic ";
					text += " ( [f_ConsumerID], [f_AttDateStart], [f_AttDateEnd]  , [f_DayShouldWork],  [f_DayRealWork] , [f_TotalLate],  [f_TotalLeaveEarly],[f_TotalOvertime], [f_TotalAbsenceDay], [f_TotalNotReadCard]";
					for (int i = 1; i <= 32; i++)
					{
						object obj = text;
						text = string.Concat(new object[] { obj, " , [f_SpecialType", i, "]" });
					}
					text = string.Concat(new object[]
					{
						text,
						", f_LateMinutes, f_LeaveEarlyMinutes, f_ManualReadTimesCount, f_Notes )  Values( ",
						dataRow["f_ConsumerID"],
						" , ",
						this.PrepareStr(dateTime, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						this.PrepareStr(dateTime2, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						num3,
						" , ",
						num4,
						" , ",
						num5,
						" , ",
						num6,
						" , ",
						this.getDecimalStr(num8),
						" , ",
						this.getDecimalStr(num7),
						" , ",
						num9
					});
					for (int i = 0; i <= 31; i++)
					{
						text = text + " , " + wgTools.PrepareStrNUnicode(this.getDecimalStr(array[i]));
					}
					using (SqlCommand sqlCommand2 = new SqlCommand(string.Concat(new object[]
					{
						text,
						", ",
						num10,
						", ",
						num11,
						", ",
						this.dvCardRecord.Count,
						", ",
						wgTools.PrepareStrNUnicode(notes),
						" )"
					}), this.cn))
					{
						if (this.cn.State == ConnectionState.Closed)
						{
							this.cn.Open();
						}
						sqlCommand2.ExecuteNonQuery();
					}
				}
				sqlDataReader.Close();
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
				this.shiftAttReportImportFromAttendenceData();
				this.shiftAttStatisticImportFromAttStatistic();
				this.logCreateReport();
				if (this.CreateCompleteEvent != null)
				{
					this.CreateCompleteEvent(true, "");
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				try
				{
					if (this.CreateCompleteEvent != null)
					{
						this.CreateCompleteEvent(false, ex.ToString());
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x0022F070 File Offset: 0x0022E070
		private void make4OneTime()
		{
			this.cnConsumer = new SqlConnection(wgAppConfig.dbConString);
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			this.dtCardRecord2 = new DataTable();
			this.dsAtt = new DataSet("Attendance");
			this.dsAtt.Clear();
			this.daAttendenceData = new SqlDataAdapter("SELECT * FROM t_d_AttendenceData WHERE 1<0", this.cn);
			this.daHoliday = new SqlDataAdapter("SELECT * FROM t_a_Holiday ORDER BY  f_NO ASC", this.cn);
			this.daHolidayType = new SqlDataAdapter("SELECT *,0 as f_fullAttendance FROM t_a_HolidayType", this.cn);
			this.daLeave = new SqlDataAdapter("SELECT * FROM t_d_Leave", this.cn);
			this.daNoCardRecord = new SqlDataAdapter("SELECT f_ReadDate,f_Character,'' as f_Type  FROM t_d_ManualCardRecord Where 1<0 ", this.cn);
			this.daNoCardRecord.Fill(this.dsAtt, "AllCardRecords");
			this.dtCardRecord2 = this.dsAtt.Tables["AllCardRecords"];
			this.dtCardRecord2.Clear();
			this.daAttendenceData.Fill(this.dsAtt, "AttendenceData");
			this.dtAttendenceData = this.dsAtt.Tables["AttendenceData"];
			this.getAttendenceParam();
			this._clearAttendenceData();
			this._clearAttStatistic();
			this.daHoliday.Fill(this.dsAtt, "Holiday");
			this.dtHoliday = this.dsAtt.Tables["Holiday"];
			this.localizedHoliday(this.dtHoliday);
			this.dvHoliday = new DataView(this.dtHoliday);
			this.dvHoliday.RowFilter = "";
			this.dvHoliday.Sort = " f_NO ASC ";
			this.daLeave.Fill(this.dsAtt, "Leave");
			this.dtLeave = this.dsAtt.Tables["Leave"];
			this.dvLeave = new DataView(this.dtLeave);
			this.dvLeave.RowFilter = "";
			this.dvLeave.Sort = " f_NO ASC ";
			this.daHolidayType.Fill(this.dsAtt, "HolidayType");
			this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
			this.localizedHolidayType(this.dtHolidayType);
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 AND f_ShiftEnabled =0) ", this.cnConsumer);
			}
			else
			{
				this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 ) ", this.cnConsumer);
			}
			this.cnConsumer.Open();
			SqlDataReader sqlDataReader = this.cmdConsumer.ExecuteReader();
			int num = 0;
			try
			{
				int num2 = 0;
				while (sqlDataReader.Read())
				{
					num = (int)sqlDataReader["f_ConsumerID"];
					this.getDutyTime((int)((byte)sqlDataReader["f_AttendEnabled"]));
					num2++;
					string text = "SELECT f_ReadDate,f_Character,'' as f_Type  ";
					text = string.Concat(new string[]
					{
						text,
						" FROM t_d_SwipeRecord INNER JOIN t_b_Reader ON ( t_b_Reader.f_Attend=1 AND t_d_SwipeRecord.f_ReaderID =t_b_Reader.f_ReaderID ) ",
						wgAppConfig.getMoreCardShiftOneUserCondition(num),
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  AND t_b_Reader.f_Attend = 1 "
					});
					if (wgAppConfig.getSystemParamByNO(54) == "1")
					{
						text += " AND f_Character >= 1 ";
					}
					text += " ORDER BY f_ReadDate ASC ";
					this.daCardRecord = new SqlDataAdapter(text, this.cn);
					text = "SELECT f_ReadDate,f_Character ";
					text = string.Concat(new string[]
					{
						text,
						string.Format(", {0} as f_Type", wgTools.PrepareStrNUnicode(CommonStr.strSignIn)),
						" FROM t_d_ManualCardRecord   WHERE f_ConsumerID=",
						num.ToString(),
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  ORDER BY f_ReadDate ASC "
					});
					this.daManualCardRecord = new SqlDataAdapter(text, this.cn);
					decimal[] array = new decimal[32];
					DataRow dataRow = null;
					if (this.DealingNumEvent != null)
					{
						this.DealingNumEvent(num2);
					}
					this.gProcVal = num2 + 1;
					if (this.bStopCreate)
					{
						return;
					}
					DateTime dateTime = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
					DateTime dateTime2 = DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double)this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime));
					DateTime dateTime3 = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					decimal num7 = 0m;
					decimal num8 = 0m;
					int num9 = 0;
					int num10 = 0;
					int num11 = 0;
					for (int i = 0; i <= array.Length - 1; i++)
					{
						array[i] = 0m;
					}
					this.dtCardRecord2 = this.dsAtt.Tables["AllCardRecords"];
					this.dsAtt.Tables["AllCardRecords"].Clear();
					this.daCardRecord.Fill(this.dsAtt, "AllCardRecords");
					this.daManualCardRecord.Fill(this.dsAtt, "AllCardRecords");
					this.dvCardRecord = new DataView(this.dtCardRecord2);
					this.dvCardRecord.RowFilter = "";
					this.dvCardRecord.Sort = " f_ReadDate ASC ";
					int j = 0;
					while (this.dvCardRecord.Count > j + 1)
					{
						if (((DateTime)this.dvCardRecord[j + 1][0]).Subtract((DateTime)this.dvCardRecord[j][0]).TotalSeconds < (double)this.tTwoReadMintime)
						{
							this.dvCardRecord[j + 1].Delete();
						}
						else
						{
							j++;
						}
					}
					while (dateTime3 <= DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double)this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime)))
					{
						dataRow = this.dtAttendenceData.NewRow();
						dataRow["f_ConsumerID"] = num;
						dataRow["f_AttDate"] = dateTime3;
						dataRow["f_LateTime"] = 0;
						dataRow["f_LeaveEarlyTime"] = 0;
						dataRow["f_OvertimeTime"] = 0;
						dataRow["f_AbsenceDay"] = 0;
						bool flag = true;
						bool flag2 = true;
						bool flag3 = false;
						this.dvCardRecord.RowFilter = "  f_ReadDate >= #" + dateTime3.ToString("yyyy-MM-dd HH:mm:ss") + "# and f_ReadDate<= " + Strings.Format(dateTime3.AddDays((double)this.normalDay), "#yyyy-MM-dd " + this.strAllowOffdutyTime + "#");
						this.dvLeave.RowFilter = " f_ConsumerID = " + num.ToString();
						if (this.dvCardRecord.Count > 0)
						{
							DateTime dateTime4 = Convert.ToDateTime(this.dvCardRecord[0]["f_ReadDate"]);
							dataRow["f_Onduty1"] = dateTime4;
							dataRow["f_Onduty1Desc"] = this.dvCardRecord[0]["f_Type"];
							if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty0.AddMinutes((double)this.tLateTimeout), "HH:mm:ss")) > 0)
							{
								if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty0.AddMinutes((double)this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
								{
									dataRow["f_Onduty1Desc"] = CommonStr.strLateness;
								}
								else
								{
									dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
								}
							}
						}
						else
						{
							dataRow["f_OnDuty1Desc"] = CommonStr.strAbsence;
						}
						int num12 = 3;
						this.dvHoliday.RowFilter = " f_NO =1 ";
						if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
						{
							num12 = 0;
						}
						else
						{
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
							{
								num12 = 1;
							}
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
							{
								num12 = 2;
							}
							this.dvHoliday.RowFilter = " f_NO =2 ";
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
							{
								num12 = 0;
							}
							else
							{
								if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
								{
									num12 = 1;
								}
								if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
								{
									num12 = 2;
								}
								this.dvHoliday.RowFilter = " f_TYPE =2 ";
								for (int k = 0; k <= this.dvHoliday.Count - 1; k++)
								{
									this.strTemp = Convert.ToString(this.dvHoliday[k]["f_Value"]);
									this.strTemp = this.strTemp + " " + ((this.dvHoliday[k]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
									DateTime dateTime5;
									DateTime.TryParse(this.strTemp, out dateTime5);
									this.strTemp = Convert.ToString(this.dvHoliday[k]["f_Value2"]);
									this.strTemp = this.strTemp + " " + ((this.dvHoliday[k]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
									DateTime dateTime6;
									DateTime.TryParse(this.strTemp, out dateTime6);
									if (dateTime5 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime6)
									{
										num12 = 0;
										break;
									}
									if (dateTime5 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= dateTime6)
									{
										num12 = 2;
									}
									if (dateTime5 <= DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime6)
									{
										num12 = 1;
									}
								}
							}
						}
						if (num12 != 3)
						{
							this.dvHoliday.RowFilter = " f_TYPE =3 ";
							for (int l = 0; l <= this.dvHoliday.Count - 1; l++)
							{
								this.strTemp = Convert.ToString(this.dvHoliday[l]["f_Value"]);
								this.strTemp = this.strTemp + " " + ((this.dvHoliday[l]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
								DateTime dateTime7;
								DateTime.TryParse(this.strTemp, out dateTime7);
								this.strTemp = Convert.ToString(this.dvHoliday[l]["f_Value2"]);
								this.strTemp = this.strTemp + " " + ((this.dvHoliday[l]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
								DateTime dateTime8;
								DateTime.TryParse(this.strTemp, out dateTime8);
								if (dateTime7 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime8)
								{
									num12 = 3;
									break;
								}
								if (dateTime7 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= dateTime8)
								{
									if (num12 == 2)
									{
										num12 = 3;
									}
									else
									{
										num12 = 1;
									}
								}
								if (dateTime7 <= DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:01")) && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime8)
								{
									if (num12 == 1)
									{
										num12 = 3;
									}
									else
									{
										num12 = 2;
									}
								}
							}
						}
						switch (num12)
						{
						case 0:
							dataRow["f_Onduty1"] = DBNull.Value;
							dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							flag = false;
							flag2 = false;
							break;
						case 1:
							break;
						default:
							if (num12 == 2 && this.SetObjToStr(dataRow["f_Onduty1"]) != "")
							{
								if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Onduty1"]).AddMinutes((double)(-(double)this.tlateAbsenceTimeout)), "HH:mm:ss"), "13:30:00") > 0)
								{
									flag3 = true;
									dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
								}
								else if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Onduty1"]).AddMinutes((double)(-(double)this.tLateTimeout)), "HH:mm:ss"), "13:30:00") > 0)
								{
									flag3 = true;
									dataRow["f_Onduty1Desc"] = CommonStr.strLateness;
								}
								else
								{
									dataRow["f_Onduty1Desc"] = "";
								}
							}
							break;
						}
						if (this.dvLeave.Count > 0)
						{
							DateTime now = DateTime.Now;
							DateTime now2 = DateTime.Now;
							string text2 = "";
							num12 = 3;
							for (int m = 0; m <= this.dvLeave.Count - 1; m++)
							{
								this.strTemp = Convert.ToString(this.dvLeave[m]["f_Value"]);
								this.strTemp = this.strTemp + " " + ((this.dvLeave[m]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
								DateTime.TryParse(this.strTemp, out now);
								this.strTemp = Convert.ToString(this.dvLeave[m]["f_Value2"]);
								this.strTemp = this.strTemp + " " + ((this.dvLeave[m]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
								DateTime.TryParse(this.strTemp, out now2);
								string text3 = Convert.ToString(this.dvLeave[m]["f_HolidayType"]);
								if (now <= dateTime3 && Convert.ToDateTime(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= now2)
								{
									text2 = text3;
									num12 = 0;
									break;
								}
								if (now <= dateTime3 && Convert.ToDateTime(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= now2)
								{
									text2 = text3;
									if (num12 == 1)
									{
										num12 = 0;
										break;
									}
									num12 = 2;
								}
								if (now <= Convert.ToDateTime(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:01")) && Convert.ToDateTime(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= now2)
								{
									if (num12 == 2)
									{
										num12 = 0;
										break;
									}
									num12 = 1;
								}
							}
							switch (num12)
							{
							case 0:
								dataRow["f_OnDuty1Desc"] = text2;
								dataRow["f_OnDuty1"] = DBNull.Value;
								break;
							case 1:
								break;
							default:
								if (num12 == 2 && this.SetObjToStr(dataRow["f_Onduty1"]) != "")
								{
									dataRow["f_OnDuty1Desc"] = text2;
									if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Onduty1"]).AddMinutes((double)(-(double)this.tlateAbsenceTimeout)), "HH:mm:ss"), "13:30:00") > 0)
									{
										flag3 = true;
										dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
									}
									else if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Onduty1"]).AddMinutes((double)(-(double)this.tLateTimeout)), "HH:mm:ss"), "13:30:00") > 0)
									{
										flag3 = true;
										dataRow["f_Onduty1Desc"] = CommonStr.strLateness;
									}
								}
								break;
							}
						}
						if (this.bChooseTwoTimes)
						{
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strLateness)
							{
								dataRow["f_OnDuty1Desc"] = "";
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)
							{
								dataRow["f_OffDuty1Desc"] = "";
							}
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strOvertime)
							{
								dataRow["f_OnDuty1Desc"] = "";
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strOvertime)
							{
								dataRow["f_OffDuty1Desc"] = "";
							}
							dataRow["f_OvertimeTime"] = 0;
							if (this.SetObjToStr(dataRow["f_OnDuty1"]) != "" && this.SetObjToStr(dataRow["f_OffDuty1"]) != "")
							{
								dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_OnDuty1"], "yyyy-MM-dd HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
							}
							flag2 = (decimal)dataRow["f_OvertimeTime"] >= this.needDutyHour;
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strLateness)
						{
							if (flag3)
							{
								dataRow["f_LateTime"] = (long)Convert.ToInt32(dataRow["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty0, "13:30:00")), DateTime.Parse(Strings.Format(dataRow["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
							}
							else
							{
								dataRow["f_LateTime"] = (long)Convert.ToInt32(dataRow["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty0, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
							}
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0)
						{
							dataRow["f_AbsenceDay"] = this.tLateAbsenceDay + this.tLeaveAbsenceDay;
							flag2 = false;
						}
						if (Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) >= 1m && num12 != 3)
						{
							dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) / 2.0m;
						}
						text = " INSERT INTO t_d_AttendenceData ";
						using (SqlCommand sqlCommand = new SqlCommand(string.Concat(new object[]
						{
							text,
							" ([f_ConsumerID], [f_AttDate], [f_Onduty1],[f_Onduty1Desc], [f_Offduty1], [f_Offduty1Desc], [f_LateTime], [f_LeaveEarlyTime],[f_OvertimeTime], [f_AbsenceDay]   )  VALUES ( ",
							dataRow["f_ConsumerID"],
							" , ",
							this.PrepareStr(dataRow["f_AttDate"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							this.PrepareStr(dataRow["f_Onduty1"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Onduty1Desc"]),
							" , ",
							this.PrepareStr(dataRow["f_Offduty1"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Offduty1Desc"]),
							" , ",
							dataRow["f_LateTime"],
							" , ",
							dataRow["f_LeaveEarlyTime"],
							" , ",
							this.getDecimalStr(dataRow["f_OvertimeTime"]),
							" , ",
							this.getDecimalStr(dataRow["f_AbsenceDay"]),
							" ) "
						}), this.cn))
						{
							if (this.cn.State == ConnectionState.Closed)
							{
								this.cn.Open();
							}
							sqlCommand.ExecuteNonQuery();
						}
						if (flag)
						{
							num3++;
						}
						string text4 = "";
						bool flag4 = true;
						bool flag5 = true;
						for (j = 0; j <= 1; j++)
						{
							switch (j)
							{
							case 0:
								text4 = this.SetObjToStr(dataRow["f_OnDuty1Desc"]);
								break;
							case 1:
								text4 = this.SetObjToStr(dataRow["f_OffDuty1Desc"]);
								break;
							}
							if (text4 == CommonStr.strLateness)
							{
								num5++;
								flag2 = false;
							}
							else if (text4 == CommonStr.strLeaveEarly)
							{
								num6++;
								flag2 = false;
							}
							else if (text4 == CommonStr.strNotReadCard)
							{
								num9++;
								flag2 = false;
							}
							else
							{
								if (text4 == CommonStr.strSignIn)
								{
									flag5 = false;
								}
								int i = 0;
								while (i <= this.dtHolidayType.Rows.Count - 1 && i < array.Length)
								{
									if (text4 == Convert.ToString(this.dtHolidayType.Rows[i]["f_HolidayType"]))
									{
										if ((int)this.dtHolidayType.Rows[i]["f_fullAttendance"] < 1)
										{
											flag2 = false;
										}
										else
										{
											flag4 = false;
										}
										if (this.bChooseOnlyOnDuty)
										{
											array[i] += 1.0m;
											break;
										}
										array[i] += 0.5m;
										break;
									}
									else
									{
										i++;
									}
								}
							}
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1"]) == "" && this.SetObjToStr(dataRow["f_OffDuty1"]) == "" && flag4)
						{
							flag2 = false;
						}
						if (!flag5 && !this.bManualRecordAsFullAttendance)
						{
							flag2 = false;
						}
						num7 += Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture);
						num8 += Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture);
						num10 += Convert.ToInt32(dataRow["f_LateTime"]);
						num11 += Convert.ToInt32(dataRow["f_LeaveEarlyTime"]);
						if (Convert.ToInt32(dataRow["f_LateTime"]) != 0 || Convert.ToInt32(dataRow["f_LeaveEarlyTime"]) != 0 || Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) != 0m)
						{
							flag2 = false;
						}
						if (this.bFullAttendanceSpecialA && (this.SetObjToStr(dataRow["f_OnDuty1"]) != "" || this.SetObjToStr(dataRow["f_OffDuty1"]) != "" || this.SetObjToStr(dataRow["f_OnDuty2"]) != "" || this.SetObjToStr(dataRow["f_OffDuty2"]) != ""))
						{
							flag2 = true;
						}
						if (flag2)
						{
							num4++;
						}
						dateTime3 = dateTime3.AddDays(1.0);
						Application.DoEvents();
					}
					this.dvCardRecord.RowFilter = string.Format("f_Type ={0}", this.PrepareStr(CommonStr.strSignIn));
					string notes = this.getNotes((int)dataRow["f_ConsumerID"], dateTime, dateTime2);
					text = " Insert Into t_d_AttStatistic ";
					text += " ( [f_ConsumerID], [f_AttDateStart], [f_AttDateEnd]  , [f_DayShouldWork],  [f_DayRealWork] , [f_TotalLate],  [f_TotalLeaveEarly],[f_TotalOvertime], [f_TotalAbsenceDay], [f_TotalNotReadCard]";
					for (int i = 1; i <= 32; i++)
					{
						object obj = text;
						text = string.Concat(new object[] { obj, " , [f_SpecialType", i, "]" });
					}
					text = string.Concat(new object[]
					{
						text,
						", f_LateMinutes, f_LeaveEarlyMinutes, f_ManualReadTimesCount, f_Notes )  Values( ",
						dataRow["f_ConsumerID"],
						" , ",
						this.PrepareStr(dateTime, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						this.PrepareStr(dateTime2, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						num3,
						" , ",
						num4,
						" , ",
						num5,
						" , ",
						num6,
						" , ",
						this.getDecimalStr(num8),
						" , ",
						this.getDecimalStr(num7),
						" , ",
						num9
					});
					for (int i = 0; i <= 31; i++)
					{
						text = text + " , " + wgTools.PrepareStrNUnicode(this.getDecimalStr(array[i]));
					}
					using (SqlCommand sqlCommand2 = new SqlCommand(string.Concat(new object[]
					{
						text,
						", ",
						num10,
						", ",
						num11,
						", ",
						this.dvCardRecord.Count,
						", ",
						wgTools.PrepareStrNUnicode(notes),
						" )"
					}), this.cn))
					{
						if (this.cn.State == ConnectionState.Closed)
						{
							this.cn.Open();
						}
						sqlCommand2.ExecuteNonQuery();
					}
				}
				sqlDataReader.Close();
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
				this.shiftAttReportImportFromAttendenceData();
				this.shiftAttStatisticImportFromAttStatistic();
				this.logCreateReport();
				if (this.CreateCompleteEvent != null)
				{
					this.CreateCompleteEvent(true, "");
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				try
				{
					if (this.CreateCompleteEvent != null)
					{
						this.CreateCompleteEvent(false, ex.ToString());
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x00230E38 File Offset: 0x0022FE38
		private void make4TwoTimes()
		{
			this.cnConsumer = new SqlConnection(wgAppConfig.dbConString);
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			this.dtCardRecord = new DataTable();
			this.dsAtt = new DataSet("Attendance");
			this.dsAtt.Clear();
			this.daAttendenceData = new SqlDataAdapter("SELECT * FROM t_d_AttendenceData WHERE 1<0", this.cn);
			this.daHoliday = new SqlDataAdapter("SELECT * FROM t_a_Holiday ORDER BY  f_NO ASC", this.cn);
			this.daHolidayType = new SqlDataAdapter("SELECT *,0 as f_fullAttendance FROM t_a_HolidayType", this.cn);
			this.daLeave = new SqlDataAdapter("SELECT * FROM t_d_Leave", this.cn);
			this.daNoCardRecord = new SqlDataAdapter("SELECT f_ReadDate,f_Character,'' as f_Type  FROM t_d_ManualCardRecord Where 1<0 ", this.cn);
			this.daNoCardRecord.Fill(this.dsAtt, "AllCardRecords");
			this.dtCardRecord = this.dsAtt.Tables["AllCardRecords"];
			this.dtCardRecord.Clear();
			this.daAttendenceData.Fill(this.dsAtt, "AttendenceData");
			this.dtAttendenceData = this.dsAtt.Tables["AttendenceData"];
			this.getAttendenceParam();
			this._clearAttendenceData();
			this._clearAttStatistic();
			this.daHoliday.Fill(this.dsAtt, "Holiday");
			this.dtHoliday = this.dsAtt.Tables["Holiday"];
			this.localizedHoliday(this.dtHoliday);
			this.dvHoliday = new DataView(this.dtHoliday);
			this.dvHoliday.RowFilter = "";
			this.dvHoliday.Sort = " f_NO ASC ";
			this.daLeave.Fill(this.dsAtt, "Leave");
			this.dtLeave = this.dsAtt.Tables["Leave"];
			this.dvLeave = new DataView(this.dtLeave);
			this.dvLeave.RowFilter = "";
			this.dvLeave.Sort = " f_NO ASC ";
			this.daHolidayType.Fill(this.dsAtt, "HolidayType");
			this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
			this.localizedHolidayType(this.dtHolidayType);
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 AND f_ShiftEnabled =0) ", this.cnConsumer);
			}
			else
			{
				this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 ) ", this.cnConsumer);
			}
			this.cnConsumer.Open();
			SqlDataReader sqlDataReader = this.cmdConsumer.ExecuteReader();
			int num = 0;
			try
			{
				int num2 = 0;
				while (sqlDataReader.Read())
				{
					num = (int)sqlDataReader["f_ConsumerID"];
					this.getDutyTime((int)((byte)sqlDataReader["f_AttendEnabled"]));
					num2++;
					string text = "SELECT f_ReadDate,f_Character,'' as f_Type  ";
					text = string.Concat(new string[]
					{
						text,
						", t_b_Reader.f_DutyOnOff  FROM t_d_SwipeRecord INNER JOIN t_b_Reader ON t_b_Reader.f_Attend=1 AND t_d_SwipeRecord.f_ReaderID =t_b_Reader.f_ReaderID ",
						wgAppConfig.getMoreCardShiftOneUserCondition(num),
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  AND t_b_Reader.f_Attend = 1 "
					});
					if (wgAppConfig.getSystemParamByNO(54) == "1")
					{
						text += " AND f_Character >= 1 ";
					}
					text += " ORDER BY f_ReadDate ASC ";
					this.daCardRecord = new SqlDataAdapter(text, this.cn);
					text = "SELECT f_ReadDate,f_Character ";
					text = string.Concat(new string[]
					{
						text,
						string.Format(", {0} as f_Type", wgTools.PrepareStrNUnicode(CommonStr.strSignIn)),
						", 3 as f_DutyOnOff  FROM t_d_ManualCardRecord   WHERE f_ConsumerID=",
						num.ToString(),
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  ORDER BY f_ReadDate ASC "
					});
					this.daManualCardRecord = new SqlDataAdapter(text, this.cn);
					decimal[] array = new decimal[32];
					DataRow dataRow = null;
					if (this.DealingNumEvent != null)
					{
						this.DealingNumEvent(num2);
					}
					this.gProcVal = num2 + 1;
					if (this.bStopCreate)
					{
						return;
					}
					DateTime dateTime = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
					DateTime dateTime2 = DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double)this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime));
					DateTime dateTime3 = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					decimal num7 = 0m;
					decimal num8 = 0m;
					int num9 = 0;
					int num10 = 0;
					int num11 = 0;
					for (int i = 0; i <= array.Length - 1; i++)
					{
						array[i] = 0m;
					}
					this.dtCardRecord = this.dsAtt.Tables["AllCardRecords"];
					this.dsAtt.Tables["AllCardRecords"].Clear();
					this.daCardRecord.Fill(this.dsAtt, "AllCardRecords");
					this.daManualCardRecord.Fill(this.dsAtt, "AllCardRecords");
					this.dvCardRecord = new DataView(this.dtCardRecord);
					this.dvCardRecord.RowFilter = "";
					this.dvCardRecord.Sort = " f_ReadDate ASC ";
					int j = 0;
					while (this.dvCardRecord.Count > j + 1)
					{
						if (((DateTime)this.dvCardRecord[j + 1][0]).Subtract((DateTime)this.dvCardRecord[j][0]).TotalSeconds < (double)this.tTwoReadMintime && Convert.ToInt32(this.dvCardRecord[j]["f_DutyOnOff"]) == Convert.ToInt32(this.dvCardRecord[j + 1]["f_DutyOnOff"]))
						{
							this.dvCardRecord[j + 1].Delete();
						}
						else
						{
							j++;
						}
					}
					while (dateTime3 <= DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double)this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime)))
					{
						dataRow = this.dtAttendenceData.NewRow();
						dataRow["f_ConsumerID"] = num;
						dataRow["f_AttDate"] = dateTime3;
						dataRow["f_LateTime"] = 0;
						dataRow["f_LeaveEarlyTime"] = 0;
						dataRow["f_OvertimeTime"] = 0;
						dataRow["f_AbsenceDay"] = 0;
						bool flag = true;
						bool flag2 = true;
						bool flag3 = false;
						bool flag4 = false;
						this.dvCardRecord.RowFilter = "  f_ReadDate >= #" + dateTime3.ToString("yyyy-MM-dd HH:mm:ss") + "# and f_ReadDate<= " + Strings.Format(dateTime3.AddDays((double)this.normalDay), "#yyyy-MM-dd " + this.strAllowOffdutyTime + "#");
						this.dvLeave.RowFilter = " f_ConsumerID = " + num.ToString();
						if (this.dvCardRecord.Count > 0)
						{
							int k = 0;
							while (k <= this.dvCardRecord.Count - 1)
							{
								DateTime dateTime4 = Convert.ToDateTime(this.dvCardRecord[k]["f_ReadDate"]);
								if (string.Compare(Strings.Format(dateTime4, "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
								{
									if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty0, "HH:mm:ss")) <= 0)
									{
										if (Convert.ToInt32(this.dvCardRecord[k]["f_DutyOnOff"]) != 2)
										{
											if (this.bEarliestAsOnDuty || this.bChooseTwoTimes)
											{
												if (this.SetObjToStr(dataRow["f_Onduty1"]) == "")
												{
													dataRow["f_Onduty1"] = dateTime4;
													dataRow["f_Onduty1Desc"] = this.dvCardRecord[k]["f_Type"];
												}
											}
											else
											{
												dataRow["f_Onduty1"] = dateTime4;
												dataRow["f_Onduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											}
										}
										k++;
										continue;
									}
									if (this.SetObjToStr(dataRow["f_Onduty1"]) == "")
									{
										if (Convert.ToInt32(this.dvCardRecord[k]["f_DutyOnOff"]) == 2)
										{
											dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
										}
										else if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty0.AddMinutes((double)this.tLateTimeout), "HH:mm:ss")) <= 0)
										{
											dataRow["f_Onduty1"] = dateTime4;
											dataRow["f_Onduty1Desc"] = this.dvCardRecord[k]["f_Type"];
										}
										else if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOnduty0.AddMinutes((double)this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
										{
											dataRow["f_Onduty1"] = dateTime4;
											dataRow["f_Onduty1Desc"] = CommonStr.strLateness;
										}
										else if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOffduty0.AddMinutes((double)(-(double)this.tLeaveTimeout)), "HH:mm:ss")) > 0)
										{
											dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
										}
										else
										{
											dataRow["f_Onduty1"] = dateTime4;
											dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
										}
									}
								}
								else if (!(this.SetObjToStr(dataRow["f_Onduty1"]) != ""))
								{
									dataRow["f_Onduty1Desc"] = CommonStr.strNotReadCard;
								}
								IL_0CF6:
								while (k <= this.dvCardRecord.Count - 1)
								{
									if (Convert.ToInt32(this.dvCardRecord[k]["f_DutyOnOff"]) != 1)
									{
										dateTime4 = Convert.ToDateTime(this.dvCardRecord[k]["f_ReadDate"]);
										if (string.Compare(Strings.Format(dateTime4, "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
										{
											if (string.Compare(Strings.Format(dateTime4, "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) <= 0)
											{
												dataRow["f_Offduty1"] = dateTime4;
												dataRow["f_Offduty1Desc"] = this.dvCardRecord[k]["f_Type"];
												if (string.Compare(Strings.Format(dateTime4.AddMinutes((double)this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) < 0)
												{
													if (string.Compare(Strings.Format(dateTime4.AddMinutes((double)this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) < 0)
													{
														dataRow["f_Offduty1Desc"] = CommonStr.strAbsence;
													}
													else
													{
														dataRow["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
													}
												}
											}
											else
											{
												dataRow["f_Offduty1"] = dateTime4;
												dataRow["f_Offduty1Desc"] = this.dvCardRecord[k]["f_Type"];
											}
										}
										else
										{
											dataRow["f_Offduty1"] = dateTime4;
											dataRow["f_Offduty1Desc"] = this.dvCardRecord[k]["f_Type"];
										}
									}
									k++;
								}
								if (this.SetObjToStr(dataRow["f_Offduty1"]) == this.SetObjToStr(dataRow["f_Onduty1"]) && (this.SetObjToStr(dataRow["f_Offduty1Desc"]).IndexOf(CommonStr.strLeaveEarly) >= 0 || this.SetObjToStr(dataRow["f_Offduty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0))
								{
									dataRow["f_Offduty1"] = DBNull.Value;
									dataRow["f_Offduty1Desc"] = CommonStr.strNotReadCard;
								}
								if (this.SetObjToStr(dataRow["f_Offduty1"]) == "" && this.SetObjToStr(dataRow["f_Onduty1"]) == "")
								{
									dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
									dataRow["f_Offduty1Desc"] = CommonStr.strAbsence;
								}
								if (this.SetObjToStr(dataRow["f_Offduty1"]) == "" && this.SetObjToStr(dataRow["f_Offduty1Desc"]) == "")
								{
									dataRow["f_Offduty1Desc"] = CommonStr.strNotReadCard;
								}
								if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0 && this.SetObjToStr(dataRow["f_OffDuty1Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
								{
									dataRow["f_OnDuty1Desc"] = "";
									goto IL_0EB3;
								}
								goto IL_0EB3;
							}
							goto IL_0CF6;
						}
						dataRow["f_OnDuty1Desc"] = CommonStr.strAbsence;
						dataRow["f_OffDuty1Desc"] = CommonStr.strAbsence;
						IL_0EB3:
						int num12 = 3;
						this.dvHoliday.RowFilter = " f_NO =1 ";
						if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
						{
							num12 = 0;
						}
						else
						{
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
							{
								num12 = 1;
							}
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2 && dateTime3.DayOfWeek == DayOfWeek.Saturday)
							{
								num12 = 2;
							}
							this.dvHoliday.RowFilter = " f_NO =2 ";
							if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
							{
								num12 = 0;
							}
							else
							{
								if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
								{
									num12 = 1;
								}
								if (Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2 && dateTime3.DayOfWeek == DayOfWeek.Sunday)
								{
									num12 = 2;
								}
								this.dvHoliday.RowFilter = " f_TYPE =2 ";
								for (int l = 0; l <= this.dvHoliday.Count - 1; l++)
								{
									this.strTemp = Convert.ToString(this.dvHoliday[l]["f_Value"]);
									this.strTemp = this.strTemp + " " + ((this.dvHoliday[l]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
									DateTime dateTime5;
									DateTime.TryParse(this.strTemp, out dateTime5);
									this.strTemp = Convert.ToString(this.dvHoliday[l]["f_Value2"]);
									this.strTemp = this.strTemp + " " + ((this.dvHoliday[l]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
									DateTime dateTime6;
									DateTime.TryParse(this.strTemp, out dateTime6);
									if (dateTime5 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime6)
									{
										num12 = 0;
										break;
									}
									if (dateTime5 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= dateTime6)
									{
										num12 = 2;
									}
									if (dateTime5 <= DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime6)
									{
										num12 = 1;
									}
								}
							}
						}
						if (num12 != 3)
						{
							this.dvHoliday.RowFilter = " f_TYPE =3 ";
							for (int m = 0; m <= this.dvHoliday.Count - 1; m++)
							{
								this.strTemp = Convert.ToString(this.dvHoliday[m]["f_Value"]);
								this.strTemp = this.strTemp + " " + ((this.dvHoliday[m]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
								DateTime dateTime7;
								DateTime.TryParse(this.strTemp, out dateTime7);
								this.strTemp = Convert.ToString(this.dvHoliday[m]["f_Value2"]);
								this.strTemp = this.strTemp + " " + ((this.dvHoliday[m]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
								DateTime dateTime8;
								DateTime.TryParse(this.strTemp, out dateTime8);
								if (dateTime7 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime8)
								{
									num12 = 3;
									break;
								}
								if (dateTime7 <= dateTime3 && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= dateTime8)
								{
									if (num12 == 2)
									{
										num12 = 3;
									}
									else
									{
										num12 = 1;
									}
								}
								if (dateTime7 <= DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:01")) && DateTime.Parse(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= dateTime8)
								{
									if (num12 == 1)
									{
										num12 = 3;
									}
									else
									{
										num12 = 2;
									}
								}
							}
						}
						switch (num12)
						{
						case 0:
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strNotReadCard || this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							}
							dataRow["f_OnDuty1Desc"] = CommonStr.strRest;
							dataRow["f_OffDuty1Desc"] = CommonStr.strRest;
							if (this.SetObjToStr(dataRow["f_Onduty1"]) != "" && this.SetObjToStr(dataRow["f_Offduty1"]) != "")
							{
								dataRow["f_OnDuty1Desc"] = CommonStr.strOvertime;
								dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
								if (string.Compare(Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
								{
									dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-1 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
								else
								{
									dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
							}
							flag = false;
							flag2 = false;
							break;
						case 1:
							if (this.SetObjToStr(dataRow["f_Offduty1"]) != "")
							{
								if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Offduty1"]).AddMinutes((double)this.tLeaveTimeout), "HH:mm:ss"), "12:00:00") < 0)
								{
									if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Offduty1"]).AddMinutes((double)this.tLeaveAbsenceTimeout), "HH:mm:ss"), "12:00:00") < 0)
									{
										flag4 = true;
										dataRow["f_Offduty1Desc"] = CommonStr.strAbsence;
									}
									else
									{
										flag4 = true;
										dataRow["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
									}
								}
								else
								{
									dataRow["f_Offduty1Desc"] = "";
								}
							}
							if (this.SetObjToStr(dataRow["f_Offduty1"]) != "")
							{
								if (Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(dateTime3, "yyyy-MM-dd"))
								{
									if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Offduty1"]).AddMinutes((double)(-(double)this.tOvertimeTimeout)), "HH:mm:ss"), "12:00:00") >= 0)
									{
										dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
										dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse("2000-1-1 12:00:00"), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-1 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
								else
								{
									dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
									dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "2000-1-1 12:00:00")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
							}
							break;
						case 2:
							if (this.SetObjToStr(dataRow["f_Onduty1"]) != "")
							{
								if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Onduty1"]).AddMinutes((double)(-(double)this.tlateAbsenceTimeout)), "HH:mm:ss"), "13:30:00") > 0)
								{
									flag3 = true;
									dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
								}
								else if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Onduty1"]).AddMinutes((double)(-(double)this.tLateTimeout)), "HH:mm:ss"), "13:30:00") > 0)
								{
									flag3 = true;
									dataRow["f_Onduty1Desc"] = CommonStr.strLateness;
								}
								else
								{
									dataRow["f_Onduty1Desc"] = "";
								}
								if (this.SetObjToStr(dataRow["f_Offduty1"]) != "")
								{
									if (string.Compare(Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
									{
										if (string.Compare(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss"), Strings.Format(this.tOffduty0.AddMinutes((double)this.tOvertimeTimeout), "HH:mm:ss")) >= 0 && string.Compare(Strings.Format(this.tOffduty0.AddMinutes((double)this.tOvertimeTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) >= 0)
										{
											dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
											dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
										}
									}
									else
									{
										dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
										dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
							}
							break;
						default:
							if (num12 == 2)
							{
								if (this.SetObjToStr(dataRow["f_Onduty1"]) != "")
								{
									if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Onduty1"]).AddMinutes((double)(-(double)this.tlateAbsenceTimeout)), "HH:mm:ss"), "13:30:00") > 0)
									{
										flag3 = true;
										dataRow["f_Onduty1Desc"] = CommonStr.strAbsence;
									}
									else if (string.Compare(Strings.Format(Convert.ToDateTime(dataRow["f_Onduty1"]).AddMinutes((double)(-(double)this.tLateTimeout)), "HH:mm:ss"), "13:30:00") > 0)
									{
										flag3 = true;
										dataRow["f_Onduty1Desc"] = CommonStr.strLateness;
									}
									else
									{
										dataRow["f_Onduty1Desc"] = "";
									}
									if (this.SetObjToStr(dataRow["f_Offduty1"]) != "")
									{
										if (string.Compare(Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
										{
											if (string.Compare(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss"), Strings.Format(this.tOffduty0.AddMinutes((double)this.tOvertimeTimeout), "HH:mm:ss")) >= 0 && string.Compare(Strings.Format(this.tOffduty0.AddMinutes((double)this.tOvertimeTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) >= 0)
											{
												dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
												dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
											}
										}
										else
										{
											dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
											dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
										}
									}
								}
							}
							else if (num12 == 3 && this.SetObjToStr(dataRow["f_Offduty1"]) != "")
							{
								if (string.Compare(Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd"), Strings.Format(dateTime3, "yyyy-MM-dd")) == 0)
								{
									if (string.Compare(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss"), Strings.Format(this.tOffduty0.AddMinutes((double)this.tOvertimeTimeout), "HH:mm:ss")) >= 0 && string.Compare(Strings.Format(this.tOffduty0.AddMinutes((double)this.tOvertimeTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) >= 0)
									{
										dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
										dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
									}
								}
								else
								{
									dataRow["f_OffDuty1Desc"] = CommonStr.strOvertime;
									dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
								}
							}
							break;
						}
						if (this.dvLeave.Count > 0)
						{
							DateTime now = DateTime.Now;
							DateTime now2 = DateTime.Now;
							string text2 = "";
							string text3 = "";
							num12 = 3;
							for (int n = 0; n <= this.dvLeave.Count - 1; n++)
							{
								this.strTemp = Convert.ToString(this.dvLeave[n]["f_Value"]);
								this.strTemp = this.strTemp + " " + ((this.dvLeave[n]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
								DateTime.TryParse(this.strTemp, out now);
								this.strTemp = Convert.ToString(this.dvLeave[n]["f_Value2"]);
								this.strTemp = this.strTemp + " " + ((this.dvLeave[n]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
								DateTime.TryParse(this.strTemp, out now2);
								string text4 = Convert.ToString(this.dvLeave[n]["f_HolidayType"]);
								if (now <= dateTime3 && Convert.ToDateTime(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= now2)
								{
									text2 = text4;
									text3 = text4;
									num12 = 0;
									break;
								}
								if (now <= dateTime3 && Convert.ToDateTime(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:00")) <= now2)
								{
									text2 = text4;
									if (num12 == 1)
									{
										num12 = 0;
										break;
									}
									num12 = 2;
								}
								if (now <= Convert.ToDateTime(Strings.Format(dateTime3, "yyyy-MM-dd 12:00:01")) && Convert.ToDateTime(Strings.Format(dateTime3, "yyyy-MM-dd 23:59:59")) <= now2)
								{
									text3 = text4;
									if (num12 == 2)
									{
										num12 = 0;
										break;
									}
									num12 = 1;
								}
							}
							switch (num12)
							{
							case 0:
								dataRow["f_OnDuty1Desc"] = text2;
								dataRow["f_OffDuty1Desc"] = text3;
								dataRow["f_OnDuty1"] = DBNull.Value;
								dataRow["f_OffDuty1"] = DBNull.Value;
								break;
							case 1:
								dataRow["f_OffDuty1Desc"] = text3;
								dataRow["f_OffDuty1"] = DBNull.Value;
								break;
							default:
								if (num12 == 2)
								{
									if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]) == CommonStr.strNotReadCard)
									{
										if (this.SetObjToStr(dataRow["f_OnDuty1"]) != "")
										{
											dataRow["f_OnDuty2"] = dataRow["f_OnDuty1"];
											dataRow["f_OnDuty1"] = DBNull.Value;
											dataRow["f_OffDuty1"] = DBNull.Value;
										}
										else if (this.SetObjToStr(dataRow["f_OffDuty1"]) != "")
										{
											dataRow["f_OnDuty2"] = dataRow["f_OffDuty1"];
											dataRow["f_OffDuty1"] = DBNull.Value;
										}
									}
									dataRow["f_OnDuty1Desc"] = text2;
									dataRow["f_OnDuty1"] = DBNull.Value;
								}
								break;
							}
						}
						if (this.bChooseTwoTimes)
						{
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strLateness)
							{
								dataRow["f_OnDuty1Desc"] = "";
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)
							{
								dataRow["f_OffDuty1Desc"] = "";
							}
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strOvertime)
							{
								dataRow["f_OnDuty1Desc"] = "";
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strOvertime)
							{
								dataRow["f_OffDuty1Desc"] = "";
							}
							dataRow["f_OvertimeTime"] = 0;
							if (this.SetObjToStr(dataRow["f_OnDuty1"]) != "" && this.SetObjToStr(dataRow["f_OffDuty1"]) != "")
							{
								dataRow["f_OvertimeTime"] = Conversion.Int(DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_OnDuty1"], "yyyy-MM-dd HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_Offduty1"], "yyyy-MM-dd HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L) / 2.0m;
							}
							flag2 = (decimal)dataRow["f_OvertimeTime"] >= this.needDutyHour;
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strLateness)
						{
							if (flag3)
							{
								dataRow["f_LateTime"] = (long)Convert.ToInt32(dataRow["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty0, "13:30:00")), DateTime.Parse(Strings.Format(dataRow["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
							}
							else
							{
								dataRow["f_LateTime"] = (long)Convert.ToInt32(dataRow["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty0, "HH:mm:ss")), DateTime.Parse(Strings.Format(dataRow["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
							}
						}
						if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)
						{
							if (flag4)
							{
								dataRow["f_LeaveEarlyTime"] = (long)Convert.ToInt32(dataRow["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_OffDuty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty0, "12:00:00")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
							}
							else
							{
								dataRow["f_LeaveEarlyTime"] = (long)Convert.ToInt32(dataRow["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(dataRow["f_OffDuty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty0, "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
							}
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0 && this.SetObjToStr(dataRow["f_OffDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0)
						{
							dataRow["f_AbsenceDay"] = this.tLateAbsenceDay + this.tLeaveAbsenceDay;
							flag2 = false;
						}
						else
						{
							if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
								flag2 = false;
							}
							if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]) == CommonStr.strAbsence)
							{
								dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
								flag2 = false;
							}
						}
						if (this.SetObjToStr(dataRow["f_OffDuty1"]) == "")
						{
							dataRow["f_OvertimeTime"] = 0;
						}
						if (Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) >= 1m && num12 != 3)
						{
							dataRow["f_AbsenceDay"] = Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) / 2.0m;
						}
						text = " INSERT INTO t_d_AttendenceData ";
						using (SqlCommand sqlCommand = new SqlCommand(string.Concat(new object[]
						{
							text,
							" ([f_ConsumerID], [f_AttDate], [f_Onduty1],[f_Onduty1Desc], [f_Offduty1], [f_Offduty1Desc], [f_LateTime], [f_LeaveEarlyTime],[f_OvertimeTime], [f_AbsenceDay]   )  VALUES ( ",
							dataRow["f_ConsumerID"],
							" , ",
							this.PrepareStr(dataRow["f_AttDate"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							this.PrepareStr(dataRow["f_Onduty1"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Onduty1Desc"]),
							" , ",
							this.PrepareStr(dataRow["f_Offduty1"], true, "yyyy-MM-dd HH:mm:ss"),
							" , ",
							wgTools.PrepareStrNUnicode(dataRow["f_Offduty1Desc"]),
							" , ",
							dataRow["f_LateTime"],
							" , ",
							dataRow["f_LeaveEarlyTime"],
							" , ",
							this.getDecimalStr(dataRow["f_OvertimeTime"]),
							" , ",
							this.getDecimalStr(dataRow["f_AbsenceDay"]),
							" ) "
						}), this.cn))
						{
							if (this.cn.State == ConnectionState.Closed)
							{
								this.cn.Open();
							}
							sqlCommand.ExecuteNonQuery();
						}
						if (flag)
						{
							num3++;
						}
						string text5 = "";
						bool flag5 = true;
						bool flag6 = true;
						for (j = 0; j <= 1; j++)
						{
							switch (j)
							{
							case 0:
								text5 = this.SetObjToStr(dataRow["f_OnDuty1Desc"]);
								break;
							case 1:
								text5 = this.SetObjToStr(dataRow["f_OffDuty1Desc"]);
								break;
							}
							if (text5 == CommonStr.strLateness)
							{
								num5++;
								flag2 = false;
							}
							else if (text5 == CommonStr.strLeaveEarly)
							{
								num6++;
								flag2 = false;
							}
							else if (text5 == CommonStr.strNotReadCard)
							{
								num9++;
								flag2 = false;
							}
							else
							{
								if (text5 == CommonStr.strSignIn)
								{
									flag6 = false;
								}
								int i = 0;
								while (i <= this.dtHolidayType.Rows.Count - 1 && i < array.Length)
								{
									if (text5 == Convert.ToString(this.dtHolidayType.Rows[i]["f_HolidayType"]))
									{
										if ((int)this.dtHolidayType.Rows[i]["f_fullAttendance"] < 1)
										{
											flag2 = false;
										}
										else
										{
											flag5 = false;
										}
										array[i] += 0.5m;
										break;
									}
									i++;
								}
							}
						}
						if (this.SetObjToStr(dataRow["f_OnDuty1"]) == "" && this.SetObjToStr(dataRow["f_OffDuty1"]) == "" && flag5)
						{
							flag2 = false;
						}
						if (!flag6 && !this.bManualRecordAsFullAttendance)
						{
							flag2 = false;
						}
						num7 += Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture);
						num8 += Convert.ToDecimal(dataRow["f_OvertimeTime"], CultureInfo.InvariantCulture);
						num10 += Convert.ToInt32(dataRow["f_LateTime"]);
						num11 += Convert.ToInt32(dataRow["f_LeaveEarlyTime"]);
						if (Convert.ToInt32(dataRow["f_LateTime"]) != 0 || Convert.ToInt32(dataRow["f_LeaveEarlyTime"]) != 0 || Convert.ToDecimal(dataRow["f_AbsenceDay"], CultureInfo.InvariantCulture) != 0m)
						{
							flag2 = false;
						}
						if (this.bFullAttendanceSpecialA && (this.SetObjToStr(dataRow["f_OnDuty1"]) != "" || this.SetObjToStr(dataRow["f_OffDuty1"]) != "" || this.SetObjToStr(dataRow["f_OnDuty2"]) != "" || this.SetObjToStr(dataRow["f_OffDuty2"]) != ""))
						{
							flag2 = true;
						}
						if (flag2)
						{
							num4++;
						}
						dateTime3 = dateTime3.AddDays(1.0);
						Application.DoEvents();
					}
					string notes = this.getNotes((int)dataRow["f_ConsumerID"], dateTime, dateTime2);
					this.dvCardRecord.RowFilter = string.Format("f_Type ={0}", this.PrepareStr(CommonStr.strSignIn));
					text = " Insert Into t_d_AttStatistic ";
					text += " ( [f_ConsumerID], [f_AttDateStart], [f_AttDateEnd]  , [f_DayShouldWork],  [f_DayRealWork] , [f_TotalLate],  [f_TotalLeaveEarly],[f_TotalOvertime], [f_TotalAbsenceDay], [f_TotalNotReadCard]";
					for (int i = 1; i <= 32; i++)
					{
						object obj = text;
						text = string.Concat(new object[] { obj, " , [f_SpecialType", i, "]" });
					}
					text = string.Concat(new object[]
					{
						text,
						", f_LateMinutes, f_LeaveEarlyMinutes, f_ManualReadTimesCount, f_Notes )  Values( ",
						dataRow["f_ConsumerID"],
						" , ",
						this.PrepareStr(dateTime, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						this.PrepareStr(dateTime2, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						num3,
						" , ",
						num4,
						" , ",
						num5,
						" , ",
						num6,
						" , ",
						this.getDecimalStr(num8),
						" , ",
						this.getDecimalStr(num7),
						" , ",
						num9
					});
					for (int i = 0; i <= 31; i++)
					{
						text = text + " , " + wgTools.PrepareStrNUnicode(this.getDecimalStr(array[i]));
					}
					using (SqlCommand sqlCommand2 = new SqlCommand(string.Concat(new object[]
					{
						text,
						", ",
						num10,
						", ",
						num11,
						", ",
						this.dvCardRecord.Count,
						", ",
						wgTools.PrepareStrNUnicode(notes),
						" )"
					}), this.cn))
					{
						if (this.cn.State == ConnectionState.Closed)
						{
							this.cn.Open();
						}
						sqlCommand2.ExecuteNonQuery();
					}
				}
				sqlDataReader.Close();
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
				this.shiftAttReportImportFromAttendenceData();
				this.shiftAttStatisticImportFromAttStatistic();
				this.logCreateReport();
				if (this.CreateCompleteEvent != null)
				{
					this.CreateCompleteEvent(true, "");
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				try
				{
					if (this.CreateCompleteEvent != null)
					{
						this.CreateCompleteEvent(false, ex.ToString());
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x00233F1C File Offset: 0x00232F1C
		private string PrepareStr(object obj)
		{
			return wgTools.PrepareStr(obj);
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x00233F24 File Offset: 0x00232F24
		private string PrepareStr(object obj, bool bDate, string dateFormat)
		{
			return wgTools.PrepareStr(obj, bDate, dateFormat);
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x00233F2E File Offset: 0x00232F2E
		private string SetObjToStr(object obj)
		{
			return wgTools.SetObjToStr(obj);
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x00233F38 File Offset: 0x00232F38
		public int shiftAttReportImportFromAttendenceData()
		{
			int num = 0;
			string text = "SELECT * FROM t_d_AttendenceData  ORDER BY f_RecID ";
			this.dtAttendenceData = new DataTable("AttendenceData");
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dtAttendenceData);
					}
				}
			}
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			this.cmd = new SqlCommand(text, this.cn);
			try
			{
				if (this.dtAttendenceData.Rows.Count > 0)
				{
					for (int i = 0; i <= this.dtAttendenceData.Rows.Count - 1; i++)
					{
						if (this.DealingNumEvent != null)
						{
							this.DealingNumEvent(i);
						}
						int num2 = 0;
						DataRow dataRow = this.dtAttendenceData.Rows[i];
						if (this.SetObjToStr(dataRow["f_OnDuty1Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
						{
							num2++;
						}
						if (this.SetObjToStr(dataRow["f_OffDuty1Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
						{
							num2++;
						}
						if (this.SetObjToStr(dataRow["f_OnDuty2Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
						{
							num2++;
						}
						if (this.SetObjToStr(dataRow["f_OffDuty2Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
						{
							num2++;
						}
						text = " INSERT INTO t_d_Shift_AttReport ";
						text = string.Concat(new object[]
						{
							text,
							" ( f_ConsumerID, f_shiftDate, f_ShiftID, f_ReadTimes  , f_OnDuty1, f_OnDuty1AttDesc, f_OnDuty1CardRecordDesc  , f_OffDuty1, f_OffDuty1AttDesc, f_OffDuty1CardRecordDesc  , f_OnDuty2, f_OnDuty2AttDesc, f_OnDuty2CardRecordDesc  , f_OffDuty2, f_OffDuty2AttDesc, f_OffDuty2CardRecordDesc  , f_OnDuty3, f_OnDuty3AttDesc, f_OnDuty3CardRecordDesc  , f_OffDuty3, f_OffDuty3AttDesc, f_OffDuty3CardRecordDesc  , f_OnDuty4, f_OnDuty4AttDesc, f_OnDuty4CardRecordDesc  , f_OffDuty4, f_OffDuty4AttDesc, f_OffDuty4CardRecordDesc  , f_LateMinutes, f_LeaveEarlyMinutes, f_OvertimeHours, f_AbsenceDays  , f_NotReadCardCount, f_bOvertimeShift  )  Values ( ",
							dataRow["f_ConsumerID"],
							",",
							this.PrepareStr(dataRow["f_AttDate"], true, "yyyy-MM-dd"),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							this.tReadCardTimes,
							",",
							this.PrepareStr(dataRow["f_OnDuty1"], true, "yyyy-MM-dd HH:mm:ss"),
							",",
							wgTools.PrepareStrNUnicode(dataRow["f_OnDuty1Desc"]),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							this.PrepareStr(dataRow["f_OffDuty1"], true, "yyyy-MM-dd HH:mm:ss"),
							",",
							wgTools.PrepareStrNUnicode(dataRow["f_OffDuty1Desc"]),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							this.PrepareStr(dataRow["f_OnDuty2"], true, "yyyy-MM-dd HH:mm:ss"),
							",",
							wgTools.PrepareStrNUnicode(dataRow["f_OnDuty2Desc"]),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							this.PrepareStr(dataRow["f_OffDuty2"], true, "yyyy-MM-dd HH:mm:ss"),
							",",
							wgTools.PrepareStrNUnicode(dataRow["f_OffDuty2Desc"]),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							wgTools.PrepareStrNUnicode(""),
							",",
							dataRow["f_LateTime"],
							",",
							dataRow["f_LeaveEarlyTime"],
							",",
							this.getDecimalStr(dataRow["f_OvertimeTime"]),
							",",
							this.getDecimalStr(dataRow["f_AbsenceDay"]),
							",",
							num2,
							",",
							wgTools.PrepareStrNUnicode(""),
							") "
						});
						if (this.cn.State == ConnectionState.Closed)
						{
							this.cn.Open();
						}
						this.cmd.CommandText = text;
						if (this.cmd.ExecuteNonQuery() <= 0)
						{
							return num;
						}
					}
				}
				return num;
			}
			catch (Exception)
			{
			}
			finally
			{
				if (this.cn != null)
				{
					this.cn.Dispose();
				}
				if (this.cmd != null)
				{
					this.cmd.Dispose();
				}
			}
			return num;
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x00234598 File Offset: 0x00233598
		public int shiftAttStatisticImportFromAttStatistic()
		{
			int num = 0;
			string text = "SELECT * FROM t_d_AttStatistic  ORDER BY f_RecID ";
			int num2;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						using (DataTable dataTable = new DataTable("AttStatistic"))
						{
							sqlDataAdapter.Fill(dataTable);
							if (dataTable.Rows.Count > 0)
							{
								sqlCommand.Connection = sqlConnection;
								sqlCommand.CommandType = CommandType.Text;
								for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
								{
									if (this.DealingNumEvent != null)
									{
										this.DealingNumEvent(i);
									}
									DataRow dataRow = dataTable.Rows[i];
									text = " INSERT INTO t_d_Shift_AttStatistic ";
									text += " ( f_ConsumerID  , f_AttDateStart, f_AttDateEnd, f_DayShouldWork  , f_DayRealWork , f_LateMinutes,f_LateCount  , f_LeaveEarlyMinutes,f_LeaveEarlyCount  , f_OvertimeHours  , f_AbsenceDays  , f_NotReadCardCount, f_ManualReadTimesCount ";
									for (int j = 1; j <= 32; j++)
									{
										text = text + " , f_SpecialType" + j.ToString();
									}
									text = string.Concat(new object[]
									{
										text,
										" , f_Notes  ) Values ( ",
										dataRow["f_ConsumerID"],
										",",
										this.PrepareStr(dataRow["f_AttDateStart"], true, "yyyy-MM-dd HH:mm:ss"),
										",",
										this.PrepareStr(dataRow["f_AttDateEnd"], true, "yyyy-MM-dd HH:mm:ss"),
										",",
										dataRow["f_DayShouldWork"],
										",",
										dataRow["f_DayRealWork"],
										",",
										dataRow["f_LateMinutes"],
										",",
										dataRow["f_TotalLate"],
										",",
										dataRow["f_LeaveEarlyMinutes"],
										",",
										dataRow["f_TotalLeaveEarly"],
										",",
										this.getDecimalStr(dataRow["f_TotalOvertime"]),
										",",
										this.getDecimalStr(dataRow["f_TotalAbsenceDay"]),
										",",
										dataRow["f_TotalNotReadCard"],
										",",
										dataRow["f_ManualReadTimesCount"]
									});
									for (int k = 1; k <= 32; k++)
									{
										text = text + " ," + wgTools.PrepareStrNUnicode(dataRow["f_SpecialType" + k.ToString()]);
									}
									text = text + " , " + wgTools.PrepareStrNUnicode(dataRow["f_Notes"]) + ") ";
									if (sqlConnection.State == ConnectionState.Closed)
									{
										sqlConnection.Open();
									}
									sqlCommand.CommandText = text;
									if (sqlCommand.ExecuteNonQuery() <= 0)
									{
										return num;
									}
								}
							}
							num2 = num;
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x00234924 File Offset: 0x00233924
		public void startCreate()
		{
			this.mainThread = new Thread(new ThreadStart(this.make));
			this.mainThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			this.mainThread.Start();
		}

		// Token: 0x0400356B RID: 13675
		private const string DEFAULT_ALLOWOFFDUTYTIME = "23:59:59";

		// Token: 0x0400356C RID: 13676
		private const string DEFAULT_ALLOWONDUTYTIME = "00:00:00";

		// Token: 0x0400356D RID: 13677
		private const string DEFAULT_ALLOWONDUTYTIME2 = "00:00";

		// Token: 0x0400356E RID: 13678
		private const int REST_AM = 2;

		// Token: 0x0400356F RID: 13679
		private const int REST_ONE_DAY = 0;

		// Token: 0x04003570 RID: 13680
		private const int REST_PM = 1;

		// Token: 0x04003571 RID: 13681
		private const int WORK_AM = 1;

		// Token: 0x04003572 RID: 13682
		private const int WORK_ONE_DAY = 3;

		// Token: 0x04003573 RID: 13683
		private const int WORK_PM = 2;

		// Token: 0x04003574 RID: 13684
		private string[] arrNormalabc;

		// Token: 0x04003575 RID: 13685
		public bool bChooseOnlyOnDuty;

		// Token: 0x04003576 RID: 13686
		public bool bChooseTwoTimes;

		// Token: 0x04003577 RID: 13687
		public bool bEarliestAsOnDuty;

		// Token: 0x04003578 RID: 13688
		private bool bFirstGetNotes;

		// Token: 0x04003579 RID: 13689
		public bool bFullAttendanceSpecialA;

		// Token: 0x0400357A RID: 13690
		private bool bManualRecordAsFullAttendance;

		// Token: 0x0400357B RID: 13691
		public bool bStopCreate;

		// Token: 0x0400357C RID: 13692
		private SqlCommand cmd;

		// Token: 0x0400357D RID: 13693
		private SqlCommand cmdConsumer;

		// Token: 0x0400357E RID: 13694
		private SqlConnection cn;

		// Token: 0x0400357F RID: 13695
		private SqlConnection cnConsumer;

		// Token: 0x04003580 RID: 13696
		private SqlConnection cnNote;

		// Token: 0x04003581 RID: 13697
		private Container components;

		// Token: 0x04003582 RID: 13698
		public string consumerName;

		// Token: 0x04003583 RID: 13699
		private SqlDataAdapter daAttendenceData;

		// Token: 0x04003584 RID: 13700
		private SqlDataAdapter daCardRecord;

		// Token: 0x04003585 RID: 13701
		private SqlDataAdapter daHoliday;

		// Token: 0x04003586 RID: 13702
		private SqlDataAdapter daHolidayType;

		// Token: 0x04003587 RID: 13703
		private SqlDataAdapter daLeave;

		// Token: 0x04003588 RID: 13704
		private SqlDataAdapter daManualCardRecord;

		// Token: 0x04003589 RID: 13705
		private SqlDataAdapter daNoCardRecord;

		// Token: 0x0400358A RID: 13706
		private DataSet dsAtt;

		// Token: 0x0400358B RID: 13707
		private DataTable dtAttendenceData;

		// Token: 0x0400358C RID: 13708
		private DataTable dtCardRecord;

		// Token: 0x0400358D RID: 13709
		private DataTable dtCardRecord1;

		// Token: 0x0400358E RID: 13710
		private DataTable dtCardRecord2;

		// Token: 0x0400358F RID: 13711
		private DataTable dtHoliday;

		// Token: 0x04003590 RID: 13712
		private DataTable dtHolidayType;

		// Token: 0x04003591 RID: 13713
		private DataTable dtLeave;

		// Token: 0x04003592 RID: 13714
		private DataView dvCardRecord;

		// Token: 0x04003593 RID: 13715
		private DataView dvHoliday;

		// Token: 0x04003594 RID: 13716
		private DataView dvLeave;

		// Token: 0x04003595 RID: 13717
		private DataView dvLeave4Notes;

		// Token: 0x04003596 RID: 13718
		public DateTime endDateTime;

		// Token: 0x04003597 RID: 13719
		private int gProcVal;

		// Token: 0x04003598 RID: 13720
		public string groupName;

		// Token: 0x04003599 RID: 13721
		private Thread mainThread;

		// Token: 0x0400359A RID: 13722
		public decimal needDutyHour;

		// Token: 0x0400359B RID: 13723
		private int normalDay;

		// Token: 0x0400359C RID: 13724
		public DateTime startDateTime;

		// Token: 0x0400359D RID: 13725
		private string strAllowOffdutyTime;

		// Token: 0x0400359E RID: 13726
		public string strAllowOndutyTime;

		// Token: 0x0400359F RID: 13727
		public string strConsumerSql;

		// Token: 0x040035A0 RID: 13728
		private string strTemp;

		// Token: 0x040035A1 RID: 13729
		private decimal tLateAbsenceDay;

		// Token: 0x040035A2 RID: 13730
		private int tlateAbsenceTimeout;

		// Token: 0x040035A3 RID: 13731
		private int tLateTimeout;

		// Token: 0x040035A4 RID: 13732
		private decimal tLeaveAbsenceDay;

		// Token: 0x040035A5 RID: 13733
		private int tLeaveAbsenceTimeout;

		// Token: 0x040035A6 RID: 13734
		private int tLeaveTimeout;

		// Token: 0x040035A7 RID: 13735
		private DateTime tOffduty0;

		// Token: 0x040035A8 RID: 13736
		private DateTime tOffduty1;

		// Token: 0x040035A9 RID: 13737
		private DateTime tOffduty2;

		// Token: 0x040035AA RID: 13738
		private DateTime tOnduty0;

		// Token: 0x040035AB RID: 13739
		private DateTime tOnduty1;

		// Token: 0x040035AC RID: 13740
		private DateTime tOnduty2;

		// Token: 0x040035AD RID: 13741
		private int tOvertimeTimeout;

		// Token: 0x040035AE RID: 13742
		private int tReadCardTimes;

		// Token: 0x040035AF RID: 13743
		private int tTwoReadMintime;

		// Token: 0x040035B0 RID: 13744
		public int userId;

		// Token: 0x0200035E RID: 862
		// (Invoke) Token: 0x06001BBC RID: 7100
		public delegate void CreateCompleteEventHandler(bool bCreated, string strDesc);

		// Token: 0x0200035F RID: 863
		// (Invoke) Token: 0x06001BC0 RID: 7104
		public delegate void DealingNumEventHandler(int num);
	}
}
