using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using WG3000_COMM.Core;

namespace WG3000_COMM.DataOper
{
	// Token: 0x02000222 RID: 546
	internal class icControllerZone
	{
		// Token: 0x06000FC4 RID: 4036 RVA: 0x0011B460 File Offset: 0x0011A460
		public int addNew(string ZoneName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.addNew_Acc(ZoneName);
			}
			int num = -9;
			if (ZoneName == null)
			{
				return -201;
			}
			if (ZoneName == "")
			{
				return -201;
			}
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						string text = " INSERT INTO t_b_Controller_Zone (f_ZoneName) values (";
						text = text + wgTools.PrepareStrNUnicode(ZoneName) + ")";
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
						text = "SELECT f_ZoneID from [t_b_Controller_Zone]  ORDER BY f_ZoneName  + '\\' ASC";
						sqlCommand.CommandText = text;
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						int num2 = 1;
						while (sqlDataReader.Read())
						{
							wgAppConfig.runUpdateSql("UPDATE t_b_Controller_Zone SET f_ZoneNO= " + num2.ToString() + " WHERE  f_ZoneID= " + sqlDataReader[0].ToString());
							num2++;
						}
						sqlDataReader.Close();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0011B59C File Offset: 0x0011A59C
		public int addNew_Acc(string ZoneName)
		{
			int num = -9;
			if (ZoneName == null)
			{
				return -201;
			}
			if (ZoneName == "")
			{
				return -201;
			}
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						string text = " INSERT INTO t_b_Controller_Zone (f_ZoneName) values (";
						text = text + wgTools.PrepareStrNUnicode(ZoneName) + ")";
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
						text = "SELECT f_ZoneID from [t_b_Controller_Zone]  ORDER BY f_ZoneName  + '\\' ASC";
						oleDbCommand.CommandText = text;
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						int num2 = 1;
						while (oleDbDataReader.Read())
						{
							wgAppConfig.runUpdateSql("UPDATE t_b_Controller_Zone SET f_ZoneNO= " + num2.ToString() + " WHERE  f_ZoneID= " + oleDbDataReader[0].ToString());
							num2++;
						}
						oleDbDataReader.Close();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0011B6C8 File Offset: 0x0011A6C8
		public int addNew4BatchExcel(string ZoneName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.addNew4BatchExcel_Acc(ZoneName);
			}
			int num = -9;
			if (ZoneName == null)
			{
				return -201;
			}
			if (ZoneName == "")
			{
				return -201;
			}
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						string text = " INSERT INTO t_b_Controller_Zone (f_ZoneName) values (";
						text = text + wgTools.PrepareStrNUnicode(ZoneName) + ")";
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0011B7AC File Offset: 0x0011A7AC
		public int addNew4BatchExcel_Acc(string ZoneName)
		{
			int num = -9;
			if (ZoneName == null)
			{
				return -201;
			}
			if (ZoneName == "")
			{
				return -201;
			}
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						string text = " INSERT INTO t_b_Controller_Zone (f_ZoneName) values (";
						text = text + wgTools.PrepareStrNUnicode(ZoneName) + ")";
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0011B880 File Offset: 0x0011A880
		public bool checkExisted(string ZoneNewName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.checkExisted_Acc(ZoneNewName);
			}
			bool flag = false;
			if (ZoneNewName != null)
			{
				if (ZoneNewName == "")
				{
					return flag;
				}
				try
				{
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
						{
							if (sqlConnection.State != ConnectionState.Open)
							{
								sqlConnection.Open();
							}
							string text = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
							text = text + " WHERE (f_ZoneName = " + wgTools.PrepareStrNUnicode(ZoneNewName) + " ) ";
							sqlCommand.CommandText = text;
							SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
							if (sqlDataReader.Read())
							{
								flag = true;
							}
							sqlDataReader.Close();
						}
						return flag;
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				return flag;
			}
			return flag;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0011B974 File Offset: 0x0011A974
		public bool checkExisted_Acc(string ZoneNewName)
		{
			bool flag = false;
			if (ZoneNewName != null)
			{
				if (ZoneNewName == "")
				{
					return flag;
				}
				try
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
						{
							if (oleDbConnection.State != ConnectionState.Open)
							{
								oleDbConnection.Open();
							}
							string text = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
							text = text + " WHERE (f_ZoneName = " + wgTools.PrepareStrNUnicode(ZoneNewName) + " ) ";
							oleDbCommand.CommandText = text;
							OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
							if (oleDbDataReader.Read())
							{
								flag = true;
							}
							oleDbDataReader.Close();
						}
						return flag;
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				return flag;
			}
			return flag;
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0011BA5C File Offset: 0x0011AA5C
		public int delete(string ZoneName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.delete_Acc(ZoneName);
			}
			int num = -9;
			if (ZoneName == null)
			{
				return -201;
			}
			if (ZoneName == "")
			{
				return -201;
			}
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						string text = " DELETE FROM t_b_Controller_Zone WHERE (f_ZoneName = ";
						text = string.Concat(new string[]
						{
							text,
							wgTools.PrepareStrNUnicode(ZoneName),
							" )  or (f_ZoneName like ",
							wgTools.PrepareStrNUnicode(ZoneName + "\\%"),
							" ) "
						});
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0011BB74 File Offset: 0x0011AB74
		public int delete_Acc(string ZoneName)
		{
			int num = -9;
			if (ZoneName == null)
			{
				return -201;
			}
			if (ZoneName == "")
			{
				return -201;
			}
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						string text = " DELETE FROM t_b_Controller_Zone WHERE (f_ZoneName = ";
						text = string.Concat(new string[]
						{
							text,
							wgTools.PrepareStrNUnicode(ZoneName),
							" )  or (f_ZoneName like ",
							wgTools.PrepareStrNUnicode(ZoneName + "\\%"),
							" ) "
						});
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0011BC7C File Offset: 0x0011AC7C
		public int getAllowedControllers(ref DataTable dtController)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getAllowedControllers_Acc(ref dtController);
			}
			int num = -9;
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						sqlCommand.CommandText = "SELECT f_ZoneName from t_b_Controller_Zone,t_b_Controller_Zone4Operator WHERE t_b_Controller_Zone4Operator.f_ZoneID = t_b_Controller_Zone.f_ZoneID  AND t_b_Controller_Zone4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_ZoneName  + '\\' ASC";
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						ArrayList arrayList = new ArrayList();
						while (sqlDataReader.Read())
						{
							arrayList.Add(sqlDataReader[0]);
						}
						sqlDataReader.Close();
						if (arrayList.Count == 0)
						{
							return 1;
						}
						string text = "SELECT f_ZoneID,f_ZoneName, f_ZoneNO from [t_b_Controller_Zone]  ";
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(text + "  ORDER BY f_ZoneName  + '\\' ASC", sqlConnection))
						{
							using (DataTable dataTable = new DataTable("Zones"))
							{
								sqlDataAdapter.Fill(dataTable);
								using (DataView dataView = new DataView(dataTable))
								{
									int i = 0;
									int num2 = 0;
									while (i < dtController.Rows.Count)
									{
										DataRow dataRow = dtController.Rows[i];
										bool flag = false;
										if (int.TryParse(dataRow["f_ZoneID"].ToString(), out num2))
										{
											dataView.RowFilter = "f_ZoneID = " + dataRow["f_ZoneID"].ToString();
											if (dataView.Count > 0)
											{
												string text2 = (string)dataView[0]["f_ZoneName"];
												for (int j = 0; j < arrayList.Count; j++)
												{
													if (text2 == arrayList[j].ToString() || text2.IndexOf(arrayList[j].ToString() + "\\") == 0)
													{
														flag = true;
														break;
													}
												}
											}
										}
										if (!flag)
										{
											dtController.Rows.Remove(dataRow);
											dtController.AcceptChanges();
										}
										else
										{
											i++;
										}
									}
								}
							}
						}
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0011BF64 File Offset: 0x0011AF64
		public int getAllowedControllers_Acc(ref DataTable dtController)
		{
			int num = -9;
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						oleDbCommand.CommandText = "SELECT f_ZoneName from t_b_Controller_Zone,t_b_Controller_Zone4Operator WHERE t_b_Controller_Zone4Operator.f_ZoneID = t_b_Controller_Zone.f_ZoneID  AND t_b_Controller_Zone4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_ZoneName  + '\\' ASC";
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						ArrayList arrayList = new ArrayList();
						while (oleDbDataReader.Read())
						{
							arrayList.Add(oleDbDataReader[0]);
						}
						oleDbDataReader.Close();
						if (arrayList.Count == 0)
						{
							return 1;
						}
						string text = "SELECT f_ZoneID,f_ZoneName, f_ZoneNO from [t_b_Controller_Zone]  ";
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(text + "  ORDER BY f_ZoneName  + '\\' ASC", oleDbConnection))
						{
							using (DataTable dataTable = new DataTable("Zones"))
							{
								oleDbDataAdapter.Fill(dataTable);
								using (DataView dataView = new DataView(dataTable))
								{
									int i = 0;
									int num2 = 0;
									while (i < dtController.Rows.Count)
									{
										DataRow dataRow = dtController.Rows[i];
										bool flag = false;
										if (int.TryParse(dataRow["f_ZoneID"].ToString(), out num2))
										{
											dataView.RowFilter = "f_ZoneID = " + dataRow["f_ZoneID"].ToString();
											if (dataView.Count > 0)
											{
												string text2 = (string)dataView[0]["f_ZoneName"];
												for (int j = 0; j < arrayList.Count; j++)
												{
													if (text2 == arrayList[j].ToString() || text2.IndexOf(arrayList[j].ToString() + "\\") == 0)
													{
														flag = true;
														break;
													}
												}
											}
										}
										if (!flag)
										{
											dtController.Rows.Remove(dataRow);
											dtController.AcceptChanges();
										}
										else
										{
											i++;
										}
									}
								}
							}
						}
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0011C23C File Offset: 0x0011B23C
		public int getZone(ref ArrayList arrZoneName, ref ArrayList arrZoneID, ref ArrayList arrZoneNO)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getZone_Acc(ref arrZoneName, ref arrZoneID, ref arrZoneNO);
			}
			int num = -9;
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						arrZoneName.Clear();
						arrZoneID.Clear();
						arrZoneNO.Clear();
						sqlCommand.CommandText = "SELECT f_ZoneName from t_b_Controller_Zone,t_b_Controller_Zone4Operator WHERE t_b_Controller_Zone4Operator.f_ZoneID = t_b_Controller_Zone.f_ZoneID  AND t_b_Controller_Zone4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_ZoneName  + '\\' ASC";
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						ArrayList arrayList = new ArrayList();
						while (sqlDataReader.Read())
						{
							arrayList.Add(sqlDataReader[0]);
						}
						sqlDataReader.Close();
						if (arrayList.Count == 0)
						{
							arrZoneName.Add("");
							arrZoneID.Add(0);
							arrZoneNO.Add(0);
						}
						string text = "SELECT f_ZoneID,f_ZoneName, f_ZoneNO from [t_b_Controller_Zone]  ";
						text += "  ORDER BY f_ZoneName  + '\\' ASC";
						sqlCommand.CommandText = text;
						sqlDataReader = sqlCommand.ExecuteReader();
						bool flag = true;
						while (sqlDataReader.Read())
						{
							if (arrayList.Count > 0)
							{
								flag = false;
							}
							for (int i = 0; i < arrayList.Count; i++)
							{
								string text2 = (string)sqlDataReader[1];
								if (text2 == arrayList[i].ToString() || text2.IndexOf(arrayList[i].ToString() + "\\") == 0)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								arrZoneID.Add(sqlDataReader[0]);
								arrZoneName.Add(sqlDataReader[1]);
								arrZoneNO.Add(sqlDataReader[2]);
							}
						}
						sqlDataReader.Close();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0011C47C File Offset: 0x0011B47C
		public int getZone_Acc(ref ArrayList arrZoneName, ref ArrayList arrZoneID, ref ArrayList arrZoneNO)
		{
			int num = -9;
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						arrZoneName.Clear();
						arrZoneID.Clear();
						arrZoneNO.Clear();
						oleDbCommand.CommandText = "SELECT f_ZoneName from t_b_Controller_Zone,t_b_Controller_Zone4Operator WHERE t_b_Controller_Zone4Operator.f_ZoneID = t_b_Controller_Zone.f_ZoneID  AND t_b_Controller_Zone4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_ZoneName  + '\\' ASC";
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						ArrayList arrayList = new ArrayList();
						while (oleDbDataReader.Read())
						{
							arrayList.Add(oleDbDataReader[0]);
						}
						oleDbDataReader.Close();
						if (arrayList.Count == 0)
						{
							arrZoneName.Add("");
							arrZoneID.Add(0);
							arrZoneNO.Add(0);
						}
						string text = "SELECT f_ZoneID,f_ZoneName, f_ZoneNO from [t_b_Controller_Zone]  ";
						text += "  ORDER BY f_ZoneName  + '\\' ASC";
						oleDbCommand.CommandText = text;
						oleDbDataReader = oleDbCommand.ExecuteReader();
						bool flag = true;
						while (oleDbDataReader.Read())
						{
							if (arrayList.Count > 0)
							{
								flag = false;
							}
							for (int i = 0; i < arrayList.Count; i++)
							{
								string text2 = (string)oleDbDataReader[1];
								if (text2 == arrayList[i].ToString() || text2.IndexOf(arrayList[i].ToString() + "\\") == 0)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								arrZoneID.Add(oleDbDataReader[0]);
								arrZoneName.Add(oleDbDataReader[1]);
								arrZoneNO.Add(oleDbDataReader[2]);
							}
						}
						oleDbDataReader.Close();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0011C6AC File Offset: 0x0011B6AC
		public static int getZoneChildMaxNo(string ZoneName, ArrayList arrZoneName, ArrayList arrZoneNO)
		{
			int num = 0;
			try
			{
				string text = ZoneName + "\\";
				for (int i = 0; i < arrZoneName.Count; i++)
				{
					if (arrZoneName[i].ToString().IndexOf(text) == 0 && int.Parse(arrZoneNO[i].ToString()) > num)
					{
						num = int.Parse(arrZoneNO[i].ToString());
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return num;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0011C728 File Offset: 0x0011B728
		public int getZoneID(string ZoneNewName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getZoneID_Acc(ZoneNewName);
			}
			int num = -1;
			if (ZoneNewName != null)
			{
				if (ZoneNewName == "")
				{
					return num;
				}
				try
				{
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
						{
							if (sqlConnection.State != ConnectionState.Open)
							{
								sqlConnection.Open();
							}
							string text = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
							text = text + " WHERE (f_ZoneName = " + wgTools.PrepareStrNUnicode(ZoneNewName) + " ) ";
							sqlCommand.CommandText = text;
							SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
							if (sqlDataReader.Read())
							{
								num = (int)sqlDataReader[0];
							}
							sqlDataReader.Close();
						}
						return num;
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				return num;
			}
			return num;
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0011C828 File Offset: 0x0011B828
		public int getZoneID_Acc(string ZoneNewName)
		{
			int num = -1;
			if (ZoneNewName != null)
			{
				if (ZoneNewName == "")
				{
					return num;
				}
				try
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
						{
							if (oleDbConnection.State != ConnectionState.Open)
							{
								oleDbConnection.Open();
							}
							string text = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
							text = text + " WHERE (f_ZoneName = " + wgTools.PrepareStrNUnicode(ZoneNewName) + " ) ";
							oleDbCommand.CommandText = text;
							OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
							if (oleDbDataReader.Read())
							{
								num = (int)oleDbDataReader[0];
							}
							oleDbDataReader.Close();
						}
						return num;
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				return num;
			}
			return num;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0011C91C File Offset: 0x0011B91C
		public int Update(string ZoneName, string ZoneNewName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.Update_Acc(ZoneName, ZoneNewName);
			}
			int num = -9;
			if (ZoneName == null)
			{
				return -201;
			}
			if (ZoneName == "")
			{
				return -201;
			}
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						string text = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
						text = string.Concat(new string[]
						{
							text,
							" WHERE (f_ZoneName = ",
							wgTools.PrepareStrNUnicode(ZoneName),
							" )  or (f_ZoneName like ",
							wgTools.PrepareStrNUnicode(ZoneName + "\\%"),
							" )  ORDER BY f_ZoneName  + '\\' ASC"
						});
						sqlCommand.CommandText = text;
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							wgAppConfig.runUpdateSql("UPDATE t_b_Controller_Zone SET f_ZoneName= " + wgTools.PrepareStrNUnicode(ZoneNewName + sqlDataReader[1].ToString().Substring(ZoneName.Length)) + " WHERE  f_ZoneID= " + sqlDataReader[0].ToString());
						}
						sqlDataReader.Close();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0011CA94 File Offset: 0x0011BA94
		public int Update_Acc(string ZoneName, string ZoneNewName)
		{
			int num = -9;
			if (ZoneName == null)
			{
				return -201;
			}
			if (ZoneName == "")
			{
				return -201;
			}
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						string text = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
						text = string.Concat(new string[]
						{
							text,
							" WHERE (f_ZoneName = ",
							wgTools.PrepareStrNUnicode(ZoneName),
							" )  or (f_ZoneName like ",
							wgTools.PrepareStrNUnicode(ZoneName + "\\%"),
							" )  ORDER BY f_ZoneName  + '\\' ASC"
						});
						oleDbCommand.CommandText = text;
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							wgAppConfig.runUpdateSql("UPDATE t_b_Controller_Zone SET f_ZoneName= " + wgTools.PrepareStrNUnicode(ZoneNewName + oleDbDataReader[1].ToString().Substring(ZoneName.Length)) + " WHERE  f_ZoneID= " + oleDbDataReader[0].ToString());
						}
						oleDbDataReader.Close();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0011CBFC File Offset: 0x0011BBFC
		public int updateZoneNO()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.updateZoneNO_Acc();
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.Open();
				}
				using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlCommand.CommandText = "SELECT f_ZoneID from [t_b_Controller_Zone]  ORDER BY f_ZoneName  + '\\' ASC ";
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					int num = 1;
					while (sqlDataReader.Read())
					{
						wgAppConfig.runUpdateSql("UPDATE t_b_Controller_Zone SET f_ZoneNO= " + num.ToString() + " WHERE  f_ZoneID= " + sqlDataReader[0].ToString());
						num++;
					}
					sqlDataReader.Close();
				}
			}
			return 1;
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0011CCC4 File Offset: 0x0011BCC4
		public int updateZoneNO_Acc()
		{
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				if (oleDbConnection.State != ConnectionState.Open)
				{
					oleDbConnection.Open();
				}
				using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
				{
					oleDbCommand.CommandText = "SELECT f_ZoneID from [t_b_Controller_Zone]  ORDER BY f_ZoneName  + '\\' ASC ";
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					int num = 1;
					while (oleDbDataReader.Read())
					{
						wgAppConfig.runUpdateSql("UPDATE t_b_Controller_Zone SET f_ZoneNO= " + num.ToString() + " WHERE  f_ZoneID= " + oleDbDataReader[0].ToString());
						num++;
					}
					oleDbDataReader.Close();
				}
			}
			return 1;
		}
	}
}
