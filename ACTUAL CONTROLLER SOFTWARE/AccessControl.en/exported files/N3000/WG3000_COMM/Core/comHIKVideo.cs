using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Core
{
	// Token: 0x020001C0 RID: 448
	public class comHIKVideo : Component
	{
		// Token: 0x06000942 RID: 2370 RVA: 0x000DBC8C File Offset: 0x000DAC8C
		public comHIKVideo()
		{
			this.aviTimelength = 6000;
			this.AutoAdjustTimeOfVideo = 1;
			this.JPEGResolution = 2;
			this.garrCamera = new ArrayList();
			this.garrCameraName = new ArrayList();
			this.garrCameraID = new ArrayList();
			this.p4players = new ArrayList();
			this.p4playersUsedIndex = -1;
			this.p4PlayPanel = new ArrayList();
			this.arrWatchingRecordListIndex = new ArrayList();
			this.arrCardRecordID = new ArrayList();
			this.arrReaderIDWithCamera = new ArrayList();
			this.arrCameraName4ReaderIDWithCamera = new ArrayList();
			this.InitializeComponent();
			this.garrCamera.Clear();
			this.garrCameraID.Clear();
			this.garrCameraName.Clear();
			this.p4players.Clear();
			comHIKVideo.videoErrInfo = "";
			try
			{
				string systemParamByNO = wgAppConfig.getSystemParamByNO(44);
				if (!string.IsNullOrEmpty(systemParamByNO))
				{
					this.aviTimelength = int.Parse(systemParamByNO);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			try
			{
				this.onlyCapturePhoto = int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(157)));
				if (string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(158))))
				{
					this.AutoAdjustTimeOfVideo = 1;
				}
				else
				{
					this.AutoAdjustTimeOfVideo = int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(158)));
				}
				if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(159))))
				{
					this.JPEGResolution = int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(159)));
				}
				this.JPEGQuality = int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(160)));
				this.JPEGCaptureDisable = int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(176)));
				if (this.JPEGCaptureDisable > 0)
				{
					comHIKVideo.bFailedNET_DVR_CaptureJPEGPicture = true;
					if (this.onlyCapturePhoto >= 2)
					{
						this.onlyCapturePhoto = 1;
					}
				}
				this.delaySecondCapture = int.Parse("0" + wgAppConfig.getSystemParamByNO(207));
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[] { EventLogEntryType.Error });
			}
			try
			{
				if (this.hikVideo == null)
				{
					this.hikVideo = new CHCNetSDK();
					CHCNetSDK.NET_DVR_Init();
					int num = CHCNetSDK.NET_DVR_IsSupport();
					if ((num & 1) <= 0)
					{
						wgAppRunInfo.raiseAppRunInfoCommStatus(CommonStr.strNET_DVR_SUPPORT_DDRAW);
						wgTools.WgDebugWrite(CommonStr.strNET_DVR_SUPPORT_DDRAW, new object[0]);
					}
					if ((num & 128) <= 0)
					{
						wgAppRunInfo.raiseAppRunInfoCommStatus(CommonStr.strNET_DVR_SUPPORT_SSE);
						wgTools.WgDebugWrite(CommonStr.strNET_DVR_SUPPORT_SSE, new object[0]);
					}
					if ((num & 2) <= 0)
					{
						wgAppRunInfo.raiseAppRunInfoCommStatus(CommonStr.strNET_DVR_SUPPORT_BLT);
						wgTools.WgDebugWrite(CommonStr.strNET_DVR_SUPPORT_BLT, new object[0]);
					}
					int num2 = 5000;
					int num3 = 1;
					CHCNetSDK.NET_DVR_SetConnectTime(uint.Parse(num2.ToString()), uint.Parse(num3.ToString()));
				}
				this.threadStopRecord = new Thread(new ThreadStart(this.timer50ms));
				this.threadStopRecord.IsBackground = true;
				this.threadStopRecord.Start();
			}
			catch (Exception ex3)
			{
				wgTools.WgDebugWrite(ex3.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x000DC040 File Offset: 0x000DB040
		public comHIKVideo(IContainer Container)
			: this()
		{
			Container.Add(this);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x000DC04F File Offset: 0x000DB04F
		public int CameraNum()
		{
			return this.p4playersUsedIndex + 1;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x000DC05C File Offset: 0x000DB05C
		public bool CaptureNewCardRecord(int readerID, string RecordAll)
		{
			bool flag = false;
			try
			{
				int num = this.arrReaderIDWithCamera.IndexOf(readerID);
				if (num < 0)
				{
					return flag;
				}
				num = this.garrCameraName.IndexOf(this.arrCameraName4ReaderIDWithCamera[num]);
				if (num < 0)
				{
					return flag;
				}
				comHIKVideo.HIKCamera hikcamera = (comHIKVideo.HIKCamera)this.garrCamera[num];
				if (hikcamera.bSaving)
				{
					this.stopRecord(hikcamera);
				}
				hikcamera.fileToSave = wgAppConfig.Path4AviJpg() + RecordAll + ".avi";
				flag = this.startRecord(hikcamera);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return flag;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x000DC124 File Offset: 0x000DB124
		public bool CaptureNewCardRecordMultiThread(int readerID, string RecordAll, ref bool bToCapturePhoto)
		{
			bool flag = false;
			try
			{
				int num = this.arrReaderIDWithCamera.IndexOf(readerID);
				if (num >= 0)
				{
					num = this.garrCameraName.IndexOf(this.arrCameraName4ReaderIDWithCamera[num]);
					if (num >= 0)
					{
						comHIKVideo.HIKCamera hikcamera = (comHIKVideo.HIKCamera)this.garrCamera[num];
						bToCapturePhoto = true;
						hikcamera.RecordAll4Thread = RecordAll;
						new Thread(new ParameterizedThreadStart(this.ThreadMainWithParameters)).Start(hikcamera);
						flag = true;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return flag;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x000DC1D0 File Offset: 0x000DB1D0
		private bool capturePhoto(comHIKVideo.HIKCamera hikcam)
		{
			return this.capturePhotoWithFileName(hikcam, hikcam.fileToSave);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x000DC1E0 File Offset: 0x000DB1E0
		private bool capturePhotoWithFileName(comHIKVideo.HIKCamera hikcam, string filename)
		{
			bool flag = false;
			try
			{
				string text = filename.Replace("avi", "bmp");
				if (comHIKVideo.bFailedNET_DVR_CaptureJPEGPicture && this.onlyCapturePhoto < 2)
				{
					if (CHCNetSDK.NET_DVR_CapturePicture(hikcam.iPlayHandle, text))
					{
						flag = true;
					}
					return flag;
				}
				CHCNetSDK.NET_DVR_JPEGPARA net_DVR_JPEGPARA = default(CHCNetSDK.NET_DVR_JPEGPARA);
				if (!string.IsNullOrEmpty(wgTools.strLogIntoFileName))
				{
					string text2 = text.Replace(".jpg", "");
					string text3 = "";
					string text4 = "";
					for (int i = 0; i < 256; i++)
					{
						net_DVR_JPEGPARA.wPicSize = (ushort)i;
						net_DVR_JPEGPARA.wPicQuality = 2;
						if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(hikcam.iUserId, hikcam.channel, ref net_DVR_JPEGPARA, text2 + "_testing_" + i.ToString() + ".jpg"))
						{
							text4 = text4 + i.ToString() + ", ";
							wgAppConfig.wgLog(string.Format("Not Supported wPicSzie = {0}, Error = {1}", i.ToString(), CHCNetSDK.NET_DVR_GetLastError().ToString()));
						}
						else
						{
							text3 = text3 + i.ToString() + ", ";
						}
					}
					wgAppConfig.wgLog(string.Format("Supported  = {0}", text3));
					wgAppConfig.wgLog(string.Format("Not Supported  = {0}", text4));
				}
				net_DVR_JPEGPARA.wPicSize = 2;
				net_DVR_JPEGPARA.wPicQuality = 0;
				net_DVR_JPEGPARA.wPicSize = (ushort)this.JPEGResolution;
				net_DVR_JPEGPARA.wPicQuality = (ushort)this.JPEGQuality;
				text = text.Replace("bmp", "jpg");
				bool flag2 = false;
				if (this.JPEGCaptureDisable <= 0)
				{
					flag2 = CHCNetSDK.NET_DVR_CaptureJPEGPicture(hikcam.iUserId, hikcam.channel, ref net_DVR_JPEGPARA, text);
				}
				if (!flag2 && this.JPEGCaptureDisable <= 0 && this.JPEGResolution == 2)
				{
					net_DVR_JPEGPARA.wPicSize = 0;
					flag2 = CHCNetSDK.NET_DVR_CaptureJPEGPicture(hikcam.iUserId, hikcam.channel, ref net_DVR_JPEGPARA, text);
					if (flag2)
					{
						wgAppConfig.setSystemParamValue(159, "Video_JPEGResolution", "0", DateTime.Now.ToString(wgTools.YMDHMSFormat));
						this.JPEGResolution = 0;
						wgAppConfig.wgLog("Change JPEGResolution Value: 2=>0 (704*576 -> 352*288)");
					}
				}
				if (flag2)
				{
					return true;
				}
				wgAppConfig.wgLog(string.Format("NET_DVR_CaptureJPEGPicture Ret = {0}, Error = {1}", flag2.ToString(), CHCNetSDK.NET_DVR_GetLastError().ToString()));
				wgAppRunInfo.raiseAppRunInfoCommStatus(CommonStr.strCaptureJPG_FAILED + ":" + text);
				if (CHCNetSDK.NET_DVR_GetLastError() == 23U)
				{
					comHIKVideo.bFailedNET_DVR_CaptureJPEGPicture = true;
				}
				else if (this.JPEGCaptureDisable > 0)
				{
					comHIKVideo.bFailedNET_DVR_CaptureJPEGPicture = true;
				}
				if (comHIKVideo.bFailedNET_DVR_CaptureJPEGPicture && this.onlyCapturePhoto < 2)
				{
					flag2 = CHCNetSDK.NET_DVR_CapturePicture(hikcam.iPlayHandle, text);
				}
			}
			catch (Exception)
			{
			}
			return flag;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x000DC4A0 File Offset: 0x000DB4A0
		private void comHIKVideo_Disposed(object sender, EventArgs e)
		{
			this.stopAllCamera();
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x000DC4A8 File Offset: 0x000DB4A8
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x000DC4C7 File Offset: 0x000DB4C7
		private void InitializeComponent()
		{
			this.components = new Container();
			base.Disposed += this.comHIKVideo_Disposed;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x000DC4E8 File Offset: 0x000DB4E8
		private int IPLng(IPAddress ip)
		{
			byte[] array = new byte[4];
			array = ip.GetAddressBytes();
			return ((int)array[3] << 24) + ((int)array[2] << 16) + ((int)array[1] << 8) + (int)array[0];
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000DC51C File Offset: 0x000DB51C
		public bool loadCamera(bool bReload, string selectedCameraID)
		{
			if (this.p4players.Count <= 0)
			{
				return false;
			}
			this.panelToolTips = new string[this.p4players.Count * 2];
			this.panelErr = new bool[this.p4players.Count * 2];
			this.panelErrInfo = new string[this.p4players.Count * 2];
			comHIKVideo.videoErrInfo = "";
			this.loadReaderIDWithCamera();
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
			string text;
			if (selectedCameraID == "")
			{
				text = "SELECT * FROM t_b_Camera WHERE f_Enabled = 1 order by f_CameraName ";
			}
			else
			{
				text = "SELECT t_b_Camera.* FROM t_b_Camera WHERE t_b_Camera.f_Enabled = 1 ";
				text = text + " And t_b_Camera.f_CameraId IN(" + selectedCameraID + ") ORDER by f_CameraName ";
			}
			try
			{
				if (dbConnection.State != ConnectionState.Open)
				{
					dbConnection.Open();
				}
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					if (this.garrCameraName.IndexOf(dbDataReader["f_CameraName"]) < 0)
					{
						if (this.p4players.Count < 0 || this.p4playersUsedIndex == this.p4players.Count - 1)
						{
							break;
						}
						this.panelErr[this.p4playersUsedIndex + 1] = false;
						comHIKVideo.HIKCamera hikcamera = new comHIKVideo.HIKCamera();
						hikcamera.ipAddr = (string)dbDataReader["f_CameraIp"];
						hikcamera.port = (int)dbDataReader["f_CameraPort"];
						hikcamera.channel = (int)dbDataReader["f_CameraChannel"] % 100000;
						hikcamera.userName = wgTools.SetObjToStr(dbDataReader["f_CameraUser"]);
						hikcamera.password = wgTools.SetObjToStr(dbDataReader["f_CameraPassword"]);
						hikcamera.cameraName = (string)dbDataReader["f_CameraName"];
						hikcamera.iPlayWnd = (IntPtr)this.p4players[this.p4playersUsedIndex + 1];
						this.panelToolTips[this.p4playersUsedIndex + 1] = hikcamera.cameraName;
						int num = -1;
						int num2 = this.newCameraUser(hikcamera, ref num);
						if (num2 >= 0)
						{
							hikcamera.iUserId = num2;
							hikcamera.iPlayHandle = num;
							this.garrCameraName.Add(hikcamera.cameraName);
							this.garrCameraID.Add(dbDataReader["f_CameraId"]);
							this.garrCamera.Add(hikcamera);
						}
						else
						{
							try
							{
								this.panelErr[this.p4playersUsedIndex + 1] = true;
							}
							catch (Exception ex)
							{
								wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
							}
						}
						this.p4playersUsedIndex++;
					}
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(text + "\r\n" + ex2.ToString(), new object[] { EventLogEntryType.Error });
			}
			finally
			{
				if (dbConnection.State != ConnectionState.Closed)
				{
					dbConnection.Close();
				}
			}
			this.bAllowTimer = true;
			return true;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x000DC88C File Offset: 0x000DB88C
		private void loadReaderIDWithCamera()
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
			string text = " SELECT t_b_CameraTriggerSource.f_Id, t_b_CameraTriggerSource.f_CameraId, t_b_CameraTriggerSource.f_ReaderID, t_b_Camera.f_CameraName";
			text += " FROM t_b_CameraTriggerSource INNER JOIN t_b_Camera ON t_b_CameraTriggerSource.f_CameraId = t_b_Camera.f_CameraId ";
			try
			{
				if (dbConnection.State != ConnectionState.Open)
				{
					dbConnection.Open();
				}
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				this.arrReaderIDWithCamera.Clear();
				this.arrCameraName4ReaderIDWithCamera.Clear();
				while (dbDataReader.Read())
				{
					if (this.arrReaderIDWithCamera.IndexOf(dbDataReader["f_ReaderID"]) < 0)
					{
						this.arrReaderIDWithCamera.Add(dbDataReader["f_ReaderID"]);
						this.arrCameraName4ReaderIDWithCamera.Add(dbDataReader["f_CameraName"]);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(text + "\r\n" + ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			finally
			{
				if (dbConnection.State != ConnectionState.Closed)
				{
					dbConnection.Close();
				}
			}
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x000DC9D0 File Offset: 0x000DB9D0
		private int newCameraUser(comHIKVideo.HIKCamera hikCam, ref int iPlayhandle)
		{
			int num = -1;
			int num2 = -1;
			try
			{
				string text = this.validAddr(hikCam.ipAddr);
				if (text != "")
				{
					CHCNetSDK.NET_DVR_Init();
					int i = 0;
					while (i <= 1)
					{
						string text2 = text;
						short num3 = (short)hikCam.port;
						string userName = hikCam.userName;
						string password = hikCam.password;
						num2 = CHCNetSDK.NET_DVR_Login_V30(text2, (int)num3, userName, password, ref this.hikNET_DVR_DEVICEINFO);
						if (num2 < 0)
						{
							long num4 = (long)int.Parse(CHCNetSDK.NET_DVR_GetLastError().ToString());
							if (num4 == 1L)
							{
								wgTools.WgDebugWrite(hikCam.cameraName + "__UsernameOrPwdError!", new object[] { EventLogEntryType.Error });
								wgAppRunInfo.raiseAppRunInfoCommStatus(hikCam.cameraName + CommonStr.strNET_DVR_PASSWORD_ERROR);
								XMessageBox.Show(hikCam.cameraName + CommonStr.strNET_DVR_PASSWORD_ERROR);
								return num;
							}
							wgAppRunInfo.raiseAppRunInfoCommStatus(hikCam.cameraName + CommonStr.strNET_BUSY_REGISTER_FAILED);
							comHIKVideo.videoErrInfo = comHIKVideo.videoErrInfo + "\r\n" + hikCam.cameraName + CommonStr.strDVR_CONNECT_ERROR;
							return num;
						}
						else
						{
							if (num2 >= 0)
							{
								break;
							}
							i++;
						}
					}
					if (num2 >= 0)
					{
						if (this.AutoAdjustTimeOfVideo != 0)
						{
							CHCNetSDK.NET_DVR_TIME net_DVR_TIME = default(CHCNetSDK.NET_DVR_TIME);
							DateTime now = DateTime.Now;
							net_DVR_TIME.dwYear = uint.Parse(now.Year.ToString());
							net_DVR_TIME.dwMonth = uint.Parse(now.Month.ToString());
							net_DVR_TIME.dwDay = uint.Parse(now.Day.ToString());
							net_DVR_TIME.dwHour = uint.Parse(now.Hour.ToString());
							net_DVR_TIME.dwMinute = uint.Parse(now.Minute.ToString());
							net_DVR_TIME.dwSecond = uint.Parse(now.Second.ToString());
							uint num5 = (uint)Marshal.SizeOf(net_DVR_TIME);
							IntPtr intPtr = Marshal.AllocHGlobal((int)num5);
							Marshal.StructureToPtr(net_DVR_TIME, intPtr, false);
							CHCNetSDK.NET_DVR_SetDVRConfig(num2, 119U, 0, intPtr, num5);
							Marshal.FreeHGlobal(intPtr);
						}
						if (this.onlyCapturePhoto >= 2)
						{
							return num2;
						}
						CHCNetSDK.NET_DVR_CLIENTINFO net_DVR_CLIENTINFO = default(CHCNetSDK.NET_DVR_CLIENTINFO);
						net_DVR_CLIENTINFO.hPlayWnd = hikCam.iPlayWnd;
						net_DVR_CLIENTINFO.lChannel = hikCam.channel;
						net_DVR_CLIENTINFO.lLinkMode = 0;
						net_DVR_CLIENTINFO.sMultiCastIP = "";
						iPlayhandle = CHCNetSDK.NET_DVR_RealPlay_V30(num2, ref net_DVR_CLIENTINFO, null, 0, 1U);
						if (iPlayhandle == -1 && net_DVR_CLIENTINFO.lChannel <= 32 && wgAppConfig.getValBySql("SELECT  f_CameraChannel FROM   t_b_Camera  WHERE f_CameraChannel>32 AND f_CameraIP= " + wgTools.PrepareStrNUnicode(hikCam.ipAddr)) <= 0)
						{
							net_DVR_CLIENTINFO.lChannel = hikCam.channel + 32;
							iPlayhandle = CHCNetSDK.NET_DVR_RealPlay_V30(num2, ref net_DVR_CLIENTINFO, null, 0, 1U);
							if (iPlayhandle >= 0)
							{
								hikCam.channel += 32;
							}
							else
							{
								net_DVR_CLIENTINFO.lChannel = hikCam.channel;
							}
						}
						if (iPlayhandle == -1)
						{
							wgTools.WgDebugWrite(string.Format("访问IP={0}: 通道{1}, 连接启动失败 dwErr=", hikCam.ipAddr, hikCam.channel) + CHCNetSDK.NET_DVR_GetLastError().ToString(), new object[] { EventLogEntryType.Error });
							wgAppRunInfo.raiseAppRunInfoCommStatus(hikCam.cameraName + CommonStr.strNET_BUSY_REGISTER_FAILED);
							comHIKVideo.videoErrInfo = comHIKVideo.videoErrInfo + "\r\n" + hikCam.cameraName + CommonStr.strDVR_CONNECT_ERROR;
							return num;
						}
						int num6 = 0;
						CHCNetSDK.NET_DVR_SetPlayerBufNumber(iPlayhandle, uint.Parse(num6.ToString()));
						int num7 = 0;
						CHCNetSDK.NET_DVR_ThrowBFrame(iPlayhandle, uint.Parse(num7.ToString()));
					}
				}
				return num;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
			finally
			{
				num = num2;
			}
			return num;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x000DCE08 File Offset: 0x000DBE08
		public object p4playersAppend(Panel PlayPanel)
		{
			bool flag = false;
			try
			{
				if (this.p4players.IndexOf(PlayPanel.Handle) < 0)
				{
					this.p4players.Add(PlayPanel.Handle);
					this.p4PlayPanel.Add(PlayPanel);
					flag = true;
				}
			}
			catch (Exception)
			{
			}
			return flag;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x000DCE74 File Offset: 0x000DBE74
		public bool pingCamera(string ip)
		{
			byte[] array = new byte[6];
			uint num = (uint)array.Length;
			try
			{
				if (wgGlobal.SafeNativeMethods.SendARP(this.IPLng(IPAddress.Parse(ip)), 0, array, ref num) == 0)
				{
					return true;
				}
				Ping ping = new Ping();
				PingOptions pingOptions = new PingOptions();
				pingOptions.DontFragment = true;
				string text = "12345678901234567890123456789012";
				byte[] bytes = Encoding.ASCII.GetBytes(text);
				int num2 = 120;
				return ping.Send(ip, num2, bytes, pingOptions).Status == IPStatus.Success;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return false;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x000DCF18 File Offset: 0x000DBF18
		public void RealDataCallBack(int lRealHandle, uint dwDataType, ref byte pBuffer, uint dwBufSize, IntPtr pUser)
		{
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x000DCF1C File Offset: 0x000DBF1C
		public bool startRecord(comHIKVideo.HIKCamera hikcam)
		{
			try
			{
				bool flag = true;
				if (!wgAppConfig.FileIsExisted(hikcam.fileToSave.Replace("avi", "bmp")))
				{
					if (this.delaySecondCapture > 0)
					{
						hikcam.dtStartCapture = DateTime.Now.AddSeconds((double)this.delaySecondCapture);
						hikcam.CaptureFileName = hikcam.fileToSave.Replace("avi", "bmp");
						flag = true;
					}
					else
					{
						flag = this.capturePhoto(hikcam);
					}
				}
				if (this.onlyCapturePhoto > 0 || !flag || wgAppConfig.FileIsExisted(hikcam.fileToSave.Replace("avi", "MP4")))
				{
					return flag;
				}
				hikcam.dtStopRec = DateTime.Now.AddSeconds((double)(this.aviTimelength / 1000));
				CHCNetSDK.NET_DVR_MakeKeyFrame(hikcam.iUserId, hikcam.channel);
				if (CHCNetSDK.NET_DVR_SaveRealData(hikcam.iPlayHandle, hikcam.fileToSave.Replace("avi", "MP4")))
				{
					hikcam.bSaving = true;
					return flag;
				}
				wgAppRunInfo.raiseAppRunInfoCommStatus(CommonStr.strRECFailed + "  " + hikcam.cameraName);
				return false;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			return true;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x000DD080 File Offset: 0x000DC080
		public void stopAllCamera()
		{
			try
			{
				CHCNetSDK.NET_DVR_StopListen();
				if (this.garrCamera.Count > 0)
				{
					foreach (object obj in this.garrCamera)
					{
						comHIKVideo.HIKCamera hikcamera = (comHIKVideo.HIKCamera)obj;
						if (hikcamera.bSaving)
						{
							this.stopRecord(hikcamera);
						}
						if (hikcamera.iPlayHandle >= 0)
						{
							if (CHCNetSDK.NET_DVR_StopRealPlay(hikcamera.iPlayHandle))
							{
								hikcamera.iPlayHandle = -1;
								if (!CHCNetSDK.NET_DVR_Logout(hikcamera.iUserId))
								{
									wgTools.WgDebugWrite(hikcamera.cameraName + " NET_DVR_Logout Failed.", new object[] { EventLogEntryType.Error });
								}
							}
							else
							{
								wgTools.WgDebugWrite(hikcamera.cameraName + "  Stop Real Play Failed.", new object[] { EventLogEntryType.Error });
							}
						}
					}
				}
				CHCNetSDK.NET_DVR_Cleanup();
				this.garrCamera.Clear();
				if (this.threadStopRecord != null)
				{
					this.threadStopRecord.Abort();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000DD1C8 File Offset: 0x000DC1C8
		private bool stopRecord(comHIKVideo.HIKCamera hikcam)
		{
			bool flag = false;
			try
			{
				flag = CHCNetSDK.NET_DVR_StopSaveRealData(hikcam.iPlayHandle);
				if (!flag)
				{
					wgAppRunInfo.raiseAppRunInfoCommStatus(CommonStr.strStopRECFailed + "  " + hikcam.cameraName);
					if (CHCNetSDK.NET_DVR_GetLastError() == 12U)
					{
						hikcam.bSaving = false;
						hikcam.fileToSave = "";
					}
					return flag;
				}
				hikcam.bSaving = false;
				hikcam.fileToSave = "";
			}
			catch (Exception)
			{
			}
			return flag;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000DD24C File Offset: 0x000DC24C
		private void ThreadMainWithParameters(object obj)
		{
			comHIKVideo.HIKCamera hikcamera = obj as comHIKVideo.HIKCamera;
			if (hikcamera.bSaving)
			{
				this.stopRecord(hikcamera);
			}
			hikcamera.fileToSave = wgAppConfig.Path4AviJpg() + hikcamera.RecordAll4Thread + ".avi";
			this.startRecord(hikcamera);
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x000DD294 File Offset: 0x000DC294
		private void timer50ms()
		{
			try
			{
				for (;;)
				{
					Thread.Sleep(50);
					if (this.bAllowTimer)
					{
						try
						{
							DateTime now = DateTime.Now;
							if (this.garrCamera.Count > 0)
							{
								foreach (object obj in this.garrCamera)
								{
									comHIKVideo.HIKCamera hikcamera = (comHIKVideo.HIKCamera)obj;
									if (hikcamera.bSaving && (now > hikcamera.dtStopRec || now.AddSeconds(60.0) < hikcamera.dtStopRec))
									{
										this.stopRecord(hikcamera);
									}
									if (!string.IsNullOrEmpty(hikcamera.CaptureFileName) && now >= hikcamera.dtStartCapture)
									{
										this.capturePhotoWithFileName(hikcamera, hikcamera.CaptureFileName);
										hikcamera.CaptureFileName = "";
									}
								}
							}
						}
						catch (Exception)
						{
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x000DD3A4 File Offset: 0x000DC3A4
		public string validAddr(string parIpAddr)
		{
			try
			{
				bool flag = true;
				for (int i = 0; i <= parIpAddr.Length - 1; i++)
				{
					string text = parIpAddr.Substring(i, 1);
					if (!Information.IsNumeric(text) && !(text == "."))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return parIpAddr;
				}
				IPHostEntry hostEntry = Dns.GetHostEntry(parIpAddr);
				IPAddress[] addressList = hostEntry.AddressList;
				string[] aliases = hostEntry.Aliases;
				if (addressList.Length > 0)
				{
					return addressList[0].ToString();
				}
				return "";
			}
			catch (Exception)
			{
			}
			return "";
		}

		// Token: 0x04001821 RID: 6177
		private ArrayList arrCameraName4ReaderIDWithCamera;

		// Token: 0x04001822 RID: 6178
		private ArrayList arrCardRecordID;

		// Token: 0x04001823 RID: 6179
		private ArrayList arrReaderIDWithCamera;

		// Token: 0x04001824 RID: 6180
		private ArrayList arrWatchingRecordListIndex;

		// Token: 0x04001825 RID: 6181
		private int AutoAdjustTimeOfVideo;

		// Token: 0x04001826 RID: 6182
		public int aviTimelength;

		// Token: 0x04001827 RID: 6183
		private bool bAllowTimer;

		// Token: 0x04001828 RID: 6184
		private static bool bFailedNET_DVR_CaptureJPEGPicture = false;

		// Token: 0x04001829 RID: 6185
		private Container components;

		// Token: 0x0400182A RID: 6186
		private int delaySecondCapture;

		// Token: 0x0400182B RID: 6187
		private ArrayList garrCamera;

		// Token: 0x0400182C RID: 6188
		private ArrayList garrCameraID;

		// Token: 0x0400182D RID: 6189
		private ArrayList garrCameraName;

		// Token: 0x0400182E RID: 6190
		public CHCNetSDK.NET_DVR_DEVICEINFO_V30 hikNET_DVR_DEVICEINFO;

		// Token: 0x0400182F RID: 6191
		private CHCNetSDK hikVideo;

		// Token: 0x04001830 RID: 6192
		private int JPEGCaptureDisable;

		// Token: 0x04001831 RID: 6193
		private int JPEGQuality;

		// Token: 0x04001832 RID: 6194
		private int JPEGResolution;

		// Token: 0x04001833 RID: 6195
		private int onlyCapturePhoto;

		// Token: 0x04001834 RID: 6196
		private ArrayList p4players;

		// Token: 0x04001835 RID: 6197
		private int p4playersUsedIndex;

		// Token: 0x04001836 RID: 6198
		private ArrayList p4PlayPanel;

		// Token: 0x04001837 RID: 6199
		public bool[] panelErr;

		// Token: 0x04001838 RID: 6200
		public string[] panelErrInfo;

		// Token: 0x04001839 RID: 6201
		public string[] panelToolTips;

		// Token: 0x0400183A RID: 6202
		private Thread threadStopRecord;

		// Token: 0x0400183B RID: 6203
		public static string videoErrInfo = "";

		// Token: 0x020001C1 RID: 449
		public class HIKCamera
		{
			// Token: 0x0400183C RID: 6204
			public bool bSaving;

			// Token: 0x0400183D RID: 6205
			public string cameraName;

			// Token: 0x0400183E RID: 6206
			public string CaptureFileName = "";

			// Token: 0x0400183F RID: 6207
			public int channel = 1;

			// Token: 0x04001840 RID: 6208
			public DateTime dtStartCapture;

			// Token: 0x04001841 RID: 6209
			public DateTime dtStopRec;

			// Token: 0x04001842 RID: 6210
			public string fileToSave = "";

			// Token: 0x04001843 RID: 6211
			public string ipAddr = "192.168.168.123";

			// Token: 0x04001844 RID: 6212
			public int iPlayHandle = -1;

			// Token: 0x04001845 RID: 6213
			public IntPtr iPlayWnd;

			// Token: 0x04001846 RID: 6214
			public int iUserId = -1;

			// Token: 0x04001847 RID: 6215
			public string password = "123";

			// Token: 0x04001848 RID: 6216
			public int port = 8000;

			// Token: 0x04001849 RID: 6217
			public string RecordAll4Thread;

			// Token: 0x0400184A RID: 6218
			public string userName = "abc";
		}
	}
}
