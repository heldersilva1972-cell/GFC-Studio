using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using JRO;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM
{
	// Token: 0x02000359 RID: 857
	internal static class Program
	{
		// Token: 0x06001AF5 RID: 6901 RVA: 0x0021F09C File Offset: 0x0021E09C
		public static void checkCardNO(float dbversion)
		{
			if (!Program.wgAppConfigIsAccessDB())
			{
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						DbConnection dbConnection = new SqlConnection(wgAppConfig.dbConString);
						DbCommand dbCommand = new SqlCommand("", dbConnection as SqlConnection);
						dbConnection.Open();
						dbCommand.CommandText = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME='t_b_Consumer' and COLUMN_NAME = 'f_CardNO' ";
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						bool flag = false;
						if (dbDataReader.Read())
						{
							string text = wgTools.SetObjToStr(dbDataReader[0]);
							if (!string.IsNullOrEmpty(text) && text.Equals("nvarchar"))
							{
								flag = true;
							}
						}
						dbDataReader.Close();
						if (flag)
						{
							wgAppConfig.wgLog("Update t_b_Consumer  f_CardNO from nvarchar to bigint");
							Cursor.Current = Cursors.WaitCursor;
							dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							string text2 = "BEGIN TRANSACTION";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							text2 = "CREATE TABLE [dbo].[t_b_Consumer_BK] (\r\n [f_ConsumerID] [int] NOT NULL ,\r\n [f_ConsumerNO] [nvarchar] (20)  NOT NULL ,\r\n [f_ConsumerName] [nvarchar] (50)  NOT NULL ,\r\n [f_CardNO] [bigint] NULL ,\r\n [f_GroupID] [int] NOT NULL ,\r\n [f_AttendEnabled] [tinyint] NOT NULL ,\r\n [f_ShiftEnabled] [tinyint] NOT NULL ,\r\n [f_DoorEnabled] [tinyint] NOT NULL ,\r\n [f_BeginYMD] [datetime] NOT NULL ,\r\n [f_EndYMD] [datetime] NOT NULL ,\r\n [f_PIN] [int] NOT NULL,\r\n[f_PrivilegeTypeID] [int] NOT NULL \r\n) ON [PRIMARY]";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							text2 = " INSERT INTO t_b_Consumer_bk ([f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID]) SELECT  [f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      , case when isnumeric(f_CardNO)=1 then   CONVERT(BIGINT, f_CardNO) else NULL end    ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID] FROM [t_b_Consumer] ";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = "COMMIT TRANSACTION";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = " TRUNCATE TABLE t_b_Consumer";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = "ALTER TABLE t_b_Consumer  ALTER COLUMN   [f_CardNO] [bigint] NULL ";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = " SET IDENTITY_INSERT [t_b_Consumer] ON ";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = " INSERT INTO t_b_Consumer ([f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID]) SELECT  [f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      , [f_CardNO]    ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID] FROM [t_b_Consumer_bk] ";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = " SET IDENTITY_INSERT [t_b_Consumer] OFF ";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = "DROP TABLE t_b_Consumer_bk";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							wgAppConfig.wgLog("Update t_b_Consumer  f_CardNO from nvarchar to bigint----OK");
							Cursor.Current = Cursors.Default;
						}
						dbCommand.CommandText = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME='t_d_SwipeRecord' and COLUMN_NAME = 'f_CardNO' ";
						dbDataReader = dbCommand.ExecuteReader();
						flag = false;
						if (dbDataReader.Read())
						{
							string text = wgTools.SetObjToStr(dbDataReader[0]);
							if (!string.IsNullOrEmpty(text) && text.Equals("nvarchar"))
							{
								flag = true;
							}
						}
						dbDataReader.Close();
						if (flag)
						{
							Cursor.Current = Cursors.WaitCursor;
							wgAppConfig.wgLog("Update t_d_SwipeRecord  f_CardNO from nvarchar to bigint  start");
							dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							string text2 = "BEGIN TRANSACTION";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							text2 = "CREATE TABLE [dbo].[t_d_SwipeRecord_bk] (\r\n [f_RecID] [int] NOT NULL ,\r\n [f_ReadDate] [datetime] NOT NULL ,\r\n [f_CardNO] [bigint] NOT NULL ,\r\n [f_ConsumerID] [int] NOT NULL ,\r\n [f_Character] [tinyint] NOT NULL ,\r\n [f_InOut] [tinyint] NOT NULL ,\r\n [f_Status] [tinyint] NOT NULL ,\r\n [f_RecOption] [tinyint] NOT NULL ,\r\n [f_ControllerSN] [int] NOT NULL ,\r\n [f_ReaderID] [int] NOT NULL ,\r\n [f_ReaderNO] [tinyint] NOT NULL ,\r\n [f_RecordFlashLoc] [int] NOT NULL ,\r\n [f_RecordAll] [nvarchar] (48) NOT NULL ,\r\n [f_Modified] [datetime] NOT NULL \r\n) ON [PRIMARY]";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = " INSERT INTO t_d_SwipeRecord_bk([f_RecID],[f_ReadDate],[f_CardNO],[f_ConsumerID] ,[f_Character]  ,[f_InOut] ,[f_Status] ,[f_RecOption] ,[f_ControllerSN]  ,[f_ReaderID] ,[f_ReaderNO] ,[f_RecordFlashLoc]  ,[f_RecordAll]  ,[f_Modified]) SELECT [f_RecID],[f_ReadDate] ,case when isnumeric(f_CardNO)=1 then   CONVERT(BIGINT, f_CardNO) else NULL end    ,[f_ConsumerID] ,[f_Character]  ,[f_InOut] ,[f_Status] ,[f_RecOption] ,[f_ControllerSN]  ,[f_ReaderID] ,[f_ReaderNO] ,[f_RecordFlashLoc]  ,[f_RecordAll]  ,[f_Modified] FROM [t_d_SwipeRecord] ";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = "COMMIT TRANSACTION";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = " TRUNCATE TABLE t_d_SwipeRecord";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = "ALTER TABLE t_d_SwipeRecord  ALTER COLUMN   [f_CardNO] [bigint] NULL ";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = " SET IDENTITY_INSERT [t_d_SwipeRecord] ON ";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = " INSERT INTO t_d_SwipeRecord([f_RecID],[f_ReadDate],[f_CardNO]  ,[f_ConsumerID] ,[f_Character]  ,[f_InOut] ,[f_Status] ,[f_RecOption] ,[f_ControllerSN]  ,[f_ReaderID] ,[f_ReaderNO] ,[f_RecordFlashLoc]  ,[f_RecordAll]  ,[f_Modified]) SELECT [f_RecID],[f_ReadDate] ,f_CardNO  ,[f_ConsumerID] ,[f_Character]  ,[f_InOut] ,[f_Status] ,[f_RecOption] ,[f_ControllerSN]  ,[f_ReaderID] ,[f_ReaderNO] ,[f_RecordFlashLoc]  ,[f_RecordAll]  ,[f_Modified] FROM [t_d_SwipeRecord_bk] ";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = " SET IDENTITY_INSERT [t_d_SwipeRecord] OFF ";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							text2 = "DROP TABLE t_d_SwipeRecord_bk";
							dbCommand.CommandText = text2;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text2);
							wgAppConfig.wgLog("Update t_d_SwipeRecord  f_CardNO from nvarchar to bigint  --OK");
							Cursor.Current = Cursors.Default;
						}
						dbConnection.Close();
						dbCommand.Dispose();
					}
				}
				catch (Exception ex)
				{
					Program.wgToolsWgDebugWrite(ex.ToString());
				}
			}
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0021F434 File Offset: 0x0021E434
		public static int dbConnectionCheck()
		{
			int num = -1;
			string text = Program.descDbConnection(wgAppConfig.GetKeyVal("dbConnection"));
			if (!string.IsNullOrEmpty(text))
			{
				wgAppConfig.dbConString = text;
			}
			if (string.IsNullOrEmpty(text))
			{
				wgAppConfig.IsAccessDB = true;
				wgAppConfig.dbConString = Program.g_cnStrAcc;
			}
			else if (text.ToUpper().IndexOf("Data Source".ToUpper()) >= 0 && text.ToUpper().IndexOf(".OLEDB".ToUpper()) < 0)
			{
				wgAppConfig.IsAccessDB = false;
			}
			else
			{
				wgAppConfig.IsAccessDB = true;
			}
			if (wgAppConfig.IsAccessDB)
			{
				return Program.dbConnectionCheck_Acc();
			}
			bool flag = true;
			do
			{
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				try
				{
					if (sqlConnection.State != ConnectionState.Open)
					{
						sqlConnection.Open();
					}
					string text2 = "SELECT * FROM t_a_SystemParam WHERE f_NO = 12";
					using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection))
					{
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						if (sqlDataReader.Read())
						{
							num = 1;
						}
						sqlDataReader.Close();
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				finally
				{
					sqlConnection.Dispose();
				}
				DateTime dateTime = DateTime.Now.AddMilliseconds((double)(-(double)Environment.TickCount));
				if (DateTime.Now > dateTime.AddSeconds(300.0))
				{
					break;
				}
				if (flag)
				{
					flag = false;
					wgAppConfig.wgLog("Failed To Connect SQL Server: dbConString=" + wgAppConfig.dbConString);
				}
				Application.DoEvents();
				Thread.Sleep(500);
			}
			while (num <= 0);
			if (num > 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x0021F5D0 File Offset: 0x0021E5D0
		public static int dbConnectionCheck_Acc()
		{
			int num = -1;
			if (!wgAppConfig.IsAccessDB)
			{
				return num;
			}
			OleDbConnection oleDbConnection = null;
			bool flag = false;
			bool flag2 = false;
			try
			{
				oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				if (oleDbConnection.State != ConnectionState.Open)
				{
					oleDbConnection.Open();
				}
				string text = "SELECT * FROM t_a_SystemParam WHERE f_NO = 12";
				string text2 = "";
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					if (oleDbDataReader.Read())
					{
						text2 = wgTools.SetObjToStr(oleDbDataReader["f_Value"]);
						num = 1;
						flag = true;
					}
					oleDbDataReader.Close();
					if (num == 1)
					{
						text = string.Format("UPDATE t_a_SystemParam  SET f_Value = {0} WHERE f_NO = 12", wgTools.PrepareStrNUnicode(text2));
						oleDbCommand.CommandText = text;
						if (oleDbCommand.ExecuteNonQuery() <= 0)
						{
							num = 0;
						}
						else
						{
							flag2 = true;
						}
					}
				}
			}
			catch (OleDbException ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				if (flag && !flag2 && ex.ErrorCode == -2147467259)
				{
					XMessageBox.Show(CommonStr.strAccessDatabaseOnlyReadNotWrite, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					Thread.Sleep(500);
					Environment.Exit(0);
					Process.GetCurrentProcess().Kill();
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			finally
			{
				if (oleDbConnection != null)
				{
					oleDbConnection.Dispose();
				}
			}
			if (num <= 0)
			{
				try
				{
					FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\" + wgAppConfig.accessDbName + ".mdb");
					if (fileInfo.Exists)
					{
						try
						{
							if ((fileInfo.Attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
							{
								fileInfo.Attributes &= ~FileAttributes.ReadOnly;
							}
							goto IL_0265;
						}
						catch (Exception)
						{
							goto IL_0265;
						}
						goto IL_0182;
						IL_0265:
						goto IL_028B;
					}
					IL_0182:
					FileInfo fileInfo2 = new FileInfo(string.Format(Application.StartupPath + "\\PHOTO\\{0}.mdbAA", wgAppConfig.accessDbName));
					if (!fileInfo2.Exists)
					{
						FileInfo fileInfo3 = new FileInfo(string.Format(Application.StartupPath + "\\{0}.mdb.gz", wgAppConfig.accessDbName));
						if (!fileInfo3.Exists)
						{
							using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(fileInfo3.FullName, FileMode.Create)))
							{
								binaryWriter.Write(Resources.iCCard3000_mdbA);
							}
						}
						wgTools.Decompress(fileInfo3);
						fileInfo3.Delete();
						return 1;
					}
					fileInfo2.CopyTo(Application.StartupPath + "\\" + wgAppConfig.accessDbName + ".mdb", true);
					fileInfo2 = new FileInfo(Application.StartupPath + "\\" + wgAppConfig.accessDbName + ".mdb");
					fileInfo2.Attributes = FileAttributes.Archive;
					return 1;
				}
				catch (Exception ex3)
				{
					wgTools.WgDebugWrite(ex3.ToString(), new object[] { EventLogEntryType.Error });
				}
			}
			IL_028B:
			if (num > 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x0021F920 File Offset: 0x0021E920
		public static void deleteOldAviPhoto()
		{
			try
			{
				int num = 0;
				int.TryParse(wgAppConfig.GetKeyVal("KEY_Video_DELETE_AVI_TIME_FREQ_MONTH"), out num);
				if (num > 0)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(wgAppConfig.Path4AviJpg());
					DateTime now = DateTime.Now;
					DateTime creationTime = new DateTime(2019, 3, 6, 0, 0, 0, 0);
					DateTime dateTime = new DateTime(2010, 1, 1, 0, 0, 0, 0);
					try
					{
						if (!DateTime.TryParse(wgAppConfig.GetKeyVal("KEY_Video_DELETE_AVI_TIME"), out dateTime))
						{
							dateTime = new DateTime(2010, 1, 1, 0, 0, 0, 0);
						}
					}
					catch
					{
					}
					if (now >= dateTime.AddMonths(1) && now >= creationTime)
					{
						foreach (FileInfo fileInfo in directoryInfo.GetFiles())
						{
							if (fileInfo.CreationTime > creationTime)
							{
								creationTime = fileInfo.CreationTime;
							}
						}
						if (creationTime.AddMonths(1) > now)
						{
							int num2 = int.Parse(now.AddMonths(-num).ToString("yyyyMM"));
							int num3 = 0;
							foreach (FileInfo fileInfo2 in directoryInfo.GetFiles())
							{
								if (fileInfo2.Name.Length >= 10)
								{
									int num4 = 99999999;
									int.TryParse(fileInfo2.Name.Substring(0, 6), out num4);
									if (num2 > num4)
									{
										fileInfo2.Delete();
										num3++;
									}
								}
							}
							if (num3 > 0)
							{
								wgAppConfig.wgLog(string.Format("auto delete avi and jpg files: {0} files", num3));
							}
							wgAppConfig.UpdateKeyVal("KEY_Video_DELETE_AVI_TIME", now.ToString("yyyy-MM-dd"));
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0021FB14 File Offset: 0x0021EB14
		public static string descDbConnection(string strDbConnection)
		{
			string text = strDbConnection;
			try
			{
				text = Program.Dpt4Database(strDbConnection);
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x0021FB40 File Offset: 0x0021EB40
		public static string Dpt4Database(string StrInput)
		{
			string text = "";
			try
			{
				if (Program.Key4Database == null)
				{
					IntPtr intPtr = Marshal.AllocHGlobal(16);
					IntPtr intPtr2 = Marshal.AllocHGlobal(16);
					Program.getKDB(intPtr);
					Program.getIVDB(intPtr2);
					Program.Key4Database = new byte[16];
					Marshal.Copy(intPtr, Program.Key4Database, 0, 16);
					Program.IV4Database = new byte[16];
					Marshal.Copy(intPtr2, Program.IV4Database, 0, 16);
					Marshal.FreeHGlobal(intPtr);
					Marshal.FreeHGlobal(intPtr2);
				}
				byte[] array = Convert.FromBase64String(wgTools.SetObjToStr(StrInput));
				if (array.Length <= 0)
				{
					return text;
				}
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
					{
						CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(Program.Key4Database, Program.IV4Database), CryptoStreamMode.Write);
						cryptoStream.Write(array, 0, array.Length);
						cryptoStream.FlushFinalBlock();
						text = Encoding.Default.GetString(memoryStream.ToArray());
					}
				}
			}
			catch
			{
				throw;
			}
			return text;
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0021FC6C File Offset: 0x0021EC6C
		public static string Ept4Database(string StrInput)
		{
			string text = "";
			try
			{
				if (Program.Key4Database == null)
				{
					IntPtr intPtr = Marshal.AllocHGlobal(16);
					IntPtr intPtr2 = Marshal.AllocHGlobal(16);
					Program.getKDB(intPtr);
					Program.getIVDB(intPtr2);
					Program.Key4Database = new byte[16];
					Marshal.Copy(intPtr, Program.Key4Database, 0, 16);
					Program.IV4Database = new byte[16];
					Marshal.Copy(intPtr2, Program.IV4Database, 0, 16);
					Marshal.FreeHGlobal(intPtr);
					Marshal.FreeHGlobal(intPtr2);
				}
				byte[] bytes = Encoding.Default.GetBytes(wgTools.SetObjToStr(StrInput));
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
					{
						CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(Program.Key4Database, Program.IV4Database), CryptoStreamMode.Write);
						cryptoStream.Write(bytes, 0, bytes.Length);
						cryptoStream.FlushFinalBlock();
						text = Convert.ToBase64String(memoryStream.ToArray());
					}
				}
			}
			catch
			{
				throw;
			}
			return text;
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0021FD88 File Offset: 0x0021ED88
		private static string getConSql(string dbName)
		{
			string text = string.Format("data source={0};initial catalog={1};integrated security=SSPI;persist security info=True", "(local)", dbName);
			if (Program.bSqlExress)
			{
				text = string.Format("data source={0};initial catalog={1};integrated security=SSPI;persist security info=True", "(local)\\sqlexpress", dbName);
			}
			return text;
		}

		// Token: 0x06001AFD RID: 6909
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int getIVDB(IntPtr k);

		// Token: 0x06001AFE RID: 6910
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int getKDB(IntPtr k);

		// Token: 0x06001AFF RID: 6911 RVA: 0x0021FDC0 File Offset: 0x0021EDC0
		public static ulong getN3000Crc()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 30000000;
			byte[] array = new byte[4];
			FileStream fileStream = new FileStream(Application.StartupPath + "\\N3000.exe", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using (BinaryReader binaryReader = new BinaryReader(fileStream))
			{
				try
				{
					while (num3-- > 0)
					{
						binaryReader.ReadByte();
						num2++;
					}
				}
				catch
				{
				}
			}
			FileStream fileStream2 = new FileStream(Application.StartupPath + "\\N3000.exe", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using (BinaryReader binaryReader2 = new BinaryReader(fileStream2))
			{
				array = binaryReader2.ReadBytes(num2);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == 91 && array[i + 1] == 165 && array[i + 2] == 24 && array[i + 3] == 129)
					{
						array[i - 1] = 0;
						break;
					}
				}
				num = wgCRC.CRC_16_IBM(array.Length, array);
			}
			return (ulong)(4294967296L + (long)num);
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x0021FEE8 File Offset: 0x0021EEE8
		public static void getNewSoftware()
		{
			try
			{
				comMjSpecialUpdate.updateMjSpecialSoftware();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			try
			{
				wgMail.sendMailOnce2018();
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x0021FF48 File Offset: 0x0021EF48
		private static string getOperatorPrivilegeInsertSql(int functionId, string functionName, string displayName)
		{
			return string.Format("Insert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl)  SELECT t_s_OperatorPrivilege.f_OperatorID,{0} as f_FunctionID,{1} as f_FunctionName ,{2} as f_FunctionDisplayName,0 as f_ReadOnly,1 as f_FullControl FROM  t_s_OperatorPrivilege WHERE t_s_OperatorPrivilege.f_functionID = 1  AND t_s_OperatorPrivilege.f_OperatorID NOT IN (SELECT t_s_OperatorPrivilege.f_OperatorID  FROM  t_s_OperatorPrivilege  WHERE t_s_OperatorPrivilege.f_functionID = {0} )", functionId, Program.wgToolsPrepareStr(functionName), Program.wgToolsPrepareStr(displayName));
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x0021FF68 File Offset: 0x0021EF68
		public static void GlobalExceptionHandler(object sender, ThreadExceptionEventArgs e)
		{
			wgTools.WgDebugWrite(e.Exception.ToString(), new object[0]);
			try
			{
				wgAppConfig.wgLog(e.Exception.ToString(), EventLogEntryType.Error, null);
				dfrmShowError dfrmShowError = new dfrmShowError();
				try
				{
					dfrmShowError.StartPosition = FormStartPosition.Manual;
					dfrmShowError.Location = new Point(0, 0);
					dfrmShowError.errInfo = e.Exception.ToString();
					dfrmShowError.ShowDialog();
				}
				catch (Exception)
				{
				}
				dfrmShowError.Dispose();
				if (!(Program.expStrDayHour == DateTime.Now.ToString("yyyy-MM-dd HH")))
				{
					Program.expStrDayHour = DateTime.Now.ToString("yyyy-MM-dd HH");
					Program.expcount = 1;
				}
				if (Program.expcount >= 3)
				{
					Thread.CurrentThread.Abort();
				}
				else
				{
					Program.expcount++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x00220058 File Offset: 0x0021F058
		public static void localize()
		{
			string keyVal = wgAppConfig.GetKeyVal("Language");
			if (keyVal != "" && !wgAppConfig.IsChineseSet(keyVal))
			{
				wgTools.DisplayFormat_DateYMDHMSWeek.Replace("dddd", "ddd");
				wgTools.DisplayFormat_DateYMDWeek.Replace("dddd", "ddd");
			}
			wgAppConfig.CultureInfoStr = keyVal;
			try
			{
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DisplayFormat_DateYMD")) && wgTools.IsValidDateTimeFormat(wgAppConfig.GetKeyVal("DisplayFormat_DateYMD")))
			{
				wgTools.DisplayFormat_DateYMD = wgAppConfig.GetKeyVal("DisplayFormat_DateYMD");
			}
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDWeek")) && wgTools.IsValidDateTimeFormat(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDWeek")))
			{
				wgTools.DisplayFormat_DateYMDWeek = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDWeek");
			}
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMS")) && wgTools.IsValidDateTimeFormat(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMS")))
			{
				wgTools.DisplayFormat_DateYMDHMS = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMS");
			}
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMSWeek")) && wgTools.IsValidDateTimeFormat(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMSWeek")))
			{
				wgTools.DisplayFormat_DateYMDHMSWeek = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMSWeek");
			}
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x002201B4 File Offset: 0x0021F1B4
		[STAThread]
		private static void Main(string[] cmdArgs)
		{
			wgTools.gPTC = "Qmqea4wQvtaDGdyWDbfbUg==";
			wgAppConfig.ProductTypeOfApp = "AccessControl";
			wgAppConfig.gRestart = true;
			wgAppConfig.gRestart = false;
			Directory.SetCurrentDirectory(Application.StartupPath);
			wgAppConfig.gRestart = false;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.ThreadException += Program.GlobalExceptionHandler;
			if (Program.n3k_comm_dll_Check() <= 0)
			{
				XMessageBox.Show(CommonStr.strLoadLibraryFailed);
				return;
			}
			wgTools.gPTC = "Qmqea4wQvtaDGdyWDbfbUg==";
			wgAppConfig.ProductTypeOfApp = "AccessControl";
			Program.localize();
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("CommTimeoutMsMin"))))
			{
				long.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("CommTimeoutMsMin")), out wgUdpComm.CommTimeoutMsMin);
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("WIFI_ACTIVE"))))
			{
				wgUdpComm.bWIFI = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("WIFI_ACTIVE")) == "1";
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_IP_PACKET_INTERVAL_MS"))))
			{
				try
				{
					WatchingService.ip_packet_interval_ms = int.Parse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_IP_PACKET_INTERVAL_MS")));
				}
				catch (Exception ex)
				{
					Program.wgToolsWgDebugWrite(ex.ToString());
				}
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_BROADCAST_PACKET_INTERVAL_MS"))))
			{
				try
				{
					WatchingService.broadcast_packet_interval_ms = int.Parse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_BROADCAST_PACKET_INTERVAL_MS")));
				}
				catch (Exception ex2)
				{
					Program.wgToolsWgDebugWrite(ex2.ToString());
				}
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVERSPECIAL_ENABLE"))))
			{
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVERSPECIAL_ENABLE")), out wgTools.bUDPCloudSpecial);
			}
			if (wgAppConfig.GetKeyVal("AutoRestartWhenNetFailed").Equals("1"))
			{
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("AutoRestartWhenNetFailed")), out wgTools.bUDPNeedCheckNetRunning);
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVERSPECIAL_SHORTPORT"))))
			{
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVERSPECIAL_SHORTPORT")), out wgTools.UDPCloudShortPortSpecial);
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDIPSPECIAL"))))
			{
				wgTools.UDPCloudIPSpecial = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDIPSPECIAL"));
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_ENABLE"))))
			{
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_ENABLE")), out wgTools.bUDPCloud);
				if (wgTools.bUDPCloud > 0)
				{
					wgAppConfig.GetKeyIntVal("KEY_P64_GPRS_REFRESHCYCLEMAX", ref wgTools.p64_gprs_refreshCycleMax);
					wgAppConfig.GetKeyIntVal("KEY_P64_GPRS_WATCHINGSENDCYCLE", ref wgTools.p64_gprs_watchingSendCycle);
					if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_CYCLE"))))
					{
						int num = 4;
						int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_CYCLE")), out num);
						if (num >= 5)
						{
							WatchingService.Watching_Cycle_ms = (num + 1) * 1000;
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_IP"))))
			{
				wgTools.UDPCloudIP = wgTools.validCloudServerIP(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_IP")));
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_SHORTPORT"))))
			{
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_SHORTPORT")), out wgTools.UDPCloudShortPort);
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_PORT"))))
			{
				int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_PORT")), out wgTools.UDPCloudPort);
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEYS_CustomBackgroud"))))
			{
				wgAppConfig.outPutBackgroundFile();
			}
			if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_CUSTOMTYPE"))))
			{
				wgTools.gCustomProductType = wgAppConfig.GetKeyVal("KEY_CUSTOMTYPE");
				wgTools.gPTC = wgTools.gCustomProductType;
			}
			wgTools.gReaderBrokenWarnActive = ((wgAppConfig.GetKeyVal("KEY_strRecordWarnEmergencyCall").IndexOf("防拆") >= 0) ? 1 : 0);
			if (cmdArgs.Length == 1)
			{
				if (cmdArgs[0].ToUpper() == "-P")
				{
					wgAppConfig.dbConString = "";
					Application.Run(new frmProductFormat());
					return;
				}
				if (cmdArgs[0].Length > 2 && cmdArgs[0].ToUpper().Substring(0, 3) == "-CS")
				{
					wgAppConfig.dbConString = "";
					icOperator.login("access", "168668");
					frmTestController frmTestController = new frmTestController();
					try
					{
						if (!(cmdArgs[0].ToUpper().Substring(0, 4) == "-CSN"))
						{
							frmTestController.onlyProduce();
						}
						Application.Run(frmTestController);
					}
					catch (Exception)
					{
					}
					frmTestController.Dispose();
					return;
				}
			}
			if (cmdArgs.Length >= 2)
			{
				batchAutoRun.commandSpecialCall(cmdArgs);
				return;
			}
			Program.pkgUnzip();
			dfrmWait dfrmWait = new dfrmWait();
			dfrmWait.Show();
			dfrmWait.Refresh();
			Program.Other_dll_Check();
			if (Program.dbConnectionCheck() > 0)
			{
				wgAppConfig.CustConfigureInit();
				Program.UpgradeDatabase();
				try
				{
					string systemParamByNO = wgAppConfig.getSystemParamByNO(30);
					if (string.IsNullOrEmpty(systemParamByNO))
					{
						wgAppConfig.setSystemParamValue(30, "Application Version", Application.ProductVersion, "V9 当前使用的应用软件版本");
					}
					else if (systemParamByNO != Application.ProductVersion)
					{
						wgAppConfig.setSystemParamValue(30, "Application Version", Application.ProductVersion, "V9 当前使用的应用软件版本");
					}
				}
				catch (Exception)
				{
				}
				dfrmWait.Close();
				if (cmdArgs.Length == 1 && (cmdArgs[0].ToUpper() == "-S" || cmdArgs[0].ToUpper() == "-WEB"))
				{
					dfrmNetControllerConfig dfrmNetControllerConfig = new dfrmNetControllerConfig();
					try
					{
						dfrmNetControllerConfig.btnAddToSystem.Visible = false;
						icOperator.login("access", "168668");
						if (cmdArgs[0].ToUpper() == "-WEB")
						{
							dfrmNetControllerConfig.btnIPAndWebConfigure.Visible = true;
						}
						Application.Run(dfrmNetControllerConfig);
					}
					catch (Exception)
					{
					}
					dfrmNetControllerConfig.Dispose();
					return;
				}
				Application.Run(new frmLogin());
				if (wgAppConfig.bNeedRestore)
				{
					try
					{
						using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
						{
							dfrmInputNewName.label1.Text = CommonStr.strInputExtendFunctionPassword;
							dfrmInputNewName.setPasswordChar('*');
							if (dfrmInputNewName.ShowDialog() != DialogResult.OK || dfrmInputNewName.strNewName != "5678")
							{
								return;
							}
						}
						if (wgAppConfig.IsAccessDB)
						{
							Application.Run(new dfrmAccessDbRestore());
						}
						else
						{
							Process.Start(new ProcessStartInfo
							{
								FileName = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1) + "SqlSet.exe",
								UseShellExecute = true
							});
						}
					}
					catch (Exception)
					{
					}
					return;
				}
				if (wgAppConfig.IsLogin)
				{
					wgAppConfig.Path4PhotoRefresh();
					wgAppConfig.Path4AviJpgRefresh();
					wgAppConfig.Path4AviJpgOnlyViewRefresh();
					try
					{
						if (int.Parse(wgAppConfig.GetKeyVal("RunTimeAt")) >= 0)
						{
							DateTime dateTime = DateTime.Parse("2012-12-1");
							wgAppConfig.UpdateKeyVal("RunTimeAt", (DateTime.Now.Subtract(dateTime).Days + 31).ToString());
						}
						else
						{
							DateTime dateTime2 = DateTime.Parse("2012-12-1");
							wgAppConfig.UpdateKeyVal("RunTimeAt", (DateTime.Now.Subtract(dateTime2).Days + 31).ToString());
						}
					}
					catch (Exception)
					{
						DateTime dateTime3 = DateTime.Parse("2012-12-1");
						wgAppConfig.UpdateKeyVal("RunTimeAt", (DateTime.Now.Subtract(dateTime3).Days + 31).ToString());
					}
					try
					{
						if (string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(49)))
						{
							wgAppConfig.setSystemParamValue(49, "Install Time", DateTime.Now.ToString(wgTools.YMDHMSFormat), "");
						}
					}
					catch (Exception)
					{
					}
					try
					{
						string text = wgAppConfig.Path4Photo();
						if (!wgAppConfig.DirectoryIsExisted(text))
						{
							wgAppConfig.CreatePhotoDirectory(text);
						}
						if (!wgAppConfig.DirectoryIsExisted(text))
						{
							wgAppConfig.wgLog(text + " " + CommonStr.strFileDirectoryNotVisited);
						}
					}
					catch (Exception)
					{
					}
					try
					{
						string text2 = wgAppConfig.Path4PhotoDefault();
						if (!wgAppConfig.DirectoryIsExisted(text2))
						{
							Directory.CreateDirectory(text2);
						}
						if (!wgAppConfig.DirectoryIsExisted(text2))
						{
							wgAppConfig.wgLog(text2 + " " + CommonStr.strFileDirectoryNotVisited);
						}
					}
					catch (Exception)
					{
					}
					icConsumer.gTimeSecondEnabled = wgAppConfig.getParamValBoolByNO(191);
					if (wgTools.bUDPCloud > 0 && wgAppConfig.getParamValBoolByNO(203) && !wgAppConfig.IsAccessControlBlue)
					{
						wgTools.bUDPOnly64 = 1;
					}
					try
					{
						string text3 = wgAppConfig.Path4PhotoDefault();
						if (wgAppConfig.DirectoryIsExisted(text3))
						{
							string text4 = text3 + "\\DoorBell.wav";
							FileInfo fileInfo = new FileInfo(text4);
							if (fileInfo.Exists && fileInfo.Length > 3000000L)
							{
								string text5 = text3 + "\\Other\\DoorBell.wav";
								FileInfo fileInfo2 = new FileInfo(text5);
								if (!fileInfo2.Exists)
								{
									GZip.Decompress(text3, text3 + "\\Other\\", "DoorBell.wav");
									fileInfo2 = new FileInfo(text5);
									if (fileInfo2.Exists)
									{
										fileInfo.Delete();
										fileInfo2.MoveTo(text4);
									}
								}
							}
						}
					}
					catch (Exception ex3)
					{
						wgAppConfig.wgLog(ex3.ToString());
					}
					try
					{
						int num2 = 0;
						int.TryParse(wgAppConfig.GetKeyVal("KEY_Video_DELETE_AVI_TIME_FREQ_MONTH"), out num2);
						if (num2 > 0)
						{
							new Thread(new ThreadStart(Program.deleteOldAviPhoto)).Start();
						}
					}
					catch (Exception ex4)
					{
						wgAppConfig.wgLog(ex4.ToString());
					}
				}
				if (wgAppConfig.IsLogin)
				{
					Application.Run(new frmADCT3000());
					if (wgAppConfig.IsAccessDB)
					{
						dfrmWait dfrmWait2 = new dfrmWait();
						dfrmWait2.Show();
						dfrmWait2.Refresh();
						wgAppConfig.backupBeforeExitByJustCopy();
						dfrmWait2.Hide();
						dfrmWait2.Close();
					}
					try
					{
						Process process = frmConsole.RunningInstance();
						if (process != null && (process.MainWindowTitle.Equals("Capture") || process.MainWindowTitle.Equals(CommonStr.strCameraPhoto)))
						{
							process.Kill();
						}
					}
					catch
					{
					}
					if (wgAppConfig.gRestart)
					{
						Process.Start(new ProcessStartInfo
						{
							FileName = Application.ExecutablePath,
							UseShellExecute = true
						});
					}
				}
				if (wgMail.bSendingMail)
				{
					for (int i = 0; i < 30; i++)
					{
						Thread.Sleep(1000);
						if (!wgMail.bSendingMail)
						{
							break;
						}
					}
				}
				try
				{
					Thread.Sleep(500);
					Environment.Exit(0);
					Process.GetCurrentProcess().Kill();
				}
				catch (Exception)
				{
				}
				return;
			}
			else
			{
				dfrmWait.Close();
				string text6 = Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."));
				text6 = text6.Substring(0, text6.LastIndexOf("."));
				string text7 = "BLUE";
				if (wgTools.gWGYTJ)
				{
					text7 = "AGYTJ";
				}
				string productTypeOfApp = wgAppConfig.ProductTypeOfApp;
				if (productTypeOfApp != null)
				{
					if (!(productTypeOfApp == "CGACCESS"))
					{
						if (productTypeOfApp == "AGACCESS")
						{
							text7 = "AG";
						}
						else if (productTypeOfApp == "XGACCESS")
						{
							text7 = "XG";
						}
					}
					else
					{
						text7 = "CG";
					}
				}
				string text8 = string.Format("Information [{0} - Ver: {1}]", text7, text6);
				if (wgAppConfig.IsAccessDB)
				{
					XMessageBox.Show(CommonStr.strAccessDatabaseNotConnected, text8, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (XMessageBox.Show(CommonStr.strSqlServerNotConnected, text8, MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) == DialogResult.OK)
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1) + "SqlSet.exe",
						UseShellExecute = true
					});
				}
				return;
			}
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x00220D9C File Offset: 0x0021FD9C
		public static int n3k_comm_dll_Check()
		{
			int num = -1;
			try
			{
				num = Program.n3k_comm_dll_Check_Existed();
				if (num > 0)
				{
					num = -1;
					if (WGPacket.Ept("WGControl") == "H5vVaR/joDreeVczBBdZhw==" && WGPacket.Dpt(WGPacket.Ept("WGControl")) == "WGControl")
					{
						num = 1;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x00220E0C File Offset: 0x0021FE0C
		public static int n3k_comm_dll_Check_Existed()
		{
			int num = 70921;
			int num2 = -1;
			string text = "n3k_comm";
			try
			{
				num2 = Program.n3k_comm_version();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				wgAppConfig.wgLogWithoutDB(ex.ToString(), EventLogEntryType.Information, null);
			}
			if (num2 < num)
			{
				try
				{
					FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\" + text + ".dll");
					byte[] array = null;
					if (fileInfo.Exists)
					{
						using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(fileInfo.FullName)))
						{
							array = binaryReader.ReadBytes(1048576);
						}
						if (array != null && array.Length == Resources.n3k_comm.Length)
						{
							int num3 = 1;
							byte[] n3k_comm = Resources.n3k_comm;
							for (int i = 0; i < array.Length; i++)
							{
								if (array[i] != n3k_comm[i])
								{
									num3 = 0;
									break;
								}
							}
							if (num3 > 0)
							{
								return 1;
							}
						}
						fileInfo.MoveTo(string.Concat(new string[]
						{
							Application.StartupPath,
							"\\",
							text,
							DateTime.Now.ToString("_yyyyMMddHHmmss"),
							".dll"
						}));
					}
					fileInfo = new FileInfo(Application.StartupPath + "\\" + text + ".dll");
					using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(fileInfo.FullName, FileMode.Create)))
					{
						binaryWriter.Write(Resources.n3k_comm);
					}
					num2 = Program.n3k_comm_version();
					if (num2 == num)
					{
						return 1;
					}
					wgAppConfig.wgLog(string.Format("n3k_comm: {0} =>{1}", num2, num));
					Process.Start(new ProcessStartInfo
					{
						FileName = Application.ExecutablePath,
						UseShellExecute = true
					});
					Thread.Sleep(500);
					Environment.Exit(0);
					Process.GetCurrentProcess().Kill();
					return 0;
				}
				catch (Exception ex2)
				{
					wgTools.WgDebugWrite(ex2.ToString(), new object[] { EventLogEntryType.Error });
					wgAppConfig.wgLog(ex2.ToString());
					return 0;
				}
				return 1;
			}
			return 1;
		}

		// Token: 0x06001B07 RID: 6919
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int n3k_comm_version();

		// Token: 0x06001B08 RID: 6920 RVA: 0x002210AC File Offset: 0x002200AC
		public static int Other_dll_Check()
		{
			try
			{
				string text = "Interop.jro";
				FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\" + text + ".dll");
				if (!fileInfo.Exists)
				{
					fileInfo = new FileInfo(Application.StartupPath + "\\" + text + ".dll");
					using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(fileInfo.FullName, FileMode.Create)))
					{
						binaryWriter.Write(Resources.Interop_JRO);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				wgAppConfig.wgLog(ex.ToString());
			}
			try
			{
				string text = "n3k_extern";
				FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\" + text + ".dll");
				byte[] array = null;
				if (fileInfo.Exists)
				{
					using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(fileInfo.FullName)))
					{
						array = binaryReader.ReadBytes(1048576);
					}
					if (array != null && array.Length == Resources.n3k_extern.Length)
					{
						int num = 1;
						byte[] n3k_extern = Resources.n3k_extern;
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i] != n3k_extern[i])
							{
								num = 0;
								break;
							}
						}
						if (num > 0)
						{
							return 1;
						}
					}
					fileInfo.MoveTo(string.Concat(new string[]
					{
						Application.StartupPath,
						"\\",
						text,
						DateTime.Now.ToString("_yyyyMMddHHmmss"),
						".dll"
					}));
				}
				fileInfo = new FileInfo(Application.StartupPath + "\\" + text + ".dll");
				using (BinaryWriter binaryWriter2 = new BinaryWriter(File.Open(fileInfo.FullName, FileMode.Create)))
				{
					binaryWriter2.Write(Resources.n3k_extern);
				}
				wgAppConfig.wgLog(string.Format("n3k_extern: overwrite", new object[0]));
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[] { EventLogEntryType.Error });
				wgAppConfig.wgLog(ex2.ToString());
			}
			return 1;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0022135C File Offset: 0x0022035C
		private static void pkgUnzip()
		{
			try
			{
				if (Application.ExecutablePath.ToUpper().IndexOf("N3K.EXE") > 0)
				{
					if (File.Exists(Application.ExecutablePath.ToUpper().Replace("N3K.EXE", "N3000.EXE")))
					{
						File.Delete(Application.ExecutablePath.ToUpper().Replace("N3K.EXE", "N3000.EXE"));
						File.Copy(Application.ExecutablePath, Application.ExecutablePath.ToUpper().Replace("N3K.EXE", "N3000.EXE"));
					}
				}
				else if (Application.ExecutablePath.ToUpper().IndexOf("N3000.EXE") > 0 && File.Exists(Application.ExecutablePath.ToUpper().Replace("N3000.EXE", "N3K.EXE")))
				{
					File.Delete(Application.ExecutablePath.ToUpper().Replace("N3000.EXE", "N3K.EXE"));
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0022145C File Offset: 0x0022045C
		public static void UpgradeDatabase()
		{
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
				float num = 0f;
				dbConnection.Open();
				string text = "SELECT f_Value FROM t_a_SystemParam WHERE f_No=9 ";
				dbCommand.CommandText = text;
				float.TryParse(dbCommand.ExecuteScalar().ToString(), out num);
				dbConnection.Close();
				dbCommand.Dispose();
				if (num > Program.dbVersionNewest)
				{
					XMessageBox.Show(string.Format(string.Concat(new string[]
					{
						CommonStr.strWrongVersionDatabaseHead,
						"{0}",
						CommonStr.strWrongVersionDatabaseMiddle,
						"{1}",
						CommonStr.strWrongVersionDatabaseTail
					}), num, Program.dbVersionNewest));
					using (dfrmUpdateOnline dfrmUpdateOnline = new dfrmUpdateOnline())
					{
						dfrmUpdateOnline.TopMost = true;
						dfrmUpdateOnline.ShowDialog();
					}
					Process.GetCurrentProcess().Kill();
					Application.Exit();
				}
				else
				{
					Program.UpgradeDatabase_common(num);
					Program.checkCardNO(num);
					if (num != Program.dbVersionNewest)
					{
						if (num <= 91.1f)
						{
							try
							{
								if (wgAppConfig.getValBySql("SELECT COUNT(*) FROM t_s_wglog") > 100000 && wgAppConfig.IsAccessDB)
								{
									try
									{
										new FileInfo(string.Format(Application.StartupPath + "\\t{0}.bak", wgAppConfig.accessDbName)).MoveTo(Application.StartupPath + string.Format("\\t{0}_V{1}_{2}.bak", wgAppConfig.accessDbName, num, DateTime.Now.ToString("yyyyMMdd_HHmmss")));
									}
									catch
									{
									}
									wgAppConfig.runUpdateSql("Delete From t_s_wglog");
									try
									{
										JetEngine jetEngine = new JetEngineClass();
										string text2 = Application.StartupPath + string.Format("\\{0}_{1}.mdb", wgAppConfig.accessDbName, num);
										jetEngine.CompactDatabase(wgAppConfig.dbConString, string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};User ID=admin;Password=;JET OLEDB:Database Password=168168;Jet OLEDB:Engine Type=5", text2));
										FileInfo fileInfo = new FileInfo(text2);
										fileInfo.CopyTo(Application.StartupPath + string.Format("\\{0}.mdb", wgAppConfig.accessDbName), true);
										fileInfo.Delete();
									}
									catch
									{
									}
								}
							}
							catch
							{
							}
						}
						try
						{
							if (Program.wgAppConfigIsAccessDB())
							{
								dbConnection = new OleDbConnection(wgAppConfig.dbConString);
								dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
							}
							else
							{
								dbConnection = new SqlConnection(wgAppConfig.dbConString);
								dbCommand = new SqlCommand("", dbConnection as SqlConnection);
							}
							dbCommand.CommandText = "SELECT * FROM t_s_Operator ";
							dbConnection.Open();
							DbDataReader dbDataReader = dbCommand.ExecuteReader();
							string text3 = "";
							while (dbDataReader.Read())
							{
								text3 = wgTools.SetObjToStr(dbDataReader["f_Password"]);
								if (!string.IsNullOrEmpty(text3))
								{
									try
									{
										string.IsNullOrEmpty(Program.Dpt4Database(text3));
										continue;
									}
									catch (Exception ex)
									{
										Program.wgToolsWgDebugWrite(ex.ToString());
										Program.wgAppConfigRunUpdateSql(string.Format("UPDATE t_s_Operator SET f_Password ={0} WHERE f_OperatorID={1} ", Program.wgToolsPrepareStr(Program.Ept4Database(text3)), dbDataReader["f_OperatorID"].ToString()));
										continue;
									}
								}
								Program.wgAppConfigRunUpdateSql(string.Format("UPDATE t_s_Operator SET f_Password ={0} WHERE f_OperatorID={1} ", Program.wgToolsPrepareStr(Program.Ept4Database("")), dbDataReader["f_OperatorID"].ToString()));
							}
							dbDataReader.Close();
							dbConnection.Close();
							dbCommand.Dispose();
						}
						catch (Exception ex2)
						{
							Program.wgToolsWgDebugWrite(ex2.ToString());
						}
						wgAppConfig.setSystemParamValue(9, "Database Version", Program.dbVersionNewest.ToString(), string.Concat(new object[]
						{
							"V",
							num,
							" => V",
							Program.dbVersionNewest
						}));
						wgAppConfig.wgLog(string.Concat(new object[]
						{
							"V",
							num,
							" => V",
							Program.dbVersionNewest
						}), EventLogEntryType.Information, null);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x00221914 File Offset: 0x00220914
		public static void UpgradeDatabase_common(float dbversion)
		{
			string text = "";
			if (dbversion < 70f)
			{
				XMessageBox.Show(string.Format(string.Concat(new string[]
				{
					CommonStr.strWrongVersionDatabaseHead,
					"{0}",
					CommonStr.strWrongVersionDatabaseMiddle,
					"{1}",
					CommonStr.strWrongVersionDatabaseTail
				}), dbversion, Program.dbVersionNewest));
				Process.GetCurrentProcess().Kill();
				return;
			}
			if (dbversion > Program.dbVersionNewest)
			{
				XMessageBox.Show(string.Format(string.Concat(new string[]
				{
					CommonStr.strWrongVersionDatabaseHead,
					"{0}",
					CommonStr.strWrongVersionDatabaseMiddle,
					"{1}",
					CommonStr.strWrongVersionDatabaseTail
				}), dbversion, Program.dbVersionNewest));
				Process.GetCurrentProcess().Kill();
				return;
			}
			if (dbversion == 73f)
			{
				if (Program.wgAppConfigGetSystemParamByNO(146) == "")
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(146,'Activate Door As Switch','','0','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
				if (Program.wgAppConfigGetSystemParamByNO(147) == "")
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(147,'Activate Valid Swipe Gap','','0','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
				if (Program.wgAppConfigGetSystemParamByNO(148) == "")
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(148,'Activate Operator Management','','0','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
			}
			if (dbversion <= 73.1f)
			{
				try
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(149,'Activate Meeting','','0','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex)
				{
					Program.wgToolsWgDebugWrite(ex.ToString());
				}
				try
				{
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "CREATE TABLE  [t_d_Reader4Meeting] ( ";
							Program.wgAppConfigRunUpdateSql(text + "f_MeetingNO   [nvarchar] (15)   NOT NULL,[f_ReaderID] INT  NULL )");
						}
						else
						{
							text = "CREATE TABLE  [t_d_Reader4Meeting] ( ";
							Program.wgAppConfigRunUpdateSql(text + "f_MeetingNO TEXT (15) NOT NULL,[f_ReaderID] INT  NULL )");
						}
					}
					catch (Exception ex2)
					{
						Program.wgToolsWgDebugWrite(ex2.ToString());
					}
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "CREATE TABLE t_d_Meeting ( ";
							Program.wgAppConfigRunUpdateSql(text + "f_MeetingNO   [nvarchar] (15)   NOT NULL,f_MeetingName   [nvarchar] (255)  NULL,f_MeetingAdr   [nvarchar] (255)  NULL,f_MeetingDateTime   DATETIME NOT NULL,f_SignStartTime   DATETIME NOT NULL,f_SignEndTime   DATETIME NOT NULL,f_Content   [nvarchar] (255)  NULL,f_Notes      [ntext]  NULL  )");
							text = " ALTER TABLE [t_d_Meeting] WITH NOCHECK ADD ";
							Program.wgAppConfigRunUpdateSql(text + " CONSTRAINT [PK_t_d_Meeting] PRIMARY KEY  CLUSTERED  ([f_MeetingNO])  ");
						}
						else
						{
							text = "CREATE TABLE t_d_Meeting ( ";
							Program.wgAppConfigRunUpdateSql(text + "f_MeetingNO TEXT (15) NOT NULL,f_MeetingName TEXT (255) NULL ,f_MeetingAdr TEXT (255) NULL ,f_MeetingDateTime   DATETIME NOT NULL,f_SignStartTime   DATETIME NOT NULL,f_SignEndTime   DATETIME NOT NULL,f_Content TEXT (255) NULL ,f_Notes MEMO ,CONSTRAINT PK_t_d_Meeting PRIMARY KEY  (f_MeetingNO))");
						}
					}
					catch (Exception ex3)
					{
						Program.wgToolsWgDebugWrite(ex3.ToString());
					}
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "CREATE TABLE t_d_MeetingAdr ( ";
							Program.wgAppConfigRunUpdateSql(text + "f_MeetingAdr   [nvarchar] (255)  NOT NULL,f_ReaderID   INT   NOT NULL Default 0,f_Notes      [ntext]  NULL  )");
						}
						else
						{
							text = "CREATE TABLE t_d_MeetingAdr ( ";
							Program.wgAppConfigRunUpdateSql(text + "f_MeetingAdr TEXT (255) NOT NULL ,f_ReaderID   INT   NOT NULL Default 0,f_Notes MEMO )");
						}
					}
					catch (Exception ex4)
					{
						Program.wgToolsWgDebugWrite(ex4.ToString());
					}
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "CREATE TABLE t_d_MeetingConsumer ( ";
							Program.wgAppConfigRunUpdateSql(text + " f_Id        [int] IDENTITY (1, 1) NOT NULL  , f_MeetingNO   [nvarchar] (15)   NOT NULL, f_ConsumerID   [int]  NOT NULL Default(0) , f_MeetingIdentity    INT NOT NULL   DEFAULT -1, f_Seat   [nvarchar] (255)  NULL, f_SignWay   [int]  NOT NULL Default(0), f_SignRealTime   DATETIME NULL, f_RecID  INT NOT NULL   DEFAULT 0 , f_Notes      [ntext]  NULL  )");
						}
						else
						{
							text = "CREATE TABLE t_d_MeetingConsumer ( ";
							Program.wgAppConfigRunUpdateSql(text + " f_Id AUTOINCREMENT NOT NULL , f_MeetingNO   TEXT (15)   NOT NULL, f_ConsumerID    INT NOT NULL   DEFAULT 0 , f_MeetingIdentity    INT NOT NULL   DEFAULT -1, f_Seat   TEXT (255) NULL, f_SignWay   int  NOT NULL Default 0, f_SignRealTime  DATETIME NULL, f_RecID  INT NOT NULL   DEFAULT 0 , f_Notes      MEMO  )");
						}
					}
					catch (Exception ex5)
					{
						Program.wgToolsWgDebugWrite(ex5.ToString());
					}
				}
				catch (Exception ex6)
				{
					Program.wgToolsWgDebugWrite(ex6.ToString());
				}
				try
				{
					text = "CREATE TABLE  [t_d_Reader4Meal] ( ";
					Program.wgAppConfigRunUpdateSql(text + "[f_ReaderID] INT  NULL, f_CostMorning   Numeric(10,2) NOT   NULL  DEFAULT -1 , f_CostLunch   Numeric(10,2)  NOT NULL  DEFAULT -1 , f_CostEvening   Numeric(10,2) NOT  NULL   DEFAULT -1 , f_CostOther   Numeric(10,2) NOT  NULL  DEFAULT -1  )");
				}
				catch (Exception ex7)
				{
					Program.wgToolsWgDebugWrite(ex7.ToString());
				}
				try
				{
					try
					{
						text = "   CREATE TABLE  [t_b_MealSetup] ";
						if (!Program.wgAppConfigIsAccessDB())
						{
							text += "( [f_ID] INT NOT NULL , [f_Value] INT NULL , [f_BeginHMS] DATETIME NULL ,[f_EndHMS] DATETIME NULL , [f_ParamVal]   Numeric(10,2)   NULL , f_Notes      [ntext]  NULL  ) ";
						}
						else
						{
							text += "( [f_ID] INT NOT NULL , [f_Value] INT NULL , [f_BeginHMS] DATETIME NULL ,[f_EndHMS] DATETIME NULL , [f_ParamVal]   Numeric(10,2)   NULL ,  f_Notes      MEMO   ) ";
						}
						Program.wgAppConfigRunUpdateSql(text);
					}
					catch (Exception ex8)
					{
						Program.wgToolsWgDebugWrite(ex8.ToString());
					}
					try
					{
						text = " INSERT INTO [t_b_MealSetup] ([f_ID], [f_Value], [f_BeginHMS] ,[f_EndHMS] , [f_ParamVal]) ";
						Program.wgAppConfigRunUpdateSql(text + "VALUES (1, 0,NULL, NULL,60)");
					}
					catch (Exception ex9)
					{
						Program.wgToolsWgDebugWrite(ex9.ToString());
					}
					try
					{
						text = " INSERT INTO [t_b_MealSetup] ([f_ID], [f_Value], [f_BeginHMS] ,[f_EndHMS] , [f_ParamVal]) ";
						string text2 = text;
						Program.wgAppConfigRunUpdateSql(string.Concat(new string[]
						{
							text2,
							"VALUES (2, 1,",
							Program.wgToolsPrepareStr("04:00", true, " HH:mm"),
							",",
							Program.wgToolsPrepareStr("09:59", true, " HH:mm"),
							",0)"
						}));
					}
					catch (Exception ex10)
					{
						Program.wgToolsWgDebugWrite(ex10.ToString());
					}
					try
					{
						text = " INSERT INTO [t_b_MealSetup] ([f_ID], [f_Value], [f_BeginHMS] ,[f_EndHMS] , [f_ParamVal]) ";
						string text2 = text;
						Program.wgAppConfigRunUpdateSql(string.Concat(new string[]
						{
							text2,
							"VALUES (3, 1,",
							Program.wgToolsPrepareStr("10:00", true, " HH:mm"),
							",",
							Program.wgToolsPrepareStr("15:59", true, " HH:mm"),
							",0)"
						}));
					}
					catch (Exception ex11)
					{
						Program.wgToolsWgDebugWrite(ex11.ToString());
					}
					try
					{
						text = " INSERT INTO [t_b_MealSetup] ([f_ID], [f_Value], [f_BeginHMS] ,[f_EndHMS] , [f_ParamVal]) ";
						string text2 = text;
						Program.wgAppConfigRunUpdateSql(string.Concat(new string[]
						{
							text2,
							"VALUES (4, 1,",
							Program.wgToolsPrepareStr("16:00", true, " HH:mm"),
							",",
							Program.wgToolsPrepareStr("21:59", true, " HH:mm"),
							",0)"
						}));
					}
					catch (Exception ex12)
					{
						Program.wgToolsWgDebugWrite(ex12.ToString());
					}
					try
					{
						text = " INSERT INTO [t_b_MealSetup] ([f_ID], [f_Value], [f_BeginHMS] ,[f_EndHMS] , [f_ParamVal]) ";
						string text2 = text;
						Program.wgAppConfigRunUpdateSql(string.Concat(new string[]
						{
							text2,
							"VALUES (5, 1,",
							Program.wgToolsPrepareStr("22:00", true, " HH:mm"),
							",",
							Program.wgToolsPrepareStr("03:59", true, " HH:mm"),
							",0)"
						}));
					}
					catch (Exception ex13)
					{
						Program.wgToolsWgDebugWrite(ex13.ToString());
					}
				}
				catch (Exception ex14)
				{
					Program.wgToolsWgDebugWrite(ex14.ToString());
				}
			}
			if (dbversion <= 73.2f)
			{
				try
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(27,'AbsentTimeout (minute)','','30','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex15)
				{
					Program.wgToolsWgDebugWrite(ex15.ToString());
				}
				try
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(28,'AllowTimeout (minute)','','10','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex16)
				{
					Program.wgToolsWgDebugWrite(ex16.ToString());
				}
				try
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(29,'LogCreatePatrolReport','','','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex17)
				{
					Program.wgToolsWgDebugWrite(ex17.ToString());
				}
				try
				{
					text = "CREATE TABLE  [t_b_Reader4Patrol] ( ";
					Program.wgAppConfigRunUpdateSql(text + "[f_ReaderID] INT  NULL )");
				}
				catch (Exception ex18)
				{
					Program.wgToolsWgDebugWrite(ex18.ToString());
				}
				try
				{
					text = "CREATE TABLE  [t_d_PatrolUsers] ( ";
					Program.wgAppConfigRunUpdateSql(text + "[f_ConsumerID] INT  NULL )");
				}
				catch (Exception ex19)
				{
					Program.wgToolsWgDebugWrite(ex19.ToString());
				}
				try
				{
					text = " CREATE TABLE t_d_PatrolRouteDetail (";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_RecId AUTOINCREMENT NOT NULL , f_RouteID int   , f_Sn int   ,f_ReaderID int , f_patroltime TEXT(5)  NULL   , f_NextDay int ,   CONSTRAINT PK_t_b_PatrolRouteDetail PRIMARY KEY ( f_RouteID,f_Sn)) ";
					}
					else
					{
						text += " f_RecId [int] IDENTITY (1, 1) NOT NULL  , f_RouteID int   , f_Sn int   ,f_ReaderID int , f_patroltime  [nvarchar] (5)   NULL   , f_NextDay int ,   CONSTRAINT PK_t_b_PatrolRouteDetail PRIMARY KEY ( f_RouteID,f_Sn)) ";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex20)
				{
					Program.wgToolsWgDebugWrite(ex20.ToString());
				}
				try
				{
					text = " CREATE TABLE t_d_PatrolRouteList (";
					text += "f_RouteID  INT NOT NULL   ,";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += " f_RouteName  TEXT (50) NOT NULL , f_Description NOTE , ";
					}
					else
					{
						text += " f_RouteName   [nvarchar] (50) NOT NULL , f_Description [ntext] NULL , ";
					}
					Program.wgAppConfigRunUpdateSql(text + "  CONSTRAINT PK_t_d_PatrolRouteList PRIMARY KEY ( f_RouteID)) ");
					text = "    CREATE UNIQUE INDEX idxf_RouteName_1 ";
					Program.wgAppConfigRunUpdateSql(text + "   ON t_d_PatrolRouteList (f_RouteName)");
				}
				catch (Exception ex21)
				{
					Program.wgToolsWgDebugWrite(ex21.ToString());
				}
				try
				{
					text = " CREATE TABLE t_d_PatrolPlanData (";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_RecID AUTOINCREMENT NOT NULL  , f_ConsumerID INT  NULL   , f_DateYM TEXT(10)  NULL  ";
					}
					else
					{
						text += " f_RecID [int] IDENTITY (1, 1) NOT NULL   , f_ConsumerID INT  NULL   , f_DateYM  [nvarchar](10)  NULL  ";
					}
					for (int i = 1; i <= 31; i++)
					{
						text = text + " , f_RouteID_" + i.ToString().PadLeft(2, '0') + "  INT   DEFAULT -1  ";
					}
					text += " , f_LogDate  DATETIME   NULL  ";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += " , f_Notes MEMO NULL ";
					}
					else
					{
						text += " ,  f_Notes      [ntext]  NULL ";
					}
					Program.wgAppConfigRunUpdateSql(text + " )");
				}
				catch (Exception ex22)
				{
					Program.wgToolsWgDebugWrite(ex22.ToString());
				}
				try
				{
					text = " CREATE TABLE t_d_PatrolDetailData (";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_RecId AUTOINCREMENT NOT NULL ,";
					}
					else
					{
						text += " f_RecId [int] IDENTITY (1, 1) NOT NULL  ,";
					}
					Program.wgAppConfigRunUpdateSql(text + " f_ConsumerID int   , f_PatrolDate  DATETIME NULL    , f_RouteID int   , f_ReaderID int   , f_PlanPatrolTime DATETIME NULL , f_RealPatrolTime DATETIME NULL , f_EventDesc  int ,  CONSTRAINT PK_t_d_PatrolDetailData PRIMARY KEY ( f_RecId)) ");
				}
				catch (Exception ex23)
				{
					Program.wgToolsWgDebugWrite(ex23.ToString());
				}
				try
				{
					text = " CREATE TABLE t_d_PatrolStatistic (";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_RecId AUTOINCREMENT NOT NULL ,";
					}
					else
					{
						text += " f_RecId [int] IDENTITY (1, 1) NOT NULL  ,";
					}
					Program.wgAppConfigRunUpdateSql(text + " f_ConsumerID int   , f_PatrolDateStart  DATETIME NULL    , f_PatrolDateEnd  DATETIME NULL    , f_TotalLate int   , f_TotalEarly int   , f_TotalAbsence int   , f_TotalNormal int   ,  CONSTRAINT PK_t_d_PatrolStatistic PRIMARY KEY ( f_RecId)) ");
				}
				catch (Exception ex24)
				{
					Program.wgToolsWgDebugWrite(ex24.ToString());
				}
				try
				{
					if (Program.wgAppConfigGetSystemParamByNO(149) == "")
					{
						text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(149,'Activate Meeting','','0','')";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex25)
				{
					Program.wgToolsWgDebugWrite(ex25.ToString());
				}
				try
				{
					if (Program.wgAppConfigGetSystemParamByNO(150) == "")
					{
						text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(150,'Activate Meal','','0','')";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex26)
				{
					Program.wgToolsWgDebugWrite(ex26.ToString());
				}
				try
				{
					if (Program.wgAppConfigGetSystemParamByNO(151) == "")
					{
						text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(151,'Activate Patrol','','0','')";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex27)
				{
					Program.wgToolsWgDebugWrite(ex27.ToString());
				}
			}
			if (dbversion <= 73.3f)
			{
				try
				{
					Program.wgAppConfigRunUpdateSql(Program.getOperatorPrivilegeInsertSql(48, "mnuPatrolDetailData", "Patrol"));
				}
				catch (Exception ex28)
				{
					Program.wgToolsWgDebugWrite(ex28.ToString());
				}
				try
				{
					Program.wgAppConfigRunUpdateSql(Program.getOperatorPrivilegeInsertSql(49, "mnuConstMeal", "Meal"));
				}
				catch (Exception ex29)
				{
					Program.wgToolsWgDebugWrite(ex29.ToString());
				}
				try
				{
					Program.wgAppConfigRunUpdateSql(Program.getOperatorPrivilegeInsertSql(50, "mnuMeeting", "Meeting"));
				}
				catch (Exception ex30)
				{
					Program.wgToolsWgDebugWrite(ex30.ToString());
				}
				try
				{
					Program.wgAppConfigRunUpdateSql(Program.getOperatorPrivilegeInsertSql(51, "mnuElevator", "Elevator"));
				}
				catch (Exception ex31)
				{
					Program.wgToolsWgDebugWrite(ex31.ToString());
				}
			}
			if (dbversion <= 73.5f)
			{
				try
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(60,'Active Fire_Broadcast','','0','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex32)
				{
					Program.wgToolsWgDebugWrite(ex32.ToString());
				}
				try
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(61,'Active Interlock_Broadcast','','0','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex33)
				{
					Program.wgToolsWgDebugWrite(ex33.ToString());
				}
				try
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(62,'Active Antiback_Broadcast','','0','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex34)
				{
					Program.wgToolsWgDebugWrite(ex34.ToString());
				}
			}
			if (dbversion <= 75.1f)
			{
				try
				{
					text = " CREATE TABLE t_d_Privilege_Of_PrivilegeType (";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_PrivilegeRecID AUTOINCREMENT NOT NULL , f_DoorID int   , f_ControlSegID int   ,f_ConsumerID int , f_ControllerID int , f_DoorNO int ,   CONSTRAINT [PK_t_d_Privilege_Of_PrivilegeType] PRIMARY KEY ( [f_ControllerID],[f_PrivilegeRecID])) ";
					}
					else
					{
						text += " f_PrivilegeRecID [int] IDENTITY (1, 1) NOT NULL  , f_DoorID int   , f_ControlSegID int   ,f_ConsumerID int , f_ControllerID int , f_DoorNO int ,   CONSTRAINT [PK_t_d_Privilege_Of_PrivilegeType] PRIMARY KEY ( [f_ControllerID],[f_PrivilegeRecID])) ";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex35)
				{
					Program.wgToolsWgDebugWrite(ex35.ToString());
				}
				try
				{
					text = " CREATE TABLE [t_d_PrivilegeType] (";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "[f_PrivilegeTypeID] AUTOINCREMENT NOT NULL , [f_PrivilegeTypeName]  TEXT (50)   NOT NULL, [f_Note] MEMO    ,  CONSTRAINT [PK_t_d_PrivilegeType] PRIMARY KEY ( [f_PrivilegeTypeID])) ";
					}
					else
					{
						text += " [f_PrivilegeTypeID] [int] IDENTITY (1, 1) NOT NULL  , [f_PrivilegeTypeName] [nvarchar](50) NOT NULL   , [f_Note] [ntext] NULL   ,  CONSTRAINT [PK_t_d_PrivilegeType] PRIMARY KEY ( [f_PrivilegeTypeID])) ";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex36)
				{
					Program.wgToolsWgDebugWrite(ex36.ToString());
				}
				try
				{
					if (Program.wgAppConfigIsAccessDB())
					{
						text = "ALTER TABLE t_b_Consumer  ADD COLUMN  [f_PrivilegeTypeID] int NOT NULL DEFAULT 0 ";
					}
					else
					{
						text = "ALTER TABLE t_b_Consumer  ADD   [f_PrivilegeTypeID] int NOT NULL DEFAULT 0 ";
					}
					Program.wgAppConfigRunUpdateSql(text);
					text = " Update t_b_Consumer ";
					Program.wgAppConfigRunUpdateSql(text + " SET f_PrivilegeTypeID = 0  ");
				}
				catch (Exception ex37)
				{
					Program.wgToolsWgDebugWrite(ex37.ToString());
				}
			}
			if ((double)dbversion <= 76.1)
			{
				try
				{
					text = " CREATE TABLE t_b_Consumer_Delete (";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "[f_DeletedID] AUTOINCREMENT NOT NULL ,[f_ConsumerID] int  NOT NULL,           [f_ConsumerNO] TEXT (20) NOT NULL,   [f_ConsumerName] TEXT (50) NOT NULL, [f_CardNO] Numeric(20,0) NULL,                 [f_GroupID] int NOT NULL,               [f_AttendEnabled] tinyint NOT NULL,     [f_ShiftEnabled] tinyint NOT NULL,      [f_DoorEnabled] tinyint NOT NULL,       [f_BeginYMD] datetime NOT NULL,         [f_EndYMD] datetime NOT NULL,           [f_PIN] int NOT NULL,                   [f_PrivilegeTypeID] int NOT NULL)      ";
					}
					else
					{
						text += " [f_DeletedID] [int] IDENTITY (1, 1) NOT NULL  ,[f_ConsumerID] [int]  NOT NULL,           [f_ConsumerNO] [nvarchar](20) NOT NULL,   [f_ConsumerName] [nvarchar](50) NOT NULL, [f_CardNO] [bigint] NULL,                 [f_GroupID] [int] NOT NULL,               [f_AttendEnabled] [tinyint] NOT NULL,     [f_ShiftEnabled] [tinyint] NOT NULL,      [f_DoorEnabled] [tinyint] NOT NULL,       [f_BeginYMD] [datetime] NOT NULL,         [f_EndYMD] [datetime] NOT NULL,           [f_PIN] [int] NOT NULL,                   [f_PrivilegeTypeID] [int] NOT NULL)     ";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex38)
				{
					Program.wgToolsWgDebugWrite(ex38.ToString());
				}
			}
			if (dbversion <= 78.4f)
			{
				try
				{
					text = "CREATE TABLE t_b_Camera ( ";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_CameraId   AUTOINCREMENT NOT NULL ,f_CameraName TEXT (255)  NOT NULL DEFAULT 'Camera' ,f_CameraIP   TEXT (255)  NOT NULL DEFAULT '192.168.168.123' ,f_CameraPort int  NOT NULL Default  9001  ,f_CameraChannel   int  NOT NULL Default 1  ,f_Enabled    int  NOT NULL Default 1  ,f_CameraUser  TEXT (255)   NULL DEFAULT 'admin' ,f_CameraPassword   TEXT (255)   NULL DEFAULT '12345' ,f_Type   int  NOT NULL  DEFAULT 0 ,f_Notes MEMO )";
					}
					else
					{
						text += "f_CameraId   [int] IDENTITY (1, 1) NOT NULL ,f_CameraName [nvarchar] (255)  NOT NULL DEFAULT ('Camera') ,f_CameraIP   [nvarchar] (255)  NOT NULL DEFAULT ('192.168.168.123') ,f_CameraPort [int]  NOT NULL Default(9001)  ,f_CameraChannel   [int]  NOT NULL Default(1)  ,f_Enabled    [int]  NOT NULL Default(1)  ,f_CameraUser   [nvarchar] (255)   NULL DEFAULT ('admin') ,f_CameraPassword   [nvarchar] (255)   NULL DEFAULT ('12345') ,f_Type       [int]  NOT NULL Default(0) ,f_Notes [ntext]  NULL  )";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex39)
				{
					Program.wgToolsWgDebugWrite(ex39.ToString());
				}
				try
				{
					text = "CREATE TABLE t_b_CameraTriggerSource ( ";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_Id AUTOINCREMENT NOT NULL ,f_CameraId int  NOT NULL  DEFAULT 0 ,f_ReaderID   int  NOT NULL  DEFAULT 0 ,f_Type   int  NOT NULL  DEFAULT 0 ,f_Notes MEMO )";
					}
					else
					{
						text += "f_Id         [int] IDENTITY (1, 1) NOT NULL  ,f_CameraId   [int]  NOT NULL Default(0) ,f_ReaderID   [int]  NOT NULL Default(0) ,f_Type       [int]  NOT NULL Default(0) ,f_Notes      [ntext]  NULL  )";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex40)
				{
					Program.wgToolsWgDebugWrite(ex40.ToString());
				}
				try
				{
					text = "CREATE TABLE t_b_Group4Meal ( ";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_Id AUTOINCREMENT NOT NULL ,f_GroupID int  NOT NULL  DEFAULT 0 ,f_Morning  Numeric(10,2)    NOT NULL  DEFAULT 0 ,f_Lunch  Numeric(10,2)    NOT NULL  DEFAULT 0 ,f_Evening  Numeric(10,2)    NOT NULL  DEFAULT 0 ,f_Other  Numeric(10,2)    NOT NULL  DEFAULT 0 ,f_Enabled    int  NOT NULL Default 1  ,f_Notes MEMO )";
					}
					else
					{
						text += "f_Id         [int] IDENTITY (1, 1) NOT NULL  ,f_GroupID int  NOT NULL  DEFAULT 0 ,f_Morning  Numeric(10,2)    NOT NULL  DEFAULT 0 ,f_Lunch  Numeric(10,2)    NOT NULL  DEFAULT 0 ,f_Evening  Numeric(10,2)    NOT NULL  DEFAULT 0 ,f_Other  Numeric(10,2)    NOT NULL  DEFAULT 0 ,f_Enabled    [int]  NOT NULL Default(1)  ,f_Notes      [ntext]  NULL  )";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex41)
				{
					Program.wgToolsWgDebugWrite(ex41.ToString());
				}
				Program.wgAppConfigRunUpdateSql("UPDATE t_a_SystemParam SET [f_Notes] = " + wgTools.PrepareStrNUnicode(Program.wgAppConfigGetSystemParamByNO(41)) + " WHERE f_NO = 41");
				Program.wgAppConfigRunUpdateSql("UPDATE t_a_SystemParam SET [f_Notes] = " + wgTools.PrepareStrNUnicode(Program.wgAppConfigGetSystemParamByNO(42)) + " WHERE f_NO = 42");
				Program.wgAppConfigRunUpdateSql("UPDATE t_a_SystemParam SET [f_Notes] = " + wgTools.PrepareStrNUnicode(Program.wgAppConfigGetSystemParamByNO(43)) + " WHERE f_NO = 43");
				try
				{
					Program.wgAppConfigRunUpdateSql(Program.getOperatorPrivilegeInsertSql(52, "mnuCameraManage", "Camera Manage"));
				}
				catch (Exception ex42)
				{
					Program.wgToolsWgDebugWrite(ex42.ToString());
				}
				try
				{
					Program.wgAppConfigRunUpdateSql(Program.getOperatorPrivilegeInsertSql(53, "mnuCameraMonitor", "Camera Monitor"));
				}
				catch (Exception ex43)
				{
					Program.wgToolsWgDebugWrite(ex43.ToString());
				}
				try
				{
					string keyVal = wgAppConfig.GetKeyVal("dbConnection");
					if (!string.IsNullOrEmpty(keyVal))
					{
						try
						{
							Program.Dpt4Database(keyVal);
						}
						catch
						{
							wgAppConfig.UpdateKeyVal("dbConnection", Program.Ept4Database(keyVal));
						}
					}
				}
				catch (Exception ex44)
				{
					Program.wgToolsWgDebugWrite(ex44.ToString());
				}
				try
				{
					if (Program.wgAppConfigIsAccessDB())
					{
						text = string.Format("ALTER TABLE t_s_Operator ALTER COLUMN [f_Password] TEXT(255) NULL ", new object[0]);
					}
					else
					{
						text = string.Format("ALTER TABLE t_s_Operator ALTER COLUMN [f_Password] [nvarchar](255) NULL ", new object[0]);
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex45)
				{
					Program.wgToolsWgDebugWrite(ex45.ToString());
				}
				try
				{
					DbConnection dbConnection;
					DbCommand dbCommand;
					if (Program.wgAppConfigIsAccessDB())
					{
						dbConnection = new OleDbConnection(wgAppConfig.dbConString);
						dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
					}
					else
					{
						dbConnection = new SqlConnection(wgAppConfig.dbConString);
						dbCommand = new SqlCommand("", dbConnection as SqlConnection);
					}
					dbCommand.CommandText = "SELECT * FROM t_s_Operator ";
					dbConnection.Open();
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					string text3 = "";
					while (dbDataReader.Read())
					{
						text3 = wgTools.SetObjToStr(dbDataReader["f_Password"]);
						if (!string.IsNullOrEmpty(text3))
						{
							try
							{
								string.IsNullOrEmpty(Program.Dpt4Database(text3));
								continue;
							}
							catch (Exception ex46)
							{
								Program.wgToolsWgDebugWrite(ex46.ToString());
								Program.wgAppConfigRunUpdateSql(string.Format("UPDATE t_s_Operator SET f_Password ={0} WHERE f_OperatorID={1} ", Program.wgToolsPrepareStr(Program.Ept4Database(text3)), dbDataReader["f_OperatorID"].ToString()));
								continue;
							}
						}
						Program.wgAppConfigRunUpdateSql(string.Format("UPDATE t_s_Operator SET f_Password ={0} WHERE f_OperatorID={1} ", Program.wgToolsPrepareStr(Program.Ept4Database("")), dbDataReader["f_OperatorID"].ToString()));
					}
					dbDataReader.Close();
					dbConnection.Close();
					dbCommand.Dispose();
				}
				catch (Exception ex47)
				{
					Program.wgToolsWgDebugWrite(ex47.ToString());
				}
			}
			if (dbversion <= 80f)
			{
				try
				{
					if (Program.wgAppConfigIsAccessDB())
					{
						text = string.Format("ALTER TABLE t_b_Controller_Zone ALTER COLUMN [f_ZoneName] TEXT(255) NULL ", new object[0]);
					}
					else
					{
						text = string.Format("ALTER TABLE t_b_Controller_Zone ALTER COLUMN [f_ZoneName] [nvarchar](2047) NULL ", new object[0]);
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex48)
				{
					Program.wgToolsWgDebugWrite(ex48.ToString());
				}
				try
				{
					if (Program.wgAppConfigIsAccessDB())
					{
						text = string.Format("ALTER TABLE t_b_Group ALTER COLUMN [f_GroupName] TEXT(255) NULL ", new object[0]);
					}
					else
					{
						text = string.Format("ALTER TABLE t_b_Group ALTER COLUMN [f_GroupName] [nvarchar](2047) NULL ", new object[0]);
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex49)
				{
					Program.wgToolsWgDebugWrite(ex49.ToString());
				}
			}
			if (dbversion <= 82f)
			{
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerClear\r\n(\r\n@ConsumerNO nvarchar(20)\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSELECT @consumerID=f_ConsumerID  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\n\r\nIF @consumerID = 0\r\nBEGIN\r\n SELECT @consumerID=f_ConsumerID  FROM t_b_Consumer_Delete \r\n WHERE f_ConsumerNO= @ConsumerNO \r\n IF @consumerID = 0\r\n BEGIN\r\n       RETURN(103)   \r\n END\r\nEND\r\n      \r\nBEGIN TRANSACTION\r\nDELETE FROM t_b_UserFloor WHERE [f_ConsumerID]=@ConsumerID\r\nDELETE FROM t_d_ShiftData  WHERE [f_ConsumerID]=@ConsumerID\r\nDELETE FROM t_d_Leave  WHERE [f_ConsumerID]=@ConsumerID\r\nDELETE FROM t_d_ManualCardRecord  WHERE [f_ConsumerID]=@ConsumerID\r\nDELETE FROM [t_d_Privilege]  WHERE [f_ConsumerID]=@ConsumerID\r\nDELETE FROM [t_b_IDCard_Lost]  WHERE [f_ConsumerID]=@ConsumerID\r\nDELETE FROM [t_b_Consumer_Other]  WHERE [f_ConsumerID]=@ConsumerID\r\nDELETE FROM [t_b_Consumer_delete]  WHERE [f_ConsumerID]=@ConsumerID\r\nDELETE FROM [t_b_Consumer] WHERE f_ConsumerNo = @consumerNO\r\n\r\nCOMMIT TRANSACTION\r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex50)
				{
					Program.wgToolsWgDebugWrite(ex50.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerDelete\r\n(\r\n@ConsumerNO nvarchar(20)\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\nDECLARE @CardNOOld  as bigint  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSET @CardNOOld = 0\r\nSELECT @consumerID=f_ConsumerID,@CardNOOld = f_CardNO  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @consumerID = 0\r\n      RETURN(103)   \r\n\r\n      \r\nBEGIN TRANSACTION\r\nIF @CardNOOld >0\r\nBEGIN\r\n DELETE FROM [t_b_IDCard_Lost] WHERE [f_CardNO]=@CardNOOld  \r\n INSERT INTO t_b_IDCard_Lost ([f_ConsumerID], [f_CardNO]) VALUES(@consumerID, @CardNOOld)\r\nEND\r\n\r\n\r\nINSERT INTO t_b_Consumer_Delete ([f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID]) \r\nSELECT [f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID] FROM [t_b_Consumer] \r\nWHERE [f_ConsumerNo]= @consumerNO\r\n\r\nDELETE FROM [t_b_Consumer] WHERE f_ConsumerNo = @consumerNO\r\n\r\nCOMMIT TRANSACTION\r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex51)
				{
					Program.wgToolsWgDebugWrite(ex51.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE [sp_wg2014_ConsumerEdit]\r\n(\r\n--工号 [不能为空]\r\n@ConsumerNO nvarchar(20),\r\n--姓名 [不能为空]\r\n@ConsumerName nvarchar(50),\r\n--部门 [可以为空, 不为空时必须是已存在的]\r\n@GroupName nvarchar(2047)=NULL,\r\n--启用考勤(缺省启用为1)\r\n@AttendEnabled int = NULL,\r\n--启用倒班(缺省不启用为0)\r\n@ShiftEnabled int = NULL,\r\n--启用门禁(缺省启用为1)\r\n@DoorEnabled int = NULL, \r\n--起始日期 yyyy-MM-dd\r\n@BeginYMD [nvarchar](50) = NULL,\r\n--截止日期 yyyy-MM-dd HH:mm[可以精确到分钟]\r\n@EndYMD [nvarchar](50) = NULL,\r\n--密码 [最多6位数字]\r\n@PIN  int = NULL,\r\n--权限类型\r\n--@PrivilegeTypeID int  = NULL,\r\n--备注\r\n@Note ntext = NULL,\r\n--职称\r\n@Title [nvarchar](50) = NULL,\r\n--学历\r\n@Culture [nvarchar](50) = NULL,\r\n--籍贯\r\n@Hometown [nvarchar](50) = NULL,\r\n--出生年月日\r\n@Birthday [nvarchar](50) = NULL,\r\n--婚姻状况\r\n@Marriage [nvarchar](50) = NULL,\r\n--入职时间\r\n@JoinDate [nvarchar](50) = NULL,\r\n--离职时间\r\n@LeaveDate [nvarchar](50)= NULL,\r\n--证件名称\r\n@CertificateType [nvarchar](50) = NULL,\r\n--证件号\r\n@CertificateID [nvarchar](50) = NULL,\r\n--社保号\r\n@SocialInsuranceNo [nvarchar](50) = NULL,\r\n--地址\r\n@Addr [nvarchar](50) = NULL,\r\n--邮编\r\n@Postcode [nvarchar](50) = NULL,\r\n--性别\r\n@Sex [nvarchar](50) = NULL,\r\n--民族\r\n@Nationality [nvarchar](50) = NULL,\r\n--宗教\r\n@Religion [nvarchar](50) = NULL,\r\n--英文名\r\n@EnglishName [nvarchar](50) = NULL,\r\n--手机\r\n@Mobile [nvarchar](50) = NULL,\r\n--家庭电话\r\n@HomePhone [nvarchar](50) = NULL,\r\n--工作电话\r\n@Telephone [nvarchar](50) = NULL,\r\n--电子邮箱\r\n@Email [nvarchar](50) = NULL,\r\n--政治面貌\r\n@Political [nvarchar](50) = NULL,\r\n--单位\r\n@CorporationName [nvarchar](50) = NULL,\r\n--技术等级\r\n@TechGrade [nvarchar](50) = NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @groupID  as int  \r\nDECLARE @consumerID  as int  \r\nDECLARE @strSQL as nvarchar(2047)\r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSELECT @consumerID=f_ConsumerID  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @consumerID = 0\r\n   RETURN(103)   \r\n\r\n      \r\nIF @ConsumerName IS NULL\r\nBEGIN\r\n   RETURN(201)\r\nEND\r\nELSE\r\nBEGIN\r\n   IF @ConsumerName = ''\r\n   BEGIN\r\n    RETURN(201)\r\n   END\r\nEND\r\n\r\nIF @GroupName IS NULL\r\n  SET @groupID=NULL\r\nELSE\r\nBEGIN\r\n IF @GroupName = ''\r\n   SET @groupID = 0\r\n ELSE\r\n  BEGIN\r\n   -- Make sure the GroupName is valid.\r\n   SELECT @groupID=f_GroupID FROM t_b_Group WHERE f_GroupName = @GroupName \r\n   IF @groupID IS NULL\r\n      RETURN(401)\r\n   IF @groupID = 0\r\n      RETURN(401)\r\n  END\r\nEND\r\n\r\nset @strSQL = 'f_ConsumerName =  ''' + @ConsumerName + ''''\r\nIF @groupID IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_GroupID =  ' + Str(@groupID)\r\n\r\nIF @AttendEnabled IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_AttendEnabled =  ' + Str(@AttendEnabled)\r\n\r\nIF @ShiftEnabled IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_ShiftEnabled =  ' + Str(@ShiftEnabled)\r\n\r\nIF @DoorEnabled IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_DoorEnabled =  ' + Str(@DoorEnabled)\r\n\r\nIF @BeginYMD IS NOT NULL\r\nBEGIN\r\n  set @strSQL = @strSQL + ',' + 'f_BeginYMD =  CONVERT( datetime, ''' + @BeginYMD + ''')'\r\nEND\r\n\r\nIF @EndYMD IS NOT NULL\r\nBEGIN\r\n  set @strSQL = @strSQL + ',' + 'f_EndYMD =  CONVERT( datetime, ''' + @EndYMD + ''')'\r\nEND\r\n\r\nIF @PIN IS NOT NULL\r\nBEGIN\r\n  set @strSQL = @strSQL + ',' + 'f_PIN =  ' + CONVERT( nvarchar(10), @PIN)\r\nEND\r\n\r\nBEGIN TRANSACTION\r\n\r\nset @strSQL = ' UPDATE t_b_Consumer  SET  ' + @strSQL + ' WHERE [f_ConsumerNO] = ''' + @consumerNO +''''\r\nEXEC(@strSQL)\r\n\r\nset @strSQL = ''\r\nIF @Title IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Title =  ''' + @Title + ''''\r\nIF @Culture IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Culture =  ''' + @Culture + ''''\r\nIF @Hometown IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Hometown =  ''' + @Hometown + ''''\r\n\r\nIF @Birthday IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Birthday =  ''' + @Birthday + ''''\r\nIF @Marriage IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Marriage =  ''' + @Marriage + ''''\r\nIF @JoinDate IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_JoinDate =  ''' + @JoinDate + ''''\r\nIF @LeaveDate IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_LeaveDate =  ''' + @LeaveDate + ''''\r\nIF @CertificateType IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_CertificateType =  ''' + @CertificateType + ''''\r\nIF @CertificateID IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_CertificateID =  ''' + @CertificateID + ''''\r\nIF @SocialInsuranceNo IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_SocialInsuranceNo =  ''' + @SocialInsuranceNo + ''''\r\nIF @Addr IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Addr =  ''' + @Addr + ''''\r\nIF @Postcode IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Postcode =  ''' + @Postcode + ''''\r\nIF @Sex IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Sex =  ''' + @Sex + ''''\r\nIF @Nationality IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Nationality =  ''' + @Nationality + ''''\r\nIF @Religion IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Religion =  ''' + @Religion + ''''\r\nIF @EnglishName IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_EnglishName =  ''' + @EnglishName + ''''\r\nIF @Mobile IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Mobile =  ''' + @Mobile + ''''\r\nIF @HomePhone IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_HomePhone =  ''' + @HomePhone + ''''\r\nIF @Telephone IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Telephone =  ''' + @Telephone + ''''\r\nIF @Email IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Email =  ''' + @Email + ''''\r\nIF @Political IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Political =  ''' + @Political + ''''\r\nIF @CorporationName IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_CorporationName =  ''' + @CorporationName + ''''\r\nIF @TechGrade IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_TechGrade =  ''' + @TechGrade + ''''\r\nIF @Note IS NOT NULL\r\n  set @strSQL = @strSQL + ',' + 'f_Note =  ''' + Convert(nvarchar(2047),@Note) + ''''\r\nIF @strSQL <> ''\r\nbegin\r\n  set @strSQL = substring(@strSQL,2,len(@strsql)-1) \r\n  set @strSQL = ' UPDATE t_b_Consumer_Other  SET  ' + @strSQL + ' WHERE [f_ConsumerID] = ' + Convert(nvarchar(20),@ConsumerID )\r\nEXEC(@strSQL)\r\nEND\r\n\r\nCOMMIT TRANSACTION\r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex52)
				{
					Program.wgToolsWgDebugWrite(ex52.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerNew\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@ConsumerName nvarchar(50),\r\n@CardNO bigint = NULL,\r\n@GroupName nvarchar(2047)=NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @groupID  as int  \r\nDECLARE @consumerID  as int  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @reccount =0\r\nSELECT @reccount=count(*) FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @reccount >0\r\n   RETURN(102)   \r\n    \r\nIF @ConsumerName IS NULL\r\nBEGIN\r\n   RETURN(201)\r\nEND\r\nELSE\r\nBEGIN\r\n   IF @ConsumerName = ''\r\n   BEGIN\r\n    RETURN(201)\r\n   END\r\nEND\r\n\r\nIF @CardNO IS NOT NULL\r\nBEGIN\r\n IF (@CardNO <= 100 OR @CardNO >= 4294967295 )\r\n  RETURN(303)\r\n \r\n SET @reccount =0\r\n SELECT @reccount=f_CardNO FROM t_b_Consumer \r\n WHERE f_CardNO= @CardNO \r\n IF @reccount IS NOT NULL AND @reccount >0\r\n   RETURN(302)         \r\nEND\r\n\r\n\r\nIF @GroupName IS NULL\r\n  SET @groupID=0\r\nELSE\r\nBEGIN\r\n IF @GroupName = ''\r\n   SET @groupID = 0\r\n ELSE\r\n  BEGIN\r\n   -- Make sure the GroupName is valid.\r\n   SELECT @groupID=f_GroupID FROM t_b_Group WHERE f_GroupName = @GroupName \r\n   IF @groupID IS NULL\r\n      RETURN(401)\r\n   IF @groupID = 0\r\n      RETURN(401)\r\n  END\r\nEND\r\n\r\nBEGIN TRANSACTION\r\nINSERT INTO [t_b_Consumer]([f_ConsumerNO], [f_ConsumerName], [f_CardNO], [f_GroupID], \r\n            [f_AttendEnabled], [f_DoorEnabled], [f_BeginYMD], [f_EndYMD],[f_PIN])\r\n            VALUES(@consumerNO,          \r\n                @consumerName, \r\n                @CardNO,\r\n                @groupID,\r\n                1 ,1 , CONVERT( datetime, '2000-01-01'), CONVERT( datetime, '2099-12-31'), '345678')     \r\n\r\nSELECT @consumerID= f_ConsumerID from [t_b_Consumer] where f_ConsumerNo = @consumerNO\r\nIF @consumerID IS NULL\r\nBEGIN\r\n  ROLLBACK TRANSACTION\r\n RETURN(9)\r\nEND\r\n\r\nINSERT INTO t_b_Consumer_Other (f_ConsumerID) values ( @consumerID)\r\n\r\nIF @CardNO IS NOT NULL \r\n   Delete from [t_b_IDCard_Lost] WHERE [f_CardNO]=@CardNO  \r\n\r\nSET @reccount =0\r\nSELECT @reccount=COUNT(*) FROM t_b_Consumer WHERE f_CardNO= @CardNO \r\nIF @reccount > 1 \r\nBEGIN\r\n  ROLLBACK TRANSACTION\r\n  RETURN(302)         \r\nEND\r\n\r\nCOMMIT TRANSACTION\r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex53)
				{
					Program.wgToolsWgDebugWrite(ex53.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerPrivilegeAdd\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@DoorName [nvarchar](50),\r\n@ControlSegID int = 1\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\nDECLARE @doorID  as int  \r\nDECLARE @controllerID as int\r\nDECLARE @doorNO as int\r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSELECT @consumerID=f_ConsumerID  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @consumerID = 0\r\n   RETURN(103)   \r\n\r\n\r\nIF @DoorName IS NULL\r\nBEGIN\r\n   RETURN(501)\r\nEND\r\n\r\nIF @DoorName = ''\r\nBEGIN\r\n  RETURN(501)\r\nEND\r\n   \r\nSET @DoorID = 0\r\nSELECT @DoorID=f_DoorID,@doorNO = f_DoorNO,@controllerID = f_ControllerID  FROM t_b_Door \r\nWHERE f_DoorName= @DoorName \r\nIF @DoorID = 0\r\n   RETURN(502)   \r\n\r\nIF @ControlSegID IS  NULL\r\n   SET @ControlSegID = 1 \r\n\r\nIF @ControlSegID IS NOT NULL\r\nBEGIN\r\n   IF (@ControlSegID  >= 255 )\r\n     RETURN(601)\r\nEND\r\n\r\nDELETE FROM  [t_d_Privilege]  WHERE f_ConsumerID = @ConsumerID AND f_DoorID = @DoorID \r\nINSERT INTO [t_d_Privilege] (f_ConsumerID, f_DoorID ,f_ControllerID, f_DoorNO, f_ControlSegID)\r\n       VALUES(@consumerID, @DoorID, @ControllerID, @DoorNO, @ControlSegID)\r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex54)
				{
					Program.wgToolsWgDebugWrite(ex54.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerPrivilegeClear\r\n(\r\n@ConsumerNO nvarchar(20)\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSELECT @consumerID=f_ConsumerID  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @consumerID = 0\r\n      RETURN(103)   \r\n\r\nDELETE FROM  [t_d_Privilege]  WHERE f_ConsumerID = @ConsumerID\r\nDELETE FROM  [t_b_UserFloor]  WHERE f_ConsumerID = @ConsumerID  \r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex55)
				{
					Program.wgToolsWgDebugWrite(ex55.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerRegisterCard\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@CardNO bigint\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\nDECLARE @CardNOOld  as bigint  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSET @CardNOOld = 0\r\nSELECT @consumerID=f_ConsumerID,@CardNOOld = f_CardNO  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @consumerID = 0\r\n      RETURN(103)   \r\n\r\nIF @CardNOOld >0\r\n      RETURN(304)\r\n\r\nIF @CardNO IS  NULL\r\n   RETURN(301)   \r\n\r\nIF @CardNO IS NOT NULL\r\nBEGIN\r\nIF (@CardNO <= 100 OR @CardNO >= 4294967295 )\r\n RETURN(303)\r\n\r\nSET @reccount =0\r\nSELECT @reccount=f_CardNO FROM t_b_Consumer \r\nWHERE f_CardNO= @CardNO \r\nIF @reccount IS NOT NULL AND @reccount >0\r\n  RETURN(302)         \r\nEND\r\n\r\nUPDATE t_b_Consumer SET  [f_CardNO]=@CardNO WHERE f_ConsumerNo = @consumerNO\r\n\r\nDELETE FROM [t_b_IDCard_Lost] WHERE [f_CardNO]=@CardNO  \r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex56)
				{
					Program.wgToolsWgDebugWrite(ex56.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerRegisterLostCard\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@NewCardNO bigint = NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\nDECLARE @CardNOOld  as bigint  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSET @CardNOOld = 0\r\nSELECT @consumerID=f_ConsumerID,@CardNOOld = f_CardNO  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\n\r\nIF @consumerID = 0\r\n      RETURN(103)   \r\n\r\nIF @CardNOOld IS NULL OR @CardNOOld =0\r\n      RETURN(305)\r\n           \r\nIF @NewCardNO IS NOT NULL\r\nBEGIN\r\n IF (@NewCardNO <= 100 OR @NewCardNO >= 4294967295 )\r\n  RETURN(303)\r\n \r\n SET @reccount =0\r\n SELECT @reccount=f_CardNO FROM t_b_Consumer \r\n WHERE f_CardNO= @NewCardNO \r\n IF @reccount IS NOT NULL AND @reccount >0\r\n   RETURN(302)         \r\nEND\r\n\r\n\r\nBEGIN TRANSACTION\r\nIF @CardNOOld >0\r\nBEGIN\r\n DELETE FROM [t_b_IDCard_Lost] WHERE [f_CardNO]=@CardNOOld  \r\n INSERT INTO t_b_IDCard_Lost ([f_ConsumerID], [f_CardNO]) VALUES(@consumerID, @CardNOOld)\r\nEND\r\n\r\nUPDATE t_b_Consumer SET  [f_CardNO]=@NewCardNO WHERE f_ConsumerNo = @consumerNO\r\n\r\nIF @NewCardNO IS NOT NULL\r\n DELETE FROM [t_b_IDCard_Lost] WHERE [f_CardNO] = @NewCardNO  \r\n\r\nCOMMIT TRANSACTION\r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex57)
				{
					Program.wgToolsWgDebugWrite(ex57.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerPrivilegeOneToMoreAdd\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@FloorName [nvarchar](50),\r\n@ControlSegID int = 1\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\nDECLARE @FloorID  as int  \r\nDECLARE @controllerID as int\r\nDECLARE @MoreFloorNum as int\r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSELECT @consumerID=f_ConsumerID  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @consumerID = 0\r\n   RETURN(103)   \r\n\r\n\r\nIF @FloorName IS NULL\r\nBEGIN\r\n   RETURN(501)\r\nEND\r\n\r\nIF @FloorName = ''\r\nBEGIN\r\n  RETURN(501)\r\nEND\r\n   \r\nSET @FloorID = 0;\r\nSET @MoreFloorNum =0;\r\n\r\nSELECT @FloorID=a.f_FloorID, @controllerID = b.f_ControllerID  FROM t_b_floor a\r\n, t_b_Controller b,t_b_Door c WHERE c.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID and a.f_DoorID = c.f_DoorID  \r\nAND (c.f_DoorName + '.' + a.f_floorName) = @FloorName \r\nIF @FloorID = 0\r\n   RETURN(502)   \r\n\r\nIF @ControlSegID IS  NULL\r\n   SET @ControlSegID = 1 \r\n\r\nIF @ControlSegID IS NOT NULL\r\nBEGIN\r\n   IF (@ControlSegID  >= 255 )\r\n     RETURN(601)\r\nEND\r\nBEGIN TRANSACTION\r\n\r\nDELETE FROM  [t_b_UserFloor]  WHERE f_ConsumerID = @ConsumerID AND f_floorID = @floorID \r\nINSERT INTO [t_b_UserFloor] (f_ConsumerID, f_floorID , f_ControlSegID, f_MoreFloorNum)\r\n       VALUES(@consumerID, @floorID, @ControlSegID, 1)\r\nSELECT @MoreFloorNum = COUNT(*) FROM   t_b_UserFloor WHERE f_ConsumerID = @ConsumerID AND f_floorID = @floorID\r\nIF (@MoreFloorNum  >0)\r\nBEGIN\r\nUPDATE t_b_UserFloor SET f_MoreFloorNum = @MoreFloorNum FROM   t_b_UserFloor WHERE f_ConsumerID = @ConsumerID AND f_floorID = @floorID\r\nCOMMIT TRANSACTION\r\n\r\nEND\r\nELSE\r\nBEGIN\r\nROLLBACK TRANSACTION\r\nEND     \r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex58)
				{
					Program.wgToolsWgDebugWrite(ex58.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_DepartmentTopNew\r\n(\r\n@GroupName nvarchar(2047)=NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @groupID  as int  \r\nDECLARE @start  as int  \r\n\r\n-- Validate parameter.\r\nIF @GroupName IS NULL\r\nBEGIN\r\n   RETURN(701)\r\nEND\r\n\r\nIF Rtrim(Ltrim(@GroupName)) = ''\r\n   RETURN(701)\r\n\r\nIF charIndex('\\', @GroupName, 0) > 0\r\n   RETURN(701)\r\n   \r\n-- Make sure the GroupName is invalid.\r\nSELECT @groupID=f_GroupID FROM t_b_Group WHERE f_GroupName = Rtrim(Ltrim(@GroupName)) \r\nIF @groupID IS NOT NULL\r\n  RETURN(702)\r\n\r\nINSERT INTO t_b_Group (f_GroupName) values (Rtrim(Ltrim(@GroupName)))\r\n\r\n    \r\nset @start = 1\r\nset @groupID = 0\r\n\r\nDECLARE rs CURSOR FOR -- LOCAL SCROLL FOR   \r\nSELECT f_GroupID from [t_b_Group]  ORDER BY f_GroupName + '\\ ASC' \r\nOPEN rs   \r\nFETCH NEXT FROM rs INTO @groupID   \r\nWHILE @@FETCH_STATUS <> -1 -- =0   \r\n    BEGIN  \r\n    UPDATE t_b_Group SET f_GroupNO=  @start WHERE  f_GroupID= @groupID\r\n     set @start = @start+1\r\n    FETCH NEXT FROM rs INTO @groupID   \r\n    END -- END @@FETCH_STATUS   \r\nCLOSE rs   \r\nDEALLOCATE rs  \r\n\r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex59)
				{
					Program.wgToolsWgDebugWrite(ex59.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_DepartmentTopEdit\r\n(\r\n@GroupName nvarchar(2047)=NULL,\r\n@GroupNewName nvarchar(2047)=NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @groupID  as int  \r\n\r\n-- Validate parameter.\r\nIF @GroupName IS NULL\r\nBEGIN\r\n   RETURN(701)\r\nEND\r\n\r\nIF Rtrim(Ltrim(@GroupName)) = ''\r\n   RETURN(701)\r\n\r\nIF charIndex('\\', @GroupName, 0) > 0\r\n   RETURN(701)\r\n\r\nIF @GroupNewName IS NULL\r\nBEGIN\r\n   RETURN(701)\r\nEND\r\n\r\nIF Rtrim(Ltrim(@GroupNewName)) = ''\r\n   RETURN(701)\r\n\r\nIF charIndex('\\', @GroupNewName, 0) > 0\r\n   RETURN(701)\r\n      \r\n-- Make sure the GroupName is valid.\r\nSELECT @groupID=f_GroupID FROM t_b_Group WHERE f_GroupName = Rtrim(Ltrim(@GroupName)) \r\nIF @groupID IS NULL\r\n  RETURN(704)\r\n\r\nIF @groupID <= 0\r\n  RETURN(704)\r\n\r\nDECLARE @groupNewID  as int  \r\n\r\nSELECT @groupNewID=f_GroupID FROM t_b_Group WHERE f_GroupName = Rtrim(Ltrim(@GroupNewName)) \r\nIF @groupNewID IS NOT NULL\r\nbegin\r\n  IF (@groupNewID > 0)\r\n  begin\r\n  RETURN(702)\r\n  end\r\nend\r\n  \r\nset @groupID = 0\r\nDECLARE @groupNameOld nvarchar(2047)\r\nDECLARE rs CURSOR FOR -- LOCAL SCROLL FOR   \r\nSELECT f_GroupID,f_GroupName from [t_b_Group]  \r\n      WHERE (f_GroupName = (Rtrim(Ltrim(@GroupName))))\r\n             or (f_GroupName like (Rtrim(Ltrim(@GroupName))) + '\\%')\r\n       ORDER BY f_GroupName  + '\\  ASC'\r\nOPEN rs   \r\nFETCH NEXT FROM rs INTO @groupID,@groupNameOld  \r\nWHILE @@FETCH_STATUS <> -1 -- =0   \r\n    BEGIN  \r\n    IF len(@groupNameOld) > len((Rtrim(Ltrim(@GroupName))))\r\n    begin\r\n    UPDATE t_b_Group SET f_GroupName= @GroupNewName + Substring(@groupNameOld,len((Rtrim(Ltrim(@GroupName))))+1,2047)\r\n    WHERE  f_GroupID=  @groupID  \r\n    end\r\n    else\r\n    begin\r\n    UPDATE t_b_Group SET f_GroupName= @GroupNewName \r\n    WHERE  f_GroupID=  @groupID  \r\n    \r\n    end\r\n\r\n    FETCH NEXT FROM rs INTO @groupID,@groupNameOld   \r\n    END -- END @@FETCH_STATUS   \r\nCLOSE rs   \r\nDEALLOCATE rs  \r\n\r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex60)
				{
					Program.wgToolsWgDebugWrite(ex60.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_DepartmentBranchNew\r\n(\r\n@GroupName nvarchar(2047)=NULL,\r\n@BranchName nvarchar(2047)=NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @groupID  as int  \r\n\r\n-- Validate parameter.\r\nIF @GroupName IS NULL\r\nBEGIN\r\n   RETURN(701)\r\nEND\r\n\r\nIF Rtrim(Ltrim(@GroupName)) = ''\r\n   RETURN(701)\r\n\r\nIF @BranchName IS NULL\r\nBEGIN\r\n   RETURN(701)\r\nEND\r\n\r\nIF Rtrim(Ltrim(@BranchName)) = ''\r\n   RETURN(701)\r\n\r\nIF charIndex('\\', @BranchName, 0) > 0\r\n   RETURN(701)\r\n      \r\n-- Make sure the GroupName is valid.\r\nSELECT @groupID=f_GroupID FROM t_b_Group WHERE f_GroupName = Rtrim(Ltrim(@GroupName)) \r\nIF @groupID IS NULL\r\n  RETURN(704)\r\n\r\nIF @groupID <= 0\r\n  RETURN(704)\r\n\r\nDECLARE @groupNewID  as int  \r\n\r\nSELECT @groupNewID=f_GroupID FROM t_b_Group WHERE f_GroupName = Rtrim(Ltrim(@GroupName)) + '\\' + Rtrim(Ltrim(@BranchName)) \r\nIF @groupNewID IS NOT NULL\r\nbegin\r\n  IF (@groupNewID > 0)\r\n  begin\r\n  RETURN(702)\r\n  end\r\nend\r\n\r\nINSERT INTO t_b_Group (f_GroupName) values (Rtrim(Ltrim(@GroupName)) + '\\' + Rtrim(Ltrim(@BranchName)) )\r\n\r\n    \r\nDECLARE @start  as int  \r\nset @start = 1\r\nset @groupID = 0\r\n\r\nDECLARE rs CURSOR FOR -- LOCAL SCROLL FOR   \r\nSELECT f_GroupID from [t_b_Group]  ORDER BY f_GroupName + '\\ ASC' \r\nOPEN rs   \r\nFETCH NEXT FROM rs INTO @groupID   \r\nWHILE @@FETCH_STATUS <> -1 -- =0   \r\n    BEGIN  \r\n    UPDATE t_b_Group SET f_GroupNO=  @start WHERE  f_GroupID= @groupID\r\n     set @start = @start+1\r\n    FETCH NEXT FROM rs INTO @groupID   \r\n    END -- END @@FETCH_STATUS   \r\nCLOSE rs   \r\nDEALLOCATE rs    \r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex61)
				{
					Program.wgToolsWgDebugWrite(ex61.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_DepartmentBranchEdit\r\n(\r\n@GroupName nvarchar(2047)=NULL,\r\n@BranchName nvarchar(2047)=NULL,\r\n@BranchNewName nvarchar(2047)=NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @groupID  as int  \r\n\r\n-- Validate parameter.\r\nIF @GroupName IS NULL\r\nBEGIN\r\n   RETURN(701)\r\nEND\r\n\r\nIF Rtrim(Ltrim(@GroupName)) = ''\r\n   RETURN(701)\r\n\r\nIF @BranchName IS NULL\r\nBEGIN\r\n   RETURN(701)\r\nEND\r\n\r\nIF Rtrim(Ltrim(@BranchName)) = ''\r\n   RETURN(701)\r\n\r\nIF charIndex('\\', @BranchName, 0) > 0\r\n   RETURN(701)\r\n\r\n\r\nIF @BranchNewName IS NULL\r\nBEGIN\r\n   RETURN(701)\r\nEND\r\n\r\nIF Rtrim(Ltrim(@BranchNewName)) = ''\r\n   RETURN(701)\r\n\r\nIF charIndex('\\', @BranchNewName, 0) > 0\r\n   RETURN(701)\r\n            \r\n-- Make sure the GroupName is valid.\r\nSELECT @groupID=f_GroupID FROM t_b_Group WHERE f_GroupName = Rtrim(Ltrim(@GroupName)) \r\nIF @groupID IS NULL\r\n  RETURN(704)\r\n\r\nIF @groupID <= 0\r\n  RETURN(704)\r\n\r\n\r\nDECLARE @groupIDBranch  as int  \r\nSELECT @groupIDBranch=f_GroupID FROM t_b_Group WHERE f_GroupName = Rtrim(Ltrim(@GroupName)) + '\\' + Rtrim(Ltrim(@BranchName)) \r\nIF @groupIDBranch IS NULL\r\n  RETURN(704)\r\n\r\nIF @groupIDBranch <= 0\r\n  RETURN(704)\r\n\r\nDECLARE @groupNewID  as int  \r\n\r\nSELECT @groupNewID=f_GroupID FROM t_b_Group WHERE f_GroupName = Rtrim(Ltrim(@GroupName)) + '\\' + Rtrim(Ltrim(@BranchNewName)) \r\nIF @groupNewID IS NOT NULL\r\nbegin\r\n  IF (@groupNewID > 0)\r\n  begin\r\n  RETURN(702)\r\n  end\r\nend\r\n\r\nset @groupID = 0\r\nDECLARE @groupNameOld nvarchar(2047)\r\nDECLARE rs CURSOR FOR -- LOCAL SCROLL FOR   \r\nSELECT f_GroupID,f_GroupName from [t_b_Group]  \r\n      WHERE (f_GroupName = (Rtrim(Ltrim(@GroupName)) + '\\' + Rtrim(Ltrim(@BranchName)) )\r\n             or (f_GroupName like (Rtrim(Ltrim(@GroupName)) + '\\' + Rtrim(Ltrim(@BranchName)) ) + '\\%'))\r\n       ORDER BY f_GroupName  + '\\  ASC'\r\nOPEN rs   \r\nFETCH NEXT FROM rs INTO @groupID,@groupNameOld  \r\nWHILE @@FETCH_STATUS <> -1 -- =0   \r\n    BEGIN  \r\n    IF len(@groupNameOld) > len((Rtrim(Ltrim(@GroupName)) + '\\' + Rtrim(Ltrim(@BranchName)) ))\r\n    begin\r\n    UPDATE t_b_Group SET f_GroupName= (Rtrim(Ltrim(@GroupName)) + '\\' + Rtrim(Ltrim(@BranchNewName)) ) + Substring(@groupNameOld,len((Rtrim(Ltrim(@GroupName)) + '\\' + Rtrim(Ltrim(@BranchName)) ))+1,2047)\r\n    WHERE  f_GroupID=  @groupID  \r\n    end\r\n    else\r\n    begin\r\n    UPDATE t_b_Group SET f_GroupName= (Rtrim(Ltrim(@GroupName)) + '\\' + Rtrim(Ltrim(@BranchNewName)) )\r\n    WHERE  f_GroupID=  @groupID  \r\n    \r\n    end\r\n\r\n    FETCH NEXT FROM rs INTO @groupID,@groupNameOld   \r\n    END -- END @@FETCH_STATUS   \r\nCLOSE rs   \r\nDEALLOCATE rs  \r\n         \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex62)
				{
					Program.wgToolsWgDebugWrite(ex62.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_DepartmentDelete\r\n(\r\n@GroupName nvarchar(2047)=NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @groupID  as int  \r\n\r\n-- Validate parameter.\r\nIF @GroupName IS NULL\r\nBEGIN\r\n   RETURN(701)\r\nEND\r\n\r\nIF Rtrim(Ltrim(@GroupName)) = ''\r\n   RETURN(701)\r\n\r\n-- Make sure the GroupName is valid.\r\nSELECT @groupID=f_GroupID FROM t_b_Group WHERE f_GroupName = Rtrim(Ltrim(@GroupName)) \r\nIF @groupID IS NULL\r\n  RETURN(704)\r\n\r\nIF @groupID <= 0\r\n  RETURN(704)\r\n\r\n\r\nDELETE From [t_b_Group]  \r\n      WHERE (f_GroupName = (Rtrim(Ltrim(@GroupName)))\r\n             or (f_GroupName like (Rtrim(Ltrim(@GroupName)) ) + '\\%'))\r\n         \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex63)
				{
					Program.wgToolsWgDebugWrite(ex63.ToString());
				}
			}
			if (dbversion <= 83f)
			{
				try
				{
					if (Program.wgAppConfigIsAccessDB())
					{
						text = "ALTER TABLE t_d_shift_AttStatistic  ADD COLUMN   f_Notes MEMO  ";
					}
					else
					{
						text = "ALTER TABLE t_d_shift_AttStatistic  ADD  f_Notes  [ntext]  NULL  ";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex64)
				{
					Program.wgToolsWgDebugWrite(ex64.ToString());
				}
				try
				{
					if (Program.wgAppConfigIsAccessDB())
					{
						text = "ALTER TABLE t_d_AttStatistic  ADD COLUMN   f_Notes MEMO  ";
					}
					else
					{
						text = "ALTER TABLE t_d_AttStatistic  ADD  f_Notes  [ntext]  NULL  ";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex65)
				{
					Program.wgToolsWgDebugWrite(ex65.ToString());
				}
			}
			if (dbversion <= 85f)
			{
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE TABLE [dbo].[t_b_Consumer_Location](\r\n [f_ConsumerID] [int] NOT NULL,\r\n [f_Note] [ntext] NULL,\r\n [f_lastSwipe_CardNO] [bigint] NOT NULL CONSTRAINT [DF_Table_1_f_lastSwipe_CardNO]  DEFAULT ((0)),\r\n [f_lastSwipe_RecID] [int] NOT NULL CONSTRAINT [DF_Table_1_f_RecIDOflastSwipe]  DEFAULT ((0)),\r\n [f_lastSwipe_ReadDate] [datetime] NULL,\r\n [f_lastSwipe_ReaderID] [tinyint] NOT NULL CONSTRAINT [DF_Table_1_f_lastSwipe_InOut1]  DEFAULT ((0)),\r\n [f_lastSwipe_InOut] [tinyint] NOT NULL CONSTRAINT [DF_t_b_Consumer_Location_f_lastSwipe_InOut]  DEFAULT ((0)),\r\n [f_lastSwipe_ControllerSN] [int] NOT NULL CONSTRAINT [DF_t_b_Consumer_Location_f_lastSwipe_ControllerSN]  DEFAULT ((0)),\r\n [f_lastRemoteOpen_ReadDate] [datetime] NULL,\r\n [f_lastRemoteOpen_ControllerSN] [int] NOT NULL CONSTRAINT [DF_t_b_Consumer_Location_f_lastRemoteOpen_ControllerSN]  DEFAULT ((0)),\r\n [f_lastRemoteOpen_DoorNO] [tinyint] NOT NULL CONSTRAINT [DF_Table_1_f_lastRemoteOpen_DoorNO1]  DEFAULT ((0)),\r\n [f_Last_InOut] [tinyint] NOT NULL CONSTRAINT [DF_Table_1_f_lastRemoteOpen_InOut1]  DEFAULT ((0))\r\n)  ";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex66)
				{
					Program.wgToolsWgDebugWrite(ex66.ToString());
				}
			}
			if (dbversion <= 87.2f)
			{
				try
				{
					DataTable dataTable = null;
					try
					{
						if (Program.wgAppConfiggetValBySql("SELECT COUNT(*) FROM t_b_Consumer_Fingerprint") > 0)
						{
							try
							{
								DataSet dataSet = new DataSet();
								text = "SELECT * FROM t_b_Consumer_Fingerprint ORDER BY f_FingerNO ";
								using (DbConnection dbConnection2 = (Program.wgAppConfigIsAccessDB() ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
								{
									using (DbCommand dbCommand2 = (Program.wgAppConfigIsAccessDB() ? new OleDbCommand(text, (OleDbConnection)dbConnection2) : new SqlCommand(text, (SqlConnection)dbConnection2)))
									{
										using (DataAdapter dataAdapter = (Program.wgAppConfigIsAccessDB() ? new OleDbDataAdapter((OleDbCommand)dbCommand2) : new SqlDataAdapter((SqlCommand)dbCommand2)))
										{
											dataAdapter.Fill(dataSet);
										}
									}
								}
								dataTable = dataSet.Tables[0];
							}
							catch (Exception ex67)
							{
								Program.wgToolsWgDebugWrite(ex67.ToString());
							}
						}
						Program.wgAppConfigRunUpdateSql("DROP TABLE t_b_Consumer_Fingerprint");
					}
					catch (Exception ex68)
					{
						Program.wgToolsWgDebugWrite(ex68.ToString());
					}
					text = " CREATE TABLE [t_b_Consumer_Fingerprint] (";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_FingerID   AUTOINCREMENT NOT NULL ,[f_FingerNO] INT NOT NULL , [f_FingerInfo] MEMO  NULL  ,f_ConsumerID int , f_Description   TEXT (255)  NULL ,[f_RegisterTime] datetime NULL,          [f_Note] MEMO  NULL  ,  CONSTRAINT [PK_t_b_Consumer_Fingerprint] PRIMARY KEY ( [f_FingerNO])) ";
					}
					else
					{
						text += "f_FingerID   [int] IDENTITY (1, 1) NOT NULL , [f_FingerNO] INT NOT NULL  , [f_FingerInfo] [ntext] NULL   , f_ConsumerID int , f_Description   [nvarchar] (255)  NULL ,[f_RegisterTime] datetime NULL,          [f_Note] [ntext] NULL   ,  CONSTRAINT [PK_t_b_Consumer_Fingerprint] PRIMARY KEY ( [f_FingerNO])) ";
					}
					Program.wgAppConfigRunUpdateSql(text);
					if (dataTable != null)
					{
						for (int j = 0; j < dataTable.Rows.Count; j++)
						{
							text = " INSERT INTO t_b_Consumer_Fingerprint (f_FingerNO, f_FingerInfo, f_ConsumerID,f_Description,f_RegisterTime, f_Note ) values (";
							Program.wgAppConfigRunUpdateSql(string.Concat(new string[]
							{
								text,
								wgTools.SetObjToStr(dataTable.Rows[j]["f_FingerNO"]),
								",",
								wgTools.PrepareStrNUnicode(dataTable.Rows[j]["f_FingerInfo"]),
								",",
								wgTools.SetObjToStr(dataTable.Rows[j]["f_ConsumerID"]),
								",",
								wgTools.PrepareStrNUnicode(dataTable.Rows[j]["f_Description"]),
								",",
								wgTools.PrepareStr(dataTable.Rows[j]["f_RegisterTime"], true, "yyyy-MM-dd HH:mm:ss"),
								",",
								wgTools.PrepareStrNUnicode(dataTable.Rows[j]["f_Note"]),
								")"
							}));
						}
					}
				}
				catch (Exception ex69)
				{
					Program.wgToolsWgDebugWrite(ex69.ToString());
				}
				try
				{
					text = "CREATE TABLE t_b_Controller_FingerPrint ( ";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_ControllerID   AUTOINCREMENT NOT NULL ,f_ControllerSN int  NOT NULL   ,f_FingerPrintName TEXT (255)  NOT NULL DEFAULT 'FingerPrint' ,f_IP   TEXT (255)  NULL ,f_Port int  NOT NULL Default  60000  ,f_Enabled    int  NOT NULL Default 1  ,f_ReaderID   int  NOT NULL Default 1  ,f_Notes MEMO ,  CONSTRAINT [PK_t_b_Controller_FingerPrint] PRIMARY KEY ( [f_ControllerID])) ";
					}
					else
					{
						text += "f_ControllerID   [int] IDENTITY (1, 1) NOT NULL ,f_ControllerSN [int]  NOT NULL   ,f_FingerPrintName [nvarchar] (255)  NOT NULL DEFAULT ('FingerPrint') ,f_IP   [nvarchar] (255)  NULL  ,f_Port [int]  NOT NULL Default(60000)  ,f_Enabled    [int]  NOT NULL Default(1)  ,f_ReaderID   [int]  NOT NULL Default(1)  ,f_Notes [ntext]  NULL  ,  CONSTRAINT [PK_t_b_Controller_FingerPrint] PRIMARY KEY ( [f_ControllerID])) ";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex70)
				{
					Program.wgToolsWgDebugWrite(ex70.ToString());
				}
				try
				{
					text = "CREATE TABLE t_b_ThirdPartyNetDevice ( ";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_DeviceId   AUTOINCREMENT NOT NULL ,";
						text += "f_DeviceName TEXT (255)  NOT NULL DEFAULT 'ThirdPartyNetDevice' ,";
						text += "f_DeviceIP   TEXT (255)  NOT NULL DEFAULT '192.168.168.123' ,";
						text += "f_DevicePort int  NOT NULL Default  9001  ,";
						text += "f_DeviceChannel   int  NOT NULL Default 1  ,";
						text += "f_Enabled    int  NOT NULL Default 1  ,";
						text += "f_DeviceUser  TEXT (255)   NULL DEFAULT 'admin' ,";
						text += "f_DevicePassword   TEXT (255)   NULL DEFAULT '12345' ,";
						text += "f_DeviceType     TEXT (255)   NULL,";
						text += "f_Factory     TEXT (255) NOT  NULL,";
						text += "f_CardNOWorkNODiff     TEXT (255) NOT  NULL,";
						text += "f_ReaderID   int  NOT NULL Default 1  ,";
						text += "f_Notes MEMO ,";
						text += "  CONSTRAINT [PK_t_b_ThirdPartyNetDevice] PRIMARY KEY ( [f_DeviceId])) ";
					}
					else
					{
						text += "f_DeviceId   [int] IDENTITY (1, 1) NOT NULL ,";
						text += "f_DeviceName [nvarchar] (255)  NOT NULL DEFAULT ('ThirdPartyNetDevice') ,";
						text += "f_DeviceIP   [nvarchar] (255)  NOT NULL DEFAULT ('192.168.168.123') ,";
						text += "f_DevicePort [int]  NOT NULL Default(9001)  ,";
						text += "f_DeviceChannel   [int]  NOT NULL Default(1)  ,";
						text += "f_Enabled    [int]  NOT NULL Default(1)  ,";
						text += "f_DeviceUser   [nvarchar] (255)   NULL DEFAULT ('admin') ,";
						text += "f_DevicePassword   [nvarchar] (255)   NULL DEFAULT ('12345') ,";
						text += "f_DeviceType       [nvarchar] (255)   NULL ,";
						text += "f_Factory       [nvarchar] (255) NOT  NULL ,";
						text += "f_CardNOWorkNODiff       [nvarchar] (255) NOT  NULL ,";
						text += "f_ReaderID   [int]  NOT NULL Default(1)  ,";
						text += "f_Notes [ntext]  NULL  ,";
						text += "  CONSTRAINT [PK_t_b_ThirdPartyNetDevice] PRIMARY KEY ( [f_DeviceId])) ";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex71)
				{
					Program.wgToolsWgDebugWrite(ex71.ToString());
				}
				try
				{
					text = " CREATE TABLE [t_b_Consumer_Face] (";
					if (Program.wgAppConfigIsAccessDB())
					{
						text += "f_ConsumerID int ,  [f_FaceInfo] MEMO  NULL  , [f_Note] MEMO  NULL  ,  CONSTRAINT [PK_t_b_Consumer_Face] PRIMARY KEY ( [f_ConsumerID])) ";
					}
					else
					{
						text += " f_ConsumerID int ,  [f_FaceInfo] [ntext] NULL   , [f_Note] [ntext] NULL   ,  CONSTRAINT [PK_t_b_Consumer_Face] PRIMARY KEY ( [f_ConsumerID])) ";
					}
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex72)
				{
					Program.wgToolsWgDebugWrite(ex72.ToString());
				}
			}
			if (dbversion <= 91.1f && !Program.wgAppConfigIsAccessDB())
			{
				try
				{
					if (Program.wgAppConfiggetValBySql("SELECT COUNT(*) FROM t_s_wglog") > 100000)
					{
						Program.wgAppConfigRunUpdateSql("TRUNCATE TABLE t_s_wglog");
					}
					string text4 = null;
					SqlConnection.ClearAllPools();
					SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
					try
					{
						text4 = sqlConnection.Database;
					}
					catch
					{
					}
					sqlConnection.Close();
					text = " ALTER DATABASE [" + text4 + "]  SET  RECOVERY SIMPLE ";
					wgAppConfig.wgLogWithoutDB(text + " Start", EventLogEntryType.Information, null);
					Program.wgAppConfigRunUpdateSql(text);
					wgAppConfig.wgLogWithoutDB(text + " End", EventLogEntryType.Information, null);
					try
					{
						text = string.Format("DBCC SHRINKDATABASE([{0}], 11, TRUNCATEONLY) ", text4);
						wgAppConfig.wgLogWithoutDB(text + " Start", EventLogEntryType.Information, null);
						Program.wgAppConfigRunUpdateSql(text);
						wgAppConfig.wgLogWithoutDB(text + " End", EventLogEntryType.Information, null);
					}
					catch
					{
						wgAppConfig.wgLogWithoutDB(text + " failed???", EventLogEntryType.Information, null);
					}
				}
				catch (Exception ex73)
				{
					Program.wgToolsWgDebugWrite(ex73.ToString());
				}
			}
			if (dbversion <= 92f)
			{
				if (!(Program.wgAppConfigGetSystemParamByNO(199) == ""))
				{
					if (!(Program.wgAppConfigGetSystemParamByNO(199) == "0"))
					{
						goto IL_17D2;
					}
				}
				try
				{
					text = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(199,'Database Subverion','','1','')";
					Program.wgAppConfigRunUpdateSql(text);
				}
				catch (Exception ex74)
				{
					Program.wgToolsWgDebugWrite(ex74.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "DROP PROCEDURE [sp_wg2014_ConsumerNew]";
						Program.wgAppConfigRunUpdateSql(text);
						text = "DROP PROCEDURE [sp_wg2014_ConsumerRegisterCard]";
						Program.wgAppConfigRunUpdateSql(text);
						text = "DROP PROCEDURE [sp_wg2014_ConsumerRegisterLostCard]";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex75)
				{
					Program.wgToolsWgDebugWrite(ex75.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerNew\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@ConsumerName nvarchar(50),\r\n@CardNO bigint = NULL,\r\n@GroupName nvarchar(2047)=NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @groupID  as int  \r\nDECLARE @consumerID  as int  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @reccount =0\r\nSELECT @reccount=count(*) FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @reccount >0\r\n   RETURN(102)   \r\n    \r\nIF @ConsumerName IS NULL\r\nBEGIN\r\n   RETURN(201)\r\nEND\r\nELSE\r\nBEGIN\r\n   IF @ConsumerName = ''\r\n   BEGIN\r\n    RETURN(201)\r\n   END\r\nEND\r\n\r\nIF @CardNO IS NOT NULL\r\nBEGIN\r\n IF (@CardNO <= 100 OR @CardNO >= 9223372036854775807 )\r\n  RETURN(303)\r\n \r\n SET @reccount =0\r\n SELECT @reccount=f_CardNO FROM t_b_Consumer \r\n WHERE f_CardNO= @CardNO \r\n IF @reccount IS NOT NULL AND @reccount >0\r\n   RETURN(302)         \r\nEND\r\n\r\n\r\nIF @GroupName IS NULL\r\n  SET @groupID=0\r\nELSE\r\nBEGIN\r\n IF @GroupName = ''\r\n   SET @groupID = 0\r\n ELSE\r\n  BEGIN\r\n   -- Make sure the GroupName is valid.\r\n   SELECT @groupID=f_GroupID FROM t_b_Group WHERE f_GroupName = @GroupName \r\n   IF @groupID IS NULL\r\n      RETURN(401)\r\n   IF @groupID = 0\r\n      RETURN(401)\r\n  END\r\nEND\r\n\r\nBEGIN TRANSACTION\r\nINSERT INTO [t_b_Consumer]([f_ConsumerNO], [f_ConsumerName], [f_CardNO], [f_GroupID], \r\n            [f_AttendEnabled], [f_DoorEnabled], [f_BeginYMD], [f_EndYMD],[f_PIN])\r\n            VALUES(@consumerNO,          \r\n                @consumerName, \r\n                @CardNO,\r\n                @groupID,\r\n                1 ,1 , CONVERT( datetime, '2000-01-01'), CONVERT( datetime, '2099-12-31'), '345678')     \r\n\r\nSELECT @consumerID= f_ConsumerID from [t_b_Consumer] where f_ConsumerNo = @consumerNO\r\nIF @consumerID IS NULL\r\nBEGIN\r\n  ROLLBACK TRANSACTION\r\n RETURN(9)\r\nEND\r\n\r\nINSERT INTO t_b_Consumer_Other (f_ConsumerID) values ( @consumerID)\r\n\r\nIF @CardNO IS NOT NULL \r\n   Delete from [t_b_IDCard_Lost] WHERE [f_CardNO]=@CardNO  \r\n\r\nSET @reccount =0\r\nSELECT @reccount=COUNT(*) FROM t_b_Consumer WHERE f_CardNO= @CardNO \r\nIF @reccount > 1 \r\nBEGIN\r\n  ROLLBACK TRANSACTION\r\n  RETURN(302)         \r\nEND\r\n\r\nCOMMIT TRANSACTION\r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex76)
				{
					Program.wgToolsWgDebugWrite(ex76.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerRegisterCard\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@CardNO bigint\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\nDECLARE @CardNOOld  as bigint  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSET @CardNOOld = 0\r\nSELECT @consumerID=f_ConsumerID,@CardNOOld = f_CardNO  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @consumerID = 0\r\n      RETURN(103)   \r\n\r\nIF @CardNOOld >0\r\n      RETURN(304)\r\n\r\nIF @CardNO IS  NULL\r\n   RETURN(301)   \r\n\r\nIF @CardNO IS NOT NULL\r\nBEGIN\r\nIF (@CardNO <= 100 OR @CardNO >= 9223372036854775807 )\r\n RETURN(303)\r\n\r\nSET @reccount =0\r\nSELECT @reccount=f_CardNO FROM t_b_Consumer \r\nWHERE f_CardNO= @CardNO \r\nIF @reccount IS NOT NULL AND @reccount >0\r\n  RETURN(302)         \r\nEND\r\n\r\nUPDATE t_b_Consumer SET  [f_CardNO]=@CardNO WHERE f_ConsumerNo = @consumerNO\r\n\r\nDELETE FROM [t_b_IDCard_Lost] WHERE [f_CardNO]=@CardNO  \r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex77)
				{
					Program.wgToolsWgDebugWrite(ex77.ToString());
				}
				try
				{
					if (!Program.wgAppConfigIsAccessDB())
					{
						text = "CREATE PROCEDURE sp_wg2014_ConsumerRegisterLostCard\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@NewCardNO bigint = NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\nDECLARE @CardNOOld  as bigint  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSET @CardNOOld = 0\r\nSELECT @consumerID=f_ConsumerID,@CardNOOld = f_CardNO  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\n\r\nIF @consumerID = 0\r\n      RETURN(103)   \r\n\r\nIF @CardNOOld IS NULL OR @CardNOOld =0\r\n      RETURN(305)\r\n           \r\nIF @NewCardNO IS NOT NULL\r\nBEGIN\r\n IF (@NewCardNO <= 100 OR @NewCardNO >= 9223372036854775807 )\r\n  RETURN(303)\r\n \r\n SET @reccount =0\r\n SELECT @reccount=f_CardNO FROM t_b_Consumer \r\n WHERE f_CardNO= @NewCardNO \r\n IF @reccount IS NOT NULL AND @reccount >0\r\n   RETURN(302)         \r\nEND\r\n\r\n\r\nBEGIN TRANSACTION\r\nIF @CardNOOld >0\r\nBEGIN\r\n DELETE FROM [t_b_IDCard_Lost] WHERE [f_CardNO]=@CardNOOld  \r\n INSERT INTO t_b_IDCard_Lost ([f_ConsumerID], [f_CardNO]) VALUES(@consumerID, @CardNOOld)\r\nEND\r\n\r\nUPDATE t_b_Consumer SET  [f_CardNO]=@NewCardNO WHERE f_ConsumerNo = @consumerNO\r\n\r\nIF @NewCardNO IS NOT NULL\r\n DELETE FROM [t_b_IDCard_Lost] WHERE [f_CardNO] = @NewCardNO  \r\n\r\nCOMMIT TRANSACTION\r\n                                \r\nRETURN(0)";
						Program.wgAppConfigRunUpdateSql(text);
					}
				}
				catch (Exception ex78)
				{
					Program.wgToolsWgDebugWrite(ex78.ToString());
				}
				IL_17D2:
				if (Program.wgAppConfigGetSystemParamByNO(199) == "1")
				{
					try
					{
						text = "UPDATE t_a_SystemParam SET f_Value='2' WHERE f_No = 199 ";
						Program.wgAppConfigRunUpdateSql(text);
					}
					catch (Exception ex79)
					{
						Program.wgToolsWgDebugWrite(ex79.ToString());
					}
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "DROP PROCEDURE [sp_wg2018_ConsumerPrivilegeTypeEdit]";
							Program.wgAppConfigRunUpdateSql(text);
						}
					}
					catch (Exception ex80)
					{
						Program.wgToolsWgDebugWrite(ex80.ToString());
					}
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "CREATE PROCEDURE sp_wg2018_ConsumerPrivilegeTypeEdit\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@PrivilegeTypeName nvarchar(50)\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\nDECLARE @privilegeTypeID  as int  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n\r\n\r\nSET @consumerID = 0\r\nSELECT @consumerID=f_ConsumerID  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @consumerID = 0\r\n      RETURN(103) \r\n\r\nSET @privilegeTypeID = 0\r\nIF @PrivilegeTypeName IS NULL\r\nBEGIN\r\n   --RETURN(101)\r\n   SET @privilegeTypeID = 0\r\nEND\r\nELSE\r\nBEGIN\r\nIF @PrivilegeTypeName = ''\r\nBEGIN\r\n --RETURN(101)\r\n SET @privilegeTypeID = 0\r\nEND\r\nELSE\r\nBEGIN\r\nSELECT @privilegeTypeID=f_PrivilegeTypeID  FROM t_d_PrivilegeType \r\nWHERE f_PrivilegeTypeName = @PrivilegeTypeName \r\nIF @privilegeTypeID = 0\r\n      RETURN(602)   \r\nEND\r\nEND   \r\n  \r\n\r\n\r\n\r\nDELETE FROM [t_d_Privilege] WHERE [f_ConsumerID]=@consumerID  \r\n\r\nUPDATE t_b_Consumer SET  [f_PrivilegeTypeID]= @privilegeTypeID WHERE f_ConsumerNo = @consumerNO\r\n\r\nIF @privilegeTypeID > 0\r\nBEGIN\r\nINSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])\r\nSELECT t_b_Consumer.f_ConsumerID, t_d_Privilege_Of_PrivilegeType.f_DoorID,t_d_Privilege_Of_PrivilegeType.[f_ControlSegID] , t_d_Privilege_Of_PrivilegeType.[f_ControllerID], t_d_Privilege_Of_PrivilegeType.[f_DoorNO] \r\n FROM t_d_Privilege_Of_PrivilegeType, t_b_Consumer \r\n WHERE [t_b_Consumer].[f_ConsumerID] = @consumerID\r\n AND (t_d_Privilege_Of_PrivilegeType.f_ConsumerID)= @privilegeTypeID\r\nEND\r\n                                \r\nRETURN(0)";
							Program.wgAppConfigRunUpdateSql(text);
						}
					}
					catch (Exception ex81)
					{
						Program.wgToolsWgDebugWrite(ex81.ToString());
					}
				}
				if (Program.wgAppConfigGetSystemParamByNO(199) == "2")
				{
					try
					{
						text = "UPDATE t_a_SystemParam SET f_Value='3' WHERE f_No = 199 ";
						Program.wgAppConfigRunUpdateSql(text);
					}
					catch (Exception ex82)
					{
						Program.wgToolsWgDebugWrite(ex82.ToString());
					}
					try
					{
						text = "CREATE TABLE t_b_ControllerTaskList4Floor ( ";
						if (Program.wgAppConfigIsAccessDB())
						{
							text += "[f_Id]   AUTOINCREMENT NOT NULL ,[f_BeginYMD] datetime NOT NULL,         [f_EndYMD] datetime NOT NULL,           [f_OperateTime] datetime NOT NULL,           [f_Monday] byte NOT NULL,     [f_Tuesday] byte NOT NULL,     [f_Wednesday] byte NOT NULL,     [f_Thursday] byte NOT NULL,    [f_Friday] byte NOT NULL,     [f_Saturday] byte NOT NULL,     [f_Sunday] byte NOT NULL,     [f_DoorID] int NOT NULL,     [f_DoorControl] int NOT NULL,     f_Notes MEMO  ,f_FloorNames MEMO  ,  CONSTRAINT [PK_t_b_TaskList4Floor] PRIMARY KEY ( [f_ID])) ";
						}
						else
						{
							text += "[f_Id]   [int] IDENTITY (1, 1) NOT NULL ,[f_BeginYMD] [datetime] NOT NULL,         [f_EndYMD] [datetime] NOT NULL,           [f_OperateTime] [datetime] NOT NULL,           [f_Monday] [tinyint] NOT NULL,     [f_Tuesday] [tinyint] NOT NULL,     [f_Wednesday] [tinyint] NOT NULL,     [f_Thursday] [tinyint] NOT NULL,    [f_Friday] [tinyint] NOT NULL,     [f_Saturday] [tinyint] NOT NULL,     [f_Sunday] [tinyint] NOT NULL,     [f_DoorID] [int] NOT NULL,     [f_DoorControl] [int] NOT NULL,     f_Notes [ntext]  NULL  ,f_FloorNames [ntext]  NULL  ,  CONSTRAINT [PK_t_b_TaskList4Floor] PRIMARY KEY ( [f_ID])) ";
						}
						Program.wgAppConfigRunUpdateSql(text);
					}
					catch (Exception ex83)
					{
						Program.wgToolsWgDebugWrite(ex83.ToString());
					}
				}
				if (Program.wgAppConfigGetSystemParamByNO(199) == "3")
				{
					try
					{
						text = "UPDATE t_a_SystemParam SET f_Value='4' WHERE f_No = 199 ";
						Program.wgAppConfigRunUpdateSql(text);
					}
					catch (Exception ex84)
					{
						Program.wgToolsWgDebugWrite(ex84.ToString());
					}
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "ALTER TABLE [t_b_Controller]  ALTER COLUMN   [f_DoorNames] [nvarchar](1024) NULL ";
							Program.wgAppConfigRunUpdateSql(text);
							text = "ALTER TABLE [t_b_Door]  ALTER COLUMN   [f_DoorName] [nvarchar](1024) NULL ";
							Program.wgAppConfigRunUpdateSql(text);
							text = "ALTER TABLE [t_b_Reader]  ALTER COLUMN   [f_ReaderName] [nvarchar](1024) NULL ";
							Program.wgAppConfigRunUpdateSql(text);
						}
					}
					catch (Exception ex85)
					{
						Program.wgToolsWgDebugWrite(ex85.ToString());
					}
				}
				if (Program.wgAppConfigGetSystemParamByNO(199) == "4")
				{
					try
					{
						text = "UPDATE t_a_SystemParam SET f_Value='5' WHERE f_No = 199 ";
						Program.wgAppConfigRunUpdateSql(text);
					}
					catch (Exception ex86)
					{
						Program.wgToolsWgDebugWrite(ex86.ToString());
					}
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "DROP PROCEDURE [sp_wg2014_ConsumerNew]";
							Program.wgAppConfigRunUpdateSql(text);
							text = "DROP PROCEDURE [sp_wg2014_ConsumerRegisterCard]";
							Program.wgAppConfigRunUpdateSql(text);
							text = "DROP PROCEDURE [sp_wg2014_ConsumerRegisterLostCard]";
							Program.wgAppConfigRunUpdateSql(text);
						}
					}
					catch (Exception ex87)
					{
						Program.wgToolsWgDebugWrite(ex87.ToString());
					}
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "CREATE PROCEDURE sp_wg2014_ConsumerNew\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@ConsumerName nvarchar(50),\r\n@CardNO bigint = NULL,\r\n@GroupName nvarchar(2047)=NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @groupID  as int  \r\nDECLARE @consumerID  as int  \r\nDECLARE @CardNOExisted  as bigint  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @reccount =0\r\nSELECT @reccount=count(*) FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @reccount >0\r\n   RETURN(102)   \r\n    \r\nIF @ConsumerName IS NULL\r\nBEGIN\r\n   RETURN(201)\r\nEND\r\nELSE\r\nBEGIN\r\n   IF @ConsumerName = ''\r\n   BEGIN\r\n    RETURN(201)\r\n   END\r\nEND\r\n\r\nIF @CardNO IS NOT NULL\r\nBEGIN\r\n IF (@CardNO <= 100 OR @CardNO >= 9223372036854775807 )\r\n  RETURN(303)\r\n \r\n SET @CardNOExisted =0\r\n SELECT @CardNOExisted=f_CardNO FROM t_b_Consumer \r\n WHERE f_CardNO= @CardNO \r\n IF @CardNOExisted IS NOT NULL AND @CardNOExisted >0\r\n   RETURN(302)         \r\nEND\r\n\r\n\r\nIF @GroupName IS NULL\r\n  SET @groupID=0\r\nELSE\r\nBEGIN\r\n IF @GroupName = ''\r\n   SET @groupID = 0\r\n ELSE\r\n  BEGIN\r\n   -- Make sure the GroupName is valid.\r\n   SELECT @groupID=f_GroupID FROM t_b_Group WHERE f_GroupName = @GroupName \r\n   IF @groupID IS NULL\r\n      RETURN(401)\r\n   IF @groupID = 0\r\n      RETURN(401)\r\n  END\r\nEND\r\n\r\nBEGIN TRANSACTION\r\nINSERT INTO [t_b_Consumer]([f_ConsumerNO], [f_ConsumerName], [f_CardNO], [f_GroupID], \r\n            [f_AttendEnabled], [f_DoorEnabled], [f_BeginYMD], [f_EndYMD],[f_PIN])\r\n            VALUES(@consumerNO,          \r\n                @consumerName, \r\n                @CardNO,\r\n                @groupID,\r\n                1 ,1 , CONVERT( datetime, '2000-01-01'), CONVERT( datetime, '2099-12-31'), '345678')     \r\n\r\nSELECT @consumerID= f_ConsumerID from [t_b_Consumer] where f_ConsumerNo = @consumerNO\r\nIF @consumerID IS NULL\r\nBEGIN\r\n  ROLLBACK TRANSACTION\r\n RETURN(9)\r\nEND\r\n\r\nINSERT INTO t_b_Consumer_Other (f_ConsumerID) values ( @consumerID)\r\n\r\nIF @CardNO IS NOT NULL \r\n   Delete from [t_b_IDCard_Lost] WHERE [f_CardNO]=@CardNO  \r\n\r\nSET @reccount =0\r\nSELECT @reccount=COUNT(*) FROM t_b_Consumer WHERE f_CardNO= @CardNO \r\nIF @reccount > 1 \r\nBEGIN\r\n  ROLLBACK TRANSACTION\r\n  RETURN(302)         \r\nEND\r\n\r\nCOMMIT TRANSACTION\r\n                                \r\nRETURN(0)";
							Program.wgAppConfigRunUpdateSql(text);
						}
					}
					catch (Exception ex88)
					{
						Program.wgToolsWgDebugWrite(ex88.ToString());
					}
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "CREATE PROCEDURE sp_wg2014_ConsumerRegisterCard\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@CardNO bigint\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\nDECLARE @CardNOOld  as bigint  \r\nDECLARE @CardNOExisted  as bigint  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSET @CardNOOld = 0\r\nSELECT @consumerID=f_ConsumerID,@CardNOOld = f_CardNO  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\nIF @consumerID = 0\r\n      RETURN(103)   \r\n\r\nIF @CardNOOld >0\r\n      RETURN(304)\r\n\r\nIF @CardNO IS  NULL\r\n   RETURN(301)   \r\n\r\nIF @CardNO IS NOT NULL\r\nBEGIN\r\nIF (@CardNO <= 100 OR @CardNO >= 9223372036854775807 )\r\n RETURN(303)\r\n\r\nSET @CardNOExisted =0\r\nSELECT @CardNOExisted=f_CardNO FROM t_b_Consumer \r\nWHERE f_CardNO= @CardNO \r\nIF @CardNOExisted IS NOT NULL AND @CardNOExisted >0\r\n  RETURN(302)         \r\nEND\r\n\r\nUPDATE t_b_Consumer SET  [f_CardNO]=@CardNO WHERE f_ConsumerNo = @consumerNO\r\n\r\nDELETE FROM [t_b_IDCard_Lost] WHERE [f_CardNO]=@CardNO  \r\n                                \r\nRETURN(0)";
							Program.wgAppConfigRunUpdateSql(text);
						}
					}
					catch (Exception ex89)
					{
						Program.wgToolsWgDebugWrite(ex89.ToString());
					}
					try
					{
						if (!Program.wgAppConfigIsAccessDB())
						{
							text = "CREATE PROCEDURE sp_wg2014_ConsumerRegisterLostCard\r\n(\r\n@ConsumerNO nvarchar(20),\r\n@NewCardNO bigint = NULL\r\n)\r\nWITH ENCRYPTION\r\nAS\r\nDECLARE @reccount as int\r\nDECLARE @consumerID  as int  \r\nDECLARE @CardNOOld  as bigint  \r\nDECLARE @CardNOExisted  as bigint  \r\n\r\n-- Validate parameter.\r\nIF @ConsumerNO IS NULL\r\nBEGIN\r\n   RETURN(101)\r\nEND\r\n\r\nIF @ConsumerNO = ''\r\nBEGIN\r\n RETURN(101)\r\nEND\r\n   \r\nSET @consumerID = 0\r\nSET @CardNOOld = 0\r\nSELECT @consumerID=f_ConsumerID,@CardNOOld = f_CardNO  FROM t_b_Consumer \r\nWHERE f_ConsumerNO= @ConsumerNO \r\n\r\nIF @consumerID = 0\r\n      RETURN(103)   \r\n\r\nIF @CardNOOld IS NULL OR @CardNOOld =0\r\n      RETURN(305)\r\n           \r\nIF @NewCardNO IS NOT NULL\r\nBEGIN\r\n IF (@NewCardNO <= 100 OR @NewCardNO >= 9223372036854775807 )\r\n  RETURN(303)\r\n \r\n SET @CardNOExisted =0\r\n SELECT @CardNOExisted=f_CardNO FROM t_b_Consumer \r\n WHERE f_CardNO= @NewCardNO \r\n IF @CardNOExisted IS NOT NULL AND @CardNOExisted >0\r\n   RETURN(302)         \r\nEND\r\n\r\n\r\nBEGIN TRANSACTION\r\nIF @CardNOOld >0\r\nBEGIN\r\n DELETE FROM [t_b_IDCard_Lost] WHERE [f_CardNO]=@CardNOOld  \r\n INSERT INTO t_b_IDCard_Lost ([f_ConsumerID], [f_CardNO]) VALUES(@consumerID, @CardNOOld)\r\nEND\r\n\r\nUPDATE t_b_Consumer SET  [f_CardNO]=@NewCardNO WHERE f_ConsumerNo = @consumerNO\r\n\r\nIF @NewCardNO IS NOT NULL\r\n DELETE FROM [t_b_IDCard_Lost] WHERE [f_CardNO] = @NewCardNO  \r\n\r\nCOMMIT TRANSACTION\r\n                                \r\nRETURN(0)";
							Program.wgAppConfigRunUpdateSql(text);
						}
					}
					catch (Exception ex90)
					{
						Program.wgToolsWgDebugWrite(ex90.ToString());
					}
				}
				if (Program.wgAppConfigGetSystemParamByNO(199) == "5")
				{
					try
					{
						text = "UPDATE t_a_SystemParam SET f_Value='6' WHERE f_No = 199 ";
						Program.wgAppConfigRunUpdateSql(text);
					}
					catch (Exception ex91)
					{
						Program.wgToolsWgDebugWrite(ex91.ToString());
					}
					try
					{
						text = "CREATE TABLE t_b_ControllerNormalOpenTimeList ( ";
						if (Program.wgAppConfigIsAccessDB())
						{
							text += "[f_Id]   AUTOINCREMENT NOT NULL ,[f_BeginYMD] datetime NOT NULL,         [f_EndYMD] datetime NOT NULL,           [f_OperateTime] datetime NOT NULL,           [f_OperateTimeEnd] datetime NOT NULL,           [f_Monday] byte NOT NULL,     [f_Tuesday] byte NOT NULL,     [f_Wednesday] byte NOT NULL,     [f_Thursday] byte NOT NULL,    [f_Friday] byte NOT NULL,     [f_Saturday] byte NOT NULL,     [f_Sunday] byte NOT NULL,     [f_DoorID] int NOT NULL,     [f_DoorControl] int NOT NULL,     f_Notes MEMO  ,f_FloorNames MEMO  ,  CONSTRAINT [PK_t_b_ControllerNormalOpenTimeList] PRIMARY KEY ( [f_ID])) ";
						}
						else
						{
							text += "[f_Id]   [int] IDENTITY (1, 1) NOT NULL ,[f_BeginYMD] [datetime] NOT NULL,         [f_EndYMD] [datetime] NOT NULL,           [f_OperateTime] [datetime] NOT NULL,           [f_OperateTimeEnd] [datetime] NOT NULL,           [f_Monday] [tinyint] NOT NULL,     [f_Tuesday] [tinyint] NOT NULL,     [f_Wednesday] [tinyint] NOT NULL,     [f_Thursday] [tinyint] NOT NULL,    [f_Friday] [tinyint] NOT NULL,     [f_Saturday] [tinyint] NOT NULL,     [f_Sunday] [tinyint] NOT NULL,     [f_DoorID] [int] NOT NULL,     [f_DoorControl] [int] NOT NULL,     f_Notes [ntext]  NULL  ,f_FloorNames [ntext]  NULL  ,  CONSTRAINT [PK_t_b_ControllerNormalOpenTimeList] PRIMARY KEY ( [f_ID])) ";
						}
						Program.wgAppConfigRunUpdateSql(text);
					}
					catch (Exception ex92)
					{
						Program.wgToolsWgDebugWrite(ex92.ToString());
					}
					try
					{
						text = "CREATE TABLE t_s_wglog4DoorOpenTooLongWarn ( ";
						if (Program.wgAppConfigIsAccessDB())
						{
							text += "[f_RecID]   AUTOINCREMENT NOT NULL ,[f_EventType]  TEXT  (50) NULL,         [f_EventDesc] MEMO,           [f_UserID] int NULL,           [f_UserName] TEXT  (50) NULL,           [f_LogDateTime] datetime NULL ,      CONSTRAINT [PK_t_s_wglog4DoorOpenTooLongWarn] PRIMARY KEY ( [f_RecID])) ";
						}
						else
						{
							text += "[f_RecID]   [int] IDENTITY (1, 1) NOT NULL ,[f_EventType] [nvarchar] (50) NULL,         [f_EventDesc] [ntext]  NULL,           [f_UserID] [int] NOT NULL  DEFAULT (0),           [f_UserName] [nvarchar] (50) NULL,           [f_LogDateTime] [datetime] NULL ,      CONSTRAINT [PK_t_s_wglog4DoorOpenTooLongWarn] PRIMARY KEY ( [f_RecID])) ";
						}
						Program.wgAppConfigRunUpdateSql(text);
					}
					catch (Exception ex93)
					{
						Program.wgToolsWgDebugWrite(ex93.ToString());
					}
				}
				try
				{
					if (Program.wgAppConfigIsAccessDB())
					{
						text = string.Format("ALTER TABLE t_b_Controller ADD COLUMN [f_DoorsNames] TEXT(255) NULL ", new object[0]);
					}
					else
					{
						text = string.Format("ALTER TABLE t_b_Controller ADD [f_DoorsNames] [nvarchar](1024) NULL ", new object[0]);
					}
					Program.wgAppConfigRunUpdateSql(text);
					text = "Update t_b_Controller Set f_DoorsNames = f_DoorNames";
					Program.wgAppConfigRunUpdateSql(text);
					Program.wgAppConfigRunUpdateSql(string.Format("ALTER TABLE t_b_Controller drop COLUMN [f_DoorNames]", new object[0]));
				}
				catch (Exception ex94)
				{
					Program.wgToolsWgDebugWrite(ex94.ToString());
				}
			}
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x00223DCC File Offset: 0x00222DCC
		private static string wgAppConfigGetSystemParamByNO(int ParaNo)
		{
			return wgAppConfig.getSystemParamByNO(ParaNo);
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x00223DD4 File Offset: 0x00222DD4
		private static int wgAppConfiggetValBySql(string strSql)
		{
			return wgAppConfig.getValBySql(strSql);
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x00223DDC File Offset: 0x00222DDC
		private static bool wgAppConfigIsAccessDB()
		{
			return wgAppConfig.IsAccessDB;
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x00223DE3 File Offset: 0x00222DE3
		private static int wgAppConfigRunUpdateSql(string strSql)
		{
			return wgAppConfig.runUpdateSql(strSql);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x00223DEB File Offset: 0x00222DEB
		private static string wgToolsPrepareStr(object obj)
		{
			return wgTools.PrepareStrNUnicode(obj);
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x00223DF3 File Offset: 0x00222DF3
		private static string wgToolsPrepareStr(object obj, bool bDate, string dateFormat)
		{
			return wgTools.PrepareStr(obj, bDate, dateFormat);
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x00223DFD File Offset: 0x00222DFD
		private static void wgToolsWgDebugWrite(string info)
		{
			wgTools.WgDebugWrite(info, new object[0]);
		}

		// Token: 0x04003554 RID: 13652
		private const string defaultDBFileName = "n3k_default.sql";

		// Token: 0x04003555 RID: 13653
		private const string wgDatabaseDefaultNameOfAdroitor = "AccessData";

		// Token: 0x04003556 RID: 13654
		private static bool bSqlExress = false;

		// Token: 0x04003557 RID: 13655
		private static float dbVersionNewest = 93f;

		// Token: 0x04003558 RID: 13656
		public static int expcount = 0;

		// Token: 0x04003559 RID: 13657
		public static string expStrDayHour = "";

		// Token: 0x0400355A RID: 13658
		private static string g_cnStrAcc = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= {0}.mdb;User ID=admin;Password=;JET OLEDB:Database Password=168168", Application.StartupPath + "\\" + wgAppConfig.accessDbName);

		// Token: 0x0400355B RID: 13659
		private static byte[] IV4Database = null;

		// Token: 0x0400355C RID: 13660
		private static byte[] Key4Database = null;

		// Token: 0x0400355D RID: 13661
		public static Thread startSlowThreadUpg;
	}
}
