using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001E1 RID: 481
	public enum OP_SSI_FLASH_Code : byte
	{
		// Token: 0x040019D4 RID: 6612
		OP_END = 255,
		// Token: 0x040019D5 RID: 6613
		OP_READ = 16,
		// Token: 0x040019D6 RID: 6614
		OP_READ_FAIL = 18,
		// Token: 0x040019D7 RID: 6615
		OP_READ_OK = 17,
		// Token: 0x040019D8 RID: 6616
		OP_WRITE = 32,
		// Token: 0x040019D9 RID: 6617
		OP_WRITE_FAIL = 34,
		// Token: 0x040019DA RID: 6618
		OP_WRITE_OK = 33,
		// Token: 0x040019DB RID: 6619
		OP_WRITE_PARAM = 48,
		// Token: 0x040019DC RID: 6620
		OP_WRITE_PARAM_FAIL = 50,
		// Token: 0x040019DD RID: 6621
		OP_WRITE_PARAM_OK = 49
	}
}
