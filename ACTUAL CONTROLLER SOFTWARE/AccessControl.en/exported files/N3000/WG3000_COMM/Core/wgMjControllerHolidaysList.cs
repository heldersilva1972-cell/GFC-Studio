using System;
using System.Collections;

namespace WG3000_COMM.Core
{
	// Token: 0x020001F6 RID: 502
	public class wgMjControllerHolidaysList
	{
		// Token: 0x06000E08 RID: 3592 RVA: 0x00106233 File Offset: 0x00105233
		public wgMjControllerHolidaysList()
		{
			this.m_data = new byte[4096];
			this.arrHolidayList = new ArrayList();
			this.Format();
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0010625C File Offset: 0x0010525C
		public wgMjControllerHolidaysList(byte[] byt4K)
		{
			this.m_data = new byte[4096];
			this.arrHolidayList = new ArrayList();
			this.Format();
			byte[] array = new byte[MjControlHolidayTime.byteLen];
			if (byt4K.Length == 4096)
			{
				int num = 0;
				while (num + MjControlHolidayTime.byteLen < byt4K.Length)
				{
					if (wgTools.IsAllFF(byt4K, num, MjControlHolidayTime.byteLen))
					{
						return;
					}
					Array.Copy(byt4K, num, array, 0, MjControlHolidayTime.byteLen);
					MjControlHolidayTime mjControlHolidayTime = new MjControlHolidayTime(array);
					this.AddItem(mjControlHolidayTime);
					num += MjControlHolidayTime.byteLen;
				}
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x001062EC File Offset: 0x001052EC
		public int AddItem(MjControlHolidayTime mjCHT)
		{
			if (this.arrHolidayList.Count >= wgMjControllerHolidaysList.MaxHolidayNum)
			{
				return -1;
			}
			if (this.arrHolidayList.Count != 0)
			{
				int num = 0;
				foreach (object obj in this.arrHolidayList)
				{
					if (BitConverter.ToString((obj as MjControlHolidayTime).ToBytes()) == BitConverter.ToString(mjCHT.ToBytes()))
					{
						return 0;
					}
					if ((obj as MjControlHolidayTime).dtStart > mjCHT.dtStart)
					{
						this.arrHolidayList.Insert(num, mjCHT);
						return 1;
					}
					num++;
				}
				this.arrHolidayList.Add(mjCHT);
				return 1;
			}
			this.arrHolidayList.Add(mjCHT);
			return 1;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x001063D4 File Offset: 0x001053D4
		public void Clear()
		{
			this.Format();
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x001063DC File Offset: 0x001053DC
		private void Format()
		{
			for (int i = 0; i < this.m_data.Length; i++)
			{
				this.m_data[i] = byte.MaxValue;
			}
			this.arrHolidayList.Clear();
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00106414 File Offset: 0x00105414
		public byte[] ToByte()
		{
			for (int i = 0; i < this.m_data.Length; i++)
			{
				this.m_data[i] = byte.MaxValue;
			}
			if (this.arrHolidayList.Count > 0)
			{
				int num = 0;
				foreach (object obj in this.arrHolidayList)
				{
					(obj as MjControlHolidayTime).ToBytes().CopyTo(this.m_data, num);
					num += MjControlHolidayTime.byteLen;
				}
			}
			return this.m_data;
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x001064BC File Offset: 0x001054BC
		public int holidayCount
		{
			get
			{
				return this.arrHolidayList.Count;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x001064C9 File Offset: 0x001054C9
		public static int MaxHolidayNum
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x04001B65 RID: 7013
		private ArrayList arrHolidayList;

		// Token: 0x04001B66 RID: 7014
		private byte[] m_data;
	}
}
