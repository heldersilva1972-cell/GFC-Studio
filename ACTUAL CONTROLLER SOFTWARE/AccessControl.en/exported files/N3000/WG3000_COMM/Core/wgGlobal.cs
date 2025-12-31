using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WG3000_COMM.Core
{
	// Token: 0x020001F0 RID: 496
	internal class wgGlobal
	{
		// Token: 0x06000C69 RID: 3177 RVA: 0x000FA316 File Offset: 0x000F9316
		private wgGlobal()
		{
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x000FA31E File Offset: 0x000F931E
		public static int ERR_PRIVILEGES_OVER200K
		{
			get
			{
				return -100001;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x000FA325 File Offset: 0x000F9325
		public static int ERR_PRIVILEGES_STOPUPLOAD
		{
			get
			{
				return -100002;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x000FA32C File Offset: 0x000F932C
		public static int ERR_SWIPERECORD_STOPGET
		{
			get
			{
				return -200002;
			}
		}

		// Token: 0x04001AAF RID: 6831
		public const int CONTROLLERID_MAX_PARTITIONNUM = 999;

		// Token: 0x04001AB0 RID: 6832
		public const string ExtendFunction_Password = "5678";

		// Token: 0x04001AB1 RID: 6833
		public const int MaxPrivilegeCount_Stat = 2000000;

		// Token: 0x04001AB2 RID: 6834
		public const int Param_ActiveAntibackShare = 62;

		// Token: 0x04001AB3 RID: 6835
		public const int Param_ActiveFireSignalShare = 60;

		// Token: 0x04001AB4 RID: 6836
		public const int Param_ActiveFlaseBoard = 64;

		// Token: 0x04001AB5 RID: 6837
		public const int Param_ActiveInterlockShare = 61;

		// Token: 0x04001AB6 RID: 6838
		public const int Param_CommonDoorControlConsumers = 154;

		// Token: 0x04001AB7 RID: 6839
		public const int Param_ConsumerUpdateLog = 50;

		// Token: 0x04001AB8 RID: 6840
		public const int Param_CreatedPartition = 53;

		// Token: 0x04001AB9 RID: 6841
		public const int Param_EarliestTimeAsOndutyForNormalAttendance = 56;

		// Token: 0x04001ABA RID: 6842
		public const int Param_FullAttendanceSpecialA = 210;

		// Token: 0x04001ABB RID: 6843
		public const int Param_InvalidSwipeNotAsAttend = 54;

		// Token: 0x04001ABC RID: 6844
		public const int Param_MoreCardShiftOneUser = 153;

		// Token: 0x04001ABD RID: 6845
		public const int Param_NormalWorkTime = 58;

		// Token: 0x04001ABE RID: 6846
		public const int Param_OffDutyLatestTimeForNormalAttendance = 55;

		// Token: 0x04001ABF RID: 6847
		public const int Param_OnlyOnDutyForNormalAttendance = 59;

		// Token: 0x04001AC0 RID: 6848
		public const int Param_OnlyTwoTimesForNormalAttendance = 57;

		// Token: 0x04001AC1 RID: 6849
		public const int Param_PatrolAbsentTimeout = 27;

		// Token: 0x04001AC2 RID: 6850
		public const int Param_PatrolAllowTimeout = 28;

		// Token: 0x04001AC3 RID: 6851
		public const int Param_PrivilegeCountByControllerLog = 51;

		// Token: 0x04001AC4 RID: 6852
		public const int Param_PrivilegeTotalLog = 52;

		// Token: 0x04001AC5 RID: 6853
		public const int ParamLoc_ActivateAccessKeypad = 123;

		// Token: 0x04001AC6 RID: 6854
		public const int ParamLoc_ActivateAntiPassBack = 132;

		// Token: 0x04001AC7 RID: 6855
		public const int ParamLoc_ActivateAutoDelayDeactivateDate = 171;

		// Token: 0x04001AC8 RID: 6856
		public const int ParamLoc_ActivateCameraManage = 156;

		// Token: 0x04001AC9 RID: 6857
		public const int ParamLoc_ActivateCard19 = 192;

		// Token: 0x04001ACA RID: 6858
		public const int ParamLoc_ActivateCheckAccessDoorMoreCards = 202;

		// Token: 0x04001ACB RID: 6859
		public const int ParamLoc_ActivateCheckDiskSpace = 204;

		// Token: 0x04001ACC RID: 6860
		public const int ParamLoc_ActivateConstMeal = 150;

		// Token: 0x04001ACD RID: 6861
		public const int ParamLoc_ActivateControllerTaskList = 131;

		// Token: 0x04001ACE RID: 6862
		public const int ParamLoc_ActivateControllerZone = 125;

		// Token: 0x04001ACF RID: 6863
		public const int ParamLoc_ActivateCreateQRCode = 195;

		// Token: 0x04001AD0 RID: 6864
		public const int ParamLoc_ActivateDisplayCertAndTel = 197;

		// Token: 0x04001AD1 RID: 6865
		public const int ParamLoc_ActivateDisplayCertID = 187;

		// Token: 0x04001AD2 RID: 6866
		public const int ParamLoc_ActivateDisplayYellowWhenDoorOpen = 173;

		// Token: 0x04001AD3 RID: 6867
		public const int ParamLoc_ActivateDontAutoLoadPrivileges = 142;

		// Token: 0x04001AD4 RID: 6868
		public const int ParamLoc_ActivateDontAutoLoadSwipeRecords = 143;

		// Token: 0x04001AD5 RID: 6869
		public const int ParamLoc_ActivateDontDisplayAccessControl = 111;

		// Token: 0x04001AD6 RID: 6870
		public const int ParamLoc_ActivateDontDisplayAttendance = 112;

		// Token: 0x04001AD7 RID: 6871
		public const int ParamLoc_ActivateDontDisplayDoorStatusRecords = 163;

		// Token: 0x04001AD8 RID: 6872
		public const int ParamLoc_ActivateDontDisplayOneCardMultifunction = 217;

		// Token: 0x04001AD9 RID: 6873
		public const int ParamLoc_ActivateDontDisplayRebootRecords = 164;

		// Token: 0x04001ADA RID: 6874
		public const int ParamLoc_ActivateDoorAsSwitch = 146;

		// Token: 0x04001ADB RID: 6875
		public const int ParamLoc_ActivateElevator = 144;

		// Token: 0x04001ADC RID: 6876
		public const int ParamLoc_ActivateFaceIDCheckInput = 201;

		// Token: 0x04001ADD RID: 6877
		public const int ParamLoc_ActivateFaceManagement = 186;

		// Token: 0x04001ADE RID: 6878
		public const int ParamLoc_ActivateFingerprintManagement = 188;

		// Token: 0x04001ADF RID: 6879
		public const int ParamLoc_ActivateFingerprintSuperCard = 189;

		// Token: 0x04001AE0 RID: 6880
		public const int ParamLoc_ActivateFirstCardOpen = 135;

		// Token: 0x04001AE1 RID: 6881
		public const int ParamLoc_ActivateGlobalAntiBackOpen = 181;

		// Token: 0x04001AE2 RID: 6882
		public const int ParamLoc_ActivatehideCardNOToolStripMenuItem = 219;

		// Token: 0x04001AE3 RID: 6883
		public const int ParamLoc_ActivateHouse = 145;

		// Token: 0x04001AE4 RID: 6884
		public const int ParamLoc_ActivateImportExtern = 198;

		// Token: 0x04001AE5 RID: 6885
		public const int ParamLoc_ActivateInterLock = 133;

		// Token: 0x04001AE6 RID: 6886
		public const int ParamLoc_ActivateLocate = 161;

		// Token: 0x04001AE7 RID: 6887
		public const int ParamLoc_ActivateLogQuery = 103;

		// Token: 0x04001AE8 RID: 6888
		public const int ParamLoc_ActivateMaps = 114;

		// Token: 0x04001AE9 RID: 6889
		public const int ParamLoc_ActivateMeeting = 149;

		// Token: 0x04001AEA RID: 6890
		public const int ParamLoc_ActivateMobileAsCardInput = 152;

		// Token: 0x04001AEB RID: 6891
		public const int ParamLoc_ActivateMultiCardAccess = 134;

		// Token: 0x04001AEC RID: 6892
		public const int ParamLoc_ActivateNBIOT_P64 = 203;

		// Token: 0x04001AED RID: 6893
		public const int ParamLoc_ActivateOpenInvalidCardMoreTimesWarn = 211;

		// Token: 0x04001AEE RID: 6894
		public const int ParamLoc_ActivateOpenTooLongWarn = 180;

		// Token: 0x04001AEF RID: 6895
		public const int ParamLoc_ActivateOperatorManagement = 148;

		// Token: 0x04001AF0 RID: 6896
		public const int ParamLoc_ActivateOtherShiftSchedule = 113;

		// Token: 0x04001AF1 RID: 6897
		public const int ParamLoc_ActivatePatrol = 151;

		// Token: 0x04001AF2 RID: 6898
		public const int ParamLoc_ActivatePCCheckAccess = 137;

		// Token: 0x04001AF3 RID: 6899
		public const int ParamLoc_ActivatePCCheckMealOpen = 169;

		// Token: 0x04001AF4 RID: 6900
		public const int ParamLoc_ActivatePeripheralControl = 124;

		// Token: 0x04001AF5 RID: 6901
		public const int ParamLoc_ActivatePersonInside = 162;

		// Token: 0x04001AF6 RID: 6902
		public const int ParamLoc_ActivateRemoteControlCloudServer = 205;

		// Token: 0x04001AF7 RID: 6903
		public const int ParamLoc_ActivateRemoteOpenDoor = 122;

		// Token: 0x04001AF8 RID: 6904
		public const int ParamLoc_ActivateRentingHouse = 178;

		// Token: 0x04001AF9 RID: 6905
		public const int ParamLoc_ActivateSecondLevelSecurity = 220;

		// Token: 0x04001AFA RID: 6906
		public const int ParamLoc_ActivateSFZReader = 168;

		// Token: 0x04001AFB RID: 6907
		public const int ParamLoc_ActivateSFZReader_IDAsCardNO = 194;

		// Token: 0x04001AFC RID: 6908
		public const int ParamLoc_ActivateSoftwareWarnAutoResetWhenAllDoorAreClosed = 216;

		// Token: 0x04001AFD RID: 6909
		public const int ParamLoc_ActivateSpSN = 190;

		// Token: 0x04001AFE RID: 6910
		public const int ParamLoc_ActivateSwipeFourTimeSetNormalOpen = 170;

		// Token: 0x04001AFF RID: 6911
		public const int ParamLoc_ActivateTimeProfile = 121;

		// Token: 0x04001B00 RID: 6912
		public const int ParamLoc_ActivateTimeSecond = 191;

		// Token: 0x04001B01 RID: 6913
		public const int ParamLoc_ActivateTimeSegLimittedAccess = 136;

		// Token: 0x04001B02 RID: 6914
		public const int ParamLoc_ActivateTwoCardCheck = 200;

		// Token: 0x04001B03 RID: 6915
		public const int ParamLoc_ActivateValidSwipeGap = 147;

		// Token: 0x04001B04 RID: 6916
		public const int ParamLoc_Activatewait60SecondsWhenInputWrongPasswords5TimesToolStripMenuItem = 218;

		// Token: 0x04001B05 RID: 6917
		public const int ParamLoc_ActivateWarnForceWithCard = 141;

		// Token: 0x04001B06 RID: 6918
		public const int ParamLoc_ActiveAccelerator = 166;

		// Token: 0x04001B07 RID: 6919
		public const int ParamLoc_ActivityPrivilegeTypeManagementMode = 167;

		// Token: 0x04001B08 RID: 6920
		public const int ParamLoc_AntiBack_Door4Exit = 183;

		// Token: 0x04001B09 RID: 6921
		public const int ParamLoc_AntiBack_FirstInThenOut = 184;

		// Token: 0x04001B0A RID: 6922
		public const int ParamLoc_AntiBack_LastQueryRecID = 182;

		// Token: 0x04001B0B RID: 6923
		public const int ParamLoc_AntiBack_PC_LimitedPersons = 213;

		// Token: 0x04001B0C RID: 6924
		public const int ParamLoc_AntiBack_RemoteOpen_Direction = 185;

		// Token: 0x04001B0D RID: 6925
		public const int ParamLoc_AntiBack_TimeSegment = 212;

		// Token: 0x04001B0E RID: 6926
		public const int ParamLoc_AsFullAttendance = 174;

		// Token: 0x04001B0F RID: 6927
		public const int ParamLoc_CapturePhotoDelaySeconds = 207;

		// Token: 0x04001B10 RID: 6928
		public const int ParamLoc_DatabaseSubversion = 199;

		// Token: 0x04001B11 RID: 6929
		public const int ParamLoc_DeactivateElevatorNO = 208;

		// Token: 0x04001B12 RID: 6930
		public const int ParamLoc_DisableReaderLEDBEEPEROutputWhenInvalidCard = 165;

		// Token: 0x04001B13 RID: 6931
		public const int ParamLoc_DoorAsSwitch_Option = 214;

		// Token: 0x04001B14 RID: 6932
		public const int ParamLoc_FaceDeviceType = 209;

		// Token: 0x04001B15 RID: 6933
		public const int ParamLoc_Get_Swipe_Records_Continuously = 177;

		// Token: 0x04001B16 RID: 6934
		public const int ParamLoc_HIDE_RESTOREDATABASE = 224;

		// Token: 0x04001B17 RID: 6935
		public const int ParamLoc_HTTPServerConfiguration = 196;

		// Token: 0x04001B18 RID: 6936
		public const int ParamLoc_InvalidSwipeWarnNeedMoreSwipe = 206;

		// Token: 0x04001B19 RID: 6937
		public const int ParamLoc_ManualRecordAsFullAttendance = 175;

		// Token: 0x04001B1A RID: 6938
		public const int ParamLoc_MOBILE_WEB_AUTOIP_DISABLE = 155;

		// Token: 0x04001B1B RID: 6939
		public const int ParamLoc_NormalShiftABC = 215;

		// Token: 0x04001B1C RID: 6940
		public const int ParamLoc_OldBoardInfo = 221;

		// Token: 0x04001B1D RID: 6941
		public const int ParamLoc_OldBoardInfoRSA = 222;

		// Token: 0x04001B1E RID: 6942
		public const int ParamLoc_PhotoNameFromConsumerNO = 193;

		// Token: 0x04001B1F RID: 6943
		public const int ParamLoc_RecordButtonEvent = 101;

		// Token: 0x04001B20 RID: 6944
		public const int ParamLoc_RecordDoorStatusEvent = 102;

		// Token: 0x04001B21 RID: 6945
		public const int ParamLoc_SMSConfiguration = 179;

		// Token: 0x04001B22 RID: 6946
		public const int ParamLoc_SN_RSA = 223;

		// Token: 0x04001B23 RID: 6947
		public const int ParamLoc_Video_AutoAdjustTimeOfVideo = 158;

		// Token: 0x04001B24 RID: 6948
		public const int ParamLoc_Video_DisableJPEGCaptureMode = 176;

		// Token: 0x04001B25 RID: 6949
		public const int ParamLoc_Video_JPEGQuality = 160;

		// Token: 0x04001B26 RID: 6950
		public const int ParamLoc_Video_JPEGResolution = 159;

		// Token: 0x04001B27 RID: 6951
		public const int ParamLoc_Video_OnlyCapturePhoto = 157;

		// Token: 0x04001B28 RID: 6952
		public const int ParamLoc_Video_OnlyDisplaySingle = 172;

		// Token: 0x04001B29 RID: 6953
		public const int TIMEOUT_TWOSWIPE_FOR_CHECK_INSIDE_BY_SWIPE = 20;

		// Token: 0x04001B2A RID: 6954
		public static int TRIGGER_EVENT_4ARM = 14336;

		// Token: 0x04001B2B RID: 6955
		public static int TRIGGER_SOURCE_4ARM = 16;

		// Token: 0x020001F1 RID: 497
		[SuppressUnmanagedCodeSecurity]
		internal static class SafeNativeMethods
		{
			// Token: 0x06000C6E RID: 3182
			[DllImport("iphlpapi.dll", ExactSpelling = true)]
			public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);
		}
	}
}
