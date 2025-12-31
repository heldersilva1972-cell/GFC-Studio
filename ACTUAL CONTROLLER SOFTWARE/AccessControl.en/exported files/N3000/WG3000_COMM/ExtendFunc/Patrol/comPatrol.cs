using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.VisualBasic;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000304 RID: 772
	public class comPatrol : Component
	{
		// Token: 0x06001707 RID: 5895 RVA: 0x001DE844 File Offset: 0x001DD844
		public comPatrol()
		{
			this.errInfo = "";
			this.tTwoReadMintime = 60;
			this.tPatrolEventDescNormal = 1;
			this.tPatrolEventDescEarly = 2;
			this.tPatrolEventDescLate = 3;
			this.tPatrolEventDescAbsence = 4;
			this.tNotPatrolTimeout = 30;
			this.tOnTimePatrolTimeout = 10;
			this.dsAtt = new DataSet();
			this.InitializeComponent();
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x001DE8A8 File Offset: 0x001DD8A8
		public comPatrol(IContainer container)
		{
			this.errInfo = "";
			this.tTwoReadMintime = 60;
			this.tPatrolEventDescNormal = 1;
			this.tPatrolEventDescEarly = 2;
			this.tPatrolEventDescLate = 3;
			this.tPatrolEventDescAbsence = 4;
			this.tNotPatrolTimeout = 30;
			this.tOnTimePatrolTimeout = 10;
			this.dsAtt = new DataSet();
			container.Add(this);
			this.InitializeComponent();
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x001DE912 File Offset: 0x001DD912
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x001DE934 File Offset: 0x001DD934
		public string errDesc(int errno)
		{
			if (errno == -999)
			{
				return CommonStr.strSqlRunFail;
			}
			if (errno != -1)
			{
				return CommonStr.strUnknown;
			}
			return CommonStr.strFailed;
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x001DE962 File Offset: 0x001DD962
		public void getPatrolParam()
		{
			this.tOnTimePatrolTimeout = (int)short.Parse(wgAppConfig.getSystemParamByNO(28));
			this.tNotPatrolTimeout = short.Parse(wgAppConfig.getSystemParamByNO(27));
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x001DE988 File Offset: 0x001DD988
		private void InitializeComponent()
		{
			this.components = new Container();
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x001DE998 File Offset: 0x001DD998
		public int logCreateReport(DateTime startDateTime, DateTime endDateTime, string groupName, string totalConsumer)
		{
			int num = -1;
			try
			{
				string text = string.Concat(new string[]
				{
					CommonStr.strPatrolCreateLog,
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
					"UPDATE t_a_SystemParam  SET [f_Value]=",
					wgTools.PrepareStrNUnicode(text4),
					" , [f_Notes] = ",
					wgTools.PrepareStrNUnicode(text3),
					" WHERE [f_NO]= 29 "
				}));
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x001DEB68 File Offset: 0x001DDB68
		private string PrepareStr(object obj)
		{
			return wgTools.PrepareStr(obj);
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x001DEB70 File Offset: 0x001DDB70
		private string PrepareStr(object obj, bool bDate, string dateFormat)
		{
			return wgTools.PrepareStr(obj, bDate, dateFormat);
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x001DEB7A File Offset: 0x001DDB7A
		private string SetObjToStr(object obj)
		{
			return wgTools.SetObjToStr(obj);
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x001DEB84 File Offset: 0x001DDB84
		public int shift_arrange_delete(int consumerId, DateTime dateStart, DateTime dateEnd)
		{
			this.cn = new SqlConnection(wgAppConfig.dbConString);
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
								text2 = " SELECT * FROM t_d_PatrolPlanData ";
								text2 = string.Concat(new object[]
								{
									text2,
									" WHERE f_ConsumerID = ",
									consumerId,
									" AND f_DateYM = ",
									wgTools.PrepareStrNUnicode(text)
								});
								if (this.cn.State != ConnectionState.Open)
								{
									this.cn.Open();
								}
								this.cmd = new SqlCommand(text2, this.cn);
								SqlDataReader sqlDataReader = this.cmd.ExecuteReader();
								if (sqlDataReader.Read())
								{
									flag = true;
									for (int i = 0; i <= sqlDataReader.FieldCount - 1; i++)
									{
										array[i] = sqlDataReader[i];
									}
								}
								else
								{
									flag = false;
								}
								sqlDataReader.Close();
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
									text2 = "  DELETE FROM t_d_PatrolPlanData ";
									text2 = text2 + " WHERE f_RecID = " + array[0];
								}
								else
								{
									text2 = "  UPDATE t_d_PatrolPlanData SET ";
									int j = 1;
									object obj = text2;
									text2 = string.Concat(new object[]
									{
										obj,
										" f_RouteID_",
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
											" , f_RouteID_",
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
								this.cmd = new SqlCommand(text2, this.cn);
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
			text2 = " DELETE FROM t_d_PatrolPlanData ";
			if (this.cn.State != ConnectionState.Open)
			{
				this.cn.Open();
			}
			this.cmd = new SqlCommand(text2, this.cn);
			if (this.cmd.ExecuteNonQuery() < 0)
			{
				num = -999;
				this.errInfo = text2;
				return num;
			}
			return 0;
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x001DEFAC File Offset: 0x001DDFAC
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

		// Token: 0x06001713 RID: 5907 RVA: 0x001DF004 File Offset: 0x001DE004
		public int shift_arrangeByRule(int consumerId, DateTime dateStart, DateTime dateEnd, int ruleLen, int[] shiftRule)
		{
			this.cn = new SqlConnection(wgAppConfig.dbConString);
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
							text2 = " SELECT * FROM t_d_PatrolPlanData ";
							text2 = string.Concat(new object[]
							{
								text2,
								" WHERE f_ConsumerID = ",
								consumerId,
								" AND f_DateYM = ",
								wgTools.PrepareStrNUnicode(text)
							});
							if (this.cn.State != ConnectionState.Open)
							{
								this.cn.Open();
							}
							this.cmd = new SqlCommand(text2, this.cn);
							SqlDataReader sqlDataReader = this.cmd.ExecuteReader();
							if (sqlDataReader.Read())
							{
								flag = true;
								for (int i = 0; i <= sqlDataReader.FieldCount - 1; i++)
								{
									array[i] = sqlDataReader[i];
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
							sqlDataReader.Close();
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
							text2 = "  UPDATE t_d_PatrolPlanData SET ";
							int k = 1;
							object obj = text2;
							text2 = string.Concat(new object[]
							{
								obj,
								" f_RouteID_",
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
									" , f_RouteID_",
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
							text2 = "  INSERT INTO t_d_PatrolPlanData  ";
							text2 += " ( f_ConsumerID , f_DateYM  ";
							for (int l = 1; l <= 31; l++)
							{
								text2 = text2 + " , f_RouteID_" + l.ToString().PadLeft(2, '0');
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
						this.cmd = new SqlCommand(text2, this.cn);
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

		// Token: 0x06001714 RID: 5908 RVA: 0x001DF4D0 File Offset: 0x001DE4D0
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
				this.dc.ColumnName = "f_PatrolDateStart";
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_PatrolDateEnd";
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_TotalLate";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_LateMinutes";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_TotalEarly";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Decimal");
				this.dc.ColumnName = "f_TotalAbsence";
				this.dc.DefaultValue = 0;
				this.dtReport.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_TotalNormal";
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

		// Token: 0x06001715 RID: 5909 RVA: 0x001DF7C4 File Offset: 0x001DE7C4
		public int shift_AttReport_Fill(DataTable dtAttReport, DataTable dtShiftWorkSchedule)
		{
			this.errInfo = "";
			int num = -1;
			try
			{
				DataRow dataRow = null;
				int num2 = -1;
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
					dataRow["f_ConsumerID"] = Convert.ToInt32(dataRow2["f_ConsumerID"]);
					if (this.SetObjToStr(dataRow2["f_EventDesc"]) == this.tPatrolEventDescEarly.ToString())
					{
						dataRow["f_TotalEarly"] = Convert.ToInt32(dataRow["f_TotalEarly"]) + 1;
					}
					else if (this.SetObjToStr(dataRow2["f_EventDesc"]) == this.tPatrolEventDescLate.ToString())
					{
						dataRow["f_TotalLate"] = Convert.ToInt32(dataRow["f_TotalLate"]) + 1;
					}
					else if (this.SetObjToStr(dataRow2["f_EventDesc"]) == this.tPatrolEventDescAbsence.ToString())
					{
						dataRow["f_TotalAbsence"] = Convert.ToInt32(dataRow["f_TotalAbsence"]) + 1;
					}
					else if (this.SetObjToStr(dataRow2["f_EventDesc"]) == this.tPatrolEventDescNormal.ToString())
					{
						dataRow["f_TotalNormal"] = Convert.ToInt32(dataRow["f_TotalNormal"]) + 1;
					}
					if (num2 < 0)
					{
						num2 = (int)dataRow2["f_ConsumerID"];
					}
					if (num2 != (int)dataRow2["f_ConsumerID"])
					{
						dtAttReport.Rows.Add(dataRow);
						dataRow = dtAttReport.NewRow();
						num2 = (int)dataRow2["f_ConsumerID"];
					}
				}
				if (num2 > 0)
				{
					dtAttReport.Rows.Add(dataRow);
					dataRow = dtAttReport.NewRow();
				}
				num = 0;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return num;
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x001DFA1C File Offset: 0x001DEA1C
		public int shift_AttReport_writetodb(DataTable dtAttReport)
		{
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			string text = "";
			this.cmd = new SqlCommand();
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
						text = " INSERT INTO t_d_PatrolStatistic ";
						text += " ( f_ConsumerID, f_PatrolDateStart, f_PatrolDateEnd ";
						text += " , f_TotalLate, f_TotalEarly, f_TotalAbsence, f_TotalNormal ";
						text += " ) ";
						text = text + " Values ( " + dataRow["f_ConsumerID"];
						text = text + "," + this.PrepareStr(dataRow["f_PatrolDateStart"], true, "yyyy-MM-dd");
						text = text + "," + this.PrepareStr(dataRow["f_PatrolDateEnd"], true, "yyyy-MM-dd");
						text = text + "," + dataRow["f_TotalLate"];
						text = text + "," + dataRow["f_TotalEarly"];
						text = text + "," + dataRow["f_TotalAbsence"];
						text = text + "," + dataRow["f_TotalNormal"];
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

		// Token: 0x06001717 RID: 5911 RVA: 0x001DFC98 File Offset: 0x001DEC98
		public int shift_AttStatistic_cleardb()
		{
			this.errInfo = "";
			int num = -1;
			try
			{
				if (wgAppConfig.runUpdateSql("Delete From t_d_PatrolStatistic") >= 0)
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

		// Token: 0x06001718 RID: 5912 RVA: 0x001DFCF4 File Offset: 0x001DECF4
		public int shift_work_schedule_analyst(DataTable dtShiftWorkSchedule)
		{
			this.getPatrolParam();
			this.errInfo = "";
			int num = -1;
			try
			{
				for (int i = 0; i <= dtShiftWorkSchedule.Rows.Count - 1; i++)
				{
					if (this.bStopCreate)
					{
						return num;
					}
					object obj = dtShiftWorkSchedule.Rows[i]["f_PlanPatrolTime"];
					if (!Information.IsDBNull(obj))
					{
						if (Information.IsDBNull(dtShiftWorkSchedule.Rows[i]["f_RealPatrolTime"]))
						{
							dtShiftWorkSchedule.Rows[i]["f_EventDesc"] = this.tPatrolEventDescAbsence;
						}
						else
						{
							object obj2 = dtShiftWorkSchedule.Rows[i]["f_RealPatrolTime"];
							TimeSpan timeSpan = Convert.ToDateTime(obj).Subtract(Convert.ToDateTime(obj2));
							if (Math.Abs(timeSpan.TotalMinutes) <= (double)this.tOnTimePatrolTimeout)
							{
								dtShiftWorkSchedule.Rows[i]["f_EventDesc"] = this.tPatrolEventDescNormal;
							}
							else if (timeSpan.TotalMinutes > (double)this.tOnTimePatrolTimeout)
							{
								dtShiftWorkSchedule.Rows[i]["f_EventDesc"] = this.tPatrolEventDescEarly;
							}
							else if (timeSpan.TotalMinutes < (double)(-(double)this.tOnTimePatrolTimeout))
							{
								dtShiftWorkSchedule.Rows[i]["f_EventDesc"] = this.tPatrolEventDescLate;
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

		// Token: 0x06001719 RID: 5913 RVA: 0x001DFEBC File Offset: 0x001DEEBC
		public int shift_work_schedule_cleardb()
		{
			this.errInfo = "";
			int num = -1;
			try
			{
				if (wgAppConfig.runUpdateSql("Delete From t_d_PatrolDetailData") >= 0)
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

		// Token: 0x0600171A RID: 5914 RVA: 0x001DFF18 File Offset: 0x001DEF18
		public int shift_work_schedule_create(out DataTable dtShiftWorkSchedule)
		{
			this.dtShiftWork = new DataTable("t_d_PatrolPlanWork");
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
				this.dc.ColumnName = "f_PatrolDate";
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_RouteID";
				this.dc.DefaultValue = -1;
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_ReaderID";
				this.dc.DefaultValue = 0;
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_PlanPatrolTime";
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.DateTime");
				this.dc.ColumnName = "f_RealPatrolTime";
				this.dtShiftWork.Columns.Add(this.dc);
				this.dc = new DataColumn();
				this.dc.DataType = Type.GetType("System.Int32");
				this.dc.ColumnName = "f_EventDesc";
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

		// Token: 0x0600171B RID: 5915 RVA: 0x001E01A4 File Offset: 0x001DF1A4
		public int shift_work_schedule_fill(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd, ref int bNotArranged)
		{
			this.cn = new SqlConnection(wgAppConfig.dbConString);
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
							string text2 = " SELECT * FROM t_d_PatrolPlanData ";
							text2 = string.Concat(new object[]
							{
								text2,
								" WHERE f_ConsumerID = ",
								consumerid,
								" AND f_DateYM = ",
								wgTools.PrepareStrNUnicode(text)
							});
							if (this.cn.State != ConnectionState.Open)
							{
								this.cn.Open();
							}
							this.cmd = new SqlCommand(text2, this.cn);
							SqlDataReader sqlDataReader = this.cmd.ExecuteReader();
							if (sqlDataReader.Read())
							{
								flag = true;
								for (int i = 0; i <= sqlDataReader.FieldCount - 1; i++)
								{
									array[i] = sqlDataReader[i];
								}
							}
							else
							{
								bNotArranged |= 1;
								flag = false;
								array[0] = -1;
							}
							sqlDataReader.Close();
						}
						do
						{
							if (flag)
							{
								int num2 = Convert.ToInt32(array[2 + dateTime3.Day]);
								if (num2 > 0)
								{
									string text2 = "SELECT * FROM t_d_PatrolRouteDetail WHERE f_RouteID = " + num2 + " ORDER BY f_Sn ";
									this.cmd = new SqlCommand(text2, this.cn);
									if (this.cn.State != ConnectionState.Open)
									{
										this.cn.Open();
									}
									SqlDataReader sqlDataReader = this.cmd.ExecuteReader();
									while (sqlDataReader.Read())
									{
										DataRow dataRow = dtShiftWorkSchedule.NewRow();
										dataRow[0] = consumerid;
										dataRow[1] = dateTime3;
										dataRow["f_RouteID"] = sqlDataReader["f_RouteID"];
										dataRow["f_ReaderID"] = sqlDataReader["f_ReaderID"];
										if ((int)sqlDataReader["f_NextDay"] > 0)
										{
											dataRow["f_PlanPatrolTime"] = Convert.ToDateTime(string.Concat(new object[]
											{
												Strings.Format(dateTime3.AddDays(1.0), "yyyy-MM-dd"),
												" ",
												sqlDataReader["f_patroltime"],
												":00"
											}));
										}
										else
										{
											dataRow["f_PlanPatrolTime"] = Convert.ToDateTime(string.Concat(new object[]
											{
												Strings.Format(dateTime3, "yyyy-MM-dd"),
												" ",
												sqlDataReader["f_patroltime"],
												":00"
											}));
										}
										dtShiftWorkSchedule.Rows.Add(dataRow);
									}
									sqlDataReader.Close();
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

		// Token: 0x0600171C RID: 5916 RVA: 0x001E05A0 File Offset: 0x001DF5A0
		public int shift_work_schedule_updatebyReadcard(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd)
		{
			this.getPatrolParam();
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			this.dsAtt = new DataSet();
			this.errInfo = "";
			int num = -1;
			DateTime dateTime = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
			DateTime dateTime2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
			if (dateTime <= dateTime2)
			{
				try
				{
					string text = "SELECT t_d_SwipeRecord.f_ConsumerID, t_d_SwipeRecord.f_ReaderID, t_d_SwipeRecord.f_ReadDate, 0 as f_used  FROM t_d_SwipeRecord, t_b_Reader,t_b_Reader4Patrol ";
					text = string.Concat(new object[]
					{
						text,
						" , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  f_ConsumerID = ",
						consumerid,
						" AND ([f_ReadDate]>= ",
						this.PrepareStr(dateStart, true, "yyyy-MM-dd 00:00:00"),
						")  AND ([f_ReadDate]<= ",
						this.PrepareStr(dateEnd.AddDays(1.0), true, "yyyy-MM-dd 23:59:59"),
						")  AND (t_d_SwipeRecord.f_ReaderID = t_b_Reader.f_ReaderID)  AND t_b_Reader.f_ReaderID = t_b_Reader4Patrol.f_ReaderID  ORDER BY f_ReadDate ASC, f_RecID ASC "
					});
					this.cmd = new SqlCommand();
					this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
					this.cmd.Connection = this.cn;
					this.cmd.CommandText = text;
					this.cmd.CommandType = CommandType.Text;
					this.daCardRecord = new SqlDataAdapter(this.cmd);
					this.daCardRecord.Fill(this.dsAtt, "CardRecord");
					this.dtCardRecord = this.dsAtt.Tables["CardRecord"];
					this.daCardRecord = new SqlDataAdapter("SELECT t_d_SwipeRecord.f_ConsumerID, t_d_SwipeRecord.f_ReaderID, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_DutyOnOff,0 as f_used FROM t_d_SwipeRecord, t_b_Reader , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  1<0  ", this.cn);
					this.daCardRecord.Fill(this.dsAtt, "ValidCardRecord");
					this.dtValidCardRecord = this.dsAtt.Tables["ValidCardRecord"];
					if (this.dtCardRecord.Rows.Count > 0)
					{
						object[] array = new object[this.dtCardRecord.Columns.Count - 1 + 1];
						DateTime dateTime3 = Convert.ToDateTime(this.dtCardRecord.Rows[0]["f_ReadDate"]);
						int num2 = Convert.ToInt32(this.dtCardRecord.Rows[0]["f_ReaderID"]);
						DataRow dataRow = this.dtValidCardRecord.NewRow();
						this.dtCardRecord.Rows[0].ItemArray.CopyTo(array, 0);
						dataRow.ItemArray = array;
						this.dtValidCardRecord.Rows.Add(dataRow);
						dateTime3 = Convert.ToDateTime(dataRow["f_ReadDate"]);
						num2 = Convert.ToInt32(dataRow["f_ReaderID"]);
						for (int i = 1; i <= this.dtCardRecord.Rows.Count - 1; i++)
						{
							if (this.bStopCreate)
							{
								return num;
							}
							DateTime dateTime4 = Convert.ToDateTime(this.dtCardRecord.Rows[i]["f_ReadDate"]);
							TimeSpan timeSpan = dateTime4.Subtract(dateTime3);
							int num3 = Convert.ToInt32(this.dtCardRecord.Rows[i]["f_ReaderID"]);
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
						object obj = dtShiftWorkSchedule.Rows[k]["f_PlanPatrolTime"];
						if (!Information.IsDBNull(obj))
						{
							bool flag = false;
							int num4 = j;
							while (j < this.dtValidCardRecord.Rows.Count)
							{
								object obj2 = this.dtValidCardRecord.Rows[j]["f_ReadDate"];
								TimeSpan timeSpan2 = Convert.ToDateTime(obj).Subtract(Convert.ToDateTime(obj2));
								if (timeSpan2.TotalMinutes > (double)this.tNotPatrolTimeout)
								{
									j++;
									num4 = j;
								}
								else
								{
									if (timeSpan2.TotalMinutes < (double)(-(double)this.tNotPatrolTimeout))
									{
										break;
									}
									if (wgTools.SetObjToStr(this.dtValidCardRecord.Rows[j]["f_used"]) != "1" && (int)dtShiftWorkSchedule.Rows[k]["f_ReaderID"] == (int)this.dtValidCardRecord.Rows[j]["f_ReaderID"])
									{
										dtShiftWorkSchedule.Rows[k]["f_RealPatrolTime"] = obj2;
										flag = true;
										this.dtValidCardRecord.Rows[j]["f_used"] = 1;
										break;
									}
									j++;
								}
							}
							if (!flag)
							{
								j = num4;
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

		// Token: 0x0600171D RID: 5917 RVA: 0x001E0B54 File Offset: 0x001DFB54
		public int shift_work_schedule_writetodb(DataTable dtShiftWorkSchedule)
		{
			this.cn = new SqlConnection(wgAppConfig.dbConString);
			this.cmd = new SqlCommand();
			string text = "";
			bool flag = true;
			this.errInfo = "";
			int num = -1;
			try
			{
				if (dtShiftWorkSchedule.Rows.Count > 0)
				{
					using (this.cmd = new SqlCommand())
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
							text = " INSERT INTO t_d_PatrolDetailData ";
							text += " ( f_ConsumerID, f_PatrolDate, f_RouteID, f_ReaderID, f_PlanPatrolTime, f_RealPatrolTime, f_EventDesc";
							text += " ) ";
							text = text + " Values ( " + dataRow["f_ConsumerID"];
							text = text + "," + this.PrepareStr(dataRow["f_PatrolDate"], true, "yyyy-MM-dd");
							text = text + "," + dataRow["f_RouteID"];
							text = text + "," + dataRow["f_ReaderID"];
							text = text + "," + this.PrepareStr(dataRow["f_PlanPatrolTime"], true, "yyyy-MM-dd HH:mm:ss");
							text = text + "," + this.PrepareStr(dataRow["f_RealPatrolTime"], true, "yyyy-MM-dd HH:mm:ss");
							text = text + "," + wgTools.PrepareStrNUnicode(dataRow["f_EventDesc"]);
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

		// Token: 0x0600171E RID: 5918 RVA: 0x001E0E0C File Offset: 0x001DFE0C
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

		// Token: 0x04002FC8 RID: 12232
		private const int EER_SQL_RUNFAIL = -999;

		// Token: 0x04002FC9 RID: 12233
		private const int ERR_FAIL = -1;

		// Token: 0x04002FCA RID: 12234
		private const int ERR_NONE = 0;

		// Token: 0x04002FCB RID: 12235
		public bool bStopCreate;

		// Token: 0x04002FCC RID: 12236
		private SqlCommand cmd;

		// Token: 0x04002FCD RID: 12237
		private SqlConnection cn;

		// Token: 0x04002FCE RID: 12238
		private IContainer components;

		// Token: 0x04002FCF RID: 12239
		private SqlDataAdapter daCardRecord;

		// Token: 0x04002FD0 RID: 12240
		private DataColumn dc;

		// Token: 0x04002FD1 RID: 12241
		private DataSet dsAtt;

		// Token: 0x04002FD2 RID: 12242
		private DataTable dtCardRecord;

		// Token: 0x04002FD3 RID: 12243
		private DataTable dtReport;

		// Token: 0x04002FD4 RID: 12244
		private DataTable dtShiftWork;

		// Token: 0x04002FD5 RID: 12245
		private DataTable dtValidCardRecord;

		// Token: 0x04002FD6 RID: 12246
		public string errInfo;

		// Token: 0x04002FD7 RID: 12247
		private short tNotPatrolTimeout;

		// Token: 0x04002FD8 RID: 12248
		private int tOnTimePatrolTimeout;

		// Token: 0x04002FD9 RID: 12249
		private int tPatrolEventDescAbsence;

		// Token: 0x04002FDA RID: 12250
		private int tPatrolEventDescEarly;

		// Token: 0x04002FDB RID: 12251
		private int tPatrolEventDescLate;

		// Token: 0x04002FDC RID: 12252
		private int tPatrolEventDescNormal;

		// Token: 0x04002FDD RID: 12253
		private int tTwoReadMintime;
	}
}
