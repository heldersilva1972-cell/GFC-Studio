using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualBasic;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000364 RID: 868
	public class comShift_Acc : Component
	{
		// Token: 0x06001C0F RID: 7183 RVA: 0x0024B6E8 File Offset: 0x0024A6E8
		public comShift_Acc()
		{
			this.strTemp = "";
			this.errInfo = "";
			this.tTwoReadMintime = 60;
			this.tAheadMinutesOnDutyFirst = 120;
			this.tAheadMinutes = 30;
			this.tDelayMinutes = 30;
			this.tOvertimeMinutes = 480;
			this.minShifDiffByMinute = 5;
			this.dsAtt = new DataSet();
			this.InitializeComponent();
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x0024B754 File Offset: 0x0024A754
		public comShift_Acc(IContainer Container)
			: this()
		{
			Container.Add(this);
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x0024B763 File Offset: 0x0024A763
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x0024B784 File Offset: 0x0024A784
		public string errDesc(int errno)
		{
			if (errno == -999)
			{
				return CommonStr.strSqlRunFail;
			}
			switch (errno)
			{
			case -7:
				return CommonStr.strTimeOverlapped;
			case -6:
				return CommonStr.strArrangedInvalidShiftID;
			case -5:
				return CommonStr.strShiftTimeOver24;
			case -4:
				return CommonStr.strErrTimeDiff;
			case -3:
				return CommonStr.strInvalidReadTimes;
			case -2:
				return CommonStr.strInvalidShiftID;
			case -1:
				return CommonStr.strFailed;
			default:
				return CommonStr.strUnknown;
			}
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x0024B7F8 File Offset: 0x0024A7F8
		public void getAttendenceParam()
		{
			this.tLateAbsenceDay = 0.5m;
			this.tLeaveAbsenceDay = 0.5m;
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			string text = "SELECT * FROM t_a_Shift_Attendence";
			try
			{
				if (this.cn.State != ConnectionState.Open)
				{
					this.cn.Open();
				}
				this.cmd = new OleDbCommand(text, this.cn);
				OleDbDataReader oleDbDataReader = this.cmd.ExecuteReader();
				while (oleDbDataReader.Read())
				{
					if ((int)oleDbDataReader["f_No"] == 1)
					{
						this.tLateTimeout = Convert.ToInt32(oleDbDataReader["f_Value"]);
					}
					else if ((int)oleDbDataReader["f_No"] == 4)
					{
						this.tLeaveTimeout = Convert.ToInt32(oleDbDataReader["f_Value"]);
					}
					else if ((int)oleDbDataReader["f_No"] == 7)
					{
						this.tOvertimeTimeout = Convert.ToInt32(oleDbDataReader["f_Value"]);
					}
					else if ((int)oleDbDataReader["f_No"] == 17)
					{
						this.tAheadMinutesOnDutyFirst = Convert.ToInt32(oleDbDataReader["f_Value"]);
					}
					else if ((int)oleDbDataReader["f_No"] == 18)
					{
						this.tAheadMinutes = Convert.ToInt32(oleDbDataReader["f_Value"]);
					}
					else if ((int)oleDbDataReader["f_No"] == 19)
					{
						this.tDelayMinutes = Convert.ToInt32(oleDbDataReader["f_Value"]);
					}
					else if ((int)oleDbDataReader["f_No"] == 20)
					{
						this.tOvertimeMinutes = Convert.ToInt32(oleDbDataReader["f_Value"]);
					}
				}
				oleDbDataReader.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			finally
			{
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
			}
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x0024BA34 File Offset: 0x0024AA34
		private void InitializeComponent()
		{
			this.components = new Container();
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x0024BA44 File Offset: 0x0024AA44
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

		// Token: 0x06001C16 RID: 7190 RVA: 0x0024BE80 File Offset: 0x0024AE80
		public static void localizedHolidayType(DataTable dt)
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
				if (dt.Columns.Contains("f_fullAttendance"))
				{
					string text = "";
					string text2 = "";
					string text3 = "";
					wgAppConfig.getSystemParamValue(174, out text, out text2, out text3);
					string text4 = text3;
					if (!string.IsNullOrEmpty(text4))
					{
						DataView dataView = new DataView(dt);
						dataView.RowFilter = string.Format("f_NO IN ({0})", text4);
						for (int i = 0; i < dataView.Count; i++)
						{
							dataView[i]["f_fullAttendance"] = 1;
						}
						dt.AcceptChanges();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x0024C114 File Offset: 0x0024B114
		public int logCreateReport(DateTime startDateTime, DateTime endDateTime, string groupName, string totalConsumer)
		{
			int num = -1;
			try
			{
				string text = string.Concat(new string[]
				{
					CommonStr.strCreateLog,
					"  [",
					CommonStr.strOperateDate,
					DateTime.Now.ToString(wgTools.DisplayFormat_DateYMDHMSWeek),
					"]"
				});
				string text2 = string.Concat(new string[]
				{
					text,
					";  ",
					CommonStr.strFrom,
					Strings.Format(startDateTime, wgTools.DisplayFormat_DateYMD),
					CommonStr.strTo,
					Strings.Format(endDateTime, wgTools.DisplayFormat_DateYMD)
				});
				string text3 = string.Concat(new string[]
				{
					text2,
					";   ",
					wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment),
					":",
					groupName,
					"            ",
					CommonStr.strUser,
					" (",
					totalConsumer,
					")"
				});
				string text4 = Strings.Format(startDateTime, "yyyy-MM-dd") + "--" + Strings.Format(endDateTime, "yyyy-MM-dd");
				wgAppConfig.runUpdateSql(string.Concat(new string[]
				{
					"UPDATE t_a_Attendence  SET [f_Value]=",
					this.PrepareStr(text4),
					" , [f_Notes] = ",
					this.PrepareStr(text3),
					" WHERE [f_NO]= 15 "
				}));
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x0024C2E4 File Offset: 0x0024B2E4
		private string PrepareStr(object obj)
		{
			return wgTools.PrepareStr(obj);
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x0024C2EC File Offset: 0x0024B2EC
		private string PrepareStr(object obj, bool bDate, string dateFormat)
		{
			return wgTools.PrepareStr(obj, bDate, dateFormat);
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x0024C2F6 File Offset: 0x0024B2F6
		private string SetObjToStr(object obj)
		{
			return wgTools.SetObjToStr(obj);
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x0024C300 File Offset: 0x0024B300
		public int shift_add(int id, string name, int readtimes, DateTime onduty1, DateTime offduty1, DateTime onduty2, DateTime offduty2, DateTime onduty3, DateTime offduty3, DateTime onduty4, DateTime offduty4, int bOvertimeShift)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			int num = -1;
			this.errInfo = "";
			try
			{
				num = this.shift_checkvalid(id, name, readtimes, onduty1, offduty1, onduty2, offduty2, onduty3, offduty3, onduty4, offduty4);
				if (num != 0)
				{
					return num;
				}
				string text = " SELECT count(*) FROM t_b_ShiftSet WHERE f_ShiftID = " + id;
				if (this.cn.State != ConnectionState.Open)
				{
					this.cn.Open();
				}
				this.cmd = new OleDbCommand(text, this.cn);
				int num2 = Convert.ToInt32(this.cmd.ExecuteScalar());
				this.cn.Close();
				if (num2 > 0)
				{
					num = -2;
					this.errInfo = id.ToString();
					return num;
				}
				text = " INSERT INTO t_b_ShiftSet ( f_ShiftID, f_ShiftName, f_ReadTimes,  f_OnDuty1, f_OffDuty1, f_OnDuty2, f_OffDuty2, f_OnDuty3, f_OffDuty3, f_OnDuty4, f_OffDuty4,f_bOvertimeShift, f_Notes)";
				text = string.Concat(new object[]
				{
					text,
					" VALUES ( ",
					id,
					" , ",
					this.PrepareStr(name),
					" , ",
					readtimes,
					" , ",
					this.PrepareStr(this.realOnduty1, true, "yyyy-MM-dd HH:mm:ss"),
					" , ",
					this.PrepareStr(this.realOffduty1, true, "yyyy-MM-dd HH:mm:ss")
				});
				if (readtimes > 2)
				{
					text = string.Concat(new string[]
					{
						text,
						" , ",
						this.PrepareStr(this.realOnduty2, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						this.PrepareStr(this.realOffduty2, true, "yyyy-MM-dd HH:mm:ss")
					});
				}
				else
				{
					text = string.Concat(new string[]
					{
						text,
						" , ",
						this.PrepareStr(""),
						" , ",
						this.PrepareStr("")
					});
				}
				if (readtimes > 4)
				{
					text = string.Concat(new string[]
					{
						text,
						" , ",
						this.PrepareStr(this.realOnduty3, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						this.PrepareStr(this.realOffduty3, true, "yyyy-MM-dd HH:mm:ss")
					});
				}
				else
				{
					text = string.Concat(new string[]
					{
						text,
						" , ",
						this.PrepareStr(""),
						" , ",
						this.PrepareStr("")
					});
				}
				if (readtimes > 6)
				{
					text = string.Concat(new string[]
					{
						text,
						" , ",
						this.PrepareStr(this.realOnduty4, true, "yyyy-MM-dd HH:mm:ss"),
						" , ",
						this.PrepareStr(this.realOffduty4, true, "yyyy-MM-dd HH:mm:ss")
					});
				}
				else
				{
					text = string.Concat(new string[]
					{
						text,
						" , ",
						this.PrepareStr(""),
						" , ",
						this.PrepareStr("")
					});
				}
				text = string.Concat(new object[]
				{
					text,
					" , ",
					bOvertimeShift,
					" , ",
					this.PrepareStr(""),
					" )"
				});
				if (this.cn.State != ConnectionState.Open)
				{
					this.cn.Open();
				}
				this.cmd = new OleDbCommand(text, this.cn);
				num2 = this.cmd.ExecuteNonQuery();
				this.cn.Close();
				if (num2 > 0)
				{
					return 0;
				}
				return -1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			finally
			{
				if (this.cn.State == ConnectionState.Open)
				{
					this.cn.Close();
				}
			}
			return num;
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x0024C794 File Offset: 0x0024B794
		public int shift_arrange_delete(int consumerId, DateTime dateStart, DateTime dateEnd)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			bool flag = false;
			object[] array = new object[37];
			this.errInfo = "";
			int num = -1;
			string text2;
			if (consumerId != 0)
			{
				DateTime dateTime = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
				DateTime dateTime2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
				if (dateTime <= dateTime2)
				{
					try
					{
						string text = "";
						DateTime dateTime3 = dateTime;
						for (;;)
						{
							if (text != Strings.Format(dateTime3, "yyyy-MM"))
							{
								text = Strings.Format(dateTime3, "yyyy-MM");
								text2 = " SELECT * FROM t_d_ShiftData ";
								text2 = string.Concat(new object[]
								{
									text2,
									" WHERE f_ConsumerID = ",
									consumerId,
									" AND f_DateYM = ",
									this.PrepareStr(text)
								});
								if (this.cn.State != ConnectionState.Open)
								{
									this.cn.Open();
								}
								this.cmd = new OleDbCommand(text2, this.cn);
								OleDbDataReader oleDbDataReader = this.cmd.ExecuteReader();
								if (oleDbDataReader.Read())
								{
									flag = true;
									for (int i = 0; i <= oleDbDataReader.FieldCount - 1; i++)
									{
										array[i] = oleDbDataReader[i];
									}
								}
								else
								{
									flag = false;
								}
								oleDbDataReader.Close();
							}
							do
							{
								array[2 + dateTime3.Day] = -1;
								dateTime3 = dateTime3.AddDays(1.0);
							}
							while (!(text != Strings.Format(dateTime3, "yyyy-MM")) && dateTime3 <= dateTime2);
							if (flag)
							{
								bool flag2 = true;
								for (int j = 1; j <= 31; j++)
								{
									if (Convert.ToInt32(array[2 + j]) > -1)
									{
										flag2 = false;
										break;
									}
								}
								if (flag2)
								{
									text2 = "  DELETE FROM t_d_ShiftData ";
									text2 = text2 + " WHERE f_RecID = " + array[0];
								}
								else
								{
									text2 = "  UPDATE t_d_ShiftData SET ";
									int j = 1;
									object obj = text2;
									text2 = string.Concat(new object[]
									{
										obj,
										" f_ShiftID_",
										j.ToString().PadLeft(2, '0'),
										" = ",
										array[2 + j]
									});
									for (j = 2; j <= 31; j++)
									{
										object obj2 = text2;
										text2 = string.Concat(new object[]
										{
											obj2,
											" , f_ShiftID_",
											j.ToString().PadLeft(2, '0'),
											" = ",
											array[2 + j]
										});
									}
									text2 = string.Concat(new object[]
									{
										text2,
										" , f_LogDate  = ",
										this.PrepareStr(DateTime.Now, true, "yyyy-MM-dd HH:mm:ss"),
										" , f_Notes  = ",
										this.PrepareStr(""),
										" WHERE f_RecID = ",
										array[0]
									});
								}
								if (this.cn.State != ConnectionState.Open)
								{
									this.cn.Open();
								}
								this.cmd = new OleDbCommand(text2, this.cn);
								if (this.cmd.ExecuteNonQuery() <= 0)
								{
									break;
								}
							}
							if (!(dateTime3 <= dateTime2))
							{
								goto Block_18;
							}
						}
						num = -999;
						this.errInfo = text2;
						return num;
						Block_18:
						num = 0;
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
					}
					finally
					{
						if (this.cn.State == ConnectionState.Open)
						{
							this.cn.Close();
						}
					}
					return num;
				}
				return num;
			}
			text2 = " DELETE FROM t_d_ShiftData ";
			if (this.cn.State != ConnectionState.Open)
			{
				this.cn.Open();
			}
			this.cmd = new OleDbCommand(text2, this.cn);
			if (this.cmd.ExecuteNonQuery() < 0)
			{
				num = -999;
				this.errInfo = text2;
				return num;
			}
			return 0;
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x0024CBBC File Offset: 0x0024BBBC
		public int shift_arrange_update(int consumerId, DateTime dateShift, int shiftID)
		{
			int num = 0;
			int[] array = new int[1];
			try
			{
				array[0] = shiftID;
				num = this.shift_arrangeByRule(consumerId, dateShift, dateShift, 1, array);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x0024CC14 File Offset: 0x0024BC14
		public int shift_arrangeByRule(int consumerId, DateTime dateStart, DateTime dateEnd, int ruleLen, int[] shiftRule)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			bool flag = false;
			object[] array = new object[37];
			this.errInfo = "";
			int num = -1;
			DateTime dateTime = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
			DateTime dateTime2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
			if (dateTime <= dateTime2)
			{
				try
				{
					string text = "";
					int num2 = 0;
					DateTime dateTime3 = dateTime;
					string text2;
					for (;;)
					{
						if (text != Strings.Format(dateTime3, "yyyy-MM"))
						{
							text = Strings.Format(dateTime3, "yyyy-MM");
							text2 = " SELECT * FROM t_d_ShiftData ";
							text2 = string.Concat(new object[]
							{
								text2,
								" WHERE f_ConsumerID = ",
								consumerId,
								" AND f_DateYM = ",
								this.PrepareStr(text)
							});
							if (this.cn.State != ConnectionState.Open)
							{
								this.cn.Open();
							}
							this.cmd = new OleDbCommand(text2, this.cn);
							OleDbDataReader oleDbDataReader = this.cmd.ExecuteReader();
							if (oleDbDataReader.Read())
							{
								flag = true;
								for (int i = 0; i <= oleDbDataReader.FieldCount - 1; i++)
								{
									array[i] = oleDbDataReader[i];
								}
							}
							else
							{
								int num3 = DateTime.DaysInMonth(dateTime3.Year, dateTime3.Month);
								flag = false;
								array[1] = consumerId;
								array[2] = this.PrepareStr(text);
								for (int j = 1; j <= 31; j++)
								{
									if (j <= num3)
									{
										array[j + 2] = -1;
									}
									else
									{
										array[j + 2] = -2;
									}
								}
							}
							oleDbDataReader.Close();
						}
						do
						{
							array[2 + dateTime3.Day] = shiftRule[num2];
							num2++;
							if (num2 >= ruleLen)
							{
								num2 = 0;
							}
							dateTime3 = dateTime3.AddDays(1.0);
						}
						while (!(text != Strings.Format(dateTime3, "yyyy-MM")) && dateTime3 <= dateTime2);
						if (flag)
						{
							text2 = "  UPDATE t_d_ShiftData SET ";
							int k = 1;
							object obj = text2;
							text2 = string.Concat(new object[]
							{
								obj,
								" f_ShiftID_",
								k.ToString().PadLeft(2, '0'),
								" = ",
								array[2 + k]
							});
							for (k = 2; k <= 31; k++)
							{
								object obj2 = text2;
								text2 = string.Concat(new object[]
								{
									obj2,
									" , f_ShiftID_",
									k.ToString().PadLeft(2, '0'),
									" = ",
									array[2 + k]
								});
							}
							text2 = string.Concat(new object[]
							{
								text2,
								" , f_LogDate  = ",
								this.PrepareStr(DateTime.Now, true, "yyyy-MM-dd HH:mm:ss"),
								" , f_Notes  = ",
								this.PrepareStr(""),
								" WHERE f_RecID = ",
								array[0]
							});
						}
						else
						{
							text2 = "  INSERT INTO t_d_ShiftData  ";
							text2 += " ( f_ConsumerID , f_DateYM  ";
							for (int l = 1; l <= 31; l++)
							{
								text2 = text2 + " , f_ShiftID_" + l.ToString().PadLeft(2, '0');
							}
							text2 = string.Concat(new object[]
							{
								text2,
								" , f_Notes    )  Values ( ",
								array[1],
								" , ",
								array[2]
							});
							for (int l = 1; l <= 31; l++)
							{
								text2 = text2 + " , " + array[2 + l];
							}
							text2 = text2 + "  , " + this.PrepareStr("") + " ) ";
						}
						if (this.cn.State != ConnectionState.Open)
						{
							this.cn.Open();
						}
						this.cmd = new OleDbCommand(text2, this.cn);
						if (this.cmd.ExecuteNonQuery() <= 0)
						{
							break;
						}
						if (!(dateTime3 <= dateTime2))
						{
							goto Block_17;
						}
					}
					num = -999;
					this.errInfo = text2;
					return num;
					Block_17:
					num = 0;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				}
				finally
				{
					if (this.cn.State == ConnectionState.Open)
					{
						this.cn.Close();
					}
				}
				return num;
			}
			return num;
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x0024D0E0 File Offset: 0x0024C0E0
		public int shift_AttReport_cleardb()
		{
			this.errInfo = "";
			int num = -1;
			try
			{
				if (wgAppConfig.runUpdateSql("Delete From t_d_shift_AttReport") >= 0)
				{
					num = 0;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x0024D13C File Offset: 0x0024C13C
		public int shift_AttReport_Create(out DataTable dtAttReport)
		{
			this.dtReport = new DataTable("t_d_AttReport");
			int num = -1;
			dtAttReport = null;
			try
			{
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_ConsumerID";
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_ShiftDate";
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_ShiftID";
				this.dc.DefaultValue = -1;
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_Readtimes";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				for (int i = 1; i <= 4; i++)
				{
					this.dc = new DataColumn();
					this.dc.DataType = Type.GetType("System.DateTime");
					this.dc.ColumnName = "f_OnDuty" + i;
					this.dtReport.Columns.Add(this.dc);
					this.dc = new DataColumn();
					this.dc.DataType = Type.GetType("System.String");
					this.dc.ColumnName = "f_OnDuty" + i + "AttDesc";
					this.dtReport.Columns.Add(this.dc);
					this.dc = new DataColumn();
					this.dc.DataType = Type.GetType("System.String");
					this.dc.ColumnName = "f_OnDuty" + i + "CardRecordDesc";
					this.dtReport.Columns.Add(this.dc);
					this.dc = new DataColumn();
					this.dc.DataType = Type.GetType("System.DateTime");
					this.dc.ColumnName = "f_OffDuty" + i;
					this.dtReport.Columns.Add(this.dc);
					this.dc = new DataColumn();
					this.dc.DataType = Type.GetType("System.String");
					this.dc.ColumnName = "f_OffDuty" + i + "AttDesc";
					this.dtReport.Columns.Add(this.dc);
					this.dc = new DataColumn();
					this.dc.DataType = Type.GetType("System.String");
					this.dc.ColumnName = "f_OffDuty" + i + "CardRecordDesc";
					this.dtReport.Columns.Add(this.dc);
				}
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_LateMinutes";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_LeaveEarlyMinutes";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Decimal");
				this.dc.ColumnName = "f_OvertimeHours";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Decimal");
				this.dc.ColumnName = "f_AbsenceDays";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_NotReadCardCount";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_bOvertimeShift";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				dtAttReport = this.dtReport.Copy();
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x0024D6FC File Offset: 0x0024C6FC
		public int shift_AttReport_Fill(DataTable dtAttReport, DataTable dtShiftWorkSchedule)
		{
			this.errInfo = "";
			int num = -1;
			try
			{
				DataRow dataRow = null;
				for (int i = 0; i <= dtShiftWorkSchedule.Rows.Count - 1; i++)
				{
					if (this.bStopCreate)
					{
						return num;
					}
					if (dataRow == null)
					{
						dataRow = dtAttReport.NewRow();
					}
					DataRow dataRow2 = dtShiftWorkSchedule.Rows[i];
					int num2 = Convert.ToInt32(dataRow2["f_Readtimes"]);
					if (Convert.ToInt32(dataRow2["f_ShiftID"]) <= 0)
					{
						dataRow["f_ConsumerID"] = dataRow2["f_ConsumerID"];
						dataRow["f_ShiftDate"] = dataRow2["f_ShiftDate"];
						dataRow["f_ShiftID"] = dataRow2["f_ShiftID"];
						dataRow["f_Readtimes"] = dataRow2["f_Readtimes"];
						for (int j = 1; j <= 4; j++)
						{
							dataRow["f_OnDuty" + j + "AttDesc"] = dataRow2["f_AttDesc"];
							dataRow["f_OffDuty" + j + "AttDesc"] = dataRow2["f_AttDesc"];
						}
						dtAttReport.Rows.Add(dataRow);
						dataRow = dtAttReport.NewRow();
					}
					else
					{
						int num3 = Convert.ToInt32(dataRow2["f_Timeseg"]);
						if (num3 == 0)
						{
							dataRow["f_LateMinutes"] = 0;
							dataRow["f_LeaveEarlyMinutes"] = 0;
							dataRow["f_OvertimeHours"] = 0;
							dataRow["f_AbsenceDays"] = 0;
							dataRow["f_NotReadCardCount"] = 0;
							dataRow["f_ConsumerID"] = dataRow2["f_ConsumerID"];
							dataRow["f_ShiftDate"] = dataRow2["f_ShiftDate"];
							dataRow["f_ShiftID"] = dataRow2["f_ShiftID"];
							dataRow["f_Readtimes"] = dataRow2["f_Readtimes"];
							dataRow["f_bOvertimeShift"] = dataRow2["f_bOvertimeShift"];
						}
						if ((num3 & 1) == 0)
						{
							dataRow["f_OnDuty" + (Conversion.Int(num3 / 2) + 1) + "AttDesc"] = this.SetObjToStr(dataRow2["f_AttDesc"]);
							dataRow["f_OnDuty" + (Conversion.Int(num3 / 2) + 1) + "CardRecordDesc"] = this.SetObjToStr(dataRow2["f_CardRecordDesc"]);
							dataRow["f_OnDuty" + (Conversion.Int(num3 / 2) + 1)] = dataRow2["f_WorkTime"];
						}
						else
						{
							dataRow["f_OffDuty" + (Conversion.Int(num3 / 2) + 1) + "AttDesc"] = this.SetObjToStr(dataRow2["f_AttDesc"]);
							dataRow["f_OffDuty" + (Conversion.Int(num3 / 2) + 1) + "CardRecordDesc"] = this.SetObjToStr(dataRow2["f_CardRecordDesc"]);
							dataRow["f_OffDuty" + (Conversion.Int(num3 / 2) + 1)] = dataRow2["f_WorkTime"];
						}
						if (this.SetObjToStr(dataRow2["f_AttDesc"]) == CommonStr.strLateness)
						{
							dataRow["f_LateMinutes"] = Convert.ToDecimal(dataRow["f_LateMinutes"], CultureInfo.InvariantCulture) + Convert.ToDecimal(dataRow2["f_Duration"], CultureInfo.InvariantCulture);
						}
						else if (this.SetObjToStr(dataRow2["f_AttDesc"]) == CommonStr.strLeaveEarly)
						{
							dataRow["f_LeaveEarlyMinutes"] = Convert.ToDecimal(dataRow["f_LeaveEarlyMinutes"], CultureInfo.InvariantCulture) + Convert.ToDecimal(dataRow2["f_Duration"], CultureInfo.InvariantCulture);
						}
						else if (this.SetObjToStr(dataRow2["f_AttDesc"]) == CommonStr.strAbsence)
						{
							if ((num3 & 1) == 0)
							{
								dataRow["f_AbsenceDays"] = Convert.ToDecimal(dataRow["f_AbsenceDays"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
							}
							else
							{
								dataRow["f_AbsenceDays"] = Convert.ToDecimal(dataRow["f_AbsenceDays"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
							}
						}
						else if (this.SetObjToStr(dataRow2["f_AttDesc"]) == CommonStr.strOvertime)
						{
							if (Convert.ToInt32(dataRow["f_bOvertimeShift"]) > 0)
							{
								if (Convert.ToInt32(dataRow["f_bOvertimeShift"]) == 1)
								{
									if ((num3 & 1) != 0 && !(this.SetObjToStr(dataRow["f_OnDuty" + (Conversion.Int(num3 / 2) + 1)]) == "") && !(this.SetObjToStr(dataRow["f_OffDuty" + (Conversion.Int(num3 / 2) + 1)]) == ""))
									{
										int num4 = (int)(Convert.ToDateTime(dataRow["f_OffDuty" + (Conversion.Int(num3 / 2) + 1)]).Subtract(Convert.ToDateTime(dataRow["f_OnDuty" + (Conversion.Int(num3 / 2) + 1)])).TotalMinutes / 30.0);
										dataRow["f_OvertimeHours"] = Convert.ToDecimal(dataRow["f_OvertimeHours"], CultureInfo.InvariantCulture) + num4 / 2.0m;
									}
								}
								else if (Convert.ToInt32(dataRow["f_bOvertimeShift"]) == 2 && (num3 & 1) != 0 && this.SetObjToStr(dataRow["f_OnDuty" + (Conversion.Int(num3 / 2) + 1)]) != "" && this.SetObjToStr(dataRow["f_OffDuty" + (Conversion.Int(num3 / 2) + 1)]) != "")
								{
									object obj = dtShiftWorkSchedule.Rows[i - 1]["f_PlanTime"];
									int num5;
									if (Convert.ToDateTime(obj).Subtract(Convert.ToDateTime(dataRow["f_OnDuty" + (Conversion.Int(num3 / 2) + 1)])).TotalMinutes < 0.0)
									{
										num5 = (int)(Convert.ToDateTime(dataRow["f_OffDuty" + (Conversion.Int(num3 / 2) + 1)]).Subtract(Convert.ToDateTime(dataRow["f_OnDuty" + (Conversion.Int(num3 / 2) + 1)])).TotalMinutes / 30.0);
									}
									else
									{
										num5 = (int)(Convert.ToDateTime(dataRow["f_OffDuty" + (Conversion.Int(num3 / 2) + 1)]).Subtract(Convert.ToDateTime(obj)).TotalMinutes / 30.0);
									}
									dataRow["f_OvertimeHours"] = Convert.ToDecimal(dataRow["f_OvertimeHours"], CultureInfo.InvariantCulture) + num5 / 2.0m;
								}
							}
							else
							{
								dataRow["f_OvertimeHours"] = Convert.ToDecimal(dataRow["f_OvertimeHours"], CultureInfo.InvariantCulture) + Conversion.Int(Convert.ToInt32(dataRow2["f_Duration"]) / 30) / 2.0m;
							}
						}
						else if (this.SetObjToStr(dataRow2["f_AttDesc"]) == CommonStr.strNotReadCard)
						{
							dataRow["f_NotReadCardCount"] = Convert.ToInt32(dataRow["f_NotReadCardCount"]) + 1;
						}
						if (num3 + 1 == num2)
						{
							if (Convert.ToDecimal(dataRow["f_AbsenceDays"], CultureInfo.InvariantCulture) > 1m)
							{
								dataRow["f_AbsenceDays"] = 1;
							}
							dtAttReport.Rows.Add(dataRow);
							dataRow = dtAttReport.NewRow();
						}
					}
				}
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x0024E040 File Offset: 0x0024D040
		public int shift_AttReport_writetodb(DataTable dtAttReport)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			string text = "";
			this.cmd = new OleDbCommand();
			bool flag = true;
			this.errInfo = "";
			int num = -1;
			try
			{
				if (dtAttReport.Rows.Count > 0)
				{
					this.cmd.Connection = this.cn;
					this.cmd.CommandType = CommandType.Text;
					for (int i = 0; i <= dtAttReport.Rows.Count - 1; i++)
					{
						if (this.bStopCreate)
						{
							return num;
						}
						DataRow dataRow = dtAttReport.Rows[i];
						text = " INSERT INTO t_d_Shift_AttReport ";
						text += " ( f_ConsumerID, f_shiftDate, f_ShiftID, f_ReadTimes ";
						text += " , f_OnDuty1, f_OnDuty1AttDesc, f_OnDuty1CardRecordDesc ";
						text += " , f_OffDuty1, f_OffDuty1AttDesc, f_OffDuty1CardRecordDesc ";
						text += " , f_OnDuty2, f_OnDuty2AttDesc, f_OnDuty2CardRecordDesc ";
						text += " , f_OffDuty2, f_OffDuty2AttDesc, f_OffDuty2CardRecordDesc ";
						text += " , f_OnDuty3, f_OnDuty3AttDesc, f_OnDuty3CardRecordDesc ";
						text += " , f_OffDuty3, f_OffDuty3AttDesc, f_OffDuty3CardRecordDesc ";
						text += " , f_OnDuty4, f_OnDuty4AttDesc, f_OnDuty4CardRecordDesc ";
						text += " , f_OffDuty4, f_OffDuty4AttDesc, f_OffDuty4CardRecordDesc ";
						text += " , f_LateMinutes, f_LeaveEarlyMinutes, f_OvertimeHours, f_AbsenceDays ";
						text += " , f_NotReadCardCount, f_bOvertimeShift ";
						text += " ) ";
						text = text + " Values ( " + dataRow["f_ConsumerID"];
						text = text + "," + this.PrepareStr(dataRow["f_shiftDate"], true, "yyyy-MM-dd");
						text = text + "," + dataRow["f_ShiftID"];
						text = text + "," + dataRow["f_ReadTimes"];
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty1"], true, "yyyy-MM-dd HH:mm:ss");
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty1AttDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty1CardRecordDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty1"], true, "yyyy-MM-dd HH:mm:ss");
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty1AttDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty1CardRecordDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty2"], true, "yyyy-MM-dd HH:mm:ss");
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty2AttDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty2CardRecordDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty2"], true, "yyyy-MM-dd HH:mm:ss");
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty2AttDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty2CardRecordDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty3"], true, "yyyy-MM-dd HH:mm:ss");
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty3AttDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty3CardRecordDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty3"], true, "yyyy-MM-dd HH:mm:ss");
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty3AttDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty3CardRecordDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty4"], true, "yyyy-MM-dd HH:mm:ss");
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty4AttDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OnDuty4CardRecordDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty4"], true, "yyyy-MM-dd HH:mm:ss");
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty4AttDesc"]);
						text = text + "," + this.PrepareStr(dataRow["f_OffDuty4CardRecordDesc"]);
						text = text + "," + dataRow["f_LateMinutes"];
						text = text + "," + dataRow["f_LeaveEarlyMinutes"];
						text = text + "," + dataRow["f_OvertimeHours"];
						text = text + "," + dataRow["f_AbsenceDays"];
						text = text + "," + dataRow["f_NotReadCardCount"];
						text = text + "," + dataRow["f_bOvertimeShift"];
						text += ") ";
						if (this.cn.State == ConnectionState.Closed)
						{
							this.cn.Open();
						}
						this.cmd.CommandText = text;
						if (this.cmd.ExecuteNonQuery() <= 0)
						{
							this.errInfo = text;
							flag = false;
							break;
						}
					}
				}
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
				if (flag)
				{
					num = 0;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString() + "\r\n" + text, new object[] { EventLogEntryType.Error });
			}
			finally
			{
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
			}
			return num;
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x0024E664 File Offset: 0x0024D664
		public int shift_AttStatistic_cleardb()
		{
			this.errInfo = "";
			int num = -1;
			try
			{
				if (wgAppConfig.runUpdateSql("Delete From t_d_shift_AttStatistic") >= 0)
				{
					num = 0;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x0024E6C0 File Offset: 0x0024D6C0
		public int shift_AttStatistic_Create(out DataTable dtAttStatistic)
		{
			this.dtReport1 = new DataTable("t_d_AttStatistic");
			int num = -1;
			dtAttStatistic = null;
			try
			{
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_ConsumerID";
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_AttDateStart";
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_AttDateEnd";
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_DayShouldWork";
				this.dc.DefaultValue = 0;
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_DayRealWork";
				this.dc.DefaultValue = 0;
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_LateMinutes";
				this.dc.DefaultValue = 0;
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_LateCount";
				this.dc.DefaultValue = 0;
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_LeaveEarlyMinutes";
				this.dc.DefaultValue = 0;
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_LeaveEarlyCount";
				this.dc.DefaultValue = 0;
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Decimal");
				this.dc.ColumnName = "f_OvertimeHours";
				this.dc.DefaultValue = 0;
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Decimal");
				this.dc.ColumnName = "f_AbsenceDays";
				this.dc.DefaultValue = 0;
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_NotReadCardCount";
				this.dc.DefaultValue = 0;
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_ManualReadTimesCount";
				this.dc.DefaultValue = 0;
				this.dtReport1.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.String");
				this.dc.ColumnName = "f_Notes";
				this.dc.DefaultValue = "";
				this.dtReport1.Columns.Add(this.dc);
				for (int i = 1; i <= 32; i++)
				{
					this.dc = new DataColumn();
					this.dc.DataType = Type.GetType("System.String");
					this.dc.ColumnName = "f_SpecialType" + i;
					this.dc.DefaultValue = "";
					this.dtReport1.Columns.Add(this.dc);
				}
				dtAttStatistic = this.dtReport1.Copy();
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x0024EC2C File Offset: 0x0024DC2C
		public int shift_AttStatistic_Fill(DataTable dtAttStatistic, DataTable dtAttReport)
		{
			this.errInfo = "";
			int num = -1;
			try
			{
				DataRow dataRow = null;
				this.cn = new OleDbConnection(wgAppConfig.dbConString);
				this.dsAtt = new DataSet();
				this.daHolidayType = new OleDbDataAdapter("SELECT *,0 as f_fullAttendance FROM t_a_HolidayType", this.cn);
				this.daHolidayType.Fill(this.dsAtt, "HolidayType");
				this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
				comShift_Acc.localizedHolidayType(this.dtHolidayType);
				for (int i = 0; i <= dtAttReport.Rows.Count - 1; i++)
				{
					bool flag = false;
					if (this.bStopCreate)
					{
						return num;
					}
					if (dataRow == null)
					{
						dataRow = dtAttStatistic.NewRow();
					}
					DataRow dataRow2 = dtAttReport.Rows[i];
					int num2 = Convert.ToInt32(dataRow2["f_Readtimes"]);
					if (i == 0)
					{
						dataRow["f_ConsumerID"] = dataRow2["f_ConsumerID"];
						dataRow["f_AttDateStart"] = dataRow2["f_ShiftDate"];
					}
					Convert.ToDateTime(dataRow2["f_ShiftDate"]);
					dataRow["f_LateMinutes"] = Convert.ToInt32(dataRow["f_LateMinutes"], CultureInfo.InvariantCulture) + Convert.ToInt32(dataRow2["f_LateMinutes"], CultureInfo.InvariantCulture);
					dataRow["f_LeaveEarlyMinutes"] = Convert.ToInt32(dataRow["f_LeaveEarlyMinutes"], CultureInfo.InvariantCulture) + Convert.ToInt32(dataRow2["f_LeaveEarlyMinutes"], CultureInfo.InvariantCulture);
					dataRow["f_AbsenceDays"] = Convert.ToDecimal(dataRow["f_AbsenceDays"], CultureInfo.InvariantCulture) + Convert.ToDecimal(dataRow2["f_AbsenceDays"], CultureInfo.InvariantCulture);
					dataRow["f_OvertimeHours"] = Convert.ToDecimal(dataRow["f_OvertimeHours"], CultureInfo.InvariantCulture) + Convert.ToDecimal(dataRow2["f_OvertimeHours"], CultureInfo.InvariantCulture);
					dataRow["f_NotReadCardCount"] = Convert.ToInt32(dataRow["f_NotReadCardCount"]) + Convert.ToInt32(dataRow2["f_NotReadCardCount"]);
					if (Convert.ToInt32(dataRow2["f_ShiftID"]) > 0)
					{
						dataRow["f_DayShouldWork"] = Convert.ToInt32(dataRow["f_DayShouldWork"]) + 1;
						if (this.SetObjToStr(dataRow2["f_OnDuty1"]) != "" && this.SetObjToStr(dataRow2["f_OffDuty1"]) != "" && Convert.ToInt32(dataRow2["f_LateMinutes"], CultureInfo.InvariantCulture) == 0 && Convert.ToInt32(dataRow2["f_LeaveEarlyMinutes"], CultureInfo.InvariantCulture) == 0 && Convert.ToInt32(dataRow2["f_NotReadCardCount"]) == 0 && Convert.ToDecimal(dataRow2["f_AbsenceDays"], CultureInfo.InvariantCulture) == 0m)
						{
							flag = true;
						}
					}
					if (CommonStr.strOvertime == this.SetObjToStr(dataRow2["f_OnDuty" + (Conversion.Int(0) + 1) + "AttDesc"]))
					{
						for (int j = 1; j <= num2; j += 2)
						{
							string text = this.SetObjToStr(dataRow2["f_OnDuty" + (Conversion.Int(j / 2) + 1)]);
							if ("" == text)
							{
								flag = false;
								break;
							}
							text = this.SetObjToStr(dataRow2["f_OffDuty" + (Conversion.Int(j / 2) + 1)]);
							if ("" == text)
							{
								flag = false;
								break;
							}
						}
					}
					if (num2 == 0)
					{
						flag = false;
					}
					for (int j = 1; j <= num2; j += 2)
					{
						if (this.bStopCreate)
						{
							return num;
						}
						string text = this.SetObjToStr(dataRow2["f_OnDuty" + (Conversion.Int(j / 2) + 1) + "CardRecordDesc"]);
						if (text != "")
						{
							if (text == CommonStr.strSignIn)
							{
								dataRow["f_ManualReadTimesCount"] = Convert.ToInt32(dataRow["f_ManualReadTimesCount"]) + 1;
							}
							else if (CommonStr.strOvertime != text)
							{
								flag = false;
							}
						}
						text = this.SetObjToStr(dataRow2["f_OffDuty" + (Conversion.Int(j / 2) + 1) + "CardRecordDesc"]);
						if (text != "")
						{
							if (text == CommonStr.strSignIn)
							{
								dataRow["f_ManualReadTimesCount"] = Convert.ToInt32(dataRow["f_ManualReadTimesCount"]) + 1;
							}
							else if (CommonStr.strOvertime != text)
							{
								flag = false;
							}
						}
						text = this.SetObjToStr(dataRow2["f_OnDuty" + (Conversion.Int(j / 2) + 1) + "AttDesc"]);
						if (text != "")
						{
							if (text == CommonStr.strLateness)
							{
								dataRow["f_LateCount"] = Convert.ToInt32(dataRow["f_LateCount"]) + 1;
							}
							else if (text == CommonStr.strLeaveEarly)
							{
								dataRow["f_LeaveEarlyCount"] = Convert.ToInt32(dataRow["f_LeaveEarlyCount"]) + 1;
							}
						}
						text = this.SetObjToStr(dataRow2["f_OffDuty" + (Conversion.Int(j / 2) + 1) + "AttDesc"]);
						if (text != "")
						{
							if (text == CommonStr.strLateness)
							{
								dataRow["f_LateCount"] = Convert.ToInt32(dataRow["f_LateCount"]) + 1;
							}
							else if (text == CommonStr.strLeaveEarly)
							{
								dataRow["f_LeaveEarlyCount"] = Convert.ToInt32(dataRow["f_LeaveEarlyCount"]) + 1;
							}
						}
					}
					if (flag)
					{
						dataRow["f_DayRealWork"] = Convert.ToInt32(dataRow["f_DayRealWork"]) + 1;
					}
					if (i == dtAttReport.Rows.Count - 1)
					{
						dataRow["f_AttDateEnd"] = dataRow2["f_ShiftDate"];
						this.shift_AttStatistic_updatebyLeave(dataRow);
						dtAttStatistic.Rows.Add(dataRow);
					}
				}
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x0024F330 File Offset: 0x0024E330
		public int shift_AttStatistic_updatebyLeave(DataRow drAttStatistic)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			DateTime now = DateTime.Now;
			this.dsAtt = new DataSet();
			this.errInfo = "";
			int num = -1;
			try
			{
				DateTime dateTime = DateTime.Parse(Strings.Format(drAttStatistic["f_AttDateStart"], "yyyy-MM-dd 12:00:00"));
				DateTime dateTime2 = DateTime.Parse(Strings.Format(drAttStatistic["f_AttDateEnd"], "yyyy-MM-dd 12:00:00"));
				int num2 = Convert.ToInt32(drAttStatistic["f_ConsumerID"]);
				if (dateTime > dateTime2)
				{
					return num;
				}
				string text = "SELECT * FROM t_d_Leave ";
				text = string.Concat(new object[] { text, " WHERE f_ConsumerID = ", num2, " ORDER BY f_Value,f_Value1  " });
				this.daLeave = new OleDbDataAdapter(text, this.cn);
				this.daLeave.Fill(this.dsAtt, "Leave");
				this.dtLeave = this.dsAtt.Tables["Leave"];
				if (this.dtLeave.Rows.Count <= 0)
				{
					return 0;
				}
				dateTime = DateTime.Parse(Strings.Format(drAttStatistic["f_AttDateStart"], "yyyy-MM-dd 08:00:00"));
				dateTime2 = DateTime.Parse(Strings.Format(drAttStatistic["f_AttDateEnd"], "yyyy-MM-dd 20:00:00"));
				object obj = dateTime;
				this.daHolidayType.Fill(this.dsAtt, "HolidayType");
				this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
				comShift_Acc.localizedHolidayType(this.dtHolidayType);
				while (!(Convert.ToDateTime(obj) > dateTime2))
				{
					int i = 0;
					while (i < this.dtLeave.Rows.Count)
					{
						string text2 = Convert.ToString(this.dtLeave.Rows[i]["f_HolidayType"]);
						this.strTemp = Convert.ToString(this.dtLeave.Rows[i]["f_Value"]);
						this.strTemp = wgTools.getDate(this.strTemp);
						this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[i]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
						DateTime dateTime3 = DateTime.Parse(this.strTemp);
						this.strTemp = Convert.ToString(this.dtLeave.Rows[i]["f_Value2"]);
						this.strTemp = wgTools.getDate(this.strTemp);
						this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[i]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
						DateTime dateTime4 = DateTime.Parse(this.strTemp);
						if (Convert.ToDateTime(obj) >= dateTime3 && Convert.ToDateTime(obj) <= dateTime4)
						{
							bool flag = false;
							for (int j = 0; j <= this.dtHolidayType.Rows.Count - 1; j++)
							{
								if (text2 == this.dtHolidayType.Rows[j]["f_HolidayType"].ToString())
								{
									if (drAttStatistic["f_SpecialType" + (j + 1)].ToString() == "")
									{
										drAttStatistic["f_SpecialType" + (j + 1)] = 0;
									}
									drAttStatistic["f_SpecialType" + (j + 1)] = Convert.ToDecimal(drAttStatistic["f_SpecialType" + (j + 1)], CultureInfo.InvariantCulture) + 0.5m;
									flag = true;
									break;
								}
							}
							if (flag)
							{
								break;
							}
							i++;
						}
						else
						{
							if (Convert.ToDateTime(obj) < dateTime3)
							{
								break;
							}
							i++;
						}
					}
					obj = Convert.ToDateTime(obj).AddHours(12.0);
				}
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			finally
			{
				if (this.cn.State == ConnectionState.Open)
				{
					this.cn.Close();
				}
			}
			return num;
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x0024F83C File Offset: 0x0024E83C
		public int shift_AttStatistic_writetodb(DataTable dtAttStatistic)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			string text = "";
			this.cmd = new OleDbCommand();
			bool flag = true;
			this.errInfo = "";
			int num = -1;
			try
			{
				if (dtAttStatistic.Rows.Count > 0)
				{
					this.cmd.Connection = this.cn;
					this.cmd.CommandType = CommandType.Text;
					for (int i = 0; i <= dtAttStatistic.Rows.Count - 1; i++)
					{
						if (this.bStopCreate)
						{
							return num;
						}
						DataRow dataRow = dtAttStatistic.Rows[i];
						text = " INSERT INTO t_d_Shift_AttStatistic ";
						text += " ( f_ConsumerID ";
						text += " , f_AttDateStart, f_AttDateEnd, f_DayShouldWork ";
						text += " , f_DayRealWork";
						text += " , f_LateMinutes,f_LateCount ";
						text += " , f_LeaveEarlyMinutes,f_LeaveEarlyCount ";
						text += " , f_OvertimeHours ";
						text += " , f_AbsenceDays ";
						text += " , f_NotReadCardCount, f_ManualReadTimesCount ";
						for (int j = 1; j <= 32; j++)
						{
							text = text + " , f_SpecialType" + j.ToString();
						}
						text += " )";
						text = text + " Values ( " + dataRow["f_ConsumerID"];
						text = text + "," + this.PrepareStr(dataRow["f_AttDateStart"], true, "yyyy-MM-dd HH:mm:ss");
						text = text + "," + this.PrepareStr(dataRow["f_AttDateEnd"], true, "yyyy-MM-dd HH:mm:ss");
						text = text + "," + dataRow["f_DayShouldWork"];
						text = text + "," + dataRow["f_DayRealWork"];
						text = text + "," + dataRow["f_LateMinutes"];
						text = text + "," + dataRow["f_LateCount"];
						text = text + "," + dataRow["f_LeaveEarlyMinutes"];
						text = text + "," + dataRow["f_LeaveEarlyCount"];
						text = text + "," + dataRow["f_OvertimeHours"];
						text = text + "," + dataRow["f_AbsenceDays"];
						text = text + "," + dataRow["f_NotReadCardCount"];
						text = text + "," + dataRow["f_ManualReadTimesCount"];
						for (int j = 1; j <= 32; j++)
						{
							text = text + " ," + this.PrepareStr(dataRow["f_SpecialType" + j.ToString()]);
						}
						text += ") ";
						if (this.cn.State == ConnectionState.Closed)
						{
							this.cn.Open();
						}
						this.cmd.CommandText = text;
						if (this.cmd.ExecuteNonQuery() <= 0)
						{
							this.errInfo = text;
							flag = false;
							break;
						}
					}
				}
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
				if (flag)
				{
					num = 0;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString() + "\r\n" + text, new object[] { EventLogEntryType.Error });
			}
			finally
			{
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
			}
			return num;
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x0024FBF0 File Offset: 0x0024EBF0
		public int shift_checkvalid(int id, string name, int readtimes, object onduty1, object offduty1, object onduty2, object offduty2, object onduty3, object offduty3, object onduty4, object offduty4)
		{
			int num = -1;
			this.errInfo = "";
			try
			{
				if (id > 99)
				{
					num = -2;
					this.errInfo = id.ToString();
					return num;
				}
				if (readtimes != 2 && readtimes != 4 && readtimes != 6 && readtimes != 8)
				{
					num = -3;
					this.errInfo = (-3).ToString();
					return num;
				}
				int num2 = 0;
				this.realOnduty1 = DateTime.Parse(Strings.Format(onduty1, "2000-1-1 HH:mm:ss"));
				if (this.tm(onduty1) > this.tm(offduty1))
				{
					num2++;
				}
				this.realOffduty1 = DateTime.Parse(Strings.Format(offduty1, "2000-1-1 HH:mm:ss")).AddDays((double)num2);
				int num3 = (int)this.realOffduty1.Subtract(this.realOnduty1).TotalMinutes;
				if (num3 < this.minShifDiffByMinute)
				{
					return -4;
				}
				if (readtimes <= 2)
				{
					return 0;
				}
				if (this.tm(offduty1) > this.tm(onduty2))
				{
					num2++;
				}
				this.realOnduty2 = DateTime.Parse(Strings.Format(onduty2, "2000-1-1 HH:mm:ss")).AddDays((double)num2);
				num3 = (int)this.realOnduty2.Subtract(this.realOffduty1).TotalMinutes;
				if (num3 < this.minShifDiffByMinute)
				{
					return -4;
				}
				if (this.tm(onduty2) > this.tm(offduty2))
				{
					num2++;
				}
				this.realOffduty2 = DateTime.Parse(Strings.Format(offduty2, "2000-1-1 HH:mm:ss")).AddDays((double)num2);
				num3 = (int)this.realOffduty2.Subtract(this.realOnduty2).TotalMinutes;
				if (num3 < this.minShifDiffByMinute)
				{
					return -4;
				}
				num3 = (int)this.realOffduty2.Subtract(this.realOnduty1).TotalMinutes;
				if (num3 >= 1440)
				{
					return -5;
				}
				if (readtimes <= 4)
				{
					return 0;
				}
				if (this.tm(offduty2) > this.tm(onduty3))
				{
					num2++;
				}
				this.realOnduty3 = DateTime.Parse(Strings.Format(onduty3, "2000-1-1 HH:mm:ss")).AddDays((double)num2);
				num3 = (int)this.realOnduty3.Subtract(this.realOffduty2).TotalMinutes;
				if (num3 < this.minShifDiffByMinute)
				{
					return -4;
				}
				if (this.tm(onduty3) > this.tm(offduty3))
				{
					num2++;
				}
				this.realOffduty3 = DateTime.Parse(Strings.Format(offduty3, "2000-1-1 HH:mm:ss")).AddDays((double)num2);
				num3 = (int)this.realOffduty3.Subtract(this.realOnduty3).TotalMinutes;
				if (num3 < this.minShifDiffByMinute)
				{
					return -4;
				}
				num3 = (int)this.realOffduty3.Subtract(this.realOnduty1).TotalMinutes;
				if (num3 >= 1440)
				{
					return -5;
				}
				if (readtimes <= 6)
				{
					return 0;
				}
				if (this.tm(offduty3) > this.tm(onduty4))
				{
					num2++;
				}
				this.realOnduty4 = DateTime.Parse(Strings.Format(onduty4, "2000-1-1 HH:mm:ss")).AddDays((double)num2);
				num3 = (int)this.realOnduty4.Subtract(this.realOffduty3).TotalMinutes;
				if (num3 < this.minShifDiffByMinute)
				{
					return -4;
				}
				if (this.tm(onduty4) > this.tm(offduty4))
				{
					num2++;
				}
				this.realOffduty4 = DateTime.Parse(Strings.Format(offduty4, "2000-1-1 HH:mm:ss")).AddDays((double)num2);
				num3 = (int)this.realOffduty4.Subtract(this.realOnduty4).TotalMinutes;
				if (num3 < this.minShifDiffByMinute)
				{
					return -4;
				}
				num3 = (int)this.realOffduty4.Subtract(this.realOnduty1).TotalMinutes;
				if (num3 >= 1440)
				{
					return -5;
				}
				if (readtimes <= 8)
				{
					num = 0;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x00250028 File Offset: 0x0024F028
		public int shift_delete(int id)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			int num = -1;
			try
			{
				string text = " DELETE FROM t_b_ShiftSet WHERE f_ShiftID = " + id;
				if (this.cn.State != ConnectionState.Open)
				{
					this.cn.Open();
				}
				this.cmd = new OleDbCommand(text, this.cn);
				this.cmd.ExecuteNonQuery();
				this.cn.Close();
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			finally
			{
				if (this.cn.State == ConnectionState.Open)
				{
					this.cn.Close();
				}
			}
			return num;
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x002500F8 File Offset: 0x0024F0F8
		public int shift_rule_checkValid(int ruleLen, int[] shiftRule)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			this.errInfo = "";
			int num = -1;
			if (ruleLen > 1)
			{
				object[,] array = new object[ruleLen + 1, 13];
				num = -1;
				try
				{
					for (int i = 0; i <= ruleLen - 1; i++)
					{
						if (this.bStopCreate)
						{
							return num;
						}
						string text = " SELECT * FROM t_b_ShiftSet WHERE f_ShiftID = " + shiftRule[i];
						if (this.cn.State != ConnectionState.Open)
						{
							this.cn.Open();
						}
						this.cmd = new OleDbCommand(text, this.cn);
						OleDbDataReader oleDbDataReader = this.cmd.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							for (int j = 0; j <= oleDbDataReader.FieldCount - 1; j++)
							{
								array[i, j] = oleDbDataReader[j];
							}
						}
						else
						{
							array[i, 0] = 0;
							if (shiftRule[i] != 0)
							{
								num = -6;
								this.errInfo = shiftRule[i].ToString();
								return num;
							}
						}
						oleDbDataReader.Close();
					}
					this.cn.Close();
					for (int i = 1; i <= ruleLen - 1; i++)
					{
						if (this.bStopCreate)
						{
							return num;
						}
						if (Convert.ToInt32(array[i - 1, 0]) != 0 && Convert.ToInt32(array[i, 0]) != 0)
						{
							int num2 = Convert.ToInt32(array[i - 1, 2]);
							DateTime dateTime = Convert.ToDateTime(array[i - 1, 2 + num2]);
							Convert.ToInt32(array[i, 2]);
							DateTime dateTime2 = Convert.ToDateTime(array[i, 3]).AddDays(1.0);
							if (dateTime2 <= dateTime || dateTime2.Subtract(dateTime).TotalMinutes < (double)this.minShifDiffByMinute)
							{
								num = -7;
								this.errInfo = string.Concat(new object[]
								{
									CommonStr.strShift,
									shiftRule[i - 1],
									CommonStr.strLastOffDuty,
									Strings.Format(dateTime, "HH:mm"),
									", "
								});
								object obj = this.errInfo;
								this.errInfo = string.Concat(new object[]
								{
									obj,
									CommonStr.strShift,
									shiftRule[i],
									CommonStr.strFirstOnDuty,
									Strings.Format(dateTime2, "HH:mm"),
									CommonStr.strTimeOverlapped
								});
								return num;
							}
						}
					}
					num = 0;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				}
				finally
				{
					if (this.cn.State == ConnectionState.Open)
					{
						this.cn.Close();
					}
				}
				return num;
			}
			return 0;
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x0025041C File Offset: 0x0024F41C
		public int shift_update(int id, string name, int readtimes, object onduty1, object offduty1, object onduty2, object offduty2, object onduty3, object offduty3, object onduty4, object offduty4, int bOvertimeShift)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			int num = -1;
			this.errInfo = "";
			try
			{
				num = this.shift_checkvalid(id, name, readtimes, onduty1, offduty1, onduty2, offduty2, onduty3, offduty3, onduty4, offduty4);
				if (num != 0)
				{
					return num;
				}
				string text = " SELECT count(*) FROM t_b_ShiftSet WHERE f_ShiftID = " + id;
				if (this.cn.State != ConnectionState.Open)
				{
					this.cn.Open();
				}
				this.cmd = new OleDbCommand(text, this.cn);
				int num2 = Convert.ToInt32(this.cmd.ExecuteScalar());
				this.cn.Close();
				if (num2 == 0)
				{
					num = -2;
					this.errInfo = id.ToString();
					return num;
				}
				text = " UPDATE t_b_ShiftSet Set ";
				text = string.Concat(new object[]
				{
					text,
					" f_ShiftName = ",
					this.PrepareStr(name),
					" , f_ReadTimes = ",
					readtimes,
					" , f_OnDuty1 = ",
					this.PrepareStr(this.realOnduty1, true, "yyyy-MM-dd HH:mm:ss"),
					" , f_OffDuty1 = ",
					this.PrepareStr(this.realOffduty1, true, "yyyy-MM-dd HH:mm:ss")
				});
				if (readtimes > 2)
				{
					text = string.Concat(new string[]
					{
						text,
						" , f_OnDuty2 = ",
						this.PrepareStr(this.realOnduty2, true, "yyyy-MM-dd HH:mm:ss"),
						" , f_OffDuty2 = ",
						this.PrepareStr(this.realOffduty2, true, "yyyy-MM-dd HH:mm:ss")
					});
				}
				else
				{
					text = string.Concat(new string[]
					{
						text,
						" , f_OnDuty2 = ",
						this.PrepareStr(""),
						" , f_OffDuty2 = ",
						this.PrepareStr("")
					});
				}
				if (readtimes > 4)
				{
					text = string.Concat(new string[]
					{
						text,
						" , f_OnDuty3 = ",
						this.PrepareStr(this.realOnduty3, true, "yyyy-MM-dd HH:mm:ss"),
						" , f_OffDuty3 =",
						this.PrepareStr(this.realOffduty3, true, "yyyy-MM-dd HH:mm:ss")
					});
				}
				else
				{
					text = string.Concat(new string[]
					{
						text,
						" , f_OnDuty3 = ",
						this.PrepareStr(""),
						" , f_OffDuty3 =",
						this.PrepareStr("")
					});
				}
				if (readtimes > 6)
				{
					text = string.Concat(new string[]
					{
						text,
						" , f_OnDuty4  = ",
						this.PrepareStr(this.realOnduty4, true, "yyyy-MM-dd HH:mm:ss"),
						" , f_OffDuty4 = ",
						this.PrepareStr(this.realOffduty4, true, "yyyy-MM-dd HH:mm:ss")
					});
				}
				else
				{
					text = string.Concat(new string[]
					{
						text,
						" , f_OnDuty4  = ",
						this.PrepareStr(""),
						" , f_OffDuty4 = ",
						this.PrepareStr("")
					});
				}
				text = string.Concat(new object[]
				{
					text,
					" ,f_bOvertimeShift = ",
					bOvertimeShift,
					" ,f_Notes = ",
					this.PrepareStr(""),
					" WHERE  f_ShiftID = ",
					id
				});
				if (this.cn.State != ConnectionState.Open)
				{
					this.cn.Open();
				}
				this.cmd = new OleDbCommand(text, this.cn);
				num2 = this.cmd.ExecuteNonQuery();
				this.cn.Close();
				if (num2 > 0)
				{
					return 0;
				}
				return -1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			finally
			{
				if (this.cn.State == ConnectionState.Open)
				{
					this.cn.Close();
				}
			}
			return num;
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x0025087C File Offset: 0x0024F87C
		public int shift_work_schedule_analyst(DataTable dtShiftWorkSchedule)
		{
			this.errInfo = "";
			int num = -1;
			try
			{
				int num2 = 0;
				for (int i = 0; i <= dtShiftWorkSchedule.Rows.Count - 1; i++)
				{
					if (this.bStopCreate)
					{
						return num;
					}
					object obj = dtShiftWorkSchedule.Rows[i]["f_PlanTime"];
					if (!Information.IsDBNull(obj))
					{
						int num3 = Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_TimeSeg"]);
						int num4 = Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_ReadTimes"]);
						int num5 = Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_TimeSeg"]) & 1;
						if (Information.IsDBNull(dtShiftWorkSchedule.Rows[i]["f_WorkTime"]))
						{
							dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = CommonStr.strNotReadCard;
						}
						else
						{
							object obj2 = dtShiftWorkSchedule.Rows[i]["f_WorkTime"];
							TimeSpan timeSpan = Convert.ToDateTime(obj).Subtract(Convert.ToDateTime(obj2));
							dtShiftWorkSchedule.Rows[i]["f_Duration"] = timeSpan.Duration().TotalMinutes;
							if (num5 == 0 && timeSpan.TotalMinutes < (double)(-(double)this.tLateTimeout))
							{
								dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = CommonStr.strLateness;
							}
							if (num5 == 1)
							{
								if (timeSpan.TotalMinutes > (double)this.tLeaveTimeout)
								{
									dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = CommonStr.strLeaveEarly;
								}
								else if (num3 == num4 - 1 && timeSpan.TotalMinutes <= (double)(-(double)this.tOvertimeTimeout))
								{
									dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = CommonStr.strOvertime;
								}
							}
						}
						if (num3 == 0)
						{
							num2 = i;
						}
						if (num3 == num4 - 1)
						{
							if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_bOvertimeShift"]) == "1")
							{
								for (int j = num2; j <= i; j++)
								{
									if (this.SetObjToStr(dtShiftWorkSchedule.Rows[j]["f_CardRecordDesc"]) == CommonStr.strSignIn || this.SetObjToStr(dtShiftWorkSchedule.Rows[j]["f_CardRecordDesc"]) == "")
									{
										dtShiftWorkSchedule.Rows[j]["f_AttDesc"] = CommonStr.strOvertime;
									}
								}
							}
							else if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_bOvertimeShift"]) == "2")
							{
								if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"]) == CommonStr.strNotReadCard && this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_CardRecordDesc"]) == "" && this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_AttDesc"]) == CommonStr.strNotReadCard && this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"]) == "")
								{
									dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"] = "";
									dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = "";
								}
								else if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_CardRecordDesc"]) == CommonStr.strSignIn || this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_CardRecordDesc"]) == "")
								{
									if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"]) != CommonStr.strNotReadCard || this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_CardRecordDesc"]) != "")
									{
										dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"] = CommonStr.strOvertime;
									}
									else if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"]) == CommonStr.strNotReadCard)
									{
										dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"] = "";
									}
									if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_AttDesc"]) != CommonStr.strNotReadCard || this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"]) != "")
									{
										dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = CommonStr.strOvertime;
									}
									else if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_AttDesc"]) == CommonStr.strNotReadCard)
									{
										dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = "";
									}
								}
							}
							else
							{
								bool flag = false;
								for (int j = num2; j <= i; j++)
								{
									if (this.SetObjToStr(dtShiftWorkSchedule.Rows[j]["f_AttDesc"]) != CommonStr.strNotReadCard || this.SetObjToStr(dtShiftWorkSchedule.Rows[j]["f_CardRecordDesc"]) != "")
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									for (int j = num2; j <= i; j++)
									{
										dtShiftWorkSchedule.Rows[j]["f_AttDesc"] = CommonStr.strAbsence;
									}
								}
							}
						}
					}
				}
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x00250EF0 File Offset: 0x0024FEF0
		public int shift_work_schedule_cleardb()
		{
			this.errInfo = "";
			int num = -1;
			try
			{
				if (wgAppConfig.runUpdateSql("Delete From t_d_Shift_Work_Schedule") >= 0)
				{
					num = 0;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x00250F4C File Offset: 0x0024FF4C
		public int shift_work_schedule_create(out DataTable dtShiftWorkSchedule)
		{
			this.dtShiftWork = new DataTable("t_d_ShiftWork");
			int num = -1;
			dtShiftWorkSchedule = null;
			try
			{
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_ConsumerID";
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_ShiftDate";
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_ShiftID";
				this.dc.DefaultValue = -1;
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_Readtimes";
				this.dc.DefaultValue = 0;
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_PlanTime";
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_TimeSeg";
				this.dc.DefaultValue = 0;
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_WorkTime";
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.String");
				this.dc.ColumnName = "f_AttDesc";
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.String");
				this.dc.ColumnName = "f_CardRecordDesc";
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_Duration";
				this.dc.DefaultValue = 0;
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_bOvertimeShift";
				this.dc.DefaultValue = 0;
				this.dtShiftWork.Columns.Add(this.dc);
				dtShiftWorkSchedule = this.dtShiftWork.Copy();
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x00251314 File Offset: 0x00250314
		public int shift_work_schedule_fill(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd, ref int bNotArranged)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			bool flag = false;
			object[] array = new object[37];
			this.errInfo = "";
			int num = -1;
			DateTime dateTime = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
			DateTime dateTime2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
			if (dateTime <= dateTime2)
			{
				try
				{
					string text = "";
					DateTime dateTime3 = dateTime;
					do
					{
						if (text != Strings.Format(dateTime3, "yyyy-MM"))
						{
							text = Strings.Format(dateTime3, "yyyy-MM");
							string text2 = " SELECT * FROM t_d_ShiftData ";
							text2 = string.Concat(new object[]
							{
								text2,
								" WHERE f_ConsumerID = ",
								consumerid,
								" AND f_DateYM = ",
								this.PrepareStr(text)
							});
							if (this.cn.State != ConnectionState.Open)
							{
								this.cn.Open();
							}
							this.cmd = new OleDbCommand(text2, this.cn);
							OleDbDataReader oleDbDataReader = this.cmd.ExecuteReader();
							if (oleDbDataReader.Read())
							{
								flag = true;
								for (int i = 0; i <= oleDbDataReader.FieldCount - 1; i++)
								{
									array[i] = oleDbDataReader[i];
								}
							}
							else
							{
								bNotArranged |= 1;
								flag = false;
								array[0] = -1;
							}
							oleDbDataReader.Close();
						}
						do
						{
							if (!flag)
							{
								DataRow dataRow = dtShiftWorkSchedule.NewRow();
								dataRow[0] = consumerid;
								dataRow[1] = dateTime3;
								dataRow["f_ShiftID"] = -1;
								dataRow["f_Readtimes"] = 0;
								dataRow["f_Duration"] = 0;
								dtShiftWorkSchedule.Rows.Add(dataRow);
							}
							else
							{
								int num2 = Convert.ToInt32(array[2 + dateTime3.Day]);
								if (num2 <= 0)
								{
									DataRow dataRow = dtShiftWorkSchedule.NewRow();
									dataRow[0] = consumerid;
									dataRow[1] = dateTime3;
									dataRow[2] = num2;
									dataRow["f_Readtimes"] = 0;
									dataRow["f_Duration"] = 0;
									switch (num2)
									{
									case -1:
										bNotArranged |= 2;
										break;
									case 0:
										dataRow["f_AttDesc"] = CommonStr.strRest;
										break;
									}
									dtShiftWorkSchedule.Rows.Add(dataRow);
								}
								else
								{
									string text2 = "SELECT * FROM t_b_ShiftSet WHERE f_ShiftID = " + num2;
									this.cmd = new OleDbCommand(text2, this.cn);
									if (this.cn.State != ConnectionState.Open)
									{
										this.cn.Open();
									}
									OleDbDataReader oleDbDataReader = this.cmd.ExecuteReader();
									if (oleDbDataReader.Read())
									{
										for (int j = 1; j <= Convert.ToInt32(oleDbDataReader["f_ReadTimes"]); j++)
										{
											DataRow dataRow = dtShiftWorkSchedule.NewRow();
											dataRow["f_Readtimes"] = 0;
											dataRow[0] = consumerid;
											dataRow[1] = dateTime3;
											dataRow[2] = num2;
											dataRow["f_Readtimes"] = oleDbDataReader["f_ReadTimes"];
											dataRow["f_PlanTime"] = Convert.ToDateTime(Strings.Format(dateTime3.AddDays((double)(Convert.ToInt32(Strings.Format(oleDbDataReader[j + 2], "dd")) - 1)), "yyyy-MM-dd") + Strings.Format(oleDbDataReader[j + 2], " HH:mm:ss"));
											dataRow["f_TimeSeg"] = j - 1;
											dataRow["f_Duration"] = 0;
											dataRow["f_bOvertimeShift"] = oleDbDataReader["f_bOvertimeShift"];
											dtShiftWorkSchedule.Rows.Add(dataRow);
										}
									}
									else
									{
										DataRow dataRow = dtShiftWorkSchedule.NewRow();
										dataRow[0] = consumerid;
										dataRow[1] = dateTime3;
										dataRow[2] = num2;
										dataRow[3] = -1;
										dataRow["f_Readtimes"] = -1;
										dataRow["f_Duration"] = 0;
										dtShiftWorkSchedule.Rows.Add(dataRow);
										bNotArranged |= 4;
									}
									oleDbDataReader.Close();
								}
							}
							dateTime3 = dateTime3.AddDays(1.0);
						}
						while (!(text != Strings.Format(dateTime3, "yyyy-MM")) && dateTime3 <= dateTime2);
					}
					while (dateTime3 <= dateTime2);
					num = 0;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				}
				finally
				{
					if (this.cn.State == ConnectionState.Open)
					{
						this.cn.Close();
					}
				}
			}
			return num;
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x0025187C File Offset: 0x0025087C
		public int shift_work_schedule_updatebyLeave(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			DateTime now = DateTime.Now;
			this.dsAtt = new DataSet();
			this.errInfo = "";
			int num = -1;
			DateTime dateTime = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
			DateTime dateTime2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
			if (dateTime <= dateTime2)
			{
				try
				{
					string text = "SELECT * FROM t_d_Leave ";
					text = string.Concat(new object[] { text, " WHERE f_ConsumerID = ", consumerid, " ORDER BY f_Value,f_Value1  " });
					this.daLeave = new OleDbDataAdapter(text, this.cn);
					this.daLeave.Fill(this.dsAtt, "Leave");
					this.dtLeave = this.dsAtt.Tables["Leave"];
					if (this.dtLeave.Rows.Count <= 0)
					{
						return 0;
					}
					int i = 0;
					while (i <= dtShiftWorkSchedule.Rows.Count - 1)
					{
						if (this.bStopCreate)
						{
							return num;
						}
						object obj = dtShiftWorkSchedule.Rows[i]["f_PlanTime"];
						if (Information.IsDBNull(obj))
						{
							int num2 = 0;
							obj = dtShiftWorkSchedule.Rows[i]["f_shiftDate"];
							for (;;)
							{
								if (num2 < this.dtLeave.Rows.Count)
								{
									string text2 = Convert.ToString(this.dtLeave.Rows[num2]["f_HolidayType"]);
									this.strTemp = Convert.ToString(this.dtLeave.Rows[num2]["f_Value"]);
									this.strTemp = wgTools.getDate(this.strTemp);
									this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[num2]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
									DateTime dateTime3;
									DateTime.TryParse(this.strTemp, out dateTime3);
									this.strTemp = Convert.ToString(this.dtLeave.Rows[num2]["f_Value2"]);
									this.strTemp = wgTools.getDate(this.strTemp);
									this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[num2]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
									DateTime dateTime4;
									DateTime.TryParse(this.strTemp, out dateTime4);
									if (Convert.ToDateTime(obj) >= dateTime3 && Convert.ToDateTime(obj) <= dateTime4)
									{
										dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"] = text2;
										dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = text2;
									}
									else
									{
										num2++;
									}
								}
							}
						}
						else
						{
							Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_TimeSeg"]);
							Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_ReadTimes"]);
							for (int j = 0; j < this.dtLeave.Rows.Count; j++)
							{
								string text3 = Convert.ToString(this.dtLeave.Rows[j]["f_HolidayType"]);
								this.strTemp = Convert.ToString(this.dtLeave.Rows[j]["f_Value"]);
								this.strTemp = wgTools.getDate(this.strTemp);
								this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[j]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
								DateTime dateTime5;
								DateTime.TryParse(this.strTemp, out dateTime5);
								this.strTemp = Convert.ToString(this.dtLeave.Rows[j]["f_Value2"]);
								this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[j]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
								DateTime dateTime6;
								DateTime.TryParse(this.strTemp, out dateTime6);
								if (Convert.ToDateTime(obj) >= dateTime5 && Convert.ToDateTime(obj) <= dateTime6)
								{
									dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = obj;
									dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"] = text3;
									break;
								}
							}
							i++;
						}
					}
					num = 0;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				}
				finally
				{
					if (this.cn.State == ConnectionState.Open)
					{
						this.cn.Close();
					}
				}
				return num;
			}
			return num;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x00251E28 File Offset: 0x00250E28
		public int shift_work_schedule_updatebyManualReadcard(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			this.dsAtt = new DataSet();
			this.errInfo = "";
			int num = -1;
			DateTime dateTime = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
			DateTime dateTime2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
			if (dateTime <= dateTime2)
			{
				try
				{
					string text = "SELECT t_d_ManualCardRecord.f_ConsumerID,  t_d_ManualCardRecord.f_ReadDate, t_d_ManualCardRecord.f_DutyOnOff FROM t_d_ManualCardRecord ";
					text = string.Concat(new object[]
					{
						text,
						" WHERE f_ConsumerID = ",
						consumerid,
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(dateStart, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(dateEnd.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  ORDER BY f_ReadDate ASC "
					});
					this.daCardRecord = new OleDbDataAdapter(text, this.cn);
					this.daCardRecord.Fill(this.dsAtt, "CardRecord");
					DataTable dataTable = this.dsAtt.Tables["CardRecord"];
					this.daCardRecord = new OleDbDataAdapter("SELECT t_d_ManualCardRecord.f_ConsumerID,  t_d_ManualCardRecord.f_ReadDate, t_d_ManualCardRecord.f_DutyOnOff FROM t_d_ManualCardRecord WHERE 1<0 ", this.cn);
					this.daCardRecord.Fill(this.dsAtt, "ValidCardRecord");
					DataTable dataTable2 = this.dsAtt.Tables["ValidCardRecord"];
					if (dataTable.Rows.Count > 0)
					{
						object[] array = new object[dataTable.Columns.Count - 1 + 1];
						DateTime dateTime3 = Convert.ToDateTime(dataTable.Rows[0]["f_ReadDate"]);
						DataRow dataRow = dataTable2.NewRow();
						dataTable.Rows[0].ItemArray.CopyTo(array, 0);
						dataRow.ItemArray = array;
						dataTable2.Rows.Add(dataRow);
						dateTime3 = Convert.ToDateTime(dataRow["f_ReadDate"]);
						for (int i = 1; i <= dataTable.Rows.Count - 1; i++)
						{
							if (this.bStopCreate)
							{
								return num;
							}
							DateTime dateTime4 = Convert.ToDateTime(dataTable.Rows[i]["f_ReadDate"]);
							if (dateTime4.Subtract(dateTime3).TotalSeconds > (double)this.tTwoReadMintime)
							{
								dateTime3 = dateTime4;
								dataRow = dataTable2.NewRow();
								dataTable.Rows[i].ItemArray.CopyTo(array, 0);
								dataRow.ItemArray = array;
								dataTable2.Rows.Add(dataRow);
							}
						}
					}
					int j = 0;
					for (int k = 0; k <= dtShiftWorkSchedule.Rows.Count - 1; k++)
					{
						if (this.bStopCreate)
						{
							return num;
						}
						object obj = dtShiftWorkSchedule.Rows[k]["f_PlanTime"];
						if (!Information.IsDBNull(obj))
						{
							int num2 = Convert.ToInt32(dtShiftWorkSchedule.Rows[k]["f_TimeSeg"]);
							int num3 = Convert.ToInt32(dtShiftWorkSchedule.Rows[k]["f_ReadTimes"]);
							while (j < dataTable2.Rows.Count)
							{
								object obj2 = dataTable2.Rows[j]["f_ReadDate"];
								TimeSpan timeSpan = Convert.ToDateTime(obj).Subtract(Convert.ToDateTime(obj2));
								if (num2 == 0)
								{
									if (timeSpan.TotalMinutes > (double)this.tAheadMinutesOnDutyFirst)
									{
										j++;
									}
									else
									{
										if (timeSpan.TotalMinutes > (double)(-(double)this.tDelayMinutes))
										{
											dtShiftWorkSchedule.Rows[k]["f_WorkTime"] = obj2;
											dtShiftWorkSchedule.Rows[k]["f_CardRecordDesc"] = CommonStr.strSignIn;
											j++;
											break;
										}
										break;
									}
								}
								if (num2 == num3 - 1)
								{
									if (timeSpan.TotalMinutes < (double)(-(double)this.tOvertimeMinutes))
									{
										break;
									}
									if (timeSpan.TotalMinutes > (double)this.tAheadMinutes)
									{
										j++;
									}
									else if (timeSpan.TotalMinutes > (double)(-(double)this.tDelayMinutes))
									{
										dtShiftWorkSchedule.Rows[k]["f_WorkTime"] = obj2;
										dtShiftWorkSchedule.Rows[k]["f_CardRecordDesc"] = CommonStr.strSignIn;
										j++;
									}
									else
									{
										if (k + 1 < dtShiftWorkSchedule.Rows.Count - 1 && !Information.IsDBNull(dtShiftWorkSchedule.Rows[k + 1]["f_PlanTime"]) && Convert.ToDateTime(dtShiftWorkSchedule.Rows[k + 1]["f_PlanTime"]).AddMinutes((double)(-(double)this.tAheadMinutesOnDutyFirst)) < Convert.ToDateTime(obj2))
										{
											break;
										}
										dtShiftWorkSchedule.Rows[k]["f_WorkTime"] = obj2;
										dtShiftWorkSchedule.Rows[k]["f_CardRecordDesc"] = CommonStr.strSignIn;
										j++;
									}
								}
								if (num2 > 0 && num2 < num3 - 1)
								{
									if (timeSpan.TotalMinutes <= (double)this.tAheadMinutes)
									{
										if (timeSpan.TotalMinutes < (double)(-(double)this.tDelayMinutes))
										{
											break;
										}
										dtShiftWorkSchedule.Rows[k]["f_WorkTime"] = obj2;
										dtShiftWorkSchedule.Rows[k]["f_CardRecordDesc"] = CommonStr.strSignIn;
										j++;
										if ((num2 & 1) == 0)
										{
											break;
										}
										if (timeSpan.TotalMinutes <= 0.0)
										{
											break;
										}
									}
									else
									{
										j++;
									}
								}
							}
						}
					}
					num = 0;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				}
				finally
				{
					if (this.cn.State == ConnectionState.Open)
					{
						this.cn.Close();
					}
				}
				return num;
			}
			return num;
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x00252470 File Offset: 0x00251470
		public int shift_work_schedule_updatebyReadcard(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			this.dsAtt = new DataSet();
			this.errInfo = "";
			int num = -1;
			DateTime dateTime = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
			DateTime dateTime2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
			if (dateTime <= dateTime2)
			{
				try
				{
					string text = "SELECT t_d_SwipeRecord.f_ConsumerID, t_d_SwipeRecord.f_ReaderID, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_DutyOnOff  FROM t_d_SwipeRecord, t_b_Reader ";
					text = string.Concat(new string[]
					{
						text,
						wgAppConfig.getMoreCardShiftOneUserCondition(consumerid),
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(dateStart, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(dateEnd.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  AND (t_d_SwipeRecord.f_ReaderID = t_b_Reader.f_ReaderID)  AND t_b_Reader.f_Attend = 1 "
					});
					if (wgAppConfig.getSystemParamByNO(54) == "1")
					{
						text += " AND f_Character >= 1 ";
					}
					text += " ORDER BY f_ReadDate ASC, f_RecID ASC ";
					this.cmd = new OleDbCommand();
					this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
					this.cmd.Connection = this.cn;
					this.cmd.CommandText = text;
					this.cmd.CommandType = CommandType.Text;
					this.daCardRecord = new OleDbDataAdapter(this.cmd);
					this.daCardRecord.Fill(this.dsAtt, "CardRecord");
					this.dtCardRecord = this.dsAtt.Tables["CardRecord"];
					this.daCardRecord = new OleDbDataAdapter("SELECT t_d_SwipeRecord.f_ConsumerID, t_d_SwipeRecord.f_ReaderID, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_DutyOnOff FROM t_d_SwipeRecord, t_b_Reader WHERE 1<0  ", this.cn);
					this.daCardRecord.Fill(this.dsAtt, "ValidCardRecord");
					this.dtValidCardRecord = this.dsAtt.Tables["ValidCardRecord"];
					if (this.dtCardRecord.Rows.Count > 0)
					{
						object[] array = new object[this.dtCardRecord.Columns.Count - 1 + 1];
						DateTime dateTime3 = Convert.ToDateTime(this.dtCardRecord.Rows[0]["f_ReadDate"]);
						int num2 = Convert.ToInt32(this.dtCardRecord.Rows[0]["f_DutyOnOff"]);
						DataRow dataRow = this.dtValidCardRecord.NewRow();
						this.dtCardRecord.Rows[0].ItemArray.CopyTo(array, 0);
						dataRow.ItemArray = array;
						this.dtValidCardRecord.Rows.Add(dataRow);
						dateTime3 = Convert.ToDateTime(dataRow["f_ReadDate"]);
						num2 = Convert.ToInt32(dataRow["f_DutyOnOff"]);
						for (int i = 1; i <= this.dtCardRecord.Rows.Count - 1; i++)
						{
							if (this.bStopCreate)
							{
								return num;
							}
							DateTime dateTime4 = Convert.ToDateTime(this.dtCardRecord.Rows[i]["f_ReadDate"]);
							TimeSpan timeSpan = dateTime4.Subtract(dateTime3);
							int num3 = Convert.ToInt32(this.dtCardRecord.Rows[i]["f_DutyOnOff"]);
							if (timeSpan.TotalSeconds > (double)this.tTwoReadMintime || num3 != num2)
							{
								dateTime3 = dateTime4;
								num2 = num3;
								dataRow = this.dtValidCardRecord.NewRow();
								this.dtCardRecord.Rows[i].ItemArray.CopyTo(array, 0);
								dataRow.ItemArray = array;
								this.dtValidCardRecord.Rows.Add(dataRow);
							}
						}
					}
					int j = 0;
					for (int k = 0; k <= dtShiftWorkSchedule.Rows.Count - 1; k++)
					{
						if (this.bStopCreate)
						{
							return num;
						}
						object obj = dtShiftWorkSchedule.Rows[k]["f_PlanTime"];
						if (!Information.IsDBNull(obj))
						{
							int num4 = Convert.ToInt32(dtShiftWorkSchedule.Rows[k]["f_TimeSeg"]);
							int num5 = Convert.ToInt32(dtShiftWorkSchedule.Rows[k]["f_ReadTimes"]);
							while (j < this.dtValidCardRecord.Rows.Count)
							{
								object obj2 = this.dtValidCardRecord.Rows[j]["f_ReadDate"];
								TimeSpan timeSpan2 = Convert.ToDateTime(obj).Subtract(Convert.ToDateTime(obj2));
								if (num4 == 0)
								{
									if (timeSpan2.TotalMinutes > (double)this.tAheadMinutesOnDutyFirst)
									{
										j++;
									}
									else
									{
										if (timeSpan2.TotalMinutes <= (double)(-(double)this.tDelayMinutes))
										{
											break;
										}
										if ((Convert.ToInt32(this.dtValidCardRecord.Rows[j]["f_DutyOnOff"]) & 1) == 1)
										{
											dtShiftWorkSchedule.Rows[k]["f_WorkTime"] = obj2;
											break;
										}
										if (timeSpan2.TotalMinutes < 0.0 && Convert.ToInt32(this.dtValidCardRecord.Rows[j]["f_DutyOnOff"]) == 2)
										{
											break;
										}
										j++;
									}
								}
								if (num4 == num5 - 1)
								{
									if (timeSpan2.TotalMinutes < (double)(-(double)this.tOvertimeMinutes))
									{
										break;
									}
									if (timeSpan2.TotalMinutes > (double)this.tAheadMinutes)
									{
										j++;
									}
									else if (timeSpan2.TotalMinutes > (double)(-(double)this.tDelayMinutes))
									{
										if ((Convert.ToInt32(this.dtValidCardRecord.Rows[j]["f_DutyOnOff"]) & 2) == 2)
										{
											dtShiftWorkSchedule.Rows[k]["f_WorkTime"] = obj2;
											j++;
										}
										else
										{
											if (timeSpan2.TotalMinutes < 0.0 && Convert.ToInt32(this.dtValidCardRecord.Rows[j]["f_DutyOnOff"]) == 1)
											{
												break;
											}
											j++;
										}
									}
									else
									{
										if ((k + 1 < dtShiftWorkSchedule.Rows.Count - 1 && !Information.IsDBNull(dtShiftWorkSchedule.Rows[k + 1]["f_PlanTime"]) && Convert.ToDateTime(dtShiftWorkSchedule.Rows[k + 1]["f_PlanTime"]).AddMinutes((double)(-(double)this.tAheadMinutesOnDutyFirst)) < Convert.ToDateTime(obj2)) || timeSpan2.TotalMinutes < (double)(-(double)this.tOvertimeMinutes))
										{
											break;
										}
										if ((Convert.ToInt32(this.dtValidCardRecord.Rows[j]["f_DutyOnOff"]) & 2) == 2)
										{
											dtShiftWorkSchedule.Rows[k]["f_WorkTime"] = obj2;
											j++;
										}
										else
										{
											if (timeSpan2.TotalMinutes < 0.0 && Convert.ToInt32(this.dtValidCardRecord.Rows[j]["f_DutyOnOff"]) == 1)
											{
												break;
											}
											j++;
										}
									}
								}
								if (num4 > 0 && num4 < num5 - 1)
								{
									if (timeSpan2.TotalMinutes <= (double)this.tAheadMinutes)
									{
										if (timeSpan2.TotalMinutes < (double)(-(double)this.tDelayMinutes))
										{
											break;
										}
										if ((num4 & 1) == 0)
										{
											if ((Convert.ToInt32(this.dtValidCardRecord.Rows[j]["f_DutyOnOff"]) & 1) == 1)
											{
												dtShiftWorkSchedule.Rows[k]["f_WorkTime"] = obj2;
												j++;
												break;
											}
											if (timeSpan2.TotalMinutes < 0.0 && Convert.ToInt32(this.dtValidCardRecord.Rows[j]["f_DutyOnOff"]) == 2)
											{
												break;
											}
											j++;
										}
										else if ((Convert.ToInt32(this.dtValidCardRecord.Rows[j]["f_DutyOnOff"]) & 2) == 2)
										{
											dtShiftWorkSchedule.Rows[k]["f_WorkTime"] = obj2;
											j++;
											if (timeSpan2.TotalMinutes <= 0.0)
											{
												break;
											}
										}
										else
										{
											if (timeSpan2.TotalMinutes < 0.0 && Convert.ToInt32(this.dtValidCardRecord.Rows[j]["f_DutyOnOff"]) == 1)
											{
												break;
											}
											j++;
										}
									}
									else
									{
										j++;
									}
								}
							}
						}
					}
					num = 0;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				}
				finally
				{
					if (this.cn.State == ConnectionState.Open)
					{
						this.cn.Close();
					}
				}
				return num;
			}
			return num;
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x00252D98 File Offset: 0x00251D98
		public int shift_work_schedule_writetodb(DataTable dtShiftWorkSchedule)
		{
			this.cn = new OleDbConnection(wgAppConfig.dbConString);
			this.cmd = new OleDbCommand();
			string text = "";
			bool flag = true;
			this.errInfo = "";
			int num = -1;
			try
			{
				if (dtShiftWorkSchedule.Rows.Count > 0)
				{
					using (this.cmd = new OleDbCommand())
					{
						this.cmd.Connection = this.cn;
						this.cmd.CommandType = CommandType.Text;
						for (int i = 0; i <= dtShiftWorkSchedule.Rows.Count - 1; i++)
						{
							if (this.bStopCreate)
							{
								return num;
							}
							DataRow dataRow = dtShiftWorkSchedule.Rows[i];
							text = " INSERT INTO t_d_Shift_Work_Schedule ";
							text += " ( f_ConsumerID, f_shiftDate, f_ShiftID, f_ReadTimes, f_PlanTime, f_TimeSeg, f_WorkTime, f_AttDesc";
							text += " , f_CardRecordDesc, f_Duration, f_bOvertimeShift";
							text += " ) ";
							text = text + " Values ( " + dataRow["f_ConsumerID"];
							text = text + "," + this.PrepareStr(dataRow["f_shiftDate"], true, "yyyy-MM-dd");
							text = text + "," + dataRow["f_ShiftID"];
							text = text + "," + dataRow["f_ReadTimes"];
							text = text + "," + this.PrepareStr(dataRow["f_PlanTime"], true, "yyyy-MM-dd HH:mm:ss");
							text = text + "," + dataRow["f_TimeSeg"];
							text = text + "," + this.PrepareStr(dataRow["f_WorkTime"], true, "yyyy-MM-dd HH:mm:ss");
							text = text + "," + this.PrepareStr(dataRow["f_AttDesc"]);
							text = text + "," + this.PrepareStr(dataRow["f_CardRecordDesc"]);
							text = text + "," + dataRow["f_Duration"];
							text = text + "," + dataRow["f_bOvertimeShift"];
							text += ") ";
							if (this.cn.State == ConnectionState.Closed)
							{
								this.cn.Open();
							}
							this.cmd.CommandText = text;
							if (this.cmd.ExecuteNonQuery() <= 0)
							{
								this.errInfo = text;
								flag = false;
								break;
							}
						}
					}
				}
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
				if (flag)
				{
					num = 0;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString() + "\r\n" + text, new object[] { EventLogEntryType.Error });
			}
			finally
			{
				if (this.cn.State != ConnectionState.Closed)
				{
					this.cn.Close();
				}
			}
			return num;
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x002530C4 File Offset: 0x002520C4
		public int tm(object dt)
		{
			int num = 0;
			try
			{
				num = int.Parse(Strings.Format((DateTime)dt, "HHmmss"));
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x04003629 RID: 13865
		private const int EER_INVILID_SHIFTID = -6;

		// Token: 0x0400362A RID: 13866
		private const int EER_SQL_RUNFAIL = -999;

		// Token: 0x0400362B RID: 13867
		private const int EER_TIMEOVERLAPPED = -7;

		// Token: 0x0400362C RID: 13868
		private const int ERR_DAYDIFF = -5;

		// Token: 0x0400362D RID: 13869
		private const int ERR_FAIL = -1;

		// Token: 0x0400362E RID: 13870
		private const int ERR_ID = -2;

		// Token: 0x0400362F RID: 13871
		private const int ERR_NONE = 0;

		// Token: 0x04003630 RID: 13872
		private const int ERR_READTIMES = -3;

		// Token: 0x04003631 RID: 13873
		private const int ERR_TIMEDIFF = -4;

		// Token: 0x04003632 RID: 13874
		public bool bStopCreate;

		// Token: 0x04003633 RID: 13875
		private OleDbCommand cmd;

		// Token: 0x04003634 RID: 13876
		private OleDbConnection cn;

		// Token: 0x04003635 RID: 13877
		private Container components;

		// Token: 0x04003636 RID: 13878
		private OleDbDataAdapter daCardRecord;

		// Token: 0x04003637 RID: 13879
		private OleDbDataAdapter daHolidayType;

		// Token: 0x04003638 RID: 13880
		private OleDbDataAdapter daLeave;

		// Token: 0x04003639 RID: 13881
		private DataColumn dc;

		// Token: 0x0400363A RID: 13882
		private DataSet dsAtt;

		// Token: 0x0400363B RID: 13883
		private DataTable dtCardRecord;

		// Token: 0x0400363C RID: 13884
		private DataTable dtHolidayType;

		// Token: 0x0400363D RID: 13885
		private DataTable dtLeave;

		// Token: 0x0400363E RID: 13886
		private DataTable dtReport;

		// Token: 0x0400363F RID: 13887
		private DataTable dtReport1;

		// Token: 0x04003640 RID: 13888
		private DataTable dtShiftWork;

		// Token: 0x04003641 RID: 13889
		private DataTable dtValidCardRecord;

		// Token: 0x04003642 RID: 13890
		public string errInfo;

		// Token: 0x04003643 RID: 13891
		private int minShifDiffByMinute;

		// Token: 0x04003644 RID: 13892
		private DateTime realOffduty1;

		// Token: 0x04003645 RID: 13893
		private DateTime realOffduty2;

		// Token: 0x04003646 RID: 13894
		private DateTime realOffduty3;

		// Token: 0x04003647 RID: 13895
		private DateTime realOffduty4;

		// Token: 0x04003648 RID: 13896
		private DateTime realOnduty1;

		// Token: 0x04003649 RID: 13897
		private DateTime realOnduty2;

		// Token: 0x0400364A RID: 13898
		private DateTime realOnduty3;

		// Token: 0x0400364B RID: 13899
		private DateTime realOnduty4;

		// Token: 0x0400364C RID: 13900
		private string strTemp;

		// Token: 0x0400364D RID: 13901
		private int tAheadMinutes;

		// Token: 0x0400364E RID: 13902
		private int tAheadMinutesOnDutyFirst;

		// Token: 0x0400364F RID: 13903
		private int tDelayMinutes;

		// Token: 0x04003650 RID: 13904
		private decimal tLateAbsenceDay;

		// Token: 0x04003651 RID: 13905
		private int tLateTimeout;

		// Token: 0x04003652 RID: 13906
		private decimal tLeaveAbsenceDay;

		// Token: 0x04003653 RID: 13907
		private int tLeaveTimeout;

		// Token: 0x04003654 RID: 13908
		private int tOvertimeMinutes;

		// Token: 0x04003655 RID: 13909
		private int tOvertimeTimeout;

		// Token: 0x04003656 RID: 13910
		private int tTwoReadMintime;
	}
}
