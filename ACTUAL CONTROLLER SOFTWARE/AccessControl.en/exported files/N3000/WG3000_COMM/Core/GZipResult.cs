using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001D2 RID: 466
	public class GZipResult
	{
		// Token: 0x040018EA RID: 6378
		public int CompressionPercent;

		// Token: 0x040018EB RID: 6379
		public bool Errors;

		// Token: 0x040018EC RID: 6380
		public int FileCount;

		// Token: 0x040018ED RID: 6381
		public GZipFileInfo[] Files;

		// Token: 0x040018EE RID: 6382
		public string TempFile;

		// Token: 0x040018EF RID: 6383
		public bool TempFileDeleted;

		// Token: 0x040018F0 RID: 6384
		public long TempFileSize;

		// Token: 0x040018F1 RID: 6385
		public string ZipFile;

		// Token: 0x040018F2 RID: 6386
		public long ZipFileSize;
	}
}
