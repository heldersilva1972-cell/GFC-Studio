using System;
using System.Collections;

namespace WG3000_COMM.Core
{
	// Token: 0x020001F8 RID: 504
	public class wgMjControllerTimeSegList
	{
		// Token: 0x06000E18 RID: 3608 RVA: 0x00106770 File Offset: 0x00105770
		public wgMjControllerTimeSegList()
		{
			this.Format();
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00106799 File Offset: 0x00105799
		public int AddItem(MjControlTimeSeg mjControlTimeSeg)
		{
			if (this.arrTaskList.Count <= 252)
			{
				this.arrTaskList.Add(mjControlTimeSeg);
				return 1;
			}
			return -1;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x001067BD File Offset: 0x001057BD
		public void Clear()
		{
			this.Format();
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x001067C8 File Offset: 0x001057C8
		private void Format()
		{
			for (int i = 0; i < this.m_data.Length; i++)
			{
				this.m_data[i] = byte.MaxValue;
			}
			this.arrTaskList.Clear();
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00106800 File Offset: 0x00105800
		public byte[] ToByte()
		{
			for (int i = 0; i < this.m_data.Length; i++)
			{
				this.m_data[i] = byte.MaxValue;
			}
			if (this.arrTaskList.Count > 0)
			{
				foreach (object obj in this.arrTaskList)
				{
					if ((obj as MjControlTimeSeg).SegIndex < 255)
					{
						(obj as MjControlTimeSeg).ToBytes().CopyTo(this.m_data, (int)(obj as MjControlTimeSeg).SegIndex * MjControlTimeSeg.byteLen);
					}
				}
			}
			return this.m_data;
		}

		// Token: 0x04001B69 RID: 7017
		private ArrayList arrTaskList = new ArrayList();

		// Token: 0x04001B6A RID: 7018
		private byte[] m_data = new byte[8192];
	}
}
