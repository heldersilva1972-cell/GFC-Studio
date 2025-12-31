using System;
using System.Runtime.InteropServices;

// Token: 0x02000344 RID: 836
internal class PlayCtrl
{
	// Token: 0x06001A47 RID: 6727 RVA: 0x0021EEC4 File Offset: 0x0021DEC4
	public static DateTime ConverUiTimeToDateTime(uint uiTime)
	{
		int num = (int)((uiTime >> 26) + 2000U);
		int num2 = (int)((uiTime >> 22) & 15U);
		int num3 = (int)((uiTime >> 17) & 31U);
		int num4 = (int)((uiTime >> 12) & 31U);
		int num5 = (int)((uiTime >> 6) & 63U);
		return new DateTime(num, num2, num3, num4, num5, (int)(uiTime & 63U));
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x0021EF0B File Offset: 0x0021DF0B
	public static void GetTimeFromUiTime(uint uiTime, ref uint uiHour, ref uint uiMinute, ref uint uiSecond)
	{
		uiHour = (uiTime >> 12) & 31U;
		uiMinute = (uiTime >> 6) & 63U;
		uiSecond = uiTime & 63U;
	}

	// Token: 0x06001A49 RID: 6729
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_AdjustWaveAudio(int nPort, int nCoefficient);

	// Token: 0x06001A4A RID: 6730
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_CheckDiscontinuousFrameNum(int nPort, int bCheck);

	// Token: 0x06001A4B RID: 6731
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_CloseFile(int nPort);

	// Token: 0x06001A4C RID: 6732
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_CloseStream(int nPort);

	// Token: 0x06001A4D RID: 6733
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_CloseStreamEx(int nPort);

	// Token: 0x06001A4E RID: 6734
	[DllImport("PlayCtrl.dll")]
	public static extern bool PLayM4_ConvertToBmpFile(IntPtr pBuf, int nSize, int nWidth, int nHeight, int nType, string sFileName);

	// Token: 0x06001A4F RID: 6735
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_ConvertToJpegFile(IntPtr pBuf, int nSize, int nWidth, int nHeight, int nType, string sFileName);

	// Token: 0x06001A50 RID: 6736
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_Fast(int nPort);

	// Token: 0x06001A51 RID: 6737
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_FreePort(int nPort);

	// Token: 0x06001A52 RID: 6738
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetAbsFrameNum(int nPort);

	// Token: 0x06001A53 RID: 6739
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetBMP(int nPort, ref byte pBitmap, uint nBufSize, ref uint pBmpSize);

	// Token: 0x06001A54 RID: 6740
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetBufferValue(int nPort, uint nBufType);

	// Token: 0x06001A55 RID: 6741
	[DllImport("PlayCtrl.dll")]
	public static extern int PlayM4_GetCaps();

	// Token: 0x06001A56 RID: 6742
	[DllImport("PlayCtrl.dll")]
	public static extern int PlayM4_GetCapsEx(uint nDDrawDeviceNum);

	// Token: 0x06001A57 RID: 6743
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetColor(int nPort, uint nRegionNum, ref int pBrightness, ref int pContrast, ref int pSaturation, ref int pHue);

	// Token: 0x06001A58 RID: 6744
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetColorKey(int nPort);

	// Token: 0x06001A59 RID: 6745
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetCurrentFrameNum(int nPort);

	// Token: 0x06001A5A RID: 6746
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetCurrentFrameRate(int nPort);

	// Token: 0x06001A5B RID: 6747
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetDDrawDeviceTotalNums();

	// Token: 0x06001A5C RID: 6748
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetDisplayBuf(int nPort);

	// Token: 0x06001A5D RID: 6749
	[DllImport("PlayCtrl.dll")]
	public static extern int PlayM4_GetDisplayType(int nPort);

	// Token: 0x06001A5E RID: 6750
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetFileHeadLength();

	// Token: 0x06001A5F RID: 6751
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetFileSpecialAttr(int nPort, ref uint pTimeStamp, ref uint pFileNum, ref uint pReserved);

	// Token: 0x06001A60 RID: 6752
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetFileTime(int nPort);

	// Token: 0x06001A61 RID: 6753
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetFileTimeEx(int nPort, ref uint pStart, ref uint pStop, ref uint pRev);

	// Token: 0x06001A62 RID: 6754
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetFileTotalFrames(int nPort);

	// Token: 0x06001A63 RID: 6755
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetJPEG(int nPort, IntPtr pJpeg, uint nBufSize, ref uint pJpegSize);

	// Token: 0x06001A64 RID: 6756
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetKeyFramePos(int nPort, uint nValue, uint nType, ref PlayCtrl.FRAME_POS pFramePos);

	// Token: 0x06001A65 RID: 6757
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetLastError(int nPort);

	// Token: 0x06001A66 RID: 6758
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetNextKeyFramePos(int nPort, uint nValue, uint nType, ref PlayCtrl.FRAME_POS pFramePos);

	// Token: 0x06001A67 RID: 6759
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetOriginalFrameCallBack(int nPort, int bIsChange, int bNormalSpeed, int nStartFrameNum, int nStartStamp, int nFileHeader, PlayCtrl.FUNGETORIGNALFRAME funGetOrignalFrame, int nUser);

	// Token: 0x06001A68 RID: 6760
	[DllImport("PlayCtrl.dll")]
	public static extern int PlayM4_GetOverlayMode(int nPort);

	// Token: 0x06001A69 RID: 6761
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetPictureQuality(int nPort, ref int bHighQuality);

	// Token: 0x06001A6A RID: 6762
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetPictureSize(int nPort, ref int pWidth, ref int pHeight);

	// Token: 0x06001A6B RID: 6763
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetPlayedFrames(int nPort);

	// Token: 0x06001A6C RID: 6764
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetPlayedTime(int nPort);

	// Token: 0x06001A6D RID: 6765
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetPlayedTimeEx(int nPort);

	// Token: 0x06001A6E RID: 6766
	[DllImport("PlayCtrl.dll")]
	public static extern float PlayM4_GetPlayPos(int nPort);

	// Token: 0x06001A6F RID: 6767
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetPort(ref int nPort);

	// Token: 0x06001A70 RID: 6768
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetRefValue(int nPort, ref byte pBuffer, ref uint pSize);

	// Token: 0x06001A71 RID: 6769
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetSdkVersion();

	// Token: 0x06001A72 RID: 6770
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetSourceBufferRemain(int nPort);

	// Token: 0x06001A73 RID: 6771
	[DllImport("PlayCtrl.dll")]
	public static extern uint PlayM4_GetSpecialData(int nPort);

	// Token: 0x06001A74 RID: 6772
	[DllImport("PlayCtrl.dll")]
	public static extern int PlayM4_GetStreamOpenMode(int nPort);

	// Token: 0x06001A75 RID: 6773
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetThrowBFrameCallBack(int nPort, PlayCtrl.FUNTHROWBFRAME funThrowBFrame, uint nUser);

	// Token: 0x06001A76 RID: 6774
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_GetTimerType(int nPort, ref uint pTimerType, ref uint pReserved);

	// Token: 0x06001A77 RID: 6775
	[DllImport("PlayCtrl.dll")]
	public static extern ushort PlayM4_GetVolume(int nPort);

	// Token: 0x06001A78 RID: 6776
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_InitDDraw(IntPtr hWnd);

	// Token: 0x06001A79 RID: 6777
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_InitDDrawDevice();

	// Token: 0x06001A7A RID: 6778
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_InputAudioData(int nPort, ref byte pBuf, uint nSize);

	// Token: 0x06001A7B RID: 6779
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_InputData(int nPort, ref byte pBuf, uint nSize);

	// Token: 0x06001A7C RID: 6780
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_InputVideoData(int nPort, ref byte pBuf, uint nSize);

	// Token: 0x06001A7D RID: 6781
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_OneByOne(int nPort);

	// Token: 0x06001A7E RID: 6782
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_OneByOneBack(int nPort);

	// Token: 0x06001A7F RID: 6783
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_OpenFile(int nPort, string sFileName);

	// Token: 0x06001A80 RID: 6784
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_OpenStream(int nPort, ref byte pFileHeadBuf, uint nSize, uint nBufPoolSize);

	// Token: 0x06001A81 RID: 6785
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_OpenStreamEx(int nPort, ref byte pFileHeadBuf, uint nSize, uint nBufPoolSize);

	// Token: 0x06001A82 RID: 6786
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_Pause(int nPort, uint nPause);

	// Token: 0x06001A83 RID: 6787
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_Play(int nPort, IntPtr hWnd);

	// Token: 0x06001A84 RID: 6788
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_PlaySound(int nPort);

	// Token: 0x06001A85 RID: 6789
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_PlaySoundShare(int nPort);

	// Token: 0x06001A86 RID: 6790
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_RealeseDDraw();

	// Token: 0x06001A87 RID: 6791
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_RefreshPlay(int nPort);

	// Token: 0x06001A88 RID: 6792
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_RefreshPlayEx(int nPort, uint nRegionNum);

	// Token: 0x06001A89 RID: 6793
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_RegisterDrawFun(int nPort, PlayCtrl.DRAWFUN DrawFun, int nUser);

	// Token: 0x06001A8A RID: 6794
	[DllImport("PlayCtrl.dll")]
	public static extern void PlayM4_ReleaseDDrawDevice();

	// Token: 0x06001A8B RID: 6795
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_ResetBuffer(int nPort, uint nBufType);

	// Token: 0x06001A8C RID: 6796
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_ResetSourceBuffer(int nPort);

	// Token: 0x06001A8D RID: 6797
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_ResetSourceBufFlag(int nPort);

	// Token: 0x06001A8E RID: 6798
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_RigisterDrawFun(int nPort, PlayCtrl.DRAWFUN DrawFun, int nUser);

	// Token: 0x06001A8F RID: 6799
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetAudioCallBack(int nPort, PlayCtrl.FUNAUDIO funAudio, int nUser);

	// Token: 0x06001A90 RID: 6800
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetCheckWatermarkCallBack(int nPort, PlayCtrl.FUNCHECKWATERMARK funCheckWatermark, uint nUser);

	// Token: 0x06001A91 RID: 6801
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetColor(int nPort, uint nRegionNum, int nBrightness, int nContrast, int nSaturation, int nHue);

	// Token: 0x06001A92 RID: 6802
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDDrawDevice(int nPort, uint nDeviceNum);

	// Token: 0x06001A93 RID: 6803
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDDrawDeviceEx(int nPort, uint nRegionNum, uint nDeviceNum);

	// Token: 0x06001A94 RID: 6804
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDecCallBack(int nPort, PlayCtrl.DECCBFUN DecCBFun);

	// Token: 0x06001A95 RID: 6805
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDecCallBackEx(int nPort, PlayCtrl.DECCBFUN DecCBFun, IntPtr pDest, int nDestSize);

	// Token: 0x06001A96 RID: 6806
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDecCallBackMend(int nPort, PlayCtrl.DECCBFUN DecCBFun, int nUser);

	// Token: 0x06001A97 RID: 6807
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDecCBStream(int nPort, uint nStream);

	// Token: 0x06001A98 RID: 6808
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDecodeFrameType(int nPort, uint nFrameType);

	// Token: 0x06001A99 RID: 6809
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDeflash(int nPort, int bDefalsh);

	// Token: 0x06001A9A RID: 6810
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDisplayBuf(int nPort, uint nNum);

	// Token: 0x06001A9B RID: 6811
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDisplayCallBack(int nPort, PlayCtrl.DISPLAYCBFUN DisplayCBFun);

	// Token: 0x06001A9C RID: 6812
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDisplayRegion(int nPort, uint nRegionNum, ref PlayCtrl.tagRECT pSrcRect, IntPtr hDestWnd, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

	// Token: 0x06001A9D RID: 6813
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetDisplayType(int nPort, int nType);

	// Token: 0x06001A9E RID: 6814
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetEncChangeMsg(int nPort, IntPtr hWnd, uint nMsg);

	// Token: 0x06001A9F RID: 6815
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetEncTypeChangeCallBack(int nPort, PlayCtrl.FUNENCCHANGE funEncChange, int nUser);

	// Token: 0x06001AA0 RID: 6816
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetFileEndCallback(int nPort, PlayCtrl.FILEENDCALLBACK FileEndCallback, IntPtr pUser);

	// Token: 0x06001AA1 RID: 6817
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetFileEndMsg(int nPort, IntPtr hWnd, uint nMsg);

	// Token: 0x06001AA2 RID: 6818
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetFileRefCallBack(int nPort, PlayCtrl.PFILEREFDONE pFileRefDone, uint nUser);

	// Token: 0x06001AA3 RID: 6819
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetGetUserDataCallBack(int nPort, PlayCtrl.FUNGETUSERDATA funGetUserData, uint nUser);

	// Token: 0x06001AA4 RID: 6820
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetImageSharpen(int nPort, uint nLevel);

	// Token: 0x06001AA5 RID: 6821
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetJpegQuality(int nQuality);

	// Token: 0x06001AA6 RID: 6822
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetOverlayFlipMode(int nPort, int bTrue);

	// Token: 0x06001AA7 RID: 6823
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetOverlayMode(int nPort, int bOverlay, uint colorKey);

	// Token: 0x06001AA8 RID: 6824
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetPicQuality(int nPort, int bHighQuality);

	// Token: 0x06001AA9 RID: 6825
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetPlayedTimeEx(int nPort, uint nTime);

	// Token: 0x06001AAA RID: 6826
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetPlayMode(int nPort, int bNormal);

	// Token: 0x06001AAB RID: 6827
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetPlayPos(int nPort, float fRelativePos);

	// Token: 0x06001AAC RID: 6828
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetRefValue(int nPort, ref byte pBuffer, uint nSize);

	// Token: 0x06001AAD RID: 6829
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetSecretKey(int nPort, int lKeyType, string pSecretKey, int lKeyLen);

	// Token: 0x06001AAE RID: 6830
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetSourceBufCallBack(int nPort, uint nThreShold, PlayCtrl.SOURCEBUFCALLBACKI SourceBufCallBack, uint dwUser, IntPtr pReserved);

	// Token: 0x06001AAF RID: 6831
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetStreamOpenMode(int nPort, uint nMode);

	// Token: 0x06001AB0 RID: 6832
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetTimerType(int nPort, uint nTimerType, uint nReserved);

	// Token: 0x06001AB1 RID: 6833
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetVerifyCallBack(int nPort, uint nBeginTime, uint nEndTime, PlayCtrl.FUNVERYFY funVerify, uint nUser);

	// Token: 0x06001AB2 RID: 6834
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SetVolume(int nPort, ushort nVolume);

	// Token: 0x06001AB3 RID: 6835
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_Slow(int nPort);

	// Token: 0x06001AB4 RID: 6836
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_Stop(int nPort);

	// Token: 0x06001AB5 RID: 6837
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_StopSound();

	// Token: 0x06001AB6 RID: 6838
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_StopSoundShare(int nPort);

	// Token: 0x06001AB7 RID: 6839
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_SyncToAudio(int nPort, int bSyncToAudio);

	// Token: 0x06001AB8 RID: 6840
	[DllImport("PlayCtrl.dll")]
	public static extern bool PlayM4_ThrowBFrameNum(int nPort, uint nNum);

	// Token: 0x040034D3 RID: 13523
	public const int AUDIO_ADPCM = 4096;

	// Token: 0x040034D4 RID: 13524
	public const int AUDIO_G711_A = 28945;

	// Token: 0x040034D5 RID: 13525
	public const int AUDIO_G711_U = 28944;

	// Token: 0x040034D6 RID: 13526
	public const int AUDIO_G722_1 = 29217;

	// Token: 0x040034D7 RID: 13527
	public const int AUDIO_G723_1 = 29233;

	// Token: 0x040034D8 RID: 13528
	public const int AUDIO_G726 = 29280;

	// Token: 0x040034D9 RID: 13529
	public const int AUDIO_G729 = 29328;

	// Token: 0x040034DA RID: 13530
	public const int AUDIO_MPEG = 8192;

	// Token: 0x040034DB RID: 13531
	public const int AUDIO_NULL = 0;

	// Token: 0x040034DC RID: 13532
	public const int BUF_AUDIO_RENDER = 4;

	// Token: 0x040034DD RID: 13533
	public const int BUF_AUDIO_SRC = 2;

	// Token: 0x040034DE RID: 13534
	public const int BUF_VIDEO_RENDER = 3;

	// Token: 0x040034DF RID: 13535
	public const int BUF_VIDEO_SRC = 1;

	// Token: 0x040034E0 RID: 13536
	public const int BY_FRAMENUM = 1;

	// Token: 0x040034E1 RID: 13537
	public const int BY_FRAMETIME = 2;

	// Token: 0x040034E2 RID: 13538
	public const int DISPLAY_NORMAL = 1;

	// Token: 0x040034E3 RID: 13539
	public const int DISPLAY_QUARTER = 2;

	// Token: 0x040034E4 RID: 13540
	public const int FOURCC_HKMI = 1212894537;

	// Token: 0x040034E5 RID: 13541
	public const int MAX_DIS_FRAMES = 50;

	// Token: 0x040034E6 RID: 13542
	public const int MAX_DISPLAY_WND = 4;

	// Token: 0x040034E7 RID: 13543
	public const int MAX_WAVE_COEF = 100;

	// Token: 0x040034E8 RID: 13544
	public const int MIN_DIS_FRAMES = 1;

	// Token: 0x040034E9 RID: 13545
	public const int MIN_WAVE_COEF = -100;

	// Token: 0x040034EA RID: 13546
	public const int PLAYM4_ALLOC_MEMORY_ERROR = 6;

	// Token: 0x040034EB RID: 13547
	public const string PLAYM4_API = "extern \"C\"__declspec(dllexport)";

	// Token: 0x040034EC RID: 13548
	public const int PLAYM4_BLT_ERROR = 22;

	// Token: 0x040034ED RID: 13549
	public const int PLAYM4_BUF_OVER = 11;

	// Token: 0x040034EE RID: 13550
	public const int PLAYM4_CHECK_FILE_ERROR = 20;

	// Token: 0x040034EF RID: 13551
	public const int PLAYM4_CREATE_DDRAW_ERROR = 9;

	// Token: 0x040034F0 RID: 13552
	public const int PLAYM4_CREATE_OBJ_ERROR = 8;

	// Token: 0x040034F1 RID: 13553
	public const int PLAYM4_CREATE_OFFSCREEN_ERROR = 10;

	// Token: 0x040034F2 RID: 13554
	public const int PLAYM4_CREATE_SOUND_ERROR = 12;

	// Token: 0x040034F3 RID: 13555
	public const int PLAYM4_DEC_AUDIO_ERROR = 5;

	// Token: 0x040034F4 RID: 13556
	public const int PLAYM4_DEC_VIDEO_ERROR = 4;

	// Token: 0x040034F5 RID: 13557
	public const int PLAYM4_EXTRACT_DATA_ERROR = 28;

	// Token: 0x040034F6 RID: 13558
	public const int PLAYM4_EXTRACT_NOT_SUPPORT = 27;

	// Token: 0x040034F7 RID: 13559
	public const int PLAYM4_FILEHEADER_UNKNOWN = 17;

	// Token: 0x040034F8 RID: 13560
	public const int PLAYM4_INIT_DECODER_ERROR = 19;

	// Token: 0x040034F9 RID: 13561
	public const int PLAYM4_INIT_TIMER_ERROR = 21;

	// Token: 0x040034FA RID: 13562
	public const int PLAYM4_JPEG_COMPRESS_ERROR = 26;

	// Token: 0x040034FB RID: 13563
	public const int PLAYM4_MAX_SUPPORTS = 500;

	// Token: 0x040034FC RID: 13564
	public const int PLAYM4_NOERROR = 0;

	// Token: 0x040034FD RID: 13565
	public const int PLAYM4_OPEN_FILE_ERROR = 7;

	// Token: 0x040034FE RID: 13566
	public const int PLAYM4_OPEN_FILE_ERROR_MULTI = 24;

	// Token: 0x040034FF RID: 13567
	public const int PLAYM4_OPEN_FILE_ERROR_VIDEO = 25;

	// Token: 0x04003500 RID: 13568
	public const int PLAYM4_ORDER_ERROR = 2;

	// Token: 0x04003501 RID: 13569
	public const int PLAYM4_PARA_OVER = 1;

	// Token: 0x04003502 RID: 13570
	public const int PLAYM4_SECRET_KEY_ERROR = 29;

	// Token: 0x04003503 RID: 13571
	public const int PLAYM4_SET_VOLUME_ERROR = 13;

	// Token: 0x04003504 RID: 13572
	public const int PLAYM4_SUPPORT_FILE_ONLY = 14;

	// Token: 0x04003505 RID: 13573
	public const int PLAYM4_SUPPORT_STREAM_ONLY = 15;

	// Token: 0x04003506 RID: 13574
	public const int PLAYM4_SYS_NOT_SUPPORT = 16;

	// Token: 0x04003507 RID: 13575
	public const int PLAYM4_TIMER_ERROR = 3;

	// Token: 0x04003508 RID: 13576
	public const int PLAYM4_UPDATE_ERROR = 23;

	// Token: 0x04003509 RID: 13577
	public const int PLAYM4_VERSION_INCORRECT = 18;

	// Token: 0x0400350A RID: 13578
	public const int SOURCE_BUF_MAX = 102400000;

	// Token: 0x0400350B RID: 13579
	public const int SOURCE_BUF_MIN = 51200;

	// Token: 0x0400350C RID: 13580
	public const int STREAME_FILE = 1;

	// Token: 0x0400350D RID: 13581
	public const int STREAME_REALTIME = 0;

	// Token: 0x0400350E RID: 13582
	public const int SUPPORT_BLT = 2;

	// Token: 0x0400350F RID: 13583
	public const int SUPPORT_BLTFOURCC = 4;

	// Token: 0x04003510 RID: 13584
	public const int SUPPORT_BLTSHRINKX = 8;

	// Token: 0x04003511 RID: 13585
	public const int SUPPORT_BLTSHRINKY = 16;

	// Token: 0x04003512 RID: 13586
	public const int SUPPORT_BLTSTRETCHX = 32;

	// Token: 0x04003513 RID: 13587
	public const int SUPPORT_BLTSTRETCHY = 64;

	// Token: 0x04003514 RID: 13588
	public const int SUPPORT_DDRAW = 1;

	// Token: 0x04003515 RID: 13589
	public const int SUPPORT_MMX = 256;

	// Token: 0x04003516 RID: 13590
	public const int SUPPORT_SSE = 128;

	// Token: 0x04003517 RID: 13591
	public const int SYSTEM_HIK = 1;

	// Token: 0x04003518 RID: 13592
	public const int SYSTEM_MPEG2_PS = 2;

	// Token: 0x04003519 RID: 13593
	public const int SYSTEM_MPEG2_TS = 3;

	// Token: 0x0400351A RID: 13594
	public const int SYSTEM_NULL = 0;

	// Token: 0x0400351B RID: 13595
	public const int SYSTEM_RTP = 4;

	// Token: 0x0400351C RID: 13596
	public const int T_AUDIO16 = 101;

	// Token: 0x0400351D RID: 13597
	public const int T_AUDIO8 = 100;

	// Token: 0x0400351E RID: 13598
	public const int T_RGB32 = 7;

	// Token: 0x0400351F RID: 13599
	public const int T_UYVY = 1;

	// Token: 0x04003520 RID: 13600
	public const int T_YV12 = 3;

	// Token: 0x04003521 RID: 13601
	public const int TIMER_1 = 1;

	// Token: 0x04003522 RID: 13602
	public const int TIMER_2 = 2;

	// Token: 0x04003523 RID: 13603
	public const int VIDEO_H264 = 1;

	// Token: 0x04003524 RID: 13604
	public const int VIDEO_MPEG4 = 3;

	// Token: 0x04003525 RID: 13605
	public const int VIDEO_NULL = 0;

	// Token: 0x02000345 RID: 837
	// (Invoke) Token: 0x06001ABB RID: 6843
	public delegate void DECCBFUN(int nPort, IntPtr pBuf, int nSize, ref PlayCtrl.FRAME_INFO pFrameInfo, int nReserved1, int nReserved2);

	// Token: 0x02000346 RID: 838
	// (Invoke) Token: 0x06001ABF RID: 6847
	public delegate void DISPLAYCBFUN(int nPort, IntPtr pBuf, int nSize, int nWidth, int nHeight, int nStamp, int nType, int nReserved);

	// Token: 0x02000347 RID: 839
	// (Invoke) Token: 0x06001AC3 RID: 6851
	public delegate void DRAWFUN(int nPort, IntPtr hDc, int nUser);

	// Token: 0x02000348 RID: 840
	// (Invoke) Token: 0x06001AC7 RID: 6855
	public delegate void FILEENDCALLBACK(int nPort, IntPtr pUser);

	// Token: 0x02000349 RID: 841
	public struct FRAME_INFO
	{
		// Token: 0x06001ACA RID: 6858 RVA: 0x0021EF2C File Offset: 0x0021DF2C
		public void Init()
		{
			this.nWidth = 0;
			this.nHeight = 0;
			this.nStamp = 0;
			this.nType = 0;
			this.nFrameRate = 0;
			this.dwFrameNum = 0U;
		}

		// Token: 0x04003526 RID: 13606
		public int nWidth;

		// Token: 0x04003527 RID: 13607
		public int nHeight;

		// Token: 0x04003528 RID: 13608
		public int nStamp;

		// Token: 0x04003529 RID: 13609
		public int nType;

		// Token: 0x0400352A RID: 13610
		public int nFrameRate;

		// Token: 0x0400352B RID: 13611
		public uint dwFrameNum;
	}

	// Token: 0x0200034A RID: 842
	public struct FRAME_POS
	{
		// Token: 0x06001ACB RID: 6859 RVA: 0x0021EF58 File Offset: 0x0021DF58
		public void Init()
		{
			this.nFilePos = 0;
			this.nFrameNum = 0;
			this.nFrameTime = 0;
			this.nErrorFrameNum = 0;
			this.pErrorTime = 0;
			this.nErrorLostFrameNum = 0;
			this.nErrorFrameSize = 0;
		}

		// Token: 0x0400352C RID: 13612
		public int nFilePos;

		// Token: 0x0400352D RID: 13613
		public int nFrameNum;

		// Token: 0x0400352E RID: 13614
		public int nFrameTime;

		// Token: 0x0400352F RID: 13615
		public int nErrorFrameNum;

		// Token: 0x04003530 RID: 13616
		public IntPtr pErrorTime;

		// Token: 0x04003531 RID: 13617
		public int nErrorLostFrameNum;

		// Token: 0x04003532 RID: 13618
		public int nErrorFrameSize;
	}

	// Token: 0x0200034B RID: 843
	public struct FRAME_TYPE
	{
		// Token: 0x06001ACC RID: 6860 RVA: 0x0021EF90 File Offset: 0x0021DF90
		public void Init()
		{
			this.pDataBuf = "";
			this.nSize = 0;
			this.nFrameNum = 0;
			this.bIsAudio = false;
			this.nReserved = 0;
		}

		// Token: 0x04003533 RID: 13619
		[MarshalAs(UnmanagedType.LPStr)]
		public string pDataBuf;

		// Token: 0x04003534 RID: 13620
		public int nSize;

		// Token: 0x04003535 RID: 13621
		public int nFrameNum;

		// Token: 0x04003536 RID: 13622
		public bool bIsAudio;

		// Token: 0x04003537 RID: 13623
		public int nReserved;
	}

	// Token: 0x0200034C RID: 844
	// (Invoke) Token: 0x06001ACE RID: 6862
	public delegate void FUNAUDIO(int nPort, string pAudioBuf, int nSize, int nStamp, int nType, int nUser);

	// Token: 0x0200034D RID: 845
	// (Invoke) Token: 0x06001AD2 RID: 6866
	public delegate void FUNCHECKWATERMARK(int nPort, ref PlayCtrl.WATERMARK_INFO pWatermarkInfo, uint nUser);

	// Token: 0x0200034E RID: 846
	// (Invoke) Token: 0x06001AD6 RID: 6870
	public delegate void FUNENCCHANGE(int nPort, int nUser);

	// Token: 0x0200034F RID: 847
	// (Invoke) Token: 0x06001ADA RID: 6874
	public delegate void FUNGETORIGNALFRAME(int nPort, ref PlayCtrl.FRAME_TYPE frameType, int nUser);

	// Token: 0x02000350 RID: 848
	// (Invoke) Token: 0x06001ADE RID: 6878
	public delegate void FUNGETUSERDATA(int nPort, ref byte pUserBuf, uint nBufLen, uint nUser);

	// Token: 0x02000351 RID: 849
	// (Invoke) Token: 0x06001AE2 RID: 6882
	public delegate void FUNTHROWBFRAME(int nPort, uint nBFrame, uint nUser);

	// Token: 0x02000352 RID: 850
	// (Invoke) Token: 0x06001AE6 RID: 6886
	public delegate void FUNVERYFY(int nPort, ref PlayCtrl.FRAME_POS pFilePos, uint bIsVideo, uint nUser);

	// Token: 0x02000353 RID: 851
	public struct HIK_MEDIAINFO
	{
		// Token: 0x06001AE9 RID: 6889 RVA: 0x0021EFBC File Offset: 0x0021DFBC
		public void Init()
		{
			this.media_fourcc = 0U;
			this.media_version = 0;
			this.device_id = 0;
			this.system_format = 0;
			this.video_format = 0;
			this.audio_format = 0;
			this.audio_channels = 0;
			this.audio_bits_per_sample = 0;
			this.audio_samplesrate = 0U;
			this.audio_bitrate = 0U;
			this.reserved = new uint[4];
		}

		// Token: 0x04003538 RID: 13624
		public uint media_fourcc;

		// Token: 0x04003539 RID: 13625
		public ushort media_version;

		// Token: 0x0400353A RID: 13626
		public ushort device_id;

		// Token: 0x0400353B RID: 13627
		public ushort system_format;

		// Token: 0x0400353C RID: 13628
		public ushort video_format;

		// Token: 0x0400353D RID: 13629
		public ushort audio_format;

		// Token: 0x0400353E RID: 13630
		public byte audio_channels;

		// Token: 0x0400353F RID: 13631
		public byte audio_bits_per_sample;

		// Token: 0x04003540 RID: 13632
		public uint audio_samplesrate;

		// Token: 0x04003541 RID: 13633
		public uint audio_bitrate;

		// Token: 0x04003542 RID: 13634
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
		public uint[] reserved;
	}

	// Token: 0x02000354 RID: 852
	// (Invoke) Token: 0x06001AEB RID: 6891
	public delegate void PFILEREFDONE(uint nPort, uint nUser);

	// Token: 0x02000355 RID: 853
	// (Invoke) Token: 0x06001AEF RID: 6895
	public delegate void SOURCEBUFCALLBACKI(int nPort, uint nBufSize, uint dwUser, IntPtr pResvered);

	// Token: 0x02000356 RID: 854
	public struct SYSTEMTIME
	{
		// Token: 0x06001AF2 RID: 6898 RVA: 0x0021F01B File Offset: 0x0021E01B
		public void Init()
		{
			this.wYear = 0;
			this.wMonth = 0;
			this.wDayOfWeek = 0;
			this.wDay = 0;
			this.wHour = 0;
			this.wMinute = 0;
			this.wSecond = 0;
			this.wMilliseconds = 0;
		}

		// Token: 0x04003543 RID: 13635
		public ushort wYear;

		// Token: 0x04003544 RID: 13636
		public ushort wMonth;

		// Token: 0x04003545 RID: 13637
		public ushort wDayOfWeek;

		// Token: 0x04003546 RID: 13638
		public ushort wDay;

		// Token: 0x04003547 RID: 13639
		public ushort wHour;

		// Token: 0x04003548 RID: 13640
		public ushort wMinute;

		// Token: 0x04003549 RID: 13641
		public ushort wSecond;

		// Token: 0x0400354A RID: 13642
		public ushort wMilliseconds;
	}

	// Token: 0x02000357 RID: 855
	public struct tagRECT
	{
		// Token: 0x06001AF3 RID: 6899 RVA: 0x0021F055 File Offset: 0x0021E055
		public void Init()
		{
			this.left = 0;
			this.top = 0;
			this.right = 0;
			this.bottom = 0;
		}

		// Token: 0x0400354B RID: 13643
		public int left;

		// Token: 0x0400354C RID: 13644
		public int top;

		// Token: 0x0400354D RID: 13645
		public int right;

		// Token: 0x0400354E RID: 13646
		public int bottom;
	}

	// Token: 0x02000358 RID: 856
	public struct WATERMARK_INFO
	{
		// Token: 0x06001AF4 RID: 6900 RVA: 0x0021F073 File Offset: 0x0021E073
		public void Init()
		{
			this.pDataBuf = "";
			this.nSize = 0;
			this.nFrameNum = 0;
			this.bRsaRight = false;
			this.nReserved = 0;
		}

		// Token: 0x0400354F RID: 13647
		[MarshalAs(UnmanagedType.LPStr)]
		public string pDataBuf;

		// Token: 0x04003550 RID: 13648
		public int nSize;

		// Token: 0x04003551 RID: 13649
		public int nFrameNum;

		// Token: 0x04003552 RID: 13650
		public bool bRsaRight;

		// Token: 0x04003553 RID: 13651
		public int nReserved;
	}
}
