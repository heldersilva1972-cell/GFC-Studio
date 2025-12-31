using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc._2019SwipeSendToCenter
{
	// Token: 0x0200032B RID: 811
	public partial class dfrmSendSwipeToCenterA : frmN3000
	{
		// Token: 0x06001997 RID: 6551 RVA: 0x00217451 File Offset: 0x00216451
		public dfrmSendSwipeToCenterA()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x00217480 File Offset: 0x00216480
		private void btnRestoreAll_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnRestoreAll.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.UpdateKeyVal("KEY_SENDDATATOCENTER_RECID", "0");
			}
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x002174CC File Offset: 0x002164CC
		private void btnSetAllrecordSent_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnSetAllrecordSent.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				wgAppConfig.UpdateKeyVal("KEY_SENDDATATOCENTER_RECID", wgAppConfig.getValBySql("SELECT f_RecID from t_d_SwipeRecord ORDER BY f_RecID DESC").ToString());
			}
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00217528 File Offset: 0x00216528
		private void btnStartUpload_Click(object sender, EventArgs e)
		{
			this.strServerURL = this.txtServerURL.Text;
			this.strServerURL = this.strServerURL.Trim();
			if (string.IsNullOrEmpty(this.strServerURL))
			{
				XMessageBox.Show("请输入有效的服务器地址!");
				return;
			}
			this.recMaxLen = (int)this.nudMaxLen.Value;
			this.bStopAll = false;
			this.btnStartUpload.Enabled = false;
			this.bServerConnected = true;
			new Thread(new ThreadStart(this.sendSwipe)).Start();
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x002175B6 File Offset: 0x002165B6
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.bStopAll = true;
			base.Close();
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x002175C5 File Offset: 0x002165C5
		private void cmdStop_Click(object sender, EventArgs e)
		{
			this.bStopAll = true;
			Thread.Sleep(500);
			this.btnStartUpload.Enabled = true;
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x002175E4 File Offset: 0x002165E4
		private void dfrmSendSwipeToCenterA_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.bStopAll = true;
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x002175F0 File Offset: 0x002165F0
		private void dfrmSendSwipeToCenterA_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_SENDDATATOCENTER_SERVERURL")))
			{
				this.txtServerURL.Text = wgAppConfig.GetKeyVal("KEY_SENDDATATOCENTER_SERVERURL");
			}
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_SENDDATATOCENTER_RECMAXLEN")))
			{
				try
				{
					this.nudMaxLen.Value = int.Parse(wgAppConfig.GetKeyVal("KEY_SENDDATATOCENTER_RECMAXLEN"));
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x00217668 File Offset: 0x00216668
		public string PushToWebWithjson(string weburl, string data, Encoding encode)
		{
			byte[] bytes = encode.GetBytes(data);
			string text = "";
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
				httpWebRequest.SendChunked = false;
				httpWebRequest.KeepAlive = false;
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.ContentLength = (long)bytes.Length;
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				text = new StreamReader(httpWebResponse.GetResponseStream(), encode).ReadToEnd();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLogWithoutDB(ex.ToString());
			}
			if (text.IndexOf("\"result\":1") > 0 || text.IndexOf("\"message\":\"success\"") > 0)
			{
				text = "1";
				this.bServerConnected = true;
				return text;
			}
			if (string.IsNullOrEmpty(text))
			{
				this.bServerConnected = false;
				text = "-13";
			}
			return text;
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x00217760 File Offset: 0x00216760
		private string sendSingleInfo(string info)
		{
			string text = string.Format("http://{0}", this.strServerURL);
			string text2 = "";
			if (this.strServerURL.ToUpper().Trim().IndexOf("HTTP") == 0)
			{
				text = this.strServerURL;
			}
			string text3 = text + text2;
			string text4 = string.Concat(new string[]
			{
				string.Format("{{ \r\n", new object[0]),
				string.Format("\"Code\": {0},\r\n", 1),
				string.Format("\"Message\": \"{0}\",\r\n", ""),
				string.Format("\"Data\": [", new object[0]),
				string.Format("{{\r\n", new object[0]),
				info,
				string.Format("}}", new object[0]),
				string.Format("]", new object[0]),
				string.Format("}}", new object[0])
			});
			return this.PushToWebWithjson(text3, text4, Encoding.UTF8);
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x00217878 File Offset: 0x00216878
		private void sendSwipe()
		{
			if (!this.bStopAll)
			{
				base.Invoke(new dfrmSendSwipeToCenterA.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { "开始上传... \r\n" });
				this.sendSingleInfo("");
				int num = int.Parse("0" + wgAppConfig.GetKeyVal("KEY_SENDDATATOCENTER_RECID"));
				bool flag = false;
				for (;;)
				{
					if (this.bServerConnected)
					{
						if (!flag)
						{
							flag = true;
							wgAppConfig.UpdateKeyVal("KEY_SENDDATATOCENTER_SERVERURL", this.strServerURL);
							wgAppConfig.UpdateKeyVal("KEY_SENDDATATOCENTER_RECMAXLEN", this.recMaxLen.ToString());
						}
						num = int.Parse("0" + wgAppConfig.GetKeyVal("KEY_SENDDATATOCENTER_RECID"));
						int valBySql = wgAppConfig.getValBySql("SELECT f_RecID from t_d_SwipeRecord ORDER BY f_RecID DESC");
						if (valBySql == num)
						{
							if (this.bStopAll)
							{
								break;
							}
							Thread.Sleep(300);
						}
						else if (valBySql < num)
						{
							wgAppConfig.UpdateKeyVal("KEY_SENDDATATOCENTER_RECID", valBySql.ToString());
						}
						else if (this.sendSwipeToDataCenter() <= 0)
						{
							Thread.Sleep(10000);
						}
					}
					else
					{
						Thread.Sleep(3000);
						this.sendSingleInfo("");
					}
					if (this.bStopAll)
					{
						return;
					}
				}
				return;
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0021799C File Offset: 0x0021699C
		private int sendSwipeToDataCenter()
		{
			int num = -1;
			try
			{
				int num2 = int.Parse("0" + wgAppConfig.GetKeyVal("KEY_SENDDATATOCENTER_RECID"));
				string text;
				if (wgAppConfig.IsAccessDB)
				{
					text = "             SELECT t_d_SwipeRecord.f_ConsumerID ,\r\n t_d_SwipeRecord.f_RecID AS f_DS_RecID, \r\n              t_d_SwipeRecord.f_CardNO AS f_DS_CardNO,  \r\n              t_b_Consumer.f_ConsumerNO AS f_DS_ConsumerNO, \r\n              t_b_Consumer.f_ConsumerName AS f_DS_ConsumerName, \r\n              t_b_Group.f_GroupName AS f_DS_GroupName, \r\n              t_d_SwipeRecord.f_ReadDate AS f_DS_ReadDate, \r\n              t_b_Reader.f_ReaderName  AS f_DS_ReaderName,\r\n           IIF( t_d_SwipeRecord.f_Character =1, '允许通过' , '禁止通过' )  AS f_DS_Character,\r\n            IIF( t_d_SwipeRecord.f_InOut=0 , '出门' , '进门' )  AS f_DS_InOut,\r\nt_b_Door.f_DoorName  AS f_DS_DoorName,\r\n              t_d_SwipeRecord.f_RecordAll AS f_DS_RecordAll\r\n\t\t\t  , t_b_Consumer_Other.f_Sex\r\n, t_b_Consumer_Other.f_CertificateID\r\n, t_b_Consumer_Other.f_Title\r\n, t_d_SwipeRecord.f_Modified\r\n,t_d_SwipeRecord.f_ControllerSN\r\n,t_b_Reader.f_ReaderNO\r\n,t_b_Reader.f_ControllerID\r\n       FROM (((((t_d_SwipeRecord LEFT JOIN t_b_Consumer ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID))  \r\n                   LEFT JOIN t_b_Consumer_Other on (t_b_Consumer_Other.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID) )  \r\n             LEFT JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = t_d_SwipeRecord.f_ReaderID) )  \r\n                LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ))  \r\nLEFT JOIN t_b_Door ON (\r\n(t_b_Door.f_ControllerID = t_b_Reader.f_ControllerID ) \r\nand  (1<400000000) and (\r\n (((t_b_Door.f_DoorNO*2) = t_b_Reader.f_ReaderNO) or ((t_b_Door.f_DoorNO*2-1) = t_b_Reader.f_ReaderNO)))\r\n)\r\n )   ";
					text = text + "  WHERE ( ((((f_RecOption - (f_RecOption mod 2)) / 2) mod 2) =0))  AND f_RecID > " + num2.ToString() + " ORDER BY f_RecID ASC";
				}
				else
				{
					text = "         SELECT  t_d_SwipeRecord.f_ConsumerID ,\r\n              t_d_SwipeRecord.f_RecID AS f_DS_RecID, \r\n              t_d_SwipeRecord.f_CardNO AS f_DS_CardNO,  \r\n              t_b_Consumer.f_ConsumerNO AS f_DS_ConsumerNO, \r\n              t_b_Consumer.f_ConsumerName AS f_DS_ConsumerName, \r\n              t_b_Group.f_GroupName AS f_DS_GroupName, \r\n              t_d_SwipeRecord.f_ReadDate AS f_DS_ReadDate, \r\n              t_b_Reader.f_ReaderName  AS f_DS_ReaderName,\r\n             CASE WHEN t_d_SwipeRecord.f_Character =1 THEN '允许通过' ELSE '禁止通过' END  AS f_DS_Character,\r\n             CASE WHEN t_d_SwipeRecord.f_InOut=0 THEN '出门' ELSE '进门' END  AS f_DS_InOut,\r\n              t_b_Door.f_DoorName  AS f_DS_DoorName,\r\n              t_d_SwipeRecord.f_RecordAll AS f_DS_RecordAll\r\n\t\t\t  , t_b_Consumer_Other.f_Sex\r\n, t_b_Consumer_Other.f_CertificateID\r\n, t_b_Consumer_Other.f_Title\r\n, t_d_SwipeRecord.f_Modified\r\n,  t_d_SwipeRecord.f_ControllerSN\r\n,t_b_Reader.f_ReaderNO\r\n,t_b_Reader.f_ControllerID\r\n                FROM (((t_d_SwipeRecord LEFT JOIN t_b_Consumer ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) \r\n                LEFT JOIN t_b_Consumer_Other on (t_b_Consumer_Other.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)   \r\n                LEFT JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = t_d_SwipeRecord.f_ReaderID) )  \r\n                LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ))\r\n                LEFT JOIN t_b_Door ON (t_b_Door.f_ControllerID = t_b_Reader.f_ControllerID \r\n                and ((f_ControllerSN>400000000 and t_b_Door.f_DoorNO = t_b_Reader.f_ReaderNO)\r\n                or (f_ControllerSN<400000000 and (((t_b_Door.f_DoorNO*2) = t_b_Reader.f_ReaderNO) or ((t_b_Door.f_DoorNO*2-1) = t_b_Reader.f_ReaderNO))))\r\n                  )\r\n        WHERE ( ((((f_RecOption - (f_RecOption % 2)) / 2) % 2) =0))";
					text = text + " AND f_RecID > " + num2.ToString() + " ORDER BY f_RecID ASC";
				}
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
				{
					using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
					{
						dbCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						dbConnection.Open();
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						int num6 = 0;
						string text2 = "";
						string text3 = "1";
						while (dbDataReader.Read())
						{
							num4 = (int)dbDataReader["f_DS_RecID"];
							if (wgTools.SetObjToStr(dbDataReader["f_DS_Character"]).Equals("允许通过"))
							{
								if (string.IsNullOrEmpty(wgTools.SetObjToStr(dbDataReader["f_DS_ConsumerName"])))
								{
									if ((int)dbDataReader["f_ConsumerID"] == 0)
									{
										DateTime dateTime = (DateTime)dbDataReader["f_Modified"];
										if (!(DateTime.Now >= dateTime.AddMinutes(5.0)))
										{
											DateTime dateTime2 = (DateTime)dbDataReader["f_Modified"];
											if (!(DateTime.Now <= dateTime2.AddMinutes(-5.0)))
											{
												string text4 = " SELECT t_b_Consumer.f_ConsumerName from t_d_SwipeRecord ,t_b_Consumer  WHERE  t_d_SwipeRecord.f_CardNO = t_b_Consumer.f_CardNO  ";
												if (!string.IsNullOrEmpty(wgAppConfig.getValStringBySql(text4)) && !string.IsNullOrEmpty(wgAppConfig.getValStringBySql(" SELECT [f_RecID]     FROM [t_s_wglog] where f_EventDesc like '%提取记录%' and f_LogDateTime >= " + wgTools.PrepareStr(DateTime.Now.AddMinutes(-5.0), true, wgTools.DisplayFormat_DateYMDHMS))))
												{
													text3 = "-101";
													break;
												}
											}
										}
									}
								}
								else
								{
									if (!string.IsNullOrEmpty(text2))
									{
										text2 = text2 + string.Format("}}", new object[0]) + string.Format(",{{ \r\n", new object[0]);
									}
									int num7 = (int)dbDataReader["f_DS_RecID"];
									DateTime dateTime3 = (DateTime)dbDataReader["f_DS_ReadDate"];
									text2 = string.Concat(new string[]
									{
										text2,
										string.Format("\"EventID\": {0},\r\n", wgTools.SetObjToStr(dbDataReader["f_DS_RecID"])),
										string.Format("\"EventTime\": \"{0}\",\r\n", dateTime3.ToString("yyyy-MM-dd HH:mm:ss")),
										string.Format("\"EmployeeID\": {0},\r\n", wgTools.SetObjToStr(dbDataReader["f_ConsumerID"])),
										string.Format("\"EmpID\": \"{0}\",\r\n", wgTools.SetObjToStr(dbDataReader["f_ConsumerID"])),
										string.Format("\"CardNo\": \"{0}\",\r\n", wgTools.SetObjToStr(dbDataReader["f_DS_CardNO"])),
										string.Format("\"EventType\": {0},\r\n", wgTools.SetObjToStr(dbDataReader["f_DS_InOut"]).Equals("进门") ? 1 : 0),
										string.Format("\"EmployeeName\": \"{0}\",\r\n", wgTools.SetObjToStr(dbDataReader["f_DS_ConsumerName"])),
										string.Format("\"Sex\": \"{0}\",\r\n", wgTools.SetObjToStr(dbDataReader["f_Sex"])),
										string.Format("\"PersonCode\": \"{0}\",\r\n", wgTools.SetObjToStr(dbDataReader["f_CertificateID"])),
										string.Format("\"ControlName\": \"{0}\",\r\n", wgTools.SetObjToStr(dbDataReader["f_ControllerSN"])),
										string.Format("\"QRCode\": \"{0}\",\r\n", ""),
										string.Format("\"DeptName\": \"{0}\",\r\n", wgTools.SetObjToStr(dbDataReader["f_DS_GroupName"])),
										string.Format("\"JobName\": \"{0}\",\r\n", wgTools.SetObjToStr(dbDataReader["f_Title"])),
										string.Format("\"EmployeeCode\": \"{0}\",\r\n", wgTools.SetObjToStr(dbDataReader["f_DS_ConsumerNO"]))
									});
									if (wgAppConfig.IsAccessDB)
									{
										if ((int)dbDataReader["f_ControllerSN"] > 400000000 && (int)dbDataReader["f_ReaderNO"] > 1)
										{
											string valStringBySql = wgAppConfig.getValStringBySql("Select t_b_Door.f_DoorName from t_b_door,t_b_reader where t_b_Door.f_DoorNO = t_b_Reader.f_ReaderNO AND t_b_Reader.f_ControllerID=" + wgTools.SetObjToStr(dbDataReader["f_ControllerID"]));
											text2 += string.Format("\"DoorName\": \"{0}\"\r\n", wgTools.SetObjToStr(valStringBySql));
										}
										else
										{
											text2 += string.Format("\"DoorName\": \"{0}\"\r\n", wgTools.SetObjToStr(dbDataReader["f_DS_DoorName"]));
										}
									}
									else
									{
										text2 += string.Format("\"DoorName\": \"{0}\"\r\n", wgTools.SetObjToStr(dbDataReader["f_DS_DoorName"]));
									}
									num6++;
									if (num6 >= this.recMaxLen)
									{
										text3 = this.sendSingleInfo(text2);
										if (!text3.Equals("1"))
										{
											break;
										}
										num3 = num7;
										this.recID4Display = num3;
										num5 += num6;
										num6 = 0;
										text2 = "";
									}
								}
							}
						}
						if (text3.Equals("1"))
						{
							if (num6 > 0)
							{
								text3 = this.sendSingleInfo(text2);
								if (text3.Equals("1"))
								{
									num3 = num4;
									this.recID4Display = num3;
									num5 += num6;
								}
							}
							else if (num4 > 0)
							{
								num3 = num4;
								this.recID4Display = num3;
							}
						}
						dbDataReader.Close();
						if (num3 > 0)
						{
							this.recID4Display = num3;
							wgAppConfig.UpdateKeyVal("KEY_SENDDATATOCENTER_RECID", num3.ToString());
						}
						if (text3.Equals("1"))
						{
							base.Invoke(new dfrmSendSwipeToCenterA.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { "上传完成. 数量= " + num5.ToString() + "\r\n" });
							return 1;
						}
						base.Invoke(new dfrmSendSwipeToCenterA.txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { "上传失败...Err= " + text3 + "\r\n" });
						return num;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLogWithoutDB(ex.ToString());
			}
			return num;
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x00218060 File Offset: 0x00217060
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.bStopAll)
			{
				if (!this.btnStartUpload.Enabled)
				{
					this.btnStartUpload.Enabled = true;
				}
			}
			else if (!this.bServerConnected)
			{
				this.btnErrConnect.Visible = !this.btnErrConnect.Visible;
			}
			else
			{
				this.btnErrConnect.Visible = false;
			}
			if (this.recID4Display > 0)
			{
				this.lblRecID.Text = "RecID=" + this.recID4Display.ToString();
			}
			if (this.txtRunInfo.Text.Length > 20000)
			{
				this.txtRunInfo.Text = this.txtRunInfo.Text.Substring(this.txtRunInfo.Text.Length - 2000);
			}
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x00218130 File Offset: 0x00217130
		private void txtInfoUpdateEntry(object info)
		{
			this.txtRunInfo.AppendText(DateTime.Now.ToString("MM-dd HH:mm:ss ") + wgTools.SetObjToStr(info) + "\r\n");
		}

		// Token: 0x04003453 RID: 13395
		private int recID4Display;

		// Token: 0x04003454 RID: 13396
		private bool bServerConnected = true;

		// Token: 0x04003455 RID: 13397
		private bool bStopAll = true;

		// Token: 0x04003456 RID: 13398
		private int recMaxLen = 1;

		// Token: 0x04003457 RID: 13399
		private string strServerURL = "";

		// Token: 0x0200032C RID: 812
		// (Invoke) Token: 0x060019A8 RID: 6568
		private delegate void txtInfoUpdate(object info);
	}
}
