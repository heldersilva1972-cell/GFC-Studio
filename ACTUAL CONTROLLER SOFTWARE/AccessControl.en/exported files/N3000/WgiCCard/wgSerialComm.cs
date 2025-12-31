using System;

namespace WgiCCard
{
	// Token: 0x02000395 RID: 917
	public class wgSerialComm
	{
		// Token: 0x06002167 RID: 8551 RVA: 0x0027E761 File Offset: 0x0027D761
		public long ClosePort()
		{
			this.wgRs232.Close();
			return 0L;
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0027E770 File Offset: 0x0027D770
		private long msToTicks(long ms)
		{
			return Convert.ToInt64(Convert.ToInt32(ms * 1000L) * 10);
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x0027E788 File Offset: 0x0027D788
		public long OpenPort()
		{
			long num;
			try
			{
				this.wgRs232.ClearInputBuffer();
				this.wgRs232.Open((int)this.CommPort, (int)this.Baudrate, 8, Rs232.DataParity.Parity_None, Rs232.DataStopBit.StopBit_1, 4096);
				num = 0L;
			}
			catch (Exception)
			{
				num = -5L;
			}
			return num;
		}

		// Token: 0x040039C8 RID: 14792
		private const byte conCmdClearAllRecordwg = 137;

		// Token: 0x040039C9 RID: 14793
		private const byte conCmdClearCardInfo = 136;

		// Token: 0x040039CA RID: 14794
		private const byte conCmdClearCntTmseg = 135;

		// Token: 0x040039CB RID: 14795
		private const byte conCmdClearReadRecordwg = 138;

		// Token: 0x040039CC RID: 14796
		private const byte conCmdDelOneCardInfo = 147;

		// Token: 0x040039CD RID: 14797
		private const byte conCmdE2PROM = 53;

		// Token: 0x040039CE RID: 14798
		private const byte conCmdInsertOneCardInfo = 146;

		// Token: 0x040039CF RID: 14799
		private const byte conCmdMCUAppdataRead = 208;

		// Token: 0x040039D0 RID: 14800
		private const byte conCmdMCUAppdataWrite = 224;

		// Token: 0x040039D1 RID: 14801
		private const byte conCmdMCUMemRead = 199;

		// Token: 0x040039D2 RID: 14802
		private const byte conCmdMCUMemWrite = 198;

		// Token: 0x040039D3 RID: 14803
		private const byte conCmdMILoadKey = 177;

		// Token: 0x040039D4 RID: 14804
		private const byte conCmdMIReadBlock = 179;

		// Token: 0x040039D5 RID: 14805
		private const byte conCmdMIReadCardSnr = 178;

		// Token: 0x040039D6 RID: 14806
		private const byte conCmdMIWriteBlock = 180;

		// Token: 0x040039D7 RID: 14807
		private const byte conCmdModifyOneCardInfo = 144;

		// Token: 0x040039D8 RID: 14808
		private const byte conCmdModifyOneCntTmseg = 143;

		// Token: 0x040039D9 RID: 14809
		private const byte conCmdOpenDoor = 148;

		// Token: 0x040039DA RID: 14810
		private const byte conCmdOperateUserRight = 162;

		// Token: 0x040039DB RID: 14811
		private const byte conCmdQueryRunstatus = 161;

		// Token: 0x040039DC RID: 14812
		private const byte conCmdReadCardInfo = 132;

		// Token: 0x040039DD RID: 14813
		private const byte conCmdReadCntTmseg = 131;

		// Token: 0x040039DE RID: 14814
		private const byte conCmdReadProductInfo = 134;

		// Token: 0x040039DF RID: 14815
		private const byte conCmdReadRecordwg = 133;

		// Token: 0x040039E0 RID: 14816
		private const byte conCmdReadRunstatus = 129;

		// Token: 0x040039E1 RID: 14817
		private const byte conCmdReadWatchInfo = 130;

		// Token: 0x040039E2 RID: 14818
		private const byte conCmdSetControllerType = 149;

		// Token: 0x040039E3 RID: 14819
		private const byte conCmdSetDoorDelay = 140;

		// Token: 0x040039E4 RID: 14820
		private const byte conCmdSetTime = 139;

		// Token: 0x040039E5 RID: 14821
		private const byte conCmdSetTimeNew = 50;

		// Token: 0x040039E6 RID: 14822
		private const byte conCmdStopSend = 145;

		// Token: 0x040039E7 RID: 14823
		private const byte conCmdWriteCardInfo = 142;

		// Token: 0x040039E8 RID: 14824
		private const byte conCmdWriteCntTmseg = 141;

		// Token: 0x040039E9 RID: 14825
		private const byte conCntTmsegLength = 26;

		// Token: 0x040039EA RID: 14826
		private const byte conFillChar = 254;

		// Token: 0x040039EB RID: 14827
		private const byte conFlagBegin = 128;

		// Token: 0x040039EC RID: 14828
		private const byte conFlagEnd = 64;

		// Token: 0x040039ED RID: 14829
		private const byte conFrameDataLength = 26;

		// Token: 0x040039EE RID: 14830
		private const byte conFrameEnd = 13;

		// Token: 0x040039EF RID: 14831
		private const byte conFrameLength = 34;

		// Token: 0x040039F0 RID: 14832
		private const byte conFrameStart = 126;

		// Token: 0x040039F1 RID: 14833
		private const byte conOneCardInfoLength = 6;

		// Token: 0x040039F2 RID: 14834
		private const byte conRunstatusLength = 23;

		// Token: 0x040039F3 RID: 14835
		private const byte conShortFrameLength = 8;

		// Token: 0x040039F4 RID: 14836
		private const byte CRC16 = 0;

		// Token: 0x040039F5 RID: 14837
		private const byte CRC8 = 128;

		// Token: 0x040039F6 RID: 14838
		private const int ERROR_IO_INCOMPLETE = 996;

		// Token: 0x040039F7 RID: 14839
		private const int ERROR_IO_PENDING = 997;

		// Token: 0x040039F8 RID: 14840
		private const int FILE_FLAG_OVERLAPPED = 1073741824;

		// Token: 0x040039F9 RID: 14841
		private const int GENERIC_READ = -2147483648;

		// Token: 0x040039FA RID: 14842
		private const int GENERIC_WRITE = 1073741824;

		// Token: 0x040039FB RID: 14843
		private const byte HF = 64;

		// Token: 0x040039FC RID: 14844
		private const int INFINITE = -1;

		// Token: 0x040039FD RID: 14845
		private const int INVALID_HANDLE_VALUE = -1;

		// Token: 0x040039FE RID: 14846
		private const int IO_BUFFER_SIZE = 1024;

		// Token: 0x040039FF RID: 14847
		private const byte KEYA = 0;

		// Token: 0x04003A00 RID: 14848
		private const byte KEYB = 4;

		// Token: 0x04003A01 RID: 14849
		private const byte KEYSET0 = 0;

		// Token: 0x04003A02 RID: 14850
		private const byte KEYSET1 = 1;

		// Token: 0x04003A03 RID: 14851
		private const byte KEYSET2 = 2;

		// Token: 0x04003A04 RID: 14852
		private const int OPEN_EXISTING = 3;

		// Token: 0x04003A05 RID: 14853
		private const byte PROKAOQING = 1;

		// Token: 0x04003A06 RID: 14854
		private const byte PROMENJIN = 2;

		// Token: 0x04003A07 RID: 14855
		private const byte PROMENJIN2 = 3;

		// Token: 0x04003A08 RID: 14856
		private const byte PROXFJ_FAKA = 4;

		// Token: 0x04003A09 RID: 14857
		private const byte PROXFJ_PUTONG = 5;

		// Token: 0x04003A0A RID: 14858
		private const byte PROXFJ_TUIKUAN = 6;

		// Token: 0x04003A0B RID: 14859
		private const int PURGE_RXABORT = 2;

		// Token: 0x04003A0C RID: 14860
		private const int PURGE_RXCLEAR = 8;

		// Token: 0x04003A0D RID: 14861
		private const int PURGE_TXABORT = 1;

		// Token: 0x04003A0E RID: 14862
		private const int PURGE_TXCLEAR = 4;

		// Token: 0x04003A0F RID: 14863
		private const int WAIT_OBJECT_0 = 0;

		// Token: 0x04003A10 RID: 14864
		private const int WAIT_TIMEOUT = 258;

		// Token: 0x04003A11 RID: 14865
		private const int WGCOMM_BADPARM = -7;

		// Token: 0x04003A12 RID: 14866
		private const int WGCOMM_BADPORT = -1;

		// Token: 0x04003A13 RID: 14867
		private const int WGCOMM_BOARDDISABLED = -15;

		// Token: 0x04003A14 RID: 14868
		private const int WGCOMM_CERROR_BADPARM = -42;

		// Token: 0x04003A15 RID: 14869
		private const int WGCOMM_CERROR_CARD_ID = -41;

		// Token: 0x04003A16 RID: 14870
		private const int WGCOMM_CONTROLLER_NOEXIST = -19;

		// Token: 0x04003A17 RID: 14871
		private const int WGCOMM_FAIL = -53;

		// Token: 0x04003A18 RID: 14872
		private const int WGCOMM_FRAMEINVALID = -14;

		// Token: 0x04003A19 RID: 14873
		private const int WGCOMM_NOTREADRECORDWG = -17;

		// Token: 0x04003A1A RID: 14874
		private const int WGCOMM_OK = 0;

		// Token: 0x04003A1B RID: 14875
		private const int WGCOMM_OPENFAIL = -5;

		// Token: 0x04003A1C RID: 14876
		private const int WGCOMM_READTIMEOUT = -13;

		// Token: 0x04003A1D RID: 14877
		private const int WGCOMM_UNKNOWNFAIL = -16;

		// Token: 0x04003A1E RID: 14878
		private const int WGCOMM_WIN32FAIL = -8;

		// Token: 0x04003A1F RID: 14879
		private const int WGCOMM_WRITETIMEOUT = -12;

		// Token: 0x04003A20 RID: 14880
		private const int WGCOMM_WRONG_COMMAND = -45;

		// Token: 0x04003A21 RID: 14881
		public long Baudrate = 9600L;

		// Token: 0x04003A22 RID: 14882
		private byte[] bytOutBuffer = new byte[34];

		// Token: 0x04003A23 RID: 14883
		public byte CommPort = 1;

		// Token: 0x04003A24 RID: 14884
		public Rs232 wgRs232 = new Rs232();
	}
}
