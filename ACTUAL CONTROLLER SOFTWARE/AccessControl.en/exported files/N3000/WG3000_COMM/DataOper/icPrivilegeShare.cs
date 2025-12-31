using System;
using System.Data;
using WG3000_COMM.Core;

namespace WG3000_COMM.DataOper
{
	// Token: 0x02000226 RID: 550
	internal class icPrivilegeShare
	{
		// Token: 0x0600100E RID: 4110 RVA: 0x001204F1 File Offset: 0x0011F4F1
		public static void setNeedRefresh()
		{
			icPrivilegeShare.m_privilegeTotal = -1;
			icPrivilegeShare.m_dvPrivilegeCount = null;
			wgAppConfig.setSystemParamValue(52, null, null, null);
			wgAppConfig.setSystemParamValue(51, null, null, null);
			icPrivilegeShare.bNeedRefresh = true;
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600100F RID: 4111 RVA: 0x0012051B File Offset: 0x0011F51B
		public static DataView dvPrivilegeCount
		{
			get
			{
				return icPrivilegeShare.m_dvPrivilegeCount;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x00120522 File Offset: 0x0011F522
		public static int privilegeTotal
		{
			get
			{
				return icPrivilegeShare.m_privilegeTotal;
			}
		}

		// Token: 0x04001C8A RID: 7306
		public static bool bNeedRefresh = true;

		// Token: 0x04001C8B RID: 7307
		private static DataView m_dvPrivilegeCount = null;

		// Token: 0x04001C8C RID: 7308
		private static int m_privilegeTotal = -1;
	}
}
