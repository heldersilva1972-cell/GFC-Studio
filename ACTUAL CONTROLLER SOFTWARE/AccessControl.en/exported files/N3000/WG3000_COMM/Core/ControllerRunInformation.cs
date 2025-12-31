using System;

namespace WG3000_COMM.Core
{
	// Token: 0x020001C4 RID: 452
	public class ControllerRunInformation : wgMjControllerRunInformation
	{
		// Token: 0x06000998 RID: 2456 RVA: 0x000DEB48 File Offset: 0x000DDB48
		public void update(byte[] wgpktData, int startIndex, uint ControllerSN)
		{
			base.UpdateInfo(wgpktData, startIndex, ControllerSN);
			for (int i = 0; i < 10; i++)
			{
				if (this.newSwipes[i] == null)
				{
					this.newSwipes[i] = new MjRec(wgpktData, (uint)(startIndex - 20 + 68 + i * 20), ControllerSN, BitConverter.ToUInt32(wgpktData, startIndex - 20 + 64 + i * 20));
				}
				else
				{
					this.newSwipes[i].Update(wgpktData, (uint)(startIndex - 20 + 68 + i * 20), ControllerSN, BitConverter.ToUInt32(wgpktData, startIndex - 20 + 64 + i * 20));
				}
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x000DEBD4 File Offset: 0x000DDBD4
		public void updateFirstP64(byte[] wgpktData, int startIndex, uint ControllerSN)
		{
			byte[] array = new byte[16];
			Array.Copy(wgpktData, 16, array, 0, 4);
			Array.Copy(wgpktData, 44, array, 4, 4);
			Array.Copy(wgpktData, 56, array, 8, 8);
			uint num = BitConverter.ToUInt32(wgpktData, 8);
			if (num > 0U)
			{
				new MjRec(array, 0U, ControllerSN, num);
				if (this.newSwipes[0] == null)
				{
					this.newSwipes[0] = new MjRec(array, 0U, ControllerSN, num);
					return;
				}
				this.newSwipes[0].Update(array, 0U, ControllerSN, num);
			}
		}

		// Token: 0x0400186F RID: 6255
		protected internal new MjRec[] newSwipes = new MjRec[10];
	}
}
