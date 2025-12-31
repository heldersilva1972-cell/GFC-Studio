using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001D1 RID: 465
	public class GZipFileInfo
	{
		// Token: 0x06000A6A RID: 2666 RVA: 0x000E4F00 File Offset: 0x000E3F00
		public bool ParseFileInfo(string fileInfo)
		{
			bool flag = false;
			try
			{
				if (!string.IsNullOrEmpty(fileInfo))
				{
					string[] array = fileInfo.Split(new char[] { ',' });
					if (array != null && array.Length == 4)
					{
						this.Index = Convert.ToInt32(array[0]);
						this.RelativePath = array[1].Replace("/", "\\");
						this.ModifiedDate = Convert.ToDateTime(array[2]);
						this.Length = Convert.ToInt32(array[3]);
						flag = true;
					}
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x040018E1 RID: 6369
		public bool AddedToTempFile;

		// Token: 0x040018E2 RID: 6370
		public string Folder;

		// Token: 0x040018E3 RID: 6371
		public int Index;

		// Token: 0x040018E4 RID: 6372
		public int Length;

		// Token: 0x040018E5 RID: 6373
		public string LocalPath;

		// Token: 0x040018E6 RID: 6374
		public DateTime ModifiedDate;

		// Token: 0x040018E7 RID: 6375
		public string RelativePath;

		// Token: 0x040018E8 RID: 6376
		public bool Restored;

		// Token: 0x040018E9 RID: 6377
		public bool RestoreRequested;
	}
}
