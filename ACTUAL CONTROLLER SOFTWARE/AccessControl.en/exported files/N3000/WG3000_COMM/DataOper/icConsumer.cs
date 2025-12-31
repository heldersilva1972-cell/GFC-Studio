using System;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.DataOper
{
	// Token: 0x0200021D RID: 541
	internal class icConsumer
	{
		// Token: 0x06000F4B RID: 3915 RVA: 0x0010F2C4 File Offset: 0x0010E2C4
		public int addNew(string newConsumerNO, string ConsumerName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.addNew_Acc(newConsumerNO, ConsumerName);
			}
			int num = -9;
			if (newConsumerNO == null)
			{
				return -401;
			}
			if (newConsumerNO == "")
			{
				return -401;
			}
			if (ConsumerName == null)
			{
				return -201;
			}
			if (ConsumerName == "")
			{
				return -201;
			}
			string text = newConsumerNO.PadLeft(10, ' ');
			try
			{
				string text2 = "BEGIN TRANSACTION";
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.ExecuteNonQuery();
						try
						{
							text2 = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_BeginYMD ) values (";
							text2 += wgTools.PrepareStrNUnicode(text);
							text2 += ",";
							text2 += wgTools.PrepareStrNUnicode(ConsumerName);
							text2 += ",";
							text2 += wgTools.PrepareStr("2012-01-01", true, this.gYMDFormat);
							text2 += ")";
							sqlCommand.CommandText = text2;
							sqlCommand.ExecuteNonQuery();
							text2 = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStrNUnicode(text);
							sqlCommand.CommandText = text2;
							int num2 = int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar()));
							this.gConsumerID = num2;
							text2 = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
							text2 += num2;
							text2 += ")";
							sqlCommand.CommandText = text2;
							sqlCommand.ExecuteNonQuery();
							text2 = "COMMIT TRANSACTION";
							sqlCommand.CommandText = text2;
							sqlCommand.ExecuteNonQuery();
							num = 1;
						}
						catch (Exception ex)
						{
							icConsumer.lastErrInfo = "strSql:  " + text2 + "\r\n" + ex.ToString();
							text2 = "ROLLBACK TRANSACTION";
							sqlCommand.CommandText = text2;
							sqlCommand.ExecuteNonQuery();
						}
						return num;
					}
				}
			}
			catch (Exception ex2)
			{
				this.wgDebugWrite(ex2.ToString());
				icConsumer.lastErrInfo = "\r\n" + ex2.ToString();
			}
			return num;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0010F524 File Offset: 0x0010E524
		public int addNew(string newConsumerNO, string ConsumerName, long CardNO, int deptID)
		{
			if (!icConsumer.isValidCardId(CardNO))
			{
				return -105;
			}
			if (wgAppConfig.IsAccessDB)
			{
				return this.addNew_Acc(newConsumerNO, ConsumerName, CardNO, deptID);
			}
			int num = -9;
			if (newConsumerNO == null)
			{
				return -401;
			}
			if (newConsumerNO == "")
			{
				return -401;
			}
			if (ConsumerName == null)
			{
				return -201;
			}
			if (ConsumerName == "")
			{
				return -201;
			}
			string text = newConsumerNO.PadLeft(10, ' ');
			try
			{
				string text2;
				if (CardNO > 0L)
				{
					text2 = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection))
						{
							sqlConnection.Open();
							if (int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar())) > 0)
							{
								return -103;
							}
							text2 = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
							sqlCommand.CommandText = text2;
							sqlCommand.ExecuteNonQuery();
						}
					}
				}
				text2 = "BEGIN TRANSACTION";
				using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand2 = new SqlCommand(text2, sqlConnection2))
					{
						sqlConnection2.Open();
						sqlCommand2.ExecuteNonQuery();
						try
						{
							text2 = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_GroupID, f_CardNO, f_BeginYMD,f_EndYMD) values (";
							text2 += wgTools.PrepareStrNUnicode(text);
							text2 += ",";
							text2 += wgTools.PrepareStrNUnicode(ConsumerName);
							text2 += ",";
							text2 += deptID.ToString();
							text2 += ",";
							text2 += ((CardNO > 0L) ? CardNO.ToString() : "NULL");
							text2 += ",";
							text2 += wgTools.PrepareStr("2016-01-01", true, icConsumer.gTimeSecondEnabled ? this.gYMDHHMMSSFormat : this.gYMDFormat);
							text2 += ",";
							text2 += wgTools.PrepareStr("2099-12-31 23:59:59", true, icConsumer.gTimeSecondEnabled ? this.gYMDHHMMSSFormat : this.gYMDFormat);
							text2 += ")";
							sqlCommand2.CommandText = text2;
							sqlCommand2.ExecuteNonQuery();
							text2 = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStrNUnicode(text);
							sqlCommand2.CommandText = text2;
							int num2 = int.Parse("0" + wgTools.SetObjToStr(sqlCommand2.ExecuteScalar()));
							this.gConsumerID = num2;
							text2 = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
							text2 += num2;
							text2 += ")";
							sqlCommand2.CommandText = text2;
							sqlCommand2.ExecuteNonQuery();
							int num3 = 0;
							if (CardNO > 0L)
							{
								text2 = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
								sqlCommand2.CommandText = text2;
								SqlDataReader sqlDataReader = sqlCommand2.ExecuteReader();
								while (sqlDataReader.Read())
								{
									num3++;
									if (num3 > 1)
									{
										break;
									}
								}
								sqlDataReader.Close();
								if (num3 <= 1)
								{
									text2 = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
									sqlCommand2.CommandText = text2;
									sqlDataReader = sqlCommand2.ExecuteReader();
									while (sqlDataReader.Read())
									{
										num3++;
										if (num3 > 1)
										{
											break;
										}
									}
									sqlDataReader.Close();
								}
							}
							if (num3 > 1)
							{
								text2 = "ROLLBACK TRANSACTION";
								sqlCommand2.CommandText = text2;
								sqlCommand2.ExecuteNonQuery();
								return num;
							}
							text2 = "COMMIT TRANSACTION";
							sqlCommand2.CommandText = text2;
							sqlCommand2.ExecuteNonQuery();
							return 1;
						}
						catch (Exception ex)
						{
							icConsumer.lastErrInfo = "strSql:  " + text2 + "\r\n" + ex.ToString();
							text2 = "ROLLBACK TRANSACTION";
							sqlCommand2.CommandText = text2;
							sqlCommand2.ExecuteNonQuery();
						}
						return num;
					}
				}
			}
			catch (Exception ex2)
			{
				icConsumer.lastErrInfo = ex2.ToString();
				this.wgDebugWrite(ex2.ToString());
			}
			return num;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0010F98C File Offset: 0x0010E98C
		public int addNew(string newConsumerNO, string ConsumerName, int GroupID, byte AttendEnabled, byte ShiftEnabled, byte DoorEnabled, DateTime BeginYMD, DateTime EndYMD, int PIN, long CardNO)
		{
			if (!icConsumer.isValidCardId(CardNO))
			{
				return -105;
			}
			if (wgTools.gWGYTJ && wgAppConfig.getValBySql("SELECT COUNT(*) FROM t_b_Consumer") >= 500)
			{
				return -500;
			}
			if (wgAppConfig.IsAccessDB)
			{
				return this.addNew_Acc(newConsumerNO, ConsumerName, GroupID, AttendEnabled, ShiftEnabled, DoorEnabled, BeginYMD, EndYMD, PIN, CardNO);
			}
			int num = -9;
			if (newConsumerNO == null)
			{
				return -401;
			}
			if (newConsumerNO == "")
			{
				return -401;
			}
			if (ConsumerName == null)
			{
				return -201;
			}
			if (ConsumerName == "")
			{
				return -201;
			}
			string text = "";
			string text2 = newConsumerNO.PadLeft(10, ' ');
			try
			{
				byte b = ShiftEnabled;
				if (AttendEnabled == 0)
				{
					b = 0;
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						if (CardNO > 0L)
						{
							text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
							sqlCommand.CommandText = text;
							if (int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar())) > 0)
							{
								return -103;
							}
							text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
						}
						text = "BEGIN TRANSACTION";
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
						wgTools.WriteLine("BEGIN TRANSACTION End: ");
						try
						{
							text = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_GroupID, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled,f_BeginYMD,f_EndYMD,f_PIN, f_CardNO) values (";
							text += wgTools.PrepareStrNUnicode(text2);
							text = text + "," + wgTools.PrepareStrNUnicode(ConsumerName);
							text = text + "," + GroupID.ToString();
							text = text + "," + AttendEnabled.ToString();
							text = text + "," + b.ToString();
							text = text + "," + DoorEnabled.ToString();
							text = text + "," + wgTools.PrepareStr(BeginYMD, true, icConsumer.gTimeSecondEnabled ? this.gYMDHHMMSSFormat : this.gYMDFormat);
							text = text + "," + wgTools.PrepareStr(EndYMD, true, this.gYMDHHMMSSFormat);
							text = text + "," + PIN.ToString();
							text = text + "," + ((CardNO > 0L) ? CardNO.ToString() : "NULL");
							text += ")";
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							wgTools.WriteLine("INSERT INTO t_b_Consumer End: ");
							text = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStrNUnicode(text2);
							sqlCommand.CommandText = text;
							int num2 = int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar()));
							wgTools.WriteLine("SELECT f_ConsumerID End: ");
							this.gConsumerID = num2;
							text = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
							text += num2;
							text += ")";
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							wgTools.WriteLine("INSERT INTO t_b_Consumer_Other End: ");
							int num3 = 0;
							if (CardNO > 0L)
							{
								text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
								sqlCommand.CommandText = text;
								SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
								while (sqlDataReader.Read())
								{
									num3++;
									if (num3 > 1)
									{
										break;
									}
								}
								sqlDataReader.Close();
								if (num3 <= 1)
								{
									text = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
									sqlCommand.CommandText = text;
									sqlDataReader = sqlCommand.ExecuteReader();
									while (sqlDataReader.Read())
									{
										num3++;
										if (num3 > 1)
										{
											break;
										}
									}
									sqlDataReader.Close();
								}
							}
							if (num3 > 1)
							{
								text = "ROLLBACK TRANSACTION";
								sqlCommand.CommandText = text;
								sqlCommand.ExecuteNonQuery();
							}
							else
							{
								text = "COMMIT TRANSACTION";
								sqlCommand.CommandText = text;
								sqlCommand.ExecuteNonQuery();
								num = 1;
							}
							wgTools.WriteLine("COMMIT TRANSACTION End: ");
						}
						catch (Exception ex)
						{
							icConsumer.lastErrInfo = "strSql:  " + text + "\r\n" + ex.ToString();
							text = "ROLLBACK TRANSACTION";
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							wgTools.WriteLine("ROLLBACK TRANSACTION End: ");
						}
						return num;
					}
				}
			}
			catch (Exception ex2)
			{
				icConsumer.lastErrInfo = ex2.ToString();
				this.wgDebugWrite(ex2.ToString());
			}
			return num;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0010FE2C File Offset: 0x0010EE2C
		public int addNew_Acc(string newConsumerNO, string ConsumerName)
		{
			int num = -9;
			if (newConsumerNO == null)
			{
				return -401;
			}
			if (newConsumerNO == "")
			{
				return -401;
			}
			if (ConsumerName == null)
			{
				return -201;
			}
			if (ConsumerName == "")
			{
				return -201;
			}
			string text = newConsumerNO.PadLeft(10, ' ');
			try
			{
				string text2 = "BEGIN TRANSACTION";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text2, oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.ExecuteNonQuery();
						try
						{
							text2 = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_BeginYMD ) values (";
							text2 += wgTools.PrepareStrNUnicode(text);
							text2 += ",";
							text2 += wgTools.PrepareStrNUnicode(ConsumerName);
							text2 += ",";
							text2 += wgTools.PrepareStr("2012-01-01", true, this.gYMDFormat);
							text2 += ")";
							oleDbCommand.CommandText = text2;
							text2 = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStrNUnicode(text);
							oleDbCommand.CommandText = text2;
							int num2 = int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()));
							this.gConsumerID = num2;
							text2 = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
							text2 += num2;
							text2 += ")";
							oleDbCommand.CommandText = text2;
							oleDbCommand.ExecuteNonQuery();
							text2 = "COMMIT TRANSACTION";
							oleDbCommand.CommandText = text2;
							oleDbCommand.ExecuteNonQuery();
							num = 1;
						}
						catch (Exception ex)
						{
							icConsumer.lastErrInfo = "strSql:  " + text2 + "\r\n" + ex.ToString();
							text2 = "ROLLBACK TRANSACTION";
							oleDbCommand.CommandText = text2;
							oleDbCommand.ExecuteNonQuery();
						}
						return num;
					}
				}
			}
			catch (Exception ex2)
			{
				icConsumer.lastErrInfo = ex2.ToString();
				this.wgDebugWrite(ex2.ToString());
			}
			return num;
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0011006C File Offset: 0x0010F06C
		public int addNew_Acc(string newConsumerNO, string ConsumerName, long CardNO, int deptID)
		{
			if (!icConsumer.isValidCardId(CardNO))
			{
				return -105;
			}
			int num = -9;
			if (newConsumerNO == null)
			{
				return -401;
			}
			if (newConsumerNO == "")
			{
				return -401;
			}
			if (ConsumerName == null)
			{
				return -201;
			}
			if (ConsumerName == "")
			{
				return -201;
			}
			string text = newConsumerNO.PadLeft(10, ' ');
			try
			{
				string text2;
				if (CardNO > 0L)
				{
					text2 = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text2, oleDbConnection))
						{
							oleDbConnection.Open();
							if (int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar())) > 0)
							{
								return -103;
							}
							text2 = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
							oleDbCommand.CommandText = text2;
							oleDbCommand.ExecuteNonQuery();
						}
					}
				}
				text2 = "BEGIN TRANSACTION";
				using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand(text2, oleDbConnection2))
					{
						oleDbConnection2.Open();
						oleDbCommand2.ExecuteNonQuery();
						try
						{
							text2 = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_GroupID, f_CardNO, f_BeginYMD, f_EndYMD) values (";
							text2 += wgTools.PrepareStrNUnicode(text);
							text2 += ",";
							text2 += wgTools.PrepareStrNUnicode(ConsumerName);
							text2 += ",";
							text2 += deptID.ToString();
							text2 += ",";
							text2 += ((CardNO > 0L) ? CardNO.ToString() : "NULL");
							text2 += ",";
							text2 += wgTools.PrepareStr("2016-01-01", true, icConsumer.gTimeSecondEnabled ? this.gYMDHHMMSSFormat : this.gYMDFormat);
							text2 += ",";
							text2 += wgTools.PrepareStr("2099-12-31 23:59:59", true, icConsumer.gTimeSecondEnabled ? this.gYMDHHMMSSFormat : this.gYMDFormat);
							text2 += ")";
							oleDbCommand2.CommandText = text2;
							oleDbCommand2.ExecuteNonQuery();
							text2 = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStrNUnicode(text);
							oleDbCommand2.CommandText = text2;
							int num2 = int.Parse("0" + wgTools.SetObjToStr(oleDbCommand2.ExecuteScalar()));
							this.gConsumerID = num2;
							text2 = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
							text2 += num2;
							text2 += ")";
							oleDbCommand2.CommandText = text2;
							oleDbCommand2.ExecuteNonQuery();
							int num3 = 0;
							if (CardNO > 0L)
							{
								text2 = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
								oleDbCommand2.CommandText = text2;
								OleDbDataReader oleDbDataReader = oleDbCommand2.ExecuteReader();
								while (oleDbDataReader.Read())
								{
									num3++;
									if (num3 > 1)
									{
										break;
									}
								}
								oleDbDataReader.Close();
								if (num3 <= 1)
								{
									text2 = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
									oleDbCommand2.CommandText = text2;
									oleDbDataReader = oleDbCommand2.ExecuteReader();
									while (oleDbDataReader.Read())
									{
										num3++;
										if (num3 > 1)
										{
											break;
										}
									}
									oleDbDataReader.Close();
								}
							}
							if (num3 > 1)
							{
								text2 = "ROLLBACK TRANSACTION";
								oleDbCommand2.CommandText = text2;
								oleDbCommand2.ExecuteNonQuery();
								return num;
							}
							text2 = "COMMIT TRANSACTION";
							oleDbCommand2.CommandText = text2;
							oleDbCommand2.ExecuteNonQuery();
							return 1;
						}
						catch (Exception ex)
						{
							icConsumer.lastErrInfo = "strSql:  " + text2 + "\r\n" + ex.ToString();
							text2 = "ROLLBACK TRANSACTION";
							oleDbCommand2.CommandText = text2;
							oleDbCommand2.ExecuteNonQuery();
						}
						return num;
					}
				}
			}
			catch (Exception ex2)
			{
				icConsumer.lastErrInfo = ex2.ToString();
				this.wgDebugWrite(ex2.ToString());
			}
			return num;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x001104C0 File Offset: 0x0010F4C0
		public int addNew_Acc(string newConsumerNO, string ConsumerName, int GroupID, byte AttendEnabled, byte ShiftEnabled, byte DoorEnabled, DateTime BeginYMD, DateTime EndYMD, int PIN, long CardNO)
		{
			if (!icConsumer.isValidCardId(CardNO))
			{
				return -105;
			}
			int num = -9;
			if (newConsumerNO == null)
			{
				return -401;
			}
			if (newConsumerNO == "")
			{
				return -401;
			}
			if (ConsumerName == null)
			{
				return -201;
			}
			if (ConsumerName == "")
			{
				return -201;
			}
			string text = "";
			string text2 = newConsumerNO.PadLeft(10, ' ');
			try
			{
				byte b = ShiftEnabled;
				if (AttendEnabled == 0)
				{
					b = 0;
				}
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						if (CardNO > 0L)
						{
							text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
							oleDbCommand.CommandText = text;
							if (int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar())) > 0)
							{
								return -103;
							}
							text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
						}
						text = "BEGIN TRANSACTION";
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
						wgTools.WriteLine("BEGIN TRANSACTION End: ");
						try
						{
							text = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_GroupID, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled,f_BeginYMD,f_EndYMD,f_PIN, f_CardNO) values (";
							text += wgTools.PrepareStrNUnicode(text2);
							text = text + "," + wgTools.PrepareStrNUnicode(ConsumerName);
							text = text + "," + GroupID.ToString();
							text = text + "," + AttendEnabled.ToString();
							text = text + "," + b.ToString();
							text = text + "," + DoorEnabled.ToString();
							text = text + "," + wgTools.PrepareStr(BeginYMD, true, icConsumer.gTimeSecondEnabled ? this.gYMDHHMMSSFormat : this.gYMDFormat);
							text = text + "," + wgTools.PrepareStr(EndYMD, true, this.gYMDHHMMSSFormat);
							text = text + "," + PIN.ToString();
							text = text + "," + ((CardNO > 0L) ? CardNO.ToString() : "NULL");
							text += ")";
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							wgTools.WriteLine("INSERT INTO t_b_Consumer End: ");
							text = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStrNUnicode(text2);
							oleDbCommand.CommandText = text;
							int num2 = int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()));
							wgTools.WriteLine("SELECT f_ConsumerID End: ");
							this.gConsumerID = num2;
							text = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
							text += num2;
							text += ")";
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							wgTools.WriteLine("INSERT INTO t_b_Consumer_Other End: ");
							int num3 = 0;
							if (CardNO > 0L)
							{
								text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
								oleDbCommand.CommandText = text;
								OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
								while (oleDbDataReader.Read())
								{
									num3++;
									if (num3 > 1)
									{
										break;
									}
								}
								oleDbDataReader.Close();
								if (num3 <= 1)
								{
									text = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
									oleDbCommand.CommandText = text;
									oleDbDataReader = oleDbCommand.ExecuteReader();
									while (oleDbDataReader.Read())
									{
										num3++;
										if (num3 > 1)
										{
											break;
										}
									}
									oleDbDataReader.Close();
								}
							}
							if (num3 > 1)
							{
								text = "ROLLBACK TRANSACTION";
								oleDbCommand.CommandText = text;
								oleDbCommand.ExecuteNonQuery();
							}
							else
							{
								text = "COMMIT TRANSACTION";
								oleDbCommand.CommandText = text;
								oleDbCommand.ExecuteNonQuery();
								num = 1;
							}
							wgTools.WriteLine("COMMIT TRANSACTION End: ");
						}
						catch (Exception ex)
						{
							icConsumer.lastErrInfo = "strSql:  " + text + "\r\n" + ex.ToString();
							text = "ROLLBACK TRANSACTION";
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							wgTools.WriteLine("ROLLBACK TRANSACTION End: ");
						}
						return num;
					}
				}
			}
			catch (Exception ex2)
			{
				icConsumer.lastErrInfo = ex2.ToString();
				this.wgDebugWrite(ex2.ToString());
			}
			return num;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00110924 File Offset: 0x0010F924
		public int addNewCard(int ConsumerID, long CardNO)
		{
			if (!icConsumer.isValidCardId(CardNO))
			{
				return -105;
			}
			if (wgAppConfig.IsAccessDB)
			{
				return this.addNewCard_Acc(ConsumerID, CardNO);
			}
			int num = -9;
			try
			{
				string text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						if (int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar())) > 0)
						{
							return -103;
						}
						text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
						text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
						sqlCommand.CommandText = text;
						if (int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar())) > 0)
						{
							return -103;
						}
						text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
						text = "UPDATE t_b_Consumer SET  [f_CardNO]= " + CardNO.ToString() + " WHERE f_ConsumerID = " + ConsumerID.ToString();
						sqlCommand.CommandText = text;
						if (sqlCommand.ExecuteNonQuery() == 1)
						{
							num = 1;
						}
						return num;
					}
				}
			}
			catch (Exception ex)
			{
				icConsumer.lastErrInfo = ex.ToString();
				this.wgDebugWrite(ex.ToString());
			}
			return num;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00110AD8 File Offset: 0x0010FAD8
		public int addNewCard_Acc(int ConsumerID, long CardNO)
		{
			if (!icConsumer.isValidCardId(CardNO))
			{
				return -105;
			}
			int num = -9;
			try
			{
				string text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						if (int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar())) > 0)
						{
							return -103;
						}
						text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
						text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
						oleDbCommand.CommandText = text;
						if (int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar())) > 0)
						{
							return -103;
						}
						text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
						text = "UPDATE t_b_Consumer SET  [f_CardNO]= " + CardNO.ToString() + " WHERE f_ConsumerID = " + ConsumerID.ToString();
						oleDbCommand.CommandText = text;
						if (oleDbCommand.ExecuteNonQuery() == 1)
						{
							num = 1;
						}
						return num;
					}
				}
			}
			catch (Exception ex)
			{
				icConsumer.lastErrInfo = ex.ToString();
				this.wgDebugWrite(ex.ToString());
			}
			return num;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00110C7C File Offset: 0x0010FC7C
		public long ConsumerNONext()
		{
			return this.ConsumerNONext("");
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x00110C8C File Offset: 0x0010FC8C
		public long ConsumerNONext(string startcaption)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.ConsumerNONext_Acc(startcaption);
			}
			long num = 0L;
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.CommandText = "SELECT [f_ConsumerNO] FROM [t_b_Consumer] ";
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							long num2 = -1L;
							string text = wgTools.SetObjToStr(sqlDataReader[0]);
							if (text == "")
							{
								num2 = 0L;
							}
							else
							{
								long.TryParse(text, out num2);
								if (num2 <= 0L && !string.IsNullOrEmpty(startcaption) && text.IndexOf(startcaption) == 0 && text.StartsWith(startcaption))
								{
									text = text.Substring(startcaption.Length);
									if (text == "")
									{
										num2 = 0L;
									}
									else
									{
										long.TryParse(text, out num2);
									}
								}
							}
							if (num < num2)
							{
								num = num2;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return num + 1L;
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x00110DBC File Offset: 0x0010FDBC
		public long ConsumerNONext_Acc(string startcaption)
		{
			long num = 0L;
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.CommandText = "SELECT [f_ConsumerNO] FROM [t_b_Consumer] ";
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							long num2 = -1L;
							string text = wgTools.SetObjToStr(oleDbDataReader[0]);
							if (text == "")
							{
								num2 = 0L;
							}
							else
							{
								long.TryParse(text, out num2);
								if (num2 <= 0L && !string.IsNullOrEmpty(startcaption) && text.IndexOf(startcaption) == 0 && text.StartsWith(startcaption))
								{
									text = text.Substring(startcaption.Length);
									if (text == "")
									{
										num2 = 0L;
									}
									else
									{
										long.TryParse(text, out num2);
									}
								}
							}
							if (num < num2)
							{
								num = num2;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return num + 1L;
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00110EDC File Offset: 0x0010FEDC
		public long ConsumerNONextWithSpace()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.ConsumerNONextWithSpace_Acc();
			}
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.CommandText = "SELECT max(Right('                    ' + [f_ConsumerNO],20)) FROM [t_b_Consumer] ";
						long num = -1L;
						string text = wgTools.SetObjToStr(sqlCommand.ExecuteScalar());
						if (text == "")
						{
							num = 1L;
						}
						else
						{
							long.TryParse(text, out num);
							if (num > 0L)
							{
								num += 1L;
							}
						}
						if (num > 0L)
						{
							sqlCommand.CommandText = "SELECT ([f_ConsumerNO]) FROM [t_b_Consumer] WHERE f_ConsumerNO=" + wgTools.PrepareStrNUnicode(num.ToString());
							if (wgTools.SetObjToStr(sqlCommand.ExecuteScalar()) == "")
							{
								return num;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return -1L;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00110FDC File Offset: 0x0010FFDC
		public long ConsumerNONextWithSpace_Acc()
		{
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.CommandText = "SELECT max(Right('                    ' + [f_ConsumerNO],20)) FROM [t_b_Consumer] ";
						long num = -1L;
						string text = wgTools.SetObjToStr(oleDbCommand.ExecuteScalar());
						if (text == "")
						{
							num = 1L;
						}
						else
						{
							long.TryParse(text, out num);
							if (num > 0L)
							{
								num += 1L;
							}
						}
						if (num > 0L)
						{
							oleDbCommand.CommandText = "SELECT ([f_ConsumerNO]) FROM [t_b_Consumer] WHERE f_ConsumerNO=" + wgTools.PrepareStrNUnicode(num.ToString());
							if (wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()) == "")
							{
								return num;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return -1L;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x001110CC File Offset: 0x001100CC
		public long ConsumerNONextWithZero()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.ConsumerNONextWithZero_Acc();
			}
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.CommandText = "SELECT max(Right('00000000000000000000' + RTRIM(LTRIM([f_ConsumerNO])),20)) FROM [t_b_Consumer] ";
						long num = -1L;
						string text = wgTools.SetObjToStr(sqlCommand.ExecuteScalar());
						if (text == "")
						{
							num = 1L;
						}
						else
						{
							long.TryParse(text, out num);
							if (num > 0L)
							{
								num += 1L;
							}
						}
						if (num > 0L)
						{
							sqlCommand.CommandText = "SELECT ([f_ConsumerNO]) FROM [t_b_Consumer] WHERE f_ConsumerNO=" + wgTools.PrepareStrNUnicode(num.ToString());
							if (wgTools.SetObjToStr(sqlCommand.ExecuteScalar()) == "")
							{
								return num;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return -1L;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x001111CC File Offset: 0x001101CC
		public long ConsumerNONextWithZero_Acc()
		{
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.CommandText = "SELECT max(Right('00000000000000000000' + RTRIM(LTRIM([f_ConsumerNO])),20)) FROM [t_b_Consumer] ";
						long num = -1L;
						string text = wgTools.SetObjToStr(oleDbCommand.ExecuteScalar());
						if (text == "")
						{
							num = 1L;
						}
						else
						{
							long.TryParse(text, out num);
							if (num > 0L)
							{
								num += 1L;
							}
						}
						if (num > 0L)
						{
							oleDbCommand.CommandText = "SELECT ([f_ConsumerNO]) FROM [t_b_Consumer] WHERE f_ConsumerNO=" + wgTools.PrepareStrNUnicode(num.ToString());
							if (wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()) == "")
							{
								return num;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return -1L;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x001112BC File Offset: 0x001102BC
		public int dimissionUser(int ConsumerID)
		{
			int num = -9;
			try
			{
				string text = "BEGIN TRANSACTION";
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand(text, dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand(text, dbConnection as SqlConnection);
				}
				dbConnection.Open();
				dbCommand.ExecuteNonQuery();
				try
				{
					wgTools.WriteLine(text);
					text = "SELECT f_CardNO FROM t_b_Consumer WHERE [f_ConsumerID]= " + ConsumerID.ToString();
					dbCommand.CommandText = text;
					long num2 = long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar()));
					if (num2 > 0L)
					{
						long num3 = num2;
						text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + num3.ToString();
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						text = "INSERT INTO t_b_IDCard_Lost ([f_ConsumerID], [f_CardNO]) VALUES( ";
						text = text + ConsumerID.ToString() + "," + num3.ToString();
						text += ")";
						dbCommand.CommandText = text;
						num2 = (long)dbCommand.ExecuteNonQuery();
					}
					text = " INSERT INTO t_b_Consumer_Delete ([f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID]) SELECT [f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID] FROM [t_b_Consumer] ";
					text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
					dbCommand.CommandText = text;
					num2 = (long)dbCommand.ExecuteNonQuery();
					wgTools.WriteLine(text);
					text = " DELETE FROM [t_b_Consumer] ";
					text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
					dbCommand.CommandText = text;
					num2 = (long)dbCommand.ExecuteNonQuery();
					wgTools.WriteLine(text);
					text = "COMMIT TRANSACTION";
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					wgTools.WriteLine(text);
					num = 1;
				}
				catch (Exception ex)
				{
					icConsumer.lastErrInfo = ex.ToString();
					this.wgDebugWrite(ex.ToString());
					this.wgDebugWrite(text);
					text = "ROLLBACK TRANSACTION";
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					wgTools.WriteLine(text);
				}
				dbConnection.Close();
			}
			catch (Exception ex2)
			{
				icConsumer.lastErrInfo = ex2.ToString();
				this.wgDebugWrite(ex2.ToString());
			}
			return num;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x001114D0 File Offset: 0x001104D0
		public int dimissionUserClear(int ConsumerID)
		{
			int num = -9;
			try
			{
				string text = "";
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand(text, dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand(text, dbConnection as SqlConnection);
				}
				dbConnection.Open();
				text = "BEGIN TRANSACTION";
				dbCommand.CommandText = text;
				dbCommand.ExecuteNonQuery();
				try
				{
					wgTools.WriteLine(text);
					text = "SELECT f_ConsumerID FROM t_b_Consumer WHERE [f_ConsumerID]= " + ConsumerID.ToString();
					dbCommand.CommandText = text;
					if (long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar())) <= 0L)
					{
						wgTools.WriteLine(text);
						text = " DELETE  FROM t_b_UserFloor ";
						text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						text = " DELETE  FROM t_d_ShiftData ";
						text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						text = " DELETE  FROM t_d_Leave ";
						text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						text = " DELETE  FROM t_d_ManualCardRecord ";
						text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						text = " DELETE FROM [t_d_Privilege] ";
						text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
						dbCommand.CommandText = text;
						dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						text = " DELETE FROM [t_b_IDCard_Lost] ";
						text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						text = " DELETE FROM [t_b_Consumer_Other] ";
						text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						text = " DELETE FROM [t_b_Consumer_Face] ";
						text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
					}
					text = " DELETE FROM [t_b_Consumer_delete] ";
					text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					wgTools.WriteLine(text);
					text = "COMMIT TRANSACTION";
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					wgTools.WriteLine(text);
					num = 1;
					this.fingerprintClear(ConsumerID);
				}
				catch (Exception ex)
				{
					icConsumer.lastErrInfo = "strSql:  " + text + "\r\n" + ex.ToString();
					this.wgDebugWrite(ex.ToString());
					this.wgDebugWrite(text);
					text = "ROLLBACK TRANSACTION";
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					wgTools.WriteLine(text);
				}
				dbConnection.Close();
			}
			catch (Exception ex2)
			{
				icConsumer.lastErrInfo = ex2.ToString();
				this.wgDebugWrite(ex2.ToString());
			}
			return num;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x001117F0 File Offset: 0x001107F0
		public int dimissionUserRestore(int ConsumerID)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.dimissionUserRestore_Acc(ConsumerID);
			}
			int num = -9;
			try
			{
				string text = "BEGIN TRANSACTION";
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand(text, dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand(text, dbConnection as SqlConnection);
				}
				dbConnection.Open();
				text = "BEGIN TRANSACTION";
				dbCommand.CommandText = text;
				dbCommand.ExecuteNonQuery();
				try
				{
					wgTools.WriteLine(text);
					text = "SELECT f_ConsumerID FROM t_b_Consumer WHERE [f_ConsumerID]= " + ConsumerID.ToString();
					dbCommand.CommandText = text;
					if (long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar())) > 0L)
					{
						text = "ROLLBACK TRANSACTION";
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
					}
					else
					{
						long num2 = 0L;
						bool flag = true;
						bool flag2 = true;
						text = "SELECT f_CardNO FROM t_b_Consumer_Delete WHERE [f_ConsumerID]= " + ConsumerID.ToString();
						dbCommand.CommandText = text;
						long num3 = long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar()));
						if (num3 > 0L)
						{
							num2 = num3;
							text = "SELECT f_CardNO FROM t_b_Consumer WHERE [f_CardNO]= " + num2.ToString();
							dbCommand.CommandText = text;
							if (long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar())) > 0L)
							{
								flag = false;
							}
						}
						text = "SELECT f_ConsumerNO FROM t_b_Consumer_Delete WHERE [f_ConsumerID]= " + ConsumerID.ToString();
						dbCommand.CommandText = text;
						string text2 = wgTools.SetObjToStr(dbCommand.ExecuteScalar());
						if (!string.IsNullOrEmpty(text2))
						{
							text = "SELECT f_ConsumerID FROM t_b_Consumer WHERE [f_ConsumerNO]= " + wgTools.PrepareStrNUnicode(text2.PadLeft(10, ' '));
							dbCommand.CommandText = text;
							if (long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar())) > 0L)
							{
								flag2 = false;
								text2 = string.Concat(new string[]
								{
									text2,
									"-",
									CommonStr.strRestored,
									"-",
									ConsumerID.ToString()
								});
								if (text2.Length > 20)
								{
									text2 = CommonStr.strRestored + "-" + ConsumerID.ToString();
								}
							}
						}
						else
						{
							flag2 = false;
							text2 = CommonStr.strRestored + "-" + ConsumerID.ToString();
						}
						text2 = text2.PadLeft(10, ' ');
						text = " SET IDENTITY_INSERT [t_b_Consumer] ON ";
						dbCommand.CommandText = text;
						num3 = (long)dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						if (flag2 && flag)
						{
							text = " INSERT INTO t_b_Consumer ([f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID]) SELECT TOP 1 [f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID] FROM [t_b_Consumer_Delete] ";
						}
						else if (flag)
						{
							text = string.Format(" INSERT INTO t_b_Consumer ([f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID]) SELECT  TOP 1  [f_ConsumerID]      ,{0}     ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID] FROM [t_b_Consumer_Delete] ", flag2 ? " [f_ConsumerNO] " : (wgTools.PrepareStrNUnicode(text2) + " as  [f_ConsumerNO] "));
						}
						else
						{
							text = string.Format(" INSERT INTO t_b_Consumer ([f_ConsumerID]      ,[f_ConsumerNO]      ,[f_ConsumerName]          ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID]) SELECT  TOP 1  [f_ConsumerID]      ,{0}     ,[f_ConsumerName]           ,NULL AS [f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID] FROM [t_b_Consumer_Delete] ", flag2 ? " [f_ConsumerNO] " : (wgTools.PrepareStrNUnicode(text2) + " as  [f_ConsumerNO] "));
						}
						text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
						dbCommand.CommandText = text;
						num3 = (long)dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						if (num2 > 0L)
						{
							text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + num2.ToString();
							dbCommand.CommandText = text;
							num3 = (long)dbCommand.ExecuteNonQuery();
						}
						text = " DELETE FROM [t_b_Consumer_Delete] ";
						text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
						dbCommand.CommandText = text;
						num3 = (long)dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						text = " SET IDENTITY_INSERT [t_b_Consumer] OFF ";
						dbCommand.CommandText = text;
						num3 = (long)dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						text = "COMMIT TRANSACTION";
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
						num = 1;
					}
				}
				catch (Exception ex)
				{
					icConsumer.lastErrInfo = ex.ToString();
					this.wgDebugWrite(ex.ToString());
					this.wgDebugWrite(text);
					text = "ROLLBACK TRANSACTION";
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
					wgTools.WriteLine(text);
				}
				dbConnection.Close();
			}
			catch (Exception ex2)
			{
				icConsumer.lastErrInfo = ex2.ToString();
				this.wgDebugWrite(ex2.ToString());
			}
			return num;
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00111C24 File Offset: 0x00110C24
		public int dimissionUserRestore_Acc(int ConsumerID)
		{
			int num = -9;
			if (wgAppConfig.IsAccessDB)
			{
				try
				{
					string text = "BEGIN TRANSACTION";
					DbConnection dbConnection;
					DbCommand dbCommand;
					if (wgAppConfig.IsAccessDB)
					{
						dbConnection = new OleDbConnection(wgAppConfig.dbConString);
						dbCommand = new OleDbCommand(text, dbConnection as OleDbConnection);
					}
					else
					{
						dbConnection = new SqlConnection(wgAppConfig.dbConString);
						dbCommand = new SqlCommand(text, dbConnection as SqlConnection);
					}
					dbConnection.Open();
					text = "BEGIN TRANSACTION";
					dbCommand.ExecuteNonQuery();
					try
					{
						wgTools.WriteLine(text);
						text = "SELECT f_ConsumerID FROM t_b_Consumer WHERE [f_ConsumerID]= " + ConsumerID.ToString();
						dbCommand.CommandText = text;
						if (long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar())) > 0L)
						{
							text = "ROLLBACK TRANSACTION";
							dbCommand.CommandText = text;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text);
						}
						else
						{
							long num2 = 0L;
							bool flag = true;
							bool flag2 = true;
							text = "SELECT f_CardNO FROM t_b_Consumer_Delete WHERE [f_ConsumerID]= " + ConsumerID.ToString();
							dbCommand.CommandText = text;
							long num3 = long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar()));
							if (num3 > 0L)
							{
								num2 = num3;
								text = "SELECT f_CardNO FROM t_b_Consumer WHERE [f_CardNO]= " + num2.ToString();
								dbCommand.CommandText = text;
								if (long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar())) > 0L)
								{
									flag = false;
								}
							}
							text = "SELECT f_ConsumerNO FROM t_b_Consumer_Delete WHERE [f_ConsumerID]= " + ConsumerID.ToString();
							dbCommand.CommandText = text;
							string text2 = wgTools.SetObjToStr(dbCommand.ExecuteScalar());
							if (!string.IsNullOrEmpty(text2))
							{
								text = "SELECT f_ConsumerID FROM t_b_Consumer WHERE [f_ConsumerNO]= " + wgTools.PrepareStrNUnicode(text2.PadLeft(10, ' '));
								dbCommand.CommandText = text;
								if (long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar())) > 0L)
								{
									flag2 = false;
									text2 = string.Concat(new string[]
									{
										text2,
										"-",
										CommonStr.strRestored,
										"-",
										ConsumerID.ToString()
									});
									if (text2.Length > 20)
									{
										text2 = CommonStr.strRestored + "-" + ConsumerID.ToString();
									}
								}
							}
							else
							{
								flag2 = false;
								text2 = CommonStr.strRestored + "-" + ConsumerID.ToString();
							}
							text2 = text2.PadLeft(10, ' ');
							long num4 = (long)(ConsumerID + 1);
							text = "SELECT Max(f_ConsumerID) FROM t_b_Consumer ";
							dbCommand.CommandText = text;
							num3 = long.Parse("0" + wgTools.SetObjToStr(dbCommand.ExecuteScalar()));
							if (num3 > 0L && num3 >= num4)
							{
								num4 = num3 + 1L;
							}
							text = string.Format("ALTER TABLE t_b_Consumer ALTER COLUMN [f_ConsumerID] COUNTER ({0}, 1) ", ConsumerID.ToString());
							dbCommand.CommandText = text;
							num3 = (long)dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text);
							if (flag2 && flag)
							{
								text = " INSERT INTO t_b_Consumer ([f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID]) SELECT  TOP 1 [f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID] FROM [t_b_Consumer_Delete] ";
							}
							else if (flag)
							{
								text = string.Format(" INSERT INTO t_b_Consumer ([f_ConsumerNO]      ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID]) SELECT  TOP 1 {0}     ,[f_ConsumerName]      ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID] FROM [t_b_Consumer_Delete] ", flag2 ? " [f_ConsumerNO] " : (wgTools.PrepareStrNUnicode(text2) + " as  [f_ConsumerNO] "));
							}
							else
							{
								text = string.Format(" INSERT INTO t_b_Consumer ([f_ConsumerNO]      ,[f_ConsumerName]          ,[f_CardNO]      ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID]) SELECT TOP 1  {0}     ,[f_ConsumerName]          ,NULL as [f_CardNO]       ,[f_GroupID]      ,[f_AttendEnabled]      ,[f_ShiftEnabled]      ,[f_DoorEnabled]      ,[f_BeginYMD]      ,[f_EndYMD]      ,[f_PIN]      ,[f_PrivilegeTypeID] FROM [t_b_Consumer_Delete] ", flag2 ? " [f_ConsumerNO] " : (wgTools.PrepareStrNUnicode(text2) + " as  [f_ConsumerNO] "));
							}
							text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
							dbCommand.CommandText = text;
							num3 = (long)dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text);
							if (num2 > 0L)
							{
								text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + num2.ToString();
								dbCommand.CommandText = text;
								num3 = (long)dbCommand.ExecuteNonQuery();
							}
							text = " DELETE FROM [t_b_Consumer_Delete] ";
							text = text + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
							dbCommand.CommandText = text;
							num3 = (long)dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text);
							text = string.Format("ALTER TABLE t_b_Consumer ALTER COLUMN [f_ConsumerID] COUNTER ({0}, 1) ", num4.ToString());
							dbCommand.CommandText = text;
							num3 = (long)dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text);
							text = "COMMIT TRANSACTION";
							dbCommand.CommandText = text;
							dbCommand.ExecuteNonQuery();
							wgTools.WriteLine(text);
							num = 1;
						}
					}
					catch (Exception ex)
					{
						icConsumer.lastErrInfo = ex.ToString();
						this.wgDebugWrite(ex.ToString());
						this.wgDebugWrite(text);
						text = "ROLLBACK TRANSACTION";
						dbCommand.CommandText = text;
						dbCommand.ExecuteNonQuery();
						wgTools.WriteLine(text);
					}
					dbConnection.Close();
				}
				catch (Exception ex2)
				{
					icConsumer.lastErrInfo = ex2.ToString();
					this.wgDebugWrite(ex2.ToString());
				}
			}
			return num;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x001120A4 File Offset: 0x001110A4
		public int editUser(int ConsumerID, string ConsumerNO, string ConsumerName, int GroupID, byte AttendEnabled, byte ShiftEnabled, byte DoorEnabled, DateTime BeginYMD, DateTime EndYMD, int PIN, long CardNO)
		{
			if (!icConsumer.isValidCardId(CardNO))
			{
				return -105;
			}
			if (wgAppConfig.IsAccessDB)
			{
				return this.editUser_Acc(ConsumerID, ConsumerNO, ConsumerName, GroupID, AttendEnabled, ShiftEnabled, DoorEnabled, BeginYMD, EndYMD, PIN, CardNO);
			}
			int num = -9;
			if (ConsumerNO == null)
			{
				return -401;
			}
			if (ConsumerNO == "")
			{
				return -401;
			}
			if (ConsumerName == null)
			{
				return -201;
			}
			if (ConsumerName == "")
			{
				return -201;
			}
			string text = "";
			string text2 = ConsumerNO.PadLeft(10, ' ');
			try
			{
				byte b = ShiftEnabled;
				if (AttendEnabled == 0)
				{
					b = 0;
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						if (CardNO > 0L)
						{
							text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
							sqlCommand.CommandText = text;
							int num2 = int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar()));
							if (num2 > 0 && ConsumerID != num2)
							{
								return -103;
							}
							text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
							sqlCommand.CommandText = text;
							num2 = sqlCommand.ExecuteNonQuery();
						}
						text = "BEGIN TRANSACTION";
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
						try
						{
							text = " UPDATE t_b_Consumer  SET  ";
							text = text + " f_ConsumerNO =" + wgTools.PrepareStrNUnicode(text2);
							text = text + ",f_ConsumerName =" + wgTools.PrepareStrNUnicode(ConsumerName);
							text = text + ",f_GroupID = " + GroupID.ToString();
							text = text + ",f_AttendEnabled = " + AttendEnabled.ToString();
							text = text + ",f_ShiftEnabled = " + b.ToString();
							text = text + ",f_DoorEnabled= " + DoorEnabled.ToString();
							text = text + ",f_BeginYMD=" + wgTools.PrepareStr(BeginYMD, true, icConsumer.gTimeSecondEnabled ? this.gYMDHHMMSSFormat : this.gYMDFormat);
							text = text + ",f_EndYMD=" + wgTools.PrepareStr(EndYMD, true, this.gYMDHHMMSSFormat);
							text = text + ",f_PIN=" + PIN.ToString();
							text = text + ",f_CardNO = " + ((CardNO > 0L) ? CardNO.ToString() : "NULL");
							text = text + " WHERE f_ConsumerID =" + ConsumerID.ToString();
							sqlCommand.CommandText = text;
							int num2 = sqlCommand.ExecuteNonQuery();
							int num3 = 0;
							if (CardNO > 0L)
							{
								text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
								sqlCommand.CommandText = text;
								SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
								while (sqlDataReader.Read())
								{
									num3++;
									if (num3 > 1)
									{
										break;
									}
								}
								sqlDataReader.Close();
								if (num3 <= 1)
								{
									text = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
									sqlCommand.CommandText = text;
									sqlDataReader = sqlCommand.ExecuteReader();
									while (sqlDataReader.Read())
									{
										num3++;
										if (num3 > 1)
										{
											break;
										}
									}
									sqlDataReader.Close();
								}
							}
							if (num3 > 1)
							{
								wgAppConfig.wgLogWithoutDB("CardNO has more users... \r\n" + text, EventLogEntryType.Information, null);
								text = "ROLLBACK TRANSACTION";
								sqlCommand.CommandText = text;
								sqlCommand.ExecuteNonQuery();
								return num;
							}
							text = "COMMIT TRANSACTION";
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							return 1;
						}
						catch (Exception ex)
						{
							icConsumer.lastErrInfo = "strSql:  " + text + "\r\n" + ex.ToString();
							wgAppConfig.wgLogWithoutDB(ex.ToString(), EventLogEntryType.Information, null);
							text = "ROLLBACK TRANSACTION";
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
						}
						return num;
					}
				}
			}
			catch (Exception ex2)
			{
				icConsumer.lastErrInfo = "strSql:  " + text + "\r\n" + ex2.ToString();
				this.wgDebugWrite(ex2.ToString());
				wgAppConfig.wgLogWithoutDB(ex2.ToString(), EventLogEntryType.Information, null);
			}
			return num;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x001124DC File Offset: 0x001114DC
		public int editUser_Acc(int ConsumerID, string ConsumerNO, string ConsumerName, int GroupID, byte AttendEnabled, byte ShiftEnabled, byte DoorEnabled, DateTime BeginYMD, DateTime EndYMD, int PIN, long CardNO)
		{
			if (!icConsumer.isValidCardId(CardNO))
			{
				return -105;
			}
			int num = -9;
			if (ConsumerNO == null)
			{
				return -401;
			}
			if (ConsumerNO == "")
			{
				return -401;
			}
			if (ConsumerName == null)
			{
				return -201;
			}
			if (ConsumerName == "")
			{
				return -201;
			}
			string text = "";
			string text2 = ConsumerNO.PadLeft(10, ' ');
			try
			{
				byte b = ShiftEnabled;
				if (AttendEnabled == 0)
				{
					b = 0;
				}
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						if (CardNO > 0L)
						{
							text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
							oleDbCommand.CommandText = text;
							int num2 = int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()));
							if (num2 > 0 && ConsumerID != num2)
							{
								return -103;
							}
							text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
							oleDbCommand.CommandText = text;
							num2 = oleDbCommand.ExecuteNonQuery();
						}
						text = "BEGIN TRANSACTION";
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
						try
						{
							text = " UPDATE t_b_Consumer  SET  ";
							text = text + " f_ConsumerNO =" + wgTools.PrepareStrNUnicode(text2);
							text = text + ",f_ConsumerName =" + wgTools.PrepareStrNUnicode(ConsumerName);
							text = text + ",f_GroupID = " + GroupID.ToString();
							text = text + ",f_AttendEnabled = " + AttendEnabled.ToString();
							text = text + ",f_ShiftEnabled = " + b.ToString();
							text = text + ",f_DoorEnabled= " + DoorEnabled.ToString();
							text = text + ",f_BeginYMD=" + wgTools.PrepareStr(BeginYMD, true, icConsumer.gTimeSecondEnabled ? this.gYMDHHMMSSFormat : this.gYMDFormat);
							text = text + ",f_EndYMD=" + wgTools.PrepareStr(EndYMD, true, this.gYMDHHMMSSFormat);
							text = text + ",f_PIN=" + PIN.ToString();
							text = text + ",f_CardNO = " + ((CardNO > 0L) ? CardNO.ToString() : "NULL");
							text = text + " WHERE f_ConsumerID =" + ConsumerID.ToString();
							oleDbCommand.CommandText = text;
							int num2 = oleDbCommand.ExecuteNonQuery();
							int num3 = 0;
							if (CardNO > 0L)
							{
								text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
								oleDbCommand.CommandText = text;
								OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
								while (oleDbDataReader.Read())
								{
									num3++;
									if (num3 > 1)
									{
										break;
									}
								}
								oleDbDataReader.Close();
								if (num3 <= 1)
								{
									text = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
									oleDbCommand.CommandText = text;
									oleDbDataReader = oleDbCommand.ExecuteReader();
									while (oleDbDataReader.Read())
									{
										num3++;
										if (num3 > 1)
										{
											break;
										}
									}
									oleDbDataReader.Close();
								}
							}
							if (num3 > 1)
							{
								text = "ROLLBACK TRANSACTION";
								oleDbCommand.CommandText = text;
								oleDbCommand.ExecuteNonQuery();
								return num;
							}
							text = "COMMIT TRANSACTION";
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							return 1;
						}
						catch (Exception ex)
						{
							icConsumer.lastErrInfo = "strSql:  " + text + "\r\n" + ex.ToString();
							text = "ROLLBACK TRANSACTION";
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
						}
						return num;
					}
				}
			}
			catch (Exception ex2)
			{
				icConsumer.lastErrInfo = "strSql:  " + text + "\r\n" + ex2.ToString();
				this.wgDebugWrite(ex2.ToString());
			}
			return num;
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x001128C4 File Offset: 0x001118C4
		public int editUserOtherInfo(int ConsumerID, string txtf_Title, string txtf_Culture, string txtf_Hometown, string txtf_Birthday, string txtf_Marriage, string txtf_JoinDate, string txtf_LeaveDate, string txtf_CertificateType, string txtf_CertificateID, string txtf_SocialInsuranceNo, string txtf_Addr, string txtf_Postcode, string txtf_Sex, string txtf_Nationality, string txtf_Religion, string txtf_EnglishName, string txtf_Mobile, string txtf_HomePhone, string txtf_Telephone, string txtf_Email, string txtf_Political, string txtf_CorporationName, string txtf_TechGrade, string txtf_Note)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.editUserOtherInfo_Acc(ConsumerID, txtf_Title, txtf_Culture, txtf_Hometown, txtf_Birthday, txtf_Marriage, txtf_JoinDate, txtf_LeaveDate, txtf_CertificateType, txtf_CertificateID, txtf_SocialInsuranceNo, txtf_Addr, txtf_Postcode, txtf_Sex, txtf_Nationality, txtf_Religion, txtf_EnglishName, txtf_Mobile, txtf_HomePhone, txtf_Telephone, txtf_Email, txtf_Political, txtf_CorporationName, txtf_TechGrade, txtf_Note);
			}
			int num = -9;
			try
			{
				string text = " UPDATE t_b_Consumer_Other  SET  ";
				text = string.Concat(new string[]
				{
					text,
					"   f_Title                 = ",
					wgTools.PrepareStrNUnicode(txtf_Title),
					"  , f_Culture               = ",
					wgTools.PrepareStrNUnicode(txtf_Culture),
					"  , f_Hometown              = ",
					wgTools.PrepareStrNUnicode(txtf_Hometown),
					"  , f_Birthday              = ",
					wgTools.PrepareStrNUnicode(txtf_Birthday),
					"  , f_Marriage              = ",
					wgTools.PrepareStrNUnicode(txtf_Marriage),
					"  , f_JoinDate              = ",
					wgTools.PrepareStrNUnicode(txtf_JoinDate),
					"  , f_LeaveDate             = ",
					wgTools.PrepareStrNUnicode(txtf_LeaveDate),
					"  , f_CertificateType       = ",
					wgTools.PrepareStrNUnicode(txtf_CertificateType),
					"  , f_CertificateID         = ",
					wgTools.PrepareStrNUnicode(txtf_CertificateID),
					"  , f_SocialInsuranceNo     = ",
					wgTools.PrepareStrNUnicode(txtf_SocialInsuranceNo),
					"  , f_Addr                  = ",
					wgTools.PrepareStrNUnicode(txtf_Addr),
					"  , f_Postcode              = ",
					wgTools.PrepareStrNUnicode(txtf_Postcode),
					"  , f_Sex                   = ",
					wgTools.PrepareStrNUnicode(txtf_Sex),
					"  , f_Nationality           = ",
					wgTools.PrepareStrNUnicode(txtf_Nationality),
					"  , f_Religion              = ",
					wgTools.PrepareStrNUnicode(txtf_Religion),
					"  , f_EnglishName           = ",
					wgTools.PrepareStrNUnicode(txtf_EnglishName),
					"  , f_Mobile                = ",
					wgTools.PrepareStrNUnicode(txtf_Mobile),
					"  , f_HomePhone             = ",
					wgTools.PrepareStrNUnicode(txtf_HomePhone),
					"  , f_Telephone             = ",
					wgTools.PrepareStrNUnicode(txtf_Telephone),
					"  , f_Email                 = ",
					wgTools.PrepareStrNUnicode(txtf_Email),
					"  , f_Political             = ",
					wgTools.PrepareStrNUnicode(txtf_Political),
					"  , f_CorporationName       = ",
					wgTools.PrepareStrNUnicode(txtf_CorporationName),
					"  , f_TechGrade             = ",
					wgTools.PrepareStrNUnicode(txtf_TechGrade),
					"  , f_Note                  = ",
					wgTools.PrepareStrNUnicode(txtf_Note),
					" WHERE f_ConsumerID =",
					ConsumerID.ToString()
				});
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.ExecuteNonQuery();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				icConsumer.lastErrInfo = ex.ToString();
				this.wgDebugWrite(ex.ToString());
			}
			return num;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00112BF0 File Offset: 0x00111BF0
		public int editUserOtherInfo_Acc(int ConsumerID, string txtf_Title, string txtf_Culture, string txtf_Hometown, string txtf_Birthday, string txtf_Marriage, string txtf_JoinDate, string txtf_LeaveDate, string txtf_CertificateType, string txtf_CertificateID, string txtf_SocialInsuranceNo, string txtf_Addr, string txtf_Postcode, string txtf_Sex, string txtf_Nationality, string txtf_Religion, string txtf_EnglishName, string txtf_Mobile, string txtf_HomePhone, string txtf_Telephone, string txtf_Email, string txtf_Political, string txtf_CorporationName, string txtf_TechGrade, string txtf_Note)
		{
			int num = -9;
			try
			{
				string text = " UPDATE t_b_Consumer_Other  SET  ";
				text = string.Concat(new string[]
				{
					text,
					"   f_Title                 = ",
					wgTools.PrepareStrNUnicode(txtf_Title),
					"  , f_Culture               = ",
					wgTools.PrepareStrNUnicode(txtf_Culture),
					"  , f_Hometown              = ",
					wgTools.PrepareStrNUnicode(txtf_Hometown),
					"  , f_Birthday              = ",
					wgTools.PrepareStrNUnicode(txtf_Birthday),
					"  , f_Marriage              = ",
					wgTools.PrepareStrNUnicode(txtf_Marriage),
					"  , f_JoinDate              = ",
					wgTools.PrepareStrNUnicode(txtf_JoinDate),
					"  , f_LeaveDate             = ",
					wgTools.PrepareStrNUnicode(txtf_LeaveDate),
					"  , f_CertificateType       = ",
					wgTools.PrepareStrNUnicode(txtf_CertificateType),
					"  , f_CertificateID         = ",
					wgTools.PrepareStrNUnicode(txtf_CertificateID),
					"  , f_SocialInsuranceNo     = ",
					wgTools.PrepareStrNUnicode(txtf_SocialInsuranceNo),
					"  , f_Addr                  = ",
					wgTools.PrepareStrNUnicode(txtf_Addr),
					"  , f_Postcode              = ",
					wgTools.PrepareStrNUnicode(txtf_Postcode),
					"  , f_Sex                   = ",
					wgTools.PrepareStrNUnicode(txtf_Sex),
					"  , f_Nationality           = ",
					wgTools.PrepareStrNUnicode(txtf_Nationality),
					"  , f_Religion              = ",
					wgTools.PrepareStrNUnicode(txtf_Religion),
					"  , f_EnglishName           = ",
					wgTools.PrepareStrNUnicode(txtf_EnglishName),
					"  , f_Mobile                = ",
					wgTools.PrepareStrNUnicode(txtf_Mobile),
					"  , f_HomePhone             = ",
					wgTools.PrepareStrNUnicode(txtf_HomePhone),
					"  , f_Telephone             = ",
					wgTools.PrepareStrNUnicode(txtf_Telephone),
					"  , f_Email                 = ",
					wgTools.PrepareStrNUnicode(txtf_Email),
					"  , f_Political             = ",
					wgTools.PrepareStrNUnicode(txtf_Political),
					"  , f_CorporationName       = ",
					wgTools.PrepareStrNUnicode(txtf_CorporationName),
					"  , f_TechGrade             = ",
					wgTools.PrepareStrNUnicode(txtf_TechGrade),
					"  , f_Note                  = ",
					wgTools.PrepareStrNUnicode(txtf_Note),
					" WHERE f_ConsumerID =",
					ConsumerID.ToString()
				});
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.ExecuteNonQuery();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				icConsumer.lastErrInfo = ex.ToString();
				this.wgDebugWrite(ex.ToString());
			}
			return num;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00112EE0 File Offset: 0x00111EE0
		public int fingerprintClear(int ConsumerID)
		{
			try
			{
				string text = "SELECT f_FingerNO From t_b_Consumer_Fingerprint ORDER BY f_FingerNO ASC";
				using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
				{
					using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
					{
						dbConnection.Open();
						text = string.Format("SELECT *  From t_b_Consumer_Fingerprint WHERE t_b_Consumer_Fingerprint.f_ConsumerID ={0}", ConsumerID);
						dbCommand.CommandText = text;
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						while (dbDataReader.Read())
						{
							string text2 = dbDataReader["f_ConsumerID"].ToString() + " " + wgAppConfig.getValStringBySql(string.Format("SELECT f_ConsumerName From t_b_Consumer WHERE  f_ConsumerID ={0}", dbDataReader["f_ConsumerID"].ToString()));
							wgAppConfig.wgLogWithoutDB(string.Format("fingerprint DELETE: ConsumerID={0},f_FingerNO={1}, info={2} ", text2, dbDataReader["f_FingerNO"], dbDataReader["f_FingerInfo"]), EventLogEntryType.Information, null);
						}
						dbDataReader.Close();
						text = string.Format("DELETE  From t_b_Consumer_Fingerprint WHERE t_b_Consumer_Fingerprint.f_ConsumerID ={0}", ConsumerID);
						dbCommand.CommandText = text;
						int num = dbCommand.ExecuteNonQuery();
						if (num > 0)
						{
							return num;
						}
						return -1;
					}
				}
			}
			catch (Exception ex)
			{
				icConsumer.lastErrInfo = ex.ToString();
				this.wgDebugWrite(ex.ToString());
			}
			return -9;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00113080 File Offset: 0x00112080
		public int fingerprintNew(int ConsumerID, int fingerLoc, string fingerInfo)
		{
			string text = "";
			string text2 = "";
			try
			{
				text = "SELECT f_FingerNO From t_b_Consumer_Fingerprint ORDER BY f_FingerNO ASC";
				using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
				{
					using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
					{
						dbConnection.Open();
						text = "SELECT f_FingerNO From t_b_Consumer_Fingerprint ORDER BY f_FingerNO ASC";
						dbCommand.CommandText = text;
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						int num = 1;
						while (dbDataReader.Read())
						{
							if ((int)dbDataReader[0] > 0)
							{
								if (num != (int)dbDataReader[0])
								{
									break;
								}
								num++;
							}
						}
						dbDataReader.Close();
						if (fingerLoc >= 0 && fingerLoc < 1024)
						{
							text = string.Format("SELECT *  From t_b_Consumer_Fingerprint WHERE  f_FingerNO ={0}", (fingerLoc + 1).ToString());
							dbCommand.CommandText = text;
							dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								string text3 = dbDataReader["f_ConsumerID"].ToString() + " " + wgAppConfig.getValStringBySql(string.Format("SELECT f_ConsumerName From t_b_Consumer WHERE  f_ConsumerID ={0}", dbDataReader["f_ConsumerID"].ToString()));
								text2 = wgTools.SetObjToStr(dbDataReader["f_Description"]);
								wgAppConfig.wgLogWithoutDB(string.Format("fingerprint DELETE: ConsumerID={0},f_FingerNO={1}, info={2} ", text3, dbDataReader["f_FingerNO"], dbDataReader["f_FingerInfo"]), EventLogEntryType.Information, null);
							}
							dbDataReader.Close();
							text = string.Format("DELETE  From t_b_Consumer_Fingerprint WHERE f_FingerNO ={0}", (fingerLoc + 1).ToString());
							dbCommand.CommandText = text;
							dbCommand.ExecuteNonQuery();
							num = fingerLoc + 1;
						}
						if (num > 1024)
						{
							return -1000;
						}
						text = " INSERT INTO t_b_Consumer_Fingerprint (f_FingerNO, f_FingerInfo, f_ConsumerID,f_Description,f_RegisterTime, f_Note ) values (";
						text += num.ToString();
						text += ",";
						text += wgTools.PrepareStrNUnicode(fingerInfo);
						text += ",";
						text += ConsumerID.ToString();
						text += ",";
						text += wgTools.PrepareStrNUnicode(text2);
						text += ",";
						text += wgTools.PrepareStr(DateTime.Now, true, "yyyy-MM-dd HH:mm:ss");
						text += ",";
						text += wgTools.PrepareStrNUnicode("");
						text += ")";
						dbCommand.CommandText = text;
						if (dbCommand.ExecuteNonQuery() > 0)
						{
							return num;
						}
						return -1;
					}
				}
			}
			catch (Exception ex)
			{
				icConsumer.lastErrInfo = ex.ToString();
				this.wgDebugWrite(ex.ToString());
				wgAppConfig.wgLog(ex.ToString() + "\r\n strsql = " + text);
			}
			return -9;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x001133A4 File Offset: 0x001123A4
		public static string getErrInfo(int ErrNO)
		{
			string text = "";
			int num = ErrNO;
			if (num <= -401)
			{
				if (num == -901)
				{
					return CommonStr.strDBConNotCreate;
				}
				if (num == -500)
				{
					return CommonStr.strUsers_500;
				}
				if (num == -401)
				{
					return CommonStr.strConsumerNOWrong;
				}
			}
			else if (num <= -103)
			{
				if (num == -201)
				{
					return CommonStr.strConsumerNameWrong;
				}
				switch (num)
				{
				case -105:
					return CommonStr.strCardInvalid;
				case -104:
					return CommonStr.strCardNotExisted;
				case -103:
					return CommonStr.strCardAlreadyUsed;
				}
			}
			else
			{
				if (num == -9)
				{
					if (icConsumer.lastErrInfo.IndexOf("IX_t_b_Consumer_ConsumerNO") <= 0 && icConsumer.lastErrInfo.IndexOf("重复") <= 0 && icConsumer.lastErrInfo.IndexOf("重複") <= 0 && icConsumer.lastErrInfo.IndexOf("duplicate") <= 0)
					{
						text = CommonStr.strOperateFailed + "  (E=" + ErrNO.ToString() + ")";
					}
					else
					{
						text = string.Concat(new string[]
						{
							CommonStr.strOperateFailed,
							"  (E=",
							ErrNO.ToString(),
							")\r\n\r\n",
							CommonStr.strConsumerNOWrong
						});
					}
					if (!string.IsNullOrEmpty(wgAppConfig.getTriggerName()))
					{
						text = string.Concat(new string[]
						{
							text,
							"\r\n\r\n",
							CommonStr.strCheckDatabase2015,
							" ",
							wgAppConfig.getTriggerName()
						});
					}
					wgAppConfig.wgLog(text + "\r\n\r\nlastErrInfo =" + icConsumer.lastErrInfo);
					return text;
				}
				if (num == 0)
				{
					return text;
				}
			}
			if (ErrNO < 0)
			{
				text = string.Concat(new string[]
				{
					CommonStr.strOperateFailed,
					"  (E=",
					ErrNO.ToString(),
					")\r\n\r\n",
					icConsumer.lastErrInfo
				});
			}
			return text;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00113584 File Offset: 0x00112584
		public bool isExisted(long CardNO)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.isExisted_Acc(CardNO);
			}
			bool flag = false;
			try
			{
				string text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						if (int.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar())) > 0)
						{
							return true;
						}
					}
					return flag;
				}
			}
			catch (Exception ex)
			{
				icConsumer.lastErrInfo = ex.ToString();
				this.wgDebugWrite(ex.ToString());
			}
			return flag;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00113654 File Offset: 0x00112654
		public bool isExisted_Acc(long CardNO)
		{
			bool flag = false;
			try
			{
				string text = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						if (int.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar())) > 0)
						{
							return true;
						}
					}
					return flag;
				}
			}
			catch (Exception ex)
			{
				icConsumer.lastErrInfo = ex.ToString();
				this.wgDebugWrite(ex.ToString());
			}
			return flag;
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00113714 File Offset: 0x00112714
		public static bool isValidCardId(long CardNO)
		{
			if (CardNO >= 0L && CardNO != 16777215L && CardNO != (long)((ulong)(-1)) && 25565535L != CardNO && (wgAppConfig.IsActivateCard19 || CardNO < (long)((ulong)(-1))))
			{
				return true;
			}
			wgAppConfig.wgLog("Invalid CardNO = " + CardNO.ToString());
			return false;
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00113764 File Offset: 0x00112764
		public int registerLostCard(int ConsumerID, long NewCardNO)
		{
			if (!icConsumer.isValidCardId(NewCardNO))
			{
				return -105;
			}
			if (wgAppConfig.IsAccessDB)
			{
				return this.registerLostCard_Acc(ConsumerID, NewCardNO);
			}
			int num = -9;
			try
			{
				string text = "SELECT f_CardNO FROM t_b_Consumer WHERE [f_ConsumerID]= " + ConsumerID.ToString();
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.CommandText = text;
						long num2 = long.Parse("0" + wgTools.SetObjToStr(sqlCommand.ExecuteScalar()));
						long num3 = 0L;
						if (num2 <= 0L)
						{
							num = -104;
						}
						else
						{
							num3 = num2;
							text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + num2.ToString();
							sqlCommand.CommandText = text;
							sqlCommand.ExecuteNonQuery();
							text = "INSERT INTO t_b_IDCard_Lost ([f_ConsumerID], [f_CardNO]) VALUES( ";
							text = string.Concat(new string[]
							{
								text,
								ConsumerID.ToString(),
								",",
								num2.ToString(),
								")"
							});
							sqlCommand.CommandText = text;
							num2 = (long)sqlCommand.ExecuteNonQuery();
						}
						if (NewCardNO <= 0L)
						{
							text = "Update t_b_Consumer SET f_CardNO = NULL WHERE [f_ConsumerID]= " + ConsumerID.ToString();
						}
						else
						{
							text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + NewCardNO.ToString();
							sqlCommand.CommandText = text;
							num2 = (long)sqlCommand.ExecuteNonQuery();
							text = "Update t_b_Consumer SET f_CardNO = " + NewCardNO.ToString() + " WHERE [f_ConsumerID]= " + ConsumerID.ToString();
						}
						sqlCommand.CommandText = text;
						num2 = (long)sqlCommand.ExecuteNonQuery();
						if (num2 >= 0L)
						{
							this.updatePhotoName(num3, NewCardNO);
							num = (int)num2;
						}
					}
					return num;
				}
			}
			catch (Exception ex)
			{
				icConsumer.lastErrInfo = ex.ToString();
				this.wgDebugWrite(ex.ToString());
			}
			return num;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x00113968 File Offset: 0x00112968
		public int registerLostCard_Acc(int ConsumerID, long NewCardNO)
		{
			if (!icConsumer.isValidCardId(NewCardNO))
			{
				return -105;
			}
			int num = -9;
			try
			{
				string text = "SELECT f_CardNO FROM t_b_Consumer WHERE [f_ConsumerID]= " + ConsumerID.ToString();
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.CommandText = text;
						long num2 = long.Parse("0" + wgTools.SetObjToStr(oleDbCommand.ExecuteScalar()));
						long num3 = 0L;
						if (num2 <= 0L)
						{
							num = -104;
						}
						else
						{
							num3 = num2;
							text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + num2.ToString();
							oleDbCommand.CommandText = text;
							oleDbCommand.ExecuteNonQuery();
							text = "INSERT INTO t_b_IDCard_Lost ([f_ConsumerID], [f_CardNO]) VALUES( ";
							text = string.Concat(new string[]
							{
								text,
								ConsumerID.ToString(),
								",",
								num2.ToString(),
								")"
							});
							oleDbCommand.CommandText = text;
							num2 = (long)oleDbCommand.ExecuteNonQuery();
						}
						if (NewCardNO <= 0L)
						{
							text = "Update t_b_Consumer SET f_CardNO = NULL WHERE [f_ConsumerID]= " + ConsumerID.ToString();
						}
						else
						{
							text = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + NewCardNO.ToString();
							oleDbCommand.CommandText = text;
							num2 = (long)oleDbCommand.ExecuteNonQuery();
							text = "Update t_b_Consumer SET f_CardNO = " + NewCardNO.ToString() + " WHERE [f_ConsumerID]= " + ConsumerID.ToString();
						}
						oleDbCommand.CommandText = text;
						num2 = (long)oleDbCommand.ExecuteNonQuery();
						if (num2 >= 0L)
						{
							this.updatePhotoName(num3, NewCardNO);
							num = (int)num2;
						}
					}
					return num;
				}
			}
			catch (Exception ex)
			{
				icConsumer.lastErrInfo = ex.ToString();
				this.wgDebugWrite(ex.ToString());
			}
			return num;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00113B5C File Offset: 0x00112B5C
		private void updatePhotoName(long oldCardNO, long NewCardNO)
		{
			try
			{
				if (!wgAppConfig.IsPhotoNameFromConsumerNO && NewCardNO > 0L && oldCardNO > 0L)
				{
					string photoFileName = wgAppConfig.getPhotoFileName(oldCardNO);
					if (!string.IsNullOrEmpty(photoFileName))
					{
						FileInfo fileInfo = new FileInfo(photoFileName);
						if (fileInfo.Exists)
						{
							string text = wgAppConfig.Path4Photo() + NewCardNO.ToString() + ".jpg";
							fileInfo.CopyTo(text, true);
							fileInfo.Delete();
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00113BEC File Offset: 0x00112BEC
		public void wgDebugWrite(string info)
		{
		}

		// Token: 0x04001C54 RID: 7252
		public const int ConsumerNOMinLen = 10;

		// Token: 0x04001C55 RID: 7253
		public int gConsumerID;

		// Token: 0x04001C56 RID: 7254
		public static bool gTimeSecondEnabled = false;

		// Token: 0x04001C57 RID: 7255
		public string gYMDFormat = "yyyy-MM-dd";

		// Token: 0x04001C58 RID: 7256
		public string gYMDHHMMSSFormat = "yyyy-MM-dd HH:mm:ss";

		// Token: 0x04001C59 RID: 7257
		private static string lastErrInfo = "";
	}
}
