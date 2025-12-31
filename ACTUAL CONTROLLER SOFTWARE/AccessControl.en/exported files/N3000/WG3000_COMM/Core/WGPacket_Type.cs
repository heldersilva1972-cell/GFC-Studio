using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000210 RID: 528
	public enum WGPacket_Type : byte
	{
		// Token: 0x04001BCC RID: 7116
		OP_BASIC = 32,
		// Token: 0x04001BCD RID: 7117
		OP_BATCH_SSI_FLASH = 34,
		// Token: 0x04001BCE RID: 7118
		OP_CONFIG = 36,
		// Token: 0x04001BCF RID: 7119
		OP_CPU_CONFIG,
		// Token: 0x04001BD0 RID: 7120
		OP_NONE,
		// Token: 0x04001BD1 RID: 7121
		OP_PRIVILEGE = 35,
		// Token: 0x04001BD2 RID: 7122
		OP_SSI_FLASH = 33
	}
}
