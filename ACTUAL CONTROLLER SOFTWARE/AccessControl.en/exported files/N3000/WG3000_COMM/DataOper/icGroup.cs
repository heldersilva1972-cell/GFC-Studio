using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using WG3000_COMM.Core;

namespace WG3000_COMM.DataOper
{
	// Token: 0x02000224 RID: 548
	internal class icGroup
	{
		// Token: 0x06000FDE RID: 4062 RVA: 0x0011D2F8 File Offset: 0x0011C2F8
		public int addNew(string GroupName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.addNew_Acc(GroupName);
			}
			int num = -9;
			if (GroupName == null)
			{
				return -201;
			}
			if (GroupName == "")
			{
				return -201;
			}
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					if (sqlConnection.State != ConnectionState.Open)
					{
						sqlConnection.Open();
					}
					string text = " INSERT INTO t_b_Group (f_GroupName) values (";
					text = text + wgTools.PrepareStrNUnicode(GroupName) + ")";
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						sqlCommand.CommandText = text;
						sqlCommand.ExecuteNonQuery();
						text = this.getOrderSql();
						sqlCommand.CommandText = text;
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						int num2 = 1;
						while (sqlDataReader.Read())
						{
							wgAppConfig.runUpdateSql("UPDATE t_b_Group SET f_GroupNO= " + num2.ToString() + " WHERE  f_GroupID= " + sqlDataReader[0].ToString());
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

		// Token: 0x06000FDF RID: 4063 RVA: 0x0011D434 File Offset: 0x0011C434
		public int addNew_Acc(string GroupName)
		{
			int num = -9;
			if (GroupName == null)
			{
				return -201;
			}
			if (GroupName == "")
			{
				return -201;
			}
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					string text = " INSERT INTO t_b_Group (f_GroupName) values (";
					text = text + wgTools.PrepareStrNUnicode(GroupName) + ")";
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						oleDbCommand.CommandText = text;
						oleDbCommand.ExecuteNonQuery();
						text = this.getOrderSql();
						oleDbCommand.CommandText = text;
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						int num2 = 1;
						while (oleDbDataReader.Read())
						{
							wgAppConfig.runUpdateSql("UPDATE t_b_Group SET f_GroupNO= " + num2.ToString() + " WHERE  f_GroupID= " + oleDbDataReader[0].ToString());
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

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0011D564 File Offset: 0x0011C564
		public int addNew4BatchExcel(string GroupName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.addNew4BatchExcel_Acc(GroupName);
			}
			int num = -9;
			if (GroupName == null)
			{
				return -201;
			}
			if (GroupName == "")
			{
				return -201;
			}
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					if (sqlConnection.State != ConnectionState.Open)
					{
						sqlConnection.Open();
					}
					string text = " INSERT INTO t_b_Group (f_GroupName) values (";
					text = text + wgTools.PrepareStrNUnicode(GroupName) + ")";
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
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

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0011D648 File Offset: 0x0011C648
		public int addNew4BatchExcel_Acc(string GroupName)
		{
			int num = -9;
			if (GroupName == null)
			{
				return -201;
			}
			if (GroupName == "")
			{
				return -201;
			}
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					string text = " INSERT INTO t_b_Group (f_GroupName) values (";
					text = text + wgTools.PrepareStrNUnicode(GroupName) + ")";
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
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

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0011D71C File Offset: 0x0011C71C
		public bool checkExisted(string GroupNewName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.checkExisted_Acc(GroupNewName);
			}
			bool flag = false;
			if (GroupNewName != null)
			{
				if (GroupNewName == "")
				{
					return flag;
				}
				try
				{
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						string text = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
						text = text + " WHERE (f_GroupName = " + wgTools.PrepareStrNUnicode(GroupNewName) + " ) ";
						using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
						{
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

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0011D810 File Offset: 0x0011C810
		public bool checkExisted_Acc(string GroupNewName)
		{
			bool flag = false;
			if (GroupNewName != null)
			{
				if (GroupNewName == "")
				{
					return flag;
				}
				try
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						string text = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
						text = text + " WHERE (f_GroupName = " + wgTools.PrepareStrNUnicode(GroupNewName) + " ) ";
						using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
						{
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

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0011D8F8 File Offset: 0x0011C8F8
		public int delete(string GroupName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.delete_Acc(GroupName);
			}
			int num = -9;
			if (GroupName == null)
			{
				return -201;
			}
			if (GroupName == "")
			{
				return -201;
			}
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					if (sqlConnection.State != ConnectionState.Open)
					{
						sqlConnection.Open();
					}
					string text = " DELETE FROM t_b_Group WHERE (f_GroupName = ";
					text = string.Concat(new string[]
					{
						text,
						wgTools.PrepareStrNUnicode(GroupName),
						" )  or (f_GroupName like ",
						wgTools.PrepareStrNUnicode(GroupName + "\\%"),
						" ) "
					});
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
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

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0011DA10 File Offset: 0x0011CA10
		public int delete_Acc(string GroupName)
		{
			int num = -9;
			if (GroupName == null)
			{
				return -201;
			}
			if (GroupName == "")
			{
				return -201;
			}
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					string text = " DELETE FROM t_b_Group WHERE (f_GroupName = ";
					text = string.Concat(new string[]
					{
						text,
						wgTools.PrepareStrNUnicode(GroupName),
						" )  or (f_GroupName like ",
						wgTools.PrepareStrNUnicode(GroupName + "\\%"),
						" ) "
					});
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
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

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0011DB18 File Offset: 0x0011CB18
		public int getGroup(ref ArrayList arrGroupName, ref ArrayList arrGroupID, ref ArrayList arrGroupNO)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getGroup_Acc(ref arrGroupName, ref arrGroupID, ref arrGroupNO);
			}
			int num = -9;
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					if (sqlConnection.State != ConnectionState.Open)
					{
						sqlConnection.Open();
					}
					arrGroupName.Clear();
					arrGroupID.Clear();
					arrGroupNO.Clear();
					ArrayList arrayList = new ArrayList();
					using (SqlCommand sqlCommand = new SqlCommand("SELECT f_GroupName from t_b_Group,t_b_Group4Operator WHERE t_b_Group4Operator.f_GroupID = t_b_Group.f_GroupID  AND t_b_Group4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_GroupName  + '\\' ASC", sqlConnection))
					{
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							arrayList.Add(sqlDataReader[0]);
						}
						sqlDataReader.Close();
						if (arrayList.Count == 0)
						{
							arrGroupName.Add("");
							arrGroupID.Add(0);
							arrGroupNO.Add(0);
						}
						string text = "SELECT f_GroupID,f_GroupName, f_GroupNO from [t_b_Group]  ";
						text += "  ORDER BY f_GroupName  + '\\' ASC";
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
								arrGroupID.Add(sqlDataReader[0]);
								arrGroupName.Add(sqlDataReader[1]);
								arrGroupNO.Add(sqlDataReader[2]);
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

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0011DD54 File Offset: 0x0011CD54
		public int getGroup_Acc(ref ArrayList arrGroupName, ref ArrayList arrGroupID, ref ArrayList arrGroupNO)
		{
			int num = -9;
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					arrGroupName.Clear();
					arrGroupID.Clear();
					arrGroupNO.Clear();
					ArrayList arrayList = new ArrayList();
					using (OleDbCommand oleDbCommand = new OleDbCommand("SELECT f_GroupName from t_b_Group,t_b_Group4Operator WHERE t_b_Group4Operator.f_GroupID = t_b_Group.f_GroupID  AND t_b_Group4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_GroupName  + '\\' ASC", oleDbConnection))
					{
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							arrayList.Add(oleDbDataReader[0]);
						}
						oleDbDataReader.Close();
						if (arrayList.Count == 0)
						{
							arrGroupName.Add("");
							arrGroupID.Add(0);
							arrGroupNO.Add(0);
						}
						string text = "SELECT f_GroupID,f_GroupName, f_GroupNO from [t_b_Group]  ";
						text += "  ORDER BY f_GroupName  + '\\' ASC";
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
								arrGroupID.Add(oleDbDataReader[0]);
								arrGroupName.Add(oleDbDataReader[1]);
								arrGroupNO.Add(oleDbDataReader[2]);
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

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0011DF7C File Offset: 0x0011CF7C
		public static int getGroupChildMaxNo(string groupName, ArrayList arrGroupName, ArrayList arrGroupNO)
		{
			int num = 0;
			try
			{
				string text = groupName + "\\";
				for (int i = 0; i < arrGroupName.Count; i++)
				{
					if (arrGroupName[i].ToString().IndexOf(text) == 0 && int.Parse(arrGroupNO[i].ToString()) > num)
					{
						num = int.Parse(arrGroupNO[i].ToString());
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return num;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0011DFF8 File Offset: 0x0011CFF8
		public int getGroupID(string GroupNewName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getGroupID_Acc(GroupNewName);
			}
			int num = -1;
			if (GroupNewName != null)
			{
				if (GroupNewName == "")
				{
					return num;
				}
				try
				{
					using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						string text = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
						text = text + " WHERE (f_GroupName = " + wgTools.PrepareStrNUnicode(GroupNewName) + " ) ";
						using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
						{
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

		// Token: 0x06000FEA RID: 4074 RVA: 0x0011E0F8 File Offset: 0x0011D0F8
		public int getGroupID_Acc(string GroupNewName)
		{
			int num = -1;
			if (GroupNewName != null)
			{
				if (GroupNewName == "")
				{
					return num;
				}
				try
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						string text = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
						text = text + " WHERE (f_GroupName = " + wgTools.PrepareStrNUnicode(GroupNewName) + " ) ";
						using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
						{
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

		// Token: 0x06000FEB RID: 4075 RVA: 0x0011E1EC File Offset: 0x0011D1EC
		public static string getGroupQuery(int groupMinNO, int groupMaxNO, ArrayList arrGroupNO, ArrayList arrGroupID)
		{
			string text = "";
			bool flag = false;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < arrGroupNO.Count; i++)
			{
				if ((int)arrGroupNO[i] <= groupMaxNO && (int)arrGroupNO[i] >= groupMinNO)
				{
					if (text == "")
					{
						text += string.Format(" {0:d} ", (int)arrGroupID[i]);
					}
					else
					{
						text += string.Format(",{0:d} ", (int)arrGroupID[i]);
					}
					if (num == 0)
					{
						num = (int)arrGroupID[i];
						num2 = (int)arrGroupID[i];
						flag = true;
					}
					else if (num2 + 1 == (int)arrGroupID[i])
					{
						num2 = (int)arrGroupID[i];
					}
					else
					{
						flag = false;
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (flag)
			{
				return string.Format(" ((f_GroupID >={0}) AND (f_GroupID <= {1}))", num, num2);
			}
			return string.Format("f_GroupID IN ({0})", text);
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0011E311 File Offset: 0x0011D311
		public string getOrderSql()
		{
			return "SELECT f_GroupID from [t_b_Group]  ORDER BY f_GroupName + '\\' ASC ";
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0011E318 File Offset: 0x0011D318
		public static string getZoneQuery(int ZoneMinNO, int ZoneMaxNO, ArrayList arrZoneNO, ArrayList arrZoneID)
		{
			string text = "";
			bool flag = false;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < arrZoneNO.Count; i++)
			{
				if ((int)arrZoneNO[i] <= ZoneMaxNO && (int)arrZoneNO[i] >= ZoneMinNO)
				{
					if (text == "")
					{
						text += string.Format(" {0:d} ", (int)arrZoneID[i]);
					}
					else
					{
						text += string.Format(",{0:d} ", (int)arrZoneID[i]);
					}
					if (num == 0)
					{
						num = (int)arrZoneID[i];
						num2 = (int)arrZoneID[i];
						flag = true;
					}
					else if (num2 + 1 == (int)arrZoneID[i])
					{
						num2 = (int)arrZoneID[i];
					}
					else
					{
						flag = false;
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (flag)
			{
				return string.Format(" ((f_ZoneID >={0}) AND (f_ZoneID <= {1}))", num, num2);
			}
			return string.Format("f_ZoneID IN ({0})", text);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0011E440 File Offset: 0x0011D440
		public int Update(string GroupName, string GroupNewName)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.Update_Acc(GroupName, GroupNewName);
			}
			int num = -9;
			if (GroupName == null)
			{
				return -201;
			}
			if (GroupName == "")
			{
				return -201;
			}
			int num2;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
				{
					try
					{
						if (sqlConnection.State != ConnectionState.Open)
						{
							sqlConnection.Open();
						}
						string text = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
						text = string.Concat(new string[]
						{
							text,
							" WHERE (f_GroupName = ",
							wgTools.PrepareStrNUnicode(GroupName),
							" )  or (f_GroupName like ",
							wgTools.PrepareStrNUnicode(GroupName + "\\%"),
							" )  ORDER BY f_GroupName  + '\\' ASC"
						});
						sqlCommand.CommandText = text;
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							wgAppConfig.runUpdateSql("UPDATE t_b_Group SET f_GroupName= " + wgTools.PrepareStrNUnicode(GroupNewName + sqlDataReader[1].ToString().Substring(GroupName.Length)) + " WHERE  f_GroupID= " + sqlDataReader[0].ToString());
						}
						sqlDataReader.Close();
						num = 1;
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
					num2 = num;
				}
			}
			return num2;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0011E5BC File Offset: 0x0011D5BC
		public int Update_Acc(string GroupName, string GroupNewName)
		{
			int num = -9;
			if (GroupName == null)
			{
				return -201;
			}
			if (GroupName == "")
			{
				return -201;
			}
			int num2;
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
				{
					try
					{
						if (oleDbConnection.State != ConnectionState.Open)
						{
							oleDbConnection.Open();
						}
						string text = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
						text = string.Concat(new string[]
						{
							text,
							" WHERE (f_GroupName = ",
							wgTools.PrepareStrNUnicode(GroupName),
							" )  or (f_GroupName like ",
							wgTools.PrepareStrNUnicode(GroupName + "\\%"),
							" )  ORDER BY f_GroupName  + '\\' ASC"
						});
						oleDbCommand.CommandText = text;
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							wgAppConfig.runUpdateSql("UPDATE t_b_Group SET f_GroupName= " + wgTools.PrepareStrNUnicode(GroupNewName + oleDbDataReader[1].ToString().Substring(GroupName.Length)) + " WHERE  f_GroupID= " + oleDbDataReader[0].ToString());
						}
						oleDbDataReader.Close();
						num = 1;
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
					num2 = num;
				}
			}
			return num2;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0011E728 File Offset: 0x0011D728
		public int updateGroupNO()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.updateGroupNO_Acc();
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.Open();
				}
				using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlCommand.CommandText = "SELECT f_GroupID from [t_b_Group]  ORDER BY f_GroupName  + '\\' ASC ";
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					int num = 1;
					while (sqlDataReader.Read())
					{
						wgAppConfig.runUpdateSql("UPDATE t_b_Group SET f_GroupNO= " + num.ToString() + " WHERE  f_GroupID= " + sqlDataReader[0].ToString());
						num++;
					}
					sqlDataReader.Close();
				}
			}
			return 1;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0011E7F0 File Offset: 0x0011D7F0
		public int updateGroupNO_Acc()
		{
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				if (oleDbConnection.State != ConnectionState.Open)
				{
					oleDbConnection.Open();
				}
				using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
				{
					oleDbCommand.CommandText = "SELECT f_GroupID from [t_b_Group]  ORDER BY f_GroupName  + '\\' ASC ";
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					int num = 1;
					while (oleDbDataReader.Read())
					{
						wgAppConfig.runUpdateSql("UPDATE t_b_Group SET f_GroupNO= " + num.ToString() + " WHERE  f_GroupID= " + oleDbDataReader[0].ToString());
						num++;
					}
					oleDbDataReader.Close();
				}
			}
			return 1;
		}
	}
}
