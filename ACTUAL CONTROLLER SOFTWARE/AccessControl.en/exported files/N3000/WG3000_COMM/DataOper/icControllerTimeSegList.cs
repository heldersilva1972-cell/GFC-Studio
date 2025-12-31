using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using WG3000_COMM.Core;

namespace WG3000_COMM.DataOper
{
	// Token: 0x02000221 RID: 545
	public class icControllerTimeSegList : wgMjControllerTimeSegList
	{
		// Token: 0x06000FC1 RID: 4033 RVA: 0x0011AC68 File Offset: 0x00119C68
		public icControllerTimeSegList()
		{
			base.Clear();
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0011AC78 File Offset: 0x00119C78
		public void fillByDB()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.fillByDB_Acc();
				return;
			}
			base.Clear();
			string text = " SELECT * FROM t_b_ControlSeg  ";
			bool paramValBoolByNO = wgAppConfig.getParamValBoolByNO(136);
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					while (sqlDataReader.Read())
					{
						if ((int)sqlDataReader["f_ControlSegID"] <= 254)
						{
							MjControlTimeSeg mjControlTimeSeg = new MjControlTimeSeg();
							mjControlTimeSeg.SegIndex = (byte)((int)sqlDataReader["f_ControlSegID"]);
							mjControlTimeSeg.weekdayControl = 0;
							mjControlTimeSeg.weekdayControl += ((sqlDataReader["f_Monday"].ToString() == "1") ? 1 : 0);
							mjControlTimeSeg.weekdayControl += ((sqlDataReader["f_Tuesday"].ToString() == "1") ? 2 : 0);
							mjControlTimeSeg.weekdayControl += ((sqlDataReader["f_Wednesday"].ToString() == "1") ? 4 : 0);
							mjControlTimeSeg.weekdayControl += ((sqlDataReader["f_Thursday"].ToString() == "1") ? 8 : 0);
							mjControlTimeSeg.weekdayControl += ((sqlDataReader["f_Friday"].ToString() == "1") ? 16 : 0);
							mjControlTimeSeg.weekdayControl += ((sqlDataReader["f_Saturday"].ToString() == "1") ? 32 : 0);
							mjControlTimeSeg.weekdayControl += ((sqlDataReader["f_Sunday"].ToString() == "1") ? 64 : 0);
							mjControlTimeSeg.hmsStart1 = wgTools.wgDateTimeParse(sqlDataReader["f_BeginHMS1"]);
							mjControlTimeSeg.hmsStart2 = wgTools.wgDateTimeParse(sqlDataReader["f_BeginHMS2"]);
							mjControlTimeSeg.hmsStart3 = wgTools.wgDateTimeParse(sqlDataReader["f_BeginHMS3"]);
							mjControlTimeSeg.hmsEnd1 = wgTools.wgDateTimeParse(sqlDataReader["f_EndHMS1"]);
							mjControlTimeSeg.hmsEnd2 = wgTools.wgDateTimeParse(sqlDataReader["f_EndHMS2"]);
							mjControlTimeSeg.hmsEnd3 = wgTools.wgDateTimeParse(sqlDataReader["f_EndHMS3"]);
							mjControlTimeSeg.ymdStart = wgTools.wgDateTimeParse(sqlDataReader["f_BeginYMD"]);
							mjControlTimeSeg.ymdEnd = wgTools.wgDateTimeParse(sqlDataReader["f_EndYMD"]);
							if (paramValBoolByNO)
							{
								mjControlTimeSeg.LimittedMode = int.Parse(sqlDataReader["f_ReaderCount"].ToString());
								mjControlTimeSeg.TotalLimittedAccess = (int)sqlDataReader["f_LimitedTimesOfDay"] & 255;
								mjControlTimeSeg.MonthLimittedAccess = ((int)sqlDataReader["f_LimitedTimesOfDay"] >> 8) & 255;
								mjControlTimeSeg.LimittedAccess1 = (int)sqlDataReader["f_LimitedTimesOfHMS1"];
								mjControlTimeSeg.LimittedAccess2 = (int)sqlDataReader["f_LimitedTimesOfHMS2"];
								mjControlTimeSeg.LimittedAccess3 = (int)sqlDataReader["f_LimitedTimesOfHMS3"];
							}
							mjControlTimeSeg.nextSeg = (byte)((int)sqlDataReader["f_ControlSegIDLinked"]);
							mjControlTimeSeg.ControlByHoliday = (byte)((int)sqlDataReader["f_ControlByHoliday"]);
							if (base.AddItem(mjControlTimeSeg) != 1)
							{
								break;
							}
						}
					}
					sqlDataReader.Close();
				}
			}
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0011B074 File Offset: 0x0011A074
		public void fillByDB_Acc()
		{
			base.Clear();
			string text = " SELECT * FROM t_b_ControlSeg  ";
			bool paramValBoolByNO = wgAppConfig.getParamValBoolByNO(136);
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					while (oleDbDataReader.Read())
					{
						if ((int)oleDbDataReader["f_ControlSegID"] <= 254)
						{
							MjControlTimeSeg mjControlTimeSeg = new MjControlTimeSeg();
							mjControlTimeSeg.SegIndex = (byte)((int)oleDbDataReader["f_ControlSegID"]);
							mjControlTimeSeg.weekdayControl = 0;
							mjControlTimeSeg.weekdayControl += ((oleDbDataReader["f_Monday"].ToString() == "1") ? 1 : 0);
							mjControlTimeSeg.weekdayControl += ((oleDbDataReader["f_Tuesday"].ToString() == "1") ? 2 : 0);
							mjControlTimeSeg.weekdayControl += ((oleDbDataReader["f_Wednesday"].ToString() == "1") ? 4 : 0);
							mjControlTimeSeg.weekdayControl += ((oleDbDataReader["f_Thursday"].ToString() == "1") ? 8 : 0);
							mjControlTimeSeg.weekdayControl += ((oleDbDataReader["f_Friday"].ToString() == "1") ? 16 : 0);
							mjControlTimeSeg.weekdayControl += ((oleDbDataReader["f_Saturday"].ToString() == "1") ? 32 : 0);
							mjControlTimeSeg.weekdayControl += ((oleDbDataReader["f_Sunday"].ToString() == "1") ? 64 : 0);
							mjControlTimeSeg.hmsStart1 = wgTools.wgDateTimeParse(oleDbDataReader["f_BeginHMS1"]);
							mjControlTimeSeg.hmsStart2 = wgTools.wgDateTimeParse(oleDbDataReader["f_BeginHMS2"]);
							mjControlTimeSeg.hmsStart3 = wgTools.wgDateTimeParse(oleDbDataReader["f_BeginHMS3"]);
							mjControlTimeSeg.hmsEnd1 = wgTools.wgDateTimeParse(oleDbDataReader["f_EndHMS1"]);
							mjControlTimeSeg.hmsEnd2 = wgTools.wgDateTimeParse(oleDbDataReader["f_EndHMS2"]);
							mjControlTimeSeg.hmsEnd3 = wgTools.wgDateTimeParse(oleDbDataReader["f_EndHMS3"]);
							mjControlTimeSeg.ymdStart = wgTools.wgDateTimeParse(oleDbDataReader["f_BeginYMD"]);
							mjControlTimeSeg.ymdEnd = wgTools.wgDateTimeParse(oleDbDataReader["f_EndYMD"]);
							if (paramValBoolByNO)
							{
								mjControlTimeSeg.LimittedMode = int.Parse(oleDbDataReader["f_ReaderCount"].ToString());
								mjControlTimeSeg.TotalLimittedAccess = (int)oleDbDataReader["f_LimitedTimesOfDay"] & 255;
								mjControlTimeSeg.MonthLimittedAccess = ((int)oleDbDataReader["f_LimitedTimesOfDay"] >> 8) & 255;
								mjControlTimeSeg.LimittedAccess1 = (int)oleDbDataReader["f_LimitedTimesOfHMS1"];
								mjControlTimeSeg.LimittedAccess2 = (int)oleDbDataReader["f_LimitedTimesOfHMS2"];
								mjControlTimeSeg.LimittedAccess3 = (int)oleDbDataReader["f_LimitedTimesOfHMS3"];
							}
							mjControlTimeSeg.nextSeg = (byte)((int)oleDbDataReader["f_ControlSegIDLinked"]);
							mjControlTimeSeg.ControlByHoliday = (byte)((int)oleDbDataReader["f_ControlByHoliday"]);
							if (base.AddItem(mjControlTimeSeg) != 1)
							{
								break;
							}
						}
					}
					oleDbDataReader.Close();
				}
			}
		}
	}
}
