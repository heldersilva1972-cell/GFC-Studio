using System;
using System.Runtime.InteropServices;

namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x0200024E RID: 590
	public class AcsDemoPublic
	{
		// Token: 0x060012C7 RID: 4807 RVA: 0x0016B568 File Offset: 0x0016A568
		public static bool CheckDate(CHCNetSDK.NET_DVR_SIMPLE_DAYTIME struItem)
		{
			return struItem.byHour <= 24 && struItem.byMinute <= 59 && struItem.bySecond <= 59;
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0016B590 File Offset: 0x0016A590
		public static bool CheckState(CHCNetSDK.NET_DVR_DATE struItem)
		{
			return struItem.wYear >= 1970 && struItem.byMonth <= 12 && struItem.byDay <= 31;
		}

		// Token: 0x0400225B RID: 8795
		public const int ALARM_INFO_T = 0;

		// Token: 0x0400225C RID: 8796
		public const int CARD_OFF_LINE = 9;

		// Token: 0x0400225D RID: 8797
		public const int CARD_ON_LINE = 8;

		// Token: 0x0400225E RID: 8798
		public const int CARDREADERTYPE = 6;

		// Token: 0x0400225F RID: 8799
		public const int CHAN_ALARM = 10;

		// Token: 0x04002260 RID: 8800
		public const int CHAN_OFF_LINE = 13;

		// Token: 0x04002261 RID: 8801
		public const int CHAN_ORIGINAL = 6;

		// Token: 0x04002262 RID: 8802
		public const int CHAN_PLAY = 7;

		// Token: 0x04002263 RID: 8803
		public const int CHAN_PLAY_ALARM = 11;

		// Token: 0x04002264 RID: 8804
		public const int CHAN_PLAY_RECORD = 9;

		// Token: 0x04002265 RID: 8805
		public const int CHAN_PLAY_RECORD_ALARM = 12;

		// Token: 0x04002266 RID: 8806
		public const int CHAN_RECORD = 8;

		// Token: 0x04002267 RID: 8807
		public const int CHANNELTYPE = 3;

		// Token: 0x04002268 RID: 8808
		public const int DEVICE_ALARM = 4;

		// Token: 0x04002269 RID: 8809
		public const int DEVICE_FORTIFY = 3;

		// Token: 0x0400226A RID: 8810
		public const int DEVICE_FORTIFY_ALARM = 5;

		// Token: 0x0400226B RID: 8811
		public const int DEVICE_LOGIN = 2;

		// Token: 0x0400226C RID: 8812
		public const int DEVICE_LOGOUT = 1;

		// Token: 0x0400226D RID: 8813
		public const int DEVICETYPE = 2;

		// Token: 0x0400226E RID: 8814
		public const int DOOR_ALWAYSCOLSED = 6;

		// Token: 0x0400226F RID: 8815
		public const int DOOR_ALWAYSOPEN = 7;

		// Token: 0x04002270 RID: 8816
		public const int DOOR_COLSED = 6;

		// Token: 0x04002271 RID: 8817
		public const int DOOR_OPEN = 7;

		// Token: 0x04002272 RID: 8818
		public const int DOORTYPE = 4;

		// Token: 0x04002273 RID: 8819
		public const int MAX_DEVICES = 512;

		// Token: 0x04002274 RID: 8820
		public const int MIRROR_CHAN_INDEX = 400;

		// Token: 0x04002275 RID: 8821
		public const int OPERATION_FAIL_T = 2;

		// Token: 0x04002276 RID: 8822
		public const int OPERATION_SUCC_T = 1;

		// Token: 0x04002277 RID: 8823
		public const int PLAY_FAIL_T = 4;

		// Token: 0x04002278 RID: 8824
		public const int PLAY_SUCC_T = 3;

		// Token: 0x04002279 RID: 8825
		public const int REGIONTYPE = 0;

		// Token: 0x0400227A RID: 8826
		public const int TREE_ALL = 0;

		// Token: 0x0400227B RID: 8827
		public const int USERTYPE = 5;

		// Token: 0x0400227C RID: 8828
		public const int ZERO_CHAN_INDEX = 500;

		// Token: 0x0400227D RID: 8829
		public static readonly string[] strCardType = new string[]
		{
			"unknown", "ordinary card", "disabled card", "black list card", "patrol card", "stress card", "super card", "guest card", "remove card", "employee card",
			"emergency card", "emergency management card"
		};

		// Token: 0x0400227E RID: 8830
		public static readonly string[] strDoorStatus = new string[] { "invalid", "sleep", "Normally open", "Normally close" };

		// Token: 0x0400227F RID: 8831
		public static readonly string[] strFingerType = new string[] { "Common fingerprint", "Stress fingerprint", "Patrol fingerprint", "Super fingerprint", "Dismissing fingerprint" };

		// Token: 0x04002280 RID: 8832
		public static readonly string[] strVerify = new string[]
		{
			"invalid", "sleep", "card and password", "card", "card or password", "fingerprint", "fingerprint and password", "fingerprint or card", "fingerprint and card", "fingerprint and card and password",
			"face or fingerprint or card or password", "face and fingerprint", "face and password", "face and card", "face", "work number and password", "fingerprint or password", "work number and fingerprint", "work number and fingerprint and password", "face and fingerprint and card",
			"face and password and fingerprint", "work number and face", "face or face and swipe card"
		};

		// Token: 0x0200024F RID: 591
		public enum DEMO_CHANNEL_TYPE
		{
			// Token: 0x04002282 RID: 8834
			DEMO_CHANNEL_TYPE_ANALOG,
			// Token: 0x04002283 RID: 8835
			DEMO_CHANNEL_TYPE_INVALID = -1,
			// Token: 0x04002284 RID: 8836
			DEMO_CHANNEL_TYPE_IP = 1,
			// Token: 0x04002285 RID: 8837
			DEMO_CHANNEL_TYPE_MIRROR
		}

		// Token: 0x02000250 RID: 592
		public struct LOCAL_LOG_INFO
		{
			// Token: 0x04002286 RID: 8838
			public int iLogType;

			// Token: 0x04002287 RID: 8839
			public string strTime;

			// Token: 0x04002288 RID: 8840
			public string strLogInfo;

			// Token: 0x04002289 RID: 8841
			public string strDevInfo;

			// Token: 0x0400228A RID: 8842
			public string strErrInfo;

			// Token: 0x0400228B RID: 8843
			public string strSavePath;
		}

		// Token: 0x02000251 RID: 593
		public struct PASSIVEDECODE_CHANINFO
		{
			// Token: 0x060012CB RID: 4811 RVA: 0x0016B778 File Offset: 0x0016A778
			public void init()
			{
				this.lPassiveHandle = -1;
				this.lRealHandle = -1;
				this.lUserID = -1;
				this.lSel = -1;
				this.hFileThread = IntPtr.Zero;
				this.hFileHandle = IntPtr.Zero;
				this.hExitThread = IntPtr.Zero;
				this.hThreadExit = IntPtr.Zero;
				this.strRecordFilePath = null;
			}

			// Token: 0x0400228C RID: 8844
			public int lPassiveHandle;

			// Token: 0x0400228D RID: 8845
			public int lRealHandle;

			// Token: 0x0400228E RID: 8846
			public int lUserID;

			// Token: 0x0400228F RID: 8847
			public int lSel;

			// Token: 0x04002290 RID: 8848
			public IntPtr hFileThread;

			// Token: 0x04002291 RID: 8849
			public IntPtr hFileHandle;

			// Token: 0x04002292 RID: 8850
			public IntPtr hExitThread;

			// Token: 0x04002293 RID: 8851
			public IntPtr hThreadExit;

			// Token: 0x04002294 RID: 8852
			public string strRecordFilePath;
		}

		// Token: 0x02000252 RID: 594
		public struct PREVIEW_IFNO
		{
			// Token: 0x04002295 RID: 8853
			public int iDeviceIndex;

			// Token: 0x04002296 RID: 8854
			public int iChanIndex;

			// Token: 0x04002297 RID: 8855
			public byte PanelNo;

			// Token: 0x04002298 RID: 8856
			public int lRealHandle;

			// Token: 0x04002299 RID: 8857
			public IntPtr hPlayWnd;
		}

		// Token: 0x02000253 RID: 595
		public struct STRU_CHANNEL_INFO
		{
			// Token: 0x060012CC RID: 4812 RVA: 0x0016B7D4 File Offset: 0x0016A7D4
			public void init()
			{
				this.iDeviceIndex = -1;
				this.iChanIndex = -1;
				this.iChannelNO = -1;
				this.iChanType = AcsDemoPublic.DEMO_CHANNEL_TYPE.DEMO_CHANNEL_TYPE_INVALID;
				this.chChanName = null;
				this.dwProtocol = 0U;
				this.dwStreamType = 0U;
				this.dwLinkMode = 0U;
				this.iPicResolution = 0;
				this.iPicQuality = 2;
				this.lRealHandle = -1;
				this.bLocalManualRec = false;
				this.bAlarm = false;
				this.bEnable = false;
				this.dwImageType = 6U;
				this.chAccessChanIP = null;
				this.pNext = IntPtr.Zero;
				this.dwPreviewMode = 0U;
				this.bPassbackRecord = false;
				this.nPreviewProtocolType = 0U;
			}

			// Token: 0x0400229A RID: 8858
			public int iDeviceIndex;

			// Token: 0x0400229B RID: 8859
			public int iChanIndex;

			// Token: 0x0400229C RID: 8860
			public AcsDemoPublic.DEMO_CHANNEL_TYPE iChanType;

			// Token: 0x0400229D RID: 8861
			public int iChannelNO;

			// Token: 0x0400229E RID: 8862
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public string chChanName;

			// Token: 0x0400229F RID: 8863
			public uint dwProtocol;

			// Token: 0x040022A0 RID: 8864
			public uint dwStreamType;

			// Token: 0x040022A1 RID: 8865
			public uint dwLinkMode;

			// Token: 0x040022A2 RID: 8866
			public bool bPassbackRecord;

			// Token: 0x040022A3 RID: 8867
			public uint dwPreviewMode;

			// Token: 0x040022A4 RID: 8868
			public int iPicResolution;

			// Token: 0x040022A5 RID: 8869
			public int iPicQuality;

			// Token: 0x040022A6 RID: 8870
			public int lRealHandle;

			// Token: 0x040022A7 RID: 8871
			public bool bLocalManualRec;

			// Token: 0x040022A8 RID: 8872
			public bool bAlarm;

			// Token: 0x040022A9 RID: 8873
			public bool bEnable;

			// Token: 0x040022AA RID: 8874
			public uint dwImageType;

			// Token: 0x040022AB RID: 8875
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string chAccessChanIP;

			// Token: 0x040022AC RID: 8876
			public uint nPreviewProtocolType;

			// Token: 0x040022AD RID: 8877
			public IntPtr pNext;
		}

		// Token: 0x02000254 RID: 596
		public struct STRU_DEVICE_INFO
		{
			// Token: 0x060012CD RID: 4813 RVA: 0x0016B874 File Offset: 0x0016A874
			public void Init()
			{
				this.iGroupNO = -1;
				this.iDeviceIndex = -1;
				this.lLoginID = -1;
				this.dwDevSoftVer = 0U;
				this.chLocalNodeName = null;
				this.chDeviceName = null;
				this.chDeviceIP = null;
				this.chDeviceIPInFileName = null;
				this.chLoginUserName = null;
				this.chLoginPwd = null;
				this.chDeviceMultiIP = null;
				this.chSerialNumber = null;
				this.iDeviceChanNum = -1;
				this.iDoorNum = -1;
				this.iDoorStatus = new int[768];
				this.sDoorName = new string[256];
				this.iStartChan = 0;
				this.iDeviceType = 0;
				this.iDiskNum = 0;
				this.lDevicePort = 8000;
				this.iAlarmInNum = 0;
				this.iAlarmOutNum = 0;
				this.iAnalogChanNum = 0;
				this.iIPChanNum = 0;
				this.byAudioInputChanNum = 0;
				this.byStartAudioInputChanNo = 0;
				this.bCycle = false;
				this.bPlayDevice = false;
				this.bVoiceTalk = false;
				this.lFortifyHandle = -1;
				this.bAlarm = false;
				this.iDeviceLoginType = 0;
				this.dwImageType = 1U;
				this.bIPRet = false;
				this.pNext = IntPtr.Zero;
				this.lFirstEnableChanIndex = 0;
				this.lTranHandle = -1;
				this.byZeroChanNum = 0;
				this.sSecretKey = "StreamNotEncrypt";
				this.iAudioEncType = 0;
				this.bySupport1 = 0;
				this.bySupport2 = 0;
				this.bySupport5 = 0;
				this.bySupport7 = 0;
				this.byStartDTalkChan = 0;
				this.byLanguageType = 0;
				this.byCharaterEncodeType = 0;
				this.bInit = true;
				this.byPanelNo = 4;
			}

			// Token: 0x040022AE RID: 8878
			public int iDeviceIndex;

			// Token: 0x040022AF RID: 8879
			public int lLoginID;

			// Token: 0x040022B0 RID: 8880
			public uint dwDevSoftVer;

			// Token: 0x040022B1 RID: 8881
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public string chLocalNodeName;

			// Token: 0x040022B2 RID: 8882
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public string chDeviceName;

			// Token: 0x040022B3 RID: 8883
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 130)]
			public string chDeviceIP;

			// Token: 0x040022B4 RID: 8884
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 130)]
			public string chDeviceIPInFileName;

			// Token: 0x040022B5 RID: 8885
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string chLoginUserName;

			// Token: 0x040022B6 RID: 8886
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string chLoginPwd;

			// Token: 0x040022B7 RID: 8887
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 130)]
			public string chDeviceMultiIP;

			// Token: 0x040022B8 RID: 8888
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
			public string chSerialNumber;

			// Token: 0x040022B9 RID: 8889
			public int iDeviceChanNum;

			// Token: 0x040022BA RID: 8890
			public int iStartChan;

			// Token: 0x040022BB RID: 8891
			public int iStartDChan;

			// Token: 0x040022BC RID: 8892
			public int iDeviceType;

			// Token: 0x040022BD RID: 8893
			public int iDiskNum;

			// Token: 0x040022BE RID: 8894
			public int lDevicePort;

			// Token: 0x040022BF RID: 8895
			public int iAlarmInNum;

			// Token: 0x040022C0 RID: 8896
			public int iAlarmOutNum;

			// Token: 0x040022C1 RID: 8897
			public int iAudioNum;

			// Token: 0x040022C2 RID: 8898
			public int iAnalogChanNum;

			// Token: 0x040022C3 RID: 8899
			public int iIPChanNum;

			// Token: 0x040022C4 RID: 8900
			public int iGroupNO;

			// Token: 0x040022C5 RID: 8901
			public bool bCycle;

			// Token: 0x040022C6 RID: 8902
			public bool bPlayDevice;

			// Token: 0x040022C7 RID: 8903
			public bool bVoiceTalk;

			// Token: 0x040022C8 RID: 8904
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public int[] lAudioHandle;

			// Token: 0x040022C9 RID: 8905
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public bool[] bCheckBroadcast;

			// Token: 0x040022CA RID: 8906
			public int lFortifyHandle;

			// Token: 0x040022CB RID: 8907
			public bool bAlarm;

			// Token: 0x040022CC RID: 8908
			public int iDeviceLoginType;

			// Token: 0x040022CD RID: 8909
			public uint dwImageType;

			// Token: 0x040022CE RID: 8910
			public bool bIPRet;

			// Token: 0x040022CF RID: 8911
			public int iEnableChanNum;

			// Token: 0x040022D0 RID: 8912
			public bool bDVRLocalAllRec;

			// Token: 0x040022D1 RID: 8913
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public int[] lVoiceCom;

			// Token: 0x040022D2 RID: 8914
			public int lFirstEnableChanIndex;

			// Token: 0x040022D3 RID: 8915
			public int lTranHandle;

			// Token: 0x040022D4 RID: 8916
			public byte byZeroChanNum;

			// Token: 0x040022D5 RID: 8917
			public byte byMainProto;

			// Token: 0x040022D6 RID: 8918
			public byte bySubProto;

			// Token: 0x040022D7 RID: 8919
			public byte bySupport;

			// Token: 0x040022D8 RID: 8920
			public byte byStartDTalkChan;

			// Token: 0x040022D9 RID: 8921
			public byte byAudioInputChanNum;

			// Token: 0x040022DA RID: 8922
			public byte byStartAudioInputChanNo;

			// Token: 0x040022DB RID: 8923
			public byte byLanguageType;

			// Token: 0x040022DC RID: 8924
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.Struct)]
			public AcsDemoPublic.STRU_CHANNEL_INFO[] pStruChanInfo;

			// Token: 0x040022DD RID: 8925
			public CHCNetSDK.NET_DVR_IPPARACFG_V40[] pStruIPParaCfgV40;

			// Token: 0x040022DE RID: 8926
			public CHCNetSDK.NET_DVR_IPALARMINCFG struAlarmInCfg;

			// Token: 0x040022DF RID: 8927
			public CHCNetSDK.NET_DVR_IPALARMINCFG_V40 pStruIPAlarmInCfgV40;

			// Token: 0x040022E0 RID: 8928
			public CHCNetSDK.NET_DVR_IPALARMOUTCFG_V40 pStruIPAlarmOutCfgV40;

			// Token: 0x040022E1 RID: 8929
			public CHCNetSDK.NET_DVR_IPALARMOUTCFG struAlarmOutCfg;

			// Token: 0x040022E2 RID: 8930
			public IntPtr pNext;

			// Token: 0x040022E3 RID: 8931
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public AcsDemoPublic.STRU_CHANNEL_INFO[] struZeroChan;

			// Token: 0x040022E4 RID: 8932
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string sSecretKey;

			// Token: 0x040022E5 RID: 8933
			public int iAudioEncType;

			// Token: 0x040022E6 RID: 8934
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.Struct)]
			public AcsDemoPublic.PASSIVEDECODE_CHANINFO[] struPassiveDecode;

			// Token: 0x040022E7 RID: 8935
			public byte bySupport1;

			// Token: 0x040022E8 RID: 8936
			public byte bySupport2;

			// Token: 0x040022E9 RID: 8937
			public byte byStartIPAlarmOutNo;

			// Token: 0x040022EA RID: 8938
			public byte byMirrorChanNum;

			// Token: 0x040022EB RID: 8939
			public ushort wStartMirrorChanNo;

			// Token: 0x040022EC RID: 8940
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
			public AcsDemoPublic.STRU_CHANNEL_INFO[] struMirrorChan;

			// Token: 0x040022ED RID: 8941
			public byte bySupport5;

			// Token: 0x040022EE RID: 8942
			public byte bySupport7;

			// Token: 0x040022EF RID: 8943
			public byte byCharaterEncodeType;

			// Token: 0x040022F0 RID: 8944
			public bool bInit;

			// Token: 0x040022F1 RID: 8945
			public byte byPanelNo;

			// Token: 0x040022F2 RID: 8946
			public int iDoorNum;

			// Token: 0x040022F3 RID: 8947
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 768, ArraySubType = UnmanagedType.I1)]
			public int[] iDoorStatus;

			// Token: 0x040022F4 RID: 8948
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.ByValTStr)]
			public string[] sDoorName;
		}
	}
}
