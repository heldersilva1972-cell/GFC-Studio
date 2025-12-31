using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.DataOper;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Core
{
	// Token: 0x020001F2 RID: 498
	internal class wgMail
	{
		// Token: 0x06000C6F RID: 3183
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int sendM(IntPtr info, int infoLen, IntPtr title, int titleLen);

		// Token: 0x06000C70 RID: 3184 RVA: 0x000FA348 File Offset: 0x000F9348
		internal static bool sendMail2014(string strInfo, string strMailSubject)
		{
			try
			{
				return wgMail.sendMail2017(strInfo, strMailSubject + DateTime.Now.ToString("_(yyyyMMdd_HH:mm:ss.fff)"));
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x000FA38C File Offset: 0x000F938C
		private static bool sendMail2017(string strInfo, string mailSubject)
		{
			return false;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x000FA390 File Offset: 0x000F9390
		internal static bool sendMail2018(string strInfo, string strMailSubject)
		{
			try
			{
				strInfo.IndexOf("DATA=");
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return true;
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x000FA3D4 File Offset: 0x000F93D4
		public static void sendMailOnce()
		{
			string text2;
			string text = wgMail.sysInfo4Mail(out text2);
			int i = 0;
			wgMail.bSendingMail = true;
			wgMail.sendMail2018(text, text2);
			while (i < 3)
			{
				if (wgMail.sendMail2014(text, text2))
				{
					wgMail.bSendingMail = false;
					return;
				}
				Thread.Sleep(15000);
				i++;
			}
			wgMail.bSendingMail = false;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000FA424 File Offset: 0x000F9424
		public static void sendMailOnce2018()
		{
			string text2;
			string text = wgMail.sysInfo4Mail(out text2);
			wgMail.bSendingMail = true;
			wgMail.sendMail2018(text, text2);
			wgMail.bSendingMail = false;
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x000FA450 File Offset: 0x000F9450
		private static string sysInfo4Mail(out string subject)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return wgMail.sysInfo4Mail_Acc(out subject);
			}
			string text = "";
			string text2 = "Mail Subject";
			try
			{
				text = text + "\r\n【软件版本】：V" + Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."));
				string systemParamByNO = wgAppConfig.getSystemParamByNO(49);
				text += "\r\n【起始日期】：";
				if (!string.IsNullOrEmpty(systemParamByNO))
				{
					text += DateTime.Parse(systemParamByNO).ToString("yyyy-MM-dd");
				}
				text += "\r\n【硬件版本】：";
				string text3;
				string text4;
				wgAppConfig.getSystemParamValue(48, out text3, out text3, out text4);
				if (!string.IsNullOrEmpty(text4) && text4.IndexOf("\r\n") >= 0)
				{
					text4 = text4.Substring(text4.IndexOf("\r\n") + "\r\n".Length);
				}
				string text5 = "";
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand("", sqlConnection))
					{
						sqlConnection.Open();
						sqlCommand.CommandText = "SELECT f_ControllerSN FROM t_b_Controller ";
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							text5 += wgTools.SetObjToStr(sqlDataReader[0]);
							if (text4.IndexOf(sqlDataReader[0].ToString() + ",VER=") >= 0)
							{
								string text6 = text4.Substring(text4.IndexOf(sqlDataReader[0].ToString() + ",VER=") + (sqlDataReader[0].ToString() + ",VER=").Length);
								if (text6.Length > 0)
								{
									text6 = text6.Substring(0, text6.IndexOf(","));
									text5 = text5 + "(v" + text6 + ");";
								}
								else
								{
									text5 += "(v  )";
								}
							}
							else
							{
								text5 += "(v  )";
							}
						}
						sqlDataReader.Close();
					}
				}
				if (!string.IsNullOrEmpty(text5))
				{
					text += text5;
				}
				text += "\r\n";
				text += "\r\n【使用者公司全称】：";
				string text7;
				string text8;
				wgAppConfig.getSystemParamValue(36, out text3, out text7, out text8);
				if (!string.IsNullOrEmpty(text7))
				{
					text += text7;
				}
				if (icOperator.checkSoftwareRegister() > 0)
				{
					text = text + "\r\n" + CommonStr.strAlreadyRegistered;
					text2 = text7 + "[" + CommonStr.strAlreadyRegistered + "]";
					text = text + "\r\n【施工和承建公司名称】：" + text8;
				}
				else
				{
					text = text + "\r\n" + CommonStr.strUnRegistered;
					text2 = text7 + "[" + CommonStr.strUnRegistered + "]";
					text = text + "\r\n【施工和承建公司名称】：" + text8;
				}
				text2 = string.Concat(new string[]
				{
					wgAppConfig.ProductTypeOfApp,
					"2014_",
					text2,
					"_V",
					Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."))
				});
				using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand2 = new SqlCommand("", sqlConnection2))
					{
						sqlConnection2.Open();
						sqlCommand2.CommandText = " SELECT COUNT(*)  from t_b_door where f_doorEnabled=1";
						SqlDataReader sqlDataReader = sqlCommand2.ExecuteReader();
						if (sqlDataReader.Read())
						{
							text = text + "\r\n【门数】：" + sqlDataReader[0].ToString();
						}
						sqlDataReader.Close();
					}
				}
				using (SqlConnection sqlConnection3 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand3 = new SqlCommand("", sqlConnection3))
					{
						sqlConnection3.Open();
						sqlCommand3.CommandText = "SELECT f_ControllerSN FROM t_b_Controller ";
						SqlDataReader sqlDataReader = sqlCommand3.ExecuteReader();
						text5 = "";
						while (sqlDataReader.Read())
						{
							text5 = text5 + "\r\n" + sqlDataReader[0].ToString();
						}
						sqlDataReader.Close();
					}
				}
				if (!string.IsNullOrEmpty(text5))
				{
					text = text + "\r\n【控制器序列号S/N】：" + text5;
				}
				text += "\r\n【其他信息】：";
				using (SqlConnection sqlConnection4 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand4 = new SqlCommand("", sqlConnection4))
					{
						sqlConnection4.Open();
						sqlCommand4.CommandText = "SELECT count(*) FROM t_b_Consumer ";
						SqlDataReader sqlDataReader = sqlCommand4.ExecuteReader();
						if (sqlDataReader.Read())
						{
							text = text + "\r\n【注册人数】：" + sqlDataReader[0].ToString();
						}
						sqlDataReader.Close();
					}
				}
				using (SqlConnection sqlConnection5 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand5 = new SqlCommand("", sqlConnection5))
					{
						sqlConnection5.Open();
						sqlCommand5.CommandText = "SELECT f_No, f_Value FROM t_a_SystemParam WHERE f_NO>100 ORDER BY f_No Asc ";
						SqlDataReader sqlDataReader = sqlCommand5.ExecuteReader();
						if (sqlDataReader.HasRows)
						{
							string text9 = "";
							text += "\r\n【启用功能(No,Val)】：\r\n";
							while (sqlDataReader.Read())
							{
								string text10 = text;
								text = string.Concat(new string[]
								{
									text10,
									" (",
									sqlDataReader["f_No"].ToString(),
									",",
									wgTools.SetObjToStr(sqlDataReader["f_Value"]),
									")"
								});
								if (wgTools.SetObjToStr(sqlDataReader["f_Value"]) == "1")
								{
									text9 = text9 + sqlDataReader["f_No"].ToString() + ";";
								}
							}
							if (!string.IsNullOrEmpty(text9))
							{
								text = text + "\r\n【已启用】：" + text9;
							}
						}
						sqlDataReader.Close();
					}
				}
				using (SqlConnection sqlConnection6 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand6 = new SqlCommand("", sqlConnection6))
					{
						sqlConnection6.Open();
						sqlCommand6.CommandText = "SELECT f_No, f_Value FROM t_a_SystemParam  WHERE f_NO<100  ORDER BY f_No Asc ";
						SqlDataReader sqlDataReader = sqlCommand6.ExecuteReader();
						if (sqlDataReader.HasRows)
						{
							text += "\r\n【参数值(No,Val)】：\r\n";
							while (sqlDataReader.Read())
							{
								string text11 = text;
								text = string.Concat(new string[]
								{
									text11,
									" (",
									sqlDataReader["f_No"].ToString(),
									",",
									wgTools.SetObjToStr(sqlDataReader["f_Value"]),
									")"
								});
							}
						}
						sqlDataReader.Close();
					}
				}
				if (!string.IsNullOrEmpty(text4))
				{
					text += "\r\n【DrvInfo】：\r\n";
					text += text4;
				}
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						text += string.Format("\r\n【{0}】： ", CommonStr.strSystem);
						text += string.Format("\r\n{0} ", managementObject["Caption"]);
						text += string.Format("\r\n{0} ", managementObject["version"].ToString());
						text += string.Format("\r\n{0} ", managementObject["CSDVersion"]);
					}
				}
				text += string.Format("\r\n【数据库版本】： ", new object[0]);
				string text12 = "SELECT @@VERSION";
				using (SqlConnection sqlConnection7 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand7 = new SqlCommand("", sqlConnection7))
					{
						sqlConnection7.Open();
						sqlCommand7.CommandText = text12;
						text12 = wgTools.SetObjToStr(sqlCommand7.ExecuteScalar());
					}
				}
				if (!string.IsNullOrEmpty(text12))
				{
					text += string.Format("\r\n{0}", text12);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			text = text + "\r\n---------------------------------------------\r\n\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
			subject = text2;
			return text;
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x000FAE6C File Offset: 0x000F9E6C
		private static string sysInfo4Mail_Acc(out string subject)
		{
			string text = "";
			string text2 = "Mail Subject";
			try
			{
				text = text + "\r\n【软件版本】：V" + Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."));
				string systemParamByNO = wgAppConfig.getSystemParamByNO(49);
				text += "\r\n【起始日期】：";
				if (!string.IsNullOrEmpty(systemParamByNO))
				{
					text += DateTime.Parse(systemParamByNO).ToString("yyyy-MM-dd");
				}
				text += "\r\n【硬件版本】：";
				string text3;
				string text4;
				wgAppConfig.getSystemParamValue(48, out text3, out text3, out text4);
				if (!string.IsNullOrEmpty(text4) && text4.IndexOf("\r\n") >= 0)
				{
					text4 = text4.Substring(text4.IndexOf("\r\n") + "\r\n".Length);
				}
				string text5 = "";
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand("", oleDbConnection))
					{
						oleDbConnection.Open();
						oleDbCommand.CommandText = "SELECT f_ControllerSN FROM t_b_Controller ";
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							text5 += wgTools.SetObjToStr(oleDbDataReader[0]);
							if (text4.IndexOf(oleDbDataReader[0].ToString() + ",VER=") >= 0)
							{
								string text6 = text4.Substring(text4.IndexOf(oleDbDataReader[0].ToString() + ",VER=") + (oleDbDataReader[0].ToString() + ",VER=").Length);
								if (text6.Length > 0)
								{
									text6 = text6.Substring(0, text6.IndexOf(","));
									text5 = text5 + "(v" + text6 + ");";
								}
								else
								{
									text5 += "(v  )";
								}
							}
							else
							{
								text5 += "(v  )";
							}
						}
						oleDbDataReader.Close();
					}
				}
				if (!string.IsNullOrEmpty(text5))
				{
					text += text5;
				}
				text += "\r\n";
				text += "\r\n【使用者公司全称】：";
				string text7;
				string text8;
				wgAppConfig.getSystemParamValue(36, out text3, out text7, out text8);
				if (!string.IsNullOrEmpty(text7))
				{
					text += text7;
				}
				if (icOperator.checkSoftwareRegister() > 0)
				{
					text = text + "\r\n" + CommonStr.strAlreadyRegistered;
					text2 = text7 + "[" + CommonStr.strAlreadyRegistered + "]";
					text = text + "\r\n【施工和承建公司名称】：" + text8;
				}
				else
				{
					text = text + "\r\n" + CommonStr.strUnRegistered;
					text2 = text7 + "[" + CommonStr.strUnRegistered + "]";
					text = text + "\r\n【施工和承建公司名称】：" + text8;
				}
				text2 = string.Concat(new string[]
				{
					wgAppConfig.ProductTypeOfApp,
					"2014_",
					text2,
					"_V",
					Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."))
				});
				using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand("", oleDbConnection2))
					{
						oleDbConnection2.Open();
						oleDbCommand2.CommandText = " SELECT COUNT(*)  from t_b_door where f_doorEnabled=1";
						OleDbDataReader oleDbDataReader = oleDbCommand2.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							text = text + "\r\n【门数】：" + oleDbDataReader[0].ToString();
						}
						oleDbDataReader.Close();
					}
				}
				using (OleDbConnection oleDbConnection3 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand3 = new OleDbCommand("", oleDbConnection3))
					{
						oleDbConnection3.Open();
						oleDbCommand3.CommandText = "SELECT f_ControllerSN FROM t_b_Controller ";
						OleDbDataReader oleDbDataReader = oleDbCommand3.ExecuteReader();
						text5 = "";
						while (oleDbDataReader.Read())
						{
							text5 = text5 + "\r\n" + oleDbDataReader[0].ToString();
						}
						oleDbDataReader.Close();
					}
				}
				if (!string.IsNullOrEmpty(text5))
				{
					text = text + "\r\n【控制器序列号S/N】：" + text5;
				}
				text += "\r\n【其他信息】：";
				using (OleDbConnection oleDbConnection4 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand4 = new OleDbCommand("", oleDbConnection4))
					{
						oleDbConnection4.Open();
						oleDbCommand4.CommandText = "SELECT count(*) FROM t_b_Consumer ";
						OleDbDataReader oleDbDataReader = oleDbCommand4.ExecuteReader();
						if (oleDbDataReader.Read())
						{
							text = text + "\r\n【注册人数】：" + oleDbDataReader[0].ToString();
						}
						oleDbDataReader.Close();
					}
				}
				using (OleDbConnection oleDbConnection5 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand5 = new OleDbCommand("", oleDbConnection5))
					{
						oleDbConnection5.Open();
						oleDbCommand5.CommandText = "SELECT f_No, f_Value FROM t_a_SystemParam WHERE f_NO>100 ORDER BY f_No Asc ";
						OleDbDataReader oleDbDataReader = oleDbCommand5.ExecuteReader();
						if (oleDbDataReader.HasRows)
						{
							string text9 = "";
							text += "\r\n【启用功能(No,Val)】：\r\n";
							while (oleDbDataReader.Read())
							{
								string text10 = text;
								text = string.Concat(new string[]
								{
									text10,
									" (",
									oleDbDataReader["f_No"].ToString(),
									",",
									wgTools.SetObjToStr(oleDbDataReader["f_Value"]),
									")"
								});
								if (wgTools.SetObjToStr(oleDbDataReader["f_Value"]) == "1")
								{
									text9 = text9 + oleDbDataReader["f_No"].ToString() + ";";
								}
							}
							if (!string.IsNullOrEmpty(text9))
							{
								text = text + "\r\n【已启用】：" + text9;
							}
						}
						oleDbDataReader.Close();
					}
				}
				using (OleDbConnection oleDbConnection6 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand6 = new OleDbCommand("", oleDbConnection6))
					{
						oleDbConnection6.Open();
						oleDbCommand6.CommandText = "SELECT f_No, f_Value FROM t_a_SystemParam  WHERE f_NO<100  ORDER BY f_No Asc ";
						OleDbDataReader oleDbDataReader = oleDbCommand6.ExecuteReader();
						if (oleDbDataReader.HasRows)
						{
							text += "\r\n【参数值(No,Val)】：\r\n";
							while (oleDbDataReader.Read())
							{
								string text11 = text;
								text = string.Concat(new string[]
								{
									text11,
									" (",
									oleDbDataReader["f_No"].ToString(),
									",",
									wgTools.SetObjToStr(oleDbDataReader["f_Value"]),
									")"
								});
							}
						}
						oleDbDataReader.Close();
					}
				}
				if (!string.IsNullOrEmpty(text4))
				{
					text += "\r\n【DrvInfo】：\r\n";
					text += text4;
				}
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						text += string.Format("\r\n【{0}】： ", CommonStr.strSystem);
						text += string.Format("\r\n{0} ", managementObject["Caption"]);
						text += string.Format("\r\n{0} ", managementObject["version"].ToString());
						text += string.Format("\r\n{0} ", managementObject["CSDVersion"]);
					}
				}
				text += string.Format("\r\n【数据库版本】： ", new object[0]);
				string text12 = "Microsoft Access";
				if (!string.IsNullOrEmpty(text12))
				{
					text += string.Format("\r\n{0}", text12);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			text = text + "\r\n---------------------------------------------\r\n\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
			subject = text2;
			return text;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000FB7F8 File Offset: 0x000FA7F8
		public static byte[] UnicodeToGB2312(string sendStr)
		{
			return Encoding.GetEncoding(936).GetBytes(sendStr);
		}

		// Token: 0x04001B2C RID: 6956
		public static bool bSendingMail;
	}
}
