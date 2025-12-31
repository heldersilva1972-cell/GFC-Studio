using System;
using System.Runtime.InteropServices;

namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x02000256 RID: 598
	public class CHCNetSDK
	{
		// Token: 0x060012DD RID: 4829
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CaptureJPEGPicture(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara, IntPtr sPicFileName);

		// Token: 0x060012DE RID: 4830
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_Cleanup();

		// Token: 0x060012DF RID: 4831
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CloseAlarmChan(int lAlarmHandle);

		// Token: 0x060012E0 RID: 4832
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CloseAlarmChan_V30(int lAlarmHandle);

		// Token: 0x060012E1 RID: 4833
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_CloseUpgradeHandle(int lUpgradeHandle);

		// Token: 0x060012E2 RID: 4834
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ControlGateway(int lUserID, int lGatewayIndex, uint dwStaic);

		// Token: 0x060012E3 RID: 4835
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetAlarmDeviceUser(int lUserID, int lUserIndex, ref CHCNetSDK.NET_DVR_ALARM_DEVICE_USER lpDeviceUser);

		// Token: 0x060012E4 RID: 4836
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDeviceAbility(int lUserID, uint dwAbilityType, IntPtr pInBuf, uint dwInLength, IntPtr pOutBuf, uint dwOutLength);

		// Token: 0x060012E5 RID: 4837
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDeviceConfig(int lUserID, uint dwCommand, uint dwCount, IntPtr lpInBuffer, uint dwInBufferSize, IntPtr lpStatusList, IntPtr lpOutBuffer, uint dwOutBufferSize);

		// Token: 0x060012E6 RID: 4838
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDVRConfig(int lUserID, uint dwCommand, int lChannel, IntPtr lpOutBuffer, uint dwOutBufferSize, ref uint lpBytesReturned);

		// Token: 0x060012E7 RID: 4839
		[DllImport("HCNetSDK.dll")]
		public static extern IntPtr NET_DVR_GetErrorMsg(ref int pErrorNo);

		// Token: 0x060012E8 RID: 4840
		[DllImport("HCNetSDK.dll")]
		public static extern uint NET_DVR_GetLastError();

		// Token: 0x060012E9 RID: 4841
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetNextRemoteConfig(int lHandle, IntPtr lpOutBuff, uint dwOutBuffSize);

		// Token: 0x060012EA RID: 4842
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetRemoteConfigState(int lHandle, IntPtr pState);

		// Token: 0x060012EB RID: 4843
		[DllImport("HCNetSDK.dll")]
		public static extern uint NET_DVR_GetSDKBuildVersion();

		// Token: 0x060012EC RID: 4844
		[DllImport("HCNetSDK.dll")]
		public static extern uint NET_DVR_GetSDKVersion();

		// Token: 0x060012ED RID: 4845
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetUpgradeProgress(int lUpgradeHandle);

		// Token: 0x060012EE RID: 4846
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_Init();

		// Token: 0x060012EF RID: 4847
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_Login_V40(ref CHCNetSDK.NET_DVR_USER_LOGIN_INFO pLoginInfo, ref CHCNetSDK.NET_DVR_DEVICEINFO_V40 lpDeviceInfo);

		// Token: 0x060012F0 RID: 4848
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_Logout_V30(int lUserID);

		// Token: 0x060012F1 RID: 4849
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_RealPlay_V30(int iUserID, ref CHCNetSDK.NET_DVR_CLIENTINFO lpClientInfo, CHCNetSDK.REALDATACALLBACK fRealDataCallBack_V30, IntPtr pUser, uint bBlocked);

		// Token: 0x060012F2 RID: 4850
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_RealPlay_V40(int lUserID, ref CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo, CHCNetSDK.RealDataCallBack fRealDataCallBack_V30, IntPtr pUser);

		// Token: 0x060012F3 RID: 4851
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_RemoteControl(int lUserID, uint dwCommand, IntPtr lpInBuffer, uint dwInBufferSize);

		// Token: 0x060012F4 RID: 4852
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SendRemoteConfig(int lHandle, uint dwDataType, IntPtr pSendBuf, uint dwBufSize);

		// Token: 0x060012F5 RID: 4853
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetAlarmDeviceUser(int lUserID, int lUserIndex, ref CHCNetSDK.NET_DVR_ALARM_DEVICE_USER lpDeviceUser);

		// Token: 0x060012F6 RID: 4854
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDeviceConfig(int lUserID, uint dwCommand, uint dwCount, IntPtr lpInBuffer, uint dwInBufferSize, IntPtr lpStatusList, IntPtr lpInParamBuffer, uint dwInParamBufferSize);

		// Token: 0x060012F7 RID: 4855
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDVRConfig(int lUserID, uint dwCommand, int lChannel, IntPtr lpInBuffer, uint dwInBufferSize);

		// Token: 0x060012F8 RID: 4856
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDVRMessageCallBack_V31(CHCNetSDK.MSGCallBack_V31 fMessageCallBack, IntPtr pUser);

		// Token: 0x060012F9 RID: 4857
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDVRMessageCallBack_V50(int iIndex, CHCNetSDK.MSGCallBack fMessageCallBack, IntPtr pUser);

		// Token: 0x060012FA RID: 4858
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetLogToFile(int nLogLevel, string strLogDir, bool bAutoDel);

		// Token: 0x060012FB RID: 4859
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_SetupAlarmChan(int lUserID);

		// Token: 0x060012FC RID: 4860
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_SetupAlarmChan_V30(int lUserID);

		// Token: 0x060012FD RID: 4861
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_SetupAlarmChan_V41(int lUserID, ref CHCNetSDK.NET_DVR_SETUPALARM_PARAM lpSetupParam);

		// Token: 0x060012FE RID: 4862
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetVoiceComClientVolume(int lVoiceComHandle, ushort wVolume);

		// Token: 0x060012FF RID: 4863
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_StartRemoteConfig(int lUserID, uint dwCommand, IntPtr lpInBuffer, int dwInBufferLen, CHCNetSDK.RemoteConfigCallback cbStateCallback, IntPtr pUserData);

		// Token: 0x06001300 RID: 4864
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_StartVoiceCom_V30(int lUserID, uint dwVoiceChan, bool bNeedCBNoEncData, CHCNetSDK.VOICEDATACALLBACKV30 fVoiceDataCallBack, IntPtr pUser);

		// Token: 0x06001301 RID: 4865
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_STDXMLConfig(int lUserID, IntPtr lpInputParam, IntPtr lpOutputParam);

		// Token: 0x06001302 RID: 4866
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopRealPlay(int iRealHandle);

		// Token: 0x06001303 RID: 4867
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopRemoteConfig(int lHandle);

		// Token: 0x06001304 RID: 4868
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopVoiceCom(int lVoiceComHandle);

		// Token: 0x06001305 RID: 4869
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_Upgrade_V40(int lUserID, uint dwUpgradeType, string sFileName, IntPtr pInbuffer, int dwInBufferLen);

		// Token: 0x06001306 RID: 4870
		[DllImport("User32.dll")]
		public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		// Token: 0x040022FC RID: 8956
		public const int ACS_ABILITY = 2049;

		// Token: 0x040022FD RID: 8957
		public const int ACS_CARD_NO_LEN = 32;

		// Token: 0x040022FE RID: 8958
		public const int CARD_PARAM_CARD_TYPE = 4;

		// Token: 0x040022FF RID: 8959
		public const int CARD_PARAM_CARD_VALID = 1;

		// Token: 0x04002300 RID: 8960
		public const int CARD_PARAM_DOOR_RIGHT = 8;

		// Token: 0x04002301 RID: 8961
		public const int CARD_PARAM_EMPLOYEE_NO = 1024;

		// Token: 0x04002302 RID: 8962
		public const int CARD_PARAM_GROUP = 64;

		// Token: 0x04002303 RID: 8963
		public const int CARD_PARAM_LEADER_CARD = 16;

		// Token: 0x04002304 RID: 8964
		public const int CARD_PARAM_PASSWORD = 128;

		// Token: 0x04002305 RID: 8965
		public const int CARD_PARAM_RIGHT_PLAN = 256;

		// Token: 0x04002306 RID: 8966
		public const int CARD_PARAM_SWIPE_NUM = 32;

		// Token: 0x04002307 RID: 8967
		public const int CARD_PARAM_SWIPED_NUM = 512;

		// Token: 0x04002308 RID: 8968
		public const int CARD_PARAM_VALID = 2;

		// Token: 0x04002309 RID: 8969
		public const int CARD_PASSWORD_LEN = 8;

		// Token: 0x0400230A RID: 8970
		public const int CARD_READER_DESCRIPTION = 32;

		// Token: 0x0400230B RID: 8971
		public const int COMM_ALARM_ACS = 20482;

		// Token: 0x0400230C RID: 8972
		public const int DEV_ID_LEN = 32;

		// Token: 0x0400230D RID: 8973
		public const int DEV_TYPE_NAME_LEN = 24;

		// Token: 0x0400230E RID: 8974
		public const int DOOR_NAME_LEN = 32;

		// Token: 0x0400230F RID: 8975
		public const int ERROR_MSG_LEN = 32;

		// Token: 0x04002310 RID: 8976
		public const int GROUP_NAME_LEN = 32;

		// Token: 0x04002311 RID: 8977
		public const int HOLIDAY_GROUP_NAME_LEN = 32;

		// Token: 0x04002312 RID: 8978
		public const int LOCAL_CONTROLLER_NAME_LEN = 32;

		// Token: 0x04002313 RID: 8979
		public const int LOG_INFO_LEN = 11840;

		// Token: 0x04002314 RID: 8980
		public const int MACADDR_LEN = 6;

		// Token: 0x04002315 RID: 8981
		public const int MAJOR_ALARM = 1;

		// Token: 0x04002316 RID: 8982
		public const int MAJOR_EVENT = 5;

		// Token: 0x04002317 RID: 8983
		public const int MAJOR_EXCEPTION = 2;

		// Token: 0x04002318 RID: 8984
		public const int MAJOR_OPERATION = 3;

		// Token: 0x04002319 RID: 8985
		public const int MAX_ALARMHOST_ALARMIN_NUM = 512;

		// Token: 0x0400231A RID: 8986
		public const int MAX_ALARMHOST_ALARMOUT_NUM = 512;

		// Token: 0x0400231B RID: 8987
		public const int MAX_ALARMHOST_VIDEO_CHAN = 64;

		// Token: 0x0400231C RID: 8988
		public const int MAX_ANALOG_CHANNUM = 32;

		// Token: 0x0400231D RID: 8989
		public const int MAX_AUDIO_V40 = 8;

		// Token: 0x0400231E RID: 8990
		public const int MAX_AUDIOOUT_PRO_TYPE = 8;

		// Token: 0x0400231F RID: 8991
		public const int MAX_CARD_READER_NUM_512 = 512;

		// Token: 0x04002320 RID: 8992
		public const int MAX_CARD_RIGHT_PLAN_NUM = 4;

		// Token: 0x04002321 RID: 8993
		public const int MAX_CASE_SENSOR_NUM = 8;

		// Token: 0x04002322 RID: 8994
		public const int MAX_CHANNUM_V30 = 64;

		// Token: 0x04002323 RID: 8995
		public const int MAX_CHANNUM_V40 = 512;

		// Token: 0x04002324 RID: 8996
		public const int MAX_DAYS = 7;

		// Token: 0x04002325 RID: 8997
		public const int MAX_DOMAIN_NAME = 64;

		// Token: 0x04002326 RID: 8998
		public const int MAX_DOOR_CODE_LEN = 8;

		// Token: 0x04002327 RID: 8999
		public const int MAX_DOOR_NUM = 32;

		// Token: 0x04002328 RID: 9000
		public const int MAX_DOOR_NUM_256 = 256;

		// Token: 0x04002329 RID: 9001
		public const int MAX_ETHERNET = 2;

		// Token: 0x0400232A RID: 9002
		public const int MAX_FACE_NUM = 2;

		// Token: 0x0400232B RID: 9003
		public const int MAX_FINGER_PRINT_LEN = 768;

		// Token: 0x0400232C RID: 9004
		public const int MAX_FINGER_PRINT_NUM = 10;

		// Token: 0x0400232D RID: 9005
		public const int MAX_GROUP_NUM = 32;

		// Token: 0x0400232E RID: 9006
		public const int MAX_GROUP_NUM_128 = 128;

		// Token: 0x0400232F RID: 9007
		public const int MAX_HOLIDAY_GROUP_NUM = 16;

		// Token: 0x04002330 RID: 9008
		public const int MAX_HOLIDAY_PLAN_NUM = 16;

		// Token: 0x04002331 RID: 9009
		public const int MAX_IP_ALARMIN = 128;

		// Token: 0x04002332 RID: 9010
		public const int MAX_IP_ALARMIN_V40 = 4096;

		// Token: 0x04002333 RID: 9011
		public const int MAX_IP_ALARMOUT = 64;

		// Token: 0x04002334 RID: 9012
		public const int MAX_IP_ALARMOUT_V40 = 4096;

		// Token: 0x04002335 RID: 9013
		public const int MAX_IP_CHANNEL = 32;

		// Token: 0x04002336 RID: 9014
		public const int MAX_IP_DEVICE = 32;

		// Token: 0x04002337 RID: 9015
		public const int MAX_IP_DEVICE_V40 = 64;

		// Token: 0x04002338 RID: 9016
		public const int MAX_LOCK_CODE_LEN = 8;

		// Token: 0x04002339 RID: 9017
		public const int MAX_NAMELEN = 16;

		// Token: 0x0400233A RID: 9018
		public const int MAX_RIGHT = 32;

		// Token: 0x0400233B RID: 9019
		public const int MAX_TIMESEGMENT_V30 = 8;

		// Token: 0x0400233C RID: 9020
		public const int MINOR_4G_MOUDLE_OFFLINE = 1081;

		// Token: 0x0400233D RID: 9021
		public const int MINOR_4G_MOUDLE_ONLINE = 1080;

		// Token: 0x0400233E RID: 9022
		public const int MINOR_AC_OFF = 1029;

		// Token: 0x0400233F RID: 9023
		public const int MINOR_AC_RESUME = 1030;

		// Token: 0x04002340 RID: 9024
		public const int MINOR_ALARMIN_ARM = 1032;

		// Token: 0x04002341 RID: 9025
		public const int MINOR_ALARMIN_BROKEN_CIRCUIT = 1025;

		// Token: 0x04002342 RID: 9026
		public const int MINOR_ALARMIN_DISARM = 1033;

		// Token: 0x04002343 RID: 9027
		public const int MINOR_ALARMIN_EXCEPTION = 1026;

		// Token: 0x04002344 RID: 9028
		public const int MINOR_ALARMIN_RESUME = 1027;

		// Token: 0x04002345 RID: 9029
		public const int MINOR_ALARMIN_SHORT_CIRCUIT = 1024;

		// Token: 0x04002346 RID: 9030
		public const int MINOR_ALARMOUT_OFF = 30;

		// Token: 0x04002347 RID: 9031
		public const int MINOR_ALARMOUT_ON = 29;

		// Token: 0x04002348 RID: 9032
		public const int MINOR_ALWAYS_CLOSE_BEGIN = 31;

		// Token: 0x04002349 RID: 9033
		public const int MINOR_ALWAYS_CLOSE_END = 32;

		// Token: 0x0400234A RID: 9034
		public const int MINOR_ALWAYS_OPEN_BEGIN = 19;

		// Token: 0x0400234B RID: 9035
		public const int MINOR_ALWAYS_OPEN_END = 20;

		// Token: 0x0400234C RID: 9036
		public const int MINOR_ANTI_SNEAK_FAIL = 10;

		// Token: 0x0400234D RID: 9037
		public const int MINOR_AUTH_PLAN_DORMANT_FAIL = 118;

		// Token: 0x0400234E RID: 9038
		public const int MINOR_AUTO_COMPLEMENT_NUMBER = 1041;

		// Token: 0x0400234F RID: 9039
		public const int MINOR_AUTO_KEY_RELAY_BREAK = 97;

		// Token: 0x04002350 RID: 9040
		public const int MINOR_AUTO_KEY_RELAY_CLOSE = 98;

		// Token: 0x04002351 RID: 9041
		public const int MINOR_AUTO_RENUMBER = 1040;

		// Token: 0x04002352 RID: 9042
		public const int MINOR_BATTERY_ELECTRIC_LOW = 1041;

		// Token: 0x04002353 RID: 9043
		public const int MINOR_BATTERY_ELECTRIC_RESUME = 1042;

		// Token: 0x04002354 RID: 9044
		public const int MINOR_BATTERY_RESUME = 1028;

		// Token: 0x04002355 RID: 9045
		public const int MINOR_CALL_CENTER = 51;

		// Token: 0x04002356 RID: 9046
		public const int MINOR_CALL_LADDER_RELAY_BREAK = 95;

		// Token: 0x04002357 RID: 9047
		public const int MINOR_CALL_LADDER_RELAY_CLOSE = 96;

		// Token: 0x04002358 RID: 9048
		public const int MINOR_CAMERA_NOT_CONNECT = 1057;

		// Token: 0x04002359 RID: 9049
		public const int MINOR_CAMERA_RESUME = 1058;

		// Token: 0x0400235A RID: 9050
		public const int MINOR_CAN_BUS_EXCEPTION = 1069;

		// Token: 0x0400235B RID: 9051
		public const int MINOR_CAN_BUS_RESUME = 1070;

		// Token: 0x0400235C RID: 9052
		public const int MINOR_CARD_AND_PSW_FAIL = 3;

		// Token: 0x0400235D RID: 9053
		public const int MINOR_CARD_AND_PSW_OVER_TIME = 5;

		// Token: 0x0400235E RID: 9054
		public const int MINOR_CARD_AND_PSW_PASS = 2;

		// Token: 0x0400235F RID: 9055
		public const int MINOR_CARD_AND_PSW_TIMEOUT = 4;

		// Token: 0x04002360 RID: 9056
		public const int MINOR_CARD_ENCRYPT_VERIFY_FAIL = 119;

		// Token: 0x04002361 RID: 9057
		public const int MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_FAIL = 44;

		// Token: 0x04002362 RID: 9058
		public const int MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_PASS = 43;

		// Token: 0x04002363 RID: 9059
		public const int MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT = 45;

		// Token: 0x04002364 RID: 9060
		public const int MINOR_CARD_FINGERPRINT_VERIFY_FAIL = 41;

		// Token: 0x04002365 RID: 9061
		public const int MINOR_CARD_FINGERPRINT_VERIFY_PASS = 40;

		// Token: 0x04002366 RID: 9062
		public const int MINOR_CARD_FINGERPRINT_VERIFY_TIMEOUT = 42;

		// Token: 0x04002367 RID: 9063
		public const int MINOR_CARD_INVALID_PERIOD = 7;

		// Token: 0x04002368 RID: 9064
		public const int MINOR_CARD_MAX_AUTHENTICATE_FAIL = 1036;

		// Token: 0x04002369 RID: 9065
		public const int MINOR_CARD_NO_RIGHT = 6;

		// Token: 0x0400236A RID: 9066
		public const int MINOR_CARD_OUT_OF_DATE = 8;

		// Token: 0x0400236B RID: 9067
		public const int MINOR_CARD_PLATFORM_VERIFY = 50;

		// Token: 0x0400236C RID: 9068
		public const int MINOR_CARD_READER_DESMANTLE_ALARM = 1030;

		// Token: 0x0400236D RID: 9069
		public const int MINOR_CARD_READER_DESMANTLE_RESUME = 1031;

		// Token: 0x0400236E RID: 9070
		public const int MINOR_CARD_READER_OFFLINE = 1033;

		// Token: 0x0400236F RID: 9071
		public const int MINOR_CARD_READER_RESUME = 1034;

		// Token: 0x04002370 RID: 9072
		public const int MINOR_CARD_RIGHT_INPUT = 1044;

		// Token: 0x04002371 RID: 9073
		public const int MINOR_CARD_RIGHT_OUTTPUT = 1045;

		// Token: 0x04002372 RID: 9074
		public const int MINOR_CASE_SENSOR_ALARM = 1032;

		// Token: 0x04002373 RID: 9075
		public const int MINOR_CASE_SENSOR_RESUME = 1033;

		// Token: 0x04002374 RID: 9076
		public const int MINOR_CERTIFICATE_BLACK_LIST = 113;

		// Token: 0x04002375 RID: 9077
		public const int MINOR_CHANNEL_CONTROLLER_DESMANTLE_ALARM = 1058;

		// Token: 0x04002376 RID: 9078
		public const int MINOR_CHANNEL_CONTROLLER_DESMANTLE_RESUME = 1059;

		// Token: 0x04002377 RID: 9079
		public const int MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_ALARM = 1060;

		// Token: 0x04002378 RID: 9080
		public const int MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_RESUME = 1061;

		// Token: 0x04002379 RID: 9081
		public const int MINOR_CHANNEL_CONTROLLER_OFF = 1037;

		// Token: 0x0400237A RID: 9082
		public const int MINOR_CHANNEL_CONTROLLER_RESUME = 1038;

		// Token: 0x0400237B RID: 9083
		public const int MINOR_CLIMBING_OVER_GATE = 136;

		// Token: 0x0400237C RID: 9084
		public const int MINOR_COM_NOT_CONNECT = 1059;

		// Token: 0x0400237D RID: 9085
		public const int MINOR_COM_RESUME = 1060;

		// Token: 0x0400237E RID: 9086
		public const int MINOR_COMBINED_VERIFY_PASS = 153;

		// Token: 0x0400237F RID: 9087
		public const int MINOR_COMBINED_VERIFY_TIMEOUT = 154;

		// Token: 0x04002380 RID: 9088
		public const int MINOR_DEV_POWER_OFF = 1025;

		// Token: 0x04002381 RID: 9089
		public const int MINOR_DEV_POWER_ON = 1024;

		// Token: 0x04002382 RID: 9090
		public const int MINOR_DEVICE_NOT_AUTHORIZE = 1061;

		// Token: 0x04002383 RID: 9091
		public const int MINOR_DISTRACT_CONTROLLER_ALARM = 1054;

		// Token: 0x04002384 RID: 9092
		public const int MINOR_DISTRACT_CONTROLLER_OFFLINE = 1052;

		// Token: 0x04002385 RID: 9093
		public const int MINOR_DISTRACT_CONTROLLER_ONLINE = 1051;

		// Token: 0x04002386 RID: 9094
		public const int MINOR_DISTRACT_CONTROLLER_RESUME = 1055;

		// Token: 0x04002387 RID: 9095
		public const int MINOR_DOOR_BUTTON_PRESS = 23;

		// Token: 0x04002388 RID: 9096
		public const int MINOR_DOOR_BUTTON_RELEASE = 24;

		// Token: 0x04002389 RID: 9097
		public const int MINOR_DOOR_CLOSE_NORMAL = 26;

		// Token: 0x0400238A RID: 9098
		public const int MINOR_DOOR_OPEN_ABNORMAL = 27;

		// Token: 0x0400238B RID: 9099
		public const int MINOR_DOOR_OPEN_NORMAL = 25;

		// Token: 0x0400238C RID: 9100
		public const int MINOR_DOOR_OPEN_OR_DORMANT_FAIL = 117;

		// Token: 0x0400238D RID: 9101
		public const int MINOR_DOOR_OPEN_OR_DORMANT_LINKAGE_OPEN_FAIL = 132;

		// Token: 0x0400238E RID: 9102
		public const int MINOR_DOOR_OPEN_OR_DORMANT_OPEN_FAIL = 130;

		// Token: 0x0400238F RID: 9103
		public const int MINOR_DOOR_OPEN_TIMEOUT = 28;

		// Token: 0x04002390 RID: 9104
		public const int MINOR_DOORBELL_RINGING = 37;

		// Token: 0x04002391 RID: 9105
		public const int MINOR_DOORCONTACT_INPUT_BROKEN_CIRCUIT = 87;

		// Token: 0x04002392 RID: 9106
		public const int MINOR_DOORCONTACT_INPUT_EXCEPTION = 88;

		// Token: 0x04002393 RID: 9107
		public const int MINOR_DOORCONTACT_INPUT_SHORT_CIRCUIT = 86;

		// Token: 0x04002394 RID: 9108
		public const int MINOR_DOORLOCK_INPUT_BROKEN_CIRCUIT = 84;

		// Token: 0x04002395 RID: 9109
		public const int MINOR_DOORLOCK_INPUT_EXCEPTION = 85;

		// Token: 0x04002396 RID: 9110
		public const int MINOR_DOORLOCK_INPUT_SHORT_CIRCUIT = 83;

		// Token: 0x04002397 RID: 9111
		public const int MINOR_DOORLOCK_OPEN_EXCEPTION = 92;

		// Token: 0x04002398 RID: 9112
		public const int MINOR_DOORLOCK_OPEN_TIMEOUT = 93;

		// Token: 0x04002399 RID: 9113
		public const int MINOR_DROP_ARM_BLOCK = 140;

		// Token: 0x0400239A RID: 9114
		public const int MINOR_DROP_ARM_BLOCK_RESUME = 141;

		// Token: 0x0400239B RID: 9115
		public const int MINOR_EMERGENCY_BUTTON_RESUME = 1053;

		// Token: 0x0400239C RID: 9116
		public const int MINOR_EMERGENCY_BUTTON_TRIGGER = 1052;

		// Token: 0x0400239D RID: 9117
		public const int MINOR_EMPLOYEE_NO_NOT_EXIST = 152;

		// Token: 0x0400239E RID: 9118
		public const int MINOR_EMPLOYEENO_AND_FACE_VERIFY_FAIL = 78;

		// Token: 0x0400239F RID: 9119
		public const int MINOR_EMPLOYEENO_AND_FACE_VERIFY_PASS = 77;

		// Token: 0x040023A0 RID: 9120
		public const int MINOR_EMPLOYEENO_AND_FACE_VERIFY_TIMEOUT = 79;

		// Token: 0x040023A1 RID: 9121
		public const int MINOR_EMPLOYEENO_AND_FP_AND_PW_VERIFY_FAIL = 73;

		// Token: 0x040023A2 RID: 9122
		public const int MINOR_EMPLOYEENO_AND_FP_AND_PW_VERIFY_PASS = 72;

		// Token: 0x040023A3 RID: 9123
		public const int MINOR_EMPLOYEENO_AND_FP_AND_PW_VERIFY_TIMEOUT = 74;

		// Token: 0x040023A4 RID: 9124
		public const int MINOR_EMPLOYEENO_AND_FP_VERIFY_FAIL = 70;

		// Token: 0x040023A5 RID: 9125
		public const int MINOR_EMPLOYEENO_AND_FP_VERIFY_PASS = 69;

		// Token: 0x040023A6 RID: 9126
		public const int MINOR_EMPLOYEENO_AND_FP_VERIFY_TIMEOUT = 71;

		// Token: 0x040023A7 RID: 9127
		public const int MINOR_EMPLOYEENO_AND_PW_FAIL = 102;

		// Token: 0x040023A8 RID: 9128
		public const int MINOR_EMPLOYEENO_AND_PW_PASS = 101;

		// Token: 0x040023A9 RID: 9129
		public const int MINOR_EMPLOYEENO_AND_PW_TIMEOUT = 103;

		// Token: 0x040023AA RID: 9130
		public const int MINOR_FACE_AND_CARD_VERIFY_FAIL = 61;

		// Token: 0x040023AB RID: 9131
		public const int MINOR_FACE_AND_CARD_VERIFY_PASS = 60;

		// Token: 0x040023AC RID: 9132
		public const int MINOR_FACE_AND_CARD_VERIFY_TIMEOUT = 62;

		// Token: 0x040023AD RID: 9133
		public const int MINOR_FACE_AND_FP_VERIFY_FAIL = 55;

		// Token: 0x040023AE RID: 9134
		public const int MINOR_FACE_AND_FP_VERIFY_PASS = 54;

		// Token: 0x040023AF RID: 9135
		public const int MINOR_FACE_AND_FP_VERIFY_TIMEOUT = 56;

		// Token: 0x040023B0 RID: 9136
		public const int MINOR_FACE_AND_PW_AND_FP_VERIFY_FAIL = 64;

		// Token: 0x040023B1 RID: 9137
		public const int MINOR_FACE_AND_PW_AND_FP_VERIFY_PASS = 63;

		// Token: 0x040023B2 RID: 9138
		public const int MINOR_FACE_AND_PW_AND_FP_VERIFY_TIMEOUT = 65;

		// Token: 0x040023B3 RID: 9139
		public const int MINOR_FACE_AND_PW_VERIFY_FAIL = 58;

		// Token: 0x040023B4 RID: 9140
		public const int MINOR_FACE_AND_PW_VERIFY_PASS = 57;

		// Token: 0x040023B5 RID: 9141
		public const int MINOR_FACE_AND_PW_VERIFY_TIMEOUT = 59;

		// Token: 0x040023B6 RID: 9142
		public const int MINOR_FACE_CARD_AND_FP_VERIFY_FAIL = 67;

		// Token: 0x040023B7 RID: 9143
		public const int MINOR_FACE_CARD_AND_FP_VERIFY_PASS = 66;

		// Token: 0x040023B8 RID: 9144
		public const int MINOR_FACE_CARD_AND_FP_VERIFY_TIMEOUT = 68;

		// Token: 0x040023B9 RID: 9145
		public const int MINOR_FACE_IMAGE_QUALITY_LOW = 1043;

		// Token: 0x040023BA RID: 9146
		public const int MINOR_FACE_RECOGNIZE_FAIL = 80;

		// Token: 0x040023BB RID: 9147
		public const int MINOR_FACE_VERIFY_FAIL = 76;

		// Token: 0x040023BC RID: 9148
		public const int MINOR_FACE_VERIFY_PASS = 75;

		// Token: 0x040023BD RID: 9149
		public const int MINOR_FINGE_RPRINT_QUALITY_LOW = 1044;

		// Token: 0x040023BE RID: 9150
		public const int MINOR_FINGER_PRINT_MODULE_NOT_CONNECT = 1055;

		// Token: 0x040023BF RID: 9151
		public const int MINOR_FINGER_PRINT_MODULE_RESUME = 1056;

		// Token: 0x040023C0 RID: 9152
		public const int MINOR_FINGERPRINT_COMPARE_FAIL = 39;

		// Token: 0x040023C1 RID: 9153
		public const int MINOR_FINGERPRINT_COMPARE_PASS = 38;

		// Token: 0x040023C2 RID: 9154
		public const int MINOR_FINGERPRINT_INEXISTENCE = 49;

		// Token: 0x040023C3 RID: 9155
		public const int MINOR_FINGERPRINT_PASSWD_VERIFY_FAIL = 47;

		// Token: 0x040023C4 RID: 9156
		public const int MINOR_FINGERPRINT_PASSWD_VERIFY_PASS = 46;

		// Token: 0x040023C5 RID: 9157
		public const int MINOR_FINGERPRINT_PASSWD_VERIFY_TIMEOUT = 48;

		// Token: 0x040023C6 RID: 9158
		public const int MINOR_FIRE_BUTTON_RESUME = 1049;

		// Token: 0x040023C7 RID: 9159
		public const int MINOR_FIRE_BUTTON_TRIGGER = 1048;

		// Token: 0x040023C8 RID: 9160
		public const int MINOR_FIRE_IMPORT_BROKEN_CIRCUIT = 1046;

		// Token: 0x040023C9 RID: 9161
		public const int MINOR_FIRE_IMPORT_RESUME = 1047;

		// Token: 0x040023CA RID: 9162
		public const int MINOR_FIRE_IMPORT_SHORT_CIRCUIT = 1045;

		// Token: 0x040023CB RID: 9163
		public const int MINOR_FIRE_RELAY_RECOVER_DOOR_RECOVER_NORMAL = 53;

		// Token: 0x040023CC RID: 9164
		public const int MINOR_FIRE_RELAY_TURN_ON_DOOR_ALWAYS_OPEN = 52;

		// Token: 0x040023CD RID: 9165
		public const int MINOR_FIRSTCARD_AUTHORIZE_BEGIN = 81;

		// Token: 0x040023CE RID: 9166
		public const int MINOR_FIRSTCARD_AUTHORIZE_END = 82;

		// Token: 0x040023CF RID: 9167
		public const int MINOR_FIRSTCARD_OPEN_WITHOUT_AUTHORIZE = 94;

		// Token: 0x040023D0 RID: 9168
		public const int MINOR_FLASH_ABNORMAL = 1032;

		// Token: 0x040023D1 RID: 9169
		public const int MINOR_FORCE_ACCESS = 135;

		// Token: 0x040023D2 RID: 9170
		public const int MINOR_FREE_GATE_PASS_NOT_AUTH = 139;

		// Token: 0x040023D3 RID: 9171
		public const int MINOR_GATE_TEMPERATURE_OVERRUN = 1071;

		// Token: 0x040023D4 RID: 9172
		public const int MINOR_HEART_BEAT = 131;

		// Token: 0x040023D5 RID: 9173
		public const int MINOR_HOST_DESMANTLE_ALARM = 1028;

		// Token: 0x040023D6 RID: 9174
		public const int MINOR_HOST_DESMANTLE_RESUME = 1029;

		// Token: 0x040023D7 RID: 9175
		public const int MINOR_HUMAN_DETECT_FAIL = 104;

		// Token: 0x040023D8 RID: 9176
		public const int MINOR_ID_CARD_READER_NOT_CONNECT = 1053;

		// Token: 0x040023D9 RID: 9177
		public const int MINOR_ID_CARD_READER_RESUME = 1054;

		// Token: 0x040023DA RID: 9178
		public const int MINOR_ILLEGAL_MESSAGE = 115;

		// Token: 0x040023DB RID: 9179
		public const int MINOR_INDICATOR_LIGHT_OFF = 1035;

		// Token: 0x040023DC RID: 9180
		public const int MINOR_INDICATOR_LIGHT_RESUME = 1036;

		// Token: 0x040023DD RID: 9181
		public const int MINOR_INTERLOCK_DOOR_NOT_CLOSE = 11;

		// Token: 0x040023DE RID: 9182
		public const int MINOR_INTRUSION_ALARM = 138;

		// Token: 0x040023DF RID: 9183
		public const int MINOR_INVALID_CARD = 9;

		// Token: 0x040023E0 RID: 9184
		public const int MINOR_INVALID_MULTI_VERIFY_PERIOD = 13;

		// Token: 0x040023E1 RID: 9185
		public const int MINOR_IR_ADAPTOR_COMM_EXCEPTION = 1076;

		// Token: 0x040023E2 RID: 9186
		public const int MINOR_IR_ADAPTOR_COMM_RESUME = 1077;

		// Token: 0x040023E3 RID: 9187
		public const int MINOR_IR_EMITTER_EXCEPTION = 1072;

		// Token: 0x040023E4 RID: 9188
		public const int MINOR_IR_EMITTER_RESUME = 1073;

		// Token: 0x040023E5 RID: 9189
		public const int MINOR_KEY_CONTROL_RELAY_BREAK = 99;

		// Token: 0x040023E6 RID: 9190
		public const int MINOR_KEY_CONTROL_RELAY_CLOSE = 100;

		// Token: 0x040023E7 RID: 9191
		public const int MINOR_LAMP_BOARD_COMM_EXCEPTION = 1074;

		// Token: 0x040023E8 RID: 9192
		public const int MINOR_LAMP_BOARD_COMM_RESUME = 1075;

		// Token: 0x040023E9 RID: 9193
		public const int MINOR_LEADER_CARD_OPEN_BEGIN = 17;

		// Token: 0x040023EA RID: 9194
		public const int MINOR_LEADER_CARD_OPEN_END = 18;

		// Token: 0x040023EB RID: 9195
		public const int MINOR_LEGAL_CARD_PASS = 1;

		// Token: 0x040023EC RID: 9196
		public const int MINOR_LEGAL_EVENT_NEARLY_FULL = 1090;

		// Token: 0x040023ED RID: 9197
		public const int MINOR_LEGAL_MESSAGE = 114;

		// Token: 0x040023EE RID: 9198
		public const int MINOR_LINKAGE_CAPTURE_PIC = 1038;

		// Token: 0x040023EF RID: 9199
		public const int MINOR_LOCAL_CONTROL_NET_BROKEN = 1043;

		// Token: 0x040023F0 RID: 9200
		public const int MINOR_LOCAL_CONTROL_NET_RSUME = 1044;

		// Token: 0x040023F1 RID: 9201
		public const int MINOR_LOCAL_CONTROL_OFFLINE = 1047;

		// Token: 0x040023F2 RID: 9202
		public const int MINOR_LOCAL_CONTROL_RESUME = 1048;

		// Token: 0x040023F3 RID: 9203
		public const int MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN = 1049;

		// Token: 0x040023F4 RID: 9204
		public const int MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME = 1050;

		// Token: 0x040023F5 RID: 9205
		public const int MINOR_LOCAL_FACE_MODELING_FAIL = 142;

		// Token: 0x040023F6 RID: 9206
		public const int MINOR_LOCAL_LOGIN_LOCK = 1064;

		// Token: 0x040023F7 RID: 9207
		public const int MINOR_LOCAL_LOGIN_UNLOCK = 1065;

		// Token: 0x040023F8 RID: 9208
		public const int MINOR_LOCAL_RESTORE_CFG = 1034;

		// Token: 0x040023F9 RID: 9209
		public const int MINOR_LOCAL_UPGRADE = 90;

		// Token: 0x040023FA RID: 9210
		public const int MINOR_LOCAL_USB_UPGRADE = 1046;

		// Token: 0x040023FB RID: 9211
		public const int MINOR_LOCK_CLOSE = 22;

		// Token: 0x040023FC RID: 9212
		public const int MINOR_LOCK_OPEN = 21;

		// Token: 0x040023FD RID: 9213
		public const int MINOR_LOW_BATTERY = 1027;

		// Token: 0x040023FE RID: 9214
		public const int MINOR_MAC_DETECT = 116;

		// Token: 0x040023FF RID: 9215
		public const int MINOR_MAINTENANCE_BUTTON_RESUME = 1051;

		// Token: 0x04002400 RID: 9216
		public const int MINOR_MAINTENANCE_BUTTON_TRIGGER = 1050;

		// Token: 0x04002401 RID: 9217
		public const int MINOR_MASTER_RS485_LOOPNODE_BROKEN = 1045;

		// Token: 0x04002402 RID: 9218
		public const int MINOR_MASTER_RS485_LOOPNODE_RESUME = 1046;

		// Token: 0x04002403 RID: 9219
		public const int MINOR_MOD_GPRS_REPORT_PARAM = 1037;

		// Token: 0x04002404 RID: 9220
		public const int MINOR_MOD_NET_REPORT_CFG = 1036;

		// Token: 0x04002405 RID: 9221
		public const int MINOR_MOD_REPORT_GROUP_PARAM = 1038;

		// Token: 0x04002406 RID: 9222
		public const int MINOR_MOTOR_SENSOR_EXCEPTION = 1068;

		// Token: 0x04002407 RID: 9223
		public const int MINOR_MULTI_VERIFY_NEED_REMOTE_OPEN = 33;

		// Token: 0x04002408 RID: 9224
		public const int MINOR_MULTI_VERIFY_REMOTE_RIGHT_FAIL = 15;

		// Token: 0x04002409 RID: 9225
		public const int MINOR_MULTI_VERIFY_REPEAT_VERIFY = 35;

		// Token: 0x0400240A RID: 9226
		public const int MINOR_MULTI_VERIFY_SUCCESS = 16;

		// Token: 0x0400240B RID: 9227
		public const int MINOR_MULTI_VERIFY_SUPER_RIGHT_FAIL = 14;

		// Token: 0x0400240C RID: 9228
		public const int MINOR_MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS = 34;

		// Token: 0x0400240D RID: 9229
		public const int MINOR_MULTI_VERIFY_TIMEOUT = 36;

		// Token: 0x0400240E RID: 9230
		public const int MINOR_NET_BROKEN = 39;

		// Token: 0x0400240F RID: 9231
		public const int MINOR_NET_RESUME = 1031;

		// Token: 0x04002410 RID: 9232
		public const int MINOR_NORMAL_CFGFILE_INPUT = 1042;

		// Token: 0x04002411 RID: 9233
		public const int MINOR_NORMAL_CFGFILE_OUTTPUT = 1043;

		// Token: 0x04002412 RID: 9234
		public const int MINOR_NOT_BELONG_MULTI_GROUP = 12;

		// Token: 0x04002413 RID: 9235
		public const int MINOR_NTP_CHECK_TIME = 1029;

		// Token: 0x04002414 RID: 9236
		public const int MINOR_OFFLINE_ECENT_NEARLY_FULL = 1035;

		// Token: 0x04002415 RID: 9237
		public const int MINOR_OPENBUTTON_INPUT_BROKEN_CIRCUIT = 90;

		// Token: 0x04002416 RID: 9238
		public const int MINOR_OPENBUTTON_INPUT_EXCEPTION = 91;

		// Token: 0x04002417 RID: 9239
		public const int MINOR_OPENBUTTON_INPUT_SHORT_CIRCUIT = 89;

		// Token: 0x04002418 RID: 9240
		public const int MINOR_PASSING_TIMEOUT = 137;

		// Token: 0x04002419 RID: 9241
		public const int MINOR_PASSWORD_MISMATCH = 151;

		// Token: 0x0400241A RID: 9242
		public const int MINOR_PEOPLE_AND_ID_CARD_COMPARE_FAIL = 112;

		// Token: 0x0400241B RID: 9243
		public const int MINOR_PEOPLE_AND_ID_CARD_COMPARE_PASS = 105;

		// Token: 0x0400241C RID: 9244
		public const int MINOR_PEOPLE_AND_ID_CARD_DEVICE_OFFLINE = 1063;

		// Token: 0x0400241D RID: 9245
		public const int MINOR_PEOPLE_AND_ID_CARD_DEVICE_ONLINE = 1062;

		// Token: 0x0400241E RID: 9246
		public const int MINOR_POS_END_ALARM = 1042;

		// Token: 0x0400241F RID: 9247
		public const int MINOR_POS_START_ALARM = 1041;

		// Token: 0x04002420 RID: 9248
		public const int MINOR_PRINTER_OFFLINE = 1079;

		// Token: 0x04002421 RID: 9249
		public const int MINOR_PRINTER_ONLINE = 1078;

		// Token: 0x04002422 RID: 9250
		public const int MINOR_PRINTER_OUT_OF_PAPER = 1088;

		// Token: 0x04002423 RID: 9251
		public const int MINOR_REMOTE_ACTUAL_GUARD = 1049;

		// Token: 0x04002424 RID: 9252
		public const int MINOR_REMOTE_ACTUAL_UNGUARD = 1050;

		// Token: 0x04002425 RID: 9253
		public const int MINOR_REMOTE_ALARMOUT_CLOSE_MAN = 215;

		// Token: 0x04002426 RID: 9254
		public const int MINOR_REMOTE_ALARMOUT_OPEN_MAN = 214;

		// Token: 0x04002427 RID: 9255
		public const int MINOR_REMOTE_ALWAYS_CLOSE = 1027;

		// Token: 0x04002428 RID: 9256
		public const int MINOR_REMOTE_ALWAYS_OPEN = 1026;

		// Token: 0x04002429 RID: 9257
		public const int MINOR_REMOTE_ARM = 121;

		// Token: 0x0400242A RID: 9258
		public const int MINOR_REMOTE_CAPTURE_PIC = 1035;

		// Token: 0x0400242B RID: 9259
		public const int MINOR_REMOTE_CFGFILE_INTPUT = 135;

		// Token: 0x0400242C RID: 9260
		public const int MINOR_REMOTE_CFGFILE_OUTPUT = 134;

		// Token: 0x0400242D RID: 9261
		public const int MINOR_REMOTE_CHECK_TIME = 1028;

		// Token: 0x0400242E RID: 9262
		public const int MINOR_REMOTE_CLEAR_CARD = 1030;

		// Token: 0x0400242F RID: 9263
		public const int MINOR_REMOTE_CLOSE_DOOR = 1025;

		// Token: 0x04002430 RID: 9264
		public const int MINOR_REMOTE_CONTROL_ALWAYS_OPEN_DOOR = 1054;

		// Token: 0x04002431 RID: 9265
		public const int MINOR_REMOTE_CONTROL_CLOSE_DOOR = 1052;

		// Token: 0x04002432 RID: 9266
		public const int MINOR_REMOTE_CONTROL_NOT_CODE_OPER_FAILED = 1051;

		// Token: 0x04002433 RID: 9267
		public const int MINOR_REMOTE_CONTROL_OPEN_DOOR = 1053;

		// Token: 0x04002434 RID: 9268
		public const int MINOR_REMOTE_DISARM = 122;

		// Token: 0x04002435 RID: 9269
		public const int MINOR_REMOTE_HOUSEHOLD_CALL_LADDER = 1048;

		// Token: 0x04002436 RID: 9270
		public const int MINOR_REMOTE_LOGIN = 112;

		// Token: 0x04002437 RID: 9271
		public const int MINOR_REMOTE_LOGOUT = 113;

		// Token: 0x04002438 RID: 9272
		public const int MINOR_REMOTE_OPEN_DOOR = 1024;

		// Token: 0x04002439 RID: 9273
		public const int MINOR_REMOTE_REBOOT = 123;

		// Token: 0x0400243A RID: 9274
		public const int MINOR_REMOTE_RESTORE_CFG = 1031;

		// Token: 0x0400243B RID: 9275
		public const int MINOR_REMOTE_UPGRADE = 126;

		// Token: 0x0400243C RID: 9276
		public const int MINOR_REMOTE_VISITOR_CALL_LADDER = 1047;

		// Token: 0x0400243D RID: 9277
		public const int MINOR_REVERSE_ACCESS = 134;

		// Token: 0x0400243E RID: 9278
		public const int MINOR_RS485_DEVICE_ABNORMAL = 58;

		// Token: 0x0400243F RID: 9279
		public const int MINOR_RS485_DEVICE_REVERT = 59;

		// Token: 0x04002440 RID: 9280
		public const int MINOR_SD_CARD_FULL = 1037;

		// Token: 0x04002441 RID: 9281
		public const int MINOR_SECURITY_MODULE_DESMANTLE_ALARM = 1039;

		// Token: 0x04002442 RID: 9282
		public const int MINOR_SECURITY_MODULE_DESMANTLE_RESUME = 1040;

		// Token: 0x04002443 RID: 9283
		public const int MINOR_SECURITY_MODULE_OFF = 1039;

		// Token: 0x04002444 RID: 9284
		public const int MINOR_SECURITY_MODULE_RESUME = 1040;

		// Token: 0x04002445 RID: 9285
		public const int MINOR_STAY_EVENT = 143;

		// Token: 0x04002446 RID: 9286
		public const int MINOR_STRESS_ALARM = 1034;

		// Token: 0x04002447 RID: 9287
		public const int MINOR_SUBMARINEBACK_COMM_BREAK = 1066;

		// Token: 0x04002448 RID: 9288
		public const int MINOR_SUBMARINEBACK_COMM_RESUME = 1067;

		// Token: 0x04002449 RID: 9289
		public const int MINOR_SUBMARINEBACK_REPLY_FAIL = 120;

		// Token: 0x0400244A RID: 9290
		public const int MINOR_TRAILING = 133;

		// Token: 0x0400244B RID: 9291
		public const int MINOR_UNLOCK_PASSWORD_OPEN_DOOR = 1039;

		// Token: 0x0400244C RID: 9292
		public const int MINOR_VERIFY_MODE_MISMATCH = 155;

		// Token: 0x0400244D RID: 9293
		public const int MINOR_WATCH_DOG_RESET = 1026;

		// Token: 0x0400244E RID: 9294
		public const int NAME_LEN = 32;

		// Token: 0x0400244F RID: 9295
		public const int NET_DVR_ALLOC_RESOURCE_ERROR = 41;

		// Token: 0x04002450 RID: 9296
		public const int NET_DVR_AUDIO_MODE_ERROR = 42;

		// Token: 0x04002451 RID: 9297
		public const int NET_DVR_BUSY = 24;

		// Token: 0x04002452 RID: 9298
		public const int NET_DVR_CAPTURE_FACE_INFO = 2510;

		// Token: 0x04002453 RID: 9299
		public const int NET_DVR_CARDHAVEINIT = 50;

		// Token: 0x04002454 RID: 9300
		public const int NET_DVR_CHAN_EXCEPTION = 18;

		// Token: 0x04002455 RID: 9301
		public const int NET_DVR_CHANNEL_ERROR = 4;

		// Token: 0x04002456 RID: 9302
		public const int NET_DVR_CLEAR_ACS_PARAM = 2118;

		// Token: 0x04002457 RID: 9303
		public const int NET_DVR_COMMANDTIMEOUT = 14;

		// Token: 0x04002458 RID: 9304
		public const int NET_DVR_CREATEFILE_ERROR = 34;

		// Token: 0x04002459 RID: 9305
		public const int NET_DVR_CREATESOCKET_ERROR = 44;

		// Token: 0x0400245A RID: 9306
		public const int NET_DVR_DEL_FACE_PARAM_CFG = 2509;

		// Token: 0x0400245B RID: 9307
		public const int NET_DVR_DEL_FINGERPRINT_CFG = 2152;

		// Token: 0x0400245C RID: 9308
		public const int NET_DVR_DEL_FINGERPRINT_CFG_V50 = 2517;

		// Token: 0x0400245D RID: 9309
		public const int NET_DVR_DEV_ADDRESS_MAX_LEN = 129;

		// Token: 0x0400245E RID: 9310
		public const int NET_DVR_DIR_ERROR = 40;

		// Token: 0x0400245F RID: 9311
		public const int NET_DVR_DISK_ERROR = 22;

		// Token: 0x04002460 RID: 9312
		public const int NET_DVR_DISK_FORMATING = 27;

		// Token: 0x04002461 RID: 9313
		public const int NET_DVR_DISK_FULL = 21;

		// Token: 0x04002462 RID: 9314
		public const int NET_DVR_DVRNORESOURCE = 28;

		// Token: 0x04002463 RID: 9315
		public const int NET_DVR_DVROPRATEFAILED = 29;

		// Token: 0x04002464 RID: 9316
		public const int NET_DVR_DVRVOICEOPENED = 31;

		// Token: 0x04002465 RID: 9317
		public const int NET_DVR_ERRORALARMPORT = 16;

		// Token: 0x04002466 RID: 9318
		public const int NET_DVR_ERRORDISKNUM = 20;

		// Token: 0x04002467 RID: 9319
		public const int NET_DVR_ERRORSERIALPORT = 15;

		// Token: 0x04002468 RID: 9320
		public const int NET_DVR_FACE_DATA_MODIFY = 2553;

		// Token: 0x04002469 RID: 9321
		public const int NET_DVR_FACE_DATA_RECORD = 2551;

		// Token: 0x0400246A RID: 9322
		public const int NET_DVR_FACE_DATA_SEARCH = 2552;

		// Token: 0x0400246B RID: 9323
		public const int NET_DVR_FILEFORMAT_ERROR = 39;

		// Token: 0x0400246C RID: 9324
		public const int NET_DVR_FILEOPENFAIL = 35;

		// Token: 0x0400246D RID: 9325
		public const int NET_DVR_GET_ACS_EVENT = 2514;

		// Token: 0x0400246E RID: 9326
		public const int NET_DVR_GET_ACS_WORK_STATUS_V50 = 2180;

		// Token: 0x0400246F RID: 9327
		public const int NET_DVR_GET_AUDIOIN_VOLUME_CFG = 6355;

		// Token: 0x04002470 RID: 9328
		public const int NET_DVR_GET_AUDIOOUT_VOLUME_CFG = 6369;

		// Token: 0x04002471 RID: 9329
		public const int NET_DVR_GET_CARD_CFG_V50 = 2178;

		// Token: 0x04002472 RID: 9330
		public const int NET_DVR_GET_CARD_READER_CFG_V50 = 2505;

		// Token: 0x04002473 RID: 9331
		public const int NET_DVR_GET_CARD_READER_PLAN = 2142;

		// Token: 0x04002474 RID: 9332
		public const int NET_DVR_GET_CARD_RIGHT_HOLIDAY_GROUP = 2134;

		// Token: 0x04002475 RID: 9333
		public const int NET_DVR_GET_CARD_RIGHT_HOLIDAY_GROUP_V50 = 2316;

		// Token: 0x04002476 RID: 9334
		public const int NET_DVR_GET_CARD_RIGHT_HOLIDAY_PLAN = 2130;

		// Token: 0x04002477 RID: 9335
		public const int NET_DVR_GET_CARD_RIGHT_HOLIDAY_PLAN_V50 = 2310;

		// Token: 0x04002478 RID: 9336
		public const int NET_DVR_GET_CARD_RIGHT_PLAN_TEMPLATE = 2138;

		// Token: 0x04002479 RID: 9337
		public const int NET_DVR_GET_CARD_RIGHT_PLAN_TEMPLATE_V50 = 2322;

		// Token: 0x0400247A RID: 9338
		public const int NET_DVR_GET_CARD_RIGHT_WEEK_PLAN = 2126;

		// Token: 0x0400247B RID: 9339
		public const int NET_DVR_GET_CARD_RIGHT_WEEK_PLAN_V50 = 2304;

		// Token: 0x0400247C RID: 9340
		public const int NET_DVR_GET_CARD_USERINFO_CFG = 2163;

		// Token: 0x0400247D RID: 9341
		public const int NET_DVR_GET_DEVICECFG_V40 = 1100;

		// Token: 0x0400247E RID: 9342
		public const int NET_DVR_GET_DOOR_CFG = 2108;

		// Token: 0x0400247F RID: 9343
		public const int NET_DVR_GET_DOOR_STATUS_HOLIDAY_GROUP = 2104;

		// Token: 0x04002480 RID: 9344
		public const int NET_DVR_GET_DOOR_STATUS_HOLIDAY_PLAN = 2102;

		// Token: 0x04002481 RID: 9345
		public const int NET_DVR_GET_DOOR_STATUS_PLAN = 2110;

		// Token: 0x04002482 RID: 9346
		public const int NET_DVR_GET_DOOR_STATUS_PLAN_TEMPLATE = 2106;

		// Token: 0x04002483 RID: 9347
		public const int NET_DVR_GET_EVENT_CARD_LINKAGE_CFG_V50 = 2181;

		// Token: 0x04002484 RID: 9348
		public const int NET_DVR_GET_EVENT_CARD_LINKAGE_CFG_V51 = 2518;

		// Token: 0x04002485 RID: 9349
		public const int NET_DVR_GET_FACE_PARAM_CFG = 2507;

		// Token: 0x04002486 RID: 9350
		public const int NET_DVR_GET_FINGERPRINT_CFG = 2150;

		// Token: 0x04002487 RID: 9351
		public const int NET_DVR_GET_FINGERPRINT_CFG_V50 = 2183;

		// Token: 0x04002488 RID: 9352
		public const int NET_DVR_GET_GROUP_CFG = 2112;

		// Token: 0x04002489 RID: 9353
		public const int NET_DVR_GET_IPPARACFG_V40 = 1062;

		// Token: 0x0400248A RID: 9354
		public const int NET_DVR_GET_NETCFG_V30 = 1000;

		// Token: 0x0400248B RID: 9355
		public const int NET_DVR_GET_NETCFG_V50 = 1015;

		// Token: 0x0400248C RID: 9356
		public const int NET_DVR_GET_TIMECFG = 118;

		// Token: 0x0400248D RID: 9357
		public const int NET_DVR_GET_VERIFY_HOLIDAY_GROUP = 2132;

		// Token: 0x0400248E RID: 9358
		public const int NET_DVR_GET_VERIFY_HOLIDAY_PLAN = 2128;

		// Token: 0x0400248F RID: 9359
		public const int NET_DVR_GET_VERIFY_PLAN_TEMPLATE = 2136;

		// Token: 0x04002490 RID: 9360
		public const int NET_DVR_GET_VERIFY_WEEK_PLAN = 2124;

		// Token: 0x04002491 RID: 9361
		public const int NET_DVR_GET_WEEK_PLAN_CFG = 2100;

		// Token: 0x04002492 RID: 9362
		public const int NET_DVR_GETLOCALIPANDMACFAIL = 53;

		// Token: 0x04002493 RID: 9363
		public const int NET_DVR_GETPLAYTIMEFAIL = 37;

		// Token: 0x04002494 RID: 9364
		public const int NET_DVR_IPMISMATCH = 55;

		// Token: 0x04002495 RID: 9365
		public const int NET_DVR_JSON_CONFIG = 2550;

		// Token: 0x04002496 RID: 9366
		public const int NET_DVR_LOGIN_PASSWD_MAX_LEN = 64;

		// Token: 0x04002497 RID: 9367
		public const int NET_DVR_LOGIN_USERNAME_MAX_LEN = 64;

		// Token: 0x04002498 RID: 9368
		public const int NET_DVR_MACMISMATCH = 56;

		// Token: 0x04002499 RID: 9369
		public const int NET_DVR_MAX_NUM = 46;

		// Token: 0x0400249A RID: 9370
		public const int NET_DVR_MAX_USERNUM = 52;

		// Token: 0x0400249B RID: 9371
		public const int NET_DVR_MODIFY_FAIL = 25;

		// Token: 0x0400249C RID: 9372
		public const int NET_DVR_NETWORK_ERRORDATA = 11;

		// Token: 0x0400249D RID: 9373
		public const int NET_DVR_NETWORK_FAIL_CONNECT = 7;

		// Token: 0x0400249E RID: 9374
		public const int NET_DVR_NETWORK_RECV_ERROR = 9;

		// Token: 0x0400249F RID: 9375
		public const int NET_DVR_NETWORK_RECV_TIMEOUT = 10;

		// Token: 0x040024A0 RID: 9376
		public const int NET_DVR_NETWORK_SEND_ERROR = 8;

		// Token: 0x040024A1 RID: 9377
		public const int NET_DVR_NODISK = 19;

		// Token: 0x040024A2 RID: 9378
		public const int NET_DVR_NOENCODEING = 54;

		// Token: 0x040024A3 RID: 9379
		public const int NET_DVR_NOENOUGH_BUF = 43;

		// Token: 0x040024A4 RID: 9380
		public const int NET_DVR_NOENOUGHPRI = 2;

		// Token: 0x040024A5 RID: 9381
		public const int NET_DVR_NOERROR = 0;

		// Token: 0x040024A6 RID: 9382
		public const int NET_DVR_NOINIT = 3;

		// Token: 0x040024A7 RID: 9383
		public const int NET_DVR_NOSPECFILE = 33;

		// Token: 0x040024A8 RID: 9384
		public const int NET_DVR_NOSUPPORT = 23;

		// Token: 0x040024A9 RID: 9385
		public const int NET_DVR_OPENHOSTSOUND_FAIL = 30;

		// Token: 0x040024AA RID: 9386
		public const int NET_DVR_OPERNOPERMIT = 13;

		// Token: 0x040024AB RID: 9387
		public const int NET_DVR_OPERNOTFINISH = 36;

		// Token: 0x040024AC RID: 9388
		public const int NET_DVR_ORDER_ERROR = 12;

		// Token: 0x040024AD RID: 9389
		public const int NET_DVR_OVER_MAXLINK = 5;

		// Token: 0x040024AE RID: 9390
		public const int NET_DVR_PARAMETER_ERROR = 17;

		// Token: 0x040024AF RID: 9391
		public const int NET_DVR_PASSWORD_ERROR = 1;

		// Token: 0x040024B0 RID: 9392
		public const int NET_DVR_PASSWORD_FORMAT_ERROR = 26;

		// Token: 0x040024B1 RID: 9393
		public const int NET_DVR_PLAYERFAILED = 51;

		// Token: 0x040024B2 RID: 9394
		public const int NET_DVR_PLAYFAIL = 38;

		// Token: 0x040024B3 RID: 9395
		public const int NET_DVR_SET_AUDIOIN_VOLUME_CFG = 6356;

		// Token: 0x040024B4 RID: 9396
		public const int NET_DVR_SET_AUDIOOUT_VOLUME_CFG = 6370;

		// Token: 0x040024B5 RID: 9397
		public const int NET_DVR_SET_CARD_CFG_V50 = 2179;

		// Token: 0x040024B6 RID: 9398
		public const int NET_DVR_SET_CARD_READER_CFG_V50 = 2506;

		// Token: 0x040024B7 RID: 9399
		public const int NET_DVR_SET_CARD_READER_PLAN = 2143;

		// Token: 0x040024B8 RID: 9400
		public const int NET_DVR_SET_CARD_RIGHT_HOLIDAY_GROUP = 2135;

		// Token: 0x040024B9 RID: 9401
		public const int NET_DVR_SET_CARD_RIGHT_HOLIDAY_GROUP_V50 = 2317;

		// Token: 0x040024BA RID: 9402
		public const int NET_DVR_SET_CARD_RIGHT_HOLIDAY_PLAN = 2131;

		// Token: 0x040024BB RID: 9403
		public const int NET_DVR_SET_CARD_RIGHT_HOLIDAY_PLAN_V50 = 2311;

		// Token: 0x040024BC RID: 9404
		public const int NET_DVR_SET_CARD_RIGHT_PLAN_TEMPLATE = 2139;

		// Token: 0x040024BD RID: 9405
		public const int NET_DVR_SET_CARD_RIGHT_PLAN_TEMPLATE_V50 = 2323;

		// Token: 0x040024BE RID: 9406
		public const int NET_DVR_SET_CARD_RIGHT_WEEK_PLAN = 2127;

		// Token: 0x040024BF RID: 9407
		public const int NET_DVR_SET_CARD_RIGHT_WEEK_PLAN_V50 = 2305;

		// Token: 0x040024C0 RID: 9408
		public const int NET_DVR_SET_CARD_USERINFO_CFG = 2164;

		// Token: 0x040024C1 RID: 9409
		public const int NET_DVR_SET_DEVICECFG_V40 = 1101;

		// Token: 0x040024C2 RID: 9410
		public const int NET_DVR_SET_DOOR_CFG = 2109;

		// Token: 0x040024C3 RID: 9411
		public const int NET_DVR_SET_DOOR_STATUS_HOLIDAY_GROUP = 2105;

		// Token: 0x040024C4 RID: 9412
		public const int NET_DVR_SET_DOOR_STATUS_HOLIDAY_PLAN = 2103;

		// Token: 0x040024C5 RID: 9413
		public const int NET_DVR_SET_DOOR_STATUS_PLAN = 2111;

		// Token: 0x040024C6 RID: 9414
		public const int NET_DVR_SET_DOOR_STATUS_PLAN_TEMPLATE = 2107;

		// Token: 0x040024C7 RID: 9415
		public const int NET_DVR_SET_EVENT_CARD_LINKAGE_CFG_V50 = 2182;

		// Token: 0x040024C8 RID: 9416
		public const int NET_DVR_SET_EVENT_CARD_LINKAGE_CFG_V51 = 2519;

		// Token: 0x040024C9 RID: 9417
		public const int NET_DVR_SET_FACE_PARAM_CFG = 2508;

		// Token: 0x040024CA RID: 9418
		public const int NET_DVR_SET_FINGERPRINT_CFG = 2151;

		// Token: 0x040024CB RID: 9419
		public const int NET_DVR_SET_FINGERPRINT_CFG_V50 = 2184;

		// Token: 0x040024CC RID: 9420
		public const int NET_DVR_SET_GROUP_CFG = 2113;

		// Token: 0x040024CD RID: 9421
		public const int NET_DVR_SET_IPPARACFG_V40 = 1063;

		// Token: 0x040024CE RID: 9422
		public const int NET_DVR_SET_NETCFG_V30 = 1001;

		// Token: 0x040024CF RID: 9423
		public const int NET_DVR_SET_NETCFG_V50 = 1016;

		// Token: 0x040024D0 RID: 9424
		public const int NET_DVR_SET_TIMECFG = 119;

		// Token: 0x040024D1 RID: 9425
		public const int NET_DVR_SET_VERIFY_HOLIDAY_GROUP = 2133;

		// Token: 0x040024D2 RID: 9426
		public const int NET_DVR_SET_VERIFY_HOLIDAY_PLAN = 2129;

		// Token: 0x040024D3 RID: 9427
		public const int NET_DVR_SET_VERIFY_PLAN_TEMPLATE = 2137;

		// Token: 0x040024D4 RID: 9428
		public const int NET_DVR_SET_VERIFY_WEEK_PLAN = 2125;

		// Token: 0x040024D5 RID: 9429
		public const int NET_DVR_SET_WEEK_PLAN_CFG = 2101;

		// Token: 0x040024D6 RID: 9430
		public const int NET_DVR_SETSOCKET_ERROR = 45;

		// Token: 0x040024D7 RID: 9431
		public const int NET_DVR_TIMEINPUTERROR = 32;

		// Token: 0x040024D8 RID: 9432
		public const int NET_DVR_UPGRADEFAIL = 49;

		// Token: 0x040024D9 RID: 9433
		public const int NET_DVR_USER_LOCKED = 153;

		// Token: 0x040024DA RID: 9434
		public const int NET_DVR_USERNOTEXIST = 47;

		// Token: 0x040024DB RID: 9435
		public const int NET_DVR_VERSIONNOMATCH = 6;

		// Token: 0x040024DC RID: 9436
		public const int NET_DVR_VIDEO_CALL_SIGNAL_PROCESS = 16032;

		// Token: 0x040024DD RID: 9437
		public const int NET_DVR_WRITEFLASHERROR = 48;

		// Token: 0x040024DE RID: 9438
		public const int NET_SDK_EMPLOYEE_NO_LEN = 32;

		// Token: 0x040024DF RID: 9439
		public const int NET_SDK_MONITOR_ID_LEN = 64;

		// Token: 0x040024E0 RID: 9440
		public const int PASSWD_LEN = 16;

		// Token: 0x040024E1 RID: 9441
		public const int SERIALNO_LEN = 48;

		// Token: 0x040024E2 RID: 9442
		public const int STREAM_ID_LEN = 32;

		// Token: 0x040024E3 RID: 9443
		public const int STRESS_PASSWORD_LEN = 8;

		// Token: 0x040024E4 RID: 9444
		public const int SUPER_PASSWORD_LEN = 8;

		// Token: 0x040024E5 RID: 9445
		public const int TEMPLATE_NAME_LEN = 32;

		// Token: 0x040024E6 RID: 9446
		public const int UNLOCK_PASSWORD_LEN = 8;

		// Token: 0x040024E7 RID: 9447
		public const int URL_LEN = 240;

		// Token: 0x040024E8 RID: 9448
		public const int WM_MSG_ADD_ACS_EVENT_TOLIST = 1002;

		// Token: 0x040024E9 RID: 9449
		public const int WM_MSG_ADD_FACE_PARAM_TOLIST = 1004;

		// Token: 0x040024EA RID: 9450
		public const int WM_MSG_GET_ACS_EVENT_FINISH = 1003;

		// Token: 0x040024EB RID: 9451
		public const int WM_MSG_GET_FACE_PARAM_FINISH = 1003;

		// Token: 0x040024EC RID: 9452
		public const int WM_MSG_SET_FACE_PARAM_FINISH = 1002;

		// Token: 0x02000257 RID: 599
		public enum ACS_ALARM_SUBEVENT_ENUM
		{
			// Token: 0x040024EE RID: 9454
			EVENT_ACS_ALARMIN_SHORT_CIRCUIT,
			// Token: 0x040024EF RID: 9455
			EVENT_ACS_ALARMIN_BROKEN_CIRCUIT,
			// Token: 0x040024F0 RID: 9456
			EVENT_ACS_ALARMIN_EXCEPTION,
			// Token: 0x040024F1 RID: 9457
			EVENT_ACS_ALARMIN_RESUME,
			// Token: 0x040024F2 RID: 9458
			EVENT_ACS_CASE_SENSOR_ALARM,
			// Token: 0x040024F3 RID: 9459
			EVENT_ACS_CASE_SENSOR_RESUME
		}

		// Token: 0x02000258 RID: 600
		public enum ACS_CARD_READER_SUBEVENT_ENUM
		{
			// Token: 0x040024F5 RID: 9461
			EVENT_ACS_STRESS_ALARM,
			// Token: 0x040024F6 RID: 9462
			EVENT_ACS_CARD_READER_DESMANTLE_ALARM,
			// Token: 0x040024F7 RID: 9463
			EVENT_ACS_LEGAL_CARD_PASS,
			// Token: 0x040024F8 RID: 9464
			EVENT_ACS_CARD_AND_PSW_PASS,
			// Token: 0x040024F9 RID: 9465
			EVENT_ACS_CARD_AND_PSW_FAIL,
			// Token: 0x040024FA RID: 9466
			EVENT_ACS_CARD_AND_PSW_TIMEOUT,
			// Token: 0x040024FB RID: 9467
			EVENT_ACS_CARD_MAX_AUTHENTICATE_FAIL,
			// Token: 0x040024FC RID: 9468
			EVENT_ACS_CARD_NO_RIGHT,
			// Token: 0x040024FD RID: 9469
			EVENT_ACS_CARD_INVALID_PERIOD,
			// Token: 0x040024FE RID: 9470
			EVENT_ACS_CARD_OUT_OF_DATE,
			// Token: 0x040024FF RID: 9471
			EVENT_ACS_INVALID_CARD,
			// Token: 0x04002500 RID: 9472
			EVENT_ACS_ANTI_SNEAK_FAIL,
			// Token: 0x04002501 RID: 9473
			EVENT_ACS_INTERLOCK_DOOR_NOT_CLOSE,
			// Token: 0x04002502 RID: 9474
			EVENT_ACS_FINGERPRINT_COMPARE_PASS,
			// Token: 0x04002503 RID: 9475
			EVENT_ACS_FINGERPRINT_COMPARE_FAIL,
			// Token: 0x04002504 RID: 9476
			EVENT_ACS_CARD_FINGERPRINT_VERIFY_PASS,
			// Token: 0x04002505 RID: 9477
			EVENT_ACS_CARD_FINGERPRINT_VERIFY_FAIL,
			// Token: 0x04002506 RID: 9478
			EVENT_ACS_CARD_FINGERPRINT_VERIFY_TIMEOUT,
			// Token: 0x04002507 RID: 9479
			EVENT_ACS_CARD_FINGERPRINT_PASSWD_VERIFY_PASS,
			// Token: 0x04002508 RID: 9480
			EVENT_ACS_CARD_FINGERPRINT_PASSWD_VERIFY_FAIL,
			// Token: 0x04002509 RID: 9481
			EVENT_ACS_CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT,
			// Token: 0x0400250A RID: 9482
			EVENT_ACS_FINGERPRINT_PASSWD_VERIFY_PASS,
			// Token: 0x0400250B RID: 9483
			EVENT_ACS_FINGERPRINT_PASSWD_VERIFY_FAIL,
			// Token: 0x0400250C RID: 9484
			EVENT_ACS_FINGERPRINT_PASSWD_VERIFY_TIMEOUT,
			// Token: 0x0400250D RID: 9485
			EVENT_ACS_FINGERPRINT_INEXISTENCE,
			// Token: 0x0400250E RID: 9486
			EVENT_ACS_FACE_VERIFY_PASS,
			// Token: 0x0400250F RID: 9487
			EVENT_ACS_FACE_VERIFY_FAIL,
			// Token: 0x04002510 RID: 9488
			EVENT_ACS_FACE_AND_FP_VERIFY_PASS,
			// Token: 0x04002511 RID: 9489
			EVENT_ACS_FACE_AND_FP_VERIFY_FAIL,
			// Token: 0x04002512 RID: 9490
			EVENT_ACS_FACE_AND_FP_VERIFY_TIMEOUT,
			// Token: 0x04002513 RID: 9491
			EVENT_ACS_FACE_AND_PW_VERIFY_PASS,
			// Token: 0x04002514 RID: 9492
			EVENT_ACS_FACE_AND_PW_VERIFY_FAIL,
			// Token: 0x04002515 RID: 9493
			EVENT_ACS_FACE_AND_PW_VERIFY_TIMEOUT,
			// Token: 0x04002516 RID: 9494
			EVENT_ACS_FACE_AND_CARD_VERIFY_PASS,
			// Token: 0x04002517 RID: 9495
			EVENT_ACS_FACE_AND_CARD_VERIFY_FAIL,
			// Token: 0x04002518 RID: 9496
			EVENT_ACS_FACE_AND_CARD_VERIFY_TIMEOUT,
			// Token: 0x04002519 RID: 9497
			EVENT_ACS_FACE_AND_PW_AND_FP_VERIFY_PASS,
			// Token: 0x0400251A RID: 9498
			EVENT_ACS_FACE_AND_PW_AND_FP_VERIFY_FAIL,
			// Token: 0x0400251B RID: 9499
			EVENT_ACS_FACE_AND_PW_AND_FP_VERIFY_TIMEOUT,
			// Token: 0x0400251C RID: 9500
			EVENT_ACS_FACE_AND_CARD_AND_FP_VERIFY_PASS,
			// Token: 0x0400251D RID: 9501
			EVENT_ACS_FACE_AND_CARD_AND_FP_VERIFY_FAIL,
			// Token: 0x0400251E RID: 9502
			EVENT_ACS_FACE_AND_CARD_AND_FP_VERIFY_TIMEOUT,
			// Token: 0x0400251F RID: 9503
			EVENT_ACS_EMPLOYEENO_AND_FP_VERIFY_PASS,
			// Token: 0x04002520 RID: 9504
			EVENT_ACS_EMPLOYEENO_AND_FP_VERIFY_FAIL,
			// Token: 0x04002521 RID: 9505
			EVENT_ACS_EMPLOYEENO_AND_FP_VERIFY_TIMEOUT,
			// Token: 0x04002522 RID: 9506
			EVENT_ACS_EMPLOYEENO_AND_FP_AND_PW_VERIFY_PASS,
			// Token: 0x04002523 RID: 9507
			EVENT_ACS_EMPLOYEENO_AND_FP_AND_PW_VERIFY_FAIL,
			// Token: 0x04002524 RID: 9508
			EVENT_ACS_EMPLOYEENO_AND_FP_AND_PW_VERIFY_TIMEOUT,
			// Token: 0x04002525 RID: 9509
			EVENT_ACS_EMPLOYEENO_AND_FACE_VERIFY_PASS,
			// Token: 0x04002526 RID: 9510
			EVENT_ACS_EMPLOYEENO_AND_FACE_VERIFY_FAIL,
			// Token: 0x04002527 RID: 9511
			EVENT_ACS_EMPLOYEENO_AND_FACE_VERIFY_TIMEOUT,
			// Token: 0x04002528 RID: 9512
			EVENT_ACS_FACE_RECOGNIZE_FAIL,
			// Token: 0x04002529 RID: 9513
			EVENT_ACS_EMPLOYEENO_AND_PW_PASS,
			// Token: 0x0400252A RID: 9514
			EVENT_ACS_EMPLOYEENO_AND_PW_FAIL,
			// Token: 0x0400252B RID: 9515
			EVENT_ACS_EMPLOYEENO_AND_PW_TIMEOUT,
			// Token: 0x0400252C RID: 9516
			EVENT_ACS_HUMAN_DETECT_FAIL
		}

		// Token: 0x02000259 RID: 601
		public enum ACS_DEV_SUBEVENT_ENUM
		{
			// Token: 0x0400252E RID: 9518
			EVENT_ACS_HOST_ANTI_DISMANTLE,
			// Token: 0x0400252F RID: 9519
			EVENT_ACS_OFFLINE_ECENT_NEARLY_FULL,
			// Token: 0x04002530 RID: 9520
			EVENT_ACS_NET_BROKEN,
			// Token: 0x04002531 RID: 9521
			EVENT_ACS_NET_RESUME,
			// Token: 0x04002532 RID: 9522
			EVENT_ACS_LOW_BATTERY,
			// Token: 0x04002533 RID: 9523
			EVENT_ACS_BATTERY_RESUME,
			// Token: 0x04002534 RID: 9524
			EVENT_ACS_AC_OFF,
			// Token: 0x04002535 RID: 9525
			EVENT_ACS_AC_RESUME,
			// Token: 0x04002536 RID: 9526
			EVENT_ACS_SD_CARD_FULL,
			// Token: 0x04002537 RID: 9527
			EVENT_ACS_LINKAGE_CAPTURE_PIC,
			// Token: 0x04002538 RID: 9528
			EVENT_ACS_IMAGE_QUALITY_LOW,
			// Token: 0x04002539 RID: 9529
			EVENT_ACS_FINGER_PRINT_QUALITY_LOW,
			// Token: 0x0400253A RID: 9530
			EVENT_ACS_BATTERY_ELECTRIC_LOW,
			// Token: 0x0400253B RID: 9531
			EVENT_ACS_BATTERY_ELECTRIC_RESUME,
			// Token: 0x0400253C RID: 9532
			EVENT_ACS_FIRE_IMPORT_SHORT_CIRCUIT,
			// Token: 0x0400253D RID: 9533
			EVENT_ACS_FIRE_IMPORT_BROKEN_CIRCUIT,
			// Token: 0x0400253E RID: 9534
			EVENT_ACS_FIRE_IMPORT_RESUME,
			// Token: 0x0400253F RID: 9535
			EVENT_ACS_MASTER_RS485_LOOPNODE_BROKEN,
			// Token: 0x04002540 RID: 9536
			EVENT_ACS_MASTER_RS485_LOOPNODE_RESUME,
			// Token: 0x04002541 RID: 9537
			EVENT_ACS_LOCAL_CONTROL_OFFLINE,
			// Token: 0x04002542 RID: 9538
			EVENT_ACS_LOCAL_CONTROL_RESUME,
			// Token: 0x04002543 RID: 9539
			EVENT_ACS_LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN,
			// Token: 0x04002544 RID: 9540
			EVENT_ACS_LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME,
			// Token: 0x04002545 RID: 9541
			EVENT_ACS_DISTRACT_CONTROLLER_ONLINE,
			// Token: 0x04002546 RID: 9542
			EVENT_ACS_DISTRACT_CONTROLLER_OFFLINE,
			// Token: 0x04002547 RID: 9543
			EVENT_ACS_FIRE_BUTTON_TRIGGER,
			// Token: 0x04002548 RID: 9544
			EVENT_ACS_FIRE_BUTTON_RESUME,
			// Token: 0x04002549 RID: 9545
			EVENT_ACS_MAINTENANCE_BUTTON_TRIGGER,
			// Token: 0x0400254A RID: 9546
			EVENT_ACS_MAINTENANCE_BUTTON_RESUME,
			// Token: 0x0400254B RID: 9547
			EVENT_ACS_EMERGENCY_BUTTON_TRIGGER,
			// Token: 0x0400254C RID: 9548
			EVENT_ACS_EMERGENCY_BUTTON_RESUME,
			// Token: 0x0400254D RID: 9549
			EVENT_ACS_MAC_DETECT
		}

		// Token: 0x0200025A RID: 602
		public enum ACS_DOOR_SUBEVENT_ENUM
		{
			// Token: 0x0400254F RID: 9551
			EVENT_ACS_LEADER_CARD_OPEN_BEGIN,
			// Token: 0x04002550 RID: 9552
			EVENT_ACS_LEADER_CARD_OPEN_END,
			// Token: 0x04002551 RID: 9553
			EVENT_ACS_ALWAYS_OPEN_BEGIN,
			// Token: 0x04002552 RID: 9554
			EVENT_ACS_ALWAYS_OPEN_END,
			// Token: 0x04002553 RID: 9555
			EVENT_ACS_ALWAYS_CLOSE_BEGIN,
			// Token: 0x04002554 RID: 9556
			EVENT_ACS_ALWAYS_CLOSE_END,
			// Token: 0x04002555 RID: 9557
			EVENT_ACS_LOCK_OPEN,
			// Token: 0x04002556 RID: 9558
			EVENT_ACS_LOCK_CLOSE,
			// Token: 0x04002557 RID: 9559
			EVENT_ACS_DOOR_BUTTON_PRESS,
			// Token: 0x04002558 RID: 9560
			EVENT_ACS_DOOR_BUTTON_RELEASE,
			// Token: 0x04002559 RID: 9561
			EVENT_ACS_DOOR_OPEN_NORMAL,
			// Token: 0x0400255A RID: 9562
			EVENT_ACS_DOOR_CLOSE_NORMAL,
			// Token: 0x0400255B RID: 9563
			EVENT_ACS_DOOR_OPEN_ABNORMAL,
			// Token: 0x0400255C RID: 9564
			EVENT_ACS_DOOR_OPEN_TIMEOUT,
			// Token: 0x0400255D RID: 9565
			EVENT_ACS_REMOTE_OPEN_DOOR,
			// Token: 0x0400255E RID: 9566
			EVENT_ACS_REMOTE_CLOSE_DOOR,
			// Token: 0x0400255F RID: 9567
			EVENT_ACS_REMOTE_ALWAYS_OPEN,
			// Token: 0x04002560 RID: 9568
			EVENT_ACS_REMOTE_ALWAYS_CLOSE,
			// Token: 0x04002561 RID: 9569
			EVENT_ACS_NOT_BELONG_MULTI_GROUP,
			// Token: 0x04002562 RID: 9570
			EVENT_ACS_INVALID_MULTI_VERIFY_PERIOD,
			// Token: 0x04002563 RID: 9571
			EVENT_ACS_MULTI_VERIFY_SUPER_RIGHT_FAIL,
			// Token: 0x04002564 RID: 9572
			EVENT_ACS_MULTI_VERIFY_REMOTE_RIGHT_FAIL,
			// Token: 0x04002565 RID: 9573
			EVENT_ACS_MULTI_VERIFY_SUCCESS,
			// Token: 0x04002566 RID: 9574
			EVENT_ACS_MULTI_VERIFY_NEED_REMOTE_OPEN,
			// Token: 0x04002567 RID: 9575
			EVENT_ACS_MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS,
			// Token: 0x04002568 RID: 9576
			EVENT_ACS_MULTI_VERIFY_REPEAT_VERIFY_FAIL,
			// Token: 0x04002569 RID: 9577
			EVENT_ACS_MULTI_VERIFY_TIMEOUT,
			// Token: 0x0400256A RID: 9578
			EVENT_ACS_REMOTE_CAPTURE_PIC,
			// Token: 0x0400256B RID: 9579
			EVENT_ACS_DOORBELL_RINGING,
			// Token: 0x0400256C RID: 9580
			EVENT_ACS_SECURITY_MODULE_DESMANTLE_ALARM,
			// Token: 0x0400256D RID: 9581
			EVENT_ACS_CALL_CENTER,
			// Token: 0x0400256E RID: 9582
			EVENT_ACS_FIRSTCARD_AUTHORIZE_BEGIN,
			// Token: 0x0400256F RID: 9583
			EVENT_ACS_FIRSTCARD_AUTHORIZE_END,
			// Token: 0x04002570 RID: 9584
			EVENT_ACS_DOORLOCK_INPUT_SHORT_CIRCUIT,
			// Token: 0x04002571 RID: 9585
			EVENT_ACS_DOORLOCK_INPUT_BROKEN_CIRCUIT,
			// Token: 0x04002572 RID: 9586
			EVENT_ACS_DOORLOCK_INPUT_EXCEPTION,
			// Token: 0x04002573 RID: 9587
			EVENT_ACS_DOORCONTACT_INPUT_SHORT_CIRCUIT,
			// Token: 0x04002574 RID: 9588
			EVENT_ACS_DOORCONTACT_INPUT_BROKEN_CIRCUIT,
			// Token: 0x04002575 RID: 9589
			EVENT_ACS_DOORCONTACT_INPUT_EXCEPTION,
			// Token: 0x04002576 RID: 9590
			EVENT_ACS_OPENBUTTON_INPUT_SHORT_CIRCUIT,
			// Token: 0x04002577 RID: 9591
			EVENT_ACS_OPENBUTTON_INPUT_BROKEN_CIRCUIT,
			// Token: 0x04002578 RID: 9592
			EVENT_ACS_OPENBUTTON_INPUT_EXCEPTION,
			// Token: 0x04002579 RID: 9593
			EVENT_ACS_DOORLOCK_OPEN_EXCEPTION,
			// Token: 0x0400257A RID: 9594
			EVENT_ACS_DOORLOCK_OPEN_TIMEOUT,
			// Token: 0x0400257B RID: 9595
			EVENT_ACS_FIRSTCARD_OPEN_WITHOUT_AUTHORIZE,
			// Token: 0x0400257C RID: 9596
			EVENT_ACS_CALL_LADDER_RELAY_BREAK,
			// Token: 0x0400257D RID: 9597
			EVENT_ACS_CALL_LADDER_RELAY_CLOSE,
			// Token: 0x0400257E RID: 9598
			EVENT_ACS_AUTO_KEY_RELAY_BREAK,
			// Token: 0x0400257F RID: 9599
			EVENT_ACS_AUTO_KEY_RELAY_CLOSE,
			// Token: 0x04002580 RID: 9600
			EVENT_ACS_KEY_CONTROL_RELAY_BREAK,
			// Token: 0x04002581 RID: 9601
			EVENT_ACS_KEY_CONTROL_RELAY_CLOSE,
			// Token: 0x04002582 RID: 9602
			EVENT_ACS_REMOTE_VISITOR_CALL_LADDER,
			// Token: 0x04002583 RID: 9603
			EVENT_ACS_REMOTE_HOUSEHOLD_CALL_LADDER,
			// Token: 0x04002584 RID: 9604
			EVENT_ACS_LEGAL_MESSAGE,
			// Token: 0x04002585 RID: 9605
			EVENT_ACS_ILLEGAL_MESSAGE
		}

		// Token: 0x0200025B RID: 603
		public enum ENUM_UPGRADE_TYPE
		{
			// Token: 0x04002587 RID: 9607
			ENUM_UPGRADE_DVR,
			// Token: 0x04002588 RID: 9608
			ENUM_UPGRADE_ACS
		}

		// Token: 0x0200025C RID: 604
		// (Invoke) Token: 0x06001309 RID: 4873
		public delegate void LoginResultCallBack(int lUserID, uint dwResult, ref CHCNetSDK.NET_DVR_DEVICEINFO_V30 lpDeviceInfo, IntPtr pUser);

		// Token: 0x0200025D RID: 605
		public enum LONG_CFG_RECV_DATA_TYPE_ENUM
		{
			// Token: 0x0400258A RID: 9610
			ENUM_ACS_RECV_DATA = 3,
			// Token: 0x0400258B RID: 9611
			ENUM_DVR_ERROR_CODE = 1,
			// Token: 0x0400258C RID: 9612
			ENUM_MSC_RECV_DATA
		}

		// Token: 0x0200025E RID: 606
		public enum LONG_CFG_SEND_DATA_TYPE_ENUM
		{
			// Token: 0x0400258E RID: 9614
			ENUM_ACS_INTELLIGENT_IDENTITY_DATA = 9,
			// Token: 0x0400258F RID: 9615
			ENUM_ACS_SEND_DATA = 3,
			// Token: 0x04002590 RID: 9616
			ENUM_CVR_PASSBACK_SEND_DATA = 8,
			// Token: 0x04002591 RID: 9617
			ENUM_DVR_DEBUG_CMD = 6,
			// Token: 0x04002592 RID: 9618
			ENUM_DVR_SCREEN_CTRL_CMD,
			// Token: 0x04002593 RID: 9619
			ENUM_DVR_VEHICLE_CHECK = 1,
			// Token: 0x04002594 RID: 9620
			ENUM_MSC_SEND_DATA,
			// Token: 0x04002595 RID: 9621
			ENUM_SEND_JSON_DATA = 11,
			// Token: 0x04002596 RID: 9622
			ENUM_TME_CARD_SEND_DATA = 4,
			// Token: 0x04002597 RID: 9623
			ENUM_TME_VEHICLE_SEND_DATA,
			// Token: 0x04002598 RID: 9624
			ENUM_VIDEO_INTERCOM_SEND_DATA = 10
		}

		// Token: 0x0200025F RID: 607
		// (Invoke) Token: 0x0600130D RID: 4877
		public delegate void MSGCallBack(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser);

		// Token: 0x02000260 RID: 608
		// (Invoke) Token: 0x06001311 RID: 4881
		public delegate bool MSGCallBack_V31(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser);

		// Token: 0x02000261 RID: 609
		public struct NET_DVR_ACS_ALARM_INFO
		{
			// Token: 0x04002599 RID: 9625
			public uint dwSize;

			// Token: 0x0400259A RID: 9626
			public uint dwMajor;

			// Token: 0x0400259B RID: 9627
			public uint dwMinor;

			// Token: 0x0400259C RID: 9628
			public CHCNetSDK.NET_DVR_TIME struTime;

			// Token: 0x0400259D RID: 9629
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sNetUser;

			// Token: 0x0400259E RID: 9630
			public CHCNetSDK.NET_DVR_IPADDR struRemoteHostAddr;

			// Token: 0x0400259F RID: 9631
			public CHCNetSDK.NET_DVR_ACS_EVENT_INFO struAcsEventInfo;

			// Token: 0x040025A0 RID: 9632
			public uint dwPicDataLen;

			// Token: 0x040025A1 RID: 9633
			public IntPtr pPicData;

			// Token: 0x040025A2 RID: 9634
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000262 RID: 610
		public struct NET_DVR_ACS_EVENT_CFG
		{
			// Token: 0x040025A3 RID: 9635
			public uint dwSize;

			// Token: 0x040025A4 RID: 9636
			public uint dwMajor;

			// Token: 0x040025A5 RID: 9637
			public uint dwMinor;

			// Token: 0x040025A6 RID: 9638
			public CHCNetSDK.NET_DVR_TIME struTime;

			// Token: 0x040025A7 RID: 9639
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] sNetUser;

			// Token: 0x040025A8 RID: 9640
			public CHCNetSDK.NET_DVR_IPADDR struRemoteHostAddr;

			// Token: 0x040025A9 RID: 9641
			public CHCNetSDK.NET_DVR_ACS_EVENT_DETAIL struAcsEventInfo;

			// Token: 0x040025AA RID: 9642
			public uint dwPicDataLen;

			// Token: 0x040025AB RID: 9643
			public IntPtr pPicData;

			// Token: 0x040025AC RID: 9644
			public ushort wInductiveEventType;

			// Token: 0x040025AD RID: 9645
			public byte byTimeType;

			// Token: 0x040025AE RID: 9646
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 61)]
			public byte[] byRes;
		}

		// Token: 0x02000263 RID: 611
		public struct NET_DVR_ACS_EVENT_COND
		{
			// Token: 0x06001314 RID: 4884 RVA: 0x0016BA80 File Offset: 0x0016AA80
			public void Init()
			{
				this.byCardNo = new byte[32];
				this.byName = new byte[32];
				this.byRes2 = new byte[2];
				this.byEmployeeNo = new byte[32];
				this.byRes = new byte[140];
			}

			// Token: 0x040025AF RID: 9647
			public uint dwSize;

			// Token: 0x040025B0 RID: 9648
			public uint dwMajor;

			// Token: 0x040025B1 RID: 9649
			public uint dwMinor;

			// Token: 0x040025B2 RID: 9650
			public CHCNetSDK.NET_DVR_TIME struStartTime;

			// Token: 0x040025B3 RID: 9651
			public CHCNetSDK.NET_DVR_TIME struEndTime;

			// Token: 0x040025B4 RID: 9652
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040025B5 RID: 9653
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byName;

			// Token: 0x040025B6 RID: 9654
			public uint dwBeginSerialNo;

			// Token: 0x040025B7 RID: 9655
			public byte byPicEnable;

			// Token: 0x040025B8 RID: 9656
			public byte byTimeType;

			// Token: 0x040025B9 RID: 9657
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x040025BA RID: 9658
			public uint dwEndSerialNo;

			// Token: 0x040025BB RID: 9659
			public uint dwIOTChannelNo;

			// Token: 0x040025BC RID: 9660
			public ushort wInductiveEventType;

			// Token: 0x040025BD RID: 9661
			public byte bySearchType;

			// Token: 0x040025BE RID: 9662
			public byte byRes1;

			// Token: 0x040025BF RID: 9663
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string szMonitorID;

			// Token: 0x040025C0 RID: 9664
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byEmployeeNo;

			// Token: 0x040025C1 RID: 9665
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 140, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000264 RID: 612
		public struct NET_DVR_ACS_EVENT_DETAIL
		{
			// Token: 0x040025C2 RID: 9666
			public uint dwSize;

			// Token: 0x040025C3 RID: 9667
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] byCardNo;

			// Token: 0x040025C4 RID: 9668
			public byte byCardType;

			// Token: 0x040025C5 RID: 9669
			public byte byWhiteListNo;

			// Token: 0x040025C6 RID: 9670
			public byte byReportChannel;

			// Token: 0x040025C7 RID: 9671
			public byte byCardReaderKind;

			// Token: 0x040025C8 RID: 9672
			public uint dwCardReaderNo;

			// Token: 0x040025C9 RID: 9673
			public uint dwDoorNo;

			// Token: 0x040025CA RID: 9674
			public uint dwVerifyNo;

			// Token: 0x040025CB RID: 9675
			public uint dwAlarmInNo;

			// Token: 0x040025CC RID: 9676
			public uint dwAlarmOutNo;

			// Token: 0x040025CD RID: 9677
			public uint dwCaseSensorNo;

			// Token: 0x040025CE RID: 9678
			public uint dwRs485No;

			// Token: 0x040025CF RID: 9679
			public uint dwMultiCardGroupNo;

			// Token: 0x040025D0 RID: 9680
			public ushort wAccessChannel;

			// Token: 0x040025D1 RID: 9681
			public byte byDeviceNo;

			// Token: 0x040025D2 RID: 9682
			public byte byDistractControlNo;

			// Token: 0x040025D3 RID: 9683
			public uint dwEmployeeNo;

			// Token: 0x040025D4 RID: 9684
			public ushort wLocalControllerID;

			// Token: 0x040025D5 RID: 9685
			public byte byInternetAccess;

			// Token: 0x040025D6 RID: 9686
			public byte byType;

			// Token: 0x040025D7 RID: 9687
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public byte[] byMACAddr;

			// Token: 0x040025D8 RID: 9688
			public byte bySwipeCardType;

			// Token: 0x040025D9 RID: 9689
			public byte byRes2;

			// Token: 0x040025DA RID: 9690
			public uint dwSerialNo;

			// Token: 0x040025DB RID: 9691
			public byte byChannelControllerID;

			// Token: 0x040025DC RID: 9692
			public byte byChannelControllerLampID;

			// Token: 0x040025DD RID: 9693
			public byte byChannelControllerIRAdaptorID;

			// Token: 0x040025DE RID: 9694
			public byte byChannelControllerIREmitterID;

			// Token: 0x040025DF RID: 9695
			public uint dwRecordChannelNum;

			// Token: 0x040025E0 RID: 9696
			public uint pRecordChannelData;

			// Token: 0x040025E1 RID: 9697
			public byte byUserType;

			// Token: 0x040025E2 RID: 9698
			public byte byCurrentVerifyMode;

			// Token: 0x040025E3 RID: 9699
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] byRe2;

			// Token: 0x040025E4 RID: 9700
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] byEmployeeNo;

			// Token: 0x040025E5 RID: 9701
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] byRes;
		}

		// Token: 0x02000265 RID: 613
		public struct NET_DVR_ACS_EVENT_INFO
		{
			// Token: 0x040025E6 RID: 9702
			public uint dwSize;

			// Token: 0x040025E7 RID: 9703
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040025E8 RID: 9704
			public byte byCardType;

			// Token: 0x040025E9 RID: 9705
			public byte byWhiteListNo;

			// Token: 0x040025EA RID: 9706
			public byte byReportChannel;

			// Token: 0x040025EB RID: 9707
			public byte byCardReaderKind;

			// Token: 0x040025EC RID: 9708
			public uint dwCardReaderNo;

			// Token: 0x040025ED RID: 9709
			public uint dwDoorNo;

			// Token: 0x040025EE RID: 9710
			public uint dwVerifyNo;

			// Token: 0x040025EF RID: 9711
			public uint dwAlarmInNo;

			// Token: 0x040025F0 RID: 9712
			public uint dwAlarmOutNo;

			// Token: 0x040025F1 RID: 9713
			public uint dwCaseSensorNo;

			// Token: 0x040025F2 RID: 9714
			public uint dwRs485No;

			// Token: 0x040025F3 RID: 9715
			public uint dwMultiCardGroupNo;

			// Token: 0x040025F4 RID: 9716
			public ushort wAccessChannel;

			// Token: 0x040025F5 RID: 9717
			public byte byDeviceNo;

			// Token: 0x040025F6 RID: 9718
			public byte byDistractControlNo;

			// Token: 0x040025F7 RID: 9719
			public uint dwEmployeeNo;

			// Token: 0x040025F8 RID: 9720
			public ushort wLocalControllerID;

			// Token: 0x040025F9 RID: 9721
			public byte byInternetAccess;

			// Token: 0x040025FA RID: 9722
			public byte byType;

			// Token: 0x040025FB RID: 9723
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMACAddr;

			// Token: 0x040025FC RID: 9724
			public byte bySwipeCardType;

			// Token: 0x040025FD RID: 9725
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 13, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000266 RID: 614
		public struct NET_DVR_ACS_PARAM_TYPE
		{
			// Token: 0x040025FE RID: 9726
			public uint dwSize;

			// Token: 0x040025FF RID: 9727
			public uint dwParamType;

			// Token: 0x04002600 RID: 9728
			public ushort wLocalControllerID;

			// Token: 0x04002601 RID: 9729
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
			public byte[] byRes;
		}

		// Token: 0x02000267 RID: 615
		public struct NET_DVR_ACS_WORK_STATUS_V50
		{
			// Token: 0x06001315 RID: 4885 RVA: 0x0016BAD0 File Offset: 0x0016AAD0
			public void Init()
			{
				this.byDoorLockStatus = new byte[256];
				this.byDoorStatus = new byte[256];
				this.byMagneticStatus = new byte[256];
				this.byCaseStatus = new byte[8];
				this.byCardReaderOnlineStatus = new byte[512];
				this.byCardReaderAntiDismantleStatus = new byte[512];
				this.byCardReaderVerifyMode = new byte[512];
				this.bySetupAlarmStatus = new byte[512];
				this.byAlarmInStatus = new byte[512];
				this.byAlarmOutStatus = new byte[512];
				this.byRes2 = new byte[123];
			}

			// Token: 0x04002602 RID: 9730
			public uint dwSize;

			// Token: 0x04002603 RID: 9731
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byDoorLockStatus;

			// Token: 0x04002604 RID: 9732
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byDoorStatus;

			// Token: 0x04002605 RID: 9733
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byMagneticStatus;

			// Token: 0x04002606 RID: 9734
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byCaseStatus;

			// Token: 0x04002607 RID: 9735
			public ushort wBatteryVoltage;

			// Token: 0x04002608 RID: 9736
			public byte byBatteryLowVoltage;

			// Token: 0x04002609 RID: 9737
			public byte byPowerSupplyStatus;

			// Token: 0x0400260A RID: 9738
			public byte byMultiDoorInterlockStatus;

			// Token: 0x0400260B RID: 9739
			public byte byAntiSneakStatus;

			// Token: 0x0400260C RID: 9740
			public byte byHostAntiDismantleStatus;

			// Token: 0x0400260D RID: 9741
			public byte byIndicatorLightStatus;

			// Token: 0x0400260E RID: 9742
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardReaderOnlineStatus;

			// Token: 0x0400260F RID: 9743
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardReaderAntiDismantleStatus;

			// Token: 0x04002610 RID: 9744
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardReaderVerifyMode;

			// Token: 0x04002611 RID: 9745
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] bySetupAlarmStatus;

			// Token: 0x04002612 RID: 9746
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmInStatus;

			// Token: 0x04002613 RID: 9747
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmOutStatus;

			// Token: 0x04002614 RID: 9748
			public uint dwCardNum;

			// Token: 0x04002615 RID: 9749
			public byte byFireAlarmStatus;

			// Token: 0x04002616 RID: 9750
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 123, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x02000268 RID: 616
		public struct NET_DVR_ALARM_DEVICE_USER
		{
			// Token: 0x04002617 RID: 9751
			public uint dwSize;

			// Token: 0x04002618 RID: 9752
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x04002619 RID: 9753
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x0400261A RID: 9754
			public CHCNetSDK.NET_DVR_IPADDR struUserIP;

			// Token: 0x0400261B RID: 9755
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byAMCAddr;

			// Token: 0x0400261C RID: 9756
			public byte byUserType;

			// Token: 0x0400261D RID: 9757
			public byte byAlarmOnRight;

			// Token: 0x0400261E RID: 9758
			public byte byAlarmOffRight;

			// Token: 0x0400261F RID: 9759
			public byte byBypassRight;

			// Token: 0x04002620 RID: 9760
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byOtherRight;

			// Token: 0x04002621 RID: 9761
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byNetPreviewRight;

			// Token: 0x04002622 RID: 9762
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byNetRecordRight;

			// Token: 0x04002623 RID: 9763
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byNetPlaybackRight;

			// Token: 0x04002624 RID: 9764
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byNetPTZRight;

			// Token: 0x04002625 RID: 9765
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sOriginalPassword;

			// Token: 0x04002626 RID: 9766
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 152, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x02000269 RID: 617
		public struct NET_DVR_ALARMER
		{
			// Token: 0x04002627 RID: 9767
			public byte byUserIDValid;

			// Token: 0x04002628 RID: 9768
			public byte bySerialValid;

			// Token: 0x04002629 RID: 9769
			public byte byVersionValid;

			// Token: 0x0400262A RID: 9770
			public byte byDeviceNameValid;

			// Token: 0x0400262B RID: 9771
			public byte byMacAddrValid;

			// Token: 0x0400262C RID: 9772
			public byte byLinkPortValid;

			// Token: 0x0400262D RID: 9773
			public byte byDeviceIPValid;

			// Token: 0x0400262E RID: 9774
			public byte bySocketIPValid;

			// Token: 0x0400262F RID: 9775
			public int lUserID;

			// Token: 0x04002630 RID: 9776
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sSerialNumber;

			// Token: 0x04002631 RID: 9777
			public uint dwDeviceVersion;

			// Token: 0x04002632 RID: 9778
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sDeviceName;

			// Token: 0x04002633 RID: 9779
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMacAddr;

			// Token: 0x04002634 RID: 9780
			public ushort wLinkPort;

			// Token: 0x04002635 RID: 9781
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string sDeviceIP;

			// Token: 0x04002636 RID: 9782
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string sSocketIP;

			// Token: 0x04002637 RID: 9783
			public byte byIpProtocol;

			// Token: 0x04002638 RID: 9784
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200026A RID: 618
		public struct NET_DVR_CAPTURE_FACE_CFG
		{
			// Token: 0x04002639 RID: 9785
			public uint dwSize;

			// Token: 0x0400263A RID: 9786
			public uint dwFaceTemplate1Size;

			// Token: 0x0400263B RID: 9787
			public IntPtr pFaceTemplate1Buffer;

			// Token: 0x0400263C RID: 9788
			public uint dwFaceTemplate2Size;

			// Token: 0x0400263D RID: 9789
			public IntPtr pFaceTemplate2Buffer;

			// Token: 0x0400263E RID: 9790
			public uint dwFacePicSize;

			// Token: 0x0400263F RID: 9791
			public IntPtr pFacePicBuffer;

			// Token: 0x04002640 RID: 9792
			public byte byFaceQuality1;

			// Token: 0x04002641 RID: 9793
			public byte byFaceQuality2;

			// Token: 0x04002642 RID: 9794
			public byte byCaptureProgress;

			// Token: 0x04002643 RID: 9795
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 125, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200026B RID: 619
		public struct NET_DVR_CAPTURE_FACE_COND
		{
			// Token: 0x04002644 RID: 9796
			public uint dwSize;

			// Token: 0x04002645 RID: 9797
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200026C RID: 620
		public struct NET_DVR_CARD_CFG_COND
		{
			// Token: 0x04002646 RID: 9798
			public uint dwSize;

			// Token: 0x04002647 RID: 9799
			public uint dwCardNum;

			// Token: 0x04002648 RID: 9800
			public byte byCheckCardNo;

			// Token: 0x04002649 RID: 9801
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x0400264A RID: 9802
			public ushort wLocalControllerID;

			// Token: 0x0400264B RID: 9803
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x0400264C RID: 9804
			public uint dwLockID;

			// Token: 0x0400264D RID: 9805
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;
		}

		// Token: 0x0200026D RID: 621
		public struct NET_DVR_CARD_CFG_SEND_DATA
		{
			// Token: 0x0400264E RID: 9806
			public uint dwSize;

			// Token: 0x0400264F RID: 9807
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x04002650 RID: 9808
			public uint dwCardUserId;

			// Token: 0x04002651 RID: 9809
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200026E RID: 622
		public struct NET_DVR_CARD_CFG_V50
		{
			// Token: 0x06001316 RID: 4886 RVA: 0x0016BB88 File Offset: 0x0016AB88
			public void Init()
			{
				this.byDoorRight = new byte[256];
				this.byBelongGroup = new byte[128];
				this.wCardRightPlan = new ushort[1024];
				this.byCardNo = new byte[32];
				this.byCardPassword = new byte[8];
				this.byName = new byte[32];
				this.byRes2 = new byte[3];
				this.byLockCode = new byte[8];
				this.byRoomCode = new byte[8];
				this.byRes3 = new byte[83];
			}

			// Token: 0x04002652 RID: 9810
			public uint dwSize;

			// Token: 0x04002653 RID: 9811
			public uint dwModifyParamType;

			// Token: 0x04002654 RID: 9812
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x04002655 RID: 9813
			public byte byCardValid;

			// Token: 0x04002656 RID: 9814
			public byte byCardType;

			// Token: 0x04002657 RID: 9815
			public byte byLeaderCard;

			// Token: 0x04002658 RID: 9816
			public byte byRes1;

			// Token: 0x04002659 RID: 9817
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byDoorRight;

			// Token: 0x0400265A RID: 9818
			public CHCNetSDK.NET_DVR_VALID_PERIOD_CFG struValid;

			// Token: 0x0400265B RID: 9819
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] byBelongGroup;

			// Token: 0x0400265C RID: 9820
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardPassword;

			// Token: 0x0400265D RID: 9821
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024, ArraySubType = UnmanagedType.I1)]
			public ushort[] wCardRightPlan;

			// Token: 0x0400265E RID: 9822
			public uint dwMaxSwipeTime;

			// Token: 0x0400265F RID: 9823
			public uint dwSwipeTime;

			// Token: 0x04002660 RID: 9824
			public ushort wRoomNumber;

			// Token: 0x04002661 RID: 9825
			public short wFloorNumber;

			// Token: 0x04002662 RID: 9826
			public uint dwEmployeeNo;

			// Token: 0x04002663 RID: 9827
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byName;

			// Token: 0x04002664 RID: 9828
			public ushort wDepartmentNo;

			// Token: 0x04002665 RID: 9829
			public ushort wSchedulePlanNo;

			// Token: 0x04002666 RID: 9830
			public byte bySchedulePlanType;

			// Token: 0x04002667 RID: 9831
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x04002668 RID: 9832
			public uint dwLockID;

			// Token: 0x04002669 RID: 9833
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byLockCode;

			// Token: 0x0400266A RID: 9834
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRoomCode;

			// Token: 0x0400266B RID: 9835
			public uint dwCardRight;

			// Token: 0x0400266C RID: 9836
			public uint dwPlanTemplate;

			// Token: 0x0400266D RID: 9837
			public uint dwCardUserId;

			// Token: 0x0400266E RID: 9838
			public byte byCardModelType;

			// Token: 0x0400266F RID: 9839
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 83, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;
		}

		// Token: 0x0200026F RID: 623
		public struct NET_DVR_CARD_READER_CFG_V50
		{
			// Token: 0x06001317 RID: 4887 RVA: 0x0016BC1C File Offset: 0x0016AC1C
			public void Init()
			{
				this.byCardReaderDescription = new byte[32];
				this.byRes = new byte[254];
			}

			// Token: 0x04002670 RID: 9840
			public uint dwSize;

			// Token: 0x04002671 RID: 9841
			public byte byEnable;

			// Token: 0x04002672 RID: 9842
			public byte byCardReaderType;

			// Token: 0x04002673 RID: 9843
			public byte byOkLedPolarity;

			// Token: 0x04002674 RID: 9844
			public byte byErrorLedPolarity;

			// Token: 0x04002675 RID: 9845
			public byte byBuzzerPolarity;

			// Token: 0x04002676 RID: 9846
			public byte bySwipeInterval;

			// Token: 0x04002677 RID: 9847
			public byte byPressTimeout;

			// Token: 0x04002678 RID: 9848
			public byte byEnableFailAlarm;

			// Token: 0x04002679 RID: 9849
			public byte byMaxReadCardFailNum;

			// Token: 0x0400267A RID: 9850
			public byte byEnableTamperCheck;

			// Token: 0x0400267B RID: 9851
			public byte byOfflineCheckTime;

			// Token: 0x0400267C RID: 9852
			public byte byFingerPrintCheckLevel;

			// Token: 0x0400267D RID: 9853
			public byte byUseLocalController;

			// Token: 0x0400267E RID: 9854
			public byte byRes1;

			// Token: 0x0400267F RID: 9855
			public ushort wLocalControllerID;

			// Token: 0x04002680 RID: 9856
			public ushort wLocalControllerReaderID;

			// Token: 0x04002681 RID: 9857
			public ushort wCardReaderChannel;

			// Token: 0x04002682 RID: 9858
			public byte byFingerPrintImageQuality;

			// Token: 0x04002683 RID: 9859
			public byte byFingerPrintContrastTimeOut;

			// Token: 0x04002684 RID: 9860
			public byte byFingerPrintRecogizeInterval;

			// Token: 0x04002685 RID: 9861
			public byte byFingerPrintMatchFastMode;

			// Token: 0x04002686 RID: 9862
			public byte byFingerPrintModuleSensitive;

			// Token: 0x04002687 RID: 9863
			public byte byFingerPrintModuleLightCondition;

			// Token: 0x04002688 RID: 9864
			public byte byFaceMatchThresholdN;

			// Token: 0x04002689 RID: 9865
			public byte byFaceQuality;

			// Token: 0x0400268A RID: 9866
			public byte byFaceRecogizeTimeOut;

			// Token: 0x0400268B RID: 9867
			public byte byFaceRecogizeInterval;

			// Token: 0x0400268C RID: 9868
			public ushort wCardReaderFunction;

			// Token: 0x0400268D RID: 9869
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardReaderDescription;

			// Token: 0x0400268E RID: 9870
			public ushort wFaceImageSensitometry;

			// Token: 0x0400268F RID: 9871
			public byte byLivingBodyDetect;

			// Token: 0x04002690 RID: 9872
			public byte byFaceMatchThreshold1;

			// Token: 0x04002691 RID: 9873
			public ushort wBuzzerTime;

			// Token: 0x04002692 RID: 9874
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 254, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000270 RID: 624
		public struct NET_DVR_CARD_READER_PLAN
		{
			// Token: 0x06001318 RID: 4888 RVA: 0x0016BC3B File Offset: 0x0016AC3B
			public void Init()
			{
				this.byRes = new byte[64];
			}

			// Token: 0x04002693 RID: 9875
			public uint dwSize;

			// Token: 0x04002694 RID: 9876
			public uint dwTemplateNo;

			// Token: 0x04002695 RID: 9877
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] byRes;
		}

		// Token: 0x02000271 RID: 625
		public struct NET_DVR_CARD_USER_INFO_CFG
		{
			// Token: 0x04002696 RID: 9878
			public uint dwSize;

			// Token: 0x04002697 RID: 9879
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] byUsername;

			// Token: 0x04002698 RID: 9880
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			public byte[] byRes2;
		}

		// Token: 0x02000272 RID: 626
		public struct NET_DVR_CLIENTINFO
		{
			// Token: 0x04002699 RID: 9881
			public int lChannel;

			// Token: 0x0400269A RID: 9882
			public uint lLinkMode;

			// Token: 0x0400269B RID: 9883
			public IntPtr hPlayWnd;

			// Token: 0x0400269C RID: 9884
			public string sMultiCastIP;
		}

		// Token: 0x02000273 RID: 627
		public struct NET_DVR_DATE
		{
			// Token: 0x0400269D RID: 9885
			public ushort wYear;

			// Token: 0x0400269E RID: 9886
			public byte byMonth;

			// Token: 0x0400269F RID: 9887
			public byte byDay;
		}

		// Token: 0x02000274 RID: 628
		public struct NET_DVR_DDNS_STREAM_CFG
		{
			// Token: 0x06001319 RID: 4889 RVA: 0x0016BC4C File Offset: 0x0016AC4C
			public void Init()
			{
				this.byRes1 = new byte[3];
				this.byRes3 = new byte[2];
				this.sDVRSerialNumber = new byte[48];
				this.sUserName = new byte[32];
				this.sPassWord = new byte[16];
				this.byRes4 = new byte[2];
			}

			// Token: 0x040026A0 RID: 9888
			public byte byEnable;

			// Token: 0x040026A1 RID: 9889
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040026A2 RID: 9890
			private CHCNetSDK.NET_DVR_IPADDR struStreamServer;

			// Token: 0x040026A3 RID: 9891
			public ushort wStreamServerPort;

			// Token: 0x040026A4 RID: 9892
			public byte byStreamServerTransmitType;

			// Token: 0x040026A5 RID: 9893
			public byte byRes2;

			// Token: 0x040026A6 RID: 9894
			private CHCNetSDK.NET_DVR_IPADDR struIPServer;

			// Token: 0x040026A7 RID: 9895
			public ushort wIPServerPort;

			// Token: 0x040026A8 RID: 9896
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;

			// Token: 0x040026A9 RID: 9897
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sDVRName;

			// Token: 0x040026AA RID: 9898
			public ushort wDVRNameLen;

			// Token: 0x040026AB RID: 9899
			public ushort wDVRSerialLen;

			// Token: 0x040026AC RID: 9900
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sDVRSerialNumber;

			// Token: 0x040026AD RID: 9901
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x040026AE RID: 9902
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassWord;

			// Token: 0x040026AF RID: 9903
			public ushort wDVRPort;

			// Token: 0x040026B0 RID: 9904
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes4;

			// Token: 0x040026B1 RID: 9905
			public byte byChannel;

			// Token: 0x040026B2 RID: 9906
			public byte byTransProtocol;

			// Token: 0x040026B3 RID: 9907
			public byte byTransMode;

			// Token: 0x040026B4 RID: 9908
			public byte byFactoryType;
		}

		// Token: 0x02000275 RID: 629
		public struct NET_DVR_DEV_CHAN_INFO
		{
			// Token: 0x0600131A RID: 4890 RVA: 0x0016BCA4 File Offset: 0x0016ACA4
			public void Init()
			{
				this.byRes = new byte[2];
				this.byDomain = new byte[64];
				this.sUserName = new byte[32];
				this.sPassword = new byte[16];
			}

			// Token: 0x040026B5 RID: 9909
			public CHCNetSDK.NET_DVR_IPADDR struIP;

			// Token: 0x040026B6 RID: 9910
			public ushort wDVRPort;

			// Token: 0x040026B7 RID: 9911
			public byte byChannel;

			// Token: 0x040026B8 RID: 9912
			public byte byTransProtocol;

			// Token: 0x040026B9 RID: 9913
			public byte byTransMode;

			// Token: 0x040026BA RID: 9914
			public byte byFactoryType;

			// Token: 0x040026BB RID: 9915
			public byte byDeviceType;

			// Token: 0x040026BC RID: 9916
			public byte byDispChan;

			// Token: 0x040026BD RID: 9917
			public byte bySubDispChan;

			// Token: 0x040026BE RID: 9918
			public byte byResolution;

			// Token: 0x040026BF RID: 9919
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x040026C0 RID: 9920
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byDomain;

			// Token: 0x040026C1 RID: 9921
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x040026C2 RID: 9922
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;
		}

		// Token: 0x02000276 RID: 630
		public struct NET_DVR_DEVICECFG_V40
		{
			// Token: 0x040026C3 RID: 9923
			public uint dwSize;

			// Token: 0x040026C4 RID: 9924
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] sDVRName;

			// Token: 0x040026C5 RID: 9925
			public uint dwDVRID;

			// Token: 0x040026C6 RID: 9926
			public uint dwRecycleRecord;

			// Token: 0x040026C7 RID: 9927
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
			public byte[] sSerialNumber;

			// Token: 0x040026C8 RID: 9928
			public uint dwSoftwareVersion;

			// Token: 0x040026C9 RID: 9929
			public uint dwSoftwareBuildDate;

			// Token: 0x040026CA RID: 9930
			public uint dwDSPSoftwareVersion;

			// Token: 0x040026CB RID: 9931
			public uint dwDSPSoftwareBuildDate;

			// Token: 0x040026CC RID: 9932
			public uint dwPanelVersion;

			// Token: 0x040026CD RID: 9933
			public uint dwHardwareVersion;

			// Token: 0x040026CE RID: 9934
			public byte byAlarmInPortNum;

			// Token: 0x040026CF RID: 9935
			public byte byAlarmOutPortNum;

			// Token: 0x040026D0 RID: 9936
			public byte byRS232Num;

			// Token: 0x040026D1 RID: 9937
			public byte byRS485Num;

			// Token: 0x040026D2 RID: 9938
			public byte byNetworkPortNum;

			// Token: 0x040026D3 RID: 9939
			public byte byDiskCtrlNum;

			// Token: 0x040026D4 RID: 9940
			public byte byDiskNum;

			// Token: 0x040026D5 RID: 9941
			public byte byDVRType;

			// Token: 0x040026D6 RID: 9942
			public byte byChanNum;

			// Token: 0x040026D7 RID: 9943
			public byte byStartChan;

			// Token: 0x040026D8 RID: 9944
			public byte byDecordChans;

			// Token: 0x040026D9 RID: 9945
			public byte byVGANum;

			// Token: 0x040026DA RID: 9946
			public byte byUSBNum;

			// Token: 0x040026DB RID: 9947
			public byte byAuxoutNum;

			// Token: 0x040026DC RID: 9948
			public byte byAudioNum;

			// Token: 0x040026DD RID: 9949
			public byte byIPChanNum;

			// Token: 0x040026DE RID: 9950
			public byte byZeroChanNum;

			// Token: 0x040026DF RID: 9951
			public byte bySupport;

			// Token: 0x040026E0 RID: 9952
			public byte byEsataUseage;

			// Token: 0x040026E1 RID: 9953
			public byte byIPCPlug;

			// Token: 0x040026E2 RID: 9954
			public byte byStorageMode;

			// Token: 0x040026E3 RID: 9955
			public byte bySupport1;

			// Token: 0x040026E4 RID: 9956
			public ushort wDevType;

			// Token: 0x040026E5 RID: 9957
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
			public byte[] byDevTypeName;

			// Token: 0x040026E6 RID: 9958
			public byte bySupport2;

			// Token: 0x040026E7 RID: 9959
			public byte byAnalogAlarmInPortNum;

			// Token: 0x040026E8 RID: 9960
			public byte byStartAlarmInNo;

			// Token: 0x040026E9 RID: 9961
			public byte byStartAlarmOutNo;

			// Token: 0x040026EA RID: 9962
			public byte byStartIPAlarmInNo;

			// Token: 0x040026EB RID: 9963
			public byte byStartIPAlarmOutNo;

			// Token: 0x040026EC RID: 9964
			public byte byHighIPChanNum;

			// Token: 0x040026ED RID: 9965
			public byte byEnableRemotePowerOn;

			// Token: 0x040026EE RID: 9966
			public ushort wDevClass;

			// Token: 0x040026EF RID: 9967
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public byte[] byRes2;
		}

		// Token: 0x02000277 RID: 631
		public struct NET_DVR_DEVICEINFO_V30
		{
			// Token: 0x040026F0 RID: 9968
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sSerialNumber;

			// Token: 0x040026F1 RID: 9969
			public byte byAlarmInPortNum;

			// Token: 0x040026F2 RID: 9970
			public byte byAlarmOutPortNum;

			// Token: 0x040026F3 RID: 9971
			public byte byDiskNum;

			// Token: 0x040026F4 RID: 9972
			public byte byDVRType;

			// Token: 0x040026F5 RID: 9973
			public byte byChanNum;

			// Token: 0x040026F6 RID: 9974
			public byte byStartChan;

			// Token: 0x040026F7 RID: 9975
			public byte byAudioChanNum;

			// Token: 0x040026F8 RID: 9976
			public byte byIPChanNum;

			// Token: 0x040026F9 RID: 9977
			public byte byZeroChanNum;

			// Token: 0x040026FA RID: 9978
			public byte byMainProto;

			// Token: 0x040026FB RID: 9979
			public byte bySubProto;

			// Token: 0x040026FC RID: 9980
			public byte bySupport;

			// Token: 0x040026FD RID: 9981
			public byte bySupport1;

			// Token: 0x040026FE RID: 9982
			public byte bySupport2;

			// Token: 0x040026FF RID: 9983
			public ushort wDevType;

			// Token: 0x04002700 RID: 9984
			public byte bySupport3;

			// Token: 0x04002701 RID: 9985
			public byte byMultiStreamProto;

			// Token: 0x04002702 RID: 9986
			public byte byStartDChan;

			// Token: 0x04002703 RID: 9987
			public byte byStartDTalkChan;

			// Token: 0x04002704 RID: 9988
			public byte byHighDChanNum;

			// Token: 0x04002705 RID: 9989
			public byte bySupport4;

			// Token: 0x04002706 RID: 9990
			public byte byLanguageType;

			// Token: 0x04002707 RID: 9991
			public byte byVoiceInChanNum;

			// Token: 0x04002708 RID: 9992
			public byte byStartVoiceInChanNo;

			// Token: 0x04002709 RID: 9993
			public byte bySupport5;

			// Token: 0x0400270A RID: 9994
			public byte bySupport6;

			// Token: 0x0400270B RID: 9995
			public byte byMirrorChanNum;

			// Token: 0x0400270C RID: 9996
			public ushort wStartMirrorChanNo;

			// Token: 0x0400270D RID: 9997
			public byte bySupport7;

			// Token: 0x0400270E RID: 9998
			public byte byRes2;
		}

		// Token: 0x02000278 RID: 632
		public struct NET_DVR_DEVICEINFO_V40
		{
			// Token: 0x0400270F RID: 9999
			public CHCNetSDK.NET_DVR_DEVICEINFO_V30 struDeviceV30;

			// Token: 0x04002710 RID: 10000
			public byte bySupportLock;

			// Token: 0x04002711 RID: 10001
			public byte byRetryLoginTime;

			// Token: 0x04002712 RID: 10002
			public byte byPasswordLevel;

			// Token: 0x04002713 RID: 10003
			public byte byProxyType;

			// Token: 0x04002714 RID: 10004
			public uint dwSurplusLockTime;

			// Token: 0x04002715 RID: 10005
			public byte byCharEncodeType;

			// Token: 0x04002716 RID: 10006
			public byte byRes1;

			// Token: 0x04002717 RID: 10007
			public byte bySupport;

			// Token: 0x04002718 RID: 10008
			public byte byRes;

			// Token: 0x04002719 RID: 10009
			public uint dwOEMCode;

			// Token: 0x0400271A RID: 10010
			public byte bySupportDev5;

			// Token: 0x0400271B RID: 10011
			public byte byLoginMode;

			// Token: 0x0400271C RID: 10012
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 246)]
			public byte[] byRes2;
		}

		// Token: 0x02000279 RID: 633
		public struct NET_DVR_DOOR_CFG
		{
			// Token: 0x0600131B RID: 4891 RVA: 0x0016BCD9 File Offset: 0x0016ACD9
			public void Init()
			{
				this.byDoorName = new byte[32];
				this.byStressPassword = new byte[8];
				this.bySuperPassword = new byte[8];
				this.byUnlockPassword = new byte[8];
				this.byRes2 = new byte[43];
			}

			// Token: 0x0400271D RID: 10013
			public uint dwSize;

			// Token: 0x0400271E RID: 10014
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byDoorName;

			// Token: 0x0400271F RID: 10015
			public byte byMagneticType;

			// Token: 0x04002720 RID: 10016
			public byte byOpenButtonType;

			// Token: 0x04002721 RID: 10017
			public byte byOpenDuration;

			// Token: 0x04002722 RID: 10018
			public byte byDisabledOpenDuration;

			// Token: 0x04002723 RID: 10019
			public byte byMagneticAlarmTimeout;

			// Token: 0x04002724 RID: 10020
			public byte byEnableDoorLock;

			// Token: 0x04002725 RID: 10021
			public byte byEnableLeaderCard;

			// Token: 0x04002726 RID: 10022
			public byte byLeaderCardMode;

			// Token: 0x04002727 RID: 10023
			public uint dwLeaderCardOpenDuration;

			// Token: 0x04002728 RID: 10024
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byStressPassword;

			// Token: 0x04002729 RID: 10025
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] bySuperPassword;

			// Token: 0x0400272A RID: 10026
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byUnlockPassword;

			// Token: 0x0400272B RID: 10027
			public byte byUseLocalController;

			// Token: 0x0400272C RID: 10028
			public byte byRes1;

			// Token: 0x0400272D RID: 10029
			public ushort wLocalControllerID;

			// Token: 0x0400272E RID: 10030
			public ushort wLocalControllerDoorNumber;

			// Token: 0x0400272F RID: 10031
			public ushort wLocalControllerStatus;

			// Token: 0x04002730 RID: 10032
			public byte byLockInputCheck;

			// Token: 0x04002731 RID: 10033
			public byte byLockInputType;

			// Token: 0x04002732 RID: 10034
			public byte byDoorTerminalMode;

			// Token: 0x04002733 RID: 10035
			public byte byOpenButton;

			// Token: 0x04002734 RID: 10036
			public byte byLadderControlDelayTime;

			// Token: 0x04002735 RID: 10037
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 43, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200027A RID: 634
		public struct NET_DVR_DOOR_STATUS_PLAN
		{
			// Token: 0x0600131C RID: 4892 RVA: 0x0016BD19 File Offset: 0x0016AD19
			public void Init()
			{
				this.byRes = new byte[64];
			}

			// Token: 0x04002736 RID: 10038
			public uint dwSize;

			// Token: 0x04002737 RID: 10039
			public uint dwTemplateNo;

			// Token: 0x04002738 RID: 10040
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] byRes;
		}

		// Token: 0x0200027B RID: 635
		public struct NET_DVR_ETHERNET_V30
		{
			// Token: 0x04002739 RID: 10041
			public CHCNetSDK.NET_DVR_IPADDR struDVRIP;

			// Token: 0x0400273A RID: 10042
			public CHCNetSDK.NET_DVR_IPADDR struDVRIPMask;

			// Token: 0x0400273B RID: 10043
			public uint dwNetInterface;

			// Token: 0x0400273C RID: 10044
			public ushort wDVRPort;

			// Token: 0x0400273D RID: 10045
			public ushort wMTU;

			// Token: 0x0400273E RID: 10046
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMACAddr;

			// Token: 0x0400273F RID: 10047
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200027C RID: 636
		public struct NET_DVR_EVENT_CARD_LINKAGE_CFG_V50
		{
			// Token: 0x0600131D RID: 4893 RVA: 0x0016BD28 File Offset: 0x0016AD28
			public void Init()
			{
				this.byRes1 = new byte[3];
				this.byAlarmout = new byte[512];
				this.byOpenDoor = new byte[256];
				this.byCloseDoor = new byte[256];
				this.byNormalOpen = new byte[256];
				this.byNormalClose = new byte[256];
				this.byRes3 = new byte[29];
				this.byReaderBuzzer = new byte[512];
				this.byAlarmOutClose = new byte[512];
				this.byAlarmInSetup = new byte[512];
				this.byAlarmInClose = new byte[512];
				this.byRes = new byte[500];
			}

			// Token: 0x04002740 RID: 10048
			public uint dwSize;

			// Token: 0x04002741 RID: 10049
			public byte byProMode;

			// Token: 0x04002742 RID: 10050
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04002743 RID: 10051
			public uint dwEventSourceID;

			// Token: 0x04002744 RID: 10052
			public CHCNetSDK.NET_DVR_EVETN_CARD_LINKAGE_UNION uLinkageInfo;

			// Token: 0x04002745 RID: 10053
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmout;

			// Token: 0x04002746 RID: 10054
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x04002747 RID: 10055
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byOpenDoor;

			// Token: 0x04002748 RID: 10056
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byCloseDoor;

			// Token: 0x04002749 RID: 10057
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byNormalOpen;

			// Token: 0x0400274A RID: 10058
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byNormalClose;

			// Token: 0x0400274B RID: 10059
			public byte byMainDevBuzzer;

			// Token: 0x0400274C RID: 10060
			public byte byCapturePic;

			// Token: 0x0400274D RID: 10061
			public byte byRecordVideo;

			// Token: 0x0400274E RID: 10062
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 29, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;

			// Token: 0x0400274F RID: 10063
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byReaderBuzzer;

			// Token: 0x04002750 RID: 10064
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmOutClose;

			// Token: 0x04002751 RID: 10065
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmInSetup;

			// Token: 0x04002752 RID: 10066
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmInClose;

			// Token: 0x04002753 RID: 10067
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 500, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200027D RID: 637
		public struct NET_DVR_EVENT_CARD_LINKAGE_CFG_V51
		{
			// Token: 0x0600131E RID: 4894 RVA: 0x0016BDF0 File Offset: 0x0016ADF0
			public void Init()
			{
				this.byRes1 = new byte[3];
				this.byAlarmout = new byte[512];
				this.byRes2 = new byte[32];
				this.byOpenDoor = new byte[256];
				this.byCloseDoor = new byte[256];
				this.byNormalOpen = new byte[256];
				this.byNormalClose = new byte[256];
				this.byRes3 = new byte[25];
				this.byReaderBuzzer = new byte[512];
				this.byAlarmOutClose = new byte[512];
				this.byAlarmInSetup = new byte[512];
				this.byAlarmInClose = new byte[512];
				this.byReaderStopBuzzer = new byte[512];
				this.byRes = new byte[512];
			}

			// Token: 0x04002754 RID: 10068
			public uint dwSize;

			// Token: 0x04002755 RID: 10069
			public byte byProMode;

			// Token: 0x04002756 RID: 10070
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04002757 RID: 10071
			public uint dwEventSourceID;

			// Token: 0x04002758 RID: 10072
			public CHCNetSDK.NET_DVR_EVETN_CARD_LINKAGE_UNION uLinkageInfo;

			// Token: 0x04002759 RID: 10073
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmout;

			// Token: 0x0400275A RID: 10074
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x0400275B RID: 10075
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byOpenDoor;

			// Token: 0x0400275C RID: 10076
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byCloseDoor;

			// Token: 0x0400275D RID: 10077
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byNormalOpen;

			// Token: 0x0400275E RID: 10078
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byNormalClose;

			// Token: 0x0400275F RID: 10079
			public byte byMainDevBuzzer;

			// Token: 0x04002760 RID: 10080
			public byte byCapturePic;

			// Token: 0x04002761 RID: 10081
			public byte byRecordVideo;

			// Token: 0x04002762 RID: 10082
			public byte byMainDevStopBuzzer;

			// Token: 0x04002763 RID: 10083
			public ushort wAudioDisplayID;

			// Token: 0x04002764 RID: 10084
			public byte byAudioDisplayMode;

			// Token: 0x04002765 RID: 10085
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 25, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;

			// Token: 0x04002766 RID: 10086
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byReaderBuzzer;

			// Token: 0x04002767 RID: 10087
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmOutClose;

			// Token: 0x04002768 RID: 10088
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmInSetup;

			// Token: 0x04002769 RID: 10089
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmInClose;

			// Token: 0x0400276A RID: 10090
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byReaderStopBuzzer;

			// Token: 0x0400276B RID: 10091
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200027E RID: 638
		public struct NET_DVR_EVENT_CARD_LINKAGE_COND
		{
			// Token: 0x0400276C RID: 10092
			public uint dwSize;

			// Token: 0x0400276D RID: 10093
			public uint dwEventID;

			// Token: 0x0400276E RID: 10094
			public ushort wLocalControllerID;

			// Token: 0x0400276F RID: 10095
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 106, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200027F RID: 639
		public struct NET_DVR_EVENT_LINKAGE_INFO
		{
			// Token: 0x04002770 RID: 10096
			public ushort wMainEventType;

			// Token: 0x04002771 RID: 10097
			public ushort wSubEventType;

			// Token: 0x04002772 RID: 10098
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000280 RID: 640
		[StructLayout(LayoutKind.Explicit)]
		public struct NET_DVR_EVETN_CARD_LINKAGE_UNION
		{
			// Token: 0x04002773 RID: 10099
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x04002774 RID: 10100
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byEmployeeNo;

			// Token: 0x04002775 RID: 10101
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMACAddr;

			// Token: 0x04002776 RID: 10102
			[FieldOffset(0)]
			public CHCNetSDK.NET_DVR_EVENT_LINKAGE_INFO struEventLinkage;
		}

		// Token: 0x02000281 RID: 641
		public struct NET_DVR_FACE_PARAM_BYCARD
		{
			// Token: 0x0600131F RID: 4895 RVA: 0x0016BED3 File Offset: 0x0016AED3
			public void Init()
			{
				this.byCardNo = new byte[32];
				this.byEnableCardReader = new byte[512];
				this.byFaceID = new byte[2];
				this.byRes1 = new byte[42];
			}

			// Token: 0x04002777 RID: 10103
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x04002778 RID: 10104
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCardReader;

			// Token: 0x04002779 RID: 10105
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byFaceID;

			// Token: 0x0400277A RID: 10106
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 42, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;
		}

		// Token: 0x02000282 RID: 642
		public struct NET_DVR_FACE_PARAM_BYREADER
		{
			// Token: 0x06001320 RID: 4896 RVA: 0x0016BF0B File Offset: 0x0016AF0B
			public void Init()
			{
				this.byRes1 = new byte[3];
				this.byCardNo = new byte[32];
				this.byRes = new byte[548];
			}

			// Token: 0x0400277B RID: 10107
			public int dwCardReaderNo;

			// Token: 0x0400277C RID: 10108
			public byte byClearAllCard;

			// Token: 0x0400277D RID: 10109
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x0400277E RID: 10110
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x0400277F RID: 10111
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 548, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000283 RID: 643
		public struct NET_DVR_FACE_PARAM_CFG
		{
			// Token: 0x06001321 RID: 4897 RVA: 0x0016BF36 File Offset: 0x0016AF36
			public void Init()
			{
				this.byCardNo = new byte[32];
				this.byEnableCardReader = new byte[512];
				this.byRes = new byte[126];
			}

			// Token: 0x04002780 RID: 10112
			public uint dwSize;

			// Token: 0x04002781 RID: 10113
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x04002782 RID: 10114
			public uint dwFaceLen;

			// Token: 0x04002783 RID: 10115
			public IntPtr pFaceBuffer;

			// Token: 0x04002784 RID: 10116
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCardReader;

			// Token: 0x04002785 RID: 10117
			public byte byFaceID;

			// Token: 0x04002786 RID: 10118
			public byte byFaceDataType;

			// Token: 0x04002787 RID: 10119
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 126, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000284 RID: 644
		public struct NET_DVR_FACE_PARAM_COND
		{
			// Token: 0x06001322 RID: 4898 RVA: 0x0016BF62 File Offset: 0x0016AF62
			public void Init()
			{
				this.byCardNo = new byte[32];
				this.byEnableCardReader = new byte[512];
				this.byRes = new byte[127];
			}

			// Token: 0x04002788 RID: 10120
			public uint dwSize;

			// Token: 0x04002789 RID: 10121
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x0400278A RID: 10122
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCardReader;

			// Token: 0x0400278B RID: 10123
			public uint dwFaceNum;

			// Token: 0x0400278C RID: 10124
			public byte byFaceID;

			// Token: 0x0400278D RID: 10125
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 127, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000285 RID: 645
		public struct NET_DVR_FACE_PARAM_CTRL_ByCard
		{
			// Token: 0x06001323 RID: 4899 RVA: 0x0016BF8E File Offset: 0x0016AF8E
			public void Init()
			{
				this.byRes1 = new byte[3];
				this.byRes = new byte[64];
				this.struProcessMode = default(CHCNetSDK.NET_DVR_FACE_PARAM_BYCARD);
				this.struProcessMode.Init();
			}

			// Token: 0x0400278E RID: 10126
			public int dwSize;

			// Token: 0x0400278F RID: 10127
			public byte byMode;

			// Token: 0x04002790 RID: 10128
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04002791 RID: 10129
			public CHCNetSDK.NET_DVR_FACE_PARAM_BYCARD struProcessMode;

			// Token: 0x04002792 RID: 10130
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000286 RID: 646
		public struct NET_DVR_FACE_PARAM_CTRL_ByReader
		{
			// Token: 0x06001324 RID: 4900 RVA: 0x0016BFC0 File Offset: 0x0016AFC0
			public void Init()
			{
				this.byRes1 = new byte[3];
				this.byRes = new byte[64];
				this.struProcessMode = default(CHCNetSDK.NET_DVR_FACE_PARAM_BYREADER);
				this.struProcessMode.Init();
			}

			// Token: 0x04002793 RID: 10131
			public int dwSize;

			// Token: 0x04002794 RID: 10132
			public byte byMode;

			// Token: 0x04002795 RID: 10133
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04002796 RID: 10134
			public CHCNetSDK.NET_DVR_FACE_PARAM_BYREADER struProcessMode;

			// Token: 0x04002797 RID: 10135
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000287 RID: 647
		public struct NET_DVR_FACE_PARAM_STATUS
		{
			// Token: 0x04002798 RID: 10136
			public uint dwSize;

			// Token: 0x04002799 RID: 10137
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x0400279A RID: 10138
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardReaderRecvStatus;

			// Token: 0x0400279B RID: 10139
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byErrorMsg;

			// Token: 0x0400279C RID: 10140
			public uint dwCardReaderNo;

			// Token: 0x0400279D RID: 10141
			public byte byTotalStatus;

			// Token: 0x0400279E RID: 10142
			public byte byFaceID;

			// Token: 0x0400279F RID: 10143
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 130, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000288 RID: 648
		public struct NET_DVR_FAILED_FACE_INFO
		{
			// Token: 0x040027A0 RID: 10144
			public int dwSize;

			// Token: 0x040027A1 RID: 10145
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] byCardNo;

			// Token: 0x040027A2 RID: 10146
			public byte byErrorCode;

			// Token: 0x040027A3 RID: 10147
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 127)]
			public byte[] byRes;
		}

		// Token: 0x02000289 RID: 649
		public struct NET_DVR_FINGER_PRINT_BYCARD
		{
			// Token: 0x040027A4 RID: 10148
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040027A5 RID: 10149
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCardReader;

			// Token: 0x040027A6 RID: 10150
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
			public byte[] byFingerPrintID;

			// Token: 0x040027A7 RID: 10151
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 34, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;
		}

		// Token: 0x0200028A RID: 650
		public struct NET_DVR_FINGER_PRINT_BYCARD_V50
		{
			// Token: 0x040027A8 RID: 10152
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040027A9 RID: 10153
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCardReader;

			// Token: 0x040027AA RID: 10154
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
			public byte[] byFingerPrintID;

			// Token: 0x040027AB RID: 10155
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040027AC RID: 10156
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byEmployeeNo;
		}

		// Token: 0x0200028B RID: 651
		public struct NET_DVR_FINGER_PRINT_BYREADER
		{
			// Token: 0x040027AD RID: 10157
			public uint dwCardReaderNo;

			// Token: 0x040027AE RID: 10158
			public byte byClearAllCard;

			// Token: 0x040027AF RID: 10159
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040027B0 RID: 10160
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040027B1 RID: 10161
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 548, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200028C RID: 652
		public struct NET_DVR_FINGER_PRINT_BYREADER_V50
		{
			// Token: 0x040027B2 RID: 10162
			public uint dwCardReaderNo;

			// Token: 0x040027B3 RID: 10163
			public byte byClearAllCard;

			// Token: 0x040027B4 RID: 10164
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040027B5 RID: 10165
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040027B6 RID: 10166
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byEmployeeNo;

			// Token: 0x040027B7 RID: 10167
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 516, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200028D RID: 653
		public struct NET_DVR_FINGER_PRINT_CFG
		{
			// Token: 0x06001325 RID: 4901 RVA: 0x0016BFF4 File Offset: 0x0016AFF4
			public void Init()
			{
				this.byCardNo = new byte[32];
				this.byEnableCardReader = new byte[512];
				this.byRes1 = new byte[30];
				this.byFingerData = new byte[768];
				this.byRes = new byte[64];
			}

			// Token: 0x040027B8 RID: 10168
			public uint dwSize;

			// Token: 0x040027B9 RID: 10169
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040027BA RID: 10170
			public uint dwFingerPrintLen;

			// Token: 0x040027BB RID: 10171
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCardReader;

			// Token: 0x040027BC RID: 10172
			public byte byFingerPrintID;

			// Token: 0x040027BD RID: 10173
			public byte byFingerType;

			// Token: 0x040027BE RID: 10174
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040027BF RID: 10175
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 768, ArraySubType = UnmanagedType.I1)]
			public byte[] byFingerData;

			// Token: 0x040027C0 RID: 10176
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200028E RID: 654
		public struct NET_DVR_FINGER_PRINT_CFG_V50
		{
			// Token: 0x06001326 RID: 4902 RVA: 0x0016C048 File Offset: 0x0016B048
			public void Init()
			{
				this.byCardNo = new byte[32];
				this.byEnableCardReader = new byte[512];
				this.byRes1 = new byte[30];
				this.byFingerData = new byte[768];
				this.byEmployeeNo = new byte[32];
				this.byLeaderFP = new byte[256];
				this.byRes = new byte[128];
			}

			// Token: 0x040027C1 RID: 10177
			public uint dwSize;

			// Token: 0x040027C2 RID: 10178
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040027C3 RID: 10179
			public uint dwFingerPrintLen;

			// Token: 0x040027C4 RID: 10180
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCardReader;

			// Token: 0x040027C5 RID: 10181
			public byte byFingerPrintID;

			// Token: 0x040027C6 RID: 10182
			public byte byFingerType;

			// Token: 0x040027C7 RID: 10183
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040027C8 RID: 10184
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 768, ArraySubType = UnmanagedType.I1)]
			public byte[] byFingerData;

			// Token: 0x040027C9 RID: 10185
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byEmployeeNo;

			// Token: 0x040027CA RID: 10186
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byLeaderFP;

			// Token: 0x040027CB RID: 10187
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200028F RID: 655
		public struct NET_DVR_FINGER_PRINT_INFO_COND
		{
			// Token: 0x06001327 RID: 4903 RVA: 0x0016C0BC File Offset: 0x0016B0BC
			public void Init()
			{
				this.byCardNo = new byte[32];
				this.byEnableCardReader = new byte[512];
				this.byRes1 = new byte[26];
			}

			// Token: 0x040027CC RID: 10188
			public uint dwSize;

			// Token: 0x040027CD RID: 10189
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040027CE RID: 10190
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCardReader;

			// Token: 0x040027CF RID: 10191
			public uint dwFingerPrintNum;

			// Token: 0x040027D0 RID: 10192
			public byte byFingerPrintID;

			// Token: 0x040027D1 RID: 10193
			public byte byCallbackMode;

			// Token: 0x040027D2 RID: 10194
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 26, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;
		}

		// Token: 0x02000290 RID: 656
		public struct NET_DVR_FINGER_PRINT_INFO_COND_V50
		{
			// Token: 0x06001328 RID: 4904 RVA: 0x0016C0E8 File Offset: 0x0016B0E8
			public void Init()
			{
				this.byCardNo = new byte[32];
				this.byEnableCardReader = new byte[512];
				this.byRes2 = new byte[2];
				this.byEmployeeNo = new byte[32];
				this.byRes1 = new byte[128];
			}

			// Token: 0x040027D3 RID: 10195
			public uint dwSize;

			// Token: 0x040027D4 RID: 10196
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040027D5 RID: 10197
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCardReader;

			// Token: 0x040027D6 RID: 10198
			public uint dwFingerPrintNum;

			// Token: 0x040027D7 RID: 10199
			public byte byFingerPrintID;

			// Token: 0x040027D8 RID: 10200
			public byte byCallbackMode;

			// Token: 0x040027D9 RID: 10201
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x040027DA RID: 10202
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byEmployeeNo;

			// Token: 0x040027DB RID: 10203
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;
		}

		// Token: 0x02000291 RID: 657
		public struct NET_DVR_FINGER_PRINT_INFO_CTRL_BYCARD
		{
			// Token: 0x040027DC RID: 10204
			public uint dwSize;

			// Token: 0x040027DD RID: 10205
			public byte byMode;

			// Token: 0x040027DE RID: 10206
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] byRes1;

			// Token: 0x040027DF RID: 10207
			public CHCNetSDK.NET_DVR_FINGER_PRINT_BYCARD struByCard;

			// Token: 0x040027E0 RID: 10208
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] byRes;
		}

		// Token: 0x02000292 RID: 658
		public struct NET_DVR_FINGER_PRINT_INFO_CTRL_BYCARD_V50
		{
			// Token: 0x040027E1 RID: 10209
			public uint dwSize;

			// Token: 0x040027E2 RID: 10210
			public byte byMode;

			// Token: 0x040027E3 RID: 10211
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] byRes1;

			// Token: 0x040027E4 RID: 10212
			public CHCNetSDK.NET_DVR_FINGER_PRINT_BYCARD_V50 struByCard;

			// Token: 0x040027E5 RID: 10213
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] byRes;
		}

		// Token: 0x02000293 RID: 659
		public struct NET_DVR_FINGER_PRINT_INFO_CTRL_BYREADER
		{
			// Token: 0x040027E6 RID: 10214
			public uint dwSize;

			// Token: 0x040027E7 RID: 10215
			public byte byMode;

			// Token: 0x040027E8 RID: 10216
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] byRes1;

			// Token: 0x040027E9 RID: 10217
			public CHCNetSDK.NET_DVR_FINGER_PRINT_BYREADER struByReader;

			// Token: 0x040027EA RID: 10218
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] byRes;
		}

		// Token: 0x02000294 RID: 660
		public struct NET_DVR_FINGER_PRINT_INFO_CTRL_BYREADER_V50
		{
			// Token: 0x040027EB RID: 10219
			public uint dwSize;

			// Token: 0x040027EC RID: 10220
			public byte byMode;

			// Token: 0x040027ED RID: 10221
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] byRes1;

			// Token: 0x040027EE RID: 10222
			public CHCNetSDK.NET_DVR_FINGER_PRINT_BYREADER_V50 struByReader;

			// Token: 0x040027EF RID: 10223
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] byRes;
		}

		// Token: 0x02000295 RID: 661
		public struct NET_DVR_FINGER_PRINT_INFO_STATUS_V50
		{
			// Token: 0x040027F0 RID: 10224
			public uint dwSize;

			// Token: 0x040027F1 RID: 10225
			public uint dwCardReaderNo;

			// Token: 0x040027F2 RID: 10226
			public byte byStatus;

			// Token: 0x040027F3 RID: 10227
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 63)]
			public byte[] byRes;
		}

		// Token: 0x02000296 RID: 662
		public struct NET_DVR_FINGER_PRINT_STATUS
		{
			// Token: 0x06001329 RID: 4905 RVA: 0x0016C13B File Offset: 0x0016B13B
			public void Init()
			{
				this.byCardNo = new byte[32];
				this.byCardReaderRecvStatus = new byte[512];
				this.byErrorMsg = new byte[32];
				this.byRes = new byte[24];
			}

			// Token: 0x040027F4 RID: 10228
			public uint dwSize;

			// Token: 0x040027F5 RID: 10229
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x040027F6 RID: 10230
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardReaderRecvStatus;

			// Token: 0x040027F7 RID: 10231
			public byte byFingerPrintID;

			// Token: 0x040027F8 RID: 10232
			public byte byFingerType;

			// Token: 0x040027F9 RID: 10233
			public byte byTotalStatus;

			// Token: 0x040027FA RID: 10234
			public byte byRes1;

			// Token: 0x040027FB RID: 10235
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byErrorMsg;

			// Token: 0x040027FC RID: 10236
			public uint dwCardReaderNo;

			// Token: 0x040027FD RID: 10237
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000297 RID: 663
		public struct NET_DVR_FINGER_PRINT_STATUS_V50
		{
			// Token: 0x0600132A RID: 4906 RVA: 0x0016C174 File Offset: 0x0016B174
			public void Init()
			{
				this.byCardNo = new byte[32];
				this.byCardReaderRecvStatus = new byte[512];
				this.byErrorMsg = new byte[32];
				this.byEmployeeNo = new byte[32];
				this.byErrorEmployeeNo = new byte[32];
				this.byRes = new byte[128];
			}

			// Token: 0x040027FE RID: 10238
			public uint dwSize;

			// Token: 0x040027FF RID: 10239
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardNo;

			// Token: 0x04002800 RID: 10240
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.I1)]
			public byte[] byCardReaderRecvStatus;

			// Token: 0x04002801 RID: 10241
			public byte byFingerPrintID;

			// Token: 0x04002802 RID: 10242
			public byte byFingerType;

			// Token: 0x04002803 RID: 10243
			public byte byTotalStatus;

			// Token: 0x04002804 RID: 10244
			public byte byRecvStatus;

			// Token: 0x04002805 RID: 10245
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byErrorMsg;

			// Token: 0x04002806 RID: 10246
			public uint dwCardReaderNo;

			// Token: 0x04002807 RID: 10247
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byEmployeeNo;

			// Token: 0x04002808 RID: 10248
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byErrorEmployeeNo;

			// Token: 0x04002809 RID: 10249
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000298 RID: 664
		public struct NET_DVR_GET_STREAM_UNION
		{
			// Token: 0x0400280A RID: 10250
			public CHCNetSDK.NET_DVR_IPCHANINFO struChanInfo;

			// Token: 0x0400280B RID: 10251
			public CHCNetSDK.NET_DVR_IPSERVER_STREAM struIPServerStream;

			// Token: 0x0400280C RID: 10252
			public CHCNetSDK.NET_DVR_PU_STREAM_CFG struPUStream;

			// Token: 0x0400280D RID: 10253
			public CHCNetSDK.NET_DVR_DDNS_STREAM_CFG struDDNSStream;

			// Token: 0x0400280E RID: 10254
			public CHCNetSDK.NET_DVR_PU_STREAM_URL struStreamUrl;

			// Token: 0x0400280F RID: 10255
			public CHCNetSDK.NET_DVR_HKDDNS_STREAM struHkDDNSStream;

			// Token: 0x04002810 RID: 10256
			public CHCNetSDK.NET_DVR_IPCHANINFO_V40 struIPChan;
		}

		// Token: 0x02000299 RID: 665
		public struct NET_DVR_GROUP_CFG
		{
			// Token: 0x04002811 RID: 10257
			public uint dwSize;

			// Token: 0x04002812 RID: 10258
			public byte byEnable;

			// Token: 0x04002813 RID: 10259
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04002814 RID: 10260
			public CHCNetSDK.NET_DVR_VALID_PERIOD_CFG struValidPeriodCfg;

			// Token: 0x04002815 RID: 10261
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byGroupName;

			// Token: 0x04002816 RID: 10262
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200029A RID: 666
		public struct NET_DVR_HKDDNS_STREAM
		{
			// Token: 0x0600132B RID: 4907 RVA: 0x0016C1D8 File Offset: 0x0016B1D8
			public void Init()
			{
				this.byRes = new byte[3];
				this.byDDNSDomain = new byte[64];
				this.byAlias = new byte[32];
				this.byRes1 = new byte[2];
				this.byDVRSerialNumber = new byte[48];
				this.byUserName = new byte[32];
				this.byPassWord = new byte[16];
				this.byRes2 = new byte[11];
			}

			// Token: 0x04002817 RID: 10263
			public byte byEnable;

			// Token: 0x04002818 RID: 10264
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x04002819 RID: 10265
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byDDNSDomain;

			// Token: 0x0400281A RID: 10266
			public ushort wPort;

			// Token: 0x0400281B RID: 10267
			public ushort wAliasLen;

			// Token: 0x0400281C RID: 10268
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlias;

			// Token: 0x0400281D RID: 10269
			public ushort wDVRSerialLen;

			// Token: 0x0400281E RID: 10270
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x0400281F RID: 10271
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] byDVRSerialNumber;

			// Token: 0x04002820 RID: 10272
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byUserName;

			// Token: 0x04002821 RID: 10273
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byPassWord;

			// Token: 0x04002822 RID: 10274
			public byte byChannel;

			// Token: 0x04002823 RID: 10275
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200029B RID: 667
		public struct NET_DVR_HOLIDAY_GROUP_CFG
		{
			// Token: 0x0600132C RID: 4908 RVA: 0x0016C24B File Offset: 0x0016B24B
			public void Init()
			{
				this.byGroupName = new byte[32];
				this.dwHolidayPlanNo = new uint[16];
				this.byRes1 = new byte[3];
				this.byRes2 = new byte[32];
			}

			// Token: 0x04002824 RID: 10276
			public uint dwSize;

			// Token: 0x04002825 RID: 10277
			public byte byEnable;

			// Token: 0x04002826 RID: 10278
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04002827 RID: 10279
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byGroupName;

			// Token: 0x04002828 RID: 10280
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U4)]
			public uint[] dwHolidayPlanNo;

			// Token: 0x04002829 RID: 10281
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200029C RID: 668
		public struct NET_DVR_HOLIDAY_GROUP_COND
		{
			// Token: 0x0600132D RID: 4909 RVA: 0x0016C280 File Offset: 0x0016B280
			public void Init()
			{
				this.byRes = new byte[106];
			}

			// Token: 0x0400282A RID: 10282
			public uint dwSize;

			// Token: 0x0400282B RID: 10283
			public uint dwHolidayGroupNumber;

			// Token: 0x0400282C RID: 10284
			public ushort wLocalControllerID;

			// Token: 0x0400282D RID: 10285
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 106, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200029D RID: 669
		public struct NET_DVR_HOLIDAY_PLAN_CFG
		{
			// Token: 0x0600132E RID: 4910 RVA: 0x0016C290 File Offset: 0x0016B290
			public void Init()
			{
				this.struPlanCfg = new CHCNetSDK.NET_DVR_SINGLE_PLAN_SEGMENT[8];
				foreach (CHCNetSDK.NET_DVR_SINGLE_PLAN_SEGMENT net_DVR_SINGLE_PLAN_SEGMENT in this.struPlanCfg)
				{
					net_DVR_SINGLE_PLAN_SEGMENT.Init();
				}
				this.byRes1 = new byte[3];
				this.byRes2 = new byte[16];
			}

			// Token: 0x0400282E RID: 10286
			public uint dwSize;

			// Token: 0x0400282F RID: 10287
			public byte byEnable;

			// Token: 0x04002830 RID: 10288
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04002831 RID: 10289
			public CHCNetSDK.NET_DVR_DATE struBeginDate;

			// Token: 0x04002832 RID: 10290
			public CHCNetSDK.NET_DVR_DATE struEndDate;

			// Token: 0x04002833 RID: 10291
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SINGLE_PLAN_SEGMENT[] struPlanCfg;

			// Token: 0x04002834 RID: 10292
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] byRes2;
		}

		// Token: 0x0200029E RID: 670
		public struct NET_DVR_HOLIDAY_PLAN_COND
		{
			// Token: 0x0600132F RID: 4911 RVA: 0x0016C2EB File Offset: 0x0016B2EB
			public void Init()
			{
				this.byRes = new byte[106];
			}

			// Token: 0x04002835 RID: 10293
			public uint dwSize;

			// Token: 0x04002836 RID: 10294
			public uint dwHolidayPlanNumber;

			// Token: 0x04002837 RID: 10295
			public ushort wLocalControllerID;

			// Token: 0x04002838 RID: 10296
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 106, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200029F RID: 671
		public struct NET_DVR_IPADDR
		{
			// Token: 0x06001330 RID: 4912 RVA: 0x0016C2FA File Offset: 0x0016B2FA
			public void Init()
			{
				this.byIPv6 = new byte[128];
			}

			// Token: 0x04002839 RID: 10297
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sIpV4;

			// Token: 0x0400283A RID: 10298
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] byIPv6;
		}

		// Token: 0x020002A0 RID: 672
		public struct NET_DVR_IPALARMINCFG
		{
			// Token: 0x0400283B RID: 10299
			public uint dwSize;

			// Token: 0x0400283C RID: 10300
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPALARMININFO[] struIPAlarmInInfo;
		}

		// Token: 0x020002A1 RID: 673
		public struct NET_DVR_IPALARMINCFG_V40
		{
			// Token: 0x0400283D RID: 10301
			public uint dwSize;

			// Token: 0x0400283E RID: 10302
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPALARMININFO_V40[] struIPAlarmInInfo;

			// Token: 0x0400283F RID: 10303
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002A2 RID: 674
		public struct NET_DVR_IPALARMININFO
		{
			// Token: 0x04002840 RID: 10304
			public byte byIPID;

			// Token: 0x04002841 RID: 10305
			public byte byAlarmIn;

			// Token: 0x04002842 RID: 10306
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002A3 RID: 675
		public struct NET_DVR_IPALARMININFO_V40
		{
			// Token: 0x04002843 RID: 10307
			public uint dwIPID;

			// Token: 0x04002844 RID: 10308
			public uint dwAlarmIn;

			// Token: 0x04002845 RID: 10309
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002A4 RID: 676
		public struct NET_DVR_IPALARMOUTCFG
		{
			// Token: 0x04002846 RID: 10310
			public uint dwSize;

			// Token: 0x04002847 RID: 10311
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPALARMOUTINFO[] struIPAlarmOutInfo;
		}

		// Token: 0x020002A5 RID: 677
		public struct NET_DVR_IPALARMOUTCFG_V40
		{
			// Token: 0x04002848 RID: 10312
			public uint dwSize;

			// Token: 0x04002849 RID: 10313
			public uint dwCurIPAlarmOutNum;

			// Token: 0x0400284A RID: 10314
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPALARMOUTINFO_V40[] struIPAlarmOutInfo;

			// Token: 0x0400284B RID: 10315
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002A6 RID: 678
		public struct NET_DVR_IPALARMOUTINFO
		{
			// Token: 0x0400284C RID: 10316
			public byte byIPID;

			// Token: 0x0400284D RID: 10317
			public byte byAlarmOut;

			// Token: 0x0400284E RID: 10318
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002A7 RID: 679
		public struct NET_DVR_IPALARMOUTINFO_V40
		{
			// Token: 0x0400284F RID: 10319
			public uint dwIPID;

			// Token: 0x04002850 RID: 10320
			public uint dwAlarmOut;

			// Token: 0x04002851 RID: 10321
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002A8 RID: 680
		public struct NET_DVR_IPCHANINFO
		{
			// Token: 0x04002852 RID: 10322
			public byte byEnable;

			// Token: 0x04002853 RID: 10323
			public byte byIPID;

			// Token: 0x04002854 RID: 10324
			public byte byChannel;

			// Token: 0x04002855 RID: 10325
			public byte byIPIDHigh;

			// Token: 0x04002856 RID: 10326
			public byte byTransProtocol;

			// Token: 0x04002857 RID: 10327
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 31, ArraySubType = UnmanagedType.I1)]
			public byte[] byres;
		}

		// Token: 0x020002A9 RID: 681
		public struct NET_DVR_IPCHANINFO_V40
		{
			// Token: 0x06001331 RID: 4913 RVA: 0x0016C30C File Offset: 0x0016B30C
			public void Init()
			{
				this.byRes = new byte[241];
			}

			// Token: 0x04002858 RID: 10328
			public byte byEnable;

			// Token: 0x04002859 RID: 10329
			public byte byRes1;

			// Token: 0x0400285A RID: 10330
			public ushort wIPID;

			// Token: 0x0400285B RID: 10331
			public uint dwChannel;

			// Token: 0x0400285C RID: 10332
			public byte byTransProtocol;

			// Token: 0x0400285D RID: 10333
			public byte byTransMode;

			// Token: 0x0400285E RID: 10334
			public byte byFactoryType;

			// Token: 0x0400285F RID: 10335
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 241, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002AA RID: 682
		public struct NET_DVR_IPDEVINFO_V31
		{
			// Token: 0x06001332 RID: 4914 RVA: 0x0016C320 File Offset: 0x0016B320
			public void Init()
			{
				this.sUserName = new byte[32];
				this.sPassword = new byte[16];
				this.byDomain = new byte[64];
				this.szDeviceID = new byte[32];
				this.byRes2 = new byte[2];
			}

			// Token: 0x04002860 RID: 10336
			public byte byEnable;

			// Token: 0x04002861 RID: 10337
			public byte byProType;

			// Token: 0x04002862 RID: 10338
			public byte byEnableQuickAdd;

			// Token: 0x04002863 RID: 10339
			public byte byRes1;

			// Token: 0x04002864 RID: 10340
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x04002865 RID: 10341
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x04002866 RID: 10342
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byDomain;

			// Token: 0x04002867 RID: 10343
			public CHCNetSDK.NET_DVR_IPADDR struIP;

			// Token: 0x04002868 RID: 10344
			public ushort wDVRPort;

			// Token: 0x04002869 RID: 10345
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] szDeviceID;

			// Token: 0x0400286A RID: 10346
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x020002AB RID: 683
		public struct NET_DVR_IPPARACFG_V40
		{
			// Token: 0x0400286B RID: 10347
			public uint dwSize;

			// Token: 0x0400286C RID: 10348
			public uint dwGroupNum;

			// Token: 0x0400286D RID: 10349
			public uint dwAChanNum;

			// Token: 0x0400286E RID: 10350
			public uint dwDChanNum;

			// Token: 0x0400286F RID: 10351
			public uint dwStartDChan;

			// Token: 0x04002870 RID: 10352
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] byAnalogChanEnable;

			// Token: 0x04002871 RID: 10353
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public CHCNetSDK.NET_DVR_IPDEVINFO_V31[] struIPDevInfo;

			// Token: 0x04002872 RID: 10354
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public CHCNetSDK.NET_DVR_STREAM_MODE[] struStreamMode;

			// Token: 0x04002873 RID: 10355
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
			public byte[] byRes2;
		}

		// Token: 0x020002AC RID: 684
		public struct NET_DVR_IPSERVER_STREAM
		{
			// Token: 0x06001333 RID: 4915 RVA: 0x0016C370 File Offset: 0x0016B370
			public void Init()
			{
				this.byRes = new byte[3];
				this.byRes1 = new ushort[2];
				this.byUserName = new byte[32];
				this.byPassWord = new byte[16];
				this.byDVRSerialNumber = new byte[48];
				this.byDVRName = new byte[32];
				this.byRes2 = new byte[11];
			}

			// Token: 0x04002874 RID: 10356
			public byte byEnable;

			// Token: 0x04002875 RID: 10357
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x04002876 RID: 10358
			public CHCNetSDK.NET_DVR_IPADDR struIPServer;

			// Token: 0x04002877 RID: 10359
			public ushort wPort;

			// Token: 0x04002878 RID: 10360
			public ushort wDvrNameLen;

			// Token: 0x04002879 RID: 10361
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byDVRName;

			// Token: 0x0400287A RID: 10362
			public ushort wDVRSerialLen;

			// Token: 0x0400287B RID: 10363
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U2)]
			public ushort[] byRes1;

			// Token: 0x0400287C RID: 10364
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] byDVRSerialNumber;

			// Token: 0x0400287D RID: 10365
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byUserName;

			// Token: 0x0400287E RID: 10366
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byPassWord;

			// Token: 0x0400287F RID: 10367
			public byte byChannel;

			// Token: 0x04002880 RID: 10368
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x020002AD RID: 685
		public struct NET_DVR_JPEGPARA
		{
			// Token: 0x04002881 RID: 10369
			public short wPicSize;

			// Token: 0x04002882 RID: 10370
			public short wPicQuality;
		}

		// Token: 0x020002AE RID: 686
		public struct NET_DVR_JSON_DATA_CFG
		{
			// Token: 0x04002883 RID: 10371
			public uint dwSize;

			// Token: 0x04002884 RID: 10372
			public IntPtr lpJsonData;

			// Token: 0x04002885 RID: 10373
			public uint dwJsonDataSize;

			// Token: 0x04002886 RID: 10374
			public IntPtr lpPicData;

			// Token: 0x04002887 RID: 10375
			public uint dwPicDataSize;

			// Token: 0x04002888 RID: 10376
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			public byte[] byRes;
		}

		// Token: 0x020002AF RID: 687
		public struct NET_DVR_LOG_V30
		{
			// Token: 0x04002889 RID: 10377
			public CHCNetSDK.NET_DVR_TIME strLogTime;

			// Token: 0x0400288A RID: 10378
			public uint dwMajorType;

			// Token: 0x0400288B RID: 10379
			public uint dwMinorType;

			// Token: 0x0400288C RID: 10380
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPanelUser;

			// Token: 0x0400288D RID: 10381
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sNetUser;

			// Token: 0x0400288E RID: 10382
			public CHCNetSDK.NET_DVR_IPADDR struRemoteHostAddr;

			// Token: 0x0400288F RID: 10383
			public uint dwParaType;

			// Token: 0x04002890 RID: 10384
			public uint dwChannel;

			// Token: 0x04002891 RID: 10385
			public uint dwDiskNumber;

			// Token: 0x04002892 RID: 10386
			public uint dwAlarmInPort;

			// Token: 0x04002893 RID: 10387
			public uint dwAlarmOutPort;

			// Token: 0x04002894 RID: 10388
			public uint dwInfoLen;

			// Token: 0x04002895 RID: 10389
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11840)]
			public string sInfo;
		}

		// Token: 0x020002B0 RID: 688
		public struct NET_DVR_NETCFG_V30
		{
			// Token: 0x06001334 RID: 4916 RVA: 0x0016C3D8 File Offset: 0x0016B3D8
			public void Init()
			{
				this.struEtherNet = new CHCNetSDK.NET_DVR_ETHERNET_V30[2];
				this.struAlarmHostIpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.struDnsServer1IpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.struDnsServer2IpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.byIpResolver = new byte[64];
				this.struMulticastIpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.struGatewayIpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.struPPPoE = default(CHCNetSDK.NET_DVR_PPPOECFG);
			}

			// Token: 0x04002896 RID: 10390
			public uint dwSize;

			// Token: 0x04002897 RID: 10391
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_ETHERNET_V30[] struEtherNet;

			// Token: 0x04002898 RID: 10392
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPADDR[] struRes1;

			// Token: 0x04002899 RID: 10393
			public CHCNetSDK.NET_DVR_IPADDR struAlarmHostIpAddr;

			// Token: 0x0400289A RID: 10394
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U2)]
			public ushort[] wRes2;

			// Token: 0x0400289B RID: 10395
			public ushort wAlarmHostIpPort;

			// Token: 0x0400289C RID: 10396
			public byte byUseDhcp;

			// Token: 0x0400289D RID: 10397
			public byte byIPv6Mode;

			// Token: 0x0400289E RID: 10398
			public CHCNetSDK.NET_DVR_IPADDR struDnsServer1IpAddr;

			// Token: 0x0400289F RID: 10399
			public CHCNetSDK.NET_DVR_IPADDR struDnsServer2IpAddr;

			// Token: 0x040028A0 RID: 10400
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byIpResolver;

			// Token: 0x040028A1 RID: 10401
			public ushort wIpResolverPort;

			// Token: 0x040028A2 RID: 10402
			public ushort wHttpPortNo;

			// Token: 0x040028A3 RID: 10403
			public CHCNetSDK.NET_DVR_IPADDR struMulticastIpAddr;

			// Token: 0x040028A4 RID: 10404
			public CHCNetSDK.NET_DVR_IPADDR struGatewayIpAddr;

			// Token: 0x040028A5 RID: 10405
			public CHCNetSDK.NET_DVR_PPPOECFG struPPPoE;

			// Token: 0x040028A6 RID: 10406
			public byte byEnablePrivateMulticastDiscovery;

			// Token: 0x040028A7 RID: 10407
			public byte byEnableOnvifMulticastDiscovery;

			// Token: 0x040028A8 RID: 10408
			public byte byEnableDNS;

			// Token: 0x040028A9 RID: 10409
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002B1 RID: 689
		public struct NET_DVR_NETCFG_V50
		{
			// Token: 0x06001335 RID: 4917 RVA: 0x0016C448 File Offset: 0x0016B448
			public void Init()
			{
				this.struEtherNet = new CHCNetSDK.NET_DVR_ETHERNET_V30[2];
				this.struRes1 = new CHCNetSDK.NET_DVR_IPADDR[2];
				this.struAlarmHostIpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.struAlarmHost2IpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.struDnsServer1IpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.struDnsServer2IpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.byIpResolver = new byte[64];
				this.struMulticastIpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.struGatewayIpAddr = default(CHCNetSDK.NET_DVR_IPADDR);
				this.struPPPoE = default(CHCNetSDK.NET_DVR_PPPOECFG);
				this.byRes = new byte[599];
			}

			// Token: 0x040028AA RID: 10410
			public uint dwSize;

			// Token: 0x040028AB RID: 10411
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_ETHERNET_V30[] struEtherNet;

			// Token: 0x040028AC RID: 10412
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPADDR[] struRes1;

			// Token: 0x040028AD RID: 10413
			public CHCNetSDK.NET_DVR_IPADDR struAlarmHostIpAddr;

			// Token: 0x040028AE RID: 10414
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x040028AF RID: 10415
			public ushort wAlarmHostIpPort;

			// Token: 0x040028B0 RID: 10416
			public byte byUseDhcp;

			// Token: 0x040028B1 RID: 10417
			public byte byIPv6Mode;

			// Token: 0x040028B2 RID: 10418
			public CHCNetSDK.NET_DVR_IPADDR struDnsServer1IpAddr;

			// Token: 0x040028B3 RID: 10419
			public CHCNetSDK.NET_DVR_IPADDR struDnsServer2IpAddr;

			// Token: 0x040028B4 RID: 10420
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byIpResolver;

			// Token: 0x040028B5 RID: 10421
			public ushort wIpResolverPort;

			// Token: 0x040028B6 RID: 10422
			public ushort wHttpPortNo;

			// Token: 0x040028B7 RID: 10423
			public CHCNetSDK.NET_DVR_IPADDR struMulticastIpAddr;

			// Token: 0x040028B8 RID: 10424
			public CHCNetSDK.NET_DVR_IPADDR struGatewayIpAddr;

			// Token: 0x040028B9 RID: 10425
			public CHCNetSDK.NET_DVR_PPPOECFG struPPPoE;

			// Token: 0x040028BA RID: 10426
			public byte byEnablePrivateMulticastDiscovery;

			// Token: 0x040028BB RID: 10427
			public byte byEnableOnvifMulticastDiscovery;

			// Token: 0x040028BC RID: 10428
			public ushort wAlarmHost2IpPort;

			// Token: 0x040028BD RID: 10429
			public CHCNetSDK.NET_DVR_IPADDR struAlarmHost2IpAddr;

			// Token: 0x040028BE RID: 10430
			public byte byEnableDNS;

			// Token: 0x040028BF RID: 10431
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 599, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002B2 RID: 690
		public struct NET_DVR_PLAN_TEMPLATE
		{
			// Token: 0x06001336 RID: 4918 RVA: 0x0016C4DE File Offset: 0x0016B4DE
			public void Init()
			{
				this.byTemplateName = new byte[32];
				this.dwHolidayGroupNo = new uint[16];
				this.byRes1 = new byte[3];
				this.byRes2 = new byte[32];
			}

			// Token: 0x040028C0 RID: 10432
			public uint dwSize;

			// Token: 0x040028C1 RID: 10433
			public byte byEnable;

			// Token: 0x040028C2 RID: 10434
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040028C3 RID: 10435
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byTemplateName;

			// Token: 0x040028C4 RID: 10436
			public uint dwWeekPlanNo;

			// Token: 0x040028C5 RID: 10437
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U4)]
			public uint[] dwHolidayGroupNo;

			// Token: 0x040028C6 RID: 10438
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x020002B3 RID: 691
		public struct NET_DVR_PLAN_TEMPLATE_COND
		{
			// Token: 0x06001337 RID: 4919 RVA: 0x0016C513 File Offset: 0x0016B513
			public void Init()
			{
				this.byRes = new byte[106];
			}

			// Token: 0x040028C7 RID: 10439
			public uint dwSize;

			// Token: 0x040028C8 RID: 10440
			public uint dwPlanTemplateNumber;

			// Token: 0x040028C9 RID: 10441
			public ushort wLocalControllerID;

			// Token: 0x040028CA RID: 10442
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 106, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002B4 RID: 692
		public struct NET_DVR_PPPOECFG
		{
			// Token: 0x040028CB RID: 10443
			public uint dwPPPOE;

			// Token: 0x040028CC RID: 10444
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sPPPoEUser;

			// Token: 0x040028CD RID: 10445
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sPPPoEPassword;

			// Token: 0x040028CE RID: 10446
			public CHCNetSDK.NET_DVR_IPADDR struPPPoEIP;
		}

		// Token: 0x020002B5 RID: 693
		public struct NET_DVR_PREVIEWINFO
		{
			// Token: 0x040028CF RID: 10447
			public int lChannel;

			// Token: 0x040028D0 RID: 10448
			public uint dwStreamType;

			// Token: 0x040028D1 RID: 10449
			public uint dwLinkMode;

			// Token: 0x040028D2 RID: 10450
			public IntPtr hPlayWnd;

			// Token: 0x040028D3 RID: 10451
			public uint bBlocked;

			// Token: 0x040028D4 RID: 10452
			public uint bPassbackRecord;

			// Token: 0x040028D5 RID: 10453
			public byte byPreviewMode;

			// Token: 0x040028D6 RID: 10454
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byStreamID;

			// Token: 0x040028D7 RID: 10455
			public byte byProtoType;

			// Token: 0x040028D8 RID: 10456
			public byte byRes1;

			// Token: 0x040028D9 RID: 10457
			public byte byVideoCodingType;

			// Token: 0x040028DA RID: 10458
			public uint dwDisplayBufNum;

			// Token: 0x040028DB RID: 10459
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 216, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002B6 RID: 694
		public struct NET_DVR_PU_STREAM_CFG
		{
			// Token: 0x040028DC RID: 10460
			public uint dwSize;

			// Token: 0x040028DD RID: 10461
			public CHCNetSDK.NET_DVR_STREAM_MEDIA_SERVER_CFG struStreamMediaSvrCfg;

			// Token: 0x040028DE RID: 10462
			public CHCNetSDK.NET_DVR_DEV_CHAN_INFO struDevChanInfo;
		}

		// Token: 0x020002B7 RID: 695
		public struct NET_DVR_PU_STREAM_URL
		{
			// Token: 0x06001338 RID: 4920 RVA: 0x0016C522 File Offset: 0x0016B522
			public void Init()
			{
				this.byRes = new byte[7];
				this.strURL = new byte[240];
			}

			// Token: 0x040028DF RID: 10463
			public byte byEnable;

			// Token: 0x040028E0 RID: 10464
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 240, ArraySubType = UnmanagedType.I1)]
			public byte[] strURL;

			// Token: 0x040028E1 RID: 10465
			public byte byTransPortocol;

			// Token: 0x040028E2 RID: 10466
			public ushort wIPID;

			// Token: 0x040028E3 RID: 10467
			public byte byChannel;

			// Token: 0x040028E4 RID: 10468
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002B8 RID: 696
		public struct NET_DVR_SETUPALARM_PARAM
		{
			// Token: 0x040028E5 RID: 10469
			public uint dwSize;

			// Token: 0x040028E6 RID: 10470
			public byte byLevel;

			// Token: 0x040028E7 RID: 10471
			public byte byAlarmInfoType;

			// Token: 0x040028E8 RID: 10472
			public byte byRetAlarmTypeV40;

			// Token: 0x040028E9 RID: 10473
			public byte byRetDevInfoVersion;

			// Token: 0x040028EA RID: 10474
			public byte byRetVQDAlarmType;

			// Token: 0x040028EB RID: 10475
			public byte byFaceAlarmDetection;

			// Token: 0x040028EC RID: 10476
			public byte bySupport;

			// Token: 0x040028ED RID: 10477
			public byte byBrokenNetHttp;

			// Token: 0x040028EE RID: 10478
			public ushort wTaskNo;

			// Token: 0x040028EF RID: 10479
			public byte byDeployType;

			// Token: 0x040028F0 RID: 10480
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040028F1 RID: 10481
			public byte byAlarmTypeURL;

			// Token: 0x040028F2 RID: 10482
			public byte byCustomCtrl;
		}

		// Token: 0x020002B9 RID: 697
		public struct NET_DVR_SIMPLE_DAYTIME
		{
			// Token: 0x040028F3 RID: 10483
			public byte byHour;

			// Token: 0x040028F4 RID: 10484
			public byte byMinute;

			// Token: 0x040028F5 RID: 10485
			public byte bySecond;

			// Token: 0x040028F6 RID: 10486
			public byte byRes;
		}

		// Token: 0x020002BA RID: 698
		public struct NET_DVR_SINGLE_PLAN_SEGMENT
		{
			// Token: 0x06001339 RID: 4921 RVA: 0x0016C540 File Offset: 0x0016B540
			public void Init()
			{
				this.byRes = new byte[5];
			}

			// Token: 0x040028F7 RID: 10487
			public byte byEnable;

			// Token: 0x040028F8 RID: 10488
			public byte byDoorStatus;

			// Token: 0x040028F9 RID: 10489
			public byte byVerifyMode;

			// Token: 0x040028FA RID: 10490
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x040028FB RID: 10491
			public CHCNetSDK.NET_DVR_TIME_SEGMENT struTimeSegment;
		}

		// Token: 0x020002BB RID: 699
		public struct NET_DVR_STREAM_MEDIA_SERVER_CFG
		{
			// Token: 0x0600133A RID: 4922 RVA: 0x0016C54E File Offset: 0x0016B54E
			public void Init()
			{
				this.byRes1 = new byte[3];
				this.byRes2 = new byte[69];
			}

			// Token: 0x040028FC RID: 10492
			public byte byValid;

			// Token: 0x040028FD RID: 10493
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040028FE RID: 10494
			public CHCNetSDK.NET_DVR_IPADDR struDevIP;

			// Token: 0x040028FF RID: 10495
			public ushort wDevPort;

			// Token: 0x04002900 RID: 10496
			public byte byTransmitType;

			// Token: 0x04002901 RID: 10497
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 69, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x020002BC RID: 700
		public struct NET_DVR_STREAM_MODE
		{
			// Token: 0x04002902 RID: 10498
			public byte byGetStreamType;

			// Token: 0x04002903 RID: 10499
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] byRes;

			// Token: 0x04002904 RID: 10500
			public CHCNetSDK.NET_DVR_GET_STREAM_UNION uGetStream;
		}

		// Token: 0x020002BD RID: 701
		public struct NET_DVR_TIME
		{
			// Token: 0x04002905 RID: 10501
			public int dwYear;

			// Token: 0x04002906 RID: 10502
			public int dwMonth;

			// Token: 0x04002907 RID: 10503
			public int dwDay;

			// Token: 0x04002908 RID: 10504
			public int dwHour;

			// Token: 0x04002909 RID: 10505
			public int dwMinute;

			// Token: 0x0400290A RID: 10506
			public int dwSecond;
		}

		// Token: 0x020002BE RID: 702
		public struct NET_DVR_TIME_EX
		{
			// Token: 0x0400290B RID: 10507
			public ushort wYear;

			// Token: 0x0400290C RID: 10508
			public byte byMonth;

			// Token: 0x0400290D RID: 10509
			public byte byDay;

			// Token: 0x0400290E RID: 10510
			public byte byHour;

			// Token: 0x0400290F RID: 10511
			public byte byMinute;

			// Token: 0x04002910 RID: 10512
			public byte bySecond;

			// Token: 0x04002911 RID: 10513
			public byte byRes;
		}

		// Token: 0x020002BF RID: 703
		public struct NET_DVR_TIME_SEGMENT
		{
			// Token: 0x04002912 RID: 10514
			public CHCNetSDK.NET_DVR_SIMPLE_DAYTIME struBeginTime;

			// Token: 0x04002913 RID: 10515
			public CHCNetSDK.NET_DVR_SIMPLE_DAYTIME struEndTime;
		}

		// Token: 0x020002C0 RID: 704
		public struct NET_DVR_USER_LOGIN_INFO
		{
			// Token: 0x04002914 RID: 10516
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
			public string sDeviceAddress;

			// Token: 0x04002915 RID: 10517
			public byte byUseTransport;

			// Token: 0x04002916 RID: 10518
			public ushort wPort;

			// Token: 0x04002917 RID: 10519
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string sUserName;

			// Token: 0x04002918 RID: 10520
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string sPassword;

			// Token: 0x04002919 RID: 10521
			public CHCNetSDK.LoginResultCallBack cbLoginResult;

			// Token: 0x0400291A RID: 10522
			public IntPtr pUser;

			// Token: 0x0400291B RID: 10523
			public bool bUseAsynLogin;

			// Token: 0x0400291C RID: 10524
			public byte byProxyType;

			// Token: 0x0400291D RID: 10525
			public byte byUseUTCTime;

			// Token: 0x0400291E RID: 10526
			public byte byLoginMode;

			// Token: 0x0400291F RID: 10527
			public byte byHttps;

			// Token: 0x04002920 RID: 10528
			public int iProxyID;

			// Token: 0x04002921 RID: 10529
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;
		}

		// Token: 0x020002C1 RID: 705
		public struct NET_DVR_VALID_PERIOD_CFG
		{
			// Token: 0x04002922 RID: 10530
			public byte byEnable;

			// Token: 0x04002923 RID: 10531
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04002924 RID: 10532
			public CHCNetSDK.NET_DVR_TIME_EX struBeginTime;

			// Token: 0x04002925 RID: 10533
			public CHCNetSDK.NET_DVR_TIME_EX struEndTime;

			// Token: 0x04002926 RID: 10534
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x020002C2 RID: 706
		public struct NET_DVR_VIDEO_CALL_COND
		{
			// Token: 0x0600133B RID: 4923 RVA: 0x0016C569 File Offset: 0x0016B569
			public void Init()
			{
				this.byRes = new byte[128];
			}

			// Token: 0x04002927 RID: 10535
			public uint dwSize;

			// Token: 0x04002928 RID: 10536
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002C3 RID: 707
		public struct NET_DVR_VIDEO_CALL_PARAM
		{
			// Token: 0x0600133C RID: 4924 RVA: 0x0016C57B File Offset: 0x0016B57B
			public void Init()
			{
				this.byRes = new byte[118];
			}

			// Token: 0x04002929 RID: 10537
			public uint dwSize;

			// Token: 0x0400292A RID: 10538
			public uint dwCmdType;

			// Token: 0x0400292B RID: 10539
			public ushort wPeriod;

			// Token: 0x0400292C RID: 10540
			public ushort wBuildingNumber;

			// Token: 0x0400292D RID: 10541
			public ushort wUnitNumber;

			// Token: 0x0400292E RID: 10542
			public ushort wFloorNumber;

			// Token: 0x0400292F RID: 10543
			public ushort wRoomNumber;

			// Token: 0x04002930 RID: 10544
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 118, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002C4 RID: 708
		public struct NET_DVR_VOLUME_CFG
		{
			// Token: 0x04002931 RID: 10545
			public uint dwSize;

			// Token: 0x04002932 RID: 10546
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public ushort[] wVolume;

			// Token: 0x04002933 RID: 10547
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] byRes;
		}

		// Token: 0x020002C5 RID: 709
		public struct NET_DVR_WEEK_PLAN_CFG
		{
			// Token: 0x0600133D RID: 4925 RVA: 0x0016C58C File Offset: 0x0016B58C
			public void Init()
			{
				this.struPlanCfg = new CHCNetSDK.NET_DVR_SINGLE_PLAN_SEGMENT[56];
				foreach (CHCNetSDK.NET_DVR_SINGLE_PLAN_SEGMENT net_DVR_SINGLE_PLAN_SEGMENT in this.struPlanCfg)
				{
					net_DVR_SINGLE_PLAN_SEGMENT.Init();
				}
				this.byRes1 = new byte[3];
				this.byRes2 = new byte[16];
			}

			// Token: 0x04002934 RID: 10548
			public uint dwSize;

			// Token: 0x04002935 RID: 10549
			public byte byEnable;

			// Token: 0x04002936 RID: 10550
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04002937 RID: 10551
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SINGLE_PLAN_SEGMENT[] struPlanCfg;

			// Token: 0x04002938 RID: 10552
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x020002C6 RID: 710
		public struct NET_DVR_WEEK_PLAN_COND
		{
			// Token: 0x0600133E RID: 4926 RVA: 0x0016C5E8 File Offset: 0x0016B5E8
			public void Init()
			{
				this.byRes = new byte[106];
			}

			// Token: 0x04002939 RID: 10553
			public uint dwSize;

			// Token: 0x0400293A RID: 10554
			public uint dwWeekPlanNumber;

			// Token: 0x0400293B RID: 10555
			public ushort wLocalControllerID;

			// Token: 0x0400293C RID: 10556
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 106, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020002C7 RID: 711
		public struct NET_DVR_XML_CONFIG_INPUT
		{
			// Token: 0x0400293D RID: 10557
			public uint dwSize;

			// Token: 0x0400293E RID: 10558
			public IntPtr lpRequestUrl;

			// Token: 0x0400293F RID: 10559
			public uint dwRequestUrlLen;

			// Token: 0x04002940 RID: 10560
			public IntPtr lpInBuffer;

			// Token: 0x04002941 RID: 10561
			public uint dwInBufferSize;

			// Token: 0x04002942 RID: 10562
			public uint dwRecvTimeOut;

			// Token: 0x04002943 RID: 10563
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] byRes;
		}

		// Token: 0x020002C8 RID: 712
		public struct NET_DVR_XML_CONFIG_OUTPUT
		{
			// Token: 0x04002944 RID: 10564
			public uint dwSize;

			// Token: 0x04002945 RID: 10565
			public IntPtr lpOutBuffer;

			// Token: 0x04002946 RID: 10566
			public uint dwOutBufferSize;

			// Token: 0x04002947 RID: 10567
			public uint dwReturnedXMLSize;

			// Token: 0x04002948 RID: 10568
			public IntPtr lpStatusBuffer;

			// Token: 0x04002949 RID: 10569
			public uint dwStatusSize;

			// Token: 0x0400294A RID: 10570
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] byRes;
		}

		// Token: 0x020002C9 RID: 713
		public enum NET_SDK_CALLBACK_STATUS_NORMAL
		{
			// Token: 0x0400294C RID: 10572
			NET_DVR_CALLBACK_STATUS_SEND_WAIT = 1006,
			// Token: 0x0400294D RID: 10573
			NET_SDK_CALLBACK_STATUS_DEV_TYPE_MISMATCH = 1005,
			// Token: 0x0400294E RID: 10574
			NET_SDK_CALLBACK_STATUS_EXCEPTION = 1003,
			// Token: 0x0400294F RID: 10575
			NET_SDK_CALLBACK_STATUS_FAILED = 1002,
			// Token: 0x04002950 RID: 10576
			NET_SDK_CALLBACK_STATUS_LANGUAGE_MISMATCH = 1004,
			// Token: 0x04002951 RID: 10577
			NET_SDK_CALLBACK_STATUS_PROCESSING = 1001,
			// Token: 0x04002952 RID: 10578
			NET_SDK_CALLBACK_STATUS_SUCCESS = 1000
		}

		// Token: 0x020002CA RID: 714
		public enum NET_SDK_CALLBACK_TYPE
		{
			// Token: 0x04002954 RID: 10580
			NET_SDK_CALLBACK_TYPE_STATUS,
			// Token: 0x04002955 RID: 10581
			NET_SDK_CALLBACK_TYPE_PROGRESS,
			// Token: 0x04002956 RID: 10582
			NET_SDK_CALLBACK_TYPE_DATA
		}

		// Token: 0x020002CB RID: 715
		public enum NET_SDK_GET_NEXT_STATUS
		{
			// Token: 0x04002958 RID: 10584
			NET_SDK_GET_NETX_STATUS_NEED_WAIT = 1001,
			// Token: 0x04002959 RID: 10585
			NET_SDK_GET_NEXT_STATUS_FAILED = 1003,
			// Token: 0x0400295A RID: 10586
			NET_SDK_GET_NEXT_STATUS_FINISH = 1002,
			// Token: 0x0400295B RID: 10587
			NET_SDK_GET_NEXT_STATUS_SUCCESS = 1000
		}

		// Token: 0x020002CC RID: 716
		// (Invoke) Token: 0x06001340 RID: 4928
		public delegate void RealDataCallBack(int lPlayHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr pUser);

		// Token: 0x020002CD RID: 717
		// (Invoke) Token: 0x06001344 RID: 4932
		public delegate void REALDATACALLBACK(int lRealHandle, uint dwDataType, ref byte pBuffer, uint dwBufSize, IntPtr pUser);

		// Token: 0x020002CE RID: 718
		// (Invoke) Token: 0x06001348 RID: 4936
		public delegate void RemoteConfigCallback(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData);

		// Token: 0x020002CF RID: 719
		// (Invoke) Token: 0x0600134C RID: 4940
		public delegate void VOICEDATACALLBACKV30(int lVoiceComHandle, string pRecvDataBuffer, uint dwBufSize, byte byAudioFlag, IntPtr pUser);
	}
}
