using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000211 RID: 529
	internal enum WGPacket_Type_internal : byte
	{
		// Token: 0x04001BD4 RID: 7124
		OP_BASIC = 32,
		// Token: 0x04001BD5 RID: 7125
		OP_BATCH_SSI_FLASH = 34,
		// Token: 0x04001BD6 RID: 7126
		OP_CONFIG = 36,
		// Token: 0x04001BD7 RID: 7127
		OP_CPU_CONFIG,
		// Token: 0x04001BD8 RID: 7128
		OP_NONE,
		// Token: 0x04001BD9 RID: 7129
		OP_PRIVILEGE = 35,
		// Token: 0x04001BDA RID: 7130
		OP_SSI_FLASH = 33
	}
}
