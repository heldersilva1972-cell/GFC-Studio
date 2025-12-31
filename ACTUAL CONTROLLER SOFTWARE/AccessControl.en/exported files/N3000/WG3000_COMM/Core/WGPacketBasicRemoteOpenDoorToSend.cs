using System;

namespace WG3000_COMM.Core
{
	// Token: 0x02000200 RID: 512
	internal class WGPacketBasicRemoteOpenDoorToSend : WGPacket
	{
		// Token: 0x06000E5A RID: 3674 RVA: 0x00107CD0 File Offset: 0x00106CD0
		public WGPacketBasicRemoteOpenDoorToSend(int DoorNO)
		{
			this.m_DoorNO = 1;
			this.m_OperatorCardNO = -1L;
			if (DoorNO < 1 || DoorNO > 4)
			{
				throw new InvalidOperationException();
			}
			this.m_DoorNO = DoorNO;
			if (!string.IsNullOrEmpty(wgTools.gCustomProductType))
			{
				try
				{
					this.m_FloorNO = int.Parse(WGPacket.Dpt(wgTools.gCustomProductType)) >> 8;
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00107D40 File Offset: 0x00106D40
		public WGPacketBasicRemoteOpenDoorToSend(int DoorNO, int FloorNO)
		{
			this.m_DoorNO = 1;
			this.m_OperatorCardNO = -1L;
			if (DoorNO < 1 || DoorNO > 4)
			{
				throw new InvalidOperationException();
			}
			this.m_DoorNO = DoorNO;
			this.m_FloorNO = FloorNO;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00107D73 File Offset: 0x00106D73
		public WGPacketBasicRemoteOpenDoorToSend(int DoorNO, int FloorNO, int FloorDelay)
		{
			this.m_DoorNO = 1;
			this.m_OperatorCardNO = -1L;
			if (DoorNO < 1 || DoorNO > 4)
			{
				throw new InvalidOperationException();
			}
			this.m_DoorNO = DoorNO;
			this.m_FloorNO = FloorNO;
			this.m_FloorDelaySec = FloorDelay;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00107DAD File Offset: 0x00106DAD
		internal void specialDoorNo4ping(int NO)
		{
			this.m_DoorNO = NO;
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00107DB8 File Offset: 0x00106DB8
		public new byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[36];
			array[0] = base.type;
			array[1] = base.code;
			Array.Copy(BitConverter.GetBytes(srcPort), 0, array, 2, 2);
			Array.Copy(BitConverter.GetBytes(this._xid), 0, array, 4, 4);
			Array.Copy(BitConverter.GetBytes(base.iDevSnFrom), 0, array, 8, 4);
			Array.Copy(BitConverter.GetBytes(base.iDevSnTo), 0, array, 12, 4);
			array[16] = base.iCallReturn;
			array[17] = this.driverVer;
			array[18] = (byte)wgTools.gPTC_internal;
			array[19] = this.reserved19;
			Array.Copy(BitConverter.GetBytes(this.m_OperatorID), 0, array, 20, 4);
			Array.Copy(BitConverter.GetBytes(this.m_OperatorCardNO), 0, array, 24, 8);
			Array.Copy(BitConverter.GetBytes(this.m_DoorNO - 1), 0, array, 32, 4);
			array[33] = (byte)this.m_FloorNO;
			Array.Copy(BitConverter.GetBytes(this.m_FloorDelaySec), 0, array, 34, 2);
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp(36U, array)), 0, array, 2, 2);
			base.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00107ED6 File Offset: 0x00106ED6
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x00107EEC File Offset: 0x00106EEC
		public int Exit
		{
			get
			{
				if (this.m_FloorNO == 0 && this.m_FloorDelaySec == 1)
				{
					return 1;
				}
				return 0;
			}
			set
			{
				if (value > 0)
				{
					this.m_FloorNO = 0;
					this.m_FloorDelaySec = 1;
					return;
				}
				this.m_FloorNO = 0;
				this.m_FloorDelaySec = 0;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00107F0F File Offset: 0x00106F0F
		// (set) Token: 0x06000E62 RID: 3682 RVA: 0x00107F17 File Offset: 0x00106F17
		public int openDelay
		{
			get
			{
				return this.m_FloorDelaySec;
			}
			set
			{
				this.m_FloorDelaySec = value;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x00107F20 File Offset: 0x00106F20
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x00107F28 File Offset: 0x00106F28
		public long OperatorCardNO
		{
			get
			{
				return this.m_OperatorCardNO;
			}
			set
			{
				this.m_OperatorCardNO = value;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x00107F31 File Offset: 0x00106F31
		// (set) Token: 0x06000E66 RID: 3686 RVA: 0x00107F39 File Offset: 0x00106F39
		public uint OperatorID
		{
			get
			{
				return this.m_OperatorID;
			}
			set
			{
				this.m_OperatorID = value;
			}
		}

		// Token: 0x04001BA3 RID: 7075
		private const int m_len = 36;

		// Token: 0x04001BA4 RID: 7076
		private int m_DoorNO;

		// Token: 0x04001BA5 RID: 7077
		private int m_FloorDelaySec;

		// Token: 0x04001BA6 RID: 7078
		private int m_FloorNO;

		// Token: 0x04001BA7 RID: 7079
		private long m_OperatorCardNO;

		// Token: 0x04001BA8 RID: 7080
		private uint m_OperatorID;
	}
}
