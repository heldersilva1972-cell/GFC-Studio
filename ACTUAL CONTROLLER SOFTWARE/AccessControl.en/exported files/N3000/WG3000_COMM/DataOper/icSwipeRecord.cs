using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.DataOper
{
	// Token: 0x02000063 RID: 99
	internal class icSwipeRecord : wgMjControllerSwipeOperate
	{
		// Token: 0x0600077D RID: 1917 RVA: 0x000D34D9 File Offset: 0x000D24D9
		public icSwipeRecord()
		{
			base.Clear();
			this.bDisplayProcess = true;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x000D3510 File Offset: 0x000D2510
		public static int AddNewSwipe_SynConsumerID(MjRec mjrec)
		{
			int num = -9;
			try
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				dbConnection.Open();
				dbCommand.CommandType = CommandType.Text;
				int[] array = new int[4];
				int num2 = 0;
				string text = " select f_ReaderID   ";
				text = text + " FROM   t_b_Reader, t_b_Controller  WHERE t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID  AND  t_b_Controller.f_ControllerSN = " + mjrec.ControllerSN.ToString() + " ORDER BY f_ReaderNO ASC";
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read() && num2 < 4)
				{
					array[num2] = (int)dbDataReader[0];
					num2++;
				}
				dbDataReader.Close();
				int swipeRecordMaxRecIdOfDB = wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
				text = " INSERT INTO t_d_SwipeRecord (f_ReadDate, f_CardNO, f_Character, f_InOut, f_Status, f_RecOption, ";
				text = string.Concat(new string[]
				{
					text,
					" f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_RecordAll) values (",
					wgTools.PrepareStr(mjrec.ReadDate, true, icSwipeRecord.gYMDHMSFormat),
					",",
					mjrec.CardID.ToString(),
					",",
					mjrec.IsPassed ? "1" : "0",
					",",
					mjrec.IsEnterIn ? "1" : "0",
					",",
					mjrec.bytStatus.ToString(),
					",",
					mjrec.bytRecOption.ToString(),
					",",
					mjrec.ControllerSN.ToString(),
					",",
					array[(int)(mjrec.ReaderNo - 1)].ToString(),
					",",
					mjrec.ReaderNo.ToString(),
					",",
					mjrec.IndexInDataFlash.ToString(),
					",",
					wgTools.PrepareStrNUnicode(mjrec.ToStringRaw()),
					");"
				});
				dbCommand.CommandText = text;
				if (dbCommand.ExecuteNonQuery() <= 0)
				{
					return num;
				}
				if (wgAppConfig.IsAccessDB)
				{
					text = " UPDATE t_d_SwipeRecord   ";
					text = text + " INNER JOIN t_b_Consumer  ON (  t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO   AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString() + " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0)))) ) SET t_d_SwipeRecord.f_ConsumerID=t_b_Consumer.f_ConsumerID ";
					dbCommand.CommandText = text;
					num2 = dbCommand.ExecuteNonQuery();
					text = " UPDATE t_d_SwipeRecord   ";
					text = text + " INNER JOIN t_b_IDCard_Lost  ON (  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO   AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString() + " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0))))  ) SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID ";
					dbCommand.CommandText = text;
					num2 = dbCommand.ExecuteNonQuery();
				}
				else
				{
					text = " UPDATE t_d_SwipeRecord   ";
					text = text + " SET t_d_SwipeRecord.f_ConsumerID=t_b_Consumer.f_ConsumerID  FROM   t_d_SwipeRecord,t_b_Consumer  WHERE  t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO   AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString() + " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
					dbCommand.CommandText = text;
					num2 = dbCommand.ExecuteNonQuery();
					text = " UPDATE t_d_SwipeRecord   ";
					text = text + " SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID  FROM   t_d_SwipeRecord,t_b_IDCard_Lost  WHERE  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO   AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString() + " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
					dbCommand.CommandText = text;
					num2 = dbCommand.ExecuteNonQuery();
				}
				num = 1;
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x000D3860 File Offset: 0x000D2860
		public static int AddNewSwipe_SynConsumerID_Acc(MjRec mjrec)
		{
			int num = -9;
			string text = "";
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						oleDbCommand.CommandType = CommandType.Text;
						oleDbCommand.Connection = oleDbConnection;
						int swipeRecordMaxRecIdOfDB = wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
						text = " INSERT INTO t_d_SwipeRecord (f_ReadDate, f_CardNO, f_Character, f_InOut, f_Status, f_RecOption, ";
						text = string.Concat(new string[]
						{
							text,
							" f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_RecordAll) values (",
							wgTools.PrepareStr(mjrec.ReadDate, true, icSwipeRecord.gYMDHMSFormat),
							",",
							mjrec.CardID.ToString(),
							",",
							mjrec.IsPassed ? "1" : "0",
							",",
							mjrec.IsEnterIn ? "1" : "0",
							",",
							mjrec.bytStatus.ToString(),
							",",
							mjrec.bytRecOption.ToString(),
							",",
							mjrec.ControllerSN.ToString(),
							",0,",
							mjrec.ReaderNo.ToString(),
							",",
							mjrec.IndexInDataFlash.ToString(),
							",",
							wgTools.PrepareStrNUnicode(mjrec.ToStringRaw()),
							");"
						});
						oleDbCommand.CommandText = text;
						if (oleDbCommand.ExecuteNonQuery() > 0)
						{
							text = " UPDATE t_d_SwipeRecord   ";
							text = text + " INNER JOIN t_b_Consumer  ON (  t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO   AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString() + " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0)))) ) SET t_d_SwipeRecord.f_ConsumerID=t_b_Consumer.f_ConsumerID ";
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							text = " UPDATE t_d_SwipeRecord   ";
							text = text + " INNER JOIN t_b_IDCard_Lost  ON (  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO   AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString() + " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0))))  ) SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID ";
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							text = "UPDATE t_d_SwipeRecord a  ";
							text = string.Concat(new string[]
							{
								text,
								"   INNER JOIN ( t_b_Reader b  INNER JOIN t_b_Controller c ON ( c.f_ControllerID = b.f_ControllerID AND c.f_ControllerSN = ",
								mjrec.ControllerSN.ToString(),
								"  )) ON ( a.f_ReaderNO = b.f_ReaderNO AND a.f_ReaderNO =",
								mjrec.ReaderNo.ToString(),
								" AND a.f_RecID >",
								swipeRecordMaxRecIdOfDB.ToString(),
								" AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0))))  ) SET a.f_ReaderID=b.f_ReaderID "
							});
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							num = 1;
						}
					}
					return num;
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x000D3B64 File Offset: 0x000D2B64
		private bool bStopComm()
		{
			return icSwipeRecord.tickOperationStart != icSwipeRecord.tickOperation;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x000D3B78 File Offset: 0x000D2B78
		protected override void DisplayProcessInfo(string info, int infoCode, int specialInfo)
		{
			if (this.bDisplayProcess)
			{
				switch (infoCode)
				{
				case 1:
					wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", info, CommonStr.strGotRecords, specialInfo));
					return;
				case 2:
					wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", info, CommonStr.strWritingRecordsToDB, specialInfo));
					return;
				case 3:
					wgAppRunInfo.raiseAppRunInfoCommStatus("");
					break;
				case 4:
					break;
				case 5:
					wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", info, CommonStr.strWatingForWritingRecordsToDB));
					return;
				default:
					wgAppRunInfo.raiseAppRunInfoCommStatus(info);
					return;
				}
				return;
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000D3C0C File Offset: 0x000D2C0C
		public int GetSwipeRecords(int ControllerSN, string IP, int Port, string DoorName)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.GetSwipeRecords_P64(ControllerSN, IP, Port, DoorName);
			}
			if (this.control == null)
			{
				this.control = new icController();
			}
			this.control.ControllerSN = ControllerSN;
			this.control.IP = IP;
			this.control.PORT = Port;
			this.control.runinfo.Clear();
			base.ControllerSN = ControllerSN;
			int num = -13;
			long commTimeoutMsMin = wgUdpComm.CommTimeoutMsMin;
			try
			{
				int num2 = 3;
				int num3 = 300;
				int num4 = 0;
				while (num4 < num2 && this.control.GetControllerRunInformationIP(-1) <= 0)
				{
					Thread.Sleep(num3);
					num4++;
				}
				if (this.control.runinfo.wgcticks <= 0U)
				{
					return num;
				}
				if (wgAppConfig.getParamValBoolByNO(177))
				{
					int num5 = wgAppConfig.getValBySql(string.Format("Select f_RecordFlashLoc From [t_d_SwipeRecord]  Where [f_ControllerSN] = {0} order by [f_RecID] desc ", this.control.ControllerSN));
					if (num5 > 0)
					{
						num5++;
					}
					if ((long)num5 <= (long)((ulong)this.control.runinfo.swipeEndIndex) && this.control.runinfo.lastGetRecordIndex != 0U)
					{
						this.control.runinfo.lastGetRecordIndex = (uint)num5;
					}
				}
				if (this.control.runinfo.newRecordsNum == 0U)
				{
					base.lastRecordFlashIndex = (int)this.control.runinfo.lastGetRecordIndex;
					return 0;
				}
				if (this.control.runinfo.netSpeedCode != 0)
				{
					wgUdpComm.CommTimeoutMsMin = 2000L;
				}
				return this.GetSwipeRecords_Realize(ControllerSN, IP, Port, DoorName);
			}
			catch (OleDbException ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				if (ex.ErrorCode == -2147467259)
				{
					XMessageBox.Show(CommonStr.strAccessDatabaseTryBackup, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
				if (!string.IsNullOrEmpty(wgAppConfig.getTriggerName()))
				{
					wgAppConfig.wgLog(wgAppConfig.getTriggerName());
					XMessageBox.Show(string.Concat(new string[]
					{
						CommonStr.strOperateFailed,
						"\r\n\r\n",
						CommonStr.strCheckDatabase2015,
						" ",
						wgAppConfig.getTriggerName()
					}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			finally
			{
				wgUdpComm.CommTimeoutMsMin = commTimeoutMsMin;
				if (wgTools.bUDPCloud > 0)
				{
					if (this.control != null)
					{
						this.control.Dispose();
						this.control = null;
					}
					if (base.wgudp != null)
					{
						base.wgudp.Close();
						base.wgudp.Dispose();
						base.wgudp = null;
					}
				}
			}
			return num;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x000D3EE0 File Offset: 0x000D2EE0
		public int GetSwipeRecords_Acc(int ControllerSN, string IP, int Port, string DoorName)
		{
			wgTools.WriteLine("getSwipeRecords_Acc Start");
			this.control.ControllerSN = ControllerSN;
			this.control.IP = IP;
			this.control.PORT = Port;
			base.ControllerSN = ControllerSN;
			int num = 0;
			if (base.wgudp == null)
			{
				base.wgudp = new wgUdpComm();
				Thread.Sleep(300);
			}
			byte[] array = null;
			WGPacketSSI_FLASH_QUERY wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, 5017600U, 5018623U);
			byte[] array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
			if (array2 == null)
			{
				return -12;
			}
			array = null;
			int num2 = 3;
			int num3 = 300;
			for (int i = 0; i < num2; i++)
			{
				num = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
				if (num >= 0)
				{
					break;
				}
				Thread.Sleep(num3);
			}
			if (num < 0)
			{
				return -13;
			}
			wgTools.WriteLine(string.Format("\r\nBegin Sending Command:\t{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			string.Format("SSI_FLASH_{0}", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
			int num4 = 4096;
			int num5 = 0;
			int swipeRecordMaxRecIdOfDB = wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
			int num6 = 0;
			int[] array3 = new int[4];
			string text = " select f_ReaderID   ";
			text = text + " FROM   t_b_Reader, t_b_Controller  WHERE t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID  AND  t_b_Controller.f_ControllerSN = " + ControllerSN.ToString() + " ORDER BY f_ReaderNO ASC";
			int num7 = 0;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					while (oleDbDataReader.Read() && num7 < 4)
					{
						array3[num7] = (int)oleDbDataReader[0];
						num7++;
					}
					oleDbDataReader.Close();
				}
			}
			num = 0;
			int num8 = (int)(this.control.runinfo.swipeEndIndex - this.control.runinfo.newRecordsNum);
			if (num8 > 0)
			{
				int swipeLoc = base.GetSwipeLoc(num8);
				wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, (uint)(swipeLoc - swipeLoc % 1024), (uint)(swipeLoc - swipeLoc % 1024 + 1024 - 1));
				array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
				if (array2 == null)
				{
					return -12;
				}
				array = null;
				for (int j = 0; j < num2; j++)
				{
					num = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
					if (num >= 0)
					{
						break;
					}
					Thread.Sleep(num3);
				}
				if (num < 0)
				{
					return -13;
				}
			}
			text = "";
			uint iStartFlashAddr = wgpacketSSI_FLASH_QUERY.iStartFlashAddr;
			bool flag = false;
			uint num9 = 0U;
			wgTools.WriteLine(string.Format("First Page:\t ={0:d}", iStartFlashAddr / 1024U));
			int num10 = num8 - num8 % 204800;
			OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString);
			OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2);
			oleDbConnection2.Open();
			while (!wgMjControllerSwipeOperate.bStopGetRecord && array != null)
			{
				WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH(array);
				uint num11 = (uint)(base.GetSwipeIndex(wgpacketSSI_FLASH.iStartFlashAddr) + num10);
				for (uint num12 = 0U; num12 < 1024U; num12 += 16U)
				{
					MjRec mjRec = new MjRec(wgpacketSSI_FLASH.ucData, num12, wgpacketSSI_FLASH.iDevSnFrom, num11);
					if (mjRec.CardID == -1L || ((mjRec.bytRecOption == 0 || mjRec.bytRecOption == 255) && mjRec.CardID == 0L))
					{
						if (this.control.runinfo.swipeEndIndex <= num11)
						{
							break;
						}
						num9 += 1U;
						num11 += 1U;
					}
					else
					{
						if (num5 > 0 || (ulong)mjRec.IndexInDataFlash >= (ulong)((long)num8))
						{
							text = "";
							text = string.Concat(new string[]
							{
								text,
								" INSERT INTO t_d_SwipeRecord (f_ReadDate, f_CardNO, f_Character, f_InOut, f_Status, f_RecOption,  f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_RecordAll) values (",
								wgTools.PrepareStr(mjRec.ReadDate, true, icSwipeRecord.gYMDHMSFormat),
								",",
								mjRec.CardID.ToString(),
								",",
								mjRec.IsPassed ? "1" : "0",
								",",
								mjRec.IsEnterIn ? "1" : "0",
								",",
								mjRec.bytStatus.ToString(),
								",",
								mjRec.bytRecOption.ToString(),
								",",
								mjRec.ControllerSN.ToString(),
								",",
								array3[(int)(mjRec.ReaderNo - 1)].ToString(),
								",",
								mjRec.ReaderNo.ToString(),
								",",
								mjRec.IndexInDataFlash.ToString(),
								",",
								wgTools.PrepareStrNUnicode(mjRec.ToStringRaw()),
								")"
							});
							oleDbCommand2.CommandText = text;
							num7 = oleDbCommand2.ExecuteNonQuery();
							num5++;
							num6 = (int)mjRec.IndexInDataFlash;
						}
						num11 += 1U;
					}
				}
				this.DisplayProcessInfo(DoorName, 1, num5);
				if (this.control.runinfo.swipeEndIndex <= num11)
				{
					break;
				}
				if (flag)
				{
					break;
				}
				wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, wgpacketSSI_FLASH_QUERY.iStartFlashAddr + 1024U, wgpacketSSI_FLASH_QUERY.iStartFlashAddr + 1024U + 1024U - 1U);
				if (wgpacketSSI_FLASH_QUERY.iStartFlashAddr > 8294399U)
				{
					wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, 5017600U, 5018623U);
					num10 += 204800;
				}
				if (wgpacketSSI_FLASH_QUERY.iStartFlashAddr == iStartFlashAddr)
				{
					break;
				}
				wgpacketSSI_FLASH_QUERY.GetNewXid();
				array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
				if (array2 == null)
				{
					break;
				}
				for (int k = 0; k < num2; k++)
				{
					num = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
					if (num >= 0)
					{
						break;
					}
					Thread.Sleep(num3);
				}
				if (num < 0 || --num4 <= 0)
				{
					break;
				}
			}
			oleDbConnection2.Close();
			wgTools.WriteLine(string.Format("Last Page:\t ={0:d}", wgpacketSSI_FLASH_QUERY.iStartFlashAddr / 1024U));
			wgTools.WriteLine(string.Format("Got Records:\t Count={0:d}", num5));
			if (num9 > 0U)
			{
				wgAppConfig.wgLog(string.Format("Got Records:\t invalidRecCount={0:d}", num9));
			}
			this.DisplayProcessInfo(DoorName, 2, num5);
			text = " UPDATE t_d_SwipeRecord   ";
			text = text + " INNER JOIN   t_b_Consumer  ON  (t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO   AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString() + " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0))))  ) SET t_d_SwipeRecord.f_ConsumerID=  t_b_Consumer.f_ConsumerID ";
			using (OleDbConnection oleDbConnection3 = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand3 = new OleDbCommand(text, oleDbConnection3))
				{
					oleDbConnection3.Open();
					oleDbCommand3.CommandTimeout = num5 / 250 + 30;
					num7 = oleDbCommand3.ExecuteNonQuery();
				}
			}
			text = " UPDATE t_d_SwipeRecord   ";
			text = text + " INNER JOIN   t_b_IDCard_Lost  ON (  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO   AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString() + " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0))))  ) SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID ";
			using (OleDbConnection oleDbConnection4 = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand4 = new OleDbCommand(text, oleDbConnection4))
				{
					oleDbConnection4.Open();
					oleDbCommand4.CommandTimeout = num5 / 250 + 30;
					num7 = oleDbCommand4.ExecuteNonQuery();
				}
			}
			num = 0;
			wgTools.WriteLine("Syn Data Info");
			if (num5 > 0)
			{
				for (int l = 0; l < num2; l++)
				{
					num = this.control.GetControllerRunInformationIP(-1);
					if (num >= 0)
					{
						break;
					}
					Thread.Sleep(num3);
				}
				if (num < 0)
				{
					return -13;
				}
				if ((long)(num6 % 204800) >= (long)((ulong)(this.control.runinfo.swipeEndIndex % 204800U)))
				{
					if (this.control.runinfo.swipeEndIndex > 204800U)
					{
						num8 = (int)(this.control.runinfo.swipeEndIndex - this.control.runinfo.swipeEndIndex % 204800U - 204800U + (uint)(num6 % 204800));
					}
					else
					{
						num8 = 0;
					}
				}
				else
				{
					num8 = (int)(this.control.runinfo.swipeEndIndex - this.control.runinfo.swipeEndIndex % 204800U + (uint)(num6 % 204800));
				}
				int num13 = 0;
				while (num13 < num2 && this.control.UpdateLastGetRecordLocationIP((uint)(num8 + 1)) < 0)
				{
					Thread.Sleep(num3);
					num13++;
				}
				base.lastRecordFlashIndex = num8 + 1;
			}
			this.DisplayProcessInfo(DoorName, 3, num5);
			return num5;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000D4804 File Offset: 0x000D3804
		public int GetSwipeRecords_Acc_P64(int ControllerSN, string IP, int Port, string DoorName, int gotIndexParam, int UDPQueue4MultithreadIndexParam = -1)
		{
			wgTools.WriteLine("getSwipeRecords_Acc Start");
			this.control.ControllerSN = ControllerSN;
			this.control.IP = IP;
			this.control.PORT = Port;
			base.ControllerSN = ControllerSN;
			if (base.wgudp == null)
			{
				base.wgudp = new wgUdpComm(UDPQueue4MultithreadIndexParam);
				if (wgTools.bUDPCloud <= 0)
				{
					Thread.Sleep(300);
				}
			}
			int num11;
			try
			{
				int num = 0;
				int num2 = 0;
				byte[] array = new byte[64];
				int info_SwipeIndex_IP = this.control.GetInfo_SwipeIndex_IP64(ref array, 0L, "", UDPQueue4MultithreadIndexParam);
				if (array == null || info_SwipeIndex_IP < 0)
				{
					this.DisplayProcessInfo("通信失败", 0, 0);
					return -1;
				}
				num = this.control.GetInfo_SwipeIndex_IP64(ref array, (long)((ulong)(-1)), "", UDPQueue4MultithreadIndexParam);
				if (array == null || info_SwipeIndex_IP < 0)
				{
					this.DisplayProcessInfo("通信失败", 0, 0);
					return -1;
				}
				int num3 = this.control.GetInfo_RecordGotIndex_IP64(ref array, "", UDPQueue4MultithreadIndexParam);
				if (gotIndexParam > 0)
				{
					num3 = gotIndexParam;
				}
				int num4 = wgAppConfig.getValBySql(string.Format("Select f_RecordFlashLoc From [t_d_SwipeRecord]  Where [f_ControllerSN] = {0} order by [f_RecID] desc ", this.control.ControllerSN));
				if (num4 > 0)
				{
					num4++;
				}
				if (num4 > num3 && num4 < num)
				{
					num3 = num4;
				}
				if (num == num3)
				{
					base.lastRecordFlashIndex = (int)this.control.runinfo.swipeEndIndex;
					return 0;
				}
				num3++;
				byte[] array2 = new byte[64];
				for (int i = 0; i < 64; i++)
				{
					array2[i] = 0;
				}
				num2 = num3;
				if (num2 > num)
				{
					num2 = info_SwipeIndex_IP - 1;
				}
				if (num3 < info_SwipeIndex_IP)
				{
					num2 = info_SwipeIndex_IP - 1;
				}
				this.control.GetInfo_Swipe_IP64(ref array, (long)num2, "", UDPQueue4MultithreadIndexParam);
				array = null;
				int num5 = 240000;
				int num6 = 0;
				int swipeRecordMaxRecIdOfDB = wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
				int[] array3 = new int[4];
				string text = " select f_ReaderID   ";
				text = text + " FROM   t_b_Reader, t_b_Controller  WHERE t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID  AND  t_b_Controller.f_ControllerSN = " + ControllerSN.ToString() + " ORDER BY f_ReaderNO ASC";
				int num7 = 0;
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read() && num7 < 4)
						{
							array3[num7] = (int)oleDbDataReader[0];
							num7++;
						}
						oleDbDataReader.Close();
					}
				}
				int num8 = num3;
				text = "";
				uint num9 = 0U;
				int num10 = num8 % 204800;
				OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString);
				OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2);
				if (UDPQueue4MultithreadIndexParam < 0)
				{
					oleDbConnection2.Open();
				}
				bool flag = false;
				while (!wgMjControllerSwipeOperate.bStopGetRecord)
				{
					array = null;
					if (this.control.GetInfo_Swipe_IP64(ref array, (long)num2, "", UDPQueue4MultithreadIndexParam) <= 0)
					{
						flag = true;
						break;
					}
					if (array != null)
					{
						byte[] array4 = new byte[16];
						Array.Copy(array, 16, array4, 0, 4);
						Array.Copy(array, 44, array4, 4, 4);
						Array.Copy(array, 56, array4, 8, 8);
						MjRec mjRec = new MjRec(array4, 0U, (uint)ControllerSN, (uint)num2);
						if (mjRec.CardID == -1L || ((mjRec.bytRecOption == 0 || mjRec.bytRecOption == 255) && mjRec.CardID == 0L))
						{
							num9 += 1U;
						}
						else if (num6 > 0 || (ulong)mjRec.IndexInDataFlash >= (ulong)((long)num8))
						{
							if (UDPQueue4MultithreadIndexParam >= 0)
							{
								this.arrSwipes.Add(mjRec.ToStringRaw());
								num6++;
							}
							else
							{
								text = "";
								text = string.Concat(new string[]
								{
									text,
									" INSERT INTO t_d_SwipeRecord (f_ReadDate, f_CardNO, f_Character, f_InOut, f_Status, f_RecOption,  f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_RecordAll) values (",
									wgTools.PrepareStr(mjRec.ReadDate, true, icSwipeRecord.gYMDHMSFormat),
									",",
									mjRec.CardID.ToString(),
									",",
									mjRec.IsPassed ? "1" : "0",
									",",
									mjRec.IsEnterIn ? "1" : "0",
									",",
									mjRec.bytStatus.ToString(),
									",",
									mjRec.bytRecOption.ToString(),
									",",
									mjRec.ControllerSN.ToString(),
									",",
									array3[(int)(mjRec.ReaderNo - 1)].ToString(),
									",",
									mjRec.ReaderNo.ToString(),
									",",
									mjRec.IndexInDataFlash.ToString(),
									",",
									wgTools.PrepareStrNUnicode(mjRec.ToStringRaw()),
									")"
								});
								oleDbCommand2.CommandText = text;
								num7 = oleDbCommand2.ExecuteNonQuery();
								num6++;
							}
							uint indexInDataFlash = mjRec.IndexInDataFlash;
						}
						num8 = num2;
					}
					this.DisplayProcessInfo(DoorName, 1, num6);
					if (num <= num2)
					{
						break;
					}
					num2++;
					if (--num5 <= 0)
					{
						break;
					}
				}
				if (UDPQueue4MultithreadIndexParam < 0)
				{
					oleDbConnection2.Close();
				}
				wgTools.WriteLine(string.Format("Got Records:\t Count={0:d}", num6));
				if (num9 > 0U)
				{
					wgAppConfig.wgLog(string.Format("Got Records:\t invalidRecCount={0:d}", num9));
				}
				this.DisplayProcessInfo(DoorName, 2, num6);
				if (UDPQueue4MultithreadIndexParam >= 0)
				{
					this.DisplayProcessInfo(DoorName, 5, 0);
					lock (icSwipeRecord.WriteDBPQueue.SyncRoot)
					{
						this.DisplayProcessInfo(DoorName, 2, 0);
						this.GetSwipeRecords_WriteToDB(DoorName);
						goto IL_06BB;
					}
				}
				text = " UPDATE t_d_SwipeRecord   ";
				text = text + " INNER JOIN   t_b_Consumer  ON  (t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO   AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString() + " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0))))  ) SET t_d_SwipeRecord.f_ConsumerID=  t_b_Consumer.f_ConsumerID ";
				using (OleDbConnection oleDbConnection3 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand3 = new OleDbCommand(text, oleDbConnection3))
					{
						oleDbConnection3.Open();
						oleDbCommand3.CommandTimeout = num6 / 250 + 30;
						num7 = oleDbCommand3.ExecuteNonQuery();
					}
				}
				text = " UPDATE t_d_SwipeRecord   ";
				text = text + " INNER JOIN   t_b_IDCard_Lost  ON (  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO   AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString() + " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0))))  ) SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID ";
				using (OleDbConnection oleDbConnection4 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand4 = new OleDbCommand(text, oleDbConnection4))
					{
						oleDbConnection4.Open();
						oleDbCommand4.CommandTimeout = num6 / 250 + 30;
						num7 = oleDbCommand4.ExecuteNonQuery();
					}
				}
				IL_06BB:
				wgTools.WriteLine("Syn Data Info");
				if (!flag && num6 > 0)
				{
					this.control.GetInfo_UpdateLastGetRecordLocationIP_IP64(ref array, (long)num8, "", UDPQueue4MultithreadIndexParam);
					base.lastRecordFlashIndex = num8 + 1;
				}
				this.DisplayProcessInfo(DoorName, 3, num6);
				num11 = num6;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (wgTools.bUDPCloud > 0)
				{
					wgTools.wgcloud.removeUDPQueue4Multithread(base.wgudp.UDPQueue4MultithreadIndex, (uint)ControllerSN);
				}
				this.control.Dispose();
				base.wgudp.Dispose();
				base.wgudp = null;
			}
			return num11;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000D5034 File Offset: 0x000D4034
		public static int GetSwipeRecords_MaxRecIdOfDB()
		{
			icSwipeRecord.recMaxIdOfDB = wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
			return icSwipeRecord.recMaxIdOfDB;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x000D5048 File Offset: 0x000D4048
		private int GetSwipeRecords_MultiThread(int ControllerSN, string IP, int Port, string DoorName)
		{
			this.control.ControllerSN = ControllerSN;
			this.control.IP = IP;
			this.control.PORT = Port;
			this.control.runinfo.Clear();
			base.ControllerSN = ControllerSN;
			int num = -13;
			long commTimeoutMsMin = wgUdpComm.CommTimeoutMsMin;
			try
			{
				num = this.GetSwipeRecords_Multithread_Realize(ControllerSN, IP, Port, DoorName);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			finally
			{
				wgUdpComm.CommTimeoutMsMin = commTimeoutMsMin;
			}
			return num;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x000D50DC File Offset: 0x000D40DC
		public int GetSwipeRecords_Multithread_Realize(int ControllerSN, string IP, int Port, string DoorName)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.GetSwipeRecords_Multithread_Realize_P64(ControllerSN, IP, Port, DoorName);
			}
			wgTools.WriteLine("getSwipeRecords Start");
			this.control.ControllerSN = ControllerSN;
			this.control.IP = IP;
			this.control.PORT = Port;
			base.ControllerSN = ControllerSN;
			int num = 0;
			int num2 = 0;
			if (base.wgudp == null)
			{
				if (wgTools.wgcloud != null)
				{
					base.wgudp = new wgUdpComm(wgTools.wgcloud.getUDPQueue4Multithread((uint)ControllerSN));
				}
				else
				{
					base.wgudp = new wgUdpComm();
				}
				if (wgTools.bUDPCloud <= 0)
				{
					Thread.Sleep(300);
				}
			}
			else if (wgTools.wgcloud != null)
			{
				base.wgudp.UDPQueue4MultithreadIndex = wgTools.wgcloud.getUDPQueue4Multithread((uint)ControllerSN);
			}
			try
			{
				int num3 = 3;
				int num4 = 300;
				WGPacketBasicRunInformationToSend wgpacketBasicRunInformationToSend = new WGPacketBasicRunInformationToSend();
				wgpacketBasicRunInformationToSend.type = 32;
				wgpacketBasicRunInformationToSend.code = 16;
				wgpacketBasicRunInformationToSend.iDevSnFrom = 0U;
				wgpacketBasicRunInformationToSend.iDevSnTo = (uint)ControllerSN;
				wgpacketBasicRunInformationToSend.iCallReturn = 0;
				byte[] array = null;
				byte[] array2 = wgpacketBasicRunInformationToSend.ToBytes(base.wgudp.udpPort);
				if (array2 == null)
				{
					wgTools.WriteLine("\r\nError 1");
					return -1;
				}
				for (int i = 0; i < num3; i++)
				{
					if (this.bStopComm())
					{
						return num2;
					}
					num2 = base.wgudp.udp_get(array2, 300, wgpacketBasicRunInformationToSend.xid, IP, Port, ref array);
					if (num2 >= 0)
					{
						break;
					}
					Thread.Sleep(num4);
				}
				if (num2 < 0)
				{
					return -13;
				}
				wgMjControllerRunInformation wgMjControllerRunInformation = new wgMjControllerRunInformation();
				wgMjControllerRunInformation.UpdateInfo_internal(array, 20, (uint)ControllerSN);
				if (wgMjControllerRunInformation.wgcticks > 0U)
				{
					this.DisplayProcessInfo(DoorName, 4, (int)wgMjControllerRunInformation.newRecordsNum);
					if (wgMjControllerRunInformation.newRecordsNum == 0U)
					{
						base.lastRecordFlashIndex = (int)wgMjControllerRunInformation.lastGetRecordIndex;
						return 0;
					}
					if (wgMjControllerRunInformation.netSpeedCode != 0)
					{
						wgUdpComm.CommTimeoutMsMin = 2000L;
					}
				}
				WGPacketSSI_FLASH_QUERY wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, 5017600U, 5018623U);
				array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
				if (array2 == null)
				{
					return -12;
				}
				array = null;
				for (int j = 0; j < num3; j++)
				{
					if (this.bStopComm())
					{
						return num2;
					}
					num2 = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
					if (num2 >= 0)
					{
						break;
					}
					Thread.Sleep(num4);
				}
				if (num2 < 0)
				{
					return -13;
				}
				wgTools.WriteLine(string.Format("\r\nBegin Sending Command:\t{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
				string.Format("SSI_FLASH_{0}", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
				int num5 = 4096;
				int num6 = 0;
				int num7 = (int)wgMjControllerRunInformation.lastGetRecordIndex;
				num2 = 0;
				num7 = (int)(wgMjControllerRunInformation.swipeEndIndex - wgMjControllerRunInformation.newRecordsNum);
				if (num7 > 0)
				{
					int swipeLoc = base.GetSwipeLoc(num7);
					wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, (uint)(swipeLoc - swipeLoc % 1024), (uint)(swipeLoc - swipeLoc % 1024 + 1024 - 1));
					array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
					if (array2 == null)
					{
						return -12;
					}
					array = null;
					for (int k = 0; k < num3; k++)
					{
						if (this.bStopComm())
						{
							return num2;
						}
						num2 = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
						if (num2 >= 0)
						{
							break;
						}
						Thread.Sleep(num4);
					}
					if (num2 < 0)
					{
						return -13;
					}
				}
				uint iStartFlashAddr = wgpacketSSI_FLASH_QUERY.iStartFlashAddr;
				bool flag = false;
				uint num8 = 0U;
				wgTools.WriteLine(string.Format("First Page:\t ={0:d}", iStartFlashAddr / 1024U));
				int num9 = num7 - num7 % 204800;
				MjRec mjRec = new MjRec();
				while (!wgMjControllerSwipeOperate.bStopGetRecord && array != null)
				{
					WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH(array);
					uint num10 = (uint)(base.GetSwipeIndex(wgpacketSSI_FLASH.iStartFlashAddr) + num9);
					for (uint num11 = 0U; num11 < 1024U; num11 += 16U)
					{
						mjRec.Update(wgpacketSSI_FLASH.ucData, num11, wgpacketSSI_FLASH.iDevSnFrom, num10);
						if (mjRec.CardID == -1L || ((mjRec.bytRecOption == 0 || mjRec.bytRecOption == 255) && mjRec.CardID == 0L))
						{
							if (wgMjControllerRunInformation.swipeEndIndex <= num10)
							{
								break;
							}
							num8 += 1U;
							num10 += 1U;
						}
						else
						{
							if (num > 0 || (ulong)mjRec.IndexInDataFlash >= (ulong)((long)num7))
							{
								this.arrSwipes.Add(mjRec.ToStringRaw());
								num++;
								num6 = (int)mjRec.IndexInDataFlash;
							}
							num10 += 1U;
						}
					}
					this.DisplayProcessInfo(DoorName, 1, num);
					if (wgMjControllerRunInformation.swipeEndIndex <= num10)
					{
						break;
					}
					if (flag)
					{
						break;
					}
					wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, wgpacketSSI_FLASH_QUERY.iStartFlashAddr + 1024U, wgpacketSSI_FLASH_QUERY.iStartFlashAddr + 1024U + 1024U - 1U);
					if (wgpacketSSI_FLASH_QUERY.iStartFlashAddr > 8294399U)
					{
						wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, 5017600U, 5018623U);
						num9 += 204800;
					}
					if (wgpacketSSI_FLASH_QUERY.iStartFlashAddr == iStartFlashAddr)
					{
						break;
					}
					wgpacketSSI_FLASH_QUERY.GetNewXid();
					array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
					if (array2 == null)
					{
						break;
					}
					for (int l = 0; l < num3; l++)
					{
						if (this.bStopComm())
						{
							return num2;
						}
						num2 = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
						if (num2 >= 0)
						{
							break;
						}
						Thread.Sleep(num4);
					}
					if (num2 < 0 || --num5 <= 0)
					{
						break;
					}
				}
				wgTools.WriteLine(string.Format("Last Page:\t ={0:d}", wgpacketSSI_FLASH_QUERY.iStartFlashAddr / 1024U));
				wgTools.WriteLine(string.Format("Got Records:\t Count={0:d}", num));
				if (num8 > 0U)
				{
					wgAppConfig.wgLog(string.Format("Got Records:\t invalidRecCount={0:d}", num8));
				}
				this.DisplayProcessInfo(DoorName, 5, 0);
				if (num > 0)
				{
					lock (icSwipeRecord.WriteDBPQueue.SyncRoot)
					{
						this.DisplayProcessInfo(DoorName, 2, 0);
						num2 = this.GetSwipeRecords_WriteToDB(DoorName);
					}
					if (num2 < 0)
					{
						return num2;
					}
				}
				num2 = 0;
				wgTools.WriteLine("Syn Data Info");
				if (num > 0)
				{
					array2 = wgpacketBasicRunInformationToSend.ToBytes(base.wgudp.udpPort);
					if (array2 == null)
					{
						wgTools.WriteLine("\r\nError 1");
						return -1;
					}
					for (int m = 0; m < num3; m++)
					{
						if (this.bStopComm())
						{
							return num2;
						}
						num2 = base.wgudp.udp_get(array2, 300, wgpacketBasicRunInformationToSend.xid, IP, Port, ref array);
						if (num2 >= 0)
						{
							break;
						}
						Thread.Sleep(num4);
					}
					if (num2 < 0)
					{
						return -13;
					}
					wgMjControllerRunInformation.UpdateInfo_internal(array, 20, (uint)ControllerSN);
					if (num2 < 0)
					{
						return -13;
					}
					if ((long)(num6 % 204800) >= (long)((ulong)(wgMjControllerRunInformation.swipeEndIndex % 204800U)))
					{
						if (wgMjControllerRunInformation.swipeEndIndex > 204800U)
						{
							num7 = (int)(wgMjControllerRunInformation.swipeEndIndex - wgMjControllerRunInformation.swipeEndIndex % 204800U - 204800U + (uint)(num6 % 204800));
						}
						else
						{
							num7 = 0;
						}
					}
					else
					{
						num7 = (int)(wgMjControllerRunInformation.swipeEndIndex - wgMjControllerRunInformation.swipeEndIndex % 204800U + (uint)(num6 % 204800));
					}
					WGPacketBasicFRamSetToSend wgpacketBasicFRamSetToSend = new WGPacketBasicFRamSetToSend(8U, (uint)(num7 + 1));
					wgpacketBasicFRamSetToSend.type = 32;
					wgpacketBasicFRamSetToSend.code = 128;
					wgpacketBasicFRamSetToSend.iDevSnFrom = 0U;
					wgpacketBasicFRamSetToSend.iDevSnTo = (uint)ControllerSN;
					wgpacketBasicFRamSetToSend.iCallReturn = 0;
					array2 = wgpacketBasicFRamSetToSend.ToBytes(base.wgudp.udpPort);
					if (array2 == null)
					{
						wgTools.WriteLine("\r\nError 1");
						return -1;
					}
					for (int n = 0; n < num3; n++)
					{
						if (this.bStopComm())
						{
							return num2;
						}
						num2 = base.wgudp.udp_get(array2, 300, wgpacketBasicFRamSetToSend.xid, IP, Port, ref array);
						if (num2 >= 0)
						{
							break;
						}
						Thread.Sleep(num4);
					}
					if (num2 < 0)
					{
						return -13;
					}
					base.lastRecordFlashIndex = num7 + 1;
				}
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (wgTools.bUDPCloud > 0)
				{
					Thread.Sleep(300);
					wgTools.wgcloud.removeUDPQueue4Multithread(base.wgudp.UDPQueue4MultithreadIndex, (uint)ControllerSN);
				}
				this.control.Dispose();
				base.wgudp.Dispose();
				base.wgudp = null;
				this.DisplayProcessInfo(DoorName, 3, num);
			}
			return num;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x000D59A0 File Offset: 0x000D49A0
		public int GetSwipeRecords_Multithread_Realize_P64(int ControllerSN, string IP, int Port, string DoorName)
		{
			if (wgTools.wgcloud != null)
			{
				return this.GetSwipeRecords_Acc_P64(ControllerSN, IP, Port, DoorName, -1, wgTools.wgcloud.getUDPQueue4Multithread((uint)ControllerSN));
			}
			return this.GetSwipeRecords_Acc_P64(ControllerSN, IP, Port, DoorName, -1, -1);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x000D59D0 File Offset: 0x000D49D0
		public int GetSwipeRecords_P64(int ControllerSN, string IP, int Port, string DoorName)
		{
			if (this.control == null)
			{
				this.control = new icController();
			}
			this.control.ControllerSN = ControllerSN;
			this.control.IP = IP;
			this.control.PORT = Port;
			this.control.runinfo.Clear();
			base.ControllerSN = ControllerSN;
			int num = -13;
			long commTimeoutMsMin = wgUdpComm.CommTimeoutMsMin;
			try
			{
				int num2 = 3;
				int num3 = 300;
				int num4 = 0;
				while (num4 < num2 && this.control.GetControllerRunInformationIP(-1) <= 0)
				{
					Thread.Sleep(num3);
					num4++;
				}
				byte[] array = null;
				if (this.control.runinfo.wgcticks <= 0U)
				{
					return num;
				}
				int num5 = this.control.GetInfo_RecordGotIndex_IP64(ref array, "", -1);
				if (wgAppConfig.getParamValBoolByNO(177))
				{
					int num6 = wgAppConfig.getValBySql(string.Format("Select f_RecordFlashLoc From [t_d_SwipeRecord]  Where [f_ControllerSN] = {0} order by [f_RecID] desc ", this.control.ControllerSN));
					if (num6 > 0)
					{
						num6++;
					}
					if (num6 <= num5 && num5 != 0)
					{
						num5 = num6;
					}
				}
				if ((ulong)this.control.runinfo.swipeEndIndex == (ulong)((long)num5))
				{
					base.lastRecordFlashIndex = (int)this.control.runinfo.swipeEndIndex;
					return 0;
				}
				if ((long)num5 > (long)((ulong)this.control.runinfo.swipeEndIndex))
				{
					num5 = 1;
				}
				return this.GetSwipeRecords_Realize_P64(ControllerSN, IP, Port, DoorName, num5);
			}
			catch (OleDbException ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				if (ex.ErrorCode == -2147467259)
				{
					XMessageBox.Show(CommonStr.strAccessDatabaseTryBackup, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
				if (!string.IsNullOrEmpty(wgAppConfig.getTriggerName()))
				{
					wgAppConfig.wgLog(wgAppConfig.getTriggerName());
					XMessageBox.Show(string.Concat(new string[]
					{
						CommonStr.strOperateFailed,
						"\r\n\r\n",
						CommonStr.strCheckDatabase2015,
						" ",
						wgAppConfig.getTriggerName()
					}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			finally
			{
				wgUdpComm.CommTimeoutMsMin = commTimeoutMsMin;
				if (wgTools.bUDPCloud > 0)
				{
					if (this.control != null)
					{
						this.control.Dispose();
						this.control = null;
					}
					if (base.wgudp != null)
					{
						base.wgudp.Close();
						base.wgudp.Dispose();
						base.wgudp = null;
					}
				}
			}
			return num;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x000D5C80 File Offset: 0x000D4C80
		public int GetSwipeRecords_Realize(int ControllerSN, string IP, int Port, string DoorName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.GetSwipeRecords_Acc(ControllerSN, IP, Port, DoorName);
			}
			wgTools.WriteLine("getSwipeRecords Start");
			this.control.ControllerSN = ControllerSN;
			this.control.IP = IP;
			this.control.PORT = Port;
			base.ControllerSN = ControllerSN;
			int num = 0;
			string text = "";
			int num14;
			try
			{
				if (base.wgudp == null)
				{
					base.wgudp = new wgUdpComm();
					Thread.Sleep(300);
				}
				byte[] array = null;
				WGPacketSSI_FLASH_QUERY wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, 5017600U, 5018623U);
				byte[] array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
				if (array2 == null)
				{
					return -12;
				}
				array = null;
				int num2 = 3;
				int num3 = 300;
				for (int i = 0; i < num2; i++)
				{
					num = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
					if (num >= 0)
					{
						break;
					}
					Thread.Sleep(num3);
				}
				if (num < 0)
				{
					return -13;
				}
				wgTools.WriteLine(string.Format("\r\nBegin Sending Command:\t{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
				string.Format("SSI_FLASH_{0}", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
				int num4 = 4096;
				int num5 = 0;
				int swipeRecordMaxRecIdOfDB = wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
				int num6 = 0;
				int[] array3 = new int[4];
				text = " select f_ReaderID   ";
				text += " FROM   t_b_Reader, t_b_Controller ";
				text += " WHERE t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID ";
				text = text + " AND  t_b_Controller.f_ControllerSN = " + ControllerSN.ToString();
				text += " ORDER BY f_ReaderNO ASC";
				int num7 = 0;
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read() && num7 < 4)
						{
							array3[num7] = (int)sqlDataReader[0];
							num7++;
						}
						sqlDataReader.Close();
					}
				}
				num = 0;
				int num8 = (int)(this.control.runinfo.swipeEndIndex - this.control.runinfo.newRecordsNum);
				if (num8 > 0)
				{
					int swipeLoc = base.GetSwipeLoc(num8);
					wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, (uint)(swipeLoc - swipeLoc % 1024), (uint)(swipeLoc - swipeLoc % 1024 + 1024 - 1));
					array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
					if (array2 == null)
					{
						return -12;
					}
					array = null;
					for (int j = 0; j < num2; j++)
					{
						num = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
						if (num >= 0)
						{
							break;
						}
						Thread.Sleep(num3);
					}
					if (num < 0)
					{
						return -13;
					}
				}
				text = "";
				uint iStartFlashAddr = wgpacketSSI_FLASH_QUERY.iStartFlashAddr;
				bool flag = false;
				uint num9 = 0U;
				wgTools.WriteLine(string.Format("First Page:\t ={0:d}", iStartFlashAddr / 1024U));
				int num10 = num8 - num8 % 204800;
				while (!wgMjControllerSwipeOperate.bStopGetRecord && array != null)
				{
					WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH(array);
					uint num11 = (uint)(base.GetSwipeIndex(wgpacketSSI_FLASH.iStartFlashAddr) + num10);
					for (uint num12 = 0U; num12 < 1024U; num12 += 16U)
					{
						MjRec mjRec = new MjRec(wgpacketSSI_FLASH.ucData, num12, wgpacketSSI_FLASH.iDevSnFrom, num11);
						if (mjRec.CardID == -1L || ((mjRec.bytRecOption == 0 || mjRec.bytRecOption == 255) && mjRec.CardID == 0L))
						{
							if (this.control.runinfo.swipeEndIndex <= num11)
							{
								break;
							}
							num9 += 1U;
							num11 += 1U;
						}
						else
						{
							if (num5 > 0 || (ulong)mjRec.IndexInDataFlash >= (ulong)((long)num8))
							{
								text += " INSERT INTO t_d_SwipeRecord (f_ReadDate, f_CardNO, f_Character, f_InOut, f_Status, f_RecOption, ";
								text += " f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_RecordAll) values (";
								text += wgTools.PrepareStr(mjRec.ReadDate, true, icSwipeRecord.gYMDHMSFormat);
								text = text + "," + mjRec.CardID.ToString();
								text = text + "," + (mjRec.IsPassed ? "1" : "0");
								text = text + "," + (mjRec.IsEnterIn ? "1" : "0");
								text = text + "," + mjRec.bytStatus.ToString();
								text = text + "," + mjRec.bytRecOption.ToString();
								text = text + "," + mjRec.ControllerSN.ToString();
								text = text + "," + array3[(int)(mjRec.ReaderNo - 1)].ToString();
								text = text + "," + mjRec.ReaderNo.ToString();
								text = text + "," + mjRec.IndexInDataFlash.ToString();
								text = text + "," + wgTools.PrepareStrNUnicode(mjRec.ToStringRaw());
								text += ")";
								text += ";";
								num5++;
								num6 = (int)mjRec.IndexInDataFlash;
							}
							num11 += 1U;
						}
					}
					this.DisplayProcessInfo(DoorName, 1, num5);
					if (this.control.runinfo.swipeEndIndex <= num11)
					{
						flag = true;
						break;
					}
					if (text != "")
					{
						using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
						{
							using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
							{
								sqlConnection2.Open();
								num7 = sqlCommand2.ExecuteNonQuery();
							}
						}
						text = "";
					}
					if (flag)
					{
						break;
					}
					wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, wgpacketSSI_FLASH_QUERY.iStartFlashAddr + 1024U, wgpacketSSI_FLASH_QUERY.iStartFlashAddr + 1024U + 1024U - 1U);
					if (wgpacketSSI_FLASH_QUERY.iStartFlashAddr > 8294399U)
					{
						wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, 5017600U, 5018623U);
						num10 += 204800;
					}
					if (wgpacketSSI_FLASH_QUERY.iStartFlashAddr == iStartFlashAddr)
					{
						break;
					}
					wgpacketSSI_FLASH_QUERY.GetNewXid();
					array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
					if (array2 == null)
					{
						break;
					}
					for (int k = 0; k < num2; k++)
					{
						num = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
						if (num >= 0)
						{
							break;
						}
						Thread.Sleep(num3);
					}
					if (num < 0 || --num4 <= 0)
					{
						break;
					}
				}
				if (text != "")
				{
					using (SqlConnection sqlConnection3 = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand3 = new SqlCommand(text, sqlConnection3))
						{
							sqlConnection3.Open();
							num7 = sqlCommand3.ExecuteNonQuery();
						}
					}
					text = "";
				}
				wgTools.WriteLine(string.Format("Last Page:\t ={0:d}", wgpacketSSI_FLASH_QUERY.iStartFlashAddr / 1024U));
				wgTools.WriteLine(string.Format("Got Records:\t Count={0:d}", num5));
				if (num9 > 0U)
				{
					wgAppConfig.wgLog(string.Format("Got Records:\t invalidRecCount={0:d}", num9));
				}
				this.DisplayProcessInfo(DoorName, 2, num5);
				text = " UPDATE t_d_SwipeRecord   ";
				text += " SET t_d_SwipeRecord.f_ConsumerID=t_b_Consumer.f_ConsumerID ";
				text += " FROM   t_d_SwipeRecord,t_b_Consumer ";
				text += " WHERE  t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO  ";
				text = text + " AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString();
				text += " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
				using (SqlConnection sqlConnection4 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand4 = new SqlCommand(text, sqlConnection4))
					{
						sqlConnection4.Open();
						sqlCommand4.CommandTimeout = num5 / 250 + 30;
						num7 = sqlCommand4.ExecuteNonQuery();
					}
				}
				text = " UPDATE t_d_SwipeRecord   ";
				text += " SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID ";
				text += " FROM   t_d_SwipeRecord,t_b_IDCard_Lost ";
				text += " WHERE  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO  ";
				text = text + " AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString();
				text += " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
				using (SqlConnection sqlConnection5 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand5 = new SqlCommand(text, sqlConnection5))
					{
						sqlConnection5.Open();
						sqlCommand5.CommandTimeout = num5 / 250 + 30;
						num7 = sqlCommand5.ExecuteNonQuery();
					}
				}
				num = 0;
				wgTools.WriteLine("Syn Data Info");
				if (num5 > 0)
				{
					for (int l = 0; l < num2; l++)
					{
						num = this.control.GetControllerRunInformationIP(-1);
						if (num >= 0)
						{
							break;
						}
						Thread.Sleep(num3);
					}
					if (num < 0)
					{
						return -13;
					}
					if ((long)(num6 % 204800) >= (long)((ulong)(this.control.runinfo.swipeEndIndex % 204800U)))
					{
						if (this.control.runinfo.swipeEndIndex > 204800U)
						{
							num8 = (int)(this.control.runinfo.swipeEndIndex - this.control.runinfo.swipeEndIndex % 204800U - 204800U + (uint)(num6 % 204800));
						}
						else
						{
							num8 = 0;
						}
					}
					else
					{
						num8 = (int)(this.control.runinfo.swipeEndIndex - this.control.runinfo.swipeEndIndex % 204800U + (uint)(num6 % 204800));
					}
					int num13 = 0;
					while (num13 < num2 && this.control.UpdateLastGetRecordLocationIP((uint)(num8 + 1)) < 0)
					{
						Thread.Sleep(num3);
						num13++;
					}
					base.lastRecordFlashIndex = num8 + 1;
				}
				this.DisplayProcessInfo(DoorName, 3, num5);
				num14 = num5;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString() + "\r\nstrSql=" + text);
				throw ex;
			}
			return num14;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000D6794 File Offset: 0x000D5794
		public int GetSwipeRecords_Realize_P64(int ControllerSN, string IP, int Port, string DoorName, int gotIndex)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.GetSwipeRecords_Acc_P64(ControllerSN, IP, Port, DoorName, gotIndex, -1);
			}
			wgTools.WriteLine("getSwipeRecords Start");
			this.control.ControllerSN = ControllerSN;
			this.control.IP = IP;
			this.control.PORT = Port;
			base.ControllerSN = ControllerSN;
			int num = 0;
			string text = "";
			int num14;
			try
			{
				if (base.wgudp == null)
				{
					base.wgudp = new wgUdpComm();
					Thread.Sleep(300);
				}
				byte[] array = null;
				WGPacketSSI_FLASH_QUERY wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, 5017600U, 5018623U);
				byte[] array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
				if (array2 == null)
				{
					return -12;
				}
				array = null;
				int num2 = 3;
				int num3 = 300;
				for (int i = 0; i < num2; i++)
				{
					num = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
					if (num >= 0)
					{
						break;
					}
					Thread.Sleep(num3);
				}
				if (num < 0)
				{
					return -13;
				}
				wgTools.WriteLine(string.Format("\r\nBegin Sending Command:\t{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
				string.Format("SSI_FLASH_{0}", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
				int num4 = 4096;
				int num5 = 0;
				int swipeRecordMaxRecIdOfDB = wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
				int num6 = 0;
				int[] array3 = new int[4];
				text = " select f_ReaderID   ";
				text += " FROM   t_b_Reader, t_b_Controller ";
				text += " WHERE t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID ";
				text = text + " AND  t_b_Controller.f_ControllerSN = " + ControllerSN.ToString();
				text += " ORDER BY f_ReaderNO ASC";
				int num7 = 0;
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read() && num7 < 4)
						{
							array3[num7] = (int)sqlDataReader[0];
							num7++;
						}
						sqlDataReader.Close();
					}
				}
				num = 0;
				int num8 = (int)(this.control.runinfo.swipeEndIndex - this.control.runinfo.newRecordsNum);
				if (num8 > 0)
				{
					int swipeLoc = base.GetSwipeLoc(num8);
					wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, (uint)(swipeLoc - swipeLoc % 1024), (uint)(swipeLoc - swipeLoc % 1024 + 1024 - 1));
					array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
					if (array2 == null)
					{
						return -12;
					}
					array = null;
					for (int j = 0; j < num2; j++)
					{
						num = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
						if (num >= 0)
						{
							break;
						}
						Thread.Sleep(num3);
					}
					if (num < 0)
					{
						return -13;
					}
				}
				text = "";
				uint iStartFlashAddr = wgpacketSSI_FLASH_QUERY.iStartFlashAddr;
				bool flag = false;
				uint num9 = 0U;
				wgTools.WriteLine(string.Format("First Page:\t ={0:d}", iStartFlashAddr / 1024U));
				int num10 = num8 - num8 % 204800;
				while (!wgMjControllerSwipeOperate.bStopGetRecord && array != null)
				{
					WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH(array);
					uint num11 = (uint)(base.GetSwipeIndex(wgpacketSSI_FLASH.iStartFlashAddr) + num10);
					for (uint num12 = 0U; num12 < 1024U; num12 += 16U)
					{
						MjRec mjRec = new MjRec(wgpacketSSI_FLASH.ucData, num12, wgpacketSSI_FLASH.iDevSnFrom, num11);
						if (mjRec.CardID == -1L || ((mjRec.bytRecOption == 0 || mjRec.bytRecOption == 255) && mjRec.CardID == 0L))
						{
							if (this.control.runinfo.swipeEndIndex <= num11)
							{
								break;
							}
							num9 += 1U;
							num11 += 1U;
						}
						else
						{
							if (num5 > 0 || (ulong)mjRec.IndexInDataFlash >= (ulong)((long)num8))
							{
								text += " INSERT INTO t_d_SwipeRecord (f_ReadDate, f_CardNO, f_Character, f_InOut, f_Status, f_RecOption, ";
								text += " f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_RecordAll) values (";
								text += wgTools.PrepareStr(mjRec.ReadDate, true, icSwipeRecord.gYMDHMSFormat);
								text = text + "," + mjRec.CardID.ToString();
								text = text + "," + (mjRec.IsPassed ? "1" : "0");
								text = text + "," + (mjRec.IsEnterIn ? "1" : "0");
								text = text + "," + mjRec.bytStatus.ToString();
								text = text + "," + mjRec.bytRecOption.ToString();
								text = text + "," + mjRec.ControllerSN.ToString();
								text = text + "," + array3[(int)(mjRec.ReaderNo - 1)].ToString();
								text = text + "," + mjRec.ReaderNo.ToString();
								text = text + "," + mjRec.IndexInDataFlash.ToString();
								text = text + "," + wgTools.PrepareStrNUnicode(mjRec.ToStringRaw());
								text += ")";
								text += ";";
								num5++;
								num6 = (int)mjRec.IndexInDataFlash;
							}
							num11 += 1U;
						}
					}
					this.DisplayProcessInfo(DoorName, 1, num5);
					if (this.control.runinfo.swipeEndIndex <= num11)
					{
						flag = true;
						break;
					}
					if (text != "")
					{
						using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
						{
							using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
							{
								sqlConnection2.Open();
								num7 = sqlCommand2.ExecuteNonQuery();
							}
						}
						text = "";
					}
					if (flag)
					{
						break;
					}
					wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, wgpacketSSI_FLASH_QUERY.iStartFlashAddr + 1024U, wgpacketSSI_FLASH_QUERY.iStartFlashAddr + 1024U + 1024U - 1U);
					if (wgpacketSSI_FLASH_QUERY.iStartFlashAddr > 8294399U)
					{
						wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)ControllerSN, 5017600U, 5018623U);
						num10 += 204800;
					}
					if (wgpacketSSI_FLASH_QUERY.iStartFlashAddr == iStartFlashAddr)
					{
						break;
					}
					wgpacketSSI_FLASH_QUERY.GetNewXid();
					array2 = wgpacketSSI_FLASH_QUERY.ToBytes(base.wgudp.udpPort);
					if (array2 == null)
					{
						break;
					}
					for (int k = 0; k < num2; k++)
					{
						num = base.wgudp.udp_get(array2, 300, wgpacketSSI_FLASH_QUERY.xid, IP, Port, ref array);
						if (num >= 0)
						{
							break;
						}
						Thread.Sleep(num3);
					}
					if (num < 0 || --num4 <= 0)
					{
						break;
					}
				}
				if (text != "")
				{
					using (SqlConnection sqlConnection3 = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand3 = new SqlCommand(text, sqlConnection3))
						{
							sqlConnection3.Open();
							num7 = sqlCommand3.ExecuteNonQuery();
						}
					}
					text = "";
				}
				wgTools.WriteLine(string.Format("Last Page:\t ={0:d}", wgpacketSSI_FLASH_QUERY.iStartFlashAddr / 1024U));
				wgTools.WriteLine(string.Format("Got Records:\t Count={0:d}", num5));
				if (num9 > 0U)
				{
					wgAppConfig.wgLog(string.Format("Got Records:\t invalidRecCount={0:d}", num9));
				}
				this.DisplayProcessInfo(DoorName, 2, num5);
				text = " UPDATE t_d_SwipeRecord   ";
				text += " SET t_d_SwipeRecord.f_ConsumerID=t_b_Consumer.f_ConsumerID ";
				text += " FROM   t_d_SwipeRecord,t_b_Consumer ";
				text += " WHERE  t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO  ";
				text = text + " AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString();
				text += " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
				using (SqlConnection sqlConnection4 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand4 = new SqlCommand(text, sqlConnection4))
					{
						sqlConnection4.Open();
						sqlCommand4.CommandTimeout = num5 / 250 + 30;
						num7 = sqlCommand4.ExecuteNonQuery();
					}
				}
				text = " UPDATE t_d_SwipeRecord   ";
				text += " SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID ";
				text += " FROM   t_d_SwipeRecord,t_b_IDCard_Lost ";
				text += " WHERE  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO  ";
				text = text + " AND  t_d_SwipeRecord.f_RecID >" + swipeRecordMaxRecIdOfDB.ToString();
				text += " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
				using (SqlConnection sqlConnection5 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand5 = new SqlCommand(text, sqlConnection5))
					{
						sqlConnection5.Open();
						sqlCommand5.CommandTimeout = num5 / 250 + 30;
						num7 = sqlCommand5.ExecuteNonQuery();
					}
				}
				num = 0;
				wgTools.WriteLine("Syn Data Info");
				if (num5 > 0)
				{
					for (int l = 0; l < num2; l++)
					{
						num = this.control.GetControllerRunInformationIP(-1);
						if (num >= 0)
						{
							break;
						}
						Thread.Sleep(num3);
					}
					if (num < 0)
					{
						return -13;
					}
					if ((long)(num6 % 204800) >= (long)((ulong)(this.control.runinfo.swipeEndIndex % 204800U)))
					{
						if (this.control.runinfo.swipeEndIndex > 204800U)
						{
							num8 = (int)(this.control.runinfo.swipeEndIndex - this.control.runinfo.swipeEndIndex % 204800U - 204800U + (uint)(num6 % 204800));
						}
						else
						{
							num8 = 0;
						}
					}
					else
					{
						num8 = (int)(this.control.runinfo.swipeEndIndex - this.control.runinfo.swipeEndIndex % 204800U + (uint)(num6 % 204800));
					}
					int num13 = 0;
					while (num13 < num2 && this.control.UpdateLastGetRecordLocationIP((uint)(num8 + 1)) < 0)
					{
						Thread.Sleep(num3);
						num13++;
					}
					base.lastRecordFlashIndex = num8 + 1;
				}
				this.DisplayProcessInfo(DoorName, 3, num5);
				num14 = num5;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString() + "\r\nstrSql=" + text);
				throw ex;
			}
			return num14;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x000D72AC File Offset: 0x000D62AC
		public int GetSwipeRecords_WriteToDB(string DoorName)
		{
			int num = -2;
			string text = "";
			int num2 = 0;
			DbConnection dbConnection = null;
			DbConnection dbConnection2 = null;
			int[] array = new int[4];
			DbCommand dbCommand;
			if (wgAppConfig.IsAccessDB)
			{
				if (dbConnection == null)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
				}
				dbCommand = new OleDbCommand(text, dbConnection as OleDbConnection);
			}
			else
			{
				if (dbConnection == null)
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
				}
				dbConnection2 = new SqlConnection(wgAppConfig.dbConString);
				dbCommand = new SqlCommand(text, dbConnection as SqlConnection);
			}
			dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
			int num3 = 0;
			SqlBulkCopy sqlBulkCopy = null;
			DataTable dataTable = null;
			if (!wgAppConfig.IsAccessDB)
			{
				sqlBulkCopy = new SqlBulkCopy(dbConnection2 as SqlConnection);
				dataTable = new DataTable("t_d_Swipe");
				dataTable.Columns.Add("f_ReadDate", Type.GetType("System.DateTime"));
				dataTable.Columns.Add("f_CardNO", Type.GetType("System.UInt64"));
				dataTable.Columns.Add("f_Character", Type.GetType("System.Byte"));
				dataTable.Columns.Add("f_InOut", Type.GetType("System.Byte"));
				dataTable.Columns.Add("f_Status", Type.GetType("System.Byte"));
				dataTable.Columns.Add("f_RecOption", Type.GetType("System.Byte"));
				dataTable.Columns.Add("f_ControllerSN", Type.GetType("System.UInt32"));
				dataTable.Columns.Add("f_ReaderID", Type.GetType("System.UInt32"));
				dataTable.Columns.Add("f_ReaderNO", Type.GetType("System.Byte"));
				dataTable.Columns.Add("f_RecordFlashLoc", Type.GetType("System.UInt32"));
				dataTable.Columns.Add("f_RecordAll", Type.GetType("System.String"));
				sqlBulkCopy.DestinationTableName = "t_d_SwipeRecord";
				sqlBulkCopy.BulkCopyTimeout = wgAppConfig.dbCommandTimeout;
				sqlBulkCopy.ColumnMappings.Add("f_ReadDate", "f_ReadDate");
				sqlBulkCopy.ColumnMappings.Add("f_CardNO", "f_CardNO");
				sqlBulkCopy.ColumnMappings.Add("f_Character", "f_Character");
				sqlBulkCopy.ColumnMappings.Add("f_InOut", "f_InOut");
				sqlBulkCopy.ColumnMappings.Add("f_Status", "f_Status");
				sqlBulkCopy.ColumnMappings.Add("f_RecOption", "f_RecOption");
				sqlBulkCopy.ColumnMappings.Add("f_ControllerSN", "f_ControllerSN");
				sqlBulkCopy.ColumnMappings.Add("f_ReaderID", "f_ReaderID");
				sqlBulkCopy.ColumnMappings.Add("f_ReaderNO", "f_ReaderNO");
				sqlBulkCopy.ColumnMappings.Add("f_RecordFlashLoc", "f_RecordFlashLoc");
				sqlBulkCopy.ColumnMappings.Add("f_RecordAll", "f_RecordAll");
				dbConnection2.Open();
			}
			try
			{
				dbConnection.Open();
				text = " select f_ReaderID   ";
				text += " FROM   t_b_Reader, t_b_Controller ";
				text += " WHERE t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID ";
				text = text + " AND  t_b_Controller.f_ControllerSN = " + base.ControllerSN.ToString();
				text += " ORDER BY f_ReaderNO ASC";
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read() && num3 < 4)
				{
					array[num3] = (int)dbDataReader[0];
					num3++;
				}
				dbDataReader.Close();
				if (icSwipeRecord.recMaxIdOfDB < 0)
				{
					icSwipeRecord.GetSwipeRecords_MaxRecIdOfDB();
				}
				int num4 = icSwipeRecord.recMaxIdOfDB;
				this.DisplayProcessInfo(DoorName, 4, this.arrSwipes.Count);
				dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
				if (this.arrSwipes.Count > 0)
				{
					MjRec mjRec = new MjRec();
					int num5 = 0;
					text = "";
					for (int i = 0; i < this.arrSwipes.Count; i++)
					{
						mjRec.Update(this.arrSwipes[i] as string);
						num2++;
						if (wgAppConfig.IsAccessDB)
						{
							text = " INSERT INTO t_d_SwipeRecord (f_ReadDate, f_CardNO, f_Character, f_InOut, f_Status, f_RecOption, ";
							text += " f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_RecordAll) values (";
							text += wgTools.PrepareStr(mjRec.ReadDate, true, icSwipeRecord.gYMDHMSFormat);
							text = text + "," + mjRec.CardID.ToString();
							text = text + "," + (mjRec.IsPassed ? "1" : "0");
							text = text + "," + (mjRec.IsEnterIn ? "1" : "0");
							text = text + "," + mjRec.bytStatus.ToString();
							text = text + "," + mjRec.bytRecOption.ToString();
							text = text + "," + mjRec.ControllerSN.ToString();
							text = text + "," + array[(int)(mjRec.ReaderNo - 1)].ToString();
							text = text + "," + mjRec.ReaderNo.ToString();
							text = text + "," + mjRec.IndexInDataFlash.ToString();
							text = text + "," + wgTools.PrepareStrNUnicode(mjRec.ToStringRaw());
							text += ")";
							dbCommand.CommandText = text;
							num3 = dbCommand.ExecuteNonQuery();
							text = "";
							num5++;
						}
						else
						{
							DataRow dataRow = dataTable.NewRow();
							dataRow["f_ReadDate"] = mjRec.ReadDate;
							dataRow["f_CardNO"] = mjRec.CardID;
							dataRow["f_Character"] = (mjRec.IsPassed ? 1 : 0);
							dataRow["f_InOut"] = (mjRec.IsEnterIn ? 1 : 0);
							dataRow["f_Status"] = mjRec.bytStatus;
							dataRow["f_RecOption"] = mjRec.bytRecOption;
							dataRow["f_ControllerSN"] = mjRec.ControllerSN;
							dataRow["f_ReaderID"] = array[(int)(mjRec.ReaderNo - 1)];
							dataRow["f_ReaderNO"] = mjRec.ReaderNo;
							dataRow["f_RecordFlashLoc"] = mjRec.IndexInDataFlash;
							dataRow["f_RecordAll"] = mjRec.ToStringRaw();
							dataTable.Rows.Add(dataRow);
							num5++;
							text = "";
							if (num5 == 10000)
							{
								num5 = 0;
								dataTable.AcceptChanges();
								sqlBulkCopy.BatchSize = dataTable.Rows.Count;
								sqlBulkCopy.WriteToServer(dataTable);
								dataTable.Clear();
							}
						}
						if (num2 % 100 == 0)
						{
							this.DisplayProcessInfo(DoorName, 2, num2);
						}
					}
					if (!wgAppConfig.IsAccessDB)
					{
						dataTable.AcceptChanges();
						if (dataTable.Rows.Count > 0)
						{
							sqlBulkCopy.BatchSize = dataTable.Rows.Count;
							sqlBulkCopy.WriteToServer(dataTable);
							dataTable.Clear();
						}
						wgTools.WriteLine("bulkCopy.Close() ");
						sqlBulkCopy.Close();
						dataTable.Dispose();
						dbConnection2.Close();
					}
				}
				this.DisplayProcessInfo(DoorName, 2, num2);
				dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
				int num6 = 0;
				if (wgAppConfig.IsAccessDB)
				{
					text = " UPDATE t_d_SwipeRecord   ";
					text += " INNER JOIN   t_b_Consumer ";
					text += " ON  (t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO  ";
					text = text + " AND  t_d_SwipeRecord.f_RecID >" + num4.ToString();
					text += " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0)))) ";
					text += " )";
					text += " SET t_d_SwipeRecord.f_ConsumerID=  t_b_Consumer.f_ConsumerID ";
				}
				else
				{
					text = " UPDATE t_d_SwipeRecord   ";
					text += " SET t_d_SwipeRecord.f_ConsumerID=t_b_Consumer.f_ConsumerID ";
					text += " FROM   t_d_SwipeRecord,t_b_Consumer ";
					text += " WHERE  t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO  ";
					text = text + " AND  t_d_SwipeRecord.f_RecID >" + num4.ToString();
					text += " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
				}
				dbCommand.CommandText = text;
				num6 += dbCommand.ExecuteNonQuery();
				if (wgAppConfig.IsAccessDB)
				{
					text = " UPDATE t_d_SwipeRecord   ";
					text += " INNER JOIN   t_b_IDCard_Lost ";
					text += " ON (  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO  ";
					text = text + " AND  t_d_SwipeRecord.f_RecID >" + num4.ToString();
					text += " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0)))) ";
					text += " )";
					text += " SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID ";
				}
				else
				{
					text = " UPDATE t_d_SwipeRecord   ";
					text += " SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID ";
					text += " FROM   t_d_SwipeRecord,t_b_IDCard_Lost ";
					text += " WHERE  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO  ";
					text = text + " AND  t_d_SwipeRecord.f_RecID >" + num4.ToString();
					text += " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
				}
				dbCommand.CommandText = text;
				num6 += dbCommand.ExecuteNonQuery();
				num = this.arrSwipes.Count;
				dbConnection.Close();
				if (num6 > num2)
				{
					icSwipeRecord.GetSwipeRecords_MaxRecIdOfDB();
				}
				else
				{
					icSwipeRecord.recMaxIdOfDB += num2;
				}
				dbCommand.Dispose();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(string.Concat(new object[]
				{
					ex.ToString(),
					"\r\nrectotal=",
					num2,
					"\r\nstrSql=\r\n",
					text
				}));
			}
			return num;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x000D7C48 File Offset: 0x000D6C48
		public int GetSwipeRecords_WriteToDB22(string DoorName)
		{
			int num = -2;
			string text = "";
			int num2 = 0;
			DbConnection dbConnection = null;
			int[] array = new int[4];
			DbCommand dbCommand;
			if (wgAppConfig.IsAccessDB)
			{
				if (dbConnection == null)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
				}
				dbCommand = new OleDbCommand(text, dbConnection as OleDbConnection);
			}
			else
			{
				if (dbConnection == null)
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
				}
				dbCommand = new SqlCommand(text, dbConnection as SqlConnection);
			}
			dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
			int num3 = 0;
			try
			{
				dbConnection.Open();
				text = " select f_ReaderID   ";
				text += " FROM   t_b_Reader, t_b_Controller ";
				text += " WHERE t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID ";
				text = text + " AND  t_b_Controller.f_ControllerSN = " + base.ControllerSN.ToString();
				text += " ORDER BY f_ReaderNO ASC";
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read() && num3 < 4)
				{
					array[num3] = (int)dbDataReader[0];
					num3++;
				}
				dbDataReader.Close();
				if (icSwipeRecord.recMaxIdOfDB < 0)
				{
					icSwipeRecord.GetSwipeRecords_MaxRecIdOfDB();
				}
				int num4 = icSwipeRecord.recMaxIdOfDB;
				this.DisplayProcessInfo(DoorName, 4, this.arrSwipes.Count);
				dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
				if (this.arrSwipes.Count > 0)
				{
					MjRec mjRec = new MjRec();
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder = new StringBuilder();
					text = "";
					for (int i = 0; i < this.arrSwipes.Count; i++)
					{
						mjRec.Update(this.arrSwipes[i] as string);
						text = " INSERT INTO t_d_SwipeRecord (f_ReadDate, f_CardNO, f_Character, f_InOut, f_Status, f_RecOption, ";
						text += " f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_RecordAll) values (";
						text += wgTools.PrepareStr(mjRec.ReadDate, true, icSwipeRecord.gYMDHMSFormat);
						text = text + "," + mjRec.CardID.ToString();
						text = text + "," + (mjRec.IsPassed ? "1" : "0");
						text = text + "," + (mjRec.IsEnterIn ? "1" : "0");
						text = text + "," + mjRec.bytStatus.ToString();
						text = text + "," + mjRec.bytRecOption.ToString();
						text = text + "," + mjRec.ControllerSN.ToString();
						text = text + "," + array[(int)(mjRec.ReaderNo - 1)].ToString();
						text = text + "," + mjRec.ReaderNo.ToString();
						text = text + "," + mjRec.IndexInDataFlash.ToString();
						text = text + "," + wgTools.PrepareStrNUnicode(mjRec.ToStringRaw());
						text += ")";
						num2++;
						if (wgAppConfig.IsAccessDB)
						{
							dbCommand.CommandText = text;
							num3 = dbCommand.ExecuteNonQuery();
							text = "";
							num5++;
						}
						else
						{
							stringBuilder.Append(text);
							stringBuilder.Append(";");
							num5++;
							num6 += text.Length;
							text = "";
							if (num5 == this.MaxSwipeByOneSql)
							{
								if (num6 > num7)
								{
									num7 = num6;
								}
								dbCommand.CommandText = stringBuilder.ToString();
								num3 = dbCommand.ExecuteNonQuery();
								stringBuilder.Remove(0, stringBuilder.Length);
								num5 = 0;
								num6 = 0;
							}
						}
						if (num2 % 100 == 0)
						{
							this.DisplayProcessInfo(DoorName, 2, num2);
						}
					}
					if (!wgAppConfig.IsAccessDB && stringBuilder.Length > 0)
					{
						dbCommand.CommandText = stringBuilder.ToString();
						num3 = dbCommand.ExecuteNonQuery();
					}
				}
				this.DisplayProcessInfo(DoorName, 2, num2);
				dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
				int num8 = 0;
				if (wgAppConfig.IsAccessDB)
				{
					text = " UPDATE t_d_SwipeRecord   ";
					text += " INNER JOIN   t_b_Consumer ";
					text += " ON  (t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO  ";
					text = text + " AND  t_d_SwipeRecord.f_RecID >" + num4.ToString();
					text += " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0)))) ";
					text += " )";
					text += " SET t_d_SwipeRecord.f_ConsumerID=  t_b_Consumer.f_ConsumerID ";
				}
				else
				{
					text = " UPDATE t_d_SwipeRecord   ";
					text += " SET t_d_SwipeRecord.f_ConsumerID=t_b_Consumer.f_ConsumerID ";
					text += " FROM   t_d_SwipeRecord,t_b_Consumer ";
					text += " WHERE  t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO  ";
					text = text + " AND  t_d_SwipeRecord.f_RecID >" + num4.ToString();
					text += " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
				}
				dbCommand.CommandText = text;
				num8 += dbCommand.ExecuteNonQuery();
				if (wgAppConfig.IsAccessDB)
				{
					text = " UPDATE t_d_SwipeRecord   ";
					text += " INNER JOIN   t_b_IDCard_Lost ";
					text += " ON (  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO  ";
					text = text + " AND  t_d_SwipeRecord.f_RecID >" + num4.ToString();
					text += " AND (  ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0) OR ( ((((f_RecOption - (f_RecOption mod 2)) / 2) Mod 4) = 3) and (((((f_Status-(f_Status mod 128))/128) Mod 2)=0) and ((((f_Status-(f_Status mod 16))/16) MOD 2)=0)))) ";
					text += " )";
					text += " SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID ";
				}
				else
				{
					text = " UPDATE t_d_SwipeRecord   ";
					text += " SET t_d_SwipeRecord.f_ConsumerID=t_b_IDCard_Lost.f_ConsumerID ";
					text += " FROM   t_d_SwipeRecord,t_b_IDCard_Lost ";
					text += " WHERE  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO  ";
					text = text + " AND  t_d_SwipeRecord.f_RecID >" + num4.ToString();
					text += " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
				}
				dbCommand.CommandText = text;
				num8 += dbCommand.ExecuteNonQuery();
				num = this.arrSwipes.Count;
				dbConnection.Close();
				if (num8 > num2)
				{
					icSwipeRecord.GetSwipeRecords_MaxRecIdOfDB();
				}
				else
				{
					icSwipeRecord.recMaxIdOfDB += num2;
				}
				dbCommand.Dispose();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(string.Concat(new object[]
				{
					ex.ToString(),
					"\r\nrectotal=",
					num2,
					"\r\nstrSql=\r\n",
					text
				}));
			}
			return num;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x000D8230 File Offset: 0x000D7230
		public int GetSwipeRecordsByDoorName(string DoorName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.GetSwipeRecordsByDoorName_Acc(DoorName);
			}
			int num = -1;
			string text = " SELECT f_ControllerSN, f_IP, f_Port";
			text = text + " FROM t_b_Controller a, t_b_Door b WHERE a.f_ControllerID = b.f_ControllerID AND b.f_DoorName =  " + wgTools.PrepareStrNUnicode(DoorName);
			int num2 = 0;
			string text2 = "";
			int num3 = 0;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						num2 = (int)sqlDataReader["f_ControllerSN"];
						text2 = wgTools.SetObjToStr(sqlDataReader["f_IP"]);
						num3 = (int)sqlDataReader["f_Port"];
					}
					sqlDataReader.Close();
				}
			}
			if (num2 > 0)
			{
				num = this.GetSwipeRecords(num2, text2, num3, DoorName);
			}
			return num;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x000D8328 File Offset: 0x000D7328
		public int GetSwipeRecordsByDoorName_Acc(string DoorName)
		{
			int num = -1;
			string text = " SELECT f_ControllerSN, f_IP, f_Port";
			text = text + " FROM t_b_Controller a, t_b_Door b WHERE a.f_ControllerID = b.f_ControllerID AND b.f_DoorName =  " + wgTools.PrepareStrNUnicode(DoorName);
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					if (oleDbDataReader.Read())
					{
						num = this.GetSwipeRecords((int)oleDbDataReader["f_ControllerSN"], wgTools.SetObjToStr(oleDbDataReader["f_IP"]), (int)oleDbDataReader["f_Port"], DoorName);
					}
					oleDbDataReader.Close();
				}
			}
			return num;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x000D83F0 File Offset: 0x000D73F0
		public int GetSwipeRecordsByDoorNameMultithread(int ControllerSN, string IP, int Port, string DoorName)
		{
			this.arrSwipes = null;
			this.arrSwipes = new ArrayList();
			int num = -1;
			if (this.bStopComm())
			{
				return num;
			}
			return this.GetSwipeRecords_MultiThread(ControllerSN, IP, Port, DoorName);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x000D8426 File Offset: 0x000D7426
		private void log(string info)
		{
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x000D8428 File Offset: 0x000D7428
		private void LongToBytes(ref byte[] outBytes, int startIndex, long val)
		{
			Array.Copy(BitConverter.GetBytes(val), 0, outBytes, startIndex, 4);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x000D843A File Offset: 0x000D743A
		public int ReadSwipeRecIDMax()
		{
			return wgAppConfig.GetSwipeRecordMaxRecIdOfDB();
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x000D8441 File Offset: 0x000D7441
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x000D8449 File Offset: 0x000D7449
		public bool bDisplayProcess
		{
			get
			{
				return this._bDisplayProcess;
			}
			set
			{
				this._bDisplayProcess = value;
			}
		}

		// Token: 0x04000D43 RID: 3395
		private bool _bDisplayProcess;

		// Token: 0x04000D44 RID: 3396
		private ArrayList arrSwipes = new ArrayList();

		// Token: 0x04000D45 RID: 3397
		private icController control = new icController();

		// Token: 0x04000D46 RID: 3398
		private static string gYMDHMSFormat = "yyyy-MM-dd HH:mm:ss";

		// Token: 0x04000D47 RID: 3399
		private int MaxSwipeByOneSql = 1000;

		// Token: 0x04000D48 RID: 3400
		private static int recMaxIdOfDB = -1;

		// Token: 0x04000D49 RID: 3401
		public static long tickOperation = 0L;

		// Token: 0x04000D4A RID: 3402
		public static long tickOperationStart = 0L;

		// Token: 0x04000D4B RID: 3403
		private static Queue WriteDBPQueue = new Queue();
	}
}
