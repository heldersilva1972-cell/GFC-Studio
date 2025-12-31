using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Threading;
using WG3000_COMM.Core;

namespace WG3000_COMM.DataOper
{
	// Token: 0x0200021E RID: 542
	internal class icConsumerShare
	{
		// Token: 0x06000F6E RID: 3950 RVA: 0x00113C1E File Offset: 0x00112C1E
		public static DataTable getDt()
		{
			return icConsumerShare.dtLastLoad;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00113C25 File Offset: 0x00112C25
		public static string getOptionalRowfilter()
		{
			return string.Format("( f_Selected <={0:d} )", icConsumerShare.m_iSelectedCurrentNoneMax);
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00113C3B File Offset: 0x00112C3B
		public static string getOptionalRowfilter(bool bFilterNotAttend)
		{
			if (bFilterNotAttend)
			{
				return string.Format("( f_Selected <={0:d} And  f_AttendEnabled > 0 )", icConsumerShare.m_iSelectedCurrentNoneMax);
			}
			return string.Format("( f_Selected <={0:d} )", icConsumerShare.m_iSelectedCurrentNoneMax);
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00113C69 File Offset: 0x00112C69
		public static string getSelectedRowfilter()
		{
			return string.Format(" ( f_Selected >{0:d} )", icConsumerShare.m_iSelectedCurrentNoneMax);
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00113C7F File Offset: 0x00112C7F
		public static string getSelectedRowfilter(bool bFilterNotAttend)
		{
			if (bFilterNotAttend)
			{
				return string.Format(" ( f_Selected >{0:d} And  f_AttendEnabled > 0 )", icConsumerShare.m_iSelectedCurrentNoneMax);
			}
			return string.Format(" ( f_Selected >{0:d} )", icConsumerShare.m_iSelectedCurrentNoneMax);
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00113CAD File Offset: 0x00112CAD
		public static int getSelectedValue()
		{
			return icConsumerShare.m_iSelectedCurrentNoneMax + 1;
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00113CB6 File Offset: 0x00112CB6
		public static string getUpdateLog()
		{
			return wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(50));
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00113CC4 File Offset: 0x00112CC4
		public static void loadUserData()
		{
			wgTools.WriteLine("loadUserData Start");
			Thread.Sleep(100);
			if (!string.IsNullOrEmpty(icConsumerShare.lastLoadUsers) && icConsumerShare.lastLoadUsers == icConsumerShare.getUpdateLog() && icConsumerShare.dtLastLoad != null && icConsumerShare.iSelectedMax + 1000 < 2147483647)
			{
				icConsumerShare.selectNoneUsers();
				icConsumerShare.dtLastLoad.AcceptChanges();
				wgTools.WriteLine("return dtLastLoad");
				return;
			}
			icConsumerShare.iSelectedMin = 1879048192;
			icConsumerShare.iSelectedMax = 1879048192;
			icConsumerShare.m_iSelectedCurrentNoneMax = 1879048192;
			string text = string.Format(" SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, {0:d} as f_Selected, f_GroupID, f_DoorEnabled, f_AttendEnabled, 0 as f_other  ", icConsumerShare.iSelectedMin) + " FROM t_b_Consumer  ORDER BY f_ConsumerNO ASC ";
			icConsumerShare.dtUser = new DataTable();
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							oleDbDataAdapter.Fill(icConsumerShare.dtUser);
						}
					}
					goto IL_0166;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						sqlDataAdapter.Fill(icConsumerShare.dtUser);
					}
				}
			}
			IL_0166:
			wgTools.WriteLine("da.Fill End");
			try
			{
				icConsumerShare.dtUser.PrimaryKey = new DataColumn[] { icConsumerShare.dtUser.Columns[0] };
			}
			catch (Exception)
			{
				throw;
			}
			icConsumerShare.lastLoadUsers = icConsumerShare.getUpdateLog();
			icConsumerShare.dtLastLoad = icConsumerShare.dtUser;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00113EDC File Offset: 0x00112EDC
		public static void selectAllUsers()
		{
			icConsumerShare.iSelectedMin--;
			icConsumerShare.m_iSelectedCurrentNoneMax = icConsumerShare.iSelectedMin;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00113EF4 File Offset: 0x00112EF4
		public static void selectNoneUsers()
		{
			icConsumerShare.iSelectedMax++;
			icConsumerShare.m_iSelectedCurrentNoneMax = icConsumerShare.iSelectedMax;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00113F0C File Offset: 0x00112F0C
		public static void setUpdateLog()
		{
			wgAppConfig.setSystemParamValue(50, null, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), null);
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x00113F35 File Offset: 0x00112F35
		public static int iSelectedCurrentNoneMax
		{
			get
			{
				return icConsumerShare.m_iSelectedCurrentNoneMax;
			}
		}

		// Token: 0x04001C5A RID: 7258
		private static DataTable dtLastLoad;

		// Token: 0x04001C5B RID: 7259
		private static DataTable dtUser = null;

		// Token: 0x04001C5C RID: 7260
		private static int iSelectedMax = 1879048192;

		// Token: 0x04001C5D RID: 7261
		private static int iSelectedMin = 1879048192;

		// Token: 0x04001C5E RID: 7262
		private static string lastLoadUsers = "";

		// Token: 0x04001C5F RID: 7263
		private static int m_iSelectedCurrentNoneMax = 1879048192;
	}
}
