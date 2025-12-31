using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001EB RID: 491
	internal class wgAppRunInfo
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000C48 RID: 3144 RVA: 0x000FA070 File Offset: 0x000F9070
		// (remove) Token: 0x06000C49 RID: 3145 RVA: 0x000FA0A4 File Offset: 0x000F90A4
		private static event wgAppRunInfo.appRunInfoCommStatusHandler appRunInfoCommStatus;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000C4A RID: 3146 RVA: 0x000FA0D8 File Offset: 0x000F90D8
		// (remove) Token: 0x06000C4B RID: 3147 RVA: 0x000FA10C File Offset: 0x000F910C
		private static event wgAppRunInfo.appRunInfoLoadNumHandler appRunInfoLoadNums;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000C4C RID: 3148 RVA: 0x000FA140 File Offset: 0x000F9140
		// (remove) Token: 0x06000C4D RID: 3149 RVA: 0x000FA174 File Offset: 0x000F9174
		private static event wgAppRunInfo.appRunInfoMonitorHandler appRunInfoMonitors;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000C4E RID: 3150 RVA: 0x000FA1A7 File Offset: 0x000F91A7
		// (remove) Token: 0x06000C4F RID: 3151 RVA: 0x000FA1AF File Offset: 0x000F91AF
		public static event wgAppRunInfo.appRunInfoCommStatusHandler evAppRunInfoCommStatus
		{
			add
			{
				wgAppRunInfo.appRunInfoCommStatus += value;
			}
			remove
			{
				wgAppRunInfo.appRunInfoCommStatus -= value;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000C50 RID: 3152 RVA: 0x000FA1B7 File Offset: 0x000F91B7
		// (remove) Token: 0x06000C51 RID: 3153 RVA: 0x000FA1BF File Offset: 0x000F91BF
		public static event wgAppRunInfo.appRunInfoLoadNumHandler evAppRunInfoLoadNum
		{
			add
			{
				wgAppRunInfo.appRunInfoLoadNums += value;
			}
			remove
			{
				wgAppRunInfo.appRunInfoLoadNums -= value;
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000C52 RID: 3154 RVA: 0x000FA1C7 File Offset: 0x000F91C7
		// (remove) Token: 0x06000C53 RID: 3155 RVA: 0x000FA1CF File Offset: 0x000F91CF
		public static event wgAppRunInfo.appRunInfoMonitorHandler evAppRunInfoMonitor
		{
			add
			{
				wgAppRunInfo.appRunInfoMonitors += value;
			}
			remove
			{
				wgAppRunInfo.appRunInfoMonitors -= value;
			}
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x000FA1D7 File Offset: 0x000F91D7
		public static void ClearAllDisplayedInfo()
		{
			wgAppRunInfo.raiseAppRunInfoLoadNums("");
			wgAppRunInfo.raiseAppRunInfoCommStatus("");
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x000FA1ED File Offset: 0x000F91ED
		public static void raiseAppRunInfoCommStatus(string info)
		{
			if (wgAppRunInfo.appRunInfoCommStatus != null)
			{
				wgAppRunInfo.appRunInfoCommStatus(info);
			}
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000FA201 File Offset: 0x000F9201
		public static void raiseAppRunInfoLoadNums(string info)
		{
			if (wgAppRunInfo.appRunInfoLoadNums != null)
			{
				wgAppRunInfo.appRunInfoLoadNums(info);
			}
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x000FA215 File Offset: 0x000F9215
		public static void raiseAppRunInfoMonitors(string info)
		{
			if (wgAppRunInfo.appRunInfoMonitors != null)
			{
				wgAppRunInfo.appRunInfoMonitors(info);
			}
		}

		// Token: 0x020001EC RID: 492
		// (Invoke) Token: 0x06000C5A RID: 3162
		public delegate void appRunInfoCommStatusHandler(string strCommStatus);

		// Token: 0x020001ED RID: 493
		// (Invoke) Token: 0x06000C5E RID: 3166
		public delegate void appRunInfoLoadNumHandler(string strNum);

		// Token: 0x020001EE RID: 494
		// (Invoke) Token: 0x06000C62 RID: 3170
		public delegate void appRunInfoMonitorHandler(string strNum);
	}
}
