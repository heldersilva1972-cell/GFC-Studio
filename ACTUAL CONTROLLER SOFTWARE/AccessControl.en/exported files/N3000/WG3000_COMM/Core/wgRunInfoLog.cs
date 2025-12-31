using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Core
{
	// Token: 0x02000212 RID: 530
	public class wgRunInfoLog
	{
		// Token: 0x06000E9C RID: 3740 RVA: 0x001090C8 File Offset: 0x001080C8
		public static void addEvent(InfoRow newInfo)
		{
			if (wgRunInfoLog.m_dt != null)
			{
				wgRunInfoLog.m_dt.AcceptChanges();
				lock (wgRunInfoLog.m_dt)
				{
					DataRow dataRow = wgRunInfoLog.m_dt.NewRow();
					dataRow[0] = newInfo.category;
					wgRunInfoLog.eventRecID++;
					dataRow[1] = wgRunInfoLog.eventRecID.ToString();
					dataRow[2] = DateTime.Now.ToString("HH:mm:ss");
					dataRow[3] = newInfo.desc;
					dataRow[4] = newInfo.information;
					dataRow[5] = newInfo.detail;
					dataRow[6] = newInfo.MjRecStr;
					object[] array = new object[]
					{
						(wgRunInfoLog.m_dt.Rows.Count + 1).ToString(),
						newInfo.desc,
						newInfo.information,
						newInfo.detail,
						newInfo.MjRecStr
					};
					wgAppConfig.wgLog(string.Format("{0},{1},{2},{3},{4}", array));
					wgRunInfoLog.m_dt.Rows.Add(dataRow);
					wgRunInfoLog.m_dt.AcceptChanges();
				}
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0010921C File Offset: 0x0010821C
		public static void addEventNotConnect(int ControllerSN, string IP, ListViewItem itm)
		{
			if (itm != null)
			{
				wgRunInfoLog.addEvent(new InfoRow
				{
					category = 101,
					desc = itm.Text,
					information = string.Format("{0}--{1}:{2:d}--IP:{3}", new object[]
					{
						CommonStr.strCommFail,
						CommonStr.strControllerSN,
						ControllerSN,
						IP
					}),
					detail = string.Format("{0}\r\n{1}\r\n{2}:\t{3:d}\r\nIP:\t{4}\r\n", new object[]
					{
						itm.Text,
						CommonStr.strCommFail,
						CommonStr.strControllerSN,
						ControllerSN,
						IP
					})
				});
				itm.ImageIndex = 3;
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x001092CC File Offset: 0x001082CC
		public static void addEventSpecial1(InfoRow newInfo, bool bWriteToDB)
		{
			if (wgRunInfoLog.m_dt != null)
			{
				DataRow dataRow = wgRunInfoLog.m_dt.NewRow();
				dataRow[0] = newInfo.category;
				wgRunInfoLog.eventRecID++;
				dataRow[1] = wgRunInfoLog.eventRecID.ToString();
				dataRow[2] = DateTime.Now.ToString("HH:mm:ss");
				dataRow[3] = newInfo.desc;
				dataRow[4] = newInfo.information;
				dataRow[5] = newInfo.detail;
				dataRow[6] = newInfo.MjRecStr;
				if (bWriteToDB)
				{
					wgAppConfig.wgLog(string.Format("{0},{1},{2},{3},{4}", new object[]
					{
						wgRunInfoLog.eventRecID.ToString(),
						newInfo.desc,
						newInfo.information,
						newInfo.detail,
						newInfo.MjRecStr
					}));
				}
				else
				{
					wgAppConfig.wgLogWithoutDB(string.Format("{0},{1},{2},{3},{4}", new object[]
					{
						wgRunInfoLog.eventRecID.ToString(),
						newInfo.desc,
						newInfo.information,
						newInfo.detail,
						newInfo.MjRecStr
					}), EventLogEntryType.Information, null);
				}
				wgRunInfoLog.m_dt.Rows.Add(dataRow);
			}
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00109411 File Offset: 0x00108411
		public static void addEventSpecial2()
		{
			if (wgRunInfoLog.m_dt != null)
			{
				wgRunInfoLog.m_dt.AcceptChanges();
				wgRunInfoLog.checkOnly5000();
			}
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0010942C File Offset: 0x0010842C
		public static void addEventToLog1(InfoRow newInfo, bool bWriteToDB)
		{
			if (wgRunInfoLog.m_dt != null)
			{
				DataRow dataRow = wgRunInfoLog.m_dt.NewRow();
				dataRow[0] = newInfo.category;
				wgRunInfoLog.eventRecID++;
				dataRow[1] = wgRunInfoLog.eventRecID.ToString();
				dataRow[2] = DateTime.Now.ToString("HH:mm:ss");
				dataRow[3] = newInfo.desc;
				dataRow[4] = newInfo.information;
				dataRow[5] = newInfo.detail;
				dataRow[6] = newInfo.MjRecStr;
				if (bWriteToDB)
				{
					wgAppConfig.wgLog(string.Format("{0},{1},{2},{3},{4}", new object[]
					{
						wgRunInfoLog.eventRecID.ToString(),
						newInfo.desc,
						newInfo.information,
						newInfo.detail,
						newInfo.MjRecStr
					}));
				}
				else
				{
					wgAppConfig.wgLogWithoutDB(string.Format("{0},{1},{2},{3},{4}", new object[]
					{
						wgRunInfoLog.eventRecID.ToString(),
						newInfo.desc,
						newInfo.information,
						newInfo.detail,
						newInfo.MjRecStr
					}), EventLogEntryType.Information, null);
				}
			}
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00109564 File Offset: 0x00108564
		private static void checkOnly5000()
		{
			if (wgRunInfoLog.m_dt.Rows.Count > 21000)
			{
				for (int i = wgRunInfoLog.m_dt.Rows.Count; i > 5000; i--)
				{
					wgRunInfoLog.m_dt.Rows[i - 1].Delete();
				}
				wgRunInfoLog.m_dt.AcceptChanges();
			}
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x001095C8 File Offset: 0x001085C8
		public static void init(out DataTable dt)
		{
			dt = new DataTable();
			dt.TableName = "runInfolog";
			dt.Columns.Add("f_Category");
			dt.Columns.Add("f_RecID");
			dt.Columns.Add("f_Time");
			dt.Columns.Add("f_Desc");
			dt.Columns.Add("f_Info");
			dt.Columns.Add("f_Detail");
			dt.Columns.Add("f_MjRecStr");
			dt.AcceptChanges();
			wgRunInfoLog.m_dt = dt;
			try
			{
				wgRunInfoLog.logRecEventMode = 0;
				int.TryParse(wgAppConfig.GetKeyVal("logRecEventMode"), out wgRunInfoLog.logRecEventMode);
				wgRunInfoLog.logRecCommFail = 0;
				int.TryParse(wgAppConfig.GetKeyVal("logRecCommFail"), out wgRunInfoLog.logRecCommFail);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04001BDB RID: 7131
		public static int eventRecID = 0;

		// Token: 0x04001BDC RID: 7132
		public static int logRecCommFail = 0;

		// Token: 0x04001BDD RID: 7133
		public static int logRecEventMode = 0;

		// Token: 0x04001BDE RID: 7134
		public static DataTable m_dt;

		// Token: 0x04001BDF RID: 7135
		public static int tcpServerAutoLoad = 1;

		// Token: 0x04001BE0 RID: 7136
		public static int tcpServerEnabled = 0;
	}
}
