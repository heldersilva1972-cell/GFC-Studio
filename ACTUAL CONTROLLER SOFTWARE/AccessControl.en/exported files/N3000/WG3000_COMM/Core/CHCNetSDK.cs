using System;
using System.Runtime.InteropServices;

namespace WG3000_COMM.Core
{
	// Token: 0x02000068 RID: 104
	public class CHCNetSDK
	{
		// Token: 0x060007A3 RID: 1955
		[DllImport("AnalyzeData.dll")]
		public static extern bool AnalyzeDataClose(int iHandle);

		// Token: 0x060007A4 RID: 1956
		[DllImport("AnalyzeData.dll")]
		public static extern uint AnalyzeDataGetLastError(int iHandle);

		// Token: 0x060007A5 RID: 1957
		[DllImport("AnalyzeData.dll")]
		public static extern int AnalyzeDataGetPacket(int iHandle, ref CHCNetSDK.PACKET_INFO pPacketInfo);

		// Token: 0x060007A6 RID: 1958
		[DllImport("AnalyzeData.dll")]
		public static extern int AnalyzeDataGetSafeHandle();

		// Token: 0x060007A7 RID: 1959
		[DllImport("AnalyzeData.dll")]
		public static extern bool AnalyzeDataGetTail(int iHandle, ref IntPtr pBuffer, ref uint uiSize);

		// Token: 0x060007A8 RID: 1960
		[DllImport("AnalyzeData.dll")]
		public static extern bool AnalyzeDataInputData(int iHandle, IntPtr pBuffer, uint uiSize);

		// Token: 0x060007A9 RID: 1961
		[DllImport("AnalyzeData.dll")]
		public static extern bool AnalyzeDataOpenStreamEx(int iHandle, byte[] pFileHead);

		// Token: 0x060007AA RID: 1962
		[DllImport("GetStream.dll")]
		public static extern int CLIENT_SDK_GetStream(CHCNetSDK.PLAY_INFO lpPlayInfo);

		// Token: 0x060007AB RID: 1963
		[DllImport("GetStream.dll")]
		public static extern bool CLIENT_SDK_GetVideoEffect(int iRealHandle, ref int iBrightValue, ref int iContrastValue, ref int iSaturationValue, ref int iHueValue);

		// Token: 0x060007AC RID: 1964
		[DllImport("GetStream.dll")]
		public static extern bool CLIENT_SDK_Init();

		// Token: 0x060007AD RID: 1965
		[DllImport("GetStream.dll")]
		public static extern bool CLIENT_SDK_MakeKeyFrame(int iRealHandle);

		// Token: 0x060007AE RID: 1966
		[DllImport("GetStream.dll")]
		public static extern bool CLIENT_SDK_SetVideoEffect(int iRealHandle, int iBrightValue, int iContrastValue, int iSaturationValue, int iHueValue);

		// Token: 0x060007AF RID: 1967
		[DllImport("GetStream.dll")]
		public static extern bool CLIENT_SDK_StopStream(int iRealHandle);

		// Token: 0x060007B0 RID: 1968
		[DllImport("GetStream.dll")]
		public static extern bool CLIENT_SDK_UnInit();

		// Token: 0x060007B1 RID: 1969
		[DllImport("GetStream.dll")]
		public static extern bool CLIENT_SetRealDataCallBack(int iRealHandle, CHCNetSDK.SETREALDATACALLBACK fRealDataCallBack, uint lUser);

		// Token: 0x060007B2 RID: 1970
		[DllImport("RecordDLL.dll")]
		public static extern bool CloseChannelRecord(int iRecordHandle);

		// Token: 0x060007B3 RID: 1971
		[DllImport("RecordDLL.dll")]
		public static extern int GetData(int iHandle, int iDataType, IntPtr pBuf, uint uiSize);

		// Token: 0x060007B4 RID: 1972
		[DllImport("RecordDLL.dll")]
		public static extern int Initialize(CHCNetSDK.STOREINFO struStoreInfo);

		// Token: 0x060007B5 RID: 1973
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_AddDVR(int lUserID);

		// Token: 0x060007B6 RID: 1974
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_AddDVR_V30(int lUserID, uint dwVoiceChan);

		// Token: 0x060007B7 RID: 1975
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_AudioPreview_Card(int lRealHandle, int bEnable);

		// Token: 0x060007B8 RID: 1976
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CaptureJPEGPicture(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara, string sPicFileName);

		// Token: 0x060007B9 RID: 1977
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CaptureJPEGPicture_NEW(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara, string sJpegPicBuffer, uint dwPicSize, ref uint lpSizeReturned);

		// Token: 0x060007BA RID: 1978
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CapturePicture(int lRealHandle, string sPicFileName);

		// Token: 0x060007BB RID: 1979
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CapturePicture_Card(int lRealHandle, string sPicFileName);

		// Token: 0x060007BC RID: 1980
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_Cleanup();

		// Token: 0x060007BD RID: 1981
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClearSurface_Card();

		// Token: 0x060007BE RID: 1982
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClickKey(int lUserID, int lKeyIndex);

		// Token: 0x060007BF RID: 1983
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClientAudioStart();

		// Token: 0x060007C0 RID: 1984
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClientAudioStart_V30(CHCNetSDK.VOICEAUDIOSTART fVoiceAudioStart, IntPtr pUser);

		// Token: 0x060007C1 RID: 1985
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClientAudioStop();

		// Token: 0x060007C2 RID: 1986
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClientGetframeformat(int lUserID, ref CHCNetSDK.NET_DVR_FRAMEFORMAT lpFrameFormat);

		// Token: 0x060007C3 RID: 1987
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClientGetframeformat_V30(int lUserID, ref CHCNetSDK.NET_DVR_FRAMEFORMAT_V30 lpFrameFormat);

		// Token: 0x060007C4 RID: 1988
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClientGetVideoEffect(int lRealHandle, ref uint pBrightValue, ref uint pContrastValue, ref uint pSaturationValue, ref uint pHueValue);

		// Token: 0x060007C5 RID: 1989
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClientSetframeformat(int lUserID, ref CHCNetSDK.NET_DVR_FRAMEFORMAT lpFrameFormat);

		// Token: 0x060007C6 RID: 1990
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClientSetframeformat_V30(int lUserID, ref CHCNetSDK.NET_DVR_FRAMEFORMAT_V30 lpFrameFormat);

		// Token: 0x060007C7 RID: 1991
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ClientSetVideoEffect(int lRealHandle, uint dwBrightValue, uint dwContrastValue, uint dwSaturationValue, uint dwHueValue);

		// Token: 0x060007C8 RID: 1992
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CloseAlarmChan(int lAlarmHandle);

		// Token: 0x060007C9 RID: 1993
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CloseAlarmChan_V30(int lAlarmHandle);

		// Token: 0x060007CA RID: 1994
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CloseFormatHandle(int lFormatHandle);

		// Token: 0x060007CB RID: 1995
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CloseSound();

		// Token: 0x060007CC RID: 1996
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CloseSound_Card(int lRealHandle);

		// Token: 0x060007CD RID: 1997
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CloseSoundShare(int lRealHandle);

		// Token: 0x060007CE RID: 1998
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_CloseUpgradeHandle(int lUpgradeHandle);

		// Token: 0x060007CF RID: 1999
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_DecCtrlDec(int lUserID, int lChannel, uint dwControlCode);

		// Token: 0x060007D0 RID: 2000
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_DecCtrlScreen(int lUserID, int lChannel, uint dwControl);

		// Token: 0x060007D1 RID: 2001
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_DecodeG711Frame(uint iType, ref byte pInBuffer, ref byte pOutBuffer);

		// Token: 0x060007D2 RID: 2002
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_DecodeG722Frame(IntPtr pDecHandle, ref byte pInBuffer, ref byte pOutBuffer);

		// Token: 0x060007D3 RID: 2003
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_DecPlayBackCtrl(int lUserID, int lChannel, uint dwControlCode, uint dwInValue, ref uint LPOutValue, ref CHCNetSDK.NET_DVR_PLAYREMOTEFILE lpRemoteFileInfo);

		// Token: 0x060007D4 RID: 2004
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_DelDVR(int lUserID);

		// Token: 0x060007D5 RID: 2005
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_DelDVR_V30(int lVoiceHandle);

		// Token: 0x060007D6 RID: 2006
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_EmailTest(int lUserID);

		// Token: 0x060007D7 RID: 2007
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_EncodeG711Frame(uint iType, ref byte pInBuffer, ref byte pOutBuffer);

		// Token: 0x060007D8 RID: 2008
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_EncodeG722Frame(IntPtr pEncodeHandle, ref byte pInBuffer, ref byte pOutBuffer);

		// Token: 0x060007D9 RID: 2009
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_FindClose(int lFindHandle);

		// Token: 0x060007DA RID: 2010
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_FindClose_V30(int lFindHandle);

		// Token: 0x060007DB RID: 2011
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindDVRLog(int lUserID, int lSelectMode, uint dwMajorType, uint dwMinorType, ref CHCNetSDK.NET_DVR_TIME lpStartTime, ref CHCNetSDK.NET_DVR_TIME lpStopTime);

		// Token: 0x060007DC RID: 2012
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindDVRLog_Matrix(int iUserID, int lSelectMode, uint dwMajorType, uint dwMinorType, ref CHCNetSDK.tagVEDIOPLATLOG lpVedioPlatLog, ref CHCNetSDK.NET_DVR_TIME lpStartTime, ref CHCNetSDK.NET_DVR_TIME lpStopTime);

		// Token: 0x060007DD RID: 2013
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindDVRLog_V30(int lUserID, int lSelectMode, uint dwMajorType, uint dwMinorType, ref CHCNetSDK.NET_DVR_TIME lpStartTime, ref CHCNetSDK.NET_DVR_TIME lpStopTime, bool bOnlySmart);

		// Token: 0x060007DE RID: 2014
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindFile(int lUserID, int lChannel, uint dwFileType, ref CHCNetSDK.NET_DVR_TIME lpStartTime, ref CHCNetSDK.NET_DVR_TIME lpStopTime);

		// Token: 0x060007DF RID: 2015
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindFile_Card(int lUserID, int lChannel, uint dwFileType, ref CHCNetSDK.NET_DVR_TIME lpStartTime, ref CHCNetSDK.NET_DVR_TIME lpStopTime);

		// Token: 0x060007E0 RID: 2016
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindFile_V30(int lUserID, ref CHCNetSDK.NET_DVR_FILECOND pFindCond);

		// Token: 0x060007E1 RID: 2017
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindFileByCard(int lUserID, int lChannel, uint dwFileType, int nFindType, ref byte sCardNumber, ref CHCNetSDK.NET_DVR_TIME lpStartTime, ref CHCNetSDK.NET_DVR_TIME lpStopTime);

		// Token: 0x060007E2 RID: 2018
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindFileByEvent(int lUserID, ref CHCNetSDK.tagNET_DVR_SEARCH_EVENT_PARAM lpSearchEventParam);

		// Token: 0x060007E3 RID: 2019
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_FindLogClose(int lLogHandle);

		// Token: 0x060007E4 RID: 2020
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_FindLogClose_V30(int lLogHandle);

		// Token: 0x060007E5 RID: 2021
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindNextEvent(int lSearchHandle, ref CHCNetSDK.tagNET_DVR_SEARCH_EVENT_RET lpSearchEventRet);

		// Token: 0x060007E6 RID: 2022
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindNextFile(int lFindHandle, ref CHCNetSDK.NET_DVR_FIND_DATA lpFindData);

		// Token: 0x060007E7 RID: 2023
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindNextFile_Card(int lFindHandle, ref CHCNetSDK.NET_DVR_FINDDATA_CARD lpFindData);

		// Token: 0x060007E8 RID: 2024
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindNextFile_V30(int lFindHandle, ref CHCNetSDK.NET_DVR_FINDDATA_V30 lpFindData);

		// Token: 0x060007E9 RID: 2025
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindNextLog(int lLogHandle, ref CHCNetSDK.NET_DVR_LOG lpLogData);

		// Token: 0x060007EA RID: 2026
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindNextLog_MATRIX(int iLogHandle, ref CHCNetSDK.NET_DVR_LOG_MATRIX lpLogData);

		// Token: 0x060007EB RID: 2027
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FindNextLog_V30(int lLogHandle, ref CHCNetSDK.NET_DVR_LOG_V30 lpLogData);

		// Token: 0x060007EC RID: 2028
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_FormatDisk(int lUserID, int lDiskNumber);

		// Token: 0x060007ED RID: 2029
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetAlarmOut(int lUserID, ref CHCNetSDK.NET_DVR_ALARMOUTSTATUS lpAlarmOutState);

		// Token: 0x060007EE RID: 2030
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetAlarmOut_V30(int lUserID, IntPtr lpAlarmOutState);

		// Token: 0x060007EF RID: 2031
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetATMPortCFG(int lUserID, ref ushort LPOutATMPort);

		// Token: 0x060007F0 RID: 2032
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetAtmProtocol(int lUserID, ref CHCNetSDK.tagNET_DVR_ATM_PROTOCOL lpAtmProtocol);

		// Token: 0x060007F1 RID: 2033
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetBehaviorParamKey(int lUserID, int lChannel, uint dwParameterKey, ref int pValue);

		// Token: 0x060007F2 RID: 2034
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetCardLastError_Card();

		// Token: 0x060007F3 RID: 2035
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetCCDCfg(int lUserID, int lChannel, ref CHCNetSDK.tagNET_DVR_CCD_CFG lpCCDCfg);

		// Token: 0x060007F4 RID: 2036
		[DllImport("HCNetSDK.dll")]
		public static extern IntPtr NET_DVR_GetChanHandle_Card(int lRealHandle);

		// Token: 0x060007F5 RID: 2037
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetConfigFile(int lUserID, string sFileName);

		// Token: 0x060007F6 RID: 2038
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetConfigFile_EX(int lUserID, string sOutBuffer, uint dwOutSize);

		// Token: 0x060007F7 RID: 2039
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetConfigFile_V30(int lUserID, string sOutBuffer, uint dwOutSize, ref uint pReturnSize);

		// Token: 0x060007F8 RID: 2040
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetDDrawDeviceTotalNums();

		// Token: 0x060007F9 RID: 2041
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDecCurLinkStatus(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_DECSTATUS lpDecStatus);

		// Token: 0x060007FA RID: 2042
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDecInfo(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_DECCFG lpDecoderinfo);

		// Token: 0x060007FB RID: 2043
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDecoderState(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_DECODERSTATE lpDecoderState);

		// Token: 0x060007FC RID: 2044
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDecTransPort(int lUserID, ref CHCNetSDK.NET_DVR_PORTCFG lpTransPort);

		// Token: 0x060007FD RID: 2045
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDeviceAbility(int lUserID, uint dwAbilityType, IntPtr pInBuf, uint dwInLength, IntPtr pOutBuf, uint dwOutLength);

		// Token: 0x060007FE RID: 2046
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetDownloadPos(int lFileHandle);

		// Token: 0x060007FF RID: 2047
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDVRConfig(int lUserID, uint dwCommand, int lChannel, IntPtr lpOutBuffer, uint dwOutBufferSize, ref uint lpBytesReturned);

		// Token: 0x06000800 RID: 2048
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDVRIPByResolveSvr(string sServerIP, ushort wServerPort, string sDVRName, ushort wDVRNameLen, string sDVRSerialNumber, ushort wDVRSerialLen, IntPtr pGetIP);

		// Token: 0x06000801 RID: 2049
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDVRIPByResolveSvr_EX(string sServerIP, ushort wServerPort, ref byte sDVRName, ushort wDVRNameLen, ref byte sDVRSerialNumber, ushort wDVRSerialLen, string sGetIP, ref uint dwPort);

		// Token: 0x06000802 RID: 2050
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDVRWorkState(int lUserID, ref CHCNetSDK.NET_DVR_WORKSTATE lpWorkState);

		// Token: 0x06000803 RID: 2051
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetDVRWorkState_V30(int lUserID, IntPtr pWorkState);

		// Token: 0x06000804 RID: 2052
		[DllImport("HCNetSDK.dll")]
		public static extern string NET_DVR_GetErrorMsg(ref int pErrorNo);

		// Token: 0x06000805 RID: 2053
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetFileByName(int lUserID, string sDVRFileName, string sSavedFileName);

		// Token: 0x06000806 RID: 2054
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetFileByTime(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_TIME lpStartTime, ref CHCNetSDK.NET_DVR_TIME lpStopTime, string sSavedFileName);

		// Token: 0x06000807 RID: 2055
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetFormatProgress(int lFormatHandle, ref int pCurrentFormatDisk, ref int pCurrentDiskPos, ref int pFormatStatic);

		// Token: 0x06000808 RID: 2056
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_Getframeformat_V31(int lUserID, ref CHCNetSDK.tagNET_DVR_FRAMEFORMAT_V31 lpFrameFormat);

		// Token: 0x06000809 RID: 2057
		[DllImport("HCNetSDK.dll")]
		public static extern uint NET_DVR_GetLastError();

		// Token: 0x0600080A RID: 2058
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetLFTrackMode(int lUserID, int lChannel, ref CHCNetSDK.tagNET_DVR_LF_TRACK_MODE lpTrackMode);

		// Token: 0x0600080B RID: 2059
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetParamSetMode(int lUserID, ref uint dwParamSetMode);

		// Token: 0x0600080C RID: 2060
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetPlayBackOsdTime(int lPlayHandle, ref CHCNetSDK.NET_DVR_TIME lpOsdTime);

		// Token: 0x0600080D RID: 2061
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetPlayBackPlayerIndex(int lPlayHandle);

		// Token: 0x0600080E RID: 2062
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetPlayBackPos(int lPlayHandle);

		// Token: 0x0600080F RID: 2063
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetPTZCruise(int lUserID, int lChannel, int lCruiseRoute, ref CHCNetSDK.NET_DVR_CRUISE_RET lpCruiseRet);

		// Token: 0x06000810 RID: 2064
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetPTZCtrl(int lRealHandle);

		// Token: 0x06000811 RID: 2065
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetPTZCtrl_Other(int lUserID, int lChannel);

		// Token: 0x06000812 RID: 2066
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetPTZProtocol(int lUserID, ref CHCNetSDK.NET_DVR_PTZCFG pPtzcfg);

		// Token: 0x06000813 RID: 2067
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetRealHeight(int lUserID, int lChannel, ref CHCNetSDK.tagNET_VCA_LINE lpLine, ref float lpHeight);

		// Token: 0x06000814 RID: 2068
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetRealLength(int lUserID, int lChannel, ref CHCNetSDK.tagNET_VCA_LINE lpLine, ref float lpLength);

		// Token: 0x06000815 RID: 2069
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetRealPlayerIndex(int lRealHandle);

		// Token: 0x06000816 RID: 2070
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetRtspConfig(int lUserID, uint dwCommand, ref CHCNetSDK.NET_DVR_RTSPCFG lpOutBuffer, uint dwOutBufferSize);

		// Token: 0x06000817 RID: 2071
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetScaleCFG(int lUserID, ref uint lpOutScale);

		// Token: 0x06000818 RID: 2072
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetScaleCFG_V30(int lUserID, ref CHCNetSDK.NET_DVR_SCALECFG pScalecfg);

		// Token: 0x06000819 RID: 2073
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetSDKAbility(ref CHCNetSDK.NET_DVR_SDKABL pSDKAbl);

		// Token: 0x0600081A RID: 2074
		[DllImport("HCNetSDK.dll")]
		public static extern uint NET_DVR_GetSDKBuildVersion();

		// Token: 0x0600081B RID: 2075
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetSDKState(ref CHCNetSDK.NET_DVR_SDKSTATE pSDKState);

		// Token: 0x0600081C RID: 2076
		[DllImport("HCNetSDK.dll")]
		public static extern uint NET_DVR_GetSDKVersion();

		// Token: 0x0600081D RID: 2077
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetSerialNum_Card(int lChannelNum, ref uint pDeviceSerialNo);

		// Token: 0x0600081E RID: 2078
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetUpgradeProgress(int lUpgradeHandle);

		// Token: 0x0600081F RID: 2079
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_GetUpgradeState(int lUpgradeHandle);

		// Token: 0x06000820 RID: 2080
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetVCADrawMode(int lUserID, int lChannel, ref CHCNetSDK.tagNET_VCA_DRAW_MODE lpDrawMode);

		// Token: 0x06000821 RID: 2081
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_GetVideoEffect(int lUserID, int lChannel, ref uint pBrightValue, ref uint pContrastValue, ref uint pSaturationValue, ref uint pHueValue);

		// Token: 0x06000822 RID: 2082
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_Init();

		// Token: 0x06000823 RID: 2083
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_InitDDraw_Card(IntPtr hParent, uint colorKey);

		// Token: 0x06000824 RID: 2084
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_InitDDrawDevice();

		// Token: 0x06000825 RID: 2085
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_InitDevice_Card(ref int pDeviceTotalChan);

		// Token: 0x06000826 RID: 2086
		[DllImport("HCNetSDK.dll")]
		public static extern IntPtr NET_DVR_InitG722Decoder(int nBitrate);

		// Token: 0x06000827 RID: 2087
		[DllImport("HCNetSDK.dll")]
		public static extern IntPtr NET_DVR_InitG722Encoder();

		// Token: 0x06000828 RID: 2088
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_INVOKE_PLATE_RECOGNIZE(int lUserID, int lChannel, string pPicFileName, ref CHCNetSDK.tagNET_DVR_PLATE_RET pPlateRet, string pPicBuf, uint dwPicBufLen);

		// Token: 0x06000829 RID: 2089
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_IsSupport();

		// Token: 0x0600082A RID: 2090
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_LockFileByName(int lUserID, string sLockFileName);

		// Token: 0x0600082B RID: 2091
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_LockPanel(int lUserID);

		// Token: 0x0600082C RID: 2092
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_Login(string sDVRIP, ushort wDVRPort, string sUserName, string sPassword, ref CHCNetSDK.NET_DVR_DEVICEINFO lpDeviceInfo);

		// Token: 0x0600082D RID: 2093
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_Login_V30(string sDVRIP, int wDVRPort, string sUserName, string sPassword, ref CHCNetSDK.NET_DVR_DEVICEINFO_V30 lpDeviceInfo);

		// Token: 0x0600082E RID: 2094
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_LogoSwitch(int lUserID, uint dwDecChan, uint dwLogoSwitch);

		// Token: 0x0600082F RID: 2095
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_Logout(int iUserID);

		// Token: 0x06000830 RID: 2096
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_Logout_V30(int lUserID);

		// Token: 0x06000831 RID: 2097
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MakeKeyFrame(int lUserID, int lChannel);

		// Token: 0x06000832 RID: 2098
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MakeKeyFrameSub(int lUserID, int lChannel);

		// Token: 0x06000833 RID: 2099
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixDiaplayControl(int lUserID, uint dwDispChanNum, uint dwDispChanCmd, uint dwCmdParam);

		// Token: 0x06000834 RID: 2100
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetDecChanEnable(int lUserID, uint dwDecChanNum, ref uint lpdwEnable);

		// Token: 0x06000835 RID: 2101
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetDecChanInfo(int lUserID, uint dwDecChanNum, ref CHCNetSDK.NET_DVR_MATRIX_DEC_CHAN_INFO lpInter);

		// Token: 0x06000836 RID: 2102
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetDecChanInfo_V30(int lUserID, uint dwDecChanNum, ref CHCNetSDK.tagDEC_MATRIX_CHAN_INFO lpInter);

		// Token: 0x06000837 RID: 2103
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetDecChanStatus(int lUserID, uint dwDecChanNum, ref CHCNetSDK.NET_DVR_MATRIX_DEC_CHAN_STATUS lpInter);

		// Token: 0x06000838 RID: 2104
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetDeviceStatus(int lUserID, ref CHCNetSDK.tagNET_DVR__DECODER_WORK_STATUS lpDecoderCfg);

		// Token: 0x06000839 RID: 2105
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetDisplayCfg(int lUserID, uint dwDispChanNum, ref CHCNetSDK.tagNET_DVR_VGA_DISP_CHAN_CFG lpDisplayCfg);

		// Token: 0x0600083A RID: 2106
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetLoopDecChanEnable(int lUserID, uint dwDecChanNum, ref uint lpdwEnable);

		// Token: 0x0600083B RID: 2107
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetLoopDecChanInfo(int lUserID, uint dwDecChanNum, ref CHCNetSDK.NET_DVR_MATRIX_LOOP_DECINFO lpInter);

		// Token: 0x0600083C RID: 2108
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetLoopDecChanInfo_V30(int lUserID, uint dwDecChanNum, ref CHCNetSDK.tagMATRIX_LOOP_DECINFO_V30 lpInter);

		// Token: 0x0600083D RID: 2109
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetLoopDecEnable(int lUserID, ref uint lpdwEnable);

		// Token: 0x0600083E RID: 2110
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetRemotePlayStatus(int lUserID, uint dwDecChanNum, ref CHCNetSDK.NET_DVR_MATRIX_DEC_REMOTE_PLAY_STATUS lpOuter);

		// Token: 0x0600083F RID: 2111
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetTranInfo(int lUserID, ref CHCNetSDK.NET_DVR_MATRIX_TRAN_CHAN_CONFIG lpTranInfo);

		// Token: 0x06000840 RID: 2112
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixGetTranInfo_V30(int lUserID, ref CHCNetSDK.tagMATRIX_TRAN_CHAN_CONFIG lpTranInfo);

		// Token: 0x06000841 RID: 2113
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixSendData(int lPassiveHandle, IntPtr pSendBuf, uint dwBufSize);

		// Token: 0x06000842 RID: 2114
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixSetDecChanEnable(int lUserID, uint dwDecChanNum, uint dwEnable);

		// Token: 0x06000843 RID: 2115
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixSetDisplayCfg(int lUserID, uint dwDispChanNum, ref CHCNetSDK.tagNET_DVR_VGA_DISP_CHAN_CFG lpDisplayCfg);

		// Token: 0x06000844 RID: 2116
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixSetLoopDecChanEnable(int lUserID, uint dwDecChanNum, uint dwEnable);

		// Token: 0x06000845 RID: 2117
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixSetLoopDecChanInfo(int lUserID, uint dwDecChanNum, ref CHCNetSDK.NET_DVR_MATRIX_LOOP_DECINFO lpInter);

		// Token: 0x06000846 RID: 2118
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixSetLoopDecChanInfo_V30(int lUserID, uint dwDecChanNum, ref CHCNetSDK.tagMATRIX_LOOP_DECINFO_V30 lpInter);

		// Token: 0x06000847 RID: 2119
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixSetRemotePlay(int lUserID, uint dwDecChanNum, ref CHCNetSDK.NET_DVR_MATRIX_DEC_REMOTE_PLAY lpInter);

		// Token: 0x06000848 RID: 2120
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixSetRemotePlayControl(int lUserID, uint dwDecChanNum, uint dwControlCode, uint dwInValue, ref uint LPOutValue);

		// Token: 0x06000849 RID: 2121
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixSetTranInfo(int lUserID, ref CHCNetSDK.NET_DVR_MATRIX_TRAN_CHAN_CONFIG lpTranInfo);

		// Token: 0x0600084A RID: 2122
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixSetTranInfo_V30(int lUserID, ref CHCNetSDK.tagMATRIX_TRAN_CHAN_CONFIG lpTranInfo);

		// Token: 0x0600084B RID: 2123
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixStartDynamic(int lUserID, uint dwDecChanNum, ref CHCNetSDK.NET_DVR_MATRIX_DYNAMIC_DEC lpDynamicInfo);

		// Token: 0x0600084C RID: 2124
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixStartDynamic_V30(int lUserID, uint dwDecChanNum, ref CHCNetSDK.NET_DVR_PU_STREAM_CFG lpDynamicInfo);

		// Token: 0x0600084D RID: 2125
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_MatrixStartPassiveDecode(int lUserID, uint dwDecChanNum, ref CHCNetSDK.tagNET_MATRIX_PASSIVEMODE lpPassiveMode);

		// Token: 0x0600084E RID: 2126
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixStopDynamic(int lUserID, uint dwDecChanNum);

		// Token: 0x0600084F RID: 2127
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_MatrixStopPassiveDecode(int lPassiveHandle);

		// Token: 0x06000850 RID: 2128
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_OpenSound(int lRealHandle);

		// Token: 0x06000851 RID: 2129
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_OpenSound_Card(int lRealHandle);

		// Token: 0x06000852 RID: 2130
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_OpenSoundShare(int lRealHandle);

		// Token: 0x06000853 RID: 2131
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_PlayBackByName(int lUserID, string sPlayBackFileName, IntPtr hWnd);

		// Token: 0x06000854 RID: 2132
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_PlayBackByTime(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_TIME lpStartTime, ref CHCNetSDK.NET_DVR_TIME lpStopTime, IntPtr hWnd);

		// Token: 0x06000855 RID: 2133
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PlayBackCaptureFile(int lPlayHandle, string sFileName);

		// Token: 0x06000856 RID: 2134
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PlayBackControl(int lPlayHandle, uint dwControlCode, uint dwInValue, ref uint LPOutValue);

		// Token: 0x06000857 RID: 2135
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PlayBackSaveData(int lPlayHandle, string sFileName);

		// Token: 0x06000858 RID: 2136
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZControl(int lRealHandle, uint dwPTZCommand, uint dwStop);

		// Token: 0x06000859 RID: 2137
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZControl_EX(int lRealHandle, uint dwPTZCommand, uint dwStop);

		// Token: 0x0600085A RID: 2138
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZControl_Other(int lUserID, int lChannel, uint dwPTZCommand, uint dwStop);

		// Token: 0x0600085B RID: 2139
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZControlWithSpeed(int lRealHandle, uint dwPTZCommand, uint dwStop, uint dwSpeed);

		// Token: 0x0600085C RID: 2140
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZControlWithSpeed_EX(int lRealHandle, uint dwPTZCommand, uint dwStop, uint dwSpeed);

		// Token: 0x0600085D RID: 2141
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZControlWithSpeed_Other(int lUserID, int lChannel, int dwPTZCommand, int dwStop, int dwSpeed);

		// Token: 0x0600085E RID: 2142
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZCruise(int lRealHandle, uint dwPTZCruiseCmd, byte byCruiseRoute, byte byCruisePoint, ushort wInput);

		// Token: 0x0600085F RID: 2143
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZCruise_EX(int lRealHandle, uint dwPTZCruiseCmd, byte byCruiseRoute, byte byCruisePoint, ushort wInput);

		// Token: 0x06000860 RID: 2144
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZCruise_Other(int lUserID, int lChannel, uint dwPTZCruiseCmd, byte byCruiseRoute, byte byCruisePoint, ushort wInput);

		// Token: 0x06000861 RID: 2145
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZMltTrack(int lRealHandle, uint dwPTZTrackCmd, uint dwTrackIndex);

		// Token: 0x06000862 RID: 2146
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZMltTrack_EX(int lRealHandle, uint dwPTZTrackCmd, uint dwTrackIndex);

		// Token: 0x06000863 RID: 2147
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZMltTrack_Other(int lUserID, int lChannel, uint dwPTZTrackCmd, uint dwTrackIndex);

		// Token: 0x06000864 RID: 2148
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZPreset(int lRealHandle, uint dwPTZPresetCmd, uint dwPresetIndex);

		// Token: 0x06000865 RID: 2149
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZPreset_EX(int lRealHandle, uint dwPTZPresetCmd, uint dwPresetIndex);

		// Token: 0x06000866 RID: 2150
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZPreset_Other(int lUserID, int lChannel, uint dwPTZPresetCmd, uint dwPresetIndex);

		// Token: 0x06000867 RID: 2151
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZSelZoomIn(int lRealHandle, ref CHCNetSDK.NET_DVR_POINT_FRAME pStruPointFrame);

		// Token: 0x06000868 RID: 2152
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZSelZoomIn_EX(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_POINT_FRAME pStruPointFrame);

		// Token: 0x06000869 RID: 2153
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZTrack(int lRealHandle, uint dwPTZTrackCmd);

		// Token: 0x0600086A RID: 2154
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZTrack_EX(int lRealHandle, uint dwPTZTrackCmd);

		// Token: 0x0600086B RID: 2155
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_PTZTrack_Other(int lUserID, int lChannel, uint dwPTZTrackCmd);

		// Token: 0x0600086C RID: 2156
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_RealPlay(int iUserID, ref CHCNetSDK.NET_DVR_CLIENTINFO lpClientInfo);

		// Token: 0x0600086D RID: 2157
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_RealPlay_Card(int lUserID, ref CHCNetSDK.NET_DVR_CARDINFO lpCardInfo, int lChannelNum);

		// Token: 0x0600086E RID: 2158
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_RealPlay_V30(int iUserID, ref CHCNetSDK.NET_DVR_CLIENTINFO lpClientInfo, CHCNetSDK.REALDATACALLBACK fRealDataCallBack_V30, IntPtr pUser, uint bBlocked);

		// Token: 0x0600086F RID: 2159
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_RebootDVR(int lUserID);

		// Token: 0x06000870 RID: 2160
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_RefreshPlay(int lPlayHandle);

		// Token: 0x06000871 RID: 2161
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_RefreshSurface_Card();

		// Token: 0x06000872 RID: 2162
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ReleaseDDraw_Card();

		// Token: 0x06000873 RID: 2163
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ReleaseDDrawDevice();

		// Token: 0x06000874 RID: 2164
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ReleaseDevice_Card();

		// Token: 0x06000875 RID: 2165
		[DllImport("HCNetSDK.dll")]
		public static extern void NET_DVR_ReleaseG722Decoder(IntPtr pDecHandle);

		// Token: 0x06000876 RID: 2166
		[DllImport("HCNetSDK.dll")]
		public static extern void NET_DVR_ReleaseG722Encoder(IntPtr pEncodeHandle);

		// Token: 0x06000877 RID: 2167
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ResetPara_Card(int lRealHandle, ref CHCNetSDK.NET_DVR_DISPLAY_PARA lpDisplayPara);

		// Token: 0x06000878 RID: 2168
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_RestoreConfig(int lUserID);

		// Token: 0x06000879 RID: 2169
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_RestoreSurface_Card();

		// Token: 0x0600087A RID: 2170
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_RigisterDrawFun(int lRealHandle, CHCNetSDK.DRAWFUN fDrawFun, uint dwUser);

		// Token: 0x0600087B RID: 2171
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_RigisterPlayBackDrawFun(int lRealHandle, CHCNetSDK.DRAWFUN fDrawFun, uint dwUser);

		// Token: 0x0600087C RID: 2172
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SaveConfig(int lUserID);

		// Token: 0x0600087D RID: 2173
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SaveRealData(int lRealHandle, string sFileName);

		// Token: 0x0600087E RID: 2174
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SaveRealData_V30(int lRealHandle, uint dwTransType, string sFileName);

		// Token: 0x0600087F RID: 2175
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SendTo232Port(int lUserID, string pSendBuf, uint dwBufSize);

		// Token: 0x06000880 RID: 2176
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SendToSerialPort(int lUserID, uint dwSerialPort, uint dwSerialIndex, string pSendBuf, uint dwBufSize);

		// Token: 0x06000881 RID: 2177
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SerialSend(int lSerialHandle, int lChannel, string pSendBuf, uint dwBufSize);

		// Token: 0x06000882 RID: 2178
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SerialStart(int lUserID, int lSerialPort, CHCNetSDK.SERIALDATACALLBACK fSerialDataCallBack, uint dwUser);

		// Token: 0x06000883 RID: 2179
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SerialStop(int lSerialHandle);

		// Token: 0x06000884 RID: 2180
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetAlarmOut(int lUserID, int lAlarmOutPort, int lAlarmOutStatic);

		// Token: 0x06000885 RID: 2181
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetATMPortCFG(int lUserID, ushort wATMPort);

		// Token: 0x06000886 RID: 2182
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetAudioMode(uint dwMode);

		// Token: 0x06000887 RID: 2183
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetBehaviorParamKey(int lUserID, int lChannel, uint dwParameterKey, int nValue);

		// Token: 0x06000888 RID: 2184
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetCCDCfg(int lUserID, int lChannel, ref CHCNetSDK.tagNET_DVR_CCD_CFG lpCCDCfg);

		// Token: 0x06000889 RID: 2185
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetConfigFile(int lUserID, string sFileName);

		// Token: 0x0600088A RID: 2186
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetConfigFile_EX(int lUserID, string sInBuffer, uint dwInSize);

		// Token: 0x0600088B RID: 2187
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetConnectTime(uint dwWaitTime, uint dwTryTimes);

		// Token: 0x0600088C RID: 2188
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDDrawDevice(int lPlayPort, uint nDeviceNum);

		// Token: 0x0600088D RID: 2189
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDecInfo(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_DECCFG lpDecoderinfo);

		// Token: 0x0600088E RID: 2190
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDecTransPort(int lUserID, ref CHCNetSDK.NET_DVR_PORTCFG lpTransPort);

		// Token: 0x0600088F RID: 2191
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDVRConfig(int lUserID, uint dwCommand, int lChannel, IntPtr lpInBuffer, uint dwInBufferSize);

		// Token: 0x06000890 RID: 2192
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDVRMessage(uint nMessage, IntPtr hWnd);

		// Token: 0x06000891 RID: 2193
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDVRMessageCallBack(CHCNetSDK.MESSAGECALLBACK fMessageCallBack, uint dwUser);

		// Token: 0x06000892 RID: 2194
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDVRMessageCallBack_V30(CHCNetSDK.MSGCallBack fMessageCallBack, IntPtr pUser);

		// Token: 0x06000893 RID: 2195
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDVRMessCallBack(CHCNetSDK.MESSCALLBACK fMessCallBack);

		// Token: 0x06000894 RID: 2196
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDVRMessCallBack_EX(CHCNetSDK.MESSCALLBACKEX fMessCallBack_EX);

		// Token: 0x06000895 RID: 2197
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetDVRMessCallBack_NEW(CHCNetSDK.MESSCALLBACKNEW fMessCallBack_NEW);

		// Token: 0x06000896 RID: 2198
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetExceptionCallBack_V30(uint nMessage, IntPtr hWnd, CHCNetSDK.EXCEPYIONCALLBACK fExceptionCallBack, IntPtr pUser);

		// Token: 0x06000897 RID: 2199
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_Setframeformat_V31(int lUserID, ref CHCNetSDK.tagNET_DVR_FRAMEFORMAT_V31 lpFrameFormat);

		// Token: 0x06000898 RID: 2200
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetLFTrackMode(int lUserID, int lChannel, ref CHCNetSDK.tagNET_DVR_LF_TRACK_MODE lpTrackMode);

		// Token: 0x06000899 RID: 2201
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetLogToFile(int bLogEnable, string strLogDir, int bAutoDel);

		// Token: 0x0600089A RID: 2202
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetNetworkEnvironment(uint dwEnvironmentLevel);

		// Token: 0x0600089B RID: 2203
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetPlayDataCallBack(int lPlayHandle, CHCNetSDK.PLAYDATACALLBACK fPlayDataCallBack, uint dwUser);

		// Token: 0x0600089C RID: 2204
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetPlayerBufNumber(int lRealHandle, uint dwBufNum);

		// Token: 0x0600089D RID: 2205
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetRealDataCallBack(int lRealHandle, CHCNetSDK.SETREALDATACALLBACK fRealDataCallBack, uint dwUser);

		// Token: 0x0600089E RID: 2206
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetReconnect(uint dwInterval, int bEnableRecon);

		// Token: 0x0600089F RID: 2207
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetRtspConfig(int lUserID, uint dwCommand, ref CHCNetSDK.NET_DVR_RTSPCFG lpInBuffer, uint dwInBufferSize);

		// Token: 0x060008A0 RID: 2208
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetScaleCFG(int lUserID, uint dwScale);

		// Token: 0x060008A1 RID: 2209
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetScaleCFG_V30(int lUserID, ref CHCNetSDK.NET_DVR_SCALECFG pScalecfg);

		// Token: 0x060008A2 RID: 2210
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetShowMode(uint dwShowType, uint colorKey);

		// Token: 0x060008A3 RID: 2211
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetStandardDataCallBack(int lRealHandle, CHCNetSDK.STDDATACALLBACK fStdDataCallBack, uint dwUser);

		// Token: 0x060008A4 RID: 2212
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_SetupAlarmChan(int lUserID);

		// Token: 0x060008A5 RID: 2213
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_SetupAlarmChan_V30(int lUserID);

		// Token: 0x060008A6 RID: 2214
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetVCADrawMode(int lUserID, int lChannel, ref CHCNetSDK.tagNET_VCA_DRAW_MODE lpDrawMode);

		// Token: 0x060008A7 RID: 2215
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetVideoEffect(int lUserID, int lChannel, uint dwBrightValue, uint dwContrastValue, uint dwSaturationValue, uint dwHueValue);

		// Token: 0x060008A8 RID: 2216
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetVoiceComClientVolume(int lVoiceComHandle, ushort wVolume);

		// Token: 0x060008A9 RID: 2217
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_SetVolume_Card(int lRealHandle, ushort wVolume);

		// Token: 0x060008AA RID: 2218
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ShutDownDVR(int lUserID);

		// Token: 0x060008AB RID: 2219
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StartDecode(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_DECODERINFO lpDecoderinfo);

		// Token: 0x060008AC RID: 2220
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StartDecSpecialCon(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_DECCHANINFO lpDecChanInfo);

		// Token: 0x060008AD RID: 2221
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StartDVRRecord(int lUserID, int lChannel, int lRecordType);

		// Token: 0x060008AE RID: 2222
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StartListen(string sLocalIP, ushort wLocalPort);

		// Token: 0x060008AF RID: 2223
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_StartListen_V30(string sLocalIP, ushort wLocalPort, CHCNetSDK.MSGCallBack DataCallback, IntPtr pUserData);

		// Token: 0x060008B0 RID: 2224
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_StartVoiceCom(int lUserID, CHCNetSDK.VOICEDATACALLBACK fVoiceDataCallBack, uint dwUser);

		// Token: 0x060008B1 RID: 2225
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_StartVoiceCom_MR(int lUserID, CHCNetSDK.VOICEDATACALLBACK fVoiceDataCallBack, uint dwUser);

		// Token: 0x060008B2 RID: 2226
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_StartVoiceCom_MR_V30(int lUserID, uint dwVoiceChan, CHCNetSDK.VOICEDATACALLBACKV30 fVoiceDataCallBack, IntPtr pUser);

		// Token: 0x060008B3 RID: 2227
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_StartVoiceCom_V30(int lUserID, uint dwVoiceChan, bool bNeedCBNoEncData, CHCNetSDK.VOICEDATACALLBACKV30 fVoiceDataCallBack, IntPtr pUser);

		// Token: 0x060008B4 RID: 2228
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopDecode(int lUserID, int lChannel);

		// Token: 0x060008B5 RID: 2229
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopDecSpecialCon(int lUserID, int lChannel, ref CHCNetSDK.NET_DVR_DECCHANINFO lpDecChanInfo);

		// Token: 0x060008B6 RID: 2230
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopDVRRecord(int lUserID, int lChannel);

		// Token: 0x060008B7 RID: 2231
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopGetFile(int lFileHandle);

		// Token: 0x060008B8 RID: 2232
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopListen();

		// Token: 0x060008B9 RID: 2233
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopListen_V30(int lListenHandle);

		// Token: 0x060008BA RID: 2234
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopPlayBack(int lPlayHandle);

		// Token: 0x060008BB RID: 2235
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopPlayBackSave(int lPlayHandle);

		// Token: 0x060008BC RID: 2236
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopRealPlay(int iRealHandle);

		// Token: 0x060008BD RID: 2237
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopSaveRealData(int lRealHandle);

		// Token: 0x060008BE RID: 2238
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_StopVoiceCom(int lVoiceComHandle);

		// Token: 0x060008BF RID: 2239
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_ThrowBFrame(int lRealHandle, uint dwNum);

		// Token: 0x060008C0 RID: 2240
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_TransPTZ(int lRealHandle, string pPTZCodeBuf, uint dwBufSize);

		// Token: 0x060008C1 RID: 2241
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_TransPTZ_EX(int lRealHandle, string pPTZCodeBuf, uint dwBufSize);

		// Token: 0x060008C2 RID: 2242
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_TransPTZ_Other(int lUserID, int lChannel, string pPTZCodeBuf, uint dwBufSize);

		// Token: 0x060008C3 RID: 2243
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_UnlockFileByName(int lUserID, string sUnlockFileName);

		// Token: 0x060008C4 RID: 2244
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_UnLockPanel(int lUserID);

		// Token: 0x060008C5 RID: 2245
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_DVR_Upgrade(int lUserID, string sFileName);

		// Token: 0x060008C6 RID: 2246
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_UploadLogo(int lUserID, uint dwDispChanNum, ref CHCNetSDK.tagNET_DVR_DISP_LOGOCFG lpDispLogoCfg, IntPtr sLogoBuffer);

		// Token: 0x060008C7 RID: 2247
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_VoiceComSendData(int lVoiceComHandle, string pSendBuf, uint dwBufSize);

		// Token: 0x060008C8 RID: 2248
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_DVR_Volume(int lRealHandle, ushort wVolume);

		// Token: 0x060008C9 RID: 2249
		[DllImport("HCNetSDK.dll")]
		public static extern int NET_SDK_RealPlay(int iUserLogID, ref CHCNetSDK.NET_SDK_CLIENTINFO lpDVRClientInfo);

		// Token: 0x060008CA RID: 2250
		[DllImport("HCNetSDK.dll")]
		public static extern bool NET_VCA_RestartLib(int lUserID, int lChannel);

		// Token: 0x060008CB RID: 2251
		[DllImport("RecordDLL.dll")]
		public static extern int OpenChannelRecord(string strCameraid, IntPtr pHead, uint dwHeadLength);

		// Token: 0x060008CC RID: 2252
		[DllImport("RecordDLL.dll")]
		public static extern int Release();

		// Token: 0x060008CD RID: 2253
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODCloseDownloadStream(IntPtr hStream);

		// Token: 0x060008CE RID: 2254
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODCloseStream(IntPtr hStream);

		// Token: 0x060008CF RID: 2255
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODDeleteSectionList(IntPtr pSecList);

		// Token: 0x060008D0 RID: 2256
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODGetStreamCurrentTime(IntPtr hStream, ref CHCNetSDK.BLOCKTIME pCurrentTime);

		// Token: 0x060008D1 RID: 2257
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODOpenDownloadStream(ref CHCNetSDK.VODOPENPARAM struVodParam, ref IntPtr phStream);

		// Token: 0x060008D2 RID: 2258
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODOpenStream(IntPtr pOpenParam, ref IntPtr phStream);

		// Token: 0x060008D3 RID: 2259
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODPauseStreamData(IntPtr hStream, bool bPause);

		// Token: 0x060008D4 RID: 2260
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODSeekStreamData(IntPtr hStream, IntPtr pStartTime);

		// Token: 0x060008D5 RID: 2261
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODServerConnect(string strServerIp, uint uiServerPort, ref IntPtr hSession, ref CHCNetSDK.CONNPARAM struConn, IntPtr hWnd);

		// Token: 0x060008D6 RID: 2262
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODServerDisconnect(IntPtr hSession);

		// Token: 0x060008D7 RID: 2263
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODSetStreamSpeed(IntPtr hStream, int iSpeed);

		// Token: 0x060008D8 RID: 2264
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODStartStreamData(IntPtr phStream);

		// Token: 0x060008D9 RID: 2265
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODStopStreamData(IntPtr hStream);

		// Token: 0x060008DA RID: 2266
		[DllImport("PdCssVodClient.dll")]
		public static extern bool VODStreamSearch(IntPtr pSearchParam, ref IntPtr pSecList);

		// Token: 0x04000D5F RID: 3423
		public const int ATM_DESC_LEN = 32;

		// Token: 0x04000D60 RID: 3424
		public const int ATM_FRAMETYPE_NUM = 4;

		// Token: 0x04000D61 RID: 3425
		public const int ATM_PROTOCOL_SORT = 4;

		// Token: 0x04000D62 RID: 3426
		public const int ATMDVR = 2;

		// Token: 0x04000D63 RID: 3427
		public const int ATMDVR_S = 18;

		// Token: 0x04000D64 RID: 3428
		public const int AUDIO_PACKET = 10;

		// Token: 0x04000D65 RID: 3429
		public const int AUX_PWRON1 = 6;

		// Token: 0x04000D66 RID: 3430
		public const int AUX_PWRON2 = 7;

		// Token: 0x04000D67 RID: 3431
		public const int BULL = 9;

		// Token: 0x04000D68 RID: 3432
		public const int CARDNUM_LEN = 20;

		// Token: 0x04000D69 RID: 3433
		public const int CHANNELTYPE = 3;

		// Token: 0x04000D6A RID: 3434
		public const int ChenTong = 16;

		// Token: 0x04000D6B RID: 3435
		public const int CLE_PRE_SEQ = 33;

		// Token: 0x04000D6C RID: 3436
		public const int CLE_PRESET = 9;

		// Token: 0x04000D6D RID: 3437
		public const int COMM_ALARM = 4352;

		// Token: 0x04000D6E RID: 3438
		public const int COMM_ALARM_PLATE = 4353;

		// Token: 0x04000D6F RID: 3439
		public const int COMM_ALARM_RULE = 4354;

		// Token: 0x04000D70 RID: 3440
		public const int COMM_ALARM_RULE_CALC = 4368;

		// Token: 0x04000D71 RID: 3441
		public const int COMM_ALARM_V30 = 16384;

		// Token: 0x04000D72 RID: 3442
		public const int COMM_IPCCFG = 16385;

		// Token: 0x04000D73 RID: 3443
		public const int COMM_IPCCFG_V31 = 16386;

		// Token: 0x04000D74 RID: 3444
		public const int COMM_TRADEINFO = 5376;

		// Token: 0x04000D75 RID: 3445
		public const int CRUISE_MAX_PRESET_NUMS = 32;

		// Token: 0x04000D76 RID: 3446
		public const int DATASTREAM_BITBLOCK = 1;

		// Token: 0x04000D77 RID: 3447
		public const int DATASTREAM_HEAD = 0;

		// Token: 0x04000D78 RID: 3448
		public const int DATASTREAM_KEYFRAME = 2;

		// Token: 0x04000D79 RID: 3449
		public const int DATASTREAM_NORMALFRAME = 3;

		// Token: 0x04000D7A RID: 3450
		public const int DEC = 4;

		// Token: 0x04000D7B RID: 3451
		public const int DEC_MAT = 20;

		// Token: 0x04000D7C RID: 3452
		public const int DECODE_TIMESEGMENT = 4;

		// Token: 0x04000D7D RID: 3453
		public const int DEF_ALARM_ERRORVISIT = 9;

		// Token: 0x04000D7E RID: 3454
		public const int DEF_ALARM_EXCEPTION = 10;

		// Token: 0x04000D7F RID: 3455
		public const int DEF_ALARM_HARDERROR = 6;

		// Token: 0x04000D80 RID: 3456
		public const int DEF_ALARM_HARDFORMAT = 5;

		// Token: 0x04000D81 RID: 3457
		public const int DEF_ALARM_HARDFULL = 2;

		// Token: 0x04000D82 RID: 3458
		public const int DEF_ALARM_IO = 1;

		// Token: 0x04000D83 RID: 3459
		public const int DEF_ALARM_MV = 4;

		// Token: 0x04000D84 RID: 3460
		public const int DEF_ALARM_NOPATCH = 8;

		// Token: 0x04000D85 RID: 3461
		public const int DEF_ALARM_RECERROR = 11;

		// Token: 0x04000D86 RID: 3462
		public const int DEF_ALARM_VH = 7;

		// Token: 0x04000D87 RID: 3463
		public const int DEF_ALARM_VL = 3;

		// Token: 0x04000D88 RID: 3464
		public const int DEF_OPE_ALARM_SETALARM = 1;

		// Token: 0x04000D89 RID: 3465
		public const int DEF_OPE_ALARM_WITHDRAWALARM = 2;

		// Token: 0x04000D8A RID: 3466
		public const int DEF_OPE_CHECKT_CHECKTIME = 1;

		// Token: 0x04000D8B RID: 3467
		public const int DEF_OPE_CHECKTIME = 9;

		// Token: 0x04000D8C RID: 3468
		public const int DEF_OPE_GETSERVSTATE = 8;

		// Token: 0x04000D8D RID: 3469
		public const int DEF_OPE_PLAYBACK = 6;

		// Token: 0x04000D8E RID: 3470
		public const int DEF_OPE_PLAYBACK_LOCALDOWNLOAD = 3;

		// Token: 0x04000D8F RID: 3471
		public const int DEF_OPE_PLAYBACK_LOCALPLAY = 2;

		// Token: 0x04000D90 RID: 3472
		public const int DEF_OPE_PLAYBACK_LOCALSEARCH = 1;

		// Token: 0x04000D91 RID: 3473
		public const int DEF_OPE_PLAYBACK_REMOTEDOWNLOAD = 7;

		// Token: 0x04000D92 RID: 3474
		public const int DEF_OPE_PLAYBACK_REMOTEPLAYFILE = 5;

		// Token: 0x04000D93 RID: 3475
		public const int DEF_OPE_PLAYBACK_REMOTEPLAYTIME = 6;

		// Token: 0x04000D94 RID: 3476
		public const int DEF_OPE_PLAYBACK_REMOTESEARCH = 4;

		// Token: 0x04000D95 RID: 3477
		public const int DEF_OPE_PRE_CAPTURE = 7;

		// Token: 0x04000D96 RID: 3478
		public const int DEF_OPE_PRE_CLOSESOUND = 9;

		// Token: 0x04000D97 RID: 3479
		public const int DEF_OPE_PRE_OPENSOUND = 8;

		// Token: 0x04000D98 RID: 3480
		public const int DEF_OPE_PRE_STARTPREVIEW = 1;

		// Token: 0x04000D99 RID: 3481
		public const int DEF_OPE_PRE_STARTRECORD = 5;

		// Token: 0x04000D9A RID: 3482
		public const int DEF_OPE_PRE_STOPCYCPLAY = 4;

		// Token: 0x04000D9B RID: 3483
		public const int DEF_OPE_PRE_STOPPREVIEW = 2;

		// Token: 0x04000D9C RID: 3484
		public const int DEF_OPE_PRE_STOPRECORD = 6;

		// Token: 0x04000D9D RID: 3485
		public const int DEF_OPE_PRE_STRATCYCPLAY = 3;

		// Token: 0x04000D9E RID: 3486
		public const int DEF_OPE_PREVIEW = 1;

		// Token: 0x04000D9F RID: 3487
		public const int DEF_OPE_PTZ_PTZCTRL = 1;

		// Token: 0x04000DA0 RID: 3488
		public const int DEF_OPE_PTZCTRL = 4;

		// Token: 0x04000DA1 RID: 3489
		public const int DEF_OPE_REMOTE_REMOTECFG = 1;

		// Token: 0x04000DA2 RID: 3490
		public const int DEF_OPE_REMOTECFG = 7;

		// Token: 0x04000DA3 RID: 3491
		public const int DEF_OPE_SETALARM = 3;

		// Token: 0x04000DA4 RID: 3492
		public const int DEF_OPE_STATE_GETSERVSTATE = 1;

		// Token: 0x04000DA5 RID: 3493
		public const int DEF_OPE_TALK = 2;

		// Token: 0x04000DA6 RID: 3494
		public const int DEF_OPE_TALK_STARTTALK = 1;

		// Token: 0x04000DA7 RID: 3495
		public const int DEF_OPE_TALK_STOPTALK = 2;

		// Token: 0x04000DA8 RID: 3496
		public const int DEF_OPE_VIDEOPARAM = 5;

		// Token: 0x04000DA9 RID: 3497
		public const int DEF_OPE_VIDEOPARAM_SET = 1;

		// Token: 0x04000DAA RID: 3498
		public const int DEF_SYS_LOCALCFG = 3;

		// Token: 0x04000DAB RID: 3499
		public const int DEF_SYS_LOGIN = 1;

		// Token: 0x04000DAC RID: 3500
		public const int DEF_SYS_LOGOUT = 2;

		// Token: 0x04000DAD RID: 3501
		public const int DELETETYPE = 2;

		// Token: 0x04000DAE RID: 3502
		public const int DESC_LEN = 16;

		// Token: 0x04000DAF RID: 3503
		public const int DEVICETYPE = 2;

		// Token: 0x04000DB0 RID: 3504
		public const int DIEBOLD = 1;

		// Token: 0x04000DB1 RID: 3505
		public const int DISP_CMD_ENLARGE_WINDOW = 1;

		// Token: 0x04000DB2 RID: 3506
		public const int DISP_CMD_RENEW_WINDOW = 2;

		// Token: 0x04000DB3 RID: 3507
		public const int DongXin = 15;

		// Token: 0x04000DB4 RID: 3508
		public const int DOWN_LEFT = 27;

		// Token: 0x04000DB5 RID: 3509
		public const int DOWN_RIGHT = 28;

		// Token: 0x04000DB6 RID: 3510
		public const int DRS918 = 21;

		// Token: 0x04000DB7 RID: 3511
		public const int DS6001_HF_B = 60;

		// Token: 0x04000DB8 RID: 3512
		public const int DS6001_HF_P = 61;

		// Token: 0x04000DB9 RID: 3513
		public const int DS6002_HF_B = 62;

		// Token: 0x04000DBA RID: 3514
		public const int DS6101_HF_B = 63;

		// Token: 0x04000DBB RID: 3515
		public const int DS6101_HF_P = 67;

		// Token: 0x04000DBC RID: 3516
		public const int DS630X_D = 27;

		// Token: 0x04000DBD RID: 3517
		public const int DS71XX_H = 71;

		// Token: 0x04000DBE RID: 3518
		public const int DS72XX_H_S = 72;

		// Token: 0x04000DBF RID: 3519
		public const int DS73XX_H_S = 73;

		// Token: 0x04000DC0 RID: 3520
		public const int DS76XX_H_S = 76;

		// Token: 0x04000DC1 RID: 3521
		public const int DS8000HC_NVR = 0;

		// Token: 0x04000DC2 RID: 3522
		public const int DS8000ME_NVR = 2;

		// Token: 0x04000DC3 RID: 3523
		public const int DS8004_AHL_A = 66;

		// Token: 0x04000DC4 RID: 3524
		public const int DS81XX_AH_S = 87;

		// Token: 0x04000DC5 RID: 3525
		public const int DS81XX_AHF_S = 88;

		// Token: 0x04000DC6 RID: 3526
		public const int DS81XX_HC_S = 83;

		// Token: 0x04000DC7 RID: 3527
		public const int DS81XX_HD_S = 84;

		// Token: 0x04000DC8 RID: 3528
		public const int DS81XX_HE_S = 85;

		// Token: 0x04000DC9 RID: 3529
		public const int DS81XX_HF_S = 86;

		// Token: 0x04000DCA RID: 3530
		public const int DS81XX_HL_S = 82;

		// Token: 0x04000DCB RID: 3531
		public const int DS81XX_HS_S = 81;

		// Token: 0x04000DCC RID: 3532
		public const int DS9000_IVS = 65;

		// Token: 0x04000DCD RID: 3533
		public const int DS9000HC_NVR = 1;

		// Token: 0x04000DCE RID: 3534
		public const int DS90XX_HF_S = 90;

		// Token: 0x04000DCF RID: 3535
		public const int DS91XX_HD_S = 92;

		// Token: 0x04000DD0 RID: 3536
		public const int DS91XX_HF_S = 91;

		// Token: 0x04000DD1 RID: 3537
		public const int DVR = 1;

		// Token: 0x04000DD2 RID: 3538
		public const int DVR_HB = 11;

		// Token: 0x04000DD3 RID: 3539
		public const int DVR_HC = 6;

		// Token: 0x04000DD4 RID: 3540
		public const int DVR_HC_S = 14;

		// Token: 0x04000DD5 RID: 3541
		public const int DVR_HC_SL = 24;

		// Token: 0x04000DD6 RID: 3542
		public const int DVR_HCS = 12;

		// Token: 0x04000DD7 RID: 3543
		public const int DVR_HD_S = 22;

		// Token: 0x04000DD8 RID: 3544
		public const int DVR_HD_SL = 23;

		// Token: 0x04000DD9 RID: 3545
		public const int DVR_HF = 8;

		// Token: 0x04000DDA RID: 3546
		public const int DVR_HF_S = 16;

		// Token: 0x04000DDB RID: 3547
		public const int DVR_HS = 9;

		// Token: 0x04000DDC RID: 3548
		public const int DVR_HS_S = 17;

		// Token: 0x04000DDD RID: 3549
		public const int DVR_HS_ST = 25;

		// Token: 0x04000DDE RID: 3550
		public const int DVR_HT = 7;

		// Token: 0x04000DDF RID: 3551
		public const int DVR_HT_S = 15;

		// Token: 0x04000DE0 RID: 3552
		public const int DVR_HTS = 10;

		// Token: 0x04000DE1 RID: 3553
		public const int DVR_MOBILE = 21;

		// Token: 0x04000DE2 RID: 3554
		public const int DVS = 3;

		// Token: 0x04000DE3 RID: 3555
		public const int DVS_A = 13;

		// Token: 0x04000DE4 RID: 3556
		public const int DVS_HW = 26;

		// Token: 0x04000DE5 RID: 3557
		public const int ENC_DEC = 5;

		// Token: 0x04000DE6 RID: 3558
		public const int EXCEPTION_ALARM = 32770;

		// Token: 0x04000DE7 RID: 3559
		public const int EXCEPTION_ALARMRECONNECT = 32774;

		// Token: 0x04000DE8 RID: 3560
		public const int EXCEPTION_AUDIOEXCHANGE = 32769;

		// Token: 0x04000DE9 RID: 3561
		public const int EXCEPTION_DISKFMT = 32785;

		// Token: 0x04000DEA RID: 3562
		public const int EXCEPTION_EXCHANGE = 32768;

		// Token: 0x04000DEB RID: 3563
		public const int EXCEPTION_PLAYBACK = 32784;

		// Token: 0x04000DEC RID: 3564
		public const int EXCEPTION_PREVIEW = 32771;

		// Token: 0x04000DED RID: 3565
		public const int EXCEPTION_RECONNECT = 32773;

		// Token: 0x04000DEE RID: 3566
		public const int EXCEPTION_SERIAL = 32772;

		// Token: 0x04000DEF RID: 3567
		public const int EXCEPTION_SERIALRECONNECT = 32775;

		// Token: 0x04000DF0 RID: 3568
		public const int FAN_PWRON = 4;

		// Token: 0x04000DF1 RID: 3569
		public const int FILE_HEAD = 0;

		// Token: 0x04000DF2 RID: 3570
		public const int FILE_NAME_LEN = 256;

		// Token: 0x04000DF3 RID: 3571
		public const int FILL_PRE_SEQ = 30;

		// Token: 0x04000DF4 RID: 3572
		public const int FOCUS_FAR = 14;

		// Token: 0x04000DF5 RID: 3573
		public const int FOCUS_NEAR = 13;

		// Token: 0x04000DF6 RID: 3574
		public const int FUJITSU = 5;

		// Token: 0x04000DF7 RID: 3575
		public const int GDYT = 12;

		// Token: 0x04000DF8 RID: 3576
		public const int GOTO_PRESET = 39;

		// Token: 0x04000DF9 RID: 3577
		public const int GuangLi = 14;

		// Token: 0x04000DFA RID: 3578
		public const int GZYY = 19;

		// Token: 0x04000DFB RID: 3579
		public const int HC_76NVR = 8;

		// Token: 0x04000DFC RID: 3580
		public const int HC_9000 = 4;

		// Token: 0x04000DFD RID: 3581
		public const int HCDVR = 1;

		// Token: 0x04000DFE RID: 3582
		public const int HEATER_PWRON = 5;

		// Token: 0x04000DFF RID: 3583
		public const int HF_I = 5;

		// Token: 0x04000E00 RID: 3584
		public const int HITACHI = 6;

		// Token: 0x04000E01 RID: 3585
		public const int IBM = 8;

		// Token: 0x04000E02 RID: 3586
		public const int IDS52XX = 64;

		// Token: 0x04000E03 RID: 3587
		public const int INSERTTYPE = 0;

		// Token: 0x04000E04 RID: 3588
		public const int IPCAM = 30;

		// Token: 0x04000E05 RID: 3589
		public const int IPCAM_X62MF = 32;

		// Token: 0x04000E06 RID: 3590
		public const int IPDOME = 40;

		// Token: 0x04000E07 RID: 3591
		public const int IPDOME_MEGA130 = 42;

		// Token: 0x04000E08 RID: 3592
		public const int IPDOME_MEGA200 = 41;

		// Token: 0x04000E09 RID: 3593
		public const int IPMOD = 50;

		// Token: 0x04000E0A RID: 3594
		public const int IRIS_CLOSE = 16;

		// Token: 0x04000E0B RID: 3595
		public const int IRIS_OPEN = 15;

		// Token: 0x04000E0C RID: 3596
		public const int IW_ENCODING_TOKEN_MAX = 32;

		// Token: 0x04000E0D RID: 3597
		public const int IW_ESSID_MAX_SIZE = 32;

		// Token: 0x04000E0E RID: 3598
		public const int KALATEL = 22;

		// Token: 0x04000E0F RID: 3599
		public const int KEY_CODE_0 = 10;

		// Token: 0x04000E10 RID: 3600
		public const int KEY_CODE_1 = 1;

		// Token: 0x04000E11 RID: 3601
		public const int KEY_CODE_11 = 44;

		// Token: 0x04000E12 RID: 3602
		public const int KEY_CODE_12 = 45;

		// Token: 0x04000E13 RID: 3603
		public const int KEY_CODE_13 = 46;

		// Token: 0x04000E14 RID: 3604
		public const int KEY_CODE_14 = 47;

		// Token: 0x04000E15 RID: 3605
		public const int KEY_CODE_15 = 48;

		// Token: 0x04000E16 RID: 3606
		public const int KEY_CODE_16 = 49;

		// Token: 0x04000E17 RID: 3607
		public const int KEY_CODE_2 = 2;

		// Token: 0x04000E18 RID: 3608
		public const int KEY_CODE_3 = 3;

		// Token: 0x04000E19 RID: 3609
		public const int KEY_CODE_4 = 4;

		// Token: 0x04000E1A RID: 3610
		public const int KEY_CODE_5 = 5;

		// Token: 0x04000E1B RID: 3611
		public const int KEY_CODE_6 = 6;

		// Token: 0x04000E1C RID: 3612
		public const int KEY_CODE_7 = 7;

		// Token: 0x04000E1D RID: 3613
		public const int KEY_CODE_8 = 8;

		// Token: 0x04000E1E RID: 3614
		public const int KEY_CODE_9 = 9;

		// Token: 0x04000E1F RID: 3615
		public const int KEY_CODE_A = 26;

		// Token: 0x04000E20 RID: 3616
		public const int KEY_CODE_ADD = 20;

		// Token: 0x04000E21 RID: 3617
		public const int KEY_CODE_CANCEL = 14;

		// Token: 0x04000E22 RID: 3618
		public const int KEY_CODE_DOWN = 16;

		// Token: 0x04000E23 RID: 3619
		public const int KEY_CODE_EDIT = 19;

		// Token: 0x04000E24 RID: 3620
		public const int KEY_CODE_ENTER = 13;

		// Token: 0x04000E25 RID: 3621
		public const int KEY_CODE_F1 = 27;

		// Token: 0x04000E26 RID: 3622
		public const int KEY_CODE_F2 = 28;

		// Token: 0x04000E27 RID: 3623
		public const int KEY_CODE_LEFT = 17;

		// Token: 0x04000E28 RID: 3624
		public const int KEY_CODE_M = 25;

		// Token: 0x04000E29 RID: 3625
		public const int KEY_CODE_MENU = 12;

		// Token: 0x04000E2A RID: 3626
		public const int KEY_CODE_MINUS = 21;

		// Token: 0x04000E2B RID: 3627
		public const int KEY_CODE_PAN = 24;

		// Token: 0x04000E2C RID: 3628
		public const int KEY_CODE_PLAY = 22;

		// Token: 0x04000E2D RID: 3629
		public const int KEY_CODE_POWER = 11;

		// Token: 0x04000E2E RID: 3630
		public const int KEY_CODE_REC = 23;

		// Token: 0x04000E2F RID: 3631
		public const int KEY_CODE_RIGHT = 18;

		// Token: 0x04000E30 RID: 3632
		public const int KEY_CODE_UP = 15;

		// Token: 0x04000E31 RID: 3633
		public const int KEY_PTZ_AP1_START = 19;

		// Token: 0x04000E32 RID: 3634
		public const int KEY_PTZ_AP1_STOP = 36;

		// Token: 0x04000E33 RID: 3635
		public const int KEY_PTZ_AP2_START = 24;

		// Token: 0x04000E34 RID: 3636
		public const int KEY_PTZ_AP2_STOP = 37;

		// Token: 0x04000E35 RID: 3637
		public const int KEY_PTZ_B1_START = 40;

		// Token: 0x04000E36 RID: 3638
		public const int KEY_PTZ_B1_STOP = 41;

		// Token: 0x04000E37 RID: 3639
		public const int KEY_PTZ_B2_START = 42;

		// Token: 0x04000E38 RID: 3640
		public const int KEY_PTZ_B2_STOP = 43;

		// Token: 0x04000E39 RID: 3641
		public const int KEY_PTZ_DOWN_START = 16;

		// Token: 0x04000E3A RID: 3642
		public const int KEY_PTZ_DOWN_STOP = 33;

		// Token: 0x04000E3B RID: 3643
		public const int KEY_PTZ_FOCUS1_START = 26;

		// Token: 0x04000E3C RID: 3644
		public const int KEY_PTZ_FOCUS1_STOP = 38;

		// Token: 0x04000E3D RID: 3645
		public const int KEY_PTZ_FOCUS2_START = 25;

		// Token: 0x04000E3E RID: 3646
		public const int KEY_PTZ_FOCUS2_STOP = 39;

		// Token: 0x04000E3F RID: 3647
		public const int KEY_PTZ_LEFT_START = 17;

		// Token: 0x04000E40 RID: 3648
		public const int KEY_PTZ_LEFT_STOP = 34;

		// Token: 0x04000E41 RID: 3649
		public const int KEY_PTZ_RIGHT_START = 18;

		// Token: 0x04000E42 RID: 3650
		public const int KEY_PTZ_RIGHT_STOP = 35;

		// Token: 0x04000E43 RID: 3651
		public const int KEY_PTZ_UP_START = 15;

		// Token: 0x04000E44 RID: 3652
		public const int KEY_PTZ_UP_STOP = 32;

		// Token: 0x04000E45 RID: 3653
		public const int LiDe = 11;

		// Token: 0x04000E46 RID: 3654
		public const int LIGHT_PWRON = 2;

		// Token: 0x04000E47 RID: 3655
		public const int LOG_INFO_LEN = 11840;

		// Token: 0x04000E48 RID: 3656
		public const int LOWCOST_DVR = 19;

		// Token: 0x04000E49 RID: 3657
		public const int MACADDR_LEN = 6;

		// Token: 0x04000E4A RID: 3658
		public const int MAJOR_ALARM = 1;

		// Token: 0x04000E4B RID: 3659
		public const int MAJOR_EXCEPTION = 2;

		// Token: 0x04000E4C RID: 3660
		public const int MAJOR_INFORMATION = 4;

		// Token: 0x04000E4D RID: 3661
		public const int MAJOR_OPERATION = 3;

		// Token: 0x04000E4E RID: 3662
		public const int MATRIXDECODER_ABILITY = 512;

		// Token: 0x04000E4F RID: 3663
		public const int MATRIXTYPE = 11;

		// Token: 0x04000E50 RID: 3664
		public const int MAX_ACTION_TYPE = 12;

		// Token: 0x04000E51 RID: 3665
		public const int MAX_ALARMIN = 16;

		// Token: 0x04000E52 RID: 3666
		public const int MAX_ALARMIN_V30 = 160;

		// Token: 0x04000E53 RID: 3667
		public const int MAX_ALARMOUT = 4;

		// Token: 0x04000E54 RID: 3668
		public const int MAX_ALARMOUT_V30 = 96;

		// Token: 0x04000E55 RID: 3669
		public const int MAX_ANALOG_ALARMIN = 32;

		// Token: 0x04000E56 RID: 3670
		public const int MAX_ANALOG_ALARMOUT = 32;

		// Token: 0x04000E57 RID: 3671
		public const int MAX_ANALOG_CHANNUM = 32;

		// Token: 0x04000E58 RID: 3672
		public const int MAX_ATM_NUM = 1;

		// Token: 0x04000E59 RID: 3673
		public const int MAX_ATM_PROTOCOL_NUM = 1025;

		// Token: 0x04000E5A RID: 3674
		public const int MAX_AUDIO = 1;

		// Token: 0x04000E5B RID: 3675
		public const int MAX_AUDIO_V30 = 2;

		// Token: 0x04000E5C RID: 3676
		public const int MAX_AUXOUT = 4;

		// Token: 0x04000E5D RID: 3677
		public const int MAX_AUXOUT_V30 = 16;

		// Token: 0x04000E5E RID: 3678
		public const int MAX_CALIB_PT = 6;

		// Token: 0x04000E5F RID: 3679
		public const int MAX_CHANNUM = 16;

		// Token: 0x04000E60 RID: 3680
		public const int MAX_CHANNUM_V30 = 64;

		// Token: 0x04000E61 RID: 3681
		public const int MAX_CHINESE_CHAR_NUM = 64;

		// Token: 0x04000E62 RID: 3682
		public const int MAX_CRUISE = 128;

		// Token: 0x04000E63 RID: 3683
		public const int MAX_CRUISE_V30 = 256;

		// Token: 0x04000E64 RID: 3684
		public const int MAX_CYCLE_CHAN = 16;

		// Token: 0x04000E65 RID: 3685
		public const int MAX_CYCLE_CHAN_V30 = 64;

		// Token: 0x04000E66 RID: 3686
		public const int MAX_DAYS = 7;

		// Token: 0x04000E67 RID: 3687
		public const int MAX_DDNS_NUMS = 10;

		// Token: 0x04000E68 RID: 3688
		public const int MAX_DECNUM = 4;

		// Token: 0x04000E69 RID: 3689
		public const int MAX_DECODECHANNUM = 32;

		// Token: 0x04000E6A RID: 3690
		public const int MAX_DECPOOLNUM = 4;

		// Token: 0x04000E6B RID: 3691
		public const int MAX_DIRNAME_LENGTH = 80;

		// Token: 0x04000E6C RID: 3692
		public const int MAX_DISKNUM = 16;

		// Token: 0x04000E6D RID: 3693
		public const int MAX_DISKNUM_V10 = 8;

		// Token: 0x04000E6E RID: 3694
		public const int MAX_DISKNUM_V30 = 33;

		// Token: 0x04000E6F RID: 3695
		public const int MAX_DISPCHANNUM = 24;

		// Token: 0x04000E70 RID: 3696
		public const int MAX_DOMAIN_NAME = 64;

		// Token: 0x04000E71 RID: 3697
		public const int MAX_EMAIL_ADDR_LEN = 48;

		// Token: 0x04000E72 RID: 3698
		public const int MAX_EMAIL_PWD_LEN = 32;

		// Token: 0x04000E73 RID: 3699
		public const int MAX_ETHERNET = 2;

		// Token: 0x04000E74 RID: 3700
		public const int MAX_EXCEPTIONNUM = 16;

		// Token: 0x04000E75 RID: 3701
		public const int MAX_EXCEPTIONNUM_V30 = 32;

		// Token: 0x04000E76 RID: 3702
		public const int MAX_HD_GROUP = 16;

		// Token: 0x04000E77 RID: 3703
		public const int MAX_IP_ALARMIN = 128;

		// Token: 0x04000E78 RID: 3704
		public const int MAX_IP_ALARMOUT = 64;

		// Token: 0x04000E79 RID: 3705
		public const int MAX_IP_CHANNEL = 32;

		// Token: 0x04000E7A RID: 3706
		public const int MAX_IP_DEVICE = 32;

		// Token: 0x04000E7B RID: 3707
		public const int MAX_LICENSE_LEN = 16;

		// Token: 0x04000E7C RID: 3708
		public const int MAX_LINK = 6;

		// Token: 0x04000E7D RID: 3709
		public const int MAX_LOOPPLANNUM = 16;

		// Token: 0x04000E7E RID: 3710
		public const int MAX_MASK_REGION_NUM = 4;

		// Token: 0x04000E7F RID: 3711
		public const int MAX_MATRIXOUT = 16;

		// Token: 0x04000E80 RID: 3712
		public const int MAX_NAMELEN = 16;

		// Token: 0x04000E81 RID: 3713
		public const int MAX_NET_DISK = 16;

		// Token: 0x04000E82 RID: 3714
		public const int MAX_NFS_DISK = 8;

		// Token: 0x04000E83 RID: 3715
		public const int MAX_PLATE_NUM = 3;

		// Token: 0x04000E84 RID: 3716
		public const int MAX_PRESET = 128;

		// Token: 0x04000E85 RID: 3717
		public const int MAX_PRESET_V30 = 256;

		// Token: 0x04000E86 RID: 3718
		public const int MAX_PREVIEW_MODE = 8;

		// Token: 0x04000E87 RID: 3719
		public const int MAX_RIGHT = 32;

		// Token: 0x04000E88 RID: 3720
		public const int MAX_RULE_NUM = 8;

		// Token: 0x04000E89 RID: 3721
		public const int MAX_SEGMENT_NUM = 6;

		// Token: 0x04000E8A RID: 3722
		public const int MAX_SERIAL_NUM = 64;

		// Token: 0x04000E8B RID: 3723
		public const int MAX_SERIAL_PORT = 8;

		// Token: 0x04000E8C RID: 3724
		public const int MAX_SERIALLEN = 36;

		// Token: 0x04000E8D RID: 3725
		public const int MAX_SERIALNUM = 2;

		// Token: 0x04000E8E RID: 3726
		public const int MAX_SHELTERNUM = 4;

		// Token: 0x04000E8F RID: 3727
		public const int MAX_STRINGNUM = 4;

		// Token: 0x04000E90 RID: 3728
		public const int MAX_STRINGNUM_EX = 8;

		// Token: 0x04000E91 RID: 3729
		public const int MAX_STRINGNUM_V30 = 8;

		// Token: 0x04000E92 RID: 3730
		public const int MAX_SUBSYSTEM_NUM = 80;

		// Token: 0x04000E93 RID: 3731
		public const int MAX_TARGET_NUM = 30;

		// Token: 0x04000E94 RID: 3732
		public const int MAX_TIMESEGMENT = 4;

		// Token: 0x04000E95 RID: 3733
		public const int MAX_TIMESEGMENT_2 = 2;

		// Token: 0x04000E96 RID: 3734
		public const int MAX_TIMESEGMENT_V30 = 8;

		// Token: 0x04000E97 RID: 3735
		public const int MAX_TRACK = 128;

		// Token: 0x04000E98 RID: 3736
		public const int MAX_TRACK_V30 = 256;

		// Token: 0x04000E99 RID: 3737
		public const int MAX_TRANSPARENTNUM = 2;

		// Token: 0x04000E9A RID: 3738
		public const int MAX_USERNUM = 16;

		// Token: 0x04000E9B RID: 3739
		public const int MAX_USERNUM_V30 = 32;

		// Token: 0x04000E9C RID: 3740
		public const int MAX_VCA_CHAN = 16;

		// Token: 0x04000E9D RID: 3741
		public const int MAX_VGA = 1;

		// Token: 0x04000E9E RID: 3742
		public const int MAX_VGA_V30 = 4;

		// Token: 0x04000E9F RID: 3743
		public const int MAX_VIDEOOUT = 2;

		// Token: 0x04000EA0 RID: 3744
		public const int MAX_VIDEOOUT_V30 = 4;

		// Token: 0x04000EA1 RID: 3745
		public const int MAX_WINDOW = 16;

		// Token: 0x04000EA2 RID: 3746
		public const int MAX_WINDOW_V30 = 32;

		// Token: 0x04000EA3 RID: 3747
		public const int MAX_WINDOWS = 16;

		// Token: 0x04000EA4 RID: 3748
		public const int MAXPROGRESS = 100;

		// Token: 0x04000EA5 RID: 3749
		public const int MEDIACONNECT = 2;

		// Token: 0x04000EA6 RID: 3750
		public const int MEDVR = 2;

		// Token: 0x04000EA7 RID: 3751
		public const int MEGA_IPCAM = 31;

		// Token: 0x04000EA8 RID: 3752
		public const int MESSAGEVALUE_CREATEFILE = 3;

		// Token: 0x04000EA9 RID: 3753
		public const int MESSAGEVALUE_DELETEFILE = 4;

		// Token: 0x04000EAA RID: 3754
		public const int MESSAGEVALUE_DISKFULL = 1;

		// Token: 0x04000EAB RID: 3755
		public const int MESSAGEVALUE_SWITCHDISK = 2;

		// Token: 0x04000EAC RID: 3756
		public const int MESSAGEVALUE_SWITCHFILE = 5;

		// Token: 0x04000EAD RID: 3757
		public const int MIN_CALIB_PT = 4;

		// Token: 0x04000EAE RID: 3758
		public const int MIN_SEGMENT_NUM = 3;

		// Token: 0x04000EAF RID: 3759
		public const int Mini_Banl = 13;

		// Token: 0x04000EB0 RID: 3760
		public const int MINOR_ALARM_IN = 1;

		// Token: 0x04000EB1 RID: 3761
		public const int MINOR_ALARM_OUT = 2;

		// Token: 0x04000EB2 RID: 3762
		public const int MINOR_DCD_LOST = 37;

		// Token: 0x04000EB3 RID: 3763
		public const int MINOR_FANABNORMAL = 49;

		// Token: 0x04000EB4 RID: 3764
		public const int MINOR_FANRESUME = 50;

		// Token: 0x04000EB5 RID: 3765
		public const int MINOR_HD_ERROR = 36;

		// Token: 0x04000EB6 RID: 3766
		public const int MINOR_HD_FULL = 35;

		// Token: 0x04000EB7 RID: 3767
		public const int MINOR_HDD_INFO = 161;

		// Token: 0x04000EB8 RID: 3768
		public const int MINOR_HIDE_ALARM_START = 5;

		// Token: 0x04000EB9 RID: 3769
		public const int MINOR_HIDE_ALARM_STOP = 6;

		// Token: 0x04000EBA RID: 3770
		public const int MINOR_ILLEGAL_ACCESS = 34;

		// Token: 0x04000EBB RID: 3771
		public const int MINOR_IP_CONFLICT = 38;

		// Token: 0x04000EBC RID: 3772
		public const int MINOR_IPC_ADD = 99;

		// Token: 0x04000EBD RID: 3773
		public const int MINOR_IPC_DEL = 100;

		// Token: 0x04000EBE RID: 3774
		public const int MINOR_IPC_IP_CONFLICT = 43;

		// Token: 0x04000EBF RID: 3775
		public const int MINOR_IPC_NO_LINK = 41;

		// Token: 0x04000EC0 RID: 3776
		public const int MINOR_IPC_SET = 101;

		// Token: 0x04000EC1 RID: 3777
		public const int MINOR_LINK_START = 166;

		// Token: 0x04000EC2 RID: 3778
		public const int MINOR_LINK_STOP = 167;

		// Token: 0x04000EC3 RID: 3779
		public const int MINOR_LOCAL_ADD_NAS = 106;

		// Token: 0x04000EC4 RID: 3780
		public const int MINOR_LOCAL_CFG_PARM = 82;

		// Token: 0x04000EC5 RID: 3781
		public const int MINOR_LOCAL_CFGFILE_INPUT = 94;

		// Token: 0x04000EC6 RID: 3782
		public const int MINOR_LOCAL_CFGFILE_OUTPUT = 93;

		// Token: 0x04000EC7 RID: 3783
		public const int MINOR_LOCAL_COPYFILE = 95;

		// Token: 0x04000EC8 RID: 3784
		public const int MINOR_LOCAL_COPYFILE_END_TIME = 105;

		// Token: 0x04000EC9 RID: 3785
		public const int MINOR_LOCAL_COPYFILE_START_TIME = 104;

		// Token: 0x04000ECA RID: 3786
		public const int MINOR_LOCAL_DEL_NAS = 107;

		// Token: 0x04000ECB RID: 3787
		public const int MINOR_LOCAL_DVR_ALARM = 98;

		// Token: 0x04000ECC RID: 3788
		public const int MINOR_LOCAL_FORMAT_HDD = 92;

		// Token: 0x04000ECD RID: 3789
		public const int MINOR_LOCAL_LOCKFILE = 96;

		// Token: 0x04000ECE RID: 3790
		public const int MINOR_LOCAL_LOGIN = 80;

		// Token: 0x04000ECF RID: 3791
		public const int MINOR_LOCAL_LOGOUT = 81;

		// Token: 0x04000ED0 RID: 3792
		public const int MINOR_LOCAL_MODIFY_TIME = 89;

		// Token: 0x04000ED1 RID: 3793
		public const int MINOR_LOCAL_PLAYBYFILE = 83;

		// Token: 0x04000ED2 RID: 3794
		public const int MINOR_LOCAL_PLAYBYTIME = 84;

		// Token: 0x04000ED3 RID: 3795
		public const int MINOR_LOCAL_PREVIEW = 88;

		// Token: 0x04000ED4 RID: 3796
		public const int MINOR_LOCAL_PTZCTRL = 87;

		// Token: 0x04000ED5 RID: 3797
		public const int MINOR_LOCAL_RECFILE_OUTPUT = 91;

		// Token: 0x04000ED6 RID: 3798
		public const int MINOR_LOCAL_SET_NAS = 108;

		// Token: 0x04000ED7 RID: 3799
		public const int MINOR_LOCAL_START_BACKUP = 102;

		// Token: 0x04000ED8 RID: 3800
		public const int MINOR_LOCAL_START_REC = 85;

		// Token: 0x04000ED9 RID: 3801
		public const int MINOR_LOCAL_STOP_BACKUP = 103;

		// Token: 0x04000EDA RID: 3802
		public const int MINOR_LOCAL_STOP_REC = 86;

		// Token: 0x04000EDB RID: 3803
		public const int MINOR_LOCAL_UNLOCKFILE = 97;

		// Token: 0x04000EDC RID: 3804
		public const int MINOR_LOCAL_UPGRADE = 90;

		// Token: 0x04000EDD RID: 3805
		public const int MINOR_LOGOFF_CODESPITTER = 170;

		// Token: 0x04000EDE RID: 3806
		public const int MINOR_LOGON_CODESPITTER = 169;

		// Token: 0x04000EDF RID: 3807
		public const int MINOR_MATRIX_STARTBUZZER = 52;

		// Token: 0x04000EE0 RID: 3808
		public const int MINOR_MATRIX_STARTTRANSFERAUDIO = 167;

		// Token: 0x04000EE1 RID: 3809
		public const int MINOR_MATRIX_STARTTRANSFERVIDEO = 161;

		// Token: 0x04000EE2 RID: 3810
		public const int MINOR_MATRIX_STOPRANSFERAUDIO = 168;

		// Token: 0x04000EE3 RID: 3811
		public const int MINOR_MATRIX_STOPTRANSFERVIDEO = 162;

		// Token: 0x04000EE4 RID: 3812
		public const int MINOR_MOTDET_START = 3;

		// Token: 0x04000EE5 RID: 3813
		public const int MINOR_MOTDET_STOP = 4;

		// Token: 0x04000EE6 RID: 3814
		public const int MINOR_NET_BROKEN = 39;

		// Token: 0x04000EE7 RID: 3815
		public const int MINOR_NET_DISK_INFO = 168;

		// Token: 0x04000EE8 RID: 3816
		public const int MINOR_REBOOT_DVR = 68;

		// Token: 0x04000EE9 RID: 3817
		public const int MINOR_REBOOT_VCA_LIB = 141;

		// Token: 0x04000EEA RID: 3818
		public const int MINOR_REC_ERROR = 40;

		// Token: 0x04000EEB RID: 3819
		public const int MINOR_REC_OVERDUE = 165;

		// Token: 0x04000EEC RID: 3820
		public const int MINOR_REC_START = 163;

		// Token: 0x04000EED RID: 3821
		public const int MINOR_REC_STOP = 164;

		// Token: 0x04000EEE RID: 3822
		public const int MINOR_REMOTE_ADD_NAS = 142;

		// Token: 0x04000EEF RID: 3823
		public const int MINOR_REMOTE_ARM = 121;

		// Token: 0x04000EF0 RID: 3824
		public const int MINOR_REMOTE_CFG_PARM = 119;

		// Token: 0x04000EF1 RID: 3825
		public const int MINOR_REMOTE_CFGFILE_INTPUT = 135;

		// Token: 0x04000EF2 RID: 3826
		public const int MINOR_REMOTE_CFGFILE_OUTPUT = 134;

		// Token: 0x04000EF3 RID: 3827
		public const int MINOR_REMOTE_DEL_NAS = 143;

		// Token: 0x04000EF4 RID: 3828
		public const int MINOR_REMOTE_DISARM = 122;

		// Token: 0x04000EF5 RID: 3829
		public const int MINOR_REMOTE_DVR_ALARM = 137;

		// Token: 0x04000EF6 RID: 3830
		public const int MINOR_REMOTE_FORMAT_HDD = 130;

		// Token: 0x04000EF7 RID: 3831
		public const int MINOR_REMOTE_GET_ALLSUBSYSTEM = 164;

		// Token: 0x04000EF8 RID: 3832
		public const int MINOR_REMOTE_GET_PARM = 118;

		// Token: 0x04000EF9 RID: 3833
		public const int MINOR_REMOTE_GET_PLANARRAY = 166;

		// Token: 0x04000EFA RID: 3834
		public const int MINOR_REMOTE_GET_STATUS = 120;

		// Token: 0x04000EFB RID: 3835
		public const int MINOR_REMOTE_IPC_ADD = 138;

		// Token: 0x04000EFC RID: 3836
		public const int MINOR_REMOTE_IPC_DEL = 139;

		// Token: 0x04000EFD RID: 3837
		public const int MINOR_REMOTE_IPC_SET = 140;

		// Token: 0x04000EFE RID: 3838
		public const int MINOR_REMOTE_LOCKFILE = 132;

		// Token: 0x04000EFF RID: 3839
		public const int MINOR_REMOTE_LOGIN = 112;

		// Token: 0x04000F00 RID: 3840
		public const int MINOR_REMOTE_LOGOUT = 113;

		// Token: 0x04000F01 RID: 3841
		public const int MINOR_REMOTE_PLAYBYFILE = 127;

		// Token: 0x04000F02 RID: 3842
		public const int MINOR_REMOTE_PLAYBYTIME = 128;

		// Token: 0x04000F03 RID: 3843
		public const int MINOR_REMOTE_PTZCTRL = 129;

		// Token: 0x04000F04 RID: 3844
		public const int MINOR_REMOTE_REBOOT = 123;

		// Token: 0x04000F05 RID: 3845
		public const int MINOR_REMOTE_RECFILE_OUTPUT = 136;

		// Token: 0x04000F06 RID: 3846
		public const int MINOR_REMOTE_SET_ALLSUBSYSTEM = 163;

		// Token: 0x04000F07 RID: 3847
		public const int MINOR_REMOTE_SET_NAS = 144;

		// Token: 0x04000F08 RID: 3848
		public const int MINOR_REMOTE_SET_PLANARRAY = 165;

		// Token: 0x04000F09 RID: 3849
		public const int MINOR_REMOTE_START_REC = 114;

		// Token: 0x04000F0A RID: 3850
		public const int MINOR_REMOTE_STOP = 131;

		// Token: 0x04000F0B RID: 3851
		public const int MINOR_REMOTE_STOP_REC = 115;

		// Token: 0x04000F0C RID: 3852
		public const int MINOR_REMOTE_UNLOCKFILE = 133;

		// Token: 0x04000F0D RID: 3853
		public const int MINOR_REMOTE_UPGRADE = 126;

		// Token: 0x04000F0E RID: 3854
		public const int MINOR_SMART_INFO = 162;

		// Token: 0x04000F0F RID: 3855
		public const int MINOR_START_DVR = 65;

		// Token: 0x04000F10 RID: 3856
		public const int MINOR_START_TRANS_CHAN = 116;

		// Token: 0x04000F11 RID: 3857
		public const int MINOR_START_VT = 124;

		// Token: 0x04000F12 RID: 3858
		public const int MINOR_STOP_ABNORMAL = 67;

		// Token: 0x04000F13 RID: 3859
		public const int MINOR_STOP_DVR = 66;

		// Token: 0x04000F14 RID: 3860
		public const int MINOR_STOP_TRANS_CHAN = 117;

		// Token: 0x04000F15 RID: 3861
		public const int MINOR_STOP_VT = 125;

		// Token: 0x04000F16 RID: 3862
		public const int MINOR_SUBSYSTEM_ABNORMALREBOOT = 51;

		// Token: 0x04000F17 RID: 3863
		public const int MINOR_SUBSYSTEMREBOOT = 160;

		// Token: 0x04000F18 RID: 3864
		public const int MINOR_VCA_ALARM_START = 7;

		// Token: 0x04000F19 RID: 3865
		public const int MINOR_VCA_ALARM_STOP = 8;

		// Token: 0x04000F1A RID: 3866
		public const int MINOR_VI_EXCEPTION = 42;

		// Token: 0x04000F1B RID: 3867
		public const int MINOR_VI_LOST = 33;

		// Token: 0x04000F1C RID: 3868
		public const int MODIFYTYPE = 1;

		// Token: 0x04000F1D RID: 3869
		public const int NAME_LEN = 32;

		// Token: 0x04000F1E RID: 3870
		public const int NanTian = 17;

		// Token: 0x04000F1F RID: 3871
		public const int NCR = 0;

		// Token: 0x04000F20 RID: 3872
		public const int NCR_2 = 23;

		// Token: 0x04000F21 RID: 3873
		public const int NET_DEC_CONTINUECYCLE = 4;

		// Token: 0x04000F22 RID: 3874
		public const int NET_DEC_STARTDEC = 1;

		// Token: 0x04000F23 RID: 3875
		public const int NET_DEC_STOPCYCLE = 3;

		// Token: 0x04000F24 RID: 3876
		public const int NET_DEC_STOPDEC = 2;

		// Token: 0x04000F25 RID: 3877
		public const int NET_DVR_ALLOC_RESOURCE_ERROR = 41;

		// Token: 0x04000F26 RID: 3878
		public const int NET_DVR_AUDIO_MODE_ERROR = 42;

		// Token: 0x04000F27 RID: 3879
		public const int NET_DVR_AUDIOSTREAMDATA = 3;

		// Token: 0x04000F28 RID: 3880
		public const int NET_DVR_BINDSOCKET_ERROR = 72;

		// Token: 0x04000F29 RID: 3881
		public const int NET_DVR_BUSY = 24;

		// Token: 0x04000F2A RID: 3882
		public const int NET_DVR_CARDHAVEINIT = 50;

		// Token: 0x04000F2B RID: 3883
		public const int NET_DVR_CHAN_EXCEPTION = 18;

		// Token: 0x04000F2C RID: 3884
		public const int NET_DVR_CHANNEL_ERROR = 4;

		// Token: 0x04000F2D RID: 3885
		public const int NET_DVR_COMMANDTIMEOUT = 14;

		// Token: 0x04000F2E RID: 3886
		public const int NET_DVR_CONVERT_SDK_ERROR = 85;

		// Token: 0x04000F2F RID: 3887
		public const int NET_DVR_CREATEDIR_ERROR = 71;

		// Token: 0x04000F30 RID: 3888
		public const int NET_DVR_CREATEFILE_ERROR = 34;

		// Token: 0x04000F31 RID: 3889
		public const int NET_DVR_CREATESOCKET_ERROR = 44;

		// Token: 0x04000F32 RID: 3890
		public const int NET_DVR_DEVICETYPE_ERROR = 80;

		// Token: 0x04000F33 RID: 3891
		public const int NET_DVR_DIR_ERROR = 40;

		// Token: 0x04000F34 RID: 3892
		public const int NET_DVR_DISK_ERROR = 22;

		// Token: 0x04000F35 RID: 3893
		public const int NET_DVR_DISK_FORMATING = 27;

		// Token: 0x04000F36 RID: 3894
		public const int NET_DVR_DISK_FULL = 21;

		// Token: 0x04000F37 RID: 3895
		public const int NET_DVR_DSSDK_ERROR = 68;

		// Token: 0x04000F38 RID: 3896
		public const int NET_DVR_DVRNORESOURCE = 28;

		// Token: 0x04000F39 RID: 3897
		public const int NET_DVR_DVROPRATEFAILED = 29;

		// Token: 0x04000F3A RID: 3898
		public const int NET_DVR_DVRVOICEOPENED = 31;

		// Token: 0x04000F3B RID: 3899
		public const int NET_DVR_ENCODER_H264 = 1;

		// Token: 0x04000F3C RID: 3900
		public const int NET_DVR_ENCODER_MPEG4 = 3;

		// Token: 0x04000F3D RID: 3901
		public const int NET_DVR_ENCODER_S264 = 2;

		// Token: 0x04000F3E RID: 3902
		public const int NET_DVR_ENCODER_UNKOWN = 0;

		// Token: 0x04000F3F RID: 3903
		public const int NET_DVR_ERRORALARMPORT = 16;

		// Token: 0x04000F40 RID: 3904
		public const int NET_DVR_ERRORDISKNUM = 20;

		// Token: 0x04000F41 RID: 3905
		public const int NET_DVR_ERRORSERIALPORT = 15;

		// Token: 0x04000F42 RID: 3906
		public const int NET_DVR_FILE_EXCEPTION = 1004;

		// Token: 0x04000F43 RID: 3907
		public const int NET_DVR_FILE_NOFIND = 1001;

		// Token: 0x04000F44 RID: 3908
		public const int NET_DVR_FILE_SUCCESS = 1000;

		// Token: 0x04000F45 RID: 3909
		public const int NET_DVR_FILEFORMAT_ERROR = 39;

		// Token: 0x04000F46 RID: 3910
		public const int NET_DVR_FILEOPENFAIL = 35;

		// Token: 0x04000F47 RID: 3911
		public const int NET_DVR_FORMAT_READONLY = 78;

		// Token: 0x04000F48 RID: 3912
		public const int NET_DVR_GET_ALARMINCFG = 114;

		// Token: 0x04000F49 RID: 3913
		public const int NET_DVR_GET_ALARMINCFG_V30 = 1024;

		// Token: 0x04000F4A RID: 3914
		public const int NET_DVR_GET_ALARMOUTCFG = 116;

		// Token: 0x04000F4B RID: 3915
		public const int NET_DVR_GET_ALARMOUTCFG_V30 = 1026;

		// Token: 0x04000F4C RID: 3916
		public const int NET_DVR_GET_AP_INFO_LIST = 305;

		// Token: 0x04000F4D RID: 3917
		public const int NET_DVR_GET_AUXOUTCFG = 140;

		// Token: 0x04000F4E RID: 3918
		public const int NET_DVR_GET_CCDPARAMCFG = 1067;

		// Token: 0x04000F4F RID: 3919
		public const int NET_DVR_GET_COMPRESSCFG = 106;

		// Token: 0x04000F50 RID: 3920
		public const int NET_DVR_GET_COMPRESSCFG_AUD = 1058;

		// Token: 0x04000F51 RID: 3921
		public const int NET_DVR_GET_COMPRESSCFG_EX = 204;

		// Token: 0x04000F52 RID: 3922
		public const int NET_DVR_GET_COMPRESSCFG_V30 = 1040;

		// Token: 0x04000F53 RID: 3923
		public const int NET_DVR_GET_CRUISE = 1020;

		// Token: 0x04000F54 RID: 3924
		public const int NET_DVR_GET_DDNSCFG = 226;

		// Token: 0x04000F55 RID: 3925
		public const int NET_DVR_GET_DDNSCFG_EX = 274;

		// Token: 0x04000F56 RID: 3926
		public const int NET_DVR_GET_DDNSCFG_V30 = 1010;

		// Token: 0x04000F57 RID: 3927
		public const int NET_DVR_GET_DECODERCFG = 110;

		// Token: 0x04000F58 RID: 3928
		public const int NET_DVR_GET_DECODERCFG_V30 = 1042;

		// Token: 0x04000F59 RID: 3929
		public const int NET_DVR_GET_DEVICECFG = 100;

		// Token: 0x04000F5A RID: 3930
		public const int NET_DVR_GET_EMAILCFG = 228;

		// Token: 0x04000F5B RID: 3931
		public const int NET_DVR_GET_EMAILCFG_V30 = 1012;

		// Token: 0x04000F5C RID: 3932
		public const int NET_DVR_GET_EMAILPARACFG = 250;

		// Token: 0x04000F5D RID: 3933
		public const int NET_DVR_GET_EVENTCOMPCFG = 132;

		// Token: 0x04000F5E RID: 3934
		public const int NET_DVR_GET_EXCEPTIONCFG = 126;

		// Token: 0x04000F5F RID: 3935
		public const int NET_DVR_GET_EXCEPTIONCFG_V30 = 1034;

		// Token: 0x04000F60 RID: 3936
		public const int NET_DVR_GET_HDCFG = 1054;

		// Token: 0x04000F61 RID: 3937
		public const int NET_DVR_GET_HDGROUP_CFG = 1056;

		// Token: 0x04000F62 RID: 3938
		public const int NET_DVR_GET_IMAGEPARAM = 1064;

		// Token: 0x04000F63 RID: 3939
		public const int NET_DVR_GET_IMAGEREGION = 1062;

		// Token: 0x04000F64 RID: 3940
		public const int NET_DVR_GET_IPALARMINCFG = 1050;

		// Token: 0x04000F65 RID: 3941
		public const int NET_DVR_GET_IPALARMOUTCFG = 1052;

		// Token: 0x04000F66 RID: 3942
		public const int NET_DVR_GET_IPPARACFG = 1048;

		// Token: 0x04000F67 RID: 3943
		public const int NET_DVR_GET_IPPARACFG_V31 = 1060;

		// Token: 0x04000F68 RID: 3944
		public const int NET_DVR_GET_IVMS_BEHAVIORCFG = 177;

		// Token: 0x04000F69 RID: 3945
		public const int NET_DVR_GET_IVMS_ENTER_REGION = 175;

		// Token: 0x04000F6A RID: 3946
		public const int NET_DVR_GET_IVMS_MASK_REGION = 173;

		// Token: 0x04000F6B RID: 3947
		public const int NET_DVR_GET_IVMS_STREAMCFG = 163;

		// Token: 0x04000F6C RID: 3948
		public const int NET_DVR_GET_LF_CFG = 161;

		// Token: 0x04000F6D RID: 3949
		public const int NET_DVR_GET_NET_DISKCFG = 1038;

		// Token: 0x04000F6E RID: 3950
		public const int NET_DVR_GET_NETAPPCFG = 222;

		// Token: 0x04000F6F RID: 3951
		public const int NET_DVR_GET_NETCFG = 102;

		// Token: 0x04000F70 RID: 3952
		public const int NET_DVR_GET_NETCFG_OTHER = 244;

		// Token: 0x04000F71 RID: 3953
		public const int NET_DVR_GET_NETCFG_V30 = 1000;

		// Token: 0x04000F72 RID: 3954
		public const int NET_DVR_GET_NFSCFG = 230;

		// Token: 0x04000F73 RID: 3955
		public const int NET_DVR_GET_NTPCFG = 224;

		// Token: 0x04000F74 RID: 3956
		public const int NET_DVR_GET_PICCFG = 104;

		// Token: 0x04000F75 RID: 3957
		public const int NET_DVR_GET_PICCFG_EX = 200;

		// Token: 0x04000F76 RID: 3958
		public const int NET_DVR_GET_PICCFG_V30 = 1002;

		// Token: 0x04000F77 RID: 3959
		public const int NET_DVR_GET_PLATECFG = 151;

		// Token: 0x04000F78 RID: 3960
		public const int NET_DVR_GET_PREVIEWCFG = 120;

		// Token: 0x04000F79 RID: 3961
		public const int NET_DVR_GET_PREVIEWCFG_AUX = 142;

		// Token: 0x04000F7A RID: 3962
		public const int NET_DVR_GET_PREVIEWCFG_AUX_V30 = 1046;

		// Token: 0x04000F7B RID: 3963
		public const int NET_DVR_GET_PREVIEWCFG_V30 = 1044;

		// Token: 0x04000F7C RID: 3964
		public const int NET_DVR_GET_PTZPOS = 293;

		// Token: 0x04000F7D RID: 3965
		public const int NET_DVR_GET_PTZSCOPE = 294;

		// Token: 0x04000F7E RID: 3966
		public const int NET_DVR_GET_RECORDCFG = 108;

		// Token: 0x04000F7F RID: 3967
		public const int NET_DVR_GET_RECORDCFG_V30 = 1004;

		// Token: 0x04000F80 RID: 3968
		public const int NET_DVR_GET_RS232CFG = 112;

		// Token: 0x04000F81 RID: 3969
		public const int NET_DVR_GET_RS232CFG_V30 = 1036;

		// Token: 0x04000F82 RID: 3970
		public const int NET_DVR_GET_RULECFG = 153;

		// Token: 0x04000F83 RID: 3971
		public const int NET_DVR_GET_SHOWSTRING = 130;

		// Token: 0x04000F84 RID: 3972
		public const int NET_DVR_GET_SHOWSTRING_EX = 238;

		// Token: 0x04000F85 RID: 3973
		public const int NET_DVR_GET_SHOWSTRING_V30 = 1030;

		// Token: 0x04000F86 RID: 3974
		public const int NET_DVR_GET_TIMECFG = 118;

		// Token: 0x04000F87 RID: 3975
		public const int NET_DVR_GET_USERCFG = 124;

		// Token: 0x04000F88 RID: 3976
		public const int NET_DVR_GET_USERCFG_EX = 202;

		// Token: 0x04000F89 RID: 3977
		public const int NET_DVR_GET_USERCFG_V30 = 1006;

		// Token: 0x04000F8A RID: 3978
		public const int NET_DVR_GET_VCA_CTRLCFG = 165;

		// Token: 0x04000F8B RID: 3979
		public const int NET_DVR_GET_VCA_ENTER_REGION = 169;

		// Token: 0x04000F8C RID: 3980
		public const int NET_DVR_GET_VCA_LINE_SEGMENT = 171;

		// Token: 0x04000F8D RID: 3981
		public const int NET_DVR_GET_VCA_MASK_REGION = 167;

		// Token: 0x04000F8E RID: 3982
		public const int NET_DVR_GET_VIDEOOUTCFG = 122;

		// Token: 0x04000F8F RID: 3983
		public const int NET_DVR_GET_VIDEOOUTCFG_V30 = 1028;

		// Token: 0x04000F90 RID: 3984
		public const int NET_DVR_GET_WIFI_CFG = 307;

		// Token: 0x04000F91 RID: 3985
		public const int NET_DVR_GET_WIFI_WORKMODE = 309;

		// Token: 0x04000F92 RID: 3986
		public const int NET_DVR_GET_ZONEANDDST = 128;

		// Token: 0x04000F93 RID: 3987
		public const int NET_DVR_GETLOCALIPANDMACFAIL = 53;

		// Token: 0x04000F94 RID: 3988
		public const int NET_DVR_GETPLAYTIMEFAIL = 37;

		// Token: 0x04000F95 RID: 3989
		public const int NET_DVR_GETTOTALFRAMES = 16;

		// Token: 0x04000F96 RID: 3990
		public const int NET_DVR_GETTOTALTIME = 17;

		// Token: 0x04000F97 RID: 3991
		public const int NET_DVR_HIDELOGO = 2;

		// Token: 0x04000F98 RID: 3992
		public const int NET_DVR_IPC_COUNT_OVERFLOW = 86;

		// Token: 0x04000F99 RID: 3993
		public const int NET_DVR_IPCHAN_NOTALIVE = 83;

		// Token: 0x04000F9A RID: 3994
		public const int NET_DVR_IPMISMATCH = 55;

		// Token: 0x04000F9B RID: 3995
		public const int NET_DVR_ISFINDING = 1002;

		// Token: 0x04000F9C RID: 3996
		public const int NET_DVR_IVMS_GET_SEARCHCFG = 179;

		// Token: 0x04000F9D RID: 3997
		public const int NET_DVR_IVMS_SET_SEARCHCFG = 178;

		// Token: 0x04000F9E RID: 3998
		public const int NET_DVR_JOINMULTICASTFAILED = 70;

		// Token: 0x04000F9F RID: 3999
		public const int NET_DVR_KEEPALIVE = 25;

		// Token: 0x04000FA0 RID: 4000
		public const int NET_DVR_LANGUAGE_ERROR = 81;

		// Token: 0x04000FA1 RID: 4001
		public const int NET_DVR_LOADDSSDKFAILED = 66;

		// Token: 0x04000FA2 RID: 4002
		public const int NET_DVR_LOADDSSDKPROC_ERROR = 67;

		// Token: 0x04000FA3 RID: 4003
		public const int NET_DVR_LOADPLAYERSDKFAILED = 64;

		// Token: 0x04000FA4 RID: 4004
		public const int NET_DVR_LOADPLAYERSDKPROC_ERROR = 65;

		// Token: 0x04000FA5 RID: 4005
		public const int NET_DVR_MACMISMATCH = 56;

		// Token: 0x04000FA6 RID: 4006
		public const int NET_DVR_MAX_DISPREGION = 16;

		// Token: 0x04000FA7 RID: 4007
		public const int NET_DVR_MAX_NUM = 46;

		// Token: 0x04000FA8 RID: 4008
		public const int NET_DVR_MAX_PLAYERPORT = 58;

		// Token: 0x04000FA9 RID: 4009
		public const int NET_DVR_MAX_USERNUM = 52;

		// Token: 0x04000FAA RID: 4010
		public const int NET_DVR_MODIFY_FAIL = 25;

		// Token: 0x04000FAB RID: 4011
		public const int NET_DVR_NETWORK_ERRORDATA = 11;

		// Token: 0x04000FAC RID: 4012
		public const int NET_DVR_NETWORK_FAIL_CONNECT = 7;

		// Token: 0x04000FAD RID: 4013
		public const int NET_DVR_NETWORK_RECV_ERROR = 9;

		// Token: 0x04000FAE RID: 4014
		public const int NET_DVR_NETWORK_RECV_TIMEOUT = 10;

		// Token: 0x04000FAF RID: 4015
		public const int NET_DVR_NETWORK_SEND_ERROR = 8;

		// Token: 0x04000FB0 RID: 4016
		public const int NET_DVR_NODEVICEBACKUP = 60;

		// Token: 0x04000FB1 RID: 4017
		public const int NET_DVR_NODISK = 19;

		// Token: 0x04000FB2 RID: 4018
		public const int NET_DVR_NOENCODEING = 54;

		// Token: 0x04000FB3 RID: 4019
		public const int NET_DVR_NOENOUGH_BUF = 43;

		// Token: 0x04000FB4 RID: 4020
		public const int NET_DVR_NOENOUGHPRI = 2;

		// Token: 0x04000FB5 RID: 4021
		public const int NET_DVR_NOERROR = 0;

		// Token: 0x04000FB6 RID: 4022
		public const int NET_DVR_NOINIT = 3;

		// Token: 0x04000FB7 RID: 4023
		public const int NET_DVR_NOMOREFILE = 1003;

		// Token: 0x04000FB8 RID: 4024
		public const int NET_DVR_NOSPACEBACKUP = 59;

		// Token: 0x04000FB9 RID: 4025
		public const int NET_DVR_NOSPECFILE = 33;

		// Token: 0x04000FBA RID: 4026
		public const int NET_DVR_NOSUPPORT = 23;

		// Token: 0x04000FBB RID: 4027
		public const int NET_DVR_OPENHOSTSOUND_FAIL = 30;

		// Token: 0x04000FBC RID: 4028
		public const int NET_DVR_OPERNOPERMIT = 13;

		// Token: 0x04000FBD RID: 4029
		public const int NET_DVR_OPERNOTFINISH = 36;

		// Token: 0x04000FBE RID: 4030
		public const int NET_DVR_ORDER_ERROR = 12;

		// Token: 0x04000FBF RID: 4031
		public const int NET_DVR_OVER_MAXLINK = 5;

		// Token: 0x04000FC0 RID: 4032
		public const int NET_DVR_PARAMETER_ERROR = 17;

		// Token: 0x04000FC1 RID: 4033
		public const int NET_DVR_PARAVERSION_ERROR = 82;

		// Token: 0x04000FC2 RID: 4034
		public const int NET_DVR_PASSWORD_ERROR = 1;

		// Token: 0x04000FC3 RID: 4035
		public const int NET_DVR_PASSWORD_FORMAT_ERROR = 26;

		// Token: 0x04000FC4 RID: 4036
		public const int NET_DVR_PICTURE_BITS_ERROR = 61;

		// Token: 0x04000FC5 RID: 4037
		public const int NET_DVR_PICTURE_DIMENSION_ERROR = 62;

		// Token: 0x04000FC6 RID: 4038
		public const int NET_DVR_PICTURE_SIZ_ERROR = 63;

		// Token: 0x04000FC7 RID: 4039
		public const int NET_DVR_PLAYAUDIOVOLUME = 11;

		// Token: 0x04000FC8 RID: 4040
		public const int NET_DVR_PLAYBACK5SNODATA = 104;

		// Token: 0x04000FC9 RID: 4041
		public const int NET_DVR_PLAYBACKEXCEPTION = 102;

		// Token: 0x04000FCA RID: 4042
		public const int NET_DVR_PLAYBACKNETCLOSE = 103;

		// Token: 0x04000FCB RID: 4043
		public const int NET_DVR_PLAYBACKOVER = 101;

		// Token: 0x04000FCC RID: 4044
		public const int NET_DVR_PLAYERFAILED = 51;

		// Token: 0x04000FCD RID: 4045
		public const int NET_DVR_PLAYFAIL = 38;

		// Token: 0x04000FCE RID: 4046
		public const int NET_DVR_PLAYFAST = 5;

		// Token: 0x04000FCF RID: 4047
		public const int NET_DVR_PLAYFRAME = 8;

		// Token: 0x04000FD0 RID: 4048
		public const int NET_DVR_PLAYGETFRAME = 15;

		// Token: 0x04000FD1 RID: 4049
		public const int NET_DVR_PLAYGETPOS = 13;

		// Token: 0x04000FD2 RID: 4050
		public const int NET_DVR_PLAYGETTIME = 14;

		// Token: 0x04000FD3 RID: 4051
		public const int NET_DVR_PLAYNORMAL = 7;

		// Token: 0x04000FD4 RID: 4052
		public const int NET_DVR_PLAYPAUSE = 3;

		// Token: 0x04000FD5 RID: 4053
		public const int NET_DVR_PLAYRESTART = 4;

		// Token: 0x04000FD6 RID: 4054
		public const int NET_DVR_PLAYSETPOS = 12;

		// Token: 0x04000FD7 RID: 4055
		public const int NET_DVR_PLAYSLOW = 6;

		// Token: 0x04000FD8 RID: 4056
		public const int NET_DVR_PLAYSTART = 1;

		// Token: 0x04000FD9 RID: 4057
		public const int NET_DVR_PLAYSTARTAUDIO = 9;

		// Token: 0x04000FDA RID: 4058
		public const int NET_DVR_PLAYSTOP = 2;

		// Token: 0x04000FDB RID: 4059
		public const int NET_DVR_PLAYSTOPAUDIO = 10;

		// Token: 0x04000FDC RID: 4060
		public const int NET_DVR_PROGRAM_EXCEPTION = 76;

		// Token: 0x04000FDD RID: 4061
		public const int NET_DVR_REALPLAY5SNODATA = 113;

		// Token: 0x04000FDE RID: 4062
		public const int NET_DVR_REALPLAYEXCEPTION = 111;

		// Token: 0x04000FDF RID: 4063
		public const int NET_DVR_REALPLAYNETCLOSE = 112;

		// Token: 0x04000FE0 RID: 4064
		public const int NET_DVR_REALPLAYRECONNECT = 114;

		// Token: 0x04000FE1 RID: 4065
		public const int NET_DVR_RTSP_SDK_ERROR = 84;

		// Token: 0x04000FE2 RID: 4066
		public const int NET_DVR_SET_ALARMINCFG = 115;

		// Token: 0x04000FE3 RID: 4067
		public const int NET_DVR_SET_ALARMINCFG_V30 = 1025;

		// Token: 0x04000FE4 RID: 4068
		public const int NET_DVR_SET_ALARMOUTCFG = 117;

		// Token: 0x04000FE5 RID: 4069
		public const int NET_DVR_SET_ALARMOUTCFG_V30 = 1027;

		// Token: 0x04000FE6 RID: 4070
		public const int NET_DVR_SET_AUXOUTCFG = 141;

		// Token: 0x04000FE7 RID: 4071
		public const int NET_DVR_SET_CCDPARAMCFG = 1068;

		// Token: 0x04000FE8 RID: 4072
		public const int NET_DVR_SET_COMPRESSCFG = 107;

		// Token: 0x04000FE9 RID: 4073
		public const int NET_DVR_SET_COMPRESSCFG_AUD = 1059;

		// Token: 0x04000FEA RID: 4074
		public const int NET_DVR_SET_COMPRESSCFG_EX = 205;

		// Token: 0x04000FEB RID: 4075
		public const int NET_DVR_SET_COMPRESSCFG_V30 = 1041;

		// Token: 0x04000FEC RID: 4076
		public const int NET_DVR_SET_CRUISE = 1021;

		// Token: 0x04000FED RID: 4077
		public const int NET_DVR_SET_DDNSCFG = 227;

		// Token: 0x04000FEE RID: 4078
		public const int NET_DVR_SET_DDNSCFG_EX = 275;

		// Token: 0x04000FEF RID: 4079
		public const int NET_DVR_SET_DDNSCFG_V30 = 1011;

		// Token: 0x04000FF0 RID: 4080
		public const int NET_DVR_SET_DECODERCFG = 111;

		// Token: 0x04000FF1 RID: 4081
		public const int NET_DVR_SET_DECODERCFG_V30 = 1043;

		// Token: 0x04000FF2 RID: 4082
		public const int NET_DVR_SET_DEVICECFG = 101;

		// Token: 0x04000FF3 RID: 4083
		public const int NET_DVR_SET_EMAILCFG = 229;

		// Token: 0x04000FF4 RID: 4084
		public const int NET_DVR_SET_EMAILCFG_V30 = 1013;

		// Token: 0x04000FF5 RID: 4085
		public const int NET_DVR_SET_EMAILPARACFG = 251;

		// Token: 0x04000FF6 RID: 4086
		public const int NET_DVR_SET_EVENTCOMPCFG = 133;

		// Token: 0x04000FF7 RID: 4087
		public const int NET_DVR_SET_EXCEPTIONCFG = 127;

		// Token: 0x04000FF8 RID: 4088
		public const int NET_DVR_SET_EXCEPTIONCFG_V30 = 1035;

		// Token: 0x04000FF9 RID: 4089
		public const int NET_DVR_SET_HDCFG = 1055;

		// Token: 0x04000FFA RID: 4090
		public const int NET_DVR_SET_HDGROUP_CFG = 1057;

		// Token: 0x04000FFB RID: 4091
		public const int NET_DVR_SET_IMAGEPARAM = 1065;

		// Token: 0x04000FFC RID: 4092
		public const int NET_DVR_SET_IMAGEREGION = 1063;

		// Token: 0x04000FFD RID: 4093
		public const int NET_DVR_SET_IPALARMINCFG = 1051;

		// Token: 0x04000FFE RID: 4094
		public const int NET_DVR_SET_IPALARMOUTCFG = 1053;

		// Token: 0x04000FFF RID: 4095
		public const int NET_DVR_SET_IPPARACFG = 1049;

		// Token: 0x04001000 RID: 4096
		public const int NET_DVR_SET_IPPARACFG_V31 = 1061;

		// Token: 0x04001001 RID: 4097
		public const int NET_DVR_SET_IVMS_BEHAVIORCFG = 176;

		// Token: 0x04001002 RID: 4098
		public const int NET_DVR_SET_IVMS_ENTER_REGION = 174;

		// Token: 0x04001003 RID: 4099
		public const int NET_DVR_SET_IVMS_MASK_REGION = 172;

		// Token: 0x04001004 RID: 4100
		public const int NET_DVR_SET_IVMS_STREAMCFG = 162;

		// Token: 0x04001005 RID: 4101
		public const int NET_DVR_SET_LF_CFG = 160;

		// Token: 0x04001006 RID: 4102
		public const int NET_DVR_SET_NET_DISKCFG = 1039;

		// Token: 0x04001007 RID: 4103
		public const int NET_DVR_SET_NETAPPCFG = 223;

		// Token: 0x04001008 RID: 4104
		public const int NET_DVR_SET_NETCFG = 103;

		// Token: 0x04001009 RID: 4105
		public const int NET_DVR_SET_NETCFG_OTHER = 245;

		// Token: 0x0400100A RID: 4106
		public const int NET_DVR_SET_NETCFG_V30 = 1001;

		// Token: 0x0400100B RID: 4107
		public const int NET_DVR_SET_NFSCFG = 231;

		// Token: 0x0400100C RID: 4108
		public const int NET_DVR_SET_NTPCFG = 225;

		// Token: 0x0400100D RID: 4109
		public const int NET_DVR_SET_PICCFG = 105;

		// Token: 0x0400100E RID: 4110
		public const int NET_DVR_SET_PICCFG_EX = 201;

		// Token: 0x0400100F RID: 4111
		public const int NET_DVR_SET_PICCFG_V30 = 1003;

		// Token: 0x04001010 RID: 4112
		public const int NET_DVR_SET_PLATECFG = 150;

		// Token: 0x04001011 RID: 4113
		public const int NET_DVR_SET_PREVIEWCFG = 121;

		// Token: 0x04001012 RID: 4114
		public const int NET_DVR_SET_PREVIEWCFG_AUX = 143;

		// Token: 0x04001013 RID: 4115
		public const int NET_DVR_SET_PREVIEWCFG_AUX_V30 = 1047;

		// Token: 0x04001014 RID: 4116
		public const int NET_DVR_SET_PREVIEWCFG_V30 = 1045;

		// Token: 0x04001015 RID: 4117
		public const int NET_DVR_SET_PTZPOS = 292;

		// Token: 0x04001016 RID: 4118
		public const int NET_DVR_SET_RECORDCFG = 109;

		// Token: 0x04001017 RID: 4119
		public const int NET_DVR_SET_RECORDCFG_V30 = 1005;

		// Token: 0x04001018 RID: 4120
		public const int NET_DVR_SET_RS232CFG = 113;

		// Token: 0x04001019 RID: 4121
		public const int NET_DVR_SET_RS232CFG_V30 = 1037;

		// Token: 0x0400101A RID: 4122
		public const int NET_DVR_SET_RULECFG = 152;

		// Token: 0x0400101B RID: 4123
		public const int NET_DVR_SET_SHOWSTRING = 131;

		// Token: 0x0400101C RID: 4124
		public const int NET_DVR_SET_SHOWSTRING_EX = 239;

		// Token: 0x0400101D RID: 4125
		public const int NET_DVR_SET_SHOWSTRING_V30 = 1031;

		// Token: 0x0400101E RID: 4126
		public const int NET_DVR_SET_TIMECFG = 119;

		// Token: 0x0400101F RID: 4127
		public const int NET_DVR_SET_USERCFG = 125;

		// Token: 0x04001020 RID: 4128
		public const int NET_DVR_SET_USERCFG_EX = 203;

		// Token: 0x04001021 RID: 4129
		public const int NET_DVR_SET_USERCFG_V30 = 1007;

		// Token: 0x04001022 RID: 4130
		public const int NET_DVR_SET_VCA_CTRLCFG = 164;

		// Token: 0x04001023 RID: 4131
		public const int NET_DVR_SET_VCA_ENTER_REGION = 168;

		// Token: 0x04001024 RID: 4132
		public const int NET_DVR_SET_VCA_LINE_SEGMENT = 170;

		// Token: 0x04001025 RID: 4133
		public const int NET_DVR_SET_VCA_MASK_REGION = 166;

		// Token: 0x04001026 RID: 4134
		public const int NET_DVR_SET_VIDEOOUTCFG = 123;

		// Token: 0x04001027 RID: 4135
		public const int NET_DVR_SET_VIDEOOUTCFG_V30 = 1029;

		// Token: 0x04001028 RID: 4136
		public const int NET_DVR_SET_WIFI_CFG = 306;

		// Token: 0x04001029 RID: 4137
		public const int NET_DVR_SET_WIFI_WORKMODE = 308;

		// Token: 0x0400102A RID: 4138
		public const int NET_DVR_SET_ZONEANDDST = 129;

		// Token: 0x0400102B RID: 4139
		public const int NET_DVR_SETSOCKET_ERROR = 45;

		// Token: 0x0400102C RID: 4140
		public const int NET_DVR_SETSPEED = 24;

		// Token: 0x0400102D RID: 4141
		public const int NET_DVR_SHOWLOGO = 1;

		// Token: 0x0400102E RID: 4142
		public const int NET_DVR_SOCKETCLOSE_ERROR = 73;

		// Token: 0x0400102F RID: 4143
		public const int NET_DVR_SOCKETLISTEN_ERROR = 75;

		// Token: 0x04001030 RID: 4144
		public const int NET_DVR_STD_AUDIODATA = 5;

		// Token: 0x04001031 RID: 4145
		public const int NET_DVR_STD_VIDEODATA = 4;

		// Token: 0x04001032 RID: 4146
		public const int NET_DVR_STREAM_TYPE_HIKPRIVT = 1;

		// Token: 0x04001033 RID: 4147
		public const int NET_DVR_STREAM_TYPE_PS = 8;

		// Token: 0x04001034 RID: 4148
		public const int NET_DVR_STREAM_TYPE_RTP = 9;

		// Token: 0x04001035 RID: 4149
		public const int NET_DVR_STREAM_TYPE_TS = 7;

		// Token: 0x04001036 RID: 4150
		public const int NET_DVR_STREAM_TYPE_UNKOWN = 0;

		// Token: 0x04001037 RID: 4151
		public const int NET_DVR_STREAMDATA = 2;

		// Token: 0x04001038 RID: 4152
		public const int NET_DVR_SUPPORT_BLT = 2;

		// Token: 0x04001039 RID: 4153
		public const int NET_DVR_SUPPORT_BLTFOURCC = 4;

		// Token: 0x0400103A RID: 4154
		public const int NET_DVR_SUPPORT_BLTSHRINKX = 8;

		// Token: 0x0400103B RID: 4155
		public const int NET_DVR_SUPPORT_BLTSHRINKY = 16;

		// Token: 0x0400103C RID: 4156
		public const int NET_DVR_SUPPORT_BLTSTRETCHX = 32;

		// Token: 0x0400103D RID: 4157
		public const int NET_DVR_SUPPORT_BLTSTRETCHY = 64;

		// Token: 0x0400103E RID: 4158
		public const int NET_DVR_SUPPORT_DDRAW = 1;

		// Token: 0x0400103F RID: 4159
		public const int NET_DVR_SUPPORT_MMX = 256;

		// Token: 0x04001040 RID: 4160
		public const int NET_DVR_SUPPORT_SSE = 128;

		// Token: 0x04001041 RID: 4161
		public const int NET_DVR_SYSHEAD = 1;

		// Token: 0x04001042 RID: 4162
		public const int NET_DVR_THROWBFRAME = 20;

		// Token: 0x04001043 RID: 4163
		public const int NET_DVR_TIMEINPUTERROR = 32;

		// Token: 0x04001044 RID: 4164
		public const int NET_DVR_UPGRADEFAIL = 49;

		// Token: 0x04001045 RID: 4165
		public const int NET_DVR_UPGRADELANGMISMATCH = 57;

		// Token: 0x04001046 RID: 4166
		public const int NET_DVR_USERID_ISUSING = 74;

		// Token: 0x04001047 RID: 4167
		public const int NET_DVR_USERNOTEXIST = 47;

		// Token: 0x04001048 RID: 4168
		public const int NET_DVR_VERSIONNOMATCH = 6;

		// Token: 0x04001049 RID: 4169
		public const int NET_DVR_VOICEMONOPOLIZE = 69;

		// Token: 0x0400104A RID: 4170
		public const int NET_DVR_WITHSAMEUSERNAME = 79;

		// Token: 0x0400104B RID: 4171
		public const int NET_DVR_WRITEFILE_FAILED = 77;

		// Token: 0x0400104C RID: 4172
		public const int NET_DVR_WRITEFLASHERROR = 48;

		// Token: 0x0400104D RID: 4173
		public const int NET_PALYM4_INIT_DECODER_ERROR = 519;

		// Token: 0x0400104E RID: 4174
		public const int NET_PLAYM4_ALLOC_MEMORY_ERROR = 506;

		// Token: 0x0400104F RID: 4175
		public const int NET_PLAYM4_BLT_ERROR = 522;

		// Token: 0x04001050 RID: 4176
		public const int NET_PLAYM4_BUF_OVER = 511;

		// Token: 0x04001051 RID: 4177
		public const int NET_PLAYM4_CHECK_FILE_ERROR = 520;

		// Token: 0x04001052 RID: 4178
		public const int NET_PLAYM4_CREATE_DDRAW_ERROR = 509;

		// Token: 0x04001053 RID: 4179
		public const int NET_PLAYM4_CREATE_OBJ_ERROR = 508;

		// Token: 0x04001054 RID: 4180
		public const int NET_PLAYM4_CREATE_OFFSCREEN_ERROR = 510;

		// Token: 0x04001055 RID: 4181
		public const int NET_PLAYM4_CREATE_SOUND_ERROR = 512;

		// Token: 0x04001056 RID: 4182
		public const int NET_PLAYM4_DEC_AUDIO_ERROR = 505;

		// Token: 0x04001057 RID: 4183
		public const int NET_PLAYM4_DEC_VIDEO_ERROR = 504;

		// Token: 0x04001058 RID: 4184
		public const int NET_PLAYM4_EXTRACT_DATA_ERROR = 528;

		// Token: 0x04001059 RID: 4185
		public const int NET_PLAYM4_EXTRACT_NOT_SUPPORT = 527;

		// Token: 0x0400105A RID: 4186
		public const int NET_PLAYM4_FILEHEADER_UNKNOWN = 517;

		// Token: 0x0400105B RID: 4187
		public const int NET_PLAYM4_INIT_TIMER_ERROR = 521;

		// Token: 0x0400105C RID: 4188
		public const int NET_PLAYM4_JPEG_COMPRESS_ERROR = 526;

		// Token: 0x0400105D RID: 4189
		public const int NET_PLAYM4_NOERROR = 500;

		// Token: 0x0400105E RID: 4190
		public const int NET_PLAYM4_OPEN_FILE_ERROR = 507;

		// Token: 0x0400105F RID: 4191
		public const int NET_PLAYM4_OPEN_FILE_ERROR_MULTI = 524;

		// Token: 0x04001060 RID: 4192
		public const int NET_PLAYM4_OPEN_FILE_ERROR_VIDEO = 525;

		// Token: 0x04001061 RID: 4193
		public const int NET_PLAYM4_ORDER_ERROR = 502;

		// Token: 0x04001062 RID: 4194
		public const int NET_PLAYM4_PARA_OVER = 501;

		// Token: 0x04001063 RID: 4195
		public const int NET_PLAYM4_SET_VOLUME_ERROR = 513;

		// Token: 0x04001064 RID: 4196
		public const int NET_PLAYM4_SUPPORT_FILE_ONLY = 514;

		// Token: 0x04001065 RID: 4197
		public const int NET_PLAYM4_SUPPORT_STREAM_ONLY = 515;

		// Token: 0x04001066 RID: 4198
		public const int NET_PLAYM4_SYS_NOT_SUPPORT = 516;

		// Token: 0x04001067 RID: 4199
		public const int NET_PLAYM4_TIMER_ERROR = 503;

		// Token: 0x04001068 RID: 4200
		public const int NET_PLAYM4_UPDATE_ERROR = 523;

		// Token: 0x04001069 RID: 4201
		public const int NET_PLAYM4_VERSION_INCORRECT = 518;

		// Token: 0x0400106A RID: 4202
		public const int NOACTION = 0;

		// Token: 0x0400106B RID: 4203
		public const int NORMALCONNECT = 1;

		// Token: 0x0400106C RID: 4204
		public const int NXS = 24;

		// Token: 0x0400106D RID: 4205
		public const int OLIVETTI = 4;

		// Token: 0x0400106E RID: 4206
		public const int PAN_AUTO = 29;

		// Token: 0x0400106F RID: 4207
		public const int PAN_LEFT = 23;

		// Token: 0x04001070 RID: 4208
		public const int PAN_RIGHT = 24;

		// Token: 0x04001071 RID: 4209
		public const int PARA_ALARM = 16;

		// Token: 0x04001072 RID: 4210
		public const int PARA_DATETIME = 1024;

		// Token: 0x04001073 RID: 4211
		public const int PARA_DECODER = 64;

		// Token: 0x04001074 RID: 4212
		public const int PARA_ENCODE = 4;

		// Token: 0x04001075 RID: 4213
		public const int PARA_EXCEPTION = 32;

		// Token: 0x04001076 RID: 4214
		public const int PARA_FRAMETYPE = 2048;

		// Token: 0x04001077 RID: 4215
		public const int PARA_IMAGE = 2;

		// Token: 0x04001078 RID: 4216
		public const int PARA_NETWORK = 8;

		// Token: 0x04001079 RID: 4217
		public const int PARA_PREVIEW = 256;

		// Token: 0x0400107A RID: 4218
		public const int PARA_RS232 = 128;

		// Token: 0x0400107B RID: 4219
		public const int PARA_SECURITY = 512;

		// Token: 0x0400107C RID: 4220
		public const int PARA_VCA_RULE = 4096;

		// Token: 0x0400107D RID: 4221
		public const int PARA_VIDEOOUT = 1;

		// Token: 0x0400107E RID: 4222
		public const int PASSWD_LEN = 16;

		// Token: 0x0400107F RID: 4223
		public const int PATHNAME_LEN = 128;

		// Token: 0x04001080 RID: 4224
		public const int PCDVR = 3;

		// Token: 0x04001081 RID: 4225
		public const int PCNVR = 6;

		// Token: 0x04001082 RID: 4226
		public const int PHONENUMBER_LEN = 32;

		// Token: 0x04001083 RID: 4227
		public const int PLATE_INFO_LEN = 1024;

		// Token: 0x04001084 RID: 4228
		public const int PLATE_NUM_LEN = 16;

		// Token: 0x04001085 RID: 4229
		public const int PTZ_PROTOCOL_NUM = 200;

		// Token: 0x04001086 RID: 4230
		public const int QHTLT = 20;

		// Token: 0x04001087 RID: 4231
		public const int REGIONTYPE = 0;

		// Token: 0x04001088 RID: 4232
		public const int RUN_CRUISE = 36;

		// Token: 0x04001089 RID: 4233
		public const int RUN_SEQ = 37;

		// Token: 0x0400108A RID: 4234
		public const int SDK_HCNETSDK = 2;

		// Token: 0x0400108B RID: 4235
		public const int SDK_PLAYMPEG4 = 1;

		// Token: 0x0400108C RID: 4236
		public const int SEARCH_EVENT_INFO_LEN = 300;

		// Token: 0x0400108D RID: 4237
		public const int SERIALNO_LEN = 48;

		// Token: 0x0400108E RID: 4238
		public const int SET_PRESET = 8;

		// Token: 0x0400108F RID: 4239
		public const int SET_SEQ_DWELL = 31;

		// Token: 0x04001090 RID: 4240
		public const int SET_SEQ_SPEED = 32;

		// Token: 0x04001091 RID: 4241
		public const int SIEMENS = 3;

		// Token: 0x04001092 RID: 4242
		public const int SMI = 7;

		// Token: 0x04001093 RID: 4243
		public const int STA_MEM_CRUISE = 34;

		// Token: 0x04001094 RID: 4244
		public const int STO_MEM_CRUISE = 35;

		// Token: 0x04001095 RID: 4245
		public const int STOP_SEQ = 38;

		// Token: 0x04001096 RID: 4246
		public const int TILT_DOWN = 22;

		// Token: 0x04001097 RID: 4247
		public const int TILT_UP = 21;

		// Token: 0x04001098 RID: 4248
		public const int TRIGGERALARMOUT = 8;

		// Token: 0x04001099 RID: 4249
		public const int UP_LEFT = 25;

		// Token: 0x0400109A RID: 4250
		public const int UP_RIGHT = 26;

		// Token: 0x0400109B RID: 4251
		public const int UPTOCENTER = 4;

		// Token: 0x0400109C RID: 4252
		public const int USERTYPE = 5;

		// Token: 0x0400109D RID: 4253
		public const int VCA_CHAN_ABILITY = 272;

		// Token: 0x0400109E RID: 4254
		public const int VCA_DEV_ABILITY = 256;

		// Token: 0x0400109F RID: 4255
		public const int VCA_MAX_POLYGON_POINT_NUM = 10;

		// Token: 0x040010A0 RID: 4256
		public const int VIDEO_B_FRAME = 2;

		// Token: 0x040010A1 RID: 4257
		public const int VIDEO_BBP_FRAME = 5;

		// Token: 0x040010A2 RID: 4258
		public const int VIDEO_BP_FRAME = 4;

		// Token: 0x040010A3 RID: 4259
		public const int VIDEO_I_FRAME = 1;

		// Token: 0x040010A4 RID: 4260
		public const int VIDEO_P_FRAME = 3;

		// Token: 0x040010A5 RID: 4261
		public const int VIDEOPLATFORM_ABILITY = 528;

		// Token: 0x040010A6 RID: 4262
		public const int WARNONAUDIOOUT = 2;

		// Token: 0x040010A7 RID: 4263
		public const int WARNONMONITOR = 1;

		// Token: 0x040010A8 RID: 4264
		public const int WIFI_MAX_AP_COUNT = 20;

		// Token: 0x040010A9 RID: 4265
		public const int WIFI_WEP_MAX_KEY_COUNT = 4;

		// Token: 0x040010AA RID: 4266
		public const int WIFI_WEP_MAX_KEY_LENGTH = 33;

		// Token: 0x040010AB RID: 4267
		public const int WIFI_WPA_PSK_MAX_KEY_LENGTH = 63;

		// Token: 0x040010AC RID: 4268
		public const int WIFI_WPA_PSK_MIN_KEY_LENGTH = 8;

		// Token: 0x040010AD RID: 4269
		public const int WINCOR_NIXDORF = 2;

		// Token: 0x040010AE RID: 4270
		public const int WIPER_PWRON = 3;

		// Token: 0x040010AF RID: 4271
		public const int WM_NETERROR = 1126;

		// Token: 0x040010B0 RID: 4272
		public const int WM_STREAMEND = 1127;

		// Token: 0x040010B1 RID: 4273
		public const int XiaoXing = 18;

		// Token: 0x040010B2 RID: 4274
		public const int YiHua = 10;

		// Token: 0x040010B3 RID: 4275
		public const int ZOOM_IN = 11;

		// Token: 0x040010B4 RID: 4276
		public const int ZOOM_OUT = 12;

		// Token: 0x02000069 RID: 105
		public enum Anonymous_26594f67_851c_4f7d_bec4_094765b7ff83
		{
			// Token: 0x040010B6 RID: 4278
			BLUE_PLATE,
			// Token: 0x040010B7 RID: 4279
			YELLOW_PLATE,
			// Token: 0x040010B8 RID: 4280
			WHITE_PLATE,
			// Token: 0x040010B9 RID: 4281
			BLACK_PLATE
		}

		// Token: 0x0200006A RID: 106
		public enum BEHAVIOR_MINOR_TYPE
		{
			// Token: 0x040010BB RID: 4283
			EVENT_TRAVERSE_PLANE,
			// Token: 0x040010BC RID: 4284
			EVENT_ENTER_AREA,
			// Token: 0x040010BD RID: 4285
			EVENT_EXIT_AREA,
			// Token: 0x040010BE RID: 4286
			EVENT_INTRUSION,
			// Token: 0x040010BF RID: 4287
			EVENT_LOITER,
			// Token: 0x040010C0 RID: 4288
			EVENT_LEFT_TAKE,
			// Token: 0x040010C1 RID: 4289
			EVENT_PARKING,
			// Token: 0x040010C2 RID: 4290
			EVENT_RUN,
			// Token: 0x040010C3 RID: 4291
			EVENT_HIGH_DENSITY,
			// Token: 0x040010C4 RID: 4292
			EVENT_STICK_UP,
			// Token: 0x040010C5 RID: 4293
			EVENT_INSTALL_SCANNER
		}

		// Token: 0x0200006B RID: 107
		public struct BLOCKTIME
		{
			// Token: 0x040010C6 RID: 4294
			public ushort wYear;

			// Token: 0x040010C7 RID: 4295
			public byte bMonth;

			// Token: 0x040010C8 RID: 4296
			public byte bDay;

			// Token: 0x040010C9 RID: 4297
			public byte bHour;

			// Token: 0x040010CA RID: 4298
			public byte bMinute;

			// Token: 0x040010CB RID: 4299
			public byte bSecond;
		}

		// Token: 0x0200006C RID: 108
		// (Invoke) Token: 0x060008DD RID: 2269
		public delegate int CALLBACKFUN_MESSAGE(int iMessageType, IntPtr pBuf, int iBufLen);

		// Token: 0x0200006D RID: 109
		public enum CAPTURE_MODE
		{
			// Token: 0x040010CD RID: 4301
			BMP_MODE,
			// Token: 0x040010CE RID: 4302
			JPEG_MODE
		}

		// Token: 0x0200006E RID: 110
		public struct CLOSEFILE_INFO
		{
			// Token: 0x040010CF RID: 4303
			public int iHandle;

			// Token: 0x040010D0 RID: 4304
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string strCameraid;

			// Token: 0x040010D1 RID: 4305
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string strFileName;

			// Token: 0x040010D2 RID: 4306
			public CHCNetSDK.BLOCKTIME tFileSwitchTime;
		}

		// Token: 0x0200006F RID: 111
		public struct CONNPARAM
		{
			// Token: 0x040010D3 RID: 4307
			public uint uiUser;

			// Token: 0x040010D4 RID: 4308
			public CHCNetSDK.ErrorCallback errorCB;
		}

		// Token: 0x02000070 RID: 112
		public struct CREATEFILE_INFO
		{
			// Token: 0x040010D5 RID: 4309
			public int iHandle;

			// Token: 0x040010D6 RID: 4310
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string strCameraid;

			// Token: 0x040010D7 RID: 4311
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string strFileName;

			// Token: 0x040010D8 RID: 4312
			public CHCNetSDK.BLOCKTIME tFileCreateTime;
		}

		// Token: 0x02000071 RID: 113
		public enum DISPLAY_MODE
		{
			// Token: 0x040010DA RID: 4314
			NORMALMODE,
			// Token: 0x040010DB RID: 4315
			OVERLAYMODE
		}

		// Token: 0x02000072 RID: 114
		// (Invoke) Token: 0x060008E1 RID: 2273
		public delegate void DRAWFUN(int lRealHandle, IntPtr hDc, uint dwUser);

		// Token: 0x02000073 RID: 115
		// (Invoke) Token: 0x060008E5 RID: 2277
		public delegate void ErrorCallback(IntPtr hSession, uint dwUser, int lErrorType);

		// Token: 0x02000074 RID: 116
		// (Invoke) Token: 0x060008E9 RID: 2281
		public delegate void EXCEPYIONCALLBACK(uint dwType, int lUserID, int lHandle, IntPtr pUser);

		// Token: 0x02000075 RID: 117
		public enum HD_STAT
		{
			// Token: 0x040010DD RID: 4317
			HD_STAT_OK,
			// Token: 0x040010DE RID: 4318
			HD_STAT_UNFORMATTED,
			// Token: 0x040010DF RID: 4319
			HD_STAT_ERROR,
			// Token: 0x040010E0 RID: 4320
			HD_STAT_SMART_FAILED,
			// Token: 0x040010E1 RID: 4321
			HD_STAT_MISMATCH,
			// Token: 0x040010E2 RID: 4322
			HD_STAT_IDLE,
			// Token: 0x040010E3 RID: 4323
			NET_HD_STAT_OFFLINE
		}

		// Token: 0x02000076 RID: 118
		public enum IVS_PARAM_KEY
		{
			// Token: 0x040010E5 RID: 4325
			BACKGROUND_UPDATE_RATE = 2,
			// Token: 0x040010E6 RID: 4326
			ENTER_CHANGE_HOLD = 13,
			// Token: 0x040010E7 RID: 4327
			ILLUMINATION_CHANGE = 11,
			// Token: 0x040010E8 RID: 4328
			MAX_MISSING_DISTANCE = 8,
			// Token: 0x040010E9 RID: 4329
			MIN_OBJECT_SIZE = 5,
			// Token: 0x040010EA RID: 4330
			MISSING_OBJECT_HOLD = 7,
			// Token: 0x040010EB RID: 4331
			OBJECT_DETECT_SENSITIVE = 1,
			// Token: 0x040010EC RID: 4332
			OBJECT_GENERATE_RATE = 6,
			// Token: 0x040010ED RID: 4333
			OBJECT_MERGE_SPEED = 9,
			// Token: 0x040010EE RID: 4334
			REPEATED_MOTION_SUPPRESS,
			// Token: 0x040010EF RID: 4335
			RESUME_DEFAULT_PARAM = 255,
			// Token: 0x040010F0 RID: 4336
			SCENE_CHANGE_RATIO = 3,
			// Token: 0x040010F1 RID: 4337
			SUPPRESS_LAMP,
			// Token: 0x040010F2 RID: 4338
			TRACK_OUTPUT_MODE = 12
		}

		// Token: 0x02000077 RID: 119
		public enum MAIN_EVENT_TYPE
		{
			// Token: 0x040010F4 RID: 4340
			EVENT_MOT_DET,
			// Token: 0x040010F5 RID: 4341
			EVENT_ALARM_IN,
			// Token: 0x040010F6 RID: 4342
			EVENT_VCA_BEHAVIOR
		}

		// Token: 0x02000078 RID: 120
		// (Invoke) Token: 0x060008ED RID: 2285
		public delegate int MESSAGECALLBACK(int lCommand, IntPtr sDVRIP, IntPtr pBuf, uint dwBufLen, uint dwUser);

		// Token: 0x02000079 RID: 121
		// (Invoke) Token: 0x060008F1 RID: 2289
		public delegate int MESSCALLBACK(int lCommand, string sDVRIP, string pBuf, uint dwBufLen);

		// Token: 0x0200007A RID: 122
		// (Invoke) Token: 0x060008F5 RID: 2293
		public delegate int MESSCALLBACKEX(int iCommand, int iUserID, string pBuf, uint dwBufLen);

		// Token: 0x0200007B RID: 123
		// (Invoke) Token: 0x060008F9 RID: 2297
		public delegate int MESSCALLBACKNEW(int lCommand, string sDVRIP, string pBuf, uint dwBufLen, ushort dwLinkDVRPort);

		// Token: 0x0200007C RID: 124
		// (Invoke) Token: 0x060008FD RID: 2301
		public delegate void MSGCallBack(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser);

		// Token: 0x0200007D RID: 125
		public struct NET_DVR_ALARMER
		{
			// Token: 0x040010F7 RID: 4343
			public byte byUserIDValid;

			// Token: 0x040010F8 RID: 4344
			public byte bySerialValid;

			// Token: 0x040010F9 RID: 4345
			public byte byVersionValid;

			// Token: 0x040010FA RID: 4346
			public byte byDeviceNameValid;

			// Token: 0x040010FB RID: 4347
			public byte byMacAddrValid;

			// Token: 0x040010FC RID: 4348
			public byte byLinkPortValid;

			// Token: 0x040010FD RID: 4349
			public byte byDeviceIPValid;

			// Token: 0x040010FE RID: 4350
			public byte bySocketIPValid;

			// Token: 0x040010FF RID: 4351
			public int lUserID;

			// Token: 0x04001100 RID: 4352
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sSerialNumber;

			// Token: 0x04001101 RID: 4353
			public uint dwDeviceVersion;

			// Token: 0x04001102 RID: 4354
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sDeviceName;

			// Token: 0x04001103 RID: 4355
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMacAddr;

			// Token: 0x04001104 RID: 4356
			public ushort wLinkPort;

			// Token: 0x04001105 RID: 4357
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string sDeviceIP;

			// Token: 0x04001106 RID: 4358
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string sSocketIP;

			// Token: 0x04001107 RID: 4359
			public byte byIpProtocol;

			// Token: 0x04001108 RID: 4360
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200007E RID: 126
		public struct NET_DVR_ALARMINCFG
		{
			// Token: 0x04001109 RID: 4361
			public uint dwSize;

			// Token: 0x0400110A RID: 4362
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sAlarmInName;

			// Token: 0x0400110B RID: 4363
			public byte byAlarmType;

			// Token: 0x0400110C RID: 4364
			public byte byAlarmInHandle;

			// Token: 0x0400110D RID: 4365
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION struAlarmHandleType;

			// Token: 0x0400110E RID: 4366
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmTime;

			// Token: 0x0400110F RID: 4367
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRelRecordChan;

			// Token: 0x04001110 RID: 4368
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnablePreset;

			// Token: 0x04001111 RID: 4369
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byPresetNo;

			// Token: 0x04001112 RID: 4370
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCruise;

			// Token: 0x04001113 RID: 4371
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byCruiseNo;

			// Token: 0x04001114 RID: 4372
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnablePtzTrack;

			// Token: 0x04001115 RID: 4373
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byPTZTrack;
		}

		// Token: 0x0200007F RID: 127
		public struct NET_DVR_ALARMINCFG_V30
		{
			// Token: 0x04001116 RID: 4374
			public uint dwSize;

			// Token: 0x04001117 RID: 4375
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sAlarmInName;

			// Token: 0x04001118 RID: 4376
			public byte byAlarmType;

			// Token: 0x04001119 RID: 4377
			public byte byAlarmInHandle;

			// Token: 0x0400111A RID: 4378
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x0400111B RID: 4379
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION_V30 struAlarmHandleType;

			// Token: 0x0400111C RID: 4380
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmTime;

			// Token: 0x0400111D RID: 4381
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byRelRecordChan;

			// Token: 0x0400111E RID: 4382
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnablePreset;

			// Token: 0x0400111F RID: 4383
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byPresetNo;

			// Token: 0x04001120 RID: 4384
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 192, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x04001121 RID: 4385
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnableCruise;

			// Token: 0x04001122 RID: 4386
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byCruiseNo;

			// Token: 0x04001123 RID: 4387
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byEnablePtzTrack;

			// Token: 0x04001124 RID: 4388
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byPTZTrack;

			// Token: 0x04001125 RID: 4389
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;
		}

		// Token: 0x02000080 RID: 128
		public struct NET_DVR_ALARMINFO
		{
			// Token: 0x06000900 RID: 2304 RVA: 0x000DB5C8 File Offset: 0x000DA5C8
			public void Init()
			{
				this.dwAlarmType = 0;
				this.dwAlarmInputNumber = 0;
				this.dwAlarmOutputNumber = new int[4];
				this.dwAlarmRelateChannel = new int[16];
				this.dwChannel = new int[16];
				this.dwDiskNumber = new int[16];
				for (int i = 0; i < 4; i++)
				{
					this.dwAlarmOutputNumber[i] = 0;
				}
				for (int j = 0; j < 16; j++)
				{
					this.dwAlarmRelateChannel[j] = 0;
					this.dwChannel[j] = 0;
				}
				for (int k = 0; k < 16; k++)
				{
					this.dwDiskNumber[k] = 0;
				}
			}

			// Token: 0x06000901 RID: 2305 RVA: 0x000DB660 File Offset: 0x000DA660
			public void Reset()
			{
				this.dwAlarmType = 0;
				this.dwAlarmInputNumber = 0;
				for (int i = 0; i < 4; i++)
				{
					this.dwAlarmOutputNumber[i] = 0;
				}
				for (int j = 0; j < 16; j++)
				{
					this.dwAlarmRelateChannel[j] = 0;
					this.dwChannel[j] = 0;
				}
				for (int k = 0; k < 16; k++)
				{
					this.dwDiskNumber[k] = 0;
				}
			}

			// Token: 0x04001126 RID: 4390
			public int dwAlarmType;

			// Token: 0x04001127 RID: 4391
			public int dwAlarmInputNumber;

			// Token: 0x04001128 RID: 4392
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
			public int[] dwAlarmOutputNumber;

			// Token: 0x04001129 RID: 4393
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U4)]
			public int[] dwAlarmRelateChannel;

			// Token: 0x0400112A RID: 4394
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U4)]
			public int[] dwChannel;

			// Token: 0x0400112B RID: 4395
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U4)]
			public int[] dwDiskNumber;
		}

		// Token: 0x02000081 RID: 129
		public struct NET_DVR_ALARMINFO_V30
		{
			// Token: 0x06000902 RID: 2306 RVA: 0x000DB6C8 File Offset: 0x000DA6C8
			public void Init()
			{
				this.dwAlarmType = 0;
				this.dwAlarmInputNumber = 0;
				this.byAlarmRelateChannel = new byte[64];
				this.byChannel = new byte[64];
				this.byAlarmOutputNumber = new byte[96];
				this.byDiskNumber = new byte[33];
				for (int i = 0; i < 64; i++)
				{
					this.byAlarmRelateChannel[i] = Convert.ToByte(0);
					this.byChannel[i] = Convert.ToByte(0);
				}
				for (int j = 0; j < 96; j++)
				{
					this.byAlarmOutputNumber[j] = Convert.ToByte(0);
				}
				for (int k = 0; k < 33; k++)
				{
					this.byDiskNumber[k] = Convert.ToByte(0);
				}
			}

			// Token: 0x06000903 RID: 2307 RVA: 0x000DB778 File Offset: 0x000DA778
			public void Reset()
			{
				this.dwAlarmType = 0;
				this.dwAlarmInputNumber = 0;
				for (int i = 0; i < 64; i++)
				{
					this.byAlarmRelateChannel[i] = Convert.ToByte(0);
					this.byChannel[i] = Convert.ToByte(0);
				}
				for (int j = 0; j < 96; j++)
				{
					this.byAlarmOutputNumber[j] = Convert.ToByte(0);
				}
				for (int k = 0; k < 33; k++)
				{
					this.byDiskNumber[k] = Convert.ToByte(0);
				}
			}

			// Token: 0x0400112C RID: 4396
			public int dwAlarmType;

			// Token: 0x0400112D RID: 4397
			public int dwAlarmInputNumber;

			// Token: 0x0400112E RID: 4398
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmOutputNumber;

			// Token: 0x0400112F RID: 4399
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmRelateChannel;

			// Token: 0x04001130 RID: 4400
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byChannel;

			// Token: 0x04001131 RID: 4401
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.I1)]
			public byte[] byDiskNumber;
		}

		// Token: 0x02000082 RID: 130
		public struct NET_DVR_ALARMMODECFG
		{
			// Token: 0x04001132 RID: 4402
			public uint dwSize;

			// Token: 0x04001133 RID: 4403
			public byte byAlarmMode;

			// Token: 0x04001134 RID: 4404
			public ushort wLoopTime;

			// Token: 0x04001135 RID: 4405
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000083 RID: 131
		public struct NET_DVR_ALARMOUTCFG
		{
			// Token: 0x04001136 RID: 4406
			public uint dwSize;

			// Token: 0x04001137 RID: 4407
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sAlarmOutName;

			// Token: 0x04001138 RID: 4408
			public uint dwAlarmOutDelay;

			// Token: 0x04001139 RID: 4409
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmOutTime;
		}

		// Token: 0x02000084 RID: 132
		public struct NET_DVR_ALARMOUTCFG_V30
		{
			// Token: 0x0400113A RID: 4410
			public uint dwSize;

			// Token: 0x0400113B RID: 4411
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sAlarmOutName;

			// Token: 0x0400113C RID: 4412
			public uint dwAlarmOutDelay;

			// Token: 0x0400113D RID: 4413
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmOutTime;

			// Token: 0x0400113E RID: 4414
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000085 RID: 133
		public struct NET_DVR_ALARMOUTSTATUS
		{
			// Token: 0x0400113F RID: 4415
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] Output;
		}

		// Token: 0x02000086 RID: 134
		public struct NET_DVR_ALARMOUTSTATUS_V30
		{
			// Token: 0x06000904 RID: 2308 RVA: 0x000DB7F2 File Offset: 0x000DA7F2
			public void Init()
			{
				this.Output = new byte[96];
			}

			// Token: 0x04001140 RID: 4416
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.I1)]
			public byte[] Output;
		}

		// Token: 0x02000087 RID: 135
		public struct NET_DVR_ALLSUBSYSTEMINFO
		{
			// Token: 0x04001141 RID: 4417
			public uint dwSize;

			// Token: 0x04001142 RID: 4418
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 80, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SUBSYSTEMINFO[] struSubSystemInfo;

			// Token: 0x04001143 RID: 4419
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000088 RID: 136
		public struct NET_DVR_ASSOCIATECFG
		{
			// Token: 0x04001144 RID: 4420
			public byte byAssociateType;

			// Token: 0x04001145 RID: 4421
			public ushort wAlarmDelay;

			// Token: 0x04001146 RID: 4422
			public byte byAlarmNum;

			// Token: 0x04001147 RID: 4423
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000089 RID: 137
		public struct NET_DVR_AUXOUTCFG
		{
			// Token: 0x04001148 RID: 4424
			public uint dwSize;

			// Token: 0x04001149 RID: 4425
			public uint dwAlarmOutChan;

			// Token: 0x0400114A RID: 4426
			public uint dwAlarmChanSwitchTime;

			// Token: 0x0400114B RID: 4427
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
			public uint[] dwAuxSwitchTime;

			// Token: 0x0400114C RID: 4428
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byAuxOrder;
		}

		// Token: 0x0200008A RID: 138
		public struct NET_DVR_CARDINFO
		{
			// Token: 0x0400114D RID: 4429
			public int lChannel;

			// Token: 0x0400114E RID: 4430
			public int lLinkMode;

			// Token: 0x0400114F RID: 4431
			[MarshalAs(UnmanagedType.LPStr)]
			public string sMultiCastIP;

			// Token: 0x04001150 RID: 4432
			public CHCNetSDK.NET_DVR_DISPLAY_PARA struDisplayPara;
		}

		// Token: 0x0200008B RID: 139
		public struct NET_DVR_CHANNELSTATE
		{
			// Token: 0x04001151 RID: 4433
			public byte byRecordStatic;

			// Token: 0x04001152 RID: 4434
			public byte bySignalStatic;

			// Token: 0x04001153 RID: 4435
			public byte byHardwareStatic;

			// Token: 0x04001154 RID: 4436
			public byte reservedData;

			// Token: 0x04001155 RID: 4437
			public uint dwBitRate;

			// Token: 0x04001156 RID: 4438
			public uint dwLinkNum;

			// Token: 0x04001157 RID: 4439
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
			public uint[] dwClientIP;
		}

		// Token: 0x0200008C RID: 140
		public struct NET_DVR_CHANNELSTATE_V30
		{
			// Token: 0x06000905 RID: 2309 RVA: 0x000DB804 File Offset: 0x000DA804
			public void Init()
			{
				this.struClientIP = new CHCNetSDK.NET_DVR_IPADDR[6];
				for (int i = 0; i < 6; i++)
				{
					this.struClientIP[i].Init();
				}
				this.byRes = new byte[12];
			}

			// Token: 0x04001158 RID: 4440
			public byte byRecordStatic;

			// Token: 0x04001159 RID: 4441
			public byte bySignalStatic;

			// Token: 0x0400115A RID: 4442
			public byte byHardwareStatic;

			// Token: 0x0400115B RID: 4443
			public byte byRes1;

			// Token: 0x0400115C RID: 4444
			public uint dwBitRate;

			// Token: 0x0400115D RID: 4445
			public uint dwLinkNum;

			// Token: 0x0400115E RID: 4446
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPADDR[] struClientIP;

			// Token: 0x0400115F RID: 4447
			public uint dwIPLinkNum;

			// Token: 0x04001160 RID: 4448
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200008D RID: 141
		public struct NET_DVR_CLIENTINFO
		{
			// Token: 0x04001161 RID: 4449
			public int lChannel;

			// Token: 0x04001162 RID: 4450
			public int lLinkMode;

			// Token: 0x04001163 RID: 4451
			public IntPtr hPlayWnd;

			// Token: 0x04001164 RID: 4452
			public string sMultiCastIP;
		}

		// Token: 0x0200008E RID: 142
		public struct NET_DVR_CODESPLITTERCFG
		{
			// Token: 0x04001165 RID: 4453
			public uint dwSize;

			// Token: 0x04001166 RID: 4454
			public CHCNetSDK.NET_DVR_CODESYSTEMINFO struCodeSubsystemInfo;

			// Token: 0x04001167 RID: 4455
			public CHCNetSDK.NET_DVR_CODESPLITTERINFO struCodeSplitterInfo;

			// Token: 0x04001168 RID: 4456
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200008F RID: 143
		public struct NET_DVR_CODESPLITTERINFO
		{
			// Token: 0x04001169 RID: 4457
			public uint dwSize;

			// Token: 0x0400116A RID: 4458
			public CHCNetSDK.NET_DVR_IPADDR struIP;

			// Token: 0x0400116B RID: 4459
			public ushort wPort;

			// Token: 0x0400116C RID: 4460
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x0400116D RID: 4461
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x0400116E RID: 4462
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x0400116F RID: 4463
			public byte byChan;

			// Token: 0x04001170 RID: 4464
			public byte by485Port;

			// Token: 0x04001171 RID: 4465
			public ushort wBaudRate;

			// Token: 0x04001172 RID: 4466
			public byte byDataBit;

			// Token: 0x04001173 RID: 4467
			public byte byStopBit;

			// Token: 0x04001174 RID: 4468
			public byte byParity;

			// Token: 0x04001175 RID: 4469
			public byte byFlowControl;

			// Token: 0x04001176 RID: 4470
			public ushort wDecoderType;

			// Token: 0x04001177 RID: 4471
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x02000090 RID: 144
		public struct NET_DVR_CODESYSTEMINFO
		{
			// Token: 0x04001178 RID: 4472
			public byte bySerialNum;

			// Token: 0x04001179 RID: 4473
			public byte byChan;

			// Token: 0x0400117A RID: 4474
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;
		}

		// Token: 0x02000091 RID: 145
		public struct NET_DVR_COLOR
		{
			// Token: 0x0400117B RID: 4475
			public byte byBrightness;

			// Token: 0x0400117C RID: 4476
			public byte byContrast;

			// Token: 0x0400117D RID: 4477
			public byte bySaturation;

			// Token: 0x0400117E RID: 4478
			public byte byHue;
		}

		// Token: 0x02000092 RID: 146
		public struct NET_DVR_COMPRESSION_INFO
		{
			// Token: 0x0400117F RID: 4479
			public byte byStreamType;

			// Token: 0x04001180 RID: 4480
			public byte byResolution;

			// Token: 0x04001181 RID: 4481
			public byte byBitrateType;

			// Token: 0x04001182 RID: 4482
			public byte byPicQuality;

			// Token: 0x04001183 RID: 4483
			public uint dwVideoBitrate;

			// Token: 0x04001184 RID: 4484
			public uint dwVideoFrameRate;
		}

		// Token: 0x02000093 RID: 147
		public struct NET_DVR_COMPRESSION_INFO_EX
		{
			// Token: 0x04001185 RID: 4485
			public byte byStreamType;

			// Token: 0x04001186 RID: 4486
			public byte byResolution;

			// Token: 0x04001187 RID: 4487
			public byte byBitrateType;

			// Token: 0x04001188 RID: 4488
			public byte byPicQuality;

			// Token: 0x04001189 RID: 4489
			public uint dwVideoBitrate;

			// Token: 0x0400118A RID: 4490
			public uint dwVideoFrameRate;

			// Token: 0x0400118B RID: 4491
			public ushort wIntervalFrameI;

			// Token: 0x0400118C RID: 4492
			public byte byIntervalBPFrame;

			// Token: 0x0400118D RID: 4493
			public byte byRes;
		}

		// Token: 0x02000094 RID: 148
		public struct NET_DVR_COMPRESSION_INFO_V30
		{
			// Token: 0x0400118E RID: 4494
			public byte byStreamType;

			// Token: 0x0400118F RID: 4495
			public byte byResolution;

			// Token: 0x04001190 RID: 4496
			public byte byBitrateType;

			// Token: 0x04001191 RID: 4497
			public byte byPicQuality;

			// Token: 0x04001192 RID: 4498
			public uint dwVideoBitrate;

			// Token: 0x04001193 RID: 4499
			public uint dwVideoFrameRate;

			// Token: 0x04001194 RID: 4500
			public ushort wIntervalFrameI;

			// Token: 0x04001195 RID: 4501
			public byte byIntervalBPFrame;

			// Token: 0x04001196 RID: 4502
			public byte byres1;

			// Token: 0x04001197 RID: 4503
			public byte byVideoEncType;

			// Token: 0x04001198 RID: 4504
			public byte byAudioEncType;

			// Token: 0x04001199 RID: 4505
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
			public byte[] byres;
		}

		// Token: 0x02000095 RID: 149
		public struct NET_DVR_COMPRESSIONCFG
		{
			// Token: 0x0400119A RID: 4506
			public uint dwSize;

			// Token: 0x0400119B RID: 4507
			public CHCNetSDK.NET_DVR_COMPRESSION_INFO struRecordPara;

			// Token: 0x0400119C RID: 4508
			public CHCNetSDK.NET_DVR_COMPRESSION_INFO struNetPara;
		}

		// Token: 0x02000096 RID: 150
		public struct NET_DVR_COMPRESSIONCFG_EX
		{
			// Token: 0x0400119D RID: 4509
			public uint dwSize;

			// Token: 0x0400119E RID: 4510
			public CHCNetSDK.NET_DVR_COMPRESSION_INFO_EX struRecordPara;

			// Token: 0x0400119F RID: 4511
			public CHCNetSDK.NET_DVR_COMPRESSION_INFO_EX struNetPara;
		}

		// Token: 0x02000097 RID: 151
		public struct NET_DVR_COMPRESSIONCFG_NEW
		{
			// Token: 0x040011A0 RID: 4512
			public uint dwSize;

			// Token: 0x040011A1 RID: 4513
			public CHCNetSDK.NET_DVR_COMPRESSION_INFO_EX struLowCompression;

			// Token: 0x040011A2 RID: 4514
			public CHCNetSDK.NET_DVR_COMPRESSION_INFO_EX struEventCompression;
		}

		// Token: 0x02000098 RID: 152
		public struct NET_DVR_COMPRESSIONCFG_V30
		{
			// Token: 0x040011A3 RID: 4515
			public uint dwSize;

			// Token: 0x040011A4 RID: 4516
			public CHCNetSDK.NET_DVR_COMPRESSION_INFO_V30 struNormHighRecordPara;

			// Token: 0x040011A5 RID: 4517
			public CHCNetSDK.NET_DVR_COMPRESSION_INFO_V30 struRes;

			// Token: 0x040011A6 RID: 4518
			public CHCNetSDK.NET_DVR_COMPRESSION_INFO_V30 struEventRecordPara;

			// Token: 0x040011A7 RID: 4519
			public CHCNetSDK.NET_DVR_COMPRESSION_INFO_V30 struNetPara;
		}

		// Token: 0x02000099 RID: 153
		public struct NET_DVR_CRUISE_PARA
		{
			// Token: 0x040011A8 RID: 4520
			public uint dwSize;

			// Token: 0x040011A9 RID: 4521
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byPresetNo;

			// Token: 0x040011AA RID: 4522
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byCruiseSpeed;

			// Token: 0x040011AB RID: 4523
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U2)]
			public ushort[] wDwellTime;

			// Token: 0x040011AC RID: 4524
			public byte byEnableThisCruise;

			// Token: 0x040011AD RID: 4525
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x0200009A RID: 154
		public struct NET_DVR_CRUISE_POINT
		{
			// Token: 0x06000906 RID: 2310 RVA: 0x000DB847 File Offset: 0x000DA847
			public void Init()
			{
				this.PresetNum = 0;
				this.Dwell = 0;
				this.Speed = 0;
				this.Reserve = 0;
			}

			// Token: 0x040011AE RID: 4526
			public byte PresetNum;

			// Token: 0x040011AF RID: 4527
			public byte Dwell;

			// Token: 0x040011B0 RID: 4528
			public byte Speed;

			// Token: 0x040011B1 RID: 4529
			public byte Reserve;
		}

		// Token: 0x0200009B RID: 155
		public struct NET_DVR_CRUISE_RET
		{
			// Token: 0x06000907 RID: 2311 RVA: 0x000DB868 File Offset: 0x000DA868
			public void Init()
			{
				this.struCruisePoint = new CHCNetSDK.NET_DVR_CRUISE_POINT[32];
				for (int i = 0; i < 32; i++)
				{
					this.struCruisePoint[i].Init();
				}
			}

			// Token: 0x040011B2 RID: 4530
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_CRUISE_POINT[] struCruisePoint;
		}

		// Token: 0x0200009C RID: 156
		public struct NET_DVR_DDNSPARA
		{
			// Token: 0x040011B3 RID: 4531
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUsername;

			// Token: 0x040011B4 RID: 4532
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x040011B5 RID: 4533
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sDomainName;

			// Token: 0x040011B6 RID: 4534
			public byte byEnableDDNS;

			// Token: 0x040011B7 RID: 4535
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x0200009D RID: 157
		public struct NET_DVR_DDNSPARA_EX
		{
			// Token: 0x040011B8 RID: 4536
			public byte byHostIndex;

			// Token: 0x040011B9 RID: 4537
			public byte byEnableDDNS;

			// Token: 0x040011BA RID: 4538
			public ushort wDDNSPort;

			// Token: 0x040011BB RID: 4539
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUsername;

			// Token: 0x040011BC RID: 4540
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x040011BD RID: 4541
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sDomainName;

			// Token: 0x040011BE RID: 4542
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sServerName;

			// Token: 0x040011BF RID: 4543
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200009E RID: 158
		public struct NET_DVR_DDNSPARA_V30
		{
			// Token: 0x040011C0 RID: 4544
			public byte byEnableDDNS;

			// Token: 0x040011C1 RID: 4545
			public byte byHostIndex;

			// Token: 0x040011C2 RID: 4546
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040011C3 RID: 4547
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.struDDNS[] struDDNS;

			// Token: 0x040011C4 RID: 4548
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200009F RID: 159
		public struct NET_DVR_DECCFG
		{
			// Token: 0x040011C5 RID: 4549
			public uint dwSize;

			// Token: 0x040011C6 RID: 4550
			public uint dwDecChanNum;

			// Token: 0x040011C7 RID: 4551
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_DECINFO[] struDecInfo;
		}

		// Token: 0x020000A0 RID: 160
		public struct NET_DVR_DECCHANINFO
		{
			// Token: 0x040011C8 RID: 4552
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sDVRIP;

			// Token: 0x040011C9 RID: 4553
			public ushort wDVRPort;

			// Token: 0x040011CA RID: 4554
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x040011CB RID: 4555
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x040011CC RID: 4556
			public byte byChannel;

			// Token: 0x040011CD RID: 4557
			public byte byLinkMode;

			// Token: 0x040011CE RID: 4558
			public byte byLinkType;
		}

		// Token: 0x020000A1 RID: 161
		public struct NET_DVR_DECCHANSTATUS
		{
			// Token: 0x040011CF RID: 4559
			public uint dwWorkType;

			// Token: 0x040011D0 RID: 4560
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sDVRIP;

			// Token: 0x040011D1 RID: 4561
			public ushort wDVRPort;

			// Token: 0x040011D2 RID: 4562
			public byte byChannel;

			// Token: 0x040011D3 RID: 4563
			public byte byLinkMode;

			// Token: 0x040011D4 RID: 4564
			public uint dwLinkType;

			// Token: 0x020000A2 RID: 162
			[StructLayout(LayoutKind.Explicit)]
			public struct objectInfo
			{
				// Token: 0x020000A3 RID: 163
				public struct fileInfo
				{
					// Token: 0x040011D5 RID: 4565
					[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.I1)]
					public byte[] fileName;
				}

				// Token: 0x020000A4 RID: 164
				public struct timeInfo
				{
					// Token: 0x040011D6 RID: 4566
					public uint dwChannel;

					// Token: 0x040011D7 RID: 4567
					[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
					public byte[] sUserName;

					// Token: 0x040011D8 RID: 4568
					[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
					public byte[] sPassword;

					// Token: 0x040011D9 RID: 4569
					public CHCNetSDK.NET_DVR_TIME struStartTime;

					// Token: 0x040011DA RID: 4570
					public CHCNetSDK.NET_DVR_TIME struStopTime;
				}

				// Token: 0x020000A5 RID: 165
				public struct userInfo
				{
					// Token: 0x040011DB RID: 4571
					[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
					public byte[] sUserName;

					// Token: 0x040011DC RID: 4572
					[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
					public byte[] sPassword;

					// Token: 0x040011DD RID: 4573
					[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 52)]
					public string cReserve;
				}
			}
		}

		// Token: 0x020000A6 RID: 166
		public struct NET_DVR_DECINFO
		{
			// Token: 0x040011DE RID: 4574
			public byte byPoolChans;

			// Token: 0x040011DF RID: 4575
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_DECCHANINFO[] struchanConInfo;

			// Token: 0x040011E0 RID: 4576
			public byte byEnablePoll;

			// Token: 0x040011E1 RID: 4577
			public byte byPoolTime;
		}

		// Token: 0x020000A7 RID: 167
		public struct NET_DVR_DECODERCFG
		{
			// Token: 0x040011E2 RID: 4578
			public uint dwSize;

			// Token: 0x040011E3 RID: 4579
			public uint dwBaudRate;

			// Token: 0x040011E4 RID: 4580
			public byte byDataBit;

			// Token: 0x040011E5 RID: 4581
			public byte byStopBit;

			// Token: 0x040011E6 RID: 4582
			public byte byParity;

			// Token: 0x040011E7 RID: 4583
			public byte byFlowcontrol;

			// Token: 0x040011E8 RID: 4584
			public ushort wDecoderType;

			// Token: 0x040011E9 RID: 4585
			public ushort wDecoderAddress;

			// Token: 0x040011EA RID: 4586
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] bySetPreset;

			// Token: 0x040011EB RID: 4587
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] bySetCruise;

			// Token: 0x040011EC RID: 4588
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] bySetTrack;
		}

		// Token: 0x020000A8 RID: 168
		public struct NET_DVR_DECODERCFG_V30
		{
			// Token: 0x040011ED RID: 4589
			public uint dwSize;

			// Token: 0x040011EE RID: 4590
			public uint dwBaudRate;

			// Token: 0x040011EF RID: 4591
			public byte byDataBit;

			// Token: 0x040011F0 RID: 4592
			public byte byStopBit;

			// Token: 0x040011F1 RID: 4593
			public byte byParity;

			// Token: 0x040011F2 RID: 4594
			public byte byFlowcontrol;

			// Token: 0x040011F3 RID: 4595
			public ushort wDecoderType;

			// Token: 0x040011F4 RID: 4596
			public ushort wDecoderAddress;

			// Token: 0x040011F5 RID: 4597
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] bySetPreset;

			// Token: 0x040011F6 RID: 4598
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] bySetCruise;

			// Token: 0x040011F7 RID: 4599
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] bySetTrack;
		}

		// Token: 0x020000A9 RID: 169
		public struct NET_DVR_DECODERINFO
		{
			// Token: 0x040011F8 RID: 4600
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byEncoderIP;

			// Token: 0x040011F9 RID: 4601
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byEncoderUser;

			// Token: 0x040011FA RID: 4602
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byEncoderPasswd;

			// Token: 0x040011FB RID: 4603
			public byte bySendMode;

			// Token: 0x040011FC RID: 4604
			public byte byEncoderChannel;

			// Token: 0x040011FD RID: 4605
			public ushort wEncoderPort;

			// Token: 0x040011FE RID: 4606
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] reservedData;
		}

		// Token: 0x020000AA RID: 170
		public struct NET_DVR_DECODERSTATE
		{
			// Token: 0x040011FF RID: 4607
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byEncoderIP;

			// Token: 0x04001200 RID: 4608
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byEncoderUser;

			// Token: 0x04001201 RID: 4609
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byEncoderPasswd;

			// Token: 0x04001202 RID: 4610
			public byte byEncoderChannel;

			// Token: 0x04001203 RID: 4611
			public byte bySendMode;

			// Token: 0x04001204 RID: 4612
			public ushort wEncoderPort;

			// Token: 0x04001205 RID: 4613
			public uint dwConnectState;

			// Token: 0x04001206 RID: 4614
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] reservedData;
		}

		// Token: 0x020000AB RID: 171
		public struct NET_DVR_DECODESCHED
		{
			// Token: 0x04001207 RID: 4615
			public CHCNetSDK.NET_DVR_SCHEDTIME struSchedTime;

			// Token: 0x04001208 RID: 4616
			public byte byDecodeType;

			// Token: 0x04001209 RID: 4617
			public byte byLoopGroup;

			// Token: 0x0400120A RID: 4618
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400120B RID: 4619
			public CHCNetSDK.NET_DVR_PU_STREAM_CFG struDynamicDec;
		}

		// Token: 0x020000AC RID: 172
		public struct NET_DVR_DECSTATUS
		{
			// Token: 0x0400120C RID: 4620
			public uint dwSize;

			// Token: 0x0400120D RID: 4621
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_DECCHANSTATUS[] struTransPortInfo;
		}

		// Token: 0x020000AD RID: 173
		public struct NET_DVR_DEVICECFG
		{
			// Token: 0x0400120E RID: 4622
			public uint dwSize;

			// Token: 0x0400120F RID: 4623
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sDVRName;

			// Token: 0x04001210 RID: 4624
			public uint dwDVRID;

			// Token: 0x04001211 RID: 4625
			public uint dwRecycleRecord;

			// Token: 0x04001212 RID: 4626
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sSerialNumber;

			// Token: 0x04001213 RID: 4627
			public uint dwSoftwareVersion;

			// Token: 0x04001214 RID: 4628
			public uint dwSoftwareBuildDate;

			// Token: 0x04001215 RID: 4629
			public uint dwDSPSoftwareVersion;

			// Token: 0x04001216 RID: 4630
			public uint dwDSPSoftwareBuildDate;

			// Token: 0x04001217 RID: 4631
			public uint dwPanelVersion;

			// Token: 0x04001218 RID: 4632
			public uint dwHardwareVersion;

			// Token: 0x04001219 RID: 4633
			public byte byAlarmInPortNum;

			// Token: 0x0400121A RID: 4634
			public byte byAlarmOutPortNum;

			// Token: 0x0400121B RID: 4635
			public byte byRS232Num;

			// Token: 0x0400121C RID: 4636
			public byte byRS485Num;

			// Token: 0x0400121D RID: 4637
			public byte byNetworkPortNum;

			// Token: 0x0400121E RID: 4638
			public byte byDiskCtrlNum;

			// Token: 0x0400121F RID: 4639
			public byte byDiskNum;

			// Token: 0x04001220 RID: 4640
			public byte byDVRType;

			// Token: 0x04001221 RID: 4641
			public byte byChanNum;

			// Token: 0x04001222 RID: 4642
			public byte byStartChan;

			// Token: 0x04001223 RID: 4643
			public byte byDecordChans;

			// Token: 0x04001224 RID: 4644
			public byte byVGANum;

			// Token: 0x04001225 RID: 4645
			public byte byUSBNum;

			// Token: 0x04001226 RID: 4646
			public byte byAuxoutNum;

			// Token: 0x04001227 RID: 4647
			public byte byAudioNum;

			// Token: 0x04001228 RID: 4648
			public byte byIPChanNum;
		}

		// Token: 0x020000AE RID: 174
		public struct NET_DVR_DEVICEINFO
		{
			// Token: 0x04001229 RID: 4649
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sSerialNumber;

			// Token: 0x0400122A RID: 4650
			public byte byAlarmInPortNum;

			// Token: 0x0400122B RID: 4651
			public byte byAlarmOutPortNum;

			// Token: 0x0400122C RID: 4652
			public byte byDiskNum;

			// Token: 0x0400122D RID: 4653
			public byte byDVRType;

			// Token: 0x0400122E RID: 4654
			public byte byChanNum;

			// Token: 0x0400122F RID: 4655
			public byte byStartChan;
		}

		// Token: 0x020000AF RID: 175
		public struct NET_DVR_DEVICEINFO_V30
		{
			// Token: 0x04001230 RID: 4656
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sSerialNumber;

			// Token: 0x04001231 RID: 4657
			public byte byAlarmInPortNum;

			// Token: 0x04001232 RID: 4658
			public byte byAlarmOutPortNum;

			// Token: 0x04001233 RID: 4659
			public byte byDiskNum;

			// Token: 0x04001234 RID: 4660
			public byte byDVRType;

			// Token: 0x04001235 RID: 4661
			public byte byChanNum;

			// Token: 0x04001236 RID: 4662
			public byte byStartChan;

			// Token: 0x04001237 RID: 4663
			public byte byAudioChanNum;

			// Token: 0x04001238 RID: 4664
			public byte byIPChanNum;

			// Token: 0x04001239 RID: 4665
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;
		}

		// Token: 0x020000B0 RID: 176
		public struct NET_DVR_DISKSTATE
		{
			// Token: 0x0400123A RID: 4666
			public uint dwVolume;

			// Token: 0x0400123B RID: 4667
			public uint dwFreeSpace;

			// Token: 0x0400123C RID: 4668
			public uint dwHardDiskStatic;
		}

		// Token: 0x020000B1 RID: 177
		public struct NET_DVR_DISP_CHAN_STATUS
		{
			// Token: 0x0400123D RID: 4669
			public byte byDispStatus;

			// Token: 0x0400123E RID: 4670
			public byte byBVGA;

			// Token: 0x0400123F RID: 4671
			public byte byVideoFormat;

			// Token: 0x04001240 RID: 4672
			public byte byWindowMode;

			// Token: 0x04001241 RID: 4673
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
			public byte[] byJoinDecChan;

			// Token: 0x04001242 RID: 4674
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byFpsDisp;

			// Token: 0x04001243 RID: 4675
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x020000B2 RID: 178
		public struct NET_DVR_DISPLAY_PARA
		{
			// Token: 0x04001244 RID: 4676
			public int bToScreen;

			// Token: 0x04001245 RID: 4677
			public int bToVideoOut;

			// Token: 0x04001246 RID: 4678
			public int nLeft;

			// Token: 0x04001247 RID: 4679
			public int nTop;

			// Token: 0x04001248 RID: 4680
			public int nWidth;

			// Token: 0x04001249 RID: 4681
			public int nHeight;

			// Token: 0x0400124A RID: 4682
			public int nReserved;
		}

		// Token: 0x020000B3 RID: 179
		public struct NET_DVR_DYNAMICDECODE
		{
			// Token: 0x0400124B RID: 4683
			public uint dwSize;

			// Token: 0x0400124C RID: 4684
			public CHCNetSDK.NET_DVR_ASSOCIATECFG struAssociateCfg;

			// Token: 0x0400124D RID: 4685
			public CHCNetSDK.NET_DVR_PU_STREAM_CFG struPuStreamCfg;

			// Token: 0x0400124E RID: 4686
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000B4 RID: 180
		public struct NET_DVR_EMAILCFG
		{
			// Token: 0x0400124F RID: 4687
			public uint dwSize;

			// Token: 0x04001250 RID: 4688
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sUserName;

			// Token: 0x04001251 RID: 4689
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sPassWord;

			// Token: 0x04001252 RID: 4690
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sFromName;

			// Token: 0x04001253 RID: 4691
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
			public string sFromAddr;

			// Token: 0x04001254 RID: 4692
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sToName1;

			// Token: 0x04001255 RID: 4693
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sToName2;

			// Token: 0x04001256 RID: 4694
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
			public string sToAddr1;

			// Token: 0x04001257 RID: 4695
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
			public string sToAddr2;

			// Token: 0x04001258 RID: 4696
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sEmailServer;

			// Token: 0x04001259 RID: 4697
			public byte byServerType;

			// Token: 0x0400125A RID: 4698
			public byte byUseAuthen;

			// Token: 0x0400125B RID: 4699
			public byte byAttachment;

			// Token: 0x0400125C RID: 4700
			public byte byMailinterval;
		}

		// Token: 0x020000B5 RID: 181
		public struct NET_DVR_EMAILCFG_V30
		{
			// Token: 0x0400125D RID: 4701
			public uint dwSize;

			// Token: 0x0400125E RID: 4702
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sAccount;

			// Token: 0x0400125F RID: 4703
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x04001260 RID: 4704
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sSmtpServer;

			// Token: 0x04001261 RID: 4705
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sPop3Server;

			// Token: 0x04001262 RID: 4706
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.struReceiver[] struStringInfo;

			// Token: 0x04001263 RID: 4707
			public byte byAttachment;

			// Token: 0x04001264 RID: 4708
			public byte bySmtpServerVerify;

			// Token: 0x04001265 RID: 4709
			public byte byMailInterval;

			// Token: 0x04001266 RID: 4710
			public byte byEnableSSL;

			// Token: 0x04001267 RID: 4711
			public ushort wSmtpPort;

			// Token: 0x04001268 RID: 4712
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 74, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x020000B6 RID: 182
			public struct struSender
			{
				// Token: 0x04001269 RID: 4713
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
				public byte[] sName;

				// Token: 0x0400126A RID: 4714
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
				public byte[] sAddress;
			}
		}

		// Token: 0x020000B7 RID: 183
		public struct NET_DVR_EMAILPARA
		{
			// Token: 0x0400126B RID: 4715
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sUsername;

			// Token: 0x0400126C RID: 4716
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x0400126D RID: 4717
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sSmtpServer;

			// Token: 0x0400126E RID: 4718
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sPop3Server;

			// Token: 0x0400126F RID: 4719
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sMailAddr;

			// Token: 0x04001270 RID: 4720
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sEventMailAddr1;

			// Token: 0x04001271 RID: 4721
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sEventMailAddr2;

			// Token: 0x04001272 RID: 4722
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x020000B8 RID: 184
		public struct NET_DVR_ETHERNET
		{
			// Token: 0x04001273 RID: 4723
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sDVRIP;

			// Token: 0x04001274 RID: 4724
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sDVRIPMask;

			// Token: 0x04001275 RID: 4725
			public uint dwNetInterface;

			// Token: 0x04001276 RID: 4726
			public ushort wDVRPort;

			// Token: 0x04001277 RID: 4727
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMACAddr;
		}

		// Token: 0x020000B9 RID: 185
		public struct NET_DVR_ETHERNET_V30
		{
			// Token: 0x04001278 RID: 4728
			public CHCNetSDK.NET_DVR_IPADDR struDVRIP;

			// Token: 0x04001279 RID: 4729
			public CHCNetSDK.NET_DVR_IPADDR struDVRIPMask;

			// Token: 0x0400127A RID: 4730
			public uint dwNetInterface;

			// Token: 0x0400127B RID: 4731
			public ushort wDVRPort;

			// Token: 0x0400127C RID: 4732
			public ushort wMTU;

			// Token: 0x0400127D RID: 4733
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMACAddr;
		}

		// Token: 0x020000BA RID: 186
		public struct NET_DVR_EXCEPTION
		{
			// Token: 0x0400127E RID: 4734
			public uint dwSize;

			// Token: 0x0400127F RID: 4735
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION[] struExceptionHandleType;
		}

		// Token: 0x020000BB RID: 187
		public struct NET_DVR_EXCEPTION_V30
		{
			// Token: 0x04001280 RID: 4736
			public uint dwSize;

			// Token: 0x04001281 RID: 4737
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION_V30[] struExceptionHandleType;
		}

		// Token: 0x020000BC RID: 188
		public struct NET_DVR_FILECOND
		{
			// Token: 0x04001282 RID: 4738
			public int lChannel;

			// Token: 0x04001283 RID: 4739
			public uint dwFileType;

			// Token: 0x04001284 RID: 4740
			public uint dwIsLocked;

			// Token: 0x04001285 RID: 4741
			public uint dwUseCardNo;

			// Token: 0x04001286 RID: 4742
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sCardNumber;

			// Token: 0x04001287 RID: 4743
			public CHCNetSDK.NET_DVR_TIME struStartTime;

			// Token: 0x04001288 RID: 4744
			public CHCNetSDK.NET_DVR_TIME struStopTime;
		}

		// Token: 0x020000BD RID: 189
		public struct NET_DVR_FIND_DATA
		{
			// Token: 0x04001289 RID: 4745
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public string sFileName;

			// Token: 0x0400128A RID: 4746
			public CHCNetSDK.NET_DVR_TIME struStartTime;

			// Token: 0x0400128B RID: 4747
			public CHCNetSDK.NET_DVR_TIME struStopTime;

			// Token: 0x0400128C RID: 4748
			public uint dwFileSize;
		}

		// Token: 0x020000BE RID: 190
		public struct NET_DVR_FINDDATA_CARD
		{
			// Token: 0x0400128D RID: 4749
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public string sFileName;

			// Token: 0x0400128E RID: 4750
			public CHCNetSDK.NET_DVR_TIME struStartTime;

			// Token: 0x0400128F RID: 4751
			public CHCNetSDK.NET_DVR_TIME struStopTime;

			// Token: 0x04001290 RID: 4752
			public uint dwFileSize;

			// Token: 0x04001291 RID: 4753
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sCardNum;
		}

		// Token: 0x020000BF RID: 191
		public struct NET_DVR_FINDDATA_V30
		{
			// Token: 0x04001292 RID: 4754
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public string sFileName;

			// Token: 0x04001293 RID: 4755
			public CHCNetSDK.NET_DVR_TIME struStartTime;

			// Token: 0x04001294 RID: 4756
			public CHCNetSDK.NET_DVR_TIME struStopTime;

			// Token: 0x04001295 RID: 4757
			public uint dwFileSize;

			// Token: 0x04001296 RID: 4758
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sCardNum;

			// Token: 0x04001297 RID: 4759
			public byte byLocked;

			// Token: 0x04001298 RID: 4760
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000C0 RID: 192
		public struct NET_DVR_FRAMEFORMAT
		{
			// Token: 0x04001299 RID: 4761
			public uint dwSize;

			// Token: 0x0400129A RID: 4762
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sATMIP;

			// Token: 0x0400129B RID: 4763
			public uint dwATMType;

			// Token: 0x0400129C RID: 4764
			public uint dwInputMode;

			// Token: 0x0400129D RID: 4765
			public uint dwFrameSignBeginPos;

			// Token: 0x0400129E RID: 4766
			public uint dwFrameSignLength;

			// Token: 0x0400129F RID: 4767
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
			public byte[] byFrameSignContent;

			// Token: 0x040012A0 RID: 4768
			public uint dwCardLengthInfoBeginPos;

			// Token: 0x040012A1 RID: 4769
			public uint dwCardLengthInfoLength;

			// Token: 0x040012A2 RID: 4770
			public uint dwCardNumberInfoBeginPos;

			// Token: 0x040012A3 RID: 4771
			public uint dwCardNumberInfoLength;

			// Token: 0x040012A4 RID: 4772
			public uint dwBusinessTypeBeginPos;

			// Token: 0x040012A5 RID: 4773
			public uint dwBusinessTypeLength;

			// Token: 0x040012A6 RID: 4774
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_FRAMETYPECODE[] frameTypeCode;
		}

		// Token: 0x020000C1 RID: 193
		public struct NET_DVR_FRAMEFORMAT_V30
		{
			// Token: 0x040012A7 RID: 4775
			public uint dwSize;

			// Token: 0x040012A8 RID: 4776
			public CHCNetSDK.NET_DVR_IPADDR struATMIP;

			// Token: 0x040012A9 RID: 4777
			public uint dwATMType;

			// Token: 0x040012AA RID: 4778
			public uint dwInputMode;

			// Token: 0x040012AB RID: 4779
			public uint dwFrameSignBeginPos;

			// Token: 0x040012AC RID: 4780
			public uint dwFrameSignLength;

			// Token: 0x040012AD RID: 4781
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
			public byte[] byFrameSignContent;

			// Token: 0x040012AE RID: 4782
			public uint dwCardLengthInfoBeginPos;

			// Token: 0x040012AF RID: 4783
			public uint dwCardLengthInfoLength;

			// Token: 0x040012B0 RID: 4784
			public uint dwCardNumberInfoBeginPos;

			// Token: 0x040012B1 RID: 4785
			public uint dwCardNumberInfoLength;

			// Token: 0x040012B2 RID: 4786
			public uint dwBusinessTypeBeginPos;

			// Token: 0x040012B3 RID: 4787
			public uint dwBusinessTypeLength;

			// Token: 0x040012B4 RID: 4788
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_FRAMETYPECODE[] frameTypeCode;

			// Token: 0x040012B5 RID: 4789
			public ushort wATMPort;

			// Token: 0x040012B6 RID: 4790
			public ushort wProtocolType;

			// Token: 0x040012B7 RID: 4791
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000C2 RID: 194
		public struct NET_DVR_FRAMETYPECODE
		{
			// Token: 0x040012B8 RID: 4792
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
			public byte[] code;
		}

		// Token: 0x020000C3 RID: 195
		public struct NET_DVR_HANDLEEXCEPTION
		{
			// Token: 0x040012B9 RID: 4793
			public uint dwHandleType;

			// Token: 0x040012BA RID: 4794
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRelAlarmOut;
		}

		// Token: 0x020000C4 RID: 196
		public struct NET_DVR_HANDLEEXCEPTION_V30
		{
			// Token: 0x040012BB RID: 4795
			public uint dwHandleType;

			// Token: 0x040012BC RID: 4796
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.I1)]
			public byte[] byRelAlarmOut;
		}

		// Token: 0x020000C5 RID: 197
		public struct NET_DVR_HDCFG
		{
			// Token: 0x040012BD RID: 4797
			public uint dwSize;

			// Token: 0x040012BE RID: 4798
			public uint dwHDCount;

			// Token: 0x040012BF RID: 4799
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SINGLE_HD[] struHDInfo;
		}

		// Token: 0x020000C6 RID: 198
		public struct NET_DVR_HDGROUP_CFG
		{
			// Token: 0x040012C0 RID: 4800
			public uint dwSize;

			// Token: 0x040012C1 RID: 4801
			public uint dwHDGroupCount;

			// Token: 0x040012C2 RID: 4802
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SINGLE_HDGROUP[] struHDGroupAttr;
		}

		// Token: 0x020000C7 RID: 199
		public struct NET_DVR_HIDEALARM
		{
			// Token: 0x040012C3 RID: 4803
			public uint dwEnableHideAlarm;

			// Token: 0x040012C4 RID: 4804
			public ushort wHideAlarmAreaTopLeftX;

			// Token: 0x040012C5 RID: 4805
			public ushort wHideAlarmAreaTopLeftY;

			// Token: 0x040012C6 RID: 4806
			public ushort wHideAlarmAreaWidth;

			// Token: 0x040012C7 RID: 4807
			public ushort wHideAlarmAreaHeight;

			// Token: 0x040012C8 RID: 4808
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION strHideAlarmHandleType;

			// Token: 0x040012C9 RID: 4809
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmTime;
		}

		// Token: 0x020000C8 RID: 200
		public struct NET_DVR_HIDEALARM_V30
		{
			// Token: 0x040012CA RID: 4810
			public uint dwEnableHideAlarm;

			// Token: 0x040012CB RID: 4811
			public ushort wHideAlarmAreaTopLeftX;

			// Token: 0x040012CC RID: 4812
			public ushort wHideAlarmAreaTopLeftY;

			// Token: 0x040012CD RID: 4813
			public ushort wHideAlarmAreaWidth;

			// Token: 0x040012CE RID: 4814
			public ushort wHideAlarmAreaHeight;

			// Token: 0x040012CF RID: 4815
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION_V30 strHideAlarmHandleType;

			// Token: 0x040012D0 RID: 4816
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmTime;
		}

		// Token: 0x020000C9 RID: 201
		public struct NET_DVR_IPADDR
		{
			// Token: 0x06000908 RID: 2312 RVA: 0x000DB8A0 File Offset: 0x000DA8A0
			public void Init()
			{
				this.byRes = new byte[128];
			}

			// Token: 0x040012D1 RID: 4817
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sIpV4;

			// Token: 0x040012D2 RID: 4818
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000CA RID: 202
		public struct NET_DVR_IPALARMINCFG
		{
			// Token: 0x040012D3 RID: 4819
			public uint dwSize;

			// Token: 0x040012D4 RID: 4820
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPALARMININFO[] struIPAlarmInInfo;
		}

		// Token: 0x020000CB RID: 203
		public struct NET_DVR_IPALARMININFO
		{
			// Token: 0x040012D5 RID: 4821
			public byte byIPID;

			// Token: 0x040012D6 RID: 4822
			public byte byAlarmIn;

			// Token: 0x040012D7 RID: 4823
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000CC RID: 204
		public struct NET_DVR_IPALARMOUTCFG
		{
			// Token: 0x06000909 RID: 2313 RVA: 0x000DB8B4 File Offset: 0x000DA8B4
			public void Init()
			{
				this.struIPAlarmOutInfo = new CHCNetSDK.NET_DVR_IPALARMOUTINFO[64];
				for (int i = 0; i < 64; i++)
				{
					this.struIPAlarmOutInfo[i].Init();
				}
			}

			// Token: 0x040012D8 RID: 4824
			public uint dwSize;

			// Token: 0x040012D9 RID: 4825
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPALARMOUTINFO[] struIPAlarmOutInfo;
		}

		// Token: 0x020000CD RID: 205
		public struct NET_DVR_IPALARMOUTINFO
		{
			// Token: 0x0600090A RID: 2314 RVA: 0x000DB8EC File Offset: 0x000DA8EC
			public void Init()
			{
				this.byRes = new byte[18];
			}

			// Token: 0x040012DA RID: 4826
			public byte byIPID;

			// Token: 0x040012DB RID: 4827
			public byte byAlarmOut;

			// Token: 0x040012DC RID: 4828
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000CE RID: 206
		public struct NET_DVR_IPCHANINFO
		{
			// Token: 0x0600090B RID: 2315 RVA: 0x000DB8FB File Offset: 0x000DA8FB
			public void Init()
			{
				this.byRes = new byte[32];
			}

			// Token: 0x040012DD RID: 4829
			public byte byEnable;

			// Token: 0x040012DE RID: 4830
			public byte byIPID;

			// Token: 0x040012DF RID: 4831
			public byte byChannel;

			// Token: 0x040012E0 RID: 4832
			public byte byProType;

			// Token: 0x040012E1 RID: 4833
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000CF RID: 207
		public struct NET_DVR_IPDEVINFO
		{
			// Token: 0x0600090C RID: 2316 RVA: 0x000DB90A File Offset: 0x000DA90A
			public void Init()
			{
				this.sUserName = new byte[32];
				this.sPassword = new byte[16];
				this.byRes = new byte[34];
			}

			// Token: 0x040012E2 RID: 4834
			public uint dwEnable;

			// Token: 0x040012E3 RID: 4835
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x040012E4 RID: 4836
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x040012E5 RID: 4837
			public CHCNetSDK.NET_DVR_IPADDR struIP;

			// Token: 0x040012E6 RID: 4838
			public ushort wDVRPort;

			// Token: 0x040012E7 RID: 4839
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 34, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000D0 RID: 208
		public struct NET_DVR_IPPARACFG
		{
			// Token: 0x0600090D RID: 2317 RVA: 0x000DB934 File Offset: 0x000DA934
			public void Init()
			{
				this.struIPDevInfo = new CHCNetSDK.NET_DVR_IPDEVINFO[32];
				for (int i = 0; i < 32; i++)
				{
					this.struIPDevInfo[i].Init();
				}
				this.byAnalogChanEnable = new byte[32];
				this.struIPChanInfo = new CHCNetSDK.NET_DVR_IPCHANINFO[32];
				for (int i = 0; i < 32; i++)
				{
					this.struIPChanInfo[i].Init();
				}
			}

			// Token: 0x040012E8 RID: 4840
			public uint dwSize;

			// Token: 0x040012E9 RID: 4841
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPDEVINFO[] struIPDevInfo;

			// Token: 0x040012EA RID: 4842
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byAnalogChanEnable;

			// Token: 0x040012EB RID: 4843
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPCHANINFO[] struIPChanInfo;
		}

		// Token: 0x020000D1 RID: 209
		public struct NET_DVR_IPPARACFG_V31
		{
			// Token: 0x0600090E RID: 2318 RVA: 0x000DB9A8 File Offset: 0x000DA9A8
			public void Init()
			{
				this.struIPDevInfo = new CHCNetSDK.tagNET_DVR_IPDEVINFO_V31[32];
				for (int i = 0; i < 32; i++)
				{
					this.struIPDevInfo[i].Init();
				}
				this.byAnalogChanEnable = new byte[32];
				this.struIPChanInfo = new CHCNetSDK.NET_DVR_IPCHANINFO[32];
				for (int i = 0; i < 32; i++)
				{
					this.struIPChanInfo[i].Init();
				}
			}

			// Token: 0x040012EC RID: 4844
			public uint dwSize;

			// Token: 0x040012ED RID: 4845
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_DVR_IPDEVINFO_V31[] struIPDevInfo;

			// Token: 0x040012EE RID: 4846
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byAnalogChanEnable;

			// Token: 0x040012EF RID: 4847
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPCHANINFO[] struIPChanInfo;
		}

		// Token: 0x020000D2 RID: 210
		public struct NET_DVR_JPEGPARA
		{
			// Token: 0x040012F0 RID: 4848
			public ushort wPicSize;

			// Token: 0x040012F1 RID: 4849
			public ushort wPicQuality;
		}

		// Token: 0x020000D3 RID: 211
		public struct NET_DVR_LOG
		{
			// Token: 0x040012F2 RID: 4850
			public CHCNetSDK.NET_DVR_TIME strLogTime;

			// Token: 0x040012F3 RID: 4851
			public uint dwMajorType;

			// Token: 0x040012F4 RID: 4852
			public uint dwMinorType;

			// Token: 0x040012F5 RID: 4853
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPanelUser;

			// Token: 0x040012F6 RID: 4854
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sNetUser;

			// Token: 0x040012F7 RID: 4855
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sRemoteHostAddr;

			// Token: 0x040012F8 RID: 4856
			public uint dwParaType;

			// Token: 0x040012F9 RID: 4857
			public uint dwChannel;

			// Token: 0x040012FA RID: 4858
			public uint dwDiskNumber;

			// Token: 0x040012FB RID: 4859
			public uint dwAlarmInPort;

			// Token: 0x040012FC RID: 4860
			public uint dwAlarmOutPort;
		}

		// Token: 0x020000D4 RID: 212
		public struct NET_DVR_LOG_MATRIX
		{
			// Token: 0x040012FD RID: 4861
			public CHCNetSDK.NET_DVR_TIME strLogTime;

			// Token: 0x040012FE RID: 4862
			public uint dwMajorType;

			// Token: 0x040012FF RID: 4863
			public uint dwMinorType;

			// Token: 0x04001300 RID: 4864
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPanelUser;

			// Token: 0x04001301 RID: 4865
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sNetUser;

			// Token: 0x04001302 RID: 4866
			public CHCNetSDK.NET_DVR_IPADDR struRemoteHostAddr;

			// Token: 0x04001303 RID: 4867
			public uint dwParaType;

			// Token: 0x04001304 RID: 4868
			public uint dwChannel;

			// Token: 0x04001305 RID: 4869
			public uint dwDiskNumber;

			// Token: 0x04001306 RID: 4870
			public uint dwAlarmInPort;

			// Token: 0x04001307 RID: 4871
			public uint dwAlarmOutPort;

			// Token: 0x04001308 RID: 4872
			public uint dwInfoLen;

			// Token: 0x04001309 RID: 4873
			public byte byDevSequence;

			// Token: 0x0400130A RID: 4874
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMacAddr;

			// Token: 0x0400130B RID: 4875
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sSerialNumber;

			// Token: 0x0400130C RID: 4876
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11785)]
			public string sInfo;
		}

		// Token: 0x020000D5 RID: 213
		public struct NET_DVR_LOG_V30
		{
			// Token: 0x0400130D RID: 4877
			public CHCNetSDK.NET_DVR_TIME strLogTime;

			// Token: 0x0400130E RID: 4878
			public uint dwMajorType;

			// Token: 0x0400130F RID: 4879
			public uint dwMinorType;

			// Token: 0x04001310 RID: 4880
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPanelUser;

			// Token: 0x04001311 RID: 4881
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sNetUser;

			// Token: 0x04001312 RID: 4882
			public CHCNetSDK.NET_DVR_IPADDR struRemoteHostAddr;

			// Token: 0x04001313 RID: 4883
			public uint dwParaType;

			// Token: 0x04001314 RID: 4884
			public uint dwChannel;

			// Token: 0x04001315 RID: 4885
			public uint dwDiskNumber;

			// Token: 0x04001316 RID: 4886
			public uint dwAlarmInPort;

			// Token: 0x04001317 RID: 4887
			public uint dwAlarmOutPort;

			// Token: 0x04001318 RID: 4888
			public uint dwInfoLen;

			// Token: 0x04001319 RID: 4889
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11840)]
			public string sInfo;
		}

		// Token: 0x020000D6 RID: 214
		public struct NET_DVR_LOOPPLAN_ARRAYCFG
		{
			// Token: 0x0400131A RID: 4890
			public uint dwSize;

			// Token: 0x0400131B RID: 4891
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_LOOPPLAN_SUBCFG[] struLoopPlanSubCfg;

			// Token: 0x0400131C RID: 4892
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000D7 RID: 215
		public struct NET_DVR_LOOPPLAN_SUBCFG
		{
			// Token: 0x0400131D RID: 4893
			public uint dwSize;

			// Token: 0x0400131E RID: 4894
			public uint dwPoolTime;

			// Token: 0x0400131F RID: 4895
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_MATRIX_CHAN_INFO_V30[] struChanConInfo;

			// Token: 0x04001320 RID: 4896
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000D8 RID: 216
		public struct NET_DVR_MATRIX_CHAN_INFO_V30
		{
			// Token: 0x04001321 RID: 4897
			public uint dwEnable;

			// Token: 0x04001322 RID: 4898
			public CHCNetSDK.NET_DVR_STREAM_MEDIA_SERVER_CFG streamMediaServerCfg;

			// Token: 0x04001323 RID: 4899
			public CHCNetSDK.tagDEV_CHAN_INFO struDevChanInfo;
		}

		// Token: 0x020000D9 RID: 217
		public struct NET_DVR_MATRIX_CHAN_STATUS
		{
			// Token: 0x04001324 RID: 4900
			public byte byDecodeStatus;

			// Token: 0x04001325 RID: 4901
			public byte byStreamType;

			// Token: 0x04001326 RID: 4902
			public byte byPacketType;

			// Token: 0x04001327 RID: 4903
			public byte byRecvBufUsage;

			// Token: 0x04001328 RID: 4904
			public byte byDecBufUsage;

			// Token: 0x04001329 RID: 4905
			public byte byFpsDecV;

			// Token: 0x0400132A RID: 4906
			public byte byFpsDecA;

			// Token: 0x0400132B RID: 4907
			public byte byCpuLoad;

			// Token: 0x0400132C RID: 4908
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x0400132D RID: 4909
			public uint dwDecodedV;

			// Token: 0x0400132E RID: 4910
			public uint dwDecodedA;

			// Token: 0x0400132F RID: 4911
			public ushort wImgW;

			// Token: 0x04001330 RID: 4912
			public ushort wImgH;

			// Token: 0x04001331 RID: 4913
			public byte byVideoFormat;

			// Token: 0x04001332 RID: 4914
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 27, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x020000DA RID: 218
		public struct NET_DVR_MATRIX_DEC_CHAN_INFO
		{
			// Token: 0x04001333 RID: 4915
			public uint dwSize;

			// Token: 0x04001334 RID: 4916
			public CHCNetSDK.NET_DVR_MATRIX_DECINFO struDecChanInfo;

			// Token: 0x04001335 RID: 4917
			public uint dwDecState;

			// Token: 0x04001336 RID: 4918
			public CHCNetSDK.NET_DVR_TIME StartTime;

			// Token: 0x04001337 RID: 4919
			public CHCNetSDK.NET_DVR_TIME StopTime;

			// Token: 0x04001338 RID: 4920
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string sFileName;
		}

		// Token: 0x020000DB RID: 219
		public struct NET_DVR_MATRIX_DEC_CHAN_STATUS
		{
			// Token: 0x04001339 RID: 4921
			public uint dwSize;

			// Token: 0x0400133A RID: 4922
			public uint dwIsLinked;

			// Token: 0x0400133B RID: 4923
			public uint dwStreamCpRate;

			// Token: 0x0400133C RID: 4924
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string cRes;
		}

		// Token: 0x020000DC RID: 220
		public struct NET_DVR_MATRIX_DEC_REMOTE_PLAY
		{
			// Token: 0x0400133D RID: 4925
			public uint dwSize;

			// Token: 0x0400133E RID: 4926
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sDVRIP;

			// Token: 0x0400133F RID: 4927
			public ushort wDVRPort;

			// Token: 0x04001340 RID: 4928
			public byte byChannel;

			// Token: 0x04001341 RID: 4929
			public byte byReserve;

			// Token: 0x04001342 RID: 4930
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x04001343 RID: 4931
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x04001344 RID: 4932
			public uint dwPlayMode;

			// Token: 0x04001345 RID: 4933
			public CHCNetSDK.NET_DVR_TIME StartTime;

			// Token: 0x04001346 RID: 4934
			public CHCNetSDK.NET_DVR_TIME StopTime;

			// Token: 0x04001347 RID: 4935
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string sFileName;
		}

		// Token: 0x020000DD RID: 221
		public struct NET_DVR_MATRIX_DEC_REMOTE_PLAY_CONTROL
		{
			// Token: 0x04001348 RID: 4936
			public uint dwSize;

			// Token: 0x04001349 RID: 4937
			public uint dwPlayCmd;

			// Token: 0x0400134A RID: 4938
			public uint dwCmdParam;
		}

		// Token: 0x020000DE RID: 222
		public struct NET_DVR_MATRIX_DEC_REMOTE_PLAY_STATUS
		{
			// Token: 0x0400134B RID: 4939
			public uint dwSize;

			// Token: 0x0400134C RID: 4940
			public uint dwCurMediaFileLen;

			// Token: 0x0400134D RID: 4941
			public uint dwCurMediaFilePosition;

			// Token: 0x0400134E RID: 4942
			public uint dwCurMediaFileDuration;

			// Token: 0x0400134F RID: 4943
			public uint dwCurPlayTime;

			// Token: 0x04001350 RID: 4944
			public uint dwCurMediaFIleFrames;

			// Token: 0x04001351 RID: 4945
			public uint dwCurDataType;

			// Token: 0x04001352 RID: 4946
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 72, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x020000DF RID: 223
		public struct NET_DVR_MATRIX_DECCHANINFO
		{
			// Token: 0x04001353 RID: 4947
			public uint dwEnable;

			// Token: 0x04001354 RID: 4948
			public CHCNetSDK.NET_DVR_MATRIX_DECINFO struDecChanInfo;
		}

		// Token: 0x020000E0 RID: 224
		public struct NET_DVR_MATRIX_DECINFO
		{
			// Token: 0x04001355 RID: 4949
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sDVRIP;

			// Token: 0x04001356 RID: 4950
			public ushort wDVRPort;

			// Token: 0x04001357 RID: 4951
			public byte byChannel;

			// Token: 0x04001358 RID: 4952
			public byte byTransProtocol;

			// Token: 0x04001359 RID: 4953
			public byte byTransMode;

			// Token: 0x0400135A RID: 4954
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400135B RID: 4955
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x0400135C RID: 4956
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;
		}

		// Token: 0x020000E1 RID: 225
		public struct NET_DVR_MATRIX_DYNAMIC_DEC
		{
			// Token: 0x0400135D RID: 4957
			public uint dwSize;

			// Token: 0x0400135E RID: 4958
			public CHCNetSDK.NET_DVR_MATRIX_DECINFO struDecChanInfo;
		}

		// Token: 0x020000E2 RID: 226
		public struct NET_DVR_MATRIX_LOOP_DECINFO
		{
			// Token: 0x0400135F RID: 4959
			public uint dwSize;

			// Token: 0x04001360 RID: 4960
			public uint dwPoolTime;

			// Token: 0x04001361 RID: 4961
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_MATRIX_DECCHANINFO[] struchanConInfo;
		}

		// Token: 0x020000E3 RID: 227
		public struct NET_DVR_MATRIX_TRAN_CHAN_CONFIG
		{
			// Token: 0x04001362 RID: 4962
			public uint dwSize;

			// Token: 0x04001363 RID: 4963
			public byte by232IsDualChan;

			// Token: 0x04001364 RID: 4964
			public byte by485IsDualChan;

			// Token: 0x04001365 RID: 4965
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] res;

			// Token: 0x04001366 RID: 4966
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_MATRIX_TRAN_CHAN_INFO[] struTranInfo;
		}

		// Token: 0x020000E4 RID: 228
		public struct NET_DVR_MATRIX_TRAN_CHAN_INFO
		{
			// Token: 0x04001367 RID: 4967
			public byte byTranChanEnable;

			// Token: 0x04001368 RID: 4968
			public byte byLocalSerialDevice;

			// Token: 0x04001369 RID: 4969
			public byte byRemoteSerialDevice;

			// Token: 0x0400136A RID: 4970
			public byte res1;

			// Token: 0x0400136B RID: 4971
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sRemoteDevIP;

			// Token: 0x0400136C RID: 4972
			public ushort wRemoteDevPort;

			// Token: 0x0400136D RID: 4973
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] res2;

			// Token: 0x0400136E RID: 4974
			public CHCNetSDK.TTY_CONFIG RemoteSerialDevCfg;
		}

		// Token: 0x020000E5 RID: 229
		public struct NET_DVR_MATRIXPARA
		{
			// Token: 0x0400136F RID: 4975
			public ushort wDisplayLogo;

			// Token: 0x04001370 RID: 4976
			public ushort wDisplayOsd;
		}

		// Token: 0x020000E6 RID: 230
		public struct NET_DVR_MATRIXPARA_V30
		{
			// Token: 0x04001371 RID: 4977
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U2)]
			public ushort[] wOrder;

			// Token: 0x04001372 RID: 4978
			public ushort wSwitchTime;

			// Token: 0x04001373 RID: 4979
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x020000E7 RID: 231
		public struct NET_DVR_MOTION
		{
			// Token: 0x04001374 RID: 4980
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 396, ArraySubType = UnmanagedType.I1)]
			public byte[] byMotionScope;

			// Token: 0x04001375 RID: 4981
			public byte byMotionSensitive;

			// Token: 0x04001376 RID: 4982
			public byte byEnableHandleMotion;

			// Token: 0x04001377 RID: 4983
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
			public string reservedData;

			// Token: 0x04001378 RID: 4984
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION strMotionHandleType;

			// Token: 0x04001379 RID: 4985
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmTime;

			// Token: 0x0400137A RID: 4986
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRelRecordChan;
		}

		// Token: 0x020000E8 RID: 232
		public struct NET_DVR_MOTION_V30
		{
			// Token: 0x0400137B RID: 4987
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6144, ArraySubType = UnmanagedType.I1)]
			public byte[] byMotionScope;

			// Token: 0x0400137C RID: 4988
			public byte byMotionSensitive;

			// Token: 0x0400137D RID: 4989
			public byte byEnableHandleMotion;

			// Token: 0x0400137E RID: 4990
			public byte byPrecision;

			// Token: 0x0400137F RID: 4991
			public byte reservedData;

			// Token: 0x04001380 RID: 4992
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION_V30 struMotionHandleType;

			// Token: 0x04001381 RID: 4993
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmTime;

			// Token: 0x04001382 RID: 4994
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byRelRecordChan;
		}

		// Token: 0x020000E9 RID: 233
		public struct NET_DVR_NETAPPCFG
		{
			// Token: 0x04001383 RID: 4995
			public uint dwSize;

			// Token: 0x04001384 RID: 4996
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sDNSIp;

			// Token: 0x04001385 RID: 4997
			public CHCNetSDK.NET_DVR_NTPPARA struNtpClientParam;

			// Token: 0x04001386 RID: 4998
			public CHCNetSDK.NET_DVR_DDNSPARA struDDNSClientParam;

			// Token: 0x04001387 RID: 4999
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 464, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x020000EA RID: 234
		public struct NET_DVR_NETCFG
		{
			// Token: 0x04001388 RID: 5000
			public uint dwSize;

			// Token: 0x04001389 RID: 5001
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_ETHERNET[] struEtherNet;

			// Token: 0x0400138A RID: 5002
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sManageHostIP;

			// Token: 0x0400138B RID: 5003
			public ushort wManageHostPort;

			// Token: 0x0400138C RID: 5004
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sIPServerIP;

			// Token: 0x0400138D RID: 5005
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sMultiCastIP;

			// Token: 0x0400138E RID: 5006
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sGatewayIP;

			// Token: 0x0400138F RID: 5007
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sNFSIP;

			// Token: 0x04001390 RID: 5008
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] sNFSDirectory;

			// Token: 0x04001391 RID: 5009
			public uint dwPPPOE;

			// Token: 0x04001392 RID: 5010
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sPPPoEUser;

			// Token: 0x04001393 RID: 5011
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sPPPoEPassword;

			// Token: 0x04001394 RID: 5012
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sPPPoEIP;

			// Token: 0x04001395 RID: 5013
			public ushort wHttpPort;
		}

		// Token: 0x020000EB RID: 235
		public struct NET_DVR_NETCFG_OTHER
		{
			// Token: 0x04001396 RID: 5014
			public uint dwSize;

			// Token: 0x04001397 RID: 5015
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sFirstDNSIP;

			// Token: 0x04001398 RID: 5016
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sSecondDNSIP;

			// Token: 0x04001399 RID: 5017
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sRes;
		}

		// Token: 0x020000EC RID: 236
		public struct NET_DVR_NETCFG_V30
		{
			// Token: 0x0400139A RID: 5018
			public uint dwSize;

			// Token: 0x0400139B RID: 5019
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_ETHERNET_V30[] struEtherNet;

			// Token: 0x0400139C RID: 5020
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPADDR[] struRes1;

			// Token: 0x0400139D RID: 5021
			public CHCNetSDK.NET_DVR_IPADDR struAlarmHostIpAddr;

			// Token: 0x0400139E RID: 5022
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U2)]
			public ushort[] wRes2;

			// Token: 0x0400139F RID: 5023
			public ushort wAlarmHostIpPort;

			// Token: 0x040013A0 RID: 5024
			public byte byUseDhcp;

			// Token: 0x040013A1 RID: 5025
			public byte byRes3;

			// Token: 0x040013A2 RID: 5026
			public CHCNetSDK.NET_DVR_IPADDR struDnsServer1IpAddr;

			// Token: 0x040013A3 RID: 5027
			public CHCNetSDK.NET_DVR_IPADDR struDnsServer2IpAddr;

			// Token: 0x040013A4 RID: 5028
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byIpResolver;

			// Token: 0x040013A5 RID: 5029
			public ushort wIpResolverPort;

			// Token: 0x040013A6 RID: 5030
			public ushort wHttpPortNo;

			// Token: 0x040013A7 RID: 5031
			public CHCNetSDK.NET_DVR_IPADDR struMulticastIpAddr;

			// Token: 0x040013A8 RID: 5032
			public CHCNetSDK.NET_DVR_IPADDR struGatewayIpAddr;

			// Token: 0x040013A9 RID: 5033
			public CHCNetSDK.NET_DVR_PPPOECFG struPPPoE;

			// Token: 0x040013AA RID: 5034
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000ED RID: 237
		public struct NET_DVR_NFSCFG
		{
			// Token: 0x0600090F RID: 2319 RVA: 0x000DBA1C File Offset: 0x000DAA1C
			public void Init()
			{
				this.struNfsDiskParam = new CHCNetSDK.NET_DVR_SINGLE_NFS[8];
				for (int i = 0; i < 8; i++)
				{
					this.struNfsDiskParam[i].Init();
				}
			}

			// Token: 0x040013AB RID: 5035
			public uint dwSize;

			// Token: 0x040013AC RID: 5036
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SINGLE_NFS[] struNfsDiskParam;
		}

		// Token: 0x020000EE RID: 238
		public struct NET_DVR_NTPPARA
		{
			// Token: 0x040013AD RID: 5037
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sNTPServer;

			// Token: 0x040013AE RID: 5038
			public ushort wInterval;

			// Token: 0x040013AF RID: 5039
			public byte byEnableNTP;

			// Token: 0x040013B0 RID: 5040
			public byte cTimeDifferenceH;

			// Token: 0x040013B1 RID: 5041
			public byte cTimeDifferenceM;

			// Token: 0x040013B2 RID: 5042
			public byte res1;

			// Token: 0x040013B3 RID: 5043
			public ushort wNtpPort;

			// Token: 0x040013B4 RID: 5044
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] res2;
		}

		// Token: 0x020000EF RID: 239
		public struct NET_DVR_PICCFG
		{
			// Token: 0x040013B5 RID: 5045
			public uint dwSize;

			// Token: 0x040013B6 RID: 5046
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sChanName;

			// Token: 0x040013B7 RID: 5047
			public uint dwVideoFormat;

			// Token: 0x040013B8 RID: 5048
			public byte byBrightness;

			// Token: 0x040013B9 RID: 5049
			public byte byContrast;

			// Token: 0x040013BA RID: 5050
			public byte bySaturation;

			// Token: 0x040013BB RID: 5051
			public byte byHue;

			// Token: 0x040013BC RID: 5052
			public uint dwShowChanName;

			// Token: 0x040013BD RID: 5053
			public ushort wShowNameTopLeftX;

			// Token: 0x040013BE RID: 5054
			public ushort wShowNameTopLeftY;

			// Token: 0x040013BF RID: 5055
			public CHCNetSDK.NET_DVR_VILOST struVILost;

			// Token: 0x040013C0 RID: 5056
			public CHCNetSDK.NET_DVR_MOTION struMotion;

			// Token: 0x040013C1 RID: 5057
			public CHCNetSDK.NET_DVR_HIDEALARM struHideAlarm;

			// Token: 0x040013C2 RID: 5058
			public uint dwEnableHide;

			// Token: 0x040013C3 RID: 5059
			public ushort wHideAreaTopLeftX;

			// Token: 0x040013C4 RID: 5060
			public ushort wHideAreaTopLeftY;

			// Token: 0x040013C5 RID: 5061
			public ushort wHideAreaWidth;

			// Token: 0x040013C6 RID: 5062
			public ushort wHideAreaHeight;

			// Token: 0x040013C7 RID: 5063
			public uint dwShowOsd;

			// Token: 0x040013C8 RID: 5064
			public ushort wOSDTopLeftX;

			// Token: 0x040013C9 RID: 5065
			public ushort wOSDTopLeftY;

			// Token: 0x040013CA RID: 5066
			public byte byOSDType;

			// Token: 0x040013CB RID: 5067
			public byte byDispWeek;

			// Token: 0x040013CC RID: 5068
			public byte byOSDAttrib;

			// Token: 0x040013CD RID: 5069
			public byte reservedData2;
		}

		// Token: 0x020000F0 RID: 240
		public struct NET_DVR_PICCFG_EX
		{
			// Token: 0x040013CE RID: 5070
			public uint dwSize;

			// Token: 0x040013CF RID: 5071
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sChanName;

			// Token: 0x040013D0 RID: 5072
			public uint dwVideoFormat;

			// Token: 0x040013D1 RID: 5073
			public byte byBrightness;

			// Token: 0x040013D2 RID: 5074
			public byte byContrast;

			// Token: 0x040013D3 RID: 5075
			public byte bySaturation;

			// Token: 0x040013D4 RID: 5076
			public byte byHue;

			// Token: 0x040013D5 RID: 5077
			public uint dwShowChanName;

			// Token: 0x040013D6 RID: 5078
			public ushort wShowNameTopLeftX;

			// Token: 0x040013D7 RID: 5079
			public ushort wShowNameTopLeftY;

			// Token: 0x040013D8 RID: 5080
			public CHCNetSDK.NET_DVR_VILOST struVILost;

			// Token: 0x040013D9 RID: 5081
			public CHCNetSDK.NET_DVR_MOTION struMotion;

			// Token: 0x040013DA RID: 5082
			public CHCNetSDK.NET_DVR_HIDEALARM struHideAlarm;

			// Token: 0x040013DB RID: 5083
			public uint dwEnableHide;

			// Token: 0x040013DC RID: 5084
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SHELTER[] struShelter;

			// Token: 0x040013DD RID: 5085
			public uint dwShowOsd;

			// Token: 0x040013DE RID: 5086
			public ushort wOSDTopLeftX;

			// Token: 0x040013DF RID: 5087
			public ushort wOSDTopLeftY;

			// Token: 0x040013E0 RID: 5088
			public byte byOSDType;

			// Token: 0x040013E1 RID: 5089
			public byte byDispWeek;

			// Token: 0x040013E2 RID: 5090
			public byte byOSDAttrib;

			// Token: 0x040013E3 RID: 5091
			public byte byHourOsdType;
		}

		// Token: 0x020000F1 RID: 241
		public struct NET_DVR_PICCFG_V30
		{
			// Token: 0x040013E4 RID: 5092
			public uint dwSize;

			// Token: 0x040013E5 RID: 5093
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sChanName;

			// Token: 0x040013E6 RID: 5094
			public uint dwVideoFormat;

			// Token: 0x040013E7 RID: 5095
			public CHCNetSDK.NET_DVR_COLOR struColor;

			// Token: 0x040013E8 RID: 5096
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 60)]
			public string reservedData;

			// Token: 0x040013E9 RID: 5097
			public uint dwShowChanName;

			// Token: 0x040013EA RID: 5098
			public ushort wShowNameTopLeftX;

			// Token: 0x040013EB RID: 5099
			public ushort wShowNameTopLeftY;

			// Token: 0x040013EC RID: 5100
			public CHCNetSDK.NET_DVR_VILOST_V30 struVILost;

			// Token: 0x040013ED RID: 5101
			public CHCNetSDK.NET_DVR_VILOST_V30 struRes;

			// Token: 0x040013EE RID: 5102
			public CHCNetSDK.NET_DVR_MOTION_V30 struMotion;

			// Token: 0x040013EF RID: 5103
			public CHCNetSDK.NET_DVR_HIDEALARM_V30 struHideAlarm;

			// Token: 0x040013F0 RID: 5104
			public uint dwEnableHide;

			// Token: 0x040013F1 RID: 5105
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SHELTER[] struShelter;

			// Token: 0x040013F2 RID: 5106
			public uint dwShowOsd;

			// Token: 0x040013F3 RID: 5107
			public ushort wOSDTopLeftX;

			// Token: 0x040013F4 RID: 5108
			public ushort wOSDTopLeftY;

			// Token: 0x040013F5 RID: 5109
			public byte byOSDType;

			// Token: 0x040013F6 RID: 5110
			public byte byDispWeek;

			// Token: 0x040013F7 RID: 5111
			public byte byOSDAttrib;

			// Token: 0x040013F8 RID: 5112
			public byte byHourOSDType;

			// Token: 0x040013F9 RID: 5113
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000F2 RID: 242
		public struct NET_DVR_PLANDECODE
		{
			// Token: 0x040013FA RID: 5114
			public uint dwSize;

			// Token: 0x040013FB RID: 5115
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_DECODESCHED[] struDecodeSched;

			// Token: 0x040013FC RID: 5116
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byres;
		}

		// Token: 0x020000F3 RID: 243
		public struct NET_DVR_PLAYREMOTEFILE
		{
			// Token: 0x040013FD RID: 5117
			public uint dwSize;

			// Token: 0x040013FE RID: 5118
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sDecoderIP;

			// Token: 0x040013FF RID: 5119
			public ushort wDecoderPort;

			// Token: 0x04001400 RID: 5120
			public ushort wLoadMode;

			// Token: 0x020000F4 RID: 244
			[StructLayout(LayoutKind.Explicit)]
			public struct mode_size
			{
				// Token: 0x04001401 RID: 5121
				[FieldOffset(0)]
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.I1)]
				public byte[] byFile;

				// Token: 0x020000F5 RID: 245
				public struct bytime
				{
					// Token: 0x04001402 RID: 5122
					public uint dwChannel;

					// Token: 0x04001403 RID: 5123
					[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
					public byte[] sUserName;

					// Token: 0x04001404 RID: 5124
					[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
					public byte[] sPassword;

					// Token: 0x04001405 RID: 5125
					public CHCNetSDK.NET_DVR_TIME struStartTime;

					// Token: 0x04001406 RID: 5126
					public CHCNetSDK.NET_DVR_TIME struStopTime;
				}
			}
		}

		// Token: 0x020000F6 RID: 246
		public struct NET_DVR_POINT_FRAME
		{
			// Token: 0x04001407 RID: 5127
			public int xTop;

			// Token: 0x04001408 RID: 5128
			public int yTop;

			// Token: 0x04001409 RID: 5129
			public int xBottom;

			// Token: 0x0400140A RID: 5130
			public int yBottom;

			// Token: 0x0400140B RID: 5131
			public int bCounter;
		}

		// Token: 0x020000F7 RID: 247
		public struct NET_DVR_PORTCFG
		{
			// Token: 0x0400140C RID: 5132
			public uint dwSize;

			// Token: 0x0400140D RID: 5133
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_PORTINFO[] struTransPortInfo;
		}

		// Token: 0x020000F8 RID: 248
		public struct NET_DVR_PORTINFO
		{
			// Token: 0x0400140E RID: 5134
			public uint dwEnableTransPort;

			// Token: 0x0400140F RID: 5135
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sDecoderIP;

			// Token: 0x04001410 RID: 5136
			public ushort wDecoderPort;

			// Token: 0x04001411 RID: 5137
			public ushort wDVRTransPort;

			// Token: 0x04001412 RID: 5138
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
			public string cReserve;
		}

		// Token: 0x020000F9 RID: 249
		public struct NET_DVR_PPPCFG
		{
			// Token: 0x04001413 RID: 5139
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sRemoteIP;

			// Token: 0x04001414 RID: 5140
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sLocalIP;

			// Token: 0x04001415 RID: 5141
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sLocalIPMask;

			// Token: 0x04001416 RID: 5142
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUsername;

			// Token: 0x04001417 RID: 5143
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x04001418 RID: 5144
			public byte byPPPMode;

			// Token: 0x04001419 RID: 5145
			public byte byRedial;

			// Token: 0x0400141A RID: 5146
			public byte byRedialMode;

			// Token: 0x0400141B RID: 5147
			public byte byDataEncrypt;

			// Token: 0x0400141C RID: 5148
			public uint dwMTU;

			// Token: 0x0400141D RID: 5149
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sTelephoneNumber;
		}

		// Token: 0x020000FA RID: 250
		public struct NET_DVR_PPPCFG_V30
		{
			// Token: 0x0400141E RID: 5150
			public CHCNetSDK.NET_DVR_IPADDR struRemoteIP;

			// Token: 0x0400141F RID: 5151
			public CHCNetSDK.NET_DVR_IPADDR struLocalIP;

			// Token: 0x04001420 RID: 5152
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sLocalIPMask;

			// Token: 0x04001421 RID: 5153
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUsername;

			// Token: 0x04001422 RID: 5154
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x04001423 RID: 5155
			public byte byPPPMode;

			// Token: 0x04001424 RID: 5156
			public byte byRedial;

			// Token: 0x04001425 RID: 5157
			public byte byRedialMode;

			// Token: 0x04001426 RID: 5158
			public byte byDataEncrypt;

			// Token: 0x04001427 RID: 5159
			public uint dwMTU;

			// Token: 0x04001428 RID: 5160
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sTelephoneNumber;
		}

		// Token: 0x020000FB RID: 251
		public struct NET_DVR_PPPOECFG
		{
			// Token: 0x04001429 RID: 5161
			public uint dwPPPOE;

			// Token: 0x0400142A RID: 5162
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sPPPoEUser;

			// Token: 0x0400142B RID: 5163
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sPPPoEPassword;

			// Token: 0x0400142C RID: 5164
			public CHCNetSDK.NET_DVR_IPADDR struPPPoEIP;
		}

		// Token: 0x020000FC RID: 252
		public struct NET_DVR_PREVIEWCFG
		{
			// Token: 0x0400142D RID: 5165
			public uint dwSize;

			// Token: 0x0400142E RID: 5166
			public byte byPreviewNumber;

			// Token: 0x0400142F RID: 5167
			public byte byEnableAudio;

			// Token: 0x04001430 RID: 5168
			public ushort wSwitchTime;

			// Token: 0x04001431 RID: 5169
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] bySwitchSeq;
		}

		// Token: 0x020000FD RID: 253
		public struct NET_DVR_PREVIEWCFG_V30
		{
			// Token: 0x04001432 RID: 5170
			public uint dwSize;

			// Token: 0x04001433 RID: 5171
			public byte byPreviewNumber;

			// Token: 0x04001434 RID: 5172
			public byte byEnableAudio;

			// Token: 0x04001435 RID: 5173
			public ushort wSwitchTime;

			// Token: 0x04001436 RID: 5174
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
			public byte[] bySwitchSeq;

			// Token: 0x04001437 RID: 5175
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020000FE RID: 254
		public struct NET_DVR_PTZ_PROTOCOL
		{
			// Token: 0x04001438 RID: 5176
			public uint dwType;

			// Token: 0x04001439 RID: 5177
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byDescribe;
		}

		// Token: 0x020000FF RID: 255
		public struct NET_DVR_PTZCFG
		{
			// Token: 0x0400143A RID: 5178
			public uint dwSize;

			// Token: 0x0400143B RID: 5179
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 200, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_PTZ_PROTOCOL[] struPtz;

			// Token: 0x0400143C RID: 5180
			public uint dwPtzNum;

			// Token: 0x0400143D RID: 5181
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000100 RID: 256
		public struct NET_DVR_PTZPOS
		{
			// Token: 0x0400143E RID: 5182
			public ushort wAction;

			// Token: 0x0400143F RID: 5183
			public ushort wPanPos;

			// Token: 0x04001440 RID: 5184
			public ushort wTiltPos;

			// Token: 0x04001441 RID: 5185
			public ushort wZoomPos;
		}

		// Token: 0x02000101 RID: 257
		public struct NET_DVR_PTZSCOPE
		{
			// Token: 0x04001442 RID: 5186
			public ushort wPanPosMin;

			// Token: 0x04001443 RID: 5187
			public ushort wPanPosMax;

			// Token: 0x04001444 RID: 5188
			public ushort wTiltPosMin;

			// Token: 0x04001445 RID: 5189
			public ushort wTiltPosMax;

			// Token: 0x04001446 RID: 5190
			public ushort wZoomPosMin;

			// Token: 0x04001447 RID: 5191
			public ushort wZoomPosMax;
		}

		// Token: 0x02000102 RID: 258
		public struct NET_DVR_PU_STREAM_CFG
		{
			// Token: 0x04001448 RID: 5192
			public uint dwSize;

			// Token: 0x04001449 RID: 5193
			public CHCNetSDK.NET_DVR_STREAM_MEDIA_SERVER_CFG struStreamMediaSvrCfg;

			// Token: 0x0400144A RID: 5194
			public CHCNetSDK.tagDEV_CHAN_INFO struDevChanInfo;
		}

		// Token: 0x02000103 RID: 259
		public struct NET_DVR_RECORD
		{
			// Token: 0x0400144B RID: 5195
			public uint dwSize;

			// Token: 0x0400144C RID: 5196
			public uint dwRecord;

			// Token: 0x0400144D RID: 5197
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_RECORDDAY[] struRecAllDay;

			// Token: 0x0400144E RID: 5198
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_RECORDSCHED[] struRecordSched;

			// Token: 0x0400144F RID: 5199
			public uint dwRecordTime;

			// Token: 0x04001450 RID: 5200
			public uint dwPreRecordTime;
		}

		// Token: 0x02000104 RID: 260
		public struct NET_DVR_RECORD_V30
		{
			// Token: 0x04001451 RID: 5201
			public uint dwSize;

			// Token: 0x04001452 RID: 5202
			public uint dwRecord;

			// Token: 0x04001453 RID: 5203
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_RECORDDAY[] struRecAllDay;

			// Token: 0x04001454 RID: 5204
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_RECORDSCHED[] struRecordSched;

			// Token: 0x04001455 RID: 5205
			public uint dwRecordTime;

			// Token: 0x04001456 RID: 5206
			public uint dwPreRecordTime;

			// Token: 0x04001457 RID: 5207
			public uint dwRecorderDuration;

			// Token: 0x04001458 RID: 5208
			public byte byRedundancyRec;

			// Token: 0x04001459 RID: 5209
			public byte byAudioRec;

			// Token: 0x0400145A RID: 5210
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
			public byte[] byReserve;
		}

		// Token: 0x02000105 RID: 261
		public struct NET_DVR_RECORDDAY
		{
			// Token: 0x0400145B RID: 5211
			public ushort wAllDayRecord;

			// Token: 0x0400145C RID: 5212
			public byte byRecordType;

			// Token: 0x0400145D RID: 5213
			public byte reservedData;
		}

		// Token: 0x02000106 RID: 262
		public struct NET_DVR_RECORDSCHED
		{
			// Token: 0x0400145E RID: 5214
			public CHCNetSDK.NET_DVR_SCHEDTIME struRecordTime;

			// Token: 0x0400145F RID: 5215
			public byte byRecordType;

			// Token: 0x04001460 RID: 5216
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
			public string reservedData;
		}

		// Token: 0x02000107 RID: 263
		public struct NET_DVR_RS232CFG
		{
			// Token: 0x04001461 RID: 5217
			public uint dwSize;

			// Token: 0x04001462 RID: 5218
			public uint dwBaudRate;

			// Token: 0x04001463 RID: 5219
			public byte byDataBit;

			// Token: 0x04001464 RID: 5220
			public byte byStopBit;

			// Token: 0x04001465 RID: 5221
			public byte byParity;

			// Token: 0x04001466 RID: 5222
			public byte byFlowcontrol;

			// Token: 0x04001467 RID: 5223
			public uint dwWorkMode;

			// Token: 0x04001468 RID: 5224
			public CHCNetSDK.NET_DVR_PPPCFG struPPPConfig;
		}

		// Token: 0x02000108 RID: 264
		public struct NET_DVR_RS232CFG_V30
		{
			// Token: 0x04001469 RID: 5225
			public uint dwSize;

			// Token: 0x0400146A RID: 5226
			public CHCNetSDK.NET_DVR_SINGLE_RS232 struRs232;

			// Token: 0x0400146B RID: 5227
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 84, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400146C RID: 5228
			public CHCNetSDK.NET_DVR_PPPCFG_V30 struPPPConfig;
		}

		// Token: 0x02000109 RID: 265
		public struct NET_DVR_RTSPCFG
		{
			// Token: 0x0400146D RID: 5229
			public uint dwSize;

			// Token: 0x0400146E RID: 5230
			public ushort wPort;

			// Token: 0x0400146F RID: 5231
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 54, ArraySubType = UnmanagedType.I1)]
			public byte[] byReserve;
		}

		// Token: 0x0200010A RID: 266
		public struct NET_DVR_SCALECFG
		{
			// Token: 0x04001470 RID: 5232
			public uint dwSize;

			// Token: 0x04001471 RID: 5233
			public uint dwMajorScale;

			// Token: 0x04001472 RID: 5234
			public uint dwMinorScale;

			// Token: 0x04001473 RID: 5235
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
			public uint[] dwRes;
		}

		// Token: 0x0200010B RID: 267
		public struct NET_DVR_SCHEDTIME
		{
			// Token: 0x04001474 RID: 5236
			public byte byStartHour;

			// Token: 0x04001475 RID: 5237
			public byte byStartMin;

			// Token: 0x04001476 RID: 5238
			public byte byStopHour;

			// Token: 0x04001477 RID: 5239
			public byte byStopMin;
		}

		// Token: 0x0200010C RID: 268
		public struct NET_DVR_SDKABL
		{
			// Token: 0x04001478 RID: 5240
			public uint dwMaxLoginNum;

			// Token: 0x04001479 RID: 5241
			public uint dwMaxRealPlayNum;

			// Token: 0x0400147A RID: 5242
			public uint dwMaxPlayBackNum;

			// Token: 0x0400147B RID: 5243
			public uint dwMaxAlarmChanNum;

			// Token: 0x0400147C RID: 5244
			public uint dwMaxFormatNum;

			// Token: 0x0400147D RID: 5245
			public uint dwMaxFileSearchNum;

			// Token: 0x0400147E RID: 5246
			public uint dwMaxLogSearchNum;

			// Token: 0x0400147F RID: 5247
			public uint dwMaxSerialNum;

			// Token: 0x04001480 RID: 5248
			public uint dwMaxUpgradeNum;

			// Token: 0x04001481 RID: 5249
			public uint dwMaxVoiceComNum;

			// Token: 0x04001482 RID: 5250
			public uint dwMaxBroadCastNum;

			// Token: 0x04001483 RID: 5251
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.U4)]
			public uint[] dwRes;
		}

		// Token: 0x0200010D RID: 269
		public struct NET_DVR_SDKSTATE
		{
			// Token: 0x04001484 RID: 5252
			public uint dwTotalLoginNum;

			// Token: 0x04001485 RID: 5253
			public uint dwTotalRealPlayNum;

			// Token: 0x04001486 RID: 5254
			public uint dwTotalPlayBackNum;

			// Token: 0x04001487 RID: 5255
			public uint dwTotalAlarmChanNum;

			// Token: 0x04001488 RID: 5256
			public uint dwTotalFormatNum;

			// Token: 0x04001489 RID: 5257
			public uint dwTotalFileSearchNum;

			// Token: 0x0400148A RID: 5258
			public uint dwTotalLogSearchNum;

			// Token: 0x0400148B RID: 5259
			public uint dwTotalSerialNum;

			// Token: 0x0400148C RID: 5260
			public uint dwTotalUpgradeNum;

			// Token: 0x0400148D RID: 5261
			public uint dwTotalVoiceComNum;

			// Token: 0x0400148E RID: 5262
			public uint dwTotalBroadCastNum;

			// Token: 0x0400148F RID: 5263
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.U4)]
			public uint[] dwRes;
		}

		// Token: 0x0200010E RID: 270
		public struct NET_DVR_SHELTER
		{
			// Token: 0x04001490 RID: 5264
			public ushort wHideAreaTopLeftX;

			// Token: 0x04001491 RID: 5265
			public ushort wHideAreaTopLeftY;

			// Token: 0x04001492 RID: 5266
			public ushort wHideAreaWidth;

			// Token: 0x04001493 RID: 5267
			public ushort wHideAreaHeight;
		}

		// Token: 0x0200010F RID: 271
		public struct NET_DVR_SHOWSTRING
		{
			// Token: 0x04001494 RID: 5268
			public uint dwSize;

			// Token: 0x04001495 RID: 5269
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SHOWSTRINGINFO[] struStringInfo;
		}

		// Token: 0x02000110 RID: 272
		public struct NET_DVR_SHOWSTRING_EX
		{
			// Token: 0x04001496 RID: 5270
			public uint dwSize;

			// Token: 0x04001497 RID: 5271
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SHOWSTRINGINFO[] struStringInfo;
		}

		// Token: 0x02000111 RID: 273
		public struct NET_DVR_SHOWSTRING_V30
		{
			// Token: 0x04001498 RID: 5272
			public uint dwSize;

			// Token: 0x04001499 RID: 5273
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SHOWSTRINGINFO[] struStringInfo;
		}

		// Token: 0x02000112 RID: 274
		public struct NET_DVR_SHOWSTRINGINFO
		{
			// Token: 0x0400149A RID: 5274
			public ushort wShowString;

			// Token: 0x0400149B RID: 5275
			public ushort wStringSize;

			// Token: 0x0400149C RID: 5276
			public ushort wShowStringTopLeftX;

			// Token: 0x0400149D RID: 5277
			public ushort wShowStringTopLeftY;

			// Token: 0x0400149E RID: 5278
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 44)]
			public string sString;
		}

		// Token: 0x02000113 RID: 275
		public struct NET_DVR_SINGLE_HD
		{
			// Token: 0x0400149F RID: 5279
			public uint dwHDNo;

			// Token: 0x040014A0 RID: 5280
			public uint dwCapacity;

			// Token: 0x040014A1 RID: 5281
			public uint dwFreeSpace;

			// Token: 0x040014A2 RID: 5282
			public uint dwHdStatus;

			// Token: 0x040014A3 RID: 5283
			public byte byHDAttr;

			// Token: 0x040014A4 RID: 5284
			public byte byHDType;

			// Token: 0x040014A5 RID: 5285
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040014A6 RID: 5286
			public uint dwHdGroup;

			// Token: 0x040014A7 RID: 5287
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x02000114 RID: 276
		public struct NET_DVR_SINGLE_HDGROUP
		{
			// Token: 0x040014A8 RID: 5288
			public uint dwHDGroupNo;

			// Token: 0x040014A9 RID: 5289
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byHDGroupChans;

			// Token: 0x040014AA RID: 5290
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000115 RID: 277
		public struct NET_DVR_SINGLE_NFS
		{
			// Token: 0x06000910 RID: 2320 RVA: 0x000DBA52 File Offset: 0x000DAA52
			public void Init()
			{
				this.sNfsDirectory = new byte[128];
			}

			// Token: 0x040014AB RID: 5291
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sNfsHostIPAddr;

			// Token: 0x040014AC RID: 5292
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] sNfsDirectory;
		}

		// Token: 0x02000116 RID: 278
		public struct NET_DVR_SINGLE_RS232
		{
			// Token: 0x040014AD RID: 5293
			public uint dwBaudRate;

			// Token: 0x040014AE RID: 5294
			public byte byDataBit;

			// Token: 0x040014AF RID: 5295
			public byte byStopBit;

			// Token: 0x040014B0 RID: 5296
			public byte byParity;

			// Token: 0x040014B1 RID: 5297
			public byte byFlowcontrol;

			// Token: 0x040014B2 RID: 5298
			public uint dwWorkMode;
		}

		// Token: 0x02000117 RID: 279
		public struct NET_DVR_STREAM_MEDIA_SERVER_CFG
		{
			// Token: 0x040014B3 RID: 5299
			public byte byValid;

			// Token: 0x040014B4 RID: 5300
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040014B5 RID: 5301
			public CHCNetSDK.NET_DVR_IPADDR struDevIP;

			// Token: 0x040014B6 RID: 5302
			public ushort wDevPort;

			// Token: 0x040014B7 RID: 5303
			public byte byTransmitType;

			// Token: 0x040014B8 RID: 5304
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 69, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x02000118 RID: 280
		public struct NET_DVR_SUBSYSTEMINFO
		{
			// Token: 0x040014B9 RID: 5305
			public byte bySubSystemType;

			// Token: 0x040014BA RID: 5306
			public byte byChan;

			// Token: 0x040014BB RID: 5307
			public byte byLoginType;

			// Token: 0x040014BC RID: 5308
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040014BD RID: 5309
			public CHCNetSDK.NET_DVR_IPADDR struSubSystemIP;

			// Token: 0x040014BE RID: 5310
			public ushort wSubSystemPort;

			// Token: 0x040014BF RID: 5311
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x040014C0 RID: 5312
			public CHCNetSDK.NET_DVR_IPADDR struSubSystemIPMask;

			// Token: 0x040014C1 RID: 5313
			public CHCNetSDK.NET_DVR_IPADDR struGatewayIpAddr;

			// Token: 0x040014C2 RID: 5314
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x040014C3 RID: 5315
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x040014C4 RID: 5316
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string sDomainName;

			// Token: 0x040014C5 RID: 5317
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string sDnsAddress;

			// Token: 0x040014C6 RID: 5318
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] sSerialNumber;
		}

		// Token: 0x02000119 RID: 281
		public struct NET_DVR_TIME
		{
			// Token: 0x040014C7 RID: 5319
			public uint dwYear;

			// Token: 0x040014C8 RID: 5320
			public uint dwMonth;

			// Token: 0x040014C9 RID: 5321
			public uint dwDay;

			// Token: 0x040014CA RID: 5322
			public uint dwHour;

			// Token: 0x040014CB RID: 5323
			public uint dwMinute;

			// Token: 0x040014CC RID: 5324
			public uint dwSecond;
		}

		// Token: 0x0200011A RID: 282
		public struct NET_DVR_TIMEPOINT
		{
			// Token: 0x040014CD RID: 5325
			public uint dwMonth;

			// Token: 0x040014CE RID: 5326
			public uint dwWeekNo;

			// Token: 0x040014CF RID: 5327
			public uint dwWeekDate;

			// Token: 0x040014D0 RID: 5328
			public uint dwHour;

			// Token: 0x040014D1 RID: 5329
			public uint dwMin;
		}

		// Token: 0x0200011B RID: 283
		public struct NET_DVR_TRADEINFO
		{
			// Token: 0x040014D2 RID: 5330
			public ushort m_Year;

			// Token: 0x040014D3 RID: 5331
			public ushort m_Month;

			// Token: 0x040014D4 RID: 5332
			public ushort m_Day;

			// Token: 0x040014D5 RID: 5333
			public ushort m_Hour;

			// Token: 0x040014D6 RID: 5334
			public ushort m_Minute;

			// Token: 0x040014D7 RID: 5335
			public ushort m_Second;

			// Token: 0x040014D8 RID: 5336
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
			public byte[] DeviceName;

			// Token: 0x040014D9 RID: 5337
			public uint dwChannelNumer;

			// Token: 0x040014DA RID: 5338
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] CardNumber;

			// Token: 0x040014DB RID: 5339
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
			public string cTradeType;

			// Token: 0x040014DC RID: 5340
			public uint dwCash;
		}

		// Token: 0x0200011C RID: 284
		public struct NET_DVR_USER
		{
			// Token: 0x040014DD RID: 5341
			public uint dwSize;

			// Token: 0x040014DE RID: 5342
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_USER_INFO[] struUser;
		}

		// Token: 0x0200011D RID: 285
		public struct NET_DVR_USER_EX
		{
			// Token: 0x040014DF RID: 5343
			public uint dwSize;

			// Token: 0x040014E0 RID: 5344
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_USER_INFO_EX[] struUser;
		}

		// Token: 0x0200011E RID: 286
		public struct NET_DVR_USER_INFO
		{
			// Token: 0x040014E1 RID: 5345
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x040014E2 RID: 5346
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x040014E3 RID: 5347
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U4)]
			public uint[] dwLocalRight;

			// Token: 0x040014E4 RID: 5348
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U4)]
			public uint[] dwRemoteRight;

			// Token: 0x040014E5 RID: 5349
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sUserIP;

			// Token: 0x040014E6 RID: 5350
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMACAddr;
		}

		// Token: 0x0200011F RID: 287
		public struct NET_DVR_USER_INFO_EX
		{
			// Token: 0x040014E7 RID: 5351
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x040014E8 RID: 5352
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x040014E9 RID: 5353
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U4)]
			public uint[] dwLocalRight;

			// Token: 0x040014EA RID: 5354
			public uint dwLocalPlaybackRight;

			// Token: 0x040014EB RID: 5355
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U4)]
			public uint[] dwRemoteRight;

			// Token: 0x040014EC RID: 5356
			public uint dwNetPreviewRight;

			// Token: 0x040014ED RID: 5357
			public uint dwNetPlaybackRight;

			// Token: 0x040014EE RID: 5358
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sUserIP;

			// Token: 0x040014EF RID: 5359
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMACAddr;
		}

		// Token: 0x02000120 RID: 288
		public struct NET_DVR_USER_INFO_V30
		{
			// Token: 0x040014F0 RID: 5360
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x040014F1 RID: 5361
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x040014F2 RID: 5362
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byLocalRight;

			// Token: 0x040014F3 RID: 5363
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRemoteRight;

			// Token: 0x040014F4 RID: 5364
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byNetPreviewRight;

			// Token: 0x040014F5 RID: 5365
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byLocalPlaybackRight;

			// Token: 0x040014F6 RID: 5366
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byNetPlaybackRight;

			// Token: 0x040014F7 RID: 5367
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byLocalRecordRight;

			// Token: 0x040014F8 RID: 5368
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byNetRecordRight;

			// Token: 0x040014F9 RID: 5369
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byLocalPTZRight;

			// Token: 0x040014FA RID: 5370
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byNetPTZRight;

			// Token: 0x040014FB RID: 5371
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byLocalBackupRight;

			// Token: 0x040014FC RID: 5372
			public CHCNetSDK.NET_DVR_IPADDR struUserIP;

			// Token: 0x040014FD RID: 5373
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMACAddr;

			// Token: 0x040014FE RID: 5374
			public byte byPriority;

			// Token: 0x040014FF RID: 5375
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000121 RID: 289
		public struct NET_DVR_USER_V30
		{
			// Token: 0x04001500 RID: 5376
			public uint dwSize;

			// Token: 0x04001501 RID: 5377
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_USER_INFO_V30[] struUser;
		}

		// Token: 0x02000122 RID: 290
		public struct NET_DVR_VGAPARA
		{
			// Token: 0x04001502 RID: 5378
			public ushort wResolution;

			// Token: 0x04001503 RID: 5379
			public ushort wFreq;

			// Token: 0x04001504 RID: 5380
			public uint dwBrightness;
		}

		// Token: 0x02000123 RID: 291
		public struct NET_DVR_VIDEOOUT
		{
			// Token: 0x04001505 RID: 5381
			public uint dwSize;

			// Token: 0x04001506 RID: 5382
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_VOOUT[] struVOOut;

			// Token: 0x04001507 RID: 5383
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_VGAPARA[] struVGAPara;

			// Token: 0x04001508 RID: 5384
			public CHCNetSDK.NET_DVR_MATRIXPARA struMatrixPara;
		}

		// Token: 0x02000124 RID: 292
		public struct NET_DVR_VIDEOOUT_V30
		{
			// Token: 0x04001509 RID: 5385
			public uint dwSize;

			// Token: 0x0400150A RID: 5386
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_VOOUT[] struVOOut;

			// Token: 0x0400150B RID: 5387
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_VGAPARA[] struVGAPara;

			// Token: 0x0400150C RID: 5388
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_MATRIXPARA_V30[] struMatrixPara;

			// Token: 0x0400150D RID: 5389
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000125 RID: 293
		public struct NET_DVR_VIDEOPLATFORM_ABILITY
		{
			// Token: 0x0400150E RID: 5390
			public uint dwSize;

			// Token: 0x0400150F RID: 5391
			public byte byCodeSubSystemNums;

			// Token: 0x04001510 RID: 5392
			public byte byDecodeSubSystemNums;

			// Token: 0x04001511 RID: 5393
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
			public byte[] byWindowMode;

			// Token: 0x04001512 RID: 5394
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000126 RID: 294
		public struct NET_DVR_VILOST
		{
			// Token: 0x04001513 RID: 5395
			public byte byEnableHandleVILost;

			// Token: 0x04001514 RID: 5396
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION strVILostHandleType;

			// Token: 0x04001515 RID: 5397
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmTime;
		}

		// Token: 0x02000127 RID: 295
		public struct NET_DVR_VILOST_V30
		{
			// Token: 0x04001516 RID: 5398
			public byte byEnableHandleVILost;

			// Token: 0x04001517 RID: 5399
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION_V30 strVILostHandleType;

			// Token: 0x04001518 RID: 5400
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmTime;
		}

		// Token: 0x02000128 RID: 296
		public struct NET_DVR_VOOUT
		{
			// Token: 0x04001519 RID: 5401
			public byte byVideoFormat;

			// Token: 0x0400151A RID: 5402
			public byte byMenuAlphaValue;

			// Token: 0x0400151B RID: 5403
			public ushort wScreenSaveTime;

			// Token: 0x0400151C RID: 5404
			public ushort wVOffset;

			// Token: 0x0400151D RID: 5405
			public ushort wBrightness;

			// Token: 0x0400151E RID: 5406
			public byte byStartMode;

			// Token: 0x0400151F RID: 5407
			public byte byEnableScaler;
		}

		// Token: 0x02000129 RID: 297
		public struct NET_DVR_WORKSTATE
		{
			// Token: 0x06000911 RID: 2321 RVA: 0x000DBA64 File Offset: 0x000DAA64
			public void Init()
			{
				this.struHardDiskStatic = new CHCNetSDK.NET_DVR_DISKSTATE[16];
				this.struChanStatic = new CHCNetSDK.NET_DVR_CHANNELSTATE[16];
				this.byAlarmInStatic = new byte[16];
				this.byAlarmOutStatic = new byte[4];
			}

			// Token: 0x04001520 RID: 5408
			public uint dwDeviceStatic;

			// Token: 0x04001521 RID: 5409
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_DISKSTATE[] struHardDiskStatic;

			// Token: 0x04001522 RID: 5410
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_CHANNELSTATE[] struChanStatic;

			// Token: 0x04001523 RID: 5411
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmInStatic;

			// Token: 0x04001524 RID: 5412
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmOutStatic;

			// Token: 0x04001525 RID: 5413
			public uint dwLocalDisplay;
		}

		// Token: 0x0200012A RID: 298
		public struct NET_DVR_WORKSTATE_V30
		{
			// Token: 0x06000912 RID: 2322 RVA: 0x000DBA9C File Offset: 0x000DAA9C
			public void Init()
			{
				this.struHardDiskStatic = new CHCNetSDK.NET_DVR_DISKSTATE[33];
				this.struChanStatic = new CHCNetSDK.NET_DVR_CHANNELSTATE_V30[64];
				for (int i = 0; i < 64; i++)
				{
					this.struChanStatic[i].Init();
				}
				this.byAlarmInStatic = new byte[96];
				this.byAlarmOutStatic = new byte[96];
				this.byAudioChanStatus = new byte[2];
				this.byRes = new byte[10];
			}

			// Token: 0x04001526 RID: 5414
			public uint dwDeviceStatic;

			// Token: 0x04001527 RID: 5415
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_DISKSTATE[] struHardDiskStatic;

			// Token: 0x04001528 RID: 5416
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_CHANNELSTATE_V30[] struChanStatic;

			// Token: 0x04001529 RID: 5417
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 160, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmInStatic;

			// Token: 0x0400152A RID: 5418
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmOutStatic;

			// Token: 0x0400152B RID: 5419
			public uint dwLocalDisplay;

			// Token: 0x0400152C RID: 5420
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byAudioChanStatus;

			// Token: 0x0400152D RID: 5421
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200012B RID: 299
		public struct NET_DVR_ZONEANDDST
		{
			// Token: 0x0400152E RID: 5422
			public uint dwSize;

			// Token: 0x0400152F RID: 5423
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04001530 RID: 5424
			public uint dwEnableDST;

			// Token: 0x04001531 RID: 5425
			public byte byDSTBias;

			// Token: 0x04001532 RID: 5426
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x04001533 RID: 5427
			public CHCNetSDK.NET_DVR_TIMEPOINT struBeginPoint;

			// Token: 0x04001534 RID: 5428
			public CHCNetSDK.NET_DVR_TIMEPOINT struEndPoint;
		}

		// Token: 0x0200012C RID: 300
		public struct NET_SDK_CLIENTINFO
		{
			// Token: 0x04001535 RID: 5429
			public int lChannel;

			// Token: 0x04001536 RID: 5430
			public int lLinkType;

			// Token: 0x04001537 RID: 5431
			public int lLinkMode;

			// Token: 0x04001538 RID: 5432
			public IntPtr hPlayWnd;

			// Token: 0x04001539 RID: 5433
			public string sMultiCastIP;

			// Token: 0x0400153A RID: 5434
			public int iMediaSrvNum;

			// Token: 0x0400153B RID: 5435
			public IntPtr pMediaSrvDir;
		}

		// Token: 0x0200012D RID: 301
		public struct PACKET_INFO
		{
			// Token: 0x0400153C RID: 5436
			public int nPacketType;

			// Token: 0x0400153D RID: 5437
			public IntPtr pPacketBuffer;

			// Token: 0x0400153E RID: 5438
			public uint dwPacketSize;

			// Token: 0x0400153F RID: 5439
			public int nYear;

			// Token: 0x04001540 RID: 5440
			public int nMonth;

			// Token: 0x04001541 RID: 5441
			public int nDay;

			// Token: 0x04001542 RID: 5442
			public int nHour;

			// Token: 0x04001543 RID: 5443
			public int nMinute;

			// Token: 0x04001544 RID: 5444
			public int nSecond;

			// Token: 0x04001545 RID: 5445
			public uint dwTimeStamp;
		}

		// Token: 0x0200012E RID: 302
		public struct PLAY_INFO
		{
			// Token: 0x04001546 RID: 5446
			public int iUserID;

			// Token: 0x04001547 RID: 5447
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
			public string strDeviceIP;

			// Token: 0x04001548 RID: 5448
			public int iDevicePort;

			// Token: 0x04001549 RID: 5449
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string strDevAdmin;

			// Token: 0x0400154A RID: 5450
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string strDevPsd;

			// Token: 0x0400154B RID: 5451
			public int iChannel;

			// Token: 0x0400154C RID: 5452
			public int iLinkMode;

			// Token: 0x0400154D RID: 5453
			public bool bUseMedia;

			// Token: 0x0400154E RID: 5454
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
			public string strMediaIP;

			// Token: 0x0400154F RID: 5455
			public int iMediaPort;
		}

		// Token: 0x0200012F RID: 303
		// (Invoke) Token: 0x06000914 RID: 2324
		public delegate void PLAYDATACALLBACK(int lPlayHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, uint dwUser);

		// Token: 0x02000130 RID: 304
		// (Invoke) Token: 0x06000918 RID: 2328
		public delegate void REALDATACALLBACK(int lRealHandle, uint dwDataType, ref byte pBuffer, uint dwBufSize, IntPtr pUser);

		// Token: 0x02000131 RID: 305
		public enum REALSOUND_MODE
		{
			// Token: 0x04001551 RID: 5457
			MONOPOLIZE_MODE = 1,
			// Token: 0x04001552 RID: 5458
			SHARE_MODE
		}

		// Token: 0x02000132 RID: 306
		public enum SDK_NETWORK_ENVIRONMENT
		{
			// Token: 0x04001554 RID: 5460
			LOCAL_AREA_NETWORK,
			// Token: 0x04001555 RID: 5461
			WIDE_AREA_NETWORK
		}

		// Token: 0x02000133 RID: 307
		public struct SECTIONLIST
		{
			// Token: 0x04001556 RID: 5462
			public CHCNetSDK.BLOCKTIME startTime;

			// Token: 0x04001557 RID: 5463
			public CHCNetSDK.BLOCKTIME stopTime;

			// Token: 0x04001558 RID: 5464
			public byte byRecType;

			// Token: 0x04001559 RID: 5465
			public IntPtr pNext;
		}

		// Token: 0x02000134 RID: 308
		public enum SEND_MODE
		{
			// Token: 0x0400155B RID: 5467
			PTOPTCPMODE,
			// Token: 0x0400155C RID: 5468
			PTOPUDPMODE,
			// Token: 0x0400155D RID: 5469
			MULTIMODE,
			// Token: 0x0400155E RID: 5470
			RTPMODE,
			// Token: 0x0400155F RID: 5471
			RESERVEDMODE
		}

		// Token: 0x02000135 RID: 309
		// (Invoke) Token: 0x0600091C RID: 2332
		public delegate void SERIALDATACALLBACK(int lSerialHandle, string pRecvDataBuffer, uint dwBufSize, uint dwUser);

		// Token: 0x02000136 RID: 310
		// (Invoke) Token: 0x06000920 RID: 2336
		public delegate void SETREALDATACALLBACK(int lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, uint dwUser);

		// Token: 0x02000137 RID: 311
		public enum SIZE_FILTER_MODE
		{
			// Token: 0x04001561 RID: 5473
			IMAGE_PIX_MODE,
			// Token: 0x04001562 RID: 5474
			REAL_WORLD_MODE
		}

		// Token: 0x02000138 RID: 312
		// (Invoke) Token: 0x06000924 RID: 2340
		public delegate void STDDATACALLBACK(int lRealHandle, uint dwDataType, ref byte pBuffer, uint dwBufSize, uint dwUser);

		// Token: 0x02000139 RID: 313
		public struct STOREINFO
		{
			// Token: 0x04001563 RID: 5475
			public int iMaxChannels;

			// Token: 0x04001564 RID: 5476
			public int iDiskGroup;

			// Token: 0x04001565 RID: 5477
			public int iStreamType;

			// Token: 0x04001566 RID: 5478
			public bool bAnalyze;

			// Token: 0x04001567 RID: 5479
			public bool bCycWrite;

			// Token: 0x04001568 RID: 5480
			public uint uiFileSize;

			// Token: 0x04001569 RID: 5481
			public CHCNetSDK.CALLBACKFUN_MESSAGE funCallback;
		}

		// Token: 0x0200013A RID: 314
		public struct struAlarmParam
		{
			// Token: 0x06000927 RID: 2343 RVA: 0x000DBB14 File Offset: 0x000DAB14
			public void init()
			{
				this.byAlarmInNo = new byte[160];
				this.byRes = new byte[236];
			}

			// Token: 0x0400156A RID: 5482
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 160, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmInNo;

			// Token: 0x0400156B RID: 5483
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 140, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200013B RID: 315
		public struct struAlarmRet
		{
			// Token: 0x06000928 RID: 2344 RVA: 0x000DBB36 File Offset: 0x000DAB36
			public void init()
			{
				this.byRes = new byte[300];
			}

			// Token: 0x0400156C RID: 5484
			public uint dwAlarmInNo;

			// Token: 0x0400156D RID: 5485
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 300, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200013C RID: 316
		public struct struDDNS
		{
			// Token: 0x0400156E RID: 5486
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUsername;

			// Token: 0x0400156F RID: 5487
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x04001570 RID: 5488
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sDomainName;

			// Token: 0x04001571 RID: 5489
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] sServerName;

			// Token: 0x04001572 RID: 5490
			public ushort wDDNSPort;

			// Token: 0x04001573 RID: 5491
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200013D RID: 317
		public struct struMotionParam
		{
			// Token: 0x06000929 RID: 2345 RVA: 0x000DBB48 File Offset: 0x000DAB48
			public void init()
			{
				this.byMotDetChanNo = new byte[64];
				this.byRes = new byte[236];
			}

			// Token: 0x04001574 RID: 5492
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byMotDetChanNo;

			// Token: 0x04001575 RID: 5493
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 236, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200013E RID: 318
		public struct struMotionRet
		{
			// Token: 0x0600092A RID: 2346 RVA: 0x000DBB67 File Offset: 0x000DAB67
			public void init()
			{
				this.byRes = new byte[300];
			}

			// Token: 0x04001576 RID: 5494
			public uint dwMotDetNo;

			// Token: 0x04001577 RID: 5495
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 300, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200013F RID: 319
		public struct struReceiver
		{
			// Token: 0x04001578 RID: 5496
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sName;

			// Token: 0x04001579 RID: 5497
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sAddress;
		}

		// Token: 0x02000140 RID: 320
		public struct struVcaParam
		{
			// Token: 0x0600092B RID: 2347 RVA: 0x000DBB79 File Offset: 0x000DAB79
			public void init()
			{
				this.byChanNo = new byte[64];
				this.byRes1 = new byte[43];
			}

			// Token: 0x0400157A RID: 5498
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byChanNo;

			// Token: 0x0400157B RID: 5499
			public byte byRuleID;

			// Token: 0x0400157C RID: 5500
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 43, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;
		}

		// Token: 0x02000141 RID: 321
		public struct struVcaRet
		{
			// Token: 0x0600092C RID: 2348 RVA: 0x000DBB95 File Offset: 0x000DAB95
			public void init()
			{
				this.byRes1 = new byte[3];
				this.byRuleName = new byte[32];
			}

			// Token: 0x0400157D RID: 5501
			public uint dwChanNo;

			// Token: 0x0400157E RID: 5502
			public byte byRuleID;

			// Token: 0x0400157F RID: 5503
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04001580 RID: 5504
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRuleName;

			// Token: 0x04001581 RID: 5505
			public CHCNetSDK.tagNET_VCA_EVENT_UNION uEvent;
		}

		// Token: 0x02000142 RID: 322
		public struct tagCAMERAPARAMCFG
		{
			// Token: 0x04001582 RID: 5506
			public uint dwSize;

			// Token: 0x04001583 RID: 5507
			public uint dwPowerLineFrequencyMode;

			// Token: 0x04001584 RID: 5508
			public uint dwWhiteBalanceMode;

			// Token: 0x04001585 RID: 5509
			public uint dwWhiteBalanceModeRGain;

			// Token: 0x04001586 RID: 5510
			public uint dwWhiteBalanceModeBGain;

			// Token: 0x04001587 RID: 5511
			public uint dwExposureMode;

			// Token: 0x04001588 RID: 5512
			public uint dwExposureSet;

			// Token: 0x04001589 RID: 5513
			public uint dwExposureUserSet;

			// Token: 0x0400158A RID: 5514
			public uint dwExposureTarget;

			// Token: 0x0400158B RID: 5515
			public uint dwIrisMode;

			// Token: 0x0400158C RID: 5516
			public uint dwGainLevel;

			// Token: 0x0400158D RID: 5517
			public uint dwBrightnessLevel;

			// Token: 0x0400158E RID: 5518
			public uint dwContrastLevel;

			// Token: 0x0400158F RID: 5519
			public uint dwSharpnessLevel;

			// Token: 0x04001590 RID: 5520
			public uint dwSaturationLevel;

			// Token: 0x04001591 RID: 5521
			public uint dwHueLevel;

			// Token: 0x04001592 RID: 5522
			public uint dwGammaCorrectionEnabled;

			// Token: 0x04001593 RID: 5523
			public uint dwGammaCorrectionLevel;

			// Token: 0x04001594 RID: 5524
			public uint dwWDREnabled;

			// Token: 0x04001595 RID: 5525
			public uint dwWDRLevel1;

			// Token: 0x04001596 RID: 5526
			public uint dwWDRLevel2;

			// Token: 0x04001597 RID: 5527
			public uint dwWDRContrastLevel;

			// Token: 0x04001598 RID: 5528
			public uint dwDayNightFilterType;

			// Token: 0x04001599 RID: 5529
			public uint dwSwitchScheduleEnabled;

			// Token: 0x0400159A RID: 5530
			public uint dwBeginTime;

			// Token: 0x0400159B RID: 5531
			public uint dwEndTime;

			// Token: 0x0400159C RID: 5532
			public uint dwDayToNightFilterLevel;

			// Token: 0x0400159D RID: 5533
			public uint dwNightToDayFilterLevel;

			// Token: 0x0400159E RID: 5534
			public uint dwDayNightFilterTime;

			// Token: 0x0400159F RID: 5535
			public uint dwBacklightMode;

			// Token: 0x040015A0 RID: 5536
			public uint dwPositionX1;

			// Token: 0x040015A1 RID: 5537
			public uint dwPositionY1;

			// Token: 0x040015A2 RID: 5538
			public uint dwPositionX2;

			// Token: 0x040015A3 RID: 5539
			public uint dwPositionY2;

			// Token: 0x040015A4 RID: 5540
			public uint dwBacklightLevel;

			// Token: 0x040015A5 RID: 5541
			public uint dwDigitalNoiseRemoveEnable;

			// Token: 0x040015A6 RID: 5542
			public uint dwDigitalNoiseRemoveLevel;

			// Token: 0x040015A7 RID: 5543
			public uint dwMirror;

			// Token: 0x040015A8 RID: 5544
			public uint dwDigitalZoom;

			// Token: 0x040015A9 RID: 5545
			public uint dwDeadPixelDetect;

			// Token: 0x040015AA RID: 5546
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.U4)]
			public uint[] dwRes;
		}

		// Token: 0x02000143 RID: 323
		public struct tagDEC_MATRIX_CHAN_INFO
		{
			// Token: 0x040015AB RID: 5547
			public uint dwSize;

			// Token: 0x040015AC RID: 5548
			public CHCNetSDK.NET_DVR_STREAM_MEDIA_SERVER_CFG streamMediaServerCfg;

			// Token: 0x040015AD RID: 5549
			public CHCNetSDK.tagDEV_CHAN_INFO struDevChanInfo;

			// Token: 0x040015AE RID: 5550
			public uint dwDecState;

			// Token: 0x040015AF RID: 5551
			public CHCNetSDK.NET_DVR_TIME StartTime;

			// Token: 0x040015B0 RID: 5552
			public CHCNetSDK.NET_DVR_TIME StopTime;

			// Token: 0x040015B1 RID: 5553
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string sFileName;

			// Token: 0x040015B2 RID: 5554
			public uint dwGetStreamMode;

			// Token: 0x040015B3 RID: 5555
			public CHCNetSDK.tagNET_MATRIX_PASSIVEMODE struPassiveMode;
		}

		// Token: 0x02000144 RID: 324
		public struct tagDEV_CHAN_INFO
		{
			// Token: 0x040015B4 RID: 5556
			public CHCNetSDK.NET_DVR_IPADDR struIP;

			// Token: 0x040015B5 RID: 5557
			public ushort wDVRPort;

			// Token: 0x040015B6 RID: 5558
			public byte byChannel;

			// Token: 0x040015B7 RID: 5559
			public byte byTransProtocol;

			// Token: 0x040015B8 RID: 5560
			public byte byTransMode;

			// Token: 0x040015B9 RID: 5561
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 71, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x040015BA RID: 5562
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x040015BB RID: 5563
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;
		}

		// Token: 0x02000145 RID: 325
		public struct tagIMAGEPARAM
		{
			// Token: 0x040015BC RID: 5564
			public uint dwSize;

			// Token: 0x040015BD RID: 5565
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagIMAGESUBPARAM[] struImageParamSched;

			// Token: 0x040015BE RID: 5566
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000146 RID: 326
		public struct tagIMAGEREGION
		{
			// Token: 0x040015BF RID: 5567
			public uint dwSize;

			// Token: 0x040015C0 RID: 5568
			public ushort wImageRegionTopLeftX;

			// Token: 0x040015C1 RID: 5569
			public ushort wImageRegionTopLeftY;

			// Token: 0x040015C2 RID: 5570
			public ushort wImageRegionWidth;

			// Token: 0x040015C3 RID: 5571
			public ushort wImageRegionHeight;

			// Token: 0x040015C4 RID: 5572
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000147 RID: 327
		public struct tagIMAGESUBPARAM
		{
			// Token: 0x040015C5 RID: 5573
			public CHCNetSDK.NET_DVR_SCHEDTIME struImageStatusTime;

			// Token: 0x040015C6 RID: 5574
			public byte byImageEnhancementLevel;

			// Token: 0x040015C7 RID: 5575
			public byte byImageDenoiseLevel;

			// Token: 0x040015C8 RID: 5576
			public byte byImageStableEnable;

			// Token: 0x040015C9 RID: 5577
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000148 RID: 328
		public struct tagMATRIX_LOOP_DECINFO_V30
		{
			// Token: 0x040015CA RID: 5578
			public uint dwSize;

			// Token: 0x040015CB RID: 5579
			public uint dwPoolTime;

			// Token: 0x040015CC RID: 5580
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_MATRIX_CHAN_INFO_V30[] struchanConInfo;

			// Token: 0x040015CD RID: 5581
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000149 RID: 329
		public struct tagMATRIX_TRAN_CHAN_CONFIG
		{
			// Token: 0x040015CE RID: 5582
			public uint dwSize;

			// Token: 0x040015CF RID: 5583
			public byte by232IsDualChan;

			// Token: 0x040015D0 RID: 5584
			public byte by485IsDualChan;

			// Token: 0x040015D1 RID: 5585
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] vyRes;

			// Token: 0x040015D2 RID: 5586
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagMATRIX_TRAN_CHAN_INFO[] struTranInfo;
		}

		// Token: 0x0200014A RID: 330
		public struct tagMATRIX_TRAN_CHAN_INFO
		{
			// Token: 0x040015D3 RID: 5587
			public byte byTranChanEnable;

			// Token: 0x040015D4 RID: 5588
			public byte byLocalSerialDevice;

			// Token: 0x040015D5 RID: 5589
			public byte byRemoteSerialDevice;

			// Token: 0x040015D6 RID: 5590
			public byte byRes1;

			// Token: 0x040015D7 RID: 5591
			public CHCNetSDK.NET_DVR_IPADDR struRemoteDevIP;

			// Token: 0x040015D8 RID: 5592
			public ushort wRemoteDevPort;

			// Token: 0x040015D9 RID: 5593
			public byte byIsEstablished;

			// Token: 0x040015DA RID: 5594
			public byte byRes2;

			// Token: 0x040015DB RID: 5595
			public CHCNetSDK.TTY_CONFIG RemoteSerialDevCfg;

			// Token: 0x040015DC RID: 5596
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byUsername;

			// Token: 0x040015DD RID: 5597
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byPassword;

			// Token: 0x040015DE RID: 5598
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;
		}

		// Token: 0x0200014B RID: 331
		public struct tagNET_DVR__DECODER_WORK_STATUS
		{
			// Token: 0x040015DF RID: 5599
			public uint dwSize;

			// Token: 0x040015E0 RID: 5600
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_MATRIX_CHAN_STATUS[] struDecChanStatus;

			// Token: 0x040015E1 RID: 5601
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_DISP_CHAN_STATUS[] struDispChanStatus;

			// Token: 0x040015E2 RID: 5602
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byAlarmInStatus;

			// Token: 0x040015E3 RID: 5603
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byAalarmOutStatus;

			// Token: 0x040015E4 RID: 5604
			public byte byAudioInChanStatus;

			// Token: 0x040015E5 RID: 5605
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 127, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200014C RID: 332
		public struct tagNET_DVR__WIFI_CFG_EX
		{
			// Token: 0x040015E6 RID: 5606
			public CHCNetSDK.tagNET_DVR_WIFIETHERNET struEtherNet;

			// Token: 0x040015E7 RID: 5607
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sEssid;

			// Token: 0x040015E8 RID: 5608
			public uint dwMode;

			// Token: 0x040015E9 RID: 5609
			public uint dwSecurity;

			// Token: 0x0200014D RID: 333
			[StructLayout(LayoutKind.Explicit)]
			public struct key
			{
				// Token: 0x0200014E RID: 334
				public struct wep
				{
					// Token: 0x040015EA RID: 5610
					public uint dwAuthentication;

					// Token: 0x040015EB RID: 5611
					public uint dwKeyLength;

					// Token: 0x040015EC RID: 5612
					public uint dwKeyType;

					// Token: 0x040015ED RID: 5613
					public uint dwActive;

					// Token: 0x040015EE RID: 5614
					[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
					public string sKeyInfo;
				}

				// Token: 0x0200014F RID: 335
				public struct wpa_psk
				{
					// Token: 0x040015EF RID: 5615
					public uint dwKeyLength;

					// Token: 0x040015F0 RID: 5616
					[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 63)]
					public string sKeyInfo;

					// Token: 0x040015F1 RID: 5617
					public byte sRes;
				}
			}
		}

		// Token: 0x02000150 RID: 336
		public struct tagNET_DVR_AP_INFO
		{
			// Token: 0x040015F2 RID: 5618
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string sSsid;

			// Token: 0x040015F3 RID: 5619
			public uint dwMode;

			// Token: 0x040015F4 RID: 5620
			public uint dwSecurity;

			// Token: 0x040015F5 RID: 5621
			public uint dwChannel;

			// Token: 0x040015F6 RID: 5622
			public uint dwSignalStrength;

			// Token: 0x040015F7 RID: 5623
			public uint dwSpeed;
		}

		// Token: 0x02000151 RID: 337
		public struct tagNET_DVR_AP_INFO_LIST
		{
			// Token: 0x040015F8 RID: 5624
			public uint dwSize;

			// Token: 0x040015F9 RID: 5625
			public uint dwCount;

			// Token: 0x040015FA RID: 5626
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_DVR_AP_INFO[] struApInfo;
		}

		// Token: 0x02000152 RID: 338
		public struct tagNET_DVR_ATM_FRAMETYPE_NEW
		{
			// Token: 0x040015FB RID: 5627
			public byte byEnable;

			// Token: 0x040015FC RID: 5628
			public byte byInputMode;

			// Token: 0x040015FD RID: 5629
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040015FE RID: 5630
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byAtmName;

			// Token: 0x040015FF RID: 5631
			public CHCNetSDK.NET_DVR_IPADDR struAtmIp;

			// Token: 0x04001600 RID: 5632
			public ushort wAtmPort;

			// Token: 0x04001601 RID: 5633
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x04001602 RID: 5634
			public uint dwAtmType;

			// Token: 0x04001603 RID: 5635
			public CHCNetSDK.tagNET_DVR_IDENTIFICAT struIdentification;

			// Token: 0x04001604 RID: 5636
			public CHCNetSDK.tagNET_DVR_FILTER struFilter;

			// Token: 0x04001605 RID: 5637
			public CHCNetSDK.tagNET_DVR_ATM_PACKAGE_OTHERS struCardNoPara;

			// Token: 0x04001606 RID: 5638
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_DVR_ATM_PACKAGE_ACTION[] struTradeActionPara;

			// Token: 0x04001607 RID: 5639
			public CHCNetSDK.tagNET_DVR_ATM_PACKAGE_OTHERS struAmountPara;

			// Token: 0x04001608 RID: 5640
			public CHCNetSDK.tagNET_DVR_ATM_PACKAGE_OTHERS struSerialNoPara;

			// Token: 0x04001609 RID: 5641
			public CHCNetSDK.tagNET_DVR_OVERLAY_CHANNEL struOverlayChan;

			// Token: 0x0400160A RID: 5642
			public CHCNetSDK.tagNET_DVR_ATM_PACKAGE_DATE byRes4;

			// Token: 0x0400160B RID: 5643
			public CHCNetSDK.tagNET_DVR_ATM_PACKAGE_TIME byRes5;

			// Token: 0x0400160C RID: 5644
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 132, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;
		}

		// Token: 0x02000153 RID: 339
		public struct tagNET_DVR_ATM_PACKAGE_ACTION
		{
			// Token: 0x0400160D RID: 5645
			public CHCNetSDK.tagNET_DVR_PACKAGE_LOCATION struPackageLocation;

			// Token: 0x0400160E RID: 5646
			public CHCNetSDK.tagNET_DVR_OSD_POSITION struOsdPosition;

			// Token: 0x0400160F RID: 5647
			public CHCNetSDK.NET_DVR_FRAMETYPECODE struActionCode;

			// Token: 0x04001610 RID: 5648
			public CHCNetSDK.NET_DVR_FRAMETYPECODE struPreCode;

			// Token: 0x04001611 RID: 5649
			public byte byActionCodeMode;

			// Token: 0x04001612 RID: 5650
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000154 RID: 340
		public struct tagNET_DVR_ATM_PACKAGE_DATE
		{
			// Token: 0x04001613 RID: 5651
			public CHCNetSDK.tagNET_DVR_PACKAGE_LOCATION struPackageLocation;

			// Token: 0x04001614 RID: 5652
			public CHCNetSDK.tagNET_DVR_DATE_FORMAT struDateForm;

			// Token: 0x04001615 RID: 5653
			public CHCNetSDK.tagNET_DVR_OSD_POSITION struOsdPosition;

			// Token: 0x04001616 RID: 5654
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x02000155 RID: 341
		public struct tagNET_DVR_ATM_PACKAGE_OTHERS
		{
			// Token: 0x04001617 RID: 5655
			public CHCNetSDK.tagNET_DVR_PACKAGE_LOCATION struPackageLocation;

			// Token: 0x04001618 RID: 5656
			public CHCNetSDK.tagNET_DVR_PACKAGE_LENGTH struPackageLength;

			// Token: 0x04001619 RID: 5657
			public CHCNetSDK.tagNET_DVR_OSD_POSITION struOsdPosition;

			// Token: 0x0400161A RID: 5658
			public CHCNetSDK.NET_DVR_FRAMETYPECODE struPreCode;

			// Token: 0x0400161B RID: 5659
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x02000156 RID: 342
		public struct tagNET_DVR_ATM_PACKAGE_TIME
		{
			// Token: 0x0400161C RID: 5660
			public CHCNetSDK.tagNET_DVR_PACKAGE_LOCATION location;

			// Token: 0x0400161D RID: 5661
			public CHCNetSDK.tagNET_DVRT_TIME_FORMAT struTimeForm;

			// Token: 0x0400161E RID: 5662
			public CHCNetSDK.tagNET_DVR_OSD_POSITION struOsdPosition;

			// Token: 0x0400161F RID: 5663
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000157 RID: 343
		public struct tagNET_DVR_ATM_PROTOCOL
		{
			// Token: 0x04001620 RID: 5664
			public uint dwSize;

			// Token: 0x04001621 RID: 5665
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_DVR_ATM_PROTOIDX[] struAtmProtoidx;

			// Token: 0x04001622 RID: 5666
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
			public uint[] dwAtmNumPerSort;
		}

		// Token: 0x02000158 RID: 344
		public struct tagNET_DVR_ATM_PROTOIDX
		{
			// Token: 0x04001623 RID: 5667
			public uint dwAtmType;

			// Token: 0x04001624 RID: 5668
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string chDesc;
		}

		// Token: 0x02000159 RID: 345
		public struct tagNET_DVR_CB_POINT
		{
			// Token: 0x04001625 RID: 5669
			public CHCNetSDK.tagNET_VCA_POINT struPoint;

			// Token: 0x04001626 RID: 5670
			public CHCNetSDK.NET_DVR_PTZPOS struPtzPos;

			// Token: 0x04001627 RID: 5671
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200015A RID: 346
		public struct tagNET_DVR_CCD_CFG
		{
			// Token: 0x04001628 RID: 5672
			public uint dwSize;

			// Token: 0x04001629 RID: 5673
			public byte byBlc;

			// Token: 0x0400162A RID: 5674
			public byte byBlcMode;

			// Token: 0x0400162B RID: 5675
			public byte byAwb;

			// Token: 0x0400162C RID: 5676
			public byte byAgc;

			// Token: 0x0400162D RID: 5677
			public byte byDayNight;

			// Token: 0x0400162E RID: 5678
			public byte byMirror;

			// Token: 0x0400162F RID: 5679
			public byte byShutter;

			// Token: 0x04001630 RID: 5680
			public byte byIrCutTime;

			// Token: 0x04001631 RID: 5681
			public byte byLensType;

			// Token: 0x04001632 RID: 5682
			public byte byEnVideoTrig;

			// Token: 0x04001633 RID: 5683
			public byte byCapShutter;

			// Token: 0x04001634 RID: 5684
			public byte byEnRecognise;
		}

		// Token: 0x0200015B RID: 347
		public struct tagNET_DVR_COMPRESSION_AUDIO
		{
			// Token: 0x04001635 RID: 5685
			public byte byAudioEncType;

			// Token: 0x04001636 RID: 5686
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
			public byte[] byres;
		}

		// Token: 0x0200015C RID: 348
		public struct tagNET_DVR_DATE_FORMAT
		{
			// Token: 0x04001637 RID: 5687
			public byte byItem1;

			// Token: 0x04001638 RID: 5688
			public byte byItem2;

			// Token: 0x04001639 RID: 5689
			public byte byItem3;

			// Token: 0x0400163A RID: 5690
			public byte byDateForm;

			// Token: 0x0400163B RID: 5691
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400163C RID: 5692
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
			public string chSeprator;

			// Token: 0x0400163D RID: 5693
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
			public string chDisplaySeprator;

			// Token: 0x0400163E RID: 5694
			public byte byDisplayForm;

			// Token: 0x0400163F RID: 5695
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 27, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x0200015D RID: 349
		public struct tagNET_DVR_DISP_LOGOCFG
		{
			// Token: 0x04001640 RID: 5696
			public uint dwCorordinateX;

			// Token: 0x04001641 RID: 5697
			public uint dwCorordinateY;

			// Token: 0x04001642 RID: 5698
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04001643 RID: 5699
			public byte byFlash;

			// Token: 0x04001644 RID: 5700
			public byte byTranslucent;

			// Token: 0x04001645 RID: 5701
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x04001646 RID: 5702
			public uint dwLogoSize;
		}

		// Token: 0x0200015E RID: 350
		public struct tagNET_DVR_FILTER
		{
			// Token: 0x04001647 RID: 5703
			public byte byEnable;

			// Token: 0x04001648 RID: 5704
			public byte byMode;

			// Token: 0x04001649 RID: 5705
			public byte byFrameBeginPos;

			// Token: 0x0400164A RID: 5706
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400164B RID: 5707
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byFilterText;

			// Token: 0x0400164C RID: 5708
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200015F RID: 351
		public struct tagNET_DVR_FRAMEFORMAT_V31
		{
			// Token: 0x0400164D RID: 5709
			public uint dwSize;

			// Token: 0x0400164E RID: 5710
			public CHCNetSDK.tagNET_DVR_ATM_FRAMETYPE_NEW struAtmFrameTypeNew;

			// Token: 0x0400164F RID: 5711
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_DVR_ATM_FRAMETYPE_NEW[] byRes;
		}

		// Token: 0x02000160 RID: 352
		public struct tagNET_DVR_IDENTIFICAT
		{
			// Token: 0x04001650 RID: 5712
			public byte byStartMode;

			// Token: 0x04001651 RID: 5713
			public byte byEndMode;

			// Token: 0x04001652 RID: 5714
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x04001653 RID: 5715
			public CHCNetSDK.NET_DVR_FRAMETYPECODE struStartCode;

			// Token: 0x04001654 RID: 5716
			public CHCNetSDK.NET_DVR_FRAMETYPECODE struEndCode;

			// Token: 0x04001655 RID: 5717
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;
		}

		// Token: 0x02000161 RID: 353
		public struct tagNET_DVR_IPALARMINFO
		{
			// Token: 0x04001656 RID: 5718
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPDEVINFO[] struIPDevInfo;

			// Token: 0x04001657 RID: 5719
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byAnalogChanEnable;

			// Token: 0x04001658 RID: 5720
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPCHANINFO[] struIPChanInfo;

			// Token: 0x04001659 RID: 5721
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPALARMININFO[] struIPAlarmInInfo;

			// Token: 0x0400165A RID: 5722
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPALARMOUTINFO[] struIPAlarmOutInfo;
		}

		// Token: 0x02000162 RID: 354
		public struct tagNET_DVR_IPALARMINFO_V31
		{
			// Token: 0x0400165B RID: 5723
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_DVR_IPDEVINFO_V31[] struIPDevInfo;

			// Token: 0x0400165C RID: 5724
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byAnalogChanEnable;

			// Token: 0x0400165D RID: 5725
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPCHANINFO[] struIPChanInfo;

			// Token: 0x0400165E RID: 5726
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPALARMININFO[] struIPAlarmInInfo;

			// Token: 0x0400165F RID: 5727
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_IPALARMOUTINFO[] struIPAlarmOutInfo;
		}

		// Token: 0x02000163 RID: 355
		public struct tagNET_DVR_IPDEVINFO_V31
		{
			// Token: 0x0600092D RID: 2349 RVA: 0x000DBBB0 File Offset: 0x000DABB0
			public void Init()
			{
				this.byRes1 = new byte[3];
				this.sUserName = new byte[32];
				this.sPassword = new byte[16];
				this.byDomain = new byte[64];
				this.byRes2 = new byte[34];
			}

			// Token: 0x04001660 RID: 5728
			public byte byEnable;

			// Token: 0x04001661 RID: 5729
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04001662 RID: 5730
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sUserName;

			// Token: 0x04001663 RID: 5731
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] sPassword;

			// Token: 0x04001664 RID: 5732
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byDomain;

			// Token: 0x04001665 RID: 5733
			public CHCNetSDK.NET_DVR_IPADDR struIP;

			// Token: 0x04001666 RID: 5734
			public ushort wDVRPort;

			// Token: 0x04001667 RID: 5735
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 34, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x02000164 RID: 356
		public struct tagNET_DVR_LF_CALIBRATION_PARAM
		{
			// Token: 0x04001668 RID: 5736
			public byte byPointNum;

			// Token: 0x04001669 RID: 5737
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400166A RID: 5738
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_DVR_CB_POINT[] struCBPoint;
		}

		// Token: 0x02000165 RID: 357
		public struct tagNET_DVR_LF_CFG
		{
			// Token: 0x0400166B RID: 5739
			public uint dwSize;

			// Token: 0x0400166C RID: 5740
			public byte byEnable;

			// Token: 0x0400166D RID: 5741
			public byte byFollowChan;

			// Token: 0x0400166E RID: 5742
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400166F RID: 5743
			public CHCNetSDK.tagNET_DVR_LF_CALIBRATION_PARAM struCalParam;
		}

		// Token: 0x02000166 RID: 358
		public struct tagNET_DVR_LF_MANUAL_CTRL_INFO
		{
			// Token: 0x04001670 RID: 5744
			public CHCNetSDK.tagNET_VCA_POINT struCtrlPoint;

			// Token: 0x04001671 RID: 5745
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000167 RID: 359
		public struct tagNET_DVR_LF_TRACK_MODE
		{
			// Token: 0x04001672 RID: 5746
			public uint dwSize;

			// Token: 0x04001673 RID: 5747
			public byte byTrackMode;

			// Token: 0x04001674 RID: 5748
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x02000168 RID: 360
			[StructLayout(LayoutKind.Explicit)]
			public struct uModeParam
			{
				// Token: 0x04001675 RID: 5749
				[FieldOffset(0)]
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
				public uint[] dwULen;

				// Token: 0x04001676 RID: 5750
				[FieldOffset(0)]
				public CHCNetSDK.tagNET_DVR_LF_MANUAL_CTRL_INFO struManualCtrl;

				// Token: 0x04001677 RID: 5751
				[FieldOffset(0)]
				public CHCNetSDK.tagNET_DVR_LF_TRACK_TARGET_INFO struTargetTrack;
			}
		}

		// Token: 0x02000169 RID: 361
		public struct tagNET_DVR_LF_TRACK_TARGET_INFO
		{
			// Token: 0x04001678 RID: 5752
			public uint dwTargetID;

			// Token: 0x04001679 RID: 5753
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200016A RID: 362
		public struct tagNET_DVR_MATRIX_ABILITY
		{
			// Token: 0x0400167A RID: 5754
			public uint dwSize;

			// Token: 0x0400167B RID: 5755
			public byte byDecNums;

			// Token: 0x0400167C RID: 5756
			public byte byStartChan;

			// Token: 0x0400167D RID: 5757
			public byte byVGANums;

			// Token: 0x0400167E RID: 5758
			public byte byBNCNums;

			// Token: 0x0400167F RID: 5759
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
			public byte[] byVGAWindowMode;

			// Token: 0x04001680 RID: 5760
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byBNCWindowMode;

			// Token: 0x04001681 RID: 5761
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x0200016B RID: 363
		public struct tagNET_DVR_NET_DISKCFG
		{
			// Token: 0x04001682 RID: 5762
			public uint dwSize;

			// Token: 0x04001683 RID: 5763
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_DVR_SINGLE_NET_DISK_INFO[] struNetDiskParam;
		}

		// Token: 0x0200016C RID: 364
		public struct tagNET_DVR_OSD_POSITION
		{
			// Token: 0x04001684 RID: 5764
			public byte byPositionMode;

			// Token: 0x04001685 RID: 5765
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04001686 RID: 5766
			public uint dwPos_x;

			// Token: 0x04001687 RID: 5767
			public uint dwPos_y;

			// Token: 0x04001688 RID: 5768
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200016D RID: 365
		public struct tagNET_DVR_OVERLAY_CHANNEL
		{
			// Token: 0x04001689 RID: 5769
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byChannel;

			// Token: 0x0400168A RID: 5770
			public uint dwDelayTime;

			// Token: 0x0400168B RID: 5771
			public byte byEnableDelayTime;

			// Token: 0x0400168C RID: 5772
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200016E RID: 366
		public struct tagNET_DVR_PACKAGE_LENGTH
		{
			// Token: 0x0400168D RID: 5773
			public byte byLengthMode;

			// Token: 0x0400168E RID: 5774
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x0400168F RID: 5775
			public uint dwFixLength;

			// Token: 0x04001690 RID: 5776
			public uint dwMaxLength;

			// Token: 0x04001691 RID: 5777
			public uint dwMinLength;

			// Token: 0x04001692 RID: 5778
			public byte byEndMode;

			// Token: 0x04001693 RID: 5779
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x04001694 RID: 5780
			public CHCNetSDK.NET_DVR_FRAMETYPECODE struEndCode;

			// Token: 0x04001695 RID: 5781
			public uint dwLengthPos;

			// Token: 0x04001696 RID: 5782
			public uint dwLengthLen;

			// Token: 0x04001697 RID: 5783
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;
		}

		// Token: 0x0200016F RID: 367
		public struct tagNET_DVR_PACKAGE_LOCATION
		{
			// Token: 0x04001698 RID: 5784
			public byte byOffsetMode;

			// Token: 0x04001699 RID: 5785
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x0400169A RID: 5786
			public uint dwOffsetPos;

			// Token: 0x0400169B RID: 5787
			public CHCNetSDK.NET_DVR_FRAMETYPECODE struTokenCode;

			// Token: 0x0400169C RID: 5788
			public byte byMultiplierValue;

			// Token: 0x0400169D RID: 5789
			public byte byEternOffset;

			// Token: 0x0400169E RID: 5790
			public byte byCodeMode;

			// Token: 0x0400169F RID: 5791
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x02000170 RID: 368
		public struct tagNET_DVR_PLATE_RET
		{
			// Token: 0x040016A0 RID: 5792
			public uint dwSize;

			// Token: 0x040016A1 RID: 5793
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byPlateNum;

			// Token: 0x040016A2 RID: 5794
			public byte byVehicleType;

			// Token: 0x040016A3 RID: 5795
			public byte byTrafficLight;

			// Token: 0x040016A4 RID: 5796
			public byte byPlateColor;

			// Token: 0x040016A5 RID: 5797
			public byte byDriveChan;

			// Token: 0x040016A6 RID: 5798
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byTimeInfo;

			// Token: 0x040016A7 RID: 5799
			public byte byCarSpeed;

			// Token: 0x040016A8 RID: 5800
			public byte byCarSpeedH;

			// Token: 0x040016A9 RID: 5801
			public byte byCarSpeedL;

			// Token: 0x040016AA RID: 5802
			public byte byRes;

			// Token: 0x040016AB RID: 5803
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 988, ArraySubType = UnmanagedType.I1)]
			public byte[] byInfo;

			// Token: 0x040016AC RID: 5804
			public uint dwPicLen;
		}

		// Token: 0x02000171 RID: 369
		public struct tagNET_DVR_SEARCH_EVENT_PARAM
		{
			// Token: 0x0600092E RID: 2350 RVA: 0x000DBBFD File Offset: 0x000DABFD
			public void init()
			{
				this.byRes = new byte[132];
				this.uSeniorPara = default(CHCNetSDK.uSeniorParam);
				this.uSeniorPara.init();
			}

			// Token: 0x040016AD RID: 5805
			public ushort wMajorType;

			// Token: 0x040016AE RID: 5806
			public ushort wMinorType;

			// Token: 0x040016AF RID: 5807
			public CHCNetSDK.NET_DVR_TIME struStartTime;

			// Token: 0x040016B0 RID: 5808
			public CHCNetSDK.NET_DVR_TIME struEndTime;

			// Token: 0x040016B1 RID: 5809
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 132, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x040016B2 RID: 5810
			public CHCNetSDK.uSeniorParam uSeniorPara;
		}

		// Token: 0x02000172 RID: 370
		public struct tagNET_DVR_SEARCH_EVENT_RET
		{
			// Token: 0x0600092F RID: 2351 RVA: 0x000DBC26 File Offset: 0x000DAC26
			public void init()
			{
				this.byChan = new byte[64];
				this.byRes = new byte[36];
				this.uSeniorRe = default(CHCNetSDK.uSeniorRet);
				this.uSeniorRe.init();
			}

			// Token: 0x040016B3 RID: 5811
			public ushort wMajorType;

			// Token: 0x040016B4 RID: 5812
			public ushort wMinorType;

			// Token: 0x040016B5 RID: 5813
			public CHCNetSDK.NET_DVR_TIME struStartTime;

			// Token: 0x040016B6 RID: 5814
			public CHCNetSDK.NET_DVR_TIME struEndTime;

			// Token: 0x040016B7 RID: 5815
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byChan;

			// Token: 0x040016B8 RID: 5816
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x040016B9 RID: 5817
			public CHCNetSDK.uSeniorRet uSeniorRe;
		}

		// Token: 0x02000173 RID: 371
		public struct tagNET_DVR_SINGLE_NET_DISK_INFO
		{
			// Token: 0x040016BA RID: 5818
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040016BB RID: 5819
			public CHCNetSDK.NET_DVR_IPADDR struNetDiskAddr;

			// Token: 0x040016BC RID: 5820
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
			public byte[] sDirectory;

			// Token: 0x040016BD RID: 5821
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 68, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x02000174 RID: 372
		public struct tagNET_DVR_VGA_DISP_CHAN_CFG
		{
			// Token: 0x040016BE RID: 5822
			public uint dwSize;

			// Token: 0x040016BF RID: 5823
			public byte byAudio;

			// Token: 0x040016C0 RID: 5824
			public byte byAudioWindowIdx;

			// Token: 0x040016C1 RID: 5825
			public byte byVgaResolution;

			// Token: 0x040016C2 RID: 5826
			public byte byVedioFormat;

			// Token: 0x040016C3 RID: 5827
			public uint dwWindowMode;

			// Token: 0x040016C4 RID: 5828
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byJoinDecChan;

			// Token: 0x040016C5 RID: 5829
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000175 RID: 373
		public struct tagNET_DVR_WIFI_CFG
		{
			// Token: 0x040016C6 RID: 5830
			public uint dwSize;

			// Token: 0x040016C7 RID: 5831
			public CHCNetSDK.tagNET_DVR__WIFI_CFG_EX struWifiCfg;
		}

		// Token: 0x02000176 RID: 374
		public struct tagNET_DVR_WIFI_WORKMODE
		{
			// Token: 0x040016C8 RID: 5832
			public uint dwSize;

			// Token: 0x040016C9 RID: 5833
			public uint dwNetworkInterfaceMode;
		}

		// Token: 0x02000177 RID: 375
		public struct tagNET_DVR_WIFIETHERNET
		{
			// Token: 0x040016CA RID: 5834
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sIpAddress;

			// Token: 0x040016CB RID: 5835
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sIpMask;

			// Token: 0x040016CC RID: 5836
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMACAddr;

			// Token: 0x040016CD RID: 5837
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] bRes;

			// Token: 0x040016CE RID: 5838
			public uint dwEnableDhcp;

			// Token: 0x040016CF RID: 5839
			public uint dwAutoDns;

			// Token: 0x040016D0 RID: 5840
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sFirstDns;

			// Token: 0x040016D1 RID: 5841
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sSecondDns;

			// Token: 0x040016D2 RID: 5842
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sGatewayIpAddr;

			// Token: 0x040016D3 RID: 5843
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] bRes2;
		}

		// Token: 0x02000178 RID: 376
		public struct tagNET_DVRT_TIME_FORMAT
		{
			// Token: 0x040016D4 RID: 5844
			public byte byTimeForm;

			// Token: 0x040016D5 RID: 5845
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040016D6 RID: 5846
			public byte byHourMode;

			// Token: 0x040016D7 RID: 5847
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;

			// Token: 0x040016D8 RID: 5848
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
			public string chSeprator;

			// Token: 0x040016D9 RID: 5849
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
			public string chDisplaySeprator;

			// Token: 0x040016DA RID: 5850
			public byte byDisplayForm;

			// Token: 0x040016DB RID: 5851
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes3;

			// Token: 0x040016DC RID: 5852
			public byte byDisplayHourMode;

			// Token: 0x040016DD RID: 5853
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 19, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes4;
		}

		// Token: 0x02000179 RID: 377
		public struct tagNET_IVMS_ALARM_JPEG
		{
			// Token: 0x040016DE RID: 5854
			public byte byPicProType;

			// Token: 0x040016DF RID: 5855
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x040016E0 RID: 5856
			public CHCNetSDK.NET_DVR_JPEGPARA struPicParam;
		}

		// Token: 0x0200017A RID: 378
		public struct tagNET_IVMS_BEHAVIORCFG
		{
			// Token: 0x040016E1 RID: 5857
			public uint dwSize;

			// Token: 0x040016E2 RID: 5858
			public byte byPicProType;

			// Token: 0x040016E3 RID: 5859
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x040016E4 RID: 5860
			public CHCNetSDK.NET_DVR_JPEGPARA struPicParam;

			// Token: 0x040016E5 RID: 5861
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_IVMS_RULECFG[] struRuleCfg;
		}

		// Token: 0x0200017B RID: 379
		public struct tagNET_IVMS_DEVSCHED
		{
			// Token: 0x040016E6 RID: 5862
			public CHCNetSDK.NET_DVR_SCHEDTIME struTime;

			// Token: 0x040016E7 RID: 5863
			public CHCNetSDK.NET_DVR_PU_STREAM_CFG struPUStream;
		}

		// Token: 0x0200017C RID: 380
		public struct tagNET_IVMS_ENTER_REGION
		{
			// Token: 0x040016E8 RID: 5864
			public uint dwSize;

			// Token: 0x040016E9 RID: 5865
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_VCA_ENTER_REGION[] struEnter;
		}

		// Token: 0x0200017D RID: 381
		public struct tagNET_IVMS_MASK_REGION_LIST
		{
			// Token: 0x040016EA RID: 5866
			public uint dwSize;

			// Token: 0x040016EB RID: 5867
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_VCA_MASK_REGION_LIST[] struList;
		}

		// Token: 0x0200017E RID: 382
		public struct tagNET_IVMS_ONE_RULE_
		{
			// Token: 0x040016EC RID: 5868
			public byte byActive;

			// Token: 0x040016ED RID: 5869
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x040016EE RID: 5870
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRuleName;

			// Token: 0x040016EF RID: 5871
			public CHCNetSDK.VCA_EVENT_TYPE dwEventType;

			// Token: 0x040016F0 RID: 5872
			public CHCNetSDK.tagNET_VCA_EVENT_UNION uEventParam;

			// Token: 0x040016F1 RID: 5873
			public CHCNetSDK.tagNET_VCA_SIZE_FILTER struSizeFilter;

			// Token: 0x040016F2 RID: 5874
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 68, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200017F RID: 383
		public struct tagNET_IVMS_RULECFG
		{
			// Token: 0x040016F3 RID: 5875
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_IVMS_ONE_RULE_[] struRule;
		}

		// Token: 0x02000180 RID: 384
		public struct tagNET_IVMS_SEARCHCFG
		{
			// Token: 0x040016F4 RID: 5876
			public uint dwSize;

			// Token: 0x040016F5 RID: 5877
			public CHCNetSDK.NET_DVR_MATRIX_DEC_REMOTE_PLAY struRemotePlay;

			// Token: 0x040016F6 RID: 5878
			public CHCNetSDK.tagNET_IVMS_ALARM_JPEG struAlarmJpeg;

			// Token: 0x040016F7 RID: 5879
			public CHCNetSDK.tagNET_IVMS_RULECFG struRuleCfg;
		}

		// Token: 0x02000181 RID: 385
		public struct tagNET_IVMS_STREAMCFG
		{
			// Token: 0x040016F8 RID: 5880
			public uint dwSize;

			// Token: 0x040016F9 RID: 5881
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_IVMS_DEVSCHED[] struDevSched;
		}

		// Token: 0x02000182 RID: 386
		public struct tagNET_MATRIX_PASSIVEMODE
		{
			// Token: 0x040016FA RID: 5882
			public ushort wTransProtol;

			// Token: 0x040016FB RID: 5883
			public ushort wPassivePort;

			// Token: 0x040016FC RID: 5884
			public CHCNetSDK.NET_DVR_IPADDR struMcastIP;

			// Token: 0x040016FD RID: 5885
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x02000183 RID: 387
		public struct tagNET_VCA_AREA
		{
			// Token: 0x040016FE RID: 5886
			public CHCNetSDK.tagNET_VCA_POLYGON struRegion;

			// Token: 0x040016FF RID: 5887
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000184 RID: 388
		public struct tagNET_VCA_BEHAVIOR_ABILITY
		{
			// Token: 0x04001700 RID: 5888
			public uint dwSize;

			// Token: 0x04001701 RID: 5889
			public uint dwAbilityType;

			// Token: 0x04001702 RID: 5890
			public byte byMaxRuleNum;

			// Token: 0x04001703 RID: 5891
			public byte byMaxTargetNum;

			// Token: 0x04001704 RID: 5892
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000185 RID: 389
		public struct tagNET_VCA_CHAN_IN_PARAM
		{
			// Token: 0x04001705 RID: 5893
			public byte byVCAType;

			// Token: 0x04001706 RID: 5894
			public byte byMode;

			// Token: 0x04001707 RID: 5895
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000186 RID: 390
		public struct tagNET_VCA_CTRLCFG
		{
			// Token: 0x04001708 RID: 5896
			public uint dwSize;

			// Token: 0x04001709 RID: 5897
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_VCA_CTRLINFO[] struCtrlInfo;

			// Token: 0x0400170A RID: 5898
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000187 RID: 391
		public struct tagNET_VCA_CTRLINFO
		{
			// Token: 0x0400170B RID: 5899
			public byte byVCAEnable;

			// Token: 0x0400170C RID: 5900
			public byte byVCAType;

			// Token: 0x0400170D RID: 5901
			public byte byStreamWithVCA;

			// Token: 0x0400170E RID: 5902
			public byte byMode;

			// Token: 0x0400170F RID: 5903
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000188 RID: 392
		public struct tagNET_VCA_DEV_ABILITY
		{
			// Token: 0x04001710 RID: 5904
			public uint dwSize;

			// Token: 0x04001711 RID: 5905
			public byte byVCAChanNum;

			// Token: 0x04001712 RID: 5906
			public byte byPlateChanNum;

			// Token: 0x04001713 RID: 5907
			public byte byBBaseChanNum;

			// Token: 0x04001714 RID: 5908
			public byte byBAdvanceChanNum;

			// Token: 0x04001715 RID: 5909
			public byte byBFullChanNum;

			// Token: 0x04001716 RID: 5910
			public byte byATMChanNum;

			// Token: 0x04001717 RID: 5911
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 34, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000189 RID: 393
		public struct tagNET_VCA_DEV_INFO
		{
			// Token: 0x04001718 RID: 5912
			public CHCNetSDK.NET_DVR_IPADDR struDevIP;

			// Token: 0x04001719 RID: 5913
			public ushort wPort;

			// Token: 0x0400171A RID: 5914
			public byte byChannel;

			// Token: 0x0400171B RID: 5915
			public byte byRes;
		}

		// Token: 0x0200018A RID: 394
		public struct tagNET_VCA_DRAW_MODE
		{
			// Token: 0x0400171C RID: 5916
			public uint dwSize;

			// Token: 0x0400171D RID: 5917
			public byte byDspAddTarget;

			// Token: 0x0400171E RID: 5918
			public byte byDspAddRule;

			// Token: 0x0400171F RID: 5919
			public byte byDspPicAddTarget;

			// Token: 0x04001720 RID: 5920
			public byte byDspPicAddRule;

			// Token: 0x04001721 RID: 5921
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200018B RID: 395
		public struct tagNET_VCA_ENTER_REGION
		{
			// Token: 0x04001722 RID: 5922
			public uint dwSize;

			// Token: 0x04001723 RID: 5923
			public byte byEnable;

			// Token: 0x04001724 RID: 5924
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x04001725 RID: 5925
			public CHCNetSDK.tagNET_VCA_POLYGON struPolygon;

			// Token: 0x04001726 RID: 5926
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x0200018C RID: 396
		[StructLayout(LayoutKind.Explicit)]
		public struct tagNET_VCA_EVENT_UNION
		{
			// Token: 0x04001727 RID: 5927
			[FieldOffset(0)]
			public CHCNetSDK.tagNET_VCA_AREA struArea;

			// Token: 0x04001728 RID: 5928
			[FieldOffset(0)]
			public CHCNetSDK.tagNET_VCA_HIGH_DENSITY struHighDensity;

			// Token: 0x04001729 RID: 5929
			[FieldOffset(0)]
			public CHCNetSDK.tagNET_VCA_INTRUSION struIntrusion;

			// Token: 0x0400172A RID: 5930
			[FieldOffset(0)]
			public CHCNetSDK.tagNET_VCA_PARAM_LOITER struLoiter;

			// Token: 0x0400172B RID: 5931
			[FieldOffset(0)]
			public CHCNetSDK.tagNET_VCA_PARKING struParking;

			// Token: 0x0400172C RID: 5932
			[FieldOffset(0)]
			public CHCNetSDK.tagNET_VCA_RUN struRun;

			// Token: 0x0400172D RID: 5933
			[FieldOffset(0)]
			public CHCNetSDK.tagNET_VCA_SCANNER struScanner;

			// Token: 0x0400172E RID: 5934
			[FieldOffset(0)]
			public CHCNetSDK.tagNET_VCA_STICK_UP struStickUp;

			// Token: 0x0400172F RID: 5935
			[FieldOffset(0)]
			public CHCNetSDK.tagNET_VCA_TAKE_LEFT struTakeTeft;

			// Token: 0x04001730 RID: 5936
			[FieldOffset(0)]
			public CHCNetSDK.tagNET_VCA_TRAVERSE_PLANE struTraversePlane;

			// Token: 0x04001731 RID: 5937
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.U4)]
			public uint[] uLen;
		}

		// Token: 0x0200018D RID: 397
		public struct tagNET_VCA_HIGH_DENSITY
		{
			// Token: 0x04001732 RID: 5938
			public CHCNetSDK.tagNET_VCA_POLYGON struRegion;

			// Token: 0x04001733 RID: 5939
			public float fDensity;

			// Token: 0x04001734 RID: 5940
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200018E RID: 398
		public struct tagNET_VCA_INTRUSION
		{
			// Token: 0x04001735 RID: 5941
			public CHCNetSDK.tagNET_VCA_POLYGON struRegion;

			// Token: 0x04001736 RID: 5942
			public ushort wDuration;

			// Token: 0x04001737 RID: 5943
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x0200018F RID: 399
		public struct tagNET_VCA_LINE
		{
			// Token: 0x04001738 RID: 5944
			public CHCNetSDK.tagNET_VCA_POINT struStart;

			// Token: 0x04001739 RID: 5945
			public CHCNetSDK.tagNET_VCA_POINT struEnd;
		}

		// Token: 0x02000190 RID: 400
		public struct tagNET_VCA_LINE_SEG_LIST
		{
			// Token: 0x0400173A RID: 5946
			public uint dwSize;

			// Token: 0x0400173B RID: 5947
			public byte bySegNum;

			// Token: 0x0400173C RID: 5948
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400173D RID: 5949
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_VCA_LINE_SEGMENT[] struSeg;
		}

		// Token: 0x02000191 RID: 401
		public struct tagNET_VCA_LINE_SEGMENT
		{
			// Token: 0x0400173E RID: 5950
			public CHCNetSDK.tagNET_VCA_POINT struStartPoint;

			// Token: 0x0400173F RID: 5951
			public CHCNetSDK.tagNET_VCA_POINT struEndPoint;

			// Token: 0x04001740 RID: 5952
			public float fValue;

			// Token: 0x04001741 RID: 5953
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000192 RID: 402
		public struct tagNET_VCA_MASK_REGION
		{
			// Token: 0x04001742 RID: 5954
			public byte byEnable;

			// Token: 0x04001743 RID: 5955
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x04001744 RID: 5956
			public CHCNetSDK.tagNET_VCA_POLYGON struPolygon;
		}

		// Token: 0x02000193 RID: 403
		public struct tagNET_VCA_MASK_REGION_LIST
		{
			// Token: 0x04001745 RID: 5957
			public uint dwSize;

			// Token: 0x04001746 RID: 5958
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x04001747 RID: 5959
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_VCA_MASK_REGION[] struMask;
		}

		// Token: 0x02000194 RID: 404
		public struct tagNET_VCA_ONE_RULE
		{
			// Token: 0x04001748 RID: 5960
			public byte byActive;

			// Token: 0x04001749 RID: 5961
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400174A RID: 5962
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRuleName;

			// Token: 0x0400174B RID: 5963
			public CHCNetSDK.VCA_EVENT_TYPE dwEventType;

			// Token: 0x0400174C RID: 5964
			public CHCNetSDK.tagNET_VCA_EVENT_UNION uEventParam;

			// Token: 0x0400174D RID: 5965
			public CHCNetSDK.tagNET_VCA_SIZE_FILTER struSizeFilter;

			// Token: 0x0400174E RID: 5966
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmTime;

			// Token: 0x0400174F RID: 5967
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

			// Token: 0x04001750 RID: 5968
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byRelRecordChan;
		}

		// Token: 0x02000195 RID: 405
		public struct tagNET_VCA_PARAM_LOITER
		{
			// Token: 0x04001751 RID: 5969
			public CHCNetSDK.tagNET_VCA_POLYGON struRegion;

			// Token: 0x04001752 RID: 5970
			public ushort wDuration;

			// Token: 0x04001753 RID: 5971
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000196 RID: 406
		public struct tagNET_VCA_PARKING
		{
			// Token: 0x04001754 RID: 5972
			public CHCNetSDK.tagNET_VCA_POLYGON struRegion;

			// Token: 0x04001755 RID: 5973
			public ushort wDuration;

			// Token: 0x04001756 RID: 5974
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x02000197 RID: 407
		public struct tagNET_VCA_PLATE_INFO
		{
			// Token: 0x04001757 RID: 5975
			public CHCNetSDK.VCA_RECOGNIZE_RESULT eResultFlag;

			// Token: 0x04001758 RID: 5976
			public CHCNetSDK.VCA_PLATE_TYPE ePlateType;

			// Token: 0x04001759 RID: 5977
			public CHCNetSDK.VCA_PLATE_COLOR ePlateColor;

			// Token: 0x0400175A RID: 5978
			public CHCNetSDK.tagNET_VCA_RECT struPlateRect;

			// Token: 0x0400175B RID: 5979
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
			public uint[] dwRes;

			// Token: 0x0400175C RID: 5980
			public uint dwLicenseLen;

			// Token: 0x0400175D RID: 5981
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sLicense;

			// Token: 0x0400175E RID: 5982
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sBelieve;
		}

		// Token: 0x02000198 RID: 408
		public struct tagNET_VCA_PLATE_PARAM
		{
			// Token: 0x0400175F RID: 5983
			public CHCNetSDK.tagNET_VCA_RECT struSearchRect;

			// Token: 0x04001760 RID: 5984
			public CHCNetSDK.tagNET_VCA_RECT struInvalidateRect;

			// Token: 0x04001761 RID: 5985
			public ushort wMinPlateWidth;

			// Token: 0x04001762 RID: 5986
			public ushort wTriggerDuration;

			// Token: 0x04001763 RID: 5987
			public byte byTriggerType;

			// Token: 0x04001764 RID: 5988
			public byte bySensitivity;

			// Token: 0x04001765 RID: 5989
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x04001766 RID: 5990
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byCharPriority;
		}

		// Token: 0x02000199 RID: 409
		public struct tagNET_VCA_PLATE_RESULT
		{
			// Token: 0x04001767 RID: 5991
			public uint dwSize;

			// Token: 0x04001768 RID: 5992
			public uint dwRelativeTime;

			// Token: 0x04001769 RID: 5993
			public uint dwAbsTime;

			// Token: 0x0400176A RID: 5994
			public byte byPlateNum;

			// Token: 0x0400176B RID: 5995
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes1;

			// Token: 0x0400176C RID: 5996
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_VCA_PLATE_INFO[] struPlateInfo;

			// Token: 0x0400176D RID: 5997
			public uint dwPicDataLen;

			// Token: 0x0400176E RID: 5998
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
			public uint[] dwRes2;

			// Token: 0x0400176F RID: 5999
			public IntPtr pImage;
		}

		// Token: 0x0200019A RID: 410
		public struct tagNET_VCA_PLATECFG
		{
			// Token: 0x04001770 RID: 6000
			public uint dwSize;

			// Token: 0x04001771 RID: 6001
			public byte byPicProType;

			// Token: 0x04001772 RID: 6002
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x04001773 RID: 6003
			public CHCNetSDK.NET_DVR_JPEGPARA struPictureParam;

			// Token: 0x04001774 RID: 6004
			public CHCNetSDK.tagNET_VCA_PLATEINFO struPlateInfo;

			// Token: 0x04001775 RID: 6005
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.NET_DVR_SCHEDTIME[] struAlarmTime;

			// Token: 0x04001776 RID: 6006
			public CHCNetSDK.NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

			// Token: 0x04001777 RID: 6007
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
			public byte[] byRelRecordChan;
		}

		// Token: 0x0200019B RID: 411
		public struct tagNET_VCA_PLATEINFO
		{
			// Token: 0x04001778 RID: 6008
			public CHCNetSDK.VCA_RECOGNIZE_SCENE eRecogniseScene;

			// Token: 0x04001779 RID: 6009
			public CHCNetSDK.tagNET_VCA_PLATE_PARAM struModifyParam;
		}

		// Token: 0x0200019C RID: 412
		public struct tagNET_VCA_POINT
		{
			// Token: 0x0400177A RID: 6010
			public float fX;

			// Token: 0x0400177B RID: 6011
			public float fY;
		}

		// Token: 0x0200019D RID: 413
		public struct tagNET_VCA_POLYGON
		{
			// Token: 0x0400177C RID: 6012
			public uint dwPointNum;
		}

		// Token: 0x0200019E RID: 414
		public struct tagNET_VCA_RECT
		{
			// Token: 0x0400177D RID: 6013
			public float fX;

			// Token: 0x0400177E RID: 6014
			public float fY;

			// Token: 0x0400177F RID: 6015
			public float fWidth;

			// Token: 0x04001780 RID: 6016
			public float fHeight;
		}

		// Token: 0x0200019F RID: 415
		public struct tagNET_VCA_RULE_ALARM
		{
			// Token: 0x04001781 RID: 6017
			public uint dwSize;

			// Token: 0x04001782 RID: 6018
			public uint dwRelativeTime;

			// Token: 0x04001783 RID: 6019
			public uint dwAbsTime;

			// Token: 0x04001784 RID: 6020
			public CHCNetSDK.tagNET_VCA_RULE_INFO struRuleInfo;

			// Token: 0x04001785 RID: 6021
			public CHCNetSDK.tagNET_VCA_TARGET_INFO struTargetInfo;

			// Token: 0x04001786 RID: 6022
			public CHCNetSDK.tagNET_VCA_DEV_INFO struDevInfo;

			// Token: 0x04001787 RID: 6023
			public uint dwPicDataLen;

			// Token: 0x04001788 RID: 6024
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
			public uint[] dwRes;

			// Token: 0x04001789 RID: 6025
			public IntPtr pImage;
		}

		// Token: 0x020001A0 RID: 416
		public struct tagNET_VCA_RULE_INFO
		{
			// Token: 0x0400178A RID: 6026
			public byte byRuleID;

			// Token: 0x0400178B RID: 6027
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400178C RID: 6028
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] byRuleName;

			// Token: 0x0400178D RID: 6029
			public CHCNetSDK.VCA_EVENT_TYPE dwEventType;

			// Token: 0x0400178E RID: 6030
			public CHCNetSDK.tagNET_VCA_EVENT_UNION uEventParam;
		}

		// Token: 0x020001A1 RID: 417
		public struct tagNET_VCA_RULECFG
		{
			// Token: 0x0400178F RID: 6031
			public uint dwSize;

			// Token: 0x04001790 RID: 6032
			public byte byPicProType;

			// Token: 0x04001791 RID: 6033
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x04001792 RID: 6034
			public CHCNetSDK.NET_DVR_JPEGPARA struPictureParam;

			// Token: 0x04001793 RID: 6035
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
			public CHCNetSDK.tagNET_VCA_ONE_RULE[] struRule;
		}

		// Token: 0x020001A2 RID: 418
		public struct tagNET_VCA_RUN
		{
			// Token: 0x04001794 RID: 6036
			public CHCNetSDK.tagNET_VCA_POLYGON struRegion;

			// Token: 0x04001795 RID: 6037
			public float fRunDistance;

			// Token: 0x04001796 RID: 6038
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020001A3 RID: 419
		public struct tagNET_VCA_SCANNER
		{
			// Token: 0x04001797 RID: 6039
			public CHCNetSDK.tagNET_VCA_POLYGON struRegion;

			// Token: 0x04001798 RID: 6040
			public ushort wDuration;

			// Token: 0x04001799 RID: 6041
			public byte bySensitivity;

			// Token: 0x0400179A RID: 6042
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020001A4 RID: 420
		public struct tagNET_VCA_SIZE_FILTER
		{
			// Token: 0x0400179B RID: 6043
			public byte byActive;

			// Token: 0x0400179C RID: 6044
			public byte byMode;

			// Token: 0x0400179D RID: 6045
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			// Token: 0x0400179E RID: 6046
			public CHCNetSDK.tagNET_VCA_RECT struMiniRect;

			// Token: 0x0400179F RID: 6047
			public CHCNetSDK.tagNET_VCA_RECT struMaxRect;
		}

		// Token: 0x020001A5 RID: 421
		public struct tagNET_VCA_STICK_UP
		{
			// Token: 0x040017A0 RID: 6048
			public CHCNetSDK.tagNET_VCA_POLYGON struRegion;

			// Token: 0x040017A1 RID: 6049
			public ushort wDuration;

			// Token: 0x040017A2 RID: 6050
			public byte bySensitivity;

			// Token: 0x040017A3 RID: 6051
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020001A6 RID: 422
		public struct tagNET_VCA_TAKE_LEFT
		{
			// Token: 0x040017A4 RID: 6052
			public CHCNetSDK.tagNET_VCA_POLYGON struRegion;

			// Token: 0x040017A5 RID: 6053
			public ushort wDuration;

			// Token: 0x040017A6 RID: 6054
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020001A7 RID: 423
		public struct tagNET_VCA_TARGET_INFO
		{
			// Token: 0x040017A7 RID: 6055
			public uint dwID;

			// Token: 0x040017A8 RID: 6056
			public CHCNetSDK.tagNET_VCA_RECT struRect;

			// Token: 0x040017A9 RID: 6057
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;
		}

		// Token: 0x020001A8 RID: 424
		public struct tagNET_VCA_TRAVERSE_PLANE
		{
			// Token: 0x040017AA RID: 6058
			public CHCNetSDK.tagNET_VCA_LINE struPlaneBottom;

			// Token: 0x040017AB RID: 6059
			public CHCNetSDK.VCA_CROSS_DIRECTION dwCrossDirection;

			// Token: 0x040017AC RID: 6060
			public byte byRes1;

			// Token: 0x040017AD RID: 6061
			public byte byPlaneHeight;

			// Token: 0x040017AE RID: 6062
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 38, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes2;
		}

		// Token: 0x020001A9 RID: 425
		public struct tagVEDIOPLATLOG
		{
			// Token: 0x040017AF RID: 6063
			public byte bySearchCondition;

			// Token: 0x040017B0 RID: 6064
			public byte byDevSequence;

			// Token: 0x040017B1 RID: 6065
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sSerialNumber;

			// Token: 0x040017B2 RID: 6066
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
			public byte[] byMacAddr;
		}

		// Token: 0x020001AA RID: 426
		public enum TRACK_MODE
		{
			// Token: 0x040017B4 RID: 6068
			MANUAL_CTRL,
			// Token: 0x040017B5 RID: 6069
			ALARM_TRACK,
			// Token: 0x040017B6 RID: 6070
			TARGET_TRACK
		}

		// Token: 0x020001AB RID: 427
		public struct TTY_CONFIG
		{
			// Token: 0x040017B7 RID: 6071
			public byte baudrate;

			// Token: 0x040017B8 RID: 6072
			public byte databits;

			// Token: 0x040017B9 RID: 6073
			public byte stopbits;

			// Token: 0x040017BA RID: 6074
			public byte parity;

			// Token: 0x040017BB RID: 6075
			public byte flowcontrol;

			// Token: 0x040017BC RID: 6076
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
			public byte[] res;
		}

		// Token: 0x020001AC RID: 428
		[StructLayout(LayoutKind.Explicit)]
		public struct uSeniorParam
		{
			// Token: 0x06000930 RID: 2352 RVA: 0x000DBC59 File Offset: 0x000DAC59
			public void init()
			{
				this.struAlarmPara = default(CHCNetSDK.struAlarmParam);
				this.struAlarmPara.init();
			}

			// Token: 0x040017BD RID: 6077
			[FieldOffset(0)]
			public CHCNetSDK.struAlarmParam struAlarmPara;

			// Token: 0x040017BE RID: 6078
			[FieldOffset(0)]
			public CHCNetSDK.struMotionParam struMotionPara;
		}

		// Token: 0x020001AD RID: 429
		[StructLayout(LayoutKind.Explicit)]
		public struct uSeniorRet
		{
			// Token: 0x06000931 RID: 2353 RVA: 0x000DBC72 File Offset: 0x000DAC72
			public void init()
			{
				this.struAlarmRe = default(CHCNetSDK.struAlarmRet);
				this.struAlarmRe.init();
			}

			// Token: 0x040017BF RID: 6079
			[FieldOffset(0)]
			public CHCNetSDK.struAlarmRet struAlarmRe;

			// Token: 0x040017C0 RID: 6080
			[FieldOffset(0)]
			public CHCNetSDK.struMotionRet struMotionRe;
		}

		// Token: 0x020001AE RID: 430
		public enum VCA_ABILITY_TYPE
		{
			// Token: 0x040017C2 RID: 6082
			ENTER_AREA_ABILITY = 2,
			// Token: 0x040017C3 RID: 6083
			EXIT_AREA_ABILITY = 4,
			// Token: 0x040017C4 RID: 6084
			HIGH_DENSITY_ABILITY = 256,
			// Token: 0x040017C5 RID: 6085
			INSTALL_SCANNER_ABILITY = -2147483648,
			// Token: 0x040017C6 RID: 6086
			INTRUSION_ABILITY = 8,
			// Token: 0x040017C7 RID: 6087
			LEFT_TAKE_ABILITY = 32,
			// Token: 0x040017C8 RID: 6088
			LF_TRACK_ABILITY = 512,
			// Token: 0x040017C9 RID: 6089
			LOITER_ABILITY = 16,
			// Token: 0x040017CA RID: 6090
			PARKING_ABILITY = 64,
			// Token: 0x040017CB RID: 6091
			RUN_ABILITY = 128,
			// Token: 0x040017CC RID: 6092
			STICK_UP_ABILITY = 1073741824,
			// Token: 0x040017CD RID: 6093
			TRAVERSE_PLANE_ABILITY = 1
		}

		// Token: 0x020001AF RID: 431
		public enum VCA_CHAN_ABILITY_TYPE
		{
			// Token: 0x040017CF RID: 6095
			VCA_ATM = 5,
			// Token: 0x040017D0 RID: 6096
			VCA_BEHAVIOR_ADVANCE = 2,
			// Token: 0x040017D1 RID: 6097
			VCA_BEHAVIOR_BASE = 1,
			// Token: 0x040017D2 RID: 6098
			VCA_BEHAVIOR_FULL = 3,
			// Token: 0x040017D3 RID: 6099
			VCA_PLATE
		}

		// Token: 0x020001B0 RID: 432
		public enum VCA_CHAN_MODE_TYPE
		{
			// Token: 0x040017D5 RID: 6101
			VCA_ATM_PANEL,
			// Token: 0x040017D6 RID: 6102
			VCA_ATM_SURROUND
		}

		// Token: 0x020001B1 RID: 433
		public enum VCA_CROSS_DIRECTION
		{
			// Token: 0x040017D8 RID: 6104
			VCA_BOTH_DIRECTION,
			// Token: 0x040017D9 RID: 6105
			VCA_LEFT_GO_RIGHT,
			// Token: 0x040017DA RID: 6106
			VCA_RIGHT_GO_LEFT
		}

		// Token: 0x020001B2 RID: 434
		public enum VCA_EVENT_TYPE
		{
			// Token: 0x040017DC RID: 6108
			VCA_ENTER_AREA = 2,
			// Token: 0x040017DD RID: 6109
			VCA_EXIT_AREA = 4,
			// Token: 0x040017DE RID: 6110
			VCA_HIGH_DENSITY = 256,
			// Token: 0x040017DF RID: 6111
			VCA_INSTALL_SCANNER = -2147483648,
			// Token: 0x040017E0 RID: 6112
			VCA_INTRUSION = 8,
			// Token: 0x040017E1 RID: 6113
			VCA_LEFT_TAKE = 32,
			// Token: 0x040017E2 RID: 6114
			VCA_LOITER = 16,
			// Token: 0x040017E3 RID: 6115
			VCA_PARKING = 64,
			// Token: 0x040017E4 RID: 6116
			VCA_RUN = 128,
			// Token: 0x040017E5 RID: 6117
			VCA_STICK_UP = 1073741824,
			// Token: 0x040017E6 RID: 6118
			VCA_TRAVERSE_PLANE = 1
		}

		// Token: 0x020001B3 RID: 435
		public enum VCA_PLATE_COLOR
		{
			// Token: 0x040017E8 RID: 6120
			VCA_BLUE_PLATE,
			// Token: 0x040017E9 RID: 6121
			VCA_YELLOW_PLATE,
			// Token: 0x040017EA RID: 6122
			VCA_WHITE_PLATE,
			// Token: 0x040017EB RID: 6123
			VCA_BLACK_PLATE
		}

		// Token: 0x020001B4 RID: 436
		public enum VCA_PLATE_TYPE
		{
			// Token: 0x040017ED RID: 6125
			VCA_STANDARD92_PLATE,
			// Token: 0x040017EE RID: 6126
			VCA_STANDARD02_PLATE,
			// Token: 0x040017EF RID: 6127
			VCA_WJPOLICE_PLATE,
			// Token: 0x040017F0 RID: 6128
			VCA_JINGCHE_PLATE,
			// Token: 0x040017F1 RID: 6129
			STANDARD92_BACK_PLATE
		}

		// Token: 0x020001B5 RID: 437
		public enum VCA_RECOGNIZE_RESULT
		{
			// Token: 0x040017F3 RID: 6131
			VCA_RECOGNIZE_FAILURE,
			// Token: 0x040017F4 RID: 6132
			VCA_IMAGE_RECOGNIZE_SUCCESS,
			// Token: 0x040017F5 RID: 6133
			VCA_VIDEO_RECOGNIZE_SUCCESS_OF_BEST_LICENSE,
			// Token: 0x040017F6 RID: 6134
			VCA_VIDEO_RECOGNIZE_SUCCESS_OF_NEW_LICENSE,
			// Token: 0x040017F7 RID: 6135
			VCA_VIDEO_RECOGNIZE_FINISH_OF_CUR_LICENSE
		}

		// Token: 0x020001B6 RID: 438
		public enum VCA_RECOGNIZE_SCENE
		{
			// Token: 0x040017F9 RID: 6137
			VCA_LOW_SPEED_SCENE,
			// Token: 0x040017FA RID: 6138
			VCA_HIGH_SPEED_SCENE,
			// Token: 0x040017FB RID: 6139
			VCA_MOBILE_CAMERA_SCENE
		}

		// Token: 0x020001B7 RID: 439
		public enum VCA_TRIGGER_TYPE
		{
			// Token: 0x040017FD RID: 6141
			INTER_TRIGGER,
			// Token: 0x040017FE RID: 6142
			EXTER_TRIGGER
		}

		// Token: 0x020001B8 RID: 440
		public enum VGA_MODE
		{
			// Token: 0x04001800 RID: 6144
			VGA_NOT_AVALIABLE,
			// Token: 0x04001801 RID: 6145
			VGA_THS8200_MODE_SVGA_60HZ,
			// Token: 0x04001802 RID: 6146
			VGA_THS8200_MODE_SVGA_75HZ,
			// Token: 0x04001803 RID: 6147
			VGA_THS8200_MODE_XGA_60HZ,
			// Token: 0x04001804 RID: 6148
			VGA_THS8200_MODE_XGA_70HZ,
			// Token: 0x04001805 RID: 6149
			VGA_THS8200_MODE_SXGA_60HZ,
			// Token: 0x04001806 RID: 6150
			VGA_THS8200_MODE_720P_60HZ,
			// Token: 0x04001807 RID: 6151
			VGA_THS8200_MODE_1080i_60HZ,
			// Token: 0x04001808 RID: 6152
			VGA_THS8200_MODE_1080P_30HZ,
			// Token: 0x04001809 RID: 6153
			VGA_THS8200_MODE_1080P_25HZ,
			// Token: 0x0400180A RID: 6154
			VGA_THS8200_MODE_UXGA_30HZ
		}

		// Token: 0x020001B9 RID: 441
		public enum VIDEO_STANDARD
		{
			// Token: 0x0400180C RID: 6156
			VS_NON,
			// Token: 0x0400180D RID: 6157
			VS_NTSC,
			// Token: 0x0400180E RID: 6158
			VS_PAL
		}

		// Token: 0x020001BA RID: 442
		public struct VODOPENPARAM
		{
			// Token: 0x0400180F RID: 6159
			public IntPtr sessionHandle;

			// Token: 0x04001810 RID: 6160
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
			public string dvrIP;

			// Token: 0x04001811 RID: 6161
			public uint dvrPort;

			// Token: 0x04001812 RID: 6162
			public uint channelNum;

			// Token: 0x04001813 RID: 6163
			public CHCNetSDK.BLOCKTIME startTime;

			// Token: 0x04001814 RID: 6164
			public CHCNetSDK.BLOCKTIME stopTime;

			// Token: 0x04001815 RID: 6165
			public uint uiUser;

			// Token: 0x04001816 RID: 6166
			public bool bUseIPServer;

			// Token: 0x04001817 RID: 6167
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string SerialNumber;

			// Token: 0x04001818 RID: 6168
			public CHCNetSDK.VodStreamFrameData streamFrameData;
		}

		// Token: 0x020001BB RID: 443
		public struct VODSEARCHPARAM
		{
			// Token: 0x04001819 RID: 6169
			public IntPtr sessionHandle;

			// Token: 0x0400181A RID: 6170
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
			public string dvrIP;

			// Token: 0x0400181B RID: 6171
			public uint dvrPort;

			// Token: 0x0400181C RID: 6172
			public uint channelNum;

			// Token: 0x0400181D RID: 6173
			public CHCNetSDK.BLOCKTIME startTime;

			// Token: 0x0400181E RID: 6174
			public CHCNetSDK.BLOCKTIME stopTime;

			// Token: 0x0400181F RID: 6175
			public bool bUseIPServer;

			// Token: 0x04001820 RID: 6176
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string SerialNumber;
		}

		// Token: 0x020001BC RID: 444
		// (Invoke) Token: 0x06000933 RID: 2355
		public delegate void VodStreamFrameData(IntPtr hStream, uint dwUser, int lFrameType, IntPtr pBuffer, uint dwSize);

		// Token: 0x020001BD RID: 445
		// (Invoke) Token: 0x06000937 RID: 2359
		public delegate void VOICEAUDIOSTART(string pRecvDataBuffer, uint dwBufSize, IntPtr pUser);

		// Token: 0x020001BE RID: 446
		// (Invoke) Token: 0x0600093B RID: 2363
		public delegate void VOICEDATACALLBACK(int lVoiceComHandle, string pRecvDataBuffer, uint dwBufSize, byte byAudioFlag, uint dwUser);

		// Token: 0x020001BF RID: 447
		// (Invoke) Token: 0x0600093F RID: 2367
		public delegate void VOICEDATACALLBACKV30(int lVoiceComHandle, string pRecvDataBuffer, uint dwBufSize, byte byAudioFlag, IntPtr pUser);
	}
}
