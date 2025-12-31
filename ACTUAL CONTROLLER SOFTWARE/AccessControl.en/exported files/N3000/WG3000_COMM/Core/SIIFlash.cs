using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001E3 RID: 483
	internal class SIIFlash
	{
		// Token: 0x06000B28 RID: 2856 RVA: 0x000EA72C File Offset: 0x000E972C
		public SIIFlash()
		{
			for (int i = 0; i < this.data.Length; i++)
			{
				this.data[i] = byte.MaxValue;
			}
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x000EA76F File Offset: 0x000E976F
		public byte getData(long addr)
		{
			if (addr < (long)this.data.Length)
			{
				return this.data[(int)((IntPtr)addr)];
			}
			return byte.MaxValue;
		}

		// Token: 0x040019EC RID: 6636
		public byte[] data = new byte[8290304];
	}
}
