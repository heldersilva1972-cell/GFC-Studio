using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001DE RID: 478
	public class MjRegisterCardsParam
	{
		// Token: 0x06000B21 RID: 2849 RVA: 0x000EA60C File Offset: 0x000E960C
		public void updateParam(byte[] wgpktData, int startIndex)
		{
			this.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR = BitConverter.ToUInt32(wgpktData, startIndex);
			int num = startIndex + 4;
			this.newPrivilegePage4KAddr = BitConverter.ToUInt32(wgpktData, num);
			num += 4;
			this.freeNewPrivilegePageAddr = BitConverter.ToUInt32(wgpktData, num);
			num += 4;
			this.bOrderInfreePrivilegePage = BitConverter.ToUInt32(wgpktData, num);
			num += 4;
			this.totalPrivilegeCount = BitConverter.ToUInt32(wgpktData, num);
			num += 4;
			this.deletedPrivilegeCount = BitConverter.ToUInt32(wgpktData, num);
			num += 4;
			uint num2 = BitConverter.ToUInt32(wgpktData, num);
			num += 4;
			uint num3 = BitConverter.ToUInt32(wgpktData, num);
			if (num3 >= 819200U)
			{
				num3 = 819200U;
			}
			for (uint num4 = num2; num4 < num3; num4 += 4096U)
			{
				num += 4;
				if (num >= wgpktData.Length)
				{
					return;
				}
				this.minPrivilegeOf4K[(int)((UIntPtr)(num4 >> 12))] = BitConverter.ToUInt32(wgpktData, num);
				num += 4;
				if (num >= wgpktData.Length)
				{
					return;
				}
				this.maxPrivilegeOf4K[(int)((UIntPtr)(num4 >> 12))] = BitConverter.ToUInt32(wgpktData, num);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x000EA6EF File Offset: 0x000E96EF
		public uint MaxPrivilegesNum
		{
			get
			{
				if (!wgTools.gADCT_internal)
				{
					return 43000U;
				}
				return 200000U;
			}
		}

		// Token: 0x040019A2 RID: 6562
		public uint bOrderInfreePrivilegePage;

		// Token: 0x040019A3 RID: 6563
		public uint deletedPrivilegeCount;

		// Token: 0x040019A4 RID: 6564
		public uint freeNewPrivilegePageAddr;

		// Token: 0x040019A5 RID: 6565
		public uint iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR;

		// Token: 0x040019A6 RID: 6566
		public uint[] maxPrivilegeOf4K = new uint[200];

		// Token: 0x040019A7 RID: 6567
		public uint[] minPrivilegeOf4K = new uint[200];

		// Token: 0x040019A8 RID: 6568
		public uint newPrivilegePage4KAddr;

		// Token: 0x040019A9 RID: 6569
		public uint totalPrivilegeCount;
	}
}
