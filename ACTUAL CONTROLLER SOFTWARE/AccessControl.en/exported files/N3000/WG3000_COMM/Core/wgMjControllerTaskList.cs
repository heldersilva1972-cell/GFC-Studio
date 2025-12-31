using System;
using System.Collections;

namespace WG3000_COMM.Core
{
	// Token: 0x020001F7 RID: 503
	public class wgMjControllerTaskList
	{
		// Token: 0x06000E10 RID: 3600 RVA: 0x001064D0 File Offset: 0x001054D0
		public wgMjControllerTaskList()
		{
			this.m_data = new byte[4096];
			this.arrTaskList = new ArrayList();
			this.Format();
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x001064FC File Offset: 0x001054FC
		public wgMjControllerTaskList(byte[] byt4K)
		{
			this.m_data = new byte[4096];
			this.arrTaskList = new ArrayList();
			this.Format();
			byte[] array = new byte[MjControlTaskItem.byteLen];
			if (byt4K.Length == 4096)
			{
				int num = 0;
				while (num + MjControlTaskItem.byteLen < byt4K.Length)
				{
					if (wgTools.IsAllFF(byt4K, num, MjControlTaskItem.byteLen))
					{
						return;
					}
					Array.Copy(byt4K, num, array, 0, MjControlTaskItem.byteLen);
					MjControlTaskItem mjControlTaskItem = new MjControlTaskItem(array);
					this.AddItem(mjControlTaskItem);
					num += MjControlTaskItem.byteLen;
				}
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0010658C File Offset: 0x0010558C
		public int AddItem(MjControlTaskItem mjCI)
		{
			if (this.arrTaskList.Count >= wgMjControllerTaskList.MaxTasksNum)
			{
				return -1;
			}
			if (this.arrTaskList.Count != 0)
			{
				int num = 0;
				foreach (object obj in this.arrTaskList)
				{
					if (BitConverter.ToString((obj as MjControlTaskItem).ToBytes()) == BitConverter.ToString(mjCI.ToBytes()))
					{
						return 0;
					}
					if ((obj as MjControlTaskItem).hms > mjCI.hms)
					{
						this.arrTaskList.Insert(num, mjCI);
						return 1;
					}
					num++;
				}
				this.arrTaskList.Add(mjCI);
				return 1;
			}
			this.arrTaskList.Add(mjCI);
			return 1;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00106674 File Offset: 0x00105674
		public void Clear()
		{
			this.Format();
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0010667C File Offset: 0x0010567C
		private void Format()
		{
			for (int i = 0; i < this.m_data.Length; i++)
			{
				this.m_data[i] = byte.MaxValue;
			}
			this.arrTaskList.Clear();
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x001066B4 File Offset: 0x001056B4
		public byte[] ToByte()
		{
			for (int i = 0; i < this.m_data.Length; i++)
			{
				this.m_data[i] = byte.MaxValue;
			}
			if (this.arrTaskList.Count > 0)
			{
				int num = 0;
				foreach (object obj in this.arrTaskList)
				{
					(obj as MjControlTaskItem).ToBytes().CopyTo(this.m_data, num);
					num += MjControlTaskItem.byteLen;
				}
			}
			return this.m_data;
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0010675C File Offset: 0x0010575C
		public static int MaxTasksNum
		{
			get
			{
				return 400;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x00106763 File Offset: 0x00105763
		public int taskCount
		{
			get
			{
				return this.arrTaskList.Count;
			}
		}

		// Token: 0x04001B67 RID: 7015
		private ArrayList arrTaskList;

		// Token: 0x04001B68 RID: 7016
		private byte[] m_data;
	}
}
